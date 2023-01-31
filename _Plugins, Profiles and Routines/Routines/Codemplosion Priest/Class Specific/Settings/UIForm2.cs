using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using Styx.WoWInternals;

namespace Hera
{
    public partial class UIForm2 : Form
    {
        private GlobalHotkey ghk;
        private bool _isLoading = true;
        private GroupBox _lastGroupUsed;
        private Dictionary<string, string> _valueTemplates = new Dictionary<string, string>();
        private const string FileName = @"UIElements.xml";
        private string _path;

        public UIForm2()
        {
            InitializeComponent();
            ghk = new GlobalHotkey(GlobalHotkey.Constants.ALT + GlobalHotkey.Constants.SHIFT + GlobalHotkey.Constants.CTRL, Keys.Z, this);
        }

        private void HandleHotkey()
        {
            // Toggle LazyRaider
            Settings.LazyRaider = Settings.LazyRaider.Contains("never") ? "... always" : "... never";
            //Utils.Log("** Global hotkey pressed. LazyRaider has been set to: " + Settings.LazyRaider);

            foreach (PropertyInfo p in typeof(Settings).GetProperties())
            {
                if (p.Name.StartsWith("_") || Settings.IgnoreSettings.Contains(p.Name) || p.Name.ToUpper() != "LAZYRAIDER") continue;

                object propValue = typeof(Settings).GetProperty(p.Name).GetValue(p.Name, null);

                foreach (TabPage tab in tabControl1.Controls)
                {
                    foreach (GroupBox gb in tab.Controls)
                    {
                        foreach (object obj in gb.Controls)
                        {
                            if (obj is ComboBox)
                            {
                                ComboBox cbox = (ComboBox) obj;
                                if (cbox.Name == "LazyRaider")
                                {
                                    cbox.SelectedItem = propValue.ToString();
                                    Utils.Log("** Global hotkey pressed. Lazy Raider has been set to: " + Settings.LazyRaider,Utils.Colour("Red"));
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == GlobalHotkey.Constants.WM_HOTKEY_MSG_ID) HandleHotkey();
            base.WndProc(ref m);
        }

        private void UIForm2_Load(object sender, EventArgs e)
        {
            ghk.Register();
            Utils.Log("** Global hotkey for LazyRaider registered: Ctrl + Alt + Shift + Z", Utils.Colour("Red"));

            string environment = Utils.IsBattleground ? "PVP" : "PVE";
            environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            ConfigSettings.CurrentEnvironment = environment;
            environmentSettings.SelectedItem = environment;
            ConfigSettings.UIActive = true;

            _path = Settings.ConfigFolder + FileName;

            #region Dynamically populate UI form with controls

            #region Add Value Templates
            foreach (XElement ele in XDocument.Load(_path).Root.Elements("ValueTemplates").Elements("Template"))
            {
                string label = ele.Element("Label").Value;
                string value = ele.Element("Value").Value;

                _valueTemplates.Add(label.ToUpper(), value);
            }
            #endregion

            #region Add Tab Pages
            foreach (XElement ele in XDocument.Load(_path).Root.Elements("TabPages").Elements("Tab"))
            {
                string label = ele.Element("Label").Value;

                TabPage tp = new TabPage { Text = label, BackColor = Color.White };
                tabControl1.TabPages.Add(tp);
            }
            #endregion

            #region Add Groupboxes to all tabs
            foreach (XElement ele in XDocument.Load(_path).Root.Elements("Groupboxes").Elements("Group"))
            {
                string label = ele.Element("Label").Value;
                string name = ele.Element("Name").Value;
                string column = ele.Element("Column").Value;
                string tabpage = ele.Element("Tab").Value;

                int leftPosition = (column == "Right" ? 318 : 8);
                GroupBox gb = new GroupBox { Left = leftPosition, Top = 220, Height = 224, Width = 302, Text = label, Name = name };
                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Text == tabpage) { tab.Controls.Add(gb); break; }
                }
            }

            #endregion

            #region Add Items to each group

            foreach (XElement ele in XDocument.Load(_path).Root.Elements("Items").Elements("Item"))
            {
                string label = ele.Element("Label").Value;
                string name = ele.Element("Name").Value;
                string groupName = ele.Element("GroupName").Value;
                string type = ele.Element("Type").Value;
                string value = ele.Element("Value").Value;
                string tooltip = ele.Element("Tooltip").Value;
                string tag = ele.Element("Tag").Value;

                // If no 'type' is passed assume it is a combo box. I use this more than the bars so its a safe assumption
                if (type.ToUpper() == "COMBO" || string.IsNullOrEmpty(type))
                {
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        // Find the groupbox name and pass it along with the other data
                        foreach (GroupBox gb in from TabPage tab in tabControl1.Controls from gb in tab.Controls.Cast<GroupBox>().Where(gb => gb.Name == groupName) select gb)
                        {
                            NewCombo(gb, label, name, value, tooltip, tag);
                            break;
                        }
                    }
                    else
                    {
                        // No Groupbox name was read so use the last Groupbox
                        NewCombo(null, label, name, value, tooltip, tag);
                    }

                }
                else if (type.ToUpper() == "BAR")
                {
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        // Find the groupbox name and pass it along with the other data
                        foreach (GroupBox gb in from TabPage tab in tabControl1.Controls from gb in tab.Controls.Cast<GroupBox>().Where(gb => gb.Name == groupName) select gb)
                        {
                            NewBar(gb, label, name, tooltip, tag);
                            break;
                        }
                    }
                    else
                    {
                        // No Groupbox name was read so use the last Groupbox
                        NewBar(null, label, name, tooltip, tag);
                    }
                }


            }



            #endregion

            #region Resize and reposition the groupboxes
            foreach (TabPage tab in tabControl1.Controls)
            {
                GroupBox leftGb = null, rightGb = null;
                foreach (GroupBox gb in tab.Controls)
                {
                    // Resize each Groupbox appropriately for the number of controls it has
                    gb.Height = ((28 * gb.Controls.Count / 2) + 28);

                    if (gb.Left == 8)
                    {
                        if (leftGb != null) { gb.Top = leftGb.Top + leftGb.Height + 4; leftGb = gb; }
                        else { leftGb = gb; gb.Top = 8; }
                    }
                    else if (gb.Left == 318)
                    {
                        if (rightGb != null) { gb.Top = rightGb.Top + rightGb.Height + 4; rightGb = gb; }
                        else { rightGb = gb; gb.Top = 8; }
                    }
                }

                // The last (left) Groupbox will be resized to full the column it is in. 
                if (leftGb != null)
                {
                    int filloutHeight = leftGb.Height + (tab.Height - (leftGb.Top + leftGb.Height + 8));
                    leftGb.Height = filloutHeight;
                }

                // The last (right) Groupbox will be resized to full the column it is in. 
                if (rightGb != null)
                {
                    int filloutHeight = rightGb.Height + (tab.Height - (rightGb.Top + rightGb.Height + 8));
                    rightGb.Height = filloutHeight;
                }
            }
            #endregion

            #endregion

            
            LoadSettings();
            

            _isLoading = false;

            if (Settings.MultipleEnvironment.Contains("never"))
            {
                environmentSettings.SelectedItem = "PVE";
                environmentSettings.Visible = false;
            }
        }

        private void LoadSettings()
        {
            #region Use Reflection to assign settings properties to UI elements

            // Disable some controls due to unknown spells
            //if (!Spell.IsKnown("Lifeblood")) { LifebloodHealth.Enabled = false; LifebloodHealth.Value = 0; LifebloodLabel.Enabled = false; }

            Text = String.Format("{0} by {1}", Codemplosion.CCName.ToUpper(), Codemplosion.AuthorName);
            lblCCName.Text = Codemplosion.CCName.ToUpper();

            // Populate the controls on the UI with the values from the settings. Through the wonders of Reflection this is an automated process.
            foreach (PropertyInfo p in typeof(Settings).GetProperties())
            {
                if (p.Name.StartsWith("_") || Settings.IgnoreSettings.Contains(p.Name)) continue;

                object propValue = typeof(Settings).GetProperty(p.Name).GetValue(p.Name, null);

                foreach (TabPage tab in tabControl1.Controls)
                {
                    foreach (GroupBox gb in tab.Controls)
                    {
                        foreach (object obj in gb.Controls)
                        {
                            // Populate all combo boxes, this will be the most common
                            if (obj is ComboBox) { ComboBox cbox = (ComboBox)obj; if (cbox.Name == p.Name) { cbox.SelectedItem = propValue.ToString(); break; } }

                                // Populate all progress bars, mostly for health or mana 
                            else if (obj is ProgressBar) { ProgressBar pbar = (ProgressBar)obj; if (pbar.Name == p.Name) { pbar.Value = (int)propValue; break; } }
                            else if (obj is TextBox)
                            {
                                TextBox tbox = (TextBox)obj; if (tbox.Name == p.Name) { tbox.Text = (string)propValue; break; }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            // Save the settings to the XML file
            Settings.Save();

            // DirtyData tells the CC the data has changed and it needs to reload it
            Settings.DirtyData = true;
            Close();
        }

        private void CommonDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            ComboBox cbox = (ComboBox)sender;
            foreach (PropertyInfo p in typeof(Settings).GetProperties().Where(p => p.Name == cbox.Name))
            {
                p.SetValue(typeof(Settings), cbox.SelectedItem.ToString(), null);
                break;
            }
        }

        private void CommonProgressBar_MouseAction(object sender, MouseEventArgs e)
        {
            if (_isLoading || e.Button != MouseButtons.Left) return;

            ProgressBar pbar = (ProgressBar)sender;
            int value = 0;

            pbar.Value = MouPosValue(e.Location.X, pbar.Width);
            value = pbar.Value;

            foreach (PropertyInfo p in typeof(Settings).GetProperties().Where(p => p.Name == pbar.Name))
            {
                p.SetValue(typeof(Settings), value, null);
                break;
            }

        }

        private void CommonTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            TextBox tbox = (TextBox)sender;
            foreach (PropertyInfo p in typeof(Settings).GetProperties().Where(p => p.Name == tbox.Name))
            {
                p.SetValue(typeof(Settings), tbox.Text, null);
                break;
            }
        }


        /// <summary>
        /// Changed the enabled/disabled status of a combobox dependant on the associated spell being known
        /// </summary>
        /// <param name="uiControl"></param>
        /// <param name="spellName">Spell name to check</param>
        /// <param name="valueIfDisabled">Default selected item if the setting is disabled</param>
        /// <returns>TRUE is the spell is know and the control is usable. FALSE if the spell is not known and the control is disabled.</returns>
        public bool SettingControlCheck(ComboBox uiControl, string spellName, string valueIfDisabled)
        {
            bool result = Spell.IsKnown(spellName);

            if (result) return true;
            uiControl.Enabled = result;
            //uiControl.Items.Add("... Unknown spell setting disabled");
            uiControl.SelectedItem = valueIfDisabled;

            return false;
        }
        
        // Neat little function to use a Progress bar like a slider control by simply clicking on it or dragging
        // REALLY NEAT! Took 3 seconds to come up with the idea and 30 minutes to figure out how to get it to work
        public int MouPosValue(double mousePosition, double progressBarWidth)
        {
            if (mousePosition < 0) mousePosition = 0;
            if (mousePosition > progressBarWidth) mousePosition = progressBarWidth;

            double ratio = mousePosition / progressBarWidth;
            double value = ratio * 100;

            if (value > 100) value = 100;
            if (value < 0) value = 0;

            return (int)Math.Ceiling(value);
        }

        private void NewCombo(GroupBox gb, string label, string name, string value, string tooltip, string itemTag)
        {
            // If not Groupbox is passed then use the last used Groupbox
            if (gb == null) gb = _lastGroupUsed;

            // If no 'name' was parsed then use the label but remove all spaces
            if (string.IsNullOrEmpty(name))
            {
                name = label.Replace(" ", "");
            }

            int comboWidth = (gb.Width - 118) - 7;
            int comboTop = ((gb.Controls.Count / 2) * 28) + 19;
            ComboBox cb = new ComboBox { Left = 118, Height = 21, Name = name, Top = comboTop, Width = comboWidth, DropDownStyle = ComboBoxStyle.DropDownList, Sorted = false };

            // If 'value' is blank then add defaults '... always' and '... never'
            if (string.IsNullOrEmpty(value)) value = "... always|... never";

            // If using a Value Template insert the template data
            if (value.ToUpper().StartsWith("VT:")) { string vtKey = value.Substring(3).ToUpper(); if (_valueTemplates.ContainsKey(vtKey))value = _valueTemplates[vtKey]; }

            // Split the string to an array and add each line to the dropdown list
            string[] strings = value.Split('|');
            foreach (string word in strings) { cb.Items.Add(word.Trim()); }

            // Tooltip
            if (!string.IsNullOrEmpty(tooltip))
            {
                ToolTip tp = new ToolTip {ToolTipIcon = ToolTipIcon.Info, ToolTipTitle = Codemplosion.CCName};
                tp.SetToolTip(cb, tooltip);
            }

            // State / Tags
            string[] tags = itemTag.Split('|');
            foreach (string tag in tags)
            {
                if (tag.ToUpper() == "DISABLED") cb.Enabled = false;
                if (tag.ToUpper() == "SORT") cb.Sorted = true;
            }

            // Events
            cb.SelectedIndexChanged += CommonDropDown_SelectedIndexChanged;

            // Add the Comobox to the Groupbox
            gb.Controls.Add(cb);

            // Add the accompanying label
            Label lb = new Label { Left = 6, Top = comboTop, Width = 106, Height = 21, AutoSize = false, TextAlign = ContentAlignment.MiddleRight, Text = label };

            // State
            if (itemTag.ToUpper().Contains("DISABLED")) lb.Enabled = false;

            // Add the label to the Groupbox
            gb.Controls.Add(lb);

            // Update the last used Groupbox
            _lastGroupUsed = gb;

        }

        private void NewBar(GroupBox gb, string label, string name, string tooltip, string state)
        {
            // If not Groupbox is passed then use the last used Groupbox
            if (gb == null) gb = _lastGroupUsed;

            int barWidth = (gb.Width - 118) - 7;
            int barTop = ((gb.Controls.Count / 2) * 28) + 19;

            ProgressBar pb = new ProgressBar { Left = 118, Top = barTop, Height = 21, Width = barWidth, Value = 0, Maximum = 100, Name = name };

            // Tooltip
            if (!string.IsNullOrEmpty(tooltip))
            {
                ToolTip tp = new ToolTip { ToolTipIcon = ToolTipIcon.Info, ToolTipTitle = Codemplosion.CCName };
                tp.SetToolTip(pb, tooltip);
            }

            // State
            if (state.ToUpper().Contains("DISABLED")) pb.Enabled = false;

            // Events
            pb.MouseClick += CommonProgressBar_MouseAction;
            pb.MouseMove += CommonProgressBar_MouseAction;

            // Add the bar to the Groupbox
            gb.Controls.Add(pb);

            // Add the accompanying label
            Label lb = new Label { Left = 6, Top = barTop, Width = 106, Height = 21, AutoSize = false, TextAlign = ContentAlignment.MiddleRight, Text = label };

            // State
            if (state.ToUpper().Contains("DISABLED")) lb.Enabled = false;

            // Add the label to the Groupbox
            gb.Controls.Add(lb);

            // Update the last used Groupbox
            _lastGroupUsed = gb;

        }

        private void environmentSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            ConfigSettings.FileName = Settings.ConfigFile;
            ConfigSettings.CurrentEnvironment = environmentSettings.SelectedItem.ToString();
            Settings.Load();
            LoadSettings();
        }

        private void UIForm2_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConfigSettings.UIActive = false;
        }

        private void UIForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ghk.Unregiser();
            Utils.Log("** Global hotkey unregistered", Utils.Colour("Red"));
        }

    }
}
