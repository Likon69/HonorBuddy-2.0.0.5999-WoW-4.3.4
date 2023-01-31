using System.Linq;
using CLU.Helpers;
using CLU.Lists;
using TreeSharp;

namespace CLU.Classes.Deathknight
{
    using global::CLU.GUI;

    class Blood : RotationBase
    {
        // public static readonly HealerBase Healer = HealerBase.Instance;

        public override string Name
        {
            get { return "Blood Deathknight"; }
        }

        public override string KeySpell
        {
            get { return "Bone Shield"; }
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
                       "This Rotation will:\n" +
                       "1. Heal using AMS, DeathPact, Lichbourne Heal, IBF, Healthstone, RuneTap, and VB\n" +
                       "2. Intelligently Heal with Deathstrikes\n" +
                       "3. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Empower Rune Weapon \n" +
                       "4. Maintain HoW only if similar buff is not present\n" +
                       "5. Maintain Bone Shield\n" +
                       "6. Use Death and Decay and Pestilence for AoE\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to Weischbier, ossirian and cowdude and Singular\n" +
                       "----------------------------------------------------------------------\n";
            }
        }

        public override Composite SingleRotation
        {
            get
            {
                return new Decorator(ret => !SettingsFile.Instance.AutoManagePauseRotation, 
                    new Sequence(
                    new PrioritySelector(
                        // Trinkets & Cooldowns
                        new Decorator(
                            ret => Units.IsTargetWorthy(Me.CurrentTarget), 
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Item.UseEngineerGloves(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"))),
                    // Extra button automation
                    Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                    // Interupts
                    Spells.CastInterupt("Mind Freeze",              ret => true, "Mind Freeze"),
                    Spells.CastInterupt("Strangulate",              ret => true, "Strangulate"),
                    Buff.CastBuff("Anti-Magic Shell",               ret => (Me.CurrentTarget.IsCasting || Me.CurrentTarget.ChanneledCastingSpellId != 0) && Me.CurrentTarget.IsTargetingMeOrPet, "AMS"),
                    // START AoE + Disease spread
                    Spells.CastAreaSpell("Pestilence", 10, false, 3, 0.0, 0.0, ret => Buff.TargetHasDebuff("Blood Plague") && Buff.TargetHasDebuff("Frost Fever") && (from enemy in Unit.EnemyUnits where !enemy.HasAura("Blood Plague") && !enemy.HasAura("Frost Fever") select enemy).Any(), "Pestilence"),
                    Spells.CastAreaSpell("Death and Decay", 10, true, 3, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry), "Death and Decay"), 						
                    // START Main Rotation
                    Buff.CastDebuff("Outbreak", 					ret => !Buff.TargetHasDebuff("Frost Fever") || !Buff.TargetHasDebuff("Blood Plague"), "Outbreak for diseases"), 
                    Spells.CastSpell("Death Strike", 				ret => Me.HealthPercent < 40 || (Me.UnholyRuneCount + Me.FrostRuneCount + Me.DeathRuneCount >= 4) || (Me.HealthPercent < 90 && !Buff.PlayerHasBuff("Blood Shield")), "Death Strike"), 
                    Spells.CastSelfSpell("Blood Tap",               ret => Me.HealthPercent <= 90 && Me.BloodRuneCount >= 1 && (Me.UnholyRuneCount == 0 || Me.FrostRuneCount == 0), "Blood Tap"), 
                    Spells.CastSelfSpell("Rune Tap",                ret => Me.HealthPercent <= 80 && Me.BloodRuneCount >= 1, "Rune Tap"),
                    Buff.CastDebuff("Plague Strike",                ret => Spells.SpellCooldown("Outbreak").TotalSeconds > 3 && Me.HealthPercent > 50 && !Buffs.UnitHasDamageReductionDebuff(Me.CurrentTarget), "Plague Strike for Scarlet Fevor"),
                    Buff.CastDebuff("Icy Touch",                    ret => Spells.SpellCooldown("Outbreak").TotalSeconds > 3 && Me.HealthPercent > 50 && !Buffs.UnitHasAttackSpeedDebuff(Me.CurrentTarget), "Icy Touch for Frost Fever"),
                    Spells.CastAreaSpell("Blood Boil", 10, false, 3, 0.0, 0.0, ret => Me.BloodRuneCount >= 1, "Blood Boil"),
                    Spells.CastSpell("Heart Strike", 				ret => (Me.BloodRuneCount > 0 || Me.DeathRuneCount > 2), "Heart Strike"), 
                    Spells.CastSpell("Rune Strike", 				ret => (Me.CurrentRunicPower >= 60 || Me.HealthPercent > 90) && (Me.UnholyRuneCount == 0 || Me.FrostRuneCount == 0) && !Buff.PlayerHasBuff("Lichborne"), "Rune Strike"), 
                    Spells.CastSpell("Death Coil",                  ret => !Spells.CanCast("Rune Strike", Me.CurrentTarget) && Me.CurrentRunicPower >= 90, "Death Coil"),
                    Buff.CastBuff("Horn of Winter",                 ret => Me.CurrentRunicPower < 34 || !Buffs.UnitHasStrAgiBuff(Me), "Horn of Winter for RP"))));
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
                    Spells.CastSelfSpell("Rune Tap",                ret => Me.HealthPercent < 90 && Buff.PlayerHasBuff("Will of the Necropolis"), "Rune Tap & WotN "),
                    Spells.HealMe("Death Coil",                     ret => Me.HealthPercent < 70 && Buff.PlayerHasBuff("Lichborne"), "Death Coil Heal"),
                    Spells.CastSelfSpell("Bone Shield",             ret => Spells.SpellCooldown("Death Strike").TotalSeconds > 3 && !Buff.PlayerHasBuff("Vampiric Blood") && !Buff.PlayerHasBuff("Icebound Fortitude") && !Buff.PlayerHasBuff("Dancing Rune Weapon") && !Buff.PlayerHasBuff("Lichborne"), "Bone Shield"), 
                    Spells.CastSelfSpell("Vampiric Blood",          ret => Me.HealthPercent < 60 && !Buff.PlayerHasBuff("Dancing Rune Weapon") && !Buff.PlayerHasBuff("Bone Shield") && !Buff.PlayerHasBuff("Icebound Fortitude") && !Buff.PlayerHasBuff("Lichborne"), "Vampiric Blood"), 
                    Spells.CastSpell("Dancing Rune Weapon", 		ret => Me.HealthPercent < 80 && !Buff.PlayerHasBuff("Bone Shield") && !Buff.PlayerHasBuff("Icebound Fortitude") && !Buff.PlayerHasBuff("Vampiric Blood") && !Buff.PlayerHasBuff("Lichborne"), "Dancing Rune Weapon"), 
                    Spells.CastSelfSpell("Lichborne",               ret => Me.HealthPercent < 60 && Me.CurrentRunicPower >= 60 && !Buff.PlayerHasBuff("Bone Shield") && !Buff.PlayerHasBuff("Vampiric Blood") && !Buff.PlayerHasBuff("Icebound Fortitude") && !Buff.PlayerHasBuff("Dancing Rune Weapon"), "Lichborne"),
                    Spells.CastSelfSpell("Raise Dead",              ret => Me.HealthPercent < 60 && !Buff.PlayerHasBuff("Bone Shield") && !Buff.PlayerHasBuff("Vampiric Blood") && !Buff.PlayerHasBuff("Icebound Fortitude") && !Buff.PlayerHasBuff("Dancing Rune Weapon") && !Buff.PlayerHasBuff("Lichborne"), "Raise Dead"), 
                    Spells.CastSelfSpell("Icebound Fortitude",      ret => Me.HealthPercent < 60 && !Buff.PlayerHasBuff("Bone Shield") && !Buff.PlayerHasBuff("Vampiric Blood") && !Buff.PlayerHasBuff("Dancing Rune Weapon") && !Buff.PlayerHasBuff("Lichborne"), "Icebound Fortitude "), 
                    Spells.CastSelfSpell("Empower Rune Weapon",     ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Spell.RuneCooldown(1) + Spell.RuneCooldown(2) + Spell.RuneCooldown(3) + Spell.RuneCooldown(4) + Spell.RuneCooldown(5) + Spell.RuneCooldown(6)) > 8 && !Buffs.UnitHasHasteBuff(Me), "Empower Rune Weapon"), 
                    Items.UseBagItem("Healthstone",                 ret => Me.HealthPercent < 40, "Healthstone")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new PrioritySelector(
                        new Decorator(
                            ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                            new PrioritySelector(
                                Buff.CastBuff("Bone Shield",        ret => (Buff.PlayerCountBuff("Bone Shield") < 2 || !Buff.PlayerHasBuff("Bone Shield")), "Bone Shield"),
                                // Spells.CastSelfSpell("Bone Shield", ret => (Buff.PlayerCountBuff("Bone Shield") < 2 || !Buff.PlayerHasBuff("Bone Shield")), "Bone Shield"),
                                Buff.CastBuff("Horn of Winter",     ret => !Buffs.UnitHasStrAgiBuff(Me), "Horn of Winter"))));
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