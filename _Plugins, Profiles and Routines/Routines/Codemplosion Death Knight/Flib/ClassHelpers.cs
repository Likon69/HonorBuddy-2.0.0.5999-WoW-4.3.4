using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Drawing;


namespace Hera
{
    public static class ClassHelpers
    {
        public static class Hunter
        {
            public enum ClassType { None = 0, Beast, Marksmanship, Survival }

            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            public static ClassType ClassSpec { get; set; }
            

            public static class Aspect
            {
                
                public static string AspectOfTheCheetahRawSetting { get; set; }
                public static int CheetahMinimumDistance { get; set; }
                public static double CheetahHostileRange { get; set; }

                public static bool CanUseAspectOfTheCheetah
                {
                    get
                    {
                        if (Me.Combat) return false;
                        if (Me.Mounted) return false;
                        if (Me.IsSwimming) return false;
                        if (!Spell.IsKnown("Aspect of the Cheetah")) return false;
                        if (IsAspectOfTheCheetah) return false;
                        if (!CLC.ResultOK(AspectOfTheCheetahRawSetting)) return false;
                        if (!Spell.CanCast("Aspect of the Cheetah")) return false;
                        if (Me.Dead || Me.IsGhost) return false;
                        if (Me.GotAlivePet && Me.Pet.GotTarget) return false;

                        int poiDistance = (int)BotPoi.Current.Location.Distance(StyxWoW.Me.Location);

                        if (poiDistance < CheetahMinimumDistance) return false;
                        bool result = Utils.HostileMobsInRange(CheetahHostileRange);

                        if (!result) { Utils.Log(string.Format("Destination is {0} yards away and no hostiles in range, using Aspect of the Cheetah", poiDistance)); }

                        return !result;
                    }
                }

                public static bool NeedToSwapAspect
                {
                    get
                    {
                        if (Me.Mounted || Me.IsFlying) return false;
                        if (Me.Combat && IsAspectOfTheCheetah) return true;
                        if (!Me.Combat && !Self.IsPowerAbove(20) && !IsAspectOfTheFox && Spell.CanCast("Aspect of the Fox"))return true;
                        if (Me.Combat && !IsAspectOfTheHawk && Spell.CanCast("Aspect of the Hawk")) return true;

                        return false;
                    }
                }

                public static void AutoAspect()
                {
                    if (Me.Combat && !IsAspectOfTheHawk && Spell.CanCast("Aspect of the Hawk")) AspectOfTheHawk();
                    if (!Me.Combat && !IsAspectOfTheFox && Spell.CanCast("Aspect of the Fox")) AspectOfTheFox();

                }

                public static bool IsAspectOfTheHawk { get { return (Me.Auras.ContainsKey("Aspect of the Hawk")); } }

                public static bool IsAspectOfTheFox { get { return (Me.Auras.ContainsKey("Aspect of the Fox")); } }

                public static bool IsAspectOfTheCheetah { get { return (Me.Auras.ContainsKey("Aspect of the Cheetah")); } }
                
                public static void AspectOfTheHawk()
                {
                    const string aoh = "Aspect of the Hawk";
                    if (Spell.IsKnown(aoh) && Spell.CanCast(aoh)) Spell.Cast(aoh);
                }

                public static void AspectOfTheFox()
                {
                    const string aof = "Aspect of the Fox";
                    if (Spell.IsKnown(aof) && Spell.CanCast(aof)) Spell.Cast(aof);
                }

                public static void AspectOfTheCheetah()
                {
                    const string aoc = "Aspect of the Cheetah";
                    if (Spell.IsKnown(aoc) && Spell.CanCast(aoc)) Spell.Cast(aoc);
                }
            }

            public static class Traps
            {
                public static bool CanUseTrap(string trapName)
                {
                    if (!Spell.IsKnown(trapName) || Spell.IsOnCooldown(trapName)) return false;

                    return Spell.CanCast(trapName);
                }

                public static void UseTrap(string trapName)
                {
                    Spell.Cast(trapName);
                }
                

                public static bool CanUseFreezingTrap
                {
                    get
                    {
                        const string trapName = "Freezing Trap";

                        if (!Spell.IsKnown(trapName) || Spell.IsOnCooldown(trapName)) return false;

                        return Spell.CanCast(trapName);
                    }
                }
                public static void FreezingTrap()
                {
                    Spell.Cast("Freezing Trap");
                }

                public static bool CanUseSnakeTrap
                {
                    get
                    {
                        const string trapName = "Snake Trap";

                        if (!Spell.IsKnown(trapName) || Spell.IsOnCooldown(trapName)) return false;

                        return Spell.CanCast(trapName);
                    }
                }
                public static void SnakeTrap()
                {
                    Spell.Cast("Snake Trap");
                }
            }

            public static class Pet
            {
                public static string PetHappinessRawSetting { get; set; }
                public static double MendPetHealth { get; set; }
                public static string PetSlot { get; set; }
                public static string PetFoodName { get; set; }

