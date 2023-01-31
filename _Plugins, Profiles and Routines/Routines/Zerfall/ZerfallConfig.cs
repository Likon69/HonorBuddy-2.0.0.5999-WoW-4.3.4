using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.Helpers;
using Zerfall.Talents;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using Color = System.Drawing.Color;
using Sequence = TreeSharp.Sequence;

namespace Zerfall
{
    public partial class ZerfallConfig : Form
    {
        public ZerfallConfig()
        {
            Logging.Write("Settings Panel Opened");
            InitializeComponent();

        }

        private void ZerfallConfig_Load(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Load();
            PullSpellSelect.SelectedItem = ZerfallSettings.Instance.PullSpellSelect;
            PetSpell.SelectedItem = ZerfallSettings.Instance.PetSpell;
            RestHealthPercentage.Value = new decimal(ZerfallSettings.Instance.RestHealthPercentage);
            RestManaPercentage.Value = new decimal(ZerfallSettings.Instance.RestManaPercentage);
            ManaPotPercent.Value = new decimal(ZerfallSettings.Instance.ManaPotPercent);
            HealthPotPercent.Value = new decimal(ZerfallSettings.Instance.HealthPotPercent);
            HealthStoneCombat.Value = new decimal(ZerfallSettings.Instance.HealthStoneCombat);
            RestStone.Checked = ZerfallSettings.Instance.RestStone;
            CurseSelect.SelectedItem = ZerfallSettings.Instance.CurseSelect;
            BaneSelect.SelectedItem = ZerfallSettings.Instance.BaneSelect;
            ArmorSelect.SelectedItem = ZerfallSettings.Instance.ArmorSelect;
            LifeTap_MP_Start.Value = new decimal(ZerfallSettings.Instance.LifeTap_MP_Start);
            LifeTap_HP_Limit.Value = new decimal(ZerfallSettings.Instance.LifeTap_HP_Limit);
            Adds.Value = new decimal(ZerfallSettings.Instance.Adds);

            Use_Shadowflame.Checked = ZerfallSettings.Instance.Use_Shadowflame;
            Use_Immolate.Checked = ZerfallSettings.Instance.Use_Immolate;
            Use_Corruption.Checked = ZerfallSettings.Instance.Use_Corruption;
            Use_LifeTap.Checked = ZerfallSettings.Instance.Use_LifeTap;
            Use_DemonSoul.Checked = ZerfallSettings.Instance.Use_DemonSoul;
            Use_Soulburn.Checked = ZerfallSettings.Instance.Use_Soulburn;
            Got_ISF.Checked = ZerfallSettings.Instance.Got_ISF;
            SoulHarvestRest.Checked = ZerfallSettings.Instance.SoulHarvestRest;
            PS_Sacrifice.Checked = ZerfallSettings.Instance.PS_Sacrifice;
            PS_BloodPact.Checked = ZerfallSettings.Instance.PS_BloodPact;
            Use_DrainSoul.Checked = ZerfallSettings.Instance.Use_DrainSoul;
            Use_DrainLife.Checked =ZerfallSettings.Instance.Use_DrainLife;
            MoveDisable.Checked = ZerfallSettings.Instance.MoveDisable;
            
            RainOfFire.Checked = ZerfallSettings.Instance.RainOfFire;
        }



        private void RestHealthPercentage_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.RestHealthPercentage = int.Parse(RestHealthPercentage.Value.ToString());
        }

        private void RestManaPercentage_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.RestManaPercentage = int.Parse(RestManaPercentage.Value.ToString());
        }

        private void PetSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.PetSpell = PetSpell.SelectedItem.ToString();
        }

        private void PullSpellSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.PullSpellSelect = PullSpellSelect.SelectedItem.ToString();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show(
                          "Are you sure you want to save settings? if Settings Still do not take effect, you may need to restart honorbuddy.",
                          "Warning",
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                Logging.Write(Color.Red,
                                    "Really? Not saving settings? thats too bad. ");
            }
            if (dr == DialogResult.Yes)
            {

                ZerfallSettings.Instance.Save();
                ZerfallSettings.Instance.Load();
                if (ZerfallSettings.Instance.IsConfigured == false)
                {
                    Logging.Write(Color.Red,
                                  "Thank you for Configuring Zerfall, If You Experiance Issues Such as Honorbuddy not Pulling Please Restart your Instance of Honorbuddy to allow the Settings to Take Effect.");
                    ZerfallSettings.Instance.IsConfigured = true;
                    Logging.Write(Color.Blue,
                                  "One thing you need to remember about me, because it just might save your life one day....I most definitely am a Madman with a Box!");
                    ZerfallSettings.Instance.Save();
                }
                else
                {
                    Logging.Write(Color.Red,
                                    "Some Settings May require you to restart honorbuddy to take effect since sometimes its Wibely Wobbly, Timey Whimey.");
                }
            }
        }

        private void RestStone_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.RestStone = RestStone.Checked;
        }

        private void HealthPotPercent_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.HealthPotPercent = int.Parse(HealthPotPercent.Value.ToString());
        }

        private void ManaPotPercent_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.ManaPotPercent = int.Parse(ManaPotPercent.Value.ToString());
        }

        private void HealthStoneCombat_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.HealthStoneCombat = int.Parse(HealthStoneCombat.Value.ToString());
        }

        private void BaneSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.BaneSelect = BaneSelect.SelectedItem.ToString();
        }

        private void CurseSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.CurseSelect = CurseSelect.SelectedItem.ToString();
        }

        private void Use_Shadowflame_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_Shadowflame = Use_Shadowflame.Checked;
        }

        private void Use_Immolate_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_Immolate = Use_Immolate.Checked;
        }

        private void Use_Corruption_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_Corruption = Use_Corruption.Checked;
        }

        private void Use_LifeTap_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_LifeTap = Use_LifeTap.Checked;
        }

        private void Use_DemonSoul_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_DemonSoul = Use_DemonSoul.Checked;
        }

        private void Use_Soulburn_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_Soulburn = Use_Soulburn.Checked;
        }

        private void Got_ISF_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Got_ISF = Got_ISF.Checked;
        }

        private void LifeTap_MP_Start_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.LifeTap_MP_Start = int.Parse(LifeTap_MP_Start.Value.ToString());
        }

        private void LifeTap_HP_Limit_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.LifeTap_HP_Limit = int.Parse(LifeTap_HP_Limit.Value.ToString());
        }

        private void PS_Sacrifice_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.PS_Sacrifice = PS_Sacrifice.Checked;
        }

        private void PS_BloodPact_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.PS_BloodPact = PS_BloodPact.Checked;
        }

        private void Use_DrainSoul_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_DrainSoul = Use_DrainSoul.Checked;
        }

        private void Use_DrainLife_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Use_DrainLife = Use_DrainLife.Checked;
        }

        private void ArmorSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.ArmorSelect = ArmorSelect.SelectedItem.ToString();
        }

        private void SoulHarvestRest_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.SoulHarvestRest = SoulHarvestRest.Checked;
            
        }

        private void Adds_ValueChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.Adds = int.Parse(Adds.Value.ToString());
        }

        private void RainOfFire_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.RainOfFire = RainOfFire.Checked;
        }

        private void MoveDisable_CheckedChanged(object sender, EventArgs e)
        {
            ZerfallSettings.Instance.MoveDisable = MoveDisable.Checked;
            if (ZerfallSettings.Instance.MoveDisable == true)
            {
                Logging.Write("Remember to Disable this Setting When not using LazyRaider", Color.DarkGreen);
            }
        }

    




    }
}
