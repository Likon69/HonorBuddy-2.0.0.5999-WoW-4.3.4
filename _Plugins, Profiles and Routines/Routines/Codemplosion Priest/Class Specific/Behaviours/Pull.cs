using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Hera
{
    public partial class Codemplosion
    {
        #region Pull
        private Composite _pullBehavior;
        public override Composite PullBehavior
        {
            get { if (_pullBehavior == null) { Utils.Log("Creating 'Pull' behavior"); _pullBehavior = CreatePullBehavior(); }  return _pullBehavior; }
        }

        private PrioritySelector CreatePullBehavior()
        {
            return new PrioritySelector(

                // If we can't navigate to the target blacklist it.
                new NeedToNavigatePath(new NavigatePath()),

                // Check if the target is suitable for pulling, if not blacklist it
                new NeedToBlacklistPullTarget(new BlacklistPullTarget()),

                // Check pull timers and blacklist bad pulls where required
                new NeedToCheckPullTimer(new BlacklistPullTarget()),

                // Auto Attack During Pull
                new NeedToAutoAttackPull(new AutoAttackPull()),

                // Shadowform
                new NeedToShadowform(new Shadowform()),

                // Face Target Pull
                new NeedToFacePull(new FacePull()),

                // Shield Before Pull
                new NeedToShieldPull(new ShieldPull()),

                // *******************************************************

                // Pull Spell
                new NeedToPullSpell(new PullSpell()),

                // Smite Pull
                new NeedToSmitePull(new SmitePull()),

                // Move To
                new NeedToMoveTo(new MoveTo()),

                // Update ObjectManager
                new Action(ret=>ObjectManager.Update())

                );
        }
        #endregion

        #region Pull Timer / Timeout
        public class NeedToCheckPullTimer : Decorator
        {
            public NeedToCheckPullTimer(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;

                return Target.PullTimerExpired;
            }
        }

        public class CheckPullTimer : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("Unable to pull {0}, blacklisting and finding another target.", Me.CurrentTarget.Name), System.Drawing.Color.FromName("Red"));
                Target.BlackList(120);
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
                if (Utils.IsBattleground) return false;
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;
                if (Target.IsElite) return false;
                if (CT.Name.Contains("Training Dummy")) return false;
                if (Me.IsInInstance) return false;
                
                return Target.CombatTimerExpired && Target.IsHealthPercentAbove(98);
            }
        }

        public class CheckCombatTimer : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("Combat with {0} is bugged, blacklisting and finding another target.", Me.CurrentTarget.Name), System.Drawing.Color.FromName("Red"));
                Target.BlackList(60);
                Utils.LagSleep();

                return RunStatus.Success;
            }
        }
        #endregion


        #region Behaviours

        #region Auto Attack During Pull
        public class NeedToAutoAttackPull : Decorator
        {
            public NeedToAutoAttackPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                if (Target.IsWithinInteractRange && !Me.IsAutoAttacking) return true;

                return false;
            }
        }

        public class AutoAttackPull : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.AutoAttack(true);

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Navigate Path
        public class NeedToNavigatePath : Decorator
        {
            public NeedToNavigatePath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                
                return !Navigator.CanNavigateFully(Me.Location,CT.Location,20);
            }
        }

        public class NavigatePath : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log("Can not navigate to target's location. Blacklisting",Utils.Colour("Red"));
                Target.BlackList(Utils.IsBattleground ? 10 : 30);

                return RunStatus.Success;
            }
        }
        #endregion

        #region Pull Spell
        public class NeedToPullSpell: Decorator
        {
            public NeedToPullSpell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.PullSpell;

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Settings.LazyRaider.Contains("always"))
                {
                    if (Target.IsDistanceMoreThan(Spell.MaxDistance(Settings.PullSpell)))
                    {
                        Movement.MoveTo((float) Spell.MaxDistance(Settings.PullSpell) - 5);
                        System.Threading.Thread.Sleep(1500);
                        return false;
                    }


                    if (Utils.IsInLineOfSight(CT.Location) && Me.IsMoving)
                    {
                        Movement.StopMoving();
                        Utils.LagSleep();
                    }
                }
                if (Me.IsMoving) return false;

                if (Spell.IsKnown(dpsSpell) && Spell.IsOnCooldown(dpsSpell)) return false;
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PullSpell : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.PullSpell;

                Target.Face();
                Utils.LagSleep();

                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                Utils.WaitWhileCasting();

                if (dpsSpell == "Vampiric Touch") Timers.Reset("VampiricTouch");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Smite Pull
        public class NeedToSmitePull : Decorator
        {
            public NeedToSmitePull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Smite";

                if (Spell.IsKnown(Settings.PullSpell) && !Spell.IsOnCooldown(Settings.PullSpell)) return false;        // If you can cast the selected pull spell bail out here
                if (Spell.CanCast(Settings.PullSpell) && !Spell.IsOnCooldown(Settings.PullSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.IsMoving) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SmitePull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Smite";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shield Pull
        public class NeedToShieldPull : Decorator
        {
            public NeedToShieldPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Power Word: Shield";

                if (!CLC.ResultOK(Settings.PWSBeforePull)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(6788, Self.AuraCheck.AllAuras)) return false;
                if (Self.IsBuffOnMe("Power Word: Shield")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShieldPull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Power Word: Shield";
                
                Spell.Cast(dpsSpell,Me);
                Utils.LagSleep();

                bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Face Pull
        public class NeedToFacePull : Decorator
        {
            public NeedToFacePull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {

                if (Settings.LazyRaider.Contains("always")) return false;
                return (!Me.IsMoving && Me.GotTarget);
            }
        }

        public class FacePull : Action
        {
            protected override RunStatus Run(object context)
            {
                Target.Face();
                return RunStatus.Failure;
            }
        }
        #endregion

        #endregion


    }
}