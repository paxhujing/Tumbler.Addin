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
        #region Methods

        /// <summary>
        /// 停止服务。
        /// </summary>
        void Stop();

        #endregion
    }
}
