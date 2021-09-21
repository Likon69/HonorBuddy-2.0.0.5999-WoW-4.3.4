using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace Singular.ClassSpecific.Shaman
{
    class Restoration
    {
        private const int RESTO_T12_ITEM_SET_ID = 1014;
        private const int ELE_T12_ITEM_SET_ID = 1016;

        private static int NumTier12Pieces
        {
            get
            {
                int count = StyxWoW.Me.Inventory.Equipped.Hands.ItemInfo.ItemSetId == RESTO_T12_ITEM_SET_ID ? 1 : 0;
                count += StyxWoW.Me.Inventory.Equipped.Legs.ItemInfo.ItemSetId == RESTO_T12_ITEM_SET_ID ? 1 : 0;
                count += StyxWoW.Me.Inventory.Equipped.Chest.ItemInfo.ItemSetId == RESTO_T12_ITEM_SET_ID ? 1 : 0;
                count += StyxWoW.Me.Inventory.Equipped.Shoulder.ItemInfo.ItemSetId == RESTO_T12_ITEM_SET_ID ? 1 : 0;
                count += StyxWoW.Me.Inventory.Equipped.Head.ItemInfo.ItemSetId == RESTO_T12_ITEM_SET_ID ? 1 : 0;
                return count;
            }
        }


        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.RestorationShaman)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoShamanHealingBuffs()
        {
            return new PrioritySelector(
                // Keep WS up at all times. Period.
                Spell.BuffSelf("Water Shield"),

                new Decorator(
                    ret => StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 3345 && StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole,  
                    Spell.Cast("Earthliving Weapon"))
                );
        }


        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.RestorationShaman)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoShamanRest()
        {
            return new PrioritySelector(
                CreateRestoShamanHealingBuffs(),
                CreateRestoShamanHealingOnlyBehavior(true),
                Rest.CreateDefaultRestBehaviour(),
                Spell.Resurrect("Ancestral Spirit"),
                CreateRestoShamanHealingOnlyBehavior(false,false)
                );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.RestorationShaman)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoShamanCombatBehavior()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                        new PrioritySelector(
                            Safers.EnsureTarget(),
                            Movement.CreateMoveToLosBehavior(),
                            Movement.CreateFaceTargetBehavior(),
                            Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                            Spell.BuffSelf("Fire Elemental Totem",
                                ret => (StyxWoW.Me.CurrentTarget.Elite || Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 3) &&
                                       !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                            Spell.BuffSelf("Searing Totem",
                                ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                                        !StyxWoW.Me.Totems.Any(
                                            t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                                    t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                                        !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                            Spell.Cast("Earth Shock"),
                            Spell.Cast("Lightning Bolt"),
                            Movement.CreateMoveToTargetBehavior(true, 35f)
                            ))
                    );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.RestorationShaman)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoShamanHealBehavior()
        {
            return
                new PrioritySelector(
                    CreateRestoShamanHealingOnlyBehavior());
        }

        public static Composite CreateRestoShamanHealingOnlyBehavior()
        {
            return CreateRestoShamanHealingOnlyBehavior(false, false);
        }

        public static Composite CreateRestoShamanHealingOnlyBehavior(bool selfOnly)
        {
            return CreateRestoShamanHealingOnlyBehavior(selfOnly, false);
        }

        public static Composite CreateRestoShamanHealingOnlyBehavior(bool selfOnly, bool moveInRange)
        {
            HealerManager.NeedHealTargeting = true;
            return new PrioritySelector(
                ctx => selfOnly ? StyxWoW.Me : HealerManager.Instance.FirstUnit,
                new Decorator(
                    ret => ret != null && (moveInRange || ((WoWUnit)ret).InLineOfSpellSight && ((WoWUnit)ret).DistanceSqr < 40 * 40),
                    new PrioritySelector(
                        Spell.WaitForCast(),
                        new Decorator(
                            ret => moveInRange,
                            Movement.CreateMoveToLosBehavior(ret => (WoWUnit)ret)),
                        Totems.CreateSetTotems(),
                        // Mana tide...
                        Spell.Cast("Mana Tide Totem", ret => StyxWoW.Me.ManaPercent < 80),
                        // Grounding...
                        Spell.Cast("Grounding Totem", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.Distance < 40 && u.IsTargetingMeOrPet && u.IsCasting)),

                        // Just pop RT on CD. Plain and simple. Calling GetBestRiptideTarget will see if we can spread RTs (T12 2pc)
                        Spell.Heal("Riptide", ret => GetBestRiptideTarget((WoWPlayer)ret)),
                        // And deal with some edge PVP cases.

                        Spell.Heal("Earth Shield", 
                            ret => (WoWUnit)ret, 
                            ret => ret is WoWPlayer && Group.Tanks.Contains((WoWPlayer)ret) && Group.Tanks.All(t => !t.HasMyAura("Earth Shield"))),

                        // Pop NS if someone is in some trouble.
                        Spell.BuffSelf("Nature's Swiftness", ret => ((WoWUnit)ret).HealthPercent < 15),
                        Spell.Heal("Unleash Elements", ret => (WoWUnit)ret, ret => ((WoWUnit)ret).HealthPercent < 40),
                        // GHW is highest priority. It should be fairly low health %. (High-end healers will have this set to 70ish
                        Spell.Heal("Greater Healing Wave", ret => (WoWUnit)ret, ret => ((WoWUnit)ret).HealthPercent < 50),
                        // Most (if not all) will leave this at 90. Its lower prio, high HPM, low HPS
                        Spell.Heal("Healing Wave", ret => (WoWUnit)ret, ret => ((WoWUnit)ret).HealthPercent < 60),


                        // CH/HR only in parties/raids
                        new Decorator(
                            ret => StyxWoW.Me.IsInParty || StyxWoW.Me.IsInRaid,
                            new PrioritySelector(
                                // This seems a bit tricky, but its really not. This is just how we cache a somewhat expensive lookup.
                                // Set the context to the "best unit" for the cluster, so we don't have to do that check twice.
                                // Then just use the context when passing the unit to throw the heal on, and the target of the heal from the cluster count.
                                // Also ensure it will jump at least 3 times. (CH is pointless to cast if it won't jump 3 times!)
                                new PrioritySelector(
                                    context => Clusters.GetBestUnitForCluster(ChainHealPlayers, ClusterType.Chained, 12f),
                                    Spell.Heal(
                                        "Chain Heal", ret => (WoWPlayer)ret,
                                        ret => Clusters.GetClusterCount((WoWPlayer)ret, ChainHealPlayers, ClusterType.Chained, 12f) > 2)),

                                // Now we're going to do the same thing as above, but this time we're going to do it with healing rain.
                                new PrioritySelector(
                                    context => Clusters.GetBestUnitForCluster(Unit.NearbyFriendlyPlayers.Cast<WoWUnit>(), ClusterType.Radius, 10f),
                                    Spell.CastOnGround(
                                        "Healing Rain", ret => ((WoWPlayer)ret).Location,
                                        ret =>
                                        Clusters.GetClusterCount((WoWPlayer)ret, Unit.NearbyFriendlyPlayers.Cast<WoWUnit>(), ClusterType.Radius, 10f) >
                                        // If we're in a raid, check for 4 players. If we're just in a party, check for 3.
                                        (StyxWoW.Me.IsInRaid ? 3 : 2))))),
                        new Decorator(
                            ret => StyxWoW.Me.Combat && StyxWoW.Me.GotTarget && Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                            new PrioritySelector(
                                Movement.CreateMoveToLosBehavior(),
                                Movement.CreateFaceTargetBehavior(),
                                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                                Spell.BuffSelf("Fire Elemental Totem",
                                    ret => (StyxWoW.Me.CurrentTarget.Elite || Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 3) &&
                                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                                Spell.BuffSelf("Searing Totem",
                                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                                           !StyxWoW.Me.Totems.Any(
                                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                                Spell.Cast("Earth Shock"),
                                Spell.Cast("Lightning Bolt"),
                                Movement.CreateMoveToTargetBehavior(true, 35f)
                                )),
                        new Decorator(
                            ret => moveInRange,
                            Movement.CreateMoveToTargetBehavior(true, 38f, ret => (WoWUnit)ret))

                )));
        }

        private static IEnumerable<WoWUnit> ChainHealPlayers
        {
            get
            {
                // TODO: Decide if we want to do this differently to ensure we take into account the T12 4pc bonus. (Not removing RT when using CH)
                return Unit.NearbyFriendlyPlayers.Where(u => u.HealthPercent < 90).Select(u => (WoWUnit)u);
            }
        }

        private static WoWPlayer GetBestRiptideTarget(WoWPlayer originalTarget)
        {
            if (!originalTarget.HasMyAura("Riptide"))
                return originalTarget;

            // Target already has RT. So lets find someone else to throw it on. Lowest health first preferably.
            return Unit.NearbyFriendlyPlayers.OrderBy(u => u.HealthPercent).Where(u => !u.HasMyAura("Riptide")).FirstOrDefault();
        }
    }
}
