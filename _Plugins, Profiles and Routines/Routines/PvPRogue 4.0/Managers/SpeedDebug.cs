using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Styx;
using Styx.Helpers;

namespace PvPRogue.Managers
{
    internal static class SpeedDebug
    {
        private static List<int> PulseLength = new List<int>();
        private static int Over500 = 0;

        internal static void Pulse(int MS)
        {
            PulseLength.Add(MS);

            if (PulseLength.Count() > 1000)
            {
                int Highest = (from CurMS in PulseLength
                               orderby CurMS descending
                               select CurMS).FirstOrDefault();

                int Lowest = (from CurMS in PulseLength
                              orderby CurMS ascending
                              select CurMS).FirstOrDefault();


                PulseLength = (from CurMS in PulseLength
                               orderby CurMS descending
                               select CurMS).ToList();

                int Total = 0;
                int Average = 0;
                int Count = 0;
                foreach (int CurInt in PulseLength)
                {
                    if (Count <= 10)
                    {
                        Logging.Write(Color.Red, "[PvPRogue Debug] - {0} Highest: {1}", Count, CurInt);
                        Logging.WriteDebug(Color.Red, "[PvPRogue Debug] - {0} Highest: {1}", Count, CurInt);
                        Count++;
                    }

                    Total += CurInt;
                    if (CurInt >= 500) Over500++;
                }
                Average = Total / PulseLength.Count();


                Logging.Write(Color.Red, "[PvPRogue Debug] - Highest: {0}  Lowest: {1}  Average: {2}  Over 500MS: {3}", Highest, Lowest, Average, Over500);
                Logging.WriteDebug(Color.Red, "[PvPRogue Debug] - Highest: {0}  Lowest: {1}  Average: {2}  Over 500MS: {3}", Highest, Lowest, Average, Over500);


                PulseLength.Clear();
                Over500 = 0;
            }
        }
    }
}
