using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Styx.Logic.BehaviorTree;
using Styx.Logic.Pathing;
using Styx.WoWInternals;

namespace HighVoltz
{

    #region PropertyBag

    public class MetaProp
    {
        private object val;

        public MetaProp(string name, Type type, params Attribute[] attributes)
        {
            Name = name;
            Type = type;
            Show = true;
            if (attributes != null)
            {
                Attributes = new Attribute[attributes.Length];
                attributes.CopyTo(Attributes, 0);
            }
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
        public Attribute[] Attributes { get; private set; }
        public bool Show { get; set; }

        public object Value
        {
            get { return val; }
            set
            {
                val = value;
                if (PropertyChanged != null) PropertyChanged(this, new MetaPropArgs(value));
            }
        }

        public event EventHandler<MetaPropArgs> PropertyChanged;
    }

    public class MetaPropArgs : EventArgs
    {
        public MetaPropArgs(object val)
        {
            Value = val;
        }

        public object Value { get; private set; }
    }

    public class PropertyBag : ICustomTypeDescriptor
    {
        private readonly Dictionary<string, MetaProp> metaPropList = new Dictionary<string, MetaProp>();

        public MetaProp this[string key]
        {
            get
            {
                MetaProp value;
                return metaPropList.TryGetValue(key, out value) ? value : null;
            }
            set
            {
                if (value == null)
                    metaPropList.Remove(key);
                else metaPropList[key] = value;
            }
        }

        public T GetValue<T>(string name)
        {
            return (T) this[name].Value;
        }

        #region Nested type: PropertyBagDescriptor

        public class PropertyBagDescriptor : PropertyDescriptor
        {
            private readonly Type type;

            public PropertyBagDescriptor(string name, Type type, Attribute[] attributes)
                : base(name, attributes)
            {
                this.type = type;
            }

            public override Type PropertyType
            {
                get { return type; }
            }

            public override bool IsReadOnly
            {
                get
                {
                    foreach (Attribute att in Attributes)
                    {
                        if (att is ReadOnlyAttribute)
                        {
                            var ro = att as ReadOnlyAttribute;
                            return ro.IsReadOnly;
                        }
                    }
                    return false;
                }
            }

            public override Type ComponentType
            {
                get { return typeof (PropertyBag); }
            }

            public override bool SupportsChangeEvents
            {
                get { return true; }
            }

            public override TypeConverter Converter
            {
                get
                {
                    foreach (Attribute att in Attributes)
                    {
                        if (att is TypeConverterAttribute)
                        {
                            var tc = att as TypeConverterAttribute;
                            return (TypeConverter) Activator.CreateInstance(Type.GetType(tc.ConverterTypeName));
                        }
                    }
                    return base.Converter;
                }
            }

            public override object GetValue(object component)
            {
                return ((PropertyBag) component)[Name].Value;
            }

            public override void SetValue(object component, object value)
            {
                ((PropertyBag) component)[Name].Value = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return GetValue(component) != null;
            }

            public override bool CanResetValue(object component)
            {
                return true;
            }

            public override void ResetValue(object component)
            {
                SetValue(component, null);
            }
        }

        #endregion

