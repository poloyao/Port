using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using YZXDMS.Models;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingStationViewModel
    {
        public ObservableCollection<StationModel> StationItems { get; set; }

        public SettingStationViewModel()
        {
            StationItems = new ObservableCollection<StationModel>();
            StationItems.Add(new StationModel()
            {
                Name = "1 工位",
                Description = "描述信息",
                Index = 0,
                DetectionItems = new ObservableCollection<DetectionOrder>()
                { new DetectionOrder()
                { Index = 0,
                Detection = new DetectionModel()
                {
                    Name = "项目",
                    AssistList = new System.Collections.Generic.List<AssistDeviceOrder>()
                    {
                        new AssistDeviceOrder()
                        {
                            AssistDevice = new AssistDeviceModel()
                            {
                                Name = "光电1",
                                AssistType = AssistDeviceType.Photoelectric
                            },
                        },
                        new AssistDeviceOrder()
                        {
                            AssistDevice = new AssistDeviceModel()
                            {
                                Name = "光电1",
                                AssistType = AssistDeviceType.Photoelectric
                            },
                        }
                    }
                }
                } }
            });

            StationItems.Add(new StationModel()
            {
                Name = "2 工位",
                Description = "描述信息",
                Index = 0,
                DetectionItems = new ObservableCollection<DetectionOrder>()
                { new DetectionOrder()
                { Index = 0,
                Detection = new DetectionModel()
                {
                    Name = "项目"
                }
                } }
            });
        }


    }
}