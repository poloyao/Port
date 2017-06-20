using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;

namespace YZXDMS.Helper
{
    /// <summary>
    /// 设备帮助类
    /// </summary>
    public class DeviceHelper
    {
        private static List<DetectionModel> Detections { get; set; } = new List<DetectionModel>();

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

            var port = new System.IO.Ports.SerialPort("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


            Detections.Add(new DetectionModel()
            {
                Name = "速度",
                DType = DetectionType.Speed,
                config = new PortConfigModel()
                {
                    DeviceProperty = DeviceProperty.检测设备,
                    StartMode = StartMode.即用即关,
                    Protocol = "XX协议",
                    Port = new System.IO.Ports.SerialPort("COM3", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
                },
                AssistList = new List<AssistDeviceOrder>()
                {
                    new AssistDeviceOrder()
                    {
                        AssistDevice = new AssistDeviceModel()
                        {
                            AssistType = AssistDeviceType.Photoelectric,
                            Name = "速度光电",
                            config = new PortConfigModel()
                            {
                                DeviceProperty = DeviceProperty.辅助设备,
                                StartMode = StartMode.保持开启,
                                Protocol = "XX协议",
                                Port = port
                            }
                        }
                    }
                }
            });
        }



        /// <summary>
        /// 获取检测模块
        /// </summary>
        /// <param name="detectionType">检测类型</param>
        /// <returns></returns>
        public static DetectionModel GetDetectionInfo(DetectionType detectionType)
        {
            var query = Detections.SingleOrDefault(x => x.DType == detectionType);
            if (query == null)
                return new DetectionModel();//throw new Exception();
            return query;
        }

        /// <summary>
        /// 获取检测模块的串口信息
        /// </summary>
        /// <param name="detectionType"></param>
        /// <returns></returns>
        public static PortConfigModel GetDetectionPortInfo(DetectionType detectionType)
        {
            var query = GetDetectionInfo(detectionType);
            return query.config;
        }

        /// <summary>
        /// 获取指定辅助串口
        /// </summary>
        /// <param name="detectionType"></param>
        /// <param name="assistType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static PortConfigModel GetAssistPortInfo(DetectionType detectionType, AssistDeviceType assistType, int index)
        {

            var query = Detections.SingleOrDefault(x => x.DType == detectionType);
            var query2 = query.AssistList.SingleOrDefault(x => x.AssistDevice.AssistType == assistType && x.Index == index);
            return query2.AssistDevice.config;
        }
                


        /// <summary>
        /// 获取工位信息
        /// </summary>
        /// <returns></returns>
        public StationModel GetStationInfo()
        {
            return null;
        }



    }
}
