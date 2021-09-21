/*
 * NOTE:    DO NOT POST ANY MODIFIED VERSIONS OF THIS TO THE FORUMS.
 * 
 *          DO NOT UTILIZE ANY PORTION OF THIS COMBAT CLASS WITHOUT
 *          THE PRIOR PERMISSION OF AUTHOR.  PERMITTED USE MUST BE
 *          ACCOMPANIED BY CREDIT/ACKNOWLEDGEMENT TO ORIGINAL AUTHOR.
 * 
 * ShamWOW Shaman CC
 * 
 * Author:  Bobby53
 * 
 * See the ShamWOW.chm file for Help
 *
 */

using System.Drawing;
using System.Linq;
using System.Xml.Linq;

using Styx.Helpers;

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Styx.WoWInternals;
using System.Collections.Generic;

namespace Bobby53
{

    /*************************************************************************
         * private class that manages defaults and access to configurable values.
         * supports loading values from XML file
         *************************************************************************/
    public class ConfigValues
    {
        public enum TypeOfPull { Fast, Ranged, Body, Auto }
        public enum PveCombatStyle { Normal, FarmingLowLevelMobs }
        public enum PvpCombatStyle { CombatOnly, HealingOverCombat, HealingOnly }
        public enum RafCombatStyle { Auto, CombatOnly, HealingOverCombat, HealingOnly }
        public enum RafHealStyle { Auto, TankOnly, RaidOnly };
        public enum SpellInterruptStyle { None, CurrentTarget, All };
        public enum SpellPriority { None, High, Low };
        public enum RaidTarget    { None, Star, Circle, Diamond, Triangle, Moon, Square, Cross, Skull };

        public const int PullTimeout = 15000;
        public const int waitWaitAfterRangedPull = 250;
        public const bool NearbyPlayerWarnings = false;
        public const bool BeepIfPlayerTargetsMe = false;

        public const double TargetTooCloseDistance = 1.75;
        public const double TargetTooCloseAdjust = 0.0;

        public string CreatedByVersion = "";

        public bool Debug = true;
        public bool UseGhostWolfForm = true;
        public int DistanceForGhostWolf = 35;
        public int RestHealthPercent = 60;
        public int RestManaPercent = 49;
        public int NeedHealHealthPercent = 50;
        public int EmergencyHealthPercent = 35;
        public int EmergencyManaPercent = 15;
        public int LifebloodPercent = 70;
        public int ShamanisticRagePercent = 65;
        public int ThunderstormPercent = 65;
        public int ManaTidePercent = 65;
        public int TrinkAtHealth = 50;
        public int TrinkAtMana = 65;
        public bool UseBandages = true;
        public int DistanceForTotemRecall = 30;
        public int TwistManaPercent = 25;    // was 65
        public int TwistDamagePercent = 50;  // was 85
        public bool DisableMovement = false;
        public bool DisableTargeting = true;
        public bool MeleeCombatBeforeLevel10 = false;
        public SpellInterruptStyle InterruptStyle = SpellInterruptStyle.All;
        public bool DetectImmunities = true;
        public bool WaterWalking = true;

        public PveCombatStyle PVE_CombatStyle = PveCombatStyle.Normal;
        public TypeOfPull PVE_PullType = TypeOfPull.Auto;
        public bool PVE_SaveForStress_Bloodlust = true;
        public bool PVE_SaveForStress_DPS_Racials = false;
        public bool PVE_SaveForStress_ElementalTotems = true;
        public bool PVE_SaveForStress_FeralSpirit = true;
        public bool PVE_SaveForStress_TotemsSelected = false;
        public int PVE_LevelsAboveAsElite = 3;
        public int PVE_StressfulMobCount = 2;
        public string PVE_TotemEarth = "Auto";
        public string PVE_TotemFire = "Auto";
        public string PVE_TotemWater = "Auto";
        public string PVE_TotemAir = "Auto";
        public string PVE_MainhandImbue = "Auto";
        public string PVE_OffhandImbue = "Auto";

        public PvpCombatStyle PVP_CombatStyle = PvpCombatStyle.CombatOnly;
        public string PVP_TotemEarth = "Auto";
        public string PVP_TotemFire = "Auto";
        public string PVP_TotemWater = "Auto";
        public string PVP_TotemAir = "Auto";
        public string PVP_MainhandImbue = "Auto";
        public string PVP_OffhandImbue = "Auto";

        public SpellPriority  PVP_CleansePriority = SpellPriority.Low;
        public SpellPriority  PVP_PurgePriority = SpellPriority.Low;
        public RaidTarget PVP_HexIcon = RaidTarget.Triangle;

        public bool PVP_PrepWaterWalking = true;
        public bool PVP_PrepWaterBreathing = true;

        public ConfigHeal PVP_Heal;

        public RafCombatStyle RAF_CombatStyle = RafCombatStyle.Auto;
        public int RAF_GroupOffHeal = 40;
        public string RAF_TotemEarth = "Auto";
        public string RAF_TotemFire = "Auto";
        public string RAF_TotemWater = "Auto";
        public string RAF_TotemAir = "Auto";
        public bool RAF_UseBloodlustOnBosses = false;
        public bool RAF_SaveFeralSpiritForBosses = true;
        public bool RAF_UseThunderstorm = true;
        public bool RAF_SaveElementalTotemsForBosses = true;
        public bool RAF_FollowClosely = true;

        public SpellPriority RAF_CleansePriority = SpellPriority.Low;
        public SpellPriority RAF_PurgePriority = SpellPriority.Low;
        public RaidTarget RAF_HexIcon = RaidTarget.Triangle;
        public RaidTarget RAF_BindIcon = RaidTarget.Triangle;

