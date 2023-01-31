using System.Collections.Generic;
using System.Linq;

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

// Full Credit to singular for this class (why re-invent the wheel?)
// Settings within GUI comming soon

namespace CLU.Helpers
{


    using CommonBehaviors.Actions;

    using Styx.Logic.Pathing;

    using TreeSharp;

    using global::CLU.GUI;

    public enum PoisonType
    {
        Instant,
        Crippling,
        MindNumbing,
        Deadly,
        Wound
    }

    public static class Poisons
    {
        static Poisons()
        {
            Lua.Events.AttachEvent("END_BOUND_TRADEABLE", HandleEndBoundTradeable);
        }

        private static void HandleEndBoundTradeable(object sender, LuaEventArgs args)
        {
            Lua.DoString("EndBoundTradeable(" + args.Args[0] + ")");
        }

        private static readonly HashSet<uint> InstantPoisons = new HashSet<uint> { 6947, 43231 };

        private static readonly HashSet<uint> CripplingPoisons = new HashSet<uint> { 3775 };

        private static readonly HashSet<uint> MindNumbingPoisons = new HashSet<uint> { 5237 };

        private static readonly HashSet<uint> DeadlyPoisons = new HashSet<uint> { 2892, 43233 };

        private static readonly HashSet<uint> WoundPoisons = new HashSet<uint> { 10918, 43235 };

        public static bool MainHandNeedsPoison { get { return StyxWoW.Me.Inventory.Equipped.MainHand != null && StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0; } }

        public static bool OffHandNeedsPoison { get { return StyxWoW.Me.Inventory.Equipped.OffHand != null && StyxWoW.Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == 0; } }

        public static bool ThrownNeedsPoison { get { return StyxWoW.Me.Inventory.Equipped.Ranged != null && StyxWoW.Me.Inventory.Equipped.Ranged.IsThrownWeapon && StyxWoW.Me.Inventory.Equipped.Ranged.TemporaryEnchantment.Id == 0; } }

        public static WoWItem MainHandPoison
        {
            get
            {
                switch (SettingsFile.Instance.MHPoison)
                {
                    case PoisonType.Instant:
                        return StyxWoW.Me.CarriedItems.Where(i => InstantPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Crippling:
                        return StyxWoW.Me.CarriedItems.Where(i => CripplingPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.MindNumbing:
                        return
                            StyxWoW.Me.CarriedItems.Where(i => MindNumbingPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Deadly:
                        return StyxWoW.Me.CarriedItems.Where(i => DeadlyPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Wound:
                        return StyxWoW.Me.CarriedItems.Where(i => WoundPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    default:
                        return null;
                }
            }
        }

        public static WoWItem OffHandPoison
        {
            get
            {
                switch (SettingsFile.Instance.OHPoison)
                {
                    case PoisonType.Instant:
                        return StyxWoW.Me.CarriedItems.Where(i => InstantPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Crippling:
                        return StyxWoW.Me.CarriedItems.Where(i => CripplingPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.MindNumbing:
                        return
                            StyxWoW.Me.CarriedItems.Where(i => MindNumbingPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Deadly:
                        return StyxWoW.Me.CarriedItems.Where(i => DeadlyPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Wound:
                        return StyxWoW.Me.CarriedItems.Where(i => WoundPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    default:
                        return null;
                }
            }
        }

        public static WoWItem ThrownPoison
        {
            get
            {
                switch (SettingsFile.Instance.ThrownPoison)
                {
                    case PoisonType.Instant:
                        return StyxWoW.Me.CarriedItems.Where(i => InstantPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Crippling:
                        return StyxWoW.Me.CarriedItems.Where(i => CripplingPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.MindNumbing:
                        return
                            StyxWoW.Me.CarriedItems.Where(i => MindNumbingPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Deadly:
                        return StyxWoW.Me.CarriedItems.Where(i => DeadlyPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    case PoisonType.Wound:
                        return StyxWoW.Me.CarriedItems.Where(i => WoundPoisons.Contains(i.Entry)).OrderByDescending(i => i.Entry).FirstOrDefault();
                    default:
                        return null;
                }
            }
        }

        public static Composite CreateApplyPoisons()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => MainHandNeedsPoison && MainHandPoison != null,
                    new Sequence(
                        new Action(ret => CLU.Instance.Log("Applying {0} to main hand", MainHandPoison.Name)),
                        new Action(ret => Navigator.PlayerMover.MoveStop()),
                        Spell.CreateWaitForLagDuration(),
                        new Action(ret => MainHandPoison.UseContainerItem()),
                        Spell.CreateWaitForLagDuration(),
                        new Action(ret => Lua.DoString("UseInventoryItem(16)")),
                        new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                        new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                        new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))),
                new Decorator(
                    ret => OffHandNeedsPoison && OffHandPoison != null,
                    new Sequence(
                        new Action(ret => CLU.Instance.Log("Applying {0} to off hand", OffHandPoison.Name)),
                        new Action(ret => Navigator.PlayerMover.MoveStop()),
                        Spell.CreateWaitForLagDuration(),
                        new Action(ret => OffHandPoison.UseContainerItem()),
                        Spell.CreateWaitForLagDuration(),
                        new Action(ret => Lua.DoString("UseInventoryItem(17)")),
                        new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                        new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                        new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))),
                new Decorator(
                    ret => ThrownNeedsPoison && ThrownPoison != null,
                    new Sequence(
                        new Action(ret => CLU.Instance.Log("Applying {0} to main hand", ThrownPoison.Name)),
                        new Action(ret => Navigator.PlayerMover.MoveStop()),
                        Spell.CreateWaitForLagDuration(),
                        new Action(ret => ThrownPoison.UseContainerItem()),
                        Spell.CreateWaitForLagDuration(),
                        new Action(ret => Lua.DoString("UseInventoryItem(18)")),
                        new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                        new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                        new WaitContinue(1, ret => false, new ActionAlwaysSucceed())))
                );
        }
    }
}
