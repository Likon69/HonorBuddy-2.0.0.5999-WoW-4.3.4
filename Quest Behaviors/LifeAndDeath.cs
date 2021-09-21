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
    public class LifeAndDeath : CustomForcedBehavior
    {
        ~LifeAndDeath()
        {
            Dispose(false);
        }

        public LifeAndDeath(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = 28758; //GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;
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


        public WoWUnit Dragon
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 49873 && u.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        private WoWPoint spot = new WoWPoint(-2739.832,-5000.674,-127.1305);

        public WoWUnit Brain
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 47960 && u.IsAlive).OrderBy(
                        u => u.Distance).FirstOrDefault();
            }
        }


        public Composite LowMeter
        {
            get
            {
                return  new Decorator(r => Me.CurrentHealth < 50 || power > 200,MoveBack);
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

        public Composite MoveBack
        {
            get
            {
                return new Decorator(r => Me.Location.Distance(spot) > 5, new Action(r=> Navigator.MoveTo(spot)));
            }
        }

      


        public Composite GetInRange
        {
            get
            {
                return new Decorator(r => Dragon != null && Me.Location.Distance(Dragon.Location) > 45 , new Action(r=> WoWMovement.ClickToMove(Dragon.Location)));
            }
        }

        public Composite SlitThroats
        {
            get
            {
                return new Decorator(r => Dragon != null && Dragon.HealthPercent < 10, new Action(delegate
                                                                                                      {
                                                                                                          Dragon.Target();
                                                                                                          Lua.DoString(
                                                                                                              "CastPetAction(3);");
                                                                                                      }));
            }
        }

        public Composite ShootFire
        {
            get
            {
                return new Decorator(r => Dragon != null, new Action(delegate
                {
                    
                    Dragon.Target();
                    Navigator.PlayerMover.MoveStop();
                    Lua.DoString(
                        "CastPetAction(1);");
                }));
            }
        }

        protected override Composite CreateBehavior()
        {
            //return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(ShootArrows,Lazor, BunchUp, new ActionAlwaysSucceed())));
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet,GetInRange,SlitThroats,ShootFire)));
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


        public int power
        {
            get
            {
                return Lua.GetReturnVal<int>("return UnitPower(\"player\",ALTERNATE_POWER_INDEX)", 0);     }
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