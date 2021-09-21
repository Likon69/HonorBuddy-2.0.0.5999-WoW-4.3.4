//!CompilerOption:AddRef:System.Design.dll

using System.Collections.Generic;
using System.Linq;
using TreeSharp;

namespace HighVoltz.Composites
{
    internal class While : If
    {
        public override string Name
        {
            get { return Professionbuddy.Instance.Strings["FlowControl_While_LongName"]; }
        }

        public override string Title
        {
            get
            {
                return string.IsNullOrEmpty(Condition)
                           ? Professionbuddy.Instance.Strings["FlowControl_While_LongName"]
                           : (Professionbuddy.Instance.Strings["FlowControl_While_Name"] + " (" + Condition + ")");
            }
        }

        public override string Help
        {
            get { return Professionbuddy.Instance.Strings["FlowControl_While_Help"]; }
        }

        protected override IEnumerable<RunStatus> Execute(object context)
        {
            if ((_isRunning && IgnoreCanRun) || CanRun(context))
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
                Reset();
                Selection = null;
                if (!shouldBreak && CanRun(context))
                {
                    PbDecorator.EndOfWhileLoopReturn = true;
                    yield return RunStatus.Success;
                }
                _isRunning = false;
            }
            yield return RunStatus.Failure;
        }

        public override object Clone()
        {
            var w = new While
                        {
                            CanRunDelegate = CanRunDelegate,
                            Condition = Condition,
                            IgnoreCanRun = IgnoreCanRun
                        };
            return w;
        }
    }
}