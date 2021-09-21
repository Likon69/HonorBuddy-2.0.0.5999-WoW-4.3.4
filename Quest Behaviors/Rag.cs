// Behavior originally contributed by mastahg based off killuntilcomplete.
//
// DOCUMENTATION:
//     
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx.Combat.CombatRoutine;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class Rag : CustomForcedBehavior
    {
        /// <summary>
        /// This is only used when you get a quest that Says, Kill anything x times. Or on the chance the wowhead ID is wrong
        /// ##Syntax##
        /// QuestId: Id of the quest.
        /// MobId, MobId2, ...MobIdN: Mob Values that it will kill.
        /// X,Y,Z: The general location where theese objects can be found
        /// </summary>
        /// 
        public Rag(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??
                           WoWPoint.Empty;
                //MobIds = GetNumberedAttributesAsArray<int>("MobId", 1, ConstrainAs.MobId, new[] {"NpcID"})
                QuestRequirementComplete = QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = QuestInLogRequirement.InLog;
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error",
                           "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message + "\nFROM HERE:\n" + except.StackTrace +
                           "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller
        public WoWPoint Location { get; private set; }
        public int QuestId = 25551;// 26581;//25551;
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private LocalPlayer Me
        {
            get { return (ObjectManager.Me); }
        }

        private List<WoWUnit> Adds
        {
            get
            {//40794 40803 31146
                return
                    (ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 40794 && !u.Dead).OrderBy(u => u.Distance).ToList());
            }
        }

        private WoWUnit Ragnaros
        {
            get { return (ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 40793)); }
        }


        ~Rag()
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


        private bool IsRanged
        {
            get
            {
                return (((Me.Class == WoWClass.Druid) &&
                         (SpellManager.HasSpell("BalanceSpell") || SpellManager.HasSpell("RestoSpell"))) ||
                        ((Me.Class == WoWClass.Shaman) &&
                         (SpellManager.HasSpell("ElementalSpell") || SpellManager.HasSpell("RestoSpell"))) ||
                        (Me.Class == WoWClass.Hunter) || (Me.Class == WoWClass.Mage) || (Me.Class == WoWClass.Priest) ||
                        (Me.Class == WoWClass.Warlock));
            }
        }

        private int Range
        {
            get { return (IsRanged ? 29 : 3); }
        }

        private int Range2
        {
            get { return (IsRanged ? 30 : 30); }
        }

        #region Overrides of CustomForcedBehavior

        public Composite AddMethod
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => Adds.Count > 0 && Adds[0].Distance > Range, MoveCloser),
                    new Decorator(ret => Adds.Count > 0 && Adds[0].Distance <= Range, DoDps));
            }
        }

        public Composite RagMethod
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => Ragnaros.Distance > Range2, MoveCloserRag),
                    new Decorator(ret => Ragnaros.Distance <= Range2, DoDps));
            }
        }


        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(
                        ret =>
                        (Me.QuestLog.GetQuestById((uint)QuestId) != null &&
                         Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted),
                        new Sequence(new Action(ret => TreeRoot.StatusText = "Finished!"),
                                     new WaitContinue(120, new Action(delegate
                                     {
                                         _isBehaviorDone = true;
                                         return RunStatus.Success;
                                     }))));

            }
        }

        public Composite DoDps
        {
            get
            {
                return
                     new Sequence(new Action(ret => Navigator.PlayerMover.MoveStop()),
                    new PrioritySelector(
                        new Decorator(ret => RoutineManager.Current.CombatBehavior != null, RoutineManager.Current.CombatBehavior),
                        new Action(ret => RoutineManager.Current.Combat())));
            }
        }
        public Composite MoveCloser
        {
            get
            {
                return new Action(delegate
                {
                    var target = Adds[0];

                    target.Target();
                    Navigator.MoveTo(target.Location);
                });
            }
        }

        public Composite MoveCloserRag
        {
            get
            {
                return new Action(delegate
                {
                    var target = Ragnaros;

                    target.Target();
                    Navigator.MoveTo(target.Location);
                });
            }
        }


        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new PrioritySelector(DoneYet,AddMethod));
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
                return (_isBehaviorDone // normal completion
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

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}