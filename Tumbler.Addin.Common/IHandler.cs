using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 表示可以处理来自其它插件的消息。
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// 处理来自其它插件的消息。
        /// </summary>
        /// <param name="message">消息。</param>
        void Handle(Object message);
    }
}
