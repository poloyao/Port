using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using YZXDMS.Models;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using YZXDMS.DataProvider;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SelectAssistViewModel : ViewModelBase, IDocumentContent
    {
        public virtual List<PortConfig> PortConfigItems { get; set; }

        public virtual PortConfig PConfig { get; set; }

        public virtual int Route { get; set; } = 1;

        public virtual AssistDeviceType ADT { get; set; }

        /// <summary>
        /// 回调的数据
        /// </summary>
        public AssistModel Data { get; set; }
        /// <summary>
        /// 是否有变化保存
        /// </summary>
        public bool IsChanged { get; set; }
        /// <summary>
        /// 是否是主检测设备
        /// </summary>
        public bool IsMain { get; set; }


        DetectorModel _detector;


        public SelectAssistViewModel()
        {
           
        }

        protected override void OnParameterChanged(object parameter)
        {
            base.OnParameterChanged(parameter);
            if (parameter != null)
            {
                this.IsMain = (bool)parameter;

                using (SQLiteDBContext db = new SQLiteDBContext())
                {
                     if (this.IsMain)
                    {
                        PortConfigItems = db.Ports.Where(x => (int)x.DeviceType < 100).ToList();
                    }
                    else
                    {
                        PortConfigItems = db.Ports.Where(x => (int)x.DeviceType > 100).ToList();
                    }
                }

            }
        }

        public void Cancel()
        {
            DocumentOwner.Close(this, true);
        }

        public void Save()
        {
            Data = new AssistModel()
            {
                //port = PConfig,
                Route = Route,
                PortId = PConfig.Id
            };
            IsChanged = true;
            DevExpress.Xpf.Core.DXMessageBox.Show("保存成功!");
            this.DocumentOwner.Close(this);
        }


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
}