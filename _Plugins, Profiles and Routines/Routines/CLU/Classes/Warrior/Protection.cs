using CLU.Helpers;

using TreeSharp;

namespace CLU.Classes.Warrior
{
    using global::CLU.GUI;

    class Protection : RotationBase
    {        

        public override string Name
        {
            get { return "Protection Warrior"; }
        }

        public override string KeySpell
        {
            get { return "Shield Slam"; }
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
                return "----------------------------------------------------------------------\n" +
                      "This Rotation will:\n" +
                      "1. Heal using Victory Rush\n" +
                      "==>             Rallying Cry if it detects player below 20%, Healthstone \n" +
                      "==>             Last Stand with Enraged Regeneration (or without)\n" +
                      "2. AutomaticCooldowns has: \n" +
                      "==> UseTrinkets \n" +
                      "==> UseRacials \n" +
                      "==> UseEngineerGloves \n" +
                      "==> Earthen Potion & Recklessness & BattleShout & Demoralizing Shout\n" +
                      "4. Best Suited for end game raiding\n" +
                      "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                      "Credits to Jamjar0207\n" +
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
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), 
                                        Items.UseBagItem("Earthen Potion",      ret => Me.HealthPercent < 35, "Earthen Potion"), 
                                        Spells.CastSelfSpell("Recklessness",    ret => (Me.CurrentTarget.HealthPercent < 20 || Buffs.UnitHasHasteBuff(Me)) && Me.HealthPercent > 80, "Recklessness"))), 
                    // Extra Action button automation
                    Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                    // Interupts
                    Spells.CastInterupt("Pummel",                   ret => true, "Pummel"), 
                    Spells.CastInterupt("Shockwave",                ret => true, "Shockwave"),
                    Spells.CastSelfSpell("Spell Reflection",        ret => (Me.CurrentTarget.IsCasting || Me.CurrentTarget.ChanneledCastingSpellId != 0) && Me.CurrentTarget.CurrentTarget == Me, "Spell Reflection"),                    
                    Spells.CastInterupt("Concussion Blow",          ret => true, "Concussion Blow"),
                    // Spells.CastInterupt("Heroic Throw",          ret => true, "Heroic Throw"), //if you are gag order talented
                    // START AoE
                    new Decorator(
                        ret => Units.CountEnnemiesInRange(Me.Location, 12) >= 3, 
                        new PrioritySelector(
                            // Spells.CastAreaSpell("Intimidating Shout", 8, false, 3, 0.0, 0.0, ret => true, "Intimidating Shout"), //only use if glyphed
                            Spells.CastSelfSpell("Retaliation",     ret => true, "Retaliation"),
                            Spells.CastSelfSpell("Inner Rage",      ret => Me.RagePercent >= 85 && !Buff.PlayerHasBuff("Berserker Rage"), "Inner Rage"),
                            Spells.CastAreaSpell("Thunder Clap", 10, false, 3, 0.0, 0.0, ret => Buff.TargetHasDebuff("Rend"), "Thunder Clap"),
                            Spells.CastConicSpell("Shockwave", 11f, 33f, ret => true, "Shockwave"),
                            Spells.CastAreaSpell("Cleave", 10, true, 3, 0.0, 0.0, ret => Me.RagePercent > 50, "Cleave"), 
                            Buff.CastDebuff("Rend",                 ret => true, "Rend"),
                            Spells.CastSpell("Revenge",             ret => true, "Revenge"),
                            Spells.CastSpell("Shield Slam",         ret => Buff.PlayerHasActiveBuff("Sword and Board"), "Shield Slam (Sword and Board)"),
                            Spells.CastSelfSpell("Shield Block",    ret => true, "Shield Block"),
                            Buff.CastDebuff("Demoralizing Shout",   ret => !Buffs.UnitHasDamageReductionDebuff(Me.CurrentTarget), "Demoralizing Shout"),
                            Spells.CastSpell("Shield Slam",         ret => true, "Shield Slam")
                            )),
                    // START Main Rotation
                    Spells.CastSpell("Shield Slam",             ret => Buff.PlayerHasActiveBuff("Sword and Board"), "Shield Slam (Sword and Board)"),
                    Spells.CastSpell("Heroic Strike",           ret => Me.RagePercent >= 60, "Heroic Strike Rage Dump"),
                    Spells.CastSelfSpell("Inner Rage",          ret => Me.RagePercent >= 85 && !Buff.PlayerHasBuff("Berserker Rage"), "Inner Rage"), 
                    Spells.CastSelfSpell("Berserker Rage",      ret => !Buff.PlayerHasBuff("Inner Rage"), "Berserker Rage"), 
                    Spells.CastSelfSpell("Shield Block",        ret => true, "Shield Block"),
                    Spells.CastSpell("Shield Slam",             ret => true, "Shield Slam"),
                    Spells.CastSpell("Devastate",               ret => Spells.SpellCooldown("Shield Slam").TotalSeconds > 1.5 && Me.RagePercent < 60 && Buff.TargetHasDebuff("Rend") && Buffs.UnitHasDamageReductionDebuff(Me.CurrentTarget) && Buffs.UnitHasAttackSpeedDebuff(Me.CurrentTarget), "Devastate waiting for Shield Slam"),
                    Spells.CastSpell("Revenge",                 ret => true, "Revenge"),
                    Spells.CastSpell("Devastate",               ret => !Buffs.UnitHasArmorReductionDebuff(Me.CurrentTarget) && Buff.TargetCountDebuff("Sunder Armor") < 3, "Devastate (Sunder Armor < 3)"),
                    Buff.CastDebuff("Demoralizing Shout",       ret => !Buffs.UnitHasDamageReductionDebuff(Me.CurrentTarget), "Demoralizing Shout"),
                    Buff.CastDebuff("Rend",                     ret => true, "Rend"),
                    Buff.CastDebuff("Thunder Clap",             ret => !Buffs.UnitHasAttackSpeedDebuff(Me.CurrentTarget), "Concussion Blow"),  
                    Spells.CastSpell("Devastate",               ret => true, "Devastate"),
                    Spells.CastConicSpell("Shockwave", 11f, 33f, ret => true, "Shockwave"),
                    Buff.CastBuff("Commanding Shout",           ret => Me.RagePercent < 40 && SettingsFile.Instance.AutoManageWarriorShout, "Commanding Shout for Rage"),
                    Buff.CastBuff("Battle Shout",               ret => Me.RagePercent < 40 && !SettingsFile.Instance.AutoManageWarriorShout, "Battle Shout for Rage"))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing, 
                    new PrioritySelector(
                        Spells.CastSpell("Victory Rush",                ret => Me.HealthPercent < 80, "Victory Rush"),
                        Spells.CastSelfSpell("Last Stand",              ret => Me.HealthPercent < 40 && !Buff.PlayerHasBuff("Shield Wall") && !Buff.PlayerHasBuff("Rallying Cry") && !Buff.PlayerHasBuff("Enraged Regeneration"), "Last Stand"),
                        Spells.CastSelfSpell("Shield Wall",             ret => Me.HealthPercent < 40 && !Buff.PlayerHasBuff("Last Stand") && !Buff.PlayerHasBuff("Rallying Cry") && !Buff.PlayerHasBuff("Enraged Regeneration"), "Shield Wall"),
                        Spells.CastSelfSpell("Rallying Cry",            ret => Me.HealthPercent > 60 && !Buff.PlayerHasBuff("Last Stand") && !Buff.PlayerHasBuff("Shield Wall") && !Buff.PlayerHasBuff("Enraged Regeneration") && Units.WarriorRallyingCryPlayers, "Rallying Cry - Somebody needs me!"),
                        Spells.CastSelfSpell("Enraged Regeneration",    ret => Me.HealthPercent < 40 && (Buff.PlayerHasBuff("Rallying Cry") || Buff.PlayerHasBuff("Last Stand")) && !Buff.PlayerHasBuff("Shield Wall"), "Enraged Regeneration"),
                        Spells.CastSelfSpell("Enraged Regeneration",    ret => Me.HealthPercent < 40 && !Buff.PlayerHasBuff("Rallying Cry") && !Buff.PlayerHasBuff("Last Stand") && !Buff.PlayerHasBuff("Shield Wall"), "Enraged Regeneration"),
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
                                Buff.CastBuff("Commanding Shout",       ret => !Buffs.UnitHasStaminaBuffs(Me) && SettingsFile.Instance.AutoManageWarriorShout, "Commanding Shout"),
                                Buff.CastBuff("Battle Shout",           ret => !Buffs.UnitHasStrAgiBuff(Me) && !SettingsFile.Instance.AutoManageWarriorShout, "Battle Shout"))));
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