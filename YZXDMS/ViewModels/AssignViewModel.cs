using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Model;
using System.Collections.ObjectModel;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class AssignViewModel
    {

        public virtual ObservableCollection<WaitDetection> WaitItems { get; set; }

        public virtual ObservableCollection<WaitDetection> CurrentItems { get; set; }

        //假设的当前线路号
        int _line = 1;

        public AssignViewModel()
        {
            Init();
        }

        void Init()
        {
            var query = Core.Core.GetDBProvider().GetWaitDetectionList();
            this.WaitItems = new ObservableCollection<WaitDetection>(query);
            var query2 = Core.Core.GetDBProvider().GetWaitDetectionList(1, _line);
            this.CurrentItems = new ObservableCollection<WaitDetection>(query2);
        }

        [Command(CanExecuteMethodName = "CanAddCurrent")]
        public void AddCurrent(WaitDetection item)
        {
            Core.Core.GetDBProvider().SetWaitDetection(item, _line);
            Init();
        }

        public bool CanAddCurrent(WaitDetection item)
        {
            if (item != null)
                return true;
            return false;
        }

    }
}