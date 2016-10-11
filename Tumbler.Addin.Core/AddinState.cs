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
        /// 卸载。
        /// </summary>
        Uninstall,
        /// <summary>
        /// 无效的挂载点。
        /// </summary>
        InvalidMountPoint,
        /// <summary>
        /// 加载程序集失败。
        /// </summary>
        LoadAssemblyFail,
        /// <summary>
        /// 加载类型失败。
        /// </summary>
        LoadTypeFail,
        /// <summary>
        /// 安装。
        /// </summary>
        Install,
        /// <summary>
        /// 依赖解析失败。
        /// </summary>
        DependencyFail,
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
