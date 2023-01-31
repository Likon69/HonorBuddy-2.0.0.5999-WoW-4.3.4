using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using System;
using System.Drawing;

namespace Hera
{
    public static class Utils
    {
        public enum CastingBreak
        {
            None = 0,
            HealthIsAbove,
            HealthIsBelow,
            PowerIsAbove,
            PowerIsBelow
        }

        private static string _logSpam;
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return Me.CurrentTarget; } }

        public static void Log(string msg, Color colour) { if (msg == _logSpam) return; Logging.Write(colour, msg); _logSpam = msg; }
        public static void Log(string msg) { if (msg == _logSpam) return; Logging.Write(msg); _logSpam = msg; }
        public static Color Colour(string nameOfColour) { return Color.FromName(nameOfColour); }

        public static string LuaGetReturnValueString(string lua) { return Lua.GetReturnValues(lua, "stuff.lua")[0]; }

        /// <summary>
        /// Sleep for the duration of your lag
        /// </summary>
        public static void LagSleep() { StyxWoW.SleepForLagDuration(); }

        public static bool CombatCheckOk() { return CombatCheckOk(null); }
        public static bool CombatCheckOk(string spellName) { return CombatCheckOk(spellName, false); }

        /// <summary>
        /// Performs common combat checking: is on GCD, Me.Casting, Me.Dead, Me.IsGhost
        /// </summary>
        /// <param name="spellName"></param>
        /// <param name="skipTargetCheck"></param>
        /// <returns>TRUE if its safe to continue, FALSE is something failed</returns>
        public static bool CombatCheckOk(string spellName, bool skipTargetCheck)
        {
            if (Spell.IsGCD || Me.IsGhost || Me.IsCasting || Me.Dead) return false;

            // You want to skip the target check if you are healing or casting a spell that does not require a target
            if (!skipTargetCheck && (!Me.GotTarget || CT.Dead)) return false;

            // if we're not checking a spell then all is good
            if (string.IsNullOrEmpty(spellName)) return true;

            // check if we know the spell and its not on cooldown
            if (!Spell.IsKnown(spellName) || Spell.IsOnCooldown(spellName)) return false;

            // all is good lets continue
            return true;
        }

        // Credit to CNG for this snipit of code
        public static bool IsNotWanding
        {
            get
            {
                if (Lua.GetReturnVal<int>("return IsAutoRepeatSpell(\"Shoot\")", 0) == 1) { return false; }
                if (Lua.GetReturnVal<int>("return HasWandEquipped()", 0) == 0) { return false; }
                return true;
            }
        }


        public static bool AddsInstance
        {
            get
            {
                List<WoWUnit> hlist =
                    (from o in ObjectManager.ObjectList
                     where o is WoWUnit
                     let p = o.ToUnit()
                     where p.Distance2D < 60
                           && !p.Dead
                           && p.Combat
                           && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                           && p.IsHostile
                           && p.Attackable
                     select p).ToList();

                return hlist.Count > 1;
            }
        }

        public static int CountOfMobsAttackingPlayer(ulong playerGUID)
        {
            List<WoWUnit> hlist =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit
                 let p = o.ToUnit()
                 where p.Distance2D < 60
                       && !p.Dead
                       && p.Combat
                       && p.CurrentTargetGuid == playerGUID
                       && p.IsHostile
                       && p.Attackable
                 select p).ToList();

            return hlist.Count;
        }

        public static bool CanAoEInstance
        {
            get
            {
                if (RaFHelper.Leader == null) return false;

                List<WoWUnit> hlist =
                    (from o in ObjectManager.ObjectList
                     where o is WoWUnit
                     let p = o.ToUnit()
                     where p.Distance2D < 60
                           && !p.Dead
                           && p.Combat
                           && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                           && p.IsHostile
                           && p.Attackable
                     select p).ToList();

                int countNearTank = hlist.Where(u => u.HealthPercent >= 25).Count(u => RaFHelper.Leader.Location.Distance(u.Location) < 15);

                // If you have 3+ adds then AoE
                return countNearTank > 2;
            }
        }

        public static int CountOfAddsInRange(double distance, WoWPoint location)
        {
            List<WoWUnit> hlist =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit
                 let p = o.ToUnit()
                 where p.Distance2D < 50
                       && !p.Dead
                       && p.Combat
                       && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                       && p.IsHostile
                       && p.Attackable
                 select p).ToList();

            return hlist.Count(u => location.Distance(u.Location) <= distance);
        }

        public static int AddsCount
        {
            get
            {
                List<WoWUnit> hlist =
               (from o in ObjectManager.ObjectList
                where o is WoWUnit
                let p = o.ToUnit()
                where p.Distance2D < 45
                      && !p.Dead
                    //&& p.IsTargetingMeOrPet
                      && (p.Aggro || p.PetAggro)
                      && p.Attackable
                      && p.CreatureType != WoWCreatureType.Critter
                select p).ToList();


                return hlist.Count;
                //return Targeting.Instance.TargetList.Count;
            }
        }

       


        /// <summary>
        /// TRUE if you have adds
        /// </summary>
        public static bool Adds
        {
            get
            {
                // I'm No longer using HB's TargetList count to do the add check as this is not producing the desired result
                // Instead using my own add check. Basically get all alive mobs attacking me or my pet
                if (!Me.IsInParty)
                {
                    List<WoWUnit> hlist =
                        (from o in ObjectManager.ObjectList
                         where o is WoWUnit
                         let p = o.ToUnit()
                         where p.Distance2D < 50
                               && !p.Dead
                               && p.IsTargetingMeOrPet // || Me.IsInInstance && p.IsTargetingMyPartyMember)
                               && p.Attackable
                         select p).ToList();


                    return hlist.Count > 1;
                }


                List<WoWUnit> hplist =
                       (from o in ObjectManager.ObjectList
                        where o is WoWUnit
                        let p = o.ToUnit()
                        where p.Distance2D < 50
                              && !p.Dead
                              && p.Combat
                              && (p.IsTargetingMyPartyMember || p.IsTargetingMeOrPet)
                              && p.Attackable
                        select p).ToList();


                return hplist.Count > 1;
            }
        }


        public static void AutoAttack(bool autoAttackOn)
        {
            if (autoAttackOn) { if (Me.IsAutoAttacking) return; Lua.DoString("StartAttack()"); return; }
            Lua.DoString("StopAttack()");
        }

        // Do a simple loop while casting a spell. 
        // Required so you don't double heal 
        public static void WaitWhileCasting()
        {
            Thread.Sleep(150);
            while (Me.IsCasting) { Thread.Sleep(100); }
        }

        public static void WaitWhileCasting(CastingBreak statCheck, double breakValue, WoWUnit targetCheck)
        {
            Thread.Sleep(150);
            while (Me.IsCasting)
            {
                Thread.Sleep(100);
                switch (statCheck)
                {
                    case CastingBreak.None:
                        Thread.Sleep(1);
                        break;

                    case CastingBreak.HealthIsAbove:
                        if (targetCheck.HealthPercent > breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;

                    case CastingBreak.HealthIsBelow:
                        if (targetCheck.HealthPercent < breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;

                    case CastingBreak.PowerIsAbove:
                        if (targetCheck.PowerPercent > breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;

                    case CastingBreak.PowerIsBelow:
                        if (targetCheck.PowerPercent < breakValue)
                        {
                            Spell.StopCasting();
                            return;
                        }
                        break;
                }
            }
        }

        // Are you in a battleground
        public static bool IsBattleground { get { return Battlegrounds.IsInsideBattleground; } }

        /// <summary>
        /// Return a WoWUnit type of a player in your party/raid in need of healing. Null if noone is in need of healing
        /// </summary>
        /// <param name="minimumHealth">The health a player must be to be considered for healing</param>
        /// <returns>WoWUnit the player most in need of healing</returns>
        public static WoWUnit PlayerNeedsHealing(double minimumHealth)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty) return null;

            // MyGroup is populated with raid members or party members, whichever you are in
            List<WoWPlayer> myGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;

            // Enumerate all players in myGroup and find the person with the lowest health %
            List<WoWPlayer> playersToHeal = (from o in myGroup
                                             let p = o.ToPlayer()
                                             where p.Distance < 40
                                                   && !p.Dead
                                                   && !p.IsGhost
                                                   && p.InLineOfSight
                                                   && p.HealthPercent < minimumHealth
                                             orderby p.HealthPercent ascending
                                             select p).ToList();

            // If playersToHeal is more than 0 then we have someone to heal
            // So return the first person in the list, they will be the most in need
            return playersToHeal.Count > 0 ? playersToHeal[0] : null;
        }

        /// <summary>
        /// The best target to attack, the one with the lowest health % and the closest not fleeing/running away
        /// </summary>
        public static WoWUnit BestTarget
        {
            get
            {
                List<WoWUnit> attackableMobs = (from o in ObjectManager.ObjectList
                                            where o is WoWUnit && o.Distance < 40
                                            let u = o.ToUnit()
                                            where u.Attackable 
                                            && u.IsAlive 
                                            && u.IsTargetingMeOrPet
                                            && !u.Fleeing
                                            orderby u.HealthPercent ascending
                                            select u).ToList();

                return attackableMobs.Count > 0 ? attackableMobs[0] : null;
            }
        }
       
        public static bool HostileMobsInRange(double searchRange)
        {
            List<WoWUnit> hlist =
               (from o in ObjectManager.ObjectList
                where o is WoWUnit
                let p = o.ToUnit()
                where p.Distance2D < searchRange
                      && !p.Dead
                      && !p.TaggedByOther
                      && p.IsHostile
                select p).ToList();


            return hlist.Count > 0;
        }

        public static bool AllMobsAttackingPetOrOther
        {
            get { return !Targeting.Instance.TargetList.Any(u => u.CurrentTargetGuid == Me.Guid); }
        }

        public static bool AttackableMobsInRange(double searchRange)
        {
            List<WoWUnit> hlist =
               (from o in ObjectManager.ObjectList
                where o is WoWUnit
                let p = o.ToUnit()
                where p.Distance2D < searchRange
                      && !p.Dead
                      && !p.TaggedByOther
                      && p.Attackable
                select p).ToList();


            return hlist.Count > 0;
        }

        public static WoWUnit AttackableMobInRange(double searchRange)
        {
            List<WoWUnit> hlist =
                (from o in ObjectManager.ObjectList
                 where o is WoWUnit
                 let p = o.ToUnit()
                 where p.Distance2D < searchRange
                       && !p.Dead
                       && !p.TaggedByOther
                       && p.Attackable
                       && !p.IsPlayer
                       && !p.IsPet
                       && p.CreatureType != WoWCreatureType.Critter // Appears to be bugged as it selects critters
                       && p.Level > 1                               // Added a level check. All critters should be level 1. 
                       && !p.IsFlying
                 orderby p.Distance2D ascending
                 select p).ToList();


            return hlist.Where(p => Navigator.CanNavigateFully(Me.Location, p.Location, 60)).FirstOrDefault();
        }

        public static bool IsInLineOfSight(WoWPoint location)
        {
            bool result = GameWorld.IsInLineOfSight(Me.Location, location);

            return result;
        }

        public static bool IsInLineOfSight()
        {
            if (Me.IsInParty)
            {
                if (RaFHelper.Leader != null && !IsInLineOfSight(RaFHelper.Leader.Location)) return false;
            }
            else
            {
                if (Me.GotTarget && !IsInLineOfSight(CT.Location)) return false;
            }

            return true;

        }

        public static void MoveToLineOfSight()
        {
            WoWPoint location = Me.Location;
            if (Me.IsInParty && RaFHelper.Leader != null) location = RaFHelper.Leader.Location;
            if (!Me.IsInParty && Me.GotTarget) location = CT.Location;

            Movement.MoveTo(location);
            while (!GameWorld.IsInLineOfSight(Me.Location, location))
            {
                Movement.MoveTo(location);
                Thread.Sleep(250);
            }

            if (Me.IsMoving) Movement.StopMoving();
        }

        public static void MoveToLineOfSight(WoWPoint location)
        {
            //Utils.Log(string.Format("We don't have LOS on {0} moving closer...", CT.Name),System.Drawing.Color.FromName("DarkRed"));
            Movement.MoveTo(location);
            while (!GameWorld.IsInLineOfSight(Me.Location, location))
            {
                Movement.MoveTo(location);
                Thread.Sleep(250);
            }

            if (Me.IsMoving) Movement.StopMoving();
        }


    }

    public static class Timers
    {
        private static Dictionary<string, Stopwatch> _timerCollection = new Dictionary<string, Stopwatch>();

        public static void Add(string timerName)
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();

            _timerCollection.Add(timerName, stw);
        }

        public static void Remove(string timerName)
        {
            _timerCollection.Remove(timerName);
        }

        public static Stopwatch Timer(string timerName)
        {
            if (!TimerExists(timerName)) return null;
            return _timerCollection[timerName];
        }

        public static bool Expired(string timerName, long maximumMilliseconds)
        {
            if (!TimerExists(timerName)) return false;
            return _timerCollection[timerName].ElapsedMilliseconds > maximumMilliseconds;
        }

        public static bool IsRunning(string timerName)
        {
            if (!TimerExists(timerName)) return false;
            return _timerCollection[timerName].IsRunning;
        }

        public static void Start(string timerName)
        {
            if (!TimerExists(timerName)) return;
            _timerCollection[timerName].Start();
        }

        public static void Stop(string timerName)
        {
            if (!TimerExists(timerName)) return;
            _timerCollection[timerName].Stop();
        }

        public static long ElapsedMilliseconds(string timerName)
        {
            if (!TimerExists(timerName)) return 0;
            return _timerCollection[timerName].ElapsedMilliseconds;
        }

        public static long ElapsedSeconds(string timerName)
        {
            if (!TimerExists(timerName)) return 0;
            return _timerCollection[timerName].ElapsedMilliseconds / 1000;
        }

        public static void Reset(string timerName)
        {
            if (!TimerExists(timerName)) return;
            _timerCollection[timerName].Reset();
            _timerCollection[timerName].Start();
        }

        public static void Recycle(string timerName, long elapsedMilliseconds)
        {
            if (_timerCollection[timerName].ElapsedMilliseconds < elapsedMilliseconds) return;

            _timerCollection[timerName].Reset();
            _timerCollection[timerName].Start();
        }

        public static bool Exists(string timerName)
        {
            return _timerCollection.ContainsKey(timerName);
        }

        private static bool TimerExists(string timerName)
        {
            if (!_timerCollection.ContainsKey(timerName))
            {
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(String.Format(" Timer '{0}' does not exist", timerName));
                Logging.WriteDebug(" ");
                Logging.WriteDebug("**********************************************************************");
                return false;
            }

            return true;
        }
    }
    
    public static class Spell
    {

        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }

        public static string BestSpell(string spells)
        {
            string[] spellList = spells.Split('+');

            foreach (string spell in spellList.Where(spell => CanCast(spell.Trim()) && !IsOnCooldown(spell.Trim())))
            {
                return spell.Trim();
            }

            return "";

        }

        public static string BestDebuff(string debuffs)
        {
            string[] debuffList = debuffs.Split('+');

            foreach (string debuff in debuffList.Where(debuff => CanCast(debuff) && !Target.IsDebuffOnTarget(debuff)))
            {
                return debuff;
            }
           
            return "";
        }

        /// <summary>
        /// TRUE if a spell is known by HB
        /// </summary>
        /// <param name="spellName">Name of the spell to check</param>
        /// <returns>TRUE if the spell is know</returns>
        public static bool IsKnown(string spellName) { return SpellManager.HasSpell(spellName); }


        /// <summary>
        /// Stop casting, goes without saying really
        /// </summary>
        public static void StopCasting() { SpellManager.StopCasting(); }

        /// <summary>
        /// TRUE if on global cooldown
        /// </summary>
        public static bool IsGCD { get { return LegacySpellManager.GlobalCooldown; } }

        
        /// <summary>
        /// TRUE if HB can cast the spell. This performs a LUA check on the spell
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns></returns>
        public static bool CanCastLUA(string spellName)
        {
            var isUsable = Lua.GetReturnValues("return IsUsableSpell('" + spellName + "')", "stuffnthings.lua");

            return isUsable != null && isUsable[0] == "1";
        }

        /// <summary>
        /// TRUE if HB can cast the spell. But first check our mana levels. 
        /// This way we can leave some mana for healing spells
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <returns>TRUE if the spell can be cast</returns>
        public static bool CanCast(string spellName)
        {
            return CanCast(spellName, 0d);
        }

        
        /// <summary>
        /// TRUE if HB can cast the spell. But first check our mana levels. 
        /// This way we can leave some mana for healing spells
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <param name="minimumPower">If power is below minimumPower percent then return FALSE</param>
        /// <returns>TRUE if the spell can be cast</returns>
        public static bool CanCast(string spellName, double minimumPower)
        {
            if (string.IsNullOrEmpty(spellName)) return false;
            return Me.CurrentPower > minimumPower && SpellManager.CanCast(spellName);
        }

        /// <summary>
        /// Cast a specific spell
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>TRUE if the spell was cast successfully</returns>
        public static bool Cast(string spellName)
        {
            bool result = SpellManager.Cast(spellName);

            if (result) Utils.Log("-" + spellName, Utils.Colour("Blue"));
            return result;
        }

        /// <summary>
        /// Cast a spell on a specific target. This does not deselect your current target if it differs from targetUnit
        /// </summary>
        /// <param name="spellName">Name of the spell to cast</param>
        /// <param name="targetUnit">WoWUnit to cast the spell on</param>
        /// <returns>TRUE if the spell was cast successfully</returns>
        public static bool Cast(string spellName, WoWUnit targetUnit)
        {
            bool result = SpellManager.Cast(spellName, targetUnit);
            string targetName;

            if (RaFHelper.Leader != null && targetUnit.Guid == RaFHelper.Leader.Guid)
                targetName = "Leader / Tank";
            else
                targetName = targetUnit.Guid == Me.Guid ? "Me" : "Target";

            if (targetName == "Target" && targetUnit.IsPlayer) targetName = targetUnit.Class.ToString();
            if (result) Utils.Log(String.Format("-{0} on {1}", spellName, targetName), Utils.Colour("Blue"));
            return result;
        }

        public static bool Cast(string spellName, WoWUnit targetUnit, bool waitForCastAndGCD)
        {
            bool result = SpellManager.Cast(spellName, targetUnit);

            

            string targetName;

            if (RaFHelper.Leader != null && targetUnit.Guid == RaFHelper.Leader.Guid)
                targetName = "Leader / Tank";
            else
                targetName = targetUnit.Guid == Me.Guid ? "Me" : "Target";

            if (targetName == "Target" && targetUnit.IsPet) targetName = "Pet";
            if (targetName == "Target" && targetUnit.IsPlayer) targetName = targetUnit.Class.ToString();
            

            if (waitForCastAndGCD)
            {
                Utils.LagSleep();
                while (Me.IsCasting) Thread.Sleep(100);
                while (IsGCD) Thread.Sleep(100);
            }

            if (result) Utils.Log(String.Format("-{0} on {1}", spellName, targetName), Utils.Colour("Blue"));
            return result;
        }



        

        /// <summary>
        /// Cast a given spell using click-to-cast spells
        /// </summary>
        /// <param name="spellName">Spell name to cast</param>
        /// <param name="clickCastLocation">WoWPoint to cast the spell</param>
        /// <returns></returns>
        public static bool Cast(string spellName, WoWPoint clickCastLocation)
        {

            bool result = SpellManager.Cast(spellName);
            LegacySpellManager.ClickRemoteLocation(clickCastLocation);

            Utils.Log("-" + spellName, Utils.Colour("Blue"));
            return result;

        }

        /// <summary>
        /// TRUE if the spell is on cooldown and can not be cast
        /// </summary>
        /// <param name="spellName"></param>
        /// <returns>Name of the spell to check</returns>
        public static bool IsOnCooldown(string spellName)
        {
            if (!IsKnown(spellName)) return true;

            bool result = SpellManager.Spells[spellName].Cooldown;

            return result;
        }

        /// <summary>
        /// Conditionally cast a debuff on your current target
        ///   * Check if the debuff is on the target
        ///   * Check if HB can cast the spell
        /// </summary>
        /// <param name="spellName">Name of the debuff to cast</param>
        /// <returns>TRUE if the debuff was cast successfully</returns>
        public static bool CastDebuff(string spellName)
        {
            if (Target.IsDebuffOnTarget(spellName) || !CanCast(spellName)) return false;

            bool result = SpellManager.Cast(spellName);
            if (!result) return false;

            Utils.Log(String.Format("{0} on {1}", spellName, Me.CurrentTarget.Name), Utils.Colour("Blue"));
            return true;
        }

        /// <summary>
        /// Cast a debuff on a specific target
        /// </summary>
        /// <param name="spellName">Name of the debuff to cast</param>
        /// <param name="targetUnit">WoWUnit to cast the debuff on</param>
        /// <returns>TRUE if the debuff was cast successfully</returns>
        public static bool CastDebuff(string spellName, WoWUnit targetUnit)
        {
            if (targetUnit.HasAura(spellName) || !CanCast(spellName)) return false;

            bool result = SpellManager.Cast(spellName, targetUnit);
            if (!result) return false;

            Utils.Log(String.Format("{0} on {1}", spellName, Me.CurrentTarget.Name), Utils.Colour("Blue"));
            return true;
        }

        /// <summary>
        /// Cast a buff on a specific target
        /// </summary>
        /// <param name="spellName">Name of the buff to cast</param>
        /// <param name="targetUnit">WoWUnit to cast the buff on</param>
        /// <returns>TRUE if the buff was cast successfully</returns>
        public static bool CastBuff(string spellName, WoWUnit targetUnit)
        {
            if (targetUnit.HasAura(spellName) || !CanCast(spellName)) return false;

            bool result = SpellManager.Cast(spellName, targetUnit);
            if (!result) return false;

            Utils.Log(spellName);
            return true;
        }


        /// <summary>
        /// TRUE if you have enough mana to cast the spell
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <returns></returns>
        public static bool IsEnoughPower(string spellName)
        {
            if (!IsKnown(spellName)) return false;
            
            return (Me.CurrentPower > SpellManager.Spells[spellName].PowerCost);
        }


        public static int PowerCost(string spellName)
        {
            if (!IsKnown(spellName)) return 9999999;
            
            return SpellManager.Spells[spellName].PowerCost;
        }


        /// <summary>
        /// Returns the maximum distance of the spell
        /// </summary>
        /// <param name="spellName">Spell name to check</param>
        /// <returns>The maximum distance the spell can be cast</returns>
        public static double MaxDistance(string spellName)
        {
            if (!IsKnown(spellName)) return 0.0;

            return SpellManager.Spells[spellName].MaxRange;
        }

        public static double MinDistance(string spellName)
        {
            if (!IsKnown(spellName)) return 0.0;

            return SpellManager.Spells[spellName].MinRange;
        }


    }

    public static class Target
    {

        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return Me.CurrentTarget; } }
        private static Stopwatch pullTimer = new Stopwatch();
        private static ulong _pullGuid;
        private static Stopwatch combatTimer = new Stopwatch();
        private static ulong _combatGuid;

        public static int CombatTimeout { get; set; }
        public static string LazyRaider { get; set; }


        /// <summary>
        /// The current health percent of the target
        /// </summary>
        public static double HealthPercent { get { return CT.HealthPercent; } }

        /// <summary>
        /// TRUE if the current target is more than 4 levels lower than you
        /// </summary>
        public static bool IsLowLevel { get { if (!Me.GotTarget) return false;  if (CT.Level < 6) return false; return CT.Level <= Me.Level - 3; } }

        /// <summary>
        /// TRUE if the current target is more than 3 levels higher than you
        /// </summary>
        public static bool IsHighLevel { get { return Me.GotTarget && CT.Level >= Me.Level - 3; } }

        public static bool IsFleeing { get { return Me.GotTarget && CT.Fleeing; } }

        public static bool IsDistanceLessThan(double distanceCheck) { return Me.GotTarget && CT.Distance < distanceCheck; }

        public static bool IsDistanceMoreThan(double distanceCheck) { return Me.GotTarget && CT.Distance > distanceCheck; }

        public static bool IsTargetingMe { get { return Me.GotTarget && CT.CurrentTargetGuid == Me.Guid; } }

        public static bool IsHealthAbove(double targetHealth) { return Me.GotTarget && CT.CurrentHealth > targetHealth; }

        public static bool IsHealthPercentAbove(double targetHealthPercent) { return Me.GotTarget && CT.HealthPercent > targetHealthPercent; }

        public static double Distance { get { return !Me.GotTarget ? 0 : CT.Distance; } }

        public static bool IsCasting { get { return Me.GotTarget && CT.IsCasting; } }

        public static bool CombatTimerExpired
        {
            get
            {
                if (!Me.GotTarget || Me.CurrentTarget.Dead) return false;

                if (_combatGuid != Me.CurrentTarget.Guid)
                {
                    Utils.Log(string.Format("New combat target {0}, resetting combat timer.", Me.CurrentTarget.Name), Color.FromName("DarkBlue"));
                    _combatGuid = Me.CurrentTarget.Guid;
                    combatTimer.Reset();
                    combatTimer.Start();
                }

                return combatTimer.Elapsed.Seconds >=  CombatTimeout;
            }
        }

        /// <summary>
        /// This is not actually true, this checks if the target's health is > 10X your own health
        /// </summary>
        public static bool IsInstanceBoss
        {
            get
            {
                if (!Me.GotTarget) return false;

                uint myHp = Me.MaxHealth;
                uint ctHp = CT.MaxHealth;
                bool result = (ctHp > myHp * 11);

                return result;
            }
        }

        public static bool IsWithinInteractRange { get { return Me.GotTarget && CT.WithinInteractRange; } }

        public static float InteractRange { get { return !Me.GotTarget ? 0 : CT.InteractRange; }
        }

        /// <summary>
        /// TRUE is the debuff is on the target.
        /// Similar to CanDebuffTarget except this does not check if you can cast the spell
        /// </summary>
        /// <param name="debuffName"></param>
        /// <returns></returns>
        public static bool IsDebuffOnTarget(string debuffName) { return Me.GotTarget && CT.HasAura(debuffName) && CT.Auras[debuffName].CreatorGuid == Me.Guid; }  //.HasAura(DebuffName)); }

        /// <summary>
        /// TRUE if the target has been tagged and you are not in Party/Raid
        /// </summary>
        public static bool IsTaggedByOther { get { return (Me.GotTarget && !(Me.IsInParty || Me.IsInRaid) && CT.TaggedByOther); } }

        /// <summary>
        /// TRUE if the target is considered a caster
        /// A caster is a NPC that has MANA
        /// </summary>
        public static bool IsCaster { get { return Me.GotTarget && CT.ManaPercent > 1; } }

        public static bool IsPlayerCaster
        {
            get
            {
                if (!Me.GotTarget) return false;
                if (CT.Class == WoWClass.DeathKnight) return false;
                if (CT.Class == WoWClass.Hunter) return false;
                if (CT.Class == WoWClass.Rogue) return false;
                if (CT.Class == WoWClass.Warrior) return false;
                if (CT.Class == WoWClass.Paladin) return false;
                if (CT.Class == WoWClass.Druid && CT.Shapeshift == ShapeshiftForm.Cat) return false;
                if (CT.Class == WoWClass.Druid && CT.Shapeshift == ShapeshiftForm.Bear) return false;


                return true;
            }
        }

        /// <summary>
        /// TRUE if HB can generate a path to the target. 
        /// </summary>
        //public static bool CanGenerateNavPath { get { return Me.GotTarget && Navigator.GeneratePath(Me.Location, CT.Location).Length > 0; } }
        public static bool CanGenerateNavPath
        {
            get { return Me.GotTarget && Navigator.CanNavigateFully(Me.Location, CT.Location, 20); }
        }

        public static bool CanGenerateNavPathWithHops(int maxHops)
        {
            {
                return Me.GotTarget && Navigator.CanNavigateFully(Me.Location, CT.Location, maxHops);
            }
        }

        /// <summary>
        /// Blacklist the target for X seconds
        /// </summary>
        /// <param name="seconds">Seconds to blacklist the target</param>
        public static void BlackList(int seconds) { if (!Me.GotTarget) return; Blacklist.Add(CT, new TimeSpan(0, 0, seconds)); Me.ClearTarget(); }

        public static bool IsPlayer { get { return Me.GotTarget && CT.IsPlayer; } }

        public static void Face()
        {
            if (LazyRaider.Contains("always")) return;
            if (!Me.GotTarget) return; CT.Face();
        }

        public static bool IsFacing { get { return Me.GotTarget && WoWMathHelper.IsFacing(Me.Location, Me.Rotation, Me.CurrentTarget.Location, WoWMathHelper.DegreesToRadians(120)); } }

        public static bool IsElite { get { return Me.GotTarget && CT.Elite; } }

        /// <summary>
        /// TRUE if you can cast the debuff on the target
        ///   * Do you have a target
        ///   * Can you cast the spell
        ///   * Is the debuff already on the target
        /// </summary>
        /// <param name="spellName">Debuff spell to cast on the target</param>
        /// <returns></returns>
        public static bool CanDebuffTarget(string spellName)
        {
            return Me.GotTarget && Spell.CanCast(spellName) && (!CT.HasAura(spellName) || CT.HasAura(spellName) && CT.Auras[spellName].CreatorGuid != Me.Guid) && CT.Distance <= Spell.MaxDistance(spellName);
        }

        /// <summary>
        /// Return the stack count of a given debuff on the current target
        /// </summary>
        /// <param name="debuffName">Debuff name to check</param>
        /// <returns>int of debuff stacks</returns>
        public static int DebuffStackCount(string debuffName)
        {
            if (!Me.GotTarget || !IsDebuffOnTarget(debuffName)) return 0;
            
            return (int)CT.Auras[debuffName].StackCount;
        }

        public static int StackCountLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""target"",""{0}"")", buffName));
            int stackCount = Lua.GetLocalizedInt32("stackCount", CT.BaseAddress);

            return stackCount;
            //return Convert.ToInt16(stackCount);
        }


        /// <summary>
        /// A number of simple checks to determine if the currently selected target should be pulled
        /// Checks the following; GotTarget, IsTotem, IsPet & Tagged
        /// </summary>
        public static bool IsValidPullTarget
        {
            get
            {
                if (Blacklist.Contains(CT.Guid)) return false;
                if (Blacklist.Contains(CT)) return false;
                if (!Me.GotTarget || (CT.IsSwimming && !Me.IsSwimming) || CT.Distance2D > 90 || !CT.InLineOfSight || CT.IsTotem || CT.IsPet || (RaFHelper.Leader == null && CT.TaggedByOther))
                    return false;

                return true;
            }
        }


        ///// <summary>
        ///// Check how long we've been in comat, is it more than 30 seconds?
        ///// </summary>
        //public static bool CombatTimerExpired
        //{
        //    get
        //    {
        //        if (!Me.GotTarget) return false;
        //        if (Me.CurrentTarget.Dead) return false;

        //        if (_combatGuid != Me.CurrentTarget.Guid)
        //        {
        //            Utils.Log(string.Format("New combat target {0}, resetting combat timer.", Me.CurrentTarget.Name), Color.FromName("DarkBlue"));
        //            _combatGuid = Me.CurrentTarget.Guid;
        //            combatTimer.Reset();
        //            combatTimer.Start();
        //        }

        //        return combatTimer.Elapsed.Seconds >= Settings.CombatTimeout;
        //    }
        //}


        /// <summary>
        /// Have you been trying to pull the target for more than 20 seconds?
        /// </summary>
        public static bool PullTimerExpired
        {
            get
            {
                if ((Self.IsBuffOnMe("Drink") || Self.IsBuffOnMe("Eat")) && _pullGuid != 0) _pullGuid = 0;

                if (_pullGuid != Me.CurrentTarget.Guid)
                {
                    Utils.Log(string.Format("New pull target {0} ({1}), resetting pull timer.", Me.CurrentTarget.Name, Me.CurrentTarget.Level), Color.FromName("DarkBlue"));
                    _pullGuid = Me.CurrentTarget.Guid;
                    pullTimer.Reset();
                    pullTimer.Start();
                }

                return pullTimer.Elapsed.Seconds >= 20;
            }
        }

        public static bool IsBuffOnTarget(int buffID)
        {
            return Me.CurrentTarget.Auras.Any(aura => aura.Value.SpellId == buffID);
        }

        public static int StackCount(int buffID)
        {
            if (!IsBuffOnTarget(buffID)) return 0;
            return (int)Me.GetAuraById(buffID).StackCount;
        }
    }
    
    public static class Self
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public enum AuraCheck { ActiveAuras, AllAuras }

        /// <summary>
        /// Simple buff check
        /// </summary>
        /// <param name="buffName">Name of the buff to check</param>
        /// <returns>TRUE if the buff is present on you</returns>
        public static bool IsBuffOnMe(string buffName)
        {
            return (Me.Auras.ContainsKey(buffName));
        }

        /// <summary>
        /// Returns a WoW Aura matching the buff ID. Searches Me.Auras (all auras)
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public static WoWAura Buff(int buffID)
        {
            foreach (KeyValuePair<string, WoWAura> aura in Me.Auras)
            {
                if (aura.Value.SpellId == buffID) return aura.Value;
            }

            return null;
        }

        public static bool IsBuffOnMe(int buffID)
        {
            return Me.ActiveAuras.Any(aura => aura.Value.SpellId == buffID);
        }

        public static bool IsBuffOnMe(int buffID, AuraCheck auras)
        {
            if (auras == AuraCheck.ActiveAuras)
            {
                return Me.ActiveAuras.Any(aura => aura.Value.SpellId == buffID);
            }

            return Me.Auras.Any(aura => aura.Value.SpellId == buffID);
        }

        public static bool IsBuffOnMeLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""player"",""{0}"")", buffName));
            string buff = Lua.GetLocalizedText("buffName", Me.BaseAddress);

            return buff == buffName;
        }

        public static int StackCountLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""player"",""{0}"")", buffName));
            int stackCount = Lua.GetLocalizedInt32("stackCount", Me.BaseAddress);

            return stackCount;
            //return Convert.ToInt16(stackCount);
        }

        public static string BuffTimeLeftLUA(string buffName)
        {
            Lua.DoString(string.Format(@"buffName,_,_,stackCount,_,_,timeLeft,_,_=UnitBuff(""player"",""{0}"")", buffName));
            string timeLeft = Lua.GetLocalizedText("timeLeft", Me.BaseAddress);

            return timeLeft;
            //return Convert.ToInt16(timeLeft);
        }

        public static string GetTimeLUA()
        {
            Lua.DoString(string.Format(@"time = GetTime()"));
            string time = Lua.GetLocalizedText("time", Me.BaseAddress);

            return time;
            //return Convert.ToInt16(timeLeft);
        }


        /// <summary>
        /// Remove a buff from the player
        /// </summary>
        /// <param name="buffName">Name of the buff to be removed</param>
        public static void RemoveBuff(string buffName)
        {
            Lua.DoString(string.Format("CancelUnitBuff('player', '{0}')", buffName));
        }

        /// <summary>
        /// Checks Spell.CanCast and Me.HasAura
        /// </summary>
        /// <param name="buffName">Name of the buff you want to cast</param>
        /// <returns></returns>
        public static bool CanBuffMe(string buffName)
        {
            return !Me.HasAura(buffName) && Spell.CanCast(buffName);
        }

        /// <summary>
        /// TRUE is the given buff is present on you
        /// </summary>
        /// <param name="buffName">Buff name you want to check for</param>
        /// <returns>TRUE if the buff is present</returns>
        public static bool BuffMe(string buffName)
        {
            Spell.Cast(buffName, Me);
            Utils.LagSleep();

            return IsBuffOnMe(buffName);
        }

        /// <summary>
        /// The number of stacks on a given buff
        /// </summary>
        /// <param name="buffName">Buff name you want to check for</param>
        /// <returns>The number (int) of stacks of the buff</returns>
        public static int StackCount(string buffName)
        {
            if (!IsBuffOnMe(buffName)) return 0;
            return (int)Me.Auras[buffName].StackCount;
        }



        /// <summary>
        /// The number of stacks of a given buff (by ID)
        /// </summary>
        /// <param name="buffID">Spell ID of the buff you want to check for</param>
        /// <returns>The number of stacks of the buff</returns>
        public static int StackCount(int buffID)
        {
            if (!IsBuffOnMe(buffID)) return 0;
            return (int) Me.GetAuraById(buffID).StackCount;
        }

        /// <summary>
        /// Scan the area (30 yards) for players of the same faction to buff
        /// </summary>
        /// <param name="spellName">Name of the buff you want to cast</param>
        /// <param name="excludeIfBuffPresent">Do not cast the buff if this spell is present on the target</param>
        /// <param name="minimumMana">Do not cast the buff if your mana is below this percent</param>
        /// <param name="buffInCombat">TRUE if you want to cast buffs on players while you are in combat</param>
        public static void BuffRandomPlayers(string spellName, string excludeIfBuffPresent, double minimumMana, bool buffInCombat)
        {
            if (Me.IsResting || IsBuffOnMe("Drink") || IsBuffOnMe("Food") || !Spell.CanCast(spellName) || Me.IsGhost || Me.Dead || Me.Mounted || (!buffInCombat && Me.Combat) || Me.ManaPercent < minimumMana)
                return;

            List<WoWPlayer> plist =
                (from o in ObjectManager.ObjectList
                 where o is WoWPlayer
                 let p = o.ToPlayer()
                 where p.Distance < 30
                       && p.Guid != Me.Guid
                       && (p.IsHorde && Me.IsHorde || p.IsAlliance && Me.IsAlliance)
                       && !p.Dead
                       && p.InLineOfSight
                       && !p.HasAura(spellName)
                       && !p.HasAura(excludeIfBuffPresent)
                 select p).ToList();


            foreach (WoWPlayer p in plist)
            {
                if (!Spell.CanCast(spellName) || !buffInCombat && p.Combat || !Me.PvpFlagged && p.PvpFlagged) return;

                Utils.Log(string.Format("Being friendly and casting {0} on a player", spellName), Utils.Colour("Green"));
                Spell.Cast(spellName, p);
                while (Spell.IsGCD) Thread.Sleep(250); 
            }

        }

        /// <summary>
        /// Scan the area (40 yards) for players of the same faction to heal
        /// </summary>
        /// <param name="spellName">Name of the sepll you want to cast</param>
        /// <param name="excludeIfBuffPresent">Do not cast the buff if this spell is present on the target</param>
        /// <param name="buffInCombat">Do not cast the buff if your mana is below this percent</param>
        /// <param name="minimumHealth">The minimum health a player must be before healing them</param>
        /// <param name="minimumMana">TRUE if you want to cast buffs on players while you are in combat</param>
        public static void HealRandomPlayers(string spellName, string excludeIfBuffPresent, bool buffInCombat, double minimumHealth, double minimumMana)
        {
            if (Me.IsResting || IsBuffOnMe("Drink") || IsBuffOnMe("Food") || !Spell.CanCast(spellName) || Me.IsGhost || Me.Dead || Me.Mounted || Me.ManaPercent < minimumMana)
                return;

            List<WoWPlayer> plist =
                (from o in ObjectManager.ObjectList
                 where o is WoWPlayer
                 let p = o.ToPlayer()
                 where p.Distance < 40
                       && p.Guid != Me.Guid
                       && (p.IsHorde && Me.IsHorde || p.IsAlliance && Me.IsAlliance)
                       && !p.Dead
                       && p.InLineOfSight
                       && p.HealthPercent > 10
                       && p.HealthPercent < minimumHealth
                       && !p.HasAura(spellName)
                       && !p.HasAura(excludeIfBuffPresent)
                 select p).ToList();



            foreach (WoWPlayer p in plist)
            {
                if (!Spell.CanCast(spellName) || (!buffInCombat && p.Combat) || p.HasAura(excludeIfBuffPresent) || p.HasAura(spellName) || (!Me.PvpFlagged && p.PvpFlagged)) return;

                while (Me.IsMoving) WoWMovement.MoveStop();

                Utils.Log("Being friendly and healing a player", Utils.Colour("Green"));
                Spell.Cast(spellName, p);
                Thread.Sleep(500);

                while (Me.IsCasting) Thread.Sleep(250);
                while (Spell.IsGCD) Thread.Sleep(250);
            }

        }

        /// <summary>
        /// Check if your health is above X value
        /// </summary>
        /// <param name="healthLevel">A value to check if your health is above</param>
        /// <returns></returns>
        public static bool IsHealthAbove(int healthLevel) { return Me.CurrentHealth > healthLevel; }

        /// <summary>
        /// Check if your health percent is above X%
        /// </summary>
        /// <param name="healthPercentLevel"></param>
        /// <returns></returns>
        public static bool IsHealthPercentAbove(int healthPercentLevel) { return Me.HealthPercent > healthPercentLevel; }

        /// <summary>
        /// Check if your power (Mana/Energy/Rage) is above X value
        /// </summary>
        /// <param name="powerLevel"></param>
        /// <returns></returns>
        public static bool IsPowerAbove(int powerLevel) { return Me.CurrentPower > powerLevel; }

        /// <summary>
        /// Check if your power (Mana/Energy/Rage) percent is above X%
        /// </summary>
        /// <param name="powerPercentLevel"></param>
        /// <returns></returns>
        public static bool IsPowerPercentAbove(int powerPercentLevel) { return Me.PowerPercent > powerPercentLevel; }
    }

    public static class Talents
    {
        private enum ClassType
        {
            None = 0,
            Spec1,
            Spec2,
            Spec3
        }
    
        private static void LoadCurrentSpec() { Load(ActiveGroup); }

        private static readonly string[] TabNames = new string[4];
        private static readonly int[] TabPoints = new int[4];
        private static int _indexGroup;
        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public static int Spec
        {
            get
            {
                LoadCurrentSpec();

                int nSpec = 0;
                if (TabPoints[1] == 0 && TabPoints[2] == 0 && TabPoints[3] == 0)
                {
                    if (Me.Level > 9)
                    { Utils.Log("*** NO TALENT POINTS HAVE BEEN SPENT YET ***"); }
                    else if (Me.Level < 10)
                    { Utils.Log("*** Below level 10 no talent points available ***"); }
                    nSpec = 0;
                    return nSpec;
                }

                if (TabPoints[1] >= (TabPoints[2] + TabPoints[3])) nSpec = 1;
                else if (TabPoints[2] >= (TabPoints[1] + TabPoints[3])) nSpec = 2;
                else if (TabPoints[3] >= (TabPoints[1] + TabPoints[2])) nSpec = 3;

                return nSpec;
            }
        }

        public static void Load(int nGroup)
        {
            int nTab;
            _indexGroup = nGroup;


            for (nTab = 1; nTab <= 3; nTab++)
            {
                try
                {
                    string luaCode = String.Format("return GetTalentTabInfo({0},false,false,{1})", nTab, _indexGroup);
                    List<string> tabInfo = Lua.GetReturnValues(luaCode, "stuff.lua");

                    TabNames[nTab] = tabInfo[1];
                    TabPoints[nTab] = Convert.ToInt32(tabInfo[4]);
                }
                catch (Exception ex) { Logging.WriteException(ex); }
            }

        }

        private static int ActiveGroup { get { return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false,false)", 0); } }

    }

    public static class Movement
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public static double MinimumDistance { get; set; }
        public static double MaximumDistance { get; set; }
        public static string LazyRaider { get; set; }

        /// <summary>
        /// Stop moving. Like da!
        /// </summary>
        public static void StopMoving()
        {
            if (LazyRaider.Contains("always")) return;
            while (Me.IsMoving) { WoWMovement.MoveStop(); Thread.Sleep(50); }
        }

        /// <summary>
        /// Move to within X yards of the target
        /// </summary>
        /// <param name="distanceFromTarget">Distance to move to the target</param>
        public static void MoveTo(float distanceFromTarget)
        {
            if (LazyRaider.Contains("always")) return;
            // Let HB do the math and find a WoWPoint X yards away from the target
            WoWPoint moveToHere = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, distanceFromTarget);

            // Use HB navigation to move to a WoWPoint. WoWPoint has been calculated in the above code
            Navigator.MoveTo(moveToHere);
        }

        public static WoWPoint PointFromTarget(float distanceFromTarget)
        {
            // Let HB do the math and find a WoWPoint X yards away from the target
            WoWPoint pointFromTarget = WoWMathHelper.CalculatePointFrom(Me.Location, Me.CurrentTarget.Location, distanceFromTarget);

            return pointFromTarget;
        }

        public static void MoveTo(WoWPoint location)
        {
            if (LazyRaider.Contains("always")) return;
            Navigator.MoveTo(location);
        }

        public static bool NeedToCheck()
        {
            float interactRange = Me.CurrentTarget.InteractRange - 2.0f;
            if (LazyRaider.Contains("always")) return false;

            if (!Target.IsFleeing && Me.CurrentTarget.Distance <= interactRange) return false;

            // If distance is less than ClassHelper.MinimumDistance and the target is NOT running away and we are moving then we should stop moving
            if (Target.IsDistanceLessThan(MinimumDistance) && !Target.IsFleeing && Me.IsMoving) WoWMovement.MoveStop();

            // TRUE if we are out of range. TRUE means we need to move closer
            bool result = Target.IsDistanceMoreThan(MaximumDistance);

            return result;
        }

        /// <summary>
        /// Move to melee distance if we need to
        /// </summary>
        public static void DistanceCheck()
        {
            // No target, nothing to do
            if (!Me.GotTarget) return;
            if (LazyRaider.Contains("always")) return;

            // If we're too close stop moving
            if (Target.IsDistanceLessThan(MinimumDistance)) StopMoving();

            // If we're more than X yards away from the current target then move to X yards from the target
            DistanceCheck(MaximumDistance, MinimumDistance);
        }

        /// <summary>
        /// TRUE if we need to perform a distance check
        /// </summary>
        public static bool NeedToCheck(double minimumDistance)
        {
            float interactRange = Me.CurrentTarget.InteractRange - 2.0f;
            if (LazyRaider.Contains("always")) return false;
            if (!Target.IsFleeing && Me.CurrentTarget.Distance <= interactRange) return false;

            // If distance is less than ClassHelper.MinimumDistance and the target is NOT running away and we are moving then we should stop moving
            if (Target.IsDistanceLessThan(minimumDistance) && !Target.IsFleeing && Me.IsMoving) WoWMovement.MoveStop();

            // TRUE if we are out of range. TRUE means we need to move closer
            bool result = Target.IsDistanceMoreThan(minimumDistance);

            return result;
        }


        /// <summary>
        /// Move to range and stop moving if we are too close
        /// </summary>
        /// <param name="maxDistance">Maximum distance away from the target you want to be. This should be max spell range or melee range</param>
        /// <param name="moveToDistance">If your distance is greater than maxDistance you will move to this distance from the target</param>
        public static void DistanceCheck(double maxDistance, double moveToDistance)
        {
            // No target, nothing to do
            if (!Me.GotTarget) return;
            if (LazyRaider.Contains("always")) return;

            // If target (NPC) is running away move as close a possible to the target
            if (Me.GotTarget && Me.CurrentTarget.Fleeing)
            {
                Utils.Log("Runner!");
                moveToDistance = -0.5;
            }

            float interactRange = Me.CurrentTarget.InteractRange - 1;
            if (!Target.IsFleeing && Target.IsDistanceLessThan(interactRange)) return;

            // We're too far from the target so move closer
            if (Me.CurrentTarget.Fleeing && Target.Distance > moveToDistance || Target.Distance > maxDistance)
            {
                Utils.Log(String.Format("Moving closer to {0}", Me.CurrentTarget.Name), Color.FromName("DarkGreen"));
                MoveTo((float)moveToDistance);
            }

            // We're too close to the target we need to stop moving
            if (Target.Distance <= moveToDistance && Me.IsMoving)
            {
                //Utils.Log("We are too close, stop moving.", Color.FromName("DarkGreen"));
                WoWMovement.MoveStop();
                return;
            }

            // We don't need to do anything so just exit
            if (Target.Distance > moveToDistance || !Me.IsMoving || Me.CurrentTarget.Fleeing)
                return;



            // When all else fails just stop moving
            WoWMovement.MoveStop();

        }

        #region Find Safe Location
        //************************************************************************************
        // Snazzy code Apoc gave me (from HB corpse rez)
        // It will find the best location furtherest away from mobs
        // I use this for kiting



        private static List<WoWPoint> AllMobsAroundUs
        {
            get
            {
                List<WoWUnit> mobs = (from o in ObjectManager.ObjectList
                                      where o is WoWUnit && o.Distance < 100
                                      let u = o.ToUnit()
                                      where u.Attackable && u.IsAlive && u.IsHostile
                                      select u).ToList();

                return mobs.Select(u => u.Location).ToList();
            }
        }

        private static WoWPoint NearestMobLoc(WoWPoint p, IEnumerable<WoWPoint> mobLocs)
        {
            var lst = (mobLocs.OrderBy(u => u.Distance(p)));
            return mobLocs.OrderBy(u => u.Distance(p)).Count() > 0 ?  lst.First() :  new WoWPoint(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        public static WoWPoint FindSafeLocation(double distanceFromTarget)
        {
            if (LazyRaider.Contains("always")) return null;

            WoWPoint myLocation = Me.Location;
            WoWPoint destinationLocation;
            List<WoWPoint> mobLocations = new List<WoWPoint>();

            mobLocations = AllMobsAroundUs;
            double bestSafetyMargin = distanceFromTarget;

            mobLocations.Add(Me.CurrentTarget.Location);

            // Rotate 10 degrees each itteration
            for (float degrees = 0f; degrees < 360f; degrees += 10f)
            {
                // Search 5 yards further away each itteration
                for (float distanceFromMob = 0f; distanceFromMob <= 35f; distanceFromMob += 5f)
                {
                    destinationLocation = myLocation.RayCast((float)(degrees * Math.PI / 180f), distanceFromMob);
                    double mobDistance = destinationLocation.Distance2D(NearestMobLoc(destinationLocation, mobLocations));

                    // Mob(s) too close to our current base-safe location, not a suitable location
                    if (mobDistance <= bestSafetyMargin) continue;

                    // Found a mob-free location, lets do further testing.
                    // * Check if we can generate a path
                    // * Check if we have LOS 

                    // Can we generate a path to this location?
                    if (Navigator.GeneratePath(Me.Location, destinationLocation).Length <= 0)
                    {
                        //Utils.Log("Mob-free location failed path generation check");
                        continue;
                    }

                    // Is the destination in line of sight?
                    if (!GameWorld.IsInLineOfSight(Me.Location, destinationLocation))
                    {
                        //Utils.Log("Mob-free location failed line of sight check");
                        continue;
                    }

                    // We pass all checks. This is a good location 
                    // Make it so 'Number 1', "Engage"
                    return destinationLocation;

                }
            }

            return Me.Location;

        }

        #endregion

    }

    public static class Inventory
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public static class ManaPotions
        {

            private static WoWItem ManaPotion
            {
                get { return Me.CarriedItems.Where(item => item.Name.Contains("Mana Pot")).FirstOrDefault(); }
            }

            public static bool IsUseable
            {
                get
                {
                    if (ManaPotion == null) return false;

                    string luacode = String.Format("return GetItemCooldown(\"{0}\")", ManaPotion.Entry);
                    return Utils.LuaGetReturnValueString(luacode) == "0";
                }
            }

            public static void Use()
            {
                if (Me.IsCasting) Spell.StopCasting();
                Utils.LagSleep();

                WoWItem manaPotion = ManaPotion;
                Utils.Log(string.Format("We're having an 'Oh Shit' moment. Using {0}", manaPotion.Name), Utils.Colour("Red"));
                ManaPotion.Interact();
            }

        }

        public static class HealthPotions
        {
            /// <summary>
            /// WoWItem type of a suitable Healing Potion in your bags. Null if nothing is found
            /// </summary>
            private static WoWItem HealthPotion
            {
                get { return Me.CarriedItems.Where(item => item.Name.Contains("Healing Pot")).FirstOrDefault(); }
            }

            /// <summary>
            /// Checks if this item is not on cooldown and can be used. Returns TRUE is the item is ok to be used
            /// </summary>
            public static bool IsUseable
            {
                get
                {
                    if (HealthPotion == null) return false;

                    string luacode = String.Format("return GetItemCooldown(\"{0}\")", HealthPotion.Entry);
                    return Utils.LuaGetReturnValueString(luacode) == "0";

                    /*


                    if (HealthPotion == null) return false;
                    Utils.Log(string.Format("We have a health potion, {0}, lets see if its on cooldown...", HealthPotion.Name));

                    //string luacode = String.Format("return GetItemCooldown(\"{0}\")", HealthPotion.Entry);
                    //Utils.Log("***** LUA = " + luacode);
                    //Utils.Log("++++ cooldown = " + HealthPotion.Cooldown);

                    //bool result = (Utils.LuaGetReturnValueString(luacode) == "0");
                    bool result = HealthPotion.Cooldown == 0;
                    Utils.Log(string.Format("Potion is useable {0}", result));

                    return result;
                     */
                }
            }

            public static void Use()
            {
                if (Me.IsCasting) Spell.StopCasting();
                Utils.LagSleep();

                WoWItem healthPotion = HealthPotion;
                Utils.Log(string.Format("We're having an 'Oh Shit' moment. Using {0}", healthPotion.Name), Utils.Colour("Red"));
                HealthPotion.Interact();
            }

        }

        public static void Drink()
        {
            // If we're not using smart eat and drink then don't do this stuff, just sit and drink

            WoWItem drink = Styx.Logic.Inventory.Consumable.GetBestDrink(false);
            if (drink == null) return;
            Utils.Log(string.Format("Drinking {0}", drink.Name), Utils.Colour("Blue"));
            LevelbotSettings.Instance.DrinkName = drink.Name;
            Styx.Logic.Common.Rest.Feed();
        }

        public static void Eat()
        {
            // If we're not using smart eat and drink then don't do this stuff, just sit and eat

            WoWItem food = Styx.Logic.Inventory.Consumable.GetBestFood(false);
            if (food == null) return;
            Utils.Log(string.Format("Eating {0}", food.Name), Utils.Colour("Blue"));
            LevelbotSettings.Instance.FoodName = food.Name;
            Styx.Logic.Common.Rest.Feed();
        }

        public static bool HaveItem(string itemName)
        {
            return Me.BagItems.Any(item => item.Name.ToUpper() == itemName.ToUpper());
        }

        public static bool HaveItem(int itemNumber)
        {
            return Me.BagItems.Any(item => item.ItemInfo.Id == itemNumber);
        }
    }
    
    public static class EventHandlers
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return Me.CurrentTarget; } }

        public static void CombatLogEventHander(object sender, LuaEventArgs args)
        {
            foreach (object arg in args.Args)
            {
                if (!(arg is String)) continue;

                var s = (string)arg;

                //if (s.Contains("Crab")) Utils.Log("CRAB-EH!",Color.Red);

                if (s.Contains("EVADE") && Me.GotTarget)
                {
                    Logging.Write("My target is Evade bugged, blacking " + CT.Name, Color.Red);
                    Target.BlackList(3600);
                    Lua.DoString("StopAttack() PetStopAttack() PetFollow()");
                    StyxWoW.Me.ClearTarget();
                }
            }
        }

        public static void TalentPointEventHander(object sender, LuaEventArgs args)
        {
            //ClassHelper.ClassSpec = (Talents.ClassType)Talents.Spec;
        }


    }

    public static class CLC
    {
        //*********************************************************************************************************
        // 
        // This is what I call my "Common Language Configuration" system.
        // It takes common language terms and makes a TRUE/FALSE setting from it.
        // I'm sure there are more elegant ways of undertaking this but it suites my purposes perfectly.
        //
        // You pass a RAW string and simply check if a phrase or keyword is present
        // Its a lot more intuitive for the user and it gives me more control over the UI
        // This way I don't need hundreds of tick boxes or controls
        // 
        //*********************************************************************************************************


        /// <summary>
        /// This is the property you assign the raw 'setting' to. The raw setting is the value passed from the Settings.XXXX property
        /// Eg CLC.RawSetting = Settings.Cleanse; Checking CLC.AfterCombatEnds you will see it returns TRUE
        /// </summary>
        public static string RawSetting { get; set; }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        // Adds 2 - 5
        private static bool OnAdds { get { return RawSetting.Contains("only on adds"); } }
        private static bool NoAdds { get { return RawSetting.Contains("only when no adds"); } }
        private static bool OnAdds3OrMore { get { return RawSetting.Contains("only on 3+ adds"); } }
        private static bool OnAdds4OrMore { get { return RawSetting.Contains("only on 4+ adds"); } }
        private static bool OnAdds5OrMore { get { return RawSetting.Contains("only on 5+ adds"); } }
        private static bool OnAdds6OrMore { get { return RawSetting.Contains("only on 6+ adds"); } }
        private static bool OnAdds7OrMore { get { return RawSetting.Contains("only on 7+ adds"); } }
        private static bool OnAdds8OrMore { get { return RawSetting.Contains("only on 8+ adds"); } }

        // Call these properties from the CC in order to check if a condition is met. 
        private static bool Always { get { return RawSetting.Contains("always"); } }
        private static bool Never { get { return RawSetting.Contains("never"); } }
        private static bool OnRunners { get { return RawSetting.Contains("on runners") || RawSetting.Contains("and runners"); } }
        private static bool IfCasting { get { return RawSetting.Contains("on casters") || RawSetting.Contains("casters and"); } }
        private static bool IfCastingOrRunning { get { return RawSetting.Contains("casters and runners"); } }
        private static bool OutOfCombat { get { return RawSetting.Contains("out of combat") || RawSetting.Contains("during pull"); } }
        private static bool Immediately { get { return RawSetting.Contains("immediately"); } }
        private static bool InBGs { get { return RawSetting.Contains("only in battleground") || RawSetting.Contains("when in battleground"); } }
        private static bool NotInBGs { get { return RawSetting.Contains("not in battleground"); } }
        private static bool InInstances { get { return RawSetting.Contains("only when in instance") || RawSetting.Contains("inside an instance") || RawSetting.Contains("only in instance"); } }
        private static bool NotInInstances { get { return RawSetting.Contains("not in instance") || RawSetting.Contains("not inside an instance"); } }
        private static bool OnHumanoids { get { return RawSetting.Contains("on humanoids"); } }
        private static bool OnUndead{ get { return RawSetting.Contains("undead"); } }
        private static bool OnDemons { get { return RawSetting.Contains("demons"); } }
        private static bool OnBeasts { get { return RawSetting.Contains("beasts"); } }
        private static bool OnElementals{ get { return RawSetting.Contains("elemental"); } }
        private static bool OnFleeing { get { return RawSetting.Contains("on fleeing"); } }
        private static bool NotLowLevel { get { return RawSetting.Contains("not low level"); } }

        // Combo points - Rogues and Druids (Cat)
        private static bool ComboPoints12 { get { return RawSetting.Contains("1-2 combo"); } }
        private static bool ComboPoints23 { get { return RawSetting.Contains("2-3 combo"); } }
        private static bool ComboPoints34 { get { return RawSetting.Contains("3-4 combo"); } }
        private static bool ComboPoints45 { get { return RawSetting.Contains("4-5 combo"); } }
        private static bool ComboPoints1OrMore { get { return RawSetting.Contains("1+ Combo"); } }
        private static bool ComboPoints2OrMore { get { return RawSetting.Contains("2+ Combo"); } }
        private static bool ComboPoints3OrMore { get { return RawSetting.Contains("3+ Combo"); } }
        private static bool ComboPoints4OrMore { get { return RawSetting.Contains("4+ Combo"); } }
        private static bool ComboPoints5OrMore { get { return RawSetting.Contains("5+ Combo"); } }

        // Holy Power - Paladin only
        private static bool HolyPower1OrMore { get { return RawSetting.Contains("1+ Holy Power"); } }
        private static bool HolyPower2OrMore { get { return RawSetting.Contains("2+ Holy Power"); } }
        private static bool HolyPower3OrMore { get { return RawSetting.Contains("3+ Holy Power"); } }

        // Shadow Orbs - Priest only
        private static bool MindSpike1OrMore { get { return RawSetting.Contains("1+ Mind Spike"); } }
        private static bool MindSpike2OrMore { get { return RawSetting.Contains("2+ Mind Spike"); } }
        private static bool MindSpike3OrMore { get { return RawSetting.Contains("3+ Mind Spike"); } }
        private static bool ShadowOrb1OrMore { get { return RawSetting.Contains("1+ Shadow Orb"); } }
        private static bool ShadowOrb2OrMore { get { return RawSetting.Contains("2+ Shadow Orb"); } }
        private static bool ShadowOrb3OrMore { get { return RawSetting.Contains("3+ Shadow Orb"); } }
        private static bool InShadowform { get { return RawSetting.Contains("only in Shadowform") || RawSetting.Contains("only when in Shadowform"); } }
        private static bool NotInShadowform { get { return RawSetting.Contains("not in Shadowform"); } }

        // Hunter Focus Fire Frenzy Stacks
        private static bool FocusFire1OrMore { get { return RawSetting.Contains("1+ Frenzy"); } }
        private static bool FocusFire2OrMore { get { return RawSetting.Contains("2+ Frenzy"); } }
        private static bool FocusFire3OrMore { get { return RawSetting.Contains("3+ Frenzy"); } }
        private static bool FocusFire4OrMore { get { return RawSetting.Contains("4+ Frenzy"); } }
        private static bool FocusFire5OrMore { get { return RawSetting.Contains("5+ Frenzy"); } }

        // Hunter Focus 
        private static bool Focus2OrMore { get { return RawSetting.Contains("focus 20+"); } }
        private static bool Focus25rMore { get { return RawSetting.Contains("focus 25+"); } }
        private static bool Focus3OrMore { get { return RawSetting.Contains("focus 30+"); } }
        private static bool Focus35rMore { get { return RawSetting.Contains("focus 35+"); } }
        private static bool Focus4OrMore { get { return RawSetting.Contains("focus 40+"); } }
        private static bool Focus45rMore { get { return RawSetting.Contains("focus 45+"); } }
        private static bool Focus5OrMore { get { return RawSetting.Contains("focus 50+"); } }
        private static bool Focus55rMore { get { return RawSetting.Contains("focus 55+"); } }
        private static bool Focus6OrMore { get { return RawSetting.Contains("focus 60+"); } }
        private static bool Focus65rMore { get { return RawSetting.Contains("focus 65+"); } }
        private static bool Focus7OrMore { get { return RawSetting.Contains("focus 70+"); } }
        private static bool Focus75rMore { get { return RawSetting.Contains("focus 75+"); } }
        private static bool Focus8OrMore { get { return RawSetting.Contains("focus 80+"); } }
        private static bool Focus85rMore { get { return RawSetting.Contains("focus 85+"); } }
        private static bool Focus9OrMore { get { return RawSetting.Contains("focus 90+"); } }
        private static bool Focus95rMore { get { return RawSetting.Contains("focus 95+"); } }

        // Health
        private static bool HealthLessThan90 { get { return RawSetting.Contains("health below 90"); } }
        private static bool HealthLessThan80 { get { return RawSetting.Contains("health below 80"); } }
        private static bool HealthLessThan70 { get { return RawSetting.Contains("health below 70"); } }
        private static bool HealthLessThan60 { get { return RawSetting.Contains("health below 60"); } }
        private static bool HealthLessThan50 { get { return RawSetting.Contains("health below 50"); } }
        private static bool HealthLessThan40 { get { return RawSetting.Contains("health below 40"); } }
        private static bool HealthLessThan30 { get { return RawSetting.Contains("health below 30"); } }
        private static bool HealthLessThan20 { get { return RawSetting.Contains("health below 20"); } }

        // Stacks
        private static bool Stacks1OrMore { get { return RawSetting.Contains("1+ stacks"); } }
        private static bool Stacks2OrMore { get { return RawSetting.Contains("2+ stacks"); } }
        private static bool Stacks3OrMore { get { return RawSetting.Contains("3+ stacks"); } }
        private static bool Stacks4OrMore { get { return RawSetting.Contains("4+ stacks"); } }
        private static bool Stacks5OrMore { get { return RawSetting.Contains("5+ stacks"); } }

        public static bool IsOkToRun
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(RawSetting)) return false;                                                 // No string passed so nothing to check
                    if (Always || Immediately) return true;                                                             // Always means always
                    if (Never) return false;                                                                            // No means no! You men are all the same
                    if (OutOfCombat && !Me.Combat) return true;                                                         // Only if we're not in combat
                    if (RawSetting.Contains("+ adds") && !Utils.Adds) return false;
                    if (OnAdds && Utils.Adds && Me.Combat) return true;                                                               // Only if we have adds
                    if (NoAdds && !Utils.Adds && Me.Combat) return true;                                                               // Only if we DON'T have adds
                    if (Me.GotTarget && IfCastingOrRunning && (Me.CurrentTarget.IsCasting || Me.CurrentTarget.Fleeing)) return true;    // Only if the target is casting or running
                    if (OnRunners && Me.GotTarget && Me.CurrentTarget.Fleeing) return true;                                              // Only if target (NPC) is running away
                    if (IfCasting && Me.GotTarget && Me.CurrentTarget.IsCasting) return true;                                            // Only if target is casting
                    if (InInstances && Me.IsInInstance) return true;                                                    // Only if you are inside an instance
                    if (InBGs && Utils.IsBattleground) return true;                                                     // Only if you are inside a battleground
                    if (NotInBGs && !Utils.IsBattleground) return true;                                                     // Only if you are NOT inside a battleground
                    if (OnHumanoids && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Humanoid) return true;  // Humanoids only. Mostly for runners
                    if (OnUndead && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Undead) return true;  
                    if (OnDemons && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Demon) return true;  
                    if (OnBeasts && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Beast) return true;
                    if (OnElementals && Me.GotTarget && Me.CurrentTarget.CreatureType == WoWCreatureType.Elemental) return true;  
                    if (OnFleeing && Me.GotTarget && Me.CurrentTarget.Fleeing) return true;  // Is target fleeing
                    if (NotInInstances && !Me.IsInInstance) return true;
                    if (NotLowLevel && Me.GotTarget && !Target.IsLowLevel) return true;

                    // X+ Adds
                    if (RawSetting.Contains("+ adds"))
                    {
                        if (OnAdds3OrMore && Utils.AddsCount >= 3) return true;
                        if (OnAdds4OrMore && Utils.AddsCount >= 4) return true;
                        if (OnAdds5OrMore && Utils.AddsCount >= 5) return true;
                        if (OnAdds6OrMore && Utils.AddsCount >= 6) return true;
                        if (OnAdds7OrMore && Utils.AddsCount >= 7) return true;
                        if (OnAdds8OrMore && Utils.AddsCount >= 8) return true;
                    }

                    // Health < X
                    if (RawSetting.Contains("health below"))
                    {
                        if (HealthLessThan90 && Me.HealthPercent < 90) return true;
                        if (HealthLessThan80 && Me.HealthPercent < 80) return true;
                        if (HealthLessThan70 && Me.HealthPercent < 70) return true;
                        if (HealthLessThan60 && Me.HealthPercent < 60) return true;
                        if (HealthLessThan50 && Me.HealthPercent < 50) return true;
                        if (HealthLessThan40 && Me.HealthPercent < 40) return true;
                        if (HealthLessThan30 && Me.HealthPercent < 30) return true;
                        if (HealthLessThan20 && Me.HealthPercent < 20) return true;
                    }

                    // If you are not a Rogue or a Druid (Cat) then don't do these checks
                    if (Me.Class == WoWClass.Rogue || Me.Class == WoWClass.Druid && Me.Shapeshift == ShapeshiftForm.Cat)
                    {
                        if (Me.ComboPoints <= 0) return false;
                        if (ComboPoints45 && Me.ComboPoints >= 4) return true;
                        if (ComboPoints34 && (Me.ComboPoints >= 3 && Me.ComboPoints <= 4)) return true;
                        if (ComboPoints23 && (Me.ComboPoints >= 2 && Me.ComboPoints <= 3)) return true;
                        if (ComboPoints12 && (Me.ComboPoints >= 1 && Me.ComboPoints <= 2)) return true;

                        if (ComboPoints1OrMore && Me.ComboPoints >= 1) return true;
                        if (ComboPoints2OrMore && Me.ComboPoints >= 2) return true;
                        if (ComboPoints3OrMore && Me.ComboPoints >= 3) return true;
                        if (ComboPoints4OrMore && Me.ComboPoints >= 4) return true;
                        if (ComboPoints5OrMore && Me.ComboPoints >= 5) return true;
                    }

                    if (Me.Class == WoWClass.Paladin)
                    {
                        //if (Me.CurrentHolyPower <= 0) return false;
                        if (HolyPower1OrMore && Me.CurrentHolyPower >= 1) return true;
                        if (HolyPower2OrMore && Me.CurrentHolyPower >= 2) return true;
                        if (HolyPower3OrMore && Me.CurrentHolyPower >= 3) return true;

                        // Other Misc
                        if (RawSetting.Contains("Sacred Duty") && IsBuffPresent("Sacred Duty")) return true;
                    }

                    if (Me.Class == WoWClass.Priest)
                    {
                        // Archangel / Evangelism
                        if (Stacks1OrMore && Self.StackCountLUA("Evangelism") >= 1) return true;
                        if (Stacks2OrMore && Self.StackCountLUA("Evangelism") >= 2) return true;
                        if (Stacks3OrMore && Self.StackCountLUA("Evangelism") >= 3) return true;
                        if (Stacks4OrMore && Self.StackCountLUA("Evangelism") >= 4) return true;
                        if (Stacks5OrMore && Self.StackCountLUA("Evangelism") >= 5) return true;

                        if (InShadowform && Self.IsBuffOnMe("Shadowform")) return true;
                        if (NotInShadowform && !Self.IsBuffOnMe("Shadowform")) return true;

                        //if (!Self.IsBuffOnMe(87178, Self.AuraCheck.AllAuras)) return false;
                        int mindSpikeCount = Target.DebuffStackCount("Mind Spike");
                        //int mindSpikeCount = Target.StackCount(87178);
                        if (MindSpike1OrMore && mindSpikeCount >= 1) return true;
                        if (MindSpike2OrMore && mindSpikeCount >= 2) return true;
                        if (MindSpike3OrMore && mindSpikeCount >= 3) return true;

                        // If no Shadow Orbs then bail out now. 
                        if (!Self.IsBuffOnMe(77487,Self.AuraCheck.AllAuras)) return false;

                        int orbCount = Self.StackCount(77487);
                        if (ShadowOrb1OrMore && orbCount >= 1) return true;
                        if (ShadowOrb2OrMore && orbCount >= 2) return true;
                        if (ShadowOrb3OrMore && orbCount >= 3) return true;

                    }

                    if (Me.Class == WoWClass.Hunter)
                    {
                        if (Focus2OrMore && Me.FocusPercent >= 20) return true;
                        if (Focus25rMore && Me.FocusPercent >= 25) return true;
                        if (Focus3OrMore && Me.FocusPercent >= 30) return true;
                        if (Focus35rMore && Me.FocusPercent >= 35) return true;
                        if (Focus4OrMore && Me.FocusPercent >= 40) return true;
                        if (Focus45rMore && Me.FocusPercent >= 45) return true;
                        if (Focus5OrMore && Me.FocusPercent >= 50) return true;
                        if (Focus55rMore && Me.FocusPercent >= 55) return true;
                        if (Focus6OrMore && Me.FocusPercent >= 60) return true;
                        if (Focus65rMore && Me.FocusPercent >= 65) return true;
                        if (Focus7OrMore && Me.FocusPercent >= 70) return true;
                        if (Focus75rMore && Me.FocusPercent >= 75) return true;
                        if (Focus8OrMore && Me.FocusPercent >= 80) return true;
                        if (Focus85rMore && Me.FocusPercent >= 85) return true;
                        if (Focus9OrMore && Me.FocusPercent >= 90) return true;
                        if (Focus95rMore && Me.FocusPercent >= 95) return true;

                        if (FocusFire1OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 1) return true;
                        if (FocusFire2OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 2) return true;
                        if (FocusFire3OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 3) return true;
                        if (FocusFire4OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 4) return true;
                        if (FocusFire5OrMore && Me.GotAlivePet && Me.Pet.Auras.ContainsKey("Frenzy Effect") && Me.Pet.Auras["Frenzy Effect"].StackCount >= 5) return true;
                    }
                }

                catch (Exception e)
                {
                    MethodBase currMethod = MethodBase.GetCurrentMethod();
                    //Debug.ModuleName = currMethod.Name;
                    //Debug.Catch(e);
                }

                return false;   // Otherwise its not going to happen
            }

        }

        public static bool ResultOK(string clcSettingString)
        {
            RawSetting = clcSettingString;
            bool result = IsOkToRun;
            return result;
        }


        private static bool IsBuffPresent(string buffToCheck)
        {
            Lua.DoString(string.Format("buffName,_,_,stackCount,_,_,_,_,_=UnitBuff(\"player\",\"{0}\")", buffToCheck));
            string buffName = Lua.GetLocalizedText("buffName", Me.BaseAddress);
            return (buffName == buffToCheck);
        }
    }

    public static class ConfigSettings
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public static string FileName = @"CustomClasses\Default Hunter\Class Specific\Config\Settings.xml";
        private static string _userFileName = "";
        private static XmlDocument _xmlDoc = new XmlDocument();
        private static XmlNode _xvar;
        private static string _currentEnvironment;
        public static bool UIActive { get; set; }
        public static string CurrentEnvironment
        {
            get { if (String.IsNullOrEmpty(_currentEnvironment)) _currentEnvironment = "PVE"; return _currentEnvironment; }
            set { _currentEnvironment = value; }
        }

        public static string UserFileName
        {
            get { _userFileName = FileName.Replace("Settings.xml", string.Format("Settings {0} {1}.xml", Me.Name,CurrentEnvironment));  return _userFileName; }
        }

        public static bool Open()
        {
            try
            {
                if (!System.IO.File.Exists(UserFileName)) System.IO.File.Copy(FileName, UserFileName);
                _xmlDoc.Load(UserFileName);
            }
            catch (Exception e)
            {
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(String.Format("Exception in XML Load {0}", e.Message));
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
                return false;
            }
            
            return true;
        }

        public static void Save()
        {
            _xmlDoc.Save(UserFileName);
        }

        public static void SetProperty(string nodeName, string nodeValue)
        {
            try
            {
                _xmlDoc.SelectSingleNode(nodeName).InnerText = nodeValue;
                
            }
            catch (Exception e)
            {
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug(String.Format("Exception in XML Save {0}", e.Message));
                Logging.WriteDebug(" ");
                Logging.WriteDebug(" ");
                Logging.WriteDebug("**********************************************************************");
                Logging.WriteDebug("**********************************************************************");
            }
        }

        public static string GetStringProperty(string nodeName)
        {
            _xvar = _xmlDoc.SelectSingleNode(nodeName);
            return Convert.ToString(_xvar.InnerText);
        }

        public static bool GetBoolProperty(string nodeName)
        {
            _xvar = _xmlDoc.SelectSingleNode(nodeName);
            return Convert.ToBoolean(_xvar.InnerText);
        }

        public static int GetIntProperty(string nodeName)
        {
            _xvar = _xmlDoc.SelectSingleNode(nodeName);
            return Convert.ToInt16(_xvar.InnerText);
        }

    }

    public static class RAF
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }

        public static WoWUnit PartyMemberWithoutBuff(string buffName)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty && !Me.IsInRaid) return null;
            return Me.PartyMembers.FirstOrDefault(p => p.Distance < 40 && !p.Dead && !p.IsGhost && p.InLineOfSight && !p.Auras.ContainsKey(buffName));
        }

        public static WoWUnit BuffPlayer(string buffname)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty && !Me.IsInRaid) return null;

            // MyGroup is populated with raid members or party members, whichever you are in
            List<WoWPlayer> myPartyOrRaidGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            return (from o in myPartyOrRaidGroup let p = o.ToPlayer() where p.Distance < 30 && !p.Dead && !p.IsGhost && p.InLineOfSight && !p.ActiveAuras.ContainsKey(buffname) select p).FirstOrDefault();
        }

        public static WoWUnit HealPlayer(int minimumHealth, int maximumDistance)
        {
            // If you're not in a party then just leave
            if (!Me.IsInParty && !Me.IsInRaid) return null;

            // MyGroup is populated with raid members or party members, whichever you are in
            List<WoWPlayer> myPartyOrRaidGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            return (from o in myPartyOrRaidGroup
                    let p = o.ToPlayer() where p.Distance < maximumDistance && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < minimumHealth orderby p.HealthPercent ascending select p).FirstOrDefault();
        }

        public static WoWUnit PartyHealerRole
        {
            get
            {
                if (!Me.IsInParty) return null;

                int i = 1;
                foreach (WoWPlayer player in Me.PartyMembers)
                {
                    string memberRole = Lua.GetReturnVal<string>(string.Format(@"return UnitGroupRolesAssigned(""party{0}"")", i), 0);

                    if (memberRole == "HEALER")
                    {
                        return player;
                    }
                    i++;
                }

                return null;
            }
        }

        public static WoWUnit PartyTankRole
        {
            get
            {
                if (!Me.IsInParty) return null;

                int i = 1;
                foreach (WoWPlayer p in Me.PartyMembers)
                {
                    string partyRole = Lua.GetReturnVal<string>(string.Format(@"return UnitGroupRolesAssigned(""party{0}"")", i), 0);

                    if (partyRole == "TANK")
                    {
                        return p;
                    }
                    i++;
                }

                return null;
            }
        }

    }

    public static class HealBot
    {
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }
        private static WoWUnit healer { get { return RAF.PartyHealerRole; } }
        private static WoWUnit tank { get { return RAF.PartyTankRole; } }

        /// <summary>
        /// Unified Healing System
        /// </summary>
        private static List<HealingSpell> _uhs = new List<HealingSpell>();

        /// <summary>
        /// Unified Healing System
        /// </summary>
        public static List<HealingSpell> UHS { get { return _uhs; } set { _uhs = value; } }

        [DefaultValue(true)]
        public static bool HealPets { get; set; }

        public class HealingSpell
        {
            /// <summary>
            /// Name of the spell to be cast
            /// </summary>
            public string SpellName { get; set; }
            /// <summary>
            /// 0 Highest priority
            /// </summary>
            [DefaultValue(0)]
            public int Priority { get; set; }
            /// <summary>
            /// Is this a buff/HoT spell. Uses spell name to identify the buff
            /// </summary>
            [DefaultValue(false)]
            public bool IsBuff { get; set; }
            /// <summary>
            /// Check for other debuffs before casting this spell. Eg PWS needs to check for Weakened Soul
            /// </summary>
            public string OtherDebuffs{ get; set; }
            /// <summary>
            /// Only evaluated if the target is the tank
            /// </summary>
            [DefaultValue(false)]
            public bool TankOnly { get; set; }
            public double HealthPercent { get; set; }
            /// <summary>
            /// Heal pets with this spell
            /// </summary>
            [DefaultValue(false)]
            public bool IncludePets { get; set; }
            /// <summary>
            /// Minimum number of mobs attacking player
            /// </summary>
            public int MinimumMobAggroCount { get; set; }
            /// <summary>
            /// Does this spell require 'click-to-cast'
            /// </summary>
            [DefaultValue(false)]
            public bool IsGroundTargeted { get; set; }
            /// <summary>
            /// Is this an AoE spell - effects multiple players
            /// </summary>
            [DefaultValue(false)]
            public bool IsAoE { get; set; }
            /// <summary>
            /// Minimum number of players below 'HealthPercent' before spell is considered
            /// </summary>
            [DefaultValue(3)]
            public int MinimumAoECount { get; set; }

            [DefaultValue(40)]
            public int MaximumDistance { get; set; }

            /// <summary>
            /// Evaluate this spell before checking the tank. 
            /// </summary>
            [DefaultValue(false)]
            public bool EvaluateBeforeTank { get; set; }
        }

        /// <summary>
        /// Sort all spells by priority.
        /// </summary>
        public static void Sort()
        {
            UHS.Sort((hs1, hs2) => hs1.Priority.CompareTo(hs2.Priority));
        }

        /// <summary>
        /// Enumerate the list of spells and party members. Heal where required
        /// </summary>
        /// <returns></returns>
        public static bool Heal()
        {
            WoWUnit Tank = tank;    // Try to cut down on the LUA calls

            // First - Check all spells to be evaluated before the tank - typically quick cast AoE spells
            foreach (HealingSpell h in UHS.Where(h => h.EvaluateBeforeTank))
            {
                if (h.IsAoE)
                {
                    int maxDistance = h.MaximumDistance > 0 ? h.MaximumDistance : 40;
                    WoWUnit AoETarget = null;

                    if (CanCastAoE(h.SpellName, maxDistance, h.MinimumAoECount, h.HealthPercent, out AoETarget))
                    {
                        Spell.Cast(h.SpellName, AoETarget ?? Me);
                        Utils.LagSleep();
                        Utils.WaitWhileCasting();
                        return true;
                    }
                }
            }

            // Now - Heal the tank
            if (Tank != null && !Tank.Dead)
            {
                string bestSpell;
                
                // But..... before healing the tank, check if we don't have any urgent heals to cast on party members
                double urgentLowHealth = Tank.HealthPercent*0.60f;
                WoWUnit urgentHealTarget = RAF.HealPlayer((int) urgentLowHealth, 40);
                if (urgentHealTarget != null && Tank.HealthPercent > 80)
                {
                    Utils.Log( string.Format("-Urgent heal required on {0}, prioritizing over the tank", urgentHealTarget.Class), Utils.Colour("Red"));
                    bestSpell = (BestSpell(UHS, urgentHealTarget));
                    if (bestSpell != null)
                    {
                        Spell.Cast(bestSpell, urgentHealTarget, true);
                        return true;
                    }
                    Utils.Log( "*** If you are seeing this message something went wrong trying to perform a pre-tank urgent heal!", Utils.Colour("Red"));
                    Utils.Log("*** 'BestSpell' was unable to find 'the best spell'!", Utils.Colour("Red"));
                }

                // Now... lets get to healing that tank
                bestSpell = (BestSpell(UHS, Tank));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, Tank, true);
                    return true;
                }
            }

            // Lets do some AoE healing
            foreach (HealingSpell h in UHS.Where(h => h.IsAoE))
            {
                //int maxDistance = h.MaximumDistance > 0 ? h.MaximumDistance : 40;
                WoWUnit AoETarget = null;

                if (CanCastAoE(h.SpellName, h.MaximumDistance, h.MinimumAoECount, h.HealthPercent, out AoETarget))
                {
                    Spell.Cast(h.SpellName, AoETarget ?? Me);
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                    return true;
                }
            }

            // Time to heal ME
            if (Me != null)
            {
                string bestSpell = (BestSpell(UHS, Me));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, Me, true);
                    return true;
                }
            }

            // Now if we have a Healer (which means we're backup healing) heal them
            if (healer != null && healer.Guid != Me.Guid)
            {
                string bestSpell = (BestSpell(UHS, healer));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, healer, true);
                    return true;
                }
            }

            // Now the remaining Party / Raid members
            List<WoWPlayer> myGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
            if (myGroup.Count > 0)
            {
                WoWUnit target = RAF.HealPlayer(99, 40);
                string bestSpell = (BestSpell(UHS, target));
                if (bestSpell != null)
                {
                    Spell.Cast(bestSpell, target, true);
                    return true;
                }
            }

            // Finally, pets - we are only here because no one else required healing
            if (HealPets && (Me.PartyMembers.Any(p => p.GotAlivePet && p.Pet.HealthPercent < 95) && Spell.CanCast("Flash Heal")))
            {
                foreach (WoWPlayer p in myGroup)
                {
                    if (!p.GotAlivePet) continue;
                    if (p.GotAlivePet && p.Pet.HealthPercent > 95) continue;

                    string bestSpell = (BestSpell(UHS, p.Pet));
                    if (bestSpell != null)
                    {
                        Spell.Cast(bestSpell, p.Pet, true);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Find the most suitable spell to cast. Working from the highest priority to the lowest
        /// </summary>
        /// <param name="allHealingSpells"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static string BestSpell(List<HealingSpell> allHealingSpells, WoWUnit target)
        {
            if (target == null) return null;

            foreach (HealingSpell spell in allHealingSpells)
            {
                if (target.Distance > spell.MaximumDistance) continue;
                if (spell.IsBuff && target.Auras.ContainsKey(spell.SpellName) && target.Auras[spell.SpellName].CreatorGuid == Me.Guid) continue;
                if (target.HealthPercent > spell.HealthPercent) continue;
                if (!string.IsNullOrEmpty(spell.OtherDebuffs) && target.Auras.ContainsKey(spell.OtherDebuffs)) continue;
                if (spell.TankOnly && (tank != null && target.Guid != tank.Guid)) continue;
                if (spell.IsAoE) continue;
                if (!spell.IncludePets && target.IsPet) continue;
                if (spell.MinimumMobAggroCount > 0 && Utils.CountOfMobsAttackingPlayer(target.Guid) < spell.MinimumMobAggroCount) continue;
                if (!String.IsNullOrEmpty(spell.SpellName)) continue;
                if (!Spell.CanCast(spell.SpellName)) continue;

                Utils.Log(string.Format("----- {0} HP [{1}]  Spell: {2}", target.Name, target.HealthPercent, spell.SpellName));
                if (string.IsNullOrEmpty(spell.SpellName)) return spell.SpellName;
            }

            return null;
        }

        /// <summary>
        /// Check all parameters and determine if we can cast the AoE spell. If so, pass out the lowest health member
        /// </summary>
        /// <param name="spellName"></param>
        /// <param name="maxDistance"></param>
        /// <param name="minCount"></param>
        /// <param name="minHealth"></param>
        /// <param name="AoETarget"></param>
        /// <returns></returns>
        private static bool CanCastAoE(string spellName, int maxDistance, int minCount, double minHealth, out WoWUnit AoETarget)
        {
            AoETarget = null;
            List<WoWPlayer> countOfUnits = (from o in Me.PartyMembers let p = o.ToPlayer() where p.Distance < maxDistance && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < minHealth orderby p.HealthPercent ascending select p).ToList();
            if (countOfUnits.Count >= minHealth) AoETarget = countOfUnits[0];

            return countOfUnits.Count >= minCount  && Spell.CanCast(spellName);
        }
    }

   
 
   
}
