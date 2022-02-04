using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Styx;
using System.Linq;

namespace Singular.ClassSpecific.Warlock
{
    public class Demonology
    {
        #region Common

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DemonologyWarlock)]
        [Behavior(BehaviorType.PreCombatBuffs)]
        [Context(WoWContext.All)]
        [Priority(1)]
        public static Composite CreateDemonologyWarlockPreCombatBuffs()
        {
            return new PrioritySelector(
                Spell.WaitForCast(false),
                Pet.CreateSummonPet("Felguard"),
                Spell.Buff("Dark Intent",
                    ret => StyxWoW.Me.PartyMembers.OrderByDescending(p => p.MaxHealth).FirstOrDefault(),
                    ret => !StyxWoW.Me.HasAura("Dark Intent"))
                );
        }

        #endregion

        #region Normal Rotation

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DemonologyWarlock)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.Normal)]
        public static Composite CreateDemonologyWarlockNormalPull()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(true),
                Helpers.Common.CreateAutoAttack(true),
                Spell.Buff("Immolate", true),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DemonologyWarlock)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateDemonologyWarlockNormalCombat()
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

                new Decorator(ret => StyxWoW.Me.CurrentTarget.Fleeing,
                    Pet.CreateCastPetAction("Axe Toss")),
                new Decorator(ret => StyxWoW.Me.GotAlivePet && Unit.NearbyUnfriendlyUnits.Count(u => u.Location.DistanceSqr(StyxWoW.Me.Pet.Location) < 10 * 10) > 1,
                    Pet.CreateCastPetAction("Felstorm")),
                // AoE rotation
                Spell.BuffSelf("Shadowflame",
                            ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10 && StyxWoW.Me.IsSafelyFacing(u, 90)) >= 3),

                Spell.BuffSelf("Howl of Terror", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10) >= 3),
                Spell.Buff("Fear", ret => Targeting.Instance.TargetList.ElementAtOrDefault(1), ret => !StyxWoW.Me.CurrentTarget.HasAura("Fear")),

                // Single target rotation
                Spell.BuffSelf("Metamorphosis"),
                Spell.BuffSelf("Demon Soul"),
                Spell.Buff("Immolate", true),
                Spell.Cast("Hand of Gul'dan"),
                Spell.Buff("Bane of Agony", true),
                Spell.Buff("Corruption", true),
                Spell.Cast("Shadow Bolt", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Shadow Trance")),
                Spell.Cast("Incinerate", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Molten Core")),
                Spell.Cast("Soul Fire", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Decimation")),
                Spell.Cast("Shadow Bolt"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DemonologyWarlock)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateDemonologyWarlockPvPPullAndCombat()
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
                    
                Pet.CreateCastPetAction("Axe Toss"),
                new Decorator(ret => StyxWoW.Me.GotAlivePet && Unit.NearbyUnfriendlyUnits.Count(u => u.Location.DistanceSqr(StyxWoW.Me.Pet.Location) < 10 * 10) > 1,
                    Pet.CreateCastPetAction("Felstorm")),

                // AoE rotation
                Spell.BuffSelf("Shadowflame",
                            ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10 && StyxWoW.Me.IsSafelyFacing(u, 90)) >= 3),

                Spell.BuffSelf("Howl of Terror", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10) >= 3),
                // Dimishing returns fucks Fear up. Avoid using it until a proper DR logic.
                //Spell.Buff("Fear", ret => Targeting.Instance.TargetList.ElementAtOrDefault(1)),

                Spell.Buff("Curse of Tongues", ret => StyxWoW.Me.CurrentTarget.PowerType == WoWPowerType.Mana),
                Spell.Buff("Curse of Elements", ret => StyxWoW.Me.CurrentTarget.PowerType != WoWPowerType.Mana),
                // Single target rotation
                Spell.BuffSelf("Metamorphosis"),
                Spell.BuffSelf("Demon Soul"),
                Spell.Buff("Immolate", true),
                Spell.Cast("Hand of Gul'dan"),
                Spell.Buff("Bane of Agony", true),
                Spell.Buff("Corruption", true),
                Spell.Cast("Shadow Bolt", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Shadow Trance")),
                Spell.Cast("Incinerate", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Molten Core")),
                Spell.Cast("Soul Fire", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Decimation")),
                Spell.Cast("Shadow Bolt"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Warlock)]
        [Spec(TalentSpec.DemonologyWarlock)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateDemonologyWarlockInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                new Decorator(
                    ret => StyxWoW.Me.CastingSpell != null && StyxWoW.Me.CastingSpell.Name == "Hellfire" && StyxWoW.Me.HealthPercent < 60,
                    new Action(ret => SpellManager.StopCasting())),
                Spell.WaitForCast(true),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Cooldowns
                Spell.BuffSelf("Soulshatter", ret => Unit.NearbyUnfriendlyUnits.Any(u => u.IsTargetingMeOrPet)),
                Spell.BuffSelf("Soulburn", ret => StyxWoW.Me.CurrentSoulShards > 0),
                Spell.BuffSelf("Demonic Empowerment"),
                Spell.Cast("Death Coil", ret => StyxWoW.Me.HealthPercent <= 70),
                Spell.Buff("Dark Intent",
                    ret => StyxWoW.Me.PartyMembers.OrderByDescending(p => p.MaxHealth).FirstOrDefault(),
                    ret => !StyxWoW.Me.HasAura("Dark Intent")),

                // AoE rotation
                new Decorator(
                    ret => Unit.NearbyUnfriendlyUnits.Count(u => u.IsTargetingMeOrPet || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember) >= 3,
                    new PrioritySelector(
                        Spell.BuffSelf("Metamorphosis"),
                        Spell.BuffSelf("Immolation Aura", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Metamorphosis")),
                        Spell.BuffSelf("Shadowflame",
                            ret => Unit.NearbyUnfriendlyUnits.Count(u => u.DistanceSqr < 10 * 10 && StyxWoW.Me.IsSafelyFacing(u, 90)) >= 3),
                        Spell.BuffSelf("Hellfire", ret => StyxWoW.Me.HealthPercent > 60)
                        )),

                // Single target rotation
                Spell.BuffSelf("Metamorphosis"),
                Spell.BuffSelf("Demon Soul"),
                Spell.Buff("Immolate", true),
                Spell.Cast("Hand of Gul'dan"),
                Spell.Buff("Bane of Doom", true),
                Spell.Buff("Corruption", true),
                Spell.Cast("Shadow Bolt", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Shadow Trance")),
                Spell.Cast("Incinerate", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Molten Core")),
                Spell.Cast("Soul Fire", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Decimation")),
                Spell.Cast("Shadow Bolt"),

                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}