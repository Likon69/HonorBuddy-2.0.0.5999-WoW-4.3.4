using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using HighVoltz.Composites;

namespace HighVoltz.Dynamic
{
    public class DynamicProperty<T> : IDynamicProperty
    {
        private string _compileError;
        private Func<object, T> _expressionMethod;

        public DynamicProperty() : this(null, "")
        {
        }

        public DynamicProperty(string code) : this(null, code)
        {
        }

        public DynamicProperty(IPBComposite parent, string code)
        {
            Code = code;
            _expressionMethod = context => default(T);
            AttachedComposite = parent;
        }

        public T Value
        {
            get { return _expressionMethod(AttachedComposite); }
        }

        #region IDynamicProperty Members

        public int CodeLineNumber { get; set; }

        public string CompileError
        {
            get { return _compileError; }
            set
            {
                if (value != "" || (value == "" && _compileError != ""))
                {
                    if (MainForm.IsValid)
                    {
                        if (AttachedComposite != null)
                        {
                            ((PBAction) AttachedComposite).Color = value != ""
                                                                       ? Color.Red
                                                                       : Color.Black;
                            MainForm.Instance.RefreshActionTree(AttachedComposite);
                        }
                        else
                            MainForm.Instance.RefreshActionTree();
                    }
                }
                if (MainForm.IsValid)
                    MainForm.Instance.ActionGrid.Refresh();
                _compileError = value;
            }
        }

        public CsharpCodeType CodeType
        {
            get { return CsharpCodeType.Expression; }
        }

        public virtual Delegate CompiledMethod
        {
            get { return _expressionMethod; }
            set { _expressionMethod = (Func<object, T>) value; }
        }

        public IPBComposite AttachedComposite { get; set; }

        public string Code { get; set; }

        public Type ReturnType
        {
            get { return typeof (T); }
        }

        #endregion

        public override string ToString()
        {
            return Code;
        }

        public static implicit operator T(DynamicProperty<T> exp)
        {
            return exp.Value;
        }

        #region Nested type: DynamivExpressionConverter

        public class DynamivExpressionConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof (DynamivExpressionConverter))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                             Type destinationType)
            {
                if (destinationType == typeof (String) && value is DynamicProperty<T>)
                {
                    return ((DynamicProperty<T>) value).Code;
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof (string))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                {
                    var ge = new DynamicProperty<T> {Code = (string) value};
                    return ge;
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        #endregion
    }
}