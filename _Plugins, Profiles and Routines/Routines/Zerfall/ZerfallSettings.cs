using System.IO;
using Styx;
using Styx.Helpers;

namespace Zerfall
{
    public class ZerfallSettings : Settings
    {
        public static readonly ZerfallSettings Instance = new ZerfallSettings();

        public ZerfallSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/Zerfall-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        #region Rest

        [Setting, DefaultValue(false)]
        public bool IsConfigured { get; set; }

        [Setting, DefaultValue(60)]
        public int RestHealthPercentage { get; set; }

        [Setting, DefaultValue(60)]
        public int RestManaPercentage { get; set; }

        [Setting, DefaultValue(45)]
        public int LifeTap_MP_Start { get; set; }

        [Setting, DefaultValue(45)]
        public int LifeTap_HP_Limit { get; set; }

        [Setting, DefaultValue("Imp")]
        public string PetSpell { get; set; }

        [Setting, DefaultValue("Shadow Bolt")]
        public string PullSpellSelect { get; set; }


        #region Spells


        [Setting, DefaultValue(false)]
        public bool RestStone { get; set; }
        
        [Setting, DefaultValue(5)]
        public int Adds { get; set; }

        [Setting, DefaultValue(45)]
        public int HealthStoneCombat { get; set; }

        [Setting, DefaultValue(45)]
        public int HealthPotPercent { get; set; }

        [Setting, DefaultValue(45)]
        public int ManaPotPercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_Immolate { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_Corruption { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_LifeTap { get; set; }

        //Curse of Exhaustion, Curse of Weakness, Curse of Tongues, Curse of the Elements
        [Setting, DefaultValue("Automatic")]
        public string CurseSelect { get; set; }

        //Bane of Agony, Bane of Doom
        [Setting, DefaultValue("Automatic")]
        public string BaneSelect { get; set; }
        // Demon Armor, Fel Armor, Auto
        [Setting, DefaultValue("Automatic")]
        public string ArmorSelect { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_Shadowflame { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_DemonSoul { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_Soulburn { get; set; }

        //Improved Soul Fire Talent
        [Setting, DefaultValue(true)]
        public bool Got_ISF { get; set; }

        [Setting, DefaultValue(true)]
        public bool Use_DrainSoul { get; set; }

        [Setting, DefaultValue(true)]
        public bool PS_Sacrifice { get; set; }

        [Setting, DefaultValue(false)]
        public bool Use_DrainLife { get; set; }

        [Setting, DefaultValue(true)]
        public bool PS_BloodPact { get; set; }

        [Setting, DefaultValue(true)]
        public bool SoulHarvestRest { get; set; }

        [Setting, DefaultValue(true)]
        public bool RainOfFire { get; set; }

        [Setting, DefaultValue(false)]
        public bool MoveDisable { get; set; }
        
        #endregion
        #endregion


    }
}
