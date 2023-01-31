using TreeSharp;
using Styx.Logic.Combat;
using CommonBehaviors.Actions;

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite _pullBuffBehavior;
        public override Composite PullBuffBehavior
        {
            get
            {
                if (_pullBuffBehavior == null)
                {
                    Log("Creating 'PullBuff' behavior");
                    _pullBuffBehavior = CreatePullBuffBehavior();
                }

                return _pullBuffBehavior;
            }
        }

        /// <summary>
        /// Creates the behavior used for pulling mobs
        /// </summary>
        /// <returns></returns>
        //public string SummonName = "Summon " + CurrentPetSpell +"";
        private Composite CreatePullBuffBehavior()
        {
            return new PrioritySelector(

       
                );
        }
    }
}
