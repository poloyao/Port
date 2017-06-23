using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using YZXDMS.Models;
using System.Collections.Generic;
using System.IO.Ports;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingStationViewModel
    {
        public List<StationModel> StationItems { get; set; } = new List<StationModel>();


        public SettingStationViewModel()
        {
            //StationItems = new List<StationModel>();
            //StationItems.Add(new StationModel()
            //{
            //    Name = "1 工位",
            //    Description = "描述信息",
            //    Index = 1,
            //    DetectionItems = new List<DetectionOrder>()
            //    {
            //        new DetectionOrder()
            //        {
            //            Index = 1,
            //            Detection = new Detection()
            //            {
            //               Name = "速度",
            //                DetectionType = DetectionType.速度,
            //                PortConfig = new PortConfig()
            //                {
            //                    Name = "速度",
            //                    Protocol = "速度协议",
            //                    PortName = PortIndex.COM10,
            //                    BaudRate = 9600,
            //                    Parity = Parity.None,
            //                    DataBits = 8,
            //                    StopBits = StopBits.One,
            //                    DeviceType = DeviceType.速度,

            //                },
            //                AssistList = new List<AssistRoute>()
            //                {
            //                    new AssistRoute()
            //                    {
            //                        RouteNumber = 1,
            //                        Assist = new AssistDevice()
            //                        {
            //                            DeviceType = AssistDeviceType.Photoelectric,
            //                            PortConfig = new PortConfig()
            //                            {
            //                                    Name = "光电",
            //                                Protocol = "光电协议",
            //                                PortName = PortIndex.COM1,
            //                                BaudRate  = 9600,
            //                                Parity = Parity.None,
            //                                DataBits = 8,
            //                                StopBits = StopBits.One,
            //                                DeviceType = DeviceType.光电设备,
            //                            },
            //                            RouteTotal = 8
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //});

            //StationItems.Add(new StationModel()
            //{
            //    Name = "2 工位",
            //    Description = "描述信息",
            //    Index = 0,
            //    DetectionItems = new List<DetectionOrder>()
            //     { new DetectionOrder()
            //     { Index = 0,
            //     Detection = new DetectionModel()
            //     {
            //         Name = "项目",
            //         config = new PortConfigModel()
            //         {
            //             Name = "项目1",
            //             StartMode = StartMode.即用即关,
            //             Port = new System.IO.Ports.SerialPort()
            //         }
            //     }
            //     } }
            //});


            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(StationItems));
            //Console.WriteLine()
            //Helpers.XmlHelper.serializeToXml(StationItems, "Station.xml");
            
            StationItems = Helpers.XmlHelper.DeserializerXml<List<StationModel>>("Station.xml");

        }







        // public SettingStationViewModel()
        // {
        //     StationItems = new List<StationModel>();
        //     StationItems.Add(new StationModel()
        //     {
        //         Name = "1 工位",
        //         Description = "描述信息",
        //         Index = 0,
        //         DetectionItems = new List<DetectionOrder>()
        //         { new DetectionOrder()
        //         { Index = 0,
        //         Detection = new DetectionModel()
        //         {
        //             Name = "项目",
        //             AssistList = new List<AssistDeviceOrder>()
        //             {
        //                 new AssistDeviceOrder()
        //                 {
        //                     AssistDevice = new AssistDeviceModel()
        //                     {
        //                         Name = "光电1",
        //                         AssistType = AssistDeviceType.Photoelectric
        //                     },
        //                 },
        //                 new AssistDeviceOrder()
        //                 {
        //                     AssistDevice = new AssistDeviceModel()
        //                     {
        //                         Name = "光电1",
        //                         AssistType = AssistDeviceType.Photoelectric
        //                     },
        //                 }
        //             }
        //         }
        //         } }
        //     });

        //     StationItems.Add(new StationModel()
        //     {
        //         Name = "2 工位",
        //         Description = "描述信息",
        //         Index = 0,
        //         DetectionItems = new List<DetectionOrder>()
        //         { new DetectionOrder()
        //         { Index = 0,
        //         Detection = new DetectionModel()
        //         {
        //             Name = "项目",
        //             config = new PortConfigModel()
        //             {
        //                 Name = "项目1",
        //                 StartMode = StartMode.即用即关,
        //                 Port = new System.IO.Ports.SerialPort()
        //             }
        //         }
        //         } }
        //     });


        //     Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(StationItems));


        //}


    }
}