using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using HighVoltz.Dynamic;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = TreeSharp.Action;

namespace HighVoltz.Composites
{
    public interface IPBComposite : ICloneable
    {
        PropertyBag Properties { get; }
        string Name { get; }
        string Title { get; }
        Color Color { get; }
        string Help { get; }
        bool IsDone { get; }
        void Reset();
        void OnProfileLoad(XElement element);
        void OnProfileSave(XElement element);
    }

    #region PBAction

    public abstract class PBAction : Action, IPBComposite
    {
        protected LocalPlayer Me = ObjectManager.Me;
        protected Professionbuddy Pb;
        [XmlIgnore] private Color _color = Color.Black;

        protected PBAction()
        {
            HasRunOnce = false;
            Pb = Professionbuddy.Instance;
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Properties = new PropertyBag();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
            Expressions = new ListDictionary();
        }

        public virtual ListDictionary Expressions { get; private set; }

        protected PropertyGrid PropertyGrid
        {
            get { return MainForm.IsValid ? MainForm.Instance.ActionGrid : null; }
        }

        protected bool HasRunOnce { get; set; }

        #region IPBComposite Members

        public virtual string Help
        {
            get { return ""; }
        }

        public virtual string Name
        {
            get { return "PBAction"; }
        }

        public virtual string Title
        {
            get { return string.Format("({0})", Name); }
        }

        public virtual Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public virtual bool IsDone { get; protected set; }

        public virtual PropertyBag Properties { get; protected set; }

        public virtual object Clone()
        {
            return this;
        }

        public virtual void Reset()
        {
            IsDone = false;
            HasRunOnce = false;
        }

        public virtual void OnProfileLoad(XElement element)
        {
        }

        public virtual void OnProfileSave(XElement element)
        {
        }

        #endregion

        protected void RefreshPropertyGrid()
        {
            if (PropertyGrid != null)
            {
                PropertyGrid.Refresh();
            }
        }

        protected void RegisterDynamicProperty(string propName)
        {
            Properties[propName].PropertyChanged += DynamicPropertyChanged;
        }

        /// <summary>
        /// If overriding this method call base method or set HasRunOnce to false.
        /// </summary>
        protected virtual void RunOnce()
        {
            HasRunOnce = true;
        }

        private void DynamicPropertyChanged(object sender, MetaPropArgs e)
        {
            ((IDynamicProperty) e.Value).AttachedComposite = this;
            DynamicCodeCompiler.CodeWasModified = true;
        }
    }

    #endregion
}