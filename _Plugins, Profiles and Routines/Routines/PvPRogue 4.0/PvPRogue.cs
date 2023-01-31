using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using PvPRogue.Managers;
using PvPRogue.Spec;
using PvPRogue.Helpers;

using Styx;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.Combat.CombatRoutine;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Inventory;
using TreeSharp;

namespace PvPRogue
{
    class PvPRogue : CombatRoutine
    {
        public override string Name { get { return "SwInY - PvPRogue " + version; } }
        public override WoWClass Class { get { return WoWClass.Rogue; } }
        public override bool WantButton { get { return true; } }
        public override void OnButtonPress() { FormSettings._Instance.Show(); }
        public Version version = new Version(4, 0);
        public LocalPlayer Me = ObjectManager.Me;

        public static Stopwatch PullTimer = new Stopwatch();
        public static int PullLengthMS = 4000;


        public override void Initialize()
        {
            ClassSettings._Instance = new ClassSettings();
            FormSettings._Instance = new FormSettings();
            
            Log.Write("Loading");
            Log.Write("Version: {0}", version);
            Managers.Spec.Update();

            // 30 is to fast lmfao, try 23
            TreeRoot.TicksPerSecond = 25;

            // incase they use CombatBot..
            PullTimer.Start();
            
            if (ClassSettings._Instance.GeneralAlwaysStealthed)
            {
                Log.Write("Always Stealthed Enabled");
                Log.WriteDebug("Disabling Use-Mount");
                Styx.Helpers.CharacterSettings.Instance.UseMount = false;
            }

            WoWChat.NeutralBattleground += Helpers.FlagReturn.ChatFilter;
            WoWChat.AllianceBattleground += Helpers.FlagReturn.ChatFilter;
            WoWChat.HordeBattleground += Helpers.FlagReturn.ChatFilter;

            Utils.Stats.Init();
        }
        public override void ShutDown()
        {
            // Defaults
            TreeRoot.TicksPerSecond = 15;
            Styx.Helpers.CharacterSettings.Instance.UseMount = true;
        }
        public override void Pull()
        {
            // Quick Target check to see if ok to pull
            if (Utils.SafeChecks.TargetSafe == false) return;
    
            WoWUnit Target = Me.CurrentTarget;

            // Move closer,  Sap distance is 10ft
            if (Target.Distance > 10 && !Spell.HasMyAura("Sap", StyxWoW.Me.CurrentTarget))
            {
                Managers.NavMan.MoveBehind(Target);                    
                return;                                             
            }

            // We need to face
            if ((!Me.IsSafelyFacing(Target, 90)) && (!Me.IsMoving))                       
            {
                Target.Face();
                return;
            }

            // Wait for full energy b4 we burst
            if (StyxWoW.Me.EnergyPercent != 100) return;

            // Next we will need to get a sap off to stop the person from moving
            // We have to check if his in combat some how,
            if (ClassSettings._Instance.GeneralToSap && !Battlegrounds.IsInsideBattleground)
            {
                if (Target.Combat == false)
                {
                    if ((SpellManager.CanCast("Sap")) && (!Spell.HasMyAura("Sap", Target)) && (Target.IsHumanoid))
                    {
                        if (Spell.Cast("Sap", Target))
                        {
                            StyxWoW.SleepForLagDuration();   // Some reason energy isnt working properly? great..... -> bandade..
                            return;
                        }
                    }
                }
            }

            // We need to check if we are behind and have correct distance.
            if ((!Target.IsPlayerBehind) || !Target.IsWithinMeleeRange)
            {
                Managers.NavMan.MoveBehind(Target);
                return;
            }

            // Our Pull Timer
            PullTimer.Reset();
            PullTimer.Start();

            // Do what we need to!
            switch (Managers.Spec.CurrentSpec)                         
            {
                case eSpec.Subtlety:
                    Spec.Subtlety.Pull.Pulse();
                    break;
            }
        }
        public override void Combat()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            using (new FrameLock())
            {
                WoWUnit Target = StyxWoW.Me.CurrentTarget;

                if (Target == null) return;

                // Handle behind
                if ((!Target.IsWithinMeleeRange) || (Target.IsPlayer && !Target.MeIsBehind && Target.IsWithinMeleeRange))
                {
                    Managers.NavMan.MoveBehind(Target);
                }

                if (!Me.IsMoving && !Me.IsSafelyFacing(Target))
                {
                    Target.Face();
                }

                // bug with being on a mount
                if (Me.Mounted) Mount.Dismount();

                // Get our Healers
                Helpers.BGHealers._Instance = new BGHealers();

                // Trinket Manager
                if (Battlegrounds.IsInsideBattleground)
                    Helpers.Trinket.Pulse();

                // Handle Combat
                switch (Managers.Spec.CurrentSpec)
                {
                    case eSpec.Subtlety:
                        Spec.Subtlety.Combat.pulse();
                        break;
                }
            }

