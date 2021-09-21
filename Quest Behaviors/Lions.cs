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
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class Lions : CustomForcedBehavior
    {
        ~Lions()
        {
            Dispose(false);
        }

        public Lions(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                QuestId = 28277;//GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
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
        public int MobIds { get; private set; }
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

        private List<WoWUnit> lions
        {
            get { return (ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 48169 && !u.Dead).ToList()); }
        }


        private List<WoWUnit> Enemies
        {
            get { return (ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.Entry == 48199 || u.Entry == 48209) && !u.Dead).OrderBy(u => u.Distance).ToList()); }
        }


        int MobCountAtLocation(WoWPoint point, float radius, params uint[] mobIds)
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Count(u => u.IsAlive && mobIds.Contains(u.Entry) && u.DistanceSqr <= radius * radius);
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

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }


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

        public Composite DoPull
        {
            get
            {
                return new PrioritySelector(
                        new Decorator(ret => RoutineManager.Current.PullBehavior != null, RoutineManager.Current.PullBehavior),
                        new Action(c => RoutineManager.Current.Pull()));
            }
        }

        public Composite MoveEm
        {
            get
            {
                return new Action(ret => UsePetAbility("Move"));
            }
        }


        public void UsePetAbility(string action)
        {

            var spell = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logging.Write(string.Format("[Pet] Casting {0}", action));
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
            if (action == "Move")
                LegacySpellManager.ClickRemoteLocation(Enemies[0].Location);

        }


        public Composite KillIt
        {
            get
            {
                return new Decorator(ret => Me.CurrentTarget.Distance < 6 || Me.Class == WoWClass.Hunter, DoDps);
            }
        }

        //MobCountAtLocation(lions[0].Location, 10, 48169) != lions.Count
        public Composite KeepClose
        {
            get
            {
                return new Decorator(ret => lions.Count > 0 && Enemies.Count > 0 && MobCountAtLocation(lions[0].Location, 15, 48199, 48209) < 1 && Me.PetSpells[0].Cooldown == false, MoveEm);
            }
        }


        public Composite Swipe
        {
            get
            {
                return new Decorator(ret => lions.Count > 0 && MobCountAtLocation(lions[0].Location, 20, 48199,48209) >= 4 && Me.PetSpells[2].Cooldown == false, new Action(ret => UsePetAbility("Claw Flurry")));
            }
        }

        public Composite Fear
        {
            get
            {
                return new Decorator(ret => lions.Count > 0 && MobCountAtLocation(lions[0].Location, 20, 48199, 48209) >= 3 && Me.PetSpells[1].Cooldown == false, new Action(ret => UsePetAbility("Fierce Roar")));
            }
        }

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, KeepClose,Swipe,Fear, new ActionAlwaysSucceed())));
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



                if (TreeRoot.Current != null && TreeRoot.Current.Root != null && TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }




                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }




        }







        #endregion
    }
}