        public ConfigHeal RAF_Heal;
        public RafHealStyle RAF_HealStyle = RafHealStyle.Auto ;
        public int RAF_FollowAtRange = 25;

        public const int EmergencySavingHealPct = 40;

        public ConfigHeal GroupHeal
        {
            get 
            {
                if (Styx.Logic.Battlegrounds.IsInsideBattleground)
                    return PVP_Heal;

                return RAF_Heal;
            }
        }

        //**** Calculated values for convenience
        public bool FarmingLowLevel
        { get { return PVE_CombatStyle == PveCombatStyle.FarmingLowLevelMobs && !Shaman.IsPVP() && !Shaman.IsRAF(); } }

        //**** Dump current settings to debug output
        public void DebugDump()
        {
            // General
            Logging.WriteDebug("  #-- GENERAL SETTINGS -----#");
            Logging.WriteDebug("  # Debug:                  '{0}'", this.Debug);
            Logging.WriteDebug("  # UseGhostWolfForm:       '{0}'", this.UseGhostWolfForm);
            Logging.WriteDebug("  # DistForGhostWolf:       '{0}'", this.DistanceForGhostWolf);
            Logging.WriteDebug("  # UseBandages:            '{0}'", this.UseBandages);
            Logging.WriteDebug("  # DistanceForTotemRecall: '{0}'", this.DistanceForTotemRecall);
            Logging.WriteDebug("  # RestHealthPercent:      '{0}'", this.RestHealthPercent);
            Logging.WriteDebug("  # RestManaPercent:        '{0}'", this.RestManaPercent);
            Logging.WriteDebug("  # TwistDamagePercent:     '{0}'", this.TwistDamagePercent);
            Logging.WriteDebug("  # TwistManaPercent:       '{0}'", this.TwistManaPercent);
            Logging.WriteDebug("  # NeedHealHealthPercent:  '{0}'", this.NeedHealHealthPercent);
            Logging.WriteDebug("  # EmergencyHealthPercent: '{0}'", this.EmergencyHealthPercent);
            Logging.WriteDebug("  # EmergencyManaPercent:   '{0}'", this.EmergencyManaPercent);
            Logging.WriteDebug("  # LifebloodPercent:       '{0}'", this.LifebloodPercent );
            Logging.WriteDebug("  # ShamanisticRagePercent: '{0}'", this.ShamanisticRagePercent);
            Logging.WriteDebug("  # ThunderstormPercent:    '{0}'", this.ThunderstormPercent);
            Logging.WriteDebug("  # ManaTidePercent:        '{0}'", this.ManaTidePercent);
            Logging.WriteDebug("  # TrinkAtHealth:          '{0}'", this.TrinkAtHealth);
            Logging.WriteDebug("  # TrinkAtMana:            '{0}'", this.TrinkAtMana);

            Logging.WriteDebug("  # DisableMovement:        '{0}'", this.DisableMovement);
            Logging.WriteDebug("  # DisableTargeting:       '{0}'", this.DisableTargeting);
            Logging.WriteDebug("  # MeleeCombatBeforeLvl10: '{0}'", this.MeleeCombatBeforeLevel10);
            Logging.WriteDebug("  # InterruptStyle:         '{0}'", this.InterruptStyle);
            Logging.WriteDebug("  # DetectImmunities:       '{0}'", this.DetectImmunities);
            Logging.WriteDebug("  # WaterWalking:           '{0}'", this.WaterWalking);

            // PVE Grinding
            Logging.WriteDebug("  #-- PVE SETTINGS ---------#");
            Logging.WriteDebug("  # PVE_CombatStyle:        '{0}'", this.PVE_CombatStyle);
            Logging.WriteDebug("  # PVE_LevelsAboveAsElite: '{0}'", this.PVE_LevelsAboveAsElite);
            Logging.WriteDebug("  # PVE_StressfulMobCount:  '{0}'", this.PVE_StressfulMobCount);
            Logging.WriteDebug("  # PVE_PullType:           '{0}'", this.PVE_PullType);
            Logging.WriteDebug("  # PVE_MainhandImbue:      '{0}'", this.PVE_MainhandImbue);
            Logging.WriteDebug("  # PVE_OffhandImbue:       '{0}'", this.PVE_OffhandImbue);
            Logging.WriteDebug("  # PVE_TotemEarth:         '{0}'", this.PVE_TotemEarth);
            Logging.WriteDebug("  # PVE_TotemFire:          '{0}'", this.PVE_TotemFire);
            Logging.WriteDebug("  # PVE_TotemWater:         '{0}'", this.PVE_TotemWater);
            Logging.WriteDebug("  # PVE_TotemAir:           '{0}'", this.PVE_TotemAir);
            Logging.WriteDebug("  # PVE_Save_Totems:        '{0}'", this.PVE_SaveForStress_TotemsSelected);
            Logging.WriteDebug("  # PVE_Save_FeralSpirit:   '{0}'", this.PVE_SaveForStress_FeralSpirit);
            Logging.WriteDebug("  # PVE_Save_Elementals:    '{0}'", this.PVE_SaveForStress_ElementalTotems);
            Logging.WriteDebug("  # PVE_Save_DPS_Racials:   '{0}'", this.PVE_SaveForStress_DPS_Racials);
            Logging.WriteDebug("  # PVE_Save_Bloodlust:     '{0}'", this.PVE_SaveForStress_Bloodlust);

            // PVP
            Logging.WriteDebug("  #-- PVP SETTINGS ---------#");
            Logging.WriteDebug("  # PVP_CombatStyle:        '{0}'", this.PVP_CombatStyle);
            Logging.WriteDebug("  # PVP_GroupNeedHeal:      '{0}'", this.PVP_Heal.MaxHealth  );
            Logging.WriteDebug("  # PVP_MainhandImbue:      '{0}'", this.PVP_MainhandImbue);
            Logging.WriteDebug("  # PVP_OffhandImbue:       '{0}'", this.PVP_OffhandImbue);
            Logging.WriteDebug("  # PVP_TotemEarth:         '{0}'", this.PVP_TotemEarth);
            Logging.WriteDebug("  # PVP_TotemFire:          '{0}'", this.PVP_TotemFire);
            Logging.WriteDebug("  # PVP_TotemWater:         '{0}'", this.PVP_TotemWater);
            Logging.WriteDebug("  # PVP_TotemAir:           '{0}'", this.PVP_TotemAir);

            Logging.WriteDebug("  # PVP_CleansePriority     '{0}'", this.PVP_CleansePriority );
            Logging.WriteDebug("  # PVP_PurgePriority       '{0}'", this.PVP_PurgePriority );
            Logging.WriteDebug("  # PVP_HexIcon             '{0}'", this.PVP_HexIcon );

            Logging.WriteDebug("  # PVP_PrepWaterWalking    '{0}'", this.PVP_PrepWaterWalking );
            Logging.WriteDebug("  # PVP_PrepWaterBreathing  '{0}'", this.PVP_PrepWaterBreathing );

            Logging.WriteDebug("  # PVP_Heal.HealingWave    '{0}'", this.PVP_Heal.HealingWave);
            Logging.WriteDebug("  # PVP_Heal.Riptide        '{0}'", this.PVP_Heal.Riptide);
            Logging.WriteDebug("  # PVP_Heal.ChainHeal      '{0}'", this.PVP_Heal.ChainHeal);
            Logging.WriteDebug("  # PVP_Heal.HealingRain    '{0}'", this.PVP_Heal.HealingRain);
            Logging.WriteDebug("  # PVP_Heal.UnleashElement '{0}'", this.PVP_Heal.UnleashElements);
            Logging.WriteDebug("  # PVP_Heal.HealingSurge   '{0}'", this.PVP_Heal.HealingSurge);
            Logging.WriteDebug("  # PVP_Heal.GreaterHW      '{0}'", this.PVP_Heal.GreaterHealingWave);
            Logging.WriteDebug("  # PVP_Heal.OhShoot        '{0}'", this.PVP_Heal.OhShoot);
            Logging.WriteDebug("  # PVP_Heal.TidalWaves     '{0}'", this.PVP_Heal.TidalWaves);
            Logging.WriteDebug("  # PVP_Heal.CleanseSpirit  '{0}'", this.PVP_Heal.Cleanse );
            Logging.WriteDebug("  # PVP_Heal.Pets           '{0}'", this.PVP_Heal.Pets);
            Logging.WriteDebug("  # PVP_Heal.SearchRange    '{0}'", this.PVP_Heal.SearchRange);

            // RAF
            Logging.WriteDebug("  #-- RAF SETTINGS ---------#");
            Logging.WriteDebug("  # RAF_CombatStyle:        '{0}'", this.RAF_CombatStyle);
            Logging.WriteDebug("  # RAF_GroupNeedHeal:      '{0}'", this.RAF_Heal.MaxHealth );
            Logging.WriteDebug("  # RAF_GroupOffHeal:       '{0}'", this.RAF_GroupOffHeal);
            Logging.WriteDebug("  # RAF_TotemEarth:         '{0}'", this.RAF_TotemEarth);
            Logging.WriteDebug("  # RAF_TotemFire:          '{0}'", this.RAF_TotemFire);
            Logging.WriteDebug("  # RAF_TotemWater:         '{0}'", this.RAF_TotemWater);
            Logging.WriteDebug("  # RAF_TotemAir:           '{0}'", this.RAF_TotemAir);
            Logging.WriteDebug("  # RAF_UseBloodlust:       '{0}'", this.RAF_UseBloodlustOnBosses );
            Logging.WriteDebug("  # RAF_SaveFeralSpirit:    '{0}'", this.RAF_SaveFeralSpiritForBosses );
            Logging.WriteDebug("  # RAF_UseThunderstorm:    '{0}'", this.RAF_UseThunderstorm );
            Logging.WriteDebug("  # RAF_SaveElementalTotems:'{0}'", this.RAF_SaveElementalTotemsForBosses );
            Logging.WriteDebug("  # RAF_FollowClosely:      '{0}'", this.RAF_FollowClosely );
            Logging.WriteDebug("  # RAF_CleansePriority     '{0}'", this.RAF_CleansePriority);
            Logging.WriteDebug("  # RAF_PurgePriority       '{0}'", this.RAF_PurgePriority);
            Logging.WriteDebug("  # RAF_HexIcon             '{0}'", this.RAF_HexIcon);
            Logging.WriteDebug("  # RAF_BindIcon            '{0}'", this.RAF_BindIcon);

            Logging.WriteDebug("  # RAF_HealStyle           '{0}'", this.RAF_HealStyle);
            Logging.WriteDebug("  # RAF_Heal.HealingWave    '{0}'", this.RAF_Heal.HealingWave);
            Logging.WriteDebug("  # RAF_Heal.Riptide        '{0}'", this.RAF_Heal.Riptide);
            Logging.WriteDebug("  # RAF_Heal.ChainHeal      '{0}'", this.RAF_Heal.ChainHeal);
            Logging.WriteDebug("  # RAF_Heal.HealingRain    '{0}'", this.RAF_Heal.HealingRain);
            Logging.WriteDebug("  # RAF_Heal.UnleashElement '{0}'", this.RAF_Heal.UnleashElements);
            Logging.WriteDebug("  # RAF_Heal.HealingSurge   '{0}'", this.RAF_Heal.HealingSurge);
            Logging.WriteDebug("  # RAF_Heal.GreaterHW      '{0}'", this.RAF_Heal.GreaterHealingWave);
            Logging.WriteDebug("  # RAF_Heal.OhShoot        '{0}'", this.RAF_Heal.OhShoot);
            Logging.WriteDebug("  # RAF_Heal.TidalWaves     '{0}'", this.RAF_Heal.TidalWaves);
            Logging.WriteDebug("  # RAF_Heal.Cleanse        '{0}'", this.RAF_Heal.Cleanse );
            Logging.WriteDebug("  # RAF_Heal.Pets           '{0}'", this.RAF_Heal.Pets);
            Logging.WriteDebug("  # RAF_Heal.SearchRange    '{0}'", this.RAF_Heal.SearchRange);
            Logging.WriteDebug("  # RAF_FollowAtRange       '{0}'", this.RAF_FollowAtRange);

            Logging.WriteDebug("  #-- SETTINGS END --------#");
        }



