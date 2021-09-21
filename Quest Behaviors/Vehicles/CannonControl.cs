// Behavior originally contributed by HighVoltz.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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
    /// Shoots a Cannon
    /// ##Syntax##
    /// VehicleId: ID of the vehicle
    /// QuestId: Id of the quest to perform this behavior on
    /// MaxAngle: Maximum Angle to aim, use /dump VehicleAimGetNormAngle() in game to get the angle
    /// MinAngle: Minimum Angle to aim, use /dump VehicleAimGetNormAngle() in game to get the angle
    /// Buttons:A series of numbers that represent the buttons to press in order of importance, separated by comma, for example Buttons ="2,1" 
    /// ExitButton: (Optional)Button to press to exit the cannon. 1-12
    /// </summary>
    public class CannonControl : CustomForcedBehavior
    {
        public CannonControl(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                Buttons = GetAttributeAsArray<int>("Buttons", true, new ConstrainTo.Domain<int>(-1, 12), null, null);
                ExitButton = GetAttributeAsNullable<int>("ExitButton", true, ConstrainAs.HotbarButton, null) ?? 0;
                MaxAngle = GetAttributeAsNullable<double>("MaxAngle", true, new ConstrainTo.Domain<double>(0.0, 1.5), null) ?? 0;
                MinAngle = GetAttributeAsNullable<double>("MinAngle", true, new ConstrainTo.Domain<double>(0.0, 1.5), null) ?? 0;
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                VehicleId = GetAttributeAsNullable<int>("VehicleId", true, ConstrainAs.VehicleId, null) ?? 0;

                ExitButton += 120;

                for (int i = 0; i < Buttons.Length; ++i)
                { Buttons[i] += 120; }
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
        public int[] Buttons { get; private set; }
        public int ExitButton { get; private set; }
        public double MaxAngle { get; private set; }
        public double MinAngle { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public List<int> SpellIds { get; private set; }
        public int VehicleId { get; private set; }

        // Private variables for internal state
        //private bool                _aimed;
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;
        readonly Stopwatch _thottleTimer = new Stopwatch();
        Random rand = new Random();

        // Private properties

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: CannonControl.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~CannonControl()
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


        public WoWObject Vehicle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWObject>(true).Where(o => o.Entry == VehicleId).
                    OrderBy(o => o.Distance).FirstOrDefault();
            }
        }

        private static bool IsInVehicle
        {
            get { return Lua.GetReturnVal<bool>("return UnitInVehicle('player')", 0); }
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
                    new Decorator(c => Vehicle == null,
                            new Action(c => LogMessage("fatal", "No cannons found."))
                        ),

                    new Decorator(c => Vehicle != null && !IsInVehicle,
                        new Action(c =>
                        {
                            if (!Vehicle.WithinInteractRange)
                            {
                                Navigator.MoveTo(Vehicle.Location);
                                LogMessage("info", "Moving to Cannon");
                            }
                            else
                                Vehicle.Interact();
                        })
                    ),
                    new Decorator(c => IsInVehicle && Vehicle != null,
                        new Action(c =>
                        {
                            // looping since current versions of HB seem to be unresponsive for periods of time
                            while (true)
                            {
                                var quest = ObjectManager.Me.QuestLog.GetQuestById((uint)QuestId);
                                if (quest.IsCompleted)
                                {
                                    if (ExitButton > 0)
                                        Lua.DoString("local _,s,_ = GetActionInfo({0}) CastSpellByID(s)", ExitButton);
                                    else
                                        Lua.DoString("VehicleExit()");
                                    _isBehaviorDone = true;
                                    return RunStatus.Success;
                                }
                                else
                                {

                                    using (new FrameLock())
                                    {

                                        Lua.DoString("VehicleAimRequestNormAngle({0})",
                                            MinAngle + (rand.NextDouble() * (MaxAngle - MinAngle)));
                                        foreach (int b in Buttons)
                                        {
                                            //Lua.DoString("local _,s,_ = GetActionInfo({0}) local c = GetSpellCooldown(s) if c == 0 then CastSpellByID(s) end ", b);
                                            //Lua.DoString("local _,s,_ = GetActionInfo({0}) CastSpellByID(s) ", b);
                                            Lua.DoString("local _,s,_ = GetActionInfo({0}) CastSpellByID(s) ", b);
                                        }
                                        //}
                                    }
                                    System.Threading.Thread.Sleep(1000);
                                }

                                _thottleTimer.Reset();
                                _thottleTimer.Start();
                            }
                        }))
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
            }
        }

        #endregion
    }
}
