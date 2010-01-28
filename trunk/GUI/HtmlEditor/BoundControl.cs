using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    ///<summary>
    /// Represents bound control used by <see cref="PageEditor" /> to mark <see cref="HtmlDesignMovableControl" /> as selected and resize it using mouse
    ///</summary>
    public class BoundControl : Control, IDisposable, IEquatable<BoundControl>
    {
        private class BoundsInfo
        {
            private readonly Rectangle[] _BoundSquares;
            private readonly Size _ResizingSquareSize = new Size(5, 5);

            public BoundsInfo(Rectangle rect)
            {
                var rs = rect.Size;
                _BoundSquares = new Rectangle[8];
                _BoundSquares[1] =
                    new Rectangle(new Point(rect.X + rs.Width / 2 - _ResizingSquareSize.Width / 2, rect.Y),
                                  _ResizingSquareSize);
                _BoundSquares[5] =
                    new Rectangle(
                        new Point(rect.X + rs.Width / 2 - _ResizingSquareSize.Width / 2,
                                  rect.Y + rs.Height - _ResizingSquareSize.Height), _ResizingSquareSize);

                _BoundSquares[7] =
                    new Rectangle(new Point(rect.X, rect.Y + rs.Height / 2 - _ResizingSquareSize.Height / 2),
                                  _ResizingSquareSize);
                _BoundSquares[3] =
                    new Rectangle(
                        new Point(rect.X + rs.Width - _ResizingSquareSize.Width,
                                  rect.Y + rs.Height / 2 - _ResizingSquareSize.Height / 2), _ResizingSquareSize);

                _BoundSquares[0] = new Rectangle(new Point(rect.X, rect.Y), _ResizingSquareSize);
                _BoundSquares[4] =
                    new Rectangle(
                        new Point(rect.X + rs.Width - _ResizingSquareSize.Width,
                                  rect.Y + rs.Height - _ResizingSquareSize.Height), _ResizingSquareSize);

                _BoundSquares[2] =
                    new Rectangle(new Point(rect.X + rs.Width - _ResizingSquareSize.Width, rect.Y), _ResizingSquareSize);
                _BoundSquares[6] =
                    new Rectangle(new Point(rect.X, rect.Y + rs.Height - _ResizingSquareSize.Height),
                                  _ResizingSquareSize);

                CurrentBoundIndex = -1;
            }

            public int CurrentBoundIndex { get; set; }

            public void Draw([NotNull]Graphics g, [NotNull]Brush brush)
            {
                for (var i = 0; i < _BoundSquares.Length; i++)
                {
                    g.FillRectangle(brush, _BoundSquares[i]);
                }
                g.Save();
            }

            public int GetBoundIndexFromPoint(Point p)
            {
                for (var i = 0; i < _BoundSquares.Length; i++)
                {
                    if (_BoundSquares[i].Contains(p))
                    {
                        return CurrentBoundIndex = i;
                    }
                }
                throw new InvalidOperationException("Invalid point");
            }

            [NotNull]
            public Region GetBoundRegion()
            {
                var region = new Region(new Rectangle(0, 0, 0, 0));
                for (var i = 0; i < _BoundSquares.Length; i++)
                {
                    region.Union(_BoundSquares[i]);
                }
                return region;
            }
        }

        ///<summary>
        /// Creates new instance of <see cref="BoundControl" /> binded to <see cref="c" />
        ///</summary>
        ///<param name="c">Instance of Movable control that new control should bind to</param>
        public BoundControl([NotNull]HtmlDesignMovableControl c)
        {
            Debug.Assert(c != null);
            Owner = c;
            c.Disposed += Dispose;
            c.Deleting += Dispose;
            Control control = c.Control;
            Location = control.Location;
            Size = control.Size;          
            Parent = control.Parent;

            _ResizeBounds = new BoundsInfo(ClientRectangle);
            Region = _ResizeBounds.GetBoundRegion();

            c.BindEvents(this);
            control.LocationChanged += Owner_LocationChanged;
            control.SizeChanged += Owner_SizeChanged;
            Show();
            BringToFront();
        }

        ///<summary>
        /// Instance of <see cref="HtmlDesignMovableControl" /> that used by this object to bind to
        ///</summary>
        [NotNull]
        public readonly HtmlDesignMovableControl Owner;

        new public void Dispose()
        {       
            Dispose(true);
        }

        public bool Equals([CanBeNull]BoundControl other)
        {
            return other != null ? Owner.Equals(other.Owner) : false;
        }

        public override bool Equals([CanBeNull]object obj)
        {
            return Equals(obj as BoundControl);
        }

        public override int GetHashCode()
        {
            return Owner.GetHashCode();
        }

        ///<summary>
        ///  Occurs when Size or Location of control has been changed 
        ///</summary>
        public event EventHandler Operation
        {
            add
            {
                SizeChanged += value;
                LocationChanged += value;
            }
            remove
            {
                SizeChanged -= value;
                LocationChanged -= value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            

            base.Dispose(disposing);
            Owner.Control.LocationChanged -= Owner_LocationChanged;
            Owner.UnBindEvents(this);
           
            Parent = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _ResizeBounds.Draw(e.Graphics, Brushes.Red);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if(_ResizeBounds != null)
            {
                var index = _ResizeBounds.CurrentBoundIndex;
                _ResizeBounds = new BoundsInfo(ClientRectangle);
                Region = _ResizeBounds.GetBoundRegion();
                _ResizeBounds.CurrentBoundIndex = index;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = __CursorsForBounds[_ResizeBounds.GetBoundIndexFromPoint(PointToClient(MousePosition)) % 4];
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _StartControlLocation = Location;
            _StartCursorLocation = MousePosition;
            _AcceptMoving = Owner.Resizable;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_AcceptMoving)
            {
                _AcceptMoving = false;
                if (_InMoving)
                {
                    _InMoving = false;
                    Owner.NotifyEndResize();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_AcceptMoving)
            {
                bool isMouseButtonPressed = (MouseButtons & (MouseButtons.Left | MouseButtons.Right)) != 0;
                if (!_InMoving)
                {
                    if (isMouseButtonPressed)
                    {
                        _InMoving = true;
                        Owner.NotifyBeginResize();
                    }
                }
                else
                {
                    if (!isMouseButtonPressed)
                    {
                        _InMoving = false;
                        Owner.NotifyEndResize();
                    }
                }
                if (_InMoving)
                {
                    var newLocation = Location;
                    var newSize = Size;
                    var curPos = MousePosition;

                    switch (_ResizeBounds.CurrentBoundIndex)
                    {
                        case 0:
                            newLocation.X = _StartControlLocation.X - _StartCursorLocation.X + curPos.X;
                            newLocation.Y = _StartControlLocation.Y - _StartCursorLocation.Y + curPos.Y;
                            newSize.Width -= (newLocation.X - Location.X);
                            newSize.Height -= (newLocation.Y - Location.Y);
                            break;

                        case 1:
                            newLocation.Y = _StartControlLocation.Y - _StartCursorLocation.Y + curPos.Y;
                            newSize.Height -= (newLocation.Y - Location.Y);
                            break;

                        case 2:
                            newLocation.Y = _StartControlLocation.Y - _StartCursorLocation.Y + curPos.Y;
                            newSize.Height -= (newLocation.Y - Location.Y);
                            newSize.Width = e.X;
                            break;

                        case 3:
                            newSize.Width = e.X;
                            break;

                        case 4:
                            newSize.Width = e.X;
                            newSize.Height = e.Y;
                            break;

                        case 5:
                            newSize.Height = e.Y;
                            break;

                        case 6:
                            newLocation.X = _StartControlLocation.X - _StartCursorLocation.X + curPos.X;
                            newSize.Width -= (newLocation.X - Location.X);
                            newSize.Height = e.Y;
                            break;

                        case 7:
                            newLocation.X = _StartControlLocation.X - _StartCursorLocation.X + curPos.X;
                            newSize.Width -= (newLocation.X - Location.X);
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                    Control ownerControl = Owner.Control;
                    ownerControl.Location = newLocation;
                    ownerControl.Size = newSize;
                }
            }
        }

        private void Owner_LocationChanged(object sender, EventArgs e)
        {
            Point temp = Owner.Control.Location;
            if (temp != Location)
            {
                Location = temp;
            }
        }

        private void Owner_SizeChanged(object sender, EventArgs e)
        {
            Size = Owner.Control.Size;
        }

        private Point _StartControlLocation;
        private Point _StartCursorLocation;
        private bool _AcceptMoving, _InMoving;
        private BoundsInfo _ResizeBounds;

        private static readonly Cursor[] __CursorsForBounds = new[] { Cursors.SizeNWSE, Cursors.SizeNS, Cursors.SizeNESW, Cursors.SizeWE };
    }
}