        public ConfigValues()
        {
            PVP_Heal = new ConfigHeal();
            PVP_Heal.HealingWave = 90;
            PVP_Heal.Riptide = 89;
            PVP_Heal.ChainHeal = 88;
            PVP_Heal.HealingRain = 68;
            PVP_Heal.GreaterHealingWave = 70;
            PVP_Heal.UnleashElements = 69;
            PVP_Heal.HealingSurge = 55;
            PVP_Heal.OhShoot = 35;
            PVP_Heal.TidalWaves = true;
            PVP_Heal.Cleanse = true;
            PVP_Heal.Pets = false;
            PVP_Heal.SearchRange = 60;

            RAF_Heal = new ConfigHeal();
            RAF_Heal.HealingWave = 90;
            RAF_Heal.Riptide = 89;
            RAF_Heal.ChainHeal = 88;
            RAF_Heal.HealingRain = 0;
            RAF_Heal.GreaterHealingWave = 70;
            RAF_Heal.UnleashElements = 69;
            RAF_Heal.HealingSurge = 55;
            RAF_Heal.OhShoot = 30;
            RAF_Heal.TidalWaves = true;
            RAF_Heal.Cleanse = true;
            RAF_Heal.Pets = false;
            RAF_Heal.SearchRange = 75;
        }

