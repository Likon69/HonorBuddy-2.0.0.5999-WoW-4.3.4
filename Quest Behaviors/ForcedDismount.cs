// Behavior originally contributed by Bobby53.
//
// DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_ForcedDismount
//
// QUICK DOX:
//      Dismounts a toon from a mount (or Druid flying form).  If flying, the behavior will
//      descend straight down to ground/water level before conducting the dismount.
//
//  Parameters (required, then optional--both listed alphabetically):
//      MaxDismountHeight [Default: 3.0]: The maximum height above ground/water at which
//          a toon is allowed to dismount.  If the toon is higher above the ground/water
//          than this, then the behavior will descend to this level before attempting
//          dismount.
//      QuestId [Default:none]:
//      QuestCompleteRequirement [Default:NotComplete]:
//      QuestInLogRequirement [Default:InLog]:
//              A full discussion of how the Quest* attributes operate is described in
//              http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors.
//
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

using CommonBehaviors.Actions;

using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors.ForcedDismount2
{
    public class ForcedDismount : CustomForcedBehavior
    {

        public ForcedDismount(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                MaxDismountHeight = GetAttributeAsNullable<double>("MaxDismountHeight", false, new ConstrainTo.Domain<double>(1.0, 75.0), null) ?? 8.0;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;

                GetAttributeAs<string>("QuestName", false, ConstrainAs.StringNonEmpty, null);     // (doc only - not used)
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller
        public double MaxDismountHeight { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private variables for internal state
        private Composite _behavior_root;
        private bool _isBehaviorDone;
        private bool _isDisposed;

        // Private properties
        private readonly TimeSpan Delay_WowClientDismount = TimeSpan.FromMilliseconds(1000);
        private readonly TimeSpan Delay_WowClientMovement = TimeSpan.FromMilliseconds(1000);
        private const string DruidFlightForm = "Flight Form";
        private const string DruidSwiftFlightForm = "Swift Flight Form";
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: ForcedDismount.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Rev: 217 $"); } }


        ~ForcedDismount()
        {
            Dispose(false);
        }


        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                // NOTE: we should call any Dispose() method for any managed or unmanaged
                // resource, if that resource provides a Dispose() method.

                // Clean up managed resources, if explicit disposal...
                if (isExplicitlyInitiatedDispose)
                {
                    // empty, for now
                }

                // Clean up unmanaged resources (if any) here...
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        #region Missing HBcore infrastructure

        // Like TreeSharp.Action, but will run the ACTIONDELEGATE just once.
        // When the delegate has not yet been run, ActionRunOnceContinue returns
        // the delegate's result.  If delegate as already been run, ActionRunOnceContinue
        // returns RunStatus.Success.
        public class ActionRunOnceContinue : Composite
        {
            public ActionRunOnceContinue(ActionDelegate actionDelegate)
            {
                _actionDelegate = actionDelegate;
            }

            public ActionRunOnceContinue(ActionSucceedDelegate actionSucceedDelegate)
            {
                _actionSucceedDelegate = actionSucceedDelegate;
            }

            protected override IEnumerable<RunStatus> Execute(object context)
            {
                if (!_hasBeenRun)
                {
                    _hasBeenRun = true;

                    if (_actionDelegate != null)
                    { yield return (_actionDelegate(context)); }
                    else if (_actionSucceedDelegate != null)
                    { _actionSucceedDelegate(context); }
                }

                yield return (RunStatus.Success);
            }

            private ActionDelegate _actionDelegate;
            private ActionSucceedDelegate _actionSucceedDelegate;
            private bool _hasBeenRun;
        }

        #endregion  // Missing HBcore infrastructure


        private bool IsReadyToDismount()
        {
            return (!Me.IsFlying
                    || Me.Location.IsOverGround(MaxDismountHeight)
                    || Me.Location.IsOverWater(MaxDismountHeight));
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return (_behavior_root ?? (_behavior_root =
                new PrioritySelector(

                    // If we're not mounted, nothing to do...
                    new Decorator(ret => !Me.Mounted,
                        new Action(delegate { _isBehaviorDone = true; })),


                    // If we're flying, we need to descend...
                    new Decorator(ret => !IsReadyToDismount(),
                            new Sequence(
                                new ActionRunOnceContinue(delegate { LogMessage("debug", "Descending before dismount"); }),
                                new Action(delegate { Navigator.PlayerMover.Move(WoWMovement.MovementDirection.Descend); }),
                                new WaitContinue(Delay_WowClientMovement, ret => IsReadyToDismount(), new ActionAlwaysSucceed())
                                )),


                    // Otherwise, dismount...
                    new Sequence(
                        new DecoratorContinue(ret => (Me.Auras.ContainsKey(DruidSwiftFlightForm)
                                                      || Me.Auras.ContainsKey(DruidFlightForm)),
                            new Action(delegate
                                {
                                    LogMessage("debug", "Cancelling Flight Form");
                                    Lua.DoString("CancelShapeshiftForm()");
                                })),

                        new DecoratorContinue(ret => Me.Mounted,
                            new Action(delegate
                                {
                                    LogMessage("debug", "Dismounting");
                                    Mount.Dismount();
                                })),

                        new WaitContinue(Delay_WowClientDismount, ret => !Me.Mounted, new ActionAlwaysSucceed()),
                        new Action(delegate { Navigator.PlayerMover.MoveStop(); })
                        )
                )));
        }




        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get
            {
                return (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
            }
        }


        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            { TreeRoot.GoalText = "Dismounting"; }
        }

        #endregion
    }


    public static class Chinajade_Extensions_WoWPoint
    {
        /// <summary>
        /// <para>Adds the provided X, Y, and Z offsets to WOWPOINT yielding a new WoWPoint.</para>
        /// <para>---</para>
        /// <para>The HBcore only provides a version of this that accepts 'float' values.
        /// This version accepts 'doubles', because it is inefficient to keep truncating
        /// data types (to float) that are provided by the Math and other libraries.</para>
        /// <para>'Double' performance is just as fast as 'Float'.  Internally, modern
        /// computer architectures calculate using maximum precision (i.e., many bits
        /// bigger than double), then truncate the result to fit.  The only benefit
        /// 'float' has over 'double' is storage space, which is negligible unless
        /// you've a database using billions of them.</para>
        /// </summary>
        /// <param name="wowPoint"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>new WoWPoint with adjusted coordinates</returns>
        public static WoWPoint Add(this WoWPoint wowPoint,
                                            double x,
                                            double y,
                                            double z)
        {
            return (new WoWPoint(wowPoint.X + x, wowPoint.Y + y, wowPoint.Z + z));
        }


        /// <summary>
        /// Returns true, if ground is within DISTANCE _below_ you.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <returns>true, if ground is within DISTANCE _below_ you.</returns>
        public static bool IsOverGround(this WoWPoint location,
                                             double distance)
        {
            WoWPoint hitLocation;

            return (GameWorld.TraceLine(location.Add(0.0, 0.0, 1.0),
                                        location.Add(0.0, 0.0, -distance),
                                        GameWorld.CGWorldFrameHitFlags.HitTestGroundAndStructures,
                                        out hitLocation));
        }


        /// <summary>
        /// Returns true, if water is within DISTANCE _below_ you.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <returns>true, if water is within DISTANCE _below_ you.</returns>
        public static bool IsOverWater(this WoWPoint location,
                                            double distance)
        {
            WoWPoint hitLocation;
            WoWPoint locationAbove = location.Add(0.0, 0.0, 1.0);
            WoWPoint locationBelow = location.Add(0.0, 0.0, -distance);

            return (GameWorld.TraceLine(locationAbove,
                                        locationBelow,
                                        GameWorld.CGWorldFrameHitFlags.HitTestLiquid,
                                        out hitLocation)
                    || GameWorld.TraceLine(locationAbove,
                                           locationBelow,
                                           GameWorld.CGWorldFrameHitFlags.HitTestLiquid2,
                                           out hitLocation));
        }
    }
}
