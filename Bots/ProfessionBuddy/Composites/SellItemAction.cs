using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using HighVoltz.Dynamic;
using Styx;
using Styx.Database;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{
    internal sealed class SellItemAction : PBAction
    {
        #region SellItemActionType enum

        public enum SellItemActionType
        {
            Specific,
            Greys,
            Whites,
            Greens,
        }

        #endregion

        private uint _entry;
        private WoWPoint _loc;

        public SellItemAction()
        {
            Properties["Location"] = new MetaProp("Location",
                                                  typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Properties["NpcEntry"] = new MetaProp("NpcEntry", typeof (uint),
                                                  new EditorAttribute(typeof (PropertyBag.EntryEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_NpcEntry"]));

            Properties["ItemID"] = new MetaProp("ItemID", typeof (string),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntry"]));

            Properties["Count"] = new MetaProp("Count", typeof (DynamicProperty<int>),
                                               new TypeConverterAttribute(
                                                   typeof (DynamicProperty<int>.DynamivExpressionConverter)),
                                               new DisplayNameAttribute(Pb.Strings["Action_Common_Count"]));

            Properties["SellItemType"] = new MetaProp("SellItemType", typeof (SellItemActionType),
                                                      new DisplayNameAttribute(
                                                          Pb.Strings["Action_SellItemAction_SellItemType"]));

            Properties["Sell"] = new MetaProp("Sell", typeof (DepositWithdrawAmount),
                                              new DisplayNameAttribute(Pb.Strings["Action_Common_Sell"]));

            ItemID = "";
            Count = new DynamicProperty<int>(this, "0");
            RegisterDynamicProperty("Count");
            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            NpcEntry = 0u;
            Sell = DepositWithdrawAmount.All;

            Properties["Location"].PropertyChanged += LocationChanged;
            Properties["SellItemType"].Value = SellItemActionType.Specific;
            Properties["SellItemType"].PropertyChanged += SellItemActionPropertyChanged;
            Properties["Sell"].PropertyChanged += SellChanged;
        }

        [PbXmlAttribute]
        public DepositWithdrawAmount Sell
        {
            get { return (DepositWithdrawAmount) Properties["Sell"].Value; }
            set { Properties["Sell"].Value = value; }
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
        public SellItemActionType SellItemType
        {
            get { return (SellItemActionType) Properties["SellItemType"].Value; }
            set { Properties["SellItemType"].Value = value; }
        }

        [PbXmlAttribute]
        public string ItemID
        {
            get { return (string) Properties["ItemID"].Value; }
            set { Properties["ItemID"].Value = value; }
        }

        [PbXmlAttribute]
        [TypeConverter(typeof (DynamicProperty<int>.DynamivExpressionConverter))]
        public DynamicProperty<int> Count
        {
            get { return (DynamicProperty<int>) Properties["Count"].Value; }
            set { Properties["Count"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_SellItemAction_Name"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("({0}) " +
                                     (SellItemType == SellItemActionType.Specific
                                          ? ItemID.ToString(CultureInfo.InvariantCulture) + " x{1} "
                                          : SellItemType.ToString()), Name, Count);
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_SellItemAction_Help"]; }
        }

        private void SellChanged(object sender, MetaPropArgs e)
        {
            Properties["Sell"].Show = Sell == DepositWithdrawAmount.Amount;
            RefreshPropertyGrid();
        }

        private void LocationChanged(object sender, MetaPropArgs e)
        {
            _loc = Util.StringToWoWPoint((string) ((MetaProp) sender).Value);
            Properties["Location"].PropertyChanged -= LocationChanged;
            Properties["Location"].Value = string.Format("{0}, {1}, {2}", _loc.X, _loc.Y, _loc.Z);
            Properties["Location"].PropertyChanged += LocationChanged;
            RefreshPropertyGrid();
        }

        private void SellItemActionPropertyChanged(object sender, MetaPropArgs e)
        {
            switch (SellItemType)
            {
                case SellItemActionType.Specific:
                    Properties["Count"].Show = true;
                    Properties["ItemID"].Show = true;
                    break;
                default:
                    Properties["Count"].Show = false;
                    Properties["ItemID"].Show = false;
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
                    if (_entry == 0)
                        _entry = NpcEntry;
                    if (_entry == 0)
                    {
                        MoveToAction.GetLocationFromDB(MoveToAction.MoveToType.NearestVendor, 0);
                        NpcResult npcResults = NpcQueries.GetNearestNpc(ObjectManager.Me.FactionTemplate,
                                                                        ObjectManager.Me.MapId,
                                                                        ObjectManager.Me.Location, UnitNPCFlags.Vendor);
                        _entry = (uint) npcResults.Entry;
                        movetoPoint = npcResults.Location;
                    }
                    WoWUnit unit = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == _entry).
                        OrderBy(o => o.Distance).FirstOrDefault();
                    if (unit != null)
                        movetoPoint = unit.Location;
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
                                return RunStatus.Success;
                            }
                        }
                    }
                }
                else
                {
                    if (SellItemType == SellItemActionType.Specific)
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
                        List<WoWItem> itemList = ObjectManager.Me.BagItems.Where(u => idList.Contains(u.Entry)).
                            Take(Sell == DepositWithdrawAmount.All ? int.MaxValue : Count).ToList();
                        using (new FrameLock())
                        {
                            foreach (WoWItem item in itemList)
                                item.UseContainerItem();
                        }
                    }
                    else
                    {
                        List<WoWItem> itemList = null;
                        IEnumerable<WoWItem> itemQuery = from item in Me.BagItems
                                                         where !Pb.ProtectedItems.Contains(item.Entry)
                                                         select item;
                        switch (SellItemType)
                        {
                            case SellItemActionType.Greys:
                                itemList = itemQuery.Where(i => i.Quality == WoWItemQuality.Poor).ToList();
                                break;
                            case SellItemActionType.Whites:
                                itemList = itemQuery.Where(i => i.Quality == WoWItemQuality.Common).ToList();
                                break;
                            case SellItemActionType.Greens:
                                itemList = itemQuery.Where(i => i.Quality == WoWItemQuality.Uncommon).ToList();
                                break;
                        }
                        if (itemList != null)
                        {
                            using (new FrameLock())
                            {
                                foreach (WoWItem item in itemList)
                                {
                                    item.UseContainerItem();
                                }
                            }
                        }
                    }
                    Professionbuddy.Log("SellItemAction Completed for {0}", ItemID);
                    IsDone = true;
                }
                return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        public override void Reset()
        {
            base.Reset();
            _entry = 0;
        }

        public override object Clone()
        {
            return new SellItemAction
                       {
                           Count = Count,
                           ItemID = ItemID,
                           SellItemType = SellItemType,
                           NpcEntry = NpcEntry,
                           Location = Location,
                           Sell = Sell
                       };
        }
    }
}