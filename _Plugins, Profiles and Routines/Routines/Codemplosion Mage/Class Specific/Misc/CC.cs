using System;
using System.Collections.Generic;
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
        public const string CCName = "Codemplosion Mage";                                   // Name of the CC displayed to the user
        public const string AuthorName = "Deathburn";                                     // Part of the string used in the CC name
        private readonly Version _versionNumber = new Version(1, 0, 0);                 // Part of the string used in the CC name
        public const WoWClass CCClass = WoWClass.Mage;                                 // The class this CC will support
        // ************************************************************************************

        
        #region HB Start Up
        void BotEvents_OnBotStarted(EventArgs args)
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.Mage.ClassSpec = (ClassHelpers.Mage.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Mage.ClassSpec, Me.Class));

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
            Timers.Add("FrostNova");
            Timers.Add("Polymorph");
            Timers.Add("EnvironmentSettings");
            Timers.Add("Flamestrike");
            Timers.Add("TimersTest");


            // Environmental Settings
            string environment = Utils.IsBattleground ? "PVP" : "PVE";
            environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            ConfigSettings.CurrentEnvironment = environment;
            //Utils.Log(string.Format("*** Loading {0} settings.", environment),Utils.Colour("Red"));

            LoadSettings(false);

        }

        public static void TalentPointEventHander(object sender, LuaEventArgs args)
        {
            ClassHelpers.Mage.ClassSpec = (ClassHelpers.Mage.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Mage.ClassSpec, Me.Class));
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

            // Remove Curse - You and all party members);
            if (!Settings.Cleanse.Contains("never") && Spell.CanCast("Remove Curse"))
            {
                List<int> urgentRemoval = new List<int> { 9999 };
                bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                if (urgentCleanse || CLC.ResultOK(Settings.Cleanse))
                {
                    List<WoWDispelType> cureableList = new List<WoWDispelType> { WoWDispelType.Curse };
                    WoWUnit p = ClassHelpers.Common.DecursePlayer(cureableList, CLC.ResultOK(Settings.Cleanse));
                    if (p != null) { if (Spell.CanCast("Remove Curse")) Spell.Cast("Remove Curse", p); }
                }
            }

            
            // Buffs and such
            if (!Me.IsFlying && !Me.Mounted)
            {
                if (!Self.IsBuffOnMe(Settings.ArmorBuff) && Spell.CanCast(Settings.ArmorBuff)) Spell.Cast(Settings.ArmorBuff);
                
                // Int on Party Members
                if (Me.IsInParty && !Me.Combat && Spell.CanCast("Arcane Brilliance"))
                {
                    const string buffName = "Arcane Brilliance";
                    WoWUnit target = RAF.PartyMemberWithoutBuff(buffName);
                    if (target != null && !target.Auras.ContainsKey("Fel Intelligence")) Spell.Cast(buffName, target);
                }
                // Int on Me
                if (!Self.IsBuffOnMe("Arcane Brilliance") && !Self.IsBuffOnMe("Fel Intelligence") && Spell.CanCast("Arcane Brilliance")) Spell.Cast("Arcane Brilliance", Me);

                // Water Elemental
                if (ClassHelpers.Mage.WaterElemental.NeedToCall) ClassHelpers.Mage.WaterElemental.Call();

                
                if (!Me.Combat)
                {
                    // Make Refreshments
                    if (!ClassHelpers.Mage.ConjuredItems.GotRefreshment() && Spell.CanCast("Conjure Refreshment"))
                    {
                        if (Me.IsMoving) Movement.StopMoving();
                        Utils.LagSleep();
                        Spell.Cast("Conjure Refreshment");
                        Utils.WaitWhileCasting();
                    } 

                    // Make Mana Gem
                    if (!ClassHelpers.Mage.ConjuredItems.HaveManaGem() && Spell.CanCast("Conjure Mana Gem"))
                    {
                        if (Me.IsMoving) Movement.StopMoving();
                        Utils.LagSleep();
                        Spell.Cast("Conjure Mana Gem");
                        Utils.WaitWhileCasting();

                    }

                    
                }
            }





        }


        #endregion
    }
}
