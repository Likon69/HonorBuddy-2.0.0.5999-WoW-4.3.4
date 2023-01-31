using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Helpers;

namespace PvPRogue.Helpers
{
    internal static class FlagReturn
    {
        internal static void ChatFilter(WoWChat.ChatSimpleMessageEventArgs e)
        {
            //if (!e.Message.Contains("Flag was dropped by")) return;

            //Alliance Flag = 179830 
            //Horde Flag = 179831

            // Find our Flag
            WoWGameObject Flag = (from CurObj in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                                  where CurObj.WithinInteractRange && CurObj.SubType != WoWGameObjectType.FlagStand
                                  where StyxWoW.Me.IsAlliance && CurObj.Entry == 179830
                                  where StyxWoW.Me.IsHorde && CurObj.Entry == 179831
                                  select CurObj).FirstOrDefault();


            if (Flag == null) return;

            // Interact
            Flag.Interact();

            Log.Write("Returning flag!");
        }
    }
}
