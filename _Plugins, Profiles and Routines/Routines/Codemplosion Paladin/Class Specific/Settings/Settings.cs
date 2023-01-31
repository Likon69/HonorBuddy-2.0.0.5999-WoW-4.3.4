using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hera
{
    public static class Settings
    {
        internal static List<string> IgnoreSettings = new List<string>(new[] { "ConfigFile", "DirtyData", "Environment", "EnvironmentLoading","_debugKey" });
        public static string ConfigFolder = @"CustomClasses\Codemplosion Paladin\Class Specific\Settings\";
        public static string ConfigFile = @"CustomClasses\Codemplosion Paladin\Class Specific\Settings\Settings.xml";
        private static string _debugKey = "nothing has been ready yet";

        // Backing fields
        private static string _lazyRaider;
        private static int _maximumPullDistance;
        private static int _minimumPullDistance;
        private static int _combatTimeout;

        public static string LazyRaider { get { return _lazyRaider; } set { _lazyRaider = value; Target.LazyRaider = value; Movement.LazyRaider = value; } }

        // Common settings template
        //public static string Environment { get; set; }
        //public static string EnvironmentLoading { get; set; }

        private static string _auraBuffs; public static string AuraBuffs { get { return _auraBuffs; } set { _auraBuffs = value; ClassHelpers.Paladin.Auras = value; } }
        private static string _seals; public static string Seals { get { return _seals; } set { _seals = value; ClassHelpers.Paladin.Seals = value; } }
        private static string _blessings; public static string Blessings { get { return _blessings; } set { _blessings = value; ClassHelpers.Paladin.Blessings = value; } }

        public static bool DirtyData { get; set; }
        public static int RestHealth { get; set; }
        public static int RestMana { get; set; }
        public static string Debug { get; set; }
        public static string RAFTarget { get; set; }
        public static string ShowUI { get; set; }
        public static string SmartEatDrink { get; set; }
        public static int ManaPotion { get; set; }
        public static int HealthPotion { get; set; }
        public static int LifebloodHealth { get; set; }
        public static int DivinePleaMana { get; set; }
        public static string Cleanse { get; set; }

        public static int RestHealPercent { get; set; }
        public static int WordOfGloryHealth { get; set; }
        public static string WordOfGloryHolyPower { get; set; }
        public static int HolyLightHealth { get; set; }
        public static string HolyLight { get; set; }
        public static int DivineLightHealth { get; set; }
        public static int HolyShockHealth { get; set; }
        public static int DivineProtectionHealth { get; set; }
        public static int DivineShieldHealth { get; set; }
        public static int ArdentDefenderHealth { get; set; }
        public static int FlashOfLightHealth { get; set; }
        public static string FlashOfLight { get; set; }
        public static int LayOnHandsHealth { get; set; }
        public static string Consecration { get; set; }
        public static string HolyWrath { get; set; }
        public static string HammerOfJustice { get; set; }
        public static string AvengingWrath { get; set; }
        public static string Rebuke { get; set; }
        public static string TheArtOfWar { get; set; }
        public static string HammerOfWrath { get; set; }
        public static string AvengersShield { get; set; }
        public static string DivineStorm { get; set; }
        public static string RighteousFury { get; set; }
        public static string HammerOfTheRighteous { get; set; }
        public static string CrusaderStrike { get; set; }
        public static string GuardianOfAncientKings{ get; set; }
        public static string Zealotry{ get; set; }
        public static string HolyShock { get; set; }        // DPS Spell only 
        public static string SacredCleansing { get; set; }
        public static string PVPDance { get; set; }
        public static int PVPDanceInterval { get; set; }
        public static string Exorcism { get; set; }
        public static string ExorcismSeconds{ get; set; }
        public static string DivinePurpose { get; set; }

        // Holy Power usage
        public static string ShieldOfTheRighteous { get; set; }
        public static string ShieldOfTheRighteousHolyPower { get; set; }
        public static string TemplarsVerdict { get; set; }
        public static string TemplarsVerdictHolyPower { get; set; }
        public static string Inquisition { get; set; }
        public static string InquisitionHolyPower { get; set; }


        public static string PartyCleanse { get; set; }
        public static string PartyHealWhen { get; set; }
        public static int PartyFlashOfLight { get; set; }
        public static int PartyHolyLight { get; set; }
        public static int PartyDivineLight{ get; set; }
        public static int PartyHolyShock { get; set; }

        public static int MaximumPullDistance { get { return _maximumPullDistance; } set { _maximumPullDistance = value; Movement.MaximumDistance = _maximumPullDistance; } }
        public static int MinimumPullDistance { get { return _minimumPullDistance; } set { _minimumPullDistance = value; Movement.MinimumDistance = _minimumPullDistance; } }
        public static int CombatTimeout { get { return _combatTimeout; } set { _combatTimeout = value; Target.CombatTimeout = _combatTimeout; } }

        // Hidden Settings, not visible in the UI
        public static int HealingSpellTimer { get; set; }
        public static double HealingModifierSolo { get; set; }
        public static int HealingAbsoluteMinimum{ get; set; }



        #region Save and load settings
        public static void Save()
        {
            ConfigSettings.FileName = Settings.ConfigFile;

            if (ConfigSettings.Open())
            {
                foreach (PropertyInfo p in typeof(Settings).GetProperties())
                {
                    if (p.Name.StartsWith("_") || IgnoreSettings.Contains(p.Name)) continue;

                    object propValue = typeof(Settings).GetProperty(p.Name).GetValue(p.Name, null);
                    ConfigSettings.SetProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name), propValue.ToString());
                }

                ConfigSettings.Save();
            }
        }

        public static void Load()
        {
            ConfigSettings.FileName = Settings.ConfigFile;

            try
            {
                if (ConfigSettings.Open())
                {
                    foreach (PropertyInfo p in typeof(Settings).GetProperties())
                    {
                        if (p.Name.StartsWith("_") || IgnoreSettings.Contains(p.Name)) continue;
                        _debugKey = p.Name;

                        switch (typeof(Settings).GetProperty(p.Name).PropertyType.Name)
                        {
                            case "Boolean": { p.SetValue(typeof(Settings), Convert.ToBoolean(ConfigSettings.GetBoolProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                            case "String": { p.SetValue(typeof(Settings), Convert.ToString(ConfigSettings.GetStringProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                            case "Int32": { p.SetValue(typeof(Settings), Convert.ToInt16(ConfigSettings.GetIntProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                            case "Double": { p.SetValue(typeof(Settings), Convert.ToDouble(ConfigSettings.GetIntProperty(String.Format("//{0}/{1}", Codemplosion.CCClass, p.Name))), null); } break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in settings load \"{0}\"", e.Message));
                Utils.Log(string.Format("Last key attempted to be read was \"{0}\"", _debugKey));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }

        }
        #endregion


    }
}