using System.Collections;
using System.Collections.Generic;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;

namespace Zerfall
{
    public static class Extensions
    {

        /// <summary>
        ///   Determine whether a class is 'squishy'. AKA: Whether it should have thorns buffed, and other things.
        /// </summary>
        /// <param name = "unit"></param>
        /// <returns></returns>
        public static bool IsSquishy(this WoWUnit unit)
        {
            switch (unit.Class)
            {
                case WoWClass.Warrior:
                case WoWClass.Paladin:
                case WoWClass.DeathKnight:
                case WoWClass.Shaman:
                case WoWClass.Druid:
                    return false;
                default:
                    return true;
            }
        }

        public static bool ContainsAny(this IEnumerable<string> enumer, IEnumerable<string> list)
        {
            foreach (var str in enumer)
            {
                if (str.ContainsAny(list))
                    return true;
            }
            return false;
        }

        public static bool ContainsAny(this string str, IEnumerable<string> list)
        {
            foreach (string s in list)
            {
                if (str.Contains(s))
                    return true;
            }
            return false;
        }

        public static string ToRealString(this IList lst)
        {
            var tmp = new List<string>();
            if (lst == null)
            {
                return "NULL";
            }
            if (lst.Count == 0)
            {
                return "NULL";
            }
            foreach (object s in lst)
            {
                try
                {
                    if (s.Equals(null))
                    {
                        tmp.Add("NULL");
                        continue;
                    }
                }
                catch
                {
                    tmp.Add("NULL");
                    continue;
                }
                tmp.Add(s.ToString());
            }
            return string.Join(",", tmp.ToArray());
        }

        public static string ToRealString<T>(this T[] lst)
        {
            var tmp = new List<T>();
            if (lst == null)
            {
                return "NULL";
            }
            tmp.AddRange(lst);
            return tmp.ToRealString();
        }
    }
}
