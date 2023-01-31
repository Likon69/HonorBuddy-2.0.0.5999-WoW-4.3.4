using CLU.Helpers;

using Styx;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

namespace CLU.Classes
{
    public abstract class RotationBase
    {
        protected static readonly Spell Spells = Spell.Instance;

        protected static readonly Unit Units = Unit.Instance;

        protected static readonly Item Items = Item.Instance;

        protected static readonly Buff Buffs = Buff.Instance;

        protected static readonly PetManager Pets = PetManager.Instance;

        protected static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        protected static WoWUnit Pet
        {
            get { return StyxWoW.Me.Pet; }
        }

        public abstract string KeySpell { get; }

        public abstract Composite SingleRotation { get; }

        public abstract Composite PVPRotation { get; }

        public abstract Composite PVERotation { get; }

        public abstract Composite Medic { get; }

        public abstract Composite PreCombat { get; }

        public abstract string Name { get; }

        public virtual string Help
        {
            get { return " No help available for this rotation."; }
        }

        // this is used by the moving/facing behavior
        // computed as distance to target's bounding box
        public virtual float CombatMinDistance
        {
            get { return 1f; }
        }

        public virtual float CombatMaxDistance
        {
            get { return 35f; }
        }
    }
}