                public static bool NeedToFeedPet
                {
                    get
                    {
                        if (string.IsNullOrEmpty(PetFoodName) || !Me.GotAlivePet || !Spell.IsKnown("Feed Pet") || !Spell.CanCast("Feed Pet") || Me.Pet.Auras.ContainsKey("Feed Pet")) return false;
                        if (!Inventory.HaveItem(PetFoodName)) return false;

                        return PetHappinessRawSetting.ToUpper().Contains("FEED PET") && !Me.Combat && Me.Pet.HappinessPercent < 75;
                    }
                }

                public static void FeedPet()
                {
                    Spell.Cast("Feed Pet");
                    Thread.Sleep(500);
                    Lua.DoString(String.Format("UseItemByName(\"{0}\")", PetFoodName)); 
                }


                public static bool NeedToHealPet
                {
                    get
                    {
                        if (!Me.GotAlivePet || !Spell.IsKnown("Mend Pet") || !Spell.CanCast("Mend Pet") || Me.Pet.Auras.ContainsKey("Mend Pet"))return false;

                        return PetHappinessRawSetting.Contains("Glyph of Mend Pet") && !Me.Combat && Me.Pet.HappinessPercent < 65 || Me.Pet.HealthPercent < MendPetHealth;
                    }
                }

                public static void HealPet() { Spell.Cast("Mend Pet"); }

                public static bool NeedToCallPet
                {
                    get
                    {
                        if (Me.Mounted || Me.GotAlivePet)return false;

                        Thread.Sleep(500);
                        ObjectManager.Update();

                        if (Me.GotAlivePet) return false;
                        if (!Spell.IsKnown("Control Pet") && !Me.GotAlivePet && Spell.CanCast("Revive Pet")) return true;

                        return Spell.CanCast(PetSlot);
                    }
                }

                public static void CallPet()
                {
                    Spell.Cast(PetSlot);
                    Thread.Sleep(1500);
                    if (Me.GotAlivePet) return;

                    if (Spell.CanCast("Revive Pet"))
                    {
                        Spell.Cast("Revive Pet");
                        Thread.Sleep(1000);
                        Utils.WaitWhileCasting();
                    }
                }

                public static void Attack()
                {
                    if (!Spell.IsKnown("Control Pet")) return;
                    if (!Me.GotAlivePet) return;
                    if (!Me.GotTarget) return;

                    Utils.Log("-Pet Attacking " + Me.CurrentTarget.Class, Utils.Colour("Blue"));

                    Lua.DoString("PetAttack()");
                }

                public static void Attack(WoWUnit mob)
                {
                    if (!Spell.IsKnown("Control Pet")) return;
                    if (!Me.GotAlivePet) return;
                    if (!Me.GotTarget) return;
                    if (mob == null) return;

                    Utils.Log("-Pet Attacking " + mob.CreatureType, Utils.Colour("Blue"));
                    Spell.Cast("PetAttack", mob);
                    //Lua.DoString("PetAttack()");
                }

                public static bool DefendMe()
                {
                    if (!Me.GotAlivePet || !Me.Combat || Me.Pet.CurrentTarget.CurrentTargetGuid == Me.Guid) return false;

                    foreach (WoWUnit thing in Targeting.Instance.TargetList.Where(thing => thing.CurrentTargetGuid == Me.Guid && Me.Pet.CurrentTargetGuid != thing.Guid))
                    {
                        Utils.Log(String.Format("Aggro from {0}, pet save my ass!", thing.Name), Color.FromName("DarkBlue"));
                        WoWUnit originalTarget = Me.CurrentTarget;
                        Utils.AutoAttack(false);
                        thing.Target();
                        Thread.Sleep(250);
                        Attack();
                        Thread.Sleep(500);
                        originalTarget.Target();
                        Utils.AutoAttack(true);
                        Thread.Sleep(500);
                        return true;
                    }


                    return false;
                }

            }
            
        }

        public static class Rogue
        {
            public enum ClassType { None = 0, Assassination, Combat, Subtlety }

            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            public static ClassType ClassSpec { get; set; }

            // Maximum pull distance for this class
            private static double _maximumDistance = 5;
            /// <summary>
            /// Maximum distance for casting spells or melee. If you are a caster set this to the maximum distance of your spell
            /// If you are melee set this to 5.5
            /// </summary>
            public static double MaximumDistance
            {
                get { return _maximumDistance; }
                set { _maximumDistance = value; }
            }

            public static WoWUnit SapTarget { get; set; }
            public static WoWUnit KillTarget { get; set; }
            public static bool CanShoot { get; set; }
            public static bool CanThrow { get; set; }

            // Move to distance for this class
            // If the target is more than MaxDistance away, the character will move to this (minimumDistance) distance
            // Useful for pulling thats at max range that are moving away from you, or fleeing targets
            private static double _minimumDistance = 3;
            /// <summary>
            /// If your distance from the target is greater than MaxDistance, it will move to this distance
            /// Set this to a few yards less than MaxDistance
            /// </summary>
            public static double MinimumDistance
            {
                get { return _minimumDistance; }
                set { _minimumDistance = value; }
            }
            
