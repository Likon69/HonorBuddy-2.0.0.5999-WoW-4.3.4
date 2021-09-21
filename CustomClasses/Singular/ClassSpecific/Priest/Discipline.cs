using System;
using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Singular.ClassSpecific.Priest
{
    public class Discipline
    {
        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.DisciplinePriest)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateDiscHealRest()
        {
            return new PrioritySelector(
                Spell.WaitForCast(),
                // Heal self before resting. There is no need to eat while we have 100% mana
                CreateDiscHealOnlyBehavior(true),
                // Rest up damnit! Do this first, so we make sure we're fully rested.
                Rest.CreateDefaultRestBehaviour(),
                // Can we res people?
                Spell.Resurrect("Resurrection"),
                // Make sure we're healing OOC too!
                CreateDiscHealOnlyBehavior(false, false)
                );
        }

        public static Composite CreateDiscHealOnlyBehavior()
        {
            return CreateDiscHealOnlyBehavior(false, true);
        }

        public static Composite CreateDiscHealOnlyBehavior(bool selfOnly)
        {
            return CreateDiscHealOnlyBehavior(selfOnly, false);
        }

        public static Composite CreateDiscHealOnlyBehavior(bool selfOnly, bool moveInRange)
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
                        // Ensure we're in range of the unit to heal, and it's in LOS.
                        //CreateMoveToAndFace(35f, ret => (WoWUnit)ret),
                        //Spell.Buff("Renew", ret => HealTargeting.Instance.TargetList.FirstOrDefault(u => !u.HasAura("Renew") && u.HealthPercent < 90) != null, ret => HealTargeting.Instance.TargetList.FirstOrDefault(u => !u.HasAura("Renew") && u.HealthPercent < 90)),
                        // use fade to drop aggro.
                        Spell.Cast("Fade", ret => (StyxWoW.Me.IsInParty || StyxWoW.Me.IsInRaid) && StyxWoW.Me.CurrentMap.IsInstance && Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0),
                        Spell.Buff(
                            "Power Word: Shield",
                            ret => (WoWUnit)ret,
                            ret => !((WoWUnit)ret).HasAura("Weakened Soul") && ((WoWUnit)ret).Combat && ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.PowerWordShield),
                        new Decorator(
                            ret =>
                            Unit.NearbyFriendlyPlayers.Count(p => !p.Dead && p.HealthPercent < SingularSettings.Instance.Priest.PrayerOfHealing) >
                            SingularSettings.Instance.Priest.PrayerOfHealingCount &&
                            (SpellManager.CanCast("Prayer of Healing") || SpellManager.CanCast("Divine Hymn")),
                            new Sequence(
                                Spell.Cast("Archangel"),
                        // This will skip over DH if we can't cast it.
                        // If we can, the sequence fails, since PoH can't be cast (as we're still casting at this point)
                                new DecoratorContinue(
                                    ret => SpellManager.CanCast("Divine Hymn"),
                                    Spell.Heal("Divine Hymn")),
                                Spell.Heal("Prayer of Healing"))),
                        Spell.Heal(
                            "Pain Supression",
                            ret => (WoWUnit)ret, 
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.PainSuppression),
                        Spell.Heal(
                            "Penance",
                            ret => (WoWUnit)ret, 
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.Penance),
                        Spell.Heal(
                            "Flash Heal",
                            ret => (WoWUnit)ret, 
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.FlashHeal),
                        Spell.Heal(
                            "Binding Heal",
                            ret => (WoWUnit)ret,
                            ret => (WoWUnit)ret != StyxWoW.Me && ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.BindingHealThem &&
                                   StyxWoW.Me.HealthPercent < SingularSettings.Instance.Priest.BindingHealMe),
                        Spell.Heal(
                            "Greater Heal",
                            ret => (WoWUnit)ret, 
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.GreaterHeal),
                        Spell.Heal(
                            "Heal",
                            ret => (WoWUnit)ret, 
                            ret => ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.Heal),
                        Spell.Heal(
                            "Renew",
                            ret => (WoWUnit)ret, 
                            ret => !((WoWUnit)ret).HasMyAura("Renew") && ((WoWUnit)ret).HealthPercent < SingularSettings.Instance.Priest.Renew),
                        Spell.Heal(
                            "Prayer of Mending",
                            ret => (WoWUnit)ret,
                            ret => !((WoWUnit)ret).HasMyAura("Prayer of Mending") && ((WoWUnit)ret).HealthPercent < 90),
                        new Decorator(
                            ret => StyxWoW.Me.Combat && StyxWoW.Me.GotTarget && Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                            new PrioritySelector(
                                Movement.CreateMoveToLosBehavior(),
                                Movement.CreateFaceTargetBehavior(),
                                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                                Spell.Buff("Shadow Word: Pain", true),
                                Spell.Cast("Penance"),
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
        [Spec(TalentSpec.DisciplinePriest)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.All)]
        public static Composite CreateDiscHealComposite()
        {
            return
                new PrioritySelector(
                    CreateDiscHealOnlyBehavior());
        }

        // This behavior is used in combat/heal AND pull. Just so we're always healing our party.
        // Note: This will probably break shit if we're solo, but oh well!
        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.DisciplinePriest)]
        [Behavior(BehaviorType.Combat)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateDiscCombatComposite()
        {
            return new PrioritySelector(

                new Decorator(
                    ret => Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                    new PrioritySelector(
                        Safers.EnsureTarget(),
                        Movement.CreateMoveToLosBehavior(),
                        Movement.CreateFaceTargetBehavior(),
                        Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                        Spell.Cast("Shadowfiend", ret => StyxWoW.Me.ManaPercent < 50),
                        Spell.Cast("Archangel", ret => StyxWoW.Me.HasAura("Evangelism", 5)),
                        Spell.Cast("Shadow Word: Death", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 25),
                        Spell.Buff("Shadow Word: Pain", true),
                        Spell.Cast("Penance"),
                        Spell.Cast("Holy Fire"),
                        Spell.Cast("Smite"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        ))
                );
        }
    }
}
