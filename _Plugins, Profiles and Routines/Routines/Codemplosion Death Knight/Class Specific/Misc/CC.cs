using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Hera
{
    public partial class Codemplosion
    {
        // ************************************************************************************
        //
        public const string CCName = "Codemplosion Death Knight";                                   // Name of the CC displayed to the user
        public const string AuthorName = "Deathburn";                                     // Part of the string used in the CC name
        private readonly Version _versionNumber = new Version(1, 0, 0);                 // Part of the string used in the CC name
        public const WoWClass CCClass = WoWClass.DeathKnight;                                 // The class this CC will support
        // ************************************************************************************

        
        #region HB Start Up
        void BotEvents_OnBotStarted(EventArgs args)
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.DeathKnight.ClassSpec = (ClassHelpers.DeathKnight.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.DeathKnight.ClassSpec, Me.Class));

            // Do important stuff on LUA events
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", EventHandlers.CombatLogEventHander);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", TalentPointEventHander);
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", TalentPointEventHander);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", TalentPointEventHander);
            //Lua.Events.AttachEvent("PARTY_MEMBER_DISABLE", PlayerDeadEventHandler);
            

            Timers.Add("Pulse");          // Only do certain things in the Pulse check every 1 second
            Timers.Add("DistanceCheck");
            Timers.Add("FaceTarget");
            Timers.Add("PullSpellCast");
            Timers.Add("EnvironmentSettings");
            Timers.Add("TimersTest");
            Timers.Add("Interact");
            Timers.Add("Spam");
            Timers.Add("BloodBoil");
            Timers.Add("DeathStrike");
            Timers.Add("Silence");
            Timers.Add("Pestilence");
            Timers.Add("Ice");
            Timers.Add("Blood");
            Timers.Add("Unholy");
            Timers.Add("PetCheck");
            

            // Environmental Settings
            string environment = Utils.IsBattleground ? "PVP" : "PVE";
            environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            ConfigSettings.CurrentEnvironment = environment;
            //Utils.Log(string.Format("*** Loading {0} settings.", environment),Utils.Colour("Red"));

            LoadSettings(false);

        }

        public static void TalentPointEventHander(object sender, LuaEventArgs args)
        {
            ClassHelpers.DeathKnight.ClassSpec = (ClassHelpers.DeathKnight.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.DeathKnight.ClassSpec, Me.Class));
        }

        public static void PlayerDeadEventHandler(object sender, LuaEventArgs args)
        {
            Utils.Log("************************* PLAY-YA DE-EDD");

            foreach (object arg in args.Args)
            {
                if (!(arg is String)) continue;
                var s = (string)arg;
                Utils.Log("*********************[PLAYED DEAD EVENT FIRED]**************************");
                Utils.Log(s);
                Utils.Log("*********************[PLAYED DEAD EVENT ENDED]**************************");

            }
        }

        // This event is fired each time you hit the Stop button in HB
        // Currently its only asigning FALSE to a variable, but you go do anything you want in here
        void BotEvents_OnBotStopped(EventArgs args)
        {
            //
        }
        #endregion

        #region Pulse
        public override void Pulse()
        {
            // HB runs this as frequenty as possible. I don't know the exact frequency but its supposed to be 5-10 times per second
            // Anything you want checked on a regular basis you may want to add here. 
            // For example buffing / healing random players

            base.Pulse();

            int lootableMobs = LootTargeting.Instance.LootingList.Count;

            // If Settings.DirtyData = true it will reload the settings from the XML file
            // This reads the XML file and re-populates the Settings class with any changed values
            if (!_isCCLoaded) { _isCCLoaded = true; Settings.DirtyData = true; }
            if (Settings.DirtyData) LoadSettings(true);

            // So we don't overload HB the below code is only run once per second
            if (!Timers.Expired("Pulse", 1000)) return;
            Timers.Recycle("Pulse", 1000);

            // Environmental Settings
            if (Timers.Expired("EnvironmentSettings", 5000))
            {
                if (Settings.MultipleEnvironment.Contains("never"))
                {
                    ConfigSettings.CurrentEnvironment = "PVE";
                }
                else
                {
                    Timers.Reset("EnvironmentSettings");
                    string environment = Utils.IsBattleground ? "PVP" : "PVE";
                    environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
                    if (!ConfigSettings.UIActive && environment != ConfigSettings.CurrentEnvironment)
                    {
                        ConfigSettings.CurrentEnvironment = environment;
                        Utils.Log(string.Format("*** Environment has changed. Loading {0} settings.", environment),Utils.Colour("Red"));
                        LoadSettings(false);
                    }
                }
            }

            // Make sure we have a target - Instance only
            // Sometimes IB was not selecting a target when we were in combat. This fucked up things immensely!
            if (Me.IsInInstance && !Me.GotTarget)
            {
                WoWUnit tank = RAF.PartyTankRole;  
                if (tank != null && tank.GotTarget && tank.Combat) RAF.PartyTankRole.CurrentTarget.Target();
            }

            
            
            // Buffs and such
            if (!Me.IsFlying && !Me.Mounted)
            {

                if (!Me.Combat)
                {
                    
                }

                if (Me.Combat)
                {
                    if (!Self.IsBuffOnMe("Horn of Winter") && Spell.CanCast("Horn of Winter")) Spell.Cast("Horn of Winter");
                }
            }





        }


        #endregion
    }
}
