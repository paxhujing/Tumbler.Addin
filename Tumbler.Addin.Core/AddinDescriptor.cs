using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tumbler.Addin.Common;

namespace Tumbler.Addin.Core
{
    /// <summary>
    /// 描述一个插件。
    /// </summary>
    internal sealed class AddinDescriptor
    {
        #region Fields

        private static readonly Dictionary<IAddin, AddinDescriptor> AddinsDescriptor = new Dictionary<IAddin, AddinDescriptor>();

        private static readonly Dictionary<String, Collection<AddinDescriptor>> DepdencieTable = new Dictionary<String, Collection<AddinDescriptor>>();

        private Type _type;

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
            if (depedencies != null)
            {
                Dependencies = depedencies.Select(x => AddinTreeNode.CompletePath(x)).ToArray();
            }
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

        /// <summary>
        /// 插件的构建状态。
        /// </summary>
        public AddinBuildState BuildState { get; private set; }

        /// <summary>
        /// 表示插件是否可以接收来自其它插件的消息。
        /// </summary>
        public Boolean CanRecieveMessage { get; private set; }

        #region AddinState

        private AddinState _addinState;

        /// <summary>
        /// 获取插件的状态。
        /// </summary>
        public AddinState AddinState
        {
            get { return _addinState; }
            internal set
            {
                if (_addinState != value)
                {
                    _addinState = value;
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
            String[] references = null;
            if (referencesAttr != null && referencesAttr.Count() != 0)
            {
                references = referencesAttr.Select(x => x.Value).ToArray();
            }
            IEnumerable<XAttribute> dependenciesAttr = xml.Element("Dependencies")?.Elements("Dependency")?.Attributes("Path");
            String[] dependencies = null;
            if (dependenciesAttr != null && dependenciesAttr.Count() != 0)
            {
                dependencies = dependenciesAttr.Select(x => x.Value).ToArray();
            }
            return new AddinDescriptor(xml.Attribute("Type")?.Value, owner, references, dependencies);
        }

        /// <summary>
        /// 解析依赖，加载程序集和类型。
        /// </summary>
        private void Analysis()
        {
            AnalysisDependencies();
            AnalysisAssemblies();
            BuildDependencies();
            BuildState = AddinBuildState.NotBuild;
        }

        /// <summary>
        /// 构建插件。
        /// </summary>
        /// <returns>代表了插件的对象，例如一个UI元素。</returns>
        public IAddin Build()
        {
            AddinBuildState state = BuildState;
            switch(state)
            {
                case AddinBuildState.Build:
                    return Addin;
                case AddinBuildState.NotBuild:
                    return GetInstance();
                case AddinBuildState.DependecyFail:
                case AddinBuildState.LoadAssemblyFail:
                case AddinBuildState.LoadTypeFail:
                    return null;
                case AddinBuildState.NotAnalysis:
                    Analysis();
                    return GetInstance();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 销毁插件实例。
        /// </summary>
        public void Destroy()
        {
            if (BuildState == AddinBuildState.Build)
            {
                RemoveDependencies();
                AddinsDescriptor.Remove(Addin);
                Addin.Dispose();
                Addin = null;
                BuildState = AddinBuildState.NotBuild;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 创建插件实例。
        /// </summary>
        /// <returns>插件实例。</returns>
        private IAddin GetInstance()
        {
            IAddin addin = (IAddin)Activator.CreateInstance(_type);
            addin.Initialize();
            AddinsDescriptor.Add(addin, this);
            Addin = addin;
            InitializeAddinState();
            BuildState = AddinBuildState.Build;
            return addin;
        }

        /// <summary>
        /// 解析插件的依赖列表。
        /// </summary>
        private void AnalysisDependencies()
        {
            if (Dependencies == null) return;
            Collection<String> unresoles = new Collection<String>();
            AddinTreeNode node = null;
            String dependency = null;
            for (Int32 i = 0; i < Dependencies.Length; i++)
            {
                dependency = Dependencies[i];
                node = AddinManager.Instance.GetNode(dependency);
                if (node == null)
                {
                    unresoles.Add(dependency);
                }
            }
            if (unresoles.Count != 0)
            {
                BuildState = AddinBuildState.DependecyFail;
                throw new AddinDependencyException(unresoles.ToArray());
            }
        }

        /// <summary>
        /// 解析插件所需程序集列表。
        /// </summary>
        private void AnalysisAssemblies()
        {
            if (References == null) return;
            LoadAssemblies();
            Type type = System.Type.GetType(Type, 
                (n)=>
                {
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    AssemblyName an = null;
                    for (Int32 i = 0; i < assemblies.Length; i++)
                    {
                        an = assemblies[i].GetName();
                        if (an.Name == n.Name || an.FullName == n.FullName)
                        {
                            return assemblies[i];
                        }
                    }
                    return null;
                },
                (a,s,b)=>
                {
                    _type = a.GetType(s);
                    if (_type == null)
                    {
                        BuildState = AddinBuildState.LoadTypeFail;
                    }
                    return _type;
                },true);
            if (type.GetInterface(typeof(IAddin).FullName) == null)
            {
                BuildState = AddinBuildState.LoadTypeFail;
                throw new TypeLoadException($"{type.FullName} must implement IAddin interface");
            }
            if (type.GetInterface(typeof(IHandler).FullName) != null)
            {
                CanRecieveMessage = true;
            }
        }

        /// <summary>
        /// 加载程序集。
        /// </summary>
        private void LoadAssemblies()
        {
            AssemblyName aname = null;
            String fullName = null;
            Collection<String> unresoles = new Collection<String>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            String file = null;
            foreach (String reference in References)
            {
                try
                {
                    file = AddinManager.GetFileFullPath(reference);
                    aname = AssemblyName.GetAssemblyName(file);
                    fullName = aname.FullName;
                    if (!assemblies.Any(x => x.FullName == fullName))
                    {
                        Assembly.LoadFrom(file);
                    }
                }
                catch (Exception)
                {
                    unresoles.Add(file);
                }
            }
            if (unresoles.Count != 0)
            {
                BuildState = AddinBuildState.LoadAssemblyFail;
                throw new AddinAssembliesException(unresoles.ToArray());
            }
        }

        /// <summary>
        /// 构建依赖项。
        /// </summary>
        private void BuildDependencies()
        {
            if (Dependencies == null) return;
            String dependency = null;
            for (Int32 i = 0; i < Dependencies.Length; i++)
            {
                dependency = Dependencies[i];
                if (!AddinDescriptor.DepdencieTable.ContainsKey(dependency))
                {
                    AddinDescriptor.DepdencieTable.Add(dependency, new Collection<AddinDescriptor>());
                }
                AddinDescriptor.DepdencieTable[dependency].Add(this);
            }
        }

        /// <summary>
        /// 初始化插件状态。
        /// </summary>
        private void InitializeAddinState()
        {
            if (Dependencies == null) return;
            AddinState state = AddinState.Unknow;
            for (Int32 i = 0; i < Dependencies.Length; i++)
            {
                state = AddinManager.Instance.GetAddinState(Dependencies[i]);
                Addin.OnDependencyStateChanged(Dependencies[i], state);
            }
        }

        /// <summary>
        /// 移除依赖。
        /// </summary>
        private void RemoveDependencies()
        {
            if (Dependencies == null) return;
            for (Int32 i = 0; i < Dependencies.Length; i++)
            {
                AddinDescriptor.DepdencieTable.Remove(Dependencies[i]);
            }
        }

        /// <summary>
        /// 状态改变时执行。
        /// </summary>
        /// <param name="newState">新状态。</param>
        private void OnStateChanged(AddinState newState)
        {
            String fullPath = this.Owner.FullPath;
            if(DepdencieTable.ContainsKey(fullPath))
            {
                Collection<AddinDescriptor> targets = DepdencieTable[fullPath];
                foreach(AddinDescriptor target in targets)
                {
                    if(target.BuildState == AddinBuildState.Build)
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
