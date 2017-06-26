using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections.ObjectModel;

namespace YZXDMS.Models
{
    /// <summary>
    /// 串口端口
    /// </summary>
    public enum PortIndex
    {
        COM1,
        COM2,
        COM3,
        COM4,
        COM5,
        COM6,
        COM7,
        COM8,
        COM9,
        COM10,
        COM11,
        COM12,
        COM13,
        COM14,
        COM15,
        COM16,
        COM17,
        COM18,
        COM19,
        COM20,
        COM21,
        COM22,
        COM23,
        COM24,
        COM25,
        COM26,
        COM27,
        COM28,
        COM29,
        COM30,
        COM31,
        COM32,

    }

    /// <summary>
    /// 启动模式
    /// </summary>
    public enum StartMode
    {
        保持开启,
        即用即关
    }

    /// <summary>
    /// 设备性质
    /// </summary>
    public enum DeviceProperty
    {
        /// <summary>
        /// 辅助设备
        /// </summary>
        辅助设备,
        /// <summary>
        /// 检测设备
        /// </summary>
        检测设备
    }

    /// <summary>
    /// 设备类型,
    /// 100之前为主设备，100之后未辅助设备
    /// </summary>
    [Flags]
    public enum DeviceType
    {
        外检 = 0,
        侧滑,
        速度,
        灯光,
        制动,
        称重,
        底盘,
        底盘间隙,
        声级计,
        功率,
        油耗,
        尾气,
        探平衡仪,

        灯屏设备 = 101,
        光电设备 = 102,
        录像设备 = 103,
        拍照设备 = 104,
    }
    
    /// <summary>
    /// 检测类型
    /// </summary>
    public enum DetectionType
    {
        外检 = 0,
        侧滑,
        速度,
        灯光,
        制动,
        称重,
        底盘,
        底盘间隙,
        声级计,
        功率,
        油耗,
        尾气,
        探平衡仪,
    }
    

    /// <summary>
    /// 串口信息
    /// </summary>
    public class PortConfig:ICloneable
    {
        /// <summary>
        /// 串口名称
        /// </summary>
        [Display(Name = "串口名称")]
        public virtual string Name { get; set; }
        [Display(Name = "端口")]
        public PortIndex PortName { get; set; }
        [Display(Name = "波特率")]
        public int BaudRate { get; set; }
        [Display(Name = "数据位")]
        public int DataBits { get; set; } = 8;
        [Display(Name = "奇偶校验")]
        public Parity Parity { get; set; }
        [Display(Name = "停止位")]
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// 通道数量
        /// </summary>
        [Display(Name = "通道数量")]

        public  int RouteTotal { get; set; } = 1;
        /// <summary>
        /// 协议厂家
        /// </summary>
        [Display(Name = "协议厂家")]
        public String Protocol { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        [Display(Name = "设备类型")]
        public DeviceType DeviceType { get; set; }

        public object Clone()
        {
            var result = new PortConfig();
            result.Name = this.Name;
            result.PortName = this.PortName;
            result.BaudRate = this.BaudRate;
            result.DataBits = this.DataBits;
            result.Parity = this.Parity;
            result.StopBits = this.StopBits;
            result.Protocol = this.Protocol;
            result.DeviceType = this.DeviceType;
            result.RouteTotal = this.RouteTotal;

            return result;
        }

        public static PortConfig Create()
        {
            var result = new PortConfig();
            result.Name = "默认";
            result.PortName = PortIndex.COM1;
            result.BaudRate = 9600;
            result.DataBits = 8;
            result.Parity = Parity.None;
            result.StopBits = StopBits.One;
            result.Protocol = "无";
            result.DeviceType = DeviceType.光电设备;

            result.RouteTotal = 1;

            return result;
        }
    }




    /// <summary>
    /// 辅助设备类型
    /// </summary>
    public enum AssistDeviceType
    {
        灯屏设备,
        光电设备,
        录像设备,
        拍照设备
        ///// <summary>
        ///// 灯屏设备
        ///// </summary>
        //LatticeScreen,
        ///// <summary>
        ///// 光电设备
        ///// </summary>
        //Photoelectric,
        ///// <summary>
        ///// 录像设备
        ///// </summary>
        //Video,
        ///// <summary>
        ///// 拍照设备
        ///// </summary>
        //Camera
    }


    /// <summary>
    /// 辅助设备
    /// </summary>
    public class AssistDevice:ICloneable
    {
        /// <summary>
        /// 串口配置
        /// </summary>
        public PortConfig PortConfig { get; set; }
        /// <summary>
        /// 辅助类型
        /// </summary>
        [Display(Name = "辅助设备类型")]
        public AssistDeviceType DeviceType { get; set; }

        ///// <summary>
        ///// 支持路数
        ///// </summary>
        //public int RouteTotal { get; set; }

        //public int UseOrder { get; set; }


        public object Clone()
        {
            AssistDevice result = new AssistDevice();
            result.DeviceType = this.DeviceType;
            result.PortConfig = (PortConfig)this.PortConfig.Clone();
            return result;
        }

    }
    

    public class AssistRoute:ICloneable
    {
        ///// <summary>
        ///// 辅助设备,待移除
        ///// </summary>
        //public AssistDevice Assist { get; set; }

        /// <summary>
        /// 串口配置
        /// </summary>
        public PortConfig PortConfig { get; set; }

        /// <summary>
        /// 调用通道
        /// </summary>
        [Display(Name = "调用通道")]
        public  int RouteNumber { get; set; }

        public object Clone()
        {
            AssistRoute result = new AssistRoute();
            //需要关联，直接引用
            //result.Assist = this.Assist;//(AssistDevice)this.Assist.Clone();
            result.PortConfig = this.PortConfig;
            result.RouteNumber = this.RouteNumber;
            return result;
        }
    }

    /// <summary>
    /// 检测项目
    /// </summary>
    public class Detection
    {
        public string Name { get; set; }
        /// <summary>
        /// 检测类型
        /// </summary>
        public DetectionType DetectionType { get; set; }
        /// <summary>
        /// 串口配置信息
        /// </summary>
        public PortConfig PortConfig { get; set; }
        /// <summary>
        /// 辅助设备
        /// </summary>
        public ObservableCollection<AssistRoute> AssistList { get; set; } = new ObservableCollection<AssistRoute>();

      

    }




    /// <summary>
    /// 检测项目排序
    /// </summary>
    public class DetectionOrder
    {
        public Detection Detection { get; set; }

        public int Index { get; set; }
    }
}
