using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;

namespace PvPRogue.Spec
{
    public static class AutoAttack
    {
        public static bool CanRun
        {
            get
            {
                if (
                    !StyxWoW.Me.IsAutoAttacking &&
                    StyxWoW.Me.CurrentTarget.IsWithinMeleeRange &&
                    !StyxWoW.Me.HasAura("Stealth") &&
                    Spec.Subtlety.Combat._LastMove != "Ambush"
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Log.Write("Auto Attack Enabled");
            StyxWoW.Me.ToggleAttack();
            return true;
        }
    }
}
