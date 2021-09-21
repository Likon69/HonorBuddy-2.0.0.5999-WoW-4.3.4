using System;
using System.Linq;

using CommonBehaviors.Actions;

using Singular.Settings;

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using System.Collections.Generic;
using Action = TreeSharp.Action;

namespace Singular.Helpers
{
    internal static class Item
    {
        public static bool HasItem(uint itemId)
        {
            return StyxWoW.Me.CarriedItems.Any(i => i.Entry == itemId);
        }

        public static bool HasWeaponImbue(WoWInventorySlot slot, string imbueName, int imbueId)
        {
            Logger.Write("Checking Weapon Imbue on " + slot + " for " + imbueName);
            var item = StyxWoW.Me.Inventory.Equipped.GetEquippedItem(slot);
            if (item == null)
            {
                Logger.Write("We have no " + slot + " equipped!");
                return true;
            }

            var enchant = item.TemporaryEnchantment;
            if (enchant != null)
                Logger.Write("VerzauberungsName: " + enchant.Name );
                Logger.Write("VerzauberungsID: " + enchant.Id);
                Logger.Write("ImbueName: " + imbueName);
                Logger.Write("Enchant: " + enchant.Name + " - " + (enchant.Name == imbueName));
            return enchant != null && enchant.Name == imbueName || imbueId == enchant.Id;
        }


        /// <summary>
        ///  Creates a behavior to use an equipped item.
        /// </summary>
        /// <param name="slot"> The slot number of the equipped item. </param>
        /// <returns></returns>
        public static Composite UseEquippedItem(uint slot)
        {
            return new PrioritySelector(
                ctx => StyxWoW.Me.Inventory.GetItemBySlot(slot),
                new Decorator(
                    ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                    new Action(ctx => UseItem((WoWItem)ctx))));

        }

        /// <summary>
        ///  Creates a behavior to use an item, in your bags or paperdoll.
        /// </summary>
        /// <param name="id"> The entry of the item to be used. </param>
        /// <returns></returns>
        public static Composite UseItem(uint id)
        {
            return new PrioritySelector(
                ctx => ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == id),
                new Decorator(
                    ctx => ctx != null && CanUseItem((WoWItem)ctx),
                    new Action(ctx => UseItem((WoWItem)ctx))));
        }

        private static bool CanUseItem(WoWItem item)
        {
            return item.Usable && item.Cooldown <= 0;
        }

        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")",0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;

