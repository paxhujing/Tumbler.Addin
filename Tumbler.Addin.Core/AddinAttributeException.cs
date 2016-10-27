using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 当处理AddinAttribute特性失败时引发的异常。
    /// </summary>
    public class AddinAttributeException : Exception
    {
        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinAttributeException 实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public AddinAttributeException(String message)
            :base(message)
        {

        }
    }
}
