using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Spec.Subtlety.Spells
{
    class Throw
    {
        public static bool CanRun
        {
            get
            {
                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if (Utils.PlayerFlee.IsFleeing &&
                    Target.Distance < 30 &&
                    StyxWoW.Me.Inventory.Equipped.Ranged.IsThrownWeapon &&
                    !Spell.HasCanSpell("Shadowstep") &&
                    !Spell.HasCanSpell("Sprint") &&
                    StyxWoW.Me.CurrentTarget.InLineOfSpellSight &&
                    !Spell.HasMyAura("Crippling Poison", Target)
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Throw";
            return Spell.Cast("Throw", StyxWoW.Me.CurrentTarget);
        }
    }
}
