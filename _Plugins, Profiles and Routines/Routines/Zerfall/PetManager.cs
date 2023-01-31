using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Zerfall.Talents;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using Color = System.Drawing.Color;
using Sequence = TreeSharp.Sequence;

namespace Zerfall
{
    class PetManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static bool CanCastPetAction(string action)
        {
            WoWPetSpell petAction = Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (petAction == null || petAction.Spell == null)
            {
                return false;
            }
            Logging.Write("Action Not on Cooldown");
            return !petAction.Spell.Cooldown;
        }
        public static void CastPetAction(string action, WoWUnit on)
        {
            var spell = Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logging.Write(string.Format("[Pet] Casting {0} on {1}", action, on.Name));
            StyxWoW.Me.SetFocus(on);
            Lua.DoString("CastPetAction({0}, 'focus')", spell.ActionBarIndex + 1);
            StyxWoW.Me.SetFocus(0);
        }


    }
}
