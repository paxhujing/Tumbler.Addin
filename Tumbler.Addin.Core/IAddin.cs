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
        /// <summary>
        /// 初始化插件。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 执行插件提供的功能。
        /// </summary>
        void Execute();
    }
}
