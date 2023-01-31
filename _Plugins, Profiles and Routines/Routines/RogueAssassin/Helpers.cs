using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace RogueAssassin
{
    internal delegate void ResetEventHandler();

    internal static class Helpers
    {
        #region Fields

        private static int? _currentEnergy;
        private static IEnumerable<WoWUnit> _nearbyEnemies;

        #endregion

        #region Properties

        /// <summary>
        /// Checks if it's safe to AoE
        /// </summary>
        /// <returns>Returns true if none of our nearby enemies are cc'd.</returns>
        public static bool AOEIsSafe
        {
            get { return !NearbyEnemies.Any(IsCrowdControlled); }
        }

        /// <summary>
        /// Cached Energy
        /// </summary>
        public static int CurrentEnergy
        {
            get
            {
                if (!_currentEnergy.HasValue) _currentEnergy = Lua.GetReturnVal<int>("return UnitMana(\"player\");", 0);

// ReSharper disable PossibleInvalidOperationException
                return _currentEnergy.Value;
// ReSharper restore PossibleInvalidOperationException
            }
        }

        /// <summary>
        /// Cached Focus
        /// </summary>
        public static WoWPlayer Focus { get; private set; }

        /// <summary>
        /// Contains the logic on whether our current target is ready for Tricks of the Trade.
        /// </summary>
        /// <returns><c>true</c> if and only if all conditions are met for the focus target to receive ToT</returns>
        public static bool FocusReadyForTricks
        {
            get
            {
                return Focus != null && Focus.Distance2D <= 100 && Focus.InLineOfSight && Focus.IsAlive
                       && Focus.IsInMyPartyOrRaid;
            }
        }

        /// <summary>
        /// Cached Enemies in AoE Range.
        /// </summary>
        public static IEnumerable<WoWUnit> NearbyEnemies
        {
            get
            {
                return _nearbyEnemies ??
                       (_nearbyEnemies =
                        ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                            .Where(
                                unit =>
                                !unit.IsFriendly &&
                                (unit.IsTargetingMeOrPet || unit.IsTargetingMyPartyMember
                                 || unit.IsTargetingMyRaidMember ||
                                 unit.IsPlayer) && !unit.IsNonCombatPet && !unit.IsCritter &&
                                unit.Distance2D <= RogueAssassin.Settings.FOKRange).ToList());
            }
        }

        /// <summary>
        /// Check if player's target is a boss using Lua.
        /// </summary>
        public static bool TargetIsBoss
        {
            get
            {
                if (!StyxWoW.Me.IsInRaid)
                {
                    return (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss)
                           ||
                           ((StyxWoW.Me.CurrentTarget.Level == StyxWoW.Me.Level + 2 && StyxWoW.Me.CurrentTarget.Elite)
                            || (StyxWoW.Me.CurrentTarget.Level > StyxWoW.Me.Level + 2));
                }

                return (StyxWoW.Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss)
                       || (StyxWoW.Me.CurrentTarget.Level > StyxWoW.Me.Level + 2 && StyxWoW.Me.CurrentTarget.Elite);
            }
        }

        #endregion

        #region Events

        public static event ResetEventHandler ResetAll;

        #endregion

        #region Methods

        /// <summary>
        /// Checks to see if specified target is under the effect of crowd control
        /// </summary>
        /// <param name="target">Target</param>
        /// <returns></returns>
        private static bool IsCrowdControlled(WoWUnit target)
        {
            // Just want to throw a shout-out to Singular for this function.
            return
                target.GetAllAuras().Any(
                    unit =>
                    unit.Spell.Mechanic == WoWSpellMechanic.Banished || unit.Spell.Mechanic == WoWSpellMechanic.Charmed
                    || unit.Spell.Mechanic == WoWSpellMechanic.Horrified
                    || unit.Spell.Mechanic == WoWSpellMechanic.Incapacitated
                    || unit.Spell.Mechanic == WoWSpellMechanic.Polymorphed
                    || unit.Spell.Mechanic == WoWSpellMechanic.Sapped
                    || unit.Spell.Mechanic == WoWSpellMechanic.Shackled
                    || unit.Spell.Mechanic == WoWSpellMechanic.Asleep
                    || unit.Spell.Mechanic == WoWSpellMechanic.Frozen);
        }

        private static void Reset()
        {
            if (ResetAll != null) ResetAll();

            _currentEnergy = null;
            _nearbyEnemies = null;

            SetFocus();
        }

        /// <summary>
        /// Resets all locally cached variables.
        /// </summary>
        public static RunStatus ResetFail()
        {
            Reset();

            return RunStatus.Failure;
        }

        /// <summary>
        /// Resets all locally cached variables.
        /// </summary>
        public static RunStatus ResetSucceed()
        {
            Reset();

            return RunStatus.Success;
        }

        private static void SetFocus()
        {
            var focusGuid =
                Lua.GetReturnVal<string>(
                    "local GUID = UnitGUID(\"focus\"); if GUID == nil then return 0 else return GUID end", 0);

            if (focusGuid == Convert.ToString(0))
            {
                if (Focus != null)
                {
                    Focus = null;

                    Logging.Write(Color.Orange, "Focus dropped -- clearing focus target.");
                }

                return;
            }

            // Remove the two starting characters (0x) from the GUID returned by lua using substring.
            // This is done so we can convert the string to a ulong.
            // Then we should set the WoWPlayer Focus to the unit which belongs to our formatted GUID.
            var focus =
                ObjectManager.GetAnyObjectByGuid<WoWPlayer>(ulong.Parse(focusGuid.Substring(2),
                                                                        NumberStyles.
                                                                            AllowHexSpecifier));

            if (Focus != focus)
                Logging.Write(Color.Orange, "Setting " + focus.Name + " as focus target.");

            Focus = focus;
        }

        #endregion
    }
}