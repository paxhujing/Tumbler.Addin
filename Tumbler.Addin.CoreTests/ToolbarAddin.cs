﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Core;
using static System.Diagnostics.Debug;

namespace Tumbler.Addin.CoreTests
{
    public class ToolbarAddin : IAddin , IHandler
    {
        public void Dispose()
        {
            WriteLine("Destroy ToolbarAddin");
        }

        public void Execute()
        {
            WriteLine("Execute ToolbarAddin");
        }

        public void Handle(Hashtable message)
        {
            WriteLine("ToolbarAddin Handle message");
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
