// Behavior originally contributed by Natfoth.
//
// WIKI DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_CompleteLogQuest
//
// QUICK DOX:
//      Allows you to 'turn in' a quest to your quest log.
//
//  Parameters (required, then optional--both listed alphabetically):
//      QuestId: Id of the quest to turn into your quest log.  It is a _fatal_ error
//               if the quest is not complete.
//
using System;
using System.Collections.Generic;
using System.Threading;

using CommonBehaviors.Actions;

using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;
using Styx.WoWInternals;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class CompleteLogQuest : CustomForcedBehavior
    {
        public CompleteLogQuest(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), new[] { "QuestID" }) ?? 0;


                // Final initialization...
                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                QuestName = (quest != null) ? quest.Name : string.Format("QuestId({0})", QuestId);
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
        public static int QuestId { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private bool _newQuest;
        private Composite _root;

        // Private properties
        private TimeSpan Delay_WaitForNewQuestOfferred = TimeSpan.FromMilliseconds(5000);
        private TimeSpan Delay_WowClientLagTime { get { return (TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150)); } }
        private int QuestIndexId { get { return (Lua.GetReturnVal<int>("return  GetQuestLogIndexByID(" + QuestId + ")", 0)); } }
        private string QuestName { get; set; }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: CompleteLogQuest.cs 218 2012-02-19 18:41:10Z raphus $"); } }
        public override string SubversionRevision { get { return ("$Revision: 218 $"); } }


        ~CompleteLogQuest()
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
                Lua.Events.DetachEvent("QUEST_DETAIL", HandleQuestDetail);
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        public void HandleQuestDetail(object obj, LuaEventArgs args)
        {
            _newQuest = true;
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return (_root ?? (_root =
                new PrioritySelector(

                    new Decorator(ret => _isBehaviorDone,
                        new ActionAlwaysSucceed()),

                    new Sequence(
                            new Action(delegate
                            {
                                TreeRoot.StatusText = "Completing Log Quest: " + QuestName;
                                Lua.DoString("ShowQuestComplete({0})", QuestIndexId);
                            }),
                            new WaitContinue(Delay_WowClientLagTime, ret => false, new ActionAlwaysSucceed()),
                            new Action(delegate { Lua.DoString("CompleteQuest()"); }),
                            new WaitContinue(Delay_WowClientLagTime, ret => false, new ActionAlwaysSucceed()),
                            new Action(delegate { Lua.DoString("GetQuestReward({0})", 1); }),
                            new WaitContinue(Delay_WaitForNewQuestOfferred, ret => _newQuest,
                                                new Action(ret => Lua.DoString("AcceptQuest()"))),
                            new Action(delegate
                            {
                                Lua.DoString("CloseQuest()");
                                TreeRoot.StatusText = "Finished!";
                                _isBehaviorDone = true;
                            })
                        ))
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
                return (_isBehaviorDone);
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

                TreeRoot.GoalText = this.GetType().Name + ": " + QuestName;

                if (quest != null)
                {
                    if (!quest.IsCompleted)
                    {
                        LogMessage("fatal", "Quest({0}, \"{1}\") is not complete.", QuestId, QuestName);
                        _isBehaviorDone = true;
                    }
                }

                else
                {
                    LogMessage("warning", "Quest({0}) is not in our log--skipping turn in.", QuestId);
                    _isBehaviorDone = true;
                }

                Lua.Events.AttachEvent("QUEST_DETAIL", HandleQuestDetail);
            }
        }

        #endregion
    }
}

