using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;

namespace Singular.ClassSpecific.Warrior
{
    static class Common
    {
        private static readonly WaitTimer ChargeTimer = new WaitTimer(TimeSpan.FromMilliseconds(2000));

        public static bool PreventDoubleCharge
        {
            get
            {
                var tmp = ChargeTimer.IsFinished;
                if (tmp)
                    ChargeTimer.Reset();
                return tmp;
            }
        }
    }
}
