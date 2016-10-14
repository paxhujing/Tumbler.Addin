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
        /// <param name="owner">拥有此插件树节点的管理器。</param>
        internal VirtualNode(String path, String id, AddinManager owner)
            : base(path, id, owner)
        {

        }

        #endregion
    }
}
