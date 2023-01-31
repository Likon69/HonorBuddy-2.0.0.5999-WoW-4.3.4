using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Druid_ID = Hera.ClassHelpers.Druid.IDs;

namespace Hera
{
    public partial class Codemplosion
    {
        // ************************************************************************************
        //
        public const string CCName = "Codemplosion Druid";                                   // Name of the CC displayed to the user
        public const string AuthorName = "Deathburn";                                     // Part of the string used in the CC name
        private readonly Version _versionNumber = new Version(1, 0, 0);                 // Part of the string used in the CC name
        public const WoWClass CCClass = WoWClass.Druid;                                 // The class this CC will support
        // ************************************************************************************

        
        #region HB Start Up
        void BotEvents_OnBotStarted(EventArgs args)
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.Druid.ClassSpec = (ClassHelpers.Druid.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Druid.ClassSpec, Me.Class));

            // Do important stuff on LUA events
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", EventHandlers.CombatLogEventHander);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", TalentPointEventHander);
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", TalentPointEventHander);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", TalentPointEventHander);
            Mount.OnMountUp += new EventHandler<MountUpEventArgs>(Mount_OnMountUp);
            //Lua.Events.AttachEvent("PARTY_MEMBER_DISABLE", PlayerDeadEventHandler);
            

            Timers.Add("Pulse");          // Only do certain things in the Pulse check every 1 second
            Timers.Add("DistanceCheck");
            Timers.Add("FaceTarget");
            Timers.Add("PullSpellCast");
            Timers.Add("EnvironmentSettings");
            Timers.Add("TimersTest");
            Timers.Add("Interact");
            Timers.Add("Spam");
            Timers.Add("Pull");
            Timers.Add("Healing");
            Timers.Add("TravelForm");
            Timers.Add("TravelFormMount");
            Timers.Add("Mushrooms");
            Timers.Add("MushroomsGoBoom");
            Timers.Add("FeralCatHealSpam");
            Timers.Add("Retarget");
            Timers.Add("FeralCharge");
            Timers.Add("RootBreak");




            // Environmental Settings
            string environment = Utils.IsBattleground ? "PVP" : "PVE";
            environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            ConfigSettings.CurrentEnvironment = environment;
            //Utils.Log(string.Format("*** Loading {0} settings.", environment),Utils.Colour("Red"));

            LoadSettings(false);
            Settings.PopulateRangedCapableMobs();
            Settings.PopulatePriorityMobs();
            Settings.PopulateHealingSpells();
            Settings.PopulateImportantInterruptSpells();

        }

        public static void TalentPointEventHander(object sender, LuaEventArgs args)
        {
            ClassHelpers.Druid.ClassSpec = (ClassHelpers.Druid.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Druid.ClassSpec, Me.Class));
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

        // The below code idea was borrowed from Bobby. Credit to him for this mount check event
        void Mount_OnMountUp(object sender, MountUpEventArgs e)
        {
            if (!Spell.IsKnown("Travel Form")) return;
            if (ClassHelpers.Druid.CLCTravelForm.ToUpper().Contains("NEVER")) return;

            if (ClassHelpers.Druid.CLCTravelForm.ToUpper().Contains("ALWAYS") && ClassHelpers.Druid.Shapeshift.IsTravelForm)
            {
                e.Cancel = true;
                return;
            }
            
            if (ClassHelpers.Druid.Shapeshift.IsTravelForm) return;
            while (Me.IsCasting) System.Threading.Thread.Sleep(250);

            /*
            if ((Me.IsInParty || Me.IsInRaid) && RAF.PartyTankRole != null && !RAF.PartyTankRole.IsFlying && RAF.PartyTankRole.Distance < 50)
            {
                e.Cancel = true;
                return;
            }
             */

            if (ClassHelpers.Druid.CLCTravelForm.ToUpper().Contains("ALWAYS"))
            {
                if (Timers.Expired("TravelFormMount", 2000))
                {
                    Spell.Cast("Travel Form");
                    Timers.Reset("TravelFormMount");
                }
                e.Cancel = true;
                return;
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

            // Decurse / Remove Corruption
            if (CLC.ResultOK(Settings.RemoveCorruptionBalance) && Spell.CanCast("Remove Corruption"))
            {
                WoWUnit p = ClassHelpers.Druid.NeedToDecursePlayer;
                if (p != null && Spell.CanCast("Remove Corruption")) Spell.Cast("Remove Corruption", p);
            }

            // Buff players with MotW);
            if (Settings.MarkOfTheWild.Contains("always") && Me.IsInParty && Me.ManaPercent > 40)
            {
                WoWUnit p = RAF.PartyMemberWithoutBuff("Mark of the Wild");
                if (p != null)
                {
                    if (Spell.CanCast("Mark of the Wild")) Spell.Cast("Mark of the Wild", p);
                }
            }


            // Predator's Swiftness
            if (CLC.ResultOK(Settings.PredatorsSwiftnessFeralCat) && !Self.IsHealthPercentAbove(Settings.PredatorsSwiftnessFeralCatHealth))
            {
                if (Self.IsBuffOnMeLUA("Predator's Swiftness") && Spell.CanCast(Settings.PredatorsSwiftnessFeralCatSpell))
                {
                    Utils.Log("** Predator's Swiftness **");
                    Spell.Cast(Settings.PredatorsSwiftnessFeralCatSpell, Me);
                }
            }


            // Resurrection
            if (!Me.Combat && Spell.CanCast("Revive") && !Me.IsCasting)
            {
                foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.Dead && !p.IsGhost && p.InLineOfSight))
                {
                    if (Timers.Exists(p.Guid.ToString()) && !Timers.Expired(p.Guid.ToString(), 15000)) continue;

                    Spell.Cast("Revive", p);
                    Utils.LagSleep();
                    Timers.Add(p.Guid.ToString());  // Prevent spamming resurrection on th same target
                    System.Threading.Thread.Sleep(1500);
                    if (!Me.IsCasting) Spell.StopCasting();
                    while (Me.IsCasting)
                    {
                        if (!p.Dead)
                        {
                            Utils.Log("-Emmm.... it appears our dead party member is now alive. So why are we still trying to rez them?");
                            Spell.StopCasting();
                        }
                    }
                    break;
                }
            }

            // Clean up timers
            foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.IsAlive && Timers.Exists(p.Guid.ToString())))
            {
                Timers.Remove(p.Guid.ToString());
            }

            // Make sure we have a target - Instance only
            // Sometimes IB was not selecting a target when we were in combat. This fucked up things immensely!
            if (Me.IsInInstance && !Me.GotTarget)
            {
                WoWUnit tank = RAF.PartyTankRole;
                if (tank != null && tank.GotTarget && tank.Combat) tank.CurrentTarget.Target();
            }

            // Try and grab a target all the time, if we're sitting around doing nothing check if anyone in our party has a target and take it. 
            if (!Me.Combat)
            foreach (WoWPlayer player in Me.PartyMembers)
            {
                if (!player.Combat) continue;
                if (player.Distance> 80) continue;
                if (!player.GotTarget) continue;

                player.CurrentTarget.Target();
            }

            // Out of combat healing. Make sure everyone is topped up.
            if (!Me.Mounted && !Me.IsFlying && !Me.Combat && (Me.IsInParty || Me.IsInRaid) && Spell.CanCast("Regrowth"))
            {
                List<WoWPlayer> myPartyOrRaidGroup = Me.PartyMembers;
                List<WoWPlayer> healTarget = (from o in myPartyOrRaidGroup let p = o.ToPlayer() where p.Distance < 40 && !p.Auras.ContainsKey("Regrowth") && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < 80 select p).ToList();
                //List<WoWPlayer> healTarget = (from o in myPartyOrRaidGroup let p = o.ToPlayer() where p.Distance < 40 && !p.Auras.ContainsKey("Regrowth") && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < 80 select p).ToList();

                if (healTarget.Count > 0)
                {
                    Spell.Cast("Regrowth", healTarget.FirstOrDefault());
                }
                else if (Me.HealthPercent < 80)
                {
                    Spell.Cast("Regrowth", Me);
                }
            }


            // Buffs and such
            if (!Me.IsFlying && !Me.Mounted)
            {
                if (!Self.IsBuffOnMe(Druid_ID.MarkOfTheWild) && Spell.CanCast("Mark of the Wild") && Settings.MarkOfTheWild.Contains("always")) Spell.Cast("Mark of the Wild", Me);

                if (!Me.Combat)
                {
                    if (!Me.IsMoving) Timers.Reset("TravelForm");
                    if (Timers.Expired("TravelForm",4000) &&  ClassHelpers.Druid.Shapeshift.CanUseTravelForm) ClassHelpers.Druid.Shapeshift.TravelForm();
                }

                /*
                if (Me.Combat)
                {

                    if (Me.GotTarget && Me.CurrentTarget.IsCasting)
                    {
                        Utils.Log("==============[TARGET IS CASTING A SPELL]==============");
                        Utils.Log("=== Spell name: " + CT.CastingSpell.Name);
                        Utils.Log("=== Spell ID: " + CT.CastingSpell.Id);
                        Utils.Log("=== Spell description: " + CT.CastingSpell.Description);
                        Utils.Log("=== Spell school: " + CT.CastingSpell.School);
                        Utils.Log("=== Spell mechanic: " + CT.CastingSpell.Mechanic.ToString());
                        Utils.Log("=== Spell cast time (ms): " + CT.CastingSpell.CastTime);
                        Utils.Log("=======================================================");
                    }
                    
                }
                 */

                /*
                if (Me.Combat)
                {
                    if (Me.IsInInstance)
                    {
                        foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                        {
                            //Utils.Log("=====================[Threat Info]============================");
                            if (!unit.Combat) continue;
                            if (unit.Distance > 80) continue;
                            Utils.Log(string.Format("-- {0}, TargetGuid {1}, ThreatStatus {2}, ThreatPercent {3}, RawPercent {4}", unit.Name, unit.ThreatInfo.TargetGuid,unit.ThreatInfo.ThreatStatus,unit.ThreatInfo.RawPercent,unit.ThreatInfo.RawPercent));
                            Utils.Log("-- Threat for ME " + unit.GetThreatInfoFor(Me));

                        }
                    }
                }
                 */
            }

            // If you are drinking/eating and the tank is in combat and low on health then get your ass up and heal
            if (Self.IsBuffOnMe("Food") || Self.IsBuffOnMe("Drink"))
            {
                WoWUnit tank = RAF.PartyTankRole;

                if (tank != null && Me.IsInInstance && tank.Combat)
                {
                    if (tank.HealthPercent < 70)
                    {
                        Utils.Log("-Oh shit! The tank has pulled while we were drinking");
                        Lua.DoString("CancelUnitBuff('player', 'Food')");
                        Lua.DoString("CancelUnitBuff('player', 'Drink')");
                    }
                }
            }





        }


        #endregion
    }
}
