using System;

using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace CLU.Helpers
{
    using System.Drawing;

    using global::CLU.GUI;

    using Action = TreeSharp.Action;

    public class Movement
    {
        /* putting all the Movement logic here */

        private static readonly Movement MovementInstance = new Movement();

        /// <summary>
        /// An instance of the Unit Class
        /// </summary>
        public static Movement Instance { get { return MovementInstance; } }

        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        /// <summary>
        ///  CLU Shortcut to our current target
        /// </summary>
        private static WoWUnit CurrentTarget
        {
            get
            {
                if (StyxWoW.Me.CurrentTarget != null)
                {
                    return StyxWoW.Me.CurrentTarget;
                }

                return null;
            }
        }

    public static Composite MovingFacingBehavior()  // TODO: Check if we have an obstacle in our way and clear it ALSO need a mounted CHECK!!.
    {
	return new Sequence(
			// No Target?
			// Stop Facing
			// Movement Enabled?
			// Aquire Target
			new DecoratorContinue(check => ObjectManager.Me.CurrentTarget == null, 
				new Sequence(
					new Action(ret => WoWMovement.StopFace()), 
					new Decorator(check => SettingsFile.Instance.AutoHandleMovement, 
						new Action(delegate {
									try
									{
                                        Logging.Write(Color.BlanchedAlmond, " [CLU Targeting] No Target. Aquiring");
										Unit.Instance.EnsureUnitTargeted.Target(); 
									}
									catch { }
								    }))
					)),
			// Are we Facing the target?
			// Is the Target in line of site?
			// Face Target
            new DecoratorContinue(check => !StyxWoW.Me.IsSafelyFacing(CurrentTarget, 45f) && CurrentTarget.InLineOfSight, 
				new Sequence(
					new Action(ret => WoWMovement.Face(CurrentTarget.Guid))
					)),
            // Target in Line of site?
            // Move to Location
            new DecoratorContinue(check => !CurrentTarget.InLineOfSight,
                new Sequence(
                new Action(ret => Logging.Write(Color.BlanchedAlmond, " [CLU Movement] Target not in LoS. Moving closer.")),
                new Action(ret => Navigator.MoveTo(CurrentTarget.Location)))),
			// Target is greater than CombatMinDistance?
			// Target is Moving?
			// We are not moving Forward?
			// Move Forward towards target
			new DecoratorContinue(check => 
										Spell.Instance.DistanceToTargetBoundingBox() >= CLU.Instance.ActiveRotation.CombatMinDistance &&
										CurrentTarget.IsMoving &&
										!Me.MovementInfo.MovingForward &&
                                        CurrentTarget.InLineOfSight, 
				new Sequence(
                    new Action(ret => Logging.Write(Color.BlanchedAlmond, " [CLU Movement] Too far away from moving target (T[{0}] >= P[{1}]). Moving forward.", Spell.Instance.DistanceToTargetBoundingBox(), CLU.Instance.ActiveRotation.CombatMinDistance)),
					new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.Forward))
					)),
            // Target is less than CombatMinDistance?
			// Target is Moving?
			// We are moving Forward
			// Stop Moving Forward
			new DecoratorContinue(check =>
                                        Spell.Instance.DistanceToTargetBoundingBox() < CLU.Instance.ActiveRotation.CombatMinDistance &&
										CurrentTarget.IsMoving &&
										Me.MovementInfo.MovingForward &&
                                        CurrentTarget.InLineOfSight, 
				new Sequence(
                    new Action(ret => Logging.Write(Color.BlanchedAlmond, " [CLU Movement] Too close to target (T[{0}] < P[{1}]). Movement Stopped.", Spell.Instance.DistanceToTargetBoundingBox(), CLU.Instance.ActiveRotation.CombatMinDistance)),
					new Action(ret => WoWMovement.MoveStop())
					)),
			// Target is not Moving?
			// Target is greater than CombatMaxDistance?
			// We are not Moving?
			// Move Forward
			new DecoratorContinue(check => 
										!CurrentTarget.IsMoving &&
										Spell.Instance.DistanceToTargetBoundingBox() >= CLU.Instance.ActiveRotation.CombatMaxDistance &&
                                        CurrentTarget.InLineOfSight, 
				new Sequence(
                    new Action(ret => Logging.Write(Color.BlanchedAlmond, " [CLU Movement] Too far away from non moving target (T[{0}] >= P[{1}]). Moving forward.", Spell.Instance.DistanceToTargetBoundingBox(), CLU.Instance.ActiveRotation.CombatMaxDistance)),
					new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.Forward, new TimeSpan(99, 99, 99)))
					)),
            // Target is less than CombatMaxDistance?
			// We are Moving?
			// We are moving Forward?
			// Stop Moving
			new DecoratorContinue(check =>
                                        Spell.Instance.DistanceToTargetBoundingBox() < CLU.Instance.ActiveRotation.CombatMaxDistance &&
										Me.IsMoving &&
										Me.MovementInfo.MovingForward &&
                                        CurrentTarget.InLineOfSight, 
				new Sequence(
                    new Action(ret => Logging.Write(Color.BlanchedAlmond, " [CLU Movement] Too close to target  (T[{0}] < P[{1}]). Movement Stopped", Spell.Instance.DistanceToTargetBoundingBox(), CLU.Instance.ActiveRotation.CombatMaxDistance)),
					new Action(ret => WoWMovement.MoveStop())
					))
		);
}

        private static WoWPoint CalculatePointBehindTarget()
        {
            return
                StyxWoW.Me.CurrentTarget.Location.RayCast(
                    StyxWoW.Me.CurrentTarget.Rotation + WoWMathHelper.DegreesToRadians(150), Spell.MeleeRange - 2f);
        }
    }
}
