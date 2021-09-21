using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;

using Styx;
using Styx.Logic.Combat;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins.PluginClass;
using Styx.Logic.BehaviorTree;

using Styx.Logic.Pathing;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Questing;
using Styx.Plugins;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Common;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic;
using Styx.Logic.Profiles;
using Styx.Logic.Inventory.Frames.LootFrame;

namespace ItemForAuraQuesthelper
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
            BSave.Enabled = !(ItemForAura.Settings.LockUI);

            TBAura1.Text = ItemForAura.Settings.Aura1;
            TBItem1.Text = ItemForAura.Settings.Item1;
            TBQuest11.Text = ItemForAura.Settings.Quest11;
            TBQuest12.Text = ItemForAura.Settings.Quest12;
            TBQuest13.Text = ItemForAura.Settings.Quest13;
            CBCombat1.Checked = ItemForAura.Settings.Combat1;

            TBAura2.Text = ItemForAura.Settings.Aura2;
            TBItem2.Text = ItemForAura.Settings.Item2;
            TBQuest21.Text = ItemForAura.Settings.Quest21;
            TBQuest22.Text = ItemForAura.Settings.Quest22;
            TBQuest23.Text = ItemForAura.Settings.Quest23;
            CBCombat2.Checked = ItemForAura.Settings.Combat2;

            TBAura3.Text = ItemForAura.Settings.Aura3;
            TBItem3.Text = ItemForAura.Settings.Item3;
            TBQuest31.Text = ItemForAura.Settings.Quest31;
            TBQuest32.Text = ItemForAura.Settings.Quest32;
            TBQuest33.Text = ItemForAura.Settings.Quest33;
            CBCombat3.Checked = ItemForAura.Settings.Combat3;

            TBAura4.Text = ItemForAura.Settings.Aura4;
            TBItem4.Text = ItemForAura.Settings.Item4;
            TBQuest41.Text = ItemForAura.Settings.Quest41;
            TBQuest42.Text = ItemForAura.Settings.Quest42;
            TBQuest43.Text = ItemForAura.Settings.Quest43;
            CBCombat4.Checked = ItemForAura.Settings.Combat4;

            TBAura5.Text = ItemForAura.Settings.Aura5;
            TBItem5.Text = ItemForAura.Settings.Item5;
            TBQuest51.Text = ItemForAura.Settings.Quest51;
            TBQuest52.Text = ItemForAura.Settings.Quest52;
            TBQuest53.Text = ItemForAura.Settings.Quest53;
            CBCombat5.Checked = ItemForAura.Settings.Combat5;

            TBAura6.Text = ItemForAura.Settings.Aura6;
            TBItem6.Text = ItemForAura.Settings.Item6;
            TBQuest61.Text = ItemForAura.Settings.Quest61;
            TBQuest62.Text = ItemForAura.Settings.Quest62;
            TBQuest63.Text = ItemForAura.Settings.Quest63;
            CBCombat6.Checked = ItemForAura.Settings.Combat6;

        }

        private void CB2_CheckedChanged(object sender, EventArgs e)
        {
            if (CB2.Checked)
            {
                TBAura2.Enabled = true;
                TBItem2.Enabled = true;
                TBQuest21.Enabled = true;
                TBQuest22.Enabled = true;
                TBQuest23.Enabled = true;
                CBCombat2.Enabled = true;
                TBAura2.Text = ItemForAura.Settings.Aura2;
                TBItem2.Text = ItemForAura.Settings.Item2;
                TBQuest21.Text = ItemForAura.Settings.Quest21;
                TBQuest22.Text = ItemForAura.Settings.Quest22;
                TBQuest23.Text = ItemForAura.Settings.Quest23;
                CBCombat2.Checked = ItemForAura.Settings.Combat2;
            }
            else
            {
                TBAura2.Enabled = false;
                TBItem2.Enabled = false;
                TBQuest21.Enabled = false;
                TBQuest22.Enabled = false;
                TBQuest23.Enabled = false;
                CBCombat2.Enabled = false;
                TBAura2.Text = "0";
                TBItem2.Text = "0";
                TBQuest21.Text = "0";
                TBQuest22.Text = "0";
                TBQuest23.Text = "0";
                CBCombat2.Checked = false;
            }
        }

        private void CB3_CheckedChanged(object sender, EventArgs e)
        {
            if (CB3.Checked)
            {
                TBAura3.Enabled = true;
                TBItem3.Enabled = true;
                TBQuest31.Enabled = true;
                TBQuest32.Enabled = true;
                TBQuest33.Enabled = true;
                CBCombat1.Enabled = true;
                TBAura3.Text = ItemForAura.Settings.Aura3;
                TBItem3.Text = ItemForAura.Settings.Item3;
                TBQuest31.Text = ItemForAura.Settings.Quest31;
                TBQuest32.Text = ItemForAura.Settings.Quest32;
                TBQuest33.Text = ItemForAura.Settings.Quest33;
                CBCombat3.Checked = ItemForAura.Settings.Combat3;
            }
            else
            {
                TBAura3.Enabled = false;
                TBItem3.Enabled = false;
                TBQuest31.Enabled = false;
                TBQuest32.Enabled = false;
                TBQuest33.Enabled = false;
                CBCombat3.Enabled = false;
                TBAura3.Text = "0";
                TBItem3.Text = "0";
                TBQuest31.Text = "0";
                TBQuest32.Text = "0";
                TBQuest33.Text = "0";
                CBCombat3.Checked = false;
            }
        }

        private void CB4_CheckedChanged(object sender, EventArgs e)
        {
            if (CB4.Checked)
            {
                TBAura4.Enabled = true;
                TBItem4.Enabled = true;
                TBQuest41.Enabled = true;
                TBQuest42.Enabled = true;
                TBQuest43.Enabled = true;
                CBCombat4.Enabled = true;
                TBAura4.Text = ItemForAura.Settings.Aura4;
                TBItem4.Text = ItemForAura.Settings.Item4;
                TBQuest41.Text = ItemForAura.Settings.Quest41;
                TBQuest42.Text = ItemForAura.Settings.Quest42;
                TBQuest43.Text = ItemForAura.Settings.Quest43;
                CBCombat4.Checked = ItemForAura.Settings.Combat4;
            }
            else
            {
                TBAura4.Enabled = false;
                TBItem4.Enabled = false;
                TBQuest41.Enabled = false;
                TBQuest42.Enabled = false;
                TBQuest43.Enabled = false;
                CBCombat4.Enabled = false;
                TBAura4.Text = "0";
                TBItem4.Text = "0";
                TBQuest41.Text = "0";
                TBQuest42.Text = "0";
                TBQuest43.Text = "0";
                CBCombat4.Checked = false;
            }
        }

        private void CB5_CheckedChanged(object sender, EventArgs e)
        {
            if (CB5.Checked)
            {
                TBAura5.Enabled = true;
                TBItem5.Enabled = true;
                TBQuest51.Enabled = true;
                TBQuest52.Enabled = true;
                TBQuest53.Enabled = true;
                CBCombat5.Enabled = true;
                TBAura5.Text = ItemForAura.Settings.Aura5;
                TBItem5.Text = ItemForAura.Settings.Item5;
                TBQuest51.Text = ItemForAura.Settings.Quest51;
                TBQuest52.Text = ItemForAura.Settings.Quest52;
                TBQuest53.Text = ItemForAura.Settings.Quest53;
                CBCombat5.Checked = ItemForAura.Settings.Combat5;
            }
            else
            {
                TBAura5.Enabled = false;
                TBItem5.Enabled = false;
                TBQuest51.Enabled = false;
                TBQuest52.Enabled = false;
                TBQuest53.Enabled = false;
                CBCombat5.Enabled = false;
                TBAura5.Text = "0";
                TBItem5.Text = "0";
                TBQuest51.Text = "0";
                TBQuest52.Text = "0";
                TBQuest53.Text = "0";
                CBCombat5.Checked = false;
            }
        }

        private void CB6_CheckedChanged(object sender, EventArgs e)
        {
            if (CB6.Checked)
            {
                TBAura6.Enabled = true;
                TBItem6.Enabled = true;
                TBQuest61.Enabled = true;
                TBQuest62.Enabled = true;
                TBQuest63.Enabled = true;
                CBCombat6.Enabled = true;
                TBAura6.Text = ItemForAura.Settings.Aura6;
                TBItem6.Text = ItemForAura.Settings.Item6;
                TBQuest61.Text = ItemForAura.Settings.Quest61;
                TBQuest62.Text = ItemForAura.Settings.Quest62;
                TBQuest63.Text = ItemForAura.Settings.Quest63;
                CBCombat6.Checked = ItemForAura.Settings.Combat6;
            }
            else
            {
                TBAura6.Enabled = false;
                TBItem6.Enabled = false;
                TBQuest61.Enabled = false;
                TBQuest62.Enabled = false;
                TBQuest63.Enabled = false;
                CBCombat6.Enabled = false;
                TBAura6.Text = "0";
                TBItem6.Text = "0";
                TBQuest61.Text = "0";
                TBQuest62.Text = "0";
                TBQuest63.Text = "0";
                CBCombat6.Checked = false;
            }
        }

        private void BSave_Click(object sender, EventArgs e)
        {
            if(TBAura1.Text == "")
                TBAura1.Text = "0";
            if (TBItem1.Text == "")
                TBItem1.Text = "0";
            if (TBQuest11.Text == "")
                TBQuest11.Text = "0";
            if (TBQuest12.Text == "")
                TBQuest12.Text = "0";
            if (TBQuest13.Text == "")
                TBQuest13.Text = "0";

            if (TBAura2.Text == "")
                TBAura2.Text = "0";
            if (TBItem2.Text == "")
                TBItem2.Text = "0";
            if (TBQuest21.Text == "")
                TBQuest21.Text = "0";
            if (TBQuest22.Text == "")
                TBQuest22.Text = "0";
            if (TBQuest23.Text == "")
                TBQuest23.Text = "0";

            if (TBAura3.Text == "")
                TBAura3.Text = "0";
            if (TBItem3.Text == "")
                TBItem3.Text = "0";
            if (TBQuest31.Text == "")
                TBQuest31.Text = "0";
            if (TBQuest32.Text == "")
                TBQuest32.Text = "0";
            if (TBQuest33.Text == "")
                TBQuest33.Text = "0";

            if (TBAura4.Text == "")
                TBAura4.Text = "0";
            if (TBItem4.Text == "")
                TBItem4.Text = "0";
            if (TBQuest41.Text == "")
                TBQuest41.Text = "0";
            if (TBQuest42.Text == "")
                TBQuest42.Text = "0";
            if (TBQuest43.Text == "")
                TBQuest43.Text = "0";

            if (TBAura5.Text == "")
                TBAura5.Text = "0";
            if (TBItem5.Text == "")
                TBItem5.Text = "0";
            if (TBQuest51.Text == "")
                TBQuest51.Text = "0";
            if (TBQuest52.Text == "")
                TBQuest52.Text = "0";
            if (TBQuest53.Text == "")
                TBQuest53.Text = "0";

            if (TBAura6.Text == "")
                TBAura6.Text = "0";
            if (TBItem6.Text == "")
                TBItem6.Text = "0";
            if (TBQuest61.Text == "")
                TBQuest61.Text = "0";
            if (TBQuest62.Text == "")
                TBQuest62.Text = "0";
            if (TBQuest63.Text == "")
                TBQuest63.Text = "0";

            if (CB1.Checked && TBAura1.Text == "0")
            {
                Logging.Write("ItemForAura: Line 1: Please insert a Aura ID or Pluginpart wont work.");
                CB1.Checked = false;
            }

            if (CB2.Checked && TBAura2.Text == "0")
            {
                Logging.Write("ItemForAura: Line 2: Please insert a Aura ID or Pluginpart wont work.");
                CB2.Checked = false;
            }
            if (CB3.Checked && TBAura3.Text == "0")
            {
                Logging.Write("ItemForAura: Line 3: Please insert a Aura ID or Pluginpart wont work.");
                CB3.Checked = false;
            }
            if (CB4.Checked && TBAura4.Text == "0")
            {
                Logging.Write("ItemForAura: Line 4: Please insert a Aura ID or Pluginpart wont work.");
                CB4.Checked = false;
            }
            if (CB5.Checked && TBAura5.Text == "0")
            {
                Logging.Write("ItemForAura: Line 5: Please insert a Aura ID or Pluginpart wont work.");
                CB5.Checked = false;
            }
            if (CB6.Checked && TBAura6.Text == "0")
            {
                Logging.Write("ItemForAura: Line 6: Please insert a Aura ID or Pluginpart wont work.");
                CB6.Checked = false;
            }

            if (CB1.Checked && TBItem1.Text == "0")
            {
                Logging.Write("ItemForAura: Line 1: Please insert a Item ID or Pluginpart wont work.");
                CB1.Checked = false;
            }
            if (CB2.Checked && TBItem2.Text == "0")
            {
                Logging.Write("ItemForAura: Line 2: Please insert a Item ID or Pluginpart wont work.");
                CB2.Checked = false;
            }
            if (CB3.Checked && TBItem3.Text == "0")
            {
                Logging.Write("ItemForAura: Line 3: Please insert a Item ID or Pluginpart wont work.");
                CB3.Checked = false;
            }
            if (CB4.Checked && TBItem4.Text == "0")
            {
                Logging.Write("ItemForAura: Line 4: Please insert a Item ID or Pluginpart wont work.");
                CB4.Checked = false;
            }
            if (CB5.Checked && TBItem5.Text == "0")
            {
                Logging.Write("ItemForAura: Line 5: Please insert a Item ID or Pluginpart wont work.");
                CB5.Checked = false;
            }
            if (CB6.Checked && TBItem6.Text == "0")
            {
                Logging.Write("ItemForAura: Line 6: Please insert a Item ID or Pluginpart wont work.");
                CB6.Checked = false;
            }

            ItemForAura.Settings.Aura1 = TBAura1.Text;
            ItemForAura.Settings.Item1 = TBItem1.Text;
            ItemForAura.Settings.Quest11 = TBQuest11.Text;
            ItemForAura.Settings.Quest12 = TBQuest12.Text;
            ItemForAura.Settings.Quest13 = TBQuest13.Text;
            ItemForAura.Settings.Combat1 = CBCombat1.Checked;

            ItemForAura.Settings.Aura2 = TBAura2.Text;
            ItemForAura.Settings.Item2 = TBItem2.Text;
            ItemForAura.Settings.Quest21 = TBQuest21.Text;
            ItemForAura.Settings.Quest22 = TBQuest22.Text;
            ItemForAura.Settings.Quest23 = TBQuest23.Text;
            ItemForAura.Settings.Combat2 = CBCombat2.Checked;

            ItemForAura.Settings.Aura3 = TBAura3.Text;
            ItemForAura.Settings.Item3 = TBItem3.Text;
            ItemForAura.Settings.Quest31 = TBQuest31.Text;
            ItemForAura.Settings.Quest32 = TBQuest32.Text;
            ItemForAura.Settings.Quest33 = TBQuest33.Text;
            ItemForAura.Settings.Combat3 = CBCombat3.Checked;

            ItemForAura.Settings.Aura4 = TBAura4.Text;
            ItemForAura.Settings.Item4 = TBItem4.Text;
            ItemForAura.Settings.Quest41 = TBQuest41.Text;
            ItemForAura.Settings.Quest42 = TBQuest42.Text;
            ItemForAura.Settings.Quest43 = TBQuest43.Text;
            ItemForAura.Settings.Combat4 = CBCombat4.Checked;

            ItemForAura.Settings.Aura5 = TBAura5.Text;
            ItemForAura.Settings.Item5 = TBItem5.Text;
            ItemForAura.Settings.Quest51 = TBQuest51.Text;
            ItemForAura.Settings.Quest52 = TBQuest52.Text;
            ItemForAura.Settings.Quest53 = TBQuest53.Text;
            ItemForAura.Settings.Combat5 = CBCombat5.Checked;

            ItemForAura.Settings.Aura6 = TBAura6.Text;
            ItemForAura.Settings.Item6 = TBItem6.Text;
            ItemForAura.Settings.Quest61 = TBQuest61.Text;
            ItemForAura.Settings.Quest62 = TBQuest62.Text;
            ItemForAura.Settings.Quest63 = TBQuest63.Text;
            ItemForAura.Settings.Combat6 = CBCombat6.Checked;

            string File = "Plugins\\ItemForAuraQuesthelper\\";
            Logging.Write("ItemForAura: SettingsSaved!");

            XmlDocument xml;
            XmlElement root;
            XmlElement element;
            XmlText text;
            XmlComment xmlComment;

            string sPath = Process.GetCurrentProcess().MainModule.FileName;
            sPath = Path.GetDirectoryName(sPath);
            sPath = Path.Combine(sPath, File);

            if (!Directory.Exists(sPath))
            {
                Logging.WriteDebug("ItemForAura: Creating config directory");
                Directory.CreateDirectory(sPath);
            }

            sPath = Path.Combine(sPath, "ItemForAura.config");

            Logging.WriteDebug("ItemForAura: Saving config file: {0}", sPath);
            xml = new XmlDocument();
            XmlDeclaration dc = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(dc);

            xmlComment = xml.CreateComment(
                "=======================================================================\n" +
                ".CONFIG  -  This is the Config File For Item for Aura - Questhelper\n\n" +
                "XML file containing settings to customize in the Item for Aura - Questhelper Plugin\n" +
                "It is STRONGLY recommended you use the Configuration UI to change this\n" +
                "file instead of direct changein it here.\n" +
                "========================================================================");

            //let's add the root element
            root = xml.CreateElement("ItemForAura");
            root.AppendChild(xmlComment);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active1");
            text = xml.CreateTextNode(CB1.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Active2");
            text = xml.CreateTextNode(CB2.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Active3");
            text = xml.CreateTextNode(CB3.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Active4");
            text = xml.CreateTextNode(CB4.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Active5");
            text = xml.CreateTextNode(CB5.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Active6");
            text = xml.CreateTextNode(CB6.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Item1");
            text = xml.CreateTextNode(TBItem1.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Item2");
            text = xml.CreateTextNode(TBItem2.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Item3");
            text = xml.CreateTextNode(TBItem3.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Item4");
            text = xml.CreateTextNode(TBItem4.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Item5");
            text = xml.CreateTextNode(TBItem5.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Item6");
            text = xml.CreateTextNode(TBItem6.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Aura1");
            text = xml.CreateTextNode(TBAura1.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Aura2");
            text = xml.CreateTextNode(TBAura2.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Aura3");
            text = xml.CreateTextNode(TBAura3.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Aura4");
            text = xml.CreateTextNode(TBAura4.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Aura5");
            text = xml.CreateTextNode(TBAura5.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Aura6");
            text = xml.CreateTextNode(TBAura6.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Quest11");
            text = xml.CreateTextNode(TBQuest11.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest21");
            text = xml.CreateTextNode(TBQuest21.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest31");
            text = xml.CreateTextNode(TBQuest31.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest41");
            text = xml.CreateTextNode(TBQuest41.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest51");
            text = xml.CreateTextNode(TBQuest51.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest61");
            text = xml.CreateTextNode(TBQuest61.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);

            //let's add another element (child of the root)
            element = xml.CreateElement("Quest12");
            text = xml.CreateTextNode(TBQuest12.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest22");
            text = xml.CreateTextNode(TBQuest22.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest32");
            text = xml.CreateTextNode(TBQuest32.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest42");
            text = xml.CreateTextNode(TBQuest42.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest52");
            text = xml.CreateTextNode(TBQuest52.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest62");
            text = xml.CreateTextNode(TBQuest62.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);

            //let's add another element (child of the root)
            element = xml.CreateElement("Quest13");
            text = xml.CreateTextNode(TBQuest13.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest23");
            text = xml.CreateTextNode(TBQuest23.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest33");
            text = xml.CreateTextNode(TBQuest33.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest43");
            text = xml.CreateTextNode(TBQuest43.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest53");
            text = xml.CreateTextNode(TBQuest53.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);
            //let's add another element (child of the root)
            element = xml.CreateElement("Quest63");
            text = xml.CreateTextNode(TBQuest63.Text.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            xml.AppendChild(root);

            //let's add another element (child of the root)
            element = xml.CreateElement("Combat1");
            text = xml.CreateTextNode(CBCombat1.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);
            //let's add another element (child of the root)
            element = xml.CreateElement("Combat2");
            text = xml.CreateTextNode(CBCombat2.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Combat3");
            text = xml.CreateTextNode(CBCombat3.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Combat4");
            text = xml.CreateTextNode(CBCombat4.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Combat5");
            text = xml.CreateTextNode(CBCombat5.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);            //let's add another element (child of the root)
            element = xml.CreateElement("Combat6");
            text = xml.CreateTextNode(CBCombat6.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            System.IO.FileStream fs = new System.IO.FileStream(@sPath, System.IO.FileMode.Create,
                                                               System.IO.FileAccess.Write);
            try
            {
                xml.Save(fs);
                fs.Close();
            }
            catch (Exception np)
            {
                Logging.Write(np.Message);
            }

        }
    }
}
