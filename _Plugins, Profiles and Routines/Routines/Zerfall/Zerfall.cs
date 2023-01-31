using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Zerfall.Talents;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;
using Color = System.Drawing.Color;
using Sequence = TreeSharp.Sequence;
namespace Zerfall
{
    public partial class Zerfall : CombatRoutine
    {
        private readonly Version _version = new Version(1, 2, 3);
        private readonly TalentManager _talentManager = new TalentManager();
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public override string Name { get { return "Zerfall " + _version; } }
        public override WoWClass Class { get { return WoWClass.Warlock; } }
        public new double? PullDistance
        {
            get
            {

                if (SpellManager.HasSpell(CurrentPullSpell))
                {
                    return SpellManager.Spells[CurrentPullSpell].MaxRange;
                }
                else
                    return 39;
            }
        }


        public Stopwatch StopSpellTimer = new Stopwatch();
        public override void Pulse()
        {

            if (!ZerfallSettings.Instance.MoveDisable && Me.Combat && Me.IsMoving && Me.CurrentTarget != null && !Me.IsFacing(Me.CurrentTarget))
            {
                Log("Im in Combat But Still Running away, Manualy Stoping to do Combat");
                if (Me.Mounted)
                {
                    Styx.Logic.Mount.Dismount();
                }
                WoWMovement.MoveStop();
            }
            if (!PetTimerCombat.IsRunning && Me.Combat)
            {
                PetTimerCombat.Reset();
                PetTimerCombat.Start();
            }
            if (PetTimerCombat.IsRunning && !Me.Combat)
            {
                PetTimerCombat.Reset();
                PetTimerCombat.Stop();
            }
            if (Me.IsValid)
            {
                
                if (!Me.Combat && Me.GotAlivePet)
                {
                 
                    if (!StopSpellTimer.IsRunning || StopSpellTimer.Elapsed.Seconds > 5)
                    {
                        //Lua.DoString("SpellStopTargeting()");
                        StopSpellTimer.Reset();
                        StopSpellTimer.Start();
                    }
                }
            }
        }

        public override void Initialize()
        {
            Logging.Write(Color.DarkSlateBlue, "------------------------------------------------------------------------------------------------------------------");
            Logging.Write(Color.DarkSlateBlue, "[{0}] is a BT Warlock Routine.!", Name);
            Logging.Write(Color.DarkSlateBlue, "Localplayer is a, level {0} {1} {2}-{3} currently in {4}", Me.Level, Me.Race, _talentManager.Spec, Me.Class, Me.RealZoneText);
            Logging.Write(Color.DarkSlateBlue, "------------------------------------------------------------------------------------------------------------------");
            Targeting.Instance.IncludeTargetsFilter += IncludeTargetsFilter;
            ZerfallSettings.Instance.Load();
            Log("Loading Saved Setttings");
            Lua.Events.AttachEvent("COMBAT_LOG_EVENT", CombatLogEventHander);
            Lua.Events.AttachEvent("UI_ERROR_MESSAGE", UIErrorEventHander);
            if (ZerfallSettings.Instance.IsConfigured == false)
            {
                Logging.Write(Color.Red,
                              "You have not configured Zerfall, Configuration is needed for this CC to work, Please Configure your Settings now");
            }
              
        }

        private static void UIErrorEventHander(object sender, LuaEventArgs args)
        {
            string Error = args.Args[0].ToString();
            
            if (Error == "Target not in line of sight" || Error == "Ziel ist nicht im Sichtfeld")
            {
                if (!ZerfallSettings.Instance.MoveDisable && Me.CurrentTarget != null && Navigator.CanNavigateFully(Me.Location, Me.CurrentTarget.Location))
                {
                    Logging.Write("Target not in Line of Sight to {0} Blacklisting for 10 sec to allow Repositioning. ", Me.CurrentTarget.Name);
                    Blacklist.Add(StyxWoW.Me.CurrentTargetGuid, TimeSpan.FromSeconds(10));
                    StyxWoW.Me.ClearTarget();
                }
                if (!ZerfallSettings.Instance.MoveDisable && Me.CurrentTarget != null && !Navigator.CanNavigateFully(Me.Location, Me.CurrentTarget.Location))
                {
                    Logging.Write("Cant Navigate to {0} Blacklisting for 3min", Me.CurrentTarget.Name);
                    Blacklist.Add(StyxWoW.Me.CurrentTargetGuid, TimeSpan.FromMinutes(3));
                    StyxWoW.Me.ClearTarget();
                    
                }
            }
        }

