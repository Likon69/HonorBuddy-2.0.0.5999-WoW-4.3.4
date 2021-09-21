using System;
using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Singular.ClassSpecific.Priest
{
    public class Holy
    {
        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.HolyPriest)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyHealRest()
        {
            return new PrioritySelector(
                Spell.WaitForCast(),
                // Heal self before resting. There is no need to eat while we have 100% mana
                CreateHolyHealOnlyBehavior(true),
                // Rest up damnit! Do this first, so we make sure we're fully rested.
                Rest.CreateDefaultRestBehaviour(),
                // Can we res people?
                Spell.Resurrect("Resurrection"),
                // Make sure we're healing OOC too!
                CreateHolyHealOnlyBehavior(false, false)
                );
        }

        public static Composite CreateHolyHealOnlyBehavior()
        {
            return CreateHolyHealOnlyBehavior(false, true);
        }

        public static Composite CreateHolyHealOnlyBehavior(bool selfOnly)
        {
            return CreateHolyHealOnlyBehavior(selfOnly, false);
        }

        public static Composite CreateHolyHealOnlyBehavior(bool selfOnly, bool moveInRange)
        {
            // Atonement - Tab 1  index 10 - 1/2 pts
            HealerManager.NeedHealTargeting = true;
            return new
                PrioritySelector(
                ret => selfOnly ? StyxWoW.Me : HealerManager.Instance.FirstUnit,
                    new Decorator(
                        ret => ret != null && (moveInRange || ((WoWUnit)ret).InLineOfSpellSight && ((WoWUnit)ret).DistanceSqr < 40 * 40),
                        new PrioritySelector(
                        Spell.WaitForCast(),
                        new Decorator(
                            ret => moveInRange,
                            Movement.CreateMoveToLosBehavior(ret => (WoWUnit)ret)),
                        // use fade to drop aggro.
                        Spell.Cast("Fade", ret => (StyxWoW.Me.IsInParty || StyxWoW.Me.IsInRaid) && StyxWoW.Me.CurrentMap.IsInstance && Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0),
    
                        Spell.Cast("Shadowfiend", ret => StyxWoW.Me.ManaPercent <= 80 && StyxWoW.Me.GotTarget),

                        Spell.BuffSelf("Desperate Prayer", ret => StyxWoW.Me.HealthPercent <= 50),
                        Spell.BuffSelf("Hymn of Hope", ret => StyxWoW.Me.ManaPercent <= 15 && (!SpellManager.HasSpell("Shadowfiend") || SpellManager.Spells["Shadowfiend"].Cooldown)),
                        Spell.BuffSelf("Divine Hymn", ret => Unit.NearbyFriendlyPlayers.Count(p => p.HealthPercent <= SingularSettings.Instance.Priest.DivineHymnHealth) >= SingularSettings.Instance.Priest.DivineHymnCount),

                        Spell.BuffSelf("Chakra"),
                        Spell.Cast(
                            "Prayer of Mending",
                            ret => (WoWUnit)ret,
                            ret => ret is WoWPlayer && Group.Tanks.Contains((WoWPlayer)ret) && !((WoWUnit)ret).HasMyAura("Prayer of Mending", 3) && 
                                   Group.Tanks.Where(t => t != (WoWPlayer)ret).All(p => !p.HasMyAura("Prayer of Mending"))),
                        Spell.Heal(
                            "Renew",
                            ret => (WoWUnit)ret,
                            ret => ret is WoWPlayer && Group.Tanks.Contains((WoWPlayer)ret) && !((WoWUnit)ret).HasMyAura("Renew")),
                        Spell.Heal("Prayer of Healing",
                            ret => (WoWUnit)ret,
                            ret => StyxWoW.Me.HasAura("Serendipity", 2) && Unit.NearbyFriendlyPlayers.Count(p => p.HealthPercent <= SingularSettings.Instance.Priest.PrayerOfHealingSerendipityHealth) >= SingularSettings.Instance.Priest.PrayerOfHealingSerendipityCount),
                        Spell.Heal("Circle of Healing",
                            ret => (WoWUnit)ret,
                            ret => Unit.NearbyFriendlyPlayers.Count(p => p.HealthPercent <= SingularSettings.Instance.Priest.CircleOfHealingHealth) >= SingularSettings.Instance.Priest.CircleOfHealingCount),
                        Spell.CastOnGround(
                            "Holy Word: Sanctuary",
                            ret => Clusters.GetBestUnitForCluster(Unit.NearbyFriendlyPlayers.Select(p => p.ToUnit()), ClusterType.Radius, 10f).Location,
                            ret => Clusters.GetClusterCount((WoWUnit)ret, Unit.NearbyFriendlyPlayers.Select(p => p.ToUnit()), ClusterType.Radius, 10f) >= 4),
                        Spell.Heal(
                            "Holy Word: Serenity",
                            ret => (WoWUnit)ret,
                            ret => ret is WoWPlayer && Group.Tanks.Contains((WoWPlayer)ret)),

                        Spell.Buff("Guardian Spirit",
                            ret => (WoWUnit)ret,
                            ret => ((WoWUnit)ret).HealthPercent <= 10),

                        Spell.CastOnGround("Lightwell", ret => StyxWoW.Me.Location.RayCast(WoWMathHelper.CalculateNeededFacing(StyxWoW.Me.Location, ((WoWUnit)ret).Location), 15f)),
                        Spell.Heal(
                            "Flash Heal",
                            ret => (WoWUnit)ret,
                            ret => StyxWoW.Me.HasAura("Surge of Light") && ((WoWUnit)ret).HealthPercent <= 90),
                        Spell.Heal(
                            "Flash Heal",
                            ret => (WoWUnit)ret,
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.HolyFlashHeal),
                        Spell.Heal(
                            "Greater Heal",
                            ret => (WoWUnit)ret,
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.HolyGreaterHeal),
                        Spell.Heal(
                            "Heal",
                            ret => (WoWUnit)ret,
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.HolyHeal),
                        new Decorator(
                            ret => StyxWoW.Me.Combat && StyxWoW.Me.GotTarget && Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                            new PrioritySelector(
                                Movement.CreateMoveToLosBehavior(),
                                Movement.CreateFaceTargetBehavior(),
                                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                                Spell.Buff("Shadow Word: Pain", true),
                                Spell.Cast("Holy Word: Chastise"),
                                Spell.Cast("Holy Fire"),
                                Spell.Cast("Smite"),
                                Movement.CreateMoveToTargetBehavior(true, 35f)
                                )),
                        new Decorator(
                            ret => moveInRange,
                            Movement.CreateMoveToTargetBehavior(true, 35f, ret => (WoWUnit)ret))

                        // Divine Hymn
                // Desperate Prayer
                // Prayer of Mending
                // Prayer of Healing
                // Power Word: Barrier
                // TODO: Add smite healing. Only if Atonement is talented. (Its useless otherwise)
                        )));
        }

        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.HolyPriest)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyHealComposite()
        {
            return
                new PrioritySelector(
                    CreateHolyHealOnlyBehavior());
        }

        // This behavior is used in combat/heal AND pull. Just so we're always healing our party.
        // Note: This will probably break shit if we're solo, but oh well!
        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.HolyPriest)]
        [Behavior(BehaviorType.Combat)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyCombatComposite()
        {
            return new PrioritySelector(

                new Decorator(
                    ret => Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                    new PrioritySelector(
                        Safers.EnsureTarget(),
                        Movement.CreateMoveToLosBehavior(),
                        Movement.CreateFaceTargetBehavior(),
                        Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                        Spell.Buff("Shadow Word: Pain", true),
                        Spell.Cast("Holy Word: Chastise"),
                        Spell.Cast("Holy Fire"),
                        Spell.Cast("Smite"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        ))
                );
        }
    }
}
