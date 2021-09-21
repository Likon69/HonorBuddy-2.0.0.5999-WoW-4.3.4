using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Combat.CombatRoutine;
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
using Bots.DungeonBuddyDll.Actions;
using Bots.DungeonBuddyDll.Attributes;
using Bots.DungeonBuddyDll.Helpers;

namespace Bots.DungeonBuddyDll.Dungeons.Cataclysm
#else
    using Bots.DungeonBuddy.Attributes;
    using Bots.DungeonBuddy.Helpers;
    using Bots.DungeonBuddy.Actions;
namespace Bots.DungeonBuddy.Dungeons.Cataclysm
#endif
{
    public class Stonecore : Dungeon
    {
        #region Overrides of Dungeon


        /// <summary>
        ///   The Map Id of this dungeon. This is the unique id for dungeons thats used to determine which dungeon, the script belongs to
        /// </summary>
        /// <value> The map identifier. </value>
        public override uint DungeonId { get { return 307; } }

        private readonly CircularQueue<WoWPoint> _corpseRun = new CircularQueue<WoWPoint>
                                                                  {
                                                                      new WoWPoint(934.2088, 638.4651, 171.1208),
                                                                      new WoWPoint(1024.311, 643.19, 162.269),
                                                                      new WoWPoint(1032.611f, 607.3205f, 160.9259f)
                                                                  };


        public override WoWPoint Entrance { get { return new WoWPoint(1032.611f, 607.3205f, 160.9259f); } }

        public override bool IsFlyingCorpseRun { get { return true; } }

        public override CircularQueue<WoWPoint> CorpseRunBreadCrumb { get { return _corpseRun; } }

        /// <summary>
        ///   IncludeTargetsFilter is used to add units to the targeting list. If you want to include a mob thats usually removed by the default filters, you shall use that.
        /// </summary>
        /// <param name="incomingunits"> Units passed into the method </param>
        /// <param name="outgoingunits"> Units passed to the targeting class </param>
        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (WoWObject obj in incomingunits)
            {
                var unit = obj as WoWUnit;
                if (unit == null)
                    continue;

                if (unit.Entry == 42695 && unit.DistanceSqr <= 25 * 25) // Stonecore Sentry
                    outgoingunits.Add(unit);

                if (StyxWoW.Me.IsTank() && unit.Entry == 42428 && (unit.DistanceSqr <= 40 * 40 || unit.Combat)) // devout followers
                    outgoingunits.Add(unit);
            }
        }

        /// <summary>
        ///   RemoveTargetsFilter is used to remove units thats not wanted in target list. Like immune mobs etc.
        /// </summary>
        /// <param name="units"> </param>
        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                o =>
                {
                    var unit = o as WoWUnit;

                    if (unit == null)
                        return false;

                    // for Corborus when he submerges
                    if (unit.Entry == 43438 && unit.HasAura("Submerge"))
                        return true;

                    // For Ozruk fight. Ranged dps should stop dpsing if the boss has spell reflect buff
                    if (unit.Entry == 42188 && unit.HasAura("Elementium Bulwark") && StyxWoW.Me.IsRange() &&
                        StyxWoW.Me.IsDps() && unit.DistanceSqr > 8 * 8)
                        return true;

                    // For Priestess fight. It becomes immune with this shield on
                    if (unit.Entry == 42333 && unit.HasAura("Energy Shield"))
                        return true;

                    if (unit.Entry == 49857) // Emerald Shale Hatchling - level 1 (criter?)
                        return true;

                    return false;
                });
        }

        /// <summary>
        ///   WeighTargetsFilter is used to weight units in the targeting list. If you want to give priority to a certain npc, you should use this method.
        /// </summary>
        /// <param name="units"> </param>
        public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
        {
            foreach (var t in units)
            {
                var prioObject = t.Object;
                var unit = prioObject as WoWUnit;
                if (unit == null)
                    continue;
                // Stonecore Sentry. kill it before it runs for help.
                if (unit.Entry == 42695)
                    t.Score += 5000;

                // Devout Follower. try to pick up aggro.
                if (unit.Entry == 42428 && StyxWoW.Me.IsTank() && unit.Combat && unit.IsTargetingMyPartyMember)
                    t.Score += 5000;
                
                if (unit.Entry == 42691 && StyxWoW.Me.IsDps()) // Rift Conjurer.
                    t.Score += 4000;

                if (unit.Entry == 43014 && StyxWoW.Me.IsDps()) // Imp
                    t.Score += 4100;

            }
        }

        #endregion

        #region Encounter Handlers

        private readonly WaitTimer _teleportTimer = new WaitTimer(TimeSpan.FromSeconds(10));
        private WoWUnit _azil;
        private WoWUnit _corborus;
        private WoWUnit _ozruk;

        private WoWUnit _slabhide;
        private WoWGameObject _stalactite;

        [EncounterHandler(0, "RootBehavior")]
        public Composite RootBehavior()
        {
            return new PrioritySelector(
                //ScriptHelpers.CreateRunAwayFromBad(ctx => !StyxWoW.Me.IsHealer(), 20, u => u is WoWPlayer && u.ToPlayer().IsInMyParty && u.ToPlayer().IsCasting),
                // Stalactite - this stuff blocks path so we need to go around.
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 2, u => u.Entry == 204337 && u.DistanceSqr <= 5 * 5));
        }

        /// <summary>
        ///   BossEntry is the Entry of the boss unit. (WoWUnit.Entry) BossName is optional. Its there just to make it easier to find which boss that composite belongs to. The context of the encounter composites is the Boss as WoWUnit
        /// </summary>
        /// <returns> </returns>
        [EncounterHandler(43438, "Corborus")]
        public Composite CorborusFight()
        {
            var roomCenterLoc = new WoWPoint(1155.49, 881.957, 284.963);

            return new PrioritySelector(
                ctx => _corborus = ctx as WoWUnit,
                // ScriptHelpers.CreateRunAwayFromBad(ctx => _corborus != null && _corborus.HasAura("Submerge"),() => roomCenterLoc,30, 15, 43438),
                // Thrashing Charge. This is the rubble that apears when Corborus submerges.
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 6, 86881, 92648) // Crystal Barrage
                );
        }


        [EncounterHandler(42692, " Stonecore Bruiser")]
        public Composite StonecoreBruiserEncounter() { return new PrioritySelector(ScriptHelpers.CreateTankFaceAwayGroupUnit(15)); }

        [EncounterHandler(43214, "Slabhide")]
        public Composite SlabhideFight()
        {
            return new PrioritySelector(
                ctx =>
                {
                    _slabhide = (WoWUnit)ctx;
                    _stalactite =
                        ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                            g =>
                            g.Entry == 204337 && Math.Abs(g.Z - StyxWoW.Me.Z) < 7 && // we want to skip any stalacite that are still hanging on ceiling
                            g.Location.DistanceSqr(_slabhide.Location) > 15 * 15).OrderBy(g => g.DistanceSqr).
                            FirstOrDefault();
                    return _slabhide;
                }, // falling rock - because there is so many we only check nearby ones to increase performance
                ScriptHelpers.CreateRunAwayFromBad(
                    ctx => _slabhide != null && _slabhide.Combat, 3, u => u.Entry == 80654 && u.DistanceSqr <= 10 * 10),
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 7, 43242), // Lava Fissure
                // run los at a stalactite when boss is casting Crystal storm
                ScriptHelpers.CreateLosLocation(
                    ctx => _slabhide.CastingSpellId == 92265 && _stalactite != null, () => _slabhide.Location,
                    () => _stalactite.Location, () => 5),
                new Decorator(
                    ctx => Math.Abs(_slabhide.Z - StyxWoW.Me.Z) < 7,
                    new PrioritySelector(
                // healer moves to boss location if tank is not in LOS
                        new Decorator(
                            ctx => StyxWoW.Me.IsHealer && !ScriptHelpers.Tank.InLineOfSpellSight,
                            new Sequence(
                                new ActionLogger("Tank is not in LOS, moving to boss"),
                                new Action(ctx => Navigator.MoveTo(_slabhide.Location)))),
                        new Decorator(
                            ctx => StyxWoW.Me.IsDps() && !ScriptHelpers.Healer.InLineOfSpellSight,
                            new Sequence(
                                new ActionLogger("Healer is not in LOS, moving to healer"),
                                new Action(ctx => Navigator.MoveTo(ScriptHelpers.Healer.Location)))),
                // only tank should be in front when boss is not flying to avoid the Sand Blast abiliy
                        ScriptHelpers.CreateTankFaceAwayGroupUnit(17),
                        ScriptHelpers.GetBehindUnit(
                            ctx =>
                            !StyxWoW.Me.IsTank() && StyxWoW.Me.IsMelee() && _slabhide.IsSafelyFacing(StyxWoW.Me, 65),
                            () => _slabhide))));
        }

        [EncounterHandler(42188, "Ozruk", Mode = CallBehaviorMode.CurrentBoss)]
        public Composite OzrukFight()
        {
            List<WoWUnit> trash = null, sentries = null;
            var trashRoomCenter = new WoWPoint(1558.568, 1154.293, 215.8544);
            var trashLoc1 = new WoWPoint(1530.955, 1163.377, 217.9972);
            var trashTankLoc1 = new WoWPoint(1504.78, 1173.021, 223.4944);
            var trashWaitLoc1 = new WoWPoint(1497.379, 1193.197, 226.1709);

            return new PrioritySelector(
                ctx => _ozruk = ctx as WoWUnit,
                new Decorator(
                    ctx => _ozruk != null && _ozruk.Combat, new PrioritySelector(
                // run out when boss is casting shatter
                                                                ScriptHelpers.CreateRunAwayFromBad(
                                                                    ctx => IsCastingShatter(_ozruk), 17, 42188),
                // run away from ground slamp impact location.
                                                                ScriptHelpers.CreateRunAwayFromLocation(
                                                                    ctx => IsCastingGroundSlam(_ozruk), 8,
                                                                    () =>
                                                                    WoWMathHelper.GetPointAt(
                                                                        _ozruk.Location, 6, _ozruk.Rotation, 0)))),
                // clear trash leading to boss.
                new Decorator(
                    ctx => _ozruk == null || !_ozruk.Combat, new PrioritySelector(
                                                                 ctx =>
                                                                 {
                                                                     sentries =
                                                                         ScriptHelpers.GetUnfriendlyNpsAtLocation(
                                                                             () => trashRoomCenter, 100,
                                                                             u => u.Entry == 42695);
                                                                     return
                                                                         trash =
                                                                         ScriptHelpers.GetUnfriendlyNpsAtLocation(
                                                                             () => trashLoc1, 40);
                                                                 }
                /*
ScriptHelpers.CreatePullNpcToLocation(
ctx => sentries.Any() && sentries[0].DistanceSqr <= 50 * 50 && !trash.Any(t => t.Location.DistanceSqr(sentries[0].Location) <= 30 * 30),
ctx => true,
() => sentries[0], () => trashTankLoc1, () => trashWaitLoc1, 10),

ScriptHelpers.CreatePullNpcToLocation(
ctx => trash.Any(),
ctx => !StyxWoW.Me.IsActuallyInCombat , // && !sentries.Any(u => u.Location.DistanceSqr(trash[0].Location) <= 30*30),
() => trash[0], () => trashTankLoc1, () => trashWaitLoc1, 10) */)));
        }

        private bool IsCastingGroundSlam(WoWUnit unit) { return unit != null && (unit.CastingSpellId == 78903 || unit.CastingSpellId == 92410); }

        private bool IsCastingShatter(WoWUnit unit) { return unit != null && (unit.CastingSpellId == 78807 || unit.CastingSpellId == 92662); }

        [EncounterHandler(42333, "High Priestess Azil", Mode = CallBehaviorMode.Proximity, BossRange = 75)]
        public Composite HighPriestessAzilFight()
        {
            List<WoWUnit> trash = null;
            List<WoWUnit> gravityWells = null, aggro = null;
            var trashLoc = new WoWPoint(1327.616, 990.7188, 207.9859);
            var trashTankLoc = new WoWPoint(1292.835, 990.944, 207.9722);
            var trashWaitLoc = new WoWPoint(1292.202, 1013.337, 209.1433);
            bool hasAggroInMelee = false;

            return new PrioritySelector(
                ctx =>
                {
                    // Devout Follower
                    trash =
                        ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 42428).OrderBy(
                            u => u.Location.DistanceSqr(trashTankLoc)).ToList();
                    aggro = trash.Where(u => u.DistanceSqr <= 50 * 50 && u.CurrentTarget == StyxWoW.Me).ToList();
                    hasAggroInMelee = aggro.Any(a => a.DistanceSqr <= 7 * 7);
                    if (aggro.Any())
                    {
                        gravityWells =
                            ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                                u =>
                                u.Entry == 42499 && u.DistanceSqr <= 30 * 30 &&
                                Navigator.CanNavigateFully(
                                    StyxWoW.Me.Location,
                                    WoWMathHelper.CalculatePointFrom(aggro[0].Location, u.Location, -12))).OrderBy(
                                        u => u.DistanceSqr).ToList();
                    }
                    return _azil = ctx as WoWUnit;
                },
                // boss combat behavior
                new Decorator(
                    ctx => _azil.Combat,
                    new PrioritySelector(
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 10, 42499), // Gravity Well

                        ScriptHelpers.CreateRunAwayFromBad(
                            ctx => _azil != null && _azil.HasAura("Energy Shield"), 5, 42333),

                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 8, 80511), // Seismic Shard Targeting

                        new Decorator(ctx => StyxWoW.Me.IsHealer && hasAggroInMelee,
                            new PrioritySelector(
                                new Decorator(ctx => SpellManager.CanCast("Fade"),
                                    new Action(ctx => SpellManager.Cast("Fade"))),
                                new Decorator(ctx => SpellManager.CanCast("Hand of Salvation"),
                                    new Action(ctx => SpellManager.Cast("Hand of Salvation", StyxWoW.Me)))
                                )),
                      //  new Decorator(ctx => StyxWoW.Me.IsTank(),
                       //     ScriptHelpers.CreateClearArea(() => trashTankLoc, 100, u => u != _azil && u.IsTargetingMyPartyMember)),
                // kite adds in to gravity well
                        ScriptHelpers.CreateLosLocation(
                            ctx =>
                            !StyxWoW.Me.IsHealer && _azil.HasAura("Energy Shield") && aggro.Any() && gravityWells != null && gravityWells.Any(),
                            () => aggro[0].Location, () => gravityWells[0].Location, () => 12), // run to tank

                        //new Decorator(
                //    ctx => aggro.Any() && StyxWoW.Me.IsFollower() && ScriptHelpers.Tank.Distance > 10,
                //    new Action(ctx => Navigator.MoveTo(ScriptHelpers.Tank.Location)))
                    new Decorator(ctx => StyxWoW.Me.IsTank() && Targeting.Instance.FirstUnit == null,
                        new Action(ctx => ScriptHelpers.MoveTankTo(trashTankLoc)))
                            )),

                // trash pull before boss encounter is started.
                new Decorator(ctx => !_azil.Combat, new PrioritySelector(
                // give healer vigilane if warrior
                        new Decorator(ctx => StyxWoW.Me.Class == WoWClass.Warrior && SpellManager.CanCast("Vigilance"),
                            new Action(ctx => SpellManager.Cast("Vigilance", ScriptHelpers.Healer))),

                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => !StyxWoW.Me.IsActuallyInCombat && trash.Any(), ctx => true,
                            () => trash[0], () => trashTankLoc, () => trashWaitLoc, 2),
                // pull the boss
                        new Decorator(
                            ctx => StyxWoW.Me.IsTank(),
                            ScriptHelpers.CreatePullNpcToLocation(
                                ctx => !trash.Any() && StyxWoW.Me.PartyMembers.All(p => p.IsAlive && p.HealthPercent > 80), ctx => true, () => _azil, () => trashLoc, null, 0))
                    )));
        }

        public override MoveResult MoveTo(WoWPoint location)
        {
            // use entrance portal.
            if ((StyxWoW.Me.X < 900 && location.X > 1200 || StyxWoW.Me.Y > 1200 && location.Y < 900) &&
                _teleportTimer.IsFinished)
            {
                var teleporter =
                    ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                        o => (o.Entry == 51396 || o.Entry == 51397) && o.HasAura("Teleporter Active Visual"));
                if (teleporter != null)
                {
                    if (!teleporter.WithinInteractRange)
                        return Navigator.MoveTo(teleporter.Location);
                    teleporter.Interact();
                    _teleportTimer.Reset();
                    return MoveResult.Moved;
                }
            }
            return MoveResult.Failed;
        }

        #endregion
    }
}