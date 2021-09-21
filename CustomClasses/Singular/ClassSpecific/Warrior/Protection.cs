using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using TreeSharp;

namespace Singular.ClassSpecific.Warrior
{
    public class Protection
    {
        private static string[] _slows;

        #region Normal

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        public static Composite CreateProtectionWarriorNormalPull()
        {
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(), //face target
                Movement.CreateFaceTargetBehavior(), // LOS check
                Movement.CreateMoveToLosBehavior(), // Auto Attack
                Helpers.Common.CreateAutoAttack(false), //Dismount
                new Decorator(ret => StyxWoW.Me.Mounted, Helpers.Common.CreateDismount("Pulling")),
                //Shoot flying targets
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying,
                    new PrioritySelector(
                        Spell.WaitForCast(), Spell.Cast("Heroic Throw"),
                        Spell.Cast(
                            "Throw",
                            ret => StyxWoW.Me.CurrentTarget.IsFlying && Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast(
                            "Shoot",
                            ret =>
                            StyxWoW.Me.CurrentTarget.IsFlying &&
                            (Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun))),
                        Movement.CreateMoveToTargetBehavior(true, 27f))), //Buff up
                Spell.BuffSelf(
                    "Commanding Shout",
                    ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")), //Charge
                Spell.Cast(
                    "Charge",
                    ret =>
                    SpellManager.HasSpell("Charge") &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Charge"].ActualMinRange(StyxWoW.Me.CurrentTarget),
                        TalentManager.HasGlyph("Long Charge")
                            ? SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget) + 5
                            : SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget))), //Heroic Leap
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret =>
                    StyxWoW.Me.CurrentTarget.Distance > 9 && !StyxWoW.Me.CurrentTarget.HasAura("Charge Stun", 1) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false &&
                    SingularSettings.Instance.Warrior.UseWarriorCloser && Common.PreventDoubleCharge),
                Spell.Cast(
                    "Heroic Throw",
                    ret =>
                    !Unit.HasAura(StyxWoW.Me.CurrentTarget, "Charge Stun") &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false), // Move to Melee
                Movement.CreateMoveToMeleeBehavior(true));
        }

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Normal)]
        public static Composite CreateProtectionNormalCombatBuffs()
        {
            return new PrioritySelector(
                Spell.BuffSelf("Defensive Stance"),
                new Decorator(
                    ret =>
                    StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Warrior.WarriorEnragedRegenerationHealth,
                    new PrioritySelector(Spell.BuffSelf("Berserker Rage"), Spell.BuffSelf("Enraged Regeneration"))),
                //Defensive Cooldowns
                Spell.BuffSelf("Shield Block"),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf(
                    "Shield Wall",
                    ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Warrior.WarriorProtShieldWallHealth),
                Spell.Buff(
                    "Demoralizing Shout",
                    ret => SpellManager.CanCast("Demoralizing Shout") && !StyxWoW.Me.CurrentTarget.HasDemoralizing()),
                //Offensive Cooldowns
                Spell.Buff(
                    "Retaliation",
                    ret =>
                    Clusters.GetClusterCount(
                        StyxWoW.Me, Unit.NearbyUnitsInCombatWithMe.Where(u => u.PowerType != WoWPowerType.Mana),
                        ClusterType.Cone, 6f) >= 3),
                Spell.BuffSelf(
                    "Deadly Calm",
                    ret =>
                    StyxWoW.Me.RagePercent < 10 &&
                    Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnitsInCombatWithMe, ClusterType.Radius, 10f) >= 3),
                Spell.BuffSelf(
                    "Inner Rage",
                    ret =>
                    StyxWoW.Me.RagePercent > 90 &&
                    ((SpellManager.HasSpell("Deadly Calm") && SpellManager.Spells["Deadly Calm"].Cooldown) ||
                     !SpellManager.HasSpell("Deadly Calm"))), // Fear Remover
                Spell.BuffSelf(
                    "Berserker Rage",
                    ret =>
                    StyxWoW.Me.HasAuraWithMechanic(
                        WoWSpellMechanic.Fleeing, WoWSpellMechanic.Sapped, WoWSpellMechanic.Incapacitated,
                        WoWSpellMechanic.Horrified)), //Buff up
                Spell.BuffSelf(
                    "Commanding Shout",
                    ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")));
        }

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        public static Composite CreateProtectionWarriorNormalCombat()
        {
            _slows = new[] {"Hamstring", "Piercing Howl", "Crippling Poison", "Hand of Freedom", "Infected Wounds"};
            return new PrioritySelector(
                ctx => TankManager.Instance.FirstUnit ?? StyxWoW.Me.CurrentTarget, //Standard
                Safers.EnsureTarget(), Movement.CreateMoveToLosBehavior(), Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(false), //Close cap on target
                Spell.Cast(
                    "Charge",
                    ret =>
                    SpellManager.HasSpell("Charge") &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Charge"].ActualMinRange(StyxWoW.Me.CurrentTarget),
                        TalentManager.HasGlyph("Long Charge")
                            ? SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget) + 5
                            : SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget))),
                Spell.Cast(
                    "Intercept",
                    ret =>
                    SpellManager.HasSpell("Intercept") && StyxWoW.Me.CurrentTarget.GotTarget &&
                    !StyxWoW.Me.CurrentTarget.CurrentTarget.IsMe &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Intercept"].ActualMinRange(StyxWoW.Me.CurrentTarget.CurrentTarget),
                        SpellManager.Spells["Intercept"].ActualMaxRange(StyxWoW.Me.CurrentTarget.CurrentTarget))),
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance <= 40),
                //Interupt or reflect
                Spell.Cast(
                    "Spell Reflection",
                    ret => StyxWoW.Me.CurrentTarget.CurrentTarget == StyxWoW.Me && StyxWoW.Me.CurrentTarget.IsCasting),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget), //PVP
                new Decorator(
                    ret => StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.IsPlayer,
                    new PrioritySelector(
                        Spell.Cast("Victory Rush"),
                        Spell.Cast(
                            "Disarm",
                            ctx =>
                            StyxWoW.Me.CurrentTarget.DistanceSqr < 36 &&
                            (StyxWoW.Me.CurrentTarget.Class == WoWClass.Warrior ||
                             StyxWoW.Me.CurrentTarget.Class == WoWClass.Rogue ||
                             StyxWoW.Me.CurrentTarget.Class == WoWClass.Paladin ||
                             StyxWoW.Me.CurrentTarget.Class == WoWClass.Hunter)), Spell.Buff("Rend"),
                        Spell.Cast(
                            "Thunder Clap",
                            ctx => StyxWoW.Me.CurrentTarget.DistanceSqr < 7*7 && StyxWoW.Me.CurrentTarget.Attackable),
                        Spell.Cast("Shockwave"),
                        Spell.Buff(
                            "Piercing Howl",
                            ret =>
                            StyxWoW.Me.CurrentTarget.Distance < 10 && StyxWoW.Me.CurrentTarget.IsPlayer &&
                            !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows)),
                        Spell.Cast(
                            "Cleave",
                            ret =>
                            Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 10f) >= 2),
                        Spell.Cast("Concussion Blow"), Spell.Cast("Shield Slam"), Spell.Cast("Revenge"),
                        Spell.Cast("Devastate"), Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent >= 50))),
                //Aoe tanking
                new Decorator(
                    ret => Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 15f) > 1,
                    new PrioritySelector(
                        Spell.Buff("Rend"),
                        Spell.Cast(
                            "Thunder Clap",
                            ctx =>
                            StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.DistanceSqr < 7*7 &&
                            StyxWoW.Me.CurrentTarget.Attackable),
                        Spell.Cast(
                            "Shockwave",
                            ret =>
                            Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 10f) >= 2),
                        Spell.Cast(
                            "Cleave",
                            ret =>
                            Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 10f) >= 2))),
                //Taunts
                //If more than 3 taunt, if needs to taunt                
                Spell.Cast(
                    "Challenging Shout", ret => TankManager.Instance.NeedToTaunt.First(),
                    ret =>
                    SingularSettings.Instance.EnableTaunting &&
                    TankManager.Instance.NeedToTaunt.Count(u => u.Distance <= 10) >= 3),
                // If there's a unit that needs taunting, do it.
                Spell.Cast(
                    "Taunt", ret => TankManager.Instance.NeedToTaunt.First(),
                    ret =>
                    SingularSettings.Instance.EnableTaunting &&
                    TankManager.Instance.NeedToTaunt.FirstOrDefault() != null), //Single Target
                Spell.Cast("Victory Rush"), Spell.Cast("Concussion Blow"), Spell.Cast("Shield Slam"),
                Spell.Cast("Revenge"), Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent >= 50),
                Spell.Buff("Rend"),
                // Tclap may not be a giant threat increase, but Blood and Thunder will refresh rend. Which all in all, is a good thing.
                // Oh, and the attack speed debuff is win as well.
                Spell.Cast(
                    "Thunder Clap",
                    ctx =>
                    StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.DistanceSqr < 7*7 &&
                    StyxWoW.Me.CurrentTarget.Attackable), Spell.Cast("Shockwave"), Spell.Cast("Devastate"),
                Movement.CreateMoveToTargetBehavior(true, 4f));
        }

        #endregion

        #region Pvp

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Battlegrounds)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        public static Composite CreateProtectionWarriorPvpPull()
        {
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(), //face target
                Movement.CreateFaceTargetBehavior(), // LOS check
                Movement.CreateMoveToLosBehavior(), // Auto Attack
                Helpers.Common.CreateAutoAttack(false), //Dismount
                new Decorator(ret => StyxWoW.Me.Mounted, Helpers.Common.CreateDismount("Pulling")),
                //Shoot flying targets
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying,
                    new PrioritySelector(
                        Spell.WaitForCast(), Spell.Cast("Heroic Throw"),
                        Spell.Cast(
                            "Throw",
                            ret => StyxWoW.Me.CurrentTarget.IsFlying && Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast(
                            "Shoot",
                            ret =>
                            StyxWoW.Me.CurrentTarget.IsFlying &&
                            (Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun))),
                        Movement.CreateMoveToTargetBehavior(true, 27f))), //Buff up
                Spell.BuffSelf(
                    "Commanding Shout",
                    ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")), //Charge
                Spell.Cast(
                    "Charge",
                    ret =>
                    SpellManager.HasSpell("Charge") &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Charge"].ActualMinRange(StyxWoW.Me.CurrentTarget),
                        TalentManager.HasGlyph("Long Charge")
                            ? SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget) + 5
                            : SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget))), //Heroic Leap
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret =>
                    StyxWoW.Me.CurrentTarget.Distance > 9 && !StyxWoW.Me.CurrentTarget.HasAura("Charge Stun", 1) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false &&
                    SingularSettings.Instance.Warrior.UseWarriorCloser && Common.PreventDoubleCharge),
                Spell.Cast(
                    "Heroic Throw",
                    ret =>
                    !Unit.HasAura(StyxWoW.Me.CurrentTarget, "Charge Stun") &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false), // Move to Melee
                Movement.CreateMoveToMeleeBehavior(true));
        }

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateProtectionPvpCombatBuffs()
        {
            return new PrioritySelector(
                Spell.BuffSelf("Defensive Stance"),
                new Decorator(
                    ret =>
                    StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Warrior.WarriorEnragedRegenerationHealth,
                    new PrioritySelector(Spell.BuffSelf("Berserker Rage"), Spell.BuffSelf("Enraged Regeneration"))),
                //Defensive Cooldowns
                Spell.BuffSelf("Shield Block"),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf(
                    "Shield Wall",
                    ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Warrior.WarriorProtShieldWallHealth),
                Spell.Buff(
                    "Demoralizing Shout",
                    ret => SpellManager.CanCast("Demoralizing Shout") && !StyxWoW.Me.CurrentTarget.HasDemoralizing()),
                //Offensive Cooldowns
                Spell.Buff(
                    "Retaliation",
                    ret =>
                    Clusters.GetClusterCount(
                        StyxWoW.Me, Unit.NearbyUnitsInCombatWithMe.Where(u => u.PowerType != WoWPowerType.Mana),
                        ClusterType.Cone, 6f) >= 3),
                Spell.BuffSelf(
                    "Deadly Calm",
                    ret =>
                    StyxWoW.Me.RagePercent < 10 &&
                    Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnitsInCombatWithMe, ClusterType.Radius, 10f) >= 3),
                Spell.BuffSelf(
                    "Inner Rage",
                    ret =>
                    StyxWoW.Me.RagePercent > 90 &&
                    ((SpellManager.HasSpell("Deadly Calm") && SpellManager.Spells["Deadly Calm"].Cooldown) ||
                     !SpellManager.HasSpell("Deadly Calm"))), // Fear Remover
                Spell.BuffSelf(
                    "Berserker Rage",
                    ret =>
                    StyxWoW.Me.HasAuraWithMechanic(
                        WoWSpellMechanic.Fleeing, WoWSpellMechanic.Sapped, WoWSpellMechanic.Incapacitated,
                        WoWSpellMechanic.Horrified)), //Buff up
                Spell.BuffSelf(
                    "Commanding Shout",
                    ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")));
        }

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        public static Composite CreateProtectionWarriorPvPCombat()
        {
            _slows = new[] {"Hamstring", "Piercing Howl", "Crippling Poison", "Hand of Freedom", "Infected Wounds"};
            return new PrioritySelector(
                ctx => TankManager.Instance.FirstUnit ?? StyxWoW.Me.CurrentTarget, //Standard
                Safers.EnsureTarget(), Movement.CreateMoveToLosBehavior(), Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(false), //Close cap on target
                Spell.Cast(
                    "Charge",
                    ret =>
                    SpellManager.HasSpell("Charge") &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Charge"].ActualMinRange(StyxWoW.Me.CurrentTarget),
                        TalentManager.HasGlyph("Long Charge")
                            ? SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget) + 5
                            : SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget))),
                Spell.Cast(
                    "Intercept",
                    ret =>
                    SpellManager.HasSpell("Intercept") && StyxWoW.Me.CurrentTarget.GotTarget &&
                    !StyxWoW.Me.CurrentTarget.CurrentTarget.IsMe &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Intercept"].ActualMinRange(StyxWoW.Me.CurrentTarget.CurrentTarget),
                        SpellManager.Spells["Intercept"].ActualMaxRange(StyxWoW.Me.CurrentTarget.CurrentTarget))),
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance <= 40),
                //Interupt or reflect
                Spell.Cast(
                    "Spell Reflection",
                    ret => StyxWoW.Me.CurrentTarget.CurrentTarget == StyxWoW.Me && StyxWoW.Me.CurrentTarget.IsCasting),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget), Spell.Cast("Victory Rush"),
                Spell.Cast(
                    "Disarm",
                    ctx =>
                    StyxWoW.Me.CurrentTarget.DistanceSqr < 36 &&
                    (StyxWoW.Me.CurrentTarget.Class == WoWClass.Warrior ||
                     StyxWoW.Me.CurrentTarget.Class == WoWClass.Rogue ||
                     StyxWoW.Me.CurrentTarget.Class == WoWClass.Paladin ||
                     StyxWoW.Me.CurrentTarget.Class == WoWClass.Hunter)), Spell.Buff("Rend"),
                Spell.Cast(
                    "Thunder Clap",
                    ctx => StyxWoW.Me.CurrentTarget.DistanceSqr < 7*7 && StyxWoW.Me.CurrentTarget.Attackable),
                Spell.Cast("Shockwave"),
                Spell.Buff(
                    "Piercing Howl",
                    ret =>
                    StyxWoW.Me.CurrentTarget.Distance < 10 && StyxWoW.Me.CurrentTarget.IsPlayer &&
                    !StyxWoW.Me.CurrentTarget.HasAnyAura(_slows)),
                Spell.Cast(
                    "Cleave",
                    ret => Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 10f) >= 2),
                Spell.Cast("Concussion Blow"), Spell.Cast("Shield Slam"), Spell.Cast("Revenge"), Spell.Cast("Devastate"),
                Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent >= 50),
                Movement.CreateMoveToTargetBehavior(true, 4f));
        }

        #endregion

        #region Instance

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Instances)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        public static Composite CreateProtectionWarriorInstancePull()
        {
            return new PrioritySelector(
                // Ensure Target
                Safers.EnsureTarget(), //face target
                Movement.CreateFaceTargetBehavior(), // LOS check
                Movement.CreateMoveToLosBehavior(), // Auto Attack
                Helpers.Common.CreateAutoAttack(false), //Dismount
                new Decorator(ret => StyxWoW.Me.Mounted, Helpers.Common.CreateDismount("Pulling")),
                //Shoot flying targets
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsFlying,
                    new PrioritySelector(
                        Spell.WaitForCast(), Spell.Cast("Heroic Throw"),
                        Spell.Cast(
                            "Throw",
                            ret => StyxWoW.Me.CurrentTarget.IsFlying && Item.RangedIsType(WoWItemWeaponClass.Thrown)),
                        Spell.Cast(
                            "Shoot",
                            ret =>
                            StyxWoW.Me.CurrentTarget.IsFlying &&
                            (Item.RangedIsType(WoWItemWeaponClass.Bow) || Item.RangedIsType(WoWItemWeaponClass.Gun))),
                        Movement.CreateMoveToTargetBehavior(true, 27f))), //Buff up
                Spell.BuffSelf(
                    "Commanding Shout",
                    ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")), //Charge
                Spell.Cast(
                    "Charge",
                    ret =>
                    SpellManager.HasSpell("Charge") &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Charge"].ActualMinRange(StyxWoW.Me.CurrentTarget),
                        TalentManager.HasGlyph("Long Charge")
                            ? SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget) + 5
                            : SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget))), //Heroic Leap
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret =>
                    StyxWoW.Me.CurrentTarget.Distance > 9 && !StyxWoW.Me.CurrentTarget.HasAura("Charge Stun", 1) &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false &&
                    SingularSettings.Instance.Warrior.UseWarriorCloser && Common.PreventDoubleCharge),
                Spell.Cast(
                    "Heroic Throw",
                    ret =>
                    !Unit.HasAura(StyxWoW.Me.CurrentTarget, "Charge Stun") &&
                    SingularSettings.Instance.Warrior.UseWarriorBasicRotation == false), // Move to Melee
                Movement.CreateMoveToMeleeBehavior(true));
        }

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.CombatBuffs)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        [Context(WoWContext.Instances)]
        public static Composite CreateProtectionInstanceCombatBuffs()
        {
            return new PrioritySelector(
                Spell.BuffSelf("Defensive Stance"),
                new Decorator(
                    ret =>
                    StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Warrior.WarriorEnragedRegenerationHealth,
                    new PrioritySelector(Spell.BuffSelf("Berserker Rage"), Spell.BuffSelf("Enraged Regeneration"))),
                //Defensive Cooldowns
                Spell.BuffSelf("Shield Block"),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")),
                Spell.BuffSelf(
                    "Shield Wall",
                    ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Warrior.WarriorProtShieldWallHealth),
                Spell.Buff(
                    "Demoralizing Shout",
                    ret => SpellManager.CanCast("Demoralizing Shout") && !StyxWoW.Me.CurrentTarget.HasDemoralizing()),
                //Offensive Cooldowns
                Spell.Buff(
                    "Retaliation",
                    ret =>
                    Clusters.GetClusterCount(
                        StyxWoW.Me, Unit.NearbyUnitsInCombatWithMe.Where(u => u.PowerType != WoWPowerType.Mana),
                        ClusterType.Cone, 6f) >= 3),
                Spell.BuffSelf(
                    "Deadly Calm",
                    ret =>
                    StyxWoW.Me.RagePercent < 10 &&
                    Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnitsInCombatWithMe, ClusterType.Radius, 10f) >= 3),
                Spell.BuffSelf(
                    "Inner Rage",
                    ret =>
                    StyxWoW.Me.RagePercent > 90 &&
                    ((SpellManager.HasSpell("Deadly Calm") && SpellManager.Spells["Deadly Calm"].Cooldown) ||
                     !SpellManager.HasSpell("Deadly Calm"))), // Fear Remover
                Spell.BuffSelf(
                    "Berserker Rage",
                    ret =>
                    StyxWoW.Me.HasAuraWithMechanic(
                        WoWSpellMechanic.Fleeing, WoWSpellMechanic.Sapped, WoWSpellMechanic.Incapacitated,
                        WoWSpellMechanic.Horrified)), //Buff up
                Spell.BuffSelf(
                    "Commanding Shout",
                    ret => StyxWoW.Me.RagePercent < 20 && SingularSettings.Instance.Warrior.UseWarriorShouts == false),
                Spell.BuffSelf(
                    "Battle Shout",
                    ret =>
                    (SingularSettings.Instance.Warrior.UseWarriorShouts ||
                     SingularSettings.Instance.Warrior.UseWarriorT12) &&
                    !StyxWoW.Me.HasAnyAura(
                        "Horn of Winter", "Roar of Courage", "Strength of Earth Totem", "Battle Shout")));
        }

        [Spec(TalentSpec.ProtectionWarrior)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        [Class(WoWClass.Warrior)]
        [Priority(500)]
        public static Composite CreateProtectionWarriorCombat()
        {
            _slows = new[] {"Hamstring", "Piercing Howl", "Crippling Poison", "Hand of Freedom", "Infected Wounds"};
            return new PrioritySelector(
                ctx => TankManager.Instance.FirstUnit ?? StyxWoW.Me.CurrentTarget, //Standard
                Safers.EnsureTarget(), Movement.CreateMoveToLosBehavior(), Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(false), //Close cap on target
                Spell.Cast(
                    "Charge",
                    ret =>
                    SpellManager.HasSpell("Charge") &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Charge"].ActualMinRange(StyxWoW.Me.CurrentTarget),
                        TalentManager.HasGlyph("Long Charge")
                            ? SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget) + 5
                            : SpellManager.Spells["Charge"].ActualMaxRange(StyxWoW.Me.CurrentTarget))),
                Spell.Cast(
                    "Intercept",
                    ret =>
                    SpellManager.HasSpell("Intercept") && StyxWoW.Me.CurrentTarget.GotTarget &&
                    !StyxWoW.Me.CurrentTarget.CurrentTarget.IsMe &&
                    StyxWoW.Me.CurrentTarget.Distance.Between(
                        SpellManager.Spells["Intercept"].ActualMinRange(StyxWoW.Me.CurrentTarget.CurrentTarget),
                        SpellManager.Spells["Intercept"].ActualMaxRange(StyxWoW.Me.CurrentTarget.CurrentTarget))),
                Spell.CastOnGround(
                    "Heroic Leap", ret => StyxWoW.Me.CurrentTarget.Location,
                    ret => StyxWoW.Me.CurrentTarget.Distance > 10 && StyxWoW.Me.CurrentTarget.Distance <= 40),
                //Interupt or reflect
                Spell.Cast(
                    "Spell Reflection",
                    ret => StyxWoW.Me.CurrentTarget.CurrentTarget == StyxWoW.Me && StyxWoW.Me.CurrentTarget.IsCasting),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget), //Aoe tanking
                new Decorator(
                    ret => Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 15f) > 1,
                    new PrioritySelector(
                        Spell.Buff("Rend"),
                        Spell.Cast(
                            "Thunder Clap",
                            ctx =>
                            StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.DistanceSqr < 7*7 &&
                            StyxWoW.Me.CurrentTarget.Attackable),
                        Spell.Cast(
                            "Shockwave",
                            ret =>
                            Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 10f) >= 2),
                        Spell.Cast(
                            "Cleave",
                            ret =>
                            Clusters.GetClusterCount(StyxWoW.Me, Unit.NearbyUnfriendlyUnits, ClusterType.Cone, 10f) >= 2))),
                //Taunts
                //If more than 3 taunt, if needs to taunt                
                Spell.Cast(
                    "Challenging Shout", ret => TankManager.Instance.NeedToTaunt.First(),
                    ret =>
                    SingularSettings.Instance.EnableTaunting &&
                    TankManager.Instance.NeedToTaunt.Count(u => u.Distance <= 10) >= 3),
                // If there's a unit that needs taunting, do it.
                Spell.Cast(
                    "Taunt", ret => TankManager.Instance.NeedToTaunt.First(),
                    ret =>
                    SingularSettings.Instance.EnableTaunting &&
                    TankManager.Instance.NeedToTaunt.FirstOrDefault() != null), //Single Target
                Spell.Cast("Victory Rush"), Spell.Cast("Concussion Blow"), Spell.Cast("Shield Slam"),
                Spell.Cast("Revenge"), Spell.Cast("Heroic Strike", ret => StyxWoW.Me.RagePercent >= 50),
                Spell.Buff("Rend"),
                // Tclap may not be a giant threat increase, but Blood and Thunder will refresh rend. Which all in all, is a good thing.
                // Oh, and the attack speed debuff is win as well.
                Spell.Cast(
                    "Thunder Clap",
                    ctx =>
                    StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.DistanceSqr < 7*7 &&
                    StyxWoW.Me.CurrentTarget.Attackable), Spell.Cast("Shockwave"), Spell.Cast("Devastate"),
                Movement.CreateMoveToTargetBehavior(true, 4f));
        }

        #endregion
    }
}