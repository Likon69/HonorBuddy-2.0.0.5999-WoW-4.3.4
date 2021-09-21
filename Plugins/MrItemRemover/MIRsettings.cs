using System.IO;
using Styx;
using Styx.Helpers;

namespace MrItemRemover
{
    public class MIRsettings : Settings
    {
        public static readonly MIRsettings Instance = new MIRsettings();

        public MIRsettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"Plugins/MrItemRemover/MrImageRemover-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }
        
        [Setting, DefaultValue(false)]
        public bool GrayItems { get; set; }

        [Setting, DefaultValue(false)]
        public bool SellGray { get; set; }

        [Setting, DefaultValue(false)]
        public bool SellGreen { get; set; }

        [Setting, DefaultValue(false)]
        public bool SellWhite { get; set; }

        [Setting, DefaultValue(true)]
        public bool EnableSell { get; set; }

        [Setting, DefaultValue(true)]
        public bool EnableRemove { get; set; }

        [Setting, DefaultValue(false)]
        public bool QuestItems { get; set; }

        [Setting, DefaultValue(2)]
        public int GoldGrays { get; set; }

        [Setting, DefaultValue(25)]
        public int SilverGrays { get; set; }

        [Setting, DefaultValue(10)]
        public int CopperGrays { get; set; }

        [Setting, DefaultValue(3)]
        public int MinPass { get; set; }
    }
}
