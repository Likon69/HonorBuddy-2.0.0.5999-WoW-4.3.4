using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;

namespace FelMaster
{
    public class FelMasterSettings : Settings
    {
        public static readonly FelMasterSettings Instance = new FelMasterSettings();

        public FelMasterSettings()
            : base(Path.Combine(Logging.ApplicationPath, string.Format(@"CustomClasses/Config/FelMaster1-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }
        [Setting, DefaultValue(true)]
        public bool T1 { get; set; }

        [Setting, DefaultValue(true)]
        public bool T2 { get; set; }

        [Setting, DefaultValue(true)]
        public bool SnD { get; set; }

        [Setting, DefaultValue(true)]
        public bool ND { get; set; }
    }
}