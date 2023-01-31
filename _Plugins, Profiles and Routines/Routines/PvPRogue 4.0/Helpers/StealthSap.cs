using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Helpers;
using Styx.Logic;

namespace PvPRogue.Helpers
{
    internal static class StealthSap
    {
        internal static void Pulse()
        {
            if (StyxWoW.Me.IsActuallyInCombat) return;
            if (!StyxWoW.Me.IsStealthed) return;
            if (!Battlegrounds.IsInsideBattleground) return;
            if (!Utils.SafeChecks.CombatReady) return;

            WoWUnit SapThisFuck = (from Enemys in ObjectManager.GetObjectsOfType<WoWPlayer>(true, false)
                       where Enemys.IsStealthed &&
                       Enemys.Distance <= 10 &&
                       !Enemys.IsFriendly &&
                       !Enemys.HasAura("Sap")
                       select Enemys).FirstOrDefault();

            Spell.Cast("Sap", SapThisFuck);
        }
    }
}
