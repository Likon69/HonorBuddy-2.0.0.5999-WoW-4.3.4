// Behavior originally contributed by Nesox.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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


namespace Styx.Bot.Quest_Behaviors
{
    /// <summary>
    /// Allows you to use items on nearby gameobjects/npc's
    /// ##Syntax##
    /// [Optional]QuestId: The id of the quest.
    /// MobId, MobId2, ...MobIdN [CountRequired:1]: The id of the object.
    /// ItemId: The id of the item to use.
    /// [Optional]NumOfTimes: Number of times to use said item.
    /// [Optional]CollectionDistance: The distance it will use to collect objects. DefaultValue:10000 yards( some NPCs can be view further then 100 yards)
    /// [Optional]HasAura: If a unit has a certian aura to check before using item. (By: j0achim)
    /// [Optional]StopMovingOnUse: (true/false) stops moving when using item. Default:true (By:HighVoltz)
    /// [Optional]HasGroundTarget: (true/false) true if you need to click the ground to cast spell in that area(Default: false)(By:HighVoltz)
    /// [Optional]IsDead: (true/false) true item is to be used on dead targets (Default: false)(By:HighVoltz)
    /// [Optional]InteractRange: The distance from the Object/NPC to use the item. Default: 4.5(By:HighVoltz)
    /// [Optional]MinionCount: Number of minions to gather. Used for the quest "The Gift That Keeps On Giving" Default: 0(By:HighVoltz)
    /// [Optional] X,Y,Z: The general location where theese objects can be found
    /// </summary>
    public class TheGiftThatKeepsOnGiving : CustomForcedBehavior
    {
        public TheGiftThatKeepsOnGiving(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                int? tmpHasAuraId = GetAttributeAsNullable<int>("HasAuraId", false, ConstrainAs.AuraId, new[] { "HasAura" });

                AuraName = string.Empty;       // populated below
                CollectionDistance = GetAttributeAsNullable<double>("CollectionDistance", false, ConstrainAs.Range, null) ?? 100;
                HasGroundTarget = GetAttributeAsNullable<bool>("HasGroundTarget", false, null, null) ?? false;
                InteractRange = GetAttributeAsNullable<double>("InteractRange", false, ConstrainAs.Range, null) ?? 4.5;
                IsDead = GetAttributeAsNullable<bool>("IsDead", false, null, null) ?? false;
                ItemId = GetAttributeAsNullable<int>("ItemId", true, ConstrainAs.ItemId, null) ?? 0;
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                MinionCount = GetAttributeAsNullable<int>("MinionCount", false, new ConstrainTo.Domain<int>(0, int.MaxValue), null) ?? 0;
                MobIds = GetNumberedAttributesAsArray<int>("MobId", 1, ConstrainAs.MobId, null);
                NumOfTimes = GetAttributeAsNullable<int>("NumOfTimes", false, ConstrainAs.RepeatCount, null) ?? 1;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                StopMovingOnUse = GetAttributeAsNullable<bool>("StopMovingOnUse", false, null, null) ?? true;

                if (tmpHasAuraId.HasValue)
                {
                    WoWSpell spell = WoWSpell.FromId(tmpHasAuraId.Value);

                    if (spell != null)
                    { AuraName = spell.Name; }
                }
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
        public string AuraName { get; private set; }
        public double CollectionDistance { get; private set; }
        public bool HasGroundTarget { get; private set; }
        public double InteractRange { get; private set; }
        public bool IsDead { get; private set; }
        public int ItemId { get; private set; }
        public WoWPoint Location { get; private set; }
        public int MinionCount { get; private set; }
        public int[] MobIds { get; private set; }
        public int NumOfTimes { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public bool StopMovingOnUse { get; private set; }

        // Private variables for internal state
        private bool _isDisposed;
        private readonly List<ulong> _npcBlacklist = new List<ulong>();
        private Composite _root;
        private readonly Stopwatch _waitTimer = new Stopwatch();

        // Private properties
        private int Counter { get; set; }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: TheGiftThatKeepsOnGiving.cs 219 2012-03-02 13:28:00Z raphus $"); } }
        public override string SubversionRevision { get { return ("$Revision: 219 $"); } }


        ~TheGiftThatKeepsOnGiving()
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


        public WoWObject Object
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWObject>(true)
                                .Where(o => ObjCheck(o, MobIds))
                                .OrderBy(o => o.Distance)
                                .FirstOrDefault();
            }
        }


