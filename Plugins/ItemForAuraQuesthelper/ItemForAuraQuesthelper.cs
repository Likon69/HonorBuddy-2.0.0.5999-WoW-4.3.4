using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Styx;
using Styx.Logic.Combat;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins.PluginClass;
using Styx.Logic.BehaviorTree;

using Styx.Logic.Pathing;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Questing;
using Styx.Plugins;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Common;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic;
using Styx.Logic.Profiles;
using Styx.Logic.Inventory.Frames.LootFrame;

namespace ItemForAuraQuesthelper
{
    class ItemForAura : HBPlugin
    {
        public override string Name { get { return "Questhelper - ItemForAura"; } }
        public override string Author { get { return "Kickazz n KaZ"; } }
        private readonly Version _version = new Version(2, 0);
        public override Version Version { get { return _version; } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return true; } }

        public static Settings Settings = new Settings();
        public static LocalPlayer Me = ObjectManager.Me;

        bool hasItBeenInitialized = false;
        static Stopwatch pulseThrottleTimer = new Stopwatch();

        string AuraToID1;
        string AuraToID2;
        string AuraToID3;
        string AuraToID4;
        string AuraToID5;
        string AuraToID6;

        public ItemForAura()
        {
            Logging.Write("ItemForAura - Questhelper - Version 2.0 Loaded.");
            Settings.Load();
        }

        public override void OnButtonPress()
        {
            Settings.Load();
            ConfigForm.ShowDialog();
        }


        private Form MyForm;
        public Form ConfigForm
        {
            get
            {
                if (MyForm == null)
                    MyForm = new Config();
                return MyForm;
            }
        }

