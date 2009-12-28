using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FireFly.CourseEditor.GUI
{
    using Course;
    using Common;
    using Properties;

    ///<summary>
    /// Class of Course Editor's main form
    ///</summary>
    public partial class MainForm : Form
    {
        ///<summary>
        /// Creates new instance of MainForm
        ///</summary>
        public MainForm()
        {
            InitializeComponent();

            Course.CourseOpened += Course_Opened;
            Course.CourseClosed += Course_Closed;
            Course.CourseChanged += () =>
            {
                saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = true;
                
            };
            Course.CourseClosing += Course_Closing;
            Settings.Default.PropertyChanged += Settings_Changed;
            _StopPreviewInvoker = StopCoursePreview;
            miShowToolBar.Checked = Settings.Default.Options_ShowToolBar;
        }

        ///<summary>
        ///  Registers new Tool Box button in main tool-set
        ///</summary>
        ///<param name="ownControl">Control owns button will be created</param>
        ///<param name="item">Instance of <see cref="ToolStripMenuItem"/> which provides information to create button</param>
        ///<param name="action">Action should be call when created button clicked</param>
        ///<returns>Action delegate to notify main form when created button should be enabled/disabled</returns>
        ///<exception cref="FireFlyException"></exception>
        public Action<bool> RegisterToolBoxButton([NotNull]Control ownControl, [NotNull]ToolStripMenuItem item, [CanBeNull]EventHandler action)
        {
            var nb = new ToolStripButton(item.Text, item.Image) { Name = item.Name, ToolTipText = item.ToolTipText, Tag = ownControl, Visible = false};
            if (item.ShortcutKeys != Keys.None)
            {
#if CHECKERS
                if (nb.ToolTipText.IsNull())
                {
                    throw new FireFlyException("{0} has empty ToolTipText property", nb.Name);
                }
#endif
                nb.ToolTipText += " (" + new KeysConverter().ConvertToString(item.ShortcutKeys) + ")";
            }
            nb.DisplayStyle = item.Image != null ? ToolStripItemDisplayStyle.Image : ToolStripItemDisplayStyle.Text;
            nb.Click += action ?? ((s, e) => item.PerformClick());
            nb.Enabled = item.Visible && item.Enabled;
            tsMain.Items.Add(nb);

            ownControl.GotFocus += (s, e) => nb.Visible = true;
            ownControl.LostFocus += (s, e) =>
            {
                nb.Visible = false;
            };
            ownControl.Disposed += (s, e) => nb.Dispose();

            return isActive =>
            {
                nb.Enabled = isActive;
                item.Visible = isActive;
            };
        }

        ///<summary>
        /// Registers new Tool Box button in main tool-set
        ///</summary>
        ///<param name="ownControl">Control owns button will be created</param>
        ///<param name="item">Instance of <see cref="ToolStripMenuItem"/> which provides information to create button</param>
        ///<returns>Action delegate to notify main form when created button should be enabled/disabled</returns>
        public Action<bool> RegisterToolBoxButton([NotNull]Control ownControl, [NotNull]ToolStripMenuItem item)
        {
            return RegisterToolBoxButton(ownControl, item, null);
        }

        ///<summary>
        /// Unregister all buttons which owned by <paramref name="ownControl"/> 
        ///</summary>
        ///<param name="ownControl">Control-owner for buttons should be removed</param>
        public void UnRegisterToolBoxItems([NotNull]Control ownControl)
        {
            var listToDelete = new List<ToolStripButton>();
            ToolStripItemCollection items = tsMain.Items;
            foreach (object item in items)
            {
                var b = item as ToolStripButton;
                if (b != null)
                {
                    if (b.Tag == ownControl)
                    {
                        listToDelete.Add(b);
                    }
                }
            }
            foreach (var b in listToDelete)
            {
                items.Remove(b);
            }
        }

        private bool SaveCourse(bool saveNew)
        {
            if ((_CourseFileName == null || saveNew))
            {
                sfdCourse.FileName = Course.Manifest.Title;
                if (sfdCourse.ShowDialog(this) == DialogResult.OK)
                {
                    _CourseFileName = sfdCourse.FileName;
                }
            }
            if (_CourseFileName != null)
            {
                if (Course.SaveToZipPackage(_CourseFileName) == true)
                {
                    if (saveNew)
                    {
                        Course.Manifest.Identifier = Path.GetFileNameWithoutExtension(_CourseFileName);
                    }
                    
                    saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = false;
                }
            }
            return _CourseFileName != null;
        }

        private void DoOpenCoursePackage(object sender, EventArgs e)
        {
            if (ofdCourse.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    OpenCourse(ofdCourse.FileName, true);
                }
                catch (IOException)
                {
                    ErrorDialog.ShowError(string.Format("File '{0}' not FireFly course", ofdCourse.FileName));
                }
            }
        }

        private void DoSaveCoursePackage(object sender, EventArgs e)
        {
            SaveCourse(sender == saveAsToolStripMenuItem);
        }

        private void DoCreateNewCourse(object sender, EventArgs e)
        {
            Course.CreateNew();
            _CourseFileName = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadDockingSettings();
            Activate();
            Manifest_TitleChanged();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
#if PRECOMPILATION
            throw new InvalidOperationException();
#else
            SplashForm.StepDone();
            ReLoadLastCourses(null);
            SplashForm.HideSplash();
#endif
        }

        private void ReLoadLastCourses(string baseCourse)
        {
            miLastCourses.DropDownItems.Clear();
            var lc = Settings.Default.LastCoursesXml;
            int lastInd = lc.Count - 1;
            if (baseCourse == null && lastInd >= 0 && File.Exists(lc[lastInd]))
            {
                OpenCourse(lc[lastInd], false);
            }
            int accessKey = 1;
            for (int i = lastInd; i >= 0; i--)
            {
                string last = lc[i];
                if (last != baseCourse)
                {
                    ToolStripItem item = new ToolStripMenuItem("&" + accessKey++ + "    " + last)
                     {
                         DisplayStyle = ToolStripItemDisplayStyle.Text,
                         Tag = last
                     };
                    item.Click += miReopen_Click;
                    miLastCourses.DropDownItems.Add(item);
                }
            }
            miLastCourses.Enabled = miLastCourses.DropDownItems.Count > 0;
        }

        private void LoadDockingSettings()
        {
            ConfigHelper.RestoreWindowSettings(this);

            var dockLayout = Settings.Default.DockingLayout;

            if (dockLayout.IsNotNull())
            {
                toolStripContainer1.ContentPanel.Controls.Remove(DockingPanel);
                DockingPanel = new DockPanel
                   {
                       DocumentStyle = DocumentStyle.DockingWindow,
                       ActiveAutoHideContent = null,
                       Dock = DockStyle.Fill,
                       Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.World),
                       Location = Point.Empty,
                       Name = "DockingPanel",
                       TabStop = false
                   };
                toolStripContainer1.ContentPanel.Controls.Add(DockingPanel);

                var m = new MemoryStream(dockLayout.Length);
                m.Write(Encoding.ASCII.GetBytes(dockLayout), 0, dockLayout.Length);
                m.Position = 0;
                DockingPanel.LoadFromXml(m, GetContentFromPersistString, true);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (Settings.Default.Options_SaveRestoreWindowsState)
            {
                SaveDockingSettings();
            }
            Settings.Default.Save();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if ((Course.State & CourseStates.Opened) > 0 && !Course.Close())
            {
                e.Cancel = true;
            }
        }

        private void SaveDockingSettings()
        {
            ConfigHelper.SaveWindowSettings(this);

            var ms = new MemoryStream();
            DockingPanel.SaveAsXml(ms, Encoding.ASCII);
            Settings.Default.DockingLayout = Encoding.ASCII.GetString(ms.ToArray());
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog();
        }

        private static DockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PropertyEditor).FullName)
            {
                return Forms.PropertyEditor;
            }
            if (persistString == typeof(ManifestBrowser).FullName)
            {
                return Forms.ManifestBrowser;
            }
            if (persistString == typeof(CourseDesigner).FullName)
            {
                return Forms.CourseDesigner;
            }
            if (persistString == typeof(CourseExplorer).FullName)
            {
                return Forms.CourseExplorer;
            }
            throw new NotImplementedException();
        }

        #region Event handlers for Course events

        private void Course_Closing(CancelEventArgs e)
        {
            if ((Course.State & CourseStates.Modified) > 0)
            {
                switch (MessageBox.Show(this, "Do you want to save Course?", "Course was changed.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        e.Cancel = !SaveCourse(false);
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void Course_Opened()
        {
            saveToolStripMenuItem.Enabled = saveAsToolStripMenuItem.Enabled = saveToolStripButton.Enabled = false;
            Course.Manifest.TitleChanged += Manifest_TitleChanged;
            Manifest_TitleChanged();

            startPreviewToolStripButton.Enabled = true;
            stopPreviewToolStripButton.Enabled = false;
            closeToolStripMenuItem.Enabled = true;
        }

        private void Course_Closed()
        {
            stopPreviewToolStripButton.Enabled = startPreviewToolStripButton.Enabled = saveAsToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = saveToolStripButton.Enabled = false;
            Manifest_TitleChanged();

            closeToolStripMenuItem.Enabled = false;
        }

        private void Manifest_TitleChanged()
        {
            Text = string.Format(Settings.Default.MainFormCaptionFormat, (Course.Manifest == null ? "" : Course.Manifest.Identifier));
        }

        #endregion

        #region Event handlers for Menu actions

        private void miView_DropDownOpening(object sender, EventArgs e)
        {
            miShowPropertyEditor.Checked = Forms.PropertyEditor.DockState != DockState.Hidden;
            miShowManifestBrowser.Checked = Forms.ManifestBrowser.DockState != DockState.Hidden;
            miShowCourseExplorer.Checked = Forms.CourseExplorer.DockState != DockState.Hidden;
            miShowCourseDesigner.Checked = Forms.CourseDesigner.DockState != DockState.Hidden;
        }

        private void miShowPropertyEditor_Click(object sender, EventArgs e)
        {
            if (miShowPropertyEditor.Checked)
            {
                Forms.PropertyEditor.DockState = Forms.PropertyEditor.PreviousDockState;
            }
            else
            {
                Forms.PropertyEditor.Close();
            }
        }

        private void miShowManifestBrowser_Click(object sender, EventArgs e)
        {
            if (miShowManifestBrowser.Checked)
            {
                Forms.ManifestBrowser.DockState = Forms.ManifestBrowser.PreviousDockState;
            }
            else
            {
                Forms.ManifestBrowser.Close();
            }
        }

        private void miShowCourseExplorer_Click(object sender, EventArgs e)
        {
            if (miShowCourseExplorer.Checked)
            {
                Forms.CourseExplorer.DockState = Forms.CourseExplorer.PreviousDockState;
            }
            else
            {
                Forms.CourseExplorer.Close();
            }
        }

        private void miShowCourseDesigner_Click(object sender, EventArgs e)
        {
            if (miShowCourseDesigner.Checked)
            {
                Forms.CourseDesigner.DockState = Forms.CourseDesigner.PreviousDockState;
            }
            else
            {
                Forms.CourseDesigner.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        ///<summary>
        /// Open specified course file
        ///</summary>
        ///<param name="fileName">Full path to course should be opened</param>
        ///<param name="reloadLast">Must or not list of last opened courses be reloaded</param>
        public void OpenCourse([NotNull]string fileName, bool reloadLast)
        {
            if (Course.OpenZipPackage(_CourseFileName = fileName) && reloadLast)
            {
                ReLoadLastCourses(fileName);
            }
            saveToolStripButton.Enabled = saveToolStripMenuItem.Enabled = false;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Course.Close();
        }

        private void miReopen_Click(object sender, EventArgs e)
        {
            OpenCourse((string)((ToolStripMenuItem)sender).Tag, true);
        }

        #endregion

        private void StartCoursePreview()
        {
            if ((Course.State & CourseStates.Opened) == 0)
            {
                ErrorDialog.ShowError("No course for preview. Please open an existing course or create new one.");
            }
            if (_PreviewProcess != null)
            {
                throw new FireFlyException("Course preview already started!");
            }

            Course.Save();

            if (_Provider == null)
            {
                _Provider = new HttpCourseProvider { PlayerLocation = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "player") };
            }
            if (!Directory.Exists(_Provider.PlayerLocation))
            {
                ErrorDialog.ShowError("Could not start course preview because player not found.");
                return;
            }
            _Provider.Start();
            _PreviewProcess = Process.Start("IExplore.exe", "http://localhost:" + HttpCourseProvider.Port + "/index.html");

            if (_PreviewProcess == null)
            {
                _Provider.Stop();
                ErrorDialog.ShowError("Cant start process for browser.");
                return;
            }
            _PreviewProcess.Exited += ((sender, e) =>
            {
                if (_PreviewProcess != null)
                {
                    Invoke(_StopPreviewInvoker);
                }
            });
            _PreviewProcess.EnableRaisingEvents = true;

            startPreviewToolStripButton.Enabled = false;
            stopPreviewToolStripButton.Enabled = true;
            Course.CourseClosed += StopCoursePreview;
        }

        private void StopCoursePreview()
        {
            Course.CourseClosed -= StopCoursePreview;
            if (_PreviewProcess != null && !_PreviewProcess.HasExited)
            {
                _PreviewProcess.Kill();
                _PreviewProcess.Dispose();
            }
            _PreviewProcess = null;
            _Provider.Stop();

            startPreviewToolStripButton.Enabled = true;
            stopPreviewToolStripButton.Enabled = false;
        }

        private void startPreviewToolStripButton1_Click(object sender, EventArgs e)
        {
            StartCoursePreview();
        }

        private void stopPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            StopCoursePreview();
        }

        private void Settings_Changed(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Options_ShowToolBar":
                    miShowToolBar.Checked = tsMain.Visible = Settings.Default.Options_ShowToolBar;
                    break;
            }
        }

        private string _CourseFileName;
        private readonly Action _StopPreviewInvoker;
        private Process _PreviewProcess;
        private HttpCourseProvider _Provider;

        private void miShowToolBar_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Options_ShowToolBar = miShowToolBar.Checked;
        }
    }
}