#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: raphus $
// $Date: 2012-01-23 23:09:45 +0200 (Pzt, 23 Oca 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Managers/HealerManager.cs $
// $LastChangedBy: raphus $
// $LastChangedDate: 2012-01-23 23:09:45 +0200 (Pzt, 23 Oca 2012) $
// $LastChangedRevision: 567 $
// $Revision: 567 $

#endregion

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Singular.Settings;

using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Singular.Managers
{
    /*
     * Targeting works like so, in order of being called
     * 
     * GetInitialObjectList - Return a list of initial objects for the targeting to use.
     * RemoveTargetsFilter - Remove anything that doesn't belong in the list.
     * IncludeTargetsFilter - If you want to include units regardless of the remove filter
     * WeighTargetsFilter - Weigh each target in the list.     
     *
     */

    internal class HealerManager : Targeting
    {
        private static readonly WaitTimer _tankReset = WaitTimer.ThirtySeconds;

        private static ulong _tankGuid;

        static HealerManager()
        {
            // Make sure we have a singleton instance!
            Instance = new HealerManager();
        }

        public new static HealerManager Instance { get; private set; }

        public static bool NeedHealTargeting { get; set; }

        public List<WoWPlayer> HealList { get { return ObjectList.ConvertAll(o => o.ToPlayer()); } }

        protected override List<WoWObject> GetInitialObjectList()
        {
            // Targeting requires a list of WoWObjects - so it's not bound to any specific type of object. Just casting it down to WoWObject will work fine.
            return ObjectManager.ObjectList.Where(o => o is WoWPlayer).ToList();
        }

        protected override void DefaultIncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            foreach (WoWObject incomingUnit in incomingUnits)
            {
                outgoingUnits.Add(incomingUnit);
            }
            //if (!outgoingUnits.Any(o => o.IsMe))
            //    outgoingUnits.Add(StyxWoW.Me);
        }

        protected override void DefaultRemoveTargetsFilter(List<WoWObject> units)
        {
            bool isHorde = StyxWoW.Me.IsHorde;
            for (int i = units.Count - 1; i >= 0; i--)
            {
                WoWObject o = units[i];
                if (!(o is WoWPlayer))
                {
                    units.RemoveAt(i);
                    continue;
                }

                WoWPlayer p = o.ToPlayer();

                // Make sure we ignore dead/ghost players. If we need res logic, they need to be in the class-specific area.
                if (p.Dead || p.IsGhost)
                {
                    units.RemoveAt(i);
                    continue;
                }

                // Check if they're hostile first. This should remove enemy players, but it's more of a sanity check than anything.
                if (p.IsHostile)
                {
                    units.RemoveAt(i);
                    continue;
                }

                // If we're horde, and they're not, fuggin ignore them!
                if (p.IsHorde != isHorde)
                {
                    units.RemoveAt(i);
                    continue;
                }

                // They're not in our party/raid. So ignore them. We can't heal them anyway.
                if (!p.IsInMyPartyOrRaid)
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (p.HealthPercent >= SingularSettings.Instance.IgnoreHealTargetsAboveHealth)
                {
                    units.RemoveAt(i);
                    continue;
                }

                // If we have movement turned off or we are inside battlegrounds, ignore people who aren't in range.
                // Almost all healing is 40 yards, so we'll use that.
                if (p.DistanceSqr > 40*40)
                {
                    if (SingularSettings.Instance.DisableAllMovement || SingularRoutine.CurrentWoWContext == WoWContext.Battlegrounds)
                    {
                        units.RemoveAt(i);
                        continue;
                    }
                }
            }

            // A little bit of a hack, but this ensures 'Me' is in the list.
            if (!units.Any(o => o.IsMe) && StyxWoW.Me.HealthPercent < SingularSettings.Instance.IgnoreHealTargetsAboveHealth)
            {
                units.Add(StyxWoW.Me);
            }
        }

        protected override void DefaultTargetWeight(List<TargetPriority> units)
        {
            var tanks = GetMainTankGuids();
            var inBg = Battlegrounds.IsInsideBattleground;
            foreach (TargetPriority prio in units)
            {
                prio.Score = 500f;
                WoWPlayer p = prio.Object.ToPlayer();

                // The more health they have, the lower the score.
                // This should give -500 for units at 100%
                // And -50 for units at 10%
                prio.Score -= p.HealthPercent * 5;

                // If they're out of range, give them a bit lower score.
                if (p.DistanceSqr > 40 * 40)
                {
                    prio.Score -= 50f;
                }

                // If they're out of LOS, again, lower score!
                if (!p.InLineOfSpellSight)
                {
                    prio.Score -= 100f;
                }

                // Give tanks more weight. If the tank dies, we all die. KEEP HIM UP.
                if (tanks.Contains(p.Guid) && p.HealthPercent != 100 && 
                    // Ignore giving more weight to the tank if we have Beacon of Light on it.
                    !p.Auras.Any(a => a.Key == "Beacon of Light" && a.Value.CreatorGuid == StyxWoW.Me.Guid))
                {
                    prio.Score += 100f;
                }

                // Give flag carriers more weight in battlegrounds. We need to keep them alive!
                if (inBg && p.Auras.Keys.Any(a => a.ToLowerInvariant().Contains("flag")))
                {
                    prio.Score += 100f;
                }
            }
        }

        private static HashSet<ulong> GetMainTankGuids()
        {
            var infos = StyxWoW.Me.IsInRaid ? StyxWoW.Me.RaidMemberInfos : StyxWoW.Me.PartyMemberInfos;

            return new HashSet<ulong>(
                from pi in infos
                where (pi.Role & WoWPartyMember.GroupRole.Tank) != 0
                select pi.Guid);
        }
    }
}