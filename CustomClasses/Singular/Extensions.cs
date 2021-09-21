#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: apoc $
// $Date: 2011-09-06 02:31:34 -0400 (Tue, 06 Sep 2011) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Extensions.cs $
// $LastChangedBy: apoc $
// $LastChangedDate: 2011-09-06 02:31:34 -0400 (Tue, 06 Sep 2011) $
// $LastChangedRevision: 361 $
// $Revision: 361 $

#endregion

using System.Text;
using Styx;
using Styx.Logic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Singular
{
    internal static class Extensions
    {

        public static bool Between(this double distance, double min, double max)
        {
            return distance >= min && distance <= max;
        }

        /// <summary>
        ///   A string extension method that turns a Camel-case string into a spaced string. (Example: SomeCamelString -> Some Camel String)
        /// </summary>
        /// <remarks>
        ///   Created 2/7/2011.
        /// </remarks>
        /// <param name = "str">The string to act on.</param>
        /// <returns>.</returns>
        public static string CamelToSpaced(this string str)
        {
            var sb = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsUpper(c))
                {
                    sb.Append(' ');
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string SafeName(this WoWObject obj)
        {
            if (obj.IsMe)
            {
                return "Myself";
            }

            string name;
            if (obj is WoWPlayer)
            {
                if (RaFHelper.Leader == obj)
                    return "Tank";

                name = ((WoWPlayer)obj).Class.ToString();
            }
            else if (obj is WoWUnit && obj.ToUnit().IsPet)
            {
                name = "Pet";
            }
            else
            {
                name = obj.Name;
            }

            return name;
        }

        public static bool IsWanding(this LocalPlayer me)
        {
            return StyxWoW.Me.AutoRepeatingSpellId == 5019;
        }

        //0x9F8+4
        public static bool CanInterrupt(this WoWUnit u)
        {
            return (ObjectManager.Wow.Read<uint>(u.BaseAddress + 0x9F8 + 0x4) & 8) != 0;
        }
    }
}