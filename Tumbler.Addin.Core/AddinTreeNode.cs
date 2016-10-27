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

        private static readonly Dictionary<String, Tuple<ReadOnlyCollection<AddinTreeNode>,Collection<AddinTreeNode>>> GlobalChilds = new Dictionary<String, Tuple<ReadOnlyCollection<AddinTreeNode>, Collection<AddinTreeNode>>>();

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
        /// <param name="mountExpose">要挂载到的节点中的挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="exposes">向外提供的挂载点。</param>
        protected AddinTreeNode(String mountTo, String mountExpose, String id, String[] exposes)
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
            MountExpose = mountExpose;
            Id = id;
            FullPath = GetFullPath();
            _hash = FullPath.GetHashCode();
            Exposes = exposes;
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
        public String MountExpose { get; }

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

        #region Internal

        /// <summary>
        /// 获取指定挂载点下的子节点。
        /// </summary>
        /// <param name="expose">挂载点。</param>
        /// <returns>子节点。</returns>
        internal ReadOnlyCollection<AddinTreeNode> GetChilds(String expose = null)
        {
            String mount;
            if (TryGetMount(out mount, expose))
            {
                return GetReadOnlyCollection(mount);
            }
            return null;
        }

        /// <summary>
        /// 将子节点添加到指定的挂载点下。
        /// </summary>
        /// <param name="expose">挂载点。</param>
        /// <param name="child">子节点。</param>
        internal void SetChild(AddinTreeNode child, String expose = null)
        {
            String mount;
            if (TryGetMount(out mount, expose))
            {
                Collection<AddinTreeNode> c = GetOrAddCollection(mount);
                c.Add(child);
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 尝试获取暴露点所在的完整路径。
        /// </summary>
        /// <param name="mount">暴露点所在的完整路径。</param>
        /// <param name="expose">暴露点。</param>
        /// <returns>如果成功返回true；否则返回false。</returns>
        private Boolean TryGetMount(out String mount, String expose = null)
        {
            mount = null;
            String[] items = Exposes;
            if (items == null) return false;
            if (String.IsNullOrWhiteSpace(expose))
            {
                if (IsVirtual)
                {
                    expose = DefaultExposePoint;
                }
                else
                {
                    return false;
                }
            }

            for (Int32 i = 0; i < items.Length; i++)
            {
                if (items[i] == expose)
                {
                    mount = $"{FullPath}/{expose}";
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取或者添加一个子节点集合。
        /// </summary>
        /// <param name="mount">挂在路径和挂载点。</param>
        /// <returns>子节点集合。</returns>
        private Collection<AddinTreeNode> GetOrAddCollection(String mount)
        {
            if (GlobalChilds.ContainsKey(mount))
            {
                return GlobalChilds[mount].Item2;
            }
            Collection<AddinTreeNode> c = new Collection<AddinTreeNode>();
            ReadOnlyCollection<AddinTreeNode> rc = new ReadOnlyCollection<AddinTreeNode>(c);
            GlobalChilds.Add(mount, new Tuple<ReadOnlyCollection<AddinTreeNode>, Collection<AddinTreeNode>>(rc, c));
            return c;
        }

        /// <summary>
        /// 获取指定挂载点下子节点只读集合。
        /// </summary>
        /// <param name="mount">挂载点。</param>
        /// <returns>子节点只读集合。</returns>
        private ReadOnlyCollection<AddinTreeNode> GetReadOnlyCollection(String mount)
        {
            if (GlobalChilds.ContainsKey(mount))
            {
                return GlobalChilds[mount].Item1;
            }
            return null;
        }

        /// <summary>
        /// 获取节点的完整路径。
        /// </summary>
        /// <returns>节点的完整路径。</returns>
        private String GetFullPath()
        {
            String mountTo = MountTo;
            String mountExpose = MountExpose;
            String temp = mountTo;
            if (!String.IsNullOrWhiteSpace(mountExpose) && DefaultExposePoint != mountExpose)
            {
                temp = $"{mountTo}/{mountExpose}";
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