        bool ObjCheck(WoWObject obj, int[] ids)
        {
            bool ret = false;
            if (ids.Contains((int)obj.Entry) && obj.Distance <= CollectionDistance && obj.InLineOfSight &&
                !_npcBlacklist.Contains(obj.Guid) && AuraCheck(obj))
            {
                ret = (!IsDead || !(obj is WoWUnit) || ((WoWUnit)obj).Dead) &&
                      (IsDead || !(obj is WoWUnit) || !((WoWUnit)obj).Dead);
            }
            // temp fix to HB killing targets without letting us using item...
            if (ret && obj is WoWUnit)
                Blacklist.Add(obj, new System.TimeSpan(0, 10, 0));
            return ret;
        }


        bool AuraCheck(WoWObject obj)
        {
            if (string.IsNullOrEmpty(AuraName) || !(obj is WoWUnit))
                return true;
            if (((WoWUnit)obj).HasAura(AuraName))
                return true;
            return false;
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =

                new PrioritySelector(ctx => Object,

                    new Decorator(ctx => ctx != null && (((WoWObject)ctx).Distance > InteractRange || !((WoWObject)ctx).InLineOfSight),
                        new Sequence(
                            new Action(ctx => TreeRoot.StatusText = "Moving to use item on - " + ((WoWObject)ctx).Name),
                            new Action(ctx => Navigator.MoveTo(((WoWObject)ctx).Location)))),

                    new Decorator(ctx => ctx != null && ((WoWObject)ctx).Distance <= InteractRange,
                        new Sequence(
                            new DecoratorContinue(c => StopMovingOnUse && Me.IsMoving,
                                new Sequence(
                                    new Action(ctx => WoWMovement.MoveStop()),
                                    new WaitContinue(5, ctx => !Me.IsMoving,
                                        new Action(ctx => StyxWoW.SleepForLagDuration()))
                                    )),

                            new Sequence(ctx => StyxWoW.Me.CarriedItems.FirstOrDefault(ret => ret.Entry == ItemId),
                // Set the status text.
                                new Action(ctx => TreeRoot.StatusText = "Using item on " + Object.Name),

                                // If we don't have the item stop!
                                new DecoratorContinue(ctx => ctx == null,
                                    new Action(ctx => LogMessage("fatal", "Could not find ItemId({0}) in inventory.", ItemId))),

                                new DecoratorContinue(ctx => Object.Type == WoWObjectType.Unit,
                                    new Action(ctx => Object.ToUnit().Target())),

                                // Face the object.
                                new Action(ctx => WoWMovement.Face(Object.Guid)),

                                // Use the item.
                                new Action(ctx => ((WoWItem)ctx).UseContainerItem()),

                                new DecoratorContinue(ctx => HasGroundTarget,
                                    new Action(ctx => LegacySpellManager.ClickRemoteLocation(Object.Location))),

                                new WaitContinue(6, ctx => false,
                                    new Sequence(
                                        new Action(ctx => StyxWoW.SleepForLagDuration()),
                                        new Action(ctx => _npcBlacklist.Add(Object.Guid)),
                                        new Action(ctx => _waitTimer.Reset()),

                                        new DecoratorContinue(ctx => !_waitTimer.IsRunning,
                                            new Action(ctx => _waitTimer.Start())),

                                        new Action(ctx => Counter++)
                                        )
                                    )
                                )
                        )),

                    new Sequence(
                        new Action(ctx => LogMessage("info", "Moving to {0}", Location)),
                        new Action(ctx => Navigator.MoveTo(Location))
                        )
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
                return ((Counter >= NumOfTimes)     // normal completion
                        || (MinionCount > 0 && (MinionCount <= Me.Minions.Count))
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


    static class WoWUnitExtentions
    {
        public static bool IsTargetingMinion(this WoWUnit unit)
        {
            if (unit.GotTarget)
            {
                foreach (var minion in ObjectManager.Me.Minions)
                {
                    if (unit.CurrentTarget == minion)
                        return true;
                }
            }
            return false;
        }
    }
}
