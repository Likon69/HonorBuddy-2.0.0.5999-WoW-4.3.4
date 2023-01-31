using TreeSharp;

namespace Hera
{
    public partial class Codemplosion
    {
        #region Heal Behaviour

        private Composite _healBehavior;

        public override Composite HealBehavior
        {
            get { if (_healBehavior == null) { Utils.Log("Creating 'Heal' behavior");  _healBehavior = CreateHealBehavior(); }  return _healBehavior; }
        }

        private static Composite CreateHealBehavior()
        {
            return new PrioritySelector(

                // Ardent Defender
                new NeedToArdentDefender(new ArdentDefender()),
                
                // Lay On Hands
                new NeedToLayOnHands(new LayOnHands()),

                // Holy Shock Heal
                new NeedToHolyShockHeal(new HolyShockHeal()),

                // Word Of Glory
                new NeedToWordOfGlory(new WordOfGlory()),

                // Divine Protection
                new NeedToDivineProtection(new DivineProtection()),

                // Holy Light
                new NeedToHolyLight(new HolyLight()),

                // Flash Of Light
                new NeedToFlashOfLight(new FlashOfLight()),

                // Divine Light
                new NeedToDivineLight(new DivineLight()),

                // Divine Shield
                new NeedToDivineShield(new DivineShield()),
                
                // Lifeblood
                new NeedToLifeblood(new Lifeblood()),

                // Divine Plea
                new NeedToDivinePlea(new DivinePlea()),

                // Use a health potion if we need it
                new NeedToUseHealthPot(new UseHealthPot())

                );
        }

        #endregion

        #region Rest Behaviour
        private Composite _restBehavior;
        public override Composite RestBehavior
        {
            get { if (_restBehavior == null) { Utils.Log("Creating 'Rest' behavior"); _restBehavior = CreateRestBehavior(); } return _restBehavior; }
        }

        private Composite CreateRestBehavior()
        {
            return new PrioritySelector(

                // We're full. Stop eating/drinking
                // No point sitting there doing nothing wating for the Eat/Drink buff to disapear
                new NeedToCancelFoodDrink(new CancelFoodDrink()),

                // Heal (Flash of Light) before drinking
                new NeedToRestHeal(new RestHeal()),

                // Eat and Drink
                new NeedToEatDrink(new EatDrink())

                );
        }

        #endregion

        #region Rest Heal

        public class NeedToRestHeal : Decorator
        {
            public NeedToRestHeal(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Flash of Light";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.RestHealPercent)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class RestHeal : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Flash of Light";
                bool result = Spell.Cast(spellName, Me);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.RestHealPercent, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Holy Light
        public class NeedToHolyLight : Decorator
        {
            public NeedToHolyLight(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Holy Light";

                if (Me.Combat && Self.IsHealthPercentAbove(Settings.HealingAbsoluteMinimum))
                {
                    if (Settings.HolyLight.Contains("Crusader") && !Self.IsBuffOnMe(94686)) return false;
                    if (!Settings.HolyLight.Contains("Crusader") && !CLC.ResultOK(Settings.HolyLight)) return false;
                }
                if (!Utils.CombatCheckOk(spellName, true)) return false;
                if (Self.IsHealthPercentAbove(Settings.HolyLightHealth)) return false;
                if (!Timers.Expired("HealingSpells",Settings.HealingSpellTimer)) return false;
                
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class HolyLight : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Holy Light";

                if (Me.IsMoving) Movement.StopMoving();
                bool result = Spell.Cast(spellName);
                
                Utils.LagSleep();
                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.HolyLightHealth, Me);
                Timers.Reset("HealingSpells");
                //while (Me.IsCasting) { if (Self.IsHealthAbove(Settings.HolyLightHealth)) Spell.StopCasting(); if (!Self.IsHealthAbove(Settings.LayOnHandsHealth)) Spell.StopCasting(); });
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Flash Of Light

        public class NeedToFlashOfLight : Decorator
        {
            public NeedToFlashOfLight(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Flash of Light";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.FlashOfLightHealth)) return false;
                if (Me.Combat && Self.IsHealthPercentAbove(Settings.HealingAbsoluteMinimum))
                    if (!CLC.ResultOK(Settings.FlashOfLight)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class FlashOfLight : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Flash of Light";
                if (Me.IsMoving) Movement.StopMoving();
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.FlashOfLightHealth, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Divine Protection

        public class NeedToDivineProtection : Decorator
        {
            public NeedToDivineProtection(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Divine Protection";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.DivineProtectionHealth)) return false;
                //if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class DivineProtection : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Divine Protection";
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                //Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.DivineProtectionHealth, Me);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Lay On Hands

        public class NeedToLayOnHands : Decorator
        {
            public NeedToLayOnHands(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Lay on Hands";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.LayOnHandsHealth)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                //if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class LayOnHands : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Lay on Hands";
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.LayOnHandsHealth, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Ardent Defender

        public class NeedToArdentDefender: Decorator
        {
            public NeedToArdentDefender(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Ardent Defender";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.ArdentDefenderHealth)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class ArdentDefender : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Ardent Defender";
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Divine Plea

        public class NeedToDivinePlea : Decorator
        {
            public NeedToDivinePlea(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Divine Plea";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsPowerPercentAbove(Settings.DivinePleaMana)) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class DivinePlea : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Divine Plea";

                bool result = Spell.Cast(spellName);
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Word of Glory
        public class NeedToWordOfGlory : Decorator
        {
            public NeedToWordOfGlory(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Word of Glory";

                if (!Utils.CombatCheckOk(spellName, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.WordOfGloryHealth)) return false;
                if (Me.CurrentHolyPower <= 0) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum,Settings.HealingModifierSolo)) return false;
                //if (!Utils.Adds && Self.IsHealthPercentAbove(Settings.HealingAbsoluteMinimum) && (Me.HealthPercent * Settings.HealingModifierSolo) > Target.HealthPercent) return false;

                return (Spell.CanCast(spellName));
            }
        }

        public class WordOfGlory : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Word of Glory";
                bool result = Spell.Cast(spellName);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Divine Light
        public class NeedToDivineLight : Decorator
        {
            public NeedToDivineLight(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Divine Light";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.DivineLightHealth)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DivineLight : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Divine Light";
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.DivineLightHealth, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Holy Shock
        public class NeedToHolyShockHeal: Decorator
        {
            public NeedToHolyShockHeal(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Holy Shock";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.HolyShockHealth)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class HolyShockHeal : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Holy Shock";
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.HolyShockHealth, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Divine Shield
        public class NeedToDivineShield: Decorator
        {
            public NeedToDivineShield(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Divine Shield";

                if (Self.IsBuffOnMe("Forbearance")) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.DivineShieldHealth)) return false;
                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DivineShield : Action
        {
            protected override RunStatus Run(object context)
            {
                string spellName = "Divine Shield";
                bool result = Spell.Cast(spellName);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.DivineShieldHealth, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion
       
    }
}
