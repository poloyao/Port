using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.POCO;
using System.Linq;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingLinkPortViewModel
    {
        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }
        public List<Detection> DetectionItems { get; set; } = new List<Detection>();


        public SettingLinkPortViewModel()
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
                            DeviceType = AssistDeviceType.Photoelectric,
                            PortConfig = new PortConfig()
                        }
                    }
                }
            });
            DetectionItems.Add(new Detection()
            {
                Name = "制动",
                DetectionType = DetectionType.制动
            });
            DetectionItems.Add(new Detection()
            {
                Name = "功率",
                DetectionType = DetectionType.功率
            });
            DetectionItems.Add(new Detection()
            {
                Name = "声级计",
                DetectionType = DetectionType.声级计
            });
            DetectionItems.Add(new Detection()
            {
                Name = "外检",
                DetectionType = DetectionType.外检
            });
            DetectionItems.Add(new Detection()
            {
                Name = "尾气",
                DetectionType = DetectionType.尾气
            });
            DetectionItems.Add(new Detection()
            {
                Name = "底盘",
                DetectionType = DetectionType.底盘
            });
            DetectionItems.Add(new Detection()
            {
                Name = "底盘间隙",
                DetectionType = DetectionType.底盘间隙
            });
            DetectionItems.Add(new Detection()
            {
                Name = "探平衡仪",
                DetectionType = DetectionType.探平衡仪
            });
            DetectionItems.Add(new Detection()
            {
                Name = "油耗",
                DetectionType = DetectionType.油耗
            });
            DetectionItems.Add(new Detection()
            {
                Name = "灯光",
                DetectionType = DetectionType.灯光
            });
            DetectionItems.Add(new Detection()
            {
                Name = "称重",
                DetectionType = DetectionType.称重
            });
            DetectionItems.Add(new Detection()
            {
                Name = "速度",
                DetectionType = DetectionType.速度
            });

        }

        /// <summary>
        /// 添加检测项目
        /// </summary>
        public void AddDetection()
        {

        }

        /// <summary>
        /// 添加关联设备
        /// </summary>
        public void AddDeviceItem()
        {
            IDocument doc = documentManagerService.CreateDocument("PortView", null, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "新增串口配置";
            var VM = (PortViewModel)doc.Content;
            doc.Show();

            if (VM.IsChange)
            {
                //if (Items == null)
                //    Items = new ObservableCollection<PortConfig>();

                //Items.Add(VM.Item);

                //Helpers.XmlHelper.serializeToXml(Items, "Port.xml");
            }
        }

        [Command(CanExecuteMethodName = "CanDeleteDeviceItem")]
        public void DeleteDeviceItem(PortConfig item)
        {
            if (DevExpress.Xpf.Core.DXMessageBox.Show($"是否要删除{item.Name} {item.PortName}串口?", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning)
                == System.Windows.MessageBoxResult.No)
                return;

            //删除后刷新配置文件并重新加载
            //Items.Remove(item);
            //Helpers.XmlHelper.serializeToXml(Items, "Port.xml");

        }

        public bool CanDeleteDeviceItem(PortConfig item)
        {
            if (item == null)
                return false;
            return true;
        }




        // public ObservableCollection<AssistDeviceOrder> AssistDeviceOrderItems { get; set; } = new ObservableCollection<AssistDeviceOrder>();

        //public SettingLinkPortViewModel()
        //{
        //    InitPort();
        //    AssistDeviceOrderItems = new ObservableCollection<AssistDeviceOrder>()
        //    {
        //        new AssistDeviceOrder()
        //        {
        //            Index = 0,
        //            AssistDevice = new AssistDeviceModel()
        //            {
        //                Name = "光电1",
        //                AssistType = AssistDeviceType.Photoelectric,
        //            }
        //        }
        //    };
        //}

        //void InitPort()
        //{

        //    var port = new System.IO.Ports.SerialPort("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


        //    DetectionItems.Add(new DetectionModel()
        //    {
        //        Name = "速度",
        //        DType = DetectionType.Speed,
        //        config = new PortConfigModel()
        //        {
        //            DeviceProperty = DeviceProperty.检测设备,
        //            StartMode = StartMode.即用即关,
        //            Protocol = "XX协议",//new ProtocolModel() { Name = "XX协议" },
        //            Port = new System.IO.Ports.SerialPort("COM3", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
        //        },
        //        AssistList = new List<AssistDeviceOrder>()
        //        {
        //            new AssistDeviceOrder()
        //            {
        //                AssistDevice = new AssistDeviceModel()
        //                {
        //                    AssistType = AssistDeviceType.Photoelectric,
        //                    Name = "速度光电",
        //                    config = new PortConfigModel()
        //                    {
        //                        DeviceProperty = DeviceProperty.辅助设备,
        //                        StartMode = StartMode.保持开启,
        //                        Protocol = "XX协议",//new ProtocolModel() {Name = "xx光电协议" },
        //                        Port = port//new System.IO.Ports.SerialPort("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
        //                    }
        //                }
        //            }
        //        }
        //    });


        //    Items.Add(new linkModel() { ID = 1, Name = "1" });
        //    Items.Add(new linkModel() { ID = 2, Name = "2",ParentId = 1 });
        //    Items.Add(new linkModel() { ID = 3, Name = "3", ParentId = 1 });
        //    Items.Add(new linkModel() { ID = 4, Name = "4", ParentId = 5 });
        //    Items.Add(new linkModel() { ID = 5, Name = "5" });
        //    Items.Add(new linkModel() { ID = 6, Name = "6" });

        //}
    }


    public class linkModel
    {
        public int ID { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }
    }
}