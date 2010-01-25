using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.Course.Manifest
{
    [XmlType("organization", Namespace = ManifestNamespaces.Imscp)]
    [Description("Element describes a particular hierarchical organization. The content of organization can be a lesson, module, course, chapter, etc")]
    [Category("Main")]
    public class OrganizationType : AbstractManifestNode, IItemContainer, ITitled
    {
        private static int number = 1;

        private ManifestNodeList<ItemType> m_SubItems;

        private MetadataType metadataField;
        
        private string identifierField;

        private string structureField;

        private SequencingType sequencingField;

        public OrganizationType()
        {
            identifier = string.Concat("Organization", number++);
            structureField = "hierarchical";
            this.Sequencing = SequencingManager.CreateOrganizationDefaultSequencing();
        }

        //TODO: Fix FFServer and remove this stub
        [XmlElement("title")]
        public string _TitleStub
        {
            get { return Title; }
            set
            {
                // just ignore it
            }
        }

        [XmlIgnore]
        public string Title
        {
            get
            {
                return IsInDeserializationMode ? null : ((ManifestType)Parent.Parent.Parent).Title;
            }
            set
            {
                if (IsInDeserializationMode)
                {
                    // for old versions - just ignore
                }
                else
                {
                    ((ManifestType) Parent.Parent).Title = value;
                }
            }
        }

        public event Action TitleChanged
        {
            add { ((ManifestType) Parent.Parent).TitleChanged += value; }
            remove { ((ManifestType) Parent.Parent).TitleChanged -= value; }
        }

        [Description("Contains relevant information that describes the content package as a whole.")]
        [Category("Main")]
        public MetadataType metadata
        {
            get
            {
                return metadataField;
            }
            set
            {
                metadataField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(DataType = "ID")]
        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [DefaultValue("hierarchical")]
        public string structure
        {
            get
            {
                return structureField;
            }
            set
            {
                structureField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [Category("Main")]
        [XmlElement("sequencing", Namespace = ManifestNamespaces.Imsss)]
        public SequencingType Sequencing
        {
            get
            {
                return sequencingField;
            }
            set
            {
                sequencingField = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        public override string ToString()
        {
            return Title;
        }

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "ItemType":
                    var item = child as ItemType;
                    if (m_SubItems.Contains(item))
                    {
                        m_SubItems.Remove(item);
                        return;
                    }
                    break;

                case "MetadataType":
                    metadata = null;
                    return;
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        [XmlElement("item")]
        [Description("Element is a node that describes the hierarchical structure of the organization")]
        [Category("Main")]
        public ManifestNodeList<ItemType> SubItems
        {
            get
            {
                if (m_SubItems == null)
                {
                    m_SubItems = new ManifestNodeList<ItemType>(this);
                }
                return m_SubItems;
            }
            set
            {
                m_SubItems = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.ChildrenReordered );
            }
        }

        private static IEnumerable<ItemType> enumerateItemRec(IItemContainer item)
        {
            foreach (var i in item.SubItems)
            {
                yield return i;
                foreach (var ri in enumerateItemRec(i))
                {
                    yield return ri;
                } 
            }
        }

        public IEnumerable<ItemType> Items
        {
            get
            {
                foreach (var i in enumerateItemRec(this))
                {
                    yield return i;
                }
            }
        }
    }

    [XmlType("organizations", Namespace = ManifestNamespaces.Imscp)]
    [Description("Contains the content structure or organization of the learning resources making up a stand-alone unit or units of instruction")]
    [Category("Main")]
    public class OrganizationsType : AbstractManifestNode, IOrganizationContainer
    {
        public OrganizationsType()
        {
            organizationField = new ManifestNodeList<OrganizationType>(this);
        }

        private ManifestNodeList<OrganizationType> organizationField;

        private string defaultField;

        [XmlElement("organization")]
        [Description("Element describes a particular hierarchical organization.The content organization can be a lesson, module, course, chapter, etc")]
        [Category("Main")]
        public ManifestNodeList<OrganizationType> Organizations
        {
            get
            {
                if (organizationField == null)
                {
                    organizationField = new ManifestNodeList<OrganizationType>(this);
                }
                return organizationField;
            }
            set
            {
                organizationField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute(DataType = "IDREF")]
        public string @default
        {
            get
            {
                return defaultField;
            }
            set
            {
                defaultField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        #region IContainer Members

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "OrganizationType":
                    {
                        var o = child as OrganizationType;
                        if (Organizations.Contains(o))
                        {
                            Organizations.Remove(o);
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Item '{0}' not found", child);
        }

        #endregion
    }
}
