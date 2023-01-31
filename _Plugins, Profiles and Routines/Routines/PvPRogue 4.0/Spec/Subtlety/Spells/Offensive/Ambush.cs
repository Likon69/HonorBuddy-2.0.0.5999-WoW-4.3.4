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
    public static class Ambush
    {
        public static bool CanRun
        {
            get
            {
                if (
                    ObjectManager.Me.HasAura("Shadow Dance") &&
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    StyxWoW.Me.CurrentTarget.MeIsSafelyBehind
                    ) return true;

                // Incase we get behind em! ambush the kunts!
                if (
                    ObjectManager.Me.HasAura("Stealth") &&
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    StyxWoW.Me.CurrentTarget.MeIsSafelyBehind
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Ambush";
            return Spell.Cast("Ambush", StyxWoW.Me.CurrentTarget);
        }
    }
}