            stopWatch.Stop();
            Managers.SpeedDebug.Pulse((int)stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
        }

        public override void Pulse()
        {
            Helpers.StealthSap.Pulse();
        }

        #region Poison/alwaysstealthed
        public override bool NeedPreCombatBuffs
        {
            get
            {
                if (StyxWoW.Me.IsCasting) return false;
                if (StyxWoW.Me.Combat) return false;
                if (StyxWoW.Me.IsFlying) return false;
                if ((!Managers.Poisons.HasMainHandPoison) && (Poisons.GetPoison(ClassSettings._Instance.SubtletyMainPoison) != null)) return true;
                if ((!Managers.Poisons.HasOffHandPoison) && (Poisons.GetPoison(ClassSettings._Instance.SubtletyOffHandPoison) != null)) return true;
                if ((!Managers.Poisons.HasThrownPoison) && (Poisons.GetPoison(ePoison.Crippling) != null) && (ClassSettings._Instance.GeneralThrownPoison)) return true;

                // Always Stealthed
                if (ClassSettings._Instance.GeneralAlwaysStealthed && Battlegrounds.IsInsideBattleground && !StyxWoW.Me.HasAura("Stealth")) return true;

                return false;
            }
        }

        public override void PreCombatBuff()
        {
            // Always Stealthed
            if (ClassSettings._Instance.GeneralAlwaysStealthed && !StyxWoW.Me.HasAura("Stealth"))
            {
                if (Spell.HasCanSpell("Stealth"))
                    Spell.Cast("Stealth");
            }


            Managers.Poisons.Pulse();
        }
        #endregion
        #region PullBuff
        public override bool NeedPullBuffs
        {
            get
            {
                // We need to be stealthed
                if (SpellManager.CanCast("Stealth") && !Spell.HasAura("Stealth")) return true;

                // If sub, we need to cast premeditation
                if ((Managers.Spec.IsSubtlety) 
                    && (Spell.HasCanSpell("Premeditation")) 
                    && (!Spell.HasMyAura("Premeditation", StyxWoW.Me.CurrentTarget))
                    && (StyxWoW.Me.CurrentTarget.InLineOfSpellSight)) return true;

                return false;
            }
        }

        public override void PullBuff()
        {
            //////////////////// MOVEMENT ISSUE WITH HB TRYING TO FIX IT ////////////////////
            if (StyxWoW.Me.CurrentTarget.IsWithinMeleeRange == false)
            {
                Managers.NavMan.MoveBehind(StyxWoW.Me.CurrentTarget);
            }
            //////////////////// MOVEMENT ISSUE WITH HB TRYING TO FIX IT ////////////////////

            // Stealth
            if (!Spell.HasAura("Stealth"))
            {
                Spell.Cast("Stealth");
                return;
            }

            // if Sub and has a target, and is stealthed, cast Premeditation
            if ((Managers.Spec.IsSubtlety) 
                && (StyxWoW.Me.GotTarget) 
                && (Spell.HasAura("Stealth")))
            {
                Spell.Cast("Premeditation", StyxWoW.Me.CurrentTarget);
                return;
            }

        }
        #endregion

        public override bool NeedRest
        {
            get
            {
                if (StyxWoW.Me.HealthPercent >= ClassSettings._Instance.PVERestAt) return false;
                if (StyxWoW.Me.HasAura("Resurrection Sickness")) return false;
                if (StyxWoW.Me.Dead || StyxWoW.Me.IsGhost ||
                    StyxWoW.Me.IsCasting || StyxWoW.Me.Combat || 
                    StyxWoW.Me.IsSwimming || StyxWoW.Me.HasAura("Food") ||
                    Consumable.GetBestFood(false) == null || Battlegrounds.IsInsideBattleground) return false;

                return true;
            }
        }

        public override void Rest()
        {
            WoWMovement.MoveStop();
            StyxWoW.SleepForLagDuration();
            Styx.Logic.Common.Rest.FeedImmediate();
            StyxWoW.SleepForLagDuration();
            Spell.Cast("Stealth");

            while (!StyxWoW.Me.Combat && StyxWoW.Me.HealthPercent < 95) { }
        }
    }
}
