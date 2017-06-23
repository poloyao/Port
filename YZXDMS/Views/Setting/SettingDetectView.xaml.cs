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





        public int Mylala
        {
            get { return (int)GetValue(MylalaProperty); }
            set { SetValue(MylalaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mylala.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MylalaProperty =
            DependencyProperty.Register("Mylala", typeof(int), typeof(SettingDetectView), new PropertyMetadata(10,OnMylala));

        private static void OnMylala(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public static int GetMyInt2(DependencyObject obj)
        {
            return (int)obj.GetValue(MyInt2Property);
        }

        public static void SetMyInt2(DependencyObject obj, int value)
        {
            obj.SetValue(MyInt2Property, value);
        }

        // Using a DependencyProperty as the backing store for MyInt2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyInt2Property =
            DependencyProperty.RegisterAttached("MyInt2", typeof(int), typeof(SettingDetectView), new PropertyMetadata(0,OnMyInit2Changed));

        private static void OnMyInit2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }




        public static object GetMyDetection(DependencyObject obj)
        {
            return (Detection)obj.GetValue(MyDetectionProperty);
        }

        public static void SetMyDetection(DependencyObject obj, object value)
        {
            obj.SetValue(MyDetectionProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyDetection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyDetectionProperty =
            DependencyProperty.RegisterAttached("MyDetection", typeof(object), typeof(SettingDetectView), new PropertyMetadata(null,OnMyDetectionChanged));

        private static void OnMyDetectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
