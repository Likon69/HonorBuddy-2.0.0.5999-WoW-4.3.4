using System.Collections.Generic;
using System.Linq;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

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
                 // All spells are checked in the order listed below, a prioritised order
                 // Put the most important spells at the top of the list

                 // TimerTest
                 //new NeedToTimerTest(new TimerTest()),

                 // Make sure we always have a target. If we don't have a target, take the pet's target - if they have one.
                 //new Decorator(ret => !Me.GotTarget && Me.GotAlivePet && Me.Pet.GotTarget, new Action(ret => Me.Pet.CurrentTarget.Target())),

                 // Check if we get aggro during the pull
                 // This is in here and not the pull because we are in combat at this point
                 new NeedToCheckAggroOnPull(new CheckAggroOnPull()),

                 // Move Closer
                 //new NeedToMoveCloser(new MoveCloser()),

                 // Fleeing Attack
                 new NeedToFleeingAttack(new FleeingAttack()),

                 // Move To
                 new NeedToMoveTo(new MoveTo()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Interact Other
                 new NeedToInteractOther(new InteractOther()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Summon Pet
                 new NeedToSummonPet(new SummonPet()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Interact - Face the target
                //new NeedToInteract(new Interact()),

                 // Move Stop
                 //new NeedToMoveStop(new MoveStop()),

                 // Dark Command - Fleeing targets only
                 new NeedToDarkCommandFleeing(new DarkCommandFleeing()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Retarget
                 new NeedToRetarget(new Retarget()),

                 // Abort combat is the target's health is 95% + after 30 seconds of combat
                 new NeedToCheckCombatTimer(new CheckCombatTimer()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // LOS Check
                 new NeedToLOSCheck(new LOSCheck()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Auto Attack
                new NeedToAutoAttack(new AutoAttack()),

                // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                // Backup
                new NeedToBackup(new Backup()),

                // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Presence
                 new NeedToPresence(new Presence()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Frost Strike
                 new NeedToFrostStrike(new FrostStrike()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // ************************************************************************************
                 // Important/time sensative spells here
                 // These are spells that need to be case asap

                 // Raise Ally
                 new NeedToRaiseAlly(new RaiseAlly()),

                 // Empower Rune Weapon
                 new NeedToEmpowerRuneWeapon(new EmpowerRuneWeapon()),

                 // Outbreak
                 new NeedToOutbreak(new Outbreak()),

                 // Blood Tap
                 new NeedToBloodTap(new BloodTap()),

                 // Death Strike
                 new NeedToDeathStrike(new DeathStrike()),

                 // Dark Transformation
                 new NeedToDarkTransformation(new DarkTransformation()),

                 // Sudden Doom - PROC
                 new NeedToSuddenDoom(new SuddenDoom()),

                 // Crimson Scourge PROC
                 new NeedToCrimsonScourge(new CrimsonScourge()),

                 // Freezing Fog PROC
                 new NeedToFreezingFog(new FreezingFog()),

                 // Killing Machine PROC - Obliterate
                 new NeedToKillingMachine(new KillingMachine()),

                 // Killing Machine PROC - Frost Strike
                 new NeedToKillingMachineFrostStrike(new KillingMachineFrostStrike()),


                 // TimerTest2============================================
                 //new NeedToTimerTest2(new TimerTest2()),


                 // Festering Strike
                 new NeedToFesteringStrike(new FesteringStrike()),

                 // Chains Of Ice
                 new NeedToChainsOfIce(new ChainsOfIce()),

                 // Death And Decay
                 new NeedToDeathAndDecay(new DeathAndDecay()),
                 
                 // Pet Attack
                 new NeedToPetAttack(new PetAttack()),

                 // Raise Dead - NON-unholy spec only
                 // Unholy spec pet is used elsewhere and is permenant
                 new NeedToRaiseDead(new RaiseDead()),

                 // Bone Shield
                 new NeedToBoneShield(new BoneShield()),

                 // Rune Tap
                 new NeedToRuneTap(new RuneTap()),

                 // Mind Freeze
                 new NeedToMindFreeze(new MindFreeze()),

                 // Strangulate
                 new NeedToStrangulate(new Strangulate()),

                 // TimerTest2============================================
                 //new NeedToTimerTest2(new TimerTest2()),


                 // Unholy Frenzy
                 new NeedToUnholyFrenzy(new UnholyFrenzy()),

                 // Diseases - Make sure Blood Plauge and Frost Fever are applied
                 new NeedToDiseases(new Diseases()),

                 // Pestilence
                 new NeedToPestilence(new Pestilence()),

                 // Blood Boil
                 new NeedToBloodBoil(new BloodBoil()),
                 
                 // Anti Magic Shell
                 new NeedToAntiMagicShell(new AntiMagicShell()),

                 // Heart Strike
                 new NeedToHeartStrike(new HeartStrike()),


                 // ************************************************************************************
                 // Other spells here

                 // TimerTest2============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 // Pillar Of Frost
                 new NeedToPillarOfFrost(new PillarOfFrost()),

                 // Rune Weapon
                 new NeedToRuneWeapon(new RuneWeapon()),

                 // Gargoyle
                 new NeedToGargoyle(new Gargoyle()),

                 // Howling Blast
                 new NeedToHowlingBlast(new HowlingBlast()),

                 // Frost Strike
                 new NeedToFrostStrike(new FrostStrike()),

                 // Death Coil
                 new NeedToDeathCoil(new DeathCoil()),

                 // Obliterate
                 new NeedToObliterate(new Obliterate()),

                 // TimerTest2 ============================================
                 //new NeedToTimerTest2(new TimerTest2()),

                 
                 // Plague Strike
                 new NeedToPlagueStrike(new PlagueStrike()),
                 
                 // Frost Strike
                 new NeedToFrostStrike(new FrostStrike()),
                 
                 // Icy Touch
                 new NeedToIcyTouch(new IcyTouch()),
                 
                 // Scourge Strike
                 new NeedToScourgeStrike(new ScourgeStrike()),
                 
                 // Blood Strike
                 new NeedToBloodStrike(new BloodStrike()),

                 // TimerTest3 ============================================
                 //new NeedToTimerTest3(new TimerTest3()),

                 // Finally just perform an update
                 new Action(ret => ObjectManager.Update())

                 );
         }
        #endregion
        
       


        #region Behaviours

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

        
        #region Retarget
        public class NeedToRetarget : Decorator
        {
            public NeedToRetarget(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Me.GotTarget && CT.IsAlive) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                //if (Me.IsInInstance) return false;
                //if (!Me.Combat) return false;

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

        #region Procs - The Art of War and Denounce
        public class NeedToProcs : Decorator
        {
            public NeedToProcs(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Exorcism";
                List<int> procList = new List<int> { 85509,96287,59578 };

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if ((from aura in Me.ActiveAuras from procID in procList where procID == aura.Value.SpellId select aura).Any()) return (Spell.CanCast(dpsSpell));

                return false;
            }
        }

        public class Procs : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Exorcism";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Interact Other
        public class NeedToInteractOther : Decorator
        {
            public NeedToInteractOther(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Target.IsFleeing) return true;
                if (!Timers.Expired("Interact", 500)) return false;
                if (!Me.GotTarget) return false;
                //if (Me.IsMoving) return false;
                if (Target.IsDistanceMoreThan(CT.InteractRange * 1.5f)) return false;
                if (Me.IsSafelyFacing(CT.Location) && !Utils.IsBattleground) return false;
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

                if (!Timers.Expired("DistanceCheck", 1000)) return false;

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
                Utils.Log("Backup a wee bit, too close to our target", Utils.Colour("Blue"));
                Timers.Reset("DistanceCheck");

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Rune Count Spam
        public class NeedToRuneCount : Decorator
        {
            public NeedToRuneCount(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Timers.Expired("Spam",1500)) return false;

                Utils.Log("** Blood:" + Me.BloodRuneCount);
                Utils.Log("** Unholy:" + Me.UnholyRuneCount);
                Utils.Log("** Frost:" + Me.FrostRuneCount);

                Timers.Reset("Spam");
                
                return false;
            }
        }

        public class RuneCount : Action
        {
            protected override RunStatus Run(object context)
            {
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


        // Combat

        #region Frost Strike

        public class NeedToFrostStrike : Decorator
        {
            public NeedToFrostStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Frost Strike";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.FrostStrike)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.RunicPowerPercent < Settings.FrostStrikeRunePower) return false;
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;

                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class FrostStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Frost Strike";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Icy Touch

        public class NeedToIcyTouch : Decorator
        {
            public NeedToIcyTouch(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Icy Touch";

                if (Self.Immobilised) return false;
                if (!Timers.Expired("Ice", Settings.AttackCooldown)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Settings.IcyTouch.Contains("Frost Fever") && !CLC.ResultOK(Settings.IcyTouch)) return false;
                if (Settings.IcyTouch.Contains("Frost Fever") && Target.IsDebuffOnTarget("Frost Fever")) return false;
                
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                
                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class IcyTouch : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Icy Touch";
                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Ice");
                

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Plague Strike

        public class NeedToPlagueStrike : Decorator
        {
            public NeedToPlagueStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Plague Strike";


                if (Self.Immobilised) return false;
                if (!Timers.Expired("Unholy", Settings.AttackCooldown)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Settings.PlagueStrike.Contains("Blood Plague") && !CLC.ResultOK(Settings.PlagueStrike)) return false;
                if (Settings.PlagueStrike.Contains("Blood Plague") && Target.IsDebuffOnTarget("Blood Plague")) return false;
                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;

                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class PlagueStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Plague Strike";

                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Unholy");
                
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(dpsSpell);
                //bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Blood Strike
        public class NeedToBloodStrike : Decorator
        {
            public NeedToBloodStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Blood Strike";
                int countOfDiseases = 0;

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Timers.Expired("Blood", Settings.AttackCooldown)) return false;
                
                //if (!CLC.ResultOK(Settings.BloodStrike)) return false;
                if (!Settings.BloodStrike.Contains("disease") && !Settings.BloodStrike.Contains("2x blood")) if (!CLC.ResultOK(Settings.BloodStrike)) return false;
                if (Settings.BloodStrike.Contains("disease"))
                {
                    if (Target.IsDebuffOnTarget("Frost Fever")) countOfDiseases = countOfDiseases + 1;
                    if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;

                    if (Settings.BloodStrike.Contains("1+ disease") && countOfDiseases < 1) return false;
                    if (Settings.BloodStrike.Contains("2+ disease") && countOfDiseases < 2) return false;
                }

                if (Settings.BloodStrike.Contains("2x blood"))
                {
                    if (Me.BloodRuneCount < 2 ) return false;
                }
                else
                {
                    //if (Target.IsDistanceMoreThan(Target.InteractRange + 1)) return false;
                    if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                }

                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class BloodStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Blood Strike";

                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Blood");
                
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(dpsSpell);
                //bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Scourge Strike
        public class NeedToScourgeStrike : Decorator
        {
            public NeedToScourgeStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Scourge Strike";
                int countOfDiseases = 0;

                if (Self.Immobilised) return false;
                if (!Timers.Expired("Unholy", Settings.AttackCooldown)) return false;
                //if (!CLC.ResultOK(Settings.ScourgeStrike)) return false;
                //if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                //if (Target.IsDistanceMoreThan(Target.InteractRange + 1)) return false;

                if (!Settings.ScourgeStrike.Contains("disease")) if (!CLC.ResultOK(Settings.ScourgeStrike)) return false;
                if (Settings.ScourgeStrike.Contains("disease"))
                {
                    if (Target.IsDebuffOnTarget("Frost Fever")) countOfDiseases = countOfDiseases + 1;
                    if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;
                    //if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;

                    if (Settings.ScourgeStrike.Contains("1+ disease") && countOfDiseases < 1) return false;
                    if (Settings.ScourgeStrike.Contains("2+ disease") && countOfDiseases < 2) return false;
                    //if (Settings.Pestilence.Contains("3+ disease") && countOfDiseases < 3) return false;
                }

                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;

                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class ScourgeStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Scourge Strike";

                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Unholy");
                

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Heart Strike
        public class NeedToHeartStrike : Decorator
        {
            public NeedToHeartStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Heart Strike";
                bool result = false;

                //Utils.Log("*********** step 1");
                if (Self.Immobilised) return false;
                //Utils.Log("*********** step 2");
                if (!CLC.ResultOK(Settings.HeartStrike)) return false;
                //Utils.Log("*********** step 3");
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                //Utils.Log("*********** step 4");
                //if (Target.IsDistanceMoreThan(Target.InteractRange + 1)) return false;
                if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                //Utils.Log("*********** step 5");

                result= (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
                //Utils.Log("*********** step 6");
                return result;
            }
        }

        public class HeartStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Heart Strike";
                //Utils.Log("*********** GO GO GO!");

                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Death Coil
        public class NeedToDeathCoil : Decorator
        {
            public NeedToDeathCoil(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Death Coil";

                if (Self.Immobilised) return false;
                //if (Me.RunicPowerPercent < 40) return false;
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;
                if (Me.RunicPowerPercent < Settings.DeathCoil) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                
                //if (Target.IsDistanceMoreThan(Target.InteractRange + 1)) return false;
		
                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class DeathCoil : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Death Coil";

                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Death Strike
        public class NeedToDeathStrike : Decorator
        {
            public NeedToDeathStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Death Strike";

                if (Self.Immobilised) return false;
                if (Self.IsHealthPercentAbove(Settings.DeathStrikeHealth)) return false;
                if (!CLC.ResultOK(Settings.DeathStrike)) return false;
                if (!Timers.Expired("DeathStrike", 6000)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange)) return false;
                if (Me.UnholyRuneCount < 1 && Me.FrostRuneCount < 1 && Me.DeathRuneCount < 2) return false;
                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;

                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
                //return (Spell.CanCast(dpsSpell));
            }
        }

        public class DeathStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Death Strike";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("DeathStrike");
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(dpsSpell);
                //bool result = Self.IsBuffOnMe(dpsSpell););

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mind Freeze
        public class NeedToMindFreeze : Decorator
        {
            public NeedToMindFreeze(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mind Freeze";

                if (Self.Immobilised) return false;
                if (!Target.IsCasting) return false;
                if (!CLC.ResultOK(Settings.MindFreeze)) return false;
                if (!Timers.Expired("Silence",3000)) return false;
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;
                //if (!CLC.ResultOK(Settings.MindFreeze)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Target.InteractRange + 1)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MindFreeze : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mind Freeze";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Silence");
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(dpsSpell);
                //bool result = Self.IsBuffOnMe(dpsSpell););

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Strangulate
        public class NeedToStrangulate : Decorator
        {
            public NeedToStrangulate(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Strangulate";
                WoWUnit silenceTarget = null;

                if (Self.Immobilised) return false;
                if (!Timers.Expired("Silence", 3000)) return false;
                if (!CLC.ResultOK(Settings.Strangulate)) return false;
                if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if (Utils.Adds)
                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (unit.Distance > Spell.MaxDistance(dpsSpell)) continue;
                    if (!unit.Combat) continue;
                    if (!unit.IsCasting) continue;
                    if (!unit.GotTarget) continue;
                    if (unit.CurrentTargetGuid != Me.Guid) continue;

                    silenceTarget = unit;
                    break;
                }

                if (silenceTarget == null)
                {
                    if (!Target.IsCasting) return false;
                    if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                }


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Strangulate : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Strangulate";
                WoWUnit silenceTarget = CT;

                if (!Target.IsCasting)
                {
                    foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                    {
                        if (unit.Distance > Spell.MaxDistance(dpsSpell)) continue;
                        if (!unit.Combat) continue;
                        if (!unit.IsCasting) continue;
                        if (!unit.GotTarget) continue;
                        if (unit.CurrentTargetGuid != Me.Guid) continue;

                        silenceTarget = unit;
                        break;
                    }
                }

                bool result = Spell.Cast(dpsSpell, silenceTarget);
                Timers.Reset("Silence");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Unholy Frenzy
        public class NeedToUnholyFrenzy : Decorator
        {
            public NeedToUnholyFrenzy(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Unholy Frenzy";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.UnholyFrenzy)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class UnholyFrenzy : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Unholy Frenzy";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mob Attacking Me
        public class NeedToMobAttackingMe : Decorator
        {
            public NeedToMobAttackingMe(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {

                if (!Me.GotAlivePet) return false;
                if (!Me.GotTarget) return false;
                //if (Me.Pet.CurrentTarget.CurrentTargetGuid == Me.CurrentTargetGuid) return false;
                return (Utils.Adds);
            }
        }

        public class MobAttackingMe : Action
        {
            protected override RunStatus Run(object context)
            {
                ClassHelpers.Hunter.Pet.DefendMe();
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Summon Pet
        public class NeedToSummonPet : Decorator
        {
            public NeedToSummonPet(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Self.Immobilised) return false;
                if (Me.GotAlivePet) return false;
                if (!CLC.ResultOK(Settings.RaiseDead)) return false;
                if (!Timers.Expired("PetCheck",1000)) return false;

                ObjectManager.Update();
                return ClassHelpers.DeathKnight.Pet.NeedToCallPet;
            }
        }

        public class SummonPet : Action
        {
            protected override RunStatus Run(object context)
            {
                Timers.Reset("PetCheck");
                ClassHelpers.DeathKnight.Pet.CallPet();
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Pet Attack
        public class NeedToPetAttack : Decorator
        {
            public NeedToPetAttack(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Utils.CombatCheckOk("", false)) return false;
                if (!Me.GotAlivePet) return false;
                if (!Me.GotTarget) return false;

                if (Me.GotTarget && !Me.Pet.GotTarget) return true;
                if (Me.Pet.CurrentTargetGuid != Me.CurrentTargetGuid) return true;

                return false;
            }
        }

        public class PetAttack : Action
        {
            protected override RunStatus Run(object context)
            {
                Lua.DoString("PetAttack()");
                System.Threading.Thread.Sleep(250);

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Bone Shield
        public class NeedToBoneShield : Decorator
        {
            public NeedToBoneShield(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Bone Shield";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.BoneShield)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.BoneShieldHealth)) return false;
                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BoneShield : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Bone Shield";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rune Tap
        public class NeedToRuneTap : Decorator
        {
            public NeedToRuneTap(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rune Tap";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.RuneTap)) return false;
                if (Self.IsHealthPercentAbove(Settings.RuneTapHealth)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RuneTap : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Rune Tap";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Death and Decay
        public class NeedToDeathAndDecay : Decorator
        {
            public NeedToDeathAndDecay(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Death and Decay";
                
                if (!Me.GotTarget) return false;
                if (Self.Immobilised) return false; 
                if (Me.IsMoving || CT.IsMoving) return false;
                if (!CLC.ResultOK(Settings.DeathAndDecay)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;
		
                //return (Spell.CanCast(dpsSpell));
                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
            }
        }

        public class DeathAndDecay : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Death and Decay";
                bool result = Spell.Cast(dpsSpell,Me.CurrentTarget.Location);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Pestilence
        public class NeedToPestilence : Decorator
        {
            public NeedToPestilence(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Pestilence";
                int countOfDiseases = 0;
                int countOfDiseaselessMobs = 0;

                if (Self.Immobilised) return false;
                if (!Utils.Adds) return false;
                if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (!Timers.Expired("Pestilence",4000)) return false;
                if (!Settings.Pestilence.Contains("disease")) if (!CLC.ResultOK(Settings.Pestilence)) return false;
                if (Settings.Pestilence.Contains("disease"))
                {
                    if (Target.IsDebuffOnTarget("Frost Fever")) countOfDiseases = countOfDiseases + 1;
                    if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;

                    if (Settings.Pestilence.Contains("1+ disease") && countOfDiseases < 1) return false;
                    if (Settings.Pestilence.Contains("2+ disease") && countOfDiseases < 2) return false;
                }
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                

                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (unit.Distance > 10) continue;
                    if (Me.CurrentTargetGuid == unit.Guid) continue;
                    if (unit.Auras.ContainsKey("Blood Plague")) continue;
                    if (unit.Auras.ContainsKey("Frost Fever")) continue;
                    countOfDiseaselessMobs = +1;
                }

                if (countOfDiseaselessMobs ==0) return false;
                
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Pestilence : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Pestilence";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Pestilence");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Blood Boil
        public class NeedToBloodBoil: Decorator
        {
            public NeedToBloodBoil(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Blood Boil";
                int countOfDiseases = 0;

                if (Self.Immobilised) return false;
                if (!Timers.Expired("BloodBoil", Settings.BloodBoilCooldown)) return false;
                if (Target.IsDistanceMoreThan(10)) return false;
                if (!Settings.BloodBoil.Contains("disease")) if (!CLC.ResultOK(Settings.BloodBoil)) return false;
                if (Settings.BloodBoil.Contains("disease"))
                {
                    if (Target.IsDebuffOnTarget("Frost Fever")) countOfDiseases = countOfDiseases + 1;
                    if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;
                    //if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;

                    if (Settings.BloodBoil.Contains("1+ disease") && countOfDiseases < 1) return false;
                    if (Settings.BloodBoil.Contains("2+ disease") && countOfDiseases < 2) return false;
                    //if (Settings.Pestilence.Contains("3+ disease") && countOfDiseases < 3) return false;
                }
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BloodBoil : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Blood Boil";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("BloodBoil");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Chains of Ice
        public class NeedToChainsOfIce : Decorator
        {
            public NeedToChainsOfIce(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Chains of Ice";

                if (Self.Immobilised) return false;
                if (Settings.ChainsOfIce.Contains("never")) return false;
                if (!Target.IsPlayer && !Target.IsFleeing) return false;
                if (Target.IsPlayer && !CT.IsMoving) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ChainsOfIce : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Chains of Ice";

                if (!Me.IsSafelyFacing(Me.CurrentTarget.Location)) { Target.Face(); Utils.LagSleep(); }
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Obliterate
        public class NeedToObliterate : Decorator
        {
            public NeedToObliterate(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Obliterate";
                int countOfDiseases = 0;

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if (!Settings.Obliterate.Contains("disease") && !Settings.Obliterate.Contains("2x Frost & Unholy")) if (!CLC.ResultOK(Settings.Obliterate)) return false;
                if (Settings.Obliterate.Contains("disease"))
                {
                    if (Target.IsDebuffOnTarget("Frost Fever")) countOfDiseases = countOfDiseases + 1;
                    if (Target.IsDebuffOnTarget("Blood Plague")) countOfDiseases = countOfDiseases + 1;

                    if (Settings.Obliterate.Contains("1+ disease") && countOfDiseases < 1) return false;
                    if (Settings.Obliterate.Contains("2+ disease") && countOfDiseases < 2) return false;
                }
                if (Settings.Obliterate.Contains("2x frost & unholy"))
                {
                    if (Me.FrostRuneCount < 2 && Me.UnholyRuneCount < 2) return false;
                }
                else
                {
                    if (Me.UnholyRuneCount < 1 && Me.FrostRuneCount < 1 && Me.DeathRuneCount < 2) return false;
                    if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                    if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                }

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Obliterate : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Obliterate";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Anti-Magic Shell
        public class NeedToAntiMagicShell : Decorator
        {
            public NeedToAntiMagicShell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Anti-Magic Shell";
                int countOfCastingsAttackingMe = 0;

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.AntiMagicShell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if (Utils.Adds)
                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (unit.Distance > 40) continue;
                    if (!unit.Combat) continue;
                    if (!unit.IsCasting) continue;
                    if (!unit.GotTarget) continue;
                    if (unit.CurrentTargetGuid != Me.Guid) continue;
                    
                    countOfCastingsAttackingMe += 1;
                }

                if (countOfCastingsAttackingMe == 0)
                {
                    if (!Target.IsCasting) return false;
                    if (Spell.CanCast("Mind Freeze") && CLC.ResultOK(Settings.MindFreeze)) return false;
                    if (Spell.CanCast("Strangulate") && CLC.ResultOK(Settings.Strangulate)) return false;
                }

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class AntiMagicShell : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Anti-Magic Shell";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Presence
        public class NeedToPresence : Decorator
        {
            public NeedToPresence(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.Presence;

                if (Self.Immobilised) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Presence : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.Presence;
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Dancing Rune Weapon
        public class NeedToRuneWeapon : Decorator
        {
            public NeedToRuneWeapon(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Dancing Rune Weapon";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.RuneWeapon)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RuneWeapon : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Dancing Rune Weapon";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Summon Gargoyle
        public class NeedToGargoyle : Decorator
        {
            public NeedToGargoyle(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Summon Gargoyle";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.Gargoyle)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Gargoyle : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Summon Gargoyle";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Howling Blast
        public class NeedToHowlingBlast : Decorator
        {
            public NeedToHowlingBlast(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Howling Blast";

                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Settings.HowlingBlast.Contains("Frost Fever") && !CLC.ResultOK(Settings.HowlingBlast)) return false;
                if (Settings.HowlingBlast.Contains("Frost Fever") && !Target.IsDebuffOnTarget("Frost Fever")) return false;
                if (!Timers.Expired("Ice", Settings.AttackCooldown)) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HowlingBlast : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Howling Blast";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Ice");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Pillar of Frost
        public class NeedToPillarOfFrost : Decorator
        {
            public NeedToPillarOfFrost(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Pillar of Frost";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.PillarOfFrost)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Timers.Expired("Ice", Settings.AttackCooldown)) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                //if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PillarOfFrost : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Pillar of Frost";
                bool result = Spell.Cast(dpsSpell);
                Timers.Reset("Ice");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Raise Dead
        public class NeedToRaiseDead : Decorator
        {
            public NeedToRaiseDead(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Raise Dead";

                if (Self.Immobilised) return false;
                if (Me.GotAlivePet) return false;
                if (!CLC.ResultOK(Settings.RaiseDead)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RaiseDead : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Raise Dead";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Dark Command - Fleeing targets only
        public class NeedToDarkCommandFleeing : Decorator
        {
            public NeedToDarkCommandFleeing(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Dark Command";

                if (!Target.IsFleeing) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DarkCommandFleeing : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Dark Command";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Outbreak
        public class NeedToOutbreak : Decorator
        {
            public NeedToOutbreak(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Outbreak";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.Outbreak)) return false;
                if (Target.IsDebuffOnTarget("Blood Plague")) return false;
                if (Target.IsDebuffOnTarget("Frost Fever")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
                //return (Spell.CanCast(dpsSpell));
            }
        }

        public class Outbreak : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Outbreak";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Empower Rune Weapon
        public class NeedToEmpowerRuneWeapon : Decorator
        {
            public NeedToEmpowerRuneWeapon(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Empower Rune Weapon";

                if (Self.Immobilised) return false;
                if (Me.BloodRuneCount >0) return false;
                if (Me.DeathRuneCount > 0) return false;
                if (Me.FrostRuneCount > 0) return false;
                if (Me.UnholyRuneCount > 0) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class EmpowerRuneWeapon : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Empower Rune Weapon";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Dark Transformation
        public class NeedToDarkTransformation : Decorator
        {
            public NeedToDarkTransformation(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                // 93426 Dark Transformation
                const string dpsSpell = "Dark Transformation";

                if (Self.Immobilised) return false;
                if (!Me.GotAlivePet) return false;
                if (!Self.IsBuffOnMe(93426)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DarkTransformation : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Dark Transformation";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Raise Ally
        public class NeedToRaiseAlly : Decorator
        {
            public NeedToRaiseAlly(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Raise Ally";

                if (Self.Immobilised) return false;
                if (!Me.IsInParty) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;

                WoWUnit raiseAlly = Me.PartyMembers.Where(player => player.Dead).FirstOrDefault(player => player.Distance <= Spell.MaxDistance(dpsSpell));
                if (raiseAlly == null) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RaiseAlly : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Raise Ally";
                WoWUnit raiseAlly = Me.PartyMembers.Where(player => player.Dead).FirstOrDefault(player => player.Distance <= Spell.MaxDistance(dpsSpell));

                bool result = Spell.Cast(dpsSpell,raiseAlly);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Instance Care - Protect party members etc
        public class NeedToInstanceCare : Decorator
        {
            public NeedToInstanceCare(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                WoWUnit target = null;
                WoWUnit tank = RAF.PartyTankRole;
                WoWUnit healer = RAF.PartyHealerRole;

                if (Self.Immobilised) return false;
                if (!Spell.IsKnown("Death Grip") || Spell.IsOnCooldown("Death Grip")) return false;
                if (!Me.IsInInstance) return false;
                if (!Me.IsInParty) return false;
                if (Me.HealthPercent < 50) return false;
                if (tank == null) return false;

                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (!unit.Combat) continue;
                    if (unit.Distance > Spell.MaxDistance("Death Grip")) continue;
                    if (unit.CurrentTargetGuid == Me.Guid) continue;
                    if (unit.CurrentTargetGuid == tank.Guid) continue;
                    //if (unit.CurrentTarget.IsPet) continue;
                    if (unit.CurrentTarget.Class == WoWClass.Warrior) continue;
                    if (unit.CurrentTarget.Class == WoWClass.DeathKnight) continue;
                    if (unit.CurrentTarget.Class == WoWClass.Rogue && unit.CurrentTarget.HealthPercent > 80) continue;
                    if (unit.CurrentTarget.Class == WoWClass.Hunter && unit.CurrentTarget.HealthPercent > 80) continue;
                    if (unit.CurrentTarget.Class == WoWClass.Paladin && healer != null && unit.CurrentTargetGuid != healer.Guid) continue;

                    ClassHelpers.Common.TempTarget = unit;
                    return true;
                }

                return false;
            }
        }

        public class InstanceCare : Action
        {
            protected override RunStatus Run(object context)
            {
                ClassHelpers.Common.TempTarget.Target();
                System.Threading.Thread.Sleep(1000);
                
                Target.Face(); 
                System.Threading.Thread.Sleep(650);

                Spell.Cast("Death Grip");


                return RunStatus.Success;
            }
        }
        #endregion

        #region Blood Tap
        public class NeedToBloodTap : Decorator
        {
            public NeedToBloodTap(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Blood Tap";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.BloodTap)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;
                if (Me.BloodRuneCount > 0) return false;
                if (Me.DeathRuneCount > 0) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BloodTap : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Blood Tap";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Festering Strike
        public class NeedToFesteringStrike : Decorator
        {
            public NeedToFesteringStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Festering Strike";

                if (Self.Immobilised) return false;
                if (!CLC.ResultOK(Settings.FesteringStrike)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.BloodRuneCount < 1 && Me.FrostRuneCount < 1 && Me.DeathRuneCount < 2) return false;
                if (Me.BloodRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (!Target.IsDebuffOnTarget("Frost Fever")) return false;
                if (!Target.IsDebuffOnTarget("Blood Plague")) return false;
                if (CT.Auras["Frost Fever"].TimeLeft.TotalSeconds > 5 && CT.Auras["Blood Plague"].TimeLeft.TotalSeconds > 5) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FesteringStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Festering Strike";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Diseases
        public class NeedToDiseases : Decorator
        {
            public NeedToDiseases(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Self.Immobilised) return false;
                if (!Me.GotTarget) return false;
                if (Spell.IsGCD) return false;
                if (Me.IsCasting) return false;

                if (Target.IsDebuffOnTarget("Frost Fever") && Target.IsDebuffOnTarget("Blood Plague")) return false;

                if (Settings.IcyTouch.Contains("Frost Fever"))
                {
                    if (!Target.IsDebuffOnTarget("Icy Touch") && (Me.FrostRuneCount > 0 || Me.DeathRuneCount > 0) && Spell.IsKnown("Icy Touch") && !Spell.IsOnCooldown("Icy Touch")) return true;
                }

                if (Settings.PlagueStrike.Contains("Blood Plague"))
                {
                    if (!Target.IsDebuffOnTarget("Blood Plague") && (Me.UnholyRuneCount > 0 || Me.DeathRuneCount > 0) && Spell.IsKnown("Plague Strike") && !Spell.IsOnCooldown("Plague Strike")) return true;
                }

                return false;
            }
        }

        public class Diseases : Action
        {
            protected override RunStatus Run(object context)
            {
                if (Settings.IcyTouch.Contains("Frost Fever") && !Target.IsDebuffOnTarget("Frost Fever"))
                {
                    Spell.Cast("Icy Touch");
                    return RunStatus.Success;
                }

                if (Settings.PlagueStrike.Contains("Blood Plague") && !Target.IsDebuffOnTarget("Blood Plague"))
                {
                    Spell.Cast("Plague Strike");
                    return RunStatus.Success;
                }

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Fleeing Attack
        public class NeedToFleeingAttack : Decorator
        {
            public NeedToFleeingAttack(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Utils.IsBattleground) return false;
                if (Target.IsPlayer) return false;
                if (!Target.IsFleeing) return false;
                if (!Spell.CanCast("Howling Blast") && !Spell.CanCast("Dark Command") && !Spell.CanCast("Icy Touch") && !Spell.CanCast("Death Coil")) return false;

                return true;
            }
        }

        public class FleeingAttack : Action
        {
            protected override RunStatus Run(object context)
            {
                if (!Me.IsMoving) Target.Face();

                if (Spell.CanCast("Howling Blast")) {Spell.Cast("Howling Blast"); return RunStatus.Success; }
                if (Spell.CanCast("Dark Command")) {Spell.Cast("Dark Command"); return RunStatus.Success; }
                if (Spell.CanCast("Icy Touch")) {Spell.Cast("Icy Touch"); return RunStatus.Success; }
                if (Spell.CanCast("Death Coil")) { Spell.Cast("Death Coil"); return RunStatus.Success; }
                
                return RunStatus.Failure;
            }
        }
        #endregion


        // Procs
        #region Sudden Doom PROC
        public class NeedToSuddenDoom : Decorator
        {
            public NeedToSuddenDoom(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Death Coil";

                if (Self.Immobilised) return false;
                if (!Self.IsBuffOnMeLUA("Sudden Doom")) return false;
                //if (!Self.IsBuffOnMe(81340)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SuddenDoom : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Death Coil";
                Utils.Log("** SUDDEN DOOM PROC **", Utils.Colour("Green"));
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Crimson Scourge PROC
        public class NeedToCrimsonScourge : Decorator
        {
            public NeedToCrimsonScourge(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Blood Boil";

                if (Self.Immobilised) return false;
                //if (!CLC.ResultOK(Settings.CrimsonScourge)) return false;
                if (!Self.IsBuffOnMe(81141)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class CrimsonScourge : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Blood Boil";
                Utils.Log("** CRIMSON SCOURGE PROC **", Utils.Colour("Green"));
                bool result = Spell.Cast(dpsSpell);
                

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Killing Machine PROC - Obliterate
        public class NeedToKillingMachine : Decorator
        {
            public NeedToKillingMachine(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Obliterate";

                if (Self.Immobilised) return false;
                //if (!Self.IsBuffOnMe(51124)) return false;
                if (!Self.IsBuffOnMeLUA("Killing Machine")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.UnholyRuneCount < 1 && Me.FrostRuneCount < 1 && Me.DeathRuneCount < 2) return false;
                if (Me.UnholyRuneCount < 1 && Me.DeathRuneCount < 1) return false;
                if (Me.FrostRuneCount < 1 && Me.DeathRuneCount < 1) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class KillingMachine : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Obliterate";
                Utils.Log("** KILLING MACHINE PROC **", Utils.Colour("Green"));
                bool result = Spell.Cast(dpsSpell);
                

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Killing Machine PROC - Frost Strike
        public class NeedToKillingMachineFrostStrike : Decorator
        {
            public NeedToKillingMachineFrostStrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Frost Strike";

                if (Self.Immobilised) return false;
                if (!Self.IsBuffOnMeLUA("Killing Machine")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Me.CurrentRunicPower < Spell.PowerCost(dpsSpell)) return false;

                return (Spell.IsKnown(dpsSpell) && !Spell.IsOnCooldown(dpsSpell));
		
            }
        }

        public class KillingMachineFrostStrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Frost Strike";
                Utils.Log("** KILLING MACHINE PROC **", Utils.Colour("Green"));
                bool result = Spell.Cast(dpsSpell);
                

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Freezing Fog PROC
        public class NeedToFreezingFog : Decorator
        {
            public NeedToFreezingFog(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Icy Touch";

                if (Self.Immobilised) return false;
                if (!Self.IsBuffOnMe(59052)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
		
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FreezingFog : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Spell.IsKnown("Howling Blast") ? "Howling Blast" : "Icy Touch";
                bool result = Spell.Cast(dpsSpell);
                Utils.Log("** FREEZING FOG PROC **", Utils.Colour("Green"));

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

      

        #endregion

    }
}