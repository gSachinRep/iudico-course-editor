namespace FireFly.CourseEditor.GUI
{
    using System.Diagnostics;
    using System.Windows.Forms;
    using WeifenLuo.WinFormsUI.Docking;

    ///<summary>
    /// Base window class for all editor windows
    ///</summary>
    public partial class EditorWindowBase : DockContent
    {
        /// <summary>
        /// FOR DESIGNER ONLY
        /// </summary>
        public EditorWindowBase()
        {
        }

        ///<summary>
        /// Creates new instance of <see cref="EditorWindowBase"/> included in specified <see cref="DockPanel"/>
        ///</summary>
        ///<param name="parentDockPanel">Parent dock panel</param>
        public EditorWindowBase(DockPanel parentDockPanel)
        {
            InitializeComponent();
            _ParentDockPanel = parentDockPanel;
            if (_PreviousDockState == DockState.Unknown)
            {
                _PreviousDockState = DockState.Float;
            }
        }

        ///<summary>
        /// Dock panel this control included in
        ///</summary>
        public DockPanel DockingPanel
        {
            get { return _ParentDockPanel; }
        }

        public DockState PreviousDockState
        {
            get { return _PreviousDockState; }
        }

        public new void Show()
        {
            Show(_ParentDockPanel);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                _PreviousDockState = DockState;
                DockState = DockState.Hidden;

                e.Cancel = true;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DockPanel _ParentDockPanel;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DockState _PreviousDockState;
    }
}