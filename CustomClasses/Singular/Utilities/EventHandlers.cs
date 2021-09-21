using System;
using System.Collections.Generic;
using Singular.Helpers;
using Singular.Managers;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Singular.Utilities
{
    public static class EventHandlers
    {
        public static void Init()
        {
            if (SingularRoutine.CurrentWoWContext != WoWContext.Battlegrounds &&
                !StyxWoW.Me.CurrentMap.IsRaid)
                AttachCombatLogEvent();
        }

        internal static void PlayerOnMapChanged(BotEvents.Player.MapChangedEventArgs args)
        {
            // Since we hooked this in ctor, make sure we are the selected CC
            if (RoutineManager.Current.Name != SingularRoutine.Instance.Name)
                return;

            if (SingularRoutine.CurrentWoWContext == WoWContext.Battlegrounds ||
                StyxWoW.Me.CurrentMap.IsRaid)
                DetachCombatLogEvent();
            else
                AttachCombatLogEvent();

            //Why would we create same behaviors all over ?
            if (SingularRoutine.LastWoWContext == SingularRoutine.CurrentWoWContext)
            {
                return;
            }

            Logger.Write("Context changed. New context: " + SingularRoutine.CurrentWoWContext + ". Rebuilding behaviors.");
            SingularRoutine.Instance.CreateBehaviors();
        }

        private static bool _combatLogAttached;
        private static void AttachCombatLogEvent()
        {
            if (_combatLogAttached)
                return;

            // DO NOT EDIT THIS UNLESS YOU KNOW WHAT YOU'RE DOING!
            // This ensures we only capture certain combat log events, not all of them.
            // This saves on performance, and possible memory leaks. (Leaks due to Lua table issues.)
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
            if (
                !Lua.Events.AddFilter(
                    "COMBAT_LOG_EVENT_UNFILTERED",
                    "return args[2] == 'SPELL_CAST_SUCCESS' or args[2] == 'SPELL_AURA_APPLIED' or args[2] == 'SPELL_MISSED' or args[2] == 'RANGE_MISSED' or args[2] =='SWING_MISSED'"))
            {
                Logger.Write("ERROR: Could not add combat log event filter! - Performance may be horrible, and things may not work properly!");
            }

            Logger.WriteDebug("Attached combat log");
            _combatLogAttached = true;
        }

        private static void DetachCombatLogEvent()
        {
            if (!_combatLogAttached)
                return;

            Logger.WriteDebug("Detached combat log");
            Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
            _combatLogAttached = false;
        }

        private static void HandleCombatLog(object sender, LuaEventArgs args)
        {
            var e = new CombatLogEventArgs(args.EventName, args.FireTimeStamp, args.Args);
            //Logger.WriteDebug("[CombatLog] " + e.Event + " - " + e.SourceName + " - " + e.SpellName);
            switch (e.Event)
            {
                case "SPELL_AURA_APPLIED":
                case "SPELL_CAST_SUCCESS":
                    if (e.SourceGuid != StyxWoW.Me.Guid)
                    {
                        return;
                    }

                    // Update the last spell we cast. So certain classes can 'switch' their logic around.
                    Spell.LastSpellCast = e.SpellName;
                    //Logger.WriteDebug("Successfully cast " + Spell.LastSpellCast);

                    // Force a wait for all summoned minions. This prevents double-casting it.
                    if (SingularRoutine.MyClass == WoWClass.Warlock && e.SpellName.StartsWith("Summon "))
                    {
                        StyxWoW.SleepForLagDuration();
                    }
                    break;

                case "SPELL_MISSED":
                case "RANGE_MISSED":
                case "SWING_MISSED":
                    if (e.Args[14].ToString() == "EVADE")
                    {
                        Logger.Write("Mob is evading. Blacklisting it!");
                        Blacklist.Add(e.DestGuid, TimeSpan.FromMinutes(30));
                        if (StyxWoW.Me.CurrentTargetGuid == e.DestGuid)
                        {
                            StyxWoW.Me.ClearTarget();
                        }

                        BotPoi.Clear("Blacklisting evading mob");
                        StyxWoW.SleepForLagDuration();
                    }
                    else if (e.Args[14].ToString() == "IMMUNE")
                    {
                        WoWUnit unit = e.DestUnit;
                        if (unit != null && !unit.IsPlayer)
                        {
                            Logger.WriteDebug("{0} is immune to {1} spell school", unit.Name,e.SpellSchool);
                            SpellImmunityManager.Add(unit.Entry, e.SpellSchool);
                        }
                    }
                    break;
            }
        }
    }
}
