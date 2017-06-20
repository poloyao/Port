using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.Collections.Generic;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingLinkPortViewModel
    {

        public List<DetectionModel> DetectionItems { get; set; } = new List<DetectionModel>();


        public SettingLinkPortViewModel()
        {
            InitPort();
        }

        void InitPort()
        {

            var port = new System.IO.Ports.SerialPort("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


            DetectionItems.Add(new DetectionModel()
            {
                Name = "速度",
                DType = DetectionType.Speed,
                config = new PortConfigModel()
                {
                    DeviceProperty = DeviceProperty.检测设备,
                    StartMode = StartMode.即用即关,
                    Protocol = "XX协议",//new ProtocolModel() { Name = "XX协议" },
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
                                Protocol = "XX协议",//new ProtocolModel() {Name = "xx光电协议" },
                                Port = port//new System.IO.Ports.SerialPort("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
                            }
                        }
                    }
                }
            });
        }
    }
}