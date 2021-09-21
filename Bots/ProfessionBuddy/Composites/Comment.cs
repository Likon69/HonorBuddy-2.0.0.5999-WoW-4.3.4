using System.ComponentModel;
using System.Drawing;

namespace HighVoltz.Composites
{
    public class Comment : PBAction
    {
        public Comment()
        {
            Properties["Text"] = new MetaProp("Text", typeof (string),
                                              new DisplayNameAttribute(Pb.Strings["Action_Comment_Name"]));
        }

        public Comment(string comment) : this()
        {
            Text = comment;
        }

        [PbXmlAttribute]
        public string Text
        {
            get { return (string) Properties["Text"].Value; }
            set { Properties["Text"].Value = value; }
        }

        public override Color Color
        {
            get { return Color.DarkGreen; }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_Comment_Help"]; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_Comment_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("// {0}", Text); }
        }

        public override bool IsDone
        {
            get { return true; }
        }

        public override object Clone()
        {
            return new Comment {Text = Text};
        }
    }
}