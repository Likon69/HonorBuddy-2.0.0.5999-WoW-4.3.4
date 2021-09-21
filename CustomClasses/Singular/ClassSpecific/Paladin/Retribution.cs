using System.Linq;
using Singular.Dynamics;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;

using TreeSharp;

namespace Singular.ClassSpecific.Paladin
{
    public class Retribution
    {

        #region Properties & Fields

        private const int RET_T13_ITEM_SET_ID = 1064;

        private static int NumTier13Pieces
        {
            get
            {
                return StyxWoW.Me.CarriedItems.Count(i => i.ItemInfo.ItemSetId == RET_T13_ITEM_SET_ID);
            }
        }

        private static bool Has2PieceTier13Bonus { get { return NumTier13Pieces >= 2; } }

        #endregion

        #region Heal
        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.All)]
        public static Composite CreateRetributionPaladinHeal()
        {
            return new PrioritySelector(
                //Spell.WaitForCast(),
                Spell.Heal("Word of Glory", ret => StyxWoW.Me,
                           ret =>
                           StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.WordOfGloryHealth &&
                           StyxWoW.Me.CurrentHolyPower == 3),
                Spell.Heal("Holy Light", ret => StyxWoW.Me,
                           ret =>
                           !SpellManager.HasSpell("Flash of Light") &&
                           StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.RetributionHealHealth),
                Spell.Heal("Flash of Light", ret => StyxWoW.Me,
                           ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.RetributionHealHealth));
        }

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Rest)]
        [Context(WoWContext.All)]
        public static Composite CreateRetributionPaladinRest()
        {
            return new PrioritySelector( // use ooc heals if we have mana to
                new Decorator(ret => !StyxWoW.Me.HasAura("Drink") && !StyxWoW.Me.HasAura("Food"),
                    CreateRetributionPaladinHeal()),
                // Rest up damnit! Do this first, so we make sure we're fully rested.
                Rest.CreateDefaultRestBehaviour(),
                // Can we res people?
                Spell.Resurrect("Redemption"));
        }
        #endregion

        #region Normal Rotation

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Heal)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateRetributionPaladinNormalPullAndCombat()
        {
            return new PrioritySelector(

                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Heals
                Spell.Heal("Word of Glory",
                    ret => (StyxWoW.Me.CurrentHolyPower == 3 || StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose")) &&
                            StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.WordOfGloryHealth),

                // Defensive
                Spell.BuffSelf("Hand of Freedom",
                    ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                          WoWSpellMechanic.Disoriented,
                                                          WoWSpellMechanic.Frozen,
                                                          WoWSpellMechanic.Incapacitated,
                                                          WoWSpellMechanic.Rooted,
                                                          WoWSpellMechanic.Slowed,
                                                          WoWSpellMechanic.Snared)),

                    Spell.BuffSelf("Divine Shield", ret => StyxWoW.Me.HealthPercent <= 20 && !StyxWoW.Me.HasAura("Forbearance") && (!StyxWoW.Me.HasAura("Horde Flag") || !StyxWoW.Me.HasAura("Alliance Flag"))),
                    Spell.BuffSelf("Divine Protection", ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.DivineProtectionHealthProt),

                    //2	Let's keep up Insight instead of Truth for grinding.  Keep up Righteousness if we need to AoE.  
                //Spell.BuffSelf("Seal of Insight", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4 || StyxWoW.Me.IsInParty),
                    Spell.BuffSelf("Seal of Righteousness", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4),

                    //7	Blow buffs seperatly.  No reason for stacking while grinding.
                    Spell.Cast("Guardian of Ancient Kings", ret => SingularSettings.Instance.Paladin.RetGoatK &&
                        Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4),

                    Spell.Cast("Zealotry", ret => SingularSettings.Instance.Paladin.RetGoatK &&
                        Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4),
                    Spell.BuffSelf("Avenging Wrath", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4 ||
                        (!StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry") && Spell.GetSpellCooldown("Zealotry").TotalSeconds > 10)),
                    Spell.BuffSelf("Blood Fury", ret => SpellManager.HasSpell("Blood Fury") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Berserking", ret => SpellManager.HasSpell("Berserking") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Lifeblood", ret => SpellManager.HasSpell("Lifeblood") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),

                    //Exo is above HoW if we're fighting Undead / Demon
                    Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War") && StyxWoW.Me.CurrentTarget.IsUndeadOrDemon()),
                    //Hammer of Wrath if < 20% or Avenging Wrath Buff
                    Spell.Cast("Hammer of Wrath", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 20 || StyxWoW.Me.ActiveAuras.ContainsKey("Avenging Wrath")),
                    //Exo if we have Art of War
                    Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War")),

                    //crusader_strike,if=holy_power<3
                    Spell.Cast("Crusader Strike", ret => StyxWoW.Me.CurrentHolyPower < 3 &&
                        (Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4 || !SpellManager.HasSpell("Divine Storm"))),
                //Replace CS with DS during AoE
                    Spell.Cast("Divine Storm", ret => StyxWoW.Me.CurrentHolyPower < 3 &&
                        Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4),
                //judgement,if=buff.zealotry.down&holy_power<3
                    Spell.Cast("Judgement", ret => !StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry") && StyxWoW.Me.CurrentHolyPower < 3),
                //inquisition,if=(buff.inquisition.down|buff.inquisition.remains<=2)&(holy_power>=3|buff.divine_purpose.react)
                    Spell.Cast("Inquisition", ret => (!StyxWoW.Me.HasAura("Inquisition") || StyxWoW.Me.ActiveAuras["Inquisition"].TimeLeft.TotalSeconds <= 2) &&
                        (StyxWoW.Me.CurrentHolyPower == 3 || StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose"))),
                //templars_verdict,if=buff.divine_purpose.react or templars_verdict,if=holy_power=3
                    Spell.Cast("Templar's Verdict", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose") || StyxWoW.Me.CurrentHolyPower == 3),
                //judgement,if=set_bonus.tier13_2pc_melee&buff.zealotry.up&holy_power<3
                    Spell.Cast("Judgement", ret => Has2PieceTier13Bonus && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry") && StyxWoW.Me.CurrentHolyPower < 3),
                    Spell.Cast("Judgement"),
                //wait,sec=0.1,if=cooldown.crusader_strike.remains<0.2&cooldown.crusader_strike.remains>0
                //
                //holy_wrath
                    Spell.Cast("Holy Wrath", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4),
                //consecration,not_flying=1,if=mana>16000
                    Spell.Cast("Consecration", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= SingularSettings.Instance.Paladin.ConsecrationCount && StyxWoW.Me.CurrentTarget.Distance <= 5),
                    Spell.Cast("Divine Plea", ret => StyxWoW.Me.CurrentMana < SingularSettings.Instance.Paladin.DivinePleaMana && StyxWoW.Me.HealthPercent > 70),

                    // Move to melee is LAST. Period.
                    Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Behavior(BehaviorType.Heal)]
        [Context(WoWContext.Battlegrounds)]

        public static Composite CreateRetributionPaladinPvPPullAndCombat()
        {
            HealerManager.NeedHealTargeting = true;
            return new PrioritySelector(
                    Safers.EnsureTarget(),
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Helpers.Common.CreateAutoAttack(true),
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                   // Defensive
                    Spell.BuffSelf("Hand of Freedom",
                    ret => !StyxWoW.Me.Auras.Values.Any(a => a.Name.Contains("Hand of") && a.CreatorGuid == StyxWoW.Me.Guid) &&
                           StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                          WoWSpellMechanic.Disoriented,
                                                          WoWSpellMechanic.Frozen,
                                                          WoWSpellMechanic.Incapacitated,
                                                          WoWSpellMechanic.Rooted,
                                                          WoWSpellMechanic.Slowed,
                                                          WoWSpellMechanic.Snared)),

                    Spell.BuffSelf("Divine Shield", ret => StyxWoW.Me.HealthPercent <= 20 && !StyxWoW.Me.HasAura("Forbearance") && (!StyxWoW.Me.HasAura("Horde Flag") || !StyxWoW.Me.HasAura("Alliance Flag"))),
                    Spell.BuffSelf("Divine Protection", ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.DivineProtectionHealthProt),

                    //  Buffs
                    Spell.BuffSelf("Retribution Aura"),
                    Spell.BuffSelf("Seal of Truth", ret => StyxWoW.Me.CurrentTarget.Entry != 28781 && !StyxWoW.Me.CurrentTarget.HasAura("Horde Flag") && !StyxWoW.Me.CurrentTarget.HasAura("Alliance Flag")),
                    Spell.BuffSelf("Seal of Justice", ret => StyxWoW.Me.CurrentTarget.Entry == 28781 || StyxWoW.Me.CurrentTarget.HasAura("Horde Flag") || StyxWoW.Me.CurrentTarget.HasAura("Alliance Flag")),

                    Spell.Cast("Guardian of Ancient Kings", ret => SingularSettings.Instance.Paladin.RetGoatK && StyxWoW.Me.CurrentTarget.Distance < 6 &&
                        (Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 3 || (StyxWoW.Me.CurrentTarget.HasAura("Horde Flag") || StyxWoW.Me.CurrentTarget.HasAura("Alliance Flag")))),

                    Spell.BuffSelf("Zealotry", ret => StyxWoW.Me.CurrentTarget.Distance <= 8),
                    Spell.BuffSelf("Avenging Wrath", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Blood Fury", ret => SpellManager.HasSpell("Blood Fury") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Berserking", ret => SpellManager.HasSpell("Berserking") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Lifeblood", ret => SpellManager.HasSpell("Lifeblood") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),

                    //Hammer of Wrath if < 20% or Avenging Wrath Buff
                    Spell.Cast("Hammer of Wrath", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 20 || StyxWoW.Me.ActiveAuras.ContainsKey("Avenging Wrath")),
                    //Exo if we have Art of War
                    Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War")),

                    Spell.Cast("Crusader Strike", ret => StyxWoW.Me.CurrentHolyPower < 3 &&
                            (Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4 || !SpellManager.HasSpell("Divine Storm"))),
                     Spell.Cast("Divine Storm", ret => StyxWoW.Me.CurrentHolyPower < 3 &&
                        Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4),
                    Spell.BuffSelf("Inquisition", ret => StyxWoW.Me.CurrentHolyPower == 3 || StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose")),
                    Spell.Cast("Templar's Verdict", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose") || StyxWoW.Me.CurrentHolyPower == 3),
                    Spell.Cast("Judgement"),
                    Spell.Cast("Holy Wrath"),
                    Spell.Cast("Consecration", ret => StyxWoW.Me.CurrentTarget.Distance <= Spell.MeleeRange && Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= SingularSettings.Instance.Paladin.ConsecrationCount),
                    Spell.Cast("Divine Plea", ret => StyxWoW.Me.ManaPercent < SingularSettings.Instance.Paladin.DivinePleaMana && StyxWoW.Me.HealthPercent > 70),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Instance Rotation

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateRetributionPaladinInstancePullAndCombat()
        {
            return new PrioritySelector(
                    Safers.EnsureTarget(),
                    Movement.CreateMoveToLosBehavior(),
                    Movement.CreateFaceTargetBehavior(),
                    Helpers.Common.CreateAutoAttack(true),
                    Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                    // Defensive
                    Spell.BuffSelf("Hand of Freedom",
                        ret => !StyxWoW.Me.Auras.Values.Any(a => a.Name.Contains("Hand of") && a.CreatorGuid == StyxWoW.Me.Guid) &&
                                StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                               WoWSpellMechanic.Disoriented,
                                                               WoWSpellMechanic.Frozen,
                                                               WoWSpellMechanic.Incapacitated,
                                                               WoWSpellMechanic.Rooted,
                                                               WoWSpellMechanic.Slowed,
                                                               WoWSpellMechanic.Snared)),

                    Spell.BuffSelf("Divine Shield", ret => StyxWoW.Me.HealthPercent <= 20 && !StyxWoW.Me.HasAura("Forbearance") && (!StyxWoW.Me.HasAura("Horde Flag") || !StyxWoW.Me.HasAura("Alliance Flag"))),
                    Spell.BuffSelf("Divine Protection", ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.DivineProtectionHealthProt),

                    //2	seal_of_truth
                    Spell.BuffSelf("Seal of Truth", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4),
                    Spell.BuffSelf("Seal of Righteousness", ret => Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4 || !SpellManager.HasSpell("Seal of Truth")),

                    //7	guardian_of_ancient_kings,if=cooldown.zealotry.remains<10
                    Spell.Cast("Guardian of Ancient Kings", ret => SingularSettings.Instance.Paladin.RetGoatK && StyxWoW.Me.CurrentTarget.IsBoss() &&
                        Spell.GetSpellCooldown("Zealotry").TotalSeconds < 10),
                //8	zealotry,if=cooldown.guardian_of_ancient_kings.remains>0&cooldown.guardian_of_ancient_kings.remains<292
                    Spell.Cast("Zealotry", ret => SingularSettings.Instance.Paladin.RetGoatK &&
                        //((!StyxWoW.Me.CurrentTarget.IsBoss() || Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4) ||
                        StyxWoW.Me.CurrentTarget.IsBoss() &&
                        Spell.GetSpellCooldown("Guardian of Ancient Kings").TotalSeconds > 0 &&
                        Spell.GetSpellCooldown("Guardian of Ancient Kings").TotalSeconds < 292),
                    Spell.BuffSelf("Avenging Wrath", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Blood Fury", ret => SpellManager.HasSpell("Blood Fury") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Berserking", ret => SpellManager.HasSpell("Berserking") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),
                    Spell.BuffSelf("Lifeblood", ret => SpellManager.HasSpell("Lifeblood") && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry")),

                    //Exo is above HoW if we're fighting Undead / Demon
                    Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War") && StyxWoW.Me.CurrentTarget.IsUndeadOrDemon()),
                    //Hammer of Wrath if < 20% or Avenging Wrath Buff
                    Spell.Cast("Hammer of Wrath", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 20 || StyxWoW.Me.ActiveAuras.ContainsKey("Avenging Wrath")),
                    //Exo is above HoW if we're fighting Undead / Demon
                    Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War")),

                    //crusader_strike,if=holy_power<3
                    Spell.Cast("Crusader Strike", ret => StyxWoW.Me.CurrentHolyPower < 3 &&
                        (Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) < 4 || !SpellManager.HasSpell("Divine Storm"))),
                //Replace CS with DS during AoE
                    Spell.Cast("Divine Storm", ret => StyxWoW.Me.CurrentHolyPower < 3 &&
                        Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= 4),
                //judgement,if=buff.zealotry.down&holy_power<3
                    Spell.Cast("Judgement", ret => !StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry") && StyxWoW.Me.CurrentHolyPower < 3),
                //inquisition,if=(buff.inquisition.down|buff.inquisition.remains<=2)&(holy_power>=3|buff.divine_purpose.react)
                    Spell.Cast("Inquisition", ret => (!StyxWoW.Me.HasAura("Inquisition") || StyxWoW.Me.ActiveAuras["Inquisition"].TimeLeft.TotalSeconds <= 2) &&
                        (StyxWoW.Me.CurrentHolyPower == 3 || StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose"))),
                //templars_verdict,if=buff.divine_purpose.react or templars_verdict,if=holy_power=3
                    Spell.Cast("Templar's Verdict", ret => StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose") || StyxWoW.Me.CurrentHolyPower == 3),
                //judgement,if=set_bonus.tier13_2pc_melee&buff.zealotry.up&holy_power<3
                    Spell.Cast("Judgement", ret => Has2PieceTier13Bonus && StyxWoW.Me.ActiveAuras.ContainsKey("Zealotry") && StyxWoW.Me.CurrentHolyPower < 3),
                //holy_wrath
                    Spell.Cast("Holy Wrath"),
                //consecration,not_flying=1,if=mana>16000
                    Spell.Cast("Consecration", ret => StyxWoW.Me.CurrentTarget.Distance <= Spell.MeleeRange && Unit.NearbyUnfriendlyUnits.Count(u => u.Distance <= 8) >= SingularSettings.Instance.Paladin.ConsecrationCount),
                //wait,sec=0.1,if=cooldown.crusader_strike.remains<0.2&cooldown.crusader_strike.remains>0
                    Spell.Cast("Divine Plea", ret => StyxWoW.Me.ManaPercent < SingularSettings.Instance.Paladin.DivinePleaMana),

                    // Move to melee is LAST. Period.
                    Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        /*
        #region Normal Rotation

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Normal)]
        public static Composite CreateRetributionPaladinNormalPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Heals
                Spell.Heal("Holy Light", ret => StyxWoW.Me, ret => !SpellManager.HasSpell("Flash of Light") && StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.RetributionHealHealth),
                Spell.Heal("Flash of Light", ret => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.RetributionHealHealth),
                Spell.Heal("Word of Glory", ret => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent <= SingularSettings.Instance.Paladin.WordOfGloryHealth && StyxWoW.Me.CurrentHolyPower == 3),

                // Defensive
                Spell.BuffSelf("Hand of Freedom",
                    ret => StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                          WoWSpellMechanic.Disoriented,
                                                          WoWSpellMechanic.Frozen,
                                                          WoWSpellMechanic.Incapacitated,
                                                          WoWSpellMechanic.Rooted,
                                                          WoWSpellMechanic.Slowed,
                                                          WoWSpellMechanic.Snared)),

                // AoE Rotation
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(8f).Count() >= SingularSettings.Instance.Paladin.ConsecrationCount,
                    new PrioritySelector(
                // Cooldowns
                        Spell.BuffSelf("Zealotry"),
                        Spell.BuffSelf("Avenging Wrath"),
                        Spell.BuffSelf("Guardian of Ancient Kings"),
                        Spell.BuffSelf("Divine Storm"),
                        Spell.BuffSelf("Consecration"),
                        Spell.BuffSelf("Holy Wrath")
                        )),

                // Rotation
                Spell.BuffSelf("Inquisition", ret => StyxWoW.Me.CurrentHolyPower == 3),
                Spell.Cast("Hammer of Justice", ret => StyxWoW.Me.HealthPercent <= 40),
                Spell.Cast("Crusader Strike"),
                Spell.Cast("Hammer of Wrath"),
                Spell.Cast("Templar's Verdict",
                    ret => StyxWoW.Me.CurrentHolyPower == 3 &&
                           (StyxWoW.Me.HasAura("Inquisition") || !SpellManager.HasSpell("Inquisition"))),
                Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War")),
                Spell.Cast("Judgement"),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion

        #region Battleground Rotation

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Battlegrounds)]
        public static Composite CreateRetributionPaladinPvPPullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),

                // Defensive
                Spell.BuffSelf("Hand of Freedom",
                    ret => !StyxWoW.Me.Auras.Values.Any(a => a.Name.Contains("Hand of") && a.CreatorGuid == StyxWoW.Me.Guid) &&
                           StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                          WoWSpellMechanic.Disoriented,
                                                          WoWSpellMechanic.Frozen,
                                                          WoWSpellMechanic.Incapacitated,
                                                          WoWSpellMechanic.Rooted,
                                                          WoWSpellMechanic.Slowed,
                                                          WoWSpellMechanic.Snared)),
                Spell.BuffSelf("Divine Shield", ret => StyxWoW.Me.HealthPercent <= 20 && !StyxWoW.Me.HasAura("Forbearance")),

                // Cooldowns
                Spell.BuffSelf("Zealotry"),
                Spell.BuffSelf("Avenging Wrath"),
                Spell.BuffSelf("Guardian of Ancient Kings"),

                // AoE Rotation
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(8f).Count() >= 3,
                    new PrioritySelector(
                        Spell.BuffSelf("Divine Storm"),
                        Spell.BuffSelf("Consecration"),
                        Spell.BuffSelf("Holy Wrath")
                        )),

                // Rotation
                Spell.BuffSelf("Inquisition", ret => StyxWoW.Me.CurrentHolyPower == 3),
                Spell.Cast("Hammer of Justice", ret => StyxWoW.Me.CurrentTarget.HealthPercent <= 40),
                Spell.Cast("Crusader Strike"),
                Spell.Cast("Hammer of Wrath"),
                Spell.Cast("Templar's Verdict",
                    ret => StyxWoW.Me.CurrentHolyPower == 3 &&
                           (StyxWoW.Me.HasAura("Inquisition") || !SpellManager.HasSpell("Inquisition"))),
                Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War")),
                Spell.Cast("Judgement"),
                Spell.BuffSelf("Holy Wrath"),
                Spell.BuffSelf("Consecration"),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion


        #region Instance Rotation

        [Class(WoWClass.Paladin)]
        [Spec(TalentSpec.RetributionPaladin)]
        [Behavior(BehaviorType.Pull)]
        [Behavior(BehaviorType.Combat)]
        [Context(WoWContext.Instances)]
        public static Composite CreateRetributionPaladinInstancePullAndCombat()
        {
            return new PrioritySelector(
                Safers.EnsureTarget(),
                Movement.CreateMoveToLosBehavior(),
                Movement.CreateFaceTargetBehavior(),
                Helpers.Common.CreateAutoAttack(true),
                Helpers.Common.CreateInterruptSpellCast(ret => StyxWoW.Me.CurrentTarget),
                Movement.CreateMoveBehindTargetBehavior(),

                // Defensive
                Spell.BuffSelf("Hand of Freedom",
                    ret => !StyxWoW.Me.Auras.Values.Any(a => a.Name.Contains("Hand of") && a.CreatorGuid == StyxWoW.Me.Guid) &&
                           StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed,
                                                          WoWSpellMechanic.Disoriented,
                                                          WoWSpellMechanic.Frozen,
                                                          WoWSpellMechanic.Incapacitated,
                                                          WoWSpellMechanic.Rooted,
                                                          WoWSpellMechanic.Slowed,
                                                          WoWSpellMechanic.Snared)),
                Spell.BuffSelf("Divine Shield", ret => StyxWoW.Me.HealthPercent <= 20 && !StyxWoW.Me.HasAura("Forbearance")),

                // Cooldowns
                new Decorator(
                    ret => StyxWoW.Me.CurrentTarget.IsBoss(),
                    new PrioritySelector(
                    Spell.BuffSelf("Zealotry"),
                    Spell.BuffSelf("Avenging Wrath"),
                    Spell.BuffSelf("Guardian of Ancient Kings"))),

                // AoE Rotation
                new Decorator(
                    ret => Unit.UnfriendlyUnitsNearTarget(8f).Count() >= SingularSettings.Instance.Paladin.ConsecrationCount,
                    new PrioritySelector(
                        Spell.BuffSelf("Divine Storm"),
                        Spell.BuffSelf("Consecration"),
                        Spell.BuffSelf("Holy Wrath")
                        )),

                // Rotation
                Spell.BuffSelf("Inquisition", ret => StyxWoW.Me.CurrentHolyPower == 3),
                Spell.Cast("Crusader Strike"),
                Spell.Cast("Hammer of Wrath"),
                Spell.Cast("Templar's Verdict",
                    ret => StyxWoW.Me.CurrentHolyPower == 3 &&
                           (StyxWoW.Me.HasAura("Inquisition") || !SpellManager.HasSpell("Inquisition"))),
                Spell.Cast("Exorcism", ret => StyxWoW.Me.ActiveAuras.ContainsKey("The Art of War")),
                Spell.Cast("Judgement"),
                Spell.BuffSelf("Holy Wrath"),
                Spell.BuffSelf("Consecration"),

                Movement.CreateMoveToMeleeBehavior(true)
                );
        }

        #endregion
         */
    }
}
