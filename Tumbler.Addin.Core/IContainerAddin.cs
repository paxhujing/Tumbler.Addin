using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 表示一个可以容纳其它插件的插件。
    /// </summary>
    public interface IContainerAddin : IAddin
    {
        /// <summary>
        /// 添加插件。
        /// </summary>
        /// <param name="child">插件。</param>
        void Add(IAddin child);

        /// <summary>
        /// 移除插件。
        /// </summary>
        /// <param name="child">插件。</param>
        void Remove(IAddin child);
    }
}
