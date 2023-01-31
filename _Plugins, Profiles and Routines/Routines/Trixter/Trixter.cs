// Trixter by mahe4
// but lovly stolen from fifty pence ;P (with permission)
//
// As always, if you wish to reuse code, please seek permission first 
// and provide credit where appropriate

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using CommonBehaviors.Actions;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Trixter
{
    public class Trixter : CombatRoutine
    {
        
        public override sealed string Name { get { return "Trixter v1.5.0"; } }
        public override WoWClass Class { get { return WoWClass.Rogue; } }

        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public override void Initialize()
        {
            Logging.Write("");
            Logging.Write("Trixter v1.5.0 is now operational.");
            Logging.Write("All Credits are going to:");
            Logging.Write("fiftypence for his MutaRaidBT");
            Logging.Write("strix for some LazyRogue Methodes");
            Logging.Write("");
            Logging.Write("");
            
        }

        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            TrixterForm conf = new TrixterForm();
            conf.ShowDialog();
        }

        #region Variables

        // *********************************************************************
        // * Change Settings here!!!                                           *
        // *********************************************************************
        public static Boolean UseKillingSpree = false;
        public static Boolean UseRecup = false;
        public static Boolean UseAdrenalin = true;
        public static Boolean UseRupture = true;
        public static Boolean UseBladeFlurry = true;
        
        public static Boolean UseVanish = true;
        public static Boolean UseVendetta = true;
        public static Boolean UseColdBlood = true;

        public static Boolean UseInterrupt = false;
        public static Boolean UseRedirect = false;
        // *********************************************************************
        // * Dont change something below, if you don't know what you are doing *
        // *********************************************************************

        public static String TotTTarget = "";
        public static WoWUnit TotTTargetGUID = null;
        private WoWUnit lasttarget = null;
        private int CurrentEnergy = 0;
        private WoWPlayer FocusTarget = null;
        public Boolean GarroteNow = false;

        #endregion

        #region Combat


        // *********************************************
        // * Combat. Now with BehaviorTree magic!      *
        // *********************************************
        // * TODO:                                     *
        // *                                           *
        // *  Interrupts                               *
        // *********************************************

        private Composite _CombatBehavior;

        public override Composite CombatBehavior 
        { 
            get 
            {
                if (_CombatBehavior == null)
                {
                    Logging.Write("Building behavior tree.");

                    _CombatBehavior = BuildBehaviorTree();
                }

                return _CombatBehavior; 
            } 
        }

        private Composite BuildBehaviorTree()
        {
            return new Decorator(ret => !Me.Mounted && Me.CurrentTarget != null,
                new PrioritySelector(
                    new Action(delegate {
                        UpdateEnergy();
                        //SetFocus();
                        //targetcheck();
                        TotT();
                        //toggleoff();
                        trinketspam();
                        return RunStatus.Failure; }),

                    //Combat for Every Specc
                    //AutoAttack(),
                    //CastSpellOnFocus("Tricks of the Trade"),
                    //CastSpell("Kick", ret => LetsInterrupt() && KickSpecific == true),
                    //CastFeint(ret => Lua.GetReturnVal<int>("return FeintNow", 0) == 1),
                    CastSpell("Kick", ret => Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast && UseInterrupt == true),
                    

                    //Combat for Combat Specc
                    new Decorator(ret => SpellManager.HasSpell(13877),
                        new PrioritySelector(
                            //CastAdrenalineRush(ret => Lua.GetReturnVal<int>("return AdrenalineNow", 0) == 1),
                            //CastSpell("Killing Spree", ret => Lua.GetReturnVal<int>("return KSNow", 0) == 1),
                            //CastRedirect(ret => lasttarget != Me.CurrentTarget && UseRedirect && (PlayerHasBuff("Shallow Insight") || PlayerHasBuff("Shallow Insight") || PlayerHasBuff("Shallow Insight"))),
                            CastSpell("Redirect", ret => Me.ComboPoints < Me.RawComboPoints && UseRedirect),
                            CastSpell("Blade Flurry", ret => UseBladeFlurry && GetHostileInRange(6, true, false).Count > 1 && !PlayerHasBuff("Blade Flurry")),
                            JustCastSpell("Blade Flurry", ret => UseBladeFlurry && GetHostileInRange(6, true, false).Count < 2 && PlayerHasBuff("Blade Flurry")),

                            CastSpell("Slice and Dice", ret => (PlayerBuffTimeLeft("Slice and Dice") < 4) && Me.ComboPoints >= 1),
                            CastSpell("Recuperate", ret => CurrentEnergy >= 30 && Me.HealthPercent < 75 && !PlayerHasBuff("Recuperate") && Me.RawComboPoints > 2 && UseRecup == true),
                            
                            CastSpell("Adrenaline Rush", ret => CurrentEnergy <= 40 && UseAdrenalin == true),
                            CastSpell("Killing Spree", ret => CurrentEnergy <= 40 && !PlayerHasBuff("Adrenaline Rush") && UseKillingSpree == true),
                            CastSpell("Rupture", ret => IsTargetBoss() && TargetDebuffTimeLeft("Rupture") == 0 && Me.ComboPoints == 5 && !PlayerHasBuff("Blade Flurry") && TargetHasBleed() && UseRupture == true),
                            CastSpell("Eviscerate", ret =>  Me.ComboPoints == 5),
                            CastSpell("Revealing Strike", ret =>  Me.ComboPoints == 4),
                            CastSpell("Sinister Strike", ret => true)
                            
                        )
                    ),

                    //Combat For Assassination Specc
                    new Decorator(ret => SpellManager.HasSpell(1329),
                        new PrioritySelector(
                            CastGarrote( ret => true),
                            CastVanish(ret => !PlayerHasBuff("Overkill") && CurrentEnergy > 60 && UseVanish),
                            CastSpell("Redirect", ret => Me.ComboPoints < Me.RawComboPoints && UseRedirect),
                            CastSpell("Vendetta", ret => UseVendetta),
                            CastSpell("Fan of Knives", ret => GetHostileInRange(6, true, false).Count > 3),
                            CastSpell("Slice and Dice", ret => !PlayerHasBuff("Slice and Dice") && Me.ComboPoints >= 1),
                            CastSpell("Rupture", ret => TargetDebuffTimeLeft("Rupture") < 2 && Me.ComboPoints >= 1),
                            CastSpell("Cold Blood", ret => Me.ComboPoints ==5 && CurrentEnergy <=90 && !PlayerHasBuff("Cold Blood") && UseColdBlood),
                            CastSpell("Envenom", ret => (TargetDebuffTimeLeft("Envenom") < 2 && Me.ComboPoints >= 4) || (CurrentEnergy >= 90 && Me.ComboPoints >= 4)),
                            CastSpell("Backstab", ret => Me.CurrentTarget.HealthPercent < 35 && BehindTarget()),
                            CastSpell("Mutilate", ret => true)
                        )
                    ),

                    //Combat for Sublety Specc
                    new Decorator(ret => SpellManager.HasSpell(36554),
                        new PrioritySelector(
                            //CD Rotation
                            new Decorator(ret => TargetDebuffTimeLeft("Find Weakness") == 0 ,
                                        new PrioritySelector(
                                            CastSpell("Shadow Dance", ret => true),
                                            new Decorator(ret => CanCastSpell("Vanish") && !PlayerHasBuff("Master of Subtlety") && CurrentEnergy > 60,
                                                new Sequence(
                                                    CastCooldown("Vanish"),
                                                    new WaitContinue(1, ret => false, new ActionAlwaysSucceed()),
                                                    CastSpell("Shadowstep"),
                                                    CastSpell("Ambush")
                                                )
                                            ),
                                            CastSpell("Preparation", ret => SpellCooldown("Vanish") > 0)
                                        )
                            ),
                            //CP builder
                            new Decorator(ret => Me.ComboPoints < 4,
                                        new PrioritySelector(
                                            CastSpell("Premeditation", ret => Me.IsStealthed || PlayerHasBuff("Shadow Dance")),
                                            CastSpell("Shadowstep", ret => Me.IsStealthed || PlayerHasBuff("Shadow Dance")),
                                            CastSpell("Ambush", ret => Me.IsStealthed || PlayerHasBuff("Shadow Dance")),
                                            CastSpell("Hemorrhage", ret => TargetDebuffTimeLeft("89775") == 0),
                                            CastSpell("Backstab", ret => BehindTarget()),
                                            CastSpell("Hemorrhage", ret => true)
                                        )
                            ),
                            //General Finisher
                            new Decorator(ret => Me.ComboPoints == 5,
                                        new PrioritySelector(
                                            CastSpell("Rupture", ret => PlayerHasBuff("Master of Subtlety") && TargetDebuffTimeLeft("Rupture") < 7),
                                            CastSpell("Slice and Dice", ret => !PlayerHasBuff("Slice and Dice")),
                                            CastSpell("Rupture", ret => TargetDebuffTimeLeft("Rupture") < 7),
                                            CastSpell("Recuperate", ret => !PlayerHasBuff("Recuperate")),
                                            CastSpell("Eviscerate", ret => true)
                                        )
                            )
                        )
                    )
                   
                    
                )
            );
        }

        #endregion

        #region Helpers

        //******************************************************************************************
        //* The following helpers may be reused at will, as long as credit is given to fiftypence. *
        //******************************************************************************************

        #region Non-BT Helpers

        /// <summary>
        ///     List of nearby enemy units that pass certain criteria, this list should only return units 
        ///     in active combat with the player, the player's party, or the player's raid.
        /// </summary>

        private List<WoWUnit> EnemyUnits
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                    .Where(unit =>
                        !unit.IsFriendly
                        && (unit.IsTargetingMeOrPet
                           || unit.IsTargetingMyPartyMember
                           || unit.IsTargetingMyRaidMember
                           || unit.IsPlayer)
                        && !unit.IsNonCombatPet
                        && !unit.IsCritter
                        && unit.Distance2D
                     <= 12).ToList();
            }
        }

        /// <summary>
        ///     Uses Lua to update current energy resource of the player. This is used
        ///     to fix the slow updating of ObjectManager.Me.CurrentEnergy.
        /// </summary>

        private void UpdateEnergy()
        {
            CurrentEnergy = Lua.GetReturnVal<int>("return UnitMana(\"player\");", 0);
        }

        /// <summary>
        ///     Uses Lua to find the GUID of the player's focus target then updates the 
        ///     global WoWPlayer FocusTarget to the new unit if appropriate. 
        /// </summary>
        
        private void SetFocus()
        {
            WoWPlayer Focus;
            string FocusGUID = Lua.GetReturnVal<string>("local GUID = UnitGUID(\"focus\"); if GUID == nil then return 0 else return GUID end", 0);

            if (FocusGUID == Convert.ToString(0))
            {
                if (FocusTarget != null)
                {
                    FocusTarget = null;

                    Logging.Write("Focus dropped -- clearing focus target.");
                }

                return;
            }

            // Remove the two starting characters (0x) from the GUID returned by lua using substring.
            // This is done so we can convert the string to a ulong.
            // Then we should set the WoWPlayer Focus to the unit which belongs to our formatted GUID.

            Focus = ObjectManager.GetAnyObjectByGuid<WoWPlayer>(ulong.Parse(FocusGUID.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier));

            if (FocusTarget != Focus && Focus.Distance2D < 100 && Focus.InLineOfSight && !Focus.Dead && Focus.IsInMyPartyOrRaid)
            {
                FocusTarget = Focus;

                Logging.Write("Setting " + FocusTarget.Name + " as focus target.");
            }
        }

        /// <summary>
        ///     Uses WoWMathHelper to ensure we are behind the target.
        ///     A 2 radians cone is tested against. We must be within 1.34r (behind target) to return true.
        ///     Hopefully this should fix HonorBuddy's IsBehind bug.
        /// </summary>

        private bool BehindTarget()
        {
            if (WoWMathHelper.IsBehind(Me.Location, Me.CurrentTarget.Location, Me.CurrentTarget.Rotation, 2f))
                return true;
            else return false;
        }

        /// <summary>
        ///     Checks to see if specified target is under the effect of crowd control
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns></returns>

        private bool IsCrowdControlled(WoWUnit target)
        {
            // Just want to throw a shout-out to Singular for this function.
            return target.GetAllAuras().Any(
            unit => unit.Spell.Mechanic == WoWSpellMechanic.Banished
                || unit.Spell.Mechanic == WoWSpellMechanic.Charmed
                || unit.Spell.Mechanic == WoWSpellMechanic.Horrified
                || unit.Spell.Mechanic == WoWSpellMechanic.Incapacitated
                || unit.Spell.Mechanic == WoWSpellMechanic.Polymorphed
                || unit.Spell.Mechanic == WoWSpellMechanic.Sapped
                || unit.Spell.Mechanic == WoWSpellMechanic.Shackled
                || unit.Spell.Mechanic == WoWSpellMechanic.Asleep
                || unit.Spell.Mechanic == WoWSpellMechanic.Frozen
            );
        }

        /// <summary>
        ///     Checks to see if any nearby units are under breakable crowd control.
        /// </summary>

        private bool ShouldWeAoe()
        {
            foreach (WoWUnit unit in EnemyUnits)
            {
                if (IsCrowdControlled(unit))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Check if player has specified active buff using Lua.
        /// </summary>
        /// <param name="BuffName"></param>

        private bool PlayerHasBuff(string BuffName)
        {
            string BuffNameLua = Lua.GetReturnValues(string.Format("local name = UnitBuff(\"player\", \"{0}\"); " +
                                                 "return name", BuffName))[0];

            if (BuffNameLua == BuffName) return true;
            else return false;
        }

        private bool TargetHasBuff(string BuffName)
        {
            string BuffNameLua = Lua.GetReturnValues(string.Format("local name = UnitBuff(\"target\", \"{0}\"); " +
                                                 "return name", BuffName))[0];

            if (BuffNameLua == BuffName) return true;
            else return false;
        }

        /// <summary>
        ///     Returns the duration of a buff on the player
        /// </summary>
        /// <param name="DebuffName"></param>

        private double PlayerBuffTimeLeft(string BuffName)
        {
            return double.Parse(Lua.GetReturnValues(string.Format("local expirationTime = select(7, UnitBuff(\"player\", \"{0}\", nil, \"player\")); " +
                                     "if expirationTime == nil then return 0 else return expirationTime - GetTime() end", BuffName))[0]);
        }

        /// <summary>
        ///     Check if target has specified debuff using Lua, and returns the time remaining on the debuff.
        /// </summary>
        /// <param name="DebuffName"></param>

        private double TargetDebuffTimeLeft(string DebuffName)
        {
            return double.Parse(Lua.GetReturnValues(string.Format("local expirationTime = select(7, UnitDebuff(\"target\", \"{0}\", nil, \"player\")); " +
                                     "if expirationTime == nil then return 0 else return expirationTime - GetTime() end", DebuffName))[0]);
        }

        private double TargetDebuffTimeLeftAll(string DebuffName)
        {
            return double.Parse(Lua.GetReturnValues(string.Format("local expirationTime = select(7, UnitDebuff(\"target\", \"{0}\", nil)); " +
                                     "if expirationTime == nil then return 0 else return expirationTime - GetTime() end", DebuffName))[0]);
        }

        /// <summary>
        ///     Gets the cooldown of specified spell using Lua
        /// </summary>
        /// <param name="SpellName">Specified Spell</param>
        /// <returns>Spell cooldown</returns>

        public double SpellCooldown(string SpellName)
        {
            Double CDleft = double.Parse(Lua.GetReturnValues(string.Format("local start, duration = GetSpellCooldown(\"{0}\"); " +
                                               "return (start+duration) - GetTime()", SpellName))[0]);
            
            if (CDleft != null)
                return CDleft;
            else
            {
                return 0;
            }
                
        }

        /// <summary>
        ///     Check if player's target is a boss using Lua.
        /// </summary>

        private bool IsTargetBoss()
        {
            string UnitClassification = Lua.GetReturnValues("local classification = UnitClassification(\"target\"); return classification")[0];
            string UnitLevel = Lua.GetReturnValues("local level = UnitLevel(\"target\"); return level")[0];
            if (!Me.IsInRaid)
            {
                if (UnitClassification == "worldboss" ||
                   (Me.CurrentTarget.Level == 87 && Me.CurrentTarget.Elite) ||
                   (Me.CurrentTarget.Level == 88))
                    return true;

                else return false;
            }
            else
            {
                if (UnitLevel == "-1")
                    return true;

                else return false;
            }
            
            
        }

        /// <summary>
        ///     Returns true if a spell is not on cooldown or if
        ///     there is 0.3 or less left on GCD, AND the spell is known
        /// </summary>

        private bool CanCastSpell(string SpellName)
        {
            return (SpellCooldown(SpellName) <= 0.3 && SpellManager.HasSpell(SpellName));
        }

        #endregion

        #region BT Helpers

        #region Cast Spell

        /// <summary>
        ///     Checks if specified spell is castable, if so casts it and writes to log.
        /// </summary>
        /// <param name="SpellName">Spell name</param>
        /// <returns></returns>

        public Composite CastSpell(string SpellName)
        {
            return new Decorator(ret => Me.CurrentTarget != null && CanCastSpell(SpellName),
                new Action(delegate {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", SpellName));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + SpellName); })
                    );
        }

        /// <summary>
        ///     Checks if specified spell is castable, if so casts it and writes to log.
        ///     Uses specified conditions.
        /// </summary>
        /// <param name="SpellName">Spell name</param>
        /// <param name="Conditions">Specified conditions</param>
        /// <returns></returns>

        public Composite CastSpell(string SpellName, CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && CanCastSpell(SpellName),
                new Action(delegate {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", SpellName));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + SpellName); })
            );
        }

        /// <summary>
        ///     Checks if specified spell is castable, if so casts it and writes to log.
        ///     Casts on focus
        /// </summary>
        /// <param name="SpellName">Spell name</param>
        /// <returns></returns>

        #endregion

        #region Cast Focus

        public Composite CastSpellOnFocus(string SpellName)
        {
            return new Decorator(ret => FocusTarget != null && !FocusTarget.Dead && SpellManager.CanCast(SpellName, FocusTarget),
                new Action(delegate {
                SpellManager.Cast(SpellName, FocusTarget);
                Logging.Write(SpellName); })
            );
        }

        /// <summary>
        ///     Checks if specified spell is castable, if so casts it and writes to log.
        ///     Casts on focus, uses specified conditions
        /// </summary>
        /// <param name="SpellName">Spell name</param>
        /// <param name="Conditions">Specified conditions</param>
        /// <returns></returns>

        public Composite CastSpellOnFocus(string SpellName, CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && FocusTarget != null && SpellManager.CanCast(SpellName, FocusTarget),
                new Action(delegate {
                SpellManager.Cast(SpellName, FocusTarget);
                Logging.Write(SpellName); })
            );
        }

        #endregion

        #region Cast Cooldowns

        /// <summary>
        ///     Checks if specified cooldown is castable, if so casts it and writes to log.
        ///     Casts with Lua to make use of the ability queue.
        /// </summary>
        /// <param name="SpellName">Cooldown name</param>
        /// <returns></returns>

        public Composite CastCooldown(string CooldownName)
        {
            return new Decorator(ret => CanCastSpell(CooldownName),
                new Action(delegate {
                Lua.DoString(String.Format("CastSpellByName(\"{0}\");", CooldownName));
                Logging.Write(CooldownName); })
            );
        }

        /// <summary>
        ///     Checks if specified cooldown is castable, if so casts it and writes to log.
        ///     Uses specified conditions.
        ///     Casts with Lua to make use of the ability queue.
        /// </summary>
        /// <param name="SpellName">Cooldown name</param>
        /// <param name="Conditions">Specified conditions</param>
        /// <returns></returns>

        public Composite CastCooldown(string CooldownName, CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && CanCastSpell(CooldownName),
                new Action(delegate {
                Lua.DoString(String.Format("CastSpellByName(\"{0}\");", CooldownName));
                Logging.Write(CooldownName); })
            );
        }

        #endregion

        /// <summary>
        ///     Checks if player is auto-attacking, and if not toggles auto-attack.
        /// </summary>
        /// <returns></returns>

        public Composite AutoAttack()
        {
            return new Decorator(ret => !Me.IsAutoAttacking,
                new Action(delegate {
                    Me.ToggleAttack();
                    Logging.Write("Auto-attack"); })
            );
        }

        /// <summary>
        ///     Checks if player is auto-attacking, and if not toggles auto-attack.
        ///     Uses specified conditions.
        /// </summary>
        /// <returns></returns>

        public Composite AutoAttack(CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && !Me.IsAutoAttacking,
                new Action(delegate {
                Me.ToggleAttack();
                Logging.Write("Auto-attack"); })
            );
        }

        #endregion

        #region LazyRogue-Helpers
        private List<WoWUnit> GetHostileInRange(int range, bool CCSensitive, bool IgnoreBanish)
        {
            List<WoWUnit> enemyMobList = new List<WoWUnit>();
            List<WoWUnit> mobList = ObjectManager.GetObjectsOfType<WoWUnit>(false);
            foreach (WoWUnit thing in mobList)
            {
                if (thing.Distance > range + 5 ||
                    thing.IsTotem ||
                   !thing.IsAlive ||
                   !thing.IsHostile ||
                    (thing.HasAura("Banish") && IgnoreBanish) ||
                   !thing.GotTarget
                   )
                {
                    continue;
                }

                if (CCSensitive && IsCrowdControlled(thing))
                {
                    enemyMobList.Clear();
                    return enemyMobList;
                }
                else if (thing.Distance < range + 1)
                    enemyMobList.Add(thing);
            }
            return enemyMobList;
        }
        #endregion

        #region MyHelpers
        public Composite JustCastSpell(string SpellName, CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret),
                new Action(delegate
            {
                Lua.DoString(String.Format("CastSpellByName(\"{0}\");", SpellName));
                Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + SpellName);
            })
            );
        }

        public Boolean TargetHasBleed()
        {
            return (TargetDebuffTimeLeftAll("Mangle") > 0 || TargetDebuffTimeLeftAll("Hemorrhage") > 0 || TargetDebuffTimeLeftAll("Tendon Rip") > 0 || TargetDebuffTimeLeftAll("Stampede") > 0 || TargetDebuffTimeLeftAll("Gore") > 0 || TargetDebuffTimeLeftAll("Trauma") > 0);
        }

        private int BuffTimeLeft(string BuffName)
        {
            if (Me.HasAura(BuffName))
                return Me.GetAuraByName(BuffName).TimeLeft.Seconds;
            else return 0;
        }

        //public Boolean LetsInterrupt()
        //{
        //    if (Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast)
        //    {
        //        if (// Raid Spell Interrupts
        //               Me.CurrentTarget.CastingSpellId == 79710
        //            || Me.CurrentTarget.CastingSpellId == 77896
        //            || Me.CurrentTarget.CastingSpellId == 92703
        //            || Me.CurrentTarget.CastingSpellId == 80735
        //            || Me.CurrentTarget.CastingSpellId == 97202
        //            || Me.CurrentTarget.CastingSpellId == 100094
        //            || Me.CurrentTarget.CastingSpellId == 99919
        //            || Me.CurrentTarget.CastingSpellId == 38627
        //            || Me.CurrentTarget.CastingSpellId == 92509
        //            || Me.CurrentTarget.CastingSpellId == 82643
        //            || Me.CurrentTarget.CastingSpellId == 91303
        //            || Me.CurrentTarget.CastingSpellId == 93133
        //            // 5man Spell Interrupts
        //            || Me.CurrentTarget.CastingSpellId == 76008
        //            || Me.CurrentTarget.CastingSpellId == 91412
        //            || Me.CurrentTarget.CastingSpellId == 75823
        //            || Me.CurrentTarget.CastingSpellId == 66134
        //            || Me.CurrentTarget.CastingSpellId == 75763
        //            || Me.CurrentTarget.CastingSpellId == 75322
        //            || Me.CurrentTarget.CastingSpellId == 89863
        //            || Me.CurrentTarget.CastingSpellId == 76043
        //            || Me.CurrentTarget.CastingSpellId == 76903
        //            || Me.CurrentTarget.CastingSpellId == 80352
        //            || Me.CurrentTarget.CastingSpellId == 87653
        //            || Me.CurrentTarget.CastingSpellId == 79351
        //            || Me.CurrentTarget.CastingSpellId == 2061
        //            || Me.CurrentTarget.CastingSpellId == 635
        //            || Me.CurrentTarget.CastingSpellId == 2061
        //            || Me.CurrentTarget.CastingSpellId == 331
        //            || Me.CurrentTarget.CastingSpellId == 96466
        //            || Me.CurrentTarget.CastingSpellId == 5176
        //            || Me.CurrentTarget.CastingSpellId == 96957
        //            || Me.CurrentTarget.CastingSpellId == 96346
        //            || Me.CurrentTarget.CastingSpellId == 96469
        //            || Me.CurrentTarget.CastingSpellId == 97094
        //            //|| Me.CurrentTarget.CastingSpellId == 0
        //            )
        //            return true;
        //        else
        //            return false;
        //    }
        //    else
        //        return false;
        //}

        public Composite CastAdrenalineRush(CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && CanCastSpell("Adrenaline Rush"),
                new Action(delegate
                {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", "Adrenaline Rush"));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + "Adrenaline Rush");
                    
                })
            );
        }

        public Composite CastRedirect(CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && CanCastSpell("Redirect"),
                new Action(delegate
                {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", "Redirect"));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + "Redirect");
                    lasttarget = Me.CurrentTarget;
                    
                })
            );
        }

        public Composite targetcheck()
        {
            return new Decorator(ret => lasttarget == null,
                new Action(delegate {
                    lasttarget = Me.CurrentTarget;})
            );
        }

        public void TotT()
        {
            //if (TotTTarget == "")
            //{
            Lua.DoString(String.Format("RunMacroText(\"/cast [@focus,help,nodead] Tricks of the Trade\")"));
            //}
            //else
            //{
            //    Lua.DoString(String.Format("RunMacroText(\"/cast [@\"{0}\",help] Tricks of the Trade\")", TotTTarget));
            //    if (SpellManager.CanCast("Tricks of the Trade") && !TotTTargetGUID.Dead && TotTTargetGUID.IsValid)
            //        SpellManager.Cast("Tricks of the Trade", TotTTargetGUID);
            //}
        }

        public static void GetTarget()
        {
            if (Me.GotTarget)
            {
                TotTTarget = Me.CurrentTarget.Name;
                TotTTargetGUID = Me.CurrentTarget;
                
            }
        }

        public static void EraseTarget()
        {
            TotTTarget = "";
            TotTTargetGUID = null;
        }

        public void toggleoff()
        {
            if (SpellCooldown("Feint") > 1 && Lua.GetReturnVal<int>("return FeintNow", 0) == 1)
            {
                Lua.DoString("FeintNow = 0;");
            }
            if (SpellManager.HasSpell(1329))
            {

                if (SpellCooldown("Killing Spree") > 1 && Lua.GetReturnVal<int>("return KSNow", 0) == 1)
                {
                    Lua.DoString("KSNow = 0;");
                }
                if (SpellCooldown("Adrenaline Rush") > 1 && Lua.GetReturnVal<int>("return AdrenalineNow", 0) == 1)
                {
                    Lua.DoString("AdrenalineNow = 0;");
                }
            }
        }

        public Composite CastFeint(CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && CanCastSpell("Feint"),
                new Action(delegate
                {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", "Feint"));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + "Feint");
                    
                    
                })
            );
        }

        public void trinketspam()
        {
            Lua.DoString(String.Format("RunMacroText(\"/use 13\")"));
            Lua.DoString(String.Format("RunMacroText(\"/use 14\")"));
            Lua.DoString(String.Format("RunMacroText(\"/use 10\")"));
            Lua.DoString(String.Format("RunMacroText(\"/use Lifeblood\")"));
        }

        
        public Composite CastVanish(CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && CanCastSpell("Vanish"),
                new Action(delegate
                {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", "Vanish"));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + "Vanish");
                    GarroteNow = true;
                    new WaitContinue(1, ret => false, new ActionAlwaysSucceed());
                })
            );
        }
        public Composite CastGarrote(CanRunDecoratorDelegate Conditions)
        {
            return new Decorator(ret => Conditions(ret) && GarroteNow,
                new Action(delegate
                {
                    Lua.DoString(String.Format("CastSpellByName(\"{0}\");", "Garrote"));
                    Logging.Write("[" + CurrentEnergy + "] [" + Me.ComboPoints + "] " + "Garrote");
                    if(TargetDebuffTimeLeft("Garrote") > 0 || !Me.IsStealthed)
                        GarroteNow = false;
                })
            );
        }
        #endregion 
        #endregion
    }
}
