using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PvPRogue
{
    public partial class FormSettings : Form
    {
        public static FormSettings _Instance;

        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            ptgSettings.SelectedObject = ClassSettings._Instance;
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Styx.Helpers.Settings)ptgSettings.SelectedObject).Save();


            if (ClassSettings._Instance.GeneralAlwaysStealthed)
            {
                Styx.Helpers.CharacterSettings.Instance.UseMount = false;
            }
            else
            {
                Styx.Helpers.CharacterSettings.Instance.UseMount = true;
            }


            e.Cancel = true;
            this.Hide();
        }
    }
}
