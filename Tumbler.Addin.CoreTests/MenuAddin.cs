using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Core;
using static System.Diagnostics.Debug;

namespace Tumbler.Addin.CoreTests
{
    public class MenuAddin : IAddin
    {
        public void Dispose()
        {
            WriteLine("Destroy MenuAddin");
        }

        public void Execute()
        {
            WriteLine("Execute MenuAddin");
        }

        public void Initialize(AddinManager manager)
        {
            WriteLine("Initialize MenuAddin");
        }

        public void OnDependencyStateChanged(String fullPath, AddinState state)
        {
            WriteLine($"MenuAddin Depden on {fullPath}--[{state}]");
        }
    }
}
