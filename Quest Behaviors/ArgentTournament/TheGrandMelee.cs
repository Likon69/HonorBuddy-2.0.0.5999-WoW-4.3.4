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
    public class TheGrandMelee : CustomForcedBehavior
    {
        ~TheGrandMelee()
        {
            Dispose(false);
        }

        public TheGrandMelee(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ??MountSpot;
                QuestId = GetAttributeAsNullable<int>("QuestId", true, ConstrainAs.QuestId(this), null) ?? 0;
                //MobIds = GetAttributeAsNullable<int>("MobId", true, ConstrainAs.MobId, null) ?? 0;

                Enemy = GetAttributeAsArray<uint>("Enemys", false, new ConstrainTo.Domain<uint>(0, 100000), new[] { "Enemy" }, null);
                EnemyDebuff = GetAttributeAsArray<uint>("EnemysDebuff", false, new ConstrainTo.Domain<uint>(0, 100000), new[] { "EnemyDebuff" }, null);
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

        uint[] Mounts = new uint[]{33798,33796,33799,33842,33791};
        private uint[] Enemy;// = new uint[] { 33384, 33306,33285,33382,33383};
        private uint[] EnemyDebuff;// = new uint[] { 64816, 64811, 64812, 64813, 64815 };

        WoWItem HordeLance()
        {
            return StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 46070);
        }
        WoWItem ArgentLance()
        {
            return StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 46106);
        }

        WoWItem BestLance()
        {
            return HordeLance() ?? ArgentLance();
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


        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsQuestComplete() && !Me.Combat, new Action(delegate
                    {

                        Lua.DoString(
    "RunMacroText(\"/leavevehicle\")");

                        mainhand.UseContainerItem();
                        if (offhand != null)
                        {
                            offhand.UseContainerItem();
                        }

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

        WoWUnit Mount
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(
                        x => Mounts.Contains(x.Entry) && x.NpcFlags == 16777216);
            }
        }

        //WoWPoint endspot = new WoWPoint(1076.7,455.7638,-44.20478);
        // WoWPoint spot = new WoWPoint(1109.848,462.9017,-45.03053);
        WoWPoint MountSpot = new WoWPoint(8426.872,711.7554,547.294);
        
        Composite GetNearMounts
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(r => Me.Location.Distance(Location) > 15, new Action(r => Navigator.MoveTo(Location))),
                     new Decorator(r => Me.Location.Distance(Location) < 15, new Action(r => Mount.Interact()))   
                        
                        
                        );
            }
        }


        Composite MountUp
        {
            get
            {
                return new Decorator(r=>!Me.IsOnTransport,GetNearMounts);
            }
        }

        WoWUnit MyMount
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(x => x.CreatedByUnitGuid == Me.Guid); }
        }



        WoWUnit WhichNPC
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(x => Debuffs.ContainsKey(x.Entry) && !Me.HasAura((int)Debuffs[x.Entry])).OrderBy(u=>u.Distance).FirstOrDefault();
            }
        }

        Composite BuffUp
        {
            get
            {
                return new Decorator(r =>!Me.Combat && (!MyMount.ActiveAuras.ContainsKey("Defend") || ( MyMount.ActiveAuras.ContainsKey("Defend") && MyMount.ActiveAuras["Defend"].StackCount < 3)), new Action(r=>UsePetSkill("Defend")));
            }
        }


        private Composite BarkNpc
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(r => !Me.Combat && WhichNPC == null, new Action(r => Navigator.MoveTo(Location))),
                    new Decorator(r=>!Me.Combat && Me.Location.Distance(WhichNPC.Location)> 5, new Action(r=>Navigator.MoveTo(WhichNPC.Location))),
                    new Decorator(r=>!Me.Combat && Me.Location.Distance(WhichNPC.Location)<= 5, new Action(r=>
                                                                                                               {
                                                                                                                   WhichNPC.Interact();
                                                                                                                   WhichNPC.Target();
                                                                                                                   Thread.Sleep(1000);
                          Lua.DoString("SelectGossipOption(1)");
                                                                                                               }
                        ))
                    
                    
                    
                    );
            }
        }




        private Composite Fight
        {
            get
            {
                return
                    new Decorator(r => Me.Combat, new Action(r =>
                                                                 {


                                                                     //Me.CurrentTarget.Face();
                                                                     //if (Me.CurrentTarget.Distance > 10)
                                                                     //   Navigator.MoveTo(Me.CurrentTarget.Location);

                                                                     //var moveTo = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, StyxWoW.Me.CurrentTarget.Location, -15f);
                                                                     //var moveTo = WoWMathHelper.CalculatePointBehind(StyxWoW.Me.CurrentTarget.Location,Me.CurrentTarget.Rotation, -15f);
                                                                     var moveTo = WoWMathHelper.CalculatePointAtSide(StyxWoW.Me.CurrentTarget.Location, Me.CurrentTarget.Rotation, 20f,false);
                                                                     //var moveTo = WoWMathHelper.GetPointAt(Me.Location, 20,Me.Rotation,0);
                                                                     if (Navigator.CanNavigateFully(StyxWoW.Me.Location, moveTo))
                                                                     {
                                                                        
                                                                         Navigator.MoveTo(moveTo);
                                                                     }
                                                                    /* if (Me.CurrentTarget.Distance < 10)
                                                                     {
                                                                         Navigator.PlayerMover.Move(
                                                                             WoWMovement.MovementDirection.Backwards);
                                                                     }
                                                                     else
                                                                     {
                                                                         Navigator.PlayerMover.MoveStop();
                                                                     }*/


                                                                     if (!MyMount.ActiveAuras.ContainsKey("Defend") || (MyMount.ActiveAuras.ContainsKey("Defend") && MyMount.ActiveAuras["Defend"].StackCount < 3))
                                                                     {
                                                                         //Me.CurrentTarget.Face();
                                                                         UsePetSkill("Defend");
                                                                         UsePetSkill("Charge");
                                                                     }
                                                                     else
                                                                     {
                                                                         if (Me.CurrentTarget.Distance > 8)
                                                                                Me.CurrentTarget.Face();
                                                                         using (new FrameLock())
                                                                         {
                                                                             UsePetSkill("Thrust");
                                                                             UsePetSkill("Charge");
                                                                             UsePetSkill("Shield-Breaker");
                                                                         }
                                                                     }
                                                                 }
                        ))



                    ;
            }
        }

        Dictionary<uint,uint> Debuffs = new Dictionary<uint, uint>();
         
        Composite LanceUp
        {
            get
            {
                return new Decorator(r => Me.Inventory.Equipped.MainHand.ItemInfo.Id != 46106 && Me.Inventory.Equipped.MainHand.ItemInfo.Id != 46070, new Action(r => BestLance().UseContainerItem()));
            }
        }

        Composite HealUp
        {
            get
            {
                return new Decorator(r => !Me.Combat && MyMount.HealthPercent < 50, new Action(r => UsePetSkill("Refresh Mount")));
            }
        }

        protected override Composite CreateBehavior()
        {
            return _root ??
                   (_root =
                    new Decorator(ret => !_isBehaviorDone,
                                  new PrioritySelector(DoneYet, LanceUp, MountUp, BuffUp, HealUp,BarkNpc,Fight, new ActionAlwaysSucceed())));
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
        private WoWItem mainhand;
        private WoWItem offhand;

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


                mainhand = Me.Inventory.Equipped.MainHand;

                if (mainhand.ItemInfo.EquipSlot != InventoryType.TwoHandWeapon)
                {
                    offhand = Me.Inventory.Equipped.OffHand;
                }


                for (int i = 0; i < Enemy.Count(); i++)
                {
                    Debuffs.Add(Enemy[i], EnemyDebuff[i]);
                }
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