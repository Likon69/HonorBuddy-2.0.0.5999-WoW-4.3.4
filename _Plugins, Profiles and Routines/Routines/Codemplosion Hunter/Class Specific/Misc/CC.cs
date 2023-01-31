using System;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Hera
{
    public partial class Codemplosion
    {
        // ************************************************************************************
        public const string CCName = "Codemplosion Hunter";                                      // Name of the CC displayed to the user
        public const string AuthorName = "Deathburn";                                         // Part of the string used in the CC name
        private readonly Version _versionNumber = new Version(1, 0, 0);                     // Part of the string used in the CC name
        public const WoWClass CCClass = WoWClass.Hunter;                                    // The class this CC will support
        // ************************************************************************************
        
        #region HB Start Up
        void BotEvents_OnBotStarted(EventArgs args)
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.Hunter.ClassSpec = (ClassHelpers.Hunter.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Hunter.ClassSpec, Me.Class));

            // Do important stuff on LUA events
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", EventHandlers.CombatLogEventHander);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", EventHandlers.TalentPointEventHander);
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", EventHandlers.TalentPointEventHander);

            // Load the settings from the XML file
            LoadSettings(false);

            Timers.Add("Pulse");
            Timers.Add("Cheetah");

        }

        // This event is fired each time you hit the Stop button in HB
        // Currently its only asigning FALSE to a variable, but you go do anything you want in here
        void BotEvents_OnBotStopped(EventArgs args)
        {
            // Nothing here
        }
        #endregion

        #region Pulse
        public override void Pulse()
        {
            //base.Pulse();

            // If Settings.DirtyData = true it will reload the settings from the XML file
            // This reads the XML file and re-populates the Settings class with any changed values
            if (!_isCCLoaded)
            {
                _isCCLoaded = true;
                Settings.DirtyData = true;
            }
            if (Settings.DirtyData) LoadSettings(true);

            // So we don't overload HB the below code is only run once per second
            if (!Timers.Expired("Pulse", 1000)) return;
            Timers.Reset("Pulse");

            if (Me.Combat) Timers.Reset("Cheetah");

            // Call and Heal Pet
            if (ClassHelpers.Hunter.Pet.NeedToFeedPet) ClassHelpers.Hunter.Pet.FeedPet();
            if (ClassHelpers.Hunter.Pet.NeedToHealPet) ClassHelpers.Hunter.Pet.HealPet();
            if (!Settings.PetSlot.Contains("never") && ClassHelpers.Hunter.Pet.NeedToCallPet) ClassHelpers.Hunter.Pet.CallPet();

            // Aspect of the Cheetah, Thundercats are go!
            if (ClassHelpers.Hunter.Aspect.CanUseAspectOfTheCheetah)
            {
                if (!Timers.Expired("Cheetah", 5000)) return;
                Timers.Reset("Cheetah");
                ClassHelpers.Hunter.Aspect.AspectOfTheCheetah();
            }

            // Consume Frenzy Effect so we don't loose the opportunity to use Focus Fire
            if (Settings.ConsumeFrenzyEffect.Contains("always") && Me.GotAlivePet)
                if (Me.Pet.Auras.ContainsKey("Frenzy Effect") && !Settings.FocusFire.Contains("never") && Me.Pet.Auras["Frenzy Effect"].TimeLeft.Seconds <= 2 && Spell.CanCast("Focus Fire"))
                {
                    Utils.Log(String.Format("Consuming Pet's Frenzy Effect ({0} stacks) with Focus Fire. Lets not waste it",Me.Pet.Auras["Frenzy Effect"].StackCount), Utils.Colour("Red"));
                    Spell.Cast("Focus Fire");
                }


            // Lets keep the fun going! Have the pet attack mobs while we're looting.
            if (CLC.ResultOK(Settings.ContinuousPulling))
            {
                int lootableMobs = LootTargeting.Instance.LootingList.Count;
                if (!Me.Combat && Self.IsHealthPercentAbove(Settings.RestHealth) && Me.GotAlivePet && !ClassHelpers.Hunter.Pet.NeedToHealPet && !Me.Pet.GotTarget && lootableMobs > 0)
                {
                    WoWUnit p = Utils.AttackableMobInRange(LevelbotSettings.Instance.PullDistance);

                    if (p != null)
                    {
                        p.Target();
                        Utils.Log(string.Format("Sending pet to attack {0} while we loot", p.Class), Utils.Colour("Red"));
                        if (Target.CanDebuffTarget("Hunter's Mark") && CLC.ResultOK(Settings.HuntersMark)) Spell.Cast("Hunter's Mark");
                        ClassHelpers.Hunter.Pet.Attack();
                    }
                }
            }
        }
#endregion
    }
}
