using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.Course
{
    using System;
    using System.Text;

    /// <summary>
    /// Base abstract class to implement custom logic to upgrade course.
    /// </summary>
    internal abstract class CourseUpgraderBase : IComparable<CourseUpgraderBase>
    {
        /// <summary>
        /// Version of course can be upgraded
        /// </summary>
        public abstract int TargetManifestVersion { get; }

        /// <summary>
        /// Performs upgrade procedure on course from Target version to next one
        /// </summary>
        public bool PerformUpgrade(StringBuilder messages)
        {
            if (!((Course.State & CourseStates.Opened) > 0 && ((Course.State & (CourseStates.Opening | CourseStates.Saving)) == 0)))
            {
                throw new FireFlyException("Unsuported state of Course: {0}, upgrading failed", Course.State);
            }
            if (Course.Manifest.version != TargetManifestVersion)
            {
                throw new FireFlyException("Manifest version {0} expected, but {1} found", TargetManifestVersion, Course.Manifest.version);
            }
            try
            {
                if (Upgrade(messages))
                {
                    Course.Manifest.version++;
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                messages.AppendLine(e.Message);
                return false;
            }
        }

        public int CompareTo(CourseUpgraderBase other)
        {
            return TargetManifestVersion.CompareTo(other);
        }

        /// <summary>
        /// When overrided in inherited classes performs upgrading actions
        /// </summary>
        protected abstract bool Upgrade(StringBuilder messages);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited = false)]
    [BaseTypeRequired(typeof(CourseUpgraderBase))]
    internal class CourseUpgratorAttribute : Attribute
    {
    }
}
