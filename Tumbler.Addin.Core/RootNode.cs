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
        #region Fields

        public const String AddinsExpose = "Addins";

        public const String ServicesExpose = "Services";

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.RootAddinTreeNode 实例。
        /// </summary>
        public RootNode()
            : base(String.Empty, null, WorkspaceId)
        {
            SetChild(new VirtualNode(this.FullPath, DefaultExposePoint, AddinsExpose));
            SetChild(new VirtualNode(this.FullPath, DefaultExposePoint, ServicesExpose));
        }

        #endregion
    }
}
