using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
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
    public class GrimBatol : Dungeon
    {
        #region Overrides of Dungeon

        public override uint DungeonId { get { return 304; } }

        public override WoWPoint Entrance { get { return new WoWPoint(-4040.428, -3448.707, 293.7093); } }

        public override bool IsFlyingCorpseRun { get { return true; } }

        private readonly CircularQueue<WoWPoint> _corpseRun = new CircularQueue<WoWPoint>
                                                              {
                                                                  new WoWPoint(
                                                                      -4141.175,
                                                                      -3636.998,
                                                                      230.4903),
                                                                  new WoWPoint(
                                                                      -4112.387,
                                                                      -3496.556,
                                                                      272.9308),
                                                                  new WoWPoint(
                                                                      -4054.679,
                                                                      -3450.042,
                                                                      294.9305),
                                                                  new WoWPoint(
                                                                      -4040.428, -3448.707, 293.7093)
                                                              };

        //public override CircularQueue<WoWPoint> CorpseRunBreadCrumb { get { return _corpseRun; } }

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                ret =>
                    {
                        if (ret.Entry == TwilightDrake && !ret.ToUnit().Combat)
                            return true;

                        if (ret.Entry == TroggDweller && !ret.ToUnit().Combat)
                            return true;

                        if (ret.Entry == DrahgaShadowburner && ret.ToUnit().HasAura("Ride Vehicle") &&
                            !StyxWoW.Me.IsRange())
                            return true;

                        if (ret.Entry == FacelessCorruptor && ret.ToUnit().HasAura("Shield of Nightmares"))
                            return true;

                        return false;
                    });
        }

        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (var obj in incomingunits)
            {
                var unit = obj as WoWUnit;
                if (unit == null)
                    continue;
                if (unit.Entry == Net && !StyxWoW.Me.Combat && unit.DistanceSqr <= 25*25)
                    outgoingunits.Add(unit);

                if (unit.Entry == InvokedFlamingSpirit)
                    outgoingunits.Add(unit);

                if (unit.Entry == FacelessCorruptor)
                    outgoingunits.Add(unit);

                if (_firstBossTrashIds.Contains(unit.Entry) && StyxWoW.Me.IsTank() && unit.DistanceSqr < 40*40)
                    outgoingunits.Add(unit);
            }
        }

        public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
        {
            foreach (var priority in units)
            {
                var unit = priority.Object as WoWUnit;
                if (unit != null)
                {
                    if (unit.Entry == InvokedFlamingSpirit)
                        priority.Score += 500;
                    if (unit.Entry == FacelessCorruptor)
                        priority.Score += 500;
                }
            }
        }

        #endregion

        private const uint BatteredRedDrake = 42571;
        private const uint BatteredRedDrakeBomber = 39294;
        private const uint Net = 42570;
        private const uint BlitzStalker = 40040;
        private const uint CaveInStalker = 40228;
        private const uint TroggDweller = 39450;
        private const int EngulfingFlames = 74039;
        private const uint DrahgaShadowburner = 40319;
        private const uint InvocationOfFlame = 40355;
        private const uint InvokedFlamingSpirit = 40357;
        private const uint SeepingTwilight = 40365;
        private const uint ValionasFlame = 75321;
        private const uint DevouringFlames = 90950;
        private const uint FacelessCorruptor = 40600;
        private const uint LavaPatch = 90754;
        private const uint TwilightDrake = 39390;

        private readonly uint[] _firstBossTrashIds = new uint[]
                                                     {
                                                         39405, // CrimsonborneSeer
                                                         39854, // AzureborneGuardian
                                                         39956, // TwilightEnforcer
                                                         39954, // TwilightShadowWeaver
                                                         41073, // Twilight Armsmaster
                                                         39626, // Crimsonborne Warlord
                                                         39909, // Azureborne Warlord
                                                         40167, // Twilight Beguiler
                                                     };

        private readonly WoWPoint _drakeAreaLoc = new WoWPoint(-455.7566, -356.561, 268.1429);

        [EncounterHandler(39294, "Battered Red Drake", Mode = CallBehaviorMode.Proximity, BossRange = 100)]
        public Composite DrakeHandler()
        {
            List<WoWUnit> trash = null;
            WoWUnit drake = null;
            WoWUnit bestBombTarget = null;

            return
                new PrioritySelector(
                    ctx =>
                    trash =
                    ScriptHelpers.GetUnfriendlyNpsAtLocation(
                        () => _drakeAreaLoc, 40, u => u.Entry != BatteredRedDrakeBomber),
                    new Decorator(ctx => trash.Any(), new Action(ctx => ScriptHelpers.MoveTankTo(trash[0].Location))),
                    // no trash left to kill by the drakes so we mount the nearest one and start bombing away.
                    new Decorator(
                        ctx => !trash.Any(),
                        new PrioritySelector(
                            ctx =>
                            drake =
                            ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                                u => u.Entry == BatteredRedDrakeBomber && u.OwnedByUnit == null).OrderBy(
                                    u => u.DistanceSqr).
                                FirstOrDefault(),
                            // Mount a drake
                            new Decorator(
                                ctx => !StyxWoW.Me.IsOnTransport && drake != null,
                                ScriptHelpers.CreateTalkToNpc(() => drake)),
                            // bombing behavior
                            new Decorator(
                                ctx => StyxWoW.Me.IsOnTransport,
                                new PrioritySelector(
                                    ctx =>
                                        {
                                            using (new FrameLock())
                                            {
                                                var targets =
                                                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                                                        u =>
                                                        u.CanSelect && u.Attackable && u.IsAlive &&
                                                        u.DistanceSqr <= 100*100).ToList();
                                                return
                                                    bestBombTarget =
                                                    targets.OrderByDescending(
                                                        u =>
                                                        targets.Count(t => u.Location.DistanceSqr(t.Location) <= 15*15))
                                                        .FirstOrDefault();
                                            }
                                        },
                                    new Decorator(
                                        ctx => bestBombTarget != null,
                                        new Action(
                                            ctx =>
                                                {
                                                    Lua.DoString(
                                                        "if GetSpellCooldown(74039) == 0 then CastSpellByID(74039) end");
                                                    if (StyxWoW.Me.CurrentPendingCursorSpell != null)
                                                        LegacySpellManager.ClickRemoteLocation(bestBombTarget.Location);
                                                })),
                                    // wait for party members to come to you
                                    new Decorator(
                                        ctx => StyxWoW.Me.PartyMembers.Any(p => p.IsOnTransport),
                                        new ActionAlwaysSucceed()))))));
        }

        [EncounterHandler(39625, "General Umbriss", Mode = CallBehaviorMode.Proximity)]
        public Composite GeneralUmbrissEncounter()
        {
            WoWUnit boss = null;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, BlitzStalker),
                // move away from front of boss if boss is casting ground siege
                new Decorator(
                    ctx =>
                    (boss.CastingSpellId == 74634 || boss.CastingSpellId == 90249) &&
                    boss.IsSafelyFacing(StyxWoW.Me, 65),
                    new Action(
                        ctx =>
                            {
                                var bossToMeFacing = WoWMathHelper.CalculateNeededFacing(
                                    boss.Location, StyxWoW.Me.Location);
                                var bossRotation = WoWMathHelper.NormalizeRadian(boss.Rotation);
                                var facingDifference = WoWMathHelper.NormalizeRadian(bossToMeFacing - bossRotation);

                                var moveTo = WoWMathHelper.CalculatePointAtSide(
                                    boss.Location, boss.Rotation, (float) boss.Distance, facingDifference > Math.PI);
                                // make sure
                                if (StyxWoW.Me.Location.DistanceSqr(moveTo) >
                                    Navigator.PathPrecision*Navigator.PathPrecision)
                                {
                                    return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(moveTo));
                                }
                                return RunStatus.Failure;
                            })),
                // clear trash in room
                ScriptHelpers.CreateClearArea(() => boss.Location, 80, u => u != boss)
                );
        }

        [EncounterHandler(40177, "Forgemaster Throngus", Mode = CallBehaviorMode.Proximity, BossRange = 160)]
        public Composite ForgemasterThrongusEncounter()
        {
            var trashAreaLoc1 = new WoWPoint(-562.7138, -487.6557, 276.5972);
            var trashTankLoc1 = new WoWPoint(-593.9854, -458.2272, 276.6417);
            var trashFollowerLoc1 = new WoWPoint(-604.5106, -460.4048, 276.5819);
            var bossTankLoc = new WoWPoint(-546.5388, -475.6344, 276.5972);

            List<WoWUnit> trash = null;
            WoWUnit boss = null;

            return new PrioritySelector(
                ctx =>
                    {
                        boss = ctx as WoWUnit;
                        trash = ScriptHelpers.GetUnfriendlyNpsAtLocation(() => trashAreaLoc1, 40, u => u != boss);
                        return boss;
                    },
                new Decorator(
                    ctx => StyxWoW.Me.X > -616f && StyxWoW.Me.Y < -455f,
                    new PrioritySelector(
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 9, CaveInStalker),
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 5, LavaPatch),
                        // heroic ability
                        ScriptHelpers.CreateDispellParty("Burning Flames", ScriptHelpers.PartyDispellType.Magic),
                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => trash != null && trash.Any(),
                            ctx => boss.Location.DistanceSqr(trash[0].Location) > 40*40,
                            () => trash[0],
                            () => trashTankLoc1,
                            () => StyxWoW.Me.IsTank() ? trashTankLoc1 : trashFollowerLoc1,
                            5),
                        new Decorator(
                            ctx => StyxWoW.Me.IsTank(),
                            ScriptHelpers.CreatePullNpcToLocation(
                                ctx => trash == null || !trash.Any(),
                                ctx => boss.DistanceSqr <= 30*30,
                                () => boss,
                                () => bossTankLoc,
                                () => bossTankLoc,
                                3)))));
        }

        [EncounterHandler(40319, "Drahga Shadowburner")]
        public Composite DrahgaShadowburnerEncounter()
        {
            WoWUnit valionas = null;
            var roomCenterLoc = new WoWPoint(-434.9266, -699.7949, 268.7657);
            return new PrioritySelector(
                ctx =>
                    {
                        valionas = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 40320);
                        return ctx;
                    },
                // normal mode ability
                new Decorator(
                    ctx =>
                    valionas != null && valionas.CastingSpellId == ValionasFlame &&
                    valionas.IsSafelyFacing(StyxWoW.Me, 45),
                    new Action(
                        ctx =>
                            {
                                var bossToMeFacing = WoWMathHelper.CalculateNeededFacing(
                                    valionas.Location, StyxWoW.Me.Location);
                                var bossRotation = WoWMathHelper.NormalizeRadian(valionas.Rotation);
                                var facingDifference = WoWMathHelper.NormalizeRadian(bossToMeFacing - bossRotation);

                                var moveTo = WoWMathHelper.CalculatePointAtSide(
                                    valionas.Location,
                                    valionas.Rotation,
                                    (float) valionas.Distance,
                                    facingDifference > Math.PI);
                                if (!Navigator.CanNavigateFully(StyxWoW.Me.Location, moveTo))
                                {
                                    moveTo = WoWMathHelper.CalculatePointAtSide(
                                        valionas.Location, valionas.Rotation, 4f, facingDifference > Math.PI);
                                }
                                // make sure
                                if (StyxWoW.Me.Location.DistanceSqr(moveTo) >
                                    Navigator.PathPrecision*Navigator.PathPrecision)
                                {
                                    return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(moveTo));
                                }
                                return RunStatus.Failure;
                            })),
                // heroic mode ability
                ScriptHelpers.GetBehindUnit(
                    ctx =>
                    valionas != null && valionas.CastingSpellId == DevouringFlames &&
                    valionas.IsSafelyFacing(StyxWoW.Me, 160),
                    () => valionas),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => !StyxWoW.Me.PartyMembers.Any(p => p.HasAura("Flaming Fixate")),
                    () => roomCenterLoc,
                    30,
                    15,
                    InvocationOfFlame),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => StyxWoW.Me.HasAura("Flaming Fixate"), () => roomCenterLoc, 30, 15, InvokedFlamingSpirit),
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, () => roomCenterLoc, 30, 7, SeepingTwilight));
        }

        private readonly int[] _bindingShadows = new[]
                                                 {
                                                     79466,
                                                     91081,
                                                 };

        private WoWUnit _erudax;

        [EncounterHandler(40484, "Erudax")]
        public Composite ErudaxEncounter()
        {
            WoWUnit shadowGaleStalker = null;
            return new PrioritySelector(
                ctx =>
                    {
                        shadowGaleStalker =
                            ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 40567);
                        return _erudax = ctx as WoWUnit;
                    },
                // run away when erudax is casting the root/lifesteal ability 
                ScriptHelpers.CreateRunAwayFromLocation(
                    ctx =>
                    _erudax != null && _bindingShadows.Contains(_erudax.CastingSpellId) &&
                    _erudax.CurrentTargetGuid != 0
                    ,
                    10,
                    () =>
                    WoWMathHelper.CalculatePointBehind(
                        _erudax.CurrentTarget.Location, _erudax.CurrentTarget.Rotation, 2)),
                // run to the center of the eye if boss is casting Shadow Gale
                new Decorator(
                    ctx =>
                    (_erudax.CastingSpellId == 75664 || _erudax.CastingSpellId == 91086) && shadowGaleStalker != null,
                    new Action(
                        ctx => Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(shadowGaleStalker.Location)))),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => StyxWoW.Me.HasAura("Feeble Body") && _erudax != null && _erudax.CurrentTarget == StyxWoW.Me,
                    20,
                    40484),
                new Decorator(
                    ctx => !_erudax.Combat,
                    ScriptHelpers.CreateClearArea(() => _erudax.Location, 100, u => u != _erudax))
                );
        }

        private const int MinDistanceOfTravelToUseDrakeSqr = 200*200;
        private readonly WaitTimer _drakeTimer = new WaitTimer(TimeSpan.FromSeconds(15));

        public override MoveResult MoveTo(WoWPoint location)
        {
            // wait for followers to catch up
            if (!_drakeTimer.IsFinished && StyxWoW.Me.IsTank())
                return MoveResult.Moved;

            var myLoc = StyxWoW.Me.Location;
            var tank = ScriptHelpers.Tank;
            if (myLoc.DistanceSqr(location) > MinDistanceOfTravelToUseDrakeSqr && location.Y < -356.561 ||
                tank != null && !tank.IsMe && tank.CharmedUnit != null && tank.CharmedUnit.Entry == BatteredRedDrake)
            {
                var drake = ObjectManager.GetObjectsOfType<WoWUnit>()
                    .Where(
                        u =>
                        u.Entry == BatteredRedDrake && u.OwnedByUnit == null && u.DistanceSqr <= 40*40 &&
                        u.Location.DistanceSqr(_drakeAreaLoc) <= 50*50)
                    .OrderBy(u => u.DistanceSqr).FirstOrDefault();
                if (drake != null)
                {
                    if (drake.DistanceSqr > 5*5)
                        return Navigator.MoveTo(drake.Location);
                    drake.Interact();
                    _drakeTimer.Reset();
                    return MoveResult.Moved;
                }
            }
            return MoveResult.Failed;
        }
    }
}