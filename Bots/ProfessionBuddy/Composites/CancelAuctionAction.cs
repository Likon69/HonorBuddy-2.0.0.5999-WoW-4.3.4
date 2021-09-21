using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{
    public sealed class CancelAuctionAction : PBAction
    {
        private WoWPoint _loc;
        private bool _subCatTypeLoaded;
        private string _subCatValueString;
        private Type _subCategoryType = typeof (WoWItemTradeGoodsClass);
        private List<AuctionEntry> _toCancelItemList;
        private List<AuctionEntry> _toScanItemList;

        public CancelAuctionAction()
        {
            Properties["ItemID"] = new MetaProp("ItemID", typeof (string),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntries"]));

            Properties["AutoFindAh"] = new MetaProp("AutoFindAh", typeof (bool),
                                                    new DisplayNameAttribute(Pb.Strings["Action_Common_AutoFindAH"]));

            Properties["Location"] = new MetaProp("Location", typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Properties["UseCategory"] = new MetaProp("UseCategory", typeof (bool),
                                                     new DisplayNameAttribute(Pb.Strings["Action_Common_UseCategory"]));

            Properties["Category"] = new MetaProp("Category", typeof (WoWItemClass),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_ItemCategory"]));

            Properties["SubCategory"] = new MetaProp("SubCategory", typeof (WoWItemTradeGoodsClass),
                                                     new DisplayNameAttribute(
                                                         Pb.Strings["Action_Common_ItemSubCategory"]));

            Properties["MinBuyout"] = new MetaProp("MinBuyout", typeof (PropertyBag.GoldEditor),
                                                   new TypeConverterAttribute(typeof (PropertyBag.GoldEditorConverter)),
                                                   new DisplayNameAttribute(Pb.Strings["Action_Common_MinBuyout"]));

            Properties["IgnoreStackSizeBelow"] = new MetaProp("IgnoreStackSizeBelow", typeof (uint),
                                                              new DisplayNameAttribute(
                                                                  Pb.Strings["Action_Common_IgnoreStackSizeBelow"]));

            ItemID = "0";
            AutoFindAh = true;
            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            UseCategory = false;
            Category = WoWItemClass.TradeGoods;
            SubCategory = WoWItemTradeGoodsClass.None;
            MinBuyout = new PropertyBag.GoldEditor("0g0s0c");
            IgnoreStackSizeBelow = 1u;

            Properties["AutoFindAh"].PropertyChanged += AutoFindAHChanged;
            Properties["Location"].PropertyChanged += LocationChanged;
            Properties["UseCategory"].PropertyChanged += UseCategoryChanged;
            Properties["Category"].PropertyChanged += CategoryChanged;

            Properties["Category"].Show = false;
            Properties["SubCategory"].Show = false;
            Properties["Location"].Show = false;
        }

        #region Callbacks

        private void LocationChanged(object sender, MetaPropArgs e)
        {
            _loc = Util.StringToWoWPoint((string) ((MetaProp) sender).Value);
            Properties["Location"].PropertyChanged -= LocationChanged;
            Properties["Location"].Value = string.Format("{0}, {1}, {2}", _loc.X, _loc.Y, _loc.Z);
            Properties["Location"].PropertyChanged += LocationChanged;
            RefreshPropertyGrid();
        }

        private void AutoFindAHChanged(object sender, MetaPropArgs e)
        {
            Properties["Location"].Show = !AutoFindAh;
            RefreshPropertyGrid();
        }

        private void UseCategoryChanged(object sender, MetaPropArgs e)
        {
            if (UseCategory)
            {
                Properties["ItemID"].Show = false;
                Properties["Category"].Show = true;
                Properties["SubCategory"].Show = true;
            }
            else
            {
                Properties["ItemID"].Show = true;
                Properties["Category"].Show = false;
                Properties["SubCategory"].Show = false;
            }
            RefreshPropertyGrid();
        }

        private void CategoryChanged(object sender, MetaPropArgs e)
        {
            object subCategory = Callbacks.GetSubCategory(Category);
            Properties["SubCategory"] = new MetaProp("SubCategory", subCategory.GetType(),
                                                     new DisplayNameAttribute("Item SubCategory"));
            SubCategory = subCategory;
            RefreshPropertyGrid();
        }

        #endregion

        [PbXmlAttribute]
        public bool UseCategory
        {
            get { return (bool) Properties["UseCategory"].Value; }
            set { Properties["UseCategory"].Value = value; }
        }

        [PbXmlAttribute]
        public WoWItemClass Category
        {
            get { return (WoWItemClass) Properties["Category"].Value; }
            set { Properties["Category"].Value = (WoWItemClass) Enum.Parse(typeof (WoWItemClass), value.ToString()); }
        }

        [PbXmlAttribute]
        public object SubCategory
        {
            get { return Properties["SubCategory"].Value; }
            set
            {
                if (value is string)
                {
                    if (_subCatTypeLoaded)
                    {
                        value = Enum.Parse(_subCategoryType, value as string);
                    }
                    else
                    {
                        _subCatValueString = (string) value;
                        return;
                    }
                }
                Properties["SubCategory"].Value = value;
                //UpdateSubCatValue(); 
            }
        }

        [PbXmlAttribute]
        public string SubCategoryType
        {
            get { return _subCategoryType.Name; }
            set
            {
                _subCatTypeLoaded = true;
                if (value != "SubCategoryType")
                {
                    string typeName = string.Format("Styx.{0}", value);
                    _subCategoryType = Assembly.GetEntryAssembly().GetType(typeName);
                }
                else
                    _subCategoryType = typeof (SubCategoryType);
                if (_subCatValueString != null)
                {
                    SubCategory = Enum.Parse(_subCategoryType, _subCatValueString);
                    _subCatValueString = null;
                }
            }
        }

        [PbXmlAttribute]
        public string ItemID
        {
            get { return (string) Properties["ItemID"].Value; }
            set { Properties["ItemID"].Value = value; }
        }

        [PbXmlAttribute]
        public bool AutoFindAh
        {
            get { return (bool) Properties["AutoFindAh"].Value; }
            set { Properties["AutoFindAh"].Value = value; }
        }

        [PbXmlAttribute]
        [TypeConverter(typeof (PropertyBag.GoldEditorConverter))]
        public PropertyBag.GoldEditor MinBuyout
        {
            get { return (PropertyBag.GoldEditor) Properties["MinBuyout"].Value; }
            set { Properties["MinBuyout"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string) Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        [PbXmlAttribute]
        public uint IgnoreStackSizeBelow
        {
            get { return (uint) Properties["IgnoreStackSizeBelow"].Value; }
            set { Properties["IgnoreStackSizeBelow"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_CancelAuctionAction_Name"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("{0}: {1}", Name, UseCategory
                                                           ? string.Format("{0} {1}", Category,
                                                                           (SubCategory != null &&
                                                                            (int) SubCategory != -1 &&
                                                                            (int) SubCategory != 0)
                                                                               ? "(" + SubCategory + ")"
                                                                               : "")
                                                           : ItemID);
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_CancelAuctionAction_Help"]; }
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                try
                {
                    if (
                        Lua.GetReturnVal<int>(
                            "if AuctionFrame and AuctionFrame:IsVisible() == 1 then return 1 else return 0 end ", 0) ==
                        0)
                    {
                        MoveToAh();
                    }
                    else if (
                        Lua.GetReturnVal<int>(
                            "if CanSendAuctionQuery('owner') == 1 then return 1 else return 0 end ", 0) == 1)
                    {
                        if (_toScanItemList == null)
                        {
                            _toScanItemList = BuildScanItemList();
                            _toCancelItemList = new List<AuctionEntry>();
                        }

                        if (_toScanItemList.Count > 0)
                        {
                            AuctionEntry ae = _toScanItemList[0];
                            bool scanDone = ScanAh(ref ae);
                            _toScanItemList[0] = ae; // update
                            if (scanDone)
                            {
                                _toCancelItemList.Add(ae);
                                _toScanItemList.RemoveAt(0);
                            }
                            if (_toScanItemList.Count == 0)
                                Professionbuddy.Debug("Finished scanning for items");
                        }
                        else
                        {
                            if (_toCancelItemList.Count == 0)
                            {
                                _toScanItemList = null;
                                IsDone = true;
                                return RunStatus.Failure;
                            }
                            if (CancelAuction(_toCancelItemList[0]))
                            {
                                _toCancelItemList.RemoveAt(0);
                            }
                        }
                    }
                    return RunStatus.Success;
                }
                catch (Exception ex)
                {
                    Professionbuddy.Err(ex.ToString());
                }
            }
            return RunStatus.Failure;
        }

        private void MoveToAh()
        {
            WoWPoint movetoPoint = _loc;
            WoWUnit auctioneer;
            if (AutoFindAh || movetoPoint == WoWPoint.Zero)
            {
                auctioneer = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.IsAuctioneer && o.IsAlive)
                    .OrderBy(o => o.Distance).FirstOrDefault();
            }
            else
            {
                auctioneer = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.IsAuctioneer
                                                                                  && o.Location.Distance(_loc) < 5)
                    .OrderBy(o => o.Distance).FirstOrDefault();
            }
            if (auctioneer != null)
                movetoPoint = WoWMathHelper.CalculatePointFrom(Me.Location, auctioneer.Location, 3);
            else if (movetoPoint == WoWPoint.Zero)
                movetoPoint = MoveToAction.GetLocationFromDB(MoveToAction.MoveToType.NearestAH, 0);
            if (movetoPoint == WoWPoint.Zero)
            {
                Professionbuddy.Err(Pb.Strings["Error_UnableToFindAuctioneer"]);
            }
            if (movetoPoint.Distance(ObjectManager.Me.Location) > 4.5)
            {
                Util.MoveTo(movetoPoint);
            }
            else if (auctioneer != null)
            {
                auctioneer.Interact();
            }
        }

        private List<AuctionEntry> BuildScanItemList()
        {
            var tmpItemlist = new List<AuctionEntry>();
            Dictionary<uint, string> myAucs = GetMyAuctions();

            if (UseCategory)
            {
                using (new FrameLock())
                {
                    foreach (var aucKV in myAucs)
                    {
                        ItemInfo info = ItemInfo.FromId(aucKV.Key);
                        if (info != null)
                        {
                            if (info.ItemClass == Category && SubCategoryCheck(info.SubClassId))
                            {
                                tmpItemlist.Add(new AuctionEntry(aucKV.Value, aucKV.Key, 0, 0));
                            }
                        }
                        else
                            Professionbuddy.Err("item cache of {0} is null", aucKV.Value);
                    }
                }
            }
            else
            {
                if (ItemID == "0" || ItemID == "")
                {
                    tmpItemlist.AddRange(myAucs.Select(kv => new AuctionEntry(kv.Value, kv.Key, 0, 0)));
                }
                else
                {
                    string[] entries = ItemID.Split(',');
                    if (entries.Length > 0)
                    {
                        foreach (string entry in entries)
                        {
                            uint id;
                            uint.TryParse(entry.Trim(), out id);
                            if (myAucs.ContainsKey(id))
                                tmpItemlist.Add(new AuctionEntry(myAucs[id], id, 0, 0));
                        }
                    }
                }
            }
            return tmpItemlist;
        }

        private bool SubCategoryCheck(object subCat)
        {
            var sub = (int) SubCategory;
            return sub == -1 || sub == 0 || (int) subCat == sub;
        }

        public override void Reset()
        {
            base.Reset();
            _toScanItemList = null;
            _toCancelItemList = null;
        }

        public override object Clone()
        {
            return new CancelAuctionAction
                       {
                           ItemID = ItemID,
                           AutoFindAh = AutoFindAh,
                           Location = Location,
                           UseCategory = UseCategory,
                           Category = Category,
                           SubCategory = SubCategory,
                           IgnoreStackSizeBelow = IgnoreStackSizeBelow,
                           MinBuyout = MinBuyout
                       };
        }

        #region Auction House

        private const string CancelAuctionLuaFormat =
            "local A =GetNumAuctionItems('owner') " +
            "local cnt=0 " +
            "for i=A,1,-1 do " +
            "local name,_,cnt,_,_,_,_,_,_,buyout,_,_,_,sold,id=GetAuctionItemInfo('owner', i) " +
            "if id == {0} and sold ~= 1 and {2} > {1} and (buyout/cnt) > {2} then " +
            "CancelAuction(i) cnt=cnt+1 " +
            "end " +
            "end " +
            "return cnt";

        private readonly Stopwatch _queueTimer = new Stopwatch();
        private int _page;
        private int _totalAuctions;

        private bool ScanAh(ref AuctionEntry ae)
        {
            bool scanned = false;
            if (!_queueTimer.IsRunning)
            {
                string lua = string.Format("QueryAuctionItems(\"{0}\" ,nil,nil,nil,nil,nil,{1}) return 1",
                                           ae.Name.ToFormatedUTF8(), _page);
                Lua.GetReturnVal<int>(lua, 0);
                Professionbuddy.Debug("Searching AH for {0}", ae.Name);
                _queueTimer.Start();
            }
            else if (_queueTimer.ElapsedMilliseconds <= 10000)
            {
                using (new FrameLock())
                {
                    if (
                        Lua.GetReturnVal<int>("if CanSendAuctionQuery('list') == 1 then return 1 else return 0 end ", 0) ==
                        1)
                    {
                        _queueTimer.Reset();
                        _totalAuctions = Lua.GetReturnVal<int>("return GetNumAuctionItems('list')", 1);
                        string lua = string.Format(SellItemOnAhAction.ScanAHFormatLua,
                                                   ae.LowestBo, ae.MyAuctions, ae.Id, IgnoreStackSizeBelow, int.MaxValue);
                        List<string> retVals = Lua.GetReturnValues(lua);
                        uint.TryParse(retVals[0], out ae.LowestBo);
                        uint.TryParse(retVals[1], out ae.MyAuctions);
                        if (++_page >= (int) Math.Ceiling((double) _totalAuctions/50))
                            scanned = true;
                    }
                }
            }
            else
            {
                scanned = true;
            }
            // reset to default values in preparations for next scan
            if (scanned)
            {
                Professionbuddy.Debug("lowest buyout {0}", ae.LowestBo);
                _queueTimer.Reset();
                _totalAuctions = 0;
                _page = 0;
            }
            return scanned;
        }

        // Indexes are {0}=ItemID, {1}=MinBuyout, {2}=LowestBuyoutFound

        private bool CancelAuction(AuctionEntry ae)
        {
            string lua = String.Format(CancelAuctionLuaFormat, ae.Id, MinBuyout.TotalCopper, ae.LowestBo);
            var numCanceled = Lua.GetReturnVal<int>(lua, 0);
            if (numCanceled > 0)
            {
                Professionbuddy.Log("Canceled {0} x{1}", ae.Name, numCanceled);
            }
            return true;
        }

        private Dictionary<uint, string> GetMyAuctions()
        {
            var ret = new Dictionary<uint, string>();
            using (new FrameLock())
            {
                var numOfMyItemsOnAH = Lua.GetReturnVal<int>("return GetNumAuctionItems('owner')", 0);
                for (int i = 1; i <= numOfMyItemsOnAH; i++)
                {
                    List<string> luaRet =
                        Lua.GetReturnValues(
                            string.Format(
                                "local name,_,_,_,_,_,_,_,_,_,_,_,_,sold,id=GetAuctionItemInfo('owner', {0}) return id,name,sold",
                                i));
                    if (luaRet != null && luaRet[2] != "1")
                    {
                        uint id = uint.Parse(luaRet[0]);
                        if (!ret.ContainsKey(id))
                            ret.Add(id, luaRet[1]);
                    }
                }
            }
            return ret;
        }

        #endregion
    }
}