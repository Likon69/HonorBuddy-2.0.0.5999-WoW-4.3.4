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
    public partial class UPaHCCBTConfigForm : Form
    {
    
        public UPaHCCBTConfigForm()
        {
            InitializeComponent();
        }


        private void UPaHCCBTConfig_Load(object sender, EventArgs e)
        {
            
            Logging.Write("Settings Panel Opened");
            UPaHBTSetting.Instance.Load();
            Populate_components();
            Change_advanced_option_visibility();
            UltimatePalaHealerBT.Instance.Load_Trinket();
            SelectHeal.Checked = UPaHBTSetting.Instance.Selective_Healing;
            RaidCK0.Checked = UPaHBTSetting.Instance.Heal_raid_member0;
            RaidCK1.Checked = UPaHBTSetting.Instance.Heal_raid_member1;
            RaidCK2.Checked = UPaHBTSetting.Instance.Heal_raid_member2;
            RaidCK3.Checked = UPaHBTSetting.Instance.Heal_raid_member3;
            RaidCK4.Checked = UPaHBTSetting.Instance.Heal_raid_member4;
            RaidCK5.Checked = UPaHBTSetting.Instance.Heal_raid_member5;
            RaidCK6.Checked = UPaHBTSetting.Instance.Heal_raid_member6;
            RaidCK7.Checked = UPaHBTSetting.Instance.Heal_raid_member7;
            RaidCK8.Checked = UPaHBTSetting.Instance.Heal_raid_member8;
            RaidCK9.Checked = UPaHBTSetting.Instance.Heal_raid_member9;
            RaidCK10.Checked = UPaHBTSetting.Instance.Heal_raid_member10;
            RaidCK11.Checked = UPaHBTSetting.Instance.Heal_raid_member11;
            RaidCK12.Checked = UPaHBTSetting.Instance.Heal_raid_member12;
            RaidCK13.Checked = UPaHBTSetting.Instance.Heal_raid_member13;
            RaidCK14.Checked = UPaHBTSetting.Instance.Heal_raid_member14;
            RaidCK15.Checked = UPaHBTSetting.Instance.Heal_raid_member15;
            RaidCK16.Checked = UPaHBTSetting.Instance.Heal_raid_member16;
            RaidCK17.Checked = UPaHBTSetting.Instance.Heal_raid_member17;
            RaidCK18.Checked = UPaHBTSetting.Instance.Heal_raid_member18;
            RaidCK19.Checked = UPaHBTSetting.Instance.Heal_raid_member19;
            RaidCK20.Checked = UPaHBTSetting.Instance.Heal_raid_member20;
            RaidCK21.Checked = UPaHBTSetting.Instance.Heal_raid_member21;
            RaidCK22.Checked = UPaHBTSetting.Instance.Heal_raid_member22;
            RaidCK23.Checked = UPaHBTSetting.Instance.Heal_raid_member23;
            RaidCK24.Checked = UPaHBTSetting.Instance.Heal_raid_member24;
            RaidCK25.Checked = UPaHBTSetting.Instance.Heal_raid_member25;
            RaidCK26.Checked = UPaHBTSetting.Instance.Heal_raid_member26;
            RaidCK27.Checked = UPaHBTSetting.Instance.Heal_raid_member27;
            RaidCK28.Checked = UPaHBTSetting.Instance.Heal_raid_member28;
            RaidCK29.Checked = UPaHBTSetting.Instance.Heal_raid_member29;
            RaidCK30.Checked = UPaHBTSetting.Instance.Heal_raid_member30;
            RaidCK31.Checked = UPaHBTSetting.Instance.Heal_raid_member31;
            RaidCK32.Checked = UPaHBTSetting.Instance.Heal_raid_member32;
            RaidCK33.Checked = UPaHBTSetting.Instance.Heal_raid_member33;
            RaidCK34.Checked = UPaHBTSetting.Instance.Heal_raid_member34;
            RaidCK35.Checked = UPaHBTSetting.Instance.Heal_raid_member35;
            RaidCK36.Checked = UPaHBTSetting.Instance.Heal_raid_member36;
            RaidCK37.Checked = UPaHBTSetting.Instance.Heal_raid_member37;
            RaidCK38.Checked = UPaHBTSetting.Instance.Heal_raid_member38;
            RaidCK39.Checked = UPaHBTSetting.Instance.Heal_raid_member39;

            General_precasting.Value = new decimal(UPaHBTSetting.Instance.General_precasting);

            Trinket1_name_LB.Text = "Name: " + UPaHBTSetting.Instance.Trinket1_name;
            Trinket1_ID_LB.Text = "ID: " + UPaHBTSetting.Instance.Trinket1_ID;
            if (!UPaHBTSetting.Instance.Trinket1_passive)
            {
                Trinket1_use_when_GB.Enabled = true;
                Trinket1_CD_LB.Text = "Cooldown: " + UPaHBTSetting.Instance.Trinket1_CD + " seconds max";
                if (UPaHBTSetting.Instance.Trinket1_use_when == 0) { Trinket1_never.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket1_use_when == 1) { Trinket1_HPS.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket1_use_when == 2) { Trinket1_mana.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket1_use_when == 3) { Trinket1_PVP.Checked = true; }
            }
            else
            {
                Trinket1_CD_LB.Text = "This Trinket is a Passive one, nothing to do";
                Trinket1_never.Checked = true;
                Trinket1_use_when_GB.Enabled = false;
            }

            Trinket2_name_LB.Text = "Name: " + UPaHBTSetting.Instance.Trinket2_name;
            Trinket2_ID_LB.Text = "ID: " + UPaHBTSetting.Instance.Trinket2_ID;
            if (!UPaHBTSetting.Instance.Trinket2_passive)
            {
                Trinket2_use_when_GB.Enabled = true;
                Trinket2_CD_LB.Text = "Cooldown: " + UPaHBTSetting.Instance.Trinket2_CD + " seconds max";
                if (UPaHBTSetting.Instance.Trinket2_use_when == 0) { Trinket2_never.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket2_use_when == 1) { Trinket2_HPS.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket2_use_when == 2) { Trinket2_mana.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket2_use_when == 3) { Trinket2_PVP.Checked = true; }
            }
            else
            {
                Trinket2_CD_LB.Text = "This Trinket is a Passive one, nothing to do";
                Trinket2_never.Checked = true;
                Trinket2_use_when_GB.Enabled = false;
            }
            /*
            Trinket1_name_LB.Text = "Name: " + UPaHBTSetting.Instance.Trinket1_name;
            Trinket1_CD_LB.Text = "Cooldown: " + UPaHBTSetting.Instance.Trinket1_CD + " seconds max";
            Trinket1_ID_LB.Text = "ID: " + UPaHBTSetting.Instance.Trinket1_ID;
            Trinket2_name_LB.Text = "Name: " + UPaHBTSetting.Instance.Trinket2_name;
            Trinket2_CD_LB.Text = "Cooldown: " + UPaHBTSetting.Instance.Trinket2_CD + " seconds max";
            Trinket2_ID_LB.Text = "ID: " + UPaHBTSetting.Instance.Trinket2_ID;
            if (UPaHBTSetting.Instance.Trinket1_use_when == 0) { Trinket1_never.Checked = true; }
            else if (UPaHBTSetting.Instance.Trinket1_use_when == 1) { Trinket1_HPS.Checked = true; }
            else if (UPaHBTSetting.Instance.Trinket1_use_when == 2) { Trinket1_mana.Checked = true; }
            else if (UPaHBTSetting.Instance.Trinket1_use_when == 3) { Trinket1_PVP.Checked = true; }
            if (UPaHBTSetting.Instance.Trinket2_use_when == 0) { Trinket2_never.Checked = true; }
            else if (UPaHBTSetting.Instance.Trinket2_use_when == 1) { Trinket2_HPS.Checked = true; }
            else if (UPaHBTSetting.Instance.Trinket2_use_when == 2) { Trinket2_mana.Checked = true; }
            else if (UPaHBTSetting.Instance.Trinket2_use_when == 3) { Trinket2_PVP.Checked = true; }
*/

            if (UPaHBTSetting.Instance.General_Stop_Healing == false)
            {
                Stop_Healing_BT.Text = "Stop ALL Healing";
                Stop_Healing_BT.ForeColor = Color.Red;
                
            }
            else
            {   
                Stop_Healing_BT.Text = "START Healing Again";
                Stop_Healing_BT.ForeColor = Color.Green;
            }

            PVP_do_not_touch_TB1.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB1;
            PVP_do_not_touch_TB2.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB2;
            PVP_do_not_touch_TB3.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB3;
            PVP_do_not_touch_TB4.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB4;
            PVP_do_not_touch_TB5.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB5;
            PVP_do_not_touch_TB6.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB6;
            PVP_do_not_touch_TB7.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB7;
            PVP_do_not_touch_TB8.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB8;
            PVP_do_not_touch_TB9.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB9;
            PVP_do_not_touch_TB10.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB10;
            PVP_do_not_touch_TB11.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB11;
            PVP_do_not_touch_TB12.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB12;
            PVP_do_not_touch_TB13.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB13;
            PVP_do_not_touch_TB14.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB14;
            PVP_do_not_touch_TB15.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB15;
            PVP_do_not_touch_TB16.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB16;
            PVP_do_not_touch_TB17.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB17;
            PVP_do_not_touch_TB18.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB18;
            PVP_do_not_touch_TB19.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB19;
            PVP_do_not_touch_TB20.Text = UPaHBTSetting.Instance.PVP_do_not_touch_TB20;

            PVP_dispell_ASAP_TB1.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB1;
            PVP_dispell_ASAP_TB2.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB2;
            PVP_dispell_ASAP_TB3.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB3;
            PVP_dispell_ASAP_TB4.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB4;
            PVP_dispell_ASAP_TB5.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB5;
            PVP_dispell_ASAP_TB6.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB6;
            PVP_dispell_ASAP_TB7.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB7;
            PVP_dispell_ASAP_TB8.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB8;
            PVP_dispell_ASAP_TB9.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB9;
            PVP_dispell_ASAP_TB10.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB10;
            PVP_dispell_ASAP_TB11.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB11;
            PVP_dispell_ASAP_TB12.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB12;
            PVP_dispell_ASAP_TB13.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB13;
            PVP_dispell_ASAP_TB14.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB14;
            PVP_dispell_ASAP_TB15.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB15;
            PVP_dispell_ASAP_TB16.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB16;
            PVP_dispell_ASAP_TB17.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB17;
            PVP_dispell_ASAP_TB18.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB18;
            PVP_dispell_ASAP_TB19.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB19;
            PVP_dispell_ASAP_TB20.Text = UPaHBTSetting.Instance.PVP_dispell_ASAP_TB20;

            PVE_do_not_touch_TB1.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB1;
            PVE_do_not_touch_TB2.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB2;
            PVE_do_not_touch_TB3.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB3;
            PVE_do_not_touch_TB4.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB4;
            PVE_do_not_touch_TB5.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB5;
            PVE_do_not_touch_TB6.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB6;
            PVE_do_not_touch_TB7.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB7;
            PVE_do_not_touch_TB8.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB8;
            PVE_do_not_touch_TB9.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB9;
            PVE_do_not_touch_TB10.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB10;
            PVE_do_not_touch_TB11.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB11;
            PVE_do_not_touch_TB12.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB12;
            PVE_do_not_touch_TB13.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB13;
            PVE_do_not_touch_TB14.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB14;
            PVE_do_not_touch_TB15.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB15;
            PVE_do_not_touch_TB16.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB16;
            PVE_do_not_touch_TB17.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB17;
            PVE_do_not_touch_TB18.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB18;
            PVE_do_not_touch_TB19.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB19;
            PVE_do_not_touch_TB20.Text = UPaHBTSetting.Instance.PVE_do_not_touch_TB20;

            PVE_dispell_ASAP_TB1.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB1;
            PVE_dispell_ASAP_TB2.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB2;
            PVE_dispell_ASAP_TB3.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB3;
            PVE_dispell_ASAP_TB4.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB4;
            PVE_dispell_ASAP_TB5.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB5;
            PVE_dispell_ASAP_TB6.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB6;
            PVE_dispell_ASAP_TB7.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB7;
            PVE_dispell_ASAP_TB8.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB8;
            PVE_dispell_ASAP_TB9.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB9;
            PVE_dispell_ASAP_TB10.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB10;
            PVE_dispell_ASAP_TB11.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB11;
            PVE_dispell_ASAP_TB12.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB12;
            PVE_dispell_ASAP_TB13.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB13;
            PVE_dispell_ASAP_TB14.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB14;
            PVE_dispell_ASAP_TB15.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB15;
            PVE_dispell_ASAP_TB16.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB16;
            PVE_dispell_ASAP_TB17.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB17;
            PVE_dispell_ASAP_TB18.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB18;
            PVE_dispell_ASAP_TB19.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB19;
            PVE_dispell_ASAP_TB20.Text = UPaHBTSetting.Instance.PVE_dispell_ASAP_TB20;

            RAFtankfromfocus.Checked = UPaHBTSetting.Instance.RAF_get_tank_from_focus;
            ARENA2v2tankfromfocus.Checked = UPaHBTSetting.Instance.ARENA2v2_get_tank_from_focus;


            Solo_get_tank_from_focus.Checked = UPaHBTSetting.Instance.Solo_get_tank_from_focus;
            Solo_Inf_of_light_wanna_DL.Checked = UPaHBTSetting.Instance.Solo_Inf_of_light_wanna_DL;
            Solo_wanna_AW.Checked = UPaHBTSetting.Instance.Solo_wanna_AW;
            Solo_wanna_buff.Checked = UPaHBTSetting.Instance.Solo_wanna_buff;
            Solo_wanna_cleanse.Checked = UPaHBTSetting.Instance.Solo_wanna_cleanse;
            Solo_wanna_CS.Checked = UPaHBTSetting.Instance.Solo_wanna_CS;
            Solo_wanna_DF.Checked = UPaHBTSetting.Instance.Solo_wanna_DF;
            Solo_wanna_DP.Checked = UPaHBTSetting.Instance.Solo_wanna_DP;
            Solo_wanna_DS.Checked = UPaHBTSetting.Instance.Solo_wanna_DS;
            Solo_wanna_everymanforhimself.Checked = UPaHBTSetting.Instance.Solo_wanna_everymanforhimself;
            Solo_wanna_face.Checked = UPaHBTSetting.Instance.Solo_wanna_face;
            Solo_wanna_GotAK.Checked = UPaHBTSetting.Instance.Solo_wanna_GotAK;
            Solo_wanna_gift.Checked = UPaHBTSetting.Instance.Solo_wanna_gift;
            Solo_wanna_HoJ.Checked = UPaHBTSetting.Instance.Solo_wanna_HoJ;
            Solo_wanna_HoP.Checked = UPaHBTSetting.Instance.Solo_wanna_HoP;
            Solo_wanna_HoS.Checked = UPaHBTSetting.Instance.Solo_wanna_HoS;
            Solo_wanna_HoW.Checked = UPaHBTSetting.Instance.Solo_wanna_HoW;
            Solo_wanna_HR.Checked = UPaHBTSetting.Instance.Solo_wanna_HR;
            Solo_wanna_Judge.Checked = UPaHBTSetting.Instance.Solo_wanna_Judge;
            Solo_wanna_LoH.Checked = UPaHBTSetting.Instance.Solo_wanna_LoH;
            Solo_wanna_mana_potion.Checked = UPaHBTSetting.Instance.Solo_wanna_mana_potion;
            Solo_wanna_mount.Checked = UPaHBTSetting.Instance.Solo_wanna_mount;
            Solo_wanna_move_to_heal.Checked = UPaHBTSetting.Instance.Solo_wanna_move_to_heal;
            Solo_wanna_move_to_HoJ.Checked = UPaHBTSetting.Instance.Solo_wanna_move_to_HoJ;
            Solo_wanna_rebuke.Checked = UPaHBTSetting.Instance.Solo_wanna_rebuke;
            Solo_wanna_stoneform.Checked = UPaHBTSetting.Instance.Solo_wanna_stoneform;
            Solo_wanna_target.Checked = UPaHBTSetting.Instance.Solo_wanna_target;
            Solo_wanna_torrent.Checked = UPaHBTSetting.Instance.Solo_wanna_torrent;
            Solo_wanna_urgent_cleanse.Checked = UPaHBTSetting.Instance.Solo_wanna_urgent_cleanse;
            if (UPaHBTSetting.Instance.Solo_aura_type == 0) { Solo_concentrationRB.Checked=true; }
            else if (UPaHBTSetting.Instance.Solo_aura_type == 1) { Solo_resistanceRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Solo_aura_type == 2) { Solo_DisabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.Solo_bless_type == 0) { Solo_bless_type_autoRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Solo_bless_type == 1) { Solo_bless_type_KingRB.Checked=true; }
            else if (UPaHBTSetting.Instance.Solo_bless_type == 2) { Solo_bless_type_MightRB.Checked=true; }
            else if (UPaHBTSetting.Instance.Solo_bless_type == 4) { Solo_bless_type_disabledRB.Checked=true; }
            if (UPaHBTSetting.Instance.Solo_intellywait == true) { Solo_intellywait.Checked = true; }
            else if (UPaHBTSetting.Instance.Solo_decice_during_GCD == false) { Solo_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.Solo_decice_during_GCD == true) { Solo_speed.Checked = true; }
            Solo_do_not_heal_above.Value = new decimal(UPaHBTSetting.Instance.Solo_do_not_heal_above);
            Solo_advanced_option.Checked = UPaHBTSetting.Instance.Solo_advanced_option;
            Solo_HR_how_far.Value = new decimal(UPaHBTSetting.Instance.Solo_HR_how_far);
            Solo_HR_how_much_health.Value = new decimal(UPaHBTSetting.Instance.Solo_HR_how_much_health);
            Solo_mana_judge.Value = new decimal(UPaHBTSetting.Instance.Solo_mana_judge);
            Solo_max_healing_distance.Value = new decimal(UPaHBTSetting.Instance.Solo_max_healing_distance);
            Solo_min_Divine_Plea_mana.Value = new decimal(UPaHBTSetting.Instance.Solo_min_Divine_Plea_mana);
            Solo_min_DL_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_DL_hp);
            Solo_min_DP_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_DP_hp);
            Solo_min_DS_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_DS_hp);
            Solo_min_FoL_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_FoL_hp);
            Solo_min_gift_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_gift_hp);
            Solo_min_HL_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_HL_hp);
            Solo_min_HoP_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_HoP_hp);
            Solo_min_HoS_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_HoS_hp);
            Solo_min_Inf_of_light_DL_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_Inf_of_light_DL_hp);
            Solo_min_LoH_hp.Value = new decimal(UPaHBTSetting.Instance.Solo_min_LoH_hp);
            Solo_min_mana_potion.Value = new decimal(UPaHBTSetting.Instance.Solo_min_mana_potion);
            Solo_min_mana_rec_trinket.Value = new decimal(UPaHBTSetting.Instance.Solo_min_mana_rec_trinket);
            Solo_min_ohshitbutton_activator.Value = new decimal(UPaHBTSetting.Instance.Solo_min_ohshitbutton_activator);
            Solo_min_player_inside_HR.Value = new decimal(UPaHBTSetting.Instance.Solo_min_player_inside_HR);
            Solo_min_stoneform.Value = new decimal(UPaHBTSetting.Instance.Solo_min_stoneform);
            Solo_min_torrent_mana_perc.Value = new decimal(UPaHBTSetting.Instance.Solo_min_torrent_mana_perc);
            Solo_rest_if_mana_below.Value = new decimal(UPaHBTSetting.Instance.Solo_rest_if_mana_below);
            Solo_use_mana_rec_trinket_every.Value = new decimal(UPaHBTSetting.Instance.Solo_use_mana_rec_trinket_every);
            Solo_wanna_move.Checked = UPaHBTSetting.Instance.Solo_wanna_move;
            Solo_wanna_crusader.Checked = UPaHBTSetting.Instance.Solo_wanna_crusader;
            Solo_wanna_lifeblood.Checked = UPaHBTSetting.Instance.Solo_wanna_lifeblood;
            Solo_do_not_dismount_ooc.Checked = UPaHBTSetting.Instance.Solo_do_not_dismount_ooc;
            Solo_do_not_dismount_EVER.Checked = UPaHBTSetting.Instance.Solo_do_not_dismount_EVER;
            Solo_get_tank_from_lua.Checked = UPaHBTSetting.Instance.Solo_get_tank_from_lua;
            Solo_answer_PVP_attacks.Checked = UPaHBTSetting.Instance.Solo_answer_PVP_attacks;
            Solo_enable_pull.Checked = UPaHBTSetting.Instance.Solo_enable_pull;
      //      BuffTT.SetToolTip(this.Solo_wanna_buff, "Enable or Disable buffing outside of combat, CC will still make sure Seal of Insign and Beacon are applied");
       //     Solo_TantfocusTT.SetToolTip(this.Solotankfromfocus, "If enabled will set your Focus as the CC tank");

            PVE_get_tank_from_focus.Checked = UPaHBTSetting.Instance.PVE_get_tank_from_focus;
            PVE_stop_DL_if_above.Value = new decimal(UPaHBTSetting.Instance.PVE_stop_DL_if_above);
            PVE_Inf_of_light_wanna_DL.Checked = UPaHBTSetting.Instance.PVE_Inf_of_light_wanna_DL;
            PVE_wanna_AW.Checked = UPaHBTSetting.Instance.PVE_wanna_AW;
            PVE_wanna_buff.Checked = UPaHBTSetting.Instance.PVE_wanna_buff;
            PVE_wanna_cleanse.Checked = UPaHBTSetting.Instance.PVE_wanna_cleanse;
            PVE_wanna_CS.Checked = UPaHBTSetting.Instance.PVE_wanna_CS;
            PVE_wanna_DF.Checked = UPaHBTSetting.Instance.PVE_wanna_DF;
            PVE_wanna_DP.Checked = UPaHBTSetting.Instance.PVE_wanna_DP;
            PVE_wanna_DS.Checked = UPaHBTSetting.Instance.PVE_wanna_DS;
            PVE_wanna_everymanforhimself.Checked = UPaHBTSetting.Instance.PVE_wanna_everymanforhimself;
            PVE_wanna_face.Checked = UPaHBTSetting.Instance.PVE_wanna_face;
            PVE_wanna_GotAK.Checked = UPaHBTSetting.Instance.PVE_wanna_GotAK;
            PVE_wanna_gift.Checked = UPaHBTSetting.Instance.PVE_wanna_gift;
            PVE_wanna_HoJ.Checked = UPaHBTSetting.Instance.PVE_wanna_HoJ;
            PVE_wanna_HoP.Checked = UPaHBTSetting.Instance.PVE_wanna_HoP;
            PVE_wanna_HoS.Checked = UPaHBTSetting.Instance.PVE_wanna_HoS;
            PVE_wanna_HoW.Checked = UPaHBTSetting.Instance.PVE_wanna_HoW;
            PVE_wanna_HR.Checked = UPaHBTSetting.Instance.PVE_wanna_HR;
            PVE_wanna_Judge.Checked = UPaHBTSetting.Instance.PVE_wanna_Judge;
            PVE_wanna_LoH.Checked = UPaHBTSetting.Instance.PVE_wanna_LoH;
            PVE_wanna_mana_potion.Checked = UPaHBTSetting.Instance.PVE_wanna_mana_potion;
            PVE_wanna_mount.Checked = UPaHBTSetting.Instance.PVE_wanna_mount;
            PVE_wanna_move_to_heal.Checked = UPaHBTSetting.Instance.PVE_wanna_move_to_heal;
            PVE_wanna_move_to_HoJ.Checked = UPaHBTSetting.Instance.PVE_wanna_move_to_HoJ;
            PVE_wanna_rebuke.Checked = UPaHBTSetting.Instance.PVE_wanna_rebuke;
            PVE_wanna_stoneform.Checked = UPaHBTSetting.Instance.PVE_wanna_stoneform;
            PVE_wanna_target.Checked = UPaHBTSetting.Instance.PVE_wanna_target;
            PVE_wanna_torrent.Checked = UPaHBTSetting.Instance.PVE_wanna_torrent;
            PVE_wanna_urgent_cleanse.Checked = UPaHBTSetting.Instance.PVE_wanna_urgent_cleanse;
            if (UPaHBTSetting.Instance.PVE_aura_type == 0) { PVE_concentrationRB.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_aura_type == 1) { PVE_resistanceRB.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_aura_type == 2) { PVE_DisabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.PVE_bless_type == 0) { PVE_bless_type_autoRB.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_bless_type == 1) { PVE_bless_type_KingRB.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_bless_type == 2) { PVE_bless_type_MightRB.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_bless_type == 4) { PVE_bless_type_disabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.PVE_intellywait == true) { PVE_intellywait.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_decice_during_GCD == false) { PVE_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.PVE_decice_during_GCD == true) { PVE_speed.Checked = true; }
            PVE_do_not_heal_above.Value = new decimal(UPaHBTSetting.Instance.PVE_do_not_heal_above);
            PVE_advanced_option.Checked = UPaHBTSetting.Instance.PVE_advanced_option;
            PVE_HR_how_far.Value = new decimal(UPaHBTSetting.Instance.PVE_HR_how_far);
            PVE_HR_how_much_health.Value = new decimal(UPaHBTSetting.Instance.PVE_HR_how_much_health);
            PVE_mana_judge.Value = new decimal(UPaHBTSetting.Instance.PVE_mana_judge);
            PVE_max_healing_distance.Value = new decimal(UPaHBTSetting.Instance.PVE_max_healing_distance);
            PVE_min_Divine_Plea_mana.Value = new decimal(UPaHBTSetting.Instance.PVE_min_Divine_Plea_mana);
            PVE_min_DL_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_DL_hp);
            PVE_min_DP_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_DP_hp);
            PVE_min_DS_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_DS_hp);
            PVE_min_FoL_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_FoL_hp);
            PVE_min_gift_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_gift_hp);
            PVE_min_HL_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_HL_hp);
            PVE_min_HoP_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_HoP_hp);
            PVE_min_HoS_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_HoS_hp);
            PVE_min_Inf_of_light_DL_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_Inf_of_light_DL_hp);
            PVE_min_LoH_hp.Value = new decimal(UPaHBTSetting.Instance.PVE_min_LoH_hp);
            PVE_min_mana_potion.Value = new decimal(UPaHBTSetting.Instance.PVE_min_mana_potion);
            PVE_min_mana_rec_trinket.Value = new decimal(UPaHBTSetting.Instance.PVE_min_mana_rec_trinket);
            PVE_min_ohshitbutton_activator.Value = new decimal(UPaHBTSetting.Instance.PVE_min_ohshitbutton_activator);
            PVE_min_player_inside_HR.Value = new decimal(UPaHBTSetting.Instance.PVE_min_player_inside_HR);
            PVE_min_stoneform.Value = new decimal(UPaHBTSetting.Instance.PVE_min_stoneform);
            PVE_min_torrent_mana_perc.Value = new decimal(UPaHBTSetting.Instance.PVE_min_torrent_mana_perc);
            PVE_rest_if_mana_below.Value = new decimal(UPaHBTSetting.Instance.PVE_rest_if_mana_below);
            PVE_use_mana_rec_trinket_every.Value = new decimal(UPaHBTSetting.Instance.PVE_use_mana_rec_trinket_every);
            PVE_wanna_crusader.Checked = UPaHBTSetting.Instance.PVE_wanna_crusader;
            PVE_wanna_lifeblood.Checked = UPaHBTSetting.Instance.PVE_wanna_lifeblood;
            PVE_do_not_dismount_ooc.Checked = UPaHBTSetting.Instance.PVE_do_not_dismount_ooc;
            PVE_do_not_dismount_EVER.Checked = UPaHBTSetting.Instance.PVE_do_not_dismount_EVER;
            PVE_get_tank_from_lua.Checked = UPaHBTSetting.Instance.PVE_get_tank_from_lua;
            PVE_healing_tank_priority.Value = new decimal(UPaHBTSetting.Instance.PVE_healing_tank_priority);
            PVE_tank_healing_priority_multiplier.Value = new decimal(UPaHBTSetting.Instance.PVE_tank_healing_priority_multiplier);
            PVE_cleanse_only_self_and_tank.Checked = UPaHBTSetting.Instance.PVE_cleanse_only_self_and_tank;
            PVE_cleanse_only_self_and_tank.Enabled = UPaHBTSetting.Instance.PVE_wanna_cleanse;

            ARENA_get_tank_from_focus.Checked = UPaHBTSetting.Instance.ARENA_get_tank_from_focus;
            ARENA_Inf_of_light_wanna_DL.Checked = UPaHBTSetting.Instance.ARENA_Inf_of_light_wanna_DL;
            ARENA_wanna_AW.Checked = UPaHBTSetting.Instance.ARENA_wanna_AW;
            ARENA_wanna_buff.Checked = UPaHBTSetting.Instance.ARENA_wanna_buff;
            ARENA_wanna_cleanse.Checked = UPaHBTSetting.Instance.ARENA_wanna_cleanse;
            ARENA_wanna_CS.Checked = UPaHBTSetting.Instance.ARENA_wanna_CS;
            ARENA_wanna_DF.Checked = UPaHBTSetting.Instance.ARENA_wanna_DF;
            ARENA_wanna_DP.Checked = UPaHBTSetting.Instance.ARENA_wanna_DP;
            ARENA_wanna_DS.Checked = UPaHBTSetting.Instance.ARENA_wanna_DS;
            ARENA_wanna_everymanforhimself.Checked = UPaHBTSetting.Instance.ARENA_wanna_everymanforhimself;
            ARENA_wanna_face.Checked = UPaHBTSetting.Instance.ARENA_wanna_face;
            ARENA_wanna_GotAK.Checked = UPaHBTSetting.Instance.ARENA_wanna_GotAK;
            ARENA_wanna_gift.Checked = UPaHBTSetting.Instance.ARENA_wanna_gift;
            ARENA_wanna_HoJ.Checked = UPaHBTSetting.Instance.ARENA_wanna_HoJ;
            ARENA_wanna_HoP.Checked = UPaHBTSetting.Instance.ARENA_wanna_HoP;
            ARENA_wanna_HoS.Checked = UPaHBTSetting.Instance.ARENA_wanna_HoS;
            ARENA_wanna_HoW.Checked = UPaHBTSetting.Instance.ARENA_wanna_HoW;
            ARENA_wanna_HR.Checked = UPaHBTSetting.Instance.ARENA_wanna_HR;
            ARENA_wanna_Judge.Checked = UPaHBTSetting.Instance.ARENA_wanna_Judge;
            ARENA_wanna_LoH.Checked = UPaHBTSetting.Instance.ARENA_wanna_LoH;
            ARENA_wanna_mana_potion.Checked = UPaHBTSetting.Instance.ARENA_wanna_mana_potion;
            ARENA_wanna_mount.Checked = UPaHBTSetting.Instance.ARENA_wanna_mount;
            ARENA_wanna_move_to_heal.Checked = UPaHBTSetting.Instance.ARENA_wanna_move_to_heal;
            ARENA_wanna_move_to_HoJ.Checked = UPaHBTSetting.Instance.ARENA_wanna_move_to_HoJ;
            ARENA_wanna_rebuke.Checked = UPaHBTSetting.Instance.ARENA_wanna_rebuke;
            ARENA_wanna_stoneform.Checked = UPaHBTSetting.Instance.ARENA_wanna_stoneform;
            ARENA_wanna_target.Checked = UPaHBTSetting.Instance.ARENA_wanna_target;
            ARENA_wanna_torrent.Checked = UPaHBTSetting.Instance.ARENA_wanna_torrent;
            ARENA_wanna_urgent_cleanse.Checked = UPaHBTSetting.Instance.ARENA_wanna_urgent_cleanse;
            if (UPaHBTSetting.Instance.ARENA_aura_type == 0) { ARENA_concentrationRB.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_aura_type == 1) { ARENA_resistanceRB.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_aura_type == 2) { ARENA_DisabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.ARENA_bless_type == 0) { ARENA_bless_type_autoRB.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_bless_type == 1) { ARENA_bless_type_KingRB.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_bless_type == 2) { ARENA_bless_type_MightRB.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_bless_type == 4) { ARENA_bless_type_disabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.ARENA_decice_during_GCD == false) { ARENA_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_decice_during_GCD == true) { ARENA_speed.Checked = true; }
            if (UPaHBTSetting.Instance.ARENA_intellywait == true) { ARENA_intellywait.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_decice_during_GCD == false) { ARENA_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.ARENA_decice_during_GCD == true) { ARENA_speed.Checked = true; }
            ARENA_do_not_heal_above.Value = new decimal(UPaHBTSetting.Instance.ARENA_do_not_heal_above);
            ARENA_advanced_option.Checked = UPaHBTSetting.Instance.ARENA_advanced_option;
            ARENA_HR_how_far.Value = new decimal(UPaHBTSetting.Instance.ARENA_HR_how_far);
            ARENA_HR_how_much_health.Value = new decimal(UPaHBTSetting.Instance.ARENA_HR_how_much_health);
            ARENA_mana_judge.Value = new decimal(UPaHBTSetting.Instance.ARENA_mana_judge);
            ARENA_max_healing_distance.Value = new decimal(UPaHBTSetting.Instance.ARENA_max_healing_distance);
            ARENA_min_Divine_Plea_mana.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_Divine_Plea_mana);
            ARENA_min_DL_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_DL_hp);
            ARENA_min_DP_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_DP_hp);
            ARENA_min_DS_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_DS_hp);
            ARENA_min_FoL_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_FoL_hp);
            ARENA_min_gift_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_gift_hp);
            ARENA_min_HL_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_HL_hp);
            ARENA_min_HoP_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_HoP_hp);
            ARENA_min_HoS_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_HoS_hp);
            ARENA_min_Inf_of_light_DL_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_Inf_of_light_DL_hp);
            ARENA_min_LoH_hp.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_LoH_hp);
            ARENA_min_mana_potion.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_mana_potion);
            ARENA_min_mana_rec_trinket.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_mana_rec_trinket);
            ARENA_min_ohshitbutton_activator.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_ohshitbutton_activator);
            ARENA_min_player_inside_HR.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_player_inside_HR);
            ARENA_min_stoneform.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_stoneform);
            ARENA_min_torrent_mana_perc.Value = new decimal(UPaHBTSetting.Instance.ARENA_min_torrent_mana_perc);
            ARENA_rest_if_mana_below.Value = new decimal(UPaHBTSetting.Instance.ARENA_rest_if_mana_below);
            ARENA_use_mana_rec_trinket_every.Value = new decimal(UPaHBTSetting.Instance.ARENA_use_mana_rec_trinket_every);
            ARENA_wanna_crusader.Checked = UPaHBTSetting.Instance.ARENA_wanna_crusader;
            ARENA_wanna_lifeblood.Checked = UPaHBTSetting.Instance.ARENA_wanna_lifeblood;
            ARENA_do_not_dismount_ooc.Checked = UPaHBTSetting.Instance.ARENA_do_not_dismount_ooc;
            ARENA_do_not_dismount_EVER.Checked = UPaHBTSetting.Instance.ARENA_do_not_dismount_EVER;
            ARENA_get_tank_from_lua.Checked = UPaHBTSetting.Instance.ARENA_get_tank_from_lua;
            ARENA_wanna_HoF.Checked = UPaHBTSetting.Instance.ARENA_wanna_HoF;
            ARENA_wanna_taunt.Checked = UPaHBTSetting.Instance.ARENA_wanna_taunt;
            ARENA_cleanse_only_self_and_tank.Checked = UPaHBTSetting.Instance.ARENA_cleanse_only_self_and_tank;
            ARENA_cleanse_only_self_and_tank.Enabled = UPaHBTSetting.Instance.ARENA_wanna_cleanse;

            Raid_get_tank_from_focus.Checked = UPaHBTSetting.Instance.Raid_get_tank_from_focus;
            Raid_stop_DL_if_above.Value = new decimal(UPaHBTSetting.Instance.Raid_stop_DL_if_above);
            Raid_Inf_of_light_wanna_DL.Checked = UPaHBTSetting.Instance.Raid_Inf_of_light_wanna_DL;
            Raid_wanna_AW.Checked = UPaHBTSetting.Instance.Raid_wanna_AW;
            Raid_wanna_buff.Checked = UPaHBTSetting.Instance.Raid_wanna_buff;
            Raid_wanna_cleanse.Checked = UPaHBTSetting.Instance.Raid_wanna_cleanse;
            Raid_wanna_CS.Checked = UPaHBTSetting.Instance.Raid_wanna_CS;
            Raid_wanna_DF.Checked = UPaHBTSetting.Instance.Raid_wanna_DF;
            Raid_wanna_DP.Checked = UPaHBTSetting.Instance.Raid_wanna_DP;
            Raid_wanna_DS.Checked = UPaHBTSetting.Instance.Raid_wanna_DS;
            Raid_wanna_everymanforhimself.Checked = UPaHBTSetting.Instance.Raid_wanna_everymanforhimself;
            Raid_wanna_face.Checked = UPaHBTSetting.Instance.Raid_wanna_face;
            Raid_wanna_GotAK.Checked = UPaHBTSetting.Instance.Raid_wanna_GotAK;
            Raid_wanna_gift.Checked = UPaHBTSetting.Instance.Raid_wanna_gift;
            Raid_wanna_HoJ.Checked = UPaHBTSetting.Instance.Raid_wanna_HoJ;
            Raid_wanna_HoP.Checked = UPaHBTSetting.Instance.Raid_wanna_HoP;
            Raid_wanna_HoS.Checked = UPaHBTSetting.Instance.Raid_wanna_HoS;
            Raid_wanna_HoW.Checked = UPaHBTSetting.Instance.Raid_wanna_HoW;
            Raid_wanna_HR.Checked = UPaHBTSetting.Instance.Raid_wanna_HR;
            Raid_wanna_Judge.Checked = UPaHBTSetting.Instance.Raid_wanna_Judge;
            Raid_wanna_LoH.Checked = UPaHBTSetting.Instance.Raid_wanna_LoH;
            Raid_wanna_mana_potion.Checked = UPaHBTSetting.Instance.Raid_wanna_mana_potion;
            Raid_wanna_mount.Checked = UPaHBTSetting.Instance.Raid_wanna_mount;
            Raid_wanna_move_to_heal.Checked = UPaHBTSetting.Instance.Raid_wanna_move_to_heal;
            Raid_wanna_move_to_HoJ.Checked = UPaHBTSetting.Instance.Raid_wanna_move_to_HoJ;
            Raid_wanna_rebuke.Checked = UPaHBTSetting.Instance.Raid_wanna_rebuke;
            Raid_wanna_stoneform.Checked = UPaHBTSetting.Instance.Raid_wanna_stoneform;
            Raid_wanna_target.Checked = UPaHBTSetting.Instance.Raid_wanna_target;
            Raid_wanna_torrent.Checked = UPaHBTSetting.Instance.Raid_wanna_torrent;
            Raid_wanna_urgent_cleanse.Checked = UPaHBTSetting.Instance.Raid_wanna_urgent_cleanse;
            if (UPaHBTSetting.Instance.Raid_aura_type == 0) { Raid_concentrationRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_aura_type == 1) { Raid_resistanceRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_aura_type == 2) { Raid_DisabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.Raid_bless_type == 0) { Raid_bless_type_autoRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_bless_type == 1) { Raid_bless_type_KingRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_bless_type == 2) { Raid_bless_type_MightRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_bless_type == 4) { Raid_bless_type_disabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.Raid_intellywait == true) { Raid_intellywait.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_decice_during_GCD == false) { Raid_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.Raid_decice_during_GCD == true) { Raid_speed.Checked = true; }
            Raid_do_not_heal_above.Value = new decimal(UPaHBTSetting.Instance.Raid_do_not_heal_above);
            Raid_advanced_option.Checked = UPaHBTSetting.Instance.Raid_advanced_option;
            Raid_HR_how_far.Value = new decimal(UPaHBTSetting.Instance.Raid_HR_how_far);
            Raid_HR_how_much_health.Value = new decimal(UPaHBTSetting.Instance.Raid_HR_how_much_health);
            Raid_mana_judge.Value = new decimal(UPaHBTSetting.Instance.Raid_mana_judge);
            Raid_max_healing_distance.Value = new decimal(UPaHBTSetting.Instance.Raid_max_healing_distance);
            Raid_min_Divine_Plea_mana.Value = new decimal(UPaHBTSetting.Instance.Raid_min_Divine_Plea_mana);
            Raid_min_DL_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_DL_hp);
            Raid_min_DP_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_DP_hp);
            Raid_min_DS_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_DS_hp);
            Raid_min_FoL_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_FoL_hp);
            Raid_min_gift_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_gift_hp);
            Raid_min_HL_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_HL_hp);
            Raid_min_HoP_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_HoP_hp);
            Raid_min_HoS_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_HoS_hp);
            Raid_min_Inf_of_light_DL_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_Inf_of_light_DL_hp);
            Raid_min_LoH_hp.Value = new decimal(UPaHBTSetting.Instance.Raid_min_LoH_hp);
            Raid_min_mana_potion.Value = new decimal(UPaHBTSetting.Instance.Raid_min_mana_potion);
            Raid_min_mana_rec_trinket.Value = new decimal(UPaHBTSetting.Instance.Raid_min_mana_rec_trinket);
            Raid_min_ohshitbutton_activator.Value = new decimal(UPaHBTSetting.Instance.Raid_min_ohshitbutton_activator);
            Raid_min_player_inside_HR.Value = new decimal(UPaHBTSetting.Instance.Raid_min_player_inside_HR);
            Raid_min_stoneform.Value = new decimal(UPaHBTSetting.Instance.Raid_min_stoneform);
            Raid_min_torrent_mana_perc.Value = new decimal(UPaHBTSetting.Instance.Raid_min_torrent_mana_perc);
            Raid_rest_if_mana_below.Value = new decimal(UPaHBTSetting.Instance.Raid_rest_if_mana_below);
            Raid_use_mana_rec_trinket_every.Value = new decimal(UPaHBTSetting.Instance.Raid_use_mana_rec_trinket_every);
            Raid_wanna_crusader.Checked = UPaHBTSetting.Instance.Raid_wanna_crusader;
            Raid_wanna_lifeblood.Checked = UPaHBTSetting.Instance.Raid_wanna_lifeblood;
            Raid_do_not_dismount_ooc.Checked = UPaHBTSetting.Instance.Raid_do_not_dismount_ooc;
            Raid_do_not_dismount_EVER.Checked = UPaHBTSetting.Instance.Raid_do_not_dismount_EVER;
            Raid_get_tank_from_lua.Checked = UPaHBTSetting.Instance.Raid_get_tank_from_lua;
            Raid_ignore_beacon.Checked = UPaHBTSetting.Instance.Raid_ignore_beacon;
            Raid_healing_tank_priority.Value = new decimal(UPaHBTSetting.Instance.Raid_healing_tank_priority);
            Raid_tank_healing_priority_multiplier.Value = new decimal(UPaHBTSetting.Instance.Raid_tank_healing_priority_multiplier);
            Raid_cleanse_only_self_and_tank.Checked = UPaHBTSetting.Instance.Raid_cleanse_only_self_and_tank;
            Raid_cleanse_only_self_and_tank.Enabled = UPaHBTSetting.Instance.Raid_wanna_cleanse;

            Battleground_get_tank_from_focus.Checked = UPaHBTSetting.Instance.Battleground_get_tank_from_focus;
            Battleground_Inf_of_light_wanna_DL.Checked = UPaHBTSetting.Instance.Battleground_Inf_of_light_wanna_DL;
            Battleground_wanna_AW.Checked = UPaHBTSetting.Instance.Battleground_wanna_AW;
            Battleground_wanna_buff.Checked = UPaHBTSetting.Instance.Battleground_wanna_buff;
            Battleground_wanna_cleanse.Checked = UPaHBTSetting.Instance.Battleground_wanna_cleanse;
            Battleground_wanna_CS.Checked = UPaHBTSetting.Instance.Battleground_wanna_CS;
            Battleground_wanna_DF.Checked = UPaHBTSetting.Instance.Battleground_wanna_DF;
            Battleground_wanna_DP.Checked = UPaHBTSetting.Instance.Battleground_wanna_DP;
            Battleground_wanna_DS.Checked = UPaHBTSetting.Instance.Battleground_wanna_DS;
            Battleground_wanna_everymanforhimself.Checked = UPaHBTSetting.Instance.Battleground_wanna_everymanforhimself;
            Battleground_wanna_face.Checked = UPaHBTSetting.Instance.Battleground_wanna_face;
            Battleground_wanna_GotAK.Checked = UPaHBTSetting.Instance.Battleground_wanna_GotAK;
            Battleground_wanna_gift.Checked = UPaHBTSetting.Instance.Battleground_wanna_gift;
            Battleground_wanna_HoJ.Checked = UPaHBTSetting.Instance.Battleground_wanna_HoJ;
            Battleground_wanna_HoP.Checked = UPaHBTSetting.Instance.Battleground_wanna_HoP;
            Battleground_wanna_HoS.Checked = UPaHBTSetting.Instance.Battleground_wanna_HoS;
            Battleground_wanna_HoW.Checked = UPaHBTSetting.Instance.Battleground_wanna_HoW;
            Battleground_wanna_HR.Checked = UPaHBTSetting.Instance.Battleground_wanna_HR;
            Battleground_wanna_Judge.Checked = UPaHBTSetting.Instance.Battleground_wanna_Judge;
            Battleground_wanna_LoH.Checked = UPaHBTSetting.Instance.Battleground_wanna_LoH;
            Battleground_wanna_mana_potion.Checked = UPaHBTSetting.Instance.Battleground_wanna_mana_potion;
            Battleground_wanna_mount.Checked = UPaHBTSetting.Instance.Battleground_wanna_mount;
            Battleground_wanna_move_to_heal.Checked = UPaHBTSetting.Instance.Battleground_wanna_move_to_heal;
            Battleground_wanna_move_to_HoJ.Checked = UPaHBTSetting.Instance.Battleground_wanna_move_to_HoJ;
            Battleground_wanna_rebuke.Checked = UPaHBTSetting.Instance.Battleground_wanna_rebuke;
            Battleground_wanna_stoneform.Checked = UPaHBTSetting.Instance.Battleground_wanna_stoneform;
            Battleground_wanna_target.Checked = UPaHBTSetting.Instance.Battleground_wanna_target;
            Battleground_wanna_torrent.Checked = UPaHBTSetting.Instance.Battleground_wanna_torrent;
            Battleground_wanna_urgent_cleanse.Checked = UPaHBTSetting.Instance.Battleground_wanna_urgent_cleanse;
            if (UPaHBTSetting.Instance.Battleground_aura_type == 0) { Battleground_concentrationRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_aura_type == 1) { Battleground_resistanceRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_aura_type == 2) { Battleground_DisabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.Battleground_bless_type == 0) { Battleground_bless_type_autoRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_bless_type == 1) { Battleground_bless_type_KingRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_bless_type == 2) { Battleground_bless_type_MightRB.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_bless_type == 4) { Battleground_bless_type_disabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.Battleground_intellywait == true) { Battleground_intellywait.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_decice_during_GCD == false) { Battleground_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.Battleground_decice_during_GCD == true) { Battleground_speed.Checked = true; }
            Battleground_do_not_heal_above.Value = new decimal(UPaHBTSetting.Instance.Battleground_do_not_heal_above);
            Battleground_advanced_option.Checked = UPaHBTSetting.Instance.Battleground_advanced_option;
            Battleground_HR_how_far.Value = new decimal(UPaHBTSetting.Instance.Battleground_HR_how_far);
            Battleground_HR_how_much_health.Value = new decimal(UPaHBTSetting.Instance.Battleground_HR_how_much_health);
            Battleground_mana_judge.Value = new decimal(UPaHBTSetting.Instance.Battleground_mana_judge);
            Battleground_max_healing_distance.Value = new decimal(UPaHBTSetting.Instance.Battleground_max_healing_distance);
            Battleground_min_Divine_Plea_mana.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_Divine_Plea_mana);
            Battleground_min_DL_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_DL_hp);
            Battleground_min_DP_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_DP_hp);
            Battleground_min_DS_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_DS_hp);
            Battleground_min_FoL_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_FoL_hp);
            Battleground_min_gift_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_gift_hp);
            Battleground_min_HL_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_HL_hp);
            Battleground_min_HoP_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_HoP_hp);
            Battleground_min_HoS_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_HoS_hp);
            Battleground_min_Inf_of_light_DL_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_Inf_of_light_DL_hp);
            Battleground_min_LoH_hp.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_LoH_hp);
            Battleground_min_mana_potion.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_mana_potion);
            Battleground_min_mana_rec_trinket.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_mana_rec_trinket);
            Battleground_min_ohshitbutton_activator.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_ohshitbutton_activator);
            Battleground_min_player_inside_HR.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_player_inside_HR);
            Battleground_min_stoneform.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_stoneform);
            Battleground_min_torrent_mana_perc.Value = new decimal(UPaHBTSetting.Instance.Battleground_min_torrent_mana_perc);
            Battleground_rest_if_mana_below.Value = new decimal(UPaHBTSetting.Instance.Battleground_rest_if_mana_below);
            Battleground_use_mana_rec_trinket_every.Value = new decimal(UPaHBTSetting.Instance.Battleground_use_mana_rec_trinket_every);
            Battleground_wanna_crusader.Checked = UPaHBTSetting.Instance.Battleground_wanna_crusader;
            Battleground_wanna_lifeblood.Checked = UPaHBTSetting.Instance.Battleground_wanna_lifeblood;
            Battleground_do_not_dismount_ooc.Checked = UPaHBTSetting.Instance.Battleground_do_not_dismount_ooc;
            Battleground_do_not_dismount_EVER.Checked = UPaHBTSetting.Instance.Battleground_do_not_dismount_EVER;
            Battleground_get_tank_from_lua.Checked = UPaHBTSetting.Instance.Battleground_get_tank_from_lua;
            Battleground_cleanse_only_self_and_tank.Checked = UPaHBTSetting.Instance.Battleground_cleanse_only_self_and_tank;
            Battleground_cleanse_only_self_and_tank.Enabled = UPaHBTSetting.Instance.Battleground_wanna_cleanse;

            WorldPVP_get_tank_from_focus.Checked = UPaHBTSetting.Instance.WorldPVP_get_tank_from_focus;
            WorldPVP_Inf_of_light_wanna_DL.Checked = UPaHBTSetting.Instance.WorldPVP_Inf_of_light_wanna_DL;
            WorldPVP_wanna_AW.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_AW;
            WorldPVP_wanna_buff.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_buff;
            WorldPVP_wanna_cleanse.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_cleanse;
            WorldPVP_wanna_CS.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_CS;
            WorldPVP_wanna_DF.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_DF;
            WorldPVP_wanna_DP.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_DP;
            WorldPVP_wanna_DS.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_DS;
            WorldPVP_wanna_everymanforhimself.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_everymanforhimself;
            WorldPVP_wanna_face.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_face;
            WorldPVP_wanna_GotAK.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_GotAK;
            WorldPVP_wanna_gift.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_gift;
            WorldPVP_wanna_HoJ.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_HoJ;
            WorldPVP_wanna_HoP.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_HoP;
            WorldPVP_wanna_HoS.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_HoS;
            WorldPVP_wanna_HoW.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_HoW;
            WorldPVP_wanna_HR.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_HR;
            WorldPVP_wanna_Judge.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_Judge;
            WorldPVP_wanna_LoH.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_LoH;
            WorldPVP_wanna_mana_potion.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_mana_potion;
            WorldPVP_wanna_mount.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_mount;
            WorldPVP_wanna_move_to_heal.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_move_to_heal;
            WorldPVP_wanna_move_to_HoJ.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_move_to_HoJ;
            WorldPVP_wanna_rebuke.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_rebuke;
            WorldPVP_wanna_stoneform.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_stoneform;
            WorldPVP_wanna_target.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_target;
            WorldPVP_wanna_torrent.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_torrent;
            WorldPVP_wanna_urgent_cleanse.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_urgent_cleanse;
            if (UPaHBTSetting.Instance.WorldPVP_aura_type == 0) { WorldPVP_concentrationRB.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_aura_type == 1) { WorldPVP_resistanceRB.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_aura_type == 2) { WorldPVP_DisabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.WorldPVP_bless_type == 0) { WorldPVP_bless_type_autoRB.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_bless_type == 1) { WorldPVP_bless_type_KingRB.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_bless_type == 2) { WorldPVP_bless_type_MightRB.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_bless_type == 4) { WorldPVP_bless_type_disabledRB.Checked = true; }
            if (UPaHBTSetting.Instance.WorldPVP_intellywait == true) { WorldPVP_intellywait.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_decice_during_GCD == false) { WorldPVP_accurancy.Checked = true; }
            else if (UPaHBTSetting.Instance.WorldPVP_decice_during_GCD == true) { WorldPVP_speed.Checked = true; }
            WorldPVP_do_not_heal_above.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_do_not_heal_above);
            WorldPVP_advanced_option.Checked = UPaHBTSetting.Instance.WorldPVP_advanced_option;
            WorldPVP_HR_how_far.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_HR_how_far);
            WorldPVP_HR_how_much_health.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_HR_how_much_health);
            WorldPVP_mana_judge.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_mana_judge);
            WorldPVP_max_healing_distance.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_max_healing_distance);
            WorldPVP_min_Divine_Plea_mana.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_Divine_Plea_mana);
            WorldPVP_min_DL_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_DL_hp);
            WorldPVP_min_DP_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_DP_hp);
            WorldPVP_min_DS_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_DS_hp);
            WorldPVP_min_FoL_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_FoL_hp);
            WorldPVP_min_gift_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_gift_hp);
            WorldPVP_min_HL_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_HL_hp);
            WorldPVP_min_HoP_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_HoP_hp);
            WorldPVP_min_HoS_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_HoS_hp);
            WorldPVP_min_Inf_of_light_DL_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_Inf_of_light_DL_hp);
            WorldPVP_min_LoH_hp.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_LoH_hp);
            WorldPVP_min_mana_potion.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_mana_potion);
            WorldPVP_min_mana_rec_trinket.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_mana_rec_trinket);
            WorldPVP_min_ohshitbutton_activator.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_ohshitbutton_activator);
            WorldPVP_min_player_inside_HR.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_player_inside_HR);
            WorldPVP_min_stoneform.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_stoneform);
            WorldPVP_min_torrent_mana_perc.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_min_torrent_mana_perc);
            WorldPVP_rest_if_mana_below.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_rest_if_mana_below);
            WorldPVP_use_mana_rec_trinket_every.Value = new decimal(UPaHBTSetting.Instance.WorldPVP_use_mana_rec_trinket_every);
            WorldPVP_wanna_crusader.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_crusader;
            WorldPVP_wanna_lifeblood.Checked = UPaHBTSetting.Instance.WorldPVP_wanna_lifeblood;
            WorldPVP_do_not_dismount_ooc.Checked = UPaHBTSetting.Instance.WorldPVP_do_not_dismount_ooc;
            WorldPVP_do_not_dismount_EVER.Checked = UPaHBTSetting.Instance.WorldPVP_do_not_dismount_EVER;
            WorldPVP_get_tank_from_lua.Checked = UPaHBTSetting.Instance.WorldPVP_get_tank_from_lua;
            WorldPVP_cleanse_only_self_and_tank.Checked = UPaHBTSetting.Instance.WorldPVP_cleanse_only_self_and_tank;
            WorldPVP_cleanse_only_self_and_tank.Enabled = UPaHBTSetting.Instance.WorldPVP_wanna_cleanse;

        }

        private void save_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                Logging.Write("Saving Setting");
                UPaHBTSetting.Instance.Save();
                Logging.Write("Saved Settings, rebuilding Behaviors");
                UltimatePalaHealerBT.Instance.CreateBehaviors();
                Logging.Write("Behaviors rebuilt");
            }
            catch (Exception ex)
            {
                Logging.Write(Color.Red, "Exception thrown (Paladin Config): {0}", ex);
            }
        }

        private void SelectHeal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Selective_Healing = SelectHeal.Checked;
            Disappear_select_healing();
        }

        private void RaidCK0_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member0 = RaidCK0.Checked;
            
        }

        private void RaidCK1_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member1 = RaidCK1.Checked;
        }

        private void Refresh_Click(object sender, System.EventArgs e)
        {
            Logging.Write("Refreshing Selective Healing View");
            UltimatePalaHealerBT.Instance.Inizialize_variable_for_GUI();
            UltimatePalaHealerBT.Instance.BuildSubGroupArray();
            /*UltimatePalaHealerBT.Instance.populate_nameofRM();
            UltimatePalaHealerBT.Instance.populate_raidsubg();
            UltimatePalaHealerBT.Instance.Inizialize_raid_names();
            UltimatePalaHealerBT.Instance.Inizialize_raid_role();*/
            Populate_components();
        }

        private void RaidCK2_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member2 = RaidCK2.Checked;
        }

        private void RaidCK3_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member3 = RaidCK3.Checked;
        }

        private void RaidCK4_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member4 = RaidCK4.Checked;
        }

        private void RaidCK5_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member5 = RaidCK5.Checked;
        }

        private void RaidCK6_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member6 = RaidCK6.Checked;
        }

        private void RaidCK7_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member7 = RaidCK7.Checked;
        }

        private void RaidCK8_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member8 = RaidCK8.Checked;
        }

        private void RaidCK9_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member9 = RaidCK9.Checked;
        }

        private void RaidCK10_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member10 = RaidCK10.Checked;
        }

        private void RaidCK11_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member11 = RaidCK11.Checked;
        }

        private void RaidCK12_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member12 = RaidCK12.Checked;
        }

        private void RaidCK13_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member13 = RaidCK13.Checked;
        }

        private void RaidCK14_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member14 = RaidCK14.Checked;
        }

        private void RaidCK15_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member15 = RaidCK15.Checked;
        }

        private void RaidCK16_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member16 = RaidCK16.Checked;
        }

        private void RaidCK17_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member17 = RaidCK17.Checked;
        }

        private void RaidCK18_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member18 = RaidCK18.Checked;
        }

        private void RaidCK19_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member19 = RaidCK19.Checked;
        }

        private void RaidCK20_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member20 = RaidCK20.Checked;
        }

        private void RaidCK21_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member21 = RaidCK21.Checked;
        }

        private void RaidCK22_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member22 = RaidCK22.Checked;
        }

        private void RaidCK23_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member23 = RaidCK23.Checked;
        }

        private void RaidCK24_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member24 = RaidCK24.Checked;
        }

        private void RAFtankfromfocus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.RAF_get_tank_from_focus = RAFtankfromfocus.Checked;
        }

        private void ARENA2v2tankfromfocus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA2v2_get_tank_from_focus = ARENA2v2tankfromfocus.Checked;
        }

        private void Raid_ignore_beacon_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_ignore_beacon = Raid_ignore_beacon.Checked;
        }

        private void Solo_Inf_of_light_wanna_DL_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_Inf_of_light_wanna_DL = Solo_Inf_of_light_wanna_DL.Checked;
            Solo_min_Inf_of_light_DL_hp.Visible = Solo_Inf_of_light_wanna_DL.Checked;
            Solo_min_Inf_of_light_DL_hpLB.Visible = Solo_Inf_of_light_wanna_DL.Checked;
        }

        private void Solo_wanna_AW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_AW = Solo_wanna_AW.Checked;
        }

        private void Solo_wanna_buff_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_buff = Solo_wanna_buff.Checked;
        }

        private void Solo_wanna_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_cleanse = Solo_wanna_cleanse.Checked;
        }

        private void Solo_wanna_CS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_CS = Solo_wanna_CS.Checked;
        }

        private void Solo_wanna_DF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_DF = Solo_wanna_DF.Checked;
        }

        private void Solo_wanna_DP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_DP = Solo_wanna_DP.Checked;
        }

        private void Solo_wanna_DS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_DS = Solo_wanna_DS.Checked;
        }

        private void Solo_wanna_everymanforhimself_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_everymanforhimself = Solo_wanna_everymanforhimself.Checked;
        }

        private void Solo_wanna_face_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_face = Solo_wanna_face.Checked;
        }

        private void Solo_wanna_GotAK_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_GotAK = Solo_wanna_GotAK.Checked;
        }

        private void Solo_wanna_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_HoJ = Solo_wanna_HoJ.Checked;
        }

        private void Solo_wanna_HoP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_HoP = Solo_wanna_HoP.Checked;
        }

        private void Solo_wanna_HoS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_HoS = Solo_wanna_HoS.Checked;
        }

        private void Solo_wanna_HoW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_HoW = Solo_wanna_HoW.Checked;
        }

        private void Solo_wanna_HR_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_HR = Solo_wanna_HR.Checked;
        }

        private void Solo_wanna_Judge_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_Judge = Solo_wanna_Judge.Checked;
        }

        private void Solo_wanna_LoH_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_LoH = Solo_wanna_LoH.Checked;
        }

        private void Solo_wanna_mana_potion_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_mana_potion = Solo_wanna_mana_potion.Checked;
        }

        private void Solo_wanna_mount_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_mount = Solo_wanna_mount.Checked;
        }

        private void Solo_wanna_move_to_heal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_move_to_heal = Solo_wanna_move_to_heal.Checked;
        }

        private void Solo_wanna_move_to_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_move_to_HoJ = Solo_wanna_move_to_HoJ.Checked;
        }

        private void Solo_wanna_rebuke_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_rebuke = Solo_wanna_rebuke.Checked;
        }

        private void Solo_wanna_stoneform_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_stoneform = Solo_wanna_stoneform.Checked;
        }

        private void Solo_wanna_target_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_target = Solo_wanna_target.Checked;
        }

        private void Solo_wanna_torrent_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_torrent = Solo_wanna_torrent.Checked;
        }

        private void Solo_wanna_urgent_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_urgent_cleanse = Solo_wanna_urgent_cleanse.Checked;
        }

        private void Solo_concentrationRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_concentrationRB.Checked) { UPaHBTSetting.Instance.Solo_aura_type = 0; }
        }

        private void Solo_auraselctGB_Enter(object sender, System.EventArgs e)
        {
            if (Solo_concentrationRB.Checked) { UPaHBTSetting.Instance.Solo_aura_type = 0; }
            else if (Solo_resistanceRB.Checked) { UPaHBTSetting.Instance.Solo_aura_type = 1; }
            else if (Solo_DisabledRB.Checked) { UPaHBTSetting.Instance.Solo_aura_type = 2; }
        }

        private void Solo_resistanceRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_resistanceRB.Checked) { UPaHBTSetting.Instance.Solo_aura_type = 1; }
        }

        private void Solo_DisabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_DisabledRB.Checked) { UPaHBTSetting.Instance.Solo_aura_type = 2; }
        }

        private void Solo_do_not_heal_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_do_not_heal_above = int.Parse(Solo_do_not_heal_above.Value.ToString());
        }

        private void Solo_advanced_option_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_advanced_option = Solo_advanced_option.Checked;
            Change_advanced_option_visibility();
        }

        private void Solo_HR_how_far_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_HR_how_far = int.Parse(Solo_HR_how_far.Value.ToString());
        }

        private void Solo_HR_how_much_health_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_HR_how_much_health = int.Parse(Solo_HR_how_much_health.Value.ToString());
        }

        private void Solo_mana_judge_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_mana_judge = int.Parse(Solo_mana_judge.Value.ToString());
        }

        private void Solo_max_healing_distance_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_max_healing_distance = int.Parse(Solo_max_healing_distance.Value.ToString());
        }

        private void Solo_min_Divine_Plea_mana_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_Divine_Plea_mana = int.Parse(Solo_min_Divine_Plea_mana.Value.ToString());
        }

        private void Solo_min_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_DL_hp = int.Parse(Solo_min_DL_hp.Value.ToString());
        }

        private void Solo_min_DP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_DP_hp = int.Parse(Solo_min_DP_hp.Value.ToString());
        }

        private void Solo_min_DS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_DS_hp = int.Parse(Solo_min_DS_hp.Value.ToString());
        }

        private void Solo_min_FoL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_FoL_hp = int.Parse(Solo_min_FoL_hp.Value.ToString());
        }

        private void Solo_min_gift_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_gift_hp = int.Parse(Solo_min_gift_hp.Value.ToString());
        }

        private void Solo_min_HL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_HL_hp = int.Parse(Solo_min_HL_hp.Value.ToString());
        }

        private void Solo_min_HoP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_HoP_hp = int.Parse(Solo_min_HoP_hp.Value.ToString());
        }

        private void Solo_min_HoS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_HoS_hp = int.Parse(Solo_min_HoS_hp.Value.ToString());
        }

        private void Solo_min_Inf_of_light_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_Inf_of_light_DL_hp = int.Parse(Solo_min_Inf_of_light_DL_hp.Value.ToString());
        }

        private void Solo_min_LoH_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_LoH_hp = int.Parse(Solo_min_LoH_hp.Value.ToString());
        }

        private void Solo_min_mana_potion_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_mana_potion = int.Parse(Solo_min_mana_potion.Value.ToString());
        }

        private void Solo_min_mana_rec_trinket_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_mana_rec_trinket = int.Parse(Solo_min_mana_rec_trinket.Value.ToString());
        }

        private void Solo_min_ohshitbutton_activator_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_ohshitbutton_activator = int.Parse(Solo_min_ohshitbutton_activator.Value.ToString());
        }

        private void Solo_min_player_inside_HR_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_player_inside_HR = int.Parse(Solo_min_player_inside_HR.Value.ToString());
        }

        private void Solo_min_stoneform_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_stoneform = int.Parse(Solo_min_stoneform.Value.ToString());
        }

        private void Solo_min_torrent_mana_perc_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_min_torrent_mana_perc = int.Parse(Solo_min_torrent_mana_perc.Value.ToString());
        }

        private void Solo_rest_if_mana_below_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_rest_if_mana_below = int.Parse(Solo_rest_if_mana_below.Value.ToString());
        }

        private void Solo_use_mana_rec_trinket_every_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_use_mana_rec_trinket_every = int.Parse(Solo_use_mana_rec_trinket_every.Value.ToString());
        }

        private void Solo_wanna_move_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_move = Solo_wanna_move.Checked;
        }

        private void Solo_wanna_lifeblood_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_lifeblood = Solo_wanna_lifeblood.Checked;
        }

        private void Solo_do_not_dismount_ooc_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_do_not_dismount_ooc = Solo_do_not_dismount_ooc.Checked;
        }

        private void Solo_do_not_dismount_EVER_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_do_not_dismount_EVER = Solo_do_not_dismount_EVER.Checked;
        }

        private void Solo_get_tank_from_lua_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_get_tank_from_lua = Solo_get_tank_from_lua.Checked;
        }

        private void Solo_wanna_gift_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_gift = Solo_wanna_gift.Checked;
        }

        private void RaidCK25_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member25 = RaidCK25.Checked;
        }

        private void RaidCK26_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member26 = RaidCK26.Checked;
        }

        private void RaidCK27_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member27 = RaidCK27.Checked;
        }

        private void RaidCK28_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member28 = RaidCK28.Checked;
        }

        private void RaidCK29_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member29 = RaidCK29.Checked;
        }

        private void RaidCK30_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member30 = RaidCK30.Checked;
        }

        private void RaidCK31_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member31 = RaidCK31.Checked;
        }

        private void RaidCK32_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member32 = RaidCK32.Checked;
        }

        private void RaidCK33_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member33 = RaidCK33.Checked;
        }

        private void RaidCK34_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member34 = RaidCK34.Checked;
        }

        private void RaidCK35_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member35 = RaidCK35.Checked;
        }

        private void RaidCK36_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member36 = RaidCK36.Checked;
        }

        private void RaidCK37_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member37 = RaidCK37.Checked;
        }

        private void RaidCK38_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member38 = RaidCK38.Checked;
        }

        private void RaidCK39_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Heal_raid_member39 = RaidCK39.Checked;
        }

        private void PVE_Inf_of_light_wanna_DL_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_Inf_of_light_wanna_DL = PVE_Inf_of_light_wanna_DL.Checked;
            PVE_min_Inf_of_light_DL_hp.Visible = PVE_Inf_of_light_wanna_DL.Checked;
            PVE_min_Inf_of_light_DL_hpLB.Visible = PVE_Inf_of_light_wanna_DL.Checked;
        }

        private void PVE_get_tank_from_focus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_get_tank_from_focus = PVE_get_tank_from_focus.Checked;
        }

        private void PVE_wanna_AW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_AW = PVE_wanna_AW.Checked;
        }

        private void PVE_wanna_buff_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_buff = PVE_wanna_buff.Checked;
        }

        private void PVE_wanna_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_cleanse = PVE_wanna_cleanse.Checked;
            PVE_cleanse_only_self_and_tank.Enabled = PVE_wanna_cleanse.Checked;
        }

        private void PVE_wanna_crusader_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_crusader = PVE_wanna_crusader.Checked;
        }

        private void PVE_wanna_CS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_CS = PVE_wanna_CS.Checked;
        }

        private void PVE_wanna_DF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_DF = PVE_wanna_DF.Checked;
        }

        private void PVE_wanna_DP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_DP = PVE_wanna_DP.Checked;
        }

        private void PVE_wanna_DS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_DS = PVE_wanna_DS.Checked;
        }

        private void PVE_wanna_everymanforhimself_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_everymanforhimself = PVE_wanna_everymanforhimself.Checked;
        }

        private void PVE_wanna_face_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_face = PVE_wanna_face.Checked;
        }

        private void PVE_wanna_gift_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_gift = PVE_wanna_gift.Checked;
        }

        private void PVE_wanna_GotAK_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_GotAK = PVE_wanna_GotAK.Checked;
        }

        private void PVE_wanna_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_HoJ = PVE_wanna_HoJ.Checked;
        }

        private void PVE_wanna_HoP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_HoP = PVE_wanna_HoP.Checked;
        }

        private void PVE_wanna_HoS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_HoS = PVE_wanna_HoS.Checked;
        }

        private void PVE_wanna_HoW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_HoW = PVE_wanna_HoW.Checked;
        }

        private void PVE_wanna_HR_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_HR = PVE_wanna_HR.Checked;
        }

        private void PVE_wanna_Judge_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_Judge = PVE_wanna_Judge.Checked;
        }

        private void PVE_wanna_LoH_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_LoH = PVE_wanna_LoH.Checked;
        }

        private void PVE_wanna_mana_potion_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_mana_potion = PVE_wanna_mana_potion.Checked;
        }

        private void PVE_wanna_mount_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_mount = PVE_wanna_mount.Checked;
        }

        private void PVE_wanna_move_to_heal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_move_to_heal = PVE_wanna_move_to_heal.Checked;
        }

        private void PVE_wanna_move_to_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_move_to_HoJ = PVE_wanna_move_to_HoJ.Checked;
        }

        private void PVE_wanna_rebuke_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_rebuke = PVE_wanna_rebuke.Checked;
        }

        private void PVE_wanna_stoneform_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_stoneform = PVE_wanna_stoneform.Checked;
        }

        private void PVE_wanna_target_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_target = PVE_wanna_target.Checked;
        }

        private void PVE_wanna_torrent_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_torrent = PVE_wanna_torrent.Checked;
        }

        private void PVE_wanna_urgent_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_urgent_cleanse = PVE_wanna_urgent_cleanse.Checked;
        }

        private void PVE_auraselctGB_Enter(object sender, System.EventArgs e)
        {

            if (PVE_concentrationRB.Checked) { UPaHBTSetting.Instance.PVE_aura_type = 0; }
            else if (PVE_resistanceRB.Checked) { UPaHBTSetting.Instance.PVE_aura_type = 1; }
            else if (PVE_DisabledRB.Checked) { UPaHBTSetting.Instance.PVE_aura_type = 2; }
        }

        private void PVE_concentrationRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_concentrationRB.Checked) { UPaHBTSetting.Instance.PVE_aura_type = 0; }
        }

        private void PVE_resistanceRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_resistanceRB.Checked) { UPaHBTSetting.Instance.PVE_aura_type = 1; }
        }

        private void PVE_DisabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_DisabledRB.Checked) { UPaHBTSetting.Instance.PVE_aura_type = 2; }
        }

        private void PVE_do_not_heal_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_heal_above = int.Parse(PVE_do_not_heal_above.Value.ToString());
        }

        private void PVE_HR_how_far_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_HR_how_far = int.Parse(PVE_HR_how_far.Value.ToString());
        }

        private void PVE_HR_how_much_health_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_HR_how_much_health = int.Parse(PVE_HR_how_much_health.Value.ToString());
        }

        private void PVE_mana_judge_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_mana_judge = int.Parse(PVE_mana_judge.Value.ToString());
        }

        private void PVE_max_healing_distance_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_max_healing_distance = int.Parse(PVE_max_healing_distance.Value.ToString());
        }

        private void PVE_min_Divine_Plea_mana_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_Divine_Plea_mana = int.Parse(PVE_min_Divine_Plea_mana.Value.ToString());
        }

        private void PVE_min_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_DL_hp = int.Parse(PVE_min_DL_hp.Value.ToString());
        }

        private void PVE_min_DP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_DP_hp = int.Parse(PVE_min_DP_hp.Value.ToString());
        }

        private void PVE_min_DS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_DS_hp = int.Parse(PVE_min_DS_hp.Value.ToString());
        }

        private void PVE_min_FoL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_FoL_hp = int.Parse(PVE_min_FoL_hp.Value.ToString());
        }

        private void PVE_min_gift_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_gift_hp = int.Parse(PVE_min_gift_hp.Value.ToString());
        }

        private void PVE_min_HL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_HL_hp = int.Parse(PVE_min_HL_hp.Value.ToString());
        }

        private void PVE_min_HoP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_HoP_hp = int.Parse(PVE_min_HoP_hp.Value.ToString());
        }

        private void PVE_min_HoS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_HoS_hp = int.Parse(PVE_min_HoS_hp.Value.ToString());
        }

        private void PVE_min_Inf_of_light_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_Inf_of_light_DL_hp = int.Parse(PVE_min_Inf_of_light_DL_hp.Value.ToString());
        }

        private void PVE_min_LoH_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_LoH_hp = int.Parse(PVE_min_LoH_hp.Value.ToString());
        }

        private void PVE_min_mana_potion_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_mana_potion = int.Parse(PVE_min_mana_potion.Value.ToString());
        }

        private void PVE_min_mana_rec_trinket_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_mana_rec_trinket = int.Parse(PVE_min_mana_rec_trinket.Value.ToString());
        }

        private void PVE_min_ohshitbutton_activator_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_ohshitbutton_activator = int.Parse(PVE_min_ohshitbutton_activator.Value.ToString());
        }

        private void PVE_min_player_inside_HR_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_player_inside_HR = int.Parse(PVE_min_player_inside_HR.Value.ToString());
        }

        private void PVE_min_stoneform_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_stoneform = int.Parse(PVE_min_stoneform.Value.ToString());
        }

        private void PVE_min_torrent_mana_perc_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_min_torrent_mana_perc = int.Parse(PVE_min_torrent_mana_perc.Value.ToString());
        }

        private void PVE_rest_if_mana_below_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_rest_if_mana_below = int.Parse(PVE_rest_if_mana_below.Value.ToString());
        }

        private void PVE_use_mana_rec_trinket_every_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_use_mana_rec_trinket_every = int.Parse(PVE_use_mana_rec_trinket_every.Value.ToString());
        }

        private void PVE_wanna_lifeblood_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_wanna_lifeblood = PVE_wanna_lifeblood.Checked;
        }

        private void PVE_do_not_dismount_ooc_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_dismount_ooc = PVE_do_not_dismount_ooc.Checked;
        }

        private void PVE_do_not_dismount_EVER_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_dismount_EVER = PVE_do_not_dismount_EVER.Checked;
        }

        private void PVE_advanced_option_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_advanced_option = PVE_advanced_option.Checked;
            Change_advanced_option_visibility();
        }

        private void PVE_get_tank_from_lua_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_get_tank_from_lua = PVE_get_tank_from_lua.Checked;
        }

        private void PVE_stop_DL_if_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_stop_DL_if_above = int.Parse(PVE_stop_DL_if_above.Value.ToString());
        }

        private void Solo_get_tank_from_focus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_get_tank_from_focus = Solo_get_tank_from_focus.Checked;
        }

        private void Solo_wanna_crusader_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_wanna_crusader = Solo_wanna_crusader.Checked;
        }

        private void ARENA_get_tank_from_focus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_get_tank_from_focus = ARENA_get_tank_from_focus.Checked;
        }

        private void ARENA_Inf_of_light_wanna_DL_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_Inf_of_light_wanna_DL = ARENA_Inf_of_light_wanna_DL.Checked;
            ARENA_min_Inf_of_light_DL_hp.Visible = ARENA_Inf_of_light_wanna_DL.Checked;
            ARENA_min_Inf_of_light_DL_hpLB.Visible = ARENA_Inf_of_light_wanna_DL.Checked;
        }

        private void ARENA_wanna_AW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_AW = ARENA_wanna_AW.Checked;
        }

        private void ARENA_wanna_buff_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_buff = ARENA_wanna_buff.Checked;
        }

        private void ARENA_wanna_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_cleanse = ARENA_wanna_cleanse.Checked;
            ARENA_cleanse_only_self_and_tank.Enabled = ARENA_wanna_cleanse.Checked;
        }

        private void ARENA_wanna_crusader_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_crusader = ARENA_wanna_crusader.Checked;
        }

        private void ARENA_wanna_CS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_CS = ARENA_wanna_CS.Checked;
        }

        private void ARENA_wanna_DF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_DF = ARENA_wanna_DF.Checked;
        }

        private void ARENA_wanna_DP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_DP = ARENA_wanna_DP.Checked;
        }

        private void ARENA_wanna_DS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_DS = ARENA_wanna_DS.Checked;
        }

        private void ARENA_wanna_everymanforhimself_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_everymanforhimself = ARENA_wanna_everymanforhimself.Checked;
        }

        private void ARENA_wanna_face_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_face = ARENA_wanna_face.Checked;
        }

        private void ARENA_wanna_gift_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_gift = ARENA_wanna_gift.Checked;
        }

        private void ARENA_wanna_GotAK_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_GotAK = ARENA_wanna_GotAK.Checked;
        }

        private void ARENA_wanna_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_HoJ = ARENA_wanna_HoJ.Checked;
        }

        private void ARENA_wanna_HoP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_HoP = ARENA_wanna_HoP.Checked;
        }

        private void ARENA_wanna_HoS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_HoS = ARENA_wanna_HoS.Checked;
        }

        private void ARENA_wanna_HoW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_HoW = ARENA_wanna_HoW.Checked;
        }

        private void ARENA_wanna_HR_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_HR = ARENA_wanna_HR.Checked;
        }

        private void ARENA_wanna_Judge_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_Judge = ARENA_wanna_Judge.Checked;
        }

        private void ARENA_wanna_LoH_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_LoH = ARENA_wanna_LoH.Checked;
        }

        private void ARENA_wanna_mana_potion_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_mana_potion = ARENA_wanna_mana_potion.Checked;
        }

        private void ARENA_wanna_mount_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_mount = ARENA_wanna_mount.Checked;
        }

        private void ARENA_wanna_move_to_heal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_move_to_heal = ARENA_wanna_move_to_heal.Checked;
        }

        private void ARENA_wanna_move_to_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_move_to_HoJ = ARENA_wanna_move_to_HoJ.Checked;
        }

        private void ARENA_wanna_rebuke_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_rebuke = ARENA_wanna_rebuke.Checked;
        }

        private void ARENA_wanna_stoneform_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_stoneform = ARENA_wanna_stoneform.Checked;
        }

        private void ARENA_wanna_target_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_target = ARENA_wanna_target.Checked;
        }

        private void ARENA_wanna_torrent_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_torrent = ARENA_wanna_torrent.Checked;
        }

        private void ARENA_wanna_urgent_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_urgent_cleanse = ARENA_wanna_urgent_cleanse.Checked;
        }

        private void ARENA_auraselctGB_Enter(object sender, System.EventArgs e)
        {
            if (ARENA_concentrationRB.Checked) { UPaHBTSetting.Instance.ARENA_aura_type = 0; }
            else if (ARENA_resistanceRB.Checked) { UPaHBTSetting.Instance.ARENA_aura_type = 1; }
            else if (ARENA_DisabledRB.Checked) { UPaHBTSetting.Instance.ARENA_aura_type = 2; }
        }

        private void ARENA_concentrationRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_concentrationRB.Checked) { UPaHBTSetting.Instance.ARENA_aura_type = 0; }
        }

        private void ARENA_resistanceRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_resistanceRB.Checked) { UPaHBTSetting.Instance.ARENA_aura_type = 1; }
        }

        private void ARENA_DisabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_DisabledRB.Checked) { UPaHBTSetting.Instance.ARENA_aura_type = 2; }
        }

        private void ARENA_do_not_heal_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_do_not_heal_above = int.Parse(ARENA_do_not_heal_above.Value.ToString());
        }

        private void ARENA_HR_how_far_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_HR_how_far = int.Parse(ARENA_HR_how_far.Value.ToString());
        }

        private void ARENA_HR_how_much_health_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_HR_how_much_health = int.Parse(ARENA_HR_how_much_health.Value.ToString());
        }

        private void ARENA_mana_judge_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_mana_judge = int.Parse(ARENA_mana_judge.Value.ToString());
        }

        private void ARENA_max_healing_distance_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_max_healing_distance = int.Parse(ARENA_max_healing_distance.Value.ToString());
        }

        private void ARENA_min_Divine_Plea_mana_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_Divine_Plea_mana = int.Parse(ARENA_min_Divine_Plea_mana.Value.ToString());
        }

        private void ARENA_min_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_DL_hp = int.Parse(ARENA_min_DL_hp.Value.ToString());
        }

        private void ARENA_min_DP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_DP_hp = int.Parse(ARENA_min_DP_hp.Value.ToString());
        }

        private void ARENA_min_DS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_DS_hp = int.Parse(ARENA_min_DS_hp.Value.ToString());
        }

        private void ARENA_min_FoL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_FoL_hp = int.Parse(ARENA_min_FoL_hp.Value.ToString());
        }

        private void ARENA_min_gift_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_gift_hp = int.Parse(ARENA_min_gift_hp.Value.ToString());
        }

        private void ARENA_min_HL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_HL_hp = int.Parse(ARENA_min_HL_hp.Value.ToString());
        }

        private void ARENA_min_HoP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_HoP_hp = int.Parse(ARENA_min_HoP_hp.Value.ToString());
        }

        private void ARENA_min_HoS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_HoS_hp = int.Parse(ARENA_min_HoS_hp.Value.ToString());
        }

        private void ARENA_min_Inf_of_light_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_Inf_of_light_DL_hp = int.Parse(ARENA_min_Inf_of_light_DL_hp.Value.ToString());
        }

        private void ARENA_min_LoH_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_LoH_hp = int.Parse(ARENA_min_LoH_hp.Value.ToString());
        }

        private void ARENA_min_mana_potion_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_mana_potion = int.Parse(ARENA_min_mana_potion.Value.ToString());
        }

        private void ARENA_min_mana_rec_trinket_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_mana_rec_trinket = int.Parse(ARENA_min_mana_rec_trinket.Value.ToString());
        }

        private void ARENA_min_ohshitbutton_activator_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_ohshitbutton_activator = int.Parse(ARENA_min_ohshitbutton_activator.Value.ToString());
        }

        private void ARENA_min_player_inside_HR_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_player_inside_HR = int.Parse(ARENA_min_player_inside_HR.Value.ToString());
        }

        private void ARENA_min_stoneform_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_stoneform = int.Parse(ARENA_min_stoneform.Value.ToString());
        }

        private void ARENA_min_torrent_mana_perc_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_min_torrent_mana_perc = int.Parse(ARENA_min_torrent_mana_perc.Value.ToString());
        }

        private void ARENA_rest_if_mana_below_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_rest_if_mana_below = int.Parse(ARENA_rest_if_mana_below.Value.ToString());
        }

        private void ARENA_use_mana_rec_trinket_every_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_use_mana_rec_trinket_every = int.Parse(ARENA_use_mana_rec_trinket_every.Value.ToString());
        }

        private void ARENA_wanna_taunt_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_taunt = ARENA_wanna_taunt.Checked;
        }

        private void ARENA_wanna_HoF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_HoF = ARENA_wanna_HoF.Checked;
        }

        private void ARENA_wanna_lifeblood_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_wanna_lifeblood = ARENA_wanna_lifeblood.Checked;
        }

        private void ARENA_do_not_dismount_ooc_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_do_not_dismount_ooc = ARENA_do_not_dismount_ooc.Checked;
        }

        private void ARENA_do_not_dismount_EVER_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_do_not_dismount_EVER = ARENA_do_not_dismount_EVER.Checked;
        }

        private void ARENA_advanced_option_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_advanced_option = ARENA_advanced_option.Checked;
            Change_advanced_option_visibility();
        }

        private void ARENA_get_tank_from_lua_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_get_tank_from_lua = ARENA_get_tank_from_lua.Checked;
        }

        private void Raid_get_tank_from_focus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_get_tank_from_focus = Raid_get_tank_from_focus.Checked;
        }

        private void Raid_Inf_of_light_wanna_DL_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_Inf_of_light_wanna_DL = Raid_Inf_of_light_wanna_DL.Checked;
            Raid_min_Inf_of_light_DL_hp.Visible = Raid_Inf_of_light_wanna_DL.Checked;
            Raid_min_Inf_of_light_DL_hpLB.Visible = Raid_Inf_of_light_wanna_DL.Checked;
        }

        private void Raid_wanna_AW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_AW = Raid_wanna_AW.Checked;
        }

        private void Raid_wanna_buff_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_buff = Raid_wanna_buff.Checked;
        }

        private void Raid_wanna_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_cleanse = Raid_wanna_cleanse.Checked;
            Raid_cleanse_only_self_and_tank.Enabled = Raid_wanna_cleanse.Checked;
        }

        private void Raid_wanna_crusader_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_crusader = Raid_wanna_crusader.Checked;
        }

        private void Raid_wanna_CS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_CS = Raid_wanna_CS.Checked;
        }

        private void Raid_wanna_DF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_DF = Raid_wanna_DF.Checked;
        }

        private void Raid_wanna_DP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_DP = Raid_wanna_DP.Checked;
        }

        private void Raid_wanna_DS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_DS = Raid_wanna_DS.Checked;
        }

        private void Raid_wanna_everymanforhimself_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_everymanforhimself = Raid_wanna_everymanforhimself.Checked;
        }

        private void Raid_wanna_face_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_face = Raid_wanna_face.Checked;
        }

        private void Raid_wanna_gift_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_gift = Raid_wanna_gift.Checked;
        }

        private void Raid_wanna_GotAK_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_GotAK = Raid_wanna_GotAK.Checked;
        }

        private void Raid_wanna_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_HoJ = Raid_wanna_HoJ.Checked;
        }

        private void Raid_wanna_HoP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_HoP = Raid_wanna_HoP.Checked;
        }

        private void Raid_wanna_HoS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_HoS = Raid_wanna_HoS.Checked;
        }

        private void Raid_wanna_HoW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_HoW = Raid_wanna_HoW.Checked;
        }

        private void Raid_wanna_HR_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_HR = Raid_wanna_HR.Checked;
        }

        private void Raid_wanna_Judge_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_Judge = Raid_wanna_Judge.Checked;
        }

        private void Raid_wanna_LoH_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_LoH = Raid_wanna_LoH.Checked;
        }

        private void Raid_wanna_mana_potion_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_mana_potion = Raid_wanna_mana_potion.Checked;
        }

        private void Raid_wanna_mount_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_mount = Raid_wanna_mount.Checked;
        }

        private void Raid_wanna_move_to_heal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_move_to_heal = Raid_wanna_move_to_heal.Checked;
        }

        private void Raid_wanna_move_to_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_move_to_HoJ = Raid_wanna_move_to_HoJ.Checked;
        }

        private void Raid_wanna_rebuke_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_rebuke = Raid_wanna_rebuke.Checked;
        }

        private void Raid_wanna_stoneform_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_stoneform = Raid_wanna_stoneform.Checked;
        }

        private void Raid_wanna_target_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_target = Raid_wanna_target.Checked;
        }

        private void Raid_wanna_torrent_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_torrent = Raid_wanna_torrent.Checked;
        }

        private void Raid_wanna_urgent_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_urgent_cleanse = Raid_wanna_urgent_cleanse.Checked;
        }

        private void Raid_auraselctGB_Enter(object sender, System.EventArgs e)
        {
            if (Raid_concentrationRB.Checked) { UPaHBTSetting.Instance.Raid_aura_type = 0; }
            else if (Raid_resistanceRB.Checked) { UPaHBTSetting.Instance.Raid_aura_type = 1; }
            else if (Raid_DisabledRB.Checked) { UPaHBTSetting.Instance.Raid_aura_type = 2; }
        }

        private void Raid_concentrationRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_concentrationRB.Checked) { UPaHBTSetting.Instance.Raid_aura_type = 0; }
        }

        private void Raid_resistanceRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_resistanceRB.Checked) { UPaHBTSetting.Instance.Raid_aura_type = 1; }
        }

        private void Raid_DisabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_DisabledRB.Checked) { UPaHBTSetting.Instance.Raid_aura_type = 2; }
        }

        private void Raid_do_not_heal_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_do_not_heal_above = int.Parse(Raid_do_not_heal_above.Value.ToString());
        }

        private void Raid_HR_how_far_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_HR_how_far = int.Parse(Raid_HR_how_far.Value.ToString());
        }

        private void Raid_HR_how_much_health_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_HR_how_much_health = int.Parse(Raid_HR_how_much_health.Value.ToString());
        }

        private void Raid_mana_judge_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_mana_judge = int.Parse(Raid_mana_judge.Value.ToString());
        }

        private void Raid_max_healing_distance_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_max_healing_distance = int.Parse(Raid_max_healing_distance.Value.ToString());
        }

        private void Raid_min_Divine_Plea_mana_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_Divine_Plea_mana = int.Parse(Raid_min_Divine_Plea_mana.Value.ToString());
        }

        private void Raid_min_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_DL_hp = int.Parse(Raid_min_DL_hp.Value.ToString());
        }

        private void Raid_min_DP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_DP_hp = int.Parse(Raid_min_DP_hp.Value.ToString());
        }

        private void Raid_min_DS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_DS_hp = int.Parse(Raid_min_DS_hp.Value.ToString());
        }

        private void Raid_min_FoL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_FoL_hp = int.Parse(Raid_min_FoL_hp.Value.ToString());
        }

        private void Raid_min_gift_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_gift_hp = int.Parse(Raid_min_gift_hp.Value.ToString());
        }

        private void Raid_min_HL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_HL_hp = int.Parse(Raid_min_HL_hp.Value.ToString());
        }

        private void Raid_min_HoP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_HoP_hp = int.Parse(Raid_min_HoP_hp.Value.ToString());
        }

        private void Raid_min_HoS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_HoS_hp = int.Parse(Raid_min_HoS_hp.Value.ToString());
        }

        private void Raid_min_Inf_of_light_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_Inf_of_light_DL_hp = int.Parse(Raid_min_Inf_of_light_DL_hp.Value.ToString());
        }

        private void Raid_min_LoH_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_LoH_hp = int.Parse(Raid_min_LoH_hp.Value.ToString());
        }

        private void Raid_min_mana_potion_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_mana_potion = int.Parse(Raid_min_mana_potion.Value.ToString());
        }

        private void Raid_min_mana_rec_trinket_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_mana_rec_trinket = int.Parse(Raid_min_mana_rec_trinket.Value.ToString());
        }

        private void Raid_min_ohshitbutton_activator_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_ohshitbutton_activator = int.Parse(Raid_min_ohshitbutton_activator.Value.ToString());
        }

        private void Raid_min_player_inside_HR_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_player_inside_HR = int.Parse(Raid_min_player_inside_HR.Value.ToString());
        }

        private void Raid_min_stoneform_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_stoneform = int.Parse(Raid_min_stoneform.Value.ToString());
        }

        private void Raid_rest_if_mana_below_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_rest_if_mana_below = int.Parse(Raid_rest_if_mana_below.Value.ToString());
        }

        private void Raid_use_mana_rec_trinket_every_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_use_mana_rec_trinket_every = int.Parse(Raid_use_mana_rec_trinket_every.Value.ToString());
        }

        private void Raid_min_torrent_mana_perc_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_min_torrent_mana_perc = int.Parse(Raid_min_torrent_mana_perc.Value.ToString());
        }

        private void Raid_wanna_lifeblood_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_wanna_lifeblood = Raid_wanna_lifeblood.Checked;
        }

        private void Raid_do_not_dismount_ooc_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_do_not_dismount_ooc = Raid_do_not_dismount_ooc.Checked;
        }

        private void Raid_do_not_dismount_EVER_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_do_not_dismount_EVER = Raid_do_not_dismount_EVER.Checked;
        }

        private void Raid_advanced_option_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_advanced_option = Raid_advanced_option.Checked;
            Change_advanced_option_visibility();
        }

        private void Raid_get_tank_from_lua_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_get_tank_from_lua = Raid_get_tank_from_lua.Checked;
        }

        private void Raid_stop_DL_if_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_stop_DL_if_above = int.Parse(Raid_stop_DL_if_above.Value.ToString());
        }

        private void Battleground_get_tank_from_focus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_get_tank_from_focus = Battleground_get_tank_from_focus.Checked;
        }

        private void Battleground_Inf_of_light_wanna_DL_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_Inf_of_light_wanna_DL = Battleground_Inf_of_light_wanna_DL.Checked;
            Battleground_min_Inf_of_light_DL_hp.Visible = Battleground_Inf_of_light_wanna_DL.Checked;
            Battleground_min_Inf_of_light_DL_hpLB.Visible = Battleground_Inf_of_light_wanna_DL.Checked;
        }

        private void Battleground_wanna_AW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_AW = Battleground_wanna_AW.Checked;
        }

        private void Battleground_wanna_buff_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_buff = Battleground_wanna_buff.Checked;
        }

        private void Battleground_wanna_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_cleanse = Battleground_wanna_cleanse.Checked;
            Battleground_cleanse_only_self_and_tank.Enabled = Battleground_wanna_cleanse.Checked;
        }

        private void Battleground_wanna_crusader_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_crusader = Battleground_wanna_crusader.Checked;
        }

        private void Battleground_wanna_CS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_CS = Battleground_wanna_CS.Checked;
        }

        private void Battleground_wanna_DF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_DF = Battleground_wanna_DF.Checked;
        }

        private void Battleground_wanna_DP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_DP = Battleground_wanna_DP.Checked;
        }

        private void Battleground_wanna_DS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_DS = Battleground_wanna_DS.Checked;
        }

        private void Battleground_wanna_everymanforhimself_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_everymanforhimself = Battleground_wanna_everymanforhimself.Checked;
        }

        private void Battleground_wanna_face_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_face = Battleground_wanna_face.Checked;
        }

        private void Battleground_wanna_gift_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_gift = Battleground_wanna_gift.Checked;
        }

        private void Battleground_wanna_GotAK_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_GotAK = Battleground_wanna_GotAK.Checked;
        }

        private void Battleground_wanna_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_HoJ = Battleground_wanna_HoJ.Checked;
        }

        private void Battleground_wanna_HoP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_HoP = Battleground_wanna_HoP.Checked;
        }

        private void Battleground_wanna_HoS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_HoS = Battleground_wanna_HoS.Checked;
        }

        private void Battleground_wanna_HoW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_HoW = Battleground_wanna_HoW.Checked;
        }

        private void Battleground_wanna_HR_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_HR = Battleground_wanna_HR.Checked;
        }

        private void Battleground_wanna_Judge_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_Judge = Battleground_wanna_Judge.Checked;
        }

        private void Battleground_wanna_LoH_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_LoH = Battleground_wanna_LoH.Checked;
        }

        private void Battleground_wanna_mana_potion_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_mana_potion = Battleground_wanna_mana_potion.Checked;
        }

        private void Battleground_wanna_mount_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_mount = Battleground_wanna_mount.Checked;
        }

        private void Battleground_wanna_move_to_heal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_move_to_heal = Battleground_wanna_move_to_heal.Checked;
        }

        private void Battleground_wanna_move_to_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_move_to_HoJ = Battleground_wanna_move_to_HoJ.Checked;
        }

        private void Battleground_wanna_rebuke_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_rebuke = Battleground_wanna_rebuke.Checked;
        }

        private void Battleground_wanna_stoneform_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_stoneform = Battleground_wanna_stoneform.Checked;
        }

        private void Battleground_wanna_target_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_target = Battleground_wanna_target.Checked;
        }

        private void Battleground_wanna_torrent_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_torrent = Battleground_wanna_torrent.Checked;
        }

        private void Battleground_wanna_urgent_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_urgent_cleanse = Battleground_wanna_urgent_cleanse.Checked;
        }

        private void Battleground_auraselctGB_Enter(object sender, System.EventArgs e)
        {
            if (Battleground_concentrationRB.Checked) { UPaHBTSetting.Instance.Battleground_aura_type = 0; }
            else if (Battleground_resistanceRB.Checked) { UPaHBTSetting.Instance.Battleground_aura_type = 1; }
            else if (Battleground_DisabledRB.Checked) { UPaHBTSetting.Instance.Battleground_aura_type = 2; }
        }

        private void Battleground_concentrationRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_concentrationRB.Checked) { UPaHBTSetting.Instance.Battleground_aura_type = 0; }
        }

        private void Battleground_resistanceRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_resistanceRB.Checked) { UPaHBTSetting.Instance.Battleground_aura_type = 1; }
        }

        private void Battleground_DisabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_DisabledRB.Checked) { UPaHBTSetting.Instance.Battleground_aura_type = 2; }
        }

        private void Battleground_do_not_heal_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_do_not_heal_above = int.Parse(Battleground_do_not_heal_above.Value.ToString());
        }

        private void Battleground_HR_how_far_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_HR_how_far = int.Parse(Battleground_HR_how_far.Value.ToString());
        }

        private void Battleground_HR_how_much_health_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_HR_how_much_health = int.Parse(Battleground_HR_how_much_health.Value.ToString());
        }

        private void Battleground_mana_judge_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_mana_judge = int.Parse(Battleground_mana_judge.Value.ToString());
        }

        private void Battleground_max_healing_distance_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_max_healing_distance = int.Parse(Battleground_max_healing_distance.Value.ToString());
        }

        private void Battleground_min_Divine_Plea_mana_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_Divine_Plea_mana = int.Parse(Battleground_min_Divine_Plea_mana.Value.ToString());
        }

        private void Battleground_min_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_DL_hp = int.Parse(Battleground_min_DL_hp.Value.ToString());
        }

        private void Battleground_min_DP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_DP_hp = int.Parse(Battleground_min_DP_hp.Value.ToString());
        }

        private void Battleground_min_DS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_DS_hp = int.Parse(Battleground_min_DS_hp.Value.ToString());
        }

        private void Battleground_min_FoL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_FoL_hp = int.Parse(Battleground_min_FoL_hp.Value.ToString());
        }

        private void Battleground_min_gift_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_gift_hp = int.Parse(Battleground_min_gift_hp.Value.ToString());
        }

        private void Battleground_min_HL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_HL_hp = int.Parse(Battleground_min_HL_hp.Value.ToString());
        }

        private void Battleground_min_HoP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_HoP_hp = int.Parse(Battleground_min_HoP_hp.Value.ToString());
        }

        private void Battleground_min_HoS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_HoS_hp = int.Parse(Battleground_min_HoS_hp.Value.ToString());
        }

        private void Battleground_min_Inf_of_light_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_Inf_of_light_DL_hp = int.Parse(Battleground_min_Inf_of_light_DL_hp.Value.ToString());
        }

        private void Battleground_min_LoH_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_LoH_hp = int.Parse(Battleground_min_LoH_hp.Value.ToString());
        }

        private void Battleground_min_mana_potion_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_mana_potion = int.Parse(Battleground_min_mana_potion.Value.ToString());
        }

        private void Battleground_min_mana_rec_trinket_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_mana_rec_trinket = int.Parse(Battleground_min_mana_rec_trinket.Value.ToString());
        }

        private void Battleground_min_ohshitbutton_activator_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_ohshitbutton_activator = int.Parse(Battleground_min_ohshitbutton_activator.Value.ToString());
        }

        private void Battleground_min_player_inside_HR_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_player_inside_HR = int.Parse(Battleground_min_player_inside_HR.Value.ToString());
        }

        private void Battleground_min_stoneform_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_stoneform = int.Parse(Battleground_min_stoneform.Value.ToString());
        }

        private void Battleground_min_torrent_mana_perc_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_min_torrent_mana_perc = int.Parse(Battleground_min_torrent_mana_perc.Value.ToString());
        }

        private void Battleground_rest_if_mana_below_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_rest_if_mana_below = int.Parse(Battleground_rest_if_mana_below.Value.ToString());
        }

        private void Battleground_use_mana_rec_trinket_every_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_use_mana_rec_trinket_every = int.Parse(Battleground_use_mana_rec_trinket_every.Value.ToString());
        }

        private void Battleground_wanna_lifeblood_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_wanna_lifeblood = Battleground_wanna_lifeblood.Checked;
        }

        private void Battleground_do_not_dismount_ooc_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_do_not_dismount_ooc = Battleground_do_not_dismount_ooc.Checked;
        }

        private void Battleground_do_not_dismount_EVER_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_do_not_dismount_EVER = Battleground_do_not_dismount_EVER.Checked;
        }

        private void Battleground_advanced_option_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_advanced_option = Battleground_advanced_option.Checked;
            Change_advanced_option_visibility();
        }

        private void Battleground_get_tank_from_lua_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_get_tank_from_lua = Battleground_get_tank_from_lua.Checked;
        }

        private void WorldPVP_Inf_of_light_wanna_DL_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_Inf_of_light_wanna_DL = WorldPVP_Inf_of_light_wanna_DL.Checked;
            WorldPVP_min_Inf_of_light_DL_hp.Visible = WorldPVP_Inf_of_light_wanna_DL.Checked;
            WorldPVP_min_Inf_of_light_DL_hpLB.Visible = WorldPVP_Inf_of_light_wanna_DL.Checked;
        }

        private void WorldPVP_get_tank_from_focus_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_get_tank_from_focus = WorldPVP_get_tank_from_focus.Checked;
        }

        private void WorldPVP_wanna_AW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_AW = WorldPVP_wanna_AW.Checked;
        }

        private void WorldPVP_wanna_buff_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_buff = WorldPVP_wanna_buff.Checked;
        }

        private void WorldPVP_wanna_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_cleanse = WorldPVP_wanna_cleanse.Checked;
            WorldPVP_cleanse_only_self_and_tank.Enabled = WorldPVP_wanna_cleanse.Checked;
        }

        private void WorldPVP_wanna_crusader_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_crusader = WorldPVP_wanna_crusader.Checked;
        }

        private void WorldPVP_wanna_CS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_CS = WorldPVP_wanna_CS.Checked;
        }

        private void WorldPVP_wanna_DF_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_DF = WorldPVP_wanna_DF.Checked;
        }

        private void WorldPVP_wanna_DP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_DP = WorldPVP_wanna_DP.Checked;
        }

        private void WorldPVP_wanna_DS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_DS = WorldPVP_wanna_DS.Checked;
        }

        private void WorldPVP_wanna_everymanforhimself_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_everymanforhimself = WorldPVP_wanna_everymanforhimself.Checked;
        }

        private void WorldPVP_wanna_face_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_face = WorldPVP_wanna_face.Checked;
        }

        private void WorldPVP_wanna_gift_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_gift = WorldPVP_wanna_gift.Checked;
        }

        private void WorldPVP_wanna_GotAK_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_GotAK = WorldPVP_wanna_GotAK.Checked;
        }

        private void WorldPVP_wanna_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_HoJ = WorldPVP_wanna_HoJ.Checked;
        }

        private void WorldPVP_wanna_HoP_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_HoP = WorldPVP_wanna_HoP.Checked;
        }

        private void WorldPVP_wanna_HoS_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_HoS = WorldPVP_wanna_HoS.Checked;
        }

        private void WorldPVP_wanna_HoW_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_HoW = WorldPVP_wanna_HoW.Checked;
        }

        private void WorldPVP_wanna_HR_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_HR = WorldPVP_wanna_HR.Checked;
        }

        private void WorldPVP_wanna_Judge_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_Judge = WorldPVP_wanna_Judge.Checked;
        }

        private void WorldPVP_wanna_LoH_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_LoH = WorldPVP_wanna_LoH.Checked;
        }

        private void WorldPVP_wanna_mana_potion_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_mana_potion = WorldPVP_wanna_mana_potion.Checked;
        }

        private void WorldPVP_wanna_mount_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_mount = WorldPVP_wanna_mount.Checked;
        }

        private void WorldPVP_wanna_move_to_heal_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_move_to_heal = WorldPVP_wanna_move_to_heal.Checked;
        }

        private void WorldPVP_wanna_move_to_HoJ_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_move_to_HoJ = WorldPVP_wanna_move_to_HoJ.Checked;
        }

        private void WorldPVP_wanna_rebuke_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_rebuke = WorldPVP_wanna_rebuke.Checked;
        }

        private void WorldPVP_wanna_stoneform_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_stoneform = WorldPVP_wanna_stoneform.Checked;
        }

        private void WorldPVP_wanna_target_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_target = WorldPVP_wanna_target.Checked;
        }

        private void WorldPVP_wanna_torrent_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_torrent = WorldPVP_wanna_torrent.Checked;
        }

        private void WorldPVP_wanna_urgent_cleanse_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_urgent_cleanse = WorldPVP_wanna_urgent_cleanse.Checked;
        }

        private void WorldPVP_auraselctGB_Enter(object sender, System.EventArgs e)
        {
            if (WorldPVP_concentrationRB.Checked) { UPaHBTSetting.Instance.WorldPVP_aura_type = 0; }
            else if (WorldPVP_resistanceRB.Checked) { UPaHBTSetting.Instance.WorldPVP_aura_type = 1; }
            else if (WorldPVP_DisabledRB.Checked) { UPaHBTSetting.Instance.WorldPVP_aura_type = 2; }
        }

        private void WorldPVP_concentrationRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_concentrationRB.Checked) { UPaHBTSetting.Instance.WorldPVP_aura_type = 0; }
        }

        private void WorldPVP_resistanceRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_resistanceRB.Checked) { UPaHBTSetting.Instance.WorldPVP_aura_type = 1; }
        }

        private void WorldPVP_DisabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_DisabledRB.Checked) { UPaHBTSetting.Instance.WorldPVP_aura_type = 2; }
        }

        private void WorldPVP_do_not_heal_above_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_do_not_heal_above = int.Parse(WorldPVP_do_not_heal_above.Value.ToString());
        }

        private void WorldPVP_HR_how_far_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_HR_how_far = int.Parse(WorldPVP_HR_how_far.Value.ToString());
        }

        private void WorldPVP_HR_how_much_health_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_HR_how_much_health = int.Parse(WorldPVP_HR_how_much_health.Value.ToString());
        }

        private void WorldPVP_mana_judge_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_mana_judge = int.Parse(WorldPVP_mana_judge.Value.ToString());
        }

        private void WorldPVP_max_healing_distance_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_max_healing_distance = int.Parse(WorldPVP_max_healing_distance.Value.ToString());
        }

        private void WorldPVP_min_Divine_Plea_mana_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_Divine_Plea_mana = int.Parse(WorldPVP_min_Divine_Plea_mana.Value.ToString());
        }

        private void WorldPVP_min_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_DL_hp = int.Parse(WorldPVP_min_DL_hp.Value.ToString());
        }

        private void WorldPVP_min_DP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_DP_hp = int.Parse(WorldPVP_min_DP_hp.Value.ToString());
        }

        private void WorldPVP_min_DS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_DS_hp = int.Parse(WorldPVP_min_DS_hp.Value.ToString());
        }

        private void WorldPVP_min_FoL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_FoL_hp = int.Parse(WorldPVP_min_FoL_hp.Value.ToString());
        }

        private void WorldPVP_min_gift_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_gift_hp = int.Parse(WorldPVP_min_gift_hp.Value.ToString());
        }

        private void WorldPVP_min_HL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_HL_hp = int.Parse(WorldPVP_min_HL_hp.Value.ToString());
        }

        private void WorldPVP_min_HoP_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_HoP_hp = int.Parse(WorldPVP_min_HoP_hp.Value.ToString());
        }

        private void WorldPVP_min_HoS_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_HoS_hp = int.Parse(WorldPVP_min_HoS_hp.Value.ToString());
        }

        private void WorldPVP_min_Inf_of_light_DL_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_Inf_of_light_DL_hp = int.Parse(WorldPVP_min_Inf_of_light_DL_hp.Value.ToString());
        }

        private void WorldPVP_min_LoH_hp_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_LoH_hp = int.Parse(WorldPVP_min_LoH_hp.Value.ToString());
        }

        private void WorldPVP_min_mana_potion_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_mana_potion = int.Parse(WorldPVP_min_mana_potion.Value.ToString());
        }

        private void WorldPVP_min_mana_rec_trinket_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_mana_rec_trinket = int.Parse(WorldPVP_min_mana_rec_trinket.Value.ToString());
        }

        private void WorldPVP_min_ohshitbutton_activator_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_ohshitbutton_activator = int.Parse(WorldPVP_min_ohshitbutton_activator.Value.ToString());
        }

        private void WorldPVP_use_mana_rec_trinket_every_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_use_mana_rec_trinket_every = int.Parse(WorldPVP_use_mana_rec_trinket_every.Value.ToString());
        }

        private void WorldPVP_min_stoneform_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_stoneform = int.Parse(WorldPVP_min_stoneform.Value.ToString());
        }

        private void WorldPVP_min_torrent_mana_perc_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_torrent_mana_perc = int.Parse(WorldPVP_min_torrent_mana_perc.Value.ToString());
        }

        private void WorldPVP_rest_if_mana_below_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_rest_if_mana_below = int.Parse(WorldPVP_rest_if_mana_below.Value.ToString());
        }

        private void WorldPVP_min_player_inside_HR_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_min_player_inside_HR = int.Parse(WorldPVP_min_player_inside_HR.Value.ToString());
        }

        private void WorldPVP_wanna_lifeblood_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_wanna_lifeblood = WorldPVP_wanna_lifeblood.Checked;
        }

        private void WorldPVP_do_not_dismount_ooc_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_do_not_dismount_ooc = WorldPVP_do_not_dismount_ooc.Checked;
        }

        private void WorldPVP_do_not_dismount_EVER_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_do_not_dismount_EVER = WorldPVP_do_not_dismount_EVER.Checked;
        }

        private void WorldPVP_advanced_option_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_advanced_option = WorldPVP_advanced_option.Checked;
            Change_advanced_option_visibility();
        }

        private void WorldPVP_get_tank_from_lua_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_get_tank_from_lua = WorldPVP_get_tank_from_lua.Checked;
        }

        private void Solo_bless_selection_Enter(object sender, System.EventArgs e)
        {
            if (Solo_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 0; }
            else if (Solo_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 1; }
            else if (Solo_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 2; }
            else if (Solo_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 4; }
        }

        private void Solo_bless_type_autoRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 0; }
        }

        private void Solo_bless_type_KingRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 1; }
        }

        private void Solo_bless_type_MightRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 2; }
        }

        private void Solo_bless_type_disabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.Solo_bless_type = 4; }
        }

        private void PVE_bless_selection_Enter(object sender, System.EventArgs e)
        {
            if (PVE_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 0; }
            else if (PVE_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 1; }
            else if (PVE_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 2; }
            else if (PVE_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 4; }
        }

        private void PVE_bless_type_autoRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 0; }
        }

        private void PVE_bless_type_KingRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 1; }
        }

        private void PVE_bless_type_MightRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 2; }
        }

        private void PVE_bless_type_disabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.PVE_bless_type = 4; }
        }

        private void Raid_bless_selection_Enter(object sender, System.EventArgs e)
        {
            if (Raid_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 0; }
            else if (Raid_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 1; }
            else if (Raid_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 2; }
            else if (Raid_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 4; }
        }

        private void Raid_bless_type_autoRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 0; }
        }

        private void Raid_bless_type_KingRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 1; }
        }

        private void Raid_bless_type_MightRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 2; }
        }

        private void Raid_bless_type_disabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.Raid_bless_type = 4; }
        }

        private void Battleground_bless_selection_Enter(object sender, System.EventArgs e)
        {
            if (Battleground_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 0; }
            else if (Battleground_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 1; }
            else if (Battleground_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 2; }
            else if (Battleground_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 4; }
        }

        private void Battleground_bless_type_autoRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 0; }
        }

        private void Battleground_bless_type_KingRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 1; }
        }

        private void Battleground_bless_type_MightRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 2; }
        }

        private void Battleground_bless_type_disabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.Battleground_bless_type = 4; }
        }

        private void ARENA_bless_selection_Enter(object sender, System.EventArgs e)
        {
            if (ARENA_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 0; }
            else if (ARENA_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 1; }
            else if (ARENA_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 2; }
            else if (ARENA_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 4; }
        }

        private void ARENA_bless_type_autoRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 0; }
        }

        private void ARENA_bless_type_KingRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 1; }
        }

        private void ARENA_bless_type_MightRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 2; }
        }

        private void ARENA_bless_type_disabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.ARENA_bless_type = 4; }
        }

        private void WorldPVP_bless_selection_Enter(object sender, System.EventArgs e)
        {
            if (WorldPVP_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 0; }
            else if (WorldPVP_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 1; }
            else if (WorldPVP_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 2; }
            else if (WorldPVP_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 4; }
        }

        private void WorldPVP_bless_type_autoRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_bless_type_autoRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 0; }
        }

        private void WorldPVP_bless_type_KingRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_bless_type_KingRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 1; }
        }

        private void WorldPVP_bless_type_MightRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_bless_type_MightRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 2; }
        }

        private void WorldPVP_bless_type_disabledRB_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_bless_type_disabledRB.Checked) { UPaHBTSetting.Instance.WorldPVP_bless_type = 4; }
        }

        private void PVE_healing_tank_priority_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_healing_tank_priority = int.Parse(PVE_healing_tank_priority.Value.ToString());
        }

        private void Raid_healing_tank_priority_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_healing_tank_priority = int.Parse(Raid_healing_tank_priority.Value.ToString());
        }

        private void Solo_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if(Solo_intellywait.Checked) {UPaHBTSetting.Instance.Solo_intellywait=true;}
            else if (Solo_speed.Checked) { UPaHBTSetting.Instance.Solo_decice_during_GCD = true; UPaHBTSetting.Instance.Solo_intellywait=false;}
            else if (Solo_accurancy.Checked) { UPaHBTSetting.Instance.Solo_decice_during_GCD = false; UPaHBTSetting.Instance.Solo_intellywait=false;}
        }

        private void Solo_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_intellywait.Checked) { UPaHBTSetting.Instance.Solo_intellywait = true; }
        }

        private void Solo_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_speed.Checked) { UPaHBTSetting.Instance.Solo_decice_during_GCD = true; UPaHBTSetting.Instance.Solo_intellywait = false; }
        }

        private void Solo_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Solo_accurancy.Checked) { UPaHBTSetting.Instance.Solo_decice_during_GCD = false; UPaHBTSetting.Instance.Solo_intellywait = false; }
        }

        private void PVE_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (PVE_intellywait.Checked) { UPaHBTSetting.Instance.PVE_intellywait = true; }
            else if (PVE_speed.Checked) { UPaHBTSetting.Instance.PVE_decice_during_GCD = true; UPaHBTSetting.Instance.PVE_intellywait = false; }
            else if (PVE_accurancy.Checked) { UPaHBTSetting.Instance.PVE_decice_during_GCD = false; UPaHBTSetting.Instance.PVE_intellywait = false; }
        }

        private void PVE_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_intellywait.Checked) { UPaHBTSetting.Instance.PVE_intellywait = true; }
        }

        private void PVE_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_speed.Checked) { UPaHBTSetting.Instance.PVE_decice_during_GCD = true; UPaHBTSetting.Instance.PVE_intellywait = false; }
        }

        private void PVE_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (PVE_accurancy.Checked) { UPaHBTSetting.Instance.PVE_decice_during_GCD = false; UPaHBTSetting.Instance.PVE_intellywait = false; }
        }

        private void Raid_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (Raid_intellywait.Checked) { UPaHBTSetting.Instance.Raid_intellywait = true; }
            else if (Raid_speed.Checked) { UPaHBTSetting.Instance.Raid_decice_during_GCD = true; UPaHBTSetting.Instance.Raid_intellywait = false; }
            else if (Raid_accurancy.Checked) { UPaHBTSetting.Instance.Raid_decice_during_GCD = false; UPaHBTSetting.Instance.Raid_intellywait = false; }
        }

        private void Raid_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_intellywait.Checked) { UPaHBTSetting.Instance.Raid_intellywait = true; }
        }

        private void Raid_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_speed.Checked) { UPaHBTSetting.Instance.Raid_decice_during_GCD = true; UPaHBTSetting.Instance.Raid_intellywait = false; }
        }

        private void Raid_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Raid_accurancy.Checked) { UPaHBTSetting.Instance.Raid_decice_during_GCD = false; UPaHBTSetting.Instance.Raid_intellywait = false; }
        }

        private void Battleground_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (Battleground_intellywait.Checked) { UPaHBTSetting.Instance.Battleground_intellywait = true; }
            else if (Battleground_speed.Checked) { UPaHBTSetting.Instance.Battleground_decice_during_GCD = true; UPaHBTSetting.Instance.Battleground_intellywait = false; }
            else if (Battleground_accurancy.Checked) { UPaHBTSetting.Instance.Battleground_decice_during_GCD = false; UPaHBTSetting.Instance.Battleground_intellywait = false; }
        }

        private void Battleground_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_intellywait.Checked) { UPaHBTSetting.Instance.Battleground_intellywait = true; }
        }

        private void Battleground_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_speed.Checked) { UPaHBTSetting.Instance.Battleground_decice_during_GCD = true; UPaHBTSetting.Instance.Battleground_intellywait = false; }
        }

        private void Battleground_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Battleground_accurancy.Checked) { UPaHBTSetting.Instance.Battleground_decice_during_GCD = false; UPaHBTSetting.Instance.Battleground_intellywait = false; }
        }

        private void ARENA_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (ARENA_intellywait.Checked) { UPaHBTSetting.Instance.ARENA_intellywait = true; }
            else if (ARENA_speed.Checked) { UPaHBTSetting.Instance.ARENA_decice_during_GCD = true; UPaHBTSetting.Instance.ARENA_intellywait = false; }
            else if (ARENA_accurancy.Checked) { UPaHBTSetting.Instance.ARENA_decice_during_GCD = false; UPaHBTSetting.Instance.ARENA_intellywait = false; }
        }

        private void ARENA_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_intellywait.Checked) { UPaHBTSetting.Instance.ARENA_intellywait = true; }
        }

        private void ARENA_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_speed.Checked) { UPaHBTSetting.Instance.ARENA_decice_during_GCD = true; UPaHBTSetting.Instance.ARENA_intellywait = false; }
        }

        private void ARENA_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ARENA_accurancy.Checked) { UPaHBTSetting.Instance.ARENA_decice_during_GCD = false; UPaHBTSetting.Instance.ARENA_intellywait = false; }
        }

        private void WorldPVP_optimizeGB_Enter(object sender, System.EventArgs e)
        {
            if (WorldPVP_intellywait.Checked) { UPaHBTSetting.Instance.WorldPVP_intellywait = true; }
            else if (WorldPVP_speed.Checked) { UPaHBTSetting.Instance.WorldPVP_decice_during_GCD = true; UPaHBTSetting.Instance.WorldPVP_intellywait = false; }
            else if (WorldPVP_accurancy.Checked) { UPaHBTSetting.Instance.WorldPVP_decice_during_GCD = false; UPaHBTSetting.Instance.WorldPVP_intellywait = false; }
        }

        private void WorldPVP_intellywait_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_intellywait.Checked) { UPaHBTSetting.Instance.WorldPVP_intellywait = true; }
        }

        private void WorldPVP_speed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_speed.Checked) { UPaHBTSetting.Instance.WorldPVP_decice_during_GCD = true; UPaHBTSetting.Instance.WorldPVP_intellywait = false; }
        }

        private void WorldPVP_accurancy_CheckedChanged(object sender, System.EventArgs e)
        {
            if (WorldPVP_accurancy.Checked) { UPaHBTSetting.Instance.WorldPVP_decice_during_GCD = false; UPaHBTSetting.Instance.WorldPVP_intellywait = false; }
        }

        private void Solo_answer_PVP_attacks_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_answer_PVP_attacks = Solo_answer_PVP_attacks.Checked;
        }

        private void PVP_do_not_touch_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB1 = PVP_do_not_touch_TB1.Text;
        }

        private void PVP_do_not_touch_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB2 = PVP_do_not_touch_TB2.Text;
        }

        private void PVP_do_not_touch_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB3 = PVP_do_not_touch_TB3.Text;
        }

        private void PVP_do_not_touch_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB4 = PVP_do_not_touch_TB4.Text;
        }

        private void PVP_do_not_touch_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB5 = PVP_do_not_touch_TB5.Text;
        }

        private void PVP_do_not_touch_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB6 = PVP_do_not_touch_TB6.Text;
        }

        private void PVP_do_not_touch_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB7 = PVP_do_not_touch_TB7.Text;
        }

        private void PVP_do_not_touch_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB8 = PVP_do_not_touch_TB8.Text;
        }

        private void PVP_do_not_touch_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB9 = PVP_do_not_touch_TB9.Text;
        }

        private void PVP_do_not_touch_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB10 = PVP_do_not_touch_TB10.Text;
        }

        private void PVP_do_not_touch_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB11 = PVP_do_not_touch_TB11.Text;
        }

        private void PVP_do_not_touch_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB12 = PVP_do_not_touch_TB12.Text;
        }

        private void PVP_do_not_touch_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB13 = PVP_do_not_touch_TB13.Text;
        }

        private void PVP_do_not_touch_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB14 = PVP_do_not_touch_TB14.Text;
        }

        private void PVP_do_not_touch_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB15 = PVP_do_not_touch_TB15.Text;
        }

        private void PVP_do_not_touch_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB16 = PVP_do_not_touch_TB16.Text;
        }

        private void PVP_do_not_touch_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB17 = PVP_do_not_touch_TB17.Text;
        }

        private void PVP_do_not_touch_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB18 = PVP_do_not_touch_TB18.Text;
        }

        private void PVP_do_not_touch_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB19 = PVP_do_not_touch_TB19.Text;
        }

        private void PVP_do_not_touch_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_do_not_touch_TB20 = PVP_do_not_touch_TB20.Text;
        }

        private void PVP_dispell_ASAP_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB1 = PVP_dispell_ASAP_TB1.Text;
        }

        private void PVP_dispell_ASAP_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB2 = PVP_dispell_ASAP_TB2.Text;
        }

        private void PVP_dispell_ASAP_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB3 = PVP_dispell_ASAP_TB3.Text;
        }

        private void PVP_dispell_ASAP_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB4 = PVP_dispell_ASAP_TB4.Text;
        }

        private void PVP_dispell_ASAP_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB5 = PVP_dispell_ASAP_TB5.Text;
        }

        private void PVP_dispell_ASAP_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB6 = PVP_dispell_ASAP_TB6.Text;
        }

        private void PVP_dispell_ASAP_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB7 = PVP_dispell_ASAP_TB7.Text;
        }

        private void PVP_dispell_ASAP_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB8 = PVP_dispell_ASAP_TB8.Text;
        }

        private void PVP_dispell_ASAP_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB9 = PVP_dispell_ASAP_TB9.Text;
        }

        private void PVP_dispell_ASAP_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB10 = PVP_dispell_ASAP_TB10.Text;
        }

        private void PVP_dispell_ASAP_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB11 = PVP_dispell_ASAP_TB11.Text;
        }

        private void PVP_dispell_ASAP_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB12 = PVP_dispell_ASAP_TB12.Text;
        }

        private void PVP_dispell_ASAP_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB13 = PVP_dispell_ASAP_TB13.Text;
        }

        private void PVP_dispell_ASAP_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB14 = PVP_dispell_ASAP_TB14.Text;
        }

        private void PVP_dispell_ASAP_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB15 = PVP_dispell_ASAP_TB15.Text;
        }

        private void PVP_dispell_ASAP_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB16 = PVP_dispell_ASAP_TB16.Text;
        }

        private void PVP_dispell_ASAP_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB17 = PVP_dispell_ASAP_TB17.Text;
        }

        private void PVP_dispell_ASAP_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB18 = PVP_dispell_ASAP_TB18.Text;
        }

        private void PVP_dispell_ASAP_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB19 = PVP_dispell_ASAP_TB19.Text;
        }

        private void PVP_dispell_ASAP_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVP_dispell_ASAP_TB20 = PVP_dispell_ASAP_TB20.Text;
        }

        private void PVE_do_not_touch_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB1 = PVE_do_not_touch_TB1.Text;
        }

        private void PVE_do_not_touch_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB2 = PVE_do_not_touch_TB2.Text;
        }

        private void PVE_do_not_touch_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB3 = PVE_do_not_touch_TB3.Text;
        }

        private void PVE_do_not_touch_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB4 = PVE_do_not_touch_TB4.Text;
        }

        private void PVE_do_not_touch_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB5 = PVE_do_not_touch_TB5.Text;
        }

        private void PVE_do_not_touch_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB6 = PVE_do_not_touch_TB6.Text;
        }

        private void PVE_do_not_touch_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB7 = PVE_do_not_touch_TB7.Text;
        }

        private void PVE_do_not_touch_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB8 = PVE_do_not_touch_TB8.Text;
        }

        private void PVE_do_not_touch_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB9 = PVE_do_not_touch_TB9.Text;
        }

        private void PVE_do_not_touch_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB10 = PVE_do_not_touch_TB10.Text;
        }

        private void PVE_do_not_touch_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB11 = PVE_do_not_touch_TB11.Text;
        }

        private void PVE_do_not_touch_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB12 = PVE_do_not_touch_TB12.Text;
        }

        private void PVE_do_not_touch_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB13 = PVE_do_not_touch_TB13.Text;
        }

        private void PVE_do_not_touch_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB14 = PVE_do_not_touch_TB14.Text;
        }

        private void PVE_do_not_touch_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB15 = PVE_do_not_touch_TB15.Text;
        }

        private void PVE_do_not_touch_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB16 = PVE_do_not_touch_TB16.Text;
        }

        private void PVE_do_not_touch_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB17 = PVE_do_not_touch_TB17.Text;
        }

        private void PVE_do_not_touch_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB18 = PVE_do_not_touch_TB18.Text;
        }

        private void PVE_do_not_touch_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB19 = PVE_do_not_touch_TB19.Text;
        }

        private void PVE_do_not_touch_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_do_not_touch_TB20 = PVE_do_not_touch_TB20.Text;
        }

        private void PVE_dispell_ASAP_TB1_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB1 = PVE_dispell_ASAP_TB1.Text;
        }

        private void PVE_dispell_ASAP_TB2_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB2 = PVE_dispell_ASAP_TB2.Text;
        }

        private void PVE_dispell_ASAP_TB3_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB3 = PVE_dispell_ASAP_TB3.Text;
        }

        private void PVE_dispell_ASAP_TB4_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB4 = PVE_dispell_ASAP_TB4.Text;
        }

        private void PVE_dispell_ASAP_TB5_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB5 = PVE_dispell_ASAP_TB5.Text;
        }

        private void PVE_dispell_ASAP_TB6_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB6 = PVE_dispell_ASAP_TB6.Text;
        }

        private void PVE_dispell_ASAP_TB7_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB7 = PVE_dispell_ASAP_TB7.Text;
        }

        private void PVE_dispell_ASAP_TB8_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB8 = PVE_dispell_ASAP_TB8.Text;
        }

        private void PVE_dispell_ASAP_TB9_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB9 = PVE_dispell_ASAP_TB9.Text;
        }

        private void PVE_dispell_ASAP_TB10_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB10 = PVE_dispell_ASAP_TB10.Text;
        }

        private void PVE_dispell_ASAP_TB11_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB11 = PVE_dispell_ASAP_TB11.Text;
        }

        private void PVE_dispell_ASAP_TB12_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB12 = PVE_dispell_ASAP_TB12.Text;
        }

        private void PVE_dispell_ASAP_TB13_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB13 = PVE_dispell_ASAP_TB13.Text;
        }

        private void PVE_dispell_ASAP_TB14_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB14 = PVE_dispell_ASAP_TB14.Text;
        }

        private void PVE_dispell_ASAP_TB15_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB15 = PVE_dispell_ASAP_TB15.Text;
        }

        private void PVE_dispell_ASAP_TB16_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB16 = PVE_dispell_ASAP_TB16.Text;
        }

        private void PVE_dispell_ASAP_TB17_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB17 = PVE_dispell_ASAP_TB17.Text;
        }

        private void PVE_dispell_ASAP_TB18_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB18 = PVE_dispell_ASAP_TB18.Text;
        }

        private void PVE_dispell_ASAP_TB19_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB19 = PVE_dispell_ASAP_TB19.Text;
        }

        private void PVE_dispell_ASAP_TB20_TextChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_dispell_ASAP_TB20 = PVE_dispell_ASAP_TB20.Text;
        }

        private void Solo_enable_pull_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Solo_enable_pull = Solo_enable_pull.Checked;
        }

        private void Trnket1_name_load_Click(object sender, System.EventArgs e)
        {
            Logging.Write("Starting loading trinkets");
            UltimatePalaHealerBT.Instance.Load_Trinket();
            Trinket1_name_LB.Text = "Name: " + UPaHBTSetting.Instance.Trinket1_name;
            Trinket1_ID_LB.Text = "ID: " + UPaHBTSetting.Instance.Trinket1_ID;
            if (!UPaHBTSetting.Instance.Trinket1_passive)
            {
                Trinket1_use_when_GB.Enabled = true;
                Trinket1_CD_LB.Text = "Cooldown: " + UPaHBTSetting.Instance.Trinket1_CD + " seconds max";
                if (UPaHBTSetting.Instance.Trinket1_use_when == 0) { Trinket1_never.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket1_use_when == 1) { Trinket1_HPS.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket1_use_when == 2) { Trinket1_mana.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket1_use_when == 3) { Trinket1_PVP.Checked = true; }
            }
            else
            {
                Trinket1_CD_LB.Text = "This Trinket is a Passive one, nothing to do";
                Trinket1_never.Checked = true;
                Trinket1_use_when_GB.Enabled = false;
            }

            Trinket2_name_LB.Text = "Name: " + UPaHBTSetting.Instance.Trinket2_name;
            Trinket2_ID_LB.Text = "ID: " + UPaHBTSetting.Instance.Trinket2_ID;
            if (!UPaHBTSetting.Instance.Trinket2_passive)
            {
                Trinket2_use_when_GB.Enabled = true;
                Trinket2_CD_LB.Text = "Cooldown: " + UPaHBTSetting.Instance.Trinket2_CD + " seconds max";
                if (UPaHBTSetting.Instance.Trinket2_use_when == 0) { Trinket2_never.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket2_use_when == 1) { Trinket2_HPS.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket2_use_when == 2) { Trinket2_mana.Checked = true; }
                else if (UPaHBTSetting.Instance.Trinket2_use_when == 3) { Trinket2_PVP.Checked = true; }
            }
            else
            {
                Trinket2_CD_LB.Text = "This Trinket is a Passive one, nothing to do";
                Trinket2_never.Checked = true;
                Trinket2_use_when_GB.Enabled = false;
            }
            Logging.Write("END loading trinkets");
        }

        private void Raid_tank_healing_priority_multiplier_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_tank_healing_priority_multiplier = int.Parse(Raid_tank_healing_priority_multiplier.Value.ToString());
        }

        private void PVE_tank_healing_priority_multiplier_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_tank_healing_priority_multiplier = int.Parse(PVE_tank_healing_priority_multiplier.Value.ToString());
        }

        private void Trinket1_never_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket1_never.Checked) { UPaHBTSetting.Instance.Trinket1_use_when = 0; }
        }

        private void Trinket1_HPS_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket1_HPS.Checked) { UPaHBTSetting.Instance.Trinket1_use_when = 1; }
        }

        private void Trinket1_mana_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket1_mana.Checked) { UPaHBTSetting.Instance.Trinket1_use_when = 2; }
        }

        private void Trinket1_PVP_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket1_PVP.Checked) { UPaHBTSetting.Instance.Trinket1_use_when = 3; }
        }

        private void Trinket2_never_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket2_never.Checked) { UPaHBTSetting.Instance.Trinket2_use_when = 0; }
        }

        private void Trinket2_HPS_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket2_HPS.Checked) { UPaHBTSetting.Instance.Trinket2_use_when = 1; }
        }

        private void Trinket2_mana_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket2_mana.Checked) { UPaHBTSetting.Instance.Trinket2_use_when = 2; }
        }

        private void Trinket2_PVP_CheckedChanged(object sender, System.EventArgs e)
        {
            if (Trinket2_PVP.Checked) { UPaHBTSetting.Instance.Trinket2_use_when = 3; }
        }

        private void Stop_Healing_BT_Click(object sender, System.EventArgs e)
        {
            if (UPaHBTSetting.Instance.General_Stop_Healing == false)
            {
                UPaHBTSetting.Instance.General_Stop_Healing = true;
                Stop_Healing_BT.Text = "START Healing Again";
                Stop_Healing_BT.ForeColor = Color.Green;
                UPaHBTSetting.Instance.Save();
                UPaHBTSetting.Instance.Load();
                UltimatePalaHealerBT.Instance.Variable_inizializer();
                Logging.Write(Color.DarkRed, "You are stopping all healing NOW!");
            }
            else
            {
                UPaHBTSetting.Instance.General_Stop_Healing = false;
                Stop_Healing_BT.Text = "Stop ALL Healing";
                Stop_Healing_BT.ForeColor = Color.Red;
                UPaHBTSetting.Instance.Save();
                UPaHBTSetting.Instance.Load();
                UltimatePalaHealerBT.Instance.Variable_inizializer();
                Logging.Write(Color.Green, "Starting Healing Again!");
            }
        }

        private void General_precasting_ValueChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.General_precasting = int.Parse(General_precasting.Value.ToString());
        }

        private void PVE_cleanse_only_self_and_tank_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.PVE_cleanse_only_self_and_tank = PVE_cleanse_only_self_and_tank.Checked;
        }

        private void Raid_cleanse_only_self_and_tank_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Raid_cleanse_only_self_and_tank = Raid_cleanse_only_self_and_tank.Checked;
        }

        private void Battleground_cleanse_only_self_and_tank_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.Battleground_cleanse_only_self_and_tank = Battleground_cleanse_only_self_and_tank.Checked;
        }

        private void ARENA_cleanse_only_self_and_tank_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.ARENA_cleanse_only_self_and_tank = ARENA_cleanse_only_self_and_tank.Checked;
        }

        private void WorldPVP_cleanse_only_self_and_tank_CheckedChanged(object sender, System.EventArgs e)
        {
            UPaHBTSetting.Instance.WorldPVP_cleanse_only_self_and_tank = WorldPVP_cleanse_only_self_and_tank.Checked;
        }

        private void cbLogging_CheckedChanged(object sender, EventArgs e)
        {
            UPaHBTSetting.Instance.WriteLog = cbLogging.Checked;
        }

    }
}
