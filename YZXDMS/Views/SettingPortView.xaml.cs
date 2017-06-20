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
    /// Interaction logic for SettingPortView.xaml
    /// </summary>
    public partial class SettingPortView : UserControl
    {
        public SettingPortView()
        {
            InitializeComponent();
        }

        private void grid_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Port")
            {
                e.DisplayText = ((System.IO.Ports.SerialPort)e.Value).PortName;
            }
        }
    }
}
