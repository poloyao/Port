using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using YZXDMS.Models;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SelectAssistViewModel:ViewModelBase, IDocumentContent
    {

        public virtual List<PortConfig> PortConfigItems { get; set; }


        public virtual PortConfig PConfig { get; set; }

        public virtual int Route { get; set; } = 1;


        public virtual AssistDeviceType ADT { get; set; }


        /// <summary>
        /// 是否有变化保存
        /// </summary>
        public bool IsChanged { get; set; }


        public AssistRoute Data { get; set; }
        /// <summary>
        /// 是否是主检测设备
        /// </summary>
        public  bool IsMain { get;  set; }

        /// <summary>
        /// 是否显示设备类型，通道等，与IsMain关联
        /// </summary>
        public virtual bool IsVisibility { get; set; }

        public SelectAssistViewModel()
        {
           // PortConfigItems = Helpers.DeviceHelper.GetPortConfigItems(); 
        }

        protected override void OnParameterChanged(object parameter)
        {
            base.OnParameterChanged(parameter);
            if (parameter != null)
            {
                var param = (parameter as SelectAssistParam);
                this.IsMain = param.IsMain;
                if (param.AR != null)
                {
                    this.Route = param.AR.RouteNumber;
                    this.PConfig = param.AR.PortConfig;
                }

                var pci = Helpers.DeviceHelper.GetPortConfigItems();

                if (this.IsMain)
                {
                    PortConfigItems = pci.Where(x => (int)x.DeviceType < 100).ToList();
                }
                else
                {
                    PortConfigItems = pci.Where(x => (int)x.DeviceType > 100).ToList();
                }

                this.IsVisibility = !this.IsMain;

            }
        }



        public void Cancel()
        {
            this.IsChanged = false;
            DocumentOwner.Close(this, true);
        }

        public void Save()
        {
            Data = new AssistRoute()
            {
                RouteNumber = Route,
                //Assist = new AssistDevice()
                //{
                //    PortConfig = PConfig,
                //    DeviceType = ADT
                //},
                PortConfig = PConfig
            };
            this.IsChanged = true;
            DevExpress.Xpf.Core.DXMessageBox.Show("保存成功!");
            this.DocumentOwner.Close(this);
        }



        //public void Selected()


        #region IDocumentContent

        public IDocumentOwner DocumentOwner { get; set; }

        public object Title { get; }

        public void OnClose(CancelEventArgs e)
        {
            e.Cancel = false;
        }

        public void OnDestroy() { }

        #endregion

    }


    public class SelectAssistParam
    {
        public AssistRoute AR { get; set; }

        public bool IsMain { get; set; }
    }

}