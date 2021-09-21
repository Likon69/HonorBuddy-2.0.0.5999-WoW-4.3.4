// Behavior originally contributed by Chinajade.
//
// LICENSE:
// This work is licensed under the
//     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// also known as CC-BY-NC-SA.  To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/3.0/
// or send a letter to
//      Creative Commons // 171 Second Street, Suite 300 // San Francisco, California, 94105, USA.
//
// DOCUMENTATION:
//      http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_CollectThings
//     
// QUICK DOX:
//      Collects items from mobs or objects when (right-click) 'interaction' is required.
//      Most useful for those type of quests where you blow something up,
//      then you have to collect the pieces.
//
//  Parameters (required, then optional--both listed alphabetically):
//      (***One or more of the following two attributes must be specified***)
//      MobIdN [REQUIRED if ObjectId is omitted]: Defines the mobs that drop the Items we're after.
//              N may be omitted, or any numeric value--multiple mobs are supported.
//      ObjectIdN [REQUIRED if MobId is omitted]: Defines the objects that drop the Items we're after.
//              N may be omitted, or any numeric value--mulitple objects are supported.
//
//      (This attribute is optional, but governs what other attributes are optional)
//      CollectUntil [Default: RequiredCountReached]: Defines the terminating condition for
//              this behavior.  Available options include:  NoTargetsInArea, RequiredCountReached, QuestComplete.
//              "Targets" means mobs or objects--whatever is dropping the items we're after.
//
//      (***These attributes may/may not be optional based on value of CollectUntil attribute***)
//      CollectItemCount [REQUIRED if CollectUntil=RequiredCountReached; Default: 1]:
//              represents the number of items we must collect for the behavior to terminate.
//      CollectItemId [REQUIRED if CollectUntil=NoTargetsInArea or RequiredCountReached; Default:none]:
//              Identifies the item we are collecting.  The only time this attribute may be omitted
//              is when we're collecting intangibles such as 'attitudes' or 'liberations' that
//              will complete the quest.
//      QuestId [REQUIRED if CollectUntil=QuestComplete; Default:none]:
//
//      (***These attibutes are completely optional***)
//      HuntingGroundRadius [Default: 120]: The range from the anchor location (i.e., X/Y/Z) location at which
//              targets (mobs or objects) will be sought.
//      IgnoreMobsInBlackspots [Default: false]: If true, mobs sitting in blackspotted areas will not be
//              considered as targets.
//      MobState [Default: DontCare]: Identifies the state in which the Mob must be to be considered
//              as a target.  The MobState only applies if the target is some form of NPC.  The MobState
//              Valid values are Alive/Dead/DontCare.
//      NonCompeteDistance [Default: 25]: If a player is within this distance of a target that looks
//              interesting to us, we'll ignore the target.  The assumption is that the player may
//              be going for the same target, and we don't want to draw attention.
//      PostInteractDelay [Default: 1500ms]: The number of milliseconds to wait after each interaction.
//              This is useful if the target requires time for the interaction to complete.
//              This value must be on the closed interval [0..61000].
//      QuestCompleteRequirement [Default:NotComplete]:
//      QuestInLogRequirement [Default:InLog]:
//              A full discussion of how the Quest* attributes operate is described in
//              http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
//      X/Y/Z [Default: Toon's initial position]: Defines the anchor of a search area for
//              which targets (mobs or objects) will be sought.  The hunting ground is defined by
//              this value coupled with the CollectionDistance.
// 
// Exmaples:
// <CustomBehavior File="CollectThings" ...other args... >
//     <Hotspot Name="Cathedral Square fishing dock" X="4554.003" Y="-4718.743" Z="883.0464" StartPoint="true" />
//     <Hotspot Name="The Shady Lady" X="4578.725" Y="-4721.257" Z="882.8724" />
//     <Hotspot Name="The Blue Recluse" X="4584.166" Y="-4693.487" Z="882.7331" StartPoint="true" />
// </CustomBehavior>
// 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using CommonBehaviors.Actions;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Inventory.Frames.LootFrame;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.Logic.Questing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace BuddyWiki.CustomBehavior.CollectThings
{
    public class CollectThings : CustomForcedBehavior
    {
        public enum CollectUntilType
        {
            NoTargetsInArea,
            RequiredCountReached,
            QuestComplete,
        }

        public enum MobStateType
        {
            Alive,
            Dead,
            DontCare,
        }

        public CollectThings(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                bool isCollectItemCountRequired = false;
                bool isCollectItemIdRequired = false;
                bool isQuestIdRequired = false;

                CollectUntil = GetAttributeAsNullable<CollectUntilType>("CollectUntil", false, null, null) ?? CollectUntilType.RequiredCountReached;
                if ((CollectUntil == CollectUntilType.NoTargetsInArea)
                    || (CollectUntil == CollectUntilType.RequiredCountReached))
                {
                    isCollectItemCountRequired = true;
                    isCollectItemIdRequired = true;
                }
                else if (CollectUntil == CollectUntilType.QuestComplete)
                { isQuestIdRequired = true; }

                CollectItemCount = GetAttributeAsNullable<int>("CollectItemCount", isCollectItemCountRequired, ConstrainAs.CollectionCount, null) ?? 1;
                CollectItemId = GetAttributeAsNullable<int>("CollectItemId", isCollectItemIdRequired, ConstrainAs.ItemId, null) ?? 0;
                HuntingGroundAnchor = GetAttributeAsNullable<WoWPoint>("", false, ConstrainAs.WoWPointNonEmpty, null) ?? Me.Location;
                HuntingGroundRadius = GetAttributeAsNullable<double>("HuntingGroundRadius", false, new ConstrainTo.Domain<double>(1.0, 200.0), new[] { "CollectionDistance" }) ?? 120.0;
                IgnoreMobsInBlackspots = GetAttributeAsNullable<bool>("IgnoreMobsInBlackspots", false, null, null) ?? false;
                MobIds = GetNumberedAttributesAsArray<int>("MobId", 0, ConstrainAs.MobId, null);
                MobState = GetAttributeAsNullable<MobStateType>("MobState", false, null, null) ?? MobStateType.DontCare;
                NonCompeteDistance = GetAttributeAsNullable<double>("NonCompeteDistance", false, new ConstrainTo.Domain<double>(1.0, 150.0), null) ?? 25.0;
                ObjectIds = GetNumberedAttributesAsArray<int>("ObjectId", 0, ConstrainAs.ObjectId, null);
                PostInteractDelay = TimeSpan.FromMilliseconds(GetAttributeAsNullable<int>("PostInteractDelay", false, new ConstrainTo.Domain<int>(0, 61000), null) ?? 1500);
                RandomizeStartingHotspot = GetAttributeAsNullable<bool>("RandomizeStartingHotspot", false, null, null) ?? false;
                QuestId = GetAttributeAsNullable<int>("QuestId", isQuestIdRequired, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;


                // Semantic coherency --
                if ((MobIds.Count() <= 0) && (ObjectIds.Count() <= 0))
                {
                    LogMessage("error", "You must specify one or more MobId(s) or ObjectId(s)");
                    IsAttributeProblem = true;
                }

                if (HuntingGroundRadius < (NonCompeteDistance * 2))
                {
                    LogMessage("error", "The CollectionDistance (saw '{0}') must be at least twice the size"
                                        + " of the NonCompeteDistance (saw '{1}').",
                                        HuntingGroundRadius,
                                        NonCompeteDistance);
                    IsAttributeProblem = true;
                }


                // Find the item name --
                ItemInfo itemInfo = ItemInfo.FromId((uint)CollectItemId);

                ItemName = (itemInfo != null) ? itemInfo.Name : string.Format("Item({0})", CollectItemId);


                // Sub-behaviors...
                _behavior_SwimBreath = new SwimBreathBehavior((messageType, format, argObjects) => LogMessage(messageType, format, argObjects));
                _behavior_HuntingGround = new HuntingGroundBehavior((messageType, format, argObjects) => LogMessage(messageType, format, argObjects),
                                                                    IsViableTarget,
                                                                    HuntingGroundAnchor,
                                                                    HuntingGroundRadius);
                _behavior_UnderwaterLooting = new UnderwaterLootingBehavior((messageType, format, argObjects) => LogMessage(messageType, format, argObjects));
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it can be quickly
                // resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller
        public int CollectItemCount { get; private set; }
        public int CollectItemId { get; private set; }
        public CollectUntilType CollectUntil { get; private set; }
        public WoWPoint HuntingGroundAnchor { get; private set; }
        public double HuntingGroundRadius { get; private set; }
        public bool IgnoreMobsInBlackspots { get; private set; }
        public int[] MobIds { get; private set; }
        public MobStateType MobState { get; private set; }
        public double NonCompeteDistance { get; private set; }
        public int[] ObjectIds { get; private set; }
        public TimeSpan PostInteractDelay { get; private set; }
        public bool RandomizeStartingHotspot { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }

        // Private properties and data...
        private HuntingGroundBehavior _behavior_HuntingGround;
        private SwimBreathBehavior _behavior_SwimBreath;
        private UnderwaterLootingBehavior _behavior_UnderwaterLooting;
        private bool _isBehaviorDone = false;
        private bool _isDisposed;
        private PluginContainer _pluginAntiDrown;
        private bool _pluginAntiDrownWasEnabled;

        private WoWObject CurrentTarget { get { return (_behavior_HuntingGround.CurrentTarget); } }
        private readonly TimeSpan Delay_MobConsumedExpiry = TimeSpan.FromMinutes(7);
        private readonly TimeSpan Delay_BlacklistPlayerTooClose = TimeSpan.FromSeconds(90);
        private TimeSpan Delay_WowClientLagTime { get { return (TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150)); } }
        private readonly TimeSpan Delay_WoWClientMovementThrottle = TimeSpan.FromMilliseconds(500);
        private string ItemName { get; set; }
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }

        // Private LINQ queries..
        private int CollectedItemCount
        {
            get
            {
                return ((int)Me.BagItems
                            .Where(item => (item.ItemInfo.Id == CollectItemId))
                            .Sum(item => item.StackCount));
            }
        }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: CollectThings.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~CollectThings()
        {
            Dispose(false);
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
                if (_pluginAntiDrown != null)
                {
                    _pluginAntiDrown.Enabled = _pluginAntiDrownWasEnabled;
                    _pluginAntiDrown = null;
                }

                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        // If player is close to a target that is interesting to us, ignore the target...
        // The player may be going for the same mob, and we don't want to draw attention.
        // We'll blacklist the mob for a bit, in case the player is running around, or following
        // us.  The excaption is ithe player is in our party, then we can freely kill any target
        // close to him.
        private bool BlacklistIfPlayerNearby(WoWObject target)
        {
            WoWUnit nearestCompetingPlayer = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                                                    .OrderBy(player => player.Location.Distance(target.Location))
                                                    .FirstOrDefault(player => player.IsPlayer
                                                                                && player.IsAlive
                                                                                && !player.IsInOurParty());

            // If player is too close to the target, ignore target for a bit...
            if ((nearestCompetingPlayer != null)
                && (nearestCompetingPlayer.Location.Distance(target.Location) <= NonCompeteDistance))
            {
                target.LocallyBlacklist(Delay_BlacklistPlayerTooClose);
                return (true);
            }

            return (false);
        }


        private void GuiShowProgress(string completionReason)
        {
            TreeRoot.GoalText = string.Format("{0}: {1}/{2} {3}", this.GetType().Name, CollectedItemCount, CollectItemCount, ItemName);

            if (completionReason != null)
            {
                LogMessage("debug", "Behavior done (" + completionReason + ")");
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;
            }
        }


        public bool IsViableTarget(WoWObject target)
        {
            bool isViable;

            if (target == null)
            { return (false); }

            isViable = (target.IsValid
                        && (MobIds.Contains((int)target.Entry) || ObjectIds.Contains((int)target.Entry))
                        && !target.IsLocallyBlacklisted()
                        && !BlacklistIfPlayerNearby(target)
                        && (IgnoreMobsInBlackspots
                            ? Targeting.IsTooNearBlackspot(ProfileManager.CurrentProfile.Blackspots,
                                                            target.Location)
                            : true));

            if (isViable && (target is WoWUnit))
            {
                WoWUnit wowUnit = target.ToUnit();

                isViable = ((wowUnit.IsAlive && (MobState == MobStateType.Alive))
                            || (wowUnit.Dead && (MobState == MobStateType.Dead))
                            || (MobState == MobStateType.DontCare));
            }

            return (isViable);
        }


        private void ParseHuntingGroundHotspots(bool randomizeStartingHotspot)
        {
            List<WoWPointNamed> huntingGroundHotspots = new List<WoWPointNamed>();

            foreach (XElement element in Element.Elements().Where(elem => (elem.Name == "Hotspot")))
            {
                double? x = ParseXmlElementDouble(element, "X", true);
                double? y = ParseXmlElementDouble(element, "Y", true);
                double? z = ParseXmlElementDouble(element, "Z", true);

                if (!x.HasValue || !y.HasValue || !z.HasValue)
                { continue; }

                bool isStarting = ParseXmlElementBool(element, "StartPoint", false) ?? false;
                string name = ParseXmlElementString(element, "Name", false);

                huntingGroundHotspots.Add(new WoWPointNamed(new WoWPoint(x.Value, y.Value, z.Value), name, isStarting));
            }

            _behavior_HuntingGround.UseHotspots(huntingGroundHotspots, randomizeStartingHotspot);
        }


        private bool? ParseXmlElementBool(XElement element,
                                            string attributeName,
                                            bool isRequired)
        {
            string location = (((IXmlLineInfo)element).HasLineInfo()
                                        ? (" @line " + ((IXmlLineInfo)element).LineNumber.ToString())
                                        : string.Empty);
            bool tmpBool;

            if (element.Attribute(attributeName) == null)
            {
                if (isRequired)
                {
                    LogMessage("error", "Hotspot{0} is missing the '{1}' attribute (required)", location, attributeName);
                    IsAttributeProblem = true;
                }
                return (null);
            }

            if (!bool.TryParse(element.Attribute(attributeName).Value, out tmpBool))
            {
                LogMessage("error", "Hotspot{0} '{1}' attribute is malformed", location, attributeName);
                IsAttributeProblem = true;
                return (null);
            }

            return (tmpBool);
        }


        private double? ParseXmlElementDouble(XElement element,
                                              string attributeName,
                                              bool isRequired)
        {
            string location = (((IXmlLineInfo)element).HasLineInfo()
                                        ? (" @line " + ((IXmlLineInfo)element).LineNumber.ToString())
                                        : string.Empty);
            double tmpDouble;

            if (element.Attribute(attributeName) == null)
            {
                if (isRequired)
                {
                    LogMessage("error", "Hotspot{0} is missing the '{1}' attribute (required)", location, attributeName);
                    IsAttributeProblem = true;
                }
                return (null);
            }

            if (!double.TryParse(element.Attribute(attributeName).Value, out tmpDouble))
            {
                LogMessage("error", "Hotspot{0} '{1}' attribute is malformed", location, attributeName);
                IsAttributeProblem = true;
                return (null);
            }

            return (tmpDouble);
        }


        private string ParseXmlElementString(XElement element,
                                              string attributeName,
                                              bool isRequired)
        {
            string location = (((IXmlLineInfo)element).HasLineInfo()
                                        ? (" @line " + ((IXmlLineInfo)element).LineNumber.ToString())
                                        : string.Empty);

            if (element.Attribute(attributeName) == null)
            {
                if (isRequired)
                {
                    LogMessage("error", "Hotspot{0} is missing the '{1}' attribute (required)", location, attributeName);
                    IsAttributeProblem = true;
                }
                return (null);
            }

            return (element.Attribute(attributeName).Value);
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return (
                new PrioritySelector(
                // If behavior done, bail...
                // Note that this is also an implicit "is quest complete" exit criteria, also.
                    new Decorator(ret => IsDone,
                        new Action(delegate { GuiShowProgress("quest complete"); })),

                    // If we've filled our inventory quota, we're done...
                    new Decorator(
                        ret => (CollectedItemCount >= CollectItemCount),
                        new Action(delegate
                            {
                                GuiShowProgress(string.Format("{0}/{1} items collected", CollectedItemCount, CollectItemCount));
                                _isBehaviorDone = true;
                            })),

                    // If we're dead, the behavior can't function so bail until alive...
                    new Decorator(ret => Me.Dead, new ActionAlwaysSucceed()),

                    // If swimming, check if we need breath...
                    _behavior_SwimBreath.CreateBehavior(),

                    // If there is loot to clean up...
                    _behavior_UnderwaterLooting.CreateBehavior(() => true),

                    // Find next target...
                    _behavior_HuntingGround.CreateBehavior_SelectTarget(() => (CollectUntil == CollectUntilType.NoTargetsInArea)),

                    // If no target and that's our exit criteria, we're done...
                    new Decorator(ret => ((CurrentTarget == null)
                                          && (CollectUntil == CollectUntilType.NoTargetsInArea)),
                        new Action(delegate
                        {
                            GuiShowProgress("No more objects/mobs in area");
                            _isBehaviorDone = true;
                        })),

                    // Otherwise, keep the unit of interest targeted...
                    new Decorator(ret => ((Me.CurrentTarget != CurrentTarget) && (CurrentTarget is WoWUnit)),
                        new Action(delegate
                        {
                            CurrentTarget.ToUnit().Target();
                            return (RunStatus.Failure);     // Fall through
                        })),

                    // Keep progress updated...
                    new Action(delegate
                    {
                        GuiShowProgress(null);
                        return (RunStatus.Failure);     // Fall through
                    }),

                    // If we're not at target, move to it...
                    _behavior_HuntingGround.CreateBehavior_MoveToTarget(),

                    // We're within interact range, collect the object...
                    new Sequence(
                        new Action(delegate { WoWMovement.MoveStop(); }),
                        new Action(delegate { _behavior_HuntingGround.MobEngaged(CurrentTarget); }),
                        new WaitContinue(Delay_WowClientLagTime, ret => false, new ActionAlwaysSucceed()),
                        new Action(delegate { CurrentTarget.Interact(); }),
                        new WaitContinue(Delay_WowClientLagTime, ret => false, new ActionAlwaysSucceed()),
                        new WaitContinue(PostInteractDelay, ret => false, new ActionAlwaysSucceed()),
                        new Action(delegate { CurrentTarget.LocallyBlacklist(Delay_MobConsumedExpiry); }),
                        new Action(delegate { Me.ClearTarget(); })
                        )
                )
            );
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get
            {
                return (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
            }
        }


        public override void OnStart()
        {
            // The XML element didn't exist when the constructor was called...
            // So we had to defer some final parsing that really should've happened in the constructor
            // to the OnStart() method.  This will parse the final arguments, and set IsAttributeProblem
            // correctly, for normal processing.
            ParseHuntingGroundHotspots(RandomizeStartingHotspot);

            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                // Disable the AntiDrown plugin if present, as it interferes with our anti-drown prevention...
                _pluginAntiDrown = PluginManager.Plugins.FirstOrDefault(plugin => (plugin.Name == "Anti-Drown"));
                if (_pluginAntiDrown != null)
                {
                    _pluginAntiDrownWasEnabled = _pluginAntiDrown.Enabled;
                    _pluginAntiDrown.Enabled = false;
                }

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");

                GuiShowProgress(null);
            }
        }

        #endregion
    }


    public class WoWPointNamed
    {
        public WoWPointNamed(WoWPoint location,
                             string name,
                             bool isStarting)
        {
            Location = location;
            Name = (!string.IsNullOrEmpty(name) ? name : location.ToString());
            IsStarting = isStarting;
        }


        public WoWPointNamed(WoWPoint location,
                             string name)
            : this(location, name, false)
        {
            // empty
        }

        public WoWPointNamed(WoWPoint location)
            : this(location, null, false)
        {
            // empty
        }

        public WoWPointNamed()
            : this(WoWPoint.Empty, null, false)
        {
            // empty
        }

        public WoWPoint Location { get; set; }
        public string Name { get; set; }
        public bool IsStarting { get; set; }
    }


    #region Reusable behaviors
    // The behaviors in this section were designed to be efficient and robust.
    // The robustness results in some larger-than-normal, but swift code.
    // We also designed them to be reused in other behaviors--just copy, paste,
    // and call them as-needed.

    public class HuntingGroundBehavior
    {
        public delegate bool BehaviorFailIfNoTargetsDelegate();
        public delegate double DistanceDelegate();
        public delegate bool IsViableTargetDelegate(WoWObject target);
        public delegate WoWPointNamed LocationDelegate();
        public delegate void LoggerDelegate(string messageType, string format, params object[] args);
        public delegate WoWObject WoWObjectDelegate();


        public HuntingGroundBehavior(LoggerDelegate loggerDelegate,
                                     IsViableTargetDelegate isViableTarget,
                                     WoWPoint huntingGroundAnchor,
                                     double collectionDistance)
        {
            CollectionDistance = collectionDistance;
            HuntingGroundAnchor = new WoWPointNamed(huntingGroundAnchor, "Hunting Ground Anchor", true);
            IsViableTarget = isViableTarget;
            Logger = loggerDelegate;

            // UseHotspots(null, false);
        }


        public void MobEngaged(WoWObject wowObject)
        {
            if (wowObject == CurrentTarget)
            { _currentTargetAutoBlacklistTimer.Stop(); }
        }


        // Public properties...
        public double CollectionDistance { get; private set; }
        public WoWObject CurrentTarget { get; private set; }
        public Queue<WoWPoint> Hotspots { get; set; }
        public WoWPointNamed HuntingGroundAnchor { get; private set; }
        public IsViableTargetDelegate IsViableTarget { get; private set; }


        // Private properties & data...
        private const string AuraName_DruidAquaticForm = "Aquatic Form";
        private readonly TimeSpan Delay_AutoBlacklist = TimeSpan.FromMinutes(7);
        private readonly TimeSpan Delay_RepopWait = TimeSpan.FromMilliseconds(500);
        private readonly TimeSpan Delay_StatusUpdateThrottle = TimeSpan.FromMilliseconds(1000);
        private readonly TimeSpan Delay_WoWClientMovementThrottle = TimeSpan.FromMilliseconds(0);
        private TimeSpan Delay_WowClientLagTime { get { return (TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150)); } }
        private readonly LoggerDelegate Logger;
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }
        private const double MinDistanceToUse_DruidAquaticForm = 27.0;
        private int SpellId_DruidAquaticForm = 1066;
        private IEnumerable<WoWObject> ViableTargets()
        {
            return (ObjectManager.GetObjectsOfType<WoWObject>(true, false)
                    .Where(target => IsViableTarget(target)
                                     && (target.Distance < CollectionDistance))
                    .OrderBy(target => (Me.IsSwimming
                                        ? target.Distance
                                        : Me.Location.SurfacePathDistance(target.Location))));
        }

        private TimeSpan _currentTargetAutoBlacklistTime = TimeSpan.FromSeconds(1);
        private readonly Stopwatch _currentTargetAutoBlacklistTimer = new Stopwatch();
        private Queue<WoWPointNamed> _hotSpots = new Queue<WoWPointNamed>();
        private WoWPointNamed _huntingGroundWaitPoint = new WoWPointNamed(WoWPoint.Empty,
                                                                                            "Hunting Ground Wait Point");
        private readonly Stopwatch _repopWaitingTime = new Stopwatch();


        /// <summary>
        /// The created behavior was meant to be used in a PrioritySelector.
        /// It may also have uses inside other TreeSharp Composites.
        /// </summary>
        /// 
        /// <returns>
        /// <para>* RunStatus.Failure, if current target is viable.
        /// It will also return Failure if no targets could be located and failIfNoTargets is true</para>
        /// <para>* RunStatus.Success, if acquiring a target (or waiting for them to repop)</para>
        /// </returns>
        ///
        public Composite CreateBehavior_SelectTarget()
        {
            return (CreateBehavior_SelectTarget(() => false));
        }

        public Composite CreateBehavior_SelectTarget(BehaviorFailIfNoTargetsDelegate failIfNoTargets)
        {
            return (
            new PrioritySelector(

                // If we haven't engaged the mob when the auto-blacklist timer expires, give up on it and move on...
                new Decorator(ret => ((CurrentTarget != null)
                                        && (_currentTargetAutoBlacklistTimer.Elapsed > _currentTargetAutoBlacklistTime)),
                    new Action(delegate
                    {
                        Logger("warning", "Taking too long to engage '{0}'--blacklisting", CurrentTarget.Name);
                        CurrentTarget.LocallyBlacklist(Delay_AutoBlacklist);
                        CurrentTarget = null;
                    })),


                // If we don't have a current target, select a new one...
                // Once we select a target, its 'locked in' (unless it gets blacklisted).  This prevents us
                // from running back and forth between two equidistant targets.
                new Decorator(ret => ((CurrentTarget == null)
                                      || !CurrentTarget.IsValid
                                      || CurrentTarget.IsLocallyBlacklisted()
                                      || !IsViableTarget(CurrentTarget)),
                    new PrioritySelector(context => CurrentTarget = ViableTargets().FirstOrDefault(),

                        // If we found next target, we're done...
                        new Decorator(ret => (CurrentTarget != null),
                            new Action(delegate
                            {
                                _huntingGroundWaitPoint.Location = WoWPoint.Empty;

                                if (CurrentTarget is WoWUnit)
                                { CurrentTarget.ToUnit().Target(); }

                                _currentTargetAutoBlacklistTime = CalculateAutoBlacklistTime(CurrentTarget);
                                _currentTargetAutoBlacklistTimer.Reset();
                                _currentTargetAutoBlacklistTimer.Start();
                            })),

                        // If we've exhausted mob/object supply in area, and we need to wait, do so...
                        new Decorator(ret => !failIfNoTargets(),

                            // Move back to hunting ground anchor --
                            new PrioritySelector(

                                // If we've more than one hotspot, head to the next one...
                                new Decorator(ret => (_hotSpots.Count() > 1),
                                    new Sequence(context => FindNextHotspot(),
                                        new Action(nextHotspot => TreeRoot.StatusText = "No targets--moving to "
                                                                                        + ((WoWPointNamed)nextHotspot).Name),
                                        CreateBehavior_InternalMoveTo(() => FindNextHotspot())
                                        )),

                                // We find a point 'near' our anchor at which to wait...
                // This way, if multiple people are using the same profile at the same time,
                // they won't be standing on top of each other.
                                new Decorator(ret => (_huntingGroundWaitPoint.Location == WoWPoint.Empty),
                                    new Action(delegate
                                        {
                                            _huntingGroundWaitPoint.Location = HuntingGroundAnchor.Location.FanOutRandom(CollectionDistance * 0.25);
                                            TreeRoot.StatusText = "No targets--moving to wait near " + _huntingGroundWaitPoint.Name;
                                            _repopWaitingTime.Reset();
                                            _repopWaitingTime.Start();
                                        })),

                                // Move to our selected random point...
                                new Decorator(ret => (Me.Location.Distance(_huntingGroundWaitPoint.Location) > Navigator.PathPrecision),
                                    CreateBehavior_InternalMoveTo(() => _huntingGroundWaitPoint)),

                                // Tell user what's going on...
                                new Sequence(
                                    new Action(delegate
                                        {
                                            TreeRoot.GoalText = this.GetType().Name + ": Waiting for Repops";
                                            TreeRoot.StatusText = "No targets in area--waiting for repops.  " + BuildTimeAsString(_repopWaitingTime.Elapsed);
                                        }),
                                    new WaitContinue(Delay_RepopWait, ret => false, new ActionAlwaysSucceed()))
                                ))
                        )),

                // Re-select target, if it was lost (perhaps, due to combat)...
                new Decorator(ret => ((CurrentTarget is WoWUnit) && (Me.CurrentTarget != CurrentTarget)),
                    new Action(delegate { CurrentTarget.ToUnit().Target(); }))
                ));
        }


        public Composite CreateBehavior_MoveNearTarget(WoWObjectDelegate target,
                                                          DistanceDelegate minRange,
                                                          DistanceDelegate maxRange)
        {
            return (
            new PrioritySelector(context => target(),

                // If we're too far from target, move closer...
                new Decorator(wowObject => (((WoWObject)wowObject).Distance > maxRange()),
                    new Sequence(
                        new DecoratorThrottled(Delay_StatusUpdateThrottle, ret => true,
                            new Action(wowObject =>
                            {
                                TreeRoot.StatusText = string.Format("Moving to {0} (distance: {1:0.0}) ",
                                                                    ((WoWObject)wowObject).Name,
                                                                    ((WoWObject)wowObject).Distance);
                            })),

                        CreateBehavior_InternalMoveTo(() => new WoWPointNamed(target().Location))
                    )),

                // If we're too close to target, back up...
                new Decorator(wowObject => (((WoWObject)wowObject).Distance < minRange()),
                    new PrioritySelector(

                        // If backing up, make sure we're facing the target...
                        new Decorator(ret => Me.MovementInfo.MovingBackward,
                            new Action(wowObject => WoWMovement.Face(((WoWObject)wowObject).Guid))),

                        // Start backing up...
                        new Action(wowObject =>
                        {
                            TreeRoot.StatusText = "Too close to \"" + ((WoWObject)wowObject).Name + "\"--backing up";
                            WoWMovement.MoveStop();
                            WoWMovement.Face(((WoWObject)wowObject).Guid);
                            WoWMovement.Move(WoWMovement.MovementDirection.Backwards);
                        })
                        )),

                // We're between MinRange and MaxRange, stop movement and face the target...
                new Decorator(ret => Me.IsMoving,
                    new Sequence(
                        new Action(delegate { WoWMovement.MoveStop(); }),
                        new Action(wowObject => WoWMovement.Face(((WoWObject)wowObject).Guid)),
                        new WaitContinue(Delay_WowClientLagTime, ret => false, new ActionAlwaysSucceed()),
                        new ActionAlwaysFail()      // fall through to next element
                        ))
            ));
        }


        public Composite CreateBehavior_MoveToLocation(LocationDelegate location)
        {
            return (
            new PrioritySelector(

                // If we're not at location, move to it...
                new Decorator(wowPoint => (Me.Location.Distance((WoWPoint)wowPoint) > Navigator.PathPrecision),
                    new Sequence(
                        new DecoratorContinueThrottled(Delay_StatusUpdateThrottle, ret => true,
                            new Action(wowPoint => TreeRoot.StatusText = "Moving to " + location().Name)),

                        CreateBehavior_InternalMoveTo(() => location())
                    ))
            ));
        }


        public Composite CreateBehavior_MoveToTarget()
        {
            return (CreateBehavior_MoveToTarget(() => CurrentTarget));
        }


        public Composite CreateBehavior_MoveToTarget(WoWObjectDelegate target)
        {
            return (
            new PrioritySelector(context => target(),

                // If we 'pass by' the current hotspot on the way to the target, advance to next hotspot...
                // This prevents bot-like 'back tracking'.
                new Decorator(ret => (Me.Location.Distance(_hotSpots.Peek().Location) < (CollectionDistance / 2)),
                    new Action(delegate
                    {
                        // Rotate to the next hotspot in the list...
                        _hotSpots.Enqueue(_hotSpots.Peek());
                        _hotSpots.Dequeue();

                        return (RunStatus.Failure);     // Fall through to next
                    })),


                // If we're not at target, move to it...
                new Decorator(wowObject => (((WoWObject)wowObject).Distance > ((WoWObject)wowObject).InteractRange),
                    new Sequence(
                        new DecoratorContinueThrottled(Delay_StatusUpdateThrottle, ret => true,
                            new Action(wowObject =>
                            {
                                TreeRoot.StatusText = string.Format("Moving to {0} (distance: {1:0.0}) ",
                                                                    ((WoWObject)wowObject).Name,
                                                                    ((WoWObject)wowObject).Distance);
                            })),

                        CreateBehavior_InternalMoveTo(() => new WoWPointNamed(target().Location))
                    )),


                // Update status...
                new Action(wowObject =>
                    {
                        TreeRoot.StatusText = string.Format("Moving to {0} (distance: {1:0.0}) ",
                                                            ((WoWObject)wowObject).Name,
                                                            ((WoWObject)wowObject).Distance);
                        return (RunStatus.Failure);
                    })
            ));
        }


        private static string BuildTimeAsString(TimeSpan timeSpan)
        {
            string formatString = string.Empty;

            if (timeSpan.Hours > 0)
            { formatString = "{0:D2}h:{1:D2}m:{2:D2}s"; }
            else if (timeSpan.Minutes > 0)
            { formatString = "{1:D2}m:{2:D2}s"; }
            else
            { formatString = "{2:D2}s"; }

            return (string.Format(formatString, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
        }


        private TimeSpan CalculateAutoBlacklistTime(WoWObject wowObject)
        {
            double timeToWowObject;

            if (Me.IsSwimming)
            { timeToWowObject = Me.Location.Distance(wowObject.Location) / Me.MovementInfo.SwimmingForwardSpeed; }
            else
            { timeToWowObject = Me.Location.SurfacePathDistance(wowObject.Location) / Me.MovementInfo.RunSpeed; }

            timeToWowObject *= 2.5;     // factor of safety
            timeToWowObject = Math.Max(timeToWowObject, 20.0);  // 20sec hard lower-limit

            return (TimeSpan.FromSeconds(timeToWowObject));
        }


        private Composite CreateBehavior_InternalMoveTo(LocationDelegate locationDelegate)
        {
            return (
            new Sequence(context => locationDelegate(),

                // Druids, switch to Aquatic Form if swimming and distance dictates...
                new DecoratorContinue(ret => (SpellManager.CanCast(SpellId_DruidAquaticForm)
                                                && !Me.HasAura(AuraName_DruidAquaticForm)
                                                && (Me.Location.Distance(locationDelegate().Location) > MinDistanceToUse_DruidAquaticForm)),
                    new Action(delegate { SpellManager.Cast(SpellId_DruidAquaticForm); })),

                // Move...
                new Action(delegate
                {
                    // Try to use Navigator to get there...
                    WoWPointNamed destination = locationDelegate();
                    MoveResult moveResult = Navigator.MoveTo(destination.Location);

                    // If Navigator fails, fall back to click-to-move...
                    if ((moveResult == MoveResult.Failed) || (moveResult == MoveResult.PathGenerationFailed))
                    { WoWMovement.ClickToMove(destination.Location); }
                }),

                new WaitContinue(Delay_WoWClientMovementThrottle, ret => false, new ActionAlwaysSucceed())
                )
            );
        }


        private WoWPointNamed FindStartingHotspot(bool randomStartingHotspot)
        {
            IEnumerable<WoWPointNamed> hotspotsByDistance;
            IEnumerable<WoWPointNamed> hotspotsStarting;
            Random random = new Random((int)DateTime.Now.Ticks);

            hotspotsByDistance = (Me.IsSwimming
                                  ? _hotSpots.OrderBy(hotspot => hotspot.Location.Distance(Me.Location))
                                  : _hotSpots.OrderBy(hotspot => hotspot.Location.SurfacePathDistance(Me.Location)));

            hotspotsStarting = hotspotsByDistance.Where(hotspot => (hotspot.IsStarting == true));
            if (hotspotsStarting.Count() <= 0)
            {
                hotspotsStarting = hotspotsByDistance;
                Logger("debug", "No explicit starting hotspot(s)--considering all");
            }

            Logger("debug", "Hotspot count: {0} ({1})", hotspotsByDistance.Count(),
                        (randomStartingHotspot ? "randomized" : "starting at nearest"));

            WoWPoint startingLocation = (randomStartingHotspot
                                                ? hotspotsStarting.OrderBy(ret => random.Next()).FirstOrDefault().Location
                                                : hotspotsStarting.FirstOrDefault().Location);

            // Rotate the hotspot queue such that the nearest hotspot is on top...
            while (_hotSpots.Peek().Location != startingLocation)
            { _hotSpots.Enqueue(_hotSpots.Dequeue()); }

            Logger("debug", "Starting hotspot is {0}", _hotSpots.Peek().Name);

            return (_hotSpots.Peek());
        }


        private WoWPointNamed FindNextHotspot()
        {
            WoWPointNamed currentHotspot = _hotSpots.Peek();

            // If we haven't reached the current hotspot, it is still the 'next' one...
            if (Me.Location.Distance(currentHotspot.Location) > Navigator.PathPrecision)
            { return (currentHotspot); }

            // Otherwise, rotate to the next hotspot in the list...
            _hotSpots.Enqueue(_hotSpots.Dequeue());

            return (_hotSpots.Peek());
        }


        public void UseHotspots(IEnumerable<WoWPointNamed> hotspots,
                                        bool randomizeStartingHotspot)
        {
            _hotSpots = new Queue<WoWPointNamed>(hotspots ?? new WoWPointNamed[0]);

            if (_hotSpots.Count() <= 0)
            { _hotSpots.Enqueue(HuntingGroundAnchor); }

            FindStartingHotspot(randomizeStartingHotspot);
        }
    }


    /// <summary>
    /// <para>This behavior moves to fill a toon's lungs when needed.  It utilizes skills available
    /// such as a Warlock's Unending Breath, or a Druid's Aquatic Form in its calculations.
    /// The behavior looks for nearby air sources (underwater vents, water's surface, etc), and
    /// bases its time to move upon the distance that needs to be traveled to get to the air source
    /// and the toon's movement speed in the water.</para>
    /// <para>This behavior will *not* use engineering and other devices that may allow you to
    /// breath--as this could interfere with AutoEquip-type plugins.  This behavior will also
    /// *not* use potions to help you breathe, as that is a 'profile-level' decision that should
    /// be made.  In other words, you wouldn't want this behavior to burn a potion to collect
    /// a handful of object from shallow, but swimmable water.  The profile would know the expected
    /// duration underwater, but there is no way for this behavior to know such information.</para>
    /// </summary>
    //
    // Usage:
    //  private SwimBreathBehavior   _swimBreathBehavior  = new SwimBreathBehavior((msgType, fmt, args) => LogMessage(msgType,  fmt, args));
    //  ...
    //  new PrioritySelector(
    //      _swimBreathBehavior.CreateBehavior(),
    //
    public class SwimBreathBehavior
    {
        public delegate void LoggerDelegate(string messageType, string format, params object[] args);


        public SwimBreathBehavior(LoggerDelegate loggerDelegate)
        {
            Logger = loggerDelegate;
        }


        // Private properites & data...
        private const string AuraName_DruidAquaticForm = "Aquatic Form";
        private const string AuraName_WarlockUnendingBreath = "Unending Breath";
        private int BreathTimeRemaining
        {
            get
            {
                return ((Timer_SwimBreath.IsVisible)
                       ? (int)Timer_SwimBreath.CurrentTime
                       : int.MaxValue);
            }
        }
        private readonly TimeSpan Delay_StatusUpdateThrottle = TimeSpan.FromMilliseconds(3000);
        private readonly LoggerDelegate Logger;
        private int MinTime_DruidBreath = 30000;    // in milliseconds
        private int MinTime_WarlockBreath = 30000;    // in milliseconds
        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        private int SpellId_DruidAquaticForm = 1066;
        private int SpellId_WarlockUnendingBreath = 5697;
        private readonly TimeSpan ThrottleTimer_BreathCheck = TimeSpan.FromSeconds(5);
        private readonly TimeSpan ThrottleTimer_WarlockBreath = TimeSpan.FromSeconds(30);
        private readonly TimeSpan Timer_AuraRefresh_EnduringBreath = TimeSpan.FromMilliseconds(180000);
        private MirrorTimerInfo Timer_SwimBreath { get { return (Me.GetMirrorTimerInfo(MirrorTimerType.Breath)); } }
        private int[] UnderwaterAirSourceObjectIds = { 177524 /* bubbly fissure */
                                                                          };

        private Composite _behaviorRoot;
        private bool _isSwimBreathNeeded;
        private AirSource _nearestAirSource;

        // Private structures...
        private struct AirSource
        {
            public WoWPoint Location;
            public string Name;

            public AirSource(WoWPoint location, string name) { Location = location; Name = name; }
            public double Distance { get { return (Location.Distance(ObjectManager.Me.Location)); } }
            public static AirSource Empty = new AirSource(WoWPoint.Empty, "NONE");
        }

        // Private LINQs
        private IEnumerable<WoWObject> UnderwaterAirSources
        {
            get
            {
                return (
                    ObjectManager.GetObjectsOfType<WoWObject>(true, false)
                .OrderBy(target => Me.Location.Distance(target.Location))
                .Where(target => UnderwaterAirSourceObjectIds.Contains((int)target.Entry))
                    );
            }
        }


        private TimeSpan AuraTimeLeft(string auraName)
        {
            WoWAura wowAura = Me.GetAuraByName(auraName);

            return ((wowAura != null) ? wowAura.TimeLeft : TimeSpan.Zero);
        }


        /// <summary>
        /// The created behavior was meant to be used in a PrioritySelector.
        /// It may also have uses inside other TreeSharp Composites.
        /// </summary>
        /// 
        /// <returns>
        /// <para>* RunStatus.Failure, if swim breath is not needed</para>
        /// <para>* RunStatus.Success, if we're catching our breath, or moving for it</para>
        /// </returns>
        /// 
        public Composite CreateBehavior()
        {
            return (_behaviorRoot ?? (_behaviorRoot =
                new Decorator(ret => Me.IsSwimming,
                    new PrioritySelector(

                        // Moving to, or fetching breath...
                        new Decorator(ret => _isSwimBreathNeeded,
                            new PrioritySelector(

                                // If toon is filling lungs, stay put until full...
                                new Decorator(ret => ((Timer_SwimBreath.ChangePerMillisecond > 0)
                                                      && (Timer_SwimBreath.CurrentTime < Timer_SwimBreath.MaxValue)),
                                    new Action(delegate
                                    {
                                        WoWMovement.MoveStop();
                                        TreeRoot.StatusText = "Waiting for full breath";
                                    })),

                                // If lungs are full, back to work...
                                new Decorator(ret => (Timer_SwimBreath.CurrentTime >= Timer_SwimBreath.MaxValue),
                                    new Action(delegate { _isSwimBreathNeeded = false; })),

                                // Move toon to air source, if needed...
                                new Decorator(ret =>
                                    {
                                        _nearestAirSource = GetNearestAirSource();
                                        return (_nearestAirSource.Distance > Navigator.PathPrecision);
                                    },
                                    new Sequence(
                                        new DecoratorContinueThrottled(Delay_StatusUpdateThrottle, ret => true,
                                            new Action(delegate
                                            {
                                                TreeRoot.StatusText = string.Format("Moving to {0} for breath. (distance {1:0.0})",
                                                                                    _nearestAirSource.Name,
                                                                                    _nearestAirSource.Distance);
                                            })),

                                        new Action(delegate { UnderwaterMoveTo(_nearestAirSource.Location); })
                                        )
                                    )
                            )),


                        // If we're a Warlock, refresh Unending Breath as needed...
                        new DecoratorThrottled(ThrottleTimer_WarlockBreath,
                            ret => (SpellManager.CanCast(SpellId_WarlockUnendingBreath)
                                    && (AuraTimeLeft(AuraName_WarlockUnendingBreath) <= Timer_AuraRefresh_EnduringBreath)),
                            new Action(delegate { SpellManager.Cast(SpellId_WarlockUnendingBreath); })),


                        // If time to breathe, do something about it...
                        new DecoratorThrottled(ThrottleTimer_BreathCheck,
                            ret => IsBreathNeeded(),
                            new PrioritySelector(

                                // If we're a Druid, switch to Aquatic form for breath...
                                new Decorator(ret => (SpellManager.CanCast(SpellId_DruidAquaticForm)
                                                      && !Me.HasAura(AuraName_DruidAquaticForm)),
                                    new Action(delegate
                                    {
                                        SpellManager.Cast(SpellId_DruidAquaticForm);
                                        TreeRoot.StatusText = "Switching to Aquatic Form for breath";
                                        _isSwimBreathNeeded = true;
                                    })),


                                // Otherwise, we need to deal with 'normal' way to catch breath...
                                new Action(delegate
                                {
                                    _nearestAirSource = GetNearestAirSource();
                                    Logger("info", "Moving to {0} for breath. (distance {1:0.0})",
                                                    _nearestAirSource.Name, _nearestAirSource.Distance);
                                    _isSwimBreathNeeded = true;
                                })
                            ))
                    ))));
        }


        private AirSource GetNearestAirSource()
        {
            // Assume water's surface is nearest breath...
            AirSource nearestAirSource = new AirSource(Me.Location.WaterSurface(), "water's surface");
            WoWObject underwaterAirSource = UnderwaterAirSources.FirstOrDefault();

            // If underwater air source exists, and is closer that the water's surface...
            if ((underwaterAirSource != null)
                && (Me.Location.Distance(underwaterAirSource.Location) <= nearestAirSource.Distance))
            {
                nearestAirSource.Location = underwaterAirSource.Location;
                nearestAirSource.Name = underwaterAirSource.Name;
            }

            return (nearestAirSource);
        }


        private bool IsBreathNeeded()
        {
            int breathTimeRemaining = BreathTimeRemaining;

            if (Me.Class == WoWClass.Druid)
            { return (breathTimeRemaining < MinTime_DruidBreath); }

            else if (Me.Class == WoWClass.Warlock)
            { return (breathTimeRemaining < MinTime_WarlockBreath); }

            // Calculate time needed to get to an air source...
            AirSource airSource = GetNearestAirSource();
            double travelTime;

            travelTime = (((airSource.Location.Distance(Me.Location) / Me.MovementInfo.SwimmingForwardSpeed)
                          * 2.75)   // factor of safety
                          + (3 * ThrottleTimer_BreathCheck.TotalSeconds));
            travelTime = Math.Min(travelTime, 30.0);    // Hard-minimum of 30secs
            travelTime *= 1000;     // to milliseconds

            return (breathTimeRemaining <= travelTime);
        }


        private void UnderwaterMoveTo(WoWPoint location)
        {
            // Try to use Navigator to get there...
            MoveResult moveResult = Navigator.MoveTo(location);

            // If Navigator fails, resort to click-to-move...
            if ((moveResult == MoveResult.Failed) || (moveResult == MoveResult.PathGenerationFailed))
            { WoWMovement.ClickToMove(location); }
        }
    }


    /// <summary>
    /// This behavior is necessary since Honorbuddy is incapable of moving underwater.
    /// </summary>
    //
    // Usage:
    //  private UnderwaterLootingBehavior   _underwaterLootingBehavior  = new UnderwaterLootingBehavior((msgType, fmt, args) => LogMessage(msgType,  fmt, args));
    //  ...
    //  new PrioritySelector(
    //      _underwaterLootingBehavior.CreateBehavior(),
    //
    public class UnderwaterLootingBehavior
    {
        public delegate bool ForceLootDelegate();
        public delegate void LoggerDelegate(string messageType, string format, params object[] args);


        public UnderwaterLootingBehavior(LoggerDelegate loggerDelegate)
        {
            Logger = loggerDelegate;
        }


        // Private properties & data...
        private const string AuraName_DruidAquaticForm = "Aquatic Form";
        private readonly TimeSpan Delay_BlacklistLootedMob = TimeSpan.FromMinutes(7);
        private readonly TimeSpan Delay_WaitForLootCleanup = TimeSpan.FromMilliseconds(5000);
        private TimeSpan Delay_WowClientLagTime { get { return (TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150)); } }
        private readonly TimeSpan Delay_WowClientWaitForLootFrame = TimeSpan.FromSeconds(10);
        private readonly LoggerDelegate Logger;
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }
        private int SpellId_DruidAquaticForm = 1066;

        private Composite _behaviorRoot;
        private WoWObject _currentTarget;

        // Private LINQ...
        private IEnumerable<WoWUnit> LootList
        {
            get
            {
                return (ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                        .Where(target => (target.Dead && target.Lootable
                                          && !target.IsLootingBlacklisted())));
            }
        }


        /// <summary>
        /// The created behavior was meant to be used in a PrioritySelector.
        /// It may also have uses inside other TreeSharp Composites.
        /// </summary>
        /// 
        /// <returns>
        /// <para>* RunStatus.Failure, if looting is not needed</para>
        /// <para>* RunStatus.Success, if we're in the process of looting things</para>
        /// </returns>
        /// 
        public Composite CreateBehavior(ForceLootDelegate forceLoot)
        {
            return (_behaviorRoot ?? (_behaviorRoot =
                new Decorator(ret => ((CharacterSettings.Instance.LootMobs || forceLoot()) && (LootList.Count() > 0)),
                    new PrioritySelector(

                        // If we're swimming, we need to do loot cleanup for ourselves...
                        new Decorator(ret => (Me.IsSwimming || forceLoot()),
                            new PrioritySelector(context => _currentTarget = LootList.FirstOrDefault(),

                                // If not at nearest target, move to it...
                                new Decorator(ret => (_currentTarget.Distance > _currentTarget.InteractRange),
                                    new Action(delegate
                                    {
                                        TreeRoot.StatusText = string.Format("Moving to loot target '{0}' (distance {1})...",
                                                                            _currentTarget.Name,
                                                                            _currentTarget.Distance);
                                        UnderwaterMoveTo(_currentTarget.Location);
                                    })),

                                // Within interact range, so loot it...
                // NOTE: that we have to locally blacklist looted targets.  They sometimes
                // have unique (e.g., quest-starting type) items on them.  If we already have
                // have such an item in our inventory, the target remains lootable, but there
                // is nothing we can pick up from it.  The blacklist prevents us from getting
                // into silly loops because of such mechanics.
                                new Sequence(
                                    new Action(delegate { WoWMovement.MoveStop(); }),
                                    new WaitContinue(Delay_WowClientLagTime, ret => false, new ActionAlwaysSucceed()),
                                    new Action(delegate { _currentTarget.Interact(); }),
                                    new WaitContinue(Delay_WowClientWaitForLootFrame,
                                                     ret => LootFrame.Instance.IsVisible,
                                                     new ActionAlwaysSucceed()),
                                    new DecoratorContinue(ret => LootFrame.Instance.IsVisible,
                                        new Action(ret => LootFrame.Instance.LootAll())),
                                    new Action(delegate { _currentTarget.LootingBlacklist(Delay_BlacklistLootedMob); })
                                    )
                            )),

                        // We're not swimming, so we want to wait for the 'normal' looting behavior
                // to scoop up the loot before allowing other behaviors to continue...
                // This keeps it from taking a few steps towards next mob, only to go back to
                // previous mob and loot it.  This technique can still fail if Honorbddy is slow to update
                // infomation.  However, it shuts a lot of back-tracking.
                        new WaitContinue(Delay_WaitForLootCleanup, ret => false, new ActionAlwaysSucceed())
                    )
                )));
        }


        private void UnderwaterMoveTo(WoWPoint location)
        {
            // If we're a Druid, use Aquatic Form...
            if (SpellManager.CanCast(SpellId_DruidAquaticForm) && !Me.HasAura(AuraName_DruidAquaticForm))
            { SpellManager.Cast(SpellId_DruidAquaticForm); }

            // Try to use Navigator to get there...
            MoveResult moveResult = Navigator.MoveTo(location);

            if ((moveResult == MoveResult.Failed) || (moveResult == MoveResult.PathGenerationFailed))
            { WoWMovement.ClickToMove(location); }
        }
    }

    #endregion  // Reusable behaviors


    #region Extensions to HBcore

    public class DecoratorContinueThrottled : DecoratorContinue
    {
        public DecoratorContinueThrottled(TimeSpan throttleTime,
                                          CanRunDecoratorDelegate canRun,
                                          Composite composite)
            : base(canRun, composite)
        {
            _throttleTime = throttleTime;

            _throttle = new Stopwatch();
            _throttle.Reset();
            _throttle.Start();
        }


        protected override bool CanRun(object context)
        {
            if (_throttle.Elapsed < _throttleTime)
            { return (false); }

            _throttle.Reset();
            _throttle.Start();

            return (base.CanRun(context));
        }


        private Stopwatch _throttle;
        private TimeSpan _throttleTime;
    }


    public class DecoratorThrottled : Decorator
    {
        public DecoratorThrottled(TimeSpan throttleTime,
                                  CanRunDecoratorDelegate canRun,
                                  Composite composite)
            : base(canRun, composite)
        {
            _throttleTime = throttleTime;

            _throttle = new Stopwatch();
            _throttle.Reset();
            _throttle.Start();
        }


        protected override bool CanRun(object context)
        {
            if (_throttle.Elapsed < _throttleTime)
            { return (false); }

            _throttle.Reset();
            _throttle.Start();

            return (base.CanRun(context));
        }


        private Stopwatch _throttle;
        private TimeSpan _throttleTime;
    }


    // The HBcore 'global' blacklist will also prevent looting.  We don't want that.
    // Since the HBcore blacklist is not built to instantiate, we have to roll our
    // own.  <sigh>
    public class LocalBlackList
    {
        public LocalBlackList(TimeSpan maxSweepTime)
        {
            _maxSweepTime = maxSweepTime;
            _stopWatchForSweeping.Start();
        }

        private Dictionary<ulong, DateTime> _blackList = new Dictionary<ulong, DateTime>();
        private TimeSpan _maxSweepTime;
        private Stopwatch _stopWatchForSweeping = new Stopwatch();


        public void Add(ulong guid, TimeSpan timeSpan)
        {
            if (_stopWatchForSweeping.Elapsed > _maxSweepTime)
            { RemoveExpired(); }

            _blackList[guid] = DateTime.Now.Add(timeSpan);
        }


        public bool Contains(ulong guid)
        {
            if (_stopWatchForSweeping.Elapsed > _maxSweepTime)
            { RemoveExpired(); }

            return (_blackList.ContainsKey(guid));
        }


        public void RemoveExpired()
        {
            DateTime now = DateTime.Now;

            List<ulong> expiredEntries = (from key in _blackList.Keys
                                          where (_blackList[key] < now)
                                          select key).ToList();

            foreach (ulong entry in expiredEntries)
            { _blackList.Remove(entry); }

            _stopWatchForSweeping.Reset();
            _stopWatchForSweeping.Start();
        }
    }


    public static class WoWObject_Extensions
    {
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }

        // We provide our own 'local' blacklist.  If we use the global one maintained by HBcore,
        // that will prevent us from looting also.
        private static LocalBlackList _blackList = new LocalBlackList(TimeSpan.FromSeconds(30));
        private static LocalBlackList _blackListLooting = new LocalBlackList(TimeSpan.FromSeconds(30));

        public static void LocallyBlacklist(this WoWObject wowObject,
                                                 TimeSpan timeSpan)
        {
            _blackList.Add(wowObject.Guid, timeSpan);
        }

        public static void LootingBlacklist(this WoWObject wowObject,
                                                 TimeSpan timeSpan)
        {
            _blackListLooting.Add(wowObject.Guid, timeSpan);
        }


        public static bool IsLocallyBlacklisted(this WoWObject wowObject)
        {
            return (_blackList.Contains(wowObject.Guid));
        }

        public static bool IsLootingBlacklisted(this WoWObject wowObject)
        {
            return (_blackListLooting.Contains(wowObject.Guid));
        }
    }


    public static class WoWUnit_Extensions
    {
        private static LocalPlayer Me { get { return (ObjectManager.Me); } }

        public static bool IsInOurParty(this WoWUnit wowUnit)
        {
            return ((Me.PartyMembers.FirstOrDefault(partyMember => (partyMember.Guid == wowUnit.Guid))) != null);
        }
    }


    public static class WoWPoint_Extensions
    {
        public static Random _random = new Random((int)DateTime.Now.Ticks);

        private static LocalPlayer Me { get { return (ObjectManager.Me); } }
        public const double TAU = (2 * Math.PI);    // See http://tauday.com/


        public static WoWPoint Add(this WoWPoint wowPoint,
                                    double x,
                                    double y,
                                    double z)
        {
            return (new WoWPoint((wowPoint.X + x), (wowPoint.Y + y), (wowPoint.Z + z)));
        }


        public static WoWPoint AddPolarXY(this WoWPoint wowPoint,
                                           double xyHeadingInRadians,
                                           double distance,
                                           double zModifier)
        {
            return (wowPoint.Add((Math.Cos(xyHeadingInRadians) * distance),
                                 (Math.Sin(xyHeadingInRadians) * distance),
                                 zModifier));
        }


        // Finds another point near the destination.  Useful when toon is 'waiting' for something
        // (e.g., boat, mob repops, etc). This allows multiple people running
        // the same profile to not stand on top of each other while waiting for
        // something.
        public static WoWPoint FanOutRandom(this WoWPoint location,
                                                double maxRadius)
        {
            const int CYLINDER_LINE_COUNT = 12;
            const int MAX_TRIES = 50;
            const double SAFE_DISTANCE_BUFFER = 1.75;

            WoWPoint candidateDestination = location;
            int tryCount;

            // Most of the time we'll find a viable spot in less than 2 tries...
            // However, if you're standing on a pier, or small platform a
            // viable alternative may take 10-15 tries--its all up to the
            // random number generator.
            for (tryCount = MAX_TRIES; tryCount > 0; --tryCount)
            {
                WoWPoint circlePoint;
                bool[] hitResults;
                WoWPoint[] hitPoints;
                int index;
                WorldLine[] traceLines = new WorldLine[CYLINDER_LINE_COUNT + 1];

                candidateDestination = location.AddPolarXY((TAU * _random.NextDouble()), (maxRadius * _random.NextDouble()), 0.0);

                // Build set of tracelines that can evaluate the candidate destination --
                // We build a cone of lines with the cone's base at the destination's 'feet',
                // and the cone's point at maxRadius over the destination's 'head'.  We also
                // include the cone 'normal' as the first entry.

                // 'Normal' vector
                index = 0;
                traceLines[index].Start = candidateDestination.Add(0.0, 0.0, maxRadius);
                traceLines[index].End = candidateDestination.Add(0.0, 0.0, -maxRadius);

                // Cylinder vectors
                for (double turnFraction = 0.0; turnFraction < TAU; turnFraction += (TAU / CYLINDER_LINE_COUNT))
                {
                    ++index;
                    circlePoint = candidateDestination.AddPolarXY(turnFraction, SAFE_DISTANCE_BUFFER, 0.0);
                    traceLines[index].Start = circlePoint.Add(0.0, 0.0, maxRadius);
                    traceLines[index].End = circlePoint.Add(0.0, 0.0, -maxRadius);
                }


                // Evaluate the cylinder...
                // The result for the 'normal' vector (first one) will be the location where the
                // destination meets the ground.  Before this MassTrace, only the candidateDestination's
                // X/Y values were valid.
                GameWorld.MassTraceLine(traceLines.ToArray(),
                                        GameWorld.CGWorldFrameHitFlags.HitTestGroundAndStructures,
                                        out hitResults,
                                        out hitPoints);

                candidateDestination = hitPoints[0];    // From 'normal', Destination with valid Z coordinate


                // Sanity check...
                // We don't want to be standing right on the edge of a drop-off (say we'e on
                // a plaform or pier).  If there is not solid ground all around us, we reject
                // the candidate.  Our test for validity is that the walking distance must
                // not be more than 20% greater than the straight-line distance to the point.
                int viableVectorCount = hitPoints.Sum(point => ((Me.Location.SurfacePathDistance(point) < (Me.Location.Distance(point) * 1.20))
                                                                      ? 1
                                                                      : 0));

                if (viableVectorCount < (CYLINDER_LINE_COUNT + 1))
                { continue; }

                // If new destination is 'too close' to our current position, try again...
                if (Me.Location.Distance(candidateDestination) <= SAFE_DISTANCE_BUFFER)
                { continue; }

                break;
            }

            // If we exhausted our tries, just go with simple destination --
            if (tryCount <= 0)
            { candidateDestination = location; }

            return (candidateDestination);
        }


        public static double SurfacePathDistance(this WoWPoint start,
                                                    WoWPoint destination)
        {
            WoWPoint[] groundPath = Navigator.GeneratePath(start, destination) ?? new WoWPoint[0];

            // We define an invalid path to be of 'infinite' length
            if (groundPath.Length <= 0)
            { return (double.MaxValue); }


            double pathDistance = start.Distance(groundPath[0]);

            for (int i = 0; i < (groundPath.Length - 1); ++i)
            { pathDistance += groundPath[i].Distance(groundPath[i + 1]); }

            return (pathDistance);
        }


        // Returns WoWPoint.Empty if unable to locate water's surface
        public static WoWPoint WaterSurface(this WoWPoint location)
        {
            WoWPoint hitLocation;
            bool hitResult;
            WoWPoint locationUpper = location.Add(0.0, 0.0, 2000.0);
            WoWPoint locationLower = location.Add(0.0, 0.0, -2000.0);

            hitResult = (GameWorld.TraceLine(locationUpper,
                                             locationLower,
                                             GameWorld.CGWorldFrameHitFlags.HitTestLiquid,
                                             out hitLocation)
                         || GameWorld.TraceLine(locationUpper,
                                                locationLower,
                                                GameWorld.CGWorldFrameHitFlags.HitTestLiquid2,
                                                out hitLocation));

            return (hitResult ? hitLocation : WoWPoint.Empty);
        }
    }

    #endregion      // Extensions to HBcore
}

