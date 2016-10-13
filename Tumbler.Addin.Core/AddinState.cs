using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 表示插件当前的状态。
    /// </summary>
    public enum AddinState
    {
        /// <summary>
        /// 未构建插件的实例。
        /// </summary>
        None,
        /// <summary>
        /// 已构建插件的实例。
        /// </summary>
        Build,
        /// <summary>
        /// 启用。
        /// </summary>
        Enable,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable,
    }
}
