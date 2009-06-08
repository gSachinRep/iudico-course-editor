using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Course;
    using Course.Manifest;
    using System.Diagnostics;

    ///<summary>
    /// Customized <see cref="TreeView"/> component to display <see cref="IManifestNode"/> elements binded to nodes.
    ///</summary>
    public class FFTreeView : TreeView
    {
        ///<summary>
        ///  Creates new instance of FFTreeView class
        ///</summary>
        public FFTreeView()
        {
            Course.ManifestChanged += Manifest_Changed;
            Course.CourseClosed += Nodes.Clear;
            Course.CourseOpened += () =>
            {
                Nodes.Clear();
                BuildManifestNode(Course.Manifest, null, 2);
                OnManifestTreeBuilt();
            };
        }

        /// <summary>
        /// This menu strip will be assigned to all tree's nodes
        /// </summary>
        [DefaultValue(null)]
        [Description("This menu strip will be assigned to all tree's nodes")]
        public ContextMenuStrip NodeContextMenuStrip { get; set; }

        protected virtual NodeAddingArgs.ActionKind OnBeforeManifestNodeAdded(IManifestNode node)
        {
            var e = new NodeAddingArgs(node);
            if (ManifestNodeAdding != null)
            {
                ManifestNodeAdding(e);
            }
            return e.Action;
        }

        protected virtual void OnManifestTreeBuilt()
        {
            if (ManifestTreeBuilt != null)
            {
                ManifestTreeBuilt();
            }
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            foreach (TreeNode node in e.Node.Nodes)
            {
                if (node.Nodes.Count == 0)
                {
                    BuildChildrens(node.Tag as IManifestNode, node, 2);
                }
            }
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);
            if (SelectedNode == e.Node)
            {
                _Counter++;
            }
            else
            {
                SelectedNode = e.Node;
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            _Counter = 1;
        }

        private int _Counter; // To fix #1986050 "Should not rename by single click"

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _Counter = 0;
        }

        protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
        {
            base.OnBeforeLabelEdit(e);
            Debug.WriteLine("BEFORE EDIT " + _Counter);
            e.CancelEdit = _Counter < 2;

        }

        private void BuildManifestNode(IManifestNode node, TreeNode parentNode, int maxDeapth)
        {
            if (node != null && maxDeapth > 0)
            {
                var pNode = parentNode;
                var nc = parentNode == null ? Nodes : parentNode.Nodes;
                switch (OnBeforeManifestNodeAdded(node))
                {
                    case NodeAddingArgs.ActionKind.None:
                        pNode = nc.Add(node.UID, node.ToString());
                        if (ManifestNodeAdded != null)
                        {
                            ManifestNodeAdded(new NodeAddedArgs(node, pNode));
                        }
                        break;

                    case NodeAddingArgs.ActionKind.Ignore:
                        break;

                    case NodeAddingArgs.ActionKind.Cancel:
                        return;

                    default:
                        throw new NotImplementedException();
                }

                if (pNode != parentNode)
                {
                    pNode.ContextMenuStrip = NodeContextMenuStrip;
                    pNode.Tag = node;
                    pNode.SelectedImageKey = pNode.ImageKey = GetImageKey(node);
                }
                else
                {
                    maxDeapth++;
                }
                BuildChildrens(node, pNode, maxDeapth);
            }
        }

        ///<summary>
        /// Retrieves image key for the node
        ///</summary>
        ///<param name="node">Node of manifest we need image for</param>
        ///<returns>Returns image key for the specified node</returns>
        public static string GetImageKey(IManifestNode node)
        {
            var item = node as ItemType;
            return item != null ? item.PageType.ToString() : node.GetType().Name;
        }

        private void BuildChildrens(IManifestNode node, TreeNode treeNode, int maxDeapth)
        {
            if (!(node is ResourceType))
            {
                var props = node.GetType().GetProperties();
                foreach (var pi in props)
                {
                    if (pi.PropertyType.Name.Contains("ManifestNodeList"))
                    {
                        var list = pi.GetValue(node, null) as IEnumerable;
                        if (list != null)
                        {
                            foreach (IManifestNode n in list)
                            {
                                BuildManifestNode(n, treeNode, maxDeapth - 1);
                            }
                        }
                    }
                    else if (pi.PropertyType.GetInterface("IManifestNode", true) != null)
                    {
                        if (pi.Name != "Parent" && (treeNode != null ? treeNode.Text : string.Empty) != "Resources")
                        {
                            BuildManifestNode(pi.GetValue(node, null) as IManifestNode, treeNode, maxDeapth - 1);
                        }
                    }
                }
            }
        }

        ///<summary>
        /// Raises before adding manifest node to tree
        ///</summary>
        [Description("This event raises before adding manifest node to tree")]
        [Category("Manifest")]
        public event Action<NodeAddingArgs> ManifestNodeAdding;

        ///<summary>
        /// Raises after manifest node has been added to the tree
        ///</summary>
        [Description("Raises after manifest node has been added to the tree")]
        [Category("Manifest")]
        public event Action<NodeAddedArgs> ManifestNodeAdded;

        ///<summary>
        /// Raises immediately after tree of manifest has been built
        ///</summary>
        [Category("Manifest")]
        [Description("Raises immediately after tree of manifest has been built")]
        public event Action ManifestTreeBuilt;

        public void Manifest_Changed(ManifestChangedEventArgs e)
        {
            var nodes = Nodes.Find(e.ChangedNode.UID, true);
            switch (nodes.Length)
            {
                case 0:
                    break;

                case 1:
                    var treeNode = nodes[0];
                    switch (e.ChangeType)
                    {
                        case ManifestChangeTypes.Changed:
                            treeNode.Text = e.ChangedNode.ToString();
                            break;

                        case ManifestChangeTypes.ChildrenRemoved:
                            nodes = Nodes.Find(e.ChangedNode.UID, true);
                            if (nodes.Length == 1)
                            {
                                var pNode = nodes[0];
                                foreach (var n in e.Nodes)
                                {
                                    nodes = pNode.Nodes.Find(n.UID, false);
                                    if (nodes.Length == 1)
                                    {
                                        nodes[0].Remove();
                                    }
                                }
                            }
                            break;

                        default:
                            treeNode.Nodes.Clear();
                            BuildChildrens(e.ChangedNode, treeNode, 3);
                            break;

                    }
                    break;

                default:
                    throw new FireFlyException("Two or more tree node with key '{0}' found (Node: {0})", e.ChangedNode.UID, e.ChangedNode);
            }
        }

        #region Nested types

        public abstract class NodeAddedArgsBase : EventArgs
        {
            public IManifestNode Node { get; private set; }

            protected NodeAddedArgsBase(IManifestNode node)
            {
                Node = node;
            }
        }

        public class NodeAddedArgs : NodeAddedArgsBase
        {
            public TreeNode TreeNode { get; private set; }

            public NodeAddedArgs(IManifestNode node, TreeNode treeNode): base(node)
            {
                TreeNode = treeNode;
            }
        }

        public class NodeAddingArgs : NodeAddedArgsBase
        {
            #region ActionKind enum

            public enum ActionKind
            {
                None,
                Ignore,
                Cancel
            }

            #endregion

            public ActionKind Action;

            public NodeAddingArgs(IManifestNode Node)
                : base(Node)
            {
            }
        }

        #endregion
    }
}