using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MrFishIt.Forms;
using Styx;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.LootFrame;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace MrFishIt
{
    public class Fisher : BotBase
    {
        #region Overrides of BotBase

        private readonly Version _version = new Version(1, 0, 5);

        public override string Name
        {
            get { return "MrFishIt [" + _version + "]"; }
        }

        private Composite _root;
        public override Composite Root
        {
            get
            {
                return _root ?? (_root = 
                    new PrioritySelector(
                        CreateFishBehavior()
                        ));
            }
        }

        public override PulseFlags PulseFlags { get { return PulseFlags.All; } }

        public override Form ConfigurationForm { get { return new FormFishConfig(); } }

        public override void Start()
        {
            ProtectedItemsManager.Add((uint)FisherSettings.Instance.LureId);

            // Load an empty profile
            if (Directory.Exists(Path.Combine(Logging.ApplicationPath, "Bots")) && Directory.Exists(Path.Combine(Logging.ApplicationPath, @"Bots\MrFishIt")))
            {
                string emtpyProfile = Path.Combine(Logging.ApplicationPath, @"Bots\MrFishIt\EmptyProfile.xml");

                if (File.Exists(emtpyProfile))
                {
                    ProfileManager.LoadNew(emtpyProfile, false);
                    return;
                }
                        
                throw new HonorbuddyUnableToStartException("Couldn't find empty profile for MrFishIt!");
            }

            Lua.Events.AttachEvent("LOOT_OPENED", HandleLootOpened);
            // Make sure we don't get logged out
            StyxSettings.Instance.LogoutForInactivity = false;
        }

        public override void Stop()
        {
            // Set it back
            StyxSettings.Instance.LogoutForInactivity = true;
            Lua.Events.DetachEvent("LOOT_OPENED", HandleLootOpened);
        }

        #endregion

        private static void HandleLootOpened(object sender, LuaEventArgs args)
        {
            Lua.DoString("for i=1, GetNumLootItems() do LootSlot(i) ConfirmBindOnUse() end CloseLoot()");
        }

        private static Composite CreateFishBehavior()
        {
            return new PrioritySelector(

                // Don't do anything if the global cooldown timer is running
                new Decorator(ret => StyxWoW.GlobalCooldown,
                    new ActionIdle()),

                // Do we need to interact with the bobber?
                new Decorator(ret => Fishing.IsBobberBobbing,
                    new Sequence(
                        // Interact with the bobber
                        new Action(delegate
                        {
                            Logging.Write("Got a bite!");
                            WoWGameObject bobber = Fishing.FishingBobber;

                            if (bobber != null)
                                bobber.Interact();

                            else
                                return RunStatus.Failure;

                            return RunStatus.Success;
                        }),

                        // Wait for the lootframe
                        new Wait(5, ret => LootFrame.Instance.IsVisible,
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Looting"),
                                new Action(ret => StyxWoW.SleepForLagDuration())
                                ))
                            )),

                // Do we need to relure?
                new DecoratorNeedToRelure(
                    new Sequence(
                        new Action(delegate
                        {
                            var lureId = FisherSettings.Instance.LureId;

                            // Weather-beaten fishing hat
                            if (lureId == 33820)
                                Lua.DoString("UseInventoryItem(1)");

                            else
                            {
                                WoWItem item =
                                    StyxWoW.Me.CarriedItems.FirstOrDefault(
                                        ret => ret.Entry == lureId);

                                if(item == null)
                                {
                                    Logging.Write(Color.Red, "Could not find lure with id:{0}, won't lure anymore!", lureId);
                                    FisherSettings.Instance.UseLure = false;
                                }
                                else
                                    item.UseContainerItem();
                            }

                            Logging.Write("Reluring");
                            return RunStatus.Success;
                        }),

                        // Wait until we got a lure on our pole
                        new Wait(5, ret => Fishing.GotLure,
                            new ActionIdle())
                        )),

                // Do we need to recast?
                new Decorator(ret => Fishing.FishingPole != null && !Fishing.IsFishing,
                    new Sequence(
                        new Action(ret => Logging.Write("Casting")),
                        new Action(ret => SpellManager.Cast("Fishing")),
                        new Wait(5, ret => !StyxWoW.Me.IsCasting, new ActionIdle()),
                        new Action(ret => StyxWoW.SleepForLagDuration())
                        )),

                new Sequence(
                    new Action(ret => TreeRoot.StatusText = "Waiting for bobber to splash"),
                    new ActionIdle()
                    )
                );
        }
    }

    internal class DecoratorNeedToRelure : Decorator
    {
        public DecoratorNeedToRelure(Composite decorated)
            : base(decorated)
        {
        }

        protected override bool CanRun(object context)
        {
            return
                FisherSettings.Instance.UseLure &&
                Fishing.FishingPole != null &&
                !Fishing.IsFishing &&
                FisherSettings.Instance.LureId != 0 &&
                !Fishing.GotLure;
        }
    }

    internal class ActionCast : Action
    {
        protected override RunStatus Run(object context)
        {
            var spell = (SpellManager.Spells.Values.Where(
                b => b.Id == 7620 ||
                    b.Id == 7731 ||
                    b.Id == 7732 ||
                    b.Id == 18248 ||
                    b.Id == 33095 ||
                    b.Id == 51294 ||
                    b.Id == 62734 ||
                    b.Id == 88868)).FirstOrDefault();

            if (spell != null)
            {
                Logging.Write("[Fishing]: Casting");
                spell.Cast();
            }

            return RunStatus.Success;
        }
    }

    internal class ActionIdle : Action
    {
        protected override RunStatus Run(object context)
        {
            if (Parent is Selector || Parent is Decorator)
                return RunStatus.Success;

            return RunStatus.Failure;
        }
    }
}
