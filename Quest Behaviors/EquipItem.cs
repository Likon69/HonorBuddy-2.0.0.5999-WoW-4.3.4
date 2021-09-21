// Behavior originally contributed by HighVoltz.
//
// WIKI DOCUMENTATION:
//      http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_EquipItem
//     
// QUICK DOX:
//      Equips the specified item into a character equipment slot.  You may specify the slot, or allow it to default.
//      If an item is already occupying the equipment slot, it will be replaced with the specified item.
//
//  Parameters (required, then optional--both listed alphabetically):
//      ItemId: Id of the item to equip
//
//      QuestId [Default:none]:
//      QuestCompleteRequirement [Default:NotComplete]:
//      QuestInLogRequirement [Default:InLog]:
//              A full discussion of how the Quest* attributes operate is described in
//              http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
//      Slot [Default: first available]: Slot into which the item will be equipped.
//              Slot are defined on http://www.wowpedia.org/Equipment_slot.
//              The values allowed for this attribute are summarized in the following table:
//                   None ("first available")   Finger0Slot         SecondaryHandSlot
//                   AmmoSlot                   Finger1Slot         ShirtSlot
//                   BackSlot                   HandsSlot           ShoulderSlot
//                   Bag0Slot                   HeadSlot            TabardSlot
//                   Bag1Slot                   LegsSlot            Trinket0Slot
//                   Bag2Slot                   MainHandSlot        Trinket1Slot
//                   Bag3Slot                   NeckSot             WaistSlot
//                   ChestSlot                  RangedSlot          WristSlot
//
using System;
using System.Collections.Generic;
using System.Linq;

using Styx.Logic.BehaviorTree;
using Styx.Logic.Inventory;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{

    public class EquipItem : CustomForcedBehavior
    {
        public EquipItem(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                ItemId = GetAttributeAsNullable<int>("ItemId", true, ConstrainAs.ItemId, null) ?? 0;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                Slot = GetAttributeAsNullable<InventorySlot>("Slot", false, null, null) ?? InventorySlot.None;
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
        public int ItemId { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public InventorySlot Slot { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: EquipItem.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~EquipItem()
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
                    new Action(c =>
                    {
                        if (Slot == InventorySlot.None)
                        {
                            Lua.DoString("EquipItemByName (\"" + ItemId + "\")");
                        }
                        else
                        {
                            WoWItem item = ObjectManager.Me.BagItems.FirstOrDefault(i => i.Entry == ItemId);
                            if (item != null)
                            {
                                Lua.DoString("PickupContainerItem({0},{1}) EquipCursorItem({2})",
                                    item.BagIndex + 1, item.BagSlot + 1, (int)Slot);
                            }
                        }
                        _isBehaviorDone = true;
                    })
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
                WoWItem item = StyxWoW.Me.CarriedItems.FirstOrDefault(ret => ret.Entry == ItemId);

                if (item != null)
                { TreeRoot.GoalText = string.Format("Equipping [{0}] Into Slot: {1}", item.Name, Slot); }
            }
        }

        #endregion
    }
}
