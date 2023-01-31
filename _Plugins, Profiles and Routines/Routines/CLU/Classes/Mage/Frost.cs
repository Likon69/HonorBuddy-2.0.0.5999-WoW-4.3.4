using CLU.Helpers;
using TreeSharp;

namespace CLU.Classes.Mage
{
    using global::CLU.GUI;
    using global::CLU.Lists;

    class Frost : RotationBase
    {
        public override string Name
        {
            get { return "Frost Mage"; }
        }

        public override string KeySpell
        {
            get { return "Deep Freeze"; }
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
                       "Credits to Obliv\n" +
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
                        // Rotation based on SimCraft - Build 15211
                        Spells.CastInterupt("Counterspell",             ret => true, "Counterspell"),
                        Items.RunMacroText("/cast Conjure Mana Gem",    ret => !Items.HaveManaGem(), "Conjure Mana Gem"),
                        Spells.ChannelSelfSpell("Evocation",            ret => Me.ManaPercent < 40 && !Me.IsMoving && (Buff.PlayerHasActiveBuff("Icy Veins") || Buffs.UnitHasHasteBuff(Me)), "Evocation"),
                        Items.UseBagItem("Mana Gem",                    ret => Me.ManaPercent < 90 && Units.IsTargetWorthy(Me.CurrentTarget), "Mana Gem"),
                        Spells.CastSelfSpell("Cold Snap",               ret => Spells.SpellCooldown("Deep Freeze").TotalSeconds > 15 && Spells.SpellCooldown("Flame Orb").TotalSeconds > 30 && Spells.SpellCooldown("Icy Veins").TotalSeconds > 30, "Cold Snap"),
                        Spells.CastSpell("Flame Orb",                   ret => Units.IsTargetWorthy(Me.CurrentTarget), "Frostfire Orb"),
                        Spells.CastSelfSpell("Mirror Image",            ret => Units.IsTargetWorthy(Me.CurrentTarget), "Mirror Image"),
                        Spells.CastSelfSpell("Icy Veins",               ret => !Buff.PlayerHasActiveBuff("Icy Veins") && !Buffs.UnitHasHasteBuff(Me) && (Buff.PlayerCountBuff("Stolen Time") > 7 || Spells.SpellCooldown("Cold Snap").TotalSeconds < 22), "Icy Veins"),
                        Spells.CastSpell("Deep Freeze",                 ret => Buff.PlayerHasActiveBuff("Fingers of Frost"), "Deep Freeze (Fingers of Frost)"),
                        Spells.CastSpell("Frostfire Bolt",              ret => Buff.PlayerHasActiveBuff("Brain Freeze"), "Frostfire Bolt (Brain Freeze)"),
                        // AoE
                        new Decorator(
                            ret => !Me.IsMoving && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 2,
                                new PrioritySelector(
                                    Spells.CastSpellAtLocation("Flamestrike", u => Me.CurrentTarget, ret => !Buff.TargetHasDebuff("Flamestrike") && !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.ManaPercent > 30 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 3, "Flamestrike"),
                                    Spells.ChannelAreaSpell("Blizzard", 11.0, true, 4, 0.0, 0.0, ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.ManaPercent > 30 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 3, "Blizzard")
                                    )),
                        Pets.CastPetSpellAtLocation("Freeze", u => Me.CurrentTarget, ret => true, "Freeze (Pet)"),
                        Spells.CastSpell("Ice Lance",                   ret => Buff.PlayerCountBuff("Fingers of Frost") > 1, "Ice Lance"),
                        // PetSpellCooldown NOT returning correctly although it does return the correct value within the method, when you call it here it returns zero for some reason...still investigating
                        // K	ice_lance,if=buff.fingers_of_frost.react&pet.water_elemental.cooldown.freeze.remains<gcd
                        Spells.CastSpell("Ice Lance", ret => Buff.PlayerHasActiveBuff("Fingers of Frost") && (Pets.PetSpellCooldown("Freeze").TotalSeconds < Spell.GCD), "Ice Lance (water_elemental_Freeze=" + Pets.PetSpellCooldown("Freeze").TotalSeconds + " < " + Spell.GCD + ")"),
                        Spells.CastSpell("Frostbolt",                   ret => true, "Frostbolt (FreezeCD=" + Pets.PetSpellCooldown("Freeze").TotalSeconds + ")"),
                        Spells.CastSpell("Ice Lance",                   ret => Me.IsMoving, "Ice Lance (Moving)"),
                        Spells.CastSpell("Fire Blast",                  ret => Me.IsMoving, "Fire Blast (Moving)")
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
                        Pets.CastPetSummonSpell("Summon Water Elemental", ret => !Me.GotAlivePet, "Water Elemental"),
                        Buff.CastBuff("Molten Armor",                       ret => true, "Molten Armor"),
                        Items.UseBagItem("Healthstone",                     ret => Me.HealthPercent < 30, "Healthstone"),
                        Buff.CastBuff("Ice Block",                          ret => Me.HealthPercent < 20 && !Buff.PlayerHasActiveBuff("Hypothermia"), "Ice Block"),
                        Buff.CastBuff("Mage Ward",                          ret => Me.HealthPercent < 50, "Mage Ward")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                            ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                            new PrioritySelector(
                                Pets.CastPetSummonSpell("Summon Water Elemental", ret => !Me.GotAlivePet, "Water Elemental"),
                                Buff.CastBuff("Molten Armor",               ret => true, "Molten Armor"),
                                Buff.CastRaidBuff("Dalaran Brilliance",     ret => !Buff.PlayerHasBuff("Arcane Brilliance"), "Dalaran Brilliance"),
                                Buff.CastRaidBuff("Arcane Brilliance",      ret => !Buff.PlayerHasBuff("Dalaran Brilliance"), "Arcane Brilliance"), // as most people say, the main difference between Dalaran and Arcane Brilliance, is that Dalaran Brilliance has a totally different casting animation (looks way cooler) that allows you to stand out from most mages and has a 10 yard increase in range.
                                Items.RunMacroText("/cast Conjure Mana Gem", ret => !Me.IsMoving && !Items.HaveManaGem(), "Conjure Mana Gem")));
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