using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PvPRogue.Utils
{
    public static class Misc
    {
        /// <summary>
        /// Dont know why this isnt in Honorbuddy ??
        /// Copyed over from my gatherbot
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <returns></returns>
        internal static double GetDistance2D(float X1, float Y1, float X2, float Y2)
        {
            return Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2));
        }
    }
}
