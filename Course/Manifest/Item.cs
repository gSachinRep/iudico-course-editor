using System;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using FireFly.CourseEditor.Common;
using FireFly.CourseEditor.GUI;
using FireFly.CourseEditor.GUI.HtmlEditor;
using System.Linq;

namespace FireFly.CourseEditor.Course.Manifest
{
    [XmlType("item", Namespace = ManifestNamespaces.Imscp)]
    [Description("Element is a node that describes the hierarchical structure of the organization")]
    [Category("Main")]
    [XmlInclude(typeof(SequencingType))]
    [XmlInclude(typeof(ResourcesType))]
    [XmlInclude(typeof(ResourceType))]
    [XmlInclude(typeof(PageType))]
    [XmlInclude(typeof(LimitConditionsType))]
    public class ItemType : AbstractManifestNode, IItemContainer, ITitled
    {
        private ManifestNodeList<ItemType> itemField;
        private PresentationType presentation;
        private PageType pageType = PageType.Unknown;
        private MetadataType metadataField;
        private SequencingType sequencingField;
        private string identifierField;
        private string _title;
        private string identifierrefField;
        private bool isvisibleField = true;
        private string parametersField;

        public ItemType()
        {
        }

        protected ItemType([NotNull]string title, [NotNull]string identifier, [NotNull]string identifierRef)
        {
            this.Identifier = identifier;
            _title = title;
            IdentifierRef = identifierRef;
        }

        public static ItemType CreateNewItem([NotNull]string title, [NotNull]string identifier, [NotNull]string identifierRef, PageType pageType)
        {
            var result = new ItemType(title, identifier, identifierRef);
            result.pageType = pageType;

            result.Sequencing = SequencingManager.CreateNewSequencing(pageType);

            if (pageType == PageType.Question)
            {
                Course.Answers.Organizations[Course.Organization.identifier].Items.Add(new Item(identifier));
            }

            return result;
        }

        /// <summary>
        /// Gets FULL path to html-file assigned to item.
        /// </summary>
        /// 
        [XmlIgnore]
        [Browsable(false)]
        public string PageHref
        {
            get
            {
                ResourceType t = Course.Manifest.resources[IdentifierRef];
                if (t != null)
                {
                    return t.MapPath(t.href);
                }
                return null;
            }
        }

        public string GetFullPath()
        {
            var res = new StringBuilder(Title);
            var p = Parent;
            while (!(p is OrganizationType) && (p as ITitled) != null)
            {
                res.Insert(0, ((ITitled)p).Title + (res.Length > 0 ? "/" : string.Empty));
                p = p.Parent;
            }
            return res.ToString();
        }

        /// <summary>
        /// Retriews total points of item - sum of all ranks of child test controls.
        /// </summary>
        /// <param name="passRank"></param>
        /// <returns></returns>
        public int GetTotalPoints(out int? passRank)
        {
#if CHECKERS
            if (PageType != PageType.Question)
            {
                throw new InvalidOperationException();
            }
#endif

            var item = Course.Answers.Organizations[Course.Organization.identifier].Items[Identifier];
            passRank = item.PassRank;
            return item.Questions.Sum(q => q.Rank ?? 0);
        }

        [XmlElement("title", Namespace = ManifestNamespaces.Imscp)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (Title != value)
                {
                    if (_title.IsNotNull() && value.IsNull())
                    {
                        ErrorDialog.ShowError("Title cannot be empty");
                    }
                    else
                    {
                        if (_title != null)
                        {
                            Identifier = IdGenerator.GenerateId(value);
                        }
                        _title = value;
                        Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
                    }
                    if (TitleChanged != null)
                    {
                        TitleChanged();
                    }
                }
            }
        }

        public event Action TitleChanged;

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
        
        [Description("Sequencing information is associated with items in a hierarchical structure by associating a single <sequencing> element with the hierarchical item")]
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

