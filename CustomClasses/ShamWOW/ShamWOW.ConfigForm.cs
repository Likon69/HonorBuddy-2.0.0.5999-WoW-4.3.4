/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS COMBAT CLASS WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * ShamWOW Shaman CC
 * 
 * Author:  Bobby53
 * 
 * See the ShamWOW.chm file for Help
 *
 */
using Styx.Helpers;

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Styx.WoWInternals;
using Styx.Logic.Combat;
using System.Collections.Generic;

namespace Bobby53
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

            // public enum PveCombatStyle { Normal, FarmingLowLevelMobs };
            cboInterruptStyle.Items.Add(new CboItem((int)ConfigValues.SpellInterruptStyle.None, "None"));
            cboInterruptStyle.Items.Add(new CboItem((int)ConfigValues.SpellInterruptStyle.CurrentTarget, "Current Target"));
            cboInterruptStyle.Items.Add(new CboItem((int)ConfigValues.SpellInterruptStyle.All, "All Targets"));

            // public enum PveCombatStyle { Normal, FarmingLowLevelMobs };
            cboPVE_CombatStyle.Items.Add(new CboItem((int)ConfigValues.PveCombatStyle.Normal, "Normal"));
            cboPVE_CombatStyle.Items.Add(new CboItem((int)ConfigValues.PveCombatStyle.FarmingLowLevelMobs, "Farming Low-Level Mobs"));

            // public enum TypeOfPull { Fast, Ranged, Body, Auto }
            cboPVE_TypeOfPull.Items.Add(new CboItem((int)ConfigValues.TypeOfPull.Auto, "Auto"));
            cboPVE_TypeOfPull.Items.Add(new CboItem((int)ConfigValues.TypeOfPull.Fast, "Fast"));
            cboPVE_TypeOfPull.Items.Add(new CboItem((int)ConfigValues.TypeOfPull.Ranged, "Ranged"));
            cboPVE_TypeOfPull.Items.Add(new CboItem((int)ConfigValues.TypeOfPull.Body, "Body"));

            // public enum PvpCombatStyle { CombatOnly, HealingOverCombat, HealingOnly }
            cboPVP_CombatStyle.Items.Add(new CboItem((int)ConfigValues.PvpCombatStyle.CombatOnly, "Combat Only"));
            cboPVP_CombatStyle.Items.Add(new CboItem((int)ConfigValues.PvpCombatStyle.HealingOverCombat, "Healing over Combat"));
            cboPVP_CombatStyle.Items.Add(new CboItem((int)ConfigValues.PvpCombatStyle.HealingOnly, "Healing Only"));

            // public enum RafCombatStyle { CombatOnly, HealingOverCombat, HealingOnly }
            cboRAF_CombatStyle.Items.Add(new CboItem((int)ConfigValues.RafCombatStyle.Auto, "Auto"));
            cboRAF_CombatStyle.Items.Add(new CboItem((int)ConfigValues.RafCombatStyle.CombatOnly, "Combat Only"));
            cboRAF_CombatStyle.Items.Add(new CboItem((int)ConfigValues.RafCombatStyle.HealingOverCombat, "Healing over Combat"));
            cboRAF_CombatStyle.Items.Add(new CboItem((int)ConfigValues.RafCombatStyle.HealingOnly, "Healing Only"));

            cboPVP_CleansePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.None, "None" ));
            cboPVP_CleansePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.High, "High" ));
            cboPVP_CleansePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.Low, "Low" ));

            cboPVP_PurgePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.None, "None" ));
            cboPVP_PurgePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.High, "High" ));
            cboPVP_PurgePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.Low, "Low" ));

            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.None,   "None"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Star,   "Star"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Circle,   "Circle"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Diamond,   "Diamond"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Triangle,   "Triangle"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Moon,   "Moon"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Square,   "Square"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Cross,   "Cross"));
            cboPVP_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Skull,   "Skull"));

            cboRAF_CleansePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.None, "None" ));
            cboRAF_CleansePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.High, "High" ));
            cboRAF_CleansePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.Low, "Low" ));

            cboRAF_PurgePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.None, "None" ));
            cboRAF_PurgePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.High, "High" ));
            cboRAF_PurgePriority.Items.Add(new CboItem((int)ConfigValues.SpellPriority.Low, "Low" ));

            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.None,   "None"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Star,   "Star"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Circle,   "Circle"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Diamond,   "Diamond"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Triangle,   "Triangle"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Moon,   "Moon"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Square,   "Square"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Cross,   "Cross"));
            cboRAF_HexIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Skull,   "Skull"));

            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.None,   "None"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Star,   "Star"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Circle,   "Circle"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Diamond,   "Diamond"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Triangle,   "Triangle"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Moon,   "Moon"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Square,   "Square"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Cross,   "Cross"));
            cboRAF_BindIcon.Items.Add(new CboItem((int)ConfigValues.RaidTarget.Skull,   "Skull"));
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            lblVersion.Text = Shaman.Version;

            // General options
            chkDebug.Checked = Shaman.cfg.Debug;

            chkUseGhostWolf.Checked = Shaman.cfg.UseGhostWolfForm;
            numDistanceForGhostWolf.Value = Shaman.cfg.DistanceForGhostWolf;
            numRestMinMana.Value = Shaman.cfg.RestManaPercent;
            numRestMinHealth.Value = Shaman.cfg.RestHealthPercent;
            numNeedHeal.Value = Shaman.cfg.NeedHealHealthPercent;
            numEmergencyMinHealth.Value = Shaman.cfg.EmergencyHealthPercent;
            numEmergencyMinMana.Value = Shaman.cfg.EmergencyManaPercent;
            numLifebloodMinHealth.Value = Shaman.cfg.LifebloodPercent;
            numShamanisticRageMinMana.Value = Shaman.cfg.ShamanisticRagePercent;
            numThunderstormMinMana.Value = Shaman.cfg.ThunderstormPercent;
            numManaTideMinMana.Value = Shaman.cfg.ManaTidePercent;
            numTrinkAtHealth.Value = Shaman.cfg.TrinkAtHealth;
            numTrinkAtMana.Value = Shaman.cfg.TrinkAtMana;

            chkUseBandages.Checked = Shaman.cfg.UseBandages;
            numDistanceReclaim.Value = Shaman.cfg.DistanceForTotemRecall;
            numPVE_TwistMana.Value = Shaman.cfg.TwistManaPercent;
            numPVE_TwistDamage.Value = Shaman.cfg.TwistDamagePercent;
            chkDisableMovement.Checked = Shaman.cfg.DisableMovement;
            chkDisableTargeting.Checked = Shaman.cfg.DisableTargeting;
            chkMeleeCombatBeforeLevel10.Checked = Shaman.cfg.MeleeCombatBeforeLevel10;
            SetComboBoxEnum(cboInterruptStyle, (int)Shaman.cfg.InterruptStyle);
            chkDetectImmunities.Checked = Shaman.cfg.DetectImmunities;
            chkWaterWalking.Checked = Shaman.cfg.WaterWalking;

            // PVE Grinding 
            SetComboBoxEnum( cboPVE_CombatStyle, (int) Shaman.cfg.PVE_CombatStyle );
            SetComboBoxEnum( cboPVE_TypeOfPull,  (int) Shaman.cfg.PVE_PullType  );
            chkPVE_Stress_FeralSpirit.Checked = Shaman.cfg.PVE_SaveForStress_FeralSpirit;
            chkPVE_Stress_ElementalTotems.Checked = Shaman.cfg.PVE_SaveForStress_ElementalTotems;
            chkPVE_Stress_DPS_Racial.Checked = Shaman.cfg.PVE_SaveForStress_DPS_Racials;
            chkPVE_Stress_Bloodlust.Checked = Shaman.cfg.PVE_SaveForStress_Bloodlust;
            chkPVE_StressOnly_TotemBar.Checked = Shaman.cfg.PVE_SaveForStress_TotemsSelected;
            numPVE_LevelsAboveAsElite.Value = Shaman.cfg.PVE_LevelsAboveAsElite;
            numPVE_StressfulMobCount.Value = Shaman.cfg.PVE_StressfulMobCount;
            SetComboBoxString(cboPVE_TotemEarth,  Shaman.cfg.PVE_TotemEarth.ToString() );
            SetComboBoxString(cboPVE_TotemFire,   Shaman.cfg.PVE_TotemFire.ToString() );
            SetComboBoxString(cboPVE_TotemWater, Shaman.cfg.PVE_TotemWater.ToString());
            SetComboBoxString(cboPVE_TotemAir, Shaman.cfg.PVE_TotemAir.ToString());
            SetComboBoxString(cboPVE_Mainhand, Shaman.cfg.PVE_MainhandImbue.ToString() );
            SetComboBoxString(cboPVE_Offhand, Shaman.cfg.PVE_OffhandImbue.ToString());

            // PVP Battlegrounds
            SetComboBoxEnum(cboPVP_CombatStyle, (int) Shaman.cfg.PVP_CombatStyle );
            SetComboBoxString(cboPVP_TotemEarth, Shaman.cfg.PVP_TotemEarth.ToString());
            SetComboBoxString(cboPVP_TotemFire, Shaman.cfg.PVP_TotemFire.ToString());
            SetComboBoxString(cboPVP_TotemWater, Shaman.cfg.PVP_TotemWater.ToString());
            SetComboBoxString(cboPVP_TotemAir, Shaman.cfg.PVP_TotemAir.ToString());
            SetComboBoxString(cboPVP_Mainhand, Shaman.cfg.PVP_MainhandImbue.ToString());
            SetComboBoxString(cboPVP_Offhand, Shaman.cfg.PVP_OffhandImbue.ToString());

            SetComboBoxEnum(cboPVP_CleansePriority, (int) Shaman.cfg.PVP_CleansePriority);
            SetComboBoxEnum(cboPVP_PurgePriority, (int)Shaman.cfg.PVP_PurgePriority);
            SetComboBoxEnum(cboPVP_HexIcon, (int)Shaman.cfg.PVP_HexIcon);

            SetComboBoxEnum(cboRAF_CleansePriority, (int) Shaman.cfg.RAF_CleansePriority);
            SetComboBoxEnum(cboRAF_PurgePriority, (int) Shaman.cfg.RAF_PurgePriority);
            SetComboBoxEnum(cboRAF_HexIcon, (int) Shaman.cfg.RAF_HexIcon);
            SetComboBoxEnum(cboRAF_BindIcon, (int) Shaman.cfg.RAF_BindIcon);

            chkPVP_PrepWaterWalking.Enabled = Shaman._hasGlyphOfWaterWalking;
            chkPVP_PrepWaterWalking.Checked = Shaman.cfg.PVP_PrepWaterWalking;
            chkPVP_PrepWaterBreathing.Enabled = Shaman._hasGlyphOfWaterBreathing;
            chkPVP_PrepWaterBreathing.Checked = Shaman.cfg.PVP_PrepWaterBreathing ;

            this.numPVP_Heal_HealingWave.Value = Shaman.cfg.PVP_Heal.HealingWave;
            this.numPVP_Heal_Riptide.Value = Shaman.cfg.PVP_Heal.Riptide;
            this.numPVP_Heal_ChainHeal.Value = Shaman.cfg.PVP_Heal.ChainHeal;
            this.numPVP_Heal_HealingRain.Value = Shaman.cfg.PVP_Heal.HealingRain;
            this.numPVP_Heal_UnleashElements.Value = Shaman.cfg.PVP_Heal.UnleashElements;
            this.numPVP_Heal_HealingSurge.Value = Shaman.cfg.PVP_Heal.HealingSurge;
            this.numPVP_Heal_GreaterHW.Value = Shaman.cfg.PVP_Heal.GreaterHealingWave;
            this.numPVP_Heal_OhShoot.Value = Shaman.cfg.PVP_Heal.OhShoot;
            this.chkPVP_Heal_TidalWaves.Checked = Shaman.cfg.PVP_Heal.TidalWaves;
            this.chkPVP_Heal_Cleanse.Checked = Shaman.cfg.PVP_Heal.Cleanse;
            // this.chkPVP_Heal_Pets.Checked = Shaman.cfg.PVP_Heal.Pets;
            // this.chkPVP_Heal_SearchRange.Checked = Shaman.cfg.PVP_Heal.SearchRange;

            // RAF
            SetComboBoxEnum(cboRAF_CombatStyle, (int)Shaman.cfg.RAF_CombatStyle);
            numRAF_GroupOffHeal.Value = Shaman.cfg.RAF_GroupOffHeal;
            chkRAF_UseThunderstorm.Checked  = Shaman.cfg.RAF_UseThunderstorm;
            chkRAF_UseBloodlust.Checked  = Shaman.cfg.RAF_UseBloodlustOnBosses;
            chk_RAF_Save_FeralSpirit.Checked  = Shaman.cfg.RAF_SaveFeralSpiritForBosses;
            chkRAF_SaveElementalTotems.Checked  = Shaman.cfg.RAF_SaveElementalTotemsForBosses;
            chkRAF_FollowClosely.Checked  = Shaman.cfg.RAF_FollowClosely;

            SetComboBoxString(cboRAF_TotemEarth, Shaman.cfg.RAF_TotemEarth.ToString());
            SetComboBoxString(cboRAF_TotemFire, Shaman.cfg.RAF_TotemFire.ToString());
            SetComboBoxString(cboRAF_TotemWater, Shaman.cfg.RAF_TotemWater.ToString());
            SetComboBoxString(cboRAF_TotemAir, Shaman.cfg.RAF_TotemAir.ToString());

            this.numRAF_Heal_HealingWave.Value = Shaman.cfg.RAF_Heal.HealingWave;
            this.numRAF_Heal_Riptide.Value = Shaman.cfg.RAF_Heal.Riptide;
            this.numRAF_Heal_ChainHeal.Value = Shaman.cfg.RAF_Heal.ChainHeal;
            this.numRAF_Heal_HealingRain.Value = Shaman.cfg.RAF_Heal.HealingRain;
            this.numRAF_Heal_UnleashElements.Value = Shaman.cfg.RAF_Heal.UnleashElements;
            this.numRAF_Heal_HealingSurge.Value = Shaman.cfg.RAF_Heal.HealingSurge;
            this.numRAF_Heal_GreaterHW.Value = Shaman.cfg.RAF_Heal.GreaterHealingWave;
            this.numRAF_Heal_OhShoot.Value = Shaman.cfg.RAF_Heal.OhShoot;
            this.chkRAF_Heal_TidalWaves.Checked = Shaman.cfg.RAF_Heal.TidalWaves;
            this.chkRAF_Heal_Cleanse.Checked = Shaman.cfg.RAF_Heal.Cleanse;
            // this.chkRAF_Heal_Pets.Checked = Shaman.cfg.RAF_Heal.Pets;
            // this.chkRAF_Heal_SearchRange.Checked = Shaman.cfg.RAF_Heal.SearchRange;

            // now enable/disable based upon settings
            lblSafeDistGhostWolf.Enabled = chkUseGhostWolf.Checked;
            numDistanceForGhostWolf.Enabled = chkUseGhostWolf.Checked;
            chkMeleeCombatBeforeLevel10.Enabled = ObjectManager.Me.Level < 10;
            // chkDisableTargeting.Enabled = chkDisableMovement.Checked;
            
            // color label text for group healing spells that aren't available
            // .. but leave enabled to control values in case spell is learned while running
            lblGrpHeal_HealingWave.Enabled = SpellManager.HasSpell("Healing Wave");
            lblGrpHeal_Riptide.Enabled =
            lblGrpHeal_TidalWaves.Enabled = SpellManager.HasSpell("Riptide");
            lblGrpHeal_Cleanse.Enabled = SpellManager.HasSpell("Cleanse Spirit");
            lblGrpHeal_UnleashElements.Enabled = SpellManager.HasSpell("Unleash Elements");
            lblGrpHeal_ChainHeal.Enabled = SpellManager.HasSpell("Chain Heal");
            lblGrpHeal_HealingRain.Enabled = SpellManager.HasSpell("Healing Rain");
            lblGrpHeal_GreaterHealingWave.Enabled = SpellManager.HasSpell("Greater Healing Wave");
            lblGrpHeal_HealingSurge.Enabled = SpellManager.HasSpell("Healing Surge");
            lblGrpHeal_OhShoot.Enabled = SpellManager.HasSpell("Nature's Swiftness");

            LoadSpellList(lvwBlacklist, Shaman._hashCleanseBlacklist);
            LoadSpellList(lvwWhitelist , Shaman._hashPurgeWhitelist );
            LoadMobList(lvwMoblist, Shaman._dictMob);
        }

        private static void LoadSpellList(ListView lvw, HashSet<int> hs)
        {
            lvw.Items.Clear();
            foreach (int id in hs)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = id.ToString();

                ListViewItem.ListViewSubItem lvs = new ListViewItem.ListViewSubItem();
                WoWSpell s = WoWSpell.FromId(id);
                if (s != null)
                {
                    lvs.Text = s.Name;
                    lvi.SubItems.Add(lvs);
                }

                lvw.Items.Add(lvi);
            }
        }

        private static void LoadMobList(ListView lvw, Dictionary<int,Mob> mobList)
        {
            lvw.Items.Clear();
            foreach (KeyValuePair<int,Mob> kv in mobList)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = kv.Value.Id.ToString();

                ListViewItem.ListViewSubItem lvs = new ListViewItem.ListViewSubItem();
                lvs.Text = kv.Value.Name ;
                lvi.SubItems.Add(lvs);

                lvs = new ListViewItem.ListViewSubItem();
                lvs.Text = kv.Value.HitBox.ToString();
                lvi.SubItems.Add(lvs);

                lvw.Items.Add(lvi);
            }
        }

        private static void SetComboBoxEnum(System.Windows.Forms.ComboBox cb, int e) 
        {
            CboItem item;
            for (int i = 0; i < cb.Items.Count; i++)
            {
                item = (CboItem) cb.Items[i];
                if (item.e == e)
                {
                    cb.SelectedIndex = i;
                    return;
                }
            }

            item = (CboItem)cb.Items[0];
            Styx.Helpers.Logging.Write("Dialog Error: combobox {0} does not have enum({1}) in list, defaulting to enum({2})", cb.Name, e, item.e);
            cb.SelectedIndex = 0;
        }

        private static void SetComboBoxString(System.Windows.Forms.ComboBox cb, string sText)
        {
            int idx = cb.FindString(sText);
            if (idx == -1)
            {
                Styx.Helpers.Logging.Write("Dialog Error: combobox {0} does not have value '{1}' in list", cb.Name, sText);
                idx = cb.FindString("Auto");
                if (idx == -1)
                {
                    Styx.Helpers.Logging.Write("Dialog Error: combobox {0} does not have an 'Auto' value either, defaulting to first in list", cb.Name);
                    idx = 0;
                }
            }

            cb.SelectedIndex = idx;
        }


        private static int GetComboBoxEnum(System.Windows.Forms.ComboBox cb)
        {
            CboItem item = (CboItem)cb.Items[cb.SelectedIndex];
            return item.e;
        }

        private static void GetComboBoxString(System.Windows.Forms.ComboBox cb, out string svalue)
        {
            svalue = cb.Items[cb.SelectedIndex].ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Styx.Helpers.Logging.Write("In ConfigForm_FormClosing()");
            Shaman.cfg.Debug = chkDebug.Checked;

            Shaman.cfg.UseGhostWolfForm = chkUseGhostWolf.Checked;
            Shaman.cfg.DistanceForGhostWolf = (int)numDistanceForGhostWolf.Value;
            Shaman.cfg.RestManaPercent = (int)numRestMinMana.Value;
            Shaman.cfg.RestHealthPercent = (int)numRestMinHealth.Value;
            Shaman.cfg.NeedHealHealthPercent = (int)numNeedHeal.Value;
            Shaman.cfg.EmergencyHealthPercent = (int)numEmergencyMinHealth.Value;
            Shaman.cfg.EmergencyManaPercent = (int)numEmergencyMinMana.Value;
            Shaman.cfg.LifebloodPercent = (int)numLifebloodMinHealth.Value;
            Shaman.cfg.ShamanisticRagePercent = (int) numShamanisticRageMinMana.Value;
            Shaman.cfg.ThunderstormPercent = (int)numThunderstormMinMana.Value;
            Shaman.cfg.ManaTidePercent = (int)numManaTideMinMana.Value;
            Shaman.cfg.TrinkAtHealth = (int)numTrinkAtHealth.Value;
            Shaman.cfg.TrinkAtMana = (int) numTrinkAtMana.Value;
            Shaman.cfg.UseBandages = chkUseBandages.Checked;
            Shaman.cfg.DistanceForTotemRecall = (int)numDistanceReclaim.Value;
            Shaman.cfg.TwistManaPercent = (int)numPVE_TwistMana.Value;
            Shaman.cfg.TwistDamagePercent = (int)numPVE_TwistDamage.Value;
            Shaman.cfg.DisableMovement = chkDisableMovement.Checked;
            Shaman.cfg.DisableTargeting = chkDisableTargeting.Checked;
            Shaman.cfg.MeleeCombatBeforeLevel10 = chkMeleeCombatBeforeLevel10.Checked;
            Shaman.cfg.InterruptStyle = (ConfigValues.SpellInterruptStyle)GetComboBoxEnum(cboInterruptStyle);
            Shaman.cfg.DetectImmunities = chkDetectImmunities.Checked;
            Shaman.cfg.WaterWalking = chkWaterWalking.Checked;

            Shaman.cfg.PVE_CombatStyle = (ConfigValues.PveCombatStyle) GetComboBoxEnum( cboPVE_CombatStyle );
            Shaman.cfg.PVE_PullType = (ConfigValues.TypeOfPull) GetComboBoxEnum( cboPVE_TypeOfPull );
            Shaman.cfg.PVE_SaveForStress_FeralSpirit = chkPVE_Stress_FeralSpirit.Checked;
            Shaman.cfg.PVE_SaveForStress_ElementalTotems = chkPVE_Stress_ElementalTotems.Checked;
            Shaman.cfg.PVE_SaveForStress_DPS_Racials = chkPVE_Stress_DPS_Racial.Checked;
            Shaman.cfg.PVE_SaveForStress_Bloodlust = chkPVE_Stress_Bloodlust.Checked;
            Shaman.cfg.PVE_SaveForStress_TotemsSelected = chkPVE_StressOnly_TotemBar.Checked;
            Shaman.cfg.PVE_LevelsAboveAsElite = (int)numPVE_LevelsAboveAsElite.Value;
            Shaman.cfg.PVE_StressfulMobCount = (int)numPVE_StressfulMobCount.Value;
            GetComboBoxString(cboPVE_TotemEarth, out Shaman.cfg.PVE_TotemEarth);
            GetComboBoxString(cboPVE_TotemFire, out Shaman.cfg.PVE_TotemFire);
            GetComboBoxString(cboPVE_TotemWater, out Shaman.cfg.PVE_TotemWater);
            GetComboBoxString(cboPVE_TotemAir, out Shaman.cfg.PVE_TotemAir);
            GetComboBoxString(cboPVE_Mainhand, out Shaman.cfg.PVE_MainhandImbue);
            GetComboBoxString(cboPVE_Offhand, out Shaman.cfg.PVE_OffhandImbue);

            Shaman.cfg.PVP_CombatStyle = (ConfigValues.PvpCombatStyle) GetComboBoxEnum(cboPVP_CombatStyle);
            // Shaman.cfg.PVP_GroupNeedHeal = (int)numPVP_GroupNeedHeal.Value;
            GetComboBoxString(cboPVP_TotemEarth, out Shaman.cfg.PVP_TotemEarth);
            GetComboBoxString(cboPVP_TotemFire, out Shaman.cfg.PVP_TotemFire);
            GetComboBoxString(cboPVP_TotemWater, out Shaman.cfg.PVP_TotemWater);
            GetComboBoxString(cboPVP_TotemAir, out Shaman.cfg.PVP_TotemAir);
            GetComboBoxString(cboPVP_Mainhand, out Shaman.cfg.PVP_MainhandImbue);
            GetComboBoxString(cboPVP_Offhand, out Shaman.cfg.PVP_OffhandImbue);

            Shaman.cfg.PVP_Heal.HealingSurge = (int)this.numPVP_Heal_HealingSurge.Value;
            Shaman.cfg.PVP_Heal.OhShoot = (int)this.numPVP_Heal_OhShoot.Value;
            Shaman.cfg.PVP_Heal.GreaterHealingWave = (int)this.numPVP_Heal_GreaterHW.Value;
            Shaman.cfg.PVP_Heal.Riptide = (int)this.numPVP_Heal_Riptide.Value;
            Shaman.cfg.PVP_Heal.ChainHeal = (int)this.numPVP_Heal_ChainHeal.Value;
            Shaman.cfg.PVP_Heal.HealingRain = (int)this.numPVP_Heal_HealingRain.Value;
            Shaman.cfg.PVP_Heal.HealingWave = (int)this.numPVP_Heal_HealingWave.Value;
            Shaman.cfg.PVP_Heal.UnleashElements = (int)this.numPVP_Heal_UnleashElements.Value;
            Shaman.cfg.PVP_Heal.TidalWaves = this.chkPVP_Heal_TidalWaves.Checked;
            Shaman.cfg.PVP_Heal.Cleanse = this.chkPVP_Heal_Cleanse.Checked;

            Shaman.cfg.RAF_CombatStyle = (ConfigValues.RafCombatStyle)GetComboBoxEnum(cboRAF_CombatStyle);
            // Shaman.cfg.RAF_GroupNeedHeal = (int)numRAF_GroupNeedHeal.Value;
            Shaman.cfg.RAF_GroupOffHeal = (int) numRAF_GroupOffHeal.Value;
            Shaman.cfg.RAF_UseThunderstorm = chkRAF_UseThunderstorm.Checked;
            Shaman.cfg.RAF_UseBloodlustOnBosses = chkRAF_UseBloodlust.Checked;
            Shaman.cfg.RAF_SaveFeralSpiritForBosses = chk_RAF_Save_FeralSpirit.Checked;
            Shaman.cfg.RAF_SaveElementalTotemsForBosses = chkRAF_SaveElementalTotems.Checked;
            Shaman.cfg.RAF_FollowClosely = chkRAF_FollowClosely.Checked;
            GetComboBoxString(cboRAF_TotemEarth, out Shaman.cfg.RAF_TotemEarth);
            GetComboBoxString(cboRAF_TotemFire, out Shaman.cfg.RAF_TotemFire);
            GetComboBoxString(cboRAF_TotemWater, out Shaman.cfg.RAF_TotemWater);
            GetComboBoxString(cboRAF_TotemAir, out Shaman.cfg.RAF_TotemAir);

            Shaman.cfg.RAF_Heal.HealingSurge	= (int) this.numRAF_Heal_HealingSurge.Value	;
            Shaman.cfg.RAF_Heal.OhShoot			= (int) this.numRAF_Heal_OhShoot.Value 		;
            Shaman.cfg.RAF_Heal.GreaterHealingWave		= (int) this.numRAF_Heal_GreaterHW.Value 		;
            Shaman.cfg.RAF_Heal.Riptide			= (int) this.numRAF_Heal_Riptide.Value 		;
            Shaman.cfg.RAF_Heal.ChainHeal = (int)this.numRAF_Heal_ChainHeal.Value;
            Shaman.cfg.RAF_Heal.HealingRain = (int)this.numRAF_Heal_HealingRain.Value;
            Shaman.cfg.RAF_Heal.HealingWave = (int)this.numRAF_Heal_HealingWave.Value;
            Shaman.cfg.RAF_Heal.UnleashElements = (int)this.numRAF_Heal_UnleashElements.Value;

            Shaman.cfg.RAF_Heal.TidalWaves = this.chkRAF_Heal_TidalWaves.Checked;
            Shaman.cfg.RAF_Heal.Cleanse = this.chkRAF_Heal_Cleanse.Checked;

            Shaman.cfg.PVP_CleansePriority = (ConfigValues.SpellPriority) GetComboBoxEnum(cboPVP_CleansePriority);
            Shaman.cfg.PVP_PurgePriority = (ConfigValues.SpellPriority)GetComboBoxEnum(cboPVP_PurgePriority);
            Shaman.cfg.PVP_HexIcon = (ConfigValues.RaidTarget)GetComboBoxEnum(cboPVP_HexIcon);

            Shaman.cfg.RAF_CleansePriority = (ConfigValues.SpellPriority)GetComboBoxEnum(cboRAF_CleansePriority);
            Shaman.cfg.RAF_PurgePriority = (ConfigValues.SpellPriority)GetComboBoxEnum(cboRAF_PurgePriority);
            Shaman.cfg.RAF_HexIcon = (ConfigValues.RaidTarget)GetComboBoxEnum(cboRAF_HexIcon);
            Shaman.cfg.RAF_BindIcon = (ConfigValues.RaidTarget)GetComboBoxEnum(cboRAF_BindIcon);


        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string sPath = Process.GetCurrentProcess().MainModule.FileName;
            sPath = Path.GetDirectoryName(sPath);
            sPath = Path.Combine(sPath, "CustomClasses\\ShamWOW\\Shaman.chm");
            Logging.WriteDebug("Loading config file: {0}", sPath);

            Help.ShowHelp(this, sPath);
        }

        private void btnReportIssue_Click(object sender, EventArgs e)
        {
            if ( Shaman.cfg.Debug == false )
            {
                MessageBox.Show( this, 
                    "You have been running ShamWOW with DEBUG turned OFF." + Environment.NewLine 
                    + "The log file does not contain information needed to" + Environment.NewLine
                    + "provide you support on this issue.  " + Environment.NewLine
                    + "" + Environment.NewLine
                    + "Please exit HonorBuddy and then re-start being sure" + Environment.NewLine
                    + "to enable the ShamWOW DEBUG flag to ON.",
                    "Warning - Debug flag is OFF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            IssueForm frm = new IssueForm();
            frm.ShowDialog();
        }


        private void chkUseGhostWolf_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseGhostWolf.Checked)
            {
                MessageBox.Show(
                    "Note:  to use Ghost Wolf you must also clear the Use Mount setting in HonorBuddy/General Settings",
                    "ShamWOW",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }

            lblSafeDistGhostWolf.Enabled = chkUseGhostWolf.Checked;
            numDistanceForGhostWolf.Enabled = chkUseGhostWolf.Checked;
        }

        private void cboPVE_TotemEarth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboPVE_TotemFire_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboPVE_TotemWater_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboPVE_TotemAir_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numPVE_TwistMana_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numPVE_TwistDamage_ValueChanged(object sender, EventArgs e)
        {
        }

        private void btnGiveRep_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(
            this,
            "http://www.buddyforum.de/reputation.php?do=addreputation&p=62511",
            HelpNavigator.TableOfContents,
            "Contents"
            );
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numPVE_TwistMana_Validating(object sender, CancelEventArgs e)
        {
            if (numPVE_TwistMana.Value >= numPVE_TwistDamage.Value)
            {
                e.Cancel = true;
                numPVE_TwistMana.Select(0, 2);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider1.SetError(
                    numPVE_TwistMana,
                    String.Format("Water Shield Mana is {0}% but must be less than Lightning Shield Mana {1}%", numPVE_TwistMana.Value, numPVE_TwistDamage.Value)
                    );
            }
        }

        private void numPVE_TwistDamage_Validating(object sender, CancelEventArgs e)
        {
            if (numPVE_TwistMana.Value >= numPVE_TwistDamage.Value)
            {
                e.Cancel = true;
                numPVE_TwistDamage.Select(0, 2);

                // Set the ErrorProvider error with the text to display. 
                this.errorProvider1.SetError(
                    numPVE_TwistDamage,
                    String.Format("Lightning Shield Mana is {1}% but must be greater than Water Shield Mana {0}%", numPVE_TwistMana.Value, numPVE_TwistDamage.Value)
                    );
            }
        }

        private void btnLogTargets_Click(object sender, EventArgs e)
        {
            Shaman.ListWowUnitsInRange();
        }

        private void cboPVP_CombatStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cboRAF_CombatStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if not combat only, then make the heal spells visible
        }

        private void tabAbout_Click(object sender, EventArgs e)
        {

        }

        private void SetRafGroupHeal()
        {
            int maxVal = 0;

            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_HealingWave.Value);
            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_Riptide.Value);
            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_GreaterHW.Value);
            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_ChainHeal.Value);
            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_HealingRain.Value);
            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_HealingSurge.Value);
            maxVal = Math.Max(maxVal, (int)this.numRAF_Heal_OhShoot.Value);

            this.numRAF_GroupNeedHeal.Value = maxVal;
        }

        private void SetPvpGroupHeal()
        {
            int maxVal = 0;

            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_HealingWave.Value);
            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_Riptide.Value);
            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_GreaterHW.Value);
            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_ChainHeal.Value);
            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_HealingRain.Value);
            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_HealingSurge.Value);
            maxVal = Math.Max(maxVal, (int)this.numPVP_Heal_OhShoot.Value);

            this.numPVP_GroupNeedHeal.Value = maxVal;
        }


        private void ConfigForm_Validating(object sender, CancelEventArgs e)
        {
            bool cancelAction = false;
            HashSet<int> healNum = new HashSet<int>();

            HealCheckReset();
            if (HealCheck(this.numRAF_Heal_HealingWave)
                || HealCheck(this.numRAF_Heal_Riptide)
                || HealCheck(this.numRAF_Heal_ChainHeal)
                || HealCheck(this.numRAF_Heal_HealingRain)
                || HealCheck(this.numRAF_Heal_GreaterHW)
                || HealCheck(this.numRAF_Heal_HealingSurge)
                || HealCheck(this.numRAF_Heal_OhShoot))
                cancelAction = true;

            HealCheckReset();
            if (HealCheck(this.numPVP_Heal_HealingWave)
                || HealCheck(this.numPVP_Heal_Riptide)
                || HealCheck(this.numPVP_Heal_ChainHeal)
                || HealCheck(this.numPVP_Heal_HealingRain)
                || HealCheck(this.numPVP_Heal_GreaterHW)
                || HealCheck(this.numPVP_Heal_HealingSurge)
                || HealCheck(this.numPVP_Heal_OhShoot))
                cancelAction = true;

            e.Cancel = cancelAction;
        }

        private HashSet<int> healNum;

        private void HealCheckReset()
        {
            healNum = new HashSet<int>();
        }

        private bool HealCheck(NumericUpDown num)
        {
            bool usedAlready = !healNum.Add((int)num.Value);

            if (usedAlready)
            {
                MessageBox.Show("Error:  Heal % for each column must be unique");
                tabOptions.SelectTab(4);
                num.Focus();
            }

            return usedAlready;
        }

        private void numPVP_Heal_ValueChanged(object sender, EventArgs e)
        {
            SetPvpGroupHeal();
        }

        private void numRAF_Heal_ValueChanged(object sender, EventArgs e)
        {
            SetRafGroupHeal();
        }

        private void btnPVP_Heal_LowMana_Click(object sender, EventArgs e)
        {
            this.numPVP_Heal_HealingWave.Value     = 91;
            this.numPVP_Heal_Riptide.Value         = 90;
            this.numPVP_Heal_ChainHeal.Value = 80;
            this.numPVP_Heal_HealingRain.Value = 79;
            this.numPVP_Heal_UnleashElements.Value = 78;
            this.numPVP_Heal_GreaterHW.Value       = 65;
            this.numPVP_Heal_HealingSurge.Value    = 45;
            this.numPVP_Heal_OhShoot.Value         = 30;
            this.chkPVP_Heal_TidalWaves.Checked = true;
            this.chkPVP_Heal_Cleanse.Checked = true;
            // this.chkPVP_Heal_Pets.Checked = def.PVP_Heal.Pets;

            SetPvpGroupHeal();
        }

        private void btnPVP_Heal_HighThroughput_Click(object sender, EventArgs e)
        {
            ConfigValues def = new ConfigValues();

            this.numPVP_Heal_HealingSurge.Value = def.PVP_Heal.HealingSurge;
            this.numPVP_Heal_OhShoot.Value = def.PVP_Heal.OhShoot;
            this.numPVP_Heal_GreaterHW.Value = def.PVP_Heal.GreaterHealingWave;
            this.numPVP_Heal_Riptide.Value = def.PVP_Heal.Riptide;
            this.numPVP_Heal_UnleashElements.Value = def.PVP_Heal.UnleashElements;
            this.numPVP_Heal_ChainHeal.Value = def.PVP_Heal.ChainHeal;
            this.numPVP_Heal_HealingRain.Value = def.PVP_Heal.HealingRain;
            this.numPVP_Heal_HealingWave.Value = def.PVP_Heal.HealingWave;
            this.chkPVP_Heal_TidalWaves.Checked = def.PVP_Heal.TidalWaves;
            this.chkPVP_Heal_Cleanse.Checked = def.PVP_Heal.Cleanse;
            // this.chkPVP_Heal_Pets.Checked = def.PVP_Heal.Pets;

            SetPvpGroupHeal();
        }

        private void btnRAF_Heal_LowMana_Click(object sender, EventArgs e)
        {
            ConfigValues def = new ConfigValues();

            this.numRAF_Heal_HealingSurge.Value = def.RAF_Heal.HealingSurge;
            this.numRAF_Heal_OhShoot.Value = def.RAF_Heal.OhShoot;
            this.numRAF_Heal_GreaterHW.Value = def.RAF_Heal.GreaterHealingWave;
            this.numRAF_Heal_Riptide.Value = def.RAF_Heal.Riptide;
            this.numRAF_Heal_UnleashElements.Value = def.RAF_Heal.UnleashElements;
            this.numRAF_Heal_ChainHeal.Value = def.RAF_Heal.ChainHeal;
            this.numRAF_Heal_HealingRain.Value = def.RAF_Heal.HealingRain;
            this.numRAF_Heal_HealingWave.Value = def.RAF_Heal.HealingWave;
            this.chkRAF_Heal_TidalWaves.Checked = def.RAF_Heal.TidalWaves;
            this.chkRAF_Heal_Cleanse.Checked = def.RAF_Heal.Cleanse;
            // this.chkRAF_Heal_Pets.Checked = def.RAF_Heal.Pets;

            SetRafGroupHeal();
        }

        private void btnRAF_Heal_RaidHealer(object sender, EventArgs e)
        {
            this.numRAF_Heal_HealingWave.Value = 90;
            this.numRAF_Heal_Riptide.Value = 0;
            this.numRAF_Heal_ChainHeal.Value = 89;
            this.numRAF_Heal_HealingRain.Value = 65;
            this.numRAF_Heal_HealingSurge.Value = 45;
            this.numRAF_Heal_UnleashElements.Value = 69;
            this.numRAF_Heal_OhShoot.Value = 30;
            this.numRAF_Heal_GreaterHW.Value = 70;
            this.chkRAF_Heal_TidalWaves.Checked = true;
            this.chkRAF_Heal_Cleanse.Checked = true;
            // this.chkRAF_Heal_Pets.Checked = false;

            SetRafGroupHeal();
        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void lblRAF_GroupNeedHeal_Click(object sender, EventArgs e)
        {

        }

        private void chkDisableMovement_CheckedChanged(object sender, EventArgs e)
        {
            // chkDisableTargeting.Enabled = chkDisableMovement.Checked;
        }

        private void btnRAF_Heal_HealingRain_Click(object sender, EventArgs e)
        {
            this.numRAF_Heal_HealingWave.Value = 0;
            this.numRAF_Heal_Riptide.Value = 90;
            this.numRAF_Heal_ChainHeal.Value = 0;
            this.numRAF_Heal_HealingRain.Value = 89;
            this.numRAF_Heal_HealingSurge.Value = 45;
            this.numRAF_Heal_UnleashElements.Value = 88;
            this.numRAF_Heal_OhShoot.Value = 30;
            this.numRAF_Heal_GreaterHW.Value = 75;
            this.chkRAF_Heal_TidalWaves.Checked = true;
            this.chkRAF_Heal_Cleanse.Checked = true;
            // this.chkRAF_Heal_Pets.Checked = false;

            SetRafGroupHeal();
        }

        private void tabHealSpells_Click(object sender, EventArgs e)
        {

        }

        private void groupBox19_Enter(object sender, EventArgs e)
        {

        }
    }

    public class CboItem
    {
        public int e;
        public string s;

        public override string  ToString()
        {
 	         return s;
        }

        public CboItem( int pe, string ps )
        {
            e = pe;
            s = ps;
        }
    }


}
