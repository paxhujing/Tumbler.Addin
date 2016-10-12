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
        /// <param name="label">插件标签。</param>
        void Initialize(String label);

        /// <summary>
        /// 执行插件提供的功能。
        /// </summary>
        void Execute();
    }
}
