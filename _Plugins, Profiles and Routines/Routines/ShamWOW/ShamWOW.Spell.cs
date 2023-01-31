/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS COMBAT CLASS WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * ShamWOW Shaman CC
 * 
 * Author:  Bobby53
 * 
 * See the ShamWOW.chm file for Help
 *
 */
#pragma warning disable 642

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Bobby53
{
    partial class Shaman
    {
        static WoWUnit unitLastSpellTarget = null;  // target of last spell cast


        // replacement for SpellManager.GlobalCooldown -- bug in 1.9.2.3 and later on some systems causing it to always be true
        private static bool GCD()
        {
            WoWSpell gcdchk;
            if (SpellManager.HasSpell("Lightning Shield"))
                gcdchk = SpellManager.Spells["Lightning Shield"];
            else
                gcdchk = SpellManager.Spells["Lightning Bolt"];

            return gcdchk.Cooldown;
        }


        public static bool IsCasting()
        {
            return GCD() || ObjectManager.Me.IsCasting; //  || 0 != _me.ChanneledCasting;
        }

        /*
         * Safe_CastSpell()
         * 
         * several different overloads providing the ability to safely
         * cast a spell with or without a range check
         */

        public enum SpellRange { NoCheck, Check };
        public enum SpellWait { NoWait, Complete };

        public static bool Safe_CastSpell(string sSpellName)
        {
            return Safe_CastSpell(sSpellName, SpellRange.NoCheck, SpellWait.Complete);
        }

        public static bool Safe_CastSpellWithRangeCheck(string sSpellName)
        {
            return Safe_CastSpell(sSpellName, SpellRange.Check, SpellWait.Complete);
        }

        public static bool Safe_CastSpell(string sSpellName, SpellRange chkRng, SpellWait chkWait)
        {
            return Safe_CastSpell(null, sSpellName, chkRng, chkWait);
        }

        public static bool Safe_CastSpell(WoWUnit unit, int spellId, SpellRange chkRng, SpellWait chkWait)
        {
            try
            {
                WoWSpell spell = WoWSpell.FromId(spellId);
                if (spell != null)
                    return Safe_CastSpell(unit, spell, chkRng, chkWait);
            }
            catch
            {
                ;
            }

            Slog("Error:  attempt to cast unknown spell #{0}", spellId);
            return false;
        }

        public static bool Safe_CastSpell(WoWUnit unit, string sSpellName, SpellRange chkRng, SpellWait chkWait)
        {
            WoWSpell spell = null;

            try
            {
                // spell = SpellManager.Spells[sSpellName];
                spell = SpellManager.Spells[sSpellName];
                System.Diagnostics.Debug.Assert(spell != null);
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug(">>> HB EXCEPTION in SpellManager.Spells[" + sSpellName + "]");
                Logging.WriteDebug(">>> Spell '" + sSpellName + "' believed to be " + (SpellManager.HasSpell(sSpellName) ? "KNOWN" : "UNKNOWN") + " was used ");
                Logging.WriteException(e);
                throw;
                // return false;
            }

            return Safe_CastSpell(unit, spell, chkRng, chkWait);
        }

        public static bool Safe_CastSpellWithRangeCheck(WoWSpell spell)
        {
            return Safe_CastSpell(null, spell, SpellRange.Check, SpellWait.Complete);
        }

        public static bool Safe_CastSpell(WoWSpell spell)
        {
            return Safe_CastSpell(null, spell, SpellRange.NoCheck, SpellWait.Complete);
        }

        public static bool Safe_CastSpell(WoWUnit unit, WoWSpell spell, SpellRange chkRng, SpellWait chkWait)
        {
            bool bCastSuccessful = false;

            // enoughPower = (_me.GetCurrentPower(spell.PowerType) >= spell.PowerCost);
            if (MeSilenced())
                ;
            else if (chkRng == SpellRange.Check && spell.HasRange
                && ((unit != null && unit.Distance >= spell.MaxRange) || (unit == null && _me.GotTarget && _me.CurrentTarget.Distance >= spell.MaxRange)))
                Dlog("Safe_CastSpell: Spell '{0}' has max range of {1:F1} but target is {2:F1} yds away - not cast", spell.Name, spell.MaxRange, unit != null ? unit.Distance : (_me.GotTarget ? _me.CurrentTarget.Distance : -1));
            else
            {
                WaitForCurrentSpell(spell);
                if (!SpellManager.CanCast(spell))
                {
                    Dlog("Safe_CastSpell: cannot cast spell '{0}' yet - cd={1}, gcd={2}, casting={3}, cost={4}, mana={5}, ", 
                        spell.Name, 
                        spell.Cooldown, 
                        SpellManager.GlobalCooldown, 
                        _me.IsCasting,
                        spell.PowerCost,
                        _me.CurrentMana
                        );
                }
                //      else if (SpellManager.GlobalCooldown)
                //      Dlog("status:  GCD is active -- not cast");
                //      else if (spell.Cooldown)
                //      Dlog("status:  spell [{0}] on cooldown - not cast", spell.Name );
                //      else if (!enoughPower)
                //      Dlog("warning:  not enough mana/energy for spell - " + spell.Name);
                //      else if ( !SpellManager.CastableSpell( spell ))
                //      Dlog("status:  missing proc/reagent/totem to cast spell - " + spell.Name);
                //      else if (!SpellManager.CanCast( spell.Id ))                    // unclear what this spell does other than periodically
                //      Dlog("warning:  cannot cast spell {0} due to missing proc/reagent/totem", spell.Name  );       // .. block casting which otherwise can occur... disabled
                else
                {
                    double udist = -1;
                    double uhealth = -1;
                  
                    try
                    {
                        if (unit == null)
                            bCastSuccessful = SpellManager.Cast(spell);
                        else
                        {
                            udist = unit.Distance;
                            uhealth = unit.HealthPercent;
                            bCastSuccessful = SpellManager.Cast(spell, unit);
                        }
                    }

                    catch (ThreadAbortException) { throw; }
                    catch (Exception e)
                    {
                        Log(Color.Red, "An Exception occured. Check debug log for details.");
                        Logging.WriteDebug("HB EXCEPTION in spell.Cast([" + spell.Id + ":" + spell.Name + "])");
                        Logging.WriteException(e);
                        return false;
                    }

                    unitLastSpellTarget = unit;

                    if (!bCastSuccessful)
                        Dlog("Safe_CastSpell: cast of {0} failed", spell.Name);
                    else
                    {
                        string info = "";
                        System.Drawing.Color clr = spell.Mechanic == WoWSpellMechanic.Healing ?
                            Color.ForestGreen : Color.DodgerBlue;

                        // spell.Mechanic always equals None currently
                        if (unit != null) // && spell.Mechanic == WoWSpellMechanic.Healing)
                            info = string.Format(" on {0} at {1:F1} yds at {2:F1}%", Safe_UnitName(unit), udist, uhealth);

                        Log(clr, "*" + spell.Name + info);

                        StyxWoW.SleepForLagDuration();
                        if (chkWait == SpellWait.Complete)
                        {
                            WaitForCurrentSpell(null);
                        }
                    }
                }
            }

            return bCastSuccessful;
        }

        // if its a heal spell, will wait until completed (or cancelled)
        // .. otherwise just return if cast is in progress.  this function
        // .. should be used at CC entry points to prevent movement causing
        // .. an unintended spell cancellation/interruption
        public bool HandleCurrentSpellCast()
        {
            if (IsHealSpellToWatch(_me.CastingSpellId) && IsHealer() && hsm != null)
                WaitForCurrentHeal(unitLastSpellTarget, hsm.NeedHeal);

            return _me.IsCasting;
        }

        // identify heal spells which could have cast time.  we'll monitor these
        // .. to see if we already reached heal threshhold and should cancel
        // .. don't worry about instants since we can't cancel anyway
        public static bool IsHealSpellToWatch(int id)
        {
            switch (id)
            {
                case 331:       // Healing Wave
                case 1064:      // Chain Heal
                case 8004:      // Healing Surge
                case 77472:     // Greater Healing Wave
                    return true;
            }
            return false;
        }

        // waits for current spell in progress and if target reaches health %
        // .. will cancel.  assumes it will only be called when heal is being cast
        // DOES NOT WAIT ON INSTANTS!!!!  THIS IS INTENTIONAL AS OTHER TASKS
        // CAN BE COMPLETED AFTER AN ENTRY POINT WHILE GCD TICKS OFF RATHER
        // THAN WAITING HERE IN A DO NOTHING LOOP
        public static bool WaitForCurrentHeal(WoWUnit unit, int healValue)
        {
            if (_me.IsCasting) 
                Dlog("WaitForCurrentHeal:  spell cast in progress");

            while (!IsGameUnstable() && _me.IsAlive && _me.IsCasting)
            {
                // we hit target value, so cancel current cast
                if (unit == null)
                {
                    ;
                }
                else if (unit.HealthPercent >= healValue)
                {
                    // only cancel if we are casting
                    SpellManager.StopCasting();
                    Slog(Color.Orange, "/stopcasting - heal target reached {0:F1}%", unit.HealthPercent);

                    // return now because we know health is above threshhold, so no need to wait further
                    return false;
                }
                else if (IsHealer()) 
                {
                    unitToSaveHeal = null;      // unit we should throw a saving heal at the expense of current heal target

                    // check if we need to bail on this heal to save self or the tank
                    if (IsRAFandTANK() && GroupTank.HealthPercent < ConfigValues.EmergencySavingHealPct)
                        unitToSaveHeal = GroupTank;
                    else if (!unit.IsMe && _me.HealthPercent < ConfigValues.EmergencySavingHealPct)
                        unitToSaveHeal = _me;

                    if (unitToSaveHeal != null && unitToSaveHeal.IsAlive && unit.Guid != unitToSaveHeal.Guid && _me.IsCasting)
                    {
                        SpellManager.StopCasting();
                        Slog(Color.Orange, "/stopcasting - switch to {0} who is dangerously low @ {1:F1}%", Safe_UnitName(unitToSaveHeal), unitToSaveHeal.HealthPercent);
                        return false;
                    }
                }

                // pause briefly
                Thread.Sleep(10);
            }


            if (IsGameUnstable())
                Dlog("WaitForCurrentSpell:  game appears to be Unstable");
            else if (_me.IsCasting)
                Dlog("WaitForCurrentHeal:  done waiting but i am still casting???");

            return true;
        }

        public static void WaitForCurrentCast()
        {
            if (!_me.IsCasting)
            {
                Dlog("WaitForCurrentCast:  no cast in progress");
            }
            else
            {
                Dlog("WaitForCurrentCast:  waiting until cast is complete");
                while (!IsGameUnstable() && _me.IsAlive && _me.IsCasting)
                {
                    // give a small time slice back
                    Thread.Sleep(10);
                }
            }
        }

        public static void WaitForCurrentSpell(WoWSpell spell)
        {
            if (!IsCasting() )
            {
                Dlog("WaitForCurrentSpell:  no cast or gcd in progress");
            }
            else
            {
                Dlog("WaitForCurrentSpell:  waiting until gcd and/or cast are complete");
                while (!IsGameUnstable() && _me.IsAlive && IsCasting())
                {
                    // give a small time slice back
                    Thread.Sleep(10);
                }
            }
        }


        private static Dictionary<int, bool> _dictIntSpell = new Dictionary<int, bool>();

        public bool IsTargetCastInterruptible(WoWUnit target)
        {
            if (target == null || !target.IsCasting)
                return false;

            int spellId = target.CastingSpell.Id;
            string spellName = target.CastingSpell.Name;

            if (_dictIntSpell.ContainsKey(spellId))
                return _dictIntSpell[spellId];

            int idxList = 8;
            List<string> list = Lua.GetReturnValues("return UnitCastingInfo(\"target\")");
            if (list == null)
            {
                idxList = 7;
                list = Lua.GetReturnValues("return UnitChannelInfo(\"target\")");
            }

            if (list == null)
            {
                Dlog("SpellInterrupt:  null return from casting information check for {0} #{1}", spellName, spellId);
                return false;
            }
#if LIST_SPELL_INFO
            Dlog("SpellInterrupt:  list[{0}]={1}", list.Count, list);
            for (int i = 0; i < list.Count; i++)
                Dlog("  list[{0}]='{1}'", i, list[i]);
#endif
            bool canInterrupt = string.IsNullOrEmpty(list[idxList]);
            _dictIntSpell.Add(spellId, canInterrupt);
            Dlog("SpellInterrupt:  added canInterrupt:{0} for spell:{1}(#{2})", canInterrupt, spellName, spellId);
            return canInterrupt;
        }
    }
}
