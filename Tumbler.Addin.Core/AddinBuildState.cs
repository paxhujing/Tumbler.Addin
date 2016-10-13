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
        /// 未解析。
        /// </summary>
        NotAnalysis,
        /// <summary>
        /// 依赖解析失败。
        /// </summary>
        DependecyFail,
        /// <summary>
        /// 加载程序集失败。
        /// </summary>
        LoadAssemblyFail,
        /// <summary>
        /// 加载类型失败。
        /// </summary>
        LoadTypeFail,
        /// <summary>
        /// 未构建插件的实例。
        /// </summary>
        NotBuild,
        /// <summary>
        /// 已构建插件的实例。
        /// </summary>
        Build,
    }
}
