using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace MrFishIt
{
    internal static class Fishing
    {
        static readonly List<int> FishingIds = new List<int> { 7620, 7731, 7732, 18248, 33095, 51294, 88868 };

        /// <summary>
        /// Returns true if you are fishing
        /// </summary>
        public static bool IsFishing { get { return FishingIds.Contains(ObjectManager.Me.ChanneledCastingSpellId); } }

        /// <summary>
        /// Returns your fishing pole
        /// </summary>
        public static WoWItem FishingPole { get { return ObjectManager.GetObjectsOfType<WoWItem>().Where(b => b.IsFishingPole()).FirstOrDefault(); } }

        /// <summary>
        /// Returns true if you have a temp-enchantm on your pole
        /// </summary>
        public static bool GotLure { get { return Lua.GetReturnVal<bool>("return GetWeaponEnchantInfo()", 0); } }

        /// <summary>
        /// Returns true if the fishing bobber is bobbing
        /// </summary>
        public static bool IsBobberBobbing { get { return FishingBobber != null && FishingBobber.IsBobbing(); } }

        /// <summary>
        /// Returns the current fishing bobber in use, null otherwise
        /// </summary>
        public static WoWGameObject FishingBobber
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                    obj =>
                    obj.SubType == WoWGameObjectType.FishingBobber
                    && obj.CreatedByGuid == ObjectManager.Me.Guid).FirstOrDefault());
            }
        }
    }

    internal static class Extensions
    {
        static readonly List<uint> PoleIds = new List<uint> { 44050, 19970, 45991, 45992, 45858, 19022, 25978, 6367, 12225, 6366, 6256, 6365 };

        public static bool IsFishingPole(this WoWItem value)
        {
            if (value == null)
                return false;

            return PoleIds.Contains(value.Entry);
        }

        public static bool IsBobbing(this WoWGameObject value)
        {
            if (value == null || value.SubType != WoWGameObjectType.FishingBobber)
                return false;

            return ((WoWFishingBobber)value.SubObj).IsBobbing;
        }
    }
}
