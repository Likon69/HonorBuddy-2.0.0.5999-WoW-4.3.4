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
    public static class Dismantle
    {
        public static bool CanRun
        {
            get
            {
                // Make sure we have the spell
                if (!StyxWoW.Me.CurrentTarget.IsWithinMeleeRange) return false;
                if (!StyxWoW.Me.CurrentTarget.IsPlayer) return false;

                // Class's we want to dismantle
                if (StyxWoW.Me.CurrentTarget.Class == WoWClass.DeathKnight) return true;
                if (StyxWoW.Me.CurrentTarget.Class == WoWClass.Hunter) return true;
                if (StyxWoW.Me.CurrentTarget.Class == WoWClass.Rogue) return true;
                if (StyxWoW.Me.CurrentTarget.Class == WoWClass.Warrior) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Dismantle";
            return Spell.Cast("Dismantle", StyxWoW.Me.CurrentTarget);
        }
    }
}
