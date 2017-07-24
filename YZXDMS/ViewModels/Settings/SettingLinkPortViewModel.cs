using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.POCO;
using System.Linq;
using DevExpress.Xpf.Core;
using YZXDMS.DataProvider;

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

            //根据枚举创建检测项目列表
            foreach (var dt in Enum.GetValues(typeof(DetectionType)))
            {
                DetectionItems.Add(new VMDetection((DetectionType)Enum.Parse(typeof(DetectionType), dt.ToString())));
            }
        }

        //public SettingLinkPortViewModel()
        //{
        //    var protConfig = Helpers.DeviceHelper.GetPortConfigItems();
        //    //if (protConfig == null)
        //    //    return;


        //    //根据枚举创建检测项目列表
        //    foreach (var dt in Enum.GetValues(typeof(DetectionType)))
        //    {
        //        //待修改未获取配置并生成相应实例
        //        //也可以在VMDetection中自己获取相关配置
        //        //Helpers.XmlHelper.serializeToXml(DetectionItems, "DetectionXXX.xml");
        //        var detItem = new Detection()
        //        {
        //            Name = dt.ToString(),
        //            DetectionType = (DetectionType)Enum.Parse(typeof(DetectionType), dt.ToString()),
        //            //PortConfig = PortConfig.Create(),
        //        };
        //        //var vmd = new VMDetection(detItem);
        //        var vmd = new VMDetection((DetectionType)Enum.Parse(typeof(DetectionType),dt.ToString()));
        //        DetectionItems.Add(vmd);
        //    }

        //    // Helpers.XmlHelper.serializeToXml(DetectionItems, "DetectionItems.xml");

        //    //var oooii = Helpers.XmlHelper.DeserializerXml<List<VMDetection>>("DetectionItems.xml");

        //    // DetectionItems = oooii;


        //    //DetectionItems.Add(new VMDetection()
        //    //{
        //    //    Detection = new Detection()
        //    //    {
        //    //        Name = "侧滑",
        //    //        DetectionType = DetectionType.侧滑,
        //    //        PortConfig = PortConfig.Create(),
        //    //        AssistList = new List<AssistRoute>()
        //    //    {
        //    //        new AssistRoute()
        //    //        {
        //    //            RouteNumber = 2,
        //    //            Assist = new AssistDevice()
        //    //            {
        //    //                DeviceType = AssistDeviceType.光电设备,
        //    //                PortConfig = new PortConfig()
        //    //            }
        //    //        }
        //    //    }
        //    //    }

        //    //});

        //}

        ///// <summary>
        ///// 添加检测项目
        ///// </summary>
        //public void AddDetection()
        //{

        //}

        ///// <summary>
        ///// 添加关联设备
        ///// </summary>
        //public void AddDeviceItem()
        //{
        //    IDocument doc = documentManagerService.CreateDocument("PortView", null, this);
        //    doc.Id = documentManagerService.Documents.Count();
        //    doc.Title = "新增串口配置";
        //    var VM = (PortViewModel)doc.Content;
        //    doc.Show();

        //    if (VM.IsChange)
        //    {
        //        //if (Items == null)
        //        //    Items = new ObservableCollection<PortConfig>();

        //        //Items.Add(VM.Item);

        //        //Helpers.XmlHelper.serializeToXml(Items, "Port.xml");
        //    }
        //}

        //[Command(CanExecuteMethodName = "CanDeleteDeviceItem")]
        //public void DeleteDeviceItem(PortConfig item)
        //{
        //    if (DevExpress.Xpf.Core.DXMessageBox.Show($"是否要删除{item.Name} {item.PortName}串口?", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning)
        //        == System.Windows.MessageBoxResult.No)
        //        return;

        //    //删除后刷新配置文件并重新加载
        //    //Items.Remove(item);
        //    //Helpers.XmlHelper.serializeToXml(Items, "Port.xml");

        //}

        //public bool CanDeleteDeviceItem(PortConfig item)
        //{
        //    if (item == null)
        //        return false;
        //    return true;
        //}




      
    }

    [POCOViewModel]
    public class VMDetection : ViewModelBase
    {

        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }

        public virtual DetectorModel Detector { get; set; }
        /// <summary>
        /// 显示主设备串口
        /// </summary>
        public virtual PortConfig MainPort { get; set; }

        /// <summary>
        /// 界面显示的辅助列表
        /// </summary>
        public virtual ObservableCollection<AssistDisplayModel> assistList { get; set; } = new ObservableCollection<AssistDisplayModel>();

        /// <summary>
        /// 加载关联信息
        /// </summary>
        /// <param name="dt"></param>
        public VMDetection(DetectionType dt)
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                var query = db.Detectors.SingleOrDefault(x => x.DetectorName == dt.ToString());
                db.Assist.ToList();
                if (query == null)
                {
                    Detector = new DetectorModel() { Id = Guid.NewGuid(), DetectorName = dt.ToString(),DetectorType = dt };
                }
                else
                {
                    Detector = query;
                    UpdateDetector();
                }

            }
        }

        /// <summary>
        /// 更改主检测设备
        /// </summary>
        [Command(true)]
        public void UpdateMainDevice()
        {
            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", true, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "主检测设备";
            var VM = (SelectAssistViewModel)doc.Content;
            VM.IsMain = true;
            doc.Show();

            if (VM.IsChanged)
            {
                using (SQLiteDBContext db = new SQLiteDBContext())
                {
                    MainPort = db.Ports.ToList().Single(x => x.Id == VM.Data.PortId);
                    this.RaisePropertiesChanged();
                    var det = db.Detectors.ToList().SingleOrDefault(x => x.Id == Detector.Id);
                    if (det == null)
                    {
                        Detector.PortId = MainPort.Id;
                        db.Detectors.Add(Detector);
                        db.SaveChanges();
                    }
                    else
                    {
                        det.PortId = MainPort.Id;
                        db.SaveChanges();
                    }
                }
            }

        }


        [Command(true)]
        public void Add()
        {

            IDocument doc = documentManagerService.CreateDocument("SelectAssistView", false, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "追加辅助设备";
            var VM = (SelectAssistViewModel)doc.Content;
            doc.Show();
            if (VM.IsChanged)
            {
                //注意实体与数据库结构并不相同，查询结果IList懒加载的问题
                using (SQLiteDBContext db = new SQLiteDBContext())
                {
                    var query = db.Detectors.ToList().SingleOrDefault(x =>  x.Id == Detector.Id);
                    if (query == null)
                    {
                        db.Detectors.Add(Detector);
                        db.SaveChanges();
                        query = db.Detectors.ToList().SingleOrDefault(x => x.Id == Detector.Id);
                    }
                    VM.Data.Id = Guid.NewGuid();
                    VM.Data.DetectorId = query.Id;
                    db.Assist.Add(VM.Data);
                    db.SaveChanges();
                    
                }
                UpdateDetector();
            }

        }
        /// <summary>
        /// 更新界面数据
        /// </summary>
        void UpdateDetector()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                var query = db.Detectors.ToList().SingleOrDefault(x => x.Id == Detector.Id);

                if (query != null)
                {
                    var main = db.Ports.ToList().SingleOrDefault(x => x.Id == query.PortId);
                    //if (main != null)
                        MainPort = main;
                    var queryAssist = db.Assist.ToList().Where(x => x.DetectorId == query.Id).ToList();
                    assistList.Clear();
                    foreach (var item in queryAssist)
                    {
                        var ass = new AssistDisplayModel() { Assist = item };
                        ass.Port = db.Ports.ToList().Single(x => x.Id == item.PortId);
                        assistList.Add(ass);
                    }
                }
                
            }
        }


        [Command(CanExecuteMethodName = "CanDeleteItem")]
        public void DeleteItem(AssistDisplayModel item)
        {
            if (DevExpress.Xpf.Core.DXMessageBox.Show($"是否删除{item.Port.Name} {item.Port.DeviceType}?", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning, System.Windows.MessageBoxResult.No)
                 == System.Windows.MessageBoxResult.Yes)
            {
                using (SQLiteDBContext db = new SQLiteDBContext())
                {
                    var query = db.Assist.ToList().Single(x => x.Id == item.Assist.Id);
                    db.Assist.Remove(query);
                    db.SaveChanges();
                    UpdateDetector();
                }
            }
        }

        public bool CanDeleteItem(AssistDisplayModel item)
        {
            return item == null ? false : true;
        }




        //public Detection Detection { get; set; }      


        //public VMDetection(Detection detection)
        //{
        //    //this.Detection = detection;

        //    //var config = Helpers.XmlHelper.DeserializerXml<Detection>($"Detection{Detection.DetectionType}.xml");
        //    //this.Detection = config;


        //    //if (this.Detection != null)
        //    //{
        //    //    if (this.Detection.AssistList != null)
        //    //    {
        //    //        this.AssistList = new ObservableCollection<AssistRoute>(this.Detection.AssistList);
        //    //    }
        //    //}


        //    //this.Detection = new Detection();

        //}

        //public VMDetection(DetectionType dt)
        //{
        //    var config = Helpers.XmlHelper.DeserializerXml<Detection>($"Detection{dt}.xml");
        //    if (config == null)
        //    {
        //        this.Detection = new Detection();
        //        this.Detection.Name = dt.ToString();
        //        this.Detection.DetectionType = dt;
        //    }
        //    else
        //        this.Detection = config;

        //    if (this.Detection.PortConfig == null)
        //        this.Detection.PortConfig = new PortConfig();

        //}

        ///// <summary>
        ///// 更新配置文件
        ///// </summary>
        //[Command(true)]
        //public void UpdateXmlConfig()
        //{
        //    //var detType = this.Detection.DetectionType;
        //    //Helpers.XmlHelper.serializeToXml(Detection, $"Detection{detType}.xml");
        //}

        ///// <summary>
        ///// 更改主检测设备
        ///// </summary>
        //[Command(true)]
        //public void UpdateMainDevice()
        //{
        //    //SelectAssistParam sap = new SelectAssistParam();
        //    //sap.IsMain = true;
        //    //IDocument doc = documentManagerService.CreateDocument("SelectAssistView", sap, this);
        //    //doc.Id = documentManagerService.Documents.Count();
        //    //doc.Title = "主检测设备";
        //    //var VM = (SelectAssistViewModel)doc.Content;
        //    //VM.IsMain = true;
        //    //doc.Show();

        //    //if (VM.IsChanged)
        //    //{
        //    //    this.Detection.PortConfig = VM.PConfig;
        //    //}

        //}



        //[DevExpress.Mvvm.DataAnnotations.Command(true)]
        //public void Add()
        //{
        //    //SelectAssistParam sap = new SelectAssistParam();
        //    //sap.IsMain = false;
        //    //IDocument doc = documentManagerService.CreateDocument("SelectAssistView", sap, this);
        //    //doc.Id = documentManagerService.Documents.Count();
        //    //doc.Title = "追加辅助设备";
        //    //var VM = (SelectAssistViewModel)doc.Content;
        //    //doc.Show();

        //    //if (VM.IsChanged)
        //    //{
        //    //    Detection.AssistList.Add(new AssistRoute()
        //    //    {
        //    //        RouteNumber = VM.Route,
        //    //        PortConfig = VM.PConfig
        //    //    });
        //    //}

        //}
        //[Command(CanExecuteMethodName = "CanShowItem")]
        //public void ShowItem(AssistRoute item)
        //{
        //    //SelectAssistParam sap = new SelectAssistParam();
        //    //sap.IsMain = false;
        //    //sap.AR = (AssistRoute)item.Clone();
        //    //IDocument doc = documentManagerService.CreateDocument("SelectAssistView", sap, this);
        //    //doc.Id = documentManagerService.Documents.Count();
        //    //doc.Title = "辅助设备";
        //    //var VM = (SelectAssistViewModel)doc.Content;
        //    //doc.Show();

        //    //if (VM.IsChanged)
        //    //{
        //    //    Detection.AssistList.Remove(item);
        //    //    Detection.AssistList.Add(new AssistRoute()
        //    //    {
        //    //        PortConfig = VM.PConfig,
        //    //        RouteNumber = VM.Route
        //    //    });
        //    //}
        //}

        //public bool CanShowItem(AssistRoute item)
        //{
        //    if (item == null)
        //        return false;
        //    return true;
        //}

        //[Command(CanExecuteMethodName = "CanDeleteItem")]
        //public void DeleteItem(AssistRoute item)
        //{
        //    DevExpress.Xpf.Core.DXMessageBox.Show($"是否删除{item.PortConfig.Name}?");
        //}

        //public bool CanDeleteItem(AssistRoute item)
        //{
        //    return item == null ? false : true;
        //}

    }


}