using CommonBehaviors.Actions;

using TreeSharp;

namespace CLU.Classes.Rogue
{
    using System.Linq;

    using global::CLU.GUI;
    using global::CLU.Helpers;

    using global::CLU.Lists;

    using Styx.WoWInternals.WoWObjects;

    class Subtlety : RotationBase
    {
        
        public override string Name
        {
            get { return "Subtlety Rogue"; }
        }

		// public static readonly HealerBase Healer = HealerBase.Instance;
		
        public override string KeySpell
        {
            get { return "Shadow Dance"; }
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
                    "4. Attempt to reduce threat with Feint\n" +
                    "5. Will interupt with Kick\n" +
                    "6. Tricks of the Trade on best target (tank, then class)\n" +
                    "7. Will heal with Recuperate and a Healthstone\n" +
					"NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
					"Credits to cowdude\n" +
                    "----------------------------------------------------------------------\n";
            }
        }

        private bool IsBehind(WoWUnit target)
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
                            ret => (Units.IsTargetWorthy(Me.CurrentTarget)),
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Item.UseEngineerGloves())),
                                // Extra button automation
                                Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                                //threat
                                Spells.CastSelfSpell("Feint",                   ret => Me.CurrentTarget.ThreatInfo.RawPercent > 80 || Units.IsMorchokStomp(), "Feint"),
                                Spells.CastInterupt("Kick",                     ret => true, "Kick"),
                                Spells.CastSpell("Redirect",                    ret => Me.RawComboPoints > 0 && Me.ComboPoints < 1, "Redirect"),
                                // Spells.CastAreaSpell("Fan of Knives", 8, false, 6, 0.0, 0.0, ret => true, "Fan of Knives"),
                                Spells.CastSpellonUnit("Tricks of the Trade", u => Units.BestTricksTarget, ret => true, "Tricks of the Trade"),
                                Spells.CastSpell("Shadow Dance",                ret => Me.CurrentEnergy > 85 && Me.ComboPoints < 5 && (!Buff.PlayerHasActiveBuff("Vanish") || !Buff.PlayerHasActiveBuff("Stealth")), "Shadow Dance"),
                                Spells.CastSpell("Vanish",                      ret => Me.CurrentEnergy > 60 && Me.ComboPoints <= 1 && Spells.SpellCooldown("Shadowstep").TotalSeconds <= 0 && !Buff.PlayerHasActiveBuff("Shadow Dance") && !Buff.PlayerHasActiveBuff("Master of Subtlety") && !Buff.TargetHasDebuff("Find Weakness"), "Vanish"),
                                Spells.CastSpell("Shadowstep",                  ret => ((Buff.PlayerHasActiveBuff("Shadow Dance") && Buff.TargetHasDebuff("Find Weakness")) || (Buff.PlayerHasActiveBuff("Vanish") || Buff.PlayerHasActiveBuff("Stealth"))) && !BossList.IgnoreShadowStep.Contains(Me.CurrentTarget.Entry), "Shadowstep"),
                                Spells.CastSpell("Premeditation",               ret => Me.ComboPoints <= 2, "Premeditation"),
                                Spells.CastSpell("Ambush",                      ret => (IsBehind(Me.CurrentTarget) || BossList.BackstabIds.Contains(Me.CurrentTarget.Entry)) && Me.ComboPoints <= 4, "Ambush"),
                                Spells.CastSpell("Preparation",                 ret => Spells.SpellCooldown("Vanish").TotalSeconds > 60, "Preparation"),
                                Spells.CastSelfSpell("Slice and Dice",          ret => Buff.PlayerBuffTimeLeft("Slice and Dice") < 3 && Me.ComboPoints == 5, "Slice and Dice"),
                                Spells.CastSpell("Rupture",                     ret => Me.ComboPoints == 5 && !Buff.TargetHasDebuff("Rupture"), "Rupture"),
                                Spells.CastSelfSpell("Recuperate",              ret => Me.ComboPoints == 5 && Buff.PlayerBuffTimeLeft("Recuperate") < 3, "Recuperate"),
                                Spells.CastSpell("Eviscerate",                  ret => Me.ComboPoints == 5 && Buff.TargetDebuffTimeLeft("Rupture").TotalSeconds > 1, "Eviscerate"), 
                                Buff.CastDebuff("Hemorrhage",                   ret => ((Me.ComboPoints < 4 && !Buffs.UnitHasBleedDamageDebuff(Me.CurrentTarget)) || !IsBehind(Me.CurrentTarget)), "Hemorrhage"),
                                Buff.CastDebuff("Hemorrhage",                   ret => ((Me.ComboPoints < 5 && Me.CurrentEnergy > 80 && !Buffs.UnitHasBleedDamageDebuff(Me.CurrentTarget)) || !IsBehind(Me.CurrentTarget)), "Hemorrhage"),
                                Spells.CastSpell("Backstab",                    ret => (IsBehind(Me.CurrentTarget) || BossList.BackstabIds.Contains(Me.CurrentTarget.Entry)) && Me.ComboPoints < 4, "Backstab"),
                                Spells.CastSpell("Backstab",                    ret => (IsBehind(Me.CurrentTarget) || BossList.BackstabIds.Contains(Me.CurrentTarget.Entry)) && Me.ComboPoints < 5 && Me.CurrentEnergy > 80, "Backstab")
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
                            Items.UseBagItem("Healthstone",             ret => Me.HealthPercent < 40, "Healthstone"),
                            Spells.CastSelfSpell("Smoke Bomb",          ret => Me.HealthPercent < 30 && Me.CurrentTarget.IsTargetingMeOrPet, "Smoke Bomb"),
                            Spells.CastSelfSpell("Combat Readiness",    ret => Me.HealthPercent < 40 && Me.CurrentTarget.IsTargetingMeOrPet, "Combat Readiness"),
                            Spells.CastSelfSpell("Evasion",             ret => Me.HealthPercent < 35 && Unit.EnemyUnits.Count(u => u.DistanceSqr < 6 * 6 && u.IsTargetingMeOrPet) >= 1, "Evasion"),
                            Spells.CastSelfSpell("Cloak of Shadows",    ret => Unit.EnemyUnits.Count(u => u.IsTargetingMeOrPet && u.IsCasting) >= 1, "Cloak of Shadows"),
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