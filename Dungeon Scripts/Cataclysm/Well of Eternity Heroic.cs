using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Helpers;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


#if USE_DUNGEONBUDDY_DLL
using Bots.DungeonBuddyDll;
using Bots.DungeonBuddyDll.Attributes;
using Bots.DungeonBuddyDll.Helpers;
namespace Bots.DungeonBuddyDll.Dungeon_Scripts.Cataclysm
#else
    using Bots.DungeonBuddy.Attributes;
    using Bots.DungeonBuddy.Helpers;
namespace Bots.DungeonBuddy.Dungeon_Scripts.Cataclysm
#endif
{
    public class WellOfEternityHeroic : Dungeon
    {
        #region Overrides of Dungeon
        public override uint DungeonId
        {
            get { return 437; }
        }
        public override Styx.Logic.Pathing.WoWPoint ExitLocation
        {
            get
            {
                return new WoWPoint(3249.494f, -5007.59f, 194.0935f);
            }
        }

        public override void RemoveTargetsFilter(List<WoWObject> units)
        {
            units.RemoveAll(
                           o =>
                           {
                               var unit = o as WoWUnit;
                               return false;
                           });
        }

        public override void IncludeTargetsFilter(List<WoWObject> incomingunits, HashSet<WoWObject> outgoingunits)
        {
            foreach (WoWObject obj in incomingunits)
            {
                var unit = obj.ToUnit();
                if (unit == null)
                    continue;
            }
        }

        public override void WeighTargetsFilter(List<Styx.Logic.Targeting.TargetPriority> units)
        {
            foreach (var priorObj in units)
            {
                var unit = priorObj.Object.ToUnit();
                if (unit == null)
                    continue;
            }
        }

        private bool _useMountSetting;
        public override void OnEnter() { _useMountSetting = CharacterSettings.Instance.UseMount; }

        public override void OnExit() { CharacterSettings.Instance.UseMount = _useMountSetting; }

        #endregion

        LocalPlayer Me { get { return StyxWoW.Me; } }

        [EncounterHandler(0,"Root Behavior")]
        public Composite RootBehavior()
        {
            return new PrioritySelector(
                new Decorator(ctx => Me.HasAura("Shadow Walk") && CharacterSettings.Instance.UseMount,
                    new Action(ctx => CharacterSettings.Instance.UseMount = false))
                );
        }
    }
}
