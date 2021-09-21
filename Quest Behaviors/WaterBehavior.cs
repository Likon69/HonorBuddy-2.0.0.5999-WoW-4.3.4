// Behavior originally contributed by Natfoth.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Styx.Combat.CombatRoutine;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class WaterBehavior : CustomForcedBehavior
    {
        /// <summary>
        /// Kill/Collect things within the Water
        /// ##Syntax##
        /// QuestId: Id of the quest.
        /// NpcID: MobId of the vehicle before it is mounted.
        /// ObjectID, ObjectID2, ObjectID3: Mob of the actual Vehicle, sometimes it will be the some but sometimes it will not be.
        /// NumberOfTimes: Button bar Number starting from 1
        /// X,Y,Z: Where you want to be at when you fire.
        /// </summary>
        /// 
        public WaterBehavior(Dictionary<string, string> args)
            : base(args)
        {
            try
            {

                LogMessage("warning", "*****\n"
                                        + "* THIS BEHAVIOR IS DEPRECATED, and will be retired on July 31th 2012.\n"
                                        + "*\n"
                                        + "* WaterBehavior adds _no_ _additonal_ _value_ over the CollectThings behavior.\n"
                                        + "* Please update the profile to use the CollectThings behavior."
                                        + "*****");


                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                MobId = GetAttributeAsNullable<int>("MobMountId", false, ConstrainAs.MobId, new[] { "MobId", "NpcId", "NpcID" }) ?? 0;
                ObjectIds = GetNumberedAttributesAsArray("ObjectId", 0, ConstrainAs.ObjectId, new[] { "ObjectID" });
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
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
        public WoWPoint Location { get; private set; }
        public int MobId { get; private set; }
        public int[] ObjectIds { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private int Counter { get; set; }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: WaterBehavior.cs 229 2012-04-25 01:57:29Z natfoth $"); } }
        public override string SubversionRevision { get { return ("$Revision: 229 $"); } }


        ~WaterBehavior()
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
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        private List<WoWUnit> NpcList
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                            .Where(u => u.Entry == MobId && !u.Dead)
                                            .OrderBy(u => u.Distance).ToList();
            }
        }

        private List<WoWUnit> LootList
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                            .Where(u => u.Entry == MobId && u.Dead && u.Lootable)
                                            .OrderBy(u => u.Distance)
                                            .ToList();
            }
        }

        public List<WoWGameObject> ObjectList
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>()
                                        .Where(u => ObjectIds.Contains((int)u.Entry) && !u.InUse && !u.IsDisabled)
                                        .OrderBy(u => u.Distance)
                                        .ToList();
            }
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

        bool isRanged
        {
            get
            {
                return (Me.Class == WoWClass.Druid &&
                    (SpellManager.HasSpell("balanceSpell") || SpellManager.HasSpell("RestoSpell")) ||
                    Me.Class == WoWClass.Shaman &&
                    (SpellManager.HasSpell("ElementalSpell") || SpellManager.HasSpell("RestoSpell")) ||
                    Me.Class == WoWClass.Hunter || Me.Class == WoWClass.Mage || Me.Class == WoWClass.Priest ||
                    Me.Class == WoWClass.Warlock);
            }
        }

        int range
        {
            get
            {
                if (isRanged)
                {
                    return 25;
                }
                else
                {
                    return 3;
                }
            }
        }


        #region Overrides of CustomForcedBehavior.

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                            new Decorator(ret => (Me.QuestLog.GetQuestById((uint)QuestId) != null && Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted),
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Finished!"),
                                    new WaitContinue(120,
                                        new Action(delegate
                                        {
                                            _isBehaviorDone = true;
                                            return RunStatus.Success;
                                        }))
                                    )),

                            new Decorator(ret => Me.GetMirrorTimerInfo(MirrorTimerType.Breath).CurrentTime < 20000 && Me.GetMirrorTimerInfo(MirrorTimerType.Breath).CurrentTime != 0,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Finished!"),
                                    new Action(ret => WoWMovement.ClickToMove(Location)),
                                    new Action(ret => Thread.Sleep(100))
                                )),

                           new Decorator(ret => LootList.Count > 0,
                                new Sequence(
                                    new DecoratorContinue(ret => !LootList[0].WithinInteractRange,
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Moving to Loot - " + LootList[0].Name + " Yards Away " + LootList[0].Location.Distance(Me.Location)),
                                            new Action(ret => WoWMovement.ClickToMove(LootList[0].Location)),
                                            new Action(ret => Thread.Sleep(300))
                                            )
                                    ),
                                    new DecoratorContinue(ret => LootList[0].WithinInteractRange,
                                        new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Looting - " + LootList[0].Name),
                                        new Action(ret => WoWMovement.MoveStop()),
                                        new Action(ret => LootList[0].Interact()),
                                        new Action(ret => Thread.Sleep(1000))
                                            ))
                                    )),

                           new Decorator(ret => NpcList.Count == 0 || ObjectList.Count == 0,
                                new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Moving To Location - X: " + Location.X + " Y: " + Location.Y),
                                        new Action(ret => Navigator.MoveTo(Location)),
                                        new Action(ret => Thread.Sleep(100))
                                    )
                                ),

                           new Decorator(ret => NpcList.Count > 0,
                                new Sequence(
                                    new DecoratorContinue(ret => NpcList[0].Location.Distance(Me.Location) > range,
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Moving to Mob - " + NpcList[0].Name + " Yards Away " + NpcList[0].Location.Distance(Me.Location)),
                                            new Action(ret => WoWMovement.ClickToMove(NpcList[0].Location)),
                                            new Action(ret => Thread.Sleep(300))
                                            )
                                    ),
                                    new DecoratorContinue(ret => NpcList[0].Location.Distance(Me.Location) <= range,
                                        new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Attacking Mob - " + NpcList[0].Name + " With Spell: " + RangeSpell.Name),
                                        new Action(ret => WoWMovement.MoveStop()),
                                        new Action(ret => NpcList[0].Target()),
                                        new Action(ret => NpcList[0].Face()),
                                        new Action(ret => Thread.Sleep(200)),
                                        new Action(ret => SpellManager.Cast(RangeSpell)),
                                        new Action(ret => Thread.Sleep(300))
                                            ))
                                    )),

                            new Decorator(ret => ObjectList.Count > 0,
                                new Sequence(
                                    new DecoratorContinue(ret => !ObjectList[0].WithinInteractRange,
                                        new Sequence(
                                            new Action(ret => TreeRoot.StatusText = "Moving to Object - " + ObjectList[0].Name + " Yards Away " + ObjectList[0].Location.Distance(Me.Location)),
                                            new Action(ret => WoWMovement.ClickToMove(ObjectList[0].Location)),
                                            new Action(ret => Thread.Sleep(300))
                                            )
                                    ),
                                    new DecoratorContinue(ret => ObjectList[0].WithinInteractRange,
                                        new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Opening Object - " + ObjectList[0].Name),
                                        new Action(ret => WoWMovement.MoveStop()),
                                        new Action(ret => ObjectList[0].Interact()),
                                        new Action(ret => Thread.Sleep(1000)),
                                        new Action(ret => Counter++)
                                            ))
                                    ))
                    ));
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get
            {
                return (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
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
                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}

