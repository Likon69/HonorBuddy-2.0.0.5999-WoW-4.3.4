using Styx;

// Types shared between Composites

namespace HighVoltz.Composites
{
    public enum SubCategoryType
    {
        None
    };

    // use as a placeholder for item categories with no sub categories defined in HB
    public enum BankType
    {
        Personal,
        Guild
    }

    public enum DepositWithdrawAmount
    {
        All,
        Amount
    }

    internal static class Callbacks
    {
        /// <summary>
        /// Returns the SubCategories enum that goes with the Category enum 'ItemClass'. 
        /// </summary>
        /// <param name="itemClass"></param>
        /// <returns></returns>
        public static object GetSubCategory(WoWItemClass itemClass)
        {
            switch (itemClass)
            {
                case WoWItemClass.Armor:
                    return WoWItemArmorClass.None;
                case WoWItemClass.Container:
                case WoWItemClass.Money:
                case WoWItemClass.Projectile:
                case WoWItemClass.Quest:
                case WoWItemClass.Quiver:
                case WoWItemClass.Reagent:
                    return SubCategoryType.None;
                case WoWItemClass.Consumable:
                    return WoWItemContainerClass.None;
                case WoWItemClass.Gem:
                    return WoWItemGemClass.None;
                case WoWItemClass.Glyph:
                    return WoWItemGlyphClass.None;
                case WoWItemClass.Key:
                    return WoWItemKeyClass.None;
                case WoWItemClass.Miscellaneous:
                    return WoWItemMiscClass.None;
                case WoWItemClass.Recipe:
                    return WoWItemRecipeClass.None;
                case WoWItemClass.TradeGoods:
                    return WoWItemTradeGoodsClass.None;
                case WoWItemClass.Weapon:
                    return WoWItemWeaponClass.None;
            }
            return SubCategoryType.None;
        }
    }

    internal struct AuctionEntry
    {
        public uint Bid;
        public uint Buyout;
        public uint Id;
        public uint LowestBo;
        public uint MyAuctions;
        public string Name;

        public AuctionEntry(string name, uint id, uint buyout, uint bid)
        {
            Name = name;
            Id = id;
            Buyout = buyout;
            Bid = bid;
            LowestBo = uint.MaxValue;
            MyAuctions = 0;
        }

        public override string ToString()
        {
            return string.Format("Name:{0} Buyout:{1} Competitor:{2}",
                                 Name, GoldString(Buyout), GoldString(LowestBo));
        }

        public static string GoldString(uint copper)
        {
            uint gold = copper/10000;
            copper %= 10000;
            uint silver = copper/100;
            copper %= 100;
            return string.Format("{0}g{1}s{2}c", gold, silver, copper);
        }
    }
}