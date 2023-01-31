using TreeSharp;

namespace CLU.Classes.Warrior
{
    using global::CLU.GUI;
    using global::CLU.Helpers;

    class Arms : RotationBase
    {
        public override string Name
        {
            get { return "Arms Warrior"; }
        }

        public override string KeySpell
        {
            get { return "Bladestorm"; }
        }

        public override float CombatMaxDistance
        {
            get { return 3.2f; }
        }

        public override string Help
        {
            get
            {
                return "----------------------------------------------------------------------\n" +
                       "This Rotation will:\n" +
                       "1. Heal using Victory Rush, Enraged Regeneration\n" +
                       "==> Rallying Cry, Healthstone \n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "3. Stance Dance\n" +
                       "4. Best Suited for end game raiding\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to gniegsch and Obliv\n" +
                       "----------------------------------------------------------------------\n";
            }
        }

        public override Composite SingleRotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(
                            ret => Units.IsTargetWorthy(Me.CurrentTarget),
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Item.UseEngineerGloves())),
                                    // Extra button automation
                                    Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                                    // Interupts
                                    Spells.CastInterupt("Pummel", ret => true, "Pummel"),
                                    Spells.CastSelfSpell("Spell Reflection", ret => Me.CurrentTarget.IsCasting, "Spell Reflection"),

                                    // Start of Actions - SimCraft as of 3/31/2012
                                    // TODO: GCD Check and Tier pce check needs attention. Spell.GCD = 0

