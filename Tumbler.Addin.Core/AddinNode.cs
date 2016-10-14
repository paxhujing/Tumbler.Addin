using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumber.Addin.Common;

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
        /// <param name="owner">拥有此插件树节点的管理器。</param>
        /// <param name="addinConfigFile">插件配置文件。</param>
        public AddinNode(String path, String id, AddinManager owner, String addinConfigFile)
            : base(path, id, owner)
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
