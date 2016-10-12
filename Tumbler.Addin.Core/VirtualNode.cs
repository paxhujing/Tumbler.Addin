using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 描述插件树中的一个虚拟节点。
    /// </summary>
    public class VirtualNode : AddinTreeNode
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.VirtualNode 实例。
        /// </summary>
        /// <param name="path">挂载点。</param>
        /// <param name="id">插件Id。</param>
        public VirtualNode(String path, String id)
            : base(path, id)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取一个标识，表明该节点是否是一个虚拟节点而不是实际的插件节点。
        /// </summary>
        public override bool IsVirtual { get; } = true;

        #endregion
    }
}