        public bool FileLoad(string sFilename, out bool didUpgrade )
        {
            XElement toplvl = XElement.Load( sFilename);
            XElement[] elements = toplvl.Elements().ToArray();

            didUpgrade = false;
            foreach (XElement elem in elements)
            {
                switch (elem.Name.ToString())
                {
                    case "version":
                        LoadStr(elem, ref CreatedByVersion);                        break;

                    case "debug":
                        LoadBool(elem, ref Debug);                                  break;

                    case "useghostwolf":
                        LoadBool(elem, ref UseGhostWolfForm);                      break;
                    case "safedistanceforghostwolf":
                        LoadInt(elem, ref DistanceForGhostWolf);                   break;
                    case "restminmana":
                        LoadInt(elem, ref RestManaPercent);                        break;
                    case "restminhealth":
                        LoadInt(elem, ref RestHealthPercent);                      break;
                    case "needheal":
                        LoadInt(elem, ref NeedHealHealthPercent);                  break;
                    case "emergencyhealth":
                        LoadInt(elem, ref EmergencyHealthPercent);                 break;
                    case "emergencymana":
                        LoadInt(elem, ref EmergencyManaPercent);                   break;
                    case "needlifeblood":
                        LoadInt(elem, ref LifebloodPercent); break;
                    case "needshamanisticrage":
                        LoadInt(elem, ref ShamanisticRagePercent);             break;
                    case "needthunderstorm":
                        LoadInt(elem, ref ThunderstormPercent); break;
                    case "needmanatide":
                        LoadInt(elem, ref ManaTidePercent); break;
                    case "trinkathealth":
                        LoadInt(elem, ref TrinkAtHealth);                break;
                    case "trinkatmana":
                        LoadInt(elem, ref TrinkAtMana);                break;

                    case "usebandages":
                        LoadBool(elem, ref UseBandages);                           break;
                    case "totemrecalldistance":
                        LoadInt(elem, ref DistanceForTotemRecall);                 break;
                    case "twistwatershield":
                        LoadInt(elem, ref TwistManaPercent);                       break;
                    case "twistlightningshield":
                        LoadInt(elem, ref TwistDamagePercent);                     break;
                    case "disablemovement":
                        LoadBool(elem, ref DisableMovement); break;
                    case "disabletargeting":
                        LoadBool(elem, ref DisableTargeting); break;
                    case "meleecombatbeforelevel10":
                        LoadBool(elem, ref MeleeCombatBeforeLevel10 ); break;
                    case "interruptstyle":
                        LoadSpellInterruptStyle(elem, ref InterruptStyle ); break;
                    case "detectimmunities":
                        LoadBool(elem, ref DetectImmunities ); break;
                    case "waterwalking":
                        LoadBool(elem, ref WaterWalking ); break;

                    case "pve_combatstyle":
                        LoadPveCombatStyle(elem, ref PVE_CombatStyle);             break;
                    case "pve_typeofpull":
                        LoadTypeOfPull(elem, ref PVE_PullType);                    break;
                    case "pve_stressonly_feralspirit":
                        LoadBool(elem, ref PVE_SaveForStress_FeralSpirit);         break;
                    case "pve_stressonly_elementaltotems":
                        LoadBool(elem, ref PVE_SaveForStress_ElementalTotems);     break;
                    case "pve_stressonly_dps_racials":
                        LoadBool(elem, ref PVE_SaveForStress_DPS_Racials);         break;
                    case "pve_stressonly_bloodlust":
                        LoadBool(elem, ref PVE_SaveForStress_Bloodlust);           break;
                    case "pve_stressonly_totembar":
                        LoadBool(elem, ref PVE_SaveForStress_TotemsSelected);      break;
                    case "pve_stresslevelsabove":
                        LoadInt(elem, ref PVE_LevelsAboveAsElite);                 break;
                    case "pve_stressfulmobcount":
                        LoadInt(elem, ref PVE_StressfulMobCount);                  break;
                    case "pve_totemearth":
                        LoadStr(elem, ref PVE_TotemEarth);                         break;
                    case "pve_totemfire":
                        LoadStr(elem, ref PVE_TotemFire);                          break;
                    case "pve_totemwater":
                        LoadStr(elem, ref PVE_TotemWater);                         break;
                    case "pve_totemair":
                        LoadStr(elem, ref PVE_TotemAir);                           break;
                    case "pve_mainhand":
                        LoadStr(elem, ref PVE_MainhandImbue);                    break;
                    case "pve_offhand":
                        LoadStr(elem, ref PVE_OffhandImbue);                     break;

                    case "pvp_combatstyle":
                        LoadPvpCombatStyle(elem, ref PVP_CombatStyle);             break;
                    case "pvp_totemearth":
                        LoadStr(elem, ref PVP_TotemEarth);                         break;
                    case "pvp_totemfire":
                        LoadStr(elem, ref PVP_TotemFire);                          break;
                    case "pvp_totemwater":
                        LoadStr(elem, ref PVP_TotemWater);                         break;
                    case "pvp_totemair":
                        LoadStr(elem, ref PVP_TotemAir);                           break;
                    case "pvp_mainhand":
                        LoadStr(elem, ref PVP_MainhandImbue);                    break;
                    case "pvp_offhand":
                        LoadStr(elem, ref PVP_OffhandImbue);                     break;

                    case "pvp_cleansepriority":
                        LoadSpellPriority(elem, ref PVP_CleansePriority); break;
                    case "pvp_purgepriority":
                        LoadSpellPriority(elem, ref PVP_PurgePriority); break;
                    case "pvp_hexicon":
                        LoadRaidIcon(elem, ref PVP_HexIcon ); break;

                    case "pvp_prepwaterbreathing":
                        LoadBool(elem, ref PVP_PrepWaterBreathing ); break;
                    case "pvp_prepwaterwalking":
                        LoadBool(elem, ref PVP_PrepWaterWalking ); break;

                    case "pvp_heal_healingwave":
                        LoadInt(elem, ref PVP_Heal.HealingWave); break;
                    case "pvp_heal_riptide":
                        LoadInt(elem, ref PVP_Heal.Riptide); break;
                    case "pvp_heal_unleashelements":
                        LoadInt(elem, ref PVP_Heal.UnleashElements); break;
                    case "pvp_heal_chainheal":
                        LoadInt(elem, ref PVP_Heal.ChainHeal); break;
                    case "pvp_heal_healingrain":
                        LoadInt(elem, ref PVP_Heal.HealingRain); break;
                    case "pvp_heal_greaterhealingwave":
                        LoadInt(elem, ref PVP_Heal.GreaterHealingWave); break;
                    case "pvp_heal_healingsurge":
                        LoadInt(elem, ref PVP_Heal.HealingSurge); break;
                    case "pvp_heal_ohshoot":
                        LoadInt(elem, ref PVP_Heal.OhShoot); break;

                    case "pvp_heal_tidalwaves":
                        LoadBool(elem, ref PVP_Heal.TidalWaves); break;
                    case "pvp_heal_cleanse":
                        LoadBool(elem, ref PVP_Heal.Cleanse); break;
                    case "pvp_heal_pets":
                        LoadBool(elem, ref PVP_Heal.Pets ); break;
                    case "pvp_heal_searchrange":
                        LoadDouble(elem, ref PVP_Heal.SearchRange); break;

                    case "raf_combatstyle":
                        LoadRafCombatStyle(elem, ref RAF_CombatStyle);             break;
                    case "raf_groupoffheal":
                        LoadInt(elem, ref RAF_GroupOffHeal ); break;
                    case "raf_usethunderstorm":
                        LoadBool(elem, ref RAF_UseThunderstorm); break;
                    case "raf_usebloodlustonbosses":
                        LoadBool(elem, ref RAF_UseBloodlustOnBosses);              break;
                    case "raf_saveferalspiritforbosses":
                        LoadBool(elem, ref RAF_SaveFeralSpiritForBosses);          break;
                    case "raf_totemearth":
                        LoadStr(elem, ref RAF_TotemEarth);                         break;
                    case "raf_totemfire":
                        LoadStr(elem, ref RAF_TotemFire);                          break;
                    case "raf_totemwater":
                        LoadStr(elem, ref RAF_TotemWater);                         break;
                    case "raf_totemair":
                        LoadStr(elem, ref RAF_TotemAir);                           break;
                    case "raf_saveelementaltotemsforbosses":
                        LoadBool(elem, ref RAF_SaveElementalTotemsForBosses );      break;
                    case "raf_followclosely":
                        LoadBool(elem, ref RAF_FollowClosely );      break;

                    case "raf_cleansepriority":
                        LoadSpellPriority(elem, ref RAF_CleansePriority); break;
                    case "raf_purgepriority":
                        LoadSpellPriority(elem, ref RAF_PurgePriority); break;
                    case "raf_hexicon":
                        LoadRaidIcon(elem, ref RAF_HexIcon); break;
                    case "raf_bindicon":
                        LoadRaidIcon(elem, ref RAF_BindIcon); break;

                    case "raf_heal_healingwave":
                        LoadInt(elem, ref RAF_Heal.HealingWave); break;
                    case "raf_heal_riptide":
                        LoadInt(elem, ref RAF_Heal.Riptide); break;
                    case "raf_heal_unleashelements":
                        LoadInt(elem, ref RAF_Heal.UnleashElements); break;
                    case "raf_heal_chainheal":
                        LoadInt(elem, ref RAF_Heal.ChainHeal); break;
                    case "raf_heal_healingrain":
                        LoadInt(elem, ref RAF_Heal.HealingRain); break;
                    case "raf_heal_greaterhealingwave":
                        LoadInt(elem, ref RAF_Heal.GreaterHealingWave); break;
                    case "raf_heal_healingsurge":
                        LoadInt(elem, ref RAF_Heal.HealingSurge); break;
                    case "raf_heal_ohshoot":
                        LoadInt(elem, ref RAF_Heal.OhShoot); break;

                    case "raf_heal_tidalwaves":
                        LoadBool(elem, ref RAF_Heal.TidalWaves); break;
                    case "raf_heal_cleanse":
                        LoadBool(elem, ref RAF_Heal.Cleanse); break;
                    case "raf_heal_pets":
                        LoadBool(elem, ref RAF_Heal.Pets); break;
                    case "raf_heal_searchrange":
                        LoadDouble(elem, ref RAF_Heal.SearchRange); break;
                    case "raf_followatrange":
                        LoadInt(elem, ref RAF_FollowAtRange ); break;

                    case "raf_healstyle":
                        LoadHealStyle(elem, ref RAF_HealStyle); break;

                    default:
                        Shaman.Dlog("error: unknown config setting: {0}={1}", elem.Name, elem.Value.ToString());
                        break;
                }
            }

            if ( String.Compare( CreatedByVersion, Shaman.Version) < 0)
            {
                didUpgrade = true;
                Shaman.Dlog("ConfigValue.FileLoad:  detected config file created by a different version");

                // UPGRADE TO 4.2.05
                if (string.Compare(CreatedByVersion, "4.2.05") < 0)
                {
                    Shaman.Slog( "" );

                    Shaman.Slog(Color.Orange, "ConfigUpgrade:  upgrading older config file version to {0}", Shaman.Version);

                    Shaman.Slog(Color.Orange, "ConfigUpgrade:  Shamanistic Rage % changed from {0} to {1}", ShamanisticRagePercent, 100);
                    ShamanisticRagePercent = 100;

                    Shaman.Slog(Color.Orange, "ConfigUpgrade:  RAF Combat Style changed from '{0}' to '{1}'", RAF_CombatStyle, "Auto");
                    RAF_CombatStyle = RafCombatStyle.Auto;

                    Shaman.Slog(Color.Orange, "ConfigUpgrade:  Shield Twist Mana % changed from '{0}' to '{1}'", TwistManaPercent, 25);
                    TwistManaPercent = 25;

                    Shaman.Slog(Color.Orange, "ConfigUpgrade:  Shield Twist Damage % changed from '{0}' to '{1}'", TwistDamagePercent, 50);
                    TwistDamagePercent = 50;

                    Shaman.Slog("");
                }

                CreatedByVersion = Shaman.Version;
                Save(sFilename);
            }

            return true;
        }

