using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Utils
{
    class SafeChecks
    {
        private static LocalPlayer Me = StyxWoW.Me;

        public static bool TargetSafe
        {
            get
            {
                if (ObjectManager.Me.GotTarget == false) return false;
                if (ObjectManager.Me.CurrentTarget.IsAlive == false) return false;

                return true;
            }
        }

        public static bool CombatReady
        {
            get
            {
                if (StyxWoW.GlobalCooldown) return false;
                if (ObjectManager.Me.IsCasting) return false;
                if (ObjectManager.Me.ChanneledCastingSpellId != 0) return false;
                if (ObjectManager.Me.Stunned) return false;
                if (ObjectManager.Me.Dead) return false;
                if (ObjectManager.Me.Mounted) return false;

                return true;
            }
        }
    }
}
