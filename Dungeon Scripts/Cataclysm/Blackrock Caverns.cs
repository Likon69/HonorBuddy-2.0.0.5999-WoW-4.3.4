using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
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
    public class BlackrockCaverns : Dungeon
    {
        #region Overrides of Dungeon

        /// <summary>
        ///   The Map Id of this dungeon. This is the unique id for dungeons thats used to determine which dungeon, the script belongs to
        /// </summary>
        /// <value> The map identifier. </value>
        public override uint DungeonId { get { return 303; } }

        public override WoWPoint Entrance { get { return new WoWPoint(-7570.482f, -1330.446f, 246.5363f); } }
        public override WoWPoint ExitLocation { get { return new WoWPoint(-213.96f, -1140.39f, 206.65f); } }

        /// <summary>
        ///   IncludeTargetsFilter is used to add units to the targeting list. If you want to include a mob thats usually removed by the default filters, you shall use that.
        /// </summary>
        /// <param name="incomingunits"> Units passed into the method </param>
        /// <param name="outgoingunits"> Units passed to the targeting class </param>
        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (WoWObject obj in incomingunits)
            {
                // For the Rom'ogg boss fight
                if (obj.Entry == ChainsOfWoe)
                {
                    outgoingunits.Add(obj);
                }
            }
        }

        /// <summary>
        ///   RemoveTargetsFilter is used to remove units thats not wanted in target list. Like immune mobs etc.
        /// </summary>
        /// <param name="units"> The incomingunits. </param>
        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                o =>
                    {
                        var unit = o as WoWUnit;

                        if (unit == null)
                            return false;


                        if (_chainsOfWoeIsUp && unit.Entry != ChainsOfWoe)
                            return true;

                        if (unit.Entry == TwilightZealot && unit.HasAura("Kneeling in Supplication"))
                            return true;

                        if (unit.HasAura("Shadow of Obsidius"))
                            return true;

                        if (unit.Entry == BellowsSLave && !unit.Combat)
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

                //We should prio Chains of Woe for Rom'ogg fight
                if (prioObject.Entry == ChainsOfWoe)
                {
                    t.Score += 400;
                }
            }
        }

        #endregion

        #region Encounter Handlers

        private const uint ChainsOfWoe = 40447;
        private const int Bonecrusher = 39665;
        private const uint EvolvedTwilightZealot = 39987;
        private const uint TwilightZealot = 50284;
        private const uint BellowsSLave = 40084;
        private const uint Runty = 40015;
        private const uint Conflagration = 39994;
        private const uint Quake = 40401;

        private readonly WoWPoint _jumpPointStartLoc = new WoWPoint(550.5932, 926.8296, 169.5558);
        private readonly WoWPoint _jumpPointEndLoc = new WoWPoint(556.8304, 936.9685, 159.3673);

        /// <summary>
        ///   Using 0 as BossEntry will make that composite the main logic for the dungeon and it will be called in every tick You can only have one main logic for a dungeon The context of the main composite is all units around as List <WoWUnit />
        /// </summary>
        /// <returns> </returns>
        [EncounterHandler(0)]
        public Composite RootLogic()
        {
            WoWUnit pathingZealot1 = null;

            var patByJumpLoc = new WoWPoint(537.9449, 908.9613, 169.5618);
            var pathingZealot1TankLoc = new WoWPoint(537.6758, 908.2151, 169.5618);

            return
                new PrioritySelector(
                    // ScriptHelpers.CreateTeleporterLogic(51340),
                    ScriptHelpers.CreateRunAwayFromBad(ctx => true, 10, 76628, 93666), // Lava Drool
                    // handle jump before 2nd boss.
                    new Decorator(
                        ctx => StyxWoW.Me.Location.DistanceSqr(_jumpPointStartLoc) <= 36*36,
                        new PrioritySelector(
                            // this is the 1st evolved twilight zealot on the top walkway
                            ctx => pathingZealot1 = ScriptHelpers.GetUnfriendlyNpsAtLocation(() => patByJumpLoc, 60, u => u.Entry == EvolvedTwilightZealot && u.Z > 165).FirstOrDefault(),
                            // kill the 1st pathing zealot before jumping.
                            ScriptHelpers.CreatePullNpcToLocation(
                                ctx => StyxWoW.Me.IsTank() && pathingZealot1 != null,
                                ctx => pathingZealot1.Location.DistanceSqr(pathingZealot1TankLoc) <= 35*35,
                                () => pathingZealot1, () => pathingZealot1TankLoc, () => pathingZealot1TankLoc, 5),
                            // handle the jump if tank decides to jump.
                            ScriptHelpers.CreateJumpDown(
                                ctx => Targeting.Instance.FirstUnit == null && pathingZealot1 == null &&
                                       StyxWoW.Me.PartyMembers.Count(u => u.DistanceSqr <= 40*40) == 4, _jumpPointStartLoc, _jumpPointEndLoc)))
                    );
        }

        private readonly Predicate<WoWUnit> _isCastingSkullCracker = u => u != null && (u.CastingSpellId == 75543 || u.CastingSpellId == 93453);

        private WoWUnit _romogg;
        bool _chainsOfWoeIsUp;

        /// <summary>
        ///   BossEntry is the Entry of the boss unit. (WoWUnit.Entry) BossName is optional. Its there just to make it easier to find which boss that composite belongs to. The context of the encounter composites is the Boss as WoWUnit Mode controls when the behavior is called. The modes are Combat(default), Proximity and CurrentBoss. If using CurrentBoss mode then the behavior is called when is looking for the boss (and durring combat). In this mode boss might not even be in ObjectManager and context can be null.
        /// </summary>
        /// <returns> </returns>
        [EncounterHandler(39665, "Rom'ogg Bonecrusher", Mode = CallBehaviorMode.Proximity, BossRange = 200)]
        public Composite RomoggEncounter()
        {
            var bossFarPathEndLoc = new WoWPoint(259.266, 911.854, 191.091);
            var trashTankLoc = new WoWPoint(201.7278, 998.2936, 195.0932);
            var tankBossLoc = new WoWPoint(208.4425, 964.6042, 190.9823);
            var partyWaitLoc = new WoWPoint(201.3972, 1008.957, 197.0022);
            WoWUnit trash = null;

            return new PrioritySelector(
                ctx => _romogg = ctx as WoWUnit,
                // Clear the area and then pull boss.
                new Decorator(
                    ctx => !_romogg.Combat,
                    new PrioritySelector(
                        ctx => trash = ScriptHelpers.GetUnfriendlyNpsAtLocation(() => tankBossLoc, 50, u => u != _romogg).FirstOrDefault(),
                // pull the trash when boss is far away
                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => trash != null && !StyxWoW.Me.Combat,
                            ctx => _romogg.Location.DistanceSqr(bossFarPathEndLoc) <= 25 * 25 || trash.Location.DistanceSqr(trashTankLoc) <= 50 * 50,
                            () => trash, () => trashTankLoc, () => StyxWoW.Me.IsTank() ? trashTankLoc : partyWaitLoc, 5),
                // pull boss when he's near and trash is cleared.

                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => StyxWoW.Me.IsTank() && trash == null && StyxWoW.Me.Location.DistanceSqr(tankBossLoc) <= 50 * 50,
                            ctx => _romogg.Location.DistanceSqr(tankBossLoc) <= 25 * 25,
                            () => _romogg, () => tankBossLoc, () => StyxWoW.Me.IsTank() ? tankBossLoc : StyxWoW.Me.Location, 5)
                        )),
                // Handle boss encounter
                new Decorator(
                    ctx => _romogg.Combat,
                    new PrioritySelector(
                        ctx => _chainsOfWoeIsUp = ObjectManager.GetObjectsOfType<WoWUnit>().Any(u => u.Entry == ChainsOfWoe),
                        ScriptHelpers.CreateRunAwayFromBad(ctx => _isCastingSkullCracker(_romogg) && _chainsOfWoeIsUp == false, 16, u => u == _romogg),
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 5, Quake))));
        }

        private readonly WoWPoint _impaledDrakeLoc = new WoWPoint(572.348, 899.8131, 155.3756);

        private List<WoWPoint> GetEvolvedLocations()
        {
            var evolvingZealots = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == TwilightZealot && u.HasAura("Kneeling in Supplication")).ToList();
            return evolvingZealots.Select(zealot => WoWMathHelper.CalculatePointFrom(_impaledDrakeLoc, zealot.Location, 3)).ToList();
        }

        private uint GetStacksOfEvolution(WoWUnit unit)
        {
            if (unit != null && unit.HasAura("Evolution"))
                return unit.Auras["Evolution"].StackCount;
            return 0;
        }

        private bool IsRange(WoWPlayer player)
        {
            bool? ret = StyxWoW.Me.PartyMemberInfos.Where(p => p.Guid == player.Guid).Select(p => p.IsRange()).FirstOrDefault();
            return ret.HasValue && ret.Value;
        }

        private WoWUnit _corla;

        [EncounterHandler(39679, "Corla, Herald of Twilight", Mode = CallBehaviorMode.Proximity, BossRange = 140)]
        public Composite CorlaEncounter()
        {
            var patByBossArea = new DungeonArea(
                new Vector2(567.4489f, 876.3621f),
                new Vector2(576.5095f, 876.5252f),
                new Vector2(577.4061f, 976.3552f),
                new Vector2(568.7939f, 976.7899f));

            var evolvedZealot2Path = new[]
                                         {
                                             new WoWPoint(587.9796, 859.4238, 175.5456),
                                             new WoWPoint(598.9826, 862.8481, 175.5456),
                                             new WoWPoint(598.9459, 873.7515, 173.9694),
                                             new WoWPoint(599.0847, 883.8752, 170.9533),
                                             new WoWPoint(599.0799, 894.8361, 169.562),
                                             new WoWPoint(598.9447, 905.6454, 169.562),
                                             new WoWPoint(598.8882, 920.5043, 169.5647),
                                             new WoWPoint(598.4432, 932.4044, 165.1852),
                                             new WoWPoint(598.0544, 945.2922, 160.0163),
                                             new WoWPoint(597.8989, 958.0173, 155.3387),
                                         };

            WoWUnit trashByBoss = null, pathingZealot2 = null;

            Func<bool> stayInBeamCondition =
                () => // healer and tank stay in beam if there is
                _corla != null && _corla.Combat &&
                GetStacksOfEvolution(StyxWoW.Me) < 80 && ScriptHelpers.IsBossAlive("Corla, Herald of Twilight") &&
                (!StyxWoW.Me.PartyMembers.Any(p => p.Location.DistanceSqr(StyxWoW.Me.Location) <= 3 * 3 && GetStacksOfEvolution(p) > 0));

            var pathingZealot2TankLoc = new WoWPoint(568.2605, 942.6967, 155.3322);

            List<WoWPoint> evolutionLocs = null;
            WoWPoint nearestEmptyEvolutionLoc = WoWPoint.Zero;

            return new PrioritySelector(
                ctx => _corla = ctx as WoWUnit,
                // Clear the area before starting encounter.
                new Decorator(
                    ctx => !_corla.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            // this is the 2st evolved twilight zealot on the top walkway. Since it walks down the ramp I can't use Z coord to differentiate between the other pathing zealots below. 
                            pathingZealot2 =
                                ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                                    u => u.Entry == EvolvedTwilightZealot && u.IsAlive && evolvedZealot2Path.Any(loc => u.Location.DistanceSqr(loc) <= 8 * 8));
                            // this is the trash group that paths in front of the boss.
                            return trashByBoss = ScriptHelpers.GetUnfriendlyNpcsAtArea(patByBossArea, u => u.Z < 165).FirstOrDefault();
                        },
                // pull pat when it's in position
                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => StyxWoW.Me.IsTank() && StyxWoW.Me.Z < 165 && trashByBoss != null,
                            ctx => trashByBoss.DistanceSqr <= 27 * 27,
                            () => trashByBoss, () => _jumpPointEndLoc, () => _jumpPointEndLoc, 4),
                // kill the 2nd pathing zealot 
                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => StyxWoW.Me.IsTank() && StyxWoW.Me.Z < 165 && pathingZealot2 != null,
                            ctx => pathingZealot2.Location.DistanceSqr(pathingZealot2TankLoc) <= 40 * 40,
                            () => pathingZealot2, () => pathingZealot2TankLoc, () => pathingZealot2TankLoc, 4)
                        )),
                // Handle boss encounter
                new Decorator(
                    ctx => _corla.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            evolutionLocs = GetEvolvedLocations();
                            //!ScriptHelpers.PartyIncludingMe.Any(p => p.Location.DistanceSqr(point) <= 3*3)
                            if (StyxWoW.Me.IsMelee()) // melee pick an empty evolution spot nearest to boss.
                            {
                                nearestEmptyEvolutionLoc = evolutionLocs
                                    .Where(
                                        loc =>
                                        !ScriptHelpers.PartyIncludingMe.Any(
                                            p => p.IsAlive && p.Location.DistanceSqr(loc) <= 3 * 3))
                                    .OrderBy(loc => _corla.Location.DistanceSqr(loc)).FirstOrDefault();
                            }
                            else
                            {
                                nearestEmptyEvolutionLoc = evolutionLocs
                                .Where(
                                    loc =>
                                    !ScriptHelpers.PartyIncludingMe.Any(
                                        p => p.IsAlive && p.Location.DistanceSqr(loc) <= 3 * 3))
                                .OrderByDescending(loc => _corla.Location.DistanceSqr(loc)).FirstOrDefault();
                            }
                            return ctx;
                        },
                // run way from the zealots if we have 80+ stacks of Evolution
                        ScriptHelpers.CreateRunAwayFromBad(
                            ctx => GetStacksOfEvolution(StyxWoW.Me) >= 80, 8, u => u.Entry == TwilightZealot),

                        new Decorator(
                            ctx => nearestEmptyEvolutionLoc != WoWPoint.Zero && GetStacksOfEvolution(StyxWoW.Me) == 0,
                            new PrioritySelector(
                                new Decorator(
                                    ctx => true,
                                    new Sequence(
                                        new DecoratorContinue(
                                            ctx => !ScriptHelpers.MovementEnabled,
                                            new Action(ctx => ScriptHelpers.RestoreMovement())),
                                        ScriptHelpers.CreateMoveToContinue(() => nearestEmptyEvolutionLoc),
                                        new Action(ctx => WoWMovement.ClickToMove(nearestEmptyEvolutionLoc)),
                                        new WaitContinue(3, ctx => GetStacksOfEvolution(StyxWoW.Me) > 0, new ActionAlwaysSucceed()),
                                        new Action(ctx => ScriptHelpers.DisableMovement(stayInBeamCondition))
                                        ))
                                ))
                        )));
        }

        private WoWUnit _karshSteelbender;


        [EncounterHandler(39698, "Karsh Steelbender", Mode = CallBehaviorMode.Proximity, BossRange = 121)]
        public Composite KarshSteelbenderEncounter()
        {
            WoWUnit trash = null;
            var grillCenterLoc = new WoWPoint(237.3117, 786.271, 95.6746);
            var tankLoc = new WoWPoint(248.5953, 773.5201, 95.90914);
            var trashTankLoc = new WoWPoint(293.7574, 817.3774, 103.516);

            var tankOnGrillTimer = new WaitTimer(TimeSpan.FromSeconds(1));
            var combatStartTimer = new Stopwatch();

            return new PrioritySelector(
                ctx => _karshSteelbender = ctx as WoWUnit,
                // clear the trash group before pulling group.
                new Decorator(
                    ctx => !_karshSteelbender.Combat,
                    new PrioritySelector(
                        ctx => trash = ScriptHelpers.GetUnfriendlyNpsAtLocation(() => grillCenterLoc, 40, u => u != _karshSteelbender).FirstOrDefault(),
                        ScriptHelpers.CreateDispellParty("Immolate", ScriptHelpers.PartyDispellType.Magic),
                        new Decorator(
                            ctx => combatStartTimer.IsRunning,
                            new Action(ctx => combatStartTimer.Reset())),
                        ScriptHelpers.CreatePullNpcToLocation(
                            ctx => trash != null,
                            ctx => trash.Location.DistanceSqr(trashTankLoc) <= 60 * 60 && trash.Location.DistanceSqr(_karshSteelbender.Location) > 20 * 20,
                            () => trash, () => trashTankLoc, () => trashTankLoc, 5),
                // Wait for the Heat Exhaustion debuf to wear off.
                        ScriptHelpers.CreateWaitAtLocationWhile(ctx => StyxWoW.Me.IsTank() && !StyxWoW.Me.Combat && StyxWoW.Me.HasAura("Heat Exhaustion"), () => trashTankLoc, 5)
                        )),
                new Decorator(
                    ctx => _karshSteelbender.Combat,
                    new PrioritySelector(
                        new Decorator(
                            ctx => !combatStartTimer.IsRunning,
                            new Action(ctx => combatStartTimer.Start())),
                        new Decorator(
                            ctx => StyxWoW.Me.IsTank(),
                            new PrioritySelector(
                // tank boss in the pillar of fire so he gains Superheated Quicksilver Armor
                                new Decorator(
                                    ctx =>
                                    _karshSteelbender.CurrentTarget == StyxWoW.Me &&
                                    combatStartTimer.Elapsed > TimeSpan.FromSeconds(5) &&
                                    (!_karshSteelbender.HasAura("Superheated Quicksilver Armor") ||
                                     (_karshSteelbender.HasAura("Superheated Quicksilver Armor") &&
                                     _karshSteelbender.Auras["Superheated Quicksilver Armor"].StackCount < 10 &&
                                     _karshSteelbender.Auras["Superheated Quicksilver Armor"].TimeLeft <= TimeSpan.FromSeconds(6))),
                                    new Sequence(
                                        ScriptHelpers.CreateMoveToContinue(() => grillCenterLoc),
                                        new WaitContinue(TimeSpan.FromMilliseconds(600), ctx => false, new ActionAlwaysSucceed()),
                                        ScriptHelpers.CreateMoveToContinue(() => tankLoc),
                                        new WaitContinue(TimeSpan.FromMilliseconds(200), ctx => false, new ActionAlwaysSucceed()))),

                // face away from party.
                // ScriptHelpers.CreateTankFaceAwayGroupUnit(8),
                                new Decorator(
                                    ctx => _karshSteelbender.CurrentTarget == StyxWoW.Me,
                                    ScriptHelpers.CreateTankUnitAtLocation(() => tankLoc, 3))
                                )),
                // range move off the grill.
                        new Decorator(
                            ctx => StyxWoW.Me.IsRange() && StyxWoW.Me.Location.DistanceSqr(grillCenterLoc) > 26 * 26,
                            new TreeSharp.Action(ctx => Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, grillCenterLoc, 26)))),
                // melee dps move behind the boss if it's facing melee.
                        ScriptHelpers.GetBehindUnit(ctx => StyxWoW.Me.IsMelee() && StyxWoW.Me.IsDps() && _karshSteelbender.IsFacing(StyxWoW.Me), () => _karshSteelbender)
                        ))
                );
        }

        private WoWUnit _beauty;

        [EncounterHandler(39700, "Beauty", Mode = CallBehaviorMode.Proximity, BossRange = 80)]
        public Composite BeautyEncounter()
        {
            WoWUnit pup = null;
            var tankPupsLoc = new WoWPoint(159.3506, 580.4273, 76.91465);

            var wallPoints = new[]
                                 {
                                     new WoWPoint(158.3496, 571.7232, 76.13611),
                                     new WoWPoint(158.1586, 595.2212, 79.35516),
                                     new WoWPoint(144.2739, 601.5454, 76.35615),
                                     new WoWPoint(119.3436, 554.9016, 76.438),
                                 };

            return new PrioritySelector(
                ctx => _beauty = ctx as WoWUnit,
                // pull the pups and kill them one by one.. only works on normal.
                new Decorator(
                    ctx => !_beauty.Combat,
                    new PrioritySelector(
                        ctx => pup = ScriptHelpers.GetUnfriendlyNpsAtLocation(() => _beauty.Location, 60, u => u != _beauty && u.Entry != Runty).FirstOrDefault(),
                        ScriptHelpers.CreatePullNpcToLocation(ctx => pup != null, () => pup, () => tankPupsLoc, 10))),
                // tank Beauty against a wall to reduce knockback
                new Decorator(
                    ctx => _beauty.Combat,
                    new PrioritySelector(
                        new Decorator(
                            ctx => StyxWoW.Me.IsTank() && _beauty.CurrentTarget == StyxWoW.Me,
                            // tank agains the closest wall.
                            ScriptHelpers.CreateTankUnitAtLocation(() => wallPoints.OrderBy(loc => StyxWoW.Me.Location.DistanceSqr(loc)).FirstOrDefault(), 5))
                        ))
                );
        }

        private readonly WaitTimer _teleportTimer = new WaitTimer(TimeSpan.FromSeconds(10));

        public override MoveResult MoveTo(WoWPoint location)
        {
            // use entrance portal.
            if ((StyxWoW.Me.Y > 1000 && location.Y < 800 || StyxWoW.Me.Y < 800 && location.Y > 1000) && !ScriptHelpers.IsBossAlive("Karsh Steelbender") && _teleportTimer.IsFinished)
            {
                var teleporter = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 51340);

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