        private static void LoadBool(XElement elem, ref bool value)
        {
            bool localVal;
            if (!bool.TryParse(elem.Value, out localVal))
            {
                localVal = value;
                Shaman.Slog(
                    "config:  setting '{0}' invalid - expected True/False but read '{1}' - defaulting to '{2}'",
                    elem.Name,
                    elem.Value,
                    localVal
                    );
            }
            value = localVal;
        }
        private static void LoadStr(XElement elem, ref string value)
        {
            value = elem.Value;
        }
        private static void LoadInt(XElement elem, ref int value)
        {
            int localVal;
            if (!int.TryParse(elem.Value, out localVal))
            {
                localVal = value;
                Shaman.Slog(
                    "config:  setting '{0}' invalid - expected integer but read '{1}' - defaulting to '{2}'",
                    elem.Name,
                    elem.Value,
                    localVal
                    );
            }
            value = localVal;
        }
        private static void LoadDouble(XElement elem, ref double value)
        {
            int localVal = 0;
            LoadInt(elem, ref localVal);
            value = (double)localVal;
        }


        private static void LoadTypeOfPull(XElement elem, ref TypeOfPull value)
        {
            switch (elem.Value.ToUpper()[0])
            {
                case 'A':
                    value = TypeOfPull.Auto;
                    break;
                case 'B':
                    value = TypeOfPull.Body;
                    break;
                case 'R':
                    value = TypeOfPull.Ranged;
                    break;
                case 'F':
                    value = TypeOfPull.Fast;
                    break;
                default:
                    Shaman.Slog(
                        "config:  setting '{0}' invalid - expected integer but read '{1}' - defaulting to '{2}'",
                        elem.Name,
                        elem.Value,
                        value
                        );
                    break;
            }
        }

