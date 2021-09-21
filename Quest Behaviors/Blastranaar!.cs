using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonBehaviors.Actions;
using Styx;
using Styx.Plugins;
using Styx.Plugins.PluginClass;
using Styx.Logic.BehaviorTree;
using TreeSharp;
using Styx.Logic.Questing;
using Styx.Logic.Profiles.Quest;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = TreeSharp.Action;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.Logic.Combat;

namespace Blastranaar
{
    public class Blastranaar : CustomForcedBehavior
    {
        public Blastranaar(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = 13947;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
            }
            catch
            {
                Logging.Write("Problem parsing a QuestId in behavior: Blastranaar");
            }
        }
        public int QuestId { get; set; }
        private bool _isBehaviorDone;
        public int MobIdThraka = 34429;
        public int MobIdSentinel = 34494;
        public int MobIdThrower = 34492;
        private Composite _root;
        public WoWPoint Location = new WoWPoint(3048.918, -497.9261, 205.6379);
        public QuestCompleteRequirement questCompleteRequirement = QuestCompleteRequirement.NotComplete;
        public QuestInLogRequirement questInLogRequirement = QuestInLogRequirement.InLog;
        public override bool IsDone
        {
            get
            {
                return _isBehaviorDone;
            }
        }
        private LocalPlayer Me
        {
            get { return (ObjectManager.Me); }
        }

        public override void OnStart()
        {
            OnStart_HandleAttributeProblem();
            if (!IsDone)
            {
                PlayerQuest Quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
                TreeRoot.GoalText = ((Quest != null) ? ("\"" + Quest.Name + "\"") : "In Progress");
            }
        }

        public List<WoWUnit> Sentinels
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdSentinel && !u.Dead && u.Distance < 100).OrderBy(u => u.Distance).ToList();
            }
        }
        public List<WoWUnit> Throwers
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdThrower && !u.Dead && u.Distance < 100).OrderBy(u => u.Distance).ToList();
            }
        }

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }
        private bool IsObjectiveComplete(int objectiveId, uint questId)
        {
            if (Me.QuestLog.GetQuestById(questId) == null)
            {
                return false;
            }
            int returnVal = Lua.GetReturnVal<int>("return GetQuestLogIndexByID(" + questId + ")", 0);
            return
                Lua.GetReturnVal<bool>(
                    string.Concat(new object[] { "return GetQuestLogLeaderBoard(", objectiveId, ",", returnVal, ")" }), 2);
        }

        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsQuestComplete(), new Action(delegate
                    {
                        Lua.DoString("CastPetAction(3)");
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));

            }
        }

        public Composite KillOne
        {
            get
            {
                return new Decorator(r => !IsObjectiveComplete(1, (uint)QuestId), new Action(r =>
                                                                                                {
                                                                                                    Lua.DoString(
                                                                                                        "CastPetAction(1)");
                                                                                                    LegacySpellManager.
                                                                                                        ClickRemoteLocation
                                                                                                        (Sentinels[0].
                                                                                                             Location);
                                                                                                }));
            }
        }
        public Composite KillTwo
        {
            get
            {
                return new Decorator(r => !IsObjectiveComplete(2, (uint)QuestId), new Action(r =>
                {
                    Lua.DoString(
                        "CastPetAction(1)");
                    LegacySpellManager.
                        ClickRemoteLocation
                        (Throwers[0].
                             Location);
                }));
            }
        }

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, KillOne, KillTwo, new ActionAlwaysSucceed())));
        }
    }
}
