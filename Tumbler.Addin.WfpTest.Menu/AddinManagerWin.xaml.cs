using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Tumber.Addin.Common;
using Tumbler.Addin.Core;

namespace Tumbler.Addin.WfpTest.Menu
{
    /// <summary>
    /// AddinManagerWin.xaml 的交互逻辑
    /// </summary>
    public partial class AddinManagerWin : Window
    {
        private ObservableCollection<AddinTreeNode> _addins;

        public AddinManagerWin()
        {
            InitializeComponent();
            IEnumerable<AddinTreeNode> installedAddins = AddinManager.Instance.GetInstallAddinInfos();
            _addins = new ObservableCollection<AddinTreeNode>(installedAddins);
            InstallAddins.ItemsSource = _addins;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Button btn = (Button)sender;
            AddinNode node = btn.DataContext as AddinNode;
            if (node == null) return;
            AddinManager.Instance.Uninstall(node);
        }
    }
}
