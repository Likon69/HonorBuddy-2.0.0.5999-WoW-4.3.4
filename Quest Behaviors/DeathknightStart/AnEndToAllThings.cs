// Behavior originally contributed by Nesox.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Bots.Grind;

using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;

using Tripper.Tools.Math;


namespace Styx.Bot.Quest_Behaviors.DeathknightStart
{
    /// <summary>
    /// Moves along a path in a vehicle using spells to inflict damage and to heals itself until the quest is completed.
    /// ##Syntax##
    /// VehicleId: Id of the vehicle
    /// ItemId: Id of the item that summons the vehicle.
    /// AttackSpell: Id of the attackspell, can be enumerated using, 'GetPetActionInfo(index)'
    /// HealSpell: Id of the healspell, can be enumerated using, 'GetPetActionInfo(index)'
    /// NpcIds: a comma separated list with id's of npc's to kill for this quest. example. NpcIds="143,2,643,1337" 
    /// </summary>
    public class AnEndToAllThings : CustomForcedBehavior
    {
        public AnEndToAllThings(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                AttackSpellId = GetAttributeAsNullable<int>("AttackSpellId", true, ConstrainAs.SpellId, new[] { "AttackSpell" }) ?? 0;
                HealNpcId = GetAttributeAsNullable<int>("HealNpcId", true, ConstrainAs.MobId, new[] { "HealNpc" }) ?? 0;
                HealSpellId = GetAttributeAsNullable<int>("HealSpellId", false, ConstrainAs.SpellId, new[] { "HealSpell" }) ?? 0;
                ItemId = GetAttributeAsNullable<int>("ItemId", false, ConstrainAs.ItemId, null) ?? 0;
                KillNpcId = GetAttributeAsNullable<int>("KillNpcId", true, ConstrainAs.MobId, new[] { "KillNpc" }) ?? 0;
                VehicleId = GetAttributeAsNullable<int>("VehicleId", true, ConstrainAs.VehicleId, null) ?? 0;
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }

        // Attributes provided by caller
        public int AttackSpellId { get; private set; }
        public int HealNpcId { get; private set; }
        public int HealSpellId { get; private set; }
        public int ItemId { get; private set; }
        public int KillNpcId { get; private set; }
        public int VehicleId { get; private set; }

        // Private variables for internal state
        private ConfigMemento _configMemento;
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private bool _isInitialized;
        private PlayerQuest _quest;
        private readonly Stopwatch _remountTimer = new Stopwatch();

