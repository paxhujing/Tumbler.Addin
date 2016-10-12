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
        /// 未加载。
        /// </summary>
        Unload,
        /// <summary>
        /// 已加载。
        /// </summary>
        Loaded,
        /// <summary>
        /// 启用。
        /// </summary>
        Enable,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable,
        /// <summary>
        /// 更新。
        /// </summary>
        Update
    }
}
