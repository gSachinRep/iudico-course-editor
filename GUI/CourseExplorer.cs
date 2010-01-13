using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using Course;
    using WeifenLuo.WinFormsUI.Docking;

    ///<summary>
    /// Window to display and edit content of course as set of files
    ///</summary>
    public partial class CourseExplorer : EditorWindowBase
    {
        private readonly Action<bool>
            _OpenActiveNotifier,
            _RenameActiveNotifier,
            _DeleteActiveNotifier;

        ///<summary>
        /// Creates new instance of <see cref="CourseExplorer"/> class docked in specified dock panel
        ///</summary>
        ///<param name="parentDockPanel">Parent panel that should be used by CourseExplorer to dock on</param>
        public CourseExplorer(DockPanel parentDockPanel)
            : base(parentDockPanel)
        {
            InitializeComponent();

            Course.CourseClosed += () => BuildTree(null);
            Course.CourseOpened += Course_Opened;

            MainForm mf = Forms.Main;
            _OpenActiveNotifier = mf.RegisterToolBoxButton(tvCourse, miOpenAssocProgram);
            _RenameActiveNotifier = mf.RegisterToolBoxButton(tvCourse, miRename);
            _DeleteActiveNotifier = mf.RegisterToolBoxButton(tvCourse, miDelete);

            Show(DockingPanel);
        }

        /// <summary>
        /// Removes directory that is subdirectory of course folder, and throws exception otherwise
        /// </summary>
        /// <param name="path">Directory should be removed</param>
        /// <returns>true if success and false otherwise</returns>
        private static void RemoveCourseSubDirectory(string path)
        {
            if (path.Contains(Course.FullPath))
            {
                Directory.Delete(path, true);
            }
            else
            {
                throw new FireFlyException("\"{0}\" is not sub-folder of \"{1}\"", path, Course.FullPath);
            }
        }

        /// <summary>
        /// Turn node to be directory
        /// </summary>
        /// <param name="node">Node to turning</param>
        private void TurnDirectory(TreeNode node)
        {
            node.SelectedImageIndex = 2;
            node.ImageIndex = 1;
            node.ContextMenuStrip = cmsNode;
        }

        /// <summary>
        /// Turn node to be file
        /// </summary>
        /// <param name="node">Node to turning</param>
        private void TurnFile(TreeNode node)
        {
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
            node.ContextMenuStrip = cmsNode;
        }

        /// <summary>
        /// Builds files and folders tree for Folder and Node recursively
        /// </summary>
        /// <param name="dir">Root folder used to build</param>
        /// <param name="node">Root node</param>
        private void BuildTree(string dir, TreeNode node)
        {
            TurnDirectory(node);

            var ds = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
            foreach (var d in ds)
            {
                TreeNode subNode = node.Nodes.Add(d, Path.GetFileName(d));
                TurnDirectory(subNode);
                BuildTree(d, subNode);
            }

            var files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);

            foreach (var f in files)
            {
                TurnFile(node.Nodes.Add(f, Path.GetFileName(f)));
            }
        }

        /// <summary>
        /// Builds tree of files and folders in <see cref="CourseExplorer"/>
        /// </summary>
        /// <param name="dir">Path to folder that should be used as base</param>                                                    
        private void BuildTree(string dir)
        {
            tvCourse.Nodes.Clear();
            if (dir.IsNotNull())
            {
                BuildTree(dir, tvCourse.Nodes.Add(Course.FullPath, Course.Manifest.Identifier));
            }
        }

        /// <summary>
        /// Determines is node folder
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <returns>Returns true if Node is folder and false otherwise</returns>
        private static bool IsFolder(TreeNode node)
        {
            return node.ImageIndex > 0;
        }

        private void tvCourse_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                tvCourse.SelectedNode = e.Node;
            }
        }

        private void cmsNode_Opening(object sender, CancelEventArgs e)
        {
            var node = tvCourse.SelectedNode;
            if (node != null)
            {
                var isFolder = IsFolder(node);

                miAdd.Visible = isFolder;
                miCreateFolder.Visible = isFolder;
                miOpenAssocProgram.Visible = !isFolder;
                miDelete.Visible = miRename.Visible = (node.Parent != null);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void miCreateFolder_Click(object sender, EventArgs e)
        {
            var node = tvCourse.SelectedNode;

            Debug.Assert(node != null, "Selected node is not defined");
            Debug.Assert(IsFolder(node), "This operation can be performed for folder only");

            string newDirTemplate = Path.Combine(node.Name, "NewFolder");
            string newDirName = newDirTemplate;
            int dirIndex = 0;

            while (Directory.Exists(newDirName))
            {
                dirIndex++;
                newDirName = newDirTemplate + dirIndex;
            }

            Directory.CreateDirectory(newDirName);
        }

        private void tvCourse_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var node = e.Node;
            Debug.Assert(node.Parent != null, "User cannot rename course root directory");
            if (e.Label != null)
            {
                var newPath = Path.Combine(Path.GetDirectoryName(node.Name), e.Label);
                if (IsFolder(node))
                {
                    if (!Directory.Exists(newPath))
                    {
                        Directory.Move(node.Name, newPath);
                    }
                    else
                    {
                        e.CancelEdit = true;
                        ErrorDialog.ShowError("Folder \"{0}\" is already exist.", e.Label);
                    }
                }
                else
                {
                    if (!File.Exists(newPath))
                    {
                        File.Move(node.Name, newPath);
                    }
                    else
                    {
                        e.CancelEdit = true;
                        ErrorDialog.ShowError("File \"{0}\" is already exist.", e.Label);
                    }
                }
            }
            else
            {
                e.CancelEdit = true;
            }
        }

        private void miRename_Click(object sender, EventArgs e)
        {
            var node = tvCourse.SelectedNode;
            Debug.Assert(node != null, "Selected node cannot be null");

            node.BeginEdit();
        }

        private void tvCourse_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                e.CancelEdit = true;
            }
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            var node = tvCourse.SelectedNode;
            Debug.Assert(node != null, "Selected node cannot be null");

            if (IsFolder(node))
            {
                RemoveCourseSubDirectory(node.Name);
            }
            else
            {
                if ((node.Text == Course.MANIFEST_FILE_NAME) && (node.Parent == null))
                {
                    ErrorDialog.ShowError("Manifest cannot be deleted");
                }
                else
                {
                    File.Delete(node.Name);
                }
            }
        }

        private void miAdd_Click(object sender, EventArgs e)
        {
            var node = tvCourse.SelectedNode;

            Debug.Assert(node != null, "Selected node cannot be null");

            if (ofdAddFiles.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var file in ofdAddFiles.FileNames)
                {
                    var shortName = Path.GetFileName(file);
                    File.Copy(file, Path.Combine(node.Name, shortName), false);
                }
            }
        }

        private void miOpenAssocProgram_Click(object sender, EventArgs e)
        {
            TreeNode node = tvCourse.SelectedNode;

            Debug.Assert(node != null, "Selected node cannot be null");
            Debug.Assert(!IsFolder(node), "Selected node must represented a file");

            psExternalProgram.StartInfo.FileName = node.Name;
            psExternalProgram.Start();
        }

        private void fsWatcher_Action(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created && (Directory.Exists(e.FullPath) || File.Exists(e.FullPath)) )
            {
                AddNode(e.FullPath);
            }
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                if (!DeleteNode(e.FullPath))
                    return;
            }
            Course.NotifyChanged();
        }

        private void fsWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            RenameNode(e.OldFullPath, e.FullPath, tvCourse.Nodes);
            Course.NotifyChanged();
        }

        private void Course_Opened()
        {
            BuildTree(fsWatcher.Path = Course.FullPath);
            tvCourse.Nodes[0].Text = Course.Manifest.Identifier;
            Course.Manifest.TitleChanged += () =>
            {
                tvCourse.Nodes[0].Text = Course.Manifest.Identifier;
            };
        }

        private static void RenameNode(string oldFullName, string newFullName, TreeNodeCollection nodes)
        {
            TreeNode n = GetNodeByPath(nodes, oldFullName);
            if (n != null)
            {
                var newFileName = Path.GetFileName(newFullName);
                n.Name = newFullName;
                n.Text = newFileName;
                ChangeAllChildrenTags(n, oldFullName, newFullName);
            }
        }

        [CanBeNull]
        private static TreeNode GetNodeByPath([NotNull]TreeNodeCollection nodes, [NotNull]string fullPath)
        {
            TreeNode[] treeNodes = nodes.Find(fullPath, true);
            if (treeNodes.Length != 1)
            {
                return null;
            }
            return treeNodes[0];
        }

        private static void ChangeAllChildrenTags(TreeNode node, string oldTag, string newTag)
        {
            foreach (TreeNode n in node.Nodes)
            {
                n.Name = n.Name.Replace(oldTag, newTag);
                if (n.Nodes.Count != 0)
                {
                    ChangeAllChildrenTags(n, Path.Combine(oldTag, n.Text), Path.Combine(newTag, n.Text));
                }
            }
        }

        private bool DeleteNode(string fullName)
        {
            TreeNode n = GetNodeByPath(tvCourse.Nodes, fullName);
            if (n != null)
            {
                tvCourse.Nodes.Remove(n);
                Forms.CourseDesigner.DeleteItemByHref(fullName, null);

                return true;
            }

            return false;
        }

        private void AddNode(string fullName)
        {
            var parentNodePath = Directory.GetParent(fullName).FullName;
            var fileName = Path.GetFileName(fullName);

            TreeNode parentNode = GetNodeByPath(tvCourse.Nodes, parentNodePath);
#if CHECKERS
            if (parentNode == null)
            {
                throw new InvalidOperationException("Could not find parent node");
            }
#endif
            TreeNode newNode = parentNode.Nodes.Add(fullName, fileName);
            if (Directory.Exists(fullName))
            {
                TurnDirectory(newNode);
            } 
            else if (File.Exists(fullName))
            {
                TurnFile(newNode);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanRenameOrDelete(TreeNode t)
        {
            string name = t.Name;
            if (name == Course.FullPath)
            {
                return false;
            }
            name = Path.GetFileName(name);
            return name != Course.ANSWERS_FILE_NAME && name != Course.MANIFEST_FILE_NAME;
        }

        private void tvCourse_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool d = CanRenameOrDelete(e.Node);
            _DeleteActiveNotifier(d);
            _RenameActiveNotifier(d);
            _OpenActiveNotifier(!IsFolder(e.Node));
        }
    }
}