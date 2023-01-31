using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using TreeSharp;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using Styx;
using Styx.WoWInternals;

namespace FelMaster
{
    public class SimCraftBase
    {
        public static LocalPlayer Me { get { return StyxWoW.Me; } }

        public SimCraftBase()
        {
            BotEvents.OnBotStarted += OnStarted;
            BotEvents.OnBotStopped += OnStopped;
        }

        private void OnStarted(object o)
        {
            Log("Events attached.");
            Lua.Events.AttachEvent("UNIT_SPELLCAST_SUCCEEDED", OnSpellFired);
            Lua.Events.AttachEvent("UNIT_SPELLCAST_INTERRUPTED", OnSpellFired);
            Lua.Events.AttachEvent("UNIT_SPELLCAST_FAILED", OnSpellFired);
            Lua.Events.AttachEvent("UNIT_SPELLCAST_FAILED_QUIET", OnSpellFired);
            Lua.Events.AttachEvent("UNIT_SPELLCAST_STOP", OnSpellFired);
        }
        private void OnStopped(object o)
        {
            Lua.Events.DetachEvent("UNIT_SPELLCAST_SUCCEEDED", OnSpellFired);
            Lua.Events.DetachEvent("UNIT_SPELLCAST_INTERRUPTED", OnSpellFired);
            Lua.Events.DetachEvent("UNIT_SPELLCAST_FAILED", OnSpellFired);
            Lua.Events.DetachEvent("UNIT_SPELLCAST_FAILED_QUIET", OnSpellFired);
            Lua.Events.DetachEvent("UNIT_SPELLCAST_STOP", OnSpellFired);
        }

        private void OnSpellFired (object sender, LuaEventArgs raw)
        {
            var args = raw.Args;
            var player = Convert.ToString(args[0]);

            if (player != "player")
            {
                return;
            }

            string spellName = Convert.ToString(args[1]);
            if (_locks[spellName].Subtract(DateTime.Now).TotalSeconds > 30)
            {
                _locks[spellName] = DateTime.Now.AddSeconds(CLIENT_LAG);
                Log("Match: {0} => {1}", raw.EventName, spellName);
            }
        }

        private const double CLIENT_LAG = 1.0; //s
        
        public void Log(string s, params object[] args)
        {
            if (s != null)
                Logging.Write(s, args);
        }

