// Behavior originally contributed by AknA.
// This is an variation of WaitTimer done by Nesox.
// InstanceTimer is a Quest Behavior developed to prevent that you get "You've entered too many instances".
// When you enter a instance you start the timer.
// When you have done your instance run you check the timer to see how long you have been in the instance.
// Calculated from that InstanceTimer will create a WaitTimer from that.
// 
// To start the timer use :
// <CustomBehavior File="Misc\InstanceTimer" Timer="Start" />
//
// To check how long you have been in instance and create a wait timer use:
// <CustomBehavior File="Misc\InstanceTimer" Timer="Check" />
// 
// The default wait time is 12min 30sec - the time you spent in instance.
// If you want to alter the wait time use :
// <CustomBehavior File="Misc\InstanceTimer" Timer="Check" WaitTime="10000" />
// WaitTime is in milliseconds and in above case is 10 seconds - the time you spent in instance.
//
using System;
using System.Collections.Generic;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using TreeSharp;
using Action = TreeSharp.Action;



namespace Styx.Bot.Quest_Behaviors.InstanceTimer
{
    /// <summary>
    /// InstanceTimer by AknA
    /// ##Syntax##
    /// Timer: Start, Check
    /// WaitTime (send with Check): time (in milliseconds) to wait sience Timer was started (default : 12min 30sec)
    /// </summary>
    public class InstanceTimer : CustomForcedBehavior
    {
        public InstanceTimer(Dictionary<string, string> args)
        : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                GoalText = GetAttributeAs("GoalText", false, ConstrainAs.StringNonEmpty, null) ?? "Waiting for {TimeRemaining}  of  {TimeDuration}";
                Timer = GetAttributeAs("Timer", true, ConstrainAs.StringNonEmpty, null) ?? "Start";
                WaitTime = GetAttributeAsNullable("WaitTime", false, ConstrainAs.Milliseconds, null) ?? 750000;
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
        public string GoalText { get; private set; }
        public int WaitTime { get; private set; }
        public string Timer { get; private set; }

        // Private variables for internal state
        private bool _isDisposed;
        private Composite _root;
        private WaitTimer _timer;
        private WaitTimer _timeInInstance;
        private string _waitTimeAsString;

        ~InstanceTimer()
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
                if (isExplicitlyInitiatedDispose) { }  // empty, for now

                // Clean up unmanaged resources (if any) here...
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }

        private string UtilSubstituteInMessage(string message)
        {
            message = message.Replace("{TimeRemaining}", UtilBuildTimeAsString(_timer.TimeLeft));
            message = message.Replace("{TimeDuration}", _waitTimeAsString);

            return (message);
        }

        private static string UtilBuildTimeAsString(TimeSpan timeSpan)
        {
            var formatString = "";
            if (timeSpan.Hours > 0) { formatString = "{0:D2}h:{1:D2}m:{2:D2}s"; }
            else if (timeSpan.Minutes > 0) { formatString = "{1:D2}m:{2:D2}s"; }
            else { formatString = "{2:D2}s"; }
            return (string.Format(formatString, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new Decorator(ret => !_timer.IsFinished,
                    new Sequence(
                        new Action(ret => TreeRoot.GoalText = (!string.IsNullOrEmpty(GoalText)
                                                            ? UtilSubstituteInMessage(GoalText)
                                                            : "Waiting for timer expiration")),
                        new Action(ret => TreeRoot.StatusText = "Wait time remaining... "
                                                + UtilBuildTimeAsString(_timer.TimeLeft)
                                                + "... of "
                                                + _waitTimeAsString),
                        new Action(delegate { return RunStatus.Success; })
                    )
                )
            );
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override bool IsDone { get { return ((_timer != null) && _timer.IsFinished); } }

        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            if (!IsDone)
            {
                if (Timer == "Start")
                {
                    WaitTime = 0;
                    Lua.DoString("StartInstanceTimerMin = date(\"%M\")");
                    Lua.DoString("StartInstanceTimerSec = date(\"%S\")");
                    Logging.Write("[InstanceTimer]: Started.");
                }
                if (Timer == "Check")
                {
                    var startInstanceVar1 = Lua.GetReturnVal<int>("return StartInstanceTimerMin", 0);
                    var startInstanceVar2 = Lua.GetReturnVal<int>("return StartInstanceTimerSec", 0);
                    Lua.DoString("EndInstanceTimerMin = date(\"%M\")");
                    Lua.DoString("EndInstanceTimerSec = date(\"%S\")");
                    var endInstanceVar1 = Lua.GetReturnVal<int>("return EndInstanceTimerMin", 0);
                    var endInstanceVar2 = Lua.GetReturnVal<int>("return EndInstanceTimerSec", 0);
                    if (endInstanceVar1 < startInstanceVar1) { endInstanceVar1 = endInstanceVar1 + 60; }
                    if (endInstanceVar2 < startInstanceVar2)
                    {
                        endInstanceVar2 = endInstanceVar2 + 60;
                        endInstanceVar1 = endInstanceVar1 - 1;
                    }
                    var calcInstanceVar = ((((endInstanceVar1 - startInstanceVar1) * 60) + (endInstanceVar2 - startInstanceVar2)) * 1000);
                    _timeInInstance = new WaitTimer(new TimeSpan(0, 0, 0, 0, calcInstanceVar));
                    var timeInInstanceAsString = UtilBuildTimeAsString(_timeInInstance.WaitTime);
                    Logging.Write("[InstanceTimer]: Your instance run took " + timeInInstanceAsString);
                    if (calcInstanceVar >= WaitTime) { WaitTime = 0; }
                    if (calcInstanceVar < WaitTime)
                    {
                        WaitTime = WaitTime - calcInstanceVar;
                        _timeInInstance = new WaitTimer(new TimeSpan(0, 0, 0, 0, WaitTime));
                        timeInInstanceAsString = UtilBuildTimeAsString(_timeInInstance.WaitTime);
                        Logging.Write("[InstanceTimer]: Waiting for " + timeInInstanceAsString);
                    }
                }
                _timer = new WaitTimer(new TimeSpan(0, 0, 0, 0, WaitTime));
                _waitTimeAsString = UtilBuildTimeAsString(_timer.WaitTime);
                _timer.Reset();
            }
        }
        #endregion
    }
}
