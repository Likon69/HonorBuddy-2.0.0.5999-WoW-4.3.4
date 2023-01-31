using CLU.Helpers;

using TreeSharp;

namespace CLU.Classes.Mage
{
    using global::CLU.GUI;
    using global::CLU.Lists;

    class Fire : RotationBase
    {
        public override string Name
        {
            get { return "Fire Mage"; }
        }

        public override string KeySpell
        {
            get { return "Pyroblast"; }
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
                       "1. Ice Block, Spellsteal\n" +
                       "2. Conjure Mana Gem\n" +
                       "3. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Flame Orb & Mana Gem & Mirror Image & Combustion \n" +
                       "4. AoE with Blast Wave & Dragon's Breath & Fire Blast & Flamestrike\n" +
                       "5. Best Suited for end game raiding\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to jamjar0207\n" +
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
                                    Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                    Items.UseBagItem("Volcanic Potion", ret => Buffs.UnitHasHasteBuff(Me), "Volcanic Potion Heroism/Bloodlust"), 
                                    Item.UseEngineerGloves())), 
			        // Comment: Dont break Invinsibility!!
			        new Decorator(
                        x => Buff.PlayerHasBuff("Invisibility"), new Action(a => CLU.Instance.Log("Invisibility active"))), 
                    // Comment: Dont break Evocation!!
                    new Decorator(
                        x => Buff.PlayerHasBuff("Evocation"), new Action(a => CLU.Instance.Log("Evocation active"))), 
                    // Extra button automation
                    Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                    Buff.CastBuff("Molten Armor",                       ret => !Buff.PlayerHasBuff("Mage Armor") && !Buff.PlayerHasBuff("Molten Armor"), "No Armor Buff - Molten Armor"),
                    Buff.CastBuff("Molten Armor",                       ret => Me.ManaPercent > 45 && Buff.PlayerHasBuff("Mage Armor"), "Molten Armor Now We Have Enough Mana Returned From Mage Armor"),
			        // Threat
                    Buff.CastBuff("Invisibility",                       ret => Me.CurrentTarget.ThreatInfo.RawPercent > 90 && !Buff.PlayerHasActiveBuff("Hypothermia"), "Invisibility"), 
					// Interupts & Steal Buffs                    
                    Spells.CastSpell("Spellsteal",                      ret => Spells.TargetHasStealableBuff() && !Me.IsMoving, "[Steal] Spellsteal"), 
                    Spells.CastInterupt("Counterspell",                 ret => true, "Counterspell"), 
					// Cooldowns
                    Items.RunMacroText("/cast Conjure Mana Gem",        ret => !Items.HaveManaGem(), "[Create] Mana Gem"), 
                    Items.UseBagItem("Mana Gem",                        ret => Me.ManaPercent < 90 && Units.IsTargetWorthy(Me.CurrentTarget), "Mana Gem"), 
                    Spells.ChannelSelfSpell("Evocation",                ret => Me.ManaPercent < 10 && !Me.IsMoving, "Evocation For Mana"), 
                    Spells.CastSpell("Scorch",                          ret => !Buff.TargetHasDebuff("Critical Mass", Me.CurrentTarget) && !Buff.TargetHasDebuff("Shadow and Flame", Me.CurrentTarget), "Scorch"),
                    Buff.CastDebuff("Combustion",                       ret => Buff.TargetHasDebuff("Living Bomb") && Buff.TargetHasDebuff("Ignite") && Buff.TargetHasDebuff("Pyroblast!") && Units.IsTargetWorthy(Me.CurrentTarget), "Combustion"), 
                    Spells.CastSelfSpell("Mirror Image",                ret => Units.IsTargetWorthy(Me.CurrentTarget), "Mirror Image"), 
                    Spells.CastSpell("Flame Orb",                       ret => Units.IsTargetWorthy(Me.CurrentTarget), "Flame Orb"),
                    Spells.CastConicSpell("Dragon's Breath", 12f, 33f, ret => true, "Dragon's Breath"),
					// AoE
					new Decorator(
                            ret => !Me.IsMoving && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 2, 
                                new PrioritySelector(
                                    Spells.CastAreaSpell("Blast Wave", 8, true, 3, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry), "Blast Wave"), 
                                    Spells.CastSpell("Fire Blast", ret => (Buff.TargetHasDebuff("Living Bomb") || Buff.TargetHasDebuff("Ignite")) && Buff.PlayerHasBuff("Impact"), "Fire Blast with Impact"),
                                    Spells.CastSpellAtLocation("Flamestrike", u => Me.CurrentTarget, ret => !Buff.TargetHasDebuff("Flamestrike") && !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.ManaPercent > 30 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 3, "Flamestrike")                                    
                                    )),                                                                                                  
			        // Default Rotaion		
                    Buff.CastDebuff("Living Bomb",                      ret => true, "Living Bomb"),
                    Spells.CastSpell("Pyroblast",                       ret => Buff.PlayerHasActiveBuff("Hot Streak"), "Hot Streak"), 
                    Spells.CastSpell("Fireball",                        ret => true, "Fireball"),
                    Spells.CastSpell("Scorch",                          ret => Me.IsMoving, "Scorch while Moving"),
                    Buff.CastBuff("Mage Armor",                         ret => Me.ManaPercent < 5 && !Buff.PlayerHasBuff("Mage Armor"), "Mage Armor We Are Low On Mana")
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
                        Items.UseBagItem("Healthstone",         ret => Me.HealthPercent < 30, "Healthstone"),
                        Buff.CastBuff("Ice Block",              ret => Me.HealthPercent < 20 && !Buff.PlayerHasActiveBuff("Hypothermia"), "Ice Block"),
                        Buff.CastBuff("Mage Ward",              ret => Me.HealthPercent < 50, "Mage Ward")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                            ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"), 
                            new PrioritySelector(
                                Buff.CastBuff("Molten Armor",                   ret => true, "Molten Armor"),
                                Buff.CastRaidBuff("Dalaran Brilliance",         ret => !Buff.PlayerHasBuff("Arcane Brilliance"), "Dalaran Brilliance"),
                                Buff.CastRaidBuff("Arcane Brilliance",          ret => !Buff.PlayerHasBuff("Dalaran Brilliance"), "Arcane Brilliance"), // as most people say, the main difference between Dalaran and Arcane Brilliance, is that Dalaran Brilliance has a totally different casting animation (looks way cooler) that allows you to stand out from most mages and has a 10 yard increase in range.
                                Items.RunMacroText("/cast Conjure Mana Gem",    ret => !Me.IsMoving && !Items.HaveManaGem(), "Conjure Mana Gem")));
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
