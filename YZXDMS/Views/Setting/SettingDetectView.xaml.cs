using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
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
using YZXDMS.Models;

namespace YZXDMS.Views
{
    /// <summary>
    /// Interaction logic for SettingDetectView.xaml
    /// </summary>
    public partial class SettingDetectView : UserControl
    {

        public SettingDetectView()
        {
            InitializeComponent();
            //DataContext = this;
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            lg.UpdateLayout();
        }


        //public object MyDetection
        //{
        //    get { return (object)GetValue(MyDetectionProperty); }
        //    set { SetValue(MyDetectionProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyDetection.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MyDetectionProperty =
        //    DependencyProperty.Register("MyDetection", typeof(object), typeof(SettingDetectView), new PropertyMetadata(null,OnMyDetectionChanged));

        //private static void OnMyDetectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
