#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: Laria $
// $Date: 2011-05-03 18:16:12 +0300 (Sal, 03 May 2011) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/Settings/MonkSettings.cs $
// $LastChangedBy: Laria$
// $LastChangedDate: 2011-05-03 18:16:12 +0300 (Sal, 03 May 2011) $
// $LastChangedRevision: 307 $
// $Revision: 307 $

#endregion

namespace Singular.Settings
{
    internal class MonkSettings : Styx.Helpers.Settings
    {
        public MonkSettings()
            : base(SingularSettings.SettingsPath + "_Monk.xml")
        {
        }
    }
}