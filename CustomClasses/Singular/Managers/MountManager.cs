using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;

namespace Singular.Managers
{
    // This class is here to deal with Ghost Wolf/Travel Form usage for shamans and druids
    internal static class MountManager
    {
        internal static void Init()
        {
            Mount.OnMountUp += Mount_OnMountUp;
        }

        private static void Mount_OnMountUp(object sender, MountUpEventArgs e)
        {
            if (e.Destination == WoWPoint.Zero)
                return;

            if (e.Destination.DistanceSqr(StyxWoW.Me.Location) < 60*60)
            {
                if (SpellManager.HasSpell("Ghost Wolf") && TalentManager.GetCount(2,6) == 2)
                {
                    e.Cancel = true;

                    if (!StyxWoW.Me.HasAura("Ghost Wolf"))
                    {
                        Logger.Write("Using Ghost Wolf instead of mounting");
                        SpellManager.Cast("Ghost Wolf");
                    }

                }
                else if (SpellManager.HasSpell("Travel Form"))
                {
                    e.Cancel = true;

                    if (!StyxWoW.Me.HasAura("Travel Form"))
                    {
                        Logger.Write("Using Travel Form instead of mounting.");
                        SpellManager.Cast("Travel Form");
                    }
                }
            }
        }
    }
}
