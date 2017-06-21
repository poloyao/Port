using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Models
{
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
    /// 辅助设备类型
    /// </summary>
    public enum AssistDeviceType
    {
        /// <summary>
        /// 灯屏设备
        /// </summary>
        LatticeScreen,
        /// <summary>
        /// 光电设备
        /// </summary>
        Photoelectric,
        /// <summary>
        /// 录像设备
        /// </summary>
        Video,
        /// <summary>
        /// 拍照设备
        /// </summary>
        Camera
    }


    /// <summary>
    /// 辅助设备
    /// </summary>
    public class AssistDeviceModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 辅助设备类型
        /// </summary>
        public AssistDeviceType AssistType { get; set; }

        public PortConfigModel config { get; set; }
        

    }

    /// <summary>
    /// 辅助设备排序
    /// </summary>
    public class AssistDeviceOrder
    {
        public AssistDeviceModel AssistDevice { get; set; }

        public int Index { get; set; }
    }

    /// <summary>
    /// 检测类型
    /// </summary>
    public enum DetectionType
    {
        /// <summary>
        /// 外检
        /// </summary>
        Shape,
        /// <summary>
        /// 侧滑
        /// </summary>
        SideSlide,
        /// <summary>
        /// 速度
        /// </summary>
        Speed,
        /// <summary>
        /// 灯光
        /// </summary>
        Light,
        /// <summary>
        /// 制动
        /// </summary>
        Brake,
        /// <summary>
        /// 称重
        /// </summary>
        Weigh,
        /// <summary>
        /// 地盘
        /// </summary>
        bottom,
        /// <summary>
        /// 地盘间隙
        /// </summary>
        BoottomInterval,
        /// <summary>
        /// 声级计
        /// </summary>
        SoundLevel,
        /// <summary>
        /// 功率
        /// </summary>
        Power,
        /// <summary>
        /// 油耗
        /// </summary>
        FuelConsumption,
        /// <summary>
        /// 尾气
        /// </summary>
        Exhaust
    }


    /// <summary>
    /// 检测项目
    /// </summary>
    public class DetectionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DetectionType DType { get; set; }

        public PortConfigModel config { get; set; }

        public List<AssistDeviceOrder> AssistList { get; set; } = new List<AssistDeviceOrder>();


    }

    /// <summary>
    /// 检测项目排序
    /// </summary>
    public class DetectionOrder
    {
        public DetectionModel Detection { get; set; }

        public int Index { get; set; }
    }
    
}
