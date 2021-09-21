// Behavior originally contributed by Unknown.
//
// DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_UseGameObject
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class UseGameObject : CustomForcedBehavior
    {
        public UseGameObject(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                NumOfTimes = GetAttributeAsNullable<int>("NumOfTimes", false, ConstrainAs.RepeatCount, null) ?? 1;
                ObjectId = GetAttributeAsNullable<int>("ObjectId", true, ConstrainAs.ObjectId, null) ?? 0;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                WaitTime = GetAttributeAsNullable("WaitTime", false, ConstrainAs.Milliseconds, null) ?? 1500;
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
        public WoWPoint Location { get; private set; }
        public int ObjectId { get; private set; }
        public int NumOfTimes { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public int WaitTime { get; private set; }

        // Private variables for internal state
        private int _counter;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private WoWGameObject GameObject
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWGameObject>()
                                 .Where(u => u.Entry == ObjectId && !u.InUse && !u.IsDisabled)
                                 .OrderBy(u => u.Distance)
                                 .FirstOrDefault());
            }
        }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: UseGameObject.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~UseGameObject()
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
            return _root ?? (_root =
                new PrioritySelector(

                    // Move to the gameobject if it isn't null and we aren't withing interact range.
                    new Decorator(ret => GameObject != null && !GameObject.WithinInteractRange,
                        new Sequence(
                            new Action(ret => TreeRoot.StatusText = "Moving to \"" + GameObject.Name + "\" at location " + GameObject.Location),
                            new Action(ret => Navigator.MoveTo(GameObject.Location))
                            )
                        ),

                    // Interact etc. 
                    new Decorator(ret => GameObject != null && GameObject.WithinInteractRange,
                // Set the context to the gameobject
                        new Sequence(ret => GameObject,

                            new DecoratorContinue(ret => StyxWoW.Me.IsMoving,
                                new Sequence(
                                    new Action(ret => WoWMovement.MoveStop()),
                                    new WaitContinue(5, ret => !StyxWoW.Me.IsMoving,
                                        new Action(ret => StyxWoW.SleepForLagDuration()))
                                    )),

                            new Action(ret => LogMessage("info", "Using Object \"{0}\" {1}/{2} times",
                                                                 ((WoWGameObject)ret).Name, _counter + 1, NumOfTimes)),
                            new Action(ret => ((WoWGameObject)ret).Interact()),
                            new Action(ret => StyxWoW.SleepForLagDuration()),
                            new Action(ret => Thread.Sleep(WaitTime)),
                            new Action(delegate { _counter++; })
                        )),

                        new Decorator(ret => Location != WoWPoint.Empty,
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Moving to location " + Location.ToString()),
                                new Action(ret => Navigator.MoveTo(Location))
                                )
                            )
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
                return ((_counter >= NumOfTimes)     // normal completion
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

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
