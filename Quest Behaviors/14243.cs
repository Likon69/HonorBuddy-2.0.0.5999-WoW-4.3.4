using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx.Database;
using Styx.Logic.Combat;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Styx.Logic.BehaviorTree;
using Action = TreeSharp.Action;

namespace Styx.Bot.Quest_Behaviors
{
    public class q14243 : CustomForcedBehavior
    {

        public q14243(Dictionary<string, string> args)
            : base(args){}


        public static LocalPlayer me = ObjectManager.Me;
		
		
        #region Overrides of CustomForcedBehavior
        public List<WoWUnit> mobList
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .Where(u => (u.Entry == 36176 && !u.Dead))
                                    .OrderBy(u => u.Distance).ToList();
            }
        }
        
		private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
					
                    new Decorator(ret => (me.QuestLog.GetQuestById(14243) != null && me.QuestLog.GetQuestById(14243).IsCompleted),
                        new Sequence(
                            new Action(ret => TreeRoot.StatusText = "Finished!"),
							new Action(ret => Lua.DoString("VehicleExit()")),
							new Action(ret => Thread.Sleep(15000)),
                            new WaitContinue(120,
                            new Action(delegate
                            {
                                _isDone = true;
                                return RunStatus.Success;
                                }))
                            )),
							
					new Decorator(ret => mobList.Count > 0,
                        new Sequence(
                            new Action(ret => TreeRoot.StatusText = "Bombing - " + mobList[0].Name),
							new Action(ret => Lua.DoString("RunMacroText('/click VehicleMenuBarActionButton1','0')")),
							new Action(ret => LegacySpellManager.ClickRemoteLocation(mobList[0].Location)),
                            new Action(ret => Thread.Sleep(2000))
                        )
					)
                )
			);
        }

        

        
        

        private bool _isDone;
        public override bool IsDone
        {
            get { return _isDone; }
        }

        #endregion
    }
}

