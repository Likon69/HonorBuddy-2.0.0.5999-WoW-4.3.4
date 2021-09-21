// Behavior originally contributed by HighVoltz.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
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
    /// Finds Npc's that match MobId,MobId2 or MobId3 and beats them up using AutoAttack. useful for a dk quest
    /// ##Syntax##
    /// MobId, MobId2, ...MobIdN[CountRequired:1]: Id of the NPC
    /// QuestId:Id of the quest to perform this Behavior on. This behavior will finish is the quest is complete. 
    /// HealthPercent(Optional) Health Present to stop autoattack at and let CC take over: Default: 25
    /// X,Y,Z: The location where you want to move to
    /// </summary>
    /// 
    public class FindAndBeatNpcs : CustomForcedBehavior
    {
        public FindAndBeatNpcs(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                HealthPercent = GetAttributeAsNullable<double>("HealthPercent", false, ConstrainAs.Percent, null) ?? 25;
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                MobIds = GetNumberedAttributesAsArray<int>("MobId", 1, ConstrainAs.MobId, null);
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
        public double HealthPercent { get; private set; }
        public WoWPoint Location { get; private set; }
        public int[] MobIds { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }
        public WoWUnit Npc
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWUnit>(true)
                                     .OrderBy(o => o.Distance)
                                     .FirstOrDefault(o => !o.Dead
                                                     && !Blacklist.Contains(o.Guid)
                                                     && (!o.GotTarget || o.IsTargetingMeOrPet)
                                                     && MobIds.Contains((int)o.Entry)));
            }
        }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: FindAndBeatNpcs.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~FindAndBeatNpcs()
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


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ??
                (_root = new PrioritySelector(
                    new Decorator(c => Npc != null,
                        new Action(c =>
                        {
                            if (!Npc.Attackable)
                                Blacklist.Add(Npc.Guid, new TimeSpan(0, 5, 0));

                            if ((Me.Combat && (Me.GotTarget && Me.CurrentTarget != Npc && !MobIds.Contains((int)Me.CurrentTarget.Entry))
                                || Me.HealthPercent < HealthPercent) || IsDone)
                            {
                                return RunStatus.Success;
                            }
                            WoWUnit lootable = ObjectManager.GetObjectsOfType<WoWUnit>().OrderBy(u => u.Distance).
                                FirstOrDefault(u => u.Lootable);
                            if (lootable != null)
                            {
                                if (!lootable.WithinInteractRange)
                                {
                                    if (Npc.Distance < 30 && SpellManager.CanCast("Death Grip"))
                                    {
                                        SpellManager.Cast("Death Grip");
                                        WoWMovement.MoveStop();
                                    }
                                    else
                                        Navigator.MoveTo(lootable.Location);
                                }
                                else
                                    lootable.Interact();
                                if (Logic.Inventory.Frames.LootFrame.LootFrame.Instance != null &&
                                    Logic.Inventory.Frames.LootFrame.LootFrame.Instance.IsVisible)
                                {
                                    Logic.Inventory.Frames.LootFrame.LootFrame.Instance.LootAll();
                                    if (Me.GotTarget)
                                        Blacklist.Add(Me.CurrentTarget, new TimeSpan(1, 0, 0));
                                    Me.ClearTarget();
                                }
                                return RunStatus.Running;
                            }
                            if (!Me.GotTarget || Me.CurrentTarget != Npc)
                                Npc.Target();
                            if (!Npc.WithinInteractRange)
                            {
                                TreeRoot.GoalText = string.Format("Moving to {0}", Npc.Name);
                                Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(Me.Location, Npc.Location, 3));
                            }
                            else
                            {
                                if (Me.IsMoving)
                                    WoWMovement.MoveStop();
                                if (!Me.IsAutoAttacking || !Me.IsSafelyFacing(ObjectManager.Me.CurrentTarget))
                                {
                                    TreeRoot.GoalText = string.Format("Bullying {0}", Npc.Name);
                                    Npc.Interact();
                                }
                            }
                            return RunStatus.Running;
                        })),
                    new Action(c => Navigator.MoveTo(Location))
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
                return (!UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
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

                TreeRoot.GoalText = GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
