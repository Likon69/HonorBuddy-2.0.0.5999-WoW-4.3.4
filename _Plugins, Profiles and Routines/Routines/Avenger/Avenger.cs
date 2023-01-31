using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Avenger
{
    class Avenger : CombatRoutine
    {
        public Version mCurVersion = new Version(1, 7);

        public override sealed string Name { get { return "Avenger v" + mCurVersion; } }
        public override WoWClass Class { get { return WoWClass.Paladin; } }
        public override bool WantButton { get { return true; } }

        private static LocalPlayer Me { get { return ObjectManager.Me; } }

        public override void OnButtonPress()
        {
            var config = new AvengerConfig();
            config.Show();
        }

        public override void Initialize()
        {
            Logging.Write("");
            Logging.Write(Color.Crimson, "Avenger v{0} CC by Shaddar & Venus112", mCurVersion);
            Logging.Write(Color.Crimson, "with improvements by fiftypence");
            Logging.Write(Color.Crimson, "Part of the Lazyraider CC project");
            Logging.Write("");
			Logging.Write(Color.Crimson, "Remember to comment on the forum!");
			Logging.Write(Color.Crimson, "/like and +rep if you like this CC!");
            Logging.Write("");
        }

        public override void PreCombatBuff()
        {
            if (!Me.Mounted && SpellManager.HasSpell("Seal of Truth") && !Me.HasAura("Seal of Truth"))
            {
                if (CastSpell("Seal of Truth"))
                {
                    Logging.Write(Color.Aqua, ">> Seal of Truth <<");
                }
            }
        }

        public override void Combat()
        {
            using (new FrameLock())
            {
                Stopwatch tickTimer = new Stopwatch();
                tickTimer.Start();

                PrintDebug();
                DoCombat();

                Logging.WriteDebug(Color.Orange, "END TICK -> {0} ms", tickTimer.ElapsedMilliseconds);
            }
        }  

        private void DoCombat()
        {
            int addCount = GetAddCount();
            bool haveAgro = CheckAgro();
            bool haveDp = Me.HasAura("Divine Purpose");

            HandleDsMechanics();

            if (Me.CurrentTarget != null && !Me.Mounted)
            {
                if (AvengerSettings.Instance.LoH && Me.HealthPercent <= 15 && !Me.ActiveAuras.ContainsKey("Forbearance") &&
                    CastSpell("Lay on Hands", Me))
                {
                    Logging.Write(Color.Crimson, ">> Lay on Hands!!! <<");
                    return;
                }

                if (haveAgro && !Me.ActiveAuras.ContainsKey("Forbearance") && Me.HealthPercent <= 30)
                {
                    if (AvengerSettings.Instance.DivineProtection &&
                        CastSpell("Divine Protection"))
                    {
                        Logging.Write(Color.Aqua, ">> Got aggro, using Divine Protection at " + Convert.ToInt16(Me.HealthPercent) + " Percent <<");
                        return;
                    }

                    if (AvengerSettings.Instance.DivineShield &&
                        CastSpell("Divine Shield"))
                    {

                        Logging.Write(Color.Aqua, ">> Got aggro, using Divine Shield at " + Convert.ToInt16(Me.HealthPercent) + " Percent <<");
                        return;
                    }
                }

                if (AvengerSettings.Instance.SOTR && addCount >= AvengerSettings.Instance.AoEMobs &&
                    !Me.Auras.ContainsKey("Seal of Righteousness") &&
                    CastSpell("Seal of Righteousness", Me))
                {
                    Logging.Write(Color.Blue, ">> Seal of Righteousness <<");
                    return;
                }

                if (addCount < AvengerSettings.Instance.AoEMobs &&
                    !Me.Auras.ContainsKey("Seal of Truth") &&
                    CastSpell("Seal of Truth", Me))
                {
                    Logging.Write(Color.Blue, ">> Seal of Truth <<");
                    return;
                }

                if (AvengerSettings.Instance.Rebuke && Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast &&
                    CastSpell("Rebuke"))
                {
                    Logging.Write(Color.White, ">> Rebuke <<");
                    return;
                }

                if (GetSpellCooldown("Crusader Strike") > 0.5 || Me.CurrentHolyPower == 3)
                {
                    if (IsTargetBoss())
                    {
                        if (AvengerSettings.Instance.GOAK && GetAuraTimeLeft("Inquisition") > 20 &&
                            (GetSpellCooldown("Zealotry") > 60 || GetSpellCooldown("Zealotry") < 10 ||
                            !AvengerSettings.Instance.Zealotry) &&
                            CastSpell("Guardian of Ancient Kings"))
                        {
                            Logging.Write(Color.Blue, ">> Guardian of Ancient Kings <<");
                            return;
                        }

                        if (AvengerSettings.Instance.Zealotry &&
                            (Me.CurrentHolyPower == 3 || haveDp) &&
                            (GetAuraTimeLeft("Guardian of Ancient Kings") <= 20 &&
                            GetSpellCooldown("Guardian of Ancient Kings") >= 15 ||
                            !AvengerSettings.Instance.GOAK) &&
                            CastSpell("Zealotry", Me))
                        {
                            Logging.Write(Color.Blue, ">> Zealotry <<");
                            return;
                        }

                        if (AvengerSettings.Instance.AvengingWrath &&
                            (GetAuraTimeLeft("Zealotry") > 0 || !AvengerSettings.Instance.Zealotry) &&
                            CastSpell("Avenging Wrath", Me))
                        {
                            Logging.Write(Color.Blue, ">> Avenging Wrath <<");
                            return;
                        }
                    }

                    if ((Me.CurrentHolyPower == 3 || haveDp) &&
                        GetAuraTimeLeft("Inquisition") < 5 &&
                        CastSpell("Inquisition"))
                    {
                        Logging.Write(Color.Aqua, ">> Inquisition <<");
                        return;
                    }

                    if ((Me.CurrentTarget.HealthPercent < 20 || Me.HasAura("Avenging Wrath")) &&
                        CastSpell("Hammer of Wrath"))
                    {
                        Logging.Write(Color.Aqua, ">> Hammer of Wrath <<");
                        return;
                    }

                    if (Me.ActiveAuras.ContainsKey("The Art of War") &&
                        CastSpell("Exorcism"))
                    {
                        Logging.Write(Color.Aqua, ">> Exorcism <<");
                        return;
                    }

                    if ((Me.CurrentHolyPower == 3 || haveDp) &&
                        CastSpell("Templar's Verdict"))
                    {
                        Logging.Write(Color.Aqua, ">> Templar's Verdict <<");
                        return;
                    }

                    if (CastSpell("Judgement"))
                    {
                        Logging.Write(Color.Aqua, ">> Judgement <<");
                        return;
                    }

                    if (Me.ManaPercent >= 35 &&
                        CastSpell("Holy Wrath"))
                    {
                        Logging.Write(Color.Aqua, ">> Holy Wrath <<");
                        return;
                    }

                    if (Me.ManaPercent >= 85 &&
                        CastSpell("Consecration"))
                    {
                        Logging.Write(Color.Aqua, ">> Consecration <<");
                        return;
                    }

                    if (Me.ManaPercent <= 90 &&
                        CastSpell("Divine Plea"))
                    {
                        Logging.Write(Color.Blue, ">> Divine Plea <<");
                        return;
                    }
                }
                else
                {
                    if ((addCount < AvengerSettings.Instance.AoEMobs || !AvengerSettings.Instance.DivineStorm || IsTargetBoss()) &&
                        CastSpell("Crusader Strike"))
                    {
                        Logging.Write(Color.Aqua, ">> Crusader Strike <<");
                        return;
                    }

                    if (addCount >= AvengerSettings.Instance.AoEMobs && AvengerSettings.Instance.DivineStorm && !IsTargetBoss() &&
                        CastSpell("Divine Storm"))
                    {
                        Logging.Write(Color.Aqua, ">> Divine Storm <<");
                        return;
                    }
                }
            }
        }

        private void PrintDebug()
        {
            Logging.WriteDebug(Color.Orange, "START TICK");

            string tarInfo = "NULL";

            if (Me.CurrentTarget != null)
            {
                tarInfo = StyxWoW.Me.CurrentTarget.ToString();
            }

            Logging.WriteDebug(Color.DarkBlue, "DP:{0} AoW:{1}", Me.HasAura("Divine Purpose"), Me.ActiveAuras.ContainsKey("The Art of War"));
            Logging.WriteDebug(Color.DarkBlue, "HP:{0} MP:{1} Pow:{2}", Me.CurrentHealth, Me.CurrentMana, Me.CurrentHolyPower);
            Logging.WriteDebug(Color.DarkBlue, "Adds:{0}", GetAddCount());
            Logging.WriteDebug(Color.DarkBlue, tarInfo);
        }

        private bool CheckAgro()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(false).Any(u => u.Aggro && u.IsHostile && u.IsTargetingMeOrPet);
        }

        private void HandleDsMechanics()
        {
            if (CheckShrapnel() || CheckUltraxionFl() || CheckUltraxionHour())
            {
                Lua.DoString("RunMacroText('/click ExtraActionButton1');");
                Logging.Write(Color.Aqua, ">> Super magical DS button pressed! <<");
            }
        }

        private bool CheckUltraxionHour()
        {
            return !Me.ActiveAuras.ContainsKey("Divine Protection") &&
                   ObjectManager.GetObjectsOfType<WoWUnit>(false)
                        .Any(u => u.Guid != Me.Guid &&
                                  u.Attackable &&
                                  u.IsCasting &&
                                  u.CastingSpell.Name == "Hour of Twilight" &&
                                  u.CurrentCastTimeLeft.TotalSeconds <= 1.5);
        }

        private bool CheckUltraxionFl()
        {
            return Me.Auras.ContainsKey("Fading Light") &&
                   Me.Auras["Fading Light"].TimeLeft.TotalSeconds <= 2;
        }

        private bool CheckShrapnel()
        {
            return Me.Auras.ContainsKey("Shrapnel") &&
                   Me.Auras["Shrapnel"].TimeLeft.TotalSeconds <= 2;
        }

        // Make sure this works.
        private int GetAddCount()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(false)
                .Count(u => u.Guid != Me.Guid &&
                            u.IsHostile &&
                            !u.IsCritter &&
                            u.Distance <= 12 &&
                            u.Attackable &&
                            (u.IsTargetingMyPartyMember ||
                            u.IsTargetingMyRaidMember ||
                            u.IsTargetingMeOrPet));
        }

        // Code taken from MutaRaidBT with permission.
        // Might cause false positives.
        // Improve later.
        private bool IsTargetBoss()
        {
            return StyxWoW.Me.CurrentTarget.Level >= 87 &&
                   (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite ||
                    StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Rare ||
                    StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.RareElite ||
                    StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss);
        }

        private bool CastSpell(string spellName)
        {
            return CastSpell(spellName, Me.CurrentTarget);
        }

        private bool CastSpell(string spellName, WoWUnit target)
        {
            if (Me.CurrentTarget != null && CanCastSpell(spellName))
            {
                SpellManager.Cast(spellName, target);
                return true;
            }

            return false;
        }

        private bool CanCastSpell(string spellName)
        {
            return !IsGcdActive() && SpellManager.HasSpell(spellName) &&
                   Me.CurrentMana >= SpellManager.Spells[spellName].PowerCost &&
                   GetSpellCooldown(spellName) <= 0.25;
        }

        private double GetSpellCooldown(string spellName)
        {
            return SpellManager.HasSpell(spellName) ? SpellManager.Spells[spellName].CooldownTimeLeft.TotalSeconds : 0;
        }

        private double GetAuraTimeLeft(string auraName)
        {
            return GetAuraTimeLeft(auraName, Me);
        }

        private double GetAuraTimeLeft(string auraName, WoWUnit target)
        {
            return GetAuraTimeLeft(auraName, target, Me.Guid);
        }

        private double GetAuraTimeLeft(string auraName, WoWUnit target, UInt64 creatorGuid)
        {
            WoWAura selectedAura = target.GetAllAuras().FirstOrDefault(aura => aura.Name == auraName && aura.CreatorGuid == creatorGuid);

            if (selectedAura != null)
            {
                return selectedAura.TimeLeft.TotalSeconds;
            }

            return 0;
        }

        private bool IsGcdActive()
        {
            return SpellManager.GlobalCooldownLeft.TotalSeconds > 0.25;
        }
    }
}