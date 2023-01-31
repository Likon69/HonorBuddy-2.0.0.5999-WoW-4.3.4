using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Hera
{
    public partial class Fpsware
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


                // *******************************************************

                // Procs - The Art of War and Denounce. Use it or lose it. 
                new NeedToProcs(new Procs()),

                // Avengers Shield
                new NeedToAvengersShieldPull(new AvengersShieldPull()),

                // Holy Shock
                new NeedToHolyShock(new HolyShock()),

                // Judgement
                new NeedToJudgement(new Judgement()),
                
                // Hand Of Reckoning
                new NeedToHandOfReckoningPull(new HandOfReckoningPull()),

                // Move to target
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

        #region Move To
        public class NeedToMoveTo : Decorator
        {
            public NeedToMoveTo(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;
                if (!Target.IsHealthPercentAbove(90) && CT.Combat) return false;

                return Target.IsDistanceMoreThan(Target.InteractRange + 0.5f);
            }
        }

        public class MoveTo : Action
        {
            protected override RunStatus Run(object context)
            {
                /*
                if (CT.IsMoving)
                {
                    float distance = (Self.IsBuffOnMe(87173) ? 0.25f : -0.1f);
                    WoWPoint pointBehind = WoWMathHelper.CalculatePointBehind(CT.Location, CT.Rotation, distance);
                    Movement.MoveTo(pointBehind);
                }
                else if (!CT.IsMoving)
                 */
                {
                    Movement.MoveTo(Target.InteractRange * 0.9f); 
                }
            
                return RunStatus.Failure;
            }
        }
        #endregion


        #endregion


    }
}