            return item.Usable && item.Cooldown <= 0;
        }

        private static void UseItem(WoWItem item)
        {
            Logger.Write("Using item: " + item.Name);
            item.Use();
        }

        /// <summary>
        ///  Checks for items in the bag, and returns the first item that has an usable spell from the specified string array.
        /// </summary>
        /// <param name="spellNames"> Array of spell names to be check.</param>
        /// <returns></returns>
        public static WoWItem FindFirstUsableItemBySpell(params string[] spellNames)
        {
            List<WoWItem> carried = StyxWoW.Me.CarriedItems;
            // Yes, this is a bit of a hack. But the cost of creating an object each call, is negated by the speed of the Contains from a hash set.
            // So take your optimization bitching elsewhere.
            var spellNameHashes = new HashSet<string>(spellNames);

            return (from i in carried
                    let spells = i.ItemSpells
                    where i.ItemInfo != null && spells != null && spells.Count != 0 &&
                          i.Usable &&
                          i.Cooldown == 0 &&
                          i.ItemInfo.RequiredLevel <= StyxWoW.Me.Level &&
                          spells.Any(s => s.IsValid && s.ActualSpell != null && spellNameHashes.Contains(s.ActualSpell.Name))
                    orderby i.ItemInfo.Level descending
                    select i).FirstOrDefault();
        }

        /// <summary>
        ///  Returns true if you have a wand equipped, false otherwise.
        /// </summary>
        public static bool HasWand
        {
            get
            {
                return StyxWoW.Me.Inventory.Equipped.Ranged != null &&
                       StyxWoW.Me.Inventory.Equipped.Ranged.ItemInfo.WeaponClass == WoWItemWeaponClass.Wand;
            }
        }

        /// <summary>
        ///   Creates a composite to use potions and healthstone.
        /// </summary>
        /// <param name = "healthPercent">Healthpercent to use health potions and healthstone</param>
        /// <param name = "manaPercent">Manapercent to use mana potions</param>
        /// <returns></returns>
        public static Composite CreateUsePotionAndHealthstone(double healthPercent, double manaPercent)
        {
            return new PrioritySelector(
                new Decorator(
                    ret => StyxWoW.Me.HealthPercent < healthPercent,
                    new PrioritySelector(
                        ctx => FindFirstUsableItemBySpell("Healthstone", "Healing Potion"),
                        new Decorator(
                            ret => ret != null,
                            new Sequence(
                                new Action(ret => Logger.Write(String.Format("Using {0}", ((WoWItem)ret).Name))),
                                new Action(ret => ((WoWItem)ret).UseContainerItem()),
                                Helpers.Common.CreateWaitForLagDuration()))
                        )),
                new Decorator(
                    ret => StyxWoW.Me.ManaPercent < manaPercent,
                    new PrioritySelector(
                        ctx => FindFirstUsableItemBySpell("Restore Mana"),
                        new Decorator(
                            ret => ret != null,
                            new Sequence(
                                new Action(ret => Logger.Write(String.Format("Using {0}", ((WoWItem)ret).Name))),
                                new Action(ret => ((WoWItem)ret).UseContainerItem()),
                                Helpers.Common.CreateWaitForLagDuration()))))
                );
        }


        public static bool UseTrinket(bool firstSlot)
        {
            TrinketUsage usage = firstSlot ? SingularSettings.Instance.Trinket1Usage : SingularSettings.Instance.Trinket2Usage;

            // If we're not going to use it, don't bother going any further. Save some performance here.
            if (usage == TrinketUsage.Never)
            {
                return false;
            }

            WoWItem item = firstSlot ? StyxWoW.Me.Inventory.Equipped.Trinket1 : StyxWoW.Me.Inventory.Equipped.Trinket2;
            //int percent = firstSlot ? SingularSettings.Instance.FirstTrinketUseAtPercent : SingularSettings.Instance.SecondTrinketUseAtPercent;

            if (item == null)
            {
                return false;
            }

            if (!CanUseEquippedItem(item))
                return false;

            bool useIt = false;
            switch (usage)
            {
                case TrinketUsage.OnCooldown:
                    // We know its off cooldown... so just use it :P
                    useIt = true;
                    break;
                case TrinketUsage.OnCooldownInCombat:
                    if (StyxWoW.Me.Combat)
                    {
                        useIt = true;
                    }
                    break;
                //case TrinketUsage.LowPower:
                //    // We use the PowerPercent here, since it applies to ALL types of power. (Runic, Mana, Rage, Energy, Focus)
                //    if (StyxWoW.Me.PowerPercent < percent)
                //    {
                //        useIt = true;
                //    }
                //    break;
                //case TrinketUsage.LowHealth:
                //    if (StyxWoW.Me.HealthPercent < percent)
                //    {
                //        useIt = true;
                //    }
                    break;
            }

            if (useIt)
            {
                Logger.Write("Popping trinket " + item.Name);
                item.Use();
                return true;
            }
            return false;
        }
        public static Composite CreateUseAlchemyBuffsBehavior()
        {
            return new PrioritySelector(
                new Decorator(
                    ret =>
                    SingularSettings.Instance.UseAlchemyFlasks && StyxWoW.Me.GetSkill(SkillLine.Alchemy).CurrentValue >= 400 &&
                    !StyxWoW.Me.Auras.Any(aura => aura.Key.StartsWith("Enhanced ") || aura.Key.StartsWith("Flask of ")), // don't try to use the flask if we already have or if we're using a better one
                    new PrioritySelector(
                        ctx => StyxWoW.Me.CarriedItems.FirstOrDefault(i => i.Entry == 58149) ?? StyxWoW.Me.CarriedItems.FirstOrDefault(i => i.Entry == 47499),
                // Flask of Enhancement / Flask of the North
                        new Decorator(
                            ret => ret != null,
                            new Sequence(
                                new Action(ret => Logger.Write(String.Format("Using {0}", ((WoWItem)ret).Name))),
                                new Action(ret => ((WoWItem)ret).UseContainerItem()),
                                Helpers.Common.CreateWaitForLagDuration()))
                        ))
                );
        }


        public static Composite CreateUseTrinketsBehavior()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => SingularSettings.Instance.Trinket1,
                    new Decorator(
                        ret => UseTrinket(true),
                        new ActionAlwaysSucceed())),
                new Decorator(
                    ret => SingularSettings.Instance.Trinket2,
                    new Decorator(
                        ret => UseTrinket(false),
                        new ActionAlwaysSucceed()))
                );
        }

        public static bool RangedIsType(WoWItemWeaponClass wepType)
        {
            var ranged = StyxWoW.Me.Inventory.Equipped.Ranged;
            if (ranged != null && ranged.IsValid)
            {
                return ranged.ItemInfo != null && ranged.ItemInfo.WeaponClass == wepType;
            }
            return false;
        }
    }
}
