using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hera
{
    public static class Settings
    {
        internal static List<string> IgnoreSettings = new List<string>(new[] { "ConfigFile", "ConfigFolder", "DirtyData", "Environment", "EnvironmentLoading", "_debugKey" });
        public static string ConfigFolder
        {
            get
            {
                const string basePath = @"CustomClasses\Codemplosion Druid\Class Specific\Settings\";
                string hbPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string workingPath = Path.Combine(hbPath, basePath);

                return workingPath;
            }
        }

        public static string ConfigFile
        {
            get
            {
                const string basePath = @"CustomClasses\Codemplosion Druid\Class Specific\Settings\Settings.xml";

                string hbPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string workingPath = Path.Combine(hbPath, basePath);

                return workingPath;
            }
        } 
        //public static string ConfigFile = @"CustomClasses\Codemplosion Druid\Class Specific\Settings\Settings.xml";
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

        // Healing Balance
        public static string RejuvenationBalance { get; set; }
        public static int RejuvenationBalanceHealth { get; set; }
        public static string NourishBalance { get; set; }
        public static int NourishBalanceHealth { get; set; }
        public static string RegrowthBalance { get; set; }
        public static int RegrowthBalanceHealth { get; set; }
        public static int InnervateManaBalance { get; set; }
        public static string RemoveCorruptionBalance { get; set; }
        public static string ThornsBalance { get; set; }
        public static string BarkskinBalance { get; set; }
        public static int BarkskinBalanceHealth{ get; set; }

        // Healing Feral Cat
        public static string RejuvenationFeralCat { get; set; }
        public static int RejuvenationFeralCatHealth { get; set; }
        public static string NourishFeralCat { get; set; }
        public static int NourishFeralCatHealth { get; set; }
        public static string RegrowthFeralCat { get; set; }
        public static int RegrowthFeralCatHealth { get; set; }
        public static int InnervateManaFeralCat { get; set; }
        public static string RemoveCorruptionFeralCat { get; set; }
        public static string ThornsFeralCat { get; set; }
        public static string BarkskinFeralCat { get; set; }
        public static int BarkskinFeralCatHealth { get; set; }
        public static int SurvivalInstinctsFeralCatHealth { get; set; }
        public static string SurvivalInstinctsFeralCat { get; set; }
        public static string DoubleDotFeralCat { get; set; }
        public static string PredatorsSwiftnessFeralCat { get; set; }
        public static string PredatorsSwiftnessFeralCatSpell { get; set; }
        public static int PredatorsSwiftnessFeralCatHealth { get; set; }
        public static string FaerieFireFeralCat { get; set; }

        // Healing Restoration
        // Tank
        public static string SwiftmendTank { get; set; }
        public static int SwiftmendTankHealth { get; set; }
        public static string HealingTouchTank { get; set; }
        public static int HealingTouchTankHealth { get; set; }
        public static string RegrowthTank { get; set; }
        public static int RegrowthTankHealth { get; set; }
        public static string RejuvenationTank { get; set; }
        public static int RejuvenationTankHealth { get; set; }
        public static string NourishTank { get; set; }
        public static int NourishTankHealth { get; set; }
        public static string LifebloomTank { get; set; }
        public static int LifebloomTankHealth { get; set; }
        public static string ThornsTankRestoration { get; set; }
        // Party Members
        public static string SwiftmendParty { get; set; }
        public static int SwiftmendPartyHealth { get; set; }
        public static string HealingTouchParty { get; set; }
        public static int HealingTouchPartyHealth { get; set; }
        public static string RegrowthParty { get; set; }
        public static int RegrowthPartyHealth { get; set; }
        public static string RejuvenationParty { get; set; }
        public static int RejuvenationPartyHealth { get; set; }
        public static string NourishParty { get; set; }
        public static int NourishPartyHealth { get; set; }
        // Everything else
        public static string WildGrowth{ get; set; }
        public static int WildGrowthHealth { get; set; }
        public static string Tranquility{ get; set; }
        public static int TranquilityHealth { get; set; }
        public static string RemoveCorruptionRestoration { get; set; }
        public static int InnervateRestoration { get; set; }
        public static string BarkskinRestoration { get; set; }
        public static string TreeOfLifeRestoration { get; set; }
        public static string RebirthRestoration { get; set; }

        
        // Combat Balance
        public static string PullSpellBalance { get { return ClassHelpers.Druid.CLCPullSpellBalance; } set { ClassHelpers.Druid.CLCPullSpellBalance = value; } }
        public static string PrimaryDPSSpell { get; set; }
        public static string MoonfireBalance { get; set; }
        public static string InsectSwarmBalance { get; set; }
        public static string FaerieFireBalance { get; set; }
        public static string Typhoon { get; set; }
        public static string ForceOfNature { get; set; }
        public static string Starfall { get; set; }
        public static string StarsurgeBalance { get; set; }
        public static string WildMushroomBalance { get; set; }
        public static string WildMushroomCountBalance { get; set; }
        public static string AOEDOTBalance { get; set; }
        
        
        public static int CatFormManaBalance { get; set; }

        // Combat Feral Cat
        public static int AttackEnergyFeralCat { get; set; }
        public static string PullSpellFeralCat { get; set; }
        public static string Rake { get; set; }
        public static string SkullBashCat{ get; set; }
        public static string SwipeCat { get; set; }
        public static string TigersFury { get; set; }
        public static string Shred { get; set; }
        public static string BerserkFeralCat { get; set; }
        public static string FeralCatRavage { get; set; }
        
        // Finishing moves
        public static string FerociousBite { get; set; }
        public static string FerociousBiteComboPoints { get; set; }
        public static string Rip { get; set; }
        public static string RipComboPoints { get; set; }
        public static string Maim { get; set; }
        public static string MaimComboPoints { get; set; }
        public static string SavageRoar { get; set; }
        public static string SavageRoarComboPoints { get; set; }
        
        

        // Misc 
        public static string Version { get; set; }
        public static string BearForm { get { return ClassHelpers.Druid.CLCBearForm; } set { ClassHelpers.Druid.CLCBearForm = value; } }
        public static int BearFormHealth { get { return ClassHelpers.Druid.CLCBearFormHealth; } set { ClassHelpers.Druid.CLCBearFormHealth = value; } }
        public static string TravelForm { get { return ClassHelpers.Druid.CLCTravelForm; } set { ClassHelpers.Druid.CLCTravelForm = value; } }
        public static int TravelFormMinDistance { get { return ClassHelpers.Druid.CLCTravelFormMinDistance; } set { ClassHelpers.Druid.CLCTravelFormMinDistance = value; } }
        public static int TravelFormHostileRange { get { return ClassHelpers.Druid.CLCTravelFormHostileRange; } set { ClassHelpers.Druid.CLCTravelFormHostileRange = value; } }
        public static string MarkOfTheWild { get; set; }
        public static string HealPets { get; set; }
        


        // Hidden Settings, not visible in the UI
        public static int _timermarker;
        public static string DoubleHeal{ get; set; }
        public static int StealthPullDistance{ get; set; }
        public static int FeralCatHealSpam { get; set; }
        public static string PriorityTargeting { get; set; }
        public static string ChaseFleeingTargets { get; set; }
        public static int MinTankHealth { get; set; }
        public static int MinTankHealthOverride { get; set; }
        public static int PetHealth { get; set; }
        


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
                    Utils.Log("It looks like the error has been fixed. Save the cheerleader, save the ... oh wait.", Utils.Colour("Green"));
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


        public static void PopulateRangedCapableMobs()
        {
            Utils.Log("-Populating ranged capable database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("RangedCapableMobs").Elements("Mob"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("EntryID").Value);
                    Utils.RangedCapableMobs.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulateRangedCapableMobs' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        public static void PopulatePriorityMobs()
        {
            Utils.Log("-Populating priority mobs database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("PriorityMobs").Elements("Mob"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("EntryID").Value);
                    Utils.PriorityMobs.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulatePriorityMobs' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        public static void PopulateHealingSpells()
        {
            Utils.Log("-Populating healing spells database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("HealingSpells").Elements("Spell"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("SpellID").Value);
                    Utils.HealingSpells.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulateHealingSpells' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        public static void PopulateImportantInterruptSpells()
        {
            Utils.Log("-Populating important interrupt spells database...");
            string path = ConfigFolder + "CommonData.xml";
            int count = 0;

            try
            {
                foreach (XElement ele in XDocument.Load(path).Root.Elements("ImportantInterruptSpells").Elements("Spell"))
                {
                    uint entryID = Convert.ToUInt32(ele.Element("SpellID").Value);
                    Utils.ImportantInterruptSpells.Add(entryID);
                    count += 1;
                }

                Utils.Log(string.Format("-{0} entries added", count));
            }
            catch (Exception e)
            {

                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log(String.Format("Exception in 'PopulateImportantInterruptSpells' \"{0}\"", e.Message));
                Utils.Log(" ");
                Utils.Log(" ");
                Utils.Log("**********************************************************************");
                Utils.Log("**********************************************************************");
            }
        }

        #endregion


    }
}