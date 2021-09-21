#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: highvoltz $
// $Date: 2012-04-14 02:41:55 +0300 (Cmt, 14 Nis 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Managers/PetManager.cs $
// $LastChangedBy: highvoltz $
// $LastChangedDate: 2012-04-14 02:41:55 +0300 (Cmt, 14 Nis 2012) $
// $LastChangedRevision: 612 $
// $Revision: 612 $

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;

namespace Singular.Managers
{
    public enum PetType
    {
        // These are CreatureFamily IDs. See 'CurrentPet' for usage.
        None = 0,
        Imp = 23,
        Felguard = 29,
        Voidwalker = 16,
        Felhunter = 15,
        Succubus = 17,
    }

    internal class PetManager
    {
        private static readonly WaitTimer CallPetTimer = WaitTimer.OneSecond;

        private static ulong _petGuid;
        private static readonly List<WoWPetSpell> PetSpells = new List<WoWPetSpell>();

        static PetManager()
        {
            // NOTE: This is a bit hackish. This fires VERY OFTEN in major cities. But should prevent us from summoning right after dismounting.
            // Lua.Events.AttachEvent("COMPANION_UPDATE", (s, e) => CallPetTimer.Reset());
            // Note: To be changed to OnDismount with new release
            Mount.OnDismount += (s, e) =>
                    {
                        if (StyxWoW.Me.Class == WoWClass.Hunter || StyxWoW.Me.Class == WoWClass.Warlock || StyxWoW.Me.PetNumber > 0)
                        {
                            PetTimer.Reset();
                        }
                    };
        }

        public static PetType CurrentPetType
        {
            get
            {
                WoWUnit myPet = StyxWoW.Me.Pet;
                if (myPet == null)
                {
                    return PetType.None;
                }
                WoWCache.CreatureCacheEntry c;
                myPet.GetCachedInfo(out c);
                return (PetType)c.FamilyID;
            }
        }

        public static bool HavePet { get { return StyxWoW.Me.GotAlivePet; } }

        public static string WantedPet { get; set; }

        public static readonly WaitTimer PetTimer = new WaitTimer(TimeSpan.FromSeconds(2));

        private static bool _wasMounted;
        internal static void Pulse()
        {
            if (!StyxWoW.Me.GotAlivePet)
            {
                PetSpells.Clear();
                return;
            }

            if (StyxWoW.Me.Mounted)
            {
                _wasMounted = true;
            }

            if (_wasMounted && !StyxWoW.Me.Mounted)
            {
                _wasMounted = false;
                PetTimer.Reset();
            }

            if (StyxWoW.Me.Pet != null && _petGuid != StyxWoW.Me.Pet.Guid)
            {
                _petGuid = StyxWoW.Me.Pet.Guid;
                PetSpells.Clear();
                // Cache the list. yea yea, we should just copy it, but I'd rather have shallow copies of each object, rather than a copy of the list.
                PetSpells.AddRange(StyxWoW.Me.PetSpells);
                PetTimer.Reset();
            }
        }

        public static bool CanCastPetAction(string action)
        {
            WoWPetSpell petAction = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (petAction == null || petAction.Spell == null)
            {
                return false;
            }

            return !petAction.Spell.Cooldown;
        }

        public static void CastPetAction(string action)
        {
            var spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logger.Write(string.Format("[Pet] Casting {0}", action));
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        public static void CastPetAction(string action, WoWUnit on)
        {
            var spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logger.Write(string.Format("[Pet] Casting {0} on {1}", action, on.SafeName()));
            StyxWoW.Me.SetFocus(on);
            Lua.DoString("CastPetAction({0}, 'focus')", spell.ActionBarIndex + 1);
            StyxWoW.Me.SetFocus(0);
        }

        //public static void EnableActionAutocast(string action)
        //{
        //    var spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
        //    if (spell == null)
        //        return;

        //    var index = spell.ActionBarIndex + 1;
        //    Logger.Write("[Pet] Enabling autocast for {0}", action, index);
        //    Lua.DoString("local index = " + index + " if not select(6, GetPetActionInfo(index)) then TogglePetAutocast(index) end");
        //}

        /// <summary>
        ///   Calls a pet by name, if applicable.
        /// </summary>
        /// <remarks>
        ///   Created 2/7/2011.
        /// </remarks>
        /// <param name = "petName">Name of the pet. This parameter is ignored for mages. Warlocks should pass only the name of the pet. Hunters should pass which pet (1, 2, etc)</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool CallPet(string petName)
        {
            if (!CallPetTimer.IsFinished)
            {
                return false;
            }

            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Warlock:
                    if (SpellManager.CanCast("Summon " + petName))
                    {
                        Logger.Write(string.Format("[Pet] Calling out my {0}", petName));
                        bool result = SpellManager.Cast("Summon " + petName);
                        //if (result)
                        //    StyxWoW.SleepForLagDuration();
                        return result;
                    }
                    break;

                case WoWClass.Mage:
                    if (SpellManager.CanCast("Summon Water Elemental"))
                    {
                        Logger.Write("[Pet] Calling out Water Elemental");
                        bool result = SpellManager.Cast("Summon Water Elemental");
                        //if (result)   - All calls to this method are now placed in a sequence that uses WaitContinue 
                        //    StyxWoW.SleepForLagDuration();
                        return result;
                    }
                    break;

                case WoWClass.Hunter:
                    if (SpellManager.CanCast("Call Pet " + petName))
                    {
                        if (!StyxWoW.Me.GotAlivePet)
                        {
                            Logger.Write(string.Format("[Pet] Calling out pet #{0}", petName));
                            bool result = SpellManager.Cast("Call Pet " + petName);
                            //if (result)
                            //    StyxWoW.SleepForLagDuration();
                            return result;
                        }
                    }
                    break;
            }
            return false;
        }
    }
}