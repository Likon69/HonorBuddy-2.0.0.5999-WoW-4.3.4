// Behavior originally contributed by Natfoth.
//
// DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_VehicleBehavior
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class VehicleBehavior : CustomForcedBehavior
    {
        /// <summary>
        /// Will control a vehicle and fire on locations/Mobs
        /// ##Syntax##
        /// QuestId: Id of the quest.
        /// NpcMountID: MobId of the vehicle before it is mounted.
        /// VehicleID: Mob of the actual Vehicle, sometimes it will be the some but sometimes it will not be.
        /// SpellIndex: Button bar Number starting from 1
        /// FireHeight: Between 0 - 99 The lower the number the closer to the ground it will be
        /// FireTillFinish: This is used for a few quests that the mob is flying but respawns fast, So the bot can fire in the same spot over and over.
        /// FireLocation Coords: Where you want to be at when you fire.
        /// TargetLocation Coords: Where you want to aim.
        /// PreviousFireLocation Coords: This should only be used if you are already inside of the vehicle when you call the behaviors again, and
        ///                                 should be the same coords as FireLocation on the call before it, Check the Wiki for more info or examples.
        /// </summary>
        /// 
        public VehicleBehavior(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                AttackButton = GetAttributeAsNullable("AttackButton", true, ConstrainAs.HotbarButton, new[] { "SpellIndex" }) ?? 0;
                FirePoint = GetAttributeAsNullable("FireLocation", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                FireHeight = GetAttributeAsNullable("FireHeight", false, new ConstrainTo.Domain<int>(1, 999), null) ?? 1;
                FireUntilFinished = GetAttributeAsNullable<bool>("FireUntilFinished", false, null, new[] { "FireTillFinish" }) ?? false;
                PreviousLocation = GetAttributeAsNullable("PreviousFireLocation", false, ConstrainAs.WoWPointNonEmpty, null);
                QuestId = GetAttributeAsNullable("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                TargetPoint = GetAttributeAsNullable("TargetLocation", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                VehicleId = GetAttributeAsNullable("VehicleId", true, ConstrainAs.VehicleId, new[] { "VehicleID" }) ?? 0;
                VehicleMountId = GetAttributeAsNullable("VehicleMountId", true, ConstrainAs.VehicleId, new[] { "NpcMountId", "NpcMountID" }) ?? 0;

                StartObjectivePoint = GetAttributeAsNullable("StartObjectivePoint", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                NPCIds = GetNumberedAttributesAsArray("MobId", 0, ConstrainAs.MobId, new[] { "NpcID" });
                EndPoint = GetAttributeAsNullable("EndPoint", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                StartPoint = GetAttributeAsNullable("StartPoint", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;

                VehicleType = GetAttributeAsNullable("VehicleType", false, new ConstrainTo.Domain<int>(0, 4), null) ?? 0;
                Counter = 0;

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
        public int AttackButton { get; set; }

        public int FireHeight { get; private set; }
        public WoWPoint FirePoint { get; private set; }
        public bool FireUntilFinished { get; set; }
        public WoWPoint? PreviousLocation { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint TargetPoint { get; private set; }
        public int VehicleId { get; set; }
        public int[] NPCIds { get; set; }
        public int VehicleMountId { get; private set; }

        public WoWPoint StartObjectivePoint { get; private set; }
        public int VehicleType { get; set; }


        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private bool _isInitialized;
        private int _pathIndex;
        private Composite _root;

        // Private properties
        private int Counter { get; set; }
        private bool InVehicle { get { return Lua.GetReturnVal<bool>("return  UnitUsingVehicle(\"player\")", 0); } }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        private List<WoWUnit> NpcAttackList
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                     .Where(ret => (NPCIds.Contains((int)ret.Entry)) && !ret.Dead)
                                     .OrderBy(u => u.Distance)
                                     .ToList();
            }
        }


        private List<WoWUnit> NpcVehicleList
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                     .Where(ret => (ret.Entry == VehicleMountId) && !ret.Dead)
                                     .OrderBy(u => u.Distance)
                                     .ToList();
            }
        }
        private WoWPoint[] Path { get; set; }
        private CircularQueue<WoWPoint> PathCircle { get; set; }
        private WoWPoint StartPoint { get; set; }    // Start point Where Mount Is
        private WoWPoint EndPoint { get; set; }

        private WoWUnit _vehicle;
        private List<WoWUnit> VehicleList
        {
            get
            {
                if (PreviousLocation.HasValue)
                {
                    return ObjectManager.GetObjectsOfType<WoWUnit>()
                                        .Where(ret => (ret.Entry == VehicleId) && !ret.Dead)
                                        .OrderBy(u => u.Location.Distance(PreviousLocation.Value))
                                        .ToList();
                }
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                    .Where(ret => (ret.Entry == VehicleId) && !ret.Dead)
                    .OrderBy(u => u.Distance)
                    .ToList();
            }
        }

        // Styx.Logic.Profiles.Quest.ProfileHelperFunctionsBase


        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: VehicleBehavior.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~VehicleBehavior()
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


        WoWPoint MoveToLocation
        {
            get
            {

                Path = Navigator.GeneratePath(_vehicle.Location, FirePoint);
                _pathIndex = 0;

                while (Path[_pathIndex].Distance(_vehicle.Location) <= 3 && _pathIndex < Path.Length - 1)
                    _pathIndex++;
                return Path[_pathIndex];

            }
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                           new Decorator(ret => (Counter > 0 && !FireUntilFinished) || (Me.QuestLog.GetQuestById((uint)QuestId) != null && Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted),
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Finished!"),
                                    new WaitContinue(120,
                                        new Action(delegate
                                        {
                                            _isBehaviorDone = true;
                                            return RunStatus.Success;
                                        }))
                                    )),

                           new Decorator(ret => !_isInitialized && VehicleType == 2,
                            new Action(ret => ParsePaths())),

                        new Decorator(c => !InVehicle && NpcVehicleList.Count == 0,
                            new Action(c =>
                            {

                                Navigator.MoveTo(StartPoint);
                                TreeRoot.StatusText = "Moving To Vehicle Location - " + " Yards Away: " + StartPoint.Distance(Me.Location);
                                return RunStatus.Success;


                            })
                        ),

                           new Decorator(c => !InVehicle && NpcVehicleList.Count > 0,
                            new Action(c =>
                            {
                                if (!NpcVehicleList[0].WithinInteractRange)
                                {
                                    Navigator.MoveTo(NpcVehicleList[0].Location);
                                    TreeRoot.StatusText = "Moving To Vehicle - " + NpcVehicleList[0].Name + " Yards Away: " + NpcVehicleList[0].Location.Distance(Me.Location);
                                }
                                else
                                {
                                    Flightor.MountHelper.Dismount();

                                    NpcVehicleList[0].Interact();
                                    PreviousLocation = Me.Location;

                                }

                                return RunStatus.Success;

                            })
                        ),
                        new Decorator(c => InVehicle && VehicleType == 0,
                            new Action(c =>
                            {
                                if (_vehicle == null)
                                {
                                    _vehicle = VehicleList[0];
                                }

                                if (_vehicle.Location.Distance(FirePoint) <= 5)
                                {
                                    TreeRoot.StatusText = "Firing Vehicle - " + _vehicle.Name + " Using Spell Index: " + AttackButton + " Height: " + FireHeight;
                                    WoWMovement.ClickToMove(TargetPoint);
                                    Thread.Sleep(500);
                                    WoWMovement.MoveStop();

                                    using (new FrameLock())
                                    {
                                        Lua.DoString("VehicleAimRequestNormAngle(0.{0})", FireHeight);
                                        Lua.DoString("CastPetAction({0})", AttackButton);
                                        Counter++;
                                        return RunStatus.Success;
                                    }
                                }
                                if (_vehicle.Location.Distance(FirePoint) > 5)
                                {
                                    TreeRoot.StatusText = "Moving To FireLocation - Yards Away: " + FirePoint.Distance(_vehicle.Location);
                                    WoWMovement.ClickToMove(MoveToLocation);
                                    _vehicle.Target();
                                }
                                return RunStatus.Running;
                            })),

                        new Decorator(c => InVehicle && VehicleType == 1,
                            new Action(c =>
                            {
                                if (_vehicle == null)
                                {
                                    _vehicle = VehicleList[0];
                                }

                                if (NpcAttackList.Count > 1)
                                {
                                    TreeRoot.StatusText = "Moving to Assult - " + NpcAttackList[0].Name + " Using Spell Index: " + AttackButton;

                                    if (_vehicle.Location.Distance(NpcAttackList[0].Location) > 20)
                                    {
                                        WoWMovement.ClickToMove(NpcAttackList[0].Location);

                                        if (Me.CurrentTarget != NpcAttackList[0])
                                            NpcAttackList[0].Target();

                                        return RunStatus.Success;
                                    }


                                    Lua.DoString("VehicleAimRequestNormAngle(0.{0})", FireHeight);
                                    Lua.DoString("CastPetAction({0})", AttackButton);
                                    Counter++;
                                    Thread.Sleep(1000);



                                    return RunStatus.Success;
                                }
                                if (_vehicle.Location.Distance(StartObjectivePoint) > 5)
                                {
                                    TreeRoot.StatusText = "Moving To Start Location - Yards Away: " + StartObjectivePoint.Distance(Me.Location);

                                    bool testfly = StyxWoW.Me.MovementInfo.CanFly;

                                    Logging.Write("" + testfly);


                                    Flightor.MoveTo(StartObjectivePoint);

                                    // WoWMovement.ClickToMove(StartObjectivePoint);

                                    _vehicle.Target();
                                }
                                return RunStatus.Running;
                            })),

                        new Decorator(c => InVehicle && VehicleType == 2,
                            new Action(c =>
                            {
                                if (_vehicle == null && VehicleList[0] != null)
                                {
                                    _vehicle = VehicleList[0];
                                }

                                if ((Counter > 0 && !FireUntilFinished) || (Me.QuestLog.GetQuestById((uint)QuestId) != null && Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted))
                                {
                                    if (EndPoint.Distance(Me.Location) > 20)
                                    {
                                        Flightor.MoveTo(EndPoint);
                                        return RunStatus.Running;
                                    }
                                    return RunStatus.Success;
                                }


                                if (PathCircle.Count == 0)
                                {
                                    //Counter++;
                                    ParsePaths();
                                    return RunStatus.Running;
                                }

                                if (PathCircle.Peek().Distance(Me.Location) > 5)
                                {
                                    Flightor.MoveTo(PathCircle.Peek());
                                    return RunStatus.Running;
                                }
                                WoWMovement.MoveStop();
                                Thread.Sleep(400);

                                if (NpcAttackList[0] != null)
                                    WoWMovement.ClickToMove(NpcAttackList[0].Location);

                                WoWMovement.MoveStop();
                                Thread.Sleep(400);
                                Lua.DoString("CastPetAction({0})", AttackButton);
                                WoWMovement.MoveStop();

                                PathCircle.Dequeue();

                                return RunStatus.Running;
                            }))

                    ));
        }

        public IEnumerable<WoWPoint> ParseWoWPoints(IEnumerable<XElement> elements)
        {
            var temp = new List<WoWPoint>();

            foreach (XElement element in elements)
            {
                XAttribute xAttribute = element.Attribute("X");
                XAttribute yAttribute = element.Attribute("Y");
                XAttribute zAttribute = element.Attribute("Z");

                float x, y, z;
                float.TryParse(xAttribute.Value, out x);
                float.TryParse(yAttribute.Value, out y);
                float.TryParse(zAttribute.Value, out z);
                temp.Add(new WoWPoint(x, y, z));
            }

            return temp;
        }

        private void ParsePaths()
        {
            var startPoint = WoWPoint.Empty;
            var path = new CircularQueue<WoWPoint>();

            foreach (WoWPoint point in ParseWoWPoints(Element.Elements().Where(elem => elem.Name == "Start")))
                startPoint = point;

            foreach (WoWPoint point in ParseWoWPoints(Element.Elements().Where(elem => elem.Name == "Path")))
                path.Enqueue(point);

            StartPoint = startPoint;
            PathCircle = path;
            _isInitialized = true;
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

                TreeRoot.GoalText = GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
