using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Logic.Combat;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx;
using Styx.Logic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Drawing;

namespace TCDiscPriest
{
    class TCDiscPriestCombatClass : CombatRoutine
    {
        private WoWUnit lastTarget = null;
        private WoWUnit tank = null;
		private WoWUnit lastTank = null;
		
		//START OF CONSTANTS ==============================
		private const bool LOGGING = true;
		private const bool DEBUG = false;
		private const bool TRACE = false;
		
		private const string DEBUG_LABEL = "DEBUG";
		private const string TRACE_LABEL = "TRACE";
		private const string TANK_CHANGE = "TANK CHANGED";
		private const string FACING = "FACING";
		
		//START OF SPELLS AND AURAS ==============================
		private const string EVANGELISM = "Evangelism";
		private const string ARCHANGEL = "Archangel";
		private const string INNER_WILL = "Inner Will";
		private const string INNER_FIRE = "Inner Fire";
		private const string INNER_FOCUS = "Inner Focus";
		private const string HYMN_OF_HOPE = "Hymn of Hope";
		private const string PRAYER_OF_HEALING = "Prayer of Healing";
		private const string PRAYER_OF_MENDING = "Prayer of Mending";
		private const string POWER_WORD_SHIELD = "Power Word: Shield";
		private const string POWER_WORD_FORTITUDE = "Power Word: Fortitude";
		private const string POWER_WORD_BARRIER = "Power Word: Barrier";
		private const string DISPEL_MAGIC = "Dispel Magic";
		private const string FEAR_WARD = "Fear Ward";
		private const string MASS_DISPEL = "Mass Dispel";
		private const string PAIN_SUPPRESSION = "Pain Suppression";
		private const string PENANCE = "Penance";
		private const string POWER_INFUSION = "Power Infusion";
		private const string CURE_DISEASE = "Cure Disease";
		private const string DIVINE_HYMN = "Divine Hymn";
		private const string FLASH_HEAL = "Flash Heal";
		private const string GREATER_HEAL = "Greater Heal";
		private const string HEAL = "Heal";
		private const string HOLY_FIRE = "Holy Fire";
		private const string RESURRECTION = "Resurrection";
		private const string SMITE = "Smite";
		private const string FADE = "Fade";
		private const string SHADOWFIEND = "Shadowfiend";
		private const string SHADOW_PROTECTION = "Shadow Protection";
		private const string WEAKENED_SOUL = "Weakened Soul";
		private const string LEVITATE = "Weakened Soul";
		
		private const string DRINK = "Drink";
		private const string FOOD = "Food";
		//END OF SPELLS AND AURAS ==============================
		
		//END OF CONSTANTS ==============================
		
        public override void Pulse()
        {
            if (Me != null && Me.IsValid && Me.IsAlive)
            {
				tank = GetTank();
				
				if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;
				
				if(lastTank == null || lastTank.Guid != tank.Guid) {
					lastTank = tank;
					LogActivity(TANK_CHANGE, tank.Name);
				}
				
				Combat();
			}
		}
		
		public override void Initialize()
        {
		}
		
        public override bool WantButton
        {
            get
            {
                return false;
			}
		}
		
        public override void Combat()
        {
            if (StyxWoW.GlobalCooldown) return;
			else if (Mounted()) return;
            else if (Self()) return;
			else if (Resurrect()) return;
			else if (SoS()) return;
			else if (AoE()) return;
			else if (TankHealing()) return;
            else if (Healing()) return;
			else if (ManaRegen()) return;
			else if (Cleansing()) return;
			else if (Buff()) return;
			else if (NeedRest) return;
		}
		
		private bool Mounted()
        {
            return Me.Mounted;
		}
		
        private WoWPlayer GetTank()
        {
            foreach (WoWPlayer player in Me.RaidMembers)
            {
                if (IsTank(player)) return player;
			}
			
			foreach (WoWPlayer player in Me.PartyMembers)
            {
                if (IsTank(player)) return player;
			}
		
            return null;
		}
		
        private string DeUnicodify(string spell)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = Encoding.UTF8.GetBytes(spell);
            
			foreach (byte b in bytes)
            {
                if (b != 0)
				sb.Append("\\" + b);
			}
			
            return sb.ToString();
		}
		
