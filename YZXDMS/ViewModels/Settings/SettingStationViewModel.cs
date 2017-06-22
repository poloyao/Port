using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using YZXDMS.Models;
using System.Collections.Generic;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingStationViewModel
    {
        public List<StationModel> StationItems { get; set; }

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