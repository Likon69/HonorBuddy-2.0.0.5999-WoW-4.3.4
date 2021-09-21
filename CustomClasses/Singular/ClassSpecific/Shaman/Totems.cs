#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author$
// $Date$
// $HeadURL$
// $LastChangedBy$
// $LastChangedDate$
// $LastChangedRevision$
// $Revision$

#endregion

using System.Collections.Generic;
using System.Linq;

using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using CommonBehaviors.Actions;

namespace Singular.ClassSpecific.Shaman
{
    internal static class Totems
    {
        public static Composite CreateSetTotems()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => !SpellManager.HasSpell("Call of the Elements"),
                    new PrioritySelector(
                        new PrioritySelector(
                            ctx => StyxWoW.Me.Totems.FirstOrDefault(t => t.WoWTotem == GetEarthTotem()),
                            new Decorator(
                                ret => GetEarthTotem() != WoWTotem.None && (ret == null || ((WoWTotemInfo) ret).Unit == null ||
                                       ((WoWTotemInfo) ret).Unit.Distance > GetTotemRange(GetEarthTotem())),
                                new Sequence(
                                    new Action(ret => Logger.Write("Casting {0} Totem", GetEarthTotem().ToString().CamelToSpaced())),
                                    new Action(ret => SpellManager.Cast(GetEarthTotem().GetTotemSpellId()))))),
                        new PrioritySelector(
                            ctx => StyxWoW.Me.Totems.FirstOrDefault(t => t.WoWTotem == GetAirTotem()),
                            new Decorator(
                                ret => GetAirTotem() != WoWTotem.None && (ret == null || ((WoWTotemInfo) ret).Unit == null ||
                                       ((WoWTotemInfo)ret).Unit.Distance > GetTotemRange(GetAirTotem())),
                                new Sequence(
                                    new Action(ret => Logger.Write("Casting {0} Totem", GetAirTotem().ToString().CamelToSpaced())),
                                    new Action(ret => SpellManager.Cast(GetAirTotem().GetTotemSpellId()))))),
                        new PrioritySelector(
                            ctx => StyxWoW.Me.Totems.FirstOrDefault(t => t.WoWTotem == GetWaterTotem()),
                            new Decorator(
                                ret => GetWaterTotem() != WoWTotem.None && (ret == null || ((WoWTotemInfo)ret).Unit == null ||
                                       ((WoWTotemInfo) ret).Unit.Distance > GetTotemRange(GetWaterTotem())),
                                new Sequence(
                                    new Action(ret => Logger.Write("Casting {0} Totem", GetWaterTotem().ToString().CamelToSpaced())),
                                    new Action(ret => SpellManager.Cast(GetWaterTotem().GetTotemSpellId())))))
                        )),
                new Decorator(
                    ret =>
                        {
                            // Hell yeah this is long, but its all clear to read
                            if (!SpellManager.HasSpell("Call of the Elements"))
                                return false;

                            var bestAirTotem = GetAirTotem();
                            var currentAirTotem = StyxWoW.Me.Totems.FirstOrDefault(t => t.WoWTotem == bestAirTotem);

                            if (currentAirTotem == null)
                            {
                                return true;
                            }

                            var airTotemAsUnit = currentAirTotem.Unit;

                            if (airTotemAsUnit == null)
                            {
                                return true;
                            }

                            if (airTotemAsUnit.Distance > GetTotemRange(bestAirTotem))
                            {
                                return true;
                            }

                            var bestEarthTotem = GetEarthTotem();
                            var currentEarthTotem = StyxWoW.Me.Totems.FirstOrDefault(t => t.WoWTotem == bestEarthTotem);

                            if (currentEarthTotem == null)
                            {
                                return true;
                            }

                            var earthTotemAsUnit = currentEarthTotem.Unit;

                            if (earthTotemAsUnit == null)
                            {
                                return true;
                            }

                            if (earthTotemAsUnit.Distance > GetTotemRange(bestEarthTotem))
                            {
                                return true;
                            }

                            var bestWaterTotem = GetWaterTotem();
                            var currentWaterTotem = StyxWoW.Me.Totems.FirstOrDefault(t => t.WoWTotem == bestWaterTotem);

                            if (currentWaterTotem == null)
                            {
                                return true;
                            }

                            var waterTotemAsUnit = currentWaterTotem.Unit;

                            if (waterTotemAsUnit == null)
                            {
                                return true;
                            }

                            if (waterTotemAsUnit.Distance > GetTotemRange(bestWaterTotem))
                            {
                                return true;
                            }

                            return false;
                        },
                    new Sequence(
                        new Action(ret => SetupTotemBar()),
                        Spell.BuffSelf("Call of the Elements")))
                            
                );
        }

        public static void SetupTotemBar()
        {
            // If the user has given specific totems to use, then use them. Otherwise, fall back to our automagical ones
            WoWTotem earth = SingularSettings.Instance.Shaman.EarthTotem;
            WoWTotem air = SingularSettings.Instance.Shaman.AirTotem;
            WoWTotem water = SingularSettings.Instance.Shaman.WaterTotem;

            SetTotemBarSlot(MultiCastSlot.ElementsEarth, earth != WoWTotem.None ? earth : GetEarthTotem());
            SetTotemBarSlot(MultiCastSlot.ElementsAir, air != WoWTotem.None ? air : GetAirTotem());
            SetTotemBarSlot(MultiCastSlot.ElementsWater, water != WoWTotem.None ? water : GetWaterTotem());
            SetTotemBarSlot(MultiCastSlot.ElementsFire, WoWTotem.None);
        }

        /// <summary>
        ///   Recalls any currently 'out' totems. This will use Totemic Recall if its known, otherwise it will destroy each totem one by one.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        public static void RecallTotems()
        {
            Logger.Write("Recalling totems!");
            if (SpellManager.HasSpell("Totemic Recall"))
            {
                SpellManager.Cast("Totemic Recall");
                return;
            }

            List<WoWTotemInfo> totems = StyxWoW.Me.Totems;
            foreach (WoWTotemInfo t in totems)
            {
                if (t != null && t.Unit != null)
                {
                    DestroyTotem(t.Type);
                }
            }
        }

        /// <summary>
        ///   Destroys the totem described by type.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        /// <param name = "type">The type.</param>
        public static void DestroyTotem(WoWTotemType type)
        {
            if (type == WoWTotemType.None)
            {
                return;
            }

            Lua.DoString("DestroyTotem({0})", (int)type);
        }

        /// <summary>
        ///   Sets a totem bar slot to the specified totem!.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        /// <param name = "slot">The slot.</param>
        /// <param name = "totem">The totem.</param>
        public static void SetTotemBarSlot(MultiCastSlot slot, WoWTotem totem)
        {
            // Make sure we have the totem bars to set. Highest first kthx
            if (slot >= MultiCastSlot.SpiritsFire && !SpellManager.HasSpell("Call of the Spirits"))
            {
                return;
            }
            if (slot >= MultiCastSlot.AncestorsFire && !SpellManager.HasSpell("Call of the Ancestors"))
            {
                return;
            }
            if (!SpellManager.HasSpell("Call of the Elements"))
            {
                return;
            }

            if (LastSetTotems.ContainsKey(slot) && LastSetTotems[slot] == totem)
            {
                return;
            }

            if (!LastSetTotems.ContainsKey(slot))
            {
                LastSetTotems.Add(slot, totem);
            }
            else
            {
                LastSetTotems[slot] = totem;
            }

            Logger.Write("Setting totem slot Call of the" + slot.ToString().CamelToSpaced() + " to " + totem.ToString().CamelToSpaced());

            Lua.DoString("SetMultiCastSpell({0}, {1})", (int)slot, totem.GetTotemSpellId());
        }

        private static readonly Dictionary<MultiCastSlot, WoWTotem> LastSetTotems = new Dictionary<MultiCastSlot, WoWTotem>();

        private static WoWTotem GetEarthTotem()
        {
            LocalPlayer me = StyxWoW.Me;
            bool isEnhance = TalentManager.CurrentSpec == TalentSpec.EnhancementShaman;
            // Solo play
            if (!me.IsInParty && !me.IsInRaid)
            {
                // Enhance, lowbie
                if (isEnhance || TalentManager.CurrentSpec == TalentSpec.Lowbie)
                {
                    if (TotemIsKnown(WoWTotem.StrengthOfEarth))
                    {
                        return WoWTotem.StrengthOfEarth;
                    }

                    return WoWTotem.None;
                }

                // Ele, resto
                if (TotemIsKnown(WoWTotem.Stoneskin))
                {
                    return WoWTotem.Stoneskin;
                }

                return WoWTotem.None;
            }

            // Raids and stuff

            // Enhance
            if (isEnhance)
            {
                if (TotemIsKnown(WoWTotem.StrengthOfEarth))
                {
                    return WoWTotem.StrengthOfEarth;
                }

                return WoWTotem.None;
            }

            if (TotemIsKnown(WoWTotem.Stoneskin))
            {
                return WoWTotem.Stoneskin;
            }

            if (TotemIsKnown(WoWTotem.StrengthOfEarth))
            {
                return WoWTotem.StrengthOfEarth;
            }

            return WoWTotem.None;
        }

        public static WoWTotem GetAirTotem()
        {
            if (TalentManager.CurrentSpec == TalentSpec.Lowbie)
            {
                return WoWTotem.None;
            }

            LocalPlayer me = StyxWoW.Me;
            bool isEnhance = TalentManager.CurrentSpec == TalentSpec.EnhancementShaman;
            
            if (!me.IsInParty && !me.IsInRaid)
            {
                if (isEnhance)
                {
                    if (TotemIsKnown(WoWTotem.Windfury))
                    {
                        return WoWTotem.Windfury;
                    }

                    return WoWTotem.None;
                }

                if (TotemIsKnown(WoWTotem.WrathOfAir))
                {
                    return WoWTotem.WrathOfAir;
                }

                return WoWTotem.None;
            }

            if (StyxWoW.Me.RaidMembers.Any(p => p.Class == WoWClass.Druid && p.Shapeshift == ShapeshiftForm.Moonkin) ||
                StyxWoW.Me.PartyMembers.Any(p => p.Class == WoWClass.Druid && p.Shapeshift == ShapeshiftForm.Moonkin))
            {
                if (TotemIsKnown(WoWTotem.Windfury))
                {
                    return WoWTotem.Windfury;
                }
            }

            if (!isEnhance && TotemIsKnown(WoWTotem.WrathOfAir))
            {
                return WoWTotem.WrathOfAir;
            }

            if (TotemIsKnown(WoWTotem.Windfury))
            {
                return WoWTotem.Windfury;
            }

            return WoWTotem.None;
        }
        
        public static WoWTotem GetWaterTotem()
        {
            // Plain and simple. If we're resto, we never want a different water totem out. Thats all there is to it.
            if (TalentManager.CurrentSpec == TalentSpec.RestorationShaman)
            {
                if (TotemIsKnown(WoWTotem.HealingStream))
                {
                    return WoWTotem.HealingStream;
                }

                return WoWTotem.None;
            }

            // Solo play. Only healing stream
            if (!StyxWoW.Me.IsInParty && !StyxWoW.Me.IsInRaid)
            {
                if (TotemIsKnown(WoWTotem.HealingStream))
                {
                    return WoWTotem.HealingStream;
                }

                return WoWTotem.None;
            }

            // Really only drop this if we don't have the pally buff, and we're not resto.
            if (!StyxWoW.Me.HasAura("Blessing of Might") && TotemIsKnown(WoWTotem.ManaSpring))
            {
                return WoWTotem.ManaSpring;
            }

            // ... yea
            if (TotemIsKnown(WoWTotem.HealingStream))
            {
                return WoWTotem.HealingStream;
            }

            return WoWTotem.None;
        }

        #region Helper shit

        public static bool NeedToRecallTotems { get { return TotemsInRange == 0 && StyxWoW.Me.Totems.Count(t => t.Unit != null) != 0; } }
        public static int TotemsInRange { get { return TotemsInRangeOf(StyxWoW.Me); } }

        public static int TotemsInRangeOf(WoWUnit unit)
        {
            return StyxWoW.Me.Totems.Where(t => t.Unit != null).Count(t => unit.Location.Distance(t.Unit.Location) < GetTotemRange(t.WoWTotem));
        }

        public static bool TotemIsKnown(WoWTotem totem)
        {
            return SpellManager.HasSpell(totem.GetTotemSpellId());
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
            float talentFactor = (TalentManager.GetCount(2, 7) * 0.15f) + 1;

            switch (totem)
            {
                case WoWTotem.Flametongue:
                case WoWTotem.Stoneskin:
                case WoWTotem.StrengthOfEarth:
                case WoWTotem.Windfury:
                case WoWTotem.WrathOfAir:
                case WoWTotem.ManaSpring:
                    return 40f * talentFactor;

                case WoWTotem.ElementalResistance:
                case WoWTotem.HealingStream:
                case WoWTotem.TranquilMind:
                case WoWTotem.Tremor:
                    return 30f * talentFactor;

                case WoWTotem.Searing:
                    return 20f * talentFactor;

                case WoWTotem.Earthbind:
                    return 10f * talentFactor;

                case WoWTotem.Grounding:
                case WoWTotem.Magma:
                    return 8f * talentFactor;

                case WoWTotem.Stoneclaw:
                    // stoneclaw isn't effected by Totemic Reach (according to basically everything online)
                    return 8f;

                case WoWTotem.EarthElemental:
                case WoWTotem.FireElemental:
                    // Not really sure about these 3.
                    return 20f;
                case WoWTotem.ManaTide:
                    // Again... not sure :S
                    return 30f * talentFactor;
            }
            return 0f;
        }

        #endregion

        #region Nested type: MultiCastSlot

        /// <summary>
        ///   A small enum to make specifying specific totem bar slots easier.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        internal enum MultiCastSlot
        {
            // Enums increment by 1 after the first defined value. So don't touch this. Its the way it is for a reason.
            // If these numbers change in the future, feel free to fill this out completely. I'm too lazy to do it - Apoc
            //
            // Note: To get the totem 'slot' just do MultiCastSlot & 3 - will return 0-3 for the totem slot this is for.
            // I'm not entirely sure how WoW shows which ones are 'current' in the slot, so we'll just set it up for ourselves
            // and remember which is which.
            ElementsFire = 133,
            ElementsEarth,
            ElementsWater,
            ElementsAir,

            AncestorsFire,
            AncestorsEarth,
            AncestorsWater,
            AncestorsAir,

            SpiritsFire,
            SpiritsEarth,
            SpiritsWater,
            SpiritsAir
        }

        #endregion
    }
}