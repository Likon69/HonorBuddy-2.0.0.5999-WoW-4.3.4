using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using TreeSharp;

using Action = TreeSharp.Action;

namespace CLU.Helpers
{
    using CommonBehaviors.Actions;

    using global::CLU.GUI;
    using global::CLU.Lists;

    public class Spell
    {
        /* putting all the spell logic here */
        public delegate WoWUnit UnitSelectionDelegate(object context);

        private static readonly Spell SpellInstance = new Spell();

        /// <summary>
        /// An instance of the Spell Class
        /// </summary>
        public static Spell Instance { get { return SpellInstance; } }

        /// <summary>
        /// known channeled spells. used as part of the isChannelled spell check method
        /// </summary>
        public readonly HashSet<string> KnownChanneledSpells = new HashSet<string>();

        /// <summary>
        /// Me! or is it you?
        /// </summary>
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        /// <summary>
        /// Clip time to account for lag
        /// </summary>
        /// <returns>Time to begin next channel</returns>
        public double ClippingDuration()
        {
            return 0.4;
        }

        /// <summary>
        /// a shortcut too GlobalCooldownLeft
        /// </summary>
        public static double GCD
        {
            get { return SpellManager.GlobalCooldownLeft.TotalSeconds; }
        }

        /// <summary>
        /// Returns true if the player is currently channeling a spell
        /// </summary>
        public bool PlayerIsChanneling
        {
            get { return StyxWoW.Me.ChanneledCastingSpellId != 0; }
        }

        /// <summary>
        ///  Creates a composite that will return a success, so long as you are currently casting. (Use this to prevent the CC from
        ///  going down to lower branches in the tree, while casting.)
        /// </summary>
        /// <param name="allowLagTollerance">Whether or not to allow lag tollerance for spell queueing</param>
        /// <returns>success, so long as you are currently casting.</returns>
        public static Composite WaitForCast(bool allowLagTollerance)
        {
            return
                new Action(ret =>
                {
                    if (!StyxWoW.Me.IsCasting)
                        return RunStatus.Failure;

                    if (!StyxWoW.GlobalCooldown)
                        return RunStatus.Failure;

                    var latency = StyxWoW.WoWClient.Latency * 2;
                    if (allowLagTollerance && StyxWoW.Me.CurrentCastTimeLeft.TotalMilliseconds < latency)
                        return RunStatus.Failure;

                    return RunStatus.Running;
                });
        }

        /// <summary>
        /// The Primary spell cast method (Currently converts the spell to its spellID, then casts it)
        /// </summary>
        /// <param name="name">name of the spell to cast.</param>
        public static void CastMySpell(string name)
        {
            var mySpellToCast = GetSpellByName(name); // Convert the string name to a wowspell

            // Fishing for KeyNotFoundException's yay!
            if (mySpellToCast != null)
            {
                SpellManager.Cast(mySpellToCast);
            }
        }

        /// <summary>
        /// Gets the spell by name (string)
        /// </summary>
        /// <param name="spellName">the spell name to get</param>
        /// <returns> Returns the spellname</returns>
        private static WoWSpell GetSpellByName(string spellName)
        {
            WoWSpell spell;
            if (!SpellManager.Spells.TryGetValue(spellName, out spell))
                spell = SpellManager.RawSpells.FirstOrDefault(s => s.Name == spellName);

            return spell;
        }

