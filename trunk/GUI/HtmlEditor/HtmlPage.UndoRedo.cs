using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Common;
    using Course;

    public partial class HtmlPage : IUndoRedoSupportable
    {
        public void Undo()
        {
#if CHECKERS
            if (!CanUndo)
            {
                throw new InvalidOperationException();
            }
#endif
            IModification m = _UndoStack.Pop();
            m.Undo();
            _RedoStack.Push(m);
            Course.NotifyChanged();
        }

        public void Redo()
        {
#if CHECKERS
            if (!CanRedo)
            {
                throw new InvalidOperationException();
            }
#endif
            IModification m = _RedoStack.Pop();
            m.Redo();
            _UndoStack.Push(m);
            Course.NotifyChanged();
        }

        [Browsable(false)]
        public bool CanUndo
        {
            get { return _UndoStack.Count > 0; }
        }

        [Browsable(false)]
        public bool CanRedo
        {
            get { return _RedoStack.Count > 0; }
        }

        [Browsable(false)]
        [NotNull]
        public string UndoTitle
        {
            get
            {
#if CHECKERS
                if (!CanUndo)
                {
                    throw new InvalidOperationException();
                }
#endif
                return _UndoStack.Peek().Title;
            }
        }

        [Browsable(false)]
        [NotNull]
        public string RedoTitle
        {
            get
            {
#if CHECKERS
                if (!CanRedo)
                {
                    throw new InvalidOperationException();
                }
#endif
                return _RedoStack.Peek().Title;
            }
        }

        internal void AddUndoOperation([NotNull]IModification m)
        {
            if (_RedoStack.Count > 0)
            {
                foreach (var md in _RedoStack)
                {
                    md.Dispose();
                }
                _RedoStack.Clear();
            }
            _UndoStack.Push(m);
            Course.NotifyChanged();
        }

        private readonly Stack<IModification> _RedoStack = new Stack<IModification>();
        private readonly Stack<IModification> _UndoStack = new Stack<IModification>();
    }

    public partial class PageEditor
    {
        private void ControlProperty_ValueChanging(object control, string propertyName, object oldValue, object newValue)
        {
            if (!_IsUndoRedoState)
            {
                HtmlPage.AddUndoOperation(HtmlControlModification.GetPropertyChanged((HtmlControl) control, propertyName, oldValue));
                UpdateUndoRedoState();
            }
        }

        private void BeginUndoRedo()
        {
#if CHECKERS
            if (_IsUndoRedoState)
            {
                throw new InvalidOperationException();
            }
#endif
            _IsUndoRedoState = true;
        }

        private void EndUndoRedo()
        {
#if CHECKERS
            if (!_IsUndoRedoState)
            {
                throw new InvalidOperationException();
            }
#endif
            _IsUndoRedoState = false;
            Forms.PropertyEditor.RefreshContent();
        }

        private void UpdateUndoRedoState()
        {
            var p = HtmlPage;
            miUndo.ToolTipText = (miUndo.Enabled = p.CanUndo) ? "Undo: " + p.UndoTitle : "No Undo operation available";
            miRedo.Enabled = p.CanRedo;
            miRedo.ToolTipText = (miRedo.Enabled = p.CanRedo) ? "Redo: " + p.RedoTitle : "No Redo operation available";
        }

        private void miUndo_Click(object sender, EventArgs e)
        {
            BeginUndoRedo();
            HtmlPage.Undo();
            UpdateUndoRedoState();
            EndUndoRedo();
        }

        private void miRedo_Click(object sender, EventArgs e)
        {
            BeginUndoRedo();
            HtmlPage.Redo();
            UpdateUndoRedoState();
            EndUndoRedo();
        }

        private void Control_BeginResize(HtmlDesignMovableControl c)
        {
            _PreviousSize = c.Control.Size;
        }

        private void Control_EndResize(HtmlDesignMovableControl c)
        {
            if (c.Control.Size != _PreviousSize)
            {
                HtmlPage.AddUndoOperation(HtmlControlModification.GetResized(c, _PreviousSize));
                UpdateUndoRedoState();
            }
        }

        private void Control_BeginMove(HtmlDesignMovableControl c)
        {
            foreach (var bc in _selectionList)
            {
                HtmlDesignMovableControl owner = bc.Owner;
                _PreviousLocations[owner] = owner.Control.Location;
            }
            c.Control.LocationChanged += Control_LocationChanged;
        }

        private void Control_EndMove(HtmlDesignMovableControl c)
        {
            IModification m;
            if (_selectionList.Count == 1)
            {
                HtmlDesignMovableControl o = _selectionList[0].Owner;
                Point previousLocation = _PreviousLocations[o];
                m = previousLocation != o.Control.Location ? HtmlControlModification.GetMoved(o, previousLocation) : (IModification)null;
            }
            else
            {
                var list = new ModificationCollection<HtmlControlModification>();
                foreach (var bc in _selectionList)
                {
                    HtmlDesignMovableControl o = bc.Owner;
                    Point previousLocation = _PreviousLocations[o];
                    if (previousLocation != o.Control.Location)
                    {
                        list.Add(HtmlControlModification.GetMoved(o, previousLocation));
                    }
                }
                if (list.Count == 1)
                {
                    m = list[0];
                }
                else
                {
                    m = list.Count > 0 ? list : null;    
                }
            }
            if(m != null)
            {
                HtmlPage.AddUndoOperation(m);
                UpdateUndoRedoState();
            }
            c.Control.LocationChanged -= Control_LocationChanged;
        }

        private void Control_LocationChanged(object sender, EventArgs e)
        {
            var winControl = (Control) sender;
            var activeControl = (HtmlDesignMovableControl) winControl.Tag;
            Point previousLocation = _PreviousLocations[activeControl];
            var offset = new Point(winControl.Location.X - previousLocation.X, winControl.Location.Y - previousLocation.Y);

            foreach (var bc in _selectionList)
            {
                HtmlDesignMovableControl c = bc.Owner;
                if (c != activeControl)
                {
                    var pl = _PreviousLocations[c];
                    pl.Offset(offset);
                    c.Control.Location = pl;
                }
            }
        }

        private bool _IsUndoRedoState;
        private readonly Dictionary<HtmlControl, Point> _PreviousLocations = new Dictionary<HtmlControl, Point>();
        private Size _PreviousSize;
    }
}
