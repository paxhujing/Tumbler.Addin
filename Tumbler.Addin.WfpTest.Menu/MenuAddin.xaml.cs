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
using Tumber.Addin.Common;

namespace Tumbler.Addin.WfpTest.Menu
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MenuAddin : System.Windows.Controls.Menu, IAddin
    {
        public MenuAddin()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }

        public void Execute()
        {
        }

        public void Initialize()
        {
            ToolTip = "This is a menu";
        }

        public void OnDependencyStateChanged(String fullPath, AddinState? state)
        {
            if (state.HasValue)
            {
                switch (state.Value)
                {
                    case AddinState.Disable:
                        IsEnabled = false;
                        break;
                    case AddinState.Enable:
                        IsEnabled = true;
                        break;
                    case AddinState.Exclude:
                        Visibility = Visibility.Collapsed;
                        break;
                    case AddinState.Include:
                        Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                IsEnabled = false;
            }
        }
    }
}
