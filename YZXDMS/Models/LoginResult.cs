using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Model;

namespace YZXDMS.Models
{
    /// <summary>
    /// 登录返回信息
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// user信息
        /// </summary>
        public Users User { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string Message { get; set; }


    }
}
