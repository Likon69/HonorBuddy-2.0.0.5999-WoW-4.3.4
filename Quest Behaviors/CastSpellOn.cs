// Behavior originally contributed by Natfoth.
//
// WIKI DOCUMENTATION:
//     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Custom_Behavior:_CastSpellOn
//
// QUICK DOX:
//      Allows you to cast a specific spell on a target.  Useful for training dummies and starting quests.
//
//  Parameters (required, then optional--both listed alphabetically):
//      MobId: Id of the target (NPC or Object) on which the spell should be cast.
//      SpellId: Spell that should be used to cast on the target
//      X,Y,Z: The general location where the targets can be found.
//
//      HpLeftAmount [Default:110 hitpoints]: How low the hitpoints on the target should be before attempting
//              to cast a spell on the target.   Note this is an absolute value, and not a percentage.
//      MinRange [Default:3 yards]: minimum distance from the target at which a cast attempt shoudl be made.
//      NumOfTimes [Default:1]: The number of times to perform th casting action on viable targets in the area.
//      QuestId [Default:none]:
//      QuestCompleteRequirement [Default:NotComplete]:
//      QuestInLogRequirement [Default:InLog]:
//              A full discussion of how the Quest* attributes operate is described in
//              http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
//      Range [Default:25 yards]: maximum distance from the target at which a cast attempt should be made.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    public class CastSpellOn : CustomForcedBehavior
    {
        public CastSpellOn(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                Location = GetAttributeAsNullable<WoWPoint>("", false, ConstrainAs.WoWPointNonEmpty, null) ?? Me.Location;
                MinRange = GetAttributeAsNullable<double>("MinRange", false, ConstrainAs.Range, null) ?? 3;
                MobHpPercentLeft = GetAttributeAsNullable<double>("MobHpPercentLeft", false, ConstrainAs.Percent, new[] { "HpLeftAmount" }) ?? 110;
                MobIds = GetNumberedAttributesAsArray<int>("MobId", 1, ConstrainAs.Milliseconds, new[] { "NpcId" });
                NumOfTimes = GetAttributeAsNullable<int>("NumOfTimes", false, ConstrainAs.RepeatCount, null) ?? 1;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                Range = GetAttributeAsNullable<double>("Range", false, ConstrainAs.Range, null) ?? 25;
                SpellIds = GetNumberedAttributesAsArray<int>("SpellId", 1, ConstrainAs.SpellId, null);
                //SpellId = GetAttributeAsNullable<int>("SpellId", false, ConstrainAs.SpellId, null) ?? 0;
                SpellName = GetAttributeAs<string>("SpellName", false, ConstrainAs.StringNonEmpty, new[] { "spellname" }) ?? "";

                Counter = 0;

                // Semantic checks
                if (Range <= MinRange)
                {
                    LogMessage("fatal", "\"Range\" attribute must be greater than \"MinRange\" attribute.");
                    IsAttributeProblem = true;
                }

                SpellId = SpellIds.FirstOrDefault(id => SpellManager.HasSpell(id));

                CastSelf = false;

                if (MobIds.FirstOrDefault() == 0)
                    CastSelf = true;
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
        public WoWPoint Location { get; private set; }
        public double MinRange { get; private set; }
        public double MobHpPercentLeft { get; private set; }
        public int[] MobIds { get; private set; }
        public int NumOfTimes { get; private set; }
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public double Range { get; private set; }
        public bool CastSelf { get; private set; }
        public int[] SpellIds { get; private set; }
        public int SpellId { get; private set; }
        public string SpellName { get; private set; }

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private int Counter { get; set; }
        private LocalPlayer Me { get { return (StyxWoW.Me); } }
        public List<WoWUnit> MobList
        {
            get
            {
                if (MobHpPercentLeft > 0)
                {
                    return (ObjectManager.GetObjectsOfType<WoWUnit>()
                                         .Where(u => MobIds.Contains((int)u.Entry) && !u.Dead && u.HealthPercent <= MobHpPercentLeft)
                                         .OrderBy(u => u.Distance).ToList());
                }
                else
                {
                    return (ObjectManager.GetObjectsOfType<WoWUnit>()
                                         .Where(u => MobIds.Contains((int)u.Entry) && !u.Dead)
                                         .OrderBy(u => u.Distance).ToList());
                }
            }
        }

        public WoWSpell CurrentBehaviorSpell
        {
            get
            {
                return WoWSpell.FromId(SpellId);
            }
        }

        public float maxSpellRange
        {
            get
            {
                if (CurrentBehaviorSpell.MaxRange == 0)
                    return 4;

                return CurrentBehaviorSpell.MaxRange;
            }
        }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: CastSpellOn.cs 240 2012-08-23 21:20:10Z natfoth $"); } }
        public override string SubversionRevision { get { return ("$Revision: 240 $"); } }


        ~CastSpellOn()
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


        Composite CreateSpellBehavior
        {
            get
            {
                return new Action(c =>
                {
                    if (SpellId > 0)
                    {

                        if (!CastSelf)
                        {
                            MobList[0].Target();
                            MobList[0].Face();
                        }
                        Thread.Sleep(300);
                        SpellManager.Cast(SpellId);

                        if (Me.QuestLog.GetQuestById((uint)QuestId) == null || QuestId == 0)
                        {
                            Counter++;
                        }
                        Thread.Sleep(300);
                        return RunStatus.Success;
                    }
                    else
                    {
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }
                });
            }
        }


        protected override Composite CreateBehavior()
        {
            return new PrioritySelector(

                new Decorator(ret => !IsDone && !_isBehaviorDone && Me.IsAlive && (Counter >= NumOfTimes || (Me.QuestLog.GetQuestById((uint)QuestId) != null && Me.QuestLog.GetQuestById((uint)QuestId).IsCompleted)),
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Finished CastSpellOn!"),
                        new Action(ret => _isBehaviorDone = true),
                        new Action(ret => RunStatus.Success))),

                        new Decorator(ret => MobList.Count == 0,
                            new Sequence(
                                    new Action(ret => TreeRoot.StatusText = "Moving To Location - X: " + Location.X + " Y: " + Location.Y),
                                    new Action(ret => Navigator.MoveTo(Location)),
                                    new Action(ret => Thread.Sleep(300))
                                )
                            ),

                        new Decorator(ret => CastSelf,
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Casting Spell - " + SpellId + " On Mob: Myself"),
                                        new Action(ret => WoWMovement.MoveStop()),
                                        new Action(ret => Thread.Sleep(300)),
                                        CreateSpellBehavior
                                        )
                                ),

                        new Decorator(ret => MobList.Count > 0 && !Me.IsCasting && SpellManager.CanCast(SpellId),
                            new Sequence(
                                   new DecoratorContinue(ret => MobList.Count > 0 && MobList[0].Location.Distance(Me.Location) >= maxSpellRange || !MobList[0].InLineOfSpellSight,
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Moving To Mob - " + MobList[0].Name + " Yards Away: " + MobList[0].Location.Distance(Me.Location)),
                                        new Action(ret => Navigator.MoveTo(MobList[0].Location))
                                        )
                                ),
                                new DecoratorContinue(ret => MobList.Count > 0 && !Me.IsCasting && SpellManager.CanCast(SpellId, MobList[0]),
                                    new Sequence(
                                           new DecoratorContinue(ret => MobList[0].Location.Distance(Me.Location) >= maxSpellRange || !MobList[0].InLineOfSpellSight,
                                            new Sequence(
                                                new Action(ret => TreeRoot.StatusText = "Moving To Mob - " + MobList[0].Name + " Yards Away: " + MobList[0].Location.Distance(Me.Location)),
                                                new Action(ret => Navigator.MoveTo(MobList[0].Location))
                                                )
                                        ))),
                                new DecoratorContinue(ret => MobList.Count > 0 && MobList[0].Location.Distance(Me.Location) < CurrentBehaviorSpell.MinRange,
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Too Close, Backing Up"),
                                        new Action(ret => MobList[0].Face()),
                                        new Action(ret => Thread.Sleep(100)),
                                        new Action(ret => WoWMovement.Move(WoWMovement.MovementDirection.Backwards)),
                                        new Action(ret => Thread.Sleep(2000)),
                                        new Action(ret => WoWMovement.MoveStop(WoWMovement.MovementDirection.Backwards))
                                        )),
                                new DecoratorContinue(ret => MobList.Count > 0 && MobList[0].Location.Distance(Me.Location) >= CurrentBehaviorSpell.MinRange && MobList[0].Location.Distance(Me.Location) <= maxSpellRange && MobList[0].InLineOfSpellSight,
                                    new Sequence(
                                        new Action(ret => TreeRoot.StatusText = "Casting Spell - " + SpellId + " On Mob: " + MobList[0].Name + " Yards Away " + MobList[0].Location.Distance(Me.Location)),
                                        new Action(ret => WoWMovement.MoveStop()),
                                        new Action(ret => Thread.Sleep(300)),
                                        CreateSpellBehavior
                                        )
                                )
                                )),
                        //Fix for Charge and other Spells which needs a target
                        new Decorator(ret => MobList.Count > 0 && MobList[0].Location.Distance(Me.Location) <= 40 && MobList[0].InLineOfSpellSight,
                            new Sequence(
                                new Action(ret => TreeRoot.StatusText = "Targetting On Mob: " + MobList[0].Name + " Yards Away " + MobList[0].Location.Distance(Me.Location)),
                                new Action(ret => WoWMovement.MoveStop()),
                                new Action(ret => MobList[0].Target()),
                                new Action(ret => Thread.Sleep(300)),
                                CreateSpellBehavior
                                )
                        )
                );
        }


        #region Overrides of CustomForcedBehavior


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
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                // Semantic coherency...
                // We had to defer this check (from the constructor) until after OnStart() was called.
                // If this behavior was used as a consequence of a class-specific action, or a quest
                // that has already been completed, then having this check in the constructor yields
                // confusing (and wrong) error messages.  Thus, we needed to defer the check until
                // we actually tried to _use_ the behavior--not just create it.
                if (!SpellManager.HasSpell(SpellId))
                {
                    WoWSpell spell = WoWSpell.FromId(SpellId);

                    LogMessage("fatal", "Toon doesn't know SpellId({0}, \"{1}\")",
                                        SpellId,
                                        ((spell != null) ? spell.Name : "unknown"));
                    _isBehaviorDone = true;
                    return;
                }

                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = this.GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
            }
        }

        #endregion
    }
}
