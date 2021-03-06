﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YZXDMS.Views
{
    /// <summary>
    /// Interaction logic for QueryTestLineView.xaml
    /// </summary>
    public partial class QueryTestLineView : UserControl
    {
        public QueryTestLineView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window owner = DevExpress.Xpf.Core.Native.LayoutHelper.FindParentObject<Window>(this);
            DevExpress.Xpf.Grid.Printing.PrintHelper.ShowPrintPreviewDialog(owner, this.gird.View);
        }
    }
}
