using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
using TreeSharp;
namespace EzFrost
{
    class DeathKnight : CombatRoutine
    {


        public override sealed string Name { get { return "EzFrost - " + ver; } }
        public override WoWClass Class { get { return WoWClass.DeathKnight; } }
        private static LocalPlayer Me { get { return ObjectManager.Me; } }
        public override bool WantButton { get { return true; } }

        private Version ver = new Version(1, 4, 1);

        public override bool NeedRest
        {
            get
            {
                return false;
            }
        }



        public override void Rest()
        {

        }

        public override bool NeedPullBuffs { get { return false; } }
        public override bool NeedCombatBuffs { get { return false; } }

        private Form _configForm;
        public override void OnButtonPress()
        {
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
                _configForm = new EzFrostSettingsWindow();

            _configForm.ShowDialog();
        }

        WoWUnit Target;



        private bool Manual
        {
            get { return (BotManager.Current.Name != "LazyRaider" && BotManager.Current.Name != "Raid Bot"); }
        }

        //You can modify these lists in order to affect how the bot acts
        //List<String> Cleanse = new List<String> { "Aftermath", "Concussive Shot", "Slow", "Infected Wounds", "Freeze", "Frost Nova", "Piercing Howl", "Earthgrab", "Entangling Roots", "Frost Shock", "Entrapment", "Chains of Ice", "Chilled", "Shattered Barrier", "Cone of Cold", "Frostbolt" };
        //List<String> Rotation = new List<String> { "Frost Strike", "Obliterate", "Howling Blast" };
        /*List<String> Rotation = new List<String> { "Necrotic Strike", "Howling Blast", "Frost Strike" };
        List<String> RangedRotation = new List<String> { "Death Grip", "Howling Blast", "Death Coil" };
        List<String> Interrupts = new List<String> { "Mind Freeze", "Strangulate", "Arcane Torrent" };
        List<String> CD = new List<String> { "Horn of Winter", "Blood Tap", "Outbreak", "Pillar of Frost", "Raise Dead" };
        List<Act> HealRotation = new List<Act> { new Act("Lichborne", 30.0f), new Act("Icebound Fortitude", 50.0f), new Act("Anti-Magic Shell", 60.0f) };
        List<Trinket> Trinkets = new List<Trinket> { new Trinket("Badge of Victory", null, 0, 100.0f), new Trinket("Essence of the Eternal Flame", null, 0, 100.0f), new Trinket("Apparatus of Khaz'goroth", "Titanic Power", 5, 60.0f), new Trinket("Fury of Angerforge", "Raw Fury", 5, 60.0f) };
        float LichbornHealPercent = 50.0f;*/

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
            else if (Target != null && Target.IsPet)
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


            //Always try and move ontop of the enemy target..unless were casting
            if (!Me.IsCasting)
            {
                Move(Target.Location);
            }


            if (Target.Distance < 2 && Manual)
                Navigator.PlayerMover.MoveStop();


            if (Me.IsCasting)
            {
            }
            else if ((Target.Distance > 30 || !Target.IsAlive) && Me.Combat && Manual)
            {
                Logging.Write(Target.Name + " is currently " + Target.Distance.ToString() + " dropping target");
                Me.ClearTarget();
                SeekTarget();
            }
            else if (isAuraActive("Freezing Fog") && CanCast("Howling Blast"))
            {
                Cast("Howling Blast");
            }
            else if ((Target.Distance >= 5d && Target.Distance < 20d))
            {
                if (Target.MovementInfo.ForwardSpeed > 7.0f && CanCast("Chains of Ice"))
                {
                    Cast("Chains of Ice");
                }
                else
                {
                    foreach (String Ability in EzFrostSettings.Instance.RangedRotation)
                    {
                        cctc(Target, Ability);
                    }
                }
                Move(Target.Location);
            }

