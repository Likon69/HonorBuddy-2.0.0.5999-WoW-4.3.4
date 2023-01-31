using Styx.WoWInternals;
using TreeSharp;
using Druid_ID = Hera.ClassHelpers.Druid.IDs;

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

                new Decorator(ret => (!Self.IsBuffOnMe(Druid_ID.MarkOfTheWild) && Spell.CanCast("Mark of the Wild") && Settings.MarkOfTheWild.Contains("always")),
                    new Action(ret => Spell.Cast("Mark of the Wild",Me))),

                // Predator's Swiftness
                new NeedToPredatorsSwiftness(new PredatorsSwiftness()),

                // Lifeblood
                new NeedToLifeblood(new Lifeblood()),

                // Use a health potion if we need it
                new NeedToUseHealthPot(new UseHealthPot()),

                // Use a mana potion if we need it
                new NeedToUseManaPot(new UseManaPot())

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

                // Cancel Food & Drink
                new NeedToCancelFood(new CancelFood()),
                new NeedToCancelDrink(new CancelDrink()),

                // Rest Heal
                new NeedToRestHeal(new RestHeal()),

                // Drink
                new NeedToDrink(new Drink()),

                // Eat
                new NeedToEat(new Eat())


                );
        }

        #endregion

        #region Rest Heal
        public class NeedToRestHeal : Decorator
        {
            public NeedToRestHeal(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Regrowth";

                if (Self.IsBuffOnMe(dpsSpell)) return false;
                if (Me.CurrentMana < Spell.PowerCost(dpsSpell)) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsHealthPercentAbove(Settings.RestHealth)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RestHeal : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Regrowth";
                bool result = Spell.Cast(dpsSpell,Me);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion


        #region Rejuvenation Balance
        public class NeedToRejuvenationBalance : Decorator
        {
            public NeedToRejuvenationBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rejuvenation";

                if (Me.Silenced) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                //if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Untalented) return false;
                if (Me.GotTarget && !Utils.Adds && Me.HealthPercent > 25 && (Me.HealthPercent * 1.2) > CT.HealthPercent && !Target.IsElite) return false;
                if (Me.Level >= 10 && ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Balance) return false;
                if (ClassHelpers.Druid.Shapeshift.IsCatForm && Me.ManaPercent < (Settings.CatFormManaBalance * 3)) return false;
                if (!CLC.ResultOK(Settings.RejuvenationBalance)) return false;
                if (Self.IsHealthPercentAbove(Settings.RejuvenationBalanceHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RejuvenationBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Rejuvenation";
                bool result = Spell.Cast(dpsSpell, Me);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Nourish Balance
        public class NeedToNourishBalance : Decorator
        {
            public NeedToNourishBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Nourish";

                if (Me.Silenced) return false;
                if (!Timers.Expired("Healing",2000)) return false;
                if (Me.GotTarget && !Utils.Adds && Me.HealthPercent > 25 && (Me.HealthPercent * 1.2) > CT.HealthPercent && !Target.IsElite) return false;
                if (Me.Level >= 10 && ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Balance) return false;
                if (ClassHelpers.Druid.Shapeshift.IsCatForm && Me.ManaPercent < (Settings.CatFormManaBalance * 3)) return false;
                if (!CLC.ResultOK(Settings.NourishBalance)) return false;
                if (Self.IsHealthPercentAbove(Settings.NourishBalanceHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class NourishBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Nourish";

                if (Settings.DoubleHeal.Contains("always") && (CLC.ResultOK(Settings.RejuvenationBalance) && !Self.IsHealthPercentAbove(Settings.RejuvenationBalanceHealth)))
                {
                    Spell.Cast("Rejuvenation", Me);
                    Utils.LagSleep();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(500);
                }


                if (Me.IsMoving){ Movement.StopMoving(); System.Threading.Thread.Sleep(500);}

                bool result = Spell.Cast(dpsSpell, Me);
                System.Threading.Thread.Sleep(500);
                ObjectManager.Update();
                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.NourishBalanceHealth, Me);
                Timers.Reset("Healing");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Regrowth Balance
        public class NeedToRegrowthBalance : Decorator
        {
            public NeedToRegrowthBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Regrowth";

                if (Me.Silenced) return false;
                if (!Timers.Expired("Healing", 2000)) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                if (Me.GotTarget && !Utils.Adds && Me.HealthPercent > 25 && (Me.HealthPercent * 1.2) > CT.HealthPercent && !Target.IsElite) return false;
                if (Me.Level >= 10 && ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Balance) return false;
                if (ClassHelpers.Druid.Shapeshift.IsCatForm && Me.ManaPercent < (Settings.CatFormManaBalance * 3)) return false;
                if (!CLC.ResultOK(Settings.RegrowthBalance)) return false;
                if (Self.IsHealthPercentAbove(Settings.RegrowthBalanceHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RegrowthBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Regrowth";

                if (Settings.DoubleHeal.Contains("always") && (CLC.ResultOK(Settings.RejuvenationBalance) && !Self.IsHealthPercentAbove(Settings.RejuvenationBalanceHealth)))
                {
                    Spell.Cast("Rejuvenation", Me);
                    Utils.LagSleep();
                    while (Spell.IsGCD) System.Threading.Thread.Sleep(500);
                }

                bool result = Spell.Cast(dpsSpell, Me);
                System.Threading.Thread.Sleep(500);
                ObjectManager.Update();
                Utils.WaitWhileCasting(Utils.CastingBreak.HealthIsAbove, Settings.RegrowthBalanceHealth, Me);
                Timers.Reset("Healing");

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Innervate Balance

        public class NeedToInnervateBalance : Decorator
        {
            public NeedToInnervateBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Innervate";
                if (Me.Silenced) return false;
                if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Balance) return false;
                if (Self.IsPowerPercentAbove(Settings.InnervateManaBalance)) return false;

                return Utils.CCheck(dpsSpell);
            }
        }

        public class InnervateBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Innervate";
                bool result = Spell.Cast(dpsSpell,Me);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Barkskin Balance

        public class NeedToBarkskinBalance : Decorator
        {
            public NeedToBarkskinBalance(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Barkskin";

                if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Balance) return false;
                if (Self.IsHealthPercentAbove(Settings.BarkskinBalanceHealth)) return false;
                bool result = Utils.CCheck(dpsSpell, "clc:" + Settings.BarkskinBalance);
                return result;
            }
        }

        public class BarkskinBalance : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Barkskin";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        // Feral Cat
        #region Barkskin Feral Cat
        public class NeedToBarkskinFeralCat : Decorator
        {
            public NeedToBarkskinFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Barkskin";

                if (!CLC.ResultOK(Settings.BarkskinFeralCat)) return false;
                if (Self.IsHealthPercentAbove(Settings.BarkskinFeralCatHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class BarkskinFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Barkskin";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Rejuvenation Feral Cat
        public class NeedToRejuvenationFeralCat : Decorator
        {
            public NeedToRejuvenationFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Rejuvenation";

                if (Me.Silenced) return false;
                if (!CLC.ResultOK(Settings.RejuvenationFeralCat)) return false;
                if (Self.IsHealthPercentAbove(Settings.RejuvenationFeralCatHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                if (Self.IsBuffOnMe(Druid_ID.SurvivalInstincts)) return false;
                if (Me.CurrentMana < ClassHelpers.Druid.Shapeshift.ShapeshiftRountTripCost(dpsSpell,"Regrowth")) return false;
                if (!Timers.Expired("FeralCatHealSpam",Settings.FeralCatHealSpam)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RejuvenationFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = false;
                const string dpsSpell = "Rejuvenation";
                if (!Self.IsBuffOnMe(dpsSpell)) {result = Spell.Cast(dpsSpell);}
                if (Settings.DoubleDotFeralCat.Contains("always") && !Self.IsBuffOnMe(Druid_ID.Regrowth) && CLC.ResultOK(Settings.RejuvenationFeralCat))
                {
                    double modifiedRegrowthHealth = Settings.RejuvenationFeralCatHealth;
                    if (Settings.RejuvenationFeralCatHealth > Settings.RegrowthFeralCatHealth) { modifiedRegrowthHealth = Settings.RejuvenationBalanceHealth*1.2; }
                    if (!Self.IsHealthPercentAbove((int)modifiedRegrowthHealth))
                    {
                        if (Spell.IsGCD || Me.IsCasting) return RunStatus.Running;
                        Utils.Log("** Regrowth will also be cast. Health is close enough **", Utils.Colour("Red"));
                        Spell.Cast("Regrowth", Me);
                    }
                }

                Timers.Reset("FeralCatHealSpam");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Regrowth Feral Cat
        public class NeedToRegrowthFeralCat : Decorator
        {
            public NeedToRegrowthFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Regrowth";

                if (Me.Silenced) return false;
                if (!CLC.ResultOK(Settings.RegrowthFeralCat)) return false;
                if (Self.IsHealthPercentAbove(Settings.RegrowthFeralCatHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(dpsSpell)) return false;
                if (Self.IsBuffOnMe(Druid_ID.SurvivalInstincts)) return false;
                if (!Timers.Expired("FeralCatHealSpam", Settings.FeralCatHealSpam)) return false;
                if (Me.CurrentMana < ClassHelpers.Druid.Shapeshift.ShapeshiftRountTripCost(dpsSpell, "Rejuvenation")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class RegrowthFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Regrowth";
                bool result = Spell.Cast(dpsSpell,Me);

                if (Settings.DoubleDotFeralCat.Contains("always") && !Self.IsBuffOnMe(Druid_ID.Rejuvenation) && CLC.ResultOK(Settings.RejuvenationFeralCat))
                {
                    double modifiedRejuvenationthHealth = Settings.RejuvenationFeralCatHealth;
                    if (Settings.RegrowthFeralCatHealth > Settings.RejuvenationFeralCatHealth) { modifiedRejuvenationthHealth = Settings.RejuvenationBalanceHealth * 1.2; }
                    if (!Self.IsHealthPercentAbove((int)modifiedRejuvenationthHealth))
                    {
                        Utils.Log("** Rejuvenation will also be cast. Health is close enough **", Utils.Colour("Red"));
                        if (Spell.IsGCD || Me.IsCasting ) return RunStatus.Running;
                        Spell.Cast("Regrowth", Me);
                    }
                }

                Timers.Reset("FeralCatHealSpam");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Nourish Feral Cat
        public class NeedToNourishFeralCat : Decorator
        {
            public NeedToNourishFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Nourish";

                if (Me.Silenced) return false;
                if (!CLC.ResultOK(Settings.NourishFeralCat)) return false;
                if (Self.IsHealthPercentAbove(Settings.NourishFeralCatHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;
                if (Self.IsBuffOnMe(Druid_ID.SurvivalInstincts)) return false;
                if (!Timers.Expired("FeralCatHealSpam", Settings.FeralCatHealSpam)) return false;
                if (Me.CurrentMana < ClassHelpers.Druid.Shapeshift.ShapeshiftRountTripCost(dpsSpell,"")) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class NourishFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Nourish";
                bool result = Spell.Cast(dpsSpell);

                Timers.Reset("FeralCatHealSpam");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Survival Instincts Feral Cat
        public class NeedToSurvivalInstinctsFeralCat : Decorator
        {
            public NeedToSurvivalInstinctsFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Survival Instincts";

                if (!CLC.ResultOK(Settings.SurvivalInstinctsFeralCat)) return false;
                if (Self.IsHealthPercentAbove(Settings.SurvivalInstinctsFeralCatHealth)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class SurvivalInstinctsFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Survival Instincts";
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion

        #region Innervate Feral Cat

        public class NeedToInnervateFeralCat : Decorator
        {
            public NeedToInnervateFeralCat(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                const string dpsSpell = "Innervate";
                if (Me.Silenced) return false;
                if (ClassHelpers.Druid.ClassSpec != ClassHelpers.Druid.ClassType.Feral) return false;
                if (Me.ManaPercent > Settings.InnervateManaFeralCat) return false;

                return Utils.CCheck(dpsSpell);
            }
        }

        public class InnervateFeralCat : Action
        {
            protected override RunStatus Run(object context)
            {
                const string dpsSpell = "Innervate";
                bool result = Spell.Cast(dpsSpell, Me);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }

        #endregion

        #region Predators Swiftness
        public class NeedToPredatorsSwiftness : Decorator
        {
            public NeedToPredatorsSwiftness(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                string dpsSpell = Settings.PredatorsSwiftnessFeralCatSpell;

                if (Me.Silenced) return false;
                if (!CLC.ResultOK(Settings.PredatorsSwiftnessFeralCat)) return false;
                if (Self.IsHealthPercentAbove(Settings.PredatorsSwiftnessFeralCatHealth)) return false;
                if (!Self.IsBuffOnMe(Druid_ID.PredatorsSwiftness)) return false;
                if (Self.Immobilised) return false;
                if (!Utils.CombatCheckOk(dpsSpell, false)) return false;

                return (Spell.CanCast(dpsSpell));
            }
        }

        public class PredatorsSwiftness : Action
        {
            protected override RunStatus Run(object context)
            {
                string dpsSpell = Settings.PredatorsSwiftnessFeralCatSpell;
                Utils.Log("** Predator's Swiftness **");
                bool result = Spell.Cast(dpsSpell);

                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion



       
    }
}
