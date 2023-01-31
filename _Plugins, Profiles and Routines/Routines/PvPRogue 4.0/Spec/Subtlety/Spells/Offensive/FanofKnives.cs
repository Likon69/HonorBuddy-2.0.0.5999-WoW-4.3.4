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

namespace PvPRogue.Spec.General
{
    public static class FanofKnives
    {
        public static bool CanRun
        {
            get
            {
                if (Managers.PlayerObjects.EnemysAround(8) > ClassSettings._Instance.MovesFOKPeople) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Subtlety.Combat._LastMove = "Fan of Knives";
            return Spell.Cast("Fan of Knives");
        }
    }
}


