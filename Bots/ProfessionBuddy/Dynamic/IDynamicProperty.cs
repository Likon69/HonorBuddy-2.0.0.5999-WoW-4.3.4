using System;
using HighVoltz.Composites;

namespace HighVoltz.Dynamic
{
    internal interface IDynamicProperty : ICSharpCode
    {
        /// <summary>
        /// This is the IPBComposite that this propery belongs to. It's set at compile time
        /// </summary>
        new IPBComposite AttachedComposite { get; set; }

        Type ReturnType { get; }
    }
}