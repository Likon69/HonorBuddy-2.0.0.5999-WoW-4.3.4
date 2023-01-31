using TreeSharp;

namespace Hera
{
    public partial class Codemplosion
    {
        #region Heal Behaviour
        private Composite _healBehavior;

        public override Composite HealBehavior
        {
            get { if (_healBehavior == null) { Utils.Log("Creating 'Heal' behavior"); _healBehavior = CreateHealBehavior(); }  return _healBehavior; }
        }

        private static Composite CreateHealBehavior()
        {
            return new PrioritySelector(

                // Lifeblood
                new NeedToLifeblood(new Lifeblood()),

                // Use a health potion if we need it
                new NeedToUseHealthPot(new UseHealthPot()),

                // Heal Pet
                new NeedToHealPet(new HealPet())

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

                // We're full. Stop eating/drinking no point sitting there doing nothing wating for the Eat/Drink buff to disapear
                new NeedToCancelFoodDrink(new CancelFoodDrink()),

                // Eat and Drink
                new NeedToEatDrink(new EatDrink())

                );
        }
        #endregion

        #region Heal Pet
        public class NeedToHealPet : Decorator
        {
            public NeedToHealPet(Composite child) : base(child) { }

            protected override bool CanRun(object context)
            {
                if (!Utils.CombatCheckOk("Mend Pet", false)) return false;
                if (!ClassHelpers.Hunter.Pet.NeedToHealPet) return false;

                return (Spell.CanCast("Mend Pet"));
            }
        }

        public class HealPet : Action
        {
            protected override RunStatus Run(object context)
            {
                bool result = Spell.Cast("Mend Pet");
                return result ? RunStatus.Success : RunStatus.Failure;
            }
        }
        #endregion



    }
}
