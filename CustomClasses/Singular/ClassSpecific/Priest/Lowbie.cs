using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Styx;
using Styx.Combat.CombatRoutine;
using TreeSharp;

namespace Singular.ClassSpecific.Priest
{
    public class Lowbie
    {
        [Class(WoWClass.Priest)]
        [Spec(TalentSpec.Lowbie)]
        [Behavior(BehaviorType.Combat)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateLowbiePriestCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                Spell.BuffSelf("Power Word: Shield", ret => !StyxWoW.Me.HasAura("Weakened Soul")),
                Spell.Heal("Flash Heal", ret => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent <= 40),

                Spell.Buff("Shadow Word: Pain"),
                Spell.Cast("Mind Blast"),
                Spell.Cast("Smite"),
                Helpers.Common.CreateUseWand(),
                Movement.CreateMoveToTargetBehavior(true, 25f)
                );
        }
    }
}
