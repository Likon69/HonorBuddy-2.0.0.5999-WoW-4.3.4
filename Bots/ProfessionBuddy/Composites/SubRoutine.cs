using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using TreeSharp;

namespace HighVoltz.Composites
{
    internal sealed class SubRoutine : GroupComposite, IPBComposite
    {
        public SubRoutine()
        {
            Properties = new PropertyBag();
            Properties["SubRoutineName"] = new MetaProp("SubRoutineName", typeof (string),
                                                        new DisplayNameAttribute(
                                                            Professionbuddy.Instance.Strings[
                                                                "Action_SubRoutine_SubroutineName"]));
            SubRoutineName = "";
        }

        [PbXmlAttribute]
        public string SubRoutineName
        {
            get { return (string) Properties["SubRoutineName"].Value; }
            set { Properties["SubRoutineName"].Value = value; }
        }

        #region IPBComposite Members

        public Color Color
        {
            get { return Color.Blue; }
        }

        public string Name
        {
            get { return Professionbuddy.Instance.Strings["Action_SubRoutine_Name"]; }
        }

        public string Title
        {
            get { return string.Format("Sub {0}", SubRoutineName); }
        }

        public PropertyBag Properties { get; private set; }

        public void Reset()
        {
            Selection = null;
            IsDone = false;
            recursiveReset(this);
        }

        public bool IsDone { get; set; }

        public object Clone()
        {
            var pd = new SubRoutine
                         {
                             SubRoutineName = SubRoutineName,
                         };
            return pd;
        }

        public string Help
        {
            get { return Professionbuddy.Instance.Strings["Action_SubRoutine_Help"]; }
        }

        public void OnProfileLoad(XElement element)
        {
        }

        public void OnProfileSave(XElement element)
        {
        }

        #endregion

        protected override IEnumerable<RunStatus> Execute(object context)
        {
            if (context == null || !(context is string) || (string) context != SubRoutineName)
                yield return RunStatus.Failure;
            foreach (Composite child in Children.SkipWhile(c => Selection != null && c != Selection))
            {
                child.Start(context);
                Selection = child;
                while (child.Tick(context) == RunStatus.Running)
                {
                    yield return RunStatus.Running;
                }
                if (child.LastStatus == RunStatus.Success)
                    yield return RunStatus.Success;
            }
            Selection = null;
            IsDone = true;
            yield return RunStatus.Failure;
        }

        private void recursiveReset(GroupComposite gc)
        {
            foreach (IPBComposite comp in gc.Children)
            {
                comp.Reset();
                if (comp is GroupComposite)
                    recursiveReset(comp as GroupComposite);
            }
        }
    }
}