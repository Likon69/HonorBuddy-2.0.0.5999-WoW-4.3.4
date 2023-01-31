using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Avenger
{
    public partial class AvengerConfig : Form
    {
        public AvengerConfig()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AvengerSettings.Instance.Load();

            BuffBox.Checked = AvengerSettings.Instance.Buff;
            Trinket1.Checked = AvengerSettings.Instance.Trinket1;
            Trinket2.Checked = AvengerSettings.Instance.Trinket2;
            SynapseSprings.Checked = AvengerSettings.Instance.SynapseSprings;
            Zealotry.Checked = AvengerSettings.Instance.Zealotry;
            AvengingWrath.Checked = AvengerSettings.Instance.AvengingWrath;
            GOAK.Checked = AvengerSettings.Instance.GOAK;
            DivineProtection.Checked = AvengerSettings.Instance.DivineProtection;
            DivineStorm.Checked = AvengerSettings.Instance.DivineStorm;
            SOTR.Checked = AvengerSettings.Instance.SOTR;
            LoH.Checked = AvengerSettings.Instance.LoH;
            DivineShield.Checked = AvengerSettings.Instance.DivineShield;
            Rebuke.Checked = AvengerSettings.Instance.Rebuke;

            AoEMobs.Value = new decimal(AvengerSettings.Instance.AoEMobs);
        }


        private void BuffBox_CheckedChanged(object sender, EventArgs e)
        {
            if (BuffBox.Checked == true)
            {
                AvengerSettings.Instance.Buff = true;
            }
            else
            {
                AvengerSettings.Instance.Buff = false;
            }
        }

        private void Trinket1_CheckedChanged(object sender, EventArgs e)
        {
            if (Trinket1.Checked == true)
            {
                AvengerSettings.Instance.Trinket1 = true;
            }
            else
            {
                AvengerSettings.Instance.Trinket1 = false;
            }
        }

        private void Trinket2_CheckedChanged(object sender, EventArgs e)
        {
            if (Trinket2.Checked == true)
            {
                AvengerSettings.Instance.Trinket2 = true;
            }
            else
            {
                AvengerSettings.Instance.Trinket2 = false;
            }
        }

        private void SynapseSprings_CheckedChanged(object sender, EventArgs e)
        {
            if (SynapseSprings.Checked == true)
            {
                AvengerSettings.Instance.SynapseSprings = true;
            }
            else
            {
                AvengerSettings.Instance.SynapseSprings = false;
            }
        }

        private void Zealotry_CheckedChanged(object sender, EventArgs e)
        {
            if (Zealotry.Checked == true)
            {
                AvengerSettings.Instance.Zealotry = true;
            }
            else
            {
                AvengerSettings.Instance.Zealotry = false;
            }
        }

        private void AvengingWrath_CheckedChanged(object sender, EventArgs e)
        {
            if (AvengingWrath.Checked == true)
            {
                AvengerSettings.Instance.AvengingWrath = true;
            }
            else
            {
                AvengerSettings.Instance.AvengingWrath = false;
            }
        }

        private void GOAK_CheckedChanged(object sender, EventArgs e)
        {
            if (GOAK.Checked == true)
            {
                AvengerSettings.Instance.GOAK = true;
            }
            else
            {
                AvengerSettings.Instance.GOAK = false;
            }
        }

        private void DivineProtection_CheckedChanged(object sender, EventArgs e)
        {
            if (DivineProtection.Checked == true)
            {
                AvengerSettings.Instance.DivineProtection = true;
            }
            else
            {
                AvengerSettings.Instance.DivineProtection = false;
            }
        }

        private void DivineStorm_CheckedChanged(object sender, EventArgs e)
        {
            if (DivineStorm.Checked == true)
            {
                AvengerSettings.Instance.DivineStorm = true;
            }
            else
            {
                AvengerSettings.Instance.DivineStorm = false;
            }
        }

        private void SOTR_CheckedChanged(object sender, EventArgs e)
        {
            if (SOTR.Checked == true)
            {
                AvengerSettings.Instance.SOTR = true;
            }
            else
            {
                AvengerSettings.Instance.SOTR = false;
            }
        }

        private void LoH_CheckedChanged(object sender, EventArgs e)
        {
            if (LoH.Checked == true)
            {
                AvengerSettings.Instance.LoH = true;
            }
            else
            {
                AvengerSettings.Instance.LoH = false;
            }
        }

        private void DivineShield_CheckedChanged(object sender, EventArgs e)
        {
            if (DivineShield.Checked == true)
            {
                AvengerSettings.Instance.DivineShield = true;
            }
            else
            {
                AvengerSettings.Instance.DivineShield = false;
            }
        }

        private void Rebuke_CheckedChanged(object sender, EventArgs e)
        {
            if (Rebuke.Checked == true)
            {
                AvengerSettings.Instance.Rebuke = true;
            }
            else
            {
                AvengerSettings.Instance.Rebuke = false;
            }
        }

        private void AoEMobs_ValueChanged(object sender, EventArgs e)
        {
            AvengerSettings.Instance.AoEMobs = (int)AoEMobs.Value;
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            AvengerSettings.Instance.Save();
            Logging.Write("Configuration Saved");
            Close();
        }
    }
}
