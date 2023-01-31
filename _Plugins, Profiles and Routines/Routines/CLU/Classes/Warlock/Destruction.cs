using CLU.Helpers;

using CommonBehaviors.Actions;

using TreeSharp;

namespace CLU.Classes.Warlock
{
    using global::CLU.GUI;
    using global::CLU.Lists;

    class Destruction : RotationBase
    {
        public override string Name
        {
            get { return "Destruction Warlock"; }
        }

        public override string KeySpell
        {
            get { return "Conflagrate"; }
        }

        public override float CombatMinDistance
        {
            get { return 30f; }
        }

        // adding some help about cooldown management
        public override string Help
        {
            get
            {
                return "\n" +
                       "----------------------------------------------------------------------\n" +
                       "This Rotation will:\n" +
                       "1. Soulshatter, Soul Harvest < 2 shards out of combat\n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Demon Soul while not moving & Summon Doomguard & Curse of the Elements & Lifeblood\n" +
                       "3. AoE with Rain of Fire, Shadowfury\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to cowdude\n" +
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
                        // Extra button automation
                        Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                        new Decorator(
                            ret => Units.IsTargetWorthy(Me.CurrentTarget),
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Item.UseEngineerGloves())),
                        Pets.CastPetSummonSpell("Summon Imp", 		    ret => !Me.GotAlivePet && (Buff.PlayerHasBuff("Demonic Rebirth") || Buff.PlayerHasBuff("Soulburn")), "Imp"),
                        // Threat
                        Buff.CastBuff("Soulshatter",                   ret => Me.GotTarget && Me.CurrentTarget.ThreatInfo.RawPercent > 90 && !Spells.PlayerIsChanneling, "Soulshatter"),
                        // Multi-Dotting will occour if there are between 1 or more and less than 6 enemys within 15yrds of your current target and you have more than 50%. //Can be disabled within the GUI
                        Spells.FindMultiDotTarget(a => Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 1 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) < 5 && Me.ManaPercent > 50, "Immolate"),
                        Spells.FindMultiDotTarget(a => Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 1 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) < 5 && Me.ManaPercent > 50, "Corruption"),
                        // Cooldown
                        Spells.CastSelfSpell("Demon Soul",              ret => Units.IsTargetWorthy(Me.CurrentTarget) && !Me.IsMoving, "Demon Soul while not moving"),
                        // AoE here
                        Spells.ChannelAreaSpell("Rain of Fire", 10, true, 4, 0.0, 0.0, ret => !Me.IsMoving && !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry), "Rain of Fire"),
					    Spells.CastAreaSpell("Shadowfury", 10, true, 4, 0.0, 0.0, ret => Me.ManaPercent > 40, "Shadowfury"),
					    // End AoE
                        Buff.CastDebuff("Curse of the Elements",        ret => Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent > 70 && !Buffs.UnitHasMagicVulnerabilityDeBuffs(Me.CurrentTarget), "Curse of the Elements"),
                        Spells.CastSelfSpell("Soulburn",               ret => !Buffs.UnitHasHasteBuff(Me), "Soulburn"),
                        Spells.CastSpell("Soul Fire",                  ret => Buff.PlayerHasBuff("Soulburn"), "Soul Fire with soulburn"),
                        Buff.CastDebuff("Immolate",                    ret => true, "Immolate"),					
					    Spells.CastSpell("Conflagrate", 			   ret => true, "Conflagrate"),
                        Buff.CastDebuff("Immolate",                    ret => Buffs.UnitHasHasteBuff(Me) && (Buff.PlayerBuffTimeLeft("Bloodlust") > 32 || Buff.PlayerBuffTimeLeft("Heroism") > 32 || Buff.PlayerBuffTimeLeft("Time Warp") > 32 || Buff.PlayerBuffTimeLeft("Ancient Hysteria") > 32) && (Spells.SpellCooldown("Conflagrate").TotalSeconds <= 3), "Immolate"),
                        Buff.CastDebuff("Bane of Doom",                 ret => Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent >= 10, "Bane of Doom"),
                        Buff.CastDebuff("Bane of Agony",                ret => !Units.IsTargetWorthy(Me.CurrentTarget) || (Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent <= 10 && !Buff.PlayerHasActiveBuff("Bane of Doom")), "Bane of Agony"),
                        Spells.CastSpellonUnit("Bane of Havoc", u => Units.BestBaneOfHavocTarget, ret => true, "Bane of Havoc on "), // + Units.BestBaneOfHavocTarget.Name
                        Buff.CastDebuff("Corruption",                  ret => true, "Corruption"),
					    Spells.CastConicSpell("Shadowflame", 11f, 33f,  ret => true, "ShadowFlame"),
                        Spells.CastSpell("Chaos Bolt",                  ret => (Spells.CastTime("Chaos Bolt") > 0.9), "Chaos Bolt"),
                        Spells.CastSelfSpell("Summon Doomguard",        ret => Units.IsTargetWorthy(Me.CurrentTarget), "Doomguard"),
                        Spells.CastSpell("Soul Fire",                   ret => Buff.PlayerHasBuff("Empowered Imp"), "Soul Fire with Empowered Imp"),
                        // Buff.CastOffensiveBuff("Soul Fire", "Empowered Imp", Buff.PlayerBuffTimeLeft("Improved Soul Fire").TotalSeconds, "Soul Fire with Improved Soul Fire... (Empowered Imp=" + Buff.PlayerBuffTimeLeft("Empowered Imp").TotalSeconds + ")"),
                        Buff.CastOffensiveBuff("Soul Fire", "Improved Soul Fire", Spells.CastTime("Soul Fire") + 1.5 + Spells.CastTime("Incinerate") + Spell.GCD, "Soul Fire... (Soul Fire.cast_time+travel_time+incinerate.cast_time+gcd=" + (Spells.CastTime("Soul Fire") + 1.5 + Spells.CastTime("Incinerate") + Spell.GCD) + ")"),
					    Spells.CastSpell("Shadowburn", 				    ret => true, "Shadowburn"),
					    Spells.CastSpell("Incinerate", 				    ret => true, "Incinerate"),
                        Spells.CastSelfSpell("Life Tap",                ret => Me.IsMoving && Me.HealthPercent > Me.ManaPercent && Me.ManaPercent < 80, "Life tap while moving"),
					    Spells.CastSpell("Fel Flame", 				    ret => Me.IsMoving, "Fel flame while moving"),
                        Spells.CastSelfSpell("Life Tap",                ret => Me.ManaPercent < 100 && !Spells.PlayerIsChanneling && Me.HealthPercent > 40, "Life tap while mana < 100%"))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing,
                    new PrioritySelector(Items.UseBagItem("Healthstone", ret => Me.HealthPercent < 40, "Healthstone")));
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
                            Buff.CastBuff("Fel Armor", ret => true, "Fel Armor"),
                            Pets.CastPetSummonSpell("Summon Imp", ret => !Me.IsMoving && !Me.GotAlivePet, "Imp"),
                            Buff.CastBuff("Soul Link", ret => Pet != null && Pet.IsAlive, "Soul Link"),
                            new Decorator(
                                ret => !Me.IsMoving,
                                new Sequence( // Waiting for a bit
                                            new ActionSleep(2000),
                                            Spells.ChannelSelfSpell("Soul Harvest", ret => Me.CurrentSoulShards < 2 && !Me.IsMoving, "[Shards] Soul Harvest - < 2 shards"))))));
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
