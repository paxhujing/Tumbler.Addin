using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tumbler.Addin.Common
{
    /// <summary>
    /// 插件基本信息。
    /// </summary>
    public class AddinBaseInfo
    {
        #region Constructors

        private AddinBaseInfo()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 插件名称。
        /// </summary>
        public String Name { get; internal set; }

        /// <summary>
        /// 作者。
        /// </summary>
        public String Author { get; internal set; }

        /// <summary>
        /// 版权。
        /// </summary>
        public String Copyright { get; internal set; }

        /// <summary>
        /// Url。
        /// </summary>
        public String Url { get; internal set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public String Description { get; internal set; }

        /// <summary>
        /// 版本。
        /// </summary>
        public String Version { get; internal set; }

        /// <summary>
        /// 配置文件路径。
        /// </summary>
        public String AddinConfigFile { get; internal set; }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 通过解析插件配置文件获取插件基本信息。
        /// </summary>
        /// <param name="addinConfigFile">插件配置文件。</param>
        /// <returns>插件基本信息。</returns>
        public static AddinBaseInfo Parse(String addinConfigFile)
        {
            if (String.IsNullOrWhiteSpace(addinConfigFile))
            {
                throw new ArgumentNullException("addinConfigFile");
            }
            if (!addinConfigFile.EndsWith(".addin"))
            {
                throw new FileLoadException("Invalid addin config file");
            }
            AddinBaseInfo info = null;
            XElement xml = XElement.Load(addinConfigFile);
            XElement infoNode = xml.Element("Info");
            if (infoNode != null)
            {
                info = new AddinBaseInfo();
                info.Name = infoNode.Attribute("Name")?.Value;
                info.Author = infoNode.Attribute("Author")?.Value;
                info.Copyright = infoNode.Attribute("Copyright")?.Value;
                info.Url = infoNode.Attribute("Url")?.Value;
                info.Description = infoNode.Attribute("Description")?.Value;
                info.Version = infoNode.Attribute("Version")?.Value;
                info.AddinConfigFile = addinConfigFile;
            }
            return info;

        }

        #endregion

        #endregion
    }
}
