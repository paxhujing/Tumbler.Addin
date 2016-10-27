using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 表示插件的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AddinAttribute : Attribute
    {
        /// <summary>
        /// 初始化类型 Tumbler.Addin.Common.AddinAttribute 实例。
        /// </summary>
        /// <param name="guid">插件的Guid。</param>
        public AddinAttribute(String guid)
            :this(Guid.Parse(guid))
        {
        }

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Common.AddinAttribute 实例。
        /// </summary>
        /// <param name="guid">插件的Guid。</param>
        private AddinAttribute(Guid guid)
        {
            Guid = guid;
        }

        /// <summary>
        /// 插件的Guid。
        /// </summary>
        public Guid Guid { get; }
    }
}
