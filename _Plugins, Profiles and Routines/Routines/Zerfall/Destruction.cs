using System;
using System.Collections.Generic;
using System.Threading;
using CommonBehaviors.Actions;
using Styx;
using Zerfall.Talents;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Sequence = TreeSharp.Sequence;
using System.Linq;
using Action = TreeSharp.Action;
using System.Diagnostics;
using Styx.Helpers;

namespace Zerfall
{
    public partial class Zerfall
    {
        public Stopwatch ISFTimer = new Stopwatch();
        public Stopwatch ImmoTimer = new Stopwatch();
        public Stopwatch PetTimerCombat = new Stopwatch();
        private Composite Destro_PVP()
        {
            return new PrioritySelector(

                              new Action(ret => Log("LOLOLOLOLO"))
                        );
        }

        private Composite Destro_Instance()
        {
            return new PrioritySelector(
                CreateSpellCheckAndCast("Life Tap", ret => ZerfallSettings.Instance.Use_LifeTap && Me.HealthPercent >= ZerfallSettings.Instance.LifeTap_HP_Limit && Me.ManaPercent <= ZerfallSettings.Instance.LifeTap_MP_Start),

                //Curses
               CreateSpellCheckAndCast("Curse of the Elements", ret => SpellManager.HasSpell(1490) && Me.GotTarget && ZerfallSettings.Instance.CurseSelect == "Elements" && !Me.CurrentTarget.Auras.ContainsKey("Curse of the Elements") && LastCast != CurseoftheElements),
               CreateSpellCheckAndCast("Curse of Weakness", ret => SpellManager.HasSpell(702) && Me.GotTarget && ZerfallSettings.Instance.CurseSelect == "Weakness" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Weakness") && LastCast != CurseofWeakness),
               CreateSpellCheckAndCast("Curse of Tongues", ret => SpellManager.HasSpell(1714) && Me.GotTarget && ZerfallSettings.Instance.CurseSelect == "Tongues" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Tongues") && LastCast != CurseofTongues),

               //if we have curse of elements use it, if not then base casting weakness or tongues on class type. 
               CreateSpellCheckAndCast("Curse of the Elements", ret => Me.GotTarget && SpellManager.HasSpell(1490) && ZerfallSettings.Instance.CurseSelect == "Automatic" && !Me.CurrentTarget.Auras.ContainsKey("Curse of the Elements") && LastCast != CurseoftheElements),
               CreateSpellCheckAndCast("Curse of Weakness", ret => Me.GotTarget && SpellManager.HasSpell(702) && !SpellManager.HasSpell(1490) && (Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.DeathKnight || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Rogue || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Paladin || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Warrior) && ZerfallSettings.Instance.CurseSelect == "Automatic" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Weakness") && LastCast != CurseofWeakness),
               CreateSpellCheckAndCast("Curse of Tongues", ret => Me.GotTarget && SpellManager.HasSpell(1714) && !SpellManager.HasSpell(1490) && (Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Mage || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Warlock || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Priest) && ZerfallSettings.Instance.CurseSelect == "Automatic" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Tongues") && LastCast != CurseofTongues),

              // new Action(ret => Logging.Write("Adds = {0}", getAdds().Count.ToString())),

                //Improved Soul Fire
               new Decorator(ret => Me.GotTarget && ZerfallSettings.Instance.Got_ISF && !Me.ActiveAuras.ContainsKey("Improved Soul Fire") && (!ISFTimer.IsRunning || ISFTimer.Elapsed.Seconds > 5),
                        new Action(ctx => ISFManager())
                        ),

               //AOE METHODS - RoF Example. 
               new Decorator(ret => SpellManager.CanCast("Rain of Fire") && ZerfallSettings.Instance.RainOfFire && getAdds2().Count >= ZerfallSettings.Instance.Adds && Me.CurrentTarget.Distance < 33,
                        new Action(ctx => AoE("Rain of Fire", Me.CurrentTarget.Location))),


               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul),

               CreateSpellCheckAndCast("Immolate",ret => Me.GotTarget && ZerfallSettings.Instance.Use_Immolate && GotMyDot("Immolate", Me.CurrentTarget, true) < 2 && LastCast != Immolate),

               CreateSpellCheckAndCast("Conflagrate", ret => Me.GotTarget && GotMyDot("Immolate", Me.CurrentTarget)),

               //Check T11 4P proc for fel flame * 3

               //Banes
               CreateSpellCheckAndCast("Bane of Doom", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Doom" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Agony" && GotMyDot("Bane of Agony", Me.CurrentTarget, true) < 2 && LastCast != BaneofAgony),

               CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && GotMyDot("Corruption", Me.CurrentTarget, true) < 2 && LastCast != Corruption),

               //Shadowflame
               CreateSpellCheckAndCast("Shadowflame", ret => Me.GotTarget && Me.CurrentTarget.Distance < 10 && ZerfallSettings.Instance.Use_Shadowflame),

               CreateSpellCheckAndCast("Soul Fire", ret => Me.GotTarget && Me.ActiveAuras.ContainsKey("Empowered Imp")),
               CreateSpellCheckAndCast("Chaos Bolt", ret => Me.GotTarget),
               CreateSpellCheckAndCast("Shadowburn", ret => Me.GotTarget && Me.CurrentTarget.HealthPercent < 20),

               //Filler
               CreateSpellCheckAndCast("Incinerate", ret => Me.GotTarget && SpellManager.HasSpell("Incinerate")),
               CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget && !SpellManager.HasSpell("Incinerate")),

               CreateSpellCheckAndCast("Fel Flame", ret => Me.GotTarget && Me.IsMoving)
               );
        }

        private Composite Destro_PVE()
        {
            return new PrioritySelector(

               new Decorator(ret => Me.GotTarget && Me.HealthPercent <= 60 && LastCast != DrainLife,
                    new PrioritySelector(
                         CreateSpellCheckAndCast("Soulburn", ret => !SpellManager.Spells["Soulburn"].Cooldown),
                         CreateSpellCheckAndCast("Drain Life")
                        )),

               CreateSpellCheckAndCast("Life Tap", ret => ZerfallSettings.Instance.Use_LifeTap && Me.HealthPercent >= ZerfallSettings.Instance.LifeTap_HP_Limit && Me.ManaPercent <= ZerfallSettings.Instance.LifeTap_MP_Start),

                //Curses
               CreateSpellCheckAndCast("Curse of the Elements", ret => SpellManager.HasSpell(1490) && Me.GotTarget && ZerfallSettings.Instance.CurseSelect == "Elements" && !Me.CurrentTarget.Auras.ContainsKey("Curse of the Elements") && LastCast != CurseoftheElements),
               CreateSpellCheckAndCast("Curse of Weakness", ret => SpellManager.HasSpell(702) && Me.GotTarget && ZerfallSettings.Instance.CurseSelect == "Weakness" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Weakness") && LastCast != CurseofWeakness),
               CreateSpellCheckAndCast("Curse of Tongues", ret => SpellManager.HasSpell(1714) && Me.GotTarget && ZerfallSettings.Instance.CurseSelect == "Tongues" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Tongues") && LastCast != CurseofTongues),

               //if we have curse of elements use it, if not then base casting weakness or tongues on class type. 
               CreateSpellCheckAndCast("Curse of the Elements", ret => Me.GotTarget && SpellManager.HasSpell(1490) && ZerfallSettings.Instance.CurseSelect == "Automatic" && !Me.CurrentTarget.Auras.ContainsKey("Curse of the Elements") && LastCast != CurseoftheElements),
               CreateSpellCheckAndCast("Curse of Weakness", ret => Me.GotTarget && SpellManager.HasSpell(702) && !SpellManager.HasSpell(1490) && (Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.DeathKnight || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Rogue || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Paladin || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Warrior) && ZerfallSettings.Instance.CurseSelect == "Automatic" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Weakness") && LastCast != CurseofWeakness),
               CreateSpellCheckAndCast("Curse of Tongues", ret => Me.GotTarget && SpellManager.HasSpell(1714) && !SpellManager.HasSpell(1490) && (Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Mage || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Warlock || Me.CurrentTarget.Class == Styx.Combat.CombatRoutine.WoWClass.Priest) && ZerfallSettings.Instance.CurseSelect == "Automatic" && !Me.CurrentTarget.Auras.ContainsKey("Curse of Tongues") && LastCast != CurseofTongues),

                //Improved Soul Fire
               new Decorator(ret => Me.GotTarget && ZerfallSettings.Instance.Got_ISF && Me.ActiveAuras.ContainsKey("Improved Soul Fire") && (!ISFTimer.IsRunning || ISFTimer.Elapsed.Seconds > 5),
                        new Action(ctx => ISFManager())
                        ),

               CreateBuffCheckAndCast("Demon Soul", ret => Me.GotTarget && ZerfallSettings.Instance.Use_DemonSoul),
               //CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul),

               CreateSpellCheckAndCast("Immolate", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Immolate && GotMyDot("Immolate", Me.CurrentTarget, true) < 2 && LastCast != Immolate),

               CreateSpellCheckAndCast("Conflagrate", ret => Me.GotTarget && GotMyDot("Immolate", Me.CurrentTarget)),

               //Check T11 4P proc for fel flame * 3

               //Banes
               CreateSpellCheckAndCast("Bane of Doom", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Doom" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Agony" && GotMyDot("Bane of Agony", Me.CurrentTarget, true) < 2 && LastCast != BaneofAgony),

               CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && GotMyDot("Corruption", Me.CurrentTarget, true) < 2 && LastCast != Corruption),

               //Shadowflame
               CreateSpellCheckAndCast("Shadowflame", ret => Me.GotTarget && Me.CurrentTarget.Distance < 10 && ZerfallSettings.Instance.Use_Shadowflame),

               CreateSpellCheckAndCast("Soul Fire", ret => Me.GotTarget && Me.ActiveAuras.ContainsKey("Empowered Imp")),
               CreateSpellCheckAndCast("Chaos Bolt", ret => Me.GotTarget),
               CreateSpellCheckAndCast("Shadowburn", ret => Me.GotTarget && Me.CurrentTarget.HealthPercent < 20),

               //Filler
               CreateSpellCheckAndCast("Incinerate", ret => Me.GotTarget && SpellManager.HasSpell("Incinerate")),
               CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget && !SpellManager.HasSpell("Incinerate")),

               CreateSpellCheckAndCast("Fel Flame", ret => Me.GotTarget && Me.IsMoving)
               );
        }
    }
}
