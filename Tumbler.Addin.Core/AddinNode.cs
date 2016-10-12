using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 描述插件树中的一个插件节点。
    /// </summary>
    public class AddinNode : AddinTreeNode
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinNode 实例。
        /// </summary>
        /// <param name="path">挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="configFile">插件配置文件。</param>
        public AddinNode(String path, String id, String configFile)
            : base(path, id)
        {
            if (String.IsNullOrWhiteSpace(configFile))
            {
                throw new ArgumentNullException("configFile");
            }
            ConfigFile = configFile;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取插件配置文件路径。
        /// </summary>
        public String ConfigFile { get; set; }

        /// <summary>
        /// 获取一个标识，表明该节点是否是一个虚拟节点而不是实际的插件节点。
        /// </summary>
        public override bool IsVirtual { get; } = false;

        #endregion
    }
}
