using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.LootFrame;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using System.Drawing;

namespace Nuok
{
    /// <summary>
    /// Custom quest behavior for basic fishing for DMF profession quest. Parts inspired
    /// from AutoAngler.
    /// By timglide
    /// Modified by Nuok :o
    /// Credit to Nesox for dismount code
    /// 
    /// You must set either a facing direction or a PoolId
    /// ItemId (Optional): Will use the item if it in your inventory, useful for one of the quests were you fish an item and have to open it
    /// QuestId (Optional): Will stop if you have completed said quest
    /// Facing Direction: This must be postive and can be found by calling Me.RotationDegrees, if its negative then the bot has bugged; 0-360 is fine
    /// PoolFish: If you want to pool fish enable
    /// PoolId: The Id of the pool you want to fish
    /// FlyToFish (Optional): Set to true to and the behavior will fly to the pool
    /// Location: Location were you want it to bot
    /// 
    /// Example:
    /// Normal Fishing
    /// <CustomBehavior File="Fishing" QuestId="26420" ItemId="100" FacingDegree="260" X="-8190.623" Y="732.9164" Z="68.12978" />
    /// Dont Fly to the fish
    /// <CustomBehavior File="Fishing" QuestId="26420" FlyToFish="False" FacingDegree="260" X="-8190.623" Y="732.9164" Z="68.12978" />
    /// Pool Fish
    /// <CustomBehavior File="Fishing" QuestId="26420" PoolFish="true" PoolId="100" X="-8190.623" Y="732.9164" Z="68.12978" />
    /// </summary>
    class Fishing : CustomForcedBehavior
    {
        #region Base
        private const float FacingLeewayDegrees = 4f;
        private const float MaxPoolDistance = 18f;
        private static readonly int[] FishingSpellIds = { 7620, 7731, 7732, 18248, 33095, 51294, 88868 };

        public Fishing(Dictionary<string, string> args)
            : base(args)
        {

            try
            {
                // no attributes
                ItemId = GetAttributeAsNullable<int>("ItemId", false, ConstrainAs.ItemId, null) ?? 0;
                FacingDirection = GetAttributeAsNullable<int>("FacingDegree", false, null, null) ?? 0;
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? Me.Location;
                PoolFish = GetAttributeAsNullable<bool>("PoolFish", false, null, null) ?? false;
                PoolId = GetAttributeAsNullable<int>("PoolId", false, ConstrainAs.ObjectId, null) ?? 0;
                FlyToFish = GetAttributeAsNullable<bool>("FlyToPool", false, null, null) ?? true;
                QuestId = GetAttributeAsNullable("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
            }
            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller

        // Private variables for internal state
        private ConfigMemento _configMemento;
        private bool _isDisposed;
        private Composite _root;

        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint Location { get; private set; }
        public int FacingDirection { get; private set; }
        public int ItemId { get; private set; }
        public bool PoolFish { get; private set; }
        public int PoolId { get; private set; }
        public bool FlyToFish { get; private set; }
        private static int fishingSpellId;
        private WoWItem mainHand;
        private WoWItem offHand;


        ~Fishing()
        {
            Dispose(false);
        }

        public static void Log(string format, params object[] args)
        {
            Logging.Write(Color.LightBlue, string.Format("Fishing: ") + format, args);
        }

        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                // NOTE: we should call any Dispose() method for any managed or unmanaged
                // resource, if that resource provides a Dispose() method.

                // Clean up managed resources, if explicit disposal...
                if (isExplicitlyInitiatedDispose)
                {
                    // empty, for now
                }

                // Clean up unmanaged resources (if any) here...
                if (_configMemento != null)
                {
                    _configMemento.Dispose();
                    _configMemento = null;
                }

                EquipOriginalWeapons();

                BotEvents.OnBotStop -= BotEvents_OnBotStop;
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }

        public void BotEvents_OnBotStop(EventArgs args)
        {
            Dispose();
        }

        public static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }
        #endregion

        /// <summary>
        /// Just ensures the angle isnt messed up
        /// </summary>
        public static float AngleNormalized
        {
            get
            {
                if (Me.RotationDegrees > 0)
                    return Me.RotationDegrees;

                else return 360 - Math.Abs(Me.RotationDegrees);
            }

        }

        #region Fishing

        public WoWItem Item
        {
            get
            {
                return StyxWoW.Me.CarriedItems.FirstOrDefault(ret => ret.Entry == ItemId);
            }
        }

