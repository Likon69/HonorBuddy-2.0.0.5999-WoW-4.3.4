using System;
using System.Collections.Generic;

using Styx.Logic.Combat;
using Styx.WoWInternals;

namespace CLU.Helpers
{
    using System.Drawing;

    using Styx.Helpers;

    public class WoWStats
    {
        private static readonly WoWStats WoWStatsInstance = new WoWStats();


        private ulong spellCasts;
        private Dictionary<string, int> spellList;
        private Dictionary<string, List<DateTime>> spellInterval;
        private DateTime start;

        /// <summary>
        /// An instance of the WoWStats Class
        /// </summary>
        public static WoWStats Instance { get { return WoWStatsInstance; } }


        public void ClearStats()
        {
                this.spellInterval.Clear();
                this.spellList.Clear();
                this.spellCasts = 0;
                this.start = DateTime.Now;
        }

        public void WoWStatsOnStarted(object o)
        {
            Lua.Events.AttachEvent("UNIT_SPELLCAST_SUCCEEDED", this.UNIT_SPELLCAST_SUCCEEDED);
            CLU.Instance.Log("WoWStats: Connected to the Grid");
            this.spellCasts = 0;
            this.spellList = new Dictionary<string, int>();
            this.spellInterval = new Dictionary<string, List<DateTime>>();
            this.start = DateTime.Now;
        }

        public void WoWStatsOnStopped(object o)
        {
            Lua.Events.DetachEvent("UNIT_SPELLCAST_SUCCEEDED", UNIT_SPELLCAST_SUCCEEDED);
        }

        private void UNIT_SPELLCAST_SUCCEEDED(object sender, LuaEventArgs raw)
        {
            var args = raw.Args;
            var player = Convert.ToString(args[0]);

            if (player != "player")
            {
                    return;
            }

            // get the english spell name, not the localized one!
            var spellID = Convert.ToInt32(args[4]);
            var spell = WoWSpell.FromId(spellID).Name;

            // initialize or increment the count for this item
            try
            {
                spellList[spell]++;
            }
            catch
            {
                spellList[spell] = spellList.ContainsKey(spell) ? spellList[spell]++ : 1;
            }

            spellCasts++;

            if (!spellInterval.ContainsKey(spell))
               spellInterval[spell] = new List<DateTime>();

            if (!spellInterval[spell].Contains(DateTime.Now))
            {
                 // CLU.Instance.Log("Adding " + DateTime.Now + " for " + spell);
                 spellInterval[spell].Add(DateTime.Now);
            }

        }

        public void PrintReport()
        {
            // var spells = spellList.OrderByDescending(x => x.Value);
            // var seconds = DateTime.Now.Subtract(start).TotalSeconds;
            var minutes = DateTime.Now.Subtract(start).TotalMinutes;

            var apm = (int)(this.spellCasts / minutes);

            CLU.Instance.Log("CLU stats report:");
            CLU.Instance.Log("Runtime: {0} minutes", Math.Round(minutes * 10.0) / 10.0);
            CLU.Instance.Log("Spells cast: {0}", spellCasts > 1000 ? ((Math.Round(spellCasts / 100.0) * 10) + "k") : spellCasts.ToString());
            CLU.Instance.Log("Average APM: {0}", apm);
            CLU.Instance.Log("------------------------------------------");
            foreach (KeyValuePair<string, int> spell in spellList)
            {
                CLU.Instance.Log(spell.Key + " was cast " + spell.Value + " time(s).");
            }
            CLU.Instance.Log("------------------------------------------");
            foreach (KeyValuePair<string, List<DateTime>> spell in spellInterval)
            {
                var lastInterval = start;
                var output = "0 ";

                for (int x = 0; x < spell.Value.Count - 1; ++x)
                {
                    var interval = spell.Value[x];
                    var difference = interval - lastInterval;
                    output = output + string.Format(", {0} ", Math.Round(difference.TotalSeconds * 100.0) / 100.0);
                    lastInterval = interval;
                }

                CLU.Instance.Log(spell.Key + " intervals: ");
                Logging.Write(Color.Aqua, " " + output);

            }
            CLU.Instance.Log("------------------------------------------");
        }
    }
}

