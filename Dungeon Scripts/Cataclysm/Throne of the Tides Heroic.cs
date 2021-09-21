using System;
using System.Collections.Generic;
using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
#if USE_DUNGEONBUDDY_DLL
using Bots.DungeonBuddyDll;
using Bots.DungeonBuddyDll.Attributes;
using Bots.DungeonBuddyDll.Helpers;

namespace Bots.DungeonBuddyDll.Dungeon_Scripts.Cataclysm
#else
    using Bots.DungeonBuddy.Attributes;
    using Bots.DungeonBuddy.Helpers;
    namespace Bots.DungeonBuddy.Dungeon_Scripts.Cataclysm
#endif
{
    public class ThroneOfTheTidesHeroic : Dungeon
    {
        #region Overrides of Dungeon

        public override uint DungeonId { get { return 324; } }
        public override WoWPoint Entrance { get { return new WoWPoint(-5583.308, 5400.118, -1797.845); } }

        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (var unit in incomingunits)
            {
                if (unit.Entry == Sapper || unit.Entry == Ozumat || unit.Entry == MindLasher || unit is WoWPlayer)
                    outgoingunits.Add(unit);

                if (unit.Entry == 44404 && StyxWoW.Me.IsDps()) // Naz'jar Tempest Witch
                    outgoingunits.Add(unit);

                if (unit.Entry == 40923 && unit.ToUnit().Combat) // Unstable Corruption
                    outgoingunits.Add(unit);

                if (unit.Entry == 40925 && StyxWoW.Me.IsTank()) // Tainted Sentry
                    outgoingunits.Add(unit);
            }
        }

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                o =>
                {
                    var unit = o as WoWUnit;
                    if (unit != null)
                    {
                        // don't dps ozumat until we have this aura.
                        if (unit.Entry == Ozumat && !StyxWoW.Me.HasAura("Tidal Surge"))
                            return true;

                        // Faceless watcher casting Ground Pound. Melee should stay away
                        if (unit.CastingSpellId == 76590 && unit.DistanceSqr < 10 * 10)
                            return true;

                        // For Lady Naz'jar encounter. Lady Naz'jar becomes immune while casting Waterspout(40586)
                        if (unit.Entry == 40586 && unit.HasAura("Waterspout"))
                            return true;

                        // For Mindbender encounter. Mindbender becomes immune to magic and heals itself on damage. We should stop dpsing
                        if (unit.HasAura("Absorb Magic"))
                            return true;

                        if (unit.Entry == 40923 && !unit.Combat) // Unstable Corruption
                            return true;
                    }
                    return false;
                });
        }

        private const uint Sapper = 44752;
        private const uint Ozumat = 44566;
        private const uint MindLasher = 44715;
        private const uint NazjarSpiritmender = 41096;

        public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
        {
            foreach (var t in units)
            {
                var prioObject = t.Object;

                //We should prio Faceless Sappers for Ozumat fight
                if ((prioObject.Entry == Sapper || prioObject.Entry == MindLasher || prioObject.Entry == NazjarSpiritmender) &&
                    StyxWoW.Me.IsDps()) // 
                    t.Score += 500;

                if (prioObject.Entry == Ozumat)
                    t.Score += 500;

                if (prioObject.Entry == 44648 && StyxWoW.Me.IsTank()) // Unyielding Behemoth
                    t.Score += 600;

                if (prioObject.Entry == 44404 && StyxWoW.Me.IsDps()) // Naz'jar Tempest Witch
                    t.Score += 500;

                if (prioObject.Entry == 40925 && StyxWoW.Me.IsTank() && Targeting.Instance.FirstUnit == null)
                    // Tainted Sentry
                    t.Score += 200;
            }
        }

        #endregion

        #region Encounter Handlers

        /// <summary>
        ///   Using 0 as BossEntry will make that composite the main logic for the dungeon and it will be called in every tick You can only have one main logic for a dungeon The context of the main composite is all units around as List <WoWUnit />
        /// </summary>
        /// <returns> </returns>
        [EncounterHandler(0)]
        public Composite RootLogic()
        {
            return
                new PrioritySelector(
                //ScriptHelpers.CreateTeleporterLogic(51391, 51395),
                    ScriptHelpers.CreateRunAwayFromBad(ctx => true, 7f, 41201),
                //  Noxious Mire
                // clump of coral in middle of room at 2nd boss that we need to avoid.
                    ScriptHelpers.CreateRunAwayFromBad(
                        ctx => ScriptHelpers.IsBossAlive("Lady Naz'jar") && StyxWoW.Me.Z > 500, 12, 205542)
                    );
        }

        private WoWUnit _ladyNazjar;

        [EncounterHandler(40586, "Lady Naz'jar", Mode = CallBehaviorMode.Proximity)]
        public Composite LadyNazjarEncounter()
        {
            const uint waterspout = 48571; // heroic only ability
            const uint fungalSpores = 40597;

            var moveForwardTimer = new WaitTimer(TimeSpan.FromSeconds(20));

            return new PrioritySelector(
                ctx => _ladyNazjar = ctx as WoWUnit,
                // she is sending waves of murlocs at group.. keep pressing forward.
                new Decorator(
                    ctx =>
                    _ladyNazjar.HasAura("Murloc Leash Visual") && StyxWoW.Me.IsTank() && moveForwardTimer.IsFinished,
                    new Sequence(
                        ScriptHelpers.CreateMoveToContinue(
                            () => WoWMathHelper.CalculatePointFrom(_ladyNazjar.Location, StyxWoW.Me.Location, 30)),
                        new Action(ctx => moveForwardTimer.Reset()))),
                new Decorator(
                    ctx => _ladyNazjar.Combat,
                    new PrioritySelector(
                        ctx => _ladyNazjar = ctx as WoWUnit,
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, 76001, 91470),
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 10, fungalSpores),
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, waterspout),
                // Geyser
                        ScriptHelpers.CreateRunAwayFromBad(
                            ctx => _ladyNazjar != null && _ladyNazjar.HasAura("Waterspout"), 12, 40586), // Waterspout
                        ScriptHelpers.CreateDispellParty("Fungal Spores", ScriptHelpers.PartyDispellType.Disease)
                        )));
        }

        [ObjectHandler(203199, "Throne of Tides Defense System")]
        public Composite ThroneofTidesDefenseSystemHandler()
        {
            WoWGameObject defenseSystem = null;
            return new PrioritySelector(
                ctx => defenseSystem = ctx as WoWGameObject,
                new Decorator(
                    ctx => defenseSystem.CanUse(),
                    ScriptHelpers.CreateInteractWithObject(() => defenseSystem))
                );
        }

        private WoWUnit _commanderUlthok;

        [EncounterHandler(40765, "Commander Ulthok")]
        public Composite CommanderUlthokEncounter()
        {
            return new PrioritySelector(
                ctx => _commanderUlthok = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromLocation(
                    ctx =>
                    _commanderUlthok != null &&
                    (_commanderUlthok.CastingSpellId == 76047 || _commanderUlthok.CastingSpellId == 96311),
                    20,
                    () => WoWMathHelper.GetPointAt(_commanderUlthok.Location, 6, _commanderUlthok.Rotation, 0)),
                // Dark Fissure
                ScriptHelpers.CreateRunAwayFromBad(ctx => StyxWoW.Me.IsTank(), 20, 40784),
                // Dark Fissure - tank pulls boss way so melee can get 
                ScriptHelpers.CreateRunAwayFromBad(ctx => !StyxWoW.Me.IsTank(), 10, 40784) // Dark Fissure
                );
        }

        private WoWUnit _erunakStonespeaker, _mindbender;

        [EncounterHandler(40825, "Erunak Stonespeaker", Mode = CallBehaviorMode.Proximity)]
        public Composite ErunakStonespeakerEncounter()
        {
            return new PrioritySelector(
                ctx =>
                {
                    _mindbender = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 40788);
                    return _erunakStonespeaker = ctx as WoWUnit;
                },
                // since this guy doesn't die we need to manually mark him as dead.
                new Decorator(
                    ctx =>
                    (_mindbender == null || _mindbender.Dead) && BossManager.CurrentBoss != null &&
                    BossManager.CurrentBoss.Entry == 40825,
                    new Action(ctx => BossManager.CurrentBoss.MarkAsDead())),
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 7, 45469),
                // Earth Shards
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 12, 40861),
                // Mind Fog
                new Decorator(
                    ctx => StyxWoW.Me.IsTank() && StyxWoW.Me.CurrentTarget == _erunakStonespeaker,
                    ScriptHelpers.CreateTankFaceAwayGroupUnit(25))
                );
        }

        private readonly WoWPoint _ozumatDpsLoc = new WoWPoint(-113.4719, 957.8307, 230.7365);
        private WoWUnit _ozumat, _neptulon;
        private readonly uint[] _blightOfOzumat = new uint[]
                                        {
                                            44801, 44834
                                        };

        [EncounterHandler(44566, "Ozumat", Mode = CallBehaviorMode.CurrentBoss)]
        public Composite OzumatEncounter()
        {
            var dpsOzmatLoc = new WoWPoint(-113.209, 957.3337, 230.738);
            return new PrioritySelector(
                ctx =>
                {
                    _neptulon = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 40792);
                    return _ozumat = ctx as WoWUnit;
                },
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 11, _blightOfOzumat),
                new Decorator(
                    ctx =>
                    Targeting.Instance.FirstUnit != null && Targeting.Instance.FirstUnit.Entry == Ozumat &&
                    !StyxWoW.Me.IsHealer() && StyxWoW.Me.Location.DistanceSqr(dpsOzmatLoc) > 4 * 4,
                    new Action(ctx => Navigator.PlayerMover.MoveTowards(dpsOzmatLoc))),

                new Decorator(
                    ctx =>
                    _ozumat == null && Targeting.Instance.FirstUnit == null && _neptulon != null && _neptulon.CanGossip &&
                    StyxWoW.Me.IsTank() && StyxWoW.Me.PartyMembers.Count(p => p.DistanceSqr < 40 * 40) == 4,
                    ScriptHelpers.CreateTalkToNpc(40792)),
                // wait for spawns.
                new Decorator(
                    ctx =>
                    _neptulon != null && _neptulon.DistanceSqr <= 90 * 90 && Targeting.Instance.FirstUnit == null &&
                    StyxWoW.Me.IsTank(),
                    new ActionAlwaysSucceed())
                );
        }

        [ObjectHandler(205216, "Neptulon's Cache")]
        public Composite NeptulonsCacheHandler()
        {
            var chestTimer = new WaitTimer(TimeSpan.FromSeconds(40));
            WoWGameObject chest = null;
            return new PrioritySelector(
                ctx =>
                chest =
                ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(o => o.Entry == 205216 && o.CanUse()),
                new Decorator(
                    ctx => chest != null && chestTimer.IsFinished,
                    new Sequence(
                        ScriptHelpers.CreateInteractWithObject(205216),
                        new Action(ctx => chestTimer.Reset()))));
        }

        [EncounterHandler(44648, "Unyielding Behemoth")]
        public Composite UnyieldingBehemothEncounter()
        {
            return new PrioritySelector(
                ScriptHelpers.CreateTankFaceAwayGroupUnit(26)
                );
        }

        private WoWUnit _facelessWatcher;

        [EncounterHandler(40936, "Faceless Watcher")]
        public Composite FacelessWatcherEncounter()
        {
            return new PrioritySelector(
                ctx => _facelessWatcher = ctx as WoWUnit,
                // when it's pounding the ground.
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => _facelessWatcher.CastingSpellId == 76590, 20, u => u == _facelessWatcher)
                );
        }

        private readonly WoWPoint _elevatorBottomRestingLoc = new WoWPoint(-215.2937, 806.2046, 253.7109);
        private readonly WoWPoint _elevatorTopRestingLoc = new WoWPoint(-215.2937, 806.2046, 791.1983);

        private readonly WoWPoint _playerBottomExitLoc = new WoWPoint(-252.8892, 808.4686, 258.7985);
        private readonly WoWPoint _playerTopExitLoc = new WoWPoint(-186.2363, 803.0603, 796.6603);

        private readonly WoWPoint _playerBottomBoardLoc = new WoWPoint(-217.0751, 805.2832, 262.3475);
        private readonly WoWPoint _playerTopBoardLoc = new WoWPoint(-217.0751, 805.2832, 799.8349);

        private WoWPoint GetRandomPointAroundLocation(WoWPoint loc, float radius) { return loc.RayCast(WoWMathHelper.DegreesToRadians(ScriptHelpers.Rnd.Next(360)), radius); }

        private readonly WaitTimer _teleportTimer = new WaitTimer(TimeSpan.FromSeconds(10));

        public override MoveResult MoveTo(WoWPoint location)
        {
            if (StyxWoW.Me.CurrentMap.IsDungeon)
            {
                // we are on the bottom floor and want to move to the top floor.
                if (!StyxWoW.Me.IsOnTransport && StyxWoW.Me.Z < 500 && location.Z > 500)
                {
                    var elevator = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                        o => o.Entry == 207209);
                    var teleporter =
                        ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                            o => o.Entry == 51391 && o.HasAura("Teleporter Active Visual"));
                    if (teleporter != null && _teleportTimer.IsFinished)
                    {
                        if (!teleporter.WithinInteractRange)
                            return Navigator.MoveTo(teleporter.Location);
                        teleporter.Interact();
                        _teleportTimer.Reset();
                        return MoveResult.Moved;
                    }
                    // elevator is at the bottom resting location, lets board it.
                    if (StyxWoW.Me.Location.DistanceSqr(_playerBottomBoardLoc) <= 40 * 40 && elevator != null &&
                        elevator.Location.DistanceSqr(_elevatorBottomRestingLoc) <= 4 * 4)
                    {
                        Navigator.PlayerMover.MoveTowards(
                            GetRandomPointAroundLocation(_playerBottomBoardLoc, ScriptHelpers.Rnd.Next(4, 8)));
                        return MoveResult.Moved;
                    } // wait for elevator here at this spot.
                    return Navigator.MoveTo(_playerBottomExitLoc);
                }

                // we are on the top floor and want to move to the bottom floor.
                if (!StyxWoW.Me.IsOnTransport && StyxWoW.Me.Z > 500 && location.Z < 500)
                {
                    var elevator = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                        o => o.Entry == 207209);
                    var teleporter =
                        ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                            o => o.Entry == 51395 && o.HasAura("Teleporter Active Visual"));
                    if (teleporter != null && _teleportTimer.IsFinished)
                    {
                        if (!teleporter.WithinInteractRange)
                            return Navigator.MoveTo(teleporter.Location);
                        teleporter.Interact();
                        _teleportTimer.Reset();
                        return MoveResult.Moved;
                    }
                    // elevator is at the bottom resting location, lets board it.
                    if (StyxWoW.Me.Location.DistanceSqr(_playerTopBoardLoc) <= 40 * 40 && elevator != null &&
                        elevator.Location.DistanceSqr(_elevatorTopRestingLoc) <= 4 * 4)
                    {
                        Navigator.PlayerMover.MoveTowards(
                            GetRandomPointAroundLocation(_playerTopBoardLoc, ScriptHelpers.Rnd.Next(4, 8)));
                        return MoveResult.Moved;
                    } // wait for elevator here at this spot.
                    return Navigator.MoveTo(_playerTopExitLoc);
                }
                // tank is on elevator so we should get on too.
                if (StyxWoW.Me.IsFollower() && ScriptHelpers.Tank != null && ScriptHelpers.Tank.IsOnTransport)
                {
                    var elevator = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                        o => o.Entry == 207209);
                    if (elevator != null && ScriptHelpers.Tank.DistanceSqr > 17 * 17 &&
                        ((elevator.Location.DistanceSqr(_elevatorBottomRestingLoc) <= 4 * 4 && StyxWoW.Me.Z < 500) ||
                         (elevator.Location.DistanceSqr(_elevatorTopRestingLoc) <= 4 * 4 && StyxWoW.Me.Z > 500)))
                    {
                        Navigator.PlayerMover.MoveTowards(
                            GetRandomPointAroundLocation(ScriptHelpers.Tank.Location, ScriptHelpers.Rnd.Next(4, 8)));
                    }

                    else if (!StyxWoW.Me.IsOnTransport && StyxWoW.Me.Z < 500)
                        return Navigator.MoveTo(_playerBottomExitLoc);

                    else if (!StyxWoW.Me.IsOnTransport && StyxWoW.Me.Z > 500)
                        return Navigator.MoveTo(_playerTopExitLoc);


                    return MoveResult.Moved;
                }

                if (StyxWoW.Me.IsOnTransport)
                {
                    var elevator = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                        o => o.Entry == 207209);

                    // get off elevator at bottom.
                    if (elevator != null && elevator.Location.DistanceSqr(_elevatorBottomRestingLoc) <= 4 * 4 &&
                        location.Z < 500)
                    {
                        Navigator.PlayerMover.MoveTowards(_playerBottomExitLoc);
                    }
                    // get off elevator at top.
                    if (elevator != null && elevator.Location.DistanceSqr(_elevatorTopRestingLoc) <= 4 * 4 &&
                        location.Z > 500)
                    {
                        Navigator.PlayerMover.MoveTowards(_playerTopExitLoc);
                    }
                    return MoveResult.Moved;
                }
                // ozumat move to this spot if fighting Ozumat
                if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.Entry == 44566 && StyxWoW.Me.IsMelee())
                {
                    return Navigator.MoveTo(_ozumatDpsLoc);
                }
            }
            return MoveResult.Failed;
        }

        #endregion
    }
}