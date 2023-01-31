using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Hera;

namespace Hera
{
    public partial class UIForm : Form
    {
        private bool _isLoading = true;

        public UIForm()
        {
            InitializeComponent();
        }

        private void UIForm_Load(object sender, EventArgs e)
        {
            // Disable some controls due to unknown spells
            if (!Spell.IsKnown("Lifeblood")) { LifebloodHealth.Enabled = false; LifebloodHealth.Value = 0; LifebloodLabel.Enabled = false; }

            Text = String.Format("{0} by {1}", Codemplosion.CCName.ToUpper(), Codemplosion.AuthorName);
            lblCCName.Text = Codemplosion.CCName.ToUpper();

            // Populate the controls on the UI with the values from the settings. Through the wonders of Reflection this is an automated process.
            foreach (var p in typeof(Settings).GetProperties())
            {
                if (p.Name.StartsWith("_") || Settings._ignoreSettings.Contains(p.Name)) continue;

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
                            else if (obj is TextBox) { TextBox tbox = (TextBox) obj; if (tbox.Name == p.Name) { tbox.Text = (string) propValue; break; }
                            }
                        }
                    }
                }
            }

            _isLoading = false;
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
            foreach (PropertyInfo p in typeof (Settings).GetProperties().Where(p => p.Name == cbox.Name))
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

            foreach (PropertyInfo p in typeof (Settings).GetProperties().Where(p => p.Name == pbar.Name))
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

        private void SpecTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SpecTemplate.SelectedItem.ToString())
            {
                case "Marksmanship":
                    FocusShot.Value = 35;
                    KillCommand.SelectedItem = "... never";
                    FocusFire.SelectedItem = "... never";
                    PetAttackDelay.SelectedItem = "... never";
                    Disengage.SelectedItem = "... always";
                    Misdirection.SelectedItem = "... always";
                    HuntersMark.SelectedItem = "... only if not low level";
                    FeignDeath.SelectedItem = "... on aggro and low health";
                    AimedShot.SelectedItem = "... focus 65+";
                    ArcaneShot.SelectedItem = "... focus 45+";
                    SerpentSting.SelectedItem = "... always";
                    Intimidation.SelectedItem = "... never";
                    RapidFire.SelectedItem = "... only on adds";
                    WidowVenom.SelectedItem = "... only in battlegrounds";
                    ExplosiveShot.SelectedItem = "... never";
                    BestialWrath.SelectedItem = "... never";
                    BlackArrow.SelectedItem = "... never";
                    RapidFire.SelectedItem = "... only on adds";
                    ChimeraShot.SelectedItem = "... always";
                    break;

                case "Beast Mastery":
                    FocusShot.Value = 35;
                    KillCommand.SelectedItem = "... always";
                    FocusFire.SelectedItem = "... on 4+ Frenzy";
                    PetAttackDelay.SelectedItem = "... never";
                    Disengage.SelectedItem = "... always";
                    Misdirection.SelectedItem = "... always";
                    HuntersMark.SelectedItem = "... only if not low level";
                    FeignDeath.SelectedItem = "... on aggro and low health";
                    AimedShot.SelectedItem = "... never";
                    ArcaneShot.SelectedItem = "... focus 45+";
                    SerpentSting.SelectedItem = "... always";
                    Intimidation.SelectedItem = "... on casters and runners";
                    RapidFire.SelectedItem = "... only on adds";
                    WidowVenom.SelectedItem = "... only in battlegrounds";
                    ExplosiveShot.SelectedItem = "... never";
                    BestialWrath.SelectedItem = "... only on adds";
                    BlackArrow.SelectedItem = "... never";
                    RapidFire.SelectedItem = "... only on adds";
                    ChimeraShot.SelectedItem = "... never";
                    break;

                case "Survival":
                    FocusShot.Value = 35;
                    PetAttackDelay.SelectedItem = "... never";
                    Disengage.SelectedItem = "... always";
                    Misdirection.SelectedItem = "... always";
                    HuntersMark.SelectedItem = "... only if not low level";
                    FeignDeath.SelectedItem = "... on aggro and low health";
                    FocusFire.SelectedItem = "... never";
                    AimedShot.SelectedItem = "... never";
                    ArcaneShot.SelectedItem = "... focus 45+";
                    SerpentSting.SelectedItem = "... always";
                    Intimidation.SelectedItem = "... never";
                    RapidFire.SelectedItem = "... only on adds";
                    WidowVenom.SelectedItem = "... only in battlegrounds";
                    KillCommand.SelectedItem = "... never";
                    ExplosiveShot.SelectedItem = "... always";
                    BestialWrath.SelectedItem = "... never";
                    BlackArrow.SelectedItem = "... always";
                    RapidFire.SelectedItem = "... only on adds";
                    ChimeraShot.SelectedItem = "... never";
                    break;
            }
        }

    

    }
}
