using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hera
{
    public static class Settings
    {
        internal static List<string> IgnoreSettings = new List<string>(new[] { "ConfigFile", "DirtyData", "Environment", "EnvironmentLoading","_debugKey" });
        public static string ConfigFolder = @"CustomClasses\Codemplosion Death Knight\Class Specific\Settings\";
        public static string ConfigFile = @"CustomClasses\Codemplosion Death Knight\Class Specific\Settings\Settings.xml";
        private static string _debugKey = "nothing has been ready yet";

        // Backing fields
        private static int _loadErrorCount;
        private static string _lazyRaider;
        private static int _maximumPullDistance;
        private static int _minimumPullDistance;
        private static int _combatTimeout;

        public static string LazyRaider { get { return _lazyRaider; } set { _lazyRaider = value; Target.LazyRaider = value; Movement.LazyRaider = value; } }
        public static int MaximumPullDistance { get { return _maximumPullDistance; } set { _maximumPullDistance = value; Movement.MaximumDistance = _maximumPullDistance; } }
        public static int MinimumPullDistance { get { return _minimumPullDistance; } set { _minimumPullDistance = value; Movement.MinimumDistance = _minimumPullDistance; } }
        public static int CombatTimeout { get { return _combatTimeout; } set { _combatTimeout = value; Target.CombatTimeout = _combatTimeout; } }

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
        public static string Cleanse { get; set; }
        public static int RestHealPercent { get; set; }

        public static string MultipleEnvironment { get; set; }

        // Healing
        public static int DeathStrikeHealth { get; set; }
        public static string DeathStrike { get; set; }
        public static string RuneTap{ get; set; }
        public static int RuneTapHealth { get; set; }
        public static string BoneShield { get; set; }
        public static int BoneShieldHealth { get; set; }
        public static string IceboundFortitude { get; set; }
        public static int IceboundFortitudeHealth { get; set; }

        // Combat
        public static string PullSpell { get; set; }
        public static int DeathCoil { get; set; }
        public static string UnholyFrenzy { get; set; }
        public static string MindFreeze { get; set; }
        public static string Strangulate { get; set; }
        public static string HeartStrike { get; set; }
        public static string IcyTouch { get; set; }
        public static string PlagueStrike { get; set; }
        public static string ScourgeStrike { get; set; }
        public static string FrostStrike { get; set; }
        public static int FrostStrikeRunePower { get; set; }
        public static string BloodStrike { get; set; }
        public static string BloodBoil { get; set; }
        public static string Pestilence { get; set; }
        public static string RaiseDead { get; set; }
        public static string DeathAndDecay { get; set; }
        public static string ChainsOfIce { get; set; }
        public static string AttackDelay { get; set; }
        public static string Obliterate { get; set; }
        public static string AntiMagicShell { get; set; }
        public static string Presence { get; set; }

        public static string RuneWeapon { get; set; }
        public static string Gargoyle { get; set; }
        public static string HowlingBlast { get; set; }
        public static string HungeringCold { get; set; }
        public static string PillarOfFrost { get; set; }
        public static string BloodTap { get; set; }
        public static string FesteringStrike { get; set; }
        public static string Outbreak { get; set; }

        // Misc 
        public static string Version { get; set; }
        

        // Hidden Settings, not visible in the UI
        public static int AttackCooldown { get; set; }
        public static int BloodBoilCooldown { get; set; }
        public static int _timermarker;

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
            //string environment = Utils.IsBattleground ? "PVP" : "PVE";
            //environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            //ConfigSettings.CurrentEnvironment = environment;

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

                if (_loadErrorCount > 0)
                {
                    _loadErrorCount += 1;
                    Utils.Log("It looks like the error has been fixed. Save the cheerleader, save the ... oh wait.", Utils.Colour("Blue"));
                    System.IO.File.Delete(ConfigSettings.UserFileName);
                    _loadErrorCount = 0;
                    Load();
                    
                }
            }
            catch (Exception e)
            {
                if (_loadErrorCount < 1)
                {
                    _loadErrorCount += 1;
                    Utils.Log("There appears to be an issue with the character specific settings file. Trying to fix it...",Utils.Colour("Red"));
                    System.IO.File.Delete(ConfigSettings.UserFileName);
                    Utils.Log("It needs more cowbell...", Utils.Colour("Red"));
                    Load();
                }
                else
                {
                    Utils.Log("**********************************************************************");
                    Utils.Log("**********************************************************************");
                    Utils.Log(" ");
                    Utils.Log(" ");
                    Utils.Log(String.Format("Exception in settings load \"{0}\"", e.Message));
                    Utils.Log(string.Format("Last key attempted to be read was \"{0}\"", _debugKey));
                    Utils.Log(" ");
                    Utils.Log(" ");
                    Utils.Log(" THE CC WAS NOT LOADED. THE END IS NEIGH! REPENT YOUR SINS NOW!");
                    Utils.Log(" ");
                    Utils.Log("**********************************************************************");
                    Utils.Log("**********************************************************************");
                }
            }

        }

        #endregion


    }
}