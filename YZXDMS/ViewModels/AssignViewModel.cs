using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Model;
using System.Collections.ObjectModel;
using System.Linq;

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
            Core.Core.AddCurrentCar(item);
           Init();
        }

        public bool CanAddCurrent(WaitDetection item)
        {
            //1.不能为null
            //2.在检测队列中不能重复
            if (item != null)
            {
                var query = Core.Core.CurrentDetectionList.SingleOrDefault(x => x.CarInfoId == item.CarInfoId);
                if (query != null)
                    return false;
                return true;
            }
            return false;
        }

    }
}