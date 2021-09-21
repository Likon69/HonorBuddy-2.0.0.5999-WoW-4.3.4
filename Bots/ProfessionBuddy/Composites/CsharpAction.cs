using System;
using System.ComponentModel;
using System.Drawing;
using HighVoltz.Dynamic;

namespace HighVoltz.Composites
{
    //this is a PBAction derived abstract class that adds functionallity for dynamically compiled Csharp expression/statement

    public abstract class CsharpAction : PBAction, ICSharpCode
    {
        private string _lastError = "";

        protected CsharpAction()
            : this(CsharpCodeType.Statements)
        {
        }

        protected CsharpAction(CsharpCodeType t)
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            CodeType = t;
            Properties["CompileError"] = new MetaProp("CompileError", typeof (string),
                                                      new ReadOnlyAttribute(true),
                                                      new DisplayNameAttribute(
                                                          Pb.Strings["Action_CSharpAction_CompileError"]));
            CompileError = "";
            Properties["CompileError"].Show = false;
            Properties["CompileError"].PropertyChanged += CompileErrorPropertyChanged;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public override Color Color
        {
            get { return string.IsNullOrEmpty(CompileError) ? base.Color : Color.Red; }
        }

        #region ICSharpCode Members

        public int CodeLineNumber { get; set; }

        public string CompileError
        {
            get { return (string) Properties["CompileError"].Value; }
            set { Properties["CompileError"].Value = value; }
        }

        public CsharpCodeType CodeType { get; protected set; }

        public virtual string Code { get; set; }

        public virtual Delegate CompiledMethod { get; set; }

        public IPBComposite AttachedComposite
        {
            get { return this; }
        }

        #endregion

        private void CompileErrorPropertyChanged(object sender, MetaPropArgs e)
        {
            if (CompileError != "" || (CompileError == "" && _lastError != ""))
            {
                MainForm.Instance.RefreshActionTree(this);
                Properties["CompileError"].Show = true;
            }
            else
                Properties["CompileError"].Show = false;
            RefreshPropertyGrid();
            _lastError = CompileError;
        }
    }
}