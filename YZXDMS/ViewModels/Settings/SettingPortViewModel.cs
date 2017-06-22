using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using YZXDMS.Models;
using DevExpress.Mvvm.POCO;
using System.Linq;
using System.Collections.ObjectModel;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingPortViewModel
    {

        public ObservableCollection<PortConfig> Items { get; set; } = new ObservableCollection<PortConfig>();

        protected IDocumentManagerService documentManagerService { get { return this.GetService<IDocumentManagerService>(); } }

        public SettingPortViewModel()
        {
            //从本地配置文件中获取到并实例化
            //Items.Add(new PortConfig()
            //{
            //    Name = "速度光电",
            //    DeviceProperty = DeviceProperty.辅助设备,
            //    StartMode = StartMode.保持开启,
            //    Protocol = "XX协议",
            //    Port = new System.IO.Ports.SerialPort("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
            //});

            //Items.Add(new PortConfig()
            //{
            //    Name = "速度",
            //    DeviceProperty = DeviceProperty.检测设备,
            //    StartMode = StartMode.保持开启,
            //    Protocol = "XX协议",
            //    Port = new System.IO.Ports.SerialPort("COM2", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
            //});
        }



        public void AddItem()
        {
            IDocument doc = documentManagerService.CreateDocument("PortView", null, this);
            doc.Id = documentManagerService.Documents.Count();
            doc.Title = "新增串口配置";
            var VM = (PortViewModel)doc.Content;
            doc.Show();

            if (VM.IsChange)
                Items.Add(VM.Item);


        }


        public void DeleteItem()
        {

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
            }
        }

    }
}