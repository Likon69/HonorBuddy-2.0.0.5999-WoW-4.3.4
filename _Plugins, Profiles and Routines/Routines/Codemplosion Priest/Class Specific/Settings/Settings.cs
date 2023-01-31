using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using Styx.WoWInternals;

namespace Hera
{
    public static class Settings
    {
        internal static List<string> IgnoreSettings = new List<string>(new[] { "ConfigFile", "DirtyData", "Environment", "EnvironmentLoading","_debugKey" });
        public static string ConfigFolder = @"CustomClasses\Codemplosion Priest\Class Specific\Settings\";
        public static string ConfigFile = @"CustomClasses\Codemplosion Priest\Class Specific\Settings\Settings.xml";
        private static string _debugKey = "nothing has been ready yet";

        // Backing fields
        private static string _lazyRaider;
        private static int _maximumPullDistance;
        private static int _minimumPullDistance;
        private static int _combatTimeout;

        public static string LazyRaider { get { return _lazyRaider; } set { _lazyRaider = value; Target.LazyRaider = value; Movement.LazyRaider = value; } }

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

        // Self Healing
        public static int FlashHealHealth { get; set; }
        public static int PowerWordShieldHealth { get; set; }
        public static int RenewHealth { get; set; }
        public static int PainSuppressionHealth { get; set; }

        // Party Healing
        public static int PartyHealerOOM { get; set; }
        public static string PartyCleanse { get; set; }
        public static string PartyHealWhen { get; set; }
        public static string PartyHealWhenSpec { get; set; }
        private static int _partyGuardianSpirit; 
        public static int PartyGuardianSpirit { get { return _partyGuardianSpirit; } set { _partyGuardianSpirit = value; ClassHelpers.Priest.PartyGuardianSpirit = value; } }
        private static int _partyPrayerOfMending;
        public static int PartyPrayerOfMending { get { return _partyPrayerOfMending; } set { _partyPrayerOfMending = value; ClassHelpers.Priest.PartyPrayerOfMending = value; } } 
        private static int _partyPenance;
        public static int PartyPenance { get { return _partyPenance; } set { _partyPenance = value; ClassHelpers.Priest.PartyPenance = value; } } 
        private static int _partyPainSuppression;
        public static int PartyPainSuppression { get { return _partyPainSuppression; } set { _partyPainSuppression = value; ClassHelpers.Priest.PartyPainSuppression = value; } }
        private static int _partyPWS;
        public static int PartyPWS { get { return _partyPWS; } set { _partyPWS = value; ClassHelpers.Priest.PartyPWS = value; } }
        private static int _partyRenew;
        public static int PartyRenew { get { return _partyRenew; } set { _partyRenew = value; ClassHelpers.Priest.PartyRenew = value; } }
        private static int _partyFlashHeal;
        public static int PartyFlashHeal { get { return _partyFlashHeal; } set { _partyFlashHeal = value; ClassHelpers.Priest.PartyFlashHeal = value; } }
        private static int _partyGreaterHeal;
        public static int PartyGreaterHeal { get { return _partyGreaterHeal; } set { _partyGreaterHeal = value; ClassHelpers.Priest.PartyGreaterHeal = value; } }
        private static string _verboseHealing;
        public static string VerboseHealing { get { return _verboseHealing; } set { _verboseHealing = value; ClassHelpers.Priest.VerboseHealing = value; } }
        public static string PrayerOfHealingCount { get; set; }
        public static int PrayerOfHealingHealth { get; set; }
        public static string CircleOfHealingCount { get; set; }
        public static int CircleOfHealingHealth { get; set; }
        public static string HealPets { get; set; }

        public static string InnerFireWill { get; set; }
        public static string PowerWordFortitude{ get; set; }
        public static string ShadowProtection { get; set; }

        public static string SmiteEvangelism { get; set; }
        public static int SmiteEvangelismHealth { get; set; }
        public static string Archangel { get; set; }
        public static string ArchangelParty { get; set; }
        public static string ResurrectPlayers { get; set; }
        public static string PowerWordBarrier { get; set; }
        public static string BouncePoM { get; set; }
        public static string WandParty { get; set; }
        public static int WandMana { get; set; }
        

        // Combat
        public static string PullSpell { get; set; }
        public static string Smite { get; set; }
        public static string ShadowWordPain { get; set; }
        public static string MindBlast { get; set; }
        public static string MindFlay { get; set; }
        public static string HolyFire{ get; set; }
        public static string DevouringPlague { get; set; }
        public static string ShadowWordDeath { get; set; }
        public static string Silence { get; set; }
        public static string VampiricTouch { get; set; }
        public static string Shadowform { get; set; }
        public static int ShadowfiendMana { get; set; }
        public static int ReserveMana { get; set; }
        public static int DispersionMana { get; set; }
        public static int HymnOfHopeMana { get; set; }
        public static string FearWard { get; set; }
        public static string MindSpike { get; set; }
        public static string Chastise { get; set; }
        public static string Penance { get; set; }
        public static string PowerInfusion { get; set; }
        public static string ShackleUndead { get; set; }
        public static string PWSBeforePull { get; set; }
        public static string PsychicScream { get; set; }
        public static string MindSear { get; set; }
        public static string HolyNova { get; set; }

        public static string Version { get; set; }

        public static int MaximumPullDistance { get { return _maximumPullDistance; } set { _maximumPullDistance = value; Movement.MaximumDistance = _maximumPullDistance; } }
        public static int MinimumPullDistance { get { return _minimumPullDistance; } set { _minimumPullDistance = value; Movement.MinimumDistance = _minimumPullDistance; } }
        public static int CombatTimeout { get { return _combatTimeout; } set { _combatTimeout = value; Target.CombatTimeout = _combatTimeout; } }

        // Hidden Settings, not visible in the UI
        public static int HealingSpellTimer { get; set; }
        public static double HealingModifierSolo { get; set; }
        public static int HealingAbsoluteMinimum{ get; set; }
        public static int InnerFocusMana { get; set; }



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