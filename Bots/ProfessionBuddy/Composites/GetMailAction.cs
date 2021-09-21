using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.MailBox;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{

    #region GetMailAction

    internal sealed class GetMailAction : PBAction
    {
        #region GetMailActionType enum

        public enum GetMailActionType
        {
            AllItems,
            Specific,
        }

        #endregion

        private const string MailFormat =
            "local numItems,totalItems = GetInboxNumItems() " +
            "local foundMail=0 " +
            "local newMailCheck = {0} " +
            "for index=numItems,1,-1 do " +
            "local _,_,sender,subj,gold,cod,_,itemCnt,_,_,hasText,canReply,IsGM=GetInboxHeaderInfo(index) " +
            "if sender ~= nil and cod == 0 and itemCnt == nil and gold == 0 and canReply == nil and IsGM == nil then " +
            "DeleteInboxItem(index) " +
            "end " +
            "if cod == 0 and ((itemCnt and itemCnt >0) or (gold and gold > 0)) then " +
            "for i=1,ATTACHMENTS_MAX_RECEIVE do " +
            "if gold and gold > 0 then TakeInboxMoney(index) foundMail = 1 break end " +
            "if GetInboxItem(index,i) ~= nil then " +
            "TakeInboxItem (index,i) " +
            "foundMail = 1 " +
            "break " +
            "end " +
            "end " +
            "end " +
            "if foundMail == 1 then break end " +
            "end " +
            "local beans = BeanCounterMail and BeanCounterMail:IsVisible() " +
            "if foundMail == 0 and ((newMailCheck == 1 and HasNewMail() == nil) or newMailCheck ==0 ) and totalItems == numItems and beans ~= 1 then return 1 else return 0 end ";

        // format index. {0} = ItemID {1}=CheckForNewMail which can be only 1 or 0
        private const string MailByIdFormat =
            "local numItems,totalItems = GetInboxNumItems() " +
            "local foundMail=0 " +
            "local newMailCheck = {1} " +
            "for index=numItems,1,-1 do " +
            "local _,_,sender,subj,gold,cod,_,itemCnt,_,_,hasText=GetInboxHeaderInfo(index) " +
            "if sender ~= nil and cod == 0 and itemCnt == nil and gold == 0 and hasText == nil then " +
            "DeleteInboxItem(index) " +
            "end " +
            "if cod == 0 and itemCnt and itemCnt >0  then " +
            "for i2=1, ATTACHMENTS_MAX_RECEIVE do " +
            "local itemlink = GetInboxItemLink(index, i2) " +
            "if itemlink ~= nil and string.find(itemlink,'{0}') then " +
            "foundMail = foundMail + 1 " +
            "TakeInboxItem(index, i2) " +
            "break " +
            "end " +
            "end " +
            "end " +
            "if foundMail == 1 then break end " +
            "end " +
            "if (foundMail == 0 and ((newMailCheck == 1 and HasNewMail() == nil) or newMailCheck ==0 )) or (foundMail == 0 and (numItems == 50 and totalItems >= 50)) then return 1 else return 0 end ";

        private Stopwatch _concludingSW = new Stopwatch();
        private List<uint> _idList;
        private WoWPoint _loc;
        private WoWGameObject _mailbox;
        private Stopwatch _refreshInboxSW = new Stopwatch();
        private Stopwatch _throttleSW = new Stopwatch();
        private Stopwatch _timeoutSW = new Stopwatch();
        private Stopwatch _waitForContentToShowSW = new Stopwatch();

        public GetMailAction()
        {
            //CheckNewMail
            Properties["ItemID"] = new MetaProp("ItemID", typeof (string),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_ItemEntries"]));

            Properties["MinFreeBagSlots"] = new MetaProp("MinFreeBagSlots", typeof (int),
                                                         new DisplayNameAttribute(
                                                             Pb.Strings["Action_Common_MinFreeBagSlots"]));

            Properties["CheckNewMail"] = new MetaProp("CheckNewMail", typeof (bool),
                                                      new DisplayNameAttribute(
                                                          Pb.Strings["Action_GetMailAction_CheckNewMail"]));

            Properties["GetMailType"] = new MetaProp("GetMailType", typeof (GetMailActionType),
                                                     new DisplayNameAttribute(Pb.Strings["Action_GetMailAction_Name"]));

            Properties["AutoFindMailBox"] = new MetaProp("AutoFindMailBox", typeof (bool),
                                                         new DisplayNameAttribute(
                                                             Pb.Strings["Action_Common_AutoFindMailbox"]));

            Properties["Location"] = new MetaProp("Location", typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            ItemID = "";
            CheckNewMail = true;
            GetMailType = GetMailActionType.AllItems;
            AutoFindMailBox = true;
            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            MinFreeBagSlots = 0;

            Properties["GetMailType"].PropertyChanged += GetMailActionPropertyChanged;
            Properties["AutoFindMailBox"].PropertyChanged += AutoFindMailBoxChanged;
            Properties["ItemID"].Show = false;
            Properties["Location"].Show = false;
            Properties["Location"].PropertyChanged += LocationChanged;
        }

        [PbXmlAttribute]
        public GetMailActionType GetMailType
        {
            get { return (GetMailActionType) Properties["GetMailType"].Value; }
            set { Properties["GetMailType"].Value = value; }
        }

        [PbXmlAttribute]
        [PbXmlAttribute("Entry")]
        public string ItemID
        {
            get { return (string) Properties["ItemID"].Value; }
            set { Properties["ItemID"].Value = value; }
        }

        [PbXmlAttribute]
        public bool CheckNewMail
        {
            get { return (bool) Properties["CheckNewMail"].Value; }
            set { Properties["CheckNewMail"].Value = value; }
        }

        [PbXmlAttribute]
        public int MinFreeBagSlots
        {
            get { return (int) Properties["MinFreeBagSlots"].Value; }
            set { Properties["MinFreeBagSlots"].Value = value; }
        }

        [PbXmlAttribute]
        public bool AutoFindMailBox
        {
            get { return (bool) Properties["AutoFindMailBox"].Value; }
            set { Properties["AutoFindMailBox"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string) Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_GetMailAction_Name"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("{0}: {1} " + (GetMailType == GetMailActionType.Specific
                                                        ? " - " +
                                                          ItemID
                                                        : ""), Name, GetMailType);
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_GetMailAction_Help"]; }
        }

        private void LocationChanged(object sender, MetaPropArgs e)
        {
            _loc = Util.StringToWoWPoint((string) ((MetaProp) sender).Value);
            Properties["Location"].PropertyChanged -= LocationChanged;
            Properties["Location"].Value = string.Format("{0}, {1}, {2}", _loc.X, _loc.Y, _loc.Z);
            Properties["Location"].PropertyChanged += LocationChanged;
            RefreshPropertyGrid();
        }

        private void AutoFindMailBoxChanged(object sender, MetaPropArgs e)
        {
            Properties["Location"].Show = !AutoFindMailBox;
            RefreshPropertyGrid();
        }

        private void GetMailActionPropertyChanged(object sender, MetaPropArgs e)
        {
            Properties["ItemID"].Show = GetMailType != GetMailActionType.AllItems;
            RefreshPropertyGrid();
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if (!_timeoutSW.IsRunning)
                    _timeoutSW.Start();
                if (_timeoutSW.ElapsedMilliseconds > 300000)
                    IsDone = true;
                WoWPoint movetoPoint = _loc;
                if (MailFrame.Instance == null || !MailFrame.Instance.IsVisible)
                {
                    if (AutoFindMailBox || movetoPoint == WoWPoint.Zero)
                    {
                        _mailbox =
                            ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                                o => o.SubType == WoWGameObjectType.Mailbox)
                                .OrderBy(o => o.Distance).FirstOrDefault();
                    }
                    else
                    {
                        _mailbox =
                            ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                                o => o.SubType == WoWGameObjectType.Mailbox
                                     && o.Location.Distance(_loc) < 10)
                                .OrderBy(o => o.Distance).FirstOrDefault();
                    }
                    if (_mailbox != null)
                        movetoPoint = WoWMathHelper.CalculatePointFrom(Me.Location, _mailbox.Location, 3);
                    if (movetoPoint == WoWPoint.Zero)
                    {
                        Professionbuddy.Err(Pb.Strings["Error_UnableToFindMailbox"]);
                        return RunStatus.Failure;
                    }

                    if (movetoPoint.Distance(ObjectManager.Me.Location) > 4.5)
                        Util.MoveTo(movetoPoint);
                    else if (_mailbox != null)
                    {
                        _mailbox.Interact();
                    }
                    return RunStatus.Success;
                }
                // mail frame is open.
                if (_idList == null)
                    _idList = BuildItemList();
                if (!_refreshInboxSW.IsRunning)
                    _refreshInboxSW.Start();
                if (!_waitForContentToShowSW.IsRunning)
                    _waitForContentToShowSW.Start();
                if (_waitForContentToShowSW.ElapsedMilliseconds < 3000)
                    return RunStatus.Success;

                if (!_concludingSW.IsRunning)
                {
                    if (_refreshInboxSW.ElapsedMilliseconds < 64000)
                    {
                        if (MinFreeBagSlots > 0 && Me.FreeNormalBagSlots - MinFreeBagSlots <= 4)
                        {
                            if (!_throttleSW.IsRunning)
                                _throttleSW.Start();
                            if (_throttleSW.ElapsedMilliseconds < 4000 - (Me.FreeNormalBagSlots - MinFreeBagSlots)*1000)
                                return RunStatus.Success;
                            _throttleSW.Reset();
                            _throttleSW.Start();
                        }
                        if (GetMailType == GetMailActionType.AllItems)
                        {
                            string lua = string.Format(MailFormat, CheckNewMail ? 1 : 0);
                            if (Me.FreeNormalBagSlots <= MinFreeBagSlots || Lua.GetReturnValues(lua)[0] == "1")
                                _concludingSW.Start();
                        }
                        else
                        {
                            if (_idList.Count > 0 && Me.FreeNormalBagSlots > MinFreeBagSlots)
                            {
                                string lua = string.Format(MailByIdFormat, _idList[0], CheckNewMail ? 1 : 0);

                                if (Lua.GetReturnValues(lua)[0] == "1")
                                    _idList.RemoveAt(0);
                            }
                            else
                                _concludingSW.Start();
                        }
                    }
                    else
                    {
                        _refreshInboxSW.Reset();
                        MailFrame.Instance.Close();
                    }
                }
                if (_concludingSW.ElapsedMilliseconds > 2000)
                    IsDone = true;
                if (IsDone)
                {
                    Professionbuddy.Log("Mail retrieval of items:{0} finished", GetMailType);
                }
                else
                    return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        private List<uint> BuildItemList()
        {
            var list = new List<uint>();
            string[] entries = ItemID.Split(',');
            if (entries.Length > 0)
            {
                foreach (string entry in entries)
                {
                    uint temp;
                    uint.TryParse(entry.Trim(), out temp);
                    list.Add(temp);
                }
            }
            else
            {
                Professionbuddy.Err(Pb.Strings["Error_NoItemEntries"]);
                IsDone = true;
            }
            return list;
        }

        public override void Reset()
        {
            base.Reset();
            _waitForContentToShowSW = new Stopwatch();
            _concludingSW = new Stopwatch();
            _timeoutSW = new Stopwatch();
            _refreshInboxSW = new Stopwatch();
            _throttleSW = new Stopwatch();
        }

        public override object Clone()
        {
            return new GetMailAction
                       {
                           ItemID = ItemID,
                           GetMailType = GetMailType,
                           _loc = _loc,
                           AutoFindMailBox = AutoFindMailBox,
                           Location = Location,
                           MinFreeBagSlots = MinFreeBagSlots,
                           CheckNewMail = CheckNewMail,
                       };
        }
    }

    #endregion
}