using System;
using System.Collections.Generic;
using System.Linq;

using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace CLU.Helpers
{
    using Styx.Combat.CombatRoutine;
    using Styx.WoWInternals;

    using global::CLU.GUI;

    public class Buff
    {
        /* putting all the Buff/Debuff logic here */

        private static readonly Buff BuffInstance = new Buff();

        /// <summary>
        /// An instance of the Buff Class
        /// </summary>
        public static Buff Instance { get { return BuffInstance; } }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private readonly HashSet<string> hasteBuffs = new HashSet<string> {
            "Bloodlust",                      //Shaman
            "Heroism",                        //Shaman
            "Time Warp",                      //Mage
            "Ancient Hysteria",               //Hunter (Core Hound)
            "Unholy Frenzy",                  //Unholy DeathKnight
            "Rapid Fire",                     //Hunter (MM)
        };

        private readonly HashSet<string> bleedDamageDebuffs = new HashSet<string>{
            "Blood Frenzy",                   //Warrior (Arms)
            "Gore",                           //Hunter (Boar)
            "Stampede",                       //Hunter (Rhino)
            "Tendon Rip",                     //Hunter (Hyena)
            "Hemorrhage",                     //Rogue (Subtlety)
            "Mangle",                          //Druid Kitty
        };

        private readonly HashSet<string> damageReductionDebuffs = new HashSet<string>{
            "Demoralizing Shout",             //Warrior
            "Demoralizing Roar",              //Druid Bear
            "Vindication",                    //Paladin
            "Scarlet Fever",                  //Blood DeathKnight   
            "Curse of Weakness",              //Warlock
            "Demoralizing Screech",            //Hunter (Carrion Bird)
        };

        private readonly HashSet<string> attackSpeedDebuffs = new HashSet<string>{
            "Judgements of the Just",         //Paladin
            "Thunder Clap",                   //Warrior
            "Infected Wounds",                //Druid Feral
            "Earth Shock",                    //Shaman Totem
            "Frost Fever",                     //Deathknight
        };

        private readonly HashSet<string> armorReductionDebuffs = new HashSet<string>{
            "Expose Armor",         		  //Warrior
            "Sunder Armor",                   //Warrior (Protection)
            "Tear Armor",                	  //Hunter (Raptor)
            "Corrosive Spit",                 //Hunter (Serpent)
            "Faerie Fire",                     //Druid Feral
        };

        private readonly HashSet<string> strAgiStaIntBuffs = new HashSet<string>{
            "Mark of the Wild",               //Druid
            "Embrace of the Shale Spider",    //Hunter (Shale Spider) 
            "Blessing of Kings",               //Paladin
        };

        private readonly HashSet<string> staminaBuffs = new HashSet<string>{
            "Qiraji Fortitude",               //Hunter (Silithid)
            "Power Word: Fortitude",          //Priest
            "Blood Pact",                     //Warlock ( Summon Imp)
            "Commanding Shout",                //Warrior
        };

        private readonly HashSet<string> manaBuffs = new HashSet<string>{
            "Arcane Brilliance",              //Mage
            "Dalaran Brilliance",             //Mage
            "Fel Intelligence",                //Warlock ( Summon Felhunter)
        };

        private readonly HashSet<string> strAgiBuffs = new HashSet<string>{
            "Horn of Winter",                 //DeathKnight
            "Roar of Courage",                //Hunter (Cat/Spirit Beast)
            "Fel Intelligence",               //Warlock ( Summon Felhunter)
            "Strength of Earth Totem",        //Shaman
            "Battle Shout",                    //Warrior
        };

        private readonly HashSet<string> mp5Buffs = new HashSet<string>{
            "Blessing of Might",              //Paladin
            "Mana Spring Totem",              //Shaman
            "Fel Intelligence",                //Warlock ( Summon Felhunter)
        };

        private readonly HashSet<string> attackPowerBuffs = new HashSet<string>{
            "Abomination's Might",            //DeathKnight
            "Trueshot Aura",                  //Hunter (Marksmanship)
            "Blessing of Might",              //Paladin
            "Unleashed Rage",                  //Shaman (Enhancement)
        };

        private readonly HashSet<string> sixPercentSpellPowerBuffs = new HashSet<string>{
            "Arcane Brilliance",              //Mage
            "Dalaran Brilliance",             //Mage
            "Flametongue Totem",               //Shaman
        };

        private readonly HashSet<string> tenPercentSpellPowerBuffs = new HashSet<string>{
            "Totemic Wrath",                  //Shaman (Elemental)
            "Demonic Pact",                    //Warlock (Demonology)
        };

        private readonly HashSet<string> fivePercentSpellHasteBuffs = new HashSet<string>{
            "Moonkin Aura",                   //Druid (Balance)
            "Mind Quickening",                //Priest (Shadow)
            "Wrath of Air Totem",              //Shaman
        };

        private readonly HashSet<string> tenPercentHasteBuffs = new HashSet<string>{
            "Hunting Party",                   //Hunter (Survival)
            "Improved Icy Talons",             //Death Knight (Frost)
            "Windfury Totem",                   //Shaman
        };

        private readonly HashSet<string> fivePercentCritBuffs = new HashSet<string>{
            "Leader of the Pack",             //Druid (Feral) 
            "Terrifying Roar",                //Hunter (Devilsaur)
            "Furious Howl",                   //Hunter (Wolf)
            "Honor Among Thieves",            //Rogue (Subtlety)
            "Elemental Oath",                 //Shaman (Elemental)
            "Rampage",                         //Warrior (Fury)
        };

        private readonly HashSet<string> magicVulnerabilityDeBuffs = new HashSet<string>{
            // "Master Poisoner",                //Rogue (Assassination) 
            "Curse of the Elements",          //ALL warlocks (Supperior as it also lowers resistances by 183 were the other classes have no other affect)
            // "Ebon Plaguebringer",             //Death Knight (unholy)
            // "Earth and Moon",                 //Druid (Balance)
            // "Fire Breath",                    //Hunter (dragonhawk)
            // "Lightning Breath",               //Hunter (wind serpent)
        };

        /// <summary>
        /// Use this to print all auras
        /// </summary>
        /// Auras = All auras, including buffs, debuffs and hidden skill mechanics (enchants, etc.)
        /// Passive auras = buffs without any duration
        /// Active auras = mostly buffs and debuffs
        public static void DumpAuras()
        {
            foreach (KeyValuePair<string, WoWAura> sp in Me.Auras)
            {
                if (Me.Auras.ContainsKey(sp.Key))
                {
                    CLU.DebugLog(sp.Key + " " + Me.Auras[sp.Key].Flags.ToString());
                }
                else
                {
                    CLU.DebugLog(sp.Key);
                }
            }
        }

        /// <summary>Returns true if the Unit is under the affects of Haste.</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has haste buff.</returns>
        public bool UnitHasHasteBuff(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.hasteBuffs.Contains(x.Value.Name));
        }

        /// <summary>Returns true if the Unit has Stamina Buffs</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has bleed damage debuff.</returns>
        public bool UnitHasMagicVulnerabilityDeBuffs(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.magicVulnerabilityDeBuffs.Contains(x.Value.Name) && Me.Class == WoWClass.Warlock);
        }

        /// <summary>Returns true if the Unit has Stamina Buffs</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has bleed damage debuff.</returns>
        public bool UnitHasStaminaBuffs(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.staminaBuffs.Contains(x.Value.Name)); 
        }

        /// <summary>Returns true if the Unit has the 30% additional damage from Bleed effects</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has bleed damage debuff.</returns>
        public bool UnitHasBleedDamageDebuff(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.bleedDamageDebuffs.Contains(x.Value.Name));   
        }

        /// <summary>Returns true if the Unit has the 10% damage reduction debuff</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has damage reduction debuff.</returns>
        public bool UnitHasDamageReductionDebuff(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.damageReductionDebuffs.Contains(x.Value.Name));
        }
        

        /// <summary>Returns true if the Unit has the Armor Reduction debuff</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has armor reduction debuff.</returns>
        public bool UnitHasArmorReductionDebuff(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.armorReductionDebuffs.Contains(x.Value.Name));
        }

        /// <summary>Returns true if the Unit has the Strength and Agility Buff</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has str agi buff.</returns>
        public bool UnitHasStrAgiBuff(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.strAgiBuffs.Contains(x.Value.Name));
        }

        /// <summary>Returns true if the Unit has the Attack Speed debuff</summary>
        /// <param name="unit">The unit to check the for.</param>
        /// <returns>The unit has attack speed debuff.</returns>
        public bool UnitHasAttackSpeedDebuff(WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => this.attackSpeedDebuffs.Contains(x.Value.Name));
        }

        /// <summary>
        /// Used to  provide the time at which to attempt to refresh our debuff
        /// </summary>
        /// <returns>Spell cast time + GCD + Client Lag</returns>
        public static double DotDelta(string name)
        {
            return (Spell.Instance.CastTime(name) + Spell.GCD) + CombatLogEvents.ClientLag;
        }

        // TODO: Decide if we need to individually return seconds left to refresh the spell.


        /// <summary>Checks for the auras on a specified unit. Returns true if the unit has any aura in the auraNames list.</summary>
        /// <param name="unit">The unit to check auras for.</param>
        /// <param name="aura">Aura names to be checked.</param>
        /// <param name="creator">Check for only self or all buffs</param>
        /// <returns>The has aura.</returns>
        public static bool HasAura(WoWUnit unit, string aura, WoWUnit creator)
        {
            return unit != null && unit.GetAllAuras().Any(a => a.Name == aura && (creator == null || a.CreatorGuid == creator.Guid));
        }

        /// <summary>Returns the timeleft of an aura by TimeSpan. Return TimeSpan.Zero if the aura doesn't exist.</summary>
        /// <param name="unit">The unit to check the aura for.</param>
        /// <param name="auraName">The name of the aura in English.</param>
        /// <param name="fromMyAura"> true if its from my aura</param>
        /// <returns>The get aura time left.</returns>
        public static TimeSpan GetAuraTimeLeft(WoWUnit unit, string auraName, bool fromMyAura)
        {
            if (unit != null)
            {
                WoWAura wantedAura = unit.GetAllAuras().FirstOrDefault(
                                        a => a.Name == auraName && a.Duration > 0 && (!fromMyAura || a.CreatorGuid == Me.Guid));
                return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;
            }

            // CLU.Instance.Log(" [GetAuraTimeLeft] Unit is null ");
            return TimeSpan.Zero;
        }

        /// <summary>Check the aura stack count thats created by the specified unit</summary>
        /// <param name="unit">The unit to check auras for.</param>
        /// <param name="auraName">The name of the aura in English</param>
        /// <param name="fromMyAura">True if you applied the aura</param>
        /// <returns>The get aura stack.</returns>
        public static uint GetAuraStack(WoWUnit unit, string auraName, bool fromMyAura)
        {
            if (unit != null)
            {
                WoWAura stackCountAura =
                    unit.GetAllAuras().FirstOrDefault(
                        a => a.Name == auraName && a.StackCount > 0 && (!fromMyAura || a.CreatorGuid == Me.Guid));

                return stackCountAura != null ? stackCountAura.StackCount : 0;
            }

            // CLU.Instance.Log(" [GetAuraStack] Unit is null ");
            return 0;
        }

        /// <summary>
        /// Casts a specified buff given specific conditions
        /// </summary>
        /// <param name="name">The name of the buff to cast in english</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>true or false</returns>
        public static Composite CastBuff(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    if (PlayerHasBuff(name))
                        return false;
                    
                    if (!cond(a))
                        return false;

                    if (!SpellManager.CanBuff(name, Me))
                        return false;

                    return true;
                },
                new Sequence(
                    new TreeSharp.Action(a => CLU.Instance.Log(" [Buff] {0} ", label)),
                    new TreeSharp.Action(a => Spell.CastMySpell(name))));
        }

        /// <summary>
        /// Casts a specified debuff given specific conditions
        /// </summary>
        /// <param name="name">The name of the debuff to cast in english</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>true or false</returns>
        public static Composite CastDebuff(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a) 
                {
                    if (!cond(a))
                    {
                        return false;
                    }

                    if (TargetDebuffTimeLeft(name).TotalSeconds > DotDelta(name))
                    {
                        return false;
                    }

                    var lockstatus = true;
                    try
                    {
                        lockstatus = DateTime.Now.Subtract(CombatLogEvents.Locks[name]).TotalSeconds > 0;
                    }
                    catch { }

                    if (!Spell.Instance.CanCast(name, Me.CurrentTarget))
                    {
                        return false;
                    }

                    return lockstatus;
                },
                new Sequence(
                    new TreeSharp.Action(a => CLU.Instance.Log(" [Casting Debuff] {0} : (RefreshTime={1}) had {2} second(s) left", label, DotDelta(name), TargetDebuffTimeLeft(name).TotalSeconds)),
                    new TreeSharp.Action(a => Spell.CastMySpell(name)),
                    new TreeSharp.Action(a => CombatLogEvents.Locks[name] = DateTime.Now.AddSeconds(Spell.Instance.CastTime(name) * 1.5 + CombatLogEvents.ClientLag))));
        }


        /// <summary>
        /// Used to refresh buffs such as Improved Soul Fire via other buffs to increase uptime.
        /// </summary>
        /// <param name="spell">Spell to cast that will re-apply the helpful buff</param>
        /// <param name="buff">buff Time left</param>
        /// <param name="maxTimeLeft">maximum time left before refresh</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>true of false</returns>
        public static Composite CastOffensiveBuff(string spell, string buff, double maxTimeLeft, string label)
        {
            return new Decorator(
                delegate 
                {

                    var lockstatus = true;
                    try
                    {
                        lockstatus = DateTime.Now.Subtract(CombatLogEvents.Locks[spell]).TotalSeconds > 0;
                    }
                    catch { }


                    if (PlayerBuffTimeLeft(buff) > maxTimeLeft)
                    {
                        return false;
                    }

                    if (!Spell.Instance.CanCast(spell, Me.CurrentTarget))
                    {
                        return false;
                    }

                    return lockstatus;
                },
                new Sequence(
                    new TreeSharp.Action(
                        a => CLU.Instance.Log(
                            label == null ? null : " [Offensive Buff] {0} ({1} time left={2})",
                            label,
                            buff,
                            PlayerBuffTimeLeft(buff))),
                    new TreeSharp.Action(a => Spell.CastMySpell(spell)),
                    new TreeSharp.Action(a => CombatLogEvents.Locks[spell] = DateTime.Now.AddSeconds(Spell.Instance.CastTime(spell) * 1.5 + CombatLogEvents.ClientLag)))); // cast time * Max Flight time (from 40yards) + Clientlag = (2.54 * 1.5 + 1 = 4.81s lock for soul fire)
        }

        /// <summary>
        ///  Casts a raid buff if the buff is not currently applied on the player or the raid
        /// </summary>
        /// <param name="name">name of the buff to cast</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output !x.Buffs.ContainsKey(name)</param>
        /// <returns>true or false</returns>
        public static Composite CastRaidBuff(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
              delegate(object a)
                    {
                        if (!SettingsFile.Instance.AutoManageRaidPartyBuff)
                            return false;

                        if (!cond(a))
                            return false;

                        if (!SpellManager.CanBuff(name))
                            return false;

                  var players = new List<WoWPlayer> { Me };
                  if (Me.IsInRaid) players.AddRange(Me.RaidMembers);
                        else if (Me.IsInParty)
                            players.AddRange(Me.PartyMembers);

                  return
                      players.Any(x => x.Distance2DSqr < 40 * 40 && x.Auras.Values.All(ret => ret.Spell.Name != name) && !x.Dead && !x.IsGhost && x.IsFriendly);
              },
              new Sequence(
                    new TreeSharp.Action(a => CLU.Instance.Log(" [Raid Buff] {0} ", label)),
                    new TreeSharp.Action(a => Spell.CastMySpell(name))));
        }

        /// <summary>Returns the buff time left on the player</summary>
        /// <param name="name">name of the buff to check</param>
        /// <returns>The player buff time left.</returns>
        public static double PlayerBuffTimeLeft(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitBuff('player', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", Spell.RealLuaEscape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                CLU.Instance.Log("Lua failed in PlayerBuffTimeLeft");
                return 999999;
            }
            
            // return GetAuraTimeLeft(Me, name, true);
        }

        /// <summary>Returns the Active debuff time left on the player</summary>
        /// <param name="name">name of the debuff to check</param>
        /// <returns>The player debuff time left.</returns>
        public static TimeSpan PlayerDebuffTimeLeft(string name)
        {
            return GetAuraTimeLeft(Me, name, false);
        }

        /// <summary>Returns the buff time left on the player</summary>
        /// <param name="name">name of the buff to check</param>
        /// <returns>The player buff time left.</returns>
        public static TimeSpan PlayerActiveBuffTimeLeft(string name)
        {
            return GetAuraTimeLeft(Me, name, true);
        }


        /// <summary>Returns the buff count on the player</summary>
        /// <param name="name">name of the buff to check</param>
        /// <returns>The player count buff.</returns>
        public static uint PlayerCountBuff(string name)
        {
            return GetAuraStack(Me, name, true);
        }

        /// <summary>Returns true if the player has the buff</summary>
        /// <param name="name">the name of the buff to check for</param>
        /// <returns>The player has buff.</returns>
        public static bool PlayerHasBuff(string name)
        {
            return Me.HasAura(name);
        }

        /// <summary>Returns true if the player has the ACTIVE buff. Good for checking procs.</summary>
        /// <param name="name">the name of the active buff to check for</param>
        /// <returns>The player has active buff.</returns>
        public static bool PlayerHasActiveBuff(string name)
        {
            return Me.ActiveAuras.ContainsKey(name);
        }

        /// <summary>Returns the debuff time left on the target</summary>
        /// <param name="name">the name of the buff to check for</param>
        /// <returns>The target debuff time left.</returns>
        public static TimeSpan TargetDebuffTimeLeft(string name)
        {
            return GetAuraTimeLeft(Me.CurrentTarget, name, true);
        }

        /// <summary>Returns the debuff count on the target</summary>
        /// <param name="name">the name of the debuff to check for</param>
        /// <returns>The target count debuff.</returns>
        public static uint TargetCountDebuff(string name)
        {
            return GetAuraStack(Me.CurrentTarget, name, true);
        }

        /// <summary>
        /// Returns the buff count on the target
        /// </summary>
        /// <param name="name">the name of the buff to check</param>
        /// <returns>the target count buff</returns>
        public uint TargetCountBuff(string name)
        {
            return GetAuraStack(Me.CurrentTarget, name, false);
        }

        /// <summary>Returns true if the target has this debuff</summary>
        /// <param name="name">the name of the debuff to check for</param>
        /// <returns>The target has debuff.</returns>
        public static bool TargetHasDebuff(string name)
        {
            return HasAura(Me.CurrentTarget, name, Me);
        }

        /// <summary>Returns true if the target has this debuff</summary>
        /// <param name="name">the name of the debuff to check for</param>
        /// <param name="unit">the unit to check for</param>
        /// <returns>The target has debuff.</returns>
        public static bool TargetHasDebuff(string name, WoWUnit unit)
        {
            return unit != null && unit.ActiveAuras.Any(x => x.Value.Name == name);
        }
    }
}
