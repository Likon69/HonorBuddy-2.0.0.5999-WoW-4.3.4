using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using HighVoltz.Dynamic;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{
    public sealed class GetItemfromBankAction : PBAction
    {
        // number of times the recipe will be crafted

        #region BankWithdrawlItemType enum

        public enum BankWithdrawlItemType
        {
            SpecificItem,
            Materials,
        }

        #endregion

        private const long GbankItemThrottle = 1000;

        private const string WithdrawItemFromGBankLuaFormat =
            "local tabnum = GetNumGuildBankTabs() " +
            "local bagged = 0 " +
            "local itemID = {0} " +
            "local  sawItem = 0  " +
            "local amount = {1} " +
            "for tab = 1,tabnum do " +
            "local _,_,iv,_,nw, rw = GetGuildBankTabInfo(tab)  " +
            "if iv and (nw > 0 or nw == -1) and (rw == -1 or rw > 0) then " +
            "SetCurrentGuildBankTab(tab) " +
            "for slot = 1, 98 do " +
            "local _,c,l=GetGuildBankItemInfo(tab, slot) " +
            "local id = tonumber(string.match(GetGuildBankItemLink(tab, slot) or '','|Hitem:(%d+)')) " +
            "if l == nil and c > 0 and (id == itemID or itemID == 0) then " +
            "sawItem = 1 " +
            "if c  <= amount then " +
            "AutoStoreGuildBankItem(tab, slot) " +
            "return c " +
            "else " +
            "local itemf  = GetItemFamily(id) " +
            "for bag =0 ,NUM_BAG_SLOTS do " +
            "local fs,bfamily = GetContainerNumFreeSlots(bag) " +
            "if fs > 0 and (bfamily == 0 or bit.band(itemf, bfamily) > 0) then " +
            "local freeSlots = GetContainerFreeSlots(bag) " +
            "SplitGuildBankItem(tab, slot, amount) " +
            "PickupContainerItem(bag, freeSlots[1]) " +
            "return amount-c " +
            "end " +
            "end " +
            "end " +
            "end " +
            "end " +
            "end " +
            "end " +
            "if sawItem == 0 then return -1 else return bagged end ";

        private const string WithdrawItemFromPersonalBankLuaFormat =
            "local numSlots = GetNumBankSlots() " +
            "local splitUsed = 0 " +
            "local bagged = 0 " +
            "local amount = {1} " +
            "local itemID = {0} " +
            "local bag1 = numSlots + 4  " +
            "while bag1 >= -1 do " +
            "if bag1 == 4 then " +
            "bag1 = -1 " +
            "end " +
            "for slot1 = 1, GetContainerNumSlots(bag1) do " +
            "local _,c,l=GetContainerItemInfo(bag1, slot1) " +
            "local id = GetContainerItemID(bag1,slot1) " +
            "if l ~= 1 and c and c > 0 and (id == itemID or itemID == 0) then " +
            "if c + bagged <= amount  then " +
            "UseContainerItem(bag1,slot1) " +
            "bagged = bagged + c " +
            "else " +
            "local itemf  = GetItemFamily(id) " +
            "for bag2 = 0,4 do " +
            "local fs,bfamily = GetContainerNumFreeSlots(bag2) " +
            "if fs > 0 and (bfamily == 0 or bit.band(itemf, bfamily) > 0) then " +
            "local freeSlots = GetContainerFreeSlots(bag2) " +
            "SplitContainerItem(bag1,slot1,amount - bagged) " +
            "if bag2 == 0 then PutItemInBackpack() else PutItemInBag(bag2) end " +
            "return " +
            "end " +
            "bag2 = bag2 -1 " +
            "end " +
            "end " +
            "if bagged >= amount then return end " +
            "end " +
            "end " +
            "bag1 = bag1 -1 " +
            "end ";

        private static Stopwatch _queueServerSW;
        private readonly Stopwatch _gbankItemThrottleSW = new Stopwatch();
        private Dictionary<uint, int> _itemList;
        private Stopwatch _itemsSW;
        private WoWPoint _loc;

        public GetItemfromBankAction()
        {
            Properties["Amount"] = new MetaProp("Amount", typeof (DynamicProperty<int>),
                                                new TypeConverterAttribute(
                                                    typeof (DynamicProperty<int>.DynamivExpressionConverter)),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_Amount"]));

            Properties["ItemID"] = new MetaProp("ItemID", typeof (string),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntries"]));

            Properties["MinFreeBagSlots"] = new MetaProp("MinFreeBagSlots", typeof (uint),
                                                         new DisplayNameAttribute(
                                                             Pb.Strings["Action_Common_MinFreeBagSlots"]));

            Properties["GetItemfromBankType"] = new MetaProp("GetItemfromBankType",
                                                             typeof (BankWithdrawlItemType),
                                                             new DisplayNameAttribute(
                                                                 Pb.Strings["Action_Common_ItemsToWithdraw"]));

            Properties["Bank"] = new MetaProp("Bank", typeof (BankType),
                                              new DisplayNameAttribute(Pb.Strings["Action_Common_Bank"]));

            Properties["AutoFindBank"] = new MetaProp("AutoFindBank", typeof (bool),
                                                      new DisplayNameAttribute(Pb.Strings["Action_Common_AutoFindBank"]));

            Properties["Location"] = new MetaProp("Location", typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Properties["NpcEntry"] = new MetaProp("NpcEntry",
                                                  typeof (uint),
                                                  new EditorAttribute(typeof (PropertyBag.EntryEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_NpcEntry"]));

            Properties["WithdrawAdditively"] = new MetaProp("WithdrawAdditively", typeof (bool),
                                                            new DisplayNameAttribute(
                                                                Pb.Strings["Action_Common_WithdrawAdditively"]));

            Properties["Withdraw"] = new MetaProp("Withdraw", typeof (DepositWithdrawAmount),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Withdraw"]));

            Amount = new DynamicProperty<int>(this, "1");
            RegisterDynamicProperty("Amount");
            ItemID = "";
            MinFreeBagSlots = 2u;
            GetItemfromBankType = BankWithdrawlItemType.SpecificItem;
            Bank = BankType.Personal;
            AutoFindBank = true;
            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            NpcEntry = 0u;
            WithdrawAdditively = true;
            Withdraw = DepositWithdrawAmount.All;

            Properties["Location"].Show = false;
            Properties["NpcEntry"].Show = false;
            Properties["Amount"].Show = false;

            Properties["AutoFindBank"].PropertyChanged += AutoFindBankChanged;
            Properties["GetItemfromBankType"].PropertyChanged += GetItemfromBankActionPropertyChanged;
            Properties["Location"].PropertyChanged += LocationChanged;
            Properties["Withdraw"].PropertyChanged += WithdrawChanged;
        }

        #region Callbacks

        private void WithdrawChanged(object sender, MetaPropArgs e)
        {
            Properties["Amount"].Show = Withdraw == DepositWithdrawAmount.Amount;
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

        private void GetItemfromBankActionPropertyChanged(object sender, MetaPropArgs e)
        {
            switch (GetItemfromBankType)
            {
                case BankWithdrawlItemType.SpecificItem:
                    Properties["Amount"].Show = true;
                    Properties["ItemID"].Show = true;
                    Properties["WithdrawAdditively"].Show = true;
                    break;
                case BankWithdrawlItemType.Materials:
                    Properties["Amount"].Show = false;
                    Properties["ItemID"].Show = false;
                    Properties["WithdrawAdditively"].Show = false;
                    break;
            }
            RefreshPropertyGrid();
        }

        #endregion

        [PbXmlAttribute]
        public DepositWithdrawAmount Withdraw
        {
            get { return (DepositWithdrawAmount) Properties["Withdraw"].Value; }
            set { Properties["Withdraw"].Value = value; }
        }

        [PbXmlAttribute]
        public BankType Bank
        {
            get { return (BankType) Properties["Bank"].Value; }
            set { Properties["Bank"].Value = value; }
        }

        [PbXmlAttribute]
        public uint MinFreeBagSlots
        {
            get { return (uint) Properties["MinFreeBagSlots"].Value; }
            set { Properties["MinFreeBagSlots"].Value = value; }
        }

        [PbXmlAttribute]
        public BankWithdrawlItemType GetItemfromBankType
        {
            get { return (BankWithdrawlItemType) Properties["GetItemfromBankType"].Value; }
            set { Properties["GetItemfromBankType"].Value = value; }
        }

        [PbXmlAttribute]
        public string ItemID
        {
            get { return (string) Properties["ItemID"].Value; }
            set { Properties["ItemID"].Value = value; }
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
        public bool WithdrawAdditively
        {
            get { return (bool) Properties["WithdrawAdditively"].Value; }
            set { Properties["WithdrawAdditively"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string) Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_GetItemFromBankAction_Name"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("{0}: " + (GetItemfromBankType == BankWithdrawlItemType.SpecificItem
                                                    ? " {1} {2}"
                                                    : ""), Name, ItemID, Amount);
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_GetItemFromBankAction_Help"]; }
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
                    if (Util.BagRoomLeft(_itemList.Keys.FirstOrDefault()) <= MinFreeBagSlots || _itemList.Count == 0)
                        IsDone = true;
                    else
                    {
                        uint itemID = _itemList.Keys.FirstOrDefault();
                        bool done;
                        if (Bank == BankType.Personal)
                            done = GetItemFromBank(itemID, _itemList[itemID]);
                        else
                        {
                            // throttle the amount of items being withdrawn from gbank per sec
                            if (!_gbankItemThrottleSW.IsRunning)
                                _gbankItemThrottleSW.Start();
                            if (_gbankItemThrottleSW.ElapsedMilliseconds < GbankItemThrottle)
                                return RunStatus.Success;

                            int ret = GetItemFromGBank(itemID, _itemList[itemID]);
                            if (ret >= 0)
                            {
                                _gbankItemThrottleSW.Reset();
                                _gbankItemThrottleSW.Start();
                            }
                            _itemList[itemID] = ret < 0 ? 0 : _itemList[itemID] - ret;
                            done = _itemList[itemID] <= 0;
                        }
                        if (done)
                        {
                            if (itemID == 0)
                                Professionbuddy.Log("Done withdrawing all items from {0} Bank", Bank);
                            else
                                Professionbuddy.Log("Done withdrawing itemID:{0} from {1} Bank", itemID, Bank);
                            _itemList.Remove(itemID);
                        }
                    }
                    _itemsSW.Reset();
                    _itemsSW.Start();
                }
                return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        private WoWObject GetLocalBanker()
        {
            WoWObject bank = null;
            List<WoWObject> bankers;
            if (Bank == BankType.Guild)
                bankers = (from banker in ObjectManager.ObjectList
                           where
                               (banker is WoWGameObject &&
                                ((WoWGameObject) banker).SubType == WoWGameObjectType.GuildBank) ||
                               (banker is WoWUnit && ((WoWUnit) banker).IsGuildBanker && ((WoWUnit) banker).IsAlive &&
                                ((WoWUnit) banker).CanSelect)
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
            else if (AutoFindBank || _loc == WoWPoint.Zero || NpcEntry == 0)
                bank = bankers.OrderBy(o => o.Distance).FirstOrDefault();
            else if (ObjectManager.Me.Location.Distance(_loc) <= 90)
            {
                bank = bankers.Where(o => o.Location.Distance(_loc) < 10).
                    OrderBy(o => o.Distance).FirstOrDefault();
            }
            return bank;
        }

        private void MoveToBanker()
        {
            WoWPoint movetoPoint = _loc;
            WoWObject bank = GetLocalBanker();
            if (bank != null)
                movetoPoint = WoWMathHelper.CalculatePointFrom(Me.Location, bank.Location, 3);
                // search the database
            else if (movetoPoint == WoWPoint.Zero)
            {
                movetoPoint =
                    MoveToAction.GetLocationFromDB(
                        Bank == BankType.Personal
                            ? MoveToAction.MoveToType.NearestBanker
                            : MoveToAction.MoveToType.NearestGB, NpcEntry);
            }
            if (movetoPoint == WoWPoint.Zero)
            {
                IsDone = true;
                Professionbuddy.Err(Pb.Strings["Error_UnableToFindBank"]);
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
            var items = new Dictionary<uint, int>();
            switch (GetItemfromBankType)
            {
                case BankWithdrawlItemType.SpecificItem:
                    //List<uint> idList = new List<uint>();
                    string[] entries = ItemID.Split(',');
                    if (entries.Length > 0)
                    {
                        foreach (string entry in entries)
                        {
                            uint itemID;
                            uint.TryParse(entry.Trim(), out itemID);
                            if (WithdrawAdditively)
                                items.Add(itemID, Withdraw == DepositWithdrawAmount.All ? int.MaxValue : Amount);
                            else
                                items.Add(itemID, Amount - Util.GetCarriedItemCount(itemID));
                        }
                    }
                    break;
                case BankWithdrawlItemType.Materials:
                    foreach (var kv in Pb.MaterialList)
                        items.Add(kv.Key, kv.Value);
                    break;
            }
            return items;
        }

        // indexes are {0} = ItemID, {1} = amount to deposit

        /// <summary>
        /// Withdraws items from gbank
        /// </summary>
        /// <param name="id">item ID</param>
        /// <param name="amount">amount to withdraw.</param>
        /// <returns>the amount withdrawn.</returns>
        public int GetItemFromGBank(uint id, int amount)
        {
            if (_queueServerSW == null)
            {
                _queueServerSW = new Stopwatch();
                _queueServerSW.Start();
                Lua.DoString("for i=GetNumGuildBankTabs(), 1, -1 do QueryGuildBankTab(i) end ");
                Professionbuddy.Log("Queuing server for gbank info");
                return 0;
            }
            if (_queueServerSW.ElapsedMilliseconds < 2000)
            {
                return 0;
            }
            string lua = string.Format(WithdrawItemFromGBankLuaFormat, id, amount);
            var retVal = Lua.GetReturnVal<int>(lua, 0);
            // -1 means no item was found.
            if (retVal == -1)
            {
                Professionbuddy.Log("No items with entry {0} could be found in gbank", id);
            }
            return retVal;
        }

        // indexes are {0} = ItemID, {1} = amount to deposit

        public bool GetItemFromBank(uint id, int amount)
        {
            string lua = string.Format(WithdrawItemFromPersonalBankLuaFormat, id, amount);
            Lua.DoString(lua);
            return true;
        }

        public override void Reset()
        {
            base.Reset();
            _queueServerSW = null;
            _itemList = null;
            _itemsSW = null;
        }

        public override object Clone()
        {
            return new GetItemfromBankAction
                       {
                           ItemID = ItemID,
                           Amount = Amount,
                           Bank = Bank,
                           GetItemfromBankType = GetItemfromBankType,
                           _loc = _loc,
                           AutoFindBank = AutoFindBank,
                           NpcEntry = NpcEntry,
                           Location = Location,
                           MinFreeBagSlots = MinFreeBagSlots,
                           WithdrawAdditively = WithdrawAdditively,
                           Withdraw = Withdraw
                       };
        }
    }
}