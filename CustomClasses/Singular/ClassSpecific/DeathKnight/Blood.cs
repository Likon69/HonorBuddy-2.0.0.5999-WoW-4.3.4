using System;
using System.Linq;
using CommonBehaviors.Actions;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Singular.ClassSpecific.DeathKnight
{
    public class Blood
    {
        #region Normal Rotation

        private readonly static WaitTimer DeathStrikeTimer = new WaitTimer(TimeSpan.FromSeconds(5));

        [Class(WoWClass.DeathKnight)]
        [Spec(TalentSpec.BloodDeathKnight)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateBloodDeathKnightNormalCombat()
        {
            return
                new PrioritySelector(
                    Safers.EnsureTarget(),
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Spell.WaitForCast(),
                    Helpers.Common.CreateAutoAttack(true),
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                    Spell.BuffSelf("Blood Presence"),

                    // Anti-magic shell
                    Spell.BuffSelf("Anti-Magic Shell",
                                    ret => Unit.NearbyUnfriendlyUnits.Any(u =>
                                                (u.IsCasting || u.ChanneledCastingSpellId != 0) &&
                                                u.CurrentTargetGuid == StyxWoW.Me.Guid &&
                                                SingularSettings.Instance.DeathKnight.UseAntiMagicShell)),

                    /*
                        Big cooldown section. By default, all cooldowns are priorotized by their time ascending
                        for maximum uptime in the long term. By default, all cooldowns are also exlusive. This
                        means they will be used in rotation rather than conjunction. This is required for high
                        end blood tanking.
                    */
                    Spell.BuffSelf("Death Pact",
                                    ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.PetSacrificePercent &&
                                           StyxWoW.Me.GotAlivePet),
                    Spell.BuffSelf("Rune Tap",
                                    ret => StyxWoW.Me.HealthPercent < 90 && StyxWoW.Me.HasAura("Will of the Necropolis")),
                    Spell.BuffSelf("Death Coil",
                                ret => StyxWoW.Me.HealthPercent < 70 && StyxWoW.Me.HasAura("Lichborne")),
                    Spell.Cast("Dancing Rune Weapon",
                                ret => SingularSettings.Instance.DeathKnight.UseDancingRuneWeapon &&
                                       Unit.NearbyUnfriendlyUnits.Count() > 2),
                    Spell.BuffSelf("Bone Shield",
                                    ret => SingularSettings.Instance.DeathKnight.UseBoneShield &&
                                           (!SingularSettings.Instance.DeathKnight.BoneShieldExclusive ||
                                                (!StyxWoW.Me.HasAura("Vampiric Blood") &&
                                                !StyxWoW.Me.HasAura("Dancing Rune Weapon") &&
                                                !StyxWoW.Me.HasAura("Lichborne") &&
                                                !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Vampiric Blood",
                                ret => SingularSettings.Instance.DeathKnight.UseVampiricBlood
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.VampiricBloodPercent
                                        && (!SingularSettings.Instance.DeathKnight.VampiricBloodExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Lichborne",
                                ret => SingularSettings.Instance.DeathKnight.UseLichborne
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.LichbornePercent
                                        && StyxWoW.Me.CurrentRunicPower >= 60
                                        && (!SingularSettings.Instance.DeathKnight.LichborneExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Raise Dead",
                                ret => (SingularSettings.Instance.DeathKnight.UsePetSacrifice
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.PetSacrificeSummonPercent
                                        && (!SingularSettings.Instance.DeathKnight.PetSacrificeExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude"))))),
                    Spell.BuffSelf("Icebound Fortitude",
                                ret => SingularSettings.Instance.DeathKnight.UseIceboundFortitude
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.IceboundFortitudePercent
                                        && (!SingularSettings.Instance.DeathKnight.IceboundFortitudeExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")))),
                    Spell.BuffSelf("Empower Rune Weapon",
                                ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.EmpowerRuneWeaponPercent
                                        && !SpellManager.CanCast("Death Strike")),
                    Spell.BuffSelf("Army of the Dead",
                                ret => SingularSettings.Instance.DeathKnight.UseArmyOfTheDead
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.ArmyOfTheDeadPercent),

                    Spell.Buff("Chains of Ice",
                        ret => StyxWoW.Me.CurrentTarget.Fleeing && !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),

                    new Sequence(
                        Spell.Cast("Death Grip",
                                    ret => StyxWoW.Me.CurrentTarget.DistanceSqr > 10 * 10),
                        new DecoratorContinue(
                            ret => StyxWoW.Me.IsMoving,
                            new Action(ret => Navigator.PlayerMover.MoveStop())),
                        new WaitContinue(1, new ActionAlwaysSucceed())
                        ),

                    // Start AoE section
                    new Decorator(ret => Unit.UnfriendlyUnitsNearTarget(12f).Count() >= SingularSettings.Instance.DeathKnight.DeathAndDecayCount,
                        new PrioritySelector(
                            Spell.CastOnGround("Death and Decay",
                                ret => StyxWoW.Me.CurrentTarget.Location,
                                ret => SingularSettings.Instance.DeathKnight.UseDeathAndDecay),
                            Spell.Cast("Outbreak", 
                                ret => !StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") || 
                                        !StyxWoW.Me.CurrentTarget.HasAura("Blood Plague")),
                            Spell.Buff("Icy Touch", true,
                                ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10 && !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost), "Frost Fever"),
                            Spell.Buff("Plague Strike", true,
                                ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10, "Blood Plague"),
                            Spell.Cast("Pestilence", 
                                ret => StyxWoW.Me.CurrentTarget.HasMyAura("Blood Plague") && 
                                        StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") &&
                                        Unit.UnfriendlyUnitsNearTarget(10f).Count(u => 
                                                !u.HasMyAura("Blood Plague") && 
                                                !u.HasMyAura("Frost Fever")) > 0),
                            new Sequence(
                                Spell.Cast("Death Strike", ret => DeathStrikeTimer.IsFinished),
                                new Action(ret => DeathStrikeTimer.Reset())),
                            Spell.Cast("Heart Strike"),
                            Spell.Cast("Rune Strike"),
                            Spell.Cast("Icy Touch", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                            Movement.CreateMoveToMeleeBehavior(true)
                            )),

                    Spell.Cast("Outbreak",
                        ret => !StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") ||
                                !StyxWoW.Me.CurrentTarget.HasAura("Blood Plague")),
                    Spell.Buff("Icy Touch", true,
                                ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10 && !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost), 
                                "Frost Fever"),
                    Spell.Buff("Plague Strike", true,
                                ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10, 
                                "Blood Plague"),
                    // If we don't have RS yet, just resort to DC. Its not the greatest, but oh well. Make sure we keep enough RP banked for a self-heal if need be.
                    Spell.Cast("Death Coil",
                                ret => !SpellManager.HasSpell("Rune Strike") && StyxWoW.Me.CurrentRunicPower >= 80),
                    Spell.Cast("Death Coil",
                                ret => !StyxWoW.Me.CurrentTarget.IsWithinMeleeRange),
                    Spell.Cast("Rune Strike"),
                    new Sequence(
                        Spell.Cast("Death Strike", ret => DeathStrikeTimer.IsFinished),
                        new Action(ret => DeathStrikeTimer.Reset())),
                    Spell.Cast("Heart Strike"),
                    Spell.Cast("Icy Touch", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                    Movement.CreateMoveToMeleeBehavior(true)
                    );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.DeathKnight)]
        [Spec(TalentSpec.BloodDeathKnight)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateBloodDeathKnightPvPCombat()
        {
            return
                new PrioritySelector(
                    Safers.EnsureTarget(),
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Spell.WaitForCast(),
                    Helpers.Common.CreateAutoAttack(true),
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                    Spell.BuffSelf("Blood Presence"),

                    // Anti-magic shell
                    Spell.BuffSelf("Anti-Magic Shell",
                                    ret => Unit.NearbyUnfriendlyUnits.Any(u =>
                                                (u.IsCasting || u.ChanneledCastingSpellId != 0) &&
                                                u.CurrentTargetGuid == StyxWoW.Me.Guid &&
                                                SingularSettings.Instance.DeathKnight.UseAntiMagicShell)),

                    /*
                        Big cooldown section. By default, all cooldowns are priorotized by their time ascending
                        for maximum uptime in the long term. By default, all cooldowns are also exlusive. This
                        means they will be used in rotation rather than conjunction. This is required for high
                        end blood tanking.
                    */
                    Spell.BuffSelf("Death Pact",
                                    ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.PetSacrificePercent &&
                                           StyxWoW.Me.GotAlivePet),
                    Spell.BuffSelf("Rune Tap",
                                    ret => StyxWoW.Me.HealthPercent < 90 && StyxWoW.Me.HasAura("Will of the Necropolis")),
                    Spell.BuffSelf("Death Coil",
                                ret => StyxWoW.Me.HealthPercent < 70 && StyxWoW.Me.HasAura("Lichborne")),
                    Spell.Cast("Dancing Rune Weapon",
                                ret => SingularSettings.Instance.DeathKnight.UseDancingRuneWeapon &&
                                       Unit.NearbyUnfriendlyUnits.Count() > 2),
                    Spell.BuffSelf("Bone Shield",
                                    ret => SingularSettings.Instance.DeathKnight.UseBoneShield &&
                                           (!SingularSettings.Instance.DeathKnight.BoneShieldExclusive ||
                                                (!StyxWoW.Me.HasAura("Vampiric Blood") &&
                                                !StyxWoW.Me.HasAura("Dancing Rune Weapon") &&
                                                !StyxWoW.Me.HasAura("Lichborne") &&
                                                !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Vampiric Blood",
                                ret => SingularSettings.Instance.DeathKnight.UseVampiricBlood
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.VampiricBloodPercent
                                        && (!SingularSettings.Instance.DeathKnight.VampiricBloodExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Lichborne",
                                ret => SingularSettings.Instance.DeathKnight.UseLichborne
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.LichbornePercent
                                        && StyxWoW.Me.CurrentRunicPower >= 60
                                        && (!SingularSettings.Instance.DeathKnight.LichborneExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Raise Dead",
                                ret => SingularSettings.Instance.DeathKnight.UsePetSacrifice
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.PetSacrificeSummonPercent
                                        && (!SingularSettings.Instance.DeathKnight.PetSacrificeExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Icebound Fortitude",
                                ret => SingularSettings.Instance.DeathKnight.UseIceboundFortitude
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.IceboundFortitudePercent
                                        && (!SingularSettings.Instance.DeathKnight.IceboundFortitudeExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")))),
                    Spell.BuffSelf("Empower Rune Weapon",
                                ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.EmpowerRuneWeaponPercent
                                        && !SpellManager.CanCast("Death Strike")),
                    Spell.BuffSelf("Army of the Dead",
                                ret => SingularSettings.Instance.DeathKnight.UseArmyOfTheDead
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.ArmyOfTheDeadPercent),

                    new Sequence(
                        Spell.Cast("Death Grip",
                                    ret => StyxWoW.Me.CurrentTarget.DistanceSqr > 10 * 10),
                        new DecoratorContinue(
                            ret => StyxWoW.Me.IsMoving,
                            new Action(ret => Navigator.PlayerMover.MoveStop())),
                        new WaitContinue(1, new ActionAlwaysSucceed())
                        ),
                    Spell.Buff("Chains of Ice",
                        ret => StyxWoW.Me.CurrentTarget.DistanceSqr > 10 * 10),

                    Spell.Cast("Outbreak",
                        ret => !StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") ||
                                !StyxWoW.Me.CurrentTarget.HasAura("Blood Plague")),
                    Spell.Buff("Icy Touch", true, 
                        ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10, 
                        "Frost Fever"),
                    Spell.Buff("Plague Strike", true,
                        ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10,
                        "Blood Plague"),
                // If we don't have RS yet, just resort to DC. Its not the greatest, but oh well. Make sure we keep enough RP banked for a self-heal if need be.
                    Spell.Cast("Death Coil",
                                ret => !SpellManager.HasSpell("Rune Strike") && StyxWoW.Me.CurrentRunicPower >= 80),
                    Spell.Cast("Death Coil",
                                ret => StyxWoW.Me.CurrentTarget.Distance > Spell.MeleeRange),
                    Spell.Cast("Rune Strike"),
                    Spell.Buff("Necrotic Strike"),
                    new Sequence(
                        Spell.Cast("Death Strike", ret => DeathStrikeTimer.IsFinished),
                        new Action(ret => DeathStrikeTimer.Reset())),
                    Spell.Cast("Heart Strike"),
                    Spell.Cast("Icy Touch"),
                    Movement.CreateMoveToMeleeBehavior(true)
                    );
        }

        #endregion

        #region Tanking - Instances and Raids

        // Blood DKs should be DG'ing everything it can when pulling. ONLY IN INSTANCES.
        [Class(WoWClass.DeathKnight)]
        [Behavior(BehaviorType.Pull)]
        [Spec(TalentSpec.BloodDeathKnight)]
        [Context(WoWContext.Instances)]
        public static Composite CreateBloodDeathKnightInstancePull()
        {
            return
                new PrioritySelector(
                    Safers.EnsureTarget(),
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Helpers.Common.CreateAutoAttack(true),
                    new Sequence(
                        Spell.Cast("Death Grip",
                                    ret => SingularSettings.Instance.EnableTaunting && StyxWoW.Me.CurrentTarget.DistanceSqr > 10 * 10),
                        new DecoratorContinue(
                            ret => StyxWoW.Me.IsMoving,
                            new Action(ret => Navigator.PlayerMover.MoveStop())),
                        new WaitContinue(1, new ActionAlwaysSucceed())),
                    Spell.Cast("Howling Blast"),
                    Spell.Cast("Icy Touch"),
                    Movement.CreateMoveToTargetBehavior(true, 5f),
                    Helpers.Common.CreateAutoAttack(true)
                    );
        }

        [Class(WoWClass.DeathKnight)]
        [Spec(TalentSpec.BloodDeathKnight)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateBloodDeathKnightInstanceCombat()
        {
            return
                new PrioritySelector(
                    Safers.EnsureTarget(),
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Spell.WaitForCast(),
                    Helpers.Common.CreateAutoAttack(true),
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                    Spell.BuffSelf("Blood Presence"),

                    // Anti-magic shell
                    Spell.BuffSelf("Anti-Magic Shell",
                                    ret => Unit.NearbyUnfriendlyUnits.Any(u =>
                                                (u.IsCasting || u.ChanneledCastingSpellId != 0) &&
                                                u.CurrentTargetGuid == StyxWoW.Me.Guid &&
                                                SingularSettings.Instance.DeathKnight.UseAntiMagicShell)),

                    /*
                        Big cooldown section. By default, all cooldowns are priorotized by their time ascending
                        for maximum uptime in the long term. By default, all cooldowns are also exlusive. This
                        means they will be used in rotation rather than conjunction. This is required for high
                        end blood tanking.
                    */
                    Spell.BuffSelf("Death Pact",
                                    ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.PetSacrificePercent &&
                                           StyxWoW.Me.GotAlivePet),
                    Spell.BuffSelf("Rune Tap",
                                    ret => StyxWoW.Me.HealthPercent < 90 && StyxWoW.Me.HasAura("Will of the Necropolis")),
                    Spell.BuffSelf("Death Coil",
                                ret => StyxWoW.Me.HealthPercent < 70 && StyxWoW.Me.HasAura("Lichborne")),
                    Spell.Cast("Dancing Rune Weapon",
                                ret => SingularSettings.Instance.DeathKnight.UseDancingRuneWeapon &&
                                       Unit.NearbyUnfriendlyUnits.Count() > 2),
                    Spell.BuffSelf("Bone Shield",
                                    ret => SingularSettings.Instance.DeathKnight.UseBoneShield &&
                                           (!SingularSettings.Instance.DeathKnight.BoneShieldExclusive ||
                                                (!StyxWoW.Me.HasAura("Vampiric Blood") &&
                                                !StyxWoW.Me.HasAura("Dancing Rune Weapon") &&
                                                !StyxWoW.Me.HasAura("Lichborne") &&
                                                !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Vampiric Blood",
                                ret => SingularSettings.Instance.DeathKnight.UseVampiricBlood
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.VampiricBloodPercent
                                        && (!SingularSettings.Instance.DeathKnight.VampiricBloodExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Lichborne",
                                ret => SingularSettings.Instance.DeathKnight.UseLichborne
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.LichbornePercent
                                        && StyxWoW.Me.CurrentRunicPower >= 60
                                        && (!SingularSettings.Instance.DeathKnight.LichborneExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Raise Dead",
                                ret => SingularSettings.Instance.DeathKnight.UsePetSacrifice
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.PetSacrificeSummonPercent
                                        && (!SingularSettings.Instance.DeathKnight.PetSacrificeExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")
                                            && !StyxWoW.Me.HasAura("Icebound Fortitude")))),
                    Spell.BuffSelf("Icebound Fortitude",
                                ret => SingularSettings.Instance.DeathKnight.UseIceboundFortitude
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.IceboundFortitudePercent
                                        && (!SingularSettings.Instance.DeathKnight.IceboundFortitudeExclusive ||
                                            (!StyxWoW.Me.HasAura("Bone Shield")
                                            && !StyxWoW.Me.HasAura("Vampiric Blood")
                                            && !StyxWoW.Me.HasAura("Dancing Rune Weapon")
                                            && !StyxWoW.Me.HasAura("Lichborne")))),
                    Spell.BuffSelf("Empower Rune Weapon",
                                ret => StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.EmpowerRuneWeaponPercent
                                        && !SpellManager.CanCast("Death Strike")),
                    Spell.BuffSelf("Army of the Dead",
                                ret => SingularSettings.Instance.DeathKnight.UseArmyOfTheDead
                                        && StyxWoW.Me.HealthPercent < SingularSettings.Instance.DeathKnight.ArmyOfTheDeadPercent),

                    new Sequence(
                        Spell.Cast("Death Grip",
                                    ret => SingularSettings.Instance.EnableTaunting &&
                                           StyxWoW.Me.CurrentTarget.DistanceSqr > 10 * 10),
                        new DecoratorContinue(
                            ret => StyxWoW.Me.IsMoving,
                            new Action(ret => Navigator.PlayerMover.MoveStop())),
                        new WaitContinue(1, new ActionAlwaysSucceed())
                        ),

                    Spell.Cast("Dark Command",
                        ret => TankManager.Instance.NeedToTaunt.FirstOrDefault(),
                        ret => SingularSettings.Instance.EnableTaunting),

                    // Start AoE section
                    new Decorator(ret => Unit.UnfriendlyUnitsNearTarget(15f).Count() >= SingularSettings.Instance.DeathKnight.DeathAndDecayCount,
                        new PrioritySelector(
                            Spell.CastOnGround("Death and Decay",
                                ret => StyxWoW.Me.CurrentTarget.Location,
                                ret => SingularSettings.Instance.DeathKnight.UseDeathAndDecay),
                            Spell.Cast("Outbreak",
                                ret => !StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") ||
                                        !StyxWoW.Me.CurrentTarget.HasAura("Blood Plague")),
                            Spell.Buff("Icy Touch", true,
                                ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10 && !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost),
                                "Frost Fever"),
                            Spell.Buff("Plague Strike", true,
                                ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10,
                                "Blood Plague"),
                            Spell.Cast("Pestilence",
                                ret => StyxWoW.Me.CurrentTarget.HasMyAura("Blood Plague") &&
                                        StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") &&
                                        Unit.UnfriendlyUnitsNearTarget(10f).Count(u =>
                                                !u.HasMyAura("Blood Plague") &&
                                                !u.HasMyAura("Frost Fever")) > 0),
                            new Sequence(
                                Spell.Cast("Death Strike", ret => DeathStrikeTimer.IsFinished),
                                new Action(ret => DeathStrikeTimer.Reset())),
                            Spell.Cast("Heart Strike"),
                            Spell.Cast("Rune Strike"),
                            Spell.Cast("Icy Touch", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                            Movement.CreateMoveToMeleeBehavior(true)
                            )),
                            
                    Spell.Cast("Outbreak",
                        ret => !StyxWoW.Me.CurrentTarget.HasMyAura("Frost Fever") ||
                                !StyxWoW.Me.CurrentTarget.HasAura("Blood Plague")),
                    Spell.Buff("Icy Touch", true,
                        ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10 && !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost),
                        "Frost Fever"),
                    Spell.Buff("Plague Strike", true,
                        ret => Spell.GetSpellCooldown("Outbreak").TotalSeconds > 10,
                        "Blood Plague"),
                // If we don't have RS yet, just resort to DC. Its not the greatest, but oh well. Make sure we keep enough RP banked for a self-heal if need be.
                    Spell.Cast("Death Coil",
                                ret => !SpellManager.HasSpell("Rune Strike") && StyxWoW.Me.CurrentRunicPower >= 80),
                    Spell.Cast("Death Coil",
                                ret => !StyxWoW.Me.CurrentTarget.IsWithinMeleeRange),
                    Spell.Cast("Rune Strike"),
                    new Sequence(
                        Spell.Cast("Death Strike", ret => DeathStrikeTimer.IsFinished),
                        new Action(ret => DeathStrikeTimer.Reset())),
                    Spell.Cast("Heart Strike"),
                    Spell.Cast("Icy Touch", ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Frost)),
                    Movement.CreateMoveToMeleeBehavior(true)
                    );
        }

        #endregion
    }
}
