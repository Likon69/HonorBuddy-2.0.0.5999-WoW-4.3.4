// Behavior originally contributed by HighVoltz.
// This behavior is tailored for the quest http://www.wowhead.com/quest=27789/troggish-troubles 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonBehaviors.Actions;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Tripper.Tools.Math;
using Action = TreeSharp.Action;
using System.Globalization;


namespace Styx.Bot.Quest_Behaviors
{
    public class ThisMeansWAR : CustomForcedBehavior
    {
        public ThisMeansWAR(Dictionary<string, string> args)
            : base(args)
        {
        }

        ~ThisMeansWAR()
        {
            Dispose(false);
        }

        private bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') then return 1 else return 0 end", 0) == 1; } }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        readonly WoWPoint _lumberMillLocation = new WoWPoint(2427.133, -1649.115, 104.0841);
        readonly WoWPoint _spiderLocation = new WoWPoint(2332.387, -1694.623, 104.5099);
        private WoWPoint _movetoPoint;
        private WoWUnit _currentTarget;

        readonly List<ulong> _blackList = new List<ulong>();
        private DateTime _stuckTimeStamp = DateTime.Now;
        private WoWPoint _lastMovetoPoint;

        public override bool IsDone
        {
            get
            {
                var quest = Me.QuestLog.GetQuestById(27001);
                return quest != null && quest.IsCompleted;
            }
        }

        public override void OnStart()
        {
            if (!IsDone)
            {
                BotEvents.OnBotStop += BotEvents_OnBotStop;
                Targeting.Instance.RemoveTargetsFilter += Instance_RemoveTargetsFilter;
            }
        }

        private static void Instance_RemoveTargetsFilter(List<WoWObject> units)
        {
            units.Clear();
        }

        public void BotEvents_OnBotStop(EventArgs args)
        {
            Dispose();
        }

        private bool _isDisposed;
        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                BotEvents.OnBotStop -= BotEvents_OnBotStop;
                Targeting.Instance.RemoveTargetsFilter -= Instance_RemoveTargetsFilter;

                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;
                base.Dispose();
            }

