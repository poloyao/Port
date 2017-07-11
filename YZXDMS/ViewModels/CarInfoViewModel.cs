using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using MySql.Data.MySqlClient;
using System.Data;
using YZXDMS.Model;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class CarInfoViewModel
    {

        public CarInfoViewModel()
        {
            UpdataItems();

            //insert into VEH_AJJG.JYDLXX(JYLSH, JYJGBH, JCXDH, XH, HPZL, HPHM, CLSBDH, FDJH, CSYS, SYXZ, CCDJRQ, JYRQ, JYYXQZ, BXZZRQ, RLZL, GL, ZS, ZJ, QLJ, HLJ, ZZL, ZBZL, CCRQ, QDXS, ZCZS, ZCZW, ZZS, ZZLY, QZDZ, YGDDTZ, ZXZXJXS, LCBDS, JYXM, JYLB, BHGX, CCDLSJ, DLSJ, JYCS, DLY, YCY, WJY, DTJYY, DPJYY, CLPP1, CLXH, SYR, CLLX, CWKC, CWKK, CWKG, CLYT, YTSX, DLYSFZH, YCYSFZH, WJYSFZH, DTJYYSFZH, DPJYYSFZH, CLSSLB, JCXLB, SJR, SJRSFZH)
            //values('J0022015052800037', '2100000044', null, '22010007034199', '02', '辽JW1115', '4JGBB86E87A235105', '27296730553088', 'J', 'A', to_date('30-05-2007', 'dd-mm-yyyy'), to_date('14-05-2014', 'dd-mm-yyyy'), to_date('31-05-2015', 'dd-mm-yyyy'), to_date('10-01-2015', 'dd-mm-yyyy'), 'A', 0, 2, 2915, 1520, 1500, 2915, 0, to_date('01-02-2007', 'dd-mm-yyyy'), '2', 1, '2', null, null, '03', '0', '1', null, 'B1,B2,B3,B0,H1,H2,H3,H4,S1,F1,C1', '01', null, to_date('28-05-2015 15:34:27', 'dd-mm-yyyy hh24:mi:ss'), to_date('28-05-2015 15:34:27', 'dd-mm-yyyy hh24:mi:ss'), 1, '徐婕', null, null, null, null, '(梅赛德斯奔驰ML350)', '4JGBB86E', '郭翠萍', 'K32', 4780, 2127, 1815, 'P1', '9', '210903196402210022', null, null, null, null, '01', '1', '郭翠萍', '210921194705150024');


        }

        public virtual ObservableCollection<CarInfo> Items { get; set; }

        public virtual ObservableCollection<WaitDetection> WaitItems { get; set; }


        void UpdataItems()
        {

            //var query = Core.Core.GetDBProvider().GetWaitDetectionList();
            //WaitItems = new ObservableCollection<WaitDetection>(query);

            var query = Core.Core.GetDBProvider().GetCarInfoList();
            Items = new ObservableCollection<CarInfo>(query);
            var query2 = Core.Core.GetDBProvider().GetWaitDetectionList();
            WaitItems = new ObservableCollection<WaitDetection>(query2);

        }

        public void AddCarInfo()
        {
            CarInfo item = new CarInfo();
            item.Name = "asdasd";
            if (Core.Core.GetDBProvider().AddCarInfoItem(item))
            {
                UpdataItems();
            }
            else
            {
                Console.WriteLine("AddCarInfo失败");
            }
    
            
        }

        public void AddWaitItem(CarInfo carInfo)
        {
            if (carInfo == null)
                return;
            if (Core.Core.GetDBProvider().AddWaitDetection(carInfo))
            {
                UpdataItems();
            }
            else
            {
                Console.WriteLine("AddWaitItem失败");
            }
            UpdataItems();
        }



        //public void Add()
        //{
        //    //Stopwatch sw2 = new Stopwatch();
        //    //sw2.Start();
        //    //jyjgxtEntities1 je = new jyjgxtEntities1();

        //    //var list = je.veh_code.Where(x => x.DMLB == 4).ToList();
        //    //var cjxx = je.jydlxx.First();
        //    //je.Dispose();
        //    //jyjgxtEntities1 je2 = new jyjgxtEntities1();
        //    //cjxx.SJR = "新人";
        //    //cjxx.JYLSH = $"J00220150528{new Random(Guid.NewGuid().GetHashCode()).Next(0, 100000)}";
            
        //    //je2.jydlxx.Add(cjxx);
        //    //var aaa3 = je2.jydlxx.Where(x => x.BHGX == "asda").ToList();
        //    //je2.SaveChanges();
        //    //sw2.Stop();
        //    //Console.WriteLine(sw2.ElapsedMilliseconds);
        //    //UpdataItems();
        //}


        //void GetCarInfo()
        //{
        //    //string M_str_sqlcon = "server=192.168.1.133;user id=root;password=root;database=jyjgxt";
        //    //MySqlConnection mycon = new MySqlConnection(M_str_sqlcon);

        //    //try
        //    //{
        //    //    mycon.Open();
        //    //    MySqlDataAdapter mda = new MySqlDataAdapter("select * from jydlxx", mycon);
        //    //    DataSet ds = new DataSet();
        //    //    mda.Fill(ds, "jydlxx");

        //    //    var www = Helpers.DataSetToEntityHelper.DataSetToEntityList<jydlxx>(ds, 0);
        //    //}
        //    //catch
        //    //{
        //    //    throw new Exception();
        //    //}
        //    //finally
        //    //{
        //    //    mycon.Close();
        //    //    mycon.Dispose();
        //    //}
        //}

    }
}