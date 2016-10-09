using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件树。
    /// </summary>
    public interface IAddinTree
    {

        void InsertAddin();

        void RemoveAddin();
    }
}
