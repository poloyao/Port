using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
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
    /// 设备类型
    /// </summary>
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
    public class PortConfig
    {
        /// <summary>
        /// 串口名称
        /// </summary>
        [Display(Name = "串口名称")]
        public string Name { get; set; }
        [Display(Name = "端口")]
        public PortIndex PortName { get; set; }
        [Display(Name = "波特率")]
        public int BaudRate { get; set; }
        [Display(Name = "数据位")]
        public int DataBits { get; set; }
        [Display(Name = "奇偶校验")]
        public Parity Parity { get; set; }
        [Display(Name = "停止位")]
        public StopBits StopBits { get; set; }

        /// <summary>
        /// 协议厂家
        /// </summary>
        [Display(Name = "协议厂家")]
        public String Protocol { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceType DeviceType { get; set; }


    }



    public class AssistDevice
    {
        public PortConfig PortConfig { get; set; }


        public int RouteNumber { get; set; }


    }

    /// <summary>
    /// 检测项目
    /// </summary>
    public class Detection
    {
        public string Name { get; set; }

        public DetectionType DetectionType { get; set; }

        public PortConfig PortConfig { get; set; }

        public List<AssistDevice> Assist { get; set; }

    }




    /// <summary>
    /// 检测项目排序
    /// </summary>
    public class DetectionOrder
    {
        public Detection Detection { get; set; }

        public int Index { get; set; }
    }
    /// <summary>
    /// 工位
    /// </summary>
    public class Station
    {
        public int Id { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工位顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        

        public List<DetectionOrder> DetectionItems { get; set; }

        //[Command(true)]
        //public void Save()
        //{
        //    //需要处理项目顺序
        //    if (DetectionItems == null)
        //    {
        //        DetectionItems = new ObservableCollection<DetectionOrder>();
        //    }
        //    DetectionItems.Add(new DetectionOrder());
        //}
    }





}
