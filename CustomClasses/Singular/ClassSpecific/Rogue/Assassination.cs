using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommonBehaviors.Actions;

using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;

using TreeSharp;

using Action = TreeSharp.Action;

namespace Singular.ClassSpecific.Rogue
{
    class Assassination
    {
        #region Normal Rotation

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.AssasinationRogue)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateAssaRogueNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.BuffSelf("Sprint", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.HasAura("Stealth")),
                Spell.BuffSelf("Stealth"),
                // Garrote if we can, SS is kinda meh as an opener.
                Spell.Cast("Garrote", ret => StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Cheap Shot", ret => !SpellManager.HasSpell("Garrote") || !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Ambush", ret => !SpellManager.HasSpell("Cheap Shot") && StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Mutilate", ret => !SpellManager.HasSpell("Cheap Shot") && !StyxWoW.Me.CurrentTarget.MeIsBehind),

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
        [Spec(TalentSpec.AssasinationRogue)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateAssaRogueNormalCombat()
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

                Spell.BuffSelf("Vendetta", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 2),

                Spell.Buff("Rupture", true, ret => StyxWoW.Me.ComboPoints >= 4 && StyxWoW.Me.CurrentTarget.Elite),
                Spell.BuffSelf("Slice and Dice",
                    ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.GetAuraTimeLeft("Slice and Dice", true).TotalSeconds < 3),
                Spell.BuffSelf("Cold Blood",
                    ret => StyxWoW.Me.ComboPoints >= 4 && StyxWoW.Me.CurrentTarget.HealthPercent >= 35 ||
                           StyxWoW.Me.ComboPoints == 5 || !SpellManager.HasSpell("Envenom")),

                Spell.Cast("Eviscerate",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 40 && StyxWoW.Me.ComboPoints >= 2),
                Spell.Cast("Eviscerate",
                    ret => (StyxWoW.Me.CurrentTarget.HealthPercent <= 40 || !SpellManager.HasSpell("Envenom") || !StyxWoW.Me.CurrentTarget.Elite) && 
                           StyxWoW.Me.ComboPoints >= 4),

                Spell.Cast("Envenom",
                    ret => StyxWoW.Me.CurrentTarget.Elite && StyxWoW.Me.CurrentTarget.HealthPercent >= 35 && StyxWoW.Me.ComboPoints >= 4),
                Spell.Cast("Envenom",
                    ret => StyxWoW.Me.CurrentTarget.Elite && StyxWoW.Me.CurrentTarget.HealthPercent < 35 && StyxWoW.Me.ComboPoints == 5),

                Spell.Cast("Backstab",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35 && TalentManager.GetCount(1, 13) > 0 &&
                           StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Cold Blood")),
                Spell.Cast("Mutilate",
                    ret => (StyxWoW.Me.CurrentTarget.HealthPercent >= 35 || TalentManager.GetCount(1, 13) == 0 ||
                           !StyxWoW.Me.CurrentTarget.MeIsBehind) && !StyxWoW.Me.HasAura("Cold Blood")),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.AssasinationRogue)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateAssaRoguePvPPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.BuffSelf("Sprint", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.HasAura("Stealth")),
                Spell.BuffSelf("Stealth"),
                // Garrote if we can, SS is kinda meh as an opener.
                Spell.Cast("Garrote", 
                    ret => StyxWoW.Me.CurrentTarget.MeIsBehind && StyxWoW.Me.CurrentTarget.PowerType == WoWPowerType.Mana),
                Spell.Cast("Cheap Shot", 
                    ret => !SpellManager.HasSpell("Garrote") || !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Ambush", ret => !SpellManager.HasSpell("Cheap Shot") && StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Mutilate", ret => !SpellManager.HasSpell("Cheap Shot") && !StyxWoW.Me.CurrentTarget.MeIsBehind),

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
        [Spec(TalentSpec.AssasinationRogue)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateAssaRoguePvPCombat()
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

                Spell.BuffSelf("Vanish",
                    ret => TalentManager.GetCount(1, 14) > 0 && StyxWoW.Me.CurrentTarget.HasMyAura("Rupture") &&
                           StyxWoW.Me.HasAura("Slice and Dice")),
                Spell.Cast("Garrote",
                    ret => (StyxWoW.Me.HasAura("Vanish") || StyxWoW.Me.IsStealthed) &&
                           StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.BuffSelf("Vendetta"),
                Spell.Buff("Rupture", true, ret => StyxWoW.Me.ComboPoints >= 4),
                Spell.BuffSelf("Slice and Dice",
                    ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.GetAuraTimeLeft("Slice and Dice", true).TotalSeconds < 3),
                Spell.BuffSelf("Cold Blood",
                    ret => StyxWoW.Me.ComboPoints >= 4 && StyxWoW.Me.CurrentTarget.HealthPercent >= 35 ||
                           StyxWoW.Me.ComboPoints == 5 || !SpellManager.HasSpell("Envenom")),
                Spell.Cast("Eviscerate",
                    ret => (StyxWoW.Me.CurrentTarget.HealthPercent <= 40 || !SpellManager.HasSpell("Envenom")) && StyxWoW.Me.ComboPoints >= 4),
                Spell.Cast("Kidney Shot",
                    ret => StyxWoW.Me.ComboPoints >= 4 && !StyxWoW.Me.CurrentTarget.IsStunned()),
                Spell.Cast("Envenom",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent >= 35 && StyxWoW.Me.ComboPoints >= 4),
                Spell.Cast("Envenom",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35 && StyxWoW.Me.ComboPoints == 5),
                Spell.Cast("Backstab",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35 && TalentManager.GetCount(1, 13) > 0 &&
                           StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Cold Blood")),
                Spell.Cast("Mutilate",
                    ret => (StyxWoW.Me.CurrentTarget.HealthPercent >= 35 || TalentManager.GetCount(1, 13) == 0 ||
                           !StyxWoW.Me.CurrentTarget.MeIsBehind) && !StyxWoW.Me.HasAura("Cold Blood")),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.AssasinationRogue)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Instances)]
        public static Composite CreateAssaRogueInstancePull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.BuffSelf("Sprint", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.HasAura("Stealth")),
                Spell.BuffSelf("Stealth"),
                // Garrote if we can, SS is kinda meh as an opener.
                Spell.Cast("Garrote", ret => StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Cheap Shot", ret => !SpellManager.HasSpell("Garrote") || !StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Ambush", ret => !SpellManager.HasSpell("Cheap Shot") && StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.Cast("Mutilate", ret => !SpellManager.HasSpell("Cheap Shot") && !StyxWoW.Me.CurrentTarget.MeIsBehind),

                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying || StyxWoW.Me.CurrentTarget.Distance2DSqr < 5*5 && Math.Abs(StyxWoW.Me.Z - StyxWoW.Me.CurrentTarget.Z) >= 5,
                    new PrioritySelector(
                        Spell.Cast("Throw", ret => Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast("Shoot", ret => Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun)),
                        Spell.Cast("Stealth", ret => StyxWoW.Me.HasAura("Stealth"))
                        )),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Class(WoWClass.Rogue)]
        [Spec(TalentSpec.AssasinationRogue)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateAssaRogueInstanceCombat()
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

                Spell.BuffSelf("Vanish",
                    ret => TalentManager.GetCount(1, 14) >0 && StyxWoW.Me.CurrentTarget.HasMyAura("Rupture") && 
                           StyxWoW.Me.HasAura("Slice and Dice")),
                Spell.Cast("Garrote", 
                    ret => (StyxWoW.Me.HasAura("Vanish") || StyxWoW.Me.IsStealthed) &&
                           StyxWoW.Me.CurrentTarget.MeIsBehind),
                Spell.BuffSelf("Vendetta", 
                    ret => StyxWoW.Me.CurrentTarget.IsBoss() && 
                           (StyxWoW.Me.CurrentTarget.HealthPercent < 35 || TalentManager.GetCount(1,13) == 0)),

                new Decorator(
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 8*8) >= 3,
                    Spell.BuffSelf("Fan of Knives", ret => Item.RangedIsType(WoWItemWeaponClass.Thrown))),

                Spell.Buff("Rupture", true, ret => StyxWoW.Me.ComboPoints >= 4),
                Spell.BuffSelf("Slice and Dice", 
                    ret => StyxWoW.Me.RawComboPoints > 0 && StyxWoW.Me.GetAuraTimeLeft("Slice and Dice", true).TotalSeconds < 3),
                Spell.BuffSelf("Cold Blood",
                    ret => StyxWoW.Me.ComboPoints >= 4 && StyxWoW.Me.CurrentTarget.HealthPercent >= 35 ||
                           StyxWoW.Me.ComboPoints == 5 || !SpellManager.HasSpell("Envenom")),
                Spell.Cast("Eviscerate", 
                    ret => (!StyxWoW.Me.CurrentTarget.Elite || !SpellManager.HasSpell("Envenom")) && StyxWoW.Me.ComboPoints >= 4),
                Spell.Cast("Envenom",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent >= 35 && StyxWoW.Me.ComboPoints >= 4),
                Spell.Cast("Envenom",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35 && StyxWoW.Me.ComboPoints == 5),
                Spell.Cast("Backstab",
                    ret => StyxWoW.Me.CurrentTarget.HealthPercent < 35 && TalentManager.GetCount(1,13) > 0 && 
                           StyxWoW.Me.CurrentTarget.MeIsBehind && !StyxWoW.Me.HasAura("Cold Blood")),
                Spell.Cast("Mutilate",
                    ret => (StyxWoW.Me.CurrentTarget.HealthPercent >= 35 || TalentManager.GetCount(1,13) == 0 ||
                           !StyxWoW.Me.CurrentTarget.MeIsBehind) && !StyxWoW.Me.HasAura("Cold Blood")),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion
    }
}
