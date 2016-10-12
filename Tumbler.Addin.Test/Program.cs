using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly[] assems = AppDomain.CurrentDomain.GetAssemblies();
            String name = AssemblyName.GetAssemblyName(@".\bin\Debug\Tumbler.Addin.Test.exe")?.FullName;
            Assembly assem = assems.FirstOrDefault(x => x.FullName == name);
        }
    }
}