        public static ulong LastTarget;
        public static int EvadeCount;
        private static void CombatLogEventHander(object sender, LuaEventArgs args)
        {
            foreach (object arg in args.Args)
            {
                if (arg is String)
                {
                    var s = (string)arg;
                    if (s.ToUpper() == "EVADE" || s.ToUpper() == "ENTKOMMEN")
                    {
                        if (!ZerfallSettings.Instance.MoveDisable)
                        {
                            if (StyxWoW.Me.GotTarget && !IsBattleGround())
                            {
                                if (LastTarget == null || Me.CurrentTarget.Guid != LastTarget) //Meaning they are not the same. 
                                {
                                    LastTarget = Me.CurrentTarget.Guid; // set guid to current target. 
                                    EvadeCount = 1; //it didnt match last target and already evaded once. 
                                    Logging.Write("Target Evaded {0} Times", EvadeCount.ToString());
                                }
                                else
                                {
                                    EvadeCount++;
                                    Logging.Write("Target Evaded {0} Times", EvadeCount.ToString());
                                    if (EvadeCount >= 3)
                                    {
                                        Logging.Write("Target Evaded {0} Times", EvadeCount.ToString());
                                        Logging.Write("Target is Evade bugged.");
                                        Logging.Write("Blacklisting for 3 hours");
                                        Blacklist.Add(StyxWoW.Me.CurrentTargetGuid, TimeSpan.FromHours(3));
                                        StyxWoW.Me.ClearTarget();
                                    }
                                }

                            }
                            else
                                if (StyxWoW.Me.GotTarget && (IsBattleGround() || !Me.IsInInstance))
                                {
                                    Logging.Write("My target is Evade bugged.");
                                    Logging.Write("Blacklisting for 1 Minute");
                                    Blacklist.Add(StyxWoW.Me.CurrentTargetGuid, TimeSpan.FromMinutes(1));
                                    StyxWoW.Me.ClearTarget();
                                }
                        }
                    }
                }
            }
        }

        public override bool WantButton { get { return true; } }
    
        private Form _configForm;
        public override void OnButtonPress()
        {
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
                _configForm = new ZerfallConfig();
            
            _configForm.ShowDialog();
        }
         
    }

    public partial class Zerfall
    {
        private static string _logSpam;
        private static void Log(string format, params object[] args)
        {
            
            
            string s = Utilities.FormatString(format, args);

            if (s != _logSpam)
            {
                if (ZerfallSettings.Instance.MoveDisable == true)
                {
                    Logging.Write(Color.DarkGreen, "[ZerFall]: {0}", string.Format(format, args));
                }
                else
                {
                    Logging.Write(Color.DarkSlateBlue, "[ZerFall]: {0}", string.Format(format, args));
                }
                _logSpam = s;
            }
        }

        private static void Log(string format)
        {
            Log(format, new object());
            /*foreach (KeyValuePair<string, WoWAura> pair in Me.Auras)
            {
                WoWAura curAura = pair.Value;
                if (curAura.SpellId == 1234)
                {
                    Lua.DoString("CancelUnitBuff('player', '"+ curAura.Name+"')");   
                }
            }*/
            //foreach (WoWSpell Spell in SpellManager.RawSpells)
           // {
                //Logging.Write("public static int " + Spell.Name + " = " + Spell.Id + ";");
            //}
        }

