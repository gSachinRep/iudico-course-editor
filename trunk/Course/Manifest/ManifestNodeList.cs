namespace FireFly.CourseEditor.Course.Manifest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    /// <summary>
    /// This class should be used to create collection contain any manifest node types.
    /// It has override methods for all methods that can modify collection and notify Course when it
    /// happened
    /// </summary>
    public class ManifestNodeList<T> : List<T>, IManifestNodeList<T> 
        where T : class, IManifestNode
    {
        public ManifestNodeList()
        {
        }

        public ManifestNodeList([NotNull]IManifestNode parent)
        {
            Parent = parent;
        }

        public new void Add([NotNull]T item)
        {
            base.Add(item);
            item.Parent = Parent;

            Course.NotifyManifestChanged(Parent, new[] { item }, ManifestChangeTypes.ChildrenAdded);
        }

        public new bool Remove([NotNull]T item)
        {
            var res = base.Remove(item);
            if (res)
            {
                Course.NotifyManifestChanged(Parent, new [] { item }, ManifestChangeTypes.ChildrenRemoved);
            }
            return res;
        }

        public new void RemoveAt(int index)
        {
            IManifestNode remItem = this[index];
            base.RemoveAt(index);
            Course.NotifyManifestChanged(Parent, new [] { remItem }, ManifestChangeTypes.ChildrenRemoved);
        }

        public new void RemoveRange(int index, int count)
        {
            // collect removed item for event
            var remItems = new IManifestNode[count];
            for (var i = index; i < index + count; i++)
            {
                remItems[i - index] = this[i];
            }
            base.RemoveRange(index, count);
            Course.NotifyManifestChanged(Parent, remItems, ManifestChangeTypes.ChildrenRemoved);
        }

        [NotNull]
        public IManifestNode Parent
        {
            get
            {
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

        public void ResolveTree(IManifestNode parent)
        {
            Parent = parent;
            for (var i = Count - 1; i >= 0; i--)
            {
                this[i].ResolveTree(this);
            }
        }

        /// <summary>
        /// Determines the unique id that specified this node
        /// </summary>
        [XmlIgnore]
#if DEBUG
        [Category("~Debug")]
#else
        [Browsable(false)]
#endif
        public string UID
        {
            get
            {
                return _UID;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IManifestNode _Parent;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _UID = AbstractManifestNode.GetNextUID();
    }
}
