using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using TreeSharp;

namespace Singular.ClassSpecific.Hunter
{
    public class Lowbie
    {
        [Class(WoWClass.Hunter)]
        [Spec(TalentSpec.Lowbie)]
        [Behavior(BehaviorType.Combat)]
        [Behavior(BehaviorType.Pull)]
        [Context(WoWContext.All)]
        public static Composite CreateLowbieCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Common.CreateHunterBackPedal(),
                Movement.CreateFaceTargetBehavior(),
                Spell.WaitForCast(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                Spell.Cast("Raptor Strike", ret => StyxWoW.Me.CurrentTarget.DistanceSqr < 5 * 5),
                // Always keep it up on our target!
                Spell.Buff("Hunter's Mark"),
                // Heal pet when below 70
                Spell.Cast("Mend Pet", ret => StyxWoW.Me.Pet.HealthPercent < 70 && !StyxWoW.Me.Pet.HasAura("Mend Pet")),
                Spell.Cast(
                    "Concussive Shot",
                    ret => StyxWoW.Me.CurrentTarget.CurrentTarget == null || StyxWoW.Me.CurrentTarget.CurrentTarget == StyxWoW.Me),
                Spell.Cast("Arcane Shot"),
                Spell.Cast("Steady Shot"),
                Movement.CreateMoveToTargetBehavior(true, 30f)
                );
        }
    }
}
