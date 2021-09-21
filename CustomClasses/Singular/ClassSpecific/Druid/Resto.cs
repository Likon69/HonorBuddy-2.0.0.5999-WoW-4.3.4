using System;
using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Singular.ClassSpecific.Druid
{
    public class Resto
    {
        [Class(WoWClass.Druid)]
        [Spec(TalentSpec.RestorationDruid)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoDruidHealRest()
        {
            return new PrioritySelector(
                Spell.WaitForCast(),
                // Heal self before resting. There is no need to eat while we have 100% mana
                CreateRestoDruidHealOnlyBehavior(true),
                // Rest up damnit! Do this first, so we make sure we're fully rested.
                Rest.CreateDefaultRestBehaviour(),
                // Can we res people?
                Spell.Resurrect("Revive"),
                // Make sure we're healing OOC too!
                CreateRestoDruidHealOnlyBehavior(false,false));
        }

        public static Composite CreateRestoDruidHealOnlyBehavior()
        {
            return CreateRestoDruidHealOnlyBehavior(false, true);
        }

        public static Composite CreateRestoDruidHealOnlyBehavior(bool selfOnly)
        {
            return CreateRestoDruidHealOnlyBehavior(selfOnly, false);
        }

        public static Composite CreateRestoDruidHealOnlyBehavior(bool selfOnly, bool moveInRange)
        {
            HealerManager.NeedHealTargeting = true;
            const uint mapleSeedId = 17034;

            return new
                PrioritySelector(
                ctx => selfOnly ? StyxWoW.Me : HealerManager.Instance.FirstUnit,
                    new Decorator(
                        ret => ret != null && (moveInRange || ((WoWUnit)ret).InLineOfSpellSight && ((WoWUnit)ret).DistanceSqr < 40 * 40),
                        new PrioritySelector(
                        Spell.WaitForCast(false),
                        new Decorator(
                            ret => moveInRange,
                            Movement.CreateMoveToLosBehavior(ret => (WoWUnit)ret)),
                        // Ensure we're in range of the unit to heal, and it's in LOS.
                        //CreateMoveToAndFace(35f, ret => (WoWUnit)ret),
                        //Cast Lifebloom on tank if
                        //1- Tank doesn't have lifebloom
                        //2- Tank has less then 3 stacks of lifebloom
                        //3- Tank has 3 stacks of lifebloom but it will expire in 3 seconds
                        Spell.Heal(
                            "Lifebloom",
                            ret => (WoWUnit)ret,
                            ret =>
                            StyxWoW.Me.Combat &&
                                // Keep 3 stacks up on the tank/leader at all times.
                                // If we're in ToL form, we can do rolling LBs for everyone. So ignore the fact that its the leader or not.
                                // LB is cheap, and VERY powerful in ToL form since you can spam it on the entire raid, for a cheap HoT and quite good 'bloom'
                            ((RaFHelper.Leader != null && (WoWUnit)ret == RaFHelper.Leader) || StyxWoW.Me.Shapeshift == ShapeshiftForm.TreeOfLife) &&
                            ((WoWUnit)ret).HealthPercent > 60 &&
                            (!((WoWUnit)ret).HasAura("Lifebloom") || ((WoWUnit)ret).Auras["Lifebloom"].StackCount < 3 ||
                             ((WoWUnit)ret).Auras["Lifebloom"].TimeLeft <= TimeSpan.FromSeconds(3))),
                        //Cast rebirth if the tank is dead. Check for Unburdened Rebirth glyph or Maple seed reagent
                        Spell.Heal(
                            "Rebirth",
                            ret => (WoWUnit)ret,
                            ret => StyxWoW.Me.Combat && RaFHelper.Leader != null && (WoWUnit)ret == RaFHelper.Leader &&
                                   ((WoWUnit)ret).Dead && (TalentManager.HasGlyph("Unburdened Rebirth") || StyxWoW.Me.BagItems.Any(i => i.Entry == mapleSeedId))),
                        Spell.Heal(
                            "Tranquility",
                            ret => StyxWoW.Me.Combat && StyxWoW.Me.IsInParty && Unit.NearbyFriendlyPlayers.Count(
                                p =>
                                p.IsAlive && p.HealthPercent <= SingularSettings.Instance.Druid.TranquilityHealth && p.Distance <= 30) >=
                                   SingularSettings.Instance.Druid.TranquilityCount),
                        //Use Innervate on party members if we have Glyph of Innervate
                        Spell.Buff(
                            "Innervate",
                            ret => (WoWUnit)ret,
                            ret =>
                            TalentManager.HasGlyph("Innervate") && StyxWoW.Me.Combat && (WoWUnit)ret != StyxWoW.Me &&
                            StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Druid.InnervateMana &&
                            ((WoWUnit)ret).PowerType == WoWPowerType.Mana && ((WoWUnit)ret).ManaPercent <= SingularSettings.Instance.Druid.InnervateMana),
                        Spell.Heal(
                            "Swiftmend",
                            ret => (WoWUnit)ret,
                            ret => StyxWoW.Me.Combat && ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Druid.Swiftmend &&
                                   (((WoWUnit)ret).HasAura("Rejuvenation") || ((WoWUnit)ret).HasAura("Regrowth"))),
                        Spell.Heal(
                            "Wild Growth",
                            ret => (WoWUnit)ret,
                            ret => StyxWoW.Me.IsInParty && Unit.NearbyFriendlyPlayers.Count(
                                p => p.IsAlive && p.HealthPercent <= SingularSettings.Instance.Druid.WildGrowthHealth &&
                                     p.Location.DistanceSqr(((WoWUnit)ret).Location) <= 30*30) >= SingularSettings.Instance.Druid.WildGrowthCount),
                        Spell.Heal(
                            "Regrowth",
                            ret => (WoWUnit)ret,
                            ret => !((WoWUnit)ret).HasMyAura("Regrowth") && ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Druid.Regrowth),
                        Spell.Heal(
                            "Healing Touch",
                            ret => (WoWUnit)ret,
                            ret => ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Druid.HealingTouch),
                        Spell.Heal(
                            "Nourish",
                            ret => (WoWUnit)ret,
                            ret => ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Druid.Nourish &&
                                   ((((WoWUnit)ret).HasAura("Rejuvenation") || ((WoWUnit)ret).HasAura("Regrowth") ||
                                    ((WoWUnit)ret).HasAura("Lifebloom") || ((WoWUnit)ret).HasAura("Wild Growth")))),
                        Spell.Heal(
                            "Rejuvenation",
                            ret => (WoWUnit)ret,
                            ret => !((WoWUnit)ret).HasMyAura("Rejuvenation") &&
                                   ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Druid.Rejuvenation),
                        new Decorator(
                            ret => StyxWoW.Me.Combat && StyxWoW.Me.GotTarget && Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                            new PrioritySelector(
                                Movement.CreateMoveToLosBehavior(),
                                Movement.CreateFaceTargetBehavior(),
                                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                                Spell.Buff("Moonfire"),
                                Spell.Cast("Starfire", ret => StyxWoW.Me.HasAura("Fury of Stormrage")),
                                Spell.Cast("Wrath"),
                                Movement.CreateMoveToTargetBehavior(true, 35f)
                                )),
                        new Decorator(
                            ret => moveInRange,
                            Movement.CreateMoveToTargetBehavior(true, 35f, ret => (WoWUnit)ret))
                        )));
        }

        [Class(WoWClass.Druid)]
        [Spec(TalentSpec.RestorationDruid)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoDruidHealBehavior()
        {
            return
                new PrioritySelector(
                    CreateRestoDruidHealOnlyBehavior());
        }

        [Class(WoWClass.Druid)]
        [Spec(TalentSpec.RestorationDruid)]
        [Behavior(BehaviorType.Combat | BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoDruidCombat()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                        new PrioritySelector(
                            Safers.EnsureTarget(),
                            Movement.CreateMoveToLosBehavior(),
                            Movement.CreateFaceTargetBehavior(),
                            Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                            Spell.Buff("Moonfire"),
                            Spell.Cast("Starfire", ret => StyxWoW.Me.HasAura("Fury of Stormrage")),
                            Spell.Cast("Wrath"),
                            Movement.CreateMoveToTargetBehavior(true, 35f)
                            ))
                    );
        }

        [Class(WoWClass.Druid)]
        [Spec(TalentSpec.RestorationDruid)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateRestoDruidCombatBuffs()
        {
            return
                new PrioritySelector(
                    Spell.BuffSelf(
                        "Tree of Life",
                        ret => StyxWoW.Me.IsInParty && Unit.NearbyFriendlyPlayers.Count(
                            p => p.IsAlive && p.HealthPercent <= SingularSettings.Instance.Druid.TreeOfLifeHealth) >=
                               SingularSettings.Instance.Druid.TreeOfLifeCount),
                    Spell.BuffSelf(
                        "Innervate",
                        ret => StyxWoW.Me.ManaPercent < 15 || StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Druid.InnervateMana),
                    Spell.BuffSelf(
                        "Barkskin",
                        ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Druid.Barkskin)
                    );
        }
    }
}
