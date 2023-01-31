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
    class Eviscerate
    {
        //Eviscerate 
        public static bool CanRun
        {
            get
            {
                if (ClassSettings._Instance.SubtletyFinisher != eSubFinisher.Eviscerate)
                {
                    if (!Spell.SpellOnCooldown("Kidney Shot"))
                        return false;
                }

                // if we got 5 combo points, well we got to do it anyways
                if (StyxWoW.Me.RawComboPoints >= 5) return true;

                if (
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    StyxWoW.Me.RawComboPoints > 1 &&
                    Spell.HasMyAuraTimeLeft("Recuperate") > 3000 &&
                    Spell.HasMyAuraTimeLeft("Recuperate") < 4000
                    ) return true;


                return false;
            }
        }   

        public static bool Run()
        {
            Combat._LastMove = "Eviscerate";
            return Spell.Cast("Eviscerate", StyxWoW.Me.CurrentTarget);
        }



        public static bool CanRunLow
        {
            get
            {
                // if we got 5 combo points, well we got to do it anyways
                if (StyxWoW.Me.RawComboPoints >= 5) return true;


                // High level
                if (
                    StyxWoW.Me.CurrentTarget.HealthPercent < 35 &&
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    StyxWoW.Me.RawComboPoints >= 4
                    ) return true;


                return false;
            }
        }
    }
}
