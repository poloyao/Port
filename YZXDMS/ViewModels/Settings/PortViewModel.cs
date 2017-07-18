using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using System.ComponentModel;
using YZXDMS.DataProvider;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class PortViewModel: ViewModelBase, IDocumentContent
    {
        public virtual PortConfig Item { get; set; } = new PortConfig();

        public bool IsChange;

        public PortViewModel()
        {

        }

        protected override void OnParameterChanged(object parameter)
        {
            base.OnParameterChanged(parameter);
            if (parameter != null)
            {
                Item = (PortConfig)parameter;
            }
        }


        public void Save()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                db.Ports.Add(Item);
                db.SaveChanges();
            }

            IsChange = true;
            DevExpress.Xpf.Core.DXMessageBox.Show("添加成功！");
            if (DocumentOwner != null)
                DocumentOwner.Close(this);
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