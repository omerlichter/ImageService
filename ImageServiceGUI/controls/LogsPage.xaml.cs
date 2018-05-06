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
using ImageServiceGUI.ViewModel;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.controls
{
    /// <summary>
    /// Interaction logic for LogsPage.xaml
    /// </summary>
    public partial class LogsPage : UserControl
    {
        private IVMLogPage vm_logPage;

        public LogsPage()
        {
            InitializeComponent();
            this.vm_logPage = new VMLogPage(new ModelLogPage());
            this.DataContext = this.vm_logPage;
        }
    }
}
