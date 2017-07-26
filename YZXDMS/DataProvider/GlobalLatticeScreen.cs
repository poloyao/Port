using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;

namespace YZXDMS.DataProvider
{
    /// <summary>
    /// 全局点阵
    /// </summary>
    public class GlobalLatticeScreen
    {
        //private static Queue<PhotoelectricModel> items { get; set; } = new Queue<PhotoelectricModel>();

        private static readonly GlobalLatticeScreen instance = new GlobalLatticeScreen();

        public static GlobalLatticeScreen GetInstance()
        {
            return instance;
        }

        private GlobalLatticeScreen()
        {

        }

        ///// <summary>
        ///// 设置指定灯屏的显示内容
        ///// </summary>
        ///// <param name="device"></param>
        ///// <param name="message"></param>
        //public static void SetMessageInfo(AssistDeviceModel device,  string message)
        //{

        //}

    }


}
