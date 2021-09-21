using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using Styx.Logic;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using TreeSharp;

namespace HighVoltz.Composites
{
    public sealed class FlyToAction : PBAction
    {
        private WoWPoint _loc;

        public FlyToAction()
        {
            Properties["Dismount"] = new MetaProp("Dismount", typeof (bool),
                                                  new DisplayNameAttribute(Pb.Strings["Action_FlyToAction_Dismount"]));

            Properties["Location"] = new MetaProp("Location", typeof (string),
                                                  new EditorAttribute(typeof (PropertyBag.LocationEditor),
                                                                      typeof (UITypeEditor)),
                                                  new DisplayNameAttribute(Pb.Strings["Action_Common_Location"]));

            Location = _loc.ToInvariantString();
            Dismount = true;

            Properties["Location"].PropertyChanged += LocationChanged;
        }

        [PbXmlAttribute]
        public bool Dismount
        {
            get { return (bool) Properties["Dismount"].Value; }
            set { Properties["Dismount"].Value = value; }
        }

        [PbXmlAttribute]
        public string Location
        {
            get { return (string) Properties["Location"].Value; }
            set { Properties["Location"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_FlyToAction_Name"]; }
        }

        public override string Title
        {
            get { return string.Format("{0}: {1} ", Name, Location); }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_FlyToAction_Help"]; }
        }

        private void LocationChanged(object sender, MetaPropArgs e)
        {
            _loc = Util.StringToWoWPoint((string) ((MetaProp) sender).Value);
            Properties["Location"].PropertyChanged -= LocationChanged;
            Properties["Location"].Value = string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", _loc.X, _loc.Y,
                                                         _loc.Z);
            Properties["Location"].PropertyChanged += LocationChanged;
            RefreshPropertyGrid();
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if (ObjectManager.Me.Location.Distance(_loc) > 6)
                {
                    Flightor.MoveTo(_loc);
                    TreeRoot.StatusText = string.Format("Flying to location {0}", _loc);
                }
                else
                {
                    if (Dismount)
                        Mount.Dismount("Dismounting flying mount");
                    //Lua.DoString("Dismount() CancelShapeshiftForm()");
                    IsDone = true;
                    TreeRoot.StatusText = string.Format("Arrived at location {0}", _loc);
                }
                return RunStatus.Success;
            }
            return RunStatus.Failure;
        }

        public override object Clone()
        {
            return new FlyToAction {Location = Location, Dismount = Dismount};
        }
    }
}