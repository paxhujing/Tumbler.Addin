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
        /// <param name="mountTo">要挂载到的节点。</param>
        /// <param name="mountPoint">要挂载到的节点中的挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="exposes">向外提供的挂载点。</param>
        internal VirtualNode(String mountTo, String mountPoint, String id, String[] exposes)
            : base(mountTo, mountPoint, id, exposes)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取一个标识，表示该节点是一个插件节点还是路径节点。
        /// </summary>
        public override Boolean IsVitual
        {
            get { return true; }
        }

        #endregion
    }
}
