using System.IO;
using Styx;
using Styx.Helpers;

namespace TCDiscPriest
{
    public class TCDiscPriestSettings : Settings
    {
        public static TCDiscPriestSettings Instance = new TCDiscPriestSettings();

        public TCDiscPriestSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/TCDiscPriest/Config/TCDiscPriest-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(true)]
        public bool UseArchangel { get; set; }
		
		[Setting, DefaultValue(true)]
        public bool UseInnerFocus { get; set; }
		
		[Setting, DefaultValue(true)]
        public bool UseInnerWill { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseInnerFire { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseCureDisease { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseDispelMagic { get; set; }
		
		[Setting, DefaultValue(true)]
        public bool UseResurrection { get; set; }
		
		[Setting, DefaultValue(true)]
        public bool UseFearWard { get; set; }
		
		[Setting, DefaultValue(true)]
        public bool UsePainSuppression { get; set; }
	
        [Setting, DefaultValue(80)]
        public int PenancePercent { get; set; }
		
		[Setting, DefaultValue(90)]
        public int TankShieldPercent { get; set; }
		
		[Setting, DefaultValue(65)]
        public int PWShieldPercent { get; set; }
		
		[Setting, DefaultValue(60)]
        public int PWBarrierPercent { get; set; }
		
		[Setting, DefaultValue(20)]
        public int FlashHealPercent { get; set; }

		[Setting, DefaultValue(70)]
        public int DivineHymnPercent { get; set; }

        [Setting, DefaultValue(90)]
        public int PowerInfusionPercent { get; set; }
		
		[Setting, DefaultValue(87)]
        public int PrayerOfHealingPercent { get; set; }
		
		[Setting, DefaultValue(60)]
        public int PainSuppressionPercent { get; set; }

        [Setting, DefaultValue(30)]
        public int ManaPercent { get; set; }

        [Setting, DefaultValue(0)]
        public int HealthPercent { get; set; }


    }
}