            public static class Poisons
            {
                private enum WeaponHand { MainHand, OffHand }

                public static string MainHandPoison { get; set; }
                public static string OffHandPoison { get; set; }

                // Deadly Poison Entry IDs
                private static readonly List<uint> PoisonDeadlyEntryID = new List<uint> { 2892, 2893, 8984, 8985, 20844, 22053, 22054, 43232, 43233 };

                // Instant Poison Entry IDs
                private static readonly List<uint> PoisonInstantEntryID = new List<uint> { 6947, 6949, 6950, 8926, 8927, 8928, 21927, 43230, 43231 };

                // Crippling Poison Entry IDs
                private static readonly List<uint> PoisonCripplingEntryID = new List<uint> { 3775 };

                // Wounding Poison Entry IDs
                private static readonly List<uint> PoisonWoundEntryID = new List<uint> { 10918, 10920, 10921, 10922, 22055, 43234, 43235 };



                private static WoWItem InstantPoisons { get { return Me.CarriedItems.FirstOrDefault(item => PoisonInstantEntryID.Contains(item.Entry)); } }
                private static WoWItem DeadlyPoisons { get { return Me.CarriedItems.FirstOrDefault(item => PoisonDeadlyEntryID.Contains(item.Entry)); } }
                private static WoWItem CripplingPoisons { get { return Me.CarriedItems.FirstOrDefault(item => PoisonCripplingEntryID.Contains(item.Entry)); } }
                private static WoWItem WoundingPoisons { get { return Me.CarriedItems.FirstOrDefault(item => PoisonWoundEntryID.Contains(item.Entry)); } }


                private static bool IsPoisonApplied(WeaponHand checkWeaponHand)
                {
                    List<string> weaponEnchants = Lua.GetReturnValues("return GetWeaponEnchantInfo()", "stuff.lua");

                    if (weaponEnchants != null)
                    {
                        bool result;
                        switch (checkWeaponHand)
                        {
                            case WeaponHand.MainHand:
                                result = weaponEnchants[0] == string.Empty || weaponEnchants[0] == "nil";
                                return !result;

                            case WeaponHand.OffHand:
                                result = weaponEnchants[3] == string.Empty || weaponEnchants[3] == "nil";
                                return !result;
                        }
                    }

                    return false;
                }

                public static bool NeedPoisons(out bool needMainHandPoison, out bool needOffHandPoison)
                {
                    needMainHandPoison = false;
                    needOffHandPoison = false;

                    if (!Spell.IsKnown("Poisons")) { return false; }

                    bool gotMainPoison = false;
                    bool gotOffPoison = false;
                    uint mainHandEnchant = Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id;
                    uint offHandEnchant = Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id;

                    // Poisons are applied
                    if (mainHandEnchant != 0 && offHandEnchant != 0) return false;

                    // Check if we have poisons in our inventory
                    switch (MainHandPoison)
                    {
                        case "[none]": break;
                        case "Instant Poison": gotMainPoison = InstantPoisons != null; break;
                        case "Crippling Poison": gotMainPoison = CripplingPoisons != null; break;
                        case "Deadly Poison": gotMainPoison = DeadlyPoisons != null; break;
                        case "Wounding Poison": gotMainPoison = WoundingPoisons != null; break;
                    }

                    switch (OffHandPoison)
                    {
                        case "[none]": break;
                        case "Instant Poison": gotOffPoison = InstantPoisons != null; break;
                        case "Crippling Poison": gotOffPoison = CripplingPoisons != null; break;
                        case "Deadly Poison": gotOffPoison = DeadlyPoisons != null; break;
                        case "Wounding Poison": gotOffPoison = WoundingPoisons != null; break;
                    }


                    // Poisons are not applied
                    if (gotMainPoison && mainHandEnchant == 0)
                    {
                        needMainHandPoison = true;
                        return true;
                    }
                    if (gotOffPoison && offHandEnchant == 0)
                    {
                        needOffHandPoison = true;
                        return true;
                    }

                    return false;
                }

