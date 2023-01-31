using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EzRet
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            EzRetSettings.Instance.Load();

            if (EzRetSettings.Instance.Trinkets[0] == "Vicious Gladiator's Badge of Victory--0-100")
            {
                EzRetSettings.Instance.Trinkets[0] = "Badge of Victory--0-100";
            }

            
            //Parse some shit that doesnt get saved properly
            List<Trinket> Trinkat = new List<Trinket>();
            foreach(String t in EzRetSettings.Instance.Trinkets)
            {
                Trinkat.Add(new Trinket(t.ToString()));
            }
            EzRetSettings.Instance._Trinkets = Trinkat.ToArray();

            List<Act> Acts = new List<Act>();
            foreach (String t in EzRetSettings.Instance.HealRotation)
            {
                Acts.Add(new Act(t.ToString()));
            }
            EzRetSettings.Instance._HealRotation = Acts.ToArray();


            propertyGrid1.SelectedObject = EzRetSettings.Instance;
            /*foreach (String str in EzRetSettings.Instance.GrayItems)
            {
                listView1.Items.Add(str);
            }*/
            /*var bindableNames = from name in EzRetSettings.Instance.GrayItems select new { Names = name };

            dataGridView1.DataSource = bindableNames.ToList();
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            //dataGridView1.DataSource = ;*/
            /*foreach (String str in EzRetSettings.Instance.Rotation)
            {
                dataGridView1.Rows.Add(str);
            }
           
            foreach (String str in EzRetSettings.Instance.Cooldowns)
            {
                dataGridView2.Rows.Add(str);
            }*/

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*List<String> Rotation = new List<String> { };
            //Need to conver the data list view shit back into their values as used by the bot

            //Convert rotation

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                Rotation.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            EzRetSettings.Instance.Rotation = Rotation.ToArray();
            EzRetSettings.Instance.Save();*/
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            //convert trinkets and heal rotation into strings
            List<String> Trinkets = new List<String>();
            foreach (Trinket t in EzRetSettings.Instance._Trinkets)
            {
                Trinkets.Add(t.ToString());
            }
            EzRetSettings.Instance.Trinkets = Trinkets.ToArray();

            List<String> Acts = new List<String>();
            foreach (Act t in EzRetSettings.Instance._HealRotation)
            {
                Acts.Add(t.ToString());
            }
            EzRetSettings.Instance.HealRotation = Acts.ToArray();



            EzRetSettings.Instance.Save();
        }
    }
}