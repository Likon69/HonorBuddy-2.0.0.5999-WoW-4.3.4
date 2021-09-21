// Behavior originally contributed by Bobby53.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors.MountHyjal
{
    public class RunLikeHell : CustomForcedBehavior
    {
        /// <summary>
        /// Allows you to Run following a specific path.  Supports options to prevent combat 
        /// (disables CC while running), use Click-To-Move instead of Navigator, and
        /// the ability to specify a mob that when it enters the specified range, causes
        /// you to move to the nexts point.  
        /// 
        /// A few key difference between this and having several RunTo/NoCombatMoveTo in sequence:
        /// - HonorBuddy allows CC control between lines if attacked; RunLikeHell does not
        /// - Syntax allows easy switch from ClickToMove() to Navigator.MoveTo() when a mesh is updated
        /// - On startup, RunLikeHell finds the closest point in the path and starts there
        /// - If Combat=true, will only fight if aggro picked up while running.. will not Pull
        /// 
        /// If QuestId is non-zero, behavior will stop when quest becomes complete even if 
        /// it has not completed NumOfTimes iterations of full path specified
        /// 
        /// You can control the movement with the options below. 
        /// 
        /// ##Syntax##
        /// [Optional] QuestId: Id of the quest (default is 0)
        /// [Optional] WaitTime: ms to pause at each point (default is 0)
        /// [Optional] MobId: wait at point until mob is within Range yds (default is to move immediately)
        /// [Optional] Range: when mob is within this distance, move to next point (default is 15)
        /// [Optional] NumOfTimes: number of times to run path (default is 1)
        /// [Optional] Combat: fight back if attacked (default is true, false you keep moving)
        /// [Optional] UseCTM: use ClickToMove if true, otherwise Navigator (default is false)
        /// [Required] <Hotspot X="" Y="" Z="" /> : child elements specifying path to run
        /// 
        /// ##Examples##
        /// following will take path one time as listed
        /// <CustomBehavior File="RunLikeHell" >
        ///     <Hotspot X="4554.003" Y="-4718.743" Z="883.0464" />
        ///     <Hotspot X="4578.725" Y="-4721.257" Z="882.8724" />
        ///     <Hotspot X="4584.166" Y="-4693.487" Z="882.7331" />
        /// </CustomBehavior>
        /// 
        /// following path up to 4 times and moves to next spot only 
        /// if the mob #40434 is within 10 yds
        /// <CustomBehavior File="RunLikeHell" NumOfTimes="4" MobId="40434" Range="10">
        ///     <Hotspot X="4554.003" Y="-4718.743" Z="883.0464" />
        ///     <Hotspot X="4578.725" Y="-4721.257" Z="882.8724" />
        ///     <Hotspot X="4584.166" Y="-4693.487" Z="882.7331" />
        /// </CustomBehavior>
        /// 
        /// following follows path up to 4 times and moves to next spot only 
        /// if the mob #40434 is within 10 yds.  stops at 4 loops or when quest complete
        /// <CustomBehavior File="RunLikeHell" QuestId="25499" NumOfTimes="4" MobId="40434" Range="10">
        ///     <Hotspot X="4554.003" Y="-4718.743" Z="883.0464" />
        ///     <Hotspot X="4578.725" Y="-4721.257" Z="882.8724" />
        ///     <Hotspot X="4584.166" Y="-4693.487" Z="882.7331" />
        /// </CustomBehavior>
        /// 
        /// </summary>
        /// 
        public RunLikeHell(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                AllowCombat = GetAttributeAsNullable<bool>("AllowCombat", false, null, new[] { "Combat" }) ?? true;
                MobId = GetAttributeAsNullable<int>("MobId", false, ConstrainAs.MobId, new[] { "NpcId" }) ?? 0;
                NumOfTimes = GetAttributeAsNullable<int>("NumOfTimes", false, ConstrainAs.RepeatCount, null) ?? 1;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                Range = GetAttributeAsNullable<double>("Range", false, ConstrainAs.Range, null) ?? 15;
                UseCTM = GetAttributeAsNullable<bool>("UseCTM", false, null, null) ?? false;
                WaitTime = GetAttributeAsNullable<int>("WaitTime", false, ConstrainAs.Milliseconds, null) ?? 0;

                _lastStateReturn = RunStatus.Success;
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
        public bool AllowCombat { get; private set; }
        public int MobId { get; private set; }
        public int NumOfTimes { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public double Range { get; private set; }
        public bool UseCTM { get; private set; }
        public int WaitTime { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private RunStatus _lastStateReturn { get; set; }
        private Composite _root;

        // Private properties
        private int Counter { get; set; }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }
        private WoWUnit Mob
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>()
                                       .Where(u => u.Entry == MobId && !u.Dead)
                                       .OrderBy(u => u.Distance).FirstOrDefault();
            }
        }
        private Queue<WoWPoint> Path { get; set; }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: RunLikeHell.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~RunLikeHell()
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
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        private bool ParsePath()
        {
            var path = new Queue<WoWPoint>();

            foreach (WoWPoint point in ParseWoWPoints(Element.Elements().Where(elem => elem.Name == "Hotspot")))
                path.Enqueue(point);

            Path = path;
            return true;
        }


        public IEnumerable<WoWPoint> ParseWoWPoints(IEnumerable<XElement> elements)
        {
            var temp = new List<WoWPoint>();

            foreach (XElement element in elements)
            {
                XAttribute xAttribute, yAttribute, zAttribute;
                xAttribute = element.Attribute("X");
                yAttribute = element.Attribute("Y");
                zAttribute = element.Attribute("Z");

                float x, y, z;
                float.TryParse(xAttribute.Value, out x);
                float.TryParse(yAttribute.Value, out y);
                float.TryParse(zAttribute.Value, out z);
                temp.Add(new WoWPoint(x, y, z));
            }

            return temp;
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new Decorator(ret => !IsDone && (!AllowCombat || !Me.Combat),
                    new PrioritySelector(
                        new Decorator(ret => !Path.Any() && Counter >= NumOfTimes,
                            new Action(delegate
                            {
                                _isBehaviorDone = true;
                            })),
                        new Decorator(ret => !Path.Any(),
                            new Action(delegate
                            {
                                Counter++;
                                ParsePath();
                            })),
                        new Decorator(ret => Path.Peek().Distance(Me.Location) <= Navigator.PathPrecision,
                            new PrioritySelector(
                                new Decorator(ret => Me.IsMoving && WaitTime > 0,
                                    new Action(delegate
                                    {
                                        WoWMovement.MoveStop();
                                        TreeRoot.GoalText = "RunLikeHell pausing " + WaitTime + " ms";
                                        System.Threading.Thread.Sleep(WaitTime);
                                    })),
                                new Decorator(ret => MobId != 0 && Mob.Distance > Range,
                                    new Action(delegate
                                    {
                                        TreeRoot.GoalText = "RunLikeHell wait for " + Mob.Name + " within " + Range + " yds";
                                    })),
                                new Action(delegate
                                {
                                    Path.Dequeue();
                                }))
                            ),
                        new Action(delegate
                        {
                            if (NumOfTimes > 1)
                                TreeRoot.GoalText = "RunLikeHell[Lap " + Counter + "] to " + Path.Peek().ToString();
                            else
                                TreeRoot.GoalText = "RunLikeHell to " + Path.Peek().ToString();

                            if (UseCTM)
                                WoWMovement.ClickToMove(Path.Peek());
                            else
                                Navigator.MoveTo(Path.Peek());

                            return _lastStateReturn;
                        })
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
                bool result = (_isBehaviorDone                                 // normal completion
                                   || (((Path != null) && !Path.Any()) && Counter >= NumOfTimes)    // no hotspots left and all iterations complete
                                   || Me.Dead || Me.IsGhost                        // i'm a ghost
                                   || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));

                if (result)
                    _isBehaviorDone = true;

                return result;
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
                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                ParsePath();        // refresh the list of points

                // find the closest point in path
                WoWPoint closePt = Path.Peek();
                double minDist = Me.Location.Distance(closePt);

                foreach (WoWPoint pt in Path)
                {
                    if (Me.Location.Distance(pt) < minDist)
                    {
                        minDist = Me.Location.Distance(pt);
                        closePt = pt;
                    }
                }

                // set closest point as current
                while (Path.Any() && closePt != Path.Peek())
                {
                    Path.Dequeue();
                }

                Counter = 1;

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");

                if (TreeRoot.Current == null)
                    LogMessage("fatal", "TreeRoot.Current == null");
                else if (TreeRoot.Current.Root == null)
                    LogMessage("fatal", "TreeRoot.Current.Root == null");
                else if (TreeRoot.Current.Root.LastStatus == RunStatus.Running)
                    LogMessage("fatal", "TreeRoot.Current.Root.LastStatus == RunStatus.Running");
                else
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (!(currentRoot is GroupComposite))
                        LogMessage("fatal", "!(currentRoot is GroupComposite)");
                    else
                    {
                        if (currentRoot is Sequence)
                            _lastStateReturn = RunStatus.Failure;
                        else if (currentRoot is PrioritySelector)
                            _lastStateReturn = RunStatus.Success;
                        else
                        {
                            LogMessage("debug", "Unknown type of Group Composite at root");
                            _lastStateReturn = RunStatus.Success;
                        }

                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                        LogMessage("debug", "Disabled Combat");
                    }
                }
            }
        }

        #endregion
    }
}
