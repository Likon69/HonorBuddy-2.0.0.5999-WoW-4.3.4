// Behavior originally contributed by Natfoth.
//
// WIKI DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_BasicMoveTo
//
// QUICK DOX:
//      Moves the toon to a desired location in the game world.
//      NOTE: This behavior is deprecated--use the built-in <RunTo> element instead.    
//
//  Parameters (required, then optional--both listed alphabetically):
//      X, Y, Z:  World coordinates to which the toon should move.
//
//      DestName [Default:"<X,Y,Z>"]:   a human-readable name of the location to which the toon is moving.
//
using System;
using System.Collections.Generic;
using System.Threading;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors.BasicMoveTo
{
    public class BasicMoveTo : CustomForcedBehavior
    {
        public BasicMoveTo(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                LogMessage("warning", "*****\n"
                                        + "* THIS BEHAVIOR IS DEPRECATED, and may be retired in a near, future release.\n"
                                        + "*\n"
                                        + "* BasicMoveTo adds _no_ _additonal_ _value_ over Honorbuddy's built-in RunTo command.\n"
                                        + "* Please update the profile to use RunTo in preference to the BasicMoveTo Behavior.\n"
                                        + "*****");

                Destination = LegacyGetAttributeAsWoWPoint("Location", false, null, "X/Y/Z")
                                    ?? GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null)
                                    ?? WoWPoint.Empty;
                DestinationName = GetAttributeAs<string>("DestName", false, ConstrainAs.StringNonEmpty, new[] { "Name" }) ?? string.Empty;

                if (string.IsNullOrEmpty(DestinationName))
                { DestinationName = Destination.ToString(); }
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
        public WoWPoint Destination { get; private set; }
        public string DestinationName { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private int Counter { get; set; }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: BasicMoveTo.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~BasicMoveTo()
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

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                    new Decorator(ret => Counter >= 1,
                        new Action(ret => _isBehaviorDone = true)),

                        new PrioritySelector(

                            new Decorator(ret => Counter == 0,
                                new Action(delegate
                                {

                                    WoWPoint destination1 = new WoWPoint(Destination.X, Destination.Y, Destination.Z);
                                    WoWPoint[] pathtoDest1 = Styx.Logic.Pathing.Navigator.GeneratePath(Me.Location, destination1);

                                    foreach (WoWPoint p in pathtoDest1)
                                    {
                                        while (!Me.Dead && p.Distance(Me.Location) > 3)
                                        {
                                            if (Me.Combat)
                                            {
                                                break;
                                            }
                                            Thread.Sleep(100);
                                            WoWMovement.ClickToMove(p);
                                        }

                                        if (Me.Combat)
                                        {
                                            break;
                                        }
                                    }

                                    if (Me.Combat)
                                    {

                                        return RunStatus.Success;
                                    }
                                    else if (!Me.Combat)
                                    {
                                        Counter++;
                                        return RunStatus.Success;
                                    }

                                    return RunStatus.Running;
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
            get { return (_isBehaviorDone); }
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
                TreeRoot.GoalText = this.GetType().Name + ": " + DestinationName;
            }
        }

        #endregion
    }
}

