using FireFly.CourseEditor.Common;

namespace FireFly.CourseEditor.Course
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    internal static class CourseUpgradeManager
    {
        static CourseUpgradeManager()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var t in types)
            {
                if (t.BaseType == typeof(CourseUpgraderBase))
                    if (t.HasCustomAttribute<CourseUpgratorAttribute>())
                    {
                        __Upgraders.Add(t.Create<CourseUpgraderBase>());
                    }
            }
            __Upgraders.Sort();

            for (var i = 0; i < __Upgraders.Count; i++)
            {
                Debug.Assert(__Upgraders[i].TargetManifestVersion == i);
            }
        }

        public static bool UpgradeCourse(out string messages)
        {
            var mess = new StringBuilder();
            var res = true;

            foreach (var u in __Upgraders)
            {
                if (Course.Manifest.version <= u.TargetManifestVersion)
                    if (!u.PerformUpgrade(mess))
                    {
                        res = false;
                        break;
                    }
            }

            messages = mess.ToString();
            return res;
        }

        public static int LastVersion { get { return __Upgraders.Count; } }

        private static readonly List<CourseUpgraderBase> __Upgraders = new List<CourseUpgraderBase>();

    }
}
