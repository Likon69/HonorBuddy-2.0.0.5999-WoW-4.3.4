using System;
using System.ComponentModel;

namespace HighVoltz.Composites
{
    public sealed class Settings : PBAction
    {
        public Settings()
        {
            Properties["DefaultValue"] = new MetaProp("DefaultValue", typeof (string),
                                                      new DisplayNameAttribute(Pb.Strings["Action_Common_DefaultValue"]));

            Properties["Type"] = new MetaProp("Type", typeof (TypeCode),
                                              new DisplayNameAttribute(Pb.Strings["Action_Common_Type"]));

            Properties["Name"] = new MetaProp("Name", typeof (string),
                                              new DisplayNameAttribute(Pb.Strings["Action_Common_Name"]));

            Properties["Summary"] = new MetaProp("Summary", typeof (string),
                                                 new DisplayNameAttribute(Pb.Strings["Action_Common_Summary"]));

            Properties["Category"] = new MetaProp("Category", typeof (string),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Category"]));

            Properties["Global"] = new MetaProp("Global", typeof (bool),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_Global"]));

            Properties["Hidden"] = new MetaProp("Hidden", typeof (bool),
                                                new DisplayNameAttribute(Pb.Strings["Action_Common_Hidden"]));

            DefaultValue = "true";
            Type = TypeCode.Boolean;
            SettingName = Pb.Strings["Action_Common_SettingName"];
            Summary = Pb.Strings["Action_Common_SummaryExample"];
            Category = "Misc";
            Global = false;
            Hidden = false;
        }

        [PbXmlAttribute]
        public string DefaultValue
        {
            get { return (string) Properties["DefaultValue"].Value; }
            set { Properties["DefaultValue"].Value = value; }
        }

        [PbXmlAttribute]
        public TypeCode Type
        {
            get { return (TypeCode) Properties["Type"].Value; }
            set { Properties["Type"].Value = value; }
        }

        [PbXmlAttribute("Name")]
        public string SettingName
        {
            get { return (string) Properties["Name"].Value; }
            set { Properties["Name"].Value = value; }
        }

        [PbXmlAttribute]
        public string Summary
        {
            get { return (string) Properties["Summary"].Value; }
            set { Properties["Summary"].Value = value; }
        }

        [PbXmlAttribute]
        public string Category
        {
            get { return (string) Properties["Category"].Value; }
            set { Properties["Category"].Value = value; }
        }

        [PbXmlAttribute]
        public bool Global
        {
            get { return (bool) Properties["Global"].Value; }
            set { Properties["Global"].Value = value; }
        }

        [PbXmlAttribute]
        public bool Hidden
        {
            get { return (bool) Properties["Hidden"].Value; }
            set { Properties["Hidden"].Value = value; }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_Settings_Help"]; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_Settings_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("{0}: {1} {2}={3}", Name, Type, SettingName, DefaultValue); }
        }

        public override bool IsDone
        {
            get { return true; }
        }

        public override object Clone()
        {
            return new Settings
                       {
                           DefaultValue = DefaultValue,
                           SettingName = SettingName,
                           Type = Type,
                           Summary = Summary,
                           Category = Category,
                           Global = Global,
                           Hidden = Hidden
                       };
        }
    }
}