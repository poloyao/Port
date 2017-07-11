using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Detections
{
    /// <summary>
    /// 点阵操作接口
    /// </summary>
    public interface ILatticeScreenOperate
    {
        /// <summary>
        /// 设置串口
        /// </summary>
        /// <param name="sp"></param>
        void SetPort(SerialPort sp);
        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="mess"></param>
        void SetMessage(string mess);
        /// <summary>
        /// 获取当前显示信息
        /// </summary>
        /// <returns></returns>
        string GetMessage();
    }

    /// <summary>
    /// 伪点阵实例
    /// </summary>
    public class LatticeScreenOperate : ILatticeScreenOperate
    {

        SerialPort _sp;

        public void SetPort(SerialPort sp)
        {
            this._sp = sp;
        }

        public void SetMessage(string mess)
        { }

        public string GetMessage()
        {
            return "";
        }

    }
}
