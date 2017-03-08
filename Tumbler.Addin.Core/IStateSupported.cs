using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 表示被状态管理器管理器状态的对象。
    /// </summary>
    public interface IStateSupported
    {
        /// <summary>
        /// 支持的状态列表。
        /// </summary>
        String[] SupportedStates { get; }

        /// <summary>
        /// 状态管理器。
        /// </summary>
        StateManager StateManager { get; }

        /// <summary>
        /// 离开指定的状态。
        /// </summary>
        /// <param name="state">状态。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        Boolean LeaveState(String state);

        /// <summary>
        /// 进入指定的状态。
        /// </summary>
        /// <param name="state">状态。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        Boolean EnterState(String state);
    }
}
