using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.POCO;
using System.Linq;
using DevExpress.Xpf.Core;

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
            //if (protConfig == null)
            //    return;


            //根据枚举创建检测项目列表
            foreach (var dt in Enum.GetValues(typeof(DetectionType)))
            {
                //待修改未获取配置并生成相应实例
                //也可以在VMDetection中自己获取相关配置
                //Helpers.XmlHelper.serializeToXml(DetectionItems, "DetectionXXX.xml");
                var detItem = new Detection()
                {
                    Name = dt.ToString(),
                    DetectionType = (DetectionType)Enum.Parse(typeof(DetectionType), dt.ToString()),
                    //PortConfig = PortConfig.Create(),
                };
                //var vmd = new VMDetection(detItem);
                var vmd = new VMDetection((DetectionType)Enum.Parse(typeof(DetectionType),dt.ToString()));
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




        public VMDetection(Detection detection)
        {
            this.Detection = detection;

            var config = Helpers.XmlHelper.DeserializerXml<Detection>($"Detection{Detection.DetectionType}.xml");
            this.Detection = config;


            //if (this.Detection != null)
            //{
            //    if (this.Detection.AssistList != null)
            //    {
            //        this.AssistList = new ObservableCollection<AssistRoute>(this.Detection.AssistList);
            //    }
            //}


            //this.Detection = new Detection();

        }

        public VMDetection(DetectionType dt)
        {
            var config = Helpers.XmlHelper.DeserializerXml<Detection>($"Detection{dt}.xml");
            if (config == null)
            {
                this.Detection = new Detection();
                this.Detection.Name = dt.ToString();
                this.Detection.DetectionType = dt;
            }
            else
                this.Detection = config;

            if (this.Detection.PortConfig == null)
                this.Detection.PortConfig = new PortConfig();

        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        [Command(true)]
        public void UpdateXmlConfig()
        {
            var detType = this.Detection.DetectionType;
            Helpers.XmlHelper.serializeToXml(Detection, $"Detection{detType}.xml");
        }

        /// <summary>
        /// 更改主检测设备
        /// </summary>
        [Command(true)]
        public void UpdateMainDevice()
        {
            SelectAssistParam sap = new SelectAssistParam();
            sap.IsMain = true;
            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", sap, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "主检测设备";
            var VM = (SelectAssistViewModel)doc.Content;
            VM.IsMain = true;
            doc.Show();

            if (VM.IsChanged)
            {
                this.Detection.PortConfig = VM.PConfig;
            }

        }



        [DevExpress.Mvvm.DataAnnotations.Command(true)]
        public void Add()
        {
            SelectAssistParam sap = new SelectAssistParam();
            sap.IsMain = false;
            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", sap, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "追加辅助设备";
            var VM = (SelectAssistViewModel)doc.Content;
            doc.Show();

            if (VM.IsChanged)
            {
                Detection.AssistList.Add(new AssistRoute()
                {
                    RouteNumber = VM.Route,
                    PortConfig = VM.PConfig
                });
            }

        }
        [Command(CanExecuteMethodName = "CanShowItem")]
        public void ShowItem(AssistRoute item)
        {
            SelectAssistParam sap = new SelectAssistParam();
            sap.IsMain = false;
            sap.AR = (AssistRoute)item.Clone();
            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", sap, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "辅助设备";
            var VM = (SelectAssistViewModel)doc.Content;
            doc.Show();

            if (VM.IsChanged)
            {
                Detection.AssistList.Remove(item);
                Detection.AssistList.Add(new AssistRoute()
                {
                    PortConfig = VM.PConfig,
                    RouteNumber = VM.Route
                });
            }
        }

        public bool CanShowItem(AssistRoute item)
        {
            if (item == null)
                return false;
            return true;
        }

        [Command(CanExecuteMethodName = "CanDeleteItem")]
        public void DeleteItem(AssistRoute item)
        {
            DevExpress.Xpf.Core.DXMessageBox.Show($"是否删除{item.PortConfig.Name}?");
        }

        public bool CanDeleteItem(AssistRoute item)
        {
            return item == null ? false : true;
        }

    }


}