// Behavior originally contributed by mastahg.
//
// DOCUMENTATION:
//     
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bots.Quest;
using CommonBehaviors.Actions;
using Styx.Combat.CombatRoutine;
using Styx.Database;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class SpecialTurnIn : CustomForcedBehavior
    {
        ~SpecialTurnIn()
        {
            Dispose(false);
        }

        public SpecialTurnIn(Dictionary<string, string> args) : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? MountSpot;
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                TurnInName = GetAttributeAs<string>("TurnInName", true, ConstrainAs.StringNonEmpty, null) ?? "";
                TurnInId = GetAttributeAsNullable<int>("TurnInId", true, ConstrainAs.MobId, null) ?? 0;
                QuestName = GetAttributeAs<string>("QuestName", true, ConstrainAs.StringNonEmpty, null) ?? "";


                LootId = GetAttributeAsNullable<int>("LootId", true, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;

                //Enemy = GetAttributeAsArray<uint>("Enemys", false, new ConstrainTo.Domain<uint>(0, 100000), new[] { "Enemy" }, null);
                //EnemyDebuff = GetAttributeAsArray<uint>("EnemysDebuff", false, new ConstrainTo.Domain<uint>(0, 100000), new[] { "EnemyDebuff" }, null);
                QuestRequirementComplete = QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = QuestInLogRequirement.InLog;
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error",
                           "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message + "\nFROM HERE:\n" + except.StackTrace +
                           "\n");
                IsAttributeProblem = true;
            }
        }

        private WoWItem HordeLance()
        {
            return StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 46070);
        }

        private WoWItem ArgentLance()
        {
            return StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 46106);
        }

        private WoWItem BestLance()
        {
            return HordeLance() ?? ArgentLance();
        }

        // Attributes provided by caller
        public uint[] MobIds { get; private set; }
        public int TurnInId { get; private set; }
        public int QuestId { get; private set; }
        public int LootId { get; private set; }
        public string TurnInName { get; private set; }
        public string QuestName { get; private set; }

        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint Location { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;


        // Private properties
        private LocalPlayer Me
        {
            get { return (ObjectManager.Me); }
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

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint) QuestId);
            return quest == null || quest.IsCompleted;
        }


        public Composite DoneYet
        {
            get
            {
                return new Decorator(ret => IsQuestComplete(), new Action(delegate
                                                                              {
                                                                                  TreeRoot.StatusText = "Finished!";
                                                                                  _isBehaviorDone = true;
                                                                                  return RunStatus.Success;
                                                                              }));
            }
        }


        public Composite GetClose
        {
            get
            {
                return
                    new PrioritySelector(
                        new Decorator(r=>StyxWoW.Me.QuestLog.GetQuestById((uint) QuestId) == null, new Action(r=>_isBehaviorDone=true)),
                        new Decorator(ret => Me.Location.Distance(turin.Location) > turin.InteractRange,
                                      new Action(r => Navigator.MoveTo(turin.Location))),
                        new Decorator(ret => !GossipFrame.Instance.IsVisible && QuestManager.QuestFrame.IsVisible,
                                      new Action(r =>
                                                     {
                                                         Thread.Sleep(300);
                                                         QuestManager.QuestFrame.SelectQuestReward(LootId);
                                                         QuestManager.QuestFrame.SelectQuestReward(LootId);
                                                         Thread.Sleep(300);
                                                         QuestManager.QuestFrame.CompleteQuest();
                                                         Thread.Sleep(300);
                                                     })),
                        new Decorator(ret => GossipFrame.Instance.IsVisible && !QuestFrame.Instance.IsVisible,
                                      new Action(r => stol())),
                        new Decorator(ret => turin.WithinInteractRange, new Action(r => turin.Interact())));
            }
        }

        protected override Composite CreateBehavior()
        {
            return _root ??
                   (_root =
                    new Decorator(ret => !_isBehaviorDone, new PrioritySelector(GetClose, new ActionAlwaysSucceed())));
        }

        private WoWUnit turin
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(x => x.Entry == turninguy.Entry); }
        }

        private NpcResult turninguy
        {
            get { return NpcQueries.GetNpcById((uint) TurnInId); }
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

        protected RunStatus stol()
        {
            if (QuestManager.QuestFrame.IsVisible && (QuestManager.QuestFrame.CurrentShownQuestId != 0))
            {
                return RunStatus.Success;
            }
            if (!QuestManager.GossipFrame.IsVisible)
            {
                if (QuestManager.QuestFrame.IsVisible)
                {
                    List<uint> quests = QuestManager.QuestFrame.Quests;
                    if ((QuestId != -1) && !quests.Contains((uint) QuestId))
                    {
                        QuestManager.QuestFrame.Close();
                        return RunStatus.Failure;
                    }
                    for (int i = 0; i < quests.Count; i++)
                    {
                        if ((QuestId == -1) || (((ulong) quests[i]) == (ulong) QuestId))
                        {
                            PlayerQuest questById = ObjectManager.Me.QuestLog.GetQuestById(quests[i]);
                            if ((QuestId != -1) || questById.IsCompleted)
                            {
                                QuestManager.GossipFrame.SelectActiveQuest(i);
                                return RunStatus.Success;
                            }
                        }
                    }
                }
            }
            else
            {
                List<GossipQuestEntry> activeQuests = QuestManager.GossipFrame.ActiveQuests;
                for (int j = 0; j < activeQuests.Count; j++)
                {
                    if ((QuestId == -1) || (activeQuests[j].Id == QuestId))
                    {
                        PlayerQuest quest = ObjectManager.Me.QuestLog.GetQuestById((uint) activeQuests[j].Id);
                        if ((QuestId != -1) || quest.IsCompleted)
                        {
                            QuestManager.GossipFrame.SelectActiveQuest(activeQuests[j].Index);
                            return RunStatus.Success;
                        }
                    }
                }
            }
            return RunStatus.Failure;
        }


        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();
            Logging.Write("Quest Behavior made by mastahg.");
            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                if (TreeRoot.Current != null && TreeRoot.Current.Root != null &&
                    TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite) currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                //GossipFrame.Instance.ActiveQuests.FirstOrDefault(x => x.Id == QuestId);
                //GossipFrame.Instance.SelectActiveQuest();
                //Me.QuestLog.GetQuestById(13862).
                //NpcResult npcById = NpcQueries.GetNpcById(node.TurnInId);


                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint) QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}