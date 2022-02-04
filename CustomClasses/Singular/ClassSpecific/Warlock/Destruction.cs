using System.Linq;

using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Styx.Logic.Combat;

namespace Singular.ClassSpecific.Warlock
{
    public class Destruction
    {
        #region Common

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DestructionWarlock)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.All)]
        [Priority(1)]
        public static Composite CreateDestructionWarlockPreCombatBuffs()
        {
            return new PrioritySelector(
                Spell.WaitForCast(false),
                Pet.CreateSummonPet("Imp"),
                Spell.Buff("Dark Intent",
                    ret => StyxWoW.Me.PartyMembers.OrderByDescending(p => p.MaxHealth).FirstOrDefault(),
                    ret => !StyxWoW.Me.HasAura("Dark Intent"))
                );
        }

        #endregion

        #region Normal Rotation

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DestructionWarlock)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateDestructionWarlockNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateAutoAttack(true),
                Spell.Cast("Soul Fire"),
                Spell.Buff("Immolate", true),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DestructionWarlock)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateDestructionWarlockNormalCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Cooldowns
                Spell.BuffSelf("Soulshatter", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.IsTargetingMeOrPet)),
                Spell.BuffSelf("Soulburn", ret => StyxWoW.Me.CurrentSoulShards > 0),
                Spell.Cast("Death Coil", ret => StyxWoW.Me.HealthPercent <= 70),

                // AoE rotation
                Spell.BuffSelf("Shadowflame",
                            ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10 && StyxWoW.Me.IsSafelyFacing(u, 90)) >= 3),

                Spell.BuffSelf("Howl of Terror", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10) >= 3),
                Spell.Buff("Fear", ret => Targeting.Instance.TargetList.ElementAtOrDefault(1), ret => !StyxWoW.Me.CurrentTarget.HasAura("Fear")),
                Spell.Buff("Fear", ret => StyxWoW.Me.HealthPercent < 80),

                // Single target rotation
                Spell.Buff("Curse of the Elements"),
                Spell.Cast("Soul Fire", ret => !StyxWoW.Me.HasAura("Improved Soul Fire")),
                Spell.Buff("Immolate", true,ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Fire)),
                Spell.Cast("Conflagrate"),
                Spell.Buff("Bane of Doom", true),
                Spell.Buff("Corruption", true),
                Spell.Cast("Soul Fire", ret => StyxWoW.Me.HasAura("Empowered Imp")),
                Spell.BuffSelf("Demon Soul"),
                Spell.Cast("Chaos Bolt"),
                Spell.Cast("Shadowburn"),
                Spell.Cast("Incinerate"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DestructionWarlock)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateDestructionWarlockPvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Cooldowns
                Spell.BuffSelf("Soulshatter", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.IsTargetingMeOrPet)),
                Spell.BuffSelf("Soulburn", ret => StyxWoW.Me.CurrentSoulShards > 0),
                Spell.Cast("Death Coil", ret => StyxWoW.Me.HealthPercent <= 70),
                Spell.Buff("Dark Intent",
                    ret => StyxWoW.Me.PartyMembers.OrderByDescending(p => p.MaxHealth).FirstOrDefault(),
                    ret => !StyxWoW.Me.HasAura("Dark Intent")),

                Spell.CastOnGround("Shadowfury", ret => StyxWoW.Me.CurrentTarget.Location),

                Spell.BuffSelf("Howl of Terror", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10) >= 3),
                // Dimishing returns fucks Fear up. Avoid using it until a proper DR logic.
                //Spell.Buff("Fear", ret => Targeting.Instance.TargetList.ElementAtOrDefault(1)),
                Spell.Buff("Curse of Tongues", ret => StyxWoW.Me.CurrentTarget.PowerType == WoWPowerType.Mana),
                Spell.Buff("Curse of Elements", ret => StyxWoW.Me.CurrentTarget.PowerType != WoWPowerType.Mana),
                // Single target rotation
                Spell.Cast("Soul Fire", ret => !StyxWoW.Me.HasAura("Improved Soul Fire")),
                Spell.Buff("Immolate", true),
                Spell.Cast("Conflagrate"),
                Spell.Buff("Bane of Doom", true),
                Spell.Buff("Corruption", true),
                Spell.Cast("Soul Fire", ret => StyxWoW.Me.HasAura("Empowered Imp")),
                Spell.BuffSelf("Demon Soul"),
                Spell.Cast("Chaos Bolt"),
                Spell.Cast("Shadowburn"),
                Spell.Cast("Incinerate"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DestructionWarlock)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateDestructionWarlockInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Cooldowns
                Spell.BuffSelf("Soulshatter", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.IsTargetingMeOrPet)),
                Spell.BuffSelf("Soulburn", ret => StyxWoW.Me.CurrentSoulShards > 0),
                Spell.Cast("Death Coil", ret => StyxWoW.Me.HealthPercent <= 70),
                Spell.Buff("Dark Intent",
                    ret => StyxWoW.Me.PartyMembers.OrderByDescending(p => p.MaxHealth).FirstOrDefault(),
                    ret => !StyxWoW.Me.HasAura("Dark Intent")),

                // AoE rotation
                new Decorator(
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember) >= 3,
                    new PrioritySelector(
                        Spell.CastOnGround("Shadowfury", ret => StyxWoW.Me.CurrentTarget.Location),
                        Spell.CastOnGround("Rain of Fire", ret => StyxWoW.Me.CurrentTarget.Location)
                        )),

                // Single target rotation
                Spell.Cast("Soul Fire", ret => !StyxWoW.Me.HasAura("Improved Soul Fire")),
                Spell.Buff("Immolate", true, ret => !StyxWoW.Me.CurrentTarget.IsImmune(WoWSpellSchool.Fire)),
                Spell.Cast("Conflagrate"),
                Spell.Buff("Bane of Doom", true),
                Spell.Buff("Corruption", true),
                Spell.Cast("Soul Fire", ret => StyxWoW.Me.HasAura("Empowered Imp")),
                Spell.BuffSelf("Demon Soul"),
                Spell.Cast("Chaos Bolt"),
                Spell.Cast("Shadowburn"),
                Spell.Cast("Incinerate"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}
