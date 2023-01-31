using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace UltimatePaladinHealerBT
{
    public partial class UltimatePalaHealerBT
    {

        public Composite Composite_Priest_Rest_Selector()
        {
            //return new PrioritySelector(
            slog("sono dentro composite selector");
            return 
                new PrioritySelector(
                new Decorator(ret => !unitcheck(Me) && !Me.IsGhost, new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Failure;
                })),
                new Decorator(ret=>unitcheck(Me) || Me.IsGhost,
                    new PrioritySelector(
                        new Action(
                        delegate
                        {
                            if (!Me.Combat) { General_we_are_in_combat = false; }
                            else { General_we_are_in_combat = true; }
                            if (General_we_were_in_combat == true && General_we_are_in_combat==false)
                            {
                                slog("Exiting Combat now!");
                            }
                            if (!Me.Combat) { General_we_were_in_combat = false; }
                            else { General_we_were_in_combat = true; }
                            return RunStatus.Failure;
                        }
            ),
                new Switch<string>(ret => usedBehaviour,
                new Action(delegate
                {
                    slog("No good behaviour found, the CC will stop now!");
                    return RunStatus.Failure;
                }),
                new SwitchArgument<string>("Ghost",
                Composite_Universal_Ghost()
                ),
                new SwitchArgument<string>("Solo",
                Composite_Priest_PVERest()
                ),
                new SwitchArgument<string>("Party or Raid",
                Composite_Priest_PVERest()
                ),
                new SwitchArgument<string>("Arena",
                Composite_Priest_PVPRest()
                ),
                new SwitchArgument<string>("World PVP",
                Composite_Priest_PVPRest()
                ),
                new SwitchArgument<string>("Battleground",
                Composite_Priest_PVPRest()
                ),
                new SwitchArgument<string>("Dungeon",
                Composite_Priest_PVERest()
                ),
                new SwitchArgument<string>("Raid",
                Composite_Priest_PVERest()
                )
                ))
                )
          );
        }
        public Composite Composite_Priest_Selector()
        {
            //return new PrioritySelector(
            slog("sono dentro composite selector");
            return 
                new PrioritySelector(
                new Decorator(ret => !unitcheck(Me) && !Me.IsGhost, new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Failure;
                })),
                new Decorator(ret=>unitcheck(Me) || Me.IsGhost,
                new Switch<string>(ret => usedBehaviour,
                new Action(delegate
                {
                    slog("No good behaviour found, the CC will stop now!");
                    return RunStatus.Failure;
                }),
                new SwitchArgument<string>("Solo",
                Composite_Priest_Solo()
                ),
                new SwitchArgument<string>("Arena",
                Composite_Priest_Arena()
                ),
                new SwitchArgument<string>("World PVP",
                Composite_Priest_WorldPVP()
                ),
                new SwitchArgument<string>("Party or Raid",
                Composite_Priest_Party_or_Raid()
                ),
                new SwitchArgument<string>("Ghost",
                Composite_Universal_Ghost()
                ),
                new SwitchArgument<string>("Battleground",
                Composite_Priest_Battleground()
                ),/*
                new SwitchArgument<string>("Solo",
                Composite_Priest_Dungeon()
                ),*/
                new SwitchArgument<string>("Dungeon",
                Composite_Priest_Dungeon()
                ),
                new SwitchArgument<string>("Raid",
                Composite_Priest_Raid()
                )
))
          );
        }

        public Composite Composite_Priest_Pull_Selector()
        {
            slog("sono dentro composite Pull selector");
            return new PrioritySelector(
                new Decorator(ret => !unitcheck(Me) && !Me.IsGhost, new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Failure;
                })),
                new Decorator(ret => unitcheck(Me) || Me.IsGhost,
                    new PrioritySelector(
                            new Decorator(ret => !_enable_pull || (_enable_pull && usedBehaviour != "Solo"),
                new Switch<string>(ret => usedBehaviour,
                new Action(delegate
                {
                    slog("No good behaviour found, the CC will stop now!");
                    return RunStatus.Failure;
                }),
                new SwitchArgument<string>("Ghost",
                Composite_Universal_Ghost()
                ),
                new SwitchArgument<string>("Solo",
                Composite_Priest_Solo()
                ),
                new SwitchArgument<string>("Party or Raid",
                Composite_Priest_Party_or_Raid()
                ),
                new SwitchArgument<string>("Arena",
                Composite_Priest_Arena()
                ),
                new SwitchArgument<string>("World PVP",
                Composite_Priest_WorldPVP()
                ),
                new SwitchArgument<string>("Battleground",
                Composite_Priest_Battleground()
                ),
                new SwitchArgument<string>("Dungeon",
                Composite_Priest_Dungeon()
                ),
                new SwitchArgument<string>("Raid",
                Composite_Priest_Raid()
                )
                )),
                new Decorator(ret => _enable_pull && usedBehaviour == "Solo",
                    Composite_Priest_Solo_Pull()
                    )))
          );
        }
        public Composite Composite_Priest_Arena()
        {
            return (
    new PrioritySelector(
        Composite_Priest_CheckMe(),
        Composite_Arena_SetTank(),
        Composite_Find_Battleground_unit_to_heal(),
        Composite_print_tar(),
        Composite_Find_Battleground_Enemy(),
        Conposite_Find_Arena_pet(),
        Composite_inizialize_subgroup(),
        Composite_Set_Focus(),
                            new Decorator(ret => Dismount(),
            new PrioritySelector(
        Composite_Priest_Self(),
        Composite_PVPRacials(),
                //new Decorator(ret => _wanna_taunt, Composite_Taunt()),    Shackle?

        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVPCleanseNow()),

        Composite_Priest_PVPHealing(),
        Composite_Priest_Mana_Rec(),
        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_PVPCleansing()),

        Composite_Priest_TopOff()
        )))
);
        }

        public Composite Composite_Priest_WorldPVP()
        {
            return (
                new PrioritySelector(
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_WorldPVP_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Find_WorldPVP_Enemy(),
                    Composite_inizialize_subgroup(),
                    Composite_Set_Focus(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Priest_Self(),
                    Composite_PVPRacials(),
        
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVPCleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_Priest_PVPHealing(),
                    Composite_Priest_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_PVPCleansing()),
                    Composite_Priest_TopOff()
                    ))
                    )
                    );
        }

        public Composite Composite_Priest_Party_or_Raid()
        {
            return(
                new PrioritySelector(

                        new Decorator(ret => !Me.IsInParty && !Me.IsInRaid,
                            new Action(
                                delegate
                                {
                                    CreateBehaviors();
                                    return RunStatus.Success;
                                }
            )),
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_RAF_Find_Enemy(),
                    Composite_Set_Focus(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Priest_Interrupt(),

                    Composite_Priest_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
                    Composite_Priest_PVEHealing(),
                    Composite_Priest_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
                    Composite_Priest_TopOff()
                 ))
                    )
                );
        }

        public Composite Composite_Priest_Battleground()
        {
            return (
    new PrioritySelector(
        Composite_Priest_CheckMe(),
        Composite_SetTank(),
        Composite_Find_Battleground_unit_to_heal(),
        Composite_Priorize_tank_healing(),
        Composite_print_tar(),
        Composite_Find_Battleground_Enemy(),
        Composite_inizialize_subgroup(),
        Composite_Set_Focus(),
                            new Decorator(ret => Dismount(),
            new PrioritySelector(
        Composite_Priest_Self(),
        Composite_PVPRacials(),
        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVPCleanseNow()),
        Composite_Priest_PVPHealing(),
        Composite_Priest_Mana_Rec(),
        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_PVPCleansing()),
        Composite_Priest_TopOff()
        )))
);
        }

        public Composite Composite_Priest_Solo()
        {
            return (
                                new PrioritySelector(
                  new Decorator(ret => Me.IsInParty || Me.IsInRaid,
                            new Action(
                                delegate
                                {
                                    CreateBehaviors();
                                    return RunStatus.Success;
                                }
            )),
               
                    Composite_Priest_CheckMe(),
                    Composite_Solo_SetTank(),
                    Composite_Solo_Find_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Solo_Find_Enemy(),
                    Composite_Set_Focus(),
                    new Decorator(ret => Dismount(),
                    new PrioritySelector(
                        Composite_Priest_PVEBuff(),
                        Composite_Priest_Self(),
                        Composite_PVERacials(),
                        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
                        new Decorator(ret => _wanna_move, Composite_Solomove()),
                        new Decorator(ret => Me.Combat, Composite_Priest_Dps()),
                        Composite_Priest_PVEHealing(),
                        Composite_Priest_Mana_Rec(),
                        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
                        new Decorator(ret => Me.Combat, Composite_Priest_Solo_Combat()),
                        Composite_Priest_TopOff()
                 )))
            );
        }

        public Composite Composite_Priest_Dungeon()
        {
            // slog("sono dentro Dungeon");
            return (
                new PrioritySelector(
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    Composite_Set_Focus(),
                    new Decorator(ret => Dismount(),
                    new PrioritySelector(
                        Composite_Priest_Interrupt(),
                        Composite_Priest_Self(),
                        Composite_PVERacials(),
                //        new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
                //        new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                        Composite_Priest_PVEHealing(),
                        //,
                        Composite_Priest_Mana_Rec(),
                        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
                        Composite_Priest_TopOff()
                //        new Decorator(ret => Me.Combat, Composite_Dps()),
                //        new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                 )))
            );
        }

        public Composite Composite_Priest_PVPRest()
        {
            return (
                new PrioritySelector(
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Battleground_unit_to_heal(),
                    Composite_Priest_NeedtoRest(),
                    Composite_Set_Focus(),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVPCleanseNow()),
                    Composite_Priest_PVPHealing(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_PVPCleansing()),
                    Composite_Priest_TopOff(),
                    new Decorator(ret => _wanna_buff, Composite_Priest_PVEBuff()),
                    new Decorator(ret => _wanna_mount, Composite_MountUp())))
        )
        );
        }

        public Composite Composite_Priest_PVERest()
        {
            return (
                
    new PrioritySelector(
        Composite_Priest_CheckMe(),
        Composite_SetTank(),
        Composite_Priest_NeedtoRest(),
        Composite_Find_unit_to_heal(),
        Composite_Set_Focus(),
        new Decorator(ret => _wanna_mount, Composite_MountUp()),
        new Decorator(ret => Dismount(),
            new PrioritySelector(
                Composite_Priest_Interrupt(),
        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
        Composite_Priest_PVEHealing(),
        //,
        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
        Composite_Priest_TopOff(),
        new Decorator(ret => _wanna_buff, Composite_Priest_PVEBuff())
        //Composite_Crusader()

        )
        )));
        }

        public Composite Composite_Priest_Solo_Pull()
        {
            return (
                new Decorator(ret => !Me.Combat,
                new PrioritySelector(
                // Clear target and return failure if it's tagged by someone else
                new Decorator(ret => !Me.IsInParty && Me.GotTarget && Me.CurrentTarget.TaggedByOther,
                              new Action(delegate
                              {
                                  SpellManager.StopCasting();
                                  slog("Current target is not tagged by me, Aborting pull!");
                                  Blacklist.Add(Me.CurrentTarget, TimeSpan.FromMinutes(30));
                                  Me.ClearTarget();
                                  return RunStatus.Failure;
                              })
                    ),
                // Blacklist target's we can't move to
                new Decorator(ret => !Me.IsInInstance && Navigator.GeneratePath(Me.Location, Me.CurrentTarget.Location).Length <= 0,
                              new Action(delegate
                              {
                                  Blacklist.Add(Me.CurrentTargetGuid, TimeSpan.FromDays(365));
                                  slog("Failed to generate path to: {0} blacklisted!",
                                      privacyname(Me.CurrentTarget));
                                  return RunStatus.Success;
                              })
                    ),
                // Move closer to the target if we are too far away or in !Los
                new Decorator(ret => _wanna_move && Me.GotTarget && (Me.CurrentTarget.Distance > PullDistance - 1 || !Me.CurrentTarget.InLineOfSight),
                              new Action(delegate
                              {
                                  slog("Moving towards:{0}", privacyname(Me.CurrentTarget));
                                  Navigator.MoveTo(Me.CurrentTarget.Location);
                              })),
                // Stop moving if we are moving
                new Decorator(ret => _wanna_move && Me.IsMoving,
                              new Action(ret => WoWMovement.MoveStop())),

                new Decorator(ret => _wanna_move && Me.GotTarget && !Me.IsFacing(Me.CurrentTarget),
                              new Action(ret => WoWMovement.Face())
                    ),
                    new Decorator(ret => unitcheck(Me.CurrentTarget) && !Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.IsPlayer && !Me.CurrentTarget.IsPet,
                    new Action(
                        delegate
                        {
                            Enemy = Me.CurrentTarget;
                            slog("Checks Done, all green, PULLING {0} NOW!", privacyname(Enemy));
                            return RunStatus.Failure;
                        })),
                        Composite_Priest_Solo_Combat())
                    )
                    );
        }
        public Composite Composite_Priest_Dps()
        {
            return (
                new Decorator(ret => _wanna_SWD && IsSpellReady("Shadow Word: Death") && Me.Combat && unitcheck(Enemy) && Enemy.Distance < 40 && Enemy.InLineOfSight && Enemy.HealthPercent < 25 && Me.HealthPercent>25,
                    new PrioritySelector(
                        new Decorator(ret => _wanna_face,
                            new Action(
                                delegate
                                {
                                    WoWMovement.Face();
                                    return RunStatus.Failure;
                                }
            )),
            new Wait(4, ret => !IsCastingOrGCD(),
                new Action(
                 delegate
                 {
                     if (Cast("Shadow Word: Death", Enemy, 40, "DPS", "Enemy low on health"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         )))));
        }

        public Composite Composite_Priest_Solo_Combat()
        {
            return (
                new Decorator(ret => unitcheck(Enemy),
                    new PrioritySelector(
                                 new Decorator(ret => IsSpellReady("Holy Word: Chastise") && Enemy.Distance < 30 && Enemy.InLineOfSight && Me.HasAura("Chakra: Chastise"),
              new Wait(4, ret => !IsCastingOrGCD(),
                  new Action(
                        delegate
                        {
                            if (_wanna_face) { WoWMovement.Face(); }
                            if (Cast("Holy Word: Chastise", Enemy, 30, "DPS", "Solo, we need to kill"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
         ))),
         new Decorator(ret => IsSpellReady("Holy Fire") && Enemy.Distance < 30 && Enemy.InLineOfSight,
              new Wait(4, ret => !IsCastingOrGCD(),
                  new Action(
                        delegate
                        {
                            if (_wanna_face) { WoWMovement.Face(); }
                            if (Cast("Holy Fire", Enemy, 30, "DPS", "Solo, we need to kill"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
         ))),
         new Decorator(ret=> !Me.HasAura("Chakra: Chastise") && IsSpellReady("Chakra"),
             new PrioritySelector(
         new Decorator(ret => SpellManager.HasSpell("Chakra") && !SpellManager.Spells["Chakra"].Cooldown && !Me.HasAura("Chakra") && !Me.HasAura("Chakra: Chastise"),
                    new Wait(4, ret => !IsCastingOrGCD(),
                        new Action(
                            delegate
                            {
                                Cast("Chakra", "Buff", "Need Chakra buff");
                                return RunStatus.Failure;
                            }
                ))),
                new Wait (2, ret=>Me.HasAura("Chakra") ||Me.HasAura("Chakra: Chastise"),new Action(delegate{return RunStatus.Failure;})),
                new Decorator(ret=>IsSpellReady("Smite"),
         new Wait(4, ret => !IsCastingOrGCD(),
             new Action(
                        delegate
                        {
                            if (_wanna_face) { WoWMovement.Face(); }
                            if (Cast("Smite", Enemy, 30, "DPS", "Solo, we need to kill"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
            ))
                        ))),
                        new Decorator(ret=>IsSpellReady("Smite"),
                                 new Wait(4, ret => !IsCastingOrGCD(),
             new Action(
                        delegate
                        {
                            if (_wanna_face) { WoWMovement.Face(); }
                            if (Cast("Smite", Enemy, 30, "DPS", "Solo, we need to kill"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
            ))
                        )))
                        );
        }
        public Composite Composite_Priest_Raid_AOE_Healing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => !GotBuff("Spirit of Redemption") && !Me.ActiveAuras.ContainsKey("Spirit of Redemption") && !Me.HasAura("Spirit of Redemption"),
                        new PrioritySelector(
                    new Decorator(ret => tank != null && tank.IsValid, Composite_Priest_Healing_check_and_buff()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Emergency_button()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Actual_AOE_Healing())
                    )),
                    new Decorator(ret => GotBuff("Spirit of Redemption") || Me.ActiveAuras.ContainsKey("Spirit of Redemption") || Me.HasAura("Spirit of Redemption"),
                        new PrioritySelector(
                        new Action(delegate { slog("Gotbuff {0} activeauras {1} hasuara {2}", GotBuff("Spirit of Redemption"), Me.ActiveAuras.ContainsKey("Spirit of Redemption"), Me.HasAura("Spirit of Redemption")); return RunStatus.Failure; }),
                        new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Emergency_button()),
                        new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Spirit_of_Redemption_Healing())
                    )
                )));
        }
        public Composite Composite_Priest_PVEHealing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => !GotBuff("Spirit of Redemption") && !Me.ActiveAuras.ContainsKey("Spirit of Redemption") && !Me.HasAura("Spirit of Redemption"),
                        new PrioritySelector(
                    new Decorator(ret => tank != null && tank.IsValid, Composite_Priest_Healing_check_and_buff()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Emergency_button()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Actual_Healing())
                    )),
                    new Decorator(ret => GotBuff("Spirit of Redemption") || Me.ActiveAuras.ContainsKey("Spirit of Redemption") || Me.HasAura("Spirit of Redemption"),
                        new PrioritySelector(
                        new Action(delegate { slog("Gotbuff {0} activeauras {1} hasuara {2}", GotBuff("Spirit of Redemption"), Me.ActiveAuras.ContainsKey("Spirit of Redemption"), Me.HasAura("Spirit of Redemption")); return RunStatus.Failure; }),
                        new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Emergency_button()),
                        new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Spirit_of_Redemption_Healing())
                    )
                )));

        }

        public Composite Composite_Priest_PVPHealing()
        {
            return (
            new PrioritySelector(
                new Decorator(ret => !GotBuff("Spirit of Redemption") && !Me.ActiveAuras.ContainsKey("Spirit of Redemption") && !Me.HasAura("Spirit of Redemption"),
                    new PrioritySelector(
                new Decorator(ret => tank != null && tank.IsValid, Composite_Priest_Healing_check_and_buff()),
                new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Emergency_button()),
                new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Actual_PVP_Healing())
                )),
                new Decorator(ret => GotBuff("Spirit of Redemption") || Me.ActiveAuras.ContainsKey("Spirit of Redemption") || Me.HasAura("Spirit of Redemption"),
                    new PrioritySelector(
                    new Action(delegate { slog("Gotbuff {0} activeauras {1} hasuara {2}", GotBuff("Spirit of Redemption"), Me.ActiveAuras.ContainsKey("Spirit of Redemption"), Me.HasAura("Spirit of Redemption")); return RunStatus.Failure; }),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Priest_Emergency_button()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Spirit_of_Redemption_Healing())
                )
            )));
        }

        public Composite Composite_Priest_Emergency_button()
        {
            return (
                    new PrioritySelector(
                        new Decorator(ret => Me.Combat && tar.HealthPercent < _min_ohshitbutton_activator,
                            new PrioritySelector(
                                new Action(
                                    delegate{
                                        slog("Oh Shit Button Called!");
                                        return RunStatus.Failure;
                                    }),
                                    Composite_Priest_PVEOhShitButton()
                                    )),
                                    new Decorator(ret=>IsSpellReady("Divine Hymm") && Should_AOE(Me, "Divine Hymm",_min_player_inside_DY, 30,_DY_how_much_health) && !Me.IsMoving,
                                        new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                            {
                                if (Cast("Divine Hymm", Me, "Heal", "Saving someone life"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                                    ));
        }
        public Composite Composite_Priest_PVEOhShitButton()
        {
            return (
                new Decorator(ret => !GotBuff("Guardian Spirit", tar) && !GotBuff("Lifeblood"),
                    new PrioritySelector(
                        new Action(
                            delegate
                            {
                                slog("CC LB {0} CC GS {1}", CanCast("Lifeblood"), CanCast("Guardian Spirit"));
                                return RunStatus.Failure;
                            }),
                        new Decorator(ret => _wanna_lifeblood && IsSpellReady("Lifeblood"),
                            new Wait(4, ret => !IsCastingOrGCD(), 
                                new Action(delegate
                            {
                                if (Cast("Lifeblood", "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_GS && IsSpellReady("Guardian Spirit") && unitcheck(tar) && unitcheck(tank) && tank==tar,
                            new Wait(4, ret => !IsCastingOrGCD(), 
                                new Action(delegate
                            {
                                if (Cast("Guardian Spirit", tar, 40, "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                            )));
        }
        public Composite Composite_Priest_Ensure_Chakra()
        {
            return(
                new Decorator(ret=>usedBehaviour!="Solo",
                new PrioritySelector(
                new Decorator(ret => IsSpellReady("Chakra") && !Me.HasAura("Chakra") && !Me.HasAura("Chakra: Serenity") && !Me.HasAura("Chakra: Sanctuary") && !Me.HasAura("Chakra: Chastise"),
                    new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate{
                                if (Cast("Chakra", "Buff", "Need Chakra buff"))
                                {return RunStatus.Success;}
                                else{return RunStatus.Failure;}
                            }
                ))),
                new Decorator(ret=>SpellManager.HasSpell("Chakra") &&Me.HasAura("Chakra") && !_raid_healer,
                        new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate{
                                if (Cast("Flash Heal", Me, 40, "Heal", "Healing"))
                                        { return RunStatus.Success; }
                                        else { return RunStatus.Failure; }
                            }
                            ))),
                            new Decorator(ret => SpellManager.HasSpell("Chakra") && Me.HasAura("Chakra") && _raid_healer,
                        new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate
                            {
                                if (Cast("Prayer of Healing", Me, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }
                            )))
                )));
        }
        public Composite Composite_Priest_Healing_check_and_buff()
        {
            return (
                new PrioritySelector(
                        new Decorator(ret => tank != null && Me.Mounted && !Me.Combat && !tank.Combat, new Action()),
                        Composite_Priest_Ensure_Chakra(),
                        new Decorator(ret => !Me.HasAura("Inner Fire") && !Me.Mounted && IsSpellReady("Inner Fire"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Inner Fire", "Buff", "Missing Inner Fire"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                            new Decorator(ret => unitcheck(tar),
                                new PrioritySelector(
                                    new Decorator(ret => tank != null && !Me.Combat && !tar.Combat && tar.Distance > _max_healing_distance, new Action()),
                                    new Decorator(ret => _wanna_move_to_heal && (tar.Distance > 30 || !tar.InLineOfSight),
                            new Action(
                                delegate
                                {
                                    slog("Healing target is too far away or not in LOS, moving to them!");
                                    if (MoveTo(tar)) { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }
                            )
                        )))
                            ));
        }
        public Composite Composite_Priest_Interrupt()
        {
            return (new PrioritySelector(
                new Decorator(ret => Me.IsCasting && lastCast != null && lastCast.IsValid && !lastCast.Dead && lastCast.HealthPercent >= Math.Max(Math.Max(_stop_GH_if_above, _GH_min_hp), _Serendipity_GH_min_hp) && Me.CastingSpell.Name == "Greater Heal",
            new Action(
                delegate
                {
                    SpellManager.StopCasting();
                    if (lastCast != null)
                    {
                        slog(Color.Brown, "Interrupting Healing, target {0} at {1} %", privacyname(lastCast), Round(lastCast.HealthPercent));
                    }
                }
            )),
            new Decorator(ret => Me.IsCasting && lastCast != null && lastCast.IsValid && !lastCast.Dead && lastCast.HealthPercent >= _do_not_heal_above && Me.CastingSpell.Name == "Heal",
                new Action(
                delegate
                {
                    SpellManager.StopCasting();
                    if (lastCast != null)
                    {
                        slog(Color.Brown, "Interrupting Healing, target {0} at {1} %", privacyname(lastCast), Round(lastCast.HealthPercent));
                    }
                }
            )),
            new Decorator(ret => Me.IsCasting && Me.CastingSpell.Name == "Hymm of Hope",
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            tar = GetHealTarget();
                            return RunStatus.Failure;
                        }
                ),
                new Decorator(ret=>unitcheck(tar) && tar.HealthPercent<_interrupt_HoH_if_someone_below,
                    new Action(
                delegate
                {
                    SpellManager.StopCasting();
                    if (lastCast != null)
                    {
                        slog(Color.Brown, "Interrupting Hymm of Hope People need to be healed!, target {0} at {1} %", privacyname(tar), Round(tar.HealthPercent));
                    }
                }
            ))
            ))));
        }
        public Composite Composite_Spirit_of_Redemption_Healing()
        {
                return (
                new PrioritySelector(
                    new Action(delegate
            {
                slog("Spirit of Redemption Healing Called! We are dead aren't we? will try to heal tar {0} at hp {1} and distance {2} me.ismoving {3} cancastGH {4} cancastFH {5} cancastH {6}  Global cooldown {7} left {8}", privacyname(tar), Round(tar.HealthPercent), Round(tar.Distance), Me.IsMoving, CanCast("Greater Heal", tar), CanCast("Flash Heal", tar), CanCast("Heal", tar), SpellManager.GlobalCooldown, SpellManager.GlobalCooldownLeft);
                if (tar == Me) { slog("I'm in Spirit of Redemption, I should never be the Target! Crap there is needed a logic change, PLS report back!"); }
                return RunStatus.Failure;
            }
            ),
            new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar),
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Flash Heal") && GotBuff("Surge of Light"),
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Flash Heal", tar, 40, "Healing", "Got Surge of Light!"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),

                                                 new Decorator(ret => IsSpellReady("Circle of Healing") && Should_AOE("Circle of Healing", _min_player_inside_CoH, 30, _CoH_how_much_health, out x),
                                                     new Decorator(ret=>unitcheck(x) && x!=Me&& x.Distance<_max_healing_distance && x.InLineOfSight,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Circle of Healing", x, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))),
                                            new Decorator(ret => IsSpellReady("Flash Heal") && GetAuraStackCount(Me, "Serendipity") < 2,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Flash Heal", tar, 40, "Buff", "Need Serendipity Buff"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Greater Heal") && GetAuraStackCount(Me, "Serendipity") == 2,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Greater Heal", tar, 40, "Heal", "Healing with Serendipity"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                            new Action(
                                delegate
                                {
                                    slog("Spirit of Redemption, no good healing spell found, moving on");
                                    return RunStatus.Failure;
                                }
                                )
                                            ));
        }

        public Composite Composite_Priest_Actual_PVP_Healing()
        {
            return (
    new PrioritySelector(
        new Action(delegate
        {
            slog("Actual healing called! will try to heal tar {0} at hp {1} and distance {2} me.ismoving {3} cancastGH {4} cancastFH {5} cancastH {6}  Global cooldown {7} left {8}", privacyname(tar), Round(tar.HealthPercent), Round(tar.Distance), Me.IsMoving, CanCast("Greater Heal", tar), CanCast("Flash Heal", tar), CanCast("Heal", tar), SpellManager.GlobalCooldown, SpellManager.GlobalCooldownLeft);
            return RunStatus.Failure;
        }
),
new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar) && tar == tank,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => _use_PoM_on_CD && _wanna_PoM && unitcheck(tank) && IsSpellReady("Prayer of Mending") && !GotBuff("Prayer of Mending", tank) && !Me.Mounted && tank != Me && tank.Distance < _max_healing_distance && tank.InLineOfSight,
        new PrioritySelector( Composite_Wait_again(), 
                    new Action(delegate
                    {
                        if (Cast("Prayer of Mending", tank, 40, "Buff", "Missing Prayer of Mending"))
                        { return RunStatus.Success; }
                        else { return RunStatus.Failure; }
                    }))),
                    new Decorator(ret => !_use_PoM_on_CD && _wanna_PoM, Composite_Prayer_of_Mending()),
                                            new Decorator(ret=>_wanna_CoH, Composite_CoH()),
                                            new Decorator(ret=>_wanna_PoH, Composite_PoH()),
                                new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar) && tar.HealthPercent < _min_renew_hp_on_non_tank,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),

                                new Decorator(ret => !_raid_healer && SpellManager.HasSpell("Holy Word: Chastise") && !WoWSpell.FromId(88684).Cooldown && tar.HealthPercent < _HWS_min_hp && Me.HasAura("Chakra: Serenity")
                                                && tar != null && tar.IsValid && !tar.Dead && tar.InLineOfSight && tar.Distance < 40,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                formatslog("Heal", "Healing", "Holy Word: Serenity", tar);
                                                if (SpellManager.Cast(WoWSpell.FromId(88684), tar))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                new Decorator(ret => _raid_healer && SpellManager.HasSpell("Holy Word: Chastise") && !WoWSpell.FromId(88685).Cooldown && Me.HasAura("Chakra: Sanctuary"),
                                    new PrioritySelector(
                                        new Decorator(ret => _AOE_check_everyone,
                                     new Decorator(ret => Should_AOE(_min_player_inside_HWSa, 30, _HWSa_how_much_health, out x),
                                         new Decorator(ret => unitcheck(x) && x.Distance < _max_healing_distance && x.InLineOfSight,
    new PrioritySelector( Composite_Wait_again(), 
        new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", x);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), x))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa=true; }
                                    else { General_casted_HWSa=false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret=>General_casted_HWSa,
                                    new Wait(1,ret=>Me.CurrentPendingCursorSpell!=null,
                                        new Action(
                                            delegate{
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(x.Location);
                                            }
                                            )))
                                ))
                                ))),

                                new Decorator(ret => !_AOE_check_everyone && _AOE_check_focus,
                                            new Decorator(ret => unitcheck(focus) && focus.Distance < _max_healing_distance && focus.InLineOfSight && Should_AOE(focus, _min_player_inside_HWSa, 30, _HWSa_how_much_health),
                                        new PrioritySelector( Composite_Wait_again(), 
        new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", focus);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), focus))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa=true; }
                                    else { General_casted_HWSa=false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret=>General_casted_HWSa,
                                    new Wait(1,ret=>Me.CurrentPendingCursorSpell!=null,
                                        new Action(
                                            delegate{
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(focus.Location);
                                            }
                                            )))
                                )))
                                ),
                                new Decorator(ret => !_AOE_check_everyone && _AOE_check_tar,
                                    new Decorator(ret => Should_AOE(tar, _min_player_inside_HWSa, 30, _HWSa_how_much_health) && tar.Distance < _max_healing_distance && tar.InLineOfSight,
                                        new PrioritySelector( Composite_Wait_again(), 
        new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", tar);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), tar))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa=true; }
                                    else { General_casted_HWSa=false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret=>General_casted_HWSa,
                                    new Wait(1,ret=>Me.CurrentPendingCursorSpell!=null,
                                        new Action(
                                            delegate{
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(tar.Location);
                                            }
                                            ))
                                ))))),
                                new Decorator(ret => !_AOE_check_everyone && _AOE_check_tank,
                                    new Decorator(ret => Should_AOE(tank, _min_player_inside_HWSa, 30, _HWSa_how_much_health) && tank.Distance < _max_healing_distance && tank.InLineOfSight,
                                        new PrioritySelector( Composite_Wait_again(), 
                                        new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", tank);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), tank))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(tank.Location);
                                            }
                                            )))
                                )))),

                                new Decorator(ret => !_AOE_check_everyone && _AOE_check_self,
                                    new Decorator(ret => Should_AOE(Me, _min_player_inside_HWSa, 30, _HWSa_how_much_health),
                                        new PrioritySelector( Composite_Wait_again(), 
                                        new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", Me);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), Me))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(Me.Location);
                                            }
                                            )))
                                ))))
                                )),
                                new Decorator(ret => IsSpellReady("Binding Heal") && !Me.IsMoving && Me.HealthPercent < _binding_heal_min_hp && tar.HealthPercent < _binding_heal_min_hp && Me != tar,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Binding Heal", tar, 40, "Heal", "Healing"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => IsSpellReady("Binding Heal") && !Me.IsMoving && tar == Me && tank != Me && Me.HealthPercent < _binding_heal_min_hp && tank.HealthPercent < _Heal_min_hp,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Binding Heal", tank, 40, "Heal", "Healing"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => IsSpellReady("Binding Heal") && tar == Me && !Me.IsMoving && Me.HealthPercent < _binding_heal_min_hp && tank.HealthPercent > _Heal_min_hp,
                                    new PrioritySelector(
                                    new Action(
                                        delegate
                                        {
                                            x = GetBindingHealTarget();
                                            return RunStatus.Failure;
                                        }),
                                        new Decorator(ret => IsSpellReady("Binding Heal") && unitcheck(x) && x.HealthPercent < _binding_heal_min_hp && x!=Me,
                                            new PrioritySelector( Composite_Wait_again(), 
                                            new Action(
                                                delegate
                                                {
                                                    if (Cast("Binding Heal", x, 40, "Heal", "Healing"))
                                                    { return RunStatus.Success; }
                                                    else { return RunStatus.Failure; }
                                                }
))))),
                                new Decorator(ret => IsSpellReady("Flash Heal") && GotBuff("Surge of Light") && tar.HealthPercent < _SoL_min_hp,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Flash Heal", tar, 40, "Healing", "Got Surge of Light!"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => IsSpellReady("Flash Heal") && !Me.IsMoving && GetAuraStackCount(Me, "Serendipity") < 2 && tar.HealthPercent < _Serendipity_min_hp,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Flash Heal", tar, 40, "Buff", "Need Serendipity Buff"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => IsSpellReady("Greater Heal") && !Me.IsMoving && GetAuraStackCount(Me, "Serendipity") == 2 && tar.HealthPercent < _Serendipity_GH_min_hp,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Greater Heal", tar, 40, "Heal", "Healing with Serendipity"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                     new Decorator(ret => IsSpellReady("Greater Heal") && !Me.IsMoving && tar.HealthPercent < _GH_min_hp,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Greater Heal", tar, 40, "Heal", "Healing"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => _wanna_pom_in_combat, Composite_Prayer_of_Mending()),
                                new Decorator(ret => IsSpellReady("Heal") && !Me.IsMoving && tar.HealthPercent < _Heal_min_hp,
    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Heal", tar, 40, "Heal", "Healing"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                new Action(
                    delegate
                    {
                        slog("no good healing spell found, moving on");
                        return RunStatus.Failure;
                    }
                    )
));
        }

        public Composite Composite_Priest_Actual_AOE_Healing()
        {
            //Prayer of Healing
            return (
                new PrioritySelector(
                    new Action(delegate
                    {
                        slog("Actual healing called! will try to heal tar {0} at hp {1} and distance {2} me.ismoving {3} cancastGH {4} cancastFH {5} cancastH {6}  Global cooldown {7} left {8}", privacyname(tar), Round(tar.HealthPercent), Round(tar.Distance), Me.IsMoving, CanCast("Greater Heal", tar), CanCast("Flash Heal", tar), CanCast("Heal", tar), SpellManager.GlobalCooldown, SpellManager.GlobalCooldownLeft);
                        return RunStatus.Failure;
                    }
            ),
            new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar) && tar == tank,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret=> _use_PoM_on_CD && unitcheck(tank) && IsSpellReady("Prayer of Mending") && !GotBuff("Prayer of Mending", tank) && !Me.Mounted && tank != Me && tank.Distance < _max_healing_distance && tank.InLineOfSight,
                    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Prayer of Mending", tank, 40, "Buff", "Missing Prayer of Mending"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                    new Decorator(ret => !_use_PoM_on_CD && _wanna_PoM, Composite_Prayer_of_Mending()),
                                            new Decorator(ret => _wanna_CoH, Composite_CoH()),
                                            new Decorator(ret => _wanna_PoH, Composite_PoH()),

                                            new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar) && tar.HealthPercent < _min_renew_hp_on_non_tank,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => SpellManager.HasSpell("Holy Word: Chastise") && !WoWSpell.FromId(88685).Cooldown && Me.HasAura("Chakra: Sanctuary"),
                                                new PrioritySelector(
                                                    new Decorator(ret => _AOE_check_everyone,
                                                 new Decorator(ret => Should_AOE(_min_player_inside_HWSa, 30, _HWSa_how_much_health, out x),
                                                     new Decorator(ret => unitcheck(x) && x.Distance < _max_healing_distance && x.InLineOfSight,
                new PrioritySelector( Composite_Wait_again(), 
                                                    new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", x);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), x))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(x.Location);
                                            }
                                            )))
                                ))))),

                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_focus,

                                                        new Decorator(ret => unitcheck(focus) && focus.Distance<_max_healing_distance && focus.InLineOfSight && Should_AOE(focus, _min_player_inside_HWSa, 30, _HWSa_how_much_health),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                                    new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", focus);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), focus))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(focus.Location);
                                            }
                                            )))
                                )))
                                            ),
                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_tar,
                                                new Decorator(ret => Should_AOE(tar, _min_player_inside_HWSa, 30, _HWSa_how_much_health) && tar.Distance < _max_healing_distance && tar.InLineOfSight,
                                                    new PrioritySelector( Composite_Wait_again(), 
                                                    new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", tar);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), tar))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(tar.Location);
                                            }
                                            )))
                                )))),
                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_tank,
                                                new Decorator(ret => Should_AOE(tank, _min_player_inside_HWSa, 30, _HWSa_how_much_health) && tank.Distance<_max_healing_distance && tank.InLineOfSight,
                                                    new PrioritySelector( Composite_Wait_again(), 
                                                    new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", tank);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), tank))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(tank.Location);
                                            }
                                            )))
                                )))),

                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_self,
                                                new Decorator(ret => Should_AOE(Me, _min_player_inside_HWSa, 30, _HWSa_how_much_health),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                                    new PrioritySelector(
                                new Action(delegate
                                {
                                    formatslog("Heal", "Healing", "Holy Word: Sanctuary", Me);
                                    if (SpellManager.Cast(WoWSpell.FromId(88685), Me))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                    { General_casted_HWSa = true; }
                                    else { General_casted_HWSa = false; }
                                    return RunStatus.Failure;
                                }),
                                new Decorator(ret => General_casted_HWSa,
                                    new Wait(1, ret => Me.CurrentPendingCursorSpell != null,
                                        new Action(
                                            delegate
                                            {
                                                slog("Casting spell HWSa {0}", Me.CurrentPendingCursorSpell.Name);
                                                LegacySpellManager.ClickRemoteLocation(Me.Location);
                                            }
                                            )))
                                ))))
                                            )),
                                            new Decorator(ret => IsSpellReady("Binding Heal") && !Me.IsMoving && Me.HealthPercent < _binding_heal_min_hp && tar.HealthPercent < _binding_heal_min_hp && Me != tar,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Binding Heal", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Binding Heal") && !Me.IsMoving && tar == Me && Me.HealthPercent < _binding_heal_min_hp && tank.HealthPercent < _Heal_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Binding Heal", tank, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Binding Heal") && tar == Me && !Me.IsMoving && Me.HealthPercent < _binding_heal_min_hp && tank.HealthPercent > _Heal_min_hp,
                                                new PrioritySelector(
                                                new Action(
                                                    delegate
                                                    {
                                                        x = GetBindingHealTarget();
                                                        return RunStatus.Failure;
                                                    }),
                                                    new Decorator(ret => IsSpellReady("Binding Heal") && unitcheck(x) && x.HealthPercent < _binding_heal_min_hp,
                                                        new PrioritySelector( Composite_Wait_again(), 
                                                        new Action(
                                                            delegate
                                                            {
                                                                if (Cast("Binding Heal", x, 40, "Heal", "Healing"))
                                                                { return RunStatus.Success; }
                                                                else { return RunStatus.Failure; }
                                                            }
            ))))),
                                            new Decorator(ret => IsSpellReady("Flash Heal") && GotBuff("Surge of Light") && tar.HealthPercent < _SoL_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Flash Heal", tar, 40, "Healing", "Got Surge of Light!"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Flash Heal") && !Me.IsMoving && GetAuraStackCount(Me, "Serendipity") < 2 && tar.HealthPercent < _Serendipity_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Flash Heal", tar, 40, "Buff", "Need Serendipity Buff"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Greater Heal") && !Me.IsMoving && GetAuraStackCount(Me, "Serendipity") == 2 && tar.HealthPercent < _Serendipity_GH_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Greater Heal", tar, 40, "Heal", "Healing with Serendipity"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                                 new Decorator(ret => IsSpellReady("Greater Heal") && !Me.IsMoving && tar.HealthPercent < _GH_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Greater Heal", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => _wanna_pom_in_combat, Composite_Prayer_of_Mending()),
                                            new Decorator(ret => IsSpellReady("Heal") && !Me.IsMoving && tar.HealthPercent < _Heal_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Heal", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                            new Action(
                                delegate
                                {
                                    slog("no good healing spell found, moving on");
                                    return RunStatus.Failure;
                                }
                                )
            ));
        }
        public Composite Composite_Priest_Actual_Healing()
        {
            return (
                new PrioritySelector(
                    new Action(delegate
            {
                slog("Actual healing called! will try to heal tar {0} at hp {1} and distance {2} me.ismoving {3} cancastGH {4} cancastFH {5} cancastH {6}  Global cooldown {7} left {8}", privacyname(tar), Round(tar.HealthPercent), Round(tar.Distance), Me.IsMoving, CanCast("Greater Heal", tar), CanCast("Flash Heal", tar), CanCast("Heal", tar), SpellManager.GlobalCooldown, SpellManager.GlobalCooldownLeft);
                return RunStatus.Failure;
            }
            ),
            new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar) && tar == tank,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret=> tar==tank,Composite_Prayer_of_Mending()),
                                            new Decorator(ret => IsSpellReady("Renew") && !GotBuff("Renew", tar) && tar.HealthPercent < _min_renew_hp_on_non_tank,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Renew", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => SpellManager.HasSpell("Holy Word: Chastise") && !WoWSpell.FromId(88684).Cooldown && tar.HealthPercent < _HWS_min_hp && Me.HasAura("Chakra: Serenity")
                                                && tar != null && tar.IsValid && !tar.Dead && tar.InLineOfSight && tar.Distance <40,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                formatslog("Heal", "Healing", "Holy Word: Serenity", tar);
                                                if (SpellManager.Cast(WoWSpell.FromId(88684), tar))//Cast("Holy Word: Serenity", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Binding Heal") && !Me.IsMoving && Me.HealthPercent < _binding_heal_min_hp && tar.HealthPercent < _binding_heal_min_hp && Me != tar,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Binding Heal", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Binding Heal") && !Me.IsMoving && tar == Me && Me.HealthPercent < _binding_heal_min_hp && tank.HealthPercent < _Heal_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Binding Heal", tank, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Binding Heal") && tar == Me && !Me.IsMoving && Me.HealthPercent < _binding_heal_min_hp && tank.HealthPercent > _Heal_min_hp,
                                                new PrioritySelector(
                                                new Action(
                                                    delegate{
                                                        x=GetBindingHealTarget();
                                                        return RunStatus.Failure;
                                                    }),
                                                    new Decorator(ret=>IsSpellReady("Binding Heal") && unitcheck(x) && x.HealthPercent<_binding_heal_min_hp,
                                                        new PrioritySelector( Composite_Wait_again(), 
                                                        new Action(
                                                            delegate{
                                                                if (Cast("Binding Heal", x, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                                            }
            ))))),
                                            new Decorator(ret => IsSpellReady("Flash Heal") && GotBuff("Surge of Light") && tar.HealthPercent < _SoL_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Flash Heal", tar, 40, "Healing", "Got Surge of Light!"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Flash Heal") && !Me.IsMoving && GetAuraStackCount(Me, "Serendipity") < 2 && tar.HealthPercent < _Serendipity_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Flash Heal", tar, 40, "Buff", "Need Serendipity Buff"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret => IsSpellReady("Greater Heal") && !Me.IsMoving && GetAuraStackCount(Me, "Serendipity") == 2 && tar.HealthPercent < _Serendipity_GH_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Greater Heal", tar, 40, "Heal", "Healing with Serendipity"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                   // new Decorator(ret => !_use_PoM_on_CD && _wanna_PoM, Composite_Prayer_of_Mending()),
                                            new Decorator(ret => _wanna_CoH, Composite_CoH()),
                                            new Decorator(ret => _wanna_PoH, Composite_PoH()),
                                                 new Decorator(ret => IsSpellReady("Greater Heal") && !Me.IsMoving && tar.HealthPercent < _GH_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Greater Heal", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                                            new Decorator(ret=>_wanna_pom_in_combat, Composite_Prayer_of_Mending()),
                                            new Decorator(ret => IsSpellReady("Heal") && !Me.IsMoving && tar.HealthPercent < _Heal_min_hp,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Heal", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))),
                            new Action(
                                delegate
                                {
                                    slog("no good healing spell found, moving on");
                                    return RunStatus.Failure;
                                }
                                )
            ));
        }

        public Composite Priest_Resurrecting()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            RessTarget = GetResTarget();
                            return RunStatus.Failure;
                        }
            ), new Decorator(ret => RessTarget != null && RessTarget.IsValid,
                new PrioritySelector(
                    new Decorator(ret => RessTarget.Distance > 29 || !RessTarget.InLineOfSight,
                        new Action(
                            delegate
                            {
                                slog("I'd want to ress {0} at distance {1} but is too far away or out of loss", privacyname(RessTarget), Round(RessTarget.Distance));
                                return RunStatus.Failure;
                            }
            )),
            new Decorator(ret => CanCast("Resurrection", RessTarget),
                new Action(delegate
                {
                    if (castaress("Resurrection", RessTarget, 30, "Buff", "Ressing"))
                    {
                        Blacklist.Add(RessTarget, new TimeSpan(0, 0, 15));
                        new Wait(15, ret => !Me.IsCasting, new Action());
                        return RunStatus.Success;
                    }
                    else
                    {
                        return RunStatus.Failure;
                    }
                }
            )
            )
            ))));
        }
        public Composite Composite_Priest_NeedtoRest()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Me.ManaPercent > 20 && usedBehaviour != "Battleground" && !Me.Combat && !tank.Combat, Priest_Resurrecting()),
                    new Decorator(ret => (Me.ManaPercent <= _rest_if_mana_below) && (!Me.Combat) && (!Me.HasAura("Drink")) && (!Me.IsMoving) && (!Me.Mounted),
                        new Action(delegate
                        {
                            slog(Color.Blue, "#Out of Combat - Mana is at {0} %. Time to drink.#", Round(Me.ManaPercent));
                            Styx.Logic.Common.Rest.Feed();
                            return RunStatus.Success;
                        })),
                        new Decorator(ret => (!Me.Combat) && (Me.HasAura("Drink") && ((Me.ManaPercent < 95) || (!GotBuff("Well Fed") && Me.ActiveAuras["Drink"].TimeLeft.TotalSeconds > 19))),
                            new Action(delegate
                            {
                                slog(Color.Blue, "#Out of Combat - Drinking but no Well Fed buff, will wait 10 sec", Round(Me.ManaPercent));
                                //return RunStatus.Success;
                                new Wait(10, ret => !GotBuff("Well Fed"), new Action(delegate { return RunStatus.Success; }));
                            }))
                ));
        }

        public Composite Composite_Priest_PVPCleanseNow()
        {
            return (
 new PrioritySelector(
     new Action(
         delegate
         {
             UrgentCleanseTarget = PVPGetUrgentCleanseTarget();
             return RunStatus.Failure;
         }
),
new Decorator(ret => unitcheck(UrgentCleanseTarget) && IsSpellReady("Dispel Magic"),
 new PrioritySelector( Composite_Wait_again(), 
     new Action(
     delegate
     {
         slog("urgent debuff dispelling {0}", urgentdebuff);
         if (Cast("Dispel Magic", UrgentCleanseTarget, 40, "Cleanse", "Urgent Debuff was found, dispelling ASAP!"))
         { return RunStatus.Success; }
         else { return RunStatus.Failure; }
     }
)))));
        }

        public Composite Composite_Priest_PVECleanseNow()
        {
            return (
             new PrioritySelector(
                 new Action(
                     delegate
                     {
                         UrgentCleanseTarget = PVEGetUrgentCleanseTarget();
                         return RunStatus.Failure;
                     }
         ),
         new Decorator(ret => unitcheck(UrgentCleanseTarget) && IsSpellReady("Dispel Magic"),
             new PrioritySelector( Composite_Wait_again(), 
                 new Action(
                 delegate
                 {
                     slog("urgent debuff dispelling {0}", urgentdebuff);
                     if (Cast("Dispel Magic", UrgentCleanseTarget, 40, "Cleanse", "Urgent Debuff was found, dispelling ASAP!"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         )))));
        }
        public Composite Composite_Priest_PVPCleansing()
        {
            return (
    new PrioritySelector(
        new Action(
            delegate
            {
                CleanseTarget = PVPGetCleanseTarget();
                return RunStatus.Failure;
            }
),
new Decorator(ret => unitcheck(CleanseTarget) && IsSpellReady("Dispel Magic"),
    new PrioritySelector( Composite_Wait_again(), 
        new Action(
        delegate
        {
            if (Cast("Dispel Magic", CleanseTarget, 40, "Cleanse", "Noone to heal, dispelling"))
            { return RunStatus.Success; }
            else { return RunStatus.Failure; }
        }
)))));
        }
        public Composite Composite_Priest_Cleansing()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            CleanseTarget = GetCleanseTarget();
                            return RunStatus.Failure;
                        }
            ),
            new Decorator(ret => unitcheck(CleanseTarget) && IsSpellReady("Dispel Magic"),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(
                    delegate
                    {
                        if (Cast("Dispel Magic", CleanseTarget, 40, "Cleanse", "Noone to heal, dispelling"))
                        { return RunStatus.Success; }
                        else { return RunStatus.Failure; }
                    }
            )))));
        }
        public Composite Composite_Priest_TopOff()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate{
                            x=GetTopOffNoRenewTarget();
                            return RunStatus.Failure;
                        }),
                        new Decorator(ret=>unitcheck(x),
                            new Action(
                                delegate{
                                    tar=x;
                                    return RunStatus.Failure;
                                })),
                new Decorator(ret => unitcheck(tar),
                    new PrioritySelector(
                        new Decorator(ret => _wanna_move_to_heal && (tar.Distance > 38 || !tar.InLineOfSight),
                            new Action(
                                delegate
                                {
                                    slog("Healing target is too far away or not in LOS, moving to them!");
                                    if (MoveTo(tar)) { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }
                            )
                        ),/*
                        new Decorator(ret => tank != null && (Me.Mounted && !Me.Combat && !tank.Combat) || ((!Me.Combat && unitcheck(tar) && !tar.Combat && tar.Distance > 40)),
                            new Action(
                                delegate
                                {
                                    return RunStatus.Failure;
                                }))*/
                        new Decorator(ret => IsSpellReady("Heal") && !(Me.IsMoving) && tar.HealthPercent > _Heal_min_hp && tar.HealthPercent < _do_not_heal_above && !(tank != null && tank.IsValid && (Me.Mounted && !Me.Combat && !tank.Combat) || ((!Me.Combat && !tar.Combat && tar.Distance > 40))),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Heal", tar, 40, "Heal", "Topping people off"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                })))
                ))));
        }
        public Composite Composite_Priest_PVEBuff()
        {
            return (
                new PrioritySelector(
                    Composite_Priest_Ensure_Chakra(),
                    new Decorator(ret => !Me.HasAura("Inner Fire") && !Me.Mounted && IsSpellReady("Inner Fire"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Inner Fire", "Buff", "Missing Inner Fire"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                    new Decorator(ret => _wanna_move_to_heal && (tank.Distance > 38 || !tank.InLineOfSight),
                        new Action(
                            delegate
                            {
                                slog("Healing target is too far away or not in LOS, moving to them!");
                                if (MoveTo(tank)) { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }
                        )
                    ),
                    Composite_Fortitude(),
                    Composite_ShadowProtection(),
                    new Decorator(ret=>_wanna_pom_ooc, Composite_Prayer_of_Mending_Buff())
                    )
                    );
        }
        public Composite Composite_CoH()
        {
            return(
                new PrioritySelector(
                            new Decorator(ret => _AOE_check_everyone,
                                                 new Decorator(ret => IsSpellReady("Circle of Healing") && Should_AOE("Circle of Healing", _min_player_inside_CoH, 30, _CoH_how_much_health, out x),
                                                     new Decorator(ret => unitcheck(x) && x.Distance < _max_healing_distance && x.InLineOfSight,
                new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Circle of Healing", x, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))))),
                                            new Decorator(ret => IsSpellReady("Circle of Healing") && !_AOE_check_everyone && _AOE_check_focus,
                                                        new Decorator(ret => unitcheck(focus) && Should_AOE(focus, "Circle of Healing", _min_player_inside_CoH, 30, _CoH_how_much_health),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Circle of Healing", focus, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))
                                            ),
                                            new Decorator(ret => IsSpellReady("Circle of Healing") && !_AOE_check_everyone && _AOE_check_tar,
                                                new Decorator(ret => Should_AOE(tar, "Circle of Healing", _min_player_inside_CoH, 30, _CoH_how_much_health),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Circle of Healing", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))),
                                            new Decorator(ret => IsSpellReady("Circle of Healing") && !_AOE_check_everyone && _AOE_check_tank,
                                                new Decorator(ret => Should_AOE(tank, "Circle of Healing", _min_player_inside_CoH, 30, _CoH_how_much_health),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Circle of Healing", tank, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))),
                                            new Decorator(ret => IsSpellReady("Circle of Healing") && !_AOE_check_everyone && _AOE_check_self,
                                                new Decorator(ret => Should_AOE(Me, "Circle of Healing", _min_player_inside_CoH, 30, _CoH_how_much_health),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Circle of Healing", Me, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))))
                                            )
                );
        }
        public Composite Composite_PoH()
        {
            return (
                new Decorator(ret => IsSpellReady("Prayer of Healing") && !Me.Mounted,
                    new PrioritySelector(
                /*new Decorator(ret => _AOE_check_everyone,
                                         new Decorator(ret => Should_PoH(_min_player_inside_PoH, 30, _PoH_how_much_health, out x),
                                             new Decorator(ret => unitcheck(x) && x.Distance < _max_healing_distance && x.InLineOfSight,
        new PrioritySelector( Composite_Wait_again(), 
                                    new Action(delegate
                                    {
                                        if (Cast("Circle of Healing", x, 40, "Heal", "Healing"))
                                        { return RunStatus.Success; }
                                        else { return RunStatus.Failure; }
                                    }))))),*/
                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_focus,
                                                        new Decorator(ret => unitcheck(focus) && Should_PoH(focus, _min_player_inside_PoH, 30, _PoH_how_much_health, SubgroupFromName(focus.Name)),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Prayer of Healing", focus, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))
                                            ),
                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_tar,
                                                new Decorator(ret => Should_PoH(tar, _min_player_inside_PoH, 30, _PoH_how_much_health, SubgroupFromName(tar.Name)),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Prayer of Healing", tar, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))),
                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_tank,
                                                new Decorator(ret => Should_PoH(tank, _min_player_inside_PoH, 30, _PoH_how_much_health, SubgroupFromName(tank.Name)),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Prayer of Healing", tank, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            })))),
                                            new Decorator(ret => !_AOE_check_everyone && _AOE_check_self,
                                                new Decorator(ret => Should_PoH(Me, _min_player_inside_PoH, 30, _PoH_how_much_health, SubgroupFromName(Me.Name)),
                                                    new PrioritySelector( Composite_Wait_again(), 
                                            new Action(delegate
                                            {
                                                if (Cast("Prayer of Healing", Me, 40, "Heal", "Healing"))
                                                { return RunStatus.Success; }
                                                else { return RunStatus.Failure; }
                                            }))))
                        )));
        }

        public Composite Composite_Prayer_of_Mending()
        {
            return (
                new Decorator(ret => unitcheck(tank) && IsSpellReady("Prayer of Mending") && !GotBuff("Prayer of Mending", tank) && !Me.Mounted && tank != Me && tank.Distance < _max_healing_distance && tank.InLineOfSight,
                    new PrioritySelector(
                        new Action(
                            delegate{
                                x=GetWhoHavePrayerOfMending();
                                return RunStatus.Failure;
                            }),
                            new Decorator(ret=>!unitcheck(x),
                    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Prayer of Mending", tank, 40, "Buff", "Missing Prayer of Mending"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))
                ))));
        }
        public Composite Composite_Prayer_of_Mending_Buff()
        {
            return(
                new Decorator(ret => unitcheck(tank) && IsSpellReady("Prayer of Mending") && !GotBuff("Prayer of Mending", tank) && !Me.Mounted && tank != Me &&tank.Distance<_max_healing_distance && tank.InLineOfSight && TankGotTarget(),
                    new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Prayer of Mending", tank, 40, "Buff", "Missing Prayer of Mending"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))
                ));
        }
        public Composite Composite_Fortitude()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            BlessTarget = GetFortitudeTarget();
                            return RunStatus.Failure;
                        }),
                        new Decorator(ret => unitcheck(BlessTarget) && !Me.Mounted && !GotBuff("Power Word: Fortitude",BlessTarget),
                        new Action(delegate
                        {
                            if (Cast("Power Word: Fortitude", BlessTarget, 40, "Buff", "Missing Fortitude"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }))));
        }
        public Composite Composite_ShadowProtection()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            BlessTarget = GetShadowProtectionTarget();
                            return RunStatus.Failure;
                        }),
                        new Decorator(ret => unitcheck(BlessTarget) &&  !Me.Mounted && !GotBuff("Shadow Protection",BlessTarget),
                        new Action(delegate
                        {
                            if (Cast("Shadow Protection", BlessTarget, 40, "Buff", "Missing Shadow Protection"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }))));
        }
        public Composite Composite_Priest_Self()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => sw.Elapsed.TotalSeconds > _use_mana_rec_trinket_every && Me.Combat && Me.ManaPercent <= _min_mana_rec_trinket,
                        new Action(delegate
                        {
                            Lua.DoString("UseItemByName(\"" + "Tyrande\'s Favorite Doll" + "\")");
                            slog(Color.Blue, "Mana at {0} % using Tyrande\'s Favorite Doll", Round(Me.ManaPercent));
                            sw.Reset();
                            sw.Start();
                            return RunStatus.Failure;
                        }
                        )
                        ),
                        new Decorator(ret => _wanna_mana_potion && Me.ManaPercent <= _min_mana_potion, Use_mana_pot()),
                        new Decorator(ret => _wanna_DP && Me.HealthPercent < _min_DP_hp && Me.Combat && IsSpellReady("Desperate Prayer"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Desperate Prayer", "Self", "Low HP"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                        new Decorator(ret => _wanna_fade && Me.HealthPercent < _min_fade_hp && Me.Combat && IsSpellReady("Fade") && IsEnemyAttakingMe(),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Fade", "Self", "Mobs are beating on me!"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => Me.Combat && unitcheck(Enemy) && Me.ManaPercent <= _min_Shadowfiend_mana_NOT_safe && Me.Combat && IsSpellReady("Shadowfiend"),
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                            {
                                if (Me.CurrentTarget != Enemy) { Enemy.Target(); }
                                if (Cast("Shadowfiend", Enemy,40, "Mana", "Cannot use Hymm of Hope, still I need mana"))
                                { return RunStatus.Failure; }
                                else { return RunStatus.Success; }
                            }))
                )
                ));
        }

        public Composite Composite_inizialize_subgroup()
        {
            return (new Action(
                delegate
                {
                    if (Me.Combat) { General_we_are_in_combat = true; }
                    else { General_we_are_in_combat = false; }
                    if (General_we_were_in_combat == false && General_we_are_in_combat)
                    {
                        if (Me.Combat) { General_we_were_in_combat = true; }
                        else { General_we_were_in_combat = false; }
                        slog("Entering in combat! Building Subgroup Array now!");
                        BuildSubGroupArray();
                    }
                    return RunStatus.Failure;
                }
            ));
        }

        public Composite Composite_Priest_Mana_Rec()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Me.Combat && unitcheck(Enemy) && Me.ManaPercent <= _min_Shadowfiend_mana_if_safe && Me.Combat && IsSpellReady("Shadowfiend") && ((_check_level_for_shadowfind && Enemy.Level>Me.Level+2) || !_check_level_for_shadowfind),
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                            {
                                if (Me.CurrentTarget != Enemy) { Enemy.Target(); }
                                if (Cast("Shadowfiend", Enemy,40, "Mana", "Noone to heal, need mana"))
                                { return RunStatus.Failure; }
                                else { return RunStatus.Success; }
                            }))
                ),
                new Decorator(ret=> _wanna_Hymm_of_Hope_after_shadowfiend&& Me.Combat && unitcheck(Enemy) && Me.Combat && IsSpellReady("Hymm of Hope"),
                    new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                            {
                                if (Cast("Hymm of Hope", "Mana", "Noone to heal, need mana"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))
                )
                )
                );
        }
        public Composite Composite_Priest_CheckMe()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            Check_Trinket();
                            return RunStatus.Failure;
                        }),
                new Decorator(ret => !unitcheck(Me), new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Success;
                })),
                new Decorator(ret => Me.IsCasting,
                    Composite_Priest_Interrupt()),
                Composite_Wait()
                )
                );
        }
        public Composite Composite_Priest_Raid()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            Decice_if_special_or_normal_raid();
                            return RunStatus.Failure;
                        }
            ),
            Composite_inizialize_subgroup(),
                    new Decorator(ret => Global_chimaeron || _selective_healing || Global_SoD,
                        Composite_Priest_Special_Raid()),
                        new Decorator(ret => !Global_chimaeron && !_selective_healing && !Global_SoD && _raid_healer,
                            Composite_Priest_AOE_Raid()),
                        new Decorator(ret => !Global_chimaeron && !_selective_healing && !Global_SoD && !_raid_healer,
                            Composite_Priest_Normal_Raid())
                            )
                            );
        }
        public Composite Composite_Priest_Special_Raid()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Global_chimaeron, new Action(delegate{slog("Chimaeron fight not yet supported for priest, CC is DISABLED!");})),
                    new Decorator(ret => Global_SoD, Composite_Priest_Spine_of_Deathwing()),
                    new Decorator(ret => _selective_healing,Composite_Priest_Raid_Selective_healing())
                )
                 );
        }
        public Composite Composite_Priest_Raid_Selective_healing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret=>_raid_healer,
                        new Action(delegate{slog("raid healer not yet supported! do not use! CC is disabled!");})),
                        new Decorator(ret=>!_raid_healer,
                         Composite_Priest_Raid_TANKH_Selective_healing())   
                    ));
        }
        public Composite Composite_Priest_Spine_of_Deathwing()
        {
            return (new PrioritySelector(
                        Composite_Priest_CheckMe(),
                        Composite_SetTank(),
                        Composite_Find_Special_Raid_unit_to_heal(),
                        Composite_Priorize_tank_healing(),
                        Composite_Priest_PVEHealing(),
                        Composite_Priest_Mana_Rec()
                    )
                );
        }
        public Composite Composite_Priest_Raid_TANKH_Selective_healing()
        {
            return (
                new PrioritySelector(
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_check_special_healing(),
                    Composite_Find_Selective_Raid_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    Composite_Set_Focus(),
                    new Decorator(ret => Dismount(),
                    new PrioritySelector(
                        Composite_Priest_Interrupt(),
                        Composite_Priest_Self(),
                        Composite_PVERacials(),
                        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
                        Composite_Priest_PVEHealing(),
                        //,
                        Composite_Priest_Mana_Rec(),
                        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
                        Composite_Priest_TopOff()
                 )))
                );
        }
        public Composite Composite_Priest_AOE_Raid()
        {
            return (
                    new PrioritySelector(
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Raid_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    Composite_Set_Focus(),
                    new Decorator(ret => Dismount(),
                    new PrioritySelector(
                        Composite_Priest_Interrupt(),
                        Composite_Priest_Self(),
                        Composite_PVERacials(),
                        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
                        Composite_Priest_Raid_AOE_Healing(),
                        Composite_Priest_Mana_Rec(),
                        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
                        Composite_Priest_TopOff()
                 )))
                 );
        }
        public Composite Composite_Priest_Normal_Raid()
        {
            return (
                    new PrioritySelector(
                    Composite_Priest_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Raid_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    Composite_Set_Focus(),
                    new Decorator(ret => Dismount(),
                    new PrioritySelector(
                        Composite_Priest_Interrupt(),
                        Composite_Priest_Self(),
                        Composite_PVERacials(),
                        new Decorator(ret => _wanna_urgent_cleanse, Composite_Priest_PVECleanseNow()),
                        Composite_Priest_PVEHealing(),
                        Composite_Priest_Mana_Rec(),
                        new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Priest_Cleansing()),
                        Composite_Priest_TopOff()
                 )))
                 );
        }
        public Composite Composite_Set_Focus()
        {
            return (
                new Decorator(ret => !_AOE_check_everyone && _AOE_check_focus,
                                        new Action(
                                            delegate
                                            {
                                                focus = focusunit();
                                                return RunStatus.Failure;
                                            })
                                            ));
        }
    }
}
