using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Logic;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Spec.Subtlety.Spells
{
    public static class Vanish
    {
        public static bool CanRun
        {
            get
            {
                // if target is fleeing and we dont have sprint and we are able to cast.
                if (Spell.HasCanSpell("Shadowstep") && 
                    Utils.PlayerFlee.IsFleeing)
                    return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Vanish";
            return Spell.Cast("Vanish");
        }
    }
}
