using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.Logic.Profiles;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;


namespace FelMaster
{
    public class FelMasterui : CombatRoutine
    {
        public override Styx.Combat.CombatRoutine.WoWClass Class
        {
            get { return Styx.Combat.CombatRoutine.WoWClass.Rogue; }
        }

        public SimCraftBase SC = null;

        public override void Initialize()
        {
		{
	            SC = new SimCraftBase();
				Logging.Write(Color.White, "******************************************************");
        	  		Logging.Write(Color.White, "                                                      ");
				Logging.Write(Color.White, "          Felmaster Community Powered CC              ");
				Logging.Write(Color.White, "                                                      ");
				Logging.Write(Color.White, "******************************************************");
				Logging.Write(Color.White, "  ..Special Thanks To Sjussju For the UI Template..   ");
		}
	}

        public override string Name
        {
            get { return "FelMaster - Sub Rogue"; }
        }

        public override TreeSharp.Composite CombatBehavior
        {
            get
            {
                return _Combat;
            }
        }


        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            FelMaster.FelMasterForm7 f7 = new FelMaster.FelMasterForm7();
            f7.ShowDialog();
        }

        public override void Pulse()
        {
	    if (StyxWoW.Me.GotTarget && StyxWoW.Me.Combat && FelMasterSettings.Instance.T1)
            {
                if (StyxWoW.Me.CurrentTarget.IsFriendly == false && StyxWoW.Me.Mounted == false)
                {
                    if (SC.IsTargetBoss() || StyxWoW.Me.CurrentTarget.IsPlayer)
                    {
                        if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown == 0)
                        {
				StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                            return;
                        }
                    }
                }
            }

