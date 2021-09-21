using System;
using System.Collections.Generic;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Linq;
using TreeSharp;
using Tripper.Tools.Math;
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
    public class LostCityOfTheTolvirHeroic : Dungeon
    {
        #region Overrides of Dungeon

        public override uint DungeonId { get { return 325; } }

        private readonly CircularQueue<WoWPoint> _corpseRun = new CircularQueue<WoWPoint>
                                                              {
                                                                  new WoWPoint(-10661.25, -1301.422, 15.35394),
                                                                  new WoWPoint(-10694.81, -1311.103, 19.476)
                                                              };

        public override CircularQueue<WoWPoint> CorpseRunBreadCrumb { get { return _corpseRun; } }

        public override bool IsFlyingCorpseRun { get { return true; } }

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                ret =>
                {
                    var unit = ret.ToUnit();
                    if (_enslavedBanditIds.Contains(ret.Entry) && !unit.IsTargetingMyPartyMember && !unit.IsTargetingMeOrPet)
                        return true;
                    if (_aughIds.Contains(ret.Entry) && StyxWoW.Me.IsTank())
                        return true;

                    if (ret.Entry == SoulFragment && StyxWoW.Me.IsTank())
                        return true;

                    if (ret.Entry == HighProphetBarim && unit.HasAura(Repentance))
                        return true;

                    if (ret.Entry == Siamat && unit.HasAura("Deflecting Winds"))
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
                    if (unit.Entry == SoulFragment && !StyxWoW.Me.IsTank() && !_ignoreSoulFragments)
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
                    if (unit.Entry == SoulFragment && !StyxWoW.Me.IsTank() && !_ignoreSoulFragments)
                        priority.Score += 500;

                    if (unit.Entry == MinionOfSiamat)
                        priority.Score -= 500;
                }
            }
        }

        #endregion

        private const uint WindTunnel = 48092;
        private const uint EarthQuake = 45126;
        private const uint FrenziedCrocolisk = 43658;
        private const uint SoulFragment = 43934;
        private const uint HarbingerOfDarkness = 43927;
        private const uint HighProphetBarim = 43612;
        private const uint Siamat = 44819;
        private const uint MinionOfSiamat = 44704;
        private const uint ServantOfSiamat = 45259;

        // there are different 'Repentance' auras but Barim gains this version in phase 2
        private const int Repentance = 82320;
        private readonly uint[] _enslavedBanditIds = new uint[]
                                           {
                                               45001,
                                               45007
                                           };
        private readonly uint[] _aughIds = new uint[]
                                           {
                                               45378,
                                               45379
                                           };


        [EncounterHandler(45122, "Oathsworn Captain")]
        public Composite OathswornCaptainEncounter() { return new PrioritySelector(ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, EarthQuake)); }

        private readonly int[] _shockwaveIds = new[] {83445, 91257};

        private WoWUnit _husam;

        [EncounterHandler(44577, "General Husam")]
        public Composite GeneralHusamEncounter()
        {
            const uint landMine = 44796;
            const uint shockwaveVisual = 44712;

            return new PrioritySelector(
                ctx => _husam = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true,
                    8,
                    u => u.Entry == landMine && u.DistanceSqr <= 14*14 && u.ToUnit().HasAura("Tol'vir Land Mine Visual")),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true,
                    3,
                    u =>
                    u.Entry == landMine && u.DistanceSqr <= 10*10 && !u.ToUnit().HasAura("Tol'vir Land Mine Visual")),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true, 5, u => u.Entry == shockwaveVisual && u.DistanceSqr <= 10*10)
                );
        }

        private readonly uint[] _viscousPoisonIds = new uint[] {81630, 90004};

        [EncounterHandler(43614, "Lockmaw")]
        public Composite LockmawEncounter()
        {
            WoWUnit lockmaw = null;

            return new PrioritySelector(
                ctx => lockmaw = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 7, _viscousPoisonIds),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true, 11, u => _aughIds.Contains(u.Entry) && u.ToUnit().HasAura("Whirlwind")),
                // remove the enrage
                new Decorator(
                    ctx => lockmaw.HasAura("Venomous Rage"),
                    new PrioritySelector(
                        new Decorator(
                            ctx => StyxWoW.Me.Class == WoWClass.Hunter && SpellManager.CanCast("Tranqilizing Shot"),
                            new Action(ctx => SpellManager.Cast("Tranqilizing Shot"))),
                        new Decorator(
                            ctx => StyxWoW.Me.Class == WoWClass.Rogue && SpellManager.CanCast("Shiv"),
                            new Action(ctx => SpellManager.Cast("Shiv")))
                        )),
                new Decorator(
                    ctx =>
                    lockmaw.Distance <= lockmaw.MeleeRange() &&
                    WoWMathHelper.IsSafelyBehind(StyxWoW.Me.Location, lockmaw.Location, lockmaw.Rotation),
                    // move to the side to avoid dust flail ability
                    new Action(
                        ctx =>
                            {
                                var bossToMeFacing = WoWMathHelper.CalculateNeededFacing(
                                    lockmaw.Location, StyxWoW.Me.Location);
                                var bossRotation = WoWMathHelper.NormalizeRadian(lockmaw.Rotation);
                                var facingDifference = WoWMathHelper.NormalizeRadian(bossToMeFacing - bossRotation);

                                var moveTo = WoWMathHelper.CalculatePointAtSide(
                                    lockmaw.Location,
                                    lockmaw.Rotation,
                                    (float) lockmaw.Distance,
                                    facingDifference > Math.PI);
                                if (!Navigator.CanNavigateFully(StyxWoW.Me.Location, moveTo))
                                {
                                    moveTo = WoWMathHelper.CalculatePointAtSide(
                                        lockmaw.Location, lockmaw.Rotation, 4f, facingDifference > Math.PI);
                                }
                                if (StyxWoW.Me.Location.DistanceSqr(moveTo) >
                                    Navigator.PathPrecision*Navigator.PathPrecision)
                                {
                                    return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(moveTo));
                                }
                                return RunStatus.Failure;
                            }))
                );
        }


        [EncounterHandler(49045, "Augh")]
        public Composite AughEncounter()
        {
            WoWUnit boss = null;
            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true, 11, u => u == boss && u.ToUnit().HasAura("Whirlwind")),
                // avoid the dragon breath
                ScriptHelpers.GetBehindUnit(
                    ctx =>
                    !ScriptHelpers.IsBossAlive("Lockmaw") && !StyxWoW.Me.IsTank() && boss.IsSafelyFacing(StyxWoW.Me),
                    () => boss)
                );
        }

        private bool _ignoreSoulFragments;

        [EncounterHandler(43612, "High Prophet Barim")]
        public Composite HighProphetBarimEncounter()
        {
            const uint heavensFury = 43801;

            var roomCenterLoc = new WoWPoint(-11007.25, -1283.995, 10.84916);
            WoWUnit boss = null;

            return new PrioritySelector(
                ctx =>
                    {
                        _ignoreSoulFragments =
                            ObjectManager.GetObjectsOfType<WoWUnit>().Any(
                                u => u.Entry == HarbingerOfDarkness && u.HealthPercent <= 30);
                        return boss = ctx as WoWUnit;
                    },
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 6, heavensFury),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => StyxWoW.Me.IsTank(), () => roomCenterLoc, 30, 15, SoulFragment),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx =>
                    !StyxWoW.Me.IsTank() && StyxWoW.Me.HasAura("Soul Sever") &&
                    StyxWoW.Me.Auras["Soul Sever"].TimeLeft <= TimeSpan.FromSeconds(4),
                    () => ScriptHelpers.Tank.Location,
                    40,
                    30,
                    HarbingerOfDarkness),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true,
                    () => roomCenterLoc,
                    30,
                    15,
                    u => u.Entry == HighProphetBarim && u.ToUnit().HasAura(Repentance)),
                ScriptHelpers.CreateDispellParty("Plague of Ages", ScriptHelpers.PartyDispellType.Disease)
                );
        }

        [EncounterHandler(44819, "Siamat")]
        public Composite SiamatEncounter()
        {
            const uint tempestStorm = 44713;

            WoWUnit boss = null;
            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, tempestStorm),
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => true, 12, u => u.Entry == ServantOfSiamat && u.ToUnit().HealthPercent <= 5)
                );
        }

        private readonly WaitTimer _windTunnelTimer = new WaitTimer(TimeSpan.FromSeconds(15));

        public override MoveResult MoveTo(WoWPoint location)
        {
            // wait on party members.
            if (!_windTunnelTimer.IsFinished && StyxWoW.Me.IsTank())
                return MoveResult.Moved;

            var myLoc = StyxWoW.Me.Location;
            var botIsInArea = myLoc.Z > 30;
            var destinationIsInArea = location.Z > 30;
            if (botIsInArea != destinationIsInArea)
            {
                if (!botIsInArea)
                {
                    var windtunnel =
                        ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                            u => u.Entry == WindTunnel && Navigator.CanNavigateFully(StyxWoW.Me.Location, u.Location)).
                            OrderBy(u => u.DistanceSqr).FirstOrDefault();
                    if (windtunnel != null)
                    {
                        if (windtunnel.DistanceSqr > 10*10)
                            return Navigator.MoveTo(windtunnel.Location);
                        windtunnel.Interact();
                        _windTunnelTimer.Reset();
                    }
                    else
                        Logger.Write("No wind tunnel found");
                }
                else
                {
                    // port out - bot will auto port back in. this is the only way to get off this area (without jumping off)
                    Lua.DoString("LFGTeleport(true)");
                }
                return MoveResult.Moved;
            }
            return MoveResult.Failed;
        }
    }
}