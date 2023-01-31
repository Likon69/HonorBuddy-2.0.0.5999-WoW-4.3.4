using System.Collections.Generic;
using System.Threading;
using CommonBehaviors.Actions;
using Styx;
using Zerfall.Talents;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Sequence = TreeSharp.Sequence;
using System.Linq;

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite _combatBehavior;
        public override Composite CombatBehavior
        {
            get
            {
                if (_combatBehavior == null)
                {
                    Log("Creating 'Combat' behavior");
                    _combatBehavior = CreateCombatBehavior();
                }
                _combatBehavior = CreateCombatBehavior();
                return _combatBehavior;
            }
        }

        /// <summary>s
        /// Creates the behavior used for combat. Castsequences, add management, crowd control etc.
        /// </summary>
        /// <returns></returns>
        private Composite CreateCombatBehavior()
        {
            return new PrioritySelector(

                    CreateCombat()
                );
        }


        private Composite CreateCombat()
        {

            return new PrioritySelector(

              /*  new Decorator(ret => RaFHelper.Leader != null,
                // Use leaders target
                              new Decorator(ret =>

                                            (RaFHelper.Leader.GotTarget && Me.GotTarget &&
                                             Me.CurrentTarget.Guid != RaFHelper.Leader.CurrentTargetGuid &&
                                             !RaFHelper.Leader.CurrentTarget.Dead &&
                                             RaFHelper.Leader.CurrentTarget.Attackable &&
                                             !RaFHelper.Leader.CurrentTarget.IsFriendly) ||

                                            (!Me.GotTarget &&
                                             RaFHelper.Leader.GotTarget &&
                                             !RaFHelper.Leader.CurrentTarget.Dead &&
                                             RaFHelper.Leader.CurrentTarget.Attackable &&
                                             !RaFHelper.Leader.CurrentTarget.IsFriendly),

                                            new Sequence(
                                                new Action(delegate
                                                {
                                                    RaFHelper.Leader.CurrentTarget.Target();
                                                }),

                                                new Wait(3, ret => Me.GotTarget,
                                                    new ActionIdle())
                                                ))),*/

                // Make sure we got a proper target
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && ((Me.GotTarget && Targeting.Instance.FirstUnit != null && Me.CurrentTarget != Targeting.Instance.FirstUnit) || !Me.GotTarget),
                    new Sequence(

                        // Set Target!
                        new Action(ret => Targeting.Instance.FirstUnit.Target()),

                        // Wait until we got a target
                        new Wait(3, ret => Me.GotTarget,
                            new ActionIdle()))
                    ),

                //Added to make sure we have a target when movement is disabled before spamming my pull sqeuence
                new Decorator(ret => ZerfallSettings.Instance.MoveDisable && !Me.GotTarget && !Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.Dead && Me.CurrentTarget.Attackable,
                    new Action(ctx => RunStatus.Success)),

                //If we have an active pet, make it attack the same target we are.
               new Decorator(ret => (Me.CurrentTarget != null || !Me.CurrentTarget.Dead) && (Me.GotTarget && Me.GotAlivePet && (!Me.Pet.GotTarget || Me.Pet.CurrentTarget != Me.CurrentTarget)),
                    new Action( ret => Lua.DoString("PetAttack()"))),

                // Face thege tart if we aren't
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.GotTarget && !Me.IsFacing(Me.CurrentTarget),
                              new Action(ret => WoWMovement.Face())),

                new Decorator(ret => Me.IsCasting || Me.Silenced,
                    new Action(ctx => RunStatus.Success)),

                // Move closer to the target if we are too far away or in !Los
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.GotTarget && (Me.CurrentTarget.Distance > PullDistance + 3 || !Me.CurrentTarget.InLineOfSight),
                    new NavigationAction(ret => Me.CurrentTarget.Location)),

                // At this point we shouldn't be moving. Atleast not with this 'simple' kind of logic
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.IsMoving,
                              new Action(ret => WoWMovement.MoveStop())),

                new Decorator(ret => Me.HealthPercent <= ZerfallSettings.Instance.HealthStoneCombat && HaveHealthStone() && HealthStoneNotCooldown(),
                              new Action(ret => UseHealthStone())),
                // Use Potions
                new Decorator(ret => Me.HealthPercent <= ZerfallSettings.Instance.HealthPotPercent && HaveHealthPotion() && HealthPotionReady(),
                              new Action(ret => UseHealthPotion())),

                new Decorator(ret => Me.ManaPercent <= ZerfallSettings.Instance.ManaPotPercent && HaveManaPotion() && ManaPotionReady(),
                              new Action(ret => UseManaPotion())),

                 new Decorator(ret => ZerfallSettings.Instance.PS_Sacrifice && !Me.Auras.ContainsKey("Sacrifice") && Me.GotAlivePet && Me.HealthPercent <= 20 && Me.Pet.CreatedBySpellId == 697,
                              new Action(ctx =>
                                  PetSkill("Sacrifice"))
                                  ),
                //Old PetCast Logic. 
                //new Decorator(ret => Me.GotAlivePet && Me.Pet.CreatedBySpellId == 30146 && getAdds2().Count >= ZerfallSettings.Instance.Adds,
                               // new Action(ctx => PetSkill("Felstorm"))),
                //Using PetManager Logic
                PetSpellCheckAndCast("Felstorm", ret => Me.GotAlivePet && Me.Pet.CreatedBySpellId == SummonFelguard && getAdds2().Count >= ZerfallSettings.Instance.Adds),

                //Soul shard Re-coop needs to turn off during LazyRaider use, Dont think Legaston designed this to be on in his CC since i added it. 
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.GotTarget && Me.CurrentSoulShards < 3 && Me.CurrentTarget.HealthPercent <= 10,
                 new PrioritySelector(
                   new Decorator(ret => Me.IsCasting,
                       new Action(ctx => SpellManager.StopCasting())),
                   CreateSpellCheckAndCast("Drain Soul")
                   )),


                   ///Same as Above Here. (!ISFTimer.IsRunning || PetTimerCombat.Elapsed.Seconds > 5)
           new Decorator(ret => ZerfallSettings.Instance.PetSpell != "No Pet" && !Me.Mounted && !Me.IsFlying && !Me.IsOnTransport && !Me.GotAlivePet && PetTimerCombat.Elapsed.Seconds >= 5,
                 new PrioritySelector(
                   CreateSpellCheckAndCast("Soulburn", ret => Me.Combat && Me.CurrentSoulShards > 0 && SpellManager.HasSpell("Soulburn")),
                   SummonPet("Summon " + CurrentPetSpell + ""))),




                //PVP SpecSwitch
                new Decorator(ret => IsBattleGround(),
                          new Switch<WarlockTalentSpec>(r => _talentManager.Spec,
                                   new SwitchArgument<WarlockTalentSpec>(Low_rotation(), WarlockTalentSpec.Lowbie),
                                   new SwitchArgument<WarlockTalentSpec>(Affliction_PVP(), WarlockTalentSpec.Affliction),
                                   new SwitchArgument<WarlockTalentSpec>(Demo_PVP(), WarlockTalentSpec.Demonology),
                                   new SwitchArgument<WarlockTalentSpec>(Destro_PVP(), WarlockTalentSpec.Destruction)
                                   )),
                //Instance SpecSwitch
                new Decorator(ret => Me.IsInInstance && Me.IsInParty,
                          new Switch<WarlockTalentSpec>(r => _talentManager.Spec,
                                   new SwitchArgument<WarlockTalentSpec>(Low_rotation(), WarlockTalentSpec.Lowbie),
                                   new SwitchArgument<WarlockTalentSpec>(Affliction_Instance(), WarlockTalentSpec.Affliction),
                                   new SwitchArgument<WarlockTalentSpec>(Demo_Instance(), WarlockTalentSpec.Demonology),
                                   new SwitchArgument<WarlockTalentSpec>(Destro_Instance(), WarlockTalentSpec.Destruction)
                                   )),
                //PVE SpecSwitch if other two fail to be used. 
                new Switch<WarlockTalentSpec>(r => _talentManager.Spec,
                                   new SwitchArgument<WarlockTalentSpec>(Low_rotation(), WarlockTalentSpec.Lowbie),
                                   new SwitchArgument<WarlockTalentSpec>(Affliction_PVE(), WarlockTalentSpec.Affliction),
                                   new SwitchArgument<WarlockTalentSpec>(Demo_PVE(), WarlockTalentSpec.Demonology),
                                   new SwitchArgument<WarlockTalentSpec>(Destro_PVE(), WarlockTalentSpec.Destruction)
                                   )

              /*new Switch<WarlockTalentSpec>(r => _talentManager.Spec,
                                   new SwitchArgument<WarlockTalentSpec>(Low_rotation(), WarlockTalentSpec.Lowbie),
                                   new SwitchArgument<WarlockTalentSpec>(Affliction_rotation(), WarlockTalentSpec.Affliction),
                                   new SwitchArgument<WarlockTalentSpec>(Demo_rotation(), WarlockTalentSpec.Demonology),
                                   new SwitchArgument<WarlockTalentSpec>(Destro_rotation(), WarlockTalentSpec.Destruction)
                                   )*/
                                    );
        }
    }
}
