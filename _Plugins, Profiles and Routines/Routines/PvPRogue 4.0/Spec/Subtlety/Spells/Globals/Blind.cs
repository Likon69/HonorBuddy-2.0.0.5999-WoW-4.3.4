using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals.WoWObjects;


namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class Blind
    {

        public static bool IfCanRun()
        {
            if (Spell.SpellOnCooldown("Blind")) return false;

            WoWPlayer HealerTarget = Helpers.BGHealers._Instance.BestHealer(15);

            if (HealerTarget == null) return false;

            Combat._LastMove = "Blind";
            return Spell.Cast("Blind", HealerTarget);
        }
    }
}
