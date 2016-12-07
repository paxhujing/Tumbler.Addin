using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 插件通信时使用的消息参数。
    /// </summary>
    public class MessageArgs
    {
        /// <summary>
        /// 初始化类型 Tumbler.Addin.Common.MessageArgs 实例。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="destination">目的地。</param>
        /// <param name="content">消息内容。</param>
        public MessageArgs(Object sender, String destination, Object content)
        {
            Destination = destination;
            Content = content;
            Sender = sender;
        }

        #region Properties

        /// <summary>
        /// 发送者。
        /// </summary>
        public Object Sender { get; }

        /// <summary>
        /// 目的地。
        /// </summary>
        public String Destination { get; }

        /// <summary>
        /// 消息内容。
        /// </summary>
        public Object Content { get; }

        /// <summary>
        /// 是否是异步消息。
        /// </summary>
        public Boolean IsAsync { get; set; }

        #endregion
    }

    /// <summary>
    /// 插件通信时使用的消息参数。
    /// </summary>
    /// <typeparam name="TContent">消息内容的类型。</typeparam>
    public class MessageArgs<TContent> : MessageArgs
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Common.MessageArgs 实例。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="destination">目的地。</param>
        /// <param name="content">消息内容。</param>
        public MessageArgs(Object sender, String destination, TContent content)
            : base(sender, destination, content)
        {
            Content = content;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 消息内容。
        /// </summary>
        public new TContent Content { get; }

        #endregion
    }
}
