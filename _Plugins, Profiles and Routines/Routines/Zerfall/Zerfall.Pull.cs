using System;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using TreeSharp;
using System.Threading;
using Styx.Logic.Pathing;
using Action = TreeSharp.Action;
using Sequence = TreeSharp.Sequence;
using CommonBehaviors.Actions;

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite _pullBehavior;
        public override Composite PullBehavior
        {
            get
            {
                if (_pullBehavior == null)
                {
                    Log("Creating 'Pull' behavior");
                    _pullBehavior = CreatePullBehavior();
                }
                _pullBehavior = CreatePullBehavior();
                return _pullBehavior;
            }
        }

        private static readonly WaitTimer PullTimer = WaitTimer.TenSeconds;
        /// <summary>
        /// Creates the behavior used for pulling mobs, (approach, attack)
        /// </summary>
        /// <returns></returns>
        private PrioritySelector CreatePullBehavior()
        {
            return new PrioritySelector(

                //Added to make sure we have a target when movement is disabled before spamming my pull sqeuence
                new Decorator(ret => ZerfallSettings.Instance.MoveDisable && !Me.GotTarget && !Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.Dead && Me.CurrentTarget.Attackable,
                    new Action(ctx => RunStatus.Success)),

                // Use leaders target
                new Decorator(
                    ret =>
                    !ZerfallSettings.Instance.MoveDisable && Me.IsInParty && RaFHelper.Leader != null && RaFHelper.Leader.GotTarget && Me.GotTarget &&
                    Me.CurrentTargetGuid != RaFHelper.Leader.CurrentTargetGuid,
                    new Action(ret =>
                               RaFHelper.Leader.CurrentTarget.Target())),

                // Clear target and return failure if it's tagged by someone else
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && !Me.IsInParty && Me.GotTarget && Me.CurrentTarget.TaggedByOther,
                              new Action(delegate
                              {
                                  SpellManager.StopCasting();
                                  Log("Current target is not tagged by me, Aborting pull!");
                                  Blacklist.Add(Me.CurrentTarget, TimeSpan.FromMinutes(30));
                                  Me.ClearTarget();
                                  return RunStatus.Failure;
                              })
                    ),

                // If we are casting we assume we are already pulling so let it 'return' smoothly. 
                // if we are in combat pull suceeded and the combat behavior should run
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && (Me.IsCasting || Me.Combat) && Me.CurrentTarget.Distance < PullDistance + 3,
                              new Action(delegate { return RunStatus.Success; })),

                // Make sure we got a proper target
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && !Me.GotTarget && !Me.IsInParty,
                              new Action(delegate
                              {
                                  Targeting.Instance.TargetList[0].Target();
                                  WoWMovement.Face();
                                  Thread.Sleep(100);
                                  return RunStatus.Success;
                              })),

                // Blacklist target's we can't move to
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Navigator.GeneratePath(Me.Location, Me.CurrentTarget.Location).Length <= 0,
                              new Action(delegate
                              {
                                  Blacklist.Add(Me.CurrentTargetGuid, TimeSpan.FromDays(365));
                                  Log("Failed to generate path to: {0} blacklisted!",
                                      Me.CurrentTarget.Name);
                                  return RunStatus.Success;
                              })
                    ),

                // Move closer to the target if we are too far away or in !Los
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.GotTarget && (Me.CurrentTarget.Distance > PullDistance || !Me.CurrentTarget.InLineOfSight),
                              new Action(delegate
                              {
                                  if (!ZerfallSettings.Instance.MoveDisable)
                                  {
                                      Log("Moving towards:{0}", Me.CurrentTarget);
                                      Navigator.MoveTo(Me.CurrentTarget.Location);
                                  }
                              })),

                // Stop moving if we are moving
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.IsMoving,
                              new Action(ret => WoWMovement.MoveStop())),

                // Face the target if we aren't
                new Decorator(ret => !ZerfallSettings.Instance.MoveDisable && Me.GotTarget && !Me.IsFacing(Me.CurrentTarget),
                              new Action(ret => WoWMovement.Face())
                    ),

                new PrioritySelector(
                   new Sequence(
                        new Action(ret => Log("Pulling {0}", Me.CurrentTarget.Name)),

                        new PrioritySelector(
                            CreateSpellCheckAndCast(CurrentPullSpell),

                            PetSpellCheckAndCast("Axe Toss", ret => Me.GotAlivePet && Me.Pet.CreatedBySpellId == SummonFelguard),

                            //hopefully start running combat when the above ends. 
                            CreateCombatBehavior()
                                          ))));
        }
    }
}
