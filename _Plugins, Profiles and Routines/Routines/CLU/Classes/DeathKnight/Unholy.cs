using System.Linq;
using CLU.Helpers;
using CLU.Lists;
using TreeSharp;

namespace CLU.Classes.DeathKnight
{
    using global::CLU.GUI;

    class Unholy : RotationBase
    {
        public override string Name
        {
            get { return "Unholy Deathknight"; }
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
					"1. Heal using AMS, IBF, Healthstone and Deathstrike < 40%\n" +
                    "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Unholy Frenzy & Summon Gargoyle & Death and Decay & Empower Rune Weapon \n" +
					"3. Maintain HoW only if similar buff is not present\n" +
					"4. Ensure we are in Unholy Presence\n" +
					"5. Use Death and Decay and Pestilence and Blood boil for AoE\n" +	
					"6. Brez players (non-specific) using Raise Ally\n" +
					"NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                    "Credits to Weischbier, ossirian, kbrebel04 and cowdude\n" +
                    "----------------------------------------------------------------------\n";
            }
        }

        public override string KeySpell
        {
            get { return "Scourge Strike"; }
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
								Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"),  // Thanks Kink
								Item.UseEngineerGloves(),
                                Spells.CastSelfSpell("Empower Rune Weapon", ret => (Me.BloodRuneCount + Me.FrostRuneCount + Me.UnholyRuneCount + Me.DeathRuneCount == 0) && !Buffs.UnitHasHasteBuff(Me), "Empower Rune Weapon"),
                                Buff.CastBuff("Unholy Frenzy", ret => Me.CurrentRunicPower >= 60 && !Buffs.UnitHasHasteBuff(Me), "Unholy Frenzy"),
								Buff.CastBuff("Summon Gargoyle", ret => Me.CurrentRunicPower >= 60 && Buffs.UnitHasHasteBuff(Me), "Gargoyle"))),
                    // Extra button automation
                    Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                    // Interupts
                    Spells.CastInterupt("Mind Freeze",      ret => true, "Mind Freeze"),
                    Spells.CastInterupt("Strangulate",      ret => true, "Strangulate"),
                    Buff.CastBuff("Anti-Magic Shell",       ret => (Me.CurrentTarget.IsCasting || Me.CurrentTarget.ChanneledCastingSpellId != 0) && Me.CurrentTarget.IsTargetingMeOrPet, "AMS"),
                    Spells.CastSelfSpell("Blood Tap",       ret => Pets.PetCountBuff("Shadow Infusion") == 5 && (Me.BloodRuneCount + Me.UnholyRuneCount + Me.DeathRuneCount == 0), "Blood Tap for Dark Transformation"),
                    Spells.CastSelfSpell("Blood Tap",       ret => Me.FrostRuneCount == 1 && (Me.UnholyRuneCount == 0 || Me.BloodRuneCount == 0), "Blood Tap"),
                    // START Disease
                    Spells.CastSpell("Outbreak",            ret => Buff.TargetDebuffTimeLeft("Blood Plague").TotalSeconds < 0.5 || Buff.TargetDebuffTimeLeft("Frost Fever").TotalSeconds < 0.5, "Outbreak"),
                    Spells.CastSpell("Icy Touch",           ret => Buff.TargetDebuffTimeLeft("Frost Fever").TotalSeconds < 0.5 && Spells.SpellCooldown("Outbreak").TotalSeconds > 2, "Icy Touch for Frost Fever"),
                    Spells.CastSpell("Plague Strike",       ret => Buff.TargetDebuffTimeLeft("Blood Plague").TotalSeconds < 0.5 && Spells.SpellCooldown("Outbreak").TotalSeconds > 2, "Plague Strike for Blood Plague"),	
                    // Start AoE----------------------------------------------------------------------------------------------
                    Spells.CastAreaSpell("Pestilence", 10, false, 3, 0.0, 0.0, ret => Buff.TargetHasDebuff("Blood Plague") && Buff.TargetHasDebuff("Frost Fever") && (from enemy in Unit.EnemyUnits where !enemy.HasAura("Blood Plague") && !enemy.HasAura("Frost Fever") select enemy).Any(), "Pestilence"),
                    // End AoE------------------------------------------------------------------------------------------------
                    Spells.CastSpell("Dark Transformation", ret => true, "Dark Transformation"),
                    // Start AoE----------------------------------------------------------------------------------------------
                    Spells.CastAreaSpell("Blood Boil", 10, false, 3, 0.0, 0.0, ret => Buff.TargetHasDebuff("Blood Plague") && Buff.TargetHasDebuff("Frost Fever") && (from enemy in Unit.EnemyUnits where enemy.HasAura("Blood Plague") && enemy.HasAura("Frost Fever") select enemy).Count() > 2, "Blood Boil"),
                    // End AoE------------------------------------------------------------------------------------------------
                    Spells.CastAreaSpell("Death and Decay", 10, true, 1, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && (Me.UnholyRuneCount == 2) && !Me.IsMoving && !Me.CurrentTarget.IsMoving && (Me.CurrentTarget.CurrentHealth > 1000000 || Me.CurrentTarget.MaxHealth == 1), "Death and Decay"),
                    Spells.CastSpell("Death Coil",          ret => Spells.SpellCooldown("Summon Gargoyle").TotalSeconds >= 1 && !Spells.CanCast("Summon Gargoyle", Me) && Pets.PetCountBuff("Shadow Infusion") < 5 && !Pets.PetHasBuff("Dark Transformation"), "Death Coil no Timmy yet"),
                    Spells.CastSpell("Scourge Strike",      ret => Me.UnholyRuneCount == 2, "Scourge Strike"),
                    Spells.CastSpell("Festering Strike",    ret => Me.BloodRuneCount == 2 && Me.FrostRuneCount == 2, "Festering Strike"),
                    Spells.CastSpell("Death Coil",          ret => Me.CurrentRunicPower >= 90, "Death Coil"),
                    Spells.CastSpell("Death Coil",          ret => Me.CurrentRunicPower >= 90 && Me.CurrentTarget.Distance > 8, "Death Coil"),
                    Spells.CastSpell("Death Coil",          ret => Buff.PlayerHasActiveBuff("Sudden Doom"), "Death Coil  (Sudden Doom)"),
                    Spells.CastAreaSpell("Death and Decay", 10, true, 1, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && !Me.IsMoving && !Me.CurrentTarget.IsMoving && (Me.CurrentTarget.CurrentHealth > 310000 || Me.CurrentTarget.MaxHealth == 1), "Death and Decay"),
                    Spells.CastSpell("Scourge Strike",      ret => true, "Scourge Strike"),
                    Spells.CastSpell("Festering Strike",    ret => true, "Festering Strike"),
                    Spells.CastSpell("Death Coil",          ret => Pets.PetCountBuff("Shadow Infusion") < 5 && !Pets.PetHasBuff("Dark Transformation"), "Death Coil no Timmy yet"),
                    Spells.CastSpell("Death Coil",          ret => Spells.SpellCooldown("Summon Gargoyle").TotalSeconds > 5 && !Spells.CanCast("Summon Gargoyle", Me) && Pets.PetBuffTimeLeft("Dark Transformation").TotalSeconds > 5 && !Spells.CanCast("Dark Transformation", Me), "Death Coil"),
                    // Start AoE----------------------------------------------------------------------------------------------
                    Spells.CastAreaSpell("Icy Touch", 10, false, 3, 0.0, 0.0, ret => Me.FrostRuneCount > 0, "Icy Touch"),
                    // End AoE------------------------------------------------------------------------------------------------
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
                        Spells.CastSelfSpell("Raise Dead",              ret => (Me.Pet == null || Me.Pet.Dead), "Raise Dead"),
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
                            Spells.CastSelfSpell("Raise Dead",  ret => (Me.Pet == null || Me.Pet.Dead), "Raise Dead"), 
                            Buff.CastBuff("Horn of Winter",    ret => !Buffs.UnitHasStrAgiBuff(Me), "Horn of Winter")));
            }
        }

        public override Composite PVPRotation
        {
            get
            {
                return SingleRotation;
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return SingleRotation;
            }
        }
    }
}