//Mr.ItemRemover - Created by CodenameGamma - 1-31-11 - For WoW Version 4.0.3
//www.honorbuddy.de
//this is a free plugin, and should not be sold, or repackaged.
//Donations Accepted. 
//Version 1.4


using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MrItemRemover
{
    using Styx.Logic;
    using System;
    using Styx.Helpers;
    using Styx.Logic.Pathing;
    using System.Threading;
    using System.Diagnostics;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;
    using System.Net;
    using Styx.Plugins.PluginClass;
    using Styx;
    using System.Xml;

    public class MrItemRemover : HBPlugin
    {
        //The ONLY Setting in this CC, turning this on will make the bot get rid of all Poor Quality items.
        //in addition to whats listed inside ItemNameList.txt. 
        public bool DeleteAllGray = MIRsettings.Instance.GrayItems;
        public bool DeleteQuestItems = MIRsettings.Instance.QuestItems;



        public static void slog(string format, params object[] args)
        { Logging.Write(Color.Red, "[Mr.ItemRemover]:" + format, args); }
        private static readonly LocalPlayer Me = ObjectManager.Me;


        //My Crappy Initalise.
        public void Inital()
        {
            Lua.Events.AttachEvent("DELETE_ITEM_CONFIRM", DeleteItemConfirmPopup);
            Lua.Events.AttachEvent("MERCHANT_SHOW", SellVenderItems);
            slog("Loading Item names.");
            MIRLoad();
            MIRsettings.Instance.Load();
        
        }
        public void SellVenderItems(object sender, LuaEventArgs args)
        {
            if (Styx.Logic.Inventory.Frames.Merchant.MerchantFrame.Instance.IsVisible && MIRsettings.Instance.EnableSell)
            {
                foreach (WoWItem Items in Me.BagItems)
                {
                    if (!Items.IsSoulbound)
                    {
                        if (Items.Quality == WoWItemQuality.Poor && MIRsettings.Instance.SellGray)
                        {
                            slog("Selling Gray Item {0}", Items.Name);
                            Items.UseContainerItem();
                        }
                        if (Items.Quality == WoWItemQuality.Common && MIRsettings.Instance.SellWhite)
                        {
                            slog("Selling White Item {0}", Items.Name);
                            Items.UseContainerItem();
                        }
                        if (Items.Quality == WoWItemQuality.Uncommon && MIRsettings.Instance.SellGreen)
                        {
                            slog("Selling Green Item {0}", Items.Name);
                            Items.UseContainerItem();
                        }
                        if (_ItemNameSell.Contains(Items.Name))
                        {
                            slog("Item Matched List Selling {0}", Items.Name);
                            Items.UseContainerItem();
                        }
                    }
                }

            }
        }
        private static void DeleteItemConfirmPopup(object sender, LuaEventArgs args)
        {
            string ItemNamePopUp = args.Args[0].ToString();

            if (Me.CurrentTarget != null)
            {
                slog("Clicking Yes to Comfirm {0}'s Removal From Inventory", ItemNamePopUp);
                Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");");
            }


        }


        public bool init;
        Stopwatch CheckTimer = new Stopwatch();
        public override void Pulse()
        {
            if (init == null || init == false)
            {
                Inital();
                init = true;

            }

            if (!Me.Combat && !Me.IsCasting && !Me.Dead && !Me.IsGhost)
            {
                //Will Run First Pulse, then every 3 mintues if not in combat. 
                if (!CheckTimer.IsRunning || CheckTimer.Elapsed.Minutes > MIRsettings.Instance.MinPass)
                {
                    CheckTimer.Reset();
                    CheckTimer.Start();
                    if (MIRsettings.Instance.EnableRemove)
                    {
                        CheckForItems();
                    }
                }


            }

        }
        public void CheckForItems()
        {
            foreach (WoWItem item in Me.BagItems)
            {
                //if item name Matches whats in the text file / the internal list (after load)
                if (_ItemName.Contains(item.Name))
                {
                    //probally not needed, but still user could be messing with thier inventory.
                    if (item != null)
                    {
                        //filtering out all items that for some reason didnt return a bag slot. 
                       // if (item.BagSlot != -1)
                        //{

                            //Adds + 1 to the Bag Index and BagSlot. this is to offset how honorbuddy deals with these values.
                            //int bagin = item.BagIndex + 1;
                            //int bagsl = item.BagSlot + 1;
                            //Printing to the log, and Deleting the Item.
                            slog("{0} Found Removing Item", item.Name);
                            item.PickUp();
                            //Lua.DoString("PickupContainerItem(" + bagin + "," + bagsl + ")");
                            Lua.DoString("DeleteCursorItem()");
                            //a small Sleep, might not be needed. 
                            Thread.Sleep(600);
                        //}

                    }
                }
                if (DeleteQuestItems && item.ItemInfo.InternalInfo.StartQuestId != 0)
                {
                    if (item != null)
                    {
                        //if (item.BagSlot != -1)
                        //{
                            ///int bagins2 = item.BagIndex + 1;
                            //int bagssl2 = item.BagSlot + 1;
                            slog("{0}'s Began a Quest. Removing", item.Name);
                            //Lua.DoString("PickupContainerItem(" + bagins2 + "," + bagssl2 + ")");
                            item.PickUp();
                            Lua.DoString("DeleteCursorItem()");
                        //}
                    }
                }
                //Process all Gray Items if enabled. 
                if (DeleteAllGray && item.Quality == WoWItemQuality.Poor)
                {
                    if (item != null)
                    {
                        //Gold Format, goes in GXX SXX CXX 
                        string Gold = MIRsettings.Instance.GoldGrays.ToString() + MIRsettings.Instance.SilverGrays.ToString() + MIRsettings.Instance.CopperGrays.ToString();
                        if (item.BagSlot != -1 && item.ItemInfo.SellPrice <= Gold.ToInt32())
                        {
                            //int bagins = item.BagIndex + 1;
                            //int bagssl = item.BagSlot + 1;
                            slog("{0}'s Item Quality was Poor Removing.", item.Name);
                            item.PickUp();
                            //Lua.DoString("PickupContainerItem(" + bagins + "," + bagssl + ")");
                            Lua.DoString("DeleteCursorItem()");
                            Thread.Sleep(600);
                        }
                    }
                }
            }
        }

        //All items from the TXT Doc are loaded here.
        public List<string> _ItemName = new List<string>
        {

        };
        //All items from the TXT Doc are loaded here.
        public List<string> _ItemNameSell = new List<string>
        {

        };
        public List<string> _InventoryList = new List<string>
        {

        };
        //file Path for Saving and Loading. 
        private string FilePathName = Path.Combine(Logging.ApplicationPath,
                                              string.Format(@"Plugins/MrItemRemover/ItemNameRemoveList.txt"));
        private string FilePathName2 = Path.Combine(Logging.ApplicationPath,
                                           string.Format(@"Plugins/MrItemRemover/ItemNameSellList.txt"));
        
        public void MIRLoad()
        {
            //Clearing List incase something is in it.
            _ItemName.Clear();
         
            try
            {
                
                StreamReader Import = new StreamReader(Convert.ToString(FilePathName));
                
                while (Import.Peek() >= 0)
                {
                    _ItemName.Add(Convert.ToString(Import.ReadLine()));

                }
             
                //After each item is read from the txt file, its loaded into the list. 
                //then the list is gone though to tell the user whats in it. 
                foreach (string item in _ItemName)
                {
                    slog("{0} Added to Remove List", item.ToString());
                }
              

            }
            catch (Exception ex)
            {

                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }

            _ItemNameSell.Clear();
            try
            {
                StreamReader Importsell = new StreamReader(Convert.ToString(FilePathName2));
                while (Importsell.Peek() >= 0)
                {
                    _ItemNameSell.Add(Convert.ToString(Importsell.ReadLine()));

                }
                foreach (string item in _ItemNameSell)
                {
                    slog("{0} Added to Sell List", item.ToString());
                }
                        
            }
            catch (Exception ex)
            {

                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
        }
        //Not Yet Implemented (Will if this Plugin Ever gets a UI.
        public void MIRSave()
        {
            StreamWriter Write;
            
            try
            {
                Write = new StreamWriter(FilePathName);
                for (int I = 0; I < _ItemName.Count; I++)
                {
                    Write.WriteLine(Convert.ToString(_ItemName[I]));
                }
                Write.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
            //Added for Sell List
            StreamWriter Write2;
            try
            {
                Write2 = new StreamWriter(FilePathName2);
                for (int I = 0; I < _ItemNameSell.Count; I++)
                {
                    Write2.WriteLine(Convert.ToString(_ItemNameSell[I]));
                }
                Write2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex.Message));
                return;
            }
        }

        //Normal Stuff.
        public override string Name { get { return "Mr.ItemRemover"; } }
        public override string Author { get { return "CnG"; } }
        public override Version Version { get { return new Version(1, 4); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Mr.ItemRemover"; } }


        public override void OnButtonPress()
        {
            ItemRemoverUI form = new ItemRemoverUI();
            form.ShowDialog();
        }
    }
}