        [Description("Element is a container element that encapsulates presentation information for a given learning activity")]
        [Category("Main")]
        [XmlElement("presentation", Namespace = ManifestNamespaces.Adlnav)]
        public PresentationType Presentation
        {
            get
            {
                return presentation;
            }
            set
            {
                presentation = value;
                Course.NotifyManifestChanged(this, new IManifestNode[1] { value }, ManifestChangeTypes.ChildrenAdded);
            }
        }

        [Browsable(false)]
        [XmlAttribute("identifier", DataType = "ID")]
        public string Identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                if (identifierField != value)
                {
                    if (pageType == PageType.Question)
                    {
                        Course.Answers.RenameItem(Identifier, value);
                    }
                    identifierField = value;
                    Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
                }
            }
        }

        [XmlAttribute("identifierref")]
        [TypeConverter(typeof(ManifestStringConverter))]
        public string IdentifierRef
        {
            get
            {
                return identifierrefField;
            }
            set
            {
                identifierrefField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        [DefaultValue(true)]
        public bool isvisible
        {
            get
            {
                return isvisibleField;
            }
            set
            {
                isvisibleField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        [XmlAttribute]
        public string parameters
        {
            get
            {
                return parametersField;
            }
            set
            {
                parametersField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed);
            }
        }

        public override string ToString()
        {
            return Title;
        }

        [XmlAttribute("pagetype")]
        [DefaultValue(PageType.Unknown)]
        public PageType PageType
        {
            get
            {
                return pageType;
            }
            set
            {
                if (pageType != PageType.Unknown && (pageType == PageType.Summary) != (value == PageType.Summary))
                {
                    throw new FireFlyException("Cannot change page type from 'summary' to any other");
                }
                if ((pageType = value) == PageType.Summary)
                {
                    new SummaryPage(this);
                }
            }
        }

        public void RemoveChild(IManifestNode child)
        {
            switch (child.GetType().Name)
            {
                case "ItemType":
                    {
                        var item = child as ItemType;
                        if (SubItems.Contains(item))
                        {
                            SubItems.Remove(item);
                            return;
                        }
                        break;
                    }
                case "SequencingType":
                    {
                        if (Sequencing != null)
                        {
                            Sequencing = null;
                            return;
                        }
                        break;
                    }
                case "PresentationType":
                    {
                        if (presentation != null)
                        {
                            presentation = null;
                            return;
                        }
                        break;
                    }
            }
            throw new FireFlyException("Manifest item '{0}' not found", child);
        }

        [XmlElement("item")]
        [Description("Element is a node that describes the hierarchical structure of the organization")]
        [Category("Main")]
        public ManifestNodeList<ItemType> SubItems
        {
            get
            {
                if (itemField == null)
                {
                    itemField = new ManifestNodeList<ItemType>(this);
                }
                return itemField;
            }
            set
            {
                // IMPORTANT: Don't check are instances the same here. Only checking full colletion is possible. See MoveUp & MoveDown method for understand
                itemField = value;
                Course.NotifyManifestChanged(this, ManifestChangeTypes.Changed & ManifestChangeTypes.ChildrenReordered);
            }
        }

        public override void Dispose()
        {
            if (pageType == PageType.Question)
            {
                Course.Answers.RemoveItem(Identifier);
            }
            
            var r = Course.Manifest.resources[IdentifierRef];
            if (r != null)
            {
                
                r.Dispose();
                Course.Manifest.resources.Resources.Remove(r);
            }
            if (pageType == PageType.Question)
            {
                /* TODO:: 
                 * 
                 * IF course doesn't contain Question, 
                 * THEN delete all scripts and remove ExaminationDependency from manifest 
                 */
            }

            if (pageType == PageType.Chapter || pageType == PageType.Chapter)
            {
                foreach (ItemType item in SubItems)
                {
                    item.Dispose();
                }
                SubItems.Clear();
            }

            base.Dispose();
            
            if (Disposed != null)
            {
                Disposed();
            }
        }

        public event Action Disposed;
    }
}
