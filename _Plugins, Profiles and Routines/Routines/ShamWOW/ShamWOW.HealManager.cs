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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Bobby53
{
    partial class Shaman
    {
        static WoWUnit unitToSaveHeal = null;       // key unit in trouble that needs immediate heal (ignore normal heal priority)

        private bool HealRaid()
        {

            bool wasSpellCast = false;
            int healPct = hsm.NeedHeal;

            // following line is here solely to make sure we wait on the GCD since
            // ... prior checks may pause only if we were casting.  This prevents 
            // ... misleading results from WoWSpell.Cooldown later in .CastHeal
            WaitForCurrentSpell(null);

            WoWPlayer p = ChooseHealTarget(healPct, SpellRange.NoCheck);
            if (p != null)
                wasSpellCast = hsm.CastHeal(p);

            // check and earth shield tank if needed
            if (IsRAFandTANK() && GroupTank.IsAlive && typeShaman == ShamanType.Resto)
            {
                bool inCombat = _me.Combat || GroupTank.Combat;
                if (GetAuraStackCount(GroupTank, "Earth Shield") < (inCombat ? 1 : 3))
                {
                    MoveToHealTarget(GroupTank, 35);
                    if (IsUnitInRange(GroupTank, 39))
                        wasSpellCast = wasSpellCast || Safe_CastSpell(GroupTank, "Earth Shield", SpellRange.Check, SpellWait.NoWait);
                }
            }

            return wasSpellCast;
        }

        private bool DispelRaid()
        {
            bool WasHealCast = false;

            bool knowCleanseSpirit = SpellManager.HasSpell("Cleanse Spirit");
            bool canCleanCurse = knowCleanseSpirit;
            bool canCleanMagic = knowCleanseSpirit && _hasTalentImprovedCleanseSpirit;
            bool knowStoneform = SpellManager.HasSpell("Stoneform");

            WoWPlayer player = (from p in GroupMembers 
                                where
                                    p.Distance <= 38
                                    && !Blacklist.Contains(p)
                                    && (from dbf in p.Debuffs
                                        where
                                            (dbf.Value.Spell.DispelType == WoWDispelType.Curse && canCleanCurse)
                                            || (dbf.Value.Spell.DispelType == WoWDispelType.Magic && canCleanMagic)
                                            || (p.IsMe &&
                                                    (
                                                        (dbf.Value.Spell.DispelType == WoWDispelType.Magic && _hasGlyphOfShamanisticRage)
                                                     || (dbf.Value.Spell.DispelType == WoWDispelType.Poison && knowStoneform)
                                                     || (dbf.Value.Spell.DispelType == WoWDispelType.Disease && knowStoneform)
                                                    )
                                               )
                                        select dbf.Value
                                        ).Any()
                                    && !(from dbf in p.Debuffs
                                        where _hashCleanseBlacklist.Contains( dbf.Value.SpellId )
                                        select dbf.Value
                                        ).Any()
                                select p
                ).FirstOrDefault();

            if (player != null)
            {
                WasHealCast = CleanseIfNeeded(player);
            }

            return WasHealCast;
        }

        private bool HealMyself()
        {
            return HealMyself(GetSelfHealThreshhold());
        }

        private bool HealMyself(int threshhold)
        {
            if (_me.HealthPercent >= threshhold)
                return false;

            Log("^Heal Target: {0}[{1}] at {2:F1}%", Safe_UnitName(_me), _me.Level, _me.HealthPercent);

            #region COMBAT_HEAL_SPECIALS
            // non-combat heal... do what we can to try and top-off
            if (_me.Combat)
            {
                if ( _me.HealthPercent < cfg.EmergencyHealthPercent || IsFightStressful())
                    Warstomp();         

                if (_me.ManaPercent <= cfg.EmergencyManaPercent && _me.HealthPercent > cfg.NeedHealHealthPercent)
                    UseManaPotionIfAvailable();

                if (_hasGlyphOfStoneClaw && _me.HealthPercent < cfg.NeedHealHealthPercent)
                {
                    if (!TotemExist(TotemId.EARTH_ELEMENTAL_TOTEM) && !TotemExist(TotemId.STONECLAW_TOTEM))
                    {
                        if (TotemCast(TotemId.STONECLAW_TOTEM))
                        {
                            Log("^Shaman Bubble: casting Stoneclaw Totem w/ Glyph");
                        }
                    }
                }

                if (_me.HealthPercent < cfg.EmergencyHealthPercent)
                {
                    UseHealthPotionIfAvailable();
                }

                if (_me.HealthPercent >= threshhold)
                    return true;
            }
            #endregion

            double checkHealth = _me.HealthPercent;
            double checkMana = _me.ManaPercent;
            bool WasHealCast = false;

            if (!_me.IsAlive)
            {
                Dlog("HealPlayer: I am dead, too late for heals now...");
                return WasHealCast;
            }

            Safe_StopMoving();

            // wait for any current spell cast... this is overly cautious...
            // if in danger of dying, fastest heal possible
            if (_me.Combat && _me.HealthPercent < cfg.EmergencyHealthPercent)
            {
                if (SpellManager.HasSpell("Tidal Force"))
                    Safe_CastSpell("Tidal Force", SpellRange.NoCheck, SpellWait.Complete);

                if (SpellManager.HasSpell("Nature's Swiftness"))
                {
                    if (!Safe_CastSpell("Nature's Swiftness", SpellRange.NoCheck, SpellWait.Complete))
                        Dlog("Nature's Swiftness on cooldown, cannot Oh S@#$ heal");
                    else
                    {
                        if (!WasHealCast && SpellManager.HasSpell("Greater Healing Wave"))
                            WasHealCast = Safe_CastSpell(_me, "Greater Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                        if (!WasHealCast && SpellManager.HasSpell("Healing Surge"))
                            WasHealCast = Safe_CastSpell(_me, "Healing Surge", SpellRange.NoCheck, SpellWait.NoWait);
                        if (!WasHealCast && SpellManager.HasSpell("Healing Wave"))
                            WasHealCast = Safe_CastSpell(_me, "Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);

                        if (WasHealCast)
                            Slog("Big Heals - clicked the Oh S@#$ button!");
                        else
                            Slog("Attempted Oh S@#$ heal but couldn't cast Healing Wave");
                    }
                }
            }

            if (!_me.Combat)
            {
                if (!WasHealCast && SpellManager.HasSpell("Greater Healing Wave"))
                    WasHealCast = Safe_CastSpell(_me, "Greater Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                if (!WasHealCast && SpellManager.HasSpell("Healing Wave") && _hasTalentMaelstromWeapon && IsAuraPresent(_me, "Maelstrom Weapon"))
                    WasHealCast = Safe_CastSpell(_me, "Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                if (!WasHealCast && SpellManager.HasSpell("Healing Surge"))
                    WasHealCast = Safe_CastSpell(_me, "Healing Surge", SpellRange.NoCheck, SpellWait.NoWait);
            }
            else
            {
                if (!WasHealCast && SpellManager.HasSpell("Riptide"))
                    WasHealCast = Safe_CastSpell(_me, "Riptide", SpellRange.NoCheck, SpellWait.NoWait);
                if (_hasTalentMaelstromWeapon && 3 <= GetAuraStackCount(_me, "Maelstrom Weapon"))
                {
                    if (!WasHealCast && SpellManager.HasSpell("Greater Healing Wave"))
                    {
                        Dlog("HealMyself:  GHW selected because of Maelstrom Weapon");
                        WasHealCast = Safe_CastSpell(_me, "Greater Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                    }
                    if (!WasHealCast && SpellManager.HasSpell("Healing Wave") )
                    {
                        Dlog("HealMyself:  HW selected because of Maelstrom Weapon");
                        WasHealCast = Safe_CastSpell(_me, "Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                    }
                }
                if (!WasHealCast && SpellManager.HasSpell("Greater Healing Wave") && IsAuraPresent(_me, "Tidal Waves"))
                {
                    Dlog("HealMyself:  GHW selected because of Tidal Waves");
                    WasHealCast = Safe_CastSpell(_me, "Greater Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                }
                if (!WasHealCast && SpellManager.HasSpell("Healing Surge") && (_me.CurrentHealth < 40 || !SpellManager.HasSpell("Greater Healing Wave")))
                {
                    Dlog("HealMyself:  HS selected because of low-health or GHW not trained");
                    WasHealCast = Safe_CastSpell(_me, "Healing Surge", SpellRange.NoCheck, SpellWait.NoWait);
                }
                if (!WasHealCast && SpellManager.HasSpell("Greater Healing Wave"))
                {
                    Dlog("HealMyself:  GHW selected by default");
                    WasHealCast = Safe_CastSpell(_me, "Greater Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                }
            }

            if (!WasHealCast)
            {
                if (!SpellManager.HasSpell("Healing Wave"))
                    Slog("No healing spells trained, you need potions or first-aid to heal", checkHealth, checkMana);
                else
                {
                    WasHealCast = Safe_CastSpell(_me, "Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);

                    // at this point no healing worked, so issue a heal failed message
                    if (!WasHealCast)
                        Slog("Casting of heal prevented: Health={0:F0}% Mana={1:F0}%", checkHealth, checkMana);
                }
            }

            if (WasHealCast)
            {
                Dlog("^Heal begun @ health:{0:F2}% mana:{1:F2}%", checkHealth, checkMana);
                WaitForCurrentHeal(_me, threshhold);
            }

            return WasHealCast;
        }

        private static bool WillChainHealHop(WoWUnit healTarget)
        {
            Stopwatch timer = new Stopwatch();
            double threshhold = hsm.NeedHeal;
            timer.Start();

            if (healTarget == null)
                return false;

#if CHAIN_HEAL_LOOKS_FOR_ONE
            WoWPlayer player = null;
            try
            {
                player = ObjectManager.GetObjectsOfType<WoWPlayer>(false).Find(
                            p => p != null
                                && p.IsPlayer && !p.IsPet
                                && p != healTarget
                                && p.IsAlive
                                && p.HealthPercent < threshhold
                                && healTarget.Location.Distance(p.Location) <= 12
                            );
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug("HB EXCEPTION in WillChainHealHop()");
                Logging.WriteException(e);
            }

            if ( player == null)
                Dlog("WillChainHealHop(): took {0} ms and found no players in range", timer.ElapsedMilliseconds);
            else
                Dlog("WillChainHealHop(): took {0} ms and found {0} at {1:F1}% and {2:F1} yds away", timer.ElapsedMilliseconds, Safe_UnitName(player), player.HealthPercent, player.Distance );

            return player != null;
#else
            var t =(from o in ObjectManager.ObjectList
                    where o is WoWPlayer && healTarget.Location.Distance(o.Location) < 12
                        let p = o.ToPlayer()
                    where p != null
                        && p.IsHorde == _me.IsHorde
                        && !p.IsPet
                        && p != healTarget
                        && p.IsAlive
                        && p.HealthPercent < threshhold
                    let c =(from oo in ObjectManager.ObjectList
                            where oo is WoWPlayer && p.Location.Distance(oo.Location) < 12
                                let pp = oo.ToPlayer()
                            where pp != null
                                && pp.IsHorde == p.IsHorde
                                && !pp.IsPet
                                && pp.IsAlive
                                && pp.HealthPercent < threshhold
                            select pp).Count()
                    orderby c descending, p.Distance ascending 
                    select new {Player = p, Count = c}).FirstOrDefault();

            if (t == null)
            {
                Dlog("WillChainHealHop: found no hops in range (took={0} ms)", timer.ElapsedMilliseconds);
                return false;
            }

            Dlog("WillChainHealHop:  found {0} near target {1} providing {2} hop targets (took={3} ms)", Safe_UnitName(t.Player), Safe_UnitName(healTarget), t.Count, timer.ElapsedMilliseconds);
            if (t.Count < 3)
                return false;

            Slog("^Chain Heal should heal {0} or more", t.Count);
            return true;
#endif
        }


        private static bool WillHealingRainCover(WoWUnit healTarget, int minCount)
        {
            List<WoWPlayer> targets = null;
            Stopwatch timer = new Stopwatch();
            double threshhold = hsm.NeedHeal;
            timer.Start();

            if (healTarget == null)
                return false;

            try
            {
                targets = (from p in GroupMembers
                           where p.IsPlayer
                                && p.IsAlive
                                && p.HealthPercent < threshhold
                                && healTarget.Location.Distance(p.Location) <= 10
                           select p
                                ).ToList();
            }
            catch (ThreadAbortException) { throw; }
            catch (Exception e)
            {
                Log(Color.Red, "An Exception occured. Check debug log for details.");
                Logging.WriteDebug("HB EXCEPTION in WillHealingRainCover()");
                Logging.WriteException(e);
            }

            Dlog("WillHealingRainCover({0}): took {1} ms", minCount, timer.ElapsedMilliseconds);
            return targets != null && targets.Count >= minCount;
        }


        public static List<WoWPlayer> _healTargets;
        private static readonly Countdown _refreshTimer = new Countdown();
        private static readonly Countdown _dumpTimer = new Countdown(10000);

        /// <summary>
        /// Chooses a nearby target for healing.  Selection is based upon
        /// which nearby friendly needs heals.  Includes _me in list so
        /// will handle self-healing to you low
        /// </summary>
        /// <returns>WOWPlayer reference of nearby player needing heals</returns>
        public static WoWPlayer ChooseHealTarget(double healLessThan, SpellRange rchk)
        {
#if COMMENT
            // use timer to ensure we aren't constantly rebuilding the list
            // .. player health and distance are very dynamic, but the number of players
            // .. in the vicinity won't change drastically in that time
            //--- NOTE:  timer is initially zero so first call builds list
            if (_healTargets == null || _refreshTimer.Done )
            {
                CreateHealTargetList();
            }

            _healTargets.Sort(CompareHealPriority);

#if LIST_HEAL_TARGETS
			for (int b = 0; b < _healTargets.Count; b++)
			{
				Slog("  {0:F0}% {1}[{2}] dist: {3:F1} in-los: {4}", _healTargets[b].HealthPercent, _healTargets[b].Name, _healTargets[b].Level, _healTargets[b].Distance, _healTargets[b].InLineOfSight );
			}
			Slog("  Total of " + _healTargets.Count);
#endif

            // restrict to 39 if movement has been disabled (so we only heal those within range of users movement)
            double searchFilter = IsMovementDisabled() ? 39 : cfg.GroupHeal.SearchRange;

            WoWPlayer lowPlayer = null;

            Dlog("ChooseHealTarget: checking {0} players within {1} yards", _healTargets.Count(), searchFilter );

            // Me and Tank okay, so choose lowest health player
            for (int a = 0; a < _healTargets.Count; a++)
            {
                try
                {
                    if (!Safe_IsValid(_healTargets[a]))
                    {
                        Dlog("ChooseHealTarget: entry[{0}] failed Safe_IsValid()", a);
                        continue;
                    }
                    if (!ObjectManager.ObjectList.Contains(_healTargets[a]))
                    {
                        Dlog("ChooseHealTarget: entry[{0}] failed Is In ObjectList()", a);
                        continue;
                    }

                    // stop looking in sorted list if we reach healing threshhold
                    if (_healTargets[a].HealthPercent > healLessThan)
                    {
                        Dlog("ChooseHealTarget:  no player currently below {0}%", healLessThan);
                        break;
                    }

                    // if target is out of range, then skip this entry
                    if (_healTargets[a].Distance > searchFilter )
                        continue;

                    // since we don't rebuild the list each time, always need to retest for dead players
                    if (_healTargets[a].CurrentHealth <= 1) // _healTargets[a].Dead || _healTargets[a].IsGhost
                    {
                        Dlog("ChooseHealTarget:  entry[{0}] is dead", a);
                    }
                    else if (rchk == SpellRange.NoCheck || (_healTargets[a].Distance < 39 && _healTargets[a].InLineOfSightOCD))
                    {
                        Dlog("ChooseHealTarget: {0}[{1}] at {2:F0}% dist: {3:F1} in-los: {4}", Safe_UnitName(_healTargets[a]), _healTargets[a].Level, _healTargets[a].HealthPercent, _healTargets[a].Distance, _healTargets[a].InLineOfSightOCD);
                        lowPlayer = _healTargets[a];
                        break;
                    }
                }
                catch (ThreadAbortException) { throw; }
                catch
                {
                    // if exception dealing with this WoWUnit, then try next in array
                    Dlog("ChooseHealTarget:  exception occurred accessing entry[{0}]", a);
                }
            }

            minGroupHealth = (int) (lowPlayer == null ? 100 : lowPlayer.HealthPercent);

            // if Me or the Tank (value in unitToSaveHeal) is at risk 
            if (unitToSaveHeal != null && unitToSaveHeal.IsAlive)
            {
                if (rchk == SpellRange.NoCheck || (unitToSaveHeal.Distance < 38 && unitToSaveHeal.InLineOfSightOCD))
                {
                    Dlog("ChooseHealTarget: SAVING {0}[{1}] at {2:F0}% dist: {3:F1} in-los: {4}", Safe_UnitName(unitToSaveHeal), unitToSaveHeal.Level, unitToSaveHeal.HealthPercent, unitToSaveHeal.Distance, unitToSaveHeal.InLineOfSightOCD);
                    lowPlayer = unitToSaveHeal.ToPlayer();
                    unitToSaveHeal = null;
                }
            }

            return lowPlayer;
#else
            int searchRange = IsMovementDisabled() ? 39 : (int) cfg.GroupHeal.SearchRange;
            WoWPlayer lowPlayer = null;

            // heal Me or Tank if at risk 
            try
            {
                if (unitToSaveHeal != null && unitToSaveHeal.CurrentHealth > 1)
                {
                    if (rchk == SpellRange.NoCheck || (unitToSaveHeal.Distance < 39 && unitToSaveHeal.InLineOfSightOCD))
                    {
                        Dlog("ChooseHealTarget: SAVING {0}[{1}] at {2:F0}% dist: {3:F1} in-los: {4}", Safe_UnitName(unitToSaveHeal), unitToSaveHeal.Level, unitToSaveHeal.HealthPercent, unitToSaveHeal.Distance, unitToSaveHeal.InLineOfSightOCD);
                        lowPlayer = unitToSaveHeal.ToPlayer();
                        unitToSaveHeal = null;
                    }
                }
            }
            catch
            {
                // tank or healer needs saving but reference is invalid at the moment
                unitToSaveHeal = null;
            }

            // if no low player found yet, find if there is one
            if (lowPlayer == null)
            {
                try
                {
                    lowPlayer = 
                        GroupMembers
                            .Where(u => CheckValidAndNearby(u, searchRange) && u.HealthPercent <= healLessThan)
                            .OrderBy(u => u.HealthPercent)
                            .FirstOrDefault();
                }
                catch (Exception e)
                {
                    Log(Color.Red, "An Exception occured. Check debug log for details.");
                    Logging.WriteDebug("HB EXCEPTION in ChooseHealTarget()");
                    Logging.WriteException(e);
                    lowPlayer = null;
                }
            }

            // now account for Me not being in GroupMembers
            if (lowPlayer == null)
                lowPlayer = _me.HealthPercent <= healLessThan ? _me : null;
            else if (_me.HealthPercent <= lowPlayer.HealthPercent)
                lowPlayer = _me;

            minGroupHealth = (int)(lowPlayer == null ? 100 : lowPlayer.HealthPercent);
            return lowPlayer;
#endif
        }

        public static WoWPlayer ChooseNextHealTarget(WoWUnit currHealTarget, double healLessThan)
        {
#if COMMENT
            for (int a = 0; a < _healTargets.Count; a++)
            {
                try
                {
                    if (!Safe_IsValid(_healTargets[a]))
                        continue;

                    if (_healTargets[a].HealthPercent > healLessThan)
                        break;

                    if (_healTargets[a].CurrentHealth <= 1)         // skip if Dead
                        ;
                    else if (_healTargets[a] == currHealTarget)     // skip if current healtarget
                        ;
                    else if (IsUnitInRange(_healTargets[a], 37))   // choose one alread in range
                    {
                        // Slog("Heal Target: {0}[{1}] at {2:F0}% dist: {3:F1} in-los: {4}", _healTargets[a].Name, _healTargets[a].Level, _healTargets[a].HealthPercent, _healTargets[a].Distance, _healTargets[a].InLineOfSight);
                        return _healTargets[a];
                    }
                }
                catch (ThreadAbortException) { throw; }
                catch
                {
                    // if exception dealing with this WoWUnit, then try next in array
                    continue;
                }
            }

            return null;
#else
            WoWPlayer nextLowest = 
                GroupMembers
                    .Where(u => CheckValidAndNearby(u, 36) && u.HealthPercent <= healLessThan && u != currHealTarget)
                    .OrderBy(u => u.HealthPercent)
                    .FirstOrDefault();

            // now account for Me not being in GroupMembers
            if (_me != currHealTarget)
            {
                if (nextLowest == null)
                    nextLowest = _me.HealthPercent <= healLessThan ? _me : null;
                else if (_me.HealthPercent <= nextLowest.HealthPercent)
                    nextLowest = _me;
            }

            return nextLowest;
#endif
        }

        private static bool CheckValidAndNearby(WoWUnit u, int dist)
        {
            try
            {
                return Safe_IsValid( u) && u.CurrentHealth > 1 && u.Distance <= dist;
            }
            catch
            {
                return false;
            }
        }

        // sort in ascending order by Health percent
        //  ..  null pointers or dead's should be at end of list
        private static int CompareHealPriority(WoWUnit x, WoWUnit y)
        {
            try
            {
                // handle nulls/deads so that they fall to end of list
                if (x == null || !x.IsAlive)
                    return (y == null || !y.IsAlive ? 0 : 1);
                else if (y == null || !y.IsAlive)
                    return -1;

                // sort 
                double healthDiff = x.HealthPercent - y.HealthPercent;

                if (healthDiff < 0.0)
                    return -1;

                if (healthDiff > 0.0)
                    return 1;
            }
            catch (ThreadAbortException) { throw; }
            catch
            {
                Dlog("CompareHealPriority: EXCEPTION: a heal target left group or released -- ignoring");
            }

            return 0;

            /*
                * -- Eventually determine a priority based upon general health, 
                * -- targets survivability, and targets savability (my word).
                * -- this would factor in can they be saved, are they a plater 
                * -- wearer, do they have a self-heal and mana, etc.
                * 
            const double _priorityTiers = 5;

            int xHealthPriority = (int)Math.Ceiling(x.HealthPercent / _priorityTiers);
            int yHealthPriority = (int)Math.Ceiling(y.HealthPercent / _priorityTiers);

            return xHealthPriority - yHealthPriority;
            */
        }
#if COMMENT
        public static void CreateHealTargetList()
        {
            List<WoWPlayer> plist = GroupMembers;
            if (!plist.Contains(ObjectManager.Me))
            {
                plist.Add(ObjectManager.Me);
                Dlog("CreateHealTargetList:  added Me to list");
            }

            double searchDist = cfg.GroupHeal.SearchRange;
            _healTargets = plist.FindAll(
                unit => unit.CurrentHealth > 1
                // && unit.IsAlive 
                // && unit.Guid != ObjectManager.Me.Guid
                );

            // _refreshTimer.Remaining = IsPVP() ? 5000 : 20000;
            _refreshTimer.Remaining = 5000;
        }
#endif
        private static void DumpHealTargetList()
        {
            int a = 1;
            foreach (WoWPlayer pd in GroupMembers)
            {
                Dlog("     HealTarget[{0}]={1}[{2}] Health={3}% @ {4:F1} yds",
                    a++,
                    Safe_UnitName(pd),
                    pd.Level,
                    pd.HealthPercent,
                    pd.Distance
                    );
            }
        }

        class HealSpellManager
        {
            public int NeedHeal = 0;
            bool BuffTidalWaves = false;
            bool HealPets = false;

            WoWUnit priorHealTarget = null;
            int priorHealTick = 0;

            private List<HealSpell> spells;

            public HealSpellManager()
            {
                spells = new List<HealSpell>();

                int Heal_HW = 0;
                int Heal_GHW = 0;
                int Heal_RT = 0;
                int Heal_UE = 0;
                int Heal_CH = 0;
                int Heal_HR = 0;
                int Heal_HS = 0;
                int Heal_OS = 0;

                if (IsRAF())
                {
                    Heal_HW = cfg.RAF_Heal.HealingWave;
                    Heal_GHW = cfg.RAF_Heal.GreaterHealingWave;
                    Heal_RT = cfg.RAF_Heal.Riptide;
                    Heal_UE = cfg.RAF_Heal.UnleashElements;
                    Heal_CH = cfg.RAF_Heal.ChainHeal;
                    Heal_HR = cfg.RAF_Heal.HealingRain;
                    Heal_HS = cfg.RAF_Heal.HealingSurge;
                    Heal_OS = cfg.RAF_Heal.OhShoot;
                    BuffTidalWaves = cfg.RAF_Heal.TidalWaves;
                    HealPets = cfg.RAF_Heal.Pets;
                }
                else if (IsPVP())
                {
                    Heal_HW = cfg.PVP_Heal.HealingWave;
                    Heal_GHW = cfg.PVP_Heal.GreaterHealingWave;
                    Heal_RT = cfg.PVP_Heal.Riptide;
                    Heal_UE = cfg.PVP_Heal.UnleashElements;
                    Heal_CH = cfg.PVP_Heal.ChainHeal;
                    Heal_HR = cfg.PVP_Heal.HealingRain;
                    Heal_HS = cfg.PVP_Heal.HealingSurge;
                    Heal_OS = cfg.PVP_Heal.OhShoot;
                    BuffTidalWaves = cfg.PVP_Heal.TidalWaves;
                    HealPets = cfg.PVP_Heal.Pets;
                }

                spells.Add(new HealSpell(Heal_HW, "Healing Wave", "Healing Wave", HealSpellManager.HealingWave));
                spells.Add(new HealSpell(Heal_GHW, "Greater Healing Wave", "Greater Healing Wave", HealSpellManager.GreaterHealingWave));
                spells.Add(new HealSpell(Heal_RT, "Riptide", "Riptide", HealSpellManager.Riptide));
                spells.Add(new HealSpell(Heal_UE, "Unleash Elements", "Unleash Elements", HealSpellManager.UnleashElements));
                spells.Add(new HealSpell(Heal_CH, "Chain Heal", "Chain Heal", HealSpellManager.ChainHeal));
                spells.Add(new HealSpell(Heal_HR, "Healing Rain", "Healing Rain", HealSpellManager.HealingRain));
                spells.Add(new HealSpell(Heal_HS, "Healing Surge", "Healing Surge", HealSpellManager.HealingSurge));
                spells.Add(new HealSpell(Heal_OS, "Oh Shoot Heal", "Nature's Swiftness", HealSpellManager.OhShoot));

                spells.Sort(HealSpellManager.Compare);
                Dlog("### HealSpellManager Dump BEFORE PRUNE");
                Dump();

                // remove those with 0 health values
                while (spells.Any() && spells[0].Health == 0)
                    spells.RemoveAt(0);

                Dlog("### HealSpellManager Dump AFTER PRUNE");
                Dump();

                // find maximum to use as NeedHeal value
                HealSpell hs = spells.LastOrDefault();
                if (hs != null)
                {
                    NeedHeal = hs.Health;
                    Dlog("HealSpellManager:  NeedHeal set to {0}", NeedHeal);
                }
                else
                {
                    if (_me.IsInParty || _me.IsInRaid)
                        Elog("HealSpellManager:  NOT CURRENTLY IN A GROUP, group healing will activate when you join");
                    NeedHeal = 0;
                }
            }

            public bool CastHeal(WoWUnit unit)
            {
                if (!unit.IsAlive)
                {
                    Dlog("CastHeal: Heal target is dead");
                    return false;
                }

                if (!unit.IsMe && !IsUnitInRange(unit, 39))
                {
                    Dlog("CastHeal:  moving to heal target who is {0} yds away", unit.Distance);
                    MoveToHealTarget(unit, 35);
                    if (!IsUnitInRange(unit, 39))
                    {
                        Dlog("CastHeal:  not within healing range, Heal Target {0} is {1:F1} yds away", Safe_UnitName(unit), unit.Distance);
                        return false;
                    }

                    Dlog("CastHeal:  stopping now that Heal Target {0} is {1:F1} yds away", Safe_UnitName(unit), unit.Distance);
                }

                if (BuffTidalWaves && SpellManager.HasSpell("Riptide"))
                {
                    WoWUnit tank = GroupTank;
                    if (IsRAF() && tank != null && tank.HealthPercent < NeedHeal && !tank.Auras.ContainsKey("Riptide"))
                    {
                        if (Riptide(tank))
                            return true;
                    }

                    if (Riptide(unit))
                    {
                        return true;
                    }
                }

                Safe_StopMoving();

                // wait here to try and cut down on Safe_SpellCast() having to wait which provides sqewed log output
                // .. indicating we cast a higher power heal (HealingSurge) while unit had higher health %

                double currHealth = unit.HealthPercent;
                HealSpell spell = spells.FirstOrDefault(s => currHealth < s.Health);
                HealSpell baseSpell = spell;

                // FIND FIRST CHOICE SPELL
                // nav up: find the correct spell in the list or next higher health % spell
                while (!IsGameUnstable() && _me.IsAlive && spell != null)
                {
                    Dlog("CastHeal:  {0} {1}% for unit with health {2:F1}%", spell.DisplayName, spell.Health, currHealth);

                    WoWSpell wowSpell = SpellManager.Spells[spell.TestSpell];
                    if (wowSpell.Cooldown)
                        Dlog("CastHeal: spell '{0}' is on cooldown", wowSpell.Name);
                    else if (spell.Cast(unit))
                    {
                        if ( !_me.Combat && !unit.Combat)
                            WaitForCurrentHeal(unit, NeedHeal);
                        return true;
                    }

                    // on CD, so move to next higher health % in list
                    spell = spells.FirstOrDefault(s => spell.Health < s.Health);
                }

#if ALLOW_NEXT_LOWER_HEALTH_SPELL
                // FIND SECOND CHOICE SPELL
                // nav dn: find next lower health % spell
                if ( baseSpell != null )
                    spell = spells.LastOrDefault( s => baseSpell.Health > s.Health );

                while ( !IsGameUnstable() && _me.IsAlive && spell != null )
                {
                    Dlog("CastHeal:  2nd choice {0} {1}% for unit with health {2:F1}%", spell.DisplayName, spell.Health, currHealth);
                    if (spell.Cast(unit))
                    {
                        WaitForCurrentHeal(unit, NeedHeal );
                        return true;
                    }

                    // on CD, so find next lower health % in list
                    spell = spells.LastOrDefault( s => spell.Health > s.Health );
                }
#endif
                Dlog("CastHeal:  failed to find usable healing spell for unit with health {0:F1}%", currHealth);
                return false;
            }

            public void UpdatePriorHealInfo(WoWUnit unit)
            {
                priorHealTick = System.Environment.TickCount;
                priorHealTarget = unit;
            }

            public void Dump()
            {
                Logging.WriteDebug("");
                Logging.WriteDebug("   % Spell Description");
                Logging.WriteDebug(" --- --------------------------------");
                foreach (HealSpell spell in spells)
                {
                    Logging.WriteDebug(" {0,3} {1}", spell.Health, spell.DisplayName);
                }
                Logging.WriteDebug("");
            }

            private static int Compare(HealSpell a, HealSpell b)
            {
                return a.Health - b.Health;
            }

            private static bool HealingWave(WoWUnit healTarget)
            {
                if (!SpellManager.HasSpell("Healing Wave"))
                    return false;
                return Safe_CastSpell(healTarget, "Healing Wave", SpellRange.Check, SpellWait.NoWait);
            }
            private static bool GreaterHealingWave(WoWUnit healTarget)
            {
                if (!SpellManager.HasSpell("Greater Healing Wave"))
                    return false;
                return Safe_CastSpell(healTarget, "Greater Healing Wave", SpellRange.Check, SpellWait.NoWait);
            }
            private static bool ChainHeal(WoWUnit healTarget)
            {
                bool castRip = false;

                if (!SpellManager.HasSpell("Chain Heal"))
                    return false;
                if (!WillChainHealHop(healTarget))
                    return false;
                if (SpellManager.HasSpell("Riptide") && !healTarget.Auras.ContainsKey("Riptide") && SpellManager.CanCast("Riptide"))
                {
                    Dlog("ChainHeal:  prepping target with Riptide");
                    castRip = Riptide(healTarget);
                }
                return castRip || Safe_CastSpell(healTarget, "Chain Heal", SpellRange.Check, SpellWait.NoWait);
            }

            private static bool HealingRain(WoWUnit healTarget)
            {
                if (!SpellManager.HasSpell("Healing Rain"))
                    return false;

                if (_me.ManaPercent < cfg.EmergencyManaPercent && !_me.Auras.ContainsKey("Clearcasting"))
                    return false;

                if (!WillHealingRainCover(healTarget, 4))
                    return false;

                if (Safe_CastSpell("Healing Rain", SpellRange.Check, SpellWait.Complete))
                {
                    if (!LegacySpellManager.ClickRemoteLocation(healTarget.Location))
                    {
                        Dlog("^Ranged AoE Click FAILED:  cancelling Healing Rain");
                        SpellManager.StopCasting();
                    }
                    else
                    {
                        Dlog("^Ranged AoE Click successful:  LET IT RAIN!!!");
                        StyxWoW.SleepForLagDuration();
                        return true;
                    }
                }

                return false;
            }

            private static bool Riptide(WoWUnit healTarget)
            {
                if (!SpellManager.HasSpell("Riptide"))
                    return false;
                return Safe_CastSpell(healTarget, "Riptide", SpellRange.Check, SpellWait.NoWait);
            }
            private static bool UnleashElements(WoWUnit healTarget)
            {
                if (!SpellManager.HasSpell("Unleash Elements"))
                    return false;
                if (!_me.HasAura("Earthliving Weapon (Passive)"))
                    return false;
                return Safe_CastSpell(healTarget, "Unleash Elements", SpellRange.Check, SpellWait.NoWait);
            }
            private static bool HealingSurge(WoWUnit healTarget)
            {
                if (!SpellManager.HasSpell("Healing Surge"))
                    return false;
                return Safe_CastSpell(healTarget, "Healing Surge", SpellRange.Check, SpellWait.NoWait);
            }
            private static bool OhShoot(WoWUnit healTarget)
            {
                bool WasHealCast = false;

                if (!healTarget.Combat)
                {
                    Dlog("OhShootHeal:  target {0} is not in combat, saving cd", Safe_UnitName(healTarget));
                    return false;
                }

                if (SpellManager.HasSpell("Tidal Force"))
                    Safe_CastSpell("Tidal Force", SpellRange.NoCheck, SpellWait.NoWait);

                if (!SpellManager.HasSpell("Nature's Swiftness"))
                    return false;

                if (!Safe_CastSpell("Nature's Swiftness", SpellRange.NoCheck, SpellWait.NoWait))
                    Dlog(" Attempted Oh S@#$ heal but Nature's Swiftness not available");
                else
                {
                    if (!WasHealCast && SpellManager.HasSpell("Greater Healing Wave"))
                        WasHealCast = Safe_CastSpell(healTarget, "Greater Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);
                    if (!WasHealCast && SpellManager.HasSpell("Healing Surge"))
                        WasHealCast = Safe_CastSpell(healTarget, "Healing Surge", SpellRange.NoCheck, SpellWait.NoWait);
                    if (!WasHealCast && SpellManager.HasSpell("Healing Wave"))
                        WasHealCast = Safe_CastSpell(healTarget, "Healing Wave", SpellRange.NoCheck, SpellWait.NoWait);

                    if (WasHealCast)
                        Slog("Big Heals - clicked the Oh S@#$ button!");
                    else
                        Slog("Attempted Oh S@#$ heal but couldn't cast Healing Wave");
                }

                return WasHealCast;
            }

            private void MoveToHealTarget(WoWUnit unit, double distRange)
            {
                if (!IsUnitInRange(unit, distRange))
                {
                    Slog("MoveToHealTarget:  moving to Heal Target {0} who is {1:F1} yds away", Safe_UnitName(unit), unit.Distance);
                    if (_me.IsCasting)
                        WaitForCurrentSpell(null);

                    MoveToUnit(unit);
                    while (!IsGameUnstable() && _me.IsAlive && _me.IsMoving && unit.IsAlive && !IsUnitInRange(unit, distRange) && unit.Distance < 100)
                    {
                        // while running, if someone else needs a heal throw a riptide on them
                        if (SpellManager.HasSpell("Riptide") && SpellManager.CanCast("Riptide"))
                        {
                            WoWPlayer otherTarget = ChooseNextHealTarget(unit, (double)hsm.NeedHeal);
                            if (otherTarget != null)
                            {
                                Slog("MoveToHealTarget:  healing {0} while moving to heal target {1}", Safe_UnitName(otherTarget), Safe_UnitName(unit));
                                Safe_CastSpell(otherTarget, "Riptide", SpellRange.Check, SpellWait.NoWait);
                                StyxWoW.SleepForLagDuration();
                            }
                        }
                    }

                    if (_me.IsMoving)
                        Safe_StopMoving();

                    Dlog("MoveToHealTarget: stopping now that Heal Target is {0} yds away", unit.Distance);
                }
            }
        }

        class HealSpell
        {
            public int Health;
            public string DisplayName;
            public string TestSpell;        // for trained and on cooldown
            public HealCast Cast;

            public delegate bool HealCast(WoWUnit healTarget);

            public HealSpell(int h, string dname, string tname, HealCast fn)
            {
                if (!SpellManager.HasSpell(tname))
                {
                    h = 0;
                    Dlog("ignoring untrained healing spell '{0}' - setting health to 0%", dname);
                }

                Health = h;
                DisplayName = dname;
                TestSpell = tname;
                Cast = fn;
            }
        }
    }
}
