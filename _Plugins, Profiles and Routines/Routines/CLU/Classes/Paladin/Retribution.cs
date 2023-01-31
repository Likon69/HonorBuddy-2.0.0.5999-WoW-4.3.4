using System.Linq;
using CLU.Helpers;
using CLU.Lists;
using TreeSharp;

namespace CLU.Classes.Paladin
{
    using global::CLU.GUI;

    class Retribution : RotationBase
    {
        private const int ItemSetId = 1064; // Tier set ID


        public override string Name
        {
            get { return "Retribution Paladin"; }
        }

        public override string KeySpell
        {
            get { return "Templar's Verdict"; }
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
                       "2pc Tier set Bonus?: " + Items.Has2PcTeirBonus(ItemSetId) + "\n" +
                       "4pc Tier set Bonus?: " + Items.Has4PcTeirBonus(ItemSetId) + "\n" +
                       "This Rotation will:\n" +
                       "1. Heal using Divine Protection, Lay on Hands, Divine Shield and Hand of Protection\n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Zealotry, Guardian of Ancient Kings and Avenging Wrath\n" +
                       "3. Seal of Righteousness & Seal of Truth swapping for AoE\n" +
                       "4. Best Suited for T13 end game raiding\n" +
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
                    // and here's our normal rotation
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
                        // Buff.CastBuff("Divine Protection", ret => Units.IsMyHourofTwilightSoak(), "Divine Protection"),
                        Spells.CastInterupt("Rebuke",           ret => true, "Rebuke"),
                        // Threat
                        Buff.CastBuff("Hand of Salvation",      ret => Me.GotTarget && Me.CurrentTarget.ThreatInfo.RawPercent > 90, "Hand of Salvation"),
                        // Seal Swapping for AoE
                        Buff.CastBuff("Seal of Righteousness",  ret => Unit.EnemyUnits.Count() >= 4, "Seal of Righteousness"),
                        Buff.CastBuff("Seal of Truth",          ret => Unit.EnemyUnits.Count() < 4, "Seal of Truth"),
                        new Decorator(
                            ret => Buff.PlayerHasBuff("Zealotry"),
                            new PrioritySelector(
                                // Cooldowns
                                Buff.CastBuff("Guardian of Ancient Kings",     ret => Spells.SpellCooldown("Zealotry").TotalSeconds < 1 && Units.IsTargetWorthy(Me.CurrentTarget), "Guardian of Ancient Kings"),
                                Buff.CastBuff("Avenging Wrath",                ret => Buff.PlayerHasBuff("Zealotry") && Units.IsTargetWorthy(Me.CurrentTarget), "Avenging Wrath"),
                                // Zealotry Rotation
                                Spells.CastSelfSpell("Inquisition",             ret => (!Buff.PlayerHasBuff("Inquisition") || Buff.PlayerBuffTimeLeft("Inquisition") <= 4) && (Me.CurrentHolyPower == 3 || Buff.PlayerHasBuff("Divine Purpose")), "Inquisition"),
                                Spells.CastAreaSpell("Divine Storm", 10, false, 4, 0.0, 0.0, ret => Me.CurrentHolyPower < 3, "Divine Storm"),
                                Spells.CastSpell("Crusader Strike", ret => Me.CurrentHolyPower < 3, "Crusader Strike"),
                                Spells.CastSpell("Templar's Verdict",           ret => (Buff.PlayerHasBuff("Divine Purpose") || Me.CurrentHolyPower == 3), "Templar's Verdict"),
                                Spells.CastSpell("Exorcism",                    ret => Buff.PlayerHasActiveBuff("The Art of War"), "Exorcism with The Art of War"),
                                Spells.CastSpell("Hammer of Wrath",             ret => (Buff.PlayerHasBuff("Avenging Wrath") || Me.CurrentTarget.HealthPercent <= 20) && Me.CurrentTarget.MaxHealth > 1, "Hammer of Wrath"),
                                Spells.CastSpell("Holy Wrath",                  ret => true, "Holy Wrath"),
                                Spells.CastSpell("Consecration",                ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.ManaPercent > 70 && !Me.IsMoving && !Me.CurrentTarget.IsMoving && Me.IsWithinMeleeRange, "Consecration"),
                                Spells.CastSelfSpell("Arcane Torrent",          ret => Me.ManaPercent < 80 && Me.CurrentHolyPower != 3, "Arcane Torrent"),
                                Buff.CastBuff("Divine Plea",                    ret => Me.ManaPercent < 75 && Me.CurrentHolyPower != 3, "Divine Plea"))),
                        new Decorator(
                            ret => !Buff.PlayerHasBuff("Zealotry"),
                                new PrioritySelector(
                                // Cooldowns
                                Buff.CastBuff("Guardian of Ancient Kings",      ret => Spells.SpellCooldown("Zealotry").TotalSeconds < 1 && Units.IsTargetWorthy(Me.CurrentTarget), "Guardian of Ancient Kings"),
                                Buff.CastBuff("Zealotry",                       ret => (Me.CurrentHolyPower == 3 || Buff.PlayerHasBuff("Divine Purpose")) && Units.IsTargetWorthy(Me.CurrentTarget), "Zealotry"),
                                // Main Rotation
                                Spells.CastSelfSpell("Inquisition",             ret => (!Buff.PlayerHasBuff("Inquisition") || Buff.PlayerBuffTimeLeft("Inquisition") <= 4) && (Me.CurrentHolyPower == 3 || Buff.PlayerHasBuff("Divine Purpose")), "Inquisition"),
                                Spells.CastAreaSpell("Divine Storm", 10, false, 4, 0.0, 0.0, ret => Me.CurrentHolyPower < 3, "Divine Storm"),
                                Spells.CastSpell("Crusader Strike",             ret => Me.CurrentHolyPower < 3, "Crusader Strike"),
                                // _Spells.CastSpell("Judgement", 					ret => Item.Has2pcTeirBonus(ItemSetId) && Buff.PlayerHasBuff("Zealotry") && Me.CurrentHolyPower < 3, "Judgement (Zealotry)"),
                                Spells.CastSpell("Judgement",                   ret => !Buff.PlayerHasBuff("Zealotry") && Me.CurrentHolyPower < 3, "Judgement"),
                                Spells.CastSpell("Templar's Verdict",           ret => (Buff.PlayerHasBuff("Divine Purpose") || Me.CurrentHolyPower == 3), "Templar's Verdict"),
                                Spells.CastSpell("Exorcism",                    ret => Buff.PlayerHasActiveBuff("The Art of War"), "Exorcism with The Art of War "),
                                Spells.CastSpell("Hammer of Wrath",             ret => (Buff.PlayerHasBuff("Avenging Wrath") || Me.CurrentTarget.HealthPercent <= 20) && Me.CurrentTarget.MaxHealth > 1, "Hammer of Wrath"),
                                Spells.CastSpell("Holy Wrath",                  ret => true, "Holy Wrath"),
                                Spells.CastSpell("Consecration",                ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.ManaPercent > 80 && !Me.IsMoving && !Me.CurrentTarget.IsMoving && Me.IsWithinMeleeRange, "Consecration"),
                                Spells.CastSelfSpell("Arcane Torrent",          ret => Me.ManaPercent < 80 && Me.CurrentHolyPower != 3, "Arcane Torrent"),
                                Buff.CastBuff("Divine Plea",                    ret => Me.ManaPercent < 75 && Me.CurrentHolyPower != 3, "Divine Plea")))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new Decorator(
                    ret => Me.HealthPercent < 100 && SettingsFile.Instance.AutoManageHealing, 
                    new PrioritySelector(
                    Buff.CastBuff("Hand of Freedom",            ret => Me.MovementInfo.ForwardSpeed < 8.05, "Hand of Freedom"),
                    Buff.CastBuff("Cleanse",                    ret => Unit.Instance.UnitIsControlled(Me, false), "Cleanse"),
                    Items.UseBagItem("Healthstone",             ret => Me.HealthPercent < 40, "Healthstone"), 
                    Buff.CastBuff("Divine Protection",          ret => Me.HealthPercent < 80, "Divine Protection"), 
                    new Decorator(
                        ret => !Buff.PlayerHasBuff("Forbearance"), 
                           new PrioritySelector(
                           Buff.CastBuff("Lay on Hands",       ret => Me.HealthPercent < 30, "Lay on Hands"), 
                           Buff.CastBuff("Divine Shield",      ret => Me.HealthPercent < 25, "Divine Shield"), 
                           Buff.CastBuff("Hand of Protection", ret => Me.HealthPercent < 20, "Hand of Protection")))));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                        ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                        new PrioritySelector(
                            Buff.CastBuff("Seal of Truth",             ret => !Buff.PlayerHasBuff("Seal of Truth"), "Seal of Truth"),
                            Buff.CastRaidBuff("Blessing of Kings",     ret => !Buff.PlayerHasBuff("Mark of the Wild"), "[Blessing] of Kings"),
                            Buff.CastRaidBuff("Blessing of Might",     ret => Buff.PlayerHasBuff("Mark of the Wild"), "[Blessing] of Might")));
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
