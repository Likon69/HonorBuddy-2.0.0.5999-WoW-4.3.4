using System;
using System.Collections.Generic;
using System.Linq;
using Styx.Combat.CombatRoutine;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Hera
{
    public partial class Codemplosion
    {
        // ************************************************************************************
        //
        public const string CCName = "Codemplosion Priest";                                   // Name of the CC displayed to the user
        public const string AuthorName = "Codemplosion";                                     // Part of the string used in the CC name
        private readonly Version _versionNumber = new Version(1, 0, 0);                 // Part of the string used in the CC name
        public const WoWClass CCClass = WoWClass.Priest;                                 // The class this CC will support
        // ************************************************************************************

        
        #region HB Start Up
        void BotEvents_OnBotStarted(EventArgs args)
        {
            // Finds the spec of your class: 0,1,2,3 and uses an enum to return something more logical
            ClassHelpers.Priest.ClassSpec = (ClassHelpers.Priest.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Priest.ClassSpec, Me.Class));

            // Do important stuff on LUA events
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", EventHandlers.CombatLogEventHander);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", TalentPointEventHander);
            Lua.Events.AttachEvent("PLAYER_TALENT_UPDATE", TalentPointEventHander);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", TalentPointEventHander);
            //Lua.Events.AttachEvent("PARTY_MEMBER_DISABLE", PlayerDeadEventHandler);
            

            Timers.Add("Pulse");          // Only do certain things in the Pulse check every 1 second
            Timers.Add("DistanceCheck");
            Timers.Add("FaceTarget");
            Timers.Add("EnvironmentSettings");
            Timers.Add("MassDispel");
            Timers.Add("ArcSmiteCombat");
            Timers.Add("HealingSpells"); // HB has an issue where it does not update health fast enough. This is a workaround to prevent heal spamming
            Timers.Add("SWPain");           // So we don't spam SWP on immune targets
            Timers.Add("VampiricTouch");    // VT is being cast more than it should. I don't think HB is seing this debuff properly
            Timers.Add("MapSpam");

            // Environmental Settings
            string environment = Utils.IsBattleground ? "PVP" : "PVE";
            environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
            ConfigSettings.CurrentEnvironment = environment;
            //Utils.Log(string.Format("*** Loading {0} settings.", environment),Utils.Colour("Red"));

            LoadSettings(false);

        }

        public static void TalentPointEventHander(object sender, LuaEventArgs args)
        {
            ClassHelpers.Priest.ClassSpec = (ClassHelpers.Priest.ClassType)Talents.Spec;
            Utils.Log(string.Format("You are a level {0} {1} {2}", Me.Level, ClassHelpers.Priest.ClassSpec, Me.Class));
        }

        public static void PlayerDeadEventHandler(object sender, LuaEventArgs args)
        {
            Utils.Log("************************* PLAY-YA DE-EDD");

            foreach (object arg in args.Args)
            {
                if (!(arg is String)) continue;
                var s = (string)arg;
                Utils.Log("*********************[PLAYED DEAD EVENT FIRED]**************************");
                Utils.Log(s);
                Utils.Log("*********************[PLAYED DEAD EVENT ENDED]**************************");

            }
        }

        // This event is fired each time you hit the Stop button in HB
        // Currently its only asigning FALSE to a variable, but you go do anything you want in here
        void BotEvents_OnBotStopped(EventArgs args)
        {
            //
        }
        #endregion

        #region Pulse
        public override void Pulse()
        {
            // HB runs this as frequenty as possible. I don't know the exact frequency but its supposed to be 5-10 times per second
            // Anything you want checked on a regular basis you may want to add here. 
            // For example buffing / healing random players

            base.Pulse();

            int lootableMobs = LootTargeting.Instance.LootingList.Count;

            // If Settings.DirtyData = true it will reload the settings from the XML file
            // This reads the XML file and re-populates the Settings class with any changed values
            if (!_isCCLoaded) { _isCCLoaded = true; Settings.DirtyData = true; }
            if (Settings.DirtyData) LoadSettings(true);

            // So we don't overload HB the below code is only run once per second
            if (!Timers.Expired("Pulse", 1000)) return;
            Timers.Recycle("Pulse", 1000);

            // Environmental Settings
            if (Timers.Expired("EnvironmentSettings", 5000))
            {
                if (Settings.MultipleEnvironment.Contains("never"))
                {
                    ConfigSettings.CurrentEnvironment = "PVE";
                }
                else
                {
                    Timers.Reset("EnvironmentSettings");
                    string environment = Utils.IsBattleground ? "PVP" : "PVE";
                    environment = ObjectManager.Me.IsInInstance ? "Instance" : environment;
                    if (!ConfigSettings.UIActive && environment != ConfigSettings.CurrentEnvironment)
                    {
                        ConfigSettings.CurrentEnvironment = environment;
                        Utils.Log(string.Format("*** Environment has changed. Loading {0} settings.", environment),Utils.Colour("Red"));
                        LoadSettings(false);
                    }
                }
            }

            // Evangelism / Archangel 
            if (!Me.Combat) Timers.Reset("ArcSmiteCombat");

            // Make sure we have a target - Instance only
            // Sometimes IB was not selecting a target when we were in combat. This fucked up things immensely!
            if (Me.IsInInstance && !Me.GotTarget)
            {
                WoWUnit tank = RAF.PartyTankRole;  
                if (tank != null && tank.GotTarget && tank.Combat) RAF.PartyTankRole.CurrentTarget.Target();
            }

            
            // Dispel Magic - You and all party members);
            if ((!Settings.Cleanse.Contains("never") || !Settings.PartyCleanse.Contains("never") )&& Spell.CanCast("Dispel Magic"))
            {
                List<int> urgentRemoval = new List<int> { 17173 };
                bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                if (urgentCleanse || CLC.ResultOK(Settings.Cleanse) || CLC.ResultOK(Settings.PartyCleanse))
                {
                    List<WoWDispelType> cureableList = new List<WoWDispelType> {WoWDispelType.Magic};
                    //if (Settings.SacredCleansing.Contains("... talented") && !cureableList.Contains(WoWDispelType.Magic)) { cureableList.Add(WoWDispelType.Magic); }

                    var p = ClassHelpers.Common.DecursePlayer(cureableList,CLC.ResultOK(Settings.PartyCleanse));
                    if (p != null)
                    {
                        if (Spell.CanCast("Dispel Magic")) Spell.Cast("Dispel Magic", p);
                    }
                }
            }

            // Cure Disease - You and all party members);
            if ((!Settings.Cleanse.Contains("never") || !Settings.PartyCleanse.Contains("never")) && Spell.CanCast("Cure Disease"))
            {
                List<int> urgentRemoval = new List<int> { 3427 };
                bool urgentCleanse = (from aura in Me.ActiveAuras from procID in urgentRemoval where procID == aura.Value.SpellId select aura).Any();

                if (urgentCleanse || CLC.ResultOK(Settings.Cleanse))
                {
                    List<WoWDispelType> cureableList = new List<WoWDispelType> { WoWDispelType.Disease };
                    var p = ClassHelpers.Common.DecursePlayer(cureableList, CLC.ResultOK(Settings.PartyCleanse));
                    if (p != null) { if (Spell.CanCast("Cure Disease")) Spell.Cast("Cure Disease", p); }
                }
            }

            // Buffs and such
            if (!Me.IsFlying && !Me.Mounted && !Me.Auras.ContainsKey("Altered Form"))
            {
                // Inner Fire/Will
                if (!Self.IsBuffOnMe("Inner Fire") && Settings.InnerFireWill.Contains("Inner Fire") && Spell.CanCast("Inner Fire")) Spell.Cast("Inner Fire", Me);
                if (!Self.IsBuffOnMe("Inner Will") && Settings.InnerFireWill.Contains("Inner Will") && Spell.CanCast("Inner Will")) Spell.Cast("Inner Will", Me);

                // Power Word Fortitude
                if (Me.IsInParty && !Me.Combat && CLC.ResultOK(Settings.PowerWordFortitude) && Spell.CanCast("Power Word: Fortitude"))
                {
                    const string buffName = "Power Word: Fortitude";
                    WoWUnit target = RAF.PartyMemberWithoutBuff(buffName);
                    if (target != null && !target.Auras.ContainsKey("Blood Pact")) Spell.Cast(buffName, target);
                }
                if (!Me.IsInParty && !Me.Combat && !Self.IsBuffOnMe("Power Word: Fortitude") && CLC.ResultOK(Settings.PowerWordFortitude) && Spell.CanCast("Power Word: Fortitude")) Spell.Cast("Power Word: Fortitude", Me);

                // Shadow Protection
                if (Me.IsInParty && !Me.Combat && CLC.ResultOK(Settings.ShadowProtection) && Spell.CanCast("Shadow Protection"))
                {
                    const string buffName = "Shadow Protection";
                    WoWUnit target = RAF.PartyMemberWithoutBuff(buffName);
                    if (target != null) Spell.Cast(buffName, target);
                }
                if (!Me.IsInParty && !Me.Combat && !Self.IsBuffOnMe("Shadow Protection") && CLC.ResultOK(Settings.ShadowProtection) && Spell.CanCast("Shadow Protection")) Spell.Cast("Shadow Protection", Me);

                // Vampiric Embrace
                if (!Self.IsBuffOnMe("Vampiric Embrace") && Spell.CanCast("Vampiric Embrace")) Spell.Cast("Vampiric Embrace");

                // Fear Ward
                if (!Self.IsBuffOnMe("Fear Ward") && CLC.ResultOK(Settings.FearWard) && Spell.CanCast("Fear Ward")) Spell.Cast("Fear Ward", Me);
            }

            // Shadowform - can be applied while flying and mounted
            //if (CLC.ResultOK(Settings.Shadowform) && !Self.IsBuffOnMe("Shadowform") && Self.IsHealthPercentAbove(Settings.RenewHealth) && Spell.CanCast("Shadowform")) Spell.Cast("Shadowform");
            if (CLC.ResultOK(Settings.Shadowform) && !Self.IsBuffOnMe(15473, Self.AuraCheck.AllAuras) && !Me.Mounted && Self.IsHealthPercentAbove(Settings.RenewHealth) && Spell.CanCast("Shadowform")) Spell.Cast("Shadowform");


            // Prayer of Healing
            if (!Me.Mounted && !Me.IsFlying && !Me.Combat && (Me.IsInParty || Me.IsInRaid))
            {
                int PoHCount = Convert.ToInt16(Settings.PrayerOfHealingCount);
                List<WoWPlayer> myPartyOrRaidGroup = Me.PartyMembers;
                List<WoWPlayer> PoH = (from o in myPartyOrRaidGroup let p = o.ToPlayer() where p.Distance < 30 && !p.Dead && !p.IsGhost && p.InLineOfSight && p.HealthPercent < Settings.PrayerOfHealingHealth select p).ToList();
                if (PoH.Count >= PoHCount && Spell.CanCast("Prayer of Healing"))
                {
                    Spell.Cast("Prayer of Healing");
                    Utils.LagSleep();
                    Utils.WaitWhileCasting();
                }
                
                // Everyone else in the party gets healed
                WoWUnit healTarget = RAF.HealPlayer(95, 40); 
                if (healTarget != null && !healTarget.Dead) ClassHelpers.Priest.PartyHealer(healTarget);
            }


            // Archangel - Use it or loose it
            if (!Me.Mounted && !Me.IsFlying && (!Me.Auras.ContainsKey("Drink") || !Me.Auras.ContainsKey("Food")) && Me.ActiveAuras.ContainsKey("Evangelism") && !Me.Combat)
            {
                const string spell = "Evangelism";
                const string archangel = "Archangel";
                double getTime = Convert.ToDouble(Self.GetTimeLUA());
                double buffTime = Convert.ToDouble(Self.BuffTimeLeftLUA(spell));
                double secondsRemaining = buffTime - getTime;

                if (secondsRemaining < 4.5 && Spell.CanCastLUA(archangel))
                {
                    Utils.Log("-Evangelism buff about to expire. Casting Archangel buff to consume it", Utils.Colour("Red"));
                    Spell.Cast(archangel);
                }
            }
            

            // Resurrection
            if (Settings.ResurrectPlayers.Contains("always") && !Me.Combat && Spell.CanCast("Resurrection") && !Me.IsCasting)
            {
                foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.Dead && !p.IsGhost && p.InLineOfSight))
                {
                    if (Timers.Exists(p.Guid.ToString()) && !Timers.Expired(p.Guid.ToString(),15000)) continue;

                    Spell.Cast("Resurrection", p);
                    Utils.LagSleep();
                    Timers.Add(p.Guid.ToString());  // Prevent spamming resurrection on th same target
                    System.Threading.Thread.Sleep(1500);
                    if (!Me.IsCasting) Spell.StopCasting();
                    while (Me.IsCasting)
                    {
                        if (!p.Dead)
                        {
                            Utils.Log("-Emmm.... it appears our dead party member is now alive. So why are we still trying to rez them?");
                            Spell.StopCasting();
                        }
                    }
                    break;
                }
            }

            // Clean up timers
            foreach (WoWPlayer p in Me.PartyMembers.Where(p => p.IsAlive && Timers.Exists(p.Guid.ToString())))
            {
                Timers.Remove(p.Guid.ToString());
            }

            

        }


        #endregion
    }
}
