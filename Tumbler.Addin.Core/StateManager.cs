using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Common;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 宿主管理器。
    /// </summary>
    public class StateManager
    {
        #region Fields

        private readonly IStateSupported _target;

        private readonly Dictionary<String, WeakReference> _states = new Dictionary<String, WeakReference>();

        private String _currentState;

        private readonly Object _syncObj = new Object();

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.StateManager 实例。
        /// </summary>
        /// <param name="target">IStateSupported 类型实例。</param>
        public StateManager(IStateSupported target)
        {
            _target = target;
            RegisterState(target.SupportedStates);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 已注册的所有状态。
        /// </summary>
        public IEnumerable<String> States => _target.SupportedStates;

        /// <summary>
        /// 当前状态。如果为null，表示当前不处于任何状态。
        /// </summary>
        public String CurrentState => _currentState;

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 转到状态。
        /// </summary>
        /// <param name="state">目标状态。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        public Boolean GotoState(String state)
        {
            if (_currentState == state) return true;
            lock (_syncObj)
            {
                Boolean result = true;
                if (_currentState != null)
                {
                    result = _target.LeaveState(state);
                }
                if (result)
                {
                    result = _target.EnterState(state);
                }
                if (result)
                {
                    _currentState = state;
                }
                return result;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 注册状态。
        /// </summary>
        /// <param name="states">状态列表。</param>
        private void RegisterState(String[] states)
        {
            foreach (String state in states)
            {
                _states.Add(state, new WeakReference(null));
            }
        }

        #endregion

        #endregion
    }
}
