// Behavior originally contributed by Raphus.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Linq;

using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class UseTransport : CustomForcedBehavior
    {
        /// <summary>
        /// Allows you to use Transports.
        /// ##Syntax##
        /// TransportId: ID of the transport.
        /// TransportStart: Start point of the transport that we will get on when its close enough to that point.
        /// TransportEnd: End point of the transport that we will get off when its close enough to that point.
        /// WaitAt: Where you wish to wait the transport at
        /// GetOff: Where you wish to end up at when transport reaches TransportEnd point
        /// StandOn: The point you wish the stand while you are in the transport
        /// </summary>
        ///
        public UseTransport(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                WoWPoint? legacyEndLocation = LegacyGetAttributeAsWoWPoint("End", false, null, "TransportEndX/Y/Z");
                WoWPoint? legacyGetOffLocation = LegacyGetAttributeAsWoWPoint("Exit", false, null, "GetOffX/Y/Z");
                WoWPoint? legacyStartLocation = LegacyGetAttributeAsWoWPoint("Start", false, null, "TransportStartX/Y/Z");
                WoWPoint? legacyWaitAtLocation = LegacyGetAttributeAsWoWPoint("Entry", false, null, "WaitAtX/Y/Z");

                DestName = GetAttributeAs<string>("DestName", false, ConstrainAs.StringNonEmpty, null) ?? "";
                EndLocation = GetAttributeAsNullable<WoWPoint>("TransportEnd", !legacyEndLocation.HasValue, ConstrainAs.WoWPointNonEmpty, null)
                                    ?? legacyEndLocation
                                    ?? WoWPoint.Empty;
                GetOffLocation = GetAttributeAsNullable<WoWPoint>("GetOff", !legacyGetOffLocation.HasValue, ConstrainAs.WoWPointNonEmpty, null)
                                    ?? legacyGetOffLocation
                                    ?? WoWPoint.Empty;
                StandLocation = GetAttributeAsNullable<WoWPoint>("StandOn", false, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
                StartLocation = GetAttributeAsNullable<WoWPoint>("TransportStart", !legacyStartLocation.HasValue, ConstrainAs.WoWPointNonEmpty, null)
                                    ?? legacyStartLocation
                                    ?? WoWPoint.Empty;
                TransportId = GetAttributeAsNullable<int>("TransportId", true, ConstrainAs.MobId, new[] { "Transport" }) ?? 0;
                WaitAtLocation = GetAttributeAsNullable<WoWPoint>("WaitAt", !legacyWaitAtLocation.HasValue, ConstrainAs.WoWPointNonEmpty, null)
                                    ?? legacyWaitAtLocation
                                    ?? WoWPoint.Empty;
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
        public string DestName { get; private set; }
        public WoWPoint EndLocation { get; private set; }
        public WoWPoint GetOffLocation { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public WoWPoint StandLocation { get; private set; }
        public WoWPoint StartLocation { get; private set; }
        public int TransportId { get; private set; }
        public WoWPoint WaitAtLocation { get; private set; }

        // Private variables for internal state
        private ConfigMemento _configMemento;
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;
        private bool _usedTransport;
        private bool _wasOnWaitLocation;

        // Private properties
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: UseTransport.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~UseTransport()
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
                if (_configMemento != null)
                { _configMemento.Dispose(); }

                _configMemento = null;

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


        private WoWPoint TransportLocation
        {
            get
            {
                var transport = ObjectManager.GetObjectsOfType<WoWGameObject>(true, false).FirstOrDefault(o => o.Entry == TransportId);

                if (transport == null)
                    return WoWPoint.Empty;

                //Tripper.Tools.Math.Matrix m = transport.GetWorldMatrix();

                //return new WoWPoint(m.M41, m.M42, m.M43);

                return transport.WorldLocation;
            }
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
                    new Decorator(
                        ret => !_wasOnWaitLocation,
                        new PrioritySelector(
                            new Decorator(
                                ret => WaitAtLocation.Distance(Me.Location) > 2,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Moving to wait location"),
                                    new Action(ret => Navigator.MoveTo(WaitAtLocation)))),
                            new Sequence(
                                new Action(ret => Navigator.PlayerMover.MoveStop()),
                                new Action(ret => Mount.Dismount()),
                                new Action(ret => _wasOnWaitLocation = true),
                                new Action(ret => TreeRoot.StatusText = "Waiting for transport")))),
                    new Decorator(
                        ret => TransportLocation != WoWPoint.Empty && TransportLocation.Distance(EndLocation) < 2 && _usedTransport,
                        new PrioritySelector(
                            new Decorator(
                                ret => Me.Location.Distance(GetOffLocation) > 2 && StyxWoW.Me.IsOnTransport,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Moving out of transport"),
                                    new Action(ret => Navigator.PlayerMover.MoveTowards(GetOffLocation)),
                                    new Action(ret => StyxWoW.SleepForLagDuration()),
                                    new DecoratorContinue(
                                        ret => Me.IsOnTransport,
                                        new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(50)))))),
                            new Action(ret => _isBehaviorDone = true))),
                    new Decorator(
                        ret => Me.IsOnTransport && StandLocation != WoWPoint.Empty && !_usedTransport,
                        new PrioritySelector(
                            new Decorator(
                                ret => Me.Location.Distance2D(StandLocation) > 2,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Moving to stand location"),
                                    new Action(ret => Navigator.PlayerMover.MoveTowards(StandLocation)))),
                            new Sequence(
                                new Action(ret => _usedTransport = true),
                                new Action(ret => Navigator.PlayerMover.MoveStop()),
                                new Action(ret => TreeRoot.StatusText = "Waiting for the end location"))
                        )),
                    new Decorator(
                        ret => TransportLocation != WoWPoint.Empty && TransportLocation.Distance(StartLocation) < 2 && !_usedTransport,
                        new PrioritySelector(
                            new Decorator(
                                ret => Me.Location.Distance2D(TransportLocation) > 2,
                                new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Moving inside transport"),
                                    new Action(ret => Navigator.PlayerMover.MoveTowards(TransportLocation)),
                                    new Action(ret => StyxWoW.SleepForLagDuration()),
                                    new DecoratorContinue(
                                        ret => !Me.IsOnTransport,
                                        new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend, TimeSpan.FromMilliseconds(50)))))),
                            new Sequence(
                                new Action(ret => _usedTransport = true),
                                new Action(ret => Navigator.PlayerMover.MoveStop()),
                                new Action(ret => TreeRoot.StatusText = "Waiting for the end location"))))
                    ));
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get { return (_isBehaviorDone); }
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
                // The ConfigMemento() class captures the user's existing configuration.
                // After its captured, we can change the configuration however needed.
                // When the memento is dispose'd, the user's original configuration is restored.
                // More info about how the ConfigMemento applies to saving and restoring user configuration
                // can be found here...
                //     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_Saving_and_Restoring_User_Configuration
                _configMemento = new ConfigMemento();

                BotEvents.OnBotStop += BotEvents_OnBotStop;

                // Disable any settings that may cause distractions --
                // When we use transport, we don't want to be distracted by other things.
                // We also set PullDistance to its minimum value.
                // NOTE: these settings are restored to their normal values when the behavior completes
                // or the bot is stopped.
                CharacterSettings.Instance.HarvestHerbs = false;
                CharacterSettings.Instance.HarvestMinerals = false;
                CharacterSettings.Instance.LootChests = false;
                CharacterSettings.Instance.LootMobs = false;
                CharacterSettings.Instance.NinjaSkin = false;
                CharacterSettings.Instance.SkinMobs = false;
                CharacterSettings.Instance.PullDistance = 1;


                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " + ((!string.IsNullOrEmpty(DestName)) ? DestName :
                                                                  (quest != null) ? ("\"" + quest.Name + "\"") :
                                                                  "In Progress");
            }
        }


        #endregion
    }
}

