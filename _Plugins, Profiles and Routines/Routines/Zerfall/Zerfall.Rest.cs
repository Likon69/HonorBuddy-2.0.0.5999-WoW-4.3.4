using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using CommonBehaviors.Actions;

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite _restBehavior;
        public override Composite RestBehavior
        {
            get
            {
                if (_restBehavior == null)
                {
                    Log("Creating 'Rest' behavior");
                    _restBehavior = CreateRestBehavior();
                }
                _restBehavior = CreateRestBehavior();
                return _restBehavior;
            }
        }

        /// <summary>
        /// Creates the behavior used for resting. Eating/Drinking
        /// </summary>
        /// <returns></returns>
        private Composite CreateRestBehavior()
        {
            return new PrioritySelector(

                // If our health and mana is 100 we make it return failure to make sure 'NeedRest' returns false and allow it to proceed executing other code!
                // also cacel any existing food/drink buff.
                new Decorator(ret => !Me.IsMoving && Me.HealthPercent == 100 && Me.ManaPercent == 100,
                              new Action(delegate
                                             {
                                                 if (Me.Buffs.ContainsKey("Food") || Me.Buffs.ContainsKey("Drink"))
                                                     Lua.DoString(
                                                         "CancelUnitBuff('player', 'Food') CancelUnitBuff('player', 'Drink')");

                                                 return RunStatus.Failure;
                                             })),

                new Decorator(ctx => !ZerfallSettings.Instance.MoveDisable && Me.IsCasting || Me.Buffs.ContainsKey("Food") || Me.Buffs.ContainsKey("Drink"),
                              new Action(delegate { return RunStatus.Success; })
                    ),

                // Don't rest if we are mounted or swimming
                new Decorator(ctx => !Me.IsMoving && !Me.Mounted && !Me.IsSwimming,
                              // Decide what and if to do          
                              new PrioritySelector(
                                  // Check if we need to  take a drink or bite
                                  new Decorator(ret =>
                                                Me.ManaPercent <= ZerfallSettings.Instance.RestManaPercentage
                                                || Me.HealthPercent <= ZerfallSettings.Instance.RestHealthPercentage ,

                                                new Sequence(
                                                    // Clear our target
                                                    new Action(ctx => Me.ClearTarget()),
                                                    // Stop moving if we are moving
                                                    new Action(delegate
                                                                   {
                                                                       WoWMovement.MoveStop();
                                                                       return Me.IsMoving
                                                                                  ? RunStatus.Running
                                                                                  : RunStatus.Success;
                                                                   }),
                                                    new Action(ctx => WarlockRest(null)))
                                                    ),
                                                    new Decorator(ret => SpellManager.CanCast("Soul Harvest") && Me.CurrentSoulShards <= 1 && Me.HealthPercent <= 75 && ZerfallSettings.Instance.SoulHarvestRest,
                                                        new Action(ret => SpellManager.Cast("Soul Harvest"))),

                                                                      new Decorator(ret => ZerfallSettings.Instance.PetSpell != "No Pet" && !Me.Mounted && !Me.IsFlying && !Me.IsOnTransport && !Me.GotAlivePet,
                                                                            new PrioritySelector(
                                                                                new Wait(4, ret => Me.GotAlivePet,
                                                                                new ActionIdle()),
                                                                                SummonPet("Summon " + CurrentPetSpell + ""))),

                                                    new Decorator(ctx => !StyxWoW.Me.NormalBagsFull,
                                                                        new PrioritySelector(
           
                                                    // Do we need to make a Gem?, don't create any if we got refreshments!
                                                     new Decorator(ret => !HaveHealthStone() && SpellManager.CanCast("Create Healthstone"),
                                                            new Action(ctx => SpellManager.Cast("Create Healthstone")
                                                    )),
                // Return 'Success' if we have the food or drink buff as then we should already be drinking
                                  new Decorator(ret => Me.HasAura("Food") || Me.HasAura("Drink"),
                                                new ActionIdle()))
                                  ))));





                  
                                  
        }

        private static RunStatus WarlockRest(object context)
        {
            if (LegacySpellManager.GlobalCooldown)
                return RunStatus.Running;
            /*if (!HaveHealthStone() && SpellManager.CanCast("Create Healthstone") && Me.ManaPercent >= SpellManager.Spells["Create Healthstone"].ManaCostPercent)
            {
                Logging.Write("Creating HealthStone");
                SpellManager.Cast("Create Healthstone");
            }*/
            if (Me.HealthPercent <= ZerfallSettings.Instance.RestHealthPercentage && !Me.Buffs.ContainsKey("Food"))
            {

                if (ZerfallSettings.Instance.RestStone && HaveHealthStone() && Me.ManaPercent >= ZerfallSettings.Instance.RestManaPercentage)
                {
                  
                    Logging.Write("Using HealthStone For Rest");
                    UseHealthStone();
                }
                else
                {

                    Styx.Logic.Common.Rest.Feed();
                }
            }

            if (Me.ManaPercent <= ZerfallSettings.Instance.RestManaPercentage && !Me.Buffs.ContainsKey("Drink"))
                {
                    Styx.Logic.Common.Rest.Feed();
                }


            

            return RunStatus.Success;
        }

  
    }
}
