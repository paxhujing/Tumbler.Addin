using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 表示一个服务。
    /// </summary>
    public interface IService : IAddin
    {
        #region Properties

        /// <summary>
        /// 获取服务的状态。
        /// </summary>
        ServiceState State { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 启动服务。
        /// </summary>
        void Launch(params Object[] args);

        /// <summary>
        /// 停止服务。
        /// </summary>
        void Stop();

        #endregion
    }
}