                public static void ApplyPoisons()
                {
                    bool needMainHandPoison;
                    bool needOffHandPoison;
                    WoWItem mainPoison = null;
                    WoWItem offPoison = null;

                    if (!NeedPoisons(out needMainHandPoison, out needOffHandPoison)) return;

                    if (Me.Mounted) Mount.Dismount();
                    if (Me.IsMoving) WoWMovement.MoveStop();


                    if (needMainHandPoison)
                        switch (MainHandPoison)
                        {
                            case "[none]": break;
                            case "Instant Poison": mainPoison = InstantPoisons; break;
                            case "Crippling Poison": mainPoison = CripplingPoisons; break;
                            case "Deadly Poison": mainPoison = DeadlyPoisons; break;
                            case "Wounding Poison": mainPoison = WoundingPoisons; break;
                        }

                    if (needOffHandPoison)
                        switch (OffHandPoison)
                        {
                            case "[none]": break;
                            case "Instant Poison": offPoison = InstantPoisons; break;
                            case "Crippling Poison": offPoison = CripplingPoisons; break;
                            case "Deadly Poison": offPoison = DeadlyPoisons; break;
                            case "Wounding Poison": offPoison = WoundingPoisons; break;
                        }


                    if (needMainHandPoison && !IsPoisonApplied(WeaponHand.MainHand))
                    {
                        Utils.Log(string.Format("Applying {0} to {1} in main hand", mainPoison.Name, Me.Inventory.Equipped.MainHand.Name));
                        Lua.DoString("UseItemByName(\"" + mainPoison.Entry + "\")");
                        Utils.LagSleep();
                        Lua.DoString("UseInventoryItem(16)");
                        Utils.LagSleep();
                        Utils.WaitWhileCasting();
                    }

                    if (needOffHandPoison && !IsPoisonApplied(WeaponHand.OffHand))
                    {
                        Utils.Log(string.Format("Applying {0} to {1} in off hand", offPoison.Name, Me.Inventory.Equipped.OffHand.Name));
                        Lua.DoString("UseItemByName(\"" + offPoison.Entry + "\")");
                        Utils.LagSleep();
                        Lua.DoString("UseInventoryItem(17)");
                        Utils.LagSleep();
                        Utils.WaitWhileCasting();
                    }

                }
            }


        }

        public static class Paladin
        {

            public enum ClassType { None = 0, Holy, Protection, Retribution}

            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            public static ClassType ClassSpec { get; set; }


            public static string Auras { get; set; }
            public static string Blessings{ get; set; }
            public static string Seals { get; set; }

            // Maximum pull distance for this class
            private static double _maximumDistance = 5;
            /// <summary>
            /// Maximum distance for casting spells or melee. If you are a caster set this to the maximum distance of your spell
            /// If you are melee set this to 5.5
            /// </summary>
            public static double MaximumDistance { get { return _maximumDistance; } set { _maximumDistance = value; } }

            // Move to distance for this class
            // If the target is more than MaxDistance away, the character will move to this (minimumDistance) distance
            // Useful for pulling thats at max range that are moving away from you, or fleeing targets
            private static double _minimumDistance = 3;
            /// <summary>
            /// If your distance from the target is greater than MaxDistance, it will move to this distance
            /// Set this to a few yards less than MaxDistance
            /// </summary>
            public static double MinimumDistance
            {
                get { return _minimumDistance; }
                set { _minimumDistance = value; }
            }

            public static class Aura
            {
                public static bool NeedToApplyAura
                {
                    get
                    {
                        if (Me.Dead || Me.IsGhost) return false;
                        if (!Me.Mounted && !Auras.Contains("Automatic"))
                        {
                            if (Self.IsBuffOnMe(Auras)) return false;
                            return Spell.IsKnown(Auras);
                        }

                        if (!Me.Mounted && !Self.IsBuffOnMe("Retribution Aura") && Spell.CanCast("Retribution Aura")) return true;
                        if (!Me.Mounted && !Self.IsBuffOnMe("Devotion Aura") && !Self.IsBuffOnMe("Retribution Aura") && Spell.CanCast("Devotion Aura")) return true;
                        if (Me.Mounted && !Self.IsBuffOnMe("Crusader Aura") && Spell.CanCast("Crusader Aura")) return true;

                        return false;
                    }
                }

                public static void ApplyAura()
                {
                    if (!Auras.Contains("Automatic"))
                    {
                        if (Self.IsBuffOnMe(Auras)) return;
                        Spell.Cast(Auras);
                        return;
                    }

                    if (!Me.Mounted && Spell.CanCast("Retribution Aura") && !Self.IsBuffOnMe("Retribution Aura"))
                    {
                        Spell.Cast("Retribution Aura");
                        return;
                    }

                    if (!Me.Mounted && Spell.CanCast("Devotion Aura") && !Self.IsBuffOnMe("Devotion Aura"))
                    {
                        Spell.Cast("Devotion Aura");
                        return;
                    }

                    if (Me.Mounted && Spell.CanCast("Crusader Aura") && !Self.IsBuffOnMe("Crusader Aura"))
                    {
                        Spell.Cast("Crusader Aura");
                        return;
                    }
                }
            }

            public static class Blessing
            {
                public static bool NeedBlessing
                {
                    get
                    {
                        if (Me.Dead || Me.IsGhost) return false;
                        if (Me.Mounted) return false;

                        if (!Blessings.Contains("Automatic"))
                        {
                            return !Self.IsBuffOnMe(Blessings) && Spell.IsKnown(Blessings);
                        }

                        if (Self.IsBuffOnMe("Blessing of Kings")) return false;
                        if (Self.IsBuffOnMe("Blessing of Might")) return false;
                        if (!Spell.IsKnown("Blessing of Kings")) return false;

                        if (Self.IsBuffOnMe("Mark of the Wild") && !Self.IsBuffOnMe("Blessing of Might") && Spell.IsKnown("Blessing of Might")) return true;
                        if (!Self.IsBuffOnMe("Blessing of Kings")) return true;

                        return false;
                    }
                }


