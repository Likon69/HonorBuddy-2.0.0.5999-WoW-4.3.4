using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using Druid_ID = Hera.ClassHelpers.Druid.IDs;

namespace Hera
{
    public partial class Codemplosion
    {
        #region Combat!
        private Composite _combatBehavior;
        public override Composite CombatBehavior
        {
            get { if (_combatBehavior == null) { Utils.Log("Creating 'Combat' behavior"); _combatBehavior = CreateCombatBehavior(); } return _combatBehavior; }
        }
        
         private PrioritySelector CreateCombatBehavior()
         {
             return new PrioritySelector(
                 // Check if we get aggro during the pull
                 // This is in here and not the pull because we are in combat at this point
                 new NeedToCheckAggroOnPull(new CheckAggroOnPull()),

                 // Retarget
                 new NeedToRetarget(new Retarget()),

                 // Abort combat is the target's health is 95% + after 30 seconds of combat
                 new NeedToCheckCombatTimer(new CheckCombatTimer()),

                 // Innervate Balance
                 new NeedToInnervateBalance(new InnervateBalance()),

                // Shapeshift Bear
                new NeedToShapeshiftBear(new ShapeshiftBear()),

                // Shapeshift Moonkin
                new NeedToShapeshiftMoonkin(new ShapeshiftMoonkin()),

                // Shapeshift Cat
                new NeedToShapeshiftCat(new ShapeshiftCat()),

                // Restoration spec - special
                new Switch<ClassHelpers.Druid.ClassType>(ret => ClassHelpers.Druid.ClassSpec, new SwitchArgument<ClassHelpers.Druid.ClassType>(RestorationCombat, ClassHelpers.Druid.ClassType.Restoration)),

                // If you are Restoration spec it should not go past this point
                new Decorator(ret => ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Restoration,new AlwaysSucceed()),

                // Specific shapeshifted combat routines.
                 new Switch<Styx.ShapeshiftForm>(ret => Me.Shapeshift,
                                                 new SwitchArgument<Styx.ShapeshiftForm>(BalanceCombat, Styx.ShapeshiftForm.Moonkin),
                                                 new SwitchArgument<Styx.ShapeshiftForm>(BalanceCombat, Styx.ShapeshiftForm.Normal),
                                                 new SwitchArgument<Styx.ShapeshiftForm>(CatCombat, Styx.ShapeshiftForm.Cat),
                                                 new SwitchArgument<Styx.ShapeshiftForm>(BearCombat, Styx.ShapeshiftForm.Bear),
                                                 new SwitchArgument<Styx.ShapeshiftForm>(BearCombat, Styx.ShapeshiftForm.DireBear)
                     )
                 );
         }

         
        #region Restoration Combat

        private Composite RestorationCombat
         {
             get 
             { 
                 return new PrioritySelector(

                     // Very simple DPS if soloing as Restoration spec
                     
                     // Moonfire Restoration
                     new NeedToMoonfireRestoration(new MoonfireRestoration()),

                     // Insect Swarm Restoration
                     new NeedToInsectSwarmRestoration(new InsectSwarmRestoration()),

                     // Wrath Restoration
                     new NeedToWrathRestoration(new WrathRestoration()),

                     // Innervate Party
                     new NeedToInnervateParty(new InnervateParty()),

                     // Barkskin Party
                     new NeedToBarkskinParty(new BarkskinParty()),

                     // Rebirth
                     new NeedToRebirth(new Rebirth()),
                     

                     // Tank Healing
                     //============================================================

                     
                     // Swiftmend Tank
                     new NeedToSwiftmendTank(new SwiftmendTank()),

                     // Rejuvenation Tank
                     new NeedToRejuvenationTank(new RejuvenationTank()),

                     // Healing Touch Tank
                     new NeedToHealingTouchTank(new HealingTouchTank()),

                     // Regrowth Tank
                     new NeedToRegrowthTank(new RegrowthTank()),

                     // Nourish Tank
                     new NeedToNourishTank(new NourishTank()),

                     // Thorns Tank
                     new NeedToThornsTank(new ThornsTank()),

                     // Lifebloom Tank
                     //new NeedToLifebloomTank(new LifebloomTank()),


                     // AoE Healing
                     //============================================================

                     // Wild Growth
                     new NeedToWildGrowth(new WildGrowth()),

                     // Tranquility
                     new NeedToTranquility(new Tranquility()),


                     // Party Healing
                     //============================================================

                     // Swiftmend Party
                     new NeedToSwiftmendParty(new SwiftmendParty()),

                     // Rejuvenation Party
                     new NeedToRejuvenationParty(new RejuvenationParty()),

                     // Regrowth Party
                     new NeedToRegrowthParty(new RegrowthParty()),

                     // Nourish Party
                     new NeedToNourishParty(new NourishParty()),
                     
                     // Healing Touch Party
                     new NeedToHealingTouchParty(new HealingTouchParty()),



                     // Lifebloom Tank
                     new NeedToLifebloomTank(new LifebloomTank()),



                     // Other stuff
                     //============================================================

                     // Remove Corruption Party
                     new NeedToRemoveCorruptionParty(new RemoveCorruptionParty()),

                     // Heal Pets
                     new NeedToHealPets(new HealPets()),

                     // AlwaysSucceed
                     new NeedToAlwaysSucceed(new AlwaysSucceed())


                 ); 
             
             }
         }

            #endregion

        #region Combat Balance
         private Composite BalanceCombat
         {
             get
             {
                 return new PrioritySelector(

                     // Healing
                     //============================================================

                     // Innervate Balance
                     new NeedToInnervateBalance(new InnervateBalance()),

                     // Thorns Balance
                     new NeedToThornsBalance(new ThornsBalance()),

                     // Barkskin Balance
                     new NeedToBarkskinBalance(new BarkskinBalance()),

                     // Rejuvenation Balance
                     new NeedToRejuvenationBalance(new RejuvenationBalance()),

                     // Nourish Balance
                     new NeedToNourishBalance(new NourishBalance()),

                     // Regrowth Balance
                     new NeedToRegrowthBalance(new RegrowthBalance()),

                     //============================================================

                     // Retarget Priority Based
                     new NeedToRetargetPriorityBased(new RetargetPriorityBased()),

                     // Distance Check Balance
                     new NeedToDistanceCheckBalance(new DistanceCheckBalance()),

                     // LoS Balance
                     new NeedToLoSBalance(new LoSBalance()),

                     // Always Face Moving Target
                     new NeedToAlwaysFaceMovingTarget(new AlwaysFaceMovingTarget()),
                     

                     // Wild Mushroom: Detonate
                     new NeedToWildMushroomDetonate(new WildMushroomDetonate()),

                     // Force Of Nature
                     new NeedToForceOfNature(new ForceOfNature()),

                     // Starfall
                     new NeedToStarfall(new Starfall()),

                     // Hurricane
                     new NeedToHurricane(new Hurricane()),

                     // Insect Swarm Balance
                     new NeedToInsectSwarmBalance(new InsectSwarmBalance()),

                     // Moonfire Balance
                     new NeedToMoonfireBalance(new MoonfireBalance()),

                     // Faerie Fire Balance
                     new NeedToFaerieFireBalance(new FaerieFireBalance()),

                     // Starsurge
                     new NeedToStarsurge(new Starsurge()),

                     // Wild Mushrooms
                     new NeedToWildMushrooms(new WildMushrooms()),

                     // Typhoon
                     new NeedToTyphoon(new Typhoon()),

                     // AoE DoT Moonfire Balance
                     new NeedToAoEDoTMoonfireBalance(new AoEDoTMoonfireBalance()),
                     // AoE DoT Insect Swarm Balance
                     new NeedToAoEDoTInsectSwarmBalance(new AoEDoTInsectSwarmBalance()),
                     

                     // Primary Balance DPS
                     new NeedToPrimaryBalanceDPS(new PrimaryBalanceDPS())

                     );
             }
         }
        #endregion

        #region Combat Cat
         private Composite CatCombat
         {
             get
             {
                 return new PrioritySelector(

                     // Healing
                     //============================================================

                     // Innervate Feral Cat
                     new NeedToInnervateFeralCat(new InnervateFeralCat()),

                     // Thorns Feral Cat
                     new NeedToThornsFeralCat(new ThornsFeralCat()),

                     // Barkskin Feral Cat
                     new NeedToBarkskinFeralCat(new BarkskinFeralCat()),

                     // Rejuvenation Feral Cat
                     new NeedToRejuvenationFeralCat(new RejuvenationFeralCat()),

                     // Nourish Feral Cat
                     new NeedToNourishFeralCat(new NourishFeralCat()),

                     // Regrowth Feral Cat
                     new NeedToRegrowthFeralCat(new RegrowthFeralCat()),

                     // Survival Instincts Feral Cat
                     new NeedToSurvivalInstinctsFeralCat(new SurvivalInstinctsFeralCat()),

                     //============================================================

                     // Retarget Priority Based
                     new NeedToRetargetPriorityBased(new RetargetPriorityBased()),

                     // Distance Check Feral
                     new NeedToDistanceCheckFeral(new DistanceCheckFeral()),

                     // Interact Other
                     new NeedToInteractOther(new InteractOther()),

                     // Backup
                     new NeedToBackup(new Backup()),

                     // Stampede Ravage - only cast if you have Stamepde buff on you after casting Feral Charge (Cat)
                     new NeedToStampedeRavage(new StampedeRavage()),

                     // Feral Charge Feral Cat Combat
                     new NeedToFeralChargeFeralCatCombat(new FeralChargeFeralCatCombat()),

                     // Maim
                     new NeedToMaim(new Maim()),

                     // Interact PVP
                     new NeedToInteractPVP(new InteractPVP()),

                     // Berserk Feral Cat
                     new NeedToBerserkFeralCat(new BerserkFeralCat()),

                     // Tiger's Fury
                     new NeedToTigersFury(new TigersFury()),

                     // Auto Attack
                     new NeedToAutoAttack(new AutoAttack()),

                     // Faerie Fire Feral Cat
                     new NeedToFaerieFireFeralCat(new FaerieFireFeralCat()),

                     // Rip
                     new NeedToRip(new Rip()),

                     // Interact PVP
                     new NeedToInteractPVP(new InteractPVP()),

                     // Ferocious Bite
                     new NeedToFerociousBite(new FerociousBite()),
                     
                     // Skull Bash Cat
                     new NeedToSkullBashCat(new SkullBashCat()),

                     // Savage Roar
                     new NeedToSavageRoar(new SavageRoar()),

                     // Rake
                     new NeedToRake(new Rake()),

                     // Shred
                     new NeedToShred(new Shred()),

                     // Swipe Cat
                     new NeedToSwipeCat(new SwipeCat()),

                     // Interact PVP
                     new NeedToInteractPVP(new InteractPVP()),

                     // Main DPS spell (Claw or Mangle)
                     new NeedToClawOrMangle(new ClawOrMangle()),

                     // Distance Check Feral - again
                     new NeedToDistanceCheckFeral(new DistanceCheckFeral())
                     );
             }
         }
         #endregion
         
