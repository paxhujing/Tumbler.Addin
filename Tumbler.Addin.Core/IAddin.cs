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
    public interface IAddin : IDisposable
    {
        /// <summary>
        /// 初始化插件。
        /// </summary>
        /// <param name="manager">插件管理器。</param>
        void Initialize(AddinManager manager);

        /// <summary>
        /// 执行插件提供的功能。
        /// </summary>
        void Execute();

        /// <summary>
        /// 依赖的插件状态改变时执行。
        /// </summary>
        /// <param name="fullPath">依赖的插件的完整路径。</param>
        /// <param name="state">改变后的状态。</param>
        void OnDependencyStateChanged(String fullPath,AddinState? state);
    }
}
