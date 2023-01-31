using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace FelMaster
{
    public partial class FelMasterForm7 : Form
    {
        public FelMasterForm7()
        {
            InitializeComponent();
        }

        private void FelMasterForm_Load(object sender, EventArgs e)
        {
            FelMasterSettings.Instance.Load();
            T1.Checked = FelMasterSettings.Instance.T1;
            T2.Checked = FelMasterSettings.Instance.T2;
            SnD.Checked = FelMasterSettings.Instance.SnD;
            ND.Checked = FelMasterSettings.Instance.ND;
        }

        private void T1_CheckedChanged(object sender, EventArgs e)
        {
            FelMasterSettings.Instance.T1 = T1.Checked;
        }

        private void T2_CheckedChanged(object sender, EventArgs e)
        {
            FelMasterSettings.Instance.T2 = T2.Checked;
        }

        private void SnD_CheckedChanged(object sender, EventArgs e)
        {
            FelMasterSettings.Instance.SnD = SnD.Checked;
        }

        private void ND_CheckedChanged(object sender, EventArgs e)
        {
            FelMasterSettings.Instance.ND = ND.Checked;
        }

        private void savecfg_Click(object sender, EventArgs e)
        {
            FelMasterSettings.Instance.Save();
            Logging.Write("FelMaster Configuration Saved");
            Close();
        }


    }
}
