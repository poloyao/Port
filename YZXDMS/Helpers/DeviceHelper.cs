using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;
using System.IO.Ports;

namespace YZXDMS.Helper
{
    /// <summary>
    /// 设备帮助类
    /// </summary>
    public class DeviceHelper
    {
        private static List<Detection> DetectionItems { get; set; } = new List<Detection>();

        //private static List<SerialPort> PortItems = new List<SerialPort>();

        /// <summary>
        /// 检测项目串口信息
        /// </summary>
        private static List<DetectionPort> detectPortItems = new List<DetectionPort>();

        /// <summary>
        /// 辅助设备串口信息，多路同串口公用一个
        /// </summary>
        public static List<AssistPort> AssistPortItems { get; private set; } = new List<AssistPort>();

        private static readonly DeviceHelper instance = new DeviceHelper();

        public static DeviceHelper GetInstance()
        {
            return instance;
        }

        private DeviceHelper()
        {
            InitPort();
            
        }

        void InitPort()
        {


            #region 模拟生成conf信息

            DetectionItems.Add(new Detection()
            {
                Name = "速度",
                DetectionType = DetectionType.速度,
                PortConfig = new PortConfig()
                {
                    Name = "速度",
                    Protocol = "速度协议",
                    PortName = PortIndex.COM10,
                    BaudRate = 9600,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    DeviceType = DeviceType.速度,

                },
                Assist = new List<AssistRoute>()
                {
                    new AssistRoute()
                    {
                        RouteNumber = 1,
                        Assist = new AssistDevice()
                        {
                            DeviceType = AssistDeviceType.Photoelectric,
                            PortConfig = new PortConfig()
                            {
                                    Name = "光电",
                                Protocol = "光电协议",
                                PortName = PortIndex.COM1,
                                BaudRate  = 9600,
                                Parity = Parity.None,
                                DataBits = 8,
                                StopBits = StopBits.One,
                                DeviceType = DeviceType.光电设备,
                            },
                            RouteTotal = 8
                        }
                    }
                }
            });

            #endregion

            #region 生成port列表


            foreach (var item in DetectionItems)
            {
                var conf = item.PortConfig;
                SerialPort sp = new SerialPort();
                sp.PortName = conf.PortName.ToString();
                sp.BaudRate = conf.BaudRate;
                sp.Parity = conf.Parity;
                sp.DataBits = conf.DataBits;
                sp.StopBits = conf.StopBits;

                detectPortItems.Add(new DetectionPort()
                {
                    Detection = item,
                    Port = sp
                });

                foreach (var assist in item.Assist)
                {
                    var aConf = assist.Assist.PortConfig;

                    //保证辅助设备多路公用一个port
                    var quaryAssist = AssistPortItems.SingleOrDefault(x => x.Assist == assist.Assist);
                    if (quaryAssist != null)
                    {
                        continue;
                     }

                    SerialPort assistsp = new SerialPort();
                    assistsp.PortName = aConf.PortName.ToString();
                    assistsp.BaudRate = aConf.BaudRate;
                    assistsp.Parity = aConf.Parity;
                    assistsp.DataBits = aConf.DataBits;
                    assistsp.StopBits = aConf.StopBits;
                    
                    AssistPortItems.Add(new AssistPort()
                    {
                        Port = assistsp,
                        Assist = assist.Assist
                    });
                }
            }




            #endregion

            #region Delete



            //var port = new System.IO.Ports.SerialPort("COM12", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);



            //Detections.Add(new Detection()
            //{
            //    Name = "速度",
            //    DType = DetectionType.Speed,
            //    config = new PortConfigModel()
            //    {
            //        DeviceProperty = DeviceProperty.检测设备,
            //        StartMode = StartMode.即用即关,
            //        Protocol = "XX协议",
            //        Port = new System.IO.Ports.SerialPort("COM10", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
            //    },
            //    AssistList = new List<AssistDeviceOrder>()
            //    {
            //        new AssistDeviceOrder()
            //        {
            //            AssistDevice = new AssistDeviceModel()
            //            {
            //                AssistType = AssistDeviceType.Photoelectric,
            //                Name = "速度光电",
            //                config = new PortConfigModel()
            //                {
            //                    DeviceProperty = DeviceProperty.辅助设备,
            //                    StartMode = StartMode.保持开启,
            //                    Protocol = "XX协议",
            //                    Port = port
            //                }
            //            }
            //        }
            //    }
            //});

            #endregion

        }

        /// <summary>
        /// 获取指定检测项目串口信息
        /// </summary>
        /// <param name="detection"></param>
        /// <returns></returns>

        public static DetectionPort GetDetectionInfo(Detection detection)
        {
            var query = detectPortItems.Single(x => x.Detection == detection);
            return query;
        }

        /// <summary>
        /// 获取检测项目
        /// </summary>
        /// <param name="detectionType"></param>
        /// <returns></returns>
        public static Detection GetDetection(DetectionType detectionType)
        {
            var query = DetectionItems.Single(x => x.DetectionType == detectionType);
            return query;
        }


        /// <summary>
        /// 获取指定辅助串口信息
        /// </summary>
        /// <param name="assist"></param>
        /// <returns></returns>
        public static AssistPort GetAssistPortInfo(AssistDevice assist)
        {
            var query = AssistPortItems.Single(x => x.Assist == assist);
            return query;
        }


        #region 删除



        ///// <summary>
        ///// 获取检测模块
        ///// </summary>
        ///// <param name="detectionType">检测类型</param>
        ///// <returns></returns>
        //public static Detection GetDetectionInfo(DetectionType detectionType)
        //{
        //    var query = Detections.SingleOrDefault(x => x.DetectionType == detectionType);
        //    if (query == null)
        //        return new Detection();//throw new Exception();
        //    return query;
        //}

        ///// <summary>
        ///// 获取检测模块的串口信息
        ///// </summary>
        ///// <param name="detectionType"></param>
        ///// <returns></returns>
        //public static PortConfigModel GetDetectionPortInfo(DetectionType detectionType)
        //{
        //    var query = GetDetectionInfo(detectionType);
        //    return query.config;
        //}

        ///// <summary>
        ///// 获取指定辅助串口
        ///// </summary>
        ///// <param name="detectionType"></param>
        ///// <param name="assistType"></param>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //public static PortConfigModel GetAssistPortInfo(DetectionType detectionType, AssistDeviceType assistType, int index)
        //{

        //    var query = Detections.SingleOrDefault(x => x.DType == detectionType);
        //    var query2 = query.AssistList.SingleOrDefault(x => x.AssistDevice.AssistType == assistType && x.Index == index);
        //    return query2.AssistDevice.config;
        //}



        ///// <summary>
        ///// 获取工位信息
        ///// </summary>
        ///// <returns></returns>
        //public StationModel GetStationInfo()
        //{
        //    return null;
        //}

        #endregion

    }

    /// <summary>
    /// 检测项目串口信息
    /// </summary>
    public class DetectionPort
    {
        /// <summary>
        /// 串口实例
        /// </summary>
        public SerialPort Port { get; set; }
        /// <summary>
        /// 配置信息
        /// </summary>
        public Detection Detection { get; set; }

    }
    /// <summary>
    /// 辅助设备串口信息
    /// </summary>
    public class AssistPort
    {
        /// <summary>
        /// 串口信息
        /// </summary>
        public SerialPort Port { get; set; }
        /// <summary>
        /// 关联设备配置
        /// </summary>
        public AssistDevice Assist { get; set; }
    }

    



}
