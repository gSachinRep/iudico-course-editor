using System;
using System.ComponentModel;

namespace FireFly.CourseEditor.GUI
{
    using Course.Manifest;
    using HtmlEditor;

    public static class NodesWrappers
    {
        public static ITitled Wrap(ITitled node)
        {
            if (node != null)
            {
                var item = node as ItemType;
                if (item != null && item.PageType == PageType.Question)
                {
                    return ExaminationWrapper.Wrap(item);
                }
                return Wraper<ITitled>.Wrap(node);
            }
            return null;
        }

        public class Wraper<T> : ITitled
            where T : ITitled
        {
            [Browsable(false)]
            public T WrappedNode { get; set; }

            [TypeConverter(typeof (ExpandableObjectConverter))]
            [DisplayName("Additional Options")]
            public T AdditionalOptions
            {
                get { return WrappedNode; }
            }

            public string Title
            {
                get { return WrappedNode.Title; }
                set { WrappedNode.Title = value; }
            }

            public event Action TitleChanged
            {
                add
                {
                    WrappedNode.TitleChanged += value;
                }
                remove
                {
                    WrappedNode.TitleChanged -= value;
                }
            }

            public static Wraper<T> Wrap(T node)
            {
                if (_Wrapper == null)
                {
                    _Wrapper = new Wraper<T>();
                }
                _Wrapper.WrappedNode = node;
                return _Wrapper;
            }

            protected static Wraper<T> _Wrapper;
        }

        public class ExaminationWrapper : Wraper<ItemType>
        {
            new public static Wraper<ItemType> Wrap(ItemType item)
            {
                if (_Wrapper == null)
                {
                    _Wrapper = new ExaminationWrapper();
                }
                _Wrapper.WrappedNode = item;
                return _Wrapper;
            }

            public int? PassRank
            {
                get
                {
                    return HtmlPage.GetPage(WrappedNode).PassRank;
                }
                set
                {
                    HtmlPage.GetPage(WrappedNode).PassRank = value;
                }
            }
        }
    }
}