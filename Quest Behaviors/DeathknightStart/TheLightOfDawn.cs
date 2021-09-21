// Behavior originally contributed by Unknown.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Linq;

using CommonBehaviors.Actions;

using Styx.Logic.BehaviorTree;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class TheLightOfDawn : CustomForcedBehavior
    {
        public TheLightOfDawn(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
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
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private variables for internal state
        private readonly Helpers.WaitTimer _afkTimer = new Helpers.WaitTimer(TimeSpan.FromMinutes(2));
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private static WoWUnit HighWarlordDarion { get { return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).FirstOrDefault(u => u.Entry == 29173); } }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: TheLightOfDawn.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~TheLightOfDawn()
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


        private void AntiAfk()
        {
            if (!_afkTimer.IsFinished) return;
            WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(100));
            _afkTimer.Reset();
        }


        #region Overrides of CustomForcedBehavior

        private readonly Styx.Helpers.WaitTimer _waitTimer = new Styx.Helpers.WaitTimer(TimeSpan.FromMinutes(10));

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                    new Decorator(ret => !_waitTimer.IsFinished,
                        new Sequence(
                            new Action(ret => AntiAfk()),
                            new Action(ret => TreeRoot.StatusText = "Waiting for the story to end"),
                            new ActionAlwaysSucceed())
                            ),

                    new Decorator(ret => HighWarlordDarion != null && HighWarlordDarion.CanGossip,
                        new PrioritySelector(
                            new Decorator(ret => !HighWarlordDarion.WithinInteractRange,
                                new Sequence(
                                   new Action(ret => TreeRoot.StatusText = "Moving to High Warlord Darion"),
                                   new Action(ret => Navigator.MoveTo(HighWarlordDarion.Location)))),

                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Talking to High Warlord Darion"),
                                new DecoratorContinue(ret => Me.IsMoving,
                                    new Sequence(
                                        new Action(ret => WoWMovement.MoveStop()),
                                        new Action(ret => StyxWoW.SleepForLagDuration())
                                        )),

                                new Action(ret => HighWarlordDarion.Interact()),
                                new WaitContinue(5, ret => GossipFrame.Instance.IsVisible,
                                    new Sequence(
                                        new DecoratorContinue(ret => GossipFrame.Instance.GossipOptionEntries == null,
                                            new Action(ret => _waitTimer.Reset())
                                            ),

                                        new DecoratorContinue(ret => GossipFrame.Instance.GossipOptionEntries != null,
                                            new Sequence(
                                                new Action(ret => GossipFrame.Instance.SelectGossipOption(0)),
                                                new Action(ret => StyxWoW.SleepForLagDuration())
                                                )))
                                    )))),

                            new Action(ret => _waitTimer.Reset())
                    ));
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
                return (!UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
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

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