        private static void LoadSpellInterruptStyle(XElement elem, ref SpellInterruptStyle value)
        {
            switch (elem.Value.ToUpper()[0])
            {
                case 'N':
                    value = SpellInterruptStyle.None;
                    break;
                case 'C':
                    value = SpellInterruptStyle.CurrentTarget;
                    break;
                case 'A':
                    value = SpellInterruptStyle.All;
                    break;
                default:
                    Shaman.Slog(
                        "config:  setting '{0}' invalid - expected pve combat style but read '{1}' - defaulting to '{2}'",
                        elem.Name,
                        elem.Value,
                        value
                        );
                    break;
            }
        }

        private static void LoadPveCombatStyle(XElement elem, ref PveCombatStyle value)
        {
            switch (elem.Value.ToUpper()[0])
            {
                case 'N':
                    value = PveCombatStyle.Normal;
                    break;
                case 'D':
                    value = PveCombatStyle.FarmingLowLevelMobs;
                    break;
                case 'F':
                    value = PveCombatStyle.FarmingLowLevelMobs;
                    break;
                default:
                    Shaman.Slog(
                        "config:  setting '{0}' invalid - expected pve combat style but read '{1}' - defaulting to '{2}'",
                        elem.Name,
                        elem.Value,
                        value
                        );
                    break;
            }
        }


