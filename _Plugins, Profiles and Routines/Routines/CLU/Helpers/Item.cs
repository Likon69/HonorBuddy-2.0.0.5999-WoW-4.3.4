using System.Linq;
using CLU.Lists;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace CLU.Helpers
{
    using Styx.Combat.CombatRoutine;

    using Action = TreeSharp.Action;

    public class Item
    {
        /* putting all the Item logic here */

        private static readonly Item ItemInstance = new Item();

        /// <summary>
        /// An instance of the Item Class
        /// </summary>
        public static Item Instance { get { return ItemInstance; } }

        /// <summary>
        /// erm...Me!
        /// </summary>
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        /// <summary>
        /// Checks the number of Tier pieces a player is wearing
        /// </summary>
        /// <param name="txxItemSetId">The Tier set ID</param>
        /// <returns>The total Number of Tier pieces</returns>
        private static int NumTierPieces(int txxItemSetId)
        {
            {
                int
                count = Me.Inventory.Equipped.Hands.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Legs.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Chest.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Shoulder.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                count += Me.Inventory.Equipped.Head.ItemInfo.ItemSetId == txxItemSetId ? 1 : 0;
                return count;
            }
        }

        /// <summary>Returns true if player has 2pc Tier set Bonus</summary>
        /// <param name="txxItemSetId">The txx Item Set Id.</param>
        /// <returns>The has 2 pc teir bonus.</returns>
        public bool Has2PcTeirBonus(int txxItemSetId)
        {
            {
                return NumTierPieces(txxItemSetId) >= 2;
            }
        }

        /// <summary>Returns true if player has 4pc Tier set Bonus</summary>
        /// <param name="txxItemSetId">The txx Item Set Id.</param>
        /// <returns>The has 4 pc teir bonus.</returns>
        public bool Has4PcTeirBonus(int txxItemSetId)
        {
            {
                return NumTierPieces(txxItemSetId) == 4;
            }
        }

        /// <summary>
        /// returns true if we have a weapon imbued
        /// </summary>
        /// <param name="slot">Inventory slot to check</param>
        /// <param name="imbueName">The name of the enchant</param>
        /// <returns>true if the weapon has the enchant</returns>
        public static bool HasWeaponImbue(WoWInventorySlot slot, string imbueName)
        {
            var item = StyxWoW.Me.Inventory.Equipped.GetEquippedItem(slot);
            if (item == null)
            {
                // CLU.Instance.Log("We have no " + slot + " equipped!");
                return true;
            }

            var enchant = item.TemporaryEnchantment;

            return enchant != null && enchant.Name == imbueName;
        }


        /// <summary>
        ///  Returns true if we meet the requirements of the weapon
        /// </summary>
        /// <param name="slot">Inventory slot to check</param>
        /// <returns>true if the weapon is suitable</returns>
        public static bool HasSuitableWeapon(WoWInventorySlot slot)
        {
            switch (slot)
            {
                case WoWInventorySlot.OffHand:
                {
                    var suitableOffhand = Me.Inventory.Equipped.OffHand != null
                                          && Me.Inventory.Equipped.OffHand.ItemInfo.ItemClass == WoWItemClass.Weapon
                                          && Me.Inventory.Equipped.MainHand != null
                                          && StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment != null;

                    if (!suitableOffhand)
                        CLU.Instance.Log("Please Ensure your weapons are correct for the TalentSpec you are using");

                    return suitableOffhand;
                }

                case WoWInventorySlot.MainHand:
                {
                    var suitableMainhand = Me.Inventory.Equipped.MainHand.ItemInfo.ItemClass == WoWItemClass.Weapon
                                          && Me.Inventory.Equipped.MainHand != null;

                    if (!suitableMainhand)
                        CLU.Instance.Log("Please Ensure your weapons are correct for the TalentSpec you are using");

                    return suitableMainhand;
                }
            }

            return false;
        }

        /// <summary>Runs Lua MacroText</summary>
        /// <param name="macro">the macro text to run</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The run macro text.</returns>
        public Composite RunMacroText(string macro, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                cond,
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [RunMacro] {0} ", label)),
                    new Action(a => Lua.DoString("RunMacroText(\"" + Spell.RealLuaEscape(macro) + "\")"))));
        }

        /// <summary>Attempts to use the bag item provided it is usable and its not on cooldown</summary>
        /// <param name="name">the name of the bag item to use</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The use bag item.</returns>
        public Composite UseBagItem(string name, CanRunDecoratorDelegate cond, string label)
        {
            WoWItem item = null;
            return new Decorator(
                delegate(object a)
                    {
                        if (!cond(a))
                            return false;
                        item = Me.BagItems.FirstOrDefault(x => x.Name == name && x.Usable && x.Cooldown <= 0);
                        return item != null;
                    },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [BagItem] {0} ", label)),
                    new Action(a => item.UseContainerItem())));
        }

        /// <summary>Creates a behavior to use an equipped item.</summary>
        /// <param name="slot">The slot number of the equipped item. </param>
        /// <returns>success or failure</returns>
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
        /// <returns>success or failure</returns>
        public static Composite UseItem(uint id)
        {
            return new PrioritySelector(
                ctx => ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == id),
                new Decorator(
                    ctx => ctx != null && CanUseItem((WoWItem)ctx),
                    new Action(ctx => UseItem((WoWItem)ctx))));
        }

        /// <summary>
        /// Checks the item cooldown is 0 and its usable
        /// </summary>
        /// <param name="item">the item to query</param>
        /// <returns>true of false</returns>
        private static bool CanUseItem(WoWItem item)
        {
            return item != null && item.Usable && item.Cooldown <= 0;
        }

        /// <summary>
        /// Checks the equiped item cooldown is 0 and its usable 
        /// </summary>
        /// <param name="item">the item to query</param>
        /// <returns>true of false</returns>
        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;

            return item.Usable && item.Cooldown <= 0;
        }

        /// <summary>
        /// Uses the item specified (simulated click)
        /// </summary>
        /// <param name="item">The item to use</param>
        private static void UseItem(WoWItem item)
        {
            if (item != null)
            {
                CLU.Instance.Log(" [UseItem] {0} ", item.Name);
                item.Use();
            }
        }

        /// <summary>
        ///  Blows your wad all over the floor
        /// </summary>
        /// <returns>Nothing but win</returns>
        public static Composite UseTrinkets()
        {
            return new PrioritySelector(delegate
            {
                foreach (WoWItem trinket in Trinkets.CurrentTrinkets.Where(trinket => CanUseEquippedItem(trinket) && TrinketUsageSatisfied(trinket) && !HasItemInBag(trinket)))
                {
                    if (trinket != null)
                    {
                        CLU.Instance.Log(" [Trinket] {0} ", trinket.Name);
                        trinket.Use();
                    }
                }

                return RunStatus.Success;
            });
        }

        /// <summary>
        /// Returns true if the trinkets conditions are met
        /// </summary>
        /// <param name="trinket">the trinket to check for</param>
        /// <returns>true if we can use the trinket</returns>
        private static bool TrinketUsageSatisfied(WoWItem trinket)
        {
            if (trinket != null)
            {

                switch (trinket.Name)
                {
                    case "Apparatus of Khaz'goroth":
                        return StyxWoW.Me.ActiveAuras["Titanic Power"].StackCount == 5;
                    case "Fury of Angerforge":
                        return StyxWoW.Me.ActiveAuras["Raw Fury"].StackCount == 5;
                    case "Scales of Life":
                        return Buff.PlayerHasBuff("Weight of a Feather") && Me.HealthPercent <= 90;
                    case "Moonwell Phial":
                        return !Me.CurrentTarget.IsMoving && !Me.IsMoving && Me.HealthPercent <= 90;
                    case "Rotting Skull":
                        if (Me.Class == WoWClass.Warrior) return Buff.PlayerCountBuff("Slaughter") == 3;
                        return true;
                    case "Eye of Unmaking":
                        if (Me.Class == WoWClass.DeathKnight) return Buff.PlayerCountBuff("Titanic Strength") == 10;
                        return true;
                    case "Darkmoon Card: Earthquake":
                        return Me.HealthPercent <= 90;
                    default:
                        return true;
                }
            }
            
            return false;
        }

        /// <summary>Uses the players engineering gloves provided the player has engineering gloves
        /// and the engineering gloves are not on cooldown and the engineering gloves are usable.</summary>
        /// <returns>true or false</returns>
        public static Composite UseEngineerGloves()
        {
            return UseEquippedItem((uint)WoWInventorySlot.Hands);
        }

        /// <summary>Returns true if the player has a mana gem in his bag.</summary>
        /// <returns>The have mana gem.</returns>
        public bool HaveManaGem()
        {
            return ObjectManager.GetObjectsOfType<WoWItem>(false).Any(item => item.Entry == 36799);
        }

        /// <summary>Returns true if the player has the item in his bag.</summary>
        /// <param name="item">The item.</param>
        /// <returns>true of false</returns>
        public static bool HasItemInBag(WoWItem item)
        {
            return item != null && Me.BagItems.Any(x => x.ItemInfo.Id == item.ItemInfo.Id);
        }
    }
}
