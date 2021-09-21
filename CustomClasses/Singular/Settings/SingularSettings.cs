#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: highvoltz $
// $Date: 2012-07-15 18:29:04 +0300 (Paz, 15 Tem 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/SingularSettings.cs $
// $LastChangedBy: highvoltz $
// $LastChangedDate: 2012-07-15 18:29:04 +0300 (Paz, 15 Tem 2012) $
// $LastChangedRevision: 646 $
// $Revision: 646 $

#endregion

using System.ComponentModel;

using Styx;
using Styx.Helpers;

using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace Singular.Settings
{
    internal class SingularSettings : Styx.Helpers.Settings
    {
        private static SingularSettings _instance;

        public SingularSettings() : base(SettingsPath + ".xml")
        {
        }

        public static string SettingsPath { get { return string.Format("{0}\\Settings\\SingularSettings_{1}", Logging.ApplicationPath, StyxWoW.Me.Name); } }

        public static SingularSettings Instance { get { return _instance ?? (_instance = new SingularSettings()); } }

        #region Category: General

        [Setting]
        [DefaultValue(false)]
        [Category("Movement")]
        [DisplayName("Disable Movement")]
        [Description("Disable all movement within the CC. This will NOT stop it from charging, blinking, etc. Only moving towards units, and facing will be disabled.")]
        public bool DisableAllMovement { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("General")]
        [DisplayName("Use Instance Rotation (Needs a restart !)")]
        [Description("When this is set to true, Singular will always use Instance rotations no matter what the current Context is.")]
        public bool UseInstanceRotation { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Targeting")]
        [DisplayName("Disable Targeting")]
        [Description("Disable all Targeting within the CC. This will NOT stop it from casting spells/heals on units other than your target. Only changing actual targets will be disabled.")]
        public bool DisableAllTargeting { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("General")]
        [DisplayName("Wait For Res Sickness")]
        [Description("Wait for resurrection sickness to wear off.")]
        public bool WaitForResSickness { get; set; }

        [Setting]
        [DefaultValue(65)]
        [Category("General")]
        [DisplayName("Min Health")]
        [Description("Minimum health to eat at.")]
        public int MinHealth { get; set; }

        [Setting]
        [DefaultValue(65)]
        [Category("General")]
        [DisplayName("Min Mana")]
        [Description("Minimum mana to drink at.")]
        public int MinMana { get; set; }
        [Setting]
        [DefaultValue(30)]
        [Category("General")]
        [DisplayName("Potion Health")]
        [Description("Minimum health to use a health pot or health stone at.")]
        public int PotionHealth { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("General")]
        [DisplayName("Potion Mana")]
        [Description("Minimum mana to use a mana pot at.")]
        public int PotionMana { get; set; }

        #endregion

        #region Category: Misc

        [Setting]
        [DefaultValue(false)]
        [Category("Misc")]
        [DisplayName("Debug Logging")]
        [Description("Enables debug logging from Singular. This will cause quite a bit of spam. Use it for diagnostics only.")]
        public bool EnableDebugLogging { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Misc")]
        [DisplayName("Disable Non Combat Behaviors")]
        [Description("Enabling that will disable non combat behaviors. (Rest, PreCombat buffs)")]
        public bool DisableNonCombatBehaviors { get; set; }


        [Setting]
        [DefaultValue(false)]
        [Category("Misc")]
        [DisplayName("Disable Pet usage")]
        [Description("Enabling that will disable pet usage")]
        public bool DisablePetUsage { get; set; }
        #endregion

        #region Category: Healing

        [Setting]
        [DefaultValue(95)]
        [Category("Healing")]
        [DisplayName("Ignore Targets Health")]
        [Description("Ignore healing targets when their health is above this value.")]
        public int IgnoreHealTargetsAboveHealth { get; set; }

        #endregion

        #region Category: Items

        [Setting]
        [DefaultValue(true)]
        [Category("Items")]
        [DisplayName("Use Flasks")]
        [Description("Uses Flask of the North or Flask of Enhancement.")]
        public bool UseAlchemyFlasks { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Items")]
        [DisplayName("Use First Trinket")]
        public bool Trinket1 { get; set; }

        [Setting]
        [DefaultValue(false)]
        [Category("Items")]
        [DisplayName("Use Second Trinket")]
        public bool Trinket2 { get; set; }

        [Setting]
        [DefaultValue(TrinketUsage.Never)]
        [Category("Items")]
        [DisplayName("Trinket 1 Usage")]
        public TrinketUsage Trinket1Usage { get; set; }

        [Setting]
        [DefaultValue(TrinketUsage.Never)]
        [Category("Items")]
        [DisplayName("Trinket 2 Usage")]
        public TrinketUsage Trinket2Usage { get; set; }

        #endregion

        #region Category: Racials

        [Setting]
        [DefaultValue(true)]
        [Category("Racials")]
        [DisplayName("Use Racials")]
        public bool UseRacials { get; set; }

        [Setting]
        [DefaultValue(30)]
        [Category("Racials")]
        [DisplayName("Gift of the Naaru HP")]
        [Description("Uses Gift of the Naaru when HP falls below this %.")]
        public int GiftNaaruHP { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Racials")]
        [DisplayName("Shadowmeld Threat Drop")]
        [Description("When in a group (and not a tank), uses shadowmeld as a threat drop.")]
        public bool ShadowmeldThreatDrop { get; set; }
        
        #endregion

        #region Category: Tanking

        [Setting]
        [DefaultValue(false)]
        [Category("Tanking")]
        [DisplayName("Disable Targeting")]
        public bool DisableTankTargetSwitching { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Tanking")]
        [DisplayName("Enable Taunting for tanks")]
        public bool EnableTaunting { get; set; }

        #endregion

        #region Class Late-Loading Wrappers

        // Do not change anything within this region.
        // It's written so we ONLY load the settings we're going to use.
        // There's no reason to load the settings for every class, if we're only executing code for a Druid.

        private DeathKnightSettings _dkSettings;

        private DruidSettings _druidSettings;

        private HunterSettings _hunterSettings;

        private MageSettings _mageSettings;
		
		private MonkSettings _monkSettings;
		
        private PaladinSettings _pallySettings;

        private PriestSettings _priestSettings;

        private RogueSettings _rogueSettings;

        private ShamanSettings _shamanSettings;

        private WarlockSettings _warlockSettings;

        private WarriorSettings _warriorSettings;

        [Browsable(false)]
        public DeathKnightSettings DeathKnight { get { return _dkSettings ?? (_dkSettings = new DeathKnightSettings()); } }

        [Browsable(false)]
        public DruidSettings Druid { get { return _druidSettings ?? (_druidSettings = new DruidSettings()); } }

        [Browsable(false)]
        public HunterSettings Hunter { get { return _hunterSettings ?? (_hunterSettings = new HunterSettings()); } }

        [Browsable(false)]
        public MageSettings Mage { get { return _mageSettings ?? (_mageSettings = new MageSettings()); } }
		
		[Browsable(false)]
        public MonkSettings Monk { get { return _monkSettings ?? (_monkSettings = new MonkSettings()); } }
		
        [Browsable(false)]
        public PaladinSettings Paladin { get { return _pallySettings ?? (_pallySettings = new PaladinSettings()); } }

        [Browsable(false)]
        public PriestSettings Priest { get { return _priestSettings ?? (_priestSettings = new PriestSettings()); } }

        [Browsable(false)]
        public RogueSettings Rogue { get { return _rogueSettings ?? (_rogueSettings = new RogueSettings()); } }

        [Browsable(false)]
        public ShamanSettings Shaman { get { return _shamanSettings ?? (_shamanSettings = new ShamanSettings()); } }

        [Browsable(false)]
        public WarlockSettings Warlock { get { return _warlockSettings ?? (_warlockSettings = new WarlockSettings()); } }

        [Browsable(false)]
        public WarriorSettings Warrior { get { return _warriorSettings ?? (_warriorSettings = new WarriorSettings()); } }

        #endregion
    }
}