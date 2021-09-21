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
    public class FireFromSky : CustomForcedBehavior
    {
        ~FireFromSky()
        {
            Dispose(false);
        }

        public FireFromSky(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                //Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??WoWPoint.Empty;
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
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
                        new Decorator(ret => RoutineManager.Current.CombatBehavior != null,
                                      RoutineManager.Current.CombatBehavior),
                        new Action(c => RoutineManager.Current.Combat()));
            }
        }

        #region Overrides of CustomForcedBehavior

        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint) QuestId);
            return quest == null || quest.IsCompleted;
        }


        public Composite DoneYet
        {
            get
            {
                return new Decorator(ret => IsQuestComplete(), new Action(delegate
                                                                                           {
                                                                                                TreeRoot.StatusText =
                                                                                                    "Finished!";
                                                                                                _isBehaviorDone = true;
                                                                                                return RunStatus.Success;
                                                                                            }));
            }
        }


        public int UnfriendlyUnitsNearTarget(float distance, WoWUnit who)
        {
            var dist = distance * distance;
            var curTarLocation = who.Location;
            return ObjectManager.GetObjectsOfType<WoWUnit>().Count(p =>  p.IsAlive && (p.Entry == 48713) && p.Location.DistanceSqr(curTarLocation) <= dist);
			// (p.Entry == 48720 || p.Entry == 48713) changed to (p.Entry == 48720)
        }
 
        public List<WoWUnit> Enemies
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsAlive && (u.Entry == 48713)).OrderByDescending(z => UnfriendlyUnitsNearTarget(20, z)).ToList();
					// (u.Entry == 48720 || u.Entry == 48713) changed to (u.Entry == 48720)

            }
        }


        public Composite ShootStuff
        {
            get
            {
                return new Decorator(ret => Me.PetSpells[0].Cooldown == false, Fire);
            }
        }

		/*
        protected WoWPoint getEstimatedPosition(WoWUnit who,double time)
        {
            var targetVelocity = 1.50;
            var targetStartingPoint = who.MovementInfo.Position;
            double x = targetStartingPoint.X +
               targetVelocity * time * who.MovementInfo.DirectionSinX;//Math.Sin(who.Rotation);
            double y = targetStartingPoint.Y +
               targetVelocity * time * who.MovementInfo.DirectionCosY;//Math.Cos(who.Rotation);
            return new WoWPoint(x, y,who.Z);
        }
		*/

        public Composite Fire
        {
            get
            {
                return new Action(delegate
                                      {
                                          ObjectManager.Update();
                                          Lua.DoString("CastPetAction(1);");
                                          //LegacySpellManager.ClickRemoteLocation(getEstimatedPosition(Enemies[0],7));

                                           if (Enemies[0].Z <= 285)
                                         {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 15));
                                          }
										  /*
										  // bottom left
                                          if ((Enemies[0].Z >= 200) && (Enemies[0].Z <= 213))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 11));
                                          }
										  
										  // middle left
                                          if ((Enemies[0].Z >= 219) && (Enemies[0].Z <= 228))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 11));
                                          }
										  // middle flat - i should change this to 2x x/y's for vector changes
										  else if ((Enemies[0].Z >= 235) && (Enemies[0].Z <= 240))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 16));
                                          }
										  //
										  /* this doesn't quite work the way i want it to
										  // Middle Flat - Left
										  else if (((Enemies[0].Z >= 236) && (Enemies[0].Z <= 240)) && ((Enemies[0].Y >= -57) && (Enemies[0].Y <= -44)) && ((Enemies[0].X >= -8534) && (Enemies[0].X <= -8515)))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 16));
                                          }
										  // Middle Flat - Right
										  else if (((Enemies[0].Z >= 236) && (Enemies[0].Z <= 240)) && ((Enemies[0].Y >= -40) && (Enemies[0].Y <= -15)) && ((Enemies[0].X >= -8509) && (Enemies[0].X <= -8507)))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 16));
                                          }
										  /
										  // middle right
                                          else if ((Enemies[0].Z >= 244) && (Enemies[0].Z <= 253))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 12));
                                          }
										  // middle top right
                                          else if ((Enemies[0].Z >= 262) && (Enemies[0].Z <= 272))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 11));
                                          }
										  // top right
                                          else if ((Enemies[0].Z >= 282) && (Enemies[0].Z <= 292))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 16));
                                          }
										  /* top
                                          else if ((Enemies[0].Z >= 292) && (Enemies[0].Z <= 307))
                                          {
                                              LegacySpellManager.ClickRemoteLocation(Enemies[0].Location.RayCast(Enemies[0].Rotation, 22));
                                          }
										  */
										  
                                          Logging.Write(UnfriendlyUnitsNearTarget(20,Enemies[0]).ToString());
                                      });
            }
        }
 
 
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet,ShootStuff)));
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
                        var root = (GroupComposite) currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint) QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " +
                                    ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}