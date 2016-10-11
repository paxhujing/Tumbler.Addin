using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 描述一个插件。
    /// </summary>
    public sealed class AddinDescriptor
    {
        #region Constructors

        public AddinDescriptor(String[] references, String[] depedencies)
        {
            References = references;
            Dependencies = depedencies;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取插件需要引用的程序集列表。
        /// </summary>
        public String[] References { get; }

        /// <summary>
        /// 获取插件依赖的其它插件列表。
        /// </summary>
        public String[] Dependencies { get; }

        #region AddinState

        /// <summary>
        /// 获取插件的状态。
        /// </summary>
        public AddinState State
        {
            get;
            internal set;
        }

        #endregion

        #endregion
    }
}
