﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Common;
using Tumbler.Addin.Core;
using static System.Diagnostics.Debug;

namespace Tumbler.Addin.CoreTests
{
    public class ToolbarAddin : IAddin , IHandler
    {
        public string MountExpose { get; private set; }

        public void Dispose()
        {
            WriteLine("Destroy ToolbarAddin");
        }

        public void Execute()
        {
            WriteLine("Execute ToolbarAddin");
        }

        public void Handle(Object message)
        {
            WriteLine("ToolbarAddin Handle message");
        }

        public void Initialize(String mountExpose, String[] exposes)
        {
            MountExpose = mountExpose;
            WriteLine("Initialize ToolbarAddin");
        }

        public void OnDependencyStateChanged(String fullPath, AddinState state)
        {
            WriteLine($"ToolbarAddin Depden on {fullPath}--[{state}]");
        }
    }
}
