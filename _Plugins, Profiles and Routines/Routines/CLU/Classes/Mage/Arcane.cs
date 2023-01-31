using System.Linq;
using CLU.Helpers;
using TreeSharp;

namespace CLU.Classes.Mage
{
    using global::CLU.GUI;

    class Arcane : RotationBase
    {
        public override string Name
        {
            get { return "Arcane Mage"; }
        }

        public override string KeySpell
        {
            get { return "Arcane Barrage"; }
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
                       "==> Flame Orb & Mana Gem & Mirror Image & Arcane Power \n" +
                       "4. AoE with Arcane Explosion & Frost Nova \n" +
                       "5. Best Suited for end game raiding\n" +
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
                                    Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                    Items.UseBagItem("Volcanic Potion", ret => Buffs.UnitHasHasteBuff(Me), "Volcanic Potion Heroism/Bloodlust"),
                                    Item.UseEngineerGloves())),
			        // Comment: Dont break Invinsibility!!
			        new Decorator(
			            x => Buff.PlayerHasBuff("Invisibility"), new Action(a => CLU.Instance.Log("Invisibility active"))),
                    // Extra button automation
                    Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
					// Interupts & Steal Buffs
                    Spells.CastSpell("Spellsteal",                  ret => Spells.TargetHasStealableBuff() && !Me.IsMoving, "[Steal] Spellsteal"),
                    Spells.CastInterupt("Counterspell",             ret => true, "Counterspell"),
					// Cooldowns
                    Spells.ChannelSelfSpell("Evocation",            ret => Me.ManaPercent < 35 && !Me.IsMoving, "Evocation"),
                    Spells.CastSpell("Flame Orb",                   ret => Units.IsTargetWorthy(Me.CurrentTarget), "Flame Orb"),
                    Items.UseBagItem("Mana Gem",                    ret => Me.ManaPercent < 90 && Units.IsTargetWorthy(Me.CurrentTarget), "Mana Gem"),
                    Spells.CastSelfSpell("Mirror Image",            ret => Buff.PlayerHasBuff("Arcane Power") && Units.IsTargetWorthy(Me.CurrentTarget), "Mirror Image"),
                    Spells.CastSelfSpell("Presence of Mind",        ret => !Buff.PlayerHasBuff("Invisibility"), "Presence of Mind"),
                    Spells.CastSelfSpell("Arcane Power",            ret => Buff.PlayerHasBuff("Improved Mana Gem") || Units.IsTargetWorthy(Me.CurrentTarget), "Arcane Power"),
                    Items.RunMacroText("/cast Conjure Mana Gem",    ret => Buff.PlayerHasBuff("Presence of Mind") && !Items.HaveManaGem(), "Conjure Mana Gem"),
					// AoE
					new Decorator(
                            ret => !Me.IsMoving && Unit.EnemyUnits.Count() > 2,
                                new PrioritySelector(
                                    Spells.CastSpell("Arcane Blast", ret => !Buff.PlayerHasBuff("Arcane Blast"), "Arcane Blast"),
                                    Spells.CastAreaSpell("Arcane Explosion", 10, false, 4, 0.0, 0.0, ret => true, "Arcane Explosion"),
                                    Spells.CastAreaSpell("Frost Nova", 10, false, 3, 0.0, 0.0, ret => true, "Frost Nova"))),
			        // Default Rotaion		
			        Spells.CastSpell("Arcane Barrage",              ret => Spells.SpellCooldown("Evocation").TotalSeconds > 10 && Me.ManaPercent < 70 && !Buff.PlayerHasActiveBuff("Arcane Missiles!"), "Arcane Barrage"),
                    Spells.ChannelSpell("Arcane Missiles",          ret => Spells.SpellCooldown("Evocation").TotalSeconds > 10 && Me.ManaPercent < 80 && Buff.PlayerHasActiveBuff("Arcane Missiles!"), "Arcane Missiles"),
                    Spells.CastSpell("Arcane Blast",                ret => !Me.IsMoving, "Arcane Blast"),
                    Spells.CastSpell("Arcane Barrage",              ret => Me.IsMoving, "Arcane Barrage (Moving)"),
                    Spells.CastSpell("Fire Blast",                  ret => Me.IsMoving, "Fire Blast (Moving)"),
			        Spells.CastSpell("Ice Lance",                   ret => Me.IsMoving, "Ice Lance (Moving)"))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing,
                    new PrioritySelector(
                        Items.UseBagItem("Healthstone",     ret => Me.HealthPercent < 30, "Healthstone"),
                        Buff.CastBuff("Ice Block",          ret => Me.HealthPercent < 20 && !Buff.PlayerHasActiveBuff("Hypothermia"), "Ice Block"),
                        Buff.CastBuff("Mage Ward",          ret => Me.HealthPercent < 50, "Mage Ward")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                            ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"), 
                            new PrioritySelector(
                                Buff.CastBuff("Mage Armor",                     ret => true, "Mage Armor"),
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
