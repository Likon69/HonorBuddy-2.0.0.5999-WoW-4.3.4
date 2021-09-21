// Behavior originally contributed by Nesox.
//
// DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_TalkToAndListenToStory
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Styx.Database;
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
    /// <summary>
    /// Allows you to interact and click thru the gossip of a number of npc's
    /// ##Syntax##
    /// QuestId: Id of the quest.
    /// NpcIds: Whitespace seperated list of id's of npc's to use.
    /// </summary>
    public class TalkToAndListenToStory : CustomForcedBehavior
    {
        public TalkToAndListenToStory(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                int[] tmpMobIds;

                LogMessage("warning", "*****\n"
                                        + "* THIS BEHAVIOR IS DEPRECATED, and may be retired in a near, future release.\n"
                                        + "*\n"
                                        + "* TalkToAndListenToStory adds _no_ _additonal_ _value_ over the InteractWith behavior (with the \"GossipOption\" attribute).\n"
                                        + "* Please update the profile to use InteractWith in preference to the TalkToAndListenToStory behavior.\n"
                                        + "*****");


                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                tmpMobIds = GetAttributeAsArray<int>("MobIds", true, ConstrainAs.MobId, new[] { "NpcIds" }, null);
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;


                foreach (int mobId in tmpMobIds)
                { _npcResults.Enqueue(NpcQueries.GetNpcById((uint)mobId)); }
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
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private readonly Queue<NpcResult> _npcResults = new Queue<NpcResult>();   // A Queue for npc's we need to talk to
        private Composite _root;

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: TalkToAndListenToStory.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~TalkToAndListenToStory()
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
                new PrioritySelector(ret => _npcResults.Count != 0 ? _npcResults.Peek() : null,

                    new Decorator(ret => ret == null,
                        new Action(ret => _isBehaviorDone = true)),

                    // Move to it if we are too far away.
                    new Decorator(ret => ret is NpcResult && ((NpcResult)ret).Location.Distance(StyxWoW.Me.Location) > 3,
                        new Sequence(
                            new Action(ret => TreeRoot.StatusText = "Moving towards - " + ((NpcResult)ret).Location),
                            new Action(ret => Navigator.MoveTo(((NpcResult)ret).Location)))
                            )
                        ,

                    new Sequence(ret => ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(unit => unit.Entry == ((NpcResult)ret).Entry),
                        new DecoratorContinue(ret => ret != null,
                            new Sequence(

                                new DecoratorContinue(ret => StyxWoW.Me.IsMoving,
                                    new Action(ret =>
                                    {
                                        WoWMovement.MoveStop();
                                        StyxWoW.SleepForLagDuration();
                                    })
                                    ),

                                new DecoratorContinue(ret => !GossipFrame.Instance.IsVisible,
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Interacting with - " + ((WoWUnit)ret).Name),
                                        new Action(ret => ((WoWUnit)ret).Interact()))
                                        ),

                                new WaitContinue(3, ret => !GossipFrame.Instance.IsVisible,
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Clicking thru gossip"),
                                        new Action(delegate
                                        {
                                            Lua.DoString("GossipTitleButton1:Click()");
                                            Thread.Sleep(1500);

                                            if (GossipFrame.Instance.IsVisible)
                                                return RunStatus.Running;

                                            _npcResults.Dequeue();
                                            return RunStatus.Success;
                                        })
                                        ))

                            )))
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

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
