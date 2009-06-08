using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using FireFly.CourseEditor.GUI.HtmlEditor;

namespace FireFly.CourseEditor.Common
{
    using Course;

    public class FFTypeDescriptor : ICustomTypeDescriptor
    {
        private readonly PropertyDescriptorCollection _DescriptorCollection;
        private readonly object _WrappedObject;

        private static readonly Dictionary<Type, PropertyDescriptorCollection> __TypedDescriptorCollection = new Dictionary<Type, PropertyDescriptorCollection>();

        public FFTypeDescriptor([NotNull]object targetObject)
        {
            _WrappedObject = targetObject;

            Type type = targetObject.GetType();
            PropertyDescriptorCollection pdc;
            if (!__TypedDescriptorCollection.TryGetValue(type, out pdc))
            {
                pdc = new PropertyDescriptorCollection(null);
                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(type))
                {
                    var desc = new MyPropDesc(pd);
                    desc.ValueChanging += Property_ValueChanging;
                    pdc.Add(desc);
                }
                foreach (BindPropertyAttribute a in type.GetCustomAttributes<BindPropertyAttribute>())
                {
                    var childProp = a.GetSourcePropertyInfo(type);
                    var v = childProp.GetValue(targetObject, null);
                    var pdcs = TypeDescriptor.GetProperties(v, false);
                    var bpd = new BindPropertyDescriptor(pdcs[a.Property], childProp, a.DisplayName, a.Description, a.Category);
                    bpd.ValueChanging += Property_ValueChanging;
                    pdc.Add(bpd);
                }
   
                __TypedDescriptorCollection.Add(type, pdc);
            }
            _DescriptorCollection = pdc;
        }

        [NotNull]
        public object Unwrap
        {
            get { return _WrappedObject; }
        }

        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection(null);
        }

        public string GetClassName()
        {
            return null;
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return null;
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        public EventDescriptorCollection GetEvents
            (Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return _DescriptorCollection;
        }

        public PropertyDescriptorCollection GetProperties(
            Attribute[] attributes)
        {
            return _DescriptorCollection;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public event Action<object, string, object, object> ValueChanging; // Don't use add and remove methods to redirect event to MyPropDesc correspondent event!!!. And don't make it static

        private void Property_ValueChanging(object component, string property, object oldValue, object newValue)
        {
            if (ValueChanging != null)
            {
                ValueChanging(((FFTypeDescriptor)component).Unwrap, property, oldValue, newValue);
            }
        }
    }

    public class BindPropertyDescriptor : MyPropDesc
    {
        private readonly PropertyInfo _ChildPropInfo;
        private readonly string _DisplayName;
        private readonly string _Description;
        private readonly string _Category;

        public BindPropertyDescriptor([NotNull]PropertyDescriptor p, [NotNull]PropertyInfo childProp, string displayName, string description, string category)
            : base(p)
        {
            _ChildPropInfo = childProp;
            _DisplayName = displayName;
            _Description = description;
            _Category = category;
        }

        public override string DisplayName
        {
            get
            {
                return _DisplayName ?? base.DisplayName;
            }
        }

        public override string Description
        {
            get
            {
                return _Description ?? base.Description;
            }
        }

        public override string Category
        {
            get
            {
                return _Category ?? base.Category;
            }
        }

        protected override object GetObject(object component)
        {
            return _ChildPropInfo.GetValue(base.GetObject(component), null);
        }
    }

    public class MyPropDesc : PropertyDescriptor
    {
        protected PropertyDescriptor propDesc;

        public MyPropDesc([NotNull]PropertyDescriptor PropDesc)
            : base(PropDesc)
        {
            propDesc = PropDesc;
        }

        public override Type ComponentType
        {
            get { return propDesc.ComponentType; }
        }

        public override bool IsReadOnly
        {
            get { return propDesc.IsReadOnly; }
        }

        public override Type PropertyType
        {
            get { return propDesc.PropertyType; }
        }

        protected virtual object GetObject(object component)
        {
            return ((FFTypeDescriptor) component).Unwrap;
        }

        public override bool CanResetValue(object component)
        {
            return propDesc.CanResetValue(GetObject(component));
        }

        public override object GetValue(object component)
        {
            return propDesc.GetValue(GetObject(component));
        }

        public override void ResetValue(object component)
        {
            propDesc.ResetValue(GetObject(component));
        }

        public override void SetValue(object component, object value)
        {
            if (ValueChanging != null)
            {
                ValueChanging(component, Name, GetValue(component), value);
            }
            Course.NotifyChanged();
            propDesc.SetValue(GetObject(component), value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public event Action<object, string, object, object> ValueChanging;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    [DebuggerDisplay("{ToString()}")]
    [BaseTypeRequired(typeof(HtmlControl), typeof(FFConfig))]
    public class BindPropertyAttribute : Attribute
    {
        [NotNull]
        public readonly string Source;
        [NotNull]
        public readonly string Property;

        [CanBeNull]
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        [CanBeNull]
        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        [CanBeNull]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
            
        }

        public BindPropertyAttribute([NotNull]string source, [NotNull] string property)
        {
            Source = source;
            Property = property;
        }

        [NotNull]
        public override string ToString()
        {
            return string.Format("BindPropertyAttribute. Source = {0}, Property = {1}", Source, Property);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _DisplayName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Category;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Description;

        public PropertyInfo GetSourcePropertyInfo([NotNull]Type t)
        {
            return t.GetProperty(Source, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }

        public PropertyInfo GetBindedPropertyInfo([NotNull]PropertyInfo sourceProperty)
        {
            return sourceProperty.PropertyType.GetProperty(Property);
        }
    }
}