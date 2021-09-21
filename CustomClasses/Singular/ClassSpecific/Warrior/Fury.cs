

using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;

using Styx;
using Styx.Combat.CombatRoutine;

using TreeSharp;
using Styx.Logic.Combat;
using Styx.Helpers;
using System;
using Action = TreeSharp.Action;



namespace Singular.ClassSpecific.Warrior
{
    public class Fury
    {
        private static string[] _slows;

        #region Normal
        [Class(WoWClass.Warrior)]
        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.Normal)]
        public static Composite CreateFuryNormalPreCombatBuffs()
        {
            return
                new PrioritySelector(
                    Spell.BuffSelf("Battle Stance", ret => Styx.Logic.LootTargeting.Instance.FirstObject == null));
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.Pull)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Normal)]
        public static Composite CreateFuryNormalPull()
        {
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(),
                // LOS check
                Movement.CreateMoveToLosBehavior(),
                //face target
                Movement.CreateFaceTargetBehavior(),
                // Auto Attack
                Helpers.Common.CreateAutoAttack(false),

                // buff up
                Spell.BuffSelf("Battle Shout", ret => (SingularSettings.Instance.Warrior.UseWarriorShouts || SingularSettings.Instance.Warrior.UseWarriorT12) && !StyxWoW.Me.HasAnyAura("Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf("Commanding Shout", ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),

                //Shoot flying targets
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false,
                    new PrioritySelector(
                        Spell.WaitForCast(),
                        Spell.Cast("Heroic Throw"),
                        Spell.Cast("Throw", ret => StyxWoW.Me.CurrentTarget.IsFlying && Item.RangedIsType(WoWItemWeaponClass.Thrown)), Spell.Cast(
                            "Shoot",
                            ret =>
                            StyxWoW.Me.CurrentTarget.IsFlying &&
                            (Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun))),
                        Movement.CreateMoveToTargetBehavior(true, 27f)
                        )),

                //low level support
                new Decorator(
                    ret => StyxWoW.Me.Level < 50,
                    new PrioritySelector(
                        Spell.BuffSelf("Battle Stance"),
                        Spell.Cast("Charge", ret => StyxWoW.Me.CurrentTarget.Distance > 12 && StyxWoW.Me.CurrentTarget.Distance <= 25),
                        Spell.Cast("Heroic Throw", ret => !StyxWoW.Me.CurrentTarget.HasAura("Charge Stun")),
                        Movement.CreateMoveToTargetBehavior(true, 5f))),

                // Heroic fury
                new Decorator(
                    ret => HasSpellIntercept(),
                    new PrioritySelector(
                        Spell.BuffSelf(
                            "Heroic Fury",
                            ret => SpellManager.Spells["Intercept"].Cooldown && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false))),

                // Get closer to target
                Spell.Cast("Charge", ret => PreventDoubleIntercept && StyxWoW.Me.CurrentTarget.Distance.Between(8f, TalentManager.HasGlyph("Long Charge") ? 30f : 25f) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),
                Spell.Cast("Intercept", ret => PreventDoubleIntercept && StyxWoW.Me.CurrentTarget.Distance.Between(8f, 25f) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance <= 40 && PreventDoubleIntercept &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),

                // Move to Melee
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Normal)]
        public static Composite CreateFuryNormalCombatBuffs()
        {
            return new PrioritySelector(
                //Heal
                Spell.Buff("Enraged Regeneration", ret => StyxWoW.Me.HealthPercent < 60 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                //Recklessness if low on hp or have Deathwish up or as gank protection
                Spell.BuffSelf("Recklessness", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // Heroic Fury
                Spell.BuffSelf("Heroic Fury", ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Rooted) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // Fear Remover
                Spell.BuffSelf("Berserker Rage", ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Fleeing, WoWSpellMechanic.Sapped, WoWSpellMechanic.Incapacitated, WoWSpellMechanic.Horrified)),
                //Deathwish
                Spell.BuffSelf("Death Wish", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                //Berserker rage to stay enraged
                Spell.BuffSelf("Berserker Rage", ret => !StyxWoW.Me.ActiveAuras.ContainsKey("Enrage") && !StyxWoW.Me.ActiveAuras.ContainsKey("Berserker Rage") && !StyxWoW.Me.ActiveAuras.ContainsKey("Death Wish")),  //!StyxWoW.Me.HasAnyAura("Enrage", "Berserker Rage", "Death Wish")),
                //Battleshout Check
                Spell.BuffSelf("Battle Shout", ret => (SingularSettings.Instance.Warrior.UseWarriorShouts || SingularSettings.Instance.Warrior.UseWarriorT12) && !StyxWoW.Me.HasAnyAura("Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf("Commanding Shout", ret => (StyxWoW.Me.RagePercent < 20 || SingularSettings.Instance.Warrior.UseWarriorT12) && SingularSettings.Instance.Warrior.UseWarriorShouts == false)
                );
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.Combat)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Normal)]
        public static Composite CreateFuryNormalCombat()
        {
            _slows = new[] { "Hamstring", "Piercing Howl", "Crippling Poison", "Hand of Freedom", "Infected Wounds" };
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(),
                // LOS check
                Movement.CreateMoveToLosBehavior(),
                // Face Target
                Movement.CreateFaceTargetBehavior(),
                // Auto Attack
                Helpers.Common.CreateAutoAttack(false),

                // Low level support
                new Decorator(ret => StyxWoW.Me.Level < 30,
                    new PrioritySelector(
                        Movement.CreateMoveBehindTargetBehavior(),
                        Spell.Cast("Victory Rush"),
                        Spell.Cast("Execute"),
                        Spell.Buff("Rend"),
                        Spell.Cast("Overpower"),
                        Spell.Cast("Bloodthirst"),
                //rage dump
                        Spell.Cast("Thunder Clap", ret => StyxWoW.Me.RagePercent > 50 && Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 6f) > 3),
                        Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent > 60),
                        Movement.CreateMoveToMeleeBehavior(true))),
                //30-50 support
                Spell.BuffSelf("Berserker Stance", ret => StyxWoW.Me.Level > 30 && SingularSettings.Instance.Warrior.UseWarriorKeepStance),

                // Dispel Bubbles
                new Decorator(ret => StyxWoW.Me.CurrentTarget.IsPlayer && (StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Ice Block") || StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Hand of Protection") || StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Divine Shield")) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false,
                    new PrioritySelector(
                        Spell.BuffSelf("Battle Stance"),
                        Spell.WaitForCast(),
                        Movement.CreateEnsureMovementStoppedBehavior(),
                        Spell.Cast("Shattering Throw"),
                        Spell.BuffSelf("Berserker Stance"),
                        Movement.CreateMoveToTargetBehavior(true, 30f)
                        )),

                // Intercept
                Spell.Cast("Intercept", ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance < 24 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser && PreventDoubleIntercept),
                //Heroic Leap
                Spell.CastOnGround("Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location, ret => StyxWoW.Me.CurrentTarget.Distance > 9 && !StyxWoW.Me.CurrentTarget.HasAura("Intercept", 1) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser && PreventDoubleIntercept),

                // ranged slow
                Spell.Buff("Piercing Howl", ret => StyxWoW.Me.CurrentTarget.Distance < 10 && StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows) && SingularSettings.Instance.Warrior.UseWarriorSlows && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // melee slow
                Spell.Buff("Hamstring", ret => StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows) && SingularSettings.Instance.Warrior.UseWarriorSlows && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),

                //Interupts
                new Decorator(
                    ret => SingularSettings.Instance.Warrior.UseWarriorInterupts,
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget)),

                Movement.CreateMoveBehindTargetBehavior(),
                //Heal up in melee
                Spell.Cast("Victory Rush", ret => SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                Spell.Cast("Heroic Throw", ret => StyxWoW.Me.CurrentTarget.Distance > 15 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),

                // engineering gloves
                Item.UseEquippedItem((uint)WoWInventorySlot.Hands),

                // AOE
                new Decorator(ret => Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 6f) >= 3 && SingularSettings.Instance.Warrior.UseWarriorAOE,
                    new PrioritySelector(
                        Spell.BuffSelf("Recklessness", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns),
                        Spell.BuffSelf("Death Wish", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns),
                        Spell.BuffSelf("Inner Rage"),
                        Spell.Cast("Whirlwind"),
                        Spell.Cast("Cleave"),
                        Spell.Cast("Raging Blow"),
                        Spell.Cast("Blood Thirst"))),

                //Rotation under 20%
                Spell.Buff("Colossus Smash"),
                Spell.Cast("Execute"),
                //Rotation over 20%               

                new Decorator(ret => SingularSettings.Instance.Warrior.UseWarriorSMF,
                    new PrioritySelector(
                        Spell.Cast("Slam", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Bloodsurge")),
                        Spell.Cast("Bloodthirst"),
                        Spell.Cast("Raging Blow"))),
                new Decorator(ret => SingularSettings.Instance.Warrior.UseWarriorSMF == false,
                    new PrioritySelector(
                        Spell.Cast("Raging Blow"),
                        Spell.Cast("Bloodthirst"),
                        Spell.Cast("Slam", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Bloodsurge")))),

                Spell.Cast("Cleave", ret =>
                    // Only even think about Cleave for more than 2 mobs. (We're probably best off using melee range)
                                Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 6f) >= 2 &&
                                    // If we have Incite, Deadly Calm, or enough rage (pooling for CS if viable) we're good.
                                (StyxWoW.Me.HasAura("Incite", 1) || CanUseRageDump())),
                Spell.Cast("Heroic Strike", ret =>
                    // Only even think about HS for less than 2 mobs. (We're probably best off using melee range)
                                Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 6f) < 2 &&
                                    // If we have Incite, or enough rage (pooling for CS if viable) we're good.
                                (StyxWoW.Me.HasAura("Incite", 1) || CanUseRageDump())),

                //Move to Melee
                Movement.CreateMoveToMeleeBehavior(true)
                );
        } 
        #endregion


        #region Pvp
        [Class(WoWClass.Warrior)]
        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateFuryPvpPreCombatBuffs()
        {
            return
                new PrioritySelector(
                    Spell.BuffSelf("Battle Stance", ret => Styx.Logic.LootTargeting.Instance.FirstObject == null));
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.Pull)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateFuryPvpPull()
        {
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(),
                // LOS check
                Movement.CreateMoveToLosBehavior(),
                //face target
                Movement.CreateFaceTargetBehavior(),
                // Auto Attack
                Helpers.Common.CreateAutoAttack(false),

                // buff up
                Spell.BuffSelf("Battle Shout", ret => (SingularSettings.Instance.Warrior.UseWarriorShouts || SingularSettings.Instance.Warrior.UseWarriorT12) && !StyxWoW.Me.HasAnyAura("Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf("Commanding Shout", ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),

                //Shoot flying targets
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false,
                    new PrioritySelector(
                        Spell.WaitForCast(),
                        Spell.Cast("Heroic Throw"),
                        Spell.Cast("Throw", ret => StyxWoW.Me.CurrentTarget.IsFlying && Item.RangedIsType(WoWItemWeaponClass.Thrown)), Spell.Cast(
                            "Shoot",
                            ret =>
                            StyxWoW.Me.CurrentTarget.IsFlying &&
                            (Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun))),
                        Movement.CreateMoveToTargetBehavior(true, 27f)
                        )),

                //low level support
                new Decorator(
                    ret => StyxWoW.Me.Level < 50,
                    new PrioritySelector(
                        Spell.BuffSelf("Battle Stance"),
                        Spell.Cast("Charge", ret => StyxWoW.Me.CurrentTarget.Distance > 12 && StyxWoW.Me.CurrentTarget.Distance <= 25),
                        Spell.Cast("Heroic Throw", ret => !StyxWoW.Me.CurrentTarget.HasAura("Charge Stun")),
                        Movement.CreateMoveToTargetBehavior(true, 5f))),

                // Heroic fury
                new Decorator(
                    ret => HasSpellIntercept(),
                    new PrioritySelector(
                        Spell.BuffSelf(
                            "Heroic Fury",
                            ret => SpellManager.Spells["Intercept"].Cooldown && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false))),

                // Get closer to target
                Spell.Cast("Charge", ret => PreventDoubleIntercept && StyxWoW.Me.CurrentTarget.Distance.Between(8f, TalentManager.HasGlyph("Long Charge") ? 30f : 25f) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),
                Spell.Cast("Intercept", ret => PreventDoubleIntercept && StyxWoW.Me.CurrentTarget.Distance.Between(8f, 25f) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance <= 40 && PreventDoubleIntercept &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),

                // Move to Melee
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateFuryPvpCombatBuffs()
        {
            return new PrioritySelector(
                //Heal
                Spell.Buff("Enraged Regeneration", ret => StyxWoW.Me.HealthPercent < 60 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                //Recklessness if low on hp or have Deathwish up or as gank protection
                Spell.BuffSelf("Recklessness", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // Heroic Fury
                Spell.BuffSelf("Heroic Fury", ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Rooted) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // Fear Remover
                Spell.BuffSelf("Berserker Rage", ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Fleeing, WoWSpellMechanic.Sapped, WoWSpellMechanic.Incapacitated, WoWSpellMechanic.Horrified)),
                //Deathwish
                Spell.BuffSelf("Death Wish", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                //Berserker rage to stay enraged
                Spell.BuffSelf("Berserker Rage", ret => !StyxWoW.Me.ActiveAuras.ContainsKey("Enrage") && !StyxWoW.Me.ActiveAuras.ContainsKey("Berserker Rage") && !StyxWoW.Me.ActiveAuras.ContainsKey("Death Wish")),  //!StyxWoW.Me.HasAnyAura("Enrage", "Berserker Rage", "Death Wish")),
                //Battleshout Check
                Spell.BuffSelf("Battle Shout", ret => (SingularSettings.Instance.Warrior.UseWarriorShouts || SingularSettings.Instance.Warrior.UseWarriorT12) && !StyxWoW.Me.HasAnyAura("Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf("Commanding Shout", ret => (StyxWoW.Me.RagePercent < 20 || SingularSettings.Instance.Warrior.UseWarriorT12) && SingularSettings.Instance.Warrior.UseWarriorShouts == false)
                );
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.Combat)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateFuryPvpCombat()
        {
            _slows = new[] { "Hamstring", "Piercing Howl", "Crippling Poison", "Hand of Freedom", "Infected Wounds" };
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(),
                // LOS check
                Movement.CreateMoveToLosBehavior(),
                // Face Target
                Movement.CreateFaceTargetBehavior(),
                // Auto Attack
                Helpers.Common.CreateAutoAttack(false),

                // Low level support
                new Decorator(ret => StyxWoW.Me.Level < 30,
                    new PrioritySelector(
                        Movement.CreateMoveBehindTargetBehavior(),
                        Spell.Cast("Victory Rush"),
                        Spell.Cast("Execute"),
                        Spell.Buff("Rend"),
                        Spell.Cast("Overpower"),
                        Spell.Cast("Bloodthirst"),
                //rage dump
                        Spell.Cast("Thunder Clap", ret => StyxWoW.Me.RagePercent > 50 && Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 6f) > 3),
                        Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent > 60),
                        Movement.CreateMoveToMeleeBehavior(true))),
                //30-50 support
                Spell.BuffSelf("Berserker Stance", ret => StyxWoW.Me.Level > 30 && SingularSettings.Instance.Warrior.UseWarriorKeepStance),

                // Dispel Bubbles
                new Decorator(ret => StyxWoW.Me.CurrentTarget.IsPlayer && (StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Ice Block") || StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Hand of Protection") || StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Divine Shield")) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false,
                    new PrioritySelector(
                        Spell.BuffSelf("Battle Stance"),
                        Spell.WaitForCast(),
                        Movement.CreateEnsureMovementStoppedBehavior(),
                        Spell.Cast("Shattering Throw"),
                        Spell.BuffSelf("Berserker Stance"),
                        Movement.CreateMoveToTargetBehavior(true, 30f)
                        )),

                // Intercept
                Spell.Cast("Intercept", ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance < 24 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser && PreventDoubleIntercept),
                //Heroic Leap
                Spell.CastOnGround("Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location, ret => StyxWoW.Me.CurrentTarget.Distance > 9 && !StyxWoW.Me.CurrentTarget.HasAura("Intercept", 1) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser && PreventDoubleIntercept),

                // ranged slow
                Spell.Buff("Piercing Howl", ret => StyxWoW.Me.CurrentTarget.Distance < 10 && StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows) && SingularSettings.Instance.Warrior.UseWarriorSlows && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // melee slow
                Spell.Buff("Hamstring", ret => StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows) && SingularSettings.Instance.Warrior.UseWarriorSlows && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),

                //Interupts
                new Decorator(
                    ret => SingularSettings.Instance.Warrior.UseWarriorInterupts,
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget)),

                Movement.CreateMoveBehindTargetBehavior(),
                //Heal up in melee
                Spell.Cast("Victory Rush", ret => SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                Spell.Cast("Heroic Throw", ret => StyxWoW.Me.CurrentTarget.Distance > 15 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),

                // engineering gloves
                Item.UseEquippedItem((uint)WoWInventorySlot.Hands),

                // AOE
                new Decorator(ret => Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 6f) >= 3 && SingularSettings.Instance.Warrior.UseWarriorAOE,
                    new PrioritySelector(
                        Spell.BuffSelf("Recklessness", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns),
                        Spell.BuffSelf("Death Wish", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns),
                        Spell.BuffSelf("Inner Rage"),
                        Spell.Cast("Whirlwind"),
                        Spell.Cast("Cleave"),
                        Spell.Cast("Raging Blow"),
                        Spell.Cast("Blood Thirst"))),

                //Rotation under 20%
                Spell.Buff("Colossus Smash"),
                Spell.Cast("Execute"),
                //Rotation over 20%               

                new Decorator(ret => SingularSettings.Instance.Warrior.UseWarriorSMF,
                    new PrioritySelector(
                        Spell.Cast("Slam", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Bloodsurge")),
                        Spell.Cast("Bloodthirst"),
                        Spell.Cast("Raging Blow"))),
                new Decorator(ret => SingularSettings.Instance.Warrior.UseWarriorSMF == false,
                    new PrioritySelector(
                        Spell.Cast("Raging Blow"),
                        Spell.Cast("Bloodthirst"),
                        Spell.Cast("Slam", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Bloodsurge")))),

                Spell.Cast("Cleave", ret =>
                    // Only even think about Cleave for more than 2 mobs. (We're probably best off using melee range)
                                Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 6f) >= 2 &&
                                    // If we have Incite, Deadly Calm, or enough rage (pooling for CS if viable) we're good.
                                (StyxWoW.Me.HasAura("Incite", 1) || CanUseRageDump())),
                Spell.Cast("Heroic Strike", ret =>
                    // Only even think about HS for less than 2 mobs. (We're probably best off using melee range)
                                Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 6f) < 2 &&
                                    // If we have Incite, or enough rage (pooling for CS if viable) we're good.
                                (StyxWoW.Me.HasAura("Incite", 1) || CanUseRageDump())),

                //Move to Melee
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }
        #endregion

        #region Instance
        [Class(WoWClass.Warrior)]
        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateFuryInstancePreCombatBuffs()
        {
            return
                new PrioritySelector(
                    Spell.BuffSelf("Battle Stance", ret => Styx.Logic.LootTargeting.Instance.FirstObject == null));
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.Pull)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Instances)]
        public static Composite CreateFuryInstancePull()
        {
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(),
                // LOS check
                Movement.CreateMoveToLosBehavior(),
                //face target
                Movement.CreateFaceTargetBehavior(),
                // Auto Attack
                Helpers.Common.CreateAutoAttack(false),

                // buff up
                Spell.BuffSelf("Battle Shout", ret => (SingularSettings.Instance.Warrior.UseWarriorShouts || SingularSettings.Instance.Warrior.UseWarriorT12) && !StyxWoW.Me.HasAnyAura("Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf("Commanding Shout", ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),

                //Shoot flying targets
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false,
                    new PrioritySelector(
                        Spell.WaitForCast(),
                        Spell.Cast("Heroic Throw"),
                        Spell.Cast("Throw", ret => StyxWoW.Me.CurrentTarget.IsFlying && Item.RangedIsType(WoWItemWeaponClass.Thrown)), Spell.Cast(
                            "Shoot",
                            ret =>
                            StyxWoW.Me.CurrentTarget.IsFlying &&
                            (Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun))),
                        Movement.CreateMoveToTargetBehavior(true, 27f)
                        )),

                //low level support
                new Decorator(
                    ret => StyxWoW.Me.Level < 50,
                    new PrioritySelector(
                        Spell.BuffSelf("Battle Stance"),
                        Spell.Cast("Charge", ret => StyxWoW.Me.CurrentTarget.Distance > 12 && StyxWoW.Me.CurrentTarget.Distance <= 25),
                        Spell.Cast("Heroic Throw", ret => !StyxWoW.Me.CurrentTarget.HasAura("Charge Stun")),
                        Movement.CreateMoveToTargetBehavior(true, 5f))),

                // Heroic fury
                new Decorator(
                    ret => HasSpellIntercept(),
                    new PrioritySelector(
                        Spell.BuffSelf(
                            "Heroic Fury",
                            ret => SpellManager.Spells["Intercept"].Cooldown && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false))),

                // Get closer to target
                Spell.Cast("Charge", ret => PreventDoubleIntercept && StyxWoW.Me.CurrentTarget.Distance.Between(8f, TalentManager.HasGlyph("Long Charge") ? 30f : 25f) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),
                Spell.Cast("Intercept", ret => PreventDoubleIntercept && StyxWoW.Me.CurrentTarget.Distance.Between(8f, 25f) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance <= 40 && PreventDoubleIntercept &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser),

                // Move to Melee
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Instances)]
        public static Composite CreateFuryInstanceCombatBuffs()
        {
            return new PrioritySelector(
                //Heal
                Spell.Buff("Enraged Regeneration", ret => StyxWoW.Me.HealthPercent < 60 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                //Recklessness if low on hp or have Deathwish up or as gank protection
                Spell.BuffSelf("Recklessness", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // Heroic Fury
                Spell.BuffSelf("Heroic Fury", ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Rooted) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // Fear Remover
                Spell.BuffSelf("Berserker Rage", ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Fleeing, WoWSpellMechanic.Sapped, WoWSpellMechanic.Incapacitated, WoWSpellMechanic.Horrified)),
                //Deathwish
                Spell.BuffSelf("Death Wish", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                //Berserker rage to stay enraged
                Spell.BuffSelf("Berserker Rage", ret => !StyxWoW.Me.ActiveAuras.ContainsKey("Enrage") && !StyxWoW.Me.ActiveAuras.ContainsKey("Berserker Rage") && !StyxWoW.Me.ActiveAuras.ContainsKey("Death Wish")),  //!StyxWoW.Me.HasAnyAura("Enrage", "Berserker Rage", "Death Wish")),
                //Battleshout Check
                Spell.BuffSelf("Battle Shout", ret => (SingularSettings.Instance.Warrior.UseWarriorShouts || SingularSettings.Instance.Warrior.UseWarriorT12) && !StyxWoW.Me.HasAnyAura("Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf("Commanding Shout", ret => (StyxWoW.Me.RagePercent < 20 || SingularSettings.Instance.Warrior.UseWarriorT12) && SingularSettings.Instance.Warrior.UseWarriorShouts == false)
                );
        }

        [Spec(TalentSpec.FuryWarrior)]
        [Behavior(BehaviorType.Combat)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Instances)]
        public static Composite CreateFuryInstanceCombat()
        {
            _slows = new[] { "Hamstring", "Piercing Howl", "Crippling Poison", "Hand of Freedom", "Infected Wounds" };
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(),
                // LOS check
                Movement.CreateMoveToLosBehavior(),
                // Face Target
                Movement.CreateFaceTargetBehavior(),
                // Auto Attack
                Helpers.Common.CreateAutoAttack(false),

                // Low level support
                new Decorator(ret => StyxWoW.Me.Level < 30,
                    new PrioritySelector(
                        Movement.CreateMoveBehindTargetBehavior(),
                        Spell.Cast("Victory Rush"),
                        Spell.Cast("Execute"),
                        Spell.Buff("Rend"),
                        Spell.Cast("Overpower"),
                        Spell.Cast("Bloodthirst"),
                //rage dump
                        Spell.Cast("Thunder Clap", ret => StyxWoW.Me.RagePercent > 50 && Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 6f) > 3),
                        Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent > 60),
                        Movement.CreateMoveToMeleeBehavior(true))),
                //30-50 support
                Spell.BuffSelf("Berserker Stance", ret => StyxWoW.Me.Level > 30 && SingularSettings.Instance.Warrior.UseWarriorKeepStance),

                // Dispel Bubbles
                new Decorator(ret => StyxWoW.Me.CurrentTarget.IsPlayer && (StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Ice Block") || StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Hand of Protection") || StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Divine Shield")) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false,
                    new PrioritySelector(
                        Spell.BuffSelf("Battle Stance"),
                        Spell.WaitForCast(),
                        Movement.CreateEnsureMovementStoppedBehavior(),
                        Spell.Cast("Shattering Throw"),
                        Spell.BuffSelf("Berserker Stance"),
                        Movement.CreateMoveToTargetBehavior(true, 30f)
                        )),

                // Intercept
                Spell.Cast("Intercept", ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance < 24 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser && PreventDoubleIntercept),
                //Heroic Leap
                Spell.CastOnGround("Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location, ret => StyxWoW.Me.CurrentTarget.Distance > 9 && !StyxWoW.Me.CurrentTarget.HasAura("Intercept", 1) && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false && SingularSettings.Instance.Warrior.UseWarriorCloser && PreventDoubleIntercept),

                // ranged slow
                Spell.Buff("Piercing Howl", ret => StyxWoW.Me.CurrentTarget.Distance < 10 && StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows) && SingularSettings.Instance.Warrior.UseWarriorSlows && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                // melee slow
                Spell.Buff("Hamstring", ret => StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows) && SingularSettings.Instance.Warrior.UseWarriorSlows && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),

                //Interupts
                new Decorator(
                    ret => SingularSettings.Instance.Warrior.UseWarriorInterupts,
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget)),

                Movement.CreateMoveBehindTargetBehavior(),
                //Heal up in mele
                Spell.Cast("Victory Rush", ret =>  SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),
                Spell.Cast("Heroic Throw", ret => StyxWoW.Me.CurrentTarget.Distance > 15 && SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false),

                // engineering gloves
                Item.UseEquippedItem((uint)WoWInventorySlot.Hands),

                // AOE
                new Decorator(ret => Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Radius, 6f) >= 3 && SingularSettings.Instance.Warrior.UseWarriorAOE,
                    new PrioritySelector(
                        Spell.BuffSelf("Recklessness", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns),
                        Spell.BuffSelf("Death Wish", ret => SingularSettings.Instance.Warrior.UseWarriorDpsCooldowns),
                        Spell.BuffSelf("Inner Rage"),
                        Spell.Cast("Whirlwind"),
                        Spell.Cast("Cleave"),
                        Spell.Cast("Raging Blow"),
                        Spell.Cast("Blood Thirst"))),

                //Rotation under 20%
                Spell.Buff("Colossus Smash"),
                Spell.Cast("Execute"),
                //Rotation over 20%               

                new Decorator(ret => SingularSettings.Instance.Warrior.UseWarriorSMF,
                    new PrioritySelector(
                        Spell.Cast("Slam", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Bloodsurge")),
                        Spell.Cast("Bloodthirst"),
                        Spell.Cast("Raging Blow"))),
                new Decorator(ret => SingularSettings.Instance.Warrior.UseWarriorSMF == false,
                    new PrioritySelector(
                        Spell.Cast("Raging Blow"),
                        Spell.Cast("Bloodthirst"),
                        Spell.Cast("Slam", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Bloodsurge")))),

                Spell.Cast("Cleave", ret =>
                    // Only even think about Cleave for more than 2 mobs. (We're probably best off using melee range)
                                Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 6f) >= 2 &&
                                    // If we have Incite, Deadly Calm, or enough rage (pooling for CS if viable) we're good.
                                (StyxWoW.Me.HasAura("Incite", 1) || CanUseRageDump())),
                Spell.Cast("Heroic Strike", ret =>
                    // Only even think about HS for less than 2 mobs. (We're probably best off using melee range)
                                Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 6f) < 2 &&
                                    // If we have Incite, or enough rage (pooling for CS if viable) we're good.
                                (StyxWoW.Me.HasAura("Incite", 1) || CanUseRageDump())),

                //Move to Melee
                Movement.CreateMoveToMeleeBehavior(true)
                );
        }
        #endregion

        #region Utils
        private static readonly WaitTimer InterceptTimer = new WaitTimer(TimeSpan.FromMilliseconds(2000));

        private static bool PreventDoubleIntercept
        {
            get
            {
                var tmp = InterceptTimer.IsFinished;
                if (tmp)
                    InterceptTimer.Reset();
                return tmp;
            }
        }

        static bool CanUseRageDump()
        {
            // Pooling rage for upcoming CS. If its > 8s, make sure we have 60 rage. < 8s, only pop it at 85 rage.
            if (SpellManager.HasSpell("Colossus Smash"))
                return SpellManager.Spells["Colossus Smash"].CooldownTimeLeft().TotalSeconds > 8 ? StyxWoW.Me.RagePercent > 60 : StyxWoW.Me.RagePercent > 85;

            // We don't know CS. So just check if we have 60 rage to use cleave.
            return StyxWoW.Me.RagePercent > 60;
        }

        static bool HasSpellIntercept()
        {
            if (SpellManager.HasSpell("Intercept"))
                return true;
            return false;
        } 
        #endregion
    }
}
