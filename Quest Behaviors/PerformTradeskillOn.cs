// Behavior originally contributed by Unknown.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using CommonBehaviors.Actions;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    class PerformTradeskillOn : CustomForcedBehavior
    {
        public PerformTradeskillOn(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                CastOnItemId = GetAttributeAsNullable<int>("CastOnItemId", false, ConstrainAs.ItemId, null) ?? 0;
                NumOfTimes = GetAttributeAsNullable<int>("NumOfTimes", false, ConstrainAs.RepeatCount, new[] { "NumTimes" }) ?? 1;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                TradeSkillId = GetAttributeAsNullable<int>("TradeSkillId", true, ConstrainAs.SpellId, null) ?? 0;
                TradeSkillItemId = GetAttributeAsNullable<int>("TradeSkillItemId", true, ConstrainAs.ItemId, null) ?? 0;
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
        public int? CastOnItemId { get; private set; }  /// If set, an item ID to cast the trade skill on.
        public int NumOfTimes { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public int TradeSkillId { get; private set; }
        public int TradeSkillItemId { get; private set; }  // Identifier for the trade skill item. E.g; the actual 'item' we use from the tradeskill window.


        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: PerformTradeskillOn.cs 239 2012-08-23 14:13:26Z raphus $"); } }
        public override string SubversionRevision { get { return ("$Revision: 239 $"); } }


        ~PerformTradeskillOn()
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


        private void PerformTradeSkill()
        {
            Lua.DoString("DoTradeSkill(" + GetTradeSkillIndex() + ", " + (NumOfTimes == 0 ? 1 : NumOfTimes) + ")");
            Thread.Sleep(500);

            if (CastOnItemId.HasValue)
            {
                var item = StyxWoW.Me.CarriedItems.FirstOrDefault(i => i.Entry == CastOnItemId.Value);
                if (item == null)
                {
                    LogMessage("fatal", "Could not find ItemId({0}).", CastOnItemId.Value);
                    return;
                }
                item.Use();
                Thread.Sleep(500);
            }

            if (Lua.GetReturnVal<bool>("return StaticPopup1:IsVisible()", 0))
                Lua.DoString("StaticPopup1Button1:Click()");

            Thread.Sleep(500);

            while (StyxWoW.Me.IsCasting)
            {
                Thread.Sleep(100);
            }

            _isBehaviorDone = true;
        }



        private Composite CreateTradeSkillCast()
        {
            return
                new PrioritySelector(
                    new Decorator(ret => Lua.GetReturnVal<bool>("return StaticPopup1:IsVisible()", 0),
                        new Action(ret => Lua.DoString("StaticPopup1Button1:Click()"))
                    ),

                    new Decorator(ret => !Lua.GetReturnVal<bool>("return TradeSkillFrame:IsVisible()", 0),
                        new Action(ret => WoWSpell.FromId((int)TradeSkillId).Cast())),

                    new Decorator(ret => StyxWoW.Me.IsCasting,
                        new ActionAlwaysSucceed()),

                new Action(ret => PerformTradeSkill()));
        }


        private int GetTradeSkillIndex()
        {
            using (new FrameLock())
            {
                int count = Lua.GetReturnVal<int>("return GetNumTradeSkills()", 0);
                for (int i = 1; i <= count; i++)
                {
                    string link = Lua.GetReturnVal<string>("return GetTradeSkillItemLink(" + i + ")", 0);

                    // Make sure it's not a category!
                    if (string.IsNullOrEmpty(link))
                    {
                        continue;
                    }

                    link = link.Remove(0, link.IndexOf(':') + 1);
                    if (link.IndexOf(':') != -1)
                        link = link.Remove(link.IndexOf(':'));
                    else
                        link = link.Remove(link.IndexOf('|'));

                    int id = int.Parse(link);

                    LogMessage("debug", "ID: " + id + " at " + i + " - " + WoWSpell.FromId(id).Name);

                    if (id == TradeSkillItemId)
                        return i;
                }
            }
            return 0;
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.IsMoving,
                    new Action(ret => Navigator.PlayerMover.MoveStop())),

                CreateTradeSkillCast());
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
