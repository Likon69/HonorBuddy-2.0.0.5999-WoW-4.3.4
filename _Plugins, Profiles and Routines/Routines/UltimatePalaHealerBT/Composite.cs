using System;
using System.Drawing;
using Styx;
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

        public Composite Composite_Solo()
        {
            return (
                new PrioritySelector(
                  /*  new Action(delegate{
                        slog("SOLOOOOOOOOOOOOO");
                        return RunStatus.Failure;
                    }),*/
                  new Decorator(ret => Me.IsInParty || Me.IsInRaid,
                            new Action(
                                delegate
                                {
                                    CreateBehaviors();
                                    return RunStatus.Success;
                                }
            )),
                    Composite_CheckMe(),
                    Composite_Solo_SetTank(),
                    Composite_Solo_Find_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Solo_Find_Enemy(),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Solo_Buff(),
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    new Decorator(ret => _wanna_move, Composite_Solomove()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    Composite_PVEHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    new Decorator(ret => Me.Combat, Composite_Solo_Combat()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime()),
                    Composite_TopOff()
                 )))
            );
        }

        public Composite Composite_Party_or_Raid()
        {
            return (
                    new PrioritySelector(
                        new Decorator(ret=>!Me.IsInParty && !Me.IsInRaid,
                            new Action(
                                delegate{
                                    CreateBehaviors();
                                    return RunStatus.Success;
                                }
            )),
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_RAF_Find_Enemy(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Interrupt(),
                //Composite_NeedtoRest(),                                                                       /*Moved to PVERest*/
                //new Decorator(ret=>_wanna_mount, Composite_MountUp()),                                     /*Moved to PVERest*/
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVEHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                //,new Decorator(ret=>_wanna_buff,Composite_PVEBuff())                                       /*Moved to PVERest*/
                 )))
            );
        }
        public Composite Composite_Arena()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_Arena_SetTank(),
                    Composite_Find_Battleground_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Find_Battleground_Enemy(),
                    Conposite_Find_Arena_pet(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Self(),
                    Composite_PVPRacials(),
                    new Decorator(ret => _wanna_taunt, Composite_Taunt()),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVPCleanseNow()),
                    new Decorator(ret => _wanna_HoF, Composite_HoFNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVPHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_PVPCleansing()),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                    )))
            );
        }
        public Composite Composite_WorldPVP()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_WorldPVP_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Find_WorldPVP_Enemy(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_BGSelf(),
                    Composite_PVPRacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVPCleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVPHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_PVPCleansing()),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())))
                    )
            );
        }
        public Composite Composite_Battleground()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Battleground_unit_to_heal(),
                    Composite_Priorize_tank_healing(),
                    Composite_print_tar(),
                    Composite_Find_Battleground_Enemy(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_BGSelf(),
                    Composite_PVPRacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVPCleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVPHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_PVPCleansing()),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())))
                    )
            );
        }
        public Composite Composite_Dungeon()
        {
            // slog("sono dentro Dungeon");
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Interrupt(),
                //Composite_NeedtoRest(),                                                                       /*Moved to PVERest*/
                //new Decorator(ret=>_wanna_mount, Composite_MountUp()),                                     /*Moved to PVERest*/
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVEHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                //,new Decorator(ret=>_wanna_buff,Composite_PVEBuff())                                       /*Moved to PVERest*/
                 )))
            );
        }
        public Composite Composite_Raid()
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
                    new Decorator(ret => Global_chimaeron || _selective_healing || Global_SoD,
                        Composite_Special_Raid()),
                        new Decorator(ret => !Global_chimaeron && !_selective_healing && !Global_SoD,
                            Composite_Normal_Raid())
                            )
                            );
            //Composite_Dungeon()
        }
        public Composite Composite_Find_WorldPVP_Enemy()
        {
            {

                return (
                    new PrioritySelector(
                        new Action(
                            delegate
                            {
                                Enemy = WorldPVPGiveEnemy(29);
                                return RunStatus.Failure;
                            }
                ),
                new Decorator(ret => _wanna_target && (Me.CurrentTarget == null || !Me.CurrentTarget.IsValid || Me.CurrentTarget.Dead),
                    new PrioritySelector(
                    new Decorator(ret => unitcheck(Enemy),
                        new Action(
                            delegate
                            {
                                Enemy.Target();
                            }
                )),
                new Decorator(ret => unitcheck(tank.CurrentTarget),
                    new Action(
                        delegate
                        {
                            tank.CurrentTarget.Target();
                        }
                )))))
                );
            }
        }
        public Composite Composite_Find_Battleground_Enemy()
        {

            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            Enemy = BattlegroundGiveEnemy(29);
                            return RunStatus.Failure;
                        }
            ),
            new Decorator(ret => _wanna_target && (Me.CurrentTarget == null || !Me.CurrentTarget.IsValid || Me.CurrentTarget.Dead),
                new PrioritySelector(
                new Decorator(ret => unitcheck(Enemy),
                    new Action(
                        delegate
                        {
                            Enemy.Target();
                        }
            )),
            new Decorator(ret => unitcheck(tank.CurrentTarget),
                new Action(
                    delegate
                    {
                        tank.CurrentTarget.Target();
                    }
            )))))
            );
        }
        public Composite Composite_Taunt()
        {
            return (
                new Decorator(ret => unitcheck(Epet) && unitcheck(Epet.CurrentTarget) && Epet.CurrentTarget != Me && unitcheck(tank) && (!Me.Mounted || !tank.Mounted) && (Me.Combat || tank.Combat) && !Me.IsCasting && IsSpellReady("Righteous Defense"),
                    new PrioritySelector( Composite_Wait_again(),  new Action(
                        delegate
                        {
                            Cast("Righteous Defense", Epet.CurrentTarget, 40, "Utility", "Taunting pet if any");
                            return RunStatus.Failure;
                        }))));
        }
        public Composite Conposite_Find_Arena_pet()
        {
            return (
         new Action(delegate
         {
             Epet = GiveEnemyPet(38);
             return RunStatus.Failure;
         }
                ));
        }
        public Composite Composite_Find_WorldPVP_unit_to_heal()
        {
            return (
                new Action(delegate
                {
                    tar = GetWorldPVPHealTarget();
                    return RunStatus.Failure;
                }
                ));
        }
        public Composite Composite_Find_Battleground_unit_to_heal()
        {
            return (
                new Action(delegate
                {
                    tar = GetBattlegroundHealTarget();
                    return RunStatus.Failure;
                }
                ));
        }
        public Composite Composite_Special_Raid()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Global_chimaeron,
                        Composite_Chimearon()),
                    new Decorator(ret => Global_SoD, 
                        Composite_Spine_of_Deathwing()),
                    new Decorator(ret =>_selective_healing,
                        Composite_Raid_Selective_healing())
                )
                 );
        }
        public Composite Composite_check_special_healing()
        {
            return new Decorator(ret => !specialhealing_warning,
                new Action(
                    delegate
                    {
                        if (Check_special_healing())
                        {
                            return RunStatus.Success;
                        }
                        else
                        {
                            return RunStatus.Failure;
                        }
                    }
            ));
        }
        public Composite Composite_Raid_Selective_healing()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_check_special_healing(),
                    Composite_Find_Selective_Raid_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Interrupt(),
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVEHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                ))));
        }
        public Composite Composite_Chimearon()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => GotBuff("Mortality"),
                        Composite_Chimaeron_p3()),
                        Composite_Chimaeron()
                        )
                        );
        }
        public Composite Composite_Spine_of_Deathwing()
        { 
            return (new PrioritySelector(
                        Composite_CheckMe(),
                        Composite_SetTank(),
                        Composite_Find_Special_Raid_unit_to_heal(),
                        Composite_Priorize_tank_healing(),
                        Composite_PVEHealing(),
                        Composite_Mana_Rec()
                    )
                );
        }
        public Composite Composite_Chimaeron_p3()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Special_Raid_unit_to_heal(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    Composite_Mana_Rec(),
                    new Decorator(ret => Me.Combat, Composite_Solo_Combat()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                 )
                );
        }
        public Composite Composite_Chimaeron()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Special_Raid_unit_to_heal(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                    Composite_Interrupt(),
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_ChimaeronHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                ));
        }
        public Composite Composite_Normal_Raid()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Raid_unit_to_heal(),
                    Composite_print_tar(),
                    Composite_Priorize_tank_healing(),
                    Composite_Find_Enemy(),
                                        new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    Composite_Interrupt(),
                    Composite_Self(),
                    Composite_PVERacials(),
                    new Decorator(ret => Me.Combat && _wanna_Judge, Composite_Judge()),
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    new Decorator(ret => Me.Combat, Composite_EnemyInterrupt()),
                    Composite_PVEHealing(),
                    Composite_Mana_Rec(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => Me.Combat, Composite_Dps()),
                    new Decorator(ret => Me.Combat, Composite_ConsumeTime())
                 )))
                 );
        }
        public Composite Composite_Priorize_tank_healing()
        {
            return(
                new Decorator(ret=>unitcheck(tar) && unitcheck(tank),
                new Action(
                    delegate{
                        if (tank.Distance<_max_healing_distance && tank.InLineOfSight && tar.HealthPercent*(1+((_healing_tank_priority/(double)100)*_tank_healing_priority_multiplier))>tank.HealthPercent)
                        {
                            slog("tar hp {0} and tank hp {1} also you priority is {2}, switching target to tank", Round(tar.HealthPercent), Round(tank.HealthPercent), _healing_tank_priority);
                            tar = tank;
                        }
                        else if (!unitcheck(tank))
                        {
                            slog(Color.DarkRed, "ERROR: Tank is not valid!");
                        }
                        else if (tank.Distance > _max_healing_distance)
                        {
                            slog(Color.DarkRed, "ERROR: Tank not in range! {0} is at distance {1}", privacyname(tank), tank.Distance);
                            if (tar.HealthPercent * (1 + ((_healing_tank_priority / (double)100) * _tank_healing_priority_multiplier)) > tank.HealthPercent)
                            {
                                slog(Color.DarkRed, "I should heal the tank, that is out of range, MOVE TO {0}!",privacyname(tank));
                                if (_wanna_move)
                                {
                                    MoveTo(tank);
                                }
                                tar = null;
                            }
                        }
                        else if (!tank.InLineOfSight)
                        {
                            slog(Color.DarkRed, "ERROR: Tank not in LOS! {0}", privacyname(tank));
                            if (tar.HealthPercent * (1 + ((_healing_tank_priority / (double)100) * _tank_healing_priority_multiplier)) > tank.HealthPercent)
                            {
                                slog(Color.DarkRed, "I should heal the tank, that is out of range, MOVE TO {0}!", privacyname(tank));
                                if (_wanna_move)
                                {
                                    MoveTo(tank);
                                }
                                tar = null;
                            }
                        }
                        return RunStatus.Failure;
                    }
                )));
        }

        public Composite Composite_Find_Selective_Raid_unit_to_heal()
        {
            return (
    new Action(delegate
    {
        tar = GetSelectiveRaidHealTarget();
        return RunStatus.Failure;
    }
    ));
        }
        public Composite Composite_Find_Special_Raid_unit_to_heal()
        {
            return (
                new Action(delegate
                {
                    tar = GetSpecialRaidHealTarget();
                    return RunStatus.Failure;
                }
                )
            );
        }
        public Composite Composite_Find_Spine_Of_Deathwing_Raid_unit_to_heal()
        {
            return (
                new Action(delegate
                    {
                        tar = GetSpineOfDeathwingHealTarget();
                        return RunStatus.Failure;
                    }
                )
            );
        }

        public Composite Composite_Find_Raid_unit_to_heal()
        {
            return (
    new Action(delegate
    {
        tar = GetRaidHealTarget();
        return RunStatus.Failure;
    }
    ));
        }
        public Composite Composite_SoloRest()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Solo_Buff(),
                    Composite_NeedtoRest(),
                    new Decorator(ret => _wanna_mount, Composite_MountUp()),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    Composite_PVEHealing(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff())),
                    Composite_Crusader()
                    )
                    );
        }
        public Composite Composite_Solo_Combat()
        {
            return (
                new Decorator(ret => unitcheck(Enemy),
                    new PrioritySelector(
                        new Decorator(ret => Me.CurrentHolyPower == 3 && !GotBuff("Inquisition"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(
                        delegate
                        {
                            if (Cast("Inquisition", "Buff", "Got plenty of holy power, time to pew pew"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
         ))),
         new Decorator(ret => IsSpellReady("Holy Shock") && Enemy.Distance < 20 && Enemy.InLineOfSight,
              new PrioritySelector( Composite_Wait_again(), 
                  new Action(
                        delegate
                        {
                            if (_wanna_face) { WoWMovement.Face(); }
                            if (Cast("Holy Shock", Enemy, 20, "DPS", "Solo, we need to kill"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
         ))),
         new PrioritySelector( Composite_Wait_again(), 
             new Action(
                        delegate
                        {
                            if (_wanna_face) { WoWMovement.Face(); }
                            if (Cast("Exorcism", Enemy, 30, "DPS", "Solo, we need to kill"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }
            )))));
        }
        public Composite Composite_Solomove()
        {
            return (
                new Decorator(ret => unitcheck(Enemy) && Enemy.Distance > 20,
                    new Action(
                        delegate
                        {
                            slog(Color.Black, "Solo: Moving to enemy");
                            Navigator.MoveTo(WoWMathHelper.CalculatePointFrom(Me.Location, Enemy.Location, 15f));
                        }
            )));
        }
        public Composite Composite_Solo_Buff()
        {
            return (
                new PrioritySelector(
                    Composite_Seal(),
                    Composite_Crusader(),
                    Composite_Aura(),
                    Composite_Blessing()
                    ));
        }
        public Composite Composite_Solo_Find_Enemy()
        {
            return (
                new PrioritySelector(
                new Action(
                    delegate
                    {
                        Enemy = SoloGiveEnemy(29);
                        return RunStatus.Failure;
                    }
            ),
            new Decorator(ret => unitcheck(Enemy) && (Me.CurrentTarget == null || !Me.CurrentTarget.IsValid || Me.CurrentTarget.Dead),
                new Action(
                    delegate
                    {
                        Enemy.Target();
                        return RunStatus.Failure;
                    }
            ))));

        }
        public Composite Composite_Solo_Find_unit_to_heal()
        {
            return (
                new Action(
                    delegate
                    {
                        if (Me.HealthPercent < 95) { tar = Me; } else { tar = null; }
                        return RunStatus.Failure;
                    }
            ));
        }
        public Composite Composite_Solo_SetTank()
        {
            return (
                new Action(
                    delegate
                    {
                        tank = Me;
                        return RunStatus.Failure;
                    }
            )
            );
        }

        public Composite Composite_PVPRacials()
        {
            return (
                new PrioritySelector(
                    Composite_Racials(),
                    new Decorator(ret => _wanna_torrent && unitcheck(Enemy) && Enemy.Distance < 10 && Enemy.IsCasting && !IsSpellReady("Rebuke"),
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(
             delegate
             {
                 if (Cast("Arcane Torrent", "Self", "Interrupting"))
                 { return RunStatus.Success; }
                 else { return RunStatus.Failure; }
             }
         )))));
        }
        public Composite Composite_Racials()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => _wanna_stoneform && IsSpellReady("Stoneform") && Me.HealthPercent < _min_stoneform && !GotBuff("Divine Protection"),
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(
                 delegate
                 {
                     if (Cast("Stoneform", "Self", "Low HP"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         ))),
         new Decorator(ret => _wanna_everymanforhimself && IsSpellReady("Every Man for Himself") && Me.Stunned && !GotBuff("Charge Stun") && !GotBuff("Sap"),
             new PrioritySelector( Composite_Wait_again(), 
                 new Action(
                 delegate
                 {
                     if (Cast("Every Man for Himself", "Self", "Stunned"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         ))),
         new Decorator(ret => _wanna_gift && IsSpellReady("Gift of the Naaru") && unitcheck(tar) && tar.HealthPercent < _min_gift_hp,
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(
             delegate
             {
                 if (Cast("Gift of the Naaru", tar, 40, "Self", "Free heal"))
                 { return RunStatus.Success; }
                 else { return RunStatus.Failure; }
             }
         )))
         ));
        }
        public Composite Composite_PVERacials()
        {
            return (
                new PrioritySelector(
                    Composite_Racials(),
         new Decorator(ret => _wanna_torrent && IsSpellReady("Arcane Torrent") && Me.ManaPercent < _min_torrent_mana_perc,
             new PrioritySelector( Composite_Wait_again(), 
                 new Action(
             delegate
             {
                 if (Cast("Arcane Torrent", "Self", "Reccing mana"))
                 { return RunStatus.Success; }
                 else { return RunStatus.Failure; }
             }
         )))));
        }
        public Composite Composite_EnemyInterrupt()
        {
            return (
                new Decorator(ret => unitcheck(Enemy) && Enemy.IsCasting,
                new PrioritySelector(
                    new Decorator(ret => _wanna_rebuke && IsSpellReady("Rebuke") && Enemy.IsWithinMeleeRange && Enemy.InLineOfSight && Enemy.IsCasting && Enemy.CanInterruptCurrentSpellCast,
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(
                 delegate
                 {
                     if (Cast("Rebuke", Enemy, 5, "EnemyInterrupt", "Interrupting"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         ))),
         new Decorator(ret => _wanna_HoJ && Enemy.HealthPercent < 50,
             new PrioritySelector(
                 new Action(
                     delegate
                     {
                         slog(Color.DarkMagenta, "Wanna Hammer of Justice on {0} at {1}", privacyname(Enemy), Round(Enemy.Distance));
                         return RunStatus.Failure;
                     }
            ),
            new Decorator(ret => _wanna_move_to_HoJ && Enemy.Distance > 10 && Enemy.Distance < 40,
                new Action(
                    delegate
                    {
                        slog(Color.DarkMagenta, "Moving in to Hammer of Justice");
                        if (MoveTo(Enemy)) { return RunStatus.Success; }
                        else { return RunStatus.Failure; }
                    }
            )),
            new Decorator(ret => IsSpellReady("Hammer of Justice") && Enemy.Distance < 10 && Enemy.InLineOfSight,
            new PrioritySelector( Composite_Wait_again(), 
                new Action(
                 delegate
                 {
                     if (Cast("Hammer of Justice", Enemy, 10, "EnemyInterupt", "Interrupting"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         )))
         )))));
        }
        public Composite Composite_Interrupt()
        {
            return (new PrioritySelector(
                new Decorator(ret => Me.IsCasting && lastCast != null && lastCast.IsValid&& !lastCast.Dead && lastCast.HealthPercent >= Math.Max(_stop_DL_if_above, _min_DL_hp) && Me.CastingSpell.Name == "Divine Light",
            new Action(
                delegate
                {
                    SpellManager.StopCasting();
                    if (lastCast != null)
                    {
                        slog(Color.Brown, "Interrupting Healing DL, target {0} at {1} %", privacyname(lastCast), Round(lastCast.HealthPercent));
                    }
                }
            )),
            new Decorator(ret => Me.IsCasting && lastCast != null && lastCast.IsValid && !lastCast.Dead && lastCast.HealthPercent >= Math.Max(_stop_DL_if_above, _min_FoL_hp) &&  Me.CastingSpell.Name == "Flash of Light",
            new Action(
                delegate
                {
                    SpellManager.StopCasting();
                    if (lastCast != null)
                    {
                        slog(Color.Brown, "Interrupting Healing FoL, target {0} at {1} %", privacyname(lastCast), Round(lastCast.HealthPercent));
                    }
                }
            )),
            new Decorator(ret => Me.IsCasting && lastCast != null && lastCast.IsValid&& !lastCast.Dead && lastCast.HealthPercent >= _do_not_heal_above && Me.CastingSpell.Name == "Holy Light",
                new Action(
                delegate
                {
                    SpellManager.StopCasting();
                    if (lastCast != null)
                    {
                        slog(Color.Brown, "Interrupting Healing HL, target {0} at {1} %",privacyname(lastCast), Round(lastCast.HealthPercent));
                    }
                }
            ))
            ));
        }
        public Composite Composite_PVPRest()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_Battleground_unit_to_heal(),
                    Composite_NeedtoRest(),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVPCleanseNow()),
                    Composite_PVPHealing(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_PVPCleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => _wanna_buff, Composite_PVEBuff()),
                    new Decorator(ret => _wanna_mount, Composite_MountUp()))),
                    Composite_Crusader()
        )
        );
        }
        public Composite Composite_ARENARest()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    new Decorator(ret=> (unitcheck(tank) && tank!=Me && tank.HealthPercent>80), Composite_ARENANeedtoRest()),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVPCleanseNow()),
                    Composite_PVPHealing(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_PVPCleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => _wanna_buff, Composite_PVEBuff()),
                    new Decorator(ret => _wanna_mount, Composite_MountUp()))),
                    Composite_Crusader()
        )
        );
        }

        public Composite Composite_Retr_Rest()
        {
            return (
             new PrioritySelector(
                 Composite_CheckMe(),
                 Composite_Retr_SetTank(),
                 Composite_NeedtoRest(),
                 new Decorator(ret => _wanna_mount, Composite_MountUp()),
                 new Decorator(ret => Dismount(),
                     new PrioritySelector(
                 new Decorator(ret => _wanna_buff, Composite_Retr_Buff()))),
                 Composite_Crusader()

                 )
                 );
        }

        public Composite Composite_PVERest()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_SetTank(),
                    Composite_Find_unit_to_heal(),
                    Composite_NeedtoRest(),
                    new Decorator(ret => _wanna_mount, Composite_MountUp()),
                    new Decorator(ret => Dismount(),
                        new PrioritySelector(
                    new Decorator(ret => _wanna_urgent_cleanse, Composite_PVECleanseNow()),
                    Composite_PVEHealing(),
                    new Decorator(ret => _wanna_cleanse && (Me.Combat || !Me.Mounted), Composite_Cleansing()),
                    Composite_TopOff(),
                    new Decorator(ret => _wanna_buff, Composite_PVEBuff()))),
                    Composite_Crusader()


                    )
                    );
        }
        public Composite Composite_ConsumeTime()
        {
            return (
                new Decorator(ret => _wanna_CS && Me.Combat && unitcheck(Enemy),
                    new PrioritySelector(
                        new Decorator(ret => _wanna_face,
                            new Action(
                                delegate
                                {
                                    WoWMovement.Face();
                                    return RunStatus.Failure;
                                }
            )),
            new Decorator(ret => IsSpellReady("Crusader Strike") && Enemy.IsWithinMeleeRange && Enemy.InLineOfSight,
            new PrioritySelector( Composite_Wait_again(), 
                new Action(
                 delegate
                 {
                     if (Cast("Crusader Strike", Enemy, 5, "DPS", "Melee range, free HP"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         ))))));
        }
        public Composite Composite_Dps()
        {
            return (
                new Decorator(ret => _wanna_HoW && IsSpellReady("Hammer of Wrath") && Me.Combat && unitcheck(Enemy) && Enemy.Distance<30 && Enemy.InLineOfSight && Enemy.HealthPercent<20,
                    new PrioritySelector(
                        new Decorator(ret => _wanna_face,
                            new Action(
                                delegate
                                {
                                    WoWMovement.Face();
                                    return RunStatus.Failure;
                                }
            )),
            new PrioritySelector( Composite_Wait_again(), 
                new Action(
                 delegate
                 {
                     if (Cast("Hammer of Wrath", Enemy, 30, "DPS", "Enemy low on health"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         )))));
        }
        public Composite Composite_HoFNow()
        {
            return (
                new Decorator(ret => IsSpellReady("Hand of Freedom"),
 new PrioritySelector(
     new Action(
         delegate
         {
             UrgentHoFTarget = GetHoFTarget();
             return RunStatus.Failure;
         }
),
new Decorator(ret => unitcheck(UrgentHoFTarget),
 new PrioritySelector( Composite_Wait_again(), 
     new Action(
     delegate
     {
         if (Cast("Hand of Freedom", UrgentHoFTarget, 40, "Cleanse", "Target is snared, HoF now"))
         { return RunStatus.Success; }
         else { return RunStatus.Failure; }
     }
))))));
        }
        public Composite Composite_PVPCleanseNow()
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
         new Decorator(ret => unitcheck(UrgentCleanseTarget),
             new PrioritySelector( Composite_Wait_again(), 
                 new Action(
                 delegate
                 {
                     slog("urgent debuff dispelling {0}", urgentdebuff);
                     if (Cast("Cleanse", UrgentCleanseTarget, 40, "Cleanse", "Urgent Debuff was found, dispelling ASAP!"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         )))));
        }
        public Composite Composite_PVECleanseNow()
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
         new Decorator(ret => unitcheck(UrgentCleanseTarget),
             new PrioritySelector( Composite_Wait_again(), 
                 new Action(
                 delegate
                 {
                     slog("urgent debuff dispelling {0}", urgentdebuff);
                     if (Cast("Cleanse", UrgentCleanseTarget, 40, "Cleanse", "Urgent Debuff was found, dispelling ASAP!"))
                     { return RunStatus.Success; }
                     else { return RunStatus.Failure; }
                 }
         )))));
        }
        public Composite Composite_PVPCleansing()
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
            new Decorator(ret => unitcheck(CleanseTarget),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(
                    delegate
                    {
                        if (Cast("Cleanse", CleanseTarget, 40, "Cleanse", "Noone to heal, dispelling"))
                        { return RunStatus.Success; }
                        else { return RunStatus.Failure; }
                    }
            )))));
        }
        public Composite Composite_Cleansing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret=>!_cleanse_only_self_and_tank,
                        new PrioritySelector(
                    new Action(
                        delegate
                        {
                            CleanseTarget = GetCleanseTarget();
                            return RunStatus.Failure;
                        }
            ),
            new Decorator(ret => unitcheck(CleanseTarget),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(
                    delegate
                    {
                        if (Cast("Cleanse", CleanseTarget, 40, "Cleanse", "Noone to heal, dispelling"))
                        { return RunStatus.Success; }
                        else { return RunStatus.Failure; }
                    }
            ))))),
            new Decorator(ret=>_cleanse_only_self_and_tank,
                new PrioritySelector(
                    new Decorator(ret=>NeedsCleanse(Me, _can_dispel_disease, _can_dispel_magic, _can_dispel_poison),
                        new Action(
                            delegate{
                                if (Cast("Cleanse", Me, 40, "Cleanse", "Noone to heal, dispelling"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }
                            )),
                            new Decorator(ret=>unitcheck(tank) &&  NeedsCleanse(tank, _can_dispel_disease, _can_dispel_magic, _can_dispel_poison),
                                new Action(
                                    delegate{
                                        if (Cast("Cleanse", tank, 40, "Cleanse", "Noone to heal, dispelling"))
                                        { return RunStatus.Success; }
                                        else { return RunStatus.Failure; }
                                    }
                                    ))
                    ))
            ));
        }
        public Composite Composite_Seal()
        {
            return (
                new PrioritySelector(
                new Decorator(ret => !GotBuff("Seal of Insight") && (!Me.Mounted) && IsSpellReady("Seal of Insight"),
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                        {
                            if (Cast("Seal of Insight", "Buff", "Missing seal"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        }))),
                        new Decorator(ret=>!SpellManager.HasSpell("Seal of Insight") && !GotBuff("Seal of Righteousness") && IsSpellReady("Seal of Righteousness") &&(!Me.Mounted),
                            new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                        {
                            if (Cast("Seal of Righteousness", "Buff", "Missing seal"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        })))
                        ));
        }

        public Composite Composite_Retr_Aura()
        {
            return (
                new Decorator(ret => !IsPaladinAura("Retribution Aura") && (!_wanna_crusader || _wanna_crusader && !Me.Mounted) && IsSpellReady("Retribution Aura"),
                            new Action(delegate
                            {
                                if (Cast("Retribution Aura", "Buff", "Missing aura"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))
                            );
        }

        public Composite Composite_Aura()
        {
            return (
                new PrioritySelector(
                        new Decorator(ret => _aura_type == 0 && !IsPaladinAura("Concentration Aura") && (!_wanna_crusader || _wanna_crusader && !Me.Mounted) && IsSpellReady("Concentration Aura"),
                            new Action(delegate
                            {
                                if (Cast("Concentration Aura", "Buff", "Missing aura"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })),
                        new Decorator(ret => _aura_type == 1 && !IsPaladinAura("Resistance Aura") && (!_wanna_crusader || _wanna_crusader && !Me.Mounted) && IsSpellReady("Resistance Aura"),
                            new Action(delegate
                            {
                                if (Cast("Resistance Aura", "Buff", "Missing aura"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                );
        }
        public Composite Composite_Beacon()
        {
            return (
                new Decorator(ret => unitcheck(tank) && BeaconNeedsRefresh(tank) && tank.Distance<40 && IsSpellReady("Beacon of Light") && tank.InLineOfSight && !Me.Mounted && !_ignore_beacon,
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                        {
                            if (Cast("Beacon of Light", tank, 40, "Buff", "Missing Beacon of light"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        })))
                );
        }
        public Composite Composite_Blessing()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            if (_bless_type == 0)
                            {
                                _should_king = ShouldKing();
                            }
                            else if (_bless_type == 1)
                            {
                                _should_king = true;
                            }
                            else if (_bless_type == 2)
                            {
                                _should_king = false;
                            }
                            BlessTarget = GetBlessTarget();
                            if (_bless_type == 4)
                            {
                                BlessTarget = null;
                            }
                            return RunStatus.Failure;
                        }
            ),
                    new Decorator(ret => _should_king && lastbless != "King",
                        new Action(
                            delegate
                            {
                                slog(Color.Violet, "We should King if needed");
                                lastbless = "King";
                            }
                        )
                    ),
                    new Decorator(ret => !_should_king && lastbless != "Might",

                        new Action(
                            delegate
                            {
                                slog(Color.Violet, "We should Might if needed");
                                lastbless = "Might";
                            }
                        )
                    ),
                    new Decorator(ret => unitcheck(BlessTarget) && ( /*(GotBuff("Mark of the Wild", BlessTarget) || GotBuff("Blessing of Kings", BlessTarget)) &&*/ !GotBuff("Blessing of Might", BlessTarget) && !_should_king && (!Me.Mounted) && IsSpellReady("Blessing of Might")),
                        new Action(delegate
                        {
                            if (Cast("Blessing of Might", BlessTarget, 40, "Buff", "Missing blessing"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        })),
                               new Decorator(ret => unitcheck(BlessTarget) && (!GotBuff("Blessing of Kings", BlessTarget) && !GotBuff("Mark of the Wild", BlessTarget) && _should_king && (!Me.Mounted) && IsSpellReady("Blessing of Kings")),
                                   new Action(delegate
                                   {
                                       if (Cast("Blessing of Kings", BlessTarget, 40, "Buff", "Missing blessing"))
                                       { return RunStatus.Success; }
                                       else { return RunStatus.Failure; }
                                   }))
                    )
                );
        }

        public Composite Composite_Retr_Buff()
        {

            return (
                new PrioritySelector(
                    Composite_Retr_Seal(),
                    Composite_Crusader(),
                    Composite_Retr_Aura(),
                    Composite_Blessing()
                    )
                    );
        }

        public Composite Composite_PVEBuff()
        {
            return (
                new PrioritySelector(
                    Composite_Seal(),
                    Composite_Crusader(),
                    Composite_Aura(),
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
                    Composite_Beacon(),
                    Composite_Blessing()
                    )
                    );
        }
        public Composite Composite_Crusader()
        {
            return (
                new Decorator(ret => _wanna_crusader && Me.Mounted && !IsPaladinAura("Crusader Aura"),

                    new Action(delegate
                    {
                        if (Cast("Crusader Aura", "Buff", "We are mounted, Crudsader aura"))
                        { return RunStatus.Success; }
                        else { return RunStatus.Failure; }
                    }))
                    );
        }
        public Composite Composite_TopOff()
        {
            return (
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
                        new Decorator(ret => !(Me.IsMoving) && tar.HealthPercent > _min_HL_hp && tar.HealthPercent < dontHealAbove && !(tank != null && tank.IsValid && (Me.Mounted && !Me.Combat && !tank.Combat) || ((!Me.Combat && !tar.Combat && tar.Distance > 40))) && IsSpellReady("Holy Light"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Light", tar, 40, "Heal", "Topping people off"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                )));
        }
        public Composite Composite_Judgeformana()
        {
            //slog("sono dentro judge");
            return (
                new Decorator(ret => IsSpellReady("Judgement"),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(delegate
                {
                    if (Cast("Judgement", Enemy, Global_Judgment_range, "Mana", "Judging to rec mana"))
                    { return RunStatus.Success; }
                    else { return RunStatus.Failure; }
                }))
            ));
        }
        public Composite Composite_Mana_Rec()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Me.Combat && unitcheck(Enemy),
                        Composite_Judgeformana()),
                    new Decorator(ret => Me.ManaPercent <= _min_Divine_Plea_mana && Me.Combat && IsSpellReady("Divine Plea"),
                        new PrioritySelector( Composite_Wait_again(), 
                            new Action(delegate
                        {
                            if (Cast("Divine Plea", "Mana", "Noone to heal, need mana"))
                            { return RunStatus.Success; }
                            else { return RunStatus.Failure; }
                        })))
                )
                );
        }
        public Composite Composite_Healing_check_and_buff()
        {
            return (
                new PrioritySelector(
                        new Decorator(ret => tank != null && Me.Mounted && !Me.Combat && !tank.Combat, new Action()),
                        Composite_Beacon(),
                        new Decorator(ret => SealNeedRefresh() && !Me.Mounted && IsSpellReady("Seal of Insight"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Seal of Insight", "Buff", "Missing Seal"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                            new Decorator(ret => SealNeedRefresh() && !Me.Mounted && !SpellManager.HasSpell("Seal of Insight") && IsSpellReady("Seal of Righteousness"),
                            new Wait(4, ret => !IsCastingOrGCD(), 
                                new Action(delegate
                                {
                                    if (Cast("Seal of Righteousness", "Buff", "Missing Seal"))
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
        public Composite Composite_PVPEmergency_button()
        {
            return (
        new PrioritySelector(
            new Decorator(ret => Me.Combat && tar.HealthPercent < _min_ohshitbutton_activator,
                new Sequence(
                    new Action(
                        delegate
                        {
                            if (GotBuff("Divine Plea", Me)) { Lua.DoString("CancelUnitBuff(\"Player\",\"Divine Plea\")"); slog(Color.DarkOrange, "Cancelling Divine Plea due to a Oh Shit! moment"); }
                            //return RunStatus.Failure;
                        }
                    ),
                        Composite_PVEOhShitButton()
                )
            ),
            new Decorator(ret => _wanna_LoH && tar.HealthPercent < _min_LoH_hp && !tar.ActiveAuras.ContainsKey("Forbearance") && IsSpellReady("Lay on Hands") && tar.Distance<40 && tar.InLineOfSight &&(Me.Combat || tar.Combat),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(delegate
                {
                    if (Cast("Lay on Hands", tar, 40, "Heal", "Saving someone life"))
                    { return RunStatus.Success; }
                    else { return RunStatus.Failure; }
                }))),
            new Decorator(ret => _wanna_HoP && tar.HealthPercent < _min_HoP_hp && tar.Guid != tank.Guid && !IsTank(tar) && !tar.ActiveAuras.ContainsKey("Forbearance") && IsSpellReady("Hand of Protection") && tar.Distance < 30 && tar.InLineOfSight && (Me.Combat || tar.Combat) && !tar.HasAura("Alliance Flag") && !tar.HasAura("Horde Flag"),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(delegate
                {
                    if (Cast("Hand of Protection", tar, 30, "Heal", "Saving someone life"))
                    { return RunStatus.Success; }
                    else { return RunStatus.Failure; }
                }))),
            new Decorator(ret => _wanna_HoS && tar.HealthPercent < _min_HoS_hp && tar.Guid == tank.Guid && IsSpellReady("Hand of Sacrifice") && tar.Distance < 30 && tar.InLineOfSight && Me.HealthPercent > 90 && (Me.Combat || tar.Combat),
                new PrioritySelector( Composite_Wait_again(), 
                    new Action(delegate
                {
                    if (Cast("Hand of Sacrifice", tar, 30, "Heal", "I'm fine can Sacrifice"))
                    { return RunStatus.Success; }
                    else { return RunStatus.Failure; }
                })))));

        }
        public Composite Composite_Emergency_button()
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
                                new Decorator(ret=>GotBuff("Divine Plea", Me),
                                new Action(
                                    delegate
                                    {
                                        Lua.DoString("CancelUnitBuff(\"Player\",\"Divine Plea\")"); 
                                        slog(Color.DarkOrange, "Cancelling Divine Plea due to a Oh Shit! moment"); 
                                        return RunStatus.Failure;
                                    }
                                )),
                                    Composite_PVEOhShitButton()
                            )
                        ),
                        new Decorator(ret => _wanna_LoH && tar.HealthPercent < _min_LoH_hp && !tar.ActiveAuras.ContainsKey("Forbearance") && IsSpellReady("Lay on Hands") && tar.Distance < 40 && tar.InLineOfSight && (Me.Combat || tar.Combat),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Lay on Hands", tar, 40, "Heal", "Saving someone life"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_HoP && tar.HealthPercent < _min_HoP_hp && tar.Guid != tank.Guid && !IsTank(tar) && !tar.ActiveAuras.ContainsKey("Forbearance") && IsSpellReady("Hand of Protection") && tar.Distance < 30 && tar.InLineOfSight && (Me.Combat || tar.Combat) && !tar.HasAura("Alliance Flag") && !tar.HasAura("Horde Flag"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Hand of Protection", tar, 30, "Heal", "Saving someone life"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_HoS && tar.HealthPercent < _min_HoS_hp && tar.Guid == tank.Guid && IsSpellReady("Hand of Sacrifice") && tar.Distance < 30 && tar.InLineOfSight && Me.HealthPercent > 90 && (Me.Combat || tar.Combat),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Hand of Sacrifice", tar, 30, "Heal", "I'm fine can Sacrifice"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))));
        }
        public Composite Composite_Actual_Healing()
        {
            return (
                new PrioritySelector(
                    new Action(delegate{
                        slog("Actual healing called! will try to heal tar {0} at hp {1} and distance {2} me.ismoving {3} cancastDL {4} cancastFL {5} cancastHL {6}  Global cooldown {7} left {8}", privacyname(tar), Round(tar.HealthPercent), Round(tar.Distance), Me.IsMoving, CanCast("Divine Light", tar), CanCast("Flash of Light", tar), CanCast("Holy Light", tar), SpellManager.GlobalCooldown, SpellManager.GlobalCooldownLeft);
                        return RunStatus.Failure;
                    }
            ),
                        new Decorator(ret => Me.CurrentHolyPower == 3,
                            new PrioritySelector(
                                new Decorator(ret => IsSpellReady("Light of Dawn") && HolyPowerDump(),
                                    new PrioritySelector( Composite_Wait_again(), 
                                        new Action(delegate
                                    {
                                        if (Cast("Light of Dawn", "Heal", "Healing"))
                                        { return RunStatus.Success; }
                                        else { return RunStatus.Failure; }
                                    }))),
                                new Decorator(ret => IsSpellReady("Word of Glory"),
                                    new PrioritySelector( Composite_Wait_again(), 
                                        new Action(delegate
                                    {
                                        if (Cast("Word of Glory", tar, 40, "Heal", "Healing"))
                                        { return RunStatus.Success; }
                                        else { return RunStatus.Failure; }
                                    })))
                            )),
                        new Decorator(ret => IsSpellReady("Holy Shock"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Shock", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_HR && IsSpellReady("Holy Radiance") && Should_AOE(tar, "Holy Radiance", _min_player_inside_HR, _HR_how_far, _HR_how_much_health),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Radiance", tar, 40, "Heal", "Healing"))
                                {
                                    slog(Color.Orange, "Casted Holy Radiance without Light Infusion at {0} with {1} % HP and {2} / {3} Members around. Distance to Me {4} y", privacyname(tar), Round(tar.HealthPercent), How_Many_Inside_AOE(tar, _min_player_inside_HR, _HR_how_far, _HR_how_much_health), _min_player_inside_HR, tar.Distance);
                                    return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _Inf_of_light_wanna_DL && GotBuff("Infusion of Light") && tar.HealthPercent < _min_Inf_of_light_DL_hp && !(Me.IsMoving) && IsSpellReady("Divine Light"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Light", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _Inf_of_light_wanna_DL && GotBuff("Infusion of Light") && tar.HealthPercent < _min_Inf_of_light_DL_hp && (Me.IsMoving) && IsSpellReady("Flash of Light"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Flash of Light", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => !_Inf_of_light_wanna_DL && GotBuff("Infusion of Light") && !(Me.IsMoving) && IsSpellReady("Holy Light"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Light", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => tar.HealthPercent < _min_FoL_hp && IsSpellReady("Flash of Light") && tar.Distance < 40 && tar.InLineOfSight && !(Me.IsMoving),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Flash of Light", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => tar.HealthPercent < _min_DL_hp && IsSpellReady("Divine Light") && tar.Distance < 40 && tar.InLineOfSight && !(Me.IsMoving),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Light", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => tar.HealthPercent < _min_HL_hp && IsSpellReady("Holy Light") && tar.Distance < 40 && tar.InLineOfSight && !(Me.IsMoving),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Light", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_HR && IsSpellReady("Holy Radiance") && Should_AOE(tar, "Holy Radiance", _min_player_inside_HR, _HR_how_far, _HR_how_much_health),
                            new PrioritySelector(Composite_Wait_again(),
                                new Action(delegate
                                {
                                    if (Cast("Holy Radiance", tar, 40, "Heal", "Healing"))
                                    {
                                        slog(Color.Green, "Casted Holy Radiance with Light Infusion at {0} with {1} % HP and {2} / {3} Members around. Distance to Me {4} y", privacyname(tar), Round(tar.HealthPercent), How_Many_Inside_AOE(tar, _min_player_inside_HR, _HR_how_far, _HR_how_much_health), _min_player_inside_HR, tar.Distance);
                                        return RunStatus.Success;
                                    }
                                    else { return RunStatus.Failure; }
                                }))),
                            new Action(
                                delegate{
                                    slog("no good healing spell found, moving on");
                                    return RunStatus.Failure;
                                }
                                )
                        ));
        }
        public Composite Composite_PVEHealing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => tank != null && tank.IsValid, Composite_Healing_check_and_buff()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Emergency_button()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Actual_Healing())
                    )
                );

        }
        public Composite Composite_PVPHealing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => tank != null && tank.IsValid, Composite_Healing_check_and_buff()),
                    new Decorator(ret => Me.Combat && !combatfrom.IsRunning, Composite_start_timer()),
                    new Decorator(ret => !Me.Combat, Composite_reset_timer()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_PVPEmergency_button()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Actual_Healing())
                    )
                );

        }
        public Composite Composite_reset_timer()
        {
            return (
                new Action(
                    delegate
                    {
                        combatfrom.Reset();
                        return RunStatus.Failure;
                    }
            ));
        }
        public Composite Composite_start_timer()
        {
            return (
                new Action(
                    delegate
                    {
                        combatfrom.Start();
                        return RunStatus.Failure;
                    }
            ));
        }
        public Composite Composite_ChimaeronHealing()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => tank != null && tank.IsValid, Composite_Healing_check_and_buff()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar) && !Global_chimaeron_p1, Composite_Emergency_button()),
                    new Decorator(ret => tank != null && tank.IsValid && unitcheck(tar), Composite_Actual_Healing())
                    )
                );
        }
        public Composite Composite_PVEOhShitButton()
        {
            return (
                new Decorator(ret => !GotBuff("Divine Favor") && !GotBuff("Avenging Wrath") && !GotBuff("Guardian of Ancient Kings") && !GotBuff("Lifeblood"),
                    new PrioritySelector(
                        new Action(
                            delegate{
                                slog("CC LB {0} CC AW {1} CC DF {2} CC GotAK {3}", CanCast("Lifeblood"),CanCast("Avenging Wrath"), CanCast("Divine Favor"),CanCast("Guardian of Ancient Kings"));
                                return RunStatus.Failure;
                            }),
                        new Decorator(ret => _wanna_lifeblood && IsSpellReady("Lifeblood"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Lifeblood", "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_AW && IsSpellReady("Avenging Wrath"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Avenging Wrath", "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_DF && IsSpellReady("Divine Favor"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Favor", "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => _wanna_GotAK && IsSpellReady("Guardian of Ancient Kings"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Guardian of Ancient Kings", "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                        )
                        )
                        );
        }

        public Composite Composite_Judge()
        {
            return (
                new Decorator(ret => (IsSpellReady("Judgement") && (!GotBuff("Judgements of the Pure") || Me.ManaPercent < _mana_judge) && unitcheck(Enemy) && Enemy.Distance < Global_Judgment_range && Enemy.InLineOfSight) && !GotBuff("Hand of Protection"),
            new PrioritySelector( Composite_Wait_again(), 
                new Action(delegate
            {
                if (Cast("Judgement", Enemy, Global_Judgment_range, "Buff", "Missing Judgment of the Pure"))
                { return RunStatus.Success; }
                else { return RunStatus.Failure; }
            }))
                    ));
        }
        public Composite Composite_BGSelf()
        {
return (
                new PrioritySelector(/*
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
                        ),*/
                    new Action(
                        delegate{
                            slog("BGself");
                            return RunStatus.Failure;
                        }
            ),
                    Composite_Trinkets(),
                        new Decorator(ret => _wanna_mana_potion && Me.ManaPercent <= _min_mana_potion, Use_mana_pot()),
                        new Decorator(ret => _wanna_DP && Me.HealthPercent < _min_DP_hp && Me.Combat && IsSpellReady("Divine Protection"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Protection", "Self", "Low HP"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => (_wanna_DS && !Me.HasAura("Alliance Flag") && !Me.HasAura("Horde Flag") && Me.HealthPercent < _min_DS_hp && !GotBuff("Forbearance") && Me.Combat && IsSpellReady("Divine Shield") || ((GotBuff("Blind") || Me.Stunned || GotBuff("Polymorph")) && unitcheck(tar) && tar.HealthPercent < _min_DS_hp)),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Shield", "Self", "Low HP"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                            new Decorator(ret => _wanna_HS_on_CD && Me.Combat, Composite_tier_bonuses())
                ));

        }
                public Composite Composite_Trinkets()
        {
            return(
                new PrioritySelector(
                    new Decorator(ret=>Trinket1!=null && _trinket1_usable && !_trinket1_passive && _trinket1_use_when==1 && unitcheck(tar) && tar.HealthPercent<_min_ohshitbutton_activator,
                        new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate{
                                slog(Color.Orange, "{0} is ready and {2} is at {3} % so using it!", Trinket1.Name, privacyname(tar), Round(tar.HealthPercent));
                                Trinket1.Use();
                                Trinket1_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                            new Decorator(ret => Trinket2 != null && _trinket2_usable && !_trinket2_passive && _trinket2_use_when == 1 && unitcheck(tar) && tar.HealthPercent < _min_ohshitbutton_activator,
                                new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "{0} is ready and {2} is at {3} % so using it!", Trinket2.Name, privacyname(tar), Round(tar.HealthPercent));
                                Trinket2.Use();
                                Trinket2_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                            new Decorator(ret => Trinket1 != null && _trinket1_usable && !_trinket1_passive && _trinket1_use_when == 2 && Me.ManaPercent < _min_mana_rec_trinket,
                                new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "{0} is ready and my mana is at {1} using it!", Trinket1.Name, Round(Me.ManaPercent));
                                Trinket1.Use();
                                Trinket1_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                            new Decorator(ret => Trinket2 != null && _trinket2_usable && !_trinket2_passive && _trinket2_use_when == 2 && Me.ManaPercent < _min_mana_rec_trinket,
                                new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "{0} is ready and my mana is at {1} using it!", Trinket2.Name, Round(Me.ManaPercent));
                                Trinket2.Use();
                                Trinket2_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                            new Decorator(ret => Trinket1 != null && _trinket1_usable && !_trinket1_passive && _trinket1_use_when == 3 && Me.Stunned && !GotBuff("Charge Stun") && !GotBuff("Sap"),
                                new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "I'm Stunned! {0} is ready using it!", Trinket1.Name);
                                Trinket1.Use();
                                Trinket1_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                            new Decorator(ret => Trinket2 != null && _trinket2_usable && !_trinket2_passive && _trinket2_use_when == 3 && Me.Stunned && !GotBuff("Charge Stun") && !GotBuff("Sap"),
                                new PrioritySelector( Composite_Wait_again(), 
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "I'm Stunned! {0} is ready using it!", Trinket2.Name);
                                Trinket2.Use();
                                Trinket2_sw.Start();
                                return RunStatus.Failure;
                            }
                            )))
                    )
                );
        }
        public Composite Composite_Self()
        {
            return (
                new PrioritySelector(/*
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
                        ),*/
                    Composite_Trinkets(),
                        new Decorator(ret => _wanna_mana_potion && Me.ManaPercent <= _min_mana_potion, Use_mana_pot()),
                        new Decorator(ret => _wanna_DP && Me.HealthPercent < _min_DP_hp && Me.Combat && IsSpellReady("Divine Protection"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Protection", "Self", "Low HP"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => (_wanna_DS && !Me.HasAura("Alliance Flag") && !Me.HasAura("Horde Flag") && Me.HealthPercent < _min_DS_hp && !GotBuff("Forbearance") && Me.Combat && IsSpellReady("Divine Shield") || ((GotBuff("Blind") || Me.Stunned || GotBuff("Polymorph")) && unitcheck(tar) && tar.HealthPercent < _min_DS_hp)),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Divine Shield", "Self", "Low HP"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                            new Decorator(ret=> _wanna_HS_on_CD && Me.Combat, Composite_tier_bonuses())
                ));
        }

        public Composite Use_mana_pot()
        {
            return (
                new Action(delegate
                {
                    return (
                     RunStatus.Failure
                    );
                })
            );
        }

        public Composite Composite_tier_bonuses()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret=>unitcheck(tar) &&IsSpellReady("Holy Shock") && tar.Distance<40,//CanCast("Holy Shock", tar) /*&& !SpellManager.Spells["Holy Shock"].Cooldown*/,
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Shock", tar, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                            new Decorator(ret=>!unitcheck(tar) && unitcheck(tank) &&IsSpellReady("Holy Shock") && tank.Distance<40,//CanCast("Holy Shock", tar) /*&& !SpellManager.Spells["Holy Shock"].Cooldown*/,
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Shock", tank, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            }))),
                            new Decorator(ret=>!unitcheck(tar) && !unitcheck(tank) && unitcheck(Me) &&IsSpellReady("Holy Shock"),//CanCast("Holy Shock", tar) /*&& !SpellManager.Spells["Holy Shock"].Cooldown*/,
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Holy Shock", Me, 40, "Heal", "Healing"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                            )
            );
        }

        public Composite Composite_MountUp()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => unitcheck(Me) && unitcheck(tank) && tank.Mounted && !Me.Mounted && !Me.Combat && !tank.Combat && Me != tank && Mount.CanMount(),
                        new Action(delegate
                        {
                            if (Me.IsMoving) { WoWMovement.MoveStop(); }
                            new Wait(1, ret => !Me.IsMoving, new Action());
                            Mount.MountUp();
                            new Wait(4, ret => Me.Mounted, new Action());
                            slog(Color.DarkOrange, "Mounting UP to follow the tank!");
                        }
                        )
            ),
                        new Decorator(ret => unitcheck(Me) && unitcheck(tank) && Me.Mounted && !Me.Combat && !tank.Combat,
                        new Action(delegate
                        {
                            slog(Color.DarkOrange, "We are mounted... chill");
                            return RunStatus.Failure;
                        }
                        )
            ),
            new Action(delegate
            {
                noncombatfrom.Reset();
                noncombatfrom.Start();
                return RunStatus.Failure;
            }
            )
                )
                );
        }

        public Composite Composite_NeedtoRest()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Me.ManaPercent > 20 && usedBehaviour != "Battleground" && !Me.Combat && !tank.Combat, Resurrecting()),
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
        public Composite Composite_ARENANeedtoRest()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Me.ManaPercent > 20 && usedBehaviour != "Battleground" && !Me.Combat && !tank.Combat, Resurrecting()),
                    new Decorator(ret => (Me.ManaPercent <= _rest_if_mana_below) && (!Me.Combat) && (!Me.HasAura("Drink")) && (!Me.IsMoving) && (!Me.Mounted),
                        new Action(delegate
                        {
                            slog(Color.Blue, "#Out of Combat - Mana is at {0} %. Time to drink.#", Round(Me.ManaPercent));
                            Styx.Logic.Common.Rest.Feed();
                            return RunStatus.Success;
                        })),
                        new Decorator(ret => (!Me.Combat) && (!tank.Combat || tank.HealthPercent>80) && (Me.HasAura("Drink") && ((Me.ManaPercent < 95) || (!GotBuff("Well Fed") && Me.ActiveAuras["Drink"].TimeLeft.TotalSeconds > 19))),
                            new Action(delegate
                            {
                                slog(Color.Blue, "#Out of Combat - Drinking but no Well Fed buff, will wait 10 sec", Round(Me.ManaPercent));
                                //return RunStatus.Success;
                                new Wait(10, ret => !GotBuff("Well Fed"), new Action(delegate { return RunStatus.Success; }));
                            }))
                ));
        }
        public Composite Resurrecting()
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
                                slog("I'd want to ress {0} at distance {1} but is too far away or out of loss", privacyname(RessTarget),Round(RessTarget.Distance));
                                return RunStatus.Failure;
                            }
            )),
            new Decorator(ret => SpellManager.HasSpell("Redemption") &&  CanCast("Redemption", RessTarget),
                new Action(delegate
                {
                    if (castaress("Redemption", RessTarget, 30, "Buff", "Ressing"))
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
        public Composite Composite_target_enemy()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => _wanna_target && (Me.CurrentTarget == null || !Me.CurrentTarget.IsValid || Me.CurrentTarget.Dead),
                new PrioritySelector(
                new Decorator(ret => unitcheck(Enemy),
                    new Action(
                        delegate
                        {
                            Enemy.Target();
                        }
            )),
            new Decorator(ret => _wanna_target && unitcheck(tank.CurrentTarget),
                new Action(
                    delegate
                    {
                        tank.CurrentTarget.Target();
                    }
            ))))
                )
                );
        }
        public Composite Composite_RAF_Find_Enemy()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            Enemy = RAFGiveEnemy(29);
                            return RunStatus.Failure;
                        }
            ),
            Composite_target_enemy()
            )
            );
        }
        public Composite Composite_Find_Enemy()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate
                        {
                            Enemy = PVEGiveEnemy(29);
                            return RunStatus.Failure;
                        }
            ),
            Composite_target_enemy()
            )
            );
        }
        public Composite Composite_print_tar()
        {
            return (
                new Action(delegate
            {/*
                populate_nameofRM();
                populate_raidsubg();
                for (i = 1; i < 41; i++)
                {
                    slog("unit number {0} name {1} subgroup {2} healraidmember {3}, healornot {4}", i, NameorRM[i], Raidsbugroup[i],_heal_raid_member[i-1],healornot[i]);
                }
                new Wait(4, ret => false,new Action());
              * */
                
                if (unitcheck(tar))
                {
                    slog("tar {0} hp {1} distance {2} tank {3} hp {4} distance {5}", privacyname(tar), Round(tar.HealthPercent),Round(tar.Distance), privacyname(tank), Round(tank.HealthPercent), Round(tank.Distance));
                }
                else if (!Me.Mounted) { slog("no tar found"); }
                return RunStatus.Failure;
            }));
        }

        public Composite Composite_Find_unit_to_heal()
        {
            return (
                new Action(delegate
                {
                    tar = GetHealTarget();
                    return RunStatus.Failure;
                }
                ));
        }
        public Composite Composite_CheckMe()
        {
            return (
                new PrioritySelector(
                    new Action(
                        delegate{
                            Check_Trinket();
                            return RunStatus.Failure;
                        }),
                new Decorator(ret => Me.IsCasting && (usedBehaviour == "Dungeon" || usedBehaviour=="Raid") ,
                    Composite_Interrupt())
                    ,Composite_Wait()
                    /*,
                    new Action(delegate{
                        slog("{0}: GCD {1} GCDleft {2} HL.CD {3} HL.CDLeft {4} WowLatency {5} CCtl {6} HSCD {7}", i++, SpellManager.GlobalCooldown, SpellManager.GlobalCooldownLeft.TotalMilliseconds, SpellManager.Spells["Holy Light"].Cooldown, SpellManager.Spells["Holy Light"].CooldownTimeLeft.TotalMilliseconds, StyxWoW.WoWClient.Latency,Me.CurrentCastTimeLeft.TotalMilliseconds, SpellManager.Spells["Holy Shock"].CooldownTimeLeft.TotalMilliseconds);
                        return RunStatus.Failure;
                    })*/
                    //FIXME: REMOVE THIS!!
                
                
                )
                );
        }

        public Composite Composite_Wait()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret=>_intellywait && myclass==0,
                    new Wait(4, ret => ((!SpellManager.GlobalCooldown && !Me.IsCasting) || (SpellManager.GlobalCooldown && !Me.IsCasting && SpellManager.Spells["Holy Light"].CooldownTimeLeft.TotalMilliseconds < _how_much_wait + Latency + Math.Min(_precasting + Latency, 350)) || (Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds < _how_much_wait + Latency + Math.Min(_precasting + Latency, 350)) || !IsCastingOrGCD()),
                        new Action(
                            delegate
                            {
                                //slog("GCD && casting {0} GCD {1} casting {2} iscastingorgcd {3}", (!SpellManager.GlobalCooldown && !Me.IsCasting), (SpellManager.GlobalCooldown && SpellManager.Spells["Holy Light"].CooldownTimeLeft.TotalMilliseconds < _how_much_wait + 2U * StyxWoW.WoWClient.Latency + _precasting), (Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds < _how_much_wait + 2U * StyxWoW.WoWClient.Latency + _precasting), !IsCastingOrGCD());
                                //slog("how much {0}", _how_much_wait + 2U * StyxWoW.WoWClient.Latency + _precasting);
                                return RunStatus.Failure;
                            }
            ))),
            new Decorator(ret => _intellywait && myclass == 1,
                    new Wait(4, ret => ((!SpellManager.GlobalCooldown && !Me.IsCasting) || (SpellManager.GlobalCooldown && !Me.IsCasting && SpellManager.Spells["Heal"].CooldownTimeLeft.TotalMilliseconds < _how_much_wait + Latency + Math.Min(_precasting + Latency, 350)) || (Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds < _how_much_wait + Latency + Math.Min(_precasting + Latency, 350)) || !IsCastingOrGCD()),
                        new Action(
                            delegate
                            {
                                //slog("GCD && casting {0} GCD {1} casting {2} iscastingorgcd {3}", (!SpellManager.GlobalCooldown && !Me.IsCasting), (SpellManager.GlobalCooldown && SpellManager.Spells["Holy Light"].CooldownTimeLeft.TotalMilliseconds < _how_much_wait + 2U * StyxWoW.WoWClient.Latency + _precasting), (Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds < _how_much_wait + 2U * StyxWoW.WoWClient.Latency + _precasting), !IsCastingOrGCD());
                                //slog("how much {0}", _how_much_wait + 2U * StyxWoW.WoWClient.Latency + _precasting);
                                return RunStatus.Failure;
                            }
            ))),
            new Decorator(ret=>!_intellywait,
                new PrioritySelector(
                    new Decorator(ret=>!_decice_during_GCD,
            new Wait(4, ret => ((!SpellManager.GlobalCooldown && !Me.IsCasting) || !IsCastingOrGCD()),
                    new Action(
                        delegate
                        {
                            return RunStatus.Failure;
                        }
                        ))
                        ),
                        new Decorator(ret=>_decice_during_GCD,
                            new Action(
                        delegate
                        {
                            return RunStatus.Failure;
                        }
                        )
                        )
                        )),
                        new Decorator(ret=>performance.IsRunning,
                            new Action(
                                delegate{
                                    if (Me.Combat)
                                    {
                                        slog("Performance: nothing decided on last cicle in {0} milliseconds", performance.Elapsed.TotalMilliseconds);
                                        Print_status();
                                    }
                                    performance.Reset();
                                    return RunStatus.Failure;
                                })),
                                new Decorator(ret => !performance.IsRunning,
                            new Action(
                                delegate
                                {
                                    performance.Start();
                                    return RunStatus.Failure;
                                }))
                        )
                );
        }

        public Composite Composite_Wait_again()
        {
               return (
                new PrioritySelector(
                                new Decorator(ret => !performance.IsRunning,
                            new Action(
                                delegate
                                {
                                    slog("Performance: ERROR! the timer was not running!");
                                    return RunStatus.Failure;
                                })),
                        new Decorator(ret=>performance.IsRunning,
                            new Action(
                                delegate{
                                    this_casted = performance.Elapsed.TotalMilliseconds;
                                    allcasttaked += this_casted;
                                    how_many_cast++;
                                    medium_time_to_decide = allcasttaked / (double)how_many_cast;
                                    //slog("Performance: spell decided in {0} milliseconds, average on {1} casts: {2}, Latency {3}", this_casted, how_many_cast, medium_time_to_decide, Latency);
                                    //slog("Performance: first wait {0} second wait {1} third wait {2}", Latency + Math.Min(_precasting + Latency, 350), 200 + Latency + Math.Min(_precasting + Latency, 350), medium_time_to_decide + Latency + Math.Min(_precasting + Latency, 350));
                                    if (max_allcasted < this_casted && how_many_cast>2)
                                    {
                                        max_allcasted = this_casted;
                                        slog("Performance: we have a new Max! {0}", max_allcasted);
                                    }
                                    _how_much_wait = (int)medium_time_to_decide;
                                    performance.Reset();
                                    return RunStatus.Failure;
                                }))
                                ,
                    new Wait(4, ret => !IsCastingOrGCD(),
                        new Action(
                            delegate{
                                return RunStatus.Failure;
                            }
            ))
                        )
                );
        }

        public Composite Composite_Arena_SetTank()
        {
            return (
                new Action(
                    delegate
                    {
                        mtank = GetTank();
                        if (mtank == null) { tank = Me; return RunStatus.Failure; }
                        if (combatfrom.Elapsed.TotalSeconds > 2)
                        {
                            //if (tank != null && Me != null && tank.IsValid && Me.IsValid && !tank.Dead && !Me.Dead)
                            if (unitcheck(Me) && unitcheck(mtank))
                            {
                                if (Me.HealthPercent <
                                    (mtank.HealthPercent - 20))//&& CanCast("Hand of Salvation"))
                                {
                                    slog(Color.Black, "Changing Tank to Me couse they are beating on me!");
                                    tank = Me;
                                    return RunStatus.Failure;
                                }
                                else
                                {
                                    tank = mtank;
                                    return RunStatus.Failure;
                                }
                            }
                            else if (unitcheck(Me))
                            {
                                tank = Me;
                                return RunStatus.Failure;
                            }
                            combatfrom.Reset();
                            combatfrom.Start();
                            slog(Color.DarkRed, "We are in arena, but i'm not valid, CC is in PAUSE");
                            return RunStatus.Failure;
                        }
                        else
                        {
                            tank = mtank;
                            return RunStatus.Failure;
                        }
                    }));
        }
        public Composite Composite_SetTank()
        {
            return (
                new Action(delegate
                {
                    tank = GetTank();
                    if (tank == null) { tank = Me; }
                    return RunStatus.Failure;
                })
            );
        }
        public bool unitcheck(WoWUnit unit)
        {
            return unit != null && unit.IsValid && !unit.Dead;
        }
        public bool unitcheck(WoWPlayer unit)
        {
            return unit != null && unit.IsValid && !unit.Dead && !unit.IsGhost;
        }

        public Composite Composite_Solo_Pull()
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
                                  slog("Moving towards:{0}",privacyname(Me.CurrentTarget));
                                  Navigator.MoveTo(Me.CurrentTarget.Location);
                              })),
                                              // Stop moving if we are moving
                new Decorator(ret => _wanna_move && Me.IsMoving,
                              new Action(ret => WoWMovement.MoveStop())),

                new Decorator(ret => _wanna_move && Me.GotTarget && !Me.IsFacing(Me.CurrentTarget),
                              new Action(ret => WoWMovement.Face())
                    ),
                    new Decorator(ret=>unitcheck(Me.CurrentTarget) && !Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.IsPlayer && !Me.CurrentTarget.IsPet,
                    new Action(
                        delegate{
                                Enemy = Me.CurrentTarget;
                                slog("Checks Done, all green, PULLING {0} NOW!",privacyname(Enemy));
                            return RunStatus.Failure;
                        })),
                        Composite_Solo_Combat())
                    )
                    );
        }
        public Composite Composite_Universal_Ghost()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret=>Me.IsGhost,
                new Action(
                    delegate{
                        return RunStatus.Failure;
                    })
                ),
                new Decorator(ret=>!Me.IsGhost,
                    new Action(
                        delegate{
                            CreateBehaviors();
                            return RunStatus.Success;
                        }
                        )
                )));
        }

        public Composite Composite_Retribution_Simple()
        {
            return (
                new PrioritySelector(
                    Composite_CheckMe(),
                    Composite_Retr_SetTank(),
                    Composite_Retr_Find_Enemy(),
                    Composite_Blessing(),
                    Composite_Retr_Seal(),
                    new Decorator(ret=>Me.Combat || (unitcheck(tank) && tank.Combat),
                        new PrioritySelector(
                    Composite_Retr_CD(),
                    Composite_Retr_Rotation()
                    ))
                    )
                    );
        }

        public Composite Composite_Retr_Seal()
        {
            return(
                new Decorator(ret=>(!GotBuff("Seal of Truth") || Me.Buffs["Seal of Truth"].TimeLeft.TotalSeconds<5) && IsSpellReady("Seal of Truth"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Seal of Truth", "Buff", "Need Seal"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))
                );
        }

        public Composite Composite_Retr_CD()
        {
            return (
                new PrioritySelector(
                new Decorator(ret => !GotBuff("Avenging Wrath") && !GotBuff("Guardian of Ancient Kings") && !GotBuff("Lifeblood") && !GotBuff("Zealotry") && unitcheck(Enemy) && Me.Combat && Enemy.IsWithinMeleeRange,
                        new Decorator(ret => IsSpellReady("Guardian of Ancient Kings") && IsSpellReady("Avenging Wrath") && IsSpellReady("Zealotry") && Me.CurrentHolyPower==1 && GotBuff("Inquisition") && Me.Buffs["Inquisition"].TimeLeft.TotalSeconds>=18,
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Guardian of Ancient Kings", "OhShit", "Need more power"))
                                { return RunStatus.Success; }
                                else { return RunStatus.Failure; }
                            })))),
                            new Decorator(ret => (Me.CurrentHolyPower == 3 || GotBuff("Divine Purpose")) && ((GotBuff("Guardian of Ancient Kings") && Me.Buffs["Guardian of Ancient Kings"].TimeLeft.TotalSeconds <= 20) || (SpellManager.HasSpell("Guardian of Ancient Kings") && SpellManager.Spells["Guardian of Ancient Kings"].CooldownTimeLeft.TotalSeconds > 121 && !GotBuff("Guardian of Ancient Kings")) || (!SpellManager.HasSpell("Guardian of Ancient Kings"))),
                                new PrioritySelector(
                                    new Decorator(ret => Trinket1 != null && _trinket1_usable && !_trinket1_passive,
                        new Wait(4, ret => !IsCastingOrGCD(),
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "{0} is ready so using it!", Trinket1.Name);
                                Trinket1.Use();
                                Trinket1_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                            new Decorator(ret => Trinket2 != null && _trinket2_usable && !_trinket2_passive,
                                new Wait(4, ret => !IsCastingOrGCD(),
                        new Action(
                            delegate
                            {
                                slog(Color.Orange, "{0} is ready so using it!", Trinket2.Name);
                                Trinket2.Use();
                                Trinket2_sw.Start();
                                return RunStatus.Failure;
                            }
                            ))),
                        new Decorator(ret => IsSpellReady("Lifeblood"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Lifeblood", "OhShit", "Need more power"))
                                { return RunStatus.Failure; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => IsSpellReady("Avenging Wrath"),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Avenging Wrath", "OhShit", "Need more power"))
                                { return RunStatus.Failure; }
                                else { return RunStatus.Failure; }
                            }))),
                        new Decorator(ret => IsSpellReady("Zealotry") && (Me.CurrentHolyPower == 3 || GotBuff("Divine Purpose")),
                            new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                            {
                                if (Cast("Zealotry", "OhShit", "Need more power"))
                                { return RunStatus.Failure; }
                                else { return RunStatus.Failure; }
                            })))
                            )
                        )
                        )
                        );
        
        }

        public Composite Composite_Retr_SetTank()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret=>Me.IsInParty || Me.IsInRaid,
                    Composite_SetTank()
                    ),
            Composite_Solo_SetTank()
                    )
                    );
        }

        public Composite Composite_Retr_Find_Enemy()
        {
            return (
                new PrioritySelector(
                    new Decorator(ret => Me.IsInParty || Me.IsInRaid,
                    Composite_Find_Enemy()
                    ),
            new Decorator(ret=> !Me.IsInParty && !Me.IsInRaid && unitcheck(Me.CurrentTarget),
                    new Action(
                delegate{
                    Enemy=Me.CurrentTarget;
                    return RunStatus.Failure;
                }
                )
                    ),
                    new Decorator(ret=>!unitcheck(Me.CurrentTarget),
                        new Action(
                            delegate{
                        Enemy=null;
                            }
                            )
            )
            )
                    );
        }

        public Composite Composite_Keep_Inquisition()
        {
            return(
                new Decorator(ret => IsSpellReady("Inquisition") && ((!GotBuff("Inquisition", Me) && (GotBuff("Divine Purpose",Me) || Me.CurrentHolyPower>0)) || (Me.CurrentHolyPower ==3 && !GotBuff("Divine Purpose",Me) && GotBuff("Inquisition") && Me.Buffs["Inquisition"].TimeLeft.TotalSeconds <= 2)),
                        new PrioritySelector( Composite_Wait_again(), 
                                new Action(delegate
                                {
                                    if (Cast("Inquisition", Me, "Buff", "Missing Inquisition"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                })))
                );
        }

        public Composite Composite_Retr_Rotation()
        {
            return (
                new PrioritySelector(
                    Composite_Keep_Inquisition(),
                    new Decorator(ret => Me.CurrentHolyPower < 3 && IsSpellReady("Crusader Strike") && unitcheck(Enemy) && Enemy.IsWithinMeleeRange,
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Crusader Strike", Enemy, 5, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                })))
                                ,
                                new Decorator(ret => (Me.CurrentHolyPower == 3 || GotBuff("Divine Purpose",Me)) && IsSpellReady("Templar's Verdict") && unitcheck(Enemy) && Enemy.IsWithinMeleeRange && !(GotBuff("Guardian of Ancient Kings") && Me.Buffs["Guardian of Ancient Kings"].TimeLeft.TotalSeconds >= 20),
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Templar's Verdict", Enemy, 5, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => GotBuff("The Art of War",Me) && Enemy.Distance<20 && IsSpellReady("Exorcism") && unitcheck(Enemy),
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Exorcism", Enemy, 20, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => unitcheck(Enemy) && Enemy.Distance<30 && (GotBuff("Avenging Wrath", Me) || Enemy.HealthPercent < 20) && IsSpellReady("Hammer of Wrath"),
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Hammer of Wrath", Enemy, 30, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => unitcheck(Enemy) && IsSpellReady("Judgement") && Enemy.Distance<Global_Judgment_range,
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Judgement", Enemy, Global_Judgment_range, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => unitcheck(Enemy) && IsSpellReady("Holy Wrath") && Enemy.Distance < 10 && Me.ManaPercent>25,
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Holy Wrath", Enemy, 10, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => unitcheck(Enemy) && IsSpellReady("Consecration") && Enemy.Distance < 8 && Me.ManaPercent > 60,
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Consecration", Enemy, 8, "DPS", "Rotation"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                }))),
                                new Decorator(ret => unitcheck(Enemy) && IsSpellReady("Divine Plea") && Enemy.Distance < 8 && Me.ManaPercent < 40,
                        new Wait(4, ret => !IsCastingOrGCD(),
                                new Action(delegate
                                {
                                    if (Cast("Divine Plea", Me, "Buff", "Needing Mana"))
                                    { return RunStatus.Success; }
                                    else { return RunStatus.Failure; }
                                })))
                    )
                    );
        }

        public Composite Composite_Pull_Selector()
        {
            slog("sono dentro composite Pull selector");
            return new PrioritySelector(
                new Decorator(ret => !unitcheck(Me) && !Me.IsGhost, new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Failure;
                })),
                                                new Decorator(ret => _Stop_Healing,
                    new Action(
                        delegate
                        {
                            slog(Color.DarkRed, "You Selected to STOP all Healing, press again to start healing again");
                            return RunStatus.Failure;
                        }
            )),
                new Decorator(ret => (unitcheck(Me) || Me.IsGhost) && !_Stop_Healing,
                    new PrioritySelector(
                            new Decorator(ret=>!_enable_pull || (_enable_pull && usedBehaviour!="Solo"),
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
                Composite_Solo()
                ),
                new SwitchArgument<string>("Party or Raid",
                Composite_Party_or_Raid()
                ),
                new SwitchArgument<string>("Arena",
                Composite_Arena()
                ),
                new SwitchArgument<string>("World PVP",
                Composite_WorldPVP()
                ),
                new SwitchArgument<string>("Battleground",
                Composite_Battleground()
                ),
                new SwitchArgument<string>("Dungeon",
                Composite_Dungeon()
                ),
                new SwitchArgument<string>("Raid",
                Composite_Raid()
                ),
                new SwitchArgument<string>("Retribution",
                    Composite_Retribution_Simple()
                    )
                )),
                new Decorator(ret=>_enable_pull && usedBehaviour=="Solo",
                    Composite_Solo_Pull()
                    )))
          );
        }

        public Composite Composite_Rest_Selector()
        {
            //return new PrioritySelector(
            slog("sono dentro composite selector");
            return new PrioritySelector(
                new Decorator(ret => !unitcheck(Me) && !Me.IsGhost, new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Failure;
                })),
                                new Decorator(ret => _Stop_Healing,
                    new Action(
                        delegate
                        {
                            slog(Color.DarkRed, "You Selected to STOP all Healing, press again to start healing again");
                            return RunStatus.Failure;
                        }
            )),
                new Decorator(ret => (unitcheck(Me) || Me.IsGhost) && !_Stop_Healing,
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
                Composite_SoloRest()
                ),
                new SwitchArgument<string>("Party or Raid",
                Composite_PVERest()
                ),
                new SwitchArgument<string>("Arena",
                Composite_PVPRest()
                ),
                new SwitchArgument<string>("World PVP",
                Composite_PVPRest()
                ),
                new SwitchArgument<string>("Battleground",
                Composite_PVPRest()
                ),
                new SwitchArgument<string>("Dungeon",
                Composite_PVERest()
                ),
                new SwitchArgument<string>("Raid",
                Composite_PVERest()
                //Composite_Raid_Selective_healing()
                
                )
                ,
                new SwitchArgument<string>("Retribution",
                    Composite_Retr_Rest()
                    )
                ))
          );
        }
        public Composite Composite_Selector()
        {
            //return new PrioritySelector(
            slog("sono dentro composite selector");
            return new PrioritySelector(
                new Decorator(ret => !unitcheck(Me) && !Me.IsGhost, new Action(delegate
                {
                    slog("We are on a loading schreen or dead or a gost, CC is in PAUSE!");
                    return RunStatus.Failure;
                })),
                new Decorator(ret=>_Stop_Healing,
                    new Action(
                        delegate{
                            slog(Color.DarkRed,"You Selected to STOP all Healing, press again to start healing again");
                            return RunStatus.Failure;
                        }
            )),
                new Decorator(ret=>(unitcheck(Me) || Me.IsGhost) && !_Stop_Healing,
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
                Composite_Solo()
                ),
                new SwitchArgument<string>("Party or Raid",
                Composite_Party_or_Raid()
                ),
                new SwitchArgument<string>("Arena",
                Composite_Arena()
                ),
                new SwitchArgument<string>("World PVP",
                Composite_WorldPVP()
                ),
                new SwitchArgument<string>("Battleground",
                Composite_Battleground()
                ),
                new SwitchArgument<string>("Dungeon",
                Composite_Dungeon()
                ),
                new SwitchArgument<string>("Raid",
                Composite_Raid()
                )
                ,
                new SwitchArgument<string>("Retribution",
                    Composite_Retribution_Simple()
                    )
                ))
          );
        }
    }
}
