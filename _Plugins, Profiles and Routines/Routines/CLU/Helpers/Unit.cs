using System.Collections.Generic;
using System.Linq;

using Styx;
using Styx.Logic;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace CLU.Helpers
{
    using System;

    using Styx.Logic.POI;

    using TreeSharp;

    using global::CLU.GUI;

    using Styx.Combat.CombatRoutine;
    using Styx.Logic.Combat;

    public class Unit
    {
        /* putting all the Unit logic here */

        private static readonly Unit UnitInstance = new Unit();

        /// <summary>
        /// An instance of the Unit Class
        /// </summary>
        public static Unit Instance { get { return UnitInstance; } }

        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }


        /// <summary>
        ///     List of nearby Ranged enemy units that pass certain criteria, this list should only return units 
        ///     in active combat with the player, the player's party, or the player's raid.
        /// </summary>
        public static IEnumerable<WoWUnit> RangedEnemyUnits
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                        .Where(unit =>
                               !unit.IsFriendly
                               && (unit.IsTargetingMeOrPet
                                   || unit.IsTargetingMyPartyMember
                                   || unit.IsTargetingMyRaidMember
                                   || unit.IsPlayer
                                   || unit.MaxHealth == 1)
                               && !unit.IsNonCombatPet
                               && !unit.IsCritter
                               && unit.Distance2D
                               <= 40).ToList();
            }
        }

        /// <summary>
        ///     List of nearby enemy units that pass certain criteria, this list should only return units 
        ///     in active combat with the player, the player's party, or the player's raid.
        /// </summary>
        public static IEnumerable<WoWUnit> EnemyUnits
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                        .Where(unit =>
                               !unit.IsFriendly
                               && (unit.IsTargetingMeOrPet
                                   || unit.IsTargetingMyPartyMember
                                   || unit.IsTargetingMyRaidMember
                                   || unit.IsPlayer
                                   || unit.MaxHealth == 1)
                               && !unit.IsNonCombatPet
                               && !unit.IsCritter
                               && unit.Distance2D
                               <= 12).ToList();
            }
        }

        public static bool IsBoss(WoWUnit unit)
        {
            return Lists.BossList.BossIds.Contains(unit.Entry);
        }

        public static bool IsTrainingDummy(WoWUnit unit)
        {
            return Lists.BossList.TrainingDummies.Contains(unit.Entry);
        }

        /// <summary>
        /// Returns a list of tanks
        /// </summary>
        private static IEnumerable<WoWPlayer> Tanks
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

        /// <summary>
        /// returns a list of healers.
        /// </summary>
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


        private static readonly HashSet<uint> IgnoreMobs = new HashSet<uint>
            {
                52288, // Venomous Effusion (NPC near the snake boss in ZG. Its the green lines on the ground. We want to ignore them.)
                52302, // Venomous Effusion Stalker (Same as above. A dummy unit)
                52320, // Pool of Acid
                52525, // Bloodvenom

                52387, // Cave in stalker - Kilnara
            };

        /// <summary>Returns true if the unit is attackable</summary>
        /// <param name="unit">unit to check for</param>
        /// <returns>The is attackable.</returns>
        private static bool IsAttackable(WoWUnit unit)
        {
            
            // Blacklisted...bad mob
            if (Blacklist.Contains(unit)) 
                return false;

            // ignore these
            if (IgnoreMobs.Contains(unit.Entry))
                return false;

            // Ignore shit we can't select/attack
            if (!unit.CanSelect || !unit.Attackable)
                return false;

            // Ignore friendlies!
            if (unit.IsFriendly)
                return false;

            // Duh
            if (unit.Dead)
                return false;

            // on a transport
            if (unit.IsOnTransport) 
                return false;

            // Mounted...whats the point?
            if (unit.Mounted) 
                return false;

            // Dummies/bosses are valid by default. Period.
            if (IsBoss(unit) || IsTrainingDummy(unit))
                return true;

            // If its a pet, lets ignore it please.
            if (unit.IsPet || unit.OwnedByRoot != null)
                return false;

            // And ignore critters/non-combat pets
            if (unit.IsNonCombatPet || unit.IsCritter)
                return false;

            // no totems!
            if (unit.IsTotem) 
                return false;

            // battleground demolishers are not a good target
            if (unit.CreatureType == WoWCreatureType.Mechanical) 
                return false;

            // if there pissing us off..have at em
            if (unit.IsTargetingMeOrPet) return true;

            return true;
        }

        /// <summary>
        /// checks if target is worth blowing a cooldown on
        /// </summary>
        /// <param name="target">the target to check</param>
        /// <returns>true or false</returns>
        public bool IsTargetWorthy(WoWUnit target)
        {
            if (!SettingsFile.Instance.AutoManageCooldowns)
                return false;

            if (target == null)
                return false;
            
            // PvP Player
            var pvpTarget = target.IsPlayer && Battlegrounds.IsInsideBattleground;

            // Miniboss not a big boss =)
            var miniBoss = (target.Level >= Me.Level + 2) && target.Elite;

            var targetIsWorthy = IsBoss(target) || miniBoss || IsTrainingDummy(target) || pvpTarget;
            if (targetIsWorthy)
            {
                CLU.DebugLog(
                    string.Format(
                        "[IsTargetWorthy] {0} is a boss? {1} or miniBoss? {2} or Training Dummy? {4}. {0} current Health = {3}",
                        CLU.SafeName(target),
                        IsBoss(target),
                        miniBoss,
                        target.CurrentHealth,
                        IsTrainingDummy(target)));
            }

            return targetIsWorthy;
        }

        /// <summary>
        /// Crowd controlled
        /// </summary>
        /// <param name="unit">unit to check for</param>
        /// <returns>true if controlled</returns>
        public static bool IsCrowdControlled(WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

            return auras.Any(
                a => a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                     a.Spell.Mechanic == WoWSpellMechanic.Disoriented ||
                     a.Spell.Mechanic == WoWSpellMechanic.Charmed ||
                     a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                     a.Spell.Mechanic == WoWSpellMechanic.Incapacitated ||
                     a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                     a.Spell.Mechanic == WoWSpellMechanic.Sapped ||
                     a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                     a.Spell.Mechanic == WoWSpellMechanic.Asleep ||
                     a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                     a.Spell.Mechanic == WoWSpellMechanic.Invulnerable ||
                     a.Spell.Mechanic == WoWSpellMechanic.Invulnerable2 ||
                     a.Spell.Mechanic == WoWSpellMechanic.Turned ||
                     a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||

                     // Really want to ignore hexed mobs.
                     a.Spell.Name == "Hex");
        }

        private readonly string[] controlDebuffs = new[]{
            "Bind Elemental",
            "Hex",
            "Polymorph",
            "Hibernate",
            "Entangling Roots",
            "Freezing Trap",
            "Wyvern Sting",
            "Repentance",
            "Psychic Scream",
            "Sap",
            "Blind",
            "Fear",
            "Seduction",
            "Howl of Terror"
        };
        private readonly string[] controlUnbreakableDebuffs = new[]{
            "Cyclone",
            "Mind Control",
            "Banish"
        };


        /// <summary>
        /// Checks to see if we are silenced or stunned (used for cancast)
        /// </summary>
        /// <param name="unit">the unit to check</param>
        /// <returns>returns is incapacitated status</returns>
        public bool IsIncapacitated(WoWUnit unit)
        {
            return unit != null && (unit.Stunned || unit.Silenced);
        }

        private enum GroupType
        {
            SINGLE,
            PARTY,
            RAID
        }

        public enum GroupLogic
        {
            PVE,
            BATTLEGROUND,
            ARENA
        }

        private static GroupType Group
        {
            get
            {
                if (Me.IsInParty)
                    return GroupType.PARTY;
                if (Me.IsInRaid)
                    return GroupType.RAID;
                return GroupType.SINGLE;
            }
        }

        public GroupLogic Logic
        {
            get
            {
                if (Battlegrounds.IsInsideBattleground)
                {
                    return GroupLogic.BATTLEGROUND;
                }

                if (StyxWoW.Me.CurrentMap.IsArena)
                {
                    return GroupLogic.ARENA;
                }

                return GroupLogic.PVE;
            }
        }

        // returns list of most focused mobs by players
        public struct FocusedUnit
        {
            public int PlayerCount;
            public WoWUnit Unit;
        }

        private List<FocusedUnit> mostFocusedUnits;

        public DateTime mostFocusedUnitsTimer = DateTime.MinValue;

        /// <summary>
        /// Refreshes the units depending on the current context
        /// </summary>
        private void RefreshMostFocusedUnits()
        {   
            var hostile = ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(
                x => IsAttackable(x) &&
                     // check for controlled units, like sheep etc
                     !UnitIsControlled(x, true));

            if (Group == GroupType.SINGLE)
            {
                hostile = hostile.Where(x => x.Distance2DSqr < 40 * 40).OrderBy(x => x.CurrentHealth);
                var ret = hostile.Select(h => new FocusedUnit { Unit = h }).ToList();
                this.mostFocusedUnits = ret;
            }
            else
            {
                // raid or party
                var friends = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                var ret = hostile.Select(h => new FocusedUnit { Unit = h, PlayerCount = friends.Count(x => x.CurrentTargetGuid == h.Guid) }).ToList();
                this.mostFocusedUnits = ret.OrderByDescending(x => x.PlayerCount).ToList();
            }

        }

        /// <summary>
        /// Refreshes units every 3 seonds.
        /// </summary>
        private IEnumerable<FocusedUnit> MostFocusedUnits
        {
            get
            {
                if (DateTime.Now.Subtract(this.mostFocusedUnitsTimer).TotalSeconds > 3)
                {
                    RefreshMostFocusedUnits();
                    this.mostFocusedUnitsTimer = DateTime.Now;
                }
                return this.mostFocusedUnits;
            }
        }

        /// <summary>
        /// Returns the most focused unit
        /// </summary>
        public WoWUnit EnsureUnitTargeted
        {
            get
            {

                // If we have a RaF leader, then use its target.
                var rafLeader = RaFHelper.Leader;
                if (rafLeader != null && rafLeader.IsValid && !rafLeader.IsMe && rafLeader.Combat &&
                    rafLeader.CurrentTarget != null && rafLeader.CurrentTarget.IsAlive && !Blacklist.Contains(rafLeader.CurrentTarget))
                {
                    return rafLeader;
                }

                // Check bot poi.
                if (BotPoi.Current.Type == PoiType.Kill)
                {
                    var unit = BotPoi.Current.AsObject as WoWUnit;

                    if (unit != null && unit.IsAlive && !unit.IsMe && !Blacklist.Contains(unit))
                    {
                        return unit;
                    }
                }

                // Does the target list have anything in it? And is the unit in combat?
                // Make sure we only check target combat, if we're NOT in a BG. (Inside BGs, all targets are valid!!)
                var firstUnit = Targeting.Instance.FirstUnit;
                if (firstUnit != null && firstUnit.IsAlive && !firstUnit.IsMe && firstUnit.Combat &&
                    !Blacklist.Contains(firstUnit))
                {
                    return firstUnit;
                }

                // Check for Instancebuddy and Disable targeting
                if (BotChecker.Instance.BotBaseInUse("Instancebuddy"))
                {
                    CLU.Instance.Log(" [BotChecker] Instancebuddy Detected. *TARGETING DISABLED*");
                    return null;
                }

                // Our Targeting.
                if (MostFocusedUnit.Unit != null)
                {
                    CLU.Instance.Log(" [Targeting] CLU targeting activated. *Engaging Hostile*");
                    return MostFocusedUnit.Unit;
                }

                return null;
            }
        }


        /// <summary>
        /// Returns the most focused unit
        /// </summary>
        private FocusedUnit MostFocusedUnit
        {
            get
            {
                return MostFocusedUnits.FirstOrDefault();
            }
        }

        /// <summary>
        /// 	Check for players to resurrect
        /// </summary>
        public List<WoWPlayer> ResurrectablePlayers
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWPlayer>().Where(
                    p => !p.IsMe && p.Dead && p.IsFriendly && p.IsInMyPartyOrRaid && p.DistanceSqr < 30 * 30).ToList();
            }
        }

        /// <summary>
        /// 	Check for players to use Rallying Cry on (Warrior Heal)
        /// </summary>
        public bool WarriorRallyingCryPlayers
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Any(u => u.HealthPercent < 20 && !u.ActiveAuras.ContainsKey("Rallying Cry") && !u.Dead && u.IsFriendly && u.DistanceSqr < 30 * 30);
            }
        }


        /// <summary>
        /// Returns the best player to cast tricks of the trade on
        /// </summary>
        public WoWUnit BestTricksTarget
        {
            get
            {
                if (!StyxWoW.Me.IsInParty && !StyxWoW.Me.IsInRaid)
                    return null;

                // If the player has a focus target set, use it instead.
                if (StyxWoW.Me.FocusedUnitGuid != 0)
                    return StyxWoW.Me.FocusedUnit;

                if (StyxWoW.Me.IsInInstance)
                {
                    if (RaFHelper.Leader != null && !RaFHelper.Leader.IsMe)
                    {
                        // Leader first, always. Otherwise, pick a rogue/DK/War pref. Fall back to others just in case.
                        return RaFHelper.Leader;
                    }

                    if (StyxWoW.Me.IsInParty)
                    {
                        var bestTank = Tanks.OrderBy(t => t.DistanceSqr).FirstOrDefault(t => t.IsAlive);

                        if (bestTank != null)
                            return bestTank;
                    }

                    var bestPlayer = GetPlayerByClassPrio(
                        100f,
                        false,
                        WoWClass.Rogue,
                        WoWClass.DeathKnight,
                        WoWClass.Warrior,
                        WoWClass.Hunter,
                        WoWClass.Mage,
                        WoWClass.Warlock,
                        WoWClass.Shaman,
                        WoWClass.Druid,
                        WoWClass.Paladin,
                        WoWClass.Priest);
                    return bestPlayer;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the best player to cast Bane of Havoc on
        /// </summary>
        public WoWUnit BestBaneOfHavocTarget
        {
            get
            {
                if (!SettingsFile.Instance.AutoManageBaneOfHavoc)
                    return null;

                // if (!StyxWoW.Me.IsInParty && !StyxWoW.Me.IsInRaid)
                //    return null;

                // If the player has a focus target set, use it instead.
                if ((Me.CurrentTarget != StyxWoW.Me.FocusedUnit) && StyxWoW.Me.FocusedUnitGuid != 0 && Me.FocusedUnit.InLineOfSpellSight && Me.FocusedUnit.IsAlive && !Me.FocusedUnit.GetAllAuras().Any(a => a.Name == "Bane of Havoc")) 
                    return StyxWoW.Me.FocusedUnit;

                var bestHostileEnemy =
                    RangedEnemyUnits.Where(
                        t => t != Me.CurrentTarget).OrderBy(
                           t => t.DistanceSqr).FirstOrDefault(t => t.IsAlive && !UnitIsControlled(t, true));

                if (bestHostileEnemy != null && !bestHostileEnemy.GetAllAuras().Any(a => a.Name == "Bane of Havoc") && StyxWoW.Me.FocusedUnit == null)
                    return bestHostileEnemy; 
                
                return null;
            }
        }

        /// <summary>Gets a player by class priority. The order of which classes are passed in, is the priority to find them.</summary>
        /// <remarks>Created 9/9/2011.</remarks>
        /// <param name="range">distance to player</param>
        /// <param name="includeDead">true or false</param>
        /// <param name="classes">A variable-length parameters list containing classes.</param>
        /// <returns>The player by class prio.</returns>
        private static WoWUnit GetPlayerByClassPrio(float range, bool includeDead, params WoWClass[] classes)
        {
            return (from woWClass in classes select StyxWoW.Me.PartyMemberInfos.FirstOrDefault(p => p.ToPlayer() != null && p.ToPlayer().Distance < range && p.ToPlayer().Class == woWClass) into unit where unit != null where !includeDead && unit.Dead || unit.Ghost select unit.ToPlayer()).FirstOrDefault();
        }

        /// <summary>Locates nearby units from location</summary>
        /// <param name="fromLocation">units location</param>
        /// <param name="radius">radius</param>
        /// <param name="playersOnly">true for players only</param>
        /// <returns>The nearby units.</returns>
        private List<WoWUnit> NearbyUnits(WoWPoint fromLocation, double radius, bool playersOnly)
        {
            List<WoWUnit> hostile = ObjectManager.GetObjectsOfType<WoWUnit>(true, false);
            var maxDistance2 = radius * radius;

            if (playersOnly)
            {
                hostile = hostile.Where(x =>
                                        x.IsPlayer && IsAttackable(x)
                                        && x.Location.Distance2DSqr(fromLocation) < maxDistance2).ToList();
            }
            else
            {
                hostile = hostile.Where(x =>
                                        !x.IsPlayer && IsAttackable(x)
                                        && x.Location.Distance2DSqr(fromLocation) < maxDistance2).ToList();
            }

            CLU.DebugLog("CountEnnemiesInRange");
            foreach (var u in hostile)
                CLU.DebugLog(" -> " + CLU.SafeName(u) + " " + u.Level);
            CLU.DebugLog("---------------------");
            return hostile;
        }

        /// <summary>Locates nearby units from location</summary>
        /// <param name="fromLocation">units location</param>
        /// <param name="radius">radius</param>
        /// <param name="playersOnly">true for players only</param>
        /// <returns>The nearby units.</returns>
        public static IEnumerable<WoWUnit> NearbyControlledUnits(WoWPoint fromLocation, double radius, bool playersOnly)
        {
            var hostile = ObjectManager.GetObjectsOfType<WoWUnit>(true, false);
            var maxDistance2 = radius * radius;

            if (playersOnly)
            {
                hostile = hostile.Where(x =>
                                        x.IsPlayer && IsAttackable(x) && IsCrowdControlled(x)
                                        && x.Location.Distance2DSqr(fromLocation) < maxDistance2).ToList();
            }
            else
            {
                hostile = hostile.Where(x =>
                                        !x.IsPlayer && IsAttackable(x) && IsCrowdControlled(x)
                                        && x.Location.Distance2DSqr(fromLocation) < maxDistance2).ToList();
            }

            CLU.DebugLog("CountControlledEnemiesInRange");
            foreach (var u in hostile)
                CLU.DebugLog(" -> " + CLU.SafeName(u) + " " + u.Level);
            CLU.DebugLog("---------------------");
            return hostile;
        }

        /// <summary>returns the amount of targets from the units location</summary>
        /// <param name="fromLocation">units location</param>
        /// <param name="maxRange">maximum range</param>
        /// <returns>The count ennemies in range.</returns>
        public int CountEnnemiesInRange(WoWPoint fromLocation, double maxRange)
        {
            return SettingsFile.Instance.AutoManageAoE ? this.NearbyUnits(fromLocation, maxRange, Battlegrounds.IsInsideBattleground).Count : 0;
        }

        /// <summary>Finds clustered targets</summary>
        /// <param name="radius">radius</param>
        /// <param name="minDistance">minimum distance</param>
        /// <param name="maxDistance">maximum distance</param>
        /// <param name="minTargets">minimum targets to qualify</param>
        /// <param name="playersOnly">true for players only</param>
        /// <returns>The find cluster targets.</returns>
        public WoWPoint FindClusterTargets(double radius, double minDistance, double maxDistance, int minTargets, bool playersOnly)
        {
            List<WoWUnit> hostile = ObjectManager.GetObjectsOfType<WoWUnit>(true, false);
            var avoid = new List<WoWUnit>();
            var maxDistance2 = (maxDistance + radius) * (maxDistance + radius);

            if (playersOnly)
            {
                hostile = hostile.Where(x =>
                    x.IsPlayer &&
                    IsAttackable(x) && x.Distance2DSqr < maxDistance2).ToList();
            }
            else
            {
                hostile = hostile.Where(x =>
                    !x.IsPlayer &&
                    IsAttackable(x) && x.Distance2DSqr < maxDistance2).ToList();
                avoid = hostile.Where(
                    x => // check for controlled units, like sheep etc
                    UnitIsControlled(x, true)).ToList();
            }

            if (hostile.Count < minTargets)
            {
                return WoWPoint.Empty;
            }

            var score = minTargets - 1;
            var best = WoWPoint.Empty;

            for (var x = Me.Location.X - maxDistance; x <= Me.Location.X + maxDistance; x++)
            {
                for (var y = Me.Location.Y - maxDistance; y <= Me.Location.Y + maxDistance; y++)
                {
                    var spot = new WoWPoint(x, y, Me.Location.Z);
                    var dSquare = spot.Distance2DSqr(Me.Location);
                    if (dSquare > maxDistance * maxDistance || dSquare < minDistance * minDistance)
                    {
                        continue;
                    }

                    if (avoid.Any(t => t.Location.Distance2DSqr(spot) <= radius * radius))
                    {
                        continue;
                    }

                    var hits = hostile.Count(t => t.Location.DistanceSqr(spot) < radius * radius);
                    if (hits > score)
                    {
                        best = spot;
                        score = hits;
                        CLU.DebugLog("ClusteredTargets(range=" + minDistance + "-" + maxDistance + ", radius=" + radius + ") => SCORE=" + score + " at " + spot);
                        foreach (var u in hostile.Where(t => t.Location.DistanceSqr(spot) < radius * radius))
                            CLU.DebugLog(" -> " + CLU.SafeName(u) + " " + u.Level);
                        CLU.DebugLog("---------------------");
                    }
                }
            }

            return best;
        }

        /// <summary>returns true if the unit is crowd controlled.</summary>
        /// <param name="unit">unit to check</param>
        /// <param name="breakOnDamageOnly">true for break on damage</param>
        /// <returns>The unit is controlled.</returns>
        public bool UnitIsControlled(WoWUnit unit, bool breakOnDamageOnly)
        {
            return unit.ActiveAuras.Any(x => x.Value.IsHarmful &&
                                             (this.controlDebuffs.Contains(x.Value.Name) ||
                                              (!breakOnDamageOnly && this.controlUnbreakableDebuffs.Contains(x.Value.Name))));
        }

        /// <summary>
        /// Returns true if Morchok is casting Stomp
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsMorchokStomp()
        {
            return
                ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Any(
                    u =>
                    u.IsAlive && u.Guid != Me.Guid && u.IsHostile && u.IsCasting
                    && u.CastingSpell.Name == "Stomp" && u.Name == "Morchok");
        }

        /// <summary>
        /// Returns true if Ultraxion is casting Hour of Twilight
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsHourofTwilight()
        {
            return
                ObjectManager.GetObjectsOfType<WoWUnit>(true, true).Any(
                    u =>
                    u.IsAlive && u.Guid != Me.Guid && u.IsHostile && u.IsCasting
                    && u.CastingSpell.Name == "Hour of Twilight" && u.CurrentCastTimeLeft.TotalMilliseconds <= 800);
        }

        /// <summary>
        /// Returns true if Ultraxion is casting Hour of Twilight and its my turn to soak
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsMyHourofTwilightSoak()
        {
            return Me.RaidMemberInfos.Any(x => x.GroupNumber == 3 && x.Guid == Me.Guid);
        }
        

        /// <summary>
        /// Returns true if Ultraxion is casting Fading Light
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsFadingLight()
        {
            return Buff.PlayerHasActiveBuff("Fading Light")
                   && Buff.PlayerDebuffTimeLeft("Fading Light").TotalSeconds <= 2;
        }

        /// <summary>
        /// Returns true if DeathWing is casting Shrapnel
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsShrapnel()
        {
            return
                ObjectManager.GetObjectsOfType<WoWUnit>(true, true).Any(
                    u =>
                    u.IsAlive && u.Guid != Me.Guid && u.IsHostile
                    &&
                    (u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember || u.IsTargetingMeOrPet
                     || u.IsTargetingAnyMinion) && u.IsCasting && u.CastingSpell.Name == "Shrapnel"
                    && u.CurrentCastTimeLeft.TotalMilliseconds <= 2000);
        }
    }
}
