using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.Collections.Generic;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class TestyViewModel
    {
        public List<Detection> DetectionItems { get; set; } = new List<Detection>();


        public TestyViewModel()
        {
            DetectionItems.Add(new Detection()
            {
                Name = "侧滑",
                DetectionType = DetectionType.侧滑,
                PortConfig = PortConfig.Create(),
                AssistList = new List<AssistRoute>()
                {
                    new AssistRoute()
                    {
                        RouteNumber = 1,
                        Assist = new AssistDevice()
                        {
                            DeviceType = AssistDeviceType.光电设备,
                            PortConfig = new PortConfig()
                        }
                    }
                }
            });
        }

    }
}