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
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using YZXDMS.ViewModels;

namespace YZXDMS
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : DXWindow
    {
        public Login()
        {
            InitializeComponent();
            (this.DataContext as LoginViewModel).LoginSucess += Login_LoginSucess;
        }

        private void Login_LoginSucess(object sender, EventArgs e)
        {
            MainWindow wm = new MainWindow();
            this.Close();
            wm.Show();
        }
    }
}
