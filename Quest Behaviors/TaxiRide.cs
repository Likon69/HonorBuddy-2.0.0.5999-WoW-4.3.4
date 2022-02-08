// Behavior originally contributed by Cava
// Part of this code obtained from HB QB's and UseTaxi.cs originaly contributed by Vlad
// LICENSE:
// This work is licensed under the
//     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// also known as CC-BY-NC-SA.  To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/3.0/
// or send a letter to
//      Creative Commons // 171 Second Street, Suite 300 // San Francisco, California, 94105, USA.

#region Summary and Documentation
// QUICK DOX:
// CavaTaxiRide interact with Flighter masters to pick a fly or get a destiny list names.
//
// BEHAVIOR ATTRIBUTES:
//
// QuestId: (Optional) - associates a quest with this behavior. 
// QuestCompleteRequirement [Default:NotComplete]:
// QuestInLogRequirement [Default:InLog]:
//	If the quest is complete or not in the quest log, this Behavior will not be executed.
//	A full discussion of how the Quest* attributes operate is described in
//      http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
//
// MobId: (Required) - Id of the Flight Master to use
// NpcState [optional; Default: DontCare]
//	[Allowed values: Alive, Dead, DontCare]
//          This represents the state the NPC must be in when searching for targets
//          with which we can interact.
//
// TaxiNumber: (Optional)- Specifies the Number of the Flight Path on the TaxiMap
// DestName: (Optional) [Default: ViewNodesOnly] - Specifies the destination NAME of the node on the TaxiMap. 
//	If bouth TaxiNumber and DestName are omitted bot will use default ViewNodesOnly, and only give an outputlist of nodes (number name)
//	The TaxiNumber its a number and have prio over the Destname (if bouth are give, bot will only use the TaxiNumber
//	The DestName should be a name string in the list of your TaxiMap node names. The argument is CASE SENSITIVE!
//
// WaitTime [optional; Default: 1500ms]
//	Defines the number of milliseconds to wait after the interaction is successfully
//	conducted before carrying on with the behavior on other mobs.
//
//
// THINGS TO KNOW:
//
// The idea of this Behavior is use the FPs, its not intended to move to near Flight master,
// its always a good idea move bot near the MobId before start this behavior
// If char doesnt know the Destiny flight node name, the will not fly, its always good idea add an RunTo (Destiny XYZ) after use this behavior
// Likethis (RunTo Near MobId) -> (use Behavior) -> (RunTo Destiny XYZ)
//
// You can use signal & inside DestName, but becomes &amp; like Fizzle &amp; Pozzik
// Cant use the signal ' inside DestName, when nodes have that like Fizzle & Pozzik's Speedbarge, Thousand Needles
// you can use the part before or after the signal ' like:
// DestName="Fizzle &amp; Pozzik" or DestName="s Speedbarge, Thousand Needles"
//
// EXAMPLES:
// <CustomBehavior File="TaxiRide" MobId="12345" NpcState="Alive" TaxiNumber="2" />
// <CustomBehavior File="TaxiRide" MobId="12345" DestName="Orgrimmar" WaitTime="1000" />
// <CustomBehavior File="TaxiRide" MobId="12345" DestName="ViewNodesOnly" />
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Styx;
using Styx.Helpers;
using Styx.Logic.Inventory.Frames.Taxi;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
#endregion

namespace Styx.Bot.Quest_Behaviors.TaxiRide
{
    public class TaxiRide : CustomForcedBehavior
    {
	#region Constructor and argument processing  
        public enum NpcStateType
        {
            Alive,
            Dead,
            DontCare,
        }

        public enum NpcCommand
        {
            Target,
        }
        
