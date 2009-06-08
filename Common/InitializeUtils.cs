using System;
using System.Diagnostics;
using System.Reflection;

namespace FireFly.CourseEditor.Common
{
    ///<summary>
    /// Marks type need to be initialized (via static method Initialize) before using
    ///</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class StartupInitializableAttribute : Attribute
    {
        ///<summary>
        /// Generic arguments for type
        /// Can be used if attribute applied to generic type with single generic argument only
        ///</summary>
        public Type[] GenericArguments { get; private set; }

        ///<summary>
        /// Marks non-generic type to be initialized
        ///</summary>
        public StartupInitializableAttribute()
        {
        }

        /// <summary>
        /// Marks generic type with single generic argument to be initialized for all of <see cref="genericArguments" />
        /// </summary>
        /// <param name="genericArguments">Generic arguments for type to initializing</param>
        public StartupInitializableAttribute(params Type[] genericArguments)
        {
            GenericArguments = genericArguments;
        }
    }

    internal static class InitializeUtils
    {
        private const string INITIALIZE_METHOD_NAME = "Initialize";

        public static void Initialize(Assembly assembly)
        {
#if LOGGER
            using (Logger.Scope(string.Format("Initialize types for {0} assembly", assembly.GetName().Name)))
            {
#endif
                foreach (var t in assembly.GetTypes())
                {
                    StartupInitializableAttribute at;
                    if (t.TryGetCustomAttribute(out at))
                    {
                        if (t.IsGenericType)
                        {
                            Debug.Assert(at.GenericArguments != null && at.GenericArguments.Length > 0);
                            foreach (var genericArgument in at.GenericArguments)
                            {
                                t.MakeGenericType(new[] {genericArgument}).GetMethod(INITIALIZE_METHOD_NAME).Invoke(null, Type.EmptyTypes);
                            }
                        }
                        else
                        {
                            Debug.Assert(at.GenericArguments == null);
                            t.GetMethod(INITIALIZE_METHOD_NAME).Invoke(null, Type.EmptyTypes);
                        }
                    }
                }

#if LOGGER
            }
#endif
        }
    }
}
