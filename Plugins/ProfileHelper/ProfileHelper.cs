using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Styx;
using Styx.Plugins.PluginClass;
using System.Windows.Forms;
using Styx.Helpers;

namespace ProfileHelper
{
    public class ProfileHelper : HBPlugin
    {
        #region Overrides

        public override string Author
        {
            get { return "raphus"; }
        }

        public override string ButtonText
        {
            get
            {
                return "Tools";
            }
        }

        public override string Name
        {
            get { return "ProfileHelper"; }
        }

        public override Version Version
        {
            get { return new Version(0, 0, 1); }
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
            new ToolForm().Show();
        }

        public override void Initialize()
        {
        }

        #endregion

        public override void Pulse()
        {
        }
    }
}
