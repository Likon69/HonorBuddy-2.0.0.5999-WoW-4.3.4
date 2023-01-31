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
         
        private Composite Low_ComCOM()
        {
            return new PrioritySelector(
                               
                              new Action(ret => Log("LOLOLOLOLO"))
                        );
        }

        private Composite Low_rotation()
        {
            return new PrioritySelector(

                 new Decorator(ret => Me.GotTarget && Me.HealthPercent <= 60 && LastCast != DrainLife,
                    new PrioritySelector(
                         CreateSpellCheckAndCast("Soulburn", ret => !SpellManager.Spells["Soulburn"].Cooldown),
                         CreateSpellCheckAndCast("Drain Life")
                        )),

                 CreateSpellCheckAndCast("Life Tap", ret => ZerfallSettings.Instance.Use_LifeTap && Me.HealthPercent >= ZerfallSettings.Instance.LifeTap_HP_Limit && Me.ManaPercent <= ZerfallSettings.Instance.LifeTap_MP_Start),
                 CreateSpellCheckAndCast("Immolate", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Immolate && !Me.CurrentTarget.Auras.ContainsKey("Immolate") && LastCast != Immolate),
                 CreateSpellCheckAndCast("Corruption", ret => Me.GotTarget && ZerfallSettings.Instance.Use_Corruption && !Me.CurrentTarget.Auras.ContainsKey("Corruption") && LastCast != Corruption),
                 CreateSpellCheckAndCast("Shadow Bolt", ret => Me.GotTarget)
            );
        }
    }
}