        public TaxiRide(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.

                QuestId = GetAttributeAsNullable("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;

                MobIds = GetNumberedAttributesAsArray<int>("MobId", 1, ConstrainAs.MobId, new[] { "NpcId" });
                NpcState = GetAttributeAsNullable<NpcStateType>("MobState", false, null, new[] { "NpcState" }) ?? NpcStateType.Alive;
                CurrentCommand = GetAttributeAsNullable<NpcCommand>("MobCommand", false, null, new[] { "NpcCommand" }) ?? NpcCommand.Target;
                WaitTime = GetAttributeAsNullable<int>("WaitTime", false, ConstrainAs.Milliseconds, null) ?? 1500;
                WaitForNpcs = GetAttributeAsNullable<bool>("WaitForNpcs", false, null, null) ?? false;
                MobHpPercentLeft = GetAttributeAsNullable<double>("MobHpPercentLeft", false, ConstrainAs.Percent, new[] { "HpLeftAmount" }) ?? 100.0;
                CollectionDistance = GetAttributeAsNullable<double>("CollectionDistance", false, ConstrainAs.Range, null) ?? 100.0;
                TaxiNumber = GetAttributeAs<string>("TaxiNumber", false, ConstrainAs.StringNonEmpty, null) ?? "0";
		        DestName = GetAttributeAs<string>("DestName", false, ConstrainAs.StringNonEmpty, null) ?? "ViewNodesOnly";
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
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public double CollectionDistance { get; private set; }
        public int[] MobIds { get; private set; }
        public NpcStateType NpcState { get; private set; }
        public NpcCommand CurrentCommand { get; private set; }
        public bool WaitForNpcs { get; private set; }
        public int WaitTime { get; private set; }
        public double MobHpPercentLeft { get; private set; }
        public string TaxiNumber { get; private set; }
	public string DestName { get; set; }
	#endregion


    #region Private and Convenience variables
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;
        private LocalPlayer Me { get { return (StyxWoW.Me); } }
        private readonly List<ulong> _npcBlacklist = new List<ulong>();

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: TaxiRide.cs 525 2013-05-22 21:28:47Z chinajade $"); } }
        public override string SubversionRevision { get { return ("$Revision: 525 $"); } }
	#endregion

        ~TaxiRide()
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

        private WoWUnit CurrentNPC
        {
            get
            {
                WoWUnit @object = null;

                var baseTargets = ObjectManager.GetObjectsOfType<WoWUnit>()
                        .OrderBy(target => target.Distance)
                        .Where(target => !_npcBlacklist.Contains(target.Guid) && !BehaviorBlacklist.Contains(target.Guid)
                        && (target.Distance < CollectionDistance)
                        && MobIds.Contains((int)target.Entry));

                var npcStateQualifiedTargets = baseTargets
                        .Where(target => ((NpcState == NpcStateType.DontCare)
                        || ((NpcState == NpcStateType.Dead) && target.Dead)
                        || ((NpcState == NpcStateType.Alive) && target.IsAlive)));
                        @object = npcStateQualifiedTargets.FirstOrDefault();

                if (@object != null)
                { LogMessage("debug", @object.Name); }

                return @object;
            }
        }

        class BehaviorBlacklist
        {
            static readonly Dictionary<ulong, BlacklistTime> SpellBlacklistDict = new Dictionary<ulong, BlacklistTime>();
            private BehaviorBlacklist()
            {
            }

            class BlacklistTime
            {
                public BlacklistTime(DateTime time, TimeSpan span)
                {
                    TimeStamp = time;
                    Duration = span;
                }
                public DateTime TimeStamp { get; private set; }
                public TimeSpan Duration { get; private set; }
            }

            static public bool Contains(ulong id)
            {
                RemoveIfExpired(id);
                return SpellBlacklistDict.ContainsKey(id);
            }

            static public void Add(ulong id, TimeSpan duration)
            {
                SpellBlacklistDict[id] = new BlacklistTime(DateTime.Now, duration);
            }

            static void RemoveIfExpired(ulong id)
            {
                if (SpellBlacklistDict.ContainsKey(id) &&
                    SpellBlacklistDict[id].TimeStamp + SpellBlacklistDict[id].Duration <= DateTime.Now)
                {
                    SpellBlacklistDict.Remove(id);
                }
            }
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(

                new Decorator(ret => TaxiNumber == "0" && DestName == "ViewNodesOnly" && CurrentCommand == NpcCommand.Target && CurrentNPC != null,
                    new Action(context =>
	                {
                        TreeRoot.StatusText = "Targeting Npc: " + CurrentNPC.Name + " Distance: " + CurrentNPC.Location.Distance(Me.Location) + " to listing known TaxiNodes";
                        CurrentNPC.Target();
                        CurrentNPC.Interact();
                        Thread.Sleep(WaitTime);
                        Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/run for i=1,NumTaxiNodes() do a=TaxiNodeName(i); print(i,a);end;"));
                        _isBehaviorDone = true;
                })),

                new Decorator(ret => TaxiNumber != "0" && CurrentCommand == NpcCommand.Target && CurrentNPC != null,
                    new Action(context =>
	                {
                        TreeRoot.StatusText = "Targeting Npc: " + CurrentNPC.Name + " Distance: " + CurrentNPC.Location.Distance(Me.Location);
                        CurrentNPC.Target();
                        CurrentNPC.Interact();
                        Thread.Sleep(WaitTime);
                        Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/click TaxiButton" + TaxiNumber));
                        _isBehaviorDone = true;
                })),
              
                new Decorator(ret => DestName != "ViewNodesOnly" && CurrentCommand == NpcCommand.Target && CurrentNPC != null,
                    new Sequence(
                        new Action(ret => TreeRoot.StatusText = "Taking a ride to: " + DestName ),
                        new Action(delegate
                            {
                                Styx.Logic.Inventory.Frames.Taxi.TaxiFrame TaxiMap = new Styx.Logic.Inventory.Frames.Taxi.TaxiFrame();
                                while (!TaxiMap.IsVisible)
								{
								    CurrentNPC.Interact();
									Thread.Sleep(3000);
								}
								if (!Me.OnTaxi)
								{
                                    Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/run for i=1,NumTaxiNodes() do a=TaxiNodeName(i); if strmatch(a,'" + DestName + "')then b=i; TakeTaxiNode(b); end end"));
                                    Thread.Sleep(2000);
								}
                        }),
                        new Action(ret => _isBehaviorDone = true)
                )),   

                new Decorator(ret => CurrentNPC == null,
                    new Action(ret => TreeRoot.StatusText = "Waiting for Npc to spawn")
            )));
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
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                TreeRoot.GoalText = "TaxiRide Started";
            }
        }

        #endregion
    }
}

