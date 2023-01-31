using CommonBehaviors.Actions;

using TreeSharp;

namespace CLU.Classes.Rogue
{
    using System.Linq;

    using Styx;

    using global::CLU.GUI;
    using global::CLU.Helpers;

    using global::CLU.Lists;

    using Styx.WoWInternals.WoWObjects;

    class Assassination : RotationBase
    {

        public override string Name
        {
            get { return "Assassination Rogue"; }
        }

        // public static readonly HealerBase Healer = HealerBase.Instance;

        public override string KeySpell
        {
            get { return "Vendetta"; }
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
                    "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                    "4. Attempt to reduce threat with Feint\n" +
                    "5. Will interupt with Kick\n" +
                    "6. Tricks of the Trade on best target (tank, then class)\n" +
                    "7. Will heal with Healthstone\n" +
                    "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                    "Credits to Obliv for this rotation\n" +
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
                            ret => (Units.IsTargetWorthy(Me.CurrentTarget) && Buff.TargetHasDebuff("Vendetta")),
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Item.UseEngineerGloves())),
                                // Extra button automation
                                Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                                // threat
                                Spells.CastSelfSpell("Feint",                   ret => Me.CurrentTarget.ThreatInfo.RawPercent > 80 || Units.IsMorchokStomp(), "Feint"),
                                Spells.CastInterupt("Kick",                     ret => true, "Kick"),
                                Spells.CastSpell("Redirect",                    ret => Me.RawComboPoints > 0 && Me.ComboPoints < 1, "Redirect"),
                                Spells.CastSpell("Garrote",                     ret => IsBehind(Me.CurrentTarget), "Garrote"),
                                Spells.CastSelfSpell("Slice and Dice",          ret => !Buff.PlayerHasBuff("Slice and Dice"), "Slice and Dice"),
                                Spells.CastSpell("Rupture",                     ret => (!Buff.TargetHasDebuff("Rupture") || Buff.TargetDebuffTimeLeft("Rupture").TotalSeconds < 2) && Buff.TargetDebuffTimeLeft("Rupture").TotalSeconds < 6, "Rupture"),
                                Spells.CastSpell("Vendetta",                    ret => Units.IsTargetWorthy(Me.CurrentTarget), "Vendetta"),
                                Spells.CastSpell("Rupture",                     ret => (!Buff.TargetHasDebuff("Rupture") || Buff.TargetDebuffTimeLeft("Rupture").TotalSeconds < 2) && Buff.TargetDebuffTimeLeft("Slice and Dice").TotalSeconds > 6, "Rupture"),
                                Spells.CastSelfSpell("Cold Blood",              ret => (Me.ComboPoints >= 4 && !Buff.TargetHasDebuff("Envenom")) || (Me.ComboPoints >= 4 && Me.CurrentEnergy > 90), "Cold Blood"),
                                Spells.CastSpell("Envenom",                     ret => Me.ComboPoints >= 4 && !Buff.TargetHasDebuff("Envenom"), "Envenom"),
                                Spells.CastSpell("Envenom",                     ret => Me.ComboPoints >= 4 && Me.CurrentEnergy > 90, "Envenom"),
                                Spells.CastSpell("Envenom",                     ret => Me.ComboPoints >= 2 && Buff.TargetDebuffTimeLeft("Slice and Dice").TotalSeconds < 3, "Envenom"),
                                Spells.CastSpell("Backstab",                    ret => (IsBehind(Me.CurrentTarget) || BossList.BackstabIds.Contains(Me.CurrentTarget.Entry)) && Me.ComboPoints < 5 && Me.CurrentTarget.HealthPercent < 35, "Backstab"),
                                Spells.CastSpell("Mutilate",                    ret => !IsBehind(Me.CurrentTarget) && Me.ComboPoints < 5 && Me.CurrentTarget.HealthPercent < 35, "Mutilate"),
                                Spells.CastSpell("Mutilate",                    ret => Me.ComboPoints < 4 && Me.CurrentTarget.HealthPercent >= 35, "Mutilate"),
                                Spells.CastSpell("Vanish",                      ret => Me.CurrentEnergy > 50 && !Buff.TargetHasDebuff("Garrote") && Units.IsTargetWorthy(Me.CurrentTarget), "Vanish"))));
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