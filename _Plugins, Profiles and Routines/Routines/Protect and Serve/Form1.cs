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

namespace ProtectAndServe
{
    public partial class ProtectAndServeConfig : Form
    {
        public ProtectAndServeConfig()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProtectAndServeSettings.Instance.Load();

            checkBox1.Checked = ProtectAndServeSettings.Instance.UseDefensiveCooldowns;
            checkBox2.Checked = ProtectAndServeSettings.Instance.Buff;

            guardianoftheancientkingspercent.Value = new decimal(ProtectAndServeSettings.Instance.GuardianoftheancientKingsPercent);
            ardentdefenderpercent.Value = new decimal(ProtectAndServeSettings.Instance.ArdentDefenderPercent);
            divineprotectionpercent.Value = new decimal(ProtectAndServeSettings.Instance.DivineProtectionPercent);
            holyshieldpercent.Value = new decimal(ProtectAndServeSettings.Instance.HolyShieldPercent);


        }

        private void guardianoftheancientkingspercent_ValueChanged(object sender, EventArgs e)
        {
            ProtectAndServeSettings.Instance.GuardianoftheancientKingsPercent = (int)guardianoftheancientkingspercent.Value;
        }

        
        private void ardentdefenderpercent_ValueChanged(object sender, EventArgs e)
        {
            ProtectAndServeSettings.Instance.ArdentDefenderPercent = (int)ardentdefenderpercent.Value;
        }


        private void divineprotectionpercent_ValueChanged(object sender, EventArgs e)
        {
            ProtectAndServeSettings.Instance.DivineProtectionPercent = (int)divineprotectionpercent.Value;
        }


        private void holyshieldpercent_ValueChanged(object sender, EventArgs e)
        {
            ProtectAndServeSettings.Instance.HolyShieldPercent = (int)holyshieldpercent.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                ProtectAndServeSettings.Instance.UseDefensiveCooldowns = true;
            }
            else
            {
                ProtectAndServeSettings.Instance.UseDefensiveCooldowns = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                ProtectAndServeSettings.Instance.Buff = true;
            }
            else
            {
                ProtectAndServeSettings.Instance.Buff = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ProtectAndServeSettings.Instance.Save();
            Logging.Write("Configuration Saved");
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
