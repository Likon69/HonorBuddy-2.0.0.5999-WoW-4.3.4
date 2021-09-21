using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

using Styx;
using Styx.Plugins.PluginClass;
using Styx.Logic.BehaviorTree;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.Logic.Pathing;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.Inventory.Frames.Quest;
using Styx.Logic.Questing;
using Styx.Plugins;
using Styx.Logic.Inventory.Frames.Gossip;
using Styx.Logic.Common;
using Styx.Logic.Inventory.Frames.Merchant;
using Styx.Logic;
using Styx.Logic.Profiles;
using Styx.Logic.Inventory.Frames.LootFrame;


namespace HighVoltz
{
	class NetherwingCollector: HBPlugin
	{
        // ********* you can modify the following settings ********************
        static bool CollectOre = true;
        static bool CollectHerbs = true;
        static bool CollectEggs = true;
        
        // ***** anything below here isn't meant to be modified *************
		public static string name { get { return "NetherwingCollector " + _version.ToString(); } }
		public override string Name { get { return name; } }
		public override string Author { get { return "HighVoltz"; } }
		private readonly static Version _version = new Version(1, 8);
		public override Version Version { get { return _version; } }
		public override string ButtonText { get { return "NetherwingCollector"; } }
		public override bool WantButton { get { return false; } }
		public static LocalPlayer Me = ObjectManager.Me;

		public override void OnButtonPress()
		{
		}

		public override void Pulse()
		{
			try
			{
                if (!inCombat)
					findAndPickupObject();
			}
			catch (ThreadAbortException) { }
			catch (Exception e)
			{
				Log("Exception in Pulse:{0}", e);
			}
		}

		public static void movetoLoc(WoWPoint loc)
		{
			Mount.MountUp();
			while (loc.Distance(Me.Location) > 8)
			{
				Navigator.MoveTo(loc);
				Thread.Sleep(100);
				if (inCombat) return;
			}
			Thread.Sleep(2000);
		}

		static public void findAndPickupObject()
		{
			ObjectManager.Update();
			List<WoWGameObject> objList = ObjectManager.GetObjectsOfType<WoWGameObject>()
				.Where(o => (o.Distance <= 150 &&
                    (CollectHerbs && o.Entry == 185881 && Me.GetSkill(SkillLine.Herbalism).CurrentValue >= 350) ||
				    (CollectOre  && o.Entry == 185877 && Me.GetSkill(SkillLine.Mining).CurrentValue >= 350)    || 
                    (CollectEggs && o.Entry == 185915)
					
					))
				.OrderBy(o => o.Distance).ToList();
			foreach (WoWGameObject o in objList)
			{
				movetoLoc(WoWMovement.CalculatePointFrom(o.Location, -4));
				if (inCombat) 
				{
					if (Me.Mounted) Mount.Dismount();
						return;
				}
				o.Interact();
				Thread.Sleep(GetPing * 2 + 250);
				while (!inCombat && Me.IsCasting)
					Thread.Sleep(100);
                Stopwatch lootTimer = new Stopwatch();
                // wait for loot frame to apear
                lootTimer.Reset();
                lootTimer.Start();
                while (LootFrame.Instance == null || !LootFrame.Instance.IsVisible)
                {
                    if (lootTimer.ElapsedMilliseconds > 5000)
                    {
                        Log("Loot window never showed up!");
                        return;
                    }
                    Thread.Sleep(100);
                }
                Lua.DoString("for i=1,GetNumLootItems() do ConfirmLootSlot(i) LootSlot(i) end");
                // wait for lootframe to close
                lootTimer.Reset();
                lootTimer.Start();
                while (LootFrame.Instance != null && LootFrame.Instance.IsVisible )
                {
                    Thread.Sleep(100);
                    if (lootTimer.ElapsedMilliseconds > 5000)
                    {
                        Log(System.Drawing.Color.Red,"looks like you have some addon interfering with loot. Disable any looting addons pls");
                        return;
                    }
                }
			}
		}
		static public void Log(string msg, params object[] args) { Logging.Write(msg, args); }

		static public void Log(System.Drawing.Color c, string msg, params object[] args) { Logging.Write(c, msg, args); }
		
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
	}
}

