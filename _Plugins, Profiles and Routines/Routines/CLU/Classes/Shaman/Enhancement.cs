using CLU.Helpers;

using TreeSharp;

namespace CLU.Classes.Shaman
{
    using System.Linq;

    using Styx;
    using Styx.WoWInternals.WoWObjects;

    using global::CLU.GUI;
    using global::CLU.Lists;

    class Enhancement : RotationBase
    {
        public override string Name
        {
            get { return "Enhancement Shaman"; }
        }

        public override string KeySpell
        {
            get { return "Stormstrike"; }
        }

        public override float CombatMaxDistance
        {
            get { return 3.2f; }
        }

        // adding some help about cooldown management
        public override string Help
        {
            get
            {
                return "\n" +
                       "----------------------------------------------------------------------\n" +
                       "This Rotation will:\n" +
                       "1. Enchant Weapons: Windfury Weapon(MainHand) & Flametongue Weapon(OffHand)\n" +
                       "2. Totems: Strength of earth, Windfury, Mana Spring, Searing (with range check) \n" +
                       "3. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Feral Spirit \n" +
                       "4. AoE with Magma Totem, Chain Lightning, Fire Nova\n" +
                       "4. Heal using: Lightning Shield\n" +
                       "6. Best Suited for end game raiding\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to fluffyhusky\n" +
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
                                        Item.UseEngineerGloves())),
                        // Extra button automation
                        Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                        // Interupts
                        Spells.CastInterupt("Wind Shear",       ret => true, "Wind Shear"),
                        // Threat
                        Buff.CastBuff("Wind Shear",             ret => Me.CurrentTarget.ThreatInfo.RawPercent > 90, "Wind Shear (Threat)"),
                        // AoE
                        new Decorator(
                                ret => !Me.IsMoving && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 8) >= 2 && !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry),
                                    new PrioritySelector(
                                        Spells.CastTotem("Magma Totem",             ret => Me.Totems.All(t => t.WoWTotem != WoWTotem.Magma), "Magma Totem"),
                                        Spells.CastSpell("Chain Lightning",         ret => Buff.PlayerCountBuff("Maelstrom Weapon") == 5, "Chain Lightning"),
                                        Spells.CastSpell("Flame Shock",             ret => true, "Flame Shock"),
                                        Spells.CastSpell("Lava Lash",               ret => Buff.TargetHasDebuff("Flame Shock"), "Lava Lash"),
                                        Spells.CastSpell("Fire Nova",               ret => true, "Fire Nova"),
                                        Spells.CastSpell("Stormstrike",             ret => true, "Stormstrike")
                                        )),
                        // Default Rotaion		
                        Spells.CastSpell("Lightning Bolt",          ret => Buff.PlayerCountBuff("Maelstrom Weapon") > 4, "Lightning Bolt"),
                        Spells.CastSpell("Feral Spirit",            ret => Units.IsTargetWorthy(Me.CurrentTarget) && Me.CurrentTarget.HealthPercent >= 10, "Feral Spirit"),
                        Spells.CastTotem("Call of the Elements",    ret => Me.Totems.All(t => t.WoWTotem != WoWTotem.StrengthOfEarth), "Call of the Elements"),
                        Spells.CastTotem("Searing Totem",           ret => Me.CurrentTarget.Distance < Totems.GetTotemRange(WoWTotem.Searing) - 2f && !Me.Totems.Any(t => t.Unit != null && t.WoWTotem == WoWTotem.Searing && t.Unit.Location.Distance(Me.CurrentTarget.Location) < Totems.GetTotemRange(WoWTotem.Searing)) && Me.Totems.All(t => t.WoWTotem != WoWTotem.FireElemental), "Searing Totem"),
                        Spells.CastSpell("Stormstrike",             ret => true, "Stormstrike"),
                        Spells.CastSpell("Lava Lash",               ret => true, "Lava Lash"),
                        Spells.CastSpell("Unleash Elements",        ret => true, "Unleash Elements"),
                        Spells.CastSpell("Flame Shock",             ret => Buff.PlayerHasBuff("Unleash Flame"), "Flame Shock (Unleash Flame)"),
                        Buff.CastDebuff("Flame Shock",              ret => true, "Flame Shock"),
                        Spells.CastSpell("Earth Shock",             ret => Buff.TargetDebuffTimeLeft("Flame Shock").TotalSeconds > 5, "Earth Shock"),
                        Buff.CastBuff("Spiritwalker's Grace",       ret => Me.IsMoving, "Spiritwalker's Grace")
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
                        new Decorator(
                            ret => Totems.NeedToRecallTotems,
                            new Sequence(
                                    new Action(ret => CLU.Instance.Log(" [Totems] Recalling Totems")),
                                    new Action(ret => Totems.RecallTotems()))),
                        Items.UseBagItem("Healthstone",             ret => Me.HealthPercent < 30, "Healthstone"),
                        Buff.CastBuff("Lightning Shield",           ret => true, "Lightning Shield"),
                        Buff.CastBuff("Shamanistic Rage",           ret => Me.HealthPercent < 60, "Shamanistic Rage"),
                        Spells.CastSelfSpell("Healing Surge",       ret => Me.HealthPercent < 20, "Healing Surge"),
                        Buff.CastBuff("Windfury Weapon",            ret => !Item.HasWeaponImbue(WoWInventorySlot.MainHand, "Windfury") && Item.HasSuitableWeapon(WoWInventorySlot.MainHand), "Windfury Weapon"),
                        Buff.CastBuff("Flametongue Weapon",         ret => !Item.HasWeaponImbue(WoWInventorySlot.OffHand, "Flametongue") && Item.HasSuitableWeapon(WoWInventorySlot.OffHand), "Flametongue Weapon")
                        ));
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
                                        ret => Totems.NeedToRecallTotems,
                                            new Sequence(
                                                new Action(ret => CLU.Instance.Log(" [Totems] Recalling Totems")),
                                                 new Action(ret => Totems.RecallTotems()))),
                                Buff.CastBuff("Lightning Shield",       ret => true, "Lightning Shield"),
                                Buff.CastBuff("Windfury Weapon", ret => !Item.HasWeaponImbue(WoWInventorySlot.MainHand, "Windfury") && Item.HasSuitableWeapon(WoWInventorySlot.MainHand), "Windfury Weapon"),
                                Buff.CastBuff("Flametongue Weapon", ret => !Item.HasWeaponImbue(WoWInventorySlot.OffHand, "Flametongue") && Item.HasSuitableWeapon(WoWInventorySlot.OffHand), "Flametongue Weapon")
                                ));
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
