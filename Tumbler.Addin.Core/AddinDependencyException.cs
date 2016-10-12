using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 当插件的依赖解析失败时引发该异常。
    /// </summary>
    public class AddinDependencyException : Exception
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinDependencyException 实例。
        /// </summary>
        /// <param name="unresolved">解析失败的依赖列表。</param>
        public AddinDependencyException(String[] unresolved)
        {
            Unresolved = unresolved;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 解析失败的依赖列表。
        /// </summary>
        public String[] Unresolved { get; }

        #endregion
    }
}
