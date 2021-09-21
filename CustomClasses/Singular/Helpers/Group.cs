using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;

namespace Singular.Helpers
{
    internal static class Group
    {
        public static bool MeIsTank
        {
            get
            {
                return (StyxWoW.Me.Role & WoWPartyMember.GroupRole.Tank) != 0 || 
                        Tanks.All(t => !t.IsAlive) && StyxWoW.Me.HasAura("Bear Form");
            }
        }

        public static bool MeIsHealer
        {
            get { return (StyxWoW.Me.Role & WoWPartyMember.GroupRole.Healer) != 0; }
        }

        public static List<WoWPlayer> Tanks
        {
            get
            {
                var result = new List<WoWPlayer>();

                if (!StyxWoW.Me.IsInParty)
                    return result;

                if ((StyxWoW.Me.Role & WoWPartyMember.GroupRole.Tank) != 0)
                    result.Add(StyxWoW.Me);

                var members = StyxWoW.Me.IsInRaid ? StyxWoW.Me.RaidMemberInfos : StyxWoW.Me.PartyMemberInfos;

                var tanks = members.Where(p => (p.Role & WoWPartyMember.GroupRole.Tank) != 0);

                result.AddRange(tanks.Where(t => t.ToPlayer() != null).Select(t => t.ToPlayer()));

                return result;
            }
        }

        public static List<WoWPlayer> Healers
        {
            get
            {
                var result = new List<WoWPlayer>();

                if (!StyxWoW.Me.IsInParty)
                    return result;

                if ((StyxWoW.Me.Role & WoWPartyMember.GroupRole.Healer) != 0)
                    result.Add(StyxWoW.Me);

                var members = StyxWoW.Me.IsInRaid ? StyxWoW.Me.RaidMemberInfos : StyxWoW.Me.PartyMemberInfos;

                var tanks = members.Where(p => (p.Role & WoWPartyMember.GroupRole.Healer) != 0);

                result.AddRange(tanks.Where(t => t.ToPlayer() != null).Select(t => t.ToPlayer()));

                return result;
            }
        }

        /// <summary>Gets a player by class priority. The order of which classes are passed in, is the priority to find them.</summary>
        /// <remarks>Created 9/9/2011.</remarks>
        /// <param name="range"></param>
        /// <param name="includeDead"></param>
        /// <param name="classes">A variable-length parameters list containing classes.</param>
        /// <returns>The player by class prio.</returns>
        public static WoWUnit GetPlayerByClassPrio(float range, bool includeDead, params WoWClass[] classes)
        {
            foreach (var woWClass in classes)
            {

                var unit =
                    StyxWoW.Me.PartyMemberInfos.FirstOrDefault(
                        p => p.ToPlayer() != null && p.ToPlayer().Distance < range && p.ToPlayer().Class == woWClass);

                if (unit != null)
                    if (!includeDead && unit.Dead || unit.Ghost)
                        return unit.ToPlayer();
            }
            return null;
        }
    }
}
