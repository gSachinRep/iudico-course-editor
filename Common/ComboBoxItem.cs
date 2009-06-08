using FireFly.CourseEditor.Course.Manifest;

namespace FireFly.CourseEditor.Common
{
    internal class ComboBoxItem
    {
        public ComboBoxItem([NotNull]ITitled value)
        {
            Value = value;
        }

        [NotNull]
        public override string ToString()
        {
            return Value.Title;
        }

        [NotNull]
        public readonly ITitled Value;
    }
}
