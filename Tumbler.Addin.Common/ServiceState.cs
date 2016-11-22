using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 表示服务的当前状态。
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// 已停止。
        /// </summary>
        Stopped,
        /// <summary>
        /// 运行中。
        /// </summary>
        Runing,
    }
}
