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
    public abstract class AddinTreeNode
    {
        #region Fields

        /// <summary>
        /// 工作空间的路径。
        /// </summary>
        public const String WorkspaceId = ".";

        private readonly Int32 _hash;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinTreeNode 实例。
        /// </summary>
        /// <param name="path">挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="manager">拥有此插件树节点的管理器。</param>
        protected AddinTreeNode(String path, String id, AddinManager manager)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            if (manager == null)
            {
                throw new ArgumentNullException("owner");
            }
            Manager = manager;
            path = CompletePath(path);
            if (!String.IsNullOrWhiteSpace(path))
            {
                FullPath = $"{path}/{id}";
            }
            else
            {
                FullPath = id;
            }
            _hash = FullPath.GetHashCode();
            Path = path;
            Id = id;
            Children = new ReadOnlyCollection<AddinTreeNode>(InnerChildren);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取节点的上级路径。
        /// </summary>
        public String Path { get; }
        
        /// <summary>
        /// 获取插件Id。
        /// </summary>
        public String Id { get; }

        /// <summary>
        /// 拥有此插件树节点的管理器。
        /// </summary>
        public AddinManager Manager { get; }

        /// <summary>
        /// 获取完整路径。
        /// </summary>
        public String FullPath { get; }

        /// <summary>
        /// 获取当前节点的子节点。
        /// </summary>
        public ReadOnlyCollection<AddinTreeNode> Children { get; }

        /// <summary>
        /// 内部获取当前节点的子节点。
        /// </summary>
        internal Collection<AddinTreeNode> InnerChildren { get; } = new Collection<AddinTreeNode>();

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 补全路径。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>包含工作空间的完成路径。</returns>
        public static String CompletePath(String path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return String.Empty;
            }
            else if (path == AddinTreeNode.WorkspaceId)
            {
                return AddinTreeNode.WorkspaceId;
            }
            else
            {
                if (path.StartsWith("./"))
                {
                    return path;
                }
                else
                {
                    if (path.StartsWith("/"))
                    {
                        return AddinTreeNode.WorkspaceId + path;
                    }
                    else
                    {
                        return $"{AddinTreeNode.WorkspaceId}/{path}";
                    }
                }
            }
        }

        /// <summary>
        /// 获取对象的Hash值。
        /// </summary>
        /// <returns>对象的Hash值。</returns>
        public override Int32 GetHashCode()
        {
            return _hash;
        }

        #endregion

        #endregion
    }
}
