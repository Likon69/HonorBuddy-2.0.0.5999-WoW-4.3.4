using System;
using System.Linq;
using CommonBehaviors.Actions;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Singular.ClassSpecific.Mage
{
    public class Arcane
    {
        #region Normal Rotation

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.ArcaneMage)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateArcaneMageNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Common.CreateStayAwayFromFrozenTargetsBehavior(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                Spell.Cast("Arcane Blast"),
                Spell.Cast("Fireball", ret => !SpellManager.HasSpell("Arcane Blast")),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.ArcaneMage)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateArcaneMageNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Common.CreateStayAwayFromFrozenTargetsBehavior(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                // Defensive stuff
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Ice Block"),
                    new ActionIdle()),
                Spell.BuffSelf("Ice Block", ret => StyxWoW.Me.HealthPercent < 10 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia")),
                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.HealthPercent <= 75),
                Spell.BuffSelf("Frost Nova", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.DistanceSqr <= 8 * 8 && !u.HasAura("Frost Nova"))),
                Common.CreateMagePolymorphOnAddBehavior(),
                
                // Burn mana phase (Only on multiple mobs while grinding. No need to dump mana off)
                new Decorator(
                    ret => StyxWoW.Me.ManaPercent > 20 && Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 2,
                    new PrioritySelector(
                        Common.CreateUseManaGemBehavior(ret => StyxWoW.Me.ManaPercent < 80),

                        Spell.BuffSelf("Arcane Power"),
                        Spell.BuffSelf("Mirror Image"),
                        Spell.BuffSelf("Flame Orb"),
                        Spell.Cast("Arcane Blast"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                // Reserve mana phase

                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.ManaPercent < 30),
                Spell.BuffSelf("Evocation", ret => StyxWoW.Me.ManaPercent < 30 && (StyxWoW.Me.HasAura("Mana Shield") || !SpellManager.HasSpell("Mana Shield"))),

                Spell.BuffSelf("Flame Orb"),
                Spell.Cast("Arcane Missiles", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Arcane Missiles!") && StyxWoW.Me.HasAura("Arcane Blast", 3)),
                Spell.Cast("Arcane Barrage", ret => StyxWoW.Me.HasAura("Arcane Blast", 3)),

                Spell.Cast("Arcane Blast"),

                // These 2 are just for support for some DPS until we get arcane blast.
                Spell.Cast("Arcane Barrage", ret => !SpellManager.HasSpell("Arcane Blast")),
                Spell.Cast("Fireball", ret => !SpellManager.HasSpell("Arcane Blast")),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.ArcaneMage)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateArcaneMagePvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Common.CreateStayAwayFromFrozenTargetsBehavior(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                // Defensive stuff
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Ice Block"),
                    new ActionIdle()),
                Spell.BuffSelf("Ice Block", ret => StyxWoW.Me.HealthPercent < 10 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia")),
                Spell.BuffSelf("Blink", ret => StyxWoW.Me.IsStunned()),
                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.HealthPercent <= 75),
                Spell.BuffSelf("Frost Nova", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.DistanceSqr <= 8 * 8 && !u.HasAura("Frost Nova") && !u.Stunned)),
                
                // Burn mana phase (Only on multiple mobs while grinding. No need to dump mana off)
                new Decorator(
                    ret => StyxWoW.Me.ManaPercent > 20,
                    new PrioritySelector(
                        Common.CreateUseManaGemBehavior(ret => StyxWoW.Me.ManaPercent < 80),
                        Spell.BuffSelf("Arcane Power"),
                        Spell.BuffSelf("Mirror Image"),
                        Spell.BuffSelf("Flame Orb"),
                        Spell.Cast("Arcane Blast"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                // Reserve mana phase

                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.ManaPercent < 30),
                Spell.BuffSelf("Evocation", ret => StyxWoW.Me.ManaPercent < 30 && (StyxWoW.Me.HasAura("Mana Shield") || !SpellManager.HasSpell("Mana Shield"))),

                Spell.BuffSelf("Flame Orb"),
                Spell.Cast("Arcane Missiles", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Arcane Missiles!") && StyxWoW.Me.HasAura("Arcane Blast", 3)),
                Spell.Cast("Arcane Barrage", ret => StyxWoW.Me.HasAura("Arcane Blast", 3)),

                Spell.Cast("Arcane Blast"),

                // These 2 are just for support for some DPS until we get arcane blast.
                Spell.Cast("Arcane Barrage", ret => !SpellManager.HasSpell("Arcane Blast")),
                Spell.Cast("Fireball", ret => !SpellManager.HasSpell("Arcane Blast")),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation
        
        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.ArcaneMage)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateArcaneMageInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                // Defensive stuff
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Ice Block"),
                    new ActionIdle()),
                Spell.BuffSelf("Ice Block", ret => StyxWoW.Me.HealthPercent < 10 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia")),
                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.HealthPercent <= 75),
                Spell.Cast("Focus Magic", 
                    ret => StyxWoW.Me.RaidMemberInfos.
                                        Where(m => m.HasRole(WoWPartyMember.GroupRole.Damage) && m.ToPlayer() != null).
                                        Select(m => m.ToPlayer()).
                                        FirstOrDefault(),
                    ret => !StyxWoW.Me.RaidMemberInfos.
                                        Any(m => m.ToPlayer() != null && m.ToPlayer().HasMyAura("Focus Magic"))),
                // AoE comes first
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        Spell.CastOnGround("Flamestrike",
                            ret => Clusters.GetBestUnitForCluster(StyxWoW.Me.Combat ? Unit.NearbyUnitsInCombatWithMe : Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 8f).Location,
                            ret => !ObjectManager.GetObjectsOfType<WoWDynamicObject>().Any(o => 
                                        o.CasterGuid == StyxWoW.Me.Guid && o.Spell.Name == "Flamestrike" &&
                                        o.Location.Distance(
                                            Clusters.GetBestUnitForCluster(StyxWoW.Me.Combat ? Unit.NearbyUnitsInCombatWithMe : Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 8f).Location) < o.Radius)),
                        Spell.Cast("Arcane Explosion",
                            ret => StyxWoW.Me.HasAura("Arcane Blast", 3) &&
                                   Clusters.GetClusterCount(StyxWoW.Me,
                                                            Unit.NearbyUnfriendlyUnits,
                                                            ClusterType.Radius,
                                                            10f) >= 3),
                        Spell.CastOnGround("Blizzard",
                            ret => Clusters.GetBestUnitForCluster(StyxWoW.Me.Combat ? Unit.NearbyUnitsInCombatWithMe : Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 8f).Location,
                            ret => StyxWoW.Me.CurrentTarget.DistanceSqr > 10* 10),

                        Spell.Cast("Arcane Blast"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                Spell.BuffSelf("Time Warp", 
                    ret => !StyxWoW.Me.IsInRaid && StyxWoW.Me.CurrentTarget.HealthPercent > 20 && StyxWoW.Me.CurrentTarget.IsBoss() &&
                           !StyxWoW.Me.HasAura("Temporal Displacement")),

                // Burn mana phase
                new Decorator(
                    ret => SpellManager.HasSpell("Evocation") && SpellManager.Spells["Evocation"].CooldownTimeLeft.TotalSeconds < 30 &&
                           StyxWoW.Me.ManaPercent > 10,
                    new PrioritySelector(
                        Common.CreateUseManaGemBehavior(ret => StyxWoW.Me.ManaPercent < 80),

                        Spell.BuffSelf("Arcane Power"),
                        Spell.BuffSelf("Mirror Image"),
                        Spell.BuffSelf("Flame Orb"),
                        Spell.Cast("Arcane Blast"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                // Reserve mana phase

                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.ManaPercent < 30),
                Spell.BuffSelf("Evocation", ret => StyxWoW.Me.ManaPercent < 30 && (StyxWoW.Me.HasAura("Mana Shield") || !SpellManager.HasSpell("Mana Shield"))),

                Spell.BuffSelf("Flame Orb"),
                Spell.Cast("Arcane Missiles", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Arcane Missiles!") && StyxWoW.Me.HasAura("Arcane Blast", 3)),
                Spell.Cast("Arcane Barrage", ret => StyxWoW.Me.HasAura("Arcane Blast", 3)),

                Spell.Cast("Arcane Blast"),

                // These 2 are just for support for some DPS until we get arcane blast.
                Spell.Cast("Arcane Barrage", ret => !SpellManager.HasSpell("Arcane Blast")),
                Spell.Cast("Fireball", ret => !SpellManager.HasSpell("Arcane Blast")),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}
