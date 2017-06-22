﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

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
    public class AssistDevice
    {
        public PortConfig PortConfig { get; set; }

        public AssistDeviceType DeviceType { get; set; }

        public int RouteTotal { get; set; }

        //public int UseOrder { get; set; }


    }

    public class AssistRoute
    {
        public AssistDevice Assist { get; set; }

        public int RouteNumber { get; set; }

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
        public List<AssistRoute> Assist { get; set; }

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
