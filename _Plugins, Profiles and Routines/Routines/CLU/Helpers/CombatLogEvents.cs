using System;
using System.Collections.Generic;

using Styx;

namespace CLU.Helpers
{
    using Styx.Logic.Combat;
    using Styx.WoWInternals;

    using global::CLU.GUI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CombatLogEvents
    {
        private static readonly CombatLogEvents CombatLogEventsInstance = new CombatLogEvents();

        private CombatLogEvents()
        {
            CLU.Instance.Log("CombatLogEvents: OnStarted");
            BotEvents.OnBotStarted += OnStarted;
            BotEvents.OnBotStopped += OnStopped;
        }

        /// <summary>
        /// An instance of the CombatLogEvents Class
        /// </summary>
        public static CombatLogEvents Instance { get { return CombatLogEventsInstance; } }

        public static readonly Dictionary<string, DateTime> Locks = new Dictionary<string, DateTime>();

        public static readonly double ClientLag = SettingsFile.Instance.AutoManageClientLag ? 1 : StyxWoW.WoWClient.Latency * 2 / 1000.0;

        private void OnStarted(object o)
        {
            CLU.Instance.Log("CombatLogEvents: Connected to the Grid");

            // Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", UpdateActiveRotation);
            // Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", UpdateActiveRotation)
            
            // means spell was cast (did not hit target yet)
            Lua.Events.AttachEvent("UNIT_SPELLCAST_SUCCEEDED", OnSpellFired_ACK);

            // user got stunned, silenced, kicked...
            Lua.Events.AttachEvent("UNIT_SPELLCAST_INTERRUPTED", OnSpellFired_NACK);

            // misc fails, due to stopcast, spell spam, etc.
            Lua.Events.AttachEvent("UNIT_SPELLCAST_FAILED", OnSpellFired_FAIL);
            Lua.Events.AttachEvent("UNIT_SPELLCAST_FAILED_QUIET", OnSpellFired_FAIL);
            Lua.Events.AttachEvent("UNIT_SPELLCAST_STOP", OnSpellFired_FAIL);
        }

        private void OnStopped(object o)
        {
            // Lua.Events.DetachEvent("CHARACTER_POINTS_CHANGED", UpdateActiveRotation);
            // Lua.Events.DetachEvent("ACTIVE_TALENT_GROUP_CHANGED", UpdateActiveRotation)

            Lua.Events.DetachEvent("UNIT_SPELLCAST_SUCCEEDED", OnSpellFired_ACK);

            Lua.Events.DetachEvent("UNIT_SPELLCAST_INTERRUPTED", OnSpellFired_NACK);

            Lua.Events.DetachEvent("UNIT_SPELLCAST_FAILED", OnSpellFired_FAIL);
            Lua.Events.DetachEvent("UNIT_SPELLCAST_FAILED_QUIET", OnSpellFired_FAIL);
            Lua.Events.DetachEvent("UNIT_SPELLCAST_STOP", OnSpellFired_FAIL);

        }

        private void UpdateActiveRotation(object sender, LuaEventArgs args)
        {
            // CLU.rotationBase = null;
        }

 	    private void OnSpellFired_ACK(object sender, LuaEventArgs raw)
        {
            OnSpellFired(true, true, raw);
        }

        private void OnSpellFired_NACK(object sender, LuaEventArgs raw)
        {
            OnSpellFired(false, true, raw);
        }

        private void OnSpellFired_FAIL(object sender, LuaEventArgs raw)
        {
            OnSpellFired(false, false, raw);
        }

        private void OnSpellFired (bool success, bool spellCast, LuaEventArgs raw)
        {
            var args = raw.Args;
            var player = Convert.ToString(args[0]);

            if (player != "player")
            {
                return;
            }

            // get the english spell name, not the localized one!
            var spellID = Convert.ToInt32(args[4]);
            var spellName = WoWSpell.FromId(spellID).Name;

            if (!success && spellCast)
            {
                CLU.Instance.Log("Woops, '{0}' cast failed: {1}", spellName, raw.EventName);
            }
             
            // if the spell is locked, let's extend it (spell travel time + client lag) / or reset it...
            if (Locks.ContainsKey(spellName))
            {
                if (success)
                {
                    // yay!
                    Locks[spellName] = DateTime.Now.AddSeconds(ClientLag + 4.0);
                }
                else
                {
                    if (spellCast)
                    {
                        // interrupted while casting
                        Locks[spellName] = DateTime.Now;
                    }
                    else
                    {
                        // failed to cast it. moar spam!
                        Locks[spellName] = DateTime.Now;
                    }
                }
            }
        }
        

        public Dictionary<string, double> DumpSpellLocks()
        {
            var ret = new Dictionary<string, double>();
            var now = DateTime.Now;

            foreach (var x in Locks)
            {
                var s = x.Value.Subtract(now).TotalSeconds;
                if (s < 0) s = 0;
                s = Math.Round(s, 3);
                ret[x.Key] = s;
            }

            return ret;
        } 
    }
}