                public static void ApplyBlessing()
                {
                    if (!Blessings.Contains("Automatic"))
                    {
                        if (Self.IsBuffOnMe(Blessings)) return;
                        Spell.Cast(Blessings, Me);
                        return;
                    }

                    if (Self.IsBuffOnMe("Mark of the Wild")) { Spell.Cast("Blessing of Might"); return; }

                    Spell.Cast("Blessing of Kings", Me);
                }
            }

            public static class Seal
            {
                public static bool NeedToaApplySeal
                {
                    get
                    {
                        if (Me.Dead || Me.IsGhost) return false;
                        if (Me.Mounted) return false;
                        if (!Timers.Expired("Seal", 15 * 1000)) return false;
                        if (!Seals.Contains("Automatic")) { if (Self.IsBuffOnMe(Seals)) return false; return Spell.IsKnown(Seals); }

                        /*
                        if (!Self.IsHealthPercentAbove(50))
                        {
                            return (Spell.IsKnown("Seal of Insight") && !Self.IsBuffOnMe("Seal of Insight"));
                        }
                         */

                        if (Spell.IsKnown("Seal of Truth") && Target.IsInstanceBoss) { return !Me.ActiveAuras.ContainsKey("Seal of Truth"); }

                        if (Spell.IsKnown("Seal of Righteousness") && !Me.ActiveAuras.ContainsKey("Seal of Righteousness")) return true;

                        return false;
                    }
                }

                public static void ApplySeal()
                {
                    Timers.Reset("Seal");

                    if (!Seals.Contains("Automatic")) { if (Self.IsBuffOnMe(Seals)) return; Spell.Cast(Seals); return; }

                    /*
                    if (!Self.IsHealthPercentAbove(50))
                    {
                        if (Spell.IsKnown("Seal of Insight") && !Self.IsBuffOnMe("Seal of Insight")) Spell.Cast("Seal of Insight");
                        return;
                    }
                     */

                    if (Spell.IsKnown("Seal of Truth") && !Me.ActiveAuras.ContainsKey("Seal of Truth") && Target.IsInstanceBoss) { Spell.Cast("Seal of Truth"); return; }

                    if (Spell.IsKnown("Seal of Righteousness") && !Me.ActiveAuras.ContainsKey("Seal of Righteousness")) Spell.Cast("Seal of Righteousness");

                }
            }
            
        }

        public static class Priest
        {

            public enum ClassType { None = 0, Discipline, Holy, Shadow}

            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            public static ClassType ClassSpec { get; set; }

            // Maximum pull distance for this class
            private static double _maximumDistance = 5;
            /// <summary>
            /// Maximum distance for casting spells or melee. If you are a caster set this to the maximum distance of your spell
            /// If you are melee set this to 5.5
            /// </summary>
            public static double MaximumDistance { get { return _maximumDistance; } set { _maximumDistance = value; } }

            // Move to distance for this class
            // If the target is more than MaxDistance away, the character will move to this (minimumDistance) distance
            // Useful for pulling thats at max range that are moving away from you, or fleeing targets
            private static double _minimumDistance = 3;
            /// <summary>
            /// If your distance from the target is greater than MaxDistance, it will move to this distance
            /// Set this to a few yards less than MaxDistance
            /// </summary>
            public static double MinimumDistance
            {
                get { return _minimumDistance; }
                set { _minimumDistance = value; }
            }

            public static double PartyGuardianSpirit { get; set; }
            public static double PartyPrayerOfMending { get; set; }
            public static double PartyPenance { get; set; }
            public static double PartyPainSuppression { get; set; }
            public static double PartyPWS { get; set; }
            public static double PartyRenew { get; set; }
            public static double PartyFlashHeal { get; set; }
            public static double PartyGreaterHeal { get; set; }
            public static string VerboseHealing { get; set; }