        if (StyxWoW.Me.GotTarget && StyxWoW.Me.Combat && FelMasterSettings.Instance.T2)
            {
                if (StyxWoW.Me.CurrentTarget.IsFriendly == false && StyxWoW.Me.Mounted == false)
                {
                    if (SC.IsTargetBoss() || StyxWoW.Me.CurrentTarget.IsPlayer)
                    {
                        if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown == 0)
                        {
				StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                            return;
                        }
                    }
                }
            }
	}


        private Composite _combat = null;
        public Composite _Combat
        {
            get
            {
                if (_combat == null)
                {
	if (SC.IsNotCCed() || !SC.PlayerHasBuff("Vanish") || !SC.PlayerHasBuff("Stealth"))
		{
                    _combat = new PrioritySelector(


                        SC.CastBuff("Stealth", a => !SC.PlayerHasBuff("Master of Sublety"), "Stealth"),

			SC.CastSpell("Cloak of Shadows", a => SimCraftBase.Me.HealthPercent < 20 || SC.Cloakdatshit(), "Cloak of Shadows"),

			SC.CastSpell("Vanish", a => SC.PlayerHasBuff("Cloak of Shadows") && !SC.NeedsCombatReadiness(), "Vanish"),

                        //SC.CastSpell("Shadowstep", a => SimCraftBase.Me.HealthPercent > 25 && SimCraftBase.Me.CurrentTarget.Distance > 10 && SimCraftBase.Me.GotTarget && !SC.PlayerHasBuff("Master of Sublety"), "Shadowstep"),

			SC.CastSpell("Evasion", a => SimCraftBase.Me.HealthPercent < 60, "Evasion"),

			SC.CastSpell("Combat Readiness", a => SimCraftBase.Me.HealthPercent < 50 && SC.NeedsCombatReadiness(), "Combat Readiness"),


                        SC.CastSpell("Shiv", a => SC.TargetNeedsShiv(), "Shiv"),

			SC.CastSpell("Dismantle", a => FelMasterSettings.Instance.ND && SC.TargetNeedsDismantle() && SimCraftBase.Me.CurrentTarget.MaxHealth > 1, "Dismantle"),


			SC.CastSpell("Shadow Dance", a => SimCraftBase.Me.CurrentTarget.IsPlayer && SimCraftBase.Me.CurrentTarget.HealthPercent < 50 && SC.PlayerHasBuff("Recuperate"), "Shadow Dance"),

                        SC.CastSpell("Cheap Shot", a => SC.PlayerHasBuff("Shadow Dance") && !SC.TargetHasDebuff("Kidney Shot") && !SC.TargetHasDebuff("Cheap Shot"), "Cheap Shot"),

                        //SC.CastSpell("Smoke Bomb", a => SC.PlayerHasBuff("Shadow Dance"), "Smoke bomb"),

                        SC.CastSpell("Garrote", a => !SC.TargetHasDebuff("Garrote") && !SC.TargetHasDebuff("Cheap Shot") && !SC.TargetHasDebuff("Kidney Shot") && SC.PlayerHasBuff("Shadow Dance") && StyxWoW.Me.CurrentTarget.MeIsBehind && SC.TargetNeedsGarrote(), "Garrote"),

			SC.CastBuff("Premeditation", a => SC.PlayerHasBuff("Shadow Dance"), "Premeditation"),

                        SC.CastSpell("Ambush", a => SC.PlayerHasBuff("Shadow Dance") && StyxWoW.Me.CurrentTarget.MeIsBehind, "Ambush"),


                        SC.CastSpell("Cheap Shot", a => SC.PlayerHasBuff("Master of Sublety"), "Cheap Shot"),


			SC.CastSpell("Kick", a => SimCraftBase.Me.GotTarget && SimCraftBase.Me.CurrentTarget.IsCasting, "Kick"),

			SC.CastSpell("Gouge", a => !StyxWoW.Me.CurrentTarget.MeIsBehind && SimCraftBase.Me.GotTarget && SimCraftBase.Me.CurrentTarget.IsCasting, "Gouge"),

                        SC.CastSpell("Kidney Shot", a => SimCraftBase.Me.CurrentTarget.IsPlayer && SC.SpellCooldown("Kick") > 1 && SC.IsUsableSpell("Kidney Shot") && SimCraftBase.Me.GotTarget && SimCraftBase.Me.CurrentTarget.IsCasting, "Kidney Shot"),


                        SC.CastSpell("Redirect", a => SpellManager.HasSpell("Redirect") && (SC.SpellCooldown("Redirect") == 0 && SC.IsUsableSpell("Redirect") && StyxWoW.Me.RawComboPoints > 1 && SimCraftBase.Me.ComboPoints == 0), "Redirecting..."),

                        SC.CastSpell("Hemorrhage", a => SC.TargetDebuffTimeLeft("Hemorrhage") < 35 && SimCraftBase.Me.ComboPoints < 5, "Hemorrhage for the Debuff"),


			SC.CastSpell("Recuperate", a => SC.IsUsableSpell("Recuperate") && SC.PlayerBuffTimeLeft("Recuperate") < 3 && SimCraftBase.Me.ComboPoints > 2, "Recuperate"),

			SC.CastSpell("Rupture", a => SimCraftBase.Me.ComboPoints > 0 && SC.TargetNeedsRupture() && SC.PlayerBuffTimeLeft("Rupture") < 1  && SC.PlayerBuffTimeLeft("Recuperate") > 0, "Rupture"),

			SC.CastSpell("Slice and Dice", a => SimCraftBase.Me.CurrentTarget.HealthPercent > 50 && SimCraftBase.Me.ComboPoints > 2 && FelMasterSettings.Instance.SnD && SC.PlayerBuffTimeLeft("Slice and Dice") < 1  && SC.PlayerBuffTimeLeft("Recuperate") > 6, "Slice and Dice"),



			//SC.CastSpell("Backstab", a => !SC.PlayerHasBuff("Shadow Dance") && StyxWoW.Me.CurrentTarget.MeIsBehind && SimCraftBase.Me.ComboPoints < 5, "Backstab"),

			SC.CastSpell("Hemorrhage", a => !SC.PlayerHasBuff("Shadow Dance") && SimCraftBase.Me.ComboPoints < 5, "Hemorrhage"),



                        SC.CastSpell("Eviscerate", a => SimCraftBase.Me.ComboPoints > 3, "Eviscerate"),

                        SC.CastSpell("Deadly Throw", a => SC.IsUsableSpell("Deadly Throw") && SimCraftBase.Me.CurrentTarget.Distance > 15, "Deadly Throw")


                    );
                   }
		}
                return _combat;
            }
        }
    }
}
