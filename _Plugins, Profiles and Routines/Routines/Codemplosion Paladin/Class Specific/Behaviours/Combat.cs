using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Hera
{
    public partial class Codemplosion
    {

        #region Combat!
        private Composite _combatBehavior;
        public override Composite CombatBehavior
        {
            get { if (_combatBehavior == null) { Utils.Log("Creating 'Combat' behavior"); _combatBehavior = CreateCombatBehavior(); } return _combatBehavior; }
        }
        
         private PrioritySelector CreateCombatBehavior()
         {
             return new PrioritySelector(
                 // All spells are checked in the order listed below, a prioritised order
                 // Put the most important spells at the top of the list

                // Make sure we always have a target. If we don't have a target, take the pet's target - if they have one.
                new Decorator(ret => !Me.GotTarget && Me.GotAlivePet && Me.Pet.GotTarget, new Action(ret => Me.Pet.CurrentTarget.Target())),

                // Trying to prevent overshoot when running towards the target
                new NeedToMoveTo(new MoveTo()),

                // PVPDance
                new NeedToPVPDance(new PVPDance()),

                // Check if we get aggro during the pull
                 // This is in here and not the pull because we are in combat at this point
                new NeedToCheckAggroOnPull(new CheckAggroOnPull()),

                // Retarget
                new NeedToRetarget(new Retarget()),

                // Abort combat is the target's health is 95% + after 30 seconds of combat
                new NeedToCheckCombatTimer(new CheckCombatTimer()),

                // LOS Check
                new NeedToLOSCheck(new LOSCheck()),

                // Auto Attack
                new NeedToAutoAttack(new AutoAttack()),

                // Interact - Face the target
                new NeedToInteract(new Interact()),

                // Backup
                new NeedToBackup(new Backup()),


                // ************************************************************************************
                 // Important/time sensative spells here
                 // These are spells that need to be case asap

                // Hammer Of Wrath
                new NeedToHammerOfWrath(new HammerOfWrath()),

                // Emergency Heal
                //new NeedToEmergencyHeal(new EmergencyHeal()),

                // Rebuke
                new NeedToRebuke(new Rebuke()),

                // Zealotry
                new NeedToZealotry(new Zealotry()),

                // Hammer Of Justice
                new NeedToHammerOfJustice(new HammerOfJustice()),

                // Procs - Divine Purpose
                new NeedToDivinePurpose(new DivinePurpose()),

                // Procs - The Art Of War and Denounce
                new NeedToProcs(new Procs()),

                // Templars Verdict
                new NeedToTemplarsVerdict(new TemplarsVerdict()),

                // Divine Storm
                new NeedToDivineStorm(new DivineStorm()),

                // Avenger's Shield
                new NeedToAvengersShield(new AvengersShield()),

                // Guardian of Ancient Kings
                new NeedToGoAK(new GoAK()),

                // Inquisition
                new NeedToInquisition(new Inquisition()),

                // Shield Of The Righteous
                new NeedToShieldOfTheRighteous(new ShieldOfTheRighteous()),

                // Consecration
                new NeedToConsecration(new Consecration()),

                // Avenging Wrath
                new NeedToAvengingWrath(new AvengingWrath()),

                // Holy Wrath
                new NeedToHolyWrath(new HolyWrath()),

                // Judgement
                new NeedToJudgement(new Judgement()),



                // ************************************************************************************
                 // Other spells here

                // Holy Shock
                new NeedToHolyShock(new HolyShock()),

                // Hammer Of The Righteous
                new NeedToHammerOfTheRighteous(new HammerOfTheRighteous()),

                // Crusader Strike
                new NeedToCrusaderStrike(new CrusaderStrike()),

                // Exorcism
                new NeedToExorcism(new Exorcism())

                );
        }
        #endregion
        
       


        #region Behaviours
        
        #region Judgement
        public class NeedToJudgement : Decorator
        {
            public NeedToJudgement(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Judgement";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(spellName))) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Judgement : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Judgement";

                if (!Me.IsMoving) { Target.Face(); Utils.LagSleep(); }
                
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion
        
        #region Hand of Reckoning
        public class NeedToHandOfReckoningPull : Decorator
        {
            public NeedToHandOfReckoningPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Hand of Reckoning";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Spell.IsOnCooldown("Judgement")) return false;
                if (Utils.IsBattleground) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(spellName))) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class HandOfReckoningPull : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Hand of Reckoning";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region PVPDance
        public class NeedToPVPDance : Decorator
        {
            public NeedToPVPDance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.PVPDance.Contains("never")) return false;
                if (Me.IsCasting) return false;
                if (!Timers.Expired("PVPDance",Settings.PVPDanceInterval)) return false;
                if (Utils.Adds) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange+1.5f)) return false;
                if (!Utils.IsBattleground) return false;
                if (CT.IsMoving) return false;

                return true;
            }
        }

        public class PVPDance : Action
        {
            protected override RunStatus Run(object context)
            {
                //Utils.Log("*** PVP DANCE!");
                WoWPoint pointBehind = WoWMathHelper.CalculatePointBehind(CT.Location, Me.CurrentTarget.Rotation, Target.InteractRange - 0.95f);
                Movement.MoveTo(pointBehind);
                Utils.LagSleep();
                Timers.Reset("PVPDance");

                while (Me.IsMoving) Thread.Sleep(150); 
                Target.Face(); 
                Utils.LagSleep();
                CT.Interact();

                return RunStatus.Failure;
            }
        }
        #endregion

       

        #region Hammer of the Righteous
        public class NeedToHammerOfTheRighteous: Decorator
        {
            public NeedToHammerOfTheRighteous(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Hammer of the Righteous";

                if (!CLC.ResultOK(Settings.HammerOfTheRighteous)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class HammerOfTheRighteous : Action
        {
            protected override RunStatus Run(object context)
            {
                if (!Me.IsMoving) Target.Face();
                string spellName = "Hammer of the Righteous";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Crusader Strike
        public class NeedToCrusaderStrike: Decorator
        {
            public NeedToCrusaderStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Crusader Strike";

                if (!CLC.ResultOK(Settings.CrusaderStrike)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class CrusaderStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                if (!Me.IsMoving) Target.Face();
                string spellName = "Crusader Strike";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Holy Shock
        public class NeedToHolyShock : Decorator
        {
            public NeedToHolyShock(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Holy Shock";

                if (!CLC.ResultOK(Settings.HolyShock)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(spellName))) return false;
                
                return (Spell.CanCast(spellName));
            }
        }

        public class HolyShock : Action
        {
            protected override RunStatus Run(object context)
            {
                if (!Me.IsMoving) Target.Face();
                string spellName = "Holy Shock";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Avenger's Shield
        public class NeedToAvengersShield : Decorator
        {
            public NeedToAvengersShield(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Avenger's Shield";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(spellName))) return false;
                if (!CLC.ResultOK(Settings.AvengersShield)) return false;
                //if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class AvengersShield : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Avenger's Shield";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Avenger's Shield Pull
        public class NeedToAvengersShieldPull : Decorator
        {
            public NeedToAvengersShieldPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Avenger's Shield";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(spellName))) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class AvengersShieldPull : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Avenger's Shield";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Consecration
        public class NeedToConsecration : Decorator
        {
            public NeedToConsecration(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Consecration";

                if (!CLC.ResultOK(Settings.Consecration)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange+0.5f)) return false;
                //if (!CT.WithinInteractRange) return false;
                //if (Settings.LowLevelCheck.Contains("always"))if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Consecration : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Consecration";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Holy Wrath
        public class NeedToHolyWrath : Decorator
        {
            public NeedToHolyWrath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Holy Wrath";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.HolyWrath)) return false;
                //if (Settings.LowLevelCheck.Contains("always")) if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class HolyWrath : Action
        {
            protected override RunStatus Run(object context)
            {
                // Issues farming low level instances. Add this to fix a crash issue
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return RunStatus.Failure;

                string spellName = "Holy Wrath";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Hammer Of Justice
        public class NeedToHammerOfJustice : Decorator
        {
            public NeedToHammerOfJustice(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Hammer of Justice";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.HammerOfJustice)) return false;
                //if (Settings.LowLevelCheck.Contains("always")) if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class HammerOfJustice : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Hammer of Justice";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Templars Verdict
        public class NeedToTemplarsVerdict : Decorator
        {
            public NeedToTemplarsVerdict(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Templar's Verdict";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.TemplarsVerdict)) return false;
                if (!CLC.ResultOK(Settings.TemplarsVerdictHolyPower)) return false;
                //if (Settings.LowLevelCheck.Contains("always")) if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class TemplarsVerdict : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Templar's Verdict";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Avenging Wrath
        public class NeedToAvengingWrath : Decorator
        {
            public NeedToAvengingWrath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Avenging Wrath";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.AvengingWrath)) return false;
                if (!Target.IsHealthAbove(50) && (!Me.IsInInstance || !Target.IsElite)) return false;
                //if (Settings.LowLevelCheck.Contains("always")) if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class AvengingWrath : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Avenging Wrath";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rebuke
        public class NeedToRebuke : Decorator
        {
            public NeedToRebuke(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Rebuke";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.Rebuke)) return false;
                if (!Target.IsCasting) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Rebuke : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Rebuke";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Divine Storm
        public class NeedToDivineStorm : Decorator
        {
            public NeedToDivineStorm(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Divine Storm";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.DivineStorm)) return false;
                if (!Settings.DivineStorm.Contains("always") && Utils.CountOfAddsInRange(8, Me.Location) < 2) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class DivineStorm : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Divine Storm";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region The Art of War and Denounce PROC
        public class NeedToTheArtOfWar : Decorator
        {
            public NeedToTheArtOfWar(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Exorcism";
                string AoWBuffName = "The Art of War";
                string DenounceBuffName = "Denounce";

                if (!CLC.ResultOK(Settings.TheArtOfWar)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                
                Lua.DoString(string.Format("buffName,_,_,stackCount,_,_,_,_,_=UnitBuff(\"player\",\"{0}\")", AoWBuffName));
                string buffName = Lua.GetLocalizedText("buffName", Me.BaseAddress);

                Lua.DoString(string.Format("denouncebuffName,_,_,stackCount,_,_,_,_,_=UnitBuff(\"player\",\"{0}\")", DenounceBuffName));
                string denounceBuffName = Lua.GetLocalizedText("denouncebuffName", Me.BaseAddress);

                if (buffName != AoWBuffName && denounceBuffName != DenounceBuffName) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class TheArtOfWar : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Exorcism";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shield of the Righteous
        public class NeedToShieldOfTheRighteous : Decorator
        {
            public NeedToShieldOfTheRighteous(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Shield of the Righteous";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.ShieldOfTheRighteous)) return false;
                if (!CLC.ResultOK(Settings.ShieldOfTheRighteousHolyPower)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class ShieldOfTheRighteous : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Shield of the Righteous";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Inquisition
        public class NeedToInquisition: Decorator
        {
            public NeedToInquisition(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Inquisition";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.Inquisition)) return false;
                if (!CLC.ResultOK(Settings.InquisitionHolyPower)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Inquisition : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Inquisition";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Guardian of Ancient Kings
        public class NeedToGoAK: Decorator
        {
            public NeedToGoAK(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Guardian of Ancient Kings";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.GuardianOfAncientKings)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class GoAK : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Guardian of Ancient Kings";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Hammer of Wrath
        public class NeedToHammerOfWrath : Decorator
        {
            public NeedToHammerOfWrath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Hammer of Wrath";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.HammerOfWrath)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class HammerOfWrath : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Hammer of Wrath";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Zealotry
        public class NeedToZealotry : Decorator
        {
            public NeedToZealotry(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Zealotry";

                if (!CLC.ResultOK(Settings.Zealotry)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Me.CurrentHolyPower < 3) return false;
                if (!Target.IsHealthAbove(40) && !Target.IsElite) return false;

                /*
                uint myHp = Me.MaxHealth;
                uint ctHp = CT.MaxHealth;
                bool result = (ctHp > myHp * 10);
                                 */
                //if (!Target.IsInstanceBoss) return false;

                //if (!CLC.ResultOK(Settings.Zealotry)) return false;
                //if (Self.CanBuffMe(spellName)) return false;
                //if (Target.CanDebuffTarget(spellName)) return false;
                //if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Zealotry : Action
        {
            protected override RunStatus Run(object context)
            {
                //Utils.Log(string.Format("{0} has very high health, using Zealotry", CT.Name));
                
                string spellName = "Zealotry";
                bool result = Spell.Cast(spellName);
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(spellName);
                //bool result = Self.IsBuffOnMe(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Emergency Heal
        public class NeedToEmergencyHeal : Decorator
        {
            public NeedToEmergencyHeal(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Holy Light";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Settings.PartyHealWhen.Contains("never")) return false;
                if (!Me.IsInParty) return false;
                if (!Self.IsPowerPercentAbove(25)) return false;

                WoWUnit healer = RAF.PartyHealerRole;
                if ((healer != null && Settings.PartyHealWhen.Contains("OOM or Dead") && (healer.ManaPercent < 30 || healer.Dead)) || Settings.PartyHealWhen.Contains("always") || healer != null && healer.Distance2D > 60)
                {
                    foreach (WoWPlayer p in Me.PartyMembers)
                    {
                        if (p.HealthPercent < Settings.PartyFlashOfLight || p.HealthPercent < Settings.PartyHolyLight)
                        {
                            if (p.InLineOfSight && p.Distance2D < 40) return true;
                        }
                    }
                }

                return false;
            }
        }

        public class EmergencyHeal : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "";

                foreach (WoWPlayer p in Me.PartyMembers)
                {
                    if (!p.Dead && p.HealthPercent < Settings.PartyFlashOfLight || !p.Dead && p.HealthPercent < Settings.PartyHolyLight)
                    {
                        if (p.HealthPercent < Settings.PartyFlashOfLight) spellName = "Flash of Light";
                        else if (p.HealthPercent < Settings.PartyHolyLight) spellName = "Holy Light";

                        int healthValue = spellName == "Holy Light" ? Settings.PartyHolyLight : Settings.PartyFlashOfLight;

                        if (Me.IsMoving) Movement.StopMoving();
                        Spell.Cast(spellName, p);
                        Utils.LagSleep();
                        Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, healthValue, p);
                        break;
                    }
                }

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Interact - Move closer or face the target
        public class NeedToInteract : Decorator
        {
            public NeedToInteract(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                return (Timers.Expired("Interact", 650));
            }
        }

        public class Interact : Action
        {
            protected override RunStatus Run(object context)
            {
                CT.Interact();
                Timers.Reset("Interact");

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Back up if we're too close
        public class NeedToBackup : Decorator
        {
            public NeedToBackup(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                double interactDistance = Target.InteractRange;

                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;
                if (Target.IsFleeing) return false;
                if (CT.IsMoving) return false;

                if (!Timers.Expired("DistanceCheck", 1000)) return false;

                return (Target.IsDistanceLessThan(interactDistance * 0.5));

            }
        }

        public class Backup : Action
        {
            protected override RunStatus Run(object context)
            {
                double interactDistance = Target.InteractRange;

                while (Target.IsDistanceLessThan(interactDistance * 0.7))
                {
                    if (CT.IsMoving) break;
                    WoWMovement.Move(WoWMovement.MovementDirection.Backwards);
                    Thread.Sleep(100);
                }

                WoWMovement.MoveStop();
                Utils.Log("Backup a wee bit, too close to our target", Utils.Colour("Blue"));
                Timers.Reset("DistanceCheck");

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Retarget
        public class NeedToRetarget : Decorator
        {
            public NeedToRetarget(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Me.GotTarget && CT.IsAlive) return false;
                if (!Me.Combat) return false;

                return true;
            }
        }

        public class Retarget : Action
        {
            protected override RunStatus Run(object context)
            {
                List<WoWUnit> hlist = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < 40 && !p.Dead && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet || p.IsTargetingMyRaidMember) && p.Attackable
                     orderby p.HealthPercent ascending
                     orderby p.Distance2D ascending
                     select p).ToList();

                // No sapped target, kill anything. 
                if (hlist.Count > 0) hlist[0].Target();

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Procs - The Art of War and Denounce
        public class NeedToProcs : Decorator
        {
            public NeedToProcs(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Exorcism";
                List<int> procList = new List<int> { 85509,96287,59578 };

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if ((from aura in Me.ActiveAuras from procID in procList where procID == aura.Value.SpellId select aura).Any()) return (Spell.CanCast(dpsSpell));

                return false;
            }
        }

        public class Procs : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Exorcism";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Procs - Divine Purpose
        public class NeedToDivinePurpose: Decorator
        {
            public NeedToDivinePurpose(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.DivinePurpose;

                // 90174 = Divine Purpose
                if (!Self.IsBuffOnMe(90174,Self.AuraCheck.AllAuras)) return false;

                if (Settings.DivinePurpose.Contains("Inquisition >"))
                {
                    string[] spellList = Settings.DivinePurpose.Split('>');
                    dpsSpell = Me.ActiveAuras.ContainsKey("Inquisition") ? spellList[1].Trim() : spellList[0].Trim();
                }

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                return (Spell.CanCast(dpsSpell));

            }
        }

        public class DivinePurpose : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.DivinePurpose;
                
                if (Settings.DivinePurpose.Contains("Inquisition >"))
                {
                    string[] spellList = Settings.DivinePurpose.Split('>');
                    dpsSpell = Me.ActiveAuras.ContainsKey("Inquisition") ? spellList[1].Trim() : spellList[0].Trim();
                }
                
                Utils.Log("-Divine Purpose Proc");
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Exorcism
        public class NeedToExorcism : Decorator
        {
            public NeedToExorcism(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Exorcism";

                int exorcismSeconds = Convert.ToInt16(Settings.ExorcismSeconds);
                int exorcismTimer = 1000*exorcismSeconds;

                if (!CLC.ResultOK(Settings.Exorcism)) return false;
                if (!Timers.Expired("Exorcism", exorcismTimer)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Exorcism : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Exorcism";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                Utils.WaitWhileCasting();

                Timers.Reset("Exorcism");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #endregion

    }
}