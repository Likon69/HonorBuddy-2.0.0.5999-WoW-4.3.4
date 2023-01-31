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
        private Composite Affliction_PVP()
        {
            return new PrioritySelector(

                              new Action(ret => Log("Not Supported Yet."))
                        );
        }

        private Composite Affliction_Instance()
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

                //Haunt
               CreateSpellCheckAndCast("Haunt", ret => Me.GotTarget && SpellManager.HasSpell("Haunt") && !SpellManager.Spells["Haunt"].Cooldown),

                //Banes
               CreateSpellCheckAndCast("Bane of Doom", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Doom" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Agony" && !GotMyDot("Bane of Agony", Me.CurrentTarget) && LastCast != BaneofAgony),

               //If i got Bane of Doom and Agony, then cast Doom, if i have Agony but no doom, then cast Agony, When Automatic.
               CreateSpellCheckAndCast("Bane of Doom", ret => SpellManager.HasSpell(980) && SpellManager.HasSpell(603) && Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Automatic" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => SpellManager.HasSpell(980) && !SpellManager.HasSpell(603) && Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Automatic" && !GotMyDot("Bane of Agony", Me.CurrentTarget) && LastCast != BaneofAgony),




               CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && GotMyDot("Corruption", Me.CurrentTarget, true) < 2 && LastCast != Corruption),

               //Immolate or UA
               CreateSpellCheckAndCast("Unstable Affliction", ret => Me.GotTarget && SpellManager.HasSpell("Unstable Affliction") && GotMyDot("Unstable Affliction", Me.CurrentTarget, true) < 2 && LastCast != UnstableAffliction),
               CreateSpellCheckAndCast("Immolate", ret => Me.GotTarget && !SpellManager.HasSpell("Unstable Affliction") && ZerfallSettings.Instance.Use_Immolate && GotMyDot("Immolate", Me.CurrentTarget, true) < 2 && LastCast != Immolate),

               CreateSpellCheckAndCast("Drain Soul", ret => Me.GotTarget && Me.CurrentTarget.HealthPercent < 25),

               //Shadowflame
               CreateSpellCheckAndCast("Shadowflame", ret => Me.GotTarget && Me.CurrentTarget.Distance < 10 && ZerfallSettings.Instance.Use_Shadowflame),

               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul),

               //Shadow Bolt Or Drain Life
               CreateSpellCheckAndCast("Drain Life", ret => Me.GotTarget && ZerfallSettings.Instance.Use_DrainLife),
               CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget && !ZerfallSettings.Instance.Use_DrainLife),

               CreateSpellCheckAndCast("Fel Flame", ret => Me.GotTarget && Me.IsMoving)
               );
        }

        private Composite Affliction_PVE()
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

                //Haunt
               CreateSpellCheckAndCast("Haunt", ret => Me.GotTarget && SpellManager.HasSpell("Haunt") && !SpellManager.Spells["Haunt"].Cooldown),

                //Banes
               CreateSpellCheckAndCast("Bane of Doom", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Doom" && GotMyDot("Bane of Doom", Me.CurrentTarget, true) < 15 && LastCast != BaneofDoom),
               CreateSpellCheckAndCast("Bane of Agony", ret => Me.GotTarget && ZerfallSettings.Instance.BaneSelect == "Agony" && !GotMyDot("Bane of Agony", Me.CurrentTarget) && LastCast != BaneofAgony),

               CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && GotMyDot("Corruption", Me.CurrentTarget, true) < 2 && LastCast != Corruption),

               //Immolate or UA
               CreateSpellCheckAndCast("Unstable Affliction", ret => Me.GotTarget && SpellManager.HasSpell("Unstable Affliction") && GotMyDot("Unstable Affliction", Me.CurrentTarget, true) < 2 && LastCast != UnstableAffliction),
               CreateSpellCheckAndCast("Immolate", ret => Me.GotTarget && !SpellManager.HasSpell("Unstable Affliction") && ZerfallSettings.Instance.Use_Immolate && GotMyDot("Immolate", Me.CurrentTarget, true) < 2 && LastCast != Immolate),

               CreateSpellCheckAndCast("Drain Soul", ret => Me.GotTarget && Me.CurrentTarget.HealthPercent < 25),

               //Shadowflame
               CreateSpellCheckAndCast("Shadowflame", ret => Me.GotTarget && Me.CurrentTarget.Distance < 10 && ZerfallSettings.Instance.Use_Shadowflame),

               CreateBuffCheckAndCast("Demon Soul", ret => Me.CurrentTarget.Elite && ZerfallSettings.Instance.Use_DemonSoul),

               //Shadow Bolt Or Drain Life
               CreateSpellCheckAndCast("Drain Life", ret => Me.GotTarget && ZerfallSettings.Instance.Use_DrainLife),
               CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget && !ZerfallSettings.Instance.Use_DrainLife),

               CreateSpellCheckAndCast("Fel Flame", ret => Me.GotTarget && Me.IsMoving)
               );
        }
    }
}
