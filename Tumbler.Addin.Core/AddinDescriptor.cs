using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 描述一个插件。
    /// </summary>
    internal sealed class AddinDescriptor
    {
        #region Fields

        private static readonly Dictionary<IAddin, AddinDescriptor> AddinsDescriptor = new Dictionary<IAddin, AddinDescriptor>();

        private static readonly Dictionary<String, Collection<AddinDescriptor>> Depdencies = new Dictionary<String, Collection<AddinDescriptor>>();

        private Boolean _isAnalysisPassed;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 Tumbler.Addin.Core.AddinDescriptor 实例。
        /// </summary>
        /// <param name="type">实现了IAddin接口的类型名称。</param>
        /// <param name="owner">拥有此描述符的插件节点。</param>
        /// <param name="references"></param>
        /// <param name="depedencies"></param>
        private AddinDescriptor(String type, AddinTreeNode owner, String[] references, String[] depedencies)
        {
            if (String.IsNullOrWhiteSpace(type)) throw new ArgumentNullException("type");
            if (owner == null) throw new ArgumentNullException("owner");
            Owner = owner;
            Type = type;
            References = references;
            Dependencies = depedencies.Select(x => AddinTreeNode.CompletePath(x)).ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取插件需要引用的程序集列表。
        /// </summary>
        public String[] References { get; }

        /// <summary>
        /// 获取插件依赖的其它插件列表。
        /// </summary>
        public String[] Dependencies { get; }

        /// <summary>
        /// 拥有此描述符的插件节点。
        /// </summary>
        public AddinTreeNode Owner { get; }

        /// <summary>
        /// 获取实现了IAddin接口的类型名称。
        /// </summary>
        public String Type { get; }

        /// <summary>
        /// 获取描述的插件对象。
        /// </summary>
        public IAddin Addin { get; private set; }

        #region AddinState

        private AddinState _state;

        /// <summary>
        /// 获取插件的状态。
        /// </summary>
        public AddinState State
        {
            get { return _state; }
            private set
            {
                if(_state != value)
                {
                    _state = value;
                    OnStateChanged(value);
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// 获取插件的对应的描述器。
        /// </summary>
        /// <param name="addin">插件。</param>
        /// <returns>插件的描述器。</returns>
        public static AddinDescriptor FindAddinDescriptor(IAddin addin)
        {
            if (AddinsDescriptor.ContainsKey(addin))
            {
                return AddinsDescriptor[addin];
            }
            return null;
        }

        /// <summary>
        /// 根据插件的配置文件得到插件的描述。
        /// </summary>
        /// <param name="configFile">插件的置文件。</param>
        /// <param name="owner">拥有此描述符的插件节点。</param>
        /// <returns>插件的描述。</returns>
        public static AddinDescriptor Parse(String configFile, AddinTreeNode owner)
        {
            if (!File.Exists(configFile)) throw new FileNotFoundException(configFile);
            XElement xml = XElement.Load(configFile)?.Element("Runtimes");
            if (xml == null) throw new FileLoadException("Invalid addin config file");
            IEnumerable<XAttribute> referencesAttr = xml.Element("Assemblies")?.Elements("Reference")?.Attributes("Path");
            String[] references = referencesAttr?.Select(x => x.Value).ToArray() ?? new String[0];
            IEnumerable<XAttribute> dependenciesAttr = xml.Element("Dependencies")?.Elements("Dependecy")?.Attributes("Path");
            String[] dependencies = dependenciesAttr?.Select(x => x.Value).ToArray() ?? new String[0];
            return new AddinDescriptor(xml.Attribute("Type")?.Value,owner, references, dependencies);
        }

        /// <summary>
        /// 构建插件。
        /// </summary>
        /// <returns>代表了插件的对象，例如一个UI元素。</returns>
        public IAddin Build()
        {
            if (State == AddinState.None)
            {
                IAddin addin = LoadAddin();
                AddinsDescriptor.Add(addin, this);
                Addin = addin;
                State = AddinState.Build;
            }
            return Addin;
        }

        /// <summary>
        /// 销毁插件实例。
        /// </summary>
        public void Destroy()
        {
            if (State != AddinState.None)
            {
                AddinsDescriptor.Remove(Addin);
                Addin.Dispose();
                Addin = null;
                State = AddinState.None;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 加载插件。
        /// </summary>
        /// <returns>插件实例。</returns>
        private IAddin LoadAddin()
        {
            if(!_isAnalysisPassed)
            {
                AnalysisDependencies();
                AnalysisAssemblies();

            }
            _isAnalysisPassed = true;
            return CreateInstance();
        }

        /// <summary>
        /// 解析插件的依赖列表。
        /// </summary>
        private void AnalysisDependencies()
        {
            if (Dependencies.Length == 0) return;
            AddinManager manager = Owner.Owner;
            Collection<String> unresoles = new Collection<String>();
            AddinTreeNode node = null;
            foreach (String dependency in Dependencies)
            {
                node = manager.GetNode(dependency);
                if (node == null)
                {
                    unresoles.Add(dependency);
                }
            }
            if (unresoles.Count != 0)
            {
                throw new AddinDependencyException(unresoles.ToArray());
            }
        }

        /// <summary>
        /// 解析插件所需程序集列表。
        /// </summary>
        private void AnalysisAssemblies()
        {
            if (References.Length == 0) return;
            AssemblyName aname = null;
            String fullName = null;
            Collection<String> unresoles = new Collection<String>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (String reference in References)
            {
                try
                {
                    aname = AssemblyName.GetAssemblyName(reference);
                    fullName = aname.FullName;
                    if (!assemblies.Any(x => x.FullName == fullName))
                    {
                        Assembly.LoadFrom(reference);
                    }
                }
                catch (Exception)
                {
                    unresoles.Add(reference);
                }
            }
            if (unresoles.Count == 0)
            {
                throw new AddinAssembliesException(unresoles.ToArray());
            }
            Type type = System.Type.GetType(Type);
            if (type == null) throw new TypeLoadException(Type);
            if (type.GetInterface(typeof(IAddin).FullName) == null)
            {
                throw new TypeLoadException($"{type.FullName} must implement IAddin interface");
            }
        }

        /// <summary>
        /// 构建依赖项。
        /// </summary>
        private void BuildDependencies()
        {
            foreach(String dependency in Dependencies)
            {
                if (!AddinDescriptor.Depdencies.ContainsKey(dependency))
                {
                    AddinDescriptor.Depdencies.Add(dependency, new Collection<AddinDescriptor>());
                }
                AddinDescriptor.Depdencies[dependency].Add(this);
            }
        }

        /// <summary>
        /// 创建插件实例。
        /// </summary>
        /// <returns>件实例。</returns>
        private IAddin CreateInstance()
        {
            Type type = System.Type.GetType(Type);
            return (IAddin)Activator.CreateInstance(type);
        }

        /// <summary>
        /// 状态改变时执行。
        /// </summary>
        /// <param name="newState">新状态。</param>
        private void OnStateChanged(AddinState newState)
        {
            String fullPath = this.Owner.FullPath;
            if(Depdencies.ContainsKey(fullPath))
            {
                Collection<AddinDescriptor> targets = Depdencies[fullPath];
                foreach(AddinDescriptor target in targets)
                {
                    if(target.State == AddinState.Build)
                    {
                        target.Addin.OnDependencyStateChanged(fullPath, newState);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
