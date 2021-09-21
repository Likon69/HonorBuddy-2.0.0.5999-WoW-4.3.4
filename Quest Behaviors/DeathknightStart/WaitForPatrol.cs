// Behavior originally contributed by HighVoltz.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using CommonBehaviors.Actions;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors.DeathknightStart.WaitForPatrol
{
    /// <summary>
    /// Waits at a safe location until an NPC is X distance way from you.. Useful for the quest in dk starter area where you have to ninja a horse but have to stay away from the stable master
    /// ##Syntax##
    /// MobId: This is the ID of the bad boy you want to stay clear of 
    /// QuestId: (Optional) The Quest to perform this behavior on
    /// Distance: The Distance to stay away from 
    /// X,Y,Z: The Safe Location location where you want wait at.
    /// </summary>
    public class WaitForPatrol : CustomForcedBehavior
    {
        public WaitForPatrol(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                AvoidMobId = GetAttributeAsNullable<int>("AvoidMobId", false, ConstrainAs.MobId, new[] { "MobId" }) ?? 0;
                MoveToMobId = GetAttributeAsNullable<int>("MoveToMobId", false, ConstrainAs.MobId, new[] { "MoveToMobID" }) ?? 0;
                AvoidDistance = GetAttributeAsNullable<double>("AvoidDistance", true, ConstrainAs.Range, new[] { "Distance" }) ?? 0;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                SafespotLocation = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                MoveToMob = GetAttributeAsNullable<bool>("MoveToMob", false, null, new[] { "MoveMob" }) ?? false;

                // Internal use --
                MobName = (AvoidNpc != null) ? AvoidNpc.Name : string.Format("MobId({0})", AvoidMobId);
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
        public int AvoidMobId { get; private set; }
        public int MoveToMobId { get; private set; }
        public double AvoidDistance { get; private set; }  // Distance to stay away from 
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint SafespotLocation { get; private set; }  // Safespot
        public bool MoveToMob { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private WoWUnit AvoidNpc
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWUnit>(true)
                                     .Where(o => o.Entry == AvoidMobId)
                                     .OrderBy(o => o.Distance)
                                     .FirstOrDefault());
            }
        }

        private WoWUnit MoveToNpc
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWUnit>(true)
                                     .Where(o => o.Entry == MoveToMobId)
                                     .OrderBy(o => o.Distance)
                                     .FirstOrDefault());
            }
        }


        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        private string MobName { get; set; }
        private static readonly TimeSpan ThrottleUserStatusUpdate = TimeSpan.FromSeconds(1);

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: WaitForPatrol.cs 220 2012-03-23 04:02:06Z natfoth $"); } }
        public override string SubversionRevision { get { return ("$Revision: 220 $"); } }


        ~WaitForPatrol()
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


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return (_root ?? (_root =
                new PrioritySelector(

                    new Decorator(ret => MoveToMob,
                        new PrioritySelector(

                         new Decorator(ret => MoveToNpc != null && MoveToNpc.Distance > 5,
                                new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Moving To Location - X: " + MoveToNpc.Location.X + " Y: " + MoveToNpc.Location.Y),
                                        new Action(ret => WoWMovement.ClickToMove(MoveToNpc.Location))
                                    )
                                ),

                         new Decorator(ret => MoveToNpc != null && MoveToNpc.Distance <= 5 && Me.Mounted,
                                new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Dismounting"),
                                        new Action(ret => Flightor.MountHelper.Dismount())
                                    )
                                ),

                         new Decorator(ret => MoveToNpc == null,
                                new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Waiting for Mob to Appear")
                                    )
                                ))),

                    new Decorator(avoidNpc => !MoveToMob,
                        new PrioritySelector(

                        // Move to our 'safe' spot, if needed...
                        new Decorator(c => Me.Location.Distance(SafespotLocation) > Navigator.PathPrecision,
                            new PrioritySelector(
                                new Decorator(c => (!Me.Mounted &&
                                                Mount.ShouldMount(SafespotLocation) &&
                                                Mount.MountUp(() => true, () => SafespotLocation)),
                                new ActionAlwaysSucceed()),

                            new CompositeThrottle(ThrottleUserStatusUpdate,
                                new Action(delegate
                                {
                                    TreeRoot.StatusText = string.Format("Moving to safe spot {0:F1} yards away.",
                                                    Me.Location.Distance(SafespotLocation));
                                    return (RunStatus.Failure);     // Fall through
                                })),

                            new Action(c => Navigator.MoveTo(SafespotLocation))
                            )),

                        // Wait for mob to move the prescribed distance away...
                        new Decorator(avoidNpc => (((WoWUnit)avoidNpc) != null)
                                                && (((WoWUnit)avoidNpc).Distance <= AvoidDistance),
                        new CompositeThrottle(ThrottleUserStatusUpdate,
                            new Sequence(
                        // Set focus to the mob we're watching (for user-feedback purposes)...
                                new DecoratorContinue(avoidNpc => (Me.FocusedUnitGuid != ((WoWUnit)avoidNpc).Guid),
                                    new Action(avoidNpc =>
                                    {
                                        Me.SetFocus(((WoWUnit)avoidNpc).Guid);
                                        return (RunStatus.Failure);         // fall through
                                    })),

                                new DecoratorContinue(avoidNpc => !Me.IsSafelyFacing((WoWUnit)avoidNpc),
                                    new Action(avoidNpc =>
                                    {
                                        ((WoWUnit)avoidNpc).Face();
                                        return (RunStatus.Failure);         // fall through
                                    })),

                                new Action(avoidNpc =>
                                {
                                    TreeRoot.StatusText = string.Format(
                                                            "Waiting on '{0}' (at {1:F1} yards) to move {2:F1} yards away",
                                                            ((WoWUnit)avoidNpc).Name,
                                                            ((WoWUnit)avoidNpc).Distance,
                                                            AvoidDistance);
                                })
                                ))),

                    // We're at safe spot, and mob is prescribed distance away.  We're done...
                    new Decorator(avoidNpc => (((WoWUnit)avoidNpc) == null)
                                                || (((WoWUnit)avoidNpc).Distance > AvoidDistance),
                        new Action(avoidNpc =>
                        {
                            Me.SetFocus(0);
                            TreeRoot.StatusText = string.Format(
                                                    "'{0}' is {1} (needed {2:F1} yards).  Behavior done.",
                                                    MobName,
                                                    ((avoidNpc != null)
                                                     ? string.Format("{0:F1} yards away", ((WoWUnit)avoidNpc).Distance)
                                                     : "clear"),
                                                    AvoidDistance);
                            _isBehaviorDone = true;
                        }))
                    )))));
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
            {
                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");

                LogMessage("info", "Moving to safe spot {0} until '{1}' moves {2} yards away.",
                            SafespotLocation,
                            MobName,
                            AvoidDistance);
            }
        }

        #endregion
    }


    public class CompositeThrottle : DecoratorContinue
    {
        public CompositeThrottle(TimeSpan throttleTime,
                                 Composite composite)
            : base(composite)
        {
            _hasRunOnce = false;
            _throttle = new Stopwatch();
            _throttleTime = throttleTime;

            _throttle.Reset();
            _throttle.Start();
        }


        protected override bool CanRun(object context)
        {
            if (_hasRunOnce && (_throttle.Elapsed < _throttleTime))
            { return (false); }

            _hasRunOnce = true;
            _throttle.Reset();
            _throttle.Start();

            return (true);
        }

        private bool _hasRunOnce;
        private readonly Stopwatch _throttle;
        private readonly TimeSpan _throttleTime;
    }
}
