using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fthdgn.SquirrelTrials.UI
{
    public static class AssemblyInfoProvider
    {
        static Assembly currentAssembly;
        public static Assembly CurrentAssembly() => currentAssembly ?? (currentAssembly = Assembly.GetExecutingAssembly());
        public static TCustomAttribute AssemblyAttribute<TCustomAttribute>()
            where TCustomAttribute : Attribute
            => CurrentAssembly().GetCustomAttribute<TCustomAttribute>();
        public static string Title => AssemblyAttribute<AssemblyTitleAttribute>()?.Title;
        public static string Version => AssemblyAttribute<AssemblyFileVersionAttribute>().Version;
    }
}
