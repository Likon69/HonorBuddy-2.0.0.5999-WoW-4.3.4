using Styx.Helpers;
namespace UltimatePaladinHealerBT
{
    partial class UPaHCCBTConfigForm
    {
        //private int How_many_in_raid; //never used, old code?
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void Disappear_select_healing()
        {
            if (!SelectHeal.Checked)
            {
                this.RaidCK0.Visible = false;
                this.RaidCK1.Visible = false;
                this.RaidCK2.Visible = false;
                this.RaidCK3.Visible = false;
                this.RaidCK4.Visible = false;
                this.RaidCK5.Visible = false;
                this.RaidCK6.Visible = false;
                this.RaidCK7.Visible = false;
                this.RaidCK8.Visible = false;
                this.RaidCK9.Visible = false;
                this.RaidCK10.Visible = false;
                this.RaidCK11.Visible = false;
                this.RaidCK12.Visible = false;
                this.RaidCK13.Visible = false;
                this.RaidCK14.Visible = false;
                this.RaidCK15.Visible = false;
                this.RaidCK16.Visible = false;
                this.RaidCK17.Visible = false;
                this.RaidCK18.Visible = false;
                this.RaidCK19.Visible = false;
                this.RaidCK20.Visible = false;
                this.RaidCK21.Visible = false;
                this.RaidCK22.Visible = false;
                this.RaidCK23.Visible = false;
                this.RaidCK24.Visible = false;
                this.RaidCK25.Visible = false;
                this.RaidCK26.Visible = false;
                this.RaidCK27.Visible = false;
                this.RaidCK28.Visible = false;
                this.RaidCK29.Visible = false;
                this.RaidCK30.Visible = false;
                this.RaidCK31.Visible = false;
                this.RaidCK32.Visible = false;
                this.RaidCK33.Visible = false;
                this.RaidCK34.Visible = false;
                this.RaidCK35.Visible = false;
                this.RaidCK36.Visible = false;
                this.RaidCK37.Visible = false;
                this.RaidCK38.Visible = false;
                this.RaidCK39.Visible = false;
            }
            else
            {
                this.RaidCK0.Visible = true;
                this.RaidCK1.Visible = true;
                this.RaidCK2.Visible = true;
                this.RaidCK3.Visible = true;
                this.RaidCK4.Visible = true;
                this.RaidCK5.Visible = true;
                this.RaidCK6.Visible = true;
                this.RaidCK7.Visible = true;
                this.RaidCK8.Visible = true;
                this.RaidCK9.Visible = true;
                this.RaidCK10.Visible = true;
                this.RaidCK11.Visible = true;
                this.RaidCK12.Visible = true;
                this.RaidCK13.Visible = true;
                this.RaidCK14.Visible = true;
                this.RaidCK15.Visible = true;
                this.RaidCK16.Visible = true;
                this.RaidCK17.Visible = true;
                this.RaidCK18.Visible = true;
                this.RaidCK19.Visible = true;
                this.RaidCK20.Visible = true;
                this.RaidCK21.Visible = true;
                this.RaidCK22.Visible = true;
                this.RaidCK23.Visible = true;
                this.RaidCK24.Visible = true;
                this.RaidCK25.Visible = true;
                this.RaidCK26.Visible = true;
                this.RaidCK27.Visible = true;
                this.RaidCK28.Visible = true;
                this.RaidCK29.Visible = true;
                this.RaidCK30.Visible = true;
                this.RaidCK31.Visible = true;
                this.RaidCK32.Visible = true;
                this.RaidCK33.Visible = true;
                this.RaidCK34.Visible = true;
                this.RaidCK35.Visible = true;
                this.RaidCK36.Visible = true;
                this.RaidCK37.Visible = true;
                this.RaidCK38.Visible = true;
                this.RaidCK39.Visible = true;
            }
        }

        private void Change_advanced_option_visibility()
        {
            this.Solo_advanced.Visible = UPaHBTSetting.Instance.Solo_advanced_option;
            this.PVE_advanced.Visible = UPaHBTSetting.Instance.PVE_advanced_option;
            this.ARENA_advanced.Visible = UPaHBTSetting.Instance.ARENA_advanced_option;
            this.Raid_advanced.Visible = UPaHBTSetting.Instance.Raid_advanced_option;
            this.Battleground_advanced.Visible = UPaHBTSetting.Instance.Battleground_advanced_option;
            this.WorldPVP_advanced.Visible = UPaHBTSetting.Instance.WorldPVP_advanced_option;
        }

        private void Populate_components()
        {
            Disappear_select_healing();/*
            this.RaidCK0.Text = UltimatePalaHealerBT.Instance.OrganizedNames[1];
            this.RaidCK1.Text = UltimatePalaHealerBT.Instance.OrganizedNames[2];
            this.RaidCK2.Text = UltimatePalaHealerBT.Instance.OrganizedNames[3];
            this.RaidCK3.Text = UltimatePalaHealerBT.Instance.OrganizedNames[4];
            this.RaidCK4.Text = UltimatePalaHealerBT.Instance.OrganizedNames[5];
            this.RaidCK5.Text = UltimatePalaHealerBT.Instance.OrganizedNames[6];
            this.RaidCK6.Text = UltimatePalaHealerBT.Instance.OrganizedNames[7];
            this.RaidCK7.Text = UltimatePalaHealerBT.Instance.OrganizedNames[8];
            this.RaidCK8.Text = UltimatePalaHealerBT.Instance.OrganizedNames[9];
            this.RaidCK9.Text = UltimatePalaHealerBT.Instance.OrganizedNames[10];
            this.RaidCK10.Text = UltimatePalaHealerBT.Instance.OrganizedNames[11];
            this.RaidCK11.Text = UltimatePalaHealerBT.Instance.OrganizedNames[12];
            this.RaidCK12.Text = UltimatePalaHealerBT.Instance.OrganizedNames[13];
            this.RaidCK13.Text = UltimatePalaHealerBT.Instance.OrganizedNames[14];
            this.RaidCK14.Text = UltimatePalaHealerBT.Instance.OrganizedNames[15];
            this.RaidCK15.Text = UltimatePalaHealerBT.Instance.OrganizedNames[16];
            this.RaidCK16.Text = UltimatePalaHealerBT.Instance.OrganizedNames[17];
            this.RaidCK17.Text = UltimatePalaHealerBT.Instance.OrganizedNames[18];
            this.RaidCK18.Text = UltimatePalaHealerBT.Instance.OrganizedNames[19];
            this.RaidCK19.Text = UltimatePalaHealerBT.Instance.OrganizedNames[20];
            this.RaidCK20.Text = UltimatePalaHealerBT.Instance.OrganizedNames[21];
            this.RaidCK21.Text = UltimatePalaHealerBT.Instance.OrganizedNames[22];
            this.RaidCK22.Text = UltimatePalaHealerBT.Instance.OrganizedNames[23];
            this.RaidCK23.Text = UltimatePalaHealerBT.Instance.OrganizedNames[24];
            this.RaidCK24.Text = UltimatePalaHealerBT.Instance.OrganizedNames[25];
            this.RaidCK25.Text = UltimatePalaHealerBT.Instance.OrganizedNames[26];
            this.RaidCK26.Text = UltimatePalaHealerBT.Instance.OrganizedNames[27];
            this.RaidCK27.Text = UltimatePalaHealerBT.Instance.OrganizedNames[28];
            this.RaidCK28.Text = UltimatePalaHealerBT.Instance.OrganizedNames[29];
            this.RaidCK29.Text = UltimatePalaHealerBT.Instance.OrganizedNames[30];
            this.RaidCK30.Text = UltimatePalaHealerBT.Instance.OrganizedNames[31];
            this.RaidCK31.Text = UltimatePalaHealerBT.Instance.OrganizedNames[32];
            this.RaidCK32.Text = UltimatePalaHealerBT.Instance.OrganizedNames[33];
            this.RaidCK33.Text = UltimatePalaHealerBT.Instance.OrganizedNames[34];
            this.RaidCK34.Text = UltimatePalaHealerBT.Instance.OrganizedNames[35];
            this.RaidCK35.Text = UltimatePalaHealerBT.Instance.OrganizedNames[36];
            this.RaidCK36.Text = UltimatePalaHealerBT.Instance.OrganizedNames[37];
            this.RaidCK37.Text = UltimatePalaHealerBT.Instance.OrganizedNames[38];
            this.RaidCK38.Text = UltimatePalaHealerBT.Instance.OrganizedNames[39];
            this.RaidCK39.Text = UltimatePalaHealerBT.Instance.OrganizedNames[40];
            */
            this.RaidCK0.Text = UltimatePalaHealerBT.Instance.WoWnames[0];
            this.RaidCK1.Text = UltimatePalaHealerBT.Instance.WoWnames[1];
            this.RaidCK2.Text = UltimatePalaHealerBT.Instance.WoWnames[2];
            this.RaidCK3.Text = UltimatePalaHealerBT.Instance.WoWnames[3];
            this.RaidCK4.Text = UltimatePalaHealerBT.Instance.WoWnames[4];
            this.RaidCK5.Text = UltimatePalaHealerBT.Instance.WoWnames[5];
            this.RaidCK6.Text = UltimatePalaHealerBT.Instance.WoWnames[6];
            this.RaidCK7.Text = UltimatePalaHealerBT.Instance.WoWnames[7];
            this.RaidCK8.Text = UltimatePalaHealerBT.Instance.WoWnames[8];
            this.RaidCK9.Text = UltimatePalaHealerBT.Instance.WoWnames[9];
            this.RaidCK10.Text = UltimatePalaHealerBT.Instance.WoWnames[10];
            this.RaidCK11.Text = UltimatePalaHealerBT.Instance.WoWnames[11];
            this.RaidCK12.Text = UltimatePalaHealerBT.Instance.WoWnames[12];
            this.RaidCK13.Text = UltimatePalaHealerBT.Instance.WoWnames[13];
            this.RaidCK14.Text = UltimatePalaHealerBT.Instance.WoWnames[14];
            this.RaidCK15.Text = UltimatePalaHealerBT.Instance.WoWnames[15];
            this.RaidCK16.Text = UltimatePalaHealerBT.Instance.WoWnames[16];
            this.RaidCK17.Text = UltimatePalaHealerBT.Instance.WoWnames[17];
            this.RaidCK18.Text = UltimatePalaHealerBT.Instance.WoWnames[18];
            this.RaidCK19.Text = UltimatePalaHealerBT.Instance.WoWnames[19];
            this.RaidCK20.Text = UltimatePalaHealerBT.Instance.WoWnames[20];
            this.RaidCK21.Text = UltimatePalaHealerBT.Instance.WoWnames[21];
            this.RaidCK22.Text = UltimatePalaHealerBT.Instance.WoWnames[22];
            this.RaidCK23.Text = UltimatePalaHealerBT.Instance.WoWnames[23];
            this.RaidCK24.Text = UltimatePalaHealerBT.Instance.WoWnames[24];
            this.RaidCK25.Text = UltimatePalaHealerBT.Instance.WoWnames[25];
            this.RaidCK26.Text = UltimatePalaHealerBT.Instance.WoWnames[26];
            this.RaidCK27.Text = UltimatePalaHealerBT.Instance.WoWnames[27];
            this.RaidCK28.Text = UltimatePalaHealerBT.Instance.WoWnames[28];
            this.RaidCK29.Text = UltimatePalaHealerBT.Instance.WoWnames[29];
            this.RaidCK30.Text = UltimatePalaHealerBT.Instance.WoWnames[30];
            this.RaidCK31.Text = UltimatePalaHealerBT.Instance.WoWnames[31];
            this.RaidCK32.Text = UltimatePalaHealerBT.Instance.WoWnames[32];
            this.RaidCK33.Text = UltimatePalaHealerBT.Instance.WoWnames[33];
            this.RaidCK34.Text = UltimatePalaHealerBT.Instance.WoWnames[34];
            this.RaidCK35.Text = UltimatePalaHealerBT.Instance.WoWnames[35];
            this.RaidCK36.Text = UltimatePalaHealerBT.Instance.WoWnames[36];
            this.RaidCK37.Text = UltimatePalaHealerBT.Instance.WoWnames[37];
            this.RaidCK38.Text = UltimatePalaHealerBT.Instance.WoWnames[38];
            this.RaidCK39.Text = UltimatePalaHealerBT.Instance.WoWnames[39];



            this.RaidL0.Visible = UltimatePalaHealerBT.Instance.Raidrole[0];
            this.RaidL1.Visible = UltimatePalaHealerBT.Instance.Raidrole[1];
            this.RaidL2.Visible = UltimatePalaHealerBT.Instance.Raidrole[2];
            this.RaidL3.Visible = UltimatePalaHealerBT.Instance.Raidrole[3];
            this.RaidL4.Visible = UltimatePalaHealerBT.Instance.Raidrole[4];
            this.RaidL5.Visible = UltimatePalaHealerBT.Instance.Raidrole[5];
            this.RaidL6.Visible = UltimatePalaHealerBT.Instance.Raidrole[6];
            this.RaidL7.Visible = UltimatePalaHealerBT.Instance.Raidrole[7];
            this.RaidL8.Visible = UltimatePalaHealerBT.Instance.Raidrole[8];
            this.RaidL9.Visible = UltimatePalaHealerBT.Instance.Raidrole[9];
            this.RaidL10.Visible = UltimatePalaHealerBT.Instance.Raidrole[10];
            this.RaidL11.Visible = UltimatePalaHealerBT.Instance.Raidrole[11];
            this.RaidL12.Visible = UltimatePalaHealerBT.Instance.Raidrole[12];
            this.RaidL13.Visible = UltimatePalaHealerBT.Instance.Raidrole[13];
            this.RaidL14.Visible = UltimatePalaHealerBT.Instance.Raidrole[14];
            this.RaidL15.Visible = UltimatePalaHealerBT.Instance.Raidrole[15];
            this.RaidL16.Visible = UltimatePalaHealerBT.Instance.Raidrole[16];
            this.RaidL17.Visible = UltimatePalaHealerBT.Instance.Raidrole[17];
            this.RaidL18.Visible = UltimatePalaHealerBT.Instance.Raidrole[18];
            this.RaidL19.Visible = UltimatePalaHealerBT.Instance.Raidrole[19];
            this.RaidL20.Visible = UltimatePalaHealerBT.Instance.Raidrole[20];
            this.RaidL21.Visible = UltimatePalaHealerBT.Instance.Raidrole[21];
            this.RaidL22.Visible = UltimatePalaHealerBT.Instance.Raidrole[22];
            this.RaidL23.Visible = UltimatePalaHealerBT.Instance.Raidrole[23];
            this.RaidL24.Visible = UltimatePalaHealerBT.Instance.Raidrole[24];
            this.RaidL25.Visible = UltimatePalaHealerBT.Instance.Raidrole[25];
            this.RaidL26.Visible = UltimatePalaHealerBT.Instance.Raidrole[26];
            this.RaidL27.Visible = UltimatePalaHealerBT.Instance.Raidrole[27];
            this.RaidL28.Visible = UltimatePalaHealerBT.Instance.Raidrole[28];
            this.RaidL29.Visible = UltimatePalaHealerBT.Instance.Raidrole[29];
            this.RaidL30.Visible = UltimatePalaHealerBT.Instance.Raidrole[30];
            this.RaidL31.Visible = UltimatePalaHealerBT.Instance.Raidrole[31];
            this.RaidL32.Visible = UltimatePalaHealerBT.Instance.Raidrole[32];
            this.RaidL33.Visible = UltimatePalaHealerBT.Instance.Raidrole[33];
            this.RaidL34.Visible = UltimatePalaHealerBT.Instance.Raidrole[34];
            this.RaidL35.Visible = UltimatePalaHealerBT.Instance.Raidrole[35];
            this.RaidL36.Visible = UltimatePalaHealerBT.Instance.Raidrole[36];
            this.RaidL37.Visible = UltimatePalaHealerBT.Instance.Raidrole[37];
            this.RaidL38.Visible = UltimatePalaHealerBT.Instance.Raidrole[38];
            this.RaidL39.Visible = UltimatePalaHealerBT.Instance.Raidrole[39];

        }
        private void InitializeComponent()
        {
            this.Solo_wanna_crusader = new System.Windows.Forms.CheckBox();
            this.save = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage14 = new System.Windows.Forms.TabPage();
            this.cbLogging = new System.Windows.Forms.CheckBox();
            this.label83 = new System.Windows.Forms.Label();
            this.General_precasting = new System.Windows.Forms.NumericUpDown();
            this.Stop_Healing_BT = new System.Windows.Forms.Button();
            this.label82 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox64 = new System.Windows.Forms.GroupBox();
            this.Solo_exorcism_min_mana = new System.Windows.Forms.NumericUpDown();
            this.Solo_holy_wrath_min_mana = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_holy_wrath = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_exorcism = new System.Windows.Forms.CheckBox();
            this.Solo_exorcism_for_denunce = new System.Windows.Forms.CheckBox();
            this.Solo_consecration_min_mana = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_consecration = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Solo_enable_pull = new System.Windows.Forms.CheckBox();
            this.Solo_answer_PVP_attacks = new System.Windows.Forms.CheckBox();
            this.Solo_optimizeGB = new System.Windows.Forms.GroupBox();
            this.Solo_account_for_lag = new System.Windows.Forms.CheckBox();
            this.Solo_intellywait = new System.Windows.Forms.RadioButton();
            this.Solo_accurancy = new System.Windows.Forms.RadioButton();
            this.Solo_speed = new System.Windows.Forms.RadioButton();
            this.Solo_bless_selection = new System.Windows.Forms.GroupBox();
            this.Solo_bless_type_disabledRB = new System.Windows.Forms.RadioButton();
            this.Solo_bless_type_MightRB = new System.Windows.Forms.RadioButton();
            this.Solo_bless_type_KingRB = new System.Windows.Forms.RadioButton();
            this.Solo_bless_type_autoRB = new System.Windows.Forms.RadioButton();
            this.Solo_overhealing_protection = new System.Windows.Forms.GroupBox();
            this.Solo_tankselection = new System.Windows.Forms.GroupBox();
            this.Solo_get_tank_from_lua = new System.Windows.Forms.CheckBox();
            this.Solo_get_tank_from_focus = new System.Windows.Forms.CheckBox();
            this.Solo_adancePanel = new System.Windows.Forms.Panel();
            this.Solo_advanced_option = new System.Windows.Forms.CheckBox();
            this.Solo_advanced = new System.Windows.Forms.GroupBox();
            this.Solo_wanna_move = new System.Windows.Forms.CheckBox();
            this.Solo_max_healing_distanceLB = new System.Windows.Forms.Label();
            this.Solo_max_healing_distance = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_target = new System.Windows.Forms.CheckBox();
            this.Solo_donothealaboveGB = new System.Windows.Forms.GroupBox();
            this.Solo_do_not_heal_above = new System.Windows.Forms.NumericUpDown();
            this.Solo_donothealaboveLB = new System.Windows.Forms.Label();
            this.Solo_wanna_face = new System.Windows.Forms.CheckBox();
            this.Solo_CleanseGB = new System.Windows.Forms.GroupBox();
            this.Solo_wanna_cleanse = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_urgent_cleanse = new System.Windows.Forms.CheckBox();
            this.Solo_interruptersGB = new System.Windows.Forms.GroupBox();
            this.Solo_wanna_HoJ = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_rebuke = new System.Windows.Forms.CheckBox();
            this.Solo_Healing = new System.Windows.Forms.GroupBox();
            this.Solo_min_Inf_of_light_DL_hpLB = new System.Windows.Forms.Label();
            this.Solo_min_Inf_of_light_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_HL_hpLB = new System.Windows.Forms.Label();
            this.Solo_min_HL_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_FoL_hpLB = new System.Windows.Forms.Label();
            this.Solo_min_FoL_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_DL_hpLB = new System.Windows.Forms.Label();
            this.Solo_min_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_Inf_of_light_wanna_DL = new System.Windows.Forms.CheckBox();
            this.Solo_mana_management = new System.Windows.Forms.GroupBox();
            this.Solo_use_mana_rec_trinket_everyLB = new System.Windows.Forms.Label();
            this.Solo_use_mana_rec_trinket_every = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_mana_rec_trinketLB = new System.Windows.Forms.Label();
            this.Solo_min_mana_rec_trinket = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_Divine_Plea_manaLB = new System.Windows.Forms.Label();
            this.Solo_min_Divine_Plea_mana = new System.Windows.Forms.NumericUpDown();
            this.Solo_mana_judgeLB = new System.Windows.Forms.Label();
            this.Solo_mana_judge = new System.Windows.Forms.NumericUpDown();
            this.Solo_HRGB = new System.Windows.Forms.GroupBox();
            this.Solo_min_player_inside_HRLB = new System.Windows.Forms.Label();
            this.Solo_min_player_inside_HR = new System.Windows.Forms.NumericUpDown();
            this.Solo_HR_how_much_healthLB = new System.Windows.Forms.Label();
            this.Solo_HR_how_much_health = new System.Windows.Forms.NumericUpDown();
            this.Solo_HR_how_farLB = new System.Windows.Forms.Label();
            this.Solo_HR_how_far = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_HR = new System.Windows.Forms.CheckBox();
            this.Solo_emergencyGB = new System.Windows.Forms.GroupBox();
            this.Solo_min_mana_potion = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_LoH_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_HoS_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_HoP_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_DS_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_emergency_buttonLB = new System.Windows.Forms.Label();
            this.Solo_min_DP_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_mana_potion = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_LoH = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_DP = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_DS = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_HoS = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_HoP = new System.Windows.Forms.CheckBox();
            this.Solo_auraselctGB = new System.Windows.Forms.GroupBox();
            this.Solo_DisabledRB = new System.Windows.Forms.RadioButton();
            this.Solo_resistanceRB = new System.Windows.Forms.RadioButton();
            this.Solo_concentrationRB = new System.Windows.Forms.RadioButton();
            this.Solo_racialsGB = new System.Windows.Forms.GroupBox();
            this.Solo_min_torrent_mana_perc = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_stoneform = new System.Windows.Forms.NumericUpDown();
            this.Solo_min_gift_hp = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_torrent = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_stoneform = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_everymanforhimself = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_gift = new System.Windows.Forms.CheckBox();
            this.Solo_movementGB = new System.Windows.Forms.GroupBox();
            this.Solo_do_not_dismount_EVER = new System.Windows.Forms.CheckBox();
            this.Solo_do_not_dismount_ooc = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_move_to_HoJ = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_mount = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_move_to_heal = new System.Windows.Forms.CheckBox();
            this.Solo_generalGB = new System.Windows.Forms.GroupBox();
            this.Solo_rest_if_mana_belowLB = new System.Windows.Forms.Label();
            this.Solo_rest_if_mana_below = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_buff = new System.Windows.Forms.CheckBox();
            this.Solo_dpsGB = new System.Windows.Forms.GroupBox();
            this.Solo_wanna_Judge = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_CS = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_HoW = new System.Windows.Forms.CheckBox();
            this.Solo_ohshitbuttonGB = new System.Windows.Forms.GroupBox();
            this.Solo_wanna_lifeblood = new System.Windows.Forms.CheckBox();
            this.Solo_min_ohshitbutton_activatorLB = new System.Windows.Forms.Label();
            this.Solo_min_ohshitbutton_activator = new System.Windows.Forms.NumericUpDown();
            this.Solo_wanna_GotAK = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_AW = new System.Windows.Forms.CheckBox();
            this.Solo_wanna_DF = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.PVE_optimizeGB = new System.Windows.Forms.GroupBox();
            this.PVE_intellywait = new System.Windows.Forms.RadioButton();
            this.PVE_accurancy = new System.Windows.Forms.RadioButton();
            this.PVE_speed = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label80 = new System.Windows.Forms.Label();
            this.PVE_tank_healing_priority_multiplier = new System.Windows.Forms.NumericUpDown();
            this.label65 = new System.Windows.Forms.Label();
            this.PVE_healing_tank_priority = new System.Windows.Forms.NumericUpDown();
            this.PVE_bless_selection = new System.Windows.Forms.GroupBox();
            this.PVE_bless_type_disabledRB = new System.Windows.Forms.RadioButton();
            this.PVE_bless_type_MightRB = new System.Windows.Forms.RadioButton();
            this.PVE_bless_type_KingRB = new System.Windows.Forms.RadioButton();
            this.PVE_bless_type_autoRB = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.PVE_stop_DL_if_above = new System.Windows.Forms.NumericUpDown();
            this.PVE_tankselection = new System.Windows.Forms.GroupBox();
            this.PVE_get_tank_from_lua = new System.Windows.Forms.CheckBox();
            this.PVE_get_tank_from_focus = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PVE_advanced_option = new System.Windows.Forms.CheckBox();
            this.PVE_advanced = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PVE_max_healing_distance = new System.Windows.Forms.NumericUpDown();
            this.PVE_wanna_target = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.PVE_do_not_heal_above = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.PVE_wanna_face = new System.Windows.Forms.CheckBox();
            this.PVE_CleanseGB = new System.Windows.Forms.GroupBox();
            this.PVE_cleanse_only_self_and_tank = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_cleanse = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_urgent_cleanse = new System.Windows.Forms.CheckBox();
            this.PVE_interruptersGB = new System.Windows.Forms.GroupBox();
            this.PVE_wanna_HoJ = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_rebuke = new System.Windows.Forms.CheckBox();
            this.PVE_Healing = new System.Windows.Forms.GroupBox();
            this.PVE_min_Inf_of_light_DL_hpLB = new System.Windows.Forms.Label();
            this.PVE_min_Inf_of_light_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.PVE_min_HL_hp = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.PVE_min_FoL_hp = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.PVE_min_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.PVE_Inf_of_light_wanna_DL = new System.Windows.Forms.CheckBox();
            this.PVE_mana_management = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.PVE_use_mana_rec_trinket_every = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.PVE_min_mana_rec_trinket = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.PVE_min_Divine_Plea_mana = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.PVE_mana_judge = new System.Windows.Forms.NumericUpDown();
            this.PVE_HRGB = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.PVE_min_player_inside_HR = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.PVE_HR_how_much_health = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.PVE_HR_how_far = new System.Windows.Forms.NumericUpDown();
            this.PVE_wanna_HR = new System.Windows.Forms.CheckBox();
            this.PVE_emergencyGB = new System.Windows.Forms.GroupBox();
            this.PVE_min_mana_potion = new System.Windows.Forms.NumericUpDown();
            this.PVE_min_LoH_hp = new System.Windows.Forms.NumericUpDown();
            this.PVE_min_HoS_hp = new System.Windows.Forms.NumericUpDown();
            this.PVE_min_HoP_hp = new System.Windows.Forms.NumericUpDown();
            this.PVE_min_DS_hp = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.PVE_min_DP_hp = new System.Windows.Forms.NumericUpDown();
            this.PVE_wanna_mana_potion = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_LoH = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_DP = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_DS = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_HoS = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_HoP = new System.Windows.Forms.CheckBox();
            this.PVE_auraselctGB = new System.Windows.Forms.GroupBox();
            this.PVE_DisabledRB = new System.Windows.Forms.RadioButton();
            this.PVE_resistanceRB = new System.Windows.Forms.RadioButton();
            this.PVE_concentrationRB = new System.Windows.Forms.RadioButton();
            this.PVE_racialsGB = new System.Windows.Forms.GroupBox();
            this.PVE_min_torrent_mana_perc = new System.Windows.Forms.NumericUpDown();
            this.PVE_min_stoneform = new System.Windows.Forms.NumericUpDown();
            this.PVE_min_gift_hp = new System.Windows.Forms.NumericUpDown();
            this.PVE_wanna_torrent = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_stoneform = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_everymanforhimself = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_gift = new System.Windows.Forms.CheckBox();
            this.PVE_movementGB = new System.Windows.Forms.GroupBox();
            this.PVE_do_not_dismount_EVER = new System.Windows.Forms.CheckBox();
            this.PVE_do_not_dismount_ooc = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_move_to_HoJ = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_mount = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_move_to_heal = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_crusader = new System.Windows.Forms.CheckBox();
            this.PVE_generalGB = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.PVE_rest_if_mana_below = new System.Windows.Forms.NumericUpDown();
            this.PVE_wanna_buff = new System.Windows.Forms.CheckBox();
            this.PVE_dpsGB = new System.Windows.Forms.GroupBox();
            this.PVE_wanna_Judge = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_CS = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_HoW = new System.Windows.Forms.CheckBox();
            this.PVE_ohshitbuttonGB = new System.Windows.Forms.GroupBox();
            this.PVE_wanna_lifeblood = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.PVE_min_ohshitbutton_activator = new System.Windows.Forms.NumericUpDown();
            this.PVE_wanna_GotAK = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_AW = new System.Windows.Forms.CheckBox();
            this.PVE_wanna_DF = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Raid_optimizeGB = new System.Windows.Forms.GroupBox();
            this.Raid_intellywait = new System.Windows.Forms.RadioButton();
            this.Raid_accurancy = new System.Windows.Forms.RadioButton();
            this.Raid_speed = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label81 = new System.Windows.Forms.Label();
            this.Raid_tank_healing_priority_multiplier = new System.Windows.Forms.NumericUpDown();
            this.label79 = new System.Windows.Forms.Label();
            this.Raid_healing_tank_priority = new System.Windows.Forms.NumericUpDown();
            this.Raid_bless_selection = new System.Windows.Forms.GroupBox();
            this.Raid_bless_type_disabledRB = new System.Windows.Forms.RadioButton();
            this.Raid_bless_type_MightRB = new System.Windows.Forms.RadioButton();
            this.Raid_bless_type_KingRB = new System.Windows.Forms.RadioButton();
            this.Raid_bless_type_autoRB = new System.Windows.Forms.RadioButton();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.Raid_stop_DL_if_above = new System.Windows.Forms.NumericUpDown();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.Raid_get_tank_from_lua = new System.Windows.Forms.CheckBox();
            this.Raid_get_tank_from_focus = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Raid_advanced_option = new System.Windows.Forms.CheckBox();
            this.Raid_advanced = new System.Windows.Forms.GroupBox();
            this.label33 = new System.Windows.Forms.Label();
            this.Raid_max_healing_distance = new System.Windows.Forms.NumericUpDown();
            this.Raid_wanna_target = new System.Windows.Forms.CheckBox();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.Raid_do_not_heal_above = new System.Windows.Forms.NumericUpDown();
            this.label34 = new System.Windows.Forms.Label();
            this.Raid_wanna_face = new System.Windows.Forms.CheckBox();
            this.Raid_ignore_beacon = new System.Windows.Forms.CheckBox();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.Raid_cleanse_only_self_and_tank = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_cleanse = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_urgent_cleanse = new System.Windows.Forms.CheckBox();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.Raid_wanna_HoJ = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_rebuke = new System.Windows.Forms.CheckBox();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.Raid_min_Inf_of_light_DL_hpLB = new System.Windows.Forms.Label();
            this.Raid_min_Inf_of_light_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.label36 = new System.Windows.Forms.Label();
            this.Raid_min_HL_hp = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.Raid_min_FoL_hp = new System.Windows.Forms.NumericUpDown();
            this.label38 = new System.Windows.Forms.Label();
            this.Raid_min_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.Raid_Inf_of_light_wanna_DL = new System.Windows.Forms.CheckBox();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label39 = new System.Windows.Forms.Label();
            this.Raid_use_mana_rec_trinket_every = new System.Windows.Forms.NumericUpDown();
            this.label40 = new System.Windows.Forms.Label();
            this.Raid_min_mana_rec_trinket = new System.Windows.Forms.NumericUpDown();
            this.label41 = new System.Windows.Forms.Label();
            this.Raid_min_Divine_Plea_mana = new System.Windows.Forms.NumericUpDown();
            this.label42 = new System.Windows.Forms.Label();
            this.Raid_mana_judge = new System.Windows.Forms.NumericUpDown();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.Raid_min_player_inside_HR = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.Raid_HR_how_much_health = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.Raid_HR_how_far = new System.Windows.Forms.NumericUpDown();
            this.Raid_wanna_HR = new System.Windows.Forms.CheckBox();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.Raid_min_mana_potion = new System.Windows.Forms.NumericUpDown();
            this.Raid_min_LoH_hp = new System.Windows.Forms.NumericUpDown();
            this.Raid_min_HoS_hp = new System.Windows.Forms.NumericUpDown();
            this.Raid_min_HoP_hp = new System.Windows.Forms.NumericUpDown();
            this.Raid_min_DS_hp = new System.Windows.Forms.NumericUpDown();
            this.label46 = new System.Windows.Forms.Label();
            this.Raid_min_DP_hp = new System.Windows.Forms.NumericUpDown();
            this.Raid_wanna_mana_potion = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_LoH = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_DP = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_DS = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_HoS = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_HoP = new System.Windows.Forms.CheckBox();
            this.Raid_auraselctGB = new System.Windows.Forms.GroupBox();
            this.Raid_DisabledRB = new System.Windows.Forms.RadioButton();
            this.Raid_resistanceRB = new System.Windows.Forms.RadioButton();
            this.Raid_concentrationRB = new System.Windows.Forms.RadioButton();
            this.groupBox30 = new System.Windows.Forms.GroupBox();
            this.Raid_min_torrent_mana_perc = new System.Windows.Forms.NumericUpDown();
            this.Raid_min_stoneform = new System.Windows.Forms.NumericUpDown();
            this.Raid_min_gift_hp = new System.Windows.Forms.NumericUpDown();
            this.Raid_wanna_torrent = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_stoneform = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_everymanforhimself = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_gift = new System.Windows.Forms.CheckBox();
            this.groupBox31 = new System.Windows.Forms.GroupBox();
            this.Raid_do_not_dismount_EVER = new System.Windows.Forms.CheckBox();
            this.Raid_do_not_dismount_ooc = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_move_to_HoJ = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_mount = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_move_to_heal = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_crusader = new System.Windows.Forms.CheckBox();
            this.Raid_generalGB = new System.Windows.Forms.GroupBox();
            this.label47 = new System.Windows.Forms.Label();
            this.Raid_rest_if_mana_below = new System.Windows.Forms.NumericUpDown();
            this.Raid_wanna_buff = new System.Windows.Forms.CheckBox();
            this.groupBox33 = new System.Windows.Forms.GroupBox();
            this.Raid_wanna_Judge = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_CS = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_HoW = new System.Windows.Forms.CheckBox();
            this.groupBox34 = new System.Windows.Forms.GroupBox();
            this.Raid_wanna_lifeblood = new System.Windows.Forms.CheckBox();
            this.label48 = new System.Windows.Forms.Label();
            this.Raid_min_ohshitbutton_activator = new System.Windows.Forms.NumericUpDown();
            this.Raid_wanna_GotAK = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_AW = new System.Windows.Forms.CheckBox();
            this.Raid_wanna_DF = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.RAFtankfromfocus = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.Battleground_optimizeGB = new System.Windows.Forms.GroupBox();
            this.Battleground_intellywait = new System.Windows.Forms.RadioButton();
            this.Battleground_accurancy = new System.Windows.Forms.RadioButton();
            this.Battleground_speed = new System.Windows.Forms.RadioButton();
            this.Battleground_bless_selection = new System.Windows.Forms.GroupBox();
            this.Battleground_bless_type_disabledRB = new System.Windows.Forms.RadioButton();
            this.Battleground_bless_type_MightRB = new System.Windows.Forms.RadioButton();
            this.Battleground_bless_type_KingRB = new System.Windows.Forms.RadioButton();
            this.Battleground_bless_type_autoRB = new System.Windows.Forms.RadioButton();
            this.groupBox29 = new System.Windows.Forms.GroupBox();
            this.groupBox32 = new System.Windows.Forms.GroupBox();
            this.Battleground_get_tank_from_lua = new System.Windows.Forms.CheckBox();
            this.Battleground_get_tank_from_focus = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Battleground_advanced_option = new System.Windows.Forms.CheckBox();
            this.Battleground_advanced = new System.Windows.Forms.GroupBox();
            this.label49 = new System.Windows.Forms.Label();
            this.Battleground_max_healing_distance = new System.Windows.Forms.NumericUpDown();
            this.Battleground_wanna_target = new System.Windows.Forms.CheckBox();
            this.groupBox36 = new System.Windows.Forms.GroupBox();
            this.Battleground_do_not_heal_above = new System.Windows.Forms.NumericUpDown();
            this.label50 = new System.Windows.Forms.Label();
            this.Battleground_wanna_face = new System.Windows.Forms.CheckBox();
            this.groupBox37 = new System.Windows.Forms.GroupBox();
            this.Battleground_cleanse_only_self_and_tank = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_cleanse = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_urgent_cleanse = new System.Windows.Forms.CheckBox();
            this.groupBox38 = new System.Windows.Forms.GroupBox();
            this.Battleground_wanna_HoJ = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_rebuke = new System.Windows.Forms.CheckBox();
            this.groupBox39 = new System.Windows.Forms.GroupBox();
            this.Battleground_min_Inf_of_light_DL_hpLB = new System.Windows.Forms.Label();
            this.Battleground_min_Inf_of_light_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.label52 = new System.Windows.Forms.Label();
            this.Battleground_min_HL_hp = new System.Windows.Forms.NumericUpDown();
            this.label53 = new System.Windows.Forms.Label();
            this.Battleground_min_FoL_hp = new System.Windows.Forms.NumericUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.Battleground_min_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.Battleground_Inf_of_light_wanna_DL = new System.Windows.Forms.CheckBox();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.label55 = new System.Windows.Forms.Label();
            this.Battleground_use_mana_rec_trinket_every = new System.Windows.Forms.NumericUpDown();
            this.label56 = new System.Windows.Forms.Label();
            this.Battleground_min_mana_rec_trinket = new System.Windows.Forms.NumericUpDown();
            this.label57 = new System.Windows.Forms.Label();
            this.Battleground_min_Divine_Plea_mana = new System.Windows.Forms.NumericUpDown();
            this.label58 = new System.Windows.Forms.Label();
            this.Battleground_mana_judge = new System.Windows.Forms.NumericUpDown();
            this.groupBox41 = new System.Windows.Forms.GroupBox();
            this.label59 = new System.Windows.Forms.Label();
            this.Battleground_min_player_inside_HR = new System.Windows.Forms.NumericUpDown();
            this.label60 = new System.Windows.Forms.Label();
            this.Battleground_HR_how_much_health = new System.Windows.Forms.NumericUpDown();
            this.label61 = new System.Windows.Forms.Label();
            this.Battleground_HR_how_far = new System.Windows.Forms.NumericUpDown();
            this.Battleground_wanna_HR = new System.Windows.Forms.CheckBox();
            this.groupBox42 = new System.Windows.Forms.GroupBox();
            this.Battleground_min_mana_potion = new System.Windows.Forms.NumericUpDown();
            this.Battleground_min_LoH_hp = new System.Windows.Forms.NumericUpDown();
            this.Battleground_min_HoS_hp = new System.Windows.Forms.NumericUpDown();
            this.Battleground_min_HoP_hp = new System.Windows.Forms.NumericUpDown();
            this.Battleground_min_DS_hp = new System.Windows.Forms.NumericUpDown();
            this.label62 = new System.Windows.Forms.Label();
            this.Battleground_min_DP_hp = new System.Windows.Forms.NumericUpDown();
            this.Battleground_wanna_mana_potion = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_LoH = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_DP = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_DS = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_HoS = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_HoP = new System.Windows.Forms.CheckBox();
            this.Battleground_auraselctGB = new System.Windows.Forms.GroupBox();
            this.Battleground_DisabledRB = new System.Windows.Forms.RadioButton();
            this.Battleground_resistanceRB = new System.Windows.Forms.RadioButton();
            this.Battleground_concentrationRB = new System.Windows.Forms.RadioButton();
            this.groupBox44 = new System.Windows.Forms.GroupBox();
            this.Battleground_min_torrent_mana_perc = new System.Windows.Forms.NumericUpDown();
            this.Battleground_min_stoneform = new System.Windows.Forms.NumericUpDown();
            this.Battleground_min_gift_hp = new System.Windows.Forms.NumericUpDown();
            this.Battleground_wanna_torrent = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_stoneform = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_everymanforhimself = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_gift = new System.Windows.Forms.CheckBox();
            this.groupBox45 = new System.Windows.Forms.GroupBox();
            this.Battleground_do_not_dismount_EVER = new System.Windows.Forms.CheckBox();
            this.Battleground_do_not_dismount_ooc = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_move_to_HoJ = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_mount = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_move_to_heal = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_crusader = new System.Windows.Forms.CheckBox();
            this.Battleground_generalGB = new System.Windows.Forms.GroupBox();
            this.label63 = new System.Windows.Forms.Label();
            this.Battleground_rest_if_mana_below = new System.Windows.Forms.NumericUpDown();
            this.Battleground_wanna_buff = new System.Windows.Forms.CheckBox();
            this.groupBox47 = new System.Windows.Forms.GroupBox();
            this.Battleground_wanna_Judge = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_CS = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_HoW = new System.Windows.Forms.CheckBox();
            this.groupBox48 = new System.Windows.Forms.GroupBox();
            this.Battleground_wanna_lifeblood = new System.Windows.Forms.CheckBox();
            this.label64 = new System.Windows.Forms.Label();
            this.Battleground_min_ohshitbutton_activator = new System.Windows.Forms.NumericUpDown();
            this.Battleground_wanna_GotAK = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_AW = new System.Windows.Forms.CheckBox();
            this.Battleground_wanna_DF = new System.Windows.Forms.CheckBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.ARENA_optimizeGB = new System.Windows.Forms.GroupBox();
            this.ARENA_intellywait = new System.Windows.Forms.RadioButton();
            this.ARENA_accurancy = new System.Windows.Forms.RadioButton();
            this.ARENA_speed = new System.Windows.Forms.RadioButton();
            this.ARENA_bless_selection = new System.Windows.Forms.GroupBox();
            this.ARENA_bless_type_disabledRB = new System.Windows.Forms.RadioButton();
            this.ARENA_bless_type_MightRB = new System.Windows.Forms.RadioButton();
            this.ARENA_bless_type_KingRB = new System.Windows.Forms.RadioButton();
            this.ARENA_bless_type_autoRB = new System.Windows.Forms.RadioButton();
            this.ARENA_special_logics = new System.Windows.Forms.GroupBox();
            this.ARENA_wanna_HoF = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_taunt = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ARENA_get_tank_from_lua = new System.Windows.Forms.CheckBox();
            this.ARENA_get_tank_from_focus = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ARENA_advanced_option = new System.Windows.Forms.CheckBox();
            this.ARENA_advanced = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ARENA_max_healing_distance = new System.Windows.Forms.NumericUpDown();
            this.ARENA_wanna_target = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.ARENA_do_not_heal_above = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.ARENA_wanna_face = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.ARENA_cleanse_only_self_and_tank = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_cleanse = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_urgent_cleanse = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.ARENA_wanna_HoJ = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_rebuke = new System.Windows.Forms.CheckBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.ARENA_min_Inf_of_light_DL_hpLB = new System.Windows.Forms.Label();
            this.ARENA_min_Inf_of_light_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.ARENA_min_HL_hp = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.ARENA_min_FoL_hp = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.ARENA_min_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.ARENA_Inf_of_light_wanna_DL = new System.Windows.Forms.CheckBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.ARENA_use_mana_rec_trinket_every = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.ARENA_min_mana_rec_trinket = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.ARENA_min_Divine_Plea_mana = new System.Windows.Forms.NumericUpDown();
            this.label26 = new System.Windows.Forms.Label();
            this.ARENA_mana_judge = new System.Windows.Forms.NumericUpDown();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.ARENA_min_player_inside_HR = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.ARENA_HR_how_much_health = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.ARENA_HR_how_far = new System.Windows.Forms.NumericUpDown();
            this.ARENA_wanna_HR = new System.Windows.Forms.CheckBox();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.ARENA_min_mana_potion = new System.Windows.Forms.NumericUpDown();
            this.ARENA_min_LoH_hp = new System.Windows.Forms.NumericUpDown();
            this.ARENA_min_HoS_hp = new System.Windows.Forms.NumericUpDown();
            this.ARENA_min_HoP_hp = new System.Windows.Forms.NumericUpDown();
            this.ARENA_min_DS_hp = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.ARENA_min_DP_hp = new System.Windows.Forms.NumericUpDown();
            this.ARENA_wanna_mana_potion = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_LoH = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_DP = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_DS = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_HoS = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_HoP = new System.Windows.Forms.CheckBox();
            this.ARENA_auraselctGB = new System.Windows.Forms.GroupBox();
            this.ARENA_DisabledRB = new System.Windows.Forms.RadioButton();
            this.ARENA_resistanceRB = new System.Windows.Forms.RadioButton();
            this.ARENA_concentrationRB = new System.Windows.Forms.RadioButton();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.ARENA_min_torrent_mana_perc = new System.Windows.Forms.NumericUpDown();
            this.ARENA_min_stoneform = new System.Windows.Forms.NumericUpDown();
            this.ARENA_min_gift_hp = new System.Windows.Forms.NumericUpDown();
            this.ARENA_wanna_torrent = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_stoneform = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_everymanforhimself = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_gift = new System.Windows.Forms.CheckBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.ARENA_do_not_dismount_EVER = new System.Windows.Forms.CheckBox();
            this.ARENA_do_not_dismount_ooc = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_move_to_HoJ = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_mount = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_move_to_heal = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_crusader = new System.Windows.Forms.CheckBox();
            this.Arena_generalGB = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.ARENA_rest_if_mana_below = new System.Windows.Forms.NumericUpDown();
            this.ARENA_wanna_buff = new System.Windows.Forms.CheckBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.ARENA_wanna_Judge = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_CS = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_HoW = new System.Windows.Forms.CheckBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.ARENA_wanna_lifeblood = new System.Windows.Forms.CheckBox();
            this.label32 = new System.Windows.Forms.Label();
            this.ARENA_min_ohshitbutton_activator = new System.Windows.Forms.NumericUpDown();
            this.ARENA_wanna_GotAK = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_AW = new System.Windows.Forms.CheckBox();
            this.ARENA_wanna_DF = new System.Windows.Forms.CheckBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.ARENA2v2tankfromfocus = new System.Windows.Forms.CheckBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.WorldPVP_optimizeGB = new System.Windows.Forms.GroupBox();
            this.WorldPVP_intellywait = new System.Windows.Forms.RadioButton();
            this.WorldPVP_accurancy = new System.Windows.Forms.RadioButton();
            this.WorldPVP_speed = new System.Windows.Forms.RadioButton();
            this.WorldPVP_bless_selection = new System.Windows.Forms.GroupBox();
            this.WorldPVP_bless_type_disabledRB = new System.Windows.Forms.RadioButton();
            this.WorldPVP_bless_type_MightRB = new System.Windows.Forms.RadioButton();
            this.WorldPVP_bless_type_KingRB = new System.Windows.Forms.RadioButton();
            this.WorldPVP_bless_type_autoRB = new System.Windows.Forms.RadioButton();
            this.groupBox43 = new System.Windows.Forms.GroupBox();
            this.groupBox46 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_get_tank_from_lua = new System.Windows.Forms.CheckBox();
            this.WorldPVP_get_tank_from_focus = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.WorldPVP_advanced_option = new System.Windows.Forms.CheckBox();
            this.WorldPVP_advanced = new System.Windows.Forms.GroupBox();
            this.label35 = new System.Windows.Forms.Label();
            this.WorldPVP_max_healing_distance = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_wanna_target = new System.Windows.Forms.CheckBox();
            this.groupBox50 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_do_not_heal_above = new System.Windows.Forms.NumericUpDown();
            this.label51 = new System.Windows.Forms.Label();
            this.WorldPVP_wanna_face = new System.Windows.Forms.CheckBox();
            this.groupBox51 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_cleanse_only_self_and_tank = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_cleanse = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_urgent_cleanse = new System.Windows.Forms.CheckBox();
            this.groupBox52 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_wanna_HoJ = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_rebuke = new System.Windows.Forms.CheckBox();
            this.groupBox53 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_min_Inf_of_light_DL_hpLB = new System.Windows.Forms.Label();
            this.WorldPVP_min_Inf_of_light_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.label66 = new System.Windows.Forms.Label();
            this.WorldPVP_min_HL_hp = new System.Windows.Forms.NumericUpDown();
            this.label67 = new System.Windows.Forms.Label();
            this.WorldPVP_min_FoL_hp = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.WorldPVP_min_DL_hp = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_Inf_of_light_wanna_DL = new System.Windows.Forms.CheckBox();
            this.groupBox54 = new System.Windows.Forms.GroupBox();
            this.label69 = new System.Windows.Forms.Label();
            this.WorldPVP_use_mana_rec_trinket_every = new System.Windows.Forms.NumericUpDown();
            this.label70 = new System.Windows.Forms.Label();
            this.WorldPVP_min_mana_rec_trinket = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.WorldPVP_min_Divine_Plea_mana = new System.Windows.Forms.NumericUpDown();
            this.label72 = new System.Windows.Forms.Label();
            this.WorldPVP_mana_judge = new System.Windows.Forms.NumericUpDown();
            this.groupBox55 = new System.Windows.Forms.GroupBox();
            this.label73 = new System.Windows.Forms.Label();
            this.WorldPVP_min_player_inside_HR = new System.Windows.Forms.NumericUpDown();
            this.label74 = new System.Windows.Forms.Label();
            this.WorldPVP_HR_how_much_health = new System.Windows.Forms.NumericUpDown();
            this.label75 = new System.Windows.Forms.Label();
            this.WorldPVP_HR_how_far = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_wanna_HR = new System.Windows.Forms.CheckBox();
            this.groupBox56 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_min_mana_potion = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_min_LoH_hp = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_min_HoS_hp = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_min_HoP_hp = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_min_DS_hp = new System.Windows.Forms.NumericUpDown();
            this.label76 = new System.Windows.Forms.Label();
            this.WorldPVP_min_DP_hp = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_wanna_mana_potion = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_LoH = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_DP = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_DS = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_HoS = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_HoP = new System.Windows.Forms.CheckBox();
            this.WorldPVP_auraselctGB = new System.Windows.Forms.GroupBox();
            this.WorldPVP_DisabledRB = new System.Windows.Forms.RadioButton();
            this.WorldPVP_resistanceRB = new System.Windows.Forms.RadioButton();
            this.WorldPVP_concentrationRB = new System.Windows.Forms.RadioButton();
            this.groupBox58 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_min_torrent_mana_perc = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_min_stoneform = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_min_gift_hp = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_wanna_torrent = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_stoneform = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_everymanforhimself = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_gift = new System.Windows.Forms.CheckBox();
            this.groupBox59 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_do_not_dismount_EVER = new System.Windows.Forms.CheckBox();
            this.WorldPVP_do_not_dismount_ooc = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_move_to_HoJ = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_mount = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_move_to_heal = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_crusader = new System.Windows.Forms.CheckBox();
            this.WorldPVP_generalGB = new System.Windows.Forms.GroupBox();
            this.label77 = new System.Windows.Forms.Label();
            this.WorldPVP_rest_if_mana_below = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_wanna_buff = new System.Windows.Forms.CheckBox();
            this.groupBox61 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_wanna_Judge = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_CS = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_HoW = new System.Windows.Forms.CheckBox();
            this.groupBox62 = new System.Windows.Forms.GroupBox();
            this.WorldPVP_wanna_lifeblood = new System.Windows.Forms.CheckBox();
            this.label78 = new System.Windows.Forms.Label();
            this.WorldPVP_min_ohshitbutton_activator = new System.Windows.Forms.NumericUpDown();
            this.WorldPVP_wanna_GotAK = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_AW = new System.Windows.Forms.CheckBox();
            this.WorldPVP_wanna_DF = new System.Windows.Forms.CheckBox();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.SH_GB8 = new System.Windows.Forms.GroupBox();
            this.RaidL35 = new System.Windows.Forms.Label();
            this.RaidL36 = new System.Windows.Forms.Label();
            this.RaidL37 = new System.Windows.Forms.Label();
            this.RaidL38 = new System.Windows.Forms.Label();
            this.RaidL39 = new System.Windows.Forms.Label();
            this.RaidCK35 = new System.Windows.Forms.CheckBox();
            this.RaidCK39 = new System.Windows.Forms.CheckBox();
            this.RaidCK36 = new System.Windows.Forms.CheckBox();
            this.RaidCK38 = new System.Windows.Forms.CheckBox();
            this.RaidCK37 = new System.Windows.Forms.CheckBox();
            this.SH_GB7 = new System.Windows.Forms.GroupBox();
            this.RaidL30 = new System.Windows.Forms.Label();
            this.RaidL31 = new System.Windows.Forms.Label();
            this.RaidL32 = new System.Windows.Forms.Label();
            this.RaidL33 = new System.Windows.Forms.Label();
            this.RaidL34 = new System.Windows.Forms.Label();
            this.RaidCK30 = new System.Windows.Forms.CheckBox();
            this.RaidCK34 = new System.Windows.Forms.CheckBox();
            this.RaidCK31 = new System.Windows.Forms.CheckBox();
            this.RaidCK33 = new System.Windows.Forms.CheckBox();
            this.RaidCK32 = new System.Windows.Forms.CheckBox();
            this.SH_GB6 = new System.Windows.Forms.GroupBox();
            this.RaidL25 = new System.Windows.Forms.Label();
            this.RaidL26 = new System.Windows.Forms.Label();
            this.RaidL27 = new System.Windows.Forms.Label();
            this.RaidL28 = new System.Windows.Forms.Label();
            this.RaidL29 = new System.Windows.Forms.Label();
            this.RaidCK25 = new System.Windows.Forms.CheckBox();
            this.RaidCK29 = new System.Windows.Forms.CheckBox();
            this.RaidCK26 = new System.Windows.Forms.CheckBox();
            this.RaidCK28 = new System.Windows.Forms.CheckBox();
            this.RaidCK27 = new System.Windows.Forms.CheckBox();
            this.SE_GB5 = new System.Windows.Forms.GroupBox();
            this.RaidL20 = new System.Windows.Forms.Label();
            this.RaidL21 = new System.Windows.Forms.Label();
            this.RaidL22 = new System.Windows.Forms.Label();
            this.RaidL23 = new System.Windows.Forms.Label();
            this.RaidL24 = new System.Windows.Forms.Label();
            this.RaidCK20 = new System.Windows.Forms.CheckBox();
            this.RaidCK24 = new System.Windows.Forms.CheckBox();
            this.RaidCK21 = new System.Windows.Forms.CheckBox();
            this.RaidCK23 = new System.Windows.Forms.CheckBox();
            this.RaidCK22 = new System.Windows.Forms.CheckBox();
            this.SE_GB4 = new System.Windows.Forms.GroupBox();
            this.RaidL15 = new System.Windows.Forms.Label();
            this.RaidL16 = new System.Windows.Forms.Label();
            this.RaidL17 = new System.Windows.Forms.Label();
            this.RaidL18 = new System.Windows.Forms.Label();
            this.RaidL19 = new System.Windows.Forms.Label();
            this.RaidCK15 = new System.Windows.Forms.CheckBox();
            this.RaidCK16 = new System.Windows.Forms.CheckBox();
            this.RaidCK17 = new System.Windows.Forms.CheckBox();
            this.RaidCK18 = new System.Windows.Forms.CheckBox();
            this.RaidCK19 = new System.Windows.Forms.CheckBox();
            this.SE_GB3 = new System.Windows.Forms.GroupBox();
            this.RaidL10 = new System.Windows.Forms.Label();
            this.RaidL11 = new System.Windows.Forms.Label();
            this.RaidL12 = new System.Windows.Forms.Label();
            this.RaidL13 = new System.Windows.Forms.Label();
            this.RaidL14 = new System.Windows.Forms.Label();
            this.RaidCK10 = new System.Windows.Forms.CheckBox();
            this.RaidCK11 = new System.Windows.Forms.CheckBox();
            this.RaidCK12 = new System.Windows.Forms.CheckBox();
            this.RaidCK13 = new System.Windows.Forms.CheckBox();
            this.RaidCK14 = new System.Windows.Forms.CheckBox();
            this.SE_GB2 = new System.Windows.Forms.GroupBox();
            this.RaidCK9 = new System.Windows.Forms.CheckBox();
            this.RaidL5 = new System.Windows.Forms.Label();
            this.RaidCK8 = new System.Windows.Forms.CheckBox();
            this.RaidL6 = new System.Windows.Forms.Label();
            this.RaidCK7 = new System.Windows.Forms.CheckBox();
            this.RaidL7 = new System.Windows.Forms.Label();
            this.RaidCK6 = new System.Windows.Forms.CheckBox();
            this.RaidL8 = new System.Windows.Forms.Label();
            this.RaidCK5 = new System.Windows.Forms.CheckBox();
            this.RaidL9 = new System.Windows.Forms.Label();
            this.SH_GB1 = new System.Windows.Forms.GroupBox();
            this.RaidCK0 = new System.Windows.Forms.CheckBox();
            this.RaidCK1 = new System.Windows.Forms.CheckBox();
            this.RaidCK2 = new System.Windows.Forms.CheckBox();
            this.RaidCK3 = new System.Windows.Forms.CheckBox();
            this.RaidCK4 = new System.Windows.Forms.CheckBox();
            this.RaidL0 = new System.Windows.Forms.Label();
            this.RaidL1 = new System.Windows.Forms.Label();
            this.RaidL2 = new System.Windows.Forms.Label();
            this.RaidL3 = new System.Windows.Forms.Label();
            this.RaidL4 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SelectHeal = new System.Windows.Forms.CheckBox();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.groupBox35 = new System.Windows.Forms.GroupBox();
            this.PVP_dispell_ASAP_TB20 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB19 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB18 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB17 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB16 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB15 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB14 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB13 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB12 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB11 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB10 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB9 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB8 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB7 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB6 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB5 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB4 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB3 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB2 = new System.Windows.Forms.TextBox();
            this.PVP_dispell_ASAP_TB1 = new System.Windows.Forms.TextBox();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.PVP_do_not_touch_TB20 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB19 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB18 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB17 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB16 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB15 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB14 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB13 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB12 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB11 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB10 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB9 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB8 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB7 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB6 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB5 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB4 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB3 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB2 = new System.Windows.Forms.TextBox();
            this.PVP_do_not_touch_TB1 = new System.Windows.Forms.TextBox();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.groupBox49 = new System.Windows.Forms.GroupBox();
            this.PVE_dispell_ASAP_TB20 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB19 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB18 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB17 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB16 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB15 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB14 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB13 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB12 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB11 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB10 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB9 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB8 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB7 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB6 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB5 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB4 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB3 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB2 = new System.Windows.Forms.TextBox();
            this.PVE_dispell_ASAP_TB1 = new System.Windows.Forms.TextBox();
            this.groupBox57 = new System.Windows.Forms.GroupBox();
            this.PVE_do_not_touch_TB20 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB19 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB18 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB17 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB16 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB15 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB14 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB13 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB12 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB11 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB10 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB9 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB8 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB7 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB6 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB5 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB4 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB3 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB2 = new System.Windows.Forms.TextBox();
            this.PVE_do_not_touch_TB1 = new System.Windows.Forms.TextBox();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.groupBox63 = new System.Windows.Forms.GroupBox();
            this.Trinket2_use_when_GB = new System.Windows.Forms.GroupBox();
            this.Trinket2_PVP = new System.Windows.Forms.RadioButton();
            this.Trinket2_mana = new System.Windows.Forms.RadioButton();
            this.Trinket2_HPS = new System.Windows.Forms.RadioButton();
            this.Trinket2_never = new System.Windows.Forms.RadioButton();
            this.Trinket2_name_LB = new System.Windows.Forms.Label();
            this.Trinket2_CD_LB = new System.Windows.Forms.Label();
            this.Trinket2_ID_LB = new System.Windows.Forms.Label();
            this.groupBox60 = new System.Windows.Forms.GroupBox();
            this.Trinket1_use_when_GB = new System.Windows.Forms.GroupBox();
            this.Trinket1_PVP = new System.Windows.Forms.RadioButton();
            this.Trinket1_mana = new System.Windows.Forms.RadioButton();
            this.Trinket1_HPS = new System.Windows.Forms.RadioButton();
            this.Trinket1_never = new System.Windows.Forms.RadioButton();
            this.Trinket1_name_LB = new System.Windows.Forms.Label();
            this.Trinket1_CD_LB = new System.Windows.Forms.Label();
            this.Trinket1_ID_LB = new System.Windows.Forms.Label();
            this.Trnket1_name_load = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.General_precasting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox64.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_exorcism_min_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_holy_wrath_min_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_consecration_min_mana)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.Solo_optimizeGB.SuspendLayout();
            this.Solo_bless_selection.SuspendLayout();
            this.Solo_tankselection.SuspendLayout();
            this.Solo_adancePanel.SuspendLayout();
            this.Solo_advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_max_healing_distance)).BeginInit();
            this.Solo_donothealaboveGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_do_not_heal_above)).BeginInit();
            this.Solo_CleanseGB.SuspendLayout();
            this.Solo_interruptersGB.SuspendLayout();
            this.Solo_Healing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_Inf_of_light_DL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_HL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_FoL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_DL_hp)).BeginInit();
            this.Solo_mana_management.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_use_mana_rec_trinket_every)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_mana_rec_trinket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_Divine_Plea_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_mana_judge)).BeginInit();
            this.Solo_HRGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_player_inside_HR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_HR_how_much_health)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_HR_how_far)).BeginInit();
            this.Solo_emergencyGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_mana_potion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_LoH_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_HoS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_HoP_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_DS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_DP_hp)).BeginInit();
            this.Solo_auraselctGB.SuspendLayout();
            this.Solo_racialsGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_torrent_mana_perc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_stoneform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_gift_hp)).BeginInit();
            this.Solo_movementGB.SuspendLayout();
            this.Solo_generalGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_rest_if_mana_below)).BeginInit();
            this.Solo_dpsGB.SuspendLayout();
            this.Solo_ohshitbuttonGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_ohshitbutton_activator)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.PVE_optimizeGB.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_tank_healing_priority_multiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_healing_tank_priority)).BeginInit();
            this.PVE_bless_selection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_stop_DL_if_above)).BeginInit();
            this.PVE_tankselection.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PVE_advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_max_healing_distance)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_do_not_heal_above)).BeginInit();
            this.PVE_CleanseGB.SuspendLayout();
            this.PVE_interruptersGB.SuspendLayout();
            this.PVE_Healing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_Inf_of_light_DL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_HL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_FoL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_DL_hp)).BeginInit();
            this.PVE_mana_management.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_use_mana_rec_trinket_every)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_mana_rec_trinket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_Divine_Plea_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_mana_judge)).BeginInit();
            this.PVE_HRGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_player_inside_HR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_HR_how_much_health)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_HR_how_far)).BeginInit();
            this.PVE_emergencyGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_mana_potion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_LoH_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_HoS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_HoP_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_DS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_DP_hp)).BeginInit();
            this.PVE_auraselctGB.SuspendLayout();
            this.PVE_racialsGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_torrent_mana_perc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_stoneform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_gift_hp)).BeginInit();
            this.PVE_movementGB.SuspendLayout();
            this.PVE_generalGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_rest_if_mana_below)).BeginInit();
            this.PVE_dpsGB.SuspendLayout();
            this.PVE_ohshitbuttonGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_ohshitbutton_activator)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.Raid_optimizeGB.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_tank_healing_priority_multiplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_healing_tank_priority)).BeginInit();
            this.Raid_bless_selection.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_stop_DL_if_above)).BeginInit();
            this.groupBox18.SuspendLayout();
            this.panel3.SuspendLayout();
            this.Raid_advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_max_healing_distance)).BeginInit();
            this.groupBox22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_do_not_heal_above)).BeginInit();
            this.groupBox23.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.groupBox25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_Inf_of_light_DL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_HL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_FoL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_DL_hp)).BeginInit();
            this.groupBox26.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_use_mana_rec_trinket_every)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_mana_rec_trinket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_Divine_Plea_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_mana_judge)).BeginInit();
            this.groupBox27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_player_inside_HR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_HR_how_much_health)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_HR_how_far)).BeginInit();
            this.groupBox28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_mana_potion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_LoH_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_HoS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_HoP_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_DS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_DP_hp)).BeginInit();
            this.Raid_auraselctGB.SuspendLayout();
            this.groupBox30.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_torrent_mana_perc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_stoneform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_gift_hp)).BeginInit();
            this.groupBox31.SuspendLayout();
            this.Raid_generalGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_rest_if_mana_below)).BeginInit();
            this.groupBox33.SuspendLayout();
            this.groupBox34.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_ohshitbutton_activator)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.Battleground_optimizeGB.SuspendLayout();
            this.Battleground_bless_selection.SuspendLayout();
            this.groupBox32.SuspendLayout();
            this.panel4.SuspendLayout();
            this.Battleground_advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_max_healing_distance)).BeginInit();
            this.groupBox36.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_do_not_heal_above)).BeginInit();
            this.groupBox37.SuspendLayout();
            this.groupBox38.SuspendLayout();
            this.groupBox39.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_Inf_of_light_DL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_HL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_FoL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_DL_hp)).BeginInit();
            this.groupBox40.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_use_mana_rec_trinket_every)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_mana_rec_trinket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_Divine_Plea_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_mana_judge)).BeginInit();
            this.groupBox41.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_player_inside_HR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_HR_how_much_health)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_HR_how_far)).BeginInit();
            this.groupBox42.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_mana_potion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_LoH_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_HoS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_HoP_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_DS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_DP_hp)).BeginInit();
            this.Battleground_auraselctGB.SuspendLayout();
            this.groupBox44.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_torrent_mana_perc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_stoneform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_gift_hp)).BeginInit();
            this.groupBox45.SuspendLayout();
            this.Battleground_generalGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_rest_if_mana_below)).BeginInit();
            this.groupBox47.SuspendLayout();
            this.groupBox48.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_ohshitbutton_activator)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.ARENA_optimizeGB.SuspendLayout();
            this.ARENA_bless_selection.SuspendLayout();
            this.ARENA_special_logics.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.ARENA_advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_max_healing_distance)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_do_not_heal_above)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_Inf_of_light_DL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_HL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_FoL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_DL_hp)).BeginInit();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_use_mana_rec_trinket_every)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_mana_rec_trinket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_Divine_Plea_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_mana_judge)).BeginInit();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_player_inside_HR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_HR_how_much_health)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_HR_how_far)).BeginInit();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_mana_potion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_LoH_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_HoS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_HoP_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_DS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_DP_hp)).BeginInit();
            this.ARENA_auraselctGB.SuspendLayout();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_torrent_mana_perc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_stoneform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_gift_hp)).BeginInit();
            this.groupBox17.SuspendLayout();
            this.Arena_generalGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_rest_if_mana_below)).BeginInit();
            this.groupBox19.SuspendLayout();
            this.groupBox20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_ohshitbutton_activator)).BeginInit();
            this.tabPage7.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.WorldPVP_optimizeGB.SuspendLayout();
            this.WorldPVP_bless_selection.SuspendLayout();
            this.groupBox46.SuspendLayout();
            this.panel5.SuspendLayout();
            this.WorldPVP_advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_max_healing_distance)).BeginInit();
            this.groupBox50.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_do_not_heal_above)).BeginInit();
            this.groupBox51.SuspendLayout();
            this.groupBox52.SuspendLayout();
            this.groupBox53.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_Inf_of_light_DL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_HL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_FoL_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_DL_hp)).BeginInit();
            this.groupBox54.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_use_mana_rec_trinket_every)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_mana_rec_trinket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_Divine_Plea_mana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_mana_judge)).BeginInit();
            this.groupBox55.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_player_inside_HR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_HR_how_much_health)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_HR_how_far)).BeginInit();
            this.groupBox56.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_mana_potion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_LoH_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_HoS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_HoP_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_DS_hp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_DP_hp)).BeginInit();
            this.WorldPVP_auraselctGB.SuspendLayout();
            this.groupBox58.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_torrent_mana_perc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_stoneform)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_gift_hp)).BeginInit();
            this.groupBox59.SuspendLayout();
            this.WorldPVP_generalGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_rest_if_mana_below)).BeginInit();
            this.groupBox61.SuspendLayout();
            this.groupBox62.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_ohshitbutton_activator)).BeginInit();
            this.tabPage9.SuspendLayout();
            this.SH_GB8.SuspendLayout();
            this.SH_GB7.SuspendLayout();
            this.SH_GB6.SuspendLayout();
            this.SE_GB5.SuspendLayout();
            this.SE_GB4.SuspendLayout();
            this.SE_GB3.SuspendLayout();
            this.SE_GB2.SuspendLayout();
            this.SH_GB1.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.groupBox35.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.groupBox49.SuspendLayout();
            this.groupBox57.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.groupBox63.SuspendLayout();
            this.Trinket2_use_when_GB.SuspendLayout();
            this.groupBox60.SuspendLayout();
            this.Trinket1_use_when_GB.SuspendLayout();
            this.SuspendLayout();
            // 
            // Solo_wanna_crusader
            // 
            this.Solo_wanna_crusader.AutoSize = true;
            this.Solo_wanna_crusader.Location = new System.Drawing.Point(6, 89);
            this.Solo_wanna_crusader.Name = "Solo_wanna_crusader";
            this.Solo_wanna_crusader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Solo_wanna_crusader.Size = new System.Drawing.Size(213, 17);
            this.Solo_wanna_crusader.TabIndex = 0;
            this.Solo_wanna_crusader.Text = "Switch to Crusader Aura when mounted";
            this.Solo_wanna_crusader.UseVisualStyleBackColor = true;
            this.Solo_wanna_crusader.CheckedChanged += new System.EventHandler(this.Solo_wanna_crusader_CheckedChanged);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(936, 697);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 1;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click_1);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage14);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage8);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Controls.Add(this.tabPage13);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1002, 679);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage14
            // 
            this.tabPage14.AutoScroll = true;
            this.tabPage14.Controls.Add(this.cbLogging);
            this.tabPage14.Controls.Add(this.label83);
            this.tabPage14.Controls.Add(this.General_precasting);
            this.tabPage14.Controls.Add(this.Stop_Healing_BT);
            this.tabPage14.Controls.Add(this.label82);
            this.tabPage14.Controls.Add(this.pictureBox1);
            this.tabPage14.Location = new System.Drawing.Point(4, 22);
            this.tabPage14.Name = "tabPage14";
            this.tabPage14.Size = new System.Drawing.Size(994, 653);
            this.tabPage14.TabIndex = 11;
            this.tabPage14.Text = "Welcome";
            this.tabPage14.UseVisualStyleBackColor = true;
            // 
            // cbLogging
            // 
            this.cbLogging.AutoSize = true;
            this.cbLogging.Location = new System.Drawing.Point(261, 483);
            this.cbLogging.Name = "cbLogging";
            this.cbLogging.Size = new System.Drawing.Size(100, 17);
            this.cbLogging.TabIndex = 5;
            this.cbLogging.Text = "Enable Logging";
            this.cbLogging.UseVisualStyleBackColor = true;
            this.cbLogging.CheckedChanged += new System.EventHandler(this.cbLogging_CheckedChanged);
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.Location = new System.Drawing.Point(280, 512);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(603, 13);
            this.label83.TabIndex = 4;
            this.label83.Text = "X10 Millisends of precasting, will also automatically factor Lag, use at your own" +
                " risk, WILL ONLY WORK WITH INTELLYWAIT";
            // 
            // General_precasting
            // 
            this.General_precasting.Location = new System.Drawing.Point(219, 506);
            this.General_precasting.Name = "General_precasting";
            this.General_precasting.Size = new System.Drawing.Size(54, 20);
            this.General_precasting.TabIndex = 3;
            this.General_precasting.ValueChanged += new System.EventHandler(this.General_precasting_ValueChanged);
            // 
            // Stop_Healing_BT
            // 
            this.Stop_Healing_BT.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Stop_Healing_BT.ForeColor = System.Drawing.Color.Red;
            this.Stop_Healing_BT.Location = new System.Drawing.Point(210, 546);
            this.Stop_Healing_BT.Name = "Stop_Healing_BT";
            this.Stop_Healing_BT.Size = new System.Drawing.Size(542, 89);
            this.Stop_Healing_BT.TabIndex = 2;
            this.Stop_Healing_BT.Text = "Stop ALL Healing";
            this.Stop_Healing_BT.UseVisualStyleBackColor = true;
            this.Stop_Healing_BT.Click += new System.EventHandler(this.Stop_Healing_BT_Click);
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Location = new System.Drawing.Point(17, 258);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(878, 13);
            this.label82.TabIndex = 1;
            this.label82.Text = "Welcome! Use the tabs above this to configure every aspect of your healing, remem" +
                "ber, settings will work only inside the relevant activity (Dungeon setting will " +
                "only work in dungeon ecc)";
            // 
            // pictureBox1
            // 
            //this.pictureBox1.Image =
            this.pictureBox1.ImageLocation = Logging.ApplicationPath + @"\CustomClasses\UltimatePalaHealerBT\Resources\paladin-CC.png"; // global::UltimatePalaHealerBT.Properties.Resources.paladin_CC;
            //this.pictureBox1.InitialImage = global::UltimatePalaHealerBT.Properties.Resources.paladin_CC;
            this.pictureBox1.Location = new System.Drawing.Point(219, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(507, 200);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // tabPage1
            // 
            this.tabPage1.AutoScroll = true;
            this.tabPage1.Controls.Add(this.groupBox64);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.Solo_optimizeGB);
            this.tabPage1.Controls.Add(this.Solo_bless_selection);
            this.tabPage1.Controls.Add(this.Solo_overhealing_protection);
            this.tabPage1.Controls.Add(this.Solo_tankselection);
            this.tabPage1.Controls.Add(this.Solo_adancePanel);
            this.tabPage1.Controls.Add(this.Solo_CleanseGB);
            this.tabPage1.Controls.Add(this.Solo_interruptersGB);
            this.tabPage1.Controls.Add(this.Solo_Healing);
            this.tabPage1.Controls.Add(this.Solo_mana_management);
            this.tabPage1.Controls.Add(this.Solo_HRGB);
            this.tabPage1.Controls.Add(this.Solo_emergencyGB);
            this.tabPage1.Controls.Add(this.Solo_auraselctGB);
            this.tabPage1.Controls.Add(this.Solo_racialsGB);
            this.tabPage1.Controls.Add(this.Solo_movementGB);
            this.tabPage1.Controls.Add(this.Solo_generalGB);
            this.tabPage1.Controls.Add(this.Solo_dpsGB);
            this.tabPage1.Controls.Add(this.Solo_ohshitbuttonGB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(994, 653);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Solo";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox64
            // 
            this.groupBox64.Controls.Add(this.Solo_exorcism_min_mana);
            this.groupBox64.Controls.Add(this.Solo_holy_wrath_min_mana);
            this.groupBox64.Controls.Add(this.Solo_wanna_holy_wrath);
            this.groupBox64.Controls.Add(this.Solo_wanna_exorcism);
            this.groupBox64.Controls.Add(this.Solo_exorcism_for_denunce);
            this.groupBox64.Controls.Add(this.Solo_consecration_min_mana);
            this.groupBox64.Controls.Add(this.Solo_wanna_consecration);
            this.groupBox64.Location = new System.Drawing.Point(784, 516);
            this.groupBox64.Name = "groupBox64";
            this.groupBox64.Size = new System.Drawing.Size(200, 123);
            this.groupBox64.TabIndex = 45;
            this.groupBox64.TabStop = false;
            this.groupBox64.Text = "Additional DPS (if mana above..)";
            // 
            // Solo_exorcism_min_mana
            // 
            this.Solo_exorcism_min_mana.Location = new System.Drawing.Point(100, 44);
            this.Solo_exorcism_min_mana.Name = "Solo_exorcism_min_mana";
            this.Solo_exorcism_min_mana.Size = new System.Drawing.Size(60, 20);
            this.Solo_exorcism_min_mana.TabIndex = 6;
            // 
            // Solo_holy_wrath_min_mana
            // 
            this.Solo_holy_wrath_min_mana.Location = new System.Drawing.Point(100, 68);
            this.Solo_holy_wrath_min_mana.Name = "Solo_holy_wrath_min_mana";
            this.Solo_holy_wrath_min_mana.Size = new System.Drawing.Size(60, 20);
            this.Solo_holy_wrath_min_mana.TabIndex = 5;
            // 
            // Solo_wanna_holy_wrath
            // 
            this.Solo_wanna_holy_wrath.AutoSize = true;
            this.Solo_wanna_holy_wrath.Location = new System.Drawing.Point(6, 68);
            this.Solo_wanna_holy_wrath.Name = "Solo_wanna_holy_wrath";
            this.Solo_wanna_holy_wrath.Size = new System.Drawing.Size(79, 17);
            this.Solo_wanna_holy_wrath.TabIndex = 4;
            this.Solo_wanna_holy_wrath.Text = "Holy Wrath";
            this.Solo_wanna_holy_wrath.UseVisualStyleBackColor = true;
            // 
            // Solo_wanna_exorcism
            // 
            this.Solo_wanna_exorcism.AutoSize = true;
            this.Solo_wanna_exorcism.Location = new System.Drawing.Point(6, 44);
            this.Solo_wanna_exorcism.Name = "Solo_wanna_exorcism";
            this.Solo_wanna_exorcism.Size = new System.Drawing.Size(68, 17);
            this.Solo_wanna_exorcism.TabIndex = 3;
            this.Solo_wanna_exorcism.Text = "Exorcism";
            this.Solo_wanna_exorcism.UseVisualStyleBackColor = true;
            // 
            // Solo_exorcism_for_denunce
            // 
            this.Solo_exorcism_for_denunce.AutoSize = true;
            this.Solo_exorcism_for_denunce.Location = new System.Drawing.Point(6, 20);
            this.Solo_exorcism_for_denunce.Name = "Solo_exorcism_for_denunce";
            this.Solo_exorcism_for_denunce.Size = new System.Drawing.Size(154, 17);
            this.Solo_exorcism_for_denunce.TabIndex = 2;
            this.Solo_exorcism_for_denunce.Text = "Exorcism to keep Denunce";
            this.Solo_exorcism_for_denunce.UseVisualStyleBackColor = true;
            // 
            // Solo_consecration_min_mana
            // 
            this.Solo_consecration_min_mana.Location = new System.Drawing.Point(100, 91);
            this.Solo_consecration_min_mana.Name = "Solo_consecration_min_mana";
            this.Solo_consecration_min_mana.Size = new System.Drawing.Size(60, 20);
            this.Solo_consecration_min_mana.TabIndex = 1;
            // 
            // Solo_wanna_consecration
            // 
            this.Solo_wanna_consecration.AutoSize = true;
            this.Solo_wanna_consecration.Location = new System.Drawing.Point(6, 91);
            this.Solo_wanna_consecration.Name = "Solo_wanna_consecration";
            this.Solo_wanna_consecration.Size = new System.Drawing.Size(88, 17);
            this.Solo_wanna_consecration.TabIndex = 0;
            this.Solo_wanna_consecration.Text = "Consecration";
            this.Solo_wanna_consecration.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.Solo_enable_pull);
            this.groupBox7.Controls.Add(this.Solo_answer_PVP_attacks);
            this.groupBox7.Location = new System.Drawing.Point(330, 536);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(244, 78);
            this.groupBox7.TabIndex = 44;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "FightBack";
            // 
            // Solo_enable_pull
            // 
            this.Solo_enable_pull.AutoSize = true;
            this.Solo_enable_pull.Location = new System.Drawing.Point(7, 20);
            this.Solo_enable_pull.Name = "Solo_enable_pull";
            this.Solo_enable_pull.Size = new System.Drawing.Size(102, 17);
            this.Solo_enable_pull.TabIndex = 1;
            this.Solo_enable_pull.Text = "Pull Enemy Mob";
            this.Solo_enable_pull.UseVisualStyleBackColor = true;
            this.Solo_enable_pull.CheckedChanged += new System.EventHandler(this.Solo_enable_pull_CheckedChanged);
            // 
            // Solo_answer_PVP_attacks
            // 
            this.Solo_answer_PVP_attacks.AutoSize = true;
            this.Solo_answer_PVP_attacks.Location = new System.Drawing.Point(7, 43);
            this.Solo_answer_PVP_attacks.Name = "Solo_answer_PVP_attacks";
            this.Solo_answer_PVP_attacks.Size = new System.Drawing.Size(190, 17);
            this.Solo_answer_PVP_attacks.TabIndex = 0;
            this.Solo_answer_PVP_attacks.Text = "Answer PVP attack on PVP realms";
            this.Solo_answer_PVP_attacks.UseVisualStyleBackColor = true;
            this.Solo_answer_PVP_attacks.CheckedChanged += new System.EventHandler(this.Solo_answer_PVP_attacks_CheckedChanged);
            // 
            // Solo_optimizeGB
            // 
            this.Solo_optimizeGB.Controls.Add(this.Solo_account_for_lag);
            this.Solo_optimizeGB.Controls.Add(this.Solo_intellywait);
            this.Solo_optimizeGB.Controls.Add(this.Solo_accurancy);
            this.Solo_optimizeGB.Controls.Add(this.Solo_speed);
            this.Solo_optimizeGB.Location = new System.Drawing.Point(578, 408);
            this.Solo_optimizeGB.Name = "Solo_optimizeGB";
            this.Solo_optimizeGB.Size = new System.Drawing.Size(200, 150);
            this.Solo_optimizeGB.TabIndex = 43;
            this.Solo_optimizeGB.TabStop = false;
            this.Solo_optimizeGB.Text = "Optimize the CC for";
            this.Solo_optimizeGB.Enter += new System.EventHandler(this.Solo_optimizeGB_Enter);
            // 
            // Solo_account_for_lag
            // 
            this.Solo_account_for_lag.AutoSize = true;
            this.Solo_account_for_lag.Location = new System.Drawing.Point(8, 91);
            this.Solo_account_for_lag.Name = "Solo_account_for_lag";
            this.Solo_account_for_lag.Size = new System.Drawing.Size(80, 17);
            this.Solo_account_for_lag.TabIndex = 3;
            this.Solo_account_for_lag.Text = "checkBox1";
            this.Solo_account_for_lag.UseVisualStyleBackColor = true;
            // 
            // Solo_intellywait
            // 
            this.Solo_intellywait.AutoSize = true;
            this.Solo_intellywait.Location = new System.Drawing.Point(7, 68);
            this.Solo_intellywait.Name = "Solo_intellywait";
            this.Solo_intellywait.Size = new System.Drawing.Size(159, 17);
            this.Solo_intellywait.TabIndex = 2;
            this.Solo_intellywait.TabStop = true;
            this.Solo_intellywait.Tag = "optimize";
            this.Solo_intellywait.Text = "IntellyWait (combat sistem 6)";
            this.Solo_intellywait.UseVisualStyleBackColor = true;
            this.Solo_intellywait.CheckedChanged += new System.EventHandler(this.Solo_intellywait_CheckedChanged);
            // 
            // Solo_accurancy
            // 
            this.Solo_accurancy.AutoSize = true;
            this.Solo_accurancy.Location = new System.Drawing.Point(7, 44);
            this.Solo_accurancy.Name = "Solo_accurancy";
            this.Solo_accurancy.Size = new System.Drawing.Size(161, 17);
            this.Solo_accurancy.TabIndex = 1;
            this.Solo_accurancy.TabStop = true;
            this.Solo_accurancy.Tag = "optimize";
            this.Solo_accurancy.Text = "Accurancy (combat sistem 5)";
            this.Solo_accurancy.UseVisualStyleBackColor = true;
            this.Solo_accurancy.CheckedChanged += new System.EventHandler(this.Solo_accurancy_CheckedChanged);
            // 
            // Solo_speed
            // 
            this.Solo_speed.AutoSize = true;
            this.Solo_speed.Location = new System.Drawing.Point(8, 20);
            this.Solo_speed.Name = "Solo_speed";
            this.Solo_speed.Size = new System.Drawing.Size(141, 17);
            this.Solo_speed.TabIndex = 0;
            this.Solo_speed.TabStop = true;
            this.Solo_speed.Tag = "optimize";
            this.Solo_speed.Text = "Speed (combat sistem 4)";
            this.Solo_speed.UseVisualStyleBackColor = true;
            this.Solo_speed.CheckedChanged += new System.EventHandler(this.Solo_speed_CheckedChanged);
            // 
            // Solo_bless_selection
            // 
            this.Solo_bless_selection.Controls.Add(this.Solo_bless_type_disabledRB);
            this.Solo_bless_selection.Controls.Add(this.Solo_bless_type_MightRB);
            this.Solo_bless_selection.Controls.Add(this.Solo_bless_type_KingRB);
            this.Solo_bless_selection.Controls.Add(this.Solo_bless_type_autoRB);
            this.Solo_bless_selection.Location = new System.Drawing.Point(822, 392);
            this.Solo_bless_selection.Name = "Solo_bless_selection";
            this.Solo_bless_selection.Size = new System.Drawing.Size(146, 118);
            this.Solo_bless_selection.TabIndex = 42;
            this.Solo_bless_selection.TabStop = false;
            this.Solo_bless_selection.Text = "Bless Selection";
            this.Solo_bless_selection.Enter += new System.EventHandler(this.Solo_bless_selection_Enter);
            // 
            // Solo_bless_type_disabledRB
            // 
            this.Solo_bless_type_disabledRB.AutoSize = true;
            this.Solo_bless_type_disabledRB.Location = new System.Drawing.Point(7, 92);
            this.Solo_bless_type_disabledRB.Name = "Solo_bless_type_disabledRB";
            this.Solo_bless_type_disabledRB.Size = new System.Drawing.Size(66, 17);
            this.Solo_bless_type_disabledRB.TabIndex = 3;
            this.Solo_bless_type_disabledRB.TabStop = true;
            this.Solo_bless_type_disabledRB.Tag = "Bless";
            this.Solo_bless_type_disabledRB.Text = "Disabled";
            this.Solo_bless_type_disabledRB.UseVisualStyleBackColor = true;
            this.Solo_bless_type_disabledRB.CheckedChanged += new System.EventHandler(this.Solo_bless_type_disabledRB_CheckedChanged);
            // 
            // Solo_bless_type_MightRB
            // 
            this.Solo_bless_type_MightRB.AutoSize = true;
            this.Solo_bless_type_MightRB.Location = new System.Drawing.Point(7, 68);
            this.Solo_bless_type_MightRB.Name = "Solo_bless_type_MightRB";
            this.Solo_bless_type_MightRB.Size = new System.Drawing.Size(51, 17);
            this.Solo_bless_type_MightRB.TabIndex = 2;
            this.Solo_bless_type_MightRB.TabStop = true;
            this.Solo_bless_type_MightRB.Tag = "Bless";
            this.Solo_bless_type_MightRB.Text = "Might";
            this.Solo_bless_type_MightRB.UseVisualStyleBackColor = true;
            this.Solo_bless_type_MightRB.CheckedChanged += new System.EventHandler(this.Solo_bless_type_MightRB_CheckedChanged);
            // 
            // Solo_bless_type_KingRB
            // 
            this.Solo_bless_type_KingRB.AutoSize = true;
            this.Solo_bless_type_KingRB.Location = new System.Drawing.Point(7, 44);
            this.Solo_bless_type_KingRB.Name = "Solo_bless_type_KingRB";
            this.Solo_bless_type_KingRB.Size = new System.Drawing.Size(46, 17);
            this.Solo_bless_type_KingRB.TabIndex = 1;
            this.Solo_bless_type_KingRB.TabStop = true;
            this.Solo_bless_type_KingRB.Tag = "Bless";
            this.Solo_bless_type_KingRB.Text = "King";
            this.Solo_bless_type_KingRB.UseVisualStyleBackColor = true;
            this.Solo_bless_type_KingRB.CheckedChanged += new System.EventHandler(this.Solo_bless_type_KingRB_CheckedChanged);
            // 
            // Solo_bless_type_autoRB
            // 
            this.Solo_bless_type_autoRB.AutoSize = true;
            this.Solo_bless_type_autoRB.Location = new System.Drawing.Point(7, 20);
            this.Solo_bless_type_autoRB.Name = "Solo_bless_type_autoRB";
            this.Solo_bless_type_autoRB.Size = new System.Drawing.Size(47, 17);
            this.Solo_bless_type_autoRB.TabIndex = 0;
            this.Solo_bless_type_autoRB.TabStop = true;
            this.Solo_bless_type_autoRB.Tag = "Bless";
            this.Solo_bless_type_autoRB.Text = "Auto";
            this.Solo_bless_type_autoRB.UseVisualStyleBackColor = true;
            this.Solo_bless_type_autoRB.CheckedChanged += new System.EventHandler(this.Solo_bless_type_autoRB_CheckedChanged);
            // 
            // Solo_overhealing_protection
            // 
            this.Solo_overhealing_protection.Location = new System.Drawing.Point(578, 301);
            this.Solo_overhealing_protection.Name = "Solo_overhealing_protection";
            this.Solo_overhealing_protection.Size = new System.Drawing.Size(236, 100);
            this.Solo_overhealing_protection.TabIndex = 41;
            this.Solo_overhealing_protection.TabStop = false;
            this.Solo_overhealing_protection.Text = "Overhealing Protection";
            // 
            // Solo_tankselection
            // 
            this.Solo_tankselection.Controls.Add(this.Solo_get_tank_from_lua);
            this.Solo_tankselection.Controls.Add(this.Solo_get_tank_from_focus);
            this.Solo_tankselection.Location = new System.Drawing.Point(12, 91);
            this.Solo_tankselection.Name = "Solo_tankselection";
            this.Solo_tankselection.Size = new System.Drawing.Size(307, 69);
            this.Solo_tankselection.TabIndex = 40;
            this.Solo_tankselection.TabStop = false;
            this.Solo_tankselection.Text = "Tank Selection";
            // 
            // Solo_get_tank_from_lua
            // 
            this.Solo_get_tank_from_lua.AutoSize = true;
            this.Solo_get_tank_from_lua.Location = new System.Drawing.Point(8, 42);
            this.Solo_get_tank_from_lua.Name = "Solo_get_tank_from_lua";
            this.Solo_get_tank_from_lua.Size = new System.Drawing.Size(156, 17);
            this.Solo_get_tank_from_lua.TabIndex = 2;
            this.Solo_get_tank_from_lua.Text = "Use the ingame RoleCheck";
            this.Solo_get_tank_from_lua.UseVisualStyleBackColor = true;
            this.Solo_get_tank_from_lua.CheckedChanged += new System.EventHandler(this.Solo_get_tank_from_lua_CheckedChanged);
            // 
            // Solo_get_tank_from_focus
            // 
            this.Solo_get_tank_from_focus.AutoSize = true;
            this.Solo_get_tank_from_focus.Location = new System.Drawing.Point(8, 19);
            this.Solo_get_tank_from_focus.Name = "Solo_get_tank_from_focus";
            this.Solo_get_tank_from_focus.Size = new System.Drawing.Size(179, 17);
            this.Solo_get_tank_from_focus.TabIndex = 1;
            this.Solo_get_tank_from_focus.Text = "Import the Tank from your Focus";
            this.Solo_get_tank_from_focus.UseVisualStyleBackColor = true;
            this.Solo_get_tank_from_focus.CheckedChanged += new System.EventHandler(this.Solo_get_tank_from_focus_CheckedChanged);
            // 
            // Solo_adancePanel
            // 
            this.Solo_adancePanel.Controls.Add(this.Solo_advanced_option);
            this.Solo_adancePanel.Controls.Add(this.Solo_advanced);
            this.Solo_adancePanel.Location = new System.Drawing.Point(12, 437);
            this.Solo_adancePanel.Name = "Solo_adancePanel";
            this.Solo_adancePanel.Size = new System.Drawing.Size(313, 202);
            this.Solo_adancePanel.TabIndex = 39;
            // 
            // Solo_advanced_option
            // 
            this.Solo_advanced_option.AutoSize = true;
            this.Solo_advanced_option.Location = new System.Drawing.Point(6, 3);
            this.Solo_advanced_option.Name = "Solo_advanced_option";
            this.Solo_advanced_option.Size = new System.Drawing.Size(144, 17);
            this.Solo_advanced_option.TabIndex = 34;
            this.Solo_advanced_option.Text = "Show Advanced Options";
            this.Solo_advanced_option.UseVisualStyleBackColor = true;
            this.Solo_advanced_option.CheckedChanged += new System.EventHandler(this.Solo_advanced_option_CheckedChanged);
            // 
            // Solo_advanced
            // 
            this.Solo_advanced.Controls.Add(this.Solo_wanna_move);
            this.Solo_advanced.Controls.Add(this.Solo_max_healing_distanceLB);
            this.Solo_advanced.Controls.Add(this.Solo_max_healing_distance);
            this.Solo_advanced.Controls.Add(this.Solo_wanna_target);
            this.Solo_advanced.Controls.Add(this.Solo_donothealaboveGB);
            this.Solo_advanced.Controls.Add(this.Solo_wanna_face);
            this.Solo_advanced.Location = new System.Drawing.Point(6, 26);
            this.Solo_advanced.Name = "Solo_advanced";
            this.Solo_advanced.Size = new System.Drawing.Size(301, 176);
            this.Solo_advanced.TabIndex = 33;
            this.Solo_advanced.TabStop = false;
            this.Solo_advanced.Text = "Advanced Options";
            // 
            // Solo_wanna_move
            // 
            this.Solo_wanna_move.AutoSize = true;
            this.Solo_wanna_move.Location = new System.Drawing.Point(13, 143);
            this.Solo_wanna_move.Name = "Solo_wanna_move";
            this.Solo_wanna_move.Size = new System.Drawing.Size(141, 17);
            this.Solo_wanna_move.TabIndex = 34;
            this.Solo_wanna_move.Text = "Move in range of Enemy";
            this.Solo_wanna_move.UseVisualStyleBackColor = true;
            this.Solo_wanna_move.CheckedChanged += new System.EventHandler(this.Solo_wanna_move_CheckedChanged);
            // 
            // Solo_max_healing_distanceLB
            // 
            this.Solo_max_healing_distanceLB.AutoSize = true;
            this.Solo_max_healing_distanceLB.Location = new System.Drawing.Point(71, 124);
            this.Solo_max_healing_distanceLB.Name = "Solo_max_healing_distanceLB";
            this.Solo_max_healing_distanceLB.Size = new System.Drawing.Size(160, 13);
            this.Solo_max_healing_distanceLB.TabIndex = 33;
            this.Solo_max_healing_distanceLB.Text = "Ignore unit more distant than this";
            // 
            // Solo_max_healing_distance
            // 
            this.Solo_max_healing_distance.Location = new System.Drawing.Point(13, 117);
            this.Solo_max_healing_distance.Name = "Solo_max_healing_distance";
            this.Solo_max_healing_distance.Size = new System.Drawing.Size(52, 20);
            this.Solo_max_healing_distance.TabIndex = 32;
            this.Solo_max_healing_distance.ValueChanged += new System.EventHandler(this.Solo_max_healing_distance_ValueChanged);
            // 
            // Solo_wanna_target
            // 
            this.Solo_wanna_target.AutoSize = true;
            this.Solo_wanna_target.Location = new System.Drawing.Point(13, 101);
            this.Solo_wanna_target.Name = "Solo_wanna_target";
            this.Solo_wanna_target.Size = new System.Drawing.Size(216, 17);
            this.Solo_wanna_target.TabIndex = 27;
            this.Solo_wanna_target.Text = "Target if no target (prevent HBCore bug)";
            this.Solo_wanna_target.UseVisualStyleBackColor = true;
            this.Solo_wanna_target.CheckedChanged += new System.EventHandler(this.Solo_wanna_target_CheckedChanged);
            // 
            // Solo_donothealaboveGB
            // 
            this.Solo_donothealaboveGB.Controls.Add(this.Solo_do_not_heal_above);
            this.Solo_donothealaboveGB.Controls.Add(this.Solo_donothealaboveLB);
            this.Solo_donothealaboveGB.Location = new System.Drawing.Point(13, 19);
            this.Solo_donothealaboveGB.Name = "Solo_donothealaboveGB";
            this.Solo_donothealaboveGB.Size = new System.Drawing.Size(248, 53);
            this.Solo_donothealaboveGB.TabIndex = 31;
            this.Solo_donothealaboveGB.TabStop = false;
            this.Solo_donothealaboveGB.Text = "Do not consider People above this health";
            // 
            // Solo_do_not_heal_above
            // 
            this.Solo_do_not_heal_above.Location = new System.Drawing.Point(6, 19);
            this.Solo_do_not_heal_above.Name = "Solo_do_not_heal_above";
            this.Solo_do_not_heal_above.Size = new System.Drawing.Size(46, 20);
            this.Solo_do_not_heal_above.TabIndex = 29;
            this.Solo_do_not_heal_above.ValueChanged += new System.EventHandler(this.Solo_do_not_heal_above_ValueChanged);
            // 
            // Solo_donothealaboveLB
            // 
            this.Solo_donothealaboveLB.AutoSize = true;
            this.Solo_donothealaboveLB.Location = new System.Drawing.Point(58, 26);
            this.Solo_donothealaboveLB.Name = "Solo_donothealaboveLB";
            this.Solo_donothealaboveLB.Size = new System.Drawing.Size(142, 13);
            this.Solo_donothealaboveLB.TabIndex = 30;
            this.Solo_donothealaboveLB.Text = "DO NOT MESS WITH THIS";
            // 
            // Solo_wanna_face
            // 
            this.Solo_wanna_face.AutoSize = true;
            this.Solo_wanna_face.Location = new System.Drawing.Point(13, 78);
            this.Solo_wanna_face.Name = "Solo_wanna_face";
            this.Solo_wanna_face.Size = new System.Drawing.Size(166, 17);
            this.Solo_wanna_face.TabIndex = 11;
            this.Solo_wanna_face.Text = "Face the target when needed";
            this.Solo_wanna_face.UseVisualStyleBackColor = true;
            this.Solo_wanna_face.CheckedChanged += new System.EventHandler(this.Solo_wanna_face_CheckedChanged);
            // 
            // Solo_CleanseGB
            // 
            this.Solo_CleanseGB.Controls.Add(this.Solo_wanna_cleanse);
            this.Solo_CleanseGB.Controls.Add(this.Solo_wanna_urgent_cleanse);
            this.Solo_CleanseGB.Location = new System.Drawing.Point(822, 314);
            this.Solo_CleanseGB.Name = "Solo_CleanseGB";
            this.Solo_CleanseGB.Size = new System.Drawing.Size(146, 72);
            this.Solo_CleanseGB.TabIndex = 38;
            this.Solo_CleanseGB.TabStop = false;
            this.Solo_CleanseGB.Text = "Cleanse";
            // 
            // Solo_wanna_cleanse
            // 
            this.Solo_wanna_cleanse.AutoSize = true;
            this.Solo_wanna_cleanse.Location = new System.Drawing.Point(8, 19);
            this.Solo_wanna_cleanse.Name = "Solo_wanna_cleanse";
            this.Solo_wanna_cleanse.Size = new System.Drawing.Size(64, 17);
            this.Solo_wanna_cleanse.TabIndex = 5;
            this.Solo_wanna_cleanse.Text = "Cleanse";
            this.Solo_wanna_cleanse.UseVisualStyleBackColor = true;
            this.Solo_wanna_cleanse.CheckedChanged += new System.EventHandler(this.Solo_wanna_cleanse_CheckedChanged);
            // 
            // Solo_wanna_urgent_cleanse
            // 
            this.Solo_wanna_urgent_cleanse.AutoSize = true;
            this.Solo_wanna_urgent_cleanse.Location = new System.Drawing.Point(8, 42);
            this.Solo_wanna_urgent_cleanse.Name = "Solo_wanna_urgent_cleanse";
            this.Solo_wanna_urgent_cleanse.Size = new System.Drawing.Size(135, 17);
            this.Solo_wanna_urgent_cleanse.TabIndex = 27;
            this.Solo_wanna_urgent_cleanse.Text = "Cleanse Urgents ASAP";
            this.Solo_wanna_urgent_cleanse.UseVisualStyleBackColor = true;
            this.Solo_wanna_urgent_cleanse.CheckedChanged += new System.EventHandler(this.Solo_wanna_urgent_cleanse_CheckedChanged);
            // 
            // Solo_interruptersGB
            // 
            this.Solo_interruptersGB.Controls.Add(this.Solo_wanna_HoJ);
            this.Solo_interruptersGB.Controls.Add(this.Solo_wanna_rebuke);
            this.Solo_interruptersGB.Location = new System.Drawing.Point(822, 125);
            this.Solo_interruptersGB.Name = "Solo_interruptersGB";
            this.Solo_interruptersGB.Size = new System.Drawing.Size(146, 77);
            this.Solo_interruptersGB.TabIndex = 37;
            this.Solo_interruptersGB.TabStop = false;
            this.Solo_interruptersGB.Text = "Interrupts";
            // 
            // Solo_wanna_HoJ
            // 
            this.Solo_wanna_HoJ.AutoSize = true;
            this.Solo_wanna_HoJ.Location = new System.Drawing.Point(9, 19);
            this.Solo_wanna_HoJ.Name = "Solo_wanna_HoJ";
            this.Solo_wanna_HoJ.Size = new System.Drawing.Size(113, 17);
            this.Solo_wanna_HoJ.TabIndex = 17;
            this.Solo_wanna_HoJ.Text = "Hammer of Justice";
            this.Solo_wanna_HoJ.UseVisualStyleBackColor = true;
            this.Solo_wanna_HoJ.CheckedChanged += new System.EventHandler(this.Solo_wanna_HoJ_CheckedChanged);
            // 
            // Solo_wanna_rebuke
            // 
            this.Solo_wanna_rebuke.AutoSize = true;
            this.Solo_wanna_rebuke.Location = new System.Drawing.Point(9, 43);
            this.Solo_wanna_rebuke.Name = "Solo_wanna_rebuke";
            this.Solo_wanna_rebuke.Size = new System.Drawing.Size(64, 17);
            this.Solo_wanna_rebuke.TabIndex = 26;
            this.Solo_wanna_rebuke.Text = "Rebuke";
            this.Solo_wanna_rebuke.UseVisualStyleBackColor = true;
            this.Solo_wanna_rebuke.CheckedChanged += new System.EventHandler(this.Solo_wanna_rebuke_CheckedChanged);
            // 
            // Solo_Healing
            // 
            this.Solo_Healing.Controls.Add(this.Solo_min_Inf_of_light_DL_hpLB);
            this.Solo_Healing.Controls.Add(this.Solo_min_Inf_of_light_DL_hp);
            this.Solo_Healing.Controls.Add(this.Solo_min_HL_hpLB);
            this.Solo_Healing.Controls.Add(this.Solo_min_HL_hp);
            this.Solo_Healing.Controls.Add(this.Solo_min_FoL_hpLB);
            this.Solo_Healing.Controls.Add(this.Solo_min_FoL_hp);
            this.Solo_Healing.Controls.Add(this.Solo_min_DL_hpLB);
            this.Solo_Healing.Controls.Add(this.Solo_min_DL_hp);
            this.Solo_Healing.Controls.Add(this.Solo_Inf_of_light_wanna_DL);
            this.Solo_Healing.Location = new System.Drawing.Point(325, 19);
            this.Solo_Healing.Name = "Solo_Healing";
            this.Solo_Healing.Size = new System.Drawing.Size(249, 160);
            this.Solo_Healing.TabIndex = 36;
            this.Solo_Healing.TabStop = false;
            this.Solo_Healing.Text = "Healing";
            // 
            // Solo_min_Inf_of_light_DL_hpLB
            // 
            this.Solo_min_Inf_of_light_DL_hpLB.AutoSize = true;
            this.Solo_min_Inf_of_light_DL_hpLB.Location = new System.Drawing.Point(72, 130);
            this.Solo_min_Inf_of_light_DL_hpLB.Name = "Solo_min_Inf_of_light_DL_hpLB";
            this.Solo_min_Inf_of_light_DL_hpLB.Size = new System.Drawing.Size(172, 13);
            this.Solo_min_Inf_of_light_DL_hpLB.TabIndex = 7;
            this.Solo_min_Inf_of_light_DL_hpLB.Text = "But only if the target is below this %";
            // 
            // Solo_min_Inf_of_light_DL_hp
            // 
            this.Solo_min_Inf_of_light_DL_hp.Location = new System.Drawing.Point(7, 124);
            this.Solo_min_Inf_of_light_DL_hp.Name = "Solo_min_Inf_of_light_DL_hp";
            this.Solo_min_Inf_of_light_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.Solo_min_Inf_of_light_DL_hp.TabIndex = 6;
            this.Solo_min_Inf_of_light_DL_hp.ValueChanged += new System.EventHandler(this.Solo_min_Inf_of_light_DL_hp_ValueChanged);
            // 
            // Solo_min_HL_hpLB
            // 
            this.Solo_min_HL_hpLB.AutoSize = true;
            this.Solo_min_HL_hpLB.Location = new System.Drawing.Point(70, 26);
            this.Solo_min_HL_hpLB.Name = "Solo_min_HL_hpLB";
            this.Solo_min_HL_hpLB.Size = new System.Drawing.Size(149, 13);
            this.Solo_min_HL_hpLB.TabIndex = 5;
            this.Solo_min_HL_hpLB.Text = "Holy Light targets under this %";
            // 
            // Solo_min_HL_hp
            // 
            this.Solo_min_HL_hp.Location = new System.Drawing.Point(7, 20);
            this.Solo_min_HL_hp.Name = "Solo_min_HL_hp";
            this.Solo_min_HL_hp.Size = new System.Drawing.Size(56, 20);
            this.Solo_min_HL_hp.TabIndex = 4;
            this.Solo_min_HL_hp.ValueChanged += new System.EventHandler(this.Solo_min_HL_hp_ValueChanged);
            // 
            // Solo_min_FoL_hpLB
            // 
            this.Solo_min_FoL_hpLB.AutoSize = true;
            this.Solo_min_FoL_hpLB.Location = new System.Drawing.Point(69, 76);
            this.Solo_min_FoL_hpLB.Name = "Solo_min_FoL_hpLB";
            this.Solo_min_FoL_hpLB.Size = new System.Drawing.Size(165, 13);
            this.Solo_min_FoL_hpLB.TabIndex = 3;
            this.Solo_min_FoL_hpLB.Text = "Flash of Light targets under this %";
            // 
            // Solo_min_FoL_hp
            // 
            this.Solo_min_FoL_hp.Location = new System.Drawing.Point(7, 74);
            this.Solo_min_FoL_hp.Name = "Solo_min_FoL_hp";
            this.Solo_min_FoL_hp.Size = new System.Drawing.Size(56, 20);
            this.Solo_min_FoL_hp.TabIndex = 2;
            this.Solo_min_FoL_hp.ValueChanged += new System.EventHandler(this.Solo_min_FoL_hp_ValueChanged);
            // 
            // Solo_min_DL_hpLB
            // 
            this.Solo_min_DL_hpLB.AutoSize = true;
            this.Solo_min_DL_hpLB.Location = new System.Drawing.Point(68, 53);
            this.Solo_min_DL_hpLB.Name = "Solo_min_DL_hpLB";
            this.Solo_min_DL_hpLB.Size = new System.Drawing.Size(158, 13);
            this.Solo_min_DL_hpLB.TabIndex = 1;
            this.Solo_min_DL_hpLB.Text = "Divine Light targets under this %";
            // 
            // Solo_min_DL_hp
            // 
            this.Solo_min_DL_hp.Location = new System.Drawing.Point(6, 46);
            this.Solo_min_DL_hp.Name = "Solo_min_DL_hp";
            this.Solo_min_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.Solo_min_DL_hp.TabIndex = 0;
            this.Solo_min_DL_hp.ValueChanged += new System.EventHandler(this.Solo_min_DL_hp_ValueChanged);
            // 
            // Solo_Inf_of_light_wanna_DL
            // 
            this.Solo_Inf_of_light_wanna_DL.AutoSize = true;
            this.Solo_Inf_of_light_wanna_DL.Location = new System.Drawing.Point(5, 100);
            this.Solo_Inf_of_light_wanna_DL.Name = "Solo_Inf_of_light_wanna_DL";
            this.Solo_Inf_of_light_wanna_DL.Size = new System.Drawing.Size(235, 17);
            this.Solo_Inf_of_light_wanna_DL.TabIndex = 2;
            this.Solo_Inf_of_light_wanna_DL.Text = "Use Divine Light when Infusion of Light proc";
            this.Solo_Inf_of_light_wanna_DL.UseVisualStyleBackColor = true;
            this.Solo_Inf_of_light_wanna_DL.CheckedChanged += new System.EventHandler(this.Solo_Inf_of_light_wanna_DL_CheckedChanged);
            // 
            // Solo_mana_management
            // 
            this.Solo_mana_management.Controls.Add(this.Solo_use_mana_rec_trinket_everyLB);
            this.Solo_mana_management.Controls.Add(this.Solo_use_mana_rec_trinket_every);
            this.Solo_mana_management.Controls.Add(this.Solo_min_mana_rec_trinketLB);
            this.Solo_mana_management.Controls.Add(this.Solo_min_mana_rec_trinket);
            this.Solo_mana_management.Controls.Add(this.Solo_min_Divine_Plea_manaLB);
            this.Solo_mana_management.Controls.Add(this.Solo_min_Divine_Plea_mana);
            this.Solo_mana_management.Controls.Add(this.Solo_mana_judgeLB);
            this.Solo_mana_management.Controls.Add(this.Solo_mana_judge);
            this.Solo_mana_management.Location = new System.Drawing.Point(12, 166);
            this.Solo_mana_management.Name = "Solo_mana_management";
            this.Solo_mana_management.Size = new System.Drawing.Size(307, 126);
            this.Solo_mana_management.TabIndex = 35;
            this.Solo_mana_management.TabStop = false;
            this.Solo_mana_management.Text = "Mana Management";
            // 
            // Solo_use_mana_rec_trinket_everyLB
            // 
            this.Solo_use_mana_rec_trinket_everyLB.AutoSize = true;
            this.Solo_use_mana_rec_trinket_everyLB.Location = new System.Drawing.Point(49, 103);
            this.Solo_use_mana_rec_trinket_everyLB.Name = "Solo_use_mana_rec_trinket_everyLB";
            this.Solo_use_mana_rec_trinket_everyLB.Size = new System.Drawing.Size(186, 13);
            this.Solo_use_mana_rec_trinket_everyLB.TabIndex = 7;
            this.Solo_use_mana_rec_trinket_everyLB.Text = "Activate your mana trinket every (NYI)";
            // 
            // Solo_use_mana_rec_trinket_every
            // 
            this.Solo_use_mana_rec_trinket_every.Location = new System.Drawing.Point(0, 100);
            this.Solo_use_mana_rec_trinket_every.Name = "Solo_use_mana_rec_trinket_every";
            this.Solo_use_mana_rec_trinket_every.Size = new System.Drawing.Size(43, 20);
            this.Solo_use_mana_rec_trinket_every.TabIndex = 6;
            this.Solo_use_mana_rec_trinket_every.ValueChanged += new System.EventHandler(this.Solo_use_mana_rec_trinket_every_ValueChanged);
            // 
            // Solo_min_mana_rec_trinketLB
            // 
            this.Solo_min_mana_rec_trinketLB.AutoSize = true;
            this.Solo_min_mana_rec_trinketLB.Location = new System.Drawing.Point(49, 78);
            this.Solo_min_mana_rec_trinketLB.Name = "Solo_min_mana_rec_trinketLB";
            this.Solo_min_mana_rec_trinketLB.Size = new System.Drawing.Size(208, 13);
            this.Solo_min_mana_rec_trinketLB.TabIndex = 5;
            this.Solo_min_mana_rec_trinketLB.Text = "Activate your Mana trinket at Mana %(NYI)";
            // 
            // Solo_min_mana_rec_trinket
            // 
            this.Solo_min_mana_rec_trinket.Location = new System.Drawing.Point(0, 74);
            this.Solo_min_mana_rec_trinket.Name = "Solo_min_mana_rec_trinket";
            this.Solo_min_mana_rec_trinket.Size = new System.Drawing.Size(43, 20);
            this.Solo_min_mana_rec_trinket.TabIndex = 4;
            this.Solo_min_mana_rec_trinket.ValueChanged += new System.EventHandler(this.Solo_min_mana_rec_trinket_ValueChanged);
            // 
            // Solo_min_Divine_Plea_manaLB
            // 
            this.Solo_min_Divine_Plea_manaLB.AutoSize = true;
            this.Solo_min_Divine_Plea_manaLB.Location = new System.Drawing.Point(49, 54);
            this.Solo_min_Divine_Plea_manaLB.Name = "Solo_min_Divine_Plea_manaLB";
            this.Solo_min_Divine_Plea_manaLB.Size = new System.Drawing.Size(121, 13);
            this.Solo_min_Divine_Plea_manaLB.TabIndex = 3;
            this.Solo_min_Divine_Plea_manaLB.Text = "Divine Plea at this mana";
            // 
            // Solo_min_Divine_Plea_mana
            // 
            this.Solo_min_Divine_Plea_mana.Location = new System.Drawing.Point(0, 47);
            this.Solo_min_Divine_Plea_mana.Name = "Solo_min_Divine_Plea_mana";
            this.Solo_min_Divine_Plea_mana.Size = new System.Drawing.Size(43, 20);
            this.Solo_min_Divine_Plea_mana.TabIndex = 2;
            this.Solo_min_Divine_Plea_mana.ValueChanged += new System.EventHandler(this.Solo_min_Divine_Plea_mana_ValueChanged);
            // 
            // Solo_mana_judgeLB
            // 
            this.Solo_mana_judgeLB.AutoSize = true;
            this.Solo_mana_judgeLB.Location = new System.Drawing.Point(49, 27);
            this.Solo_mana_judgeLB.Name = "Solo_mana_judgeLB";
            this.Solo_mana_judgeLB.Size = new System.Drawing.Size(242, 13);
            this.Solo_mana_judgeLB.TabIndex = 1;
            this.Solo_mana_judgeLB.Text = "Judgement on Cooldown when mana is below this";
            // 
            // Solo_mana_judge
            // 
            this.Solo_mana_judge.Location = new System.Drawing.Point(0, 20);
            this.Solo_mana_judge.Name = "Solo_mana_judge";
            this.Solo_mana_judge.Size = new System.Drawing.Size(43, 20);
            this.Solo_mana_judge.TabIndex = 0;
            this.Solo_mana_judge.ValueChanged += new System.EventHandler(this.Solo_mana_judge_ValueChanged);
            // 
            // Solo_HRGB
            // 
            this.Solo_HRGB.Controls.Add(this.Solo_min_player_inside_HRLB);
            this.Solo_HRGB.Controls.Add(this.Solo_min_player_inside_HR);
            this.Solo_HRGB.Controls.Add(this.Solo_HR_how_much_healthLB);
            this.Solo_HRGB.Controls.Add(this.Solo_HR_how_much_health);
            this.Solo_HRGB.Controls.Add(this.Solo_HR_how_farLB);
            this.Solo_HRGB.Controls.Add(this.Solo_HR_how_far);
            this.Solo_HRGB.Controls.Add(this.Solo_wanna_HR);
            this.Solo_HRGB.Location = new System.Drawing.Point(12, 298);
            this.Solo_HRGB.Name = "Solo_HRGB";
            this.Solo_HRGB.Size = new System.Drawing.Size(307, 133);
            this.Solo_HRGB.TabIndex = 32;
            this.Solo_HRGB.TabStop = false;
            this.Solo_HRGB.Text = "Holy Radiance Settings";
            // 
            // Solo_min_player_inside_HRLB
            // 
            this.Solo_min_player_inside_HRLB.AutoSize = true;
            this.Solo_min_player_inside_HRLB.Location = new System.Drawing.Point(61, 103);
            this.Solo_min_player_inside_HRLB.Name = "Solo_min_player_inside_HRLB";
            this.Solo_min_player_inside_HRLB.Size = new System.Drawing.Size(145, 13);
            this.Solo_min_player_inside_HRLB.TabIndex = 27;
            this.Solo_min_player_inside_HRLB.Text = "Need this many people inside";
            // 
            // Solo_min_player_inside_HR
            // 
            this.Solo_min_player_inside_HR.Location = new System.Drawing.Point(7, 97);
            this.Solo_min_player_inside_HR.Name = "Solo_min_player_inside_HR";
            this.Solo_min_player_inside_HR.Size = new System.Drawing.Size(44, 20);
            this.Solo_min_player_inside_HR.TabIndex = 26;
            this.Solo_min_player_inside_HR.ValueChanged += new System.EventHandler(this.Solo_min_player_inside_HR_ValueChanged);
            // 
            // Solo_HR_how_much_healthLB
            // 
            this.Solo_HR_how_much_healthLB.AutoSize = true;
            this.Solo_HR_how_much_healthLB.Location = new System.Drawing.Point(58, 76);
            this.Solo_HR_how_much_healthLB.Name = "Solo_HR_how_much_healthLB";
            this.Solo_HR_how_much_healthLB.Size = new System.Drawing.Size(196, 13);
            this.Solo_HR_how_much_healthLB.TabIndex = 25;
            this.Solo_HR_how_much_healthLB.Text = "Consider unit with less than this Health%";
            // 
            // Solo_HR_how_much_health
            // 
            this.Solo_HR_how_much_health.Location = new System.Drawing.Point(7, 70);
            this.Solo_HR_how_much_health.Name = "Solo_HR_how_much_health";
            this.Solo_HR_how_much_health.Size = new System.Drawing.Size(44, 20);
            this.Solo_HR_how_much_health.TabIndex = 24;
            this.Solo_HR_how_much_health.ValueChanged += new System.EventHandler(this.Solo_HR_how_much_health_ValueChanged);
            // 
            // Solo_HR_how_farLB
            // 
            this.Solo_HR_how_farLB.AutoSize = true;
            this.Solo_HR_how_farLB.Location = new System.Drawing.Point(58, 49);
            this.Solo_HR_how_farLB.Name = "Solo_HR_how_farLB";
            this.Solo_HR_how_farLB.Size = new System.Drawing.Size(173, 13);
            this.Solo_HR_how_farLB.TabIndex = 23;
            this.Solo_HR_how_farLB.Text = "Consider unit nearer then this to me";
            // 
            // Solo_HR_how_far
            // 
            this.Solo_HR_how_far.Location = new System.Drawing.Point(6, 43);
            this.Solo_HR_how_far.Name = "Solo_HR_how_far";
            this.Solo_HR_how_far.Size = new System.Drawing.Size(45, 20);
            this.Solo_HR_how_far.TabIndex = 22;
            this.Solo_HR_how_far.ValueChanged += new System.EventHandler(this.Solo_HR_how_far_ValueChanged);
            // 
            // Solo_wanna_HR
            // 
            this.Solo_wanna_HR.AutoSize = true;
            this.Solo_wanna_HR.Location = new System.Drawing.Point(6, 19);
            this.Solo_wanna_HR.Name = "Solo_wanna_HR";
            this.Solo_wanna_HR.Size = new System.Drawing.Size(96, 17);
            this.Solo_wanna_HR.TabIndex = 21;
            this.Solo_wanna_HR.Text = "Holy Radiance";
            this.Solo_wanna_HR.UseVisualStyleBackColor = true;
            this.Solo_wanna_HR.CheckedChanged += new System.EventHandler(this.Solo_wanna_HR_CheckedChanged);
            // 
            // Solo_emergencyGB
            // 
            this.Solo_emergencyGB.Controls.Add(this.Solo_min_mana_potion);
            this.Solo_emergencyGB.Controls.Add(this.Solo_min_LoH_hp);
            this.Solo_emergencyGB.Controls.Add(this.Solo_min_HoS_hp);
            this.Solo_emergencyGB.Controls.Add(this.Solo_min_HoP_hp);
            this.Solo_emergencyGB.Controls.Add(this.Solo_min_DS_hp);
            this.Solo_emergencyGB.Controls.Add(this.Solo_emergency_buttonLB);
            this.Solo_emergencyGB.Controls.Add(this.Solo_min_DP_hp);
            this.Solo_emergencyGB.Controls.Add(this.Solo_wanna_mana_potion);
            this.Solo_emergencyGB.Controls.Add(this.Solo_wanna_LoH);
            this.Solo_emergencyGB.Controls.Add(this.Solo_wanna_DP);
            this.Solo_emergencyGB.Controls.Add(this.Solo_wanna_DS);
            this.Solo_emergencyGB.Controls.Add(this.Solo_wanna_HoS);
            this.Solo_emergencyGB.Controls.Add(this.Solo_wanna_HoP);
            this.Solo_emergencyGB.Location = new System.Drawing.Point(325, 185);
            this.Solo_emergencyGB.Name = "Solo_emergencyGB";
            this.Solo_emergencyGB.Size = new System.Drawing.Size(247, 182);
            this.Solo_emergencyGB.TabIndex = 22;
            this.Solo_emergencyGB.TabStop = false;
            this.Solo_emergencyGB.Text = "Emergency Buttons";
            // 
            // Solo_min_mana_potion
            // 
            this.Solo_min_mana_potion.Location = new System.Drawing.Point(120, 154);
            this.Solo_min_mana_potion.Name = "Solo_min_mana_potion";
            this.Solo_min_mana_potion.Size = new System.Drawing.Size(64, 20);
            this.Solo_min_mana_potion.TabIndex = 30;
            this.Solo_min_mana_potion.ValueChanged += new System.EventHandler(this.Solo_min_mana_potion_ValueChanged);
            // 
            // Solo_min_LoH_hp
            // 
            this.Solo_min_LoH_hp.Location = new System.Drawing.Point(120, 130);
            this.Solo_min_LoH_hp.Name = "Solo_min_LoH_hp";
            this.Solo_min_LoH_hp.Size = new System.Drawing.Size(64, 20);
            this.Solo_min_LoH_hp.TabIndex = 29;
            this.Solo_min_LoH_hp.ValueChanged += new System.EventHandler(this.Solo_min_LoH_hp_ValueChanged);
            // 
            // Solo_min_HoS_hp
            // 
            this.Solo_min_HoS_hp.Location = new System.Drawing.Point(120, 106);
            this.Solo_min_HoS_hp.Name = "Solo_min_HoS_hp";
            this.Solo_min_HoS_hp.Size = new System.Drawing.Size(64, 20);
            this.Solo_min_HoS_hp.TabIndex = 28;
            this.Solo_min_HoS_hp.ValueChanged += new System.EventHandler(this.Solo_min_HoS_hp_ValueChanged);
            // 
            // Solo_min_HoP_hp
            // 
            this.Solo_min_HoP_hp.Location = new System.Drawing.Point(120, 84);
            this.Solo_min_HoP_hp.Name = "Solo_min_HoP_hp";
            this.Solo_min_HoP_hp.Size = new System.Drawing.Size(64, 20);
            this.Solo_min_HoP_hp.TabIndex = 27;
            this.Solo_min_HoP_hp.ValueChanged += new System.EventHandler(this.Solo_min_HoP_hp_ValueChanged);
            // 
            // Solo_min_DS_hp
            // 
            this.Solo_min_DS_hp.Location = new System.Drawing.Point(120, 58);
            this.Solo_min_DS_hp.Name = "Solo_min_DS_hp";
            this.Solo_min_DS_hp.Size = new System.Drawing.Size(64, 20);
            this.Solo_min_DS_hp.TabIndex = 26;
            this.Solo_min_DS_hp.ValueChanged += new System.EventHandler(this.Solo_min_DS_hp_ValueChanged);
            // 
            // Solo_emergency_buttonLB
            // 
            this.Solo_emergency_buttonLB.AutoSize = true;
            this.Solo_emergency_buttonLB.Location = new System.Drawing.Point(120, 16);
            this.Solo_emergency_buttonLB.Name = "Solo_emergency_buttonLB";
            this.Solo_emergency_buttonLB.Size = new System.Drawing.Size(58, 13);
            this.Solo_emergency_buttonLB.TabIndex = 25;
            this.Solo_emergency_buttonLB.Text = "Use Below";
            // 
            // Solo_min_DP_hp
            // 
            this.Solo_min_DP_hp.Location = new System.Drawing.Point(120, 35);
            this.Solo_min_DP_hp.Name = "Solo_min_DP_hp";
            this.Solo_min_DP_hp.Size = new System.Drawing.Size(64, 20);
            this.Solo_min_DP_hp.TabIndex = 24;
            this.Solo_min_DP_hp.ValueChanged += new System.EventHandler(this.Solo_min_DP_hp_ValueChanged);
            // 
            // Solo_wanna_mana_potion
            // 
            this.Solo_wanna_mana_potion.AutoSize = true;
            this.Solo_wanna_mana_potion.Location = new System.Drawing.Point(6, 154);
            this.Solo_wanna_mana_potion.Name = "Solo_wanna_mana_potion";
            this.Solo_wanna_mana_potion.Size = new System.Drawing.Size(113, 17);
            this.Solo_wanna_mana_potion.TabIndex = 23;
            this.Solo_wanna_mana_potion.Text = "Mana Potion (NYI)";
            this.Solo_wanna_mana_potion.UseVisualStyleBackColor = true;
            this.Solo_wanna_mana_potion.CheckedChanged += new System.EventHandler(this.Solo_wanna_mana_potion_CheckedChanged);
            // 
            // Solo_wanna_LoH
            // 
            this.Solo_wanna_LoH.AutoSize = true;
            this.Solo_wanna_LoH.Location = new System.Drawing.Point(6, 130);
            this.Solo_wanna_LoH.Name = "Solo_wanna_LoH";
            this.Solo_wanna_LoH.Size = new System.Drawing.Size(87, 17);
            this.Solo_wanna_LoH.TabIndex = 23;
            this.Solo_wanna_LoH.Text = "Lay on Hand";
            this.Solo_wanna_LoH.UseVisualStyleBackColor = true;
            this.Solo_wanna_LoH.CheckedChanged += new System.EventHandler(this.Solo_wanna_LoH_CheckedChanged);
            // 
            // Solo_wanna_DP
            // 
            this.Solo_wanna_DP.AutoSize = true;
            this.Solo_wanna_DP.Location = new System.Drawing.Point(6, 38);
            this.Solo_wanna_DP.Name = "Solo_wanna_DP";
            this.Solo_wanna_DP.Size = new System.Drawing.Size(107, 17);
            this.Solo_wanna_DP.TabIndex = 8;
            this.Solo_wanna_DP.Text = "Divine Protection";
            this.Solo_wanna_DP.UseVisualStyleBackColor = true;
            this.Solo_wanna_DP.CheckedChanged += new System.EventHandler(this.Solo_wanna_DP_CheckedChanged);
            // 
            // Solo_wanna_DS
            // 
            this.Solo_wanna_DS.AutoSize = true;
            this.Solo_wanna_DS.Location = new System.Drawing.Point(6, 61);
            this.Solo_wanna_DS.Name = "Solo_wanna_DS";
            this.Solo_wanna_DS.Size = new System.Drawing.Size(88, 17);
            this.Solo_wanna_DS.TabIndex = 9;
            this.Solo_wanna_DS.Text = "Divine Shield";
            this.Solo_wanna_DS.UseVisualStyleBackColor = true;
            this.Solo_wanna_DS.CheckedChanged += new System.EventHandler(this.Solo_wanna_DS_CheckedChanged);
            // 
            // Solo_wanna_HoS
            // 
            this.Solo_wanna_HoS.AutoSize = true;
            this.Solo_wanna_HoS.Location = new System.Drawing.Point(6, 107);
            this.Solo_wanna_HoS.Name = "Solo_wanna_HoS";
            this.Solo_wanna_HoS.Size = new System.Drawing.Size(111, 17);
            this.Solo_wanna_HoS.TabIndex = 19;
            this.Solo_wanna_HoS.Text = "Hand of Salvation";
            this.Solo_wanna_HoS.UseVisualStyleBackColor = true;
            this.Solo_wanna_HoS.CheckedChanged += new System.EventHandler(this.Solo_wanna_HoS_CheckedChanged);
            // 
            // Solo_wanna_HoP
            // 
            this.Solo_wanna_HoP.AutoSize = true;
            this.Solo_wanna_HoP.Location = new System.Drawing.Point(6, 84);
            this.Solo_wanna_HoP.Name = "Solo_wanna_HoP";
            this.Solo_wanna_HoP.Size = new System.Drawing.Size(115, 17);
            this.Solo_wanna_HoP.TabIndex = 18;
            this.Solo_wanna_HoP.Text = "Hand of Protection";
            this.Solo_wanna_HoP.UseVisualStyleBackColor = true;
            this.Solo_wanna_HoP.CheckedChanged += new System.EventHandler(this.Solo_wanna_HoP_CheckedChanged);
            // 
            // Solo_auraselctGB
            // 
            this.Solo_auraselctGB.Controls.Add(this.Solo_DisabledRB);
            this.Solo_auraselctGB.Controls.Add(this.Solo_resistanceRB);
            this.Solo_auraselctGB.Controls.Add(this.Solo_concentrationRB);
            this.Solo_auraselctGB.Location = new System.Drawing.Point(822, 19);
            this.Solo_auraselctGB.Name = "Solo_auraselctGB";
            this.Solo_auraselctGB.Size = new System.Drawing.Size(146, 100);
            this.Solo_auraselctGB.TabIndex = 28;
            this.Solo_auraselctGB.TabStop = false;
            this.Solo_auraselctGB.Text = "Select Aura";
            this.Solo_auraselctGB.Enter += new System.EventHandler(this.Solo_auraselctGB_Enter);
            // 
            // Solo_DisabledRB
            // 
            this.Solo_DisabledRB.AutoSize = true;
            this.Solo_DisabledRB.Location = new System.Drawing.Point(7, 68);
            this.Solo_DisabledRB.Name = "Solo_DisabledRB";
            this.Solo_DisabledRB.Size = new System.Drawing.Size(66, 17);
            this.Solo_DisabledRB.TabIndex = 2;
            this.Solo_DisabledRB.TabStop = true;
            this.Solo_DisabledRB.Tag = "Aura";
            this.Solo_DisabledRB.Text = "Disabled";
            this.Solo_DisabledRB.UseVisualStyleBackColor = true;
            this.Solo_DisabledRB.CheckedChanged += new System.EventHandler(this.Solo_DisabledRB_CheckedChanged);
            // 
            // Solo_resistanceRB
            // 
            this.Solo_resistanceRB.AutoSize = true;
            this.Solo_resistanceRB.Location = new System.Drawing.Point(7, 44);
            this.Solo_resistanceRB.Name = "Solo_resistanceRB";
            this.Solo_resistanceRB.Size = new System.Drawing.Size(78, 17);
            this.Solo_resistanceRB.TabIndex = 1;
            this.Solo_resistanceRB.TabStop = true;
            this.Solo_resistanceRB.Tag = "Aura";
            this.Solo_resistanceRB.Text = "Resistance";
            this.Solo_resistanceRB.UseVisualStyleBackColor = true;
            this.Solo_resistanceRB.CheckedChanged += new System.EventHandler(this.Solo_resistanceRB_CheckedChanged);
            // 
            // Solo_concentrationRB
            // 
            this.Solo_concentrationRB.AutoSize = true;
            this.Solo_concentrationRB.Location = new System.Drawing.Point(7, 20);
            this.Solo_concentrationRB.Name = "Solo_concentrationRB";
            this.Solo_concentrationRB.Size = new System.Drawing.Size(91, 17);
            this.Solo_concentrationRB.TabIndex = 0;
            this.Solo_concentrationRB.TabStop = true;
            this.Solo_concentrationRB.Tag = "Aura";
            this.Solo_concentrationRB.Text = "Concentration";
            this.Solo_concentrationRB.UseVisualStyleBackColor = true;
            this.Solo_concentrationRB.CheckedChanged += new System.EventHandler(this.Solo_concentrationRB_CheckedChanged);
            // 
            // Solo_racialsGB
            // 
            this.Solo_racialsGB.Controls.Add(this.Solo_min_torrent_mana_perc);
            this.Solo_racialsGB.Controls.Add(this.Solo_min_stoneform);
            this.Solo_racialsGB.Controls.Add(this.Solo_min_gift_hp);
            this.Solo_racialsGB.Controls.Add(this.Solo_wanna_torrent);
            this.Solo_racialsGB.Controls.Add(this.Solo_wanna_stoneform);
            this.Solo_racialsGB.Controls.Add(this.Solo_wanna_everymanforhimself);
            this.Solo_racialsGB.Controls.Add(this.Solo_wanna_gift);
            this.Solo_racialsGB.Location = new System.Drawing.Point(578, 170);
            this.Solo_racialsGB.Name = "Solo_racialsGB";
            this.Solo_racialsGB.Size = new System.Drawing.Size(236, 124);
            this.Solo_racialsGB.TabIndex = 15;
            this.Solo_racialsGB.TabStop = false;
            this.Solo_racialsGB.Text = "Racials";
            // 
            // Solo_min_torrent_mana_perc
            // 
            this.Solo_min_torrent_mana_perc.Location = new System.Drawing.Point(172, 95);
            this.Solo_min_torrent_mana_perc.Name = "Solo_min_torrent_mana_perc";
            this.Solo_min_torrent_mana_perc.Size = new System.Drawing.Size(59, 20);
            this.Solo_min_torrent_mana_perc.TabIndex = 17;
            this.Solo_min_torrent_mana_perc.ValueChanged += new System.EventHandler(this.Solo_min_torrent_mana_perc_ValueChanged);
            // 
            // Solo_min_stoneform
            // 
            this.Solo_min_stoneform.Location = new System.Drawing.Point(172, 71);
            this.Solo_min_stoneform.Name = "Solo_min_stoneform";
            this.Solo_min_stoneform.Size = new System.Drawing.Size(59, 20);
            this.Solo_min_stoneform.TabIndex = 16;
            this.Solo_min_stoneform.ValueChanged += new System.EventHandler(this.Solo_min_stoneform_ValueChanged);
            // 
            // Solo_min_gift_hp
            // 
            this.Solo_min_gift_hp.Location = new System.Drawing.Point(172, 45);
            this.Solo_min_gift_hp.Name = "Solo_min_gift_hp";
            this.Solo_min_gift_hp.Size = new System.Drawing.Size(59, 20);
            this.Solo_min_gift_hp.TabIndex = 15;
            this.Solo_min_gift_hp.ValueChanged += new System.EventHandler(this.Solo_min_gift_hp_ValueChanged);
            // 
            // Solo_wanna_torrent
            // 
            this.Solo_wanna_torrent.AutoSize = true;
            this.Solo_wanna_torrent.Location = new System.Drawing.Point(16, 95);
            this.Solo_wanna_torrent.Name = "Solo_wanna_torrent";
            this.Solo_wanna_torrent.Size = new System.Drawing.Size(147, 17);
            this.Solo_wanna_torrent.TabIndex = 14;
            this.Solo_wanna_torrent.Text = "Arcane Torrent (for mana)";
            this.Solo_wanna_torrent.UseVisualStyleBackColor = true;
            this.Solo_wanna_torrent.CheckedChanged += new System.EventHandler(this.Solo_wanna_torrent_CheckedChanged);
            // 
            // Solo_wanna_stoneform
            // 
            this.Solo_wanna_stoneform.AutoSize = true;
            this.Solo_wanna_stoneform.Location = new System.Drawing.Point(16, 71);
            this.Solo_wanna_stoneform.Name = "Solo_wanna_stoneform";
            this.Solo_wanna_stoneform.Size = new System.Drawing.Size(74, 17);
            this.Solo_wanna_stoneform.TabIndex = 13;
            this.Solo_wanna_stoneform.Text = "Stoneform";
            this.Solo_wanna_stoneform.UseVisualStyleBackColor = true;
            this.Solo_wanna_stoneform.CheckedChanged += new System.EventHandler(this.Solo_wanna_stoneform_CheckedChanged);
            // 
            // Solo_wanna_everymanforhimself
            // 
            this.Solo_wanna_everymanforhimself.AutoSize = true;
            this.Solo_wanna_everymanforhimself.Location = new System.Drawing.Point(16, 24);
            this.Solo_wanna_everymanforhimself.Name = "Solo_wanna_everymanforhimself";
            this.Solo_wanna_everymanforhimself.Size = new System.Drawing.Size(129, 17);
            this.Solo_wanna_everymanforhimself.TabIndex = 10;
            this.Solo_wanna_everymanforhimself.Text = "Every Man for Himself";
            this.Solo_wanna_everymanforhimself.UseVisualStyleBackColor = true;
            this.Solo_wanna_everymanforhimself.CheckedChanged += new System.EventHandler(this.Solo_wanna_everymanforhimself_CheckedChanged);
            // 
            // Solo_wanna_gift
            // 
            this.Solo_wanna_gift.AutoSize = true;
            this.Solo_wanna_gift.Location = new System.Drawing.Point(16, 47);
            this.Solo_wanna_gift.Name = "Solo_wanna_gift";
            this.Solo_wanna_gift.Size = new System.Drawing.Size(104, 17);
            this.Solo_wanna_gift.TabIndex = 12;
            this.Solo_wanna_gift.Text = "Gift of the Naaru";
            this.Solo_wanna_gift.UseVisualStyleBackColor = true;
            this.Solo_wanna_gift.CheckedChanged += new System.EventHandler(this.Solo_wanna_gift_CheckedChanged);
            // 
            // Solo_movementGB
            // 
            this.Solo_movementGB.Controls.Add(this.Solo_do_not_dismount_EVER);
            this.Solo_movementGB.Controls.Add(this.Solo_do_not_dismount_ooc);
            this.Solo_movementGB.Controls.Add(this.Solo_wanna_move_to_HoJ);
            this.Solo_movementGB.Controls.Add(this.Solo_wanna_mount);
            this.Solo_movementGB.Controls.Add(this.Solo_wanna_move_to_heal);
            this.Solo_movementGB.Controls.Add(this.Solo_wanna_crusader);
            this.Solo_movementGB.Location = new System.Drawing.Point(325, 373);
            this.Solo_movementGB.Name = "Solo_movementGB";
            this.Solo_movementGB.Size = new System.Drawing.Size(249, 156);
            this.Solo_movementGB.TabIndex = 25;
            this.Solo_movementGB.TabStop = false;
            this.Solo_movementGB.Text = "Movement";
            // 
            // Solo_do_not_dismount_EVER
            // 
            this.Solo_do_not_dismount_EVER.AutoSize = true;
            this.Solo_do_not_dismount_EVER.Location = new System.Drawing.Point(6, 137);
            this.Solo_do_not_dismount_EVER.Name = "Solo_do_not_dismount_EVER";
            this.Solo_do_not_dismount_EVER.Size = new System.Drawing.Size(130, 17);
            this.Solo_do_not_dismount_EVER.TabIndex = 27;
            this.Solo_do_not_dismount_EVER.Text = "Don\'t Dismount EVER";
            this.Solo_do_not_dismount_EVER.UseVisualStyleBackColor = true;
            this.Solo_do_not_dismount_EVER.CheckedChanged += new System.EventHandler(this.Solo_do_not_dismount_EVER_CheckedChanged);
            // 
            // Solo_do_not_dismount_ooc
            // 
            this.Solo_do_not_dismount_ooc.AutoSize = true;
            this.Solo_do_not_dismount_ooc.Location = new System.Drawing.Point(6, 113);
            this.Solo_do_not_dismount_ooc.Name = "Solo_do_not_dismount_ooc";
            this.Solo_do_not_dismount_ooc.Size = new System.Drawing.Size(169, 17);
            this.Solo_do_not_dismount_ooc.TabIndex = 26;
            this.Solo_do_not_dismount_ooc.Text = "Don\'t Dismount Out of Combat";
            this.Solo_do_not_dismount_ooc.UseVisualStyleBackColor = true;
            this.Solo_do_not_dismount_ooc.CheckedChanged += new System.EventHandler(this.Solo_do_not_dismount_ooc_CheckedChanged);
            // 
            // Solo_wanna_move_to_HoJ
            // 
            this.Solo_wanna_move_to_HoJ.AutoSize = true;
            this.Solo_wanna_move_to_HoJ.Location = new System.Drawing.Point(7, 66);
            this.Solo_wanna_move_to_HoJ.Name = "Solo_wanna_move_to_HoJ";
            this.Solo_wanna_move_to_HoJ.Size = new System.Drawing.Size(201, 17);
            this.Solo_wanna_move_to_HoJ.TabIndex = 25;
            this.Solo_wanna_move_to_HoJ.Text = "Move in Range to Hammer of Justice";
            this.Solo_wanna_move_to_HoJ.UseVisualStyleBackColor = true;
            this.Solo_wanna_move_to_HoJ.CheckedChanged += new System.EventHandler(this.Solo_wanna_move_to_HoJ_CheckedChanged);
            // 
            // Solo_wanna_mount
            // 
            this.Solo_wanna_mount.AutoSize = true;
            this.Solo_wanna_mount.Location = new System.Drawing.Point(6, 19);
            this.Solo_wanna_mount.Name = "Solo_wanna_mount";
            this.Solo_wanna_mount.Size = new System.Drawing.Size(73, 17);
            this.Solo_wanna_mount.TabIndex = 23;
            this.Solo_wanna_mount.Text = "Mount Up";
            this.Solo_wanna_mount.UseVisualStyleBackColor = true;
            this.Solo_wanna_mount.CheckedChanged += new System.EventHandler(this.Solo_wanna_mount_CheckedChanged);
            // 
            // Solo_wanna_move_to_heal
            // 
            this.Solo_wanna_move_to_heal.AutoSize = true;
            this.Solo_wanna_move_to_heal.Location = new System.Drawing.Point(6, 42);
            this.Solo_wanna_move_to_heal.Name = "Solo_wanna_move_to_heal";
            this.Solo_wanna_move_to_heal.Size = new System.Drawing.Size(136, 17);
            this.Solo_wanna_move_to_heal.TabIndex = 24;
            this.Solo_wanna_move_to_heal.Text = "Move in Range to Heal";
            this.Solo_wanna_move_to_heal.UseVisualStyleBackColor = true;
            this.Solo_wanna_move_to_heal.CheckedChanged += new System.EventHandler(this.Solo_wanna_move_to_heal_CheckedChanged);
            // 
            // Solo_generalGB
            // 
            this.Solo_generalGB.Controls.Add(this.Solo_rest_if_mana_belowLB);
            this.Solo_generalGB.Controls.Add(this.Solo_rest_if_mana_below);
            this.Solo_generalGB.Controls.Add(this.Solo_wanna_buff);
            this.Solo_generalGB.Location = new System.Drawing.Point(12, 19);
            this.Solo_generalGB.Name = "Solo_generalGB";
            this.Solo_generalGB.Size = new System.Drawing.Size(307, 66);
            this.Solo_generalGB.TabIndex = 16;
            this.Solo_generalGB.TabStop = false;
            this.Solo_generalGB.Text = "General";
            // 
            // Solo_rest_if_mana_belowLB
            // 
            this.Solo_rest_if_mana_belowLB.AutoSize = true;
            this.Solo_rest_if_mana_belowLB.Location = new System.Drawing.Point(64, 44);
            this.Solo_rest_if_mana_belowLB.Name = "Solo_rest_if_mana_belowLB";
            this.Solo_rest_if_mana_belowLB.Size = new System.Drawing.Size(101, 13);
            this.Solo_rest_if_mana_belowLB.TabIndex = 6;
            this.Solo_rest_if_mana_belowLB.Text = "Rest at this Mana %";
            // 
            // Solo_rest_if_mana_below
            // 
            this.Solo_rest_if_mana_below.Location = new System.Drawing.Point(5, 41);
            this.Solo_rest_if_mana_below.Name = "Solo_rest_if_mana_below";
            this.Solo_rest_if_mana_below.Size = new System.Drawing.Size(53, 20);
            this.Solo_rest_if_mana_below.TabIndex = 5;
            this.Solo_rest_if_mana_below.ValueChanged += new System.EventHandler(this.Solo_rest_if_mana_below_ValueChanged);
            // 
            // Solo_wanna_buff
            // 
            this.Solo_wanna_buff.AutoSize = true;
            this.Solo_wanna_buff.Location = new System.Drawing.Point(5, 19);
            this.Solo_wanna_buff.Name = "Solo_wanna_buff";
            this.Solo_wanna_buff.Size = new System.Drawing.Size(86, 17);
            this.Solo_wanna_buff.TabIndex = 4;
            this.Solo_wanna_buff.Text = "Enable Buffs";
            this.Solo_wanna_buff.UseVisualStyleBackColor = true;
            this.Solo_wanna_buff.CheckedChanged += new System.EventHandler(this.Solo_wanna_buff_CheckedChanged);
            // 
            // Solo_dpsGB
            // 
            this.Solo_dpsGB.Controls.Add(this.Solo_wanna_Judge);
            this.Solo_dpsGB.Controls.Add(this.Solo_wanna_CS);
            this.Solo_dpsGB.Controls.Add(this.Solo_wanna_HoW);
            this.Solo_dpsGB.Location = new System.Drawing.Point(822, 208);
            this.Solo_dpsGB.Name = "Solo_dpsGB";
            this.Solo_dpsGB.Size = new System.Drawing.Size(146, 100);
            this.Solo_dpsGB.TabIndex = 14;
            this.Solo_dpsGB.TabStop = false;
            this.Solo_dpsGB.Text = "DPS";
            // 
            // Solo_wanna_Judge
            // 
            this.Solo_wanna_Judge.AutoSize = true;
            this.Solo_wanna_Judge.Location = new System.Drawing.Point(17, 70);
            this.Solo_wanna_Judge.Name = "Solo_wanna_Judge";
            this.Solo_wanna_Judge.Size = new System.Drawing.Size(78, 17);
            this.Solo_wanna_Judge.TabIndex = 23;
            this.Solo_wanna_Judge.Text = "Judgement";
            this.Solo_wanna_Judge.UseVisualStyleBackColor = true;
            this.Solo_wanna_Judge.CheckedChanged += new System.EventHandler(this.Solo_wanna_Judge_CheckedChanged);
            // 
            // Solo_wanna_CS
            // 
            this.Solo_wanna_CS.AutoSize = true;
            this.Solo_wanna_CS.Location = new System.Drawing.Point(17, 24);
            this.Solo_wanna_CS.Name = "Solo_wanna_CS";
            this.Solo_wanna_CS.Size = new System.Drawing.Size(98, 17);
            this.Solo_wanna_CS.TabIndex = 6;
            this.Solo_wanna_CS.Text = "Crusader Strike";
            this.Solo_wanna_CS.UseVisualStyleBackColor = true;
            this.Solo_wanna_CS.CheckedChanged += new System.EventHandler(this.Solo_wanna_CS_CheckedChanged);
            // 
            // Solo_wanna_HoW
            // 
            this.Solo_wanna_HoW.AutoSize = true;
            this.Solo_wanna_HoW.Location = new System.Drawing.Point(17, 47);
            this.Solo_wanna_HoW.Name = "Solo_wanna_HoW";
            this.Solo_wanna_HoW.Size = new System.Drawing.Size(109, 17);
            this.Solo_wanna_HoW.TabIndex = 20;
            this.Solo_wanna_HoW.Text = "Hammer of Wrath";
            this.Solo_wanna_HoW.UseVisualStyleBackColor = true;
            this.Solo_wanna_HoW.CheckedChanged += new System.EventHandler(this.Solo_wanna_HoW_CheckedChanged);
            // 
            // Solo_ohshitbuttonGB
            // 
            this.Solo_ohshitbuttonGB.Controls.Add(this.Solo_wanna_lifeblood);
            this.Solo_ohshitbuttonGB.Controls.Add(this.Solo_min_ohshitbutton_activatorLB);
            this.Solo_ohshitbuttonGB.Controls.Add(this.Solo_min_ohshitbutton_activator);
            this.Solo_ohshitbuttonGB.Controls.Add(this.Solo_wanna_GotAK);
            this.Solo_ohshitbuttonGB.Controls.Add(this.Solo_wanna_AW);
            this.Solo_ohshitbuttonGB.Controls.Add(this.Solo_wanna_DF);
            this.Solo_ohshitbuttonGB.Location = new System.Drawing.Point(580, 19);
            this.Solo_ohshitbuttonGB.Name = "Solo_ohshitbuttonGB";
            this.Solo_ohshitbuttonGB.Size = new System.Drawing.Size(236, 145);
            this.Solo_ohshitbuttonGB.TabIndex = 13;
            this.Solo_ohshitbuttonGB.TabStop = false;
            this.Solo_ohshitbuttonGB.Text = "Oh Shit! Buttons";
            // 
            // Solo_wanna_lifeblood
            // 
            this.Solo_wanna_lifeblood.AutoSize = true;
            this.Solo_wanna_lifeblood.Location = new System.Drawing.Point(6, 47);
            this.Solo_wanna_lifeblood.Name = "Solo_wanna_lifeblood";
            this.Solo_wanna_lifeblood.Size = new System.Drawing.Size(69, 17);
            this.Solo_wanna_lifeblood.TabIndex = 11;
            this.Solo_wanna_lifeblood.Text = "Lifeblood";
            this.Solo_wanna_lifeblood.UseVisualStyleBackColor = true;
            this.Solo_wanna_lifeblood.CheckedChanged += new System.EventHandler(this.Solo_wanna_lifeblood_CheckedChanged);
            // 
            // Solo_min_ohshitbutton_activatorLB
            // 
            this.Solo_min_ohshitbutton_activatorLB.AutoSize = true;
            this.Solo_min_ohshitbutton_activatorLB.Location = new System.Drawing.Point(63, 24);
            this.Solo_min_ohshitbutton_activatorLB.Name = "Solo_min_ohshitbutton_activatorLB";
            this.Solo_min_ohshitbutton_activatorLB.Size = new System.Drawing.Size(135, 13);
            this.Solo_min_ohshitbutton_activatorLB.TabIndex = 10;
            this.Solo_min_ohshitbutton_activatorLB.Text = "Press if someone is this low";
            // 
            // Solo_min_ohshitbutton_activator
            // 
            this.Solo_min_ohshitbutton_activator.Location = new System.Drawing.Point(6, 20);
            this.Solo_min_ohshitbutton_activator.Name = "Solo_min_ohshitbutton_activator";
            this.Solo_min_ohshitbutton_activator.Size = new System.Drawing.Size(50, 20);
            this.Solo_min_ohshitbutton_activator.TabIndex = 9;
            this.Solo_min_ohshitbutton_activator.ValueChanged += new System.EventHandler(this.Solo_min_ohshitbutton_activator_ValueChanged);
            // 
            // Solo_wanna_GotAK
            // 
            this.Solo_wanna_GotAK.AutoSize = true;
            this.Solo_wanna_GotAK.Location = new System.Drawing.Point(6, 122);
            this.Solo_wanna_GotAK.Name = "Solo_wanna_GotAK";
            this.Solo_wanna_GotAK.Size = new System.Drawing.Size(167, 17);
            this.Solo_wanna_GotAK.TabIndex = 8;
            this.Solo_wanna_GotAK.Text = "Guardian of the Ancient Kings";
            this.Solo_wanna_GotAK.UseVisualStyleBackColor = true;
            this.Solo_wanna_GotAK.CheckedChanged += new System.EventHandler(this.Solo_wanna_GotAK_CheckedChanged);
            // 
            // Solo_wanna_AW
            // 
            this.Solo_wanna_AW.AutoSize = true;
            this.Solo_wanna_AW.Location = new System.Drawing.Point(6, 70);
            this.Solo_wanna_AW.Name = "Solo_wanna_AW";
            this.Solo_wanna_AW.Size = new System.Drawing.Size(103, 17);
            this.Solo_wanna_AW.TabIndex = 3;
            this.Solo_wanna_AW.Text = "Avenging Wrath";
            this.Solo_wanna_AW.UseVisualStyleBackColor = true;
            this.Solo_wanna_AW.CheckedChanged += new System.EventHandler(this.Solo_wanna_AW_CheckedChanged);
            // 
            // Solo_wanna_DF
            // 
            this.Solo_wanna_DF.AutoSize = true;
            this.Solo_wanna_DF.Location = new System.Drawing.Point(6, 95);
            this.Solo_wanna_DF.Name = "Solo_wanna_DF";
            this.Solo_wanna_DF.Size = new System.Drawing.Size(86, 17);
            this.Solo_wanna_DF.TabIndex = 7;
            this.Solo_wanna_DF.Text = "Divine Favor";
            this.Solo_wanna_DF.UseVisualStyleBackColor = true;
            this.Solo_wanna_DF.CheckedChanged += new System.EventHandler(this.Solo_wanna_DF_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.PVE_optimizeGB);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.PVE_bless_selection);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.PVE_tankselection);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.PVE_CleanseGB);
            this.tabPage2.Controls.Add(this.PVE_interruptersGB);
            this.tabPage2.Controls.Add(this.PVE_Healing);
            this.tabPage2.Controls.Add(this.PVE_mana_management);
            this.tabPage2.Controls.Add(this.PVE_HRGB);
            this.tabPage2.Controls.Add(this.PVE_emergencyGB);
            this.tabPage2.Controls.Add(this.PVE_auraselctGB);
            this.tabPage2.Controls.Add(this.PVE_racialsGB);
            this.tabPage2.Controls.Add(this.PVE_movementGB);
            this.tabPage2.Controls.Add(this.PVE_generalGB);
            this.tabPage2.Controls.Add(this.PVE_dpsGB);
            this.tabPage2.Controls.Add(this.PVE_ohshitbuttonGB);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(994, 653);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Dungeon";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // PVE_optimizeGB
            // 
            this.PVE_optimizeGB.Controls.Add(this.PVE_intellywait);
            this.PVE_optimizeGB.Controls.Add(this.PVE_accurancy);
            this.PVE_optimizeGB.Controls.Add(this.PVE_speed);
            this.PVE_optimizeGB.Location = new System.Drawing.Point(580, 462);
            this.PVE_optimizeGB.Name = "PVE_optimizeGB";
            this.PVE_optimizeGB.Size = new System.Drawing.Size(200, 100);
            this.PVE_optimizeGB.TabIndex = 58;
            this.PVE_optimizeGB.TabStop = false;
            this.PVE_optimizeGB.Text = "Optimize the CC for";
            this.PVE_optimizeGB.Enter += new System.EventHandler(this.PVE_optimizeGB_Enter);
            // 
            // PVE_intellywait
            // 
            this.PVE_intellywait.AutoSize = true;
            this.PVE_intellywait.Location = new System.Drawing.Point(7, 68);
            this.PVE_intellywait.Name = "PVE_intellywait";
            this.PVE_intellywait.Size = new System.Drawing.Size(159, 17);
            this.PVE_intellywait.TabIndex = 2;
            this.PVE_intellywait.TabStop = true;
            this.PVE_intellywait.Tag = "optimize";
            this.PVE_intellywait.Text = "IntellyWait (combat sistem 6)";
            this.PVE_intellywait.UseVisualStyleBackColor = true;
            this.PVE_intellywait.CheckedChanged += new System.EventHandler(this.PVE_intellywait_CheckedChanged);
            // 
            // PVE_accurancy
            // 
            this.PVE_accurancy.AutoSize = true;
            this.PVE_accurancy.Location = new System.Drawing.Point(7, 44);
            this.PVE_accurancy.Name = "PVE_accurancy";
            this.PVE_accurancy.Size = new System.Drawing.Size(161, 17);
            this.PVE_accurancy.TabIndex = 1;
            this.PVE_accurancy.TabStop = true;
            this.PVE_accurancy.Tag = "optimize";
            this.PVE_accurancy.Text = "Accurancy (combat sistem 5)";
            this.PVE_accurancy.UseVisualStyleBackColor = true;
            this.PVE_accurancy.CheckedChanged += new System.EventHandler(this.PVE_accurancy_CheckedChanged);
            // 
            // PVE_speed
            // 
            this.PVE_speed.AutoSize = true;
            this.PVE_speed.Location = new System.Drawing.Point(8, 20);
            this.PVE_speed.Name = "PVE_speed";
            this.PVE_speed.Size = new System.Drawing.Size(141, 17);
            this.PVE_speed.TabIndex = 0;
            this.PVE_speed.TabStop = true;
            this.PVE_speed.Tag = "optimize";
            this.PVE_speed.Text = "Speed (combat sistem 4)";
            this.PVE_speed.UseVisualStyleBackColor = true;
            this.PVE_speed.CheckedChanged += new System.EventHandler(this.PVE_speed_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label80);
            this.groupBox1.Controls.Add(this.PVE_tank_healing_priority_multiplier);
            this.groupBox1.Controls.Add(this.label65);
            this.groupBox1.Controls.Add(this.PVE_healing_tank_priority);
            this.groupBox1.Location = new System.Drawing.Point(580, 356);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 100);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tank Healing Priority";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Location = new System.Drawing.Point(68, 54);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(48, 13);
            this.label80.TabIndex = 3;
            this.label80.Text = "Multiplier";
            // 
            // PVE_tank_healing_priority_multiplier
            // 
            this.PVE_tank_healing_priority_multiplier.Location = new System.Drawing.Point(15, 47);
            this.PVE_tank_healing_priority_multiplier.Name = "PVE_tank_healing_priority_multiplier";
            this.PVE_tank_healing_priority_multiplier.Size = new System.Drawing.Size(45, 20);
            this.PVE_tank_healing_priority_multiplier.TabIndex = 2;
            this.PVE_tank_healing_priority_multiplier.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PVE_tank_healing_priority_multiplier.ValueChanged += new System.EventHandler(this.PVE_tank_healing_priority_multiplier_ValueChanged);
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(68, 26);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(150, 13);
            this.label65.TabIndex = 1;
            this.label65.Text = "From 0% to 100% more priority ";
            // 
            // PVE_healing_tank_priority
            // 
            this.PVE_healing_tank_priority.Location = new System.Drawing.Point(15, 20);
            this.PVE_healing_tank_priority.Name = "PVE_healing_tank_priority";
            this.PVE_healing_tank_priority.Size = new System.Drawing.Size(45, 20);
            this.PVE_healing_tank_priority.TabIndex = 0;
            this.PVE_healing_tank_priority.ValueChanged += new System.EventHandler(this.PVE_healing_tank_priority_ValueChanged);
            // 
            // PVE_bless_selection
            // 
            this.PVE_bless_selection.Controls.Add(this.PVE_bless_type_disabledRB);
            this.PVE_bless_selection.Controls.Add(this.PVE_bless_type_MightRB);
            this.PVE_bless_selection.Controls.Add(this.PVE_bless_type_KingRB);
            this.PVE_bless_selection.Controls.Add(this.PVE_bless_type_autoRB);
            this.PVE_bless_selection.Location = new System.Drawing.Point(822, 409);
            this.PVE_bless_selection.Name = "PVE_bless_selection";
            this.PVE_bless_selection.Size = new System.Drawing.Size(146, 118);
            this.PVE_bless_selection.TabIndex = 56;
            this.PVE_bless_selection.TabStop = false;
            this.PVE_bless_selection.Text = "Bless Selection";
            this.PVE_bless_selection.Enter += new System.EventHandler(this.PVE_bless_selection_Enter);
            // 
            // PVE_bless_type_disabledRB
            // 
            this.PVE_bless_type_disabledRB.AutoSize = true;
            this.PVE_bless_type_disabledRB.Location = new System.Drawing.Point(7, 92);
            this.PVE_bless_type_disabledRB.Name = "PVE_bless_type_disabledRB";
            this.PVE_bless_type_disabledRB.Size = new System.Drawing.Size(66, 17);
            this.PVE_bless_type_disabledRB.TabIndex = 3;
            this.PVE_bless_type_disabledRB.TabStop = true;
            this.PVE_bless_type_disabledRB.Tag = "Bless";
            this.PVE_bless_type_disabledRB.Text = "Disabled";
            this.PVE_bless_type_disabledRB.UseVisualStyleBackColor = true;
            this.PVE_bless_type_disabledRB.CheckedChanged += new System.EventHandler(this.PVE_bless_type_disabledRB_CheckedChanged);
            // 
            // PVE_bless_type_MightRB
            // 
            this.PVE_bless_type_MightRB.AutoSize = true;
            this.PVE_bless_type_MightRB.Location = new System.Drawing.Point(7, 68);
            this.PVE_bless_type_MightRB.Name = "PVE_bless_type_MightRB";
            this.PVE_bless_type_MightRB.Size = new System.Drawing.Size(51, 17);
            this.PVE_bless_type_MightRB.TabIndex = 2;
            this.PVE_bless_type_MightRB.TabStop = true;
            this.PVE_bless_type_MightRB.Tag = "Bless";
            this.PVE_bless_type_MightRB.Text = "Might";
            this.PVE_bless_type_MightRB.UseVisualStyleBackColor = true;
            this.PVE_bless_type_MightRB.CheckedChanged += new System.EventHandler(this.PVE_bless_type_MightRB_CheckedChanged);
            // 
            // PVE_bless_type_KingRB
            // 
            this.PVE_bless_type_KingRB.AutoSize = true;
            this.PVE_bless_type_KingRB.Location = new System.Drawing.Point(7, 44);
            this.PVE_bless_type_KingRB.Name = "PVE_bless_type_KingRB";
            this.PVE_bless_type_KingRB.Size = new System.Drawing.Size(46, 17);
            this.PVE_bless_type_KingRB.TabIndex = 1;
            this.PVE_bless_type_KingRB.TabStop = true;
            this.PVE_bless_type_KingRB.Tag = "Bless";
            this.PVE_bless_type_KingRB.Text = "King";
            this.PVE_bless_type_KingRB.UseVisualStyleBackColor = true;
            this.PVE_bless_type_KingRB.CheckedChanged += new System.EventHandler(this.PVE_bless_type_KingRB_CheckedChanged);
            // 
            // PVE_bless_type_autoRB
            // 
            this.PVE_bless_type_autoRB.AutoSize = true;
            this.PVE_bless_type_autoRB.Location = new System.Drawing.Point(7, 20);
            this.PVE_bless_type_autoRB.Name = "PVE_bless_type_autoRB";
            this.PVE_bless_type_autoRB.Size = new System.Drawing.Size(47, 17);
            this.PVE_bless_type_autoRB.TabIndex = 0;
            this.PVE_bless_type_autoRB.TabStop = true;
            this.PVE_bless_type_autoRB.Tag = "Bless";
            this.PVE_bless_type_autoRB.Text = "Auto";
            this.PVE_bless_type_autoRB.UseVisualStyleBackColor = true;
            this.PVE_bless_type_autoRB.CheckedChanged += new System.EventHandler(this.PVE_bless_type_autoRB_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.PVE_stop_DL_if_above);
            this.groupBox2.Location = new System.Drawing.Point(578, 301);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(236, 49);
            this.groupBox2.TabIndex = 55;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Overhealing Protection";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(67, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(171, 13);
            this.label17.TabIndex = 56;
            this.label17.Text = "Cancel Divine Light if Above this %";
            // 
            // PVE_stop_DL_if_above
            // 
            this.PVE_stop_DL_if_above.Location = new System.Drawing.Point(8, 19);
            this.PVE_stop_DL_if_above.Name = "PVE_stop_DL_if_above";
            this.PVE_stop_DL_if_above.Size = new System.Drawing.Size(54, 20);
            this.PVE_stop_DL_if_above.TabIndex = 55;
            this.PVE_stop_DL_if_above.ValueChanged += new System.EventHandler(this.PVE_stop_DL_if_above_ValueChanged);
            // 
            // PVE_tankselection
            // 
            this.PVE_tankselection.Controls.Add(this.PVE_get_tank_from_lua);
            this.PVE_tankselection.Controls.Add(this.PVE_get_tank_from_focus);
            this.PVE_tankselection.Location = new System.Drawing.Point(12, 91);
            this.PVE_tankselection.Name = "PVE_tankselection";
            this.PVE_tankselection.Size = new System.Drawing.Size(307, 69);
            this.PVE_tankselection.TabIndex = 54;
            this.PVE_tankselection.TabStop = false;
            this.PVE_tankselection.Text = "Tank Selection";
            // 
            // PVE_get_tank_from_lua
            // 
            this.PVE_get_tank_from_lua.AutoSize = true;
            this.PVE_get_tank_from_lua.Location = new System.Drawing.Point(8, 42);
            this.PVE_get_tank_from_lua.Name = "PVE_get_tank_from_lua";
            this.PVE_get_tank_from_lua.Size = new System.Drawing.Size(156, 17);
            this.PVE_get_tank_from_lua.TabIndex = 2;
            this.PVE_get_tank_from_lua.Text = "Use the ingame RoleCheck";
            this.PVE_get_tank_from_lua.UseVisualStyleBackColor = true;
            this.PVE_get_tank_from_lua.CheckedChanged += new System.EventHandler(this.PVE_get_tank_from_lua_CheckedChanged);
            // 
            // PVE_get_tank_from_focus
            // 
            this.PVE_get_tank_from_focus.AutoSize = true;
            this.PVE_get_tank_from_focus.Location = new System.Drawing.Point(8, 19);
            this.PVE_get_tank_from_focus.Name = "PVE_get_tank_from_focus";
            this.PVE_get_tank_from_focus.Size = new System.Drawing.Size(179, 17);
            this.PVE_get_tank_from_focus.TabIndex = 1;
            this.PVE_get_tank_from_focus.Text = "Import the Tank from your Focus";
            this.PVE_get_tank_from_focus.UseVisualStyleBackColor = true;
            this.PVE_get_tank_from_focus.CheckedChanged += new System.EventHandler(this.PVE_get_tank_from_focus_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PVE_advanced_option);
            this.panel1.Controls.Add(this.PVE_advanced);
            this.panel1.Location = new System.Drawing.Point(12, 437);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(313, 202);
            this.panel1.TabIndex = 53;
            // 
            // PVE_advanced_option
            // 
            this.PVE_advanced_option.AutoSize = true;
            this.PVE_advanced_option.Location = new System.Drawing.Point(6, 3);
            this.PVE_advanced_option.Name = "PVE_advanced_option";
            this.PVE_advanced_option.Size = new System.Drawing.Size(144, 17);
            this.PVE_advanced_option.TabIndex = 34;
            this.PVE_advanced_option.Text = "Show Advanced Options";
            this.PVE_advanced_option.UseVisualStyleBackColor = true;
            this.PVE_advanced_option.CheckedChanged += new System.EventHandler(this.PVE_advanced_option_CheckedChanged);
            // 
            // PVE_advanced
            // 
            this.PVE_advanced.Controls.Add(this.label1);
            this.PVE_advanced.Controls.Add(this.PVE_max_healing_distance);
            this.PVE_advanced.Controls.Add(this.PVE_wanna_target);
            this.PVE_advanced.Controls.Add(this.groupBox3);
            this.PVE_advanced.Controls.Add(this.PVE_wanna_face);
            this.PVE_advanced.Location = new System.Drawing.Point(6, 26);
            this.PVE_advanced.Name = "PVE_advanced";
            this.PVE_advanced.Size = new System.Drawing.Size(301, 173);
            this.PVE_advanced.TabIndex = 33;
            this.PVE_advanced.TabStop = false;
            this.PVE_advanced.Text = "Advanced Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Ignore unit more distant than this";
            // 
            // PVE_max_healing_distance
            // 
            this.PVE_max_healing_distance.Location = new System.Drawing.Point(13, 117);
            this.PVE_max_healing_distance.Name = "PVE_max_healing_distance";
            this.PVE_max_healing_distance.Size = new System.Drawing.Size(52, 20);
            this.PVE_max_healing_distance.TabIndex = 32;
            this.PVE_max_healing_distance.ValueChanged += new System.EventHandler(this.PVE_max_healing_distance_ValueChanged);
            // 
            // PVE_wanna_target
            // 
            this.PVE_wanna_target.AutoSize = true;
            this.PVE_wanna_target.Location = new System.Drawing.Point(13, 101);
            this.PVE_wanna_target.Name = "PVE_wanna_target";
            this.PVE_wanna_target.Size = new System.Drawing.Size(216, 17);
            this.PVE_wanna_target.TabIndex = 27;
            this.PVE_wanna_target.Text = "Target if no target (prevent HBCore bug)";
            this.PVE_wanna_target.UseVisualStyleBackColor = true;
            this.PVE_wanna_target.CheckedChanged += new System.EventHandler(this.PVE_wanna_target_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.PVE_do_not_heal_above);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(13, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 53);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Do not consider People above this health";
            // 
            // PVE_do_not_heal_above
            // 
            this.PVE_do_not_heal_above.Location = new System.Drawing.Point(6, 19);
            this.PVE_do_not_heal_above.Name = "PVE_do_not_heal_above";
            this.PVE_do_not_heal_above.Size = new System.Drawing.Size(46, 20);
            this.PVE_do_not_heal_above.TabIndex = 29;
            this.PVE_do_not_heal_above.ValueChanged += new System.EventHandler(this.PVE_do_not_heal_above_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "DO NOT MESS WITH THIS";
            // 
            // PVE_wanna_face
            // 
            this.PVE_wanna_face.AutoSize = true;
            this.PVE_wanna_face.Location = new System.Drawing.Point(13, 78);
            this.PVE_wanna_face.Name = "PVE_wanna_face";
            this.PVE_wanna_face.Size = new System.Drawing.Size(166, 17);
            this.PVE_wanna_face.TabIndex = 11;
            this.PVE_wanna_face.Text = "Face the target when needed";
            this.PVE_wanna_face.UseVisualStyleBackColor = true;
            this.PVE_wanna_face.CheckedChanged += new System.EventHandler(this.PVE_wanna_face_CheckedChanged);
            // 
            // PVE_CleanseGB
            // 
            this.PVE_CleanseGB.Controls.Add(this.PVE_cleanse_only_self_and_tank);
            this.PVE_CleanseGB.Controls.Add(this.PVE_wanna_cleanse);
            this.PVE_CleanseGB.Controls.Add(this.PVE_wanna_urgent_cleanse);
            this.PVE_CleanseGB.Location = new System.Drawing.Point(822, 314);
            this.PVE_CleanseGB.Name = "PVE_CleanseGB";
            this.PVE_CleanseGB.Size = new System.Drawing.Size(146, 95);
            this.PVE_CleanseGB.TabIndex = 52;
            this.PVE_CleanseGB.TabStop = false;
            this.PVE_CleanseGB.Text = "Cleanse";
            // 
            // PVE_cleanse_only_self_and_tank
            // 
            this.PVE_cleanse_only_self_and_tank.AutoSize = true;
            this.PVE_cleanse_only_self_and_tank.Location = new System.Drawing.Point(17, 65);
            this.PVE_cleanse_only_self_and_tank.Name = "PVE_cleanse_only_self_and_tank";
            this.PVE_cleanse_only_self_and_tank.Size = new System.Drawing.Size(134, 17);
            this.PVE_cleanse_only_self_and_tank.TabIndex = 28;
            this.PVE_cleanse_only_self_and_tank.Text = "But only Self and Tank";
            this.PVE_cleanse_only_self_and_tank.UseVisualStyleBackColor = true;
            this.PVE_cleanse_only_self_and_tank.CheckedChanged += new System.EventHandler(this.PVE_cleanse_only_self_and_tank_CheckedChanged);
            // 
            // PVE_wanna_cleanse
            // 
            this.PVE_wanna_cleanse.AutoSize = true;
            this.PVE_wanna_cleanse.Location = new System.Drawing.Point(7, 42);
            this.PVE_wanna_cleanse.Name = "PVE_wanna_cleanse";
            this.PVE_wanna_cleanse.Size = new System.Drawing.Size(64, 17);
            this.PVE_wanna_cleanse.TabIndex = 5;
            this.PVE_wanna_cleanse.Text = "Cleanse";
            this.PVE_wanna_cleanse.UseVisualStyleBackColor = true;
            this.PVE_wanna_cleanse.CheckedChanged += new System.EventHandler(this.PVE_wanna_cleanse_CheckedChanged);
            // 
            // PVE_wanna_urgent_cleanse
            // 
            this.PVE_wanna_urgent_cleanse.AutoSize = true;
            this.PVE_wanna_urgent_cleanse.Location = new System.Drawing.Point(7, 19);
            this.PVE_wanna_urgent_cleanse.Name = "PVE_wanna_urgent_cleanse";
            this.PVE_wanna_urgent_cleanse.Size = new System.Drawing.Size(135, 17);
            this.PVE_wanna_urgent_cleanse.TabIndex = 27;
            this.PVE_wanna_urgent_cleanse.Text = "Cleanse Urgents ASAP";
            this.PVE_wanna_urgent_cleanse.UseVisualStyleBackColor = true;
            this.PVE_wanna_urgent_cleanse.CheckedChanged += new System.EventHandler(this.PVE_wanna_urgent_cleanse_CheckedChanged);
            // 
            // PVE_interruptersGB
            // 
            this.PVE_interruptersGB.Controls.Add(this.PVE_wanna_HoJ);
            this.PVE_interruptersGB.Controls.Add(this.PVE_wanna_rebuke);
            this.PVE_interruptersGB.Location = new System.Drawing.Point(822, 125);
            this.PVE_interruptersGB.Name = "PVE_interruptersGB";
            this.PVE_interruptersGB.Size = new System.Drawing.Size(146, 77);
            this.PVE_interruptersGB.TabIndex = 51;
            this.PVE_interruptersGB.TabStop = false;
            this.PVE_interruptersGB.Text = "Interrupts";
            // 
            // PVE_wanna_HoJ
            // 
            this.PVE_wanna_HoJ.AutoSize = true;
            this.PVE_wanna_HoJ.Location = new System.Drawing.Point(9, 19);
            this.PVE_wanna_HoJ.Name = "PVE_wanna_HoJ";
            this.PVE_wanna_HoJ.Size = new System.Drawing.Size(113, 17);
            this.PVE_wanna_HoJ.TabIndex = 17;
            this.PVE_wanna_HoJ.Text = "Hammer of Justice";
            this.PVE_wanna_HoJ.UseVisualStyleBackColor = true;
            this.PVE_wanna_HoJ.CheckedChanged += new System.EventHandler(this.PVE_wanna_HoJ_CheckedChanged);
            // 
            // PVE_wanna_rebuke
            // 
            this.PVE_wanna_rebuke.AutoSize = true;
            this.PVE_wanna_rebuke.Location = new System.Drawing.Point(9, 43);
            this.PVE_wanna_rebuke.Name = "PVE_wanna_rebuke";
            this.PVE_wanna_rebuke.Size = new System.Drawing.Size(64, 17);
            this.PVE_wanna_rebuke.TabIndex = 26;
            this.PVE_wanna_rebuke.Text = "Rebuke";
            this.PVE_wanna_rebuke.UseVisualStyleBackColor = true;
            this.PVE_wanna_rebuke.CheckedChanged += new System.EventHandler(this.PVE_wanna_rebuke_CheckedChanged);
            // 
            // PVE_Healing
            // 
            this.PVE_Healing.Controls.Add(this.PVE_min_Inf_of_light_DL_hpLB);
            this.PVE_Healing.Controls.Add(this.PVE_min_Inf_of_light_DL_hp);
            this.PVE_Healing.Controls.Add(this.label4);
            this.PVE_Healing.Controls.Add(this.PVE_min_HL_hp);
            this.PVE_Healing.Controls.Add(this.label5);
            this.PVE_Healing.Controls.Add(this.PVE_min_FoL_hp);
            this.PVE_Healing.Controls.Add(this.label6);
            this.PVE_Healing.Controls.Add(this.PVE_min_DL_hp);
            this.PVE_Healing.Controls.Add(this.PVE_Inf_of_light_wanna_DL);
            this.PVE_Healing.Location = new System.Drawing.Point(325, 19);
            this.PVE_Healing.Name = "PVE_Healing";
            this.PVE_Healing.Size = new System.Drawing.Size(249, 160);
            this.PVE_Healing.TabIndex = 50;
            this.PVE_Healing.TabStop = false;
            this.PVE_Healing.Text = "Healing";
            // 
            // PVE_min_Inf_of_light_DL_hpLB
            // 
            this.PVE_min_Inf_of_light_DL_hpLB.AutoSize = true;
            this.PVE_min_Inf_of_light_DL_hpLB.Location = new System.Drawing.Point(72, 130);
            this.PVE_min_Inf_of_light_DL_hpLB.Name = "PVE_min_Inf_of_light_DL_hpLB";
            this.PVE_min_Inf_of_light_DL_hpLB.Size = new System.Drawing.Size(172, 13);
            this.PVE_min_Inf_of_light_DL_hpLB.TabIndex = 7;
            this.PVE_min_Inf_of_light_DL_hpLB.Text = "But only if the target is below this %";
            // 
            // PVE_min_Inf_of_light_DL_hp
            // 
            this.PVE_min_Inf_of_light_DL_hp.Location = new System.Drawing.Point(7, 124);
            this.PVE_min_Inf_of_light_DL_hp.Name = "PVE_min_Inf_of_light_DL_hp";
            this.PVE_min_Inf_of_light_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.PVE_min_Inf_of_light_DL_hp.TabIndex = 6;
            this.PVE_min_Inf_of_light_DL_hp.ValueChanged += new System.EventHandler(this.PVE_min_Inf_of_light_DL_hp_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Holy Light targets under this %";
            // 
            // PVE_min_HL_hp
            // 
            this.PVE_min_HL_hp.Location = new System.Drawing.Point(7, 20);
            this.PVE_min_HL_hp.Name = "PVE_min_HL_hp";
            this.PVE_min_HL_hp.Size = new System.Drawing.Size(56, 20);
            this.PVE_min_HL_hp.TabIndex = 4;
            this.PVE_min_HL_hp.ValueChanged += new System.EventHandler(this.PVE_min_HL_hp_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Flash of Light targets under this %";
            // 
            // PVE_min_FoL_hp
            // 
            this.PVE_min_FoL_hp.Location = new System.Drawing.Point(7, 74);
            this.PVE_min_FoL_hp.Name = "PVE_min_FoL_hp";
            this.PVE_min_FoL_hp.Size = new System.Drawing.Size(56, 20);
            this.PVE_min_FoL_hp.TabIndex = 2;
            this.PVE_min_FoL_hp.ValueChanged += new System.EventHandler(this.PVE_min_FoL_hp_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(68, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Divine Light targets under this %";
            // 
            // PVE_min_DL_hp
            // 
            this.PVE_min_DL_hp.Location = new System.Drawing.Point(6, 46);
            this.PVE_min_DL_hp.Name = "PVE_min_DL_hp";
            this.PVE_min_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.PVE_min_DL_hp.TabIndex = 0;
            this.PVE_min_DL_hp.ValueChanged += new System.EventHandler(this.PVE_min_DL_hp_ValueChanged);
            // 
            // PVE_Inf_of_light_wanna_DL
            // 
            this.PVE_Inf_of_light_wanna_DL.AutoSize = true;
            this.PVE_Inf_of_light_wanna_DL.Location = new System.Drawing.Point(5, 100);
            this.PVE_Inf_of_light_wanna_DL.Name = "PVE_Inf_of_light_wanna_DL";
            this.PVE_Inf_of_light_wanna_DL.Size = new System.Drawing.Size(235, 17);
            this.PVE_Inf_of_light_wanna_DL.TabIndex = 2;
            this.PVE_Inf_of_light_wanna_DL.Text = "Use Divine Light when Infusion of Light proc";
            this.PVE_Inf_of_light_wanna_DL.UseVisualStyleBackColor = true;
            this.PVE_Inf_of_light_wanna_DL.CheckedChanged += new System.EventHandler(this.PVE_Inf_of_light_wanna_DL_CheckedChanged);
            // 
            // PVE_mana_management
            // 
            this.PVE_mana_management.Controls.Add(this.label7);
            this.PVE_mana_management.Controls.Add(this.PVE_use_mana_rec_trinket_every);
            this.PVE_mana_management.Controls.Add(this.label8);
            this.PVE_mana_management.Controls.Add(this.PVE_min_mana_rec_trinket);
            this.PVE_mana_management.Controls.Add(this.label9);
            this.PVE_mana_management.Controls.Add(this.PVE_min_Divine_Plea_mana);
            this.PVE_mana_management.Controls.Add(this.label10);
            this.PVE_mana_management.Controls.Add(this.PVE_mana_judge);
            this.PVE_mana_management.Location = new System.Drawing.Point(12, 166);
            this.PVE_mana_management.Name = "PVE_mana_management";
            this.PVE_mana_management.Size = new System.Drawing.Size(307, 126);
            this.PVE_mana_management.TabIndex = 49;
            this.PVE_mana_management.TabStop = false;
            this.PVE_mana_management.Text = "Mana Management";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(49, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(186, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Activate your mana trinket every (NYI)";
            // 
            // PVE_use_mana_rec_trinket_every
            // 
            this.PVE_use_mana_rec_trinket_every.Location = new System.Drawing.Point(0, 100);
            this.PVE_use_mana_rec_trinket_every.Name = "PVE_use_mana_rec_trinket_every";
            this.PVE_use_mana_rec_trinket_every.Size = new System.Drawing.Size(43, 20);
            this.PVE_use_mana_rec_trinket_every.TabIndex = 6;
            this.PVE_use_mana_rec_trinket_every.ValueChanged += new System.EventHandler(this.PVE_use_mana_rec_trinket_every_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(49, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(208, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Activate your Mana trinket at Mana %(NYI)";
            // 
            // PVE_min_mana_rec_trinket
            // 
            this.PVE_min_mana_rec_trinket.Location = new System.Drawing.Point(0, 74);
            this.PVE_min_mana_rec_trinket.Name = "PVE_min_mana_rec_trinket";
            this.PVE_min_mana_rec_trinket.Size = new System.Drawing.Size(43, 20);
            this.PVE_min_mana_rec_trinket.TabIndex = 4;
            this.PVE_min_mana_rec_trinket.ValueChanged += new System.EventHandler(this.PVE_min_mana_rec_trinket_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(49, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Divine Plea at this mana";
            // 
            // PVE_min_Divine_Plea_mana
            // 
            this.PVE_min_Divine_Plea_mana.Location = new System.Drawing.Point(0, 47);
            this.PVE_min_Divine_Plea_mana.Name = "PVE_min_Divine_Plea_mana";
            this.PVE_min_Divine_Plea_mana.Size = new System.Drawing.Size(43, 20);
            this.PVE_min_Divine_Plea_mana.TabIndex = 2;
            this.PVE_min_Divine_Plea_mana.ValueChanged += new System.EventHandler(this.PVE_min_Divine_Plea_mana_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(49, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(242, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Judgement on Cooldown when mana is below this";
            // 
            // PVE_mana_judge
            // 
            this.PVE_mana_judge.Location = new System.Drawing.Point(0, 20);
            this.PVE_mana_judge.Name = "PVE_mana_judge";
            this.PVE_mana_judge.Size = new System.Drawing.Size(43, 20);
            this.PVE_mana_judge.TabIndex = 0;
            this.PVE_mana_judge.ValueChanged += new System.EventHandler(this.PVE_mana_judge_ValueChanged);
            // 
            // PVE_HRGB
            // 
            this.PVE_HRGB.Controls.Add(this.label11);
            this.PVE_HRGB.Controls.Add(this.PVE_min_player_inside_HR);
            this.PVE_HRGB.Controls.Add(this.label12);
            this.PVE_HRGB.Controls.Add(this.PVE_HR_how_much_health);
            this.PVE_HRGB.Controls.Add(this.label13);
            this.PVE_HRGB.Controls.Add(this.PVE_HR_how_far);
            this.PVE_HRGB.Controls.Add(this.PVE_wanna_HR);
            this.PVE_HRGB.Location = new System.Drawing.Point(12, 298);
            this.PVE_HRGB.Name = "PVE_HRGB";
            this.PVE_HRGB.Size = new System.Drawing.Size(307, 133);
            this.PVE_HRGB.TabIndex = 48;
            this.PVE_HRGB.TabStop = false;
            this.PVE_HRGB.Text = "Holy Radiance Settings";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(61, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(145, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Need this many people inside";
            // 
            // PVE_min_player_inside_HR
            // 
            this.PVE_min_player_inside_HR.Location = new System.Drawing.Point(7, 97);
            this.PVE_min_player_inside_HR.Name = "PVE_min_player_inside_HR";
            this.PVE_min_player_inside_HR.Size = new System.Drawing.Size(44, 20);
            this.PVE_min_player_inside_HR.TabIndex = 26;
            this.PVE_min_player_inside_HR.ValueChanged += new System.EventHandler(this.PVE_min_player_inside_HR_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(58, 76);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(196, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Consider unit with less than this Health%";
            // 
            // PVE_HR_how_much_health
            // 
            this.PVE_HR_how_much_health.Location = new System.Drawing.Point(7, 70);
            this.PVE_HR_how_much_health.Name = "PVE_HR_how_much_health";
            this.PVE_HR_how_much_health.Size = new System.Drawing.Size(44, 20);
            this.PVE_HR_how_much_health.TabIndex = 24;
            this.PVE_HR_how_much_health.ValueChanged += new System.EventHandler(this.PVE_HR_how_much_health_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(58, 49);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(221, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "Consider unit nearer then this to healed target";
            // 
            // PVE_HR_how_far
            // 
            this.PVE_HR_how_far.Location = new System.Drawing.Point(6, 43);
            this.PVE_HR_how_far.Name = "PVE_HR_how_far";
            this.PVE_HR_how_far.Size = new System.Drawing.Size(45, 20);
            this.PVE_HR_how_far.TabIndex = 22;
            this.PVE_HR_how_far.ValueChanged += new System.EventHandler(this.PVE_HR_how_far_ValueChanged);
            // 
            // PVE_wanna_HR
            // 
            this.PVE_wanna_HR.AutoSize = true;
            this.PVE_wanna_HR.Location = new System.Drawing.Point(6, 19);
            this.PVE_wanna_HR.Name = "PVE_wanna_HR";
            this.PVE_wanna_HR.Size = new System.Drawing.Size(96, 17);
            this.PVE_wanna_HR.TabIndex = 21;
            this.PVE_wanna_HR.Text = "Holy Radiance";
            this.PVE_wanna_HR.UseVisualStyleBackColor = true;
            this.PVE_wanna_HR.CheckedChanged += new System.EventHandler(this.PVE_wanna_HR_CheckedChanged);
            // 
            // PVE_emergencyGB
            // 
            this.PVE_emergencyGB.Controls.Add(this.PVE_min_mana_potion);
            this.PVE_emergencyGB.Controls.Add(this.PVE_min_LoH_hp);
            this.PVE_emergencyGB.Controls.Add(this.PVE_min_HoS_hp);
            this.PVE_emergencyGB.Controls.Add(this.PVE_min_HoP_hp);
            this.PVE_emergencyGB.Controls.Add(this.PVE_min_DS_hp);
            this.PVE_emergencyGB.Controls.Add(this.label14);
            this.PVE_emergencyGB.Controls.Add(this.PVE_min_DP_hp);
            this.PVE_emergencyGB.Controls.Add(this.PVE_wanna_mana_potion);
            this.PVE_emergencyGB.Controls.Add(this.PVE_wanna_LoH);
            this.PVE_emergencyGB.Controls.Add(this.PVE_wanna_DP);
            this.PVE_emergencyGB.Controls.Add(this.PVE_wanna_DS);
            this.PVE_emergencyGB.Controls.Add(this.PVE_wanna_HoS);
            this.PVE_emergencyGB.Controls.Add(this.PVE_wanna_HoP);
            this.PVE_emergencyGB.Location = new System.Drawing.Point(325, 185);
            this.PVE_emergencyGB.Name = "PVE_emergencyGB";
            this.PVE_emergencyGB.Size = new System.Drawing.Size(247, 182);
            this.PVE_emergencyGB.TabIndex = 45;
            this.PVE_emergencyGB.TabStop = false;
            this.PVE_emergencyGB.Text = "Emergency Buttons";
            // 
            // PVE_min_mana_potion
            // 
            this.PVE_min_mana_potion.Location = new System.Drawing.Point(120, 154);
            this.PVE_min_mana_potion.Name = "PVE_min_mana_potion";
            this.PVE_min_mana_potion.Size = new System.Drawing.Size(64, 20);
            this.PVE_min_mana_potion.TabIndex = 30;
            this.PVE_min_mana_potion.ValueChanged += new System.EventHandler(this.PVE_min_mana_potion_ValueChanged);
            // 
            // PVE_min_LoH_hp
            // 
            this.PVE_min_LoH_hp.Location = new System.Drawing.Point(120, 130);
            this.PVE_min_LoH_hp.Name = "PVE_min_LoH_hp";
            this.PVE_min_LoH_hp.Size = new System.Drawing.Size(64, 20);
            this.PVE_min_LoH_hp.TabIndex = 29;
            this.PVE_min_LoH_hp.ValueChanged += new System.EventHandler(this.PVE_min_LoH_hp_ValueChanged);
            // 
            // PVE_min_HoS_hp
            // 
            this.PVE_min_HoS_hp.Location = new System.Drawing.Point(120, 106);
            this.PVE_min_HoS_hp.Name = "PVE_min_HoS_hp";
            this.PVE_min_HoS_hp.Size = new System.Drawing.Size(64, 20);
            this.PVE_min_HoS_hp.TabIndex = 28;
            this.PVE_min_HoS_hp.ValueChanged += new System.EventHandler(this.PVE_min_HoS_hp_ValueChanged);
            // 
            // PVE_min_HoP_hp
            // 
            this.PVE_min_HoP_hp.Location = new System.Drawing.Point(120, 84);
            this.PVE_min_HoP_hp.Name = "PVE_min_HoP_hp";
            this.PVE_min_HoP_hp.Size = new System.Drawing.Size(64, 20);
            this.PVE_min_HoP_hp.TabIndex = 27;
            this.PVE_min_HoP_hp.ValueChanged += new System.EventHandler(this.PVE_min_HoP_hp_ValueChanged);
            // 
            // PVE_min_DS_hp
            // 
            this.PVE_min_DS_hp.Location = new System.Drawing.Point(120, 58);
            this.PVE_min_DS_hp.Name = "PVE_min_DS_hp";
            this.PVE_min_DS_hp.Size = new System.Drawing.Size(64, 20);
            this.PVE_min_DS_hp.TabIndex = 26;
            this.PVE_min_DS_hp.ValueChanged += new System.EventHandler(this.PVE_min_DS_hp_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(120, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "Use Below";
            // 
            // PVE_min_DP_hp
            // 
            this.PVE_min_DP_hp.Location = new System.Drawing.Point(120, 35);
            this.PVE_min_DP_hp.Name = "PVE_min_DP_hp";
            this.PVE_min_DP_hp.Size = new System.Drawing.Size(64, 20);
            this.PVE_min_DP_hp.TabIndex = 24;
            this.PVE_min_DP_hp.ValueChanged += new System.EventHandler(this.PVE_min_DP_hp_ValueChanged);
            // 
            // PVE_wanna_mana_potion
            // 
            this.PVE_wanna_mana_potion.AutoSize = true;
            this.PVE_wanna_mana_potion.Location = new System.Drawing.Point(6, 154);
            this.PVE_wanna_mana_potion.Name = "PVE_wanna_mana_potion";
            this.PVE_wanna_mana_potion.Size = new System.Drawing.Size(113, 17);
            this.PVE_wanna_mana_potion.TabIndex = 23;
            this.PVE_wanna_mana_potion.Text = "Mana Potion (NYI)";
            this.PVE_wanna_mana_potion.UseVisualStyleBackColor = true;
            this.PVE_wanna_mana_potion.CheckedChanged += new System.EventHandler(this.PVE_wanna_mana_potion_CheckedChanged);
            // 
            // PVE_wanna_LoH
            // 
            this.PVE_wanna_LoH.AutoSize = true;
            this.PVE_wanna_LoH.Location = new System.Drawing.Point(6, 130);
            this.PVE_wanna_LoH.Name = "PVE_wanna_LoH";
            this.PVE_wanna_LoH.Size = new System.Drawing.Size(87, 17);
            this.PVE_wanna_LoH.TabIndex = 23;
            this.PVE_wanna_LoH.Text = "Lay on Hand";
            this.PVE_wanna_LoH.UseVisualStyleBackColor = true;
            this.PVE_wanna_LoH.CheckedChanged += new System.EventHandler(this.PVE_wanna_LoH_CheckedChanged);
            // 
            // PVE_wanna_DP
            // 
            this.PVE_wanna_DP.AutoSize = true;
            this.PVE_wanna_DP.Location = new System.Drawing.Point(6, 38);
            this.PVE_wanna_DP.Name = "PVE_wanna_DP";
            this.PVE_wanna_DP.Size = new System.Drawing.Size(107, 17);
            this.PVE_wanna_DP.TabIndex = 8;
            this.PVE_wanna_DP.Text = "Divine Protection";
            this.PVE_wanna_DP.UseVisualStyleBackColor = true;
            this.PVE_wanna_DP.CheckedChanged += new System.EventHandler(this.PVE_wanna_DP_CheckedChanged);
            // 
            // PVE_wanna_DS
            // 
            this.PVE_wanna_DS.AutoSize = true;
            this.PVE_wanna_DS.Location = new System.Drawing.Point(6, 61);
            this.PVE_wanna_DS.Name = "PVE_wanna_DS";
            this.PVE_wanna_DS.Size = new System.Drawing.Size(88, 17);
            this.PVE_wanna_DS.TabIndex = 9;
            this.PVE_wanna_DS.Text = "Divine Shield";
            this.PVE_wanna_DS.UseVisualStyleBackColor = true;
            this.PVE_wanna_DS.CheckedChanged += new System.EventHandler(this.PVE_wanna_DS_CheckedChanged);
            // 
            // PVE_wanna_HoS
            // 
            this.PVE_wanna_HoS.AutoSize = true;
            this.PVE_wanna_HoS.Location = new System.Drawing.Point(6, 107);
            this.PVE_wanna_HoS.Name = "PVE_wanna_HoS";
            this.PVE_wanna_HoS.Size = new System.Drawing.Size(111, 17);
            this.PVE_wanna_HoS.TabIndex = 19;
            this.PVE_wanna_HoS.Text = "Hand of Salvation";
            this.PVE_wanna_HoS.UseVisualStyleBackColor = true;
            this.PVE_wanna_HoS.CheckedChanged += new System.EventHandler(this.PVE_wanna_HoS_CheckedChanged);
            // 
            // PVE_wanna_HoP
            // 
            this.PVE_wanna_HoP.AutoSize = true;
            this.PVE_wanna_HoP.Location = new System.Drawing.Point(6, 84);
            this.PVE_wanna_HoP.Name = "PVE_wanna_HoP";
            this.PVE_wanna_HoP.Size = new System.Drawing.Size(115, 17);
            this.PVE_wanna_HoP.TabIndex = 18;
            this.PVE_wanna_HoP.Text = "Hand of Protection";
            this.PVE_wanna_HoP.UseVisualStyleBackColor = true;
            this.PVE_wanna_HoP.CheckedChanged += new System.EventHandler(this.PVE_wanna_HoP_CheckedChanged);
            // 
            // PVE_auraselctGB
            // 
            this.PVE_auraselctGB.Controls.Add(this.PVE_DisabledRB);
            this.PVE_auraselctGB.Controls.Add(this.PVE_resistanceRB);
            this.PVE_auraselctGB.Controls.Add(this.PVE_concentrationRB);
            this.PVE_auraselctGB.Location = new System.Drawing.Point(822, 19);
            this.PVE_auraselctGB.Name = "PVE_auraselctGB";
            this.PVE_auraselctGB.Size = new System.Drawing.Size(146, 100);
            this.PVE_auraselctGB.TabIndex = 47;
            this.PVE_auraselctGB.TabStop = false;
            this.PVE_auraselctGB.Text = "Select Aura";
            this.PVE_auraselctGB.Enter += new System.EventHandler(this.PVE_auraselctGB_Enter);
            // 
            // PVE_DisabledRB
            // 
            this.PVE_DisabledRB.AutoSize = true;
            this.PVE_DisabledRB.Location = new System.Drawing.Point(7, 68);
            this.PVE_DisabledRB.Name = "PVE_DisabledRB";
            this.PVE_DisabledRB.Size = new System.Drawing.Size(66, 17);
            this.PVE_DisabledRB.TabIndex = 2;
            this.PVE_DisabledRB.TabStop = true;
            this.PVE_DisabledRB.Text = "Disabled";
            this.PVE_DisabledRB.UseVisualStyleBackColor = true;
            this.PVE_DisabledRB.CheckedChanged += new System.EventHandler(this.PVE_DisabledRB_CheckedChanged);
            // 
            // PVE_resistanceRB
            // 
            this.PVE_resistanceRB.AutoSize = true;
            this.PVE_resistanceRB.Location = new System.Drawing.Point(7, 44);
            this.PVE_resistanceRB.Name = "PVE_resistanceRB";
            this.PVE_resistanceRB.Size = new System.Drawing.Size(78, 17);
            this.PVE_resistanceRB.TabIndex = 1;
            this.PVE_resistanceRB.TabStop = true;
            this.PVE_resistanceRB.Text = "Resistance";
            this.PVE_resistanceRB.UseVisualStyleBackColor = true;
            this.PVE_resistanceRB.CheckedChanged += new System.EventHandler(this.PVE_resistanceRB_CheckedChanged);
            // 
            // PVE_concentrationRB
            // 
            this.PVE_concentrationRB.AutoSize = true;
            this.PVE_concentrationRB.Location = new System.Drawing.Point(7, 20);
            this.PVE_concentrationRB.Name = "PVE_concentrationRB";
            this.PVE_concentrationRB.Size = new System.Drawing.Size(91, 17);
            this.PVE_concentrationRB.TabIndex = 0;
            this.PVE_concentrationRB.TabStop = true;
            this.PVE_concentrationRB.Text = "Concentration";
            this.PVE_concentrationRB.UseVisualStyleBackColor = true;
            this.PVE_concentrationRB.CheckedChanged += new System.EventHandler(this.PVE_concentrationRB_CheckedChanged);
            // 
            // PVE_racialsGB
            // 
            this.PVE_racialsGB.Controls.Add(this.PVE_min_torrent_mana_perc);
            this.PVE_racialsGB.Controls.Add(this.PVE_min_stoneform);
            this.PVE_racialsGB.Controls.Add(this.PVE_min_gift_hp);
            this.PVE_racialsGB.Controls.Add(this.PVE_wanna_torrent);
            this.PVE_racialsGB.Controls.Add(this.PVE_wanna_stoneform);
            this.PVE_racialsGB.Controls.Add(this.PVE_wanna_everymanforhimself);
            this.PVE_racialsGB.Controls.Add(this.PVE_wanna_gift);
            this.PVE_racialsGB.Location = new System.Drawing.Point(578, 170);
            this.PVE_racialsGB.Name = "PVE_racialsGB";
            this.PVE_racialsGB.Size = new System.Drawing.Size(236, 124);
            this.PVE_racialsGB.TabIndex = 43;
            this.PVE_racialsGB.TabStop = false;
            this.PVE_racialsGB.Text = "Racials";
            // 
            // PVE_min_torrent_mana_perc
            // 
            this.PVE_min_torrent_mana_perc.Location = new System.Drawing.Point(172, 95);
            this.PVE_min_torrent_mana_perc.Name = "PVE_min_torrent_mana_perc";
            this.PVE_min_torrent_mana_perc.Size = new System.Drawing.Size(59, 20);
            this.PVE_min_torrent_mana_perc.TabIndex = 17;
            this.PVE_min_torrent_mana_perc.ValueChanged += new System.EventHandler(this.PVE_min_torrent_mana_perc_ValueChanged);
            // 
            // PVE_min_stoneform
            // 
            this.PVE_min_stoneform.Location = new System.Drawing.Point(172, 71);
            this.PVE_min_stoneform.Name = "PVE_min_stoneform";
            this.PVE_min_stoneform.Size = new System.Drawing.Size(59, 20);
            this.PVE_min_stoneform.TabIndex = 16;
            this.PVE_min_stoneform.ValueChanged += new System.EventHandler(this.PVE_min_stoneform_ValueChanged);
            // 
            // PVE_min_gift_hp
            // 
            this.PVE_min_gift_hp.Location = new System.Drawing.Point(172, 45);
            this.PVE_min_gift_hp.Name = "PVE_min_gift_hp";
            this.PVE_min_gift_hp.Size = new System.Drawing.Size(59, 20);
            this.PVE_min_gift_hp.TabIndex = 15;
            this.PVE_min_gift_hp.ValueChanged += new System.EventHandler(this.PVE_min_gift_hp_ValueChanged);
            // 
            // PVE_wanna_torrent
            // 
            this.PVE_wanna_torrent.AutoSize = true;
            this.PVE_wanna_torrent.Location = new System.Drawing.Point(16, 95);
            this.PVE_wanna_torrent.Name = "PVE_wanna_torrent";
            this.PVE_wanna_torrent.Size = new System.Drawing.Size(147, 17);
            this.PVE_wanna_torrent.TabIndex = 14;
            this.PVE_wanna_torrent.Text = "Arcane Torrent (for mana)";
            this.PVE_wanna_torrent.UseVisualStyleBackColor = true;
            this.PVE_wanna_torrent.CheckedChanged += new System.EventHandler(this.PVE_wanna_torrent_CheckedChanged);
            // 
            // PVE_wanna_stoneform
            // 
            this.PVE_wanna_stoneform.AutoSize = true;
            this.PVE_wanna_stoneform.Location = new System.Drawing.Point(16, 71);
            this.PVE_wanna_stoneform.Name = "PVE_wanna_stoneform";
            this.PVE_wanna_stoneform.Size = new System.Drawing.Size(74, 17);
            this.PVE_wanna_stoneform.TabIndex = 13;
            this.PVE_wanna_stoneform.Text = "Stoneform";
            this.PVE_wanna_stoneform.UseVisualStyleBackColor = true;
            this.PVE_wanna_stoneform.CheckedChanged += new System.EventHandler(this.PVE_wanna_stoneform_CheckedChanged);
            // 
            // PVE_wanna_everymanforhimself
            // 
            this.PVE_wanna_everymanforhimself.AutoSize = true;
            this.PVE_wanna_everymanforhimself.Location = new System.Drawing.Point(16, 24);
            this.PVE_wanna_everymanforhimself.Name = "PVE_wanna_everymanforhimself";
            this.PVE_wanna_everymanforhimself.Size = new System.Drawing.Size(129, 17);
            this.PVE_wanna_everymanforhimself.TabIndex = 10;
            this.PVE_wanna_everymanforhimself.Text = "Every Man for Himself";
            this.PVE_wanna_everymanforhimself.UseVisualStyleBackColor = true;
            this.PVE_wanna_everymanforhimself.CheckedChanged += new System.EventHandler(this.PVE_wanna_everymanforhimself_CheckedChanged);
            // 
            // PVE_wanna_gift
            // 
            this.PVE_wanna_gift.AutoSize = true;
            this.PVE_wanna_gift.Location = new System.Drawing.Point(16, 47);
            this.PVE_wanna_gift.Name = "PVE_wanna_gift";
            this.PVE_wanna_gift.Size = new System.Drawing.Size(104, 17);
            this.PVE_wanna_gift.TabIndex = 12;
            this.PVE_wanna_gift.Text = "Gift of the Naaru";
            this.PVE_wanna_gift.UseVisualStyleBackColor = true;
            this.PVE_wanna_gift.CheckedChanged += new System.EventHandler(this.PVE_wanna_gift_CheckedChanged);
            // 
            // PVE_movementGB
            // 
            this.PVE_movementGB.Controls.Add(this.PVE_do_not_dismount_EVER);
            this.PVE_movementGB.Controls.Add(this.PVE_do_not_dismount_ooc);
            this.PVE_movementGB.Controls.Add(this.PVE_wanna_move_to_HoJ);
            this.PVE_movementGB.Controls.Add(this.PVE_wanna_mount);
            this.PVE_movementGB.Controls.Add(this.PVE_wanna_move_to_heal);
            this.PVE_movementGB.Controls.Add(this.PVE_wanna_crusader);
            this.PVE_movementGB.Location = new System.Drawing.Point(325, 373);
            this.PVE_movementGB.Name = "PVE_movementGB";
            this.PVE_movementGB.Size = new System.Drawing.Size(249, 156);
            this.PVE_movementGB.TabIndex = 46;
            this.PVE_movementGB.TabStop = false;
            this.PVE_movementGB.Text = "Movement";
            // 
            // PVE_do_not_dismount_EVER
            // 
            this.PVE_do_not_dismount_EVER.AutoSize = true;
            this.PVE_do_not_dismount_EVER.Location = new System.Drawing.Point(6, 137);
            this.PVE_do_not_dismount_EVER.Name = "PVE_do_not_dismount_EVER";
            this.PVE_do_not_dismount_EVER.Size = new System.Drawing.Size(130, 17);
            this.PVE_do_not_dismount_EVER.TabIndex = 27;
            this.PVE_do_not_dismount_EVER.Text = "Don\'t Dismount EVER";
            this.PVE_do_not_dismount_EVER.UseVisualStyleBackColor = true;
            this.PVE_do_not_dismount_EVER.CheckedChanged += new System.EventHandler(this.PVE_do_not_dismount_EVER_CheckedChanged);
            // 
            // PVE_do_not_dismount_ooc
            // 
            this.PVE_do_not_dismount_ooc.AutoSize = true;
            this.PVE_do_not_dismount_ooc.Location = new System.Drawing.Point(6, 113);
            this.PVE_do_not_dismount_ooc.Name = "PVE_do_not_dismount_ooc";
            this.PVE_do_not_dismount_ooc.Size = new System.Drawing.Size(169, 17);
            this.PVE_do_not_dismount_ooc.TabIndex = 26;
            this.PVE_do_not_dismount_ooc.Text = "Don\'t Dismount Out of Combat";
            this.PVE_do_not_dismount_ooc.UseVisualStyleBackColor = true;
            this.PVE_do_not_dismount_ooc.CheckedChanged += new System.EventHandler(this.PVE_do_not_dismount_ooc_CheckedChanged);
            // 
            // PVE_wanna_move_to_HoJ
            // 
            this.PVE_wanna_move_to_HoJ.AutoSize = true;
            this.PVE_wanna_move_to_HoJ.Location = new System.Drawing.Point(7, 66);
            this.PVE_wanna_move_to_HoJ.Name = "PVE_wanna_move_to_HoJ";
            this.PVE_wanna_move_to_HoJ.Size = new System.Drawing.Size(201, 17);
            this.PVE_wanna_move_to_HoJ.TabIndex = 25;
            this.PVE_wanna_move_to_HoJ.Text = "Move in Range to Hammer of Justice";
            this.PVE_wanna_move_to_HoJ.UseVisualStyleBackColor = true;
            this.PVE_wanna_move_to_HoJ.CheckedChanged += new System.EventHandler(this.PVE_wanna_move_to_HoJ_CheckedChanged);
            // 
            // PVE_wanna_mount
            // 
            this.PVE_wanna_mount.AutoSize = true;
            this.PVE_wanna_mount.Location = new System.Drawing.Point(6, 19);
            this.PVE_wanna_mount.Name = "PVE_wanna_mount";
            this.PVE_wanna_mount.Size = new System.Drawing.Size(73, 17);
            this.PVE_wanna_mount.TabIndex = 23;
            this.PVE_wanna_mount.Text = "Mount Up";
            this.PVE_wanna_mount.UseVisualStyleBackColor = true;
            this.PVE_wanna_mount.CheckedChanged += new System.EventHandler(this.PVE_wanna_mount_CheckedChanged);
            // 
            // PVE_wanna_move_to_heal
            // 
            this.PVE_wanna_move_to_heal.AutoSize = true;
            this.PVE_wanna_move_to_heal.Location = new System.Drawing.Point(6, 42);
            this.PVE_wanna_move_to_heal.Name = "PVE_wanna_move_to_heal";
            this.PVE_wanna_move_to_heal.Size = new System.Drawing.Size(136, 17);
            this.PVE_wanna_move_to_heal.TabIndex = 24;
            this.PVE_wanna_move_to_heal.Text = "Move in Range to Heal";
            this.PVE_wanna_move_to_heal.UseVisualStyleBackColor = true;
            this.PVE_wanna_move_to_heal.CheckedChanged += new System.EventHandler(this.PVE_wanna_move_to_heal_CheckedChanged);
            // 
            // PVE_wanna_crusader
            // 
            this.PVE_wanna_crusader.AutoSize = true;
            this.PVE_wanna_crusader.Location = new System.Drawing.Point(6, 89);
            this.PVE_wanna_crusader.Name = "PVE_wanna_crusader";
            this.PVE_wanna_crusader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PVE_wanna_crusader.Size = new System.Drawing.Size(213, 17);
            this.PVE_wanna_crusader.TabIndex = 0;
            this.PVE_wanna_crusader.Text = "Switch to Crusader Aura when mounted";
            this.PVE_wanna_crusader.UseVisualStyleBackColor = true;
            this.PVE_wanna_crusader.CheckedChanged += new System.EventHandler(this.PVE_wanna_crusader_CheckedChanged);
            // 
            // PVE_generalGB
            // 
            this.PVE_generalGB.Controls.Add(this.label15);
            this.PVE_generalGB.Controls.Add(this.PVE_rest_if_mana_below);
            this.PVE_generalGB.Controls.Add(this.PVE_wanna_buff);
            this.PVE_generalGB.Location = new System.Drawing.Point(12, 19);
            this.PVE_generalGB.Name = "PVE_generalGB";
            this.PVE_generalGB.Size = new System.Drawing.Size(307, 66);
            this.PVE_generalGB.TabIndex = 44;
            this.PVE_generalGB.TabStop = false;
            this.PVE_generalGB.Text = "General";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(64, 44);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Rest at this Mana %";
            // 
            // PVE_rest_if_mana_below
            // 
            this.PVE_rest_if_mana_below.Location = new System.Drawing.Point(5, 41);
            this.PVE_rest_if_mana_below.Name = "PVE_rest_if_mana_below";
            this.PVE_rest_if_mana_below.Size = new System.Drawing.Size(53, 20);
            this.PVE_rest_if_mana_below.TabIndex = 5;
            this.PVE_rest_if_mana_below.ValueChanged += new System.EventHandler(this.PVE_rest_if_mana_below_ValueChanged);
            // 
            // PVE_wanna_buff
            // 
            this.PVE_wanna_buff.AutoSize = true;
            this.PVE_wanna_buff.Location = new System.Drawing.Point(5, 19);
            this.PVE_wanna_buff.Name = "PVE_wanna_buff";
            this.PVE_wanna_buff.Size = new System.Drawing.Size(86, 17);
            this.PVE_wanna_buff.TabIndex = 4;
            this.PVE_wanna_buff.Text = "Enable Buffs";
            this.PVE_wanna_buff.UseVisualStyleBackColor = true;
            this.PVE_wanna_buff.CheckedChanged += new System.EventHandler(this.PVE_wanna_buff_CheckedChanged);
            // 
            // PVE_dpsGB
            // 
            this.PVE_dpsGB.Controls.Add(this.PVE_wanna_Judge);
            this.PVE_dpsGB.Controls.Add(this.PVE_wanna_CS);
            this.PVE_dpsGB.Controls.Add(this.PVE_wanna_HoW);
            this.PVE_dpsGB.Location = new System.Drawing.Point(822, 208);
            this.PVE_dpsGB.Name = "PVE_dpsGB";
            this.PVE_dpsGB.Size = new System.Drawing.Size(146, 100);
            this.PVE_dpsGB.TabIndex = 42;
            this.PVE_dpsGB.TabStop = false;
            this.PVE_dpsGB.Text = "DPS";
            // 
            // PVE_wanna_Judge
            // 
            this.PVE_wanna_Judge.AutoSize = true;
            this.PVE_wanna_Judge.Location = new System.Drawing.Point(17, 70);
            this.PVE_wanna_Judge.Name = "PVE_wanna_Judge";
            this.PVE_wanna_Judge.Size = new System.Drawing.Size(78, 17);
            this.PVE_wanna_Judge.TabIndex = 23;
            this.PVE_wanna_Judge.Text = "Judgement";
            this.PVE_wanna_Judge.UseVisualStyleBackColor = true;
            this.PVE_wanna_Judge.CheckedChanged += new System.EventHandler(this.PVE_wanna_Judge_CheckedChanged);
            // 
            // PVE_wanna_CS
            // 
            this.PVE_wanna_CS.AutoSize = true;
            this.PVE_wanna_CS.Location = new System.Drawing.Point(17, 24);
            this.PVE_wanna_CS.Name = "PVE_wanna_CS";
            this.PVE_wanna_CS.Size = new System.Drawing.Size(98, 17);
            this.PVE_wanna_CS.TabIndex = 6;
            this.PVE_wanna_CS.Text = "Crusader Strike";
            this.PVE_wanna_CS.UseVisualStyleBackColor = true;
            this.PVE_wanna_CS.CheckedChanged += new System.EventHandler(this.PVE_wanna_CS_CheckedChanged);
            // 
            // PVE_wanna_HoW
            // 
            this.PVE_wanna_HoW.AutoSize = true;
            this.PVE_wanna_HoW.Location = new System.Drawing.Point(17, 47);
            this.PVE_wanna_HoW.Name = "PVE_wanna_HoW";
            this.PVE_wanna_HoW.Size = new System.Drawing.Size(109, 17);
            this.PVE_wanna_HoW.TabIndex = 20;
            this.PVE_wanna_HoW.Text = "Hammer of Wrath";
            this.PVE_wanna_HoW.UseVisualStyleBackColor = true;
            this.PVE_wanna_HoW.CheckedChanged += new System.EventHandler(this.PVE_wanna_HoW_CheckedChanged);
            // 
            // PVE_ohshitbuttonGB
            // 
            this.PVE_ohshitbuttonGB.Controls.Add(this.PVE_wanna_lifeblood);
            this.PVE_ohshitbuttonGB.Controls.Add(this.label16);
            this.PVE_ohshitbuttonGB.Controls.Add(this.PVE_min_ohshitbutton_activator);
            this.PVE_ohshitbuttonGB.Controls.Add(this.PVE_wanna_GotAK);
            this.PVE_ohshitbuttonGB.Controls.Add(this.PVE_wanna_AW);
            this.PVE_ohshitbuttonGB.Controls.Add(this.PVE_wanna_DF);
            this.PVE_ohshitbuttonGB.Location = new System.Drawing.Point(580, 19);
            this.PVE_ohshitbuttonGB.Name = "PVE_ohshitbuttonGB";
            this.PVE_ohshitbuttonGB.Size = new System.Drawing.Size(236, 145);
            this.PVE_ohshitbuttonGB.TabIndex = 41;
            this.PVE_ohshitbuttonGB.TabStop = false;
            this.PVE_ohshitbuttonGB.Text = "Oh Shit! Buttons";
            // 
            // PVE_wanna_lifeblood
            // 
            this.PVE_wanna_lifeblood.AutoSize = true;
            this.PVE_wanna_lifeblood.Location = new System.Drawing.Point(6, 47);
            this.PVE_wanna_lifeblood.Name = "PVE_wanna_lifeblood";
            this.PVE_wanna_lifeblood.Size = new System.Drawing.Size(69, 17);
            this.PVE_wanna_lifeblood.TabIndex = 11;
            this.PVE_wanna_lifeblood.Text = "Lifeblood";
            this.PVE_wanna_lifeblood.UseVisualStyleBackColor = true;
            this.PVE_wanna_lifeblood.CheckedChanged += new System.EventHandler(this.PVE_wanna_lifeblood_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(63, 24);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(135, 13);
            this.label16.TabIndex = 10;
            this.label16.Text = "Press if someone is this low";
            // 
            // PVE_min_ohshitbutton_activator
            // 
            this.PVE_min_ohshitbutton_activator.Location = new System.Drawing.Point(6, 20);
            this.PVE_min_ohshitbutton_activator.Name = "PVE_min_ohshitbutton_activator";
            this.PVE_min_ohshitbutton_activator.Size = new System.Drawing.Size(50, 20);
            this.PVE_min_ohshitbutton_activator.TabIndex = 9;
            this.PVE_min_ohshitbutton_activator.ValueChanged += new System.EventHandler(this.PVE_min_ohshitbutton_activator_ValueChanged);
            // 
            // PVE_wanna_GotAK
            // 
            this.PVE_wanna_GotAK.AutoSize = true;
            this.PVE_wanna_GotAK.Location = new System.Drawing.Point(6, 122);
            this.PVE_wanna_GotAK.Name = "PVE_wanna_GotAK";
            this.PVE_wanna_GotAK.Size = new System.Drawing.Size(167, 17);
            this.PVE_wanna_GotAK.TabIndex = 8;
            this.PVE_wanna_GotAK.Text = "Guardian of the Ancient Kings";
            this.PVE_wanna_GotAK.UseVisualStyleBackColor = true;
            this.PVE_wanna_GotAK.CheckedChanged += new System.EventHandler(this.PVE_wanna_GotAK_CheckedChanged);
            // 
            // PVE_wanna_AW
            // 
            this.PVE_wanna_AW.AutoSize = true;
            this.PVE_wanna_AW.Location = new System.Drawing.Point(6, 70);
            this.PVE_wanna_AW.Name = "PVE_wanna_AW";
            this.PVE_wanna_AW.Size = new System.Drawing.Size(103, 17);
            this.PVE_wanna_AW.TabIndex = 3;
            this.PVE_wanna_AW.Text = "Avenging Wrath";
            this.PVE_wanna_AW.UseVisualStyleBackColor = true;
            this.PVE_wanna_AW.CheckedChanged += new System.EventHandler(this.PVE_wanna_AW_CheckedChanged);
            // 
            // PVE_wanna_DF
            // 
            this.PVE_wanna_DF.AutoSize = true;
            this.PVE_wanna_DF.Location = new System.Drawing.Point(6, 95);
            this.PVE_wanna_DF.Name = "PVE_wanna_DF";
            this.PVE_wanna_DF.Size = new System.Drawing.Size(86, 17);
            this.PVE_wanna_DF.TabIndex = 7;
            this.PVE_wanna_DF.Text = "Divine Favor";
            this.PVE_wanna_DF.UseVisualStyleBackColor = true;
            this.PVE_wanna_DF.CheckedChanged += new System.EventHandler(this.PVE_wanna_DF_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.Raid_optimizeGB);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.Raid_bless_selection);
            this.tabPage3.Controls.Add(this.groupBox15);
            this.tabPage3.Controls.Add(this.groupBox18);
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.groupBox23);
            this.tabPage3.Controls.Add(this.groupBox24);
            this.tabPage3.Controls.Add(this.groupBox25);
            this.tabPage3.Controls.Add(this.groupBox26);
            this.tabPage3.Controls.Add(this.groupBox27);
            this.tabPage3.Controls.Add(this.groupBox28);
            this.tabPage3.Controls.Add(this.Raid_auraselctGB);
            this.tabPage3.Controls.Add(this.groupBox30);
            this.tabPage3.Controls.Add(this.groupBox31);
            this.tabPage3.Controls.Add(this.Raid_generalGB);
            this.tabPage3.Controls.Add(this.groupBox33);
            this.tabPage3.Controls.Add(this.groupBox34);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(994, 653);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Raid";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Raid_optimizeGB
            // 
            this.Raid_optimizeGB.Controls.Add(this.Raid_intellywait);
            this.Raid_optimizeGB.Controls.Add(this.Raid_accurancy);
            this.Raid_optimizeGB.Controls.Add(this.Raid_speed);
            this.Raid_optimizeGB.Location = new System.Drawing.Point(578, 462);
            this.Raid_optimizeGB.Name = "Raid_optimizeGB";
            this.Raid_optimizeGB.Size = new System.Drawing.Size(200, 100);
            this.Raid_optimizeGB.TabIndex = 74;
            this.Raid_optimizeGB.TabStop = false;
            this.Raid_optimizeGB.Text = "Optimize the CC for";
            this.Raid_optimizeGB.Enter += new System.EventHandler(this.Raid_optimizeGB_Enter);
            // 
            // Raid_intellywait
            // 
            this.Raid_intellywait.AutoSize = true;
            this.Raid_intellywait.Location = new System.Drawing.Point(7, 68);
            this.Raid_intellywait.Name = "Raid_intellywait";
            this.Raid_intellywait.Size = new System.Drawing.Size(159, 17);
            this.Raid_intellywait.TabIndex = 2;
            this.Raid_intellywait.TabStop = true;
            this.Raid_intellywait.Tag = "optimize";
            this.Raid_intellywait.Text = "IntellyWait (combat sistem 6)";
            this.Raid_intellywait.UseVisualStyleBackColor = true;
            this.Raid_intellywait.CheckedChanged += new System.EventHandler(this.Raid_intellywait_CheckedChanged);
            // 
            // Raid_accurancy
            // 
            this.Raid_accurancy.AutoSize = true;
            this.Raid_accurancy.Location = new System.Drawing.Point(7, 44);
            this.Raid_accurancy.Name = "Raid_accurancy";
            this.Raid_accurancy.Size = new System.Drawing.Size(161, 17);
            this.Raid_accurancy.TabIndex = 1;
            this.Raid_accurancy.TabStop = true;
            this.Raid_accurancy.Tag = "optimize";
            this.Raid_accurancy.Text = "Accurancy (combat sistem 5)";
            this.Raid_accurancy.UseVisualStyleBackColor = true;
            this.Raid_accurancy.CheckedChanged += new System.EventHandler(this.Raid_accurancy_CheckedChanged);
            // 
            // Raid_speed
            // 
            this.Raid_speed.AutoSize = true;
            this.Raid_speed.Location = new System.Drawing.Point(8, 20);
            this.Raid_speed.Name = "Raid_speed";
            this.Raid_speed.Size = new System.Drawing.Size(141, 17);
            this.Raid_speed.TabIndex = 0;
            this.Raid_speed.TabStop = true;
            this.Raid_speed.Tag = "optimize";
            this.Raid_speed.Text = "Speed (combat sistem 4)";
            this.Raid_speed.UseVisualStyleBackColor = true;
            this.Raid_speed.CheckedChanged += new System.EventHandler(this.Raid_speed_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label81);
            this.groupBox4.Controls.Add(this.Raid_tank_healing_priority_multiplier);
            this.groupBox4.Controls.Add(this.label79);
            this.groupBox4.Controls.Add(this.Raid_healing_tank_priority);
            this.groupBox4.Location = new System.Drawing.Point(580, 356);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(236, 100);
            this.groupBox4.TabIndex = 73;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tank Healing Priority";
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(68, 53);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(48, 13);
            this.label81.TabIndex = 5;
            this.label81.Text = "Multiplier";
            // 
            // Raid_tank_healing_priority_multiplier
            // 
            this.Raid_tank_healing_priority_multiplier.Location = new System.Drawing.Point(15, 46);
            this.Raid_tank_healing_priority_multiplier.Name = "Raid_tank_healing_priority_multiplier";
            this.Raid_tank_healing_priority_multiplier.Size = new System.Drawing.Size(45, 20);
            this.Raid_tank_healing_priority_multiplier.TabIndex = 4;
            this.Raid_tank_healing_priority_multiplier.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Raid_tank_healing_priority_multiplier.ValueChanged += new System.EventHandler(this.Raid_tank_healing_priority_multiplier_ValueChanged);
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(68, 26);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(150, 13);
            this.label79.TabIndex = 1;
            this.label79.Text = "From 0% to 100% more priority ";
            // 
            // Raid_healing_tank_priority
            // 
            this.Raid_healing_tank_priority.Location = new System.Drawing.Point(15, 20);
            this.Raid_healing_tank_priority.Name = "Raid_healing_tank_priority";
            this.Raid_healing_tank_priority.Size = new System.Drawing.Size(45, 20);
            this.Raid_healing_tank_priority.TabIndex = 0;
            this.Raid_healing_tank_priority.ValueChanged += new System.EventHandler(this.Raid_healing_tank_priority_ValueChanged);
            // 
            // Raid_bless_selection
            // 
            this.Raid_bless_selection.Controls.Add(this.Raid_bless_type_disabledRB);
            this.Raid_bless_selection.Controls.Add(this.Raid_bless_type_MightRB);
            this.Raid_bless_selection.Controls.Add(this.Raid_bless_type_KingRB);
            this.Raid_bless_selection.Controls.Add(this.Raid_bless_type_autoRB);
            this.Raid_bless_selection.Location = new System.Drawing.Point(822, 417);
            this.Raid_bless_selection.Name = "Raid_bless_selection";
            this.Raid_bless_selection.Size = new System.Drawing.Size(146, 118);
            this.Raid_bless_selection.TabIndex = 72;
            this.Raid_bless_selection.TabStop = false;
            this.Raid_bless_selection.Text = "Bless Selection";
            this.Raid_bless_selection.Enter += new System.EventHandler(this.Raid_bless_selection_Enter);
            // 
            // Raid_bless_type_disabledRB
            // 
            this.Raid_bless_type_disabledRB.AutoSize = true;
            this.Raid_bless_type_disabledRB.Location = new System.Drawing.Point(7, 92);
            this.Raid_bless_type_disabledRB.Name = "Raid_bless_type_disabledRB";
            this.Raid_bless_type_disabledRB.Size = new System.Drawing.Size(66, 17);
            this.Raid_bless_type_disabledRB.TabIndex = 3;
            this.Raid_bless_type_disabledRB.TabStop = true;
            this.Raid_bless_type_disabledRB.Tag = "Bless";
            this.Raid_bless_type_disabledRB.Text = "Disabled";
            this.Raid_bless_type_disabledRB.UseVisualStyleBackColor = true;
            this.Raid_bless_type_disabledRB.CheckedChanged += new System.EventHandler(this.Raid_bless_type_disabledRB_CheckedChanged);
            // 
            // Raid_bless_type_MightRB
            // 
            this.Raid_bless_type_MightRB.AutoSize = true;
            this.Raid_bless_type_MightRB.Location = new System.Drawing.Point(7, 68);
            this.Raid_bless_type_MightRB.Name = "Raid_bless_type_MightRB";
            this.Raid_bless_type_MightRB.Size = new System.Drawing.Size(51, 17);
            this.Raid_bless_type_MightRB.TabIndex = 2;
            this.Raid_bless_type_MightRB.TabStop = true;
            this.Raid_bless_type_MightRB.Tag = "Bless";
            this.Raid_bless_type_MightRB.Text = "Might";
            this.Raid_bless_type_MightRB.UseVisualStyleBackColor = true;
            this.Raid_bless_type_MightRB.CheckedChanged += new System.EventHandler(this.Raid_bless_type_MightRB_CheckedChanged);
            // 
            // Raid_bless_type_KingRB
            // 
            this.Raid_bless_type_KingRB.AutoSize = true;
            this.Raid_bless_type_KingRB.Location = new System.Drawing.Point(7, 44);
            this.Raid_bless_type_KingRB.Name = "Raid_bless_type_KingRB";
            this.Raid_bless_type_KingRB.Size = new System.Drawing.Size(46, 17);
            this.Raid_bless_type_KingRB.TabIndex = 1;
            this.Raid_bless_type_KingRB.TabStop = true;
            this.Raid_bless_type_KingRB.Tag = "Bless";
            this.Raid_bless_type_KingRB.Text = "King";
            this.Raid_bless_type_KingRB.UseVisualStyleBackColor = true;
            this.Raid_bless_type_KingRB.CheckedChanged += new System.EventHandler(this.Raid_bless_type_KingRB_CheckedChanged);
            // 
            // Raid_bless_type_autoRB
            // 
            this.Raid_bless_type_autoRB.AutoSize = true;
            this.Raid_bless_type_autoRB.Location = new System.Drawing.Point(7, 20);
            this.Raid_bless_type_autoRB.Name = "Raid_bless_type_autoRB";
            this.Raid_bless_type_autoRB.Size = new System.Drawing.Size(47, 17);
            this.Raid_bless_type_autoRB.TabIndex = 0;
            this.Raid_bless_type_autoRB.TabStop = true;
            this.Raid_bless_type_autoRB.Tag = "Bless";
            this.Raid_bless_type_autoRB.Text = "Auto";
            this.Raid_bless_type_autoRB.UseVisualStyleBackColor = true;
            this.Raid_bless_type_autoRB.CheckedChanged += new System.EventHandler(this.Raid_bless_type_autoRB_CheckedChanged);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label19);
            this.groupBox15.Controls.Add(this.Raid_stop_DL_if_above);
            this.groupBox15.Location = new System.Drawing.Point(578, 301);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(236, 49);
            this.groupBox15.TabIndex = 71;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Overhealing Protection";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(67, 26);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(171, 13);
            this.label19.TabIndex = 56;
            this.label19.Text = "Cancel Divine Light if Above this %";
            // 
            // Raid_stop_DL_if_above
            // 
            this.Raid_stop_DL_if_above.Location = new System.Drawing.Point(8, 19);
            this.Raid_stop_DL_if_above.Name = "Raid_stop_DL_if_above";
            this.Raid_stop_DL_if_above.Size = new System.Drawing.Size(54, 20);
            this.Raid_stop_DL_if_above.TabIndex = 55;
            this.Raid_stop_DL_if_above.ValueChanged += new System.EventHandler(this.Raid_stop_DL_if_above_ValueChanged);
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.Raid_get_tank_from_lua);
            this.groupBox18.Controls.Add(this.Raid_get_tank_from_focus);
            this.groupBox18.Location = new System.Drawing.Point(12, 91);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(307, 69);
            this.groupBox18.TabIndex = 70;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Tank Selection";
            // 
            // Raid_get_tank_from_lua
            // 
            this.Raid_get_tank_from_lua.AutoSize = true;
            this.Raid_get_tank_from_lua.Location = new System.Drawing.Point(8, 42);
            this.Raid_get_tank_from_lua.Name = "Raid_get_tank_from_lua";
            this.Raid_get_tank_from_lua.Size = new System.Drawing.Size(156, 17);
            this.Raid_get_tank_from_lua.TabIndex = 2;
            this.Raid_get_tank_from_lua.Text = "Use the ingame RoleCheck";
            this.Raid_get_tank_from_lua.UseVisualStyleBackColor = true;
            this.Raid_get_tank_from_lua.CheckedChanged += new System.EventHandler(this.Raid_get_tank_from_lua_CheckedChanged);
            // 
            // Raid_get_tank_from_focus
            // 
            this.Raid_get_tank_from_focus.AutoSize = true;
            this.Raid_get_tank_from_focus.Location = new System.Drawing.Point(8, 19);
            this.Raid_get_tank_from_focus.Name = "Raid_get_tank_from_focus";
            this.Raid_get_tank_from_focus.Size = new System.Drawing.Size(179, 17);
            this.Raid_get_tank_from_focus.TabIndex = 1;
            this.Raid_get_tank_from_focus.Text = "Import the Tank from your Focus";
            this.Raid_get_tank_from_focus.UseVisualStyleBackColor = true;
            this.Raid_get_tank_from_focus.CheckedChanged += new System.EventHandler(this.Raid_get_tank_from_focus_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Raid_advanced_option);
            this.panel3.Controls.Add(this.Raid_advanced);
            this.panel3.Location = new System.Drawing.Point(12, 437);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(313, 202);
            this.panel3.TabIndex = 69;
            // 
            // Raid_advanced_option
            // 
            this.Raid_advanced_option.AutoSize = true;
            this.Raid_advanced_option.Location = new System.Drawing.Point(6, 3);
            this.Raid_advanced_option.Name = "Raid_advanced_option";
            this.Raid_advanced_option.Size = new System.Drawing.Size(144, 17);
            this.Raid_advanced_option.TabIndex = 34;
            this.Raid_advanced_option.Text = "Show Advanced Options";
            this.Raid_advanced_option.UseVisualStyleBackColor = true;
            this.Raid_advanced_option.CheckedChanged += new System.EventHandler(this.Raid_advanced_option_CheckedChanged);
            // 
            // Raid_advanced
            // 
            this.Raid_advanced.Controls.Add(this.label33);
            this.Raid_advanced.Controls.Add(this.Raid_max_healing_distance);
            this.Raid_advanced.Controls.Add(this.Raid_wanna_target);
            this.Raid_advanced.Controls.Add(this.groupBox22);
            this.Raid_advanced.Controls.Add(this.Raid_wanna_face);
            this.Raid_advanced.Controls.Add(this.Raid_ignore_beacon);
            this.Raid_advanced.Location = new System.Drawing.Point(6, 26);
            this.Raid_advanced.Name = "Raid_advanced";
            this.Raid_advanced.Size = new System.Drawing.Size(301, 173);
            this.Raid_advanced.TabIndex = 33;
            this.Raid_advanced.TabStop = false;
            this.Raid_advanced.Text = "Advanced Options";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(71, 124);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(160, 13);
            this.label33.TabIndex = 33;
            this.label33.Text = "Ignore unit more distant than this";
            // 
            // Raid_max_healing_distance
            // 
            this.Raid_max_healing_distance.Location = new System.Drawing.Point(13, 117);
            this.Raid_max_healing_distance.Name = "Raid_max_healing_distance";
            this.Raid_max_healing_distance.Size = new System.Drawing.Size(52, 20);
            this.Raid_max_healing_distance.TabIndex = 32;
            this.Raid_max_healing_distance.ValueChanged += new System.EventHandler(this.Raid_max_healing_distance_ValueChanged);
            // 
            // Raid_wanna_target
            // 
            this.Raid_wanna_target.AutoSize = true;
            this.Raid_wanna_target.Location = new System.Drawing.Point(13, 101);
            this.Raid_wanna_target.Name = "Raid_wanna_target";
            this.Raid_wanna_target.Size = new System.Drawing.Size(216, 17);
            this.Raid_wanna_target.TabIndex = 27;
            this.Raid_wanna_target.Text = "Target if no target (prevent HBCore bug)";
            this.Raid_wanna_target.UseVisualStyleBackColor = true;
            this.Raid_wanna_target.CheckedChanged += new System.EventHandler(this.Raid_wanna_target_CheckedChanged);
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.Raid_do_not_heal_above);
            this.groupBox22.Controls.Add(this.label34);
            this.groupBox22.Location = new System.Drawing.Point(13, 19);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(248, 53);
            this.groupBox22.TabIndex = 31;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Do not consider People above this health";
            // 
            // Raid_do_not_heal_above
            // 
            this.Raid_do_not_heal_above.Location = new System.Drawing.Point(6, 19);
            this.Raid_do_not_heal_above.Name = "Raid_do_not_heal_above";
            this.Raid_do_not_heal_above.Size = new System.Drawing.Size(46, 20);
            this.Raid_do_not_heal_above.TabIndex = 29;
            this.Raid_do_not_heal_above.ValueChanged += new System.EventHandler(this.Raid_do_not_heal_above_ValueChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(58, 26);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(142, 13);
            this.label34.TabIndex = 30;
            this.label34.Text = "DO NOT MESS WITH THIS";
            // 
            // Raid_wanna_face
            // 
            this.Raid_wanna_face.AutoSize = true;
            this.Raid_wanna_face.Location = new System.Drawing.Point(13, 78);
            this.Raid_wanna_face.Name = "Raid_wanna_face";
            this.Raid_wanna_face.Size = new System.Drawing.Size(166, 17);
            this.Raid_wanna_face.TabIndex = 11;
            this.Raid_wanna_face.Text = "Face the target when needed";
            this.Raid_wanna_face.UseVisualStyleBackColor = true;
            this.Raid_wanna_face.CheckedChanged += new System.EventHandler(this.Raid_wanna_face_CheckedChanged);
            // 
            // Raid_ignore_beacon
            // 
            this.Raid_ignore_beacon.AutoSize = true;
            this.Raid_ignore_beacon.Location = new System.Drawing.Point(10, 143);
            this.Raid_ignore_beacon.Name = "Raid_ignore_beacon";
            this.Raid_ignore_beacon.Size = new System.Drawing.Size(190, 17);
            this.Raid_ignore_beacon.TabIndex = 3;
            this.Raid_ignore_beacon.Text = "Do not cast/check Beacon of light";
            this.Raid_ignore_beacon.UseVisualStyleBackColor = true;
            this.Raid_ignore_beacon.CheckedChanged += new System.EventHandler(this.Raid_ignore_beacon_CheckedChanged);
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.Raid_cleanse_only_self_and_tank);
            this.groupBox23.Controls.Add(this.Raid_wanna_cleanse);
            this.groupBox23.Controls.Add(this.Raid_wanna_urgent_cleanse);
            this.groupBox23.Location = new System.Drawing.Point(822, 314);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(146, 100);
            this.groupBox23.TabIndex = 68;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Cleanse";
            // 
            // Raid_cleanse_only_self_and_tank
            // 
            this.Raid_cleanse_only_self_and_tank.AutoSize = true;
            this.Raid_cleanse_only_self_and_tank.Location = new System.Drawing.Point(17, 65);
            this.Raid_cleanse_only_self_and_tank.Name = "Raid_cleanse_only_self_and_tank";
            this.Raid_cleanse_only_self_and_tank.Size = new System.Drawing.Size(134, 17);
            this.Raid_cleanse_only_self_and_tank.TabIndex = 29;
            this.Raid_cleanse_only_self_and_tank.Text = "But only Self and Tank";
            this.Raid_cleanse_only_self_and_tank.UseVisualStyleBackColor = true;
            this.Raid_cleanse_only_self_and_tank.CheckedChanged += new System.EventHandler(this.Raid_cleanse_only_self_and_tank_CheckedChanged);
            // 
            // Raid_wanna_cleanse
            // 
            this.Raid_wanna_cleanse.AutoSize = true;
            this.Raid_wanna_cleanse.Location = new System.Drawing.Point(7, 42);
            this.Raid_wanna_cleanse.Name = "Raid_wanna_cleanse";
            this.Raid_wanna_cleanse.Size = new System.Drawing.Size(64, 17);
            this.Raid_wanna_cleanse.TabIndex = 5;
            this.Raid_wanna_cleanse.Text = "Cleanse";
            this.Raid_wanna_cleanse.UseVisualStyleBackColor = true;
            this.Raid_wanna_cleanse.CheckedChanged += new System.EventHandler(this.Raid_wanna_cleanse_CheckedChanged);
            // 
            // Raid_wanna_urgent_cleanse
            // 
            this.Raid_wanna_urgent_cleanse.AutoSize = true;
            this.Raid_wanna_urgent_cleanse.Location = new System.Drawing.Point(7, 19);
            this.Raid_wanna_urgent_cleanse.Name = "Raid_wanna_urgent_cleanse";
            this.Raid_wanna_urgent_cleanse.Size = new System.Drawing.Size(135, 17);
            this.Raid_wanna_urgent_cleanse.TabIndex = 27;
            this.Raid_wanna_urgent_cleanse.Text = "Cleanse Urgents ASAP";
            this.Raid_wanna_urgent_cleanse.UseVisualStyleBackColor = true;
            this.Raid_wanna_urgent_cleanse.CheckedChanged += new System.EventHandler(this.Raid_wanna_urgent_cleanse_CheckedChanged);
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.Raid_wanna_HoJ);
            this.groupBox24.Controls.Add(this.Raid_wanna_rebuke);
            this.groupBox24.Location = new System.Drawing.Point(822, 125);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(146, 77);
            this.groupBox24.TabIndex = 67;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Interrupts";
            // 
            // Raid_wanna_HoJ
            // 
            this.Raid_wanna_HoJ.AutoSize = true;
            this.Raid_wanna_HoJ.Location = new System.Drawing.Point(9, 19);
            this.Raid_wanna_HoJ.Name = "Raid_wanna_HoJ";
            this.Raid_wanna_HoJ.Size = new System.Drawing.Size(113, 17);
            this.Raid_wanna_HoJ.TabIndex = 17;
            this.Raid_wanna_HoJ.Text = "Hammer of Justice";
            this.Raid_wanna_HoJ.UseVisualStyleBackColor = true;
            this.Raid_wanna_HoJ.CheckedChanged += new System.EventHandler(this.Raid_wanna_HoJ_CheckedChanged);
            // 
            // Raid_wanna_rebuke
            // 
            this.Raid_wanna_rebuke.AutoSize = true;
            this.Raid_wanna_rebuke.Location = new System.Drawing.Point(9, 43);
            this.Raid_wanna_rebuke.Name = "Raid_wanna_rebuke";
            this.Raid_wanna_rebuke.Size = new System.Drawing.Size(64, 17);
            this.Raid_wanna_rebuke.TabIndex = 26;
            this.Raid_wanna_rebuke.Text = "Rebuke";
            this.Raid_wanna_rebuke.UseVisualStyleBackColor = true;
            this.Raid_wanna_rebuke.CheckedChanged += new System.EventHandler(this.Raid_wanna_rebuke_CheckedChanged);
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.Raid_min_Inf_of_light_DL_hpLB);
            this.groupBox25.Controls.Add(this.Raid_min_Inf_of_light_DL_hp);
            this.groupBox25.Controls.Add(this.label36);
            this.groupBox25.Controls.Add(this.Raid_min_HL_hp);
            this.groupBox25.Controls.Add(this.label37);
            this.groupBox25.Controls.Add(this.Raid_min_FoL_hp);
            this.groupBox25.Controls.Add(this.label38);
            this.groupBox25.Controls.Add(this.Raid_min_DL_hp);
            this.groupBox25.Controls.Add(this.Raid_Inf_of_light_wanna_DL);
            this.groupBox25.Location = new System.Drawing.Point(325, 19);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(249, 160);
            this.groupBox25.TabIndex = 66;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Healing";
            // 
            // Raid_min_Inf_of_light_DL_hpLB
            // 
            this.Raid_min_Inf_of_light_DL_hpLB.AutoSize = true;
            this.Raid_min_Inf_of_light_DL_hpLB.Location = new System.Drawing.Point(72, 130);
            this.Raid_min_Inf_of_light_DL_hpLB.Name = "Raid_min_Inf_of_light_DL_hpLB";
            this.Raid_min_Inf_of_light_DL_hpLB.Size = new System.Drawing.Size(172, 13);
            this.Raid_min_Inf_of_light_DL_hpLB.TabIndex = 7;
            this.Raid_min_Inf_of_light_DL_hpLB.Text = "But only if the target is below this %";
            // 
            // Raid_min_Inf_of_light_DL_hp
            // 
            this.Raid_min_Inf_of_light_DL_hp.Location = new System.Drawing.Point(7, 124);
            this.Raid_min_Inf_of_light_DL_hp.Name = "Raid_min_Inf_of_light_DL_hp";
            this.Raid_min_Inf_of_light_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.Raid_min_Inf_of_light_DL_hp.TabIndex = 6;
            this.Raid_min_Inf_of_light_DL_hp.ValueChanged += new System.EventHandler(this.Raid_min_Inf_of_light_DL_hp_ValueChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(70, 26);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(149, 13);
            this.label36.TabIndex = 5;
            this.label36.Text = "Holy Light targets under this %";
            // 
            // Raid_min_HL_hp
            // 
            this.Raid_min_HL_hp.Location = new System.Drawing.Point(7, 20);
            this.Raid_min_HL_hp.Name = "Raid_min_HL_hp";
            this.Raid_min_HL_hp.Size = new System.Drawing.Size(56, 20);
            this.Raid_min_HL_hp.TabIndex = 4;
            this.Raid_min_HL_hp.ValueChanged += new System.EventHandler(this.Raid_min_HL_hp_ValueChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(69, 76);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(165, 13);
            this.label37.TabIndex = 3;
            this.label37.Text = "Flash of Light targets under this %";
            // 
            // Raid_min_FoL_hp
            // 
            this.Raid_min_FoL_hp.Location = new System.Drawing.Point(7, 74);
            this.Raid_min_FoL_hp.Name = "Raid_min_FoL_hp";
            this.Raid_min_FoL_hp.Size = new System.Drawing.Size(56, 20);
            this.Raid_min_FoL_hp.TabIndex = 2;
            this.Raid_min_FoL_hp.ValueChanged += new System.EventHandler(this.Raid_min_FoL_hp_ValueChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(68, 53);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(158, 13);
            this.label38.TabIndex = 1;
            this.label38.Text = "Divine Light targets under this %";
            // 
            // Raid_min_DL_hp
            // 
            this.Raid_min_DL_hp.Location = new System.Drawing.Point(6, 46);
            this.Raid_min_DL_hp.Name = "Raid_min_DL_hp";
            this.Raid_min_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.Raid_min_DL_hp.TabIndex = 0;
            this.Raid_min_DL_hp.ValueChanged += new System.EventHandler(this.Raid_min_DL_hp_ValueChanged);
            // 
            // Raid_Inf_of_light_wanna_DL
            // 
            this.Raid_Inf_of_light_wanna_DL.AutoSize = true;
            this.Raid_Inf_of_light_wanna_DL.Location = new System.Drawing.Point(5, 100);
            this.Raid_Inf_of_light_wanna_DL.Name = "Raid_Inf_of_light_wanna_DL";
            this.Raid_Inf_of_light_wanna_DL.Size = new System.Drawing.Size(235, 17);
            this.Raid_Inf_of_light_wanna_DL.TabIndex = 2;
            this.Raid_Inf_of_light_wanna_DL.Text = "Use Divine Light when Infusion of Light proc";
            this.Raid_Inf_of_light_wanna_DL.UseVisualStyleBackColor = true;
            this.Raid_Inf_of_light_wanna_DL.CheckedChanged += new System.EventHandler(this.Raid_Inf_of_light_wanna_DL_CheckedChanged);
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.label39);
            this.groupBox26.Controls.Add(this.Raid_use_mana_rec_trinket_every);
            this.groupBox26.Controls.Add(this.label40);
            this.groupBox26.Controls.Add(this.Raid_min_mana_rec_trinket);
            this.groupBox26.Controls.Add(this.label41);
            this.groupBox26.Controls.Add(this.Raid_min_Divine_Plea_mana);
            this.groupBox26.Controls.Add(this.label42);
            this.groupBox26.Controls.Add(this.Raid_mana_judge);
            this.groupBox26.Location = new System.Drawing.Point(12, 166);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(307, 126);
            this.groupBox26.TabIndex = 65;
            this.groupBox26.TabStop = false;
            this.groupBox26.Text = "Mana Management";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(49, 103);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(186, 13);
            this.label39.TabIndex = 7;
            this.label39.Text = "Activate your mana trinket every (NYI)";
            // 
            // Raid_use_mana_rec_trinket_every
            // 
            this.Raid_use_mana_rec_trinket_every.Location = new System.Drawing.Point(0, 100);
            this.Raid_use_mana_rec_trinket_every.Name = "Raid_use_mana_rec_trinket_every";
            this.Raid_use_mana_rec_trinket_every.Size = new System.Drawing.Size(43, 20);
            this.Raid_use_mana_rec_trinket_every.TabIndex = 6;
            this.Raid_use_mana_rec_trinket_every.ValueChanged += new System.EventHandler(this.Raid_use_mana_rec_trinket_every_ValueChanged);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(49, 78);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(208, 13);
            this.label40.TabIndex = 5;
            this.label40.Text = "Activate your Mana trinket at Mana %(NYI)";
            // 
            // Raid_min_mana_rec_trinket
            // 
            this.Raid_min_mana_rec_trinket.Location = new System.Drawing.Point(0, 74);
            this.Raid_min_mana_rec_trinket.Name = "Raid_min_mana_rec_trinket";
            this.Raid_min_mana_rec_trinket.Size = new System.Drawing.Size(43, 20);
            this.Raid_min_mana_rec_trinket.TabIndex = 4;
            this.Raid_min_mana_rec_trinket.ValueChanged += new System.EventHandler(this.Raid_min_mana_rec_trinket_ValueChanged);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(49, 54);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(121, 13);
            this.label41.TabIndex = 3;
            this.label41.Text = "Divine Plea at this mana";
            // 
            // Raid_min_Divine_Plea_mana
            // 
            this.Raid_min_Divine_Plea_mana.Location = new System.Drawing.Point(0, 47);
            this.Raid_min_Divine_Plea_mana.Name = "Raid_min_Divine_Plea_mana";
            this.Raid_min_Divine_Plea_mana.Size = new System.Drawing.Size(43, 20);
            this.Raid_min_Divine_Plea_mana.TabIndex = 2;
            this.Raid_min_Divine_Plea_mana.ValueChanged += new System.EventHandler(this.Raid_min_Divine_Plea_mana_ValueChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(49, 27);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(242, 13);
            this.label42.TabIndex = 1;
            this.label42.Text = "Judgement on Cooldown when mana is below this";
            // 
            // Raid_mana_judge
            // 
            this.Raid_mana_judge.Location = new System.Drawing.Point(0, 20);
            this.Raid_mana_judge.Name = "Raid_mana_judge";
            this.Raid_mana_judge.Size = new System.Drawing.Size(43, 20);
            this.Raid_mana_judge.TabIndex = 0;
            this.Raid_mana_judge.ValueChanged += new System.EventHandler(this.Raid_mana_judge_ValueChanged);
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.label43);
            this.groupBox27.Controls.Add(this.Raid_min_player_inside_HR);
            this.groupBox27.Controls.Add(this.label44);
            this.groupBox27.Controls.Add(this.Raid_HR_how_much_health);
            this.groupBox27.Controls.Add(this.label45);
            this.groupBox27.Controls.Add(this.Raid_HR_how_far);
            this.groupBox27.Controls.Add(this.Raid_wanna_HR);
            this.groupBox27.Location = new System.Drawing.Point(12, 298);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(307, 133);
            this.groupBox27.TabIndex = 64;
            this.groupBox27.TabStop = false;
            this.groupBox27.Text = "Holy Radiance Settings";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(61, 103);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(145, 13);
            this.label43.TabIndex = 27;
            this.label43.Text = "Need this many people inside";
            // 
            // Raid_min_player_inside_HR
            // 
            this.Raid_min_player_inside_HR.Location = new System.Drawing.Point(7, 97);
            this.Raid_min_player_inside_HR.Name = "Raid_min_player_inside_HR";
            this.Raid_min_player_inside_HR.Size = new System.Drawing.Size(44, 20);
            this.Raid_min_player_inside_HR.TabIndex = 26;
            this.Raid_min_player_inside_HR.ValueChanged += new System.EventHandler(this.Raid_min_player_inside_HR_ValueChanged);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(58, 76);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(196, 13);
            this.label44.TabIndex = 25;
            this.label44.Text = "Consider unit with less than this Health%";
            // 
            // Raid_HR_how_much_health
            // 
            this.Raid_HR_how_much_health.Location = new System.Drawing.Point(7, 70);
            this.Raid_HR_how_much_health.Name = "Raid_HR_how_much_health";
            this.Raid_HR_how_much_health.Size = new System.Drawing.Size(44, 20);
            this.Raid_HR_how_much_health.TabIndex = 24;
            this.Raid_HR_how_much_health.ValueChanged += new System.EventHandler(this.Raid_HR_how_much_health_ValueChanged);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(58, 49);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(221, 13);
            this.label45.TabIndex = 23;
            this.label45.Text = "Consider unit nearer then this to healed target";
            // 
            // Raid_HR_how_far
            // 
            this.Raid_HR_how_far.Location = new System.Drawing.Point(6, 43);
            this.Raid_HR_how_far.Name = "Raid_HR_how_far";
            this.Raid_HR_how_far.Size = new System.Drawing.Size(45, 20);
            this.Raid_HR_how_far.TabIndex = 22;
            this.Raid_HR_how_far.ValueChanged += new System.EventHandler(this.Raid_HR_how_far_ValueChanged);
            // 
            // Raid_wanna_HR
            // 
            this.Raid_wanna_HR.AutoSize = true;
            this.Raid_wanna_HR.Location = new System.Drawing.Point(6, 19);
            this.Raid_wanna_HR.Name = "Raid_wanna_HR";
            this.Raid_wanna_HR.Size = new System.Drawing.Size(96, 17);
            this.Raid_wanna_HR.TabIndex = 21;
            this.Raid_wanna_HR.Text = "Holy Radiance";
            this.Raid_wanna_HR.UseVisualStyleBackColor = true;
            this.Raid_wanna_HR.CheckedChanged += new System.EventHandler(this.Raid_wanna_HR_CheckedChanged);
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.Raid_min_mana_potion);
            this.groupBox28.Controls.Add(this.Raid_min_LoH_hp);
            this.groupBox28.Controls.Add(this.Raid_min_HoS_hp);
            this.groupBox28.Controls.Add(this.Raid_min_HoP_hp);
            this.groupBox28.Controls.Add(this.Raid_min_DS_hp);
            this.groupBox28.Controls.Add(this.label46);
            this.groupBox28.Controls.Add(this.Raid_min_DP_hp);
            this.groupBox28.Controls.Add(this.Raid_wanna_mana_potion);
            this.groupBox28.Controls.Add(this.Raid_wanna_LoH);
            this.groupBox28.Controls.Add(this.Raid_wanna_DP);
            this.groupBox28.Controls.Add(this.Raid_wanna_DS);
            this.groupBox28.Controls.Add(this.Raid_wanna_HoS);
            this.groupBox28.Controls.Add(this.Raid_wanna_HoP);
            this.groupBox28.Location = new System.Drawing.Point(325, 185);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(247, 182);
            this.groupBox28.TabIndex = 61;
            this.groupBox28.TabStop = false;
            this.groupBox28.Text = "Emergency Buttons";
            // 
            // Raid_min_mana_potion
            // 
            this.Raid_min_mana_potion.Location = new System.Drawing.Point(120, 154);
            this.Raid_min_mana_potion.Name = "Raid_min_mana_potion";
            this.Raid_min_mana_potion.Size = new System.Drawing.Size(64, 20);
            this.Raid_min_mana_potion.TabIndex = 30;
            this.Raid_min_mana_potion.ValueChanged += new System.EventHandler(this.Raid_min_mana_potion_ValueChanged);
            // 
            // Raid_min_LoH_hp
            // 
            this.Raid_min_LoH_hp.Location = new System.Drawing.Point(120, 130);
            this.Raid_min_LoH_hp.Name = "Raid_min_LoH_hp";
            this.Raid_min_LoH_hp.Size = new System.Drawing.Size(64, 20);
            this.Raid_min_LoH_hp.TabIndex = 29;
            this.Raid_min_LoH_hp.ValueChanged += new System.EventHandler(this.Raid_min_LoH_hp_ValueChanged);
            // 
            // Raid_min_HoS_hp
            // 
            this.Raid_min_HoS_hp.Location = new System.Drawing.Point(120, 106);
            this.Raid_min_HoS_hp.Name = "Raid_min_HoS_hp";
            this.Raid_min_HoS_hp.Size = new System.Drawing.Size(64, 20);
            this.Raid_min_HoS_hp.TabIndex = 28;
            this.Raid_min_HoS_hp.ValueChanged += new System.EventHandler(this.Raid_min_HoS_hp_ValueChanged);
            // 
            // Raid_min_HoP_hp
            // 
            this.Raid_min_HoP_hp.Location = new System.Drawing.Point(120, 84);
            this.Raid_min_HoP_hp.Name = "Raid_min_HoP_hp";
            this.Raid_min_HoP_hp.Size = new System.Drawing.Size(64, 20);
            this.Raid_min_HoP_hp.TabIndex = 27;
            this.Raid_min_HoP_hp.ValueChanged += new System.EventHandler(this.Raid_min_HoP_hp_ValueChanged);
            // 
            // Raid_min_DS_hp
            // 
            this.Raid_min_DS_hp.Location = new System.Drawing.Point(120, 58);
            this.Raid_min_DS_hp.Name = "Raid_min_DS_hp";
            this.Raid_min_DS_hp.Size = new System.Drawing.Size(64, 20);
            this.Raid_min_DS_hp.TabIndex = 26;
            this.Raid_min_DS_hp.ValueChanged += new System.EventHandler(this.Raid_min_DS_hp_ValueChanged);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(120, 16);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(58, 13);
            this.label46.TabIndex = 25;
            this.label46.Text = "Use Below";
            // 
            // Raid_min_DP_hp
            // 
            this.Raid_min_DP_hp.Location = new System.Drawing.Point(120, 35);
            this.Raid_min_DP_hp.Name = "Raid_min_DP_hp";
            this.Raid_min_DP_hp.Size = new System.Drawing.Size(64, 20);
            this.Raid_min_DP_hp.TabIndex = 24;
            this.Raid_min_DP_hp.ValueChanged += new System.EventHandler(this.Raid_min_DP_hp_ValueChanged);
            // 
            // Raid_wanna_mana_potion
            // 
            this.Raid_wanna_mana_potion.AutoSize = true;
            this.Raid_wanna_mana_potion.Location = new System.Drawing.Point(6, 154);
            this.Raid_wanna_mana_potion.Name = "Raid_wanna_mana_potion";
            this.Raid_wanna_mana_potion.Size = new System.Drawing.Size(113, 17);
            this.Raid_wanna_mana_potion.TabIndex = 23;
            this.Raid_wanna_mana_potion.Text = "Mana Potion (NYI)";
            this.Raid_wanna_mana_potion.UseVisualStyleBackColor = true;
            this.Raid_wanna_mana_potion.CheckedChanged += new System.EventHandler(this.Raid_wanna_mana_potion_CheckedChanged);
            // 
            // Raid_wanna_LoH
            // 
            this.Raid_wanna_LoH.AutoSize = true;
            this.Raid_wanna_LoH.Location = new System.Drawing.Point(6, 130);
            this.Raid_wanna_LoH.Name = "Raid_wanna_LoH";
            this.Raid_wanna_LoH.Size = new System.Drawing.Size(87, 17);
            this.Raid_wanna_LoH.TabIndex = 23;
            this.Raid_wanna_LoH.Text = "Lay on Hand";
            this.Raid_wanna_LoH.UseVisualStyleBackColor = true;
            this.Raid_wanna_LoH.CheckedChanged += new System.EventHandler(this.Raid_wanna_LoH_CheckedChanged);
            // 
            // Raid_wanna_DP
            // 
            this.Raid_wanna_DP.AutoSize = true;
            this.Raid_wanna_DP.Location = new System.Drawing.Point(6, 38);
            this.Raid_wanna_DP.Name = "Raid_wanna_DP";
            this.Raid_wanna_DP.Size = new System.Drawing.Size(107, 17);
            this.Raid_wanna_DP.TabIndex = 8;
            this.Raid_wanna_DP.Text = "Divine Protection";
            this.Raid_wanna_DP.UseVisualStyleBackColor = true;
            this.Raid_wanna_DP.CheckedChanged += new System.EventHandler(this.Raid_wanna_DP_CheckedChanged);
            // 
            // Raid_wanna_DS
            // 
            this.Raid_wanna_DS.AutoSize = true;
            this.Raid_wanna_DS.Location = new System.Drawing.Point(6, 61);
            this.Raid_wanna_DS.Name = "Raid_wanna_DS";
            this.Raid_wanna_DS.Size = new System.Drawing.Size(88, 17);
            this.Raid_wanna_DS.TabIndex = 9;
            this.Raid_wanna_DS.Text = "Divine Shield";
            this.Raid_wanna_DS.UseVisualStyleBackColor = true;
            this.Raid_wanna_DS.CheckedChanged += new System.EventHandler(this.Raid_wanna_DS_CheckedChanged);
            // 
            // Raid_wanna_HoS
            // 
            this.Raid_wanna_HoS.AutoSize = true;
            this.Raid_wanna_HoS.Location = new System.Drawing.Point(6, 107);
            this.Raid_wanna_HoS.Name = "Raid_wanna_HoS";
            this.Raid_wanna_HoS.Size = new System.Drawing.Size(111, 17);
            this.Raid_wanna_HoS.TabIndex = 19;
            this.Raid_wanna_HoS.Text = "Hand of Salvation";
            this.Raid_wanna_HoS.UseVisualStyleBackColor = true;
            this.Raid_wanna_HoS.CheckedChanged += new System.EventHandler(this.Raid_wanna_HoS_CheckedChanged);
            // 
            // Raid_wanna_HoP
            // 
            this.Raid_wanna_HoP.AutoSize = true;
            this.Raid_wanna_HoP.Location = new System.Drawing.Point(6, 84);
            this.Raid_wanna_HoP.Name = "Raid_wanna_HoP";
            this.Raid_wanna_HoP.Size = new System.Drawing.Size(115, 17);
            this.Raid_wanna_HoP.TabIndex = 18;
            this.Raid_wanna_HoP.Text = "Hand of Protection";
            this.Raid_wanna_HoP.UseVisualStyleBackColor = true;
            this.Raid_wanna_HoP.CheckedChanged += new System.EventHandler(this.Raid_wanna_HoP_CheckedChanged);
            // 
            // Raid_auraselctGB
            // 
            this.Raid_auraselctGB.Controls.Add(this.Raid_DisabledRB);
            this.Raid_auraselctGB.Controls.Add(this.Raid_resistanceRB);
            this.Raid_auraselctGB.Controls.Add(this.Raid_concentrationRB);
            this.Raid_auraselctGB.Location = new System.Drawing.Point(822, 19);
            this.Raid_auraselctGB.Name = "Raid_auraselctGB";
            this.Raid_auraselctGB.Size = new System.Drawing.Size(146, 100);
            this.Raid_auraselctGB.TabIndex = 63;
            this.Raid_auraselctGB.TabStop = false;
            this.Raid_auraselctGB.Text = "Select Aura";
            this.Raid_auraselctGB.Enter += new System.EventHandler(this.Raid_auraselctGB_Enter);
            // 
            // Raid_DisabledRB
            // 
            this.Raid_DisabledRB.AutoSize = true;
            this.Raid_DisabledRB.Location = new System.Drawing.Point(7, 68);
            this.Raid_DisabledRB.Name = "Raid_DisabledRB";
            this.Raid_DisabledRB.Size = new System.Drawing.Size(66, 17);
            this.Raid_DisabledRB.TabIndex = 2;
            this.Raid_DisabledRB.TabStop = true;
            this.Raid_DisabledRB.Text = "Disabled";
            this.Raid_DisabledRB.UseVisualStyleBackColor = true;
            this.Raid_DisabledRB.CheckedChanged += new System.EventHandler(this.Raid_DisabledRB_CheckedChanged);
            // 
            // Raid_resistanceRB
            // 
            this.Raid_resistanceRB.AutoSize = true;
            this.Raid_resistanceRB.Location = new System.Drawing.Point(7, 44);
            this.Raid_resistanceRB.Name = "Raid_resistanceRB";
            this.Raid_resistanceRB.Size = new System.Drawing.Size(78, 17);
            this.Raid_resistanceRB.TabIndex = 1;
            this.Raid_resistanceRB.TabStop = true;
            this.Raid_resistanceRB.Text = "Resistance";
            this.Raid_resistanceRB.UseVisualStyleBackColor = true;
            this.Raid_resistanceRB.CheckedChanged += new System.EventHandler(this.Raid_resistanceRB_CheckedChanged);
            // 
            // Raid_concentrationRB
            // 
            this.Raid_concentrationRB.AutoSize = true;
            this.Raid_concentrationRB.Location = new System.Drawing.Point(7, 20);
            this.Raid_concentrationRB.Name = "Raid_concentrationRB";
            this.Raid_concentrationRB.Size = new System.Drawing.Size(91, 17);
            this.Raid_concentrationRB.TabIndex = 0;
            this.Raid_concentrationRB.TabStop = true;
            this.Raid_concentrationRB.Text = "Concentration";
            this.Raid_concentrationRB.UseVisualStyleBackColor = true;
            this.Raid_concentrationRB.CheckedChanged += new System.EventHandler(this.Raid_concentrationRB_CheckedChanged);
            // 
            // groupBox30
            // 
            this.groupBox30.Controls.Add(this.Raid_min_torrent_mana_perc);
            this.groupBox30.Controls.Add(this.Raid_min_stoneform);
            this.groupBox30.Controls.Add(this.Raid_min_gift_hp);
            this.groupBox30.Controls.Add(this.Raid_wanna_torrent);
            this.groupBox30.Controls.Add(this.Raid_wanna_stoneform);
            this.groupBox30.Controls.Add(this.Raid_wanna_everymanforhimself);
            this.groupBox30.Controls.Add(this.Raid_wanna_gift);
            this.groupBox30.Location = new System.Drawing.Point(578, 170);
            this.groupBox30.Name = "groupBox30";
            this.groupBox30.Size = new System.Drawing.Size(236, 124);
            this.groupBox30.TabIndex = 59;
            this.groupBox30.TabStop = false;
            this.groupBox30.Text = "Racials";
            // 
            // Raid_min_torrent_mana_perc
            // 
            this.Raid_min_torrent_mana_perc.Location = new System.Drawing.Point(172, 95);
            this.Raid_min_torrent_mana_perc.Name = "Raid_min_torrent_mana_perc";
            this.Raid_min_torrent_mana_perc.Size = new System.Drawing.Size(59, 20);
            this.Raid_min_torrent_mana_perc.TabIndex = 17;
            this.Raid_min_torrent_mana_perc.ValueChanged += new System.EventHandler(this.Raid_min_torrent_mana_perc_ValueChanged);
            // 
            // Raid_min_stoneform
            // 
            this.Raid_min_stoneform.Location = new System.Drawing.Point(172, 71);
            this.Raid_min_stoneform.Name = "Raid_min_stoneform";
            this.Raid_min_stoneform.Size = new System.Drawing.Size(59, 20);
            this.Raid_min_stoneform.TabIndex = 16;
            this.Raid_min_stoneform.ValueChanged += new System.EventHandler(this.Raid_min_stoneform_ValueChanged);
            // 
            // Raid_min_gift_hp
            // 
            this.Raid_min_gift_hp.Location = new System.Drawing.Point(172, 45);
            this.Raid_min_gift_hp.Name = "Raid_min_gift_hp";
            this.Raid_min_gift_hp.Size = new System.Drawing.Size(59, 20);
            this.Raid_min_gift_hp.TabIndex = 15;
            this.Raid_min_gift_hp.ValueChanged += new System.EventHandler(this.Raid_min_gift_hp_ValueChanged);
            // 
            // Raid_wanna_torrent
            // 
            this.Raid_wanna_torrent.AutoSize = true;
            this.Raid_wanna_torrent.Location = new System.Drawing.Point(16, 95);
            this.Raid_wanna_torrent.Name = "Raid_wanna_torrent";
            this.Raid_wanna_torrent.Size = new System.Drawing.Size(147, 17);
            this.Raid_wanna_torrent.TabIndex = 14;
            this.Raid_wanna_torrent.Text = "Arcane Torrent (for mana)";
            this.Raid_wanna_torrent.UseVisualStyleBackColor = true;
            this.Raid_wanna_torrent.CheckedChanged += new System.EventHandler(this.Raid_wanna_torrent_CheckedChanged);
            // 
            // Raid_wanna_stoneform
            // 
            this.Raid_wanna_stoneform.AutoSize = true;
            this.Raid_wanna_stoneform.Location = new System.Drawing.Point(16, 71);
            this.Raid_wanna_stoneform.Name = "Raid_wanna_stoneform";
            this.Raid_wanna_stoneform.Size = new System.Drawing.Size(74, 17);
            this.Raid_wanna_stoneform.TabIndex = 13;
            this.Raid_wanna_stoneform.Text = "Stoneform";
            this.Raid_wanna_stoneform.UseVisualStyleBackColor = true;
            this.Raid_wanna_stoneform.CheckedChanged += new System.EventHandler(this.Raid_wanna_stoneform_CheckedChanged);
            // 
            // Raid_wanna_everymanforhimself
            // 
            this.Raid_wanna_everymanforhimself.AutoSize = true;
            this.Raid_wanna_everymanforhimself.Location = new System.Drawing.Point(16, 24);
            this.Raid_wanna_everymanforhimself.Name = "Raid_wanna_everymanforhimself";
            this.Raid_wanna_everymanforhimself.Size = new System.Drawing.Size(129, 17);
            this.Raid_wanna_everymanforhimself.TabIndex = 10;
            this.Raid_wanna_everymanforhimself.Text = "Every Man for Himself";
            this.Raid_wanna_everymanforhimself.UseVisualStyleBackColor = true;
            this.Raid_wanna_everymanforhimself.CheckedChanged += new System.EventHandler(this.Raid_wanna_everymanforhimself_CheckedChanged);
            // 
            // Raid_wanna_gift
            // 
            this.Raid_wanna_gift.AutoSize = true;
            this.Raid_wanna_gift.Location = new System.Drawing.Point(16, 47);
            this.Raid_wanna_gift.Name = "Raid_wanna_gift";
            this.Raid_wanna_gift.Size = new System.Drawing.Size(104, 17);
            this.Raid_wanna_gift.TabIndex = 12;
            this.Raid_wanna_gift.Text = "Gift of the Naaru";
            this.Raid_wanna_gift.UseVisualStyleBackColor = true;
            this.Raid_wanna_gift.CheckedChanged += new System.EventHandler(this.Raid_wanna_gift_CheckedChanged);
            // 
            // groupBox31
            // 
            this.groupBox31.Controls.Add(this.Raid_do_not_dismount_EVER);
            this.groupBox31.Controls.Add(this.Raid_do_not_dismount_ooc);
            this.groupBox31.Controls.Add(this.Raid_wanna_move_to_HoJ);
            this.groupBox31.Controls.Add(this.Raid_wanna_mount);
            this.groupBox31.Controls.Add(this.Raid_wanna_move_to_heal);
            this.groupBox31.Controls.Add(this.Raid_wanna_crusader);
            this.groupBox31.Location = new System.Drawing.Point(325, 373);
            this.groupBox31.Name = "groupBox31";
            this.groupBox31.Size = new System.Drawing.Size(249, 156);
            this.groupBox31.TabIndex = 62;
            this.groupBox31.TabStop = false;
            this.groupBox31.Text = "Movement";
            // 
            // Raid_do_not_dismount_EVER
            // 
            this.Raid_do_not_dismount_EVER.AutoSize = true;
            this.Raid_do_not_dismount_EVER.Location = new System.Drawing.Point(6, 137);
            this.Raid_do_not_dismount_EVER.Name = "Raid_do_not_dismount_EVER";
            this.Raid_do_not_dismount_EVER.Size = new System.Drawing.Size(130, 17);
            this.Raid_do_not_dismount_EVER.TabIndex = 27;
            this.Raid_do_not_dismount_EVER.Text = "Don\'t Dismount EVER";
            this.Raid_do_not_dismount_EVER.UseVisualStyleBackColor = true;
            this.Raid_do_not_dismount_EVER.CheckedChanged += new System.EventHandler(this.Raid_do_not_dismount_EVER_CheckedChanged);
            // 
            // Raid_do_not_dismount_ooc
            // 
            this.Raid_do_not_dismount_ooc.AutoSize = true;
            this.Raid_do_not_dismount_ooc.Location = new System.Drawing.Point(6, 113);
            this.Raid_do_not_dismount_ooc.Name = "Raid_do_not_dismount_ooc";
            this.Raid_do_not_dismount_ooc.Size = new System.Drawing.Size(169, 17);
            this.Raid_do_not_dismount_ooc.TabIndex = 26;
            this.Raid_do_not_dismount_ooc.Text = "Don\'t Dismount Out of Combat";
            this.Raid_do_not_dismount_ooc.UseVisualStyleBackColor = true;
            this.Raid_do_not_dismount_ooc.CheckedChanged += new System.EventHandler(this.Raid_do_not_dismount_ooc_CheckedChanged);
            // 
            // Raid_wanna_move_to_HoJ
            // 
            this.Raid_wanna_move_to_HoJ.AutoSize = true;
            this.Raid_wanna_move_to_HoJ.Location = new System.Drawing.Point(7, 66);
            this.Raid_wanna_move_to_HoJ.Name = "Raid_wanna_move_to_HoJ";
            this.Raid_wanna_move_to_HoJ.Size = new System.Drawing.Size(201, 17);
            this.Raid_wanna_move_to_HoJ.TabIndex = 25;
            this.Raid_wanna_move_to_HoJ.Text = "Move in Range to Hammer of Justice";
            this.Raid_wanna_move_to_HoJ.UseVisualStyleBackColor = true;
            this.Raid_wanna_move_to_HoJ.CheckedChanged += new System.EventHandler(this.Raid_wanna_move_to_HoJ_CheckedChanged);
            // 
            // Raid_wanna_mount
            // 
            this.Raid_wanna_mount.AutoSize = true;
            this.Raid_wanna_mount.Location = new System.Drawing.Point(6, 19);
            this.Raid_wanna_mount.Name = "Raid_wanna_mount";
            this.Raid_wanna_mount.Size = new System.Drawing.Size(73, 17);
            this.Raid_wanna_mount.TabIndex = 23;
            this.Raid_wanna_mount.Text = "Mount Up";
            this.Raid_wanna_mount.UseVisualStyleBackColor = true;
            this.Raid_wanna_mount.CheckedChanged += new System.EventHandler(this.Raid_wanna_mount_CheckedChanged);
            // 
            // Raid_wanna_move_to_heal
            // 
            this.Raid_wanna_move_to_heal.AutoSize = true;
            this.Raid_wanna_move_to_heal.Location = new System.Drawing.Point(6, 42);
            this.Raid_wanna_move_to_heal.Name = "Raid_wanna_move_to_heal";
            this.Raid_wanna_move_to_heal.Size = new System.Drawing.Size(136, 17);
            this.Raid_wanna_move_to_heal.TabIndex = 24;
            this.Raid_wanna_move_to_heal.Text = "Move in Range to Heal";
            this.Raid_wanna_move_to_heal.UseVisualStyleBackColor = true;
            this.Raid_wanna_move_to_heal.CheckedChanged += new System.EventHandler(this.Raid_wanna_move_to_heal_CheckedChanged);
            // 
            // Raid_wanna_crusader
            // 
            this.Raid_wanna_crusader.AutoSize = true;
            this.Raid_wanna_crusader.Location = new System.Drawing.Point(6, 89);
            this.Raid_wanna_crusader.Name = "Raid_wanna_crusader";
            this.Raid_wanna_crusader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Raid_wanna_crusader.Size = new System.Drawing.Size(213, 17);
            this.Raid_wanna_crusader.TabIndex = 0;
            this.Raid_wanna_crusader.Text = "Switch to Crusader Aura when mounted";
            this.Raid_wanna_crusader.UseVisualStyleBackColor = true;
            this.Raid_wanna_crusader.CheckedChanged += new System.EventHandler(this.Raid_wanna_crusader_CheckedChanged);
            // 
            // Raid_generalGB
            // 
            this.Raid_generalGB.Controls.Add(this.label47);
            this.Raid_generalGB.Controls.Add(this.Raid_rest_if_mana_below);
            this.Raid_generalGB.Controls.Add(this.Raid_wanna_buff);
            this.Raid_generalGB.Location = new System.Drawing.Point(12, 19);
            this.Raid_generalGB.Name = "Raid_generalGB";
            this.Raid_generalGB.Size = new System.Drawing.Size(307, 66);
            this.Raid_generalGB.TabIndex = 60;
            this.Raid_generalGB.TabStop = false;
            this.Raid_generalGB.Text = "General";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(64, 44);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(101, 13);
            this.label47.TabIndex = 6;
            this.label47.Text = "Rest at this Mana %";
            // 
            // Raid_rest_if_mana_below
            // 
            this.Raid_rest_if_mana_below.Location = new System.Drawing.Point(5, 41);
            this.Raid_rest_if_mana_below.Name = "Raid_rest_if_mana_below";
            this.Raid_rest_if_mana_below.Size = new System.Drawing.Size(53, 20);
            this.Raid_rest_if_mana_below.TabIndex = 5;
            this.Raid_rest_if_mana_below.ValueChanged += new System.EventHandler(this.Raid_rest_if_mana_below_ValueChanged);
            // 
            // Raid_wanna_buff
            // 
            this.Raid_wanna_buff.AutoSize = true;
            this.Raid_wanna_buff.Location = new System.Drawing.Point(5, 19);
            this.Raid_wanna_buff.Name = "Raid_wanna_buff";
            this.Raid_wanna_buff.Size = new System.Drawing.Size(86, 17);
            this.Raid_wanna_buff.TabIndex = 4;
            this.Raid_wanna_buff.Text = "Enable Buffs";
            this.Raid_wanna_buff.UseVisualStyleBackColor = true;
            this.Raid_wanna_buff.CheckedChanged += new System.EventHandler(this.Raid_wanna_buff_CheckedChanged);
            // 
            // groupBox33
            // 
            this.groupBox33.Controls.Add(this.Raid_wanna_Judge);
            this.groupBox33.Controls.Add(this.Raid_wanna_CS);
            this.groupBox33.Controls.Add(this.Raid_wanna_HoW);
            this.groupBox33.Location = new System.Drawing.Point(822, 208);
            this.groupBox33.Name = "groupBox33";
            this.groupBox33.Size = new System.Drawing.Size(146, 100);
            this.groupBox33.TabIndex = 58;
            this.groupBox33.TabStop = false;
            this.groupBox33.Text = "DPS";
            // 
            // Raid_wanna_Judge
            // 
            this.Raid_wanna_Judge.AutoSize = true;
            this.Raid_wanna_Judge.Location = new System.Drawing.Point(17, 70);
            this.Raid_wanna_Judge.Name = "Raid_wanna_Judge";
            this.Raid_wanna_Judge.Size = new System.Drawing.Size(78, 17);
            this.Raid_wanna_Judge.TabIndex = 23;
            this.Raid_wanna_Judge.Text = "Judgement";
            this.Raid_wanna_Judge.UseVisualStyleBackColor = true;
            this.Raid_wanna_Judge.CheckedChanged += new System.EventHandler(this.Raid_wanna_Judge_CheckedChanged);
            // 
            // Raid_wanna_CS
            // 
            this.Raid_wanna_CS.AutoSize = true;
            this.Raid_wanna_CS.Location = new System.Drawing.Point(17, 24);
            this.Raid_wanna_CS.Name = "Raid_wanna_CS";
            this.Raid_wanna_CS.Size = new System.Drawing.Size(98, 17);
            this.Raid_wanna_CS.TabIndex = 6;
            this.Raid_wanna_CS.Text = "Crusader Strike";
            this.Raid_wanna_CS.UseVisualStyleBackColor = true;
            this.Raid_wanna_CS.CheckedChanged += new System.EventHandler(this.Raid_wanna_CS_CheckedChanged);
            // 
            // Raid_wanna_HoW
            // 
            this.Raid_wanna_HoW.AutoSize = true;
            this.Raid_wanna_HoW.Location = new System.Drawing.Point(17, 47);
            this.Raid_wanna_HoW.Name = "Raid_wanna_HoW";
            this.Raid_wanna_HoW.Size = new System.Drawing.Size(109, 17);
            this.Raid_wanna_HoW.TabIndex = 20;
            this.Raid_wanna_HoW.Text = "Hammer of Wrath";
            this.Raid_wanna_HoW.UseVisualStyleBackColor = true;
            this.Raid_wanna_HoW.CheckedChanged += new System.EventHandler(this.Raid_wanna_HoW_CheckedChanged);
            // 
            // groupBox34
            // 
            this.groupBox34.Controls.Add(this.Raid_wanna_lifeblood);
            this.groupBox34.Controls.Add(this.label48);
            this.groupBox34.Controls.Add(this.Raid_min_ohshitbutton_activator);
            this.groupBox34.Controls.Add(this.Raid_wanna_GotAK);
            this.groupBox34.Controls.Add(this.Raid_wanna_AW);
            this.groupBox34.Controls.Add(this.Raid_wanna_DF);
            this.groupBox34.Location = new System.Drawing.Point(580, 19);
            this.groupBox34.Name = "groupBox34";
            this.groupBox34.Size = new System.Drawing.Size(236, 145);
            this.groupBox34.TabIndex = 57;
            this.groupBox34.TabStop = false;
            this.groupBox34.Text = "Oh Shit! Buttons";
            // 
            // Raid_wanna_lifeblood
            // 
            this.Raid_wanna_lifeblood.AutoSize = true;
            this.Raid_wanna_lifeblood.Location = new System.Drawing.Point(6, 47);
            this.Raid_wanna_lifeblood.Name = "Raid_wanna_lifeblood";
            this.Raid_wanna_lifeblood.Size = new System.Drawing.Size(69, 17);
            this.Raid_wanna_lifeblood.TabIndex = 11;
            this.Raid_wanna_lifeblood.Text = "Lifeblood";
            this.Raid_wanna_lifeblood.UseVisualStyleBackColor = true;
            this.Raid_wanna_lifeblood.CheckedChanged += new System.EventHandler(this.Raid_wanna_lifeblood_CheckedChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(63, 24);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(135, 13);
            this.label48.TabIndex = 10;
            this.label48.Text = "Press if someone is this low";
            // 
            // Raid_min_ohshitbutton_activator
            // 
            this.Raid_min_ohshitbutton_activator.Location = new System.Drawing.Point(6, 20);
            this.Raid_min_ohshitbutton_activator.Name = "Raid_min_ohshitbutton_activator";
            this.Raid_min_ohshitbutton_activator.Size = new System.Drawing.Size(50, 20);
            this.Raid_min_ohshitbutton_activator.TabIndex = 9;
            this.Raid_min_ohshitbutton_activator.ValueChanged += new System.EventHandler(this.Raid_min_ohshitbutton_activator_ValueChanged);
            // 
            // Raid_wanna_GotAK
            // 
            this.Raid_wanna_GotAK.AutoSize = true;
            this.Raid_wanna_GotAK.Location = new System.Drawing.Point(6, 122);
            this.Raid_wanna_GotAK.Name = "Raid_wanna_GotAK";
            this.Raid_wanna_GotAK.Size = new System.Drawing.Size(167, 17);
            this.Raid_wanna_GotAK.TabIndex = 8;
            this.Raid_wanna_GotAK.Text = "Guardian of the Ancient Kings";
            this.Raid_wanna_GotAK.UseVisualStyleBackColor = true;
            this.Raid_wanna_GotAK.CheckedChanged += new System.EventHandler(this.Raid_wanna_GotAK_CheckedChanged);
            // 
            // Raid_wanna_AW
            // 
            this.Raid_wanna_AW.AutoSize = true;
            this.Raid_wanna_AW.Location = new System.Drawing.Point(6, 70);
            this.Raid_wanna_AW.Name = "Raid_wanna_AW";
            this.Raid_wanna_AW.Size = new System.Drawing.Size(103, 17);
            this.Raid_wanna_AW.TabIndex = 3;
            this.Raid_wanna_AW.Text = "Avenging Wrath";
            this.Raid_wanna_AW.UseVisualStyleBackColor = true;
            this.Raid_wanna_AW.CheckedChanged += new System.EventHandler(this.Raid_wanna_AW_CheckedChanged);
            // 
            // Raid_wanna_DF
            // 
            this.Raid_wanna_DF.AutoSize = true;
            this.Raid_wanna_DF.Location = new System.Drawing.Point(6, 95);
            this.Raid_wanna_DF.Name = "Raid_wanna_DF";
            this.Raid_wanna_DF.Size = new System.Drawing.Size(86, 17);
            this.Raid_wanna_DF.TabIndex = 7;
            this.Raid_wanna_DF.Text = "Divine Favor";
            this.Raid_wanna_DF.UseVisualStyleBackColor = true;
            this.Raid_wanna_DF.CheckedChanged += new System.EventHandler(this.Raid_wanna_DF_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.RAFtankfromfocus);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(994, 653);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "RAF";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // RAFtankfromfocus
            // 
            this.RAFtankfromfocus.AutoSize = true;
            this.RAFtankfromfocus.Location = new System.Drawing.Point(440, 19);
            this.RAFtankfromfocus.Name = "RAFtankfromfocus";
            this.RAFtankfromfocus.Size = new System.Drawing.Size(179, 17);
            this.RAFtankfromfocus.TabIndex = 2;
            this.RAFtankfromfocus.Text = "Import the Tank from your Focus";
            this.RAFtankfromfocus.UseVisualStyleBackColor = true;
            this.RAFtankfromfocus.CheckedChanged += new System.EventHandler(this.RAFtankfromfocus_CheckedChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.AutoScroll = true;
            this.tabPage5.Controls.Add(this.Battleground_optimizeGB);
            this.tabPage5.Controls.Add(this.Battleground_bless_selection);
            this.tabPage5.Controls.Add(this.groupBox29);
            this.tabPage5.Controls.Add(this.groupBox32);
            this.tabPage5.Controls.Add(this.panel4);
            this.tabPage5.Controls.Add(this.groupBox37);
            this.tabPage5.Controls.Add(this.groupBox38);
            this.tabPage5.Controls.Add(this.groupBox39);
            this.tabPage5.Controls.Add(this.groupBox40);
            this.tabPage5.Controls.Add(this.groupBox41);
            this.tabPage5.Controls.Add(this.groupBox42);
            this.tabPage5.Controls.Add(this.Battleground_auraselctGB);
            this.tabPage5.Controls.Add(this.groupBox44);
            this.tabPage5.Controls.Add(this.groupBox45);
            this.tabPage5.Controls.Add(this.Battleground_generalGB);
            this.tabPage5.Controls.Add(this.groupBox47);
            this.tabPage5.Controls.Add(this.groupBox48);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(994, 653);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Battleground";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // Battleground_optimizeGB
            // 
            this.Battleground_optimizeGB.Controls.Add(this.Battleground_intellywait);
            this.Battleground_optimizeGB.Controls.Add(this.Battleground_accurancy);
            this.Battleground_optimizeGB.Controls.Add(this.Battleground_speed);
            this.Battleground_optimizeGB.Location = new System.Drawing.Point(580, 406);
            this.Battleground_optimizeGB.Name = "Battleground_optimizeGB";
            this.Battleground_optimizeGB.Size = new System.Drawing.Size(200, 100);
            this.Battleground_optimizeGB.TabIndex = 73;
            this.Battleground_optimizeGB.TabStop = false;
            this.Battleground_optimizeGB.Text = "Optimize the CC for";
            this.Battleground_optimizeGB.Enter += new System.EventHandler(this.Battleground_optimizeGB_Enter);
            // 
            // Battleground_intellywait
            // 
            this.Battleground_intellywait.AutoSize = true;
            this.Battleground_intellywait.Location = new System.Drawing.Point(7, 68);
            this.Battleground_intellywait.Name = "Battleground_intellywait";
            this.Battleground_intellywait.Size = new System.Drawing.Size(159, 17);
            this.Battleground_intellywait.TabIndex = 2;
            this.Battleground_intellywait.TabStop = true;
            this.Battleground_intellywait.Tag = "optimize";
            this.Battleground_intellywait.Text = "IntellyWait (combat sistem 6)";
            this.Battleground_intellywait.UseVisualStyleBackColor = true;
            this.Battleground_intellywait.CheckedChanged += new System.EventHandler(this.Battleground_intellywait_CheckedChanged);
            // 
            // Battleground_accurancy
            // 
            this.Battleground_accurancy.AutoSize = true;
            this.Battleground_accurancy.Location = new System.Drawing.Point(7, 44);
            this.Battleground_accurancy.Name = "Battleground_accurancy";
            this.Battleground_accurancy.Size = new System.Drawing.Size(161, 17);
            this.Battleground_accurancy.TabIndex = 1;
            this.Battleground_accurancy.TabStop = true;
            this.Battleground_accurancy.Tag = "optimize";
            this.Battleground_accurancy.Text = "Accurancy (combat sistem 5)";
            this.Battleground_accurancy.UseVisualStyleBackColor = true;
            this.Battleground_accurancy.CheckedChanged += new System.EventHandler(this.Battleground_accurancy_CheckedChanged);
            // 
            // Battleground_speed
            // 
            this.Battleground_speed.AutoSize = true;
            this.Battleground_speed.Location = new System.Drawing.Point(8, 20);
            this.Battleground_speed.Name = "Battleground_speed";
            this.Battleground_speed.Size = new System.Drawing.Size(141, 17);
            this.Battleground_speed.TabIndex = 0;
            this.Battleground_speed.TabStop = true;
            this.Battleground_speed.Tag = "optimize";
            this.Battleground_speed.Text = "Speed (combat sistem 4)";
            this.Battleground_speed.UseVisualStyleBackColor = true;
            this.Battleground_speed.CheckedChanged += new System.EventHandler(this.Battleground_speed_CheckedChanged);
            // 
            // Battleground_bless_selection
            // 
            this.Battleground_bless_selection.Controls.Add(this.Battleground_bless_type_disabledRB);
            this.Battleground_bless_selection.Controls.Add(this.Battleground_bless_type_MightRB);
            this.Battleground_bless_selection.Controls.Add(this.Battleground_bless_type_KingRB);
            this.Battleground_bless_selection.Controls.Add(this.Battleground_bless_type_autoRB);
            this.Battleground_bless_selection.Location = new System.Drawing.Point(822, 411);
            this.Battleground_bless_selection.Name = "Battleground_bless_selection";
            this.Battleground_bless_selection.Size = new System.Drawing.Size(146, 118);
            this.Battleground_bless_selection.TabIndex = 72;
            this.Battleground_bless_selection.TabStop = false;
            this.Battleground_bless_selection.Text = "Bless Selection";
            this.Battleground_bless_selection.Enter += new System.EventHandler(this.Battleground_bless_selection_Enter);
            // 
            // Battleground_bless_type_disabledRB
            // 
            this.Battleground_bless_type_disabledRB.AutoSize = true;
            this.Battleground_bless_type_disabledRB.Location = new System.Drawing.Point(7, 92);
            this.Battleground_bless_type_disabledRB.Name = "Battleground_bless_type_disabledRB";
            this.Battleground_bless_type_disabledRB.Size = new System.Drawing.Size(66, 17);
            this.Battleground_bless_type_disabledRB.TabIndex = 3;
            this.Battleground_bless_type_disabledRB.TabStop = true;
            this.Battleground_bless_type_disabledRB.Tag = "Bless";
            this.Battleground_bless_type_disabledRB.Text = "Disabled";
            this.Battleground_bless_type_disabledRB.UseVisualStyleBackColor = true;
            this.Battleground_bless_type_disabledRB.CheckedChanged += new System.EventHandler(this.Battleground_bless_type_disabledRB_CheckedChanged);
            // 
            // Battleground_bless_type_MightRB
            // 
            this.Battleground_bless_type_MightRB.AutoSize = true;
            this.Battleground_bless_type_MightRB.Location = new System.Drawing.Point(7, 68);
            this.Battleground_bless_type_MightRB.Name = "Battleground_bless_type_MightRB";
            this.Battleground_bless_type_MightRB.Size = new System.Drawing.Size(51, 17);
            this.Battleground_bless_type_MightRB.TabIndex = 2;
            this.Battleground_bless_type_MightRB.TabStop = true;
            this.Battleground_bless_type_MightRB.Tag = "Bless";
            this.Battleground_bless_type_MightRB.Text = "Might";
            this.Battleground_bless_type_MightRB.UseVisualStyleBackColor = true;
            this.Battleground_bless_type_MightRB.CheckedChanged += new System.EventHandler(this.Battleground_bless_type_MightRB_CheckedChanged);
            // 
            // Battleground_bless_type_KingRB
            // 
            this.Battleground_bless_type_KingRB.AutoSize = true;
            this.Battleground_bless_type_KingRB.Location = new System.Drawing.Point(7, 44);
            this.Battleground_bless_type_KingRB.Name = "Battleground_bless_type_KingRB";
            this.Battleground_bless_type_KingRB.Size = new System.Drawing.Size(46, 17);
            this.Battleground_bless_type_KingRB.TabIndex = 1;
            this.Battleground_bless_type_KingRB.TabStop = true;
            this.Battleground_bless_type_KingRB.Tag = "Bless";
            this.Battleground_bless_type_KingRB.Text = "King";
            this.Battleground_bless_type_KingRB.UseVisualStyleBackColor = true;
            this.Battleground_bless_type_KingRB.CheckedChanged += new System.EventHandler(this.Battleground_bless_type_KingRB_CheckedChanged);
            // 
            // Battleground_bless_type_autoRB
            // 
            this.Battleground_bless_type_autoRB.AutoSize = true;
            this.Battleground_bless_type_autoRB.Location = new System.Drawing.Point(7, 20);
            this.Battleground_bless_type_autoRB.Name = "Battleground_bless_type_autoRB";
            this.Battleground_bless_type_autoRB.Size = new System.Drawing.Size(47, 17);
            this.Battleground_bless_type_autoRB.TabIndex = 0;
            this.Battleground_bless_type_autoRB.TabStop = true;
            this.Battleground_bless_type_autoRB.Tag = "Bless";
            this.Battleground_bless_type_autoRB.Text = "Auto";
            this.Battleground_bless_type_autoRB.UseVisualStyleBackColor = true;
            this.Battleground_bless_type_autoRB.CheckedChanged += new System.EventHandler(this.Battleground_bless_type_autoRB_CheckedChanged);
            // 
            // groupBox29
            // 
            this.groupBox29.Location = new System.Drawing.Point(578, 300);
            this.groupBox29.Name = "groupBox29";
            this.groupBox29.Size = new System.Drawing.Size(236, 100);
            this.groupBox29.TabIndex = 71;
            this.groupBox29.TabStop = false;
            this.groupBox29.Text = "Overhealing Protection";
            // 
            // groupBox32
            // 
            this.groupBox32.Controls.Add(this.Battleground_get_tank_from_lua);
            this.groupBox32.Controls.Add(this.Battleground_get_tank_from_focus);
            this.groupBox32.Location = new System.Drawing.Point(12, 91);
            this.groupBox32.Name = "groupBox32";
            this.groupBox32.Size = new System.Drawing.Size(307, 69);
            this.groupBox32.TabIndex = 70;
            this.groupBox32.TabStop = false;
            this.groupBox32.Text = "Tank Selection";
            // 
            // Battleground_get_tank_from_lua
            // 
            this.Battleground_get_tank_from_lua.AutoSize = true;
            this.Battleground_get_tank_from_lua.Location = new System.Drawing.Point(8, 42);
            this.Battleground_get_tank_from_lua.Name = "Battleground_get_tank_from_lua";
            this.Battleground_get_tank_from_lua.Size = new System.Drawing.Size(156, 17);
            this.Battleground_get_tank_from_lua.TabIndex = 2;
            this.Battleground_get_tank_from_lua.Text = "Use the ingame RoleCheck";
            this.Battleground_get_tank_from_lua.UseVisualStyleBackColor = true;
            this.Battleground_get_tank_from_lua.CheckedChanged += new System.EventHandler(this.Battleground_get_tank_from_lua_CheckedChanged);
            // 
            // Battleground_get_tank_from_focus
            // 
            this.Battleground_get_tank_from_focus.AutoSize = true;
            this.Battleground_get_tank_from_focus.Location = new System.Drawing.Point(8, 19);
            this.Battleground_get_tank_from_focus.Name = "Battleground_get_tank_from_focus";
            this.Battleground_get_tank_from_focus.Size = new System.Drawing.Size(179, 17);
            this.Battleground_get_tank_from_focus.TabIndex = 1;
            this.Battleground_get_tank_from_focus.Text = "Import the Tank from your Focus";
            this.Battleground_get_tank_from_focus.UseVisualStyleBackColor = true;
            this.Battleground_get_tank_from_focus.CheckedChanged += new System.EventHandler(this.Battleground_get_tank_from_focus_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Battleground_advanced_option);
            this.panel4.Controls.Add(this.Battleground_advanced);
            this.panel4.Location = new System.Drawing.Point(12, 437);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(313, 202);
            this.panel4.TabIndex = 69;
            // 
            // Battleground_advanced_option
            // 
            this.Battleground_advanced_option.AutoSize = true;
            this.Battleground_advanced_option.Location = new System.Drawing.Point(6, 3);
            this.Battleground_advanced_option.Name = "Battleground_advanced_option";
            this.Battleground_advanced_option.Size = new System.Drawing.Size(144, 17);
            this.Battleground_advanced_option.TabIndex = 34;
            this.Battleground_advanced_option.Text = "Show Advanced Options";
            this.Battleground_advanced_option.UseVisualStyleBackColor = true;
            this.Battleground_advanced_option.CheckedChanged += new System.EventHandler(this.Battleground_advanced_option_CheckedChanged);
            // 
            // Battleground_advanced
            // 
            this.Battleground_advanced.Controls.Add(this.label49);
            this.Battleground_advanced.Controls.Add(this.Battleground_max_healing_distance);
            this.Battleground_advanced.Controls.Add(this.Battleground_wanna_target);
            this.Battleground_advanced.Controls.Add(this.groupBox36);
            this.Battleground_advanced.Controls.Add(this.Battleground_wanna_face);
            this.Battleground_advanced.Location = new System.Drawing.Point(6, 26);
            this.Battleground_advanced.Name = "Battleground_advanced";
            this.Battleground_advanced.Size = new System.Drawing.Size(301, 173);
            this.Battleground_advanced.TabIndex = 33;
            this.Battleground_advanced.TabStop = false;
            this.Battleground_advanced.Text = "Advanced Options";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(71, 124);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(160, 13);
            this.label49.TabIndex = 33;
            this.label49.Text = "Ignore unit more distant than this";
            // 
            // Battleground_max_healing_distance
            // 
            this.Battleground_max_healing_distance.Location = new System.Drawing.Point(13, 117);
            this.Battleground_max_healing_distance.Name = "Battleground_max_healing_distance";
            this.Battleground_max_healing_distance.Size = new System.Drawing.Size(52, 20);
            this.Battleground_max_healing_distance.TabIndex = 32;
            this.Battleground_max_healing_distance.ValueChanged += new System.EventHandler(this.Battleground_max_healing_distance_ValueChanged);
            // 
            // Battleground_wanna_target
            // 
            this.Battleground_wanna_target.AutoSize = true;
            this.Battleground_wanna_target.Location = new System.Drawing.Point(13, 101);
            this.Battleground_wanna_target.Name = "Battleground_wanna_target";
            this.Battleground_wanna_target.Size = new System.Drawing.Size(216, 17);
            this.Battleground_wanna_target.TabIndex = 27;
            this.Battleground_wanna_target.Text = "Target if no target (prevent HBCore bug)";
            this.Battleground_wanna_target.UseVisualStyleBackColor = true;
            this.Battleground_wanna_target.CheckedChanged += new System.EventHandler(this.Battleground_wanna_target_CheckedChanged);
            // 
            // groupBox36
            // 
            this.groupBox36.Controls.Add(this.Battleground_do_not_heal_above);
            this.groupBox36.Controls.Add(this.label50);
            this.groupBox36.Location = new System.Drawing.Point(13, 19);
            this.groupBox36.Name = "groupBox36";
            this.groupBox36.Size = new System.Drawing.Size(248, 53);
            this.groupBox36.TabIndex = 31;
            this.groupBox36.TabStop = false;
            this.groupBox36.Text = "Do not consider People above this health";
            // 
            // Battleground_do_not_heal_above
            // 
            this.Battleground_do_not_heal_above.Location = new System.Drawing.Point(6, 19);
            this.Battleground_do_not_heal_above.Name = "Battleground_do_not_heal_above";
            this.Battleground_do_not_heal_above.Size = new System.Drawing.Size(46, 20);
            this.Battleground_do_not_heal_above.TabIndex = 29;
            this.Battleground_do_not_heal_above.ValueChanged += new System.EventHandler(this.Battleground_do_not_heal_above_ValueChanged);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(58, 26);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(142, 13);
            this.label50.TabIndex = 30;
            this.label50.Text = "DO NOT MESS WITH THIS";
            // 
            // Battleground_wanna_face
            // 
            this.Battleground_wanna_face.AutoSize = true;
            this.Battleground_wanna_face.Location = new System.Drawing.Point(13, 78);
            this.Battleground_wanna_face.Name = "Battleground_wanna_face";
            this.Battleground_wanna_face.Size = new System.Drawing.Size(166, 17);
            this.Battleground_wanna_face.TabIndex = 11;
            this.Battleground_wanna_face.Text = "Face the target when needed";
            this.Battleground_wanna_face.UseVisualStyleBackColor = true;
            this.Battleground_wanna_face.CheckedChanged += new System.EventHandler(this.Battleground_wanna_face_CheckedChanged);
            // 
            // groupBox37
            // 
            this.groupBox37.Controls.Add(this.Battleground_cleanse_only_self_and_tank);
            this.groupBox37.Controls.Add(this.Battleground_wanna_cleanse);
            this.groupBox37.Controls.Add(this.Battleground_wanna_urgent_cleanse);
            this.groupBox37.Location = new System.Drawing.Point(822, 314);
            this.groupBox37.Name = "groupBox37";
            this.groupBox37.Size = new System.Drawing.Size(146, 95);
            this.groupBox37.TabIndex = 68;
            this.groupBox37.TabStop = false;
            this.groupBox37.Text = "Cleanse";
            // 
            // Battleground_cleanse_only_self_and_tank
            // 
            this.Battleground_cleanse_only_self_and_tank.AutoSize = true;
            this.Battleground_cleanse_only_self_and_tank.Location = new System.Drawing.Point(17, 65);
            this.Battleground_cleanse_only_self_and_tank.Name = "Battleground_cleanse_only_self_and_tank";
            this.Battleground_cleanse_only_self_and_tank.Size = new System.Drawing.Size(134, 17);
            this.Battleground_cleanse_only_self_and_tank.TabIndex = 30;
            this.Battleground_cleanse_only_self_and_tank.Text = "But only Self and Tank";
            this.Battleground_cleanse_only_self_and_tank.UseVisualStyleBackColor = true;
            this.Battleground_cleanse_only_self_and_tank.CheckedChanged += new System.EventHandler(this.Battleground_cleanse_only_self_and_tank_CheckedChanged);
            // 
            // Battleground_wanna_cleanse
            // 
            this.Battleground_wanna_cleanse.AutoSize = true;
            this.Battleground_wanna_cleanse.Location = new System.Drawing.Point(7, 42);
            this.Battleground_wanna_cleanse.Name = "Battleground_wanna_cleanse";
            this.Battleground_wanna_cleanse.Size = new System.Drawing.Size(64, 17);
            this.Battleground_wanna_cleanse.TabIndex = 5;
            this.Battleground_wanna_cleanse.Text = "Cleanse";
            this.Battleground_wanna_cleanse.UseVisualStyleBackColor = true;
            this.Battleground_wanna_cleanse.CheckedChanged += new System.EventHandler(this.Battleground_wanna_cleanse_CheckedChanged);
            // 
            // Battleground_wanna_urgent_cleanse
            // 
            this.Battleground_wanna_urgent_cleanse.AutoSize = true;
            this.Battleground_wanna_urgent_cleanse.Location = new System.Drawing.Point(7, 19);
            this.Battleground_wanna_urgent_cleanse.Name = "Battleground_wanna_urgent_cleanse";
            this.Battleground_wanna_urgent_cleanse.Size = new System.Drawing.Size(135, 17);
            this.Battleground_wanna_urgent_cleanse.TabIndex = 27;
            this.Battleground_wanna_urgent_cleanse.Text = "Cleanse Urgents ASAP";
            this.Battleground_wanna_urgent_cleanse.UseVisualStyleBackColor = true;
            this.Battleground_wanna_urgent_cleanse.CheckedChanged += new System.EventHandler(this.Battleground_wanna_urgent_cleanse_CheckedChanged);
            // 
            // groupBox38
            // 
            this.groupBox38.Controls.Add(this.Battleground_wanna_HoJ);
            this.groupBox38.Controls.Add(this.Battleground_wanna_rebuke);
            this.groupBox38.Location = new System.Drawing.Point(822, 125);
            this.groupBox38.Name = "groupBox38";
            this.groupBox38.Size = new System.Drawing.Size(146, 77);
            this.groupBox38.TabIndex = 67;
            this.groupBox38.TabStop = false;
            this.groupBox38.Text = "Interrupts";
            // 
            // Battleground_wanna_HoJ
            // 
            this.Battleground_wanna_HoJ.AutoSize = true;
            this.Battleground_wanna_HoJ.Location = new System.Drawing.Point(9, 19);
            this.Battleground_wanna_HoJ.Name = "Battleground_wanna_HoJ";
            this.Battleground_wanna_HoJ.Size = new System.Drawing.Size(113, 17);
            this.Battleground_wanna_HoJ.TabIndex = 17;
            this.Battleground_wanna_HoJ.Text = "Hammer of Justice";
            this.Battleground_wanna_HoJ.UseVisualStyleBackColor = true;
            this.Battleground_wanna_HoJ.CheckedChanged += new System.EventHandler(this.Battleground_wanna_HoJ_CheckedChanged);
            // 
            // Battleground_wanna_rebuke
            // 
            this.Battleground_wanna_rebuke.AutoSize = true;
            this.Battleground_wanna_rebuke.Location = new System.Drawing.Point(9, 43);
            this.Battleground_wanna_rebuke.Name = "Battleground_wanna_rebuke";
            this.Battleground_wanna_rebuke.Size = new System.Drawing.Size(64, 17);
            this.Battleground_wanna_rebuke.TabIndex = 26;
            this.Battleground_wanna_rebuke.Text = "Rebuke";
            this.Battleground_wanna_rebuke.UseVisualStyleBackColor = true;
            this.Battleground_wanna_rebuke.CheckedChanged += new System.EventHandler(this.Battleground_wanna_rebuke_CheckedChanged);
            // 
            // groupBox39
            // 
            this.groupBox39.Controls.Add(this.Battleground_min_Inf_of_light_DL_hpLB);
            this.groupBox39.Controls.Add(this.Battleground_min_Inf_of_light_DL_hp);
            this.groupBox39.Controls.Add(this.label52);
            this.groupBox39.Controls.Add(this.Battleground_min_HL_hp);
            this.groupBox39.Controls.Add(this.label53);
            this.groupBox39.Controls.Add(this.Battleground_min_FoL_hp);
            this.groupBox39.Controls.Add(this.label54);
            this.groupBox39.Controls.Add(this.Battleground_min_DL_hp);
            this.groupBox39.Controls.Add(this.Battleground_Inf_of_light_wanna_DL);
            this.groupBox39.Location = new System.Drawing.Point(325, 19);
            this.groupBox39.Name = "groupBox39";
            this.groupBox39.Size = new System.Drawing.Size(249, 160);
            this.groupBox39.TabIndex = 66;
            this.groupBox39.TabStop = false;
            this.groupBox39.Text = "Healing";
            // 
            // Battleground_min_Inf_of_light_DL_hpLB
            // 
            this.Battleground_min_Inf_of_light_DL_hpLB.AutoSize = true;
            this.Battleground_min_Inf_of_light_DL_hpLB.Location = new System.Drawing.Point(72, 130);
            this.Battleground_min_Inf_of_light_DL_hpLB.Name = "Battleground_min_Inf_of_light_DL_hpLB";
            this.Battleground_min_Inf_of_light_DL_hpLB.Size = new System.Drawing.Size(172, 13);
            this.Battleground_min_Inf_of_light_DL_hpLB.TabIndex = 7;
            this.Battleground_min_Inf_of_light_DL_hpLB.Text = "But only if the target is below this %";
            // 
            // Battleground_min_Inf_of_light_DL_hp
            // 
            this.Battleground_min_Inf_of_light_DL_hp.Location = new System.Drawing.Point(7, 124);
            this.Battleground_min_Inf_of_light_DL_hp.Name = "Battleground_min_Inf_of_light_DL_hp";
            this.Battleground_min_Inf_of_light_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.Battleground_min_Inf_of_light_DL_hp.TabIndex = 6;
            this.Battleground_min_Inf_of_light_DL_hp.ValueChanged += new System.EventHandler(this.Battleground_min_Inf_of_light_DL_hp_ValueChanged);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(70, 26);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(149, 13);
            this.label52.TabIndex = 5;
            this.label52.Text = "Holy Light targets under this %";
            // 
            // Battleground_min_HL_hp
            // 
            this.Battleground_min_HL_hp.Location = new System.Drawing.Point(7, 20);
            this.Battleground_min_HL_hp.Name = "Battleground_min_HL_hp";
            this.Battleground_min_HL_hp.Size = new System.Drawing.Size(56, 20);
            this.Battleground_min_HL_hp.TabIndex = 4;
            this.Battleground_min_HL_hp.ValueChanged += new System.EventHandler(this.Battleground_min_HL_hp_ValueChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(69, 76);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(165, 13);
            this.label53.TabIndex = 3;
            this.label53.Text = "Flash of Light targets under this %";
            // 
            // Battleground_min_FoL_hp
            // 
            this.Battleground_min_FoL_hp.Location = new System.Drawing.Point(7, 74);
            this.Battleground_min_FoL_hp.Name = "Battleground_min_FoL_hp";
            this.Battleground_min_FoL_hp.Size = new System.Drawing.Size(56, 20);
            this.Battleground_min_FoL_hp.TabIndex = 2;
            this.Battleground_min_FoL_hp.ValueChanged += new System.EventHandler(this.Battleground_min_FoL_hp_ValueChanged);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(68, 53);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(158, 13);
            this.label54.TabIndex = 1;
            this.label54.Text = "Divine Light targets under this %";
            // 
            // Battleground_min_DL_hp
            // 
            this.Battleground_min_DL_hp.Location = new System.Drawing.Point(6, 46);
            this.Battleground_min_DL_hp.Name = "Battleground_min_DL_hp";
            this.Battleground_min_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.Battleground_min_DL_hp.TabIndex = 0;
            this.Battleground_min_DL_hp.ValueChanged += new System.EventHandler(this.Battleground_min_DL_hp_ValueChanged);
            // 
            // Battleground_Inf_of_light_wanna_DL
            // 
            this.Battleground_Inf_of_light_wanna_DL.AutoSize = true;
            this.Battleground_Inf_of_light_wanna_DL.Location = new System.Drawing.Point(5, 100);
            this.Battleground_Inf_of_light_wanna_DL.Name = "Battleground_Inf_of_light_wanna_DL";
            this.Battleground_Inf_of_light_wanna_DL.Size = new System.Drawing.Size(235, 17);
            this.Battleground_Inf_of_light_wanna_DL.TabIndex = 2;
            this.Battleground_Inf_of_light_wanna_DL.Text = "Use Divine Light when Infusion of Light proc";
            this.Battleground_Inf_of_light_wanna_DL.UseVisualStyleBackColor = true;
            this.Battleground_Inf_of_light_wanna_DL.CheckedChanged += new System.EventHandler(this.Battleground_Inf_of_light_wanna_DL_CheckedChanged);
            // 
            // groupBox40
            // 
            this.groupBox40.Controls.Add(this.label55);
            this.groupBox40.Controls.Add(this.Battleground_use_mana_rec_trinket_every);
            this.groupBox40.Controls.Add(this.label56);
            this.groupBox40.Controls.Add(this.Battleground_min_mana_rec_trinket);
            this.groupBox40.Controls.Add(this.label57);
            this.groupBox40.Controls.Add(this.Battleground_min_Divine_Plea_mana);
            this.groupBox40.Controls.Add(this.label58);
            this.groupBox40.Controls.Add(this.Battleground_mana_judge);
            this.groupBox40.Location = new System.Drawing.Point(12, 166);
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.Size = new System.Drawing.Size(307, 126);
            this.groupBox40.TabIndex = 65;
            this.groupBox40.TabStop = false;
            this.groupBox40.Text = "Mana Management";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(49, 103);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(186, 13);
            this.label55.TabIndex = 7;
            this.label55.Text = "Activate your mana trinket every (NYI)";
            // 
            // Battleground_use_mana_rec_trinket_every
            // 
            this.Battleground_use_mana_rec_trinket_every.Location = new System.Drawing.Point(0, 100);
            this.Battleground_use_mana_rec_trinket_every.Name = "Battleground_use_mana_rec_trinket_every";
            this.Battleground_use_mana_rec_trinket_every.Size = new System.Drawing.Size(43, 20);
            this.Battleground_use_mana_rec_trinket_every.TabIndex = 6;
            this.Battleground_use_mana_rec_trinket_every.ValueChanged += new System.EventHandler(this.Battleground_use_mana_rec_trinket_every_ValueChanged);
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(49, 78);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(208, 13);
            this.label56.TabIndex = 5;
            this.label56.Text = "Activate your Mana trinket at Mana %(NYI)";
            // 
            // Battleground_min_mana_rec_trinket
            // 
            this.Battleground_min_mana_rec_trinket.Location = new System.Drawing.Point(0, 74);
            this.Battleground_min_mana_rec_trinket.Name = "Battleground_min_mana_rec_trinket";
            this.Battleground_min_mana_rec_trinket.Size = new System.Drawing.Size(43, 20);
            this.Battleground_min_mana_rec_trinket.TabIndex = 4;
            this.Battleground_min_mana_rec_trinket.ValueChanged += new System.EventHandler(this.Battleground_min_mana_rec_trinket_ValueChanged);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(49, 54);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(121, 13);
            this.label57.TabIndex = 3;
            this.label57.Text = "Divine Plea at this mana";
            // 
            // Battleground_min_Divine_Plea_mana
            // 
            this.Battleground_min_Divine_Plea_mana.Location = new System.Drawing.Point(0, 47);
            this.Battleground_min_Divine_Plea_mana.Name = "Battleground_min_Divine_Plea_mana";
            this.Battleground_min_Divine_Plea_mana.Size = new System.Drawing.Size(43, 20);
            this.Battleground_min_Divine_Plea_mana.TabIndex = 2;
            this.Battleground_min_Divine_Plea_mana.ValueChanged += new System.EventHandler(this.Battleground_min_Divine_Plea_mana_ValueChanged);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(49, 27);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(242, 13);
            this.label58.TabIndex = 1;
            this.label58.Text = "Judgement on Cooldown when mana is below this";
            // 
            // Battleground_mana_judge
            // 
            this.Battleground_mana_judge.Location = new System.Drawing.Point(0, 20);
            this.Battleground_mana_judge.Name = "Battleground_mana_judge";
            this.Battleground_mana_judge.Size = new System.Drawing.Size(43, 20);
            this.Battleground_mana_judge.TabIndex = 0;
            this.Battleground_mana_judge.ValueChanged += new System.EventHandler(this.Battleground_mana_judge_ValueChanged);
            // 
            // groupBox41
            // 
            this.groupBox41.Controls.Add(this.label59);
            this.groupBox41.Controls.Add(this.Battleground_min_player_inside_HR);
            this.groupBox41.Controls.Add(this.label60);
            this.groupBox41.Controls.Add(this.Battleground_HR_how_much_health);
            this.groupBox41.Controls.Add(this.label61);
            this.groupBox41.Controls.Add(this.Battleground_HR_how_far);
            this.groupBox41.Controls.Add(this.Battleground_wanna_HR);
            this.groupBox41.Location = new System.Drawing.Point(12, 298);
            this.groupBox41.Name = "groupBox41";
            this.groupBox41.Size = new System.Drawing.Size(307, 133);
            this.groupBox41.TabIndex = 64;
            this.groupBox41.TabStop = false;
            this.groupBox41.Text = "Holy Radiance Settings";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(61, 103);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(145, 13);
            this.label59.TabIndex = 27;
            this.label59.Text = "Need this many people inside";
            // 
            // Battleground_min_player_inside_HR
            // 
            this.Battleground_min_player_inside_HR.Location = new System.Drawing.Point(7, 97);
            this.Battleground_min_player_inside_HR.Name = "Battleground_min_player_inside_HR";
            this.Battleground_min_player_inside_HR.Size = new System.Drawing.Size(44, 20);
            this.Battleground_min_player_inside_HR.TabIndex = 26;
            this.Battleground_min_player_inside_HR.ValueChanged += new System.EventHandler(this.Battleground_min_player_inside_HR_ValueChanged);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(58, 76);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(196, 13);
            this.label60.TabIndex = 25;
            this.label60.Text = "Consider unit with less than this Health%";
            // 
            // Battleground_HR_how_much_health
            // 
            this.Battleground_HR_how_much_health.Location = new System.Drawing.Point(7, 70);
            this.Battleground_HR_how_much_health.Name = "Battleground_HR_how_much_health";
            this.Battleground_HR_how_much_health.Size = new System.Drawing.Size(44, 20);
            this.Battleground_HR_how_much_health.TabIndex = 24;
            this.Battleground_HR_how_much_health.ValueChanged += new System.EventHandler(this.Battleground_HR_how_much_health_ValueChanged);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(58, 49);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(221, 13);
            this.label61.TabIndex = 23;
            this.label61.Text = "Consider unit nearer then this to healed target";
            // 
            // Battleground_HR_how_far
            // 
            this.Battleground_HR_how_far.Location = new System.Drawing.Point(6, 43);
            this.Battleground_HR_how_far.Name = "Battleground_HR_how_far";
            this.Battleground_HR_how_far.Size = new System.Drawing.Size(45, 20);
            this.Battleground_HR_how_far.TabIndex = 22;
            this.Battleground_HR_how_far.ValueChanged += new System.EventHandler(this.Battleground_HR_how_far_ValueChanged);
            // 
            // Battleground_wanna_HR
            // 
            this.Battleground_wanna_HR.AutoSize = true;
            this.Battleground_wanna_HR.Location = new System.Drawing.Point(6, 19);
            this.Battleground_wanna_HR.Name = "Battleground_wanna_HR";
            this.Battleground_wanna_HR.Size = new System.Drawing.Size(96, 17);
            this.Battleground_wanna_HR.TabIndex = 21;
            this.Battleground_wanna_HR.Text = "Holy Radiance";
            this.Battleground_wanna_HR.UseVisualStyleBackColor = true;
            this.Battleground_wanna_HR.CheckedChanged += new System.EventHandler(this.Battleground_wanna_HR_CheckedChanged);
            // 
            // groupBox42
            // 
            this.groupBox42.Controls.Add(this.Battleground_min_mana_potion);
            this.groupBox42.Controls.Add(this.Battleground_min_LoH_hp);
            this.groupBox42.Controls.Add(this.Battleground_min_HoS_hp);
            this.groupBox42.Controls.Add(this.Battleground_min_HoP_hp);
            this.groupBox42.Controls.Add(this.Battleground_min_DS_hp);
            this.groupBox42.Controls.Add(this.label62);
            this.groupBox42.Controls.Add(this.Battleground_min_DP_hp);
            this.groupBox42.Controls.Add(this.Battleground_wanna_mana_potion);
            this.groupBox42.Controls.Add(this.Battleground_wanna_LoH);
            this.groupBox42.Controls.Add(this.Battleground_wanna_DP);
            this.groupBox42.Controls.Add(this.Battleground_wanna_DS);
            this.groupBox42.Controls.Add(this.Battleground_wanna_HoS);
            this.groupBox42.Controls.Add(this.Battleground_wanna_HoP);
            this.groupBox42.Location = new System.Drawing.Point(325, 185);
            this.groupBox42.Name = "groupBox42";
            this.groupBox42.Size = new System.Drawing.Size(247, 182);
            this.groupBox42.TabIndex = 61;
            this.groupBox42.TabStop = false;
            this.groupBox42.Text = "Emergency Buttons";
            // 
            // Battleground_min_mana_potion
            // 
            this.Battleground_min_mana_potion.Location = new System.Drawing.Point(120, 154);
            this.Battleground_min_mana_potion.Name = "Battleground_min_mana_potion";
            this.Battleground_min_mana_potion.Size = new System.Drawing.Size(64, 20);
            this.Battleground_min_mana_potion.TabIndex = 30;
            this.Battleground_min_mana_potion.ValueChanged += new System.EventHandler(this.Battleground_min_mana_potion_ValueChanged);
            // 
            // Battleground_min_LoH_hp
            // 
            this.Battleground_min_LoH_hp.Location = new System.Drawing.Point(120, 130);
            this.Battleground_min_LoH_hp.Name = "Battleground_min_LoH_hp";
            this.Battleground_min_LoH_hp.Size = new System.Drawing.Size(64, 20);
            this.Battleground_min_LoH_hp.TabIndex = 29;
            this.Battleground_min_LoH_hp.ValueChanged += new System.EventHandler(this.Battleground_min_LoH_hp_ValueChanged);
            // 
            // Battleground_min_HoS_hp
            // 
            this.Battleground_min_HoS_hp.Location = new System.Drawing.Point(120, 106);
            this.Battleground_min_HoS_hp.Name = "Battleground_min_HoS_hp";
            this.Battleground_min_HoS_hp.Size = new System.Drawing.Size(64, 20);
            this.Battleground_min_HoS_hp.TabIndex = 28;
            this.Battleground_min_HoS_hp.ValueChanged += new System.EventHandler(this.Battleground_min_HoS_hp_ValueChanged);
            // 
            // Battleground_min_HoP_hp
            // 
            this.Battleground_min_HoP_hp.Location = new System.Drawing.Point(120, 84);
            this.Battleground_min_HoP_hp.Name = "Battleground_min_HoP_hp";
            this.Battleground_min_HoP_hp.Size = new System.Drawing.Size(64, 20);
            this.Battleground_min_HoP_hp.TabIndex = 27;
            this.Battleground_min_HoP_hp.ValueChanged += new System.EventHandler(this.Battleground_min_HoP_hp_ValueChanged);
            // 
            // Battleground_min_DS_hp
            // 
            this.Battleground_min_DS_hp.Location = new System.Drawing.Point(120, 58);
            this.Battleground_min_DS_hp.Name = "Battleground_min_DS_hp";
            this.Battleground_min_DS_hp.Size = new System.Drawing.Size(64, 20);
            this.Battleground_min_DS_hp.TabIndex = 26;
            this.Battleground_min_DS_hp.ValueChanged += new System.EventHandler(this.Battleground_min_DS_hp_ValueChanged);
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(120, 16);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(58, 13);
            this.label62.TabIndex = 25;
            this.label62.Text = "Use Below";
            // 
            // Battleground_min_DP_hp
            // 
            this.Battleground_min_DP_hp.Location = new System.Drawing.Point(120, 35);
            this.Battleground_min_DP_hp.Name = "Battleground_min_DP_hp";
            this.Battleground_min_DP_hp.Size = new System.Drawing.Size(64, 20);
            this.Battleground_min_DP_hp.TabIndex = 24;
            this.Battleground_min_DP_hp.ValueChanged += new System.EventHandler(this.Battleground_min_DP_hp_ValueChanged);
            // 
            // Battleground_wanna_mana_potion
            // 
            this.Battleground_wanna_mana_potion.AutoSize = true;
            this.Battleground_wanna_mana_potion.Location = new System.Drawing.Point(6, 154);
            this.Battleground_wanna_mana_potion.Name = "Battleground_wanna_mana_potion";
            this.Battleground_wanna_mana_potion.Size = new System.Drawing.Size(113, 17);
            this.Battleground_wanna_mana_potion.TabIndex = 23;
            this.Battleground_wanna_mana_potion.Text = "Mana Potion (NYI)";
            this.Battleground_wanna_mana_potion.UseVisualStyleBackColor = true;
            this.Battleground_wanna_mana_potion.CheckedChanged += new System.EventHandler(this.Battleground_wanna_mana_potion_CheckedChanged);
            // 
            // Battleground_wanna_LoH
            // 
            this.Battleground_wanna_LoH.AutoSize = true;
            this.Battleground_wanna_LoH.Location = new System.Drawing.Point(6, 130);
            this.Battleground_wanna_LoH.Name = "Battleground_wanna_LoH";
            this.Battleground_wanna_LoH.Size = new System.Drawing.Size(87, 17);
            this.Battleground_wanna_LoH.TabIndex = 23;
            this.Battleground_wanna_LoH.Text = "Lay on Hand";
            this.Battleground_wanna_LoH.UseVisualStyleBackColor = true;
            this.Battleground_wanna_LoH.CheckedChanged += new System.EventHandler(this.Battleground_wanna_LoH_CheckedChanged);
            // 
            // Battleground_wanna_DP
            // 
            this.Battleground_wanna_DP.AutoSize = true;
            this.Battleground_wanna_DP.Location = new System.Drawing.Point(6, 38);
            this.Battleground_wanna_DP.Name = "Battleground_wanna_DP";
            this.Battleground_wanna_DP.Size = new System.Drawing.Size(107, 17);
            this.Battleground_wanna_DP.TabIndex = 8;
            this.Battleground_wanna_DP.Text = "Divine Protection";
            this.Battleground_wanna_DP.UseVisualStyleBackColor = true;
            this.Battleground_wanna_DP.CheckedChanged += new System.EventHandler(this.Battleground_wanna_DP_CheckedChanged);
            // 
            // Battleground_wanna_DS
            // 
            this.Battleground_wanna_DS.AutoSize = true;
            this.Battleground_wanna_DS.Location = new System.Drawing.Point(6, 61);
            this.Battleground_wanna_DS.Name = "Battleground_wanna_DS";
            this.Battleground_wanna_DS.Size = new System.Drawing.Size(88, 17);
            this.Battleground_wanna_DS.TabIndex = 9;
            this.Battleground_wanna_DS.Text = "Divine Shield";
            this.Battleground_wanna_DS.UseVisualStyleBackColor = true;
            this.Battleground_wanna_DS.CheckedChanged += new System.EventHandler(this.Battleground_wanna_DS_CheckedChanged);
            // 
            // Battleground_wanna_HoS
            // 
            this.Battleground_wanna_HoS.AutoSize = true;
            this.Battleground_wanna_HoS.Location = new System.Drawing.Point(6, 107);
            this.Battleground_wanna_HoS.Name = "Battleground_wanna_HoS";
            this.Battleground_wanna_HoS.Size = new System.Drawing.Size(111, 17);
            this.Battleground_wanna_HoS.TabIndex = 19;
            this.Battleground_wanna_HoS.Text = "Hand of Salvation";
            this.Battleground_wanna_HoS.UseVisualStyleBackColor = true;
            this.Battleground_wanna_HoS.CheckedChanged += new System.EventHandler(this.Battleground_wanna_HoS_CheckedChanged);
            // 
            // Battleground_wanna_HoP
            // 
            this.Battleground_wanna_HoP.AutoSize = true;
            this.Battleground_wanna_HoP.Location = new System.Drawing.Point(6, 84);
            this.Battleground_wanna_HoP.Name = "Battleground_wanna_HoP";
            this.Battleground_wanna_HoP.Size = new System.Drawing.Size(115, 17);
            this.Battleground_wanna_HoP.TabIndex = 18;
            this.Battleground_wanna_HoP.Text = "Hand of Protection";
            this.Battleground_wanna_HoP.UseVisualStyleBackColor = true;
            this.Battleground_wanna_HoP.CheckedChanged += new System.EventHandler(this.Battleground_wanna_HoP_CheckedChanged);
            // 
            // Battleground_auraselctGB
            // 
            this.Battleground_auraselctGB.Controls.Add(this.Battleground_DisabledRB);
            this.Battleground_auraselctGB.Controls.Add(this.Battleground_resistanceRB);
            this.Battleground_auraselctGB.Controls.Add(this.Battleground_concentrationRB);
            this.Battleground_auraselctGB.Location = new System.Drawing.Point(822, 19);
            this.Battleground_auraselctGB.Name = "Battleground_auraselctGB";
            this.Battleground_auraselctGB.Size = new System.Drawing.Size(146, 100);
            this.Battleground_auraselctGB.TabIndex = 63;
            this.Battleground_auraselctGB.TabStop = false;
            this.Battleground_auraselctGB.Text = "Select Aura";
            this.Battleground_auraselctGB.Enter += new System.EventHandler(this.Battleground_auraselctGB_Enter);
            // 
            // Battleground_DisabledRB
            // 
            this.Battleground_DisabledRB.AutoSize = true;
            this.Battleground_DisabledRB.Location = new System.Drawing.Point(7, 68);
            this.Battleground_DisabledRB.Name = "Battleground_DisabledRB";
            this.Battleground_DisabledRB.Size = new System.Drawing.Size(66, 17);
            this.Battleground_DisabledRB.TabIndex = 2;
            this.Battleground_DisabledRB.TabStop = true;
            this.Battleground_DisabledRB.Text = "Disabled";
            this.Battleground_DisabledRB.UseVisualStyleBackColor = true;
            this.Battleground_DisabledRB.CheckedChanged += new System.EventHandler(this.Battleground_DisabledRB_CheckedChanged);
            // 
            // Battleground_resistanceRB
            // 
            this.Battleground_resistanceRB.AutoSize = true;
            this.Battleground_resistanceRB.Location = new System.Drawing.Point(7, 44);
            this.Battleground_resistanceRB.Name = "Battleground_resistanceRB";
            this.Battleground_resistanceRB.Size = new System.Drawing.Size(78, 17);
            this.Battleground_resistanceRB.TabIndex = 1;
            this.Battleground_resistanceRB.TabStop = true;
            this.Battleground_resistanceRB.Text = "Resistance";
            this.Battleground_resistanceRB.UseVisualStyleBackColor = true;
            this.Battleground_resistanceRB.CheckedChanged += new System.EventHandler(this.Battleground_resistanceRB_CheckedChanged);
            // 
            // Battleground_concentrationRB
            // 
            this.Battleground_concentrationRB.AutoSize = true;
            this.Battleground_concentrationRB.Location = new System.Drawing.Point(7, 20);
            this.Battleground_concentrationRB.Name = "Battleground_concentrationRB";
            this.Battleground_concentrationRB.Size = new System.Drawing.Size(91, 17);
            this.Battleground_concentrationRB.TabIndex = 0;
            this.Battleground_concentrationRB.TabStop = true;
            this.Battleground_concentrationRB.Text = "Concentration";
            this.Battleground_concentrationRB.UseVisualStyleBackColor = true;
            this.Battleground_concentrationRB.CheckedChanged += new System.EventHandler(this.Battleground_concentrationRB_CheckedChanged);
            // 
            // groupBox44
            // 
            this.groupBox44.Controls.Add(this.Battleground_min_torrent_mana_perc);
            this.groupBox44.Controls.Add(this.Battleground_min_stoneform);
            this.groupBox44.Controls.Add(this.Battleground_min_gift_hp);
            this.groupBox44.Controls.Add(this.Battleground_wanna_torrent);
            this.groupBox44.Controls.Add(this.Battleground_wanna_stoneform);
            this.groupBox44.Controls.Add(this.Battleground_wanna_everymanforhimself);
            this.groupBox44.Controls.Add(this.Battleground_wanna_gift);
            this.groupBox44.Location = new System.Drawing.Point(578, 170);
            this.groupBox44.Name = "groupBox44";
            this.groupBox44.Size = new System.Drawing.Size(236, 124);
            this.groupBox44.TabIndex = 59;
            this.groupBox44.TabStop = false;
            this.groupBox44.Text = "Racials";
            // 
            // Battleground_min_torrent_mana_perc
            // 
            this.Battleground_min_torrent_mana_perc.Location = new System.Drawing.Point(172, 95);
            this.Battleground_min_torrent_mana_perc.Name = "Battleground_min_torrent_mana_perc";
            this.Battleground_min_torrent_mana_perc.Size = new System.Drawing.Size(59, 20);
            this.Battleground_min_torrent_mana_perc.TabIndex = 17;
            this.Battleground_min_torrent_mana_perc.ValueChanged += new System.EventHandler(this.Battleground_min_torrent_mana_perc_ValueChanged);
            // 
            // Battleground_min_stoneform
            // 
            this.Battleground_min_stoneform.Location = new System.Drawing.Point(172, 71);
            this.Battleground_min_stoneform.Name = "Battleground_min_stoneform";
            this.Battleground_min_stoneform.Size = new System.Drawing.Size(59, 20);
            this.Battleground_min_stoneform.TabIndex = 16;
            this.Battleground_min_stoneform.ValueChanged += new System.EventHandler(this.Battleground_min_stoneform_ValueChanged);
            // 
            // Battleground_min_gift_hp
            // 
            this.Battleground_min_gift_hp.Location = new System.Drawing.Point(172, 45);
            this.Battleground_min_gift_hp.Name = "Battleground_min_gift_hp";
            this.Battleground_min_gift_hp.Size = new System.Drawing.Size(59, 20);
            this.Battleground_min_gift_hp.TabIndex = 15;
            this.Battleground_min_gift_hp.ValueChanged += new System.EventHandler(this.Battleground_min_gift_hp_ValueChanged);
            // 
            // Battleground_wanna_torrent
            // 
            this.Battleground_wanna_torrent.AutoSize = true;
            this.Battleground_wanna_torrent.Location = new System.Drawing.Point(16, 95);
            this.Battleground_wanna_torrent.Name = "Battleground_wanna_torrent";
            this.Battleground_wanna_torrent.Size = new System.Drawing.Size(147, 17);
            this.Battleground_wanna_torrent.TabIndex = 14;
            this.Battleground_wanna_torrent.Text = "Arcane Torrent (for mana)";
            this.Battleground_wanna_torrent.UseVisualStyleBackColor = true;
            this.Battleground_wanna_torrent.CheckedChanged += new System.EventHandler(this.Battleground_wanna_torrent_CheckedChanged);
            // 
            // Battleground_wanna_stoneform
            // 
            this.Battleground_wanna_stoneform.AutoSize = true;
            this.Battleground_wanna_stoneform.Location = new System.Drawing.Point(16, 71);
            this.Battleground_wanna_stoneform.Name = "Battleground_wanna_stoneform";
            this.Battleground_wanna_stoneform.Size = new System.Drawing.Size(74, 17);
            this.Battleground_wanna_stoneform.TabIndex = 13;
            this.Battleground_wanna_stoneform.Text = "Stoneform";
            this.Battleground_wanna_stoneform.UseVisualStyleBackColor = true;
            this.Battleground_wanna_stoneform.CheckedChanged += new System.EventHandler(this.Battleground_wanna_stoneform_CheckedChanged);
            // 
            // Battleground_wanna_everymanforhimself
            // 
            this.Battleground_wanna_everymanforhimself.AutoSize = true;
            this.Battleground_wanna_everymanforhimself.Location = new System.Drawing.Point(16, 24);
            this.Battleground_wanna_everymanforhimself.Name = "Battleground_wanna_everymanforhimself";
            this.Battleground_wanna_everymanforhimself.Size = new System.Drawing.Size(129, 17);
            this.Battleground_wanna_everymanforhimself.TabIndex = 10;
            this.Battleground_wanna_everymanforhimself.Text = "Every Man for Himself";
            this.Battleground_wanna_everymanforhimself.UseVisualStyleBackColor = true;
            this.Battleground_wanna_everymanforhimself.CheckedChanged += new System.EventHandler(this.Battleground_wanna_everymanforhimself_CheckedChanged);
            // 
            // Battleground_wanna_gift
            // 
            this.Battleground_wanna_gift.AutoSize = true;
            this.Battleground_wanna_gift.Location = new System.Drawing.Point(16, 47);
            this.Battleground_wanna_gift.Name = "Battleground_wanna_gift";
            this.Battleground_wanna_gift.Size = new System.Drawing.Size(104, 17);
            this.Battleground_wanna_gift.TabIndex = 12;
            this.Battleground_wanna_gift.Text = "Gift of the Naaru";
            this.Battleground_wanna_gift.UseVisualStyleBackColor = true;
            this.Battleground_wanna_gift.CheckedChanged += new System.EventHandler(this.Battleground_wanna_gift_CheckedChanged);
            // 
            // groupBox45
            // 
            this.groupBox45.Controls.Add(this.Battleground_do_not_dismount_EVER);
            this.groupBox45.Controls.Add(this.Battleground_do_not_dismount_ooc);
            this.groupBox45.Controls.Add(this.Battleground_wanna_move_to_HoJ);
            this.groupBox45.Controls.Add(this.Battleground_wanna_mount);
            this.groupBox45.Controls.Add(this.Battleground_wanna_move_to_heal);
            this.groupBox45.Controls.Add(this.Battleground_wanna_crusader);
            this.groupBox45.Location = new System.Drawing.Point(325, 373);
            this.groupBox45.Name = "groupBox45";
            this.groupBox45.Size = new System.Drawing.Size(249, 156);
            this.groupBox45.TabIndex = 62;
            this.groupBox45.TabStop = false;
            this.groupBox45.Text = "Movement";
            // 
            // Battleground_do_not_dismount_EVER
            // 
            this.Battleground_do_not_dismount_EVER.AutoSize = true;
            this.Battleground_do_not_dismount_EVER.Location = new System.Drawing.Point(6, 137);
            this.Battleground_do_not_dismount_EVER.Name = "Battleground_do_not_dismount_EVER";
            this.Battleground_do_not_dismount_EVER.Size = new System.Drawing.Size(130, 17);
            this.Battleground_do_not_dismount_EVER.TabIndex = 27;
            this.Battleground_do_not_dismount_EVER.Text = "Don\'t Dismount EVER";
            this.Battleground_do_not_dismount_EVER.UseVisualStyleBackColor = true;
            this.Battleground_do_not_dismount_EVER.CheckedChanged += new System.EventHandler(this.Battleground_do_not_dismount_EVER_CheckedChanged);
            // 
            // Battleground_do_not_dismount_ooc
            // 
            this.Battleground_do_not_dismount_ooc.AutoSize = true;
            this.Battleground_do_not_dismount_ooc.Location = new System.Drawing.Point(6, 113);
            this.Battleground_do_not_dismount_ooc.Name = "Battleground_do_not_dismount_ooc";
            this.Battleground_do_not_dismount_ooc.Size = new System.Drawing.Size(169, 17);
            this.Battleground_do_not_dismount_ooc.TabIndex = 26;
            this.Battleground_do_not_dismount_ooc.Text = "Don\'t Dismount Out of Combat";
            this.Battleground_do_not_dismount_ooc.UseVisualStyleBackColor = true;
            this.Battleground_do_not_dismount_ooc.CheckedChanged += new System.EventHandler(this.Battleground_do_not_dismount_ooc_CheckedChanged);
            // 
            // Battleground_wanna_move_to_HoJ
            // 
            this.Battleground_wanna_move_to_HoJ.AutoSize = true;
            this.Battleground_wanna_move_to_HoJ.Location = new System.Drawing.Point(7, 66);
            this.Battleground_wanna_move_to_HoJ.Name = "Battleground_wanna_move_to_HoJ";
            this.Battleground_wanna_move_to_HoJ.Size = new System.Drawing.Size(201, 17);
            this.Battleground_wanna_move_to_HoJ.TabIndex = 25;
            this.Battleground_wanna_move_to_HoJ.Text = "Move in Range to Hammer of Justice";
            this.Battleground_wanna_move_to_HoJ.UseVisualStyleBackColor = true;
            this.Battleground_wanna_move_to_HoJ.CheckedChanged += new System.EventHandler(this.Battleground_wanna_move_to_HoJ_CheckedChanged);
            // 
            // Battleground_wanna_mount
            // 
            this.Battleground_wanna_mount.AutoSize = true;
            this.Battleground_wanna_mount.Location = new System.Drawing.Point(6, 19);
            this.Battleground_wanna_mount.Name = "Battleground_wanna_mount";
            this.Battleground_wanna_mount.Size = new System.Drawing.Size(73, 17);
            this.Battleground_wanna_mount.TabIndex = 23;
            this.Battleground_wanna_mount.Text = "Mount Up";
            this.Battleground_wanna_mount.UseVisualStyleBackColor = true;
            this.Battleground_wanna_mount.CheckedChanged += new System.EventHandler(this.Battleground_wanna_mount_CheckedChanged);
            // 
            // Battleground_wanna_move_to_heal
            // 
            this.Battleground_wanna_move_to_heal.AutoSize = true;
            this.Battleground_wanna_move_to_heal.Location = new System.Drawing.Point(6, 42);
            this.Battleground_wanna_move_to_heal.Name = "Battleground_wanna_move_to_heal";
            this.Battleground_wanna_move_to_heal.Size = new System.Drawing.Size(136, 17);
            this.Battleground_wanna_move_to_heal.TabIndex = 24;
            this.Battleground_wanna_move_to_heal.Text = "Move in Range to Heal";
            this.Battleground_wanna_move_to_heal.UseVisualStyleBackColor = true;
            this.Battleground_wanna_move_to_heal.CheckedChanged += new System.EventHandler(this.Battleground_wanna_move_to_heal_CheckedChanged);
            // 
            // Battleground_wanna_crusader
            // 
            this.Battleground_wanna_crusader.AutoSize = true;
            this.Battleground_wanna_crusader.Location = new System.Drawing.Point(6, 89);
            this.Battleground_wanna_crusader.Name = "Battleground_wanna_crusader";
            this.Battleground_wanna_crusader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Battleground_wanna_crusader.Size = new System.Drawing.Size(213, 17);
            this.Battleground_wanna_crusader.TabIndex = 0;
            this.Battleground_wanna_crusader.Text = "Switch to Crusader Aura when mounted";
            this.Battleground_wanna_crusader.UseVisualStyleBackColor = true;
            this.Battleground_wanna_crusader.CheckedChanged += new System.EventHandler(this.Battleground_wanna_crusader_CheckedChanged);
            // 
            // Battleground_generalGB
            // 
            this.Battleground_generalGB.Controls.Add(this.label63);
            this.Battleground_generalGB.Controls.Add(this.Battleground_rest_if_mana_below);
            this.Battleground_generalGB.Controls.Add(this.Battleground_wanna_buff);
            this.Battleground_generalGB.Location = new System.Drawing.Point(12, 19);
            this.Battleground_generalGB.Name = "Battleground_generalGB";
            this.Battleground_generalGB.Size = new System.Drawing.Size(307, 66);
            this.Battleground_generalGB.TabIndex = 60;
            this.Battleground_generalGB.TabStop = false;
            this.Battleground_generalGB.Text = "General";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(64, 44);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(101, 13);
            this.label63.TabIndex = 6;
            this.label63.Text = "Rest at this Mana %";
            // 
            // Battleground_rest_if_mana_below
            // 
            this.Battleground_rest_if_mana_below.Location = new System.Drawing.Point(5, 41);
            this.Battleground_rest_if_mana_below.Name = "Battleground_rest_if_mana_below";
            this.Battleground_rest_if_mana_below.Size = new System.Drawing.Size(53, 20);
            this.Battleground_rest_if_mana_below.TabIndex = 5;
            this.Battleground_rest_if_mana_below.ValueChanged += new System.EventHandler(this.Battleground_rest_if_mana_below_ValueChanged);
            // 
            // Battleground_wanna_buff
            // 
            this.Battleground_wanna_buff.AutoSize = true;
            this.Battleground_wanna_buff.Location = new System.Drawing.Point(5, 19);
            this.Battleground_wanna_buff.Name = "Battleground_wanna_buff";
            this.Battleground_wanna_buff.Size = new System.Drawing.Size(86, 17);
            this.Battleground_wanna_buff.TabIndex = 4;
            this.Battleground_wanna_buff.Text = "Enable Buffs";
            this.Battleground_wanna_buff.UseVisualStyleBackColor = true;
            this.Battleground_wanna_buff.CheckedChanged += new System.EventHandler(this.Battleground_wanna_buff_CheckedChanged);
            // 
            // groupBox47
            // 
            this.groupBox47.Controls.Add(this.Battleground_wanna_Judge);
            this.groupBox47.Controls.Add(this.Battleground_wanna_CS);
            this.groupBox47.Controls.Add(this.Battleground_wanna_HoW);
            this.groupBox47.Location = new System.Drawing.Point(822, 208);
            this.groupBox47.Name = "groupBox47";
            this.groupBox47.Size = new System.Drawing.Size(146, 100);
            this.groupBox47.TabIndex = 58;
            this.groupBox47.TabStop = false;
            this.groupBox47.Text = "DPS";
            // 
            // Battleground_wanna_Judge
            // 
            this.Battleground_wanna_Judge.AutoSize = true;
            this.Battleground_wanna_Judge.Location = new System.Drawing.Point(17, 70);
            this.Battleground_wanna_Judge.Name = "Battleground_wanna_Judge";
            this.Battleground_wanna_Judge.Size = new System.Drawing.Size(78, 17);
            this.Battleground_wanna_Judge.TabIndex = 23;
            this.Battleground_wanna_Judge.Text = "Judgement";
            this.Battleground_wanna_Judge.UseVisualStyleBackColor = true;
            this.Battleground_wanna_Judge.CheckedChanged += new System.EventHandler(this.Battleground_wanna_Judge_CheckedChanged);
            // 
            // Battleground_wanna_CS
            // 
            this.Battleground_wanna_CS.AutoSize = true;
            this.Battleground_wanna_CS.Location = new System.Drawing.Point(17, 24);
            this.Battleground_wanna_CS.Name = "Battleground_wanna_CS";
            this.Battleground_wanna_CS.Size = new System.Drawing.Size(98, 17);
            this.Battleground_wanna_CS.TabIndex = 6;
            this.Battleground_wanna_CS.Text = "Crusader Strike";
            this.Battleground_wanna_CS.UseVisualStyleBackColor = true;
            this.Battleground_wanna_CS.CheckedChanged += new System.EventHandler(this.Battleground_wanna_CS_CheckedChanged);
            // 
            // Battleground_wanna_HoW
            // 
            this.Battleground_wanna_HoW.AutoSize = true;
            this.Battleground_wanna_HoW.Location = new System.Drawing.Point(17, 47);
            this.Battleground_wanna_HoW.Name = "Battleground_wanna_HoW";
            this.Battleground_wanna_HoW.Size = new System.Drawing.Size(109, 17);
            this.Battleground_wanna_HoW.TabIndex = 20;
            this.Battleground_wanna_HoW.Text = "Hammer of Wrath";
            this.Battleground_wanna_HoW.UseVisualStyleBackColor = true;
            this.Battleground_wanna_HoW.CheckedChanged += new System.EventHandler(this.Battleground_wanna_HoW_CheckedChanged);
            // 
            // groupBox48
            // 
            this.groupBox48.Controls.Add(this.Battleground_wanna_lifeblood);
            this.groupBox48.Controls.Add(this.label64);
            this.groupBox48.Controls.Add(this.Battleground_min_ohshitbutton_activator);
            this.groupBox48.Controls.Add(this.Battleground_wanna_GotAK);
            this.groupBox48.Controls.Add(this.Battleground_wanna_AW);
            this.groupBox48.Controls.Add(this.Battleground_wanna_DF);
            this.groupBox48.Location = new System.Drawing.Point(580, 19);
            this.groupBox48.Name = "groupBox48";
            this.groupBox48.Size = new System.Drawing.Size(236, 145);
            this.groupBox48.TabIndex = 57;
            this.groupBox48.TabStop = false;
            this.groupBox48.Text = "Oh Shit! Buttons";
            // 
            // Battleground_wanna_lifeblood
            // 
            this.Battleground_wanna_lifeblood.AutoSize = true;
            this.Battleground_wanna_lifeblood.Location = new System.Drawing.Point(6, 47);
            this.Battleground_wanna_lifeblood.Name = "Battleground_wanna_lifeblood";
            this.Battleground_wanna_lifeblood.Size = new System.Drawing.Size(69, 17);
            this.Battleground_wanna_lifeblood.TabIndex = 11;
            this.Battleground_wanna_lifeblood.Text = "Lifeblood";
            this.Battleground_wanna_lifeblood.UseVisualStyleBackColor = true;
            this.Battleground_wanna_lifeblood.CheckedChanged += new System.EventHandler(this.Battleground_wanna_lifeblood_CheckedChanged);
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(63, 24);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(135, 13);
            this.label64.TabIndex = 10;
            this.label64.Text = "Press if someone is this low";
            // 
            // Battleground_min_ohshitbutton_activator
            // 
            this.Battleground_min_ohshitbutton_activator.Location = new System.Drawing.Point(6, 20);
            this.Battleground_min_ohshitbutton_activator.Name = "Battleground_min_ohshitbutton_activator";
            this.Battleground_min_ohshitbutton_activator.Size = new System.Drawing.Size(50, 20);
            this.Battleground_min_ohshitbutton_activator.TabIndex = 9;
            this.Battleground_min_ohshitbutton_activator.ValueChanged += new System.EventHandler(this.Battleground_min_ohshitbutton_activator_ValueChanged);
            // 
            // Battleground_wanna_GotAK
            // 
            this.Battleground_wanna_GotAK.AutoSize = true;
            this.Battleground_wanna_GotAK.Location = new System.Drawing.Point(6, 122);
            this.Battleground_wanna_GotAK.Name = "Battleground_wanna_GotAK";
            this.Battleground_wanna_GotAK.Size = new System.Drawing.Size(167, 17);
            this.Battleground_wanna_GotAK.TabIndex = 8;
            this.Battleground_wanna_GotAK.Text = "Guardian of the Ancient Kings";
            this.Battleground_wanna_GotAK.UseVisualStyleBackColor = true;
            this.Battleground_wanna_GotAK.CheckedChanged += new System.EventHandler(this.Battleground_wanna_GotAK_CheckedChanged);
            // 
            // Battleground_wanna_AW
            // 
            this.Battleground_wanna_AW.AutoSize = true;
            this.Battleground_wanna_AW.Location = new System.Drawing.Point(6, 70);
            this.Battleground_wanna_AW.Name = "Battleground_wanna_AW";
            this.Battleground_wanna_AW.Size = new System.Drawing.Size(103, 17);
            this.Battleground_wanna_AW.TabIndex = 3;
            this.Battleground_wanna_AW.Text = "Avenging Wrath";
            this.Battleground_wanna_AW.UseVisualStyleBackColor = true;
            this.Battleground_wanna_AW.CheckedChanged += new System.EventHandler(this.Battleground_wanna_AW_CheckedChanged);
            // 
            // Battleground_wanna_DF
            // 
            this.Battleground_wanna_DF.AutoSize = true;
            this.Battleground_wanna_DF.Location = new System.Drawing.Point(6, 95);
            this.Battleground_wanna_DF.Name = "Battleground_wanna_DF";
            this.Battleground_wanna_DF.Size = new System.Drawing.Size(86, 17);
            this.Battleground_wanna_DF.TabIndex = 7;
            this.Battleground_wanna_DF.Text = "Divine Favor";
            this.Battleground_wanna_DF.UseVisualStyleBackColor = true;
            this.Battleground_wanna_DF.CheckedChanged += new System.EventHandler(this.Battleground_wanna_DF_CheckedChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.AutoScroll = true;
            this.tabPage6.Controls.Add(this.ARENA_optimizeGB);
            this.tabPage6.Controls.Add(this.ARENA_bless_selection);
            this.tabPage6.Controls.Add(this.ARENA_special_logics);
            this.tabPage6.Controls.Add(this.groupBox5);
            this.tabPage6.Controls.Add(this.groupBox6);
            this.tabPage6.Controls.Add(this.panel2);
            this.tabPage6.Controls.Add(this.groupBox9);
            this.tabPage6.Controls.Add(this.groupBox10);
            this.tabPage6.Controls.Add(this.groupBox11);
            this.tabPage6.Controls.Add(this.groupBox12);
            this.tabPage6.Controls.Add(this.groupBox13);
            this.tabPage6.Controls.Add(this.groupBox14);
            this.tabPage6.Controls.Add(this.ARENA_auraselctGB);
            this.tabPage6.Controls.Add(this.groupBox16);
            this.tabPage6.Controls.Add(this.groupBox17);
            this.tabPage6.Controls.Add(this.Arena_generalGB);
            this.tabPage6.Controls.Add(this.groupBox19);
            this.tabPage6.Controls.Add(this.groupBox20);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(994, 653);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Arena";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // ARENA_optimizeGB
            // 
            this.ARENA_optimizeGB.Controls.Add(this.ARENA_intellywait);
            this.ARENA_optimizeGB.Controls.Add(this.ARENA_accurancy);
            this.ARENA_optimizeGB.Controls.Add(this.ARENA_speed);
            this.ARENA_optimizeGB.Location = new System.Drawing.Point(580, 512);
            this.ARENA_optimizeGB.Name = "ARENA_optimizeGB";
            this.ARENA_optimizeGB.Size = new System.Drawing.Size(200, 100);
            this.ARENA_optimizeGB.TabIndex = 61;
            this.ARENA_optimizeGB.TabStop = false;
            this.ARENA_optimizeGB.Text = "Optimize the CC for";
            this.ARENA_optimizeGB.Enter += new System.EventHandler(this.ARENA_optimizeGB_Enter);
            // 
            // ARENA_intellywait
            // 
            this.ARENA_intellywait.AutoSize = true;
            this.ARENA_intellywait.Location = new System.Drawing.Point(7, 68);
            this.ARENA_intellywait.Name = "ARENA_intellywait";
            this.ARENA_intellywait.Size = new System.Drawing.Size(159, 17);
            this.ARENA_intellywait.TabIndex = 2;
            this.ARENA_intellywait.TabStop = true;
            this.ARENA_intellywait.Tag = "optimize";
            this.ARENA_intellywait.Text = "IntellyWait (combat sistem 6)";
            this.ARENA_intellywait.UseVisualStyleBackColor = true;
            this.ARENA_intellywait.CheckedChanged += new System.EventHandler(this.ARENA_intellywait_CheckedChanged);
            // 
            // ARENA_accurancy
            // 
            this.ARENA_accurancy.AutoSize = true;
            this.ARENA_accurancy.Location = new System.Drawing.Point(7, 44);
            this.ARENA_accurancy.Name = "ARENA_accurancy";
            this.ARENA_accurancy.Size = new System.Drawing.Size(161, 17);
            this.ARENA_accurancy.TabIndex = 1;
            this.ARENA_accurancy.TabStop = true;
            this.ARENA_accurancy.Tag = "optimize";
            this.ARENA_accurancy.Text = "Accurancy (combat sistem 5)";
            this.ARENA_accurancy.UseVisualStyleBackColor = true;
            this.ARENA_accurancy.CheckedChanged += new System.EventHandler(this.ARENA_accurancy_CheckedChanged);
            // 
            // ARENA_speed
            // 
            this.ARENA_speed.AutoSize = true;
            this.ARENA_speed.Location = new System.Drawing.Point(8, 20);
            this.ARENA_speed.Name = "ARENA_speed";
            this.ARENA_speed.Size = new System.Drawing.Size(141, 17);
            this.ARENA_speed.TabIndex = 0;
            this.ARENA_speed.TabStop = true;
            this.ARENA_speed.Tag = "optimize";
            this.ARENA_speed.Text = "Speed (combat sistem 4)";
            this.ARENA_speed.UseVisualStyleBackColor = true;
            this.ARENA_speed.CheckedChanged += new System.EventHandler(this.ARENA_speed_CheckedChanged);
            // 
            // ARENA_bless_selection
            // 
            this.ARENA_bless_selection.Controls.Add(this.ARENA_bless_type_disabledRB);
            this.ARENA_bless_selection.Controls.Add(this.ARENA_bless_type_MightRB);
            this.ARENA_bless_selection.Controls.Add(this.ARENA_bless_type_KingRB);
            this.ARENA_bless_selection.Controls.Add(this.ARENA_bless_type_autoRB);
            this.ARENA_bless_selection.Location = new System.Drawing.Point(822, 415);
            this.ARENA_bless_selection.Name = "ARENA_bless_selection";
            this.ARENA_bless_selection.Size = new System.Drawing.Size(146, 118);
            this.ARENA_bless_selection.TabIndex = 60;
            this.ARENA_bless_selection.TabStop = false;
            this.ARENA_bless_selection.Text = "Bless Selection";
            this.ARENA_bless_selection.Enter += new System.EventHandler(this.ARENA_bless_selection_Enter);
            // 
            // ARENA_bless_type_disabledRB
            // 
            this.ARENA_bless_type_disabledRB.AutoSize = true;
            this.ARENA_bless_type_disabledRB.Location = new System.Drawing.Point(7, 92);
            this.ARENA_bless_type_disabledRB.Name = "ARENA_bless_type_disabledRB";
            this.ARENA_bless_type_disabledRB.Size = new System.Drawing.Size(66, 17);
            this.ARENA_bless_type_disabledRB.TabIndex = 3;
            this.ARENA_bless_type_disabledRB.TabStop = true;
            this.ARENA_bless_type_disabledRB.Tag = "Bless";
            this.ARENA_bless_type_disabledRB.Text = "Disabled";
            this.ARENA_bless_type_disabledRB.UseVisualStyleBackColor = true;
            this.ARENA_bless_type_disabledRB.CheckedChanged += new System.EventHandler(this.ARENA_bless_type_disabledRB_CheckedChanged);
            // 
            // ARENA_bless_type_MightRB
            // 
            this.ARENA_bless_type_MightRB.AutoSize = true;
            this.ARENA_bless_type_MightRB.Location = new System.Drawing.Point(7, 68);
            this.ARENA_bless_type_MightRB.Name = "ARENA_bless_type_MightRB";
            this.ARENA_bless_type_MightRB.Size = new System.Drawing.Size(51, 17);
            this.ARENA_bless_type_MightRB.TabIndex = 2;
            this.ARENA_bless_type_MightRB.TabStop = true;
            this.ARENA_bless_type_MightRB.Tag = "Bless";
            this.ARENA_bless_type_MightRB.Text = "Might";
            this.ARENA_bless_type_MightRB.UseVisualStyleBackColor = true;
            this.ARENA_bless_type_MightRB.CheckedChanged += new System.EventHandler(this.ARENA_bless_type_MightRB_CheckedChanged);
            // 
            // ARENA_bless_type_KingRB
            // 
            this.ARENA_bless_type_KingRB.AutoSize = true;
            this.ARENA_bless_type_KingRB.Location = new System.Drawing.Point(7, 44);
            this.ARENA_bless_type_KingRB.Name = "ARENA_bless_type_KingRB";
            this.ARENA_bless_type_KingRB.Size = new System.Drawing.Size(46, 17);
            this.ARENA_bless_type_KingRB.TabIndex = 1;
            this.ARENA_bless_type_KingRB.TabStop = true;
            this.ARENA_bless_type_KingRB.Tag = "Bless";
            this.ARENA_bless_type_KingRB.Text = "King";
            this.ARENA_bless_type_KingRB.UseVisualStyleBackColor = true;
            this.ARENA_bless_type_KingRB.CheckedChanged += new System.EventHandler(this.ARENA_bless_type_KingRB_CheckedChanged);
            // 
            // ARENA_bless_type_autoRB
            // 
            this.ARENA_bless_type_autoRB.AutoSize = true;
            this.ARENA_bless_type_autoRB.Location = new System.Drawing.Point(7, 20);
            this.ARENA_bless_type_autoRB.Name = "ARENA_bless_type_autoRB";
            this.ARENA_bless_type_autoRB.Size = new System.Drawing.Size(47, 17);
            this.ARENA_bless_type_autoRB.TabIndex = 0;
            this.ARENA_bless_type_autoRB.TabStop = true;
            this.ARENA_bless_type_autoRB.Tag = "Bless";
            this.ARENA_bless_type_autoRB.Text = "Auto";
            this.ARENA_bless_type_autoRB.UseVisualStyleBackColor = true;
            this.ARENA_bless_type_autoRB.CheckedChanged += new System.EventHandler(this.ARENA_bless_type_autoRB_CheckedChanged);
            // 
            // ARENA_special_logics
            // 
            this.ARENA_special_logics.Controls.Add(this.ARENA_wanna_HoF);
            this.ARENA_special_logics.Controls.Add(this.ARENA_wanna_taunt);
            this.ARENA_special_logics.Location = new System.Drawing.Point(580, 406);
            this.ARENA_special_logics.Name = "ARENA_special_logics";
            this.ARENA_special_logics.Size = new System.Drawing.Size(234, 100);
            this.ARENA_special_logics.TabIndex = 59;
            this.ARENA_special_logics.TabStop = false;
            this.ARENA_special_logics.Text = "Special Logics";
            // 
            // ARENA_wanna_HoF
            // 
            this.ARENA_wanna_HoF.AutoSize = true;
            this.ARENA_wanna_HoF.Location = new System.Drawing.Point(6, 43);
            this.ARENA_wanna_HoF.Name = "ARENA_wanna_HoF";
            this.ARENA_wanna_HoF.Size = new System.Drawing.Size(158, 17);
            this.ARENA_wanna_HoF.TabIndex = 35;
            this.ARENA_wanna_HoF.Text = "Hand of Freedom on snared";
            this.ARENA_wanna_HoF.UseVisualStyleBackColor = true;
            this.ARENA_wanna_HoF.CheckedChanged += new System.EventHandler(this.ARENA_wanna_HoF_CheckedChanged);
            // 
            // ARENA_wanna_taunt
            // 
            this.ARENA_wanna_taunt.AutoSize = true;
            this.ARENA_wanna_taunt.Location = new System.Drawing.Point(6, 19);
            this.ARENA_wanna_taunt.Name = "ARENA_wanna_taunt";
            this.ARENA_wanna_taunt.Size = new System.Drawing.Size(136, 17);
            this.ARENA_wanna_taunt.TabIndex = 34;
            this.ARENA_wanna_taunt.Text = "Taunt Enemy Pet if any";
            this.ARENA_wanna_taunt.UseVisualStyleBackColor = true;
            this.ARENA_wanna_taunt.CheckedChanged += new System.EventHandler(this.ARENA_wanna_taunt_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(578, 300);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(236, 100);
            this.groupBox5.TabIndex = 57;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Overhealing Protection";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.ARENA_get_tank_from_lua);
            this.groupBox6.Controls.Add(this.ARENA_get_tank_from_focus);
            this.groupBox6.Location = new System.Drawing.Point(12, 91);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(307, 69);
            this.groupBox6.TabIndex = 56;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Tank Selection";
            // 
            // ARENA_get_tank_from_lua
            // 
            this.ARENA_get_tank_from_lua.AutoSize = true;
            this.ARENA_get_tank_from_lua.Location = new System.Drawing.Point(8, 42);
            this.ARENA_get_tank_from_lua.Name = "ARENA_get_tank_from_lua";
            this.ARENA_get_tank_from_lua.Size = new System.Drawing.Size(156, 17);
            this.ARENA_get_tank_from_lua.TabIndex = 2;
            this.ARENA_get_tank_from_lua.Text = "Use the ingame RoleCheck";
            this.ARENA_get_tank_from_lua.UseVisualStyleBackColor = true;
            this.ARENA_get_tank_from_lua.CheckedChanged += new System.EventHandler(this.ARENA_get_tank_from_lua_CheckedChanged);
            // 
            // ARENA_get_tank_from_focus
            // 
            this.ARENA_get_tank_from_focus.AutoSize = true;
            this.ARENA_get_tank_from_focus.Location = new System.Drawing.Point(8, 19);
            this.ARENA_get_tank_from_focus.Name = "ARENA_get_tank_from_focus";
            this.ARENA_get_tank_from_focus.Size = new System.Drawing.Size(179, 17);
            this.ARENA_get_tank_from_focus.TabIndex = 1;
            this.ARENA_get_tank_from_focus.Text = "Import the Tank from your Focus";
            this.ARENA_get_tank_from_focus.UseVisualStyleBackColor = true;
            this.ARENA_get_tank_from_focus.CheckedChanged += new System.EventHandler(this.ARENA_get_tank_from_focus_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ARENA_advanced_option);
            this.panel2.Controls.Add(this.ARENA_advanced);
            this.panel2.Location = new System.Drawing.Point(12, 437);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(313, 202);
            this.panel2.TabIndex = 55;
            // 
            // ARENA_advanced_option
            // 
            this.ARENA_advanced_option.AutoSize = true;
            this.ARENA_advanced_option.Location = new System.Drawing.Point(6, 3);
            this.ARENA_advanced_option.Name = "ARENA_advanced_option";
            this.ARENA_advanced_option.Size = new System.Drawing.Size(144, 17);
            this.ARENA_advanced_option.TabIndex = 34;
            this.ARENA_advanced_option.Text = "Show Advanced Options";
            this.ARENA_advanced_option.UseVisualStyleBackColor = true;
            this.ARENA_advanced_option.CheckedChanged += new System.EventHandler(this.ARENA_advanced_option_CheckedChanged);
            // 
            // ARENA_advanced
            // 
            this.ARENA_advanced.Controls.Add(this.label3);
            this.ARENA_advanced.Controls.Add(this.ARENA_max_healing_distance);
            this.ARENA_advanced.Controls.Add(this.ARENA_wanna_target);
            this.ARENA_advanced.Controls.Add(this.groupBox8);
            this.ARENA_advanced.Controls.Add(this.ARENA_wanna_face);
            this.ARENA_advanced.Location = new System.Drawing.Point(6, 26);
            this.ARENA_advanced.Name = "ARENA_advanced";
            this.ARENA_advanced.Size = new System.Drawing.Size(301, 173);
            this.ARENA_advanced.TabIndex = 33;
            this.ARENA_advanced.TabStop = false;
            this.ARENA_advanced.Text = "Advanced Options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Ignore unit more distant than this";
            // 
            // ARENA_max_healing_distance
            // 
            this.ARENA_max_healing_distance.Location = new System.Drawing.Point(13, 117);
            this.ARENA_max_healing_distance.Name = "ARENA_max_healing_distance";
            this.ARENA_max_healing_distance.Size = new System.Drawing.Size(52, 20);
            this.ARENA_max_healing_distance.TabIndex = 32;
            this.ARENA_max_healing_distance.ValueChanged += new System.EventHandler(this.ARENA_max_healing_distance_ValueChanged);
            // 
            // ARENA_wanna_target
            // 
            this.ARENA_wanna_target.AutoSize = true;
            this.ARENA_wanna_target.Location = new System.Drawing.Point(13, 101);
            this.ARENA_wanna_target.Name = "ARENA_wanna_target";
            this.ARENA_wanna_target.Size = new System.Drawing.Size(216, 17);
            this.ARENA_wanna_target.TabIndex = 27;
            this.ARENA_wanna_target.Text = "Target if no target (prevent HBCore bug)";
            this.ARENA_wanna_target.UseVisualStyleBackColor = true;
            this.ARENA_wanna_target.CheckedChanged += new System.EventHandler(this.ARENA_wanna_target_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.ARENA_do_not_heal_above);
            this.groupBox8.Controls.Add(this.label18);
            this.groupBox8.Location = new System.Drawing.Point(13, 19);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(248, 53);
            this.groupBox8.TabIndex = 31;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Do not consider People above this health";
            // 
            // ARENA_do_not_heal_above
            // 
            this.ARENA_do_not_heal_above.Location = new System.Drawing.Point(6, 19);
            this.ARENA_do_not_heal_above.Name = "ARENA_do_not_heal_above";
            this.ARENA_do_not_heal_above.Size = new System.Drawing.Size(46, 20);
            this.ARENA_do_not_heal_above.TabIndex = 29;
            this.ARENA_do_not_heal_above.ValueChanged += new System.EventHandler(this.ARENA_do_not_heal_above_ValueChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(58, 26);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(142, 13);
            this.label18.TabIndex = 30;
            this.label18.Text = "DO NOT MESS WITH THIS";
            // 
            // ARENA_wanna_face
            // 
            this.ARENA_wanna_face.AutoSize = true;
            this.ARENA_wanna_face.Location = new System.Drawing.Point(13, 78);
            this.ARENA_wanna_face.Name = "ARENA_wanna_face";
            this.ARENA_wanna_face.Size = new System.Drawing.Size(166, 17);
            this.ARENA_wanna_face.TabIndex = 11;
            this.ARENA_wanna_face.Text = "Face the target when needed";
            this.ARENA_wanna_face.UseVisualStyleBackColor = true;
            this.ARENA_wanna_face.CheckedChanged += new System.EventHandler(this.ARENA_wanna_face_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.ARENA_cleanse_only_self_and_tank);
            this.groupBox9.Controls.Add(this.ARENA_wanna_cleanse);
            this.groupBox9.Controls.Add(this.ARENA_wanna_urgent_cleanse);
            this.groupBox9.Location = new System.Drawing.Point(822, 314);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(146, 95);
            this.groupBox9.TabIndex = 54;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Cleanse";
            // 
            // ARENA_cleanse_only_self_and_tank
            // 
            this.ARENA_cleanse_only_self_and_tank.AutoSize = true;
            this.ARENA_cleanse_only_self_and_tank.Location = new System.Drawing.Point(17, 65);
            this.ARENA_cleanse_only_self_and_tank.Name = "ARENA_cleanse_only_self_and_tank";
            this.ARENA_cleanse_only_self_and_tank.Size = new System.Drawing.Size(134, 17);
            this.ARENA_cleanse_only_self_and_tank.TabIndex = 30;
            this.ARENA_cleanse_only_self_and_tank.Text = "But only Self and Tank";
            this.ARENA_cleanse_only_self_and_tank.UseVisualStyleBackColor = true;
            this.ARENA_cleanse_only_self_and_tank.CheckedChanged += new System.EventHandler(this.ARENA_cleanse_only_self_and_tank_CheckedChanged);
            // 
            // ARENA_wanna_cleanse
            // 
            this.ARENA_wanna_cleanse.AutoSize = true;
            this.ARENA_wanna_cleanse.Location = new System.Drawing.Point(7, 42);
            this.ARENA_wanna_cleanse.Name = "ARENA_wanna_cleanse";
            this.ARENA_wanna_cleanse.Size = new System.Drawing.Size(64, 17);
            this.ARENA_wanna_cleanse.TabIndex = 5;
            this.ARENA_wanna_cleanse.Text = "Cleanse";
            this.ARENA_wanna_cleanse.UseVisualStyleBackColor = true;
            this.ARENA_wanna_cleanse.CheckedChanged += new System.EventHandler(this.ARENA_wanna_cleanse_CheckedChanged);
            // 
            // ARENA_wanna_urgent_cleanse
            // 
            this.ARENA_wanna_urgent_cleanse.AutoSize = true;
            this.ARENA_wanna_urgent_cleanse.Location = new System.Drawing.Point(7, 19);
            this.ARENA_wanna_urgent_cleanse.Name = "ARENA_wanna_urgent_cleanse";
            this.ARENA_wanna_urgent_cleanse.Size = new System.Drawing.Size(135, 17);
            this.ARENA_wanna_urgent_cleanse.TabIndex = 27;
            this.ARENA_wanna_urgent_cleanse.Text = "Cleanse Urgents ASAP";
            this.ARENA_wanna_urgent_cleanse.UseVisualStyleBackColor = true;
            this.ARENA_wanna_urgent_cleanse.CheckedChanged += new System.EventHandler(this.ARENA_wanna_urgent_cleanse_CheckedChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.ARENA_wanna_HoJ);
            this.groupBox10.Controls.Add(this.ARENA_wanna_rebuke);
            this.groupBox10.Location = new System.Drawing.Point(822, 125);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(146, 77);
            this.groupBox10.TabIndex = 53;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Interrupts";
            // 
            // ARENA_wanna_HoJ
            // 
            this.ARENA_wanna_HoJ.AutoSize = true;
            this.ARENA_wanna_HoJ.Location = new System.Drawing.Point(9, 19);
            this.ARENA_wanna_HoJ.Name = "ARENA_wanna_HoJ";
            this.ARENA_wanna_HoJ.Size = new System.Drawing.Size(113, 17);
            this.ARENA_wanna_HoJ.TabIndex = 17;
            this.ARENA_wanna_HoJ.Text = "Hammer of Justice";
            this.ARENA_wanna_HoJ.UseVisualStyleBackColor = true;
            this.ARENA_wanna_HoJ.CheckedChanged += new System.EventHandler(this.ARENA_wanna_HoJ_CheckedChanged);
            // 
            // ARENA_wanna_rebuke
            // 
            this.ARENA_wanna_rebuke.AutoSize = true;
            this.ARENA_wanna_rebuke.Location = new System.Drawing.Point(9, 43);
            this.ARENA_wanna_rebuke.Name = "ARENA_wanna_rebuke";
            this.ARENA_wanna_rebuke.Size = new System.Drawing.Size(64, 17);
            this.ARENA_wanna_rebuke.TabIndex = 26;
            this.ARENA_wanna_rebuke.Text = "Rebuke";
            this.ARENA_wanna_rebuke.UseVisualStyleBackColor = true;
            this.ARENA_wanna_rebuke.CheckedChanged += new System.EventHandler(this.ARENA_wanna_rebuke_CheckedChanged);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.ARENA_min_Inf_of_light_DL_hpLB);
            this.groupBox11.Controls.Add(this.ARENA_min_Inf_of_light_DL_hp);
            this.groupBox11.Controls.Add(this.label20);
            this.groupBox11.Controls.Add(this.ARENA_min_HL_hp);
            this.groupBox11.Controls.Add(this.label21);
            this.groupBox11.Controls.Add(this.ARENA_min_FoL_hp);
            this.groupBox11.Controls.Add(this.label22);
            this.groupBox11.Controls.Add(this.ARENA_min_DL_hp);
            this.groupBox11.Controls.Add(this.ARENA_Inf_of_light_wanna_DL);
            this.groupBox11.Location = new System.Drawing.Point(325, 19);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(249, 160);
            this.groupBox11.TabIndex = 52;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Healing";
            // 
            // ARENA_min_Inf_of_light_DL_hpLB
            // 
            this.ARENA_min_Inf_of_light_DL_hpLB.AutoSize = true;
            this.ARENA_min_Inf_of_light_DL_hpLB.Location = new System.Drawing.Point(72, 130);
            this.ARENA_min_Inf_of_light_DL_hpLB.Name = "ARENA_min_Inf_of_light_DL_hpLB";
            this.ARENA_min_Inf_of_light_DL_hpLB.Size = new System.Drawing.Size(172, 13);
            this.ARENA_min_Inf_of_light_DL_hpLB.TabIndex = 7;
            this.ARENA_min_Inf_of_light_DL_hpLB.Text = "But only if the target is below this %";
            // 
            // ARENA_min_Inf_of_light_DL_hp
            // 
            this.ARENA_min_Inf_of_light_DL_hp.Location = new System.Drawing.Point(7, 124);
            this.ARENA_min_Inf_of_light_DL_hp.Name = "ARENA_min_Inf_of_light_DL_hp";
            this.ARENA_min_Inf_of_light_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.ARENA_min_Inf_of_light_DL_hp.TabIndex = 6;
            this.ARENA_min_Inf_of_light_DL_hp.ValueChanged += new System.EventHandler(this.ARENA_min_Inf_of_light_DL_hp_ValueChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(70, 26);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(149, 13);
            this.label20.TabIndex = 5;
            this.label20.Text = "Holy Light targets under this %";
            // 
            // ARENA_min_HL_hp
            // 
            this.ARENA_min_HL_hp.Location = new System.Drawing.Point(7, 20);
            this.ARENA_min_HL_hp.Name = "ARENA_min_HL_hp";
            this.ARENA_min_HL_hp.Size = new System.Drawing.Size(56, 20);
            this.ARENA_min_HL_hp.TabIndex = 4;
            this.ARENA_min_HL_hp.ValueChanged += new System.EventHandler(this.ARENA_min_HL_hp_ValueChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(69, 76);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(165, 13);
            this.label21.TabIndex = 3;
            this.label21.Text = "Flash of Light targets under this %";
            // 
            // ARENA_min_FoL_hp
            // 
            this.ARENA_min_FoL_hp.Location = new System.Drawing.Point(7, 74);
            this.ARENA_min_FoL_hp.Name = "ARENA_min_FoL_hp";
            this.ARENA_min_FoL_hp.Size = new System.Drawing.Size(56, 20);
            this.ARENA_min_FoL_hp.TabIndex = 2;
            this.ARENA_min_FoL_hp.ValueChanged += new System.EventHandler(this.ARENA_min_FoL_hp_ValueChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(68, 53);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(158, 13);
            this.label22.TabIndex = 1;
            this.label22.Text = "Divine Light targets under this %";
            // 
            // ARENA_min_DL_hp
            // 
            this.ARENA_min_DL_hp.Location = new System.Drawing.Point(6, 46);
            this.ARENA_min_DL_hp.Name = "ARENA_min_DL_hp";
            this.ARENA_min_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.ARENA_min_DL_hp.TabIndex = 0;
            this.ARENA_min_DL_hp.ValueChanged += new System.EventHandler(this.ARENA_min_DL_hp_ValueChanged);
            // 
            // ARENA_Inf_of_light_wanna_DL
            // 
            this.ARENA_Inf_of_light_wanna_DL.AutoSize = true;
            this.ARENA_Inf_of_light_wanna_DL.Location = new System.Drawing.Point(5, 100);
            this.ARENA_Inf_of_light_wanna_DL.Name = "ARENA_Inf_of_light_wanna_DL";
            this.ARENA_Inf_of_light_wanna_DL.Size = new System.Drawing.Size(235, 17);
            this.ARENA_Inf_of_light_wanna_DL.TabIndex = 2;
            this.ARENA_Inf_of_light_wanna_DL.Text = "Use Divine Light when Infusion of Light proc";
            this.ARENA_Inf_of_light_wanna_DL.UseVisualStyleBackColor = true;
            this.ARENA_Inf_of_light_wanna_DL.CheckedChanged += new System.EventHandler(this.ARENA_Inf_of_light_wanna_DL_CheckedChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.label23);
            this.groupBox12.Controls.Add(this.ARENA_use_mana_rec_trinket_every);
            this.groupBox12.Controls.Add(this.label24);
            this.groupBox12.Controls.Add(this.ARENA_min_mana_rec_trinket);
            this.groupBox12.Controls.Add(this.label25);
            this.groupBox12.Controls.Add(this.ARENA_min_Divine_Plea_mana);
            this.groupBox12.Controls.Add(this.label26);
            this.groupBox12.Controls.Add(this.ARENA_mana_judge);
            this.groupBox12.Location = new System.Drawing.Point(12, 166);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(307, 126);
            this.groupBox12.TabIndex = 51;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Mana Management";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(49, 103);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(186, 13);
            this.label23.TabIndex = 7;
            this.label23.Text = "Activate your mana trinket every (NYI)";
            // 
            // ARENA_use_mana_rec_trinket_every
            // 
            this.ARENA_use_mana_rec_trinket_every.Location = new System.Drawing.Point(0, 100);
            this.ARENA_use_mana_rec_trinket_every.Name = "ARENA_use_mana_rec_trinket_every";
            this.ARENA_use_mana_rec_trinket_every.Size = new System.Drawing.Size(43, 20);
            this.ARENA_use_mana_rec_trinket_every.TabIndex = 6;
            this.ARENA_use_mana_rec_trinket_every.ValueChanged += new System.EventHandler(this.ARENA_use_mana_rec_trinket_every_ValueChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(49, 78);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(208, 13);
            this.label24.TabIndex = 5;
            this.label24.Text = "Activate your Mana trinket at Mana %(NYI)";
            // 
            // ARENA_min_mana_rec_trinket
            // 
            this.ARENA_min_mana_rec_trinket.Location = new System.Drawing.Point(0, 74);
            this.ARENA_min_mana_rec_trinket.Name = "ARENA_min_mana_rec_trinket";
            this.ARENA_min_mana_rec_trinket.Size = new System.Drawing.Size(43, 20);
            this.ARENA_min_mana_rec_trinket.TabIndex = 4;
            this.ARENA_min_mana_rec_trinket.ValueChanged += new System.EventHandler(this.ARENA_min_mana_rec_trinket_ValueChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(49, 54);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(121, 13);
            this.label25.TabIndex = 3;
            this.label25.Text = "Divine Plea at this mana";
            // 
            // ARENA_min_Divine_Plea_mana
            // 
            this.ARENA_min_Divine_Plea_mana.Location = new System.Drawing.Point(0, 47);
            this.ARENA_min_Divine_Plea_mana.Name = "ARENA_min_Divine_Plea_mana";
            this.ARENA_min_Divine_Plea_mana.Size = new System.Drawing.Size(43, 20);
            this.ARENA_min_Divine_Plea_mana.TabIndex = 2;
            this.ARENA_min_Divine_Plea_mana.ValueChanged += new System.EventHandler(this.ARENA_min_Divine_Plea_mana_ValueChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(49, 27);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(242, 13);
            this.label26.TabIndex = 1;
            this.label26.Text = "Judgement on Cooldown when mana is below this";
            // 
            // ARENA_mana_judge
            // 
            this.ARENA_mana_judge.Location = new System.Drawing.Point(0, 20);
            this.ARENA_mana_judge.Name = "ARENA_mana_judge";
            this.ARENA_mana_judge.Size = new System.Drawing.Size(43, 20);
            this.ARENA_mana_judge.TabIndex = 0;
            this.ARENA_mana_judge.ValueChanged += new System.EventHandler(this.ARENA_mana_judge_ValueChanged);
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label27);
            this.groupBox13.Controls.Add(this.ARENA_min_player_inside_HR);
            this.groupBox13.Controls.Add(this.label28);
            this.groupBox13.Controls.Add(this.ARENA_HR_how_much_health);
            this.groupBox13.Controls.Add(this.label29);
            this.groupBox13.Controls.Add(this.ARENA_HR_how_far);
            this.groupBox13.Controls.Add(this.ARENA_wanna_HR);
            this.groupBox13.Location = new System.Drawing.Point(12, 298);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(307, 133);
            this.groupBox13.TabIndex = 50;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Holy Radiance Settings";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(61, 103);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(145, 13);
            this.label27.TabIndex = 27;
            this.label27.Text = "Need this many people inside";
            // 
            // ARENA_min_player_inside_HR
            // 
            this.ARENA_min_player_inside_HR.Location = new System.Drawing.Point(7, 97);
            this.ARENA_min_player_inside_HR.Name = "ARENA_min_player_inside_HR";
            this.ARENA_min_player_inside_HR.Size = new System.Drawing.Size(44, 20);
            this.ARENA_min_player_inside_HR.TabIndex = 26;
            this.ARENA_min_player_inside_HR.ValueChanged += new System.EventHandler(this.ARENA_min_player_inside_HR_ValueChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(58, 76);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(196, 13);
            this.label28.TabIndex = 25;
            this.label28.Text = "Consider unit with less than this Health%";
            // 
            // ARENA_HR_how_much_health
            // 
            this.ARENA_HR_how_much_health.Location = new System.Drawing.Point(7, 70);
            this.ARENA_HR_how_much_health.Name = "ARENA_HR_how_much_health";
            this.ARENA_HR_how_much_health.Size = new System.Drawing.Size(44, 20);
            this.ARENA_HR_how_much_health.TabIndex = 24;
            this.ARENA_HR_how_much_health.ValueChanged += new System.EventHandler(this.ARENA_HR_how_much_health_ValueChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(58, 49);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(221, 13);
            this.label29.TabIndex = 23;
            this.label29.Text = "Consider unit nearer then this to healed target";
            // 
            // ARENA_HR_how_far
            // 
            this.ARENA_HR_how_far.Location = new System.Drawing.Point(6, 43);
            this.ARENA_HR_how_far.Name = "ARENA_HR_how_far";
            this.ARENA_HR_how_far.Size = new System.Drawing.Size(45, 20);
            this.ARENA_HR_how_far.TabIndex = 22;
            this.ARENA_HR_how_far.ValueChanged += new System.EventHandler(this.ARENA_HR_how_far_ValueChanged);
            // 
            // ARENA_wanna_HR
            // 
            this.ARENA_wanna_HR.AutoSize = true;
            this.ARENA_wanna_HR.Location = new System.Drawing.Point(6, 19);
            this.ARENA_wanna_HR.Name = "ARENA_wanna_HR";
            this.ARENA_wanna_HR.Size = new System.Drawing.Size(96, 17);
            this.ARENA_wanna_HR.TabIndex = 21;
            this.ARENA_wanna_HR.Text = "Holy Radiance";
            this.ARENA_wanna_HR.UseVisualStyleBackColor = true;
            this.ARENA_wanna_HR.CheckedChanged += new System.EventHandler(this.ARENA_wanna_HR_CheckedChanged);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.ARENA_min_mana_potion);
            this.groupBox14.Controls.Add(this.ARENA_min_LoH_hp);
            this.groupBox14.Controls.Add(this.ARENA_min_HoS_hp);
            this.groupBox14.Controls.Add(this.ARENA_min_HoP_hp);
            this.groupBox14.Controls.Add(this.ARENA_min_DS_hp);
            this.groupBox14.Controls.Add(this.label30);
            this.groupBox14.Controls.Add(this.ARENA_min_DP_hp);
            this.groupBox14.Controls.Add(this.ARENA_wanna_mana_potion);
            this.groupBox14.Controls.Add(this.ARENA_wanna_LoH);
            this.groupBox14.Controls.Add(this.ARENA_wanna_DP);
            this.groupBox14.Controls.Add(this.ARENA_wanna_DS);
            this.groupBox14.Controls.Add(this.ARENA_wanna_HoS);
            this.groupBox14.Controls.Add(this.ARENA_wanna_HoP);
            this.groupBox14.Location = new System.Drawing.Point(325, 185);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(247, 182);
            this.groupBox14.TabIndex = 47;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Emergency Buttons";
            // 
            // ARENA_min_mana_potion
            // 
            this.ARENA_min_mana_potion.Location = new System.Drawing.Point(120, 154);
            this.ARENA_min_mana_potion.Name = "ARENA_min_mana_potion";
            this.ARENA_min_mana_potion.Size = new System.Drawing.Size(64, 20);
            this.ARENA_min_mana_potion.TabIndex = 30;
            this.ARENA_min_mana_potion.ValueChanged += new System.EventHandler(this.ARENA_min_mana_potion_ValueChanged);
            // 
            // ARENA_min_LoH_hp
            // 
            this.ARENA_min_LoH_hp.Location = new System.Drawing.Point(120, 130);
            this.ARENA_min_LoH_hp.Name = "ARENA_min_LoH_hp";
            this.ARENA_min_LoH_hp.Size = new System.Drawing.Size(64, 20);
            this.ARENA_min_LoH_hp.TabIndex = 29;
            this.ARENA_min_LoH_hp.ValueChanged += new System.EventHandler(this.ARENA_min_LoH_hp_ValueChanged);
            // 
            // ARENA_min_HoS_hp
            // 
            this.ARENA_min_HoS_hp.Location = new System.Drawing.Point(120, 106);
            this.ARENA_min_HoS_hp.Name = "ARENA_min_HoS_hp";
            this.ARENA_min_HoS_hp.Size = new System.Drawing.Size(64, 20);
            this.ARENA_min_HoS_hp.TabIndex = 28;
            this.ARENA_min_HoS_hp.ValueChanged += new System.EventHandler(this.ARENA_min_HoS_hp_ValueChanged);
            // 
            // ARENA_min_HoP_hp
            // 
            this.ARENA_min_HoP_hp.Location = new System.Drawing.Point(120, 84);
            this.ARENA_min_HoP_hp.Name = "ARENA_min_HoP_hp";
            this.ARENA_min_HoP_hp.Size = new System.Drawing.Size(64, 20);
            this.ARENA_min_HoP_hp.TabIndex = 27;
            this.ARENA_min_HoP_hp.ValueChanged += new System.EventHandler(this.ARENA_min_HoP_hp_ValueChanged);
            // 
            // ARENA_min_DS_hp
            // 
            this.ARENA_min_DS_hp.Location = new System.Drawing.Point(120, 58);
            this.ARENA_min_DS_hp.Name = "ARENA_min_DS_hp";
            this.ARENA_min_DS_hp.Size = new System.Drawing.Size(64, 20);
            this.ARENA_min_DS_hp.TabIndex = 26;
            this.ARENA_min_DS_hp.ValueChanged += new System.EventHandler(this.ARENA_min_DS_hp_ValueChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(120, 16);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(58, 13);
            this.label30.TabIndex = 25;
            this.label30.Text = "Use Below";
            // 
            // ARENA_min_DP_hp
            // 
            this.ARENA_min_DP_hp.Location = new System.Drawing.Point(120, 35);
            this.ARENA_min_DP_hp.Name = "ARENA_min_DP_hp";
            this.ARENA_min_DP_hp.Size = new System.Drawing.Size(64, 20);
            this.ARENA_min_DP_hp.TabIndex = 24;
            this.ARENA_min_DP_hp.ValueChanged += new System.EventHandler(this.ARENA_min_DP_hp_ValueChanged);
            // 
            // ARENA_wanna_mana_potion
            // 
            this.ARENA_wanna_mana_potion.AutoSize = true;
            this.ARENA_wanna_mana_potion.Location = new System.Drawing.Point(6, 154);
            this.ARENA_wanna_mana_potion.Name = "ARENA_wanna_mana_potion";
            this.ARENA_wanna_mana_potion.Size = new System.Drawing.Size(113, 17);
            this.ARENA_wanna_mana_potion.TabIndex = 23;
            this.ARENA_wanna_mana_potion.Text = "Mana Potion (NYI)";
            this.ARENA_wanna_mana_potion.UseVisualStyleBackColor = true;
            this.ARENA_wanna_mana_potion.CheckedChanged += new System.EventHandler(this.ARENA_wanna_mana_potion_CheckedChanged);
            // 
            // ARENA_wanna_LoH
            // 
            this.ARENA_wanna_LoH.AutoSize = true;
            this.ARENA_wanna_LoH.Location = new System.Drawing.Point(6, 130);
            this.ARENA_wanna_LoH.Name = "ARENA_wanna_LoH";
            this.ARENA_wanna_LoH.Size = new System.Drawing.Size(87, 17);
            this.ARENA_wanna_LoH.TabIndex = 23;
            this.ARENA_wanna_LoH.Text = "Lay on Hand";
            this.ARENA_wanna_LoH.UseVisualStyleBackColor = true;
            this.ARENA_wanna_LoH.CheckedChanged += new System.EventHandler(this.ARENA_wanna_LoH_CheckedChanged);
            // 
            // ARENA_wanna_DP
            // 
            this.ARENA_wanna_DP.AutoSize = true;
            this.ARENA_wanna_DP.Location = new System.Drawing.Point(6, 38);
            this.ARENA_wanna_DP.Name = "ARENA_wanna_DP";
            this.ARENA_wanna_DP.Size = new System.Drawing.Size(107, 17);
            this.ARENA_wanna_DP.TabIndex = 8;
            this.ARENA_wanna_DP.Text = "Divine Protection";
            this.ARENA_wanna_DP.UseVisualStyleBackColor = true;
            this.ARENA_wanna_DP.CheckedChanged += new System.EventHandler(this.ARENA_wanna_DP_CheckedChanged);
            // 
            // ARENA_wanna_DS
            // 
            this.ARENA_wanna_DS.AutoSize = true;
            this.ARENA_wanna_DS.Location = new System.Drawing.Point(6, 61);
            this.ARENA_wanna_DS.Name = "ARENA_wanna_DS";
            this.ARENA_wanna_DS.Size = new System.Drawing.Size(88, 17);
            this.ARENA_wanna_DS.TabIndex = 9;
            this.ARENA_wanna_DS.Text = "Divine Shield";
            this.ARENA_wanna_DS.UseVisualStyleBackColor = true;
            this.ARENA_wanna_DS.CheckedChanged += new System.EventHandler(this.ARENA_wanna_DS_CheckedChanged);
            // 
            // ARENA_wanna_HoS
            // 
            this.ARENA_wanna_HoS.AutoSize = true;
            this.ARENA_wanna_HoS.Location = new System.Drawing.Point(6, 107);
            this.ARENA_wanna_HoS.Name = "ARENA_wanna_HoS";
            this.ARENA_wanna_HoS.Size = new System.Drawing.Size(111, 17);
            this.ARENA_wanna_HoS.TabIndex = 19;
            this.ARENA_wanna_HoS.Text = "Hand of Salvation";
            this.ARENA_wanna_HoS.UseVisualStyleBackColor = true;
            this.ARENA_wanna_HoS.CheckedChanged += new System.EventHandler(this.ARENA_wanna_HoS_CheckedChanged);
            // 
            // ARENA_wanna_HoP
            // 
            this.ARENA_wanna_HoP.AutoSize = true;
            this.ARENA_wanna_HoP.Location = new System.Drawing.Point(6, 84);
            this.ARENA_wanna_HoP.Name = "ARENA_wanna_HoP";
            this.ARENA_wanna_HoP.Size = new System.Drawing.Size(115, 17);
            this.ARENA_wanna_HoP.TabIndex = 18;
            this.ARENA_wanna_HoP.Text = "Hand of Protection";
            this.ARENA_wanna_HoP.UseVisualStyleBackColor = true;
            this.ARENA_wanna_HoP.CheckedChanged += new System.EventHandler(this.ARENA_wanna_HoP_CheckedChanged);
            // 
            // ARENA_auraselctGB
            // 
            this.ARENA_auraselctGB.Controls.Add(this.ARENA_DisabledRB);
            this.ARENA_auraselctGB.Controls.Add(this.ARENA_resistanceRB);
            this.ARENA_auraselctGB.Controls.Add(this.ARENA_concentrationRB);
            this.ARENA_auraselctGB.Location = new System.Drawing.Point(822, 19);
            this.ARENA_auraselctGB.Name = "ARENA_auraselctGB";
            this.ARENA_auraselctGB.Size = new System.Drawing.Size(146, 100);
            this.ARENA_auraselctGB.TabIndex = 49;
            this.ARENA_auraselctGB.TabStop = false;
            this.ARENA_auraselctGB.Text = "Select Aura";
            this.ARENA_auraselctGB.Enter += new System.EventHandler(this.ARENA_auraselctGB_Enter);
            // 
            // ARENA_DisabledRB
            // 
            this.ARENA_DisabledRB.AutoSize = true;
            this.ARENA_DisabledRB.Location = new System.Drawing.Point(7, 68);
            this.ARENA_DisabledRB.Name = "ARENA_DisabledRB";
            this.ARENA_DisabledRB.Size = new System.Drawing.Size(66, 17);
            this.ARENA_DisabledRB.TabIndex = 2;
            this.ARENA_DisabledRB.TabStop = true;
            this.ARENA_DisabledRB.Text = "Disabled";
            this.ARENA_DisabledRB.UseVisualStyleBackColor = true;
            this.ARENA_DisabledRB.CheckedChanged += new System.EventHandler(this.ARENA_DisabledRB_CheckedChanged);
            // 
            // ARENA_resistanceRB
            // 
            this.ARENA_resistanceRB.AutoSize = true;
            this.ARENA_resistanceRB.Location = new System.Drawing.Point(7, 44);
            this.ARENA_resistanceRB.Name = "ARENA_resistanceRB";
            this.ARENA_resistanceRB.Size = new System.Drawing.Size(78, 17);
            this.ARENA_resistanceRB.TabIndex = 1;
            this.ARENA_resistanceRB.TabStop = true;
            this.ARENA_resistanceRB.Text = "Resistance";
            this.ARENA_resistanceRB.UseVisualStyleBackColor = true;
            this.ARENA_resistanceRB.CheckedChanged += new System.EventHandler(this.ARENA_resistanceRB_CheckedChanged);
            // 
            // ARENA_concentrationRB
            // 
            this.ARENA_concentrationRB.AutoSize = true;
            this.ARENA_concentrationRB.Location = new System.Drawing.Point(7, 20);
            this.ARENA_concentrationRB.Name = "ARENA_concentrationRB";
            this.ARENA_concentrationRB.Size = new System.Drawing.Size(91, 17);
            this.ARENA_concentrationRB.TabIndex = 0;
            this.ARENA_concentrationRB.TabStop = true;
            this.ARENA_concentrationRB.Text = "Concentration";
            this.ARENA_concentrationRB.UseVisualStyleBackColor = true;
            this.ARENA_concentrationRB.CheckedChanged += new System.EventHandler(this.ARENA_concentrationRB_CheckedChanged);
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.ARENA_min_torrent_mana_perc);
            this.groupBox16.Controls.Add(this.ARENA_min_stoneform);
            this.groupBox16.Controls.Add(this.ARENA_min_gift_hp);
            this.groupBox16.Controls.Add(this.ARENA_wanna_torrent);
            this.groupBox16.Controls.Add(this.ARENA_wanna_stoneform);
            this.groupBox16.Controls.Add(this.ARENA_wanna_everymanforhimself);
            this.groupBox16.Controls.Add(this.ARENA_wanna_gift);
            this.groupBox16.Location = new System.Drawing.Point(578, 170);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(236, 124);
            this.groupBox16.TabIndex = 45;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Racials";
            // 
            // ARENA_min_torrent_mana_perc
            // 
            this.ARENA_min_torrent_mana_perc.Location = new System.Drawing.Point(172, 95);
            this.ARENA_min_torrent_mana_perc.Name = "ARENA_min_torrent_mana_perc";
            this.ARENA_min_torrent_mana_perc.Size = new System.Drawing.Size(59, 20);
            this.ARENA_min_torrent_mana_perc.TabIndex = 17;
            this.ARENA_min_torrent_mana_perc.ValueChanged += new System.EventHandler(this.ARENA_min_torrent_mana_perc_ValueChanged);
            // 
            // ARENA_min_stoneform
            // 
            this.ARENA_min_stoneform.Location = new System.Drawing.Point(172, 71);
            this.ARENA_min_stoneform.Name = "ARENA_min_stoneform";
            this.ARENA_min_stoneform.Size = new System.Drawing.Size(59, 20);
            this.ARENA_min_stoneform.TabIndex = 16;
            this.ARENA_min_stoneform.ValueChanged += new System.EventHandler(this.ARENA_min_stoneform_ValueChanged);
            // 
            // ARENA_min_gift_hp
            // 
            this.ARENA_min_gift_hp.Location = new System.Drawing.Point(172, 45);
            this.ARENA_min_gift_hp.Name = "ARENA_min_gift_hp";
            this.ARENA_min_gift_hp.Size = new System.Drawing.Size(59, 20);
            this.ARENA_min_gift_hp.TabIndex = 15;
            this.ARENA_min_gift_hp.ValueChanged += new System.EventHandler(this.ARENA_min_gift_hp_ValueChanged);
            // 
            // ARENA_wanna_torrent
            // 
            this.ARENA_wanna_torrent.AutoSize = true;
            this.ARENA_wanna_torrent.Location = new System.Drawing.Point(16, 95);
            this.ARENA_wanna_torrent.Name = "ARENA_wanna_torrent";
            this.ARENA_wanna_torrent.Size = new System.Drawing.Size(156, 17);
            this.ARENA_wanna_torrent.TabIndex = 14;
            this.ARENA_wanna_torrent.Text = "Arcane Torrent (to interrupt)";
            this.ARENA_wanna_torrent.UseVisualStyleBackColor = true;
            this.ARENA_wanna_torrent.CheckedChanged += new System.EventHandler(this.ARENA_wanna_torrent_CheckedChanged);
            // 
            // ARENA_wanna_stoneform
            // 
            this.ARENA_wanna_stoneform.AutoSize = true;
            this.ARENA_wanna_stoneform.Location = new System.Drawing.Point(16, 71);
            this.ARENA_wanna_stoneform.Name = "ARENA_wanna_stoneform";
            this.ARENA_wanna_stoneform.Size = new System.Drawing.Size(74, 17);
            this.ARENA_wanna_stoneform.TabIndex = 13;
            this.ARENA_wanna_stoneform.Text = "Stoneform";
            this.ARENA_wanna_stoneform.UseVisualStyleBackColor = true;
            this.ARENA_wanna_stoneform.CheckedChanged += new System.EventHandler(this.ARENA_wanna_stoneform_CheckedChanged);
            // 
            // ARENA_wanna_everymanforhimself
            // 
            this.ARENA_wanna_everymanforhimself.AutoSize = true;
            this.ARENA_wanna_everymanforhimself.Location = new System.Drawing.Point(16, 24);
            this.ARENA_wanna_everymanforhimself.Name = "ARENA_wanna_everymanforhimself";
            this.ARENA_wanna_everymanforhimself.Size = new System.Drawing.Size(129, 17);
            this.ARENA_wanna_everymanforhimself.TabIndex = 10;
            this.ARENA_wanna_everymanforhimself.Text = "Every Man for Himself";
            this.ARENA_wanna_everymanforhimself.UseVisualStyleBackColor = true;
            this.ARENA_wanna_everymanforhimself.CheckedChanged += new System.EventHandler(this.ARENA_wanna_everymanforhimself_CheckedChanged);
            // 
            // ARENA_wanna_gift
            // 
            this.ARENA_wanna_gift.AutoSize = true;
            this.ARENA_wanna_gift.Location = new System.Drawing.Point(16, 47);
            this.ARENA_wanna_gift.Name = "ARENA_wanna_gift";
            this.ARENA_wanna_gift.Size = new System.Drawing.Size(104, 17);
            this.ARENA_wanna_gift.TabIndex = 12;
            this.ARENA_wanna_gift.Text = "Gift of the Naaru";
            this.ARENA_wanna_gift.UseVisualStyleBackColor = true;
            this.ARENA_wanna_gift.CheckedChanged += new System.EventHandler(this.ARENA_wanna_gift_CheckedChanged);
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.ARENA_do_not_dismount_EVER);
            this.groupBox17.Controls.Add(this.ARENA_do_not_dismount_ooc);
            this.groupBox17.Controls.Add(this.ARENA_wanna_move_to_HoJ);
            this.groupBox17.Controls.Add(this.ARENA_wanna_mount);
            this.groupBox17.Controls.Add(this.ARENA_wanna_move_to_heal);
            this.groupBox17.Controls.Add(this.ARENA_wanna_crusader);
            this.groupBox17.Location = new System.Drawing.Point(325, 373);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(249, 156);
            this.groupBox17.TabIndex = 48;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Movement";
            // 
            // ARENA_do_not_dismount_EVER
            // 
            this.ARENA_do_not_dismount_EVER.AutoSize = true;
            this.ARENA_do_not_dismount_EVER.Location = new System.Drawing.Point(6, 137);
            this.ARENA_do_not_dismount_EVER.Name = "ARENA_do_not_dismount_EVER";
            this.ARENA_do_not_dismount_EVER.Size = new System.Drawing.Size(130, 17);
            this.ARENA_do_not_dismount_EVER.TabIndex = 27;
            this.ARENA_do_not_dismount_EVER.Text = "Don\'t Dismount EVER";
            this.ARENA_do_not_dismount_EVER.UseVisualStyleBackColor = true;
            this.ARENA_do_not_dismount_EVER.CheckedChanged += new System.EventHandler(this.ARENA_do_not_dismount_EVER_CheckedChanged);
            // 
            // ARENA_do_not_dismount_ooc
            // 
            this.ARENA_do_not_dismount_ooc.AutoSize = true;
            this.ARENA_do_not_dismount_ooc.Location = new System.Drawing.Point(6, 113);
            this.ARENA_do_not_dismount_ooc.Name = "ARENA_do_not_dismount_ooc";
            this.ARENA_do_not_dismount_ooc.Size = new System.Drawing.Size(169, 17);
            this.ARENA_do_not_dismount_ooc.TabIndex = 26;
            this.ARENA_do_not_dismount_ooc.Text = "Don\'t Dismount Out of Combat";
            this.ARENA_do_not_dismount_ooc.UseVisualStyleBackColor = true;
            this.ARENA_do_not_dismount_ooc.CheckedChanged += new System.EventHandler(this.ARENA_do_not_dismount_ooc_CheckedChanged);
            // 
            // ARENA_wanna_move_to_HoJ
            // 
            this.ARENA_wanna_move_to_HoJ.AutoSize = true;
            this.ARENA_wanna_move_to_HoJ.Location = new System.Drawing.Point(7, 66);
            this.ARENA_wanna_move_to_HoJ.Name = "ARENA_wanna_move_to_HoJ";
            this.ARENA_wanna_move_to_HoJ.Size = new System.Drawing.Size(201, 17);
            this.ARENA_wanna_move_to_HoJ.TabIndex = 25;
            this.ARENA_wanna_move_to_HoJ.Text = "Move in Range to Hammer of Justice";
            this.ARENA_wanna_move_to_HoJ.UseVisualStyleBackColor = true;
            this.ARENA_wanna_move_to_HoJ.CheckedChanged += new System.EventHandler(this.ARENA_wanna_move_to_HoJ_CheckedChanged);
            // 
            // ARENA_wanna_mount
            // 
            this.ARENA_wanna_mount.AutoSize = true;
            this.ARENA_wanna_mount.Location = new System.Drawing.Point(6, 19);
            this.ARENA_wanna_mount.Name = "ARENA_wanna_mount";
            this.ARENA_wanna_mount.Size = new System.Drawing.Size(73, 17);
            this.ARENA_wanna_mount.TabIndex = 23;
            this.ARENA_wanna_mount.Text = "Mount Up";
            this.ARENA_wanna_mount.UseVisualStyleBackColor = true;
            this.ARENA_wanna_mount.CheckedChanged += new System.EventHandler(this.ARENA_wanna_mount_CheckedChanged);
            // 
            // ARENA_wanna_move_to_heal
            // 
            this.ARENA_wanna_move_to_heal.AutoSize = true;
            this.ARENA_wanna_move_to_heal.Location = new System.Drawing.Point(6, 42);
            this.ARENA_wanna_move_to_heal.Name = "ARENA_wanna_move_to_heal";
            this.ARENA_wanna_move_to_heal.Size = new System.Drawing.Size(136, 17);
            this.ARENA_wanna_move_to_heal.TabIndex = 24;
            this.ARENA_wanna_move_to_heal.Text = "Move in Range to Heal";
            this.ARENA_wanna_move_to_heal.UseVisualStyleBackColor = true;
            this.ARENA_wanna_move_to_heal.CheckedChanged += new System.EventHandler(this.ARENA_wanna_move_to_heal_CheckedChanged);
            // 
            // ARENA_wanna_crusader
            // 
            this.ARENA_wanna_crusader.AutoSize = true;
            this.ARENA_wanna_crusader.Location = new System.Drawing.Point(6, 89);
            this.ARENA_wanna_crusader.Name = "ARENA_wanna_crusader";
            this.ARENA_wanna_crusader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ARENA_wanna_crusader.Size = new System.Drawing.Size(213, 17);
            this.ARENA_wanna_crusader.TabIndex = 0;
            this.ARENA_wanna_crusader.Text = "Switch to Crusader Aura when mounted";
            this.ARENA_wanna_crusader.UseVisualStyleBackColor = true;
            this.ARENA_wanna_crusader.CheckedChanged += new System.EventHandler(this.ARENA_wanna_crusader_CheckedChanged);
            // 
            // Arena_generalGB
            // 
            this.Arena_generalGB.Controls.Add(this.label31);
            this.Arena_generalGB.Controls.Add(this.ARENA_rest_if_mana_below);
            this.Arena_generalGB.Controls.Add(this.ARENA_wanna_buff);
            this.Arena_generalGB.Location = new System.Drawing.Point(12, 19);
            this.Arena_generalGB.Name = "Arena_generalGB";
            this.Arena_generalGB.Size = new System.Drawing.Size(307, 66);
            this.Arena_generalGB.TabIndex = 46;
            this.Arena_generalGB.TabStop = false;
            this.Arena_generalGB.Text = "General";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(64, 44);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(101, 13);
            this.label31.TabIndex = 6;
            this.label31.Text = "Rest at this Mana %";
            // 
            // ARENA_rest_if_mana_below
            // 
            this.ARENA_rest_if_mana_below.Location = new System.Drawing.Point(5, 41);
            this.ARENA_rest_if_mana_below.Name = "ARENA_rest_if_mana_below";
            this.ARENA_rest_if_mana_below.Size = new System.Drawing.Size(53, 20);
            this.ARENA_rest_if_mana_below.TabIndex = 5;
            this.ARENA_rest_if_mana_below.ValueChanged += new System.EventHandler(this.ARENA_rest_if_mana_below_ValueChanged);
            // 
            // ARENA_wanna_buff
            // 
            this.ARENA_wanna_buff.AutoSize = true;
            this.ARENA_wanna_buff.Location = new System.Drawing.Point(5, 19);
            this.ARENA_wanna_buff.Name = "ARENA_wanna_buff";
            this.ARENA_wanna_buff.Size = new System.Drawing.Size(86, 17);
            this.ARENA_wanna_buff.TabIndex = 4;
            this.ARENA_wanna_buff.Text = "Enable Buffs";
            this.ARENA_wanna_buff.UseVisualStyleBackColor = true;
            this.ARENA_wanna_buff.CheckedChanged += new System.EventHandler(this.ARENA_wanna_buff_CheckedChanged);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.ARENA_wanna_Judge);
            this.groupBox19.Controls.Add(this.ARENA_wanna_CS);
            this.groupBox19.Controls.Add(this.ARENA_wanna_HoW);
            this.groupBox19.Location = new System.Drawing.Point(822, 208);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(146, 100);
            this.groupBox19.TabIndex = 44;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "DPS";
            // 
            // ARENA_wanna_Judge
            // 
            this.ARENA_wanna_Judge.AutoSize = true;
            this.ARENA_wanna_Judge.Location = new System.Drawing.Point(17, 70);
            this.ARENA_wanna_Judge.Name = "ARENA_wanna_Judge";
            this.ARENA_wanna_Judge.Size = new System.Drawing.Size(78, 17);
            this.ARENA_wanna_Judge.TabIndex = 23;
            this.ARENA_wanna_Judge.Text = "Judgement";
            this.ARENA_wanna_Judge.UseVisualStyleBackColor = true;
            this.ARENA_wanna_Judge.CheckedChanged += new System.EventHandler(this.ARENA_wanna_Judge_CheckedChanged);
            // 
            // ARENA_wanna_CS
            // 
            this.ARENA_wanna_CS.AutoSize = true;
            this.ARENA_wanna_CS.Location = new System.Drawing.Point(17, 24);
            this.ARENA_wanna_CS.Name = "ARENA_wanna_CS";
            this.ARENA_wanna_CS.Size = new System.Drawing.Size(98, 17);
            this.ARENA_wanna_CS.TabIndex = 6;
            this.ARENA_wanna_CS.Text = "Crusader Strike";
            this.ARENA_wanna_CS.UseVisualStyleBackColor = true;
            this.ARENA_wanna_CS.CheckedChanged += new System.EventHandler(this.ARENA_wanna_CS_CheckedChanged);
            // 
            // ARENA_wanna_HoW
            // 
            this.ARENA_wanna_HoW.AutoSize = true;
            this.ARENA_wanna_HoW.Location = new System.Drawing.Point(17, 47);
            this.ARENA_wanna_HoW.Name = "ARENA_wanna_HoW";
            this.ARENA_wanna_HoW.Size = new System.Drawing.Size(109, 17);
            this.ARENA_wanna_HoW.TabIndex = 20;
            this.ARENA_wanna_HoW.Text = "Hammer of Wrath";
            this.ARENA_wanna_HoW.UseVisualStyleBackColor = true;
            this.ARENA_wanna_HoW.CheckedChanged += new System.EventHandler(this.ARENA_wanna_HoW_CheckedChanged);
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.ARENA_wanna_lifeblood);
            this.groupBox20.Controls.Add(this.label32);
            this.groupBox20.Controls.Add(this.ARENA_min_ohshitbutton_activator);
            this.groupBox20.Controls.Add(this.ARENA_wanna_GotAK);
            this.groupBox20.Controls.Add(this.ARENA_wanna_AW);
            this.groupBox20.Controls.Add(this.ARENA_wanna_DF);
            this.groupBox20.Location = new System.Drawing.Point(580, 19);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(236, 145);
            this.groupBox20.TabIndex = 43;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Oh Shit! Buttons";
            // 
            // ARENA_wanna_lifeblood
            // 
            this.ARENA_wanna_lifeblood.AutoSize = true;
            this.ARENA_wanna_lifeblood.Location = new System.Drawing.Point(6, 47);
            this.ARENA_wanna_lifeblood.Name = "ARENA_wanna_lifeblood";
            this.ARENA_wanna_lifeblood.Size = new System.Drawing.Size(69, 17);
            this.ARENA_wanna_lifeblood.TabIndex = 11;
            this.ARENA_wanna_lifeblood.Text = "Lifeblood";
            this.ARENA_wanna_lifeblood.UseVisualStyleBackColor = true;
            this.ARENA_wanna_lifeblood.CheckedChanged += new System.EventHandler(this.ARENA_wanna_lifeblood_CheckedChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(63, 24);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(135, 13);
            this.label32.TabIndex = 10;
            this.label32.Text = "Press if someone is this low";
            // 
            // ARENA_min_ohshitbutton_activator
            // 
            this.ARENA_min_ohshitbutton_activator.Location = new System.Drawing.Point(6, 20);
            this.ARENA_min_ohshitbutton_activator.Name = "ARENA_min_ohshitbutton_activator";
            this.ARENA_min_ohshitbutton_activator.Size = new System.Drawing.Size(50, 20);
            this.ARENA_min_ohshitbutton_activator.TabIndex = 9;
            this.ARENA_min_ohshitbutton_activator.ValueChanged += new System.EventHandler(this.ARENA_min_ohshitbutton_activator_ValueChanged);
            // 
            // ARENA_wanna_GotAK
            // 
            this.ARENA_wanna_GotAK.AutoSize = true;
            this.ARENA_wanna_GotAK.Location = new System.Drawing.Point(6, 122);
            this.ARENA_wanna_GotAK.Name = "ARENA_wanna_GotAK";
            this.ARENA_wanna_GotAK.Size = new System.Drawing.Size(167, 17);
            this.ARENA_wanna_GotAK.TabIndex = 8;
            this.ARENA_wanna_GotAK.Text = "Guardian of the Ancient Kings";
            this.ARENA_wanna_GotAK.UseVisualStyleBackColor = true;
            this.ARENA_wanna_GotAK.CheckedChanged += new System.EventHandler(this.ARENA_wanna_GotAK_CheckedChanged);
            // 
            // ARENA_wanna_AW
            // 
            this.ARENA_wanna_AW.AutoSize = true;
            this.ARENA_wanna_AW.Location = new System.Drawing.Point(6, 70);
            this.ARENA_wanna_AW.Name = "ARENA_wanna_AW";
            this.ARENA_wanna_AW.Size = new System.Drawing.Size(103, 17);
            this.ARENA_wanna_AW.TabIndex = 3;
            this.ARENA_wanna_AW.Text = "Avenging Wrath";
            this.ARENA_wanna_AW.UseVisualStyleBackColor = true;
            this.ARENA_wanna_AW.CheckedChanged += new System.EventHandler(this.ARENA_wanna_AW_CheckedChanged);
            // 
            // ARENA_wanna_DF
            // 
            this.ARENA_wanna_DF.AutoSize = true;
            this.ARENA_wanna_DF.Location = new System.Drawing.Point(6, 95);
            this.ARENA_wanna_DF.Name = "ARENA_wanna_DF";
            this.ARENA_wanna_DF.Size = new System.Drawing.Size(86, 17);
            this.ARENA_wanna_DF.TabIndex = 7;
            this.ARENA_wanna_DF.Text = "Divine Favor";
            this.ARENA_wanna_DF.UseVisualStyleBackColor = true;
            this.ARENA_wanna_DF.CheckedChanged += new System.EventHandler(this.ARENA_wanna_DF_CheckedChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.AutoScroll = true;
            this.tabPage7.Controls.Add(this.ARENA2v2tankfromfocus);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(994, 653);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Arena 2v2";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // ARENA2v2tankfromfocus
            // 
            this.ARENA2v2tankfromfocus.AutoSize = true;
            this.ARENA2v2tankfromfocus.Location = new System.Drawing.Point(479, 21);
            this.ARENA2v2tankfromfocus.Name = "ARENA2v2tankfromfocus";
            this.ARENA2v2tankfromfocus.Size = new System.Drawing.Size(179, 17);
            this.ARENA2v2tankfromfocus.TabIndex = 2;
            this.ARENA2v2tankfromfocus.Text = "Import the Tank from your Focus";
            this.ARENA2v2tankfromfocus.UseVisualStyleBackColor = true;
            this.ARENA2v2tankfromfocus.CheckedChanged += new System.EventHandler(this.ARENA2v2tankfromfocus_CheckedChanged);
            // 
            // tabPage8
            // 
            this.tabPage8.AutoScroll = true;
            this.tabPage8.Controls.Add(this.WorldPVP_optimizeGB);
            this.tabPage8.Controls.Add(this.WorldPVP_bless_selection);
            this.tabPage8.Controls.Add(this.groupBox43);
            this.tabPage8.Controls.Add(this.groupBox46);
            this.tabPage8.Controls.Add(this.panel5);
            this.tabPage8.Controls.Add(this.groupBox51);
            this.tabPage8.Controls.Add(this.groupBox52);
            this.tabPage8.Controls.Add(this.groupBox53);
            this.tabPage8.Controls.Add(this.groupBox54);
            this.tabPage8.Controls.Add(this.groupBox55);
            this.tabPage8.Controls.Add(this.groupBox56);
            this.tabPage8.Controls.Add(this.WorldPVP_auraselctGB);
            this.tabPage8.Controls.Add(this.groupBox58);
            this.tabPage8.Controls.Add(this.groupBox59);
            this.tabPage8.Controls.Add(this.WorldPVP_generalGB);
            this.tabPage8.Controls.Add(this.groupBox61);
            this.tabPage8.Controls.Add(this.groupBox62);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(994, 653);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "WorldPVP";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // WorldPVP_optimizeGB
            // 
            this.WorldPVP_optimizeGB.Controls.Add(this.WorldPVP_intellywait);
            this.WorldPVP_optimizeGB.Controls.Add(this.WorldPVP_accurancy);
            this.WorldPVP_optimizeGB.Controls.Add(this.WorldPVP_speed);
            this.WorldPVP_optimizeGB.Location = new System.Drawing.Point(580, 406);
            this.WorldPVP_optimizeGB.Name = "WorldPVP_optimizeGB";
            this.WorldPVP_optimizeGB.Size = new System.Drawing.Size(200, 100);
            this.WorldPVP_optimizeGB.TabIndex = 89;
            this.WorldPVP_optimizeGB.TabStop = false;
            this.WorldPVP_optimizeGB.Text = "Optimize the CC for";
            this.WorldPVP_optimizeGB.Enter += new System.EventHandler(this.WorldPVP_optimizeGB_Enter);
            // 
            // WorldPVP_intellywait
            // 
            this.WorldPVP_intellywait.AutoSize = true;
            this.WorldPVP_intellywait.Location = new System.Drawing.Point(7, 68);
            this.WorldPVP_intellywait.Name = "WorldPVP_intellywait";
            this.WorldPVP_intellywait.Size = new System.Drawing.Size(159, 17);
            this.WorldPVP_intellywait.TabIndex = 2;
            this.WorldPVP_intellywait.TabStop = true;
            this.WorldPVP_intellywait.Tag = "optimize";
            this.WorldPVP_intellywait.Text = "IntellyWait (combat sistem 6)";
            this.WorldPVP_intellywait.UseVisualStyleBackColor = true;
            this.WorldPVP_intellywait.CheckedChanged += new System.EventHandler(this.WorldPVP_intellywait_CheckedChanged);
            // 
            // WorldPVP_accurancy
            // 
            this.WorldPVP_accurancy.AutoSize = true;
            this.WorldPVP_accurancy.Location = new System.Drawing.Point(7, 44);
            this.WorldPVP_accurancy.Name = "WorldPVP_accurancy";
            this.WorldPVP_accurancy.Size = new System.Drawing.Size(161, 17);
            this.WorldPVP_accurancy.TabIndex = 1;
            this.WorldPVP_accurancy.TabStop = true;
            this.WorldPVP_accurancy.Tag = "optimize";
            this.WorldPVP_accurancy.Text = "Accurancy (combat sistem 5)";
            this.WorldPVP_accurancy.UseVisualStyleBackColor = true;
            this.WorldPVP_accurancy.CheckedChanged += new System.EventHandler(this.WorldPVP_accurancy_CheckedChanged);
            // 
            // WorldPVP_speed
            // 
            this.WorldPVP_speed.AutoSize = true;
            this.WorldPVP_speed.Location = new System.Drawing.Point(8, 20);
            this.WorldPVP_speed.Name = "WorldPVP_speed";
            this.WorldPVP_speed.Size = new System.Drawing.Size(141, 17);
            this.WorldPVP_speed.TabIndex = 0;
            this.WorldPVP_speed.TabStop = true;
            this.WorldPVP_speed.Tag = "optimize";
            this.WorldPVP_speed.Text = "Speed (combat sistem 4)";
            this.WorldPVP_speed.UseVisualStyleBackColor = true;
            this.WorldPVP_speed.CheckedChanged += new System.EventHandler(this.WorldPVP_speed_CheckedChanged);
            // 
            // WorldPVP_bless_selection
            // 
            this.WorldPVP_bless_selection.Controls.Add(this.WorldPVP_bless_type_disabledRB);
            this.WorldPVP_bless_selection.Controls.Add(this.WorldPVP_bless_type_MightRB);
            this.WorldPVP_bless_selection.Controls.Add(this.WorldPVP_bless_type_KingRB);
            this.WorldPVP_bless_selection.Controls.Add(this.WorldPVP_bless_type_autoRB);
            this.WorldPVP_bless_selection.Location = new System.Drawing.Point(822, 426);
            this.WorldPVP_bless_selection.Name = "WorldPVP_bless_selection";
            this.WorldPVP_bless_selection.Size = new System.Drawing.Size(146, 118);
            this.WorldPVP_bless_selection.TabIndex = 88;
            this.WorldPVP_bless_selection.TabStop = false;
            this.WorldPVP_bless_selection.Text = "Bless Selection";
            this.WorldPVP_bless_selection.Enter += new System.EventHandler(this.WorldPVP_bless_selection_Enter);
            // 
            // WorldPVP_bless_type_disabledRB
            // 
            this.WorldPVP_bless_type_disabledRB.AutoSize = true;
            this.WorldPVP_bless_type_disabledRB.Location = new System.Drawing.Point(7, 92);
            this.WorldPVP_bless_type_disabledRB.Name = "WorldPVP_bless_type_disabledRB";
            this.WorldPVP_bless_type_disabledRB.Size = new System.Drawing.Size(66, 17);
            this.WorldPVP_bless_type_disabledRB.TabIndex = 3;
            this.WorldPVP_bless_type_disabledRB.TabStop = true;
            this.WorldPVP_bless_type_disabledRB.Tag = "Bless";
            this.WorldPVP_bless_type_disabledRB.Text = "Disabled";
            this.WorldPVP_bless_type_disabledRB.UseVisualStyleBackColor = true;
            this.WorldPVP_bless_type_disabledRB.CheckedChanged += new System.EventHandler(this.WorldPVP_bless_type_disabledRB_CheckedChanged);
            // 
            // WorldPVP_bless_type_MightRB
            // 
            this.WorldPVP_bless_type_MightRB.AutoSize = true;
            this.WorldPVP_bless_type_MightRB.Location = new System.Drawing.Point(7, 68);
            this.WorldPVP_bless_type_MightRB.Name = "WorldPVP_bless_type_MightRB";
            this.WorldPVP_bless_type_MightRB.Size = new System.Drawing.Size(51, 17);
            this.WorldPVP_bless_type_MightRB.TabIndex = 2;
            this.WorldPVP_bless_type_MightRB.TabStop = true;
            this.WorldPVP_bless_type_MightRB.Tag = "Bless";
            this.WorldPVP_bless_type_MightRB.Text = "Might";
            this.WorldPVP_bless_type_MightRB.UseVisualStyleBackColor = true;
            this.WorldPVP_bless_type_MightRB.CheckedChanged += new System.EventHandler(this.WorldPVP_bless_type_MightRB_CheckedChanged);
            // 
            // WorldPVP_bless_type_KingRB
            // 
            this.WorldPVP_bless_type_KingRB.AutoSize = true;
            this.WorldPVP_bless_type_KingRB.Location = new System.Drawing.Point(7, 44);
            this.WorldPVP_bless_type_KingRB.Name = "WorldPVP_bless_type_KingRB";
            this.WorldPVP_bless_type_KingRB.Size = new System.Drawing.Size(46, 17);
            this.WorldPVP_bless_type_KingRB.TabIndex = 1;
            this.WorldPVP_bless_type_KingRB.TabStop = true;
            this.WorldPVP_bless_type_KingRB.Tag = "Bless";
            this.WorldPVP_bless_type_KingRB.Text = "King";
            this.WorldPVP_bless_type_KingRB.UseVisualStyleBackColor = true;
            this.WorldPVP_bless_type_KingRB.CheckedChanged += new System.EventHandler(this.WorldPVP_bless_type_KingRB_CheckedChanged);
            // 
            // WorldPVP_bless_type_autoRB
            // 
            this.WorldPVP_bless_type_autoRB.AutoSize = true;
            this.WorldPVP_bless_type_autoRB.Location = new System.Drawing.Point(7, 20);
            this.WorldPVP_bless_type_autoRB.Name = "WorldPVP_bless_type_autoRB";
            this.WorldPVP_bless_type_autoRB.Size = new System.Drawing.Size(47, 17);
            this.WorldPVP_bless_type_autoRB.TabIndex = 0;
            this.WorldPVP_bless_type_autoRB.TabStop = true;
            this.WorldPVP_bless_type_autoRB.Tag = "Bless";
            this.WorldPVP_bless_type_autoRB.Text = "Auto";
            this.WorldPVP_bless_type_autoRB.UseVisualStyleBackColor = true;
            this.WorldPVP_bless_type_autoRB.CheckedChanged += new System.EventHandler(this.WorldPVP_bless_type_autoRB_CheckedChanged);
            // 
            // groupBox43
            // 
            this.groupBox43.Location = new System.Drawing.Point(578, 300);
            this.groupBox43.Name = "groupBox43";
            this.groupBox43.Size = new System.Drawing.Size(236, 100);
            this.groupBox43.TabIndex = 87;
            this.groupBox43.TabStop = false;
            this.groupBox43.Text = "Overhealing Protection";
            // 
            // groupBox46
            // 
            this.groupBox46.Controls.Add(this.WorldPVP_get_tank_from_lua);
            this.groupBox46.Controls.Add(this.WorldPVP_get_tank_from_focus);
            this.groupBox46.Location = new System.Drawing.Point(12, 91);
            this.groupBox46.Name = "groupBox46";
            this.groupBox46.Size = new System.Drawing.Size(307, 69);
            this.groupBox46.TabIndex = 86;
            this.groupBox46.TabStop = false;
            this.groupBox46.Text = "Tank Selection";
            // 
            // WorldPVP_get_tank_from_lua
            // 
            this.WorldPVP_get_tank_from_lua.AutoSize = true;
            this.WorldPVP_get_tank_from_lua.Location = new System.Drawing.Point(8, 42);
            this.WorldPVP_get_tank_from_lua.Name = "WorldPVP_get_tank_from_lua";
            this.WorldPVP_get_tank_from_lua.Size = new System.Drawing.Size(156, 17);
            this.WorldPVP_get_tank_from_lua.TabIndex = 2;
            this.WorldPVP_get_tank_from_lua.Text = "Use the ingame RoleCheck";
            this.WorldPVP_get_tank_from_lua.UseVisualStyleBackColor = true;
            this.WorldPVP_get_tank_from_lua.CheckedChanged += new System.EventHandler(this.WorldPVP_get_tank_from_lua_CheckedChanged);
            // 
            // WorldPVP_get_tank_from_focus
            // 
            this.WorldPVP_get_tank_from_focus.AutoSize = true;
            this.WorldPVP_get_tank_from_focus.Location = new System.Drawing.Point(8, 19);
            this.WorldPVP_get_tank_from_focus.Name = "WorldPVP_get_tank_from_focus";
            this.WorldPVP_get_tank_from_focus.Size = new System.Drawing.Size(179, 17);
            this.WorldPVP_get_tank_from_focus.TabIndex = 1;
            this.WorldPVP_get_tank_from_focus.Text = "Import the Tank from your Focus";
            this.WorldPVP_get_tank_from_focus.UseVisualStyleBackColor = true;
            this.WorldPVP_get_tank_from_focus.CheckedChanged += new System.EventHandler(this.WorldPVP_get_tank_from_focus_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.WorldPVP_advanced_option);
            this.panel5.Controls.Add(this.WorldPVP_advanced);
            this.panel5.Location = new System.Drawing.Point(12, 437);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(313, 202);
            this.panel5.TabIndex = 85;
            // 
            // WorldPVP_advanced_option
            // 
            this.WorldPVP_advanced_option.AutoSize = true;
            this.WorldPVP_advanced_option.Location = new System.Drawing.Point(6, 3);
            this.WorldPVP_advanced_option.Name = "WorldPVP_advanced_option";
            this.WorldPVP_advanced_option.Size = new System.Drawing.Size(144, 17);
            this.WorldPVP_advanced_option.TabIndex = 34;
            this.WorldPVP_advanced_option.Text = "Show Advanced Options";
            this.WorldPVP_advanced_option.UseVisualStyleBackColor = true;
            this.WorldPVP_advanced_option.CheckedChanged += new System.EventHandler(this.WorldPVP_advanced_option_CheckedChanged);
            // 
            // WorldPVP_advanced
            // 
            this.WorldPVP_advanced.Controls.Add(this.label35);
            this.WorldPVP_advanced.Controls.Add(this.WorldPVP_max_healing_distance);
            this.WorldPVP_advanced.Controls.Add(this.WorldPVP_wanna_target);
            this.WorldPVP_advanced.Controls.Add(this.groupBox50);
            this.WorldPVP_advanced.Controls.Add(this.WorldPVP_wanna_face);
            this.WorldPVP_advanced.Location = new System.Drawing.Point(6, 26);
            this.WorldPVP_advanced.Name = "WorldPVP_advanced";
            this.WorldPVP_advanced.Size = new System.Drawing.Size(301, 173);
            this.WorldPVP_advanced.TabIndex = 33;
            this.WorldPVP_advanced.TabStop = false;
            this.WorldPVP_advanced.Text = "Advanced Options";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(71, 124);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(160, 13);
            this.label35.TabIndex = 33;
            this.label35.Text = "Ignore unit more distant than this";
            // 
            // WorldPVP_max_healing_distance
            // 
            this.WorldPVP_max_healing_distance.Location = new System.Drawing.Point(13, 117);
            this.WorldPVP_max_healing_distance.Name = "WorldPVP_max_healing_distance";
            this.WorldPVP_max_healing_distance.Size = new System.Drawing.Size(52, 20);
            this.WorldPVP_max_healing_distance.TabIndex = 32;
            this.WorldPVP_max_healing_distance.ValueChanged += new System.EventHandler(this.WorldPVP_max_healing_distance_ValueChanged);
            // 
            // WorldPVP_wanna_target
            // 
            this.WorldPVP_wanna_target.AutoSize = true;
            this.WorldPVP_wanna_target.Location = new System.Drawing.Point(13, 101);
            this.WorldPVP_wanna_target.Name = "WorldPVP_wanna_target";
            this.WorldPVP_wanna_target.Size = new System.Drawing.Size(216, 17);
            this.WorldPVP_wanna_target.TabIndex = 27;
            this.WorldPVP_wanna_target.Text = "Target if no target (prevent HBCore bug)";
            this.WorldPVP_wanna_target.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_target.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_target_CheckedChanged);
            // 
            // groupBox50
            // 
            this.groupBox50.Controls.Add(this.WorldPVP_do_not_heal_above);
            this.groupBox50.Controls.Add(this.label51);
            this.groupBox50.Location = new System.Drawing.Point(13, 19);
            this.groupBox50.Name = "groupBox50";
            this.groupBox50.Size = new System.Drawing.Size(248, 53);
            this.groupBox50.TabIndex = 31;
            this.groupBox50.TabStop = false;
            this.groupBox50.Text = "Do not consider People above this health";
            // 
            // WorldPVP_do_not_heal_above
            // 
            this.WorldPVP_do_not_heal_above.Location = new System.Drawing.Point(6, 19);
            this.WorldPVP_do_not_heal_above.Name = "WorldPVP_do_not_heal_above";
            this.WorldPVP_do_not_heal_above.Size = new System.Drawing.Size(46, 20);
            this.WorldPVP_do_not_heal_above.TabIndex = 29;
            this.WorldPVP_do_not_heal_above.ValueChanged += new System.EventHandler(this.WorldPVP_do_not_heal_above_ValueChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(58, 26);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(142, 13);
            this.label51.TabIndex = 30;
            this.label51.Text = "DO NOT MESS WITH THIS";
            // 
            // WorldPVP_wanna_face
            // 
            this.WorldPVP_wanna_face.AutoSize = true;
            this.WorldPVP_wanna_face.Location = new System.Drawing.Point(13, 78);
            this.WorldPVP_wanna_face.Name = "WorldPVP_wanna_face";
            this.WorldPVP_wanna_face.Size = new System.Drawing.Size(166, 17);
            this.WorldPVP_wanna_face.TabIndex = 11;
            this.WorldPVP_wanna_face.Text = "Face the target when needed";
            this.WorldPVP_wanna_face.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_face.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_face_CheckedChanged);
            // 
            // groupBox51
            // 
            this.groupBox51.Controls.Add(this.WorldPVP_cleanse_only_self_and_tank);
            this.groupBox51.Controls.Add(this.WorldPVP_wanna_cleanse);
            this.groupBox51.Controls.Add(this.WorldPVP_wanna_urgent_cleanse);
            this.groupBox51.Location = new System.Drawing.Point(822, 314);
            this.groupBox51.Name = "groupBox51";
            this.groupBox51.Size = new System.Drawing.Size(146, 101);
            this.groupBox51.TabIndex = 84;
            this.groupBox51.TabStop = false;
            this.groupBox51.Text = "Cleanse";
            // 
            // WorldPVP_cleanse_only_self_and_tank
            // 
            this.WorldPVP_cleanse_only_self_and_tank.AutoSize = true;
            this.WorldPVP_cleanse_only_self_and_tank.Location = new System.Drawing.Point(17, 65);
            this.WorldPVP_cleanse_only_self_and_tank.Name = "WorldPVP_cleanse_only_self_and_tank";
            this.WorldPVP_cleanse_only_self_and_tank.Size = new System.Drawing.Size(134, 17);
            this.WorldPVP_cleanse_only_self_and_tank.TabIndex = 30;
            this.WorldPVP_cleanse_only_self_and_tank.Text = "But only Self and Tank";
            this.WorldPVP_cleanse_only_self_and_tank.UseVisualStyleBackColor = true;
            this.WorldPVP_cleanse_only_self_and_tank.CheckedChanged += new System.EventHandler(this.WorldPVP_cleanse_only_self_and_tank_CheckedChanged);
            // 
            // WorldPVP_wanna_cleanse
            // 
            this.WorldPVP_wanna_cleanse.AutoSize = true;
            this.WorldPVP_wanna_cleanse.Location = new System.Drawing.Point(9, 42);
            this.WorldPVP_wanna_cleanse.Name = "WorldPVP_wanna_cleanse";
            this.WorldPVP_wanna_cleanse.Size = new System.Drawing.Size(64, 17);
            this.WorldPVP_wanna_cleanse.TabIndex = 5;
            this.WorldPVP_wanna_cleanse.Text = "Cleanse";
            this.WorldPVP_wanna_cleanse.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_cleanse.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_cleanse_CheckedChanged);
            // 
            // WorldPVP_wanna_urgent_cleanse
            // 
            this.WorldPVP_wanna_urgent_cleanse.AutoSize = true;
            this.WorldPVP_wanna_urgent_cleanse.Location = new System.Drawing.Point(9, 19);
            this.WorldPVP_wanna_urgent_cleanse.Name = "WorldPVP_wanna_urgent_cleanse";
            this.WorldPVP_wanna_urgent_cleanse.Size = new System.Drawing.Size(135, 17);
            this.WorldPVP_wanna_urgent_cleanse.TabIndex = 27;
            this.WorldPVP_wanna_urgent_cleanse.Text = "Cleanse Urgents ASAP";
            this.WorldPVP_wanna_urgent_cleanse.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_urgent_cleanse.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_urgent_cleanse_CheckedChanged);
            // 
            // groupBox52
            // 
            this.groupBox52.Controls.Add(this.WorldPVP_wanna_HoJ);
            this.groupBox52.Controls.Add(this.WorldPVP_wanna_rebuke);
            this.groupBox52.Location = new System.Drawing.Point(822, 125);
            this.groupBox52.Name = "groupBox52";
            this.groupBox52.Size = new System.Drawing.Size(146, 77);
            this.groupBox52.TabIndex = 83;
            this.groupBox52.TabStop = false;
            this.groupBox52.Text = "Interrupts";
            // 
            // WorldPVP_wanna_HoJ
            // 
            this.WorldPVP_wanna_HoJ.AutoSize = true;
            this.WorldPVP_wanna_HoJ.Location = new System.Drawing.Point(9, 19);
            this.WorldPVP_wanna_HoJ.Name = "WorldPVP_wanna_HoJ";
            this.WorldPVP_wanna_HoJ.Size = new System.Drawing.Size(113, 17);
            this.WorldPVP_wanna_HoJ.TabIndex = 17;
            this.WorldPVP_wanna_HoJ.Text = "Hammer of Justice";
            this.WorldPVP_wanna_HoJ.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_HoJ.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_HoJ_CheckedChanged);
            // 
            // WorldPVP_wanna_rebuke
            // 
            this.WorldPVP_wanna_rebuke.AutoSize = true;
            this.WorldPVP_wanna_rebuke.Location = new System.Drawing.Point(9, 43);
            this.WorldPVP_wanna_rebuke.Name = "WorldPVP_wanna_rebuke";
            this.WorldPVP_wanna_rebuke.Size = new System.Drawing.Size(64, 17);
            this.WorldPVP_wanna_rebuke.TabIndex = 26;
            this.WorldPVP_wanna_rebuke.Text = "Rebuke";
            this.WorldPVP_wanna_rebuke.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_rebuke.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_rebuke_CheckedChanged);
            // 
            // groupBox53
            // 
            this.groupBox53.Controls.Add(this.WorldPVP_min_Inf_of_light_DL_hpLB);
            this.groupBox53.Controls.Add(this.WorldPVP_min_Inf_of_light_DL_hp);
            this.groupBox53.Controls.Add(this.label66);
            this.groupBox53.Controls.Add(this.WorldPVP_min_HL_hp);
            this.groupBox53.Controls.Add(this.label67);
            this.groupBox53.Controls.Add(this.WorldPVP_min_FoL_hp);
            this.groupBox53.Controls.Add(this.label68);
            this.groupBox53.Controls.Add(this.WorldPVP_min_DL_hp);
            this.groupBox53.Controls.Add(this.WorldPVP_Inf_of_light_wanna_DL);
            this.groupBox53.Location = new System.Drawing.Point(325, 19);
            this.groupBox53.Name = "groupBox53";
            this.groupBox53.Size = new System.Drawing.Size(249, 160);
            this.groupBox53.TabIndex = 82;
            this.groupBox53.TabStop = false;
            this.groupBox53.Text = "Healing";
            // 
            // WorldPVP_min_Inf_of_light_DL_hpLB
            // 
            this.WorldPVP_min_Inf_of_light_DL_hpLB.AutoSize = true;
            this.WorldPVP_min_Inf_of_light_DL_hpLB.Location = new System.Drawing.Point(72, 130);
            this.WorldPVP_min_Inf_of_light_DL_hpLB.Name = "WorldPVP_min_Inf_of_light_DL_hpLB";
            this.WorldPVP_min_Inf_of_light_DL_hpLB.Size = new System.Drawing.Size(172, 13);
            this.WorldPVP_min_Inf_of_light_DL_hpLB.TabIndex = 7;
            this.WorldPVP_min_Inf_of_light_DL_hpLB.Text = "But only if the target is below this %";
            // 
            // WorldPVP_min_Inf_of_light_DL_hp
            // 
            this.WorldPVP_min_Inf_of_light_DL_hp.Location = new System.Drawing.Point(7, 124);
            this.WorldPVP_min_Inf_of_light_DL_hp.Name = "WorldPVP_min_Inf_of_light_DL_hp";
            this.WorldPVP_min_Inf_of_light_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.WorldPVP_min_Inf_of_light_DL_hp.TabIndex = 6;
            this.WorldPVP_min_Inf_of_light_DL_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_Inf_of_light_DL_hp_ValueChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(70, 26);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(149, 13);
            this.label66.TabIndex = 5;
            this.label66.Text = "Holy Light targets under this %";
            // 
            // WorldPVP_min_HL_hp
            // 
            this.WorldPVP_min_HL_hp.Location = new System.Drawing.Point(7, 20);
            this.WorldPVP_min_HL_hp.Name = "WorldPVP_min_HL_hp";
            this.WorldPVP_min_HL_hp.Size = new System.Drawing.Size(56, 20);
            this.WorldPVP_min_HL_hp.TabIndex = 4;
            this.WorldPVP_min_HL_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_HL_hp_ValueChanged);
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(69, 76);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(165, 13);
            this.label67.TabIndex = 3;
            this.label67.Text = "Flash of Light targets under this %";
            // 
            // WorldPVP_min_FoL_hp
            // 
            this.WorldPVP_min_FoL_hp.Location = new System.Drawing.Point(7, 74);
            this.WorldPVP_min_FoL_hp.Name = "WorldPVP_min_FoL_hp";
            this.WorldPVP_min_FoL_hp.Size = new System.Drawing.Size(56, 20);
            this.WorldPVP_min_FoL_hp.TabIndex = 2;
            this.WorldPVP_min_FoL_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_FoL_hp_ValueChanged);
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(68, 53);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(158, 13);
            this.label68.TabIndex = 1;
            this.label68.Text = "Divine Light targets under this %";
            // 
            // WorldPVP_min_DL_hp
            // 
            this.WorldPVP_min_DL_hp.Location = new System.Drawing.Point(6, 46);
            this.WorldPVP_min_DL_hp.Name = "WorldPVP_min_DL_hp";
            this.WorldPVP_min_DL_hp.Size = new System.Drawing.Size(56, 20);
            this.WorldPVP_min_DL_hp.TabIndex = 0;
            this.WorldPVP_min_DL_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_DL_hp_ValueChanged);
            // 
            // WorldPVP_Inf_of_light_wanna_DL
            // 
            this.WorldPVP_Inf_of_light_wanna_DL.AutoSize = true;
            this.WorldPVP_Inf_of_light_wanna_DL.Location = new System.Drawing.Point(5, 100);
            this.WorldPVP_Inf_of_light_wanna_DL.Name = "WorldPVP_Inf_of_light_wanna_DL";
            this.WorldPVP_Inf_of_light_wanna_DL.Size = new System.Drawing.Size(235, 17);
            this.WorldPVP_Inf_of_light_wanna_DL.TabIndex = 2;
            this.WorldPVP_Inf_of_light_wanna_DL.Text = "Use Divine Light when Infusion of Light proc";
            this.WorldPVP_Inf_of_light_wanna_DL.UseVisualStyleBackColor = true;
            this.WorldPVP_Inf_of_light_wanna_DL.CheckedChanged += new System.EventHandler(this.WorldPVP_Inf_of_light_wanna_DL_CheckedChanged);
            // 
            // groupBox54
            // 
            this.groupBox54.Controls.Add(this.label69);
            this.groupBox54.Controls.Add(this.WorldPVP_use_mana_rec_trinket_every);
            this.groupBox54.Controls.Add(this.label70);
            this.groupBox54.Controls.Add(this.WorldPVP_min_mana_rec_trinket);
            this.groupBox54.Controls.Add(this.label71);
            this.groupBox54.Controls.Add(this.WorldPVP_min_Divine_Plea_mana);
            this.groupBox54.Controls.Add(this.label72);
            this.groupBox54.Controls.Add(this.WorldPVP_mana_judge);
            this.groupBox54.Location = new System.Drawing.Point(12, 166);
            this.groupBox54.Name = "groupBox54";
            this.groupBox54.Size = new System.Drawing.Size(307, 126);
            this.groupBox54.TabIndex = 81;
            this.groupBox54.TabStop = false;
            this.groupBox54.Text = "Mana Management";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(49, 103);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(186, 13);
            this.label69.TabIndex = 7;
            this.label69.Text = "Activate your mana trinket every (NYI)";
            // 
            // WorldPVP_use_mana_rec_trinket_every
            // 
            this.WorldPVP_use_mana_rec_trinket_every.Location = new System.Drawing.Point(0, 100);
            this.WorldPVP_use_mana_rec_trinket_every.Name = "WorldPVP_use_mana_rec_trinket_every";
            this.WorldPVP_use_mana_rec_trinket_every.Size = new System.Drawing.Size(43, 20);
            this.WorldPVP_use_mana_rec_trinket_every.TabIndex = 6;
            this.WorldPVP_use_mana_rec_trinket_every.ValueChanged += new System.EventHandler(this.WorldPVP_use_mana_rec_trinket_every_ValueChanged);
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(49, 78);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(208, 13);
            this.label70.TabIndex = 5;
            this.label70.Text = "Activate your Mana trinket at Mana %(NYI)";
            // 
            // WorldPVP_min_mana_rec_trinket
            // 
            this.WorldPVP_min_mana_rec_trinket.Location = new System.Drawing.Point(0, 74);
            this.WorldPVP_min_mana_rec_trinket.Name = "WorldPVP_min_mana_rec_trinket";
            this.WorldPVP_min_mana_rec_trinket.Size = new System.Drawing.Size(43, 20);
            this.WorldPVP_min_mana_rec_trinket.TabIndex = 4;
            this.WorldPVP_min_mana_rec_trinket.ValueChanged += new System.EventHandler(this.WorldPVP_min_mana_rec_trinket_ValueChanged);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(49, 54);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(121, 13);
            this.label71.TabIndex = 3;
            this.label71.Text = "Divine Plea at this mana";
            // 
            // WorldPVP_min_Divine_Plea_mana
            // 
            this.WorldPVP_min_Divine_Plea_mana.Location = new System.Drawing.Point(0, 47);
            this.WorldPVP_min_Divine_Plea_mana.Name = "WorldPVP_min_Divine_Plea_mana";
            this.WorldPVP_min_Divine_Plea_mana.Size = new System.Drawing.Size(43, 20);
            this.WorldPVP_min_Divine_Plea_mana.TabIndex = 2;
            this.WorldPVP_min_Divine_Plea_mana.ValueChanged += new System.EventHandler(this.WorldPVP_min_Divine_Plea_mana_ValueChanged);
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(49, 27);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(242, 13);
            this.label72.TabIndex = 1;
            this.label72.Text = "Judgement on Cooldown when mana is below this";
            // 
            // WorldPVP_mana_judge
            // 
            this.WorldPVP_mana_judge.Location = new System.Drawing.Point(0, 20);
            this.WorldPVP_mana_judge.Name = "WorldPVP_mana_judge";
            this.WorldPVP_mana_judge.Size = new System.Drawing.Size(43, 20);
            this.WorldPVP_mana_judge.TabIndex = 0;
            this.WorldPVP_mana_judge.ValueChanged += new System.EventHandler(this.WorldPVP_mana_judge_ValueChanged);
            // 
            // groupBox55
            // 
            this.groupBox55.Controls.Add(this.label73);
            this.groupBox55.Controls.Add(this.WorldPVP_min_player_inside_HR);
            this.groupBox55.Controls.Add(this.label74);
            this.groupBox55.Controls.Add(this.WorldPVP_HR_how_much_health);
            this.groupBox55.Controls.Add(this.label75);
            this.groupBox55.Controls.Add(this.WorldPVP_HR_how_far);
            this.groupBox55.Controls.Add(this.WorldPVP_wanna_HR);
            this.groupBox55.Location = new System.Drawing.Point(12, 298);
            this.groupBox55.Name = "groupBox55";
            this.groupBox55.Size = new System.Drawing.Size(307, 133);
            this.groupBox55.TabIndex = 80;
            this.groupBox55.TabStop = false;
            this.groupBox55.Text = "Holy Radiance Settings";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(61, 103);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(145, 13);
            this.label73.TabIndex = 27;
            this.label73.Text = "Need this many people inside";
            // 
            // WorldPVP_min_player_inside_HR
            // 
            this.WorldPVP_min_player_inside_HR.Location = new System.Drawing.Point(7, 97);
            this.WorldPVP_min_player_inside_HR.Name = "WorldPVP_min_player_inside_HR";
            this.WorldPVP_min_player_inside_HR.Size = new System.Drawing.Size(44, 20);
            this.WorldPVP_min_player_inside_HR.TabIndex = 26;
            this.WorldPVP_min_player_inside_HR.ValueChanged += new System.EventHandler(this.WorldPVP_min_player_inside_HR_ValueChanged);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(58, 76);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(196, 13);
            this.label74.TabIndex = 25;
            this.label74.Text = "Consider unit with less than this Health%";
            // 
            // WorldPVP_HR_how_much_health
            // 
            this.WorldPVP_HR_how_much_health.Location = new System.Drawing.Point(7, 70);
            this.WorldPVP_HR_how_much_health.Name = "WorldPVP_HR_how_much_health";
            this.WorldPVP_HR_how_much_health.Size = new System.Drawing.Size(44, 20);
            this.WorldPVP_HR_how_much_health.TabIndex = 24;
            this.WorldPVP_HR_how_much_health.ValueChanged += new System.EventHandler(this.WorldPVP_HR_how_much_health_ValueChanged);
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(58, 49);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(221, 13);
            this.label75.TabIndex = 23;
            this.label75.Text = "Consider unit nearer then this to healed target";
            // 
            // WorldPVP_HR_how_far
            // 
            this.WorldPVP_HR_how_far.Location = new System.Drawing.Point(6, 43);
            this.WorldPVP_HR_how_far.Name = "WorldPVP_HR_how_far";
            this.WorldPVP_HR_how_far.Size = new System.Drawing.Size(45, 20);
            this.WorldPVP_HR_how_far.TabIndex = 22;
            this.WorldPVP_HR_how_far.ValueChanged += new System.EventHandler(this.WorldPVP_HR_how_far_ValueChanged);
            // 
            // WorldPVP_wanna_HR
            // 
            this.WorldPVP_wanna_HR.AutoSize = true;
            this.WorldPVP_wanna_HR.Location = new System.Drawing.Point(6, 19);
            this.WorldPVP_wanna_HR.Name = "WorldPVP_wanna_HR";
            this.WorldPVP_wanna_HR.Size = new System.Drawing.Size(96, 17);
            this.WorldPVP_wanna_HR.TabIndex = 21;
            this.WorldPVP_wanna_HR.Text = "Holy Radiance";
            this.WorldPVP_wanna_HR.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_HR.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_HR_CheckedChanged);
            // 
            // groupBox56
            // 
            this.groupBox56.Controls.Add(this.WorldPVP_min_mana_potion);
            this.groupBox56.Controls.Add(this.WorldPVP_min_LoH_hp);
            this.groupBox56.Controls.Add(this.WorldPVP_min_HoS_hp);
            this.groupBox56.Controls.Add(this.WorldPVP_min_HoP_hp);
            this.groupBox56.Controls.Add(this.WorldPVP_min_DS_hp);
            this.groupBox56.Controls.Add(this.label76);
            this.groupBox56.Controls.Add(this.WorldPVP_min_DP_hp);
            this.groupBox56.Controls.Add(this.WorldPVP_wanna_mana_potion);
            this.groupBox56.Controls.Add(this.WorldPVP_wanna_LoH);
            this.groupBox56.Controls.Add(this.WorldPVP_wanna_DP);
            this.groupBox56.Controls.Add(this.WorldPVP_wanna_DS);
            this.groupBox56.Controls.Add(this.WorldPVP_wanna_HoS);
            this.groupBox56.Controls.Add(this.WorldPVP_wanna_HoP);
            this.groupBox56.Location = new System.Drawing.Point(325, 185);
            this.groupBox56.Name = "groupBox56";
            this.groupBox56.Size = new System.Drawing.Size(247, 182);
            this.groupBox56.TabIndex = 77;
            this.groupBox56.TabStop = false;
            this.groupBox56.Text = "Emergency Buttons";
            // 
            // WorldPVP_min_mana_potion
            // 
            this.WorldPVP_min_mana_potion.Location = new System.Drawing.Point(120, 154);
            this.WorldPVP_min_mana_potion.Name = "WorldPVP_min_mana_potion";
            this.WorldPVP_min_mana_potion.Size = new System.Drawing.Size(64, 20);
            this.WorldPVP_min_mana_potion.TabIndex = 30;
            this.WorldPVP_min_mana_potion.ValueChanged += new System.EventHandler(this.WorldPVP_min_mana_potion_ValueChanged);
            // 
            // WorldPVP_min_LoH_hp
            // 
            this.WorldPVP_min_LoH_hp.Location = new System.Drawing.Point(120, 130);
            this.WorldPVP_min_LoH_hp.Name = "WorldPVP_min_LoH_hp";
            this.WorldPVP_min_LoH_hp.Size = new System.Drawing.Size(64, 20);
            this.WorldPVP_min_LoH_hp.TabIndex = 29;
            this.WorldPVP_min_LoH_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_LoH_hp_ValueChanged);
            // 
            // WorldPVP_min_HoS_hp
            // 
            this.WorldPVP_min_HoS_hp.Location = new System.Drawing.Point(120, 106);
            this.WorldPVP_min_HoS_hp.Name = "WorldPVP_min_HoS_hp";
            this.WorldPVP_min_HoS_hp.Size = new System.Drawing.Size(64, 20);
            this.WorldPVP_min_HoS_hp.TabIndex = 28;
            this.WorldPVP_min_HoS_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_HoS_hp_ValueChanged);
            // 
            // WorldPVP_min_HoP_hp
            // 
            this.WorldPVP_min_HoP_hp.Location = new System.Drawing.Point(120, 84);
            this.WorldPVP_min_HoP_hp.Name = "WorldPVP_min_HoP_hp";
            this.WorldPVP_min_HoP_hp.Size = new System.Drawing.Size(64, 20);
            this.WorldPVP_min_HoP_hp.TabIndex = 27;
            this.WorldPVP_min_HoP_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_HoP_hp_ValueChanged);
            // 
            // WorldPVP_min_DS_hp
            // 
            this.WorldPVP_min_DS_hp.Location = new System.Drawing.Point(120, 58);
            this.WorldPVP_min_DS_hp.Name = "WorldPVP_min_DS_hp";
            this.WorldPVP_min_DS_hp.Size = new System.Drawing.Size(64, 20);
            this.WorldPVP_min_DS_hp.TabIndex = 26;
            this.WorldPVP_min_DS_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_DS_hp_ValueChanged);
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(120, 16);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(58, 13);
            this.label76.TabIndex = 25;
            this.label76.Text = "Use Below";
            // 
            // WorldPVP_min_DP_hp
            // 
            this.WorldPVP_min_DP_hp.Location = new System.Drawing.Point(120, 35);
            this.WorldPVP_min_DP_hp.Name = "WorldPVP_min_DP_hp";
            this.WorldPVP_min_DP_hp.Size = new System.Drawing.Size(64, 20);
            this.WorldPVP_min_DP_hp.TabIndex = 24;
            this.WorldPVP_min_DP_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_DP_hp_ValueChanged);
            // 
            // WorldPVP_wanna_mana_potion
            // 
            this.WorldPVP_wanna_mana_potion.AutoSize = true;
            this.WorldPVP_wanna_mana_potion.Location = new System.Drawing.Point(6, 154);
            this.WorldPVP_wanna_mana_potion.Name = "WorldPVP_wanna_mana_potion";
            this.WorldPVP_wanna_mana_potion.Size = new System.Drawing.Size(113, 17);
            this.WorldPVP_wanna_mana_potion.TabIndex = 23;
            this.WorldPVP_wanna_mana_potion.Text = "Mana Potion (NYI)";
            this.WorldPVP_wanna_mana_potion.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_mana_potion.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_mana_potion_CheckedChanged);
            // 
            // WorldPVP_wanna_LoH
            // 
            this.WorldPVP_wanna_LoH.AutoSize = true;
            this.WorldPVP_wanna_LoH.Location = new System.Drawing.Point(6, 130);
            this.WorldPVP_wanna_LoH.Name = "WorldPVP_wanna_LoH";
            this.WorldPVP_wanna_LoH.Size = new System.Drawing.Size(87, 17);
            this.WorldPVP_wanna_LoH.TabIndex = 23;
            this.WorldPVP_wanna_LoH.Text = "Lay on Hand";
            this.WorldPVP_wanna_LoH.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_LoH.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_LoH_CheckedChanged);
            // 
            // WorldPVP_wanna_DP
            // 
            this.WorldPVP_wanna_DP.AutoSize = true;
            this.WorldPVP_wanna_DP.Location = new System.Drawing.Point(6, 38);
            this.WorldPVP_wanna_DP.Name = "WorldPVP_wanna_DP";
            this.WorldPVP_wanna_DP.Size = new System.Drawing.Size(107, 17);
            this.WorldPVP_wanna_DP.TabIndex = 8;
            this.WorldPVP_wanna_DP.Text = "Divine Protection";
            this.WorldPVP_wanna_DP.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_DP.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_DP_CheckedChanged);
            // 
            // WorldPVP_wanna_DS
            // 
            this.WorldPVP_wanna_DS.AutoSize = true;
            this.WorldPVP_wanna_DS.Location = new System.Drawing.Point(6, 61);
            this.WorldPVP_wanna_DS.Name = "WorldPVP_wanna_DS";
            this.WorldPVP_wanna_DS.Size = new System.Drawing.Size(88, 17);
            this.WorldPVP_wanna_DS.TabIndex = 9;
            this.WorldPVP_wanna_DS.Text = "Divine Shield";
            this.WorldPVP_wanna_DS.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_DS.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_DS_CheckedChanged);
            // 
            // WorldPVP_wanna_HoS
            // 
            this.WorldPVP_wanna_HoS.AutoSize = true;
            this.WorldPVP_wanna_HoS.Location = new System.Drawing.Point(6, 107);
            this.WorldPVP_wanna_HoS.Name = "WorldPVP_wanna_HoS";
            this.WorldPVP_wanna_HoS.Size = new System.Drawing.Size(111, 17);
            this.WorldPVP_wanna_HoS.TabIndex = 19;
            this.WorldPVP_wanna_HoS.Text = "Hand of Salvation";
            this.WorldPVP_wanna_HoS.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_HoS.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_HoS_CheckedChanged);
            // 
            // WorldPVP_wanna_HoP
            // 
            this.WorldPVP_wanna_HoP.AutoSize = true;
            this.WorldPVP_wanna_HoP.Location = new System.Drawing.Point(6, 84);
            this.WorldPVP_wanna_HoP.Name = "WorldPVP_wanna_HoP";
            this.WorldPVP_wanna_HoP.Size = new System.Drawing.Size(115, 17);
            this.WorldPVP_wanna_HoP.TabIndex = 18;
            this.WorldPVP_wanna_HoP.Text = "Hand of Protection";
            this.WorldPVP_wanna_HoP.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_HoP.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_HoP_CheckedChanged);
            // 
            // WorldPVP_auraselctGB
            // 
            this.WorldPVP_auraselctGB.Controls.Add(this.WorldPVP_DisabledRB);
            this.WorldPVP_auraselctGB.Controls.Add(this.WorldPVP_resistanceRB);
            this.WorldPVP_auraselctGB.Controls.Add(this.WorldPVP_concentrationRB);
            this.WorldPVP_auraselctGB.Location = new System.Drawing.Point(822, 19);
            this.WorldPVP_auraselctGB.Name = "WorldPVP_auraselctGB";
            this.WorldPVP_auraselctGB.Size = new System.Drawing.Size(146, 100);
            this.WorldPVP_auraselctGB.TabIndex = 79;
            this.WorldPVP_auraselctGB.TabStop = false;
            this.WorldPVP_auraselctGB.Text = "Select Aura";
            this.WorldPVP_auraselctGB.Enter += new System.EventHandler(this.WorldPVP_auraselctGB_Enter);
            // 
            // WorldPVP_DisabledRB
            // 
            this.WorldPVP_DisabledRB.AutoSize = true;
            this.WorldPVP_DisabledRB.Location = new System.Drawing.Point(7, 68);
            this.WorldPVP_DisabledRB.Name = "WorldPVP_DisabledRB";
            this.WorldPVP_DisabledRB.Size = new System.Drawing.Size(66, 17);
            this.WorldPVP_DisabledRB.TabIndex = 2;
            this.WorldPVP_DisabledRB.TabStop = true;
            this.WorldPVP_DisabledRB.Text = "Disabled";
            this.WorldPVP_DisabledRB.UseVisualStyleBackColor = true;
            this.WorldPVP_DisabledRB.CheckedChanged += new System.EventHandler(this.WorldPVP_DisabledRB_CheckedChanged);
            // 
            // WorldPVP_resistanceRB
            // 
            this.WorldPVP_resistanceRB.AutoSize = true;
            this.WorldPVP_resistanceRB.Location = new System.Drawing.Point(7, 44);
            this.WorldPVP_resistanceRB.Name = "WorldPVP_resistanceRB";
            this.WorldPVP_resistanceRB.Size = new System.Drawing.Size(78, 17);
            this.WorldPVP_resistanceRB.TabIndex = 1;
            this.WorldPVP_resistanceRB.TabStop = true;
            this.WorldPVP_resistanceRB.Text = "Resistance";
            this.WorldPVP_resistanceRB.UseVisualStyleBackColor = true;
            this.WorldPVP_resistanceRB.CheckedChanged += new System.EventHandler(this.WorldPVP_resistanceRB_CheckedChanged);
            // 
            // WorldPVP_concentrationRB
            // 
            this.WorldPVP_concentrationRB.AutoSize = true;
            this.WorldPVP_concentrationRB.Location = new System.Drawing.Point(7, 20);
            this.WorldPVP_concentrationRB.Name = "WorldPVP_concentrationRB";
            this.WorldPVP_concentrationRB.Size = new System.Drawing.Size(91, 17);
            this.WorldPVP_concentrationRB.TabIndex = 0;
            this.WorldPVP_concentrationRB.TabStop = true;
            this.WorldPVP_concentrationRB.Text = "Concentration";
            this.WorldPVP_concentrationRB.UseVisualStyleBackColor = true;
            this.WorldPVP_concentrationRB.CheckedChanged += new System.EventHandler(this.WorldPVP_concentrationRB_CheckedChanged);
            // 
            // groupBox58
            // 
            this.groupBox58.Controls.Add(this.WorldPVP_min_torrent_mana_perc);
            this.groupBox58.Controls.Add(this.WorldPVP_min_stoneform);
            this.groupBox58.Controls.Add(this.WorldPVP_min_gift_hp);
            this.groupBox58.Controls.Add(this.WorldPVP_wanna_torrent);
            this.groupBox58.Controls.Add(this.WorldPVP_wanna_stoneform);
            this.groupBox58.Controls.Add(this.WorldPVP_wanna_everymanforhimself);
            this.groupBox58.Controls.Add(this.WorldPVP_wanna_gift);
            this.groupBox58.Location = new System.Drawing.Point(578, 170);
            this.groupBox58.Name = "groupBox58";
            this.groupBox58.Size = new System.Drawing.Size(236, 124);
            this.groupBox58.TabIndex = 75;
            this.groupBox58.TabStop = false;
            this.groupBox58.Text = "Racials";
            // 
            // WorldPVP_min_torrent_mana_perc
            // 
            this.WorldPVP_min_torrent_mana_perc.Location = new System.Drawing.Point(172, 95);
            this.WorldPVP_min_torrent_mana_perc.Name = "WorldPVP_min_torrent_mana_perc";
            this.WorldPVP_min_torrent_mana_perc.Size = new System.Drawing.Size(59, 20);
            this.WorldPVP_min_torrent_mana_perc.TabIndex = 17;
            this.WorldPVP_min_torrent_mana_perc.ValueChanged += new System.EventHandler(this.WorldPVP_min_torrent_mana_perc_ValueChanged);
            // 
            // WorldPVP_min_stoneform
            // 
            this.WorldPVP_min_stoneform.Location = new System.Drawing.Point(172, 71);
            this.WorldPVP_min_stoneform.Name = "WorldPVP_min_stoneform";
            this.WorldPVP_min_stoneform.Size = new System.Drawing.Size(59, 20);
            this.WorldPVP_min_stoneform.TabIndex = 16;
            this.WorldPVP_min_stoneform.ValueChanged += new System.EventHandler(this.WorldPVP_min_stoneform_ValueChanged);
            // 
            // WorldPVP_min_gift_hp
            // 
            this.WorldPVP_min_gift_hp.Location = new System.Drawing.Point(172, 45);
            this.WorldPVP_min_gift_hp.Name = "WorldPVP_min_gift_hp";
            this.WorldPVP_min_gift_hp.Size = new System.Drawing.Size(59, 20);
            this.WorldPVP_min_gift_hp.TabIndex = 15;
            this.WorldPVP_min_gift_hp.ValueChanged += new System.EventHandler(this.WorldPVP_min_gift_hp_ValueChanged);
            // 
            // WorldPVP_wanna_torrent
            // 
            this.WorldPVP_wanna_torrent.AutoSize = true;
            this.WorldPVP_wanna_torrent.Location = new System.Drawing.Point(16, 95);
            this.WorldPVP_wanna_torrent.Name = "WorldPVP_wanna_torrent";
            this.WorldPVP_wanna_torrent.Size = new System.Drawing.Size(147, 17);
            this.WorldPVP_wanna_torrent.TabIndex = 14;
            this.WorldPVP_wanna_torrent.Text = "Arcane Torrent (for mana)";
            this.WorldPVP_wanna_torrent.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_torrent.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_torrent_CheckedChanged);
            // 
            // WorldPVP_wanna_stoneform
            // 
            this.WorldPVP_wanna_stoneform.AutoSize = true;
            this.WorldPVP_wanna_stoneform.Location = new System.Drawing.Point(16, 71);
            this.WorldPVP_wanna_stoneform.Name = "WorldPVP_wanna_stoneform";
            this.WorldPVP_wanna_stoneform.Size = new System.Drawing.Size(74, 17);
            this.WorldPVP_wanna_stoneform.TabIndex = 13;
            this.WorldPVP_wanna_stoneform.Text = "Stoneform";
            this.WorldPVP_wanna_stoneform.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_stoneform.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_stoneform_CheckedChanged);
            // 
            // WorldPVP_wanna_everymanforhimself
            // 
            this.WorldPVP_wanna_everymanforhimself.AutoSize = true;
            this.WorldPVP_wanna_everymanforhimself.Location = new System.Drawing.Point(16, 24);
            this.WorldPVP_wanna_everymanforhimself.Name = "WorldPVP_wanna_everymanforhimself";
            this.WorldPVP_wanna_everymanforhimself.Size = new System.Drawing.Size(129, 17);
            this.WorldPVP_wanna_everymanforhimself.TabIndex = 10;
            this.WorldPVP_wanna_everymanforhimself.Text = "Every Man for Himself";
            this.WorldPVP_wanna_everymanforhimself.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_everymanforhimself.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_everymanforhimself_CheckedChanged);
            // 
            // WorldPVP_wanna_gift
            // 
            this.WorldPVP_wanna_gift.AutoSize = true;
            this.WorldPVP_wanna_gift.Location = new System.Drawing.Point(16, 47);
            this.WorldPVP_wanna_gift.Name = "WorldPVP_wanna_gift";
            this.WorldPVP_wanna_gift.Size = new System.Drawing.Size(104, 17);
            this.WorldPVP_wanna_gift.TabIndex = 12;
            this.WorldPVP_wanna_gift.Text = "Gift of the Naaru";
            this.WorldPVP_wanna_gift.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_gift.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_gift_CheckedChanged);
            // 
            // groupBox59
            // 
            this.groupBox59.Controls.Add(this.WorldPVP_do_not_dismount_EVER);
            this.groupBox59.Controls.Add(this.WorldPVP_do_not_dismount_ooc);
            this.groupBox59.Controls.Add(this.WorldPVP_wanna_move_to_HoJ);
            this.groupBox59.Controls.Add(this.WorldPVP_wanna_mount);
            this.groupBox59.Controls.Add(this.WorldPVP_wanna_move_to_heal);
            this.groupBox59.Controls.Add(this.WorldPVP_wanna_crusader);
            this.groupBox59.Location = new System.Drawing.Point(325, 373);
            this.groupBox59.Name = "groupBox59";
            this.groupBox59.Size = new System.Drawing.Size(249, 156);
            this.groupBox59.TabIndex = 78;
            this.groupBox59.TabStop = false;
            this.groupBox59.Text = "Movement";
            // 
            // WorldPVP_do_not_dismount_EVER
            // 
            this.WorldPVP_do_not_dismount_EVER.AutoSize = true;
            this.WorldPVP_do_not_dismount_EVER.Location = new System.Drawing.Point(6, 137);
            this.WorldPVP_do_not_dismount_EVER.Name = "WorldPVP_do_not_dismount_EVER";
            this.WorldPVP_do_not_dismount_EVER.Size = new System.Drawing.Size(130, 17);
            this.WorldPVP_do_not_dismount_EVER.TabIndex = 27;
            this.WorldPVP_do_not_dismount_EVER.Text = "Don\'t Dismount EVER";
            this.WorldPVP_do_not_dismount_EVER.UseVisualStyleBackColor = true;
            this.WorldPVP_do_not_dismount_EVER.CheckedChanged += new System.EventHandler(this.WorldPVP_do_not_dismount_EVER_CheckedChanged);
            // 
            // WorldPVP_do_not_dismount_ooc
            // 
            this.WorldPVP_do_not_dismount_ooc.AutoSize = true;
            this.WorldPVP_do_not_dismount_ooc.Location = new System.Drawing.Point(6, 113);
            this.WorldPVP_do_not_dismount_ooc.Name = "WorldPVP_do_not_dismount_ooc";
            this.WorldPVP_do_not_dismount_ooc.Size = new System.Drawing.Size(169, 17);
            this.WorldPVP_do_not_dismount_ooc.TabIndex = 26;
            this.WorldPVP_do_not_dismount_ooc.Text = "Don\'t Dismount Out of Combat";
            this.WorldPVP_do_not_dismount_ooc.UseVisualStyleBackColor = true;
            this.WorldPVP_do_not_dismount_ooc.CheckedChanged += new System.EventHandler(this.WorldPVP_do_not_dismount_ooc_CheckedChanged);
            // 
            // WorldPVP_wanna_move_to_HoJ
            // 
            this.WorldPVP_wanna_move_to_HoJ.AutoSize = true;
            this.WorldPVP_wanna_move_to_HoJ.Location = new System.Drawing.Point(7, 66);
            this.WorldPVP_wanna_move_to_HoJ.Name = "WorldPVP_wanna_move_to_HoJ";
            this.WorldPVP_wanna_move_to_HoJ.Size = new System.Drawing.Size(201, 17);
            this.WorldPVP_wanna_move_to_HoJ.TabIndex = 25;
            this.WorldPVP_wanna_move_to_HoJ.Text = "Move in Range to Hammer of Justice";
            this.WorldPVP_wanna_move_to_HoJ.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_move_to_HoJ.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_move_to_HoJ_CheckedChanged);
            // 
            // WorldPVP_wanna_mount
            // 
            this.WorldPVP_wanna_mount.AutoSize = true;
            this.WorldPVP_wanna_mount.Location = new System.Drawing.Point(6, 19);
            this.WorldPVP_wanna_mount.Name = "WorldPVP_wanna_mount";
            this.WorldPVP_wanna_mount.Size = new System.Drawing.Size(73, 17);
            this.WorldPVP_wanna_mount.TabIndex = 23;
            this.WorldPVP_wanna_mount.Text = "Mount Up";
            this.WorldPVP_wanna_mount.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_mount.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_mount_CheckedChanged);
            // 
            // WorldPVP_wanna_move_to_heal
            // 
            this.WorldPVP_wanna_move_to_heal.AutoSize = true;
            this.WorldPVP_wanna_move_to_heal.Location = new System.Drawing.Point(6, 42);
            this.WorldPVP_wanna_move_to_heal.Name = "WorldPVP_wanna_move_to_heal";
            this.WorldPVP_wanna_move_to_heal.Size = new System.Drawing.Size(136, 17);
            this.WorldPVP_wanna_move_to_heal.TabIndex = 24;
            this.WorldPVP_wanna_move_to_heal.Text = "Move in Range to Heal";
            this.WorldPVP_wanna_move_to_heal.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_move_to_heal.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_move_to_heal_CheckedChanged);
            // 
            // WorldPVP_wanna_crusader
            // 
            this.WorldPVP_wanna_crusader.AutoSize = true;
            this.WorldPVP_wanna_crusader.Location = new System.Drawing.Point(6, 89);
            this.WorldPVP_wanna_crusader.Name = "WorldPVP_wanna_crusader";
            this.WorldPVP_wanna_crusader.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.WorldPVP_wanna_crusader.Size = new System.Drawing.Size(213, 17);
            this.WorldPVP_wanna_crusader.TabIndex = 0;
            this.WorldPVP_wanna_crusader.Text = "Switch to Crusader Aura when mounted";
            this.WorldPVP_wanna_crusader.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_crusader.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_crusader_CheckedChanged);
            // 
            // WorldPVP_generalGB
            // 
            this.WorldPVP_generalGB.Controls.Add(this.label77);
            this.WorldPVP_generalGB.Controls.Add(this.WorldPVP_rest_if_mana_below);
            this.WorldPVP_generalGB.Controls.Add(this.WorldPVP_wanna_buff);
            this.WorldPVP_generalGB.Location = new System.Drawing.Point(12, 19);
            this.WorldPVP_generalGB.Name = "WorldPVP_generalGB";
            this.WorldPVP_generalGB.Size = new System.Drawing.Size(307, 66);
            this.WorldPVP_generalGB.TabIndex = 76;
            this.WorldPVP_generalGB.TabStop = false;
            this.WorldPVP_generalGB.Text = "General";
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(64, 44);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(101, 13);
            this.label77.TabIndex = 6;
            this.label77.Text = "Rest at this Mana %";
            // 
            // WorldPVP_rest_if_mana_below
            // 
            this.WorldPVP_rest_if_mana_below.Location = new System.Drawing.Point(5, 41);
            this.WorldPVP_rest_if_mana_below.Name = "WorldPVP_rest_if_mana_below";
            this.WorldPVP_rest_if_mana_below.Size = new System.Drawing.Size(53, 20);
            this.WorldPVP_rest_if_mana_below.TabIndex = 5;
            this.WorldPVP_rest_if_mana_below.ValueChanged += new System.EventHandler(this.WorldPVP_rest_if_mana_below_ValueChanged);
            // 
            // WorldPVP_wanna_buff
            // 
            this.WorldPVP_wanna_buff.AutoSize = true;
            this.WorldPVP_wanna_buff.Location = new System.Drawing.Point(5, 19);
            this.WorldPVP_wanna_buff.Name = "WorldPVP_wanna_buff";
            this.WorldPVP_wanna_buff.Size = new System.Drawing.Size(86, 17);
            this.WorldPVP_wanna_buff.TabIndex = 4;
            this.WorldPVP_wanna_buff.Text = "Enable Buffs";
            this.WorldPVP_wanna_buff.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_buff.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_buff_CheckedChanged);
            // 
            // groupBox61
            // 
            this.groupBox61.Controls.Add(this.WorldPVP_wanna_Judge);
            this.groupBox61.Controls.Add(this.WorldPVP_wanna_CS);
            this.groupBox61.Controls.Add(this.WorldPVP_wanna_HoW);
            this.groupBox61.Location = new System.Drawing.Point(822, 208);
            this.groupBox61.Name = "groupBox61";
            this.groupBox61.Size = new System.Drawing.Size(146, 100);
            this.groupBox61.TabIndex = 74;
            this.groupBox61.TabStop = false;
            this.groupBox61.Text = "DPS";
            // 
            // WorldPVP_wanna_Judge
            // 
            this.WorldPVP_wanna_Judge.AutoSize = true;
            this.WorldPVP_wanna_Judge.Location = new System.Drawing.Point(17, 70);
            this.WorldPVP_wanna_Judge.Name = "WorldPVP_wanna_Judge";
            this.WorldPVP_wanna_Judge.Size = new System.Drawing.Size(78, 17);
            this.WorldPVP_wanna_Judge.TabIndex = 23;
            this.WorldPVP_wanna_Judge.Text = "Judgement";
            this.WorldPVP_wanna_Judge.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_Judge.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_Judge_CheckedChanged);
            // 
            // WorldPVP_wanna_CS
            // 
            this.WorldPVP_wanna_CS.AutoSize = true;
            this.WorldPVP_wanna_CS.Location = new System.Drawing.Point(17, 24);
            this.WorldPVP_wanna_CS.Name = "WorldPVP_wanna_CS";
            this.WorldPVP_wanna_CS.Size = new System.Drawing.Size(98, 17);
            this.WorldPVP_wanna_CS.TabIndex = 6;
            this.WorldPVP_wanna_CS.Text = "Crusader Strike";
            this.WorldPVP_wanna_CS.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_CS.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_CS_CheckedChanged);
            // 
            // WorldPVP_wanna_HoW
            // 
            this.WorldPVP_wanna_HoW.AutoSize = true;
            this.WorldPVP_wanna_HoW.Location = new System.Drawing.Point(17, 47);
            this.WorldPVP_wanna_HoW.Name = "WorldPVP_wanna_HoW";
            this.WorldPVP_wanna_HoW.Size = new System.Drawing.Size(109, 17);
            this.WorldPVP_wanna_HoW.TabIndex = 20;
            this.WorldPVP_wanna_HoW.Text = "Hammer of Wrath";
            this.WorldPVP_wanna_HoW.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_HoW.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_HoW_CheckedChanged);
            // 
            // groupBox62
            // 
            this.groupBox62.Controls.Add(this.WorldPVP_wanna_lifeblood);
            this.groupBox62.Controls.Add(this.label78);
            this.groupBox62.Controls.Add(this.WorldPVP_min_ohshitbutton_activator);
            this.groupBox62.Controls.Add(this.WorldPVP_wanna_GotAK);
            this.groupBox62.Controls.Add(this.WorldPVP_wanna_AW);
            this.groupBox62.Controls.Add(this.WorldPVP_wanna_DF);
            this.groupBox62.Location = new System.Drawing.Point(580, 19);
            this.groupBox62.Name = "groupBox62";
            this.groupBox62.Size = new System.Drawing.Size(236, 145);
            this.groupBox62.TabIndex = 73;
            this.groupBox62.TabStop = false;
            this.groupBox62.Text = "Oh Shit! Buttons";
            // 
            // WorldPVP_wanna_lifeblood
            // 
            this.WorldPVP_wanna_lifeblood.AutoSize = true;
            this.WorldPVP_wanna_lifeblood.Location = new System.Drawing.Point(6, 47);
            this.WorldPVP_wanna_lifeblood.Name = "WorldPVP_wanna_lifeblood";
            this.WorldPVP_wanna_lifeblood.Size = new System.Drawing.Size(69, 17);
            this.WorldPVP_wanna_lifeblood.TabIndex = 11;
            this.WorldPVP_wanna_lifeblood.Text = "Lifeblood";
            this.WorldPVP_wanna_lifeblood.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_lifeblood.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_lifeblood_CheckedChanged);
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(63, 24);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(135, 13);
            this.label78.TabIndex = 10;
            this.label78.Text = "Press if someone is this low";
            // 
            // WorldPVP_min_ohshitbutton_activator
            // 
            this.WorldPVP_min_ohshitbutton_activator.Location = new System.Drawing.Point(6, 20);
            this.WorldPVP_min_ohshitbutton_activator.Name = "WorldPVP_min_ohshitbutton_activator";
            this.WorldPVP_min_ohshitbutton_activator.Size = new System.Drawing.Size(50, 20);
            this.WorldPVP_min_ohshitbutton_activator.TabIndex = 9;
            this.WorldPVP_min_ohshitbutton_activator.ValueChanged += new System.EventHandler(this.WorldPVP_min_ohshitbutton_activator_ValueChanged);
            // 
            // WorldPVP_wanna_GotAK
            // 
            this.WorldPVP_wanna_GotAK.AutoSize = true;
            this.WorldPVP_wanna_GotAK.Location = new System.Drawing.Point(6, 122);
            this.WorldPVP_wanna_GotAK.Name = "WorldPVP_wanna_GotAK";
            this.WorldPVP_wanna_GotAK.Size = new System.Drawing.Size(167, 17);
            this.WorldPVP_wanna_GotAK.TabIndex = 8;
            this.WorldPVP_wanna_GotAK.Text = "Guardian of the Ancient Kings";
            this.WorldPVP_wanna_GotAK.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_GotAK.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_GotAK_CheckedChanged);
            // 
            // WorldPVP_wanna_AW
            // 
            this.WorldPVP_wanna_AW.AutoSize = true;
            this.WorldPVP_wanna_AW.Location = new System.Drawing.Point(6, 70);
            this.WorldPVP_wanna_AW.Name = "WorldPVP_wanna_AW";
            this.WorldPVP_wanna_AW.Size = new System.Drawing.Size(103, 17);
            this.WorldPVP_wanna_AW.TabIndex = 3;
            this.WorldPVP_wanna_AW.Text = "Avenging Wrath";
            this.WorldPVP_wanna_AW.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_AW.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_AW_CheckedChanged);
            // 
            // WorldPVP_wanna_DF
            // 
            this.WorldPVP_wanna_DF.AutoSize = true;
            this.WorldPVP_wanna_DF.Location = new System.Drawing.Point(6, 95);
            this.WorldPVP_wanna_DF.Name = "WorldPVP_wanna_DF";
            this.WorldPVP_wanna_DF.Size = new System.Drawing.Size(86, 17);
            this.WorldPVP_wanna_DF.TabIndex = 7;
            this.WorldPVP_wanna_DF.Text = "Divine Favor";
            this.WorldPVP_wanna_DF.UseVisualStyleBackColor = true;
            this.WorldPVP_wanna_DF.CheckedChanged += new System.EventHandler(this.WorldPVP_wanna_DF_CheckedChanged);
            // 
            // tabPage9
            // 
            this.tabPage9.AutoScroll = true;
            this.tabPage9.Controls.Add(this.SH_GB8);
            this.tabPage9.Controls.Add(this.SH_GB7);
            this.tabPage9.Controls.Add(this.SH_GB6);
            this.tabPage9.Controls.Add(this.SE_GB5);
            this.tabPage9.Controls.Add(this.SE_GB4);
            this.tabPage9.Controls.Add(this.SE_GB3);
            this.tabPage9.Controls.Add(this.SE_GB2);
            this.tabPage9.Controls.Add(this.SH_GB1);
            this.tabPage9.Controls.Add(this.btnRefresh);
            this.tabPage9.Controls.Add(this.SelectHeal);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(994, 653);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = "Selective Healing";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // SH_GB8
            // 
            this.SH_GB8.Controls.Add(this.RaidL35);
            this.SH_GB8.Controls.Add(this.RaidL36);
            this.SH_GB8.Controls.Add(this.RaidL37);
            this.SH_GB8.Controls.Add(this.RaidL38);
            this.SH_GB8.Controls.Add(this.RaidL39);
            this.SH_GB8.Controls.Add(this.RaidCK35);
            this.SH_GB8.Controls.Add(this.RaidCK39);
            this.SH_GB8.Controls.Add(this.RaidCK36);
            this.SH_GB8.Controls.Add(this.RaidCK38);
            this.SH_GB8.Controls.Add(this.RaidCK37);
            this.SH_GB8.Location = new System.Drawing.Point(714, 218);
            this.SH_GB8.Name = "SH_GB8";
            this.SH_GB8.Size = new System.Drawing.Size(200, 121);
            this.SH_GB8.TabIndex = 59;
            this.SH_GB8.TabStop = false;
            this.SH_GB8.Text = "Gruppo 8";
            // 
            // RaidL35
            // 
            this.RaidL35.AutoSize = true;
            this.RaidL35.Location = new System.Drawing.Point(6, 16);
            this.RaidL35.Name = "RaidL35";
            this.RaidL35.Size = new System.Drawing.Size(14, 13);
            this.RaidL35.TabIndex = 46;
            this.RaidL35.Text = "T";
            this.RaidL35.Visible = false;
            // 
            // RaidL36
            // 
            this.RaidL36.AutoSize = true;
            this.RaidL36.Location = new System.Drawing.Point(6, 35);
            this.RaidL36.Name = "RaidL36";
            this.RaidL36.Size = new System.Drawing.Size(14, 13);
            this.RaidL36.TabIndex = 47;
            this.RaidL36.Text = "T";
            this.RaidL36.Visible = false;
            // 
            // RaidL37
            // 
            this.RaidL37.AutoSize = true;
            this.RaidL37.Location = new System.Drawing.Point(6, 55);
            this.RaidL37.Name = "RaidL37";
            this.RaidL37.Size = new System.Drawing.Size(14, 13);
            this.RaidL37.TabIndex = 48;
            this.RaidL37.Text = "T";
            this.RaidL37.Visible = false;
            // 
            // RaidL38
            // 
            this.RaidL38.AutoSize = true;
            this.RaidL38.Location = new System.Drawing.Point(6, 80);
            this.RaidL38.Name = "RaidL38";
            this.RaidL38.Size = new System.Drawing.Size(14, 13);
            this.RaidL38.TabIndex = 49;
            this.RaidL38.Text = "T";
            this.RaidL38.Visible = false;
            // 
            // RaidL39
            // 
            this.RaidL39.AutoSize = true;
            this.RaidL39.Location = new System.Drawing.Point(6, 103);
            this.RaidL39.Name = "RaidL39";
            this.RaidL39.Size = new System.Drawing.Size(14, 13);
            this.RaidL39.TabIndex = 50;
            this.RaidL39.Text = "T";
            this.RaidL39.Visible = false;
            // 
            // RaidCK35
            // 
            this.RaidCK35.AutoSize = true;
            this.RaidCK35.Location = new System.Drawing.Point(26, 16);
            this.RaidCK35.Name = "RaidCK35";
            this.RaidCK35.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK35.Size = new System.Drawing.Size(86, 17);
            this.RaidCK35.TabIndex = 21;
            this.RaidCK35.Text = "checkBox19";
            this.RaidCK35.UseVisualStyleBackColor = true;
            this.RaidCK35.CheckedChanged += new System.EventHandler(this.RaidCK35_CheckedChanged);
            // 
            // RaidCK39
            // 
            this.RaidCK39.AutoSize = true;
            this.RaidCK39.Location = new System.Drawing.Point(26, 103);
            this.RaidCK39.Name = "RaidCK39";
            this.RaidCK39.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK39.Size = new System.Drawing.Size(86, 17);
            this.RaidCK39.TabIndex = 25;
            this.RaidCK39.Text = "checkBox23";
            this.RaidCK39.UseVisualStyleBackColor = true;
            this.RaidCK39.CheckedChanged += new System.EventHandler(this.RaidCK39_CheckedChanged);
            // 
            // RaidCK36
            // 
            this.RaidCK36.AutoSize = true;
            this.RaidCK36.Location = new System.Drawing.Point(26, 38);
            this.RaidCK36.Name = "RaidCK36";
            this.RaidCK36.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK36.Size = new System.Drawing.Size(86, 17);
            this.RaidCK36.TabIndex = 22;
            this.RaidCK36.Text = "checkBox20";
            this.RaidCK36.UseVisualStyleBackColor = true;
            this.RaidCK36.CheckedChanged += new System.EventHandler(this.RaidCK36_CheckedChanged);
            // 
            // RaidCK38
            // 
            this.RaidCK38.AutoSize = true;
            this.RaidCK38.Location = new System.Drawing.Point(26, 80);
            this.RaidCK38.Name = "RaidCK38";
            this.RaidCK38.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK38.Size = new System.Drawing.Size(86, 17);
            this.RaidCK38.TabIndex = 24;
            this.RaidCK38.Text = "checkBox22";
            this.RaidCK38.UseVisualStyleBackColor = true;
            this.RaidCK38.CheckedChanged += new System.EventHandler(this.RaidCK38_CheckedChanged);
            // 
            // RaidCK37
            // 
            this.RaidCK37.AutoSize = true;
            this.RaidCK37.Location = new System.Drawing.Point(26, 59);
            this.RaidCK37.Name = "RaidCK37";
            this.RaidCK37.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK37.Size = new System.Drawing.Size(86, 17);
            this.RaidCK37.TabIndex = 23;
            this.RaidCK37.Text = "checkBox21";
            this.RaidCK37.UseVisualStyleBackColor = true;
            this.RaidCK37.CheckedChanged += new System.EventHandler(this.RaidCK37_CheckedChanged);
            // 
            // SH_GB7
            // 
            this.SH_GB7.Controls.Add(this.RaidL30);
            this.SH_GB7.Controls.Add(this.RaidL31);
            this.SH_GB7.Controls.Add(this.RaidL32);
            this.SH_GB7.Controls.Add(this.RaidL33);
            this.SH_GB7.Controls.Add(this.RaidL34);
            this.SH_GB7.Controls.Add(this.RaidCK30);
            this.SH_GB7.Controls.Add(this.RaidCK34);
            this.SH_GB7.Controls.Add(this.RaidCK31);
            this.SH_GB7.Controls.Add(this.RaidCK33);
            this.SH_GB7.Controls.Add(this.RaidCK32);
            this.SH_GB7.Location = new System.Drawing.Point(478, 218);
            this.SH_GB7.Name = "SH_GB7";
            this.SH_GB7.Size = new System.Drawing.Size(200, 121);
            this.SH_GB7.TabIndex = 58;
            this.SH_GB7.TabStop = false;
            this.SH_GB7.Text = "Group 7";
            // 
            // RaidL30
            // 
            this.RaidL30.AutoSize = true;
            this.RaidL30.Location = new System.Drawing.Point(6, 16);
            this.RaidL30.Name = "RaidL30";
            this.RaidL30.Size = new System.Drawing.Size(14, 13);
            this.RaidL30.TabIndex = 46;
            this.RaidL30.Text = "T";
            this.RaidL30.Visible = false;
            // 
            // RaidL31
            // 
            this.RaidL31.AutoSize = true;
            this.RaidL31.Location = new System.Drawing.Point(6, 35);
            this.RaidL31.Name = "RaidL31";
            this.RaidL31.Size = new System.Drawing.Size(14, 13);
            this.RaidL31.TabIndex = 47;
            this.RaidL31.Text = "T";
            this.RaidL31.Visible = false;
            // 
            // RaidL32
            // 
            this.RaidL32.AutoSize = true;
            this.RaidL32.Location = new System.Drawing.Point(6, 55);
            this.RaidL32.Name = "RaidL32";
            this.RaidL32.Size = new System.Drawing.Size(14, 13);
            this.RaidL32.TabIndex = 48;
            this.RaidL32.Text = "T";
            this.RaidL32.Visible = false;
            // 
            // RaidL33
            // 
            this.RaidL33.AutoSize = true;
            this.RaidL33.Location = new System.Drawing.Point(6, 80);
            this.RaidL33.Name = "RaidL33";
            this.RaidL33.Size = new System.Drawing.Size(14, 13);
            this.RaidL33.TabIndex = 49;
            this.RaidL33.Text = "T";
            this.RaidL33.Visible = false;
            // 
            // RaidL34
            // 
            this.RaidL34.AutoSize = true;
            this.RaidL34.Location = new System.Drawing.Point(6, 103);
            this.RaidL34.Name = "RaidL34";
            this.RaidL34.Size = new System.Drawing.Size(14, 13);
            this.RaidL34.TabIndex = 50;
            this.RaidL34.Text = "T";
            this.RaidL34.Visible = false;
            // 
            // RaidCK30
            // 
            this.RaidCK30.AutoSize = true;
            this.RaidCK30.Location = new System.Drawing.Point(26, 16);
            this.RaidCK30.Name = "RaidCK30";
            this.RaidCK30.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK30.Size = new System.Drawing.Size(86, 17);
            this.RaidCK30.TabIndex = 21;
            this.RaidCK30.Text = "checkBox19";
            this.RaidCK30.UseVisualStyleBackColor = true;
            this.RaidCK30.CheckedChanged += new System.EventHandler(this.RaidCK30_CheckedChanged);
            // 
            // RaidCK34
            // 
            this.RaidCK34.AutoSize = true;
            this.RaidCK34.Location = new System.Drawing.Point(26, 103);
            this.RaidCK34.Name = "RaidCK34";
            this.RaidCK34.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK34.Size = new System.Drawing.Size(86, 17);
            this.RaidCK34.TabIndex = 25;
            this.RaidCK34.Text = "checkBox23";
            this.RaidCK34.UseVisualStyleBackColor = true;
            this.RaidCK34.CheckedChanged += new System.EventHandler(this.RaidCK34_CheckedChanged);
            // 
            // RaidCK31
            // 
            this.RaidCK31.AutoSize = true;
            this.RaidCK31.Location = new System.Drawing.Point(26, 38);
            this.RaidCK31.Name = "RaidCK31";
            this.RaidCK31.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK31.Size = new System.Drawing.Size(86, 17);
            this.RaidCK31.TabIndex = 22;
            this.RaidCK31.Text = "checkBox20";
            this.RaidCK31.UseVisualStyleBackColor = true;
            this.RaidCK31.CheckedChanged += new System.EventHandler(this.RaidCK31_CheckedChanged);
            // 
            // RaidCK33
            // 
            this.RaidCK33.AutoSize = true;
            this.RaidCK33.Location = new System.Drawing.Point(26, 80);
            this.RaidCK33.Name = "RaidCK33";
            this.RaidCK33.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK33.Size = new System.Drawing.Size(86, 17);
            this.RaidCK33.TabIndex = 24;
            this.RaidCK33.Text = "checkBox22";
            this.RaidCK33.UseVisualStyleBackColor = true;
            this.RaidCK33.CheckedChanged += new System.EventHandler(this.RaidCK33_CheckedChanged);
            // 
            // RaidCK32
            // 
            this.RaidCK32.AutoSize = true;
            this.RaidCK32.Location = new System.Drawing.Point(26, 59);
            this.RaidCK32.Name = "RaidCK32";
            this.RaidCK32.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK32.Size = new System.Drawing.Size(86, 17);
            this.RaidCK32.TabIndex = 23;
            this.RaidCK32.Text = "checkBox21";
            this.RaidCK32.UseVisualStyleBackColor = true;
            this.RaidCK32.CheckedChanged += new System.EventHandler(this.RaidCK32_CheckedChanged);
            // 
            // SH_GB6
            // 
            this.SH_GB6.Controls.Add(this.RaidL25);
            this.SH_GB6.Controls.Add(this.RaidL26);
            this.SH_GB6.Controls.Add(this.RaidL27);
            this.SH_GB6.Controls.Add(this.RaidL28);
            this.SH_GB6.Controls.Add(this.RaidL29);
            this.SH_GB6.Controls.Add(this.RaidCK25);
            this.SH_GB6.Controls.Add(this.RaidCK29);
            this.SH_GB6.Controls.Add(this.RaidCK26);
            this.SH_GB6.Controls.Add(this.RaidCK28);
            this.SH_GB6.Controls.Add(this.RaidCK27);
            this.SH_GB6.Location = new System.Drawing.Point(243, 218);
            this.SH_GB6.Name = "SH_GB6";
            this.SH_GB6.Size = new System.Drawing.Size(200, 121);
            this.SH_GB6.TabIndex = 57;
            this.SH_GB6.TabStop = false;
            this.SH_GB6.Text = "Group 6";
            // 
            // RaidL25
            // 
            this.RaidL25.AutoSize = true;
            this.RaidL25.Location = new System.Drawing.Point(6, 16);
            this.RaidL25.Name = "RaidL25";
            this.RaidL25.Size = new System.Drawing.Size(14, 13);
            this.RaidL25.TabIndex = 46;
            this.RaidL25.Text = "T";
            this.RaidL25.Visible = false;
            // 
            // RaidL26
            // 
            this.RaidL26.AutoSize = true;
            this.RaidL26.Location = new System.Drawing.Point(6, 35);
            this.RaidL26.Name = "RaidL26";
            this.RaidL26.Size = new System.Drawing.Size(14, 13);
            this.RaidL26.TabIndex = 47;
            this.RaidL26.Text = "T";
            this.RaidL26.Visible = false;
            // 
            // RaidL27
            // 
            this.RaidL27.AutoSize = true;
            this.RaidL27.Location = new System.Drawing.Point(6, 55);
            this.RaidL27.Name = "RaidL27";
            this.RaidL27.Size = new System.Drawing.Size(14, 13);
            this.RaidL27.TabIndex = 48;
            this.RaidL27.Text = "T";
            this.RaidL27.Visible = false;
            // 
            // RaidL28
            // 
            this.RaidL28.AutoSize = true;
            this.RaidL28.Location = new System.Drawing.Point(6, 80);
            this.RaidL28.Name = "RaidL28";
            this.RaidL28.Size = new System.Drawing.Size(14, 13);
            this.RaidL28.TabIndex = 49;
            this.RaidL28.Text = "T";
            this.RaidL28.Visible = false;
            // 
            // RaidL29
            // 
            this.RaidL29.AutoSize = true;
            this.RaidL29.Location = new System.Drawing.Point(6, 103);
            this.RaidL29.Name = "RaidL29";
            this.RaidL29.Size = new System.Drawing.Size(14, 13);
            this.RaidL29.TabIndex = 50;
            this.RaidL29.Text = "T";
            this.RaidL29.Visible = false;
            // 
            // RaidCK25
            // 
            this.RaidCK25.AutoSize = true;
            this.RaidCK25.Location = new System.Drawing.Point(26, 16);
            this.RaidCK25.Name = "RaidCK25";
            this.RaidCK25.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK25.Size = new System.Drawing.Size(86, 17);
            this.RaidCK25.TabIndex = 21;
            this.RaidCK25.Text = "checkBox19";
            this.RaidCK25.UseVisualStyleBackColor = true;
            this.RaidCK25.CheckedChanged += new System.EventHandler(this.RaidCK25_CheckedChanged);
            // 
            // RaidCK29
            // 
            this.RaidCK29.AutoSize = true;
            this.RaidCK29.Location = new System.Drawing.Point(26, 103);
            this.RaidCK29.Name = "RaidCK29";
            this.RaidCK29.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK29.Size = new System.Drawing.Size(86, 17);
            this.RaidCK29.TabIndex = 25;
            this.RaidCK29.Text = "checkBox23";
            this.RaidCK29.UseVisualStyleBackColor = true;
            this.RaidCK29.CheckedChanged += new System.EventHandler(this.RaidCK29_CheckedChanged);
            // 
            // RaidCK26
            // 
            this.RaidCK26.AutoSize = true;
            this.RaidCK26.Location = new System.Drawing.Point(26, 38);
            this.RaidCK26.Name = "RaidCK26";
            this.RaidCK26.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK26.Size = new System.Drawing.Size(86, 17);
            this.RaidCK26.TabIndex = 22;
            this.RaidCK26.Text = "checkBox20";
            this.RaidCK26.UseVisualStyleBackColor = true;
            this.RaidCK26.CheckedChanged += new System.EventHandler(this.RaidCK26_CheckedChanged);
            // 
            // RaidCK28
            // 
            this.RaidCK28.AutoSize = true;
            this.RaidCK28.Location = new System.Drawing.Point(26, 80);
            this.RaidCK28.Name = "RaidCK28";
            this.RaidCK28.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK28.Size = new System.Drawing.Size(86, 17);
            this.RaidCK28.TabIndex = 24;
            this.RaidCK28.Text = "checkBox22";
            this.RaidCK28.UseVisualStyleBackColor = true;
            this.RaidCK28.CheckedChanged += new System.EventHandler(this.RaidCK28_CheckedChanged);
            // 
            // RaidCK27
            // 
            this.RaidCK27.AutoSize = true;
            this.RaidCK27.Location = new System.Drawing.Point(26, 59);
            this.RaidCK27.Name = "RaidCK27";
            this.RaidCK27.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK27.Size = new System.Drawing.Size(86, 17);
            this.RaidCK27.TabIndex = 23;
            this.RaidCK27.Text = "checkBox21";
            this.RaidCK27.UseVisualStyleBackColor = true;
            this.RaidCK27.CheckedChanged += new System.EventHandler(this.RaidCK27_CheckedChanged);
            // 
            // SE_GB5
            // 
            this.SE_GB5.Controls.Add(this.RaidL20);
            this.SE_GB5.Controls.Add(this.RaidL21);
            this.SE_GB5.Controls.Add(this.RaidL22);
            this.SE_GB5.Controls.Add(this.RaidL23);
            this.SE_GB5.Controls.Add(this.RaidL24);
            this.SE_GB5.Controls.Add(this.RaidCK20);
            this.SE_GB5.Controls.Add(this.RaidCK24);
            this.SE_GB5.Controls.Add(this.RaidCK21);
            this.SE_GB5.Controls.Add(this.RaidCK23);
            this.SE_GB5.Controls.Add(this.RaidCK22);
            this.SE_GB5.Location = new System.Drawing.Point(15, 215);
            this.SE_GB5.Name = "SE_GB5";
            this.SE_GB5.Size = new System.Drawing.Size(200, 121);
            this.SE_GB5.TabIndex = 56;
            this.SE_GB5.TabStop = false;
            this.SE_GB5.Text = "Group 5";
            // 
            // RaidL20
            // 
            this.RaidL20.AutoSize = true;
            this.RaidL20.Location = new System.Drawing.Point(6, 16);
            this.RaidL20.Name = "RaidL20";
            this.RaidL20.Size = new System.Drawing.Size(14, 13);
            this.RaidL20.TabIndex = 46;
            this.RaidL20.Text = "T";
            this.RaidL20.Visible = false;
            // 
            // RaidL21
            // 
            this.RaidL21.AutoSize = true;
            this.RaidL21.Location = new System.Drawing.Point(6, 38);
            this.RaidL21.Name = "RaidL21";
            this.RaidL21.Size = new System.Drawing.Size(14, 13);
            this.RaidL21.TabIndex = 47;
            this.RaidL21.Text = "T";
            this.RaidL21.Visible = false;
            // 
            // RaidL22
            // 
            this.RaidL22.AutoSize = true;
            this.RaidL22.Location = new System.Drawing.Point(6, 57);
            this.RaidL22.Name = "RaidL22";
            this.RaidL22.Size = new System.Drawing.Size(14, 13);
            this.RaidL22.TabIndex = 48;
            this.RaidL22.Text = "T";
            this.RaidL22.Visible = false;
            // 
            // RaidL23
            // 
            this.RaidL23.AutoSize = true;
            this.RaidL23.Location = new System.Drawing.Point(6, 80);
            this.RaidL23.Name = "RaidL23";
            this.RaidL23.Size = new System.Drawing.Size(14, 13);
            this.RaidL23.TabIndex = 49;
            this.RaidL23.Text = "T";
            this.RaidL23.Visible = false;
            // 
            // RaidL24
            // 
            this.RaidL24.AutoSize = true;
            this.RaidL24.Location = new System.Drawing.Point(6, 103);
            this.RaidL24.Name = "RaidL24";
            this.RaidL24.Size = new System.Drawing.Size(14, 13);
            this.RaidL24.TabIndex = 50;
            this.RaidL24.Text = "T";
            this.RaidL24.Visible = false;
            // 
            // RaidCK20
            // 
            this.RaidCK20.AutoSize = true;
            this.RaidCK20.Location = new System.Drawing.Point(26, 16);
            this.RaidCK20.Name = "RaidCK20";
            this.RaidCK20.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK20.Size = new System.Drawing.Size(86, 17);
            this.RaidCK20.TabIndex = 21;
            this.RaidCK20.Text = "checkBox19";
            this.RaidCK20.UseVisualStyleBackColor = true;
            this.RaidCK20.CheckedChanged += new System.EventHandler(this.RaidCK20_CheckedChanged);
            // 
            // RaidCK24
            // 
            this.RaidCK24.AutoSize = true;
            this.RaidCK24.Location = new System.Drawing.Point(26, 103);
            this.RaidCK24.Name = "RaidCK24";
            this.RaidCK24.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK24.Size = new System.Drawing.Size(86, 17);
            this.RaidCK24.TabIndex = 25;
            this.RaidCK24.Text = "checkBox23";
            this.RaidCK24.UseVisualStyleBackColor = true;
            this.RaidCK24.CheckedChanged += new System.EventHandler(this.RaidCK24_CheckedChanged);
            // 
            // RaidCK21
            // 
            this.RaidCK21.AutoSize = true;
            this.RaidCK21.Location = new System.Drawing.Point(26, 38);
            this.RaidCK21.Name = "RaidCK21";
            this.RaidCK21.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK21.Size = new System.Drawing.Size(86, 17);
            this.RaidCK21.TabIndex = 22;
            this.RaidCK21.Text = "checkBox20";
            this.RaidCK21.UseVisualStyleBackColor = true;
            this.RaidCK21.CheckedChanged += new System.EventHandler(this.RaidCK21_CheckedChanged);
            // 
            // RaidCK23
            // 
            this.RaidCK23.AutoSize = true;
            this.RaidCK23.Location = new System.Drawing.Point(26, 80);
            this.RaidCK23.Name = "RaidCK23";
            this.RaidCK23.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK23.Size = new System.Drawing.Size(86, 17);
            this.RaidCK23.TabIndex = 24;
            this.RaidCK23.Text = "checkBox22";
            this.RaidCK23.UseVisualStyleBackColor = true;
            this.RaidCK23.CheckedChanged += new System.EventHandler(this.RaidCK23_CheckedChanged);
            // 
            // RaidCK22
            // 
            this.RaidCK22.AutoSize = true;
            this.RaidCK22.Location = new System.Drawing.Point(26, 59);
            this.RaidCK22.Name = "RaidCK22";
            this.RaidCK22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK22.Size = new System.Drawing.Size(86, 17);
            this.RaidCK22.TabIndex = 23;
            this.RaidCK22.Text = "checkBox21";
            this.RaidCK22.UseVisualStyleBackColor = true;
            this.RaidCK22.CheckedChanged += new System.EventHandler(this.RaidCK22_CheckedChanged);
            // 
            // SE_GB4
            // 
            this.SE_GB4.Controls.Add(this.RaidL15);
            this.SE_GB4.Controls.Add(this.RaidL16);
            this.SE_GB4.Controls.Add(this.RaidL17);
            this.SE_GB4.Controls.Add(this.RaidL18);
            this.SE_GB4.Controls.Add(this.RaidL19);
            this.SE_GB4.Controls.Add(this.RaidCK15);
            this.SE_GB4.Controls.Add(this.RaidCK16);
            this.SE_GB4.Controls.Add(this.RaidCK17);
            this.SE_GB4.Controls.Add(this.RaidCK18);
            this.SE_GB4.Controls.Add(this.RaidCK19);
            this.SE_GB4.Location = new System.Drawing.Point(714, 60);
            this.SE_GB4.Name = "SE_GB4";
            this.SE_GB4.Size = new System.Drawing.Size(200, 121);
            this.SE_GB4.TabIndex = 55;
            this.SE_GB4.TabStop = false;
            this.SE_GB4.Text = "Group 4";
            // 
            // RaidL15
            // 
            this.RaidL15.AutoSize = true;
            this.RaidL15.Location = new System.Drawing.Point(6, 16);
            this.RaidL15.Name = "RaidL15";
            this.RaidL15.Size = new System.Drawing.Size(14, 13);
            this.RaidL15.TabIndex = 41;
            this.RaidL15.Text = "T";
            this.RaidL15.Visible = false;
            // 
            // RaidL16
            // 
            this.RaidL16.AutoSize = true;
            this.RaidL16.Location = new System.Drawing.Point(6, 39);
            this.RaidL16.Name = "RaidL16";
            this.RaidL16.Size = new System.Drawing.Size(14, 13);
            this.RaidL16.TabIndex = 42;
            this.RaidL16.Text = "T";
            this.RaidL16.Visible = false;
            // 
            // RaidL17
            // 
            this.RaidL17.AutoSize = true;
            this.RaidL17.Location = new System.Drawing.Point(6, 59);
            this.RaidL17.Name = "RaidL17";
            this.RaidL17.Size = new System.Drawing.Size(14, 13);
            this.RaidL17.TabIndex = 43;
            this.RaidL17.Text = "T";
            this.RaidL17.Visible = false;
            // 
            // RaidL18
            // 
            this.RaidL18.AutoSize = true;
            this.RaidL18.Location = new System.Drawing.Point(6, 79);
            this.RaidL18.Name = "RaidL18";
            this.RaidL18.Size = new System.Drawing.Size(14, 13);
            this.RaidL18.TabIndex = 44;
            this.RaidL18.Text = "T";
            this.RaidL18.Visible = false;
            // 
            // RaidL19
            // 
            this.RaidL19.AutoSize = true;
            this.RaidL19.Location = new System.Drawing.Point(6, 99);
            this.RaidL19.Name = "RaidL19";
            this.RaidL19.Size = new System.Drawing.Size(14, 13);
            this.RaidL19.TabIndex = 45;
            this.RaidL19.Text = "T";
            this.RaidL19.Visible = false;
            // 
            // RaidCK15
            // 
            this.RaidCK15.AutoSize = true;
            this.RaidCK15.Location = new System.Drawing.Point(26, 12);
            this.RaidCK15.Name = "RaidCK15";
            this.RaidCK15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK15.Size = new System.Drawing.Size(86, 17);
            this.RaidCK15.TabIndex = 16;
            this.RaidCK15.Text = "checkBox14";
            this.RaidCK15.UseVisualStyleBackColor = true;
            this.RaidCK15.CheckedChanged += new System.EventHandler(this.RaidCK15_CheckedChanged);
            // 
            // RaidCK16
            // 
            this.RaidCK16.AutoSize = true;
            this.RaidCK16.Location = new System.Drawing.Point(26, 35);
            this.RaidCK16.Name = "RaidCK16";
            this.RaidCK16.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK16.Size = new System.Drawing.Size(86, 17);
            this.RaidCK16.TabIndex = 17;
            this.RaidCK16.Text = "checkBox15";
            this.RaidCK16.UseVisualStyleBackColor = true;
            this.RaidCK16.CheckedChanged += new System.EventHandler(this.RaidCK16_CheckedChanged);
            // 
            // RaidCK17
            // 
            this.RaidCK17.AutoSize = true;
            this.RaidCK17.Location = new System.Drawing.Point(26, 55);
            this.RaidCK17.Name = "RaidCK17";
            this.RaidCK17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK17.Size = new System.Drawing.Size(86, 17);
            this.RaidCK17.TabIndex = 18;
            this.RaidCK17.Text = "checkBox16";
            this.RaidCK17.UseVisualStyleBackColor = true;
            this.RaidCK17.CheckedChanged += new System.EventHandler(this.RaidCK17_CheckedChanged);
            // 
            // RaidCK18
            // 
            this.RaidCK18.AutoSize = true;
            this.RaidCK18.Location = new System.Drawing.Point(26, 75);
            this.RaidCK18.Name = "RaidCK18";
            this.RaidCK18.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK18.Size = new System.Drawing.Size(86, 17);
            this.RaidCK18.TabIndex = 19;
            this.RaidCK18.Text = "checkBox17";
            this.RaidCK18.UseVisualStyleBackColor = true;
            this.RaidCK18.CheckedChanged += new System.EventHandler(this.RaidCK18_CheckedChanged);
            // 
            // RaidCK19
            // 
            this.RaidCK19.AutoSize = true;
            this.RaidCK19.Location = new System.Drawing.Point(26, 95);
            this.RaidCK19.Name = "RaidCK19";
            this.RaidCK19.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK19.Size = new System.Drawing.Size(86, 17);
            this.RaidCK19.TabIndex = 20;
            this.RaidCK19.Text = "checkBox18";
            this.RaidCK19.UseVisualStyleBackColor = true;
            this.RaidCK19.CheckedChanged += new System.EventHandler(this.RaidCK19_CheckedChanged);
            // 
            // SE_GB3
            // 
            this.SE_GB3.Controls.Add(this.RaidL10);
            this.SE_GB3.Controls.Add(this.RaidL11);
            this.SE_GB3.Controls.Add(this.RaidL12);
            this.SE_GB3.Controls.Add(this.RaidL13);
            this.SE_GB3.Controls.Add(this.RaidL14);
            this.SE_GB3.Controls.Add(this.RaidCK10);
            this.SE_GB3.Controls.Add(this.RaidCK11);
            this.SE_GB3.Controls.Add(this.RaidCK12);
            this.SE_GB3.Controls.Add(this.RaidCK13);
            this.SE_GB3.Controls.Add(this.RaidCK14);
            this.SE_GB3.Location = new System.Drawing.Point(478, 60);
            this.SE_GB3.Name = "SE_GB3";
            this.SE_GB3.Size = new System.Drawing.Size(200, 121);
            this.SE_GB3.TabIndex = 54;
            this.SE_GB3.TabStop = false;
            this.SE_GB3.Text = "Group 3";
            // 
            // RaidL10
            // 
            this.RaidL10.AutoSize = true;
            this.RaidL10.Location = new System.Drawing.Point(6, 16);
            this.RaidL10.Name = "RaidL10";
            this.RaidL10.Size = new System.Drawing.Size(14, 13);
            this.RaidL10.TabIndex = 36;
            this.RaidL10.Text = "T";
            this.RaidL10.Visible = false;
            // 
            // RaidL11
            // 
            this.RaidL11.AutoSize = true;
            this.RaidL11.Location = new System.Drawing.Point(6, 39);
            this.RaidL11.Name = "RaidL11";
            this.RaidL11.Size = new System.Drawing.Size(14, 13);
            this.RaidL11.TabIndex = 37;
            this.RaidL11.Text = "T";
            this.RaidL11.Visible = false;
            // 
            // RaidL12
            // 
            this.RaidL12.AutoSize = true;
            this.RaidL12.Location = new System.Drawing.Point(6, 59);
            this.RaidL12.Name = "RaidL12";
            this.RaidL12.Size = new System.Drawing.Size(14, 13);
            this.RaidL12.TabIndex = 38;
            this.RaidL12.Text = "T";
            this.RaidL12.Visible = false;
            // 
            // RaidL13
            // 
            this.RaidL13.AutoSize = true;
            this.RaidL13.Location = new System.Drawing.Point(6, 79);
            this.RaidL13.Name = "RaidL13";
            this.RaidL13.Size = new System.Drawing.Size(14, 13);
            this.RaidL13.TabIndex = 39;
            this.RaidL13.Text = "T";
            this.RaidL13.Visible = false;
            // 
            // RaidL14
            // 
            this.RaidL14.AutoSize = true;
            this.RaidL14.Location = new System.Drawing.Point(6, 99);
            this.RaidL14.Name = "RaidL14";
            this.RaidL14.Size = new System.Drawing.Size(14, 13);
            this.RaidL14.TabIndex = 40;
            this.RaidL14.Text = "T";
            this.RaidL14.Visible = false;
            // 
            // RaidCK10
            // 
            this.RaidCK10.AutoSize = true;
            this.RaidCK10.Location = new System.Drawing.Point(23, 16);
            this.RaidCK10.Name = "RaidCK10";
            this.RaidCK10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK10.Size = new System.Drawing.Size(80, 17);
            this.RaidCK10.TabIndex = 11;
            this.RaidCK10.Text = "checkBox9";
            this.RaidCK10.UseVisualStyleBackColor = true;
            this.RaidCK10.CheckedChanged += new System.EventHandler(this.RaidCK10_CheckedChanged);
            // 
            // RaidCK11
            // 
            this.RaidCK11.AutoSize = true;
            this.RaidCK11.Location = new System.Drawing.Point(23, 39);
            this.RaidCK11.Name = "RaidCK11";
            this.RaidCK11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK11.Size = new System.Drawing.Size(86, 17);
            this.RaidCK11.TabIndex = 12;
            this.RaidCK11.Text = "checkBox10";
            this.RaidCK11.UseVisualStyleBackColor = true;
            this.RaidCK11.CheckedChanged += new System.EventHandler(this.RaidCK11_CheckedChanged);
            // 
            // RaidCK12
            // 
            this.RaidCK12.AutoSize = true;
            this.RaidCK12.Location = new System.Drawing.Point(23, 59);
            this.RaidCK12.Name = "RaidCK12";
            this.RaidCK12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK12.Size = new System.Drawing.Size(86, 17);
            this.RaidCK12.TabIndex = 13;
            this.RaidCK12.Text = "checkBox11";
            this.RaidCK12.UseVisualStyleBackColor = true;
            this.RaidCK12.CheckedChanged += new System.EventHandler(this.RaidCK12_CheckedChanged);
            // 
            // RaidCK13
            // 
            this.RaidCK13.AutoSize = true;
            this.RaidCK13.Location = new System.Drawing.Point(23, 80);
            this.RaidCK13.Name = "RaidCK13";
            this.RaidCK13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK13.Size = new System.Drawing.Size(86, 17);
            this.RaidCK13.TabIndex = 14;
            this.RaidCK13.Text = "checkBox12";
            this.RaidCK13.UseVisualStyleBackColor = true;
            this.RaidCK13.CheckedChanged += new System.EventHandler(this.RaidCK13_CheckedChanged);
            // 
            // RaidCK14
            // 
            this.RaidCK14.AutoSize = true;
            this.RaidCK14.Location = new System.Drawing.Point(23, 98);
            this.RaidCK14.Name = "RaidCK14";
            this.RaidCK14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK14.Size = new System.Drawing.Size(86, 17);
            this.RaidCK14.TabIndex = 15;
            this.RaidCK14.Text = "checkBox13";
            this.RaidCK14.UseVisualStyleBackColor = true;
            this.RaidCK14.CheckedChanged += new System.EventHandler(this.RaidCK14_CheckedChanged);
            // 
            // SE_GB2
            // 
            this.SE_GB2.Controls.Add(this.RaidCK9);
            this.SE_GB2.Controls.Add(this.RaidL5);
            this.SE_GB2.Controls.Add(this.RaidCK8);
            this.SE_GB2.Controls.Add(this.RaidL6);
            this.SE_GB2.Controls.Add(this.RaidCK7);
            this.SE_GB2.Controls.Add(this.RaidL7);
            this.SE_GB2.Controls.Add(this.RaidCK6);
            this.SE_GB2.Controls.Add(this.RaidL8);
            this.SE_GB2.Controls.Add(this.RaidCK5);
            this.SE_GB2.Controls.Add(this.RaidL9);
            this.SE_GB2.Location = new System.Drawing.Point(243, 60);
            this.SE_GB2.Name = "SE_GB2";
            this.SE_GB2.Size = new System.Drawing.Size(200, 121);
            this.SE_GB2.TabIndex = 53;
            this.SE_GB2.TabStop = false;
            this.SE_GB2.Text = "Group 2";
            // 
            // RaidCK9
            // 
            this.RaidCK9.AutoSize = true;
            this.RaidCK9.Location = new System.Drawing.Point(24, 99);
            this.RaidCK9.Name = "RaidCK9";
            this.RaidCK9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK9.Size = new System.Drawing.Size(80, 17);
            this.RaidCK9.TabIndex = 10;
            this.RaidCK9.Text = "checkBox8";
            this.RaidCK9.UseVisualStyleBackColor = true;
            this.RaidCK9.CheckedChanged += new System.EventHandler(this.RaidCK9_CheckedChanged);
            // 
            // RaidL5
            // 
            this.RaidL5.AutoSize = true;
            this.RaidL5.Location = new System.Drawing.Point(6, 20);
            this.RaidL5.Name = "RaidL5";
            this.RaidL5.Size = new System.Drawing.Size(14, 13);
            this.RaidL5.TabIndex = 31;
            this.RaidL5.Text = "T";
            // 
            // RaidCK8
            // 
            this.RaidCK8.AutoSize = true;
            this.RaidCK8.Location = new System.Drawing.Point(26, 80);
            this.RaidCK8.Name = "RaidCK8";
            this.RaidCK8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK8.Size = new System.Drawing.Size(80, 17);
            this.RaidCK8.TabIndex = 9;
            this.RaidCK8.Text = "checkBox7";
            this.RaidCK8.UseVisualStyleBackColor = true;
            this.RaidCK8.CheckedChanged += new System.EventHandler(this.RaidCK8_CheckedChanged);
            // 
            // RaidL6
            // 
            this.RaidL6.AutoSize = true;
            this.RaidL6.Location = new System.Drawing.Point(6, 40);
            this.RaidL6.Name = "RaidL6";
            this.RaidL6.Size = new System.Drawing.Size(14, 13);
            this.RaidL6.TabIndex = 32;
            this.RaidL6.Text = "T";
            // 
            // RaidCK7
            // 
            this.RaidCK7.AutoSize = true;
            this.RaidCK7.Location = new System.Drawing.Point(26, 60);
            this.RaidCK7.Name = "RaidCK7";
            this.RaidCK7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK7.Size = new System.Drawing.Size(15, 14);
            this.RaidCK7.TabIndex = 8;
            this.RaidCK7.UseVisualStyleBackColor = true;
            this.RaidCK7.CheckedChanged += new System.EventHandler(this.RaidCK7_CheckedChanged);
            // 
            // RaidL7
            // 
            this.RaidL7.AutoSize = true;
            this.RaidL7.Location = new System.Drawing.Point(6, 59);
            this.RaidL7.Name = "RaidL7";
            this.RaidL7.Size = new System.Drawing.Size(14, 13);
            this.RaidL7.TabIndex = 33;
            this.RaidL7.Text = "T";
            // 
            // RaidCK6
            // 
            this.RaidCK6.AutoSize = true;
            this.RaidCK6.Location = new System.Drawing.Point(26, 40);
            this.RaidCK6.Name = "RaidCK6";
            this.RaidCK6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK6.Size = new System.Drawing.Size(15, 14);
            this.RaidCK6.TabIndex = 7;
            this.RaidCK6.UseVisualStyleBackColor = true;
            this.RaidCK6.CheckedChanged += new System.EventHandler(this.RaidCK6_CheckedChanged);
            // 
            // RaidL8
            // 
            this.RaidL8.AutoSize = true;
            this.RaidL8.Location = new System.Drawing.Point(6, 79);
            this.RaidL8.Name = "RaidL8";
            this.RaidL8.Size = new System.Drawing.Size(14, 13);
            this.RaidL8.TabIndex = 34;
            this.RaidL8.Text = "T";
            // 
            // RaidCK5
            // 
            this.RaidCK5.AutoSize = true;
            this.RaidCK5.Location = new System.Drawing.Point(26, 20);
            this.RaidCK5.Name = "RaidCK5";
            this.RaidCK5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK5.Size = new System.Drawing.Size(15, 14);
            this.RaidCK5.TabIndex = 6;
            this.RaidCK5.UseVisualStyleBackColor = true;
            this.RaidCK5.CheckedChanged += new System.EventHandler(this.RaidCK5_CheckedChanged);
            // 
            // RaidL9
            // 
            this.RaidL9.AutoSize = true;
            this.RaidL9.Location = new System.Drawing.Point(6, 92);
            this.RaidL9.Name = "RaidL9";
            this.RaidL9.Size = new System.Drawing.Size(14, 13);
            this.RaidL9.TabIndex = 35;
            this.RaidL9.Text = "T";
            // 
            // SH_GB1
            // 
            this.SH_GB1.Controls.Add(this.RaidCK0);
            this.SH_GB1.Controls.Add(this.RaidCK1);
            this.SH_GB1.Controls.Add(this.RaidCK2);
            this.SH_GB1.Controls.Add(this.RaidCK3);
            this.SH_GB1.Controls.Add(this.RaidCK4);
            this.SH_GB1.Controls.Add(this.RaidL0);
            this.SH_GB1.Controls.Add(this.RaidL1);
            this.SH_GB1.Controls.Add(this.RaidL2);
            this.SH_GB1.Controls.Add(this.RaidL3);
            this.SH_GB1.Controls.Add(this.RaidL4);
            this.SH_GB1.Location = new System.Drawing.Point(15, 56);
            this.SH_GB1.Name = "SH_GB1";
            this.SH_GB1.Size = new System.Drawing.Size(200, 121);
            this.SH_GB1.TabIndex = 52;
            this.SH_GB1.TabStop = false;
            this.SH_GB1.Text = "Group 1";
            // 
            // RaidCK0
            // 
            this.RaidCK0.AutoSize = true;
            this.RaidCK0.Location = new System.Drawing.Point(33, 19);
            this.RaidCK0.Name = "RaidCK0";
            this.RaidCK0.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK0.Size = new System.Drawing.Size(15, 14);
            this.RaidCK0.TabIndex = 1;
            this.RaidCK0.UseVisualStyleBackColor = true;
            this.RaidCK0.CheckedChanged += new System.EventHandler(this.RaidCK0_CheckedChanged);
            // 
            // RaidCK1
            // 
            this.RaidCK1.AutoSize = true;
            this.RaidCK1.Location = new System.Drawing.Point(33, 39);
            this.RaidCK1.Name = "RaidCK1";
            this.RaidCK1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK1.Size = new System.Drawing.Size(15, 14);
            this.RaidCK1.TabIndex = 2;
            this.RaidCK1.UseVisualStyleBackColor = true;
            this.RaidCK1.CheckedChanged += new System.EventHandler(this.RaidCK1_CheckedChanged);
            // 
            // RaidCK2
            // 
            this.RaidCK2.AutoSize = true;
            this.RaidCK2.Location = new System.Drawing.Point(33, 59);
            this.RaidCK2.Name = "RaidCK2";
            this.RaidCK2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK2.Size = new System.Drawing.Size(15, 14);
            this.RaidCK2.TabIndex = 3;
            this.RaidCK2.UseVisualStyleBackColor = true;
            this.RaidCK2.CheckedChanged += new System.EventHandler(this.RaidCK2_CheckedChanged);
            // 
            // RaidCK3
            // 
            this.RaidCK3.AutoSize = true;
            this.RaidCK3.Location = new System.Drawing.Point(33, 79);
            this.RaidCK3.Name = "RaidCK3";
            this.RaidCK3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK3.Size = new System.Drawing.Size(15, 14);
            this.RaidCK3.TabIndex = 4;
            this.RaidCK3.UseVisualStyleBackColor = true;
            this.RaidCK3.CheckedChanged += new System.EventHandler(this.RaidCK3_CheckedChanged);
            // 
            // RaidCK4
            // 
            this.RaidCK4.AutoSize = true;
            this.RaidCK4.Location = new System.Drawing.Point(33, 99);
            this.RaidCK4.Name = "RaidCK4";
            this.RaidCK4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RaidCK4.Size = new System.Drawing.Size(15, 14);
            this.RaidCK4.TabIndex = 5;
            this.RaidCK4.UseVisualStyleBackColor = true;
            this.RaidCK4.CheckedChanged += new System.EventHandler(this.RaidCK4_CheckedChanged);
            // 
            // RaidL0
            // 
            this.RaidL0.AutoSize = true;
            this.RaidL0.Location = new System.Drawing.Point(13, 20);
            this.RaidL0.Name = "RaidL0";
            this.RaidL0.Size = new System.Drawing.Size(14, 13);
            this.RaidL0.TabIndex = 26;
            this.RaidL0.Text = "T";
            // 
            // RaidL1
            // 
            this.RaidL1.AutoSize = true;
            this.RaidL1.Location = new System.Drawing.Point(13, 40);
            this.RaidL1.Name = "RaidL1";
            this.RaidL1.Size = new System.Drawing.Size(14, 13);
            this.RaidL1.TabIndex = 27;
            this.RaidL1.Text = "T";
            // 
            // RaidL2
            // 
            this.RaidL2.AutoSize = true;
            this.RaidL2.Location = new System.Drawing.Point(13, 60);
            this.RaidL2.Name = "RaidL2";
            this.RaidL2.Size = new System.Drawing.Size(14, 13);
            this.RaidL2.TabIndex = 28;
            this.RaidL2.Text = "T";
            // 
            // RaidL3
            // 
            this.RaidL3.AutoSize = true;
            this.RaidL3.Location = new System.Drawing.Point(13, 80);
            this.RaidL3.Name = "RaidL3";
            this.RaidL3.Size = new System.Drawing.Size(14, 13);
            this.RaidL3.TabIndex = 29;
            this.RaidL3.Text = "T";
            // 
            // RaidL4
            // 
            this.RaidL4.AutoSize = true;
            this.RaidL4.Location = new System.Drawing.Point(13, 100);
            this.RaidL4.Name = "RaidL4";
            this.RaidL4.Size = new System.Drawing.Size(14, 13);
            this.RaidL4.TabIndex = 30;
            this.RaidL4.Text = "T";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(899, 417);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 51;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // SelectHeal
            // 
            this.SelectHeal.AutoSize = true;
            this.SelectHeal.Location = new System.Drawing.Point(367, 24);
            this.SelectHeal.Name = "SelectHeal";
            this.SelectHeal.Size = new System.Drawing.Size(109, 17);
            this.SelectHeal.TabIndex = 0;
            this.SelectHeal.Text = "Selective Healing";
            this.SelectHeal.UseVisualStyleBackColor = true;
            this.SelectHeal.CheckedChanged += new System.EventHandler(this.SelectHeal_CheckedChanged);
            // 
            // tabPage10
            // 
            this.tabPage10.AutoScroll = true;
            this.tabPage10.Controls.Add(this.tabControl2);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new System.Drawing.Size(994, 653);
            this.tabPage10.TabIndex = 9;
            this.tabPage10.Text = "Dispell";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage11);
            this.tabControl2.Controls.Add(this.tabPage12);
            this.tabControl2.Location = new System.Drawing.Point(3, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(999, 647);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.groupBox35);
            this.tabPage11.Controls.Add(this.groupBox21);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(991, 621);
            this.tabPage11.TabIndex = 0;
            this.tabPage11.Text = "PVP";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // groupBox35
            // 
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB20);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB19);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB18);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB17);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB16);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB15);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB14);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB13);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB12);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB11);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB10);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB9);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB8);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB7);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB6);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB5);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB4);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB3);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB2);
            this.groupBox35.Controls.Add(this.PVP_dispell_ASAP_TB1);
            this.groupBox35.Location = new System.Drawing.Point(361, 15);
            this.groupBox35.Name = "groupBox35";
            this.groupBox35.Size = new System.Drawing.Size(268, 577);
            this.groupBox35.TabIndex = 1;
            this.groupBox35.TabStop = false;
            this.groupBox35.Text = "Dispell Theese ASAP";
            // 
            // PVP_dispell_ASAP_TB20
            // 
            this.PVP_dispell_ASAP_TB20.Location = new System.Drawing.Point(7, 514);
            this.PVP_dispell_ASAP_TB20.Name = "PVP_dispell_ASAP_TB20";
            this.PVP_dispell_ASAP_TB20.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB20.TabIndex = 19;
            this.PVP_dispell_ASAP_TB20.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB20_TextChanged);
            // 
            // PVP_dispell_ASAP_TB19
            // 
            this.PVP_dispell_ASAP_TB19.Location = new System.Drawing.Point(7, 488);
            this.PVP_dispell_ASAP_TB19.Name = "PVP_dispell_ASAP_TB19";
            this.PVP_dispell_ASAP_TB19.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB19.TabIndex = 18;
            this.PVP_dispell_ASAP_TB19.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB19_TextChanged);
            // 
            // PVP_dispell_ASAP_TB18
            // 
            this.PVP_dispell_ASAP_TB18.Location = new System.Drawing.Point(7, 462);
            this.PVP_dispell_ASAP_TB18.Name = "PVP_dispell_ASAP_TB18";
            this.PVP_dispell_ASAP_TB18.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB18.TabIndex = 17;
            this.PVP_dispell_ASAP_TB18.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB18_TextChanged);
            // 
            // PVP_dispell_ASAP_TB17
            // 
            this.PVP_dispell_ASAP_TB17.Location = new System.Drawing.Point(7, 436);
            this.PVP_dispell_ASAP_TB17.Name = "PVP_dispell_ASAP_TB17";
            this.PVP_dispell_ASAP_TB17.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB17.TabIndex = 16;
            this.PVP_dispell_ASAP_TB17.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB17_TextChanged);
            // 
            // PVP_dispell_ASAP_TB16
            // 
            this.PVP_dispell_ASAP_TB16.Location = new System.Drawing.Point(7, 410);
            this.PVP_dispell_ASAP_TB16.Name = "PVP_dispell_ASAP_TB16";
            this.PVP_dispell_ASAP_TB16.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB16.TabIndex = 15;
            this.PVP_dispell_ASAP_TB16.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB16_TextChanged);
            // 
            // PVP_dispell_ASAP_TB15
            // 
            this.PVP_dispell_ASAP_TB15.Location = new System.Drawing.Point(7, 384);
            this.PVP_dispell_ASAP_TB15.Name = "PVP_dispell_ASAP_TB15";
            this.PVP_dispell_ASAP_TB15.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB15.TabIndex = 14;
            this.PVP_dispell_ASAP_TB15.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB15_TextChanged);
            // 
            // PVP_dispell_ASAP_TB14
            // 
            this.PVP_dispell_ASAP_TB14.Location = new System.Drawing.Point(7, 358);
            this.PVP_dispell_ASAP_TB14.Name = "PVP_dispell_ASAP_TB14";
            this.PVP_dispell_ASAP_TB14.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB14.TabIndex = 13;
            this.PVP_dispell_ASAP_TB14.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB14_TextChanged);
            // 
            // PVP_dispell_ASAP_TB13
            // 
            this.PVP_dispell_ASAP_TB13.Location = new System.Drawing.Point(7, 332);
            this.PVP_dispell_ASAP_TB13.Name = "PVP_dispell_ASAP_TB13";
            this.PVP_dispell_ASAP_TB13.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB13.TabIndex = 12;
            this.PVP_dispell_ASAP_TB13.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB13_TextChanged);
            // 
            // PVP_dispell_ASAP_TB12
            // 
            this.PVP_dispell_ASAP_TB12.Location = new System.Drawing.Point(7, 306);
            this.PVP_dispell_ASAP_TB12.Name = "PVP_dispell_ASAP_TB12";
            this.PVP_dispell_ASAP_TB12.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB12.TabIndex = 11;
            this.PVP_dispell_ASAP_TB12.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB12_TextChanged);
            // 
            // PVP_dispell_ASAP_TB11
            // 
            this.PVP_dispell_ASAP_TB11.Location = new System.Drawing.Point(7, 280);
            this.PVP_dispell_ASAP_TB11.Name = "PVP_dispell_ASAP_TB11";
            this.PVP_dispell_ASAP_TB11.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB11.TabIndex = 10;
            this.PVP_dispell_ASAP_TB11.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB11_TextChanged);
            // 
            // PVP_dispell_ASAP_TB10
            // 
            this.PVP_dispell_ASAP_TB10.Location = new System.Drawing.Point(7, 254);
            this.PVP_dispell_ASAP_TB10.Name = "PVP_dispell_ASAP_TB10";
            this.PVP_dispell_ASAP_TB10.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB10.TabIndex = 9;
            this.PVP_dispell_ASAP_TB10.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB10_TextChanged);
            // 
            // PVP_dispell_ASAP_TB9
            // 
            this.PVP_dispell_ASAP_TB9.Location = new System.Drawing.Point(7, 228);
            this.PVP_dispell_ASAP_TB9.Name = "PVP_dispell_ASAP_TB9";
            this.PVP_dispell_ASAP_TB9.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB9.TabIndex = 8;
            this.PVP_dispell_ASAP_TB9.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB9_TextChanged);
            // 
            // PVP_dispell_ASAP_TB8
            // 
            this.PVP_dispell_ASAP_TB8.Location = new System.Drawing.Point(7, 202);
            this.PVP_dispell_ASAP_TB8.Name = "PVP_dispell_ASAP_TB8";
            this.PVP_dispell_ASAP_TB8.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB8.TabIndex = 7;
            this.PVP_dispell_ASAP_TB8.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB8_TextChanged);
            // 
            // PVP_dispell_ASAP_TB7
            // 
            this.PVP_dispell_ASAP_TB7.Location = new System.Drawing.Point(7, 176);
            this.PVP_dispell_ASAP_TB7.Name = "PVP_dispell_ASAP_TB7";
            this.PVP_dispell_ASAP_TB7.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB7.TabIndex = 6;
            this.PVP_dispell_ASAP_TB7.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB7_TextChanged);
            // 
            // PVP_dispell_ASAP_TB6
            // 
            this.PVP_dispell_ASAP_TB6.Location = new System.Drawing.Point(7, 150);
            this.PVP_dispell_ASAP_TB6.Name = "PVP_dispell_ASAP_TB6";
            this.PVP_dispell_ASAP_TB6.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB6.TabIndex = 5;
            this.PVP_dispell_ASAP_TB6.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB6_TextChanged);
            // 
            // PVP_dispell_ASAP_TB5
            // 
            this.PVP_dispell_ASAP_TB5.Location = new System.Drawing.Point(7, 124);
            this.PVP_dispell_ASAP_TB5.Name = "PVP_dispell_ASAP_TB5";
            this.PVP_dispell_ASAP_TB5.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB5.TabIndex = 4;
            this.PVP_dispell_ASAP_TB5.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB5_TextChanged);
            // 
            // PVP_dispell_ASAP_TB4
            // 
            this.PVP_dispell_ASAP_TB4.Location = new System.Drawing.Point(7, 98);
            this.PVP_dispell_ASAP_TB4.Name = "PVP_dispell_ASAP_TB4";
            this.PVP_dispell_ASAP_TB4.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB4.TabIndex = 3;
            this.PVP_dispell_ASAP_TB4.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB4_TextChanged);
            // 
            // PVP_dispell_ASAP_TB3
            // 
            this.PVP_dispell_ASAP_TB3.Location = new System.Drawing.Point(7, 72);
            this.PVP_dispell_ASAP_TB3.Name = "PVP_dispell_ASAP_TB3";
            this.PVP_dispell_ASAP_TB3.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB3.TabIndex = 2;
            this.PVP_dispell_ASAP_TB3.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB3_TextChanged);
            // 
            // PVP_dispell_ASAP_TB2
            // 
            this.PVP_dispell_ASAP_TB2.Location = new System.Drawing.Point(7, 46);
            this.PVP_dispell_ASAP_TB2.Name = "PVP_dispell_ASAP_TB2";
            this.PVP_dispell_ASAP_TB2.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB2.TabIndex = 1;
            this.PVP_dispell_ASAP_TB2.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB2_TextChanged);
            // 
            // PVP_dispell_ASAP_TB1
            // 
            this.PVP_dispell_ASAP_TB1.Location = new System.Drawing.Point(7, 20);
            this.PVP_dispell_ASAP_TB1.Name = "PVP_dispell_ASAP_TB1";
            this.PVP_dispell_ASAP_TB1.Size = new System.Drawing.Size(161, 20);
            this.PVP_dispell_ASAP_TB1.TabIndex = 0;
            this.PVP_dispell_ASAP_TB1.TextChanged += new System.EventHandler(this.PVP_dispell_ASAP_TB1_TextChanged);
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB20);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB19);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB18);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB17);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB16);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB15);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB14);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB13);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB12);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB11);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB10);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB9);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB8);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB7);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB6);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB5);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB4);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB3);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB2);
            this.groupBox21.Controls.Add(this.PVP_do_not_touch_TB1);
            this.groupBox21.Location = new System.Drawing.Point(16, 15);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(290, 577);
            this.groupBox21.TabIndex = 0;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Do not Touch People With Theese";
            // 
            // PVP_do_not_touch_TB20
            // 
            this.PVP_do_not_touch_TB20.Location = new System.Drawing.Point(7, 530);
            this.PVP_do_not_touch_TB20.Name = "PVP_do_not_touch_TB20";
            this.PVP_do_not_touch_TB20.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB20.TabIndex = 19;
            this.PVP_do_not_touch_TB20.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB20_TextChanged);
            // 
            // PVP_do_not_touch_TB19
            // 
            this.PVP_do_not_touch_TB19.Location = new System.Drawing.Point(7, 503);
            this.PVP_do_not_touch_TB19.Name = "PVP_do_not_touch_TB19";
            this.PVP_do_not_touch_TB19.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB19.TabIndex = 18;
            this.PVP_do_not_touch_TB19.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB19_TextChanged);
            // 
            // PVP_do_not_touch_TB18
            // 
            this.PVP_do_not_touch_TB18.Location = new System.Drawing.Point(7, 476);
            this.PVP_do_not_touch_TB18.Name = "PVP_do_not_touch_TB18";
            this.PVP_do_not_touch_TB18.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB18.TabIndex = 17;
            this.PVP_do_not_touch_TB18.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB18_TextChanged);
            // 
            // PVP_do_not_touch_TB17
            // 
            this.PVP_do_not_touch_TB17.Location = new System.Drawing.Point(7, 449);
            this.PVP_do_not_touch_TB17.Name = "PVP_do_not_touch_TB17";
            this.PVP_do_not_touch_TB17.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB17.TabIndex = 16;
            this.PVP_do_not_touch_TB17.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB17_TextChanged);
            // 
            // PVP_do_not_touch_TB16
            // 
            this.PVP_do_not_touch_TB16.Location = new System.Drawing.Point(7, 422);
            this.PVP_do_not_touch_TB16.Name = "PVP_do_not_touch_TB16";
            this.PVP_do_not_touch_TB16.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB16.TabIndex = 15;
            this.PVP_do_not_touch_TB16.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB16_TextChanged);
            // 
            // PVP_do_not_touch_TB15
            // 
            this.PVP_do_not_touch_TB15.Location = new System.Drawing.Point(7, 396);
            this.PVP_do_not_touch_TB15.Name = "PVP_do_not_touch_TB15";
            this.PVP_do_not_touch_TB15.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB15.TabIndex = 14;
            this.PVP_do_not_touch_TB15.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB15_TextChanged);
            // 
            // PVP_do_not_touch_TB14
            // 
            this.PVP_do_not_touch_TB14.Location = new System.Drawing.Point(7, 369);
            this.PVP_do_not_touch_TB14.Name = "PVP_do_not_touch_TB14";
            this.PVP_do_not_touch_TB14.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB14.TabIndex = 13;
            this.PVP_do_not_touch_TB14.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB14_TextChanged);
            // 
            // PVP_do_not_touch_TB13
            // 
            this.PVP_do_not_touch_TB13.Location = new System.Drawing.Point(7, 342);
            this.PVP_do_not_touch_TB13.Name = "PVP_do_not_touch_TB13";
            this.PVP_do_not_touch_TB13.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB13.TabIndex = 12;
            this.PVP_do_not_touch_TB13.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB13_TextChanged);
            // 
            // PVP_do_not_touch_TB12
            // 
            this.PVP_do_not_touch_TB12.Location = new System.Drawing.Point(7, 315);
            this.PVP_do_not_touch_TB12.Name = "PVP_do_not_touch_TB12";
            this.PVP_do_not_touch_TB12.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB12.TabIndex = 11;
            this.PVP_do_not_touch_TB12.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB12_TextChanged);
            // 
            // PVP_do_not_touch_TB11
            // 
            this.PVP_do_not_touch_TB11.Location = new System.Drawing.Point(7, 288);
            this.PVP_do_not_touch_TB11.Name = "PVP_do_not_touch_TB11";
            this.PVP_do_not_touch_TB11.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB11.TabIndex = 10;
            this.PVP_do_not_touch_TB11.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB11_TextChanged);
            // 
            // PVP_do_not_touch_TB10
            // 
            this.PVP_do_not_touch_TB10.Location = new System.Drawing.Point(7, 262);
            this.PVP_do_not_touch_TB10.Name = "PVP_do_not_touch_TB10";
            this.PVP_do_not_touch_TB10.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB10.TabIndex = 9;
            this.PVP_do_not_touch_TB10.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB10_TextChanged);
            // 
            // PVP_do_not_touch_TB9
            // 
            this.PVP_do_not_touch_TB9.Location = new System.Drawing.Point(7, 235);
            this.PVP_do_not_touch_TB9.Name = "PVP_do_not_touch_TB9";
            this.PVP_do_not_touch_TB9.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB9.TabIndex = 8;
            this.PVP_do_not_touch_TB9.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB9_TextChanged);
            // 
            // PVP_do_not_touch_TB8
            // 
            this.PVP_do_not_touch_TB8.Location = new System.Drawing.Point(7, 208);
            this.PVP_do_not_touch_TB8.Name = "PVP_do_not_touch_TB8";
            this.PVP_do_not_touch_TB8.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB8.TabIndex = 7;
            this.PVP_do_not_touch_TB8.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB8_TextChanged);
            // 
            // PVP_do_not_touch_TB7
            // 
            this.PVP_do_not_touch_TB7.Location = new System.Drawing.Point(7, 181);
            this.PVP_do_not_touch_TB7.Name = "PVP_do_not_touch_TB7";
            this.PVP_do_not_touch_TB7.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB7.TabIndex = 6;
            this.PVP_do_not_touch_TB7.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB7_TextChanged);
            // 
            // PVP_do_not_touch_TB6
            // 
            this.PVP_do_not_touch_TB6.Location = new System.Drawing.Point(7, 154);
            this.PVP_do_not_touch_TB6.Name = "PVP_do_not_touch_TB6";
            this.PVP_do_not_touch_TB6.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB6.TabIndex = 5;
            this.PVP_do_not_touch_TB6.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB6_TextChanged);
            // 
            // PVP_do_not_touch_TB5
            // 
            this.PVP_do_not_touch_TB5.Location = new System.Drawing.Point(7, 128);
            this.PVP_do_not_touch_TB5.Name = "PVP_do_not_touch_TB5";
            this.PVP_do_not_touch_TB5.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB5.TabIndex = 4;
            this.PVP_do_not_touch_TB5.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB5_TextChanged);
            // 
            // PVP_do_not_touch_TB4
            // 
            this.PVP_do_not_touch_TB4.Location = new System.Drawing.Point(7, 101);
            this.PVP_do_not_touch_TB4.Name = "PVP_do_not_touch_TB4";
            this.PVP_do_not_touch_TB4.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB4.TabIndex = 3;
            this.PVP_do_not_touch_TB4.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB4_TextChanged);
            // 
            // PVP_do_not_touch_TB3
            // 
            this.PVP_do_not_touch_TB3.Location = new System.Drawing.Point(7, 74);
            this.PVP_do_not_touch_TB3.Name = "PVP_do_not_touch_TB3";
            this.PVP_do_not_touch_TB3.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB3.TabIndex = 2;
            this.PVP_do_not_touch_TB3.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB3_TextChanged);
            // 
            // PVP_do_not_touch_TB2
            // 
            this.PVP_do_not_touch_TB2.Location = new System.Drawing.Point(7, 47);
            this.PVP_do_not_touch_TB2.Name = "PVP_do_not_touch_TB2";
            this.PVP_do_not_touch_TB2.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB2.TabIndex = 1;
            this.PVP_do_not_touch_TB2.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB2_TextChanged);
            // 
            // PVP_do_not_touch_TB1
            // 
            this.PVP_do_not_touch_TB1.Location = new System.Drawing.Point(7, 20);
            this.PVP_do_not_touch_TB1.Name = "PVP_do_not_touch_TB1";
            this.PVP_do_not_touch_TB1.Size = new System.Drawing.Size(149, 20);
            this.PVP_do_not_touch_TB1.TabIndex = 0;
            this.PVP_do_not_touch_TB1.TextChanged += new System.EventHandler(this.PVP_do_not_touch_TB1_TextChanged);
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.groupBox49);
            this.tabPage12.Controls.Add(this.groupBox57);
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(991, 621);
            this.tabPage12.TabIndex = 1;
            this.tabPage12.Text = "PVE";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // groupBox49
            // 
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB20);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB19);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB18);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB17);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB16);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB15);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB14);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB13);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB12);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB11);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB10);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB9);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB8);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB7);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB6);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB5);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB4);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB3);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB2);
            this.groupBox49.Controls.Add(this.PVE_dispell_ASAP_TB1);
            this.groupBox49.Location = new System.Drawing.Point(361, 15);
            this.groupBox49.Name = "groupBox49";
            this.groupBox49.Size = new System.Drawing.Size(268, 577);
            this.groupBox49.TabIndex = 3;
            this.groupBox49.TabStop = false;
            this.groupBox49.Text = "Dispell Theese ASAP";
            // 
            // PVE_dispell_ASAP_TB20
            // 
            this.PVE_dispell_ASAP_TB20.Location = new System.Drawing.Point(7, 514);
            this.PVE_dispell_ASAP_TB20.Name = "PVE_dispell_ASAP_TB20";
            this.PVE_dispell_ASAP_TB20.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB20.TabIndex = 19;
            this.PVE_dispell_ASAP_TB20.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB20_TextChanged);
            // 
            // PVE_dispell_ASAP_TB19
            // 
            this.PVE_dispell_ASAP_TB19.Location = new System.Drawing.Point(7, 488);
            this.PVE_dispell_ASAP_TB19.Name = "PVE_dispell_ASAP_TB19";
            this.PVE_dispell_ASAP_TB19.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB19.TabIndex = 18;
            this.PVE_dispell_ASAP_TB19.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB19_TextChanged);
            // 
            // PVE_dispell_ASAP_TB18
            // 
            this.PVE_dispell_ASAP_TB18.Location = new System.Drawing.Point(7, 462);
            this.PVE_dispell_ASAP_TB18.Name = "PVE_dispell_ASAP_TB18";
            this.PVE_dispell_ASAP_TB18.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB18.TabIndex = 17;
            this.PVE_dispell_ASAP_TB18.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB18_TextChanged);
            // 
            // PVE_dispell_ASAP_TB17
            // 
            this.PVE_dispell_ASAP_TB17.Location = new System.Drawing.Point(7, 436);
            this.PVE_dispell_ASAP_TB17.Name = "PVE_dispell_ASAP_TB17";
            this.PVE_dispell_ASAP_TB17.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB17.TabIndex = 16;
            this.PVE_dispell_ASAP_TB17.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB17_TextChanged);
            // 
            // PVE_dispell_ASAP_TB16
            // 
            this.PVE_dispell_ASAP_TB16.Location = new System.Drawing.Point(7, 410);
            this.PVE_dispell_ASAP_TB16.Name = "PVE_dispell_ASAP_TB16";
            this.PVE_dispell_ASAP_TB16.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB16.TabIndex = 15;
            this.PVE_dispell_ASAP_TB16.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB16_TextChanged);
            // 
            // PVE_dispell_ASAP_TB15
            // 
            this.PVE_dispell_ASAP_TB15.Location = new System.Drawing.Point(7, 384);
            this.PVE_dispell_ASAP_TB15.Name = "PVE_dispell_ASAP_TB15";
            this.PVE_dispell_ASAP_TB15.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB15.TabIndex = 14;
            this.PVE_dispell_ASAP_TB15.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB15_TextChanged);
            // 
            // PVE_dispell_ASAP_TB14
            // 
            this.PVE_dispell_ASAP_TB14.Location = new System.Drawing.Point(7, 358);
            this.PVE_dispell_ASAP_TB14.Name = "PVE_dispell_ASAP_TB14";
            this.PVE_dispell_ASAP_TB14.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB14.TabIndex = 13;
            this.PVE_dispell_ASAP_TB14.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB14_TextChanged);
            // 
            // PVE_dispell_ASAP_TB13
            // 
            this.PVE_dispell_ASAP_TB13.Location = new System.Drawing.Point(7, 332);
            this.PVE_dispell_ASAP_TB13.Name = "PVE_dispell_ASAP_TB13";
            this.PVE_dispell_ASAP_TB13.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB13.TabIndex = 12;
            this.PVE_dispell_ASAP_TB13.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB13_TextChanged);
            // 
            // PVE_dispell_ASAP_TB12
            // 
            this.PVE_dispell_ASAP_TB12.Location = new System.Drawing.Point(7, 306);
            this.PVE_dispell_ASAP_TB12.Name = "PVE_dispell_ASAP_TB12";
            this.PVE_dispell_ASAP_TB12.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB12.TabIndex = 11;
            this.PVE_dispell_ASAP_TB12.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB12_TextChanged);
            // 
            // PVE_dispell_ASAP_TB11
            // 
            this.PVE_dispell_ASAP_TB11.Location = new System.Drawing.Point(7, 280);
            this.PVE_dispell_ASAP_TB11.Name = "PVE_dispell_ASAP_TB11";
            this.PVE_dispell_ASAP_TB11.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB11.TabIndex = 10;
            this.PVE_dispell_ASAP_TB11.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB11_TextChanged);
            // 
            // PVE_dispell_ASAP_TB10
            // 
            this.PVE_dispell_ASAP_TB10.Location = new System.Drawing.Point(7, 254);
            this.PVE_dispell_ASAP_TB10.Name = "PVE_dispell_ASAP_TB10";
            this.PVE_dispell_ASAP_TB10.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB10.TabIndex = 9;
            this.PVE_dispell_ASAP_TB10.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB10_TextChanged);
            // 
            // PVE_dispell_ASAP_TB9
            // 
            this.PVE_dispell_ASAP_TB9.Location = new System.Drawing.Point(7, 228);
            this.PVE_dispell_ASAP_TB9.Name = "PVE_dispell_ASAP_TB9";
            this.PVE_dispell_ASAP_TB9.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB9.TabIndex = 8;
            this.PVE_dispell_ASAP_TB9.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB9_TextChanged);
            // 
            // PVE_dispell_ASAP_TB8
            // 
            this.PVE_dispell_ASAP_TB8.Location = new System.Drawing.Point(7, 202);
            this.PVE_dispell_ASAP_TB8.Name = "PVE_dispell_ASAP_TB8";
            this.PVE_dispell_ASAP_TB8.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB8.TabIndex = 7;
            this.PVE_dispell_ASAP_TB8.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB8_TextChanged);
            // 
            // PVE_dispell_ASAP_TB7
            // 
            this.PVE_dispell_ASAP_TB7.Location = new System.Drawing.Point(7, 176);
            this.PVE_dispell_ASAP_TB7.Name = "PVE_dispell_ASAP_TB7";
            this.PVE_dispell_ASAP_TB7.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB7.TabIndex = 6;
            this.PVE_dispell_ASAP_TB7.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB7_TextChanged);
            // 
            // PVE_dispell_ASAP_TB6
            // 
            this.PVE_dispell_ASAP_TB6.Location = new System.Drawing.Point(7, 150);
            this.PVE_dispell_ASAP_TB6.Name = "PVE_dispell_ASAP_TB6";
            this.PVE_dispell_ASAP_TB6.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB6.TabIndex = 5;
            this.PVE_dispell_ASAP_TB6.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB6_TextChanged);
            // 
            // PVE_dispell_ASAP_TB5
            // 
            this.PVE_dispell_ASAP_TB5.Location = new System.Drawing.Point(7, 124);
            this.PVE_dispell_ASAP_TB5.Name = "PVE_dispell_ASAP_TB5";
            this.PVE_dispell_ASAP_TB5.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB5.TabIndex = 4;
            this.PVE_dispell_ASAP_TB5.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB5_TextChanged);
            // 
            // PVE_dispell_ASAP_TB4
            // 
            this.PVE_dispell_ASAP_TB4.Location = new System.Drawing.Point(7, 98);
            this.PVE_dispell_ASAP_TB4.Name = "PVE_dispell_ASAP_TB4";
            this.PVE_dispell_ASAP_TB4.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB4.TabIndex = 3;
            this.PVE_dispell_ASAP_TB4.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB4_TextChanged);
            // 
            // PVE_dispell_ASAP_TB3
            // 
            this.PVE_dispell_ASAP_TB3.Location = new System.Drawing.Point(7, 72);
            this.PVE_dispell_ASAP_TB3.Name = "PVE_dispell_ASAP_TB3";
            this.PVE_dispell_ASAP_TB3.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB3.TabIndex = 2;
            this.PVE_dispell_ASAP_TB3.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB3_TextChanged);
            // 
            // PVE_dispell_ASAP_TB2
            // 
            this.PVE_dispell_ASAP_TB2.Location = new System.Drawing.Point(7, 46);
            this.PVE_dispell_ASAP_TB2.Name = "PVE_dispell_ASAP_TB2";
            this.PVE_dispell_ASAP_TB2.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB2.TabIndex = 1;
            this.PVE_dispell_ASAP_TB2.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB2_TextChanged);
            // 
            // PVE_dispell_ASAP_TB1
            // 
            this.PVE_dispell_ASAP_TB1.Location = new System.Drawing.Point(7, 20);
            this.PVE_dispell_ASAP_TB1.Name = "PVE_dispell_ASAP_TB1";
            this.PVE_dispell_ASAP_TB1.Size = new System.Drawing.Size(161, 20);
            this.PVE_dispell_ASAP_TB1.TabIndex = 0;
            this.PVE_dispell_ASAP_TB1.TextChanged += new System.EventHandler(this.PVE_dispell_ASAP_TB1_TextChanged);
            // 
            // groupBox57
            // 
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB20);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB19);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB18);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB17);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB16);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB15);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB14);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB13);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB12);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB11);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB10);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB9);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB8);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB7);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB6);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB5);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB4);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB3);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB2);
            this.groupBox57.Controls.Add(this.PVE_do_not_touch_TB1);
            this.groupBox57.Location = new System.Drawing.Point(16, 15);
            this.groupBox57.Name = "groupBox57";
            this.groupBox57.Size = new System.Drawing.Size(290, 577);
            this.groupBox57.TabIndex = 2;
            this.groupBox57.TabStop = false;
            this.groupBox57.Text = "Do not Touch People With Theese";
            // 
            // PVE_do_not_touch_TB20
            // 
            this.PVE_do_not_touch_TB20.Location = new System.Drawing.Point(7, 530);
            this.PVE_do_not_touch_TB20.Name = "PVE_do_not_touch_TB20";
            this.PVE_do_not_touch_TB20.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB20.TabIndex = 19;
            this.PVE_do_not_touch_TB20.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB20_TextChanged);
            // 
            // PVE_do_not_touch_TB19
            // 
            this.PVE_do_not_touch_TB19.Location = new System.Drawing.Point(7, 503);
            this.PVE_do_not_touch_TB19.Name = "PVE_do_not_touch_TB19";
            this.PVE_do_not_touch_TB19.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB19.TabIndex = 18;
            this.PVE_do_not_touch_TB19.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB19_TextChanged);
            // 
            // PVE_do_not_touch_TB18
            // 
            this.PVE_do_not_touch_TB18.Location = new System.Drawing.Point(7, 476);
            this.PVE_do_not_touch_TB18.Name = "PVE_do_not_touch_TB18";
            this.PVE_do_not_touch_TB18.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB18.TabIndex = 17;
            this.PVE_do_not_touch_TB18.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB18_TextChanged);
            // 
            // PVE_do_not_touch_TB17
            // 
            this.PVE_do_not_touch_TB17.Location = new System.Drawing.Point(7, 449);
            this.PVE_do_not_touch_TB17.Name = "PVE_do_not_touch_TB17";
            this.PVE_do_not_touch_TB17.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB17.TabIndex = 16;
            this.PVE_do_not_touch_TB17.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB17_TextChanged);
            // 
            // PVE_do_not_touch_TB16
            // 
            this.PVE_do_not_touch_TB16.Location = new System.Drawing.Point(7, 422);
            this.PVE_do_not_touch_TB16.Name = "PVE_do_not_touch_TB16";
            this.PVE_do_not_touch_TB16.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB16.TabIndex = 15;
            this.PVE_do_not_touch_TB16.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB16_TextChanged);
            // 
            // PVE_do_not_touch_TB15
            // 
            this.PVE_do_not_touch_TB15.Location = new System.Drawing.Point(7, 396);
            this.PVE_do_not_touch_TB15.Name = "PVE_do_not_touch_TB15";
            this.PVE_do_not_touch_TB15.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB15.TabIndex = 14;
            this.PVE_do_not_touch_TB15.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB15_TextChanged);
            // 
            // PVE_do_not_touch_TB14
            // 
            this.PVE_do_not_touch_TB14.Location = new System.Drawing.Point(7, 369);
            this.PVE_do_not_touch_TB14.Name = "PVE_do_not_touch_TB14";
            this.PVE_do_not_touch_TB14.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB14.TabIndex = 13;
            this.PVE_do_not_touch_TB14.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB14_TextChanged);
            // 
            // PVE_do_not_touch_TB13
            // 
            this.PVE_do_not_touch_TB13.Location = new System.Drawing.Point(7, 342);
            this.PVE_do_not_touch_TB13.Name = "PVE_do_not_touch_TB13";
            this.PVE_do_not_touch_TB13.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB13.TabIndex = 12;
            this.PVE_do_not_touch_TB13.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB13_TextChanged);
            // 
            // PVE_do_not_touch_TB12
            // 
            this.PVE_do_not_touch_TB12.Location = new System.Drawing.Point(7, 315);
            this.PVE_do_not_touch_TB12.Name = "PVE_do_not_touch_TB12";
            this.PVE_do_not_touch_TB12.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB12.TabIndex = 11;
            this.PVE_do_not_touch_TB12.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB12_TextChanged);
            // 
            // PVE_do_not_touch_TB11
            // 
            this.PVE_do_not_touch_TB11.Location = new System.Drawing.Point(7, 288);
            this.PVE_do_not_touch_TB11.Name = "PVE_do_not_touch_TB11";
            this.PVE_do_not_touch_TB11.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB11.TabIndex = 10;
            this.PVE_do_not_touch_TB11.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB11_TextChanged);
            // 
            // PVE_do_not_touch_TB10
            // 
            this.PVE_do_not_touch_TB10.Location = new System.Drawing.Point(7, 262);
            this.PVE_do_not_touch_TB10.Name = "PVE_do_not_touch_TB10";
            this.PVE_do_not_touch_TB10.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB10.TabIndex = 9;
            this.PVE_do_not_touch_TB10.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB10_TextChanged);
            // 
            // PVE_do_not_touch_TB9
            // 
            this.PVE_do_not_touch_TB9.Location = new System.Drawing.Point(7, 235);
            this.PVE_do_not_touch_TB9.Name = "PVE_do_not_touch_TB9";
            this.PVE_do_not_touch_TB9.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB9.TabIndex = 8;
            this.PVE_do_not_touch_TB9.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB9_TextChanged);
            // 
            // PVE_do_not_touch_TB8
            // 
            this.PVE_do_not_touch_TB8.Location = new System.Drawing.Point(7, 208);
            this.PVE_do_not_touch_TB8.Name = "PVE_do_not_touch_TB8";
            this.PVE_do_not_touch_TB8.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB8.TabIndex = 7;
            this.PVE_do_not_touch_TB8.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB8_TextChanged);
            // 
            // PVE_do_not_touch_TB7
            // 
            this.PVE_do_not_touch_TB7.Location = new System.Drawing.Point(7, 181);
            this.PVE_do_not_touch_TB7.Name = "PVE_do_not_touch_TB7";
            this.PVE_do_not_touch_TB7.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB7.TabIndex = 6;
            this.PVE_do_not_touch_TB7.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB7_TextChanged);
            // 
            // PVE_do_not_touch_TB6
            // 
            this.PVE_do_not_touch_TB6.Location = new System.Drawing.Point(7, 154);
            this.PVE_do_not_touch_TB6.Name = "PVE_do_not_touch_TB6";
            this.PVE_do_not_touch_TB6.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB6.TabIndex = 5;
            this.PVE_do_not_touch_TB6.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB6_TextChanged);
            // 
            // PVE_do_not_touch_TB5
            // 
            this.PVE_do_not_touch_TB5.Location = new System.Drawing.Point(7, 128);
            this.PVE_do_not_touch_TB5.Name = "PVE_do_not_touch_TB5";
            this.PVE_do_not_touch_TB5.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB5.TabIndex = 4;
            this.PVE_do_not_touch_TB5.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB5_TextChanged);
            // 
            // PVE_do_not_touch_TB4
            // 
            this.PVE_do_not_touch_TB4.Location = new System.Drawing.Point(7, 101);
            this.PVE_do_not_touch_TB4.Name = "PVE_do_not_touch_TB4";
            this.PVE_do_not_touch_TB4.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB4.TabIndex = 3;
            this.PVE_do_not_touch_TB4.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB4_TextChanged);
            // 
            // PVE_do_not_touch_TB3
            // 
            this.PVE_do_not_touch_TB3.Location = new System.Drawing.Point(7, 74);
            this.PVE_do_not_touch_TB3.Name = "PVE_do_not_touch_TB3";
            this.PVE_do_not_touch_TB3.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB3.TabIndex = 2;
            this.PVE_do_not_touch_TB3.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB3_TextChanged);
            // 
            // PVE_do_not_touch_TB2
            // 
            this.PVE_do_not_touch_TB2.Location = new System.Drawing.Point(7, 47);
            this.PVE_do_not_touch_TB2.Name = "PVE_do_not_touch_TB2";
            this.PVE_do_not_touch_TB2.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB2.TabIndex = 1;
            this.PVE_do_not_touch_TB2.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB2_TextChanged);
            // 
            // PVE_do_not_touch_TB1
            // 
            this.PVE_do_not_touch_TB1.Location = new System.Drawing.Point(7, 20);
            this.PVE_do_not_touch_TB1.Name = "PVE_do_not_touch_TB1";
            this.PVE_do_not_touch_TB1.Size = new System.Drawing.Size(149, 20);
            this.PVE_do_not_touch_TB1.TabIndex = 0;
            this.PVE_do_not_touch_TB1.TextChanged += new System.EventHandler(this.PVE_do_not_touch_TB1_TextChanged);
            // 
            // tabPage13
            // 
            this.tabPage13.AutoScroll = true;
            this.tabPage13.Controls.Add(this.groupBox63);
            this.tabPage13.Controls.Add(this.groupBox60);
            this.tabPage13.Controls.Add(this.Trnket1_name_load);
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Size = new System.Drawing.Size(994, 653);
            this.tabPage13.TabIndex = 10;
            this.tabPage13.Text = "Trinkets";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // groupBox63
            // 
            this.groupBox63.Controls.Add(this.Trinket2_use_when_GB);
            this.groupBox63.Controls.Add(this.Trinket2_name_LB);
            this.groupBox63.Controls.Add(this.Trinket2_CD_LB);
            this.groupBox63.Controls.Add(this.Trinket2_ID_LB);
            this.groupBox63.Location = new System.Drawing.Point(550, 41);
            this.groupBox63.Name = "groupBox63";
            this.groupBox63.Size = new System.Drawing.Size(310, 245);
            this.groupBox63.TabIndex = 1;
            this.groupBox63.TabStop = false;
            this.groupBox63.Text = "Trinket 2";
            // 
            // Trinket2_use_when_GB
            // 
            this.Trinket2_use_when_GB.Controls.Add(this.Trinket2_PVP);
            this.Trinket2_use_when_GB.Controls.Add(this.Trinket2_mana);
            this.Trinket2_use_when_GB.Controls.Add(this.Trinket2_HPS);
            this.Trinket2_use_when_GB.Controls.Add(this.Trinket2_never);
            this.Trinket2_use_when_GB.Location = new System.Drawing.Point(10, 100);
            this.Trinket2_use_when_GB.Name = "Trinket2_use_when_GB";
            this.Trinket2_use_when_GB.Size = new System.Drawing.Size(257, 124);
            this.Trinket2_use_when_GB.TabIndex = 6;
            this.Trinket2_use_when_GB.TabStop = false;
            this.Trinket2_use_when_GB.Text = "Use When";
            // 
            // Trinket2_PVP
            // 
            this.Trinket2_PVP.AutoSize = true;
            this.Trinket2_PVP.Location = new System.Drawing.Point(7, 92);
            this.Trinket2_PVP.Name = "Trinket2_PVP";
            this.Trinket2_PVP.Size = new System.Drawing.Size(134, 17);
            this.Trinket2_PVP.TabIndex = 3;
            this.Trinket2_PVP.TabStop = true;
            this.Trinket2_PVP.Text = "PVP (remove stun ecc)";
            this.Trinket2_PVP.UseVisualStyleBackColor = true;
            this.Trinket2_PVP.CheckedChanged += new System.EventHandler(this.Trinket2_PVP_CheckedChanged);
            // 
            // Trinket2_mana
            // 
            this.Trinket2_mana.AutoSize = true;
            this.Trinket2_mana.Location = new System.Drawing.Point(7, 68);
            this.Trinket2_mana.Name = "Trinket2_mana";
            this.Trinket2_mana.Size = new System.Drawing.Size(102, 17);
            this.Trinket2_mana.TabIndex = 2;
            this.Trinket2_mana.TabStop = true;
            this.Trinket2_mana.Text = "Mana Cooldown";
            this.Trinket2_mana.UseVisualStyleBackColor = true;
            this.Trinket2_mana.CheckedChanged += new System.EventHandler(this.Trinket2_mana_CheckedChanged);
            // 
            // Trinket2_HPS
            // 
            this.Trinket2_HPS.AutoSize = true;
            this.Trinket2_HPS.Location = new System.Drawing.Point(7, 44);
            this.Trinket2_HPS.Name = "Trinket2_HPS";
            this.Trinket2_HPS.Size = new System.Drawing.Size(111, 17);
            this.Trinket2_HPS.TabIndex = 1;
            this.Trinket2_HPS.TabStop = true;
            this.Trinket2_HPS.Text = "Healing Cooldown";
            this.Trinket2_HPS.UseVisualStyleBackColor = true;
            this.Trinket2_HPS.CheckedChanged += new System.EventHandler(this.Trinket2_HPS_CheckedChanged);
            // 
            // Trinket2_never
            // 
            this.Trinket2_never.AutoSize = true;
            this.Trinket2_never.Location = new System.Drawing.Point(7, 20);
            this.Trinket2_never.Name = "Trinket2_never";
            this.Trinket2_never.Size = new System.Drawing.Size(54, 17);
            this.Trinket2_never.TabIndex = 0;
            this.Trinket2_never.TabStop = true;
            this.Trinket2_never.Text = "Never";
            this.Trinket2_never.UseVisualStyleBackColor = true;
            this.Trinket2_never.CheckedChanged += new System.EventHandler(this.Trinket2_never_CheckedChanged);
            // 
            // Trinket2_name_LB
            // 
            this.Trinket2_name_LB.AutoSize = true;
            this.Trinket2_name_LB.Location = new System.Drawing.Point(7, 25);
            this.Trinket2_name_LB.Name = "Trinket2_name_LB";
            this.Trinket2_name_LB.Size = new System.Drawing.Size(38, 13);
            this.Trinket2_name_LB.TabIndex = 4;
            this.Trinket2_name_LB.Text = "Name:";
            // 
            // Trinket2_CD_LB
            // 
            this.Trinket2_CD_LB.AutoSize = true;
            this.Trinket2_CD_LB.Location = new System.Drawing.Point(6, 70);
            this.Trinket2_CD_LB.Name = "Trinket2_CD_LB";
            this.Trinket2_CD_LB.Size = new System.Drawing.Size(60, 13);
            this.Trinket2_CD_LB.TabIndex = 3;
            this.Trinket2_CD_LB.Text = "Cooldown: ";
            // 
            // Trinket2_ID_LB
            // 
            this.Trinket2_ID_LB.AutoSize = true;
            this.Trinket2_ID_LB.Location = new System.Drawing.Point(7, 47);
            this.Trinket2_ID_LB.Name = "Trinket2_ID_LB";
            this.Trinket2_ID_LB.Size = new System.Drawing.Size(24, 13);
            this.Trinket2_ID_LB.TabIndex = 2;
            this.Trinket2_ID_LB.Text = "ID: ";
            // 
            // groupBox60
            // 
            this.groupBox60.Controls.Add(this.Trinket1_use_when_GB);
            this.groupBox60.Controls.Add(this.Trinket1_name_LB);
            this.groupBox60.Controls.Add(this.Trinket1_CD_LB);
            this.groupBox60.Controls.Add(this.Trinket1_ID_LB);
            this.groupBox60.Location = new System.Drawing.Point(27, 41);
            this.groupBox60.Name = "groupBox60";
            this.groupBox60.Size = new System.Drawing.Size(281, 245);
            this.groupBox60.TabIndex = 0;
            this.groupBox60.TabStop = false;
            this.groupBox60.Text = "Trinket 1";
            // 
            // Trinket1_use_when_GB
            // 
            this.Trinket1_use_when_GB.Controls.Add(this.Trinket1_PVP);
            this.Trinket1_use_when_GB.Controls.Add(this.Trinket1_mana);
            this.Trinket1_use_when_GB.Controls.Add(this.Trinket1_HPS);
            this.Trinket1_use_when_GB.Controls.Add(this.Trinket1_never);
            this.Trinket1_use_when_GB.Location = new System.Drawing.Point(9, 100);
            this.Trinket1_use_when_GB.Name = "Trinket1_use_when_GB";
            this.Trinket1_use_when_GB.Size = new System.Drawing.Size(257, 124);
            this.Trinket1_use_when_GB.TabIndex = 5;
            this.Trinket1_use_when_GB.TabStop = false;
            this.Trinket1_use_when_GB.Text = "Use When";
            // 
            // Trinket1_PVP
            // 
            this.Trinket1_PVP.AutoSize = true;
            this.Trinket1_PVP.Location = new System.Drawing.Point(7, 92);
            this.Trinket1_PVP.Name = "Trinket1_PVP";
            this.Trinket1_PVP.Size = new System.Drawing.Size(134, 17);
            this.Trinket1_PVP.TabIndex = 3;
            this.Trinket1_PVP.TabStop = true;
            this.Trinket1_PVP.Text = "PVP (remove stun ecc)";
            this.Trinket1_PVP.UseVisualStyleBackColor = true;
            this.Trinket1_PVP.CheckedChanged += new System.EventHandler(this.Trinket1_PVP_CheckedChanged);
            // 
            // Trinket1_mana
            // 
            this.Trinket1_mana.AutoSize = true;
            this.Trinket1_mana.Location = new System.Drawing.Point(7, 68);
            this.Trinket1_mana.Name = "Trinket1_mana";
            this.Trinket1_mana.Size = new System.Drawing.Size(102, 17);
            this.Trinket1_mana.TabIndex = 2;
            this.Trinket1_mana.TabStop = true;
            this.Trinket1_mana.Text = "Mana Cooldown";
            this.Trinket1_mana.UseVisualStyleBackColor = true;
            this.Trinket1_mana.CheckedChanged += new System.EventHandler(this.Trinket1_mana_CheckedChanged);
            // 
            // Trinket1_HPS
            // 
            this.Trinket1_HPS.AutoSize = true;
            this.Trinket1_HPS.Location = new System.Drawing.Point(7, 44);
            this.Trinket1_HPS.Name = "Trinket1_HPS";
            this.Trinket1_HPS.Size = new System.Drawing.Size(111, 17);
            this.Trinket1_HPS.TabIndex = 1;
            this.Trinket1_HPS.TabStop = true;
            this.Trinket1_HPS.Text = "Healing Cooldown";
            this.Trinket1_HPS.UseVisualStyleBackColor = true;
            this.Trinket1_HPS.CheckedChanged += new System.EventHandler(this.Trinket1_HPS_CheckedChanged);
            // 
            // Trinket1_never
            // 
            this.Trinket1_never.AutoSize = true;
            this.Trinket1_never.Location = new System.Drawing.Point(7, 20);
            this.Trinket1_never.Name = "Trinket1_never";
            this.Trinket1_never.Size = new System.Drawing.Size(54, 17);
            this.Trinket1_never.TabIndex = 0;
            this.Trinket1_never.TabStop = true;
            this.Trinket1_never.Text = "Never";
            this.Trinket1_never.UseVisualStyleBackColor = true;
            this.Trinket1_never.CheckedChanged += new System.EventHandler(this.Trinket1_never_CheckedChanged);
            // 
            // Trinket1_name_LB
            // 
            this.Trinket1_name_LB.AutoSize = true;
            this.Trinket1_name_LB.Location = new System.Drawing.Point(7, 25);
            this.Trinket1_name_LB.Name = "Trinket1_name_LB";
            this.Trinket1_name_LB.Size = new System.Drawing.Size(38, 13);
            this.Trinket1_name_LB.TabIndex = 4;
            this.Trinket1_name_LB.Text = "Name:";
            // 
            // Trinket1_CD_LB
            // 
            this.Trinket1_CD_LB.AutoSize = true;
            this.Trinket1_CD_LB.Location = new System.Drawing.Point(6, 70);
            this.Trinket1_CD_LB.Name = "Trinket1_CD_LB";
            this.Trinket1_CD_LB.Size = new System.Drawing.Size(60, 13);
            this.Trinket1_CD_LB.TabIndex = 3;
            this.Trinket1_CD_LB.Text = "Cooldown: ";
            // 
            // Trinket1_ID_LB
            // 
            this.Trinket1_ID_LB.AutoSize = true;
            this.Trinket1_ID_LB.Location = new System.Drawing.Point(7, 47);
            this.Trinket1_ID_LB.Name = "Trinket1_ID_LB";
            this.Trinket1_ID_LB.Size = new System.Drawing.Size(24, 13);
            this.Trinket1_ID_LB.TabIndex = 2;
            this.Trinket1_ID_LB.Text = "ID: ";
            // 
            // Trnket1_name_load
            // 
            this.Trnket1_name_load.Location = new System.Drawing.Point(847, 361);
            this.Trnket1_name_load.Name = "Trnket1_name_load";
            this.Trnket1_name_load.Size = new System.Drawing.Size(75, 23);
            this.Trnket1_name_load.TabIndex = 0;
            this.Trnket1_name_load.Text = "Load";
            this.Trnket1_name_load.UseVisualStyleBackColor = true;
            this.Trnket1_name_load.Click += new System.EventHandler(this.Trnket1_name_load_Click);
            // 
            // UPaHCCBTConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 737);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.save);
            this.Name = "UPaHCCBTConfigForm";
            this.Text = "UPaHCCBTConfigForm";
            this.Load += new System.EventHandler(this.UPaHCCBTConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage14.ResumeLayout(false);
            this.tabPage14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.General_precasting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.groupBox64.ResumeLayout(false);
            this.groupBox64.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_exorcism_min_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_holy_wrath_min_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_consecration_min_mana)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.Solo_optimizeGB.ResumeLayout(false);
            this.Solo_optimizeGB.PerformLayout();
            this.Solo_bless_selection.ResumeLayout(false);
            this.Solo_bless_selection.PerformLayout();
            this.Solo_tankselection.ResumeLayout(false);
            this.Solo_tankselection.PerformLayout();
            this.Solo_adancePanel.ResumeLayout(false);
            this.Solo_adancePanel.PerformLayout();
            this.Solo_advanced.ResumeLayout(false);
            this.Solo_advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_max_healing_distance)).EndInit();
            this.Solo_donothealaboveGB.ResumeLayout(false);
            this.Solo_donothealaboveGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_do_not_heal_above)).EndInit();
            this.Solo_CleanseGB.ResumeLayout(false);
            this.Solo_CleanseGB.PerformLayout();
            this.Solo_interruptersGB.ResumeLayout(false);
            this.Solo_interruptersGB.PerformLayout();
            this.Solo_Healing.ResumeLayout(false);
            this.Solo_Healing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_Inf_of_light_DL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_HL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_FoL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_DL_hp)).EndInit();
            this.Solo_mana_management.ResumeLayout(false);
            this.Solo_mana_management.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_use_mana_rec_trinket_every)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_mana_rec_trinket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_Divine_Plea_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_mana_judge)).EndInit();
            this.Solo_HRGB.ResumeLayout(false);
            this.Solo_HRGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_player_inside_HR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_HR_how_much_health)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_HR_how_far)).EndInit();
            this.Solo_emergencyGB.ResumeLayout(false);
            this.Solo_emergencyGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_mana_potion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_LoH_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_HoS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_HoP_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_DS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_DP_hp)).EndInit();
            this.Solo_auraselctGB.ResumeLayout(false);
            this.Solo_auraselctGB.PerformLayout();
            this.Solo_racialsGB.ResumeLayout(false);
            this.Solo_racialsGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_torrent_mana_perc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_stoneform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_gift_hp)).EndInit();
            this.Solo_movementGB.ResumeLayout(false);
            this.Solo_movementGB.PerformLayout();
            this.Solo_generalGB.ResumeLayout(false);
            this.Solo_generalGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_rest_if_mana_below)).EndInit();
            this.Solo_dpsGB.ResumeLayout(false);
            this.Solo_dpsGB.PerformLayout();
            this.Solo_ohshitbuttonGB.ResumeLayout(false);
            this.Solo_ohshitbuttonGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Solo_min_ohshitbutton_activator)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.PVE_optimizeGB.ResumeLayout(false);
            this.PVE_optimizeGB.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_tank_healing_priority_multiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_healing_tank_priority)).EndInit();
            this.PVE_bless_selection.ResumeLayout(false);
            this.PVE_bless_selection.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_stop_DL_if_above)).EndInit();
            this.PVE_tankselection.ResumeLayout(false);
            this.PVE_tankselection.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PVE_advanced.ResumeLayout(false);
            this.PVE_advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_max_healing_distance)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_do_not_heal_above)).EndInit();
            this.PVE_CleanseGB.ResumeLayout(false);
            this.PVE_CleanseGB.PerformLayout();
            this.PVE_interruptersGB.ResumeLayout(false);
            this.PVE_interruptersGB.PerformLayout();
            this.PVE_Healing.ResumeLayout(false);
            this.PVE_Healing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_Inf_of_light_DL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_HL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_FoL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_DL_hp)).EndInit();
            this.PVE_mana_management.ResumeLayout(false);
            this.PVE_mana_management.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_use_mana_rec_trinket_every)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_mana_rec_trinket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_Divine_Plea_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_mana_judge)).EndInit();
            this.PVE_HRGB.ResumeLayout(false);
            this.PVE_HRGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_player_inside_HR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_HR_how_much_health)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_HR_how_far)).EndInit();
            this.PVE_emergencyGB.ResumeLayout(false);
            this.PVE_emergencyGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_mana_potion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_LoH_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_HoS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_HoP_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_DS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_DP_hp)).EndInit();
            this.PVE_auraselctGB.ResumeLayout(false);
            this.PVE_auraselctGB.PerformLayout();
            this.PVE_racialsGB.ResumeLayout(false);
            this.PVE_racialsGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_torrent_mana_perc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_stoneform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_gift_hp)).EndInit();
            this.PVE_movementGB.ResumeLayout(false);
            this.PVE_movementGB.PerformLayout();
            this.PVE_generalGB.ResumeLayout(false);
            this.PVE_generalGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_rest_if_mana_below)).EndInit();
            this.PVE_dpsGB.ResumeLayout(false);
            this.PVE_dpsGB.PerformLayout();
            this.PVE_ohshitbuttonGB.ResumeLayout(false);
            this.PVE_ohshitbuttonGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PVE_min_ohshitbutton_activator)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.Raid_optimizeGB.ResumeLayout(false);
            this.Raid_optimizeGB.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_tank_healing_priority_multiplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_healing_tank_priority)).EndInit();
            this.Raid_bless_selection.ResumeLayout(false);
            this.Raid_bless_selection.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_stop_DL_if_above)).EndInit();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.Raid_advanced.ResumeLayout(false);
            this.Raid_advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_max_healing_distance)).EndInit();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_do_not_heal_above)).EndInit();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_Inf_of_light_DL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_HL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_FoL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_DL_hp)).EndInit();
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_use_mana_rec_trinket_every)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_mana_rec_trinket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_Divine_Plea_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_mana_judge)).EndInit();
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_player_inside_HR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_HR_how_much_health)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_HR_how_far)).EndInit();
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_mana_potion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_LoH_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_HoS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_HoP_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_DS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_DP_hp)).EndInit();
            this.Raid_auraselctGB.ResumeLayout(false);
            this.Raid_auraselctGB.PerformLayout();
            this.groupBox30.ResumeLayout(false);
            this.groupBox30.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_torrent_mana_perc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_stoneform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_gift_hp)).EndInit();
            this.groupBox31.ResumeLayout(false);
            this.groupBox31.PerformLayout();
            this.Raid_generalGB.ResumeLayout(false);
            this.Raid_generalGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_rest_if_mana_below)).EndInit();
            this.groupBox33.ResumeLayout(false);
            this.groupBox33.PerformLayout();
            this.groupBox34.ResumeLayout(false);
            this.groupBox34.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Raid_min_ohshitbutton_activator)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.Battleground_optimizeGB.ResumeLayout(false);
            this.Battleground_optimizeGB.PerformLayout();
            this.Battleground_bless_selection.ResumeLayout(false);
            this.Battleground_bless_selection.PerformLayout();
            this.groupBox32.ResumeLayout(false);
            this.groupBox32.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.Battleground_advanced.ResumeLayout(false);
            this.Battleground_advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_max_healing_distance)).EndInit();
            this.groupBox36.ResumeLayout(false);
            this.groupBox36.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_do_not_heal_above)).EndInit();
            this.groupBox37.ResumeLayout(false);
            this.groupBox37.PerformLayout();
            this.groupBox38.ResumeLayout(false);
            this.groupBox38.PerformLayout();
            this.groupBox39.ResumeLayout(false);
            this.groupBox39.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_Inf_of_light_DL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_HL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_FoL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_DL_hp)).EndInit();
            this.groupBox40.ResumeLayout(false);
            this.groupBox40.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_use_mana_rec_trinket_every)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_mana_rec_trinket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_Divine_Plea_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_mana_judge)).EndInit();
            this.groupBox41.ResumeLayout(false);
            this.groupBox41.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_player_inside_HR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_HR_how_much_health)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_HR_how_far)).EndInit();
            this.groupBox42.ResumeLayout(false);
            this.groupBox42.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_mana_potion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_LoH_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_HoS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_HoP_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_DS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_DP_hp)).EndInit();
            this.Battleground_auraselctGB.ResumeLayout(false);
            this.Battleground_auraselctGB.PerformLayout();
            this.groupBox44.ResumeLayout(false);
            this.groupBox44.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_torrent_mana_perc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_stoneform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_gift_hp)).EndInit();
            this.groupBox45.ResumeLayout(false);
            this.groupBox45.PerformLayout();
            this.Battleground_generalGB.ResumeLayout(false);
            this.Battleground_generalGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_rest_if_mana_below)).EndInit();
            this.groupBox47.ResumeLayout(false);
            this.groupBox47.PerformLayout();
            this.groupBox48.ResumeLayout(false);
            this.groupBox48.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Battleground_min_ohshitbutton_activator)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.ARENA_optimizeGB.ResumeLayout(false);
            this.ARENA_optimizeGB.PerformLayout();
            this.ARENA_bless_selection.ResumeLayout(false);
            this.ARENA_bless_selection.PerformLayout();
            this.ARENA_special_logics.ResumeLayout(false);
            this.ARENA_special_logics.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ARENA_advanced.ResumeLayout(false);
            this.ARENA_advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_max_healing_distance)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_do_not_heal_above)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_Inf_of_light_DL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_HL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_FoL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_DL_hp)).EndInit();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_use_mana_rec_trinket_every)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_mana_rec_trinket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_Divine_Plea_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_mana_judge)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_player_inside_HR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_HR_how_much_health)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_HR_how_far)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_mana_potion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_LoH_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_HoS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_HoP_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_DS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_DP_hp)).EndInit();
            this.ARENA_auraselctGB.ResumeLayout(false);
            this.ARENA_auraselctGB.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_torrent_mana_perc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_stoneform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_gift_hp)).EndInit();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.Arena_generalGB.ResumeLayout(false);
            this.Arena_generalGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_rest_if_mana_below)).EndInit();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ARENA_min_ohshitbutton_activator)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.WorldPVP_optimizeGB.ResumeLayout(false);
            this.WorldPVP_optimizeGB.PerformLayout();
            this.WorldPVP_bless_selection.ResumeLayout(false);
            this.WorldPVP_bless_selection.PerformLayout();
            this.groupBox46.ResumeLayout(false);
            this.groupBox46.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.WorldPVP_advanced.ResumeLayout(false);
            this.WorldPVP_advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_max_healing_distance)).EndInit();
            this.groupBox50.ResumeLayout(false);
            this.groupBox50.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_do_not_heal_above)).EndInit();
            this.groupBox51.ResumeLayout(false);
            this.groupBox51.PerformLayout();
            this.groupBox52.ResumeLayout(false);
            this.groupBox52.PerformLayout();
            this.groupBox53.ResumeLayout(false);
            this.groupBox53.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_Inf_of_light_DL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_HL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_FoL_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_DL_hp)).EndInit();
            this.groupBox54.ResumeLayout(false);
            this.groupBox54.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_use_mana_rec_trinket_every)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_mana_rec_trinket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_Divine_Plea_mana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_mana_judge)).EndInit();
            this.groupBox55.ResumeLayout(false);
            this.groupBox55.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_player_inside_HR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_HR_how_much_health)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_HR_how_far)).EndInit();
            this.groupBox56.ResumeLayout(false);
            this.groupBox56.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_mana_potion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_LoH_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_HoS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_HoP_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_DS_hp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_DP_hp)).EndInit();
            this.WorldPVP_auraselctGB.ResumeLayout(false);
            this.WorldPVP_auraselctGB.PerformLayout();
            this.groupBox58.ResumeLayout(false);
            this.groupBox58.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_torrent_mana_perc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_stoneform)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_gift_hp)).EndInit();
            this.groupBox59.ResumeLayout(false);
            this.groupBox59.PerformLayout();
            this.WorldPVP_generalGB.ResumeLayout(false);
            this.WorldPVP_generalGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_rest_if_mana_below)).EndInit();
            this.groupBox61.ResumeLayout(false);
            this.groupBox61.PerformLayout();
            this.groupBox62.ResumeLayout(false);
            this.groupBox62.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPVP_min_ohshitbutton_activator)).EndInit();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.SH_GB8.ResumeLayout(false);
            this.SH_GB8.PerformLayout();
            this.SH_GB7.ResumeLayout(false);
            this.SH_GB7.PerformLayout();
            this.SH_GB6.ResumeLayout(false);
            this.SH_GB6.PerformLayout();
            this.SE_GB5.ResumeLayout(false);
            this.SE_GB5.PerformLayout();
            this.SE_GB4.ResumeLayout(false);
            this.SE_GB4.PerformLayout();
            this.SE_GB3.ResumeLayout(false);
            this.SE_GB3.PerformLayout();
            this.SE_GB2.ResumeLayout(false);
            this.SE_GB2.PerformLayout();
            this.SH_GB1.ResumeLayout(false);
            this.SH_GB1.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.groupBox35.ResumeLayout(false);
            this.groupBox35.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.tabPage12.ResumeLayout(false);
            this.groupBox49.ResumeLayout(false);
            this.groupBox49.PerformLayout();
            this.groupBox57.ResumeLayout(false);
            this.groupBox57.PerformLayout();
            this.tabPage13.ResumeLayout(false);
            this.groupBox63.ResumeLayout(false);
            this.groupBox63.PerformLayout();
            this.Trinket2_use_when_GB.ResumeLayout(false);
            this.Trinket2_use_when_GB.PerformLayout();
            this.groupBox60.ResumeLayout(false);
            this.groupBox60.PerformLayout();
            this.Trinket1_use_when_GB.ResumeLayout(false);
            this.Trinket1_use_when_GB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox Solo_wanna_crusader;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.CheckBox SelectHeal;
        private System.Windows.Forms.CheckBox RaidCK0;
        private System.Windows.Forms.CheckBox RaidCK1;
        private System.Windows.Forms.Label RaidL0;
        private System.Windows.Forms.CheckBox RaidCK24;
        private System.Windows.Forms.CheckBox RaidCK23;
        private System.Windows.Forms.CheckBox RaidCK22;
        private System.Windows.Forms.CheckBox RaidCK21;
        private System.Windows.Forms.CheckBox RaidCK20;
        private System.Windows.Forms.CheckBox RaidCK19;
        private System.Windows.Forms.CheckBox RaidCK18;
        private System.Windows.Forms.CheckBox RaidCK17;
        private System.Windows.Forms.CheckBox RaidCK16;
        private System.Windows.Forms.CheckBox RaidCK15;
        private System.Windows.Forms.CheckBox RaidCK14;
        private System.Windows.Forms.CheckBox RaidCK13;
        private System.Windows.Forms.CheckBox RaidCK12;
        private System.Windows.Forms.CheckBox RaidCK11;
        private System.Windows.Forms.CheckBox RaidCK10;
        private System.Windows.Forms.CheckBox RaidCK9;
        private System.Windows.Forms.CheckBox RaidCK8;
        private System.Windows.Forms.CheckBox RaidCK7;
        private System.Windows.Forms.CheckBox RaidCK6;
        private System.Windows.Forms.CheckBox RaidCK5;
        private System.Windows.Forms.CheckBox RaidCK4;
        private System.Windows.Forms.CheckBox RaidCK3;
        private System.Windows.Forms.CheckBox RaidCK2;
        private System.Windows.Forms.Label RaidL24;
        private System.Windows.Forms.Label RaidL23;
        private System.Windows.Forms.Label RaidL22;
        private System.Windows.Forms.Label RaidL21;
        private System.Windows.Forms.Label RaidL20;
        private System.Windows.Forms.Label RaidL19;
        private System.Windows.Forms.Label RaidL18;
        private System.Windows.Forms.Label RaidL17;
        private System.Windows.Forms.Label RaidL16;
        private System.Windows.Forms.Label RaidL15;
        private System.Windows.Forms.Label RaidL14;
        private System.Windows.Forms.Label RaidL13;
        private System.Windows.Forms.Label RaidL12;
        private System.Windows.Forms.Label RaidL11;
        private System.Windows.Forms.Label RaidL10;
        private System.Windows.Forms.Label RaidL9;
        private System.Windows.Forms.Label RaidL8;
        private System.Windows.Forms.Label RaidL7;
        private System.Windows.Forms.Label RaidL6;
        private System.Windows.Forms.Label RaidL5;
        private System.Windows.Forms.Label RaidL4;
        private System.Windows.Forms.Label RaidL3;
        private System.Windows.Forms.Label RaidL2;
        private System.Windows.Forms.Label RaidL1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox Solo_get_tank_from_focus;
        private System.Windows.Forms.CheckBox RAFtankfromfocus;
        private System.Windows.Forms.CheckBox ARENA2v2tankfromfocus;
        private System.Windows.Forms.CheckBox Raid_ignore_beacon;
        private System.Windows.Forms.CheckBox Solo_Inf_of_light_wanna_DL;
        private System.Windows.Forms.CheckBox Solo_wanna_AW;
        private System.Windows.Forms.CheckBox Solo_wanna_buff;
        private System.Windows.Forms.CheckBox Solo_wanna_cleanse;
        private System.Windows.Forms.CheckBox Solo_wanna_CS;
        private System.Windows.Forms.CheckBox Solo_wanna_DF;
        private System.Windows.Forms.CheckBox Solo_wanna_DP;
        private System.Windows.Forms.CheckBox Solo_wanna_DS;
        private System.Windows.Forms.CheckBox Solo_wanna_everymanforhimself;
        private System.Windows.Forms.CheckBox Solo_wanna_face;
        private System.Windows.Forms.GroupBox Solo_generalGB;
        private System.Windows.Forms.GroupBox Solo_racialsGB;
        private System.Windows.Forms.CheckBox Solo_wanna_gift;
        private System.Windows.Forms.GroupBox Solo_dpsGB;
        private System.Windows.Forms.GroupBox Solo_ohshitbuttonGB;
        private System.Windows.Forms.CheckBox Solo_wanna_GotAK;
        private System.Windows.Forms.CheckBox Solo_wanna_HoJ;
        private System.Windows.Forms.CheckBox Solo_wanna_HoP;
        private System.Windows.Forms.CheckBox Solo_wanna_HoS;
        private System.Windows.Forms.CheckBox Solo_wanna_HoW;
        private System.Windows.Forms.CheckBox Solo_wanna_HR;
        private System.Windows.Forms.GroupBox Solo_emergencyGB;
        private System.Windows.Forms.CheckBox Solo_wanna_Judge;
        private System.Windows.Forms.CheckBox Solo_wanna_LoH;
        private System.Windows.Forms.CheckBox Solo_wanna_mana_potion;
        private System.Windows.Forms.CheckBox Solo_wanna_mount;
        private System.Windows.Forms.GroupBox Solo_movementGB;
        private System.Windows.Forms.CheckBox Solo_wanna_move_to_heal;
        private System.Windows.Forms.CheckBox Solo_wanna_move_to_HoJ;
        private System.Windows.Forms.CheckBox Solo_wanna_rebuke;
        private System.Windows.Forms.CheckBox Solo_wanna_stoneform;
        private System.Windows.Forms.CheckBox Solo_wanna_target;
        private System.Windows.Forms.CheckBox Solo_wanna_torrent;
        private System.Windows.Forms.CheckBox Solo_wanna_urgent_cleanse;
        private System.Windows.Forms.GroupBox Solo_auraselctGB;
        private System.Windows.Forms.RadioButton Solo_DisabledRB;
        private System.Windows.Forms.RadioButton Solo_resistanceRB;
        private System.Windows.Forms.RadioButton Solo_concentrationRB;
        private System.Windows.Forms.NumericUpDown Solo_do_not_heal_above;
        private System.Windows.Forms.GroupBox Solo_donothealaboveGB;
        private System.Windows.Forms.Label Solo_donothealaboveLB;
        private System.Windows.Forms.GroupBox Solo_HRGB;
        private System.Windows.Forms.CheckBox Solo_advanced_option;
        private System.Windows.Forms.GroupBox Solo_advanced;
        private System.Windows.Forms.Label Solo_HR_how_farLB;
        private System.Windows.Forms.NumericUpDown Solo_HR_how_far;
        private System.Windows.Forms.Label Solo_HR_how_much_healthLB;
        private System.Windows.Forms.NumericUpDown Solo_HR_how_much_health;
        private System.Windows.Forms.GroupBox Solo_mana_management;
        private System.Windows.Forms.Label Solo_mana_judgeLB;
        private System.Windows.Forms.NumericUpDown Solo_mana_judge;
        private System.Windows.Forms.Label Solo_max_healing_distanceLB;
        private System.Windows.Forms.NumericUpDown Solo_max_healing_distance;
        private System.Windows.Forms.Label Solo_min_Divine_Plea_manaLB;
        private System.Windows.Forms.NumericUpDown Solo_min_Divine_Plea_mana;
        private System.Windows.Forms.GroupBox Solo_Healing;
        private System.Windows.Forms.Label Solo_min_DL_hpLB;
        private System.Windows.Forms.NumericUpDown Solo_min_DL_hp;
        private System.Windows.Forms.Label Solo_emergency_buttonLB;
        private System.Windows.Forms.NumericUpDown Solo_min_DP_hp;
        private System.Windows.Forms.NumericUpDown Solo_min_DS_hp;
        private System.Windows.Forms.Label Solo_min_FoL_hpLB;
        private System.Windows.Forms.NumericUpDown Solo_min_FoL_hp;
        private System.Windows.Forms.NumericUpDown Solo_min_gift_hp;
        private System.Windows.Forms.Label Solo_min_HL_hpLB;
        private System.Windows.Forms.NumericUpDown Solo_min_HL_hp;
        private System.Windows.Forms.NumericUpDown Solo_min_HoP_hp;
        private System.Windows.Forms.NumericUpDown Solo_min_HoS_hp;
        private System.Windows.Forms.Label Solo_min_Inf_of_light_DL_hpLB;
        private System.Windows.Forms.NumericUpDown Solo_min_Inf_of_light_DL_hp;
        private System.Windows.Forms.NumericUpDown Solo_min_LoH_hp;
        private System.Windows.Forms.NumericUpDown Solo_min_mana_potion;
        private System.Windows.Forms.Label Solo_min_mana_rec_trinketLB;
        private System.Windows.Forms.NumericUpDown Solo_min_mana_rec_trinket;
        private System.Windows.Forms.Label Solo_min_ohshitbutton_activatorLB;
        private System.Windows.Forms.NumericUpDown Solo_min_ohshitbutton_activator;
        private System.Windows.Forms.Label Solo_min_player_inside_HRLB;
        private System.Windows.Forms.NumericUpDown Solo_min_player_inside_HR;
        private System.Windows.Forms.NumericUpDown Solo_min_stoneform;
        private System.Windows.Forms.NumericUpDown Solo_min_torrent_mana_perc;
        private System.Windows.Forms.Label Solo_rest_if_mana_belowLB;
        private System.Windows.Forms.NumericUpDown Solo_rest_if_mana_below;
        private System.Windows.Forms.Label Solo_use_mana_rec_trinket_everyLB;
        private System.Windows.Forms.NumericUpDown Solo_use_mana_rec_trinket_every;
        private System.Windows.Forms.CheckBox Solo_wanna_move;
        private System.Windows.Forms.CheckBox Solo_wanna_lifeblood;
        private System.Windows.Forms.CheckBox Solo_do_not_dismount_ooc;
        private System.Windows.Forms.CheckBox Solo_do_not_dismount_EVER;
        private System.Windows.Forms.GroupBox Solo_CleanseGB;
        private System.Windows.Forms.GroupBox Solo_interruptersGB;
        private System.Windows.Forms.Panel Solo_adancePanel;
        //private System.Windows.Forms.ToolTip BuffTT;
        //private System.Windows.Forms.ToolTip Solo_TantfocusTT;
        private System.Windows.Forms.GroupBox Solo_tankselection;
        private System.Windows.Forms.CheckBox Solo_get_tank_from_lua;
        private System.Windows.Forms.GroupBox SE_GB5;
        private System.Windows.Forms.GroupBox SE_GB4;
        private System.Windows.Forms.GroupBox SE_GB3;
        private System.Windows.Forms.GroupBox SE_GB2;
        private System.Windows.Forms.GroupBox SH_GB1;
        private System.Windows.Forms.GroupBox SH_GB6;
        private System.Windows.Forms.Label RaidL25;
        private System.Windows.Forms.Label RaidL26;
        private System.Windows.Forms.Label RaidL27;
        private System.Windows.Forms.Label RaidL28;
        private System.Windows.Forms.Label RaidL29;
        private System.Windows.Forms.CheckBox RaidCK25;
        private System.Windows.Forms.CheckBox RaidCK29;
        private System.Windows.Forms.CheckBox RaidCK26;
        private System.Windows.Forms.CheckBox RaidCK28;
        private System.Windows.Forms.CheckBox RaidCK27;
        private System.Windows.Forms.GroupBox SH_GB8;
        private System.Windows.Forms.Label RaidL35;
        private System.Windows.Forms.Label RaidL36;
        private System.Windows.Forms.Label RaidL37;
        private System.Windows.Forms.Label RaidL38;
        private System.Windows.Forms.Label RaidL39;
        private System.Windows.Forms.CheckBox RaidCK35;
        private System.Windows.Forms.CheckBox RaidCK39;
        private System.Windows.Forms.CheckBox RaidCK36;
        private System.Windows.Forms.CheckBox RaidCK38;
        private System.Windows.Forms.CheckBox RaidCK37;
        private System.Windows.Forms.GroupBox SH_GB7;
        private System.Windows.Forms.Label RaidL30;
        private System.Windows.Forms.Label RaidL31;
        private System.Windows.Forms.Label RaidL32;
        private System.Windows.Forms.Label RaidL33;
        private System.Windows.Forms.Label RaidL34;
        private System.Windows.Forms.CheckBox RaidCK30;
        private System.Windows.Forms.CheckBox RaidCK34;
        private System.Windows.Forms.CheckBox RaidCK31;
        private System.Windows.Forms.CheckBox RaidCK33;
        private System.Windows.Forms.CheckBox RaidCK32;
        private System.Windows.Forms.GroupBox PVE_tankselection;
        private System.Windows.Forms.CheckBox PVE_get_tank_from_lua;
        private System.Windows.Forms.CheckBox PVE_get_tank_from_focus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox PVE_advanced_option;
        private System.Windows.Forms.GroupBox PVE_advanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown PVE_max_healing_distance;
        private System.Windows.Forms.CheckBox PVE_wanna_target;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown PVE_do_not_heal_above;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox PVE_wanna_face;
        private System.Windows.Forms.GroupBox PVE_CleanseGB;
        private System.Windows.Forms.CheckBox PVE_wanna_cleanse;
        private System.Windows.Forms.CheckBox PVE_wanna_urgent_cleanse;
        private System.Windows.Forms.GroupBox PVE_interruptersGB;
        private System.Windows.Forms.CheckBox PVE_wanna_HoJ;
        private System.Windows.Forms.CheckBox PVE_wanna_rebuke;
        private System.Windows.Forms.GroupBox PVE_Healing;
        private System.Windows.Forms.Label PVE_min_Inf_of_light_DL_hpLB;
        private System.Windows.Forms.NumericUpDown PVE_min_Inf_of_light_DL_hp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown PVE_min_HL_hp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown PVE_min_FoL_hp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown PVE_min_DL_hp;
        private System.Windows.Forms.CheckBox PVE_Inf_of_light_wanna_DL;
        private System.Windows.Forms.GroupBox PVE_mana_management;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown PVE_use_mana_rec_trinket_every;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown PVE_min_mana_rec_trinket;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown PVE_min_Divine_Plea_mana;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown PVE_mana_judge;
        private System.Windows.Forms.GroupBox PVE_HRGB;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown PVE_min_player_inside_HR;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown PVE_HR_how_much_health;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown PVE_HR_how_far;
        private System.Windows.Forms.CheckBox PVE_wanna_HR;
        private System.Windows.Forms.GroupBox PVE_emergencyGB;
        private System.Windows.Forms.NumericUpDown PVE_min_mana_potion;
        private System.Windows.Forms.NumericUpDown PVE_min_LoH_hp;
        private System.Windows.Forms.NumericUpDown PVE_min_HoS_hp;
        private System.Windows.Forms.NumericUpDown PVE_min_HoP_hp;
        private System.Windows.Forms.NumericUpDown PVE_min_DS_hp;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown PVE_min_DP_hp;
        private System.Windows.Forms.CheckBox PVE_wanna_mana_potion;
        private System.Windows.Forms.CheckBox PVE_wanna_LoH;
        private System.Windows.Forms.CheckBox PVE_wanna_DP;
        private System.Windows.Forms.CheckBox PVE_wanna_DS;
        private System.Windows.Forms.CheckBox PVE_wanna_HoS;
        private System.Windows.Forms.CheckBox PVE_wanna_HoP;
        private System.Windows.Forms.GroupBox PVE_auraselctGB;
        private System.Windows.Forms.RadioButton PVE_DisabledRB;
        private System.Windows.Forms.RadioButton PVE_resistanceRB;
        private System.Windows.Forms.RadioButton PVE_concentrationRB;
        private System.Windows.Forms.GroupBox PVE_racialsGB;
        private System.Windows.Forms.NumericUpDown PVE_min_torrent_mana_perc;
        private System.Windows.Forms.NumericUpDown PVE_min_stoneform;
        private System.Windows.Forms.NumericUpDown PVE_min_gift_hp;
        private System.Windows.Forms.CheckBox PVE_wanna_torrent;
        private System.Windows.Forms.CheckBox PVE_wanna_stoneform;
        private System.Windows.Forms.CheckBox PVE_wanna_everymanforhimself;
        private System.Windows.Forms.CheckBox PVE_wanna_gift;
        private System.Windows.Forms.GroupBox PVE_movementGB;
        private System.Windows.Forms.CheckBox PVE_do_not_dismount_EVER;
        private System.Windows.Forms.CheckBox PVE_do_not_dismount_ooc;
        private System.Windows.Forms.CheckBox PVE_wanna_move_to_HoJ;
        private System.Windows.Forms.CheckBox PVE_wanna_mount;
        private System.Windows.Forms.CheckBox PVE_wanna_move_to_heal;
        private System.Windows.Forms.CheckBox PVE_wanna_crusader;
        private System.Windows.Forms.GroupBox PVE_generalGB;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown PVE_rest_if_mana_below;
        private System.Windows.Forms.CheckBox PVE_wanna_buff;
        private System.Windows.Forms.GroupBox PVE_dpsGB;
        private System.Windows.Forms.CheckBox PVE_wanna_Judge;
        private System.Windows.Forms.CheckBox PVE_wanna_CS;
        private System.Windows.Forms.CheckBox PVE_wanna_HoW;
        private System.Windows.Forms.GroupBox PVE_ohshitbuttonGB;
        private System.Windows.Forms.CheckBox PVE_wanna_lifeblood;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown PVE_min_ohshitbutton_activator;
        private System.Windows.Forms.CheckBox PVE_wanna_GotAK;
        private System.Windows.Forms.CheckBox PVE_wanna_AW;
        private System.Windows.Forms.CheckBox PVE_wanna_DF;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown PVE_stop_DL_if_above;
        private System.Windows.Forms.GroupBox Solo_bless_selection;
        private System.Windows.Forms.GroupBox Solo_overhealing_protection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox ARENA_get_tank_from_lua;
        private System.Windows.Forms.CheckBox ARENA_get_tank_from_focus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox ARENA_advanced_option;
        private System.Windows.Forms.GroupBox ARENA_advanced;
        private System.Windows.Forms.CheckBox ARENA_wanna_taunt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ARENA_max_healing_distance;
        private System.Windows.Forms.CheckBox ARENA_wanna_target;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.NumericUpDown ARENA_do_not_heal_above;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox ARENA_wanna_face;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox ARENA_wanna_cleanse;
        private System.Windows.Forms.CheckBox ARENA_wanna_urgent_cleanse;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.CheckBox ARENA_wanna_HoJ;
        private System.Windows.Forms.CheckBox ARENA_wanna_rebuke;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label ARENA_min_Inf_of_light_DL_hpLB;
        private System.Windows.Forms.NumericUpDown ARENA_min_Inf_of_light_DL_hp;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown ARENA_min_HL_hp;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown ARENA_min_FoL_hp;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.NumericUpDown ARENA_min_DL_hp;
        private System.Windows.Forms.CheckBox ARENA_Inf_of_light_wanna_DL;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown ARENA_use_mana_rec_trinket_every;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.NumericUpDown ARENA_min_mana_rec_trinket;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.NumericUpDown ARENA_min_Divine_Plea_mana;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.NumericUpDown ARENA_mana_judge;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.NumericUpDown ARENA_min_player_inside_HR;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown ARENA_HR_how_much_health;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.NumericUpDown ARENA_HR_how_far;
        private System.Windows.Forms.CheckBox ARENA_wanna_HR;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.NumericUpDown ARENA_min_mana_potion;
        private System.Windows.Forms.NumericUpDown ARENA_min_LoH_hp;
        private System.Windows.Forms.NumericUpDown ARENA_min_HoS_hp;
        private System.Windows.Forms.NumericUpDown ARENA_min_HoP_hp;
        private System.Windows.Forms.NumericUpDown ARENA_min_DS_hp;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.NumericUpDown ARENA_min_DP_hp;
        private System.Windows.Forms.CheckBox ARENA_wanna_mana_potion;
        private System.Windows.Forms.CheckBox ARENA_wanna_LoH;
        private System.Windows.Forms.CheckBox ARENA_wanna_DP;
        private System.Windows.Forms.CheckBox ARENA_wanna_DS;
        private System.Windows.Forms.CheckBox ARENA_wanna_HoS;
        private System.Windows.Forms.CheckBox ARENA_wanna_HoP;
        private System.Windows.Forms.GroupBox ARENA_auraselctGB;
        private System.Windows.Forms.RadioButton ARENA_DisabledRB;
        private System.Windows.Forms.RadioButton ARENA_resistanceRB;
        private System.Windows.Forms.RadioButton ARENA_concentrationRB;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.NumericUpDown ARENA_min_torrent_mana_perc;
        private System.Windows.Forms.NumericUpDown ARENA_min_stoneform;
        private System.Windows.Forms.NumericUpDown ARENA_min_gift_hp;
        private System.Windows.Forms.CheckBox ARENA_wanna_torrent;
        private System.Windows.Forms.CheckBox ARENA_wanna_stoneform;
        private System.Windows.Forms.CheckBox ARENA_wanna_everymanforhimself;
        private System.Windows.Forms.CheckBox ARENA_wanna_gift;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.CheckBox ARENA_do_not_dismount_EVER;
        private System.Windows.Forms.CheckBox ARENA_do_not_dismount_ooc;
        private System.Windows.Forms.CheckBox ARENA_wanna_move_to_HoJ;
        private System.Windows.Forms.CheckBox ARENA_wanna_mount;
        private System.Windows.Forms.CheckBox ARENA_wanna_move_to_heal;
        private System.Windows.Forms.CheckBox ARENA_wanna_crusader;
        private System.Windows.Forms.GroupBox Arena_generalGB;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.NumericUpDown ARENA_rest_if_mana_below;
        private System.Windows.Forms.CheckBox ARENA_wanna_buff;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.CheckBox ARENA_wanna_Judge;
        private System.Windows.Forms.CheckBox ARENA_wanna_CS;
        private System.Windows.Forms.CheckBox ARENA_wanna_HoW;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.CheckBox ARENA_wanna_lifeblood;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.NumericUpDown ARENA_min_ohshitbutton_activator;
        private System.Windows.Forms.CheckBox ARENA_wanna_GotAK;
        private System.Windows.Forms.CheckBox ARENA_wanna_AW;
        private System.Windows.Forms.CheckBox ARENA_wanna_DF;
        private System.Windows.Forms.GroupBox ARENA_special_logics;
        private System.Windows.Forms.CheckBox ARENA_wanna_HoF;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown Raid_stop_DL_if_above;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.CheckBox Raid_get_tank_from_lua;
        private System.Windows.Forms.CheckBox Raid_get_tank_from_focus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox Raid_advanced_option;
        private System.Windows.Forms.GroupBox Raid_advanced;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown Raid_max_healing_distance;
        private System.Windows.Forms.CheckBox Raid_wanna_target;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.NumericUpDown Raid_do_not_heal_above;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.CheckBox Raid_wanna_face;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.CheckBox Raid_wanna_cleanse;
        private System.Windows.Forms.CheckBox Raid_wanna_urgent_cleanse;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.CheckBox Raid_wanna_HoJ;
        private System.Windows.Forms.CheckBox Raid_wanna_rebuke;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.Label Raid_min_Inf_of_light_DL_hpLB;
        private System.Windows.Forms.NumericUpDown Raid_min_Inf_of_light_DL_hp;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.NumericUpDown Raid_min_HL_hp;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.NumericUpDown Raid_min_FoL_hp;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.NumericUpDown Raid_min_DL_hp;
        private System.Windows.Forms.CheckBox Raid_Inf_of_light_wanna_DL;
        private System.Windows.Forms.GroupBox groupBox26;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.NumericUpDown Raid_use_mana_rec_trinket_every;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.NumericUpDown Raid_min_mana_rec_trinket;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.NumericUpDown Raid_min_Divine_Plea_mana;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.NumericUpDown Raid_mana_judge;
        private System.Windows.Forms.GroupBox groupBox27;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.NumericUpDown Raid_min_player_inside_HR;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.NumericUpDown Raid_HR_how_much_health;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.NumericUpDown Raid_HR_how_far;
        private System.Windows.Forms.CheckBox Raid_wanna_HR;
        private System.Windows.Forms.GroupBox groupBox28;
        private System.Windows.Forms.NumericUpDown Raid_min_mana_potion;
        private System.Windows.Forms.NumericUpDown Raid_min_LoH_hp;
        private System.Windows.Forms.NumericUpDown Raid_min_HoS_hp;
        private System.Windows.Forms.NumericUpDown Raid_min_HoP_hp;
        private System.Windows.Forms.NumericUpDown Raid_min_DS_hp;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.NumericUpDown Raid_min_DP_hp;
        private System.Windows.Forms.CheckBox Raid_wanna_mana_potion;
        private System.Windows.Forms.CheckBox Raid_wanna_LoH;
        private System.Windows.Forms.CheckBox Raid_wanna_DP;
        private System.Windows.Forms.CheckBox Raid_wanna_DS;
        private System.Windows.Forms.CheckBox Raid_wanna_HoS;
        private System.Windows.Forms.CheckBox Raid_wanna_HoP;
        private System.Windows.Forms.GroupBox Raid_auraselctGB;
        private System.Windows.Forms.RadioButton Raid_DisabledRB;
        private System.Windows.Forms.RadioButton Raid_resistanceRB;
        private System.Windows.Forms.RadioButton Raid_concentrationRB;
        private System.Windows.Forms.GroupBox groupBox30;
        private System.Windows.Forms.NumericUpDown Raid_min_torrent_mana_perc;
        private System.Windows.Forms.NumericUpDown Raid_min_stoneform;
        private System.Windows.Forms.NumericUpDown Raid_min_gift_hp;
        private System.Windows.Forms.CheckBox Raid_wanna_torrent;
        private System.Windows.Forms.CheckBox Raid_wanna_stoneform;
        private System.Windows.Forms.CheckBox Raid_wanna_everymanforhimself;
        private System.Windows.Forms.CheckBox Raid_wanna_gift;
        private System.Windows.Forms.GroupBox groupBox31;
        private System.Windows.Forms.CheckBox Raid_do_not_dismount_EVER;
        private System.Windows.Forms.CheckBox Raid_do_not_dismount_ooc;
        private System.Windows.Forms.CheckBox Raid_wanna_move_to_HoJ;
        private System.Windows.Forms.CheckBox Raid_wanna_mount;
        private System.Windows.Forms.CheckBox Raid_wanna_move_to_heal;
        private System.Windows.Forms.CheckBox Raid_wanna_crusader;
        private System.Windows.Forms.GroupBox Raid_generalGB;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.NumericUpDown Raid_rest_if_mana_below;
        private System.Windows.Forms.CheckBox Raid_wanna_buff;
        private System.Windows.Forms.GroupBox groupBox33;
        private System.Windows.Forms.CheckBox Raid_wanna_Judge;
        private System.Windows.Forms.CheckBox Raid_wanna_CS;
        private System.Windows.Forms.CheckBox Raid_wanna_HoW;
        private System.Windows.Forms.GroupBox groupBox34;
        private System.Windows.Forms.CheckBox Raid_wanna_lifeblood;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.NumericUpDown Raid_min_ohshitbutton_activator;
        private System.Windows.Forms.CheckBox Raid_wanna_GotAK;
        private System.Windows.Forms.CheckBox Raid_wanna_AW;
        private System.Windows.Forms.CheckBox Raid_wanna_DF;
        private System.Windows.Forms.GroupBox groupBox29;
        private System.Windows.Forms.GroupBox groupBox32;
        private System.Windows.Forms.CheckBox Battleground_get_tank_from_lua;
        private System.Windows.Forms.CheckBox Battleground_get_tank_from_focus;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox Battleground_advanced_option;
        private System.Windows.Forms.GroupBox Battleground_advanced;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.NumericUpDown Battleground_max_healing_distance;
        private System.Windows.Forms.CheckBox Battleground_wanna_target;
        private System.Windows.Forms.GroupBox groupBox36;
        private System.Windows.Forms.NumericUpDown Battleground_do_not_heal_above;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.CheckBox Battleground_wanna_face;
        private System.Windows.Forms.GroupBox groupBox37;
        private System.Windows.Forms.CheckBox Battleground_wanna_cleanse;
        private System.Windows.Forms.CheckBox Battleground_wanna_urgent_cleanse;
        private System.Windows.Forms.GroupBox groupBox38;
        private System.Windows.Forms.CheckBox Battleground_wanna_HoJ;
        private System.Windows.Forms.CheckBox Battleground_wanna_rebuke;
        private System.Windows.Forms.GroupBox groupBox39;
        private System.Windows.Forms.Label Battleground_min_Inf_of_light_DL_hpLB;
        private System.Windows.Forms.NumericUpDown Battleground_min_Inf_of_light_DL_hp;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.NumericUpDown Battleground_min_HL_hp;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.NumericUpDown Battleground_min_FoL_hp;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.NumericUpDown Battleground_min_DL_hp;
        private System.Windows.Forms.CheckBox Battleground_Inf_of_light_wanna_DL;
        private System.Windows.Forms.GroupBox groupBox40;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.NumericUpDown Battleground_use_mana_rec_trinket_every;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.NumericUpDown Battleground_min_mana_rec_trinket;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.NumericUpDown Battleground_min_Divine_Plea_mana;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.NumericUpDown Battleground_mana_judge;
        private System.Windows.Forms.GroupBox groupBox41;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown Battleground_min_player_inside_HR;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown Battleground_HR_how_much_health;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.NumericUpDown Battleground_HR_how_far;
        private System.Windows.Forms.CheckBox Battleground_wanna_HR;
        private System.Windows.Forms.GroupBox groupBox42;
        private System.Windows.Forms.NumericUpDown Battleground_min_mana_potion;
        private System.Windows.Forms.NumericUpDown Battleground_min_LoH_hp;
        private System.Windows.Forms.NumericUpDown Battleground_min_HoS_hp;
        private System.Windows.Forms.NumericUpDown Battleground_min_HoP_hp;
        private System.Windows.Forms.NumericUpDown Battleground_min_DS_hp;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.NumericUpDown Battleground_min_DP_hp;
        private System.Windows.Forms.CheckBox Battleground_wanna_mana_potion;
        private System.Windows.Forms.CheckBox Battleground_wanna_LoH;
        private System.Windows.Forms.CheckBox Battleground_wanna_DP;
        private System.Windows.Forms.CheckBox Battleground_wanna_DS;
        private System.Windows.Forms.CheckBox Battleground_wanna_HoS;
        private System.Windows.Forms.CheckBox Battleground_wanna_HoP;
        private System.Windows.Forms.GroupBox Battleground_auraselctGB;
        private System.Windows.Forms.RadioButton Battleground_DisabledRB;
        private System.Windows.Forms.RadioButton Battleground_resistanceRB;
        private System.Windows.Forms.RadioButton Battleground_concentrationRB;
        private System.Windows.Forms.GroupBox groupBox44;
        private System.Windows.Forms.NumericUpDown Battleground_min_torrent_mana_perc;
        private System.Windows.Forms.NumericUpDown Battleground_min_stoneform;
        private System.Windows.Forms.NumericUpDown Battleground_min_gift_hp;
        private System.Windows.Forms.CheckBox Battleground_wanna_torrent;
        private System.Windows.Forms.CheckBox Battleground_wanna_stoneform;
        private System.Windows.Forms.CheckBox Battleground_wanna_everymanforhimself;
        private System.Windows.Forms.CheckBox Battleground_wanna_gift;
        private System.Windows.Forms.GroupBox groupBox45;
        private System.Windows.Forms.CheckBox Battleground_do_not_dismount_EVER;
        private System.Windows.Forms.CheckBox Battleground_do_not_dismount_ooc;
        private System.Windows.Forms.CheckBox Battleground_wanna_move_to_HoJ;
        private System.Windows.Forms.CheckBox Battleground_wanna_mount;
        private System.Windows.Forms.CheckBox Battleground_wanna_move_to_heal;
        private System.Windows.Forms.CheckBox Battleground_wanna_crusader;
        private System.Windows.Forms.GroupBox Battleground_generalGB;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.NumericUpDown Battleground_rest_if_mana_below;
        private System.Windows.Forms.CheckBox Battleground_wanna_buff;
        private System.Windows.Forms.GroupBox groupBox47;
        private System.Windows.Forms.CheckBox Battleground_wanna_Judge;
        private System.Windows.Forms.CheckBox Battleground_wanna_CS;
        private System.Windows.Forms.CheckBox Battleground_wanna_HoW;
        private System.Windows.Forms.GroupBox groupBox48;
        private System.Windows.Forms.CheckBox Battleground_wanna_lifeblood;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.NumericUpDown Battleground_min_ohshitbutton_activator;
        private System.Windows.Forms.CheckBox Battleground_wanna_GotAK;
        private System.Windows.Forms.CheckBox Battleground_wanna_AW;
        private System.Windows.Forms.CheckBox Battleground_wanna_DF;
        private System.Windows.Forms.GroupBox groupBox43;
        private System.Windows.Forms.GroupBox groupBox46;
        private System.Windows.Forms.CheckBox WorldPVP_get_tank_from_lua;
        private System.Windows.Forms.CheckBox WorldPVP_get_tank_from_focus;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox WorldPVP_advanced_option;
        private System.Windows.Forms.GroupBox WorldPVP_advanced;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown WorldPVP_max_healing_distance;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_target;
        private System.Windows.Forms.GroupBox groupBox50;
        private System.Windows.Forms.NumericUpDown WorldPVP_do_not_heal_above;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_face;
        private System.Windows.Forms.GroupBox groupBox51;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_cleanse;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_urgent_cleanse;
        private System.Windows.Forms.GroupBox groupBox52;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_HoJ;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_rebuke;
        private System.Windows.Forms.GroupBox groupBox53;
        private System.Windows.Forms.Label WorldPVP_min_Inf_of_light_DL_hpLB;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_Inf_of_light_DL_hp;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_HL_hp;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_FoL_hp;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_DL_hp;
        private System.Windows.Forms.CheckBox WorldPVP_Inf_of_light_wanna_DL;
        private System.Windows.Forms.GroupBox groupBox54;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.NumericUpDown WorldPVP_use_mana_rec_trinket_every;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_mana_rec_trinket;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_Divine_Plea_mana;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.NumericUpDown WorldPVP_mana_judge;
        private System.Windows.Forms.GroupBox groupBox55;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_player_inside_HR;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.NumericUpDown WorldPVP_HR_how_much_health;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.NumericUpDown WorldPVP_HR_how_far;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_HR;
        private System.Windows.Forms.GroupBox groupBox56;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_mana_potion;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_LoH_hp;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_HoS_hp;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_HoP_hp;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_DS_hp;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_DP_hp;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_mana_potion;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_LoH;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_DP;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_DS;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_HoS;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_HoP;
        private System.Windows.Forms.GroupBox WorldPVP_auraselctGB;
        private System.Windows.Forms.RadioButton WorldPVP_DisabledRB;
        private System.Windows.Forms.RadioButton WorldPVP_resistanceRB;
        private System.Windows.Forms.RadioButton WorldPVP_concentrationRB;
        private System.Windows.Forms.GroupBox groupBox58;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_torrent_mana_perc;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_stoneform;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_gift_hp;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_torrent;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_stoneform;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_everymanforhimself;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_gift;
        private System.Windows.Forms.GroupBox groupBox59;
        private System.Windows.Forms.CheckBox WorldPVP_do_not_dismount_EVER;
        private System.Windows.Forms.CheckBox WorldPVP_do_not_dismount_ooc;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_move_to_HoJ;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_mount;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_move_to_heal;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_crusader;
        private System.Windows.Forms.GroupBox WorldPVP_generalGB;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.NumericUpDown WorldPVP_rest_if_mana_below;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_buff;
        private System.Windows.Forms.GroupBox groupBox61;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_Judge;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_CS;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_HoW;
        private System.Windows.Forms.GroupBox groupBox62;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_lifeblood;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.NumericUpDown WorldPVP_min_ohshitbutton_activator;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_GotAK;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_AW;
        private System.Windows.Forms.CheckBox WorldPVP_wanna_DF;
        private System.Windows.Forms.RadioButton Solo_bless_type_disabledRB;
        private System.Windows.Forms.RadioButton Solo_bless_type_MightRB;
        private System.Windows.Forms.RadioButton Solo_bless_type_KingRB;
        private System.Windows.Forms.RadioButton Solo_bless_type_autoRB;
        private System.Windows.Forms.GroupBox PVE_bless_selection;
        private System.Windows.Forms.RadioButton PVE_bless_type_disabledRB;
        private System.Windows.Forms.RadioButton PVE_bless_type_MightRB;
        private System.Windows.Forms.RadioButton PVE_bless_type_KingRB;
        private System.Windows.Forms.RadioButton PVE_bless_type_autoRB;
        private System.Windows.Forms.GroupBox Raid_bless_selection;
        private System.Windows.Forms.RadioButton Raid_bless_type_disabledRB;
        private System.Windows.Forms.RadioButton Raid_bless_type_MightRB;
        private System.Windows.Forms.RadioButton Raid_bless_type_KingRB;
        private System.Windows.Forms.RadioButton Raid_bless_type_autoRB;
        private System.Windows.Forms.GroupBox Battleground_bless_selection;
        private System.Windows.Forms.RadioButton Battleground_bless_type_disabledRB;
        private System.Windows.Forms.RadioButton Battleground_bless_type_MightRB;
        private System.Windows.Forms.RadioButton Battleground_bless_type_KingRB;
        private System.Windows.Forms.RadioButton Battleground_bless_type_autoRB;
        private System.Windows.Forms.GroupBox ARENA_bless_selection;
        private System.Windows.Forms.RadioButton ARENA_bless_type_disabledRB;
        private System.Windows.Forms.RadioButton ARENA_bless_type_MightRB;
        private System.Windows.Forms.RadioButton ARENA_bless_type_KingRB;
        private System.Windows.Forms.RadioButton ARENA_bless_type_autoRB;
        private System.Windows.Forms.GroupBox WorldPVP_bless_selection;
        private System.Windows.Forms.RadioButton WorldPVP_bless_type_disabledRB;
        private System.Windows.Forms.RadioButton WorldPVP_bless_type_MightRB;
        private System.Windows.Forms.RadioButton WorldPVP_bless_type_KingRB;
        private System.Windows.Forms.RadioButton WorldPVP_bless_type_autoRB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.NumericUpDown PVE_healing_tank_priority;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.NumericUpDown Raid_healing_tank_priority;
        private System.Windows.Forms.GroupBox Solo_optimizeGB;
        private System.Windows.Forms.RadioButton Solo_intellywait;
        private System.Windows.Forms.RadioButton Solo_accurancy;
        private System.Windows.Forms.RadioButton Solo_speed;
        private System.Windows.Forms.GroupBox PVE_optimizeGB;
        private System.Windows.Forms.RadioButton PVE_intellywait;
        private System.Windows.Forms.RadioButton PVE_accurancy;
        private System.Windows.Forms.RadioButton PVE_speed;
        private System.Windows.Forms.GroupBox Raid_optimizeGB;
        private System.Windows.Forms.RadioButton Raid_intellywait;
        private System.Windows.Forms.RadioButton Raid_accurancy;
        private System.Windows.Forms.RadioButton Raid_speed;
        private System.Windows.Forms.GroupBox Battleground_optimizeGB;
        private System.Windows.Forms.RadioButton Battleground_intellywait;
        private System.Windows.Forms.RadioButton Battleground_accurancy;
        private System.Windows.Forms.RadioButton Battleground_speed;
        private System.Windows.Forms.GroupBox ARENA_optimizeGB;
        private System.Windows.Forms.RadioButton ARENA_intellywait;
        private System.Windows.Forms.RadioButton ARENA_accurancy;
        private System.Windows.Forms.RadioButton ARENA_speed;
        private System.Windows.Forms.GroupBox WorldPVP_optimizeGB;
        private System.Windows.Forms.RadioButton WorldPVP_intellywait;
        private System.Windows.Forms.RadioButton WorldPVP_accurancy;
        private System.Windows.Forms.RadioButton WorldPVP_speed;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox Solo_answer_PVP_attacks;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.GroupBox groupBox35;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB1;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB5;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB4;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB3;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB2;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB1;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB20;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB19;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB18;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB17;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB16;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB15;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB14;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB13;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB12;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB11;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB10;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB9;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB8;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB7;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB6;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB5;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB4;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB3;
        private System.Windows.Forms.TextBox PVP_dispell_ASAP_TB2;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB20;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB19;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB18;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB17;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB16;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB15;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB14;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB13;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB12;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB11;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB10;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB9;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB8;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB7;
        private System.Windows.Forms.TextBox PVP_do_not_touch_TB6;
        private System.Windows.Forms.GroupBox groupBox49;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB20;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB19;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB18;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB17;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB16;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB15;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB14;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB13;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB12;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB11;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB10;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB9;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB8;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB7;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB6;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB5;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB4;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB3;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB2;
        private System.Windows.Forms.TextBox PVE_dispell_ASAP_TB1;
        private System.Windows.Forms.GroupBox groupBox57;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB20;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB19;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB18;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB17;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB16;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB15;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB14;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB13;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB12;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB11;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB10;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB9;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB8;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB7;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB6;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB5;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB4;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB3;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB2;
        private System.Windows.Forms.TextBox PVE_do_not_touch_TB1;
        private System.Windows.Forms.CheckBox Solo_enable_pull;
        private System.Windows.Forms.TabPage tabPage13;
        private System.Windows.Forms.GroupBox groupBox60;
        private System.Windows.Forms.Label Trinket1_ID_LB;
        private System.Windows.Forms.Button Trnket1_name_load;
        private System.Windows.Forms.Label Trinket1_CD_LB;
        private System.Windows.Forms.NumericUpDown PVE_tank_healing_priority_multiplier;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.NumericUpDown Raid_tank_healing_priority_multiplier;
        private System.Windows.Forms.Label Trinket1_name_LB;
        private System.Windows.Forms.GroupBox groupBox63;
        private System.Windows.Forms.Label Trinket2_name_LB;
        private System.Windows.Forms.Label Trinket2_CD_LB;
        private System.Windows.Forms.Label Trinket2_ID_LB;
        private System.Windows.Forms.GroupBox Trinket1_use_when_GB;
        private System.Windows.Forms.GroupBox Trinket2_use_when_GB;
        private System.Windows.Forms.RadioButton Trinket2_PVP;
        private System.Windows.Forms.RadioButton Trinket2_mana;
        private System.Windows.Forms.RadioButton Trinket2_HPS;
        private System.Windows.Forms.RadioButton Trinket2_never;
        private System.Windows.Forms.RadioButton Trinket1_PVP;
        private System.Windows.Forms.RadioButton Trinket1_mana;
        private System.Windows.Forms.RadioButton Trinket1_HPS;
        private System.Windows.Forms.RadioButton Trinket1_never;
        private System.Windows.Forms.TabPage tabPage14;
        private System.Windows.Forms.Button Stop_Healing_BT;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown General_precasting;
        private System.Windows.Forms.CheckBox Solo_account_for_lag;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.CheckBox PVE_cleanse_only_self_and_tank;
        private System.Windows.Forms.CheckBox Raid_cleanse_only_self_and_tank;
        private System.Windows.Forms.CheckBox Battleground_cleanse_only_self_and_tank;
        private System.Windows.Forms.CheckBox ARENA_cleanse_only_self_and_tank;
        private System.Windows.Forms.CheckBox WorldPVP_cleanse_only_self_and_tank;
        private System.Windows.Forms.GroupBox groupBox64;
        private System.Windows.Forms.NumericUpDown Solo_exorcism_min_mana;
        private System.Windows.Forms.NumericUpDown Solo_holy_wrath_min_mana;
        private System.Windows.Forms.CheckBox Solo_wanna_holy_wrath;
        private System.Windows.Forms.CheckBox Solo_wanna_exorcism;
        private System.Windows.Forms.CheckBox Solo_exorcism_for_denunce;
        private System.Windows.Forms.NumericUpDown Solo_consecration_min_mana;
        private System.Windows.Forms.CheckBox Solo_wanna_consecration;
        private System.Windows.Forms.CheckBox cbLogging;
    }
}