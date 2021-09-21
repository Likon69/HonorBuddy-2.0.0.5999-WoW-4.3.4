#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: highvoltz $
// $Date: 2012-07-07 06:02:12 +0300 (Cmt, 07 Tem 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/MageSettings.cs $
// $LastChangedBy: highvoltz $
// $LastChangedDate: 2012-07-07 06:02:12 +0300 (Cmt, 07 Tem 2012) $
// $LastChangedRevision: 645 $
// $Revision: 645 $

#endregion

using System.ComponentModel;
using Styx.Helpers;

namespace Singular.Settings
{
    internal class MageSettings : Styx.Helpers.Settings
    {
        public MageSettings()
            : base(SingularSettings.SettingsPath + "_Mage.xml")
        {

        }
        #region Category: Common

        [SettingAttribute]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Common")]
        [DisplayName("Summon Table If In A Party")]
        [Description("Summons a food table instead of using conjured food if in a party")]
        public bool SummonTableIfInParty { get; set; }

        #endregion
    }
}