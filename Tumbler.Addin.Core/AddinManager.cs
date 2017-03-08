using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tumbler.Addin.Common;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 插件管理器。
    /// </summary>
    public class AddinManager
    {
        #region Fields

        /// <summary>
        /// 插件服务的实例。
        /// </summary>
        public static readonly AddinManager Instance = new AddinManager();

        private AddinTreeNode _root;

        private readonly Dictionary<String, AddinTreeNode> _nodes = new Dictionary<string, AddinTreeNode>();

        private Boolean _isInit;

        private Queue<MessageArgs> _messageQueue;

        private CancellationTokenSource _syncEvent;

        private MethodInfo _mi;

        private readonly Object _syncObj = new Object();

        /// <summary>
        /// 所有目标对象。
        /// </summary>
        public const String AllTargets = "*";

        /// <summary>
        /// 宿主目标对象。
        /// </summary>
        public const String HostTarget = ".";

        private IAddinHost _host;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinManager 实例。
        /// </summary>
        private AddinManager()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取服务配置文件。
        /// </summary>
        public String ConfigFile { get; private set; }

        /// <summary>
        /// 获取插件数量。
        /// </summary>
        public Int32 Count { get; private set; }

        /// <summary>
        /// 获取或设置过滤器。过滤器用于根据指定条件筛选符合要求的插件。
        /// </summary>
        public Func<XElement,Boolean> Filter { get; set; }

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 初始化插件管理器。
        /// </summary>
        /// <param name="host">宿主。</param>
        /// <param name="configFile">配置文件。</param>
        /// <param name="initExposes">初始化挂载点。</param>
        public void Initialize(IAddinHost host, String configFile, Tuple<String, String, String[]>[] initExposes = null)
        {
            if (_isInit) return;
            if (host == null) throw new ArgumentNullException("host");
            _host = host;
            if (String.IsNullOrWhiteSpace(configFile)) throw new ArgumentNullException("configFile");
            configFile = AddinManager.GetFileFullPath(configFile);
            if (!File.Exists(configFile)) throw new FileNotFoundException(configFile);
            ConfigFile = configFile;
            _root = new RootNode();
            _nodes.Add(_root.FullPath, _root);
            _nodes.Add(_root.GetChilds()[0].FullPath, _root.GetChilds()[0]);
            _nodes.Add(_root.GetChilds()[1].FullPath, _root.GetChilds()[1]);
            CreateAddinTreeNodes(initExposes);
            GenerateAddinTree();
            _isInit = true;
        }

        /// <summary>
        /// 获取已安装的插件信息。
        /// </summary>
        /// <returns>已安装插件的信息列表。</returns>
        public IEnumerable<AddinBaseInfo> GetInstallAddinInfos()
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            return _nodes.Values.Skip(3).OfType<AddinNode>().Select(x => x.Info);
        }

        /// <summary>
        /// 构建第一级的所有插件。
        /// </summary>
        /// <returns>第一级的所有插件列表。</returns>
        public IAddin[] BuildFirstLevelAddins()
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            IList<IAddin> addins = new List<IAddin>();
            BuildImpl(_root.GetChilds()[0], ref addins);
            return addins.ToArray();
        }

        /// <summary>
        /// 构建第一级的所有服务。
        /// </summary>
        /// <returns>第一级的所有服务列表。</returns>
        public IService[] BuildFirstLevelServices()
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            IList<IAddin> addins = new List<IAddin>();
            BuildImpl(_root.GetChilds()[1], ref addins);
            return addins.Cast<IService>().ToArray();
        }

        /// <summary>
        /// 安装插件。
        /// </summary>
        /// <param name="addinConfigFile">插件配置文件。</param>
        public void Install(String addinConfigFile)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            if (String.IsNullOrWhiteSpace(addinConfigFile))
            {
                throw new ArgumentNullException("addinConfigFile");
            }
            if(!addinConfigFile.EndsWith(".addin"))
            {
                throw new FileLoadException("Invalid addin config file");
            }

            XElement xml = XElement.Load(ConfigFile);
            XElement exsited = xml.Elements("Addin").SingleOrDefault(x => x.Attribute("ref")?.Value == addinConfigFile);
            if (exsited != null) return;
            XElement newElement = XElement.Parse($"<Addin ref='{addinConfigFile}'/>");
            xml.Add(newElement);
            xml.Save(ConfigFile);
        }

        /// <summary>
        /// 卸载插件。
        /// </summary>
        /// <param name="addinNode">插件树节点。</param>
        public void Uninstall(AddinNode addinNode)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            String addinConfigFile = addinNode.AddinConfigFile;
            UninstalImpl(addinConfigFile);
        }

        /// <summary>
        /// 卸载插件。
        /// </summary>
        /// <param name="info">插件基本信息。</param>
        public void Uninstall(AddinBaseInfo info)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            String addinConfigFile = info.AddinConfigFile;
            if (String.IsNullOrWhiteSpace(addinConfigFile)) return;
            UninstalImpl(addinConfigFile);
        }

        /// <summary>
        /// 获取指定路径的插件树节点。
        /// </summary>
        /// <param name="fullPath">路径。如果为“*”表示所有插件。</param>
        /// <returns>插件树节点。</returns>
        public IEnumerable<AddinTreeNode> GetNode(String fullPath)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            if (String.IsNullOrWhiteSpace(fullPath)) return null;
            if (fullPath == AllTargets)
            {
                return _nodes.Values;
            }
            else
            {
                fullPath = AddinTreeNode.CompletePath(fullPath);
                if (_nodes.ContainsKey(fullPath))
                {
                    return new AddinTreeNode[] { _nodes[fullPath] };
                }
            }
            return null;
        }

        #endregion

        #region Internal

        /// <summary>
        /// 向其它插件发送消息。
        /// </summary>
        /// <param name="sender">发送者。</param>
        /// <param name="fullPath">目标插件的完整路径。</param>
        /// <param name="content">消息。</param>
        /// <param name="isAsync">是否异步处理消息。</param>
        internal void SendMessage<TContent>(Object sender, String fullPath, TContent content, Boolean isAsync = false)
        {
            MessageArgs<TContent> message = new MessageArgs<TContent>(sender, fullPath, content);
            if (isAsync)
            {
                lock (_syncObj)
                {
                    if (_messageQueue == null) InitMessageDispatcher();
                    message.IsAsync = true;
                    _messageQueue.Enqueue(message);
                    if (_messageQueue.Count == 1)
                    {
                        ((ManualResetEvent)_syncEvent.Token.WaitHandle).Set();
                    }
                }
            }
            else
            {
                SendMessageImpl<TContent> (message);
            }
        }

        /// <summary>
        /// 获取文件的完整路径。
        /// </summary>
        /// <param name="file">文件部分路径或完整路径。</param>
        /// <returns>完整路径。</returns>
        internal static String GetFileFullPath(String file)
        {
            String root = Path.GetPathRoot(file);
            if (root == String.Empty)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            else if(root == @"\")
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.Remove(0, 1));
            }
            else
            {
                return file;
            }
        }

        /// <summary>
        /// 构建指定插件的下一级插件列表。
        /// </summary>
        /// <param name="addin">下一级插件的父级插件。</param>
        /// <returns>下一级插件列表。</returns>
        internal IAddin[] BuildChildAddins(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) throw new InvalidOperationException("This addin is out of control");
            IList<IAddin> addins = new List<IAddin>();
            BuildImpl(descriptor.Owner, ref addins);
            return addins.ToArray();
        }

        /// <summary>
        /// 获取插件的子插件列表。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>子插件列表。</returns>
        internal IAddin[] GetChildAddins(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) throw new InvalidOperationException("This addin is out of control");
            String[] exposes = descriptor.Owner.Exposes;
            return GetChildsImpl(descriptor.Owner);
        }

        /// <summary>
        /// 构建指定插件的下一级插件列表。
        /// </summary>
        /// <param name="service">下一级插件的父级服务。</param>
        /// <returns>下一级服务列表。</returns>
        internal IService[] BuildChildService(IService service)
        {
            return (IService[])BuildChildAddins(service);
        }

        /// <summary>
        /// 销毁插件。
        /// </summary>
        /// <param name="addin">插件。</param>
        internal void Destroy(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) throw new InvalidOperationException("This addin is out of control");
            descriptor.Destroy();
        }

        /// <summary>
        /// 获取插件挂载的完整路径。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>挂载的完整路径。</returns>
        internal String GetFullPath(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) return null;
            return descriptor.Owner.FullPath;
        }

        /// <summary>
        /// 获取目标对象挂载的完整路径。
        /// </summary>
        /// <param name="target">目标对象。</param>
        /// <returns>挂载的完整路径。</returns>
        internal String GetFullPath(Object target)
        {
            if (target == _host) return HostTarget;
            IAddin addin = target as IAddin;
            if (addin == null) return null;
            return GetFullPath(addin);
        }

        /// <summary>
        /// 获取插件的挂载路径。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>挂载路径。</returns>
        internal String GetMountTo(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) return null;
            return descriptor.Owner.MountTo;
        }

        /// <summary>
        /// 获取插件的挂载点。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>挂载点。</returns>
        internal String GetMountExpose(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) return null;
            return descriptor.Owner.MountExpose;
        }

        /// <summary>
        /// 获取插件的暴露点。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>插件的暴露点。</returns>
        internal String[] GetExposes(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            if (descriptor == null) return null;
            return descriptor.Owner.Exposes;
        }

        /// <summary>
        /// 通知插件状态已经改变。
        /// </summary>
        /// <param name="addin">状态改变的插件。</param>
        internal void NotifyStateChanged(IAddin addin)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            String fullPath = descriptor.Owner.FullPath;
            Collection<AddinDescriptor> descriptors = AddinDescriptor.GetDependencies(fullPath);
            if (descriptors == null || descriptors.Count == 0) return;
            for (Int32 i = 0; i < descriptors.Count; i++)
            {
                if (descriptors[i].BuildState == AddinBuildState.Build)
                {
                    descriptors[i].Addin.OnDependencyStateChanged(fullPath, addin.State);
                }
            }
        }

        /// <summary>
        /// 通知插件数据已经改变。
        /// </summary>
        /// <param name="addin">状态改变的插件。</param>
        /// <param name="name">数据名称。</param>
        /// <param name="newData">新数据。</param>
        /// <param name="oldData">旧数据。</param>
        internal void NotifyDataChanged(IAddin addin, String name, Object newData, Object oldData)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(addin);
            String fullPath = descriptor.Owner.FullPath;
            Collection<AddinDescriptor> descriptors = AddinDescriptor.GetDependencies(fullPath);
            if (descriptors == null || descriptors.Count == 0) return;
            for (Int32 i = 0; i < descriptors.Count; i++)
            {
                if (descriptors[i].BuildState == AddinBuildState.Build)
                {
                    descriptors[i].Addin.OnDataChanged(fullPath, name, newData, oldData);
                }
            }
        }

        /// <summary>
        /// 停止依赖该服务的其它服务。
        /// </summary>
        /// <param name="service">主服务。</param>
        internal void StopDependencies(IService service)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            AddinDescriptor descriptor = AddinDescriptor.FindAddinDescriptor(service);
            if (descriptor == null) throw new InvalidOperationException("This service is out of control");
            Collection<AddinDescriptor> children = AddinDescriptor.GetDependencies(descriptor.Owner.FullPath);
            if (children == null) return;
            AddinDescriptor temp = null;
            IService s = null;
            for (Int32 i = 0; i < children.Count; i++)
            {
                temp = children[i];
                if (temp.BuildState != AddinBuildState.Build)
                {
                    continue;
                }
                s = temp.Addin as IService;
                if (s != null && s.State == AddinState.IncludeOrRuning)
                {
                    s.Stop();
                }
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 向其它插件发送消息。
        /// </summary>
        /// <param name="message">消息。</param>
        private void SendMessageImpl<TContent>(MessageArgs<TContent> message)
        {
            if (!_isInit) throw new InvalidOperationException("Need initialize");
            if (message.Sender != _host && message.Destination == HostTarget)
            {
                _host.OnReceive(message);
            }
            else
            {
                IEnumerable<AddinNode> nodes = GetNode(message.Destination).OfType<AddinNode>();
                if (nodes == null) return;
                foreach (AddinNode node in nodes)
                {
                    if (node == message.Sender) continue;
                    AddinDescriptor descriptor = node.Descriptor.IsValueCreated ? node.Descriptor.Value : null;
                    if (descriptor != null && descriptor.BuildState == AddinBuildState.Build)
                    {
                        IHandler<TContent> handler = descriptor.Addin as IHandler<TContent>;
                        if (handler == null) continue;
                        handler.Handle(message);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化消息调度器。
        /// </summary>
        private void InitMessageDispatcher()
        {
            _mi = this.GetType().GetMethod("SendMessageImpl", BindingFlags.NonPublic | BindingFlags.Instance);
            _messageQueue = new Queue<MessageArgs>();
            _syncEvent = new CancellationTokenSource();
            Task.Factory.StartNew(DispatchMessage, _syncEvent.Token, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 卸载插件核心方法。
        /// </summary>
        /// <param name="addinConfigFile">插件配置文件。</param>
        private void UninstalImpl(String addinConfigFile)
        {
            XElement xml = XElement.Load(ConfigFile);
            XElement removeItem = xml.Elements("Addin").SingleOrDefault(x =>
            {
                String file = x.Attribute("ref")?.Value;
                if (String.IsNullOrWhiteSpace(file)) return false;
                return AddinManager.GetFileFullPath(file) == addinConfigFile;
            });
            if (removeItem != null)
            {
                removeItem.Remove();
            }
            xml.Save(ConfigFile);
        }

        /// <summary>
        /// 创建插件树节点列表。
        /// </summary>
        /// <param name="initExposes">初始化挂载点。</param>
        private void CreateAddinTreeNodes(Tuple<String, String, String[]>[] initExposes)
        {
            XElement xml = XElement.Load(ConfigFile);
            if (xml == null) throw new FileLoadException("Invalid addin manager config file");
            AddinTreeNode node = null;
            if (initExposes != null && initExposes.Length != 0)
            {
                foreach(Tuple<String, String, String[]> point in initExposes)
                {
                    node = new VirtualNode(point.Item1, AddinTreeNode.DefaultExposePoint, point.Item2, point.Item3);
                    _nodes.Add(node.FullPath, node);
                }
            }
            Func<XElement, Boolean> filter = Filter;
            IEnumerable<XElement> loadList = null;
            if (filter != null)
            {
                loadList = xml.Elements("Addin").Where(filter);
            }
            else
            {
                loadList = xml.Elements("Addin");
            }
            foreach (XElement element in loadList)
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
            if (String.IsNullOrWhiteSpace(file)) return null;
            file = AddinManager.GetFileFullPath(file);
            if (!File.Exists(file)) return null;
            XElement xml = XElement.Load(file)?.Element("Path");
            if (xml == null) throw new FileLoadException("Invalid addin installation file");
            String mountTo = xml.Attribute("MountTo")?.Value;
            String mountExpose = xml.Attribute("MountExpose")?.Value;
            Guid guid = Guid.Parse(xml.Attribute("Guid").Value);
            String id = xml.Attribute("Id").Value;
            String[] exposes = xml.Elements("Expose")?.Attributes("Point")?.Where(x => !String.IsNullOrWhiteSpace(x.Value)).Select(x => x.Value).ToArray();
            return new AddinNode(mountTo, mountExpose, id, guid, exposes, file);
        }

        /// <summary>
        /// 生成插件树。
        /// </summary>
        private void GenerateAddinTree()
        {
            foreach (AddinTreeNode node in _nodes.Values.Skip(3).ToArray())
            {
                if (_nodes.ContainsKey(node.MountTo))
                {
                    _nodes[node.MountTo].SetChild(node, node.MountExpose);
                    continue;
                }
                _nodes.Remove(node.FullPath);
            }
            Count = _nodes.Count - 2;
        }

        /// <summary>
        /// 构建插件列表。
        /// </summary>
        /// <returns>插件列表。</returns>
        private IAddin[] BuildAddins(ReadOnlyDictionary<String,Collection<AddinTreeNode>> nodes)
        {
            Collection<AddinTreeNode> temp = null;
            Collection<IAddin> addins = new Collection<IAddin>();
            foreach(String point in nodes.Keys)
            {
                temp = nodes[point];
                foreach (AddinTreeNode node in temp)
                {
                    if (node.IsVirtual) continue;
                    addins.Add(((AddinNode)node).Buid());
                }
            }
            return addins.ToArray();
        }

        /// <summary>
        /// 构建节点的插件实例。
        /// </summary>
        /// <param name="node">节点。</param>
        /// <param name="addins">构建完成的插件。</param>
        private void BuildImpl(AddinTreeNode node, ref IList<IAddin> addins)
        {
            ReadOnlyCollection<AddinTreeNode> items = null;
            if (node.Exposes == null) return;
            foreach (String expose in node.Exposes)
            {
                items = node.GetChilds(expose);
                if (items == null) continue;
                foreach (AddinTreeNode item in items)
                {
                    if (item.IsVirtual)
                    {
                        BuildImpl(item, ref addins);
                    }
                    else
                    {
                        try
                        {
                            addins.Add(((AddinNode)item).Buid());
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取节点的子插件实例。
        /// </summary>
        /// <param name="node">节点。</param>
        /// <returns>子节点列表。</returns>
        private IAddin[] GetChildsImpl(AddinTreeNode node)
        {
            ReadOnlyCollection<AddinTreeNode> items = null;
            if (node.IsVirtual || node.Exposes == null) return new IAddin[0];
            IList<IAddin> addins = new List<IAddin>();
            Lazy<AddinDescriptor> temp = null;
            foreach (String expose in node.Exposes)
            {
                items = node.GetChilds(expose);
                if (items == null) continue;
                foreach (AddinTreeNode item in items)
                {
                    if (item.IsVirtual) continue;
                    temp = ((AddinNode)item).Descriptor;
                    if (temp.IsValueCreated && temp.Value.BuildState == AddinBuildState.Build)
                    {
                        addins.Add(temp.Value.Addin);
                    }
                }
            }
            return addins.ToArray();
        }

        /// <summary>
        /// 调度消息。
        /// </summary>
        private void DispatchMessage(Object state)
        {
            CancellationToken token = (CancellationToken)state;
            ManualResetEvent syncEvent = (ManualResetEvent)token.WaitHandle;
            MessageArgs message = null;
            while (!token.IsCancellationRequested)
            {
                syncEvent.WaitOne();
                lock (_syncObj)
                {
                    message = _messageQueue.Dequeue();
                    if (_messageQueue.Count == 0)
                    {
                        syncEvent.Reset();
                    }
                }

                _mi.MakeGenericMethod(message.Content.GetType()).Invoke(this, new Object[] { message });
            }
        }

        #endregion

        #endregion
    }
}
