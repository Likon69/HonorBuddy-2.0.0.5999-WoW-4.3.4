using System;
using System.Drawing;
using System.Windows.Forms;

namespace CLU.GUI
{
    using System.Collections.Generic;
    using System.Reflection;

    using global::CLU.Helpers;

    public partial class Configuration : Form
    {
        public static Configuration instance = new Configuration();
        private static CLU ulc = null;

        private Configuration()
        {
            InitializeComponent();

            paint();

            Version_Label.Text = string.Format("Version: {0}", CLU.Version);


            // keybinds - Populate
            kb_multidotmanage_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_healdefensemanage_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_interuptmanage_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_raidpartybuff_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_aoemanage_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_dsextrabutton_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_cooldownmanage_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_pauserotation_cmbo.DataSource = EnumWithName<Keybinds.Keyboardfunctions>.ParseEnum();
            kb_multidotmanage_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_healdefensemanage_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_interuptmanage_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_raidpartybuff_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_aoemanage_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_dsextrabutton_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_cooldownmanage_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;
            kb_pauserotation_cmbo.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cooldown_Combo.SelectedIndex = cooldown_Combo.Items.IndexOf(SettingsFile.Instance.HandleCooldowns ? "Automatic" : "Manual");
            this.dsextrabuttonclick_combo.SelectedIndex = dsextrabuttonclick_combo.Items.IndexOf(SettingsFile.Instance.Handleextraactionbutton ? "Automatic" : "Manual");
            this.spriestrotation_combo.SelectedIndex = spriestrotation_combo.Items.IndexOf(SettingsFile.Instance.HandleSpriestRotSelector ? "Default" : "MindSpike");
            this.aoeManagement_Combo.SelectedIndex = aoeManagement_Combo.Items.IndexOf(SettingsFile.Instance.HandleAoE ? "Automatic" : "Manual");
            this.raidpartybuffmanage_combo.SelectedIndex = raidpartybuffmanage_combo.Items.IndexOf(SettingsFile.Instance.HandleRaidPartyBuff ? "Automatic" : "Manual");
            this.interruptmanagent_combo.SelectedIndex = interruptmanagent_combo.Items.IndexOf(SettingsFile.Instance.HandleInterrupts ? "Automatic" : "Manual");
            this.healingmanage_combo.SelectedIndex = healingmanage_combo.Items.IndexOf(SettingsFile.Instance.HandleHealing ? "Automatic" : "Manual");
            this.banemanagement_combo.SelectedIndex = banemanagement_combo.Items.IndexOf(SettingsFile.Instance.HandleBaneOfHavoc ? "Automatic" : "Manual");
            this.multidotting_combo.SelectedIndex = multidotting_combo.Items.IndexOf(SettingsFile.Instance.HandleMultiDotting ? "Automatic" : "Manual");
            this.shoutmanagement_combo.SelectedIndex = shoutmanagement_combo.Items.IndexOf(SettingsFile.Instance.HandleWarriorShout ? "Commanding" : "Battle");
            this.clientlag_combo.SelectedIndex = clientlag_combo.Items.IndexOf(SettingsFile.Instance.HandleClientLag ? "Default" : "Detect");
            this.totemmanage_combo.SelectedIndex = totemmanage_combo.Items.IndexOf(SettingsFile.Instance.HandleTotems ? "Automatic" : "Manual");
            this.handlemovement_combo.SelectedIndex = handlemovement_combo.Items.IndexOf(SettingsFile.Instance.HandleMovement ? "Yes" : "No");
            

            // Keybinds - Display saved keybind
            this.kb_multidotmanage_cmbo.SelectedIndex = kb_multidotmanage_cmbo.FindStringExact(SettingsFile.Instance.KeybindMultiDottingManagement != string.Empty ? SettingsFile.Instance.KeybindMultiDottingManagement : "Nothing");
            this.kb_healdefensemanage_cmbo.SelectedIndex = kb_healdefensemanage_cmbo.FindStringExact(SettingsFile.Instance.KeybindHealDefensiveManagement != string.Empty ? SettingsFile.Instance.KeybindHealDefensiveManagement : "Nothing");
            this.kb_interuptmanage_cmbo.SelectedIndex = kb_interuptmanage_cmbo.FindStringExact(SettingsFile.Instance.KeybindInteruptManagement != string.Empty ? SettingsFile.Instance.KeybindInteruptManagement : "Nothing");
            this.kb_raidpartybuff_cmbo.SelectedIndex = kb_raidpartybuff_cmbo.FindStringExact(SettingsFile.Instance.KeybindRaidPartyBuffManagement != string.Empty ? SettingsFile.Instance.KeybindRaidPartyBuffManagement : "Nothing");
            this.kb_aoemanage_cmbo.SelectedIndex = kb_aoemanage_cmbo.FindStringExact(SettingsFile.Instance.KeybindAoEManagement != string.Empty ? SettingsFile.Instance.KeybindAoEManagement : "Nothing");
            this.kb_dsextrabutton_cmbo.SelectedIndex = kb_dsextrabutton_cmbo.FindStringExact(SettingsFile.Instance.KeybindDsExtraButtonClick != string.Empty ? SettingsFile.Instance.KeybindDsExtraButtonClick : "Nothing");
            this.kb_cooldownmanage_cmbo.SelectedIndex = kb_cooldownmanage_cmbo.FindStringExact(SettingsFile.Instance.KeybindCooldownManagement != string.Empty ? SettingsFile.Instance.KeybindCooldownManagement : "Nothing");
            this.kb_pauserotation_cmbo.SelectedIndex = kb_pauserotation_cmbo.FindStringExact(SettingsFile.Instance.KeybindPauseRotation != string.Empty ? SettingsFile.Instance.KeybindPauseRotation : "Nothing");

            this.FormClosing += this.SaveSettings;
        }

