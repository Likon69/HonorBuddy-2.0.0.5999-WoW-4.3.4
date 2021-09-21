// Behavior originally contributed by Chinajade.
//
// LICENSE:
// This work is licensed under the 
//     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// also known as CC-BY-NC-SA.  To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/3.0/
// or send a letter to
//      Creative Commons
//      171 Second Street, Suite 300
//      San Francisco, California, 94105, USA.
//
// DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_UserDialog
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using System.Xml;

using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;


namespace BuddyWiki.CustomBehavior.UserDialog
{
    // Visual Studio's "Designer" requires the form to be the first class in the file...
    // <sigh> So much for alphabetical class listings.
    class UserDialogForm : Form
    {
        public UserDialogForm(AsyncCompletionToken completionToken,
                              string toonName,
                              string dialogTitle,
                              string dialogMessage,
                              string expiryActionName,
                              int expiryRemainingInSeconds,
                              bool isBotStopAllowed,
                              bool isStopOnContinue,
                              SystemSound soundCue,
                              int soundCuePeriodInSeconds)
        {
            _completionToken = completionToken;
            _completionToken.PopdownResponse = PopdownReason.UNKNOWN;
            _expiryActionHandler = ExpiryActionHandler.GetEnumItemByName(expiryActionName);
            _expiryRemainingInSeconds = expiryRemainingInSeconds;
            _isBotStopAllowed = isBotStopAllowed;
            _isStopOnContinue = isStopOnContinue;
            _soundCue = soundCue;
            _soundCuePeriodInSeconds = soundCuePeriodInSeconds;


            // Dialog creation
            InitializeComponent();

            this.ControlBox = false;    // disable close box for this dialog
            this.MinimizeBox = false;    // disable minimize box for this dialog
            this.MaximizeBox = false;    // disable maximize box for this dialog
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            this.FormClosing += new FormClosingEventHandler(dialogForm_FormClosing);

            heartbeatPulseTimer.Stop();


            // Dialog identity
            this.Text = String.Format("['{0}' UserDialog] {1}", toonName, dialogTitle);
            this.textBoxMessage.Text = dialogMessage.Replace("\\n", Environment.NewLine).Replace("\\t", "\t");
            this.textBoxMessage.SelectionStart = this.textBoxMessage.SelectionLength;
            this.checkBoxAutoDefend.Checked = _completionToken.IsAutoDefend;
            this.buttonStopBot.Enabled = isBotStopAllowed;
            this.labelStatus.Text = "";

            // If only 'stop' allowed, convert the normally 'profile continue' button to 'stop'
            if (_isStopOnContinue)
            {
                this.buttonStopBot.Visible = false;
                this.buttonContinueProfile.Text = "Stop Bot";
            }


            // Setup the Expiry countdown, if enabled--
            // Our pulse timer goes off every second to notify the user how much time
            // remains before the expiry action will be executed.
            if (_expiryRemainingInSeconds > 0)
            {
                _expiryActionHandler.Initialize(this);
                labelStatus.Text = UtilBuildTimeRemainingStatusText(_expiryActionHandler.ActionAsString(), _expiryRemainingInSeconds);
            }


            // Setup the audible warnings --
            // Note: *Never* try to set the System.Windows.Forms.Timer.Interval to zero.
            // Doing so will trigger a Windoze bug that prevents the dialog from
            // opening.
            switch (_soundCuePeriodInSeconds)
            {
                case 0:
                    // Play no sound--nothing to do
                    _soundPeriodRemaining = 0;
                    this.checkBoxSuppressAudio.Enabled = false;
                    break;

                case 1:
                    // Play sound once for dialog open
                    _soundPeriodRemaining = 0;
                    soundCue.Play();
                    this.checkBoxSuppressAudio.Enabled = false;
                    break;

                default:
                    // Play sound now for dialog open, and
                    // arrange to play the sound at the specified intervals
                    _soundPeriodRemaining = _soundCuePeriodInSeconds;
                    soundCue.Play();
                    this.checkBoxSuppressAudio.Enabled = true;
                    break;
            }



            // Our heartbeat pulse handler looks for things like timer expiry, and dialog close requests.
            // Polling techniques like this offend us; however, our choices are limited, since
            // .NET requires the actual processing of a request to occur on the same thread
            // that created the dialog and its widgets.  Yet, .NET provides us with no clean mechanisms
            // for external event notifications other than through timers.
            heartbeatPulseTimer.Interval = 1000;    // one second
            heartbeatPulseTimer.Enabled = true;
        }


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonContinueProfile = new System.Windows.Forms.Button();
            this.buttonStopBot = new System.Windows.Forms.Button();
            this.checkBoxSuppressAudio = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoDefend = new System.Windows.Forms.CheckBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.heartbeatPulseTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.textBoxMessage.Location = new System.Drawing.Point(12, 12);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessage.Size = new System.Drawing.Size(404, 134);
            this.textBoxMessage.TabIndex = 0;
            // 
            // buttonContinueProfile
            // 
            this.buttonContinueProfile.Location = new System.Drawing.Point(315, 152);
            this.buttonContinueProfile.Name = "buttonContinueProfile";
            this.buttonContinueProfile.Size = new System.Drawing.Size(101, 23);
            this.buttonContinueProfile.TabIndex = 1;
            this.buttonContinueProfile.Text = "Continue Profile";
            this.buttonContinueProfile.UseVisualStyleBackColor = true;
            this.buttonContinueProfile.Click += new System.EventHandler(this.buttonContinueProfile_Click);
            // 
            // buttonStopBot
            // 
            this.buttonStopBot.Enabled = false;
            this.buttonStopBot.Location = new System.Drawing.Point(228, 152);
            this.buttonStopBot.Name = "buttonStopBot";
            this.buttonStopBot.Size = new System.Drawing.Size(81, 23);
            this.buttonStopBot.TabIndex = 2;
            this.buttonStopBot.Text = "Stop Bot";
            this.buttonStopBot.UseVisualStyleBackColor = true;
            this.buttonStopBot.Click += new System.EventHandler(this.buttonStopBot_Click);
            // 
            // checkBoxSuppressAudio
            // 
            this.checkBoxSuppressAudio.AutoSize = true;
            this.checkBoxSuppressAudio.Location = new System.Drawing.Point(12, 175);
            this.checkBoxSuppressAudio.Name = "checkBoxSuppressAudio";
            this.checkBoxSuppressAudio.Size = new System.Drawing.Size(197, 17);
            this.checkBoxSuppressAudio.TabIndex = 3;
            this.checkBoxSuppressAudio.Text = "Suppress Periodic Audible Warnings";
            this.checkBoxSuppressAudio.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoDefend
            // 
            this.checkBoxAutoDefend.AutoSize = true;
            this.checkBoxAutoDefend.Location = new System.Drawing.Point(12, 152);
            this.checkBoxAutoDefend.Name = "checkBoxAutoDefend";
            this.checkBoxAutoDefend.Size = new System.Drawing.Size(142, 17);
            this.checkBoxAutoDefend.TabIndex = 4;
            this.checkBoxAutoDefend.Text = "Auto Defend, if attacked";
            this.checkBoxAutoDefend.UseVisualStyleBackColor = true;
            this.checkBoxAutoDefend.CheckedChanged += new System.EventHandler(this.checkBoxAutoDefend_CheckedChanged);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(225, 182);
            this.labelStatus.MinimumSize = new System.Drawing.Size(180, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(180, 13);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // heartbeatPulseTimer
            // 
            this.heartbeatPulseTimer.Interval = 1000;
            this.heartbeatPulseTimer.Tick += new System.EventHandler(this.heartbeatPulseTimer_Tick);
            // 
            // UserDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 204);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.checkBoxAutoDefend);
            this.Controls.Add(this.checkBoxSuppressAudio);
            this.Controls.Add(this.buttonStopBot);
            this.Controls.Add(this.buttonContinueProfile);
            this.Controls.Add(this.textBoxMessage);
            this.Name = "UserDialogForm";
            this.Text = "UserDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private void checkBoxAutoDefend_CheckedChanged(object sender, EventArgs e)
        {
            _completionToken.IsAutoDefend = checkBoxAutoDefend.Checked;
        }


        private void buttonContinueProfile_Click(object sender, EventArgs e)
        {
            if (_isStopOnContinue)
            { _completionToken.PopdownResponse = PopdownReason.BotStopViaUser; }
            else
            { _completionToken.PopdownResponse = PopdownReason.ContinueViaUser; }

            Close();
        }


        private void buttonStopBot_Click(object sender, EventArgs e)
        {
            _completionToken.PopdownResponse = PopdownReason.BotStopViaUser;

            Close();
        }


        public abstract class ExpiryActionHandler : EnumBaseType<ExpiryActionHandler>
        {
            public static readonly ExpiryActionHandler HandleInputDisabled_BotStop = new InputDisabled_BotStop();
            public static readonly ExpiryActionHandler HandleInputDisabled_Continue = new InputDisabled_Continue();
            public static readonly ExpiryActionHandler HandleInputDisabled_EnableInput = new InputDisabled_EnableInput();
            public static readonly ExpiryActionHandler HandleInputEnabled_BotStop = new InputEnabled_BotStop();
            public static readonly ExpiryActionHandler HandleInputEnabled_Continue = new InputEnabled_Continue();


            // Caller-visible methods --
            public abstract string ActionAsString();
            public abstract void Initialize(UserDialogForm userDialogForm);
            public abstract void WrapUp(UserDialogForm userDialogForm);

            public static List<string> GetEnumNames() { return (GetBaseEnumNames()); }
            public static ReadOnlyCollection<ExpiryActionHandler> GetEnumItems() { return (GetBaseEnumItems()); }
            public static ExpiryActionHandler GetEnumItemByName(string name) { return (GetBaseEnumItemByName(name)); }


            // (Non-visible) specific behaviors --
            private class InputDisabled_BotStop : ExpiryActionHandler
            {
                public override string ActionAsString() { return ("Stopping Bot"); }

                public override void Initialize(UserDialogForm userDialogForm)
                {
                    userDialogForm.buttonContinueProfile.Enabled = false;
                    userDialogForm.buttonStopBot.Enabled = false;
                }

                public override void WrapUp(UserDialogForm userDialogForm)
                {
                    userDialogForm._completionToken.PopdownResponse = PopdownReason.BotStopViaExpiry;
                    userDialogForm.Close();
                }
            }

            private class InputDisabled_Continue : ExpiryActionHandler
            {
                public override string ActionAsString() { return ("Continuing"); }

                public override void Initialize(UserDialogForm userDialogForm)
                {
                    userDialogForm.buttonContinueProfile.Enabled = false;
                    userDialogForm.buttonStopBot.Enabled = false;
                }

                public override void WrapUp(UserDialogForm userDialogForm)
                {
                    userDialogForm._completionToken.PopdownResponse = PopdownReason.ContinueViaExpiry;
                    userDialogForm.Close();
                }
            }

            private class InputDisabled_EnableInput : ExpiryActionHandler
            {
                public override string ActionAsString() { return ("User choice enabled"); }

                public override void Initialize(UserDialogForm userDialogForm)
                {
                    userDialogForm.buttonContinueProfile.Enabled = false;
                    userDialogForm.buttonStopBot.Enabled = false;
                }

                public override void WrapUp(UserDialogForm userDialogForm)
                {
                    userDialogForm.buttonContinueProfile.Enabled = true;
                    if (userDialogForm._isBotStopAllowed)
                    { userDialogForm.buttonStopBot.Enabled = true; }
                }
            }

            private class InputEnabled_BotStop : ExpiryActionHandler
            {
                public override string ActionAsString() { return ("Stopping Bot"); }

                public override void Initialize(UserDialogForm userDialogForm)
                {
                    userDialogForm.buttonContinueProfile.Enabled = true;
                    userDialogForm.buttonStopBot.Enabled = true;
                }

                public override void WrapUp(UserDialogForm userDialogForm)
                {
                    userDialogForm._completionToken.PopdownResponse = PopdownReason.BotStopViaExpiry;
                    userDialogForm.Close();
                }
            }

            private class InputEnabled_Continue : ExpiryActionHandler
            {
                public override string ActionAsString() { return ("Continuing"); }

                public override void Initialize(UserDialogForm userDialogForm)
                {
                    userDialogForm.buttonContinueProfile.Enabled = true;
                    userDialogForm.buttonStopBot.Enabled = true;
                }

                public override void WrapUp(UserDialogForm userDialogForm)
                {
                    userDialogForm._completionToken.PopdownResponse = PopdownReason.ContinueViaExpiry;
                    userDialogForm.Close();
                }
            }
        }


        private void dialogForm_FormClosing(object sender, FormClosingEventArgs evt)
        {
            heartbeatPulseTimer.Enabled = false;
            labelStatus.Text = "";
        }


        private void heartbeatPulseTimer_Tick(object sender, EventArgs e)
        {
            // If we've received a 'pop down' request...
            if (_completionToken.PopdownRequest.IsPopdown())
            {
                _completionToken.PopdownResponse = _completionToken.PopdownRequest;
                Close();
            }


            // If the expiry timer is ticking...
            // Recall that an _expiryRemainingInSeconds of zero means the timer is not running.
            if (_expiryRemainingInSeconds > 0)
            {
                --_expiryRemainingInSeconds;
                labelStatus.Text = UtilBuildTimeRemainingStatusText(_expiryActionHandler.ActionAsString(), _expiryRemainingInSeconds);

                if (_expiryRemainingInSeconds <= 0)
                {
                    // An expiry action doesn't always close the dialog --
                    // Sometimes, it will do things like enable inputs that were disabled.
                    _expiryActionHandler.WrapUp(this);
                    labelStatus.Text = "";
                }
            }


            // If time for audible alert...
            // Recall that a _soundPeriodRemaining of zero means periodic audible alerts are not being issued.
            if (_soundPeriodRemaining > 0)
            {
                --_soundPeriodRemaining;

                if (_soundPeriodRemaining <= 0)
                {
                    if (!checkBoxSuppressAudio.Checked)
                    { _soundCue.Play(); }

                    _soundPeriodRemaining = _soundCuePeriodInSeconds;
                }
            }
        }


        private static string UtilBuildTimeRemainingStatusText(string actionName,
                                                                 int timeRemaining)
        {
            string formatString = "";
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);

            if (timeSpan.Hours > 0)
            { formatString = "{0} in {1:D2}h:{2:D2}m:{3:D2}s."; }
            else if (timeSpan.Minutes > 0)
            { formatString = "{0} in {2:D2}m:{3:D2}s."; }
            else
            { formatString = "{0} in {3:D2}s."; }

            return (string.Format(formatString, actionName, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
        }


        // VS-generated data members
        private System.Windows.Forms.Button buttonContinueProfile;
        private System.Windows.Forms.Button buttonStopBot;
        private CheckBox checkBoxAutoDefend;
        private System.Windows.Forms.CheckBox checkBoxSuppressAudio;
        private System.Windows.Forms.Timer heartbeatPulseTimer;
        private Label labelStatus;
        private System.Windows.Forms.TextBox textBoxMessage;

        // Hand-written data members
        private AsyncCompletionToken _completionToken;
        private ExpiryActionHandler _expiryActionHandler;
        private int _expiryRemainingInSeconds;
        private readonly bool _isBotStopAllowed;
        private readonly bool _isStopOnContinue;
        private readonly SystemSound _soundCue;
        private readonly int _soundCuePeriodInSeconds;
        private int _soundPeriodRemaining;
    }


    public class UserDialog : CustomForcedBehavior
    {
        public UserDialog(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                string[] expiryActionNames = UserDialogForm.ExpiryActionHandler.GetEnumNames().ToArray();
                Dictionary<string, System.Media.SystemSound> soundsAllowed = new Dictionary<string, System.Media.SystemSound>()
                                                                 {
                                                                      { "Asterisk",       System.Media.SystemSounds.Asterisk },
                                                                      { "Beep",           System.Media.SystemSounds.Beep },
                                                                      { "Exclamation",    System.Media.SystemSounds.Exclamation },
                                                                      { "Hand",           System.Media.SystemSounds.Hand },
                                                                      { "Question",       System.Media.SystemSounds.Question },
                                                                 };
                string tmpSoundCueName = "";



                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                DialogText = GetAttributeAs<string>("Text", true, ConstrainAs.StringNonEmpty, null) ?? "";
                DialogTitle = GetAttributeAs<string>("Title", false, ConstrainAs.StringNonEmpty, null) ?? "Attention Required...";
                ExpiryActionName = GetAttributeAs<string>("ExpiryAction", false, new ConstrainTo.SpecificValues<string>(expiryActionNames), null) ?? "InputEnabled_Continue";
                ExpiryTime = GetAttributeAsNullable<int>("ExpiryTime", false, new ConstrainTo.Domain<int>(1, int.MaxValue), null) ?? 0;
                IsBotStopAllowed = GetAttributeAsNullable<bool>("AllowBotStop", false, null, null) ?? false;
                IsStopOnContinue = GetAttributeAsNullable<bool>("StopOnContinue", false, null, null) ?? false;
                QuestId = GetAttributeAsNullable("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                tmpSoundCueName = GetAttributeAs<string>("SoundCue", false, new ConstrainTo.SpecificValues<string>(soundsAllowed.Keys.ToArray()), null) ?? "Asterisk";
                SoundCue = soundsAllowed[tmpSoundCueName];
                SoundCueIntervalInSeconds = GetAttributeAsNullable<int>("SoundCueInterval", false, new ConstrainTo.Domain<int>(0, int.MaxValue), null) ?? 60;

                // Note: we don't want to actually create the dialog here, as that will cause it
                // to popup even if the quest is complete.  This action is properly deferred until
                // OnStart(), which handles IsDone processing.
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
        public string DialogText { get; private set; }
        public string DialogTitle { get; private set; }
        public string ExpiryActionName { get; private set; }
        public int ExpiryTime { get; private set; }
        public bool IsBotStopAllowed { get; private set; }
        public bool IsStopOnContinue { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public System.Media.SystemSound SoundCue { get; private set; }
        public int SoundCueIntervalInSeconds { get; private set; }

        // Private variables for internal state
        private TreeSharp.Composite _behavior;
        private AsyncCompletionToken _completionToken;
        private ConfigMemento _configMemento;
        private bool _isBehaviorDone;
        private bool _isDisposed;

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: UserDialog.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~UserDialog()
        {
            Dispose(false);
        }


        public /*virtual*/ void Dispose(bool isExplicitlyInitiatedDispose)
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
                if (_completionToken != null)
                { _completionToken.Dispose(); }

                if (_configMemento != null)
                { _configMemento.Dispose(); }

                _completionToken = null;
                _configMemento = null;

                BotEvents.OnBotStop -= BotEvents_OnBotStop;
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        private void UserDialogExitProcessing(PopdownReason popdownReason)
        {
            // Log to Honorbuddy why we're exiting behavior...
            if (popdownReason.IsReasonKnown())
            {
                string directiveRequester = (IsStopOnContinue ? "Profile Writer request"
                                                        : popdownReason.IsPopdown() ? "Notification criteria no longer valid"
                                                        : popdownReason.IsTimerExpiry() ? "Profile Writer request"
                                                        : popdownReason.IsUserResponse() ? "User request"
                                                        : "Profile Writer request");
                string messageType = (popdownReason.IsTimerExpiry() ? "timer expired"
                                                        : popdownReason.IsUserResponse() ? "user response"
                                                        : popdownReason.IsPopdown() ? "completion criteria"
                                                        : "info");
                string terminationMessage = string.Format("{0} {1}",
                                                                    (popdownReason.IsBotStop() ? "Honorbuddy stopped due to "
                                                                     : "Continuing profile due to"),
                                                                     directiveRequester);

                TreeRoot.StatusText = terminationMessage;
                LogMessage(messageType, terminationMessage);
            }

            if (popdownReason.IsBotStop())
            { TreeRoot.Stop(); }
        }


        private void BotEvents_OnBotStop(EventArgs args)
        {
            UserDialogExitProcessing(PopdownReason.UNKNOWN);
            Dispose();
        }


        #region Overrides of CustomForcedBehavior

        protected override TreeSharp.Composite CreateBehavior()
        {
            if (_behavior == null)
            {
                _behavior = new TreeSharp.PrioritySelector(
                    new TreeSharp.Action(ret =>
                    {
                        bool isDialogComplete = _completionToken.IsComplete;
                        bool isProgressing = UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete);

                        // We're complete... wrap it up
                        if (isDialogComplete || !isProgressing)
                        {
                            // If we're no longer progressing, we don't want to wait for the user response to close up shop...
                            // Thus, we use UNKNOWN when not progressing.  This will also keep us from blocking and waiting for
                            // PopdownResponse to be populated by the UserDialogForm we're trying to close.
                            UserDialogExitProcessing(isDialogComplete
                                                     ? _completionToken.PopdownResponse
                                                     : PopdownReason.PopdownCompletionCriteriaMet);

                            _isBehaviorDone = true;
                            return (TreeSharp.RunStatus.Success);
                        }

                        // If 'auto defend' is on and we're in combat, we skip this node to allow combat to take place
                        // somewhere in our parent's subtree
                        if (_completionToken.IsAutoDefend && StyxWoW.Me.IsActuallyInCombat)
                            return (TreeSharp.RunStatus.Failure);


                        // 'auto defend is off'...
                        // RunStatus.Running returns us to this node.  This allows the user to control everything manually--
                        // including combat
                        return (TreeSharp.RunStatus.Running);
                    })
                );
            }

            return (_behavior);
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
                // The ConfigMemento() class captures the user's existing configuration.
                // After its captured, we can change the configuration however needed.
                // When the memento is dispose'd, the user's original configuration is restored.
                // More info about how the ConfigMemento applies to saving and restoring user configuration
                // can be found here...
                //     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_Saving_and_Restoring_User_Configuration
                _configMemento = new ConfigMemento();

                BotEvents.OnBotStop += BotEvents_OnBotStop;

                // We don't want the bot running around harvesting while the user
                // is manually controlling it trying to get the task completed.
                // (We've already captured the existing configuration which will
                //  be restored when this behavior exits, or the bot is stopped.
                //  So there are no worries about destroying user's configuration.)
                CharacterSettings.Instance.HarvestHerbs = false;
                CharacterSettings.Instance.HarvestMinerals = false;
                CharacterSettings.Instance.LootChests = false;
                CharacterSettings.Instance.LootMobs = false;
                CharacterSettings.Instance.NinjaSkin = false;
                CharacterSettings.Instance.SkinMobs = false;

                _completionToken = new AsyncCompletionToken(StyxWoW.Me.Name,
                                                            DialogTitle,
                                                            DialogText,
                                                            ExpiryActionName,
                                                            ExpiryTime,
                                                            IsBotStopAllowed,
                                                            IsStopOnContinue,
                                                            SoundCue,
                                                            SoundCueIntervalInSeconds);

                TreeRoot.GoalText = "User Attention Required...";
                TreeRoot.StatusText = "Waiting for user dialog to close";
            }
        }

        #endregion
    }


    //============================================================
    // The remainder of this file are support classes to get the work done --
    //

    // We know this is clumsy, but its a .NET-ism --
    // Form widgets can *only* be controlled by the thread that creates them.
    // Thus, we make this guarantee by wrapping all creation & processing actions
    // in a method called by just one thread.  <sigh>
    class AsyncCompletionToken : IDisposable
    {
        public AsyncCompletionToken(string toonName,
                                    string dialogTitle,
                                    string dialogMessage,
                                    string expiryActionName,
                                    int expiryRemainingInSeconds,
                                    bool isBotStopAllowed,
                                    bool isStopOnContinue,
                                    SystemSound soundCue,
                                    int soundPeriodInSeconds)
        {
            _dialogDelegate = new UserDialogDelegate(PopupDialogViaDelegate);
            _isAutoDefend = true;
            _popdownRequest = PopdownReason.UNKNOWN;
            _popdownResponse = PopdownReason.UNKNOWN;

            _actResult = _dialogDelegate.BeginInvoke(this,
                                                     toonName,
                                                     dialogTitle,
                                                     dialogMessage,
                                                     expiryActionName,
                                                     expiryRemainingInSeconds,
                                                     isBotStopAllowed,
                                                     isStopOnContinue,
                                                     soundCue,
                                                     soundPeriodInSeconds,
                                                     null,
                                                     null);
        }


        public void Dispose()
        {
            PopdownRequest = PopdownReason.PopdownAndDispose;
            WaitForComplete();
        }


        // We know this is clumsy, but its a .NET-ism --
        // Form widgets can *only* be controlled by the thread that creates them.
        // Thus, we make this guarantee by wrapping all creation & processing actions
        // in a method called by just one thread.  <sigh>
        private static void PopupDialogViaDelegate(AsyncCompletionToken completionToken,
                                                   string toonName,
                                                   string dialogTitle,
                                                   string dialogMessage,
                                                   string expiryActionName,
                                                   int expiryRemainingInSeconds,
                                                   bool isBotStopAllowed,
                                                   bool isStopOnContinue,
                                                   SystemSound soundCue,
                                                   int soundPeriodInSeconds)
        {
            UserDialogForm dialogForm = new UserDialogForm(completionToken,
                                                                     toonName,
                                                                     dialogTitle,
                                                                     dialogMessage,
                                                                     expiryActionName,
                                                                     expiryRemainingInSeconds,
                                                                     isBotStopAllowed,
                                                                     isStopOnContinue,
                                                                     soundCue,
                                                                     soundPeriodInSeconds);

            // Popup the window--
            // We'd *really* like to make this dialog a child of the main Honorbuddy window.
            // By doing such, the dialog would be 'brought to the front' any time te Honorbuddy main
            // window was.
            // Alas, C#/WindowsForms disallows this because the main HB GUI and this dialog are
            // on separate threads.
            dialogForm.TopMost = true;
            dialogForm.Activate();
            dialogForm.ShowDialog();
        }


        // IsAutoDefend is updated by UserDialogForm any time the user alters the corresponding
        // checkbox on the UserDialogForm.
        public bool IsAutoDefend
        {
            get { lock (_stateLock) { return (_isAutoDefend); } }
            set { lock (_stateLock) { _isAutoDefend = value; } }
        }


        // This is a request to the UserDialogForm, asking it to close
        // for the provided reason.
        // Note that UserDialogForm may pop down for other reasons, such
        // as the user clicking a button, or the expiry timer elapsing.
        public PopdownReason PopdownRequest
        {
            get { lock (_stateLock) { return (_popdownRequest); } }
            set { lock (_stateLock) { _popdownRequest = value; } }
        }


        // This is a response from the UserDialogForm, making available
        // the reason that it popped down.  If the PopdownResponse is not
        // yet available, accessing this value will block the caller until
        // it becomes available.  If the caller doesn't want to be blocked,
        // use the IsComplete property to determine whether or not to
        // read this one.
        public PopdownReason PopdownResponse
        {
            get
            {
                WaitForComplete();

                lock (_stateLock) { return (_popdownResponse); }
            }
            set { lock (_stateLock) { _popdownResponse = value; } }
        }


        public bool IsComplete
        {
            get { return (_actResult.IsCompleted); }
        }

        public void WaitForComplete()
        {
            if (!IsComplete)
                _dialogDelegate.EndInvoke(_actResult);
        }


        private IAsyncResult _actResult;
        private UserDialogDelegate _dialogDelegate;
        private bool _isAutoDefend;
        private PopdownReason _popdownRequest;
        private PopdownReason _popdownResponse;
        private readonly object _stateLock = new object();
    }


    enum PopdownReason
    {
        BotStopViaExpiry,
        BotStopViaUser,
        ContinueViaExpiry,
        ContinueViaUser,
        PopdownAndDispose,
        PopdownCompletionCriteriaMet,
        UNKNOWN,
    };

    static class PopdownReason_ExtensionMethods
    {
        public static bool IsBotStop(this PopdownReason popdownReason)
        {
            return ((popdownReason == PopdownReason.BotStopViaExpiry)
                    || (popdownReason == PopdownReason.BotStopViaUser));
        }

        public static bool IsContinue(this PopdownReason popdownReason)
        {
            return ((popdownReason == PopdownReason.ContinueViaExpiry)
                    || (popdownReason == PopdownReason.ContinueViaUser));
        }

        public static bool IsReasonKnown(this PopdownReason popdownReason)
        {
            return (popdownReason != PopdownReason.UNKNOWN);
        }

        public static bool IsPopdown(this PopdownReason popdownReason)
        {
            return ((popdownReason == PopdownReason.PopdownAndDispose)
                    || (popdownReason == PopdownReason.PopdownCompletionCriteriaMet));
        }

        public static bool IsTimerExpiry(this PopdownReason popdownReason)
        {
            return ((popdownReason == PopdownReason.BotStopViaExpiry)
                    || (popdownReason == PopdownReason.ContinueViaExpiry));
        }


        public static bool IsUserResponse(this PopdownReason popdownReason)
        {
            return ((popdownReason == PopdownReason.BotStopViaUser)
                    || (popdownReason == PopdownReason.ContinueViaUser));
        }
    }


    delegate void UserDialogDelegate(AsyncCompletionToken completionToken,
                                            string toonName,
                                            string dialogTitle,
                                            string dialogMessage,
                                            string expiryActionName,
                                            int expiryRemainingInSeconds,
                                            bool isBotStopAllowed,
                                            bool isStopOnContinue,
                                            SystemSound soundCue,
                                            int soundPeriodInSeconds);


    //============================================================
    // Candidate functionality for CustomForcedBehavior base class --


    // using SystemCollections.ObjectModel

    /// <summary>
    /// Base class for the classic enumeration pattern.  The enumeration pattern allows
    /// behavior (e.g., methods) to be associated with each enumerable item that is
    /// defined.
    /// 
    /// The enumeration pattern has these characteristics:
    /// * This technique is *fast*--no reflection is involved
    /// * Any number of methods can be associatd with each enumerated item
    /// * Strongly typed
    /// * Enforces type safety against accidental misuse
    /// * Can be used in If statements and iterated over
    /// * Cannot be used in a switch statement. This is a feature--switch statements
    ///   are not object-oriented and a major source of maintenance errors.
    ///   As enumerations are added, 'default' cases in switch statements mask
    ///   the omission of their handling.
    ///   
    ///  A good tutorial on this pattern can be found here...
    ///      http://www.codeproject.com/KB/cs/EnhancedEnums.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumBaseType<T> where T : EnumBaseType<T>
    {
        protected EnumBaseType()
        {
            _enumValues.Add(this.GetType().Name, (T)this);
        }

        public static List<string> GetBaseEnumNames()
        {
            return (_enumValues.Keys.ToList());
        }

        public static ReadOnlyCollection<T> GetBaseEnumItems()
        {
            return (_enumValues.Values.ToList().AsReadOnly());
        }

        public static T GetBaseEnumItemByName(string name)
        {
            T returnValue;

            if (_enumValues.TryGetValue(name, out returnValue))
            { return (returnValue); }

            throw (new ArgumentException(string.Format("Invalid enum item name ('{0}') provided for {1}",
                                                       name,
                                                       typeof(T).Name)));
        }

        private static Dictionary<string, T> _enumValues = new Dictionary<string, T>();
    }
}
