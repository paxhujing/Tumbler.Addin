using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tumbler.Addin.Common;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件扩展方法。
    /// </summary>
    public static class AddinExtensions
    {
        /// <summary>
        /// 通知插件状态已经改变。
        /// </summary>
        /// <param name="addin">状态改变的插件。</param>
        public static void NotifyStateChanged(this IAddin addin)
        {
            AddinManager.Instance.NotifyStateChanged(addin);
        }

        /// <summary>
        /// 通知插件状态已经改变。
        /// </summary>
        /// <param name="addin">状态改变的插件。</param>
        /// <param name="name">数据名称。</param>
        /// <param name="newData">新数据。</param>
        /// <param name="oldData">旧数据。</param>
        public static void NotifyDataChanged(this IAddin addin, String name, Object newData, Object oldData)
        {
            AddinManager.Instance.NotifyDataChanged(addin, name, newData, oldData);
        }

        /// <summary>
        /// 向其它插件（包括宿主）发送消息。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="content">消息内容。</param>
        /// <param name="isAsync">是否异步处理消息。</param>
        public static void SendMessageToAll(this Object sender, Object content, Boolean isAsync = false)
        {
            AddinManager.Instance.SendMessage(sender, AddinManager.AllTargets, content, isAsync);
        }

        /// <summary>
        /// 向其它插件发送消息。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="fullPath">目标插件的完整路径。</param>
        /// <param name="content">消息内容。</param>
        /// <param name="isAsync">是否异步处理消息。</param>
        public static void SendMessage(this Object sender, String fullPath, Object content, Boolean isAsync = false)
        {
            AddinManager.Instance.SendMessage(sender, fullPath, content, isAsync);
        }

        /// <summary>
        /// 向其它插件（包括宿主）发送消息。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="content">消息内容。</param>
        /// <param name="isAsync">是否异步处理消息。</param>
        public static void SendMessageToAll<TCotnent>(this Object sender, TCotnent content, Boolean isAsync = false)
        {
            AddinManager.Instance.SendMessage<TCotnent>(sender, AddinManager.AllTargets, content, isAsync);
        }

        /// <summary>
        /// 向宿主发送消息。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="content">消息内容。</param>
        /// <param name="isAsync">是否异步处理消息。</param>
        public static void SendMessageToHost(this Object sender, KeyValuePair<Byte, Object> content, Boolean isAsync = false)
        {
            AddinManager.Instance.SendMessage(sender, AddinManager.HostTarget, content, isAsync);
        }

        /// <summary>
        /// 向其它插件发送消息。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="fullPath">目标插件的完整路径。</param>
        /// <param name="content">消息内容。</param>
        /// <param name="isAsync">是否异步处理消息。</param>
        public static void SendMessage<TCotnent>(this Object sender, String fullPath, TCotnent content, Boolean isAsync = false)
        {
            AddinManager.Instance.SendMessage<TCotnent>(sender, fullPath, content, isAsync);
        }

        /// <summary>
        /// 销毁插件。
        /// </summary>
        /// <param name="addin">插件。</param>
        public static void Destroy(this IAddin addin)
        {
            AddinManager.Instance.Destroy(addin);
        }

        /// <summary>
        /// 获取插件的子插件列表。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>子插件列表。</returns>
        public static IAddin[] GetChildAddins(this IAddin addin)
        {
            return AddinManager.Instance.GetChildAddins(addin);
        }

        /// <summary>
        /// 构建指定插件的下一级插件列表。
        /// </summary>
        /// <param name="addin">下一级插件的父级插件。</param>
        /// <returns>下一级插件列表。</returns>
        public static IAddin[] BuildChildAddins(this IAddin addin)
        {
            return AddinManager.Instance.BuildChildAddins(addin);
        }

        /// <summary>
        /// 构建指定插件的下一级插件列表。
        /// </summary>
        /// <param name="service">下一级插件的父级服务。</param>
        /// <returns>下一级服务列表。</returns>
        public static IService[] BuildChildService(this IService service)
        {
            return AddinManager.Instance.BuildChildService(service);
        }

        /// <summary>
        /// 获取插件的挂载点。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>挂载点。</returns>
        public static String GetMountExpose(this IAddin addin)
        {
            return AddinManager.Instance.GetMountExpose(addin);
        }

        /// <summary>
        /// 获取插件的暴露点。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>插件的暴露点。</returns>
        public static String[] GetExposes(this IAddin addin)
        {
            return AddinManager.Instance.GetExposes(addin);
        }

        /// <summary>
        /// 获取插件的挂载路径。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>挂载路径。</returns>
        public static String GetMountTo(this IAddin addin)
        {
            return AddinManager.Instance.GetMountTo(addin);
        }

        /// <summary>
        /// 获取插件挂载的完整路径。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>挂载的完整路径。</returns>
        public static String GetFullPath(this IAddin addin)
        {
            return AddinManager.Instance.GetFullPath(addin);
        }

        /// <summary>
        /// 停止依赖该服务的其它服务。
        /// </summary>
        /// <param name="service">主服务。</param>
        public static void StopDependencies(this IService service)
        {
            AddinManager.Instance.StopDependencies(service);
        }
    }
}
