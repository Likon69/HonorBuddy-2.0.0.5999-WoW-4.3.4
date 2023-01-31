using Styx;
using Styx.Helpers;

namespace RogueAssassin
{
    public class RASettings : Settings
    {
        public RASettings()
            : base(string.Format("{0}\\Settings\\RogueAssassin_{1}.xml", Logging.ApplicationPath, StyxWoW.Me.Name))
        {
            Load();
        }

        [Setting, DefaultValue(4)]
        public int FOKMinTargets { get; set; }

        [Setting, DefaultValue(false)]
        public bool FOKOnBoss { get; set; }

        [Setting, DefaultValue(true)]
        public bool ColdBloodBossOnly { get; set; }

        [Setting, DefaultValue(12f)]
        public float FOKRange { get; set; }

        [Setting, DefaultValue(true)]
        public bool FOK { get; set; }

        [Setting, DefaultValue(false)]
        public bool ItemsBossOnly { get; set; }

        [Setting, DefaultValue(true)]
        public bool Vanish { get; set; }

        [Setting, DefaultValue(true)]
        public bool VanishBossOnly { get; set; }

        [Setting, DefaultValue(true)]
        public bool VendettaBossOnly { get; set; }

        ~RASettings()
        {
            Save();
        }
    }
}