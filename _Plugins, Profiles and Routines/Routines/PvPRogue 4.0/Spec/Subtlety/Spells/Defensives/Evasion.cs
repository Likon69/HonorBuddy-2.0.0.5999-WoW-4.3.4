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
    public static class Evasion
    {
        public static bool CanRun
        {
            get
            {
                // If im getting targeted by 2 or more people evasion
                if (
                    StyxWoW.Me.HealthPercent < 65
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Evasion";
            return Spell.Cast("Evasion");
        }

    }
}
