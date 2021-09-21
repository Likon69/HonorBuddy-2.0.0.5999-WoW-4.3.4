//!CompilerOption:AddRef:System.Design.dll

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Threading;
using HighVoltz.Dynamic;
using TreeSharp;

namespace HighVoltz.Composites
{
    public sealed class WaitAction : CsharpAction
    {
        private readonly Stopwatch _timeout = new Stopwatch();

        public WaitAction()
            : base(CsharpCodeType.BoolExpression)
        {
            Properties["Timeout"] = new MetaProp("Timeout", typeof (DynamicProperty<int>),
                                                 new TypeConverterAttribute(
                                                     typeof (DynamicProperty<int>.DynamivExpressionConverter)),
                                                 new DisplayNameAttribute(Pb.Strings["Action_Common_Timeout"]));

            Properties["Condition"] = new MetaProp("Condition", typeof (string),
                                                   new EditorAttribute(typeof (MultilineStringEditor),
                                                                       typeof (UITypeEditor)),
                                                   new DisplayNameAttribute(Pb.Strings["Action_WaitAction_Condition"]));

            Timeout = new DynamicProperty<int>(this, "2000");

            Condition = "false";
            CanRunDelegate = u => false;
        }

        public CanRunDecoratorDelegate CanRunDelegate { get; set; }

        [PbXmlAttribute]
        public string Condition
        {
            get { return (string) Properties["Condition"].Value; }
            set { Properties["Condition"].Value = value; }
        }

        [PbXmlAttribute]
        [TypeConverter(typeof (DynamicProperty<int>.DynamivExpressionConverter))]
        public DynamicProperty<int> Timeout
        {
            get { return (DynamicProperty<int>) Properties["Timeout"].Value; }
            set { Properties["Timeout"].Value = value; }
        }

        public override string Name
        {
            get { return Pb.Strings["Action_WaitAction_LongName"]; }
        }

        public override string Title
        {
            get
            {
                return string.Format("{0} ({1}) {2}:{3}",
                                     Pb.Strings["Action_WaitAction_Name"], Condition,
                                     Pb.Strings["Action_Common_Timeout"], Timeout);
            }
        }

        public override string Help
        {
            get { return Pb.Strings["Action_WaitAction_Help"]; }
        }

        public override string Code
        {
            get { return Condition; }
        }

        public override Delegate CompiledMethod
        {
            get { return CanRunDelegate; }
            set { CanRunDelegate = (CanRunDecoratorDelegate) value; }
        }

        protected override RunStatus Run(object context)
        {
            if (!IsDone)
            {
                if (!_timeout.IsRunning)
                    _timeout.Start();
                try
                {
                    if (_timeout.ElapsedMilliseconds >= Timeout || CanRunDelegate(null))
                    {
                        _timeout.Stop();
                        _timeout.Reset();
                        Professionbuddy.Debug("Wait Until {0} Completed", Condition);
                        IsDone = true;
                    }
                    else
                        return RunStatus.Success;
                }
                catch (Exception ex)
                {
                    if (ex.GetType() != typeof (ThreadAbortException))
                        Professionbuddy.Err("{0}:({1})\n{2}", Pb.Strings["Action_WaitAction_Name"], Condition, ex);
                }
            }
            return RunStatus.Failure;
        }

        public override object Clone()
        {
            return new WaitAction {Condition = Condition, Timeout = Timeout};
        }
    }
}