using CLU.Helpers;
using CLU.Lists;

using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace CLU.Classes.Druid
{
    using global::CLU.GUI;

    class Feral : RotationBase
    {
        private const int ItemSetId = 1058; // Tier set ID

        public override string Name
        {
            get { return "Feral Druid"; }
        }

        public override float CombatMaxDistance
        {
            get { return 3.2f; }
        }

        public override string KeySpell
        {
            get { return "Mangle"; }
        }

        public override string Help
        {
            get
            {
                return "\n" +
                       "----------------------------------------------------------------------\n" +
                       "2pc Tier set Bonus?: " + Items.Has2PcTeirBonus(ItemSetId) + "\n" +
                       "4pc Tier set Bonus?: " + Items.Has4PcTeirBonus(ItemSetId) + "\n" +
                       "This Rotation will:\n" +
                       "1. Heal using Frenzied Regeneration, Survival Instincts, Barkskin\n" +
                       "2. AutomaticCooldowns has: \n" +
                       "==> UseTrinkets \n" +
                       "==> UseRacials \n" +
                       "==> UseEngineerGloves \n" +
                       "==> Enrage & Berserk & Berserking & Lifeblood\n" +
                       "3. Checks for T13 2pc Kitty Bonus\n" +
                       "4. Ignores Shred for Ultraxion\n" +
                       "5. Hot swap rotation from bear to cat if you change form\n" +
                       "6. This Rotation will not get you a beer\n" +
                       "NOTE: PvP uses single target rotation - It's not designed for PvP use. \n" +
                       "Credits to Singular, mrwowbuddy and cowdude\n" +
                       "----------------------------------------------------------------------\n";
            }
        }

        private bool IsBehind(WoWUnit target)
        {
            // WoWMathHelper.IsBehind(Me.Location, target.Location, target.Rotation, (float)Math.PI * 5 / 6)
            return target != null && target.MeIsBehind;
        }

        public override Composite SingleRotation
        {
            get 
            {
                return new Decorator(ret => !SettingsFile.Instance.AutoManagePauseRotation, 
                    new Sequence(
					new PrioritySelector(
                        // HandleMovement? If so, Choose our form!
                        new Decorator(
                            ret => SettingsFile.Instance.AutoHandleMovement,
                                 new PrioritySelector(
                                    Spell.Instance.CastSelfSpell("Bear Form", ret => Me.HealthPercent <= 20, "Bear Form"),
                                     Spell.Instance.CastSelfSpell("Cat Form", ret => Me.HealthPercent > 20, "Cat Form"))),
					    new Decorator(
					        ret => Buff.PlayerHasBuff("Bear Form"), 
					        new PrioritySelector(
					            // Cooldowns
					            new Decorator(
					                ret => Units.IsTargetWorthy(Me.CurrentTarget), 
					                new PrioritySelector(
                                                    Item.UseTrinkets(),
                                                    Spells.UseRacials(),
                                                    Spells.CastSelfSpell("Enrage", ret => !Spells.CanCast("Berserk", Me) && Me.CurrentRage < 80, "Enrage"), 
                                                    Spells.CastSelfSpell("Berserk", ret => Buff.PlayerHasBuff("Pulverize") && Buff.TargetCountDebuff("Lacerate") >= 1, "Berserk"), 
                                                    Item.UseEngineerGloves(),
                                                    Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"))),
					            // Extra button automation
                                Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
								// Interupts
                                Spells.CastInterupt("Skull Bash",           ret => true, "Skull Bash"),							
								// 1.5 Keep up at least 1 stack of Lacerate up during beserk
                                Buff.CastDebuff("Lacerate",                ret => (Buff.TargetDebuffTimeLeft("Lacerate").TotalSeconds < 2) && Buff.PlayerHasBuff("Berserk"), "Lacerate (Beserk)"), 
								// 2. Mangle on cooldown
                                Spells.CastSpell("Mangle (Bear)",           ret => true, "Mangle"), 
								// 3.1 AoE Swipe
                                Spells.CastAreaSpell("Swipe (Bear)", 8, false, 3, 0.0, 0.0, ret => true, "Swipe"), 
								// 1. Keep Demoralizing Roar up
                                Buff.CastDebuff("Demoralizing Roar",       ret => !Buffs.UnitHasDamageReductionDebuff(Me.CurrentTarget), "Demoralizing Roar"), 
								// 3. Thrash
								Spells.CastSpell("Thrash", 				    ret => true, "Thrash"), 
								// 3.2 Maul & Clearcasting AoE
								Spells.CastAreaSpell("Maul", 5, false, 3, 0.0, 0.0, ret => Buff.PlayerHasBuff("Clearcasting"), "Maul"), 
								// 4. Keep up at least 1 stack of Lacerate
					            Buff.CastDebuff("Lacerate",                ret => (Buff.TargetDebuffTimeLeft("Lacerate").TotalSeconds < 2) && !Spells.CanCast("Pulverize", Me.CurrentTarget), "Lacerate"), 
								// 5. Keep up the Pulverize buff
								Spells.CastSpell("Pulverize", 			    ret => Buff.PlayerHasBuff("Clearcasting") && !Buff.PlayerHasBuff("Pulverize") && (Buff.TargetCountDebuff("Lacerate") >= 1), "Pulverize (Clear Casting)"), 
								Spells.CastSpell("Pulverize", 			    ret => !Buff.PlayerHasBuff("Pulverize") && (Buff.TargetCountDebuff("Lacerate") > 2), "Pulverize"),							
								// 6. Faerie Fire
								Spells.CastSpell("Faerie Fire (Feral)",     ret => Buff.TargetCountDebuff("Faerie Fire") < 3 && !Buffs.UnitHasArmorReductionDebuff(Me.CurrentTarget), "Faerie Fire (Feral)"), 
								// 7. Keep up a 3 stack of Lacerate
                                Buff.CastDebuff("Lacerate",                ret => Buff.TargetCountDebuff("Lacerate") < 3, "Lacerate (< 3)"), 
								// 8. Spend excess rage on Maul
								Spells.CastSpell("Maul", 				    ret => Me.RagePercent > 70, "Maul"),
					            Spells.CastSpell("Lacerate",                ret => true, "Lacerate"))),
					    //--------------------------------------------------------------------------------------------------------------------------------------------------------------- cat / bear
					    new Decorator(
					        ret => Buff.PlayerHasBuff("Cat Form"),
					        new PrioritySelector(
					            // Cooldowns
					            new Decorator(
					                ret => Units.IsTargetWorthy(Me.CurrentTarget),
					                    new PrioritySelector(
                                                Item.UseTrinkets(),
                                                Spells.UseRacials(),
					                            Item.UseEngineerGloves(), 
                                                Buff.CastBuff("Lifeblood", ret => true, "Lifeblood"))),  // Thanks Kink
					            // Extra button automation
                                Items.RunMacroText("/click ExtraActionButton1", ret => (Units.IsFadingLight() || Units.IsShrapnel() || Units.IsHourofTwilight()) && SettingsFile.Instance.AutoExtraActionButton, "[Push Button] ExtraActionButton1"),
					            // Interupts
                                Spells.CastInterupt("Skull Bash",               ret => true, "Skull Bash"), 
								// 1. Tiger's Fury
                                Spells.CastSelfSpell("Tiger's Fury",            ret => Me.CurrentEnergy <= (Items.Has2PcTeirBonus(ItemSetId) ? 45 : 35) && !Buff.PlayerHasBuff("Clearcasting"), "Tigers Fury"), 
								// Kitty Cooldown
                                Spells.CastSelfSpell("Berserk",                 ret => Units.IsTargetWorthy(Me.CurrentTarget) && (Buff.PlayerHasBuff("Tiger's Fury") || Buff.TargetDebuffTimeLeft("Tiger's Fury").TotalSeconds > 6), "Berserk"), 
                                Spells.CastSelfSpell("Berserking",              ret => Units.IsTargetWorthy(Me.CurrentTarget) && Buff.PlayerHasBuff("Tiger's Fury"), "Berserking"), 
								// 2. AoE Swipe
								Spells.CastAreaSpell("Swipe (Cat)", 8, false, 3, 0.0, 0.0, ret => true, "Swipe"), 
								// 2. Mangle 
                                Spells.CastSpell("Mangle (Cat)",                ret => !Buffs.UnitHasBleedDamageDebuff(Me.CurrentTarget), "Mangle"), 
                                Items.RunMacroText("/cast Ravage",              ret => Buff.PlayerHasBuff("Stampede") && Buff.TargetDebuffTimeLeft("Stampede").TotalSeconds <= 1, "Ravage"),
                                Spells.CastSpell("Ferocious Bite",              ret => Me.ComboPoints >= 1 && Buff.TargetHasDebuff("Rip") && Buff.TargetDebuffTimeLeft("Rip").TotalSeconds <= 2.1 && ((Me.CurrentTarget.HealthPercent <= (Items.Has2PcTeirBonus(ItemSetId) ? 60 : 25)) || Me.CurrentTarget.MaxHealth == 1), "Ferocious Bite Rip"),
                                Spells.CastSpell("Ferocious Bite",              ret => Me.ComboPoints == 5 && Buff.TargetHasDebuff("Rip") && ((Me.CurrentTarget.HealthPercent <= (Items.Has2PcTeirBonus(ItemSetId) ? 60 : 25)) || Me.CurrentTarget.MaxHealth == 1), "Ferocious Bite Rip"), 
								// 2. Extend Rip
                                Spells.CastSpell("Shred",                       ret => !BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry) && Buff.TargetHasDebuff("Rip") && Buff.TargetDebuffTimeLeft("Rip").TotalSeconds <= 4, "Shred (Extend Rip)"), 
                                Spells.CastSpell("Mangle (Cat)",                ret => (!IsBehind(Me.CurrentTarget) || BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry)) && Buff.TargetHasDebuff("Rip") && Buff.TargetDebuffTimeLeft("Rip").TotalSeconds <= 4 && (Me.CurrentTarget.HealthPercent > (Items.Has2PcTeirBonus(ItemSetId) ? 60 : 25) || Me.CurrentTarget.MaxHealth == 1), "Mangle (Extend Rip) [NotBehind]"),
                                Buff.CastDebuff("Rip",                         ret => Me.ComboPoints == 5 && Buff.TargetDebuffTimeLeft("Rip").TotalSeconds < 2 && (Buff.PlayerHasBuff("Berserk") || (Buff.TargetDebuffTimeLeft("Rip").TotalSeconds <= Spells.SpellCooldown("Tiger's Fury").TotalSeconds)), "Rip"), 
								Spells.CastSpell("Ferocious Bite", 				ret => Me.ComboPoints == 5 && Buff.TargetDebuffTimeLeft("Rip").TotalSeconds > 5 && Buff.PlayerBuffTimeLeft("Savage Roar") >= 3 && Buff.PlayerHasBuff("Berserk"), "Ferocious Bite"), 
                                Buff.CastDebuff("Rake",                        ret => Buff.PlayerHasBuff("Tiger's Fury") && (Buff.TargetDebuffTimeLeft("Rake").TotalSeconds < 8.9), "Rake (TF.up & Rake < 9)"), 
								Buff.CastDebuff("Rake", 						ret => Buff.TargetDebuffTimeLeft("Rake").TotalSeconds < 2.9 && (Buff.PlayerHasBuff("Berserk") || Me.CurrentEnergy > 70 || ((Spells.SpellCooldown("Tiger's Fury").TotalSeconds + 0.1) > Buff.TargetDebuffTimeLeft("rake").TotalSeconds)), "Rake"), 
                                Spells.CastSpell("Shred",                       ret => !BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry) && Buff.PlayerHasBuff("Clearcasting"), "Shred (OoC)"), 
                                Spells.CastSpell("Mangle (Cat)",                ret => (!IsBehind(Me.CurrentTarget) || BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry)) && Buff.PlayerHasBuff("Clearcasting"), "Mangle (OoC) [NotBehind]"), 
								// Keep up Savage Roar
                                Spells.CastSpell("Savage Roar",                 ret => Me.ComboPoints > 0 && Buff.PlayerBuffTimeLeft("Savage Roar") <= 1, "Savage Roar"), 
                                Items.RunMacroText("/cast Ravage",              ret => Buff.PlayerHasBuff("Stampede") && Spells.SpellCooldown("Tiger's Fury").TotalSeconds < 1, "Ravage"),
                                Spells.CastSpell("Ferocious Bite",              ret => Me.ComboPoints == 5 && Me.CurrentTarget.HealthPercent < 10, "Ferocious Bite"), 
								Spells.CastSpell("Ferocious Bite", 				ret => Me.ComboPoints == 5 && Buff.TargetDebuffTimeLeft("Rip").TotalSeconds >= 14 && Buff.PlayerBuffTimeLeft("Savage Roar") >= 10, "Ferocious Bite"), 
                                Items.RunMacroText("/cast Ravage",              ret => Buff.PlayerHasBuff("Stampede") && Buff.PlayerHasBuff("Tiger's Fury") && !Buff.PlayerHasBuff("Clearcasting") && Me.CurrentEnergy > 80, "Ravage"), 
                                Spells.CastSpell("Shred",                       ret => !BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry) && (Buff.PlayerHasBuff("Berserk") || Buff.PlayerHasBuff("Tiger's Fury")), "Shred (TF or Beserk)"), 
                                Spells.CastSpell("Mangle (Cat)",                ret => (!IsBehind(Me.CurrentTarget) || BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry)) && (Buff.PlayerHasBuff("Berserk") || Buff.PlayerHasBuff("Tiger's Fury")), "Mangle (TF or Beserk) [NotBehind]"), 
                                Spells.CastSpell("Shred",                       ret => !BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry) && (Me.ComboPoints < 5 || Buff.TargetDebuffTimeLeft("Rip").TotalSeconds < 3) || (Me.ComboPoints == 0 && Buff.PlayerBuffTimeLeft("Savage Roar") < 2), "Shred"), 
                                Spells.CastSpell("Mangle (Cat)",                ret => (!IsBehind(Me.CurrentTarget) || BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry)) && (Me.ComboPoints < 5 || Buff.TargetDebuffTimeLeft("Rip").TotalSeconds < 3) || (Me.ComboPoints == 0 && Buff.PlayerBuffTimeLeft("Savage Roar") < 2) && Me.CurrentEnergy > 40, "Mangle [NotBehind]"), 
                                Spells.CastSpell("Shred",                       ret => !BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry) && Spells.SpellCooldown("Tiger's Fury").TotalSeconds <= 3, "Shred"), 
                                Spells.CastSpell("Mangle (Cat)",                ret => (!IsBehind(Me.CurrentTarget) || BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry)) && Spells.SpellCooldown("Tiger's Fury").TotalSeconds <= 3, "Mangle (TF) [NotBehind]"), 
                                Spells.CastSpell("Shred",                       ret => !BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry) && Me.CurrentEnergy > 80, "Shred (Capping)"), 
                                Spells.CastSpell("Mangle (Cat)",                ret => (!IsBehind(Me.CurrentTarget) || BossList.IgnoreShred.Contains(Me.CurrentTarget.Entry)) && Me.CurrentEnergy > 80, "Mangle (Capping) [NotBehind]"),
					            Buff.CastDebuff("Faerie Fire (Feral)",          ret => Buff.TargetCountDebuff("Faerie Fire") < 3 && !Buffs.UnitHasArmorReductionDebuff(Me.CurrentTarget), "Faerie Fire (Feral)"))))));
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
					        ret => Buff.PlayerHasBuff("Bear Form"),
                            new PrioritySelector(
                                Spells.CastSelfSpell("Frenzied Regeneration",   ret => Me.HealthPercent <= 25 && !Buff.PlayerHasBuff("Survival Instincts"), "Frenzied Regeneration"), 
					            Spells.CastSelfSpell("Survival Instincts",      ret => Me.HealthPercent <= 40 && !Buff.PlayerHasBuff("Frenzied Regeneration"), "Survival Instincts"),
                                Spells.CastSelfSpell("Barkskin",                ret => Me.HealthPercent <= 80, "Barkskin"),
                                Items.UseBagItem("Healthstone",                 ret => Me.HealthPercent < 40, "Healthstone"))),
                        new Decorator(
                            ret => Buff.PlayerHasBuff("Cat Form"),
                            new PrioritySelector(
                                Spells.CastSelfSpell("Survival Instincts",      ret => Me.HealthPercent <= 40, "Survival Instincts"),
					            Spells.CastSelfSpell("Barkskin",                ret => Me.HealthPercent <= 80, "Barkskin"),
                                Items.UseBagItem("Healthstone",                 ret => Me.HealthPercent < 40, "Healthstone")))));
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