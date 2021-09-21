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
using Styx.Logic.Pathing;
using Styx.Logic.Profiles.Quest;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class UndyingTwilight : CustomForcedBehavior
    {
        ~UndyingTwilight()
        {
            Dispose(false);
        }

        public UndyingTwilight(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = 26875; //GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;
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


        public Composite DoDps
        {
            get
            {
                return
                    new PrioritySelector(
                        new Decorator(ret => RoutineManager.Current.CombatBehavior != null, RoutineManager.Current.CombatBehavior),
                        new Action(c => RoutineManager.Current.Combat()));
            }
        }




        #region Overrides of CustomForcedBehavior

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }


        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsQuestComplete() && !Me.Combat, new Action(delegate
                    {
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));

            }
        }

        public void PullMob()
        {
            string spell = "";

            switch (Me.Class)
            {
                case WoWClass.Mage:
                    if (Me.GotAlivePet)
                        SetPetMode("Passive");

                    spell = "Ice Lance";
                    break;
                case WoWClass.Druid:
                    spell = "Moonfire";
                    break;
                case WoWClass.Paladin:
                    spell = "Judgement";
                    break;
                case WoWClass.Priest:
                    spell = "Shadow Word: Pain";
                    break;
                case WoWClass.Shaman:
                    if (Me.GotAlivePet)
                        SetPetMode("Passive");
                    spell = "Flame Shock";
                    break;
                case WoWClass.Warlock:
                    if (Me.GotAlivePet)
                        SetPetMode("Passive");

                    spell = "Corruption";
                    break;
                case WoWClass.DeathKnight:
                    if (Me.GotAlivePet)
                        SetPetMode("Passive");
                     spell = "Icy Touch";
                     if (!SpellManager.CanCast(spell) && SpellManager.CanCast("Death Coil"))
                         spell = "Death Coil";

                   
                    break;
                case WoWClass.Hunter:
                    if (Me.GotAlivePet)
                        SetPetMode("Passive");

                    spell = "Arcane Shot";
                    break;
                case WoWClass.Warrior:
                    if (SpellManager.CanCast("Shoot"))
                        spell = "Shoot";
                    if (SpellManager.CanCast("Throw"))
                        spell = "Throw";
                    break;
                case WoWClass.Rogue:
                    if (SpellManager.CanCast("Shoot"))
                        spell = "Shoot";
                    if (SpellManager.CanCast("Throw"))
                        spell = "Throw";
                    break;

            }

            if (!String.IsNullOrEmpty(spell) && SpellManager.CanCast(spell))
            {
                SpellManager.Cast(spell);
            }


        }

        public void SetPetMode(string action)
        {

            var spell = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logging.Write(string.Format("[Pet] Casting {0}", action));
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
            
        }

        bool IsObjectiveComplete(int objectiveId, uint questId)
        {
            if (this.Me.QuestLog.GetQuestById(questId) == null)
            {
                return false;
            }
            int returnVal = Lua.GetReturnVal<int>("return GetQuestLogIndexByID(" + questId + ")", 0);
            return Lua.GetReturnVal<bool>(string.Concat(new object[] { "return GetQuestLogLeaderBoard(", objectiveId, ",", returnVal, ")" }), 2);
        }

        public List<WoWUnit> Tagged
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsHostile && u.IsAlive && u.TaggedByMe).OrderBy(u => u.Distance).ToList();
            }
        }

        public WoWUnit Rager
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 44683 && u.IsAlive && u.HealthPercent < 50).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        public WoWUnit RagerTagged
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 44683 && u.IsAlive && u.TaggedByMe).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        public WoWUnit AttackingMe
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.CurrentTarget == Me).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        public List<WoWUnit> NotTagged
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry != 44683 && u.IsHostile && u.IsAlive && !u.TaggedByMe && u.CurrentTarget != null && u.CurrentTarget != Me && u.HealthPercent < 50).OrderBy(u => u.Distance).ToList();
            }
        }


        public Composite PullOne
        {
            get
            {
                return new Action(delegate
                {
                    Navigator.PlayerMover.MoveStop();
                   Rager.Target();
                   Rager.Face();
                    PullMob();
                });
            }
        }

        public Composite PullOther
        {
            get
            {
                return new Action(delegate
                {
                    Navigator.PlayerMover.MoveStop();
                    NotTagged[0].Target();
                    NotTagged[0].Face();
                    PullMob();
                });
            }
        }



        public Composite KillAttacker
        {
            get
            {
                return new Action(delegate
                {
                    Navigator.PlayerMover.MoveStop();
                    AttackingMe.Target();
                    AttackingMe.Face();
                    PullMob();
                });
            }
        }


        public Composite RagerStuff
        {
            get
            {
                return new Decorator(ret => !IsObjectiveComplete(2, (uint)QuestId), new Decorator(r => RagerTagged == null,PullOne));
            }
        }



        public Composite KillAttackers
        {
            get
            {
                return new Decorator(r => Me.Combat && AttackingMe != null, KillAttacker);
            }
        }


        public Composite OtherStuff
        {
            get
            {
                return new Decorator(r => !IsObjectiveComplete(1, (uint)QuestId) && Tagged.Count < 3, PullOther);
            }
        }


        
         //WoWPoint endspot = new WoWPoint(1076.7,455.7638,-44.20478);
       // WoWPoint spot = new WoWPoint(1109.848,462.9017,-45.03053);
        WoWPoint spot = new WoWPoint(1104.14,467.4733,-44.5488);
        
        public Composite StayClose
        {
            get
            {
                return new Sequence( new ActionDebugString("Too far, moving back to location"),new Decorator(r => Me.Location.Distance(spot) > 5, new Action(r=>WoWMovement.ClickToMove(spot))));
            }
        }
  

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet,KillAttackers, StayClose,RagerStuff, OtherStuff, new ActionAlwaysSucceed())));
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


                if (TreeRoot.Current != null && TreeRoot.Current.Root != null && TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                // Me.QuestLog.GetQuestById(27761).GetObjectives()[2].

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }




        }







        #endregion
    }
}