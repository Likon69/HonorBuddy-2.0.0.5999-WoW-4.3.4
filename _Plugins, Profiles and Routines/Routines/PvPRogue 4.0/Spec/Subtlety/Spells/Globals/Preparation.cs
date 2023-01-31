using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Logic.Combat;

namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class Preparation
    {
        public static bool CanRun
        {
            get
            {
                if (
                    Spell.SpellOnCooldown("Shadowstep") &&
                    Spell.SpellOnCooldown("Sprint")
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Preparation";
            return Spell.Cast("Preparation");
        }
    }
}
