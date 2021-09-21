// Behavior originally contributed by HighVoltz.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    /// <summary>
    /// Moves to location while in a vehicle
    /// ##Syntax##
    /// VehicleId, VehicleId2, ...VehicleIdN [CountRequired:1]: ID of the vehicle (sometimes the 'same' vehicle may have multiple IDs)
    /// UseNavigator: (optional) true/false. Setting to false will use Click To Move instead of the Navigator. Default true
    /// Precision: (optional) This behavior moves on to the next waypoint when at Precision distance or less to current waypoint. Default 4;
    /// MobID: (optional) NPC ID to cast spell on to cast spell on.. not required even if you specify a spellID
    /// SpellID: (optional) Casts spell after reaching location.
    /// CastTime: (optional) The Spell Cast Time. Default 0;
    /// CastNum: (optional) The Spell Cast Time. Default 1;
    /// IgnoreCombat: (optional) true/false. Setting to true will keep running the behavior even if in combat.Default true
    /// QuestId: (optional) Quest that this behavior performs on.
    /// Hop : (optional) true/false. Setting this to true will cause the bot to jump while its moving to location.. this serves as an anti-stuck mechanic Default: false
    /// X,Y,Z: The location where you want to move to
    /// </summary>
    public class VehicleMover : CustomForcedBehavior
    {
        public VehicleMover(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                CastNum = GetAttributeAsNullable<int>("CastNum", false, ConstrainAs.RepeatCount, null) ?? 1;
                CastTime = GetAttributeAsNullable<int>("CastTime", false, new ConstrainTo.Domain<int>(0, 30000), null) ?? 0;
                Hop = GetAttributeAsNullable<bool>("Hop", false, null, null) ?? false;
                IgnoreCombat = GetAttributeAsNullable<bool>("IgnoreCombat", false, null, null) ?? true;
                Location = GetAttributeAsNullable<WoWPoint>("", false, ConstrainAs.WoWPointNonEmpty, null) ?? Me.Location;
                MobIds = GetNumberedAttributesAsArray<int>("MobId", 0, ConstrainAs.MobId, new[] { "MobID", "NpcId" });
                Precision = GetAttributeAsNullable<double>("Precision", false, new ConstrainTo.Domain<double>(2.0, 100.0), null) ?? 4.0;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                SpellId = GetAttributeAsNullable<int>("SpellId", false, ConstrainAs.SpellId, new[] { "SpellID" }) ?? 0;
                UseNavigator = GetAttributeAsNullable<bool>("UseNavigator", false, null, null) ?? true;
                VehicleIds = GetNumberedAttributesAsArray<int>("VehicleId", 1, ConstrainAs.VehicleId, new[] { "VehicleID" });
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
        public int CastNum { get; private set; }
        public int CastTime { get; private set; }
        public bool Hop { get; private set; }
        public bool IgnoreCombat { get; private set; }
        public WoWPoint Location { get; private set; }
        public int[] MobIds { get; private set; }
        public WoWPoint[] Path { get; private set; }
        public double Precision { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public int SpellId { get; private set; }
        public bool UseNavigator { get; private set; }
        public int[] VehicleIds { get; private set; }

        // Private variables for internal state
        private int _castCounter;
        private bool _casted = false;
        private Stopwatch _castStopwatch = new Stopwatch();// cast timer.
        private bool _isBehaviorDone = false;
        private bool _isDisposed;
        private WoWPoint _lastPoint;
        private int _pathIndex;
        private Stopwatch _pauseStopwatch = new Stopwatch();// add a small pause before casting.. 
        private Composite _root;
        private Stopwatch _stuckTimer = new Stopwatch();

        // Private properties
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: VehicleMover.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~VehicleMover()
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


        bool InVehicle
        {
            get { return Lua.GetReturnVal<int>("return UnitIsControlling('player')", 0) == 1; }
        }


        public WoWUnit Vehicle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>(true)
                                    .Where(o => VehicleIds.Contains((int)o.Entry))
                                    .OrderBy(o => o.Distance)
                                    .FirstOrDefault();
            }
        }


        Composite CreateSpellBehavior
        {
            get
            {
                return new Action(c =>
                {
                    if (SpellId > 0)
                    {
                        if (!_casted)
                        {
                            if (!_pauseStopwatch.IsRunning)
                                _pauseStopwatch.Start();
                            if (_pauseStopwatch.ElapsedMilliseconds >= 1000 || CastTime == 0)
                            {
                                if (ObjectManager.Me.IsMoving && CastTime > 0)
                                {
                                    WoWMovement.MoveStop();
                                    if (IgnoreCombat)
                                        return RunStatus.Running;
                                    else
                                        return RunStatus.Success;
                                }
                                // getting a "Spell not learned" error if using HB's spell casting api..
                                Lua.DoString("CastSpellByID({0})", SpellId);
                                _castCounter++;
                                _casted = true;
                                if (CastTime == 0)
                                {
                                    _isBehaviorDone = true;
                                    return RunStatus.Success;
                                }
                                _pauseStopwatch.Stop();
                                _pauseStopwatch.Reset();
                                _castStopwatch.Reset();
                                _castStopwatch.Start();
                            }
                        }
                        else if (_castStopwatch.ElapsedMilliseconds >= CastTime)
                        {
                            if (_castCounter < CastNum)
                            {
                                _casted = false;
                                _castStopwatch.Stop();
                                _castStopwatch.Reset();
                            }
                            else
                            {
                                _castStopwatch.Stop();
                                _castStopwatch.Reset();
                                _isBehaviorDone = true;
                                return RunStatus.Success;
                            }
                        }
                        if (IgnoreCombat)
                            return RunStatus.Running;
                        else
                            return RunStatus.Success; ;
                    }
                    else
                    {
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }

                });
            }
        }


        WoWPoint moveToLocation
        {
            get
            {
                if (UseNavigator)
                {
                    WoWUnit vehicle = Vehicle;
                    if (MobIds.Count() > 0)
                    {
                        // target mob and move to it 
                        WoWUnit mob = ObjectManager.GetObjectsOfType<WoWUnit>(true).Where(o => MobIds.Contains((int)o.Entry)).
                            OrderBy(o => o.Distance).FirstOrDefault();
                        if (mob != null)
                        {
                            if (!ObjectManager.Me.GotTarget || ObjectManager.Me.CurrentTarget != mob)
                                mob.Target();
                            if (mob.Location.Distance(Location) > 4)
                            {
                                Location = mob.Location;
                                Path = Navigator.GeneratePath(vehicle.Location, Location);
                                _pathIndex = 0;
                                if (Path == null || Path.Length == 0)
                                    UseNavigator = false;
                            }
                        }
                    }
                    if (Hop && (!_stuckTimer.IsRunning || _stuckTimer.ElapsedMilliseconds > 2000))
                    {
                        _stuckTimer.Reset();
                        if (!_stuckTimer.IsRunning)
                            _stuckTimer.Start();
                        if (_lastPoint.Distance(vehicle.Location) <= 3)
                        {
                            WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend | WoWMovement.MovementDirection.StrafeLeft);
                            WoWMovement.MoveStop(WoWMovement.MovementDirection.JumpAscend | WoWMovement.MovementDirection.StrafeLeft);
                        }
                        _lastPoint = vehicle.Location;
                    }

                    if (vehicle.Location.Distance(Path[_pathIndex]) <= Precision && _pathIndex < Path.Length - 1)
                        _pathIndex++;
                    return Path[_pathIndex];
                }
                else
                    return Location;
            }
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ??
                (_root = new PrioritySelector(
                    new Decorator(c => !ObjectManager.Me.IsAlive, // if we ignore combat and die... 
                        new Action(c =>
                        {
                            return RunStatus.Failure;
                        })),
                    new Decorator(c => Vehicle == null,
                        new Action(c =>
                        {
                            return RunStatus.Failure;
                        })),
                    new Action(c =>
                    {
                        if (!InVehicle)
                        {
                            try
                            {
                                LogMessage("info", "Moving to Vehicle {0}", Vehicle.Name);
                                if (!Vehicle.WithinInteractRange)
                                    Navigator.MoveTo(Vehicle.Location);
                                else
                                    Vehicle.Interact();

                                if (IgnoreCombat && StyxWoW.Me.IsAlive)
                                    return RunStatus.Running;
                                else
                                    return RunStatus.Success;
                            }
                            catch { }
                        }
                        return RunStatus.Failure;
                    }),
                    new Decorator(c => UseNavigator && Path == null,
                        new Action(c =>
                        {
                            WoWUnit vehicle = Vehicle;
                            Path = Navigator.GeneratePath(vehicle.Location, Location);
                            if (Path == null || Path.Length == 0)
                            { LogMessage("fatal", "Unable to genorate path to {0}", Location); }

                            if (IgnoreCombat)
                                return RunStatus.Failure;
                            return RunStatus.Success;

                        })),
                    new Action(c =>
                    {
                        if (Vehicle.Location.Distance(Location) > Precision && !ObjectManager.Me.Dead)
                        {
                            WoWMovement.ClickToMove(moveToLocation);
                            if (IgnoreCombat)
                                return RunStatus.Running;
                            else
                                return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    }),
                    new Decorator(c => Vehicle.Location.Distance(Location) <= Precision,
                        CreateSpellBehavior)
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

                TreeRoot.StatusText = string.Format("{0}: {1} while in VehicleId({2}) using {3}",
                                                    this.GetType().Name,
                                                    Location, VehicleIds[0], UseNavigator ? "Navigator" : "Click-To-Move");
            }
        }

        #endregion
    }
}