        public static void Display(CLU ulc)
        {
            Configuration.ulc = ulc;
            if (instance == null || instance.IsDisposed)
            {
                instance = new Configuration();
            }

            if (!instance.Visible)
            {
                SetDoubleBuffered(instance.panel1);
                instance.Show();
                instance.timer1.Start();
            }
        }

        private void SaveSettings(object sender, EventArgs e)
        {
            SettingsFile.Instance.HandleCooldowns = this.cooldown_Combo.Text == "Automatic";
            SettingsFile.Instance.Handleextraactionbutton = this.dsextrabuttonclick_combo.Text == "Automatic";
            SettingsFile.Instance.HandleSpriestRotSelector = this.spriestrotation_combo.Text == "Default";
            SettingsFile.Instance.HandleAoE = this.aoeManagement_Combo.Text == "Automatic";
            SettingsFile.Instance.HandleRaidPartyBuff = this.raidpartybuffmanage_combo.Text == "Automatic";
            SettingsFile.Instance.HandleInterrupts = this.interruptmanagent_combo.Text == "Automatic";
            SettingsFile.Instance.HandleHealing = this.healingmanage_combo.Text == "Automatic";
            SettingsFile.Instance.HandleBaneOfHavoc = this.banemanagement_combo.Text == "Automatic";
            SettingsFile.Instance.HandleMultiDotting = this.multidotting_combo.Text == "Automatic";
            SettingsFile.Instance.HandleWarriorShout = this.shoutmanagement_combo.Text == "Commanding";
            SettingsFile.Instance.HandleClientLag = this.clientlag_combo.Text == "Default";
            SettingsFile.Instance.HandleTotems = this.totemmanage_combo.Text == "Automatic";
            SettingsFile.Instance.HandleMovement = this.handlemovement_combo.Text == "Yes";
            
            // keybinds - Save Selected
            SettingsFile.Instance.KeybindPauseRotation = GetEnumValues(kb_pauserotation_cmbo);
            SettingsFile.Instance.KeybindCooldownManagement = GetEnumValues(kb_cooldownmanage_cmbo);
            SettingsFile.Instance.KeybindDsExtraButtonClick = GetEnumValues(kb_dsextrabutton_cmbo);
            SettingsFile.Instance.KeybindAoEManagement = GetEnumValues(kb_aoemanage_cmbo);
            SettingsFile.Instance.KeybindRaidPartyBuffManagement = GetEnumValues(kb_raidpartybuff_cmbo);
            SettingsFile.Instance.KeybindInteruptManagement = GetEnumValues(kb_interuptmanage_cmbo);
            SettingsFile.Instance.KeybindHealDefensiveManagement = GetEnumValues(kb_healdefensemanage_cmbo);
            SettingsFile.Instance.KeybindMultiDottingManagement = GetEnumValues(kb_multidotmanage_cmbo);
         
            SettingsFile.Instance.Save();
        }