        public override void Pulse()
        {
            if (!hasItBeenInitialized)
            {
                if (Settings.Aura1 != "0")
                    AuraToID1 = WoWSpell.FromId(Convert.ToInt32(Settings.Aura1)).Name;
                if (Settings.Aura2 != "0")
                    AuraToID2 = WoWSpell.FromId(Convert.ToInt32(Settings.Aura2)).Name;
                if (Settings.Aura3 != "0")
                    AuraToID3 = WoWSpell.FromId(Convert.ToInt32(Settings.Aura3)).Name;
                if (Settings.Aura4 != "0")
                    AuraToID4 = WoWSpell.FromId(Convert.ToInt32(Settings.Aura4)).Name;
                if (Settings.Aura5 != "0")
                    AuraToID5 = WoWSpell.FromId(Convert.ToInt32(Settings.Aura5)).Name;
                if (Settings.Aura6 != "0")
                    AuraToID6 = WoWSpell.FromId(Convert.ToInt32(Settings.Aura6)).Name;
            }

            if (!pulseThrottleTimer.IsRunning || pulseThrottleTimer.ElapsedMilliseconds >= 1000)
            {
                pulseThrottleTimer.Reset();
                pulseThrottleTimer.Start();
// dont use the Item while ...
                if (Me.IsCasting || Me.IsInInstance || Me.IsOnTransport || Battlegrounds.IsInsideBattleground
                   || Me.Dead || Me.IsGhost)
                    return;

                ObjectManager.Update();

                if (Settings.Active1 && !Me.HasAura(AuraToID1) && (
						((Settings.Quest11 == "0") && (Settings.Quest12 == "0") && (Settings.Quest13 == "0")) ||
                        ((Settings.Quest11 != "0") && HasQuest(Convert.ToInt32(Settings.Quest11)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest11))) ||
                        ((Settings.Quest12 != "0") && HasQuest(Convert.ToInt32(Settings.Quest12)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest12))) ||
                        ((Settings.Quest13 != "0") && HasQuest(Convert.ToInt32(Settings.Quest13)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest13)))
						))
                {
                    List<WoWItem> objList = ObjectManager.GetObjectsOfType<WoWItem>()
                    .Where(o => (o.Entry == (Convert.ToInt32(Settings.Item1))))
                    .OrderBy(o => o.Distance).ToList();

                    foreach (WoWItem o in objList)
                    {
                        o.Use();
                        Thread.Sleep(500);
                    }
                    while (Me.IsCasting)
                    {
                        Thread.Sleep(100);
                    }
                }
                if (Settings.Active2 && !Me.HasAura(AuraToID2) && (
                        ((Settings.Quest21 == "0") && (Settings.Quest22 == "0") && (Settings.Quest23 == "0")) ||
                        ((Settings.Quest21 != "0") && HasQuest(Convert.ToInt32(Settings.Quest21)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest21))) ||
                        ((Settings.Quest22 != "0") && HasQuest(Convert.ToInt32(Settings.Quest22)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest22))) ||
                        ((Settings.Quest23 != "0") && HasQuest(Convert.ToInt32(Settings.Quest23)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest23)))
                        ))
                {
                    List<WoWItem> objList = ObjectManager.GetObjectsOfType<WoWItem>()
                    .Where(o => (o.Entry == (Convert.ToInt32(Settings.Item2))))
                    .OrderBy(o => o.Distance).ToList();

                    foreach (WoWItem o in objList)
                    {
                        o.Use();
                        Thread.Sleep(500);
                    }
                    while (Me.IsCasting)
                    {
                        Thread.Sleep(100);
                    }
                }
                if (Settings.Active3 && !Me.HasAura(AuraToID3) && (
                        ((Settings.Quest31 == "0") && (Settings.Quest32 == "0") && (Settings.Quest33 == "0")) ||
                        ((Settings.Quest31 != "0") && HasQuest(Convert.ToInt32(Settings.Quest31)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest31))) ||
                        ((Settings.Quest32 != "0") && HasQuest(Convert.ToInt32(Settings.Quest32)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest32))) ||
                        ((Settings.Quest33 != "0") && HasQuest(Convert.ToInt32(Settings.Quest33)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest33)))
                        ))
                {
                    List<WoWItem> objList = ObjectManager.GetObjectsOfType<WoWItem>()
                    .Where(o => (o.Entry == (Convert.ToInt32(Settings.Item3))))
                    .OrderBy(o => o.Distance).ToList();

                    foreach (WoWItem o in objList)
                    {
                        o.Use();
                        Thread.Sleep(500);
                    }
                    while (Me.IsCasting)
                    {
                        Thread.Sleep(100);
                    }
                }
                if (Settings.Active4 && !Me.HasAura(AuraToID4) && (
                        ((Settings.Quest41 == "0") && (Settings.Quest42 == "0") && (Settings.Quest43 == "0")) ||
                        ((Settings.Quest41 != "0") && HasQuest(Convert.ToInt32(Settings.Quest41)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest41))) ||
                        ((Settings.Quest42 != "0") && HasQuest(Convert.ToInt32(Settings.Quest42)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest42))) ||
                        ((Settings.Quest43 != "0") && HasQuest(Convert.ToInt32(Settings.Quest43)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest43)))
                        ))
                {
                    List<WoWItem> objList = ObjectManager.GetObjectsOfType<WoWItem>()
                    .Where(o => (o.Entry == (Convert.ToInt32(Settings.Item4))))
                    .OrderBy(o => o.Distance).ToList();

                    foreach (WoWItem o in objList)
                    {
                        o.Use();
                        Thread.Sleep(500);
                    }
                    while (Me.IsCasting)
                    {
                        Thread.Sleep(100);
                    }
                }
                if (Settings.Active5 && !Me.HasAura(AuraToID5) && (
                        ((Settings.Quest51 == "0") && (Settings.Quest52 == "0") && (Settings.Quest53 == "0")) ||
                        ((Settings.Quest51 != "0") && HasQuest(Convert.ToInt32(Settings.Quest51)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest51))) ||
                        ((Settings.Quest52 != "0") && HasQuest(Convert.ToInt32(Settings.Quest52)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest52))) ||
                        ((Settings.Quest53 != "0") && HasQuest(Convert.ToInt32(Settings.Quest53)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest53)))
                        ))
                {
                    List<WoWItem> objList = ObjectManager.GetObjectsOfType<WoWItem>()
                    .Where(o => (o.Entry == (Convert.ToInt32(Settings.Item5))))
                    .OrderBy(o => o.Distance).ToList();

                    foreach (WoWItem o in objList)
                    {
                        o.Use();
                        Thread.Sleep(500);
                    }
                    while (Me.IsCasting)
                    {
                        Thread.Sleep(100);
                    }
                }
                if (Settings.Active6 && !Me.HasAura(AuraToID6) && (
                        ((Settings.Quest61 == "0") && (Settings.Quest62 == "0") && (Settings.Quest63 == "0")) ||
                        ((Settings.Quest61 != "0") && HasQuest(Convert.ToInt32(Settings.Quest61)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest61))) ||
                        ((Settings.Quest62 != "0") && HasQuest(Convert.ToInt32(Settings.Quest62)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest62))) ||
                        ((Settings.Quest63 != "0") && HasQuest(Convert.ToInt32(Settings.Quest63)) && !IsQuestCompleted(Convert.ToInt32(Settings.Quest63)))
                        ))
                {
                    List<WoWItem> objList = ObjectManager.GetObjectsOfType<WoWItem>()
                    .Where(o => (o.Entry == (Convert.ToInt32(Settings.Item6))))
                    .OrderBy(o => o.Distance).ToList();

                    foreach (WoWItem o in objList)
                    {
                        o.Use();
                        Thread.Sleep(500);
                    }
                    while (Me.IsCasting)
                    {
                        Thread.Sleep(100);
                    }
                }
         
            }
        }

        private static bool IsQuestCompleted(Int32 ID)
        {
            //to make sure every header is expanded in quest log
            Lua.DoString("ExpandQuestHeader(0)");
            //number of values in quest log (includes headers like "Durator")
            int QuestCount = Lua.GetReturnVal<int>("return select(1, GetNumQuestLogEntries())", 0);
            for (int i = 1; i <= QuestCount; i++)
            {
                List<string> QuestInfo = Lua.LuaGetReturnValue("return GetQuestLogTitle(" + i + ")", "raphus.lua");

                //pass if the index isHeader or isCollapsed
                if (QuestInfo[4] == "1" || QuestInfo[5] == "1")
                    continue;

                string QuestStatus = null;
                if (QuestInfo[6] == "1")
                    QuestStatus = "completed";
                else if (QuestInfo[6] == "-1")
                    QuestStatus = "failed";
                else
                    QuestStatus = "in progress";
                if (QuestInfo[8] == Convert.ToString(ID) && QuestStatus == "completed")
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasQuest(Int32 ID)
        {
            //to make sure every header is expanded in quest log
            Lua.DoString("ExpandQuestHeader(0)");
            //number of values in quest log (includes headers like "Durator")
            int QuestCount = Lua.GetReturnVal<int>("return select(1, GetNumQuestLogEntries())", 0);
            for (int i = 1; i <= QuestCount; i++)
            {
                List<string> QuestInfo = Lua.LuaGetReturnValue("return GetQuestLogTitle(" + i + ")", "raphus.lua");

                //pass if the index isHeader or isCollapsed
                if (QuestInfo[4] == "1" || QuestInfo[5] == "1")
                    continue;

                string QuestStatus = null;
                if (QuestInfo[8] == Convert.ToString(ID))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
