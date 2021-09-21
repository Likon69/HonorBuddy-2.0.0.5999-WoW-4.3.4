#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author$
// $Date$
// $HeadURL$
// $LastChangedBy$
// $LastChangedDate$
// $LastChangedRevision$
// $Revision$

#endregion

using System;
using Singular.Managers;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;

namespace Singular.Helpers
{
    /// <summary>Bitfield of flags for specifying DispelCapabilities.</summary>
    /// <remarks>Created 5/3/2011.</remarks>
    [Flags]
    public enum DispelCapabilities
    {
        None = 0,
        Curse = 1,
        Disease = 2,
        Poison = 4,
        Magic = 8,
        All = Curse | Disease | Poison | Magic
    }

    internal static class Dispelling
    {
        /// <summary>Gets the dispel capabilities of the current player.</summary>
        /// <value>The capabilities.</value>
        public static DispelCapabilities Capabilities
        {
            get
            {
                DispelCapabilities ret = DispelCapabilities.None;
                if (CanDispelCurse)
                {
                    ret |= DispelCapabilities.Curse;
                }
                if (CanDispelMagic)
                {
                    ret |= DispelCapabilities.Magic;
                }
                if (CanDispelPoison)
                {
                    ret |= DispelCapabilities.Poison;
                }
                if (CanDispelDisease)
                {
                    ret |= DispelCapabilities.Disease;
                }

                return ret;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel diseases.</summary>
        /// <value>true if we can dispel diseases, false if not.</value>
        public static bool CanDispelDisease
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Paladin:
                        return true;
                    case WoWClass.Priest:
                        return true;
					//case WoWClass.Monk
						//return true;
                }
                return false;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel poison.</summary>
        /// <value>true if we can dispel poison, false if not.</value>
        public static bool CanDispelPoison
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Druid:
                        return true;
                    case WoWClass.Paladin:
                        return true;
                        //case WoWClass.Monk:
                        //return true;
                }
                return false;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel curses.</summary>
        /// <value>true if we can dispel curses, false if not.</value>
        public static bool CanDispelCurse
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Druid:
                        return true;

                    case WoWClass.Shaman:
                        return true;

                    case WoWClass.Mage:
                        return true;
                }
                return false;
            }
        }

        /// <summary>Gets a value indicating whether we can dispel magic.</summary>
        /// <value>true if we can dispel magic, false if not.</value>
        public static bool CanDispelMagic
        {
            get
            {
                switch (StyxWoW.Me.Class)
                {
                        // 1, 14 - Paladin - Sacred Cleansing
                        // 3, 17 - Druid - Nature's Cure
                        // 3, 12 - Shaman - Improved Cleanse Spirit
                    case WoWClass.Druid:
                        return TalentManager.GetCount(3, 17) != 0;
                    case WoWClass.Paladin:
                        return TalentManager.GetCount(1, 14) != 0;
                    case WoWClass.Shaman:
                        return TalentManager.GetCount(3, 12) != 0;

                        // Priests can dispel magic natively.
                    case WoWClass.Priest:
                        return true;

                        //case WoWClass.Monk: // Monks need the passive talent "internal medicine" ~lvl 20
                        //return StyxWoW.Me.HasAura("Internal Medicine");
                }
                return false;
            }
        }

        /// <summary>Gets a dispellable types on unit. </summary>
        /// <remarks>Created 5/3/2011.</remarks>
        /// <param name="unit">The unit.</param>
        /// <returns>The dispellable types on unit.</returns>
        public static DispelCapabilities GetDispellableTypesOnUnit(WoWUnit unit)
        {
            DispelCapabilities ret = DispelCapabilities.None;
            foreach(var debuff in unit.Debuffs.Values)
            {
                switch (debuff.Spell.DispelType)
                {
                    case WoWDispelType.Magic:
                        ret |= DispelCapabilities.Magic;
                        break;
                    case WoWDispelType.Curse:
                        ret |= DispelCapabilities.Curse;
                        break;
                    case WoWDispelType.Disease:
                        ret |= DispelCapabilities.Disease;
                        break;
                    case WoWDispelType.Poison:
                        ret |= DispelCapabilities.Poison;
                        break;
                }
            }
            return ret;
        }

        /// <summary>Queries if we can dispel unit 'unit'. </summary>
        /// <remarks>Created 5/3/2011.</remarks>
        /// <param name="unit">The unit.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool CanDispelUnit(WoWUnit unit)
        {
            return (Capabilities & GetDispellableTypesOnUnit(unit)) != 0;
        }
    }
}