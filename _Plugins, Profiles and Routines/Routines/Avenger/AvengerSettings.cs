using System.IO;
using Styx;
using Styx.Helpers;

namespace Avenger
{
    public class AvengerSettings : Settings
    {
        public static readonly AvengerSettings Instance = new AvengerSettings();

        public AvengerSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/Avenger-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(4)]
        public int AoEMobs { get; set; }

        [Setting, DefaultValue(true)]
        public bool Buff { get; set; }

        [Setting, DefaultValue(true)]
        public bool Trinket1 { get; set; }

        [Setting, DefaultValue(true)]
        public bool Trinket2 { get; set; }

        [Setting, DefaultValue(true)]
        public bool SynapseSprings { get; set; }

        [Setting, DefaultValue(true)]
        public bool Zealotry { get; set; }

        [Setting, DefaultValue(true)]
        public bool AvengingWrath { get; set; }

        [Setting, DefaultValue(true)]
        public bool GOAK { get; set; }

        [Setting, DefaultValue(false)]
        public bool DivineProtection { get; set; }

        [Setting, DefaultValue(true)]
        public bool DivineStorm { get; set; }

        [Setting, DefaultValue(true)]
        public bool SOTR { get; set; }

        [Setting, DefaultValue(false)]
        public bool LoH { get; set; }

        [Setting, DefaultValue(false)]
        public bool DivineShield { get; set; }

        [Setting, DefaultValue(false)]
        public bool Rebuke { get; set; }

    }
}