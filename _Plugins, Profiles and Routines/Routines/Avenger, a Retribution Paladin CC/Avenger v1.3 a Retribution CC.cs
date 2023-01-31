using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using System.Drawing;
using Styx;
using Styx.Combat.CombatRoutine;
using Styx.Helpers;
using Styx.Logic;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace Avenger
{
    class Classname : CombatRoutine
    {
        public override sealed string Name { get { return "Avenger v1.3"; } }

        public override WoWClass Class { get { return WoWClass.Paladin; } }


        private static LocalPlayer Me { get { return ObjectManager.Me; } }
   
        #region Log
        private void slog(string format, params object[] args) //use for slogging
        {
            Logging.Write(format, args);
        }
        #endregion
        

        #region Initialize
        public override void Initialize()
        {
			Logging.Write(Color.White, "___________________________________________________");
            Logging.Write(Color.Crimson, "-------- Avenger v1.3 CC by Shaddar -------");
			Logging.Write(Color.Crimson, "-----  Remember to comment on the forum! -----");
			Logging.Write(Color.Crimson, "-------- /like and +rep if you like this CC! ------");
			Logging.Write(Color.White, "___________________________________________________");
        }
        #endregion

        #region Settings

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            Logging.Write(Color.Aqua, "No need to configure anything!");
        }
        #endregion

		#region Rest

        public override bool NeedRest
        {
            get
            {
                if (!Me.Mounted && !Me.HasAura("Blessing of Might") && !Me.HasAura("Mark of the Wild"))
					{
					if(CastSpell("Blessing of Might"))
                    return true;
						{
						Logging.Write(Color.Aqua, ">> Blessing of Might <<");
						}
					}

				if (!Me.Mounted && SpellManager.HasSpell("Seal of Truth") && !Me.HasAura("Seal of Truth"))
					{
					if(CastSpell("Seal of Truth"))
					return true;
						{
						Logging.Write(Color.Aqua, ">> Seal of Truth <<");
						}
					}
                return false;
            }
        }

        public override void Rest()
		{

		}
        #endregion


		#region Add Detection
        //Credit to CodeNameGamma for detectAdds code
        private List<WoWUnit> adds = new List<WoWUnit>();

		private List<WoWUnit> detectAdds()
        {
            if (Me.CurrentTarget != null)
            {
                List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                            unit.Guid != Me.Guid &&
                            unit.Location.Distance(Me.CurrentTarget.Location) <= 10 &&
							(unit.IsTargetingMyPartyMember || unit.IsTargetingMyRaidMember || unit.IsTargetingMeOrPet) &&
                            !unit.IsFriendly &&
                            !unit.IsPet &&
                            !Styx.Logic.Blacklist.Contains(unit.Guid));

                if (addList.Count > 1)
                {
                    Logging.Write(Color.Crimson, "Detected " + addList.Count.ToString() + " adds! Switchting to AoE Rotation!");
                }
                return addList;
            }
            else
            {
                return null;
            }
            
        }  
		private bool IsTargetBoss()
        {
            string UnitClassification = Lua.GetReturnValues("local classification = UnitClassification(\"target\"); return classification")[0];
            string UnitLevel = Lua.GetReturnValues("local level = UnitLevel(\"target\"); return level")[0];
            if (!Me.IsInRaid)
            {
                if (UnitClassification == "worldboss" ||
                   (Me.CurrentTarget.Level == 87 && Me.CurrentTarget.Elite) && Me.CurrentTarget.MaxHealth > 3500000 ||
                   (Me.CurrentTarget.Level == 88))
                    return true;

                else return false;
            }
            else
            {
                if (UnitLevel == "-1")
                    return true;

                else return false;
            }
            
            
        }
        #endregion

        #region CastSpell Method
        // Credit to Apoc for the below CastSpell code
        // Used for calling CastSpell in the Combat Rotation
        public bool CastSpell(string spellName)
        {
            if (SpellManager.CanCast(spellName))
            {
                SpellManager.Cast(spellName);
                // We managed to cast the spell, so return true, saying we were able to cast it.
                return true;
            }
            // Can't cast the spell right now, so return false.
            return false;
        }
		public bool CastBuff(string name)
        {
            if (!PlayerHasBuff(name) && SpellManager.CanBuff(name, Me))
            {
                SpellManager.Cast(name);
                slog(name);
                return true;
            }
            return false;
        }

        public bool PlayerHasBuff(string name)
        {
            return PlayerBuffTimeLeft(name) > 0;
        }

        public bool TargetHasDebuff(string name)
        {
            return TargetDebuffTimeLeft(name) != 0; // <0 means passive aura; >0 means active one
        }
		public double PlayerBuffTimeLeft(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitBuff('player', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", Lua.Escape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                slog("Lua failed in AuraTimeLeft");
                return 999999;
            }
        }

        private double TargetDebuffTimeLeft(string DebuffName)
        {
            return double.Parse(Lua.GetReturnValues(string.Format("local expirationTime = select(7, UnitDebuff(\"target\", \"{0}\", nil, \"player\")); " +
                                     "if expirationTime == nil then return 0 else return expirationTime - GetTime() end", DebuffName))[0]);
        }

        public double TargetDebuffTimeLeftAll(string name)
        {
            try
            {
                var lua = string.Format("local x=select(7, UnitDebuff(\"target\", \"{0}\", nil)); if x==nil then return 0 else return x-GetTime() end", Lua.Escape(name));
                var t = double.Parse(Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                slog("Lua failed in TargetAuraTimeLeft");
                return 999999;
            }
        }
		#endregion

        #region CombatStart


        private void AutoAttack()
        {
            if (!Me.IsAutoAttacking && Me.CurrentTarget.Distance <=7d)
            {
                Lua.DoString("StartAttack()");
            }
        }
        #endregion

        #region Combat
      
        public override void Combat()
        {
            adds = detectAdds();	

			if (Me.CurrentTarget != null  && Me.CurrentTarget.IsAlive == true && Me.Mounted == false)
            {

				{
				if (Me.HealthPercent <=15 && Me.Combat)
					{
					if (CastSpell("Lay on Hands")==true)
						{
						Logging.Write(Color.Crimson, ">> Lay on Hands!!! <<");
						}
					}
				}

//////////////////////////////////////////////Interrupts here///////////////////////////////////////////////////////////////////////////////////////////////////

//				{
//				if (Me.CurrentTarget.IsCasting && Me.CanInterruptCurrentSpellCast)
//                  {  
//					if (CastSpell("Rebuke"))
//						{
//						Logging.Write(Color.White, ">> Rebuke <<");
//						}
//					}
//				}

//////////////////////////////////////////////AoE Rotation here/////////////////////////////////////////////////////////////////////////////////////////////////
				
				{
				if (adds.Count >= 4 && !PlayerHasBuff("Seal of Righteousness") && PlayerHasBuff("Seal of Truth"))
					{
					if(CastSpell("Seal of Righteousness"))
						{
						Logging.Write(Color.Aqua, ">> Seal of Righteousness <<");
						}
					}
				}
				{
				if (adds.Count >= 4)
					{
					if (CastSpell("Divine Storm"))
						{
						Logging.Write(Color.Aqua, ">> Divie Storm <<");
						}
					}
				}

/////////////////////////////////////////////Single Target DPS here////////////////////////////////////////////////////////////////////////////////////////////

				{
				if (adds.Count <= 3 && !PlayerHasBuff("Seal of Truth") && PlayerHasBuff("Seal of Righteousness"))
					{
					if(CastSpell("Seal of Truth"))
						{
						Logging.Write(Color.Aqua, ">> Seal of Truth <<");
						}
					}
				}
				{
				if (PlayerHasBuff("Divine Purpose") && !PlayerHasBuff("Inquisition") || PlayerHasBuff("Zealotry") && !PlayerHasBuff("Inquisition") || Me.CurrentHolyPower >=2 && !PlayerHasBuff("Inquisition"))
					{
					if (CastSpell("Inquisition"))
						{
						Logging.Write(Color.Aqua, ">> Inquisition <<");
						}
					}
				}
				{
				if (PlayerHasBuff("Divine Purpose") && PlayerHasBuff("Inquisition") || PlayerHasBuff("Zealotry") && PlayerHasBuff("Inquisition") || Me.CurrentHolyPower >=3 && PlayerHasBuff("Inquisition"))
					{
					if (CastSpell("Templar's Verdict"))
						{
						Logging.Write(Color.Aqua, ">> Templar's Verdict <<");
						}
					}
				}
				{
				if (PlayerHasBuff("Avenging Wrath") || Me.CurrentTarget.HealthPercent <=20)
					{
					if (CastSpell("Hammer of Wrath"))
						{
						Logging.Write(Color.Aqua, ">> Crusader Strike <<");
						}
					}
				}
				{
				if (adds.Count <= 3)
					{
					if (CastSpell("Crusader Strike"))
						{
						Logging.Write(Color.Aqua, ">> Crusader Strike <<");
						}
					}
				}
				{
				if (CastSpell("Judgement"))
					{
					Logging.Write(Color.Aqua, ">> Judgement <<");
					}
				}
				{
				if (PlayerHasBuff("The Art of War") && !PlayerHasBuff("Divine Purpose"))
					{
					if (CastSpell("Exorcism"))
						{
						Logging.Write(Color.Aqua, ">> Exorcism <<");
						}
					}
				}
				{
				if ((!PlayerHasBuff("Divine Purpose") || !PlayerHasBuff("Zealotry") || !PlayerHasBuff("The Art of War") || Me.CurrentHolyPower >=3) &&
					Me.CurrentTarget.Distance <= 12d && Me.ManaPercent >=35)	
					{
					if (CastSpell("Holy Wrath"))               
						{
						Logging.Write(Color.Aqua, ">> Holy Wrath <<");
						}
					}
				}
				{
				if ((!PlayerHasBuff("Divine Purpose") || !PlayerHasBuff("Zealotry") || !PlayerHasBuff("The Art of War") || Me.CurrentHolyPower >=3) &&
					Me.CurrentTarget.Distance <= 8d && Me.ManaPercent >=70)	
					{
					if (CastSpell("Consecration"))               
						{
						Logging.Write(Color.Aqua, ">> Consecration <<");
						}
					}
				}

/////////////////////////////////////////////Cooldowns are here////////////////////////////////////////////////////////////////////////////////////////////////

				
				{
                if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
				    if (CastSpell("Zealotry"))
						{
				        Logging.Write(Color.Blue, ">> Zealotry <<");
						}
					}
				}
				{
				if (IsTargetBoss())
					{
				    if (CastSpell("Avenging Wrath"))
						{
				        Logging.Write(Color.Blue, ">> Avenging Wrath <<");
						}
					}
				}
				{
				if (Me.ManaPercent <=20)
					{
					if (CastSpell("Divine Plea"))
						{
						Logging.Write(Color.Blue, ">> Divine Plea <<");
			            }
					}
				}   
				{
				if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
				    if (CastSpell("Guardian of Ancient Kings"))
						{
				        Logging.Write(Color.Blue, ">> Guardian of Ancient Kings <<");
						}
					}
				}
				{
				if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
					Lua.DoString("RunMacroText('/use 10');");
					}
				}
				{
				if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
					Lua.DoString("RunMacroText('/use 13');");
					}
				}
				{
				if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
					Lua.DoString("RunMacroText('/use 14');");
					}
				}

//////////////////////////////////////////////////Racial Skills here/////////////////////////////////////////////////////////////////////////////////////////

				{
				if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
					Lua.DoString("RunMacroText('/Cast Berserking');");
					}
				}
				{
				if (IsTargetBoss() && Me.CurrentTarget.Distance <= 10d)
					{
					Lua.DoString("RunMacroText('/Cast Blood Fury');");
					}
				}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            }                      
            
        }  
  
        #endregion

    }
}