using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MrItemRemover;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.IO;

namespace MrItemRemover
{
    public partial class ItemRemoverUI : Form
    {
        MrItemRemover MIRBase = new MrItemRemover();
        public static void slog(string format, params object[] args)
        { Logging.Write(Color.Red, "[Mr.ItemRemover]:" + format, args); }
        public ItemRemoverUI()
        {
            InitializeComponent();
        }
        private string GoldImangePathName = Path.Combine(Logging.ApplicationPath, string.Format(@"Plugins/MrItemRemover/Gold2.bmp"));
        private string refreshImangePathName = Path.Combine(Logging.ApplicationPath, string.Format(@"Plugins/MrItemRemover/ref.bmp"));
        private void ItemRemoverUI_Load(object sender, EventArgs e)
        {
            MIRBase.MIRLoad();
            Bitmap GoldImg = new Bitmap(GoldImangePathName);
            Bitmap refresh = new Bitmap(refreshImangePathName);
            GoldBox.Image = GoldImg;
            resf.Image = refresh;
            GrayItems.Checked = MIRsettings.Instance.GrayItems;
            SellGray.Checked = MIRsettings.Instance.SellGray;
            SellWhite.Checked = MIRsettings.Instance.SellWhite;
            SellGreen.Checked = MIRsettings.Instance.SellGreen;
            EnableRemove.Checked = MIRsettings.Instance.EnableRemove;
            EnableSell.Checked = MIRsettings.Instance.EnableSell;
            Qitem.Checked = MIRsettings.Instance.QuestItems;
            GoldGrays.Text = MIRsettings.Instance.GoldGrays.ToString();
            SilverGrays.Text = MIRsettings.Instance.SilverGrays.ToString();
            CopperGrays.Text = MIRsettings.Instance.CopperGrays.ToString();
            MinPass.Text = MIRsettings.Instance.MinPass.ToString();
            foreach(string itm in MIRBase._ItemName)
            {
                RemoveList.Items.Add(itm);
            }
            foreach (string itm in MIRBase._ItemNameSell)
            {
                SellList.Items.Add(itm);
            }
            foreach(WoWItem BagItem in ObjectManager.GetObjectsOfType<WoWItem>(false))
            {
                if (BagItem.BagSlot != -1 && !CurinList.Items.Contains(new ListViewItem(BagItem.Name)))
                {
                    ListViewItem Item = new ListViewItem(BagItem.Name);
                    Item.Tag = BagItem;
                    if (BagItem.Quality == Styx.WoWItemQuality.Common)
                    {
                        Item.ForeColor = Color.White;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Poor)
                    {
                        Item.ForeColor = Color.Gray;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Epic)
                    {
                        Item.ForeColor = Color.Purple;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Rare)
                    {
                        Item.ForeColor = Color.RoyalBlue;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Legendary)
                    {
                        Item.ForeColor = Color.Orange;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Heirloom)
                    {
                        Item.ForeColor = Color.Goldenrod;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Uncommon)
                    {
                        Item.ForeColor = Color.LimeGreen;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Artifact)
                    {
                        Item.ForeColor = Color.Goldenrod;
                    }
                    Item.SubItems.Add(BagItem.StackCount.ToString());
                    Item.SubItems.Add(BagItem.IsSoulbound.ToString());
                    CurinList.Items.Add(Item);
                    
                    
                }
            }
        }

        private void RemoveList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RSI_Click(object sender, EventArgs e)
        {
            if (RemoveList.SelectedItem != null)
            {
                slog("{0} Removed", RemoveList.SelectedItem.ToString());
                MIRBase._ItemName.Remove(RemoveList.SelectedItem.ToString());
                RemoveList.Items.Remove(RemoveList.SelectedItem);
            }
        }

        private void AdditemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddItem_Click(object sender, EventArgs e)
        {
            if(AdditemName.Text != null)
            {
                RemoveList.Items.Add(AdditemName.Text);
                MIRBase._ItemName.Add(AdditemName.Text);
                
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            MIRBase.MIRSave();
            MIRsettings.Instance.Save();
        }

        private void CurInventory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddToRL_Click(object sender, EventArgs e)
        {
            if (CurinList.SelectedItems[0] != null)
            {
                RemoveList.Items.Add(CurinList.SelectedItems[0].Text);
                MIRBase._ItemName.Add(CurinList.SelectedItems[0].Text);
            }
        }

        private void GrayItems_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.GrayItems = GrayItems.Checked;
        }

        private void GoldGrays_TextChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.GoldGrays = GoldGrays.Text.ToInt32();
        }

        private void MinPass_TextChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.MinPass = MinPass.Text.ToInt32();
        }

        private void Qitem_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.QuestItems = Qitem.Checked;
        }

        private void CurinList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RCK_Click(object sender, EventArgs e)
        {
            slog("Item Check was Run Manualy");
            MIRBase.CheckForItems();
        }

        private void SilverGrays_TextChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.SilverGrays = SilverGrays.Text.ToInt32();
        }

        private void CopperGrays_TextChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.CopperGrays = CopperGrays.Text.ToInt32();
        }

   

        private void refresh_Click(object sender, EventArgs e)
        {
            CurinList.Items.Clear();
            ObjectManager.Update();
            foreach (WoWItem BagItem in Styx.StyxWoW.Me.BagItems)
            {
                if (BagItem.BagSlot != -1 && !CurinList.Items.Contains(new ListViewItem(BagItem.Name)))
                {
                    ListViewItem Item = new ListViewItem(BagItem.Name);
                    Item.Tag = BagItem;
                    if (BagItem.Quality == Styx.WoWItemQuality.Common)
                    {
                        Item.ForeColor = Color.White;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Poor)
                    {
                        Item.ForeColor = Color.Gray;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Epic)
                    {
                        Item.ForeColor = Color.Purple;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Rare)
                    {
                        Item.ForeColor = Color.RoyalBlue;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Legendary)
                    {
                        Item.ForeColor = Color.Orange;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Heirloom)
                    {
                        Item.ForeColor = Color.Goldenrod;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Uncommon)
                    {
                        Item.ForeColor = Color.LimeGreen;
                    }
                    if (BagItem.Quality == Styx.WoWItemQuality.Artifact)
                    {
                        Item.ForeColor = Color.Goldenrod;
                    }
                    Item.SubItems.Add(BagItem.StackCount.ToString());
                    Item.SubItems.Add(BagItem.IsSoulbound.ToString());
                    CurinList.Items.Add(Item);


                }
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void RemoveSellItem_Click(object sender, EventArgs e)
        {
            if (SellList.SelectedItem != null)
            {
                slog("{0} Removed", SellList.SelectedItem.ToString());
                MIRBase._ItemName.Remove(SellList.SelectedItem.ToString());
                SellList.Items.Remove(SellList.SelectedItem);
            }
        }

        private void AddToSL_Click(object sender, EventArgs e)
        {
            if (CurinList.SelectedItems[0] != null)
            {
                SellList.Items.Add(CurinList.SelectedItems[0].Text);
                MIRBase._ItemNameSell.Add(CurinList.SelectedItems[0].Text);
            }
        }

        private void EnableRemove_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.EnableRemove = EnableRemove.Checked;
        }

        private void EnableSell_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.EnableSell = EnableSell.Checked;
        }

        private void SellGray_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.SellGray = SellGray.Checked;
        }

        private void SellGreen_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.SellGreen = SellGreen.Checked;
        }

        private void SellWhite_CheckedChanged(object sender, EventArgs e)
        {
            MIRsettings.Instance.SellWhite = SellWhite.Checked;
        }



  






    }
}
