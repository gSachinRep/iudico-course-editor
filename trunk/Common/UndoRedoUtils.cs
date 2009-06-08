using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FireFly.CourseEditor.Common
{
    using GUI.HtmlEditor;
    using Course.Manifest;

    ///<summary>
    /// Represents classes witch support undo redo operations
    ///</summary>
    public interface IUndoRedoSupportable
    {
        /// <summary>
        /// Undo last operation
        /// </summary>
        void Undo();
        /// <summary>
        /// Redo last undo operation
        /// </summary>
        void Redo();
        /// <summary>
        ///  
        /// </summary>
        bool CanUndo { get; }
        /// <summary>
        /// 
        /// </summary>
        bool CanRedo { get; }
        /// <summary>
        /// Title for undo operation can be performed with <see cref="Undo" />
        /// </summary>
        string UndoTitle { get; }
        /// <summary>
        /// Title for redo operation can be performed with <see cref="Redo" />
        /// </summary>
        string RedoTitle { get; }
    }

    ///<summary>
    /// represents type of modification
    ///</summary>
    public enum MODIFICATION_TYPE
    {
        ADDED,
        REMOVED,
        PROPERTY_CHANGED,
        MOVED,
        RESIZED
    }

    ///<summary>
    /// Represents classes that mean modification of instance of <see cref="IUndoRedoSupportable"/>
    /// States of instance should be checked by implementation
    ///</summary>
    public interface IModification : IDisposable, ITitled
    {
        /// <summary>
        /// Undo this modification
        /// </summary>
        void Undo();
        /// <summary>
        /// Redo this modification
        /// </summary>
        void Redo();
    }

    ///<summary>
    /// Represent collection of <typeparamref name="TModification"/> objects which understanding as single modification by <see cref="IUndoRedoSupportable" /> objects.
    ///</summary>
    ///<typeparam name="TModification">Type that all modification should conform to</typeparam>
    public class ModificationCollection<TModification> : List<TModification>, IModification
        where TModification : IModification
    {
        public void Dispose()
        {
            foreach (var m in this)
            {
                m.Dispose();
            }
        }

        [NotNull]
        public string Title
        {
            get
            {
                var b = new StringBuilder();
                bool first = true;
                foreach (var m in this)
                {
                    if (!first)
                    {
                        b.Append(", ");
                    }
                    else
                    {
                        first = false;
                    }
                    b.Append(m.Title);
                }
                return b.ToString();
            }
            set { throw new NotSupportedException(); }
        }

        public event Action TitleChanged { add { } remove { } }

        public void Undo()
        {
            foreach (var m in this)
            {
                m.Undo();
            }
        }

        public void Redo()
        {
            foreach (var m in this)
            {
                m.Redo();
            }
        }
    }

    ///<summary>
    /// Represents single modification of <see cref="HtmlControl"/>
    ///</summary>
    [DebuggerDisplay("{ChangeType} - {Control} - {State}")]
    public struct HtmlControlModification : IModification
    {
        private enum MODIFICATION_STATE
        {
            FOR_UNDO,
            FOR_REDO
        }

        ///<summary>
        /// Creates modification for added control
        ///</summary>
        ///<param name="control">Control that has been added</param>
        ///<returns>Instance of <see cref="HtmlControlModification"/></returns>
        public static HtmlControlModification GetAdded([NotNull]HtmlControl control)
        {
            return new HtmlControlModification(MODIFICATION_TYPE.ADDED, control, null);
        }

        ///<summary>
        /// Creates modification for removed control
        ///</summary>
        ///<param name="control">Control that has been removed</param>
        ///<returns>Instance of <see cref="HtmlControlModification"/></returns>
        public static HtmlControlModification GetRemoved([NotNull]HtmlControl control)
        {
            return new HtmlControlModification(MODIFICATION_TYPE.REMOVED, control, null) { _Parent = control.Parent };
        }

        ///<summary>
        /// Creates modification for moved control
        ///</summary>
        ///<param name="control">Control that has been moved</param>
        ///<param name="previousLocation">Previous location of control</param>
        ///<returns>Instance of <see cref="HtmlControlModification"/></returns>
        public static HtmlControlModification GetMoved(/*HtmlDesignMovableControl*/ [NotNull] HtmlControl control, Point previousLocation)
        {
            return new HtmlControlModification(MODIFICATION_TYPE.MOVED, control, previousLocation);
        }

        ///<summary>
        /// Creates modification for resized control
        ///</summary>
        ///<param name="control">Control that has been resized</param>
        ///<param name="previousSize">Previous size of control</param>
        ///<returns>Instance of <see cref="HtmlControlModification"/></returns>
        public static HtmlControlModification GetResized(/*HtmlDesignMovableControl*/ [NotNull] HtmlControl control, Size previousSize)
        {
            return new HtmlControlModification(MODIFICATION_TYPE.RESIZED, control, previousSize);
        }

        ///<summary>
        /// Creates modification for control which property has been changed
        ///</summary>
        ///<param name="control">Control which property has been changed</param>
        ///<param name="propertyName">Name of property that was changed</param>
        ///<param name="oldValue">Old value of property</param>
        ///<returns>Instance of <see cref="HtmlControlModification"/></returns>
        public static HtmlControlModification GetPropertyChanged([NotNull]HtmlControl control, [NotNull]string propertyName, object oldValue)
        {
            return new HtmlControlModification(MODIFICATION_TYPE.PROPERTY_CHANGED, control, oldValue) { PropertyName = propertyName };
        }

        private HtmlControlModification(MODIFICATION_TYPE type, [NotNull]HtmlControl control, object value)
        {
#if CHECKERS
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
#endif
            ChangeType = type;
            Control = control;
            OldValue = value;
            _Parent = null;
            PropertyName = null;
            _State = MODIFICATION_STATE.FOR_UNDO;
        }

        ///<summary>
        /// Kind of modification
        ///</summary>
        public readonly MODIFICATION_TYPE ChangeType;

        ///<summary>
        /// Control that was modified
        ///</summary>
        [NotNull]
        public readonly HtmlControl Control;
        /// <summary>
        /// Value before modification, or value after <see cref="Undo" /> called. (size, position, property value)
        /// </summary>
        public object OldValue;

        ///<summary>
        /// Name of property if <see cref="ChangeType" /> is equals to <see cref="MODIFICATION_TYPE.PROPERTY_CHANGED" /> 
        ///</summary>
        [NotNull]
        public string PropertyName;

        private HtmlControl _Parent;
        private MODIFICATION_STATE _State;

        public void Dispose()
        {
            switch (ChangeType)
            {
                case MODIFICATION_TYPE.ADDED:
                    if (_State == MODIFICATION_STATE.FOR_REDO)
                    {
                        Control.Dispose();
                    }
                    break;

                case MODIFICATION_TYPE.PROPERTY_CHANGED:
                    var d = OldValue as IDisposable;
                    if (d != null)
                    {
                        d.Dispose();
                    }
                    break;

                case MODIFICATION_TYPE.REMOVED:
                    if (_State == MODIFICATION_STATE.FOR_UNDO)
                    {
                        Control.Dispose();
                    }
                    break;

                case MODIFICATION_TYPE.RESIZED:
                case MODIFICATION_TYPE.MOVED:
                    break;

                default:
                    throw new InvalidOperationException();

            }
        }

        public void Undo()
        {
            if (_State != MODIFICATION_STATE.FOR_UNDO)
            {
                throw new InvalidOperationException();
            }
            DoUndoRedo();
            _State = MODIFICATION_STATE.FOR_REDO;
        }

        public void Redo()
        {
            if (_State != MODIFICATION_STATE.FOR_REDO)
            {
                throw new InvalidOperationException();
            }
            DoUndoRedo();
            _State = MODIFICATION_STATE.FOR_UNDO;
        }

        private void DoUndoRedo()
        {
            switch (ChangeType)
            {
                case MODIFICATION_TYPE.ADDED:
                    if (_State == MODIFICATION_STATE.FOR_REDO)
                    {
                        DoRedoAddOrUndoRemove();
                    }
                    else
                    {
                        DoUndoAddOrRedoRemove();
                    }
                    break;

                case MODIFICATION_TYPE.PROPERTY_CHANGED:
                    DoChangeValue();
                    break;

                case MODIFICATION_TYPE.REMOVED:
                    if (_State == MODIFICATION_STATE.FOR_UNDO)
                    {
                        DoRedoAddOrUndoRemove();
                    }
                    else
                    {
                        DoUndoAddOrRedoRemove();
                    }
                    break;

                case MODIFICATION_TYPE.MOVED:
                    DoMove();
                    break;

                case MODIFICATION_TYPE.RESIZED:
                    DoSize();
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void DoChangeValue()
        {
            var typeDescInstance = new FFTypeDescriptor(Control);
            PropertyDescriptor pd = typeDescInstance.GetProperties()[PropertyName];
            object temp = pd.GetValue(typeDescInstance);
            pd.SetValue(typeDescInstance, OldValue);
            OldValue = temp;
        }

        private void DoRedoAddOrUndoRemove()
        {
#if CHECKERS
            if (_Parent == null)
            {
                throw new InvalidOperationException();
            }
#endif
            Control.Parent = _Parent;
            _Parent = null;
            var dn = Control as IDeleteNotifiable;
            if (dn != null)
            {
                dn.NotifyUndoDelete();
            }
        }

        private void DoUndoAddOrRedoRemove()
        {
#if CHECKERS
            if (_Parent != null)
            {
                throw new InvalidOperationException();
            }
#endif
            _Parent = Control.Parent;
            Control.Parent = null;
            var dn = Control as IDeleteNotifiable;
            if (dn != null)
            {
                dn.NotifyDelete();
            }
        }

        private void DoMove()
        {
            Control c = Control.Control;
            object temp = c.Location;
            c.Location = (Point)OldValue;
            OldValue = temp;
        }

        private void DoSize()
        {
            Control c = Control.Control;
            object temp = c.Size;
            c.Size = (Size)OldValue;
            OldValue = temp;
        }

        #region ITitled Members

        [NotNull]
        public string Title
        {
            get
            {
                switch (ChangeType)
                {
                    case MODIFICATION_TYPE.ADDED:
                        return "Adding '" + Control.Title + "'";

                    case MODIFICATION_TYPE.PROPERTY_CHANGED:
                        return "Changing '" + PropertyName + "' of '" + Control.Title + "'";

                    case MODIFICATION_TYPE.REMOVED:
                        return "Removing '" + Control.Title + "'";

                    case MODIFICATION_TYPE.MOVED:
                        return "Moving '" + Control.Title + "'";

                    case MODIFICATION_TYPE.RESIZED:
                        return "Changing size of '" + Control.Title + "'";

                    default:
                        throw new NotImplementedException();
                }
            }
            set { throw new NotSupportedException(); }
        }

        public event Action TitleChanged
        {
            add { }
            remove { }
        }

        #endregion
    }
}
