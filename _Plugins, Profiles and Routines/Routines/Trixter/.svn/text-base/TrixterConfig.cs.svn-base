using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace Trixter
{
    public partial class TrixterForm : Form
    {
        public TrixterForm()
        {
            InitializeComponent();
        }

        private void TrixterConfig_Load(object sender, EventArgs e)
        {

            if (Trixter.UseKillingSpree)
            {
                LabelKS.Text = "ON";
                LabelKS.ForeColor = Color.Green;
            }
            else
            {
                LabelKS.Text = "OFF";
                LabelKS.ForeColor = Color.Red;
            }

            if (Trixter.UseRecup)
            {
                LabelRecup.Text = "ON";
                LabelRecup.ForeColor = Color.Green;
            }
            else
            {
                LabelRecup.Text = "OFF";
                LabelRecup.ForeColor = Color.Red;
            }
            if (Trixter.UseAdrenalin)
            {
                LabelAdrenalin.Text = "ON";
                LabelAdrenalin.ForeColor = Color.Green;
            }
            else
            {
                LabelAdrenalin.Text = "OFF";
                LabelAdrenalin.ForeColor = Color.Red;
            }
            if (Trixter.UseRupture)
            {
                LabelRupture.Text = "ON";
                LabelRupture.ForeColor = Color.Green;
            }
            else
            {
                LabelRupture.Text = "OFF";
                LabelRupture.ForeColor = Color.Red;
            }
            if (Trixter.UseBladeFlurry)
            {
                LabelBladeFlurry.Text = "ON";
                LabelBladeFlurry.ForeColor = Color.Green;
            }
            else
            {
                LabelBladeFlurry.Text = "OFF";
                LabelBladeFlurry.ForeColor = Color.Red;
            }

            if (Trixter.UseInterrupt)
            {
                LabelInterrupt.Text = "ON";
                LabelInterrupt.ForeColor = Color.Green;
            }
            else
            {
                LabelInterrupt.Text = "OFF";
                LabelInterrupt.ForeColor = Color.Red;
            }

            if (Trixter.UseRedirect)
            {
                LabelRedirect.Text = "ON";
                LabelRedirect.ForeColor = Color.Green;
            }
            else
            {
                LabelRedirect.Text = "OFF";
                LabelRedirect.ForeColor = Color.Red;
            }

            if (Trixter.UseVanish)
            {
                LabelVanish.Text = "ON";
                LabelVanish.ForeColor = Color.Green;
            }
            else
            {
                LabelVanish.Text = "OFF";
                LabelVanish.ForeColor = Color.Red;
            }

            if (Trixter.UseVendetta)
            {
                LabelVendetta.Text = "ON";
                LabelVendetta.ForeColor = Color.Green;
            }
            else
            {
                LabelVendetta.Text = "OFF";
                LabelVendetta.ForeColor = Color.Red;
            }

            if (Trixter.UseColdBlood)
            {
                LabelColdBlood.Text = "ON";
                LabelColdBlood.ForeColor = Color.Green;
            }
            else
            {
                LabelColdBlood.Text = "OFF";
                LabelColdBlood.ForeColor = Color.Red;
            }

            //LabelTarget.Text = Trixter.TotTTarget;

        }

        private void ButtonKS_Click(object sender, EventArgs e)
        {
            if (Trixter.UseKillingSpree == true)
            {
                Trixter.UseKillingSpree = false;
                LabelKS.Text = "OFF";
                LabelKS.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseKillingSpree = true;
                LabelKS.Text = "ON";
                LabelKS.ForeColor = Color.Green;
            }
        }

        private void ButtonAdrenalin_Click(object sender, EventArgs e)
        {
            if (Trixter.UseAdrenalin == true)
            {
                Trixter.UseAdrenalin = false;
                LabelAdrenalin.Text = "OFF";
                LabelAdrenalin.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseAdrenalin = true;
                LabelAdrenalin.Text = "ON";
                LabelAdrenalin.ForeColor = Color.Green;
            }
        }

        private void ButtonRecup_Click(object sender, EventArgs e)
        {
            if (Trixter.UseRecup == true)
            {
                Trixter.UseRecup = false;
                LabelRecup.Text = "OFF";
                LabelRecup.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseRecup = true;
                LabelRecup.Text = "ON";
                LabelRecup.ForeColor = Color.Green;
            }
        }

        private void ButtonRupture_Click(object sender, EventArgs e)
        {
            if (Trixter.UseRupture == true)
            {
                Trixter.UseRupture = false;
                LabelRupture.Text = "OFF";
                LabelRupture.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseRupture = true;
                LabelRupture.Text = "ON";
                LabelRupture.ForeColor = Color.Green;
            }
        }

        private void buttonTopMost_Click(object sender, EventArgs e)
        {
            if (TrixterForm.ActiveForm.TopMost == false)
            {
                TrixterForm.ActiveForm.TopMost = true;
                buttonTopMost.Text = "YES";
                buttonTopMost.ForeColor = Color.Green;
            }
            else
            {
                TrixterForm.ActiveForm.TopMost = false;
                buttonTopMost.Text = "NO";
                buttonTopMost.ForeColor = Color.Red;
            }
        }

        private void ButtonBladeFlurry_Click(object sender, EventArgs e)
        {
            if (Trixter.UseBladeFlurry == true)
            {
                Trixter.UseBladeFlurry = false;
                LabelBladeFlurry.Text = "OFF";
                LabelBladeFlurry.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseBladeFlurry = true;
                LabelBladeFlurry.Text = "ON";
                LabelBladeFlurry.ForeColor = Color.Green;
            }
        }

        private void ButtonGetTarget_Click(object sender, EventArgs e)
        {
            Trixter.GetTarget();
            LabelTarget.Text = Trixter.TotTTarget;
        }

        private void ButtonEraseTarget_Click(object sender, EventArgs e)
        {
            Trixter.EraseTarget();
            LabelTarget.Text = Trixter.TotTTarget;
        }

        private void ButtonInterrupt_Click(object sender, EventArgs e)
        {
            if (Trixter.UseInterrupt == true)
            {
                Trixter.UseInterrupt = false;
                LabelInterrupt.Text = "OFF";
                LabelInterrupt.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseInterrupt = true;
                LabelInterrupt.Text = "ON";
                LabelInterrupt.ForeColor = Color.Green;
            }
        }

        private void ButtonRedirect_Click(object sender, EventArgs e)
        {
            if (Trixter.UseRedirect == true)
            {
                Trixter.UseRedirect = false;
                LabelRedirect.Text = "OFF";
                LabelRedirect.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseRedirect = true;
                LabelRedirect.Text = "ON";
                LabelRedirect.ForeColor = Color.Green;
            }
        }

        private void ButtonVanish_Click(object sender, EventArgs e)
        {
            if (Trixter.UseVanish == true)
            {
                Trixter.UseVanish = false;
                LabelVanish.Text = "OFF";
                LabelVanish.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseVanish = true;
                LabelVanish.Text = "ON";
                LabelVanish.ForeColor = Color.Green;
            }
        }

        private void ButtonVendetta_Click(object sender, EventArgs e)
        {
            if (Trixter.UseVendetta == true)
            {
                Trixter.UseVendetta = false;
                LabelVendetta.Text = "OFF";
                LabelVendetta.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseVendetta = true;
                LabelVendetta.Text = "ON";
                LabelVendetta.ForeColor = Color.Green;
            }
        }

        private void ButtonColdBlood_Click(object sender, EventArgs e)
        {
            if (Trixter.UseColdBlood == true)
            {
                Trixter.UseColdBlood = false;
                LabelColdBlood.Text = "OFF";
                LabelColdBlood.ForeColor = Color.Red;
            }
            else
            {
                Trixter.UseColdBlood = true;
                LabelColdBlood.Text = "ON";
                LabelColdBlood.ForeColor = Color.Green;
            }
        }

        

        
    }
}