        private bool IsTank(WoWPlayer player)
        {
            return Lua.GetReturnValues("return UnitGroupRolesAssigned('" + DeUnicodify(player.Name) + "')").First() == "TANK";
		}
		
		private bool IsHealer(WoWPlayer player)
        {
            return Lua.GetReturnValues("return UnitGroupRolesAssigned('" + DeUnicodify(player.Name) + "')").First() == "HEALER";
		}
		
		private bool ManaRegen()
		{
			WoWUnit target = tank.CurrentTarget;
			
			if (target != null && !target.IsFriendly && target.InLineOfSight && !target.Dead) 
			{
				if (Me.ManaPercent <= 80 && CanCast(SHADOWFIEND))
				{
					LogActivity(SHADOWFIEND, target.Name);
					return Cast(SHADOWFIEND, target);
				}
			}
			
			if (Me.ManaPercent <= 80 && CanCast(HYMN_OF_HOPE) && !Me.IsMoving)
            {
				LogActivity(HYMN_OF_HOPE);
                return Cast(HYMN_OF_HOPE);
			}
			
			return false;
		}
		
        private bool Self()
        {	
			if (Me.Combat && CanCast(ARCHANGEL) && (Me.Auras[EVANGELISM].StackCount == 5 || Me.Auras[EVANGELISM].TimeLeft.TotalSeconds <= 5) && TCDiscPriestSettings.Instance.UseArchangel)
			{
				LogActivity(ARCHANGEL);
				return Cast(ARCHANGEL);
			}
			
			if (Me.IsFalling && CanCast(LEVITATE))
			{
				LogActivity(LEVITATE);
				return Cast(LEVITATE);
			}
			
			if (Me.Combat && Me.HealthPercent < 100 && CanCast(FADE))
			{
				LogActivity(FADE);
				return Cast(FADE);
			}
			
			if (CanCast(INNER_WILL) && !isAuraActive(INNER_WILL) && TCDiscPriestSettings.Instance.UseInnerWill)
			{
				LogActivity(INNER_WILL);
				return Cast(INNER_WILL);
			}
			
			if (CanCast(INNER_FIRE) && !isAuraActive(INNER_FIRE) && TCDiscPriestSettings.Instance.UseInnerFire)
			{
				LogActivity(INNER_FIRE);
				return Cast(INNER_FIRE);
			}
			
			if (Me.Combat && CanCast(INNER_FOCUS) && !isAuraActive(INNER_FOCUS) && TCDiscPriestSettings.Instance.UseInnerFocus)
			{
				LogActivity(INNER_FOCUS);
				return Cast(INNER_FOCUS);
			}
			
            return false;
		}
		
		private bool AoE()
        {
			if (ShouldCast(TCDiscPriestSettings.Instance.DivineHymnPercent) && CanCast(DIVINE_HYMN) && !Me.IsMoving)
			{
				LogActivity(DIVINE_HYMN);
				return Cast(DIVINE_HYMN);
			}

			if (ShouldCast(TCDiscPriestSettings.Instance.PowerInfusionPercent) && CanCast(POWER_INFUSION))
			{
				LogActivity(POWER_INFUSION);
				return Cast(POWER_INFUSION);
			}
			
			if (ShouldCast(TCDiscPriestSettings.Instance.PrayerOfHealingPercent) && CanCast(PRAYER_OF_HEALING) && !Me.IsMoving)
			{
				LogActivity(PRAYER_OF_HEALING);
				return Cast(PRAYER_OF_HEALING);
			}
		
            return false;
		}
		
		private bool SoS()
		{
			WoWPlayer target = GetHealTarget();
		
            if (target != null)
            {
                if (target.Distance > 40 || !target.InLineOfSight) return true;
                else
                {
                    double hp = target.HealthPercent;
					
					if (DEBUG) hp = 31;
					
					if (hp < TCDiscPriestSettings.Instance.FlashHealPercent && CanCast(FLASH_HEAL) && !Me.IsMoving && isAuraActive(WEAKENED_SOUL, target))
                    {
						LogActivity(FLASH_HEAL, target.Name);
						return Cast(FLASH_HEAL, target);
					}
					
					if (hp < TCDiscPriestSettings.Instance.FlashHealPercent && !isAuraActive(POWER_WORD_SHIELD, target) && !isAuraActive(WEAKENED_SOUL, target) && CanCast(POWER_WORD_SHIELD)) 
					{
						LogActivity(POWER_WORD_SHIELD, target.Name);
						return Cast(POWER_WORD_SHIELD, target);
					}
					
                    return false;
				}
			}
            
			return false;
		}
		
