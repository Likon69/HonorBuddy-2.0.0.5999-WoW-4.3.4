using System;
using HighVoltz.Composites;

namespace HighVoltz.Dynamic
{
    public enum CsharpCodeType
    {
        BoolExpression,
        Statements,
        Declaration,
        Expression
    }

    public interface ICSharpCode
    {
        int CodeLineNumber { get; set; }
        string CompileError { get; set; }
        CsharpCodeType CodeType { get; }
        string Code { get; }
        Delegate CompiledMethod { get; set; }
        IPBComposite AttachedComposite { get; }
    }
}