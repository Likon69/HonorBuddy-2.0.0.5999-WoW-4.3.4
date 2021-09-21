using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Plugins;
using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.Logic.Pathing;

namespace HighVoltz
{
    public class FormHBToggle : Form
    {
        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components;

        /// <summary>
        ///   Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"> true if managed resources should be disposed; otherwise, false. </param>
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
        ///   Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._hbKeyBindButton = new System.Windows.Forms.Button();
            this._movementKeyBindButton = new System.Windows.Forms.Button();
            this._label1 = new System.Windows.Forms.Label();
            this._label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HBKeyBindButton
            // 
            this._hbKeyBindButton.BackColor = System.Drawing.SystemColors.Control;
            this._hbKeyBindButton.Location = new System.Drawing.Point(12, 25);
            this._hbKeyBindButton.Name = "_hbKeyBindButton";
            this._hbKeyBindButton.Size = new System.Drawing.Size(117, 25);
            this._hbKeyBindButton.TabIndex = 0;
            this._hbKeyBindButton.Text = "Click to set Keybind";
            this._hbKeyBindButton.UseVisualStyleBackColor = false;
            this._hbKeyBindButton.Click += new System.EventHandler(this.KeyBindButtonClick);
            this._hbKeyBindButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyBindButtonKeyDown);
            // 
            // MovementKeyBindButton
            // 
            this._movementKeyBindButton.BackColor = System.Drawing.SystemColors.Control;
            this._movementKeyBindButton.Location = new System.Drawing.Point(12, 77);
            this._movementKeyBindButton.Name = "_movementKeyBindButton";
            this._movementKeyBindButton.Size = new System.Drawing.Size(117, 25);
            this._movementKeyBindButton.TabIndex = 0;
            this._movementKeyBindButton.Text = "Click to set Keybind";
            this._movementKeyBindButton.UseVisualStyleBackColor = false;
            this._movementKeyBindButton.Click += new System.EventHandler(this.KeyBindButtonClick);
            this._movementKeyBindButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyBindButtonKeyDown);
            // 
            // label1
            // 
            this._label1.AutoSize = true;
            this._label1.Location = new System.Drawing.Point(12, 9);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(58, 13);
            this._label1.TabIndex = 1;
            this._label1.Text = "HB Toggle";
            // 
            // label2
            // 
            this._label2.AutoSize = true;
            this._label2.Location = new System.Drawing.Point(9, 61);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(93, 13);
            this._label2.TabIndex = 1;
            this._label2.Text = "Movement Toggle";
            // 
            // FormHB_Toggle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(141, 113);
            this.Controls.Add(this._movementKeyBindButton);
            this.Controls.Add(this._label2);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._hbKeyBindButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHBToggle";
            this.Text = "Key Binding";
            this.Shown += new System.EventHandler(this.FormHBToggleShown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button _movementKeyBindButton;
        private Label _label1;
        private Label _label2;
        private Button _hbKeyBindButton;

        public FormHBToggle() { InitializeComponent(); }

        private bool _keyBindMode;

        private void KeyBindButtonClick(object sender, EventArgs e)
        {
            if (!_keyBindMode)
            {
                var btn = (Button)sender;
                btn.BackColor = SystemColors.GradientInactiveCaption;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Text = "Press key combination";
                _keyBindMode = true;
            }
        }

        private void KeyBindButtonKeyDown(object sender, KeyEventArgs e)
        {
            // return if not changing keybinds.
            if (_keyBindMode)
            {
                var btn = (Button)sender;
                var key = GetKeyBind((Button)sender, e);
                if (key != Keys.None)
                {
                    switch (btn.Name)
                    {
                        case "_hbKeyBindButton":
                            HBToggle.MySettings.HBKeybind = key;
                            HBToggle.HBToggleKeybind.KeyBind = HBToggle.MySettings.HBKeybind;
                            break;
                        case "_movementKeyBindButton":
                            HBToggle.MySettings.MovementKeybind = key;
                            HBToggle.MovementKeybind.KeyBind = HBToggle.MySettings.MovementKeybind;
                            break;
                    }
                    HBToggle.MySettings.Save();
                    _keyBindMode = false;
                }
            }
        }

        private Keys GetKeyBind(Button sender, KeyEventArgs e)
        {
            // convert modifier keys to string to be displayed on button
            string keybindstring = (e.Shift ? "Shift " : "") + (e.Control ? "Ctrl " : "") + (e.Alt ? "Alt " : "");
            sender.Text = keybindstring;
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu)
            {
                return Keys.None;
            }
            // display numbers in button text correctly.
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                keybindstring += e.KeyValue - 48;
            }
            else
            {
                keybindstring += e.KeyCode.ToString();
            }
            sender.Text = keybindstring;
            sender.FlatStyle = FlatStyle.Standard;
            sender.BackColor = SystemColors.Control;
            return e.KeyData;
        }

        private void FormHBToggleShown(object sender, EventArgs e)
        {
            _hbKeyBindButton.Text = HBToggle.MySettings.HBKeybind == 0 ? "Click to set Keybind" : VkeyToString(HBToggle.MySettings.HBKeybind);
            _movementKeyBindButton.Text = HBToggle.MySettings.MovementKeybind == 0 ? "Click to set Keybind" : VkeyToString(HBToggle.MySettings.MovementKeybind);
        }

        private string VkeyToString(Keys vKey)
        {
            // extract modifier keys                 
            string keybindstring = ((vKey & Keys.Shift) != 0 ? "Shift " : "") +
                                   ((vKey & Keys.Control) != 0 ? "Ctrl " : "") + ((vKey & Keys.Alt) != 0 ? "Alt " : "");
            // Shift = 0x00010000, Ctrl= 0x00020000, Alt = 0x00040000
            // remove modifer bits
            vKey ^= (Keys.Shift & vKey) | (Keys.Control & vKey) | (Keys.Alt & vKey);
            // display numbers in button text correctly.
            switch (vKey)
            {
                case Keys.D0:
                    keybindstring += "0";
                    break;
                case Keys.D1:
                    keybindstring += "1";
                    break;
                case Keys.D2:
                    keybindstring += "2";
                    break;
                case Keys.D3:
                    keybindstring += "3";
                    break;
                case Keys.D4:
                    keybindstring += "4";
                    break;
                case Keys.D5:
                    keybindstring += "5";
                    break;
                case Keys.D6:
                    keybindstring += "6";
                    break;
                case Keys.D7:
                    keybindstring += "7";
                    break;
                case Keys.D8:
                    keybindstring += "8";
                    break;
                case Keys.D9:
                    keybindstring += "9";
                    break;
                default:
                    keybindstring += vKey.ToString();
                    break;
            }
            return keybindstring;
        }
    }


    public class KeyBindClass
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        private Keys _keyBind;
        // these modifer bools are true if our keybind uses them. otherwise false.
        private bool _shiftkey;
        private bool _ctrlkey;
        private bool _altkey;
        public KeyBindClass() { _keyBind = new Keys(); }

        public KeyBindClass(Keys k)
        { // k holds both modifer and  key so we need to extract the modifer
            _shiftkey = (k & Keys.Shift) != 0;
            _ctrlkey = (k & Keys.Control) != 0;
            _altkey = (k & Keys.Alt) != 0;
            // now we remove the modifer flags from k and assign it to _keybind.
            _keyBind = (Keys.Shift & k) | (Keys.Control & k) | (Keys.Alt & k);
        }

        public Keys KeyBind
        {
            get { return _keyBind; }
            set
            { // check for modifer flags 
                _shiftkey = (value & Keys.Shift) != 0;
                _ctrlkey = (value & Keys.Control) != 0;
                _altkey = (value & Keys.Alt) != 0;
                // remove modifer bits
                value ^= (Keys.Shift & value) | (Keys.Control & value) | (Keys.Alt & value);
                _keyBind = value;
            }
        }

        public static bool IsKeyDown(Keys vKey) { return (GetAsyncKeyState(vKey) != 0); }

        private Keys _lastKey;

        public bool PollKeys()
        { // checks button status and returns true if our beybind is pressed.
            if (IsKeyDown(_keyBind) && _lastKey != _keyBind && (!(_shiftkey ^ IsKeyDown(Keys.ShiftKey))) &&
                (!(_ctrlkey ^ IsKeyDown(Keys.ControlKey))) && (!(_altkey ^ IsKeyDown(Keys.Menu))))
            {
                _lastKey = _keyBind;
                return true;
            }
            if (!IsKeyDown(_keyBind))
            {
                _lastKey = Keys.None;
            }
            return false;
        }
    }


    public class HBToggle : HBPlugin
    {
        private readonly FormHBToggle _configForm = new FormHBToggle();
        public override string Name { get { return "HB Toggle"; } }
        public override string Author { get { return "HighVoltz"; } }
        private readonly Version _version = new Version(1, 7);
        public override Version Version { get { return _version; } }
        public override string ButtonText { get { return "KeyBinding"; } }
        public override bool WantButton { get { return true; } }
        internal static KeyBindClass HBToggleKeybind = new KeyBindClass();
        internal static KeyBindClass MovementKeybind = new KeyBindClass();
        internal static HBToggleSettings MySettings;


        // making a new thread because pulse() doesn't get called when bot is paused and it doesn't get called frequently 
        // enough to get acurrate key readings. thread terminates when HB terminates.
        private Thread _keyPollThread;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static readonly IntPtr WowHandle = ObjectManager.WoWProcess.MainWindowHandle;
        // contains info about this plugin, used to determine if this plugin is enabled from my thread loop.
        private static PluginContainer _myPlugin;

        public override void OnButtonPress()
        {
            try
            {
                _configForm.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }

        public override void Pulse() { }

        public override void Initialize()
        {
            try
            {
                MySettings = new HBToggleSettings(Application.StartupPath + "\\Settings\\HBToggle\\HB_Toggle.xml");
                HBToggleKeybind.KeyBind = MySettings.HBKeybind;
                MovementKeybind.KeyBind = MySettings.MovementKeybind;

                _keyPollThread = new Thread(ThreadLoop)
                                 {
                                     IsBackground = true
                                 };
                Logging.Write("Starting Thread: keyPollThread");
                _keyPollThread.Start();
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }


        public override void Dispose() { _keyPollThread.Abort(); }

        // this is being run in a new thread
        public void ThreadLoop()
        {
            while (true)
            {
                try
                {
                    // if keybind is pressed and our wow window is the currently active window then toggle HB
                    IntPtr fgWinHwnd = GetForegroundWindow();
                    if (_myPlugin == null)
                        _myPlugin = PluginManager.Plugins.FirstOrDefault(p => p.Name.Equals(Name));
                    if (HBToggleKeybind.PollKeys() && (WowHandle == fgWinHwnd) &&
                        (_myPlugin != null && _myPlugin.Enabled))
                        ToggleHB();
                    if (MovementKeybind.PollKeys() && (WowHandle == fgWinHwnd) &&
                        (_myPlugin != null && _myPlugin.Enabled))
                        ToggleMovement();
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    Logging.WriteException(ex);
                }
            }
        }

        private static void ToggleHB()
        {
            if (TreeRoot.IsRunning)
            {
                Logging.Write("Honorbuddy: Off");
                TreeRoot.Stop();
                WoWMovement.MoveStop();
                Lua.DoString("UIErrorsFrame:AddMessage(\"HonorBuddy: OFF\", 1.0, 0.4, 0.0)");
            }
            else if (TreeRoot.Current != null)
            {
                Logging.Write("Honorbuddy: On");
                Lua.DoString("UIErrorsFrame:AddMessage(\"HonorBuddy: ON\", 0.0, 1.0, 0.0)");
                TreeRoot.Start();
            }
            else
            {
                Logging.Write("Need to start HonorBuddy 1st");
                Lua.DoString("UIErrorsFrame:AddMessage(\"Err: Start HonorBuddy\", 1.0, 0.0, 0.0)");
            }
        }

        private static bool _isMovementEnabled = true;

        private static void ToggleMovement()
        {
            if (_isMovementEnabled)
            {
                Logging.Write("Movement: Off");
                Lua.DoString("UIErrorsFrame:AddMessage(\"Movement: OFF\", 1.0, 0.4, 0.0)");
                WoWMovement.MoveStop();
                Navigator.PlayerMover = new NoMover();
                Navigator.NavigationProvider.StuckHandler = new NoUnstuck();
                _isMovementEnabled = false;
            }
            else
            {
                Logging.Write("Movement: On");
                Lua.DoString("UIErrorsFrame:AddMessage(\"Movement: ON\", 0.0, 1.0, 0.0)");
                Navigator.PlayerMover = new ClickToMoveMover();
                Navigator.NavigationProvider.StuckHandler = OriginalUnstuckHandler;
                _isMovementEnabled = true;
            }
        }

        #region Embedded Type - NoMover

        public class NoMover : IPlayerMover
        {
            public void Move(WoWMovement.MovementDirection direction) { }
            public void MoveStop() { }
            public void MoveTowards(WoWPoint location) { }
        }

        #endregion

        private static readonly IStuckHandler OriginalUnstuckHandler = Navigator.NavigationProvider.StuckHandler;

        #region Embedded Type - NoUnstuck

        public class NoUnstuck : IStuckHandler
        {
            public bool IsStuck() { return false; }
            public void Reset() { }
            public void Unstick() { }
        }

        #endregion
    }
}

#region Settings

namespace HighVoltz
{
    public class HBToggleSettings : Settings
    {
        public HBToggleSettings(string settingsPath)
            : base(settingsPath) { Load(); }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public Keys HBKeybind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public Keys MovementKeybind { get; set; }
    }
}

#endregion