        private bool Healing()
        {
            WoWPlayer healTarget = GetHealTarget();
		
            if (healTarget != null)
            {
				double hp = healTarget.HealthPercent;
			
				if (DEBUG) hp = 31;
			
                if (healTarget.Distance > 40 || !healTarget.InLineOfSight) return true;
                else
                {		
					if (isAuraActive(WEAKENED_SOUL, healTarget) && CanCast(GREATER_HEAL)) 
					{
						LogActivity(GREATER_HEAL, healTarget.Name);
						return Cast(GREATER_HEAL, healTarget);
					}
					
					if (healTarget.HealthPercent < TCDiscPriestSettings.Instance.PWShieldPercent && !isAuraActive(POWER_WORD_SHIELD, healTarget) && !isAuraActive(WEAKENED_SOUL, healTarget) && CanCast(POWER_WORD_SHIELD)) 
					{
						LogActivity(POWER_WORD_SHIELD, healTarget.Name);
						return Cast(POWER_WORD_SHIELD, healTarget);
					}
					
					if (healTarget.HealthPercent < TCDiscPriestSettings.Instance.PainSuppressionPercent && healTarget.Guid != tank.Guid && CanCast(PAIN_SUPPRESSION) && TCDiscPriestSettings.Instance.UsePainSuppression) 
					{
						LogActivity(PAIN_SUPPRESSION, healTarget.Name);
						return Cast(PAIN_SUPPRESSION, healTarget);
					}
					
					if (healTarget.HealthPercent < TCDiscPriestSettings.Instance.PenancePercent && CanCast(PENANCE) && !Me.IsMoving) 
					{
						LogActivity(PENANCE, healTarget.Name);
						return Cast(PENANCE, healTarget);
					}
					
					if (healTarget.HealthPercent < TCDiscPriestSettings.Instance.PWBarrierPercent && CanCast(POWER_WORD_BARRIER))
					{
						LogActivity(POWER_WORD_BARRIER);
						Cast(POWER_WORD_BARRIER);
						return LegacySpellManager.ClickRemoteLocation(healTarget.Location);
					}
				}
			}
			
			if (tank != null)
			{
				WoWUnit target = tank.CurrentTarget;
				
				if (target != null)
				{
					if (target.Distance > 30 || !target.InLineOfSight || target.Dead) return true;
					else 
					{
						if (tank.Combat && !target.IsFriendly && CanCast(HOLY_FIRE) && !Me.IsMoving) 
						{
							if (!Me.IsFacing(target))
							{
								LogActivity(FACING, target.Name);
								Me.SetFacing(target);
							}
							
							LogActivity(HOLY_FIRE, target.Name);
							return Cast(HOLY_FIRE, target);
						}
						
						if (tank.Combat && !target.IsFriendly && CanCast(SMITE) && !Me.IsMoving) 
						{
							if (!Me.IsFacing(target))
							{
								LogActivity(FACING, target.Name);
								Me.SetFacing(target);
							}
							
							LogActivity(SMITE, target.Name);
							return Cast(SMITE, target);
						}
					}
				}
			}
            
			return false;
		}
		
        private bool TankHealing()
        {
            if (tank != null)
            {
                if (tank.Distance > 40 || !tank.InLineOfSight) return true;
                else
                {
					if (tank.Combat && !isAuraActive(PRAYER_OF_MENDING, tank) && CanCast(PRAYER_OF_MENDING)) 
					{
						LogActivity(PRAYER_OF_MENDING, tank.Name);
						return Cast(PRAYER_OF_MENDING, tank);
					}
					
					if (tank.Combat && tank.HealthPercent < TCDiscPriestSettings.Instance.TankShieldPercent && !isAuraActive(POWER_WORD_SHIELD, tank) && !isAuraActive(WEAKENED_SOUL, tank) && CanCast(POWER_WORD_SHIELD)) 
					{
						LogActivity(POWER_WORD_SHIELD, tank.Name);
						return Cast(POWER_WORD_SHIELD, tank);
					}

                    return false;
				}
			}
            
			return false;
		}
		
