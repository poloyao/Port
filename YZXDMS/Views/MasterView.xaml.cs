using System;
using System.Collections.Generic;
using System.Globalization;
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
using YZXDMS.ViewModels;

namespace YZXDMS.Views
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : UserControl
    {
        public MasterView()
        {
            InitializeComponent();
            //List<int> items = new List<int>();
            //for (int i = 0; i < 5; i++)
            //{
            //    items.Add(i);
            //}
            //this.grid.ItemsSource = items;
        }

        private void grid_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            DetectResultStatus temp;
            var ee = e.DisplayText;
            switch (e.DisplayText)
            {
                case "Wait":
                    e.DisplayText = "/YZXDMS;component/Img/car2.png";
                    break;
                case "Qualified":
                    break;
                case "Unqualified":
                    break;
                case "NotChecked":
                    break;
                default:
                    break;
            }
            //var sss = (DetectResultStatus)e.DisplayText
            //if (Guid.TryParse(e.DisplayText, out temp))
            //{
            //    try
            //    {
            //        var dt = Common.SingleTypeCode.GetInstance().GetCommonCode(e.Value.ToString()).Name;
            //        e.DisplayText = dt;
            //    }
            //    catch (Exception)
            //    {
            //        Helpers.MessageHelper.DisplayTextConversionFailMessage(e.DisplayText);
            //        e.DisplayText = string.Empty;
            //    }
            //}
        }
    }


    public class ResultConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
