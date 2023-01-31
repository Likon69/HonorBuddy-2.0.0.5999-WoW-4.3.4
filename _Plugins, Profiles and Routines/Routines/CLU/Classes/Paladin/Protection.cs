using System.Linq;

using TreeSharp;

namespace CLU.Classes.Paladin
{
    using global::CLU.GUI;
    using global::CLU.Helpers;
    using global::CLU.Lists;

    class Protection : RotationBase
    {
        public override string Name
        {
            get { return "Protection Paladin"; }
        }

        public override string KeySpell
        {
            get { return "Avenger's Shield"; }
        }

        public override float CombatMaxDistance
        {
            get { return 3.2f; }
        }
        
        public override string Help
        {
            get
            {
                return "----------------------------------------------------------------------\n" +
                       "This Rotation will:\n" +
                       "1. Heal using Divine Protection, Lay on Hands, Divine Shield, Hand of Protection\n" +
                       "==> Word of glory & Guardian of Ancient Kings \n" + 
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Avenging Wrath & Lifeblood\n" +
                       "3. Seal of Truth & Seal of Truth swapping for low mana\n" +
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
                    new PrioritySelector(
                    new Decorator(
                            ret => Units.IsTargetWorthy(Me.CurrentTarget),
                                new PrioritySelector(
                                        Item.UseTrinkets(),
                                        Spells.UseRacials(),
                                        Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"), // Thanks Kink
                                        Buff.CastBuff("Avenging Wrath", ret => Buff.PlayerHasBuff("Inquisition"), "Wings with Inquisition"),
                                        Item.UseEngineerGloves())),
                        // Extra button automation
                        Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
                        Buff.CastBuff("Holy Shield",                    ret => true, "Holy Shield"),
						Spells.CastInterupt("Rebuke",         		    ret => true, "Rebuke"),
                        Spells.CastInterupt("Hammer of Justice",        ret => true, "Hammer of Justice"),
                        Spells.CastSpell("Shield of the Righteous",     ret => Me.CurrentHolyPower == 3 && Buff.PlayerHasBuff("Sacred Duty"), "Shield of the Righteous Proc"),
                        Spells.CastSpell("Shield of the Righteous",     ret => Me.CurrentHolyPower == 3 && Buff.PlayerHasBuff("Inquisition"), "Shield of the Righteous"),
                        Spells.CastSelfSpell("Inquisition",             ret => Me.CurrentHolyPower == 3 && !Buff.PlayerHasBuff("Inquisition"), "Inquisition"),
						// AOE
                        Spells.CastSelfSpell("Inquisition", ret => Me.CurrentHolyPower <= 3 && !Buff.PlayerHasBuff("Inquisition") && Unit.EnemyUnits.Count() >= 3, "Inquisition"),
                        Spells.CastAreaSpell("Hammer of the Righteous", 8, false, 2, 0.0, 0.0, ret => true, "Hammer of the Righteous"),
                        // end AoE
                        Spells.CastSpell("Crusader Strike",             ret => true, "Crusader Strike"),
                        Spells.CastSpell("Avenger's Shield",            ret => Buff.PlayerHasBuff("Grand Crusader"), "Avengers Shield Proc"),
                        Spells.CastSpell("Judgement",                   ret => true, "Judgement"),
                        Spells.CastSpell("Avenger's Shield",            ret => true, "Avengers Shield"),
                        Spells.CastSpell("Hammer of Wrath",             ret => true, "Hammer of Wrath"),
                        Spells.CastSpell("Consecration",                ret => !BossList.IgnoreAoE.Contains(Me.CurrentTarget.Entry) && Me.ManaPercent > 70 && !Me.IsMoving && !Me.CurrentTarget.IsMoving && Me.IsWithinMeleeRange, "Consecration"),
                        Spells.CastSpell("Holy Wrath",                  ret => true, "Holy Wrath"),
                        Buff.CastBuff("Divine Plea",                    ret => Me.CurrentMana < 17000 || Me.CurrentHolyPower != 3, "Divine Plea")));
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
                            ret => !Buff.PlayerHasBuff("Forbearance") && !Buff.PlayerHasBuff("Ardent Defender") && !Buff.PlayerHasBuff("Guardian of Ancient Kings"),
                            new PrioritySelector(
                            Buff.CastBuff("Lay on Hands",           ret => Me.HealthPercent < 30, "Lay on Hands"),
                            Buff.CastBuff("Divine Shield",          ret => Me.HealthPercent < 25, "Divine Shield")
                            )),
                        Spells.CastSpell("Word of Glory",           ret => Me.HealthPercent < 65 && Me.CurrentHolyPower > 1, "Word of Glory"),
                        Buff.CastBuff("Guardian of Ancient Kings",  ret => Me.HealthPercent < 50 && !Buff.PlayerHasBuff("Ardent Defender"), "Guardian of Ancient Kings"),
                        Buff.CastBuff("Ardent Defender",            ret => Me.HealthPercent < 30 && !Buff.PlayerHasBuff("Guardian of Ancient Kings"), "Ardent Defender"),
                        Items.UseBagItem("Healthstone",             ret => Me.HealthPercent < 40, "Healthstone"),
                        Buff.CastBuff("Divine Protection",          ret => Me.HealthPercent < 60, "Divine Protection"),
                        Buff.CastBuff("Seal of Truth",              ret => Me.CurrentMana > 9000, "Seal of Truth"),
                        Buff.CastBuff("Seal of Insight",            ret => Me.CurrentMana < 4000, "Seal of Insight for Mana Gen")));
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                        ret => !Me.Mounted && !Me.Dead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                        new PrioritySelector(
                            Buff.CastBuff("Righteous Fury", ret => true, "Righteous Fury"),
                            Buff.CastRaidBuff("Blessing of Kings", ret => !Buff.PlayerHasBuff("Mark of the Wild"), "[Blessing] of Kings"),
                            Buff.CastRaidBuff("Blessing of Might", ret => Buff.PlayerHasBuff("Mark of the Wild"), "[Blessing] of Might")));
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