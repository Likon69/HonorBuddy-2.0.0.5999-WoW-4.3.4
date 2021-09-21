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
    public class TroggishTroubles : CustomForcedBehavior
    {
        public TroggishTroubles(Dictionary<string, string> args)
            : base(args)
        {
        }

        ~TroggishTroubles()
        {
            Dispose(false);
        }

        private bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') then return 1 else return 0 end", 0) == 1; } }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        public override bool IsDone
        {
            get
            {
                var quest = Me.QuestLog.GetQuestById(27789);
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
        // This is really ugly but due to pulse taking too long to tick this is the only path I can take.
        public override void OnTick()
        {
            Logging.Write("Let the trog genocide commence");
            while (!IsDone)
            {
                try
                {
                    WoWPulsator.Pulse(PulseFlags.Objects);
                    if (!InVehicle)
                    {
                        var turret = GetTurret();
                        if (turret != null)
                        {
                            if (turret.DistanceSqr > 5 * 5)
                                Navigator.MoveTo(turret.Location);
                            else
                                turret.Interact();
                        }
                        else
                        {
                            Logging.Write("Unable to find turret");
                        }
                    }
                    // if 5 or mobs are within 20 units of turret then use Power Burst ability
                    if (MobCountAtLocation(Me.Location, 20, 46711, 46712) >= 5)
                        Lua.DoString("if GetPetActionCooldown(2) == 0 then CastPetAction(2) end");
                    var target = GetBestTarget();
                    if (target != null)
                    {
                        WoWMovement.ClickToMove(target.Location);
                        Lua.DoString("if GetPetActionCooldown(1) == 0 then CastPetAction(1) end");
                    }
                }
                catch (Exception ex)
                {
                    Logging.Write(ex.ToString());
                }
            }
        }

        /*
        private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new PrioritySelector(
                // if not in a turret than move to one and interact with it
                new Decorator(ret => !InVehicle,
                    new Sequence(ctx => GetTurret(), // set Turret as context
                        new DecoratorContinue(ctx => ctx != null && ((WoWUnit)ctx).DistanceSqr > 5 * 5,
                            new Action(ctx =>
                                           {
                                               Navigator.MoveTo(((WoWUnit) ctx).Location);
                                               TreeRoot.StatusText = "Moving To Turret";
                                           })),
                        new DecoratorContinue(ctx => ctx != null && ((WoWUnit)ctx).DistanceSqr <= 5 * 5,
                            new Action(ctx =>
                                           {
                                               Logging.Write("Interacting with turret");
                                               ((WoWUnit) ctx).Interact();
                                           })))),

                // use Power Burst ability if 5 or more mobs are within 10 units of bot.
                new Decorator(ctx => MobCountAtLocation(Me.Location, 10, 46711, 46712) >= 5,
                    new Sequence(
                        new Action(ctx => Logging.Write("Using Power Burst")),
                        new Action(ctx => Lua.GetReturnVal<int>("if GetPetActionCooldown(2) == 0 then CastPetAction(2) return 1 else return 0 end", 0) == 1 ? RunStatus.Success : RunStatus.Failure))),
                new Sequence(ctx => BestTarget,
                    new Action(ctx => TreeRoot.StatusText = "Let the trog genocide commence"),
                    new Action(ctx =>
                    {
                        if (ctx != null)
                        {
                            WoWMovement.ClickToMove(((WoWUnit)ctx).Location);
                            Lua.DoString("CastPetAction(1)");
                            return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    })//,
                    //new WaitContinue(1, ctx => ctx == null, new ActionAlwaysSucceed())
                )));
        }
        */

        WoWUnit GetBestTarget()
        {
            using (new FrameLock())
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                    .Where(u => u.IsAlive && (u.Entry == 46711 || u.Entry == 46712))
                    .OrderByDescending(u => MobCountAtLocation(u.Location, 10, 46711, 46712))
                    .ThenBy(u => u.DistanceSqr)
                    .FirstOrDefault();
            }
        }

        int MobCountAtLocation(WoWPoint point, float radius, params uint[] mobIds)
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Count(u => u.IsAlive && mobIds.Contains(u.Entry) && u.DistanceSqr <= radius * radius);
        }

        WoWUnit GetTurret()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.CharmedByUnitGuid == 0 || u.CharmedByUnitGuid == Me.Guid) && u.Entry == 46707)
                .OrderBy(u => u.DistanceSqr).
                FirstOrDefault();
        }
    }
}