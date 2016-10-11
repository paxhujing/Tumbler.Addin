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
    internal class RootAddinTreeNode : AddinTreeNode
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.RootAddinTreeNode 实例。
        /// </summary>
        public RootAddinTreeNode()
            : base(null, WorkspaceNodeId)
        {
            Children.Add(new AddinTreeNode(WorkspaceNodeId, "Addins"));
            Children.Add(new AddinTreeNode(WorkspaceNodeId, "Services"));
        }

        #endregion
    }
}
