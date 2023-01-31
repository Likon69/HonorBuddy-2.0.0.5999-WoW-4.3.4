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
    class DeadlyThrow
    {
        public static bool CanRun
        {
            get
            {
                if (
                    Utils.PlayerFlee.IsFleeing && 
                    StyxWoW.Me.CurrentTarget.Distance < 30 && 
                    StyxWoW.Me.ComboPoints >= 1 &&
                    StyxWoW.Me.Inventory.Equipped.Ranged.IsThrownWeapon &&
                    !Spell.HasCanSpell("Shadowstep") &&
                    !Spell.HasCanSpell("Sprint") &&
                    StyxWoW.Me.CurrentTarget.InLineOfSpellSight
                    ) return true;

                return false;
            }
        }

        public static bool Run()
        {
            Combat._LastMove = "Deadly Throw";
            return Spell.Cast("Deadly Throw", StyxWoW.Me.CurrentTarget);
        }
    }
}
