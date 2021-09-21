//!CompilerOption:AddRef:System.Design.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using HighVoltz.Dynamic;
using TreeSharp;

namespace HighVoltz.Composites
{
    public class If : GroupComposite, ICSharpCode, IPBComposite
    {
        #region Properties

        protected PropertyGrid PropertyGrid
        {
            get { return MainForm.IsValid ? MainForm.Instance.ActionGrid : null; }
        }

        [XmlIgnore]
        public virtual PropertyBag Properties { get; private set; }

        protected void RefreshPropertyGrid()
        {
            if (PropertyGrid != null)
            {
                PropertyGrid.Refresh();
            }
        }

        #endregion

        protected static readonly object LockObject = new object();
        protected bool _isRunning;
        private string _lastError = "";

        public If()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Properties = new PropertyBag();
            Properties["IgnoreCanRun"] = new MetaProp("IgnoreCanRun", typeof (bool),
                                                      new DisplayNameAttribute(
                                                          Professionbuddy.Instance.Strings["FlowControl_If_IgnoreCanRun"
                                                              ]));

            Properties["Condition"] = new MetaProp("Condition",
                                                   typeof (string),
                                                   new EditorAttribute(typeof (MultilineStringEditor),
                                                                       typeof (UITypeEditor)),
                                                   new DisplayNameAttribute(
                                                       Professionbuddy.Instance.Strings["FlowControl_If_Condition"]));

            Properties["CompileError"] = new MetaProp("CompileError", typeof (string), new ReadOnlyAttribute(true),
                                                      new DisplayNameAttribute(
                                                          Professionbuddy.Instance.Strings[
                                                              "Action_CSharpAction_CompileError"]));

            CanRunDelegate = c => false;
            Condition = "";
            CompileError = "";
            Properties["CompileError"].Show = false;

            Properties["Condition"].PropertyChanged += Condition_PropertyChanged;
            Properties["CompileError"].PropertyChanged += CompileErrorPropertyChanged;
            IgnoreCanRun = true;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public virtual CanRunDecoratorDelegate CanRunDelegate { get; set; }

        [PbXmlAttribute]
        public virtual string Condition
        {
            get { return (string) Properties["Condition"].Value; }
            set { Properties["Condition"].Value = value; }
        }

        [PbXmlAttribute]
        public virtual bool IgnoreCanRun
        {
            get { return (bool) Properties["IgnoreCanRun"].Value; }
            set { Properties["IgnoreCanRun"].Value = value; }
        }

        #region ICSharpCode Members

        public virtual Delegate CompiledMethod
        {
            get { return CanRunDelegate; }
            set { CanRunDelegate = (CanRunDecoratorDelegate) value; }
        }

        public virtual string Code
        {
            get { return Condition; }
            set { Condition = value; }
        }

        public int CodeLineNumber { get; set; }

        public string CompileError
        {
            get { return (string) Properties["CompileError"].Value; }
            set { Properties["CompileError"].Value = value; }
        }

        public CsharpCodeType CodeType
        {
            get { return CsharpCodeType.BoolExpression; }
        }

        public IPBComposite AttachedComposite
        {
            get { return this; }
        }

        #endregion

        #region IPBComposite Members

        public virtual void Reset()
        {
            _isRunning = IsDone = false;
            Selection = null;
            recursiveReset(this);
        }

        /// <summary>
        /// Returns true if the If Condition is finished executing its children or condition isn't met.
        /// </summary>
        public virtual bool IsDone { get; set; }

        public virtual Color Color
        {
            get { return string.IsNullOrEmpty(CompileError) ? Color.Blue : Color.Red; }
        }

        public virtual string Name
        {
            get { return Professionbuddy.Instance.Strings["FlowControl_If_LongName"]; }
        }

        public virtual string Title
        {
            get
            {
                return string.IsNullOrEmpty(Condition)
                           ? Professionbuddy.Instance.Strings["FlowControl_If_LongName"]
                           : (Professionbuddy.Instance.Strings["FlowControl_If_Name"] + " (" + Condition + ")");
            }
        }


        public virtual object Clone()
        {
            var pd = new If
                         {
                             CanRunDelegate = CanRunDelegate,
                             Condition = Condition,
                             IgnoreCanRun = IgnoreCanRun
                         };
            return pd;
        }

        public virtual string Help
        {
            get { return Professionbuddy.Instance.Strings["FlowControl_If_Help"]; }
        }

        public void OnProfileLoad(XElement element)
        {
        }

        public void OnProfileSave(XElement element)
        {
        }

        #endregion

        private void Condition_PropertyChanged(object sender, EventArgs e)
        {
            DynamicCodeCompiler.CodeWasModified = true;
        }

        private void CompileErrorPropertyChanged(object sender, EventArgs e)
        {
            if (CompileError != "" || (CompileError == "" && _lastError != ""))
                MainForm.Instance.RefreshActionTree(this);
            Properties["CompileError"].Show = CompileError != "";
            RefreshPropertyGrid();
            _lastError = CompileError;
        }

        protected virtual bool CanRun(object context)
        {
            try
            {
                return CanRunDelegate(context);
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof (ThreadAbortException))
                    Professionbuddy.Err("{0}: {1}\nErr:{2}", Professionbuddy.Instance.Strings["FlowControl_If_LongName"],
                                        Condition, ex);
                return false;
            }
        }

        protected override IEnumerable<RunStatus> Execute(object context)
        {
            if (!IsDone && ((_isRunning && IgnoreCanRun) || CanRun(context)))
            {
                _isRunning = true;
                bool shouldBreak = false;
                foreach (Composite child in Children.SkipWhile(c => Selection != null && c != Selection))
                {
                    child.Start(context);
                    Selection = child;
                    while (child.Tick(context) == RunStatus.Running)
                    {
                        if (!IgnoreCanRun && !CanRun(context))
                        {
                            shouldBreak = true;
                            break;
                        }
                        yield return RunStatus.Running;
                    }
                    if (shouldBreak)
                        break;
                    if (child.LastStatus == RunStatus.Success)
                        yield return RunStatus.Success;
                }
                Selection = null;
                IsDone = true;
                _isRunning = false;
            }
            yield return RunStatus.Failure;
        }

        private void recursiveReset(If gc)
        {
            foreach (IPBComposite comp in gc.Children)
            {
                comp.Reset();
                if (comp is If)
                    recursiveReset(comp as If);
            }
        }
    }
}