                                    // 6	berserker_rage,if=buff.deadly_calm.down&cooldown.deadly_calm.remains>1.5&rage<=95,use_off_gcd=1
                                    Spells.CastSelfSpell("Berserker Rage", ret => !Buff.PlayerHasActiveBuff("Deadly Calm") && Spells.SpellCooldown("Deadly Calm").TotalSeconds > 1.5 && Me.CurrentRage <= 95, "Berserker Rage"),
                                    // 7	deadly_calm,use_off_gcd=1
                                    Spells.CastSelfSpell("Deadly Calm", ret => Units.IsTargetWorthy(Me.CurrentTarget), "Deadly Calm"),
                                    // 8	inner_rage,if=buff.deadly_calm.down&cooldown.deadly_calm.remains>15,use_off_gcd=1
                                    Spells.CastSelfSpell("Inner Rage", ret => !Buff.PlayerHasActiveBuff("Deadly Calm") && Spells.SpellCooldown("Deadly Calm").TotalSeconds > 15, "Inner Rage"),
                                    // 9	recklessness,if=target.health_pct>90|target.health_pct<=20,use_off_gcd=1
                                    Spells.CastSpell("Recklessness", ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Me.CurrentTarget.HealthPercent > 90 || Me.CurrentTarget.HealthPercent <= 20), "Recklessness"),
                                    // A stance,choose=berserker,if=buff.taste_for_blood.down&dot.rend.remains>0&rage<=75,use_off_gcd=1
                                    Spells.CastSelfSpell("Berserker Stance", ret => !Buff.PlayerHasBuff("Berserker Stance") && !Buff.PlayerHasActiveBuff("Taste for Blood") && Buff.TargetHasDebuff("Rend") && Me.CurrentRage <= 75, "**Berserker Stance**"),
                                    // B	stance,choose=battle,use_off_gcd=1,if=!dot.rend.ticking
                                    Spells.CastSelfSpell("Battle Stance", ret => !Buff.PlayerHasBuff("Battle Stance") && !Buff.TargetHasDebuff("Rend"), "**Battle Stance**"),
                                    // C	stance,choose=battle,use_off_gcd=1,if=(buff.taste_for_blood.up|buff.overpower.up)&rage<=75&cooldown.mortal_strike.remains>=1.5,use_off_gcd=1
                                    Spells.CastSelfSpell("Battle Stance", ret => !Buff.PlayerHasBuff("Battle Stance") && (Buff.PlayerHasActiveBuff("Taste for Blood") || Buff.PlayerHasActiveBuff("Overpower")) && Me.CurrentRage <= 75 && Spells.SpellCooldown("Mortal Strike").TotalSeconds > 1.5, "**Battle Stance**"),
                                    // D	sweeping_strikes,if=target.adds>0,use_off_gcd=1
                                    Spells.CastAreaSpell("Sweeping Strikes", 5, false, 2, 0.0, 0.0, a => (Buff.PlayerHasBuff("Berserker Stance") || Buff.PlayerHasBuff("Battle Stance")), "Sweeping Strikes"),
                                    // E	cleave,if=target.adds>0,use_off_gcd=1
                                    Spells.CastAreaSpell("Cleave", 5, false, 3, 0.0, 0.0, a => (Buff.PlayerHasBuff("Berserker Stance") || Buff.PlayerHasBuff("Battle Stance")) && Me.CurrentRage > 40, "Cleave"),
                                    Spells.CastSpell("Rend", ret => !Buff.TargetHasDebuff("Rend"), "Rend"),
                                    // Disabled for now.  We need to only use if we have Blood and Thunder.
                                    // Spells.CastAreaSpell("Thunder Clap", 8, false, 2, 0.0, 0.0, a => Buff.PlayerHasBuff("Battle Stance") && Buff.TargetDebuffTimeLeft("Thunder Clap").TotalSeconds < 12.5 && Buff.TargetHasDebuff("Rend"), "TC Rend"),
                                    // G	bladestorm,if=target.adds>0&!buff.deadly_calm.up&!buff.sweeping_strikes.up
                                    Spells.CastAreaSpell("Bladestorm", 5, false, 4, 0.0, 0.0, a => !Buff.PlayerHasActiveBuff("Deadly Calm") && !Buff.PlayerHasActiveBuff("Sweeping Strikes"), "Bladestorm"),
                                    // Q	heroic_strike,use_off_gcd=1,if=buff.deadly_calm.up
                                    Spells.CastSpell("Heroic Strike", ret => Buff.PlayerHasActiveBuff("Deadly Calm"), "Heroic Strike"),
                                    // R	heroic_strike,use_off_gcd=1,if=rage>85
                                    Spells.CastSpell("Heroic Strike", ret => Me.CurrentRage > 85, "Heroic Strike"),
                                    // S	heroic_strike,use_off_gcd=1,if=buff.inner_rage.up&target.health_pct>20&(rage>=60|(set_bonus.tier13_2pc_melee&rage>=50))
                                    Spells.CastSpell("Heroic Strike", ret => Buff.PlayerHasBuff("Inner Rage") && Me.CurrentTarget.HealthPercent > 20 && Me.CurrentRage > 60, "Heroic Strike"),
                                    // T	heroic_strike,use_off_gcd=1,if=buff.inner_rage.up&target.health_pct<=20&((rage>=60|(set_bonus.tier13_2pc_melee&rage>=50))|buff.battle_trance.up)
                                    Spells.CastSpell("Heroic Strike", ret => Buff.PlayerHasBuff("Inner Rage") && Me.CurrentTarget.HealthPercent <= 20 && Buff.PlayerHasActiveBuff("Battle Trance"), "Heroic Strike"),
                                    Spells.CastSpell("Mortal Strike", ret => Me.CurrentTarget.HealthPercent > 20, "Mortal Strike"),
                                    Spells.CastSpell("Colossus Smash", ret => !Buff.TargetHasDebuff("Colossus Smash"), "Colossus Smash"),
                                    Spells.CastSpell("Execute", ret => Buff.PlayerActiveBuffTimeLeft("Executioner").TotalSeconds < 2.5, "Execute"),
                                    Spells.CastSpell("Mortal Strike", ret => Me.CurrentTarget.HealthPercent <= 20 && (Buff.TargetDebuffTimeLeft("Rend").TotalSeconds < 3 || !Buff.PlayerHasActiveBuff("Wrecking Crew") || Me.CurrentRage <= 25 || Me.CurrentRage >= 35), "Mortal Strike"),
                                    Spells.CastSpell("Execute", ret => Me.CurrentRage > 90, "Execute"),
                                    Spells.CastSpell("Overpower", ret => Buff.PlayerHasActiveBuff("Taste for Blood") || Buff.PlayerHasActiveBuff("Overpower"), "Overpower"),
                                    Spells.CastSpell("Execute", ret => true, "Execute"),
                                    Spells.CastSpell("Colossus Smash", ret => Buff.TargetDebuffTimeLeft("Colossus Smash").TotalSeconds < 1.5, "Colossus Smash"),
                                    Spells.CastSpell("Slam", ret => (Me.CurrentRage >= 35 || Buff.PlayerHasActiveBuff("Battle Trance") || Buff.PlayerHasActiveBuff("Deadly Calm")), "Slam"),
                                    Buff.CastBuff("Battle Shout", ret => Me.CurrentRage < 60, "Battle Shout"));
                                    // (Buff.PlayerHasBuff("Berserker Stance") || Buff.PlayerHasBuff("Battle Stance")) && 
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing,
                    new PrioritySelector(
                        Spells.CastSpell("Victory Rush",                ret => Me.HealthPercent < 80 && Buff.PlayerHasBuff("Victorious"), "Victory Rush"),
                        Spells.CastSelfSpell("Shield Wall",             ret => Me.HealthPercent < 35 && !Buff.PlayerHasBuff("Rallying Cry") && !Buff.PlayerHasBuff("Enraged Regeneration"), "Shield Wall"),
                        Spells.CastSelfSpell("Enraged Regeneration",    ret => Me.HealthPercent < 45 && !Buff.PlayerHasBuff("Rallying Cry") && !Buff.PlayerHasBuff("Shield Wall"), "Enraged Regeneration"),
                        Spells.CastSelfSpell("Rallying Cry",            ret => Me.HealthPercent < 45 && !Buff.PlayerHasBuff("Shield Wall") && !Buff.PlayerHasBuff("Enraged Regeneration"), "Rallying Cry"),
                        Items.UseBagItem("Healthstone",                 ret => Me.HealthPercent < 40 && !Buff.PlayerHasBuff("Rallying Cry") && !Buff.PlayerHasBuff("Enraged Regeneration") && !Buff.PlayerHasBuff("Shield Wall"), "Healthstone")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                        ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                        new PrioritySelector(
                            Buff.CastBuff("Battle Shout", ret => !Buff.PlayerHasBuff("Battle Shout"), "Battle Shout")));
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