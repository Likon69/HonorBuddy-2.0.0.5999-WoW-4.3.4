using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
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
    public class q25066 : CustomForcedBehavior
	{
		public q25066(Dictionary<string, string> args)
            : base(args){}
    
        
        public static LocalPlayer me = ObjectManager.Me;
		static public bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') or not(GetBonusBarOffset()==0) then return 1 else return 0 end", 0) == 1; } }
		WoWPoint endloc = new WoWPoint(1662.314, 2717.742, 189.7396);
		WoWPoint startloc = new WoWPoint(1782.963, 2884.958, 157.274);
		WoWPoint flyloc = new WoWPoint(1782.963, 2884.958, 157.274);
		
        
		public List<WoWUnit> objmob
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .Where(u => (u.Entry == 39039 && !u.Dead))
                                    .OrderBy(u => u.Distance).ToList();
            }
        }
		public List<WoWUnit> flylist
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .Where(u => (u.Entry == 38387 && !u.Dead))
                                    .OrderBy(u => u.Distance).ToList();
            }
        }
        private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
					
					
                    new Decorator(ret => me.QuestLog.GetQuestById(25066) !=null && me.QuestLog.GetQuestById(25066).IsCompleted,
						new Sequence(
                            new Action(ret => TreeRoot.StatusText = "Finished!"),
                            new WaitContinue(120,
                            new Action(delegate
                            {
                                _isDone = true;
                                return RunStatus.Success;
                            })))),
					new Decorator(ret => !InVehicle,
						new Action(ret =>
						{
							if (flylist.Count == 0)
							{
								Navigator.MoveTo(flyloc);
								Thread.Sleep(1000);
							}
							if (flylist.Count > 0 && flylist[0].Location.Distance(me.Location) > 5)
							{
								Navigator.MoveTo(flylist[0].Location);
								Thread.Sleep(1000);
							}
							if (flylist.Count > 0 && flylist[0].Location.Distance(me.Location) <= 5)
							{
								WoWMovement.MoveStop();
								flylist[0].Interact();
								Thread.Sleep(1000);
								Lua.DoString("SelectGossipOption(1)");
                                Thread.Sleep(1000);
							}
						})),
					new Decorator(ret => InVehicle,
						new Action(ret =>
						{
							if (!InVehicle)
								return RunStatus.Success;
							if (me.QuestLog.GetQuestById(25066).IsCompleted)
							{
								while (me.Location.Distance(endloc) > 10)
								{
									WoWMovement.ClickToMove(endloc);
									Thread.Sleep(1000);
								}
								Lua.DoString("VehicleExit()");
								return RunStatus.Success;
							}
							if (objmob.Count == 0)
							{
								WoWMovement.ClickToMove(startloc);
								Thread.Sleep(1000);
							}
							if (objmob.Count > 0)
							{
								objmob[0].Target();
								WoWMovement.ClickToMove(objmob[0].Location);
								Thread.Sleep(100);
								Lua.DoString("UseAction(122, 'target', 'LeftButton')");
								Lua.DoString("UseAction(121, 'target', 'LeftButton')");
							}
							return RunStatus.Running;
						}
					))
					
					
                )
			);
        }

        

        
        

        private bool _isDone;
        public override bool IsDone
        {
            get { return _isDone; }
        }

    }
}

