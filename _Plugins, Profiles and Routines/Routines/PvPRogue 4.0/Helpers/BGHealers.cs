using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Helpers;



namespace PvPRogue.Helpers
{

    /// <summary>
    /// Created by SwInY - 3/3/2012
    /// </summary>
    class BGHealers
    {
        /// <summary>
        /// Our Instance of this class
        /// </summary>
        public static BGHealers _Instance;

        /// <summary>
        /// Our [ In Object Manager ] Healers list using LUA
        /// LUA - GetBattlefieldScore(i)
        /// </summary>
        public List<WoWPlayer> lHealers = new List<WoWPlayer>();

        private string[] HealingSpecs = { "Restoration", "Holy", "Discipline"};

        /// <summary>
        /// This will create the list
        /// </summary>
        public BGHealers()
        {
            // Frame lock it as we are gunna be calling a loop of lua
            //using (new FrameLock())
            //{
                int BGPlayerCount = Lua.GetReturnVal<int>("return GetNumBattlefieldScores()", 0);

                // Loop threw all the players in the BG
                for (int i = 1; i < BGPlayerCount; i++)
                {
                    List<string> GetLua = Lua.GetReturnValues(string.Format("return GetBattlefieldScore({0})", i));
                    string[] NameSplit = GetLua[0].ToString().Split('-');
                    string Name = NameSplit[0].Trim();
                    string Spec = GetLua[15].ToString();

                    if (!HealingSpecs.Contains(Spec)) continue;

                    // Convert a "name" into a WoWPlayer 
                    WoWPlayer PlayerHealer = (from CurPlayer in ObjectManager.GetObjectsOfType<WoWPlayer>()
                        where CurPlayer.Name == Name
                        select CurPlayer).FirstOrDefault();

                    // If we actually have a player that we can see in our object manager
                    // Add him to our list!
                    if (PlayerHealer != null)
                    {
                        lHealers.Add(PlayerHealer);
                    }
                }
            //}
        }


        public bool IsHealer(WoWUnit Player)
        {
            int Count = (from FindBest in Helpers.BGHealers._Instance.lHealers
                    where FindBest.Guid ==  Player.Guid
                    select FindBest).Count();

            if (Count == 0) return false;
            return true;
        }

        /// <summary>
        /// Returns the best ENEMY healer to target from a Distance
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public WoWPlayer BestHealer(int Distance)
        {
            return (from FindBest in Helpers.BGHealers._Instance.lHealers
                    where FindBest.Distance < Distance &&
                    !FindBest.IsFriendly &&
                    FindBest.InLineOfSpellSight  &&
                    !IsBeingTargeted(FindBest)
                    select FindBest).FirstOrDefault();
            
        }

        private bool IsBeingTargeted(WoWPlayer Player)
        {
            foreach (WoWPlayer LoopUnit in ObjectManager.GetObjectsOfType<WoWPlayer>())
            {
                if (Player.IsAlliance == LoopUnit.IsAlliance) continue;

                if ((LoopUnit.CurrentTarget == Player) 
                    && (Utils.Misc.GetDistance2D(LoopUnit.X, LoopUnit.Y, Player.X, Player.Y) < 35)) return true;
            }

            return false;
        }

    }
}
