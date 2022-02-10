using System.Collections.Generic;
using System.Linq;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Styx.Bot.Quest_Behaviors
{
    public class SetHearthstone : CustomForcedBehavior
    {
        private readonly string _goalText;
        private bool _done;

        public SetHearthstone(Dictionary<string, string> args)
            : base(args)
        {
            NpcId = GetAttributeAsNullable<int>("MobId", false, ConstrainAs.MobId, new[] { "NpcId" }) ?? 0;
            AreaId = GetAttributeAsNullable<int>("AreaId", false, ConstrainAs.MobId, null) ?? 0;
            Location = GetAttributeAsNullable<WoWPoint>("", false, ConstrainAs.WoWPointNonEmpty, null) ?? Me.Location;
            Name = GetAttributeAs<string>("Name", false, ConstrainAs.StringNonEmpty, null) ?? "";

            if (!string.IsNullOrEmpty(Name))
            {
                _goalText = "Setting Hearthstone at " + Name;
            }
            else
            {
                _goalText = "Setting Hearthstone at NPC #" + NpcId;
            }
        }

        public int NpcId { get; set; }
        public WoWPoint Location { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        private LocalPlayer Me { get { return (StyxWoW.Me); } }

        public override bool IsDone { get { return _done; } }

        private WoWUnit InnKeeper
        {
            get
            {
                if (NpcId == 0)
                {
                    return null;
                }

                return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).FirstOrDefault(u => u.IsInnkeeper && u.Entry == NpcId);
            }
        }

        public override void OnStart()
        {
            TreeRoot.GoalText = _goalText;
            Lua.Events.AttachEvent("CONFIRM_BINDER", HandleConfirmBinder);
        }

        private bool _confirmEventFired;
        private void HandleConfirmBinder(object sender, LuaEventArgs args)
        {
            Lua.DoString("ConfirmBinder(); StaticPopup_Hide('CONFIRM_BINDER')");
            Lua.Events.DetachEvent("CONFIRM_BINDER", HandleConfirmBinder);
            _confirmEventFired = true;
        }

        private bool GossipFrameActive()
        {
            return GossipFrame.Instance.IsVisible;
        }

        private bool StaticPopupActive()
        {
            return Lua.GetReturnVal<bool>("return StaticPopup1 and StaticPopup1:IsVisible()", 0);
        }

        private void SelectSetLocationGossipOption()
        {
            foreach (GossipEntry entry in GossipFrame.Instance.GossipOptionEntries)
            {
                if (entry.Type == GossipEntry.GossipEntryType.Binder)
                {
                    Logging.Write("Selecting gossip option: " + entry.Text + " - #" + entry.Index);
                    GossipFrame.Instance.SelectGossipOption(entry.Index);
                }
            }
        }

        private void EnsureBindingCorrect()
        {
            uint areaId = StyxWoW.Me.HearthstoneAreaId;
            uint zoneId = StyxWoW.Me.ZoneId;

            _done = areaId == zoneId;
        }

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => AreaId != 0 && StyxWoW.Me.HearthstoneAreaId == AreaId,
                    new Action(ret => _done = true)),
                new Decorator(
                    ret => InnKeeper != null,
                    new PrioritySelector(
                        ctx => InnKeeper,
                // Found the Innkeeper, but its a bit far away. Get within interact distance!
                        new Decorator(
                            ret => ((WoWUnit)ret).Distance > 4.5f,
                            new Action(ret => Navigator.MoveTo(((WoWUnit)ret).Location))),
                // Now, we open up the gossip frame, and see if we can find a 'set my location' option
                        new Sequence(
                // First, interact.
                            new Action(ret => ((WoWUnit)ret).Interact()),
                // Some inn keepers offer the option to select "make this your home",
                // while others just throw the popup at you. This should ensure both are handled!
                            new Wait(
                                5, ret => GossipFrameActive() || StaticPopupActive(),
                                new PrioritySelector(
                // If we even made it here, it means we have a window open.
                // The next part is trivial at best. FIRST, we deal with selecting
                // The "Make this my home" gossip option.
                                    new Decorator(
                                        ret => GossipFrameActive(),
                                        new Action(ret => SelectSetLocationGossipOption())),
                                    new Wait(
                                        5, ret => _confirmEventFired,
                                        new Action(ret => _done = true))
                                    )
                                )
                            )
                        )
                    ),
                new Action(ret => Navigator.MoveTo(Location))
                );
        }
    }
}