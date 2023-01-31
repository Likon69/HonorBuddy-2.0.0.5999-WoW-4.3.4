using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Logic;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;

namespace PvPRogue.Managers
{
    public static class NavMan
    {

        public static void MoveBehind(WoWUnit Unit)
        {
            // Movement Checks
            if (IsGlueEnabled && Unit.Distance < 10) return;
            if (!ClassSettings._Instance.GeneralMovement) return;


            WoWPoint BehindLocation = WoWMathHelper.CalculatePointBehind(Unit.Location, Unit.Rotation, 1.7f);
            Navigator.MoveTo(BehindLocation);
        }


        public static bool IsGlueEnabled
        {
            get
            {
                int PluginCount = (from MyPlugin in Styx.Plugins.PluginManager.Plugins
                                   where MyPlugin.Name == "Glue" && MyPlugin.Enabled == true
                                   select MyPlugin).Count();

                if (PluginCount >= 1) 
                    return true;

                return false;
            }
        }

    }
}
