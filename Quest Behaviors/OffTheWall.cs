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
    public class OffTheWall : CustomForcedBehavior
    {
        ~OffTheWall()
        {
            Dispose(false);
        }

        public OffTheWall(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = 28591; //GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
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

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }



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



        public WoWUnit Marksmen
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 49124 && u.IsAlive).OrderBy(
                        u => u.Distance).FirstOrDefault();
            }
        }



        public WoWUnit Cannoner
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 49025 && u.IsAlive).OrderBy(
                        u => u.Distance).FirstOrDefault();
            }
        }

        public WoWUnit Cannon
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 49060 && u.IsAlive).OrderBy(
                        u => u.Distance).FirstOrDefault();
            }
        }




        private WoWPoint endspot = new WoWPoint(1076.7, 455.7638, -44.20478);
        private WoWPoint spot = new WoWPoint(1109.848, 462.9017, -45.03053);


        WoWUnit GetTurret()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.CharmedByUnitGuid == 0 || u.CharmedByUnitGuid == Me.Guid) && u.Entry == 49135)
                .OrderBy(u => u.DistanceSqr).
                FirstOrDefault();
        }

        protected override Composite CreateBehavior()
        {
            //return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(ShootArrows,Lazor, BunchUp, new ActionAlwaysSucceed())));
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(new Action(ret => Loopstuff()))));
        }

        private bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') then return 1 else return 0 end", 0) == 1; } }


        public void Loopstuff()
        {
            while (true)
            {
                ObjectManager.Update();
                if (IsQuestComplete())
                {
                    _isBehaviorDone = true;
                    break;
                }



                try
                {

                    if (!InVehicle)
                    {
                        var turret = GetTurret();
                        if (turret != null)
                        {
                            if (turret.DistanceSqr > 5 * 5)
                            {
                                //Navigator.MoveTo(turret.Location);
                            }
                            else
                                turret.Interact();
                        }
                        else
                        {
                            Logging.Write("Unable to find turret");
                        }
                    }
                    else
                    {

                        if (Me.CurrentTarget != null &&
                            (Me.CurrentTarget.Distance < 60 || Me.CurrentTarget.InLineOfSight))
                        {



                            WoWMovement.ClickToMove(Me.CurrentTarget.Location);
                            //WoWMovement.ClickToMove(Me.CurrentTarget.Location.RayCast(Me.CurrentTarget.Rotation, 20));
                            var x = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(z => z.CharmedByUnit == Me);

                            Tripper.Tools.Math.Vector3 v = Me.CurrentTarget.Location - Me.Location;
                            v.Normalize();
                            Lua.DoString(
                                string.Format(
                                    "VehicleAimIncrement(({0} - VehicleAimGetAngle())); CastPetAction(1);CastPetAction(2);",
                                    Math.Asin(v.Z).ToString()));


                        }
                        else
                        {
                            if (!IsObjectiveComplete(1, (uint) QuestId))
                            {
                                if (Marksmen != null)
                                    Marksmen.Target();
                            }
                            else if (!IsObjectiveComplete(2, (uint) QuestId))
                            {
                                if (Cannoner != null)
                                    Cannoner.Target();
                            }
                            else if (!IsObjectiveComplete(3, (uint) QuestId))
                            {
                                if (Cannon != null)
                                    Cannon.Target();
                            }

                        }
                    }

                }
                catch (Exception exception)
                {

                }
            }
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