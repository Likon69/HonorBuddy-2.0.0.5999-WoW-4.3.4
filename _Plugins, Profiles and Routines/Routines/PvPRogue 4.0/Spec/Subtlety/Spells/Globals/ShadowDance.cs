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
    public static class ShadowDance
    {
        public static bool CanRun
        {
            get
            {
                if ( 
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    StyxWoW.Me.CurrentTarget.MeIsBehind &&
                    StyxWoW.Me.HealthPercent > 70 &&
                    !StyxWoW.Me.CurrentTarget.IsMoving &&
                    Spell.HasMyAuraTimeLeft("Recuperate") > 5000 &&
                    !Spell.SpellOnCooldown("Shadow Dance")
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            
            Combat._LastMove = "Shadow Dance";
            return Spell.Cast("Shadow Dance", StyxWoW.Me.CurrentTarget);
        }

        public static bool EnergySave()
        {
            if (StyxWoW.Me.EnergyPercent < 75) return true;
            return false;
        }
    }
}
