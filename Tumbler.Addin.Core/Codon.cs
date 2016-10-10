using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 描述了实现插件功能的具体对象。
    /// </summary>
    public class Codon
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.Codon 实例。
        /// </summary>
        /// <param name="class">实现插件功能的类型。</param>
        /// <param name="addin">所属的插件。</param>
        /// <param name="order">插件的安装顺序。-1表示按默认顺序安装。</param>
        public Codon(String @class,IAddin addin ,Int32 order = -1)
        {
            Class = @class;
            Order = order;
            Addin = addin;
        }

        #endregion
        /// <summary>
        /// 获取所属的插件。
        /// </summary>
        public IAddin Addin { get; }

        /// <summary>
        /// 获取实现插件功能的类型。
        /// </summary>
        public String Class { get; }

        /// <summary>
        /// 获取插件的安装顺序。
        /// </summary>
        public Int32 Order { get; }

        /// <summary>
        /// 初始化参数。
        /// </summary>
        public NameValueCollection Parameters { get; } = new NameValueCollection();

        Object BuildItem();
    }
}
