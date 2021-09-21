#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author: highvoltz $
// $Date: 2012-08-16 20:12:47 +0300 (Per, 16 Ağu 2012) $
// $HeadURL: http://svn.apocdev.com/singular/trunk/Singular/SingularRoutine.cs $
// $LastChangedBy: highvoltz $
// $LastChangedDate: 2012-08-16 20:12:47 +0300 (Per, 16 Ağu 2012) $
// $LastChangedRevision: 649 $
// $Revision: 649 $

#endregion

using System;
using System.Linq;
using System.Reflection;

using Singular.Dynamics;
using Singular.GUI;
using Singular.Helpers;
using Singular.Managers;
using Singular.Settings;
using Singular.Utilities;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Singular
{
    public partial class SingularRoutine : CombatRoutine
    {
        private Composite _combatBehavior;
        private Composite _combatBuffsBehavior;
        private Composite _healBehavior;
        private WoWClass _myClass;
        private Composite _preCombatBuffsBehavior;
        private Composite _pullBehavior;
        private Composite _pullBuffsBehavior;
        private Composite _restBehavior;

        public SingularRoutine()
        {
            Instance = this;

            // Yes, we are hooking in ctor. To be able to refresh behaviors before a botbase caches us, we need to do that
            BotEvents.Player.OnMapChanged += EventHandlers.PlayerOnMapChanged;
        }

        public static SingularRoutine Instance { get; private set; }

        public override string Name { get { return "Singular v2 $Revision: 649 $"; } }

        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        public override bool WantButton { get { return true; } }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        internal static WoWClass MyClass { get; set; }

        internal static event EventHandler<WoWContextEventArg> OnWoWContextChanged;

        internal static WoWContext LastWoWContext { get; set; }

        internal static WoWContext CurrentWoWContext
        {
            get
            {
                var map = StyxWoW.Me.CurrentMap;

                if (map.IsBattleground || map.IsArena)
                {
                    return WoWContext.Battlegrounds;
                }

                if (map.IsDungeon)
                {
                    return WoWContext.Instances;
                }

                return WoWContext.Normal;
            }
        }

        public override Composite CombatBehavior { get { return _combatBehavior; } }

        public override Composite CombatBuffBehavior { get { return _combatBuffsBehavior; } }

        public override Composite HealBehavior { get { return _healBehavior; } }

        public override Composite PreCombatBuffBehavior { get { return _preCombatBuffsBehavior; } }

        public override Composite PullBehavior { get { return _pullBehavior; } }

        public override Composite PullBuffBehavior { get { return _pullBuffsBehavior; } }

        public override Composite RestBehavior { get { return _restBehavior; } }

        private static bool IsMounted
        {
            get
            {
                switch (StyxWoW.Me.Shapeshift)
                {
                    case ShapeshiftForm.FlightForm:
                    case ShapeshiftForm.EpicFlightForm:
                        return true;
                }
                return StyxWoW.Me.Mounted;
            }
        }

        public override void OnButtonPress()
        {
            new ConfigurationForm().ShowDialog();
        }

        private ulong _lastTargetGuid = 0;

        public override void Pulse()
        {
            if (_lastTargetGuid != StyxWoW.Me.CurrentTargetGuid)
            {
                _lastTargetGuid = StyxWoW.Me.CurrentTargetGuid;
                // Don't print this shit if we don't need to. Kthx.
                if (_lastTargetGuid != 0)
                {
                    // Add other target switch info stuff here.

                    Logger.WriteDebug("Switched targets!");
                    Logger.WriteDebug("Melee Distance: " + Spell.MeleeRange);
                    Logger.WriteDebug("Health: " + StyxWoW.Me.CurrentTarget.MaxHealth);
                    Logger.WriteDebug("Level: " + StyxWoW.Me.CurrentTarget.Level);
                }
            }

            // Double cast shit
            Spell.DoubleCastPreventionDict.RemoveAll(t => DateTime.UtcNow.Subtract(t).TotalMilliseconds >= 2500);
            //Only pulse for classes with pets
            if(StyxWoW.Me.Class== WoWClass.Hunter 
                || StyxWoW.Me.Class==WoWClass.DeathKnight 
                || StyxWoW.Me.Class==WoWClass.Warlock
                || StyxWoW.Me.Class==WoWClass.Mage) PetManager.Pulse();

            if (HealerManager.NeedHealTargeting)
                HealerManager.Instance.Pulse();

            if (Group.MeIsTank && CurrentWoWContext != WoWContext.Battlegrounds && (Me.IsInParty || Me.IsInRaid))
                TankManager.Instance.Pulse();
        }

        public override void Initialize()
        {
            //Caching current class here to avoid issues with loading screens where Class return None and we cant build behaviors
            _myClass = Me.Class;

            Logger.Write("Starting Singular v" + Assembly.GetExecutingAssembly().GetName().Version);
            Logger.Write("Determining talent spec.");
            try
            {
                TalentManager.Update();
            }
            catch (Exception e)
            {
                StopBot(e.ToString());
            }
            Logger.Write("Current spec is " + TalentManager.CurrentSpec.ToString().CamelToSpaced());

            if (!CreateBehaviors())
            {
                return;
            }
            Logger.Write("Behaviors created!");

            // When we actually need to use it, we will.
            EventHandlers.Init();
            MountManager.Init();
            //Logger.Write("Combat log event handler started.");
        }

        public bool CreateBehaviors()
        {
            // let behaviors be notified if context changes.
            if (OnWoWContextChanged != null)
                OnWoWContextChanged(this, new WoWContextEventArg(CurrentWoWContext, LastWoWContext));

            //Caching the context to not recreate same behaviors repeatedly.
            LastWoWContext = CurrentWoWContext;

            // If these fail, then the bot will be stopped. We want to make sure combat/pull ARE implemented for each class.
            if (!EnsureComposite(true, BehaviorType.Combat, out _combatBehavior))
            {
                return false;
            }

            if (!EnsureComposite(true, BehaviorType.Pull, out _pullBehavior))
            {
                return false;
            }

            // If there's no class-specific resting, just use the default, which just eats/drinks when low.
            if (!EnsureComposite(false, BehaviorType.Rest, out _restBehavior))
            {
                Logger.Write("Using default rest behavior.");
                _restBehavior = Helpers.Rest.CreateDefaultRestBehaviour();
            }

            // These are optional. If they're not implemented, we shouldn't stop because of it.
            EnsureComposite(false, BehaviorType.CombatBuffs, out _combatBuffsBehavior);
            // This is a small bugfix. Just to ensure we always pop trinkets, etc.
            if (_combatBuffsBehavior == null)
                _combatBuffsBehavior = new PrioritySelector();
            EnsureComposite(false, BehaviorType.Heal, out _healBehavior);
            EnsureComposite(false, BehaviorType.PullBuffs, out _pullBuffsBehavior);
            EnsureComposite(false, BehaviorType.PreCombatBuffs, out _preCombatBuffsBehavior);



            // Since we can be lazy, we're going to fix a bug right here and now.
            // We should *never* cast buffs while mounted. EVER. So we simply wrap it in a decorator, and be done with it.
            // 4/11/2012 - Changed to use a LockSelector to increased performance.
            if (_preCombatBuffsBehavior != null)
            {
                _preCombatBuffsBehavior =
                    new Decorator(
                    ret => !IsMounted && !Me.IsOnTransport && !SingularSettings.Instance.DisableNonCombatBehaviors,
                        new LockSelector(
                            _preCombatBuffsBehavior));
            }

            if (_combatBuffsBehavior != null)
            {
                _combatBuffsBehavior = new Decorator(
                    ret => !IsMounted && !Me.IsOnTransport,
                    new LockSelector(
                    //Item.CreateUseAlchemyBuffsBehavior(),
                        Item.CreateUseTrinketsBehavior(),
                    //Item.CreateUsePotionAndHealthstone(SingularSettings.Instance.PotionHealth, SingularSettings.Instance.PotionMana),
                        _combatBuffsBehavior)
                    );
            }

            // There are some classes that uses spells in rest behavior. Basicly we don't want Rest to be called while flying.
            if (_restBehavior != null)
            {
                _restBehavior = new Decorator(
                    ret => !Me.IsFlying && !Me.IsOnTransport && !SingularSettings.Instance.DisableNonCombatBehaviors,
                    new LockSelector(
                        _restBehavior));
            }

            // Wrap all the behaviors with a LockSelector which basically wraps the child bahaviors with a framelock.
            // This will generally reduce the time it takes to pulse the behavior thus increasing performance of the cc
            if (_healBehavior != null)
            {
                _healBehavior = new LockSelector(
                    _healBehavior);
            }

            if (_pullBuffsBehavior != null)
            {
                _pullBuffsBehavior = new LockSelector(
                    _pullBuffsBehavior);
            }

            _combatBehavior = new LockSelector(
                _combatBehavior);

            _pullBehavior = new LockSelector(
                _pullBehavior);
            return true;
        }

        private bool EnsureComposite(bool error, BehaviorType type, out Composite composite)
        {
            Logger.WriteDebug("Creating " + type + " behavior.");
            int count = 0;
            composite = CompositeBuilder.GetComposite(_myClass, TalentManager.CurrentSpec, type, CurrentWoWContext, out count);
            if ((composite == null || count <= 0) && error)
            {
                StopBot(
                    string.Format(
                        "Singular currently does not support {0} for this class/spec combination, in this context! [{1}, {2}, {3}]", type, _myClass,
                        TalentManager.CurrentSpec, CurrentWoWContext));
                return false;
            }
            return composite != null;
        }

        private static void StopBot(string reason)
        {
            Logger.Write(reason);
            TreeRoot.Stop();
        }

        #region Nested type: LockSelector
        /// <summary>
        /// This behavior wraps the child behaviors in a 'FrameLock' which can provide a big performance improvement 
        /// if the child behaviors makes multiple api calls that internally run off a frame in WoW in one CC pulse.
        /// </summary>
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children)
                : base(children)
            {
            }
            public override RunStatus Tick(object context)
            {
                using (new FrameLock())
                {
                    return base.Tick(context);
                }
            }
        }
        #endregion

        #region Nested type: WoWContextEventArg
        public class WoWContextEventArg : EventArgs
        {
            public WoWContextEventArg(WoWContext currentContext, WoWContext prevContext)
            {
                CurrentContext = currentContext;
                PreviousContext = prevContext;
            }
            public readonly WoWContext CurrentContext;
            public readonly WoWContext PreviousContext;
        }
        #endregion
    }
}