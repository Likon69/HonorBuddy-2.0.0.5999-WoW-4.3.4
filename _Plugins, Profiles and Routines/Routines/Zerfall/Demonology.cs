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

namespace Zerfall
{
    public partial class Zerfall
    {
        private Composite Demo_PVP()
        {
            return new PrioritySelector(

                              new Action(ret => Log("Not Supported Yet"))
                        );
        }

        private Composite Demo_Instance()
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

               new Decorator(ret => Me.CurrentTarget.Elite,
                    new PrioritySelector(
                         CreateBuffCheckAndCast("Metamorphosis"),
                         CreateSpellCheckAndCast("Immolation Aura", ret => Me.CurrentTarget.Distance < 5)
                        )),

                //Demon Soul if demon is Felguard
               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul && Me.Pet.CreatedBySpellId == 30146),

               CreateSpellCheckAndCast("Immolate", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Immolate && GotMyDot("Immolate", Me.CurrentTarget, true) < 2 && LastCast != Immolate),
               CreateSpellCheckAndCast("Hand of Gul'dan", ret => Me.GotTarget && !SpellManager.Spells["Hand of Gul'dan"].Cooldown),

               //Banes
               CreateSpellCheckAndCast("Bane of Doom", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Doom" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Agony" && !GotMyDot("Bane of Agony", Me.CurrentTarget) && LastCast != BaneofAgony),

               CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && GotMyDot("Corruption", Me.CurrentTarget, true) < 2 && LastCast != Corruption),

               //Shadowflame
               CreateSpellCheckAndCast("Shadowflame", ret => Me.GotTarget && Me.CurrentTarget.Distance < 10 && ZerfallSettings.Instance.Use_Shadowflame),

               CreateSpellCheckAndCast("Incinerate", ret => Me.GotTarget && Me.ActiveAuras.ContainsKey("Molten Core")),
               CreateSpellCheckAndCast("Soul Fire", ret => Me.GotTarget && Me.ActiveAuras.ContainsKey("Decimation")),

               //Demon Soul if demon is Succubus
               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul && CurrentPetSpell == "Succubus" && Me.Pet.CreatedBySpellId == 712),

               CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget),

               CreateSpellCheckAndCast("Fel Flame", ret => Me.GotTarget && Me.IsMoving)
               );
        }
        private Composite Demo_PVE()
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

               new Decorator(ret => Me.CurrentTarget.Elite,
                    new PrioritySelector(
                         CreateBuffCheckAndCast("Metamorphosis"),
                         CreateSpellCheckAndCast("Immolation Aura", ret => Me.CurrentTarget.Distance < 5)
                        )),

                //Demon Soul if demon is Felguard
               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul && Me.Pet.CreatedBySpellId == 30146),

               CreateSpellCheckAndCast("Immolate", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Immolate && GotMyDot("Immolate", Me.CurrentTarget, true) < 2 && LastCast != Immolate),
               CreateSpellCheckAndCast("Hand of Gul'dan", ret => SpellManager.HasSpell("Hand of Gul'dan") && Me.GotTarget && !SpellManager.Spells["Hand of Gul'dan"].Cooldown),

               //Banes
               CreateSpellCheckAndCast("Bane of Doom", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Doom" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Agony" && !GotMyDot("Bane of Agony", Me.CurrentTarget) && LastCast != BaneofAgony),

               CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && GotMyDot("Corruption", Me.CurrentTarget, true) < 2 && LastCast != Corruption),

               //Shadowflame
               CreateSpellCheckAndCast("Shadowflame", ret => Me.GotTarget && Me.CurrentTarget.Distance < 10 && ZerfallSettings.Instance.Use_Shadowflame),

               CreateSpellCheckAndCast("Incinerate", ret => Me.GotTarget && Me.ActiveAuras.ContainsKey("Molten Core")),
               CreateSpellCheckAndCast("Soul Fire", ret => Me.GotTarget && Me.ActiveAuras.ContainsKey("Decimation")),

               //Demon Soul if demon is Succubus
               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul && CurrentPetSpell == "Succubus" && Me.Pet.CreatedBySpellId == 712),

               CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget),

               CreateSpellCheckAndCast("Fel Flame", ret => Me.GotTarget && Me.IsMoving)
               );
        }
    }
}
