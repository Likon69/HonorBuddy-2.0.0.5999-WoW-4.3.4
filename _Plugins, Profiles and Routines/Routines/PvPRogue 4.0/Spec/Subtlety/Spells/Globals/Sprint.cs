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
    class Sprint
    {

        public static bool CanRun
        {
            get
            {
                if (Utils.PlayerFlee.IsFleeing) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Sprint";
            return Spell.Cast("Sprint");
        }
    }
}
