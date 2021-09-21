using System;
using System.Linq;
using CommonBehaviors.Actions;
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

namespace Singular.ClassSpecific.Priest
{
    public class Shadow
    {
        #region Normal Rotation

        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.ShadowPriest)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateShadowPriestNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),

                Spell.BuffSelf("Power Word: Shield", 
                    ret => SingularSettings.Instance.Priest.UseShieldPrePull && !StyxWoW.Me.HasAura("Weakened Soul") && !SpellManager.HasSpell("Mind Spike")),
                Spell.Cast("Holy Fire", ctx => StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Shadow)),
                Spell.Cast("Smite", ctx => StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Shadow)),
                Spell.Buff("Devouring Plague", true, 
                    ret => SingularSettings.Instance.Priest.DevouringPlagueFirst),
                Spell.Buff("Vampiric Touch", true, ret => !SpellManager.HasSpell("Mind Spike") || StyxWoW.Me.CurrentTarget.Elite),
                Spell.Cast("Mind Blast"),
                Spell.Cast("Smite", ret => !SpellManager.HasSpell("Mind Blast")),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.ShadowPriest)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateShadowPriestNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                Spell.BuffSelf("Shadow Form"),

                // Defensive stuff
                Spell.BuffSelf("Power Word: Shield", 
                    ret => !StyxWoW.Me.HasAura("Weakened Soul") &&
                           (!SpellManager.HasSpell("Mind Spike") || StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Priest.ShieldHealthPercent)),
                Spell.BuffSelf("Dispersion", ret => StyxWoW.Me.ManaPercent < SingularSettings.Instance.Priest.DispersionMana),
                Spell.BuffSelf("Psychic Scream", 
                    ret => SingularSettings.Instance.Priest.UsePsychicScream &&
                           Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10) >= SingularSettings.Instance.Priest.PsychicScreamAddCount),
                
                Spell.Heal("Flash Heal", ret => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Priest.ShadowFlashHealHealth),
                // don't attempt to heal unless below a certain percentage health
                new Decorator(ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.Priest.DontHealPercent,
                    new PrioritySelector(
                        Spell.Heal("Flash Heal", ret => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent < 40)
                        )),
                // for NPCs immune to shadow damage.
                Spell.Cast("Holy Fire", ctx => StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Shadow)),
                Spell.Cast("Smite", ctx => StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Shadow)),

                // Before Mind Spike
                new Decorator(
                    ret => !SpellManager.HasSpell("Mind Spike") || StyxWoW.Me.CurrentTarget.Elite,
                    new PrioritySelector(
                        Spell.Cast("Shadow Word: Death", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 25),
                        // We don't want to dot targets below 40% hp to conserve mana. Mind Blast/Flay will kill them soon anyway
                        Spell.Buff("Shadow Word: Pain", true, ret => StyxWoW.Me.CurrentTarget.Elite || StyxWoW.Me.CurrentTarget.HealthPercent > 40),
                        Spell.Cast("Mind Blast", ret => StyxWoW.Me.HasAura("Shadow Orb")),
                        Spell.Buff("Vampiric Touch", true, ret => StyxWoW.Me.CurrentTarget.Elite || StyxWoW.Me.CurrentTarget.HealthPercent > 40),
                        Spell.Buff("Devouring Plague", true, ret => StyxWoW.Me.CurrentTarget.Elite || StyxWoW.Me.CurrentTarget.HealthPercent > 40),
                        Spell.Cast("Mind Blast"),
                        // Use archangel on adds or elite mobs to be safe
                        Spell.BuffSelf("Archangel", 
                            ret => StyxWoW.Me.CurrentTarget.Elite || Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet) >= 2),
                        Spell.Cast("Shadowfiend", 
                            ret => StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Priest.ShadowfiendMana &&
                                   StyxWoW.Me.CurrentTarget.HealthPercent >= 60),
                        // Mana check is for mana management. Don't mess with it
                        Spell.Cast("Mind Flay", ret => StyxWoW.Me.ManaPercent >= SingularSettings.Instance.Priest.MindFlayMana),
                        Helpers.Common.CreateUseWand(ret => SingularSettings.Instance.Priest.UseWand),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                // After Mind Spike
                Spell.Cast("Shadow Word: Death", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 25),
                Spell.Cast("Mind Blast", ret => StyxWoW.Me.HasAura("Mind Melt", 2)),
                Spell.Cast("Mind Spike"),
                Helpers.Common.CreateUseWand(ret => SingularSettings.Instance.Priest.UseWand),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.ShadowPriest)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateShadowPriestPvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Spell.BuffSelf("Shadow Form"),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Defensive stuff
                Spell.BuffSelf("Power Word: Shield", ret => !StyxWoW.Me.HasAura("Weakened Soul")),
                Spell.BuffSelf("Dispersion", ret => StyxWoW.Me.HealthPercent < 40),
                Spell.BuffSelf("Psychic Scream", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10*10) >= 1),

                // Offensive
                Spell.Buff("Shadow Word: Pain", true),
                Spell.Cast("Mind Blast", ret => StyxWoW.Me.HasAura("Shadow Orb")),
                Spell.Buff("Vampiric Touch", true),
                Spell.Buff("Devouring Plague", true),
                Spell.Cast("Mind Blast"),
                Spell.Cast("Shadow Word: Death", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 25),
                Spell.BuffSelf("Archangel"),
                Spell.Cast("Shadowfiend"),
                Spell.Cast("Mind Flay"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.ShadowPriest)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.Instances)]
        public static Composite CreateShadowPriestRest()
        {
            return new PrioritySelector(
                Spell.Resurrect("Resurrection"),
                Rest.CreateDefaultRestBehaviour()
                );
        }

        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.ShadowPriest)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateShadowPriestInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                Spell.BuffSelf("Shadow Form"),

                // use fade to drop aggro.
                Spell.Cast("Fade", ret => (StyxWoW.Me.IsInParty || StyxWoW.Me.IsInRaid) && Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0),

                // Shadow immune npcs.
                Spell.Cast("Holy Fire", ctx => StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Shadow)),
                Spell.Cast("Smite", ctx => StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Shadow)),

                // AoE Rotation
                new PrioritySelector(
                    ret => Group.Tanks.FirstOrDefault(t => 
                                Clusters.GetClusterCount(t, Unit.NearbyUnfriendlyUnits,ClusterType.Radius, 10f) >= 3),
                    new Decorator(
                        ret => ret != null,
                        Spell.Cast("Mind Sear", ret => (WoWUnit)ret))),
                        
                // In case of a guild raid
                new Decorator(
                    ret => !Group.Tanks.Any() && Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    Spell.Cast("Mind Sear")),



                // Single target boss rotation
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsBoss(),
                    new PrioritySelector(
                        Spell.Buff("Shadow Word: Pain", true),
                        Spell.Cast("Mind Blast", ret => StyxWoW.Me.HasAura("Shadow Orb")),
                        Spell.Buff("Vampiric Touch", true),
                        Spell.Buff("Devouring Plague", true),
                        Spell.Cast("Mind Blast"),
                        Spell.Cast("Shadow Word: Death", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 25),
                        Spell.BuffSelf("Archangel"),
                        Spell.Cast("Shadowfiend"),
                        Spell.Cast("Mind Flay"),
                        Movement.CreateMoveToTargetBehavior(true, 35f)
                        )),

                // Single target trash rotation
                Spell.Cast("Mind Blast", ret => StyxWoW.Me.HasAura("Mind Melt", 2)),
                Spell.Cast("Shadow Word: Death", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 25),
                Spell.Cast("Mind Spike"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}
