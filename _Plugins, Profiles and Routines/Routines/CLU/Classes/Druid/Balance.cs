using System.Linq;

using CLU.Helpers;

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace CLU.Classes.Druid
{
    using global::CLU.GUI;

    class Balance : RotationBase
    {
        public override string Name
        {
            get { return "Balance Druid"; }
        }

        public override string KeySpell
        {
            get { return "Starfall"; }
        }

        public override float CombatMinDistance
        {
            get { return 30f; }
        }

        private static int MushroomCount
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => (o.FactionId == 4 || o.FactionId == 6 || o.FactionId == 8 || o.FactionId == 22) && o.Name.ToUpper().Contains("WILD MUSHROOM") && o.Distance <= 40).Count(o => o.CreatedByUnitGuid == StyxWoW.Me.Guid); }
            // Thanks to falldown who helped with the faction.  Singular and FPS for the logic and code.
        }

        // adding some help about cooldown management
        public override string Help
        {
            get
            {
                return "\n" +
                       "----------------------------------------------------------------------\n" +
                       "This Rotation will:\n" +
                       "1. Attempt to heal with healthstone\n" +
                       "2. Raid buff Mark of the Wild\n" +
                       "3. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Force of Nature & Volcanic Potion & Faerie Fire\n" +
                       "4. AoE with Wild Mushroom, Starfall, \n" +
                       "5. Best Suited for end game raiding\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to Obliv for creating this rotation\n" +
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
                            ret => Buff.PlayerHasBuff("Moonkin Form") && !Buff.PlayerHasActiveBuff("Shadowmeld"),
                            new PrioritySelector(
                                new Decorator(
                                    ret => Units.IsTargetWorthy(Me.CurrentTarget),
                                    new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Items.UseBagItem("Volcanic Potion", ret => Buffs.UnitHasHasteBuff(Me), "Volcanic Potion Heroism/Bloodlust"),
                                        Item.UseEngineerGloves())),
                        // Extra button automation
                        Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                        Spells.CastSpell("Faerie Fire",                     ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Buff.TargetCountDebuff("Faerie Fire") < 3 || !Buffs.UnitHasArmorReductionDebuff(Me.CurrentTarget)), "Faerie Fire"),
                        // Spells.CastSpell("Faerie Fire",                     ret => Buff.TargetDebuffTimeLeft("Faerie Fire").TotalSeconds < 5 && Buff.TargetCountDebuff("Faerie Fire") == 3, "Faerie Fire"),
                        // 8	wild_mushroom_detonate,if=buff.wild_mushroom.stack=3
                        Spells.CastSpell("Wild Mushroom: Detonate",         ret => MushroomCount == 3, "Detonate Shrooms!"),
                        Spells.CastSpell("Insect Swarm",                    ret => (Buff.TargetDebuffTimeLeft("Insect Swarm").TotalSeconds < 2), "Insect Swarm"),
                        // B	wild_mushroom_detonate,moving=0,if=buff.wild_mushroom.stack>0&buff.solar_eclipse.up
                        Spells.CastSpell("Wild Mushroom: Detonate",         ret => MushroomCount > 0 && Buff.PlayerHasBuff("Eclipse (Solar)"), "Detonate Shrooms!"),
                        // Spells.CastSpell("Typhoon",                      ret => Me.IsMoving, "Typhoon (Moving)"),
                        // Starfall when we Lunar Eclipse
                        Spells.CastSelfSpell("Starfall",                    ret => (Units.IsTargetWorthy(Me.CurrentTarget) || Units.CountEnnemiesInRange(Me.Location, 40) >= 3) && Me.CurrentEclipse < -80 && Buff.PlayerHasBuff("Eclipse (Lunar)"), "Starfall"),
                        // Moonfire / Sunfire
                        Spells.CastSpell("Moonfire",                        ret => (!Buff.PlayerHasBuff("Eclipse (Solar)") && Buff.TargetDebuffTimeLeft("Moonfire").TotalSeconds < 2 && !Buff.TargetHasDebuff("Sunfire")) || (Buff.PlayerHasBuff("Eclipse (Solar)") && Buff.TargetDebuffTimeLeft("Sunfire").TotalSeconds < 2 && !Buff.TargetHasDebuff("Moonfire")), "Sunfire"),
                        // Make sure we cast it unless we're about to Eclipse
                        Spells.CastSpell("Starsurge",                       ret => (Me.CurrentEclipse >= -85 && Me.CurrentEclipse <= 85) || (Buff.PlayerHasBuff("Eclipse (Solar)") || Buff.PlayerHasBuff("Eclipse (Lunar)")), "Starsurge"),
                        Spells.CastSelfSpell("Innervate",                   ret => Me.ManaPercent < 50, "Innvervate"),
                        Spells.CastSpellAtLocation("Force of Nature", u => Me.CurrentTarget, ret => Units.IsTargetWorthy(Me.CurrentTarget), "Force of Nature"),
                        Spells.CastSpellAtLocation("Wild Mushroom", u => Me.CurrentTarget, ret => Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 6) >= 3 && MushroomCount < 3, "Wild Mushroom"),
                        // Spells.CastSpell("Wrath",                       ret => Me.CurrentEclipse >= 80 && Me.CastingSpell.Name == "Starfire", "Wrath"),
                        // Spells.CastSpell("Starfire",                    ret => Me.CurrentEclipse <= -87 && Me.CastingSpell.Name == "Wrath", "Starfire"),
                        Spells.CastSpell("Starfire",                        ret => Buff.PlayerHasBuff("Eclipse (Lunar)") || Me.CurrentEclipse == -100, "Starfire"),
                        Spells.CastSpell("Wrath",                           ret => Buff.PlayerHasBuff("Eclipse (Solar)") || Me.CurrentEclipse == 100, "Wrath"),
                        Spells.CastSpell("Starfire",                        ret => Me.CurrentEclipse > 0, "Starfire"),
                        Spells.CastSpell("Wrath",                           ret => Me.CurrentEclipse < 0, "Wrath"),
                        Spells.CastSpell("Starfire",                        ret => true, "Starfire"),
                        Spells.CastSpellAtLocation("Wild Mushroom", u => Me.CurrentTarget, ret => Units.IsTargetWorthy(Me.CurrentTarget) && Me.IsMoving && !Me.CurrentTarget.IsMoving && MushroomCount < 3, "Wild Mushroom"),
                        // Not working for some reason
                        Items.RunMacroText("/cast Starsurge",               ret => Me.IsMoving && Buff.PlayerHasActiveBuff("Shooting Stars"), "Starsurge"),
                        Spells.CastSpell("Moonfire",                        ret => Me.IsMoving, "Moonfire"))))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing,
                    new PrioritySelector(
                        Items.UseBagItem("Healthstone", ret => Me.HealthPercent < 30, "Healthstone")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                            ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                            new PrioritySelector(
                                Buff.CastRaidBuff("Mark of the Wild", ret => !Buff.PlayerHasBuff("Blessing of Kings"), "Mark of the Wild")));
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
