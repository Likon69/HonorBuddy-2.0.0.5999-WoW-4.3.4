using System;
using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using TreeSharp;
using CommonBehaviors.Actions;

namespace Singular.ClassSpecific.Rogue
{
    public class Subtlety
    {
        #region Normal Rotation

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.SubtletyRogue)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateSubtletyRogueNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.BuffSelf("Stealth"),
                // Garrote if we can, SS is kinda meh as an opener.
                Spell.Cast("Premeditation"),
                Spell.Cast("Shadowstep"),
                Spell.Cast("Garrote", ret => StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Ambush", ret => !SpellManager.HasSpell("Garrote") && StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Cheap Shot", ret => !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Hemorrhage", ret => !SpellManager.HasSpell("Cheap Shot") && !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Sinister Strike", ret => !SpellManager.HasSpell("Hemorrhage")),

                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying || StyxWoW.Me.CurrentTarget.Distance2DSqr < 5 * 5 && Math.Abs(StyxWoW.Me.Z - StyxWoW.Me.CurrentTarget.Z) >= 5,
                    new PrioritySelector(
                        Spell.Cast("Throw", ret => Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast("Shoot", ret => Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun)),
                        Spell.Cast("Stealth", ret => StyxWoW.Me.HasAura("Stealth"))
                        )),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.SubtletyRogue)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateSubtletyRogueNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                new Decorator(
                    ret => !StyxWoW.Me.HasAura("Vanish"),
                    Helpers.Common.CreateAutoAttack(true)),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Don't do anything if we casted vanish
                new Decorator(
                    ret => StyxWoW.Me.HasAura("Vanish"),
                    new ActionAlwaysSucceed()),

                // Defensive
                Spell.BuffSelf("Evasion",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 6 * 6 && u.IsTargetingMeOrPet) >= 2),

                Spell.BuffSelf("Cloak of Shadows",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet && u.IsCasting) >= 1),

                Spell.BuffSelf("Smoke Bomb", ret => StyxWoW.Me.HealthPercent < 15),

                Common.CreateRogueBlindOnAddBehavior(),

                // Redirect if we have CP left
                Spell.Cast("Redirect", ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.ComboPoints < 1),

                Spell.BuffSelf("Vanish",
                    ret => StyxWoW.Me.HealthPercent < 20),

                Spell.BuffSelf("Preparation",
                    ret => SpellManager.HasSpell("Vanish") && SpellManager.Spells["Vanish"].CooldownTimeLeft.TotalSeconds > 10 &&
                           StyxWoW.Me.HealthPercent < 25),

                Spell.BuffSelf("Shadow Dance", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 3),
                Spell.Cast("Cheap Shot", ret => StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.BuffSelf("Slice and Dice",
                    ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.GetAuraTimeLeft("Slice and Dice", true).TotalSeconds < 3),
                Spell.Buff("Rupture", true, ret => StyxWoW.Me.ComboPoints >= 4 && StyxWoW.Me.CurrentTarget.Elite),
                Spell.BuffSelf("Recuperate", ret => TalentManager.GetCount(3, 8) > 0 && StyxWoW.Me.RawComboPoints > 0),
                Spell.Cast("Eviscerate", ret => StyxWoW.Me.CurrentTarget.HealthPercent < 40 && StyxWoW.Me.ComboPoints >= 2),
                Spell.Cast("Eviscerate", ret => StyxWoW.Me.ComboPoints == 5),
                Spell.Cast("Ambush", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.Cast("Backstab", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.Cast("Hemorrhage", ret => !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Sinister Strike", ret => !SpellManager.HasSpell("Hemorrhage") && !StyxWoW.Me.CurrentTarget.MeIsBehind),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.SubtletyRogue)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateSubtletyRoguePvPPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.BuffSelf("Stealth"),
                // Garrote if we can, SS is kinda meh as an opener.
                Spell.Cast("Premeditation"),
                Spell.Cast("Shadowstep"),

                Spell.Cast("Garrote",
                    ret => StyxWoW.Me.CurrentTarget.MeIsBehind && StyxWoW.Me.CurrentTarget.PowerType == WoWPowerType.Mana),
                Spell.Cast("Cheap Shot",
                    ret => !SpellManager.HasSpell("Garrote") || !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Ambush", ret => !SpellManager.HasSpell("Cheap Shot") && StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Hemorrhage", ret => !SpellManager.HasSpell("Cheap Shot") && !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Sinister Strike", ret => !SpellManager.HasSpell("Hemorrhage")),

                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying || StyxWoW.Me.CurrentTarget.Distance2DSqr < 5 * 5 && Math.Abs(StyxWoW.Me.Z - StyxWoW.Me.CurrentTarget.Z) >= 5,
                    new PrioritySelector(
                        Spell.Cast("Throw", ret => Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast("Shoot", ret => Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun)),
                        Spell.Cast("Stealth", ret => StyxWoW.Me.HasAura("Stealth"))
                        )),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.SubtletyRogue)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateSubtletyRoguePvPCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                new Decorator(
                    ret => !StyxWoW.Me.HasAura("Vanish"),
                    Helpers.Common.CreateAutoAttack(true)),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Defensive
                Spell.BuffSelf("Evasion",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 6 * 6 && u.IsTargetingMeOrPet) >= 1),

                Spell.BuffSelf("Cloak of Shadows",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet && u.IsCasting) >= 1),

                Spell.BuffSelf("Smoke Bomb", ret => StyxWoW.Me.HealthPercent < 15),

                // Redirect if we have CP left
                Spell.Cast("Redirect", ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.ComboPoints < 1),

                Spell.BuffSelf("Preparation",
                    ret => SpellManager.HasSpell("Vanish") && SpellManager.Spells["Vanish"].CooldownTimeLeft.TotalSeconds > 10 &&
                           SpellManager.Spells["Shadowstep"].CooldownTimeLeft.TotalSeconds > 10),
                Spell.BuffSelf("Shadow Dance", ret => StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.BuffSelf("Slice and Dice",
                    ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.GetAuraTimeLeft("Slice and Dice", true).TotalSeconds < 3),
                Spell.Buff("Rupture", true, ret => StyxWoW.Me.ComboPoints >= 4),
                Spell.BuffSelf("Recuperate", ret => TalentManager.GetCount(3, 8) > 0 && StyxWoW.Me.RawComboPoints > 0),
                Spell.Cast("Eviscerate", ret => StyxWoW.Me.ComboPoints == 5),
                // Vanish + Shadowstep + Premeditation + Ambush combo
                new Decorator(
                    ret => StyxWoW.Me.HasAura("Vanish"),
                    new PrioritySelector(
                        Spell.Cast("Shadowstep"),
                        Spell.BuffSelf("Premeditation", ret => StyxWoW.Me.ComboPoints <= 3),
                        Spell.Cast("Ambush", ret => StyxWoW.Me.CurrentTarget.MeIsBehind)
                        )),
                Spell.Cast("Ambush", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.Cast("Backstab", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.Cast("Hemorrhage", ret => !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Sinister Strike", ret => !SpellManager.HasSpell("Hemorrhage") && !StyxWoW.Me.CurrentTarget.MeIsBehind),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.SubtletyRogue)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Instances)]
        public static Composite CreateSubtletyRogueInstancePull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.BuffSelf("Stealth"),
                // Garrote if we can, SS is kinda meh as an opener.
                Spell.Cast("Premeditation", ret => StyxWoW.Me.ComboPoints <= 3),
                Spell.Cast("Shadowstep"),
                Spell.Cast("Garrote", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Vanish")),
                Spell.Cast("Ambush", ret => (!SpellManager.HasSpell("Garrote") || StyxWoW.Me.HasAura("Vanish")) && StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Cheap Shot", ret => !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Hemorrhage", ret => !SpellManager.HasSpell("Cheap Shot") && !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Sinister Strike", ret => !SpellManager.HasSpell("Hemorrhage")),

                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying || StyxWoW.Me.CurrentTarget.Distance2DSqr < 5 * 5 && Math.Abs(StyxWoW.Me.Z - StyxWoW.Me.CurrentTarget.Z) >= 5,
                    new PrioritySelector(
                        Spell.Cast("Throw", ret => Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast("Shoot", ret => Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun)),
                        Spell.Cast("Stealth", ret => StyxWoW.Me.HasAura("Stealth"))
                        )),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.SubtletyRogue)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateSubtletyRogueInstanceCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                new Decorator(
                    ret => !StyxWoW.Me.HasAura("Vanish"),
                    Helpers.Common.CreateAutoAttack(true)),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Defensive
                Spell.BuffSelf("Evasion",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 6 * 6 && u.IsTargetingMeOrPet) >= 1),

                Spell.BuffSelf("Cloak of Shadows",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet && u.IsCasting) >= 1),

                // Redirect if we have CP left
                Spell.Cast("Redirect", ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.ComboPoints < 1),

                // Agro management
                Spell.Cast(
                    "Tricks of the Trade",
                    ret => Common.BestTricksTarget,
                    ret => SingularSettings.Instance.Rogue.UseTricksOfTheTrade),

                Spell.Cast("Feint", ret => StyxWoW.Me.CurrentTarget.ThreatInfo.RawPercent > 80),

                Movement.CreateMoveBehindTargetBehavior(),

                new Decorator(
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 8 * 8) >= 3,
                    Spell.BuffSelf("Fan of Knives", ret => Item.RangedIsType(WoWItemWeaponClass.Thrown))),

                Spell.BuffSelf("Preparation", 
                    ret => SpellManager.HasSpell("Vanish") && SpellManager.Spells["Vanish"].CooldownTimeLeft.TotalSeconds > 10 &&
                           SpellManager.Spells["Shadowstep"].CooldownTimeLeft.TotalSeconds > 10),
                Spell.BuffSelf("Shadow Dance", ret => StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.BuffSelf("Slice and Dice",
                    ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.GetAuraTimeLeft("Slice and Dice", true).TotalSeconds < 3),
                Spell.Buff("Rupture", true, ret => StyxWoW.Me.ComboPoints >= 4),
                Spell.BuffSelf("Recuperate", ret => TalentManager.GetCount(3, 8) > 0 && StyxWoW.Me.RawComboPoints > 0),
                Spell.Cast("Eviscerate", ret => StyxWoW.Me.ComboPoints == 5),
                // Vanish + Shadowstep + Premeditation + Ambush combo
                new Decorator(
                    ret => StyxWoW.Me.HasAura("Vanish"),
                    new PrioritySelector(
                        Spell.Cast("Shadowstep"),
                        Spell.BuffSelf("Premeditation", ret => StyxWoW.Me.ComboPoints <= 3),
                        Spell.Cast("Ambush", ret => StyxWoW.Me.CurrentTarget.MeIsBehind)
                        )),
                Spell.Cast("Ambush", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.Cast("Backstab", ret => StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Shadow Dance")),
                Spell.Cast("Hemorrhage", ret => !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Sinister Strike", ret => !SpellManager.HasSpell("Hemorrhage") && !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion
    }
}
