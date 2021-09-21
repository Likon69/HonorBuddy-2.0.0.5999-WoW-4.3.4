#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: raphus $
// $Date: 2012-02-06 08:18:55 +0200 (Pzt, 06 Şub 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/HunterSettings.cs $
// $LastChangedBy: raphus $
// $LastChangedDate: 2012-02-06 08:18:55 +0200 (Pzt, 06 Şub 2012) $
// $LastChangedRevision: 587 $
// $Revision: 587 $

#endregion

using System;
using System.ComponentModel;

using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;

using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace Singular.Settings
{
    internal class HunterSettings : Styx.Helpers.Settings
    {
        public HunterSettings()
            : base(SingularSettings.SettingsPath + "_Hunter.xml")
        {
        }

        #region Category: Pet

        [Setting]
        [DefaultValue("1")]
        [Category("Pet")]
        [DisplayName("Pet Slot")]
        public string PetSlot { get; set; }

        [Setting]
        [DefaultValue(70)]
        [Category("Pet")]
        [DisplayName("Mend Pet Percent")]
        public double MendPetPercent { get; set; }

        #endregion

        #region Category: Common

        [Setting]
        [DefaultValue(false)]
        [Category("Common")]
        [DisplayName("Use Disengage")]
        [Description("Will be used in battlegrounds no matter what this is set")]
        public bool UseDisengage { get; set; }

        #endregion
    }
}