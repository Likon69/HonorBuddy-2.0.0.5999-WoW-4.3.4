using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using System.Drawing;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Avenger
{
    class Classname : CombatRoutine
    {
        public override sealed string Name { get { return "Avenger v1.9"; } }

        public override WoWClass Class { get { return WoWClass.Paladin; } }


        private static LocalPlayer Me { get { return ObjectManager.Me; } }
   
        #region Log
        private void slog(string format, params object[] args) //use for slogging
        {
            Logging.Write(format, args);
        }
        #endregion
        

        #region Initialize
        public override void Initialize()
        {
			Logging.Write(Color.White, "___________________________________________________");
            Logging.Write(Color.Crimson, "-------- Avenger v1.9 CC by Shaddar & Venus112 -------");
			Logging.Write(Color.Crimson, "-----  Remember to comment on the forum! -----");
			Logging.Write(Color.Crimson, "-------- /like and +rep if you like this CC! ------");
            Logging.Write(Color.Crimson, "-------- Thank you to Fiftypence for support ------");
			Logging.Write(Color.White, "___________________________________________________");
        }
        #endregion

        #region Settings

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {

            Avenger.AvengerConfig f1 = new Avenger.AvengerConfig();
            f1.ShowDialog();
        }
        #endregion

        #region Rest

        public override bool  NeedRest
        {
            get
            {
                {
                    if (!Me.Mounted && SpellManager.HasSpell("Seal of Truth") && !Me.HasAura("Seal of Truth"))
                    {
                        if (CastSpell("Seal of Truth"))
                            Logging.Write(Color.Aqua, ">> Seal of Truth <<");
                        return true;

                    }
                }
                return false;
            }
        }

        public override void  Rest()
        {

        }
        #endregion

        #region
        public bool GotAggro()
        {
            foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, true))
            {
                if (u.Aggro && u.IsHostile && u.IsTargetingMeOrPet)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Buffing
        private bool BoKMe()
        {
            if (Me.IsInRaid || Me.IsInParty)
            {
                if (!Me.HasAura("Blessing of Kings") && !Me.HasAura("Mark of the Wild") && AvengerSettings.Instance.Buff)
                {
                    if (!Me.Mounted)
                    {
                        if (CastSpell("Blessing of Kings"))
                            Logging.Write(Color.Lime, "Buffing BoK on me");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool BoMMe()
        {
            if (Me.IsInRaid || Me.IsInParty)
            {
                if (!Me.HasAura("Blessing of Might") && !Me.Mounted && AvengerSettings.Instance.Buff)
                {
                    if (Me.HasAura("Mark of the Wild") || (Me.HasAura("Blessing of Kings") && Me.Auras["Blessing of Kings"].CreatorGuid != Me.Guid))
                    {
                        if (CastSpell("Blessing of Might"))
                            Logging.Write(Color.Lime, "Buffing BoM on me");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool BuffCheck()
        {
            foreach (WoWPlayer p in Me.PartyMembers)
            {
                if (!p.Auras.ContainsKey("Blessing of Might") && p.IsAlive && p.Distance < 30)
                {
                    if (Me.Auras.ContainsKey("Blessing of Might") && Me.Auras["Blessing of Might"].CreatorGuid == Me.Guid && AvengerSettings.Instance.Buff)
                    {
                        if (CastSpell("Blessing of Might"))
                        {
                            Logging.Write(Color.Lime, "Buffing BoM on Party/Raidmember");
                            return true;
                        }

                    }
                }


                if (!p.Auras.ContainsKey("Blessing of Kings") && p.IsAlive && p.Distance < 30)
                {
                    if (Me.Auras.ContainsKey("Blessing of Kings") && Me.Auras["Blessing of Kings"].CreatorGuid == Me.Guid && AvengerSettings.Instance.Buff)
                    {
                        if (CastSpell("Blessing of Kings"))
                        {
                            Logging.Write(Color.Lime, "Buffing BoM on Party/Raidmember");
                            return true;
                        }

                    }
                }

            } return false;

        }

        #endregion

        #region Dragon Soul

        public bool DebuffByID(int spellId)
        {
            if (Me.HasAura(spellId) && StyxWoW.Me.GetAuraById(spellId).TimeLeft.TotalMilliseconds <= 2000)
                return true;
            else return false;
        }

        public bool Ultra()
        {
            using (new FrameLock())
            {
                if (!StyxWoW.Me.ActiveAuras.ContainsKey("Divine Protection"))
                {
                    if (AvengerSettings.Instance.DSL)
                    {
                        foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, true))
                        {
                            if (u.IsAlive
                                && u.Guid != Me.Guid
                                && u.IsHostile
                                && u.IsCasting
                                && (u.CastingSpell.Id == 109417
                                    || u.CastingSpell.Id == 109416
                                    || u.CastingSpell.Id == 109415
                                    || u.CastingSpell.Id == 106371)
                                && u.CurrentCastTimeLeft.TotalMilliseconds <= 800)
                                return true;
                        }

                    }
                }
            }
            return false;
        }

        public bool UltraFL()
        {
            {
                if (AvengerSettings.Instance.DSL && 
                    (DebuffByID(110079)
                    || DebuffByID(110080)
                    || DebuffByID(110070)
                    || DebuffByID(110069)
                    || DebuffByID(109075)
                    || DebuffByID(110068)
                    || DebuffByID(105925)
                    || DebuffByID(110078)))
                    return true;
            }
            return false;
        }

        public bool DW()
        {
            {
                if (AvengerSettings.Instance.DSL &&
                    (DebuffByID(110139)
                    || DebuffByID(110140)
                    || DebuffByID(110141)
                    || DebuffByID(106791)
                    || DebuffByID(109599)
                    || DebuffByID(106794)
                    || DebuffByID(109597)
                    || DebuffByID(109598)))
                    return true;
            }
            return false;
        }
        #endregion

        #region Add Detection
        private int addCount()
        {
            int count = 0;
            foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(false, false))
            {
                if (u.Guid != Me.Guid
                    && u.IsHostile
                    && !u.IsCritter
                    && (u.Location.Distance(Me.CurrentTarget.Location) <= 12 || u.Location.Distance2D(Me.CurrentTarget.Location) <= 12)
                    && (u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember || u.IsTargetingMeOrPet)
                    && !u.IsFriendly)
                {
                    count++;
                }
            }
            return count;

        }
        private bool IsTargetBoss()
        {
            using (new FrameLock())
            {
                if ((Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss ||
                (Me.CurrentTarget.Level >= 85 && Me.CurrentTarget.Elite) && Me.CurrentTarget.MaxHealth > 3500000)
                || Me.CurrentTarget.Name == "Wing Tentacle" 
                || Me.CurrentTarget.Name == "Arm Tentacle")
                    return true;

                else return false;
            }
        }
        #endregion

        #region CastSpell Method
        // Credit to Apoc for the below CastSpell code
        // Used for calling CastSpell in the Combat Rotation


        //Credit to Wulf!
        public bool CastSpell(string spellName, WoWPlayer target)
        {

            if (SpellManager.CanCast(spellName, target) && !SpellManager.GlobalCooldown)
            {
                if (target == Me)
                {
                    //SpellManager.Cast(spellName, Me);
                    return false;
                }
                else
                {
                    SpellManager.Cast(spellName, target);
                    return true;
                }
            }
            return false;
        }


        public bool CastSpell(string spellName)
        {
            if (SpellManager.CanCast(spellName) && !SpellManager.GlobalCooldown && !Me.IsCasting)
            {
                SpellManager.Cast(spellName);
                // We managed to cast the spell, so return true, saying we were able to cast it.
                return true;
            }            // Can't cast the spell right now, so return false.
            return false;
        }

                public bool CastThatSpell(string spellName)
        {
            {
                if (SpellManager.HasSpell(spellName) && SpellManager.Spells[spellName].CooldownTimeLeft.TotalMilliseconds < 500)
                {
                    SpellManager.Cast(spellName);
                    return true;
                }

            }
            return false;
        }
        #endregion

        #region CombatStart


        private void AutoAttack()
        {
            if (!Me.IsAutoAttacking && Me.CurrentTarget.Distance <=7d)
            {
                Lua.DoString("StartAttack()");
            }
        }
        #endregion

        #region Combat
      
        public override void Combat()
        {

			if (Me.CurrentTarget != null  && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {

                {
                    if (Me.HealthPercent <= 15 && Me.Combat && AvengerSettings.Instance.LoH && !Me.ActiveAuras.ContainsKey("Forbearance"))
                    {
                        if (CastSpell("Lay on Hands") == true)
                        {
                            Logging.Write(Color.Crimson, ">> Lay on Hands!!! <<");
                        }
                    }
                }

                //////////////////////////////////////////////Prio spells///////////////////////////////////////////////////////////////////////////////////////////////////////

                {
                    if (Ultra() && AvengerSettings.Instance.DSL)
                    {
                        Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                        {
                            Logging.Write(Color.Aqua, ">> Heroic Will! <<");
                        }
                    }
                }
                {
                    if (UltraFL() && AvengerSettings.Instance.DSL)
                    {
                        Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                        {
                            Logging.Write(Color.Aqua, ">> Heroic Will! <<");
                        }
                    }
                }
                {
                    if (DW() && AvengerSettings.Instance.DSL)
                    {
                        Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                        {
                            Logging.Write(Color.Aqua, ">> Enter the dream! <<");
                        }
                    }
                }

                //////////////////////////////////////////////Defensive Cooldowns///////////////////////////////////////////////////////////////////////////////////////////////
                
                {
                    if (GotAggro() && Me.HealthPercent <= 95 && SpellManager.CanCast("Divine Protection") && AvengerSettings.Instance.DivineProtection)
                    {
                        if (CastSpell("Divine Protection"))
                        {
                            Logging.Write(Color.Aqua, ">> Got aggro, using Divine Protection" + Convert.ToInt16(Me.HealthPercent) + "Percent <<");
                        }
                    }
                }

                {
                    if (GotAggro() && Me.HealthPercent <= 35 && SpellManager.CanCast("Divine Shield") && AvengerSettings.Instance.DivineShield && !Me.ActiveAuras.ContainsKey("Forbearance"))
                    {
                        if (CastSpell("Divine Shield"))
                        {
                            Logging.Write(Color.Aqua, ">> Got aggro, using Divine Shield" + Convert.ToInt16(Me.HealthPercent) + "Percent <<");
                        }
                    }
                }


                //////////////////////////////////////////////Interrupts here///////////////////////////////////////////////////////////////////////////////////////////////////

                {
                    if (Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast && AvengerSettings.Instance.Rebuke)
                    {
                        if (CastSpell("Rebuke"))
                        {
                            Logging.Write(Color.White, ">> Rebuke <<");
                        }
                    }
                }

                //////////////////////////////////////////////AoE Rotation here/////////////////////////////////////////////////////////////////////////////////////////////////

                {
                    if (addCount() >= AvengerSettings.Instance.AoEMobs && AvengerSettings.Instance.SOTR && !Me.Auras.ContainsKey("Seal of Righteousness") && Me.Auras.ContainsKey("Seal of Truth"))
                    {
                        if (CastSpell("Seal of Righteousness"))
                        {
                            Logging.Write(Color.Aqua, ">> Seal of Righteousness <<");
                        }
                    }
                }
                {
                    if (addCount() >= AvengerSettings.Instance.AoEMobs && AvengerSettings.Instance.DivineStorm)
                    {
                        if (CastSpell("Divine Storm"))
                        {
                            Logging.Write(Color.Aqua, ">> Divie Storm <<");
                        }
                    }
                }

                /////////////////////////////////////////////Cooldowns are here////////////////////////////////////////////////////////////////////////////////////////////////
                {
                    if (IsTargetBoss() && (Me.CurrentTarget.IsWithinMeleeRange || Me.Location.Distance(Me.CurrentTarget.Location) <= 10 || Me.Location.Distance2D(Me.CurrentTarget.Location) <= 10) && AvengerSettings.Instance.Zealotry && (Me.CurrentHolyPower == 3 || Me.ActiveAuras.ContainsKey("Divine Purpose")) && Me.ActiveAuras.ContainsKey("Inquisition"))
                    {
                        if ((Me.ActiveAuras.ContainsKey("Guardian of Ancient Kings") && Me.Auras["Guardian of Ancient Kings"].TimeLeft.TotalSeconds <= 20) || (!SpellManager.CanCast("Guardian of ancient kings")) || (!AvengerSettings.Instance.GOAK))

                        if (CastSpell("Zealotry"))
                        {
                            Logging.Write(Color.Blue, ">> Zealotry <<");
                        }
                    }
                }
                {
                    if (IsTargetBoss() && AvengerSettings.Instance.AvengingWrath && Me.ActiveAuras.ContainsKey("Inquisition") && (Me.CurrentTarget.IsWithinMeleeRange || Me.Location.Distance(Me.CurrentTarget.Location) <= 10 || Me.Location.Distance2D(Me.CurrentTarget.Location) <= 10))
                    {
                        if (AvengerSettings.Instance.Zealotry && Me.ActiveAuras.ContainsKey("Zealotry") || (!AvengerSettings.Instance.Zealotry))
                        {
                            if (CastThatSpell("Avenging Wrath"))
                            {
                                Logging.Write(Color.Blue, ">> Avenging Wrath <<");
                            }
                        }
                    }
                }
                {
                    if (Me.ManaPercent <= 20)
                    {
                        if (CastSpell("Divine Plea"))
                        {
                            Logging.Write(Color.Blue, ">> Divine Plea <<");
                        }
                    }
                }
                {
                    if (IsTargetBoss() && (Me.CurrentTarget.IsWithinMeleeRange || Me.Location.Distance(Me.CurrentTarget.Location) <= 10 || Me.Location.Distance2D(Me.CurrentTarget.Location) <= 10) && AvengerSettings.Instance.GOAK && Me.ActiveAuras.ContainsKey("Inquisition") && SpellManager.CanCast("Zealotry"))
                    {
                        if (CastSpell("Guardian of Ancient Kings"))
                        {
                            Logging.Write(Color.Blue, ">> Guardian of Ancient Kings <<");
                        }
                    }
                }
                {
                    if (IsTargetBoss() && (Me.CurrentTarget.IsWithinMeleeRange || Me.Location.Distance(Me.CurrentTarget.Location) <= 10 || Me.Location.Distance2D(Me.CurrentTarget.Location) <= 10) && AvengerSettings.Instance.SynapseSprings && StyxWoW.Me.Inventory.Equipped.Hands != null && StyxWoW.Me.Inventory.Equipped.Hands.Cooldown <= 0)
                    {
                        if (AvengerSettings.Instance.Zealotry && Me.ActiveAuras.ContainsKey("Zealotry") || (!AvengerSettings.Instance.Zealotry))
                        {
                            Lua.DoString("RunMacroText('/use 10');");
                        }
                    }
                }
                {
                    if (IsTargetBoss() && (Me.CurrentTarget.IsWithinMeleeRange || Me.Location.Distance(Me.CurrentTarget.Location) <= 10 || Me.Location.Distance2D(Me.CurrentTarget.Location) <= 10) && AvengerSettings.Instance.Trinket1 && StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                    {
                        if (AvengerSettings.Instance.Zealotry && Me.ActiveAuras.ContainsKey("Zealotry") || (!AvengerSettings.Instance.Zealotry))
                        {
                            Lua.DoString("RunMacroText('/use 13');");
                        }
                    }
                }
                {
                    if (IsTargetBoss() && (Me.CurrentTarget.IsWithinMeleeRange || Me.Location.Distance(Me.CurrentTarget.Location) <= 10 || Me.Location.Distance2D(Me.CurrentTarget.Location) <= 10) && AvengerSettings.Instance.Trinket2 && StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                    {
                        if (AvengerSettings.Instance.Zealotry && Me.ActiveAuras.ContainsKey("Zealotry") || (!AvengerSettings.Instance.Zealotry))
                        {
                            Lua.DoString("RunMacroText('/use 14');");
                        }
                    }
                }

                /////////////////////////////////////////////Single Target DPS here////////////////////////////////////////////////////////////////////////////////////////////

                {
                    if (addCount() < AvengerSettings.Instance.AoEMobs && !Me.Auras.ContainsKey("Seal of Truth") && Me.Auras.ContainsKey("Seal of Righteousness"))
                    {
                        if (CastSpell("Seal of Truth"))
                        {
                            Logging.Write(Color.Aqua, ">> Seal of Truth <<");
                        }
                    }
                }
                {
                    if ((Me.Auras.ContainsKey("Divine Purpose") && (!Me.Auras.ContainsKey("Inquisition") || Me.Auras["Inquisition"].TimeLeft.Seconds <= 3)) || (Me.Auras.ContainsKey("Zealotry") && (!Me.Auras.ContainsKey("Inquisition") || Me.Auras["Inquisition"].TimeLeft.Seconds <= 3)) || (Me.CurrentHolyPower == 3 && (!Me.Auras.ContainsKey("Inquisition") || Me.Auras["Inquisition"].TimeLeft.Seconds <= 3)))
                    {
                        if (CastSpell("Inquisition"))
                        {
                            Logging.Write(Color.Aqua, ">> Inquisition <<");
                        }
                    }
                }
                {
                    if (Me.Auras.ContainsKey("Divine Purpose") && Me.Auras.ContainsKey("Inquisition") || Me.Auras.ContainsKey("Zealotry") && Me.Auras.ContainsKey("Inquisition") || Me.CurrentHolyPower >= 3 && Me.Auras.ContainsKey("Inquisition"))
                    {
                        if (CastSpell("Templar's Verdict"))
                        {
                            Logging.Write(Color.Aqua, ">> Templar's Verdict <<");
                        }
                    }
                }
                {
                    if (Me.Auras.ContainsKey("Avenging Wrath") || Me.CurrentTarget.HealthPercent <= 20)
                    {
                        if (CastSpell("Hammer of Wrath"))
                        {
                            Logging.Write(Color.Aqua, ">> Crusader Strike <<");
                        }
                    }
                }
                {
                    if (addCount() < AvengerSettings.Instance.AoEMobs)
                    {
                        if (CastSpell("Crusader Strike"))
                        {
                            Logging.Write(Color.Aqua, ">> Crusader Strike <<");
                        }
                    }
                }
                {
                    if (StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War") && SpellManager.CanCast("Exorcism") && !Me.Auras.ContainsKey("Divine Purpose") && !Me.ActiveAuras.ContainsKey("Zealotry"))
                    {
                        if (CastSpell("Exorcism"))
                        {
                            Logging.Write(Color.Aqua, ">> Exorcism <<");
                        }
                    }
                }
                {
                    if (!Me.ActiveAuras.ContainsKey("Zealotry") || Me.ActiveAuras.ContainsKey("Zealotry") && SpellManager.Spells["Crusader Strike"].CooldownTimeLeft.TotalMilliseconds >= 1500 && SpellManager.Spells["Hammer of Wrath"].CooldownTimeLeft.TotalMilliseconds >= 1500)
                    {
                        if (CastSpell("Judgement"))
                        {
                            Logging.Write(Color.Aqua, ">> Judgement <<");
                        }
                    }
                }
                {
                    if ((!Me.Auras.ContainsKey("Divine Purpose") || !Me.Auras.ContainsKey("Zealotry") || !Me.Auras.ContainsKey("The Art of War") || Me.CurrentHolyPower >= 3) &&
                        Me.CurrentTarget.Distance <= 12d && Me.ManaPercent >= 35 && !SpellManager.CanCast("Judgement") && (!SpellManager.CanCast("Crusader Strike") || !SpellManager.CanCast("Divine Storm")))
                    {
                        if (CastSpell("Holy Wrath"))
                        {
                            Logging.Write(Color.Aqua, ">> Holy Wrath <<");
                        }
                    }
                }
                {
                    if ((!Me.Auras.ContainsKey("Divine Purpose") || !Me.Auras.ContainsKey("Zealotry") || !Me.Auras.ContainsKey("The Art of War") || Me.CurrentHolyPower >= 3) &&
                        Me.CurrentTarget.Distance <= 8d && Me.ManaPercent >= 70 && !SpellManager.CanCast("Judgement") && (!SpellManager.CanCast("Crusader Strike") || !SpellManager.CanCast("Divine Storm")))
                    {
                        if (CastSpell("Consecration"))
                        {
                            Logging.Write(Color.Aqua, ">> Consecration <<");
                        }
                    }
                }

            }                      
            
        }  
  
        #endregion

    }
}