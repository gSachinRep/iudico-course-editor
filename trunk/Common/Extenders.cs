using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Windows.Forms;
using FireFly.CourseEditor.Course.Manifest;

namespace FireFly.CourseEditor.Common
{
    ///<summary>
    /// Extensions method for all Project's classes
    ///</summary>
    public static class Extenders
    {
        public static string HttpEncode([CanBeNull]this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        public static string HttpDecode([CanBeNull]this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        public static bool IsEqual([CanBeNull]this string s1, [CanBeNull]string s2)
        {
            return s1 == s2 || (s1.IsNull() && s2.IsNull());
        }

        public static bool IsNull([CanBeNull]this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNotNull([CanBeNull]this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static string IsNull([CanBeNull]this string @string, string ifNull)
        {
            return string.IsNullOrEmpty(@string) ? ifNull : @string;
        }

        public static bool Exists<T>([NotNull]this IEnumerable<T> collection, [NotNull]Predicate<T> condition)
        {
            foreach (var t in collection)
            {
                if (condition(t))
                {
                    return true;
                }
            }
            return false;
        }

        public static T Create<T>([NotNull]this Type t)
        {
            return (T) t.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
        }

        public static void AddTitledItem([NotNull]this ComboBox.ObjectCollection items, [NotNull]ITitled value)
        {
            items.Add(new ComboBoxItem(value));
        }

        public static bool ConfirmDelete([NotNull]string name)
        {
            return MessageBox.Show(string.Format("Are you sure to delete '{0}'?", name), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        [NotNull]
        public static T GetCustomAttribute<T>([NotNull]this ICustomAttributeProvider ap)
            where T : Attribute
        {
            T result;
            if (!ap.TryGetCustomAttribute(out result))
            {
                throw new InvalidOperationException("Invalid count of applied attributes");
            }
            return result;
        }

        public static bool TryGetCustomAttribute<T>([NotNull] this ICustomAttributeProvider ap, [NotNull]out T result)
            where T: Attribute
        {
            object[] ats = ap.GetCustomAttributes(typeof(T), true);
            if (ats.Length == 1)
            {
                result = (T)ats[0];
                return true;
            }
            result = null;
            return false;
        }

        [NotNull]
        public static T[] GetCustomAttributes<T>([NotNull]this ICustomAttributeProvider ap)
            where T: Attribute
        {
            return (T[]) ap.GetCustomAttributes(typeof (T), true);
        }

        public static bool HasCustomAttribute<T>([NotNull]this ICustomAttributeProvider ap)
            where T : Attribute
        {
            T fake;
            return ap.TryGetCustomAttribute(out fake);
            
        }
    }
}
