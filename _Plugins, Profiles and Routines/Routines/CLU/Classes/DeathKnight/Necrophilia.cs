using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Necrophilia
{
    public class DeathKnight : CombatRoutine
    {
        public override sealed string Name
        {
            get { return "Necro_s"; }
        }

        public override WoWClass Class
        {
            get { return WoWClass.DeathKnight; }
        }

        private static LocalPlayer MeOb
        {
            get { return ObjectManager.Me; }
        }

        private static LocalPlayer MeSt
        {
            get { return StyxWoW.Me; }
        }

        private static WoWUnit CurTarget
        {
            get { return StyxWoW.Me.CurrentTarget; }
        }

        public static List<WoWUnit> NearbyUnfriendlyUnits
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(p =>
                                                                                   !p.IsFriendly
                                                                                   && (p.IsTargetingMeOrPet
                                                                                       || p.IsTargetingMyPartyMember
                                                                                       || p.IsTargetingMyRaidMember
                                                                                       || p.IsPlayer
                                                                                       || p.MaxHealth == 1)
                                                                                   && !p.IsNonCombatPet && !p.IsCritter
                                                                                   && p.Distance2DSqr <= (24 * 24)).
                    ToList();
            }
        }

        public override Composite PreCombatBuffBehavior
        {
            get
            {
                return
                    new Decorator(_PreCombatBuffBehavior());
            }
        }

        private static void Slog(string format, params object[] args)
        {
            if (format != null)
                Logging.Write(Color.WhiteSmoke, format, args);
        }

        public override void Initialize()
        {
            if (MeOb.IsValid)
                Slog(@"Init rdy");
            if (!MeOb.IsValid)
                Slog("Get valid mofo");
        }

        public override void Pulse()
        {
            if (MeOb.Combat)
            {
                Logging.Write(
                    Color.Red,
                    "[F:{0}] [U:{1}] [D:{2}] [RP:{3}]",
                    MeOb.FrostRuneCount,
                    MeOb.UnholyRuneCount,
                    MeOb.DeathRuneCount,
                    MeOb.CurrentRunicPower);
            }

            if (MeOb.Dead && !MeOb.Combat)
                Logging.Write(Color.Red, "Dead! ...Seriously...?");
            if (MeOb.Mounted && !MeOb.Combat)
                Logging.Write(Color.Red, "Mounted!");
            if (MeOb.Dead && MeOb.Combat)
                Logging.Write(Color.Red, "You died, idiot!");
            if (MeOb.Mounted && MeOb.Combat)
                Logging.Write(Color.Red, "You're mounted and want to start a fight? Duh...!");
        }

        public static Composite _PreCombatBuffBehavior()
        {
            return
                new PrioritySelector(
                    new Decorator(
                        ret => MeOb.CurrentTarget != null,
                        new Decorator(
                            ret => !MeOb.CurrentTarget.IsFriendly,
                            BuffMe("Horn of Winter", ret => true, "Horn of Winter", "Horn of Winter"))),
                    BuffMe(
                        "Path of Frost",
                        ret => (MeOb.Mounted && !MeOb.Combat) || MeOb.IsSwimming,
                        "Path of Frost",
                        "Path of Frost"));
        }

        #region Category: Combat

        public override Composite CombatBehavior
        {
            get
            {
                return
                    new PrioritySelector(
                        new Decorator(
                            ret => SpellManager.HasSpell("Frost Strike"),
                            new PrioritySelector(
                                new Decorator(ret => !MeOb.Combat || !MeOb.IsWithinMeleeRange, _PreCombatBuffBehavior()),
                                new Decorator(ret => MeOb.Combat && MeOb.IsInRaid, FrostRaidBehavior()),
                                new Decorator(ret => MeOb.Combat && !MeOb.IsInRaid, FrostSoloBehavior()))),
                        new Decorator(
                            ret => SpellManager.HasSpell("Scourge Strike"),
                            new PrioritySelector(
                                new Decorator(ret => !MeOb.Combat || !MeOb.IsWithinMeleeRange, _PreCombatBuffBehavior()),
                                new Decorator(ret => MeOb.Combat && MeOb.IsInRaid, UnholyRaidBehaviour()),
                                new Decorator(ret => MeOb.Combat && !MeOb.IsInRaid, UnholySoloBehaviour()))));
            }
        }

        public Composite FrostRaidBehavior()
        {
            return
                new PrioritySelector(
                    Buff(
                        "Dark Simulacrum",
                        true,
                        ret => CurTarget,
                        ret => CurTarget.IsBoss() && CurTarget.IsCasting && CurTarget.CastingSpellId == 108567,
                        "Dark Simulacrum",
                        "Dark Simulacrum"),
                    BuffMe(
                        "Anti-Magic Shell",
                        ret => MeOb.HealthPercent < 80 || (CurTarget.IsBoss() && CurTarget.IsTargetingMeOrPet),
                        "Anti-Magic Shell",
                        "Anti-Magic Shell"),
                    BuffMe(
                        "Icebound Fortitude",
                        ret => MeSt.HealthPercent < 60 && CurTarget.IsBoss(),
                        "Icebound Fortitude",
                        "Icebound Fortitude"),
                    CastSpell(
                        "Howling Blast",
                        ret => CurTarget,
                        ret => NearbyUnfriendlyUnits.Count(a => a.DistanceSqr <= (15 * 15)) > 2,
                        "Howling Blast"),
                    Frostdiseases(),
                    BuffMe("Pillar of Frost", ret => CurTarget.IsBoss(), "Pillar of Frost", "Pillar of Frost"),
                    Buff("Raise Dead", true, ret => MeSt, ret => CurTarget.IsBoss(), "Raise Dead", "Pillar of Frost"),
                    Buff("Raise Dead", true, ret => MeSt, ret => CurTarget.IsBoss(), "Raise Dead", "Unholy Strength"),
                    CastSpell(
                        "Empower Rune Weapon",
                        ret => MeOb.FrostRuneCount == 0 && MeOb.UnholyRuneCount < 2 && MeOb.DeathRuneCount == 0,
                        "Empower Rune Weapon 0<20"),
                    new Decorator(
                        ret => CurTarget.IsBoss(),
                        new Action(ret =>
                                                 {
                                                     UseTinkers();
                                                     UseTrinkets();
                        })),
                    CastSpell("Howling Blast", ret => NearbyUnfriendlyUnits.Count > 2, "Howling Blast Aoe"),
                    CastSpell("Frost Strike", ret => NearbyUnfriendlyUnits.Count > 2, "Frost Strike Aoe"),
                    CastSpell(
                        "Obliterate",
                        ret => MeOb.FrostRuneCount == 2 && MeOb.UnholyRuneCount == 2 && MeOb.DeathRuneCount == 2,
                        "Obliterate FFUUDD"),
                    // Looks like a double up of code (Same as above)?
                    // CastSpell(
                    // "Obliterate",
                    // ret => MeOb.FrostRuneCount == 2 && MeOb.UnholyRuneCount == 2 || MeOb.DeathRuneCount == 2,
                    // "Obliterate FFUU | DD"),
                    CastSpell("Obliterate", ret => MeSt.HasAura("Killing Machine"), "Obliterate KM"),
                    CastSpell("Frost Strike", ret => MeOb.CurrentRunicPower >= 120, "Frost Strike >= 120"),
                    CastSpell("Obliterate", ret => MeOb.UnholyRuneCount == 2, "Obliterate UU"),
                    CastSpell("Howling Blast", ret => MeOb.CurrentRunicPower < 90, "Howling Blast < 90"),
                    CastSpell("Frost Strike", ret => MeOb.CurrentRunicPower > 90, "Frost Strike > 90"),
                    CastSpell("Howling Blast", ret => MeOb.CurrentRunicPower < 60, "Howling Blast < 60"),
                    CastSpell("Howling Blast", "Howling Blast"),
                    CastSpell("Frost Strike", "Frost Strike"),
                    Buff("Horn of Winter", true, ret => MeSt, ret => true, "Horn of Winter", "Horn of Winter"));
        }

        public Composite FrostSoloBehavior()
        {
            return
                new PrioritySelector(
                    BuffMe(
                        "Anti-Magic Shell",
                        ret =>
                        MeOb.HealthPercent < 60
                        ||
                        (CurTarget.IsTargetingMeOrPet && CurTarget.IsCasting && CurTarget.PowerType == WoWPowerType.Mana),
                        "Anti-Magic Shell",
                        "Anti-Magic Shell"),
                    BuffMe(
                        "Icebound Fortitude", ret => MeOb.HealthPercent < 40, "Icebound Fortitude", "Icebound Fortitude"),
                    CastSpell(
                        "Death Strike",
                        ret => CurTarget,
                        ret => MeOb.HealthPercent < 40 || MeOb.HasMyAura("Dark Succor"),
                        "Death Strike"),
                    Buff(
                        "Outbreak",
                        true,
                        ret => CurTarget,
                        ret => MeOb.HealthPercent <= (CurTarget.HealthPercent / 3),
                        "Outbreak FFBP",
                        "Blood Plague",
                        "Frost Fever"),
                    Buff(
                        "Howling Blast",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0,
                        "Howling Blast FF",
                        "Frost Fever"),
                    Buff(
                        "Plague Strike",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0 && CurTarget.Elite,
                        "Plague Strike BP",
                        "Blood Plague"),
                    BuffMe(
                        "Pillar of Frost",
                        ret =>
                        MeOb.HealthPercent <= (CurTarget.HealthPercent / 3)
                        || CurTarget.HealthPercent > (MeOb.HealthPercent * 0.5) || CurTarget.Elite,
                        "Pillar of Frost",
                        "Pillar of Frost"),
                    Buff(
                        "Raise Dead",
                        true,
                        ret => MeSt,
                        ret => NearbyUnfriendlyUnits.Count(u => u.DistanceSqr <= (15 * 15)) > 2,
                        "Raise Dead",
                        "Pillar of Frost"),
                    Buff(
                        "Raise Dead",
                        true,
                        ret => MeSt,
                        ret => NearbyUnfriendlyUnits.Count(u => u.DistanceSqr <= (15 * 15)) > 2,
                        "Raise Dead",
                        "Unholy Strength"),
                    CastSpell(
                        "Empower Rune Weapon",
                        ret =>
                        MeOb.FrostRuneCount == 0 && MeOb.UnholyRuneCount < 2 && MeOb.DeathRuneCount == 0
                        && MeOb.HealthPercent <= (CurTarget.HealthPercent / 3),
                        "Empower Rune Weapon 0<20"),
                    new Decorator(
                        ret => CurTarget.Elite || CurTarget.IsBoss(),
                        new Action(
                            ret => 
                            {
                                UseTinkers();
                                UseTrinkets();
                            })),
                    CastSpell("Howling Blast", ret => NearbyUnfriendlyUnits.Count > 2, "Howling Blast Aoe"),
                    CastSpell("Frost Strike", ret => NearbyUnfriendlyUnits.Count > 2, "Frost Strike Aoe"),
                    CastSpell(
                        "Obliterate",
                        ret => MeOb.FrostRuneCount == 2 && MeOb.UnholyRuneCount == 2 && MeOb.DeathRuneCount == 2,
                        "Obliterate FFUUDD"),
                    // Looks like a double up of code (Same as above)?
                    // CastSpell(
                    //    "Obliterate",
                    //    ret => MeOb.FrostRuneCount == 2 && MeOb.UnholyRuneCount == 2 || MeOb.DeathRuneCount == 2,
                    //    "Obliterate FFUU | DD"),
                    CastSpell("Obliterate", ret => MeSt.HasAura("Killing Machine"), "Obliterate KM"),
                    CastSpell("Frost Strike", ret => MeOb.CurrentRunicPower >= 120, "Frost Strike >= 120"),
                    CastSpell("Obliterate", ret => MeOb.UnholyRuneCount == 2, "Obliterate UU"),
                    CastSpell("Howling Blast", ret => MeOb.CurrentRunicPower < 90, "Howling Blast < 90"),
                    CastSpell("Frost Strike", ret => MeOb.CurrentRunicPower > 90, "Frost Strike > 90"),
                    CastSpell("Howling Blast", ret => MeOb.CurrentRunicPower < 60, "Howling Blast < 60"),
                    CastSpell("Howling Blast", "Howling Blast"),
                    CastSpell("Frost Strike", "Frost Strike"),
                    Buff("Horn of Winter", true, ret => MeSt, ret => true, "Horn of Winter", "Horn of Winter"));
        }

        public Composite UnholyRaidBehaviour()
        {
            return
                new PrioritySelector(
                    Buff(
                        "Dark Simulacrum",
                        true,
                        ret => CurTarget,
                        ret => CurTarget.IsBoss() && CurTarget.IsCasting && CurTarget.CastingSpellId == 108567,
                        "Dark Simulacrum",
                        "Dark Simulacrum"),
                    BuffMe(
                        "Anti-Magic Shell",
                        ret => MeOb.HealthPercent < 80 || (CurTarget.IsBoss() && CurTarget.IsTargetingMeOrPet),
                        "Anti-Magic Shell",
                        "Anti-Magic Shell"),
                    BuffMe(
                        "Icebound Fortitude",
                        ret => MeOb.HealthPercent < 60 && CurTarget.IsBoss(),
                        "Icebound Fortitude",
                        "Icebound Fortitude"),
                    CastSpell("Dark Transformation", "Dark Transformation"),
                    Unholydiseases(),
                    CastSpell(
                        "Empower Rune Weapon",
                        ret =>
                        MeOb.FrostRuneCount == 0 && MeOb.UnholyRuneCount < 2
                        && (MeOb.DeathRuneCount == 0 || MeOb.BloodRuneCount == 0),
                        "Empower Rune Weapon 0<20|0"),
                    new Decorator(
                        ret => CurTarget.IsBoss(),
                        new Action(
                            ret => 
                            {
                                UseTinkers();
                                UseTrinkets();
                            })));
        }

        public Composite UnholySoloBehaviour()
        {
            return
                new PrioritySelector(
                    BuffMe(
                        "Anti-Magic Shell",
                        ret =>
                        MeOb.HealthPercent < 60
                        ||
                        (CurTarget.IsTargetingMeOrPet && CurTarget.IsCasting && CurTarget.PowerType == WoWPowerType.Mana),
                        "Anti-Magic Shell",
                        "Anti-Magic Shell"),
                    BuffMe(
                        "Icebound Fortitude", ret => MeOb.HealthPercent < 40, "Icebound Fortitude", "Icebound Fortitude"),
                    CastSpell(
                        "Death Strike",
                        ret => CurTarget,
                        ret => MeOb.HealthPercent < 40 || MeOb.HasMyAura("Dark Succor"),
                        "Death Strike"),
                    CastSpell("Dark Transformation", "Dark Transformation"),
                    Buff(
                        "Outbreak",
                        true,
                        ret => CurTarget,
                        ret => MeOb.HealthPercent <= (CurTarget.HealthPercent / 3),
                        "Outbreak FFBP",
                        "Blood Plague",
                        "Frost Fever"),
                    Buff(
                        "Icy Touch",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0,
                        "Icy Touch FF",
                        "Frost Fever"),
                    Buff(
                        "Plague Strike",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0 && CurTarget.Elite,
                        "Plague Strike BP",
                        "Blood Plague"),
                    CastSpell(
                        "Empower Rune Weapon",
                        ret =>
                        (MeOb.FrostRuneCount == 0 && MeOb.UnholyRuneCount < 2
                         && (MeOb.DeathRuneCount == 0 || MeOb.BloodRuneCount == 0))
                        || MeOb.HealthPercent <= (CurTarget.HealthPercent / 3),
                        "Empower Rune Weapon 0<20"),
                    new Decorator(
                        ret => CurTarget.Elite || CurTarget.IsBoss() || MeOb.HealthPercent < 45,
                        new Action(
                            ret => 
                            {
                                UseTinkers();
                                UseTrinkets();
                            })));
        }

        private static Composite Frostdiseases()
        {
            return
                new PrioritySelector(
                    Buff(
                        "Outbreak",
                        true,
                        ret => CurTarget,
                        ret => CurTarget.IsBoss(),
                        "Outbreak FFBP",
                        "Blood Plague",
                        "Frost Fever"),
                    Buff(
                        "Howling Blast",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0 && CurTarget.IsBoss(),
                        "Howling Blast FF",
                        "Frost Fever"),
                    Buff(
                        "Plague Strike",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0 && CurTarget.IsBoss(),
                        "Plague Strike BP",
                        "Blood Plague"),
                    CastSpell(
                        "Plague Strike",
                        ret => CurTarget,
                        ret => CurTarget.Auras["Blood Plague"].TimeLeft.Milliseconds < 500,
                        "Plague Strike"));
        }

        private static Composite Unholydiseases()
        {
            return
                new PrioritySelector(
                    Buff(
                        "Outbreak",
                        true,
                        ret => CurTarget,
                        ret => CurTarget.IsBoss(),
                        "Outbreak FFBP",
                        "Blood Plague",
                        "Frost Fever"),
                    Buff(
                        "Icy Touch",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0 && CurTarget.IsBoss(),
                        "Icy Touch FF",
                        "Frost Fever"),
                    Buff(
                        "Plague Strike",
                        true,
                        ret => CurTarget,
                        ret => SpellManager.Spells["Outbreak"].CooldownTimeLeft.Seconds > 0 && CurTarget.IsBoss(),
                        "Plague Strike BP",
                        "Blood Plague"));
        }

        #endregion

        #region Category: Spell

        #region Delegates

        public delegate bool SimpleBooleanDelegate(object context);

        public delegate WoWUnit UnitSelectionDelegate(object context);

        #endregion

        public static readonly Dictionary<string, DateTime> DoubleCastPreventionDict =
            new Dictionary<string, DateTime>();

        public static bool UseTrinkets()
        {
            if (MeOb.Inventory.Equipped.Trinket1.Usable && MeOb.Inventory.Equipped.Trinket1.Cooldown <= 0 &&
                MeOb.Inventory.Equipped.Trinket1 != null)
                return MeOb.Inventory.Equipped.Trinket1.Use();
            if (MeOb.Inventory.Equipped.Trinket2.Usable && MeOb.Inventory.Equipped.Trinket2.Cooldown <= 0 &&
                MeOb.Inventory.Equipped.Trinket2 != null)
                return MeOb.Inventory.Equipped.Trinket2.Use();
            return false;
        }

        public static bool UseTinkers()
        {
            if (MeOb.Inventory.Equipped.Hands.Usable && MeOb.Inventory.Equipped.Hands.Cooldown <= 0 &&
                MeOb.Inventory.Equipped.Hands != null)
                return MeOb.Inventory.Equipped.Hands.Use();
            return false;
        }

        public static Composite CastSpell(
            string spell, UnitSelectionDelegate onunit, SimpleBooleanDelegate requirements, string log)
        {
            return
                new Decorator(
                    ret =>
                    SpellManager.CanCast(spell, onunit(ret), false, false) &&
                    (!SpellManager.GlobalCooldown ||
                     SpellManager.GlobalCooldownLeft.Milliseconds < 300) &&
                    spell != null &&
                    // onunit != null && !Expression is alwaays true.
                    requirements != null &&
                    log != null &&
                    onunit(ret) != null &&
                    requirements(ret) &&
                    SpellManager.Spells[spell].CooldownTimeLeft.Milliseconds < 300,
                    new Action(
                        ret =>
                            {
                                SpellManager.Cast(spell, onunit(ret));
                                Slog("[Spell] Casting " + log);
                            }));
        }

        public static Composite Cast(
            string name, UnitSelectionDelegate onUnit, SimpleBooleanDelegate requirements, string log)
        {
            return Cast(name, onUnit, requirements, log);
        }

        public Composite CastSpell(string spell, SimpleBooleanDelegate requirements, string log)
        {
            return CastSpell(spell, ret => CurTarget, requirements, log);
        }

        public Composite CastSpell(string spell, string log)
        {
            return CastSpell(spell, ret => CurTarget, ret => true, log);
        }

        public static Composite Buff(
            string name,
            bool myBuff,
            UnitSelectionDelegate onUnit,
            SimpleBooleanDelegate requirements,
            string log,
            params string[] buffNames)
        {
            return
                new Decorator(
                    ret => onUnit(ret) != null && !DoubleCastPreventionDict.ContainsKey(name) &&
                           buffNames.All(b => myBuff ? !onUnit(ret).HasMyAura(b) : !onUnit(ret).HasAura(b)),
                    new Sequence(
                        // new Action(ctx => _lastBuffCast = name),
                        CastSpell(name, onUnit, requirements, log),
                        new DecoratorContinue(
                            ret => SpellManager.Spells[name].CastTime > 0,
                            new Sequence(
                                new WaitContinue(
                                    1,
                                    ret => StyxWoW.Me.IsCasting,
                                    new Action(ret => UpdateDoubleCastDict(name))))
                            ))
                    );
        }

        public static Composite BuffMe(string name, SimpleBooleanDelegate requirements, string log,
                                       params string[] buffnames)
        {
            return Buff(name, true, ret => StyxWoW.Me, requirements, log, buffnames);
        }

        private static void UpdateDoubleCastDict(string spellName)
        {
            if (DoubleCastPreventionDict.ContainsKey(spellName))
                DoubleCastPreventionDict[spellName] = DateTime.UtcNow;

            DoubleCastPreventionDict.Add(spellName, DateTime.UtcNow);
        }

        #endregion
    }

    #region Category: Aura

    internal static class Aura
    {
        public static bool HasAura(this WoWUnit unit, string aura)
        {
            return HasAura(unit, aura, 0);
        }

        /// <summary>
        ///  Checks the aura count by the name on specified unit.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <param name="stacks"> The stack count of the aura to return true. </param>
        /// <returns></returns>
        public static bool HasAura(this WoWUnit unit, string aura, int stacks)
        {
            return HasAura(unit, aura, stacks, null);
        }

        public static bool HasAllMyAuras(this WoWUnit unit, params string[] auras)
        {
            return auras.All(unit.HasMyAura);
        }

        /// <summary>
        ///  Check the aura count thats created by yourself by the name on specified unit
        /// </summary>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <returns></returns>
        public static bool HasMyAura(this WoWUnit unit, string aura)
        {
            return HasMyAura(unit, aura, 0);
        }

        /// <summary>
        ///  Check the aura count thats created by yourself by the name on specified unit
        /// </summary>
        /// <param name="aura"> The name of the aura in English. </param>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="stacks"> The stack count of the aura to return true. </param>
        /// <returns></returns>
        public static bool HasMyAura(this WoWUnit unit, string aura, int stacks)
        {
            return HasAura(unit, aura, stacks, StyxWoW.Me);
        }

        private static bool HasAura(this WoWUnit unit, string aura, int stacks, WoWUnit creator)
        {
            return unit.GetAllAuras().Any(a => a.Name == aura && a.StackCount >= stacks &&
                                               (creator == null || a.CreatorGuid == creator.Guid));
        }

        /// <summary>
        ///  Checks for the auras on a specified unit. Returns true if the unit has any aura in the auraNames list.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="auraNames"> Aura names to be checked. </param>
        /// <returns></returns>
        public static bool HasAnyAura(this WoWUnit unit, params string[] auraNames)
        {
            WoWAuraCollection auras = unit.GetAllAuras();
            var hashes = new HashSet<string>(auraNames);
            return auras.Any(a => hashes.Contains(a.Name));
        }

        /// <summary>
        ///  Checks for the auras on a specified unit. Returns true if the unit has any aura with any of the mechanics in the mechanics list.
        /// </summary>
        /// <param name="unit"> The unit to check auras for. </param>
        /// <param name="mechanics"> Mechanics to be checked. </param>
        /// <returns></returns>
        public static bool HasAuraWithMechanic(this WoWUnit unit, params WoWSpellMechanic[] mechanics)
        {
            WoWAuraCollection auras = unit.GetAllAuras();
            return auras.Any(a => mechanics.Contains(a.Spell.Mechanic));
        }

        /// <summary>
        ///  Returns the timeleft of an aura by TimeSpan. Return TimeSpan.Zero if the aura doesn't exist.
        /// </summary>
        /// <param name="auraName"> The name of the aura in English. </param>
        /// <param name="onUnit"> The unit to check the aura for. </param>
        /// <param name="fromMyAura"> Check for only self or all buffs</param>
        /// <returns></returns>
        public static TimeSpan GetAuraTimeLeft(this WoWUnit onUnit, string auraName, bool fromMyAura)
        {
            WoWAura wantedAura =
                onUnit.GetAllAuras().FirstOrDefault(a => a.Name == auraName && (!fromMyAura || a.CreatorGuid == StyxWoW.Me.Guid));

            return wantedAura != null ? wantedAura.TimeLeft : TimeSpan.Zero;
        }
    }

    #endregion

    #region Category: Unit/BossList

    #region Category: Unit

    internal static class Unit
    {
        public static bool IsBoss(this WoWUnit unit)
        {
            return BossList.BossIds.Contains(unit.Entry);
        }

        public static bool IsTrainingDummy(this WoWUnit unit)
        {
            return BossList.TrainingDummies.Contains(unit.Entry);
        }
    }

    #endregion

    #region Category: BossList

    public static class BossList
    {
        #region Boss Entries

        #region Ids

        private static readonly HashSet<uint> Dummies = new HashSet<uint>
                                                            {
                                                                31146,
                                                                // Raider's
                                                                46647,
                                                                // 81-85
                                                                32546,
                                                                // Ebon Knight's (DK)
                                                                31144,
                                                                // 79-80
                                                                32543,
                                                                // Veteran's (Eastern Plaguelands)
                                                                32667,
                                                                // 70
                                                                32542,
                                                                // 65 EPL
                                                                32666,
                                                                // 60
                                                                30527,
                                                                // ?? Boss one (no idea?)
                                                            };

        private static readonly HashSet<uint> Bosses = new HashSet<uint>
                                                           {
                                                               //Icecrown Citadel
                                                               36612,
                                                               //Lord Marrowgar
                                                               36855,
                                                               //Lady Deathwhisper

                                                               //Gunship Battle
                                                               37813,
                                                               //Deathbringer Saurfang
                                                               36626,
                                                               //Festergut
                                                               36627,
                                                               //Rotface
                                                               36678,
                                                               //Professor Putricide
                                                               37972,
                                                               //Prince Keleseth (Icecrown Citadel)
                                                               37970,
                                                               //Prince Valanar
                                                               37973,
                                                               //Prince Taldaram (Icecrown Citadel)
                                                               37955,
                                                               //Queen Lana'thel
                                                               36789,
                                                               //Valithria Dreamwalker
                                                               37950,
                                                               //Valithria Dreamwalker (Phased)
                                                               37868,
                                                               //Risen Archmage, Valitrhia Add
                                                               36791,
                                                               //Blazing Skeleton, Valithria Add
                                                               37934,
                                                               //Blistering Zombie, Valithria Add
                                                               37886,
                                                               //Gluttonous Abomination, Valithria Add
                                                               37985,
                                                               //Dream Cloud , Valithria "Add" 
                                                               36853,
                                                               //Sindragosa
                                                               36597,
                                                               //The Lich King (Icecrown Citadel)
                                                               37217,
                                                               //Precious
                                                               37025,
                                                               //Stinki
                                                               36661,
                                                               //Rimefang <Drake of Tyrannus>

                                                               //Blackrock Mountain: Blackrock Caverns
                                                               39665,
                                                               //Rom'ogg Bonecrusher
                                                               39679,
                                                               //Corla, Herald of Twilight
                                                               39698,
                                                               //Karsh Steelbender
                                                               39700,
                                                               //Beauty
                                                               39705,
                                                               //Ascendant Lord Obsidius

                                                               //Abyssal Maw: Throne of the Tides
                                                               40586,
                                                               //Lady Naz'jar
                                                               40765,
                                                               //Commander Ulthok
                                                               40825,
                                                               //Erunak Stonespeaker
                                                               40788,
                                                               //Mindbender Ghur'sha
                                                               42172,
                                                               //Ozumat

                                                               //The Stonecore
                                                               43438,
                                                               //Corborus
                                                               43214,
                                                               //Slabhide
                                                               42188,
                                                               //Ozruk
                                                               42333,
                                                               //High Priestess Azil

                                                               //The Vortex Pinnacle
                                                               43878,
                                                               //Grand Vizier Ertan
                                                               43873,
                                                               //Altairus
                                                               43875,
                                                               //Asaad

                                                               //Grim Batol
                                                               39625,
                                                               //General Umbriss
                                                               40177,
                                                               //Forgemaster Throngus
                                                               40319,
                                                               //Drahga Shadowburner
                                                               40484,
                                                               //Erudax

                                                               //Halls of Origination
                                                               39425,
                                                               //Temple Guardian Anhuur
                                                               39428,
                                                               //Earthrager Ptah
                                                               39788,
                                                               //Anraphet
                                                               39587,
                                                               //Isiset
                                                               39731,
                                                               //Ammunae
                                                               39732,
                                                               //Setesh
                                                               39378,
                                                               //Rajh

                                                               //Lost City of the Tolvir
                                                               44577,
                                                               //General Husam
                                                               43612,
                                                               //High Prophet Barim
                                                               43614,
                                                               //Lockmaw
                                                               49045,
                                                               //Augh
                                                               44819,
                                                               //Siamat

                                                               //Baradin Hold
                                                               47120,
                                                               //Argaloth
                                                               52363,
                                                               // Occu'thar

                                                               //Blackrock Mountain: Blackwing Descent
                                                               41570,
                                                               //Magmaw
                                                               42180,
                                                               //Toxitron
                                                               41378,
                                                               //Maloriak
                                                               41442,
                                                               //Atramedes
                                                               43296,
                                                               //Chimaeron
                                                               41376,
                                                               //Nefarian

                                                               //Throne of the Four Winds
                                                               45871,
                                                               //Nezir
                                                               46753,
                                                               //Al'Akir

                                                               //The Bastion of Twilight
                                                               45992,
                                                               //Valiona
                                                               45993,
                                                               //Theralion
                                                               44600,
                                                               //Halfus Wyrmbreaker
                                                               43735,
                                                               //Elementium Monstrosity
                                                               43324,
                                                               //Cho'gall
                                                               45213,
                                                               //Sinestra (heroic)

                                                               //World Dragons
                                                               14889,
                                                               //Emeriss
                                                               14888,
                                                               //Lethon
                                                               14890,
                                                               //Taerar
                                                               14887,
                                                               //Ysondre

                                                               //Azshara
                                                               14464,
                                                               //Avalanchion
                                                               6109,
                                                               //Azuregos

                                                               //Un'Goro Crater
                                                               14461,
                                                               //Baron Charr

                                                               //Silithus
                                                               15205,
                                                               //Baron Kazum <Abyssal High Council>
                                                               15204,
                                                               //High Marshal Whirlaxis <Abyssal High Council>
                                                               15305,
                                                               //Lord Skwol <Abyssal High Council>
                                                               15203,
                                                               //Prince Skaldrenox <Abyssal High Council>
                                                               14454,
                                                               //The Windreaver

                                                               //Searing Gorge
                                                               9026,
                                                               //Overmaster Pyron

                                                               //Winterspring
                                                               14457,
                                                               //Princess Tempestria

                                                               //Hellfire Peninsula
                                                               18728,
                                                               //Doom Lord Kazzak
                                                               12397,
                                                               //Lord Kazzak

                                                               //Shadowmoon Valley
                                                               17711,
                                                               //Doomwalker

                                                               //Nagrand
                                                               18398,
                                                               //Brokentoe
                                                               18069,
                                                               //Mogor <Hero of the Warmaul>, friendly
                                                               18399,
                                                               //Murkblood Twin
                                                               18400,
                                                               //Rokdar the Sundered Lord
                                                               18401,
                                                               //Skra'gath
                                                               18402,
                                                               //Warmaul Champion

                                                               // Cata Zul'gurub
                                                               52155,
                                                               //High Priest Venoxis
                                                               52151,
                                                               //Bloodlord Mandokir
                                                               52271,
                                                               //Hazza'ra
                                                               52059,
                                                               //High Priestess Kilnara
                                                               52053,
                                                               //Zanzil
                                                               52148,
                                                               //Jin'do the Godbreaker

                                                               //Firelands
                                                               53691,
                                                               //Shannox
                                                               52558,
                                                               //Lord Rhyolith
                                                               52498,
                                                               //Beth'tilac
                                                               52530,
                                                               //Alysrazor
                                                               53494,
                                                               //Baleroc
                                                               52571,
                                                               //Majordomo Staghelm
                                                               52409,
                                                               //Ragnaros

                                                               //Dragon Soul
                                                               55265,
                                                               // Morchok
                                                               57773,
                                                               // Kohcrom (Heroic Morchok encounter)
                                                               55308,
                                                               // Zon'ozz
                                                               55312,
                                                               // Yor'sahj
                                                               55689,
                                                               // Hagara
                                                               55294,
                                                               // Ultraxion
                                                               56427,
                                                               // Blackhorn

                                                               56846,
                                                               // Arm Tentacle -- Madness of DW
                                                               56167,
                                                               // Arm Tentacle -- Madness of DW
                                                               56168,
                                                               // Wing Tentacle - Madness of DW
                                                               57962,
                                                               // Deathwing ----- Madness of DW (his head)
                                                               54431,
                                                               //Echo of Baine
                                                               54445,
                                                               54123,
                                                               54544,
                                                               54432,
                                                               54590,
                                                               54968,
                                                               54938,
                                                               55085,
                                                               54853,
                                                               55419,
                                                               54969,
                                                           };

        #endregion

        static BossList()
        {
            foreach (uint bossId in Dummies)
            {
                Bosses.Add(bossId);
            }
        }

        public static HashSet<uint> BossIds
        {
            get { return Bosses; }
        }

        public static HashSet<uint> TrainingDummies
        {
            get { return Dummies; }
        }

        #endregion
    }

    #endregion

    #endregion
}