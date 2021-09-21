using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Inventory.Frames.Gossip;
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
    public class HallsOfOrigination : Dungeon
    {
        #region Overrides of Dungeon

        public override uint DungeonId { get { return 305; } }

        public override WoWPoint Entrance { get { return new WoWPoint(-10220.02, -1838.361, 20.12786); } }

        private readonly CircularQueue<WoWPoint> _corpseRun = new CircularQueue<WoWPoint>
                                                              {
                                                                  new WoWPoint(
                                                                      -10249,
                                                                      -2112.89,
                                                                      77.60123),
                                                                  new WoWPoint(
                                                                      -10183.18,
                                                                      -1997.53,
                                                                      67.36435),
                                                                  new WoWPoint(
                                                                      -10184.61,
                                                                      -1897.09,
                                                                      27.91174),
                                                                  new WoWPoint(
                                                                      -10187.94,
                                                                      -1832.484,
                                                                      27.84191),
                                                                  new WoWPoint(
                                                                      -10227,
                                                                      -1838.81,
                                                                      33.86435),
                                                              };

        //public override CircularQueue<WoWPoint> CorpseRunBreadCrumb { get { return _corpseRun; } }

        // public override bool IsFlyingCorpseRun { get { return true; } }

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                ret =>
                    {
                        if ((ret.Entry == StoneTroggBrute || ret.Entry == StoneTroggPillager ||
                             ret.Entry == StoneTroggRockFlinger) && ret.DistanceSqr > 20*20 && !ret.ToUnit().Combat)
                            return true;

                        // remove this boss from target list (except if bot is healer) since it's immune
                        if (ret.Entry == TempleGuardianAnhuur && ret.ToUnit().HasAura("Shield of Light") &&
                            !StyxWoW.Me.IsHealer)
                            return true;

                        // ignore all pitvipers when anhuur is not immune unless outside of the pit.
                        if (ret.Entry == PitViper && (!_anhuurIsImmune && _ignoreSnakes && _anhuurIsCastingHymn) || 
                            (_anhuurIsImmune && StyxWoW.Me.IsDps() && ret.ToUnit().CurrentTarget != StyxWoW.Me))
                        {
                            return true;
                        }

                        if (ret.Entry == VoidSentinel &&
                            ((StyxWoW.Me.IsTank() && ret.ToUnit().CurrentTarget == StyxWoW.Me) || !StyxWoW.Me.IsTank()))
                            return true;

                        return false;
                    });
        }

        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (var obj in incomingunits)
            {
                var unit = obj as WoWUnit;
                if (unit != null)
                {
                    if (unit.Entry == TempleGuardianAnhuur && unit.Combat && StyxWoW.Me.IsHealer())
                        outgoingunits.Add(unit);

                    if (unit.Entry == PitViper && unit.Combat && ((!_anhuurIsCastingHymn && unit.X > 75 && unit.DistanceSqr < 40 * 40) || !_ignoreSnakes || (StyxWoW.Me.IsTank() && _anhuurIsImmune)))
                        outgoingunits.Add(unit);

                    if (unit.Entry == WaterBubble)
                        outgoingunits.Add(unit);

                    if ((unit.Entry == SeedlingPod || unit.Entry == BloodpetalBlossom) && StyxWoW.Me.IsDps())
                        outgoingunits.Add(unit);

                    if (unit.Entry == CelestialFamiliar)
                        outgoingunits.Add(unit);
                }
            }
        }

        public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
        {
            foreach (var priority in units)
            {
                var unit = priority.Object as WoWUnit;
                if (unit != null)
                {
                    if (unit.Entry == PitViper && _anhuurIsImmune && StyxWoW.Me.IsTank() && unit.Combat && unit.IsTargetingMyPartyMember)
                        priority.Score += 500;

                    if (unit.Entry == WaterBubble)
                        priority.Score += 500;

                    if (unit.Entry == SeedlingPod && StyxWoW.Me.IsDps())
                        priority.Score += 400;

                    if (unit.Entry == BloodpetalBlossom && StyxWoW.Me.IsDps())
                        priority.Score += 450;

                    if (unit.Entry == VoidSentinel && StyxWoW.Me.IsTank())
                        priority.Score += 500;

                    if (unit.Entry == CelestialFamiliar)
                        priority.Score += 500;
                }
            }
        }

        #endregion

        private const uint TempleGuardianAnhuur = 39425;
        private const uint SearingLight = 40283;
        private const uint PitViper = 39444;
        private const uint WaterBubble = 41257;
        private const uint Camel = 39443;
        private const uint SeedlingPod = 40716;
        private const uint BloodpetalBlossom = 40620;
        private const uint CelestialFamiliar = 39795;
        private const uint SolarWinds = 47922;

        private const uint StoneTroggPillager = 39804;
        private const uint StoneTroggRockFlinger = 40252;
        private const uint StoneTroggBrute = 40251;

        [EncounterHandler(0, "Root")]
        public Composite RootBehavior()
        {
            return new PrioritySelector(

                new Action(
                    ctx =>
                        {
                            if (!StyxWoW.Me.IsTank())
                            {
                                var tankI =
                                    DungeonBot.PartyMemberInfos.FirstOrDefault(
                                        p => p.HasRole(WoWPartyMember.GroupRole.Tank));

                                if (tankI != null)
                                {
                                    var tank = tankI.ToPlayer();
                                    var loc = tank != null ? tank.Location : tankI.Location3D;
                                    var myLoc = StyxWoW.Me.Location;

                                    if (IsOnLift(tank) || IsOnLift(StyxWoW.Me) ||
                                        (myLoc.Z < 200 && loc.Z > 200 || myLoc.Z > 200 && loc.Z < 200))
                                        return ElevatorBehavior(loc) ? RunStatus.Success : RunStatus.Failure;
                                }
                            }
                            return RunStatus.Failure;
                        }));
        }

        private bool _anhuurIsImmune;
        private WoWGameObject _beaconOfLight;
        private readonly WoWPoint _anhuurPlatformCenterLoc = new WoWPoint(-639.6255, 342.8197, 77.76077);

        private enum AnhuurBeaconSide
        {
            Right,
            Left
        }

        private AnhuurBeaconSide _anhuurBeaconSide;
        private bool _ignoreSnakes;
        private bool _anhuurIsCastingHymn;
        uint[] _beaconOfLightIds = new uint[] { 207218, 207219, 203133, 203136 };


        [EncounterHandler(39425, "Temple Guardian Anhuur", Mode = CallBehaviorMode.Proximity, BossRange = 100)]
        public Composite TempleGuardianAnhuurEncounter()
        {
            WoWPlayer tank = null;
            WoWUnit boss = null;
            var partyMembers = new List<WoWPlayer>();
            var hasSnakeAggro = false;
            var rightLeverLoc = new WoWPoint(-677.7728, 342.0336, 53.70121);
            var leftLeverLoc = new WoWPoint(-603.611, 340.9965, 54.47164);
            var topOfRampLoc = new WoWPoint(-640.4803, 383.8842, 83.86601);

            var leftTopJumpLoc = new WoWPoint(-626.5026, 332.8715, 77.75826);
            var leftBottomJumpLoc = new WoWPoint(-615.3797, 329.2838, 53.02038);

            var rightTopJumpLoc = new WoWPoint(-655.1562, 334.6572, 77.75832);
            var rightBottomJumpLoc = new WoWPoint(-666.0155, 335.7277, 53.33085);

            return new PrioritySelector(
                ctx =>
                {
                    boss = (WoWUnit)ctx;
                    var healer = ScriptHelpers.Healer;
                    tank = ScriptHelpers.Tank;
                    partyMembers.Clear();
                    if (StyxWoW.Me.IsDps())
                    {
                        partyMembers.AddRange(
                            ScriptHelpers.PartyIncludingMe.Where(p => p != healer && p != tank && p.IsAlive).OrderBy
                                (
                                    p => p.MaxHealth));

                        var maxHealRank = partyMembers.IndexOf(StyxWoW.Me);
                        _anhuurBeaconSide = maxHealRank == 0 ? AnhuurBeaconSide.Right : AnhuurBeaconSide.Left;
                    }
                    else if (!StyxWoW.Me.IsDps())
                    {
                        _anhuurBeaconSide = AnhuurBeaconSide.Right;
                    }
                    _beaconOfLight =
                        ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                            u => _beaconOfLightIds.Contains(u.Entry) && u.CanUse()).OrderBy(
                                u =>
                                u.Location.DistanceSqr(
                                    _anhuurBeaconSide == AnhuurBeaconSide.Right ? rightLeverLoc : leftLeverLoc))
                            .FirstOrDefault();

                    _anhuurIsImmune = boss.HasAura("Shield of Light");
                    _anhuurIsCastingHymn = boss.CastingSpellId == 75323 || boss.CastingSpellId == 75322;

                    var angrySnakes = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == PitViper && u.Combat).ToList();
                    _ignoreSnakes = angrySnakes.Count <= 2;
                    hasSnakeAggro = angrySnakes.Any(u => u.CurrentTarget == StyxWoW.Me);

                    return boss;
                },
                new Decorator(
                    ctx => boss.Combat,
                    new PrioritySelector(
                // run from Searing Light. ignore the ones that are on steps since we'll need to run through them to get to boss.
                        ScriptHelpers.CreateRunAwayFromBad(
                            ctx => true,
                            6,
                            u =>
                            u.Entry == SearingLight &&
                            (u.Z > 75 && u.Location.DistanceSqr(_anhuurPlatformCenterLoc) <= 30 * 30 || u.Z > 80)),
                // dispell Divine Reckoning
                        ScriptHelpers.CreateDispellParty("Divine Reckoning", ScriptHelpers.PartyDispellType.Magic),

                        // jump point handling
                        new Decorator(
                            ctx => _anhuurIsImmune && _beaconOfLight != null && StyxWoW.Me.Z > 75,
                            new PrioritySelector(
                                new Decorator(ctx => StyxWoW.Me.Location.DistanceSqr(_anhuurBeaconSide == AnhuurBeaconSide.Right ? rightTopJumpLoc : leftTopJumpLoc) > 7 * 7,
                                    new Action(ctx => Navigator.MoveTo(_anhuurBeaconSide == AnhuurBeaconSide.Right ? rightTopJumpLoc : leftTopJumpLoc))),
                            new Action(ctx => Navigator.PlayerMover.MoveTowards(_anhuurBeaconSide == AnhuurBeaconSide.Right ? rightBottomJumpLoc : leftBottomJumpLoc)))),

                        // beacon behavior
                        new Decorator(
                            ctx => _anhuurIsImmune && _beaconOfLight != null && !StyxWoW.Me.IsHealer(),
                // flip the switches.
                                    new PrioritySelector(
                                        new Decorator(ctx => _beaconOfLight.DistanceSqr > 3 * 3,
                                            new Action(ctx => Navigator.MoveTo(_beaconOfLight.Location))),

                                        new Decorator(ctx => !hasSnakeAggro,
                                            ScriptHelpers.CreateInteractWithObject(() => _beaconOfLight, 11, false)))),


                        new Decorator(ctx => !_anhuurIsImmune,
                            new PrioritySelector(
                // run to tank if has snake aggro. we'll aoe them down
                                new Decorator(ctx => !_ignoreSnakes && hasSnakeAggro && tank.IsAlive && tank.DistanceSqr > 5 * 5,
                                    new Action(ctx => Navigator.MoveTo(tank.Location))),
                // force everyone to move to the top if still down in the pit.
                // controlled moveto allows CC to still cast spells e.g. heals if needed.
                                new Decorator(ctx => _ignoreSnakes && StyxWoW.Me.Z < 75 && ScriptHelpers.MovementEnabled,
                                    new Action(ctx => ScriptHelpers.ControlledMoveTo(topOfRampLoc)))
                                ))
                                ))
                );
        }

        [EncounterHandler(39908, "Brann Bronzebeard", BossRange = 20, Mode = CallBehaviorMode.Proximity)]
        public Composite BrannBronzebeardEncounter()
        {
            WoWUnit brann = null;
            const uint vaultOfLightsEntranceDoor = 202313;
            WoWGameObject door = null;

            return new PrioritySelector(
                ctx =>
                    {
                        door =
                            ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                                g => g.Entry == vaultOfLightsEntranceDoor);
                        return brann = ctx as WoWUnit;
                    },
                new Decorator(
                    ctx => brann.CanGossip && door != null && door.State == WoWGameObjectState.Ready,
                    ScriptHelpers.CreateTalkToNpc(() => brann, 0)));
        }


        [EncounterHandler(39800, "Flame Warden")]
        public Composite FlameWardenEncounter()
        {
            // ToDo : Lava Eruption
            return new PrioritySelector(ScriptHelpers.CreateSpreadOutLogic(ctx => true, 7));
        }

        [EncounterHandler(39803, "Air Warden")]
        public Composite AirWardenEncounter()
        {
            const uint whirlingWinds = 41245;
            return new PrioritySelector(ScriptHelpers.CreateRunAwayFromBad(ctx => true, 9, whirlingWinds));
        }

        [EncounterHandler(39802, "Water Warden")]
        public Composite WaterWardenEncounter()
        {
            const uint aquaBomb = 41264;

            return new PrioritySelector(ScriptHelpers.CreateRunAwayFromBad(ctx => true, 9, aquaBomb));
        }

        private readonly int[] _rockWaveIds = new[] {77234, 91162};

        [EncounterHandler(39801, "Earth Warden")]
        public Composite EarthWardenEncounter()
        {
            WoWUnit boss = null;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                new Decorator(
                    ctx => _rockWaveIds.Contains(boss.CastingSpellId) && boss.IsFacing(StyxWoW.Me),
                    // run to the side if boss is casting rockwave
                    new Action(
                        ctx =>
                            {
                                var bossToMeFacing = WoWMathHelper.CalculateNeededFacing(
                                    boss.Location, StyxWoW.Me.Location);
                                var bossRotation = WoWMathHelper.NormalizeRadian(boss.Rotation);
                                var facingDifference = WoWMathHelper.NormalizeRadian(bossToMeFacing - bossRotation);

                                var moveTo = WoWMathHelper.CalculatePointAtSide(
                                    boss.Location, boss.Rotation, (float) boss.Distance, facingDifference > Math.PI);
                                if (!Navigator.CanNavigateFully(StyxWoW.Me.Location, moveTo))
                                {
                                    moveTo = WoWMathHelper.CalculatePointAtSide(
                                        boss.Location, boss.Rotation, 4f, facingDifference > Math.PI);
                                }
                                // make sure
                                if (StyxWoW.Me.Location.DistanceSqr(moveTo) >
                                    Navigator.PathPrecision*Navigator.PathPrecision)
                                {
                                    return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(moveTo));
                                }
                                return RunStatus.Failure;
                            })),
                // spread out to lessen the damage from rockwave ability
                ScriptHelpers.CreateSpreadOutLogic(ctx => true, () => boss.Location, 10, 30));
        }

        private readonly int[] _omegaStanceIds = new[] {75622, 91208};
        private readonly uint[] _minibossIds = new uint[] {39800, 39801, 39802, 39803};

        [EncounterHandler(39788, "Anraphet", Mode = CallBehaviorMode.CurrentBoss)]
        public Composite AnraphetEncounter()
        {
            WoWUnit boss = null;
            const int alphaBeamsSpell = 76184;
            const uint alphaBeam = 41144;
            WoWUnit miniBoss = null;

            return new PrioritySelector(
                ctx =>
                    {
                        miniBoss =
                            ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                                u => _minibossIds.Contains(u.Entry) && u.IsAlive).OrderBy(u => u.DistanceSqr).
                                FirstOrDefault();
                        return boss = ctx as WoWUnit;
                    },
                new Decorator(ctx => miniBoss != null, new Action(ctx => ScriptHelpers.MoveTankTo(miniBoss.Location))),
                // boss behavior
                new Decorator(
                    ctx => boss != null && boss.Combat,
                    new PrioritySelector(
                        // dispell Nemesis Strike
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, alphaBeam),
                        ScriptHelpers.CreateDispellParty("Nemesis Strike", ScriptHelpers.PartyDispellType.Magic),
                        // group up for aoe heals.
                        new Decorator(
                            ctx => _omegaStanceIds.Contains(boss.CastingSpellId) && ScriptHelpers.Tank.DistanceSqr > 5*5,
                            new Action(
                                ctx =>
                                Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(ScriptHelpers.Tank.Location)))),
                        // spread out to lessen damage from Alpha Beam
                        ScriptHelpers.CreateSpreadOutLogic(ctx => boss.CastingSpellId == alphaBeamsSpell, 8))));
        }

        [EncounterHandler(39428, "Earthrager Ptah", Mode = CallBehaviorMode.Proximity, BossRange = 200)]
        public Composite EarthragerPtahEncounter()
        {
            WoWUnit boss = null;
            WoWUnit camel = null;
            const uint quickSand = 40503;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                // find a camel to hop on.
                new Decorator(
                    ctx => StyxWoW.Me.CharmedUnitGuid == 0 && Targeting.Instance.FirstUnit == null,
                    new PrioritySelector(
                        ctx =>
                        camel =
                        ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == Camel && u.OwnedByUnit == null).
                            OrderBy(u => u.DistanceSqr).FirstOrDefault(),
                        ScriptHelpers.CreateTalkToNpc(() => camel))),
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 6, 75339, 89882),
                // run from earth spikes
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 10, quickSand),
                // stack on tank for aoe
                new Decorator(
                    ctx => boss.HasAura("Ptah Explosion") && ScriptHelpers.Tank.DistanceSqr > 10*10,
                    new Action(
                        ctx => Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(ScriptHelpers.Tank.Location)))),
                // face boss away from party.
                new Decorator(
                    ctx => StyxWoW.Me.IsTank() && boss.CurrentTarget == StyxWoW.Me,
                    ScriptHelpers.CreateTankFaceAwayGroupUnit(10)));
        }

        [EncounterHandler(39731, "Ammunae", Mode = CallBehaviorMode.Proximity, BossRange = 100)]
        public Composite AmmunaeEncounter()
        { // todo check why dps are not attacking pods
            WoWUnit boss = null;
            const int sporeCloud = 75701;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                // clear the room before pulling boss
                new Decorator(
                    ctx => !boss.Combat, ScriptHelpers.CreateClearArea(() => boss.Location, 74, u => u != boss)),
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, sporeCloud),
                ScriptHelpers.CreateDispellParty("Wither", ScriptHelpers.PartyDispellType.Magic));
        }

        [EncounterHandler(39732, "Setesh")]
        public Composite SeteshEncounter()
        {
            const uint chaosBlast = 41041;
            const uint reignOfChaos = 41168;
            WoWUnit boss = null;
            WoWUnit chaosSeed = null;

            return new PrioritySelector(
                ctx =>
                    {
                        chaosSeed = !StyxWoW.Me.HasAura("Seed of Chaos")
                                        ? ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                                            u => u.Entry == 41126)
                                        : null;
                        return boss = ctx as WoWUnit;
                    },
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 13, chaosBlast),
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 5, reignOfChaos),
                // get the dps increasing buff
                new Decorator(
                    ctx => chaosSeed != null && StyxWoW.Me.IsDps(),
                    new Action(ctx => Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(chaosSeed.Location)))));
        }


        [EncounterHandler(39587, "Isiset")]
        public Composite IsisetEncounter()
        {
            const int supernova = 74136;
            const uint reignOfChaos = 41168;
            WoWUnit boss = null;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                new Decorator(
                    ctx =>
                    boss.CastingSpellId == supernova && boss.CurrentCastTimeLeft <= TimeSpan.FromSeconds(1),
                    new Action(
                        ctx =>
                            {
                                var newDir =
                                    WoWMathHelper.NormalizeRadian(
                                        WoWMathHelper.CalculateNeededFacing(StyxWoW.Me.Location, boss.Location) -
                                        (float) Math.PI);
                                StyxWoW.Me.SetFacing(newDir);
                            })),
                ScriptHelpers.CreateDispellEnemy("Veil of Sky",ScriptHelpers.EnemyDispellType.Magic, () => boss));
        }

        [EncounterHandler(39378, "Rajh")]
        public Composite RajhEncounter()
        {
            WoWUnit boss = null;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit, ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, SolarWinds));
        }

        private const uint TransitDevice = 204979;
        private const uint LiftController = 207669;
        private const uint LiftOfTheMakers = 207547;
        private const uint VoidSentinel = 41208;

        private readonly WoWPoint _liftBottomLoc = new WoWPoint(-505.5215, 193.4112, 79.26682);
        private readonly WoWPoint _liftTopLoc = new WoWPoint(-505.5215, 193.4112, 330.1028);
        private readonly WoWPoint _liftBottomWaitAtLoc = new WoWPoint(-490.6393, 213.1238, 79.81146);
        private readonly WoWPoint _liftBottomBoardLoc = new WoWPoint(-492.0947, 206.9395, 79.80594);
        private readonly WoWPoint _liftTopExitPoint = new WoWPoint(-480.6105, 215.0368, 330.6841);

        private readonly WaitTimer _transistDeviceTimer = new WaitTimer(TimeSpan.FromSeconds(8));

        public override MoveResult MoveTo(WoWPoint location)
        {
            if (StyxWoW.Me.IsTank() && !_transistDeviceTimer.IsFinished)
                return MoveResult.Moved;
            var myloc = StyxWoW.Me.Location;

            // use transit devices
            if (myloc.DistanceSqr(location) >= 40000) // 200 units
            {
                var transitDevice =
                    ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                        u => u.Entry == TransitDevice && u.DistanceSqr <= 10000).OrderBy(u => u.DistanceSqr).
                        FirstOrDefault();
                if (transitDevice != null)
                {
                    if (transitDevice.WithinInteractRange)
                    {
                        Logger.Write("Using Transit Device");
                        transitDevice.Interact();
                        _transistDeviceTimer.Reset();
                    }
                    else
                        return Navigator.MoveTo(transitDevice.Location);
                    return MoveResult.Moved;
                }
            }

            if (ElevatorBehavior(location))
                return MoveResult.Moved;

            return MoveResult.Failed;
        }

        public bool ElevatorBehavior(WoWPoint destination)
        {
            var myloc = StyxWoW.Me.Location;
            var currentFloorLevel = myloc.Z < 200 ? 1 : 2;
            var destinationFloorLevel = destination.Z < 200 ? 1 : 2;
            var tank = ScriptHelpers.Tank;

            if (currentFloorLevel != destinationFloorLevel || tank != null && !tank.IsMe && IsOnLift(tank))
            {
                if (currentFloorLevel == 1)
                {
                    var lift =
                        ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(g => g.Entry == LiftOfTheMakers);
                    if (lift == null)
                        return false;

                    var liftIsAtRestingPlace = lift.Location.DistanceSqr(_liftBottomLoc) <= 3*3;

                    if (!StyxWoW.Me.IsOnTransport)
                    {
                        // move to lift controler loc
                        if (myloc.DistanceSqr(_liftBottomWaitAtLoc) > 5*5)
                        {
                            var moveResult = Navigator.MoveTo(_liftBottomWaitAtLoc);
                            return moveResult != MoveResult.Failed && moveResult != MoveResult.PathGenerationFailed;
                        }
                        // board the lift if it's at resting place.
                        if (liftIsAtRestingPlace)
                            Navigator.PlayerMover.MoveTowards(_liftBottomBoardLoc);
                        else
                            BringLiftToFloor(1);
                    }
                    else if (liftIsAtRestingPlace)
                    {
                        var allAboard = !(from memberI in DungeonBot.PartyMemberInfos
                                          let unit = memberI.ToPlayer()
                                          let loc = unit != null ? unit.Location : memberI.Location3D
                                          where
                                              unit != null && !unit.IsOnTransport && loc.Z < 200 ||
                                              unit == null && loc.Z < 200
                                          select memberI).Any();

                        if (allAboard)
                            BringLiftToFloor(2);
                    }
                }
                else if (currentFloorLevel == 2)
                {
                    // port out - bot will auto port back in. this is the only way to get to 1st floor.
                    Lua.DoString("LFGTeleport(true)");
                }
                return true;
            }
            // exit the lift once it gets to top
            if (IsOnLift(StyxWoW.Me))
            {
                var lift =
                    ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(g => g.Entry == LiftOfTheMakers);

                if (lift != null && lift.Location.DistanceSqr(_liftTopLoc) <= 3*3)
                    Navigator.PlayerMover.MoveTowards(_liftTopExitPoint);

                return true;
            }
            return false;
        }

        private bool IsOnLift(WoWPlayer player) { return player != null && player.IsOnTransport && player.Transport.Entry == LiftOfTheMakers; }

        private readonly WaitTimer _liftControllerTimer = new WaitTimer(TimeSpan.FromSeconds(15));

        private void BringLiftToFloor(int floor)
        {
            if (!_liftControllerTimer.IsFinished)
                return;

            var controller =
                ObjectManager.GetObjectsOfType<WoWGameObject>().Where(o => o.Entry == LiftController).OrderBy(
                    o => o.DistanceSqr).FirstOrDefault();

            if (controller == null)
            {
                Logger.Write("No lift controller found");
                return;
            }

            if (!GossipFrame.Instance.IsVisible)
                controller.Interact();
            else
            {
                GossipFrame.Instance.SelectGossipOption(floor - 1);
                _liftControllerTimer.Reset();
            }
        }
    }
}