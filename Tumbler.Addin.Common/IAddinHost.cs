namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 插件宿主。
    /// </summary>
    public interface IAddinHost
    {
        /// <summary>
        /// 收到消息。
        /// </summary>
        /// <param name="message">消息。</param>
        void OnReceive(MessageArgs message);
    }
}
