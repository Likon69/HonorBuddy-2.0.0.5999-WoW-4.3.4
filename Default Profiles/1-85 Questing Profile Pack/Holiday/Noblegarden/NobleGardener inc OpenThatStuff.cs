using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx;
using Styx.Helpers;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using Styx.Logic.Pathing;
using Styx.Logic.BehaviorTree;
using System.Threading;
using Styx.Logic.Inventory.Frames.LootFrame;

namespace NobleGardenerAchievement
{
    public class NobleGardenerAchievement : HBPlugin
    {
        #region Nothing to see here!
        public override string Name { get { return "[Noble Gardener]"; } }
        public override string Author { get { return "BarryDurex"; } }
        public override Version Version { get { return new Version(1, 0, 4); } }
        public override string ButtonText { get { return "reload"; } }
        public override bool WantButton { get { return true; } }

        private static uint[,] _data = new uint[,] { 
			{1, 35792}, // Mage Hunter Personal Effects for Quest(12000)
			{1, 20767}, // Scum Covered Bag
			{1, 61387}, // Hidden Stash
			{1, 62829}, // Magnetized Scrap Collector
			{1, 32724}, // Sludge Covered Object
			{1, 45072}, // Noblegarden Egg
        };


        private void slog(string format, params object[] args)
        { Logging.Write(System.Drawing.Color.SeaGreen, Name + ": " + format, args); }

        private static List<string> Chocolates = new List<string>();
        private static List<string> killAchievement = new List<string>();
        private static List<ulong> blacklistCritters = new List<ulong>();
        private static Stopwatch stuckTimer = new Stopwatch();
        private static Stopwatch retreatTimer = new Stopwatch();
        private long retreatTime;
        LocalPlayer Me = ObjectManager.Me;
        private bool complete = false;
        private bool isInitialize;
        private bool moveBack;
        private WoWPoint backPoint = WoWPoint.Empty;
        private bool searchEgg;
        #endregion

        public override void Initialize()
        {
            isInitialize = false;
            searchEgg = false;
        }
        public override void OnButtonPress()
        {
            updateList(true);
            //achievementFound = false;
        }

        public static double XYDist(WoWGameObject wgo)
        {
            return Math.Sqrt(Math.Pow(wgo.Location.X - StyxWoW.Me.Location.X, 2) + Math.Pow(wgo.Location.Y - StyxWoW.Me.Location.Y, 2));
        }
                    //bool achievementFound = false;

        public override void Pulse()
        {
            if (!isInitialize) { updateList(true); isInitialize = true; }

            while (!complete && !Me.Combat)
            {
                ObjectManager.Update();
                List<WoWGameObject> _ObjList = ObjectManager.GetObjectsOfType<WoWGameObject>();
                double xyDist = double.MaxValue;
                WoWGameObject bestNode = null;
                //int i = 0;
                //foreach (WoWGameObject obj in _ObjList)
                //for (int i = _ObjList.Count - 1; i >= 0; i--)
                //{
                //    slog("search for best node");
                //    TreeRoot.StatusText = Name + ": search for best node";
                //    if (_ObjList[i].Entry != (uint)113768)
                //    {
                //        _ObjList.Remove(_ObjList[i]);
                //        continue;
                //    }
                //}
                int i = 0;
                while (i <= _ObjList.Count)
                {                    
                    foreach (WoWGameObject egg in _ObjList)
                    {
                        ++i;
                        slog("search for best node");
                        TreeRoot.StatusText = Name + ": search for best node";
                        if (egg.Entry == 113768 && egg.CanUse() ||
							(egg.Entry == 113769 && egg.CanUse()) ||
							(egg.Entry == 113770 && egg.CanUse()) ||
							(egg.Entry == 113771 && egg.CanUse()))
                        {
                            double dist = egg.Distance2D;
                            if (xyDist > dist)
                            {
                                if (dist < 100)
                                {
                                    xyDist = dist;
                                    bestNode = egg;
                                }
                            }
                        }
                        continue;
                    }
                }
                //if (bestNode != null) { slog("GuID: " + bestNode.Guid + " Distance: " + (int)bestNode.Distance); }
                //slog("found " + bestNode.Name + " Distance: " + (int)bestNode.Distance + "yards");
                //slog(bestNode.ToString());
                //TreeRoot.StatusText = Name + ": found: [" + bestNode.Name + "] Distance: " + (int)bestNode.Distance + "yards";
                //achievementFound = true;

                while (searchEgg == false && bestNode != null && bestNode.Location != WoWPoint.Empty && bestNode.CanUse() && !bestNode.CanUseNow())
                {
                    slog("Move to " + bestNode.Name);
                    TreeRoot.StatusText = Name + ": Move to " + bestNode.Name + " Distance: " + (int)Me.Location.Distance(bestNode.Location) + "yards";
                    Navigator.MoveTo(bestNode.Location);
                    Thread.Sleep(100);
                }
                Navigator.MoveTo(Me.Location);
                //WoWMovement.MoveStop();
                while (bestNode != null && bestNode.IsValid /* && bestNode.Distance <= 5*/ && stuckTimer.ElapsedMilliseconds <= 2000 && bestNode.CanUseNow())
                {
                    stuckTimer.Start();
                    WoWMovement.MoveStop();
                    //if (bestNode != null) slog("Interact..");
                    TreeRoot.StatusText = Name + ": Interact..";
                    slog("looting..");
                    bestNode.Interact();
                    //Thread.Sleep(3000);
                    while (LootFrame.Instance.IsVisible)
                    {
                        TreeRoot.StatusText = Name + ": looting..";
                        //TreeRoot.StatusText = "";
                        bestNode = null;
                        Thread.Sleep(1500);
			CheckInventoryItems();

			
                    }
                }
                stuckTimer.Stop(); stuckTimer.Reset();
            }
        }

private void CheckInventoryItems()
        {
           foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>())
            {
            for (int i = 0; i <= _data.GetUpperBound(0); i++)
            {
                if (_data[i, 1] == item.Entry)
                {
                    int cnt = Convert.ToInt32(Lua.GetReturnValues(String.Format("return GetItemCount(\"{0}\")", item.Name), Name + ".lua")[0]);
                    int max = (int)(cnt / _data[i, 0]);
                    for (int j = 0; j < max; j++)
                    {
                        String s = String.Format("UseItemByName(\"{0}\")", item.Name);
                        slog("Using {0} we have {1}", item.Name, cnt);
                        Lua.DoString(s);
                        StyxWoW.SleepForLagDuration();
                    }
                    break;
                }
            }
        }
        }


