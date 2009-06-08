namespace FireFly.CourseEditor.GUI
{
    partial class CourseExplorer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CourseExplorer));
            this.tvCourse = new System.Windows.Forms.TreeView();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.cmsNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenAssocProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.miRename = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdAddFiles = new System.Windows.Forms.OpenFileDialog();
            this.psExternalProgram = new System.Diagnostics.Process();
            this.fsWatcher = new System.IO.FileSystemWatcher();
            this.cmsNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // tvCourse
            // 
            this.tvCourse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCourse.ImageIndex = 0;
            this.tvCourse.ImageList = this.ilIcons;
            this.tvCourse.LabelEdit = true;
            this.tvCourse.Location = new System.Drawing.Point(0, 0);
            this.tvCourse.Name = "tvCourse";
            this.tvCourse.SelectedImageIndex = 0;
            this.tvCourse.Size = new System.Drawing.Size(253, 271);
            this.tvCourse.TabIndex = 0;
            this.tvCourse.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCourse_AfterLabelEdit);
            this.tvCourse.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCourse_AfterSelect);
            this.tvCourse.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvCourse_NodeMouseClick);
            this.tvCourse.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCourse_BeforeLabelEdit);
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.ilIcons.Images.SetKeyName(0, "new_document16.bmp");
            this.ilIcons.Images.SetKeyName(1, "folder_closed16_h.bmp");
            this.ilIcons.Images.SetKeyName(2, "folder_open16_h.bmp");
            // 
            // cmsNode
            // 
            this.cmsNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDelete,
            this.miAdd,
            this.miOpenAssocProgram,
            this.miCreateFolder,
            this.miRename});
            this.cmsNode.Name = "cmsNode";
            this.cmsNode.Size = new System.Drawing.Size(217, 136);
            this.cmsNode.Opening += new System.ComponentModel.CancelEventHandler(this.cmsNode_Opening);
            // 
            // miDelete
            // 
            this.miDelete.Image = global::FireFly.CourseEditor.Properties.Resources.delete_16;
            this.miDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miDelete.Name = "miDelete";
            this.miDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miDelete.Size = new System.Drawing.Size(216, 22);
            this.miDelete.Text = "&Delete";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // miAdd
            // 
            this.miAdd.Image = global::FireFly.CourseEditor.Properties.Resources.add_to_list16;
            this.miAdd.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miAdd.Name = "miAdd";
            this.miAdd.Size = new System.Drawing.Size(216, 22);
            this.miAdd.Text = "&Add...";
            this.miAdd.Click += new System.EventHandler(this.miAdd_Click);
            // 
            // miOpenAssocProgram
            // 
            this.miOpenAssocProgram.Image = global::FireFly.CourseEditor.Properties.Resources.right_green24_h;
            this.miOpenAssocProgram.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miOpenAssocProgram.Name = "miOpenAssocProgram";
            this.miOpenAssocProgram.Size = new System.Drawing.Size(216, 22);
            this.miOpenAssocProgram.Text = "&Open (assosiated program)";
            this.miOpenAssocProgram.Click += new System.EventHandler(this.miOpenAssocProgram_Click);
            // 
            // miCreateFolder
            // 
            this.miCreateFolder.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miCreateFolder.Name = "miCreateFolder";
            this.miCreateFolder.Size = new System.Drawing.Size(216, 22);
            this.miCreateFolder.Text = "&New Folder";
            this.miCreateFolder.Click += new System.EventHandler(this.miCreateFolder_Click);
            // 
            // miRename
            // 
            this.miRename.Image = global::FireFly.CourseEditor.Properties.Resources.Rename;
            this.miRename.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.miRename.Name = "miRename";
            this.miRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.miRename.Size = new System.Drawing.Size(216, 22);
            this.miRename.Text = "&Rename";
            this.miRename.Click += new System.EventHandler(this.miRename_Click);
            // 
            // ofdAddFiles
            // 
            this.ofdAddFiles.Multiselect = true;
            this.ofdAddFiles.RestoreDirectory = true;
            this.ofdAddFiles.ShowReadOnly = true;
            this.ofdAddFiles.SupportMultiDottedExtensions = true;
            // 
            // psExternalProgram
            // 
            this.psExternalProgram.StartInfo.Domain = "";
            this.psExternalProgram.StartInfo.LoadUserProfile = false;
            this.psExternalProgram.StartInfo.Password = null;
            this.psExternalProgram.StartInfo.StandardErrorEncoding = null;
            this.psExternalProgram.StartInfo.StandardOutputEncoding = null;
            this.psExternalProgram.StartInfo.UserName = "";
            this.psExternalProgram.SynchronizingObject = this;
            // 
            // fsWatcher
            // 
            this.fsWatcher.EnableRaisingEvents = true;
            this.fsWatcher.IncludeSubdirectories = true;
            this.fsWatcher.NotifyFilter = ((System.IO.NotifyFilters)((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName)));
            this.fsWatcher.SynchronizingObject = this;
            this.fsWatcher.Renamed += new System.IO.RenamedEventHandler(this.fsWatcher_Renamed);
            this.fsWatcher.Deleted += new System.IO.FileSystemEventHandler(this.fsWatcher_Action);
            this.fsWatcher.Created += new System.IO.FileSystemEventHandler(this.fsWatcher_Action);
            // 
            // CourseExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 271);
            this.Controls.Add(this.tvCourse);
            this.Name = "CourseExplorer";
            this.TabText = "Course Explorer";
            this.Text = "Course Explorer";
            this.cmsNode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fsWatcher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvCourse;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.ContextMenuStrip cmsNode;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripMenuItem miAdd;
        private System.Windows.Forms.ToolStripMenuItem miOpenAssocProgram;
        private System.Windows.Forms.ToolStripMenuItem miCreateFolder;
        private System.Windows.Forms.ToolStripMenuItem miRename;
        private System.Windows.Forms.OpenFileDialog ofdAddFiles;
        private System.Diagnostics.Process psExternalProgram;
        private System.IO.FileSystemWatcher fsWatcher;

    }
}