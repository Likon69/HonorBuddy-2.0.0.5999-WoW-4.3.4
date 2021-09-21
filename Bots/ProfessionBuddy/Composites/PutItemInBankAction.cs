using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using HighVoltz.Dynamic;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{

    #region PutItemInBankAction

    public sealed class PutItemInBankAction : PBAction
    {
        #region Strings

        private const string GbankSlotInfo =
            "local _,c,l=GetGuildBankItemInfo({0}, {1}) " +
            "if c > 0 and l == nil then " +
            "local id = tonumber(string.match(GetGuildBankItemLink({0},{1}), 'Hitem:(%d+)')) " +
            "local maxStack = select(8,GetItemInfo(id)) " +
            "return id,c,maxStack " +
            "elseif c == 0 then " +
            "return 0,0,0 " +
            "end ";

        #endregion

        private const long GbankItemThrottle = 1000;

        private const string DepositItemInPersonalBankLuaFormat =
            "local bagged = 0 " +
            "local bagInfo = {{0}} " +
            "local bag = -1 " +
            "local i=1; " +
            "local _,_,_,_,_,_,_,maxStack = GetItemInfo({0}) " +
            "while bag <= 11 do " +
            "local itemf  = GetItemFamily({0}) " +
            "local fs,bfamily = GetContainerNumFreeSlots(bag) " +
            "if fs > 0 and (bfamily == 0 or bit.band(itemf, bfamily) > 0) then " +
            "for slot=1, GetContainerNumSlots(bag) do " +
            "local _,c,l = GetContainerItemInfo(bag, slot) " +
            "local id = GetContainerItemID(bag, slot) or 0 " +
            "if c == nil then " +
            "bagInfo[i]={{bag,slot,maxStack}} " +
            "i=i+1 " +
            "elseif l == nil and id == {0} and c < maxStack then " +
            "bagInfo[i]={{bag,slot,maxStack-c}} " +
            "i=i+1 " +
            "end " +
            "end " +
            "end " +
            "bag = bag + 1 " +
            "if bag == 0 then bag = 5 end " +
            "end " +
            "i=1 " +
            "for bag = 0,4 do " +
            "for slot=1,GetContainerNumSlots(bag) do " +
            "if i > #bagInfo then return end " +
            "local id = GetContainerItemID(bag,slot) or 0 " +
            "local _,c,l = GetContainerItemInfo(bag, slot) " +
            "local _,_,_,_,_,_,_, maxStack = GetItemInfo(id) " +
            "if id == {0} and l == nil then " +
            "if c + bagged <= {1} and c <= bagInfo[i][3] then " +
            "PickupContainerItem(bag, slot) " +
            "PickupContainerItem(bagInfo[i][1], bagInfo[i][2]) " +
            "bagged = bagged + c " +
            "else " +
            "local cnt = {1}-bagged " +
            "if cnt > bagInfo[i][3] then cnt = bagInfo[i][3] end " +
            "SplitContainerItem(bag,slot, cnt) " +
            "PickupContainerItem(bagInfo[i][1], bagInfo[i][2]) " +
            "bagged = bagged + cnt " +
            "end " +
            "i=i+1 " +
            "end " +
            "if bagged == {1} then return end " +
            "end " +
            "end return ";

        private readonly Stopwatch _gbankItemThrottleSW = new Stopwatch();
        private Dictionary<uint, int> _itemList;
        private WoWPoint _loc;

        public PutItemInBankAction()
        {
            Properties["Amount"] = new MetaProp("Amount", typeof (DynamicProperty<int>),
                                                new TypeConverterAttribute(
                                                    typeof (DynamicProperty<int>.DynamivExpressionConverter)),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_Amount"]));

            Properties["ItemID"] = new MetaProp("ItemID", typeof (string),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntries"]));

            Properties["Bank"] = new MetaProp("Bank", typeof (BankType),
                                              new DisplayNameAttribute(Pb.Strings["Action_Common_Bank"]));

            Properties["AutoFindBank"] = new MetaProp("AutoFindBank", typeof (bool),
                                                      new DisplayNameAttribute(Pb.Strings["Action_Common_AutoFindBank"]));

            Properties["Location"] = new MetaProp("Location", typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Properties["NpcEntry"] = new MetaProp("NpcEntry", typeof (uint),
                                                  new EditorAttribute(typeof (PropertyBag.EntryEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_NpcEntry"]));

            Properties["GuildTab"] = new MetaProp("GuildTab", typeof (uint),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_GuildTab"]));

            Properties["UseCategory"] = new MetaProp("UseCategory", typeof (bool),
                                                     new DisplayNameAttribute(Pb.Strings["Action_Common_UseCategory"]));

            Properties["Category"] = new MetaProp("Category", typeof (WoWItemClass),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_ItemCategory"]));

            Properties["SubCategory"] = new MetaProp("SubCategory", typeof (WoWItemTradeGoodsClass),
                                                     new DisplayNameAttribute(
                                                         Pb.Strings["Action_Common_ItemSubCategory"]));

            Properties["Deposit"] = new MetaProp("Deposit", typeof (DepositWithdrawAmount),
                                                 new DisplayNameAttribute(Pb.Strings["Action_Common_Deposit"]));

            Amount = new DynamicProperty<int>(this, "0");
            RegisterDynamicProperty("Amount");
            ItemID = "";
            Bank = BankType.Personal;
            AutoFindBank = true;
            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            NpcEntry = 0u;
            GuildTab = 0u;
            UseCategory = true;
            Category = WoWItemClass.TradeGoods;
            SubCategory = WoWItemTradeGoodsClass.None;
            Deposit = DepositWithdrawAmount.All;

            Properties["ItemID"].Show = false;
            Properties["Location"].Show = false;
            Properties["NpcEntry"].Show = false;
            Properties["GuildTab"].Show = false;
            Properties["Amount"].Show = false;

            Properties["AutoFindBank"].PropertyChanged += AutoFindBankChanged;
            Properties["Bank"].PropertyChanged += PutItemInBankActionPropertyChanged;
            Properties["Location"].PropertyChanged += LocationChanged;
            Properties["UseCategory"].PropertyChanged += UseCategoryChanged;
            Properties["Category"].PropertyChanged += CategoryChanged;
            Properties["Deposit"].PropertyChanged += DepositChanged;
        }

        #region Callbacks

        private void DepositChanged(object sender, MetaPropArgs e)
        {
            Properties["Deposit"].Show = Deposit == DepositWithdrawAmount.Amount;
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

        private void PutItemInBankActionPropertyChanged(object sender, MetaPropArgs e)
        {
            Properties["GuildTab"].Show = Bank != BankType.Personal;
            RefreshPropertyGrid();
        }

        private void AutoFindBankChanged(object sender, MetaPropArgs e)
        {
            if (AutoFindBank)
            {
                Properties["Location"].Show = false;
                Properties["NpcEntry"].Show = false;
            }
            else
            {
                Properties["Location"].Show = true;
                Properties["NpcEntry"].Show = true;
            }
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
        public DepositWithdrawAmount Deposit
        {
            get { return (DepositWithdrawAmount) Properties["Deposit"].Value; }
            set { Properties["Deposit"].Value = value; }
        }

        [PbXmlAttribute]
        public bool UseCategory
        {
            get { return (bool) Properties["UseCategory"].Value; }
            set { Properties["UseCategory"].Value = value; }
        }

        public WoWItemClass Category
        {
            get { return (WoWItemClass) Properties["Category"].Value; }
            set { Properties["Category"].Value = value; }
        }

        public object SubCategory
        {
            get { return Properties["SubCategory"].Value; }
            set { Properties["SubCategory"].Value = value; }
        }

        [PbXmlAttribute]
        public BankType Bank
        {
            get { return (BankType) Properties["Bank"].Value; }
            set { Properties["Bank"].Value = value; }
        }

        [PbXmlAttribute("Entry")]
        [PbXmlAttribute]
        public string ItemID
        {
            get { return (string) Properties["ItemID"].Value; }
            set { Properties["ItemID"].Value = value; }
        }

        [PbXmlAttribute]
        public uint GuildTab
        {
            get { return (uint) Properties["GuildTab"].Value; }
            set { Properties["GuildTab"].Value = value; }
        }

        [PbXmlAttribute]
        public uint NpcEntry
        {
            get { return (uint) Properties["NpcEntry"].Value; }
            set { Properties["NpcEntry"].Value = value; }
        }

        [PbXmlAttribute]
        [TypeConverter(typeof (DynamicProperty<int>.DynamivExpressionConverter))]
        public DynamicProperty<int> Amount
        {
            get { return (DynamicProperty<int>) Properties["Amount"].Value; }
            set { Properties["Amount"].Value = value; }
        }

        [PbXmlAttribute]
        public bool AutoFindBank
        {
            get { return (bool) Properties["AutoFindBank"].Value; }
            set { Properties["AutoFindBank"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string) Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_PutItemInBankAction_Name"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("{0}: {1} {2}", Name,
                                     UseCategory ? string.Format("{0} {1}", Category, SubCategory) : ItemID,
                                     Amount > 0 ? Amount.ToString() : "");
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_PutItemInBankAction_Help"]; }
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if ((Bank == BankType.Guild && !Util.IsGbankFrameVisible) ||
                    (Bank == BankType.Personal && !Util.IsBankFrameOpen))
                {
                    MoveToBanker();
                }
                else
                {
                    if (_itemsSW == null)
                    {
                        _itemsSW = new Stopwatch();
                        _itemsSW.Start();
                    }
                    else if (_itemsSW.ElapsedMilliseconds < Util.WoWPing*3)
                        return RunStatus.Success;
                    if (_itemList == null)
                        _itemList = BuildItemList();
                    // no bag space... 
                    if (_itemList.Count == 0)
                        IsDone = true;
                    else
                    {
                        uint itemID = _itemList.Keys.FirstOrDefault();
                        bool done;
                        if (Bank == BankType.Personal)
                            done = PutItemInBank(itemID, _itemList[itemID]);
                        else
                        {
                            // throttle the amount of items being withdrawn from gbank per sec
                            if (!_gbankItemThrottleSW.IsRunning)
                                _gbankItemThrottleSW.Start();
                            if (_gbankItemThrottleSW.ElapsedMilliseconds < GbankItemThrottle)
                                return RunStatus.Success;
                            _gbankItemThrottleSW.Reset();
                            _gbankItemThrottleSW.Start();
                            int ret = PutItemInGBank(itemID, _itemList[itemID], GuildTab);
                            _itemList[itemID] = ret < 0 ? 0 : _itemList[itemID] - ret;
                            done = _itemList[itemID] <= 0;
                        }
                        if (done)
                        {
                            Professionbuddy.Debug("Done Depositing Item:{0} to bank", itemID);
                            _itemList.Remove(itemID);
                        }
                        _itemsSW.Reset();
                        _itemsSW.Start();
                    }
                }
                if (IsDone)
                {
                    Professionbuddy.Log("Deposited Items:[{0}] to {1} Bank", ItemID, Bank);
                }
                else
                    return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        private void MoveToBanker()
        {
            WoWPoint movetoPoint = _loc;
            WoWObject bank = GetLocalBanker();
            if (bank != null)
                movetoPoint = WoWMathHelper.CalculatePointFrom(Me.Location, bank.Location, 4);
                // search the database
            else if (movetoPoint == WoWPoint.Zero)
            {
                movetoPoint = MoveToAction.GetLocationFromDB(Bank == BankType.Personal
                                                                 ? MoveToAction.MoveToType.NearestBanker
                                                                 : MoveToAction.MoveToType.NearestGB, NpcEntry);
            }
            if (movetoPoint == WoWPoint.Zero)
            {
                IsDone = true;
                Professionbuddy.Err("Unable to find bank");
            }
            if (movetoPoint.Distance(ObjectManager.Me.Location) > 4)
            {
                Util.MoveTo(movetoPoint);
            }
                // since there are many personal bank replacement addons I can't just check if frame is open and be generic.. using events isn't reliable
            else if (bank != null)
            {
                bank.Interact();
            }
            else
            {
                IsDone = true;
                Professionbuddy.Err(Pb.Strings["Error_UnableToFindBank"]);
            }
        }

        private Dictionary<uint, int> BuildItemList()
        {
            var itemList = new Dictionary<uint, int>();
            IEnumerable<WoWItem> tmpItemlist = from item in Me.BagItems
                                               where !item.IsConjured && !item.IsSoulbound && !item.IsDisabled
                                               select item;
            if (UseCategory)
                foreach (WoWItem item in tmpItemlist)
                {
                    if (!Pb.ProtectedItems.Contains(item.Entry) && item.ItemInfo.ItemClass == Category &&
                        SubCategoryCheck(item) && !itemList.ContainsKey(item.Entry))
                    {
                        itemList.Add(item.Entry, Deposit == DepositWithdrawAmount.Amount
                                                     ? Amount
                                                     : Util.GetCarriedItemCount(item.Entry));
                    }
                }
            else
            {
                string[] entries = ItemID.Split(',');
                if (entries.Length > 0)
                {
                    foreach (string entry in entries)
                    {
                        uint itemID;
                        uint.TryParse(entry.Trim(), out itemID);
                        itemList.Add(itemID, Deposit == DepositWithdrawAmount.Amount
                                                 ? Amount
                                                 : Util.GetCarriedItemCount(itemID));
                    }
                }
                else
                {
                    Professionbuddy.Err(Pb.Strings["Error_NoItemEntries"]);
                    IsDone = true;
                }
            }
            return itemList;
        }

        private bool SubCategoryCheck(WoWItem item)
        {
            var sub = (int) SubCategory;
            if (sub == -1 || sub == 0)
                return true;
            PropertyInfo prop =
                item.ItemInfo.GetType().GetProperties().FirstOrDefault(t => t.PropertyType == SubCategory.GetType());
            if (prop != null)
            {
                object val = prop.GetValue(item.ItemInfo, null);
                if (val != null && (int) val == sub)
                    return true;
            }
            return false;
        }

        private WoWObject GetLocalBanker()
        {
            WoWObject bank = null;
            List<WoWObject> bankers;
            if (Bank == BankType.Guild)
                bankers = (from banker in ObjectManager.ObjectList
                           where IsValidGuildBank(banker)
                           select banker).ToList();
            else
                bankers = (from banker in ObjectManager.ObjectList
                           where (banker is WoWUnit &&
                                  ((WoWUnit) banker).IsBanker &&
                                  ((WoWUnit) banker).IsAlive &&
                                  ((WoWUnit) banker).CanSelect)
                           select banker).ToList();
            if (!AutoFindBank && NpcEntry != 0)
                bank = bankers.Where(b => b.Entry == NpcEntry).OrderBy(o => o.Distance).FirstOrDefault();
            else if (AutoFindBank || _loc == WoWPoint.Zero)
                bank = bankers.OrderBy(o => o.Distance).FirstOrDefault();
            else if (ObjectManager.Me.Location.Distance(_loc) <= 90)
            {
                bank = bankers.Where(o => o.Location.Distance(_loc) < 10).
                    OrderBy(o => o.Distance).FirstOrDefault();
            }
            return bank;
        }

        private bool IsValidGuildBank(WoWObject obj)
        {
            if (obj is WoWGameObject)
            {
                var banker = (WoWGameObject) obj;
                return banker.SubType == WoWGameObjectType.GuildBank &&
                       (banker.CreatedByGuid == 0 || banker.CreatedByGuid == Me.Guid);
            }
            if (obj is WoWUnit)
            {
                var banker = (WoWUnit) obj;
                return banker.IsGuildBanker && banker.IsAlive && banker.CanSelect;
            }
            return false;
        }

        // indexes are {0} = ItemID, {1} = amount to deposit.


        private bool PutItemInBank(uint id, int amount)
        {
            string lua = string.Format(DepositItemInPersonalBankLuaFormat, id, amount <= 0 ? int.MaxValue : amount);
            Lua.DoString(lua);
            return true;
        }

        public override void Reset()
        {
            base.Reset();
            _queueServerSW = null;
            _bankSlots = null;
            _itemList = null;
            _itemsSW = null;
        }

        public override object Clone()
        {
            return new PutItemInBankAction
                       {
                           ItemID = ItemID,
                           Amount = Amount,
                           Bank = Bank,
                           NpcEntry = NpcEntry,
                           _loc = _loc,
                           GuildTab = GuildTab,
                           AutoFindBank = AutoFindBank,
                           Parent = Parent,
                           Location = Location,
                           UseCategory = UseCategory,
                           Category = Category,
                           SubCategory = SubCategory,
                           Deposit = Deposit
                       };
        }

        public override void OnProfileLoad(XElement element)
        {
            XAttribute cat = element.Attribute("Category");
            XAttribute subCatAttr = element.Attribute("SubCategory");
            XAttribute subCatTypeAttr = element.Attribute("SubCategoryType");
            if (cat != null)
            {
                Category = (WoWItemClass) Enum.Parse(typeof (WoWItemClass), cat.Value);
                cat.Remove();
            }
            if (subCatAttr != null && subCatTypeAttr != null)
            {
                Type subCategoryType;
                if (subCatTypeAttr.Value != "SubCategoryType")
                {
                    string typeName = string.Format("Styx.{0}", subCatTypeAttr.Value);
                    subCategoryType = Assembly.GetEntryAssembly().GetType(typeName);
                }
                else
                    subCategoryType = typeof (SubCategoryType);
                SubCategory = Enum.Parse(subCategoryType, subCatAttr.Value);
                subCatAttr.Remove();
                subCatTypeAttr.Remove();
            }
        }

        public override void OnProfileSave(XElement element)
        {
            element.Add(new XAttribute("Category", Category.ToString()));
            element.Add(new XAttribute("SubCategoryType", SubCategory.GetType().Name));
            element.Add(new XAttribute("SubCategory", SubCategory.ToString()));
        }

        #region GuildBank

        private const int GuildTabSlotNum = 98;
        private List<BankSlotInfo> _bankSlots;
        private Stopwatch _itemsSW;
        private Stopwatch _queueServerSW;

        public int PutItemInGBank(uint id, int amount, uint tab)
        {
            using (new FrameLock())
            {
                if (_queueServerSW == null)
                {
                    _queueServerSW = new Stopwatch();
                    _queueServerSW.Start();
                    Lua.DoString(
                        "for i=GetNumGuildBankTabs(), 1, -1 do QueryGuildBankTab(i) end SetCurrentGuildBankTab({0}) ",
                        tab == 0 ? 1 : tab);
                    Professionbuddy.Log("Queuing server for gbank info");
                    return 0;
                }
                if (_queueServerSW.ElapsedMilliseconds < 2000)
                    return 0;
                if (_bankSlots == null)
                    _bankSlots = GetBankSlotInfo();
                var tabCnt = Lua.GetReturnVal<int>("return GetNumGuildBankTabs()", 0);
                var currentTab = Lua.GetReturnVal<int>("return GetCurrentGuildBankTab()", 0);

                IEnumerable<BankSlotInfo> slotsInCurrentTab = _bankSlots.Where(slotI => slotI.Bag == currentTab);
                WoWItem itemToDeposit = Me.CarriedItems.OrderBy(item => item.StackCount)
                    .FirstOrDefault(item => item.Entry == id && !item.IsDisabled);
                if (itemToDeposit != null)
                {
                    int depositAmount = amount > 0 && amount < (int) itemToDeposit.StackCount
                                            ? amount
                                            : (int) itemToDeposit.StackCount;

                    BankSlotInfo emptySlot = slotsInCurrentTab.FirstOrDefault(slotI => slotI.StackSize == 0);
                    BankSlotInfo partialStack = slotsInCurrentTab
                        .FirstOrDefault(
                            slotI => slotI.ItemID == id && slotI.MaxStackSize - slotI.StackSize >= depositAmount);
                    if (partialStack != null || emptySlot != null)
                    {
                        bool slotIsEmpty = partialStack == null;
                        int bSlotIndex = slotIsEmpty ? _bankSlots.IndexOf(emptySlot) : _bankSlots.IndexOf(partialStack);
                        _bankSlots[bSlotIndex].StackSize += itemToDeposit.StackCount;
                        if (slotIsEmpty)
                        {
                            _bankSlots[bSlotIndex].ItemID = itemToDeposit.Entry;
                            _bankSlots[bSlotIndex].MaxStackSize = itemToDeposit.ItemInfo.MaxStackSize;
                        }
                        if (depositAmount == itemToDeposit.StackCount)
                            itemToDeposit.UseContainerItem();
                        else
                        {
                            Lua.DoString("SplitContainerItem({0},{1},{2}) PickupGuildBankItem({3},{4})",
                                         itemToDeposit.BagIndex + 1, itemToDeposit.BagSlot + 1, depositAmount,
                                         _bankSlots[bSlotIndex].Bag, _bankSlots[bSlotIndex].Slot);
                        }
                        return depositAmount;
                    }
                    if (tab > 0 || currentTab == tabCnt)
                    {
                        Professionbuddy.Log("Guild Tab: {0} is full", tab);
                        return -1;
                    }
                    if (tab == 0 && currentTab < tabCnt)
                    {
                        Lua.DoString("SetCurrentGuildBankTab({0})", currentTab + 1);
                        return 0;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Returns a list of bag/gbank tab slots with empty/partial full slots.
        /// </summary>
        /// <returns></returns>
        private List<BankSlotInfo> GetBankSlotInfo()
        {
            var bankSlotInfo = new List<BankSlotInfo>();
            using (new FrameLock())
            {
                if (Bank == BankType.Guild)
                {
                    var tabCnt = Lua.GetReturnVal<int>("return GetNumGuildBankTabs()", 0);
                    int minTab = GuildTab > 0 ? (int) GuildTab : 1;
                    int maxTab = GuildTab > 0 ? (int) GuildTab : tabCnt;
                    for (int tab = minTab; tab <= maxTab; tab++)
                    {
                        // check permissions for tab
                        bool canDespositInTab =
                            Lua.GetReturnVal<int>(
                                string.Format(
                                    "local _,_,v,d =GetGuildBankTabInfo({0}) if v==1 and d==1 then return 1 else return 0 end",
                                    tab), 0) == 1;
                        if (canDespositInTab)
                        {
                            for (int slot = 1; slot <= GuildTabSlotNum; slot++)
                            {
                                // 3 return values in following order, ItemID,StackSize,MaxStackSize
                                string lua = string.Format(GbankSlotInfo, tab, slot);
                                List<string> retVals = Lua.GetReturnValues(lua);
                                bankSlotInfo.Add(new BankSlotInfo(tab, 0, slot, uint.Parse(retVals[0]),
                                                                  uint.Parse(retVals[1]), int.Parse(retVals[2])));
                            }
                        }
                    }
                }
            }
            return bankSlotInfo;
        }

        #endregion

        #region Nested type: BankSlotInfo

        private class BankSlotInfo : IEquatable<BankSlotInfo>
        {
            public BankSlotInfo(int bag, int bagType, int slot, uint itemID, uint stackSize, int maxStackSize)
            {
                Bag = bag;
                BagType = bagType;
                Slot = slot;
                ItemID = itemID;
                StackSize = stackSize;
                MaxStackSize = maxStackSize;
            }

            public int Bag { get; private set; } // this is also the tab for GBank
            public int BagType { get; private set; }
            public int Slot { get; private set; }
            public uint ItemID { get; set; } //  0 if slot is has no items.
            public uint StackSize { get; set; } // amount of items in slot..
            public int MaxStackSize { get; set; }
            // public static BankSlotInfo Zero { get { return new BankSlotInfo(0, 0, 0, 0, 0, 0); } }

            #region IEquatable<BankSlotInfo> Members

            public bool Equals(BankSlotInfo other)
            {
                return other != null && Bag == other.Bag && Slot == other.Slot;
            }

            #endregion

            public override bool Equals(object obj)
            {
                return Equals(obj as BankSlotInfo);
            }

            public override int GetHashCode()
            {
                return (Bag*1000 + Slot).GetHashCode();
            }

            public static bool operator ==(BankSlotInfo a, BankSlotInfo b)
            {
                if ((object) a == null || (object) b == null)
                    return Equals(a, b);
                return a.Equals(b);
            }

            public static bool operator !=(BankSlotInfo a, BankSlotInfo b)
            {
                return !(a == b);
            }
        }

        #endregion
    }

    #endregion
}