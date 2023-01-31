using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Logic.Combat;

namespace PvPRogue.Helpers
{

    /*
     Logic and lists behind this is from - eXemplar's Trinket Racial Plugin
     Re-Designed - eXemplar's gave Permission
     */

    public static class Trinket
    {

        private static List<WoWSpellMechanic> lTrinket = new List<WoWSpellMechanic>(){
            WoWSpellMechanic.Asleep,
            WoWSpellMechanic.Charmed,
            WoWSpellMechanic.Disoriented,
            WoWSpellMechanic.Fleeing,
            WoWSpellMechanic.Frozen,
            WoWSpellMechanic.Horrified,
            WoWSpellMechanic.Incapacitated,
            WoWSpellMechanic.Polymorphed,
            WoWSpellMechanic.Stunned
        };


        private static List<string> lIgnore = new List<string>()
        {
            "Hamstring",
            "Crippling Poison",
            "Infected Wounds",
            "Piercing Howl",
            "Frostbolt"
        };

        private static List<string> lBlacklist = new List<string>()
        {
            "Frost Nova"
        };


        public static void Pulse()
        {
            // Loop threw each of our
            foreach (KeyValuePair<String, WoWAura> entry in StyxWoW.Me.ActiveAuras)
            {
                // Create our spell that is on us so we can use it
                WoWSpell Spell = WoWSpell.FromId(entry.Value.SpellId);

                // Anything Blacklisted contine;
                if (lIgnore.Contains(Spell.Name)) continue;

                // if theres something we need to trinket, lets do it
                if (lTrinket.Contains(Spell.Mechanic) || lBlacklist.Contains(Spell.Name))
                {
                    if (entry.Value.TimeLeft.Seconds >= 3)
                    {
                        Log.WriteDebug("Trinketing - Name: {0}  Timeleft: {1}", Spell.Name, entry.Value.TimeLeft);
                        UseTrinket();
                    }
                }

            } 
        }

        private static void UseTrinket()
        {
            // Check First Trinket
            if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && (StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains("Medallion of ") || StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains("Inherited Insignia")))
            {
                // Check if its off cooldown
                if (StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {
                    StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                    Log.Write("->Trinket: Used");
                    StyxWoW.SleepForLagDuration();
                    return;
                }
            }

            // Check Second Trinket
            if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && (StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains("Medallion of ") || StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains("Inherited Insignia")))
            {
                // Check if its off cooldown
                if (StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                    Log.Write("->Trinket: Used");
                    StyxWoW.SleepForLagDuration();
                    return;
                }
            }
        }

    }
}
