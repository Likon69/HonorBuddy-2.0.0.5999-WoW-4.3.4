using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.Helpers;

namespace UltimatePaladinHealerBT
{
    public partial class UPrHCCBTConfigForm : Form
    {
        public UPrHCCBTConfigForm()
        {
            InitializeComponent();
        }
        private void UPrHCCBTConfig_Load(object sender, EventArgs e)
        {

            Logging.Write("Settings Panel Opened");
            UPrHBTSetting.Instance.Load();

            if (UPrHBTSetting.Instance.PVE_intellywait == true) { PVE_intellywait.Checked = true; }
            else if (UPrHBTSetting.Instance.PVE_decice_during_GCD == false) { PVE_accurancy.Checked = true; }
            else if (UPrHBTSetting.Instance.PVE_decice_during_GCD == true) { PVE_speed.Checked = true; }
            if (UPrHBTSetting.Instance.Raid_intellywait == true) { Raid_intellywait.Checked = true; }
            else if (UPrHBTSetting.Instance.Raid_decice_during_GCD == false) { Raid_accurancy.Checked = true; }
            else if (UPrHBTSetting.Instance.Raid_decice_during_GCD == true) { Raid_speed.Checked = true; }
            if (UPrHBTSetting.Instance.Raid_general_raid_healer == 0) { Raid_tank_healer.Checked = true; }
            else if (UPrHBTSetting.Instance.Raid_general_raid_healer == 1) { Raid_AOE_healer.Checked = true; }
            if (UPrHBTSetting.Instance.Battleground_general_raid_healer == 0) { Battleground_tank_healer.Checked = true; }
            else if (UPrHBTSetting.Instance.Battleground_general_raid_healer == 1) { Battleground_AOE_healer.Checked = true; }


            PVP_do_not_touch_TB1.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB1;
            PVP_do_not_touch_TB2.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB2;
            PVP_do_not_touch_TB3.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB3;
            PVP_do_not_touch_TB4.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB4;
            PVP_do_not_touch_TB5.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB5;
            PVP_do_not_touch_TB6.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB6;
            PVP_do_not_touch_TB7.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB7;
            PVP_do_not_touch_TB8.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB8;
            PVP_do_not_touch_TB9.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB9;
            PVP_do_not_touch_TB10.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB10;
            PVP_do_not_touch_TB11.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB11;
            PVP_do_not_touch_TB12.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB12;
            PVP_do_not_touch_TB13.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB13;
            PVP_do_not_touch_TB14.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB14;
            PVP_do_not_touch_TB15.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB15;
            PVP_do_not_touch_TB16.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB16;
            PVP_do_not_touch_TB17.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB17;
            PVP_do_not_touch_TB18.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB18;
            PVP_do_not_touch_TB19.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB19;
            PVP_do_not_touch_TB20.Text = UPrHBTSetting.Instance.PVP_do_not_touch_TB20;

            PVP_dispell_ASAP_TB1.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB1;
            PVP_dispell_ASAP_TB2.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB2;
            PVP_dispell_ASAP_TB3.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB3;
            PVP_dispell_ASAP_TB4.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB4;
            PVP_dispell_ASAP_TB5.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB5;
            PVP_dispell_ASAP_TB6.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB6;
            PVP_dispell_ASAP_TB7.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB7;
            PVP_dispell_ASAP_TB8.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB8;
            PVP_dispell_ASAP_TB9.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB9;
            PVP_dispell_ASAP_TB10.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB10;
            PVP_dispell_ASAP_TB11.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB11;
            PVP_dispell_ASAP_TB12.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB12;
            PVP_dispell_ASAP_TB13.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB13;
            PVP_dispell_ASAP_TB14.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB14;
            PVP_dispell_ASAP_TB15.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB15;
            PVP_dispell_ASAP_TB16.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB16;
            PVP_dispell_ASAP_TB17.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB17;
            PVP_dispell_ASAP_TB18.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB18;
            PVP_dispell_ASAP_TB19.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB19;
            PVP_dispell_ASAP_TB20.Text = UPrHBTSetting.Instance.PVP_dispell_ASAP_TB20;

            PVE_do_not_touch_TB1.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB1;
            PVE_do_not_touch_TB2.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB2;
            PVE_do_not_touch_TB3.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB3;
            PVE_do_not_touch_TB4.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB4;
            PVE_do_not_touch_TB5.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB5;
            PVE_do_not_touch_TB6.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB6;
            PVE_do_not_touch_TB7.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB7;
            PVE_do_not_touch_TB8.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB8;
            PVE_do_not_touch_TB9.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB9;
            PVE_do_not_touch_TB10.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB10;
            PVE_do_not_touch_TB11.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB11;
            PVE_do_not_touch_TB12.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB12;
            PVE_do_not_touch_TB13.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB13;
            PVE_do_not_touch_TB14.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB14;
            PVE_do_not_touch_TB15.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB15;
            PVE_do_not_touch_TB16.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB16;
            PVE_do_not_touch_TB17.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB17;
            PVE_do_not_touch_TB18.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB18;
            PVE_do_not_touch_TB19.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB19;
            PVE_do_not_touch_TB20.Text = UPrHBTSetting.Instance.PVE_do_not_touch_TB20;

            PVE_dispell_ASAP_TB1.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB1;
            PVE_dispell_ASAP_TB2.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB2;
            PVE_dispell_ASAP_TB3.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB3;
            PVE_dispell_ASAP_TB4.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB4;
            PVE_dispell_ASAP_TB5.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB5;
            PVE_dispell_ASAP_TB6.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB6;
            PVE_dispell_ASAP_TB7.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB7;
            PVE_dispell_ASAP_TB8.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB8;
            PVE_dispell_ASAP_TB9.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB9;
            PVE_dispell_ASAP_TB10.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB10;
            PVE_dispell_ASAP_TB11.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB11;
            PVE_dispell_ASAP_TB12.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB12;
            PVE_dispell_ASAP_TB13.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB13;
            PVE_dispell_ASAP_TB14.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB14;
            PVE_dispell_ASAP_TB15.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB15;
            PVE_dispell_ASAP_TB16.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB16;
            PVE_dispell_ASAP_TB17.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB17;
            PVE_dispell_ASAP_TB18.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB18;
            PVE_dispell_ASAP_TB19.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB19;
            PVE_dispell_ASAP_TB20.Text = UPrHBTSetting.Instance.PVE_dispell_ASAP_TB20;

        }

