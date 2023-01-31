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
    class Kick
    {
        public static bool CanRun
        {
            get
            {
                if (
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    StyxWoW.Me.CurrentTarget.IsCasting &&
                    StyxWoW.Me.CurrentTarget.CanInterruptCurrentSpellCast
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Kick";
            return Spell.Cast("Kick", StyxWoW.Me.CurrentTarget);
        }
    }
}