            public static bool PartyHealer(WoWUnit healTarget)
            {
                const string guardianSpirit = "Guardian Spirit";
                const string prayerOfMending = "Prayer of Mending";
                const string penance = "Penance";
                const string painSuppression = "Pain Suppression";
                const string pws = "Power Word: Shield";
                const string renew = "Renew";
                const string flashHeal = "Flash Heal";
                const string greaterHeal = "Greater Heal";
                bool result = false;
                const int sleepTime = 25;

                if (healTarget != null)
                {
                    if (healTarget.HealthPercent < PartyGuardianSpirit && Spell.CanCast(guardianSpirit) && !healTarget.Auras.ContainsKey(guardianSpirit) && !healTarget.IsPet && healTarget.Combat)
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyGuardianSpirit[{2}]", healTarget.Class, healTarget.HealthPercent, PartyGuardianSpirit));
                        result = Spell.Cast(guardianSpirit, healTarget, true);
                        Thread.Sleep(sleepTime);
                    }

                    if (healTarget.HealthPercent < PartyPrayerOfMending && Spell.CanCast(prayerOfMending) && !healTarget.Auras.ContainsKey(prayerOfMending) && !healTarget.IsPet && healTarget.Combat)
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyPrayerOfMending[{2}]", healTarget.Class, healTarget.HealthPercent, PartyPrayerOfMending));
                        bool resultother = Spell.Cast(prayerOfMending, healTarget,true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }

                    if (healTarget.HealthPercent < PartyPWS && Spell.CanCast(pws) && Utils.CountOfMobsAttackingPlayer(healTarget.Guid) > 0 && !healTarget.Auras.ContainsKey(pws) && !healTarget.Auras.ContainsKey("Weakened Soul") && !healTarget.IsPet)
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyPWS[{2}]", healTarget.Class, healTarget.HealthPercent, PartyPWS));
                        bool resultother = Spell.Cast(pws, healTarget, true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }

                    if (healTarget.HealthPercent < PartyPenance && Spell.CanCast(penance) && !healTarget.IsPet)
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyPenance[{2}]", healTarget.Class, healTarget.HealthPercent, PartyPenance));
                        bool resultother =Spell.Cast(penance, healTarget,true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }

                    if (healTarget.HealthPercent < PartyPainSuppression && Spell.CanCast(painSuppression) && Utils.CountOfMobsAttackingPlayer(healTarget.Guid) > 0 && !healTarget.Auras.ContainsKey(painSuppression) && !healTarget.IsPet && healTarget.Combat)
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyPainSuppression[{2}]", healTarget.Class, healTarget.HealthPercent, PartyPainSuppression));
                        bool resultother = Spell.Cast(painSuppression, healTarget,true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }
                    

                    if (healTarget.HealthPercent < PartyRenew && Spell.CanCast(renew) && !healTarget.Auras.ContainsKey(renew))
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyRenew[{2}]", healTarget.Class, healTarget.HealthPercent, PartyRenew));
                        bool resultother = Spell.Cast(renew, healTarget,true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }

                    if (healTarget.HealthPercent < PartyFlashHeal && Spell.CanCast(flashHeal))
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyFlashHeal[{2}]", healTarget.Class, healTarget.HealthPercent, PartyFlashHeal));
                        if (Spell.CanCast("Inner Focus") && !Self.IsBuffOnMe("Inner Focus")) { Spell.Cast("Inner Focus"); System.Threading.Thread.Sleep(500); }
                        bool resultother =Spell.Cast(flashHeal, healTarget,true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }

                    if ((healTarget.HealthPercent < PartyGreaterHeal && Spell.CanCast(greaterHeal) && !healTarget.IsPet) && RAF.PartyTankRole != null && healTarget.Guid == RAF.PartyTankRole.Guid)
                    {
                        if (VerboseHealing.Contains("always")) Utils.Log(string.Format("-{0} HP[{1}] < PartyGreaterHeal[{2}]", healTarget.Class, healTarget.HealthPercent, PartyGreaterHeal));
                        if (Spell.CanCast("Inner Focus") && !Self.IsBuffOnMe("Inner Focus")) { Spell.Cast("Inner Focus"); System.Threading.Thread.Sleep(500); }

                        bool resultother = Spell.Cast(greaterHeal, healTarget, true);
                        if (!result) result = resultother;
                        Thread.Sleep(sleepTime);
                    }
                }

                return result;
            }

        }

        public static class Common
        {
            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            private static WoWUnit CT { get { return ObjectManager.Me.CurrentTarget; } }

            /// <summary>
            /// A temporary 'place holder' for a WoWUnit
            /// </summary>
            public static WoWUnit TempTarget { get; set; }

            /// <summary>
            /// If you are unlikely to die but your health is below a healing check value then don't heal. 
            /// There is no point healing if your target is going to die before you are. 
            /// </summary>
            /// <param name="absoluteMinimumHealth">If below this value (ie 25) then continue with heal</param>
            /// <param name="healingModifierSolo">Modify current health percent by this value. EG 80 percent of your health)</param>
            /// <returns>TRUE if it is safe to skip healing. FALSE if you need to heal</returns>
            public static bool SkipHeal(int absoluteMinimumHealth, double healingModifierSolo)
            {
                // If its safe to skip healing return TRUE. 

                double modifier = healingModifierSolo / 100;

                if (Utils.Adds) return false;
                if (Target.IsElite) return false;
                if (Utils.IsBattleground) return false;
                if (!Self.IsHealthPercentAbove(absoluteMinimumHealth)) return false;

                if ((Me.HealthPercent * modifier) > CT.HealthPercent) return true;

                return false;
            }