		private bool ShouldCast(int threshold)
		{
			int memberCountBelowThreshold = GetMemberCountBelowThreshold(threshold);
			
			return ((memberCountBelowThreshold >= 15 && Me.RaidMembers.Count >= 15) || 
			(memberCountBelowThreshold >= 6 && Me.RaidMembers.Count >= 6) || 
			(memberCountBelowThreshold >= 3 && Me.RaidMembers.Count == 0 && Me.PartyMembers.Count >= 3));
		}
		
        private bool CanCast(string spell, WoWUnit target)
        {
            return SpellManager.CanCast(spell, target);
		}
		
        private bool CanCast(string spell)
        {
            return SpellManager.CanCast(spell);
		}
		
        private bool Cast(string spell, WoWUnit target)
        {
            if (SpellManager.Cast(spell, target))
            {
                lastTarget = target;
                return true;
			}
            
			return false;
		}
		
        private bool Cast(string spell)
        {
            lastTarget = Me;
            return SpellManager.Cast(spell, Me);
		}
		
        private bool Cleansing()
        {
			if (TCDiscPriestSettings.Instance.UseCureDisease)
            {
				WoWPlayer player = GetCureTarget();
				
				if (player != null && !Blacklist.Contains(player.Guid))
				{
					if (player.Distance > 40 || !player.InLineOfSight) return true;
					else if (CanCast(CURE_DISEASE, player))
					{
						LogActivity(CURE_DISEASE, player.Name);
						Blacklist.Add(player, new TimeSpan(0, 0, 2));
						return Cast(CURE_DISEASE, player);
					}
					
					return false;
				}
			} 
			
			if (TCDiscPriestSettings.Instance.UseDispelMagic)
            {
				WoWPlayer player = GetDispelTarget();
				
				if (player != null && !Blacklist.Contains(player.Guid))
				{
					if (player.Distance > 40 || !player.InLineOfSight) return true;
					else if (CanCast(DISPEL_MAGIC, player))
					{
						LogActivity(DISPEL_MAGIC, player.Name);
						Blacklist.Add(player, new TimeSpan(0, 0, 2));
						return Cast(DISPEL_MAGIC, player);
					}
					
					return false;
				}
			} 
			
			return false;
		}
		
        private bool NeedsCure(WoWPlayer player)
        {
            foreach (WoWAura a in player.Auras.Values)
            {
                if (a.IsHarmful && Me.ManaPercent > 35)
                {
                    WoWDispelType t = a.Spell.DispelType;
                    if (t == WoWDispelType.Poison) return true;
				}
			}
			
            return false;
		}
		
		private bool NeedsDispel(WoWPlayer player)
        {
            foreach (WoWAura a in player.Auras.Values)
            {
                if (a.IsHarmful && Me.ManaPercent > 35)
                {
                    WoWDispelType t = a.Spell.DispelType;
                    if (t == WoWDispelType.Magic) return true;
				}
			}
			
            return false;
		}
		
		private int GetMemberCountBelowThreshold(int threshold)
		{
			return Enumerable.Count((from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
			orderby unit.HealthPercent ascending
			where !unit.Dead
			where (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid)
			where !unit.IsGhost
			where unit.Distance <= 30
			where unit.HealthPercent <= threshold
			select unit));
		}
		
