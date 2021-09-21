using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using HighVoltz.Dynamic;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{
    internal sealed class BuyItemAction : PBAction
    {
        #region BuyItemActionType enum

        public enum BuyItemActionType
        {
            SpecificItem,
            Material,
        }

        #endregion

        private const string BuyItemFormat =
            "local amount={1} " +
            "local id={0} " +
            "local stackSize " +
            "local index = -1 " +
            "local quantity " +
            "for i=1,GetMerchantNumItems() do " +
            "local link=GetMerchantItemLink(i) " +
            "if link then if link:find(id) then " +
            "index=i " +
            "stackSize=GetMerchantItemMaxStack(i) " +
            "end " +
            "end " +
            "end " +
            "if index == -1 then return -1 end " +
            "while amount>0 do " +
            "if amount>=stackSize then " +
            "quantity=stackSize " +
            "else " +
            "quantity=amount " +
            "end " +
            "BuyMerchantItem(index, quantity) " +
            "amount=amount-quantity " +
            "end " +
            "return 1 ";

        private Stopwatch _concludingSw = new Stopwatch();
        // add pause at the end to give objectmanager a chance to update.

        private WoWPoint _loc;

        public BuyItemAction()
        {
            Properties["Location"] = new MetaProp("Location", typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Properties["NpcEntry"] = new MetaProp("NpcEntry", typeof (uint),
                                                  new EditorAttribute(typeof (PropertyBag.EntryEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_NpcEntry"]));

            Properties["ItemID"] = new MetaProp("ItemID", typeof (string),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntries"]));

            Properties["Count"] = new MetaProp("Count", typeof (DynamicProperty<int>),
                                               new TypeConverterAttribute(
                                                   typeof (DynamicProperty<int>.DynamivExpressionConverter)),
                                               new DisplayNameAttribute(Pb.Strings["Action_Common_Count"]));

            Properties["BuyItemType"] = new MetaProp("BuyItemType", typeof (BuyItemActionType),
                                                     new DisplayNameAttribute(Pb.Strings["Action_Common_Buy"]));

            Properties["BuyAdditively"] = new MetaProp("BuyAdditively", typeof (bool),
                                                       new DisplayNameAttribute(
                                                           Pb.Strings["Action_Common_BuyAdditively"]));
            ItemID = "";
            Count = new DynamicProperty<int>(this, "1"); // dynamic expression
            RegisterDynamicProperty("Count");
            BuyItemType = BuyItemActionType.Material;
            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            NpcEntry = 0u;
            BuyAdditively = true;

            Properties["ItemID"].Show = false;
            Properties["Count"].Show = false;
            Properties["BuyAdditively"].Show = false;
            Properties["Location"].PropertyChanged += LocationChanged;
            Properties["BuyItemType"].PropertyChanged += BuyItemActionPropertyChanged;
        }

        [PbXmlAttribute]
        public uint NpcEntry
        {
            get { return (uint) Properties["NpcEntry"].Value; }
            set { Properties["NpcEntry"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string) Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        [PbXmlAttribute]
        [PbXmlAttribute("Entry")]
        public string ItemID
        {
            get { return (string) Properties["ItemID"].Value; }
            set { Properties["ItemID"].Value = value; }
        }

        [PbXmlAttribute]
        public BuyItemActionType BuyItemType
        {
            get { return (BuyItemActionType) Properties["BuyItemType"].Value; }
            set { Properties["BuyItemType"].Value = value; }
        }

        [PbXmlAttribute]
        [TypeConverter(typeof (DynamicProperty<int>.DynamivExpressionConverter))]
        public DynamicProperty<int> Count
        {
            get { return (DynamicProperty<int>) Properties["Count"].Value; }
            set { Properties["Count"].Value = value; }
        }

        [PbXmlAttribute]
        public bool BuyAdditively
        {
            get { return (bool) Properties["BuyAdditively"].Value; }
            set { Properties["BuyAdditively"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_BuyItemAction_Name"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("{0}: " + (BuyItemType == BuyItemActionType.SpecificItem ? "{1} x{2}" : "{3}"),
                                     Name, ItemID, Count, Pb.Strings["Action_Common_Material"]);
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_BuyItemAction_Help"]; }
        }

        private void LocationChanged(object sender, MetaPropArgs e)
        {
            _loc = Util.StringToWoWPoint((string) ((MetaProp) sender).Value);
            Properties["Location"].PropertyChanged -= LocationChanged;
            Properties["Location"].Value = string.Format("{0}, {1}, {2}", _loc.X, _loc.Y, _loc.Z);
            Properties["Location"].PropertyChanged += LocationChanged;
            RefreshPropertyGrid();
        }

        private void BuyItemActionPropertyChanged(object sender, MetaPropArgs e)
        {
            switch (BuyItemType)
            {
                case BuyItemActionType.Material:
                    Properties["ItemID"].Show = false;
                    Properties["Count"].Show = false;
                    Properties["BuyAdditively"].Show = false;
                    break;
                case BuyItemActionType.SpecificItem:
                    Properties["ItemID"].Show = true;
                    Properties["Count"].Show = true;
                    Properties["BuyAdditively"].Show = true;
                    break;
            }
            RefreshPropertyGrid();
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if (MerchantFrame.Instance == null || !MerchantFrame.Instance.IsVisible)
                {
                    WoWPoint movetoPoint = _loc;
                    WoWUnit unit = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == NpcEntry).
                        OrderBy(o => o.Distance).FirstOrDefault();
                    if (unit != null)
                        movetoPoint = WoWMathHelper.CalculatePointFrom(Me.Location, unit.Location, 3);
                    else if (movetoPoint == WoWPoint.Zero)
                        movetoPoint = MoveToAction.GetLocationFromDB(MoveToAction.MoveToType.NpcByID, NpcEntry);
                    if (movetoPoint != WoWPoint.Zero && ObjectManager.Me.Location.Distance(movetoPoint) > 4.5)
                    {
                        Util.MoveTo(movetoPoint);
                    }
                    else if (unit != null)
                    {
                        unit.Target();
                        unit.Interact();
                    }
                    if (GossipFrame.Instance != null && GossipFrame.Instance.IsVisible &&
                        GossipFrame.Instance.GossipOptionEntries != null)
                    {
                        foreach (GossipEntry ge in GossipFrame.Instance.GossipOptionEntries)
                        {
                            if (ge.Type == GossipEntry.GossipEntryType.Vendor)
                            {
                                GossipFrame.Instance.SelectGossipOption(ge.Index);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // check if we have merchant frame open at correct NPC
                    if (NpcEntry > 0 && Me.GotTarget && Me.CurrentTarget.Entry != NpcEntry)
                    {
                        MerchantFrame.Instance.Close();
                        return RunStatus.Success;
                    }
                    if (!_concludingSw.IsRunning)
                    {
                        if (BuyItemType == BuyItemActionType.SpecificItem)
                        {
                            var idList = new List<uint>();
                            string[] entries = ItemID.Split(',');
                            if (entries.Length > 0)
                            {
                                foreach (string entry in entries)
                                {
                                    uint temp;
                                    uint.TryParse(entry.Trim(), out temp);
                                    idList.Add(temp);
                                }
                            }
                            else
                            {
                                Professionbuddy.Err(Pb.Strings["Error_NoItemEntries"]);
                                IsDone = true;
                                return RunStatus.Failure;
                            }
                            foreach (uint id in idList)
                            {
                                int count = !BuyAdditively ? Count - Util.GetCarriedItemCount(id) : Count;
                                if (count > 0)
                                    BuyItem(id, (uint) count);
                            }
                        }
                        else if (BuyItemType == BuyItemActionType.Material)
                        {
                            foreach (var kv in Pb.MaterialList)
                            {
                                // only buy items if we don't have enough in bags...
                                int amount = kv.Value - (int) Ingredient.GetInBagItemCount(kv.Key);
                                if (amount > 0)
                                    BuyItem(kv.Key, (uint) amount);
                            }
                        }
                        _concludingSw.Start();
                    }
                    if (_concludingSw.ElapsedMilliseconds >= 2000)
                    {
                        Professionbuddy.Log("BuyItemAction Completed");
                        IsDone = true;
                    }
                }
                if (!IsDone)
                    return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        // Credits to Inrego
        // Index are {0}=ItemID, {1}=Amount
        // returns 1 if item is found, otherwise -1

        public static void BuyItem(uint id, uint count)
        {
            //bool found = false;
            //foreach (MerchantItem mi in MerchantFrame.Instance.GetAllMerchantItems())
            //{
            //    if (mi.ItemId == id)
            //    {
            //        // since BuyItem can only by up to 20 items we need to run it multiple times when buying over 20 items
            //        var stacks = (int)(count / 20);
            //        var leftovers = (int)(count % 20);
            //        if (count >= 20)
            //        {
            //            //using (new FrameLock()) // framelock was causing DCs
            //            //{
            //            for (int i = 0; i < stacks; i++)
            //                MerchantFrame.Instance.BuyItem(mi.Index, 20);
            //            if (leftovers > 0)
            //                MerchantFrame.Instance.BuyItem(mi.Index, leftovers);
            //            //}
            //        }
            //        else
            //            MerchantFrame.Instance.BuyItem(mi.Index, leftovers);
            //        found = true;
            //        break;
            //    }
            //}
            string lua = string.Format(BuyItemFormat, id, count);
            bool found = Lua.GetReturnVal<int>(lua, 0) == 1;
            Professionbuddy.Log("item {0} {1}", id, found ? "bought " : "not found");
        }

        public override void Reset()
        {
            base.Reset();
            _concludingSw = new Stopwatch();
        }

        public override object Clone()
        {
            return new BuyItemAction
                       {
                           Count = Count,
                           ItemID = ItemID,
                           BuyItemType = BuyItemType,
                           Location = Location,
                           NpcEntry = NpcEntry,
                           BuyAdditively = BuyAdditively
                       };
        }
    }
}