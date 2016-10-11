using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件树节点。
    /// </summary>
    internal class AddinTreeNode
    {
        #region Fields

        /// <summary>
        /// 工作空间的路径。
        /// </summary>
        public const String WorkspaceNodeId = ".";

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinTreeNode 实例。
        /// </summary>
        /// <param name="path">挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="configFile">插件配置文件。</param>
        public AddinTreeNode(String path, String id, String configFile)
        {
            if (!path.StartsWith("./"))
            {
                if (path.StartsWith("/"))
                {
                    path = WorkspaceNodeId + path;
                }
                else
                {
                    path = $"{WorkspaceNodeId}/{path}";
                }
            }
            if(path.EndsWith("/"))
            {
                FullPath = path + id;
            }
            else
            {
                FullPath = $"{path}/{id}";
            }
            Path = path;
            Id = id;
            ConfigFile = configFile;
        }

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinTreeNode 实例。
        /// </summary>
        /// <param name="path">挂载点。</param>
        /// <param name="id">插件Id。</param>
        internal AddinTreeNode(String path, String id)
        {
            Path = path;
            Id = id;
            FullPath = String.IsNullOrWhiteSpace(path) ? id : $"{path}/{id}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 挂载点。
        /// </summary>
        public String Path { get; }
        
        /// <summary>
        /// 插件Id。
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// 完整路径。
        /// </summary>
        public String FullPath { get; }

        /// <summary>
        /// 插件配置文件。
        /// </summary>
        public String ConfigFile { get; set; }

        /// <summary>
        /// 子节点。
        /// </summary>
        public Collection<AddinTreeNode> Children { get; } = new Collection<AddinTreeNode>();
       
        #endregion
    }
}
