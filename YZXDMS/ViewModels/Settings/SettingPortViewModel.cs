using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using YZXDMS.Models;
using DevExpress.Mvvm.POCO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using YZXDMS.DataProvider;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingPortViewModel
    {

        public virtual ObservableCollection<PortConfig> Items { get; set; } = new ObservableCollection<PortConfig>();

        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }

        public SettingPortViewModel()
        {
            UpdatePortItems();            
        }

        private void UpdatePortItems()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                var port = db.Ports.ToList();
                Items = new ObservableCollection<PortConfig>(port);
            }
        }

        private void DeletePortItem(PortConfig port)
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                //var item = db.Ports.SingleOrDefault(x => x.Id == port.Id);
                //db.Ports.ToList().Where(x=>x.Id == Guid.Parse("5ae34417-ea9f-4dec-974a-fdc6ce02056e"))
                var item = db.Ports.ToList().SingleOrDefault(x => x.Id == port.Id);
                db.Ports.Remove(item);
                db.SaveChanges();
            }
            UpdatePortItems();
        }

        public void AddItem()
        {
            IDocument doc = documentManagerService.CreateDocument("PortView", null, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "新增串口配置";
            var VM = (PortViewModel)doc.Content;
            doc.Show();

            UpdatePortItems();            
        }

        [Command(CanExecuteMethodName = "CanDeleteItem")]
        public void DeleteItem(PortConfig item)
        {
            if (DevExpress.Xpf.Core.DXMessageBox.Show($"是否要删除{item.Name} {item.PortName}串口?", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning)
                == System.Windows.MessageBoxResult.No)
                return;

            //删除后刷新配置文件并重新加载

            DeletePortItem(item);
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

            DeletePortItem(item);
            //if (VM.IsChange)
            //{
            //    Items.Remove(item);
            //    Items.Add(VM.Item);
            //    Helpers.XmlHelper.serializeToXml(Items, "Port.xml");
            //}
        }

    }

    
}