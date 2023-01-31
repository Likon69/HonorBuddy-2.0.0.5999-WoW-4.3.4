using System.Linq;
using CLU.Helpers;
using CLU.Lists;
using TreeSharp;

namespace CLU.Classes.DeathKnight
{
    using global::CLU.GUI;

    class Frost : RotationBase
    {
        public override string Name
        {
            get { return "MasterFrost Deathknight"; }
        }

        public override float CombatMaxDistance
        {
            get { return 3.2f; }
        }

        // adding some help
        public override string Help
        {
            get
            {
                return "\n" +
                       "----------------------------------------------------------------------\n" +
                       "Masterfrost: HB OBL Mastery. It is the best dps but hard.\n" +
                       "[*] Mastery > Haste\n" +
                       "[*] Unholy runes are gamed to force RE procs on blood/frost\n" +
                       "[*] HB is prioritised unless resources start to stack high\n" +
                       "[*] during high resources, OBL prioritises to keep runes used\n" +
                       "This Rotation will:\n" +
                       "1. Heal using AMS, IBF, Healthstone and Deathstrike < 40%\n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Pillar of Frost & Raise Dead & Death and Decay & Empower Rune Weapon \n" +
                       "3. Maintain HoW only if similar buff is not present\n" +
                       "4. Ensure we are in Unholy Presence (Commented out)\n" +
                       "5. Brez players (non-specific) using Raise Ally\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to Weischbier, ossirian, imdasandman and cowdude\n" +
                       "----------------------------------------------------------------------\n";
            }
        }

        public override string KeySpell
        {
            get { return "Howling Blast"; }
        }

