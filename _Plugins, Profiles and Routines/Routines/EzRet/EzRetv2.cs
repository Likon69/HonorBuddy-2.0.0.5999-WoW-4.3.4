using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Logic.POI;

namespace EzRet
{
    class Paladin : CombatRoutine
    {


        public override sealed string Name { get { return "EzRet - " + ver; } }
        public override WoWClass Class { get { return WoWClass.Paladin; } }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }


        private Version ver = new Version(3, 4, 0);


        private bool Manual
        {
            get { return (BotManager.Current.Name != "LazyRaider" && BotManager.Current.Name != "Raid Bot"); }
        }


        public override bool NeedRest
        {
            get
            {
                return false;
            }
        }

        public override bool WantButton { get { return true; } }

        private Form _configForm;
        public override void OnButtonPress()
        {
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
                _configForm = new SettingsForm();

            _configForm.ShowDialog();
        }

        public override void Rest()
        {
           
        }

        public override bool NeedPullBuffs { get { return false; } }
        public override bool NeedCombatBuffs { get { return false; } }


        WoWUnit Target;



        //Config
        //private String SealName = "Seal of Truth";
        //private static int HEAL_THRESHOLD = 10;
        //private static int PULL_DISTANCE = 30;
        private static float HOF_THRESHOLD = 8.05f;
        //private static float LOH_THRESHOLD = 25.0f;
        //private static float DP_THRESHOLD = 60.0f;
        //private static bool DisableInq = true;
        //private static int InqRefreshDuration = 6;//In seconds
        //private static bool InstantHealsOnly = true;

        //You can modify these lists in order to affect how the bot acts
        List<String> Cleanse = new List<String> { "Aftermath", "Concussive Shot", "Slow", "Infected Wounds", "Freeze", "Frost Nova", "Piercing Howl", "Earthgrab", "Entangling Roots", "Frost Shock", "Entrapment", "Chains of Ice", "Chilled", "Shattered Barrier", "Cone of Cold", "Frostbolt" };
        WoWSpell Rebuke;
        public override void Combat()
        {
            Target = Me.CurrentTarget;

           
          int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;


            if (Target == null || !Target.Attackable)
            {
                return;
            }

            //Lets try some new logic here..lets go for the owner
            else if (Target != null && Target.IsPet && Manual)
            {
                WoWUnit Owner = Target.CreatedByUnit;
                if (Owner != null && Owner.IsValid && Owner.IsAlive)
                {
                    Blacklist.Add(Target, new TimeSpan(0, 0, 5));
                    Logging.Write("Changing targets to pet owner");
                    Target = Owner;
                    TargetUnit(Target);
                }
            }
            //Face the target
            Face(Target);
            //Always try and move ontop of the enemy target
            Move(Target.Location);

            if (Target.Distance < 2 && Manual)
                Navigator.PlayerMover.MoveStop();

            if ((Target.Distance > 30 || !Target.IsAlive) && Me.Combat && Manual)
            {
                Logging.Write(Target.Name + " is currently " + Target.Distance.ToString() + " dropping target");
                Me.ClearTarget();
                SeekTarget();
            }
            else if ((Target.HealthPercent <= 20d || isAuraActive("Avenging Wrath")) && CanCast("Hammer of Wrath"))
            {
                Cast("Hammer of Wrath");
            }
            else if (isAuraActive("The Art of War") && CanCast("Exorcism"))
            {
                Cast("Exorcism");
            }
            else if ((Target.Distance >= 5d || Target.Distance < 15d) && CanCast("Judgement"))
            {
                Cast("Judgement");
                Move(Target.Location);
            }
            else if (Target.Distance < 6)
            {
                foreach (String Ability in EzRetSettings.Instance.Cooldowns)
                {
                    cctc(Ability);
                }

                if (Target.IsCasting && Target.CanInterruptCurrentSpellCast)
                {
                    foreach (String Ability in EzRetSettings.Instance.KickRotation)
                    {

                        //New only use other abilities if rebuke off CD
                        if (!CanCast(Ability)) continue;
                        if (Ability.Equals("Rebuke"))
                        {
                            Cast("Rebuke");
                        }
                        else
                        {
                            if (Rebuke.CooldownTimeLeft.Seconds > 0 && Rebuke.CooldownTimeLeft.Seconds < 8)
                            {
                                Cast(Ability);
                            }
                        }
                    }
                }
                else if (Me.CurrentHolyPower == 3d || isAuraActive("Divine Purpose"))
                {

                    cctc("Zealotry");
                    if ((!isAuraActive("Inquisition") || Me.ActiveAuras["Inquisition"].TimeLeft.Seconds <= EzRetSettings.Instance.InqRefreshDuration) && !EzRetSettings.Instance.DisableInq)
                    {
                        Cast("Inquisition");
                    }
                    else
                    {
                        cctc("Templar's Verdict");
                    }

                }
                else
                {

                    foreach (String Ability in EzRetSettings.Instance.Rotation)
                    {
                        if (cctc(Ability))
                        {
                            break;
                        }
                    }
                }
                Usetrinkets();
            }


        }