        #region Behavior Tree Composite Helpers
        public static int LastCast;

        public delegate WoWUnit UnitSelectDelegate(object context);
        public Composite CreateBuffCheckAndCast(string name)
        {
            return new Decorator(ret => SpellManager.CanBuff(name),
                                 new Action(ret => SpellManager.Buff(name)));
        }

        public Composite CreateBuffCheckAndCast(string name, UnitSelectDelegate onUnit)
        {
            return new Decorator(ret => SpellManager.CanBuff(name, onUnit(ret)),
                                 new Action(ret => SpellManager.Buff(name, onUnit(ret))));
        }

        public Composite CreateBuffCheckAndCast(string name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanBuff(name),
                                 new Action(ret => SpellManager.Buff(name)));
        }
        public Composite CreateBuffCheckAndCast(string name, bool OnMe)
        {
            return new Decorator(ret => !Me.Auras.ContainsKey(name),
                                  new Action(ret => SpellManager.Buff(name)));
        }
        public Composite CreateBuffCheckAndCast(string name, UnitSelectDelegate onUnit, CanRunDecoratorDelegate extra)
        {
            return CreateBuffCheckAndCast(name, onUnit, extra, false);
        }

        public Composite CreateBuffCheckAndCast(string name, UnitSelectDelegate onUnit, CanRunDecoratorDelegate extra, bool targetLast)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanBuff(name, onUnit(ret)),
                                 new Action(ret => SpellManager.Buff(name, onUnit(ret))));
        }

        public static Composite CreateSpellCheckAndCast(string name)
        {
            return new Decorator(ret => SpellManager.HasSpell(name) && SpellManager.CanCast(name),
                                 new Sequence(
                                     new Action(ret => SpellManager.Cast(name)),
                                     new Action(ret => LastCast = SpellManager.Spells[name].Id),
                                     new Action(ctx => Log("Casting {0}", name)))

                                 );
        }

        public Composite CreateSpellCheckAndCast(string name, WoWUnit onUnit)
        {
            return new Decorator(ret => SpellManager.HasSpell(name) && SpellManager.CanCast(name),
                 new Sequence(
                                     new Action(ret => SpellManager.Cast(name, onUnit)),
                                     new Action(ret => LastCast = SpellManager.Spells[name].Id),
                                     new Action(ctx => Log("Casting {0}", name)))
                                 );
        }

        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => SpellManager.HasSpell(name) && extra(ret) && SpellManager.CanCast(name),
                 new Sequence(
                                     new Action(ret => SpellManager.Cast(name)),
                                     new Action(ret => LastCast = SpellManager.Spells[name].Id),
                                     new Action(ctx => Log("Casting {0}", name)))
                );
        }

        public Composite CreateSpellCheckAndCast(string name, bool checkRange)
        {
            return new Decorator(ret => SpellManager.CanCast(name, checkRange) && SpellManager.HasSpell(name),
                                  new Sequence(
                                     new Action(ret => SpellManager.Cast(name)),
                                     new Action(ret => LastCast = SpellManager.Spells[name].Id),
                                     new Action(ctx => Log("Casting {0}", name)))
                                     );
        }

        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra, ActionDelegate extraAction)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanCast(name) && SpellManager.HasSpell(name),
                                 new Action(delegate(object ctx)
                                 {
                                     
                                     SpellManager.Cast(name);
                                     LastCast = SpellManager.Spells[name].Id;
                                     extraAction(ctx);
                                     return RunStatus.Success;
                                 }));
        }
        public Composite CreateSpellCheckAndCast(string name, string Buffname)
        {

            return new Decorator(ret => SpellManager.CanCast(name) && SpellManager.HasSpell(name),
                                 new Action(delegate
                                 {
                                     
                                     SpellManager.Cast(name);
                                     LastCast = SpellManager.Spells[name].Id;
                                     StyxWoW.SleepForLagDuration();
                                     if (Me.CurrentTarget.Auras.ContainsKey(Buffname))
                                     {
                                         SpellManager.StopCasting();
                                     }
                                     return RunStatus.Success;
                                 })
                                 );
        }
        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra, ActionDelegate extraAction, WoWUnit onUnit)
        {
            return new Decorator(ret => extra(ret) && SpellManager.CanCast(name) && SpellManager.HasSpell(name),
                                 new Action(delegate(object ctx)
                                 {
                                     
                                     SpellManager.Cast(name, onUnit);
                                     LastCast = SpellManager.Spells[name].Id;
                                     extraAction(ctx);
                                     return RunStatus.Success;
                                 }));
        }
        public Composite Cast(string name)
        {
            return new Decorator(ret => SpellManager.CanCast(name) && SpellManager.HasSpell(name),
                 new Sequence(
                     
                                     new Action(ret => SpellManager.Cast(name)),
                                     new Action(ret => LastCast = SpellManager.Spells[name].Id),
                                     new Action(ctx => Log("Casting {0}", name)))
                                 );
        }
        public Composite PetSpellCheckAndCast(string name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && PetManager.CanCastPetAction(name),
                                 new Action(delegate(object ctx)
                                 {

                                     PetManager.CastPetAction(name, Me.CurrentTarget);
                                     Log("Casting Pet Action {0}", name);
                                     
                                     return RunStatus.Success;
                                 }));
        }

        public Composite SummonPet(string PetSpellName)
        {

            return new Decorator(ret => SpellManager.CanCast(PetSpellName) && SpellManager.HasSpell(PetSpellName),
                                 new Action(delegate
                                 {
                                     if (PetSpellName != "Summon Automatic")
                                     {
                                         SpellManager.Cast(PetSpellName);
                                         StyxWoW.SleepForLagDuration();
                                     }
                                     if (PetSpellName == "Summon Automatic")
                                     {
                                         switch (_talentManager.Spec)
                                         {
                                                 case WarlockTalentSpec.Lowbie:
                                                 if (SpellManager.HasSpell(SummonImp))
                                                 {
                                                     SpellManager.Cast(SummonImp);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 break;
                                                 case WarlockTalentSpec.Affliction:
                                                 if (SpellManager.HasSpell(SummonImp))
                                                 {
                                                     SpellManager.Cast(SummonImp);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 if (SpellManager.HasSpell(SummonImp) && SpellManager.HasSpell(SummonVoidwalker))
                                                 {
                                                     SpellManager.Cast(SummonVoidwalker);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 break;
                                                 case WarlockTalentSpec.Demonology:
                                                 if (SpellManager.HasSpell(SummonImp) && !SpellManager.HasSpell(SummonVoidwalker) && !SpellManager.HasSpell(SummonFelguard))
                                                 {
                                                     SpellManager.Cast(SummonImp);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 if (SpellManager.HasSpell(SummonImp) && SpellManager.HasSpell(SummonVoidwalker) && !SpellManager.HasSpell(SummonFelguard))
                                                 {
                                                     SpellManager.Cast(SummonVoidwalker);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 if (SpellManager.HasSpell(SummonImp) && SpellManager.HasSpell(SummonVoidwalker) && SpellManager.HasSpell(SummonFelguard))
                                                 {
                                                     SpellManager.Cast(SummonFelguard);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 break;
                                                 case WarlockTalentSpec.Destruction:
                                                 if (SpellManager.HasSpell(SummonImp))
                                                 {
                                                     SpellManager.Cast(SummonImp);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 if (SpellManager.HasSpell(SummonImp) && SpellManager.HasSpell(SummonVoidwalker))
                                                 {
                                                     SpellManager.Cast(SummonVoidwalker);
                                                     StyxWoW.SleepForLagDuration();
                                                 }
                                                 break;
                                         }
                                        
                                     }
                                         return RunStatus.Success;
                                     
                                 })
                                 );
        }


        #endregion
    }
}
