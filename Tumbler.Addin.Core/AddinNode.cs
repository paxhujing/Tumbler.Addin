using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Common;

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
        /// <param name="mountTo">要挂载到的节点。</param>
        /// <param name="mountPoint">要挂载到的节点中的挂载点。</param>
        /// <param name="id">插件Id。</param>
        /// <param name="exposes">向外提供的挂载点。</param>
        /// <param name="addinConfigFile">插件的配置文件。</param>
        public AddinNode(String mountTo, String mountPoint, String id, String[] exposes, String addinConfigFile)
            : base(mountTo, mountPoint, id, exposes)
        {
            if (String.IsNullOrWhiteSpace(addinConfigFile))
            {
                throw new ArgumentNullException("configFile");
            }
            AddinConfigFile = addinConfigFile;
            Descriptor = new Lazy<AddinDescriptor>(() => AddinDescriptor.Parse(AddinConfigFile, this));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取一个标识，表示该节点是一个插件节点还是路径节点。
        /// </summary>
        public override Boolean IsVitual
        {
            get { return false; }
        }

        /// <summary>
        /// 获取插件配置文件路径。
        /// </summary>
        public String AddinConfigFile { get; set; }

        /// <summary>
        /// 获取插件的描述。
        /// </summary>
        internal Lazy<AddinDescriptor> Descriptor { get; }

        #region Info

        private WeakReference<AddinBaseInfo> _info = new WeakReference<AddinBaseInfo>(null);
        /// <summary>
        /// 插件基本信息。
        /// </summary>
        public AddinBaseInfo Info
        {
            get
            {
                AddinBaseInfo target = null;
                if (!_info.TryGetTarget(out target))
                {
                    target = AddinBaseInfo.Parse(AddinConfigFile);
                    _info.SetTarget(target);
                }
                return target;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public

        #endregion

        #region Internal

        /// <summary>
        /// 构建插件。
        /// </summary>
        /// <returns>代表了插件的对象，例如一个UI元素。</returns>
        internal IAddin Buid()
        {
            return Descriptor.Value?.Build();
        }

        #endregion

        #endregion
    }
}
