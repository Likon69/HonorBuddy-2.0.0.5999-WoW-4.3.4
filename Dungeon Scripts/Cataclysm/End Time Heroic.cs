using System;
using System.Diagnostics;
using System.Linq;
using Bots.DungeonBuddy.Actions;
using CommonBehaviors.Actions;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.Gossip;
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
    public class HeroicEndTime : Dungeon
    {
        #region Overrides of Dungeon

        /// <summary>
        ///   The Map Id of this dungeon. This is the unique id for dungeons thats used to determine which dungeon, the script belongs to
        /// </summary>
        /// <value> The map identifier. </value>
        public override uint DungeonId { get { return 435; } }

        public override WoWPoint Entrance { get { return new WoWPoint(-8281.537, -4453.549, -208.3219); } }

        /// <summary>
        ///   IncludeTargetsFilter is used to add units to the targeting list. If you want to include a mob thats usually removed by the default filters, you shall use that.
        /// </summary>
        /// <param name="incomingunits"> Units passed into the method </param>
        /// <param name="outgoingunits"> Units passed to the targeting class </param>
        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (WoWObject obj in incomingunits)
            {
                var unit = obj.ToUnit();
                if (unit == null)
                    continue;

                if (unit.Entry == EchoOfBaine && StyxWoW.Me.IsHealer())
                    // make sure healer has something in target list so he heals lava damage.
                    outgoingunits.Add(unit);

                if (unit.Entry == FountainOfLight)
                    outgoingunits.Add(unit);

                if (unit.Entry == RisenGhoul)
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

                    if (_runAwayFromSylvanas)
                        return true;

                    if (unit.Entry == EchoOfBaine && !StyxWoW.Me.IsHealer() && StyxWoW.Me.Z < BaineLavaZLevel)
                        return true;

                    if (unit.HasAura("Shadow of Obsidius"))
                        return true;

                    if (unit.Entry == EchoOfJaina && !unit.Combat && _ignoreJaina)
                        return true;

                    if (unit.Entry == EchoOfSylvanas && unit.HasAura("Calling of the Highborne") && unit.Combat &&
                        !StyxWoW.Me.IsHealer())
                        return true;
                    if (_doingMoonWalk &&
                        (StyxWoW.Me.IsTank() && (!unit.Combat || unit.Combat && unit.CurrentTarget == StyxWoW.Me) ||
                         StyxWoW.Me.IsDps()))
                        return true;

                    if (!_doingMoonWalk && saberIds.Contains(unit.Entry) && !unit.Combat)
                        return true;
                    // ignore murozond if he's flying to not pull aggro on pull.
                    if (unit.Entry == Murozond && StyxWoW.Me.IsDps() && unit.StateFlag != WoWStateFlag.None)
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
                var unit = t.Object.ToUnit();
                if (unit == null)
                    continue;

                if (unit.Entry == FountainOfLight)
                    t.Score += 500;

                if (unit.Entry == RisenGhoul)
                    t.Score += 1000 + ((unit.MaxHealth - unit.CurrentHealth) * 1000) -
                               unit.Location.DistanceSqr(_ghoulLoc);

                if (unit.Entry == InfiniteWarden)
                    t.Score += 500;
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

                    );
        }

        private const uint Nozdormu = 54476;
        private const uint Alurmi = 57864;
        private const uint Murozond = 54432;
        private const uint UndyingFlame = 54550;
        private const uint FountainOfLight = 54795;
        private const uint FragmentOfJainasStaff = 209318;
        private const uint FlarecoreEmber = 54446;
        private const uint RisenGhoul = 54191;
        private const uint EchoOfJaina = 54445;
        private const uint EchoOfSylvanas = 54123;
        private const uint PoolOfMoonlight = 54508;
        private const uint EchoOfBaine = 54431;
        private const int DestroyedPlatformDisplayId = 10886;
        private const float BaineLavaZLevel = 129.95f;
        private const uint BainesTotem = 54434;
        private const int ThrowTotem = 101603;
        private const uint HourglassOfTime = 209249;
        private const uint InfiniteWarden = 54923;

        [EncounterHandler(54476, "Nozdormu", Mode = CallBehaviorMode.Proximity)]
        public Composite NozdormuEncounter()
        {
            WoWUnit nozdormu = null;
            return new PrioritySelector(
                ctx => nozdormu = ctx as WoWUnit,
                new Decorator(
                    ctx => nozdormu.QuestGiverStatus == QuestGiverStatus.Available,
                    ScriptHelpers.CreatePickupQuest(Nozdormu))
                );
        }


        [EncounterHandler(57864, "Alurmi", Mode = CallBehaviorMode.Proximity)]
        public Composite AlurmiEncounter()
        {
            WoWUnit alurmi = null;
            return new PrioritySelector(
                ctx => alurmi = ctx as WoWUnit,
                new Decorator(
                    ctx => alurmi.QuestGiverStatus == QuestGiverStatus.Available,
                    ScriptHelpers.CreatePickupQuest(Alurmi))
                );
        }

        private const uint NorthPlatform = 209693;
        private const uint EastPlatform = 209670;
        private const uint SouthPlatform = 209695;
        private const uint WestPlatform = 209694;

        private readonly uint[] _platformIds = new[]
                                               {
                                                   EastPlatform,
                                                   SouthPlatform,
                                                   NorthPlatform,
                                                   WestPlatform
                                               };

        #region Echo of Baine

        [EncounterHandler(54543, "Time-Twisted Drake")]
        public Composite TimeTwistedDrakeEncounter() { return ScriptHelpers.CreateTankFaceAwayGroupUnit(17); }

        /// <summary>
        ///   BossEntry is the Entry of the boss unit. (WoWUnit.Entry) BossName is optional. Its there just to make it easier to find which boss that composite belongs to. The context of the encounter composites is the Boss as WoWUnit
        /// </summary>
        /// <returns> </returns>
        [EncounterHandler(54431, "Echo of Baine", Mode = CallBehaviorMode.Proximity, BossRange = 210)]
        public Composite EchoOfBaineFightLogic()
        {
            WoWUnit boss = null;
            WoWGameObject[] activeIslands = null;
            WoWGameObject islandBossIsOn = null;
            WoWGameObject islandHealerIsOn = null;
            WoWGameObject currentIsland = null;
            WoWGameObject movetoIsland = null;
            WoWUnit totem = null;
            var eastIslandStandLoc = new WoWPoint(4380.827f, 1412.093f, 130.823f);
            var southIslandStandPoint = new WoWPoint(4360.898f, 1457.463f, 130.0503f);
            var northIslandStandPoint = new WoWPoint(4389.616f, 1456.567f, 130.3061f);

            var lavaCenterLoc = new WoWPoint(4378.449, 1445.719, 127.5554);

            return new PrioritySelector(
                ctx =>
                {
                    boss = ctx as WoWUnit;

                    return boss;
                },
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 4, UndyingFlame),
                // ooc behavior.
                new Decorator(
                    ctx => !boss.Combat,
                    new PrioritySelector(
                        new Decorator(
                            ctx => StyxWoW.Me.Y <= 1370 && Targeting.Instance.FirstUnit == null && StyxWoW.Me.IsTank(),
                            new Action(ctx => ScriptHelpers.MoveTankTo(boss.Location))),
                        new Decorator(
                            ctx => StyxWoW.Me.Y > 1370 && !StyxWoW.Me.Combat,
                            new PrioritySelector(
                                ctx =>
                                {
                                    if (ScriptHelpers.MovementEnabled)
                                        ScriptHelpers.DisableMovement(() => !boss.Combat);
                                    var y = StyxWoW.Me.Y;

                                    if (y < 1400)
                                        return eastIslandStandLoc;
                                    if (y > 1400 && y < 1450)
                                        return StyxWoW.Me.IsRange() && StyxWoW.Me.IsDps()
                                                   ? southIslandStandPoint
                                                   : northIslandStandPoint;
                                    if (y > 1450 && StyxWoW.Me.IsTank() &&
                                        StyxWoW.Me.PartyMembers.Count(
                                            p =>
                                            !p.HasAura("Magma") && p.Location.DistanceSqr(boss.Location) < 60 * 60 &&
                                            p.IsAlive && p.HealthPercent >= 75) == 4)
                                        return boss.Location;
                                    return ctx;
                                },
                // don't rely on getting a rez in the lava. 
                                new Decorator(
                                    ctx => StyxWoW.Me.Dead && StyxWoW.Me.Z < BaineLavaZLevel,
                                    new Action(ctx => Lua.DoString("UseSoulstone() RepopMe()"))),
                                new Decorator(
                                    ctx => StyxWoW.Me.HasAura("Magma"),
                                    new Action(ctx => Logger.Write("[Echo Of Baine] Waiting for Magma debuf to drop"))),
                                new Decorator<WoWPoint>(
                                    moveto => StyxWoW.Me.Location.DistanceSqr(moveto) > 3 * 3,
                                    new Helpers.Action<WoWPoint>(moveto => WoWMovement.ClickToMove(moveto))),
                                new Decorator(
                                    ctx =>
                                    !StyxWoW.Me.IsHealer() || StyxWoW.Me.IsHealer() && StyxWoW.Me.Z < BaineLavaZLevel,
                                    new ActionAlwaysSucceed())
                                )
                            )
                        )),
                // boss combat behavior
                new Decorator(
                    ctx => boss.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            totem =
                                ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                                    u => u.Entry == BainesTotem);
                            using (new FrameLock())
                            {
                                activeIslands =
                                    ObjectManager.GetObjectsOfType<WoWGameObject>().Where(
                                        u =>
                                        _platformIds.Contains(u.Entry) && u.DisplayId != DestroyedPlatformDisplayId)
                                        .
                                        ToArray();

                                islandBossIsOn = boss.Z >= BaineLavaZLevel
                                                     ? activeIslands.FirstOrDefault(
                                                         i => i.Location.DistanceSqr(boss.Location) <= 15 * 15)
                                                     : null;

                                var healer = ScriptHelpers.Healer;
                                islandHealerIsOn = healer != null && healer.Z >= BaineLavaZLevel
                                                       ? activeIslands.FirstOrDefault(
                                                           i => i.Location.DistanceSqr(healer.Location) <= 15 * 15)
                                                       : null;

                                currentIsland = StyxWoW.Me.Z >= BaineLavaZLevel
                                                    ? activeIslands.FirstOrDefault(
                                                        i => i.Location.DistanceSqr(StyxWoW.Me.Location) <= 15 * 15)
                                                    : null;

                                movetoIsland = null;

                                if ((StyxWoW.Me.IsMelee() && currentIsland != islandBossIsOn &&
                                     islandBossIsOn != null) ||
                                    (currentIsland == null))
                                {
                                    movetoIsland =
                                        activeIslands.Where(
                                            i =>
                                            (activeIslands.Length > 1 && i != islandHealerIsOn ||
                                             activeIslands.Length == 1) &&
                                            i.Location.DistanceSqr(boss.Location) <= 50 * 50).
                                            OrderBy(i => i.DistanceSqr).FirstOrDefault() ?? activeIslands.
                                                                                                OrderBy(i => i.DistanceSqr).FirstOrDefault();
                                }
                            }
                            return ctx;
                        },
                // island behavior 
                        new Decorator(
                            ctx => movetoIsland != null && movetoIsland != currentIsland,
                            new Action(
                                ctx =>
                                {
                                    WoWMovement.ClickToMove(movetoIsland.Location);
                                    if (ScriptHelpers.MovementEnabled)
                                        ScriptHelpers.DisableMovement(() => StyxWoW.Me.Z < BaineLavaZLevel);
                                    return RunStatus.Failure;
                                })
                            ),
                // if boss is in lava then pull him to the center
                        new Decorator(
                            ctx =>
                            currentIsland != null && boss.Z < BaineLavaZLevel && ((StyxWoW.Me.IsTank() &&
                                                                                   boss.CurrentTarget == StyxWoW.Me) ||
                                                                                  StyxWoW.Me.IsDps() &&
                                                                                  StyxWoW.Me.IsMelee()),
                            new Sequence(
                                new Action(ctx => WoWMovement.ClickToMove(currentIsland.Location)),
                                new ActionSetActivity(
                                    "[Echo Of Baine] Moving to center of island while boss is in lava.")))
                // move boss to edge of island for ranged.
                /*
new Decorator(
    ctx => currentIsland != null && StyxWoW.Me.IsTank() && boss.CurrentTarget == StyxWoW.Me,
    new Action(
        ctx =>
            {
                var tankLoc = WoWMathHelper.CalculatePointFrom(
                    lavaCenterLoc, currentIsland.Location, 7);
                if (StyxWoW.Me.Location.DistanceSqr(tankLoc) > 3*3)
                {
                    WoWMovement.ClickToMove(tankLoc);
                    Logger.Write("[Echo Of Baine] Moving to edge of island for ranged.");
                    TreeRoot.StatusText = "[Echo Of Baine] Moving to edge of island for ranged.";
                    return RunStatus.Success;
                }
                return RunStatus.Failure;
            })) */,
                        ScriptHelpers.CreateDispellParty("Molten Blast", ScriptHelpers.PartyDispellType.Magic),
                        new Decorator(
                            ctx => StyxWoW.Me.HasPendingSpell("Throw Totem"),
                            new Sequence(
                                new Action(ctx => LegacySpellManager.ClickRemoteLocation(boss.Location)),
                                new Action(ctx => Logger.Write("[Echo Of Baine] Throwing totem on boss"))
                                )),
                // pickup totem

                        new Decorator(
                            ctx =>
                            totem != null &&
                            ScriptHelpers.PartyIncludingMe.OrderBy(p => p.Location.DistanceSqr(totem.Location)).
                                FirstOrDefault().IsMe,
                            new Sequence(
                                new Action(ctx => Logger.Write("[Echo Of Baine] Picking up totem")),
                                new Action(ctx => totem.Interact()))),
                        new Decorator(
                            ctx =>
                            StyxWoW.Me.IsMelee() && StyxWoW.Me.IsDps() && !StyxWoW.Me.HasAura("Molten Fists") &&
                            currentIsland != null,
                            new Sequence(
                                new Action(ctx => Logger.Write("[Echo Of Baine] Getting Molten Fists")),
                                new Action(
                                    ctx =>
                                    WoWMovement.ClickToMove(
                                        WoWMathHelper.CalculatePointFrom(
                                            StyxWoW.Me.Location, currentIsland.Location, 15)))))
                        )));
        }

        #endregion

        #region Echo of Jaina

        [ObjectHandler(209318, "Fragment of Jaina's Staff", ObjectRange = 250)]
        public Composite FragmentofJainasStaffHandler()
        {
            WoWGameObject fragment = null;
            return new PrioritySelector(
                ctx => fragment = ctx as WoWGameObject,
                new Decorator(
                    ctx => fragment.DistanceSqr <= 25 * 25 && !StyxWoW.Me.Combat,
                    ScriptHelpers.CreateInteractWithObject(() => fragment, 2, false)),
                new Action(ctx => ScriptHelpers.MoveTankTo(fragment.Location))
                );
        }

        private bool _ignoreJaina;

        [EncounterHandler(54445, "Echo of Jaina", Mode = CallBehaviorMode.Proximity, BossRange = 200)]
        public Composite EchoofJainaEncounter()
        {
            WoWUnit boss = null;
            WoWUnit flamecore = null;
            bool handleFlameCore = false;
            const int frostboltVolley = 101810;
            WoWUnit trash = null;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                new Decorator(
                    ctx => !boss.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            trash =
                                ScriptHelpers.GetUnfriendlyNpsAtLocation(() => boss.Location, 150, u => u != boss).
                                    FirstOrDefault();
                            _ignoreJaina = trash != null;
                            return ctx;
                        },
                        ScriptHelpers.CreateRunAwayFromBad(ctx => trash != null, 20, u => u == boss),
                        new Decorator(
                            ctx => trash != null,
                            ScriptHelpers.CreateClearArea(() => boss.Location, 150, u => u != boss)),
                        new Decorator(
                            ctx => trash == null,
                            new Action(ctx => ScriptHelpers.MoveTankTo(boss.Location)))
                        )),
                // boss behavior
                new Decorator(
                    ctx => boss.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            flamecore =
                                ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                                    u => u.Entry == FlarecoreEmber && u.HasAura("Flarecore"));

                            if (flamecore != null && StyxWoW.Me.IsDps())
                            {
                                var rangedDps =
                                    ScriptHelpers.GetPartyMembersByRole(
                                        ScriptHelpers.PartyRole.Dps |
                                        ScriptHelpers.PartyRole.Ranged).Where(p => p.IsAlive).OrderBy(
                                            p => p.MaxHealth).FirstOrDefault();

                                var meleeDps =
                                    ScriptHelpers.GetPartyMembersByRole(
                                        ScriptHelpers.PartyRole.Dps |
                                        ScriptHelpers.PartyRole.Ranged).Where(p => p.IsAlive).OrderBy(
                                            p => p.MaxHealth).FirstOrDefault();
                                if (rangedDps != null && rangedDps.IsMe)
                                    handleFlameCore = true;
                                else if (rangedDps == null && meleeDps != null && meleeDps.IsMe)
                                    handleFlameCore = true;
                                else handleFlameCore = false;
                            }

                            return ctx;
                        },
                        new Decorator(
                            ctx => handleFlameCore,
                            new Sequence(
                                new Action(ctx => Logger.Write("Moving to Flamecore")),
                                new Action(
                                    ctx => Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(flamecore.Location))))),
                        ScriptHelpers.CreateInterruptCast(() => boss, frostboltVolley)
                        )));
        }

        #endregion

        #region Echo of Sylvanas

        private readonly WoWPoint _sylvanasLoc = new WoWPoint(3840.03, 914.043, 56.0547);
        private bool _runAwayFromSylvanas;
        private readonly WoWPoint _ghoulLoc = new WoWPoint(3823.271, 935.1199, 55.81496);

        [EncounterHandler(54123, "Echo of Sylvanas", Mode = CallBehaviorMode.Proximity, BossRange = 250)]
        public Composite EchoofSylvanasEncounter()
        {
            WoWUnit boss = null;
            WoWUnit trash = null;
            const uint blightedArrows = 54403;
            var lastGhoulLoc = WoWPoint.Zero;
            var runToLoc = WoWPoint.Zero;
            var rangeNukeSpot = WoWPoint.Zero;

            return new PrioritySelector(
                ctx => boss = ctx as WoWUnit,
                new Decorator(
                    ctx => !boss.Combat,
                    new PrioritySelector(
                        ctx => trash =
                               ScriptHelpers.GetUnfriendlyNpsAtLocation(() => boss.Location, 70, u => u != boss).
                                   FirstOrDefault(),
                        new Decorator(
                            ctx => trash != null,
                            ScriptHelpers.CreateClearArea(() => boss.Location, 70, u => u != boss)),
                        new Decorator(
                            ctx =>
                            StyxWoW.Me.IsTank() && boss.DistanceSqr <= 40 * 40 &&
                            StyxWoW.Me.PartyMembers.Count(p => p.IsAlive && p.DistanceSqr <= 40 * 40) != 4,
                            new PrioritySelector(
                                new Decorator(
                                    ctx => StyxWoW.Me.IsMoving,
                                    new Action(ctx => WoWMovement.MoveStop())),
                                new ActionAlwaysSucceed())),
                        new Decorator(
                            ctx => trash == null,
                            new Action(ctx => ScriptHelpers.MoveTankTo(boss.Location)))
                        )),
                // boss behavior
                new Decorator(
                    ctx => boss.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            var ghouls =
                                ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                                    u => u.Entry == RisenGhoul && u.IsAlive).ToList();
                            _runAwayFromSylvanas = ghouls.Count > 0 && ghouls.Count < 8 ||
                                                   ghouls.Any(g => g.Location.Distance2DSqr(boss.Location) <= 7 * 7);

                            if (Targeting.Instance.FirstUnit != null &&
                                Targeting.Instance.FirstUnit.Entry == RisenGhoul && !_runAwayFromSylvanas)
                            {
                                lastGhoulLoc = Targeting.Instance.FirstUnit.Location;
                                rangeNukeSpot = StyxWoW.Me.Class == WoWClass.Hunter
                                                    ? _sylvanasLoc
                                                    : WoWMathHelper.CalculatePointFrom(
                                                        lastGhoulLoc, _sylvanasLoc, 5);
                            }
                            else
                            {
                                rangeNukeSpot = _sylvanasLoc;
                            }

                            if (_runAwayFromSylvanas)
                                runToLoc = WoWMathHelper.CalculatePointFrom(lastGhoulLoc, boss.Location, 16);
                            return ctx;
                        },
                        new Decorator(
                            ctx => _runAwayFromSylvanas,
                            new PrioritySelector(
                                new Decorator(
                                    ctx => StyxWoW.Me.Location.DistanceSqr(runToLoc) > 3 * 3,
                                    new Action(
                                        ctx =>
                                        {
                                            if (StyxWoW.Me.MovementInfo.MovingBackward)
                                                WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards);

                                            if (!ScriptHelpers.MovementEnabled)
                                                ScriptHelpers.RestoreMovement();
                                            Logger.Write("[Echo Of Sylvanas] Running from Slyvanas");

                                            return Navigator.GetRunStatusFromMoveResult(Navigator.MoveTo(runToLoc));
                                        })),
                                new ActionAlwaysSucceed()
                                )),
                        ScriptHelpers.CreateRunAwayFromBad(ctx => true, 6, blightedArrows),
                        ScriptHelpers.CreateSpreadOutLogic(ctx => !boss.HasAura("Jump"), () => boss.Location, 11, 30),
                        new Decorator(
                            ctx => boss.HasAura("Jump"),
                            new PrioritySelector(
                // ranged behavior for ghouls.
                                new Decorator(
                                    ctx => StyxWoW.Me.IsRange(),
                                    new Action(
                                        ctx =>
                                        {
                                            if (StyxWoW.Me.Location.DistanceSqr(rangeNukeSpot) > 3 * 3)
                                                WoWMovement.ClickToMove(rangeNukeSpot);
                                            return RunStatus.Failure;
                                        })),
                // melee behavior for ghouls
                                new Decorator(
                                    ctx =>
                                    StyxWoW.Me.IsMelee() && Targeting.Instance.FirstUnit != null &&
                                    Targeting.Instance.FirstUnit.Entry == RisenGhoul,
                                    new PrioritySelector(
                // manually move to ghoul if a warrior to prevent charge as it sometimes lands behind and causing warrior to die.
                                        new Decorator(
                                            ctx =>
                                            StyxWoW.Me.Class == WoWClass.Warrior &&
                                            Targeting.Instance.FirstUnit.DistanceSqr > 8 * 8,
                                            new Action(ctx => Navigator.MoveTo(Targeting.Instance.FirstUnit.Location))),
                                        new Action(
                                            ctx =>
                                            {
                                                if (ScriptHelpers.MovementEnabled)
                                                    ScriptHelpers.DisableMovement(
                                                        () => boss != null && boss.HasAura("Jump"));
                                                BackpedalUnitToLocation(_sylvanasLoc, Targeting.Instance.FirstUnit);
                                                return RunStatus.Failure;
                                            })))
                                )),
                        ScriptHelpers.CreateDispellParty(
                            "Shriek of the Highborne", ScriptHelpers.PartyDispellType.Magic),
                // place raidmarker
                        new Action(
                            ctx =>
                            {
                                using (new FrameLock())
                                {
                                    var placedMarker =
                                        ScriptHelpers.GetReturnVal<bool>(
                                            "if IsRaidMarkerActive(1) == false then PlaceRaidMarker(1) return 1 end return nil",
                                            0);
                                    if (placedMarker)
                                    {
                                        var point = WoWMathHelper.GetPointAt(
                                            _ghoulLoc,
                                            (float)ScriptHelpers.Rnd.NextDouble() * 4f,
                                            (float)ScriptHelpers.Rnd.NextDouble() *
                                            (float)(Math.PI * 2),
                                            0);
                                        LegacySpellManager.ClickRemoteLocation(point);
                                    }
                                }
                                return RunStatus.Failure;
                            }),
                        new Decorator(
                            ctx => Targeting.Instance.FirstUnit == null,
                            new ActionAlwaysSucceed())
                        )
                    ));
        }


        private void BackpedalUnitToLocation(WoWPoint destination, WoWUnit unit)
        {
            using (new FrameLock())
            {
                var unitLoc = unit.Location;
                var myLoc = StyxWoW.Me.Location;

                var unitToDestinationDistance = unitLoc.Distance(destination);
                var meToDestinationDistance = myLoc.Distance(destination);

                var maxMeleeRange = unit.MeleeRange();
                //float minMeleeRange = maxMeleeRange;
                var me2Unit = myLoc - unit.Location;
                var unit2End = unit.Location - destination;
                me2Unit.Normalize();
                unit2End.Normalize();

                if (!WoWMovement.IsFacing)
                    WoWMovement.ConstantFace(unit.Guid);

                if (unit.Distance > maxMeleeRange || meToDestinationDistance >= unitToDestinationDistance)
                {
                    if (StyxWoW.Me.MovementInfo.MovingBackward)
                        Lua.DoString("MoveBackwardStop()");
                    if (StyxWoW.Me.MovementInfo.MovingStrafeLeft)
                        Lua.DoString("StrafeLeftStop()");
                    if (StyxWoW.Me.MovementInfo.MovingStrafeRight)
                        Lua.DoString("StrafeRightStop()");

                    if (!StyxWoW.Me.MovementInfo.MovingForward)
                        Lua.DoString("MoveForwardStart()");
                    return;
                }
                if (StyxWoW.Me.MovementInfo.MovingForward)
                    Lua.DoString("MoveForwardStop()");

                if (myLoc.DistanceSqr(unitLoc) <= maxMeleeRange * maxMeleeRange && !StyxWoW.Me.MovementInfo.MovingBackward)
                    Lua.DoString("MoveBackwardStart()");
                else if (StyxWoW.Me.MovementInfo.MovingBackward)
                    Lua.DoString("MoveBackwardStop()");


                var dot = Math.Abs(me2Unit.Dot(unit2End));
                if (dot < 0.9f)
                {
                    var isLeft = myLoc.IsPointLeftOfLine(unit.Location, destination);

                    if (!StyxWoW.Me.MovementInfo.MovingStrafeLeft && isLeft)
                        Lua.DoString("StrafeLeftStart()");

                    if (!StyxWoW.Me.MovementInfo.MovingStrafeRight && !isLeft)
                        Lua.DoString("StrafeRightStart()");
                }
                else if (StyxWoW.Me.MovementInfo.MovingStrafeLeft)
                    Lua.DoString("StrafeLeftStop()");
                else if (StyxWoW.Me.MovementInfo.MovingStrafeRight)
                    Lua.DoString("StrafeRightStop()");
            }
        }

        #endregion

        #region Echo of Tyrande

        private bool _doingMoonWalk;
        private WoWUnit _tyrande;

        private readonly uint[] saberIds = new uint[]
                                           {
                                               54688, 54699, 54700
                                           };

        [EncounterHandler(54544, "Echo of Tyrande", Mode = CallBehaviorMode.Proximity, BossRange = 250)]
        public Composite EchoofTyrandeEncounter()
        {
            var movePoint = WoWPoint.Zero;

            const int stardust = 102173;
            const int darkMoonlight = 102414;
            const int moonlance = 102151;

            const uint moonLanceRoot = 54574;
            var moonLanceIds = new uint[] { 54580, 54581, 54582 };
            var eyeOfEluneIds = new uint[]
                                {
                                    // 54939,// range 28
                                    //  54940, // range 28
                                    //  54941, // range 35
                                    //54942, // range 35
                                    54594, // range 20
                                    54597
                                } // range 20
                ;
            WoWUnit poolOfMoonlight = null;
            bool bossIsActive = false;

            return new PrioritySelector(
                ctx => _tyrande = ctx as WoWUnit,
                // out of combat behavior
                new Decorator(
                    ctx => !_tyrande.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            bossIsActive = !_tyrande.HasAura("In Shadow");
                            if (!bossIsActive)
                            {
                                poolOfMoonlight =
                                    ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                                        u => u.Entry == PoolOfMoonlight);

                                var tank = ScriptHelpers.Tank;
                                _doingMoonWalk = poolOfMoonlight != null && tank != null &&
                                                 tank.Location.DistanceSqr(poolOfMoonlight.Location) > 5 * 5;
                            }
                            return ctx;
                        },
                        new Decorator(
                            ctx => !bossIsActive && poolOfMoonlight != null,
                            new Action(ctx => ScriptHelpers.MoveTankTo(poolOfMoonlight.Location))),
                        new Decorator(
                            ctx => poolOfMoonlight == null,
                            new Action(ctx => ScriptHelpers.MoveTankTo(_tyrande.Location))))),
                // in combat behavior
                new Decorator(
                    ctx => _tyrande.Combat,
                    new PrioritySelector(
                        ctx =>
                        {
                            movePoint = StyxWoW.Me.Location;
                            if (
                                ObjectManager.GetObjectsOfType<WoWUnit>().Any(
                                    u =>
                                    eyeOfEluneIds.Contains(u.Entry) && u.DistanceSqr <= 10 * 10 &&
                                    u.Location.DistanceSqr(_tyrande.Location) > 18 * 18))
                                movePoint = WoWMathHelper.CalculatePointFrom(
                                    movePoint, _tyrande.Location, 11);

                            var lance =
                                ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                                    u => u.Entry == moonLanceRoot);

                            if (lance != null)
                            {
                                var targetLoc = lance.Location;
                                var moonlanceDirection =
                                    WoWMathHelper.CalculateNeededFacing(
                                        _tyrande.Location, targetLoc);
                                if (WoWMathHelper.IsFacing(
                                    targetLoc,
                                    moonlanceDirection,
                                    StyxWoW.Me.Location,
                                    WoWMathHelper.DegreesToRadians(35)))
                                {
                                    float newAngle;
                                    if (StyxWoW.Me.Location.IsPointLeftOfLine(
                                        _tyrande.Location, targetLoc))
                                    {
                                        newAngle =
                                            WoWMathHelper.NormalizeRadian(
                                                moonlanceDirection +
                                                WoWMathHelper.DegreesToRadians(35));
                                    }
                                    else
                                    {
                                        newAngle =
                                            WoWMathHelper.NormalizeRadian(
                                                moonlanceDirection -
                                                WoWMathHelper.DegreesToRadians(35));
                                    }
                                    movePoint = _tyrande.Location.RayCast(
                                        newAngle, movePoint.Distance(_tyrande.Location));
                                }
                            }
                            return ctx;
                        },
                //ScriptHelpers.CreateRunAwayFromBad(ctx => true, () => _tyrande.Location, 20, 6, moonLanceIds),
                // ScriptHelpers.CreateRunAwayFromBad(ctx => true, 18, eyeOfEluneIds),
                        new Decorator(
                            ctx => StyxWoW.Me.Location.DistanceSqr(movePoint) > 3 * 3,
                            new Action(ctx => Navigator.PlayerMover.MoveTowards(movePoint))),
                // move to the outer edge for the Dark Moonlight aura.
                        new Decorator(
                            ctx =>
                            StyxWoW.Me.IsRange() && StyxWoW.Me.PowerType == WoWPowerType.Mana &&
                            _tyrande.Distance2DSqr <= 15 * 15,
                            new Action(
                                ctx =>
                                Navigator.PlayerMover.MoveTowards(
                                    WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, _tyrande.Location, 16)))),
                        ScriptHelpers.CreateSpreadOutLogic(ctx => true, () => _tyrande.Location, 8, 20),
                        ScriptHelpers.CreateInterruptCast(() => _tyrande, stardust)
                        ))
                );
        }

        #endregion


        bool _activateHourGlass;

        [EncounterHandler(54432, "Murozond", Mode = CallBehaviorMode.CurrentBoss, BossRange = 250)]
        public Composite MurozondFightLogic()
        {
            const int distortionBomb = 101984;

            var murozondSpawnTimer = new Stopwatch();
            WoWUnit murozond = null;
            WoWGameObject hourglassOfTime = null;

            return new Decorator(
                ctx => _murozondArea.IsPointInPoly(StyxWoW.Me.Location),
                new PrioritySelector(
                    ctx => murozond = ctx as WoWUnit,

                                    new Action(ctx =>
                                    {
                                        var s = ScriptHelpers.GetPartyMembersByRole(
                                             ScriptHelpers.PartyRole.Dps |
                                             ScriptHelpers.PartyRole.Ranged).Where(p => p.IsAlive).OrderBy(
                                                 p => p.MaxHealth).ToList();
                                        return RunStatus.Failure;
                                    }),

                // wait for Murozond to spawn after a wipe.
                    new Decorator(
                        ctx => murozond == null && StyxWoW.Me.IsTank() &&  !ScriptHelpers.GetUnfriendlyNpsAtLocation(() => StyxWoW.Me.Location, 200).Any(),
                        new PrioritySelector(
                            new Decorator(
                                ctx => !murozondSpawnTimer.IsRunning,
                                new Action(ctx => murozondSpawnTimer.Start())),
                            new Decorator(
                                ctx => murozondSpawnTimer.Elapsed < TimeSpan.FromMinutes(2),
                                new ActionSetActivity("Waiting for Murozond to spawn")),
                            new Decorator(
                                ctx => murozondSpawnTimer.Elapsed >= TimeSpan.FromMinutes(2),
                                new Action(ctx =>
                                               {
                                                   Logger.Write("Murozond is dead, dungeon over.");
                                                   return RunStatus.Failure;
                                               }))
                            )),

                        ScriptHelpers.CreatePullNpcToLocation(ctx => murozond != null && !murozond.Combat && StyxWoW.Me.IsTank() && murozond.Distance2DSqr <= 30 * 30, () => murozond, null, 0),

                // combat logic for Murozond.
                    new Decorator(
                        ctx => murozond != null && murozond.Combat,
                        new PrioritySelector(
                            ctx =>
                            {
                                var hourglassUsesLeft = ScriptHelpers.GetReturnVal<int>(" return UnitPower('player', ALTERNATE_POWER_INDEX)", 0);

                                _activateHourGlass = false;
                                if (hourglassUsesLeft > 0)
                                {
                                    hourglassOfTime =
                                        ObjectManager.GetObjectsOfType<WoWGameObject>().
                                            FirstOrDefault(g => g.Entry == HourglassOfTime);

                                    // person that clicks the activator should be a ranged dps.
                                    var asignedHourglassActivator =
                                        ScriptHelpers.GetPartyMembersByRole(
                                            ScriptHelpers.PartyRole.Dps |
                                            ScriptHelpers.PartyRole.Ranged).Where(p => p.IsAlive).OrderBy(
                                                p => p.MaxHealth).FirstOrDefault()
                                        ?? // no ranged dps ? try healer
                                        ScriptHelpers.GetPartyMembersByRole(ScriptHelpers.PartyRole.Healer).FirstOrDefault(u => !u.HasAura(27827)) ??
                                        // if no ranged are alive then have a melee dps go click it.
                                        ScriptHelpers.GetPartyMembersByRole(
                                            ScriptHelpers.PartyRole.Dps |
                                            ScriptHelpers.PartyRole.Melee).Where(p => p.IsAlive).OrderBy(
                                                p => p.MaxHealth).FirstOrDefault();

                                    if (asignedHourglassActivator != null && asignedHourglassActivator.IsMe)
                                    {
                                        if (StyxWoW.Me.PartyMembers.Any(p => p.Dead))
                                        {
                                            Logger.Write("we have a dead party member. interacting with hourglass");
                                            _activateHourGlass = true;
                                        }
                                        if (murozond.HealthPercent / 16.66f < hourglassUsesLeft)
                                            _activateHourGlass = true;
                                    }
                                }
                                return ctx;
                            },
                                new Decorator(ctx => murozondSpawnTimer.IsRunning,
                                    new Action(ctx => murozondSpawnTimer.Reset())),

                                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 9, distortionBomb),
                // stay away from the hourglass unless you're clicking it.
                                ScriptHelpers.CreateRunAwayFromBad(ctx => _murozondArea.IsPointInPoly(StyxWoW.Me.Location) && !_activateHourGlass, () => murozond != null && murozond.Combat ? 30 : 21, HourglassOfTime),
                                new Decorator(ctx => _activateHourGlass,
                                    new PrioritySelector(
                                        new ActionLogger("Interacting with Hourglass"),
                                        new Decorator(ctx => hourglassOfTime.DistanceSqr > 20 * 20,
                                            new Action(ctx => Navigator.MoveTo(hourglassOfTime.Location))),

                                        new Decorator(ctx => hourglassOfTime.DistanceSqr <= 20 * 20,
                                            new Action(ctx => hourglassOfTime.Interact())))) ,
                                            /*
                                        new Decorator(ctx => !StyxWoW.Me.IsTank() && !_activateHourGlass,
                                            new Action(
                                                ctx =>
                                                {
                                                    var myLoc = StyxWoW.Me.Location;

                                                    var bossLoc = murozond.Location;
                                                    var bossRotation = murozond.Rotation;
                                                    // check if I'm behind boss
                                                    bool isBehind = WoWMathHelper.IsBehind(
                                                        myLoc, bossLoc, bossRotation, WoWMathHelper.DegreesToRadians(60));

                                                    bool isInFront = WoWMathHelper.IsFacing(
                                                        bossLoc, bossRotation, myLoc, WoWMathHelper.DegreesToRadians(60));

                                                    if (isBehind || isInFront)
                                                    {
                                                        bool isRightOfBoss = !myLoc.IsPointLeftOfLine(bossLoc, bossLoc.RayCast(murozond.Rotation, 10));

                                                        WoWPoint moveToSegmentStart = WoWMathHelper.CalculatePointAtSide(
                                                            bossLoc,
                                                            murozond.Rotation,
                                                            StyxWoW.Me.IsRange() ? 30 : 3.5f, isRightOfBoss);

                                                        WoWPoint moveToSegmentEnd = WoWMathHelper.CalculatePointAtSide(
                                                            bossLoc,
                                                            murozond.Rotation,
                                                            StyxWoW.Me.IsRange()
                                                                ? murozond.MeleeRange() + 30
                                                                : murozond.MeleeRange(), isRightOfBoss);

                                                        var moveto = myLoc.GetNearestPointOnSegment(moveToSegmentStart, moveToSegmentEnd);

                                                        if (StyxWoW.Me.Location.DistanceSqr(moveto) >
                                                            Navigator.PathPrecision * Navigator.PathPrecision)
                                                        {
                                                            return
                                                                Navigator.GetRunStatusFromMoveResult(
                                                                    Navigator.MoveTo(moveto));
                                                        }
                                                    }
                                                    return RunStatus.Failure;
                                                }))*/
                                            new Decorator(ctx => SpellManager.CanCast("Time Warp"),
                                                new Action(ctx => SpellManager.Cast("Time Warp"))),

                                            new Decorator(ctx => SpellManager.CanCast("Heroism"),
                                                new Action(ctx => SpellManager.Cast("Heroism"))),

                                            new Decorator(ctx => SpellManager.CanCast("Bloodlust"),
                                                new Action(ctx => SpellManager.Cast("Bloodlust")))
                                // ScriptHelpers.CreateTankFaceAwayGroupUnit(20)
                            ))
                    ));
        }

        private const uint TimeTransitDevice = 209441;

        private readonly uint[] _timeTransitDevices = new uint[]
                                                      {
                                                          209438, // Tyrande
                                                          209439, // Baine
                                                          209440, // Murozond
                                                          209441,
                                                          209442,
                                                          209443, // sylvanas
                                                      };

        private readonly WaitTimer _useTransitTimer = new WaitTimer(TimeSpan.FromSeconds(15));

        private readonly DungeonArea _startArea = new DungeonArea(
            new Vector2(3687.522f, -367.1982f),
            new Vector2(3672.32f, -399.8057f),
            new Vector2(3708.545f, -418.6846f),
            new Vector2(3750.661f, -416.4578f),
            new Vector2(3781.039f, -402.2898f),
            new Vector2(3782.589f, -388.5927f),
            new Vector2(3768.944f, -371.1147f),
            new Vector2(3782.589f, -388.5927f),
            new Vector2(3716.445f, -356.5307f));

        private readonly DungeonArea _jainaArea = new DungeonArea(
            new Vector2(3268.74f, 345.4715f),
            new Vector2(2959.062f, 294.5733f),
            new Vector2(2946.141f, 684.3937f),
            new Vector2(3242.467f, 707.7469f));

        private readonly DungeonArea _sylvanasArea = new DungeonArea(
            new Vector2(4000.151f, 1285.679f),
            new Vector2(4029.977f, 561.3909f),
            new Vector2(3565.349f, 577.9777f),
            new Vector2(3569.989f, 1269.092f));

        private readonly DungeonArea _baineArea = new DungeonArea(
            new Vector2(4518.893f, 1207.923f),
            new Vector2(4286.12f, 1216.014f),
            new Vector2(4296.304f, 1588.682f),
            new Vector2(4505.072f, 1616.999f));

        private readonly DungeonArea _tyrandeArea = new DungeonArea(
            new Vector2(3009.238f, -246.5252f),
            new Vector2(2623.441f, -227.9786f),
            new Vector2(2658.125f, 298.742f),
            new Vector2(2993.23f, 312.0955f));

        private readonly DungeonArea _murozondArea = new DungeonArea(
            new Vector2(4261.447f, -589.5588f),
            new Vector2(4004.345f, -591.4884f),
            new Vector2(4010.591f, -248.5168f),
            new Vector2(4234.73f, -233.563f));

        private readonly WoWPoint _startPortalLoc = new WoWPoint(3697.036, -367.9028, 113.7821);
        private readonly WoWPoint _jainaPortalLoc = new WoWPoint(2991.796, 560.4416, 25.28839);
        private readonly WoWPoint _sylvanasPortalLoc = new WoWPoint(3829.036, 1110.17, 84.14674);
        private readonly WoWPoint _bainePortalLoc = new WoWPoint(4349.405, 1285.794, 147.6583);
        private readonly WoWPoint _tyrandePortalLoc = new WoWPoint(2937.735, 83.60256, 6.670205);
        private readonly WoWPoint _murozondPortalLoc = new WoWPoint(4035.225, -353.193, 122.541);

        public override MoveResult MoveTo(WoWPoint location)
        {
            if (!_useTransitTimer.IsFinished && StyxWoW.Me.IsTank())
                return MoveResult.Moved;
            var myLoc = StyxWoW.Me.Location;

            var myArea = GetAreaForLocation(myLoc);
            var endArea = GetAreaForLocation(location);

            if (myArea == null && StyxWoW.Me.IsAlive)
            {
                Logger.Write("[End Time] I don't know where I am... zoning outside");
                Lua.DoString("LFGTeleport(true)");
            }

            if (myArea != endArea && myArea != null && endArea != null)
            {
                if (endArea == _startArea || myArea == _baineArea) // no going through lava to get to portal...
                {
                    // port out - bot will auto port back in. this is the only way to get to 1st area.
                    Lua.DoString("LFGTeleport(true)");
                }

                var portalLoc = GetPortalLocationForArea(myArea);
                var portal =
                    ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                        g => _timeTransitDevices.Contains(g.Entry) && g.Location.Distance2DSqr(portalLoc) < 100 * 100);

                if (portal == null)
                    return Navigator.MoveTo(portalLoc);

                if (portal.DistanceSqr > 5 * 5)
                    return Navigator.MoveTo(portal.Location);

                if (!GossipFrame.Instance.IsVisible)
                    portal.Interact();
                else if (GossipFrame.Instance.GossipOptionEntries != null)
                {
                    GossipFrame.Instance.SelectGossipOption(GossipFrame.Instance.GossipOptionEntries.Count - 1);
                    _useTransitTimer.Reset();
                }

                return MoveResult.Moved;
            }
            return MoveResult.Failed;
        }

        private DungeonArea GetAreaForLocation(WoWPoint location)
        {
            if (_startArea.IsPointInPoly(location))
                return _startArea;

            if (_jainaArea.IsPointInPoly(location))
                return _jainaArea;

            if (_sylvanasArea.IsPointInPoly(location))
                return _sylvanasArea;

            if (_baineArea.IsPointInPoly(location))
                return _baineArea;

            if (_tyrandeArea.IsPointInPoly(location))
                return _tyrandeArea;

            if (_murozondArea.IsPointInPoly(location))
                return _murozondArea;

            return null;
        }

        private WoWPoint GetPortalLocationForArea(DungeonArea area)
        {
            if (area == _startArea)
                return _startPortalLoc;

            if (area == _jainaArea)
                return _jainaPortalLoc;

            if (area == _sylvanasArea)
                return _sylvanasPortalLoc;

            if (area == _baineArea)
                return _bainePortalLoc;

            if (area == _tyrandeArea)
                return _tyrandePortalLoc;

            if (area == _murozondArea)
                return _murozondPortalLoc;

            return WoWPoint.Zero;
        }

        #endregion
    }
}