#define LOG_MOVINGS
namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    ///<summary>
    /// Represents control 
    ///</summary>
    public abstract class HtmlDesignMovableControl : HtmlControl, IEquatable<HtmlDesignMovableControl>, IDeleteNotifiable
    {
        ///<summary>
        /// Gets or sets value determining can the control be moved in design mode
        ///</summary>
        [DefaultValue(true)]
        [Category("Design")]
        [Description("Determine can the control be moved in design mode")]
        public bool Movable
        {
            get { return _Movable; }
            set
            {
                if (_Movable != value)
                {
                    _Movable = value;
                    if (value)
                    {
                        Control.MouseDown += Control_MouseDown;
                        Control.MouseUp += Control_MouseUp;
                    }
                    else
                    {
                        Control.MouseDown -= Control_MouseDown;
                        Control.MouseUp -= Control_MouseUp;
                    }
                }
            }
        }

        ///<summary>
        /// Gets or sets value determining can the control be resized in design mode
        ///</summary>
        [DefaultValue(true)]
        [Category("Design")]
        [Description("Determine can the control be resized in design mode")]
        public bool Resizable
        {
            get { return _Resizable; }
            set
            {
                _Resizable = value;
            }
        }

        public bool Equals([CanBeNull]HtmlDesignMovableControl other)
        {
            return other != null ? Name == other.Name : false;
        }

        public override bool Equals([CanBeNull]object obj)
        {
            return Equals(obj as HtmlDesignMovableControl);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        ///<summary>
        /// Occurs when control is begun moving
        ///</summary>
        public event Action<HtmlDesignMovableControl> BeginMove;

        ///<summary>
        /// Occurs when control is moved
        ///</summary>
        public event Action<HtmlDesignMovableControl> EndMove;

        ///<summary>
        /// Occurs when control is begun resizing
        ///</summary>
        public event Action<HtmlDesignMovableControl> BeginResize;

        ///<summary>
        /// Occurs when control is resized
        ///</summary>
        public event Action<HtmlDesignMovableControl> EndResize;

        #region IDeleteNotifieble members

        public void NotifyDelete()
        {
            if (Deleting != null)
            {
                Deleting();
            }
        }

        public void NotifyUndoDelete()
        {
            if (UnDeleting != null)
            {
                UnDeleting();
            }
        }

        public event Action Deleting;
        public event Action UnDeleting;

        #endregion


        protected /* virtual */ void OnBeginResize()
        {
            LogMovings("BeginResize");
            if (BeginResize != null)
            {
                BeginResize(this);
            }
        }

        protected /* virtual */ void OnEndResize()
        {
            LogMovings("EndResize");
            if (EndResize != null)
            {
                EndResize(this);
            }
        }

        protected /* virtual */ void OnBeginMove()
        {
            Debug.Assert(!_InMoving);
            LogMovings("BeginMove");
            _InMoving = true;
            Control.Cursor = Cursors.NoMove2D;
            if (BeginMove != null)
            {
                BeginMove(this);
            }
            
        }

        protected /* virtual */ void OnEndMove()
        {
            Debug.Assert(_InMoving);
            LogMovings("EndMove");
            _InMoving = false;
            Control.MouseMove -= Control_MouseMove;
            Control.Cursor = Cursors.Default;
            if (EndMove != null)
            {
                EndMove(this);
            }
        }

        protected override void ReCreateControl()
        {
            base.ReCreateControl();
            if (_Movable)
            {
                Control.MouseDown += Control_MouseDown;
                Control.MouseUp += Control_MouseUp;
            }
            Control.LostFocus += Control_LostFocus;
        }

        internal void BindEvents([NotNull]BoundControl bc)
        {
            Control.LocationChanged += Control_LocationChanged;
            bc.Operation += BoundControl_Operation;
        }

        internal void UnBindEvents([NotNull]BoundControl bc)
        {
            Control.LocationChanged -= Control_LocationChanged;
            bc.Operation -= BoundControl_Operation;
        }

        internal void NotifyBeginResize()
        {
            OnBeginResize();
        }

        internal void NotifyEndResize()
        {
            OnEndResize();
        }

        private void Control_LocationChanged(object sender, EventArgs e)
        {
            if (!_InMoving)
            {
                OnBeginMove();
            }
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            _StartControlLocation = Control.Location;
            _StartCursorLocation = Control.MousePosition;

            Control.MouseMove += Control_MouseMove;
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (_InMoving)
            {
                OnEndMove();
            }
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (Movable)
            {
                bool isMouseButtonPressed = (Control.MouseButtons & (MouseButtons.Left | MouseButtons.Right)) != 0;
                if (!_InMoving)
                {
                    if (isMouseButtonPressed)
                    {
                        OnBeginMove();
                    }
                }
                else
                {
                    if(!isMouseButtonPressed)
                    {
                        OnEndMove();
                    }
                }
                if (_InMoving)
                {
                    var curPos = Control.MousePosition;
                    Control.Location = new Point
                        (_StartControlLocation.X - _StartCursorLocation.X + curPos.X,
                         _StartControlLocation.Y - _StartCursorLocation.Y + curPos.Y);
                }
            }
        }

        private void Control_LostFocus(object sender, EventArgs e)
        {
            if (_InMoving)
            {
                OnEndMove();  
            }
        }

        private void BoundControl_Operation(object sender, EventArgs e)
        {
            var bc = (BoundControl)sender;
            Control.Size = bc.Size;
            Control.Location = bc.Location;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _InMoving;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _Movable = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _Resizable = true;

        private Point _StartCursorLocation, _StartControlLocation;

        [Conditional("LOG_MOVINGS")]
        private void LogMovings(string actionName)
        {
            Debug.WriteLine(Name + " - " + actionName);
        }
    }
}