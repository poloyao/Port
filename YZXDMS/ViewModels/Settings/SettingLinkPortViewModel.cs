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
    /// <summary>
    /// 设置检测项目关联设备
    /// </summary>
    [POCOViewModel]
    public class SettingLinkPortViewModel
    {
        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }
        public List<VMDetection> DetectionItems { get; set; } = new List<VMDetection>();


        public SettingLinkPortViewModel()
        {
            var protConfig = Helpers.DeviceHelper.GetPortConfigItems();

            //根据枚举创建检测项目列表
            foreach (var dt in Enum.GetValues(typeof(DetectionType)))
            {
                var vmd = new VMDetection()
                {
                    Detection = new Detection()
                    {
                        Name = dt.ToString(),
                        DetectionType = (DetectionType)Enum.Parse(typeof(DetectionType), dt.ToString()),
                        PortConfig = PortConfig.Create(),
                        AssistList = new List<AssistRoute>()
                        {
                            new AssistRoute()
                            {
                                RouteNumber = 2,
                                Assist = new AssistDevice()
                                {
                                    DeviceType = AssistDeviceType.光电设备,
                                    PortConfig = protConfig.Last()
                                }
                            }
                        }
                    }
                };
                DetectionItems.Add(vmd);
            }

            // Helpers.XmlHelper.serializeToXml(DetectionItems, "DetectionItems.xml");

            //var oooii = Helpers.XmlHelper.DeserializerXml<List<VMDetection>>("DetectionItems.xml");

            // DetectionItems = oooii;


            //DetectionItems.Add(new VMDetection()
            //{
            //    Detection = new Detection()
            //    {
            //        Name = "侧滑",
            //        DetectionType = DetectionType.侧滑,
            //        PortConfig = PortConfig.Create(),
            //        AssistList = new List<AssistRoute>()
            //    {
            //        new AssistRoute()
            //        {
            //            RouteNumber = 2,
            //            Assist = new AssistDevice()
            //            {
            //                DeviceType = AssistDeviceType.光电设备,
            //                PortConfig = new PortConfig()
            //            }
            //        }
            //    }
            //    }

            //});

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




      
    }

    
    public class VMDetection : ViewModelBase
    {

        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }
        public Detection Detection { get; set; }
        [DevExpress.Mvvm.DataAnnotations.Command(true)]
        public void Add()
        {
            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", null, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "追加辅助设备";
            var VM = (SelectAssistViewModel)doc.Content;
            doc.Show();

            if (VM.IsChanged)
            {
                Detection.AssistList.Add(new AssistRoute()
                {
                    RouteNumber = VM.Route,
                    Assist = new AssistDevice()
                    {
                        DeviceType = VM.ADT,
                        PortConfig = VM.PConfig
                    }
                });
            }

        }
        [Command(CanExecuteMethodName = "CanShowItem")]
        public void ShowItem(AssistRoute item)
        {
            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", item.Clone(), this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "辅助设备";
            var VM = (SelectAssistViewModel)doc.Content;
            doc.Show();

            if (VM.IsChanged)
            {
                item.RouteNumber = VM.Route;
                item.Assist.DeviceType = VM.ADT;
                item.Assist.PortConfig = VM.PConfig;
            }
        }

        public bool CanShowItem(AssistRoute item)
        {
            if (item == null)
                return false;
            return true;
        }

    }


}