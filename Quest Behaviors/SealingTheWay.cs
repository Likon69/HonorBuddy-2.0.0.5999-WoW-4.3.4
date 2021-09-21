// Behavior originally contributed by mastahg.
//
// DOCUMENTATION:
//     
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bots.Grind;
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
    public class SealingTheWay : CustomForcedBehavior
    {
        ~SealingTheWay()
        {
            Dispose(false);
        }

        public SealingTheWay(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = 26501;//GetAttributeAsNullable<int>("QuestId",false, ConstrainAs.QuestId(this), null) ?? 0;
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


        private bool IsObjectiveComplete(int objectiveId, uint questId)
        {
            if (this.Me.QuestLog.GetQuestById(questId) == null)
            {
                return false;
            }
            int returnVal = Lua.GetReturnVal<int>("return GetQuestLogIndexByID(" + questId + ")", 0);
            return
                Lua.GetReturnVal<bool>(
                    string.Concat(new object[] { "return GetQuestLogLeaderBoard(", objectiveId, ",", returnVal, ")" }), 2);
        }



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

        public WoWUnit Geomancer(WoWPoint loc)
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 43170 && u.IsAlive && u.Location.Distance(loc) <= 5).OrderBy(u => u.Distance).FirstOrDefault();
        }

        public WoWUnit Bad(WoWPoint loc)
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && !u.IsPlayer && u.CurrentTarget != null && (u.CurrentTarget == Geomancer(loc) || u.CurrentTarget == Me)).OrderBy(u => u.Distance).FirstOrDefault();
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
                return
                    new PrioritySelector(
                        new Decorator(ret => RoutineManager.Current.PullBehavior != null, RoutineManager.Current.PullBehavior),
                        new Action(c => RoutineManager.Current.Pull()));
            }
        }
        public WoWPoint[] Spots = new WoWPoint[] {new WoWPoint(411.33,1659.2,348.8838),
new WoWPoint(420.792,1718.1,349.4922),
new WoWPoint(457.47,1727.42,348.5146),
new WoWPoint(491.014,1659.59,348.2862)};


        public Composite Part(int i)
        {
            return new Decorator(r => !IsObjectiveComplete(i, (uint)QuestId), new PrioritySelector(

                 new Decorator(r => Geomancer(Spots[i - 1]) != null && Geomancer(Spots[i - 1]).Distance > 10, new Action(r => Flightor.MoveTo(Geomancer(Spots[i - 1]).Location))),
                    new Decorator(r => (Me.CurrentTarget == null || (Me.CurrentTarget != null && Me.CurrentTarget.IsFriendly)) && Bad(Spots[i - 1]) != null, new Action(r => Bad(Spots[i - 1]).Target())),
                    new Decorator(r => (Me.CurrentTarget == null || (Me.CurrentTarget != null && Me.CurrentTarget.IsFriendly)) && (Geomancer(Spots[i - 1]).CurrentTarget != null), new Action(r => Geomancer(Spots[i - 1]).CurrentTarget.Target())),

                    //new Decorator(r => Me.CurrentTarget != null && !Me.CurrentTarget.IsFriendly, DoDps),
                new Decorator(r => !Me.Combat && Bad(Spots[i - 1]) != null, DoPull),
                    new Decorator(r => Bad(Spots[i - 1]) == null, UseItem(i - 1))));

        }

        public WoWItem Rock
        {
            get { return Me.BagItems.FirstOrDefault(x => x.Entry == 58885); }
        }

        public Composite UseItem(int x)
        {

            return new Action(delegate
            {
                var g = Geomancer(Spots[x]);
                if (g.Distance > 5)
                    Navigator.MoveTo(g.Location);
                g.Target();
                Rock.Use();
            });

        }

        protected override Composite CreateBehavior()
        {

            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, LevelBot.CreateCombatBehavior(), Part(1), Part(2), Part(3), Part(4))));
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






                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}