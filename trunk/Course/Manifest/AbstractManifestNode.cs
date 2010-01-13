namespace FireFly.CourseEditor.Course.Manifest
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class AbstractManifestNode : IDisposable, IManifestNode
    {
        /// <summary>
        /// Determines parent node
        /// </summary>
        [XmlIgnore]
#if DEBUG
        [Category("~Debug")]
        [ReadOnly(true)]
#else
        [Browsable(false)]
#endif
        [NotNull]
        public IManifestNode Parent
        {
            get
            {
                if (IsInDeserializationMode)
                {
                    throw new InvalidOperationException("Parent cannot be accessed while deserialization is not finished");
                }
                return _Parent;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Parent cannot be null");
                }
                _Parent = value;
            }
        }

        /// <summary>
        /// Determines the unique id that specified this node
        /// </summary>
        [XmlIgnore]
#if DEBUG
        [Category("~Debug")]
        [ReadOnly(true)]
#else
        [Browsable(false)]
#endif
        [NotNull]
        public string UID { get { return _UID; } }

        public void ResolveTree([NotNull]IManifestNode parent)
        {
            // TODO: Precompile this code to expression by type
            Parent = parent;
            foreach (PropertyInfo p in GetType().GetProperties())
            {
                if (p.PropertyType.GetInterface("IManifestNode") != null && p.Name != "Parent" && p.CanWrite)
                {
                    object o = p.GetValue(this, null);
                    if (o != null)
                    {
                        ((IManifestNode)o).ResolveTree(this);
                    }
                }
            }
        }

        [NotNull]
        public override string ToString()
        {
            return (string)Properties.Settings.Default[GetType().Name + "_ToString"];
        }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr
        {
            get
            {
                return _AnyAttrField;
            }
            set
            {
                _AnyAttrField = value;
            }
        }

        #region IDisposable Members

        private void _dispose(object o)
        {
            if (o is IEnumerable)
            {
                foreach (object i in o as IEnumerable)
                {
                    _dispose(i);
                }

            }
            foreach (PropertyInfo p in GetType().GetProperties())
            {
                if (p.Name != "Parent")
                {
                    var po = p.GetValue(this, null) as IDisposable;
                    if (po != null)
                    {
                        po.Dispose();
                    }
                }
            }
        }

        public virtual void Dispose()
        {
            _dispose(this);
        }

        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IManifestNode _Parent;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private XmlAttribute[] _AnyAttrField;

        ///<summary>
        /// Generates unique UID to identify Manifest Nodes
        ///</summary>
        ///<returns>String representation of unique value</returns>
        public static string GetNextUID()
        {
            return Guid.NewGuid().ToString();
        }

        protected bool IsInDeserializationMode
        {
            get { return _Parent == null; }
        }

        private readonly string _UID = GetNextUID();
    }
}
