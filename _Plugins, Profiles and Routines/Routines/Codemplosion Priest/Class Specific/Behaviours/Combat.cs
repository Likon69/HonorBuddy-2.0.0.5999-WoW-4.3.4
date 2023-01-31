using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Logic.Combat;
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

                 // Move To
                 new NeedToMoveTo(new MoveTo()),

                 // Shadowform
                 new NeedToShadowform(new Shadowform()),

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
                 new NeedToAutoAttack(new AutoAttack()),

                 // Face Target
                 new NeedToFaceTarget(new FaceTarget()),


                 // ************************************************************************************
                 // Important/time sensative spells here
                 // These are spells that need to be case asap

                 // Dispersion
                 new NeedToDispersion(new Dispersion()),

                 // Shadow Word Death
                 new NeedToShadowWordDeath(new ShadowWordDeath()),

                 // Silence
                 new NeedToSilence(new Silence()),

                 // Shadowfiend
                 new NeedToShadowfiend(new Shadowfiend()),

                 // Shackle
                 new NeedToShackle(new Shackle()),



                 // ************************************************************************************
                 // Other spells here

                 // Archangle
                 new NeedToArchangle(new Archangle()),

                 // Power Word: Barrier
                 new NeedToPowerWordBarrier(new PowerWordBarrier()),

                 // Power Infusion
                 new NeedToPowerInfusion(new PowerInfusion()),

                 // Holy Nova - AoE only
                 new NeedToHolyNova(new HolyNova()),

                 // Holy Fire
                 new NeedToHolyFire(new HolyFire()),

                 // Vampiric Touch
                 new NeedToVampiricTouch(new VampiricTouch()),

                 // Devouring Plague
                 new NeedToDevouringPlague(new DevouringPlague()),

                 // Shadow Word Pain
                 new NeedToShadowWordPain(new ShadowWordPain()),

                 // Psychic Scream
                 new NeedToPsychicScream(new PsychicScream()),

                 // Mind Sear
                 new NeedToMindSear(new MindSear()),

                 // Mind Blast
                 new NeedToMindBlast(new MindBlast()),
                 
                 // MindSpike
                 new NeedToMindSpike(new MindSpike()),

                 // Chastise
                 new NeedToChastise(new Chastise()),

                 // Penance
                 new NeedToPenance(new Penance()),

                 // Chakra
                 new NeedToChakra(new Chakra()),
                 
                 // Smite
                 new NeedToSmite(new Smite()),

                 // Mind Flay
                 new NeedToMindFlay(new MindFlay()),

                 // Fade
                 new NeedToFade(new Fade()),

                 // Hymn of Hope
                 new NeedToHymnOfHope(new HymnOfHope()),
                 
                 // Party Healer
                 new NeedToPartyHealer(new PartyHealer()),
                 
                 // Party Healer UHS
                 //new NeedToPartyHealerUHS(new PartyHealerUHS()),

                 // Party Decurse
                 new NeedToPartyDecurse(new PartyDecurse()),

                 // Archangel
                 //new NeedToArchangle(new Archangle()),

                 // Bounce Prayer of Mending
                 new NeedToBouncePOM(new BouncePOM()),

                 // Heal Pets
                 new NeedToHealPets(new HealPets()),

                 // Smite Evangelism
                 new NeedToSmiteEvangelism(new SmiteEvangelism()),

                 // Wand Party
                 new NeedToWandParty(new WandParty()),

                 // Move To
                 new NeedToMoveTo(new MoveTo()),

                 // Finally just perform an update
                 new Action(ret => ObjectManager.Update())

                 );
         }
        #endregion
        
       


        #region Behaviours
        
        #region Retarget
        public class NeedToRetarget : Decorator
        {
            public NeedToRetarget(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (Me.GotTarget && CT.IsAlive) return false;
                if (!Me.Combat) return false;

                return true;
            }
        }

        public class Retarget : Action
        {
            protected override RunStatus Run(object context)
            {
                List<WoWUnit> hlist = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < 40 && !p.Dead && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet || p.IsTargetingMyRaidMember) && p.Attackable
                     orderby p.HealthPercent ascending
                     orderby p.Distance2D ascending
                     select p).ToList();

                // No sapped target, kill anything. 
                if (hlist.Count > 0) hlist[0].Target();

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
                if (Settings.PartyHealWhen.Contains("dedicated") && Me.IsInInstance) distance = 28;
                return Me.IsMoving || Target.IsDistanceMoreThan(distance);
            }
        }

        public class MoveTo : Action
        {
            protected override RunStatus Run(object context)
            {
                double distance = Settings.MaximumPullDistance;
                double minDistance = Settings.MinimumPullDistance;

                if (Settings.PartyHealWhen.Contains("dedicated") && Me.IsInInstance)
                {
                    distance = 29;
                    minDistance = 25;
                }
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

        #region Shadowform

        public class NeedToShadowform : Decorator
        {
            public NeedToShadowform(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shadowform";
                
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (Self.IsBuffOnMe("Dispersion")) return false;
                if (!CLC.ResultOK(Settings.Shadowform)) return false;
                if (Self.IsBuffOnMe(15473,Self.AuraCheck.AllAuras)) return false;

                // Make sure we don't need to heal ourself first
                if (!Self.IsHealthPercentAbove(Settings.FlashHealHealth)) return false;
                if (!Self.IsHealthPercentAbove(Settings.RenewHealth)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Shadowform : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shadowform";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                return result ? RunStatus.Success : RunStatus.Failure;
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

        #region Smite
        public class NeedToSmite : Decorator
        {
            public NeedToSmite(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Smite";

                if (!Settings.Smite.Contains("when in battleground"))
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.Smite)) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Smite : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Smite";
                bool SWDCD = Spell.IsOnCooldown("Shadow Word: Death");

                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                if (Settings.ShadowWordDeath.Contains("target < 25%") && Spell.IsKnown("Shadow Word: Death") && !SWDCD)
                {
                    while (Me.IsCasting)
                    {
                        ObjectManager.Update();
                        if (Target.HealthPercent < 26)
                        {
                            Spell.StopCasting();
                            while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                            Utils.Log("-Interrupting Smite to cast Shadow Word: Death - PEW PEW PEW!", Utils.Colour("Red"));
                            Spell.Cast("Shadow Word: Death");
                            return RunStatus.Success;
                        }
                    }
                }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shadow Word Pain
        public class NeedToShadowWordPain : Decorator
        {
            public NeedToShadowWordPain(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shadow Word: Pain";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.ShadowWordPain)) return false;
                if (CLC.ResultOK(Settings.MindSpike) && Spell.CanCast("Mind Spike")) return false;
                if (!Settings.ShadowWordPain.Contains("when in battleground"))
                    if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                //if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Timers.Expired("SWPain",3500)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.CanDebuffTarget(dpsSpell)) return false;
                if (!Target.IsElite && !Target.IsHealthPercentAbove(30)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShadowWordPain : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shadow Word: Pain";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = Target.IsDebuffOnTarget(dpsSpell);
                Timers.Reset("SWPain");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Vampiric Touch

        public class NeedToVampiricTouch : Decorator
        {
            public NeedToVampiricTouch(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Vampiric Touch";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.VampiricTouch)) return false;
                if (CLC.ResultOK(Settings.MindSpike) && Spell.CanCast("Mind Spike")) return false;
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (Target.IsDebuffOnTarget(dpsSpell)) return false;
                if (!Timers.Expired("VampiricTouch", 3000)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.IsElite && !Target.IsHealthPercentAbove(30)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class VampiricTouch : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Vampiric Touch";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();
                Utils.WaitWhileCasting();
                
                Timers.Reset("VampiricTouch");
                bool result = Target.IsDebuffOnTarget(dpsSpell);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Mind Blast
        public class NeedToMindBlast : Decorator
        {
            public NeedToMindBlast(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mind Blast";

                //int mindSpikeCount = Target.DebuffStackCount("Mind Spike");
                //int mindSpikeCount = Target.StackCount(87178);
                //Utils.Log("===== Mind Spike: " + mindSpikeCount);

                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Settings.MindBlast.Contains("when in battleground"))
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                //if (!Spell.IsOnCooldown("Smite") && Settings.Smite.Contains("always"))
                    if (!CLC.ResultOK(Settings.MindBlast)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MindBlast : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mind Blast";
                bool SWDCD = Spell.IsOnCooldown("Shadow Word: Death");

                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                if (Settings.ShadowWordDeath.Contains("target < 25%") && Spell.IsKnown("Shadow Word: Death") && !SWDCD)
                {
                    while (Me.IsCasting) { ObjectManager.Update();
                        if (Target.HealthPercent < 26)
                        {
                            Spell.StopCasting();
                            while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                            Utils.Log("-Interrupting Mind Blast to cast Shadow Word: Death - PEW PEW PEW!",Utils.Colour("Red"));
                            Spell.Cast("Shadow Word: Death");
                            return RunStatus.Success;
                        }
                    }
                }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mind Spike
        public class NeedToMindSpike: Decorator
        {
            public NeedToMindSpike(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mind Spike";

                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!CLC.ResultOK(Settings.MindSpike)) return false;

                if (!Settings.Smite.Contains("when in battleground"))
                    if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString()))
                        return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MindSpike : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mind Spike";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Chastise

        public class NeedToChastise : Decorator
        {
            public NeedToChastise(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Chastise";

                if (!CLC.ResultOK(Settings.Chastise)) return false;
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Chastise : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Chastise";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Penance
        public class NeedToPenance : Decorator
        {
            public NeedToPenance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Penance";

                if (!CLC.ResultOK(Settings.Penance)) return false;
                if (!Settings.Penance.Contains("when in battleground"))
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Penance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Penance";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mind Sear
        public class NeedToMindSear : Decorator
        {
            public NeedToMindSear(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mind Sear";

                if (Me.IsMoving) return false;
                if (Settings.MindSear.Contains("never")) return false;
                if (!Me.IsInInstance && !CLC.ResultOK(Settings.MindSear)) return false;
                
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                if (Me.IsInInstance && RAF.PartyTankRole != null && (Utils.CountOfAddsInRange(10, RAF.PartyTankRole.Location) >= 3 && Spell.CanCast(dpsSpell))) return true;

                return Spell.CanCast(dpsSpell);
            }
        }

        public class MindSear : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Mind Sear";
                bool result = Spell.Cast(dpsSpell,RAF.PartyTankRole);
                Utils.LagSleep();
                Utils.WaitWhileCasting();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Mind Flay
        public class NeedToMindFlay: Decorator
        {
            public NeedToMindFlay(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Mind Flay";

                if (!CLC.ResultOK(Settings.MindFlay)) return false;
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class MindFlay : Action
        {
            protected override RunStatus Run(object context)
            {
                bool SWDCD = Spell.IsOnCooldown("Shadow Word: Death");
                const string dpsSpell = "Mind Flay";

                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                if (Settings.ShadowWordDeath.Contains("target < 25%") && Spell.IsKnown("Shadow Word: Death") && !SWDCD)
                {
                    while (Me.IsCasting)
                    {
                        ObjectManager.Update();
                        if (Target.HealthPercent < 26)
                        {
                            Spell.StopCasting();
                            while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                            Utils.Log("-Interrupting Mind Flay to cast Shadow Word: Death - PEW PEW PEW!", Utils.Colour("Red"));
                            Spell.Cast("Shadow Word: Death");
                            return RunStatus.Success;
                        }
                    }
                }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Holy Fire
        public class NeedToHolyFire: Decorator
        {
            public NeedToHolyFire(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Holy Fire";

                if (!CLC.ResultOK(Settings.HolyFire)) return false;
                if (!Settings.HolyFire.Contains("when in battleground"))
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.CanDebuffTarget(dpsSpell)) return false;
                if (!Target.IsElite && !Target.IsHealthPercentAbove(30)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HolyFire : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Holy Fire";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = Target.IsDebuffOnTarget(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Devouring Plague
        public class NeedToDevouringPlague: Decorator
        {
            public NeedToDevouringPlague(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Devouring Plague";

                if (!Me.GotTarget) return false;
                if (!CLC.ResultOK(Settings.DevouringPlague)) return false;
                if (CT.CreatureType == WoWCreatureType.Mechanical) return false;
                if (CLC.ResultOK(Settings.MindSpike) && Spell.CanCast("Mind Spike")) return false;
                if (!Settings.DevouringPlague.Contains("when in battleground"))
                    if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString()))
                        return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Self.IsPowerPercentAbove(Settings.WandMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.CanDebuffTarget(dpsSpell)) return false;
                if (!Target.IsElite && !Target.IsHealthPercentAbove(30)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DevouringPlague : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Devouring Plague";
                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = Target.IsDebuffOnTarget(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shadow Word: Death
        public class NeedToShadowWordDeath: Decorator
        {
            public NeedToShadowWordDeath(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shadow Word: Death";

                if (!Settings.ShadowWordDeath.Contains("when in battleground"))
                    if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                
                ObjectManager.Update();
                if (Settings.ShadowWordDeath.Contains("target < 25% health")) { if (Target.IsHealthPercentAbove(26)) return false; }
                else { if (!CLC.ResultOK(Settings.ShadowWordDeath)) return false; }
                if (!Spell.CanCast(dpsSpell)) return false;

                // Stop casting and SWD them!
                if (Settings.ShadowWordDeath.Contains("target < 25% health") && (!Target.IsHealthPercentAbove(26)) && Me.IsCasting)
                {
                    Spell.StopCasting();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                }

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class ShadowWordDeath : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shadow Word: Death";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shadowfiend
        public class NeedToShadowfiend : Decorator
        {
            public NeedToShadowfiend(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shadowfiend";

                if (Self.IsPowerPercentAbove(Settings.ShadowfiendMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.IsElite && !Target.IsHealthPercentAbove(30) && !Utils.Adds) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Shadowfiend : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shadowfiend";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                Utils.WaitWhileCasting();
                while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                Spell.Cast("Shadow Word: Pain");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Silence
        public class NeedToSilence: Decorator
        {
            public NeedToSilence(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Silence";

                if (!CLC.ResultOK(Settings.Silence)) return false;
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Target.CanDebuffTarget(dpsSpell)) return false;
                if (!Target.IsCasting) return false;
                //if (!Target.IsElite && !Target.IsHealthPercentAbove(30)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Silence : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Silence";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion
        
        #region Power Infusion
        public class NeedToPowerInfusion : Decorator
        {
            public NeedToPowerInfusion(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Power Infusion";

                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!CLC.ResultOK(Settings.PowerInfusion)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Self.CanBuffMe(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PowerInfusion : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Power Infusion";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Shackle
        public class NeedToShackle : Decorator
        {
            public NeedToShackle(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shackle Undead";

                if (!CLC.ResultOK(Settings.ShackleUndead)) return false;
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Utils.Adds) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!ClassHelpers.Common.CanCrowdControlUnit(dpsSpell,30,WoWCreatureType.Undead)) return false;
                if (ClassHelpers.Common.GotCrowdControledUnit(dpsSpell,30) != null) return false;

                //if (!Self.CanBuffMe(dpsSpell)) return false;
                //if (!Target.CanDebuffTarget(dpsSpell)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Shackle : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shackle Undead";
                ClassHelpers.Common.CrowdControlUnit(dpsSpell, 30, WoWCreatureType.Undead);
                Utils.LagSleep();
                Utils.WaitWhileCasting();
                //bool result = Spell.Cast(dpsSpell);
                //Utils.LagSleep();
                //bool result = Target.IsDebuffOnTarget(dpsSpell);
                //bool result = Self.IsBuffOnMe(dpsSpell);););

                return RunStatus.Success; //: RunStatus.Failure;
            }
        }
        #endregion

        #region Psychic Scream
        public class NeedToPsychicScream : Decorator
        {
            public NeedToPsychicScream(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Psychic Scream";

                if (!CLC.ResultOK(Settings.PsychicScream)) return false;

                if (!Settings.ShadowWordDeath.Contains("when in battleground"))
                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;
                if (!Self.IsPowerPercentAbove(Settings.ReserveMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Target.IsDistanceMoreThan(8)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PsychicScream : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Psychic Scream";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Dispersion
        public class NeedToDispersion : Decorator
        {
            public NeedToDispersion(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Dispersion";

                // Stop logic processing any further until Dispersion is gone
                if (Self.IsBuffOnMe(dpsSpell)) return true;

                if (Self.IsPowerPercentAbove(Settings.DispersionMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Dispersion : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Dispersion";

                // Stop logic processing any further until Dispersion is gone
                if (Self.IsBuffOnMe(dpsSpell)) return RunStatus.Success;

                Spell.Cast(dpsSpell);
                Utils.LagSleep();

                bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Fade - Instance only
        public class NeedToFade : Decorator
        {
            public NeedToFade(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Fade";

                //if (!Me.IsInInstance) return false;
                if (!Me.IsInParty) return false;
                if (Utils.IsBattleground) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Utils.CountOfMobsAttackingPlayer(Me.Guid) <= 0) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Fade : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Fade";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Chakra
        public class NeedToChakra : Decorator
        {
            public NeedToChakra(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Chakra";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(81207)) return false;           // Sanctuary
                if (Self.IsBuffOnMe(81208)) return false;           // Serenity
                if (Self.IsBuffOnMe(81209)) return false;           // Chastise
                if (Self.IsBuffOnMe(dpsSpell)) return false;        // Chakra

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Chakra : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Chakra";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Hymn of Hope
        public class NeedToHymnOfHope : Decorator
        {
            public NeedToHymnOfHope(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Hymn of Hope";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsPowerPercentAbove(Settings.HymnOfHopeMana)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HymnOfHope : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Hymn of Hope";
                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();
                Utils.WaitWhileCasting();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Smite Evangelism
        public class NeedToSmiteEvangelism : Decorator
        {
            public NeedToSmiteEvangelism(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Smite";

                if (!Me.GotTarget) return false;
                if (!Timers.Expired("ArcSmiteCombat",2500)) return false;
                if (!Me.IsInInstance && !Utils.IsBattleground) return false;
                if (Settings.SmiteEvangelism.Contains("never")) return false;
                if (Target.IsDistanceMoreThan(30)) return false;
                if (Target.HealthPercent < 35) return false;
                if (Settings.SmiteEvangelism.Contains("90%") && !Self.IsPowerPercentAbove(90)) return false;
                if (Settings.SmiteEvangelism.Contains("85%") && !Self.IsPowerPercentAbove(85)) return false;
                if (Settings.SmiteEvangelism.Contains("80%") && !Self.IsPowerPercentAbove(80)) return false;
                if (Settings.SmiteEvangelism.Contains("75%") && !Self.IsPowerPercentAbove(75)) return false;
                if (Settings.SmiteEvangelism.Contains("70%") && !Self.IsPowerPercentAbove(70)) return false;
                if (Settings.SmiteEvangelism.Contains("65%") && !Self.IsPowerPercentAbove(65)) return false;
                if (Settings.SmiteEvangelism.Contains("60%") && !Self.IsPowerPercentAbove(60)) return false;
                if (Settings.SmiteEvangelism.Contains("55%") && !Self.IsPowerPercentAbove(55)) return false;
                if (Settings.SmiteEvangelism.Contains("50%") && !Self.IsPowerPercentAbove(50)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (RAF.HealPlayer(Settings.SmiteEvangelismHealth, 50) != null) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SmiteEvangelism : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Smite";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Archangle (Solo / Questing)
        public class NeedToArchangle : Decorator
        {
            public NeedToArchangle(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Archangel";

                if (!Me.ActiveAuras.ContainsKey("Evangelism")) return false;
                if ((Me.IsInInstance || Utils.IsBattleground) && !CLC.ResultOK(Settings.ArchangelParty)) return false;
                if ((!Me.IsInInstance && !Utils.IsBattleground) && !CLC.ResultOK(Settings.Archangel)) return false;

                if (Me.IsCasting) return false;
                if (!Spell.CanCastLUA(dpsSpell)) return false; // Using LUA because the normal method fails

                //Utils.Log("********************** this... is... it!");
                const string spell = "Evangelism";
                const string archangle = "Archangel";
                double getTime = Convert.ToDouble(Self.GetTimeLUA());
                double buffTime = Convert.ToDouble(Self.BuffTimeLeftLUA(spell));
                double secondsRemaining = buffTime - getTime;

                if (secondsRemaining < 4.5 && Spell.CanCastLUA(archangle))
                {
                    Utils.Log("-Evangelism buff about to expire. Casting Archangel buff to consume it", Utils.Colour("Red"));
                    return true;
                }

                if (Me.ActiveAuras.ContainsKey("Archangel")) return false;
                return (Spell.CanCastLUA(dpsSpell));
            }
        }

        public class Archangle : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Archangel";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region BouncePOM
        public class NeedToBouncePOM : Decorator
        {
            public NeedToBouncePOM(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Shadow Word: Death";

                if (!CLC.ResultOK(Settings.BouncePoM)) return false;
                if (!Self.IsBuffOnMe("Prayer of Mending")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BouncePOM : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Shadow Word: Death";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
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

                if ((Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhen.Contains("dedicated"))
                {
                    if (!Me.IsInParty) return false;
                    if (!CLC.ResultOK(Settings.WandParty)) return false;
                    if (!Me.GotTarget) return false;
                    if (!Utils.IsInLineOfSight(Me.CurrentTarget.Location)) return false;
                    if (Target.IsDistanceMoreThan(30)) return false;
                    if (Spell.IsGCD || Me.IsCasting) return false;
                    if (!Utils.IsNotWanding) return false;
                    if (RAF.HealPlayer(Settings.SmiteEvangelismHealth, 50) != null) return false;
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

        #region Power Word: Barrier
        public class NeedToPowerWordBarrier : Decorator
        {
            public NeedToPowerWordBarrier(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Power Word: Barrier";

                if (!CLC.ResultOK(Settings.PowerWordBarrier)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PowerWordBarrier : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Power Word: Barrier";
                Spell.Cast(dpsSpell,Me.Location);
                //System.Threading.Thread.Sleep(500);



                return RunStatus.Success;
            }
        }
        #endregion

        #region Holy Nova
        public class NeedToHolyNova : Decorator
        {
            public NeedToHolyNova(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Holy Nova";

                if (Settings.HolyNova.Contains("never")) return false;

                if (!Settings.HolyNova.Contains("when in battleground"))
                    if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) return false;

                if (!Utils.Adds) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                int count = Utils.CountOfAddsInRange(10, Me.Location);
                
                
                if (Settings.HolyNova.Contains("3+ adds") && count < 3) return false;
                if (Settings.HolyNova.Contains("4+ adds") && count < 4) return false;
                if (Settings.HolyNova.Contains("5+ adds") && count < 5) return false;
                if (Settings.HolyNova.Contains("6+ adds") && count < 6) return false;
                if (Settings.HolyNova.Contains("7+ adds") && count < 7) return false;
                if (Settings.HolyNova.Contains("8+ adds") && count < 8) return false;
                if (Settings.HolyNova.Contains("9+ adds") && count < 9) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HolyNova : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Holy Nova";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        

        #region Heal Pets
        public class NeedToHealPets : Decorator
        {
            public NeedToHealPets(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Flash Heal";

                if (!CLC.ResultOK(Settings.HealPets)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Self.IsPowerPercentAbove(40)) return false;

                // Party members with pets, and pets need healing, and can cast spell - return true
                return Me.PartyMembers.Any( p => p.GotAlivePet && (p.Pet.HealthPercent < Settings.RenewHealth || p.Pet.HealthPercent < Settings.FlashHealHealth)) && Spell.CanCast(dpsSpell);
            }
        }

        public class HealPets : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = false;
                foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.GotAlivePet && (p.Pet.HealthPercent < Settings.RenewHealth || p.Pet.HealthPercent < Settings.FlashHealHealth)))
                {
                    result = ClassHelpers.Priest.PartyHealer(p.Pet);
                    if (result) break;
                }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Party Decurse
        public class NeedToPartyDecurse : Decorator
        {
            public NeedToPartyDecurse(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Dispel Magic";

                if (!CLC.ResultOK(Settings.PartyCleanse)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (!Me.IsInMyParty && !Me.IsInRaid) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PartyDecurse : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = false;

                // Dispel Magic - You and all party members);
                if (!Settings.PartyCleanse.Contains("never") && Spell.CanCast("Dispel Magic"))
                {
                    List<int> urgentRemoval = new List<int> { 17173 };
                    bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                    if (urgentCleanse || CLC.ResultOK(Settings.PartyCleanse))
                    {
                        List<WoWDispelType> cureableList = new List<WoWDispelType> { WoWDispelType.Magic };
                        var p = ClassHelpers.Common.DecursePlayer(cureableList,true);
                        if (p != null) { if (Spell.CanCast("Dispel Magic")) { result = Spell.Cast("Dispel Magic", p); } }
                    }
                }

                // Cure Disease - You and all party members);
                if (!Settings.PartyCleanse.Contains("never") && Spell.CanCast("Cure Disease"))
                {
                    List<int> urgentRemoval = new List<int> { 3427 };
                    bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                    if (urgentCleanse || CLC.ResultOK(Settings.PartyCleanse))
                    {
                        List<WoWDispelType> cureableList = new List<WoWDispelType> { WoWDispelType.Disease };
                        var p = ClassHelpers.Common.DecursePlayer(cureableList, true);
                        if (p != null) { if (Spell.CanCast("Cure Disease")) result = Spell.Cast("Cure Disease", p); }
                    }
                }

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Party Healer
        public class NeedToPartyHealer : Decorator
        {
            public NeedToPartyHealer(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Me.IsInParty && !Me.IsInRaid) return false;
                if (Settings.PartyHealWhen.Contains("never")) return false;
                if (!Utils.CombatCheckOk("Flash Heal", false)) return false;
                if (Settings.PartyHealWhen.Contains("OOM or Dead"))
                {
                    WoWUnit healer = RAF.PartyHealerRole;
                    if (healer != null && (healer.Dead || healer.IsGhostVisible)) return true;
                    if (healer != null && healer.ManaPercent < Settings.PartyHealerOOM) return true;
                    
                    return false;
                }

                return true;
            }
        }

        public class PartyHealer : Action
        {
            protected override RunStatus Run(object context)
            {
                WoWUnit healer = RAF.PartyHealerRole;
                WoWUnit tank = RAF.PartyTankRole;

                // Circle of Healing
                int CoHCount = Convert.ToInt16(Settings.CircleOfHealingCount);
                List<WoWPlayer> myPartyOrRaidGroup = Me.PartyMembers;
                List<WoWPlayer> CoH = (from o in myPartyOrRaidGroup let p = o.ToPlayer() where p.Distance < 40 && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < Settings.CircleOfHealingHealth orderby p.HealthPercent ascending select p).ToList();
                if (CoH.Count >= CoHCount && Spell.CanCast("Circle of Healing"))
                {
                    Spell.Cast("Circle of Healing", CoH[0]);
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                    return RunStatus.Success;
                }

                // Heal the almighty Tank - But only if we don't have any urgent heals and tank > 80
                if (tank != null && !tank.Dead)
                {
                    double urgentHealth = tank.HealthPercent*0.60f;
                    WoWUnit urgentHealTarget = RAF.HealPlayer((int)urgentHealth, 40);
                    if (urgentHealTarget != null && tank.HealthPercent > 80)
                    {
                        Utils.Log(string.Format("-Urgent heal required on {0}, prioritizing over the tank", urgentHealTarget.Class),Utils.Colour("Red"));
                        ClassHelpers.Priest.PartyHealer(urgentHealTarget);
                    }

                    bool result = ClassHelpers.Priest.PartyHealer(tank);
                    if (result) return RunStatus.Success;
                }

                // Prayer of Healing
                int PoHCount = Convert.ToInt16(Settings.PrayerOfHealingCount);
                List<WoWPlayer> partyMembers = Me.PartyMembers;
                List<WoWPlayer> PoH = (from o in partyMembers let p = o.ToPlayer() where p.Distance < 30 && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < Settings.PrayerOfHealingHealth select p).ToList();
                if (PoH.Count >= PoHCount && Spell.CanCast("Prayer of Healing"))
                {
                    if (Spell.CanCast("Inner Focus") && !Self.IsBuffOnMe("Inner Focus")) { Spell.Cast("Inner Focus"); System.Threading.Thread.Sleep(500); }
                    Spell.Cast("Prayer of Healing");
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                    return RunStatus.Success;
                }

                // Heal Me
                if (Me != null && !Me.Dead)
                {
                    bool result = ClassHelpers.Priest.PartyHealer(Me);
                    if (result) return RunStatus.Success;
                }
                
                // Party healer
                if (healer != null && !healer.Dead && healer != Me)
                {
                    bool result = ClassHelpers.Priest.PartyHealer(healer);
                    if (result) return RunStatus.Success;
                }

                // Crude way to get the upper value of all settings
                int upperHealthValue;
                List<int> healthValues = new List<int> { Settings.PartyGuardianSpirit, Settings.PartyPrayerOfMending, Settings.PartyPenance, Settings.PartyPainSuppression, Settings.PartyPWS, Settings.PartyRenew, Settings.PartyFlashHeal, Settings.PartyGreaterHeal };
                healthValues.Sort();
                upperHealthValue = healthValues[healthValues.Count - 1];


                // Everyone else in the party gets healed
                bool everyoneResult = false;
                WoWUnit healTarget = RAF.HealPlayer(upperHealthValue, 50);
                if (healTarget != null && !healTarget.Dead)
                {
                    everyoneResult = ClassHelpers.Priest.PartyHealer(healTarget);
                }

                return everyoneResult ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Party Healer UHS
        public class NeedToPartyHealerUHS : Decorator
        {
            public NeedToPartyHealerUHS(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                bool result = false;
                if (!Me.IsInParty && !Me.IsInRaid) return false;
                if (Settings.PartyHealWhen.Contains("never")) return false;
                if (!Utils.CombatCheckOk("Flash Heal", false)) return false;
                if (Settings.PartyHealWhen.Contains("OOM or Dead"))
                {
                    WoWUnit healer = RAF.PartyHealerRole;
                    if (healer != null && (healer.Dead || healer.IsGhostVisible)) result = true;
                    if (healer != null && healer.ManaPercent < Settings.PartyHealerOOM) result = true;
                }

                if (Settings.PartyHealWhen.Contains("dedicated healer") && (Me.IsInInstance || Me.IsInRaid || Utils.IsBattleground) && Settings.PartyHealWhenSpec.Contains(ClassHelpers.Priest.ClassSpec.ToString())) result = true;

                if (result)
                {
                    // Clear everything out
                    HealBot.UHS.Clear();

                    // Populate the UHS
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Circle of Healing",Priority = 0,MaximumDistance = 40, EvaluateBeforeTank = true, IsAoE = true, HealthPercent = Settings.CircleOfHealingHealth, MinimumAoECount = Convert.ToInt16(Settings.CircleOfHealingCount) });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Guardian Spirit", Priority = 0, MaximumDistance = 40, HealthPercent = Settings.PartyGuardianSpirit });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Prayer of Mending", Priority = 10, MaximumDistance = 40, HealthPercent = Settings.PartyPrayerOfMending, IsBuff = true });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Power Word: Shield", Priority = 30, MaximumDistance = 40, HealthPercent = Settings.PartyPWS, IsBuff = true, OtherDebuffs = "Weakened Soul", MinimumMobAggroCount = 1 });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Penance", Priority = 50, MaximumDistance = 40, HealthPercent = Settings.PartyPenance });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Pain Suppression", Priority = 60, MaximumDistance = 40, HealthPercent = Settings.PartyPainSuppression, IsBuff = true });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Prayer of Healing", MaximumDistance = 40, Priority = 70, IsAoE = true, HealthPercent = Settings.PrayerOfHealingHealth, MinimumAoECount = Convert.ToInt16(Settings.PrayerOfHealingCount) });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Renew", IncludePets = true, Priority = 80, MaximumDistance = 40, HealthPercent = Settings.PartyRenew, IsBuff = true });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Flash Heal", IncludePets = true, MaximumDistance = 40, Priority = 90, HealthPercent = Settings.PartyFlashHeal });
                    HealBot.UHS.Add(new HealBot.HealingSpell { SpellName = "Greater Heal", MaximumDistance = 40, Priority = 100, HealthPercent = Settings.PartyGreaterHeal });

                    // Sort spells based on priority
                    HealBot.Sort();

                    // Is it ok to heal pets?
                    HealBot.HealPets = Settings.HealPets.Contains("always");
                }


                return result;
            }
        }

        public class PartyHealerUHS : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = HealBot.Heal();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #endregion

    }
}