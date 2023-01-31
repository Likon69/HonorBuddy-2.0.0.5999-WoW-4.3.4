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

                
                // Lifeblood
                new NeedToLifeblood(new Lifeblood()),

                // Pain Suppression
                new NeedToPainSuppression(new PainSuppression()),

                // Renew
                new NeedToRenew(new Renew()),

                // Power Word Shield
                new NeedToPowerWordShield(new PowerWordShield()),

                // Flash Heal
                new NeedToFlashHeal(new FlashHeal()),

                // Use Mana Potion
                new NeedToUseManaPot(new UseManaPot())

                // Use a health potion if we need it
                //new NeedToUseHealthPot(new UseHealthPot())

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

                // Dispersion Rest
                new NeedToDispersionRest(new DispersionRest()),

                // Cancel Food
                new NeedToCancelFood(new CancelFood()),

                // Cancel Drink
                new NeedToCancelDrink(new CancelDrink()),

                // Heal (Flash Heal) before drinking
                new NeedToRestHeal(new RestHeal()),

                // Drink
                new NeedToDrink(new Drink()),

                // Eat
                new NeedToEat(new Eat()),

                // Eat and Drink
                //new NeedToEatDrink(new EatDrink()),

                // Shadowform
                new NeedToShadowform(new Shadowform())

                );
        }

        #endregion

        #region Rest Heal

        public class NeedToRestHeal : Decorator
        {
            public NeedToRestHeal(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string spellName = "Flash Heal";

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
                string spellName = "Flash Heal";
                bool result = Spell.Cast(spellName, Me);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.RestHealPercent, Me);
                Timers.Reset("HealingSpells");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Flash Heal
        public class NeedToFlashHeal : Decorator
        {
            public NeedToFlashHeal(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Flash Heal";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.FlashHealHealth)) return false;

                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class FlashHeal : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Flash Heal";

                // Cast PWS first if its needed
                // 6788 - Weakened Soul
                if (!Self.IsHealthPercentAbove(Settings.PowerWordShieldHealth) && !Self.IsBuffOnMe(6788, Self.AuraCheck.AllAuras) && Spell.CanCast("Power Word: Shield"))
                {
                    Spell.Cast("Power Word: Shield", Me);
                    Utils.LagSleep();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                }

                // Inner Focus on low mana
                //if (!Self.IsPowerPercentAbove(Settings.InnerFocusMana) && Spell.CanCast("Inner Focus") && !Self.IsBuffOnMe("Inner Focus"))
                if (Spell.CanCast("Inner Focus") && !Self.IsBuffOnMe("Inner Focus"))
                {
                    Spell.Cast("Inner Focus", Me);
                    Utils.LagSleep();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                }


                bool result = Spell.Cast(dpsSpell,Me);
                Utils.LagSleep();

                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.FlashHealHealth, Me);
                Timers.Reset("HealingSpells");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Pain Suppression
        public class NeedToPainSuppression: Decorator
        {
            public NeedToPainSuppression(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Pain Suppression";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.PainSuppressionHealth)) return false;

                if (!Timers.Expired("HealingSpells", Settings.HealingSpellTimer)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PainSuppression : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Pain Suppression";


                bool result = Spell.Cast(dpsSpell, Me);
                Utils.LagSleep();
                Timers.Reset("HealingSpells");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Power Word Shield
        public class NeedToPowerWordShield: Decorator
        {
            public NeedToPowerWordShield(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                // 6788 - Weakened Soul
                const string dpsSpell = "Power Word: Shield";

                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.PowerWordShieldHealth)) return false;
                if (ClassHelpers.Common.SkipHeal(Settings.HealingAbsoluteMinimum, Settings.HealingModifierSolo)) return false;
                if (Self.IsBuffOnMe(6788,Self.AuraCheck.AllAuras)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PowerWordShield : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Power Word: Shield";

                if (!Self.IsHealthPercentAbove(Settings.RenewHealth) && !Self.IsBuffOnMe("Renew") && Spell.CanCast("Renew"))
                {
                    Spell.Cast("Renew", Me);
                    Utils.LagSleep();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                }


                bool result = Spell.Cast(dpsSpell);
                Utils.LagSleep();

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Renew
        public class NeedToRenew : Decorator
        {
            public NeedToRenew(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Renew";

                //if (!CLC.ResultOK(Settings.Renew)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.RenewHealth)) return false;
                if (Self.IsBuffOnMe("Renew")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class Renew : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Renew";

                // Cast PWS first if its needed
                // 6788 - Weakened Soul
                if (!Self.IsHealthPercentAbove(Settings.PowerWordShieldHealth) && !Self.IsBuffOnMe(6788, Self.AuraCheck.AllAuras) && Spell.CanCast("Power Word: Shield"))
                {
                    Spell.Cast("Power Word: Shield", Me);
                    Utils.LagSleep();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(250);
                }

                Spell.Cast(dpsSpell);
                Utils.LagSleep();
                
                bool result = Self.IsBuffOnMe(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Dispersion Rest
        public class NeedToDispersionRest : Decorator
        {
            public NeedToDispersionRest(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Dispersion";

                if (Self.IsBuffOnMe(dpsSpell)) return true;

                if (!Self.IsHealthPercentAbove(Settings.RestHealPercent)) return false;
                if (Self.IsPowerPercentAbove(Settings.RestMana)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class DispersionRest : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Dispersion";
                if (Self.IsBuffOnMe(dpsSpell)) return RunStatus.Success;
                
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

       
    }
}
