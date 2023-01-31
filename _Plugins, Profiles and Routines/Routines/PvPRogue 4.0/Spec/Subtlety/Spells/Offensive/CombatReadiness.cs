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
    public static class CombatReadiness
    {
         public static bool CanRun
        {
            get
            {
                if (
                    StyxWoW.Me.HealthPercent < 75 &&
                    Managers.PlayerObjects.MeleeTargeting >= 2
                    ) return true;
               
                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Combat Readiness";
            return Spell.Cast("Combat Readiness");
        }
    }
}