        private WoWPlayer GetHealTarget()
        {
			if (DEBUG) return Me;
			else return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
			orderby unit.HealthPercent ascending
			where !unit.Dead
			where !unit.IsGhost
			where unit.Distance <= 40
			where unit.HealthPercent < 100
			where (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid)
			select unit).FirstOrDefault();
		}
		
        private IEnumerable<WoWPlayer> GetResurrectTargets()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
			orderby unit.MaxHealth descending
			where unit.Dead
			where unit.IsInMyPartyOrRaid
			where !unit.IsGhost
			where unit.Distance <= 40
			select unit);
		}
		
		private WoWPlayer GetCureTarget()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
			orderby unit.HealthPercent ascending
			where !unit.Dead
			where !unit.IsGhost
			where unit.Distance <= 40
			where NeedsCure(unit)
			where (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid)
			select unit).FirstOrDefault();
		}
		
		private WoWPlayer GetDispelTarget()
        {
            return (from unit in ObjectManager.GetObjectsOfType<WoWPlayer>(true, true)
			orderby unit.HealthPercent ascending
			where !unit.Dead
			where !unit.IsGhost
			where unit.Distance <= 40
			where NeedsDispel(unit)
			where (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid)
			select unit).FirstOrDefault();
		}
		
        private bool Resurrect()
        {
            foreach (WoWPlayer player in GetResurrectTargets())
            {
                if (Blacklist.Contains(player.Guid)) continue;
                else
                {
                    if (player.Distance > 40 || !player.InLineOfSight) return true;
					else if (CanCast(RESURRECTION, player) && TCDiscPriestSettings.Instance.UseResurrection && !Me.IsMoving)
                    {
                        LogActivity(RESURRECTION, player.Name);
                        Blacklist.Add(player, new TimeSpan(0, 0, 15));
                        return Cast(RESURRECTION, player);
					}
                    
					return false;
				}
			}
			
            return false;
		}
		
        private bool Buff()
        {
            if (!isAuraActive(POWER_WORD_FORTITUDE) && CanCast(POWER_WORD_FORTITUDE))
            {
				LogActivity(POWER_WORD_FORTITUDE);
                return Cast(POWER_WORD_FORTITUDE);
			}
			
            foreach (WoWPlayer player in Me.PartyMembers)
            {
                if (player.Distance > 40 || player.Dead || player.IsGhost || !player.InLineOfSight) continue;
                else if (!isAuraActive(POWER_WORD_FORTITUDE, player) && CanCast(POWER_WORD_FORTITUDE))
                {
					LogActivity(POWER_WORD_FORTITUDE, player.Name);
                    return Cast(POWER_WORD_FORTITUDE, player);
				}
			}
			
			if (!isAuraActive(SHADOW_PROTECTION) && CanCast(SHADOW_PROTECTION))
            {
				LogActivity(SHADOW_PROTECTION);
                return Cast(SHADOW_PROTECTION);
			}
			
            foreach (WoWPlayer player in Me.PartyMembers)
            {
                if (player.Distance > 40 || player.Dead || player.IsGhost || !player.InLineOfSight) continue;
                else if (!isAuraActive(SHADOW_PROTECTION, player) && CanCast(SHADOW_PROTECTION))
                {
					LogActivity(SHADOW_PROTECTION, player.Name);
                    return Cast(SHADOW_PROTECTION, player);
				}
			}
			
			if (tank.Combat && !isAuraActive(FEAR_WARD, tank) && CanCast(FEAR_WARD) && TCDiscPriestSettings.Instance.UseFearWard && tank.InLineOfSight) 
			{
				LogActivity(FEAR_WARD, tank.Name);
				return Cast(FEAR_WARD, tank);
			}
			
            return false;
		}
		
        private bool isAuraActive(string name)
        {
            return isAuraActive(name, Me);
		}
		
        private bool isAuraActive(string name, WoWUnit unit)
        {
			bool isAuraActive = unit.Auras.ContainsKey(name);
			
			if(TRACE) LogActivity(TRACE_LABEL, name, (isAuraActive ? "[ACTIVE]" : "[NOT ACTIVE]"));
			
            return isAuraActive;
		}
		
		public override bool NeedRest
        {
            get
            {
                if (Me.ManaPercent <= TCDiscPriestSettings.Instance.ManaPercent &&
				!isAuraActive(DRINK) && !Me.Combat && !Me.IsMoving && !Me.IsCasting)
                {
                    LogActivity("Drinking");
					Styx.Logic.Common.Rest.Feed();
                    return true;
				}
                if (Me.HealthPercent <= TCDiscPriestSettings.Instance.HealthPercent &&
				!isAuraActive(FOOD) && !Me.Combat && !Me.IsMoving && !Me.IsCasting)
                {
                    LogActivity("Eating");
					Styx.Logic.Common.Rest.Feed();
                    return true;
				}
				
                return false;
			}
		}
		
        public override bool NeedPullBuffs { get { Pulse(); return false; } }
		
        public override bool NeedCombatBuffs { get { Pulse(); return false; } }
		
        public override bool NeedPreCombatBuffs { get { Pulse(); return false; } }
		
		private void LogActivity(params string[] words)
		{
			if (LOGGING)
			Logging.Write(String.Join(": ", words));
		}
		
        public override sealed string Name { get { return "TCDiscPriest v1.0.0"; } }
		
        public override WoWClass Class { get { return WoWClass.Priest; } }
		
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
	}
}