        #region Combat Bear
         private Composite BearCombat
         {
             get
             {
                 return new PrioritySelector(


                     //============================================================

                     // Retarget Priority Based
                     new NeedToRetargetPriorityBased(new RetargetPriorityBased()),

                     // Distance Check Feral
                     new NeedToDistanceCheckFeral(new DistanceCheckFeral()),

                     // Interact Other
                     new NeedToInteractOther(new InteractOther()),

                     // Backup
                     new NeedToBackup(new Backup()),

                     // Auto Attack
                     new NeedToAutoAttack(new AutoAttack()),

                     // Enrage
                     new NeedToEnrage(new Enrage()),

                     // Feral Charge Bear
                     new NeedToFeralChargeBear(new FeralChargeBear()),

                     // Bash
                     new NeedToBash(new Bash()),

                     // Maul
                     new NeedToMaul(new Maul()),

                     // Mangle Bear
                     new NeedToMangleBear(new MangleBear()),

                     // Swipe Bear
                     new NeedToSwipeBear(new SwipeBear()),

                     // Distance Check Feral - again
                     new NeedToDistanceCheckFeral(new DistanceCheckFeral())

                     );
             }
         }
         #endregion



        #endregion

        #region Behaviours

        #region AlwaysSucceed
        public class NeedToAlwaysSucceed : Decorator
        {
            public NeedToAlwaysSucceed(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                return true;
            }
        }

        public class AlwaysSucceed : Action
        {
            protected override RunStatus Run(object context)
            {
                ObjectManager.Update();
                return RunStatus.Success;
            }
        }
        #endregion


        #region TimerTest
        public class NeedToTimerTest : Decorator
        {
            public NeedToTimerTest(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                Settings._timermarker = 1;
                Timers.Reset("TimersTest");

                Utils.Log("[TimerTest Mark " + Settings._timermarker + "] : " + Timers.ElapsedMilliseconds("TimersTest"));
                return false;
            }
        }

        public class TimerTest : Action
        {
            protected override RunStatus Run(object context)
            {
                return RunStatus.Failure;
            }
        }
        #endregion

        #region TimerTest2
        public class NeedToTimerTest2 : Decorator
        {
            public NeedToTimerTest2(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                Settings._timermarker += 1;
                Utils.Log("[TimerTest Mark " + Settings._timermarker + "] : " + Timers.ElapsedMilliseconds("TimersTest"));
                return false;
            }
        }

        public class TimerTest2 : Action
        {
            protected override RunStatus Run(object context)
            {
                return RunStatus.Failure;
            }
        }
        #endregion

        #region TimerTest3
        public class NeedToTimerTest3 : Decorator
        {
            public NeedToTimerTest3(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                Settings._timermarker += 1;
                Utils.Log("[TimerTest Mark " + Settings._timermarker + " - FINISHED TIMER TEST] : " + Timers.ElapsedMilliseconds("TimersTest"));
                return false;
            }
        }

        public class TimerTest3 : Action
        {
            protected override RunStatus Run(object context)
            {
                return RunStatus.Failure;
            }
        }
        #endregion

        #region RetargetPVP
        public class NeedToRetargetPVP : Decorator
        {
            public NeedToRetargetPVP(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Utils.IsBattleground) return false;
                if (!Me.GotTarget) return false;
                if (Me.CurrentTarget.IsPet) return true;

                return false;
            }
        }

        public class RetargetPVP : Action
        {
            protected override RunStatus Run(object context)
            {
                Me.CurrentTarget.Pet.OwnedByUnit.Target();
                return RunStatus.Failure;
            }
        }
        #endregion
        
        #region Retarget
        public class NeedToRetarget : Decorator
        {
            public NeedToRetarget(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                //if (Utils.IsBattleground && Me.GotTarget && CT.IsPet) return true;
                if (Me.GotTarget && CT.IsAlive) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Restoration) return false;

