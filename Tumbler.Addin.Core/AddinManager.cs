using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tumber.Addin.Common;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件管理器。
    /// </summary>
    public class AddinManager
    {
        #region Fields

        private readonly AddinTreeNode _root;

        private readonly Dictionary<String, AddinTreeNode> _nodes = new Dictionary<string, AddinTreeNode>();

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinManager 实例。
        /// </summary>
        /// <param name="configFile">服务配置文件。</param>
        public AddinManager(String configFile)
        {
            if (String.IsNullOrWhiteSpace(configFile)) throw new ArgumentNullException("configFile");
            if (!File.Exists(configFile)) throw new FileNotFoundException(configFile);
            ConfigFile = configFile;
            _root = new RootNode(this);
            _nodes.Add(_root.FullPath, _root);
            _nodes.Add(_root.InnerChildren[0].FullPath, _root.InnerChildren[0]);
            _nodes.Add(_root.InnerChildren[1].FullPath, _root.InnerChildren[1]);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取服务配置文件。
        /// </summary>
        public String ConfigFile { get; }

        /// <summary>
        /// 获取插件数量。
        /// </summary>
        public Int32 Count { get; private set; }

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

        /// <summary>
        /// 构建第一级的所有插件。
        /// </summary>
        /// <returns>第一级的所有插件列表。</returns>
        public IAddin[] BuildFirstLevelAddins()
        {
            return BuildlAddins(_root.Children[0].Children);
        }

        /// <summary>
        /// 构建指定插件的下一级插件列表。
        /// </summary>
        /// <param name="addin">下一级插件的父级插件。</param>
        /// <returns>下一级插件列表。</returns>
        public IAddin[] BuildChildAddins(IAddin addin)
        {
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) throw new InvalidOperationException("This addin is out of control");
            return BuildlAddins(descriptor.Owner.Children);
        }

        /// <summary>
        /// 销毁插件。
        /// </summary>
        /// <param name="addin">插件。</param>
        public void Destroy(IAddin addin)
        {
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) throw new InvalidOperationException("This addin is out of control");
            descriptor.Destroy();
        }

        /// <summary>
        /// 销毁插件。
        /// </summary>
        /// <param name="addinNode">插件树节点。</param>
        public void Uninstall(AddinNode addinNode)
        {
            String configFile = addinNode.ConfigFile;
            XElement xml = XElement.Load(ConfigFile);
            XElement removeItem = xml.Elements("Addin").SingleOrDefault(x => x.Attribute("ref")?.Value == configFile);
            if(removeItem != null)
            {
                removeItem.Remove();
            }
            xml.Save(ConfigFile);
        }

        /// <summary>
        /// 获取指定路径的插件状态。
        /// </summary>
        /// <param name="fullPath">插件所在完整路径。</param>
        /// <returns>插件状态。如果为null表示当前无法获取到插件的状态，可能是当前节点不是插件节点AddinNode，或者插件还未构建。</returns>
        public AddinState? GetAddinState(String fullPath)
        {
            AddinTreeNode node = GetNode(fullPath);
            if (node == null) throw new InvalidOperationException("Unknow path");
            AddinNode addinNode = node as AddinNode;
            if(addinNode != null && addinNode.Descriptor.IsValueCreated)
            {
                return addinNode.Descriptor.Value.AddinState;
            }
            return null;
        }

        /// <summary>
        /// 获取插件的状态。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>插件状态。如果为null表示插件管理器无法跟踪该插件。</returns>
        public AddinState? GetAddinState(IAddin addin)
        {
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) return null;
            return descriptor.AddinState;
        }

        /// <summary>
        /// 设置插件的状态。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <param name="state">状态。</param>
        /// <returns>设置成功返回true；否则返回false。</returns>
        public Boolean SetAddinState(IAddin addin, AddinState state)
        {
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) return false;
            descriptor.AddinState = state;
            return true;
        }

        /// <summary>
        /// 向其它插件发送消息。
        /// </summary>
        /// <param name="fullPath">目标插件的完整路径。</param>
        /// <param name="message">消息。</param>
        public void SendMessage(String fullPath, Hashtable message)
        {
            AddinNode node = GetNode(fullPath) as AddinNode;
            if (node == null) return;
            AddinDescriptor descriptor = node.Descriptor.IsValueCreated ? node.Descriptor.Value : null;
            if (descriptor != null && descriptor.BuildState == AddinBuildState.Build && descriptor.CanRecieveMessage)
            {
                ((IHandler)descriptor.Addin).Handle(message);
            }
        }

        /// <summary>
        /// 获取指定路径的插件树节点。
        /// </summary>
        /// <param name="fullPath">路径。</param>
        /// <returns>插件树节点。</returns>
        public AddinTreeNode GetNode(String fullPath)
        {
            if (String.IsNullOrWhiteSpace(fullPath)) return null;
            fullPath = AddinTreeNode.CompletePath(fullPath);
            if (_nodes.ContainsKey(fullPath))
            {
                return _nodes[fullPath];
            }
            return null;
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
                if (node != null && !_nodes.ContainsKey(node.FullPath))
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
            XElement xml = XElement.Load(file)?.Element("Runtimes");
            if (xml == null) throw new FileLoadException("Invalid addin installation file");
            return new AddinNode(xml.Attribute("Path")?.Value, xml.Attribute("Id").Value, this, file);
        }

        /// <summary>
        /// 生成插件树。
        /// </summary>
        private void GenerateAddinTree()
        {
            foreach (AddinTreeNode node in _nodes.Values.Skip(3).ToArray())
            {
                if(_nodes.ContainsKey(node.Path))
                {
                    _nodes[node.Path].InnerChildren.Add(node);
                }
                else
                {
                    _nodes.Remove(node.FullPath);
                }
            }
            Count = _nodes.Count - 2;
        }

        /// <summary>
        /// 构建插件列表。
        /// </summary>
        /// <returns>插件列表。</returns>
        private IAddin[] BuildlAddins(ReadOnlyCollection<AddinTreeNode> nodes)
        {
            AddinNode addinNode = null;
            Collection<IAddin> addins = new Collection<IAddin>();
            foreach (AddinTreeNode node in nodes)
            {
                addinNode = node as AddinNode;
                if (addinNode != null)
                {
                    addins.Add(addinNode.Buid());
                }
            }
            return addins.ToArray();
        }

        #endregion

        #endregion
    }
}
