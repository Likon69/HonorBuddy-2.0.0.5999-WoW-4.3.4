using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Logic;
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


                 // Move To
                 new NeedToMoveTo(new MoveTo()),

                 // Make sure we always have a target. If we don't have a target, take the pet's target - if they have one.
                 new Decorator(ret => !Me.GotTarget && Me.GotAlivePet && Me.Pet.GotTarget, new Action(ret => Me.Pet.CurrentTarget.Target())),

                 // Check if we get aggro during the pull
                 // This is in here and not the pull because we are in combat at this point
                 new NeedToCheckAggroOnPull(new CheckAggroOnPull()),

                 // Retarget
                 new NeedToRetarget(new Retarget()),

                 // Abort combat is the target's health is 95% + after 30 seconds of combat
                 new NeedToCheckCombatTimer(new CheckCombatTimer()),

                 // LOS Check
                 new NeedToLOSCheck(new LOSCheck()),

                 // Auto Attack
                 //new NeedToAutoAttack(new AutoAttack()),

                 // Face Target
                 new NeedToFaceTarget(new FaceTarget()),


                 // ************************************************************************************
                 // Important/time sensative spells here
                 // These are spells that need to be case asap

                 // Ice Barrier
                 new NeedToIceBarrier(new IceBarrier()),

                 // Mana Shield
                 new NeedToManaShield(new ManaShield()),

                 // Mage Ward
                 new NeedToMageWard(new MageWard()),

                 // Evocation
                 new NeedToEvocation(new Evocation()),

                 // Pull Sheep
                 new NeedToPullSheep(new PullSheep()),

                 // Brain Freeze - PROC
                 new NeedToBrainFreeze(new BrainFreeze()),

                 // Hot Streak - PROC
                 new NeedToHotStreak(new HotStreak()),

                 // Combustion - Only with Pyroblast debuff
                 new NeedToCombustion(new Combustion()),

                 // Flame Orb
                 new NeedToFlameOrb(new FlameOrb()),

                 // Time Warp
                 new NeedToTimeWarp(new TimeWarp()),

                 // Arcane Power
                 new NeedToArcanePower(new ArcanePower()),

                 // Flamestrike
                 new NeedToFlamestrike(new Flamestrike()),

                 // Blast Wave
                 new NeedToBlastWave(new BlastWave()),

                 // Blizzard
                 new NeedToBlizzard(new Blizzard()),

                 // Arcane Explosion
                 new NeedToArcaneExplosion(new ArcaneExplosion()),

                 // Sheep
                 new NeedToSheep(new Sheep()),

                 // Pet Attack
                 new NeedToPetAttack(new PetAttack()),

                 // Ice Lance
                 new NeedToIceLance(new IceLance()),

                 // Frost Nova Spell
                 new NeedToFrostNovaSpell(new FrostNovaSpell()),

                 // Frost Nova TEST
                 new NeedToFrostNovaOther(new FrostNovaOther()),

                 // Frost Nova
                 new NeedToFrostNova(new FrostNova()),

                 // Dragon's Breath
                 new NeedToDragonsBreath(new DragonsBreath()),

                 // Counterspell
                 new NeedToCounterspell(new Counterspell()),

                 // Mirror Image
                 new NeedToMirrorImage(new MirrorImage()),

                 // Icy Veins
                 new NeedToIcyVeins(new IcyVeins()),

                 // Living Bomb
                 new NeedToLivingBomb(new LivingBomb()),

                 // Arcane Barrage
                 new NeedToArcaneBarrage(new ArcaneBarrage()),

                 // Arcane Missiles
                 new NeedToArcaneMissiles(new ArcaneMissiles()),

                 // Fire Blast
                 new NeedToFireBlast(new FireBlast()),

                 
                 // Cone Of Cold
                 new NeedToConeOfCold(new ConeOfCold()),


                 // ************************************************************************************
                 // Other spells here

                 // Presence of Mind
                 new NeedToPresenceOfMind(new PresenceOfMind()),

                 
                 // Pyroblast
                 new NeedToPyroblast(new Pyroblast()),

                 // Main DPS Spell
                 new NeedToMainDPSSpell(new MainDPSSpell()),

                 // Arcane Blast
                 //new NeedToArcaneBlast(new ArcaneBlast()),
                 
                 // Fireball
                 //new NeedToFireball(new Fireball()),

                 // Frostbolt
                 //new NeedToFrostbolt(new Frostbolt()),

                 // Wand
                 new NeedToWandParty(new WandParty()),

                 // Move To
                 new NeedToMoveTo(new MoveTo()),



                 // TimerTest2
                 //new NeedToTimerTest2(new TimerTest2()),


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
                Utils.Log("*********** 1:" + Timers.ElapsedMilliseconds("TimersTest"));
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
                Utils.Log("*********** 2: " + Timers.ElapsedMilliseconds("TimersTest"));
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
        
        #region Retarget
        public class NeedToRetarget : Decorator
        {
            public NeedToRetarget(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Me.GotTarget && CT.IsAlive) return false;
                //if (!Me.Combat) return false;

                return true;
            }
        }

        public class Retarget : Action
        {
            protected override RunStatus Run(object context)
            {
                List<WoWUnit> hlist = (from o in ObjectManager.ObjectList
                                       where o is WoWUnit
                                       let p = o.ToUnit()
                                       where
                                           p.Distance2D < 40 && !p.Auras.ContainsKey("Polymorph") && !p.Dead && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet || p.IsTargetingMyRaidMember) && p.Attackable
                                       orderby p.HealthPercent ascending
                                       orderby p.Distance2D ascending
                                       select p).ToList();

                if (hlist.Count > 0)
                {
                    hlist[0].Target();
                    return RunStatus.Success;
                }

                WoWUnit sheepedUnit = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < 50 && p.Auras.ContainsKey("Polymorph") select p).FirstOrDefault();

                if (sheepedUnit !=null)
                {
                    sheepedUnit.Target();

                    WoWPoint pointToGo = Movement.FindSafeLocation(20);
                    WoWMovement.ClickToMove(pointToGo);
                    System.Threading.Thread.Sleep(500);
                    while (Me.IsMoving) System.Threading.Thread.Sleep(500);

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
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!Me.GotTarget) return false;
                if (Me.IsCasting) return false;

                double distance = Settings.MaximumPullDistance;
                return Me.IsMoving || Target.IsDistanceMoreThan(distance);
            }
        }

        public class MoveTo : Action
        {
            protected override RunStatus Run(object context)
            {
                double distance = Settings.MaximumPullDistance;
                double minDistance = Settings.MinimumPullDistance;

               
                Movement.DistanceCheck(distance, minDistance);
                /*
                if (Self.IsPowerPercentAbove(Settings.ReserveMana))
                {
                    Movement.DistanceCheck(Settings.MaximumPullDistance, Settings.MinimumPullDistance);
                }
                else
                {
                    Movement.DistanceCheck(Target.InteractRange, Target.InteractRange *0.75);
                }
                 */

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

        #region Procs - Divine Purpose
        public class NeedToDivinePurpose: Decorator
        {
            public NeedToDivinePurpose(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                /*
                string dpsSpell = Settings.DivinePurpose;

                // 90174 = Divine Purpose
                if (!Self.IsBuffOnMe(90174,Self.AuraCheck.AllAuras)) return false;

                if (Settings.DivinePurpose.Contains("Inquisition >"))
                {
                    string[] spellList = Settings.DivinePurpose.Split('>');
                    dpsSpell = Me.ActiveAuras.ContainsKey("Inquisition") ? spellList[1].Trim() : spellList[0].Trim();
                }

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                return (Spell.CanCast(dpsSpell));
                 */
                return false;

            }
        }

        public class DivinePurpose : Action
        {
            protected override RunStatus Run(object context)
            {
                /*
                string dpsSpell = Settings.DivinePurpose;
                
                if (Settings.DivinePurpose.Contains("Inquisition >"))
                {
                    string[] spellList = Settings.DivinePurpose.Split('>');
                    dpsSpell = Me.ActiveAuras.ContainsKey("Inquisition") ? spellList[1].Trim() : spellList[0].Trim();
                }
                
                Utils.Log("-Divine Purpose Proc");
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
                 */
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Wand Party
        public class NeedToWandParty : Decorator
        {
            public NeedToWandParty(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shoot";

                if ((Me.IsInInstance || Utils.IsBattleground))// && Settings.PartyHealWhen.Contains("dedicated"))
                {
                    if (!Me.IsInParty) return false;
                    //if (!CLC.ResultOK(Settings.WandParty)) return false;
                    if (!Me.GotTarget) return false;
                    if (!Utils.IsInLineOfSight(Me.CurrentTarget.Location)) return false;
                    if (Target.IsDistanceMoreThan(30)) return false;
                    if (Spell.IsGCD || Me.IsCasting) return false;
                    if (!Utils.IsNotWanding) return false;
                }
                else
                {
                    if (!Me.GotTarget) return false;
                    if (Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                    if (!Utils.IsInLineOfSight(Me.CurrentTarget.Location)) return false;
                    if (Target.IsDistanceMoreThan(30)) return false;
                    if (Spell.IsGCD || Me.IsCasting) return false;
                    if (!Utils.IsNotWanding) return false;
                }
                

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class WandParty : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shoot";
                Spell.Cast(dpsSpell);

                return RunStatus.Failure;
            }
        }
        #endregion

        #region Run Away
        public class NeedToRunAway : Decorator
        {
            public NeedToRunAway(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.GotTarget) return false;
                if (Target.IsDebuffOnTarget("Frost Nova") && CT.Auras["Frost Nova"].TimeLeft.Seconds >= 2) return true;
                if (Target.IsDebuffOnTarget("Dragon's Breath") && CT.Auras["Dragon's Breath"].TimeLeft.Seconds >= 2) return true;

                return false;
            }
        }

        public class RunAway : Action
        {
            protected override RunStatus Run(object context)
            {
                const double distanceFrom = 15;
                bool result = false;
                WoWPoint pointToGo = Movement.FindSafeLocation(distanceFrom);
                WoWMovement.ClickToMove(pointToGo);

                System.Threading.Thread.Sleep(1500);
                if (Spell.CanCast("Blink"))
                {
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                    Spell.Cast("Blink");
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    while (Me.IsMoving)
                    {
                        WoWMovement.ClickToMove(pointToGo);
                        pointToGo = Movement.FindSafeLocation(distanceFrom);
                        System.Threading.Thread.Sleep(250);
                    }
                }

                //while (Me.IsMoving)
                {
                    System.Threading.Thread.Sleep(500);
                    ObjectManager.Update();
                }

                WoWMovement.MoveStop();
                WoWMovement.Face(CT.Guid);
                System.Threading.Thread.Sleep(500);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mana Gem
        public class NeedToManaGem : Decorator
        {
            public NeedToManaGem(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Self.IsPowerPercentAbove(Settings.ManaGem)) return false;
                if (!Utils.CombatCheckOk("", true)) return false;
                if (!ClassHelpers.Mage.ConjuredItems.HaveManaGem()) return false;
                //if (ClassHelpers.Mage.ConjuredItems.ManaGem == null) return false;
                string luacode = String.Format("return GetItemCooldown(\"{0}\")", ClassHelpers.Mage.ConjuredItems.ManaGem.Entry);
                
                return Utils.LuaGetReturnValueString(luacode) == "0";
            }
        }

        public class ManaGem : Action
        {
            protected override RunStatus Run(object context)
            {
                ClassHelpers.Mage.ConjuredItems.UseManaGem();

                return RunStatus.Failure;
            }
        }
        #endregion


        #region Fireball
        public class NeedToFireball : Decorator
        {
            public NeedToFireball(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Fireball";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.Fireball)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Fireball : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Fireball";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Arcane Blast

        public class NeedToArcaneBlast : Decorator
        {
            public NeedToArcaneBlast(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Arcane Blast";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ArcaneBlast)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ArcaneBlast : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Arcane Blast";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Arcane Missiles
        public class NeedToArcaneMissiles : Decorator
        {
            public NeedToArcaneMissiles(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Arcane Missiles";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ArcaneMissiles)) return false;
                if (!Self.IsBuffOnMe(79683,Self.AuraCheck.AllAuras)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ArcaneMissiles : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Arcane Missiles";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Fire Blast
        public class NeedToFireBlast : Decorator
        {
            public NeedToFireBlast(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Fire Blast";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.FireBlast)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FireBlast : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Fire Blast";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Frostbolt
        public class NeedToFrostbolt : Decorator
        {
            public NeedToFrostbolt(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Frostbolt";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.Frostbolt)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Frostbolt : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Frostbolt";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Cone Of Cold
        public class NeedToConeOfCold : Decorator
        {
            public NeedToConeOfCold(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Cone of Cold";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ConeOfCold)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(12)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ConeOfCold : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Cone of Cold";
                bool result = Spell.Cast(dpsSpell);
                
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Frost Nova
        public class NeedToFrostNova : Decorator
        {
            public NeedToFrostNova(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Frost Nova";

                if (Settings.LazyRaider.Contains("always")) return false;
                if (Settings.AlternateFrostNova.Contains("always")) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.FrostNova)) return false;
                if (Me.IsInInstance) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(12)) return false;
                if (!Utils.Adds && !Target.IsHealthPercentAbove(50) && Self.IsHealthPercentAbove(65)) return false;
                if (Target.IsLowLevel) return false;
                if (!Timers.Expired("FrostNova",3000)) return false;

                if (ClassHelpers.Mage.GotSheep()) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FrostNova : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Frost Nova";
                double distanceFrom = Settings.FrostNovaDistance;
                WoWPoint pointToGo = Movement.FindSafeLocation(distanceFrom);
                Timers.Reset("FrostNova");

                WoWMovement.ClickToMove(pointToGo);
                if (Spell.CanCast(dpsSpell)) Spell.Cast(dpsSpell);

                System.Threading.Thread.Sleep(500);
                while (Spell.IsGCD) System.Threading.Thread.Sleep(500);
                ObjectManager.Update();

                while (Me.IsMoving) { WoWMovement.ClickToMove(pointToGo); System.Threading.Thread.Sleep(250); if (!Target.IsDebuffOnTarget(dpsSpell)) break; }
                /*
                if (!Spell.CanCast("Blink"))
                {
                    while (Me.IsMoving) { WoWMovement.ClickToMove(pointToGo); System.Threading.Thread.Sleep(250); if (!Target.IsDebuffOnTarget(dpsSpell)) break; }
                }
                else
                {
                    System.Threading.Thread.Sleep(250);
                    WoWMovement.ClickToMove(pointToGo); System.Threading.Thread.Sleep(250);
                    if (Target.IsDebuffOnTarget(dpsSpell))
                    {
                        Spell.Cast("Blink");
                        System.Threading.Thread.Sleep(300);
                    }
                }
                 */

                WoWMovement.MoveStop();
                WoWMovement.Face(CT.Guid);
                System.Threading.Thread.Sleep(350);

                if (CLC.ResultOK(Settings.Pyroblast) && Spell.CanCast("Pyroblast")) Spell.Cast("Pyroblast");
                if (Spell.CanCast("Ice Lance")) Spell.Cast("Ice Lance");

                //ObjectManager.Update();
                bool result = Target.IsDebuffOnTarget(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Frost Nova Other
        public class NeedToFrostNovaOther : Decorator
        {
            public NeedToFrostNovaOther(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Frost Nova";

                if (Settings.LazyRaider.Contains("always")) return false;
                if (Settings.AlternateFrostNova.Contains("never")) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.FrostNova)) return false;
                if (Me.IsInInstance) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(13)) return false;
                if (!Utils.Adds && !Target.IsHealthPercentAbove(50) && Self.IsHealthPercentAbove(65)) return false;
                if (Target.IsLowLevel) return false;
                if (!Timers.Expired("FrostNova", 3000)) return false;

                if (ClassHelpers.Mage.GotSheep()) return false;


                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FrostNovaOther : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Frost Nova";
                double distanceFrom = Settings.FrostNovaDistance;
                //WoWPoint pointToGo = Movement.FindSafeLocation(distanceFrom);
                Timers.Reset("FrostNova");

                //WoWMovement.ClickToMove(pointToGo);
                Target.Face();
                System.Threading.Thread.Sleep(250);

                //WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                WoWMovement.Move(WoWMovement.MovementDirection.Backwards);
                
                if (Spell.CanCast(dpsSpell)) Spell.Cast(dpsSpell);
                //WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                
                

                System.Threading.Thread.Sleep(500);
                //WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                while (Spell.IsGCD) System.Threading.Thread.Sleep(500);
                
                ObjectManager.Update();

                WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                if (Spell.CanCast("Ice Lance")) Spell.Cast("Ice Lance");

                WoWMovement.MoveStop(WoWMovement.MovementDirection.All);

                //ObjectManager.Update();
                bool result = Target.IsDebuffOnTarget(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion


      
        #region Ice Lance
        public class NeedToIceLance : Decorator
        {
            public NeedToIceLance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                // 44544 Fingers of Frost proc
                const string spellName = "Ice Lance";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(spellName))) return false;
                if (Target.IsDebuffOnTarget("Frostbite")) return true;
                //if (Target.IsDebuffOnTarget("Frost Nova")) return true;
                if (Self.IsBuffOnMe(44544) && Self.Buff(44544).TimeLeft.TotalSeconds > 0) return true;
                //if (Self.IsBuffOnMe(44544,Self.AuraCheck.AllAuras)) Utils.Log("*************** FINGERS OF FROST BOOOYA! OTHER");
                //if (Me.Auras.ContainsKey("Fingers of Frost") && Me.Auras["Fingers of Frost"].TimeLeft.TotalSeconds > 0) return true;

                return false;
            }
        }

        public class IceLance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Ice Lance";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
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
                bool result = false;
                
                Lua.DoString("PetAttack()");
                System.Threading.Thread.Sleep(250);

                //result = Me.Pet.CurrentTargetGuid == Me.CurrentTargetGuid;

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Counterspell
        public class NeedToCounterspell : Decorator
        {
            public NeedToCounterspell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Counterspell";

                if (!CLC.ResultOK(Settings.Counterspell)) return false;
                if (!Target.IsCasting) return false;
                if (!Target.IsCaster) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                
                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Counterspell : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Counterspell";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Arcane Barrage

        public class NeedToArcaneBarrage : Decorator
        {
            public NeedToArcaneBarrage(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Arcane Barrage";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ArcaneBarrage)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (Spell.IsOnCooldown(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ArcaneBarrage : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Arcane Barrage";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Mage Ward
        public class NeedToMageWard : Decorator
        {
            public NeedToMageWard(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Mage Ward";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.MageWard)) return false;
                if (!Utils.CombatCheckOk(spellName, true)) return false;
                if (!Target.IsCasting) return false;
                if (!Target.IsTargetingMe) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class MageWard : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Mage Ward";
                bool result = Spell.Cast(spellName);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mirror Image

        public class NeedToMirrorImage : Decorator
        {
            public NeedToMirrorImage(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mirror Image";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.MirrorImage)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;
                //if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MirrorImage : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mirror Image";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Dragon's Breath
        public class NeedToDragonsBreath : Decorator
        {
            public NeedToDragonsBreath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Dragon's Breath";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.DragonsBreath)) return false;
                if (!Utils.CombatCheckOk(spellName, true)) return false;
                if (Target.IsDistanceMoreThan(12)) return false;
                if (ClassHelpers.Mage.GotSheep(40) &&  ClassHelpers.Mage.SheepUnit.Behind(Me))
                {}
                else
                {
                    if (ClassHelpers.Mage.GotSheep(Me.Location, 12)) return false;
                }
                

                return (Spell.CanCast(spellName));
            }
        }

        public class DragonsBreath : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Dragon's Breath";
                Target.Face(); System.Threading.Thread.Sleep(400);

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Sheep
        public class NeedToSheep: Decorator
        {
            public NeedToSheep(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Polymorph";

                //if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (Settings.LazyRaider.Contains("always")) return false;
                if (!CLC.ResultOK(Settings.Sheep)) return false;
                if (Me.IsInInstance) return false;
                if (!Utils.Adds) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Timers.Expired("Polymorph",2500)) return false;

                // Can we SHEEP a target?
                WoWUnit wUnit = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < 40 && p.IsHostile && !p.Dead && p.IsTargetingMeOrPet && (p.CreatureType == WoWCreatureType.Beast || p.CreatureType == WoWCreatureType.Humanoid) && !p.Auras.ContainsKey(dpsSpell) && p.HealthPercent > 96 && p.Attackable select p).FirstOrDefault();
                if (wUnit == null) return false;

                // Do we already have a SHEEP?
                WoWUnit sheepedUnit = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < 40 && p.Auras.ContainsKey(dpsSpell) select p).FirstOrDefault();
                if (sheepedUnit != null) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Sheep : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Polymorph";

                WoWUnit ccUnit = (from o in ObjectManager.ObjectList
                                  where o is WoWUnit
                                  let p = o.ToUnit()
                                  where p.Distance2D < 40 && p.IsHostile && !p.Dead && p.IsTargetingMeOrPet && (p.CreatureType == WoWCreatureType.Beast || p.CreatureType == WoWCreatureType.Humanoid) && !p.Auras.ContainsKey(dpsSpell) && p.HealthPercent > 96 && p.Attackable
                                  select p).FirstOrDefault();

                if (ccUnit != null)
                {
                    // Cast PoM if we can
                    if (CLC.ResultOK(Settings.PresenceOfMind))
                    {
                        Spell.Cast("Presence of Mind");
                        System.Threading.Thread.Sleep(500);
                    }

                    // Cast the CC spell on the appropriate target
                    Spell.Cast(dpsSpell, ccUnit);
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                    System.Threading.Thread.Sleep(500);
                    ObjectManager.Update();
                    Timers.Reset("Polymorph");

                    // Now retarget (if we need to) a mob without our CC spell
                    if (Me.CurrentTarget.Auras.ContainsKey(dpsSpell))
                    {
                        ObjectManager.Update();
                        WoWUnit attackUnit = (from o in ObjectManager.ObjectList
                                              where o is WoWUnit
                                              let p = o.ToUnit()
                                              where p.Distance2D < 40 && !p.Dead && p.IsTargetingMeOrPet && !p.Auras.ContainsKey(dpsSpell) && p.Attackable
                                              orderby p.HealthPercent ascending
                                              select p).FirstOrDefault();
                        attackUnit.Target();
                        System.Threading.Thread.Sleep(1000);
                    }
                }

                

                return RunStatus.Success; //: RunStatus.Failure;
            }
        }
        #endregion

        #region Ice Barrier

        public class NeedToIceBarrier : Decorator
        {
            public NeedToIceBarrier(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Ice Barrier";


                if (!CLC.ResultOK(Settings.IceBarrier)) return false;
                if (Self.IsHealthPercentAbove(Settings.IceBarrierHealth)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class IceBarrier : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Ice Barrier";
                
                Spell.Cast(dpsSpell);
                Utils.LagSleep();
                bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Icy Veins
        public class NeedToIcyVeins : Decorator
        {
            public NeedToIcyVeins(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Icy Veins";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.IcyVeins)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class IcyVeins : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Icy Veins";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Living Bomb

        public class NeedToLivingBomb : Decorator
        {
            public NeedToLivingBomb(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Living Bomb";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.LivingBomb)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class LivingBomb : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Living Bomb";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();
                bool result = Target.IsDebuffOnTarget(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Pyroblast

        public class NeedToPyroblast : Decorator
        {
            public NeedToPyroblast(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Pyroblast";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.Pyroblast)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Pyroblast : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Pyroblast";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Slow

        public class NeedToSlow : Decorator
        {
            public NeedToSlow(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Slow";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.Slow)) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Slow : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Slow";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = Target.IsDebuffOnTarget(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Arcane Explosion
        public class NeedToArcaneExplosion: Decorator
        {
            public NeedToArcaneExplosion(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Arcane Explosion";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (Settings.ArcaneExplosion.Contains("never")) return false;
                if (!Utils.Adds) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;

                int count = Utils.CountOfAddsInRange(10, Me.Location);

                if (Settings.ArcaneExplosion.Contains("3+ adds") && count < 3) return false;
                if (Settings.ArcaneExplosion.Contains("4+ adds") && count < 4) return false;
                if (Settings.ArcaneExplosion.Contains("5+ adds") && count < 5) return false;
                if (Settings.ArcaneExplosion.Contains("6+ adds") && count < 6) return false;
                if (Settings.ArcaneExplosion.Contains("7+ adds") && count < 7) return false;
                if (Settings.ArcaneExplosion.Contains("8+ adds") && count < 8) return false;
                if (Settings.ArcaneExplosion.Contains("9+ adds") && count < 9) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ArcaneExplosion : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Arcane Explosion";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Time Warp
        public class NeedToTimeWarp : Decorator
        {
            public NeedToTimeWarp(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Time Warp";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.TimeWarp)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class TimeWarp : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Time Warp";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Flame Orb

        public class NeedToFlameOrb : Decorator
        {
            public NeedToFlameOrb(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Flame Orb";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.FlameOrb)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FlameOrb : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Flame Orb";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Mana Shield
        public class NeedToManaShield : Decorator
        {
            public NeedToManaShield(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mana Shield";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ManaShield)) return false;
                if (Self.IsHealthPercentAbove(Settings.ManaShieldHealth)) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ManaShield : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mana Shield";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();
                bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Arcane Power
        public class NeedToArcanePower : Decorator
        {
            public NeedToArcanePower(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Arcane Power";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ArcanePower)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ArcanePower : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Arcane Power";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Presence of Mind
        public class NeedToPresenceOfMind : Decorator
        {
            public NeedToPresenceOfMind(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Presence of Mind";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.PresenceOfMind)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PresenceOfMind : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Presence of Mind";
                Spell.Cast(dpsSpell);
                System.Threading.Thread.Sleep(500);
                while (Spell.IsGCD) System.Threading.Thread.Sleep(500);
                
                return RunStatus.Failure;
            }
        }
        #endregion

        #region Scorch

        public class NeedToScorch : Decorator
        {
            public NeedToScorch(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Scorch";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.Scorch)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Scorch : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Scorch";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Evocation
        public class NeedToEvocation : Decorator
        {
            public NeedToEvocation(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Evocation";

                if (Self.IsPowerPercentAbove(Settings.EvocationMana) && Self.IsHealthPercentAbove(Settings.EvocationHealth)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Evocation : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Evocation";

                if (Me.IsMoving) WoWMovement.MoveStop(); 
                while (Me.IsMoving) { WoWMovement.MoveStop();  System.Threading.Thread.Sleep(500); }

                bool result = Spell.Cast(dpsSpell);
                System.Threading.Thread.Sleep(1000);
                Utils.WaitWhileCasting();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Pull Sheep

        public class NeedToPullSheep : Decorator
        {
            public NeedToPullSheep(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.PullSpell;

                if (!Timers.Expired("PullSpellCast", 4000)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.IsDebuffOnTarget("Polymorph")) return false;
                if (!CLC.ResultOK(Settings.PullSheep)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PullSheep : Action
        {
            protected override RunStatus Run(object context)
            {
                Utils.Log("-Using 'pull spell' to break sheep",Utils.Colour("Red"));
                Timers.Reset("PullSpellCast");

                WoWPoint pointToGo = Movement.FindSafeLocation(20);
                WoWMovement.ClickToMove(pointToGo);
                System.Threading.Thread.Sleep(500);
                while (Me.IsMoving) System.Threading.Thread.Sleep(250);

                Target.Face();
                System.Threading.Thread.Sleep(400);

                string dpsSpell = Settings.PullSpell;
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Main DPS Spell

        public class NeedToMainDPSSpell : Decorator
        {
            public NeedToMainDPSSpell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                //if (Spell.IsGCD) return false;
                //if (Me.IsCasting) return false;

                string dpsSpell = Settings.MainDPSSpell;
                
                if (!Spell.IsKnown(Settings.MainDPSSpell)) dpsSpell = "Fireball";
                if (Spell.IsOnCooldown(Settings.MainDPSSpell))
                {
                    switch (Settings.MainDPSSpell)
                    {
                        case "Arcane Blast":
                            dpsSpell = "Fireball";
                            break;

                        case "Fireball":
                            dpsSpell = "Frostbolt";
                            break;

                        case "Frostbolt":
                            dpsSpell = "Fireball";
                            break;

                        case "Scorch":
                            dpsSpell = "Frostbolt";
                            break;

                        case "Ice Lance":
                            dpsSpell = "Fireball";
                            break;
                    }
                }

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MainDPSSpell : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.MainDPSSpell;

                if (!Spell.IsKnown(Settings.MainDPSSpell)) dpsSpell = "Fireball";
                if (Spell.IsOnCooldown(Settings.MainDPSSpell))
                {
                    switch (Settings.MainDPSSpell)
                    {
                        case "Arcane Blast":
                            dpsSpell = "Fireball";
                            break;

                        case "Fireball":
                            dpsSpell = "Frostbolt";
                            break;

                        case "Frostbolt":
                            dpsSpell = "Fireball";
                            break;

                        case "Scorch":
                            dpsSpell = "Frostbolt";
                            break;

                        case "Ice Lance":
                            dpsSpell = "Fireball";
                            break;
                    }
                }

                bool result = Spell.Cast(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Blizzard
        public class NeedToBlizzard : Decorator
        {
            public NeedToBlizzard(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Blizzard";

                if (Settings.Blizzard.Contains("never")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                //if (!Utils.Adds) return false;
                if (Me.IsMoving) return false;

                WoWUnit tank = Me.IsInInstance ? RaFHelper.Leader : Me;
                int count = Utils.CountOfAddsInRange(8, tank.Location);
                if (ClassHelpers.Mage.GotSheep(tank.Location, 10)) return false;

                if (Settings.Blizzard.Contains("only on adds") && !Utils.Adds) return false;
                if (Settings.Blizzard.Contains("3+ adds") && count < 3) return false;
                if (Settings.Blizzard.Contains("4+ adds") && count < 4) return false;
                if (Settings.Blizzard.Contains("5+ adds") && count < 5) return false;
                if (Settings.Blizzard.Contains("6+ adds") && count < 6) return false;
                if (Settings.Blizzard.Contains("7+ adds") && count < 7) return false;
                if (Settings.Blizzard.Contains("8+ adds") && count < 8) return false;
                if (Settings.Blizzard.Contains("9+ adds") && count < 9) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Blizzard : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Blizzard";
                double maxDistance = Spell.MaxDistance(dpsSpell) - 1;
                WoWUnit tank = Me.IsInInstance? RaFHelper.Leader : Me;

                if (tank != Me)
                    while (tank.Distance2D > maxDistance)
                    {
                        // if we're out of Blizzard range then move closer
                        Movement.MoveTo(tank.Location);
                        Utils.Log("Out of range for Blizzard, moving closer");
                    }

                if (!Utils.IsInLineOfSight(tank.Location)) Utils.MoveToLineOfSight(tank.Location);
                if (Me.IsMoving) Movement.StopMoving();

                // Once we finally get here, if the tank is moving then just bail out
                if (tank.IsMoving) return RunStatus.Failure;

                // Cast AoE at this location
                WoWPoint AoELocation = tank.Location;
                if (!Me.IsInParty) AoELocation = CT.Location;

                bool result = Spell.Cast(dpsSpell, AoELocation);
                System.Threading.Thread.Sleep(500);
                while (Me.IsCasting)
                {
                    // If the tank is not within 6 yards of our original casting location then bail out (assuming mobs won't be there either)
                    // If we're out of combat bail out (assuming mobs are all dead)
                    if (tank.Location.Distance(AoELocation) > 6 || !tank.Combat)
                    {
                        Utils.Log("** Stop casting Blizzard **");
                        WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                        WoWMovement.MoveStop(WoWMovement.MovementDirection.JumpAscend);
                        break;
                    }

                    System.Threading.Thread.Sleep(250);

                }
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Flamestrike
        public class NeedToFlamestrike : Decorator
        {
            public NeedToFlamestrike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Flamestrike";

                if (!Timers.Expired("Flamestrike", 10 * 1000)) return false;
                if (Settings.Flamestrike.Contains("never")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, true)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                
                //if (!Utils.Adds) return false;
                if (Me.IsMoving) return false;

                WoWUnit tank = Me.IsInInstance ? RaFHelper.Leader : Me;
                if (ClassHelpers.Mage.GotSheep(tank.Location, 9)) return false;
                int count = Utils.CountOfAddsInRange(10, tank.Location);

                if (Settings.Flamestrike.Contains("only on adds") && !Utils.Adds) return false;
                if (Settings.Flamestrike.Contains("3+ adds") && count < 3) return false;
                if (Settings.Flamestrike.Contains("4+ adds") && count < 4) return false;
                if (Settings.Flamestrike.Contains("5+ adds") && count < 5) return false;
                if (Settings.Flamestrike.Contains("6+ adds") && count < 6) return false;
                if (Settings.Flamestrike.Contains("7+ adds") && count < 7) return false;
                if (Settings.Flamestrike.Contains("8+ adds") && count < 8) return false;
                if (Settings.Flamestrike.Contains("9+ adds") && count < 9) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Flamestrike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Flamestrike";
                double maxDistance = Spell.MaxDistance(dpsSpell) - 1;
                WoWUnit tank = Me.IsInInstance ? RaFHelper.Leader : Me;

                Timers.Reset("Flamestrike");

                if (tank != Me)
                    while (tank.Distance2D > maxDistance)
                    {
                        // if we're out of Blizzard range then move closer
                        Movement.MoveTo(tank.Location);
                        Utils.Log("Out of range for Blizzard, moving closer");
                    }

                if (!Utils.IsInLineOfSight(tank.Location)) Utils.MoveToLineOfSight(tank.Location);
                if (Me.IsMoving) Movement.StopMoving();

                // Once we finally get here, if the tank is moving then just bail out
                if (tank.IsMoving) return RunStatus.Failure;

                // Cast AoE at this location
                WoWPoint AoELocation = tank.Location;
                if (!Me.IsInParty) AoELocation = CT.Location;

                bool result = Spell.Cast(dpsSpell, AoELocation);
                System.Threading.Thread.Sleep(500);
                while (Me.IsCasting)
                {
                    // If the tank is not within 6 yards of our original casting location then bail out (assuming mobs won't be there either)
                    // If we're out of combat bail out (assuming mobs are all dead)
                    
                    if (tank.Location.Distance(AoELocation) > 6 || !tank.Combat)
                    {
                        Utils.Log("** Stop casting Flamestrike **");
                        WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend);
                        WoWMovement.MoveStop(WoWMovement.MovementDirection.JumpAscend);
                        break;
                    }
                    

                    System.Threading.Thread.Sleep(250);
                }
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Blast Wave
        public class NeedToBlastWave : Decorator
        {
            public NeedToBlastWave(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Blast Wave";

                if (Settings.BlastWave.Contains("never")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                //if (!Utils.Adds) return false;
                if (Me.IsMoving) return false;

                WoWUnit tank = Me.IsInInstance ? RaFHelper.Leader : Me;
                if (ClassHelpers.Mage.GotSheep(tank.Location, 10)) return false;
                int count = Utils.CountOfAddsInRange(10, tank.Location);

                if (Settings.BlastWave.Contains("only on adds") && !Utils.Adds) return false;
                if (Settings.BlastWave.Contains("3+ adds") && count < 3) return false;
                if (Settings.BlastWave.Contains("4+ adds") && count < 4) return false;
                if (Settings.BlastWave.Contains("5+ adds") && count < 5) return false;
                if (Settings.BlastWave.Contains("6+ adds") && count < 6) return false;
                if (Settings.BlastWave.Contains("7+ adds") && count < 7) return false;
                if (Settings.BlastWave.Contains("8+ adds") && count < 8) return false;
                if (Settings.BlastWave.Contains("9+ adds") && count < 9) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BlastWave : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Blast Wave";
                double maxDistance = Spell.MaxDistance(dpsSpell) - 1;
                WoWUnit tank = Me.IsInInstance ? RaFHelper.Leader : Me;

                if (tank != Me)
                {
                    while (tank.Distance2D > maxDistance)
                    {
                        // if we're out of Blizzard range then move closer
                        Movement.MoveTo(tank.Location);
                        Utils.Log("Out of range for Blast Wave, moving closer");
                    }

                    if (!Utils.IsInLineOfSight(tank.Location)) Utils.MoveToLineOfSight(tank.Location);
                }
                if (Me.IsMoving) Movement.StopMoving();

                // Once we finally get here, if the tank is moving then just bail out
                if (tank.IsMoving) return RunStatus.Failure;

                // Cast AoE at this location
                WoWPoint AoELocation = tank.Location;
                if (!Me.IsInParty) AoELocation = CT.Location;

                bool result = Spell.Cast(dpsSpell, AoELocation);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Combustion

        public class NeedToCombustion : Decorator
        {
            public NeedToCombustion(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Combustion";

                if (!CLC.ResultOK(Settings.Combustion)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;
                if (!Target.IsDebuffOnTarget("Pyroblast") && !Target.IsDebuffOnTarget("Pyroblast!")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Combustion : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Combustion";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Frost Nova Spell

        public class NeedToFrostNovaSpell : Decorator
        {
            public NeedToFrostNovaSpell(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.FrostNovaSpell;
                if (!Spell.IsKnown(Settings.FrostNovaSpell)) dpsSpell = "Ice Lance";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.IsDebuffOnTarget("Frost Nova")) return false;
                if (Target.IsDistanceMoreThan(Spell.MaxDistance(dpsSpell))) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FrostNovaSpell : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.FrostNovaSpell;
                if (!Spell.IsKnown(Settings.FrostNovaSpell)) dpsSpell = "Ice Lance";

                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        // Procs

        #region Brain Freeze Proc
        public class NeedToBrainFreeze : Decorator
        {
            public NeedToBrainFreeze(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                // 57761 = Brain Freeze proc
                const string spellName = "Fireball";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Self.IsBuffOnMe(57761)) return false;
                if (Self.Buff(57761).TimeLeft.TotalSeconds > 0) return true;
                //if (!Me.Auras.ContainsKey("Brain Freeze")) return false;
                //if (Me.Auras["Brain Freeze"].TimeLeft.TotalSeconds > 0) return true;

                return false;
            }
        }

        public class BrainFreeze : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = Spell.IsKnown("Frostfire Bolt") ? "Frostfire Bolt" : "Fireball";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Hot Streak Proc
        public class NeedToHotStreak : Decorator
        {
            public NeedToHotStreak(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string spellName = "Pyroblast";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (!Self.IsBuffOnMe(48108)) return false;
                if (Self.Buff(48108).TimeLeft.TotalSeconds > 0) return true;

                //if (!Me.Auras.ContainsKey("Hot Streak")) return false;
                //if (Me.Auras["Hot Streak"].TimeLeft.TotalSeconds > 0) return true;

                return false;
            }
        }

        public class HotStreak : Action
        {
            protected override RunStatus Run(object context)
            {
                const string spellName = "Pyroblast";
                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion
      

        #endregion

    }
}