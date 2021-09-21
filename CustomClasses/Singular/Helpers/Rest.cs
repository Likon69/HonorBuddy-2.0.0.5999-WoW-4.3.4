using System.Linq;
using System.Threading;

using CommonBehaviors.Actions;

using Singular.Settings;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic.Combat;
using Styx.Logic.Inventory;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace Singular.Helpers
{
    internal static class Rest
    {

        private static bool CorpseAround
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Any(
                    u => u.Distance < 5 && u.Dead &&
                         (u.CreatureType == WoWCreatureType.Humanoid || u.CreatureType == WoWCreatureType.Undead));
            }
        }

        private static bool PetInCombat
        {
            get { return StyxWoW.Me.GotAlivePet && StyxWoW.Me.PetInCombat; }
        }

        public static Composite CreateDefaultRestBehaviour()
        {
            return

                // Don't fucking run the rest behavior (or any other) if we're dead or a ghost. Thats all.
                new Decorator(
                    ret => !StyxWoW.Me.Dead && !StyxWoW.Me.IsGhost && !StyxWoW.Me.IsCasting,
                    new PrioritySelector(
                // Make sure we wait out res sickness. Fuck the classes that can deal with it. :O
                        new Decorator(
                            ret => SingularSettings.Instance.WaitForResSickness && StyxWoW.Me.HasAura("Resurrection Sickness"),
                            new Action(ret => { })),
                // Wait while cannibalizing
                        new Decorator(
                            ret => StyxWoW.Me.CastingSpell != null && StyxWoW.Me.CastingSpell.Name == "Cannibalize" &&
                                   (StyxWoW.Me.HealthPercent < 95 || (StyxWoW.Me.PowerType == WoWPowerType.Mana && StyxWoW.Me.ManaPercent < 95)),
                            new Sequence(
                                new Action(ret => Logger.Write("Waiting for Cannibalize")),
                                new ActionAlwaysSucceed())),
                // Cannibalize support goes before drinking/eating
                        new Decorator(
                            ret =>
                            (StyxWoW.Me.HealthPercent <= SingularSettings.Instance.MinHealth ||
                             (StyxWoW.Me.PowerType == WoWPowerType.Mana && StyxWoW.Me.ManaPercent <= SingularSettings.Instance.MinMana)) &&
                            SpellManager.CanCast("Cannibalize") && CorpseAround,
                            new Sequence(
                                new Action(ret => Navigator.PlayerMover.MoveStop()),
                                Helpers.Common.CreateWaitForLagDuration(),
                                new Action(ret => SpellManager.Cast("Cannibalize")),
                                new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))),
                // Check if we're allowed to eat (and make sure we have some food. Don't bother going further if we have none.
                        new Decorator(
                            ret =>
                            !StyxWoW.Me.IsSwimming && StyxWoW.Me.HealthPercent <= SingularSettings.Instance.MinHealth && !StyxWoW.Me.HasAura("Food") &&
                            Consumable.GetBestFood(false) != null,
                            new PrioritySelector(
                                new Decorator(
                                    ret => StyxWoW.Me.IsMoving,
                                    new Action(ret => Navigator.PlayerMover.MoveStop())),
                                new Sequence(
                                    new Action(
                                        ret =>
                                        {
                                            Styx.Logic.Common.Rest.FeedImmediate();
                                        }),
                                    Helpers.Common.CreateWaitForLagDuration()))),
                // Make sure we're a class with mana, if not, just ignore drinking all together! Other than that... same for food.
                        new Decorator(
                            ret =>
                            !StyxWoW.Me.IsSwimming && (StyxWoW.Me.PowerType == WoWPowerType.Mana || StyxWoW.Me.Class == WoWClass.Druid) &&
                            StyxWoW.Me.ManaPercent <= SingularSettings.Instance.MinMana &&
                            !StyxWoW.Me.HasAura("Drink") && Consumable.GetBestDrink(false) != null,
                            new PrioritySelector(
                                new Decorator(
                                    ret => StyxWoW.Me.IsMoving,
                                    new Action(ret => Navigator.PlayerMover.MoveStop())),
                                new Sequence(
                                    new Action(ret =>
                                        {
                                            Styx.Logic.Common.Rest.DrinkImmediate();
                                        }),
                                    Helpers.Common.CreateWaitForLagDuration()))),
                // This is to ensure we STAY SEATED while eating/drinking. No reason for us to get up before we have to.
                        new Decorator(
                            ret =>
                            (StyxWoW.Me.HasAura("Food") && StyxWoW.Me.HealthPercent < 95) ||
                            (StyxWoW.Me.HasAura("Drink") && StyxWoW.Me.PowerType == WoWPowerType.Mana && StyxWoW.Me.ManaPercent < 95),
                            new ActionAlwaysSucceed()),
                        new Decorator(
                            ret =>
                            ((StyxWoW.Me.PowerType == WoWPowerType.Mana && StyxWoW.Me.ManaPercent <= SingularSettings.Instance.MinMana) ||
                            StyxWoW.Me.HealthPercent <= SingularSettings.Instance.MinHealth) && !StyxWoW.Me.CurrentMap.IsBattleground,
                            new Action(ret => Logger.Write("We have no food/drink. Waiting to recover our health/mana back")))
                        ));
        }

    }
}