        private static void LoadPvpCombatStyle(XElement elem, ref PvpCombatStyle value)
        {
            switch (elem.Value.ToLower())
            {
                case "combatonly":
                    value = PvpCombatStyle.CombatOnly;
                    break;
                case "healingovercombat":
                    value = PvpCombatStyle.HealingOverCombat;
                    break;
                case "healingonly":
                    value = PvpCombatStyle.HealingOnly;
                    break;
                default:
                    Shaman.Slog(
                        "config:  setting '{0}' invalid - expected pvp combat style but read '{1}' - defaulting to '{2}'",
                        elem.Name,
                        elem.Value,
                        value
                        );
                    break;
            }
        }


        private static void LoadRafCombatStyle(XElement elem, ref RafCombatStyle value)
        {
            switch (elem.Value.ToLower())
            {
                case "auto":
                    value = RafCombatStyle.Auto;
                    break;
                case "combatonly":
                    value = RafCombatStyle.CombatOnly;
                    break;
                case "healingovercombat":
                    value = RafCombatStyle.HealingOverCombat;
                    break;
                case "healingonly":
                    value = RafCombatStyle.HealingOnly;
                    break;
                default:
                    Shaman.Slog(
                        "config:  setting '{0}' invalid - expected Raf combat style but read '{1}' - defaulting to '{2}'",
                        elem.Name,
                        elem.Value,
                        value
                        );
                    break;
            }
        }

        private static void LoadSpellPriority(XElement elem, ref SpellPriority pri)
        {
            try
            {
                pri = (SpellPriority)Enum.Parse( typeof(SpellPriority), elem.Value);
            }
            catch
            {
            }
        }

        private static void LoadRaidIcon(XElement elem, ref RaidTarget icon)
        {
            try
            {
                icon = (RaidTarget)Enum.Parse(typeof(RaidTarget), elem.Value);
            }
            catch
            {
            }
        }

        private static void LoadHealStyle(XElement elem, ref RafHealStyle style)
        {
            try
            {
                style = (RafHealStyle)Enum.Parse(typeof(RafHealStyle), elem.Value);
            }
            catch
            {
            }
        }

