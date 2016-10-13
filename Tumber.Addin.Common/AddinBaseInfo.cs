using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumber.Addin.Common
{
    /// <summary>
    /// 插件基本信息。
    /// </summary>
    public class AddinBaseInfo
    {
        /// <summary>
        /// 插件名称。
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 作者。
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// 版权。
        /// </summary>
        public String Copyright { get; set; }

        /// <summary>
        /// Url。
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 版本。
        /// </summary>
        public String Version { get; set; }
    }
}