        public Composite CastSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    return cond(a) && CanCast(name);
                },
                new Sequence(
                    new TreeSharp.Action(a => Log(label)),
                    new TreeSharp.Action(a => CastSpell(name))
                )
            );
        }		


		public static int HowManyMobsNearby(WoWUnit target, int range )
		{
			int rangeSqr = range * range;
			int cnt = 
			(from o in ObjectManager.ObjectList
			where o is WoWUnit && target.Location.DistanceSqr(o.Location) <= rangeSqr
			let u = o.ToUnit()
			where u.Combat && u.IsAlive && u.Attackable
			&& ((u.IsPlayer && u.ToPlayer().IsHorde != ObjectManager.Me.IsHorde) || (!u.IsFriendly))
			select u
			).Count();
			return cnt;
		}




        private Dictionary<string, DateTime> _locks = new Dictionary<string, DateTime>();
        public Composite CastDebuff(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    var X = true;
                    try
                    {
                        X = DateTime.Now.Subtract(_locks[name]).TotalSeconds > 0;
                    }
                    catch { }

                    return X &&
                        cond(a) && CanCast(name) && TargetDebuffTimeLeft(name) < DotDelta(name);
                },
                new Sequence(
                    new TreeSharp.Action(a => Log(label)),
                    new TreeSharp.Action(a => CastSpell(name)),
                    new TreeSharp.Action(a => _locks[name] = DateTime.Now.AddMinutes(1))
                )
            );
        }

        public Composite CastOffensiveBuff(string spell, string buff, double maxTimeLeft, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    var X = true;
                    try
                    {
                        X = DateTime.Now.Subtract(_locks[spell]).TotalSeconds > 0;
                    }
                    catch { }

                    return X && CanCast(spell) && PlayerBuffTimeLeft(buff) < maxTimeLeft;
                },
                new Sequence(
                    new TreeSharp.Action(a => Log(label == null ? null : (label + " (time left = " + PlayerBuffTimeLeft(buff) + ")"))),
                    new TreeSharp.Action(a => CastSpell(spell)),
                    new TreeSharp.Action(a => _locks[spell] = DateTime.Now.AddMinutes(1))
                )
            );
        }
	
	public bool IsNotCCed()
	{
		return (!TargetHasDebuff("Gouge") || !TargetHasDebuff("Blind") || !TargetHasDebuff("Hungering Cold") || !TargetHasDebuff("Hibernate") || !TargetHasDebuff("Freezing Trap") || !TargetHasDebuff("Wyvern Sting") || TargetHasDebuff("Bad Manner") || TargetHasDebuff("Polymorph") || TargetHasDebuff("Ring of Frost") || TargetHasDebuff("Repentance") || TargetHasDebuff("Shackle Undead") || TargetHasDebuff("Sap") || TargetHasDebuff("Hex") || TargetHasDebuff("Cyclone") || TargetHasDebuff("Scatter Shot") || TargetHasDebuff("Entrapment") || TargetHasDebuff("Dragon's Breath") || TargetHasDebuff("Mind Control") || TargetHasDebuff("Bind Elemental"));
	}

	public bool TargetNeedsShiv()
	{
		return (SpellManager.HasSpell("Shiv") && TargetHasBuff("Unholy Frenzy") || TargetHasBuff("Vengeance") || TargetHasBuff("Enrage") || TargetHasBuff("Savage Roar") || TargetHasBuff("Owlkin Frenzy") || TargetHasBuff("Berserker Rage") || TargetHasBuff("Wrecking Crew") || TargetHasBuff("Death Wish") || TargetHasBuff("Bastion of Defense"));
	}
	
	public bool Cloakdatshit()
	{
		return (SpellManager.HasSpell("Cloak of Shadows") && PlayerHasDebuff("Combustion"));
	}

	public bool NeedsCombatReadiness()
	{
		return (SpellManager.HasSpell("Combat Readiness") && (PlayerHasDebuff("Deep Wounds") || PlayerHasDebuff("Rend") || PlayerHasDebuff("Garrote") || PlayerHasDebuff("Rupture") || PlayerHasDebuff("Lacerate") || PlayerHasDebuff("Rip") || PlayerHasDebuff("Rake") || PlayerHasDebuff("Pounce") || PlayerHasDebuff("Piercing Shots")));
	}

	public bool TargetNeedsDismantle()
	{
		return (SpellManager.HasSpell("Dismantle") && (!TargetHasDebuff("Cheap Shot") || !TargetHasDebuff("Kidney Shot")) && Me.CurrentTarget.IsPlayer && (Me.CurrentTarget.Class == WoWClass.DeathKnight || Me.CurrentTarget.Class == WoWClass.Rogue || Me.CurrentTarget.Class == WoWClass.Warrior || Me.CurrentTarget.Class == WoWClass.Paladin || Me.CurrentTarget.Class == WoWClass.Hunter));
	}

	public bool TargetNeedsRupture()
	{
		return (SpellManager.HasSpell("Rupture") && Me.CurrentTarget.Class == WoWClass.Rogue);
	}

	public bool TargetNeedsGarrote()
	{
		return (SpellManager.HasSpell("Garrote") && Me.CurrentTarget.IsPlayer && (Me.CurrentTarget.Class == WoWClass.Priest || Me.CurrentTarget.Class == WoWClass.Druid || Me.CurrentTarget.Class == WoWClass.Shaman || Me.CurrentTarget.Class == WoWClass.Paladin || Me.CurrentTarget.Class == WoWClass.Warlock || Me.CurrentTarget.Class == WoWClass.Mage));
	}

	public bool IsTargetBoss() 
	{ 
		if (Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss || (Me.CurrentTarget.Level >= 85 && Me.CurrentTarget.Elite) && Me.CurrentTarget.MaxHealth > 3500000)
		return true; 

		else return false; 
	}  

        public bool PlayerHasBuff(string name)
        {
            return PlayerBuffTimeLeft(name) > 0;
        }

        public bool PlayerHasDebuff(string name)
        {
            return PlayerDebuffTimeLeft(name) > 0;
        }

        public bool TargetHasDebuff(string name)
        {
            return TargetDebuffTimeLeft(name) != 0; // <0 means passive aura; >0 means active one
        }

        public bool TargetHasBuff(string name)
        {
            return TargetBuffTimeLeft(name) != 0; // <0 means passive aura; >0 means active one
        }

        public double DotDelta(string name)
        {
            return 2.0;
        }

        public double CastTime(string name)
        {
            var s = SpellManager.Spells[name];
            return (double)s.CastTime / 1000.0;
        }

        public bool CanCast(string name)
        {
            return (!Me.IsMoving || CastTime(name) == 0) && IsUsableSpell(name) && SpellCooldown(name) <= 0.5;
        }

        public void CastSpell(string name)
        {
            Logging.WriteDebug("CastSpell({0})", name);
            Lua.DoString(string.Format("CastSpellByName(\"{0}\")", Lua.Escape(name)));
        }

        public double SpellCooldown(string name)
        {
            var lua = string.Format("local x,y=GetSpellCooldown(\"{0}\"); return x+y-GetTime()", Lua.Escape(name));
            try
            {
                return double.Parse(Lua.GetReturnValues(lua)[0]);
            }
            catch
            {
                Log("Lua failed in SpellCooldown: " + lua);
                return 99999;
            }
        }

        public bool IsUsableSpell(string name)
        {
            var lua = string.Format("local x=select(1, IsUsableSpell(\"{0}\")); if x==nil then return 0 else return 1 end", Lua.Escape(name));
            try
            {
                return Lua.GetReturnValues(lua)[0] == "1";
            }
            catch
            {
                Log("Lua failed in IsUsableSpell: " + lua);
                return false;
            }
        }

        public double PlayerBuffTimeLeft(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitBuff('player', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", Lua.Escape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Log("Lua failed in AuraTimeLeft");
                return 999999;
            }
        }

        public double PlayerDebuffTimeLeft(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitDebuff('player', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", Lua.Escape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Log("Lua failed in AuraTimeLeft");
                return 999999;
            }
        }

        public double TargetDebuffTimeLeft(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitDebuff(\"target\", \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", Lua.Escape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Log("Lua failed in TargetAuraTimeLeft");
                return 999999;
            }
        }

        public double TargetBuffTimeLeft(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitBuff(\"target\", \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", Lua.Escape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Log("Lua failed in TargetAuraTimeLeft");
                return 999999;
            }
        }
		
        public Composite CastBuff(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    return !PlayerHasBuff(name) && cond(a) && SpellManager.CanBuff(name, Me);
                },
                new Sequence(
                    new TreeSharp.Action(a => Log(label)),
                    new TreeSharp.Action(a => CastSpell(name))
                )
            );
        }

    }
}
