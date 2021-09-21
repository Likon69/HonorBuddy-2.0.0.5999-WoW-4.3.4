using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Styx;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Styx.Bot.Quest_Behaviors.MountHyjal
{
    public class GreaterOfTwoEvils : CustomForcedBehavior
    {
        /// <summary>
        /// GreaterOfTwoEvils by Bobby53
        /// 
        /// Completes the quest http://www.wowhead.com/quest=25310
        /// by using the item to enter a vehicle then casting
        /// its attack and shield abilities as needed to defeat the target
        /// 
        /// Note: you must already be within 100 yds of MobId when starting
        /// 
        /// ##Syntax##
        /// QuestId: Id of the quest (default is 0)
        /// MobId:  Id of the mob to kill
        /// [Optional] QuestName: optional quest name (documentation only)
        /// 
        /// </summary>
        Dictionary<string, object> recognizedAttributes = new Dictionary<string, object>()
        {
            {"QuestId",null},        
            {"MobId",null},          
            {"NpcId",null},          
            {"QuestName",null},             //  (doc only - not used)
        };

        public LocalPlayer Me { get { return ObjectManager.Me; } }

        public GreaterOfTwoEvils(Dictionary<string, string> args)
            : base(args)
        {
            CheckForUnrecognizedAttributes(recognizedAttributes);

            bool error = false;

            uint questId;
            if (!uint.TryParse(Args["QuestId"], out questId))
            {
                Logging.Write("Parsing attribute 'QuestId' in GreaterOfTwoEvils behavior failed! please check your profile!");
                error = true;
            }


            uint mobId;
            string argName = Args.ContainsKey("MobId") ? "MobId" : "NpcId";
            if (!uint.TryParse(Args[argName], out mobId))
            {
                Logging.Write("Parsing attribute '{0}' in GreaterOfTwoEvils behavior failed! please check your profile!", argName );
                error = true;
            }

            if (error)
                TreeRoot.Stop();

            QuestId = questId;
            MobId = mobId;
        }

        public uint QuestId { get; private set; }
        public uint MobId { get; private set; }

        static int lineCount = 0;
        static RunStatus lastStateReturn = RunStatus.Success;

        public static void Log(string msg, params object[] args)
        {
            // following linecount hack is to stop dup suppression of Log window
            Logging.Write(Color.Blue, "[GreaterOfTwoEvils] " + msg + (++lineCount % 2 == 0 ? "" : " "), args);
        }

        public static void DLog(string msg, params object[] args)
        {
            // following linecount hack is to stop dup suppression of Log window
            Logging.Write(Color.Blue, "(GreaterOfTwoEvils) " + msg + (++lineCount % 2 == 0 ? "" : " "), args);
        }

        public bool DoWeHaveQuest()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById(QuestId);
            
            return quest != null;
        }

        public bool IsQuestComplete()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById(QuestId);
            return quest == null || quest.IsCompleted;
        }

        public bool HasAura(WoWUnit unit, int auraId)
        {
            WoWAura aura = (from a in unit.Auras
                            where a.Value.SpellId == auraId
                            select a.Value).FirstOrDefault();
            return aura != null;
        }

        private WoWUnit Target
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                       .Where(u => u.Entry == MobId && !u.Dead )
                                       .OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        private bool _isDone;
        public override bool IsDone
        {
            get
            {
                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById(QuestId);

                return
                    _isDone ||
                    (quest != null && quest.IsCompleted) ||
                    quest == null;
            }
        }

        public override void OnStart()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById(QuestId);

            if (quest != null)
            {
                TreeRoot.GoalText = "Doing: " + quest.Name;
            }
            else
            {
                TreeRoot.GoalText = "GreaterOfTwoEvils - ";
            }

            if (TreeRoot.Current == null)
                Log("ERROR - TreeRoot.Current == null");
            else if (TreeRoot.Current.Root == null )
                Log("ERROR - TreeRoot.Current.Root == null");
            else if (TreeRoot.Current.Root.LastStatus == RunStatus.Running)
                Log("ERROR - TreeRoot.Current.Root.LastStatus == RunStatus.Running");
            else
            {
                var currentRoot = TreeRoot.Current.Root;
                if (!(currentRoot is GroupComposite))
                    Log("ERROR - !(currentRoot is GroupComposite)");
                else 
                {
                    if (currentRoot is Sequence)
                        lastStateReturn = RunStatus.Failure ;
                    else if (currentRoot is PrioritySelector)
                        lastStateReturn = RunStatus.Success;
                    else
                    {
                        DLog("unknown type of Group Composite at root");
                        lastStateReturn = RunStatus.Success;
                    }

                    var root = (GroupComposite)currentRoot;
                    root.InsertChild(0, CreateBehavior());
                }
            }
        }


        private Composite _root;
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new Decorator( ret => !_isDone, 
                    new PrioritySelector(
                        new Decorator(ret => IsQuestComplete(),
                            new PrioritySelector(
                                new Decorator(ret => Me.IsOnTransport,
                                    new Action(delegate 
                                    { 
                                        DLog( "Quest complete - cancelling Flame Ascendancy");
                			Lua.DoString("VehicleExit()");
                                        StyxWoW.SleepForLagDuration();
                                        return RunStatus.Success;
                                    })
                                ),
                                new Action(delegate
                                {
                                    _isDone = true;
                                    StyxWoW.SleepForLagDuration();
                                    return RunStatus.Success;
                                })
                            )
                        ),

                        // loop waiting for target only if no buff
                        new Decorator(ret => Target == null,
                            new Action(delegate
                            {
                                StyxWoW.SleepForLagDuration();
                                return RunStatus.Success;
                            })
                        ),

                        // loop waiting for CurrentTarget only if no buff
                        new Decorator(ret => Target != Me.CurrentTarget,
                            new Action(delegate
                            {
                                WoWUnit target = Target;
                                target.Target();
                                StyxWoW.SleepForLagDuration();
                                return RunStatus.Success;
                            })
                        ),

                        // use item to get buff (enter vehicle)
                        new Decorator(ret => !Me.IsOnTransport,
                            new Action(delegate
                            {
                                WoWItem item = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(i => i != null && i.Entry == 42499);
                                if (item == null)
                                {
                                    Log("ERROR - quest item Talisman of Flame Ascendancy not in inventory");
                                    TreeRoot.Stop();
                                }

                                Log("Use: {0}", item.Name );
                                item.Use(true);
                                StyxWoW.SleepForLagDuration();
                                return RunStatus.Success;
                            })
                        ),

                        new Decorator(ret => Target.Distance > 5,
                            new Action(delegate
                            {
                                DLog("Moving towards target");
                                Navigator.MoveTo(Target.Location);
                                return RunStatus.Success;
                            })
                        ),

                        new Decorator(ret => Target.Distance <= 5 && Me.IsMoving,
                            new Action(delegate
                            {
                                DLog("At target, so stopping");
                                WoWMovement.MoveStop();
                                return RunStatus.Success;
                            })
                        ),

                        new Decorator(ret => !StyxWoW.GlobalCooldown && !Blacklist.Contains(2),
                            new Action(delegate
                            {
                                Log("Cast Flame Shield");
                                Lua.DoString("RunMacroText(\"/click BonusActionButton4\")");
                                Blacklist.Add(2, TimeSpan.FromMilliseconds(8000));
                                return RunStatus.Success;
                            })
                        ),

                        new Decorator(ret => !StyxWoW.GlobalCooldown && !Blacklist.Contains(1),
                            new Action(delegate
                            {
                                Log("Cast Attack");
                                Lua.DoString("RunMacroText(\"/click BonusActionButton1\")");
                                Blacklist.Add(1, TimeSpan.FromMilliseconds(1500));
                                return RunStatus.Success;
                            })
                        ),

                        new Action(delegate
                        {
                            DLog("Waiting for Cooldown");
                            return lastStateReturn;
                        })
                    )
                )
            );
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
