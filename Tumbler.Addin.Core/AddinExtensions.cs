﻿using System;
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
        /// 设置插件状态。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <param name="state">状态。</param>
        /// <returns>设置成功返回true；否则返回false。</returns>
        public static Boolean SetState(this IAddin addin, AddinState state)
        {
            return AddinManager.Instance.SetAddinState(addin, state);
        }

        /// <summary>
        /// 获取插件状态。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>插件状态。</returns>
        public static AddinState GetState(this IAddin addin)
        {
            return AddinManager.Instance.GetAddinState(addin);
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
    }
}