        #region UpdateList
        public void updateList(bool slog_)
        {
            //slog("Update search criteria..");
            //byte _complete = 0;
            //Chocolates.Clear();

            //// Achievement Chocolate Lover ID 2417
            //if (!Completed(2417))
            //{
            //    int AchievementMaxCriteria = Lua.GetReturnVal<int>("local value = GetAchievementNumCriteria(2417); return value", 0);


            //    string quantityString = RunLuaString(2417, AchievementMaxCriteria, "quantityString");
            //    string quantity = RunLuaString(2417, AchievementMaxCriteria, "quantity");
            //    string flags = RunLuaString(2417, AchievementMaxCriteria, "flags");

            //    //if (slog_) { slog("[{0}] ({1}/{2})  uncomplete! Search egs..", AchievementName(2417), done, AchievementMaxCriteria); }
            //    slog("quantityString: {0}  -  quantity: {1}  -  flags: {2}", quantityString, quantity, flags);
            //}
            //else { if (slog_) { slog("[" + AchievementName(2417) + "] is complete!"); } ++_complete; }

            //// Achievement Chocoholic ID 2418
            //if (!Completed(2418))
            //{
            //    int AchievementMaxCriteria = Lua.GetReturnVal<int>("local value = GetAchievementNumCriteria(2418); return value", 0);


            //    string quantityString = RunLuaString(2418, AchievementMaxCriteria, "quantityString");
            //    string quantity = RunLuaString(2418, AchievementMaxCriteria, "quantity");   
            //    string flags = RunLuaString(2418, AchievementMaxCriteria, "flags");
                
            //    //if (slog_) { slog("[{0}] ({1}/{2})  uncomplete! Search egs..", AchievementName(2418), done, AchievementMaxCriteria); }
            //    slog("quantityString: {0}  -  quantity: {1}  -  flags: {2}", quantityString, quantity, flags);
            //}
            //else { if (slog_) { slog("[" + AchievementName(2418) + "] is complete!"); } ++_complete; }
        }
        #endregion

        #region Lua code
        public string RunLuaString(int achievementID, int criteria, string value)
        {
            //int AchievementMaxCriteria = Lua.GetReturnVal<int>("local value = GetAchievementNumCriteria(1206); return value", 0);
            string Value = Lua.GetReturnVal<string>("local criteriaString, criteriaType, completed, quantity, reqQuantity, charName, flags, assetID, quantityString, criteriaID = GetAchievementCriteriaInfo(" + achievementID + ", " + criteria + "); return " + value + "", 0);
            return Value;
        }

        public bool Completed(int achievementID)
        {
            string Statistic = Lua.GetReturnVal<string>("local value = GetStatistic(" + achievementID + "); return value", 0);
            if (Statistic == "0")
                return true;

            //bool completed2 = Lua.GetReturnVal<bool>("local IDNumber, Name, Points, Completed, Month, Day, Year, Description, Flags, Image, RewardText, isGuildAch = GetAchievementInfo(" + achievementID + "); return Completed", 0);
            //int max = Lua.GetReturnVal<int>("local value = GetAchievementNumCriteria(" + achievementID + "); return value", 0);
            //bool completed = Lua.GetReturnVal<bool>("local criteriaString, criteriaType, completed, quantity, reqQuantity, charName, flags, assetID, quantityString, criteriaID = GetAchievementCriteriaInfo(" + achievementID + ", " + max + "); return completed", 0);

            return false;
        }

        public string AchievementName(int achievementID)
        {
            string name = Lua.GetReturnVal<string>("local IDNumber, Name, Points, Completed, Month, Day, Year, Description, Flags, Image, RewardText, isGuildAch = GetAchievementInfo(" + achievementID + "); return Name", 0);
            return name;
        }

        //private bool KillCriteria(string Name)
        //{
        //    int AchievementMaxCriteria = Lua.GetReturnVal<int>("local value = GetAchievementNumCriteria(2556); return value", 0);

        //    for (int i = 0; i <= AchievementMaxCriteria; ++i)
        //    {
        //        string name = RunLuaString(2556, i, "criteriaString");

        //        if (name == Name)
        //            return true;
        //    }
        //    return false;
        //}

        private void RunMakro(string Makrotext)
        {
            Lua.DoString("RunMacroText(\"" + Makrotext + "\")");
        }

        private bool IsOutdoors()
        {
            if (Lua.GetReturnVal<int>("local value = IsOutdoors(); return value", 0) == 1)
                return true;

            return false;
        }
        #endregion

    }
}