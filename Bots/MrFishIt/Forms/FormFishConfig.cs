using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MrFishIt.Forms
{
    public partial class FormFishConfig : Form
    {
        public FormFishConfig()
        {
            InitializeComponent();
        }

        private void FormFishConfig_Load(object sender, EventArgs e)
        {
            cbxLures.Items.Add(new LureEntry("Aquadynamic Fish Attractor", 6533));
            cbxLures.Items.Add(new LureEntry("Aquadynamic Fish Lens", 6811));
            cbxLures.Items.Add(new LureEntry("Bright Baubles", 6532));
            cbxLures.Items.Add(new LureEntry("Flesh Eating Worm", 7307));
            cbxLures.Items.Add(new LureEntry("Glow Worm", 46006));
            cbxLures.Items.Add(new LureEntry("Nightcrawlers", 6530));
            cbxLures.Items.Add(new LureEntry("Sharpened Fish Hook", 34861));
            cbxLures.Items.Add(new LureEntry("Shiny Bauble", 6529));
            cbxLures.Items.Add(new LureEntry("Weather-Beaten Fishing Hat", 33820));

            FisherSettings.Instance.Load();
            UpdateGUISettings();
            RegisterSettingsEventHandlers();

            pbNemo.Image = Image.FromStream(new MemoryStream(new WebClient().DownloadData("http://dl.dropbox.com/u/4381027/nemo.png")));
            Icon = new Icon(new MemoryStream(new WebClient().DownloadData("http://dl.dropbox.com/u/4381027/nemo.ico")), 32, 32);
        }

        private void UpdateGUISettings()
        {
            var settings = FisherSettings.Instance;

            cbUseLure.Checked = settings.UseLure;

            int lureId = settings.LureId;
            for (int i = 0; i < cbxLures.Items.Count; i++)
            {
                var entry = cbxLures.Items[i];
                if (entry is LureEntry)
                {
                    var lure = (LureEntry)entry;

                    if (lure.Id == lureId)
                    {
                        cbxLures.SelectedIndex = i;
                    }
                }
            }
        }

        private void RegisterSettingsEventHandlers()
        {
            cbUseLure.CheckedChanged += (sender, args) => FisherSettings.Instance.UseLure = cbUseLure.Checked;

            cbxLures.SelectedIndexChanged += (sender, args) =>
                                                 {
                                                     var item = cbxLures.SelectedItem;
                                                     if(item is LureEntry)
                                                     {
                                                         var lure = (LureEntry)item;
                                                         FisherSettings.Instance.LureId = lure.Id;
                                                     }
                                                 };

            btnSaveClose.Click += (sender, args) =>
            {
                FisherSettings.Instance.Save();
                Close();
            };

            FormClosing += (sender, args) => FisherSettings.Instance.Save();
        }
    }

    public class LureEntry
    {
        private readonly string _name;
        private readonly int _id;

        public LureEntry(string name, int id)
        {
            _name = name;
            _id = id;
        }

        /// <summary>
        /// Returns the name of the lure.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Returns the Id of the lure.
        /// </summary>
        public int Id { get { return _id; } }

        public override string ToString()
        {
            return string.Format("[{0}]", Name);
        }
    }
}
