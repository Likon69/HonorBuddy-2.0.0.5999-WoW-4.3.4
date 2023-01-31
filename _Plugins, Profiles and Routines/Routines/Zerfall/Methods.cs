using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = System.Action;
using Sequence = Styx.Logic;
using Styx.WoWInternals.World;


namespace Zerfall
{
    public partial class Zerfall
    {

        public string CurrentPullSpell
        {
            get
            {
                return ZerfallSettings.Instance.PullSpellSelect;
            }
        }
        public string CurrentPetSpell
        {
            get
            {
                return ZerfallSettings.Instance.PetSpell;
            }
        }
        public void test()
        {
            WoWPoint[] pathtoDest = Styx.Logic.Pathing.Navigator.GeneratePath(Me.Location, Me.Location);
            //List<WoWPoint> Pathto = Navigator.GeneratePath(Me.Location, Me.Location);
            foreach (KeyValuePair<string, WoWAura> pair in Me.Auras)
            {
                WoWAura curAura = pair.Value;
                Logging.Write(curAura.Name + " " + curAura.Spell.Description);
                if (curAura.Spell.Mechanic == WoWSpellMechanic.Asleep)
                {
                    Lua.DoString("CancelUnitBuff('player', '" + curAura.Name + "')");
                }
            }
        }
        //Pet listing. Me.Pet.CreatedBySpellId
        //688 - Imp
        //697 - Voidwalker
        //691 - Felhunter
        //712 - Succubus
        //30146 - Felguard
        //old Pet Casting Logic
        public void PetSkill(string SkillName)
        {
            switch (Me.Pet.CreatedBySpellId)
            {
                case 697:
                    if (SkillName == "Sacrifice" && !PetActionOnCooldown(4))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(3, \"pet\")");
                        
                    }
                    if (SkillName == "Agressive" && !PetActionOnCooldown(8))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(8)");
                    }
                    if (SkillName == "Defensive" && !PetActionOnCooldown(9))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(9)");
                    }
                    if (SkillName == "Passive" && !PetActionOnCooldown(10))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(10)");
                    }
                    break;
                case 688:
                    if (SkillName == "Firebolt" && !PetActionOnCooldown(4))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(3, \"pet\")");
                    }
                    if (SkillName == "Blood Pact" && !PetActionOnCooldown(5))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(2, \"pet\")");
                    }
                    if (SkillName == "Agressive" && !PetActionOnCooldown(8))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(8)");
                    }
                    if (SkillName == "Defensive" && !PetActionOnCooldown(9))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(9)");
                    }
                    if (SkillName == "Passive" && !PetActionOnCooldown(10))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(10)");
                    }
                    break;
                case 712:
                    if (SkillName == "Lash of Pain" && !PetActionOnCooldown(4))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(2, \"pet\")");
                    }
                    if (SkillName == "Seduction" && !PetActionOnCooldown(5))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(4, \"pet\")");
                    }
                    if (SkillName == "Agressive" && !PetActionOnCooldown(8))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(8)");
                    }
                    if (SkillName == "Defensive" && !PetActionOnCooldown(9))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(9)");
                    }
                    if (SkillName == "Passive" && !PetActionOnCooldown(10))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(10)");
                    }
                    break;
                case 691:
                    if (SkillName == "Agressive" && !PetActionOnCooldown(8))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(8)");
                    }
                    if (SkillName == "Defensive" && !PetActionOnCooldown(9))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(9)");
                    }
                    if (SkillName == "Passive" && !PetActionOnCooldown(10))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(10)");
                    }
                    break;
                case 30146:
                    if (SkillName == "Felstorm" && !PetActionOnCooldown(6))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(3, \"pet\")");
                    }
                    if (SkillName == "Axe Toss" && !PetActionOnCooldown(6))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(2, \"pet\")");
                    }
                    if (SkillName == "Legion Strike" && !PetActionOnCooldown(5))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(4, \"pet\")");
                    }
                    if (SkillName == "Pursuit" && !PetActionOnCooldown(4))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastSpell(5, \"pet\")");
                    }
                    if (SkillName == "Agressive" && !PetActionOnCooldown(8))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(8)");
                    }
                    if (SkillName == "Defensive" && !PetActionOnCooldown(9))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(9)");
                    }
                    if (SkillName == "Passive" && !PetActionOnCooldown(10))
                    {
                        Log("Casting {0}", SkillName);
                        Lua.DoString("CastPetAction(10)");
                    }
                    break;
            }
            
        }



        private static void IncludeTargetsFilter(List<WoWObject> incomingUnits, HashSet<WoWObject> outgoingUnits)
        {
            if (!StyxWoW.Me.GotAlivePet || (StyxWoW.Me.GotAlivePet && !StyxWoW.Me.Pet.Combat))
                return;

            for (int i = 0; i < incomingUnits.Count; i++)
            {
                if (incomingUnits[i] is WoWUnit)
                {
                    WoWUnit u = incomingUnits[i].ToUnit();
                    if (u.Combat && (u.IsTargetingMeOrPet || u.PetAggro))
                        outgoingUnits.Add(u);
                }
            }
        }

        public static bool IsInPartyOrRaid()
        {
            if (Me.PartyMembers.Count > 0)
                return true;
            
            return false;
        }
        public static List<WoWUnit> getAdds()
        {
            if (Me.IsInInstance && Me.IsInParty)
            {
                List<WoWUnit> AddList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
unit.Guid != Me.Guid &&
unit.IsTargetingMyPartyMember &&
!unit.IsFriendly &&
!unit.IsPet &&
!unit.IsTotem &&
unit != Me.CurrentTarget &&
!Styx.Logic.Blacklist.Contains(unit.Guid));

                return AddList;
            }
            else
            {
                List<WoWUnit> AddList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
               unit.Guid != Me.Guid &&
               unit.IsTargetingMeOrPet &&
               !unit.IsFriendly &&
               !unit.IsPet &&
               !unit.IsTotem &&
               unit != Me.CurrentTarget &&
               !Styx.Logic.Blacklist.Contains(unit.Guid));

                return AddList;
            }
        }
        public static List<WoWUnit> getallunits()
        {
            List<WoWUnit> AllUnitsList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
           unit.Guid != Me.Guid &&
           !unit.IsFriendly &&
           !unit.Combat &&
           !Styx.Logic.Blacklist.Contains(unit.Guid));

            return AllUnitsList;

        }
        public static List<WoWUnit> getTotems()
        {
            List<WoWUnit> TotemsList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
           unit.Guid != Me.Guid &&
           !unit.IsFriendly &&
           !unit.IsPet &&
           unit.IsTotem &&
           !Styx.Logic.Blacklist.Contains(unit.Guid));

            return TotemsList;

        }
        public static void FindClostestPlayer(int Range)
        {
            List<WoWPlayer> PlrNearList2 = (from p in ObjectManager.GetObjectsOfType<WoWPlayer>() let d = p.Distance where d <= Range && !p.Dead && !p.IsFriendly orderby d ascending select p).ToList();

            if (IsBattleGround() && PlrNearList2.Count > 0)
            {
                Log("My CurrentTarget died or got removed, finding new.");
                PlrNearList2[0].Target();
            }
        }
        public static List<WoWUnit> getAdds2()
        {
            List<WoWUnit> AddList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
           unit.Guid != Me.Guid &&
           (unit.IsTargetingMeOrPet || unit.IsTargetingMyPartyMember) &&
           !unit.IsFriendly &&
           !unit.IsPet &&
           //unit != Me.CurrentTarget &&
           !Styx.Logic.Blacklist.Contains(unit.Guid));
           //Log("Adds = {0}", AddList.Count().ToString());
           return AddList;

        }
        /*public static WoWUnit FindDoubleTarget()
        {
            List<WoWUnit> AddList = (from p in ObjectManager.GetObjectsOfType<WoWPlayer>()
                                     let d = p.HealthPercent
                                     where d <= HealthP && p.Guid != Me.Guid &&
                                         (p.IsTargetingMeOrPet || p.IsTargetingMyPartyMember) &&
                                         !p.IsFriendly &&
                                         !p.IsPet &&
                                         p != Me.CurrentTarget &&
                                         !Styx.Logic.Blacklist.Contains(p.Guid)
                                     orderby d ascending
                                     select p).ToList();



            return AddList;

        }
         * */
        public static bool Adds()
        {

            List<WoWUnit> mobList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                unit.Guid != Me.Guid &&
                unit.IsTargetingMeOrPet &&
                !unit.IsFriendly &&
                !Styx.Logic.Blacklist.Contains(unit.Guid));

            if (mobList.Count > 0)
            {
                return true;
            }
            return false;

        }

        public RunStatus AoE(string Spellnames)
        {
            Log("Casting {0}", Spellnames.ToString());
            SpellManager.Cast(Spellnames);
            LastCast = SpellManager.Spells[Spellnames].Id;
            return RunStatus.Success;
        }
        public RunStatus AoE(string Spellnames, WoWPoint Location)
        {
            Log("Casting {0}", Spellnames.ToString());
            SpellManager.Cast(Spellnames);
            LastCast = SpellManager.Spells[Spellnames].Id;
            Thread.Sleep(500);
            LegacySpellManager.ClickRemoteLocation(Location);
            return RunStatus.Success;
        }

        public static bool GotMyDot(string AuraName, WoWUnit UnitCheck)
        {
            if (UnitCheck.ActiveAuras.ContainsKey(AuraName) && UnitCheck.ActiveAuras[AuraName].CreatorGuid != Me.Guid)
            {
                return false;
            }
            if (!UnitCheck.ActiveAuras.ContainsKey(AuraName))
            {
                return false;
            }
            if (UnitCheck.ActiveAuras.ContainsKey(AuraName) && UnitCheck.ActiveAuras[AuraName].CreatorGuid == Me.Guid)
            {
                return true;
            }
            return false;
        }

        public static double GotMyDot(string AuraName, WoWUnit UnitCheck, bool retTimer)
        {
            if (UnitCheck.ActiveAuras.ContainsKey(AuraName) && UnitCheck.ActiveAuras[AuraName].CreatorGuid != Me.Guid)
            {
                return 0;
            }
            if (!UnitCheck.ActiveAuras.ContainsKey(AuraName))
            {
                return 0;
            }
            if (UnitCheck.ActiveAuras.ContainsKey(AuraName) && UnitCheck.ActiveAuras[AuraName].CreatorGuid == Me.Guid)
            {
                return UnitCheck.ActiveAuras[AuraName].TimeLeft.TotalSeconds;
            }
            return 0;
        }

        private static bool IsNotWanding
        {
            get
            {
                if (Lua.GetReturnVal<int>("return IsAutoRepeatSpell(\"Shoot\")", 0) == 1) { return false; }
                if (Lua.GetReturnVal<int>("return HasWandEquipped()", 0) == 0) { return false; }
                return true;
            }
        }
        
        private static bool PetActionOnCooldown(int Action)
        {
            return Me.PetSpells[Action - 1].Cooldown;
        }




        public static bool PartyHaveCurse()
        {
            List<int> playerBuffs = new List<int>();
            foreach (WoWPlayer p in Me.PartyMembers)
            {
                if (p.Debuffs.Values.ToList().Exists(aura => aura.Spell.DispelType == WoWDispelType.Curse))
                {
                    playerBuffs.Add(1);
                }
            }
            if (playerBuffs.Count > 0)
            {
                playerBuffs.Clear();
                return true;
            }
            else
            {
                playerBuffs.Clear();
                return false;
            }
        }
        public static void PartyDeCurse()
        {

            foreach (WoWPlayer p in Me.PartyMembers)
            {
                if (p.Debuffs.Values.ToList().Exists(aura => aura.Spell.DispelType == WoWDispelType.Curse))
                {
                    Log("Removing Curse from {0}", p.Name);
                    SpellManager.Cast("Remove Curse", p);
                }
            }

        }


        public static WoWItem HealthStone;
        public static bool HaveHealthStone()
        {
            if (!SpellManager.HasSpell("Create Healthstone"))
            {
                //to help eleaviate low level number crunching by returning false of i dont have the spell.
                return false;
            }

            if (HealthStone == null)
            {
                foreach (WoWItem item in Me.BagItems)
                {
                    if (item.Entry == 5512)
                    {
                        HealthStone = item;
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HealthStoneNotCooldown()
        {
            if (HealthStone != null && HealthStone.BaseAddress != 0)
            {
                
                if (HealthStone.Cooldown == 0)
                {
                    return true;
                }

            }
            return false;
        }
        public static void UseHealthStone()
        {
            if (HealthStone != null && HealthStoneNotCooldown())
            {
                Log("Popping HealthStone");
                HealthStone.Use();
            }
        }

        public bool isItemInCooldown(WoWItem item)
        {
            if (Equals(null, item))
                return true;

            string cd_st;
            Lua.DoString("s=GetItemCooldown(" + item.Entry + ")");
            cd_st = Lua.GetLocalizedText("s", ObjectManager.Me.BaseAddress);
            if (cd_st == "0")
                return false;

            return true;

        }

        private WoWItem HaveItemCheck(List<int> listId)
        {

           
            foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>(false))
            {
                if (listId.Contains(Convert.ToInt32(item.Entry)))
                {

                    return item;
                }
            }
            return null;
        }

        private static bool IsBattleGround()
        {
            return Styx.Logic.Battlegrounds.IsInsideBattleground;
        }




        WoWItem CurrentManaPotion;
        public static ulong LastTargetManaPot;
        public bool HaveManaPotion()
        {
            //whole idea is to make sure CurrentHealthPotion is not null, and to check once every battle. 
            if (CurrentManaPotion == null)
            {
                if (LastTargetManaPot == null || Me.CurrentTarget.Guid != LastTargetManaPot) //Meaning they are not the same. 
                {
                    LastTargetManaPot = Me.CurrentTarget.Guid; // set guid to current target. 
                    List<WoWItem> ManaPot =
                    (from obj in
                         Me.BagItems.Where(
                             ret => ret != null && ret.BaseAddress != 0 &&
                             (ret.ItemInfo.ItemClass == WoWItemClass.Consumable) &&
                             (ret.ItemInfo.ContainerClass == WoWItemContainerClass.Potion) &&
                             (ret.ItemSpells[0].ActualSpell.SpellEffect1.EffectType == WoWSpellEffectType.Energize))
                     select obj).ToList();
                    if (ManaPot.Count > 0)
                    {

                        //on first check, set CurrentHealthPotion so we dont keep running the list looking for one, 
                        CurrentManaPotion = ManaPot.FirstOrDefault();
                        Log("Potion Found {0}", ManaPot.FirstOrDefault().Name);
                        return true;

                    }
                }


                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ManaPotionReady()
        {
            if (CurrentManaPotion != null && CurrentManaPotion.BaseAddress != 0)
            {
                if (CurrentManaPotion.Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void UseManaPotion()
        {
            if (CurrentManaPotion != null && CurrentManaPotion.BaseAddress != 0)
            {
                if (CurrentManaPotion.Cooldown == 0)
                {
                    Log("Using {0} to Regen Mana", CurrentManaPotion.Name.ToString());
                    CurrentManaPotion.Use();
                }
            }
        }







        public static ulong LastTargetHPPot;
        WoWItem CurrentHealthPotion;
        public bool HaveHealthPotion()
        {
            //whole idea is to make sure CurrentHealthPotion is not null, and to check once every battle. 
            if (CurrentHealthPotion == null)
            {
                if (LastTargetHPPot == null || Me.CurrentTarget.Guid != LastTargetHPPot) //Meaning they are not the same. 
                {
                    LastTarget = Me.CurrentTarget.Guid; // set guid to current target. 
                    List<WoWItem> HPPot =
                    (from obj in
                         Me.BagItems.Where(
                             ret => ret != null && ret.BaseAddress != 0 &&
                             (ret.ItemInfo.ItemClass == WoWItemClass.Consumable) &&
                             (ret.ItemInfo.ContainerClass == WoWItemContainerClass.Potion) &&
                             (ret.ItemSpells[0].ActualSpell.SpellEffect1.EffectType == WoWSpellEffectType.Heal))
                     select obj).ToList();
                    if (HPPot.Count > 0)
                    {
                        
                        //on first check, set CurrentHealthPotion so we dont keep running the list looking for one, 
                        CurrentHealthPotion = HPPot.FirstOrDefault();
                        Log("Potion Found {0}", HPPot.FirstOrDefault().Name);
                        return true;

                    }
                }


                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HealthPotionReady()
        {
            if (CurrentHealthPotion != null && CurrentHealthPotion.BaseAddress != 0)
            {
                if (CurrentHealthPotion.Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void UseHealthPotion()
        {
            if (CurrentHealthPotion != null && CurrentHealthPotion.BaseAddress != 0)
            {
                if (CurrentHealthPotion.Cooldown == 0)
                {
                    Log("Using {0} to Regen Health", CurrentHealthPotion.Name.ToString());
                    CurrentHealthPotion.Use();
                }
            }
        }

        public void ISFManager()
        {
            if (SpellManager.HasSpell("Soul Fire"))
            {
                if (ZerfallSettings.Instance.Use_Soulburn && SpellManager.CanCast("Soulburn"))
                {
                    SpellManager.Cast("Soulburn");
                    StyxWoW.SleepForLagDuration();
                }

                SpellManager.Cast("Soul Fire");
                LastCast = SoulFire;
                ISFTimer.Reset();
                ISFTimer.Start();
            }
        }

        public void ImmoManager()
        {
            
            SpellManager.Cast("Immolate");
            LastCast = Immolate;
            ImmoTimer.Reset();
            ImmoTimer.Start();
        }
    }


}

