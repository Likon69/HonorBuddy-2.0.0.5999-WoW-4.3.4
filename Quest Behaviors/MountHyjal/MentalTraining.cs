// Behavior originally contributed by Bobby53.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors.MountHyjal
{
    public class MentalTraining : CustomForcedBehavior
    {
        /// <summary>
        /// Completes the quest http://www.wowhead.com/quest=25299
        /// This behavior completes the quest by correctly responding to
        /// 10 yes/no questions by checking the toons question aura.
        /// 
        /// Requires you to already be in position at the quest give Instructor Mylva X="4524.021" Y="-4731.176" Z="887.9406"
        /// 
        /// ##Syntax##
        /// QuestId: Id of the quest (default is 0)
        /// [Optional] QuestName: optional quest name (documentation only)
        /// </summary>
        /// 
        public MentalTraining(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                /* */
                GetAttributeAs<string>("QuestName", false, ConstrainAs.StringNonEmpty, null);            // (doc only - not used)
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
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private static int _lineCount;
        private Composite _root;

        // Private properties
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: MentalTraining.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~MentalTraining()
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


        public void Log(string format, params object[] args)
        {
            // following linecount hack is to stop dup suppression of Log window
            LogMessage("info", Color.Green, format + (++_lineCount % 2 == 0 ? "" : " "), args);
        }


        public bool DoWeHaveQuest()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest != null;
        }

        public bool IsQuestComplete()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }

        public bool HasAura(WoWUnit unit, int auraId)
        {
            WoWAura aura = (from a in unit.Auras
                            where a.Value.SpellId == auraId
                            select a.Value).FirstOrDefault();
            return aura != null;
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                    // check if we have finished 10 questions (marked complete)
                    new Decorator(ret => IsQuestComplete(),
                        new PrioritySelector(
                            new Decorator(ret => Me.HasAura("Mental Training"),
                                new Action(delegate
                                {
                                    Log("Mental Training complete - exiting Orb");
                                    Lua.DoString("RunMacroText(\"/click BonusActionButton4\")");
                                    StyxWoW.SleepForLagDuration();
                                    return RunStatus.Success;
                                })
                            ),
                            new Action(ret => _isBehaviorDone = true)
                        )
                    ),

                    // if we don't have vehicle buff, use Orb of Ascension
                    new Decorator(ret => !Me.HasAura("Mental Training"),
                        new Action(delegate
                        {
                            Log("Using Orb of Ascension");
                            // WoWItem orb =  Me.Inventory.Items.FirstOrDefault( i => i != null && i.Entry == 52828 );
                            WoWItem orb = ObjectManager.GetObjectsOfType<WoWItem>().Where(u => u.Entry == 52828).FirstOrDefault();
                            if (orb == null)
                            { LogMessage("fatal", "Quest item \"Orb of Ascension\" not in inventory."); }

                            orb.Use(true);
                            StyxWoW.SleepForLagDuration();
                            return RunStatus.Success;
                        })
                    ),

                    // if we have YES aura 74008, then click yes
                    new Decorator(ret => HasAura(Me, 74008),
                        new Action(delegate
                        {
                            Log("Answering YES");
                            Thread.Sleep(500);
                            Lua.DoString("RunMacroText(\"/click BonusActionButton1\")");
                            StyxWoW.SleepForLagDuration();
                            return RunStatus.Success;
                        })
                    ),

                    // if we have NO aura 74009, then click no
                    new Decorator(ret => HasAura(Me, 74009),
                        new Action(delegate
                        {
                            Log("Answering NO");
                            Thread.Sleep(500);
                            Lua.DoString("RunMacroText(\"/click BonusActionButton2\")");
                            StyxWoW.SleepForLagDuration();
                            return RunStatus.Success;
                        })
                    ),
                new Action(delegate
                {
                    return RunStatus.Success;
                })
             )
          );
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
