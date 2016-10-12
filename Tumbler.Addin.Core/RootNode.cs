using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 框架提供的工作控件对应的节点。
    /// </summary>
    internal class RootNode : VirtualNode
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.RootAddinTreeNode 实例。
        /// </summary>
        public RootNode()
            : base(String.Empty, WorkspaceId)
        {
            InnerChildren.Add(new VirtualNode(WorkspaceId, "Addins"));
            InnerChildren.Add(new VirtualNode(WorkspaceId, "Services"));
        }

        #endregion
    }
}
