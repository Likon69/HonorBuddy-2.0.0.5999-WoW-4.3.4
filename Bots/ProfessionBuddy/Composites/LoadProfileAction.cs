using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using Styx.Logic.Profiles;
using TreeSharp;

namespace HighVoltz.Composites
{

    #region LoadProfileAction

    public sealed class LoadProfileAction : PBAction
    {
        #region LoadProfileType enum

        public enum LoadProfileType
        {
            Honorbuddy,
            Professionbuddy
        }

        #endregion

        public LoadProfileAction()
        {
            Properties["Path"] = new MetaProp("Path", typeof (string),
                                              new EditorAttribute(typeof (PropertyBag.FileLocationEditor),
                                                                  typeof (UITypeEditor)),
                                              new DisplayNameAttribute(Pb.Strings["Action_Common_Path"]));

            Properties["ProfileType"] = new MetaProp("ProfileType", typeof (LoadProfileType),
                                                     new DisplayNameAttribute("Action_LoadProfileAction_ProfileType"));

            Path = "";
            ProfileType = LoadProfileType.Honorbuddy;
        }

        [PbXmlAttribute]
        public LoadProfileType ProfileType
        {
            get { return (LoadProfileType) Properties["ProfileType"].Value; }
            set { Properties["ProfileType"].Value = value; }
        }

        [PbXmlAttribute]
        public string Path
        {
            get { return (string) Properties["Path"].Value; }
            set { Properties["Path"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_LoadProfileAction_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("{0}: {1}", Name, Path); }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_LoadProfileAction_Help"]; }
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                Load();
                IsDone = true;
            }
            return RunStatus.Failure;
        }

        public void Load()
        {
            string absPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Pb.CurrentProfile.XmlPath), Path);
            if (ProfileManager.XmlLocation != absPath)
            {
                try
                {
                    Professionbuddy.Debug("Loading Profile :{0}, previous profile was {1}", absPath,
                                          ProfileManager.XmlLocation);
                    if (string.IsNullOrEmpty(Path))
                    {
                        ProfileManager.LoadEmpty();
                    }
                    else if (File.Exists(absPath))
                    {
                        ProfileManager.LoadNew(absPath, true);
                    }
                    else
                    {
                        Professionbuddy.Err("{0}: {1}", Pb.Strings["Error_UnableToFindProfile"], Path);
                    }
                }
                catch
                {
                }
            }
        }

        public override object Clone()
        {
            return new LoadProfileAction {Path = Path, ProfileType = ProfileType};
        }
    }

    #endregion
}