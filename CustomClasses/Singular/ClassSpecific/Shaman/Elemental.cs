using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace Singular.ClassSpecific.Shaman
{
    public class Elemental
    {
        #region Common

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.All)]
        public static Composite CreateElementalShamanPreCombatBuffs()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != 283 && StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole && SpellManager.HasSpell("Flametongue Weapon") &&
                            SpellManager.CanCast("Flametongue Weapon", null, false, false),
                    new Sequence(
                        new Action(ret => Lua.DoString("CancelItemTempEnchantment(1)")),
                        new Action(ret => Logger.Write("Imbuing main hand weapon with Flametongue")),
                        new Action(ret => SpellManager.Cast("Flametongue Weapon", null))
                        )),

                Spell.Cast("Lightning Shield", ret => StyxWoW.Me, ret => !StyxWoW.Me.HasAura("Lightning Shield", 2)),
                new Decorator(ret => Totems.NeedToRecallTotems,
                    new Action(ret => Totems.RecallTotems()))
                );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateElementalShamanRest()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => !StyxWoW.Me.HasAura("Drink") && !StyxWoW.Me.HasAura("Food"),
                        CreateElementalShamanHeal()),
                    Rest.CreateDefaultRestBehaviour(),
                    Spell.Resurrect("Ancestral Spirit")
                    );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.Instances)]
        [Context(WoWContext.Normal)]
        public static Composite CreateElementalShamanHeal()
        {
            return
                new Decorator(
                    ret => SingularSettings.Instance.Shaman.EnhancementHeal,
                    new PrioritySelector(
                        // Heal the party in dungeons if the healer is dead
                        new Decorator(
                            ret => StyxWoW.Me.CurrentMap.IsDungeon && !StyxWoW.Me.IsInRaid &&
                                   Group.Healers.Count(h => h.IsAlive) == 0,
                            Restoration.CreateRestoShamanHealingOnlyBehavior()),

                        // This will work for both solo play and battlegrounds
                        new Decorator(
                            ret => Group.Healers.Any() && Group.Healers.Count(h => h.IsAlive) == 0,
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
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateElementalShamanNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Spell.Cast("Lightning Shield", ret => StyxWoW.Me, ret => !StyxWoW.Me.HasAura("Lightning Shield", 2)),

                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.DistanceSqr < 40 * 40,
                    Totems.CreateSetTotems()),
                Spell.Cast("Lightning Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateElementalShamanNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Totems.CreateSetTotems(),
                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                Spell.BuffSelf("Lightning Shield"),

                Spell.BuffSelf("Spiritwalker's Grace", ret => StyxWoW.Me.IsMoving),

                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        Spell.BuffSelf("Elemental Mastery"),
                        Spell.BuffSelf("Thunderstorm"),
                        Spell.CastOnGround("Earthquake", ret => StyxWoW.Me.CurrentTarget.Location),
                        Spell.Cast("Chain Lightning", ret => Clusters.GetBestUnitForCluster(Unit.UnfriendlyUnitsNearTarget(15f), ClusterType.Chained, 12))
                        )),

                // Totem stuff
                // Pop the ele on bosses
                Spell.BuffSelf("Fire Elemental Totem", ret => StyxWoW.Me.CurrentTarget.Elite && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                Spell.BuffSelf("Searing Totem",
                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                           !StyxWoW.Me.Totems.Any(
                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),

                Spell.Buff("Flame Shock", true),
                Spell.Cast("Lava Burst"),
                Spell.Cast("Earth Shock",
                    ret => (StyxWoW.Me.HasAura("Lightning Shield", 7) || TalentManager.GetCount(1, 13) == 0) &&
                           StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Flame Shock", true).TotalSeconds > 6),
                Spell.Cast("Unleash Elements",
                    ret => (StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 8024) && StyxWoW.Me.IsMoving && !StyxWoW.Me.HasAura("Spiritwalker's Grace")),
                Spell.Cast("Chain Lightning", ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 2),
                Spell.Cast("Lightning Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateElementalShamanPvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Totems.CreateSetTotems(),
                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.BuffSelf("Lightning Shield"),

                Spell.BuffSelf("Spiritwalker's Grace", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat),

                Spell.BuffSelf("Elemental Mastery",
                    ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat &&
                           (!SpellManager.HasSpell("Spiritwalker's Grace") ||
                           SpellManager.Spells["Spiritwalker's Grace"].Cooldown && !StyxWoW.Me.HasAura("Spiritwalker's Grace"))),
                Spell.BuffSelf("Thunderstorm", ret => StyxWoW.Me.IsStunned()),
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        // Pop the ele on bosses
                        Spell.BuffSelf("Fire Elemental Totem", ret => !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                        Spell.CastOnGround("Earthquake", ret => StyxWoW.Me.CurrentTarget.Location),
                        Spell.Cast("Chain Lightning", ret => Clusters.GetBestUnitForCluster(Unit.UnfriendlyUnitsNearTarget(15f), ClusterType.Chained, 12))
                        )),

                // Totem stuff
                Spell.BuffSelf("Searing Totem",
                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                           !StyxWoW.Me.Totems.Any(
                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),

                Spell.Buff("Flame Shock", true),
                Spell.Cast("Lava Burst"),
                Spell.Cast("Earth Shock",
                    ret => (StyxWoW.Me.HasAura("Lightning Shield", 7) || TalentManager.GetCount(1, 13) == 0) &&
                           StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Flame Shock", true).TotalSeconds > 6),
                Spell.Cast("Unleash Elements",
                    ret => (StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 8024) && StyxWoW.Me.IsMoving && !StyxWoW.Me.HasAura("Spiritwalker's Grace")),
                Spell.Cast("Chain Lightning", ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 2),
                Spell.Cast("Lightning Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Shaman)]
        [Spec(TalentSpec.ElementalShaman)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateElementalShamanInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Totems.CreateSetTotems(),
                Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.BuffSelf("Lightning Shield"),
                Spell.BuffSelf("Spiritwalker's Grace", ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat),
                Spell.BuffSelf("Elemental Mastery",
                    ret => StyxWoW.Me.HasAnyAura("Bloodlust", "Heroism", "Time Warp", "Ancient Hysteria")),
                Spell.BuffSelf("Elemental Mastery", 
                    ret => StyxWoW.Me.IsMoving && StyxWoW.Me.Combat &&
                           (!SpellManager.HasSpell("Spiritwalker's Grace") || 
                           SpellManager.Spells["Spiritwalker's Grace"].Cooldown && !StyxWoW.Me.HasAura("Spiritwalker's Grace"))),

                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        Spell.CastOnGround("Earthquake", ret => StyxWoW.Me.CurrentTarget.Location),
                        Spell.Cast("Chain Lightning", ret => Clusters.GetBestUnitForCluster(Unit.UnfriendlyUnitsNearTarget(15f), ClusterType.Chained, 12))
                        )),
                
                // Totem stuff
                // Pop the ele on bosses
                Spell.BuffSelf("Fire Elemental Totem", ret => StyxWoW.Me.CurrentTarget.IsBoss() && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),
                Spell.BuffSelf("Searing Totem",
                    ret => StyxWoW.Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f &&
                           !StyxWoW.Me.Totems.Any(
                                t => t.Unit != null && t.WoWTotem == WoWTotem.Searing &&
                                     t.Unit.Location.Distance(StyxWoW.Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) &&
                           !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental)),

                Spell.Buff("Flame Shock", true),
                Spell.Cast("Lava Burst"),
                Spell.Cast("Earth Shock", 
                    ret => (StyxWoW.Me.HasAura("Lightning Shield", 7) || TalentManager.GetCount(1,13) == 0) &&
                           StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Flame Shock", true).TotalSeconds > 6),
                Spell.Cast("Unleash Elements",
                    ret => (StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 8024) && StyxWoW.Me.IsMoving && !StyxWoW.Me.HasAura("Spiritwalker's Grace")),
                Spell.Cast("Chain Lightning", ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 2),
                Spell.Cast("Lightning Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}