        public override Composite SingleRotation
        {
            get
            {
			return new Decorator(ret => !SettingsFile.Instance.AutoManagePauseRotation, 
                new Sequence(
					new PrioritySelector(
					    new Decorator(
					        ret => Units.IsTargetWorthy(Me.CurrentTarget),
					            new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
								        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Item.UseEngineerGloves())),
					    // Extra button automation
                        Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
					    Buff.CastBuff("Pillar of Frost", ret => Units.IsTargetWorthy(Me.CurrentTarget), "Pillar of Frost"),
					    // Interupts
					    Spells.CastInterupt("Mind Freeze",      ret => true, "Mind Freeze"),
					    Spells.CastInterupt("Strangulate",      ret => true, "Strangulate"),
                        Buff.CastBuff("Anti-Magic Shell",       ret => (Me.CurrentTarget.IsCasting || Me.CurrentTarget.ChanneledCastingSpellId != 0) && Me.CurrentTarget.IsTargetingMeOrPet, "AMS"),
					    Spells.CastSelfSpell("Blood Tap",       ret => Spell.RuneCooldown(1) > 2 && Spell.RuneCooldown(2) > 2, "Blood Tap"),
                        Buff.CastBuff("Raise Dead",             ret => Units.IsTargetWorthy(Me.CurrentTarget) && Buff.PlayerHasBuff("Pillar of Frost") && Buff.PlayerBuffTimeLeft("Pillar of Frost") <= 10 && Buff.PlayerHasBuff("Unholy Strength"), "Raise Dead"),
					    Spells.CastSpell("Outbreak",            ret => Buff.TargetDebuffTimeLeft("Blood Plague").TotalSeconds < 0.5 || Buff.TargetDebuffTimeLeft("Frost Fever").TotalSeconds < 0.5, "Outbreak"),
						Spells.CastSpell("Howling Blast",       ret => Buff.TargetDebuffTimeLeft("Frost Fever").TotalSeconds < 0.5, "Howling Blast (Frost Fever)"),
						Spells.CastSpell("Plague Strike",       ret => Buff.TargetDebuffTimeLeft("Blood Plague").TotalSeconds < 0.5, "Plague Strike"),
						// Start AoE------------------------------------------------------------------------------------------------
                        Spells.CastAreaSpell("Howling Blast", 10, false, 3, 0.0, 0.0, ret => (Me.FrostRuneCount >= 1 || Me.DeathRuneCount >= 1) && Unit.EnemyUnits.Count() >= 3, "Howling Blast"),
                        Spells.CastAreaSpell("Death and Decay", 10, true, 3, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.UnholyRuneCount == 2 && Unit.EnemyUnits.Count() >= 3 && !Me.IsMoving && !Me.CurrentTarget.IsMoving && Units.IsTargetWorthy(Me.CurrentTarget), "Death and Decay"),
                        Spells.CastAreaSpell("Death and Decay", 10, true, 3, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && (Spell.RuneCooldown(4) == 0 && Spell.RuneCooldown(3) <= 1) || (Spell.RuneCooldown(3) == 0 && Spell.RuneCooldown(4) <= 1) && Unit.EnemyUnits.Count() >= 3 && !Me.IsMoving && !Me.CurrentTarget.IsMoving && Units.IsTargetWorthy(Me.CurrentTarget), "Death and Decay"),
                        Spells.CastAreaSpell("Plague Strike", 10, false, 3, 0.0, 0.0, ret => Spells.SpellCooldown("Death and Decay").TotalSeconds > 6 && Me.UnholyRuneCount == 2 && Unit.EnemyUnits.Count() >= 3, "Plague Strike"),
						// End AoE------------------------------------------------------------------------------------------------
                        Spells.CastSpell("Obliterate",          ret => Me.DeathRuneCount >= 1 && Me.FrostRuneCount >= 1 && Me.UnholyRuneCount >= 1 && Unit.EnemyUnits.Count() == 1, "Obliterate 1st"),
						Spells.CastSpell("Frost Strike",        ret => Me.CurrentRunicPower >= 120, "Frost Strike (Runic Power 120)"),
                        Spells.CastSpell("Obliterate",          ret => (Me.DeathRuneCount == 2 && Me.FrostRuneCount == 2) || (Me.DeathRuneCount == 2 && Me.UnholyRuneCount == 2) || (Me.UnholyRuneCount == 2 && Me.FrostRuneCount == 2) && Unit.EnemyUnits.Count() == 1, "Obliterate 2nd"),
						Spells.CastSpell("Frost Strike",        ret => Me.CurrentRunicPower >= 110, "Frost Strike (Runic Power 110)"),
						Spells.CastSpell("Howling Blast",       ret => Buff.PlayerHasBuff("Freezing Fog"), "Howling Blast (Rime)"),
						Spells.CastSpell("Frost Strike",        ret => Me.CurrentRunicPower >= 100, "Frost Strike (Runic Power 100)"),
						Spells.CastSpell("Obliterate",          ret => Buff.PlayerHasBuff("Killing Machine"), "Obliterate (Killing Machine)"),
						Spells.CastSpell("Obliterate",          ret => Me.UnholyRuneCount == 2, "Obliterate (2x Unholy Runes)"),
						Spells.CastSpell("Obliterate",          ret => (Spell.RuneCooldown(4) == 0 && Spell.RuneCooldown(3) <= 1) || (Spell.RuneCooldown(3) == 0 && Spell.RuneCooldown(4) <= 1), "Obliterate (Unholy Rune less than 1 second)"),
						Spells.CastSpell("Obliterate",          ret => (Spell.RuneCooldown(4) == 0 && Spell.RuneCooldown(3) < 4) || (Spell.RuneCooldown(3) == 0 && Spell.RuneCooldown(4) < 4) && (Me.FrostRuneCount + Me.DeathRuneCount == 1), "Obliterate (Unholy Rune less than 4 seconds)"),
						// Start AoE------------------------------------------------------------------------------------------------
                        Spells.CastAreaSpell("Frost Strike", 10, false, 3, 0.0, 0.0, ret => Buff.PlayerHasBuff("Killing Machine") && Unit.EnemyUnits.Count() >= 3, "Frost Strike"),
						// End AoE------------------------------------------------------------------------------------------------
						Spells.CastSpell("Howling Blast",       ret => true, "Howling Blast"),
                        Spells.CastSpell("Howling Blast",       ret => Me.CurrentRunicPower < 60 && !Buffs.UnitHasHasteBuff(Me), "Howling Blast (under 80 Runic Power)"),
						// Start More AoE------------------------------------------------------------------------------------------------
                        Spells.CastAreaSpell("Death and Decay", 10, true, 3, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.UnholyRuneCount == 1 && Unit.EnemyUnits.Count() >= 3 && !Me.IsMoving && !Me.CurrentTarget.IsMoving && Units.IsTargetWorthy(Me.CurrentTarget), "Death and Decay"),
                        Spells.CastAreaSpell("Plague Strike", 10, false, 3, 0.0, 0.0, ret => Spells.SpellCooldown("Death and Decay").TotalSeconds > 6 && Me.UnholyRuneCount == 1 && Unit.EnemyUnits.Count() >= 3, "Plague Strike"),
						// End More AoE------------------------------------------------------------------------------------------------
                        Spells.CastSpell("Obliterate",          ret => Me.CurrentRunicPower >= 60 && Buffs.UnitHasHasteBuff(Me), "Obliterate (over 80 Runic Power)"),
						Spells.CastSpell("Frost Strike",        ret => true, "Frost Strike"),
                        Spells.CastSelfSpell("Empower Rune Weapon", ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Spell.RuneCooldown(1) + Spell.RuneCooldown(2) + Spell.RuneCooldown(3) + Spell.RuneCooldown(4) + Spell.RuneCooldown(5) + Spell.RuneCooldown(6)) > 8 && !Buffs.UnitHasHasteBuff(Me), "Empower Rune Weapon"),
                        Buff.CastBuff("Horn of Winter",         ret => Me.CurrentRunicPower < 32 || !Buffs.UnitHasStrAgiBuff(Me), "Horn of Winter for RP"))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing,
                    new PrioritySelector(
                        Spells.CastSelfSpell("Death Pact",              ret => Me.HealthPercent < 60 && Me.GotAlivePet, "Death Pact"),
                        Spells.CastSelfSpell("Raise Dead",              ret => Me.HealthPercent < 60 && !Buff.PlayerHasBuff("Icebound Fortitude"), "Raise Dead"),
                        Spells.CastSelfSpell("Icebound Fortitude",      ret => Me.HealthPercent < 60, "Icebound Fortitude "),
                        Spells.CastSpell("Death Strike",                ret => Me.HealthPercent < 40, "Death Strike"),
                        Items.UseBagItem("Healthstone", 				ret => Me.HealthPercent < 40, "Healthstone")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return
                    new Decorator(
                        ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                        new PrioritySelector(
                            Buff.CastBuff("Horn of Winter", ret => !Buffs.UnitHasStrAgiBuff(Me), "Horn of Winter")));
            }
        }

        public override Composite PVPRotation
        {
            get { return SingleRotation; }
        }

        public override Composite PVERotation
        {
            get { return SingleRotation; }
        }

    }
}