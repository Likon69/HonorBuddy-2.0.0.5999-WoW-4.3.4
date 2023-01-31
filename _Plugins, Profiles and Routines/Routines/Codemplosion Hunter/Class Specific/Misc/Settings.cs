using System;
using System.Collections.Generic;

namespace Hera
{
    public static class Settings
    {
        public static List<string> _ignoreSettings = new List<string>(new[] { "ConfigFile", "DirtyData" });
        public static string ConfigFile = @"CustomClasses\Codemplosion Hunter\Class Specific\Misc\Settings.xml";

        // Backing fields
        private static int _maximumPullDistance;
        private static int _minimumPullDistance;
        private static int _combatTimeout;
        private static string _aspectOfTheCheetah;
        private static int _cheetahHostileRange;
        private static int _cheetahMinDistance;
        private static string _petSlot;
        private static int _mendPetHealth;
        private static string _petHappiness;
        private static string _petFoodName;
        private static string _lazyRaider;

        // Common settings template

        public static string LazyRaider { get { return _lazyRaider; } set { _lazyRaider = value; Target.LazyRaider = value; Movement.LazyRaider = value;} }
        public static int PreLootMinDistance{ get; set; }
        public static int PreLootMaxHealth { get; set; }
        public static bool DirtyData { get; set; }
        public static string LowLevelCheck { get; set; }
        public static int RestHealth { get; set; }
        public static int RestMana { get; set; }
        public static string Debug { get; set; }
        public static string RAFTarget { get; set; }
        public static string ShowUI { get; set; }
        public static string SmartEatDrink { get; set; }
        public static string PullSpell { get; set; }
        public static int MaximumPullDistance { get { return _maximumPullDistance; } set { _maximumPullDistance = value; Movement.MaximumDistance = _maximumPullDistance; } } 
        public static int MinimumPullDistance { get { return _minimumPullDistance; } set { _minimumPullDistance = value; Movement.MinimumDistance = _minimumPullDistance; } }
        public static int CombatTimeout { get { return _combatTimeout; } set { _combatTimeout = value; Target.CombatTimeout = _combatTimeout; } }
        public static int HealthPotion { get; set; }
        public static int LifebloodHealth { get; set; }
        public static int FocusShot { get; set; }
        public static int ManaPotion { get; set; }
        
        // Aspect of the Cheetah
        public static string AspectOfTheCheetah { get { return _aspectOfTheCheetah; } set { _aspectOfTheCheetah = value; ClassHelpers.Hunter.Aspect.AspectOfTheCheetahRawSetting = _aspectOfTheCheetah; } }
        public static int CheetahMinDistance { get { return _cheetahMinDistance; } set { _cheetahMinDistance = value; ClassHelpers.Hunter.Aspect.CheetahMinimumDistance = _cheetahMinDistance; } }
        public static int CheetahHostileRange { get { return _cheetahHostileRange; } set { _cheetahHostileRange = value; ClassHelpers.Hunter.Aspect.CheetahHostileRange = _cheetahHostileRange; }  }

        // Misc
        public static string PreEmptiveLooting { get; set; }
        public static string SerpentSting { get; set; }
        public static string AimedShot { get; set; }
        public static string ArcaneShot { get; set; }
        public static int FeignDeathHealth { get; set; }
        public static string TrapInCombat { get; set; }
        public static string TrapOnAdds { get; set; }
        public static string BlackArrow { get; set; }
        public static string ExplosiveShot { get; set; }
        public static string KillCommand { get; set; }
        public static int DeterrenceHealth { get; set; }
        public static string RapidFire { get; set; }
        public static string BestialWrath { get; set; }
        public static string Intimidation { get; set; }
        public static string Misdirection { get; set; }
        public static string TrapLauncher { get; set; }
        public static string Disengage { get; set; }
        public static string WidowVenom { get; set; }
        public static string HuntersMark { get; set; }
        public static string ChimeraShot { get; set; }
        public static string FeignDeath { get; set; }
        public static string FocusFire { get; set; }
        public static string ConsumeFrenzyEffect { get; set; }
        public static string ContinuousPulling { get; set; }
        public static string PetAttackDelay { get; set; }

        // Pet
        public static string PetSlot { get { return _petSlot; } set { _petSlot = value; ClassHelpers.Hunter.Pet.PetSlot = _petSlot; } }
        public static string PetHappiness { get { return _petHappiness; } set { _petHappiness = value; ClassHelpers.Hunter.Pet.PetHappinessRawSetting = value; } }
        public static int MendPetHealth { get { return _mendPetHealth; } set { _mendPetHealth = value; ClassHelpers.Hunter.Pet.MendPetHealth = value; } }
        public static string PetFoodName { get { return _petFoodName; } set { _petFoodName = value; ClassHelpers.Hunter.Pet.PetFoodName = value; } }


        public static void Save()
        {
            ConfigSettings.FileName = Settings.ConfigFile;

            if (ConfigSettings.Open())
            {
                foreach (var p in typeof(Settings).GetProperties())
                {
                    if (p.Name.StartsWith("_") || _ignoreSettings.Contains(p.Name)) continue;

                    object propValue = typeof(Settings).GetProperty(p.Name).GetValue(p.Name, null);
                    ConfigSettings.SetProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name), propValue.ToString());
                }

                ConfigSettings.Save();
            }
        }

        public static void Load()
        {
            ConfigSettings.FileName = Settings.ConfigFile;

            if (ConfigSettings.Open())
            {
                foreach (var p in typeof(Settings).GetProperties())
                {
                    if (p.Name.StartsWith("_") || _ignoreSettings.Contains(p.Name)) continue;

                    switch (typeof(Settings).GetProperty(p.Name).PropertyType.Name)
                    {
                        case "Boolean": { p.SetValue(typeof(Settings), Convert.ToBoolean(ConfigSettings.GetBoolProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                        case "String": { p.SetValue(typeof(Settings), Convert.ToString(ConfigSettings.GetStringProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                        case "Int32": { p.SetValue(typeof(Settings), Convert.ToInt16(ConfigSettings.GetIntProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                    }
                }
            }
        }
    }
}
