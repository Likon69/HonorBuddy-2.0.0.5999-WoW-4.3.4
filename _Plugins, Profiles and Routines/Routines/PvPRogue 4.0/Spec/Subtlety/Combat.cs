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
using CommonBehaviors.Actions;
using Styx.Logic.BehaviorTree;
using TreeSharp;

namespace PvPRogue.Spec.Subtlety
{

    public static class Combat
    {
        public static string _LastMove;

        public static void pulse()
        {
            if (!Utils.SafeChecks.CombatReady) return;

            // Auto Attack
            if (AutoAttack.CanRun) { if (AutoAttack.Run()) return; }

            // If target is running, we need to chase!
            if (Spells.Vanish.CanRun) { Spells.Vanish.Run(); }                          // Used for -> Vanish -> Shadow Step -> Ambush
            if (Spells.Shadowstep.CanRun) { if (Spells.Shadowstep.Run()) return; }
            if (Spells.DeadlyThrow.CanRun) { if (Spells.DeadlyThrow.Run()) return; }
            if (Spells.Sprint.CanRun) { if (Spells.Sprint.Run()) return; }
            if (Spells.Throw.CanRun) { if (Spells.Throw.Run()) return; } 

            // Our main moves!
            if (Spells.ShadowDance.CanRun) { if (Spells.ShadowDance.EnergySave()) return; }
            if (Spells.ShadowDance.CanRun) { if (Spells.ShadowDance.Run()) return; }
            if (Spells.Ambush.CanRun) { if (Spells.Ambush.Run()) return; }

            // Defensives
            if (Spells.Evasion.CanRun) { if (Spells.Evasion.Run()) return; }
            if (Spells.Kick.CanRun) { if (Spells.Kick.Run()) return; }
            if (Spells.CombatReadiness.CanRun) { if (Spells.CombatReadiness.Run()) return; }
            if (Spells.SmokeBombDefensive.CanRun) { if (Spells.SmokeBombDefensive.Run()) return; }
            if (Spells.CloakOfShadows.CanRun) { if (Spells.CloakOfShadows.Run()) return; }
            if (Spells.Blind.IfCanRun()) return;

            // Special Moves
            if (Spells.Shiv.CanRun) { if (Spells.Shiv.Run()) return; }
            if (Spells.Dismantle.CanRun) { if (Spells.Dismantle.Run()) return; }
            if (Spells.Redirect.CanRun) { if (Spells.Redirect.Run()) return; }
            if (Spells.SmokeBombOffensive.CanRun) { if (Spells.SmokeBombOffensive.Run()) return; }
            if (Spells.Preparation.CanRun) { if (Spells.Preparation.Run()) return; }

            // Finishing moves.
            if (Spells.Recuperate.CanRun) { if (Spells.Recuperate.Run()) return; }
            if (Spells.SliceandDice.CanRun) { if (Spells.SliceandDice.Run()) return; }
            if (Spells.KidneyShot.CanRun) { if (Spells.KidneyShot.Run()) return; }
            if (Spells.Eviscerate.CanRun) { if (Spells.Eviscerate.Run()) return; }
            
            // Other stuff
            if (Spec.General.FanofKnives.CanRun) { if (Spec.General.FanofKnives.Run()) return; }

            // if we are not close enough do nothing
            if (!StyxWoW.Me.CurrentTarget.IsWithinMeleeRange) return;

            Spell.Cast(Spells.MainMove.GetName, StyxWoW.Me.CurrentTarget);
        }

    }
}
