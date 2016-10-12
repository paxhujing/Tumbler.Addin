using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 当加载插件引用的程序集失败时引发的异常。
    /// </summary>
    public class AddinAssembliesException : Exception
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinAssembliesException 实例。
        /// </summary>
        /// <param name="unresolved">加载失败的程序集列表。</param>
        public AddinAssembliesException(String[] unresolved)
        {
            Unresolved = unresolved;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 加载失败的程序集列表。
        /// </summary>
        public String[] Unresolved { get; }

        #endregion
    }
}
