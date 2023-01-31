using System;
using System.Collections.Generic;
using System.Threading;
using Styx;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Zerfall
{
    public partial class Zerfall
    {

      
        public static int UnstableAffliction = 30108;
public static int AutoAttack = 6603;
public static int BaneofAgony = 980;
public static int BaneofDoom = 603;
public static int Banish = 710;
public static int ControlDemon = 93375;
public static int Cooking = 33359;
public static int Corruption = 172;
public static int CreateHealthstone = 6201;
public static int CreateSoulstone = 693;
public static int CurseoftheElements = 1490;
public static int CurseofTongues = 1714;
public static int CurseofWeakness = 702;
public static int DeathCoil = 6789;
public static int DemonArmor = 687;
public static int DemonLeap = 54785;
public static int DemonicCircleSummon = 48018;
public static int DemonicCircleTeleport = 48020;
public static int DemonicEmpowerment = 47193;
public static int DemonicKnowledge = 84740;
public static int DrainLife = 689;
public static int DrainSoul = 1120;
public static int EnslaveDemon = 1098;
public static int EscapeArtist = 20589;
public static int EyeofKilrogg = 126;
public static int Fear = 5782;
public static int FelArmor = 28176;
public static int HandofGuldan = 71521;
public static int HealthFunnel = 755;
public static int Hellfire = 1949;
public static int HowlofTerror = 5484;
public static int Immolate = 348;
public static int ImmolationAura = 50589;
public static int Incinerate = 29722;
public static int LifeTap = 1454;
public static int Metamorphosis = 47241;
public static int Nethermancy = 86091;
public static int RainofFire = 5740;
public static int RitualofSouls = 29893;
public static int RitualofSummoning = 698;
public static int SearingPain = 5676;
public static int SeedofCorruption = 27243;
public static int ShadowBolt = 686;
public static int ShadowWard = 6229;
public static int Shadowflame = 47897;
public static int Shoot = 5019;
public static int SoulFire = 6353;
public static int SoulHarvest = 79268;
public static int SoulLink = 19028;
public static int Soulburn = 74434;
public static int Soulshatter = 29858;
public static int SummonDoomguard = 18540;
public static int SummonFelguard = 30146;
public static int SummonFelhunter = 691;
public static int SummonImp = 688;
public static int SummonInfernal = 1122;
public static int SummonSuccubus = 712;
public static int SummonVoidwalker = 697;
public static int TheQuickandtheDead = 83950;
public static int UnendingBreath = 5697;

        internal class Spells
        {

            private static Spells _instance;
            public static Spells Instance
            {
                get { return _instance ?? (_instance = new Spells()); }
            }

            public WoWSpell this[string spellName]
            {
                get { return LegacySpellManager.KnownSpells.ContainsKey(spellName) ? LegacySpellManager.KnownSpells[spellName] : null; }
            }

            public void StopCasting()
            {
                Lua.DoString("SpellStopCasting()");
            }

            public bool CanCast(string spellname)
            {
                WoWSpell spell = this[spellname];

                if (spell == null || spell.Cooldown || LegacySpellManager.GlobalCooldown || StyxWoW.Me.IsCasting)
                    return false;

                if (Lua.GetReturnVal<int>("return IsUsableSpell(\"" + spell.Name + "\")", 0) != 1)
                    return false;

                return true;
            }

            public void Cast(string spell)
            {
                Log("Casting: " + this[spell].Name);
                this[spell].Cast();

                var sleepTime = Lua.GetReturnVal<int>("return GetNetStats()", 2);
                Thread.Sleep(sleepTime * 2);
            }




            /// <summary>
            /// Buffs ourselves with the specified spell.
            /// </summary>
            /// <param name="spell"></param>
            public void Buff(string spell)
            {
                Buff(spell, StyxWoW.Me, true);
            }

            /// <summary>
            /// Buffs the specified unit with a spell. Optionally targeting the current target.
            /// </summary>
            /// <param name="spell">The name of the spell to buff.</param>
            /// <param name="target">The unit to buff.</param>
            /// <param name="targetLast">Whether or not we should re-target our old target after buffing the unit.</param>
            public void Buff(string spell, WoWUnit target, bool targetLast)
            {
                var autoSelfCast = Lua.GetReturnVal<bool>("return GetCVar('autoSelfCast')", 0);

                // don't target self if we got AutoSelfCast on, or if target isn't me
                if (target != Me || target == Me && !autoSelfCast)
                    target.Target();

                // Quick sleep to allow WoW to update us
                Thread.Sleep(100);
                Log("Buffing " + target.Name + " with " + spell);
                this[spell].Cast();

                if (targetLast && (target != Me && !autoSelfCast))
                {
                    // Again... wait for WoW to update/cast
                    Thread.Sleep(100);
                    StyxWoW.Me.TargetLastTarget();
                    // Sheesh... so much waiting...
                    Thread.Sleep(100);
                }
            }

            /// <summary>
            /// Determines whether or not we have a spell.
            /// </summary>
            /// <param name="spell"></param>
            /// <returns></returns>
            public bool HasSpell(string spell)
            {
                return SpellManager.HasSpell(spell);
            }

            /// <summary>
            /// Determines if we can buff ourselves with the specified spell.
            /// </summary>
            /// <param name="spell"></param>
            /// <returns></returns>
            public bool CanBuff(string spell)
            {
                return CanBuff(spell, StyxWoW.Me);
            }

            /// <summary>
            /// Determines if we can buff the specified unit with the chosen spell.
            /// </summary>
            /// <param name="spell">The spell to check for</param>
            /// <param name="unit">The unit to check</param>
            /// <returns></returns>
            public bool CanBuff(string spell, WoWUnit unit)
            {
                return CanCast(spell) && !unit.Auras.ContainsKey(spell);
            }

            private readonly Random _rand = new Random();
            /// <summary>
            /// Casts a random spell at our current target (or ourselves)
            /// </summary>
            /// <param name="spells"></param>
            public void CastRandom(List<string> spells)
            {
                string curSpell = spells[_rand.Next(0, spells.Count)];
                while (spells.Count != 0 && !CanCast(curSpell))
                {
                    spells.Remove(curSpell);
                    curSpell = spells.Count != 0 ? spells[_rand.Next(0, spells.Count)] : null;
                }
                if (curSpell != null)
                {
                    this[curSpell].Cast();
                }
            }
        }
      
        private static bool GotPet()
        {
            List<string> HasPet = Lua.GetReturnValues("return HasPetUI(1)", "hawker.lua");
            if (HasPet[0] == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
