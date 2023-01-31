//CustomClass Template - Created by CodenameGamma
//Replace Layout with the CC name, 
//and WoWClass.Mage with the Class your Designing for.
//Created July, 3rd 2010
//For use with Honorbuddy
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Text;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace ProtectAndServe
{
    class Classname : CombatRoutine
    {
        public override sealed string Name { get { return "Venus112 Protect&Serve V.1.9"; } }

        public override WoWClass Class { get { return WoWClass.Paladin; } }

        private static LocalPlayer Me { get { return ObjectManager.Me; } }




        private void slog(string format, params object[] args) //use for slogging
        {
            Logging.Write(format, args);
        }

        public override void Initialize()
        {
            Logging.Write(Color.Red, "Venus112 Protect&Serve V.1.9");
        }

        public override bool WantButton
        {
            get
            {

                return true;
            }
        }

        public override void OnButtonPress()
        {

            ProtectAndServe.ProtectAndServeConfig f1 = new ProtectAndServe.ProtectAndServeConfig();
            f1.ShowDialog();
        }

        #region Pre Combat Buffs

        public override bool NeedPreCombatBuffs
        {
            get
            {
                RighteousFury();
                SealofTruth();
                BoKMe();
                BoMMe();
                BuffCheck();
                return false;
            }
        }

        public override void PreCombatBuff()
        {

        }

        #endregion

        #region Combat Buffs

        public override bool NeedCombatBuffs
        {
            get
            {
                LayonHands();
                HolyShield();
                GuardianoftheancientKings();
                ArdentDefender();
                DivineProtection();
                DivinePlea();
                SealofTruth();
                RighteousFury();
                BoKMe();
                BoMMe();
                BuffCheck();
                return false;
            }
        }

        public override void CombatBuff()
        {

        }

        #endregion

        #region BoM/BoK
        public bool BoKMe()
        {
            if (Me.IsInRaid || Me.IsInParty)
            {
                if (!Me.HasAura("Blessing of Kings") && !Me.HasAura("Mark of the Wild") && ProtectAndServeSettings.Instance.Buff)
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

        public bool BoMMe()
        {
            if (Me.IsInRaid || Me.IsInParty)
            {
                if (!Me.HasAura("Blessing of Might") && !Me.Mounted && ProtectAndServeSettings.Instance.Buff)
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

        public bool BuffCheck()
        {
            foreach (WoWPlayer p in Me.PartyMembers)
            {
                 if (!p.Auras.ContainsKey("Blessing of Might") && p.IsAlive && p.Distance < 30)
                {
                    if (Me.Auras.ContainsKey("Blessing of Might") && Me.Auras["Blessing of Might"].CreatorGuid == Me.Guid && ProtectAndServeSettings.Instance.Buff)
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
                    if (Me.Auras.ContainsKey("Blessing of Kings") && Me.Auras["Blessing of Kings"].CreatorGuid == Me.Guid && ProtectAndServeSettings.Instance.Buff)
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

        #region Falling

        public void HandleFalling() { }

        #endregion

        #region Combat

        public override void Combat()
        {

            if (!StyxWoW.Me.IsInInstance)
            {
                NeedEmergencyHeals();
            }
            {
                WoWUnit TauntTarget = FindTauntTarget();

                //Credits to fiftypence for basic taunt logic
                if (TauntTarget != null)
                {
                    WoWUnit AlliedTauntTarget = TauntTarget.CurrentTarget;
                    StyxWoW.SleepForLagDuration();
                    if (EnemyUnitsTauntable.Count == 1)
                    {
                        if (TauntTarget.Distance2D <= 30
                            && SpellManager.CanCast("Hand of Reckoning"))
                        {
                            SpellManager.Cast("Hand of Reckoning", TauntTarget);
                            Logging.Write(Color.Red, "Hand of Reckoning on " + TauntTarget.Name);
                            return;
                        }
                    }

                    else if (TauntTarget.Distance2D <= 40
                        && SpellManager.CanCast("Righteous Defense", AlliedTauntTarget)
                        && !TauntTarget.HasAura("Hand of Reckoning"))
                    {

                        if (AlliedTauntTarget != null)
                        {
                            SpellManager.Cast("Righteous Defense", AlliedTauntTarget);
                            Logging.Write(Color.Red, "Righteous Defense on " + AlliedTauntTarget.Name);
                            return;
                        }
                    } if (TauntTarget.Distance2D <= 30
                             && SpellManager.CanCast("Hand of Reckoning"))
                    {
                        SpellManager.Cast("Hand of Reckoning", TauntTarget);
                        Logging.Write(Color.Red, "Hand of Reckoning on " + TauntTarget.Name);
                        return;
                    }
                }
            }



            adds = detectAdds();
            saveLives();
            if ((Me.CurrentTarget != null && !Me.CurrentTarget.IsFriendly && Me.CurrentTarget.IsAlive && adds.Count == 1 && Me.Mounted == false) /*|| (Me.CurrentTarget != null && !Me.CurrentTarget.IsFriendly && Me.CurrentTarget.IsAlive == true && adds.Count == 2 && Me.Mounted == false)*/) //1-2 Mobs			
            {
                saveLives();
              
                if (Me.HealthPercent < 40 && Me.CurrentHolyPower == 3)
                {
                    if (CastSpell("Word of Glory"))
                    {
                        Logging.Write(Color.Lime, "Word of Glory");
                    }
                }


                if (Me.Rooted)
                {
                    if (CastSpell("Hand of Freedom"))
                    {
                        Logging.Write(Color.Lime, "Hand of Freedom");
                    }
                }

                if (Me.CurrentTarget.Fleeing)
                {
                    if (CastSpell("Hammer of Justice"))
                    {
                        Logging.Write(Color.Lime, "Hammer of Justice");
                    }
                }

                if ((Me.CurrentTarget.HealthPercent > 20 && ((Me.IsInInstance && (StyxWoW.Me.CurrentTarget.Level == StyxWoW.Me.Level + 2 && (StyxWoW.Me.CurrentTarget.Elite || (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss)))))) || (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss && !Me.IsInInstance))
                {
                    if (CastSpell("Avenging Wrath"))
                    {
                        Logging.Write(Color.Lime, "Avenging Wrath");
                    }
                }

                if (Me.CurrentHolyPower == 3 && Me.Auras.ContainsKey("Sacred Duty"))
                {
                    if (StyxWoW.Me.Auras["Sacred Duty"].Duration >= 1)
                    {
                        if (CastSpell("Shield of the Righteous"))
                        {
                            Logging.Write(Color.Lime, "Shield of Righteous");
                        }
                    }
                }
                if (Me.CurrentHolyPower == 3)
                {
                    if (CastSpell("Shield of the Righteous"))
                    {
                        Logging.Write(Color.Lime, "Shield of Righteous");
                    }
                }


                if (CastSpell("Crusader Strike"))
                {
                    Logging.Write(Color.Lime, "Crusader Strike");
                }

                if (Me.Auras.ContainsKey("Grand Crusader"))
                {
                    if (StyxWoW.Me.Auras["Grand Crusader"].Duration >= 1)
                    {
                        if (CastSpell("Avenger's Shield"))
                        {
                            Logging.Write(Color.Lime, "Avenger's Shield");
                        }
                    }
                }
                if (CastSpell("Avenger's Shield"))
                {
                    Logging.Write(Color.Lime, "Avenger's Shield");
                }

                if (Me.CurrentTarget.HealthPercent < 20)
                {
                    if (CastSpell("Hammer of Wrath"))
                    {
                        Logging.Write(Color.Lime, "Hammer of Wrath");
                    }
                }

                {
                    if (!Me.CurrentTarget.IsFriendly || Me.ManaPercent < 80)
                    {
                        if (CastSpell("Judgement"))
                            Logging.Write(Color.Lime, "Judgement");
                    }
                }

                if (Me.ManaPercent > 50 && Me.Location.Distance(Me.CurrentTarget.Location) <= 10 && !Me.IsMoving)
                {
                    if (CastSpell("Holy Wrath"))
                    {
                        Logging.Write(Color.Lime, "Holy Wrath");
                    }
                }

                if (Me.ManaPercent > 50 && Me.Location.Distance(Me.CurrentTarget.Location) <= 10 && !Me.IsMoving)
                {
                    if (CastSpell("Consecration"))
                    {
                        Logging.Write(Color.Lime, "Consecration");
                    }
                }
            }

            if (Me.CurrentTarget != null && !Me.CurrentTarget.IsFriendly && Me.CurrentTarget.IsAlive && adds.Count > 1 && Me.Mounted == false) //3+ Mobs			
            {
                saveLives();
                
                if (Me.HealthPercent < 40 && Me.CurrentHolyPower == 3)
                {
                    if (CastSpell("Word of Glory"))
                    {
                        Logging.Write(Color.Lime, "Word of Glory");
                    }
                }

                if ((Me.CurrentTarget.HealthPercent > 20 && ((Me.IsInInstance && (StyxWoW.Me.CurrentTarget.Level == StyxWoW.Me.Level + 2 && (StyxWoW.Me.CurrentTarget.Elite || (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss)))))) || (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss && !Me.IsInInstance))
                {
                    if (CastSpell("Avenging Wrath"))
                    {
                        Logging.Write(Color.Lime, "Avenging Wrath");
                    }
                }

                if (Me.CurrentTarget.Fleeing)
                {
                    if (CastSpell("Hammer of Justice"))
                    {
                        Logging.Write(Color.Lime, "Hammer of Justice");
                    }
                }

                if (CastSpell("Hammer of the Righteous"))
                {
                    Logging.Write(Color.Lime, "Hammer of the Righteous");
                }

                if ((Me.CurrentHolyPower == 2 || Me.CurrentHolyPower == 3) && !Me.Auras.ContainsKey("Inquisition"))
                {
                    if (CastSpell("Inquisition"))
                    {
                        Logging.Write(Color.Lime, "Inquisition");
                    }
                }


                if (Me.ManaPercent > 50 && Me.Location.Distance(Me.CurrentTarget.Location) <= 10 && !Me.IsMoving)
                {
                    if (CastSpell("Consecration"))
                    {
                        Logging.Write(Color.Lime, "Consecration");
                    }
                }

                if (Me.ManaPercent > 50 && Me.Location.Distance(Me.CurrentTarget.Location) <= 10 && !Me.IsMoving)
                {
                    if (CastSpell("Holy Wrath"))
                    {
                        Logging.Write(Color.Lime, "Holy Wrath");
                    }
                }


                {
                    if (!Me.CurrentTarget.IsFriendly || Me.ManaPercent < 80)
                    {
                        if (CastSpell("Judgement"))
                            Logging.Write(Color.Lime, "Judgement");
                    }
                }

                if (Me.CurrentHolyPower == 3 && Me.Auras.ContainsKey("Inquisition"))
                {
                    if (CastSpell("Shield of the Righteous"))
                    {
                        Logging.Write(Color.Lime, "Shield of Righteous");
                    }
                }

                if (CastSpell("Avenger's Shield"))
                {
                    Logging.Write(Color.Lime, "Avenger's");
                }

                if (Me.CurrentTarget.HealthPercent < 20)
                {
                    if (CastSpell("Hammer of Wrath"))
                    {
                        Logging.Write(Color.Lime, "Hammer of Wrath");
                    }
                }

            }

        }

        #endregion
              
        #region Spells


        private void AutoAttack()
        {
            if (!Me.IsAutoAttacking)
            {
                Lua.DoString("StartAttack()");
            }

        }
        #endregion

        #region CastSpell Method
        // Credit to Apoc for the below CastSpell code
        // Used for calling CastSpell in the Combat Rotation


        //Credit to Wulf!
        public bool CastSpell(string spellName, WoWPlayer target)
        {
            if (SpellManager.CanCast(spellName, target) && !SpellManager.GlobalCooldown && !Me.IsCasting)
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
            }
            // Can't cast the spell right now, so return false.
            return false;
        }
        #endregion

        #region Add Detection
        //Credit to CodeNameGamma for detectAdds code
        private List<WoWUnit> adds = new List<WoWUnit>();

        private List<WoWUnit> detectAdds()
        {
            List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        unit.Location.Distance(Me.CurrentTarget.Location) < 10.00 &&
                        ((unit.IsTargetingMyPartyMember || unit.IsTargetingMyRaidMember || unit.IsTargetingMeOrPet) || (unit.Name == "Risen Ghoul" || unit.Name == "Raider's Training Dummy" || unit.Name == "Deathwing" || unit.Name == "Wing Tentacle" || unit.Name == "Arm Tentacle" || unit.Name == "Cho'gall" || unit.Name == "Nefarian" || unit.Name == "Mannoroth")) &&
                        !unit.IsFriendly &&
                        !unit.IsPet &&
                        !Styx.Logic.Blacklist.Contains(unit.Guid));

            if (addList.Count > 2)
            {
                //Logging.Write(Color.Orange, "Detected " + addList.Count.ToString() + " adds! Switchting to AoE mode!");
            }
            return addList;
        }
        #endregion


        #region Righteous Fury

        public bool RighteousFury()
        {
            if (!Me.HasAura("Righteous Fury") && !StyxWoW.Me.Mounted)
            {
                if (CastSpell("Righteous Fury"))
                {
                    Logging.Write(Color.Lime, "Righteous Fury");
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Seal of Truth

        public bool SealofTruth()
        {
            if ((!Me.Auras.ContainsKey("Seal of Truth") && !Me.Auras.ContainsKey("Seal of Insight")) && !StyxWoW.Me.Mounted)
            {
                if (CastSpell("Seal of Truth"))
                {
                    Logging.Write(Color.Lime, "Seal of Truth");
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Holy Shield

        public bool HolyShield()
        {
            if (Me.HealthPercent < ProtectAndServeSettings.Instance.HolyShieldPercent && ProtectAndServeSettings.Instance.UseDefensiveCooldowns)
            {
                if (SpellManager.CanCast("Holy Shield"))
                {
                    if (CastSpell("Holy Shield"))
                    {
                        Logging.Write(Color.Lime, "Holy Shield" + "   " + Convert.ToInt16(Me.HealthPercent) + "Percent");
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Lay on Hands
        public bool LayonHands()
        {
            if (Me.HealthPercent < 15 && !StyxWoW.Me.HasAura("Forbearance") && !StyxWoW.Me.HasAura("Ardent Defender") && !SpellManager.CanCast("Ardent Defender"))
            {
                if (SpellManager.CanCast("Lay on Hands"))
                {
                    if (CastSpell("Lay on Hands"))
                    {
                        Logging.Write(Color.Lime, "Lay on Hands" + "   " + Convert.ToInt16(Me.HealthPercent) + "Percent");
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion


        #region Divine Protection

        public bool DivineProtection()
        {
            if (Me.HealthPercent < ProtectAndServeSettings.Instance.DivineProtectionPercent && ProtectAndServeSettings.Instance.UseDefensiveCooldowns)
            {
                // Thank you to No1knowsy for helping me to understand
                if (!Me.HasAura("Holy Shield") && !SpellManager.CanCast("Holy Shield"))
                {
                    if (SpellManager.CanCast("Divine Protection"))
                    {
                        if (CastSpell("Divine Protection"))
                        {
                            Logging.Write(Color.Lime, "Divine Protection" + "   " + Convert.ToInt16(Me.HealthPercent) + "Percent");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region Ardent Defender
        public bool ArdentDefender()
        {
            if (Me.HealthPercent < ProtectAndServeSettings.Instance.ArdentDefenderPercent && ProtectAndServeSettings.Instance.UseDefensiveCooldowns)
            {
                if (SpellManager.CanCast("Ardent Defender"))
                {
                    if (CastSpell("Ardent Defender"))
                    {
                        Logging.Write(Color.Lime, "Ardent Defender" + "   " + Convert.ToInt16(Me.HealthPercent) + "Percent");
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Guardian of the ancient Kings
        public bool GuardianoftheancientKings()
        {
            if (Me.HealthPercent < ProtectAndServeSettings.Instance.GuardianoftheancientKingsPercent && ProtectAndServeSettings.Instance.UseDefensiveCooldowns)
            {
                if (SpellManager.CanCast("Guardian of Ancient Kings"))
                {
                    if (CastSpell("Guardian of Ancient Kings"))
                    {
                        Logging.Write(Color.Lime, "Guardian of Ancient Kings" + "   " + Convert.ToInt16(Me.HealthPercent) + "Percent");
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion


        #region Divine Plea
        public bool DivinePlea()
        {
            if (Me.ManaPercent < 50)
            {
                if (SpellManager.CanCast("Divine Plea"))
                {
                    if (CastSpell("Divine Plea"))
                    {
                        Logging.Write(Color.Lime, "Divine Plea" + "   " + Convert.ToInt16(Me.ManaPercent) + "Percent");
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Need Emergency Heal
        public bool NeedEmergencyHeals()
        {

            if (Me.HealthPercent < 50 && Me.CurrentHolyPower == 3)
            {
                if (CastSpell("Word of Glory"))
                {
                    Logging.Write(Color.Lime, "Word of Glory");
                    return true;
                }
            }
            return false;
        }
        #endregion

        //Credit to Wulf!
        #region saveLives
        private void saveLives()
        {
            foreach (WoWPartyMember e in Me.PartyMemberInfos)
            {
                foreach (WoWPlayer d in Me.PartyMembers)
                {
                    double friendHp = d.HealthPercent;
                    double friendMp = d.ManaPercent;
                    double friendMax = d.MaxMana;

                    {
                        if (friendHp < 15 && friendMp > 25 && friendMax > Me.MaxMana * 3 && !d.Auras.ContainsKey("Forbearance") && Me.HealthPercent > 40)
                        {
                            if (CastSpell("Lay on Hands", d))
                                Logging.Write(Color.Orange, "[+] Lay on Hands on " + d);
                        }
                    }

                    if (friendHp < 30 && Me.HealthPercent > 40 && d.Combat && d.Distance < 30)
                    {
                        if (CastSpell("Hand of Sacrifice", d))
                            Logging.Write(Color.Orange, "[+] HoS on " + d);
                    }
                    if (friendHp < 20 && Me.HealthPercent > 40 && d.Combat && d.Distance < 30)
                    {
                        if (e.Role == WoWPartyMember.GroupRole.Tank)
                            if (!d.Auras.ContainsKey("Forbearance"))
                            {
                                if (CastSpell("Hand of Protection", d))
                                    Logging.Write(Color.Orange, "[+] HoP on " + d);
                            }
                    }
                }
            }
        }
        #endregion



        //Credits to fiftypence for basic taunt logic
        public List<WoWUnit> EnemyUnitsTauntable
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                    .Where(unit =>
                        !unit.IsFriendly
                        && (unit.IsTargetingMyPartyMember || unit.IsTargetingMyRaidMember)
                        && unit.IsAlive
                        && unit.ThreatInfo.RawPercent < 100
                        && unit.MaxHealth < Me.MaxHealth * 3
                        && unit.Elite
                        && !unit.IsTargetingMeOrPet
                        && !unit.IsPet
                        && !unit.IsNonCombatPet
                        && !unit.IsCritter
                        && unit.DistanceSqr
                    <= 80 * 80).ToList();
            }
        }

        public WoWUnit FindTauntTarget()
        {
            return (from unit in EnemyUnitsTauntable
                    orderby unit.DistanceSqr descending
                    select unit).FirstOrDefault();
        }
    }
}