        /// <summary>This is CLU's cancast method. It checks ALOT! Returns true if the player can cast the spell.</summary>
        /// <param name="name">name of the spell to check.</param>
        /// <param name="target">The target.</param>
        /// <returns>The can cast.</returns>
        public bool CanCast(string name, WoWUnit target)
        {
            // no target....hmmmm
            if (target == null)
            {
                return false;
            }

            if (StyxWoW.GlobalCooldown)
            {
                return false;
            }

            // Dont do shit if we are Stunned, or Silenced
             if (Unit.Instance.IsIncapacitated(Me))
             {
                 return false;
             }

            var spellToCast = GetSpellByName(name); // Convert the string name to a wowspell
            
            // Fishing for KeyNotFoundException's yay!
            if (spellToCast == null)
            {
                 return false;
            }

             // Do we have enough mana, rage, runic power, etc ?
             if (Me.CurrentPower < spellToCast.PowerCost)
             {
                    return false;
             }

             // Is the casttime of the spell 0 and its not a channelled spell or i am not moving. (If we are moving can we cast instant spells and spells that are allowed to be cast while moving?)  // && IsChanneledSpell(spellToCast.Name)
             var instantCast = (spellToCast.CastTime == 0 && !spellToCast.IsFunnel) || Buff.PlayerHasBuff("Aspect of the Fox") || spellToCast.Name == "Scorch";
             if (Me.IsMoving)
             {
                 if (!instantCast)
                 {
                     return false;
                 }
             }

            var inRange = spellToCast.MaxRange <= 0
                                                 ? target.Distance < MeleeRange
                                                 : target.Distance < spellToCast.MaxRange;
            // Check spell range - Ignore it if we are targeting a boss that has a fucked up range ie: (Ultraxion, Deathwing, etc)
            if (!inRange && !BossList.IgnoreRangeCheck.Contains(target.Entry))
            {
                // TODO: Check if DistanceToBoundingBox() will be affective in this check considering it returns a value less than .Distance (ie: true hit box)
                return false;
            }

            // Ok now I am happy to use Honorbuddys Cancast method. But! checkRange and checkMoving are set
            // to false as I now know that checkmoving does not account for aspect of the fox and checkrange does not allow me to do my little IgnoreRangeCheck hack...
            // Apoc, Raphus or Nesox...I know...hackish.

            return SpellManager.CanCast(spellToCast, target, false, false, true);
        }

        /// <summary>
        ///  Returns the current Melee range for the player
        /// </summary>
        public static float MeleeRange
        {
            get
            {
                // If we have no target... then give nothing.
                if (Me.CurrentTargetGuid == 0)
                    return 0f;

                if (Me.CurrentTarget != null)
                {
                    return Me.CurrentTarget.IsPlayer ? 3.5f : Math.Max(5f, Me.CombatReach + 1.3333334f + Me.CurrentTarget.CombatReach);
                }

                return 0f;
            }
        }

        /// <summary>Escape Lua names using UTF8 encoding.
        /// Heres a url we can use to decode this. http://software.hixie.ch/utilities/cgi/unicode-decoder/utf8-decoder
        /// Usefull when a Lua command fails.</summary>
        /// <param name="luastring">the string to encode</param>
        /// <returns>The real lua escape.</returns>
        public static string RealLuaEscape(string luastring)
        {
                var bytes = Encoding.UTF8.GetBytes(luastring);
                return bytes.Aggregate(String.Empty, (current, b) => current + ("\\" + b));
        }

        /// <summary>Returns the current casttime of the spell.</summary>
        /// <param name="name">the name of the spell to check for</param>
        /// <returns>The cast time.</returns>
        public double CastTime(string name)
        {
            try
            {
                if (!SpellManager.HasSpell(name))
                    return 999999.9;

                WoWSpell s = SpellManager.Spells[name];
                return s.CastTime / 1000.0;
            }
            catch
            {
                CLU.Instance.Log(" [ERROR] in CastTime: {0} ", name);
                return 999999.9;
            }
        }

        /// <summary>Returns the spellcooldown using Timespan (00:00:00.0000000)
        /// gtfo if the player dosn't have the spell.</summary>
        /// <param name="name">the name of the spell to check for</param>
        /// <returns>The spell cooldown.</returns>
        public TimeSpan SpellCooldown(string name)
        {
            var spellToCheck = GetSpellByName(name); // Convert the string name to a WoWspell

            // Fishing for KeyNotFoundException's yay!
            return spellToCheck == null ? TimeSpan.MaxValue : spellToCheck.CooldownTimeLeft;
        }


        /// <summary>Casts a spell by name on a target without checking the Cancast Method</summary>
        /// <param name="name">the name of the spell to cast in engrish</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast spell.</returns>
        public Composite CastSpecialSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    if (!cond(a))
                        return false;