            public static WoWUnit DecursePlayer(List<WoWDispelType> canCureList, bool includePartyMembers)
            {
                if (canCureList.Count == 0) return null;

                if ((from aura in Me.Auras where aura.Value.IsHarmful from dispelType in canCureList where dispelType == aura.Value.Spell.DispelType select aura).Any())
                {
                    return Me;
                }

                if (!includePartyMembers) return null;
                if (Me.IsInParty || Me.IsInRaid)
                {
                    List<WoWPlayer> myPartyOrRaidGroup = Me.IsInRaid ? Me.RaidMembers : Me.PartyMembers;
                    WoWUnit player = (from p in myPartyOrRaidGroup from aura in p.Auras where aura.Value.IsHarmful from dispelType in canCureList where dispelType == aura.Value.Spell.DispelType select p).FirstOrDefault();
                    
                    if (Utils.IsBattleground && player.Distance > 50) return null;
                    return player;
                }

                return null;
            }

            

            /// <summary>
            /// Return the unit with a crowd control (user defined) spell on it. If none are CC'ed it will return null
            /// </summary>
            /// <param name="crowdControlSpellName"></param>
            /// <param name="maximumDistance"></param>
            /// <returns></returns>
            public static WoWUnit GotCrowdControledUnit(string crowdControlSpellName, double maximumDistance)
            {
                List<WoWUnit> hlist = (from o in ObjectManager.ObjectList
                                       where o is WoWUnit
                                       let p = o.ToUnit()
                                       where p.Distance2D < maximumDistance
                                             && !p.Dead
                                             //&& p.IsTargetingMeOrPet
                                             && p.Auras.ContainsKey(crowdControlSpellName)
                                       //&& p.Attackable
                                       select p).ToList();

                return hlist.Count > 0 ? hlist[0] : null;
            }

            /// <summary>
            /// Do we have a suitable unit to apply a crowd control spell to?
            /// </summary>
            /// <param name="crowdControlSpellName">Name of the spell to cast on the target</param>
            /// <param name="maximumDistance">Maximum distance of the target</param>
            /// <param name="creatureType">Type of creature to apply our CC spell to</param>
            /// <returns></returns>
            public static bool CanCrowdControlUnit(string crowdControlSpellName,double maximumDistance,WoWCreatureType creatureType)
            {
                WoWUnit wUnit = (from o in ObjectManager.ObjectList
                                 where o is WoWUnit
                                 let p = o.ToUnit()
                                 where
                                     p.Distance2D < maximumDistance && !p.Dead && p.IsTargetingMeOrPet &&
                                     p.CreatureType == creatureType && !p.Auras.ContainsKey(crowdControlSpellName) &&
                                     p.HealthPercent > 95 &&
                                     p.Attackable
                                 select p).FirstOrDefault();

                return wUnit != null;
            }

            /// <summary>
            /// Do it! Apply our crowd control spell to the appropriate unit. 
            /// </summary>
            /// <param name="crowdControlSpellName"></param>
            /// <param name="maximumDistance"></param>
            /// <param name="creatureType"></param>
            public static void CrowdControlUnit(string crowdControlSpellName, double maximumDistance, WoWCreatureType creatureType)
            {
                WoWUnit ccUnit = (from o in ObjectManager.ObjectList
                                 where o is WoWUnit
                                 let p = o.ToUnit()
                                 where
                                     p.Distance2D < maximumDistance && !p.Dead && p.IsTargetingMeOrPet &&
                                     p.CreatureType == creatureType && !p.Auras.ContainsKey(crowdControlSpellName) &&
                                     p.HealthPercent > 95 &&
                                     p.Attackable
                                 select p).FirstOrDefault();

                if (ccUnit != null)
                {
                    // Cast the CC spell on the appropriate target
                    Spell.Cast(crowdControlSpellName, ccUnit);
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                    while (Spell.IsGCD) Thread.Sleep(250);
                    ObjectManager.Update();


                    // Now retarget (if we need to) a mob without our CC spell
                    if (Me.CurrentTarget.Auras.ContainsKey(crowdControlSpellName))
                    {
                        ObjectManager.Update();
                        WoWUnit attackUnit = (from o in ObjectManager.ObjectList
                                              where o is WoWUnit
                                              let p = o.ToUnit()
                                              where
                                                  p.Distance2D < 40 && !p.Dead && p.IsTargetingMeOrPet &&
                                                  !p.Auras.ContainsKey(crowdControlSpellName) && p.Attackable
                                              orderby p.HealthPercent ascending
                                              select p).FirstOrDefault();
                        attackUnit.Target();
                        Thread.Sleep(1000);
                    }


                }
            }
        }

        public static class Mage
        {

            public enum ClassType { None = 0, Arcane, Fire, Frost}

            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            public static ClassType ClassSpec { get; set; }

            // Maximum pull distance for this class
            private static double _maximumDistance = 5;
            /// <summary>
            /// Maximum distance for casting spells or melee. If you are a caster set this to the maximum distance of your spell
            /// If you are melee set this to 5.5
            /// </summary>
            public static double MaximumDistance { get { return _maximumDistance; } set { _maximumDistance = value; } }

            // Move to distance for this class
            // If the target is more than MaxDistance away, the character will move to this (minimumDistance) distance
            // Useful for pulling thats at max range that are moving away from you, or fleeing targets
            private static double _minimumDistance = 3;
            /// <summary>
            /// If your distance from the target is greater than MaxDistance, it will move to this distance
            /// Set this to a few yards less than MaxDistance
            /// </summary>
            public static double MinimumDistance
            {
                get { return _minimumDistance; }
                set { _minimumDistance = value; }
            }

