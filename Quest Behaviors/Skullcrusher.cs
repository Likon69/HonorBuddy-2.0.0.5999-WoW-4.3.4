// Behavior originally contributed by mastahg.
//
// DOCUMENTATION:
//     
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonBehaviors.Actions;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Tripper.Tools.Math;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class Skullcrusher : CustomForcedBehavior
    {
        ~Skullcrusher()
        {
            Dispose(false);
        }

        public Skullcrusher(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;
                QuestRequirementComplete = QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = QuestInLogRequirement.InLog;
                MobIds = new uint[] { 50635, 50638, 50643, 50636 };
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
        public uint[] MobIds { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint Location { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;


        // Private properties
        private LocalPlayer Me
        {
            get { return (ObjectManager.Me); }
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
            /*var items = ObjectManager.GetObjectsOfType<WoWDynamicObject>();
            foreach (var obj in items) 
            {
                Logging.Write("{0} Radius: {1}", obj.Name, obj.Radius);
            }*/


            _isDisposed = true;
        }



        #region Overrides of CustomForcedBehavior




        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsQuestComplete(), new Action(delegate
                    {
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));

            }
        }

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }


        public WoWUnit Boss
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 46732 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }


        private WoWPoint spot = new WoWPoint(-2739.832, -5000.674, -127.1305);


        public WoWUnit Ally
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsFriendly && !u.IsPlayer && u.HealthPercent < 10).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }
        public WoWUnit Horn
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 50653 || u.Entry == 50655).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }


        public WoWUnit GoodAltar
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => MobIds.Contains(u.Entry) && u.CanSelect).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }


        public WoWUnit AltarOfShadow
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 50640).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        public Composite DoDps
        {
            get
            {
                return
                    new PrioritySelector(
                        new Decorator(ret => RoutineManager.Current.CombatBehavior != null, RoutineManager.Current.CombatBehavior),
                        new Action(c => RoutineManager.Current.Combat()));
            }
        }





        bool insidecircle(WoWPoint Altar, int radius)
        {
            return (Math.Pow((Me.Location.X - Altar.X), 2) + Math.Pow((Me.Location.Y - Altar.Y), 2) < Math.Pow(radius, 2));
        }


        public Composite AllyStuff
        {
            get
            {
                return new PrioritySelector(new Decorator(r => !Ally.WithinInteractRange, new Action(delegate
                                                                                                         {
                                                                                                             Navigator.
                                                                                                                 MoveTo(
                                                                                                                     Ally
                                                                                                                         .
                                                                                                                         Location);
                                                                                                         })),
                                                                                                        new Decorator(r => Ally.WithinInteractRange, new Action(delegate
                                                                                                        {
                                                                                                            Navigator.PlayerMover.MoveStop();
                                                                                                            Ally.Face();
                                                                                                            Ally.Interact();
                                                                                                        }))


                                                                                                        );
            }
        }

        public Composite SaveAlly
        {
            get
            {
                return new Decorator(r => Ally != null, AllyStuff);
            }
        }

        public Composite ClickAltar
        {
            get
            {
                return new Decorator(r => GoodAltar != null, new Action(z => GoodAltar.Interact()));
            }
        }


        public Composite ClickHorn
        {
            get
            {
                return new Decorator(r => Horn != null, new Action(z => Horn.Interact()));
            }
        }




        public Composite MoveOutOfShadow
        {
            get
            {
                return new Decorator(r => AltarOfShadow != null && insidecircle(AltarOfShadow.Location, 15), new Action(delegate
                {
                    Logging.Write("Moving out of shadow.");
                    Navigator.MoveTo(Boss.Location);
                }));
            }
        }

        public Composite CheckTarget
        {
            get
            {
                return new Decorator(r => Me.CurrentTarget == null || (Me.CurrentTarget != null && Me.CurrentTarget != Boss && Boss != null), new Action(r => Boss.Target()));
            }
        }





        protected override Composite CreateBehavior()
        {

            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, ClickHorn, SaveAlly, ClickAltar, MoveOutOfShadow, CheckTarget, DoDps)));
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
                if (TreeRoot.Current != null && TreeRoot.Current.Root != null &&
                    TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                // Me.QuestLog.GetQuestById(27761).GetObjectives()[2].




                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}