
using Styx.Logic.Combat;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite _healBehavior;
        public override Composite HealBehavior
        {
            get
            {
                if (_healBehavior == null)
                {
                    Log("Creating 'Heal' behavior");
                    _healBehavior = CreateHealBehavior();
                }

                return _healBehavior;
            }
        }

        /// <summary>
        /// Creates the behavior used for healing
        /// </summary>
        /// <returns></returns>
        private static Composite CreateHealBehavior()
        {
            return new PrioritySelector(

                // Cast lifeblood if we have it
                new Decorator(ret => SpellManager.HasSpell("Lifeblood") && SpellManager.CanCast("Lifeblood"),
                              new Action(ret => SpellManager.Cast("Lifeblood")))

               
                );
        }
    }
}
