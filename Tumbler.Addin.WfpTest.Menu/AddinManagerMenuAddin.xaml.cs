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
using Tumbler.Addin.Core;

namespace Tumbler.Addin.WfpTest.Menu
{
    /// <summary>
    /// AddinManagerMenuAddin.xaml 的交互逻辑
    /// </summary>
    public partial class AddinManagerMenuAddin : MenuItem, IAddin
    {
        public AddinManagerMenuAddin()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }

        public void Execute()
        {
            AddinManager.Instance.SendMessage("./Addins/Menu", new System.Collections.Hashtable());
            AddinManagerWin win = new AddinManagerWin();
            win.ShowDialog();
        }

        public void Initialize()
        {
        }

        public void OnDependencyStateChanged(string fullPath, AddinState? state)
        {
            MessageBox.Show($"{fullPath} state changed {state}");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Execute();
        }
    }
}