        public void Save(string sFilename)
        {
            XDocument doc = new XDocument();
            doc.Add( new XElement("ShamWOW",
                        new XElement("version", CreatedByVersion ),

                        new XElement("debug", Debug),

                        new XElement("useghostwolf", UseGhostWolfForm),
                        new XElement("safedistanceforghostwolf", DistanceForGhostWolf),
                        new XElement("restminmana", RestManaPercent),
                        new XElement("restminhealth", RestHealthPercent),
                        new XElement("needheal", NeedHealHealthPercent),
                        new XElement("emergencyhealth", EmergencyHealthPercent),
                        new XElement("emergencymana", EmergencyManaPercent),
                        new XElement("needlifeblood", LifebloodPercent),
                        new XElement("needshamanisticrage", ShamanisticRagePercent),
                        new XElement("needthunderstorm", ThunderstormPercent),
                        new XElement("needmanatide", ManaTidePercent),
                        new XElement("trinkathealth", TrinkAtHealth),
                        new XElement("trinkatmana", TrinkAtMana),

                        new XElement("usebandages", UseBandages),
                        new XElement("totemrecalldistance", DistanceForTotemRecall),
                        new XElement("twistwatershield", TwistManaPercent),
                        new XElement("twistlightningshield", TwistDamagePercent),
                        new XElement("disablemovement", DisableMovement),
                        new XElement("disabletargeting", DisableTargeting),
                        new XElement("meleecombatbeforelevel10", MeleeCombatBeforeLevel10),
                        new XElement("interruptstyle", InterruptStyle),
                        new XElement("detectimmunities", DetectImmunities),
                        new XElement("waterwalking", WaterWalking),

                        new XElement("pve_combatstyle", PVE_CombatStyle),
                        new XElement("pve_typeofpull", PVE_PullType),
                        new XElement("pve_stressonly_feralspirit", PVE_SaveForStress_FeralSpirit),
                        new XElement("pve_stressonly_elementaltotems", PVE_SaveForStress_ElementalTotems),
                        new XElement("pve_stressonly_dps_racials", PVE_SaveForStress_DPS_Racials),
                        new XElement("pve_stressonly_bloodlust", PVE_SaveForStress_Bloodlust),
                        new XElement("pve_stressonly_totembar", PVE_SaveForStress_TotemsSelected),
                        new XElement("pve_stresslevelsabove", PVE_LevelsAboveAsElite),
                        new XElement("pve_stressfulmobcount", PVE_StressfulMobCount),
                        new XElement("pve_totemearth", PVE_TotemEarth),
                        new XElement("pve_totemfire", PVE_TotemFire),
                        new XElement("pve_totemwater", PVE_TotemWater),
                        new XElement("pve_totemair", PVE_TotemAir),
                        new XElement("pve_mainhand", PVE_MainhandImbue),
                        new XElement("pve_offhand", PVE_OffhandImbue),

                        new XElement("pvp_combatstyle", PVP_CombatStyle),
                        // new XElement("pvp_groupneedheal", PVP_GroupNeedHeal),
                        new XElement("pvp_totemearth", PVP_TotemEarth),
                        new XElement("pvp_totemfire", PVP_TotemFire),
                        new XElement("pvp_totemwater", PVP_TotemWater),
                        new XElement("pvp_totemair", PVP_TotemAir),
                        new XElement("pvp_mainhand", PVP_MainhandImbue),
                        new XElement("pvp_offhand", PVP_OffhandImbue),

                        new XElement("pvp_cleansepriority", PVP_CleansePriority),
                        new XElement("pvp_purgepriority", PVP_PurgePriority),
                        new XElement("pvp_hexicon", PVP_HexIcon),                      

                        new XElement("pvp_prepwaterbreathing", PVP_PrepWaterBreathing),                      
                        new XElement("pvp_prepwaterwalking", PVP_PrepWaterWalking),                      

                        new XElement("pvp_heal_healingwave", PVP_Heal.HealingWave ),
                        new XElement("pvp_heal_chainheal", PVP_Heal.ChainHeal),
                        new XElement("pvp_heal_healingrain", PVP_Heal.HealingRain),
                        new XElement("pvp_heal_riptide", PVP_Heal.Riptide),
                        new XElement("pvp_heal_greaterhealingwave", PVP_Heal.GreaterHealingWave),
                        new XElement("pvp_heal_ohshoot", PVP_Heal.OhShoot),
                        new XElement("pvp_heal_healingsurge", PVP_Heal.HealingSurge),
                        new XElement("pvp_heal_unleashelements", PVP_Heal.UnleashElements),
                        new XElement("pvp_heal_tidalwaves", PVP_Heal.TidalWaves),
                        new XElement("pvp_heal_cleanse", PVP_Heal.Cleanse),
                        new XElement("pvp_heal_pets", PVP_Heal.Pets),
                        new XElement("pvp_heal_searchrange", PVP_Heal.SearchRange ),

                        new XElement("raf_combatstyle", RAF_CombatStyle),
                        // new XElement("raf_groupneedheal", RAF_GroupNeedHeal),
                        new XElement("raf_groupoffheal", RAF_GroupOffHeal),
                        new XElement("raf_usethunderstorm", RAF_UseThunderstorm),
                        new XElement("raf_usebloodlustonbosses", RAF_UseBloodlustOnBosses),
                        new XElement("raf_saveferalspiritforbosses", RAF_SaveFeralSpiritForBosses),
                        new XElement("raf_saveelementaltotemsforbosses", RAF_SaveElementalTotemsForBosses ),
                        new XElement("raf_followclosely", RAF_FollowClosely ),

                        new XElement("raf_totemearth", RAF_TotemEarth),
                        new XElement("raf_totemfire", RAF_TotemFire),
                        new XElement("raf_totemwater", RAF_TotemWater),
                        new XElement("raf_totemair", RAF_TotemAir),

                        new XElement("raf_cleansepriority", RAF_CleansePriority),
                        new XElement("raf_purgepriority", RAF_PurgePriority),
                        new XElement("raf_hexicon", RAF_HexIcon),
                        new XElement("raf_bindicon", RAF_BindIcon),
                        
                        new XElement( "raf_heal_healingwave", RAF_Heal.HealingWave ),
                        new XElement("raf_heal_chainheal", RAF_Heal.ChainHeal),
                        new XElement("raf_heal_healingrain", RAF_Heal.HealingRain),
                        new XElement("raf_heal_riptide", RAF_Heal.Riptide),
                        new XElement( "raf_heal_greaterhealingwave", RAF_Heal.GreaterHealingWave),
                        new XElement( "raf_heal_ohshoot", RAF_Heal.OhShoot),
                        new XElement( "raf_heal_healingsurge", RAF_Heal.HealingSurge),
                        new XElement("raf_heal_unleashelements", RAF_Heal.UnleashElements),

                        new XElement("raf_heal_tidalwaves", RAF_Heal.TidalWaves),
                        new XElement("raf_heal_cleanse", RAF_Heal.Cleanse),
                        new XElement("raf_heal_pets", RAF_Heal.Pets),
                        new XElement("raf_heal_searchrange", RAF_Heal.SearchRange),

                        new XElement("raf_followatrange", RAF_FollowAtRange),

                        new XElement("raf_healstyle", RAF_HealStyle )
                        )
                    );

            doc.Save(sFilename);
        }

    }

    public class ConfigHeal
    {
        public int HealingWave= 0;
        public int Riptide= 0;
        public int UnleashElements = 0;
        public int ChainHeal= 0;
        public int HealingRain = 0;
        public int GreaterHealingWave= 0;
        public int HealingSurge= 0;
        public int OhShoot= 0;
        public bool TidalWaves = true;
        public bool Cleanse = true;
        public bool Pets = false;
        public double SearchRange = 50;

        public ConfigHeal()
        {
        }

#if WE_DONT_USE
        public ConfigHeal(int hw, int rt, int ue, int ch, int ghw, int hs, int os, bool tw, bool pets, double search)
        {
            HealingWave= hw;
            Riptide= rt;
            ChainHeal= ch;
            GreaterHealingWave= ghw;
            HealingSurge= hs;
            OhShoot= os;
            TidalWaves = tw;
            Pets = pets;
            SearchRange = search;
        }
#endif
        public int MaxHealth
        {
            get
            {
                int maxVal = 0;
                maxVal = Math.Max(maxVal, HealingWave);
                maxVal = Math.Max(maxVal, Riptide);
                maxVal = Math.Max(maxVal, UnleashElements );
                maxVal = Math.Max(maxVal, GreaterHealingWave);
                maxVal = Math.Max(maxVal, ChainHeal);
                maxVal = Math.Max(maxVal, HealingRain);
                maxVal = Math.Max(maxVal, HealingSurge);
                maxVal = Math.Max(maxVal, OhShoot);
                return maxVal;
            }
        }
    }
}
