using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tumbler.Addin.Common;

namespace Tumbler.Addin.WfpTest.Menu
{
    /// <summary>
    /// ExportAddin.xaml 的交互逻辑
    /// </summary>
    public partial class ExportAddin : MenuItem,IAddin
    {
        public ExportAddin()
        {
            InitializeComponent();
        }

        public string MountPoint { get; private set; }

        public void Dispose()
        {
        }

        public void Execute()
        {
        }

        public void Initialize(string mountPoint, String[] exposes)
        {
            MountPoint = mountPoint;
        }

        public void OnDependencyStateChanged(string fullPath, AddinState? state)
        {
        }
    }
}
