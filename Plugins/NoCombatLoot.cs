namespace Skinfix
{
    using System.Drawing;
    using System.Linq;
    using Levelbot.Actions.Combat;
    using Styx.Combat.CombatRoutine;
    using Styx.Logic.Combat;
    using Styx.Logic.Inventory.Frames.LootFrame;
    using Styx.Logic;
    using System;
    using Styx.Helpers;
    using Styx.Logic.Pathing;
    using System.Threading;
    using System.Diagnostics;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml.Linq;
    using System.Net;
    using Styx.Plugins.PluginClass;
    using Styx;

    public class Skinfix : HBPlugin
    {
        //Normal Stuff.
        public override string Name { get { return "NoCombatLoot"; } }
        public override string Author { get { return "Inrego"; } }
        public override Version Version { get { return new Version(1, 0); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "None"; } }

        //Logging Class for your conviance
        public static void slog(string format, params object[] args)
        {
            Logging.Write(Color.Crimson, "[CLoot]: " + format, args);
        }
        private static readonly LocalPlayer Me = ObjectManager.Me;
        private double LootRange
        {
            get { return LootTargeting.LootRadius; }
        }
        private IOrderedEnumerable<WoWUnit> Lootables
        {
            get
            {
                var targetsList = ObjectManager.GetObjectsOfType<WoWUnit>(false, false).Where(
                        p => (p.Lootable || p.CanSkin) && p.Distance <= LootRange).OrderBy(l => l.Distance);
                return targetsList;
            }
        }
        public override void Pulse()
        {
            if (Me.IsCasting || Lootables.Count() == 0 || Me.IsMoving || Me.Combat == true)
                return;
            if (LootFrame.Instance != null && LootFrame.Instance.IsVisible)
            {
                LootFrame.Instance.LootAll();
                return;
            }
            var lootTarget = Lootables.FirstOrDefault();
            if (lootTarget.Distance > lootTarget.InteractRange)
            {
                slog("Moving to loot {0}.", lootTarget.Name);
                Navigator.MoveTo(lootTarget.Location);
            }
            else
            {
                lootTarget.Interact();
                return;
            }
        }
    }
}

