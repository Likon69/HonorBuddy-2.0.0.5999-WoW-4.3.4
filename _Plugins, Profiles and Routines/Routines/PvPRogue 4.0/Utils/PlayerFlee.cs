using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic;


namespace PvPRogue.Utils
{
    class PlayerFlee
    {

        private static ulong _FleeGUID;
        private static Stopwatch _FleeSW = new Stopwatch();
        public static bool IsFleeing
        {
            get
            {
                // Movement Checker
                if (!ClassSettings._Instance.GeneralMovement) return false;

                if (StyxWoW.Me.CurrentTarget.Guid == _FleeGUID)
                {
                    // If we close enough just reset everything
                    if (StyxWoW.Me.CurrentTarget.IsWithinMeleeRange)
                    {
                        _FleeGUID = 0;
                        _FleeSW.Reset();
                        return false;
                    }

                    // Blacklist these noobs.
                    if (_FleeSW.Elapsed.Seconds > 15)
                    {
                        Blacklist.Add(StyxWoW.Me.CurrentTarget.Guid, TimeSpan.FromMinutes(1));
                    }

                    // If we been chasing for 5 secs return his running
                    if (_FleeSW.Elapsed.Seconds >= 3)
                    {
                        Log.Write("->Enemy Fleeing");
                        return true;
                    }
                }
                else
                {
                    _FleeGUID = StyxWoW.Me.CurrentTarget.Guid;
                    _FleeSW.Reset();
                    _FleeSW.Start();
                }
                return false;
            }
        }
    }
}
