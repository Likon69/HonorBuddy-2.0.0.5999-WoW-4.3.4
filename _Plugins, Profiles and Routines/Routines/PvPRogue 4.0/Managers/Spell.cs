using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Combat;

namespace PvPRogue
{
    public static class Spell
    {

        public static bool HasMyAura(string SpellName, WoWUnit Player)
        {
            try
            {
                if (!Player.HasAura(SpellName)) return false;
                if (!Player.ActiveAuras[SpellName].IsActive) return false;
                if (Player.ActiveAuras[SpellName].CreatorGuid != StyxWoW.Me.Guid) return false;

                return true;
            } catch (Exception) { return false; }
        }


        public static bool HasMyAura(string SpellName)
        {
            try
            {
                if (!ObjectManager.Me.HasAura(SpellName)) return false;
                if (!ObjectManager.Me.ActiveAuras[SpellName].IsActive) return false;
                if (ObjectManager.Me.ActiveAuras[SpellName].CreatorGuid != StyxWoW.Me.Guid) return false;

                return true;
            } catch (Exception) { return false; }
        }

        /// <summary>
        /// Returns time left in MS.
        /// </summary>
        /// <param name="SpellName"></param>
        /// <param name="Player"></param>
        /// <returns></returns>
        public static int HasMyAuraTimeLeft(string SpellName, WoWUnit Player)
        {
            try
            {
                if (!Player.HasAura(SpellName)) return 0;
                if (!Player.ActiveAuras[SpellName].IsActive) return 0;
                if (Player.ActiveAuras[SpellName].CreatorGuid != StyxWoW.Me.Guid) return 0;


                int Total = Player.ActiveAuras[SpellName].TimeLeft.Seconds * 1000;
                return Player.ActiveAuras[SpellName].TimeLeft.Milliseconds + Total;
            } catch (Exception) { return 0; }
            
        }

        public static int HasMyAuraTimeLeft(string SpellName)
        {
            return HasMyAuraTimeLeft(SpellName, StyxWoW.Me);
        }

        public static bool HasAura(string SpellName)
        {
            if (!ObjectManager.Me.HasAura(SpellName)) return false;

            return true;
        }

        /// <summary>
        /// Returns if can cast, and has spell
        /// </summary>
        /// <param name="SpellName"></param>
        /// <returns></returns>
        public static bool HasCanSpell(string SpellName)
        {
            if (SpellManager.HasSpell(SpellName) && SpellManager.CanCast(SpellName)) return true;

            return false;
        }

        /// <summary>
        /// Casts Spell on self
        /// </summary>
        /// <param name="SpellName"></param>
        /// <returns></returns>
        public static bool Cast(string SpellName)
        {
            if (!SpellManager.HasSpell(SpellName)) return false;
            if (StyxWoW.Me.CurrentEnergy < SpellManager.Spells[SpellName].PowerCost) return true;
            if (!SpellManager.CanCast(SpellName)) return false;

            Log.Write("Casting {0}", SpellName);

            return SpellManager.Cast(SpellName, StyxWoW.Me);
        }

        /// <summary>
        /// Casts spell
        /// </summary>
        /// <param name="SpellName"></param>
        /// <param name="Player"></param>
        /// <returns></returns>
        public static bool Cast(string SpellName, WoWUnit CastOn)
        {
            if (!SpellManager.HasSpell(SpellName)) return false;
            if (StyxWoW.Me.CurrentEnergy < SpellManager.Spells[SpellName].PowerCost) return true;
            if (!SpellManager.CanCast(SpellName)) return false;

            Log.Write("Casting {0} on [{1}]", SpellName, CastOn.Name);

            return SpellManager.Cast(SpellName, CastOn);
        }






        public static bool SpellOnCooldown(string Spell)
        {
            if (HasCanSpell(Spell))
            {
                return SpellManager.Spells[Spell].Cooldown;
            }

            return true;
        }


    }
}
