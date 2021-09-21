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
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors.MountHyjal
{
    public class BearsUpThere : CustomForcedBehavior
    {
        /// <summary>
        /// BearsUpThere by Bobby53 
        /// 
        /// Completes the vehicle quest http://www.wowhead.com/quest=25462
        /// 
        /// To use, you must use the Ladder at <RunTo  X="5254.562" Y="-1536.917" Z="1361.341" />
        /// Due to how the coordinate system is relative to the vehicle once you enter, it
        /// is setup to only support this specific ladder.  
        /// 
        /// ##Syntax##
        /// QuestId: Id of the quest (default is 0)
        /// [Optional] QuestName: optional quest name (documentation only)
        /// </summary>
        /// 
        public BearsUpThere(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                /* */
                GetAttributeAs<string>("QuestName", false, ConstrainAs.StringNonEmpty, null);      // (doc only - not used)
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
        public bool RunningBehavior = true;

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: BearsUpThere.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        //  LEVEL: -1=unknown, 0=tree top, 1=highest, 2=middle, 3=lowest
        const int LEVEL_BOTTOM = 1;
        const int LEVEL_TOP = 5;
        const int LEVEL_UNKNOWN = 0;
        int _lvlCurrent = LEVEL_UNKNOWN;

        const int AURA_CLIMBING_TREE = 74920;
        const int AURA_IN_TREE = 46598;
        const int CLIMB_UP = 74922;
        const int CLIMB_DOWN_AT_TOP = 75070;
        const int CLIMB_DOWN = 74974;
        const int CHUCK_A_BEAR = 75139;

        /*
            RIGHT SIDE:  isontransport:True, rotation:1.356836,  degrees:77.741
            LEFT SIDE:  isontransport:True, rotation:1.612091,  degrees:92.366
            ENTRY:  isontransport:True, rotation:0.1570796,  degrees:9
         */
        // these are values recorded from tree @ 14:33
        //  ..  when taking the right ladder (while facing tree)
        //  ..  angle while on tree level other than top is always 9
        //  ..  if you are on correct tree and correct side

        const double AIM_ANGLE = -0.97389394044876;
        const double TRAMP_RIGHT_SIDE = 77.741;
        const double TRAMP_LEFT_SIDE = 92.366;


        ~BearsUpThere()
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


        public void Dlog(string format, params object[] args)
        {
            LogMessage("debug", Color.CornflowerBlue, string.Format(format, args));
        }

        private static bool IsInVehicle
        {
            get { return Lua.GetReturnVal<bool>("return UnitInVehicle('player')", 0); }
        }

        private void WaitForCurrentSpell()
        {
            while (StyxWoW.GlobalCooldown)
                Thread.Sleep(100);
            while (StyxWoW.Me.IsCasting)
                Thread.Sleep(100);
        }

        private bool CanCastNow(int spellId)
        {
#if  FIGUERED_OUT_VEHICLE_SPELLS
            if (!SpellManager.HasSpell(spellId))
            {
                Elog("spell manager does not know spellid: {0}", spellId);
                TreeRoot.Stop();
            }

            int stopWaiting = System.Environment.TickCount + 5000;
            while ( !SpellManager.CanCast( spellId) && stopWaiting > Environment.TickCount )
                Thread.Sleep(100);

            return SpellManager.CanCast( spellId );
#else
            WaitForCurrentSpell();
            return true;
#endif
        }

        private RunStatus ClimbUp()
        {
            bool canCast = CanCastNow(CLIMB_UP);
            WoWPoint lastPos = Me.Location;
            Lua.DoString("CastSpellByID({0})", CLIMB_UP);
            WaitForCurrentSpell();
            Thread.Sleep(2000);

            if (Me.Location.Distance(lastPos) != 0)
            {
                Dlog("(Climb Up) moved +{0:F1} yds, pos: {1}", Me.Location.Distance(lastPos), Me.Location);
                if (!IsClimbingTheTree())
                    _lvlCurrent = LEVEL_TOP;
                else
                    _lvlCurrent++;
            }
            else
                Dlog("(Climb Up) no movement UP occurred");

            return RunStatus.Success;
        }

        private RunStatus ClimbDown()
        {
            int spellId;

            // spell id to move down is different if you are at top of tree
            if (IsClimbingTheTree())
                spellId = CLIMB_DOWN;
            else
                spellId = CLIMB_DOWN_AT_TOP;

            WoWPoint lastPos = Me.Location;
            CanCastNow(spellId);
            Lua.DoString("CastSpellByID({0})", spellId);
            WaitForCurrentSpell();

            // wait longer if at top due to UI skin change
            Thread.Sleep(spellId == CLIMB_DOWN_AT_TOP ? 3000 : 2000);

            if (Me.Location.Distance(lastPos) != 0)
            {
                _lvlCurrent--;
                Dlog("(Climb Down) moved -{0:F1} yds, pos: {1}", Me.Location.Distance(lastPos), Me.Location);
            }
            else
                Dlog("(Climb Down) no movement DOWN occurred");

            return RunStatus.Success;
        }

        private double GetAimAngle()
        {
            return Lua.GetReturnVal<double>("return VehicleAimGetAngle()", 0);
        }

        private double GetAimAdjustment()
        {
            return GetAimAngle() - AIM_ANGLE;
        }

        private bool NeedAimAngle()
        {
            return Math.Abs(GetAimAdjustment()) > 0.0001;
        }

        private RunStatus AimAngle()
        {
            double angleAdjust = GetAimAdjustment();
            Dlog("(Aim Angle) adjusting current angle {0} by {1} to {2}", GetAimAngle(), angleAdjust, AIM_ANGLE);

            Lua.DoString("VehicleAimDecrement({0})", angleAdjust);

            StyxWoW.SleepForLagDuration();
            return RunStatus.Success;
        }

        private bool NeedAimDirection()
        {
            double normRotation = TRAMP_LEFT_SIDE > TRAMP_RIGHT_SIDE ? 0 : 360;
            if (Me.Transport.RotationDegrees < TRAMP_RIGHT_SIDE)
                return true;

            if ((Me.Transport.RotationDegrees + normRotation) > (TRAMP_LEFT_SIDE + normRotation))
                return true;

            return false;
        }

        private RunStatus AimDirection()
        {
            double normRotation = TRAMP_LEFT_SIDE > TRAMP_RIGHT_SIDE ? 0 : 360;
            double currRotation = Me.Transport.RotationDegrees;
            Dlog("(AimRotation) Trampoline Boundary - Left Edge: {0}  Right Edge: {1}", TRAMP_LEFT_SIDE, TRAMP_RIGHT_SIDE);

            WoWMovement.MovementDirection whichWay = WoWMovement.MovementDirection.None;
            string dirCmd;

            // left/right - get current direction and turn until on trampoline
            if (Me.Transport.RotationDegrees < TRAMP_RIGHT_SIDE)
            {
                whichWay = WoWMovement.MovementDirection.TurnLeft;
                dirCmd = "TurnLeft";
            }
            else if ((Me.Transport.RotationDegrees + normRotation) > (TRAMP_LEFT_SIDE + normRotation))
            {
                whichWay = WoWMovement.MovementDirection.TurnRight;
                dirCmd = "TurnRight";
            }
            else // if (whichWay == WoWMovement.MovementDirection.None)
            {
                Dlog("(AimRotation) Done, Ending Rotation: {0}", Me.Transport.RotationDegrees);
                return RunStatus.Failure;
            }

            Dlog("(AimRotation) Current Rotation: {0} - {1}", Me.Transport.RotationDegrees, whichWay.ToString().ToUpper());
#if WOWMOVEMENT_TIMED_TURNS_STOPFAILING
            WoWMovement.Move(whichWay, TimeSpan.FromMilliseconds( 10));
            WoWMovement.MoveStop(whichWay);
            // loop until we actually move
            while ( 0.001 > (currRotation - Me.Transport.RotationDegrees ))
               StyxWoW.SleepForLagDuration();
#elif WOWMOVEMENT_TURNS_STOPFAILING
            WoWMovement.Move(whichWay);
            Thread.Sleep(10);
            WoWMovement.MoveStop(whichWay);
            // loop until we actually move
            while ( 0.001 > (currRotation - Me.Transport.RotationDegrees ))
               StyxWoW.SleepForLagDuration();
#else
            // doing LUA calls these because WoWMovement API doesn't stop turning quickly enough
            Lua.DoString(dirCmd + "Start()");
            Thread.Sleep(10);
            Lua.DoString(dirCmd + "Stop()");
#endif
            return RunStatus.Success;
        }

        private RunStatus ChuckBear()
        {
            Dlog("(Chuck-A-Bear) threw bear at trampoline");
            bool canCast = CanCastNow(CHUCK_A_BEAR);
            Lua.DoString("CastSpellByID({0})", CHUCK_A_BEAR);
            WaitForCurrentSpell();
            Thread.Sleep(4000);
            return RunStatus.Success;
        }

        bool IsBearCubInBags()
        {
#if USE_OM
            WoWItem item = ObjectManager.GetObjectsOfType<WoWItem>().Find(unit => unit.Entry == 54439);
#else
            WoWItem item = Me.BagItems.Find(unit => unit.Entry == 54439);
#endif
            return item != null;
        }

        RunStatus LootClosestBear()
        {
            List<WoWUnit> bears =
                   (from o in ObjectManager.ObjectList
                    where o is WoWUnit
                    let unit = o.ToUnit()
                    where
                        unit.Entry == 40240
                        && 15 < unit.WorldLocation.Distance(Me.Transport.WorldLocation)
                    orderby
                        unit.WorldLocation.Distance(Me.Transport.WorldLocation) ascending
                    select unit
                        ).ToList();

            foreach (WoWUnit bear in bears)
            {
                StyxWoW.SleepForLagDuration();

                bear.Target();  // target so we can use LUA func
                bool bChkLua = Lua.GetReturnVal<bool>("return CheckInteractDistance(\"target\", 1)", 0);

                bool bChkInt = bear.WithinInteractRange;
                if (!bChkLua && !bChkInt)
                    continue;

                bear.Interact();
                WaitForCurrentSpell();
                StyxWoW.SleepForLagDuration();

                ObjectManager.Update();
                StyxWoW.SleepForLagDuration();

                if (IsBearCubInBags())
                {
                    LogMessage("info", "(Loot Bear) grabbed a bear to throw");
                    return RunStatus.Success;
                }
            }

            Dlog("(Loot Bear) no bear at level {0}", _lvlCurrent);
            return RunStatus.Failure;
        }

        public bool InTree()
        {
            RunningBehavior = Me.Transport != null;
            //Dlog("Checking Tree: HasAura: " + IsInVehicle + " Is Climbing Tree: " + IsClimbingTheTree());
            return Me.Transport != null || IsClimbingTheTree();
        }

        public bool IsClimbingTheTree()
        {
            return HasAura(AURA_CLIMBING_TREE);
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

        public bool HasAura(int auraId)
        {
            WoWAura aura = (from a in Me.Auras
                            where a.Value.SpellId == auraId
                            select a.Value).FirstOrDefault();

            return Me.HasAura(Styx.Logic.Combat.WoWSpell.FromId(auraId).Name);
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                    // check if we left tree/vehicle
                    new Decorator(ret => !InTree(), new Action(ret => _isBehaviorDone = true)),

                    // is quest abandoned or complete?
                //  ..  move down until we auto-exit vehicle
                    new Decorator(ret => !DoWeHaveQuest() || IsQuestComplete(), new Action(ret => ClimbDown())),

                    // level unknown and already at top?  set to top then
                    new Decorator(ret => _lvlCurrent == LEVEL_UNKNOWN && !IsClimbingTheTree(),
                                    new Action(delegate
                                        {
                                            _lvlCurrent = LEVEL_TOP;
                                            return RunStatus.Success;
                                        })),

                    // level unknown?
                //  ..  move to top and establish known level
                    new Decorator(ret => _lvlCurrent == LEVEL_UNKNOWN, new Action(ret => ClimbUp())),

                    // have a bear in inventory?
                    new Decorator(ret => IsBearCubInBags(),
                        new PrioritySelector(
                //  ..  below top?  move up
                            new Decorator(ret => _lvlCurrent != LEVEL_TOP, new Action(ret => ClimbUp())),
                //  ..  aim trajectory angle
                            new Decorator(ret => NeedAimAngle(), new Action(ret => AimAngle())),
                //  ..  aim direction (left/right)
                            new Decorator(ret => NeedAimDirection(), new Action(ret => AimDirection())),
                //  ..  throw                           
                            new Action(ret => ChuckBear())
                            )
                        ),

                    // at top with no bear?
                //  ..  move down
                    new Decorator(ret => _lvlCurrent == LEVEL_TOP, new Action(ret => ClimbDown())),

                    // lootable bears here?
                //  ..  loot a bear
                    new Decorator(ret => !IsBearCubInBags(), new Action(ret => LootClosestBear())),

                    // can we move down without leaving vehicle?
                    new Decorator(ret => _lvlCurrent > LEVEL_BOTTOM, new Action(ret => ClimbDown())),

                    // move up
                    new Decorator(ret => _lvlCurrent < LEVEL_TOP, new Action(ret => ClimbUp()))
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
                return (!RunningBehavior && (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete)));
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
                if (DoWeHaveQuest() && !IsQuestComplete() && !InTree())
                {
                    LogMessage("fatal", "==================================================================\n"
                                        + "NOT IN TREE!!!  ENTER TREE TO USE CUSTOM BEHAVIOR\n"
                                        + "==================================================================");
                }

                else
                {
                    PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                    TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
                }
            }
        }

        #endregion
    }
}

