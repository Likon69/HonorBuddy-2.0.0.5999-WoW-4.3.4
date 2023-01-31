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
    public static class Shadowstep
    {
        public static bool CanRun
        {
            get
            {
                // If we need to use this for our Shadow Dance to do more damage, lets do it!
                if (StyxWoW.Me.CurrentTarget.HasAura("Find Weakness") && 
                    StyxWoW.Me.HasAura("Shadow Dance") && 
                    StyxWoW.Me.CurrentTarget.InLineOfSpellSight
                    ) return true;


                // if target is fleeing and we dont have sprint and we are able to cast.
                if (Utils.PlayerFlee.IsFleeing &&
                    !Spell.HasMyAura("Sprint") && 
                    StyxWoW.Me.CurrentTarget.InLineOfSpellSight
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Shadowstep";
            return Spell.Cast("Shadowstep", StyxWoW.Me.CurrentTarget);
        }
    }
}
