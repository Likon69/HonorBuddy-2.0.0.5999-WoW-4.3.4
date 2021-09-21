using System;
using System.Linq;

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
using Singular.Lists;
using Styx.WoWInternals.WoWObjects;

namespace Singular.ClassSpecific.Shaman
{
    public class Enhancement
    {
        #region Common

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateEnhancementShamanPreCombatBuffs()
        {
            return new PrioritySelector(

                new Decorator(
                    ret => StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 283 && StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole && SpellManager.HasSpell("Windfury Weapon") && 
                            SpellManager.CanCast("Windfury Weapon", null, false, false),
                    new Sequence(
                        new Action(ret => Lua.DoString("CancelItemTempEnchantment(1) CancelItemTempEnchantment(2)")),
                        new Action(ret => Logger.Write("Imbuing main hand weapon with Windfury")),
                        new Action(ret => SpellManager.Cast("Windfury Weapon", null)))),
                new Decorator(
                    ret => StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 5 && StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole && !SpellManager.HasSpell("Windfury Weapon") &&
                            SpellManager.CanCast("Flametongue Weapon", null, false, false),
                    new Sequence(
                        new Action(ret => Lua.DoString("CancelItemTempEnchantment(1) CancelItemTempEnchantment(2)")),
                        new Action(ret => Logger.Write("Imbuing main hand weapon with Flametongue")),
                        new Action(ret => SpellManager.Cast("Flametongue Weapon", null)))),
                new Decorator(
                    ret => StyxWoW.Me.Inventory.Equipped.OffHand != null && 
                           StyxWoW.Me.Inventory.Equipped.OffHand.ItemInfo.ItemClass == WoWItemClass.Weapon &&
                           StyxWoW.Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != 5 &&
                           StyxWoW.Me.Inventory.Equipped.MainHand != null && StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment != null && StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole && SpellManager.CanCast("Flametongue Weapon", null, false, false),
                    new Sequence(
                        new Action(ret => Lua.DoString("CancelItemTempEnchantment(2)")),
                        new Action(ret => Logger.Write("Imbuing off hand weapon with Flametongue")),
                        new Action(ret => SpellManager.Cast("Flametongue Weapon", null)))),
                
                Spell.Cast("Lightning Shield", ret => StyxWoW.Me, ret => !StyxWoW.Me.HasAura("Lightning Shield", 2)),
                new Decorator(ret => Totems.NeedToRecallTotems,
                    new Action(ret => Totems.RecallTotems()))
                );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateEnhancementShamanRest()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => !StyxWoW.Me.HasAura("Drink") && !StyxWoW.Me.HasAura("Food"),
                        CreateEnhancementShamanHeal()),
                    Rest.CreateDefaultRestBehaviour(),
                    Spell.Resurrect("Ancestral Spirit")
                    );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.Instances)]
        [Context(WoWContext.Normal)]
        public static Composite CreateEnhancementShamanHeal()
        {
            return
                new Decorator(
                    ret => SingularSettings.Instance.Shaman.EnhancementHeal,
                    new PrioritySelector(
                // Heal the party in dungeons if the healer is dead
                        new Decorator(
                            ret => StyxWoW.Me.CurrentMap.IsDungeon && !StyxWoW.Me.IsInRaid &&
                                   Group.Healers.Any() && Group.Healers.Count(h => h.IsAlive) == 0,
                            Restoration.CreateRestoShamanHealingOnlyBehavior()),

                        // This will work for both solo play and battlegrounds
                        new Decorator(
                            ret => Group.Healers.Count(h => h.IsAlive) == 0,
                            new PrioritySelector(
                                Spell.Heal("Healing Wave",
                                    ret => StyxWoW.Me,
                                    ret => !SpellManager.HasSpell("Healing Surge") && StyxWoW.Me.HealthPercent <= 60),

                                Spell.Heal("Healing Surge",
                                    ret => StyxWoW.Me,
                                    ret => StyxWoW.Me.HealthPercent <= 60)))
                        ));
        }

        #endregion

        #region Normal Rotation

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateEnhancementShamanNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Spell.Cast("Lightning Shield", ret => StyxWoW.Me, ret => !StyxWoW.Me.HasAura("Lightning Shield", 2)),

                new Decorator(
                    ret => StyxWoW.Me.Level < 20,
                    new PrioritySelector(
                        new Decorator(
                            ret => StyxWoW.Me.CurrentTarget.DistanceSqr < 40 * 40,
                            Totems.CreateSetTotems()),
                        Spell.Cast("Lightning Bolt"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                Common.CreateAutoAttack(true),
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.DistanceSqr < 20 * 20,
                    Totems.CreateSetTotems()),
                Spell.Cast("Earth Shock"),
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateEnhancementShamanNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Totems.CreateSetTotems(),
                Common.CreateAutoAttack(true),
                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.BuffSelf("Lightning Shield"),
                Spell.BuffSelf("Spiritwalker's Grace", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat),
                Spell.BuffSelf("Feral Spirit", ret => StyxWoW.Me.CurrentTarget.Elite || Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 3),

                // Totem stuff
                // Pop the ele on bosses
                Spell.BuffSelf("Fire Elemental Totem",
                    ret => (StyxWoW.Me.CurrentTarget.Elite || Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 3) && 
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                Spell.BuffSelf("Magma Totem",
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10*10 && u.IsTargetingMeOrPet) >= 3 &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental || t.WoWTotem == WoWTotem.Magma)),
                Spell.BuffSelf("Searing Totem",
                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                           !StyxWoW.Me.Totems.Any(
                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental || t.WoWTotem == WoWTotem.Magma)),

                // Low level stuff first.
                new Decorator(
                    ret => StyxWoW.Me.Level < 20,
                    new PrioritySelector(
                        Spell.Cast("Lava Lash"),
                        Spell.Cast("Primal Strike"),
                        Spell.Cast("Earth Shock"),
                        Spell.Cast("Lightning Bolt"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                Spell.Cast("Stormstrike"),
                Spell.Cast("Primal Strike", ret => !SpellManager.HasSpell("Stormstrike")),
                Spell.Buff("Flame Shock", true,
                    ret => (StyxWoW.Me.HasAura("Unleash Wind") || !SpellManager.HasSpell("Unleash Elements")) &&
                           (StyxWoW.Me.CurrentTarget.Elite ||
                           Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10 && u.IsTargetingMeOrPet) >= 3 && SpellManager.HasSpell("Fire Nova"))),
                Spell.Cast("Earth Shock",
                    ret => StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Flame Shock", true).TotalSeconds > 6 || !StyxWoW.Me.CurrentTarget.Elite),
                Spell.Cast("Lava Lash",
                    ret => StyxWoW.Me.Inventory.Equipped.OffHand != null &&
                           StyxWoW.Me.Inventory.Equipped.OffHand.ItemInfo.ItemClass == WoWItemClass.Weapon),
                Spell.BuffSelf("Fire Nova",
                    ret => StyxWoW.Me.CurrentTarget.HasMyAura("Flame Shock") &&
                           Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10 && u.IsTargetingMeOrPet) >= 3),
                Spell.Cast("Chain Lightning", ret => StyxWoW.Me.HasAura("Maelstrom Weapon", 5) && Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 2),
                Spell.Cast("Lightning Bolt", ret => StyxWoW.Me.HasAura("Maelstrom Weapon", 5)),
                Spell.Cast("Unleash Elements"),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateEnhancementShamanPvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Totems.CreateSetTotems(),
                Common.CreateAutoAttack(true),
                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.BuffSelf("Lightning Shield"),
                Spell.BuffSelf("Spiritwalker's Grace", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat),
                Spell.BuffSelf("Feral Spirit"),

                // Totem stuff
                // Pop the ele on bosses
                Spell.BuffSelf("Fire Elemental Totem", 
                    ret => StyxWoW.Me.HealthPercent >= 80 && StyxWoW.Me.CurrentTarget.DistanceSqr < 20*20 && 
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                Spell.BuffSelf("Searing Totem",
                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                           !StyxWoW.Me.Totems.Any(
                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),

                Spell.Cast("Stormstrike"),
                Spell.Cast("Primal Strike", ret => !SpellManager.HasSpell("Stormstrike")),
                Spell.Cast("Lava Lash", 
                    ret => StyxWoW.Me.Inventory.Equipped.OffHand != null && 
                           StyxWoW.Me.Inventory.Equipped.OffHand.ItemInfo.ItemClass == WoWItemClass.Weapon),
                Spell.Cast("Chain Lightning", ret => StyxWoW.Me.HasAura("Maelstrom Weapon", 5) && Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 2),
                Spell.Cast("Lightning Bolt", ret => StyxWoW.Me.HasAura("Maelstrom Weapon", 5)),
                Spell.Cast("Unleash Elements"),
                Spell.Buff("Flame Shock", true, ret => StyxWoW.Me.HasAura("Unleash Wind") || !SpellManager.HasSpell("Unleash Elements")),
                Spell.Cast("Earth Shock", ret => StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Flame Shock", true).TotalSeconds > 6),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.EnhancementShaman)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateEnhancementShamanInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Totems.CreateSetTotems(),
                Common.CreateAutoAttack(true),
                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.BuffSelf("Lightning Shield"),
                Spell.BuffSelf("Spiritwalker's Grace", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat),
                Spell.BuffSelf("Feral Spirit"),
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        Spell.Cast("Unleash Elements"),
                        Spell.BuffSelf("Magma Totem", 
                            ret => !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.Magma)),
                        Spell.Buff("Flame Shock", true),
                        Spell.Cast("Lava Lash", 
                            ret => StyxWoW.Me.CurrentTarget.HasMyAura("Flame Shock") &&
                                   StyxWoW.Me.Inventory.Equipped.OffHand != null && 
                                   StyxWoW.Me.Inventory.Equipped.OffHand.ItemInfo.ItemClass == WoWItemClass.Weapon),
                        Spell.Cast("Fire Nova"),
                        Spell.Cast("Chain Lightning", ret => StyxWoW.Me.HasAura("Maelstrom Weapon", 5)),
                        Spell.Cast("Stormstrike"),
                        Movement.CreateMoveToMeleeBehavior(true)
                        )),

                // Totem stuff
                // Pop the ele on bosses
                Spell.BuffSelf("Fire Elemental Totem", 
                    ret => StyxWoW.Me.CurrentTarget.IsBoss() && StyxWoW.Me.CurrentTarget.DistanceSqr < 20*20 &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                Spell.BuffSelf("Searing Totem",
                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                           !StyxWoW.Me.Totems.Any(
                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),

                Spell.Cast("Stormstrike"),
                Spell.Cast("Primal Strike", ret => !SpellManager.HasSpell("Stormstrike")),
                Spell.Cast("Lava Lash",
                    ret => StyxWoW.Me.Inventory.Equipped.OffHand != null && 
                           StyxWoW.Me.Inventory.Equipped.OffHand.ItemInfo.ItemClass == WoWItemClass.Weapon),
                Spell.Cast("Lightning Bolt", ret => StyxWoW.Me.HasAura("Maelstrom Weapon", 5)),
                Spell.Cast("Unleash Elements"),
                Spell.Buff("Flame Shock", true, ret => StyxWoW.Me.HasAura("Unleash Wind") || !SpellManager.HasSpell("Unleash Elements")),
                Spell.Cast("Earth Shock", ret => StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Flame Shock", true).TotalSeconds > 6),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

    }
}
