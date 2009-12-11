using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using FireFly.CourseEditor.Course;
using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.GUI.HtmlEditor
{
    public class TestCasesEditor : CollectionEditor
    {
        public TestCasesEditor(Type type)
            : base(type)
        {
            if (type != typeof(List<CompiledTestCase>))
            {
                throw new NotSupportedException();
            }
        }

        protected override string GetDisplayText(object value)
        {
            var c = (CompiledTestCase) value;
            return GetShortString(c.Input) + " -> " + GetShortString(c.Output);
        }

        private static string GetShortString(string value)
        {
            if (value.IsNull())
            {
                return "(none)";
            }
            const int maxSize = 10;
            return value.Length < maxSize+1 ? value : value.Remove(maxSize);
        }

        protected override object CreateInstance(Type itemType)
        {
            if (itemType != typeof(CompiledTestCase))
            {
                throw new NotSupportedException();
            }
            return new CompiledTestCase();
        }

        protected override object[] GetItems(object editValue)
        {
            return ((List<CompiledTestCase>) editValue).ToArray();
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof (CompiledTestCase);
        }                                                                                                                              

        protected override Type[] CreateNewItemTypes()
        {
            return new [] {typeof (CompiledTestCase)};
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override IList GetObjectsFromInstance(object instance)
        {
            return new List<CompiledTestCase> { (CompiledTestCase) instance };
        }
    }
}
