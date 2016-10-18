﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Common;
using Tumbler.Addin.Core;
using static System.Diagnostics.Debug;

namespace Tumbler.Addin.CoreTests
{
    public class MenuAddin : IAddin
    {
        public string MountExpose { get; private set; }

        public void Dispose()
        {
            WriteLine("Destroy MenuAddin");
        }

        public void Execute()
        {
            WriteLine("Execute MenuAddin");
        }

        public void Initialize(String mountExpose, String[] exposes)
        {
            MountExpose = mountExpose;
            WriteLine("Initialize MenuAddin");
        }

        public void OnDependencyStateChanged(String fullPath, AddinState state)
        {
            WriteLine($"MenuAddin Depend on {fullPath}--[{state}]");
        }
    }
}
