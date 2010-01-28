using System.Collections.Generic;
using System.Text;
using FireFly.CourseEditor.Course.Manifest;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using HighlightControl;
    using AdvancedCompiledTestControl;

    ///<summary>
    /// Html Editor
    ///</summary>
    public partial class PageEditor : UserControl
    {
        ///<summary>
        /// Creates new instance of <see cref="PageEditor"/>
        ///</summary>
        public PageEditor()
        {
            InitializeComponent();

            btnButton.Tag = typeof(HtmlButton);
            btnLabel.Tag = typeof(HtmlLabel);
            btnTextEdit.Tag = typeof(HtmlTextBox);
            btnComboBox.Tag = typeof(HtmlComboBox);
            btnSimpleQuestion.Tag = typeof(HtmlSimpleQuestion);
            btnHighlightedText.Tag = typeof(HtmlHighlightedCode);
            btnCodeSnippet.Tag = typeof(HtmlCodeSnippet);
            btnCompiledTest.Tag = typeof(HtmlCompiledTest);
            btnAdvancedCompiledTest.Tag = typeof(HtmlAdvancedCompiledTest);

            miProperties.Click += (sender, e) => Forms.PropertyEditor.Show();

            EventHandler toolButtonClick = (sender, e) => { CheckedButton = sender as ToolStripButton; };

            foreach (ToolStripItem b in tsComponents.Items)
            {
                if (b.Tag is Type)
                {
                    b.Click += toolButtonClick;
                }
            }
        }

        ///<summary>
        /// Try to parse text as html code and return result
        ///</summary>
        ///<param name="text">Text to parse</param>
        ///<param name="item">Item to assign</param>
        ///<returns></returns>
        public bool TryParsePageText(string text, ItemType item)
        {
            var p = CreateHtmlPage();
            try
            {
                p.SetPageItem(item);
                p.ParseStream(new StringReader(text), ControlControlParsed);
                HtmlPage = p;
                return true;
            }
            catch
            {
                p.Dispose();
                return false;
            }
        }

        ///<summary>
        /// Retrieves scope to display in <see cref="PropertyEditor"/>
        ///</summary>
        ///<returns></returns>
        [CanBeNull]
        public List<ITitled> GetScope()
        {
            if (HtmlPage == null)
            {
                return null;
            }
            return new List<HtmlControl>(HtmlPage.HtmlControls) { HtmlPage }.ConvertAll<ITitled>(t => t);
        }

        ///<summary>
        /// Page is editing now
        ///</summary>
        [CanBeNull]
        public HtmlPage HtmlPage
        {
            get { return _htmlPage; }
            private set
            {
                _htmlPage = value;
                if (PageChanged != null)
                {
                    PageChanged(value);
                }
            }
        }

        ///<summary>
        /// Raises when instance of <see cref="HtmlPage"/> created by editor
        ///</summary>
        public event Action<HtmlPage> PageCreated;

        ///<summary>
        /// Occurs when page assigned to editor is changed
        ///</summary>
        public event Action<HtmlPage> PageChanged;

        ///<summary>
        /// Assign item to current page
        ///</summary>
        ///<param name="node"></param>
        public void SetPageItem([NotNull]ItemType node)
        {
            SuspendLayout();
            try
            {
                if (HtmlPage != null)
                {
                    HtmlPage.Parent = null;
                }
                HtmlPage p = HtmlPage.GetPage(node);
                if (p == null)
                {
                    p = HtmlPage = CreateHtmlPage();
                    p.SetPageItem(node);
                    p.ParseHtmlFile(node.PageHref, ControlControlParsed);
                }
                else
                {
                    HtmlPage = p;
                }
                HtmlPage.Parent = this;
                UpdateUndoRedoState();
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        #region Selecton Utils

        private void SelectControl(HtmlDesignMovableControl c)
        {
#if CHECKERS
            if (IsControlSelected(c))
            {
                throw new FireFlyException("{0} is already selected", c.Title);
            }
#endif
            _selectionList.Add(new BoundControl(c));
            Control winControl = c.Control;
            winControl.KeyUp += ControlKeyUp;
            c.BeginMove += Control_BeginMove;
            c.EndMove += Control_EndMove;
            c.BeginResize += Control_BeginResize;
            c.EndResize += Control_EndResize;
            Action d = () =>
            {
                if (IsControlSelected(c))
                {
                    UnSelectControl(c);
                }
            };
            c.Deleting += d;
            c.Disposed += d;
            if (c is HtmlCodeSnippet)
            {
                Forms.Main.RegisterToolBoxButton(winControl, miEditInMSWord)(true);
            }
            Forms.Main.RegisterToolBoxButton(winControl, miProperties)(true);
            Forms.Main.RegisterToolBoxButton(winControl, miDelete)(true);
            Debug.WriteLine("PageEditor: '" + c.Title + "' - Selected");
            SelectionChanged();
        }
        private void FreeResources(HtmlDesignMovableControl c)
        {
            if ((c as HtmlCodeSnippet) != null)
            {
                (c.Control as CodeSnippet).DeleteResources();
            }
        }

        private void UnSelectControl(HtmlDesignMovableControl c)
        {
            UnSelectControlInternal(c);
            SelectionChanged();
            c.Control.Focus();
        }

        private void UnSelectControlInternal(HtmlDesignMovableControl c)
        {
            int index = _selectionList.FindIndex(bc => bc.Owner.Equals(c));
#if CHECKERS
            if (index < 0)
            {
                throw new FireFlyException("{0} is not selected", c.Title);
            }
#endif
            Control winControl = c.Control;
            winControl.KeyUp -= ControlKeyUp;
            c.BeginMove -= Control_BeginMove;
            c.EndMove -= Control_EndMove;
            c.BeginResize -= Control_BeginResize;
            c.EndResize -= Control_EndResize;
            Forms.Main.UnRegisterToolBoxItems(winControl);
            _selectionList[index].Dispose();
            _selectionList.RemoveAt(index);
            Debug.WriteLine("PageEditor: '" + c.Title + "' - DeSelected");
        }

        private bool IsSelectionEmpty
        {
            get { return _selectionList.Count == 0; }
        }

        private bool IsControlSelected(HtmlControl control)
        {
            return _selectionList.Exists(c => c.Owner == control);
        }

        private void UnSelectAll()
        {
            UnSelectAllInternal();
            SelectionChanged();
        }

        private void UnSelectAllInternal()
        {
            foreach (var c in _selectionList.ToArray())
            {
                UnSelectControl(c.Owner);
            }
        }

        private void SelectionChanged()
        {
            if (IsSelectionEmpty)
            {
                Forms.PropertyEditor.SetContext(null, null, null);
            }
            else
            {
                Forms.PropertyEditor.SetContext(_selectionList.ConvertAll(b => b.Owner).ToArray(), GetScope(), PropertyEditorSelectedObjectChanged)
                    .ValueChanging += ControlProperty_ValueChanging;
            }
        }

        internal void PropertyEditorSelectedObjectChanged(object newSelection, FFTypeDescriptor typeDescriptor)
        {
            UnSelectAll();
            var page = newSelection as HtmlPage;
            if (page == null)
            {
                SelectControl((HtmlDesignMovableControl)newSelection);
            }
            typeDescriptor.ValueChanging += ControlProperty_ValueChanging;
        }

        #endregion

        [NotNull]
        private HtmlPage CreateHtmlPage()
        {
            var p = new HtmlPage { Parent = this };
            var c = p.Control;
            var top = tsComponents.Top + tsComponents.Height + 1;
            c.Top = top;
            c.Width = ClientSize.Width;
            c.Height = ClientSize.Height - top;
            c.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            c.Click += PanelClick;
            if (PageCreated != null)
            {
                PageCreated(p);
            }
            return p;
        }

        [ReadOnly(true)] // To avoid design-time errors
        [Browsable(false)]
        [CanBeNull]
        private ToolStripButton CheckedButton
        {
            [DebuggerStepThrough]
            get { return _checkedButton; }
            set
            {
                if (_checkedButton != value)
                {
                    if (_checkedButton != null)
                    {
                        _checkedButton.Checked = false;
                    }
                    if (value != null && !btnPointer.Equals(value))
                    {
                        btnPointer.Checked = false;
                        value.Checked = true;
                        _checkedButton = value;
                    }
                    else
                    {
                        btnPointer.Checked = true;
                        _checkedButton = null;
                    }
                }
            }
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
#if CHECKERS
            if (IsSelectionEmpty)
            {
                throw new InvalidOperationException();
            }
#endif
            var b = new StringBuilder();
            bool isFirst = true;
            foreach (var c in _selectionList)
            {
                if (!isFirst)
                {
                    b.Append(", ");
                }
                else
                {
                    isFirst = false;
                }
                b.Append(c.Owner.Title);
            }

            if (Extenders.ConfirmDelete(b.ToString()))
            {
                var list = _selectionList.ToArray();
                UnSelectAll();
                var mg = new ModificationCollection<HtmlControlModification>();
                foreach (var c in list)
                {
                    FreeResources(c.Owner);
                    HtmlDesignMovableControl owner = c.Owner;
                    owner.NotifyDelete();

                    mg.Add(HtmlControlModification.GetRemoved(owner));

                    owner.Parent = null;
                }
                if (mg.Count > 0)
                {
                    HtmlPage.AddUndoOperation(mg.Count == 1 ? (IModification)mg[0] : mg);
                }
                else
                {
                    throw new InvalidOperationException();
                }
                UpdateUndoRedoState();
            }
        }

        private void cmsHtmlElement_Opening(object sender, CancelEventArgs e)
        {
            if (miDelete.Enabled = miProperties.Enabled = !IsSelectionEmpty)
            {
                miEditInMSWord.Visible = _selectionList.Count == 1 && (_selectionList[0].Owner is HtmlCodeSnippet);
            }
        }

        private void ControlControlParsed([NotNull]HtmlControl owner, [NotNull]HtmlControl c)
        {
            SetUpControl(c);
        }

        private void SetUpControl([NotNull]HtmlControl c)
        {
            c.Control.MouseDoubleClick += ControlMouseDoubleClick;
            c.Control.MouseDown += ControlMouseDown;
            c.Control.ContextMenuStrip = cmsHtmlElement;
        }

        private void ControlMouseDown(object sender, MouseEventArgs e)
        {
            var designControl = (HtmlDesignMovableControl)((Control)sender).Tag;
            if ((ModifierKeys & Keys.Control) == 0)
            {
                if (!IsControlSelected(designControl))
                {
                    UnSelectAll();
                    SelectControl(designControl);
                }
            }
            else
            {
                if (IsControlSelected(designControl))
                {
                    UnSelectControl(designControl);
                }
                else
                {
                    SelectControl(designControl);
                }
            }
        }

        private static void ControlKeyUp(object sender, KeyEventArgs e)
        {
            Control c;

            if ( ((Control)sender).GetType().Name == "TextBox" )
                return;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    c = ((Control)sender);
                    if (e.Shift)
                    {
                        if (c.Height > 20)
                        {
                            c.Height -= e.Control ? 10 : 1;
                        }
                    }
                    else
                    {
                        if (c.Top > 20)
                        {
                            c.Top -= e.Control ? 10 : 1;
                        }
                    }
                    break;

                case Keys.Down:
                    c = ((Control)sender);
                    if (e.Shift)
                    {
                        c.Height += e.Control ? 10 : 1;
                    }
                    else
                    {
                        c.Top += e.Control ? 10 : 1;
                    }
                    break;

                case Keys.Left:
                    c = ((Control)sender);
                    if (e.Shift)
                    {
                        if (c.Width > 20)
                        {
                            c.Width -= e.Control ? 10 : 1;
                        }
                    }
                    else
                    {
                        if (c.Left > 20)
                        {
                            c.Left -= e.Control ? 10 : 1;
                        }
                    }
                    break;

                case Keys.Right:
                    c = ((Control)sender);
                    if (e.Shift)
                    {
                        c.Width += e.Control ? 10 : 1;
                    }
                    else
                    {
                        c.Left += e.Control ? 10 : 1;
                    }
                    break;
            }
        }

        private static void ControlMouseDoubleClick(object sender, MouseEventArgs e)
        {
            Forms.PropertyEditor.Show();
            Forms.PropertyEditor.BringToFront();
        }

        private void editInMSWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if CHECKERS
            if (_selectionList.Count != 1)
            {
                throw new InvalidOperationException();
            }
#endif
            ((CodeSnippet)_selectionList[0].Owner.Control).EditInWord();
        }

        private void PanelClick(object sender, EventArgs e)
        {
            if (CheckedButton != null)
            {
                var c = ((Type)CheckedButton.Tag).Create<HtmlDesignMovableControl>();
                c.Parent = HtmlPage;
                c.Control.Location = HtmlPage.Control.PointToClient(MousePosition);
                SetUpControl(c);
                UnSelectAllInternal();
                SelectControl(c);
                CheckedButton = null;
                c.Control.Focus();
                HtmlPage.AddUndoOperation(HtmlControlModification.GetAdded(c));
                UpdateUndoRedoState();
            }
            else
            {
                HtmlPage.Control.Focus();
                UnSelectAll();
                Forms.PropertyEditor.SetContext(HtmlPage, GetScope(), PropertyEditorSelectedObjectChanged).ValueChanging += ControlProperty_ValueChanging;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ToolStripButton _checkedButton;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<BoundControl> _selectionList = new List<BoundControl>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HtmlPage _htmlPage;
    }
}