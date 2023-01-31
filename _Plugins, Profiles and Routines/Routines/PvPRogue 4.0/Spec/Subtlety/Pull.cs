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

namespace PvPRogue.Spec.Subtlety
{
    public static class Pull
    {
        public static void Pulse()
        {
            // Lets open!
            if (Spell.HasCanSpell("Shadowstep") && !Battlegrounds.IsInsideBattleground && ClassSettings._Instance.PVEStep) Spell.Cast("Shadowstep", StyxWoW.Me.CurrentTarget);
            Spell.Cast(OpenerName, StyxWoW.Me.CurrentTarget);               
        }

        /// <summary>
        /// Gets the spellname for the opener
        /// </summary>
        /// <returns>SpellName</returns>
        public static string OpenerName
        {
            get
            {
                // Frame lock it, as we are calling CanCast.
                using (new FrameLock())
                {
                    // Garrote
                    if (ClassSettings._Instance.SubtletyOpener == eSubOpener.Garrote)
                    {
                        if (Spell.HasCanSpell("Garrote"))
                            return "Garrote";
                    }

                    // Last resort is ambush
                    // if they dont have this, they seriously need to quit life.
                    return "Ambush";
                }
            }
        }
    }
}