        #region ICustomTypeDescriptor definitions

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor) this).GetProperties(new Attribute[0]);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] metaProps = (from prop in metaPropList.Values
                                              where prop.Show
                                              select new PropertyBagDescriptor(prop.Name, prop.Type, prop.Attributes)).
                ToArray();
            return new PropertyDescriptorCollection(metaProps);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        #region UITypeEditors and Type Converters

        #region Nested type: EntryEditor

        public class EntryEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (ObjectManager.IsInGame)
                {
                    if (!TreeRoot.IsRunning)
                        ObjectManager.Update();
                    return ObjectManager.Me.GotTarget ? ObjectManager.Me.CurrentTarget.Entry : 0;
                }
                return value;
            }
        }

        #endregion

        #region GoldEditor

        #region Nested type: GoldEditor

        public class GoldEditor
        {
            private uint copper;
            private uint gold;

            private uint silver;

            public GoldEditor()
            {
                Gold = 0;
                Silver = 0;
                Copper = 0;
            }

            public GoldEditor(string gold)
                : this()
            {
                SetValues(gold);
            }

            [RefreshProperties(RefreshProperties.Repaint)]
            public uint Gold
            {
                get { return gold; }
                set
                {
                    gold = value;
                    if (OnChanged != null)
                        OnChanged(this, null);
                }
            }

            [RefreshProperties(RefreshProperties.Repaint)]
            public uint Silver
            {
                get { return silver; }
                set
                {
                    silver = value > 99 ? 99 : value;
                    if (OnChanged != null)
                        OnChanged(this, null);
                }
            }

            [RefreshProperties(RefreshProperties.Repaint)]
            public uint Copper
            {
                get { return copper; }
                set
                {
                    copper = value > 99 ? 99 : value;
                    if (OnChanged != null)
                        OnChanged(this, null);
                }
            }

            [Browsable(false)]
            public uint TotalCopper
            {
                get { return Copper + (Silver*100) + (Gold*10000); }
            }

            public event EventHandler OnChanged;

            public bool SetValues(string values)
            {
                try
                {
                    int gI = values.IndexOf('g');
                    int sI = values.IndexOf('s');
                    int cI = values.IndexOf('c');
                    string g = values.Substring(0, gI);
                    string s = values.Substring(gI + 1, (sI - gI) - 1);
                    string c = values.Substring(sI + 1, (cI - sI) - 1);
                    uint gold, silver, copper;
                    uint.TryParse(g, out gold);
                    uint.TryParse(s, out silver);
                    uint.TryParse(c, out copper);
                    Gold = gold;
                    Silver = silver > 99 ? 99 : silver;
                    Copper = copper > 99 ? 99 : copper;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public override string ToString()
            {
                return string.Format("{0}g{1}s{2}c", Gold, Silver, Copper);
            }
        }

        #endregion

        #region Nested type: GoldEditorConverter

        [TypeConverter(typeof (GoldEditorConverter))]
        public class GoldEditorConverter : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof (GoldEditorConverter))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                             Type destinationType)
            {
                if (destinationType == typeof (String) && value is GoldEditor)
                {
                    var ge = (GoldEditor) value;
                    return ge.ToString();
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
                    var ge = new GoldEditor();
                    if (!ge.SetValues((string) value))
                        throw new ArgumentException("Can not convert '" + (string) value + "' to type GoldEditor");
                    return ge;
                }
                return base.ConvertFrom(context, culture, value);
            }
        }

        #endregion

        #endregion

        #region Nested type: FileLocationEditor

        public class FileLocationEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (var ofd = new OpenFileDialog())
                {
                    string pbPath = Path.GetDirectoryName(Professionbuddy.Instance.MySettings.LastProfile);
                    if (string.IsNullOrEmpty(pbPath))
                    {
                        MessageBox.Show("Please save your profile 1st");
                        return "";
                    }
                    ofd.Filter = "Xml files|*.xml|All files|*.*";
                    ofd.InitialDirectory = pbPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName.Contains(pbPath))
                        {
                            string relative = ofd.FileName.Substring(pbPath.Length + 1);
                            return relative;
                        }
                        else
                        {
                            MessageBox.Show(
                                "File needs to be in same folder or in a subfolder from your professionbuddy profile");
                            return "";
                        }
                    }
                }
                return value;
            }
        }

        #endregion

        #region Nested type: LocationEditor

        public class LocationEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (ObjectManager.IsInGame)
                {
                    if (!TreeRoot.IsRunning)
                        ObjectManager.Update();
                    WoWPoint loc = ObjectManager.Me.GotTarget
                                       ? ObjectManager.Me.CurrentTarget.Location
                                       : ObjectManager.Me.Location;
                    return string.Format("{0}, {1}, {2}", loc.X, loc.Y, loc.Z);
                }
                return value;
            }
        }

        #endregion

        #endregion
    }

    #endregion
}