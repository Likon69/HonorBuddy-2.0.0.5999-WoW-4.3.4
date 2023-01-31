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
    public static class MainMove
    {

        public static string GetName
        {
            // Hemorrhage - [Sub Spec] [ infront ] - Must have up at all times
            // Backstab - 18 [ Behind ]
            // Sinister Strike - lowbie 
            get
            {
                // Frame lock it, as we are calling CanCast.
                // First check if we need to re-apply the bleed
                if (Spell.HasCanSpell("Hemorrhage") && !Spell.HasMyAura("Hemorrhage", StyxWoW.Me.CurrentTarget))
                    return "Hemorrhage";

                // Behind Check
                if (Spell.HasCanSpell("Backstab") && StyxWoW.Me.CurrentTarget.MeIsSafelyBehind)
                    return "Backstab";

                if (SpellManager.HasSpell("Backstab") && StyxWoW.Me.CurrentTarget.MeIsSafelyBehind) return "";

                // Check if we can cast hemorrhage
                if (Spell.HasCanSpell("Hemorrhage"))
                    return "Hemorrhage";                    

                // Last resort is Sinister Strike
                return "Sinister Strike";
            }
        }
    }
}
