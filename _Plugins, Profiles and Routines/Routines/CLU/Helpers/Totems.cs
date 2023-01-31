using System.Collections.Generic;
using System.Linq;

using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace CLU.Helpers
{
    public class Totems
    {
        /* putting all the Totem logic here */

        private static readonly Totems TotemsInstance = new Totems();

        /// <summary>
        /// An instance of the Totems Class
        /// </summary>
        public static Totems Instance { get { return TotemsInstance; } }

        /// <summary>
        ///   Recalls any currently 'out' totems. This will use Totemic Recall if its known, otherwise it will destroy each totem one by one.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        public static void RecallTotems()
        {
            CLU.DebugLog("Recalling totems!");
            if (SpellManager.HasSpell("Totemic Recall"))
            {
                SpellManager.Cast("Totemic Recall");
                return;
            }

            List<WoWTotemInfo> totems = StyxWoW.Me.Totems;
            foreach (WoWTotemInfo t in totems.Where(t => t != null && t.Unit != null))
            {
                DestroyTotem(t.Type);
            }
        }

        /// <summary>
        ///   Destroys the totem described by type.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        /// <param name = "type">The type.</param>
        private static void DestroyTotem(WoWTotemType type)
        {
            if (type == WoWTotemType.None)
            {
                return;
            }

            Lua.DoString("DestroyTotem({0})", (int)type);
        }

        /// <summary>
        /// Determines if we need to recall totems
        /// </summary>
        public static bool NeedToRecallTotems { get { return TotemsInRange == 0 && StyxWoW.Me.Totems.Count(t => t.Unit != null) != 0; } }

        /// <summary>
        /// checks totems in range of us
        /// </summary>
        private static int TotemsInRange { get { return TotemsInRangeOf(StyxWoW.Me); } }

        /// <summary>
        /// Checks totem is in range of a unit
        /// </summary>
        /// <param name="unit">the unit to check for</param>
        /// <returns>count of totems in range</returns>
        private static int TotemsInRangeOf(WoWUnit unit)
        {
            return StyxWoW.Me.Totems.Where(t => t.Unit != null).Count(t => unit.Location.Distance(t.Unit.Location) < GetTotemRange(t.WoWTotem));
        }

        /// <summary>
        ///   Finds the max range of a specific totem, where you'll still receive the buff.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        /// <param name = "totem">The totem.</param>
        /// <returns>The calculated totem range.</returns>
        public static float GetTotemRange(WoWTotem totem)
        {
            // 15% extra range if talented for Totemic Reach for each point
            // float talentFactor = (TalentManager.GetCount(2, 7) * 0.15f) + 1;

            switch (totem)
            {
                case WoWTotem.Flametongue:
                case WoWTotem.Stoneskin:
                case WoWTotem.StrengthOfEarth:
                case WoWTotem.Windfury:
                case WoWTotem.WrathOfAir:
                case WoWTotem.ManaSpring:
                    return 40f;

                case WoWTotem.ElementalResistance:
                case WoWTotem.HealingStream:
                case WoWTotem.TranquilMind:
                case WoWTotem.Tremor:
                    return 30f;

                case WoWTotem.Searing:
                    return 20f;

                case WoWTotem.Earthbind:
                    return 10f;

                case WoWTotem.Grounding:
                case WoWTotem.Magma:
                    return 8f;

                case WoWTotem.Stoneclaw:
                    // stoneclaw isn't effected by Totemic Reach (according to basically everything online)
                    return 8f;

                case WoWTotem.EarthElemental:
                case WoWTotem.FireElemental:
                    // Not really sure about these 3.
                    return 20f;
                case WoWTotem.ManaTide:
                    // Again... not sure :S
                    return 30f;
            }

            return 0f;
        }
    }
}
