using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;

namespace PvPRogue.Managers
{
    class Energy
    {
        /// <summary>
        /// Our energy to hold @ when not bursting
        /// </summary>
        public static int nEnergy = 60;

        /// <summary>
        /// Defines if we are going threw a bursting phase
        /// </summary>
        public static bool IsBursting
        {
            get
            {
                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if (Spell.HasMyAuraTimeLeft("Garrote", Target) > 3000) return true;     // Opener 
                if (Spell.HasMyAuraTimeLeft("Cheap Shot", Target) > 0) return true;     // Opener
                if (Spell.HasMyAuraTimeLeft("Shadow Dance", Target) > 0) return true;   // Dance -> use all energy!

                if (!SpellManager.HasSpell("Shadow Dance")) return true;                // Since were low level, lets always burst
                if (Spell.SpellOnCooldown("Shadow Dance")) return true;                 // If its on cooldown lets just burst!
                return false;
            }
        }
    }
}
