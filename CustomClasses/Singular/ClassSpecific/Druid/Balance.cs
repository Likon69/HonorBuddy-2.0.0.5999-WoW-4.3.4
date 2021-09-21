using System;
using System.Linq;

using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace Singular.ClassSpecific.Druid
{
    public class Balance
    {
        # region Properties & Fields

        private static string _oldDps = "Wrath";

        private static int StarfallRange { get { return TalentManager.HasGlyph("Focus") ? 20 : 40; } }

        private static int CurrentEclipse { get { return BitConverter.ToInt32(BitConverter.GetBytes(StyxWoW.Me.CurrentEclipse), 0); } }

        private static string BoomkinDpsSpell
        {
            get
            {
                if (StyxWoW.Me.HasAura("Eclipse (Solar)"))
                {
                    _oldDps = "Wrath";
                }
                // This doesn't seem to register for whatever reason.
                else if (StyxWoW.Me.HasAura("Eclipse (Lunar)")) //Eclipse (Lunar) => 48518
                {
                    _oldDps = "Starfire";
                }

                return _oldDps;
            }
        }

        static int MushroomCount
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == 47649 && o.Distance <= 40).Count(o => o.CreatedByUnitGuid == StyxWoW.Me.Guid); }
        }

        static WoWUnit BestAoeTarget
        {
            get { return Clusters.GetBestUnitForCluster(Unit.NearbyUnfriendlyUnits.Where(u => u.Combat && !u.IsCrowdControlled()), ClusterType.Radius, 8f); }
        }

        #endregion

        #region Normal Rotation

        [Class(WoWClass.Druid)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Spec(TalentSpec.BalanceDruid)]
        [Context(WoWContext.Normal)]
        public static Composite CreateBalanceDruidNormalCombat()
        {
            Common.WantedDruidForm = ShapeshiftForm.Moonkin;
            return new PrioritySelector(
                Spell.WaitForCast(true),
                //Heals, will not heal if in a party or if disabled via setting
                Common.CreateNonRestoHeals(),


                //Innervate
                Spell.Buff("Innervate", ret => StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Druid.InnervateMana),

                Spell.BuffSelf("Moonkin Form"),

                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),

                // Ensure we do /petattack if we have treants up.
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        // If we got 3 shrooms out. Pop 'em
                        Spell.Cast("Wild Mushroom: Detonate", ret => MushroomCount == 3),

                        // If Detonate is coming off CD, make sure we drop some more shrooms. 3 seconds is probably a little late, but good enough.
                        Spell.CastOnGround("Wild Mushroom", 
                            ret => BestAoeTarget.Location,
                            ret => BestAoeTarget != null && Spell.GetSpellCooldown("Wild Mushroom: Detonate").TotalSeconds <= 5),

                        // Spread MF/IS
                        Spell.CastOnGround("Force of Nature", 
                            ret => StyxWoW.Me.CurrentTarget.Location, 
                            ret => StyxWoW.Me.HasAura("Eclipse (Solar)")),
                        Spell.Cast("Starfall", 
                            ret => StyxWoW.Me, 
                            ret => SingularSettings.Instance.Druid.UseStarfall && StyxWoW.Me.HasAura("Eclipse (Lunar)")),
                
                        Spell.Cast("Moonfire", 
                            ret => Unit.NearbyUnfriendlyUnits.FirstOrDefault(u => 
                                        u.Combat && !u.IsCrowdControlled() && !u.HasMyAura("Moonfire") && !u.HasMyAura("Sunfire"))),
                        Spell.Cast("Insect Swarm", 
                            ret => Unit.NearbyUnfriendlyUnits.FirstOrDefault(u => 
                                        u.Combat && !u.IsCrowdControlled() && !u.HasMyAura("Insect Swarm")))
                        )),

                // Starsurge on every proc. Plain and simple.
                Spell.Cast("Starsurge"),

                // Refresh MF/SF
                Spell.Cast("Moonfire", 
                    ret => (StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Moonfire", true).TotalSeconds < 3 &&
                            StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Sunfire", true).TotalSeconds < 3) ||
                            (!StyxWoW.Me.HasAura("Nature's Grace") && TalentManager.GetCount(1,1) > 0) ||
                            StyxWoW.Me.IsMoving),

                // Make sure we keep IS up. Clip the last tick. (~3s)
                Spell.Cast("Insect Swarm", ret => StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Insect Swarm", true).TotalSeconds < 3),

                // And then just spam Wrath/Starfire
                Spell.Cast("Wrath", ret => BoomkinDpsSpell == "Wrath"),
                Spell.Cast("Starfire", ret => BoomkinDpsSpell == "Starfire"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Druid)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Spec(TalentSpec.BalanceDruid)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateBalanceDruidPvPCombat()
        {
            Common.WantedDruidForm = ShapeshiftForm.Moonkin;
            return new PrioritySelector(
                Spell.WaitForCast(true),

                //Inervate
                Spell.Buff("Innervate", ret => StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Druid.InnervateMana),

                Spell.BuffSelf("Moonkin Form"),
                Spell.BuffSelf("Barkskin", 
                    ret => StyxWoW.Me.IsCrowdControlled() || StyxWoW.Me.HealthPercent < 40),
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),

                // Ensure we do /petattack if we have treants up.
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Spread MF/IS
                Spell.CastOnGround("Force of Nature",
                    ret => StyxWoW.Me.CurrentTarget.Location),
                Spell.Cast("Starfall",
                    ret => StyxWoW.Me,
                    ret => SingularSettings.Instance.Druid.UseStarfall),
                Spell.Buff("Faerie Fire", 
                    ret => StyxWoW.Me.CurrentTarget.Class == WoWClass.Rogue ||
                           StyxWoW.Me.CurrentTarget.Class == WoWClass.Druid),
                // Starsurge on every proc. Plain and simple.
                Spell.Cast("Starsurge"),
                // Refresh MF/SF
                Spell.Cast("Moonfire",
                    ret => (StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Moonfire", true).TotalSeconds < 3 &&
                            StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Sunfire", true).TotalSeconds < 3) ||
                            (!StyxWoW.Me.HasAura("Nature's Grace") && TalentManager.GetCount(1, 1) > 0) ||
                            StyxWoW.Me.IsMoving),
                // Make sure we keep IS up. Clip the last tick. (~3s)
                Spell.Cast("Insect Swarm", ret => StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Insect Swarm", true).TotalSeconds < 3),
                // And then just spam Wrath/Starfire
                Spell.Cast("Wrath", ret => BoomkinDpsSpell == "Wrath"),
                Spell.Cast("Starfire", ret => BoomkinDpsSpell == "Starfire"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Druid)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Spec(TalentSpec.BalanceDruid)]
        [Context(WoWContext.Instances)]
        public static Composite CreateBalanceDruidInstanceCombat()
        {
            Common.WantedDruidForm = ShapeshiftForm.Moonkin;
            return new PrioritySelector(
                Spell.WaitForCast(true),

                //Inervate
                Spell.Buff("Innervate",
                    ret => (from raidMember in StyxWoW.Me.RaidMemberInfos
                                let player = raidMember.ToPlayer()
                                where player != null && raidMember.HasRole(WoWPartyMember.GroupRole.Healer) && player.ManaPercent <= 15
                                select player).FirstOrDefault()),

                Spell.BuffSelf("Innervate", 
                    ret => StyxWoW.Me.ManaPercent <= SingularSettings.Instance.Druid.InnervateMana),
                Spell.BuffSelf("Moonkin Form"),

                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),

                // Ensure we do /petattack if we have treants up.
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.Cast("Starfall", 
                    ret => StyxWoW.Me, 
                    ret => SingularSettings.Instance.Druid.UseStarfall && StyxWoW.Me.HasAura("Eclipse (Lunar)") &&
                           StyxWoW.Me.CurrentTarget.IsBoss()),
                Spell.CastOnGround("Force of Nature", 
                    ret => StyxWoW.Me.CurrentTarget.Location, 
                    ret => StyxWoW.Me.HasAura("Eclipse (Solar)") && StyxWoW.Me.CurrentTarget.IsBoss()),

                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(10f).Count() >= 3,
                    new PrioritySelector(
                        // If we got 3 shrooms out. Pop 'em
                        Spell.Cast("Wild Mushroom: Detonate", ret => MushroomCount == 3),

                        // If Detonate is coming off CD, make sure we drop some more shrooms. 3 seconds is probably a little late, but good enough.
                        Spell.CastOnGround("Wild Mushroom",
                            ret => BestAoeTarget.Location,
                            ret => BestAoeTarget != null && Spell.GetSpellCooldown("Wild Mushroom: Detonate").TotalSeconds <= 5),

                        // Spread MF/IS
                        Spell.CastOnGround("Force of Nature",
                            ret => StyxWoW.Me.CurrentTarget.Location,
                            ret => StyxWoW.Me.HasAura("Eclipse (Solar)")),
                        Spell.Cast("Starfall",
                            ret => StyxWoW.Me,
                            ret => SingularSettings.Instance.Druid.UseStarfall && StyxWoW.Me.HasAura("Eclipse (Lunar)")),

                        Spell.Cast("Moonfire",
                            ret => Unit.NearbyUnfriendlyUnits.FirstOrDefault(u => 
                                        u.Combat && !u.IsCrowdControlled() && !u.HasMyAura("Moonfire") && !u.HasMyAura("Sunfire"))),
                        Spell.Cast("Insect Swarm",
                            ret => Unit.NearbyUnfriendlyUnits.FirstOrDefault(u => 
                                        u.Combat && !u.IsCrowdControlled() &&!u.HasMyAura("Insect Swarm")))
                        )),

                // Starsurge on every proc. Plain and simple.
                Spell.Cast("Starsurge"),

                // Refresh MF/SF
                Spell.Cast("Moonfire",
                    ret => (StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Moonfire", true).TotalSeconds < 3 &&
                            StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Sunfire", true).TotalSeconds < 3) ||
                            (!StyxWoW.Me.HasAura("Nature's Grace") && TalentManager.GetCount(1, 1) > 0) ||
                            StyxWoW.Me.IsMoving),

                // Make sure we keep IS up. Clip the last tick. (~3s)
                Spell.Cast("Insect Swarm", ret => StyxWoW.Me.CurrentTarget.GetAuraTimeLeft("Insect Swarm", true).TotalSeconds < 3),

                // And then just spam Wrath/Starfire
                Spell.Cast("Wrath", ret => BoomkinDpsSpell == "Wrath"),
                Spell.Cast("Starfire", ret => BoomkinDpsSpell == "Starfire"),
                Movement.CreateMoveToTargetBehavior(true, 35f)
                );
        }

        #endregion
    }
}
