namespace FireFly.CourseEditor.Course.Manifest
{
    using System.ComponentModel;

    internal class ManifestStringConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ManifestNodeList<ResourceType> resourceList = Course.Manifest.resources.Resources;
            var resourceListString = new string[resourceList.Count + 1];
            resourceListString[0] = string.Empty;

            for (int i = 1; i < resourceListString.Length; i++)
            {
                resourceListString[i] = resourceList[i - 1].ToString();
            }

            return new StandardValuesCollection(resourceListString);
        }
    }
}