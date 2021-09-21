#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: raphus $
// $Date: 2012-02-28 19:21:20 +0200 (Sal, 28 Şub 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/RogueSettings.cs $
// $LastChangedBy: raphus $
// $LastChangedDate: 2012-02-28 19:21:20 +0200 (Sal, 28 Şub 2012) $
// $LastChangedRevision: 604 $
// $Revision: 604 $

#endregion

using System.ComponentModel;
using Singular.ClassSpecific.Rogue;
using Styx.Helpers;

using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace Singular.Settings
{
    internal class RogueSettings : Styx.Helpers.Settings
    {
        public RogueSettings()
            : base(SingularSettings.SettingsPath + "_Rogue.xml")
        {
        }

        [Setting]
        [DefaultValue(PoisonType.Instant)]
        [Category("Common")]
        [DisplayName("Main Hand Poison")]
        [Description("Main Hand Poison")]
        public PoisonType MHPoison { get; set; }

        [Setting]
        [DefaultValue(PoisonType.Deadly)]
        [Category("Common")]
        [DisplayName("Off Hand Poison")]
        [Description("Off Hand Poison")]
        public PoisonType OHPoison { get; set; }

        [Setting]
        [DefaultValue(PoisonType.Wound)]
        [Category("Common")]
        [DisplayName("Thrown Poison")]
        [Description("Thrown Poison")]
        public PoisonType ThrownPoison { get; set; }


        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Interrupt Spells")]
        [Description("Interrupt Spells")]
        public bool InterruptSpells { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Use TotT")]
        [Description("Use TotT")]
        public bool UseTricksOfTheTrade { get; set; }


        [Setting]
        [DefaultValue(true)]
        [Category("Combat Spec")]
        [DisplayName("Use Rupture Finisher")]
        [Description("Use Rupture Finisher")]
        public bool CombatUseRuptureFinisher { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Combat Spec")]
        [DisplayName("Use Expose Armor")]
        [Description("Use Expose Armor")]
        public bool UseExposeArmor { get; set; }
    }
}