        public void Usetrinkets()
        {
            foreach (Trinket t in EzRetSettings.Instance._Trinkets)
            {
                
                if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Name.Contains(t.TrinketName) && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown <= 0)
                {  
                    if (Target.HealthPercent <= t.Trigger)
                    {
                        if (string.IsNullOrEmpty(t.StackName))
                        {
                            Logging.Write("Trinket one");
                            StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                        }
                        else if (HasAuraStacks(t.StackName, t.StackNumber, Me))
                        {
                               Logging.Write("Trinket one");
                            StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                        }
                    }

                }
                else if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Name.Contains(t.TrinketName) && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown <= 0)
                {
                    if (Target.HealthPercent <= t.Trigger)
                    {
                        if (string.IsNullOrEmpty(t.StackName))
                        {
                            Logging.Write("Trinket two");
                            StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                        }
                        else if (HasAuraStacks(t.StackName, t.StackNumber, Me))
                        {
                            Logging.Write("Trinket two");
                            StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                        }
                    }
                }

            }
        }

        System.Timers.Timer Heartbeat;
        /// <summary>
        /// Gonna try something interesting, using a timer rather then pulse to check for target usage, as it seems sometimes pulse stops getting called
        /// </summary>
        public override void Initialize()
        {
            Logging.Write("Init gets called");
            EzRetSettings.Instance.Load();

            List<Trinket> Trinkat = new List<Trinket>();
            foreach (String t in EzRetSettings.Instance.Trinkets)
            {
                Trinkat.Add(new Trinket(t.ToString()));
            }
            EzRetSettings.Instance._Trinkets = Trinkat.ToArray();

            List<Act> Acts = new List<Act>();
            foreach (String t in EzRetSettings.Instance.HealRotation)
            {
                Acts.Add(new Act(t.ToString()));
            }
            EzRetSettings.Instance._HealRotation = Acts.ToArray();


            if (SpellManager.HasSpell("Rebuke"))
            {
                Rebuke = SpellManager.Spells["Rebuke"];
            }
            else
            {
                SpellManager.Spells.TryGetValue("Rebuke", out Rebuke);
            }


            Heartbeat = new System.Timers.Timer(200);
            Heartbeat.Elapsed += _timer_Elapsed;
            Heartbeat.Enabled = true;


            Logging.Write(Color.Turquoise,"Interested in a more advanced version? More features for PVE and PVP? Please check out my Vanguard release at http://www.ownedcore.com/forums/world-of-warcraft/world-of-warcraft-bots-programs/341367-vanguard-advanced-paladin-cc-honorbuddy-now-pvp.html");

            //base.Initialize();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!Me.Mounted && !Me.GotTarget && !Me.Dead)
            {
                SeekTarget();
            }
        }


        public bool HasAuraStacks(String aura, int stacks, WoWUnit unit)
        {
            if (unit.ActiveAuras.ContainsKey(aura))
            {
                return unit.ActiveAuras[aura].StackCount >= stacks;
            }
            return false;
        }


        /*public override void Pulse()
        {
            //Logging.Write("Pulse");
            if (!Me.IsActuallyInCombat && !Me.Mounted && !Me.GotTarget && !Me.Dead)
            {
                //Logging.Write("Pulse Pull");
                SeekTarget();
            }
        }*/

        void Cast(string Name)
        {
            Logging.Write(Name);
            SpellManager.Cast(Name);
        }

        bool CanCast(string Name)
        {
            return SpellManager.CanCast(Name);
        }
        /// <summary>
        /// Can cast, then cast
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        bool cctc(string Name)
        {
            if (SpellManager.CanCast(Name))
            {
                Logging.Write(Name);
                SpellManager.Cast(Name);
                return true;
            }
            return false;
        }

        bool cctc(WoWUnit Who, String Name)
        {
            if (SpellManager.CanCast(Name))
            {
                Logging.Write(Name + "@" + Who);
                SpellManager.Cast(Name, Who);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Taken from singulars PVP helper function file, and then modified
        /// </summary>
        /// <param name="unit"></param>
        protected void TargetUnit(WoWUnit unit)
        {
            Logging.Write("Targeting " + unit.Name);
            WoWMovement.ConstantFace(unit.Guid);
            BotPoi.Current = new BotPoi(unit, PoiType.Kill);
            Tar(unit);
        }

        public override void Pull()
        {

                
            int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;
                
            if (!Me.GotTarget || !Me.CurrentTarget.IsAlive)
            {
                SeekTarget();
            }
            else
            {
                cctc("Judgement");
                Move(Me.CurrentTarget.Location);
            }

        }

        private void SeekTarget()
        {

            if (BotManager.Current.Name != "BGBuddy" || !Battlegrounds.IsInsideBattleground)
                return;

            WoWPlayer unit = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false).
                Where(p => p.IsHostile && !p.IsTotem && !p.IsPet && !p.Dead && p.DistanceSqr <= (10 * 10)).
                OrderBy(u => u.HealthPercent).FirstOrDefault();

            if (unit == null)
            {
                unit = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false).Where(
                                       p => p.IsHostile && !p.IsTotem && !p.IsPet && !p.Dead && p.DistanceSqr <= (35 * 35)).OrderBy(
                                           u => u.DistanceSqr).FirstOrDefault();
            }
            if (unit != null)
            {
                TargetUnit(unit);
                Move(unit.Location);
            }


        }

        private void Move(WoWPoint loc)
        {
            if (Manual)
            {
                Navigator.MoveTo(loc);
            }
        }

        private void Tar(WoWUnit tar)
        {
            if (Manual)
            {
                tar.Target();
            }
        }
        private void Face(WoWUnit tar)
        {
            if (Manual)
            {
                tar.Face();
            }
        }



        
       

   
        public override void PreCombatBuff()
        {

            if (!isAuraActive(EzRetSettings.Instance.DPSSeal) && CanCast(EzRetSettings.Instance.DPSSeal))
            {
                Cast(EzRetSettings.Instance.DPSSeal);
            }
            else
            {
                cctc(Me, Buffcheck());
            }
        }


        public override bool NeedPreCombatBuffs
        {
            get
            {
                if (Me.Mounted)
                    return false;

                if (SpellManager.GlobalCooldown)
                    return true;

                if (!isAuraActive(EzRetSettings.Instance.DPSSeal) && CanCast(EzRetSettings.Instance.DPSSeal))
                    return true;


                return (Buffcheck() != null);
            }
        }

        public override bool NeedHeal
        {
            get
            {//(Me.IsFalling && CanCast("Hand of Protection")) || 
                return CleanseTime() || HealCheck() || (Me.MovementInfo.ForwardSpeed < 8.05 && CanCast("Hand of Freedom"));
            }
            
        }

        public override void Heal()
        {

            int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;

            if (Me.MovementInfo.ForwardSpeed < HOF_THRESHOLD && CanCast("Hand of Freedom"))
            {
                Cast("Hand of Freedom");
            }
            /*else if (Me.IsFalling && CanCast("Hand of Protection"))
            {
                Cast("Hand of Protection");
            }*/
            else if (CleanseTime())
            {
                cctc("Cleanse");
            }

            //Run this everytime
            foreach (Act action in EzRetSettings.Instance._HealRotation)
            {
                if ((Me.HealthPercent <= action.Trigger) && CanCast(action.Spell))
                {
                    Cast(action.Spell);
                }
            }

        }



        /// <summary>
        /// Return the name of the buff we need to use
        /// </summary>
        /// <returns></returns>
        private string Buffcheck()
        {
            if (!isAuraActive("Blessing of Might") && CanCast("Blessing of Might"))
            {
                return "Blessing of Might";
            }
            else if (isAuraActive("Blessing of Might"))
            {
                if (CanCast("Blessing of Kings") && Me.ActiveAuras["Blessing of Might"].CreatorGuid != Me.Guid && !isAuraActive("Blessing of Kings") && !isAuraActive("Mark of the Wild"))
                {
                    return "Blessing of Kings";
                }
            }
            else if (!SpellManager.HasSpell("Blessing of Might"))
            {
                if (CanCast("Blessing of Kings") && !isAuraActive("Blessing of Kings") && !isAuraActive("Mark of the Wild"))
                {
                    return "Blessing of Kings";
                }
            }


            return null;
        }



        /// <summary>
        /// Runs through all the checks in the healrotation
        /// </summary>
        /// <returns></returns>
        private bool HealCheck()
        {
            foreach (Act action in EzRetSettings.Instance._HealRotation)
            {
                if ((Me.HealthPercent <= action.Trigger) && CanCast(action.Spell))
                {
                    return true;
                }
            }
            return false;
        }
        private bool isAuraActive(string name)
        {
            //Me.Auras.con
            return Me.ActiveAuras.ContainsKey(name);
        }


        /// <summary>
        /// This function returns true only if the presence of one of the dispelable debuffs is applied
        /// if more the one debuff is applied we are probably being focus and shouldnt waste time dispelling
        /// Orginally this would return true once one debuff was detected
        /// </summary>
        /// <returns></returns>
        private bool CleanseTime()
        {


            if (Me.ManaPercent < 50)
                return false;

            if (Me.GotTarget)
            {
                if (Me.CurrentTarget.Distance < 5)
                {
                    return false;
                }
            }

            int Debuffs = 0;
            foreach (string spell in Cleanse)
            {
                if (isAuraActive(spell))
                {
                    Debuffs++;
                }
            }
            if (Debuffs <= 2 && Debuffs != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}