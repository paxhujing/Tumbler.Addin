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

        /// <summary>
        /// 所有节点都具有的默认挂载点。
        /// </summary>
        public const String DefaultExposePoint = "Default";

        private readonly Int32 _hash;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinTreeNode 实例。
        /// </summary>
        /// <param name="mountTo">要挂载到的节点。</param>
        /// <param name="mountPoint">要挂载到的节点中的挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="exposes">向外提供的挂载点。</param>
        protected AddinTreeNode(String mountTo, String mountPoint, String id, String[] exposes)
        {
            if (mountTo == null)
            {
                throw new ArgumentNullException("mountTo");
            }
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }
            MountTo = CompletePath(mountTo);
            MountPoint = mountPoint;
            Id = id;
            FullPath = GetFullPath();
            _hash = FullPath.GetHashCode();
            Exposes = exposes;
            if (exposes != null && exposes.Length != 0)
            {
                foreach (String expose in exposes)
                {
                    InnerChildren.Add(expose, new Collection<AddinTreeNode>());
                }
            }
            InnerChildren.Add(DefaultExposePoint, new Collection<AddinTreeNode>());
            Children = new ReadOnlyDictionary<String, Collection<AddinTreeNode>>(InnerChildren);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取一个标识，表示该节点是一个插件节点还是路径节点。
        /// </summary>
        public abstract Boolean IsVirtual { get; }

        /// <summary>
        /// 获取要挂载到的节点路径。
        /// </summary>
        public String MountTo { get; }

        /// <summary>
        /// 获取要挂载到的节点中的挂载点。
        /// </summary>
        public String MountPoint { get; }

        /// <summary>
        /// 获取插件Id。
        /// </summary>
        public String Id { get; }

        /// <summary>
        /// 获取向外提供的挂载点。
        /// </summary>
        public String[] Exposes { get; }

        /// <summary>
        /// 获取完整路径。
        /// </summary>
        public String FullPath { get; }

        /// <summary>
        /// 获取当前节点的子节点。
        /// </summary>
        public ReadOnlyDictionary<String, Collection<AddinTreeNode>> Children { get; }

        /// <summary>
        /// 内部获取当前节点的子节点。
        /// </summary>
        internal Dictionary<String, Collection<AddinTreeNode>> InnerChildren { get; } = new Dictionary<String, Collection<AddinTreeNode>>();

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 补全路径。
        /// </summary>
        /// <param name="mountTo">要挂载到的节点。</param>
        /// <returns>包含工作空间的完成路径。</returns>
        public static String CompletePath(String mountTo)
        {
            String fullMountTo = null;
            if (String.IsNullOrWhiteSpace(mountTo))
            {
                fullMountTo = String.Empty;
            }
            else if (mountTo == AddinTreeNode.WorkspaceId)
            {
                fullMountTo = AddinTreeNode.WorkspaceId;
            }
            else
            {
                if (mountTo.StartsWith("./"))
                {
                    fullMountTo = mountTo;
                }
                else
                {
                    if (mountTo.StartsWith("/"))
                    {
                        fullMountTo = AddinTreeNode.WorkspaceId + mountTo;
                    }
                    else
                    {
                        fullMountTo = $"{AddinTreeNode.WorkspaceId}/{mountTo}";
                    }
                }
            }
            return fullMountTo;
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

        #region Private

        /// <summary>
        /// 获取节点的完整路径。
        /// </summary>
        /// <returns>节点的完整路径。</returns>
        private String GetFullPath()
        {
            String mountTo = MountTo;
            String mountPoint = MountPoint;
            String temp = mountTo;
            if (!String.IsNullOrWhiteSpace(mountPoint) && DefaultExposePoint != mountPoint)
            {
                temp = $"{mountTo}/{mountPoint}";
            }
            if (mountTo != String.Empty)
            {
                return $"{temp}/{Id}";
            }
            else
            {
                return Id;
            }
        }

        #endregion

        #endregion
    }
}
