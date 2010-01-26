using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Common;
    using HighlightControl;
    using Course.Manifest;
  using AdvancedCompiledTestControl;

    ///<summary>
    /// Abstract base class for all controls of Editor.
    ///</summary>
    [BindProperty("Control", "Location")]
    [BindProperty("Control", "Size")]
    [BindProperty("Control", "Text")]
    [BindProperty("Control", "Font")]
    public abstract partial class HtmlControl : IDisposable
    {
        ///<summary>
        ///  Gets or sets name of control
        ///</summary>
        ///<exception cref="FireFlyException"></exception>
        [Category("Design")]
        [NotNull]
        public string Name
        {
            get
            {
#if CHECKERS
                if (!IsWinControlCreated)
                {
                    throw new FireFlyException("Control is null");
                }
#endif
                return Control.Name ?? string.Empty;
            }
            set
            {
#if CHECKERS
                if (!IsWinControlCreated)
                {
                    throw new FireFlyException("Control is null");
                }
#endif
                if (Control.Name != value)
                {
                    Control.Name = value;
                    if (TitleChanged != null)
                    {
                        TitleChanged();
                    }
                }
            }
        }

        ///<summary>
        /// Gets or sets parent of control
        ///</summary>
        [Browsable(false)]
        [CanBeNull]
        public HtmlControl Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent != value)
                {
                    if (_ErrorControl != null)
                    {
                        _ErrorControl.Dispose();
                        _ErrorControl = null;
                    }
                    if ((_Parent = value) != null)
                    {
                        if (!IsWinControlCreated)
                        {
                            ReCreateControl();

                            // Calculate name for created control
                            var t = Control.GetType();
                            var name = t.Name.StartsWith("Html") ? t.Name.Substring(4) : t.Name;
                            var indexes = new List<int>();
                            foreach (var hc in value.HtmlControls)
                            {
                                var n = hc.Name ?? string.Empty;
                                if (n.StartsWith(name))
                                {
                                    int index;
                                    if (int.TryParse(n.Substring(name.Length), out index))
                                    {
                                        indexes.Add(index);
                                    }
                                }
                            }
                            var curInd = 1;
                            while (indexes.Contains(curInd))
                            {
                                curInd++;
                            }
                            Name = name + (curInd > 0 ? curInd.ToString() : "");
                        }
                        _Control.Parent = _Parent.Control;
                        ReValidate();
                    }
                    else
                    {
                        Control.Parent = null;
                    }
                }
            }
        }

        ///<summary>
        /// Gets Win-control represents current one
        ///</summary>
        [Browsable(false)]
        [DebuggerNonUserCode]
        [NotNull]
        public Control Control
        {
            get
            {
#if CHECKERS
                if (!IsWinControlCreated)
                {
                    throw new FireFlyException("Control is not created");
                }
#endif
                return _Control;
            }
        }

        ///<summary>
        /// Check is Win-Control already created or not
        ///</summary>
        [Browsable(false)]
        public bool IsWinControlCreated
        {
            get
            {
                return _Control != null;
            }
        }

        ///<summary>
        /// Get all control included by this one
        ///</summary>
        [Browsable(false)]
        [NotNull]
        public IEnumerable<HtmlControl> HtmlControls
        {
            get
            {
                if (IsWinControlCreated)
                {
                    foreach (Control c in Control.Controls)
                    {
                        if (c.Tag != null)
                        {
                            yield return (HtmlControl)c.Tag;
                        }
                    }
                }
                else
                {
                    yield break;
                }
            }
        }

        ///<summary>
        /// Gets or sets title of control
        ///</summary>
        [CanBeNull]
        public string Title
        {
            get { return Name; }
            set
            {
                Name = value;
            }
        }

        public event Action TitleChanged;

        /// <summary>
        /// Free up memory used by object
        /// </summary>
        public virtual void Dispose()
        {
            if (IsWinControlCreated)
            {
                Control.Dispose();
            }
            if (_ErrorControl != null)
            {
                _ErrorControl.Dispose();
            }
            if (Disposed != null)
            {
                Disposed();
            }
        }

        ///<summary>
        /// Raises when control has been disposed
        /// <seealso cref="Dispose"/>
        /// <seealso cref="IDisposable" />
        ///</summary>
        public event Action Disposed;

        ///<summary>
        /// Parses html file as sub-tree of this control
        ///</summary>
        ///<param name="htmlFilePath">Path to file should be parsed</param>
        ///<param name="onControlParsed">Delegate should be invoked for all control after parsing</param>
        public void ParseHtmlFile([NotNull]string htmlFilePath, [NotNull]Action<HtmlControl, HtmlControl> onControlParsed)
        {
            FFDebug.EnterMethod(Cattegory.HtmlControl, string.Format("HtmlFile:'{0}'", htmlFilePath));

            using (var f = new StreamReader(htmlFilePath))
            {
                ParseStream(f, onControlParsed);
            }

            FFDebug.LeaveMethod(Cattegory.HtmlControl, MethodBase.GetCurrentMethod());
        }

        ///<summary>
        ///
        ///</summary>
        ///<param name="source">Instance of <see cref="Stream"/> content of which should be parsed</param>
        ///<param name="onControlParsed">Delegate should be invoked for all control after parsing</param>
        public void ParseStream([NotNull]TextReader source, [NotNull]Action<HtmlControl, HtmlControl> onControlParsed)
        {
            FFDebug.EnterMethod(Cattegory.HtmlControl, "Parsing stream");

            var doc = new XmlDocument();
            doc.Load(source);
            Debug.Assert(doc.DocumentElement != null);
            Parse(doc.DocumentElement, onControlParsed);
            OnParsed();
            ReValidate();

            FFDebug.LeaveMethod(Cattegory.HtmlControl, "Parsing stream");
        }

        [CanBeNull]
        public override string ToString()
        {
            return Name;
        }

        [NotNull]
        protected abstract Control CreateWindowControl();

        protected virtual void ReCreateControl()
        {
            if (_Control != null)
            {
                _Control.Dispose();
            }
            _Control = CreateWindowControl();
            _Control.Tag = this;
            _Control.Disposed += (s, e) => ((IDisposable)((Control)s).Tag).Dispose();
            _Control.ControlAdded += (s, e) =>
            {
                var v = e.Control.Tag as IValidateble;
                if (v != null)
                {
                    v.ErrorFound += ChildControl_ErrorFound;
                    v.ErrorFixed += ChildControl_ErrorFixed;
                }
            };
            _Control.ControlRemoved += (s, e) =>
            {
                var v = e.Control.Tag as IValidateble;
                if (v != null)
                {
                    RemoveErrorsFrom(v);
                    v.ErrorFixed -= ChildControl_ErrorFixed;
                    v.ErrorFound -= ChildControl_ErrorFound;
                }
            };
        }

        private void RemoveErrorsFrom([NotNull]IValidateble source)
        {
            for (var i = _ErrorList.Count - 1; i >= 0; i--)
            {
                var error = _ErrorList[i];
                if (error.Source.Equals(source))
                {
                    _ErrorList.RemoveAt(i);
                    OnErrorFixed(error);
                }
            }
        }

        private void ChildControl_ErrorFixed(IValidateble source, Error error)
        {
            OnErrorFixed(error);
        }

        private void ChildControl_ErrorFound(IValidateble source, Error error)
        {
            OnErrorFound(error);
        }

        protected virtual void OnParsed()
        {
            if (Parsed != null)
            {
                Parsed();
            }
        }

        [NotNull]
        protected HtmlPageBase GetParentPage()
        {
            var cur = this;
            do
            {
                var page = cur as HtmlPageBase;
                if (page != null)
                {
                    return page;
                }
                cur = cur.Parent;
                if (cur == null)
                {
                    throw new FireFlyException("Cannot find page");
                }
            }
            while (true);
        }

        [CanBeNull]
        private HtmlControl GetControlForParse([NotNull]XmlNode node)
        {
            switch (node.Name)
            {
                case "input":
                    switch (node.Attributes["type"].Value)
                    {
                        case "text":
                            return new HtmlTextBox();

                        case "button":
                            return new HtmlButton();
                    }
                    break;

                case "select":
                    return new HtmlComboBox();

                case "textarea":
                    var id = node.Attributes["id"];
                    if (id == null || id.Value != "traceLog")
                    {
                        return new HtmlCompiledTest();
                    }
                    break;

                case "span":
                    var attribute = node.Attributes["name"];
                    return attribute != null && attribute.Value == "code" ? (HtmlControl)new HtmlHighlightedCode() : new HtmlLabel();

                case "div":
                    var at = node.Attributes["name"];
                    if (at != null)
                    {
                        var name = at.Value;
                        if (name == "advancedCompiledTest")
                        {
                          return new HtmlAdvancedCompiledTest();
                        }
                        if (name == "snippet")
                        {
                            return new HtmlCodeSnippet();
                        }
                        if (name.StartsWith("gen:"))
                        {
                            return new HtmlSimpleQuestion();
                        }
                    }
                    break;

                case "script":
                    var sc = node.InnerText;
                    if (sc.IsNotNull())
                    {
                        // TODO: This is incorrect to load scripts here. This method should don't have side effect 
                        foreach (Match im in HtmlTextBox.TextBoxInit.Matches(sc))
                        {
                            var c = (HtmlTextBox)Control.Controls[im.Groups["id"].Value].Tag;
                            c.EmptyText = im.Groups["emptyText"].Value;
                        }
                    }
                    break;
            }

            return null;
        }

        private void Parse([NotNull]XmlNode node, [NotNull] Action<HtmlControl, HtmlControl> onControlParsed)
        {
            var c = GetControlForParse(node);
            if (c != null)
            {
                c.Parent = this;
                c.Parse(node);
                c.OnParsed();
                onControlParsed(this, c);
            }
            else if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Parse(childNode, onControlParsed);
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Control _Control;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HtmlControl _Parent;
    }

    ///<summary>
    /// Represents objects that need JavaScript code to initialize in browser
    ///</summary>
    public interface IJavaScriptInitializable
    {
        ///<summary>
        /// Gets JavaScript code to initialize control
        ///</summary>
        ///<returns></returns>
        [CanBeNull]
        string GetJavaScriptInitializer();
    }

    ///<summary>
    /// Represents object which should be notified about delete/undelete operation made on them
    ///</summary>
    public interface IDeleteNotifiable
    {
        /// <summary>
        /// Notify object that it will be deleted soon
        /// </summary>
        void NotifyDelete();
        /// <summary>
        /// Notify object that it will be undeleted soon
        /// </summary>
        void NotifyUndoDelete();

        ///<summary>
        /// Raises before control is deleted
        ///</summary>
        event Action Deleting;
        /// <summary>
        /// Raises before control is undeleted
        /// </summary>
        event Action UnDeleting;
    }

    ///<summary>
    /// Helper class to treat strings from html and vice-versa.  
    ///</summary>
    public static class HtmlUtility
    {
        ///<summary>
        /// Encodes quotes of <see cref="text" /> to can be used in Html code
        ///</summary>
        ///<param name="text">Text to treat</param>
        ///<returns>Text with encoded quotes</returns>
        [CanBeNull]
        public static string QuotesEncode([CanBeNull]string text)
        {
            return !string.IsNullOrEmpty(text) ? text.Replace("\"", "\\\"").Replace("'", "\'") : text;
        }

        ///<summary>
        ///  Decoded quotes of <see cref="text"/> to can be used in design mode
        ///</summary>
        ///<param name="text">Text to decode</param>
        ///<returns>Decoded string</returns>
        [CanBeNull]
        public static string QuotesDecode([CanBeNull]string text)
        {
            return !string.IsNullOrEmpty(text) ? text.Replace("\\\"", "\"").Replace("\'", "'") : text;
        }
    }
}