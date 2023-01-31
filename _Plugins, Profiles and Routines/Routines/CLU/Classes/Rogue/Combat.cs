using CommonBehaviors.Actions;

using TreeSharp;

namespace CLU.Classes.Rogue
{
    using System.Linq;

    using global::CLU.GUI;
    using global::CLU.Helpers;

    using Styx.WoWInternals.WoWObjects;

    class Combat : RotationBase
    {
        
        public override string Name
        {
            get { return "Combat Rogue"; }
        }

		// public static readonly HealerBase Healer = HealerBase.Instance;
		
        public override string KeySpell
        {
            get { return "Killing Spree"; }
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
                    "1. Attempt to evade/escape crowd control with Evasion, Cloak of Shadows, Smoke Bomb, Combat Readiness.\n" +
                    "2. Rotation is set up for Hemorrhage\n" +
                    "3. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Adrenaline Rush & Killing Spree\n" +
                    "4. Attempt to reduce threat with Feint\n" +
                    "5. Will interupt with Kick\n" +
                    "6. Tricks of the Trade on best target (tank, then class)\n" +
                    "7. Will heal with Recuperate and a Healthstone\n" +
                    "8. Expose Armor on Bosses only if similar buff is not present\n" +
					"NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
					"Credits to cowdude\n" +
                    "----------------------------------------------------------------------\n";
            }
        }

        private static bool IsBehind(WoWUnit target)
        {
            // WoWMathHelper.IsBehind(Me.Location, target.Location, target.Rotation, (float)Math.PI * 5 / 6)
            return target != null && target.MeIsBehind;
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
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Item.UseEngineerGloves())),
                                // Extra button automation
                                Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                                Spells.CastInterupt("Kick",                     ret => true, "Kick"),
                                Spells.CastSpell("Redirect",                    ret => Me.RawComboPoints > 0 && Me.ComboPoints < 1, "Redirect"),
                                Items.RunMacroText("/cancelaura Blade Flurry",  ret => Unit.EnemyUnits.Count() < 2 && Buff.PlayerHasBuff("Blade Flurry"), "[CancelAura] Blade Flurry"),
                                Spells.CastSelfSpell("Blade Flurry",            ret => Unit.EnemyUnits.Count() >= 2 && SettingsFile.Instance.AutoManageAoE, "Blade Flurry"),
                                Spells.CastAreaSpell("Fan of Knives", 8, false, 6, 0.0, 0.0, ret => Me.CurrentEnergy > 85, "Fan of Knives"),
                                Spells.CastSpellonUnit("Tricks of the Trade", u => Units.BestTricksTarget, ret => true, "Tricks of the Trade"),
                                Spells.CastSpell("Expose Armor",                ret => Me.ComboPoints == 5 && Units.IsTargetWorthy(Me.CurrentTarget) && !Buffs.UnitHasArmorReductionDebuff(Me.CurrentTarget), "Expose Armor"),
                                Spells.CastSelfSpell("Slice and Dice",          ret => Buff.PlayerBuffTimeLeft("Slice and Dice") < 2, "Slice and Dice"),
                                Spells.CastSelfSpell("Killing Spree",           ret => Me.CurrentEnergy < 35 && Buff.PlayerBuffTimeLeft("Slice and Dice") > 4 && !Buff.PlayerHasBuff("Adrenaline Rush") && SettingsFile.Instance.AutoManageCooldowns, "Killing Spree"),
                                Spells.CastSelfSpell("Adrenaline Rush",         ret => Me.CurrentEnergy < 35 && Units.IsTargetWorthy(Me.CurrentTarget), "Vanish"),
                                Spells.CastSpell("Eviscerate",                  ret => Me.ComboPoints == 5 && (Buff.PlayerHasBuff("Moderate Insight") || Buff.PlayerHasBuff("Deep Insight")), "Eviscerate & Moderate Insight or Deep Insight"),
                                Spells.CastSpell("Rupture",                     ret => Me.ComboPoints == 5 && !Buff.TargetHasDebuff("Rupture") && Buffs.UnitHasBleedDamageDebuff(Me.CurrentTarget), "Rupture"),
                                Spells.CastSpell("Eviscerate",                  ret => Me.ComboPoints == 5, "Eviscerate"),
                                Spells.CastSpell("Revealing Strike",            ret => Me.ComboPoints == 4 && !Buff.TargetHasDebuff("Revealing Strike"), "Revealing Strike"),
                                Spells.CastSpell("Sinister Strike",             ret => Me.ComboPoints < 5, "Sinister Strike")
                                )));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing,
                        new PrioritySelector(
                        Items.UseBagItem("Healthstone",                 ret => Me.HealthPercent < 40, "Healthstone"),
                        Spells.CastSelfSpell("Smoke Bomb",              ret => Me.HealthPercent < 30 && Me.CurrentTarget.IsTargetingMeOrPet, "Smoke Bomb"),
                        Spells.CastSelfSpell("Combat Readiness",        ret => Me.HealthPercent < 40 && Me.CurrentTarget.IsTargetingMeOrPet, "Combat Readiness"),
                        Spells.CastSelfSpell("Evasion",                 ret => Me.HealthPercent < 35 && Unit.EnemyUnits.Count(u => u.DistanceSqr < 6 * 6 && u.IsTargetingMeOrPet) >= 1, "Evasion"),
                        Spells.CastSelfSpell("Cloak of Shadows",        ret => Unit.EnemyUnits.Count(u => u.IsTargetingMeOrPet && u.IsCasting) >= 1, "Cloak of Shadows"),
                        Poisons.CreateApplyPoisons()));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                            ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                            new PrioritySelector(
                                new Decorator(
                                    ret => true,
                                        new PrioritySelector(
                                             Poisons.CreateApplyPoisons()))));
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
