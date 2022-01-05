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
    public class Protection
    {
        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.HolyPaladin)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateHolyPaladinRest()
        {
            return new PrioritySelector(
                // Rest up damnit! Do this first, so we make sure we're fully rested.
                Rest.CreateDefaultRestBehaviour(),
                // Can we res people?
                Spell.Resurrect("Redemption"));
        }


        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.ProtectionPaladin)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.All)]
        public static Composite CreateProtectionPaladinCombat()
        {
            return new PrioritySelector(
                ctx => TankManager.Instance.FirstUnit ?? StyxWoW.Me.CurrentTarget,
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => (WoWUnit)ret),

                // Seal twisting. If our mana gets stupid low, just throw on insight to get some mana back quickly, then put our main seal back on.
                // This is Seal of Truth once we get it, Righteousness when we dont.
                Spell.BuffSelf("Seal of Insight", ret => StyxWoW.Me.ManaPercent < 25),
                Spell.BuffSelf("Seal of Truth", ret => StyxWoW.Me.ManaPercent >= 25),
                Spell.BuffSelf("Seal of Righteousness", ret => StyxWoW.Me.ManaPercent >= 25 && !SpellManager.HasSpell("Seal of Truth")),

                // Defensive
                Spell.BuffSelf("Hand of Freedom",
                    ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                          WoWSpellMechanic.Disoriented,
                                                          WoWSpellMechanic.Frozen,
                                                          WoWSpellMechanic.Incapacitated,
                                                          WoWSpellMechanic.Rooted,
                                                          WoWSpellMechanic.Slowed,
                                                          WoWSpellMechanic.Snared)),

                Spell.BuffSelf("Divine Shield",
                    ret => StyxWoW.Me.CurrentMap.IsBattleground && StyxWoW.Me.HealthPercent <= 20 && !StyxWoW.Me.HasAura("Forbearance")),

                Spell.Cast("Hand of Reckoning",
                    ret => TankManager.Instance.NeedToTaunt.FirstOrDefault(),
                    ret => SingularSettings.Instance.EnableTaunting && StyxWoW.Me.IsInInstance),

                //Multi target
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(8f).Any(),
                    new PrioritySelector(
			Spell.Cast("Divine Plea", ret => StyxWoW.Me.ManaPercent < 75),
                        Spell.Cast("Hammer of the Righteous"),
                        Spell.Cast("Hammer of Justice", ctx => !StyxWoW.Me.IsInParty),
                        Spell.Cast("Consecration", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= SingularSettings.Instance.Paladin.ProtConsecrationCount 
                            || StyxWoW.Me.CurrentTarget.IsBoss()),
                        Spell.Cast("Holy Wrath"),
                        Spell.Cast("Avenger's Shield", ret => !SingularSettings.Instance.Paladin.AvengersPullOnly),
                        Spell.BuffSelf("Inquisition"),
                        Spell.Cast("Shield of the Righteous", ret => StyxWoW.Me.CurrentHolyPower == 3),
                        Spell.Cast("Judgement"),
                        Spell.Cast("Crusader Strike"),
                        Movement.CreateMoveToMeleeBehavior(true)
                        )),
                //Single target
		Spell.Cast("Divine Plea", ret => StyxWoW.Me.ManaPercent < 75),
                Spell.Cast("Shield of the Righteous", ret => StyxWoW.Me.CurrentHolyPower == 3),
                Spell.Cast("Crusader Strike"),
                Spell.Cast("Hammer of Justice"),
                Spell.Cast("Judgement"),
                Spell.Cast("Hammer of Wrath", ret => ((WoWUnit)ret).HealthPercent <= 20),
                Spell.Cast("Avenger's Shield", ret => !SingularSettings.Instance.Paladin.AvengersPullOnly),
                // Don't waste mana on cons if its not a boss.
                Spell.Cast("Consecration", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= SingularSettings.Instance.Paladin.ProtConsecrationCount),
                Spell.Cast("Holy Wrath"),
                Movement.CreateMoveToMeleeBehavior(true));
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.ProtectionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateProtectionPaladinPull()
        {
            return
                new PrioritySelector(
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Helpers.Common.CreateAutoAttack(true),
                    Spell.Cast("Avenger's Shield"),
                    Spell.Cast("Judgement"),
                    Movement.CreateMoveToTargetBehavior(true, 5f)
                    );
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.ProtectionPaladin)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateProtectionPaladinCombatBuffs()
        {
            return
                new PrioritySelector(
                    Spell.Cast(
                        "Hand of Reckoning",
                        ret => TankManager.Instance.NeedToTaunt.FirstOrDefault(),
                        ret => SingularSettings.Instance.EnableTaunting && TankManager.Instance.NeedToTaunt.Count != 0),
                    Spell.BuffSelf("Avenging Wrath"),
                    Spell.BuffSelf(
                        "Lay on Hands",
                        ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.LayOnHandsHealth && !StyxWoW.Me.HasAura("Forbearance")),
                    Spell.BuffSelf(
                        "Guardian of Ancient Kings",
                        ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.GoAKHealth),
                    Spell.BuffSelf(
                        "Ardent Defender",
                        ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.ArdentDefenderHealth),
                    Spell.BuffSelf(
                        "Divine Protection",
                        ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.DivineProtectionHealthProt),

                    Spell.BuffSelf("Word of Glory", ret => StyxWoW.Me.HealthPercent < 50 && StyxWoW.Me.CurrentHolyPower == 3),
                    Spell.BuffSelf("Word of Glory", ret => StyxWoW.Me.HealthPercent < 25 && StyxWoW.Me.CurrentHolyPower == 2),
                    Spell.BuffSelf("Word of Glory", ret => StyxWoW.Me.HealthPercent < 15 && StyxWoW.Me.CurrentHolyPower == 1)
                    );
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.ProtectionPaladin)]
        [Behavior(BehaviorType.PullBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateProtectionPaladinPullBuffs()
        {
            return
                new PrioritySelector(
                    Spell.BuffSelf("Divine Plea")
                    );
        }
    }
}
