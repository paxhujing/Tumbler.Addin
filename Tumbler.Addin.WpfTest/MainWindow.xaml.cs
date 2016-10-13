﻿using System;
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
using Tumbler.Addin.Core;

namespace Tumbler.Addin.WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private AddinManager _manager = AddinManager.Instance;

        public MainWindow()
        {
            InitializeComponent();
            _manager.Initialize(@"E:\Tumbler.Addin\Tumbler.Addin.WpfTest\Addins.xml");
            IAddin addin = _manager.BuildFirstLevelAddins()[0];
            MenuPanel.Content = addin;
        }
    }
}