        // Private properties
        public WoWPetSpell AttackSpell { get { return Me.PetSpells.FirstOrDefault(s => s.Spell != null && s.Spell.Id == AttackSpellId); } }
        public IEnumerable<WoWUnit> HealNpcs
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                       .Where(ret => ret.HealthPercent > 1 && ret.Entry == HealNpcId);
            }
        }
        public WoWPetSpell HealSpell { get { return Me.PetSpells.FirstOrDefault(s => s.Spell != null && s.Spell.Id == HealSpellId); } }
        private CircularQueue<WoWPoint> EndPath { get; set; }       // End Path
        public IEnumerable<WoWUnit> KillNpcs
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                       .Where(ret => ret.HealthPercent > 1 && ret.Entry == KillNpcId);
            }
        }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        private CircularQueue<WoWPoint> Path { get; set; }
        private WoWPoint StartPoint { get; set; }    // Start path
        public WoWUnit Vehicle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .FirstOrDefault(ret => ret.Entry == VehicleId && ret.CreatedByUnitGuid == Me.Guid);
            }
        }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: AnEndToAllThings.cs 219 2012-03-02 13:28:00Z raphus $"); } }
        public override string SubversionRevision { get { return ("$Revision: 219 $"); } }


        ~AnEndToAllThings()
        {
            Dispose(false);
        }

        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                // NOTE: we should call any Dispose() method for any managed or unmanaged
                // resource, if that resource provides a Dispose() method.

                // Clean up managed resources, if explicit disposal...
                if (isExplicitlyInitiatedDispose)
                {
                    // empty, for now
                }

                // Clean up unmanaged resources (if any) here...
                if (_configMemento != null)
                { _configMemento.Dispose(); }

                _configMemento = null;

                BotEvents.OnBotStop -= BotEvents_OnBotStop;
                BotEvents.Player.OnPlayerDied -= Player_OnPlayerDied;
                Targeting.Instance.RemoveTargetsFilter -= Instance_RemoveTargetsFilter;

                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        public void BotEvents_OnBotStop(EventArgs args)
        {
            Dispose();
        }


        public IEnumerable<WoWPoint> ParseWoWPoints(IEnumerable<XElement> elements)
        {
            var temp = new List<WoWPoint>();

            foreach (XElement element in elements)
            {
                XAttribute xAttribute, yAttribute, zAttribute;
                xAttribute = element.Attribute("X");
                yAttribute = element.Attribute("Y");
                zAttribute = element.Attribute("Z");

                float x, y, z;
                float.TryParse(xAttribute.Value, out x);
                float.TryParse(yAttribute.Value, out y);
                float.TryParse(zAttribute.Value, out z);
                temp.Add(new WoWPoint(x, y, z));
            }

            return temp;
        }

        private void ParsePaths()
        {
            var endPath = new CircularQueue<WoWPoint>();
            var startPoint = WoWPoint.Empty;
            var path = new CircularQueue<WoWPoint>();

            foreach (WoWPoint point in ParseWoWPoints(Element.Elements().Where(elem => elem.Name == "Start")))
                startPoint = point;

            foreach (WoWPoint point in ParseWoWPoints(Element.Elements().Where(elem => elem.Name == "End")))
                endPath.Enqueue(point);

            foreach (WoWPoint point in ParseWoWPoints(Element.Elements().Where(elem => elem.Name == "Hop")))
                path.Enqueue(point);

            StartPoint = startPoint;
            EndPath = endPath;
            Path = path;
            _isInitialized = true;
        }

        /// <summary> Returns a quest object, 'An end to all things...' </summary>
        private PlayerQuest Quest
        {
            get
            {
                return _quest ?? (_quest = Me.QuestLog.GetQuestById(12779));
            }
        }

        /// <summary> The vehicle as a wowunit </summary>
        public static void CastPetAction(WoWPetSpell spell)
        {
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        private static void Player_OnPlayerDied()
        {
            LevelBot.ShouldUseSpiritHealer = true;
        }

        private static void Instance_RemoveTargetsFilter(List<WoWObject> units)
        {
            units.Clear();
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return
                new PrioritySelector(

                    new Decorator(ret => !_isInitialized,
                        new Action(ret => ParsePaths())),

                    // Go home.
                    new Decorator(ret => Quest != null && Quest.IsCompleted || _remountTimer.Elapsed.TotalMinutes >= 14,
                        new PrioritySelector(
                            new Decorator(ret => ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(r => r.Entry == 29107 && Vehicle.Location.Distance(r.Location) <= 10) != null,
                                new Sequence(
                                    new Action(ret => Lua.DoString("VehicleExit()")),
                                    new Action(ret => _isBehaviorDone = Quest.IsCompleted),
                                    new Action(ret => _remountTimer.Reset())
                                    )),

                            new Decorator(ret => Vehicle == null,
                                new Sequence(ret => Me.CarriedItems.FirstOrDefault(i => i.Entry == ItemId),
                                    new DecoratorContinue(ret => ret == null,
                                        new Sequence(
                                            new Action(ret => LogMessage("fatal", "Unable to find ItemId({0}) in inventory.", ItemId))
                                            )),

                                    new WaitContinue(60, ret => ((WoWItem)ret).Cooldown == 0,
                // Use the item
                                        new Sequence(
                                            new Action(ret => ((WoWItem)ret).UseContainerItem()),
                                            new Action(ret => ParsePaths()))
                                        ),

                                    // Wait until we are in the vehicle
                                    new WaitContinue(5, ret => Vehicle != null,
                                        new Sequence(
                                            new Action(ret => _remountTimer.Reset()),
                                            new Action(ret => _remountTimer.Start()),
                                            new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(500)))
                                            )
                                        ))),

                            new Decorator(ret => EndPath.Peek().Distance(Vehicle.Location) <= 6,
                                new Action(ret => EndPath.Dequeue())),

                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Flying back to turn in the quest"),
                                new Action(ret => WoWMovement.ClickToMove(EndPath.Peek())))
                                )),

                    new Decorator(ret => Vehicle == null,
                        new Sequence(ret => Me.CarriedItems.FirstOrDefault(i => i.Entry == ItemId),
                            new DecoratorContinue(ret => ret == null,
                                new Sequence(
                                    new Action(ret => LogMessage("fatal", "Unable to locate ItemId({0}) in inventory.", ItemId))
                                    )),

                            new WaitContinue(60, ret => ((WoWItem)ret).Cooldown == 0,
                // Use the item
                                new Sequence(
                                    new Action(ret => ParsePaths()),
                                    new Action(ret => ((WoWItem)ret).UseContainerItem()))
                                ),

                            // Wait until we are in the vehicle
                            new WaitContinue(5, ret => Vehicle != null,
                                new Sequence(
                                    new Action(ret => _remountTimer.Reset()),
                                    new Action(ret => _remountTimer.Start()),
                                    new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(500)))
                                    )
                                ))),

                    new Decorator(ret => Vehicle != null,

                        new PrioritySelector(
                            new Decorator(ret => !_remountTimer.IsRunning,
                                new Action(ret => _remountTimer.Start())),

                            new Decorator(
                                ret => StartPoint != WoWPoint.Empty,
                                new PrioritySelector(
                                    new Decorator(
                                        ret => Vehicle.Location.Distance2D(StartPoint) < 15,
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Pathing through"),
                                            new Action(ret => StartPoint = WoWPoint.Empty))),
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Moving towards start point"),
                                        new Action(ret => Navigator.PlayerMover.MoveTowards(StartPoint))))),

                            new Decorator(ret => Path.Peek().Distance2DSqr(Vehicle.Location) <= 30 * 30,
                                new Action(ret => Path.Dequeue())),

                            new Decorator(ret => (Vehicle.HealthPercent <= 70 || Vehicle.ManaPercent <= 35) &&
                                                 HealNpcs != null && HealSpell != null && !HealSpell.Spell.Cooldown,
                                 new PrioritySelector(
                                    ret => HealNpcs.Where(n => Vehicle.IsSafelyFacing(n)).OrderBy(n => n.DistanceSqr).FirstOrDefault(),
                                    new Decorator(
                                        ret => ret != null && ((WoWUnit)ret).InLineOfSight,
                                        new PrioritySelector(
                                            new Decorator(
                                                ret => ((WoWUnit)ret).Location.Distance(Vehicle.Location) > 15,
                                                new Action(ret => WoWMovement.ClickToMove(((WoWUnit)ret).Location.Add(0, 0, 10)))),
                                            new Action(ret =>
                                            {
                                                WoWMovement.MoveStop();
                                                CastPetAction(HealSpell);
                                            }))))),

                            new Sequence(
                                ret => KillNpcs.Where(n => n.Distance2DSqr > 20 * 20 && Vehicle.IsSafelyFacing(n)).OrderBy(n => n.DistanceSqr).FirstOrDefault(),
                                new DecoratorContinue(
                                    ret => ret != null && ((WoWUnit)ret).InLineOfSight && AttackSpell != null && !AttackSpell.Spell.Cooldown && !StyxWoW.GlobalCooldown,
                                    new Sequence(
                                        new Action(ret =>
                                            {
                                                Vector3 v = ((WoWUnit)ret).Location - StyxWoW.Me.Location;
                                                v.Normalize();
                                                Lua.DoString(string.Format(
                                                    "local pitch = {0}; local delta = pitch - VehicleAimGetAngle() + 0.1; VehicleAimIncrement(delta);",
                                                    Math.Asin(v.Z).ToString(CultureInfo.InvariantCulture)));
                                            }),
                                        new Action(ret => CastPetAction(AttackSpell)),
                                        new Action(ret => StyxWoW.SleepForLagDuration()))),
                                        new Action(ret => StyxWoW.SleepForLagDuration()),
                                new Action(ret => WoWMovement.ClickToMove(Path.Peek()))
                                )
                        )));
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get { return (_isBehaviorDone); }
        }


        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                // The ConfigMemento() class captures the user's existing configuration.
                // After its captured, we can change the configuration however needed.
                // When the memento is dispose'd, the user's original configuration is restored.
                // More info about how the ConfigMemento applies to saving and restoring user configuration
                // can be found here...
                //     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_Saving_and_Restoring_User_Configuration
                _configMemento = new ConfigMemento();

                BotEvents.Player.OnPlayerDied += Player_OnPlayerDied;
                BotEvents.OnBotStop += BotEvents_OnBotStop;
                Targeting.Instance.RemoveTargetsFilter += Instance_RemoveTargetsFilter;

                // Disable any settings that may cause distractions --
                // When we do this quest, we don't want to be distracted by other things.
                // NOTE: these settings are restored to their normal values when the behavior completes
                // or the bot is stopped.
                CharacterSettings.Instance.HarvestHerbs = false;
                CharacterSettings.Instance.HarvestMinerals = false;
                CharacterSettings.Instance.LootChests = false;
                CharacterSettings.Instance.LootMobs = false;
                CharacterSettings.Instance.NinjaSkin = false;
                CharacterSettings.Instance.SkinMobs = false;
                CharacterSettings.Instance.RessAtSpiritHealers = true;

                TreeRoot.GoalText = this.GetType().Name + ": In Progress";
            }
        }

        #endregion
    }
}
