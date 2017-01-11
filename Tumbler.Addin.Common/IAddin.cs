using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 表示一个插件。
    /// </summary>
    public interface IAddin : IDisposable
    {
        #region Properties

        /// <summary>
        /// 获取插件状态。
        /// </summary>
        AddinState State { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化插件。
        /// </summary>
        void Initialize();

        /// <summary>
        /// 执行插件提供的功能。
        /// </summary>
        void Execute(params Object[] args);

        /// <summary>
        /// 依赖的插件状态改变时执行。
        /// </summary>
        /// <param name="fullPath">依赖的插件的完整路径。</param>
        /// <param name="state">改变后的状态。</param>
        void OnDependencyStateChanged(String fullPath, AddinState state);

        /// <summary>
        /// 数据改变时执行。
        /// </summary>
        /// <param name="fullPath">依赖的插件的完整路径。</param>
        /// <param name="newData">新数据。</param>
        /// <param name="oldData">旧数据。</param>
        void OnDataChanged(String fullPath, Object newData, Object oldData);

        #endregion
    }
}
