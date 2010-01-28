using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HtmlWriter = System.Web.UI.HtmlTextWriter;
using HtmlWriterAttribute = System.Web.UI.HtmlTextWriterAttribute;
using HtmlWriterStyle = System.Web.UI.HtmlTextWriterStyle;
using HtmlWriterTag = System.Web.UI.HtmlTextWriterTag;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    using Course;
    using HighlightControl;
    using Common;
    using Course.Manifest;

    ///<summary>
    /// Base class for all pages
    ///</summary>
    [DebuggerDisplay("Page for '{PageItem}' item")]
    public abstract class HtmlPageBase : HtmlControl, ITitled
    {
        private static readonly Dictionary<string, HtmlPageBase> __Pages = new Dictionary<string, HtmlPageBase>();

        internal static void ReleasePages()
        {
            var pages = __Pages.Values.ToList(); // duplicate collection. page will remove itself from ms_Pages on disposing
            foreach (var p in pages)
            {
                p.Dispose();
            }
            __Pages.Clear();
        }

        internal static void StorePages()
        {
            foreach (var p in __Pages)
            {
                p.Value.Store();
            }
        }

        protected HtmlPageBase(ItemType pageItem)
        {
            SetPageItem(pageItem);
        }

        protected HtmlPageBase()
        {
        }

        ///<summary>
        /// Languages that was initialized in current page
        ///</summary>
        public HtmlHighlightedCode.LANGUAGE InitializedLanguages;

        ///<summary>
        /// Scripts will be included in result html page
        ///</summary>
        [ReadOnly(true)]
        [DisplayName("Included scripts")]
        [Description("Scripts will be included in result html page")]
        [Editor(Consts.STRING_EDITOR_TYPE_NAME, typeof(UITypeEditor))]
        [NotNull]
        public IncludesCollection Scripts
        {
            get
            {
                if (_Scripts == null)
                {
                    _Scripts = new IncludesCollection(__NeededScripts);
                }

                return _Scripts;
            }
            set { _Scripts = value; }
        }

        ///<summary>
        /// Styles will be included in result html page
        ///</summary>
        [ReadOnly(true)]
        [DisplayName("Included styles")]
        [Description("Styles will be included in result html page")]
        [Editor(Consts.STRING_EDITOR_TYPE_NAME, typeof(UITypeEditor))]
        [NotNull]
        public IncludesCollection Styles
        {
            get
            {
                if (_Styles == null)
                {
                    _Styles = new IncludesCollection();
                }
                return _Styles;
            }
            set { _Styles = value; }
        }

        ///<summary>
        /// Retrieves page by assigned item
        ///</summary>
        ///<param name="item"></param>
        ///<returns></returns>
        [CanBeNull]
        public static HtmlPageBase GetPage([NotNull]ItemType item)
        {
            HtmlPageBase p;
            return __Pages.TryGetValue(item.UID, out p) ? p : null;
        }

        ///<summary>
        /// Sets item to page
        ///</summary>
        ///<param name="item">item to set</param>
        public void SetPageItem([NotNull]ItemType item)
        {
            HtmlPageBase p;
            if (__Pages.TryGetValue(item.UID, out p) && p != this)
            {
                p.Dispose();
            }
            PageItem = item;
            __Pages[item.UID] = this;
            ReValidate();
        }

        /// <summary>
        /// Item of manifest this page assigned
        /// </summary>
        [Browsable(false)]
        [NotNull]
        public ItemType PageItem
        {
            get
            {
#if CHECKERS
                if (_PageItem == null)
                {
                    throw new FireFlyException("PageItem was not assigned");
                }
#endif
                return _PageItem;
            }
            protected set
            {
                if (_PageItem != null)
                {
                    throw new InvalidOperationException("PageItem is already assigned");
                }
                _PageItem = value;
                value.Disposed += Dispose;
            }
        }

        ///<summary>
        /// Retrieve has this page item or not
        ///</summary>
        [Browsable(false)]
        public bool HasPageItem
        {
            get
            {
                return _PageItem != null;
            }
        }

        ///<summary>
        /// Writes JavaScripts included in page
        ///</summary>
        ///<param name="w">Instance of <see cref="System.Web.UI.HtmlTextWriter"/> to write</param>
        public void WriteIncludedScripts([NotNull]HtmlWriter w)
        {
            foreach (var sc in Scripts)
            {
                w.AddAttribute(HtmlWriterAttribute.Src, sc);
                w.AddAttribute(HtmlWriterAttribute.Type, "text/javascript");
                w.RenderBeginTag(HtmlWriterTag.Script);
                w.RenderEndTag();
            }
        }

        ///<summary>
        /// Writes CSS styles included in page
        ///</summary>
        ///<param name="w">Instance of <see cref="System.Web.UI.HtmlTextWriter"/> to write</param>
        public void WriteIncludedStyles([NotNull]HtmlWriter w)
        {
            foreach (var c in Styles)
            {
                w.AddAttribute(HtmlWriterAttribute.Rel, "stylesheet");
                w.AddAttribute(HtmlWriterAttribute.Href, c);
                w.RenderBeginTag(HtmlWriterTag.Link);
                w.RenderEndTag();
            }
        }

        ///<summary>
        /// Writes inner html of page
        ///</summary>
        ///<param name="w">Instance of <see cref="System.Web.UI.HtmlTextWriter"/> to write</param>
        public virtual void WriteInnerHtml([NotNull]HtmlWriter w)
        {
            Course.EnsureCourseContainsResources(Course.NAMESPACE, string.Empty, __NeededScripts);
            Course.EnsureCourseContainsResources(Course.NAMESPACE, string.Empty, __NeededFiles);
            Debug.Assert(IsWinControlCreated);
            foreach (Control c in Control.Controls)
            {
                if (c.Tag is HtmlControl)
                {
                    ((HtmlControl)c.Tag).WriteHtml(w);
                }
            }
        }

        ///<summary>
        ///  Writes trace log element
        ///</summary>
        ///<param name="w">Instance of <see cref="System.Web.UI.HtmlTextWriter"/> to write</param>
        public static void WriteTraceLogElement([NotNull]HtmlWriter w)
        {
            w.AddAttribute(HtmlWriterAttribute.Id, "traceLog");
            w.AddAttribute(HtmlWriterAttribute.ReadOnly, "readonly");
            w.AddStyleAttribute(HtmlWriterStyle.Width, "100%");
            w.AddStyleAttribute(HtmlWriterStyle.Height, "100");
            w.AddStyleAttribute(HtmlWriterStyle.Display, "none");
            w.AddStyleAttribute("bottom", "0%");
            w.AddStyleAttribute(HtmlWriterStyle.Left, "0px");
            w.AddStyleAttribute(HtmlWriterStyle.Position, "absolute");
            w.RenderBeginTag(HtmlWriterTag.Textarea);
            w.RenderEndTag();
        }

        [NotNull]
        protected override Control CreateWindowControl()
        {
            var res = new ScrollableControl { AutoScroll = true };
            ControlEventHandler handler = (s, e) =>
            {
                if (!(e.Control is ErrorControl))
                {
                    ReValidate();
                }
            };
            res.ControlAdded += handler;
            res.ControlRemoved += handler;
            return res;
        }

        [CanBeNull]
        protected override ErrorControl ErrorControl
        {
            get
            {
                return null;
            }
        }

        ///<summary>
        /// Gets or sets title of page
        ///</summary>
        [CanBeNull]
        new public string Title
        {
            get
            {
                return PageItem.Title;
            }
            set
            {
                PageItem.Title = value;
            }
        }

        ///<summary>
        /// Collection of included files such as css scripts or JavaScript files
        ///</summary>
        public class IncludesCollection : List<string>
        {
            ///<summary>
            /// Creates empty collection
            ///</summary>
            public IncludesCollection()
            {
            }

            /// <summary>
            /// Creates new instance of <see cref="IncludesCollection" /> based on <see cref="collection" />
            /// </summary>
            /// <param name="collection"></param>
            public IncludesCollection([NotNull]IEnumerable<string> collection)
                : base(collection)
            {
            }

            ///<summary>
            /// Include all elements specified in <see cref="items" />
            ///</summary>
            ///<param name="items"></param>
            public void IncludeAll(params string[] items)
            {
                foreach (var item in items)
                {
                    if (!Contains(item))
                    {
                        Add(item);
                    }
                }
            }

            ///<summary>
            /// Include all elements specified in <see cref="items" /> related to <see cref="root" />
            ///</summary>
            ///<param name="root"></param>
            ///<param name="items"></param>
            public void IncludeAllWithRoot([NotNull]string root, params string[] items)
            {
                foreach (var s in items)
                {
                    var elem = Path.Combine(root, s);
                    if (!Contains(elem))
                    {
                        Add(elem);
                    }
                }
            }

            ///<summary>
            /// Exclude all elements specified in <see cref="items" /> 
            ///</summary>
            ///<param name="items"></param>
            public void RemoveAll(params string[] items)
            {
                foreach (var s in items)
                {
                    Remove(s);
                }
            }

            ///<summary>
            /// Exclude all elements specified in <see cref="items" /> related to <see cref="root" />
            ///</summary>
            ///<param name="root"></param>
            ///<param name="items"></param>
            public void RemoveAllWithRoot([NotNull]string root, params string[] items)
            {
                foreach (var s in items)
                {
                    Remove(Path.Combine(root, s));
                }
            }
        }

        public static readonly string[] __NeededScripts = { /*"help.js",*/  "LMSDebugger.js", "LMSIntf.js", "jquery.min.extended.js", "SCOObj.js"};
        public static readonly string[] __NeededFiles = { "expressInstall.swf", "flXHR.swf", "flXHR.js", "flensed.js", "checkplayer.js", "swfobject.js" };
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IncludesCollection _Scripts, _Styles;

        private ItemType _PageItem;

        ///<summary>
        /// Free up resources and remove page from singleton registry
        ///</summary>
        public override void Dispose()
        {
            base.Dispose();
            if (IsWinControlCreated)
            {
                ClearControls();
            }
            if (HasPageItem)
            {
                __Pages.Remove(PageItem.UID);
            }
        }

        protected static void RemoveControl([NotNull]Control c)
        {
            var hc = (IDisposable)c.Tag;
            Debug.Assert(hc != null);
            var deleteNotifiable = hc as IDeleteNotifiable;
            if (deleteNotifiable != null)
            {
                deleteNotifiable.NotifyDelete();
            }
            hc.Dispose();
        }

        ///<summary>
        /// Remove all controls from page
        ///</summary>
        public void ClearControls()
        {
            foreach (Control c in Control.Controls)
            {
                if (!(c is ErrorControl || c is BoundControl))
                {
                    RemoveControl(c);
                }
            }
        }

        ///<summary>
        /// When overloads in derived classes stores content of page to html file
        ///</summary>
        public abstract void Store();
    }
}