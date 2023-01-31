using System.IO;
using Styx;
using Styx.Helpers;

namespace ProtectAndServe
{
    public class ProtectAndServeSettings : Settings
    {
        public static readonly ProtectAndServeSettings Instance = new ProtectAndServeSettings();

        public ProtectAndServeSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/ProtectAndServe-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(15)]
        public int LayonHandsPercent { get; set; }

        [Setting, DefaultValue(50)]
        public int GuardianoftheancientKingsPercent { get; set; }

        [Setting, DefaultValue(20)]
        public int ArdentDefenderPercent { get; set; }

        [Setting, DefaultValue(95)]
        public int DivineProtectionPercent { get; set; }

        [Setting, DefaultValue(95)]
        public int HolyShieldPercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseDefensiveCooldowns { get; set; }

        [Setting, DefaultValue(true)]
        public bool Buff { get; set; }

    }
}