            /// <summary>
            /// TRUE if you have a sheeped mob within range of the location
            /// </summary>
            /// <param name="location"></param>
            /// <param name="distance"></param>
            /// <returns></returns>
            public static bool GotSheep(WoWPoint location, double distance)
            {
                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>())
                {
                    if (!unit.Auras.ContainsKey("Polymorph")) continue;
                    if (unit.Location.Distance(location) > distance) continue;

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Checks a 12 yard radius for a sheeped target
            /// </summary>
            /// <returns></returns>
            public static bool GotSheep()
            {
                return GotSheep(12);
            }

            public static bool GotSheep(int maxDistance)
            {
                WoWUnit sheepedUnit = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Distance2D < maxDistance && p.Auras.ContainsKey("Polymorph") select p).FirstOrDefault();
                return (sheepedUnit != null);
            }

            public static WoWUnit SheepUnit
            {
                get
                {
                    WoWUnit sheepedUnit = (from o in ObjectManager.ObjectList where o is WoWUnit let p = o.ToUnit() where p.Auras.ContainsKey("Polymorph") select p).FirstOrDefault();
                    return sheepedUnit;
                }
            }



            public static class WaterElemental
            {
                public static bool NeedToCall
                {
                    get
                    {
                        if (Me.Mounted) return false;
                        if (ClassSpec != ClassType.Frost) return false;
                        if (Me.GotAlivePet) return false;

                        return Spell.CanCast("Summon Water Elemental");
                    }
                }

                public static void Call()
                {
                    Spell.Cast("Summon Water Elemental");
                    Thread.Sleep(1000);
                }

                public static void Attack()
                {
                    Utils.Log("-Elemental Attacking " + Me.CurrentTarget.Class, Utils.Colour("Blue"));
                    Lua.DoString("PetAttack()");
                }

            }

            public static class ConjuredItems
            {
                // The below code is taken from CnG's Mage all credit to him
                private static readonly List<int> ConjureRefreshmentItems = new List<int> { 65500, 65515, 65516, 65517, 43518, 43523, 65499 };

                private static int GotRefreshments
                {
                    get
                    {
                        return (from items in ConjureRefreshmentItems from item in ObjectManager.GetObjectsOfType<WoWItem>(false) where item.Entry == items select items).FirstOrDefault();
                    }
                }

                public static void MakeRefreshment()
                {
                    Spell.Cast("Conjure Refreshment");
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                }

                public static bool GotRefreshment()
                {
                    return GotRefreshments != 0;
                }

                // Mana Gems
                public static bool ManaGemNotCooldown()
                {
                    return _manaGem != null && _manaGem.Cooldown == 0;
                }

                public static WoWItem ManaGem
                {
                    get { return _manaGem; }
                }

                public static void UseManaGem()
                {
                    if (_manaGem != null && ManaGemNotCooldown())
                    {
                        Lua.DoString(string.Format("UseItemByName(\"{0}\")", _manaGem.Name));
                    }
                }

                private static WoWItem _manaGem;
                public static bool HaveManaGem()
                {

                    foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>(false).Where(item => item.Entry == 36799))
                    {
                        _manaGem = item;
                        return true;
                    }

                    return false;
                }
            }

        }

        public static class DeathKnight
        {

            public enum ClassType { None = 0, Blood, Frost, Unholy}

            private static LocalPlayer Me { get { return ObjectManager.Me; } }
            public static ClassType ClassSpec { get; set; }


            // Maximum pull distance for this class
            private static double _maximumDistance = 5;
            /// <summary>
            /// Maximum distance for casting spells or melee. If you are a caster set this to the maximum distance of your spell
            /// If you are melee set this to 5.5
            /// </summary>
            public static double MaximumDistance { get { return _maximumDistance; } set { _maximumDistance = value; } }

            // Move to distance for this class
            // If the target is more than MaxDistance away, the character will move to this (minimumDistance) distance
            // Useful for pulling thats at max range that are moving away from you, or fleeing targets
            private static double _minimumDistance = 3;
            /// <summary>
            /// If your distance from the target is greater than MaxDistance, it will move to this distance
            /// Set this to a few yards less than MaxDistance
            /// </summary>
            public static double MinimumDistance
            {
                get { return _minimumDistance; }
                set { _minimumDistance = value; }
            }

            public static class Pet
            {
                public static bool NeedToCallPet
                {
                    get
                    {
                        if (Me.Mounted || Me.IsFlying || Me.GotAlivePet) return false;

                        //Thread.Sleep(500);
                        //ObjectManager.Update();

                        if (Me.GotAlivePet) return false;
                        if (ClassSpec != ClassType.Unholy) return false;

                        return Spell.CanCast("Raise Dead");
                    }
                }

                public static void CallPet()
                {
                    Spell.Cast("Raise Dead");
                }
            }


        }



    }
}
