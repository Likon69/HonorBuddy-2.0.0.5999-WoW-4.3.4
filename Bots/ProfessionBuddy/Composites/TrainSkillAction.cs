using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Inventory.Frames.Trainer;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace HighVoltz.Composites
{
    internal sealed class TrainSkillAction : PBAction
    {
        private WoWPoint _loc;

        public TrainSkillAction()
        {
            Properties["Location"] = new MetaProp("Location", typeof(string),
                                                  new EditorAttribute(typeof(PropertyBag.LocationEditor),
                                                                      typeof(UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Properties["NpcEntry"] = new MetaProp("NpcEntry", typeof(uint),
                                                  new EditorAttribute(typeof(PropertyBag.EntryEditor),
                                                                      typeof(UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_NpcEntry"]));

            _loc = WoWPoint.Zero;
            Location = _loc.ToInvariantString();
            NpcEntry = 0u;

            Properties["Location"].PropertyChanged += LocationChanged;
        }

        [PbXmlAttribute]
        public uint NpcEntry
        {
            get { return (uint)Properties["NpcEntry"].Value; }
            set { Properties["NpcEntry"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string)Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_TrainSkillAction_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("{0}: {1}", Name, NpcEntry); }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_TrainSkillAction_Help"]; }
        }

        private void LocationChanged(object sender, MetaPropArgs e)
        {
            _loc = Util.StringToWoWPoint((string)((MetaProp)sender).Value);
            Properties["Location"].PropertyChanged -= LocationChanged;
            Properties["Location"].Value = string.Format("{0}, {1}, {2}", _loc.X, _loc.Y, _loc.Z);
            Properties["Location"].PropertyChanged += LocationChanged;
            RefreshPropertyGrid();
        }

        readonly WaitTimer _trainWaitTimer = new WaitTimer(TimeSpan.FromSeconds(2));
        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if (TrainerFrame.Instance == null || !TrainerFrame.Instance.IsVisible || !ObjectManager.Me.GotTarget ||
                    (ObjectManager.Me.GotTarget && ObjectManager.Me.CurrentTarget.Entry != NpcEntry))
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
                        if (Me.IsMoving)
                            WoWMovement.MoveStop();
                        unit.Target();
                        unit.Interact();
                    }
                    if (GossipFrame.Instance != null && GossipFrame.Instance.IsVisible &&
                        GossipFrame.Instance.GossipOptionEntries != null)
                    {
                        foreach (GossipEntry ge in GossipFrame.Instance.GossipOptionEntries)
                        {
                            if (ge.Type == GossipEntry.GossipEntryType.Trainer)
                            {
                                GossipFrame.Instance.SelectGossipOption(ge.Index);
                                return RunStatus.Success;
                            }
                        }
                    }
                    return RunStatus.Success;
                }
                if (_trainWaitTimer.IsFinished)
                {
                    using (new FrameLock())
                    {
                        Lua.DoString("SetTrainerServiceTypeFilter('available', 1)");
                        // check if there is any abilities to that need training.
                        var numOfAvailableAbilities =
                            Lua.GetReturnVal<int>(
                                "local a=0 for n=GetNumTrainerServices(),1,-1 do if select(3,GetTrainerServiceInfo(n)) == 'available' then a=a+1 end end return a ",
                                0);
                        if (numOfAvailableAbilities == 0)
                        {
                            IsDone = true;
                            Professionbuddy.Log("Done training");
                           return RunStatus.Failure;
                        }
                        Lua.DoString("BuyTrainerService(0) ");
                        _trainWaitTimer.Reset();
                    }
                }
                return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        public override object Clone()
        {
            return new TrainSkillAction { NpcEntry = NpcEntry, _loc = _loc, Location = Location };
        }
    }
}