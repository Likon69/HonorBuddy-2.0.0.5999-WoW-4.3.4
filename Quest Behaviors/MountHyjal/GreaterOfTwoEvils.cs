// Behavior originally contributed by Bobby53.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
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
        /// </summary>
        /// 
        public GreaterOfTwoEvils(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                /* */
                GetAttributeAs<string>("QuestName", false, ConstrainAs.StringNonEmpty, null);      //  (doc only - not used)
                MobId = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, new[] { "NpcId" }) ?? 0;
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller
        public int MobId { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private static RunStatus _lastStateReturn = RunStatus.Success;
        private static int _lineCount = 0;
        private Composite _root;

        // Private properties
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: GreaterOfTwoEvils.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~GreaterOfTwoEvils()
        {
            Dispose(false);
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


        public void Log(string format, params object[] args)
        {
            // following linecount hack is to stop dup suppression of Log window
            LogMessage("info", format + (++_lineCount % 2 == 0 ? "" : " "), args);
        }

        public void DLog(string format, params object[] args)
        {
            // following linecount hack is to stop dup suppression of Log window
            LogMessage("debug", format + (++_lineCount % 2 == 0 ? "" : " "), args);
        }

        public bool DoWeHaveQuest()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

            return quest != null;
        }

        public bool IsQuestComplete()
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
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
                                       .Where(u => u.Entry == MobId && !u.Dead)
                                       .OrderBy(u => u.Distance).FirstOrDefault();
            }
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new Decorator(ret => !_isBehaviorDone,
                    new PrioritySelector(
                        new Decorator(ret => IsQuestComplete(),
                            new PrioritySelector(
                                new Decorator(ret => Me.HasAura("Flame Ascendancy"),
                                    new Action(delegate
                                    {
                                        DLog("Quest complete - cancelling Flame Ascendancy");
                                        Lua.DoString("RunMacroText(\"/cancelaura Flame Ascendancy\")");
                                        StyxWoW.SleepForLagDuration();
                                        return RunStatus.Success;
                                    })
                                ),
                                new Action(delegate
                                {
                                    _isBehaviorDone = true;
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
                        new Decorator(ret => !Me.HasAura("Flame Ascendancy"),
                            new Action(delegate
                            {
                                WoWItem item = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(i => i != null && i.Entry == 54814);
                                if (item == null)
                                {
                                    LogMessage("fatal", "Quest item \"Talisman of Flame Ascendancy\" not in inventory.");
                                    TreeRoot.Stop();
                                }

                                Log("Use: {0}", item.Name);
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

                        new Decorator(ret => !StyxWoW.GlobalCooldown && !Blacklist.Contains(2) && !Me.Auras.ContainsKey("Flame Shield"),
                            new Action(delegate
                            {
                                Log("Cast Flame Shield");
                                Lua.DoString("RunMacroText(\"/click BonusActionButton2\")");
                                Blacklist.Add(2, TimeSpan.FromMilliseconds(6000));
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
                            return _lastStateReturn;
                        })
                    )
                )
            );
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get
            {
                return (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
            }
        }


        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                if (TreeRoot.Current == null)
                    LogMessage("fatal", "TreeRoot.Current == null");
                else if (TreeRoot.Current.Root == null)
                    LogMessage("fatal", "TreeRoot.Current.Root == null");
                else if (TreeRoot.Current.Root.LastStatus == RunStatus.Running)
                    LogMessage("fatal", "TreeRoot.Current.Root.LastStatus == RunStatus.Running");
                else
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (!(currentRoot is GroupComposite))
                        LogMessage("fatal", "!(currentRoot is GroupComposite)");
                    else
                    {
                        if (currentRoot is Sequence)
                            _lastStateReturn = RunStatus.Failure;
                        else if (currentRoot is PrioritySelector)
                            _lastStateReturn = RunStatus.Success;
                        else
                        {
                            DLog("Unknown type of Group Composite at root");
                            _lastStateReturn = RunStatus.Success;
                        }

                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