            _isDisposed = true;
        }

        private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new PrioritySelector(
                // if not in a turret than move to one and interact with it
                new Decorator(ret => !InVehicle,
                    new Sequence(ctx => GetMustang(), // set Turret as context
                        new DecoratorContinue(ctx => ctx != null && ((WoWUnit)ctx).DistanceSqr > 5 * 5,
                            new Action(ctx =>
                                           {
                                               Navigator.MoveTo(((WoWUnit)ctx).Location);
                                               TreeRoot.StatusText = "Moving To Mustang";
                                           })),
                        new DecoratorContinue(ctx => ctx != null && ((WoWUnit)ctx).DistanceSqr <= 5 * 5,
                            new Action(ctx =>
                                           {
                                               Logging.Write("Interacting with Mustang");
                                               if (Me.Mounted)
                                                   Mount.Dismount();
                                               ((WoWUnit)ctx).Interact();
                                           })))),
                // Find the nearest spider and if none exist then move to thier spawn location
                    new Decorator(ret => _currentTarget == null || !_currentTarget.IsValid || !_currentTarget.IsAlive,
                            new Action(ctx =>
                                           {
                                               _currentTarget = ObjectManager.GetObjectsOfType<WoWUnit>()
                                                   .Where(
                                                       u =>
                                                       u.IsAlive && !_blackList.Contains(u.Guid) && u.Entry == 44284).
                                                   OrderBy(u => u.DistanceSqr).FirstOrDefault();
                                               if (_currentTarget == null)
                                               {
                                                   Navigator.MoveTo(_spiderLocation);
                                                   Logging.Write("No spiders found. Moving to spawn point");
                                               }
                                               else
                                               {
                                                   _movetoPoint = WoWMathHelper.CalculatePointFrom(_lumberMillLocation,
                                                                                                   _currentTarget.
                                                                                                       Location, -5);
                                                   Logging.Write("Locked on a new target. Distance {0}", _currentTarget.Distance);
                                               }
                                           })),


                            new Sequence(
                                new Action(ctx => TreeRoot.StatusText = "Scaring spider towards lumber mill"),
                                new Action(ctx =>
                                               { // blacklist spider if it doesn't move
                                                   if (DateTime.Now - _stuckTimeStamp > TimeSpan.FromSeconds(6))
                                                   {
                                                       _stuckTimeStamp = DateTime.Now;
                                                       if (_movetoPoint.DistanceSqr(_lastMovetoPoint) < 3 * 3)
                                                       {
                                                           Logging.Write("Blacklisting spider");
                                                           _blackList.Add(_currentTarget.Guid);
                                                           _currentTarget = null;
                                                           return RunStatus.Failure;
                                                       }
                                                       _lastMovetoPoint = _movetoPoint;
                                                   }
                                                   return RunStatus.Success;
                                               }),
                                new Action(ctx =>
                                                {
                                                    // update movepoint
                                                    _movetoPoint =
                                                        WoWMathHelper.CalculatePointFrom(_lumberMillLocation,
                                                                                        _currentTarget.
                                                                                            Location, -6);
                                                    if (_movetoPoint.DistanceSqr(Me.Location) >4 * 4)
                                                    {
                                                        Navigator.MoveTo(_movetoPoint);
                                                        return RunStatus.Running;
                                                    }
                                                    return RunStatus.Success;
                                                }),
                                new WaitContinue(2, ret => !Me.IsMoving, new ActionAlwaysSucceed()),
                                new Action(ctx =>
                                               {
                                                   using (new FrameLock())
                                                   {
                                                       Me.SetFacing(_lumberMillLocation);
                                                       WoWMovement.Move(WoWMovement.MovementDirection.ForwardBackMovement);
                                                       WoWMovement.MoveStop(WoWMovement.MovementDirection.ForwardBackMovement);
                                                       //Lua.DoString("CastSpellByID(83605)");
                                                   }
                                               }),
                            new WaitContinue(TimeSpan.FromMilliseconds(200), ret => false, new ActionAlwaysSucceed()),
                             new Action(ctx => Lua.DoString("CastSpellByID(83605)"))

                /*
                              new Action(ctx =>
                               {
                                   var unit = ctx as WoWUnit;
                                   if (unit != null && unit.IsValid && unit.IsAlive)
                                   {
                                       // move to a point that places the spider between player and lumber mill
                                       var movetoPoint = WoWMathHelper.CalculatePointFrom(_lumberMillLocation, unit.Location, -5);
                                       // blacklist spider if its not moving
                                       if (DateTime.Now - _stuckTimeStamp > TimeSpan.FromSeconds(6))
                                       {
                                           _stuckTimeStamp = DateTime.Now;
                                           if (movetoPoint.DistanceSqr(_lastMovetoPoint) < 2 * 2)
                                           {
                                               Logging.Write("Blacklisting spider");
                                               _blackList.Add(unit.Guid);
                                               return RunStatus.Failure;
                                           }
                                           _lastMovetoPoint = movetoPoint;
                                       }

                                       if (movetoPoint.DistanceSqr(Me.Location) > 6 * 6)
                                           Navigator.MoveTo(movetoPoint);
                                       else
                                       {
                                           using (new FrameLock())
                                           {
                                               //WoWMovement.MoveStop();
                                               //Me.SetFacing(_lumberMillLocation);
                                               Lua.DoString("CastSpellByID(83605)");
                                           }
                                       }
                                       return RunStatus.Running;
                                   }
                                   return RunStatus.Failure;
                               })*/
                                 )));
        }


        WoWUnit GetMustang()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.CharmedByUnitGuid == 0 || u.CharmedByUnitGuid == Me.Guid) && u.Entry == 44836)
                .OrderBy(u => u.DistanceSqr).
                FirstOrDefault();
        }
    }
}