            else if (Target.IsWithinMeleeRange)
            {

                foreach (String Ability in EzFrostSettings.Instance.Cooldowns)
                {
                    cctc(Ability);
                }



                if (EzFrostSettings.Instance.EmpowerRuneWeapon && Me.DeathRuneCount == 0 && Me.UnholyRuneCount == 0 && Me.FrostRuneCount == 0)
                {
                    cctc("Empower Rune Weapon");
                }


                if (Target.IsCasting && Target.CanInterruptCurrentSpellCast)
                {
                    foreach (String Ability in EzFrostSettings.Instance.KickRotation)
                    {
                        if (cctc(Ability))
                        {
                            Thread.Sleep(50);
                            break;
                        }
                    }
                }
                else if ((Me.HealthPercent < 30.0f || isAuraActive("Dark Succor")) && CanCast("Death Strike") && Me.HealthPercent < 80.0f)
                {
                    Cast("Death Strike");
                }
                else
                {
                    if (isAuraActive("Killing Machine") && (CanCast("Obliterate") || CanCast("Frost Strike")))
                    {
                        //If the target has both disaeses then obl, otherwise frost strike
                        if (CanCast("Obliterate"))
                        {
                            Cast("Obliterate");
                        }
                        else
                        {
                            Cast("Frost Strike");
                        }
                    }

                    foreach (String Ability in EzFrostSettings.Instance.Rotation)
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
            foreach (Trinket t in EzFrostSettings.Instance._Trinkets)
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
            EzFrostSettings.Instance.Load();

            List<Trinket> Trinkat = new List<Trinket>();
            foreach (String t in EzFrostSettings.Instance.Trinkets)
            {
                Trinkat.Add(new Trinket(t.ToString()));
            }
            EzFrostSettings.Instance._Trinkets = Trinkat.ToArray();

            List<Act> Acts = new List<Act>();
            foreach (String t in EzFrostSettings.Instance.HealRotation)
            {
                Acts.Add(new Act(t.ToString()));
            }
            EzFrostSettings.Instance._HealRotation = Acts.ToArray();


            Heartbeat = new System.Timers.Timer(200);
            Heartbeat.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            Heartbeat.Enabled = true;
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

        void Cast(string Name)
        {
            Logging.Write(Name);
            SpellManager.Cast(Name);
        }

        bool CanCast(string Name)
        {
            return SpellManager.CanCast(Name);
        }
        bool CanCast(WoWUnit Target, string Name)
        {
            return SpellManager.CanCast(Name, Target);
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
            else
            {
                return false;
            }
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
                foreach (String Ability in EzFrostSettings.Instance.RangedRotation)
                {
                    cctc(Me.CurrentTarget, Ability);
                }
                foreach (String Ability in EzFrostSettings.Instance.Rotation)
                {
                    if (cctc(Ability))
                    {
                        break;
                    }
                }

                Cast("Attack");
                Move(Me.CurrentTarget.Location);
            }

        }

        private void SeekTarget()
        {
            if (Styx.BotManager.Current.Name != "BGBuddy")
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
                Target = tar;
                tar.Target();
                WoWMovement.ConstantFace(tar.Guid);
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
            cctc(Me, Buffcheck());
        }


        public override bool NeedPreCombatBuffs
        {
            get
            {
                if (Me.Mounted)
                    return false;

                return (Buffcheck() != null);
            }
        }

        public override bool NeedHeal
        {
            get
            {
                return HealCheck();
            }
        }

        public override void Heal()
        {

            int Toggle = Lua.GetReturnVal<int>("return Toggle and 0 or 1", 0);

            if (Toggle != 1)
                return;


            if (isAuraActive("Lichborne") && Me.HealthPercent <= EzFrostSettings.Instance.LichborneHealStop && CanCast(Me, "Death Coil"))
            {
                cctc(Me, "Death Coil");
            }

            if (Me.HealthPercent <= EzFrostSettings.Instance.HealthStone)
            {
                WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512);
                if (hs != null)
                {
                    hs.Use();
                }

            }


            foreach (Act action in EzFrostSettings.Instance._HealRotation)
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
            if (!isAuraActive("Battle Shout") && !isAuraActive("Horn of Winter") && !isAuraActive("Strength Of Earth Totem") && !isAuraActive("Roar of Courage") && !Me.Mounted)
            {
                return "Horn of Winter";
            }

            if (EzFrostSettings.Instance.UnholyPresence && !Me.Auras.ContainsKey("Unholy Presence"))
            {
                return "Unholy Presence";
            }

            return null;
        }



        /// <summary>
        /// Runs through all the checks in the healrotation
        /// </summary>
        /// <returns></returns>
        private bool HealCheck()
        {
            foreach (Act action in EzFrostSettings.Instance._HealRotation)
            {
                if ((Me.HealthPercent <= action.Trigger) && CanCast(action.Spell))
                {
                    return true;
                }
            }

            if (isAuraActive("Lichborne") && Me.HealthPercent <= EzFrostSettings.Instance.LichborneHealStop && CanCast(Me, "Death Coil"))
            {
                return true;
            }

            if (Me.HealthPercent <= EzFrostSettings.Instance.HealthStone)
            {
                WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512);
                if (hs != null)
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



    }
}