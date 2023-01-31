using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Styx;
using Styx.Helpers;

namespace PvPRogue
{
    public static class Log
    {
        public static Color LogColour = Color.Cyan;
        public static string PreText = "[SwInY - PvPRogue] ";

        public static void Write(string Value, params object[] args)
        {
            Value = string.Format(Value, args);

            Logging.Write(LogColour, PreText + Value);
            Logging.WriteDebug(LogColour, PreText + Value);
        }

        public static void WriteDebug(string Value, params object[] args)
        {
            Value = string.Format(Value, args);

            Logging.WriteDebug(LogColour, PreText + Value);
        }
    }
}
