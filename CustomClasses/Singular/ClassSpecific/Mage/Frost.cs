using System.Linq;
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
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Singular.ClassSpecific.Mage
{
    public class Frost
    {
        #region Normal Rotation

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.FrostMage)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateFrostMageNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Common.CreateStayAwayFromFrozenTargetsBehavior(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),
                new Decorator(ctx => SingularSettings.Instance.DisablePetUsage && StyxWoW.Me.GotAlivePet,
                    new Action(ctx => Lua.DoString("PetDismiss()"))),
                // We want our pet alive !
                new Decorator(
                    ret => !SingularSettings.Instance.DisablePetUsage && !StyxWoW.Me.GotAlivePet && PetManager.PetTimer.IsFinished && SpellManager.CanCast("Summon Water Elemental"),
                    new Sequence(
                        new Action(ret => PetManager.CallPet("Summon Water Elemental")),
                        Helpers.Common.CreateWaitForLagDuration())),
                Spell.Cast("Frostbolt", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                Spell.Cast("Frostfire Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.FrostMage)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateFrostMageNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Common.CreateStayAwayFromFrozenTargetsBehavior(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                // We want our pet alive !
                new Decorator(
                    ret => !StyxWoW.Me.GotAlivePet && PetManager.PetTimer.IsFinished && SpellManager.CanCast("Summon Water Elemental"),
                    new Sequence(
                        new Action(ret => PetManager.CallPet("Summon Water Elemental")),
                        Helpers.Common.CreateWaitForLagDuration())),

                // Defensive stuff
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Ice Block"),
                    new ActionIdle()),
                Spell.BuffSelf("Ice Block", ret => StyxWoW.Me.HealthPercent < 20 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia")),

                // Cooldowns
                Spell.BuffSelf("Evocation",
                    ret => StyxWoW.Me.ManaPercent < 30 || (TalentManager.HasGlyph("Evocation") && StyxWoW.Me.HealthPercent < 50)),
                Spell.BuffSelf("Mage Ward", ret => StyxWoW.Me.HealthPercent <= 80),
                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.HealthPercent <= 60),

                new Decorator(
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 3,
                    new PrioritySelector(
                        Spell.BuffSelf("Mirror Image"),
                        Spell.BuffSelf("Icy Veins")
                        )),
                Common.CreateUseManaGemBehavior(ret => StyxWoW.Me.ManaPercent < 80),

                new Decorator(
                    ret => !Unit.NearbyUnfriendlyUnits.Any(u => u.DistanceSqr < 10 * 10 && u.IsCrowdControlled()),
                    new PrioritySelector(
                        Pet.CreateCastPetActionOnLocation("Freeze", ret => !StyxWoW.Me.Mounted && !StyxWoW.Me.CurrentTarget.HasAura("Frost Nova") && StyxWoW.Me.GotAlivePet && StyxWoW.Me.Pet.ManaPercent >= 12),
                        Spell.BuffSelf("Frost Nova", 
                            ret => Unit.NearbyUnfriendlyUnits.Any(u => 
                                            u.DistanceSqr <= 8 * 8 && !u.HasAura("Freeze") && 
                                            !u.HasAura("Frost Nova") && !u.Stunned))
                        )),
                
                Common.CreateMagePolymorphOnAddBehavior(),
                // Rotation
                Spell.Cast("Deep Freeze", 
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Fingers of Frost") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Freeze") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Frost Nova")),
                Spell.BuffSelf("Flame Orb"),
                Spell.Cast("Arcane Missiles", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Arcane Missiles!")),
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Brain Freeze"),
                    new PrioritySelector(
                        Spell.Cast("Frostfire Bolt"),
                        Spell.Cast("Fireball")
                        )),
                Spell.Cast("Ice Lance",
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Fingers of Frost") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Freeze") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Frost Nova") ||
                           StyxWoW.Me.IsMoving),
                Spell.Cast("Frostbolt", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                Spell.Cast("Frostfire Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.FrostMage)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateFrostMagePvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Common.CreateStayAwayFromFrozenTargetsBehavior(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                // We want our pet alive !
                new Decorator(
                    ret => !StyxWoW.Me.GotAlivePet && PetManager.PetTimer.IsFinished && SpellManager.CanCast("Summon Water Elemental"),
                    new Sequence(
                        new Action(ret => PetManager.CallPet("Summon Water Elemental")),
                        Helpers.Common.CreateWaitForLagDuration())),

                // Defensive stuff
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Ice Block"),
                    new ActionIdle()),
                Spell.BuffSelf("Ice Block", ret => StyxWoW.Me.HealthPercent < 10 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia")),
                Spell.BuffSelf("Blink", ret => StyxWoW.Me.IsStunned() || StyxWoW.Me.IsRooted()),
                Spell.BuffSelf("Mana Shield", ret => StyxWoW.Me.HealthPercent <= 75),
                Pet.CreateCastPetActionOnLocation("Freeze", ret => !StyxWoW.Me.Mounted && !StyxWoW.Me.CurrentTarget.HasAura("Frost Nova") && StyxWoW.Me.GotAlivePet && StyxWoW.Me.Pet.ManaPercent >= 12),
                Spell.BuffSelf("Frost Nova", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.DistanceSqr <= 8 * 8 && !u.HasAura("Freeze") && !u.HasAura("Frost Nova") && !u.Stunned)),

                Common.CreateUseManaGemBehavior(ret => StyxWoW.Me.ManaPercent < 80),
                // Cooldowns
                Spell.BuffSelf("Evocation", ret => StyxWoW.Me.ManaPercent < 30),
                Spell.BuffSelf("Mirror Image"),
                Spell.BuffSelf("Mage Ward", ret => StyxWoW.Me.HealthPercent <= 75),
                Spell.BuffSelf("Icy Veins"),

                // Rotation
                Spell.Cast("Deep Freeze",
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Fingers of Frost") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Freeze") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Frost Nova")),
                Spell.BuffSelf("Flame Orb"),
                Spell.Cast("Arcane Missiles", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Arcane Missiles!")),
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Brain Freeze"),
                    new PrioritySelector(
                        Spell.Cast("Frostfire Bolt"),
                        Spell.Cast("Fireball")
                        )),
                Spell.Cast("Ice Lance", 
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Fingers of Frost") || 
                           StyxWoW.Me.CurrentTarget.HasAura("Freeze") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Frost Nova") || 
                           StyxWoW.Me.IsMoving),
                Spell.Cast("Frostbolt"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Mage)]
        [Spec(TalentSpec.FrostMage)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateFrostMageInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Spell.WaitForCast(true),

                // We want our pet alive !
                new Decorator(
                    ret => !StyxWoW.Me.GotAlivePet && PetManager.PetTimer.IsFinished && SpellManager.CanCast("Summon Water Elemental"),
                    new Sequence(
                        new Action(ret => PetManager.CallPet("Summon Water Elemental")),
                        Helpers.Common.CreateWaitForLagDuration())),

                // Defensive stuff
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Ice Block"),
                    new ActionIdle()),
                Spell.BuffSelf("Ice Block", ret => StyxWoW.Me.HealthPercent < 20 && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia")),

                // Cooldowns
                Spell.BuffSelf("Evocation", ret => StyxWoW.Me.ManaPercent < 30),
                Spell.BuffSelf("Mirror Image"),
                Spell.BuffSelf("Mage Ward", ret => StyxWoW.Me.HealthPercent <= 75),
                Spell.BuffSelf("Icy Veins"),

                Common.CreateUseManaGemBehavior(ret => StyxWoW.Me.ManaPercent < 80),
                // AoE comes first
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        Spell.CastOnGround("Flamestrike", 
                            ret => Clusters.GetBestUnitForCluster(Unit.NearbyUnitsInCombatWithMe, ClusterType.Radius, 8f).Location,
                            ret => !ObjectManager.GetObjectsOfType<WoWDynamicObject>().Any(o => 
                                        o.CasterGuid == StyxWoW.Me.Guid && o.Spell.Name == "Flamestrike" &&
                                        o.Location.Distance(
                                            Clusters.GetBestUnitForCluster(Unit.NearbyUnitsInCombatWithMe, ClusterType.Radius, 8f).Location) < o.Radius)),
                        Spell.Cast("Cone of Cold", 
                            ret => Clusters.GetClusterCount(StyxWoW.Me.CurrentTarget, 
                                                            Unit.NearbyUnfriendlyUnits,
                                                            ClusterType.Cone, 15f) >= 3),
                        Spell.CastOnGround("Blizzard",
                            ret => StyxWoW.Me.CurrentTarget.Location),
                        Spell.Cast("Arcane Explosion",
                            ret => Clusters.GetClusterCount(StyxWoW.Me,
                                                            Unit.NearbyUnfriendlyUnits,
                                                            ClusterType.Radius,
                                                            10f) >= 3),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                Spell.BuffSelf("Time Warp",
                    ret => !StyxWoW.Me.IsInRaid && StyxWoW.Me.CurrentTarget.HealthPercent > 20 && StyxWoW.Me.CurrentTarget.IsBoss() &&
                           !StyxWoW.Me.HasAura("Temporal Displacement")),

                // Rotation
                Spell.Cast("Deep Freeze",
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Fingers of Frost") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Freeze") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Frost Nova")),
                Spell.BuffSelf("Flame Orb"),
                Pet.CreateCastPetActionOnLocation("Freeze"),
                Spell.Cast("Arcane Missiles", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Arcane Missiles!")),
                new Decorator(
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Brain Freeze"),
                    new PrioritySelector(
                        Spell.Cast("Frostfire Bolt"),
                        Spell.Cast("Fireball")
                        )),
                Spell.Cast("Ice Lance",
                    ret => StyxWoW.Me.ActiveAuras.ContainsKey("Fingers of Frost") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Freeze") ||
                           StyxWoW.Me.CurrentTarget.HasAura("Frost Nova") ||
                           StyxWoW.Me.IsMoving),
                Spell.Cast("Frostbolt", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                Spell.Cast("Frostfire Bolt"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}
