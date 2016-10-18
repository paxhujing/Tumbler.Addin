using System;
using System.Collections;
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
using Tumbler.Addin.Core;

namespace Tumbler.Addin.WfpTest.Menu
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MenuAddin : System.Windows.Controls.Menu, IAddin ,IHandler
    {
        public MenuAddin()
        {
            InitializeComponent();
        }

        public string MountExpose { get; private set; }

        public void Dispose()
        {
        }

        public void Execute()
        {
            IAddin[] addins = this.BuildChildAddins();
            foreach (IAddin addin in addins)
            {
                switch(addin.MountExpose)
                {
                    case "File":
                        FileMenu.Items.Add(addin);
                        break;
                    default:
                        AddChild(addin);
                        break;
                }
            }
        }

        public void Handle(Object message)
        {
            //MessageBox.Show("Hadle request");
        }

        public void Initialize(String mountPoint, String[] exposes)
        {
            MountExpose = mountPoint;
            ToolTip = "This is a menu";
        }

        public void OnDependencyStateChanged(String fullPath, AddinState state)
        {
            switch (state)
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
                    IsEnabled = false;
                    break;
            }
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Execute();
            //IsEnabled = false;
            //AddinManager.Instance.SetAddinState(this, AddinState.Disable);
        }
    }
}
