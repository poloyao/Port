using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class LoginViewModel
    {
        public event EventHandler LoginSucess;

        public virtual string Account { get; set; } = "admin";

        public virtual string Pwd { get; set; } = "admin";

        public virtual string Message { get; set; }

        public void Login()
        {
            if (Pwd != null && Pwd.Length > 0)
            {
                try
                {
                    var query = Core.Core.GetDBProvider().Login(Account, Pwd);
                    if (query.IsSuccess == true)
                    {
                        //成功后操作此动作
                        Core.Core.User = query.User;
                        LoginSucess(this, EventArgs.Empty);

                        return;
                    }
                    else
                    {
                        Message = query.Message;
                    }
                }
                catch (Exception)
                {

                    Message = "连接失败";
                }

            }
        }
    }
}
