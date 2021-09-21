using System.ComponentModel;
using TreeSharp;

namespace HighVoltz.Composites
{
    public sealed class ChangeBotAction : PBAction
    {
        public ChangeBotAction()
        {
            Properties["BotName"] = new MetaProp("BotName", typeof (string),
                                                 new DisplayNameAttribute(Pb.Strings["Action_ChangeBotAction_BotName"]));
            BotName = "";
        }

        [PbXmlAttribute]
        public string BotName
        {
            get { return (string) Properties["BotName"].Value; }
            set { Properties["BotName"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_ChangeBotAction_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("{0}: {1}", Name, BotName); }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_ChangeBotAction_Help"]; }
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                try
                {
                    Professionbuddy.ChangeSecondaryBot(BotName);
                }
                finally
                {
                    IsDone = true;
                }
            }
            return RunStatus.Failure;
        }

        public override object Clone()
        {
            return new ChangeBotAction {BotName = BotName};
        }
    }
}