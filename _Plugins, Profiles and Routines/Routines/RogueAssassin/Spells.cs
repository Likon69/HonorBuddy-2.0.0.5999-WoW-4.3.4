using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace RogueAssassin
{
    internal static class Spells
    {
        #region IDs

        public const int BACKSTAB = 53;
        public const int COLD_BLOOD = 14177;
        public const int DEADLY_POISON = 2823;
        public const int ENVENOM = 32645;
        public const int FAN_OF_KNIVES = 51723;
        public const int GARROTE = 703;
        public const int MUTILATE = 1329;
        public const int OVERKILL = 58427;
        public const int REDIRECT = 73981;
        public const int RUPTURE = 1943;
        public const int SLICE_AND_DICE = 5171;
        public const int TRICKS_OF_THE_TRADE = 57934;
        public const int VANISH = 1856;
        public const int VENDETTA = 79140;

        #endregion

        #region Methods

        public static Composite Cast(int spell)
        {
            return Cast(spell, ret => true);
        }

        public static Composite Cast(int spell, CanRunDecoratorDelegate conditions)
        {
            return new Decorator(ret => conditions(ret) && SpellManager.CanCast(spell) && SpellManager.Cast(spell),
                                 new SpellLog(spell));
        }

        /// <summary>
        /// Casts a spell and executes <paramref name="onSuccess"/> if successful.
        /// This method will not log anything by default.
        /// </summary>
        /// <param name="spell">Id of the spell to cast.</param>
        /// <param name="conditions">Conditions to test before casting.</param>
        /// <param name="onSuccess">Action to run on success.</param>
        /// <returns><see cref="Decorator"/> that casts a spell and performs a custom action.</returns>
        public static Composite Cast(int spell, CanRunDecoratorDelegate conditions, Action onSuccess)
        {
            return new Decorator(ret => conditions(ret) && SpellManager.CanCast(spell) && SpellManager.Cast(spell), onSuccess);
        }

        public static Composite Cast(int spell, WoWUnit target)
        {
            return Cast(spell, target, ret => true);
        }

        public static Composite Cast(int spell, WoWUnit target, CanRunDecoratorDelegate condition)
        {
            return new Decorator(
                ret => condition(ret) && SpellManager.CanCast(spell, target) && SpellManager.Cast(spell, target),
                new SpellLog(spell, target));
        }

        public static Composite CastFocus(int spell)
        {
            return CastFocus(spell, ret => true);
        }

        public static Composite CastFocus(int spell, CanRunDecoratorDelegate condition)
        {
            return new Decorator(
                ret =>
                condition(ret) && SpellManager.CanCast(spell, Helpers.Focus) && SpellManager.Cast(spell, Helpers.Focus),
                new SpellLog(spell, Helpers.Focus));
        }

        public static Composite CastStatus(int spell)
        {
            return CastStatus(spell, ret => true);
        }

        public static Composite CastStatus(int spell, CanRunDecoratorDelegate conditions)
        {
            return new Decorator(ret => conditions(ret) && SpellManager.CanCast(spell),
                                 new Action(context =>
                                                {
                                                    if (SpellManager.Cast(spell))
                                                    {
                                                        LogSpell(spell);
                                                        return RunStatus.Success;
                                                    }
                                                    return RunStatus.Failure;
                                                }));
        }

        public static Composite CastStatus(int spell, WoWUnit target)
        {
            return CastStatus(spell, target, ret => true);
        }

        public static Composite CastStatus(int spell, WoWUnit target, CanRunDecoratorDelegate conditions)
        {
            return new Decorator(ret => conditions(ret) && SpellManager.CanCast(spell, target),
                                 new Action(context =>
                                                {
                                                    if (SpellManager.Cast(spell, target))
                                                    {
                                                        LogSpell(spell);
                                                        return RunStatus.Success;
                                                    }

                                                    return RunStatus.Failure;
                                                }));
        }

        private static void LogSpell(int spell)
        {
            Logging.Write(Color.LightBlue,
                          string.Format("[{0}] [{1}] {2}",
                                        Helpers.CurrentEnergy,
                                        StyxWoW.Me.ComboPoints,
                                        SpellManager.RawSpells.First(s => s.Id == spell).Name));
        }

        #endregion

        #region Nested type: SpellLog

        private class SpellLog : Action
        {
            private readonly int _spell;
            private readonly WoWUnit _target;

            public SpellLog(int spell)
            {
                _spell = spell;
            }

            public SpellLog(int spell, WoWUnit target)
            {
                _spell = spell;
                _target = target;
            }

            protected override RunStatus Run(object context)
            {
                if (_target == null)
                {
                    Logging.Write(Color.LightBlue,
                                  string.Format("[{0}] [{1}] {2}",
                                                StyxWoW.Me.CurrentPower,
                                                StyxWoW.Me.ComboPoints,
                                                SpellManager.RawSpells.First(s => s.Id == _spell).Name));
                }
                else
                    Logging.Write(Color.LightBlue,
                                  string.Format("[{0}] [{1}] {2} @ {3}",
                                                StyxWoW.Me.CurrentPower,
                                                StyxWoW.Me.ComboPoints,
                                                SpellManager.RawSpells.First(s => s.Id == _spell).Name,
                                                _target));

                return Parent is Selector ? RunStatus.Failure : RunStatus.Success;
            }
        }

        #endregion
    }
}