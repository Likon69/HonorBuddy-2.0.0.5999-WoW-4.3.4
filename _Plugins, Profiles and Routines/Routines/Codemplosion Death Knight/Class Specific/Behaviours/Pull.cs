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

                // Face Target Pull
                new NeedToFacePull(new FacePull()),


                // *******************************************************

                // Summon Pet
                 new NeedToSummonPet(new SummonPet()),

                // Presence
                new NeedToPresence(new Presence()),

                // Outbreak
                new NeedToOutbreak(new Outbreak()),

                // Death Coil Pull
                new NeedToDeathCoilPull(new DeathCoilPull()),

                // Death Grip Pull
                new NeedToDeathGripPull(new DeathGripPull()),

                // Icy Touch Pull
                new NeedToIcyTouchPull(new IcyTouchPull()),

                
                // Finally just move to the target.
                new Action(ret => Movement.DistanceCheck(Target.InteractRange, Target.InteractRange)),

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
                if (!Timers.Expired("PullSpellCast",5000)) return false;

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Settings.LazyRaider.Contains("always"))
                {
                    if (Target.IsDistanceMoreThan(Spell.MaxDistance(Settings.PullSpell)))
                    {
                        Movement.MoveTo((float) Spell.MaxDistance(Settings.PullSpell) - 2);
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

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Icy Touch Pull
        public class NeedToIcyTouchPull : Decorator
        {
            public NeedToIcyTouchPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Icy Touch";

                //if (Spell.IsKnown(Settings.PullSpell) && !Spell.IsOnCooldown(Settings.PullSpell)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell)) && Spell.CanCast("Death Grip")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
		
                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class IcyTouchPull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Icy Touch";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Death Grip Pull
        public class NeedToDeathGripPull : Decorator
        {
            public NeedToDeathGripPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Death Grip";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (Target.IsDistanceLessThan(Spell.MaxDistance("Icy Touch")) && Spell.CanCast("Icy Touch")) return false;
                if (Me.UnholyRuneCount < 1) return false;
		
                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class DeathGripPull : Action
        {
            protected override RunStatus Run(object context)
            {
                if (Me.IsMoving) WoWMovement.MoveStop();
                System.Threading.Thread.Sleep(500);
                Target.Face();

                const string dpsSpell = "Death Grip";
                bool result = Spell.Cast(dpsSpell);


                System.Threading.Thread.Sleep(1000);


                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Death Coil
        public class NeedToDeathCoilPull : Decorator
        {
            public NeedToDeathCoilPull(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Death Coil";

                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;
                if (Me.RunicPowerPercent < Settings.DeathCoil) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DeathCoilPull : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Death Coil";
                bool result = Spell.Cast(dpsSpell);
                

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