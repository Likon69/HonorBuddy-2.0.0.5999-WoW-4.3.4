
namespace CLU.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CommonBehaviors.Actions;

    using Styx;
    using Styx.Combat.CombatRoutine;
    using Styx.Logic.Combat;
    using Styx.Logic.Pathing;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;

    using TreeSharp;

    using global::CLU.GUI;

    using Action = TreeSharp.Action;

    public class PetManager
    {
        /* putting all the Pets Spell/Buff/Debuff logic here */

        private static readonly Spell Spells = Spell.Instance;

        private static readonly PetManager PetsInstance = new PetManager();

        /// <summary>
        /// An instance of the Pets Class
        /// </summary>
        public static PetManager Instance { get { return PetsInstance; } }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static ulong petGuid;
        private static readonly List<WoWPetSpell> PetSpells = new List<WoWPetSpell>();

        internal static void Pulse()
        {
            if (!StyxWoW.Me.GotAlivePet)
            {
                PetSpells.Clear();
                return;
            }

            if (StyxWoW.Me.Pet != null && petGuid != StyxWoW.Me.Pet.Guid)
            {
                petGuid = StyxWoW.Me.Pet.Guid;
                PetSpells.Clear();
                PetSpells.AddRange(StyxWoW.Me.PetSpells);
            }
        }

        /*--------------------------------------------------------
         * Spells
         *--------------------------------------------------------
         */

        /// <summary>Returns the Pet spell cooldown using Timespan (00:00:00.0000000)
        /// gtfo if the Pet dosn't have the spell.</summary>
        /// <param name="name">the name of the spell to check for</param>
        /// <returns>The spell cooldown.</returns>
        public TimeSpan PetSpellCooldown(string name)
        {   
            WoWPetSpell petAction = PetSpells.FirstOrDefault(p => p.ToString() == name);
            if (petAction == null || petAction.Spell == null)
            {
                return TimeSpan.Zero;
            }

                // CLU.Instance.Log(" [PetSpellCooldown] {0} : {1}", name, petAction.Spell.CooldownTimeLeft);
            return petAction.Spell.CooldownTimeLeft;
        }

        /// <summary>Returns a summoned pet</summary>
        /// <param name="name">the name of the spell</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast pet summon spell.</returns>
        public Composite CastPetSummonSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            var isWarlock = Me.Class == WoWClass.Warlock && SpellManager.Spells[name].Name.StartsWith("Summon ");
            if (isWarlock)
            {
                Spells.KnownChanneledSpells.Add(name);
            }

            return new Decorator(
                delegate(object a)
                {
                    if (!cond(a))
                        return false;

                    if (!Spells.CanCast(name, Me))
                        return false;

                    return true;
                },
                new Decorator(
                    new Sequence(
                        new Action(a => CLU.Instance.Log(" [Pet Summon] {0} ", label)),
                        new Action(a => Spell.CastMySpell(name)),
                        new Wait(
                            5,
                            a => Me.GotAlivePet || !Me.IsCasting,
                            new PrioritySelector(
                                new Decorator(a => StyxWoW.Me.IsCasting, new Action(ret => SpellManager.StopCasting())),
                                new ActionAlwaysSucceed())))));
        }

        /// <summary>Cast's a pet spell</summary>
        /// <param name="name">the pet spell to cast</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast pet spell.</returns>
        public Composite CastPetSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                a => cond(a) && CanCastPetSpell(name),
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Pet Spell] {0} ", label)), new Action(a => CastMyPetSpell(name))));
        }

        /// <summary>Returns true if the pet spell can be cast</summary>
        /// <param name="name">the name of the spell to check</param>
        /// <returns>The can cast pet spell.</returns>
        public bool CanCastPetSpell(string name)
        {
            WoWPetSpell petAction = Me.PetSpells.FirstOrDefault(p => p.ToString() == name);
            if (petAction == null || petAction.Spell == null)
            {
                return false;
            }

            return !petAction.Spell.Cooldown;
        }

        /// <summary>
        /// the main cast pet spell method (uses Lua)
        /// </summary>
        /// <param name="name">the name of the spell to cast</param>
        private static void CastMyPetSpell(string name)
        {
            var spell = Me.PetSpells.FirstOrDefault(p => p.ToString() == name);
            if (spell == null)
                return;

            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        /// <summary>Casts a Pet spell at the units location</summary>
        /// <param name="name">the name of the spell to cast</param>
        /// <param name="onUnit">The on Unit.</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast spell at location.</returns>
        public Composite CastPetSpellAtLocation(string name, Spell.UnitSelectionDelegate onUnit, CanRunDecoratorDelegate cond, string label)
        {
            // CLU.DebugLog(" [CastSpellAtLocation] name = {0} location = {1} and can we cast it? {2}", name, CLU.SafeName(unit), CanCast(name));
            return new Decorator(
                delegate(object a)
                {
                    if (!SettingsFile.Instance.AutoManageAoE)
                        return false;


                    WoWPoint currTargetLocation = Me.CurrentTarget.Location;
                    if (Unit.NearbyControlledUnits(currTargetLocation, 20, false).Any())
                        return false;

                    if (!cond(a))
                        return false;


                    if (!CanCastPetSpell(name))
                        return false;

                    return onUnit != null;
                },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Pet Casting at location] {0} ", label)),
                    new Action(a => CastMyPetSpell(name)),
                // new WaitContinue(
                //    0,
                //    ret => StyxWoW.Me.CurrentPendingCursorSpell != null &&
                //           StyxWoW.Me.CurrentPendingCursorSpell.Name == name,
                //    new ActionAlwaysSucceed()),
                    new Action(a => LegacySpellManager.ClickRemoteLocation(onUnit(a).Location))));
        }

        /*--------------------------------------------------------
         * Buffs
         *--------------------------------------------------------
         */

        /// <summary>Returns true if the Pet has the buff</summary>
        /// <param name="name">the name of the buff to check for</param>
        /// <returns>The pet has buff.</returns>
        public bool PetHasBuff(string name)
        {
            // Me.Pet.ActiveAuras.ContainsKey(name);
            return Me.GotAlivePet && Buff.HasAura(Me.Pet, name, Me.Pet);
        }

        /// <summary>Returns true if the Pet has the buff</summary>
        /// <param name="name">the name of the buff to check for</param>
        /// <returns>The pet has buff.</returns>
        public bool PetHasActiveBuff(string name)
        {
            return Me.GotAlivePet && Me.Pet.ActiveAuras.ContainsKey(name);
        }

        /// <summary>Returns the buff count on the Pet</summary>
        /// <param name="name">the name of the buff to check</param>
        /// <returns>The pet count buff.</returns>
        public uint PetCountBuff(string name)
        {
            return !Me.GotAlivePet ? 0 : Buff.GetAuraStack(Me.Pet, name, false);
        }

        /// <summary>Returns the buff time left on the Pet</summary>
        /// <param name="name">the name of the buff to check for</param>
        /// <returns>The pet buff time left.</returns>
        public TimeSpan PetBuffTimeLeft(string name)
        {
            return !Me.GotAlivePet ? TimeSpan.Zero : Buff.GetAuraTimeLeft(Me.Pet, name, false);
        }
    }
}
