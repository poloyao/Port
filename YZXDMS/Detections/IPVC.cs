using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Detections
{
    /// <summary>
    /// 光电设备接口
    /// </summary>
    public interface IPVC
    {
        /// <summary>
        /// 设置串口
        /// </summary>
        /// <param name="sp"></param>
        void SetPort(SerialPort sp);


    }
}
