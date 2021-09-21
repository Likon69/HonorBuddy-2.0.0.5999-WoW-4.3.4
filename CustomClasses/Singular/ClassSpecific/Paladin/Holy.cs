using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Singular.ClassSpecific.Paladin
{
    public static class Holy
    {
        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.HolyPaladin)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyPaladinRest()
        {
            return new PrioritySelector(
                // Heal self before resting. There is no need to eat while we have 100% mana
                CreatePaladinHealBehavior(true),
                // Rest up damnit! Do this first, so we make sure we're fully rested.
                Rest.CreateDefaultRestBehaviour(),
                // Can we res people?
                Spell.Resurrect("Redemption"),
                // Make sure we're healing OOC too!
                CreatePaladinHealBehavior(false, false));
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.HolyPaladin)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyPaladinHealBehavior()
        {
            return
                new PrioritySelector(
                    CreatePaladinHealBehavior());
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.HolyPaladin)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyPaladinCombatBuffsBehavior()
        {
            return
                new PrioritySelector(
                    Spell.BuffSelf(
                        "Divine Plea",
                        ret => StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Paladin.DivinePleaMana)
                    );
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.HolyPaladin)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyPaladinCombatBehavior()
        {
            return
                new PrioritySelector(
                    Spell.Buff("Judgement",
                               ret => SpellManager.HasSpell("Judgement") && StyxWoW.Me.GotTarget &&
                                       StyxWoW.Me.CurrentTarget.Distance <= SpellManager.Spells["Judgement"].MaxRange - 2 &&
                                       StyxWoW.Me.CurrentTarget.InLineOfSpellSight &&
                                       StyxWoW.Me.IsSafelyFacing(StyxWoW.Me.CurrentTarget)),
                    new Decorator(
                        ret => Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                        new PrioritySelector(
                            Safers.EnsureTarget(),
                            Movement.CreateMoveToLosBehavior(),
                            Movement.CreateFaceTargetBehavior(),
                            Helpers.Common.CreateAutoAttack(true),
                            Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                            Spell.Buff("Judgement"),
                            Spell.Cast("Hammer of Wrath"),
                            Spell.Cast("Holy Shock"),
                            Spell.Cast("Crusader Strike"),
                            Spell.Cast("Exorcism"),
                            Spell.Cast("Holy Wrath"),
                            Spell.Cast("Consecration"),
                            Movement.CreateMoveToTargetBehavior(true, 5f)
                            ))
                    );
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.HolyPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyPaladinPullBehavior()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => !StyxWoW.Me.IsInParty && !StyxWoW.Me.IsInRaid,
                    new PrioritySelector(
                        Movement.CreateMoveToLosBehavior(),
                        Movement.CreateFaceTargetBehavior(),
                        Spell.Cast("Judgement"),
                        Helpers.Common.CreateAutoAttack(true),
                        Movement.CreateMoveToTargetBehavior(true, 5f)
                        ))
                );
        }

        internal static Composite CreatePaladinHealBehavior()
        {
            return CreatePaladinHealBehavior(false, true);
        }

        internal static Composite CreatePaladinHealBehavior(bool selfOnly)
        {
            return CreatePaladinHealBehavior(selfOnly, false);
        }

        internal static Composite CreatePaladinHealBehavior(bool selfOnly, bool moveInRange)
        {
            HealerManager.NeedHealTargeting = true;

            return
                new PrioritySelector(
                    ctx => selfOnly ? StyxWoW.Me : HealerManager.Instance.FirstUnit,
                    new Decorator(
                    ret => ret != null && (moveInRange || ((WoWUnit)ret).InLineOfSpellSight && ((WoWUnit)ret).DistanceSqr < 40 * 40),
                        new PrioritySelector(
                            Spell.WaitForCast(),
                            new Decorator(
                                ret => moveInRange,
                                Movement.CreateMoveToLosBehavior(ret => (WoWUnit)ret)),
                            Spell.Cast(
                                "Beacon of Light",
                                ret => (WoWUnit)ret,
                                ret => ret is WoWPlayer && Group.Tanks.Contains((WoWPlayer)ret) && Group.Tanks.All(t => !t.HasMyAura("Beacon of Light"))),
                            Spell.Heal(
                                "Lay on Hands",
                                ret => (WoWUnit)ret,
                                ret => StyxWoW.Me.Combat && !((WoWUnit)ret).HasAura("Forbearance") &&
                                       ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Paladin.LayOnHandsHealth),
                            Spell.Heal(
                                "Light of Dawn",
                                ret => StyxWoW.Me,
                                ret => StyxWoW.Me.CurrentHolyPower == 3 &&
                                       Unit.NearbyFriendlyPlayers.Count(p =>
                                           p.HealthPercent <= SingularSettings.Instance.Paladin.LightOfDawnHealth && p != StyxWoW.Me &&
                                           p.DistanceSqr < 30 * 30 && StyxWoW.Me.IsSafelyFacing(p.Location)) >= SingularSettings.Instance.Paladin.LightOfDawnCount),
                            Spell.Heal(
                                "Word of Glory",
                                ret => (WoWUnit)ret,
                                ret => StyxWoW.Me.CurrentHolyPower == 3 && ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Paladin.WordOfGloryHealth),
                            Spell.Heal(
                                "Holy Shock",
                                ret => (WoWUnit)ret,
                                ret => ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Paladin.HolyShockHealth),
                            Spell.Heal(
                                "Flash of Light",
                                ret => (WoWUnit)ret,
                                ret => ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Paladin.FlashOfLightHealth),
                            Spell.Heal(
                                "Divine Light",
                                ret => (WoWUnit)ret,
                                ret => ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Paladin.DivineLightHealth),
                            Spell.Heal(
                                "Holy Light",
                                ret => (WoWUnit)ret,
                                ret => ((WoWUnit)ret).HealthPercent <= SingularSettings.Instance.Paladin.HolyLightHealth),
                            new Decorator(
                                ret => StyxWoW.Me.Combat && StyxWoW.Me.GotTarget && Unit.NearbyFriendlyPlayers.Count(u => u.IsInMyPartyOrRaid) == 0,
                                new PrioritySelector(
                                    Movement.CreateMoveToLosBehavior(),
                                    Movement.CreateFaceTargetBehavior(),
                                    Helpers.Common.CreateAutoAttack(true),
                                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                                    Spell.Buff("Judgement"),
                                    Spell.Cast("Hammer of Wrath"),
                                    Spell.Cast("Holy Shock"),
                                    Spell.Cast("Crusader Strike"),
                                    Spell.Cast("Exorcism"),
                                    Spell.Cast("Holy Wrath"),
                                    Spell.Cast("Consecration"),
                                    Movement.CreateMoveToTargetBehavior(true, 5f)
                                    )),
                            new Decorator(
                                ret => moveInRange,
                // Get in range
                                Movement.CreateMoveToTargetBehavior(true, 35f, ret => (WoWUnit)ret))
                            )));
        }
    }
}