                return true;
            }
        }

        public class Retarget : Action
        {
            protected override RunStatus Run(object context)
            {
               if (Me.GotAlivePet && Me.Pet.GotTarget)
                {
                    Me.Pet.CurrentTarget.Target();
                    Target.Face();
                    return RunStatus.Success;
                }


                WoWUnit unit = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < 40 && p.GotTarget && p.CurrentTarget.IsTargetingAnyMinion select p).FirstOrDefault();

                if (unit !=null)
                {
                    unit.Target();
                    Target.Face();
                    return RunStatus.Success;
                }

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Retarget Priority Based
        public class NeedToRetargetPriorityBased : Decorator
        {
            public NeedToRetargetPriorityBased(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;
                if (Settings.PriorityTargeting.Contains("never")) return false;
                if (!Utils.Adds) return false;
                if (!Timers.Expired("Retarget", 2500)) return false;
                int targetWeight;

                WoWUnit bestTarget = Utils.BestTarget(out targetWeight);
                if (bestTarget == null) return false;

                if (Me.GotTarget && targetWeight >= Utils.TargetWeight(CT)) { Timers.Reset("Retarget"); return false; }
                if (Me.GotTarget && bestTarget.Guid == Me.CurrentTarget.Guid) { Timers.Reset("Retarget"); return false; }

                // If we're here its because our current target is not the 'best target'
                return true;
            }
        }

        public class RetargetPriorityBased : Action
        {
            protected override RunStatus Run(object context)
            {
                int targetWeight;
                WoWUnit bestTarget = Utils.BestTarget(out targetWeight);
                if (bestTarget == null) return RunStatus.Failure;
                
                bestTarget.Target();
                bestTarget.Face();
                Utils.Log("** Target found using priority based targeting - retargeting **");
                Timers.Reset("Retarget");

                return RunStatus.Success;
            }
        }
        #endregion

        #region Move To
        public class NeedToMoveTo : Decorator
        {
            public NeedToMoveTo(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                
                if (Self.Immobilised) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;
                if (Target.IsFleeing) return false;

                if (CT.IsBehind(Me) && Me.IsMoving)
                {
                    Movement.StopMoving();
                    CT.Face();
                    return false;
                }
                //if (!Target.IsHealthPercentAbove(90) && CT.Combat) return false;

                //Utils.Log("******** IR: " + CT.InteractRange);
                return Target.IsDistanceMoreThan(CT.InteractRange);
            }
        }

        public class MoveTo : Action
        {
            protected override RunStatus Run(object context)
            {
                float precision = Navigator.PathPrecision;
                float interactRange = CT.InteractRange * (float)0.95;

                //Utils.Log("******** RevIR: " + interactRange);
                Navigator.PathPrecision = (interactRange) - (float) 0.1;
                WoWPoint location = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, interactRange);
                if (Navigator.GeneratePath(Me.Location, location).Length <= 0) return RunStatus.Failure;

                Movement.MoveTo(location);
                Navigator.PathPrecision = precision;

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Move Stop
        public class NeedToMoveStop : Decorator
        {
            public NeedToMoveStop(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Self.Immobilised) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;
                if (!Me.IsMoving) return false;
                if (Target.IsFleeing) return false;
                if (Target.IsPlayer) return false;
                if (Target.IsDistanceLessThan(Target.InteractRange)) return true;

                return false;
            }
        }

        public class MoveStop : Action
        {
            protected override RunStatus Run(object context)
            {

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Interact Other
        public class NeedToInteractOther : Decorator
        {
            public NeedToInteractOther(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.ToUpper().Contains("ALWAYS")) return false;
                if (!Me.GotTarget) return false;
                if (Target.IsFleeing) return true;
                if (!Timers.Expired("Interact", 500)) return false;
                //if (Me.IsMoving) return false;
                if (Target.IsDistanceMoreThan(CT.InteractRange * 1.5f)) return false;
                //if (Me.IsSafelyFacing(CT.Location) && !Utils.IsBattleground) return false;
                //if (Target.IsDistanceLessThan(Target.InteractRange * 0.55f) && Me.IsSafelyFacing(CT.Location) && !Utils.IsBattleground) return false;
                //if (!CT.WithinInteractRange) return false;

                return true;
            }
        }

        public class InteractOther : Action
        {
            protected override RunStatus Run(object context)
            {
                Timers.Reset("Interact");
                CT.Interact();
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Interact PVP - Trying to increase the movemnt reaction times for PVP
        public class NeedToInteractPVP : Decorator
        {
            public NeedToInteractPVP(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.ToUpper().Contains("ALWAYS")) return false;
                if (!Me.GotTarget) return false;
                if (Target.IsFleeing) return true;
                if (!CT.IsPlayer || !Utils.IsBattleground) return false;
                if (Target.IsDistanceMoreThan(CT.InteractRange * 1.5f)) return false;

                return true;
            }
        }

        public class InteractPVP : Action
        {
            protected override RunStatus Run(object context)
            {
                Timers.Reset("Interact");
                CT.Interact();
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Interact - Move closer or face the target
        public class NeedToInteract : Decorator
        {
            public NeedToInteract(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;

                /*
                if (Target.IsDistanceMoreThan(Target.InteractRange * 1.5)) return false;

                if (!Me.GotTarget) return false;
                if (Target.IsFleeing || CT.IsMoving)
                {
                    //Utils.Log("********** moving + interact");
                    CT.Interact();
                }
                 */
                if (!Timers.Expired("Interact", 350)) return false;

                //ObjectManager.Update();
                return true;
            }
        }

        public class Interact : Action
        {
            protected override RunStatus Run(object context)
            {
                CT.Interact();
                Timers.Reset("Interact");

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Back up if we're too close
        public class NeedToBackup : Decorator
        {
            public NeedToBackup(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                double interactDistance = Target.InteractRange;

                if (Self.Immobilised) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;
                if (Target.IsFleeing) return false;
                if (CT.IsMoving) return false;

                if (!Timers.Expired("DistanceCheck", 500)) return false;

                return (Target.IsDistanceLessThan(interactDistance * 0.60));

            }
        }

        public class Backup : Action
        {
            protected override RunStatus Run(object context)
            {
                double interactDistance = Target.InteractRange;

                while (Target.IsDistanceLessThan(interactDistance * 0.7))
                {
                    if (CT.IsMoving) break;
                    WoWMovement.Move(WoWMovement.MovementDirection.Backwards);
                    System.Threading.Thread.Sleep(100);
                }

                WoWMovement.MoveStop();
                Utils.Log("Backup a wee bit, too close to our target", Utils.Colour("Green"));
                Timers.Reset("DistanceCheck");

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Move Closer
        public class NeedToMoveCloser : Decorator
        {
            public NeedToMoveCloser(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Self.Immobilised) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange * 1.5))
                {
                    return true;
                    //Movement.MoveTo(Target.InteractRange);
                }

                return false;
            }
        }

        public class MoveCloser : Action
        {
            protected override RunStatus Run(object context)
            {
                Movement.MoveTo(Target.InteractRange);
                return RunStatus.Success;
            }
        }
        #endregion

        #region Thorns
        public class NeedToThorns : Decorator
        {
            public NeedToThorns(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Thorns";

                if (!ClassHelpers.Druid.IsCasterCapable) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Self.CanBuffMe(dpsSpell)) return false;
                if (!Utils.Adds) return false;


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Thorns : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Thorns";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Innervate
        public class NeedToInnervate : Decorator
        {
            public NeedToInnervate(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Self.IsPowerPercentAbove(Settings.InnervateManaBalance)) return false;
                if (!ClassHelpers.Druid.IsCasterCapable) return false;
                if (!Utils.CombatCheckOk("Innervate", true)) return false;
                if (Self.IsBuffOnMe(Druid_ID.Innervate)) return false;

                return Spell.CanCast("Innervate");
            }
        }

        public class Innervate : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = Spell.Cast("Innervate", Me);
                Utils.LagSleep();
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        // Procs

        #region Shooting Stars PROC
        public class NeedToShootingStars : Decorator
        {
            public NeedToShootingStars(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Starsurge";

                if (Self.Immobilised) return false;
                if (!Self.IsBuffOnMe(Druid_ID.ShootingStars)) return false;
                //if (!Self.IsBuffOnMeLUA("Shooting Stars")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                //if (!Self.IsBuffOnMe(81141)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShootingStars : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Starsurge";

                Utils.Log("** Shooting Stars PROC **", Utils.Colour("Red"));
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        // Combat

        #region Feral Cat

        #region Distance Check Feral
        public class NeedToDistanceCheckFeral : Decorator
        {
            public NeedToDistanceCheckFeral(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.ToUpper().Contains("ALWAYS")) return false;
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;
                float distance = !Target.IsFleeing ? Target.InteractRange : Target.InteractRange * 0.1f;

                if (Target.IsDistanceLessThan(distance) && Me.IsMoving) WoWMovement.MoveStop();
                if (Target.IsDistanceMoreThan(distance)) return true;

                return false;
            }
        }

        public class DistanceCheckFeral : Action
        {
            protected override RunStatus Run(object context)
            {
                // Use for Pull and Combat distance check.
                // If we are pulling the adjust the distance to 1 yard. 
                float distanceMoveTo = Me.Combat ? Target.InteractRange : 0;

                if (ClassHelpers.Druid.Shapeshift.IsCatForm && Timers.Expired("RootBreak",4000))
                {
                    foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
                    {
                        if (!aura.Value.IsHarmful) continue;
                        if (aura.Value.Spell.Mechanic == WoWSpellMechanic.Rooted || aura.Value.Spell.Mechanic == WoWSpellMechanic.Polymorphed)
                        {
                            if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;

                            Utils.Log("-Breaking restraints by shapeshifting...");
                            ClassHelpers.Druid.Shapeshift.CatForm();
                            Timers.Reset("RootBreak");
                        }
                    }
                }
                
                Movement.MoveTo(distanceMoveTo);

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Claw or Mangle
        public class NeedToClawOrMangle : Decorator
        {
            public NeedToClawOrMangle(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Spell.IsKnown("Mangle (Cat)") ? "Mangle (Cat)" : "Claw";

                if (CLC.ResultOK(Settings.FerociousBite) && CLC.ResultOK(Settings.FerociousBiteComboPoints) && Spell.IsKnown("Ferocious Bite")) return false;
                
                if (!Self.IsPowerPercentAbove(Settings.AttackEnergyFeralCat)) return false;
                if (Me.CurrentEnergy < Spell.PowerCost(dpsSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if (CLC.ResultOK(Settings.Shred) && Spell.IsKnown("Shred"))
                {
                    if (Me.IsSafelyBehind(CT) && !CT.IsSafelyFacing(Me))
                    {
                        return false;
                    }
                }

                return Spell.CanCast(dpsSpell);
            }
        }

        public class ClawOrMangle : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Spell.IsKnown("Mangle (Cat)") ? "Mangle (Cat)" : "Claw";

                if (!Me.IsSafelyFacing(CT))
                {
                    Target.Face();
                    return RunStatus.Running;
                }
                bool result = Spell.Cast(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Ferocious Bite
        public class NeedToFerociousBite : Decorator
        {
            public NeedToFerociousBite(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Ferocious Bite";

                if (!Me.GotTarget) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy <= Spell.PowerCost(dpsSpell)) return false;

                // Use it or lose it. Target is about to die, best we use our combo points
                if (!Target.IsElite && Me.ComboPoints >= 2 && !Target.IsHealthPercentAbove(30) && Spell.IsKnown(dpsSpell)) return true;

                if (!CLC.ResultOK(Settings.FerociousBite)) return false;
                if (!CLC.ResultOK(Settings.FerociousBiteComboPoints)) return false;
                
                //if (CT.IsSafelyBehind(Me)) return false;
                if (!Me.IsSafelyFacing(CT.Location)) return false;

                return Spell.CanCast(dpsSpell);
            }
        }

        public class FerociousBite : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Ferocious Bite";

                bool result = Spell.Cast(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rip
        public class NeedToRip : Decorator
        {
            public NeedToRip(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rip";

                if (!CLC.ResultOK(Settings.Rip)) return false;
                if (!CLC.ResultOK(Settings.RipComboPoints)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy <= Spell.PowerCost(dpsSpell)) return false;
                if (Target.IsDebuffOnTarget(dpsSpell))
                {
                    foreach (KeyValuePair<string, WoWAura> aura in CT.Auras)
                    {
                        if (aura.Key != dpsSpell) continue;
                        if (aura.Value.CreatorGuid != Me.Guid) continue;
                        if (aura.Value.TimeLeft.TotalSeconds > 4) return false;
                    }
                }
                if (CT.IsSafelyBehind(Me)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Rip : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Rip";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Maim
        public class NeedToMaim : Decorator
        {
            public NeedToMaim(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Maim";

                if (!Me.GotTarget) return false;
                if (!CLC.ResultOK(Settings.Maim)) return false;
                if (!CLC.ResultOK(Settings.MaimComboPoints)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy <= Spell.PowerCost(dpsSpell)) return false;
                if (!Me.IsSafelyFacing(CT.Location)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Maim : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Maim";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rake
        public class NeedToRake : Decorator
        {
            public NeedToRake(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rake";

                if (!CLC.ResultOK(Settings.Rake)) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) { if (CT.Auras.Where(aura => aura.Key == dpsSpell).Any(aura => aura.Value.TimeLeft.TotalSeconds > 3)) { return false; } }
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy < Spell.PowerCost(dpsSpell)) return false;
                //if (CT.IsSafelyBehind(Me)) return false;
                if (!Me.IsSafelyFacing(CT.Location)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Rake : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Rake";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Skull Bash Cat
        public class NeedToSkullBashCat : Decorator
        {
            public NeedToSkullBashCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Skull Bash(Cat Form)";

                if (!Me.GotTarget) return false;
                if (!CT.IsCasting) return false;
                if (!CLC.ResultOK(Settings.SkullBashCat)) return false;
                //if (Settings.SkullBashCat.Contains("never")) return false;
                //if (Settings.SkullBashCat.Contains("healing") && !CT.CastingSpell.Description.ToUpper().Contains("HEAL")) return false;
                if (Me.CurrentEnergy < 25) return false;
                if (Self.Immobilised) return false;
                if (Me.IsCasting || Spell.IsGCD) return false;
                if (Target.IsDistanceMoreThan(13)) return false;
                if (CT.CastingSpell.CastTime <= 1000) return false;
                if (!SpellManager.CanCast(80965)) return false;

                return (Spell.CanCastLUA(dpsSpell));
            }
        }

        public class SkullBashCat : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log(string.Format("-Interrupting {0}, being cast by {1}", CT.CastingSpell.Name, CT.Name));
                const string dpsSpell = "Skull Bash(Cat Form)";
                Spell.CastByNameLUA(dpsSpell);

                return RunStatus.Success;
            }
        }
        #endregion

        #region Swipe Cat
        public class NeedToSwipeCat : Decorator
        {
            public NeedToSwipeCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Swipe (Cat)";

                if (!Utils.Adds) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy < 50) return false;

                bool cancast = Spell.CanCast(dpsSpell);

                int countOfMobs = Utils.CountOfAddsInRange(8, Me.Location);
                if (Settings.SwipeCat.Contains("3+") && countOfMobs >= 3 && cancast) return true;
                if (Settings.SwipeCat.Contains("4+") && countOfMobs >= 4 && cancast) return true;
                if (Settings.SwipeCat.Contains("5+") && countOfMobs >= 5 && cancast) return true;
                if (Settings.SwipeCat.Contains("6+") && countOfMobs >= 6 && cancast) return true;
                if (Settings.SwipeCat.Contains("7+") && countOfMobs >= 7 && cancast) return true;
                if (Settings.SwipeCat.Contains("8+") && countOfMobs >= 8 && cancast) return true;

                return false;
            }
        }

        public class SwipeCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Swipe (Cat)";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Tiger's Fury
        public class NeedToTigersFury : Decorator
        {
            public NeedToTigersFury(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Tiger's Fury";

                if (!CLC.ResultOK(Settings.TigersFury)) return false;
                if (Self.Immobilised) return false;
                if (Self.IsBuffOnMe(Druid_ID.TigersFury,Self.AuraCheck.ActiveAuras)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy < Spell.PowerCost(dpsSpell)) return false;
                if (Self.IsBuffOnMe(Druid_ID.Berserk)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class TigersFury : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Tiger's Fury";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Thorns Feral Cat
        public class NeedToThornsFeralCat : Decorator
        {
            public NeedToThornsFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Thorns";

                if (!CLC.ResultOK(Settings.ThornsFeralCat)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(Druid_ID.Thorns,Self.AuraCheck.ActiveAuras)) return false;

                bool thornsOK = ObjectManager.GetObjectsOfType<WoWUnit>().Where(unit => unit.Combat).Where(unit => unit.Distance <= unit.InteractRange).Any(unit => unit.CurrentTargetGuid == Me.Guid);

                return (Spell.CanCast(dpsSpell) && thornsOK);
            }
        }

        public class ThornsFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Thorns";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shred
        public class NeedToShred : Decorator
        {
            public NeedToShred(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shred";

                if (!CLC.ResultOK(Settings.Shred)) return false;
                if (!Me.IsSafelyBehind(CT)) return false;
                if (CT.IsSafelyFacing(Me)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy < Spell.PowerCost(dpsSpell)) return false;
                if (CLC.ResultOK(Settings.Rip) && CLC.ResultOK(Settings.RipComboPoints) && !Target.IsDebuffOnTarget(Druid_ID.Rip) && Spell.CanCast("Rip")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Shred : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = "Shred";
                const string mangleDebuff = "Mangle";

                if (Target.IsDebuffOnTarget(mangleDebuff))
                {
                    foreach (KeyValuePair<string, WoWAura> aura in CT.Auras)
                    {
                        if (aura.Key != mangleDebuff) continue;
                        if (aura.Value.TimeLeft.TotalSeconds < 5)
                        {
                            dpsSpell = "Mangle (Cat)";
                        }
                    }
                }
                if (!Target.IsDebuffOnTarget(mangleDebuff) && Spell.CanCast("Mangle (Cat)")) dpsSpell = "Mangle (Cat)";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Savage Roar
        public class NeedToSavageRoar : Decorator
        {
            public NeedToSavageRoar(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Savage Roar";

                if (!CLC.ResultOK(Settings.SavageRoar)) return false;
                if (!CLC.ResultOK(Settings.SavageRoarComboPoints)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(Druid_ID.SavageRoar,Self.AuraCheck.AllAuras)) return false;
                if (Me.CurrentEnergy < Spell.PowerCost(dpsSpell)) return false;
                if (CLC.ResultOK(Settings.Rip) && CLC.ResultOK(Settings.RipComboPoints) && !Target.IsDebuffOnTarget(Druid_ID.Rip) && Spell.CanCast("Rip")) return false;
                if (CLC.ResultOK(Settings.Rip) && CLC.ResultOK(Settings.RipComboPoints) && Target.IsDebuffOnTarget(Druid_ID.Rip))
                {
                    double timeLeft = 0;
                    foreach (KeyValuePair<string, WoWAura> aura in CT.Auras)
                    {
                        if (aura.Value.SpellId != Druid_ID.Rip) continue;
                        if (aura.Value.CreatorGuid != Me.Guid) continue;
                        
                        timeLeft = aura.Value.TimeLeft.TotalSeconds;
                        if (timeLeft < 6)
                        {
                            Utils.Log("** Delaying Savage Roar - RIP time left < 6 seconds **");
                            return false;
                        }

                        if (Me.ComboPoints == 5 && timeLeft < 8)
                        {
                            Utils.Log("** Delaying Savage Roar - 5 Combo points and RIP due to expire soon, lets save up our points **");
                            return false;
                        }
                    }
                }


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SavageRoar : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Savage Roar";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Berserk Feral Cat
        public class NeedToBerserkFeralCat : Decorator
        {
            public NeedToBerserkFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Berserk";

                if (!CLC.ResultOK(Settings.BerserkFeralCat)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                //if (Self.IsBuffOnMeLUA("Tiger's Fury")) return false;
                if (Self.IsBuffOnMe(Druid_ID.TigersFury)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BerserkFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Berserk";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Stampede Ravage
        public class NeedToStampedeRavage : Decorator
        {
            public NeedToStampedeRavage(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                if (!CLC.ResultOK(Settings.FeralCatRavage)) return false;
                if (Self.Immobilised) return false;
                //if (Self.IsBuffOnMeLUA("Stampede") && Timers.Expired("FeralCharge", 5000)) return true;
                if (Self.IsBuffOnMe(Druid_ID.Stampede)  && Timers.Expired("FeralCharge", 5000)) return true;

                return false;
            }
        }

        public class StampedeRavage : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Ravage!";
                Spell.CastByNameLUA(dpsSpell);

                return RunStatus.Success;
            }
        }
        #endregion

        #region Faerie Fire Feral Cat
        public class NeedToFaerieFireFeralCat : Decorator
        {
            public NeedToFaerieFireFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Faerie Fire (Feral)";

                if (!CLC.ResultOK(Settings.FaerieFireFeralCat)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDebuffOnTarget(Druid_ID.FaerieFire)) return false;
                if (!Me.IsSafelyFacing(CT.Location)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FaerieFireFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Faerie Fire (Feral)";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Feral Charge Feral Cat Combat
        public class NeedToFeralChargeFeralCatCombat : Decorator
        {
            public NeedToFeralChargeFeralCatCombat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Feral Charge (Cat)";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceLessThan(9)) return false;
                if (Target.IsDistanceMoreThan(25)) return false;
                if (CT.InteractRange > Spell.MinDistance(dpsSpell)) return false;
                if (!Me.IsSafelyFacing(CT.Location)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FeralChargeFeralCatCombat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Feral Charge (Cat)";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("FeralCharge");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        




        #endregion

        #region Feral Bear

        #region Mangle Bear
        public class NeedToMangleBear : Decorator
        {
            public NeedToMangleBear(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mangle (Bear)";

                //if (!CLC.ResultOK(Settings.MangleBear)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentEnergy < Spell.PowerCost(dpsSpell)) return false;
                if (!Me.IsSafelyFacing(CT)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MangleBear : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mangle (Bear)";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Feral Charge Bear
        public class NeedToFeralChargeBear : Decorator
        {
            public NeedToFeralChargeBear(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Feral Charge (Bear)";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceLessThan(9)) return false;
                if (Target.IsDistanceMoreThan(25)) return false;
                if (CT.InteractRange > Spell.MinDistance(dpsSpell)) return false;
                if (!Me.IsSafelyFacing(CT.Location)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FeralChargeBear : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Feral Charge (Bear)";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Maul
        public class NeedToMaul : Decorator
        {
            public NeedToMaul(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Maul";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Maul : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Maul";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Enrage
        public class NeedToEnrage : Decorator
        {
            public NeedToEnrage(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Enrage";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.EnergyPercent > 40) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Enrage : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Enrage";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Bash
        public class NeedToBash : Decorator
        {
            public NeedToBash(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Bash";

                //if (!CLC.ResultOK(Settings.Bash)) return false;
                if (!Me.GotTarget) return false;
                if (!CT.IsCasting) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Me.IsSafelyFacing(CT)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Bash : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Bash";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region SwipeBear
        public class NeedToSwipeBear : Decorator
        {
            public NeedToSwipeBear(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Swipe (Bear)";

                //if (!CLC.ResultOK(Settings.SwipeBear)) return false;
                if (!Utils.Adds) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SwipeBear : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Swipe (Bear)";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #endregion

        #region Balance

        #region LoS - Balance
        public class NeedToLoSBalance : Decorator
        {
            public NeedToLoSBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget || (Me.GotTarget && CT.Dead)) return false;
                return !Me.CurrentTarget.InLineOfSightOCD;
            }
        }

        public class LoSBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.MoveToLineOfSight(CT.Location);
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Primary DPS Spell
        public class NeedToPrimaryBalanceDPS : Decorator
        {
            public NeedToPrimaryBalanceDPS(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (ClassHelpers.Druid.IsHealerOnly) return false;
                if (Me.IsMoving) return false;
                //if (Self.IsBuffOnMeLUA("Shooting Stars") && !Spell.IsOnCooldown("Starsurge")) return false;
                if (Self.IsBuffOnMe(Druid_ID.ShootingStars, Self.AuraCheck.ActiveAuras) && !Spell.IsOnCooldown("Starsurge")) return false;

                string dpsSpell = Settings.PrimaryDPSSpell;
                if (dpsSpell == "Automatic") dpsSpell = ClassHelpers.Druid.BalanceDPSSpell;

                if (Self.IsBuffOnMe(Druid_ID.EclipseSolar,Self.AuraCheck.AllAuras)) dpsSpell = "Wrath";
                if (Self.IsBuffOnMe(Druid_ID.EclipseLunar, Self.AuraCheck.AllAuras)) dpsSpell = "Starfire";
                ClassHelpers.Druid.BalanceDPSSpell = dpsSpell;

                // Fall back if the primary DPS spell is on cooldown
                if (Spell.IsOnCooldown(dpsSpell)) { switch (dpsSpell) { case "Starfire": dpsSpell = "Wrath"; break; case "Wrath": dpsSpell = "Starfire"; break; } }

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!ClassHelpers.Druid.IsCasterCapable) return false;
                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Feral && Spell.IsKnown("Cat Form")) return false;
                if (!Spell.IsEnoughPower(dpsSpell)) return false;
                if (!Utils.IsInLineOfSight(CT.Location)) { Utils.MoveToLineOfSight(CT.Location); return false; }

                bool result = (Spell.CanCast(dpsSpell));

                return result;
            }
        }

        public class PrimaryBalanceDPS : Action
        {
            protected override RunStatus Run(object context)
            {
                //if (!Utils.IsInLineOfSight(CT.Location)) Utils.MoveToLineOfSight(CT.Location);

                //Target.Face();
                if (!Me.IsSafelyFacing(CT) && Settings.LazyRaider.ToUpper().Contains("NEVER"))
                {
                    Target.Face();
                    return RunStatus.Running;
                }
                
                string dpsSpell = ClassHelpers.Druid.BalanceDPSSpell;

                if (Self.IsBuffOnMe(Druid_ID.EclipseSolar, Self.AuraCheck.AllAuras)) dpsSpell = "Wrath";
                if (Self.IsBuffOnMe(Druid_ID.EclipseLunar, Self.AuraCheck.AllAuras)) dpsSpell = "Starfire";

                // Fall back if the primary DPS spell is on cooldown
                if (Spell.IsOnCooldown(dpsSpell)) { switch (dpsSpell) { case "Starfire": dpsSpell = "Wrath"; break; case "Wrath": dpsSpell = "Starfire"; break; } }

                bool result = Spell.Cast(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Moonfire Balance
        public class NeedToMoonfireBalance : Decorator
        {
            public NeedToMoonfireBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Moonfire";
                
                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Feral) return false;
                return Utils.CCheck(dpsSpell, "target_isnot:debuffed|Moonfire|Sunfire", "clc:" + Settings.MoonfireBalance);
            }
        }

        public class MoonfireBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Moonfire";
                if (!Me.IsSafelyFacing(CT) && Settings.LazyRaider.ToUpper().Contains("NEVER"))
                {
                    Target.Face();
                    return RunStatus.Running;
                }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Faerie Fire Balance

        public class NeedToFaerieFireBalance : Decorator
        {
            public NeedToFaerieFireBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Faerie Fire";

                if (Target.IsDebuffOnTarget(dpsSpell)) return false;
                return Utils.CCheck(dpsSpell,"clc:"+ Settings.FaerieFireBalance);
            }
        }

        public class FaerieFireBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Faerie Fire";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Starsurge
        public class NeedToStarsurge : Decorator
        {
            public NeedToStarsurge(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Starsurge";

                if (!Me.GotTarget) return false;
                if (Self.IsBuffOnMe(Druid_ID.ShootingStars) && (!Spell.IsGCD && !Me.IsCasting)) return true;
                //if (Self.IsBuffOnMeLUA("Shooting Stars") && (!Spell.IsGCD && !Me.IsCasting)) return true;
                if (!CLC.ResultOK(Settings.StarsurgeBalance)) return false;

                return Utils.CCheck(dpsSpell,"Timerexpired:Pull|2000");
            }
        }

        public class Starsurge : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Starsurge";

                if (!Me.GotTarget) return RunStatus.Failure;

                if (!Me.IsSafelyFacing(CT) && Settings.LazyRaider.ToUpper().Contains("NEVER"))
                {
                    Target.Face();
                    return RunStatus.Running;
                }
                bool result = Spell.Cast(dpsSpell);

                //while (Me.IsCasting) System.Threading.Thread.Sleep(500); Timers.Reset("Pull");
                Timers.Reset("Pull");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Insect Swarm Balance
        public class NeedToInsectSwarmBalance : Decorator
        {
            public NeedToInsectSwarmBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Insect Swarm";
                
                return Utils.CCheck(dpsSpell,"target_isnot:debuffed|Insect Swarm","clc:"+Settings.InsectSwarmBalance);
            }
        }

        public class InsectSwarmBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Insect Swarm";
                Target.Face();
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Thorns Balance

        public class NeedToThornsBalance : Decorator
        {
            public NeedToThornsBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Thorns";
                bool thornsOK = false;
                
                if (Self.IsBuffOnMe(Druid_ID.Thorns)) return false;
                bool result = Utils.CCheck(dpsSpell, "clc:" + Settings.ThornsBalance);

                if (result) thornsOK = ObjectManager.GetObjectsOfType<WoWUnit>().Where(unit => unit.Combat).Where(unit => unit.Distance <= unit.InteractRange).Any(unit => unit.CurrentTargetGuid == Me.Guid);

                return result && thornsOK;
            }
        }

        public class ThornsBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Thorns";
                bool result = Spell.Cast(dpsSpell,Me);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Typhoon

        public class NeedToTyphoon : Decorator
        {
            public NeedToTyphoon(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Typhoon";

                bool result = Utils.CCheck(dpsSpell, "clc:" + Settings.Typhoon);

                return result;
            }
        }

        public class Typhoon : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Typhoon";
                if (!Me.IsSafelyFacing(CT) && Settings.LazyRaider.ToUpper().Contains("NEVER"))
                {
                    Target.Face();
                    return RunStatus.Running;
                    //System.Threading.Thread.Sleep(700);
                }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Force Of Nature

        public class NeedToForceOfNature : Decorator
        {
            public NeedToForceOfNature(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Force of Nature";

                return Utils.CCheck(dpsSpell,"clc:"+ Settings.ForceOfNature);
            }
        }

        public class ForceOfNature : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Force of Nature";
                bool result = Spell.Cast(dpsSpell,CT.Location);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Starfall

        public class NeedToStarfall : Decorator
        {
            public NeedToStarfall(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Starfall";

                return Utils.CCheck(dpsSpell,"clc:"+Settings.Starfall);
            }
        }

        public class Starfall : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Starfall";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Wild Mushrooms
        public class NeedToWildMushrooms : Decorator
        {
            public NeedToWildMushrooms(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Wild Mushroom";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                int mushyCount = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.FactionId == 4).Count(o => o.CreatedByUnitGuid == Me.Guid);
                if (mushyCount == 3) return false;

                if (!CLC.ResultOK(Settings.WildMushroomBalance)) return false;
                if (Me.GotTarget && CT.IsMoving) return false;
                if (!Timers.Expired("MushroomsGoBoom", 5000)) return false;
                //if (Spell.IsOnCooldown("Wild Mushroom: Detonate")) return false;
                
                if (Settings.WildMushroomCountBalance.Contains("1 Wild") && mushyCount >= 1) return false;
                if (Settings.WildMushroomCountBalance.Contains("2 Wild") && mushyCount >= 2) return false;
                if (Settings.WildMushroomCountBalance.Contains("3 Wild") && mushyCount >= 3) return false;
                

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class WildMushrooms : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Wild Mushroom";
                bool result = Spell.Cast(dpsSpell,CT.Location);
                Timers.Reset("Mushrooms");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Wild Mushroom Detonate
        public class NeedToWildMushroomDetonate : Decorator
        {
            public NeedToWildMushroomDetonate(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Wild Mushroom: Detonate";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell,false)) return false;

                if (!Timers.Expired("Mushrooms", 3300)) return false;
                //int mushyCount = ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.FactionId == 4).Count(o => o.CreatedByUnitGuid == Me.Guid);
                int mushyCount = 0;

                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (unit.Distance > 40) continue;
                    if (unit.FactionId != 4) continue;
                    if (!unit.Name.ToUpper().Contains("WILD MUSHROOM")) continue;
                    if (unit.CreatedByUnitGuid != Me.Guid) continue;

                    mushyCount += 1;
                }

                if (mushyCount == 0) return false;
                if (Settings.WildMushroomCountBalance.Contains("1 Wild") && mushyCount >= 1) return (Spell.CanCast(dpsSpell));
                if (Settings.WildMushroomCountBalance.Contains("2 Wild") && mushyCount >= 2) return (Spell.CanCast(dpsSpell));
                if (Settings.WildMushroomCountBalance.Contains("3 Wild") && mushyCount >= 3) return (Spell.CanCast(dpsSpell));

                return false;
            }
        }

        public class WildMushroomDetonate : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Wild Mushroom: Detonate";

                bool result = Spell.Cast(dpsSpell);
                if (result) Timers.Reset("MushroomsGoBoom");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region AoE DoT Moonfire Balance
        public class NeedToAoEDoTMoonfireBalance: Decorator
        {
            public NeedToAoEDoTMoonfireBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = "Moonfire";
                bool DoTAdds = false;

                if (!Utils.Adds) return false;
                if (!CLC.ResultOK(Settings.AOEDOTBalance)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Feral) return false;
                
                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (!unit.Combat) continue;
                    if (unit.Dead) continue;

                    foreach (KeyValuePair<string, WoWAura> aura in unit.Auras)
                    {
                        if (aura.Value.SpellId == Druid_ID.Moonfire && aura.Value.TimeLeft.TotalSeconds > 4) continue;
                        if (aura.Value.SpellId == Druid_ID.Sunfire && aura.Value.TimeLeft.TotalSeconds > 4) continue;
                    }

                    //if (unit.Auras.ContainsKey("Moonfire") && unit.Auras["Moonfire"].TimeLeft.TotalSeconds > 4) continue;
                    //if (unit.Auras.ContainsKey("Sunfire") && unit.Auras["Sunfire"].TimeLeft.TotalSeconds > 4) continue;
                    //if ((unit.Auras.ContainsKey("Sunfire") || unit.Auras.ContainsKey("Moonfire"))) continue;
                    if (!unit.InLineOfSightOCD) continue;
                    if (!unit.Attackable) continue;
                    if (unit.Pacified) continue;
                    if (!unit.IsTargetingMeOrPet && !unit.IsTargetingMyPartyMember && !unit.IsTargetingMyRaidMember) continue;
                    if (Utils.CrowdControlSpellsList.Any(s => unit.Auras.ContainsKey(s))) continue;
                    //if (unit.HealthPercent < 25 && !unit.Elite) continue;
                    if (unit.Pacified) continue;

                    DoTAdds = true;
                }

                return (Spell.CanCast(dpsSpell) && DoTAdds);
            }
        }

        public class AoEDoTMoonfireBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Moonfire";
                bool result = false;

                

                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;

                    //if (Self.IsBuffOnMeLUA("Shooting Stars") && !Spell.IsOnCooldown("Starsurge")) return RunStatus.Success;
                    if (Self.IsBuffOnMe(Druid_ID.ShootingStars) && !Spell.IsOnCooldown("Starsurge")) return RunStatus.Success;
                    if (!unit.Combat) continue;
                    if (unit.Dead) continue;
                    foreach (KeyValuePair<string, WoWAura> aura in unit.Auras)
                    {
                        if (aura.Value.SpellId == Druid_ID.Moonfire && aura.Value.TimeLeft.TotalSeconds > 4) continue;
                        if (aura.Value.SpellId == Druid_ID.Sunfire && aura.Value.TimeLeft.TotalSeconds > 4) continue;
                    }
                    //if (unit.Auras.ContainsKey("Moonfire") && unit.Auras["Moonfire"].TimeLeft.TotalSeconds > 4) continue;
                    //if (unit.Auras.ContainsKey("Sunfire") && unit.Auras["Sunfire"].TimeLeft.TotalSeconds > 4) continue;
                    //if (unit.Auras.ContainsKey("Sunfire") || unit.Auras.ContainsKey("Moonfire")) continue;
                    if (!unit.InLineOfSightOCD) continue;
                    if (unit.Distance > Spell.MaxDistance(dpsSpell)) continue;
                    if (!unit.IsTargetingMeOrPet && !unit.IsTargetingMyPartyMember && !unit.IsTargetingMyRaidMember) continue;
                    if (!Me.IsSafelyFacing(unit)) continue;
                    if (unit.Pacified) continue;
                    if (!unit.Attackable) continue;
                    if (unit.Pacified) continue;
                    if (Utils.CrowdControlSpellsList.Any(s => unit.Auras.ContainsKey(s))) continue;
                    //if (unit.HealthPercent < 25 && !unit.Elite) continue;

                    if (Me.CurrentMana < Spell.PowerCost(dpsSpell)) break;
                    string nameOfTarget = !unit.IsPlayer ? unit.Name : unit.Class.ToString();
                    Utils.Log("-AoE (Moonfire) dotting " + nameOfTarget);

                    Spell.Cast(dpsSpell,unit);
                    if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;
                    result = true;
                }
                
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region AoE DoT Insect Swarm Balance
        public class NeedToAoEDoTInsectSwarmBalance : Decorator
        {
            public NeedToAoEDoTInsectSwarmBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = "Insect Swarm";
                bool DoTAdds = false;

                if (!Utils.Adds) return false;
                if (!CLC.ResultOK(Settings.AOEDOTBalance)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Feral) return false;

                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (!unit.Combat) continue;
                    if (unit.Dead) continue;
                    //if (unit.Auras.ContainsKey(dpsSpell) && unit.Auras[dpsSpell].TimeLeft.TotalSeconds > 2) continue;
                    foreach (KeyValuePair<string, WoWAura> aura in unit.Auras)
                    {
                        if (aura.Value.SpellId == Druid_ID.InsectSwarm && aura.Value.TimeLeft.TotalSeconds > 4) continue;
                    }

                    if (!unit.InLineOfSightOCD) continue;
                    if (!unit.Attackable) continue;
                    if (unit.Pacified) continue;
                    if (unit.Distance > Spell.MaxDistance(dpsSpell)) continue;
                    if (!unit.IsTargetingMeOrPet && !unit.IsTargetingMyPartyMember && !unit.IsTargetingMyRaidMember) continue;
                    if (Utils.CrowdControlSpellsList.Any(s => unit.Auras.ContainsKey(s))) continue;
                    if (unit.Elite && unit.HealthPercent < 10 || !unit.Elite && unit.HealthPercent < 25) continue;

                    //if (unit.HealthPercent < 25 && !unit.Elite) continue;

                    DoTAdds = true;
                }

                return (Spell.CanCast(dpsSpell) && DoTAdds);
            }
        }

        public class AoEDoTInsectSwarmBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Insect Swarm";
                bool result = false;

                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;

                    if (Self.IsBuffOnMe(Druid_ID.ShootingStars) && !Spell.IsOnCooldown("Starsurge")) return RunStatus.Success;
                    //if (Self.IsBuffOnMeLUA("Shooting Stars") && !Spell.IsOnCooldown("Starsurge")) return RunStatus.Success;
                    if (!unit.Combat) continue;
                    if (unit.Dead) continue;
                    //if (unit.Auras.ContainsKey(dpsSpell) && unit.Auras[dpsSpell].TimeLeft.TotalSeconds > 2) continue;
                    foreach (KeyValuePair<string, WoWAura> aura in unit.Auras)
                    {
                        if (aura.Value.SpellId == Druid_ID.InsectSwarm && aura.Value.TimeLeft.TotalSeconds > 4) continue;
                    }
                    if (!unit.InLineOfSightOCD) continue;
                    if (!unit.Attackable) continue;
                    if (unit.Pacified) continue;
                    if (unit.Distance > Spell.MaxDistance(dpsSpell)) continue;
                    if (!unit.IsTargetingMeOrPet && !unit.IsTargetingMyPartyMember && !unit.IsTargetingMyRaidMember) continue;
                    if (Utils.CrowdControlSpellsList.Any(s => unit.Auras.ContainsKey(s))) continue;
                    if (unit.Elite && unit.HealthPercent < 10 || !unit.Elite && unit.HealthPercent < 25) continue;
                    //if (unit.HealthPercent < 25 && !unit.Elite) continue;

                    string nameOfTarget = !unit.IsPlayer ? unit.Name : unit.Class.ToString();
                    Utils.Log("-AoE (Insect Swarm) dotting " + nameOfTarget);

                    Spell.Cast(dpsSpell, unit);
                    if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;
                    result = true;
                }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Always Face Moving Target
        public class NeedToAlwaysFaceMovingTarget : Decorator
        {
            public NeedToAlwaysFaceMovingTarget(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Settings.LazyRaider.Contains("always")) return false;
                if (Me.IsMoving) return false;
                if (Me.GotTarget && Me.CurrentTarget.IsMoving) return true;

                return false;
            }
        }

        public class AlwaysFaceMovingTarget : Action
        {
            protected override RunStatus Run(object context)
            {
                Target.Face();
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Hurricane
        public class NeedToHurricane : Decorator
        {
            public NeedToHurricane(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Hurricane";

                if (!Self.IsBuffOnMe(Druid_ID.ClearcastingBalance)) return false;
                if (!Utils.Adds) return false;
                if (!CLC.ResultOK(Settings.AOEDOTBalance)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if (RAF.PartyTankRole == null) return false;
                if (RAF.PartyTankRole.Distance > 29) return false;
                if (RAF.PartyTankRole.IsMoving) return false;
                if (Me.IsMoving) return false;

                if (Utils.CountOfAddsInRange(8, RAF.PartyTankRole.Location) < 3) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Hurricane : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Hurricane";
                bool result = Spell.Cast(dpsSpell, RAF.PartyTankRole.Location);


                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #endregion

        #region Restoration

        #region Moonfire Restoration
        public class NeedToMoonfireRestoration : Decorator
        {
            public NeedToMoonfireRestoration(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Moonfire";

                if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Restoration) return false;
                if (Me.IsInParty && Me.IsInInstance || Me.IsInRaid || Utils.IsBattleground ) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MoonfireRestoration : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Moonfire";

                if (!Me.IsSafelyFacing(CT.Location)) { Target.Face(); return RunStatus.Running; }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Wrath Restoration
        public class NeedToWrathRestoration : Decorator
        {
            public NeedToWrathRestoration(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Wrath";

                if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Restoration) return false;
                if (Me.IsInParty && Me.IsInInstance || Me.IsInRaid || Utils.IsBattleground) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class WrathRestoration : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Wrath";

                if (!Me.IsSafelyFacing(CT.Location)) { Target.Face(); return RunStatus.Running; }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Insect Swarm Restoration
        public class NeedToInsectSwarmRestoration : Decorator
        {
            public NeedToInsectSwarmRestoration(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Insect Swarm";

                if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Restoration) return false;
                if (Me.IsInParty && Me.IsInInstance || Me.IsInRaid || Utils.IsBattleground) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class InsectSwarmRestoration : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Insect Swarm";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Swiftmend Tank
        public class NeedToSwiftmendTank : Decorator
        {
            public NeedToSwiftmendTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Swiftmend";
                WoWUnit tank = RAF.PartyTankRole;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;

                bool hotFound = tank.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Rejuvenation || aura.Value.SpellId == Druid_ID.Regrowth);
                if (!hotFound) return false;

                //if (!tank.Auras.ContainsKey("Rejuvenation")) return false;
                //if (!tank.Auras.ContainsKey("Regrowth")) return false;
                if (!CLC.ResultOK(Settings.SwiftmendTank)) return false;
                if (tank.HealthPercent > Settings.SwiftmendTankHealth) return false;

                int healingWeight;
                int tankScore;
                bool overrideTank = false;
                if (overrideTank)
                if (!Settings.SwiftmendParty.Contains("never"))
                {
                    WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.SwiftmendPartyHealth);
                    if (player !=null && player.Guid != tank.Guid)
                    {
                        tankScore = Utils.HealingWeight(tank.ToPlayer());
                        if (tankScore > healingWeight)
                        {
                            Utils.Log( string.Format("-Overriding tank healing. {0} scored {1} while the tank's score is {2}", player.Class, healingWeight, tankScore)); 
                            return false;
                        }
                    }
                }
                if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;
                
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SwiftmendTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Swiftmend";
                WoWUnit tank = RAF.PartyTankRole;

                bool result = Spell.Cast(dpsSpell, tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Healing Touch Tank
        public class NeedToHealingTouchTank : Decorator
        {
            public NeedToHealingTouchTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Healing Touch";
                WoWUnit tank = RAF.PartyTankRole;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;
                if (!CLC.ResultOK(Settings.HealingTouchTank)) return false;
                if (tank.HealthPercent > Settings.HealingTouchTankHealth) return false;

                int healingWeight;
                int tankScore;
                bool overrideTank = false;
                if (overrideTank)
                if (!Settings.HealingTouchParty.Contains("never"))
                {
                    WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.HealingTouchPartyHealth);
                    if (player !=null && player.Guid != tank.Guid)
                    {
                        tankScore = Utils.HealingWeight(tank.ToPlayer());
                        if (tankScore > healingWeight)
                        {
                            Utils.Log( string.Format("-Overriding tank healing. {0} scored {1} while the tank's score is {2}", player.Class, healingWeight, tankScore)); 
                            return false;
                        }
                    }
                }
                if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HealingTouchTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Healing Touch";
                WoWUnit tank = RAF.PartyTankRole;

                if (tank.HealthPercent < 50)
                if (Spell.IsKnown("Nature's Swiftness") && Spell.CanCast("Nature's Swiftness"))
                {
                    Spell.Cast("Nature's Swiftness");
                    System.Threading.Thread.Sleep(100);
                }

                bool result = Spell.Cast(dpsSpell,tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Regrowth Tank
        public class NeedToRegrowthTank : Decorator
        {
            public NeedToRegrowthTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Regrowth";
                WoWUnit tank = RAF.PartyTankRole;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;
                //if (tank.Auras.ContainsKey(dpsSpell) && tank.Auras[dpsSpell].CreatorGuid == Me.Guid) return false;
                if (tank.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Regrowth && aura.Value.CreatorGuid == Me.Guid))
                {
                    return false;
                }
                //if (tank.Auras.ContainsKey(dpsSpell)) return false;
                if (!CLC.ResultOK(Settings.RegrowthTank)) return false;
                if (tank.HealthPercent > Settings.RegrowthTankHealth) return false;
                
                int healingWeight;
                int tankScore;
                bool overrideTank = false;
                if (overrideTank)
                if (!Settings.RegrowthParty.Contains("never"))
                {
                    WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.RegrowthPartyHealth);
                    if (player !=null && player.Guid != tank.Guid)
                    {
                        tankScore = Utils.HealingWeight(tank.ToPlayer());
                        if (tankScore > healingWeight)
                        {
                            Utils.Log( string.Format("-Overriding tank healing. {0} scored {1} while the tank's score is {2}", player.Class, healingWeight, tankScore));
                            return false;
                        }
                    }
                }
                if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RegrowthTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Regrowth";
                WoWUnit tank = RAF.PartyTankRole;

                bool result = Spell.Cast(dpsSpell, tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rejuvenation Tank
        public class NeedToRejuvenationTank : Decorator
        {
            public NeedToRejuvenationTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rejuvenation";
                WoWUnit tank = RAF.PartyTankRole;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;
                //if (tank.Auras.ContainsKey(dpsSpell) && tank.Auras[dpsSpell].CreatorGuid == Me.Guid) return false;
                if (tank.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Rejuvenation && aura.Value.CreatorGuid == Me.Guid))
                {
                    return false;
                }
                if (!CLC.ResultOK(Settings.RejuvenationTank)) return false;
                if (tank.HealthPercent > Settings.RejuvenationTankHealth) return false;

                int healingWeight;
                int tankScore;
                bool overrideTank = false;
                if (overrideTank)
                if (!Settings.RejuvenationParty.Contains("never"))
                {
                    WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.RejuvenationPartyHealth);
                    if (player !=null && player.Guid != tank.Guid)
                    {
                        tankScore = Utils.HealingWeight(tank.ToPlayer());
                        if (tankScore > healingWeight)
                        {
                            Utils.Log( string.Format("-Overriding tank healing. {0} scored {1} while the tank's score is {2}", player.Class, healingWeight, tankScore)); 
                            return false;
                        }
                    }
                }
                if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RejuvenationTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Rejuvenation";
                WoWUnit tank = RAF.PartyTankRole;

                bool result = Spell.Cast(dpsSpell, tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Nourish Tank
        public class NeedToNourishTank : Decorator
        {
            public NeedToNourishTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Nourish";
                WoWUnit tank = RAF.PartyTankRole;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;
                if (!CLC.ResultOK(Settings.NourishTank)) return false;
                if (tank.HealthPercent > Settings.NourishTankHealth) return false;

                int healingWeight;
                int tankScore;
                bool overrideTank = false;
                if (overrideTank)
                if (!Settings.NourishParty.Contains("never"))
                {
                    WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.NourishPartyHealth);
                    if (player !=null && player.Guid != tank.Guid)
                    {
                        tankScore = Utils.HealingWeight(tank.ToPlayer());
                        if (tankScore > healingWeight)
                        {
                            Utils.Log( string.Format("-Overriding tank healing. {0} scored {1} while the tank's score is {2}", player.Class, healingWeight, tankScore));
                            return false;
                        }
                    }
                }
                if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class NourishTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Nourish";
                WoWUnit tank = RAF.PartyTankRole;

                bool result = Spell.Cast(dpsSpell, tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Lifebloom Tank
        public class NeedToLifebloomTank : Decorator
        {
            public NeedToLifebloomTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                uint stackCount = 0;
                const string dpsSpell = "Lifebloom";
                WoWUnit tank = RAF.PartyTankRole;
                bool canCast = false;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;
                if (!Settings.LifebloomTank.Contains("keep up 3 stacks"))
                    if (!CLC.ResultOK(Settings.LifebloomTank)) return false;
                if (tank.HealthPercent > Settings.LifebloomTankHealth) return false;

                int healingWeight;
                int tankScore;
                if (!Settings.RegrowthParty.Contains("never"))
                {
                    WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.RegrowthPartyHealth);
                    if (player !=null && player.Guid != tank.Guid)
                    {
                        tankScore = Utils.HealingWeight(tank.ToPlayer());
                        if (tankScore > healingWeight)
                        {
                            Utils.Log( string.Format("-Overriding tank healing. {0} scored {1} while the tank's score is {2}", player.Class, healingWeight, tankScore));
                            return false;
                        }
                    }
                }
                //if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;
                bool hotFound = tank.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Lifebloom && aura.Value.CreatorGuid == Me.Guid);
                if (hotFound)
                {
                    bool okToCastLifebloom = false;
                    foreach (KeyValuePair<string, WoWAura> aura in tank.Auras)
                    {
                        if (aura.Value.SpellId != Druid_ID.Lifebloom) continue;
                        if (aura.Value.CreatorGuid != Me.Guid) continue;
                        //if (aura.Key != dpsSpell) continue;
                        //if (aura.Value.CreatorGuid != Me.Guid) continue;
                        
                        stackCount = aura.Value.StackCount;
                        if (stackCount < 3)
                        {
                            okToCastLifebloom = true;
                            break;
                        }
                        
                        if (stackCount == 3 && Settings.LifebloomTank.Contains("keep up 3 stacks"))
                        {
                            if (tank.Auras[dpsSpell].TimeLeft.TotalSeconds < 4)
                            {
                                okToCastLifebloom = true;
                                break;
                            }
                        }
                    }

                    if (!okToCastLifebloom) return false;
                }

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;


                canCast = Spell.CanCast(dpsSpell);

                //if (canCast) Utils.Log("-Lifebloom stack count = "+ stackCount);
                return (canCast);
            }
        }

        public class LifebloomTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Lifebloom";
                WoWUnit tank = RAF.PartyTankRole;

                bool result = Spell.Cast(dpsSpell,tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Thorns Tank
        public class NeedToThornsTank : Decorator
        {
            public NeedToThornsTank(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Thorns";
                WoWUnit tank = RAF.PartyTankRole;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (tank == null || tank.Dead) return false;
                if (!CLC.ResultOK(Settings.ThornsTankRestoration)) return false;
                //if (tank.Auras.ContainsKey(dpsSpell) && tank.Auras[dpsSpell].CreatorGuid == Me.Guid) return false;
                bool hotFound = tank.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Thorns);// && aura.Value.CreatorGuid == Me.Guid);
                if (hotFound) return false;

                if (ClassHelpers.Druid.RestorationSupport.UrgentPlayerHeal(Settings.MinTankHealth, Settings.MinTankHealthOverride)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!tank.InLineOfSightOCD) return false;
                if (tank.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ThornsTank : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Thorns";
                WoWUnit tank = RAF.PartyTankRole;

                bool result = Spell.Cast(dpsSpell, tank);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion


        #region Swiftmend Party
        public class NeedToSwiftmendParty : Decorator
        {
            public NeedToSwiftmendParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Swiftmend";
                if (Me.IsCasting || Spell.IsGCD) return false;
                if (!Spell.IsKnown(dpsSpell)) return false;
                if (Spell.IsOnCooldown(dpsSpell)) return false;

                //List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                //WoWUnit player = (from o in players let p = o.ToPlayer() where p.Distance < 80 && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.SwiftmendPartyHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
                int healingWeight;
                WoWUnit player = Utils.BestHealTarget(out healingWeight,Settings.SwiftmendPartyHealth);

                if (player == null) return false;

                //if (!player.Auras.ContainsKey("Rejuvenation")) return false;
                //if (!player.Auras.ContainsKey("Regrowth")) return false;
                bool hotFound = player.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Rejuvenation || aura.Value.SpellId == Druid_ID.Regrowth);
                if (!hotFound) return false;

                if (!CLC.ResultOK(Settings.SwiftmendParty)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!player.InLineOfSightOCD) return false;
                if (player.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                ClassHelpers.Druid.RestorationSupport.HealTarget = player;

                return true;
            }
        }

        public class SwiftmendParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Swiftmend";
                bool result = Spell.Cast(dpsSpell,ClassHelpers.Druid.RestorationSupport.HealTarget);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Regrowth Party
        public class NeedToRegrowthParty : Decorator
        {
            public NeedToRegrowthParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Regrowth";
                if (Me.IsCasting || Spell.IsGCD) return false;

                //List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                //WoWUnit player = (from o in players let p = o.ToPlayer() where p.Distance < 80 && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.RegrowthPartyHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
                int healingWeight;
                WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.RegrowthPartyHealth);

                if (player == null) return false;

                //if (player.Auras.ContainsKey(dpsSpell) && player.Auras[dpsSpell].CreatorGuid == Me.Guid) return false;
                bool hotFound = player.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Thorns && aura.Value.CreatorGuid == Me.Guid);
                if (hotFound) return false;

                if (!CLC.ResultOK(Settings.RegrowthParty)) return false;
                if (player.HealthPercent > Settings.RegrowthPartyHealth) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!player.InLineOfSightOCD) return false;
                if (player.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                ClassHelpers.Druid.RestorationSupport.HealTarget = player;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RegrowthParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Regrowth";
                bool result = Spell.Cast(dpsSpell, ClassHelpers.Druid.RestorationSupport.HealTarget);
                //if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Nourish Party
        public class NeedToNourishParty : Decorator
        {
            public NeedToNourishParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Nourish";
                if (Me.IsCasting || Spell.IsGCD) return false;
                if (Settings.NourishParty.Contains("never")) return false;

                //List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                //WoWUnit player = (from o in players let p = o.ToPlayer() where p.Distance < 80 && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.NourishPartyHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
                int healingWeight;
                WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.NourishPartyHealth);

                if (player == null) return false;
                bool isCaster = (player.Class == WoWClass.Mage || player.Class == WoWClass.Priest || player.Class == WoWClass.Warlock || player.Class == WoWClass.Shaman || player.Class == WoWClass.Druid && player.Shapeshift == ShapeshiftForm.Normal);
                if (Settings.NourishParty.Contains("only on cloth") && !isCaster) return false;

                //if (!CLC.ResultOK(Settings.NourishParty)) return false;
                if (player.HealthPercent > Settings.NourishPartyHealth) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!player.InLineOfSightOCD) return false;
                if (player.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                ClassHelpers.Druid.RestorationSupport.HealTarget = player;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class NourishParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Nourish";
                bool result = Spell.Cast(dpsSpell, ClassHelpers.Druid.RestorationSupport.HealTarget);
                //if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rejuvenation Party
        public class NeedToRejuvenationParty : Decorator
        {
            public NeedToRejuvenationParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rejuvenation";
                if (Me.IsCasting || Spell.IsGCD) return false;

                //List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                //WoWUnit player = (from o in players let p = o.ToPlayer() where p.Distance < 80 && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.RejuvenationPartyHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
                int healingWeight;
                WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.RejuvenationPartyHealth);

                if (player == null) return false;

                bool hotFound = player.Auras.Any(aura => aura.Value.SpellId == Druid_ID.Rejuvenation && aura.Value.CreatorGuid == Me.Guid);
                if (hotFound) return false;
                //if (player.Auras.ContainsKey("Rejuvenation") && player.Auras[dpsSpell].CreatorGuid == Me.Guid) return false;
                if (!CLC.ResultOK(Settings.RejuvenationParty)) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!player.InLineOfSightOCD) return false;
                if (player.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                ClassHelpers.Druid.RestorationSupport.HealTarget = player;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RejuvenationParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Rejuvenation";
                bool result = Spell.Cast(dpsSpell, ClassHelpers.Druid.RestorationSupport.HealTarget);
                

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Healing Touch Party
        public class NeedToHealingTouchParty : Decorator
        {
            public NeedToHealingTouchParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Healing Touch";
                if (Me.IsCasting || Spell.IsGCD) return false;
                if (Settings.HealingTouchParty.Contains("never")) return false;

                //List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                //WoWUnit player = (from o in players let p = o.ToPlayer() where p.Distance < 80 && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.HealingTouchPartyHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
                int healingWeight;
                WoWUnit player = Utils.BestHealTarget(out healingWeight, Settings.HealingTouchPartyHealth);

                if (player == null) return false;
                bool isCaster = (player.Class == WoWClass.Mage || player.Class == WoWClass.Priest || player.Class == WoWClass.Warlock || player.Class == WoWClass.Shaman || player.Class == WoWClass.Druid && player.Shapeshift == ShapeshiftForm.Normal);
                if (Settings.HealingTouchParty.Contains("non-clothies") && isCaster) return false;

                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON
                if (!player.InLineOfSightOCD) return false;
                if (player.Distance > Spell.MaxDistance(dpsSpell)) return false;
                // IGNORE THESE FOR NOW - BUT WILL NEED TO DO SOMETHING ABOUT IT LATER ON

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                ClassHelpers.Druid.RestorationSupport.HealTarget = player;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HealingTouchParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Healing Touch";
                if (Spell.IsKnown("Nature's Swiftness") && Spell.CanCast("Nature's Swiftness"))
                {
                    Spell.Cast("Nature's Swiftness");
                    System.Threading.Thread.Sleep(100);
                }
                bool result = Spell.Cast(dpsSpell, ClassHelpers.Druid.RestorationSupport.HealTarget);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion


        #region Wild Growth
        public class NeedToWildGrowth : Decorator
        {
            public NeedToWildGrowth(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Wild Growth";

                if (Settings.WildGrowth.Contains("never")) return false;
                if (Me.IsCasting || Spell.IsGCD) return false;

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                List<WoWPlayer> AoE = (from o in players let p = o.ToPlayer() where p.Distance < Spell.MaxDistance(dpsSpell) && p.InLineOfSightOCD && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.WildGrowthHealth orderby p.HealthPercent ascending select p).ToList();
                
                if (AoE.Count <= 1) return false;
                if (Settings.WildGrowth.Contains("2+") && AoE.Count < 2) return false;
                if (Settings.WildGrowth.Contains("3+") && AoE.Count < 3) return false;
                if (Settings.WildGrowth.Contains("4+") && AoE.Count < 4) return false;
                if (Settings.WildGrowth.Contains("5+") && AoE.Count < 5) return false;

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                ClassHelpers.Druid.RestorationSupport.HealTarget = AoE.FirstOrDefault();

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class WildGrowth : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Wild Growth";
                bool result = Spell.Cast(dpsSpell, ClassHelpers.Druid.RestorationSupport.HealTarget);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Tranquility
        public class NeedToTranquility : Decorator
        {
            public NeedToTranquility(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Tranquility";

                if (Settings.Tranquility.Contains("never")) return false;
                if (Me.IsCasting || Spell.IsGCD) return false;
                if (Self.Immobilised) return false;

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                List<WoWPlayer> AoE = (from o in players let p = o.ToPlayer() where p.Distance < Spell.MaxDistance(dpsSpell) && p.InLineOfSightOCD && !p.Dead && !p.IsGhost && p.HealthPercent < Settings.TranquilityHealth orderby p.HealthPercent ascending select p).ToList();

                if (AoE.Count <= 1) return false;
                if (Settings.Tranquility.Contains("2+") && AoE.Count < 2) return false;
                if (Settings.Tranquility.Contains("3+") && AoE.Count < 3) return false;
                if (Settings.Tranquility.Contains("4+") && AoE.Count < 4) return false;
                if (Settings.Tranquility.Contains("5+") && AoE.Count < 5) return false;

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Tranquility : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Tranquility";
                bool result = Spell.Cast(dpsSpell);
                //if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Remove Corruption Party
        public class NeedToRemoveCorruptionParty : Decorator
        {
            public NeedToRemoveCorruptionParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                bool canDispel = false;
                const string dpsSpell = "Remove Corruption";
                
                if (Settings.RemoveCorruptionRestoration.Contains("never")) return false;
                if (Settings.RemoveCorruptionRestoration.Contains("out of combat")) return false;

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Me.IsInParty && !Me.IsInRaid) return false;

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers; players.Add(Me);
                foreach (WoWPlayer player in players)
                {
                    canDispel = player.Auras.Where(aura => aura.Value.IsHarmful).Any(aura => aura.Value.Spell.DispelType == WoWDispelType.Poison || aura.Value.Spell.DispelType == WoWDispelType.Curse || aura.Value.Spell.DispelType == WoWDispelType.Magic);
                    if (canDispel)
                    {
                        break;
                    }
                }

                return (Spell.CanCast(dpsSpell) && canDispel);
            }
        }

        public class RemoveCorruptionParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Remove Corruption";

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                foreach (WoWPlayer player in players)
                {
                    bool canDispel = player.Auras.Where(aura => aura.Value.IsHarmful).Any(aura => aura.Value.Spell.DispelType == WoWDispelType.Poison || aura.Value.Spell.DispelType == WoWDispelType.Curse || aura.Value.Spell.DispelType == WoWDispelType.Magic);
                    if (canDispel)
                    {
                        bool result = Spell.Cast(dpsSpell, player);
                        return result ? RunStatus.Success : RunStatus.Failure;
                    }
                }

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Innervate Party
        public class NeedToInnervateParty : Decorator
        {
            public NeedToInnervateParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Innervate";

                if (Self.Immobilised) return false;
                if (Me.ManaPercent > Settings.InnervateRestoration) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class InnervateParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Innervate";
                bool result = Spell.Cast(dpsSpell,Me);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Barkskin Party
        public class NeedToBarkskinParty : Decorator
        {
            public NeedToBarkskinParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Barkskin";
                bool result = false;

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (Settings.BarkskinRestoration.Contains("never")) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Settings.BarkskinRestoration.Contains("aggro"))
                {
                    foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>(false,false))
                    {
                        if (!unit.Combat) continue;
                        if (unit.CurrentTargetGuid != Me.Guid)continue;
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }

                return (Spell.CanCast(dpsSpell) && result);
            }
        }

        public class BarkskinParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Barkskin";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rebirth
        public class NeedToRebirth : Decorator
        {
            public NeedToRebirth(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rebirth";

                if (Me.IsCasting || Spell.IsGCD) return false;
                if (!CLC.ResultOK(Settings.RebirthRestoration)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                bool canCast = players.Where(player => player.Dead).Where(player => player.Distance <= Spell.MaxDistance(dpsSpell)).Any(player => player.InLineOfSightOCD);

                return (Spell.CanCast(dpsSpell) && canCast);
            }
        }

        public class Rebirth : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = false;
                const string dpsSpell = "Rebirth";

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                foreach (WoWPlayer player in players)
                {
                    if (!player.Dead) continue;
                    if (player.Distance > Spell.MaxDistance(dpsSpell)) continue;
                    if (!player.InLineOfSightOCD) continue;

                    result = Spell.Cast(dpsSpell, player);
                    return result ? RunStatus.Success : RunStatus.Failure;
                }


                return RunStatus.Failure;
            }
        }
        #endregion

        #region HealPets
        public class NeedToHealPets : Decorator
        {
            public NeedToHealPets(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                bool healPet = false;
                const string dpsSpell = "Regrowth";

                if (!CLC.ResultOK(Settings.HealPets)) return false;
                if (!Self.IsPowerPercentAbove(80)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                List<WoWUnit> pets = new List<WoWUnit>();

                foreach (WoWPlayer player in players)
                {
                    if (player.HealthPercent < 80) return false;
                    if (!player.GotAlivePet) continue;
                    if (player.Class == WoWClass.Warlock) continue;
                    if (player.Pet.HealthPercent > Settings.PetHealth) continue;
                    healPet = true;
                }

                return (Spell.CanCast(dpsSpell) && healPet);
            }
        }

        public class HealPets : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = false;
                const string dpsSpell = "Regrowth";

                List<WoWPlayer> players = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                List<WoWUnit> pets = new List<WoWUnit>();

                foreach (WoWPlayer player in players)
                {
                    if (!player.GotAlivePet) continue;
                    if (player.Class == WoWClass.Warlock) continue;
                    if (player.Pet.HealthPercent > Settings.PetHealth) continue;

                    result = Spell.Cast(dpsSpell,player.Pet);
                    return result ? RunStatus.Success : RunStatus.Failure;
                }

                

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Tree Of Life
        public class NeedToTreeOfLife : Decorator
        {
            public NeedToTreeOfLife(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                bool canCast = false;
                const string dpsSpell = "Tree of Life";

                if (Spell.IsGCD || Me.IsCasting) return false;
                if (Settings.TreeOfLifeRestoration.Contains("never")) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Me.IsInRaid && !Me.IsInInstance) return false;
                if (!Spell.IsKnown(dpsSpell)|| Spell.IsOnCooldown(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class TreeOfLife : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Tree of Life";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion
      


      
        #endregion






        #region Shapeshift Moonkin
        public class NeedToShapeshiftMoonkin : Decorator
        {
            public NeedToShapeshiftMoonkin(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = "Moonkin Form";

                if (ClassHelpers.Druid.Shapeshift.IsMoonkinForm) return false;
                if (ClassHelpers.Druid.Shapeshift.IsCatForm && Me.ManaPercent < (Settings.CatFormManaBalance * 3)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;

                if (Utils.Adds)
                {
                    if (!Self.IsHealthPercentAbove(Settings.RejuvenationBalanceHealth)) return false;
                    if (!Self.IsHealthPercentAbove(Settings.RegrowthBalanceHealth)) return false;
                    if (!Self.IsHealthPercentAbove(Settings.NourishBalanceHealth)) return false;
                }

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShapeshiftMoonkin : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = "Moonkin Form";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();
                bool result = true;//Self.IsBuffOnMe(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shapeshift Bear
        public class NeedToShapeshiftBear : Decorator
        {
            public NeedToShapeshiftBear(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = "Bear Form";

                if (Settings.BearForm.Contains("never")) return false;
                if (ClassHelpers.Druid.Shapeshift.IsBearForm) return false;
                if (Spell.IsKnown("Moonkin Form")) return false;
                if (Settings.BearForm.Contains("low health") && Self.IsHealthAbove(Settings.BearFormHealth)) return false;
                if (Settings.BearForm.Contains("2+ adds") && !Utils.Adds) return false;
                if (Settings.BearForm.Contains("3+ adds") && Utils.AddsCount < 3) return false;
                if (Settings.BearForm.Contains("4+ adds") && Utils.AddsCount < 4) return false;
                if (Settings.BearForm.Contains("5+ adds") && Utils.AddsCount < 5) return false;
                if (Settings.BearForm.Contains("6+ adds") && Utils.AddsCount < 6) return false;
                if (Settings.BearForm.Contains("7+ adds") && Utils.AddsCount < 7) return false;

                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShapeshiftBear : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = "Bear Form";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = true;//Self.IsBuffOnMe(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shapeshift Cat
        public class NeedToShapeshiftCat : Decorator
        {
            public NeedToShapeshiftCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = "Cat Form";

                if (ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Restoration) return false;
                if (ClassHelpers.Druid.Shapeshift.IsCatForm) return false;
                if (Settings.BearForm.Contains("always")) return false;
                if (Settings.BearForm.Contains("low health") && !Self.IsHealthAbove(Settings.BearFormHealth)) return false;
                if (Settings.BearForm.Contains("2+ adds") && Utils.Adds) return false;
                if (Settings.BearForm.Contains("3+ adds") && Utils.AddsCount >= 3) return false;
                if (Settings.BearForm.Contains("4+ adds") && Utils.AddsCount >= 4) return false;
                if (Settings.BearForm.Contains("5+ adds") && Utils.AddsCount >= 5) return false;
                if (Settings.BearForm.Contains("6+ adds") && Utils.AddsCount >= 6) return false;
                if (Settings.BearForm.Contains("7+ adds") && Utils.AddsCount >= 7) return false;
                if (ClassHelpers.Druid.Shapeshift.IsBearForm && !Utils.Adds && Self.IsPowerPercentAbove(10)) return false;
                if ((Spell.IsKnown("Moonkin Form") || ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Balance || ClassHelpers.Druid.ClassSpec == ClassHelpers.Druid.ClassType.Untalented) && Me.ManaPercent > (Settings.CatFormManaBalance)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShapeshiftCat : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = "Cat Form";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = true;//Self.IsBuffOnMe(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

      

        #endregion

    }
}