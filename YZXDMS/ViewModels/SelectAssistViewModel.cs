using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using YZXDMS.Models;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SelectAssistViewModel:ViewModelBase
    {

        public List<PortConfig> PortConfigItems { get; set; }


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
        public bool IsMain { get; internal set; }

        public SelectAssistViewModel()
        {
            PortConfigItems = Helpers.DeviceHelper.GetPortConfigItems(); 
        }

        protected override void OnParameterChanged(object parameter)
        {
            base.OnParameterChanged(parameter);
            if (parameter != null)
            {
                var param = (parameter as AssistRoute);
                //this.ADT = param.Assist.DeviceType;
                this.Route = param.RouteNumber;
                //this.PConfig = param.Assist.PortConfig;

                this.PConfig = param.PortConfig;
            }
        }



        public void Cancel()
        {

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
        }



        //public void Selected()

    }
}