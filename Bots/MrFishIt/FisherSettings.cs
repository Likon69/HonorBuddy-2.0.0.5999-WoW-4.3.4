using System.IO;
using System.Windows.Forms;
using Styx;
using Styx.Helpers;

namespace MrFishIt
{
    public class FisherSettings : Settings
    {
        public static FisherSettings Instance = new FisherSettings();

        public FisherSettings()
            : base(Path.Combine(Application.StartupPath, string.Format(@"Settings\MrFishIt_{0}.xml", StyxWoW.Me != null ? StyxWoW.Me.Name : "")))
        {
        }

        [Setting(Explanation = "The id of the lure this bot will use."), DefaultValue(0)]
        public int LureId { get; set; }

        [Setting(Explanation = "Use lure?"), DefaultValue(false)]
        public bool UseLure { get; set; }
    }
}
