using System.Threading;
using TreeSharp;

namespace Hera
{
    public partial class Codemplosion
    {
        #region Pull
        private Composite _pullBehavior;
        public override Composite PullBehavior { get { if (_pullBehavior == null) { Utils.Log("Creating 'Pull' behavior"); _pullBehavior = CreatePullBehavior(); } return _pullBehavior; } }

        private PrioritySelector CreatePullBehavior()
        {
            return new PrioritySelector(
                // If we can't reach the target blacklist it.
                new Decorator(ret => !Target.CanGenerateNavPath, new Action(ret => Target.BlackList(1000))),

                // Check if the target is suitable for pulling, if not blacklist it
                new NeedToBlacklistPullTarget(new BlacklistPullTarget()),

                // Check pull timers and blacklist bad pulls where required
                new NeedToCheckPullTimer(new BlacklistPullTarget()),

                // Auto Attack - turn it off for pulling
                new Decorator(ret => Me.IsAutoAttacking, new Action(ret => Utils.AutoAttack(false))),

                // Hunters Mark
                //new NeedToHuntersMark(new HuntersMark()),

                // Move to attack range
                new Decorator(ret => Movement.NeedToCheck(), new Action(ret => Movement.DistanceCheck(Movement.MaximumDistance, Movement.MinimumDistance))),

                // Misdirection
                new NeedToMisdirection(new Misdirection()),

                // Pet Attack Pull
                new NeedToPetAttackPull(new PetAttackPull()),

                // Aspects
                new NeedToAspects(new Aspects()),

                
                // *******************************************************

                // Face Target Pull
                new NeedToFaceTargetPull(new FaceTargetPull()),

                // Pull Spell
                new NeedToPullSpell(new PullSpell()),

                // Aimed Shot Pull
                //new NeedToAimedShotPull(new AimedShotPull()),

                // Steady Shot Pull
                new NeedToSteadyShotPull(new SteadyShotPull()),

                // Concussive Shot
                new NeedToConcussiveShotPull(new ConcussiveShotPull()),

                // Arcane Shot
                new NeedToArcaneShotPull(new ArcaneShotPull()),

                // Finally just move to the target.
                new Action(ret => Movement.DistanceCheck(Movement.MaximumDistance, Movement.MinimumDistance))
                );
        }
        #endregion

        #region Pull Timer / Timeout
        public class NeedToCheckPullTimer : Decorator
        {
            public NeedToCheckPullTimer(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                return Target.PullTimerExpired;
            }
        }

        public class CheckPullTimer : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("Unable to pull {0}, blacklisting and finding another target.", Me.CurrentTarget.Name), System.Drawing.Color.FromName("Red"));
                Target.BlackList(1200);
                Me.ClearTarget();

