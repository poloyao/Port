using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using YZXDMS.Models;
using DevExpress.Mvvm.POCO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingPortViewModel
    {

        public ObservableCollection<PortConfig> Items { get; set; } = new ObservableCollection<PortConfig>();

        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }

        public SettingPortViewModel()
        {

            var configs = Helpers.DeviceHelper.GetPortConfigItems();
            //如果回传的值未null 则从新创建xml文件
            if (configs == null)
                return;
            Items = new ObservableCollection<PortConfig>(configs);

            //如果没有Port.xml文件，则搜索Detection.xml文件，根据此文件结构创建port.xml文件
            //Items = Helpers.XmlHelper.DeserializerXml<ObservableCollection<PortConfig>>("Port.xml");
            //if (Items == null)
            //{
            //    List<Detection> det = Helpers.XmlHelper.DeserializerXml<List<Detection>>("Detection.xml");
            //    if (det != null)
            //    {
            //        Items = new ObservableCollection<PortConfig>();

            //        foreach (var detItem in det)
            //        {
            //            if (detItem.PortConfig == null)
            //                continue;
            //            Items.Add(detItem.PortConfig);

            //            if (detItem.AssistList == null)
            //                continue;
            //            foreach (var assistItem in detItem.AssistList)
            //            {
            //                if (assistItem.Assist.PortConfig == null)
            //                    continue;

            //                //此处可以忽略，在循环完毕后，删除同类项。但需要创建比较器
            //                List<PortConfig> tempds = new List<PortConfig>();
            //                foreach (var ds in Items)
            //                {
            //                    var comp = Helpers.DataHelper.EntityComparison(ds, assistItem.Assist.PortConfig);
            //                    if(!comp)
            //                        tempds.Add(assistItem.Assist.PortConfig);
            //                }
            //                tempds.ForEach(x => { Items.Insert(Items.Count(), x); });
            //            }
            //        }

            //        //var kkkk = Items.Distinct(System.Collections.Generic.Comparer.Default);
            //    }                
            //}
            


        }



        public void AddItem()
        {
            IDocument doc = documentManagerService.CreateDocument("PortView", null, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "新增串口配置";
            var VM = (PortViewModel)doc.Content;
            doc.Show();

            if (VM.IsChange)
            {
                if (Items == null)
                    Items = new ObservableCollection<PortConfig>();

                Items.Add(VM.Item);

                Helpers.XmlHelper.serializeToXml(Items, "Port.xml");
            }
        }

        [Command(CanExecuteMethodName = "CanDeleteItem")]
        public void DeleteItem(PortConfig item)
        {
            if (DevExpress.Xpf.Core.DXMessageBox.Show($"是否要删除{item.Name} {item.PortName}串口?", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning)
                == System.Windows.MessageBoxResult.No)
                return;

            //删除后刷新配置文件并重新加载
            Items.Remove(item);
            Helpers.XmlHelper.serializeToXml(Items, "Port.xml");

        }

        public bool CanDeleteItem(PortConfig item)
        {
            if (item == null)
                return false;
            return true;
        }


        public void Selected(PortConfig item)
        {
            if (item == null)
                return;

            PortConfig itemClone = (PortConfig)item.Clone();
            IDocument doc = documentManagerService.CreateDocument("PortView", itemClone, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = $"{item.Name }串口配置";
            var VM = (PortViewModel)doc.Content;
            doc.Show();
            if (VM.IsChange)
            {
                Items.Remove(item);
                Items.Add(VM.Item);
                Helpers.XmlHelper.serializeToXml(Items, "Port.xml");
            }
        }

    }

    
}