#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: highvoltz $
// $Date: 2012-06-08 05:29:19 +0300 (Cum, 08 Haz 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Managers/TankManager.cs $
// $LastChangedBy: highvoltz $
// $LastChangedDate: 2012-06-08 05:29:19 +0300 (Cum, 08 Haz 2012) $
// $LastChangedRevision: 640 $
// $Revision: 640 $

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Singular.Helpers;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
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

    internal class TankManager : Targeting
    {
        static TankManager()
        {
            Instance = new TankManager {NeedToTaunt = new List<WoWUnit>()};
        }

        public new static TankManager Instance { get; set; }
        public List<WoWUnit> NeedToTaunt { get; private set; }

        public static readonly WaitTimer TargetingTimer = new WaitTimer(TimeSpan.FromSeconds(1));

        protected override List<WoWObject> GetInitialObjectList()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Cast<WoWObject>().ToList();
        }

        protected override void DefaultRemoveTargetsFilter(List<WoWObject> units)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (!units[i].IsValid)
                {
                    units.RemoveAt(i);
                    continue;
                }

                WoWUnit u = units[i].ToUnit();

                if (u.IsFriendly || u.Dead || u.IsPet || !u.Combat || u.IsCrowdControlled())
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (u.DistanceSqr > 40 * 40)
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (Unit.IgnoreMobs.Contains(u.Entry))
                {
                    units.RemoveAt(i);
                    continue;
                }

                if (u.CurrentTarget == null) 
                    continue;

                WoWUnit tar = u.CurrentTarget;
                if (!tar.IsPlayer || !tar.IsHostile) 
                    continue;

                units.RemoveAt(i);
            }
        }

        protected override void DefaultIncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            foreach (WoWObject i in incomingUnits)
            {
                var unit = i as WoWUnit;

                outgoingUnits.Add(i);
            }
        }

        protected override void DefaultTargetWeight(List<TargetPriority> units)
        {
            NeedToTaunt.Clear();
            List<WoWPlayer> members = StyxWoW.Me.IsInRaid ? StyxWoW.Me.RaidMembers : StyxWoW.Me.PartyMembers;
            foreach (TargetPriority p in units)
            {
                WoWUnit u = p.Object.ToUnit();

                // I have 1M threat -> nearest party has 990k -> leaves 10k difference. Subtract 10k
                // I have 1M threat -> nearest has 400k -> Leaves 600k difference -> subtract 600k
                // The further the difference, the less the unit is weighted.
                // If they have MORE threat than I do, the number is -10k -> which subtracted = +10k weight.
                int aggroDiff = GetAggroDifferenceFor(u, members);
                p.Score -= aggroDiff;

                // If we have NO threat on the mob. Taunt the fucking thing.
                // Don't taunt fleeing mobs!
                if (aggroDiff < 0 && !u.Fleeing)
                {
                    NeedToTaunt.Add(u);
                }
            }
        }

        private static int GetAggroDifferenceFor(WoWUnit unit, IEnumerable<WoWPlayer> partyMembers)
        {
            uint myThreat = unit.ThreatInfo.ThreatValue;
            uint highestParty = (from p in partyMembers
                                 let tVal = unit.GetThreatInfoFor(p).ThreatValue
                                 orderby tVal descending
                                 select tVal).FirstOrDefault();

            int result = (int)myThreat - (int)highestParty;
            return result;
        }
    }
}