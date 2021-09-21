using System;
using System.Collections.Generic;
using System.Linq;

using Styx.WoWInternals.WoWObjects;

namespace Singular.Helpers
{
    internal static class Clusters
    {
        public static int GetClusterCount(WoWUnit target, IEnumerable<WoWUnit> otherUnits, ClusterType type, float clusterRange)
        {
            if (otherUnits.Count() == 0)
                return 0;

            switch (type)
            {
                case ClusterType.Radius:
                    return GetRadiusClusterCount(target, otherUnits, clusterRange);
                case ClusterType.Chained:
                    return GetChainedClusterCount(target, otherUnits, clusterRange);
                case ClusterType.Cone:
                    return GetConeClusterCount(target, otherUnits, clusterRange);
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public static WoWUnit GetBestUnitForCluster(IEnumerable<WoWUnit> units, ClusterType type, float clusterRange)
        {
            if (!units.Any())
                return null;

            switch (type)
            {
                case ClusterType.Radius:
                    return (from u in units
                            select new { Count = GetRadiusClusterCount(u, units, clusterRange), Unit = u }).OrderByDescending(a => a.Count).
                        FirstOrDefault().Unit;
                case ClusterType.Chained:
                    return (from u in units
                            select new { Count = GetChainedClusterCount(u, units, clusterRange), Unit = u }).OrderByDescending(a => a.Count).
                        FirstOrDefault().Unit;
                // coned doesn't have a best unit, since we are the source
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private static int GetConeClusterCount(WoWUnit target, IEnumerable<WoWUnit> otherUnits, float distance)
        {
            var targetLoc = target.Location;
            return otherUnits.Count(u => target.IsSafelyFacing(u, 90f) && u.Location.Distance(targetLoc) <= distance); // most (if not all) cone spells are 90 degrees
        }

        private static int GetRadiusClusterCount(WoWUnit target, IEnumerable<WoWUnit> otherUnits, float radius)
        {
            var targetLoc = target.Location;
            return otherUnits.Count(u => u.Location.DistanceSqr(targetLoc) <= radius * radius);
        }

        private static int GetChainedClusterCount(WoWUnit target, IEnumerable<WoWUnit> otherUnits, float chainRange)
        {
            var unitCounters = otherUnits.Select(u => GetUnitsChainWillJumpTo(target, otherUnits.ToList(), chainRange).Count);

            return unitCounters.Max() + 1;
        }

        private static List<WoWUnit> GetUnitsChainWillJumpTo(WoWUnit target, List<WoWUnit> otherUnits, float chainRange)
        {
            var targetLoc = target.Location;
            var targetGuid = target.Guid;
            for (int i = otherUnits.Count - 1; i >= 0; i--)
            {
                if (otherUnits[i].Guid == targetGuid || otherUnits[i].Location.DistanceSqr(targetLoc) > chainRange)
                    otherUnits.RemoveAt(i);
            }
            return otherUnits;
        }
    }
}
