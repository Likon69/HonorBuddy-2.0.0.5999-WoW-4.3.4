using System.Collections.Generic;
using Styx;

namespace Singular.Helpers
{
    public enum PartyBuff
    {
        None,
        KingsOrMark,
        Fortitude,
        Resistance,
        // Battle Shout, Horn of Winter
        StrengthAgility,
    }

    class Party
    {
        public IEnumerable<PartyBuff> GetMissingPartyBuffs()
        {
            HashSet<PartyBuff> buffs = new HashSet<PartyBuff>();
            foreach (var a in StyxWoW.Me.GetAllAuras())
            {
                foreach (var se in a.Spell.SpellEffects)
                {
                    if (se == null)
                        continue;

                   
                }
            }
            return buffs;
        }
    }
}