                return RunStatus.Success;
            }
        }
        #endregion

        #region Combat Timer / Timeout
        public class NeedToCheckCombatTimer : Decorator
        {
            public NeedToCheckCombatTimer(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;
                if (Target.IsElite) return false;                           // Elites are going to take more of a beating so ignore this check
                if (CT.Name.Contains("Training")) return false;             // Don't time out on training dummies


                return Target.CombatTimerExpired && Target.IsHealthPercentAbove(95);
            }
        }

        public class CheckCombatTimer : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("Combat with {0} is bugged, blacklisting and finding another target.", Me.CurrentTarget.Name), System.Drawing.Color.FromName("Red"));
                Target.BlackList(1200);
                Utils.LagSleep();

                return RunStatus.Success;
            }
        }
        #endregion


        // ******************************************************************************************
        // Pull spells are here

        #region Pull Spell
        public class NeedToPullSpell : Decorator
        {
            public NeedToPullSpell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.PullSpell;

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                
                return Spell.CanCast(dpsSpell);
            }
        }

        public class PullSpell : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.PullSpell;
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Concussive Shot Pull
        public class NeedToConcussiveShotPull : Decorator
        {
            public NeedToConcussiveShotPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Concussive Shot";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Target.CanDebuffTarget(spellName)) return false;
                if (Me.IsInInstance) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class ConcussiveShotPull : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Concussive Shot";
                Spell.Cast(spellName);
                Utils.LagSleep();

                bool result = Target.IsDebuffOnTarget(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Arcane Shot PULL
        public class NeedToArcaneShotPull : Decorator
        {
            public NeedToArcaneShotPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Arcane Shot";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Self.IsPowerPercentAbove(45)) return false;
                if (Spell.IsKnown("Concussive Shot") && !Spell.IsOnCooldown("Concussive Shot")) return false;
                //if (!CLC.ResultOK(Settings.ArcaneShot)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class ArcaneShotPull : Action
        {
            protected override RunStatus Run(object context)
            {
                Target.Face();
                string spellName = "Arcane Shot";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Steady Shot PULL
        public class NeedToSteadyShotPull : Decorator
        {
            public NeedToSteadyShotPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Steady Shot";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsPowerPercentAbove(90)) return false;
                if (Spell.IsKnown("Concussive Shot") && !Spell.IsOnCooldown("Concussive Shot")) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class SteadyShotPull : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Steady Shot";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Aimed Shot Pull

        public class NeedToAimedShotPull : Decorator
        {
            public NeedToAimedShotPull(Composite child)
                : base(child)
            {
            }

            protected override bool CanRun(object context)
            {
                string spellName = "Aimed Shot";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Self.IsPowerPercentAbove(99)) return false;


                return (Spell.CanCast(spellName));
            }
        }

        public class AimedShotPull : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Aimed Shot";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Pet Attack Pull
        public class NeedToPetAttackPull : Decorator
        {
            public NeedToPetAttackPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Utils.CombatCheckOk("", false)) return false;
                if (!Me.GotAlivePet) return false;
                if (Me.Pet.Combat) return false;
                if (Me.Pet.CurrentTarget != null) return false;
                if (Me.Level < 10) return false;

                return true;
            }
        }

        public class PetAttackPull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string huntersMark = "Hunter's Mark";

                ClassHelpers.Hunter.Pet.Attack();
                Thread.Sleep(500);
                Target.Face();

                if (Utils.IsBattleground || Me.IsInInstance || Target.IsLowLevel) return RunStatus.Failure;
                
                Thread.Sleep(500);
                
                if (CLC.ResultOK(Settings.HuntersMark) && !Target.IsDebuffOnTarget(huntersMark) && Spell.CanCast(huntersMark)) Spell.Cast(huntersMark);
                if (Target.IsDistanceMoreThan(38)) Movement.MoveTo(CT.Location);
                if (Me.IsMoving && Target.IsDistanceLessThan(35)) Movement.StopMoving();

                if (Settings.PetAttackDelay.Contains("never"))
                {
                    Thread.Sleep(500);
                    if (Self.IsBuffOnMe("Aspect of the Cheetah") && Spell.CanCast("Aspect of the Hawk"))
                    {
                        Spell.Cast("Aspect of the Hawk");
                        Utils.LagSleep();
                        while (Spell.IsGCD) Thread.Sleep(250);
                    }

                    // Cast pull spell
                    if (Spell.CanCast(Settings.PullSpell))
                    {
                        Spell.Cast(Settings.PullSpell);
                        Utils.LagSleep();
                        while (Spell.IsGCD) Thread.Sleep(250);
                    }
                    else if (Spell.CanCast("Concussive Shot"))
                    {
                        Spell.Cast("Concussive Shot");
                        Utils.LagSleep();
                        while (Spell.IsGCD) Thread.Sleep(250);
                    }
                    else if (Spell.CanCast("Arcane Shot"))
                    {
                        Spell.Cast("Arcane Shot");
                        Utils.LagSleep();
                        while (Spell.IsGCD) Thread.Sleep(250);
                    }

                    Utils.AutoAttack(true);
                    return RunStatus.Success;
                }

                // Pet Attack Delay
                while (!CT.Combat && !Me.Combat)
                {
                    if (CLC.ResultOK(Settings.HuntersMark) && !Target.IsDebuffOnTarget(huntersMark) && Spell.CanCast(huntersMark)) Spell.Cast(huntersMark);
                    Thread.Sleep(250);
                    if (Self.IsBuffOnMe("Aspect of the Cheetah") && Spell.CanCast("Aspect of the Hawk")) Spell.Cast("Aspect of the Hawk");

                    if (Target.IsDistanceMoreThan(38)) Movement.MoveTo(CT.Location);
                    if (Me.IsMoving && Target.IsDistanceLessThan(35)) Movement.StopMoving();
                }

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Misdirection
        public class NeedToMisdirection : Decorator
        {
            public NeedToMisdirection(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Misdirection";
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!CLC.ResultOK(Settings.Misdirection)) return false;
                if (Target.IsLowLevel) return false;
                if (!Me.GotAlivePet) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class Misdirection : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Misdirection";
                bool result = Spell.Cast(spellName, Me.Pet);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

    }
}