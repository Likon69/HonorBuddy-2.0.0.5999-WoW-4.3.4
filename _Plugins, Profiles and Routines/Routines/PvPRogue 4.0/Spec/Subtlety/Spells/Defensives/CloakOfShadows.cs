using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Logic.Combat;
namespace PvPRogue.Spec.Subtlety.Spells
{
    /// <summary>
    /// This Class is for the devensive COS + Vanish
    /// </summary>
    public static class CloakOfShadows
    {

            private static List<WoWSpellMechanic> lToCOS = new List<WoWSpellMechanic>(){
                WoWSpellMechanic.Disoriented,
                WoWSpellMechanic.Snared,
                WoWSpellMechanic.Slowed,
            };

            private static List<string> lIgnore = new List<string>()
            {
                "Hamstring",
                "Crippling Poison",
                "Infected Wounds",
                "Piercing Howl",
                "Frostbolt"
            };

            public static bool CanRun
            {
                get
                {
                    if (Spell.SpellOnCooldown("Cloak of Shadows")) return false;
                    if (StyxWoW.Me.CurrentTarget.IsWithinMeleeRange) return false;

                    foreach (KeyValuePair<String, WoWAura> entry in StyxWoW.Me.ActiveAuras)
                    {
                        if (lIgnore.Contains(entry.Value.Name)) continue;

                        WoWSpell Spellz = WoWSpell.FromId(entry.Value.SpellId);

                        // if theres something we need to trinket, lets do it
                        if (lToCOS.Contains(Spellz.Mechanic))
                        {
                            Log.Write("Cloaking [{0}]  Mechanic: {1}  TimeLeft: {2}", Spellz.Name, Spellz.Mechanic, entry.Value.TimeLeft);
                            return true;
                        }
                    } 

                    return false;
                }
            }

            public static bool Run()
            {
                Combat._LastMove = "Cloak of Shadows";
                return Spell.Cast("Cloak of Shadows");
            }

    }
}
