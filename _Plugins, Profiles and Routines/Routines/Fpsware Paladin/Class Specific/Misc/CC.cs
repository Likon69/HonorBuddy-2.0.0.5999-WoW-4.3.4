using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Hera
{
    public partial class Fpsware
    {
        // ************************************************************************************
        //
        public const string CCName = "Fpsware Paladin";                                   // Name of the CC displayed to the user
        public const string AuthorName = "Fpsware";                                     // Part of the string used in the CC name
        private readonly Version _versionNumber = new Version(0, 1, 2);                 // Part of the string used in the CC name
        public const WoWClass CCClass = WoWClass.Paladin;                                 // The class this CC will support
        // ************************************************************************************

        #region HB Start Up
        void BotEvents_OnBotStarted(EventArgs args)
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.Paladin.ClassSpec = (ClassHelpers.Paladin.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Paladin.ClassSpec, Me.Class));

            // Do important stuff on LUA events
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", EventHandlers.CombatLogEventHander);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", EventHandlers.TalentPointEventHander);
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", EventHandlers.TalentPointEventHander);

            Timers.Add("Pulse");          // Only do certain things in the Pulse check every 1 second
            Timers.Add("Seal");           // So we don't spam Seals
            Timers.Add("Environment");    // Check environment so we can dynamically load settings
            Timers.Add("DistanceCheck");
            Timers.Add("Interact");
            Timers.Add("HealingSpells"); // HB has an issue where it does not update health fast enough. This is a workaround to prevent heal spamming
            Timers.Add("PVPDance");
            Timers.Add("Exorcism");

            LoadSettings(true);

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
            if (!_isCCLoaded)
            {
                _isCCLoaded = true;
                Settings.DirtyData = true;
            }
            if (Settings.DirtyData) LoadSettings(true);

            // So we don't overload HB the below code is only run once per second
            if (!Timers.Expired("Pulse", 1000)) return;
            Timers.Recycle("Pulse", 1000);

            //
            if (!Me.Combat) Timers.Reset("PVPDance");


            // Decurse - You and all party members);
            if (!Settings.Cleanse.Contains("never") && Spell.CanCast("Cleanse"))
            {
                List<int> urgentRemoval = new List<int> { 35201 };
                bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                if (urgentCleanse || CLC.ResultOK(Settings.Cleanse))
                {
                    List<WoWDispelType> cureableList = new List<WoWDispelType> {WoWDispelType.Disease, WoWDispelType.Poison};
                    if (Settings.SacredCleansing.Contains("... talented") && !cureableList.Contains(WoWDispelType.Magic)) { cureableList.Add(WoWDispelType.Magic); }

                    WoWUnit p = ClassHelpers.Common.DecursePlayer(cureableList);
                    if (p != null) { if (Spell.CanCast("Cleanse")) Spell.Cast("Cleanse", p); }
                }
            }

            // Aura. Blessing (if soloing). Seal.
            if (ClassHelpers.Paladin.Aura.NeedToApplyAura && !Spell.IsGCD) ClassHelpers.Paladin.Aura.ApplyAura();
            if (!Me.IsInParty && ClassHelpers.Paladin.Blessing.NeedBlessing && !Spell.IsGCD) ClassHelpers.Paladin.Blessing.ApplyBlessing();
            if (ClassHelpers.Paladin.Seal.NeedToaApplySeal && !Spell.IsGCD) ClassHelpers.Paladin.Seal.ApplySeal();


            // Blessing (in party) - Special consideration for Druids (MotW = Kings)
            if (Me.IsInParty && Self.IsPowerPercentAbove(30) && !Spell.IsGCD && !Me.Combat && Settings.Blessings.Contains("Automatic"))
            {
                bool isKingsOk = true;
                bool seeDeadPeople = false;
                foreach (WoWPlayer p in Me.PartyMembers)                                        // First, check if we have a Druid in the group
                {
                    if (p.Dead || p.IsGhost) seeDeadPeople = true;
                    if (p.Class == WoWClass.Druid) isKingsOk = false;
                }

                if (!seeDeadPeople)                                                             // Only buff if everyone is alive
                {
                    string buff = isKingsOk ? "Blessing of Kings" : "Blessing of Might";        // Is it OK to use Kings?
                    WoWUnit player = RAF.BuffPlayer(buff);                                      // Find player to buff
                    if (player != null) { if (Spell.CanCast(buff)) Spell.Cast(buff, player); }  // Cast the buff
                }
            }

            // Righteous Fury
            if (Settings.RighteousFury.Contains("always") && !Self.IsBuffOnMe("Righteous Fury") && Spell.CanCast("Righteous Fury")) { Spell.Cast("Righteous Fury", Me); }
            if (Settings.RighteousFury.Contains("never") && Self.IsBuffOnMe("Righteous Fury")) { Self.RemoveBuff("Righteous Fury"); }
        }


        #endregion
    }
}
