using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Models
{


    /// <summary>
    /// 启动模式
    /// </summary>
    public enum StartMode
    {
        ///// <summary>
        ///// 维持现状。
        ///// </summary>
        //KeepStatus,
        ///// <summary>
        ///// 保持开启。常开状态，直接读取、调用，如光电、录像等辅助设备
        ///// </summary>
        //HoldOpen,
        ///// <summary>
        ///// 用后即关。
        ///// 不用时关闭.
        ///// </summary>
        //AfterClose
        保持开启,
        即用即关
    }

    /// <summary>
    /// 串口配置文件
    /// </summary>
    public class PortConfigModel:ICloneable
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        public string Name { get; set; }
        /// <summary>
        /// 串口
        /// </summary>
        [Display(Name = "串口")]
        public SerialPort Port { get; set; }
        /// <summary>
        /// 设备性质
        /// </summary>
        [Display(Name = "设备性质")]
        public DeviceProperty DeviceProperty { get; set; }
        /// <summary>
        /// 协议厂家
        /// </summary>
        [Display(Name = "协议厂家")]
        public String Protocol { get; set; }
        /// <summary>
        /// 启动模式
        /// </summary>
        [Display(Name = "启动模式")]
        public StartMode StartMode { get; set; }

        public object Clone()
        {
            PortConfigModel item = new PortConfigModel();
            item.Name = this.Name;
            item.DeviceProperty = this.DeviceProperty;
            item.Protocol = this.Protocol;
            item.StartMode = this.StartMode;
            if (this.Port != null)
                item.Port = new SerialPort(
                    this.Port.PortName,
                    this.Port.BaudRate,
                    this.Port.Parity,
                    this.Port.DataBits,
                    this.Port.StopBits);


            return item;
        }
    }

    

    /// <summary>
    /// 协议
    /// </summary>
    public class ProtocolModel
    {
        /// <summary>
        /// 协议名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 协议ID
        /// </summary>
        public int ID { get; set; }
    }
}
