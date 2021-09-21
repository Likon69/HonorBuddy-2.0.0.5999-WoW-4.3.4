// ReSharper disable CheckNamespace
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace Styx.Bot.Quest_Behaviors
// ReSharper restore CheckNamespace
{
    public class DefendObject : CustomForcedBehavior
    {
        private WoWPoint _location;
        private uint[] _objectId;
        private int _questId;
        private QuestCompleteRequirement _questRequirementComplete;
        private QuestInLogRequirement _questRequirementInLog;
        private readonly LocalPlayer _me = ObjectManager.Me;
        private int _maxRange;
        private WoWUnit _defendObject;


        private List<WoWUnit> _enemyUnits = new List<WoWUnit>();
        private readonly Styx.Helpers.WaitTimer _enemyListTimer = Styx.Helpers.WaitTimer.FiveSeconds;

        /// <summary>
        /// Defends an Object.
        /// </summary>
        /// <remarks>
        /// Created 12/8/2010.
        /// </remarks>
        /// <param name="args">A variable-length parameters list containing arguments.</param>
        public DefendObject(Dictionary<string, string> args) : base(args)
        {
            try
            {
                _location = GetAttributeAsNullable("", false, ConstrainAs.WoWPointNonEmpty, null) ?? _me.Location;
                _objectId = GetNumberedAttributesAsArray<uint>("ObjectId", 1, null, new[] {"NpcId", "MobId"}) ?? new uint[] { 27430 };
                _questId = GetAttributeAsNullable("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                _questRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                _questRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                _maxRange = GetAttributeAsNullable<int>("MaxRange", false, null, null) ?? 40;
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

        Composite _root;
        protected override Composite CreateBehavior()
        {

            return _root ?? (_root = new Decorator(ret => _me.IsAlive && !IsDone,
                new PrioritySelector(
                    new Decorator(ret => !_me.Combat,
                        new PrioritySelector(
                            new Decorator(ret => _defendObject == null,
                                new Sequence(
                                    new Action(o => TreeRoot.StatusText = "Moving to defend location ..."),
                                    new Action(o => Navigator.MoveTo(_location)),
                                    new Action(o => PopulateList()))),
                            new Decorator(ret => _defendObject.Distance > _maxRange,
                                new Sequence(
                                    new Action(o => TreeRoot.StatusText = "Too far away from Defendant"),
                                    new Action(o => Navigator.MoveTo(_defendObject.Location)),
                                    new Action(o => TreeRoot.StatusText = "Standing guard...."))),

                    new Decorator(ret => _enemyUnits.Count > 0,
                        new PrioritySelector(
                            new Decorator(ret => _me.CurrentTarget == null || !_enemyUnits.Contains(_me.CurrentTarget),
                                new Action(o => _enemyUnits.FirstOrDefault().Target())),
                            new Decorator(ret => _me.CurrentTarget != null && _enemyUnits.Contains(_me.CurrentTarget),
                                new PrioritySelector(
                                    new Decorator(ret => RoutineManager.Current.PullBehavior != null,
                                        RoutineManager.Current.PullBehavior),
                                    new Decorator(ret => RoutineManager.Current.PullBehavior == null,
                                        new Action(o => RoutineManager.Current.Pull()))))))
                            
                            
                            )))));
        }


        public override void Dispose()
        {
            
        }


        private bool _isDone = false;
        public override bool IsDone
        {
            get
            {
                return _isDone || !UtilIsProgressRequirementsMet(_questId, _questRequirementInLog, _questRequirementComplete);
            }
        }

        public override void OnStart()
        {
            
        }

        private void PopulateList()
        {
            _enemyUnits =
                ObjectManager.GetObjectsOfType<WoWUnit>().Where(
                    o => o.CurrentTarget == _defendObject).OrderBy(
                        o => o.Location.Distance(_defendObject.Location)) as List<WoWUnit>;
          
            if (_me.Location.Distance(_location) <= 30)
            {
                FindObject();
                if (_defendObject == null)
                    _isDone = true;
            }  
            Logging.WriteDebug(Color.LimeGreen, "DefendObject: PopulateList()!");
        }

        public override void OnTick()
        {
            if (_enemyListTimer.IsFinished)
            {
                if (_defendObject != null)
                {
                    PopulateList();
                }
                else
                {
                    try
                    {
                        FindObject();
                        Logging.WriteDebug(Color.LightSalmon, "DefendObject: Attempting to find Defendant...");
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteDebug(Color.LimeGreen,
                                           "DefendObject: TickException: " + ex.Message + " Trace: " + ex.StackTrace);
                    }
                }
            }
            _enemyListTimer.Reset();
        }

        private void FindObject()
        {
            _defendObject =
                ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == _objectId.First()).FirstOrDefault();
        }
    }
}
