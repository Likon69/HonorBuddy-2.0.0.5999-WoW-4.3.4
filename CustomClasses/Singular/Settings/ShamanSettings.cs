#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: raphus $
// $Date: 2012-03-04 02:46:04 +0200 (Paz, 04 Mar 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/ShamanSettings.cs $
// $LastChangedBy: raphus $
// $LastChangedDate: 2012-03-04 02:46:04 +0200 (Paz, 04 Mar 2012) $
// $LastChangedRevision: 605 $
// $Revision: 605 $

#endregion

using System.ComponentModel;

using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;

using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace Singular.Settings
{
    internal class ShamanSettings : Styx.Helpers.Settings
    {
        public ShamanSettings()
            : base(SingularSettings.SettingsPath + "_Shaman.xml")
        {
        }

        #region Category: Totems
        [Setting]
        [DefaultValue(WoWTotem.None)]
        [Category("Totems")]
        [Description("The totem to use for this slot. Select 'None' for automatic usage.")]
        public WoWTotem EarthTotem { get; set; }
        [Setting]
        [DefaultValue(WoWTotem.None)]
        [Category("Totems")]
        [Description("The totem to use for this slot. Select 'None' for automatic usage.")]
        public WoWTotem WaterTotem { get; set; }
        [Setting]
        [DefaultValue(WoWTotem.None)]
        [Category("Totems")]
        [Description("The totem to use for this slot. Select 'None' for automatic usage.")]
        public WoWTotem AirTotem { get; set; }
        #endregion

        #region Category: Enhancement
        [Setting]
        [DefaultValue(CastOn.All)]
        [Category("Enhancement")]
        [DisplayName("Feral Spirit")]
        [Description("Selecet on what type of fight you would like to cast Feral Spirit")]
        public CastOn CastOn  { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Enhancement")]
        [DisplayName("Enhancement Heal")]
        public bool EnhancementHeal { get; set; }

        #endregion

        #region Category: Elemental

        [Setting]
        [DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Enable AOE Support")]
        public bool IncludeAoeRotation { get; set; }

        [Setting]
        [DefaultValue(true)]
        [Category("Elemental")]
        [DisplayName("Elemental Heal")]
        public bool ElementalHeal { get; set; }

        #endregion
    }
}