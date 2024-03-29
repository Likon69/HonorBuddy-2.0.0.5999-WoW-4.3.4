﻿using System.Collections.Generic;
using CommonBehaviors.Actions;
using Styx;
using Styx.Logic;
using Styx.Logic.Pathing;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

#if USE_DUNGEONBUDDY_DLL
    using Bots.DungeonBuddyDll;
    using Bots.DungeonBuddyDll.Attributes;
    using Bots.DungeonBuddyDll.Helpers;
namespace Bots.DungeonBuddyDll.Dungeon_Scripts.Burning_Crusade
#else
    using Bots.DungeonBuddy.Attributes;
    using Bots.DungeonBuddy.Helpers;
    namespace Bots.DungeonBuddy.Dungeon_Scripts.Burning_Crusade
#endif

{
    public class HellfireRamparts : Dungeon
    {
        #region Overrides of Dungeon

        /// <summary>
        ///   The mapid of this dungeon.
        /// </summary>
        /// <value> The map identifier. </value>
        public override uint DungeonId { get { return 136; } }

        public override void OnEnter() { _lootedReinforcedFelIronChest = false; }

        public override WoWPoint Entrance { get { return new WoWPoint(-365.0169, 3093.254, -14.35639); } }
        public override WoWPoint ExitLocation { get { return new WoWPoint(-1357.323, 1635.028, 68.53062 ); } }

        public override void WeighTargetsFilter(List<Targeting.TargetPriority> units)
        {
            foreach (var targetPriority in units)
            {
                switch (targetPriority.Object.Entry)
                {
                    case 17537:
                        targetPriority.Score += 120;
                    break;
                    case 17540: // Fiendish Hound
                    case 17309: // Hellfire Watcher
                        if (StyxWoW.Me.IsDps())
                            targetPriority.Score += 120;
                        break;
                }
            }
        }

        #endregion


        [EncounterHandler(17536, "Nazan")]
        public Composite NazanEncounter()
        {
            return new PrioritySelector(
                ScriptHelpers.CreateTankFaceAwayGroupUnit(15));
        }

        [EncounterHandler(17536, "Omor the Unscarred")]
        public Composite OmorTheUnscarredEncounter()
        {
            return new PrioritySelector(
                ScriptHelpers.CreateSpreadOutLogic(ctx => StyxWoW.Me.HasAura("Bane of Treachery" ), 20));
        }

        private bool _lootedReinforcedFelIronChest;

        [ObjectHandler(185168, "Reinforced Fel Iron Chest")]
        public Composite ReinforcedFelIronChestHandler()
        {
            return new Decorator(
                ctx => !_lootedReinforcedFelIronChest,
                new Sequence(
                    ScriptHelpers.CreateMoveToContinue(185168),
                    new WaitContinue(3, ctx => !StyxWoW.Me.IsMoving, new ActionAlwaysSucceed()),
                    new Action(ctx => ((WoWGameObject)ctx).Interact()),
                    new WaitContinue(10, ctx => false, new ActionAlwaysSucceed()),
                    new Action(ctx => _lootedReinforcedFelIronChest = true)));
        }

        [ObjectHandler(181890, "Liquid Fire", ObjectRange = 15)]
        public Composite LiquidFireHandler()
        {
            return new PrioritySelector(
                ScriptHelpers.CreateRunAwayFromBad(ctx => true, 5f, 181890));
        }
    }
}