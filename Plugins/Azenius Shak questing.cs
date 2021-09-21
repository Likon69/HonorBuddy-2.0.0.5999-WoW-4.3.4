using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Styx;
using Styx.Combat;
using Styx.Combat.CombatRoutine;
using Styx.Database;
using Styx.Helpers;
using Styx.Loaders;
using Styx.Logic;
using Styx.Logic.AreaManagement;
using Styx.Logic.AreaManagement.Triangulation;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Common;
using Styx.Logic.Inventory;
using Styx.Logic.Inventory.Frames;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Inventory.Frames.LootFrame;
using Styx.Logic.Inventory.Frames.MailBox;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Inventory.Frames.Taxi;
using Styx.Logic.Inventory.Frames.Trainer;
using Styx.Logic.Pathing;
using Styx.Logic.Pathing.OnDemandDownloading;
using Styx.Logic.POI;
using Styx.Logic.Profiles;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.Patchables;
using Styx.Plugins;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.Misc.DBC;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;
using System.Diagnostics;
using System.Threading;

namespace Shak_questing
{
    public class Shak_questing : HBPlugin
    {
        #region Required stuff
        public override string Author
        {
            get { return "Shakaza"; }
        }
        public override string Name
        {
            get { return "Azenius"; }
        }
        public override Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }
        public override bool WantButton
        {
            get
            {
                return false;
            }
        }
        public override string ButtonText
        {
            get
            {
                return "Settings";
            }
        }
        #endregion
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        bool IsAttached27964 = false;
        private static Stopwatch FuckingWait = new Stopwatch();
        static public bool Obj1Done10162 { get { return Lua.GetReturnVal<int>("a,b,c=GetQuestLogLeaderBoard(1,GetQuestLogIndexByID(10162));if c==1 then return 1 else return 0 end", 0) == 1; } }
        static public bool Obj2Done10162 { get { return Lua.GetReturnVal<int>("a,b,c=GetQuestLogLeaderBoard(2,GetQuestLogIndexByID(10162));if c==1 then return 1 else return 0 end", 0) == 1; } }
        static public bool Obj3Done10162 { get { return Lua.GetReturnVal<int>("a,b,c=GetQuestLogLeaderBoard(3,GetQuestLogIndexByID(10162));if c==1 then return 1 else return 0 end", 0) == 1; } }
        static bool Obj4Done28226 { get { return Lua.GetReturnVal<int>("a,b,c=GetQuestLogLeaderBoard(4,GetQuestLogIndexByID(28226));if c==1 then return 1 else return 0 end", 0) == 1; } }
        #region List
        public List<WoWUnit> EyeOfGrillok
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 19440 && !u.Dead).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> Yenniku
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 2530 && u.CurrentTargetGuid == Me.Guid && !u.Dead).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> SpiderThingy
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 44284 && u.CurrentTargetGuid == Me.Guid && !u.Dead).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> AdrineTowhide
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .Where(u => u.Entry == 44456)
                                    .OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> RessGoblin
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 36608)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWGameObject> q24817controller
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(ret => (ret.Entry == 202108 && !Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> q24817_hammer
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 36682 && !Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> q24817_vehicle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 38318 && !Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> q24958_Giant_Turtle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 38855 && !Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> StickboneBerserker
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 44329 && u.Distance < 5).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWGameObject> ScourgeBoneAnimus
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(u => u.Entry == 204966 && !Me.Dead && u.Distance < 10).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> StonevaultRuffian
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46711 && !u.Dead && u.Distance < 200).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> StonevaultGoon
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46712 && !u.Dead && u.Distance < 200).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> WardensPawn
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 46344 && !Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> Kalaran
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46859 && !u.Dead && u.Distance < 10).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> Moldarr
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46938 && !u.Dead && u.Distance < 8).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> Jirakka
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46860 && !u.Dead && u.Distance < 8).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> Nyxondra
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46861 && !u.Dead && u.Distance < 25).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> BloodSailCorsair
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 43726 && !u.Dead && u.Distance < 25).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWGameObject> GatewayShaadraz
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(u => u.Entry == 183351 && !Me.Dead).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWGameObject> GatewayMurketh
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(u => u.Entry == 183350 && !Me.Dead).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> MoargOverseer
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 19397 && !Me.Dead).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> GanArgPeon
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 19398 && !Me.Dead).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWGameObject> FelCannon
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(u => u.Entry == 19399 && !Me.Dead).OrderBy(ret => ret.Distance).ToList();
            }
        }
        #endregion
        public override void Pulse()
        {
            #region Goblin Death
            if (Me.Race == WoWRace.Goblin && Me.HasAura("Near Death!") && Me.ZoneId == 4720 && RessGoblin.Count > 0)
            {
                RessGoblin[0].Interact();
                Thread.Sleep(1000);
                Lua.DoString("RunMacroText('/click QuestFrameCompleteQuestButton')");
            }
            #endregion
            #region Quest 6544
            if (Me.QuestLog.GetQuestById(6544) != null && !Me.QuestLog.GetQuestById(6544).IsCompleted && Me.QuestLog.GetQuestById(6544).IsFailed)
            {
                Me.QuestLog.AbandonQuestById(6544);
            }
            #endregion
            #region Quest 6641
            if (Me.QuestLog.GetQuestById(6641) != null && !Me.QuestLog.GetQuestById(6641).IsCompleted)
            {
                if (Me.CurrentTarget == null)
                    return;
                if (Me.CurrentTarget != null && Me.CurrentTarget.CurrentTargetGuid != Me.Guid)
                {
                    RoutineManager.Current.Pull();
                }
            }
            #endregion
            #region Quest 13980
            if (Me.QuestLog.GetQuestById(13980) != null && !Me.QuestLog.GetQuestById(13980).IsCompleted && (Me.MinimapZoneText == "The Skunkworks" || Me.MinimapZoneText == "Talondeep Vale") && !Me.HasAura("Jinx's Elf Detection Ray"))
            {
                Lua.DoString("UseItemByName(46776)");
                Thread.Sleep(500);
            }
            #endregion
            #region Quest 14236
            if (!Me.Dead && Me.QuestLog.GetQuestById(14236) != null && !Me.QuestLog.GetQuestById(14236).IsCompleted)
            {
                WoWPoint q142361 = new WoWPoint(638.7761, 2780.211, 88.81393);
                WoWPoint q142362 = new WoWPoint(634.825, 2824.758, 87.50606);
                WoWPoint q142363 = new WoWPoint(684.2277, 2821.671, 86.48402);
                WoWPoint q142364 = new WoWPoint(646.324, 2859.586, 87.25509);
                WoWPoint q142365 = new WoWPoint(700.0909, 2848.549, 84.93351);
                WoWPoint q142366 = new WoWPoint(610.5126, 2908.886, 91.3634);
                WoWPoint q142367 = new WoWPoint(574.0838, 2886.616, 90.26514);
                WoWPoint q142368 = new WoWPoint(582.1985, 2797.607, 88.356);
                WoWPoint q142369 = new WoWPoint(602.7809, 2784.686, 88.45428);
                while (!Me.HasAura("Weed Whacker"))
                {
                    Lua.DoString("UseItemByName(49108)");
                    Thread.Sleep(1000);
                }
                while (!Me.QuestLog.GetQuestById(14236).IsCompleted)
                {
                    WoWMovement.ClickToMove(q142361);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142362);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142363);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142364);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142365);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142366);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142367);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142368);
                    Thread.Sleep(20000);
                    WoWMovement.ClickToMove(q142369);
                    Thread.Sleep(20000);
                }
            }
            #endregion
            #region Quest 24958
            if (Me.QuestLog.GetQuestById(24958) != null && !Me.QuestLog.GetQuestById(24958).IsCompleted && !Me.Dead)
            {
                WoWPoint wowpoint = new WoWPoint(1305.009, 1183.095, 121.1527);
                while (Me.Location.Distance(wowpoint) > 5)
                {
                    Navigator.MoveTo(wowpoint);
                    Thread.Sleep(500);
                }
                if (q24958_Giant_Turtle.Count != 0)
                {
                    q24958_Giant_Turtle[0].Target();
                    q24958_Giant_Turtle[0].Face();
                }
                while (Me.QuestLog.GetQuestById(24958) != null && !Me.QuestLog.GetQuestById(24958).IsCompleted && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive)
                {
                    WoWMovement.MoveStop();
                    Thread.Sleep(100);
                    Lua.DoString("UseItemByName(52043)");
                    Thread.Sleep(100);
                }

            }
            #endregion
            #region Quest 13961
            if (Me.QuestLog.GetQuestById(13961) != null && !Me.QuestLog.GetQuestById(13961).IsCompleted)
            {
                if (Me.CurrentTargetGuid != 0 && Me.CurrentTarget.Name == "Razormane Pillager" && !Me.HasAura("Dragging a Razormane"))
                {
                    while (!Me.CurrentTarget.IsFriendly)
                    {
                        Lua.DoString("UseItemByName(46722)");
                        Thread.Sleep(500);
                    }
                    if (Me.CurrentTarget.IsFriendly)
                    {
                        while (Me.CurrentTarget.Distance > 5)
                        {
                            Navigator.MoveTo(Me.CurrentTarget.Location);
                            Thread.Sleep(100);
                        }
                        Me.CurrentTarget.Interact();
                        Thread.Sleep(500);
                        Lua.DoString("SelectGossipOption(1)");
                        Thread.Sleep(500);
                    }
                }
            }
            #endregion
            #region Quest 25165
            if (Me.QuestLog.GetQuestById(25165) != null && !Me.QuestLog.GetQuestById(25165).IsCompleted && !Me.HasAura("Poison Extraction Totem"))
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(52505)", 0);
                if (!IsOnCD)
                {
                    Lua.DoString("UseItemByName(52505)");
                }
            }
            #endregion
            #region Quest 26321
            if (Me.QuestLog.GetQuestById(26321) != null && !Me.QuestLog.GetQuestById(26321).IsCompleted)
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(58165)", 0);
                if (!IsOnCD && !Me.HasAura("A Lashtail Hatchling: Hatchling Guardian Aura"))
                {
                    Lua.DoString("UseItemByName(58165)");
                }
            }
            #endregion
            #region Quest 26325
            if (Me.QuestLog.GetQuestById(26325) != null && !Me.QuestLog.GetQuestById(26325).IsCompleted)
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(58165)", 0);
                if (!IsOnCD && !Me.HasAura("A Lashtail Hatchling: Hatchling Guardian Aura"))
                {
                    Lua.DoString("UseItemByName(58165)");
                }
            }
            #endregion
            #region Quest 26953
            if (Me.QuestLog.GetQuestById(26953) != null && !Me.QuestLog.GetQuestById(26953).IsCompleted && !Me.HasAura("Zen'Kiki Guardian Aura"))
            {
                if (AdrineTowhide.Count > 0 && AdrineTowhide[0].Distance < 5)
                {
                    AdrineTowhide[0].Interact();
                    Thread.Sleep(1000);
                    Lua.DoString("SelectGossipOption(1)");
                }
                else if (AdrineTowhide.Count > 0 && AdrineTowhide[0].Distance > 5)
                {
                    while (AdrineTowhide[0].Distance > 5)
                    {
                        Navigator.MoveTo(AdrineTowhide[0].Location);
                        Thread.Sleep(100);
                    }
                }
                else if (AdrineTowhide.Count < 1)
                {
                    WoWPoint TowHide = new WoWPoint(1796.26, -1684.78, 60.1698);
                    while (Me.Location.Distance(TowHide) > 5)
                    {
                        Navigator.MoveTo(TowHide);
                        Thread.Sleep(100);
                    }
                }
            }
            #endregion
            #region Quest 26925
            if (Me.QuestLog.GetQuestById(26925) != null && !Me.QuestLog.GetQuestById(26925).IsCompleted && (StickboneBerserker.Count >= 1))
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(60678)", 0);
                if (!IsOnCD)
                {
                    Lua.DoString("UseItemByName(60678)");
                    LegacySpellManager.ClickRemoteLocation(StickboneBerserker[0].Location);
                }
            }
            #endregion
            #region Quest 26648
            if (Me.QuestLog.GetQuestById(26648) != null && !Me.QuestLog.GetQuestById(26648).IsCompleted)
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(59226)", 0);
                if (!IsOnCD && !Me.HasAura("Dead Eye's Intuition"))
                {
                    Lua.DoString("UseItemByName(59226)");
                }
            }
            #endregion
            #region Quest 14238
            if (Me.QuestLog.GetQuestById(14238) != null && !Me.QuestLog.GetQuestById(14238).IsCompleted && !Me.HasAura("Infrared Heat Focals"))
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(49611)", 0);
                if (!IsOnCD)
                {
                    Lua.DoString("UseItemByName(49611)");
                }
            }
            #endregion
            #region Quest 27789
            if (Me.QuestLog.GetQuestById(27789) != null && !Me.QuestLog.GetQuestById(27789).IsCompleted)
            {
                while (Me.QuestLog.GetQuestById(27789) != null && !Me.QuestLog.GetQuestById(27789).IsCompleted)
                {
                    StonevaultRuffian[0].Target();
                    WoWMovement.ConstantFace(Me.CurrentTarget.Guid);
                    Lua.DoString("CastPetAction(1)");
                    StonevaultGoon[0].Target();
                    WoWMovement.ConstantFace(Me.CurrentTarget.Guid);
                    Lua.DoString("CastPetAction(1)");
                    Lua.DoString("CastPetAction(2)");
                }
            }
            #endregion
            #region Quest 27771
            if (Me.QuestLog.GetQuestById(27771) != null && !Me.QuestLog.GetQuestById(27771).IsCompleted)
            {
                while (Me.QuestLog.GetQuestById(27771) != null && !Me.QuestLog.GetQuestById(27771).IsCompleted)
                {
                    Lua.DoString("RunMacroText('/cleartarget')");
                    FuckingWait.Start();
                    if (FuckingWait.Elapsed.Seconds > 12)
                        return;
                }
            }
            #endregion
            #region Quest 27885
            if (!Me.Dead && Me.QuestLog.GetQuestById(27885) != null && !Me.QuestLog.GetQuestById(27885).IsCompleted)
            {
                WoWPoint q278850 = new WoWPoint(-6970.479, -3439.854, 200.8959);
                WoWPoint q278851 = new WoWPoint(-6968.06, -3440.255, 200.8969);
                WoWPoint q278852 = new WoWPoint(-6964.444, -3440.112, 200.8969);
                WoWPoint q278853 = new WoWPoint(-6961.984, -3439.921, 200.8963);
                WoWPoint q278854 = new WoWPoint(-6959.851, -3445.163, 201.2538);
                WoWPoint q278855 = new WoWPoint(-6959.738, -3447.433, 201.6079);
                WoWPoint q278856 = new WoWPoint(-6964.568, -3450.362, 200.8965);
                WoWPoint q278857 = new WoWPoint(-6966.959, -3450.602, 200.8965);
                WoWPoint q278858 = new WoWPoint(-6969.584, -3445.054, 200.8965);
                WoWPoint q278859 = new WoWPoint(-6969.789, -3442.688, 200.8965);
                WoWPoint q278811 = new WoWPoint(-6964.177, -3440.75, 200.8958);
                WoWPoint q278812 = new WoWPoint(-6961.631, -3440.965, 200.8958);
                WoWPoint q278813 = new WoWPoint(-6960.158, -3445.682, 200.8958);
                WoWPoint q278814 = new WoWPoint(-6960.385, -3447.647, 200.8958);
                WoWPoint q278815 = new WoWPoint(-6964.737, -3449.662, 200.8958);
                WoWPoint q278816 = new WoWPoint(-6967, -3449.534, 200.8955);
                WoWPoint q278817 = new WoWPoint(-6968.355, -3445.081, 200.8955);
                WoWPoint q278818 = new WoWPoint(-6968.267, -3442.952, 200.8955);
                WoWPoint q278821 = new WoWPoint(-6964.177, -3440.75, 200.8958);
                WoWPoint q278822 = new WoWPoint(-6961.631, -3440.965, 200.8958);
                WoWPoint q278823 = new WoWPoint(-6960.158, -3445.682, 200.8958);
                WoWPoint q278824 = new WoWPoint(-6960.385, -3447.647, 200.8958);
                WoWPoint q278825 = new WoWPoint(-6964.737, -3449.662, 200.8958);
                WoWPoint q278826 = new WoWPoint(-6967, -3449.534, 200.8955);
                WoWPoint q278827 = new WoWPoint(-6968.355, -3445.081, 200.8955);
                WoWPoint q278828 = new WoWPoint(-6968.267, -3442.952, 200.8955);
                WoWPoint q278841 = new WoWPoint(-6964.177, -3440.75, 200.8958);
                WoWPoint q278842 = new WoWPoint(-6961.631, -3440.965, 200.8958);
                WoWPoint q278843 = new WoWPoint(-6960.158, -3445.682, 200.8958);
                WoWPoint q278844 = new WoWPoint(-6961.002, -3447.482, 200.8966);
                WoWPoint q278845 = new WoWPoint(-6964.568, -3445.147, 200.8966);
                while (!Me.QuestLog.GetQuestById(27885).IsCompleted)
                {
                    WoWMovement.ClickToMove(q278850);
                    Thread.Sleep(3000);
                    WoWMovement.ClickToMove(q278851);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278852);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278853);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278854);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278855);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278856);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278857);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278858);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278859);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278811);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278812);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278813);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278814);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278815);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278816);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278817);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278818);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278821);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278822);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278823);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278824);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278825);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278826);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278827);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278828);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278841);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278842);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278843);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278844);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                    WoWMovement.ClickToMove(q278845);
                    Thread.Sleep(1000);
                    WardensPawn[0].Target();
                    WardensPawn[0].Interact();
                    Thread.Sleep(1000);
                }
            }
            #endregion
            #region Quest 27893
            if (Me.QuestLog.GetQuestById(27893) != null && !Me.QuestLog.GetQuestById(27893).IsCompleted)
            {
                WoWPoint q278931 = new WoWPoint(-6730.564, -2448.625, 272.7784);
                WoWPoint q278932 = new WoWPoint(-6805.746, -2435.204, 272.7776);
                while (!Me.QuestLog.GetQuestById(27893).IsCompleted)
                {
                    WoWMovement.ClickToMove(q278931);
                    Thread.Sleep(10000);
                    WoWMovement.ClickToMove(q278932);
                    Thread.Sleep(13000);
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(1)");
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(4)");
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(8)");
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(2)");
                    Thread.Sleep(5000);
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(5)");
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(7)");
                    Thread.Sleep(5000);
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(1)");
                    Thread.Sleep(5000);
                    Lua.DoString("RunMacroText('/target Darkflight Flameblade')");
                    Lua.DoString("CastPetAction(1)");
                }
            }
            #endregion
            #region Quest 27894
            if (Me.QuestLog.GetQuestById(27894) != null && !Me.QuestLog.GetQuestById(27894).IsCompleted)
            {
                while (!Me.QuestLog.GetQuestById(27894).IsCompleted && (Kalaran.Count >= 1))
                {
                    Lua.DoString("CastPetAction(1)");
                    Lua.DoString("CastPetAction(4)");
                    Lua.DoString("CastPetAction(8)");
                    Lua.DoString("CastPetAction(2)");
                    Lua.DoString("CastPetAction(5)");
                    Lua.DoString("CastPetAction(7)");
                    FuckingWait.Start();
                    if (FuckingWait.Elapsed.Seconds > 12)
                        return;
                }
            }
            #endregion
            #region Quest 27895
            if (Me.QuestLog.GetQuestById(27895) != null && !Me.QuestLog.GetQuestById(27895).IsCompleted)
            {
                while (!Me.QuestLog.GetQuestById(27895).IsCompleted && (Moldarr.Count >= 1))
                {
                    Lua.DoString("CastPetAction(1)");
                    Lua.DoString("CastPetAction(4)");
                    Lua.DoString("CastPetAction(8)");
                    Lua.DoString("CastPetAction(2)");
                    Lua.DoString("CastPetAction(5)");
                    Lua.DoString("CastPetAction(7)");
                    Lua.DoString("AttackTarget()");
                    FuckingWait.Start();
                    if (FuckingWait.Elapsed.Seconds > 12)
                        return;
                }
            }
            #endregion
            #region Quest 27895
            if (Me.QuestLog.GetQuestById(27895) != null && !Me.QuestLog.GetQuestById(27895).IsCompleted)
            {
                while (!Me.QuestLog.GetQuestById(27895).IsCompleted && (Jirakka.Count >= 1))
                {
                    Lua.DoString("CastPetAction(1)");
                    Lua.DoString("CastPetAction(4)");
                    Lua.DoString("CastPetAction(8)");
                    Lua.DoString("CastPetAction(2)");
                    Lua.DoString("CastPetAction(5)");
                    Lua.DoString("CastPetAction(7)");
                    Lua.DoString("AttackTarget()");
                    FuckingWait.Start();
                    if (FuckingWait.Elapsed.Seconds > 12)
                        return;
                }
            }
            #endregion
            #region Quest 27896
            if (Me.QuestLog.GetQuestById(27896) != null && !Me.QuestLog.GetQuestById(27896).IsCompleted)
            {
                while (!Me.QuestLog.GetQuestById(27896).IsCompleted && (Nyxondra.Count >= 1))
                {
                    Lua.DoString("RunMacroText('/target Nyxondra')");
                    Lua.DoString("CastPetAction(1)");
                    Lua.DoString("CastPetAction(4)");
                    Lua.DoString("CastPetAction(8)");
                    Lua.DoString("CastPetAction(2)");
                    Lua.DoString("CastPetAction(5)");
                    Lua.DoString("CastPetAction(7)");
                    FuckingWait.Start();
                    if (FuckingWait.Elapsed.Seconds > 12)
                        return;
                }
            }
            #endregion
            #region Quest 27964
            if (Me.QuestLog.GetQuestById(27964) != null && !Me.QuestLog.GetQuestById(27964).IsCompleted && !IsAttached27964)
            {
                Lua.Events.AttachEvent("CHAT_MSG_MONSTER_YELL", MY_27964);
                IsAttached27964 = true;
            }
            if (Me.QuestLog.GetQuestById(27964) != null && Me.QuestLog.GetQuestById(27964).IsCompleted && IsAttached27964)
            {
                Lua.Events.DetachEvent("CHAT_MSG_MONSTER_YELL", MY_27964);
                IsAttached27964 = false;
            }
            if (Me.QuestLog.GetQuestById(27964) != null && !Me.QuestLog.GetQuestById(27964).IsCompleted)
            {
                WoWObject wo = null;
                try
                {
                    wo = ObjectManager.GetObjectsOfType<WoWObject>().Where(u => u.Entry == 206573 && !Me.Combat && !Me.Dead).FirstOrDefault();
                }
                catch { }
                if (wo != null)
                {
                    while (!Me.QuestLog.GetQuestById(27964).IsCompleted)
                    {
                        while (wo.Distance > 5)
                        {
                            Navigator.MoveTo(wo.Location);
                            Thread.Sleep(100);
                        }
                        if (wo.Distance < 5)
                        {
                            wo.Interact();
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            #endregion
            #region Quest 28226
            if (Me.QuestLog.GetQuestById(28226) != null && !Me.QuestLog.GetQuestById(28226).IsCompleted)
            {

                if (GossipFrame.Instance.IsVisible)
                {
                    if (!Obj4Done28226 && GossipFrame.Instance.GossipOptionEntries.Count > 1)
                    {
                        Lua.DoString("SelectGossipOption(2)");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Lua.DoString("SelectGossipOption(1)");
                        Thread.Sleep(1000);
                    }
                }
            }
            #endregion
            #region Quest 26922
            if (Me.QuestLog.GetQuestById(26922) != null)
            {
                if (StickboneBerserker.Count > 0)
                {
                    StickboneBerserker[0].Target();
                    Thread.Sleep(500);
                    Lua.DoString("UseItemByName(60678)");
                    Thread.Sleep(500);
                    LegacySpellManager.ClickRemoteLocation(StickboneBerserker[0].Location);
                    Thread.Sleep(1000);
                }
                if (ScourgeBoneAnimus.Count > 0)
                {
                    Lua.DoString("UseItemByName(60678)");
                    Thread.Sleep(500);
                    LegacySpellManager.ClickRemoteLocation(ScourgeBoneAnimus[0].Location);
                    Thread.Sleep(1000);
                }
            }
            #endregion
            #region Quest 10129
            if (Me.QuestLog.GetQuestById(10129) != null && !Me.QuestLog.GetQuestById(10129).IsCompleted)
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(28038)", 0);
                if (!IsOnCD)
                {
                    Lua.DoString("UseItemByName(28038)");
                    LegacySpellManager.ClickRemoteLocation(GatewayShaadraz[0].Location);
                    Lua.DoString("UseItemByName(28038)");
                    LegacySpellManager.ClickRemoteLocation(GatewayMurketh[0].Location);
                     Thread.Sleep(10000);
               }
            }
            #endregion
            #region Quest 10162
            if (Me.QuestLog.GetQuestById(10162) != null && !Me.QuestLog.GetQuestById(10162).IsCompleted)
            {
                bool IsOnCD = Lua.GetReturnVal<bool>("GetItemCooldown(28132)", 0);
                if (!IsOnCD)
                {
                    if (!Obj1Done10162)
                    {
                        Lua.DoString("UseItemByName(28132)");
                        LegacySpellManager.ClickRemoteLocation(MoargOverseer[0].Location);
                    }
                    if (Obj1Done10162)
                    {
                        Lua.DoString("UseItemByName(28132)");
                        LegacySpellManager.ClickRemoteLocation(GanArgPeon[0].Location);
                    }
                    if (Obj2Done10162)
                    {
                        Lua.DoString("UseItemByName(28132)");
                        LegacySpellManager.ClickRemoteLocation(FelCannon[0].Location);
                    }
                }
            #endregion
                #region Quest 26305
                if (Me.QuestLog.GetQuestById(26305) != null && !Me.QuestLog.GetQuestById(26305).IsCompleted)
                {
                    if (Me.CurrentTarget.Entry == 2530)
                    {
                        Lua.DoString("UseItemByName(3912)");
                    }
                }
                #endregion
                #region Disguise Quests
                if (Me.QuestLog.GetQuestById(28439) != null || Me.QuestLog.GetQuestById(28440) != null || Me.QuestLog.GetQuestById(28432) != null || Me.QuestLog.GetQuestById(28433) != null || Me.QuestLog.GetQuestById(28434) != null || Me.QuestLog.GetQuestById(28435) != null)
                {
                    int counter = 0;
                    foreach (WoWAura s in ObjectManager.Me.ActiveAuras.Values)
                    {
                        if (s.Name.Contains("Disguise"))
                        {
                            counter++;
                        }
                    }
                    if (counter > 0)
                    {
                        Lua.DoString("UseItemByName(63357)");
                        Thread.Sleep(6000);
                    }
                }
                #endregion
                #region Quest 27001
                if (Me.QuestLog.GetQuestById(27001) != null && !Me.QuestLog.GetQuestById(27001).IsCompleted && SpiderThingy.Count > 0)
                {
                    WoWPoint wp = new WoWPoint(2432.375, -1650.377, 104.1796);
                    while (Me.Location.Distance(wp) > 3)
                    {
                        Navigator.MoveTo(wp);
                        Thread.Sleep(100);
                    }
                }
                #endregion
                #region Quest 10813
                if (Me.QuestLog.GetQuestById(10813) != null && !Me.QuestLog.GetQuestById(10813).IsCompleted && EyeOfGrillok.Count > 0)
                {
                    if (!Me.HasAura("Eye of Grillok"))
                    {
                        EyeOfGrillok[0].Target();
                        while (Me.Location.Distance(EyeOfGrillok[0].Location) > 10)
                        {
                            Navigator.MoveTo(EyeOfGrillok[0].Location);
                            Thread.Sleep(200);
                        }
                        Lua.DoString("UseItemByName(31463)");
                        Thread.Sleep(1000);
                    }
                }
                if (Me.QuestLog.GetQuestById(10813) != null && !Me.QuestLog.GetQuestById(10813).IsCompleted && Me.HasAura("Eye of Grillok"))
                {
                    WoWPoint wp = new WoWPoint(-1325.649,2356.759, 88.95618);
                    if (!Me.Combat)
                    {
                        while (Me.Location.Distance(wp) > 5)
                        {
                            Flightor.MoveTo(wp);
                            Thread.Sleep(200);
                        }
                    }
                }
                #endregion
            }
        }
        public void MY_27964(object Sender, LuaEventArgs arg)
        {
            if (arg.Args[0].ToString().Contains("What in hell's name is going on out here?"))
            {
                Thread.Sleep(2000);
                WoWUnit boss = null;
                try
                {
                    boss = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 47271 && !u.Dead).First();
                }
                catch { }
                if (boss != null && boss.IsAlive)
                {
                    while (boss.Distance > 5)
                    {
                        Navigator.MoveTo(boss.Location);
                        Thread.Sleep(100);
                    }
                    boss.Target();
                    boss.Interact();
                }
            }
        }
    }
}
