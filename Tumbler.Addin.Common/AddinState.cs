using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 表示插件当前的状态。
    /// </summary>
    public enum AddinState
    {
        /// <summary>
        /// 未知状态。
        /// </summary>
        Unknow,
        /// <summary>
        /// 启用。
        /// </summary>
        Enable,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable,
        /// <summary>
        /// 不可见或不可执行或服务停止。
        /// </summary>
        ExcludeOrStop,
        /// <summary>
        /// 可见或可执行或服务启动。
        /// </summary>
        IncludeOrRuning,
    }
}
