using Styx.Logic.Combat;
using TreeSharp;
using CommonBehaviors.Actions;

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite _preCombatBuffBehavior;
        public override Composite PreCombatBuffBehavior
        {
            get
            {
                if (_preCombatBuffBehavior == null)
                {
                    Log("Creating 'PreCombatBuff' behavior");
                    _preCombatBuffBehavior = CreatePreCombatBuffsBehavior();
                }
                _preCombatBuffBehavior = CreatePreCombatBuffsBehavior();
                return _preCombatBuffBehavior;
            }
        }

        /// <summary>
        /// Creates the behavior used for buffing with regular buffs. eg; 'Power Word: Fortitude', 'Inner Fire' etc...
        /// </summary>
        /// <returns></returns>
        private Composite CreatePreCombatBuffsBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => !Me.Mounted && !Me.ActiveAuras.ContainsKey("Demon Armor") && !Me.ActiveAuras.ContainsKey("Fel Armor") && !SpellManager.HasSpell("Fel Armor") && SpellManager.HasSpell("Demon Armor") && ZerfallSettings.Instance.ArmorSelect == "Automatic",
                              new PrioritySelector(
                                  CreateBuffCheckAndCast("Demon Armor")
                                  )),
                new Decorator(ret => !Me.Mounted && !Me.ActiveAuras.ContainsKey("Demon Armor") && !Me.ActiveAuras.ContainsKey("Fel Armor") && SpellManager.HasSpell("Fel Armor") && SpellManager.HasSpell("Demon Armor") && ZerfallSettings.Instance.ArmorSelect == "Automatic",
                              new PrioritySelector(
                                  CreateBuffCheckAndCast("Fel Armor")
                                  )),
                new Decorator(ret => !Me.Mounted && !Me.ActiveAuras.ContainsKey("Demon Armor") && !Me.ActiveAuras.ContainsKey("Fel Armor") && ZerfallSettings.Instance.ArmorSelect == "Demon Armor",
                              new PrioritySelector(
                                  CreateBuffCheckAndCast("Demon Armor")
                                  )),

                new Decorator(ret => !Me.Mounted && !Me.ActiveAuras.ContainsKey("Demon Armor") && !Me.ActiveAuras.ContainsKey("Fel Armor") && ZerfallSettings.Instance.ArmorSelect == "Fel Armor",
                              new PrioritySelector(
                                  CreateBuffCheckAndCast("Fel Armor")
                                  )),

                new Decorator(ret => !HaveHealthStone() && SpellManager.CanCast("Create Healthstone"),
                              new PrioritySelector(
                                  new Wait(4, ret => HaveHealthStone(),
                                  new ActionIdle()),
                                  new Action(ctx => SpellManager.Cast("Create Healthstone"))
                                                    ))
                                  

                );
        }
    }
}
