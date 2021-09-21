namespace PluginShattariHelper
{
    using Styx;
    using Styx.Combat.CombatRoutine;
    using Styx.Helpers;
    using Styx.Logic;
    using Styx.Logic.Combat;
    using Styx.Logic.Inventory;
    using Styx.Logic.Inventory.Frames.Gossip;
    using Styx.Logic.Pathing;
    using Styx.Logic.Profiles;
    using Styx.Plugins.PluginClass;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Linq;

public class ShatarriHelper : HBPlugin
   {
      public override string Name { get { return "Sha'tari Helper"; } }
      public override string Author { get { return "echelon17"; } }
      public override Version Version { get { return _version; } }
      private readonly Version _version = new Version(1, 0, 0, 0);
      public int numClaws = 0;
      public int numSpines = 0;
      public int numTalons = 0;
      public int numScales = 0;

      private static LocalPlayer Me { get { return ObjectManager.Me; } }
      
      // Borrowed from Netherwing Collector - thanks!
    	public static void movetoLoc(WoWPoint loc)
      {
			Mount.MountUp();
			while (loc.Distance(Me.Location) > 4)
			{
				Navigator.MoveTo(loc);
				Thread.Sleep(100);
				if (inCombat) return;
			}
			Thread.Sleep(2000);
      }
      
      static public bool inCombat
      {
			get
			{
				if (Me.Combat || !Me.IsAlive) return true;
				return false;
			}
      }

		public static int GetPing
		{
			get
			{
				return Lua.GetReturnVal<int>("return GetNetStats()", 2);
			}
		}
		
		static int NumOfItemsInBag(uint entry)
  {
    return Lua.GetReturnValues("return GetItemCount (\"" + entry.ToString() + "\")", "fish.lua")[0].ToInt32();
  }
		
      public override void Pulse() {
          // Check for the elixir aura:
          if (!Me.HasAura("Elixir of Shadows")) {
            foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>()) {
              if (item.Entry == 32446) {
								Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Using {0} we have {1}", item.Name, item.StackCount);
								Lua.DoString("UseItemByName(\"" + item.Name + "\")");
							}
						}
          }
          else {
            // Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Already have required aura.");
          }
        
        // Check if a skull pile is nearby
        ObjectManager.Update();
        List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .Where(o => (o.Distance <= Styx.Logic.LootTargeting.LootRadius && o.Entry == 185913))
                .OrderBy(o => o.Distance).ToList();
			foreach (WoWGameObject o in objList)
			{
                if (NumOfItemsInBag(32620) >= 10) {
                Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Activating Skull-Pile");
                  movetoLoc(o.Location);
                  if (inCombat) 
                  {
                    if (Me.Mounted) Mount.Dismount();
                    return;
                }
                o.Interact();
                Thread.Sleep(GetPing * 5 + 50);
                numClaws = NumOfItemsInBag(32716);
                numTalons = NumOfItemsInBag(32715);
                numSpines = NumOfItemsInBag(32717);
                numScales = NumOfItemsInBag(32718);
                //Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: {0} claws {1} talons {2} spines {3} scales", numClaws, numTalons, numSpines, numScales);
                if (numClaws < numTalons || numClaws < numSpines || numClaws < numScales) {
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Activating Gezzarak for Claws.");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("SelectGossipOption(\"1\")");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("CloseGossip()");
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Waiting for spawn.");
                  Thread.Sleep(7000);
                  break;
                }
                if (numTalons < numClaws || numTalons < numSpines || numTalons < numScales) {
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Activating Akkarai for Talons.");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("SelectGossipOption(\"2\")");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("CloseGossip()");
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Waiting for spawn.");
                  Thread.Sleep(7000);
                  break;
                }
                if (numSpines < numClaws || numSpines < numTalons || numSpines < numScales) {
                Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Activating Karrog for Spine.");
                Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("SelectGossipOption(\"3\")");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("CloseGossip()");
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Waiting for spawn.");
                  Thread.Sleep(7000);
                  break;
                }
                if (numScales < numClaws || numScales < numTalons || numScales < numSpines) {
                Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Activating Vakkiz for Scales.");
                Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("SelectGossipOption(\"4\")");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("CloseGossip()");
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Waiting for spawn.");
                  Thread.Sleep(7000);
                  break;
                }
                if (numClaws == numTalons && numClaws == numSpines && numClaws == numScales) {
                Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Activating Gezzarak for Claws (start loop).");
                Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("SelectGossipOption(\"1\")");
                  Thread.Sleep(GetPing * 5 + 50);
                  Lua.DoString("CloseGossip()");
                  Styx.Helpers.Logging.Write(Color.FromName("DarkRed"), "[Sha'tari Helper]: Waiting for spawn.");
                  Thread.Sleep(7000);
                  break;
                }
                }
			}
		}
}
}