using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyVersion("1.1.52.118")]
[assembly: AssemblyFileVersion("1.1.52.18")]

[assembly : AssemblyTitle("FireFly Course Editor")]
[assembly : AssemblyDescription("")]
[assembly : AssemblyConfiguration("")]
[assembly : AssemblyCompany("")]
[assembly : AssemblyProduct("CourseEditor")]
[assembly : AssemblyCopyright("")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]

[assembly : ComVisible(false)]
[assembly : Guid("58506e44-d32a-44b5-9a1f-b14ffae63862")]
                                      
[assembly : FileDialogPermission(SecurityAction.RequestMinimum)]
[assembly : UIPermission(SecurityAction.RequestMinimum)]
[assembly : EnvironmentPermission(SecurityAction.RequestMinimum)]
[assembly : ReflectionPermission(SecurityAction.RequestMinimum, ReflectionEmit=false, MemberAccess = true)]
[assembly : RegistryPermission(SecurityAction.RequestRefuse)]