using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 表示一个插件。
    /// </summary>
    public interface IAddin
    {
        String Id { get; }

        Type Type { get; }

        Object Build();
    }
}
