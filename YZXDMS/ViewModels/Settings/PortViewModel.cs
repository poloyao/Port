using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.ComponentModel;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class PortViewModel: ViewModelBase, IDocumentContent
    {
        //public virtual PortConfigModel Item { get; set; } = new PortConfigModel();

        //public bool IsChange;

        //public PortViewModel()
        //{
        //    Item.Port = new System.IO.Ports.SerialPort();
        //    //Item.Protocol = new ProtocolModel();
        //}

        //protected override void OnParameterChanged(object parameter)
        //{
        //    base.OnParameterChanged(parameter);
        //    if (parameter != null)
        //    {
        //        Item = (PortConfigModel)parameter;
        //    }
        //}


        //public void Save()
        //{
        //    IsChange = true;
        //    DevExpress.Xpf.Core.DXMessageBox.Show("添加成功！");
        //    DocumentOwner.Close(this);
        //}

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