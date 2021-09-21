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
using Tripper.Tools.Math;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class Shroud : CustomForcedBehavior
    {
        ~Shroud()
        {
            Dispose(false);
        }

        public Shroud(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = 28367;//GetAttributeAsNullable<int>("QuestId",false, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;
                QuestRequirementComplete = QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = QuestInLogRequirement.InLog;
                MobIds = new uint[] { 50635, 50638, 50643, 50636 };
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
        public int FlightSpot;
        public int State = 4;
        public Pair Target;
        public WoWPoint[] FlightPath  = new WoWPoint[]{new WoWPoint(0,0,0),new WoWPoint(-9065.746, -178.7902, 186.2216)
,new WoWPoint(-8767.904, -47.82138, 186.2216)
,new WoWPoint(-8839.586, 94.10641, 186.2216)
,new WoWPoint(-8977.347, 180.7038, 186.2216)
,new WoWPoint(-9150.972, 66.71052, 186.2216)};

        public Pair[] SafeSpots = new Pair[]
                                      {
                                          new Pair(new WoWPoint(-8865.017, -67.25845, 142.4352),
                                                   new WoWPoint(-8879.103, -27.9558, 141.0528)),
                                          new Pair(new WoWPoint(-8880.18, 100.8362, 142.3729),
                                                   new WoWPoint(-8923.264, 81.26907, 141.0495)),
                                          new Pair(new WoWPoint(-9009.845, 136.4343, 141.2677),
                                                   new WoWPoint(-9020.673, 102.5993, 141.0485)),
                                          new Pair(new WoWPoint(-9055.021, 111.2494, 142.7882),
                                                   new WoWPoint(-9050.942, 78.64673, 141.0491)),
                                          new Pair(new WoWPoint(-9055.021, 111.2494, 142.7882),
                                                   new WoWPoint(-9058.617, 60.17788, 141.0492)),
                                          new Pair(new WoWPoint(-9098.441, -107.1487, 142.0959),
                                                   new WoWPoint(-9074.733, -81.41763, 141.049)),
                                          new Pair(new WoWPoint(-8897.744, -86.20571, 142.4366),
                                                   new WoWPoint(-8927.161, -55.22403, 141.0703)),
                                          new Pair(new WoWPoint(-8867.855, -68.69208, 142.4369),
                                                   new WoWPoint(-8878.626, -28.24805, 141.0537)),
                                          new Pair(new WoWPoint(-8903.402, 116.398, 142.4555),
                                                   new WoWPoint(-8913.154, 105.3958, 141.0488)),
                                          new Pair(new WoWPoint(-8883.854, 100.8373, 141.4563),
                                                   new WoWPoint(-8912.104, 82.27669, 141.0491))
                                      };



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

        public struct Pair
        {
            public WoWPoint LandingSpot,BarrelSpot;
          
            public Pair(WoWPoint l,WoWPoint b)
            {
                LandingSpot = l;
                BarrelSpot = b;
            }
        }

        #region Overrides of CustomForcedBehavior




        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsQuestComplete(), new Action(delegate
                                                                           {
                                                                               
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));

            }
        }

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }

        public WoWUnit Dragon
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == 48444 && u.IsAlive); }
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


        public Composite GetToCurrent
        {
            get
            {
                return
                    new Decorator(r => Me.Location.Distance(FlightPath[Math.Abs(FlightSpot)]) > 1, new Action(r => Flightor.MoveTo(FlightPath[Math.Abs(FlightSpot)])));
            }
        }

        public Composite SetNext
        {
            get { return new Action(r => IncreaseFlight()); }
        }


        public void IncreaseFlight()
        {
            FlightSpot = Math.Abs(FlightSpot);
            if (FlightSpot == 5)
            {
                FlightSpot = 1;
            }
            else
            {
                FlightSpot++;
            }

        }

        public bool ValidSafeSpot()
        {
            foreach (var x in from x in SafeSpots where Me.Location.Distance(x.LandingSpot) < 60 let barrel = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                u => u.Entry == 207127 && u.Location.Distance(x.BarrelSpot) < u.InteractRange) where barrel != null select x)
            {
                Target = x;
                return true;
            }
            return false;
        }


        public Composite CheckSafeSpots
        {
            get
            {
                return
                    new Decorator(r => ValidSafeSpot(), new Action(r => State = 1));
            }
        }


        public Composite MountUp
        {
            get
            {
                return
                    new Decorator(r => !Me.Mounted, new Action(r => Flightor.MountHelper.MountUp()));
            }
        }

        public Composite Circle
        {
            get
            {
                return new Decorator(r=> State == 4,new PrioritySelector(MountUp,CheckSafeSpots,GetToCurrent, SetNext));
            }
        }

        public Composite StateOne
        {
            get
            {
                return new Decorator(r => State == 1 || State == 3, new PrioritySelector(GetThere,StepIncrease));
            }
        }

        public Composite StepIncrease
        {
            get
            {
                return
                    new Decorator(r => Me.Location.Distance(Target.LandingSpot) <= 1, new Action(delegate
                                                                                                     {
                                                                                                         Flightor.MountHelper.Dismount();
                                                                                                         RemoveCloak();
                                                                                                         State++;
                                                                                                     }));
            }
        }


        public WoWItem ShroudItem
        {
            get { return Me.BagItems.FirstOrDefault(x => x.Entry == 63699); }
        }


        public Composite GetCloaked
        {
            get
            {
                return
                    new Decorator(r => !Me.HasAura("Shroud of the Makers"), new Action(r => ShroudItem.Use()));
            }
        }


        public WoWGameObject Barrel
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(
                                                    u =>
                                                    u.Entry == 207127 &&
                                                    u.Location.Distance(Target.BarrelSpot) < u.InteractRange);
            }
        }

        public Composite ClickIt
        {
            get { return new Action(delegate
                                        {
                                            if (Barrel != null)
                                            {
                                                Barrel.Interact();
                                            }

                                        }); }
        }


        public Composite GoBack
        {
            get
            {
                return
                    new Decorator(r => Barrel == null, new Action(r => State = 3));
            }
        }

        public Composite StateTwo
        {
            get
            {
                return new Decorator(r => State == 2 && Dragon != null && Dragon.Distance > 100, new PrioritySelector(Removepet,GoBack,GetCloaked, MoveOut, ClickIt));
            }
        }

        public Composite Removepet
        {
            get
            {
                return
                    new Decorator(r => Me.Pet != null, new Action(r => Lua.DoString("PetDismiss();")));
            }
        }

        
        public Composite StateTwoDragon
        {
            get
            {
                return new Decorator(r => State == 2 && Dragon != null && Dragon.Distance < 100, new PrioritySelector(GetThere));
            }
        }


        public Composite MoveOut
        {
            get
            {
                return
                    new Decorator(r => Me.Location.Distance(Target.BarrelSpot) > 1, new Action(r => WoWMovement.ClickToMove(Target.BarrelSpot)));
            }
        }

        public Composite GetThere
        {
            get
            {
                return
                    new Decorator(r => Me.Location.Distance(Target.LandingSpot) > 1, new Action(r => WoWMovement.ClickToMove(Target.LandingSpot)));
            }
        }


        protected override Composite CreateBehavior()
        {

            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, StateOne, StateTwo, StateTwoDragon, Circle, new ActionAlwaysSucceed())));
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
                return (_isBehaviorDone // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
            }
        }

        public void RemoveCloak()
        {
            var aura = Me.GetAllAuras().FirstOrDefault(x => x.SpellId == 90139);
            if (aura != null)
            {
                Lua.DoString("RunMacroText(\"/cancelaura " + aura.Name + "\")");
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
                if (TreeRoot.Current != null && TreeRoot.Current.Root != null &&
                    TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                //Find the current closest flight spot;
                float distance = float.MaxValue;
                FlightSpot = 0;
                for(int i =0;i<FlightPath.Count();i++)
                {
                    if (Me.Location.Distance(FlightPath[i]) < distance)
                    {
                        distance = Me.Location.Distance(FlightPath[i]);
                        FlightSpot = i;
                    }
                }
                

                

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}