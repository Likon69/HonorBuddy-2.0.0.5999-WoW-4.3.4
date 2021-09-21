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
    public class q25251 : CustomForcedBehavior
	{
		public q25251(Dictionary<string, string> args)
            : base(args){}
    
        
        public static LocalPlayer me = ObjectManager.Me;
		static public bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') or not(GetBonusBarOffset()==0) then return 1 else return 0 end", 0) == 1; } }
		static public bool OnCooldown1 { get { return Lua.GetReturnVal<int>("a,b,c=GetActionCooldown(121);if b==0 then return 1 else return 0 end", 0) == 0; } }
		static public bool OnCooldown2 { get { return Lua.GetReturnVal<int>("a,b,c=GetActionCooldown(122);if b==0 then return 1 else return 0 end", 0) == 0; } }
		static public bool OnCooldown3 { get { return Lua.GetReturnVal<int>("a,b,c=GetActionCooldown(123);if b==0 then return 1 else return 0 end", 0) == 0; } }
		WoWPoint startloc = new WoWPoint(2298.823, 2433.5, 26.45126);
		WoWPoint flyloc = new WoWPoint(2120.643, 2402.012, 49.6927);
		WoWPoint temploc = new WoWPoint(2400.707, 2532.421, 4.890985);
        private bool locreached;
		public List<WoWUnit> objmob
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .Where(u => (u.Entry == 39582 && !u.Dead))
                                    .OrderBy(u => u.Distance).ToList();
            }
        }
		public List<WoWUnit> flylist
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                    .Where(u => (u.Entry == 39592 && !u.Dead))
                                    .OrderBy(u => u.Distance).ToList();
            }
        }
        private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
					
					
                    new Decorator(ret => me.QuestLog.GetQuestById(25251) !=null && me.QuestLog.GetQuestById(25251).IsCompleted,
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
							}
						})),
					new Decorator(ret => InVehicle,
						new Action(ret =>
						{
							if (!InVehicle)
								return RunStatus.Success;
							if (me.QuestLog.GetQuestById(25251).IsCompleted)
							{
								while (me.Location.Distance(flyloc) > 10)
								{
									Navigator.MoveTo(flyloc);
									Thread.Sleep(1000);
								}
								Lua.DoString("VehicleExit()");
								return RunStatus.Success;
							}
							if (objmob.Count == 0)
							{
								if (me.Location.Distance(temploc) <= 7)
									locreached = true;
								if (!locreached)
								{
									Navigator.MoveTo(temploc);
									Thread.Sleep(1000);
								}
								else
								Navigator.MoveTo(startloc);
								Thread.Sleep(1000);
							}
							if (objmob.Count > 0 && (objmob[0].Location.Distance(me.Location) > 40 || !objmob[0].InLineOfSight))
							{
								
								objmob[0].Target();
								if (me.Location.Distance(temploc) <= 7)
									locreached = true;
								if (!locreached)
								{
									Navigator.MoveTo(temploc);
									Thread.Sleep(1000);
								}
								if (locreached)
								{
									Navigator.MoveTo(objmob[0].Location);
									Thread.Sleep(1000);
								}
								Thread.Sleep(1000);
							}
							if (objmob.Count > 0 && objmob[0].Location.Distance(me.Location) <= 40 && objmob[0].InLineOfSight)
							{
								WoWMovement.Move(WoWMovement.MovementDirection.Backwards);
								WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards);
								objmob[0].Target();
								objmob[0].Face();
								if (!OnCooldown3)
								Lua.DoString("UseAction(123, 'target', 'LeftButton')");
								if (!OnCooldown2)
								Lua.DoString("UseAction(122, 'target', 'LeftButton')");
								if (!OnCooldown1 && objmob[0].Location.Distance(me.Location) <= 10)
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

