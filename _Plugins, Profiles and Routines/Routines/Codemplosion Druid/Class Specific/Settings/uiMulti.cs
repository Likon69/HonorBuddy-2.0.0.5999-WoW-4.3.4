using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hera.Class_Specific.Settings
{
    public partial class uiMulti : Form
    {
        public uiMulti()
        {
            InitializeComponent();
        }

        public UIForm2 SourceUI { get; set; }
        public ComboBox SourceDropDown { get; set; }
        public string FormTitleMessage { get; set; }
        private bool andLogic = false;

        private void uiMulti_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormTitleMessage)) FormTitleMessage = "Selection multiple options...";
            this.Text = FormTitleMessage;
            string[] options = (SourceDropDown.Tag !=null) ? SourceDropDown.Tag.ToString().Split('|') : new string[0];

            if (options.Count() > 0) andLogic = (options[0].ToUpper() == "AND");

            foreach (string item in SourceDropDown.Items)
            {
                bool isChecked = false;
                if (item.ToUpper().Contains("MULTIPLE OPTIONS")) continue;
                if (item.ToUpper().Contains("NEVER")) continue;
                if (item.ToUpper().Contains("ALWAYS")) continue;

                foreach (string s in options.Where(s => item.ToUpper() == s.ToUpper()))
                {
                    isChecked = true;
                }

                clb1.Items.Add(item,isChecked);
            }

            comboConditionEvaluation.SelectedIndex = andLogic ? 1 : 0;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string result = clb1.CheckedItems.Cast<string>().Aggregate("", (current, item) => current + (item + "|"));
            string conditionalResult = "";

            if (andLogic) conditionalResult = "AND|" + result;
            else conditionalResult = result;
            if (conditionalResult.EndsWith("|")) conditionalResult = conditionalResult.TrimEnd('|');

            SourceUI.ForceMultipleOptionsUpdate(SourceDropDown, conditionalResult);

            this.Close();
        }

        private void comboConditionEvaluation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboConditionEvaluation.SelectedItem.ToString().ToUpper().Contains("OR"))
            {
                andLogic = false;
                labelDescription.Text =
                    "All options will be evaluated using 'OR' logic. If ANY of the selected conditions are met then the spell will be cast. If no conditions are met the spell will not be cast.";
            }
            else
            {
                andLogic = true;
                labelDescription.Text =
                    "All options will be evaluated using 'AND' logic. ALL of the selected conditions must be met before the spell will be cast. If one condition fails the spell will not be cast.";
                
            }
        }

    }
}
