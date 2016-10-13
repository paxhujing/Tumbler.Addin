using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Core;
using static System.Diagnostics.Debug;

namespace Tumbler.Addin.CoreTests
{
    public class ToolbarAddin : IAddin
    {
        public void Dispose()
        {
            WriteLine("Destroy ToolbarAddin");
        }

        public void Execute()
        {
            WriteLine("Execute ToolbarAddin");
        }

        public void Initialize(AddinManager manager)
        {
            WriteLine("Initialize ToolbarAddin");
        }

        public void OnDependencyStateChanged(String fullPath, AddinState? state)
        {
            WriteLine($"ToolbarAddin Depden on {fullPath}--[{state}]");
        }
    }
}
