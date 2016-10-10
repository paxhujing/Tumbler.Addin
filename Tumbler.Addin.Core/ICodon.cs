﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    public interface ICodon
    {
        IAddin Addin { get; }

        String Class { get; }

        String Id { get; }
    }
}
