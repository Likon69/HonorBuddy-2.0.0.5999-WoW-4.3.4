// Behavior originally contributed by mastahg.
//
// DOCUMENTATION:
//     
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonBehaviors.Actions;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class GetKraken : CustomForcedBehavior
    {
        ~GetKraken()
        {
            Dispose(false);
        }

        public GetKraken(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = 14108;//GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;

                //Enemy = GetAttributeAsArray<uint>("Enemys", false, new ConstrainTo.Domain<uint>(0, 100000), new[] { "Enemy" }, null);
                //EnemyDebuff = GetAttributeAsArray<uint>("EnemysDebuff", false, new ConstrainTo.Domain<uint>(0, 100000), new[] { "EnemyDebuff" }, null);
                QuestRequirementComplete = QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = QuestInLogRequirement.InLog;


            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error",
                           "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message + "\nFROM HERE:\n" + except.StackTrace +
                           "\n");
                IsAttributeProblem = true;
            }
        }



        WoWItem Spear()
        {
            return StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 46954);
        }


        // Attributes provided by caller
        public uint[] MobIds { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint Location { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;



        // Private properties
        private LocalPlayer Me
        {
            get { return (ObjectManager.Me); }
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






        #region Overrides of CustomForcedBehavior

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
                    new Decorator(r => IsQuestComplete(), new Action(delegate
                                                                                                {

                                                                                                    TreeRoot.StatusText = "Finished!";
                                                                                                    _isBehaviorDone = true;
                                                                                                    return RunStatus.Success;
                                                                                                }));

            }
        }



        public void UsePetSkill(string action)
        {

            var spell = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;
            Logging.Write(string.Format("[Pet] Casting {0}", action));
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);

        }


        //WoWPoint endspot = new WoWPoint(1076.7,455.7638,-44.20478);
        // WoWPoint spot = new WoWPoint(1109.848,462.9017,-45.03053);
        //WoWPoint MountSpot = new WoWPoint(8426.872,711.7554,547.294);





        //33429 = lt.
        //33438 = boneguard
        //34127 = commander


        WoWUnit Deepcaller
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(x => x.Entry == 35092 && x.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        WoWUnit Mount
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(x => x.Entry == 35117 && x.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        WoWUnit Kraken
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(x => x.Entry == 34925 && x.IsAlive).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }


        public Composite Part1
        {
            get
            {
                return new Decorator(r => !IsObjectiveComplete(2, (uint)QuestId) && Deepcaller != null, new Action(r =>
                                                                                             {
                                                                                                 Deepcaller.Target();
                                                                                                 Spear().Use();
                                                                                             }));
            }
        }

        public Composite Part2
        {
            get
            {
                return new Decorator(r => !IsObjectiveComplete(1, (uint)QuestId) && Kraken != null, new Action(r =>
                                                                                                 {
                                                                                                     Kraken.Target();
                                                                                                     Spear().Use();
                                                                                                 }));
            }
        }

        Dictionary<uint, uint> Debuffs = new Dictionary<uint, uint>();



        protected override Composite CreateBehavior()
        {
            return _root ??
                   (_root =
                    new Decorator(ret => !_isBehaviorDone,
                                  new PrioritySelector(DoneYet, Part1, Part2, new ActionAlwaysSucceed())));
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

            Logging.Write("Quest Behavior made by mastahg.");
            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {



                /*if (TreeRoot.Current != null && TreeRoot.Current.Root != null && TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }*/

                // Me.QuestLog.GetQuestById(27761).GetObjectives()[2].

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");


                while (!IsQuestComplete())
                {
                    ObjectManager.Update();
                    if(!Me.IsOnTransport && Mount != null)
                    {
                        Mount.Interact();
                        GossipFrame.Instance.SelectGossipOption(0);
                    }
                    else if (!IsObjectiveComplete(1, (uint)QuestId) && Kraken != null)
                    {
                        Kraken.Target();
                        Spear().Use();
                    }
                    else if (!IsObjectiveComplete(2, (uint)QuestId) && Deepcaller != null)
                    {
                        Deepcaller.Target();
                        Spear().Use();
                    }

                }
                _isBehaviorDone = true;


            }




        }







        #endregion
    }
}