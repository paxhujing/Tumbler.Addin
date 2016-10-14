using Microsoft.Win32;
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
using Tumbler.Addin.Common;
using Tumbler.Addin.Core;

namespace Tumbler.Addin.WfpTest.Menu
{
    /// <summary>
    /// AddinManagerWin.xaml 的交互逻辑
    /// </summary>
    public partial class AddinManagerWin : Window
    {
        private ObservableCollection<AddinBaseInfo> _addins;

        public AddinManagerWin()
        {
            InitializeComponent();
            IEnumerable<AddinBaseInfo> installedAddins = AddinManager.Instance.GetInstallAddinInfos();
            _addins = new ObservableCollection<AddinBaseInfo>(installedAddins);
            InstallAddins.ItemsSource = _addins;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Button btn = (Button)sender;
            AddinBaseInfo info = (AddinBaseInfo)btn.DataContext;
            AddinManager.Instance.Uninstall(info);
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "插件配置文件(*.addin)|*.addin";
            dialog.Multiselect = true;
            Boolean? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                AddinBaseInfo info = null;
                foreach (String fileName in dialog.FileNames)
                {
                    info = AddinBaseInfo.Parse(fileName);
                    _addins.Add(info);
                    AddinManager.Instance.Install(fileName);
                }
            }
        }
    }
}