        private void CooldownComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Cooldowns = {0} ", this.cooldown_Combo.SelectedItem.ToString());
            this.cooldown_Combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void DsextrabuttonclickComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] DS Extrabuttonclick = {0} ", this.dsextrabuttonclick_combo.SelectedItem.ToString());
            this.dsextrabuttonclick_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void SpriestrotationComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Shadow Priest Rotation = {0} ", this.spriestrotation_combo.SelectedItem.ToString());
            this.spriestrotation_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void AoeManagementComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] AoE Management = {0} ", this.aoeManagement_Combo.SelectedItem.ToString());
            this.aoeManagement_Combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void RaidpartybuffmanageComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Raid/Party Buff Management = {0} ", this.raidpartybuffmanage_combo.SelectedItem.ToString());
            this.raidpartybuffmanage_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void InterruptmanagentComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Interrupt Management = {0} ", this.interruptmanagent_combo.SelectedItem.ToString());
            this.interruptmanagent_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void HealingmanageComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Heal/Defensive Management = {0} ", this.healingmanage_combo.SelectedItem.ToString());
            this.healingmanage_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void BanemanagementComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Bane of Havoc Management = {0} ", this.banemanagement_combo.SelectedItem.ToString());
            this.banemanagement_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void MultidottingComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Multi-dotting Management = {0} ", this.multidotting_combo.SelectedItem.ToString());
            this.multidotting_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void ShoutmanagementComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Warrior Shout Management = {0} ", this.shoutmanagement_combo.SelectedItem.ToString());
            this.shoutmanagement_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void ClientlagComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] ClientLag (" + CombatLogEvents.ClientLag + ") Management = {0} ", this.clientlag_combo.SelectedItem.ToString());
            this.clientlag_combo.SelectedIndexChanged += this.SaveSettings;           
        }

        private void TotemmanageComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Totem Management = {0} ", this.totemmanage_combo.SelectedItem.ToString());
            this.totemmanage_combo.SelectedIndexChanged += this.SaveSettings;
        }

        private void HandlemovementComboSelectedIndexChanged(object sender, EventArgs e)
        {
            CLU.Instance.Log(" [Settings] Handle Movement = {0} ", this.handlemovement_combo.SelectedItem.ToString());
            this.handlemovement_combo.SelectedIndexChanged += this.SaveSettings;
        }

        /* ============================== Keybinds ======================================================================== */

        private void KbpauserotationSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_pauserotation_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_pauserotation_cmbo.SelectedItem.ToString(), kb_pauserotation_cmbo.Name);
           // this.kb_pauserotation_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        private void KbcooldownmanageSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_cooldownmanage_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_cooldownmanage_cmbo.SelectedItem.ToString(), kb_cooldownmanage_cmbo.Name);
           // this.kb_cooldownmanage_cmbo.SelectedIndexChanged += this.SaveSettings;

        }

        private void KbdsextrabuttonSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_dsextrabutton_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_dsextrabutton_cmbo.SelectedItem.ToString(), kb_dsextrabutton_cmbo.Name);
           // this.kb_dsextrabutton_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        private void KbaoemanageSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_aoemanage_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_aoemanage_cmbo.SelectedItem.ToString(), kb_aoemanage_cmbo.Name);
          // this.kb_aoemanage_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        private void KbraidpartybuffSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_raidpartybuff_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_raidpartybuff_cmbo.SelectedItem.ToString(), kb_raidpartybuff_cmbo.Name);
           // this.kb_raidpartybuff_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        private void KbinteruptmanageSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_interuptmanage_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_interuptmanage_cmbo.SelectedItem.ToString(), kb_interuptmanage_cmbo.Name);
           // this.kb_interuptmanage_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        private void KbhealdefensemanageSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_healdefensemanage_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_healdefensemanage_cmbo.SelectedItem.ToString(), kb_healdefensemanage_cmbo.Name);
          //  this.kb_healdefensemanage_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        private void KbmultidotmanageSelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessControls(this, kb_multidotmanage_cmbo); // Loop through the Combo Boxs and see if the user has chosen the keybind already
            CLU.Instance.Log(" [Keybinds] {0} assigned to {1} ", this.kb_multidotmanage_cmbo.SelectedItem.ToString(), kb_multidotmanage_cmbo.Name);
          //  this.kb_multidotmanage_cmbo.SelectedIndexChanged += this.SaveSettings;
        }

        /// <summary>
        /// Paints the WoWSpellLockWatcher information to the panel
        /// </summary>
        private void paint()
        {
            this.panel1.Controls.Clear();

            var W = 316;
            var dy = 20;

            var y = 0;
            var locks = CombatLogEvents.Instance.DumpSpellLocks();

            var col1 = 10;
            var col2 = (int)(col1 + (W - 20.0) * 0.6);
            var col3 = (int)(col2 + (W - 20.0) * 0.4 / 2.0);

            foreach (var x in locks)
            {
                var color = x.Value <= 0 ? Color.DarkGreen : Color.Red;

                var A = new Label();
                A.Text = x.Key;
                A.ForeColor = color;
                A.Size = new Size(col2 - col1, dy);
                A.TextAlign = ContentAlignment.MiddleLeft;
                A.Location = new Point(col1, y);

                var B = new Label();
                B.Text = x.Value.ToString();
                B.ForeColor = color;
                B.Size = new Size(col3 - col2, dy);
                B.TextAlign = ContentAlignment.MiddleLeft;
                B.Location = new Point(col2, y);

                var C = new Label();
                C.Text = Buff.TargetHasDebuff(x.Key) ? "Up" : "Down";
                C.ForeColor = color;
                C.Size = new Size(100, dy);
                C.TextAlign = ContentAlignment.MiddleLeft;
                C.Location = new Point(col3, y);

                this.panel1.Controls.Add(A);
                this.panel1.Controls.Add(B);
                this.panel1.Controls.Add(C);

                y += dy;
            }
        }

        /// <summary>
        /// Timer to update SpellLockWatcher
        /// </summary>
        private void Timer1Tick1(object sender, EventArgs e)
        {
            try
            {
                paint();
            }
            catch { }

        }

        /// <summary>
        /// map Enum values to a string
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        public class EnumWithName<T>
        {
            private string Name { get; set; }

            private T Value { get; set; }

            public static EnumWithName<T>[] ParseEnum()
            {
                var list = new List<EnumWithName<T>>();

                foreach (object o in Enum.GetValues(typeof(T)))
                {
                    list.Add(new EnumWithName<T>
                    {
                        Name = Enum.GetName(typeof(T), o).Replace('_', ' '),
                        Value = (T)o
                    });
                }

                return list.ToArray();
            }

            public override string ToString()
            {
                return this.Name;
            }
        }

        /// <summary>
        ///  set protected property Control.Double­Buffered to true This is useful, if 
        ///  you want to avoid flickering of controls such as ListView (when updating) 
        ///  or Panel (when you draw on it).
        /// </summary>
        /// <param name="control">the control to set DoubleBuffered</param>
        private static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                control,
                new object[] { true });
        }

        /// <summary>
        /// Gets the Enum string value from the Combo Box
        /// </summary>
        /// <param name="cmbobox">the combo box to retreive the Enum</param>
        /// <returns>the string value at the selected index of the combo box</returns>
        private string GetEnumValues(ComboBox cmbobox)
        {
            var type = (EnumWithName<Keybinds.Keyboardfunctions>)cmbobox.SelectedItem;
            return type.ToString();
        }

        /// <summary>
        /// Prints the WoWstats Report
        /// </summary>
        private void PrintreportClick(object sender, EventArgs e)
        {
            WoWStats.Instance.PrintReport();
        }

        /// <summary>
        /// Clears all the WoWstats 
        /// </summary>
        private void ClearstatsButtonClick(object sender, EventArgs e)
        {
            WoWStats.Instance.ClearStats();
        }

        /// <summary>
        /// Displays information about your current target
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetinfoButtonClick(object sender, EventArgs e)
        {
            TargetInfo.Display();
        }

        /// <summary>
        /// Close Form
        /// </summary>
        private void CloseClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ProcessControls(Control ctrlContainer, ComboBox cmbobox)
        {
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl.GetType() == typeof(ComboBox))
                {
                    try
                    {
                        if (ctrl.Name != cmbobox.Name)
                        {
                            bool matchs = ((ComboBox)ctrl).SelectedValue.ToString() == cmbobox.SelectedValue.ToString();
                            if (matchs && ((ComboBox)ctrl).SelectedValue.ToString() != "Nothing")
                            {
                                CLU.Instance.Log(" [Keybind Match] {0} == {1}. Please Make another selection", ctrl.Name, cmbobox.Name);
                                cmbobox.SelectedIndex = cmbobox.FindStringExact("Nothing");
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                kb_save_label.Text = "Click Save";
                // if a control contains sub controls check
                if (ctrl.HasChildren)
                {
                    ProcessControls(ctrl, cmbobox);
                }
            }
        }

        private void KbSaveClick(object sender, EventArgs e)
        {
            SaveSettings(sender, e);
            kb_save_label.Text = "Settings Saved";
        }
    }
}
