using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class Shiv
    {
        public static bool CanRun
        {
            get
            {
                // Make sure we have the spell
                if (!StyxWoW.Me.CurrentTarget.IsWithinMeleeRange) return false;
                if (!Spell.HasCanSpell("Shiv")) return false;


                // Check for target has spells.
                if (StyxWoW.Me.CurrentTarget.HasAura("Enrage")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Wrecking Crew")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Savage Roar")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Unholy Frenzy")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Berserker Rage")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Death Wish")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Owlkin Frenzy")) return true;
                if (StyxWoW.Me.CurrentTarget.HasAura("Bastion of Defense")) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Shiv";
            return Spell.Cast("Shiv", StyxWoW.Me.CurrentTarget);
        }

    }
}
