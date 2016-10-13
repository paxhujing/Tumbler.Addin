using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件的构建状态状态。
    /// </summary>
    internal enum AddinBuildState
    {
        /// <summary>
        /// 未构建插件的实例。
        /// </summary>
        None,
        /// <summary>
        /// 构建失败。
        /// </summary>
        BuildFail,
        /// <summary>
        /// 已构建插件的实例。
        /// </summary>
        Build,
    }
}
