// Behavior originally contributed by Bobby53.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Drawing;

using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;

using TreeSharp;


namespace Styx.Bot.Quest_Behaviors
{
    public class Halt : CustomForcedBehavior
    {
        /// <summary>
        /// Stops the Quest Bot.  Will write 'Msg' to the log and Goal Text.
        /// Also write the line number it halted at for easily locating in profile.
        /// 
        /// Useful for testing assumptions in quest profile and during profile
        /// development to force profile to automatically stop at designated point
        /// 
        /// ##Syntax##
        /// [optional] QuestId: Id of the quest (default is 0)
        /// [optional] Msg: text value to display (default says stopped by profile)
        /// [optional] Color: color to use for message in log (default is red)
        /// 
        /// Note:  QuestId behaves the same as on every other behavior.  If 0, then
        /// halt always occurs.  Otherwise, for non-zero QuestId only halts if the
        /// character has the quest and its not completed
        /// </summary>
        public Halt(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                LogMessage("warning", "*****\n"
                                        + "* THIS BEHAVIOR IS DEPRECATED, and will be retired on July 31th 2012.\n"
                                        + "*\n"
                                        + "* Halt adds _no_ _additonal_ _value_ over the UserSettings behavior.\n"
                                        + "* Please update the profile to use the UserSettings behavior and use StopBot."
                                        + "*****");

                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                Color = GetAttributeAsNullable<Color>("Color", false, null, null) ?? Color.Red;
                Message = GetAttributeAs<string>("Message", false, ConstrainAs.StringNonEmpty, new[] { "Msg", "Text" }) ?? "Quest Profile HALT";
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
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
        public Color Color { get; private set; }
        public string Message { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        private bool _isDisposed;

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: Halt.cs 229 2012-04-25 01:57:29Z natfoth $"); } }
        public override string SubversionRevision { get { return ("$Revision: 229 $"); } }


        ~Halt()
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
            return (null);
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
                return (!UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
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
                LogMessage("", Color, "\n\n    " + Message + "\n");

                TreeRoot.GoalText = Message;
                TreeRoot.Stop();
            }
        }

        #endregion
    }
}

