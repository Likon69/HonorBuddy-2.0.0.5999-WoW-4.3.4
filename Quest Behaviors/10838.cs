using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.BehaviorTree;
using TreeSharp;
using Action = TreeSharp.Action;
using Styx.Logic.Pathing;
using Styx.Combat;
using Styx.Logic.Combat;
using System.Threading;
using System.Diagnostics;

namespace _10838
{
    public class _10838:CustomForcedBehavior
    {
        public _10838(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ??0;
            }
            catch
            {
                Logging.Write("Problem parsing a QuestId in behavior: 10838");
            }
        }
        public int QuestId { get; set; }
        public WoWPoint Location = new WoWPoint(-145.9611, 3192.408, -65.09953);
        public int ItemId = 31606;
        public int MobId = 22258;
        public Stopwatch TimeOut = new Stopwatch();
        public override string SubversionId { get { return ("$Id: 10838.cs 191 2011-08-02 18:59:20Z Azenius $"); } }
        public override string SubversionRevision { get { return ("$Revision: 1 $"); } }

        private ConfigMemento _configMemento;
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        private List<WoWUnit> MobList
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWUnit>()
                                        .Where(u => u.Entry == MobId && !u.Dead)
                                        .OrderBy(u => u.Distance).ToList());
            }
        }
        private List<WoWUnit> Mobs
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry != MobId && !u.Dead && u.IsHostile && u.Distance < 15).OrderBy(u => u.Distance).ToList();
            }
        }

         ~_10838()
        {
            Dispose(false);
        }


        public void     Dispose(bool    isExplicitlyInitiatedDispose)
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
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        public void    BotEvents_OnBotStop(EventArgs args)
        {
             Dispose();
        }


        WoWSpell RangeSpell
        {
            get
            {
                switch (Me.Class)
                {
                    case Styx.Combat.CombatRoutine.WoWClass.Druid:
                        return SpellManager.Spells["Starfire"];
                    case Styx.Combat.CombatRoutine.WoWClass.Hunter:
                        return SpellManager.Spells["Arcane Shot"];
                    case Styx.Combat.CombatRoutine.WoWClass.Mage:
                        return SpellManager.Spells["Frost Bolt"];
                    case Styx.Combat.CombatRoutine.WoWClass.Priest:
                        return SpellManager.Spells["Shoot"];
                    case Styx.Combat.CombatRoutine.WoWClass.Shaman:
                        return SpellManager.Spells["Lightning Bolt"];
                    case Styx.Combat.CombatRoutine.WoWClass.Warlock:
                        return SpellManager.Spells["Curse of Agony"];
                    default: // should never get to here but adding this since the compiler complains
                        return SpellManager.Spells["Auto Attack"]; ;
                }
            }
        }

        public override void   Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get
            {
                return (_isBehaviorDone);
            }
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

                BotEvents.OnBotStop  += BotEvents_OnBotStop;

                // Disable any settings that may interfere with the escort --
                // When we escort, we don't want to be distracted by other things.
                // NOTE: these settings are restored to their normal values when the behavior completes
                // or the bot is stopped.
                CharacterSettings.Instance.HarvestHerbs = false;
                CharacterSettings.Instance.HarvestMinerals = false;
                CharacterSettings.Instance.LootChests = false;
                CharacterSettings.Instance.LootMobs = false;
                CharacterSettings.Instance.NinjaSkin = false;
                CharacterSettings.Instance.SkinMobs = false;

                WoWUnit     mob     = ObjectManager.GetObjectsOfType<WoWUnit>()
                                      .Where(unit => unit.Entry == MobId)
                                      .FirstOrDefault();

                TreeRoot.GoalText = "Escorting " + ((mob != null) ? mob.Name : ("Mob(" + MobId + ")"));
            }
        }

        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                            new Decorator(ret => Me.QuestLog.GetQuestById((uint)QuestId) != null && Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Finished!"),
                                    new WaitContinue(120,
                                        new Action(delegate
                                        {
                                            _isBehaviorDone = true;
                                            return RunStatus.Success;
                                        }))
                                    )),

                           new Decorator(ret => MobList.Count == 0 && Me.Location.Distance(Location) > 5,
                                new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Moving To Location - X: " + Location.X + " Y: " + Location.Y),
                                        new Action(ret => Flightor.MoveTo(Location)),
                                        new Action(ret => Thread.Sleep(300))
                                    )
                                ),
                           new Decorator(ret => MobList.Count == 0 && Me.Location.Distance(Location) <= 5,
                               new Sequence(
                                        new Action(ret => Lua.DoString("UseItemByName({0})", ItemId)),
                                        new Action(ret => TimeOut.Start())
                                        )
                               ),
                           new Decorator(ret => TimeOut.ElapsedMilliseconds >= 300000,
                               new Sequence(
                                   new Action(ret => MobList[0].Interact()),
                                   new Action(ret => Thread.Sleep(500)),
                                   new Action(ret => Lua.DoString("SelectGossipOption(1)"))
                                   )
                              ),

                           new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.IsFriendly,
                               new Action(ret => Me.ClearTarget())),

                           new Decorator(
                               ret => Mobs.Count > 0 && Mobs[0].IsHostile,
                               new PrioritySelector(
                                   new Decorator(
                                       ret => Me.CurrentTarget != Mobs[0],
                                       new Action(ret =>
                                       {
                                           Mobs[0].Target();
                                           StyxWoW.SleepForLagDuration();
                                       })),
                                   new Decorator(
                                       ret => !Me.Combat,
                                       new PrioritySelector(
                                            new Decorator(
                                                ret => RoutineManager.Current.PullBehavior != null,
                                                RoutineManager.Current.PullBehavior),
                                            new Action(ret => RoutineManager.Current.Pull()))))),

                           new Decorator(ret => Mobs.Count > 0 && (Me.Combat || Mobs[0].Combat),
                                new PrioritySelector(
                                    new Decorator(
                                        ret => Me.CurrentTarget == null && Mobs[0].CurrentTarget != null,
                                        new Sequence(
                                        new Action(ret => MobList[0].Target()),
                                        new Action(ret => StyxWoW.SleepForLagDuration()))),
                                    new Decorator(
                                        ret => !Me.Combat,
                                        new PrioritySelector(
                                            new Decorator(
                                                ret => RoutineManager.Current.PullBehavior != null,
                                                RoutineManager.Current.PullBehavior),
                                            new Action(ret => RoutineManager.Current.Pull())))))

                        )
                    );
        }
        #endregion
    }
}
