using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Styx;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Pathing;
using TreeSharp;
using Action = TreeSharp.Action;

namespace _24817
{
    public class _24817:CustomForcedBehavior
    {
        public _24817(Dictionary<string, string> Args)
            : base(Args)
        {
            try
            {
                QuestId = GetAttributeAsQuestId("QuestId", true, null) ?? 0;
            }
            catch
            {
                Styx.Helpers.Logging.Write("Problem in 24817 Behavior parsing QuestId Attibute!");
            }
        }

        public int QuestId { get; set; }
        public QuestCompleteRequirement questCompleteRequirement = QuestCompleteRequirement.NotComplete;
        public QuestInLogRequirement questInLogRequirement = QuestInLogRequirement.InLog;
        private bool IsBehaviorDone = false;
        private Composite _root;
        public List<WoWGameObject> q24817controller
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(ret => (ret.Entry == 202108 && !ObjectManager.Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> q24817_hammer
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 36682 && !ObjectManager.Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public List<WoWUnit> q24817_vehicle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(ret => (ret.Entry == 38318 && !ObjectManager.Me.Dead)).OrderBy(ret => ret.Distance).ToList();
            }
        }
        public override bool IsDone
        {
            get
            {
                return (IsBehaviorDone);
            }
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
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
                    new Decorator(
                        ret => !ObjectManager.Me.HasAura("Mechashark X-Steam"),
                                new Sequence(
                                    new Action(ret => Navigator.MoveTo(q24817controller[0].Location)),
                                    new Action(ret => q24817controller[0].Interact()),
                                    new Action(ret => Thread.Sleep(5000))
                                    )),
                    new Decorator(
                        ret => q24817_hammer[0].IsAlive,
                        new PrioritySelector(
                            new Decorator(
                                ret => ObjectManager.Me.CurrentTarget != q24817_hammer[0],
                                new Sequence(
                                    new Action(ret => 
                                    {
							            if (q24817_hammer.Count > 0 && q24817_hammer[0].Location.Distance(ObjectManager.Me.Location) > 45)
							            {
								            Navigator.MoveTo(q24817_hammer[0].Location);
								            Thread.Sleep(100);
							            }
							            if (q24817_hammer.Count > 0 && (q24817_hammer[0].Location.Distance(ObjectManager.Me.Location) <= 45))
							            {
                                            while (!ObjectManager.Me.QuestLog.GetQuestById(24817).IsCompleted)
                                            {
                                                q24817_hammer[0].Face();
                                                q24817_hammer[0].Target();
                                                WoWMovement.Move(WoWMovement.MovementDirection.Backwards);
                                                Thread.Sleep(200);
                                                WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards);
                                                Lua.DoString("CastPetAction(3)");
                                                Lua.DoString("CastPetAction(2)");
                                                Lua.DoString("CastPetAction(1)");
                                            }
							            }
                                    }))))),
                     new Decorator(
                         ret => ObjectManager.Me.QuestLog.GetQuestById(24817).IsCompleted,
                         new Sequence(
                             new Action(ret => Lua.DoString("VehicleExit()")),
                             new Action(ret => IsBehaviorDone = true)))
                    ));

        }
    }
}
