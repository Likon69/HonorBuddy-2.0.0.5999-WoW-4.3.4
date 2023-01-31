using System.Collections.Generic;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using System;
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
            get { if (_combatBehavior == null)  {  Utils.Log("Creating 'Combat' behavior");  _combatBehavior = CreateCombatBehavior();  }  return _combatBehavior; }
        }

        private PrioritySelector CreateCombatBehavior()
        {
            return new PrioritySelector(
                // All spells are checked in the order listed below, a prioritised order
                
                // Make sure we always have a target. If we don't have a target, take the pet's target - if they have one.
                new Decorator(ret => (!Me.GotTarget || Me.GotTarget && Me.CurrentTarget.Dead) && Me.GotAlivePet && Me.Pet.GotTarget, new Action(ret => Me.Pet.CurrentTarget.Target())),
                new Decorator(ret => (!Me.GotTarget || Me.GotTarget && Me.CurrentTarget.Dead) && Targeting.Instance.TargetList.Count > 0, new Action(ret => Targeting.Instance.TargetList[0].Target())),

                // Check if we get aggro during the pull
                // This is in here and not the pull because we are in combat at this point
                new NeedToCheckAggroOnPull(new CheckAggroOnPull()),

                // Abort combat is the target's health is 95% + after 30 seconds of combat
                //new NeedToCheckCombatTimer(new CheckCombatTimer()),

                // Kind of important that we face the target
                new NeedToFaceTarget(new FaceTarget()),

                // Aspects
                new NeedToAspects(new Aspects()),

                // Auto attack
                new NeedToAutoAttack(new AutoAttack()),

                // Pre Loot Movement
                new NeedToPreLootMovement(new PreLootMovement()),

                // LOS Check
                new NeedToLOSCheck(new LOSCheck()),

                // Distance check. Make sure we are in attacking range at all times
                new NeedToCheckDistance(new CheckDistance()),

                // Face Target
                new NeedToFaceTarget(new FaceTarget()),

                // Feign Death
                new NeedToFeignDeath(new FeignDeath()),

                // Widow Venom - Almost always used in BG. So needs to be cast ASAP
                new NeedToWidowVenom(new WidowVenom()),

                
                // ************************************************************************************
                // Important/time sensative spells here
                // These are spells that need to be case asap

                // Fire! Proc
                new NeedToFireProc(new FireProc()),

                // Fervor
                new NeedToFervor(new Fervor()),

                // Trap Launcher
                new NeedToTrapLauncher(new TrapLauncher()),

                // Traps In Combat
                new NeedToTrapsInCombat(new TrapsInCombat()),

                // Traps On Adds
                new NeedToTrapsOnAdds(new TrapsOnAdds()),

                // Focus Fire
                new NeedToFocusFire(new FocusFire()),

                // Disengage - PVE Only
                new NeedToDisengage(new Disengage()),

                // Disengage - Battlegrounds Only
                new NeedToDisengageBGOnly(new DisengageBGOnly()),

                // Backup - if closer than 5 yards
                new NeedToBackup(new Backup()),

                // Raptor Strike
                new NeedToRaptorStrike(new RaptorStrike()),

                // Kill Shot
                new NeedToKillShot(new KillShot()),

                // Steady Shot - Survival spec only
                new NeedToSteadyShotSurvival(new SteadyShotSurvival()),

                // Intimidation
                new NeedToIntimidation(new Intimidation()),

                // Silencing Shot
                new NeedToSilencingShot(new SilencingShot()),

                // Explosive Shot
                new NeedToExplosiveShot(new ExplosiveShot()),

                // Kill Command
                new NeedToKillCommand(new KillCommand()),

                // Rapid Fire
                new NeedToRapidFire(new RapidFire()),

                // Bestial Wrath
                new NeedToBestialWrath(new BestialWrath()),

                // Mob Attacking Me - send pet to attack them
                new NeedToMobAttackingMe(new MobAttackingMe()),

                // ************************************************************************************
                // Other spells here

                // Hunters Mark
                new NeedToHuntersMark(new HuntersMark()),

                // Black Arrow
                new NeedToBlackArrow(new BlackArrow()),

                // Serpent Sting
                new NeedToSerpentSting(new SerpentSting()),

                // Chimera Shot
                new NeedToChimeraShot(new ChimeraShot()),

                // Multi Shot
                new NeedToMultiShot(new MultiShot()),

                // Aimed Shot
                new NeedToAimedShot(new AimedShot()),

                // Arcane Shot
                new NeedToArcaneShot(new ArcaneShot()),

                // Steady Shot
                new NeedToFocusShot(new FocusShot())


                );
        }
        #endregion

        #region Focus Fire
        public class NeedToFocusFire : Decorator
        {
            public NeedToFocusFire(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Focus Fire";

                if (!Utils.CombatCheckOk(spellName)) return false;
                if (!Me.GotAlivePet) return false;
                if (!CLC.ResultOK(Settings.FocusFire)) return false;
                if (Me.Auras.ContainsKey("Focus Fire")) return false;

                return Spell.CanCast(spellName);
            }
        }

        public class FocusFire : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Focus Fire";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Widow Venom
        public class NeedToWidowVenom : Decorator
        {
            public NeedToWidowVenom(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Widow Venom";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.WidowVenom)) return false;
                if (!Target.CanDebuffTarget(spellName)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class WidowVenom : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Widow Venom";

                Spell.Cast(spellName);
                Utils.LagSleep();
                bool result = Target.IsDebuffOnTarget(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Pre Loot Movement
        public class NeedToPreLootMovement : Decorator
        {
            public NeedToPreLootMovement(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                bool okToMove = false;

                if (!Me.GotTarget) return false;
                if (Me.IsInInstance || Utils.IsBattleground) return false;
                if (!CLC.ResultOK(Settings.PreEmptiveLooting)) return false;
                if (Target.IsElite) return false;
                if (!Target.IsDistanceMoreThan(Settings.PreLootMinDistance)) return false;

                if (!Target.IsLowLevel && !Target.IsHealthPercentAbove(Settings.PreLootMaxHealth) || Target.IsLowLevel && !Target.IsHealthPercentAbove(60)) okToMove = true;

                if (!okToMove || Utils.Adds || CT.CurrentTargetGuid == Me.Guid && Target.IsDistanceLessThan(20)) return false;
                
                return true;
            }
        }

        public class PreLootMovement : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log("-Entering pre-loot movement", Utils.Colour("Green"));
                Movement.MoveTo(4);
                System.Threading.Thread.Sleep(1500);
                while (Me.IsMoving)
                {
                    Movement.MoveTo(4);
                    System.Threading.Thread.Sleep(250);
                    if (Spell.CanCast("Kill Shot")) Spell.Cast("Kill Shot");
                    if (Self.IsPowerPercentAbove(40) && Spell.CanCast("Arcane Shot")) Spell.Cast("Arcane Shot");
                    if (Utils.Adds) break;
                    if (Target.IsLowLevel) break;
                }

                bool result = true;
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Trap Launcher
        public class NeedToTrapLauncher : Decorator
        {
            public NeedToTrapLauncher(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Trap Launcher";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Settings.TrapLauncher.Contains("never")) return false;
                if (Spell.IsOnCooldown(Settings.TrapLauncher)) return false;
                if (Target.IsLowLevel) return false;
                if (Target.IsDistanceLessThan(10)) return false;
                if (!Target.IsHealthPercentAbove(35) && !Target.IsElite) return false;
                if (CT.IsMoving) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class TrapLauncher : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Trap Launcher";
                WoWPoint trapLocation = Movement.PointFromTarget(1);
                bool result = Spell.Cast(spellName);

                Utils.LagSleep();
                Lua.DoString(String.Format("CastSpellByName(\"{0}\")", Settings.TrapLauncher));
                LegacySpellManager.ClickRemoteLocation(trapLocation);
                Utils.Log(string.Format("Using {0} with Trap Launcher", Settings.TrapLauncher));

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Arcane Shot
        public class NeedToArcaneShot : Decorator
        {
            public NeedToArcaneShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Arcane Shot";

                if (!CLC.ResultOK(Settings.ArcaneShot)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.ArcaneShot)) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;
                if (Self.IsBuffOnMe(53224) && !Self.IsBuffOnMeLUA("Improved Steady Shot") && !Utils.IsBattleground) return false;
                
                return (Spell.CanCast(spellName));
            }
        }

        public class ArcaneShot : Action
        {
            protected override RunStatus Run(object context)
            {
                Target.Face();
                const string spellName = "Arcane Shot";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Backup
        public class NeedToBackup : Decorator
        {
            public NeedToBackup(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;
                if (Target.IsDistanceMoreThan(5.5)) return false;
                if (!Utils.CombatCheckOk("", false)) return false;
                if (CT.CurrentTargetGuid == Me.Guid) return false;
                if (!Target.IsLowLevel && !Target.IsElite && !Target.IsHealthPercentAbove(45)) return false;

                if (CT.CurrentTargetGuid == Me.Pet.Guid || CT.CurrentTargetGuid != Me.Guid && Me.IsInInstance) return true;

                return false;
            }
        }

        public class Backup : Action
        {
            protected override RunStatus Run(object context)
            {
                Movement.MoveTo(Movement.FindSafeLocation(7, true));
                System.Threading.Thread.Sleep(250);
                while (Me.IsMoving) { System.Threading.Thread.Sleep(250); }

                if (Me.IsMoving) Movement.StopMoving();
                Target.Face();

                return RunStatus.Success;
            }
        }
        #endregion

        #region Steady / Cobra Shot
        public class NeedToFocusShot : Decorator
        {
            public NeedToFocusShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = Spell.IsKnown("Cobra Shot") ? "Cobra Shot" : "Steady Shot";

                if (Self.IsBuffOnMe(53224)) spellName = "Steady Shot";      // Improved Steady Shot
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsPowerAbove(95)) return false;
                if (!Target.IsHealthPercentAbove(35) && !Target.IsElite) return false;
                if (CT.CurrentTargetGuid == Me.Guid && Target.IsDistanceLessThan(25)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class FocusShot : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = Spell.IsKnown("Cobra Shot") ? "Cobra Shot" : "Steady Shot";

                if (Self.IsBuffOnMe(53224)) spellName = "Steady Shot";      // Improved Steady Shot
                bool result = Spell.Cast(spellName);

                Utils.WaitWhileCasting();
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Raptor Strike
        public class NeedToRaptorStrike : Decorator
        {
            public NeedToRaptorStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Raptor Strike";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class RaptorStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Raptor Strike";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Hunters Mark
        public class NeedToHuntersMark : Decorator
        {
            public NeedToHuntersMark(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Hunter's Mark";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.HuntersMark)) return false;
                if (Target.IsDebuffOnTarget(spellName)) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class HuntersMark : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Hunter's Mark";

                Spell.Cast(spellName);
                Utils.LagSleep();

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Aimed Shot

        public class NeedToAimedShot : Decorator
        {
            public NeedToAimedShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Aimed Shot";

                if (!CLC.ResultOK(Settings.AimedShot)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Target.IsHealthPercentAbove(35)) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class AimedShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Aimed Shot";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Silencing Shot
        public class NeedToSilencingShot : Decorator
        {
            public NeedToSilencingShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Silencing Shot";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Target.IsCasting) return false;
                if (Target.IsLowLevel) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class SilencingShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Silencing Shot";
                bool result = Spell.Cast(spellName);
                
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region MultiShot

        public class NeedToMultiShot : Decorator
        {
            public NeedToMultiShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Multi-Shot";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Utils.Adds && !Me.IsInInstance) return false;
                //if (Me.IsInInstance && !Self.IsPowerAbove(45)) return false;
                if (!Utils.AllMobsAttackingPetOrOther) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class MultiShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Multi-Shot";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Kill Shot

        public class NeedToKillShot : Decorator
        {
            public NeedToKillShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Kill Shot";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsHealthPercentAbove(20)) return false;
                //if (!Self.IsPowerAbove(50)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class KillShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Kill Shot";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Explosive Shot

        public class NeedToExplosiveShot : Decorator
        {
            public NeedToExplosiveShot(Composite child)
                : base(child)
            {
            }

            protected override bool CanRun(object context)
            {
                const string spellName = "Explosive Shot";

                if (!CLC.ResultOK(Settings.ExplosiveShot)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                //if (!Self.IsPowerAbove(Settings.FocusShot)) return false;
                if (!Target.IsHealthPercentAbove(20)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class ExplosiveShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Explosive Shot";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Rapid Fire

        public class NeedToRapidFire : Decorator
        {
            public NeedToRapidFire(Composite child)
                : base(child)
            {
            }

            protected override bool CanRun(object context)
            {
                const string spellName = "Rapid Fire";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.RapidFire)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class RapidFire : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Rapid Fire";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Bestial Wrath

        public class NeedToBestialWrath : Decorator
        {
            public NeedToBestialWrath(Composite child)
                : base(child)
            {
            }

            protected override bool CanRun(object context)
            {
                const string spellName = "Bestial Wrath";
                if (!Me.Pet.GotTarget) return false;
                if (!Me.GotAlivePet) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.BestialWrath)) return false;
                if (!Target.IsHealthPercentAbove(35)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class BestialWrath : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Bestial Wrath";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Intimidation

        public class NeedToIntimidation : Decorator
        {
            public NeedToIntimidation(Composite child)
                : base(child)
            {
            }

            protected override bool CanRun(object context)
            {
                const string spellName = "Intimidation";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Me.GotAlivePet) return false;
                if (!CLC.ResultOK(Settings.Intimidation)) return false;
                //if (!Target.IsCasting) return false;
                //if (!Self.IsPowerAbove(50)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class Intimidation : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Intimidation";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Feign Death

        public class NeedToFeignDeath : Decorator
        {
            public NeedToFeignDeath(Composite child)
                : base(child)
            {
            }

            protected override bool CanRun(object context)
            {
                const string spellName = "Feign Death";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Settings.FeignDeath.Contains("never")) return false;

                if (Settings.FeignDeath.Contains("on aggro") && !Utils.AllMobsAttackingPetOrOther && Spell.CanCast(spellName))
                {
                    if ((!Target.IsHealthPercentAbove(30) && !Target.IsElite && !Utils.Adds) || (Me.GotAlivePet && Me.GotTarget && CT.GetThreatInfoFor(Me.Pet).RawPercent < 5) || !Settings.FeignDeath.Contains("low health")) return false;
                    if (Target.IsCaster || Target.IsDistanceLessThan(Target.InteractRange + 10)) return true;
                }
                
                if ((Settings.FeignDeath.Contains("low health") && Self.IsHealthPercentAbove(Settings.FeignDeathHealth)) || !Me.GotAlivePet || Utils.IsBattleground || Utils.AllMobsAttackingPetOrOther) return false;

                return Spell.CanCast(spellName);
            }
        }

        public class FeignDeath : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Feign Death";
                bool result = Spell.Cast(spellName);

                System.Threading.Thread.Sleep(1000);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Concussive Shot
        public class NeedToConcussiveShot : Decorator
        {
            public NeedToConcussiveShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Concussive Shot";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Target.CanDebuffTarget(spellName)) return false;
                if (!Spell.CanCast(spellName)) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;

                if (Target.IsDistanceMoreThan(15) && !Target.IsCaster && CT.CurrentTargetGuid == Me.Guid && CT.IsMoving && Target.IsHealthPercentAbove(30)) return true;

                return false;
            }
        }

        public class ConcussiveShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Concussive Shot";

                Spell.Cast(spellName);
                Utils.LagSleep();

                bool result = Target.IsDebuffOnTarget(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Kill Command

        public class NeedToKillCommand : Decorator
        {
            public NeedToKillCommand(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Kill Command";

                if (!CLC.ResultOK(Settings.KillCommand)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Me.GotAlivePet) return false;
                if (!Me.Pet.GotTarget) return false;
                if (Me.Pet.CurrentTarget.Location.Distance(Me.Pet.Location) > Target.InteractRange) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class KillCommand : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Kill Command";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Chimera Shot

        public class NeedToChimeraShot : Decorator
        {
            public NeedToChimeraShot(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Chimera Shot";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.ChimeraShot)) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class ChimeraShot : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Chimera Shot";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Black Arrow

        public class NeedToBlackArrow : Decorator
        {
            public NeedToBlackArrow(Composite child) : base(child) { } 

            protected override bool CanRun(object context)
            {
                const string spellName = "Black Arrow";

                if (!CLC.ResultOK(Settings.BlackArrow)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDebuffOnTarget(spellName)) return false;
                if (!Self.IsPowerAbove(Settings.FocusShot)) return false;
                
                return (Spell.CanCast(spellName));
            }
        }

        public class BlackArrow : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Black Arrow";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Serpent Sting

        public class NeedToSerpentSting : Decorator
        {
            public NeedToSerpentSting(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Serpent Sting";

                if (!CLC.ResultOK(Settings.SerpentSting)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDebuffOnTarget(spellName)) return false;
                if (!Self.IsPowerPercentAbove(Settings.FocusShot)) return false;
                if (!Target.IsHealthPercentAbove(25) && !Target.IsElite) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class SerpentSting : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Serpent Sting";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Aspects
        public class NeedToAspects : Decorator
        {
            public NeedToAspects(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Utils.CombatCheckOk("", false)) return false;

                return ClassHelpers.Hunter.Aspect.NeedToSwapAspect;
            }
        }

        public class Aspects : Action
        {
            protected override RunStatus Run(object context)
            {
                ClassHelpers.Hunter.Aspect.AutoAspect();
                return RunStatus.Success;
            }
        }
        #endregion

        #region Traps In Combat
        public class NeedToTrapsInCombat : Decorator
        {
            public NeedToTrapsInCombat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = Settings.TrapInCombat;

                if (spellName.Contains("never")) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsLowLevel) return false;
                if (!Target.IsHealthPercentAbove(35) && !Target.IsElite) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange + 1)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class TrapsInCombat : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = Settings.TrapInCombat;
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Traps on Adds
        public class NeedToTrapsOnAdds : Decorator
        {
            public NeedToTrapsOnAdds(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = Settings.TrapOnAdds;

                if (spellName.Contains("never")) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsLowLevel) return false;
                if (!Utils.Adds) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class TrapsOnAdds : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = Settings.TrapOnAdds;
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mob Attacking Me
        public class NeedToMobAttackingMe : Decorator
        {
            public NeedToMobAttackingMe(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Me.Level < 10) return false;
                return (Utils.Adds);
            }
        }

        public class MobAttackingMe : Action
        {
            protected override RunStatus Run(object context)
            {
                ClassHelpers.Hunter.Pet.DefendMe();
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Fervor
        public class NeedToFervor : Decorator
        {
            public NeedToFervor(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Fervor";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Me.IsInInstance && !Utils.Adds) return false;
                if (Self.IsPowerAbove(30)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Fervor : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Fervor";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Disengage

        public class NeedToDisengage : Decorator
        {
            public NeedToDisengage(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Disengage";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Utils.IsBattleground) return false;
                if (!Target.IsDistanceLessThan(6)) return false;
                if (!CLC.ResultOK(Settings.Disengage)) return false;
                if (Target.IsTargetingMe) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Disengage : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Disengage";
                int wcFocus = Spell.PowerCost("Wing Clip");
                int deFocus = Spell.PowerCost("Disengage");

                if (Self.IsPowerAbove(wcFocus + deFocus) && Spell.CanCast("Wing Clip"))
                {
                    Spell.Cast("Wing Clip");
                    System.Threading.Thread.Sleep(1000);
                }

                bool result = Spell.Cast(spellName);
                Utils.LagSleep();
                while (Me.IsMoving) { System.Threading.Thread.Sleep(150); }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Disengage - Battleground Only

        public class NeedToDisengageBGOnly : Decorator
        {
            public NeedToDisengageBGOnly(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Disengage";
                int wcFocus = Spell.PowerCost("Wing Clip");
                int deFocus = Spell.PowerCost("Disengage");

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Spell.CanCast("Wing Clip")) return false;
                if (!Utils.IsBattleground) return false;
                if (!Target.IsDistanceLessThan(6)) return false;
                if (!Self.IsPowerAbove(wcFocus + deFocus)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class DisengageBGOnly : Action
        {
            protected override RunStatus Run(object context)
            {
                Spell.Cast("Wing Clip");
                System.Threading.Thread.Sleep(1000);
                if (!Target.IsDebuffOnTarget("Wing Clip")) return RunStatus.Failure;

                const string spellName = "Disengage";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Steady Shot Survival spec only
        public class NeedToSteadyShotSurvival : Decorator
        {
            public NeedToSteadyShotSurvival(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Steady Shot";
                bool improvedSteadyShot = false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsBuffOnMeLUA("Improved Steady Shot")) return false;
                foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
                {
                    if (aura.Key.Contains("Improved Steady Shot") || aura.Value.SpellId == 53224)
                    {
                        improvedSteadyShot = true;
                        break;
                    }
                }
                if (!improvedSteadyShot) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class SteadyShotSurvival : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Steady Shot";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Fire! Proc
        public class NeedToFireProc : Decorator
        {
            public NeedToFireProc(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Aimed Shot";

                if (!Self.IsBuffOnMe(82926)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FireProc : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Aimed Shot";
                bool result = Spell.Cast(dpsSpell);
                Utils.Log("-Fire! Proc",Utils.Colour("Red"));
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(dpsSpell);
                //bool result = Self.IsBuffOnMe(dpsSpell););

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion


    }
}