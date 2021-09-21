using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Styx.Database;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Combat;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Styx.Bot.Quest_Behaviors
{
    public class Escort6641 : CustomForcedBehavior
    {


        bool success = true;


        public Escort6641(Dictionary<string, string> args)
            : base(args)
        {

        }


        public static LocalPlayer me = ObjectManager.Me;

        public List<WoWUnit> npcList;
		public List<WoWUnit> npcList2;
		public List<WoWGameObject> objList;

        /// <summary>
        /// A Queue for npc's we need to talk to
        /// </summary>
        //private WoWUnit CurrentUnit { get { return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(unit => unit.Distance < 100 && unit.Entry == MobId); } }

        #region Overrides of CustomForcedBehavior

        private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
				
				new Decorator(ret => ((me.QuestLog.GetQuestById(6641) != null &&
                         me.QuestLog.GetQuestById(6641).IsCompleted) || me.QuestLog.GetQuestById(6641).IsFailed),
                        new Action(ret => _isDone = true)),

                        new PrioritySelector(

                           new Decorator(ret => !me.Dead,
                                new Action(delegate
                                {
                                    ObjectManager.Update();

                                    npcList = ObjectManager.GetObjectsOfType<WoWUnit>()
                                        .Where(u => u.Entry == 12717)
                                        .OrderBy(u => u.Distance).ToList();
										
									npcList2 = ObjectManager.GetObjectsOfType<WoWUnit>()
                                        .Where(u => u.Entry == 12940)
                                        .OrderBy(u => u.Distance).ToList();
										
									objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                                        .Where(u => u.Entry == 178247)
                                        .OrderBy(u => u.Distance).ToList();
									if (npcList.Count == 0)	_isDone = true;
									if (me.Combat || npcList[0].Combat)
										{
											return RunStatus.Success;
										}
									if (npcList2.Count > 0)
										if (npcList2[0].Location.Distance(me.Location) < 30)
                                            {
												WoWMovement.MoveStop();
												npcList2[0].Face();
												npcList2[0].Target();
                                                SpellManager.Cast("Arcane Shot");
                                                Thread.Sleep(2000);
                                                return RunStatus.Success;
                                            }
                                            else
                                            {
                                                Navigator.MoveTo(npcList2[0].Location);
                                                Thread.Sleep(300);
                                                return RunStatus.Running;
                                            }

									if (objList.Count >= 1 && !me.Combat && objList[0].Location.Distance(me.Location) < 20)
									{
										if (objList[0].Location.Distance(me.Location) < 3)
                                            {
												WoWMovement.MoveStop();
												Thread.Sleep(6000);
                                                objList[0].Interact();
                                                Thread.Sleep(2000);
                                                return RunStatus.Success;
                                            }
                                            else
                                            {
                                                Navigator.MoveTo(objList[0].Location);
                                                Thread.Sleep(300);
                                                return RunStatus.Running;
                                            }
									}
                                    else if (npcList.Count >= 1 && !me.Combat)
                                    {
                                        Navigator.MoveTo(npcList[0].Location);

                                    }

                                    return RunStatus.Running;

                                })
                                ),

                            new Action(ret => Thread.Sleep(100))
                        )
                    ));
        }

        private bool _isDone;
        public override bool IsDone
        {
            get { return _isDone; }
        }

        #endregion
    }
}
