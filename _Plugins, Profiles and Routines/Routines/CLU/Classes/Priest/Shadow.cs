using CLU.Helpers;

using TreeSharp;

namespace CLU.Classes.Priest
{
    using global::CLU.GUI;
    using global::CLU.Lists;

    class Shadow : RotationBase
    {

        // private static HashSet<int> ItemSetIds { get { return ItemSetId; } }

        // private static readonly HashSet<int> ItemSetId = new HashSet<int>
        // {
        //    1067,
        //    // Tier set ID Regalia of Dying light (Normal)
        //    -491,
        //    // Tier set ID Regalia of Dying light (Heroic)
        //    -472,
        //    // Tier set ID Regalia of Dying light (Raid Finder)
        // };
        
        private const int ItemSetId = 1067; // Tier set ID Regalia of Dying light (Normal)
        // private const int ItemSetIdH = -491; // Tier set ID Regalia of Dying light (Heroic)
        // private const int ItemSetIdRF = -472; // Tier set ID Regalia of Dying light (Raid Finder)

        public override string Name
        {
            get { return "Shadow Priest"; }
        }

        public override string KeySpell
        {
            get { return "Mind Flay"; }
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
                       "2pc Tier set Bonus?: " + Items.Has2PcTeirBonus(ItemSetId) + "\n" +
                       "4pc Tier set Bonus?: " + Items.Has4PcTeirBonus(ItemSetId) + "\n" +
                       "This CC will:\n" +
                       "1. Fade on threat, Shadowform during combat, Dispersion, Power Word: Shield, Flash Heal\n" +
                       "==>Buffs: Power Word: Fortitude, Shadow Protection, Inner Fire, Vampiric Embrace \n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Shadowfiend & Dispersion & Archangel\n" +
                       "3. AoE with Mind Sear\n" +
                       "4. Best Suited for end game raiding\n" +
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
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"),
                                        Item.UseEngineerGloves())),
                        // Threat
                        Buff.CastBuff("Fade", ret => Me.GotTarget && Me.CurrentTarget.ThreatInfo.RawPercent > 90 && !Spells.PlayerIsChanneling, "Fade"),
                        Items.RunMacroText("/cast Shadowform", ret => !Buff.PlayerHasBuff("Shadowform"), "Shadowform"),
                    // Default Rotation
                    new Decorator(
                            ret => SettingsFile.Instance.SpriestRotationSelector,
                                new PrioritySelector(
                                        Spell.StopCast(ret => Me.CastingSpell.Name == "Mind Flay" && (Buff.TargetDebuffTimeLeft("Shadow Word: Pain").TotalSeconds < Buff.DotDelta("Shadow Word: Pain") || Buff.TargetDebuffTimeLeft("Devouring Plague").TotalSeconds < Buff.DotDelta("Devouring Plague") || Buff.TargetDebuffTimeLeft("Vampiric Touch").TotalSeconds < Buff.DotDelta("Vampiric Touch") && Spells.SpellCooldown("Mind Blast").TotalSeconds < Buff.DotDelta("Mind Blast")), "Mind Flay"),
                                        // Multi-Dotting will occour if there are between 1 or more and less than 6 enemys within 15yrds of your current target and you have more than 50% mana and we have Empowered Shadow. //Can be disabled within the GUI
                                         Spells.FindMultiDotTarget(a => Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 1 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) < 5 && Me.ManaPercent > 50 && Me.CurrentTarget.HealthPercent <= 25, "Shadow Word: Death"),
                                         Spells.FindMultiDotTarget(a => Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 1 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) < 5 && Me.ManaPercent > 50 && Me.CurrentTarget.HealthPercent > 25 && Buff.PlayerHasActiveBuff("Empowered Shadow"), "Shadow Word: Pain"),
                                         Spells.FindMultiDotTarget(a => Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) > 4 && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 15) < 6 && Me.ManaPercent > 50 && Me.CurrentTarget.HealthPercent > 25 && Buff.PlayerHasActiveBuff("Empowered Shadow"), "Vampiric Touch"),
                                        // End Multi-Dotting
                                        Spells.CastSpell("Mind Blast",              ret => !Me.IsMoving, "Mind Blast"),
                                        Buff.CastDebuff("Shadow Word: Pain",        ret => true, "Shadow Word: Pain"),
                                        Spells.CastSpell("Mind Sear",               ret => !Me.IsMoving && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 12) > 4 && !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Buff.PlayerHasActiveBuff("Empowered Shadow"), "Mind Sear"),
                                        Buff.CastDebuff("Devouring Plague",         ret => true, "Devouring Plague"),
                                        Buff.CastDebuff("Vampiric Touch",           ret => !Me.IsMoving, "Vampiric Touch"),
                                        Spells.CastSelfSpell("Archangel",           ret => SettingsFile.Instance.AutoManageCooldowns && Buff.PlayerCountBuff("Dark Evangelism") > 4 && Buff.TargetDebuffTimeLeft("Vampiric Touch").TotalSeconds > 5 && Buff.TargetDebuffTimeLeft("Devouring Plague").TotalSeconds > 5, "Archangel"),
                                        Spells.CastSpell("Shadow Word: Death",      ret => (Items.Has2PcTeirBonus(ItemSetId) ? Me.CurrentTarget.HealthPercent <= 100 : Me.CurrentTarget.HealthPercent <= 25), "Shadow Word: Death"),
                                        Spells.CastSelfSpell("Shadowfiend",         ret => Units.IsTargetWorthy(Me.CurrentTarget), "Shadowfiend"),
                                        Spells.CastSpell("Shadow Word: Death",      ret => Me.ManaPercent < 10, "Shadow Word: Death - Low Mana"),
                                        Spells.CastSpecialSpell("Mind Flay",        ret => !Me.IsMoving && Spells.SpellCooldown("Mind Blast").TotalSeconds > Buff.DotDelta("Mind Blast") && Buff.TargetDebuffTimeLeft("Mind Flay").TotalSeconds <= Spells.ClippingDuration() && Buff.TargetHasDebuff("Shadow Word: Pain") && Buff.TargetHasDebuff("Vampiric Touch") && Buff.TargetHasDebuff("Devouring Plague"), "Mind Flay"),
                                        Spells.CastSpell("Shadow Word: Death",      ret => Me.IsMoving, "Shadow Word: Death - Moving"),
                                        Spells.CastSpell("Devouring Plague",        ret => Me.IsMoving && Me.ManaPercent > 10, "Devouring Plague"),
                                        Spells.CastSelfSpell("Dispersion",          ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Me.HealthPercent < 10 || Me.ManaPercent < 10), "Dispersion")
                                    )),
                    // MindSpike Experimental Rotation
                    new Decorator(
                            ret => !SettingsFile.Instance.SpriestRotationSelector,
                                new PrioritySelector(
                                       Spells.CastSpell("Mind Flay",                    ret => !Me.IsMoving && Buff.PlayerCountBuff("Dark Evangelism") < 5 && Spells.SpellCooldown("Shadowfiend").TotalSeconds <= 3, "Mind Flay"),
                                       Spells.CastSelfSpell("Shadowfiend",              ret => Units.IsTargetWorthy(Me.CurrentTarget) && Buff.PlayerCountBuff("Dark Evangelism") > 4 && Spells.SpellCooldown("Archangel").TotalSeconds <= 0.1, "Shadowfiend"),
                                       Spells.CastSelfSpell("Archangel",                ret => SettingsFile.Instance.AutoManageCooldowns && Buff.PlayerCountBuff("Dark Evangelism") > 4 && Me.GotAlivePet, "Archangel"),
                                       Items.RunMacroText("/cancelaura Mind Melt",      ret => Buff.PlayerHasActiveBuff("Mind Melt") && Spells.SpellCooldown("Mind Blast").TotalSeconds <= 0.1, "[CancelAura] Mind Melt"),
                                       Spell.StopCast(ret => Me.CastingSpell.Name == "Mind Flay" && (Buff.TargetDebuffTimeLeft("Shadow Word: Pain").TotalSeconds < (Spell.GCD + 0.5) || Buff.TargetDebuffTimeLeft("Devouring Plague").TotalSeconds < (Spell.GCD + 1) || Buff.TargetDebuffTimeLeft("Vampiric Touch").TotalSeconds < (Spells.CastTime("Vampiric Touch"))) && Buff.PlayerCountBuff("Dark Evangelism") == 5 && Spells.SpellCooldown("Shadowfiend").TotalSeconds > 3, "Mind Flay"),
                                       Spells.CastSpell("Mind Blast",                   ret => !Me.IsMoving, "Mind Blast"),
                                       Spells.CastSpell("Shadow Word: Death",           ret => (Items.Has2PcTeirBonus(ItemSetId) ? Me.CurrentTarget.HealthPercent <= 100 : Me.CurrentTarget.HealthPercent <= 25) && Me.GotAlivePet, "Shadow Word: Death"),
                                       Spells.CastSpell("Mind Spike",                   ret => !Me.IsMoving && Me.GotAlivePet && Spells.SpellCooldown("Shadowfiend").TotalSeconds > (Spell.GCD + 0.6), "Mind Spike"),  // && !Buffs.UnitHasHasteBuff(Me)
                                       Buff.CastDebuff("Vampiric Touch",                ret => !Me.IsMoving, "Vampiric Touch"),
                                       Buff.CastDebuff("Shadow Word: Pain",             ret => true, "Shadow Word: Pain (GCD+0.5=" + (Spell.GCD + 0.5) + ")"),
                                       Spells.CastSpell("Mind Sear",                    ret => !Me.IsMoving && Units.CountEnnemiesInRange(Me.CurrentTarget.Location, 12) > 4 && !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry), "Mind Sear"),
                                       Buff.CastDebuff("Devouring Plague",              ret => true, "Devouring Plague (GCD+1=" + (Spell.GCD + 1) + ")"),
                                       Spells.CastSelfSpell("Archangel",                ret => SettingsFile.Instance.AutoManageCooldowns && Buff.PlayerCountBuff("Dark Evangelism") > 4 && Buff.TargetDebuffTimeLeft("Vampiric Touch").TotalSeconds > 5 && Buff.TargetDebuffTimeLeft("Devouring Plague").TotalSeconds > 5, "Archangel"),
                                       Spells.CastSpell("Shadow Word: Death",           ret => (Items.Has2PcTeirBonus(ItemSetId) ? Me.CurrentTarget.HealthPercent <= 100 : Me.CurrentTarget.HealthPercent <= 25), "Shadow Word: Death"),
                                       // Spells.CastSpell("Shadow Word: Death",        ret => Me.CurrentTarget.HealthPercent <= 25 || Me.CurrentTarget.MaxHealth == 1, "Shadow Word: Death"), // will cast on cooldown on the dummy.
                                       Spells.CastSelfSpell("Shadowfiend",              ret => Units.IsTargetWorthy(Me.CurrentTarget), "Shadowfiend"),
                                       Spells.CastSpell("Shadow Word: Death",           ret => Me.ManaPercent < 10, "Shadow Word: Death - Low Mana"),
                                       Spells.CastSpell("Mind Flay",                    ret => !Me.IsMoving, "Mind Flay"),
                                       Spells.CastSpell("Shadow Word: Death",           ret => Me.IsMoving, "Shadow Word: Death - Moving"),
                                       Buff.CastDebuff("Devouring Plague",              ret => Me.IsMoving && Me.ManaPercent > 10, "Devouring Plague"),
                                       Spells.CastSelfSpell("Dispersion",               ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Me.HealthPercent < 10 || Me.ManaPercent < 10), "Dispersion")
                                    ))
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
                        Spells.CastSelfSpell("Power Word: Shield",  ret => !Buff.PlayerHasBuff("Weakened Soul") && Me.HealthPercent < 60, "Power Word: Shield"),
                        Items.UseBagItem("Healthstone",             ret => Me.HealthPercent < 40, "Healthstone"),
                        Spells.CastSpell("Flash Heal",              ret => Me.HealthPercent < (Me.Combat ? 10 : 50), "Emergency flash heal")));
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
                            // Items.RunMacroText("/cast Shadowform", ret => !Buff.PlayerHasBuff("Shadowform"), "Shadowform"),
                            Buff.CastRaidBuff("Power Word: Fortitude", ret => true, "Power Word: Fortitude"),
                            Buff.CastRaidBuff("Shadow Protection", ret => true, "Shadow Protection"),
                            Buff.CastBuff("Inner Fire", ret => true, "Inner Fire"),
                            Buff.CastBuff("Vampiric Embrace", ret => true, "Vampiric Embrace"))));
            }
        }

        public override Composite PVPRotation
        {
            get { return this.SingleRotation; }
        }

        public override Composite PVERotation
        {
            get { return this.SingleRotation; }
        }
    }
}
