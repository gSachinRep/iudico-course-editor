using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Course;
    using Common;
    using Course.Manifest;

    public partial class HtmlControl : IValidateble
    {
        public void ReValidate()
        {
#if LOGGER
            using (Logger.Scope(IsWinControlCreated ? Title : "<Win control is not created>" + " - Validating"))
            {
#endif
                if (BeginValidate != null)
                {
                    BeginValidate(this);
                }
                Error[] backup = _ErrorList.ToArray();
                ClearErrors();
                InternalValidate();
                foreach (var e in Errors.Except(backup))
                {
                    OnErrorFound(e);
                }
                foreach (var e in backup.Except(Errors))
                {
                    OnErrorFixed(e);
                }
                if (EndValidate != null)
                {
                    EndValidate(this);
                }
#if LOGGER
            }
#endif
        }

        [Browsable(false)]
        public IErrors Errors
        {
            get { return _ErrorList; }
        }

        [Browsable(false)]
        public bool IsValid
        {
            get { return _ErrorList.Count == 0; }
        }

        [Browsable(false)]
        public event Action<IValidateble, Error> ErrorFound;

        [Browsable(false)]
        public event Action<IValidateble, Error> ErrorFixed;

        [Browsable(false)]
        public event Action<IValidateble> BeginValidate;

        [Browsable(false)]
        public event Action<IValidateble> EndValidate;

        protected readonly ErrorsCollection _ErrorList = new ErrorsCollection();

        protected virtual void OnErrorFound(Error error)
        {
            if (!_ErrorList.Contains(error))
            {
                _ErrorList.Add(error);
                if (ErrorFound != null)
                {
                    ErrorFound(this, error);
                }
                var errorControl = ErrorControl;
                if (errorControl != null)
                {
                    errorControl.SetErrors(Errors);
                }
            }
        }

        protected virtual void OnErrorFixed(Error error)
        {
            if (_ErrorList.Remove(error))
            {
                if (ErrorFixed != null)
                {
                    ErrorFixed(this, error);
                }
                if (IsValid)
                {
                    if (_ErrorControl != null)
                    {
                        _ErrorControl.Dispose();
                    }
                }
                else
                {
                    if (ErrorControl != null)
                    {
                        ErrorControl.SetErrors(_ErrorList);
                    }
                }
            }
        }

        protected void AddError(string message)
        {
            AddError(message, message);
        }

        protected void AddError(object id, string message)
        {
            OnErrorFound(new Error(this, id, message));
        }

        protected void RemoveError(object id)
        {
            var e = _ErrorList.Find(x => x.ID == id);
            if (e != null)
            {
                OnErrorFixed(e);
            }
        }

        protected internal void ClearErrors()
        {
            var list = _ErrorList.ToList(); // Clone error list because it will be changed during OnErrorFixed call.
            foreach (var e in list)
            {
                OnErrorFixed(e);
            }
        }

        protected virtual void InternalValidate()
        {
            ClearErrors();
            foreach (var c in HtmlControls)
            {
                c.ReValidate();
            }
        }

        protected virtual ErrorControl ErrorControl
        {
            get
            {
                if (_ErrorControl == null)
                {
                    EventHandler locationChanged = (sender, e) =>
                    {
                        _ErrorControl.Location = new Point(Control.Left - _ErrorControl.Width - 2, Control.Top);
                    };
                    _ErrorControl = new ErrorControl(Control.Parent);
                    _ErrorControl.Disposed += (sender, e) =>
                    {
                        _ErrorControl = null;
                        Control.LocationChanged -= locationChanged;
                    };
                    Control.LocationChanged += locationChanged;
                    Control.Parent.Controls.Add(_ErrorControl);
                    _ErrorControl.Location = new Point(Control.Left - _ErrorControl.Width - 2, Control.Top);
                }
                return _ErrorControl;
            }
        }

        private ErrorControl _ErrorControl;
    }

    ///<summary>
    /// Class to validate full course
    ///</summary>
    public class CourseValidator
    {
        private StringBuilder _Errors;
        private ErrorsCollection _CurrentList;

        ///<summary>
        /// Run validation
        ///</summary>
        ///<returns></returns>
        public bool Validate()
        {
            _Errors = new StringBuilder();
            _CurrentList = new ErrorsCollection();
            foreach (var i in Course.Organization.Items)
            {
                if (i.PageType == PageType.Question)
                {
                    var p = HtmlPageBase.GetPage(i);
                    if (p == null)
                    {
                        using (var page = new HtmlPage { Parent = new Form() })
                        {
                            page.Name = i.Title;
                            page.SetPageItem(i);
                            page.ParseHtmlFile(i.PageHref, (htmlPage, control) => { });
                            page.ReValidate();
                            _CurrentList.AddRange(page.Errors);
                        }
                    }
                    else
                    {
                        p.ReValidate();
                        _CurrentList.AddRange(p.Errors);
                    }

                    foreach (var error in _CurrentList)
                    {
                        if (_Errors.Length > 0)
                        {
                            _Errors.AppendLine();
                        }
                        _Errors.Append(i.GetFullPath());
                        if (error.Source != i)
                        {
                            _Errors.Append("/" + error.Source.Title);
                        }
                        _Errors.Append(": ");
                        _Errors.Append(error.Message);
                    }
                }
            }
            return _Errors.Length == 0;
        }

        ///<summary>
        /// Build error-string
        ///</summary>
        ///<returns></returns>
        ///<exception cref="InvalidOperationException"></exception>
        public string GetErrorMessages()
        {
#if CHECKERS
            if (_Errors == null || _Errors.Length == 0)
            {
                throw new InvalidOperationException();
            }
#endif
            return _Errors.ToString();
        }
    }
}
