using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EzFrost
{
    public partial class EzFrostSettingsWindow : Form
    {
        public EzFrostSettingsWindow()
        {
            InitializeComponent();
        }

        private void EzFrostSettingsWindow_Load(object sender, EventArgs e)
        {
            EzFrostSettings.Instance.Load();



            //Parse some shit that doesnt get saved properly
            List<Trinket> Trinkat = new List<Trinket>();
            foreach (String t in EzFrostSettings.Instance.Trinkets)
            {
                Trinkat.Add(new Trinket(t.ToString()));
            }
            EzFrostSettings.Instance._Trinkets = Trinkat.ToArray();

           List<Act> Acts = new List<Act>();
            foreach (String t in EzFrostSettings.Instance.HealRotation)
            {
                Acts.Add(new Act(t.ToString()));
            }
            EzFrostSettings.Instance._HealRotation = Acts.ToArray();


            propertyGrid1.SelectedObject = EzFrostSettings.Instance;
        }

        private void EzFrostSettingsWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            //convert trinkets and heal rotation into strings
            List<String> Trinkets = new List<String>();
            foreach (Trinket t in EzFrostSettings.Instance._Trinkets)
            {
                Trinkets.Add(t.ToString());
            }
            EzFrostSettings.Instance.Trinkets = Trinkets.ToArray();

            List<String> Acts = new List<String>();
            foreach (Act t in EzFrostSettings.Instance._HealRotation)
            {
                Acts.Add(t.ToString());
            }
            EzFrostSettings.Instance.HealRotation = Acts.ToArray();



            EzFrostSettings.Instance.Save();
        }
    }
}