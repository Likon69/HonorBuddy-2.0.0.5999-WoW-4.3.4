using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.ComponentModel;
using Styx.Helpers;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace PvPRogue
{
    class ClassSettings : Styx.Helpers.Settings
    {

        public static ClassSettings _Instance;

        public ClassSettings()
            : base(Path.Combine(Path.Combine(Logging.ApplicationPath, "Settings"), string.Format("PvPRogue_{0}.xml", Styx.StyxWoW.Me.Name)))
        {
        }

        #region PVE
        [Setting]
        [DefaultValue(40)]
        [Category("PVE")]
        [DisplayName("Percent to rest at [PVE]")]
        [Description("What percent to rest @, this is for PVE only")]
        public int PVERestAt { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("PVE")]
        [DisplayName("Use Shadowstep as a opener")]
        [Description("to use Shadowstep as a opener")]
        public bool PVEStep { get; set; }

        #endregion 
        #region General
        [Setting]
        [DefaultValue(false)]
        [Category("General")]
        [DisplayName("Always Stealthed")]
        [Description("true/false - Enabling this will make your rogue walk around the BG in stealth, [Disabling Mount's]")]
        public bool GeneralAlwaysStealthed { get; set; }


        [Setting]
        [DefaultValue(false)]
        [Category("General")]
        [DisplayName("Try to Sap")]
        [Description("true/false - To try initial Sap on pulling")]
        public bool GeneralToSap { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("General")]
        [DisplayName("Poison Thrown Weapon")]
        [Description("To put Poison on throwning weapon")]
        public bool GeneralThrownPoison { get; set; }


        [Setting]
        [DefaultValue(true)]
        [Category("General")]
        [DisplayName("Enable Movement")]
        [Description("True/False - To enable movement of the CC - [ False for LazyRaiders ]")]
        public bool GeneralMovement { get; set; }


        #endregion


        #region GeneralMoves
        [Setting]
        [DefaultValue(4)]
        [Category("Moves")]
        [DisplayName("People Before Fan of Knives")]
        [Description("People around before using Fan of Knives")]
        public int MovesFOKPeople { get; set; }
        #endregion

        #region SUB
        [Setting]   
        [DefaultValue(eSubOpener.Ambush)]
        [Category("Subtlety")]
        [DisplayName("Sub Pull Opener")]
        [Description("Move to open with as subtlety")]
        public eSubOpener SubtletyOpener { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Subtlety")]
        [DisplayName("Initial Pull Burst")]
        [Description("Allow initial Pull Burst, (Use's a finisher instead of Recup)")]
        public bool SubtletyInitBurst { get; set; }

        [Setting]
        [DefaultValue(eSubFinisher.Eviscerate)]
        [Category("Subtlety")]
        [DisplayName("Finisher")]
        [Description("What move to use as finisher when target health > 35%")]
        public eSubFinisher SubtletyFinisher { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Subtlety")]
        [DisplayName("Kidney Shot - Smoke Bomb")]
        [Description("Should it use Smoke Bomb when using Kidney Shot")]
        public bool SubtletyKidneySmokeBomb { get; set; }

        [Setting]
        [DefaultValue(ePoison.Instant)]
        [Category("Subtlety")]
        [DisplayName("Main Hand Poison")]
        [Description("Poison to apply to main hand")]
        public ePoison SubtletyMainPoison { get; set; }


        [Setting]
        [DefaultValue(ePoison.Crippling)]
        [Category("Subtlety")]
        [DisplayName("Off Hand Poison")]
        [Description("Poison to apply to off hand")]
        public ePoison SubtletyOffHandPoison { get; set; }


        #endregion

    }

    public enum eSubFinisher
    {
        Kidney_Shot,
        Eviscerate
    }

    /// <summary>
    /// Enum for Sub Opening Move
    /// </summary>
    public enum eSubOpener
    {
        Garrote,
        Ambush
    }

    /// <summary>
    /// List of poison
    /// </summary>
    public enum ePoison
    {
        Instant,
        Crippling,
        MindNumbing,
        Deadly,
        Wound
    }
}
