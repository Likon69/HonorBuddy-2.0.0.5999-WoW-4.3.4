using System;
using System.Linq;
using CommonBehaviors.Actions;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using TreeSharp;
using Styx;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
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
    public class VortexPinnacle : Dungeon
    {
        #region Overrides of Dungeon

        /// <summary>
        ///   The Map Id of this dungeon. This is the unique id for dungeons thats used to determine which dungeon, the script belongs to
        /// </summary>
        /// <value> The map identifier. </value>
        public override uint DungeonId { get { return 311; } }

        public override WoWPoint Entrance { get { return new WoWPoint(-11523.62f, -2319.026f, 613.0181f); } }

        private CircularQueue<WoWPoint> _corpseRunBreadCrumb;
        /*
        public override CircularQueue<WoWPoint> CorpseRunBreadCrumb
        {
            get
            {
                return _corpseRunBreadCrumb ??
                       (_corpseRunBreadCrumb =
                        new CircularQueue<WoWPoint>
                            {
                                new WoWPoint(-11518.17f, -2166.622f, 513.0512f),
                                new WoWPoint(-11509.4f, -2285.51f, 614.3616f),
                                new WoWPoint(-11523.62f, -2319.026f, 613.0181f)
                            });
            }
        }*/

        public override bool IsFlyingCorpseRun { get { return true; } }

        private const uint LurkingTempest = 45704;
        private const uint HowlingGale = 45572;

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                ret =>
                {
                    if (ret.Entry == LurkingTempest && ret.ToUnit().HasAura("Feign Death")) // can't kill these
                    {
                        if (StyxWoW.Me.GotAlivePet && StyxWoW.Me.Pet.CurrentTarget == ret)
                        {
                            Lua.DoString("PetFollow()");
                        }
                        return true;
                    }
                    // remove howling gale if the knockback aura is disabled.
                    if (ret.Entry == HowlingGale && (HowlingGaleBlacklist.Contains(ret) || ret.DistanceSqr > 30*30))
                    {
                        if (StyxWoW.Me.GotAlivePet && StyxWoW.Me.Pet.CurrentTarget == ret)
                        {
                            Lua.DoString("PetFollow()");
                        }
                        return true;
                    }
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
                    if (unit.Entry == HowlingGale && !HowlingGaleBlacklist.Contains(unit) && unit.DistanceSqr <= 30 * 30)
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
                }
            }
        }

        #endregion

        #region Encounter Handlers

        /// <summary>
        ///   Using 0 as BossEntry will make that composite the main logic for the dungeon and it will be called in every tick You can only have one main logic for a dungeon The context of the main composite is all units around as List <WoWUnit />
        /// </summary>
        /// <returns> </returns>
        [EncounterHandler(0)]
        public Composite RootLogic() { return new PrioritySelector(); }

        private WoWUnit _ertan;

        [EncounterHandler(43878, "Grand Vizier Ertan")]
        public Composite ErtanLogic()
        {
            const int vortexId = 46007;
            return new PrioritySelector(
                ctx => _ertan = ctx as WoWUnit,
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx =>
                    _ertan != null &&
                    ObjectManager.GetObjectsOfType<WoWUnit>().Any(
                        u => u.Entry == vortexId && u.Location.Distance2DSqr(_ertan.Location) <= 10 * 10), 8,
                    u => u == _ertan), ScriptHelpers.CreateRunAwayFromBad(ctx => true, 6, vortexId),
                new Decorator(
                    ctx => ctx is WoWUnit && _ertan.DistanceSqr > 15 * 15,
                    new Sequence(
                        new Action(ctx => Logger.Write("[Ertan Encounter] Getting closer to the boss")),
                        new Action(ctx => Navigator.PlayerMover.MoveTowards((_ertan.Location))))));
        }


        [EncounterHandler(43875, "Asaad")]
        public Composite AsaadLogic()
        {
            const int unstableGroundingFieldSpell = 86911;
            const int supremacyOfTheStorm = 86930;
            const int stormTarget = 46387;
            WoWUnit boss = null;
            WoWPoint groundingFieldLocation = WoWPoint.Zero;

            var groundingFieldWaitTimer = new WaitTimer(TimeSpan.FromSeconds(4));

            return new PrioritySelector(
                ctx =>
                {
                    var stormTargets =
                        ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                            u =>
                            u.Entry == stormTarget && (u.HasAura(86921) || u.HasAura(86923) || u.HasAura(86925))).
                            ToList();
                    if (stormTargets.Any())
                        groundingFieldLocation =
                            stormTargets.Aggregate(
                                WoWPoint.Zero, (current, woWPoint) => current + woWPoint.Location) /
                            stormTargets.Count;
                    else
                        groundingFieldLocation = WoWPoint.Zero;

                    return boss = ctx as WoWUnit;
                },
                new Decorator(
                    ctx =>
                    (boss.CastingSpellId == unstableGroundingFieldSpell || boss.CastingSpellId == supremacyOfTheStorm ||
                     !groundingFieldWaitTimer.IsFinished) && groundingFieldLocation != WoWPoint.Zero,
                    new PrioritySelector(
                        new Sequence(
                            new Action(ctx => Logger.Write("[Asaad Encounter] Running inside grounding field")),
                            new Action(ctx => groundingFieldWaitTimer.Reset()),
                            new Action(ctx => Navigator.PlayerMover.MoveTowards(groundingFieldLocation))),
                // stop behavior here (prevents moving out of triangle to kill adds.
                        new ActionAlwaysSucceed())),
                new Decorator(
                    ctx => groundingFieldWaitTimer.IsFinished,
                    ScriptHelpers.CreateSpreadOutLogic(
                        ctx => groundingFieldWaitTimer.IsFinished, () => boss.Location, 13, 30)));
        }

        [EncounterHandler(45572, "Howling Gale", Mode = CallBehaviorMode.Proximity, BossRange = 30)]
        public Composite HowlingGaleEncounter()
        {
            WoWUnit orb = null;
            return new PrioritySelector(
                ctx => orb = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 45572 && !HowlingGaleBlacklist.Contains(u) && u.DistanceSqr <= 30 * 30).OrderBy(u => u.DistanceSqr).FirstOrDefault(),
                new Decorator(
                    ctx =>StyxWoW.Me.X > -1090.885 && orb != null,
                    new Sequence(
                        new Action(ctx => orb.Target()),
                        new WaitContinue(2, ctx => StyxWoW.Me.CurrentTarget == orb,new ActionAlwaysSucceed()),
                        ScriptHelpers.CreateCastRangedAbility(),
                        new WaitContinue(3,ctx => StyxWoW.Me.IsCasting,new ActionAlwaysSucceed()),
                        new Action(ctx => HowlingGaleBlacklist.Add(orb, TimeSpan.FromSeconds(12))))));
        }

        [EncounterHandler(45919, "Young Storm Dragon")]
        public Composite YoungStormDragonEncounter()
        {
            WoWUnit drake = null;
            WoWDynamicObject healingWell = null;
            return new PrioritySelector(
                ctx =>
                {
                    healingWell = ObjectManager.GetObjectsOfType<WoWDynamicObject>().FirstOrDefault(d => d.Entry == 88201);
                    return drake = ctx as WoWUnit;
                },
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx =>
                    StyxWoW.Me.IsTank() && healingWell != null &&
                    healingWell.Location.Distance2DSqr(drake.Location) <= 20 * 20, () => healingWell.Location, 40,30,
                    u => u == healingWell), ScriptHelpers.CreateTankFaceAwayGroupUnit(17));
        }

        //[EncounterHandler(43878, "Grand Vizier Ertan")]
        //public Composite ErtanLogic()
        //{
        //    const int vortexId = 46007;
        //    return
        //        new PrioritySelector(
        //                ctx =>
        //                    {
        //                        var vortexes =
        //                                    ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(
        //                                        u => u.Entry == vortexId && u.Location.DistanceSqr(((WoWUnit)ctx).Location) > 20 * 20 
        //                                             && u.IsSafelyFacing((WoWUnit)ctx, 60));

        //                        if (vortexes.Count() == 0)
        //                            return ctx;

        //                        var firstTwo = vortexes.OrderBy(u => u.DistanceSqr).Take(2);

        //                        return firstTwo.Select(v => v.Location).Aggregate((p1, p2) => p1 + p2) / firstTwo.Count();
        //                    },
        //            new Decorator(
        //                ctx => ctx is WoWPoint,
        //                new Sequence(
        //                    new Action(ctx => Logger.Write("[Ertan Encounter] Avoiding Vortexes")),
        //                    new Action(ctx => Navigator.PlayerMover.MoveTowards((WoWPoint)ctx)),
        //                    new DecoratorContinue(
        //                        ctx => !ScriptHelpers.Healer.IsMe,
        //                        new WaitContinue(
        //                            10,
        //                            ctx => false,
        //                            new ActionAlwaysSucceed())),
        //                    new WaitContinue(
        //                        3, 
        //                        ctx => false, 
        //                        new ActionAlwaysSucceed())
        //                    )),

        //            new Decorator(
        //                ctx => ctx is WoWUnit && ((WoWUnit)ctx).DistanceSqr > 15 * 15,
        //                new Sequence(
        //                    new Action(ctx => Logger.Write("[Ertan Encounter] Getting closer to the boss")),
        //                    new Action(ctx => Navigator.PlayerMover.MoveTowards(((WoWUnit)ctx).Location))))

        //            );
        //}


        private readonly DungeonArea _firstArea =
            new DungeonArea(
                new Vector2(-375.6963f, 115.0375f), new Vector2(-246.2538f, 6.154482f),
                new Vector2(-556.3135f, -324.7931f), new Vector2(-864.868f, -22.49899f),
                new Vector2(-670.7045f, 150.8543f));

        private readonly DungeonArea _secondArea =
            new DungeonArea(
                new Vector2(-845.3011f, -132.8148f), new Vector2(-944.6406f, -256.0248f),
                new Vector2(-1311.896f, 33.37508f), new Vector2(-1180.948f, 170.9117f));

        private readonly DungeonArea _thirdArea =
            new DungeonArea(
                new Vector2(-532.2311f, 669.4817f), new Vector2(-541.2621f, 380.0819f),
                new Vector2(-1189.979f, 387.2452f), new Vector2(-1211.051f, 649.4243f));

        // leads to middle area (at 1st boss)
        private readonly WoWPoint _firstAreaSlipStreamLoc1 = new WoWPoint(-768.213f, -53.6862f, 639.926f);
        // leads to middle area (at entrance)
        private readonly WoWPoint _firstAreaSlipStreamLoc2 = new WoWPoint(-322.3307, -25.92011, 626.9792);
        // leads to last area
        private readonly WoWPoint _firstAreaSlipStreamLoc3 = new WoWPoint(-379.3557, 30.80832, 626.9794);
        private readonly WoWPoint _secondAreaSlipStreamLoc1 = new WoWPoint(-1197.921, 107.1408, 740.7061);

        private readonly WaitTimer _slipStreamTimer = new WaitTimer(TimeSpan.FromSeconds(15));

        public override MoveResult MoveTo(WoWPoint location)
        {
            var myArea = GetMyArea();
            var destinationArea = GetDestinationArea(location);
            if (destinationArea == myArea)
            {
                var tank = ScriptHelpers.Tank;
                if (tank != null && !tank.IsMe)
                {
                    if (tank.Location.Distance2DSqr(_firstAreaSlipStreamLoc1) <= 20 * 20 || tank.Location.Distance2DSqr(_firstAreaSlipStreamLoc2) <= 20 * 20)
                        destinationArea = _secondArea;

                    if (tank.Location.Distance2DSqr(_firstAreaSlipStreamLoc3) <= 20 * 20 || tank.Location.Distance2DSqr(_secondAreaSlipStreamLoc1) <= 20 * 20)
                        destinationArea = _thirdArea;

                    if (tank.HasAura("Slipstream"))
                    {
                        if (tank.Location.Distance2DSqr(_firstAreaSlipStreamLoc1) <= 60 * 60 || tank.Location.Distance2DSqr(_firstAreaSlipStreamLoc2) <= 60 * 60)
                            destinationArea = _secondArea;

                        if (tank.Location.Distance2DSqr(_firstAreaSlipStreamLoc3) <= 60 * 60 || tank.Location.Distance2DSqr(_secondAreaSlipStreamLoc1) <= 60 * 60)
                            destinationArea = _thirdArea;
                    }
                }
            }

            if (myArea != destinationArea && myArea != null && destinationArea != null)
            {
                if (!_slipStreamTimer.IsFinished )
                    return MoveResult.Moved;

                if (myArea == _firstArea)
                {
                    WoWPoint slipStreamLoc;
                    if (destinationArea == _secondArea)
                    {
                        slipStreamLoc = (!ScriptHelpers.IsBossAlive("Grand Vizier Ertan") ||
                                         GetSlipStreamNearLocation(_firstAreaSlipStreamLoc2) != null) &&
                                        StyxWoW.Me.Location.DistanceSqr(_firstAreaSlipStreamLoc2) <
                                        StyxWoW.Me.Location.DistanceSqr(_firstAreaSlipStreamLoc1)
                                            ? _firstAreaSlipStreamLoc2
                                            : _firstAreaSlipStreamLoc1;
                    }
                    else
                        slipStreamLoc = _firstAreaSlipStreamLoc3;

                    var slipStream = GetSlipStreamNearLocation(slipStreamLoc);
                    if (slipStream != null && slipStream.Distance2DSqr <= 20 * 20)
                    {
                        slipStream.Interact();
                        _slipStreamTimer.Reset();
                    }
                    else
                        return Navigator.MoveTo(slipStreamLoc);
                }

                if (myArea == _secondArea)
                {
                    if (destinationArea == _firstArea && DungeonBot.CurrentLfgDungeonId > 0)
                    {
                        // port out - bot will auto port back in. this is the only way to get to 1st area.
                        Lua.DoString("LFGTeleport(true)");
                    }
                    else if (destinationArea == _thirdArea)
                    {
                        var slipStream = GetSlipStreamNearLocation(_secondAreaSlipStreamLoc1);
                        if (slipStream != null && slipStream.Distance2DSqr <= 20 * 20)
                        {
                            slipStream.Interact();
                            _slipStreamTimer.Reset();
                        }
                        else
                           return Navigator.MoveTo(_secondAreaSlipStreamLoc1);
                    }
                }
                if (myArea == _thirdArea)
                {
                    // port out - bot will auto port back in. this is the only way to get to 1st area.
                    Lua.DoString("LFGTeleport(true)");
                }
                return MoveResult.Moved;
            }
            return MoveResult.Failed;
        }

        private DungeonArea GetMyArea()
        {
            var myLoc = StyxWoW.Me.Location;

            if (_firstArea.IsPointInPoly(myLoc))
                return _firstArea;

            if (_secondArea.IsPointInPoly(myLoc))
                return _secondArea;

            if (_thirdArea.IsPointInPoly(myLoc))
                return _thirdArea;

            return null;
        }

        private DungeonArea GetDestinationArea(WoWPoint destination)
        {

            if (_firstArea.IsPointInPoly(destination))
                return _firstArea;

            if (_secondArea.IsPointInPoly(destination))
                return _secondArea;

            if (_thirdArea.IsPointInPoly(destination))
                return _thirdArea;

            return null;
        }

        private WoWUnit GetSlipStreamNearLocation(WoWPoint point)
        {
            return
                ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                    u => u.Entry == 45455 && u.Location.Distance2DSqr(point) < 25 * 25);
        }


        private static class HowlingGaleBlacklist
        {
            private static readonly Dictionary<ulong, BlacklistTime> _blacklist = new Dictionary<ulong, BlacklistTime>();

            public static void Add(WoWObject obj, TimeSpan time) { _blacklist[obj.Guid] = new BlacklistTime(DateTime.Now, time); }

            public static bool Contains(WoWObject obj)
            {
                _blacklist.RemoveAll(time => time.TimeStamp + time.Duration <= DateTime.Now);
                return _blacklist.ContainsKey(obj.Guid);
            }

            private class BlacklistTime
            {
                public BlacklistTime(DateTime time, TimeSpan duration)
                {
                    TimeStamp = time;
                    Duration = duration;
                }

                public DateTime TimeStamp { get; private set; }
                public TimeSpan Duration { get; private set; }
            }
        }

        #endregion
    }
}