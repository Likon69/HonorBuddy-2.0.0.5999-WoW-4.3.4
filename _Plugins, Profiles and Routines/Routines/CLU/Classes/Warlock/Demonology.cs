using CLU.Helpers;

using CommonBehaviors.Actions;

using TreeSharp;

namespace CLU.Classes.Warlock
{
    using global::CLU.GUI;
    using global::CLU.Lists;

    class Demonology : RotationBase
    {
        public override string Name
        {
            get { return "Demonology Warlock"; }
        }

        public override string KeySpell
        {
            get { return "Summon Felguard"; }
        }

        // I want to keep moving at melee range while morph is available
        // note that this info is used only if you enable moving/facing in the CC settings.
        public override float CombatMaxDistance
        {
            get { return (Spells.CanCast("Metamorphosis", Me) || Buff.PlayerHasBuff("Metamorphosis")) ? 10f : 35f; }
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
                       "This CC will:\n" +
                       "1. Soulshatter, Interupts with Axe Toss, Soul Harvest < 2 shards out of combat\n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Demon Soul & Summon Doomguard & Metamorphosis & Curse of the Elements & Lifeblood\n" +
                       "==> Felgaurd to Felhunter Swap\n" +
                       "3. AoE with Hellfire and Felstorm and Shadowflame\n" +
                       "4. Best Suited for end game raiding\n" +
                       "Ensure you're at least at 10yards from your target to maximize your dps, even during burst phase if possible.\n" +
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
                        new Decorator(
                            ret => Units.IsTargetWorthy(Me.CurrentTarget),
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"),
                                        Item.UseEngineerGloves())),
                        // Pets.CastPetSummonSpell("Summon Felguard", ret => _Units.IsTargetWorthy(Me.CurrentTarget) && !Me.GotAlivePet && (Buffs.PlayerHasBuff("Demonic Rebirth") || Buffs.PlayerHasBuff("Soulburn")), "Felguard"),
                        // Pets.CastPetSummonSpell("Summon Felhunter", ret => _Units.IsTargetWorthy(Me.CurrentTarget) && !Me.GotAlivePet && (Buffs.PlayerHasBuff("Demonic Rebirth") || Buffs.PlayerHasBuff("Soulburn")), "Felhunter"),
                        // Extra button automation
                        Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                        // START Interupts & Spell casts
                        Pets.CastPetSpell("Axe Toss",                     ret => Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast && Pets.CanCastPetSpell("Axe Toss"), "Axe Toss"),
                        // Threat
                        Buff.CastBuff("Soulshatter",                        ret => Me.GotTarget && Me.CurrentTarget.ThreatInfo.RawPercent > 90 && !Spells.PlayerIsChanneling, "[High Threat] Soulshatter - Stupid Tank"),
                        // Cooldowns
                        new Decorator(
                            ret => Units.IsTargetWorthy(Me.CurrentTarget),
                                new PrioritySelector(
                                    Buff.CastBuff("Metamorphosis",                      ret => true, "Metamorphosis"),
                                    Spells.CastSelfSpell("Demon Soul",                  ret => true, "Demon Soul"),
                                    Spells.CastSelfSpell("Summon Doomguard",            ret => true, "Doomguard"), 
                                    Pets.CastPetSpell("Felstorm",                       ret => Me.Pet.Location.Distance(Me.CurrentTarget.Location) < Spell.MeleeRange, "Felstorm(Special Ability)"),
                                    Spells.CastSelfSpell("Soulburn",                    ret => Pets.CanCastPetSpell("Axe Toss") && !Pets.PetHasBuff("Felstorm"), "Soulburn to raise Felhunter"),
                                    Pets.CastPetSummonSpell("Summon Felhunter",         ret => !Pets.PetHasBuff("Felstorm") && Buff.PlayerHasBuff("Soulburn") && !Pets.CanCastPetSpell("Devour Magic"), "Felhunter")
                                    )),
                        Spells.CastSelfSpell("Soulburn",                    ret => !Buffs.UnitHasHasteBuff(Me) && !Pets.PetHasBuff("Felstorm"), "Soulburn"),
                        Spells.CastSpell("Soul Fire",                       ret => Buff.PlayerHasBuff("Soulburn"), "Soul Fire with soulburn"),
                        // AoE
                        Pets.CastPetSpell("Felstorm",                       ret => Me.Pet.Location.Distance(Me.CurrentTarget.Location) < Spell.MeleeRange && !Pets.PetHasBuff("Felstorm") && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 12) > 0 && (Me.CurrentTarget.CurrentHealth > 310000 || Me.CurrentTarget.MaxHealth == 1), "Felstorm(Special Ability)"),
                        Spells.ChannelAreaSpell("Hellfire", 11.0, false, 4, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry), "Hellfire"),
                        // Main Rotation
                        Buff.CastDebuff("Curse of the Elements",            ret => Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent > 70 && !Buffs.UnitHasMagicVulnerabilityDeBuffs(Me.CurrentTarget), "Curse of the Elements"),
                        Buff.CastDebuff("Immolate",                         ret => true, "Immolate"),
                        Buff.CastDebuff("Bane of Doom",                     ret => Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent >= 10, "Bane of Doom"),
                        Buff.CastDebuff("Bane of Agony",                    ret => !Units.IsTargetWorthy(Me.CurrentTarget) || (Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent <= 10 && !Buff.PlayerHasActiveBuff("Bane of Doom")), "Bane of Agony"),
                        Buff.CastDebuff("Corruption",                       ret => true, "Corruption"),
						Spells.CastConicSpell("Shadowflame", 11f, 33f, ret => true, "ShadowFlame"),
						Spells.CastSpell("Hand of Gul'dan", 			    ret => true, "Hand of Gul'dan"),
                        Spells.CastSelfSpell("Immolation Aura",             ret => Buff.PlayerBuffTimeLeft("Metamorphosis") > 10 && Spells.DistanceToTargetBoundingBox() <= 10f, "Immolation Aura"),
                        Spells.CastSpell("Incinerate",                      ret => Buff.PlayerHasActiveBuff("Molten Core"), "Incinerate"),
                        Spells.CastSpell("Shadow Bolt",                     ret => Buff.PlayerHasBuff("Shadow Trance"), "Instant Shadow Bolt"),
                        Spells.CastSpell("Soul Fire",                       ret => Buff.PlayerBuffTimeLeft("Decimation") > 0, "Soul Fire with Decimation"),
                        // Spells.CastSpell("Soul Fire",                       ret => Buff.PlayerHasActiveBuff("Decimation"), "Soul Fire with Decimation"),
                        Spells.CastSelfSpell("Life Tap",                    ret => Me.ManaPercent <= 30 && !Spells.PlayerIsChanneling && Me.HealthPercent > 40 && !Buffs.UnitHasHasteBuff(Me) && !Buff.PlayerHasBuff("Metamorphosis") && !Buff.PlayerHasBuff("Demon Soul: Felguard"), "Life tap - mana < 30%"),
                        Spells.CastSpell("Incinerate",                      ret => true, "Incinerate"),
                        Spells.CastSelfSpell("Life Tap",                    ret => Me.IsMoving && Me.HealthPercent > Me.ManaPercent && Me.ManaPercent < 80, "Life tap while moving"),
                        Spells.CastSpell("Fel Flame", 					    ret => Me.IsMoving, "Fel flame while moving"),
                        Spells.CastSelfSpell("Life Tap",                    ret => Me.ManaPercent < 100 && !Spells.PlayerIsChanneling && Me.HealthPercent > 40, "Life tap - mana < 100%"))));
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
                            Buff.CastBuff("Fel Armor",                     ret => true, "Fel Armor"),
                            Pets.CastPetSummonSpell("Summon Felguard",    ret => !Me.IsMoving && !Me.GotAlivePet, "Felguard"),
                            Buff.CastBuff("Soul Link",                     ret => Pet != null && Pet.IsAlive, "Soul Link"),
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