        private void save_Click(object sender, System.EventArgs e)
        {
            try
            {
                Logging.Write("Saving Setting");
                UPrHBTSetting.Instance.Save();
                Logging.Write("Saved Settings, rebuilding Behaviors");
                UltimatePalaHealerBT.Instance.CreateBehaviors();
                Logging.Write("Behaviors rebuilt");
            }
            catch (Exception ex)
            {
                Logging.Write(Color.Red, "Exception thrown (Priest Config): {0}", ex);
            }
        }


        private void PVE_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (PVE_intellywait.Checked) { UPrHBTSetting.Instance.PVE_intellywait = true; }
            else if (PVE_speed.Checked) { UPrHBTSetting.Instance.PVE_decice_during_GCD = true; UPrHBTSetting.Instance.PVE_intellywait = false; }
            else if (PVE_accurancy.Checked) { UPrHBTSetting.Instance.PVE_decice_during_GCD = false; UPrHBTSetting.Instance.PVE_intellywait = false; }
        }

        private void PVE_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_intellywait.Checked) { UPrHBTSetting.Instance.PVE_intellywait = true; }
        }

        private void PVE_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_speed.Checked) { UPrHBTSetting.Instance.PVE_decice_during_GCD = true; UPrHBTSetting.Instance.PVE_intellywait = false; }
        }

        private void PVE_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_accurancy.Checked) { UPrHBTSetting.Instance.PVE_decice_during_GCD = false; UPrHBTSetting.Instance.PVE_intellywait = false; }
        }

        private void Raid_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (Raid_intellywait.Checked) { UPrHBTSetting.Instance.Raid_intellywait = true; }
            else if (Raid_speed.Checked) { UPrHBTSetting.Instance.Raid_decice_during_GCD = true; UPrHBTSetting.Instance.Raid_intellywait = false; }
            else if (Raid_accurancy.Checked) { UPrHBTSetting.Instance.Raid_decice_during_GCD = false; UPrHBTSetting.Instance.Raid_intellywait = false; }
        }

        private void Raid_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_intellywait.Checked) { UPrHBTSetting.Instance.Raid_intellywait = true; }
        }

        private void Raid_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_speed.Checked) { UPrHBTSetting.Instance.Raid_decice_during_GCD = true; UPrHBTSetting.Instance.Raid_intellywait = false; }
        }

        private void Raid_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_accurancy.Checked) { UPrHBTSetting.Instance.Raid_decice_during_GCD = false; UPrHBTSetting.Instance.Raid_intellywait = false; }
        }

        private void Tank_or_AOEGB_Enter(object sender, System.EventArgs e)
        {
            if (Raid_tank_healer.Checked) { UPrHBTSetting.Instance.Raid_general_raid_healer = 0; }
            else if (Raid_AOE_healer.Checked) { UPrHBTSetting.Instance.Raid_general_raid_healer = 1; }
        }

        private void Raid_tank_healer_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_tank_healer.Checked) { UPrHBTSetting.Instance.Raid_general_raid_healer = 0; }
        }

        private void Raid_AOE_healer_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_AOE_healer.Checked) { UPrHBTSetting.Instance.Raid_general_raid_healer = 1; }
        }

        private void Battleground_Tank_or_AOEGB_Enter(object sender, System.EventArgs e)
        {
            if (Battleground_tank_healer.Checked) { UPrHBTSetting.Instance.Battleground_general_raid_healer = 0; }
            else if (Battleground_AOE_healer.Checked) { UPrHBTSetting.Instance.Battleground_general_raid_healer = 1; }
        }

        private void Battleground_tank_healer_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_tank_healer.Checked) { UPrHBTSetting.Instance.Battleground_general_raid_healer = 0; }
        }

        private void Battleground_AOE_healer_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_AOE_healer.Checked) { UPrHBTSetting.Instance.Battleground_general_raid_healer = 1; }
        }

        private void Battleground_reload_subgroup_Click(object sender, System.EventArgs e)
        {
            UltimatePalaHealerBT.Instance.BuildSubGroupArray();
        }

        private void PVP_do_not_touch_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB1 = PVP_do_not_touch_TB1.Text;
        }

        private void PVP_do_not_touch_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB2 = PVP_do_not_touch_TB2.Text;
        }

        private void PVP_do_not_touch_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB3 = PVP_do_not_touch_TB3.Text;
        }

        private void PVP_do_not_touch_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB4 = PVP_do_not_touch_TB4.Text;
        }

        private void PVP_do_not_touch_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB5 = PVP_do_not_touch_TB5.Text;
        }

        private void PVP_do_not_touch_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB6 = PVP_do_not_touch_TB6.Text;
        }

        private void PVP_do_not_touch_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB7 = PVP_do_not_touch_TB7.Text;
        }

        private void PVP_do_not_touch_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB8 = PVP_do_not_touch_TB8.Text;
        }

        private void PVP_do_not_touch_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB9 = PVP_do_not_touch_TB9.Text;
        }

        private void PVP_do_not_touch_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB10 = PVP_do_not_touch_TB10.Text;
        }

        private void PVP_do_not_touch_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB11 = PVP_do_not_touch_TB11.Text;
        }

        private void PVP_do_not_touch_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB12 = PVP_do_not_touch_TB12.Text;
        }

        private void PVP_do_not_touch_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB13 = PVP_do_not_touch_TB13.Text;
        }

        private void PVP_do_not_touch_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB14 = PVP_do_not_touch_TB14.Text;
        }

        private void PVP_do_not_touch_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB15 = PVP_do_not_touch_TB15.Text;
        }

        private void PVP_do_not_touch_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB16 = PVP_do_not_touch_TB16.Text;
        }

        private void PVP_do_not_touch_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB17 = PVP_do_not_touch_TB17.Text;
        }

        private void PVP_do_not_touch_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB18 = PVP_do_not_touch_TB18.Text;
        }

        private void PVP_do_not_touch_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB19 = PVP_do_not_touch_TB19.Text;
        }

        private void PVP_do_not_touch_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_do_not_touch_TB20 = PVP_do_not_touch_TB20.Text;
        }

        private void PVP_dispell_ASAP_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB1 = PVP_dispell_ASAP_TB1.Text;
        }

        private void PVP_dispell_ASAP_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB2 = PVP_dispell_ASAP_TB2.Text;
        }

        private void PVP_dispell_ASAP_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB3 = PVP_dispell_ASAP_TB3.Text;
        }

        private void PVP_dispell_ASAP_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB4 = PVP_dispell_ASAP_TB4.Text;
        }

        private void PVP_dispell_ASAP_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB5 = PVP_dispell_ASAP_TB5.Text;
        }

        private void PVP_dispell_ASAP_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB6 = PVP_dispell_ASAP_TB6.Text;
        }

        private void PVP_dispell_ASAP_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB7 = PVP_dispell_ASAP_TB7.Text;
        }

        private void PVP_dispell_ASAP_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB8 = PVP_dispell_ASAP_TB8.Text;
        }

        private void PVP_dispell_ASAP_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB9 = PVP_dispell_ASAP_TB9.Text;
        }

        private void PVP_dispell_ASAP_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB10 = PVP_dispell_ASAP_TB10.Text;
        }

        private void PVP_dispell_ASAP_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB11 = PVP_dispell_ASAP_TB11.Text;
        }

        private void PVP_dispell_ASAP_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB12 = PVP_dispell_ASAP_TB12.Text;
        }

        private void PVP_dispell_ASAP_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB13 = PVP_dispell_ASAP_TB13.Text;
        }

        private void PVP_dispell_ASAP_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB14 = PVP_dispell_ASAP_TB14.Text;
        }

        private void PVP_dispell_ASAP_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB15 = PVP_dispell_ASAP_TB15.Text;
        }

        private void PVP_dispell_ASAP_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB16 = PVP_dispell_ASAP_TB16.Text;
        }

        private void PVP_dispell_ASAP_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB17 = PVP_dispell_ASAP_TB17.Text;
        }

        private void PVP_dispell_ASAP_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB18 = PVP_dispell_ASAP_TB18.Text;
        }

        private void PVP_dispell_ASAP_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB19 = PVP_dispell_ASAP_TB19.Text;
        }

        private void PVP_dispell_ASAP_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVP_dispell_ASAP_TB20 = PVP_dispell_ASAP_TB20.Text;
        }

        private void PVE_do_not_touch_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB1 = PVE_do_not_touch_TB1.Text;
        }

        private void PVE_do_not_touch_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB2 = PVE_do_not_touch_TB2.Text;
        }

        private void PVE_do_not_touch_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB3 = PVE_do_not_touch_TB3.Text;
        }

        private void PVE_do_not_touch_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB4 = PVE_do_not_touch_TB4.Text;
        }

        private void PVE_do_not_touch_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB5 = PVE_do_not_touch_TB5.Text;
        }

        private void PVE_do_not_touch_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB6 = PVE_do_not_touch_TB6.Text;
        }

        private void PVE_do_not_touch_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB7 = PVE_do_not_touch_TB7.Text;
        }

        private void PVE_do_not_touch_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB8 = PVE_do_not_touch_TB8.Text;
        }

        private void PVE_do_not_touch_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB9 = PVE_do_not_touch_TB9.Text;
        }

        private void PVE_do_not_touch_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB10 = PVE_do_not_touch_TB10.Text;
        }

        private void PVE_do_not_touch_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB11 = PVE_do_not_touch_TB11.Text;
        }

        private void PVE_do_not_touch_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB12 = PVE_do_not_touch_TB12.Text;
        }

        private void PVE_do_not_touch_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB13 = PVE_do_not_touch_TB13.Text;
        }

        private void PVE_do_not_touch_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB14 = PVE_do_not_touch_TB14.Text;
        }

        private void PVE_do_not_touch_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB15 = PVE_do_not_touch_TB15.Text;
        }

        private void PVE_do_not_touch_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB16 = PVE_do_not_touch_TB16.Text;
        }

        private void PVE_do_not_touch_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB17 = PVE_do_not_touch_TB17.Text;
        }

        private void PVE_do_not_touch_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB18 = PVE_do_not_touch_TB18.Text;
        }

        private void PVE_do_not_touch_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB19 = PVE_do_not_touch_TB19.Text;
        }

        private void PVE_do_not_touch_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_do_not_touch_TB20 = PVE_do_not_touch_TB20.Text;
        }

        private void PVE_dispell_ASAP_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB1 = PVE_dispell_ASAP_TB1.Text;
        }

        private void PVE_dispell_ASAP_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB2 = PVE_dispell_ASAP_TB2.Text;
        }

        private void PVE_dispell_ASAP_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB3 = PVE_dispell_ASAP_TB3.Text;
        }

        private void PVE_dispell_ASAP_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB4 = PVE_dispell_ASAP_TB4.Text;
        }

        private void PVE_dispell_ASAP_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB5 = PVE_dispell_ASAP_TB5.Text;
        }

        private void PVE_dispell_ASAP_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB6 = PVE_dispell_ASAP_TB6.Text;
        }

        private void PVE_dispell_ASAP_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB7 = PVE_dispell_ASAP_TB7.Text;
        }

        private void PVE_dispell_ASAP_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB8 = PVE_dispell_ASAP_TB8.Text;
        }

        private void PVE_dispell_ASAP_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB9 = PVE_dispell_ASAP_TB9.Text;
        }

        private void PVE_dispell_ASAP_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB10 = PVE_dispell_ASAP_TB10.Text;
        }

        private void PVE_dispell_ASAP_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB11 = PVE_dispell_ASAP_TB11.Text;
        }

        private void PVE_dispell_ASAP_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB12 = PVE_dispell_ASAP_TB12.Text;
        }

        private void PVE_dispell_ASAP_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB13 = PVE_dispell_ASAP_TB13.Text;
        }

        private void PVE_dispell_ASAP_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB14 = PVE_dispell_ASAP_TB14.Text;
        }

        private void PVE_dispell_ASAP_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB15 = PVE_dispell_ASAP_TB15.Text;
        }

        private void PVE_dispell_ASAP_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB16 = PVE_dispell_ASAP_TB16.Text;
        }

        private void PVE_dispell_ASAP_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB17 = PVE_dispell_ASAP_TB17.Text;
        }

        private void PVE_dispell_ASAP_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB18 = PVE_dispell_ASAP_TB18.Text;
        }

        private void PVE_dispell_ASAP_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB19 = PVE_dispell_ASAP_TB19.Text;
        }

        private void PVE_dispell_ASAP_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPrHBTSetting.Instance.PVE_dispell_ASAP_TB20 = PVE_dispell_ASAP_TB20.Text;
        }

        private void cbLogging_CheckedChanged(object sender, EventArgs e)
        {
            UPrHBTSetting.Instance.WriteLog = cbLogging.Checked;
        }
    }
}
