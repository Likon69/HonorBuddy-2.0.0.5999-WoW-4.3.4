// Behavior originally contributed by HighVoltz.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using System.Linq;

using Styx.Helpers;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Common;
using Styx.Logic.Inventory;
using Styx.Logic.Pathing;
using Styx.Logic.Questing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;
using Action = TreeSharp.Action;


namespace Styx.Bot.Quest_Behaviors
{
    /// <summary>
    /// Into the Realm of Shadows]
    /// ##Syntax##
    /// X,Y,Z: The location where you want to move to
    /// </summary>
    public class IntotheRealmofShadows : CustomForcedBehavior
    {
        public IntotheRealmofShadows(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                Location = GetAttributeAsNullable<WoWPoint>("", true, ConstrainAs.WoWPointNonEmpty, null) ?? WoWPoint.Empty;
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

        // Private variables for internal state
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;

        // Private properties
        private bool IsInVehicle { get { return Lua.GetReturnVal<int>("return UnitIsControlling('player')", 0) == 1; } }
        private LocalPlayer Me { get { return (ObjectManager.Me); } }

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: IntotheRealmofShadows.cs 217 2012-02-11 16:52:02Z Nesox $"); } }
        public override string SubversionRevision { get { return ("$Revision: 217 $"); } }


        ~IntotheRealmofShadows()
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


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root =
                new PrioritySelector(
                    new Action(c =>
                    {
                        if (Me.HealthPercent < 2)
                        {
                            return RunStatus.Failure;
                        }
                        if (Me.HealthPercent < 60 && !Me.IsActuallyInCombat)
                        {
                            WoWItem food = Consumable.GetBestFood(true);
                            CharacterSettings.Instance.FoodName = food != null ? food.Name : string.Empty;
                            Rest.Feed();
                            return RunStatus.Running;
                        }
                        if (IsInVehicle)
                        {
                            _isBehaviorDone = true;
                            return RunStatus.Success;
                        }
                        WoWUnit Horse = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 28782).OrderBy(u => u.Distance).FirstOrDefault();
                        if (Horse != null)
                        {
                            if (!Me.IsActuallyInCombat)
                            {
                                if (Horse.Distance > 4)
                                    Navigator.MoveTo(Horse.Location);
                                else
                                    Horse.Interact();
                            }
                            if (Me.IsActuallyInCombat)
                            {
                                if (Me.CurrentTarget != null)
                                {
                                    if (Me.CurrentTarget.Dead)
                                    {
                                        Me.ClearTarget();
                                    }
                                    else if (Me.CurrentTarget.Entry == 28768)
                                    {
                                        if (!Me.IsSafelyFacing(Horse))
                                            Horse.Face();
                                    }
                                    else if (!Me.IsSafelyFacing(Me.CurrentTarget))
                                        Me.CurrentTarget.Face();
                                    if (Me.IsMoving)
                                    {
                                        WoWMovement.MoveStop();
                                    }
                                    if (!Me.IsSafelyFacing(Me.CurrentTarget))
                                        Me.CurrentTarget.Face();
                                    if (SpellManager.CanCast("Icy Touch"))
                                        SpellManager.Cast("Icy Touch");
                                    if (SpellManager.CanCast("Plague Strike"))
                                        SpellManager.Cast("Plague Strike");
                                    if (SpellManager.CanCast("Blood Strike"))
                                        SpellManager.Cast("Blood Strike");
                                    if (SpellManager.CanCast("Death Coil"))
                                        SpellManager.Cast("Death Coil");
                                }
                            }
                        }
                        else
                            Navigator.MoveTo(Location);

                        return RunStatus.Running;
                    }

                )));
        }


        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public override bool IsDone
        {
            get { return _isBehaviorDone; }
        }


        public override void OnStart()
        {            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                TreeRoot.GoalText = this.GetType().Name + ": In Progress";
            }
        }

        #endregion
    }
}