                    return true;
                },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Casting] {0} ", label)), new Action(a => CastMySpell(name))));
        }

        /// <summary>Casts a spell by name on a target</summary>
        /// <param name="name">the name of the spell to cast in engrish</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast spell.</returns>
        public Composite CastSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                    {
                        if (!cond(a))
                            return false;

                        if (!this.CanCast(name, Me.CurrentTarget))
                            return false;

                        return true;
                    },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Casting] {0} ", label)), new Action(a => CastMySpell(name))));
        }

        /// <summary>Casts self spells eg: 'Fient', 'Shield Wall', 'Blood Tap', 'Rune Tap' </summary>
        /// <param name="name">the name of the spell in english</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast self spell.</returns>
        public Composite CastSelfSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                    {
                        if (!cond(a))
                            return false;

                        if (!this.CanCast(name, Me))
                            return false;

                        return true;
                    },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Casting] {0} ", label)), new Action(a => CastMySpell(name))));
        }

        /// <summary>Casts a spell on a specified unit</summary>
        /// <param name="name">the name of the spell to cast</param>
        /// <param name="onUnit">The Unit.</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast spell on the unit</returns>
        public Composite CastSpellonUnit(string name, UnitSelectionDelegate onUnit, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    if (!cond(a))
                        return false;
                    
                     if (!this.CanCast(name, onUnit(a)))
                        return false;

                    return onUnit(a) != null;
                },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Casting] {0} on {1}", label, CLU.SafeName(onUnit(a)))),
                    new Action(a => SpellManager.Cast(name, onUnit(a)))));
        }

        /// <summary>Casts the interupt by name on your current target. Checks CanInterruptCurrentSpellCast.</summary>
        /// <param name="name">the name of the spell in english</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast interupt.</returns>
        public Composite CastInterupt(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                    {
                        if (!SettingsFile.Instance.AutoManageInterrupts)
                            return false;

                        if (!cond(a))
                            return false;

                        if (Me.CurrentTarget != null && !(Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast))
                            return false;

                        if (Me.CurrentTarget != null && !this.CanCast(name, Me.CurrentTarget))
                            return false;

                        return true;
                    },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Interupt] {0} ", label)), new Action(a => CastMySpell(name))));
        }

        /// <summary>Casts a Totem by name</summary>
        /// <param name="name">the name of the spell to cast in engrish</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast spell.</returns>
        public Composite CastTotem(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                {
                    if (!SettingsFile.Instance.AutoManageTotems)
                        return false;

                    if (!cond(a))
                        return false;

                    if (!this.CanCast(name, Me.CurrentTarget))
                        return false;

                    return true;
                },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Totem] {0} ", label)), new Action(a => CastMySpell(name))));
        }

        /// <summary>Returns true if the spell is a known channeled spell</summary>
        /// <param name="name">the name of the spell to check</param>
        /// <returns>The is channeled spell.</returns>
        private bool IsChanneledSpell(string name)
        {
            return this.KnownChanneledSpells != null && this.KnownChanneledSpells.Contains(name);
        }

        /// <summary>Channel spell on target. Will not break channel and adds the name of spell to knownChanneledSpells</summary>
        /// <param name="name">the name of the spell to channel</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The channel spell.</returns>
        public Composite ChannelSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            this.KnownChanneledSpells.Add(name);
            return
                new PrioritySelector(
                    new Decorator(
                        x => this.PlayerIsChanneling && Me.ChanneledCastingSpellId == SpellManager.Spells[name].Id,
                        new Action(a => CLU.Instance.Log(" [Channeling] {0} ", name))),
                     this.CastSpell(name, cond, label));
        }

        /// <summary>Channel spell on player. Will not break channel and adds the name of spell to _knownChanneledSpells</summary>
        /// <param name="name">the name of the spell to channel</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The channel self spell.</returns>
        public Composite ChannelSelfSpell(string name, CanRunDecoratorDelegate cond, string label)
        {
            this.KnownChanneledSpells.Add(name);
            return
                new PrioritySelector(
                    new Decorator(
                        x => this.PlayerIsChanneling && Me.ChanneledCastingSpellId == SpellManager.Spells[name].Id,
                        new Action(a => CLU.Instance.Log(" [Channeling] {0} ", name))),
                    this.CastSelfSpell(name, cond, label));
        }

        /// <summary>Casts an area spell such as DnD or Hellfire</summary>
        /// <param name="name">name of the area spell</param>
        /// <param name="radius">radius</param>
        /// <param name="requiresTerrainClick">true if the area spell requires a terrain click</param>
        /// <param name="minAffectedTargets">the minimum affected targets in the cluster</param>
        /// <param name="minRange">minimum range</param>
        /// <param name="maxRange">maximum range</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast area spell.</returns>
        public Composite CastAreaSpell(
            string name,
            double radius,
            bool requiresTerrainClick,
            int minAffectedTargets,
            double minRange,
            double maxRange,
            CanRunDecoratorDelegate cond,
            string label)
        {
            WoWPoint bestLocation = WoWPoint.Empty;
            return new Decorator(
                delegate(object a)
                    {
                        if (!SettingsFile.Instance.AutoManageAoE)
                            return false;

                        if (!cond(a))
                            return false;

                    bestLocation = Unit.Instance.FindClusterTargets(
                        radius, minRange, maxRange, minAffectedTargets, Battlegrounds.IsInsideBattleground);
                    if (bestLocation == WoWPoint.Empty)
                            return false;

                        if (!this.CanCast(name, Me.CurrentTarget))
                            return false;

                        return true;
                    },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [AoE] {0} ", label)),
                    new Action(a => CastMySpell(name)),
                    new DecoratorContinue(x => requiresTerrainClick, new Action(a => LegacySpellManager.ClickRemoteLocation(bestLocation)))));
        }

        /// <summary>Casts a spell at the units location</summary>
        /// <param name="name">the name of the spell to cast</param>
        /// <param name="onUnit">The on Unit.</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast spell at location.</returns>
        public Composite CastSpellAtLocation(string name, UnitSelectionDelegate onUnit, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                delegate(object a)
                    {
                        if (!SettingsFile.Instance.AutoManageAoE)
                            return false;

                        if (!cond(a))
                            return false;

                        return onUnit != null && this.CanCast(name, onUnit(a));
                },
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Casting at Location] {0} ", label)),
                    new Action(a => CastMySpell(name)),
                    // new WaitContinue(
                    //    0,
                    //    ret => StyxWoW.Me.CurrentPendingCursorSpell != null &&
                    //           StyxWoW.Me.CurrentPendingCursorSpell.Name == name,
                    //    new ActionAlwaysSucceed()),
                    new Action(a => LegacySpellManager.ClickRemoteLocation(onUnit(a).Location))));
        }

        /// <summary>
        /// Sets a trap at the current targets location.
        /// </summary>
        /// <param name="trapName">the name of the trap to use</param>
        /// <param name="onUnit">the unit to place the trap on.</param>
        /// <param name="cond">check conditions supplied are true </param>
        /// <returns>nothing</returns>
        public Composite HunterTrapBehavior(string trapName, UnitSelectionDelegate onUnit, CanRunDecoratorDelegate cond)
        {
            return new PrioritySelector(
                new Decorator(
                    delegate(object a)
                    {
                        if (!SettingsFile.Instance.AutoManageAoE)
                            return false;

                        if (!cond(a)) return false;

                        return onUnit != null && onUnit(a) != null && !onUnit(a).IsMoving && onUnit(a).DistanceSqr < 40 * 40
                        && SpellManager.HasSpell(trapName) && !SpellManager.Spells[trapName].Cooldown;
                    },
                    new PrioritySelector(
                        Buff.CastBuff("Trap Launcher", ret => true, "Trap Launcher"),
                        new Decorator(
                            ret => Me.HasAura("Trap Launcher"),
                            new Sequence(
                                new Switch<string>(ctx => trapName,
                                    new SwitchArgument<string>("Immolation Trap",
                                        new Action(ret => LegacySpellManager.CastSpellById(82945))),
                                    new SwitchArgument<string>("Freezing Trap",
                                        new Action(ret => LegacySpellManager.CastSpellById(60192))),
                                    new SwitchArgument<string>("Explosive Trap",
                                        new Action(ret => LegacySpellManager.CastSpellById(82939))),
                                    new SwitchArgument<string>("Ice Trap",
                                        new Action(ret => LegacySpellManager.CastSpellById(82941))),
                                    new SwitchArgument<string>("Snake Trap",
                                        new Action(ret => LegacySpellManager.CastSpellById(82948)))
                                    ),
                                // new ActionSleep(200),
                                new Action(a => LegacySpellManager.ClickRemoteLocation(onUnit(a).Location)))))));
        }

        /// <summary>Channels an area spell such as Rain of Fire</summary>
        /// <param name="name">name of the area spell</param>
        /// <param name="radius">radius</param>
        /// <param name="requiresTerrainClick">true if the area spell requires a terrain click</param>
        /// <param name="minAffectedTargets">the minimum affected targets in the cluster</param>
        /// <param name="minRange">minimum range</param>
        /// <param name="maxRange">maximum range</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The channel area spell.</returns>
        public Composite ChannelAreaSpell(
            string name,
            double radius,
            bool requiresTerrainClick,
            int minAffectedTargets,
            double minRange,
            double maxRange,
            CanRunDecoratorDelegate cond,
            string label)
        {
            WoWPoint bestLocation = WoWPoint.Empty;
            return new Decorator(
                delegate(object a)
                    {
                        if (!SettingsFile.Instance.AutoManageAoE)
                            return false;

                        if (!cond(a))
                            return false;

                        if (!this.CanCast(name, Me))
                            return false;

                    bestLocation = Unit.Instance.FindClusterTargets(
                        radius, minRange, maxRange, minAffectedTargets, Battlegrounds.IsInsideBattleground);
                    return bestLocation != WoWPoint.Empty;
                    },
                new PrioritySelector(
                    // dont break it if already casting it
                    new Decorator(
                        x => this.PlayerIsChanneling && Me.ChanneledCastingSpellId == SpellManager.Spells[name].Id,
                        new Action(a => CLU.Instance.Log(name))),
                    // casting logic
                    new Sequence(
                        new Action(a => CLU.Instance.Log(" [AoE Channel] {0} ", label)),
                        new Action(a => CastMySpell(name)),
                        new DecoratorContinue(
                                x => requiresTerrainClick,
                                    new Action(a => LegacySpellManager.ClickRemoteLocation(bestLocation))))));
        }   

        /// <summary>
        /// distance to the targets bounding box
        /// </summary>
        /// <returns>Returns the distance to the targets bounding box</returns>
        public float DistanceToTargetBoundingBox()
        {
            return Me.CurrentTarget == null ? 999999f : DistanceToTargetBoundingBox(Me.CurrentTarget);
        }

        /// <summary>get the distance of this point to our point (taking a stab at this description)</summary>
        /// <param name="target">unit to use as the distance check</param>
        /// <returns>The distance to target bounding box.</returns>
        public static float DistanceToTargetBoundingBox(WoWUnit target)
        {
            return (float)Math.Max(0f, target.Distance - target.BoundingRadius);
        }

        /// <summary>Returns the angle we are facing towards given our point to the targets point  (taking a stab at this description)</summary>
        /// <param name="me">the player</param>
        /// <param name="target">the target</param>
        /// <returns>The facing towards unit radians.</returns>
        private static float FacingTowardsUnitRadians(WoWPoint me, WoWPoint target)
        {
            try
            {
                WoWPoint direction = me.GetDirectionTo(target);
                direction.Normalize();
                float myFacing = ObjectManager.Me.Rotation;

                // real and safe tan reverse function
                double ret = Math.Atan2(direction.Y, direction.X);

                double alpha = Math.Abs(myFacing - ret);
                if (alpha > Math.PI)
                {
                    alpha = Math.Abs(2 * Math.PI - alpha);
                }

                if (Double.IsNaN(alpha)) return 0f;
                return (float)alpha;
            }
            catch
            {
                return 0f;
            }
        }

        /// <summary>Determines how we are facing the target in degrees  (taking a stab at this description)</summary>
        /// <param name="me">the player</param>
        /// <param name="target">the target</param>
        /// <returns>The facing towards unit degrees.</returns>
        public static float FacingTowardsUnitDegrees(WoWPoint me, WoWPoint target)
        {
            return (float)(FacingTowardsUnitRadians(me, target) * 180.0 / Math.PI);
        }

        /// <summary>Casts a spell provided we are inrange and facing the target </summary>
        /// <param name="name">name of the spell to cast</param>
        /// <param name="maxDistance">maximum distance</param>
        /// <param name="maxAngleDeltaDegrees">maximum angle in degrees</param>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The cast conic spell.</returns>
        public Composite CastConicSpell(
            string name, float maxDistance, float maxAngleDeltaDegrees, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                a => cond(a) && this.CanCast(name, Me.CurrentTarget) &&
                     this.DistanceToTargetBoundingBox() <= maxDistance &&
                     FacingTowardsUnitDegrees(Me.Location, Me.CurrentTarget.Location) <=
                     maxAngleDeltaDegrees,
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Casting Conic] {0} ", label)),
                    new Action(a => CastMySpell(name))));
        }

        /// <summary>Stop casting, plain and simple.</summary>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="label">A descriptive label for the clients GUI logging output</param>
        /// <returns>The stop cast.</returns>
        public static Composite StopCast(CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                x => (Me.IsCasting || Me.ChanneledCastingSpellId > 0) && cond(x),
                new Sequence(
                    new Action(a => CLU.Instance.Log(" [Stop Casting] {0} ", label)),
                    new Action(a => SpellManager.StopCasting())));
        }


        /// <summary>
        /// Returns the cooldown of a rune in seconds, Rune count is backwards (eg:4,3,2,1,0 Zero is READY)
        /// </summary>
        /// first_blood = 1
        /// second_blood = 2
        /// first_Unholy = 3
        /// second_Unholy = 4
        /// first_Frost = 5
        /// second_Frost = 6
        /// <param name="rune">number of the run to check (see above)</param>
        /// <returns>The cooldown of the rune specified.</returns>
        public static double RuneCooldown(int rune)
        {
                string runename = String.Empty;
                if (rune == 1)
                    runename = "Blood_1";
                else if (rune == 2)
                    runename = "Blood_2";
                else if (rune == 3)
                    runename = "Unholy_1";
                else if (rune == 4)
                    runename = "Unholy_2";
                else if (rune == 5)
                    runename = "Frost_1";
                else if (rune == 6)
                    runename = "Frost_2";

                // Lets track some rune cooldowns!		
                var lua =
                    String.Format(
                        "local r_start, r_duration, r_ready = GetRuneCooldown({0}) if r_start > 0 then return math.ceil((r_start + r_duration) - GetTime()) else return 0 end",
                        rune);
                try
                {
                    var retValue = Double.Parse(Lua.GetReturnValues(lua)[0]);
                    return retValue;
                }
                catch
                {
                    CLU.Instance.Log("Lua failed in RuneCooldown: " + lua);
                    return 9999;
                }
        }

        /// <summary>Return true of the target has a stealable HELPFUL buff</summary>
        /// <returns>The target has stealable buff.</returns>
        public bool TargetHasStealableBuff()
        {
            // should count how many buffs the target has but meh
                for (int i = 1; i <= 40; i++)
                {
                    try
                    {
                        List<string> luaRet =
                            Lua.GetReturnValues(
                                String.Format(
                                    "local buffName, _, _, _, _, _, _, _, isStealable = UnitAura(\"target\", {0}, \"HELPFUL\") return isStealable,buffName",
                                    i));

                        if (luaRet != null && luaRet[0] == "1")
                        {
                            var stealableSpell = !Buff.PlayerHasActiveBuff(luaRet[0]) && (luaRet[0] != "Arcane Brilliance" && luaRet[0] != "Dalaran Brilliance");
                            if (stealableSpell)
                            {
                                CLU.Instance.Log("Buff Name: {0} isStealable: {1}", luaRet[1], luaRet[0]);
                            }

                            return stealableSpell;
                        }
                    }
                    catch
                    {
                        CLU.Instance.Log("Lua failed in TargetHasStealableBuff");
                        return false;
                    }
                }

            return false;
        }

        /// <summary>
        /// Returns a list of players with the highest mana power descending.
        /// </summary>
        /// <returns>returns a list of players</returns>
        private static IEnumerable<WoWPlayer> HighestMana()
        {
            return (from o in ObjectManager.ObjectList
                    where o is WoWPlayer
                    let p = o.ToPlayer()
                    where p.IsFriendly
                          && p.IsInMyPartyOrRaid
                          && !p.IsMe
                          && !p.Dead
                          && (p.PowerType == WoWPowerType.Mana)
                          && p.IsPlayer && !p.IsPet
                    orderby p.MaxPower descending
                    select p).ToList();
        }

        /// <summary>Return the player to apply focus magic too (will probably go for a static list)</summary>
        /// <returns>The best focus magic target.</returns>
        public WoWPlayer BestFocusMagicTarget()
        {
            int countWithMyFM = HighestMana().Count(p => p.HasAura("Focus Magic") && p.Auras["Focus Magic"].CreatorGuid == Me.Guid);

            return countWithMyFM < 1 ? HighestMana().FirstOrDefault() : null;
        }

        /// <summary>
        /// Use this to print all known spells
        /// </summary>
        public static void DumpSpells()
        {
            foreach (KeyValuePair<string, WoWSpell> sp in SpellManager.Spells)
            {
                if (SpellManager.Spells.ContainsKey(sp.Key))
                {
                    CLU.DebugLog(sp.Key + " " + SpellManager.Spells[sp.Key]);
                }
                else
                {
                    CLU.DebugLog(sp.Key);
                }
            }
        }


        /// <summary>
        ///  Blows your wad all over the floor
        /// </summary>
        /// <returns>Nothing but win</returns>
        public Composite UseRacials()
        {
            return new PrioritySelector(delegate
            {
                foreach (WoWSpell r in CurrentRacials.Where(racial => this.CanCast(racial.Name, Me) && RacialUsageSatisfied(racial)))
                {
                    CLU.Instance.Log(" [Racial Abilitie] {0} ", r.Name);
                    CastMySpell(r.Name);
                }

                return RunStatus.Success;
            });
        }

        /// <summary>
        /// Returns true if the racials conditions are met
        /// </summary>
        /// <param name="racial">the racial to check for</param>
        /// <returns>true if we can use the racial</returns>
        private static bool RacialUsageSatisfied(WoWSpell racial)
        {
            if (racial != null)
            {
                switch (racial.Name)
                {
                    case "Stoneform":
                        return Me.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Bleeding || a.Spell.DispelType == WoWDispelType.Disease || a.Spell.DispelType == WoWDispelType.Poison);
                    case "Escape Artist":
                        return Me.Rooted;
                    case "Every Man for Himself":
                        return Unit.IsCrowdControlled(Me);
                    case "Shadowmeld":
                        return false;
                    case "Gift of the Naaru":
                        return Me.HealthPercent <= 80;
                    case "Darkflight":
                        return false;
                    case "Blood Fury":
                        return true;
                    case "War Stomp":
                        return false;
                    case "Berserking":
                        return true;
                    case "Will of the Forsaken":
                        return Unit.IsCrowdControlled(Me);
                    case "Cannibalize":
                        return false;
                    case "Arcane Torrent":
                        return Me.ManaPercent < 91 && Me.Class != WoWClass.DeathKnight;
                    case "Rocket Barrage":
                        return true;

                    default:
                        return false;
                }
            }

            return false;
        }

        public static IEnumerable<WoWSpell> CurrentRacials
        {
            get
            {
                return SpellManager.RawSpells.Where(racial => racial != null && Racials.Contains(racial.Name)).ToList();
            }
        }

        private static HashSet<string> Racials { get { return _racials; } }

        private static readonly HashSet<string> _racials = new HashSet<string> {
            "Stoneform",                        // Activate to remove poison, disease, and bleed effects; +10% Armor; Lasts 8 seconds. 2 minute cooldown.
            "Escape Artist",                    // Escape the effects of any immobilization or movement speed reduction effect. Instant cast. 1.45 min cooldown
            "Every Man for Himself",            // Removes all movement impairing effects and all effects which cause loss of control of your character. This effect 
            "Shadowmeld",                       // Activate to slip into the shadows, reducing the chance for enemies to detect your presence. Lasts until cancelled or upon 
            "Gift of the Naaru",                // Heals the target for 20% of their total health over 15 sec. 3 minute cooldown.
            "Darkflight",                        // Activates your true form, increasing movement speed by 40% for 10 sec. 3 minute cooldown.
            "Blood Fury",                        // Activate to increase attack power and spell damage by an amount based on level/class for 15 seconds. 2 minute cooldown.
            "War Stomp",                        // Activate to stun opponents - Stuns up to 5 enemies within 8 yards for 2 seconds. 2 minute cooldown.
            "Berserking",                        // Activate to increase attack and casting speed by 20% for 10 seconds. 3 minute cooldown.
            "Will of the Forsaken",              // Removes any Charm, Fear and Sleep effect. 2 minute cooldown.
            "Cannibalize",                        // When activated, regenerates 7% of total health and mana every 2 seconds for 10 seconds. Only works on Humanoid or Undead corpses within 5 yards. Any movement, action, or damage taken while Cannibalizing will cancel the effect.
            "Arcane Torrent",                     // Activate to silence all enemies within 8 yards for 2 seconds. In addition, you gain 15 Energy, 15 Runic Power or 6% Mana. 2 min. cooldown.
            "Rocket Barrage",                     // Launches your belt rockets at an enemy, dealing X-Y fire damage. (24-30 at level 1; 1654-2020 at level 80). 2 min. cooldown.
        };

        public Composite HealMe(string name, CanRunDecoratorDelegate cond, string label)
        {
            return new Decorator(
                x => cond(x) && this.CanCast(name, Me),
                new Sequence(
                    new Action(a => Me.Target()),
                    new Action(a => CLU.Instance.Log(" [Casting Self Heal] {0} ", label)),
                    new Action(a => CastMySpell(name))));
        }

        // public bool NeedToTranqShot
        // {
        //    get
        //    {
        //        var lua = string.Format("buff = { 99646 } local candispel = 1 for i,v in ipairs(buff) do if UnitDebuffID(&quot;target&quot;,v) then candispel = nil end end local i = 1 local buff,_,_,_,bufftype = UnitBuff(&quot;target&quot;, i) while buff do if (bufftype == &quot;Magic&quot; or buff == &quot;Enrage&quot;) and candispel then return true end i = i + 1; buff,_,_,_,bufftype = UnitBuff(&quot;target&quot;, i) end");
        //        try
        //        {
        //            return Lua.GetReturnValues(lua)[0] == "true";
        //        }
        //        catch
        //        {
        //            CLU.Instance.Log("Lua failed in TargetIsEnrage: " + lua);
        //            return false;
        //        }
        //    }
        // }

        /// <summary>
        /// Finds a target that does not have the specified spell and applys it.
        /// </summary>
        /// <param name="cond">The conditions that must be true</param>
        /// <param name="spell">The spell to be cast</param>
        /// <returns></returns>
        public Composite FindMultiDotTarget(CanRunDecoratorDelegate cond, string spell)
        {
            return new Decorator(cond,
                new Sequence(
                    //get a target
                    new Action( delegate {

                        if (!SettingsFile.Instance.AutoManageMultiDotting)
                                return RunStatus.Success;
                           
                            WoWUnit target = Unit.RangedEnemyUnits.FirstOrDefault(u => !u.HasAura(spell));
                            if (target != null)
                            {
                                // CLU.Instance.Log(target.Name);
                                target.Target();
                                return RunStatus.Success;
                            }
                        return RunStatus.Failure;
                    }),
                    // if success, keep going. Else quit
                    new PrioritySelector(
                        Buff.CastDebuff(spell, cond, spell)
                        )));
        }

        /// <summary>
        /// This is meant to replace the 'SleepForLagDuration()' method. Should only be used in a Sequence
        /// </summary>
        /// <returns></returns>
        public static Composite CreateWaitForLagDuration()
        {
            return new WaitContinue(TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150), ret => false, new ActionAlwaysSucceed());
        }
    }
}