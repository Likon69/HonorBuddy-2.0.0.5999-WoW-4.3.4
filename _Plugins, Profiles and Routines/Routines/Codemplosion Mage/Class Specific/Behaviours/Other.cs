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

                // Mana Gem
                new NeedToManaGem(new ManaGem()),

                // Evocation
                 new NeedToEvocation(new Evocation()),
                
                // Lifeblood
                new NeedToLifeblood(new Lifeblood()),

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

                // Evocation
                new NeedToEvocation(new Evocation()),

                // Cancel Food
                new NeedToCancelFood(new CancelFood()),

                // Cancel Drink
                new NeedToCancelDrink(new CancelDrink()),

                // Drink
                new NeedToDrink(new Drink()),

                // Eat
                new NeedToEat(new Eat())

                // Eat and Drink
                //new NeedToEatDrink(new EatDrink()),

                );
        }

        #endregion



       
    }
}
