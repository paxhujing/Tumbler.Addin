using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件管理器。
    /// </summary>
    public class AddinManager
    {
        #region Fields

        private readonly AddinTreeNode _root = new RootAddinTreeNode();

        private readonly Dictionary<String, AddinTreeNode> _nodes = new Dictionary<string, AddinTreeNode>();

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinManager 实例。
        /// </summary>
        /// <param name="configFile">服务配置文件。</param>
        public AddinManager(String configFile)
        {
            ConfigFile = configFile;
            _nodes.Add(_root.Children[0].FullPath, _root.Children[0]);
            _nodes.Add(_root.Children[1].FullPath, _root.Children[1]);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 服务配置文件。
        /// </summary>
        public String ConfigFile { get; }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 初始化插件管理器。
        /// </summary>
        public void Initialize()
        {
            if (String.IsNullOrWhiteSpace(ConfigFile)) return;
            if (!File.Exists(ConfigFile)) throw new FileNotFoundException(ConfigFile);
            CreateAddinTreeNodes();
            GenerateAddinTree();
        }

        #endregion

        #region Private

        /// <summary>
        /// 创建插件树节点列表。
        /// </summary>
        private void CreateAddinTreeNodes()
        {
            XElement xml = XElement.Load(ConfigFile);
            if (xml == null) throw new FileLoadException("Invalid addin manager config file");
            AddinTreeNode node = null;
            foreach (XElement element in xml.Elements("Addin"))
            {
                node = GetAddinTreeNode(element);
                if (node != null)
                {
                    _nodes.Add(node.FullPath, node);
                }
            }
        }

        /// <summary>
        /// 创建插件树节点。
        /// </summary>
        /// <param name="element">插件配置。</param>
        /// <returns>插件树节点。</returns>
        private AddinTreeNode GetAddinTreeNode(XElement element)
        {
            XAttribute attribute = element.Attribute("ref");
            if (attribute == null) return null;
            String file = attribute.Value;
            if (String.IsNullOrWhiteSpace(file) || !File.Exists(file)) return null;
            XElement xml = XElement.Load(file);
            if (xml == null) throw new FileLoadException("Invalid addin config file");
            XAttribute pathAttr = xml.Attribute("Path");
            if(pathAttr == null) throw new FileLoadException("Invalid addin config file.[Path]");
            XAttribute idAttr = xml.Attribute("Id");
            if (idAttr == null) throw new FileLoadException("Invalid addin config file.[Id]");
            return new AddinTreeNode(pathAttr.Value, idAttr.Value, file);
        }

        /// <summary>
        /// 生成插件树。
        /// </summary>
        private void GenerateAddinTree()
        {
            foreach(AddinTreeNode node in _nodes.Values.Skip(2).ToArray())
            {
                if(_nodes.ContainsKey(node.Path))
                {
                    _nodes[node.Path].Children.Add(node);
                }
                else
                {
                    _nodes.Remove(node.FullPath);
                }
            }
        }

        #endregion

        #endregion
    }
}