        public int GetFishingSpellId()
        {
            foreach (int i in FishingSpellIds)
            {
                if (SpellManager.HasSpell(i)) return i;
            }

            return 0;
        }

        public WoWGameObject FishingPool
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>().Where(o => o.IsValid && o.SubType == WoWGameObjectType.FishingHole && o.Entry == PoolId && o.Distance2D < MaxPoolDistance).
                    OrderBy(o => o.Distance).FirstOrDefault();
            }
        }

        public static WoWGameObject Bobber
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWGameObject>()
                    .FirstOrDefault(
                        o => o.IsValid && o.SubType == WoWGameObjectType.FishingBobber &&
                        o.CreatedByGuid == Me.Guid);
            }
        }

        public static bool IsBobbing
        {
            get
            {
                return null != Bobber ? ((WoWFishingBobber)Bobber.SubObj).IsBobbing : false;
            }
        }

        public static bool IsPoleEquipped
        {
            get
            {
                WoWItem mainHand = Me.Inventory.Equipped.MainHand;
                return null != mainHand && WoWItemWeaponClass.FishingPole == mainHand.ItemInfo.WeaponClass;
            }
        }

        public static bool EquipPole()
        {
            var pole = Me.BagItems.Where(i => i.ItemInfo.WeaponClass == WoWItemWeaponClass.FishingPole).OrderBy(i => i.ItemInfo.Level).FirstOrDefault();

            if (null != pole)
            {
                Log("Equipping {0}.", pole.Name);
                Lua.DoString("EquipItemByName({0})", pole.Entry);
                return true;
            }

            return false;
        }


        #region Composites
        public static Composite CreateUseBobber()
        {
            return
                new Decorator(ret => IsBobbing,
                    new Action(ret =>
                    {
                        Log("Fishing Bobber Bobed");
                        ((WoWFishingBobber)Bobber.SubObj).Use();
                        return RunStatus.Success;
                    }));
        }

        public static Composite EquipFishingGear()
        {
            return
                    new Decorator(ret => !IsPoleEquipped,
                        new Action(ret =>
                            {
                                EquipPole();
                                if (!EquipPole())
                                {
                                    _isDone = true;
                                    return RunStatus.Success;
                                }
                                return RunStatus.Failure;
                            }));
        }

        private static WaitTimer CastDelay = new WaitTimer(TimeSpan.FromSeconds(2));

        public static Composite CastLine()
        {
            return 
            new PrioritySelector(
                new Decorator(ret => CastDelay.IsFinished,
                    new Sequence(
                        new Action(ret =>
                            {
                                Log("Casting Fishing Line");
                                TreeRoot.StatusText = "Casting line.";
                                SpellManager.Cast(fishingSpellId);
                                CastDelay.Reset();
                            }),
                            new Action(ret => RunStatus.Success))),
                new Action(ret => RunStatus.Success));
        }
        #endregion
        public void EquipOriginalWeapons()
        {
            SpellManager.StopCasting();
            StyxWoW.SleepForLagDuration();
            using (new FrameLock())
            {
                if (null != mainHand && mainHand != Me.Inventory.Equipped.MainHand)
                {
                    Log("Equipping original main hand: {0}.", mainHand.Name);
                    Lua.DoString("EquipItemByName({0})", mainHand.Entry);
                }

                if (null != offHand && offHand != Me.Inventory.Equipped.OffHand)
                {
                    Log("Equipping original off hand: {0}.", offHand.Name);
                    Lua.DoString("EquipItemByName({0})", offHand.Entry);
                }

                if (null != mainHand || null != offHand)
                {
                    Thread.Sleep(500);
                }
            }
        }

        #region Lures

        private static HashSet<uint> LureIds = new HashSet<uint>
        {
            68049, 62673, 34861, 46006, 6533, 7307, 6532, 6530, 6811, 6529, 67404,
        };

        public static bool HasLureApplied
        {
            get
            {
                var FishingPool = StyxWoW.Me.Inventory.Equipped.GetEquippedItem(WoWInventorySlot.MainHand);
                return FishingPool.TemporaryEnchantment.IsValid;
            }
        }

        public static WoWItem FishingLure
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWItem>().
                    Where(i => i.ItemInfo.RequiredSkillLevel <= StyxWoW.Me.GetSkill(SkillLine.Fishing).CurrentValue && LureIds.Contains(i.Entry)).OrderBy(i => i.ItemInfo.RequiredSkillLevel).FirstOrDefault();
            }
        }

        private static WaitTimer LureTimer = new WaitTimer(TimeSpan.FromSeconds(10));

        public static Composite ApplyLure()
        {
            return
                new PrioritySelector(
                new Decorator(ctx => IsPoleEquipped && !HasLureApplied && FishingLure != null && LureTimer.IsFinished,
                    new Sequence(
                        new Action(ctx => Log("Applying Lure {0}", FishingLure.Name)),
                        new Action(ctx => TreeRoot.StatusText="Applying Lure "+ FishingLure.Name),
                        new Action(ctx => FishingLure.Use()), 
                        new Action(ret => LureTimer.Reset()),
                        new Action(ret => RunStatus.Success))));

        }
        #endregion
        #endregion

        #region Overrides of CustomForcedBehavior

        protected override TreeSharp.Composite CreateBehavior()
        {
            return _root ?? (_root = new PrioritySelector(
                new Decorator(ret => IsDone, new Action(c =>
                {
                    TreeRoot.StatusText = "Fishing complete!";
                    return RunStatus.Success;
                })),

                new Decorator(ret => null != LootFrame.Instance && LootFrame.Instance.IsVisible,
                    new Action(c =>
                {
                    LootFrame.Instance.LootAll();
                    StyxWoW.SleepForLagDuration();
                    return RunStatus.Success;
                })),

                //This is for quests were your required to use the fish to complete the quest

                new Decorator(ret => Item != null,
                    new Action(ret =>
                        {
                            Log("Using {0}", Item.Name);
                            Item.Use();
                            return RunStatus.Success;
                        })),

                //Make sure were at location
                new Decorator(ret => Location.Distance(Me.Location) <= 2,
                    new PrioritySelector(
                        EquipFishingGear(),

                    //If were pool fishing and the pool isnt there then were done
                    new Decorator(ret => PoolFish && FishingPool == null,
                        new Action(ret =>
                            {
                                LogMessage("debug", "No pool was found with Id: {0} at {1}", PoolId, Location);
                                _isDone = true;
                                return RunStatus.Success;
                            })),

                    //Taken from ForcedDismount, credit to nesox
                    new Decorator(ret => Flightor.MountHelper.Mounted,
                        new Action(ret => Flightor.MountHelper.Dismount())),

                    //Make sure were not moving before trying to cast
                    new Decorator(ret => Me.IsMoving,
                        new Action(ret => WoWMovement.MoveStop())),

                    //Need to face water
                    new Decorator(ret => !PoolFish && Math.Abs((FacingDirection - AngleNormalized)) > FacingLeewayDegrees,
                        new Action(ret => Me.SetFacing(WoWMathHelper.DegreesToRadians(FacingDirection)))),

                    //Face the pool
                    new Decorator(ret => PoolFish && !Me.IsFacing(FishingPool.Location),
                        new Sequence(
                            new Action(ret => Logging.WriteDebug("Facing {0}", FishingPool.Name)),
                            new Action(ret => Me.SetFacing(FishingPool.Location)))),
        
                    ApplyLure(),
                    new Decorator(ret => Me.IsCasting,
                        new PrioritySelector(
                            CreateUseBobber(),
                            new Decorator(ret => null == Bobber,
                                    CastLine()),
                            //Rescast the line if its too far from the pool
                            new Decorator(ret => PoolFish && (Bobber.Location.Distance2D(FishingPool.Location) > 3.6f),
                                CastLine()),

                            new Action(ret => RunStatus.Success))),
                    CastLine())),

                    //Lets fly to the pool if we can
                    new Decorator(ret => Location.Distance(Me.Location) > 20 && FlyToFish,
                                new Action(ret => Flightor.MoveTo(Location, 5f))),

                    new Decorator(ret => Location.Distance(Me.Location) > 2,
                        new Sequence(
                            new Action(ret => { TreeRoot.StatusText = "Moving towards fishing spot"; }),
                            new Action(ret => Navigator.MoveTo(Location))))
                    ));
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private static bool _isDone = false;

        public override bool IsDone
        {
            get
            {
                return (_isDone ||
                    !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
            }
        }

        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                BotEvents.OnBotStop += BotEvents_OnBotStop;
                TreeRoot.GoalText = "Fishing";
                mainHand = Me.Inventory.Equipped.MainHand;
                offHand = Me.Inventory.Equipped.OffHand;
                fishingSpellId = GetFishingSpellId();

                CastDelay.Reset();
                LureTimer.Reset();

                if (0 == fishingSpellId)
                {
                    LogMessage("error", "You don't have the fishing skill, skipping.");
                    _isDone = true;
                }
            }
        }

        #endregion




    }
}
