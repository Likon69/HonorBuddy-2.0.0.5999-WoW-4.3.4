// Behavior originally contributed by Natfoth.
//
// WIKI DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_BasicVehicleBehavior
//
// QUICK DOX:
//      Allows you to Interact with (e.g., 'right-click') mobs that are nearby.
//
//  Parameters (required, then optional--both listed alphabetically):
//      MountX, MountY, MountZ:     world-coordinates where the toon should be standing to enter the vehicle
//      VehicleId:  Id of the Vehicle we seek to mount
//      X, Y, Z:    world-coordinates for the vehicle's destination.
//
//      QuestId [Default:none]:
//      QuestCompleteRequirement [Default:NotComplete]:
//      QuestInLogRequirement [Default:InLog]:
//              A full discussion of how the Quest* attributes operate is described in
//              http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
//      SpellId [Default:none]: Spell to cast (if any) once the vehicle arrives at its destination
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class BasicVehicleBehaviour : CustomForcedBehavior
    {
        public BasicVehicleBehaviour(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                LogMessage("warning", "*****\n"
                                        + "* THIS BEHAVIOR IS DEPRECATED, and will be retired on July 31th 2012.\n"
                                        + "*\n"
                                        + "* BasicVehicleBehavior adds _no_ _additonal_ _value_ over the VehicleMover behavior.\n"
                                        + "* Please update the profile to use the VehicleMover behavior."
                                        + "*****");

                LocationDest = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, new[] { "Dest" }) ?? WoWPoint.Empty;
                LocationMount = GetAttributeAsNullable<WoWPoint>("Mount", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                SpellCastId = GetAttributeAsNullable<int>("SpellId", false, ConstrainAs.SpellId, null) ?? 0;
                VehicleId = GetAttributeAsNullable<int>("VehicleId", true, ConstrainAs.VehicleId, null) ?? 0;

                MountedPoint = WoWPoint.Empty;
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
        public WoWPoint LocationDest { get; private set; }
        public WoWPoint LocationMount { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public int SpellCastId { get; private set; }
        public int VehicleId { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;
        private List<WoWUnit> _vehicleList;

        // Private properties
        private int Counter { get; set; }
        public bool IsMounted { get; set; }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        public WoWPoint MountedPoint { get; private set; }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: BasicVehicleBehaviour.cs 229 2012-04-25 01:57:29Z natfoth $"); } }
        public override string SubversionRevision { get { return ("$Revision: 229 $"); } }


        ~BasicVehicleBehaviour()
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


        #region Overrides of CustomForcedBehavior

        /// <summary>
        /// A Queue for npc's we need to talk to
        /// </summary>
        //private WoWUnit CurrentUnit { get { return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(unit => unit.Entry == VehicleId); } }

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                    new Decorator(ret => (QuestId != 0 && Me.QuestLog.GetQuestById((uint)QuestId) != null &&
                         Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted),
                        new Action(ret => _isBehaviorDone = true)),

                    new Decorator(ret => Counter >= 1,
                        new Action(ret => _isBehaviorDone = true)),

                        new PrioritySelector(

                            new Decorator(ret => IsMounted != true && _vehicleList == null,
                                new Action(ctx =>
                                {
                                    WoWPoint destination1 = new WoWPoint(LocationMount.X, LocationMount.Y, LocationMount.Z);
                                    WoWPoint[] pathtoDest1 = Styx.Logic.Pathing.Navigator.GeneratePath(Me.Location, destination1);

                                    foreach (WoWPoint p1 in pathtoDest1)
                                    {
                                        while (!Me.Dead && p1.Distance(Me.Location) > 3)
                                        {
                                            Thread.Sleep(100);
                                            WoWMovement.ClickToMove(p1);
                                        }
                                    }

                                    ObjectManager.Update();
                                    _vehicleList = ObjectManager.GetObjectsOfType<WoWUnit>()
                                      .Where(ret => (ret.Entry == VehicleId) && !ret.Dead).OrderBy(ret => ret.Location.Distance(Me.Location)).ToList();

                                })
                                ),

                            new Decorator(ret => _vehicleList[0] != null && !_vehicleList[0].WithinInteractRange && IsMounted != true,
                                new Action(ret => Navigator.MoveTo(_vehicleList[0].Location))
                                ),

                            new Decorator(ret => StyxWoW.Me.IsMoving,
                                new Action(ret =>
                                {
                                    WoWMovement.MoveStop();
                                    StyxWoW.SleepForLagDuration();
                                })
                                ),

                            new Decorator(ret => IsMounted != true,
                                new Action(ctx =>
                                {

                                    MountedPoint = Me.Location;
                                    _vehicleList[0].Interact();
                                    StyxWoW.SleepForLagDuration();
                                    IsMounted = true;

                                    ObjectManager.Update();
                                    _vehicleList = ObjectManager.GetObjectsOfType<WoWUnit>()
                                      .Where(ret => (ret.Entry == VehicleId) && !ret.Dead).OrderBy(ret => ret.Location.Distance(MountedPoint)).ToList();
                                    Thread.Sleep(3000);
                                })
                                ),

                            new Decorator(ret => IsMounted = true,
                                new Action(ret =>
                                {
                                    WoWPoint destination = new WoWPoint(LocationDest.X, LocationDest.Y, LocationDest.Z);
                                    WoWPoint[] pathtoDest = Styx.Logic.Pathing.Navigator.GeneratePath(_vehicleList[0].Location, destination);

                                    foreach (WoWPoint p in pathtoDest)
                                    {
                                        while (!_vehicleList[0].Dead && p.Distance(_vehicleList[0].Location) > 3)
                                        {
                                            Thread.Sleep(100);
                                            WoWMovement.ClickToMove(p);
                                        }

                                    }

                                    Lua.DoString("CastSpellByID(" + SpellCastId + ")");

                                    Counter++;
                                })
                                ),

                            new Action(ret => LogMessage("debug", string.Empty))
                        )
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
                TreeRoot.GoalText = this.GetType().Name + ": In Progress";
            }
        }

        #endregion
    }
}
