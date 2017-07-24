using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using YZXDMS.Models;
using System.Collections.Generic;
using System.IO.Ports;
using YZXDMS.DataProvider;
using System.Linq;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingStationViewModel
    {
        public ObservableCollection<DetectorModel> DetectorItems { get; set; }

        public ObservableCollection<DetectorModel> Station1 { get; set; } = new ObservableCollection<DetectorModel>();
        public ObservableCollection<DetectorModel> Station2 { get; set; } = new ObservableCollection<DetectorModel>();
        public ObservableCollection<DetectorModel> Station3 { get; set; } = new ObservableCollection<DetectorModel>();
        public ObservableCollection<DetectorModel> Station4 { get; set; } = new ObservableCollection<DetectorModel>();
        

        public SettingStationViewModel()
        {
            Init();
        }
        /// <summary>
        /// 初始化列表
        /// </summary>
        private void Init()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                //读取检测项目信息
                var query = db.Detectors.ToList();
                DetectorItems = new ObservableCollection<DetectorModel>();
                //将项目分配到各工位
                foreach (var item in query)
                {
                    switch (item.StationValue)
                    {
                        case 1:
                            Station1.Add(item);
                            break;
                        case 2:
                            Station2.Add(item);
                            break;
                        case 3:
                            Station3.Add(item);
                            break;
                        case 4:
                            Station4.Add(item);
                            break;
                        default:
                            DetectorItems.Add(item);
                            break;
                    }
                }
            }
        }

        public void Save()
        {           
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                var StationItems = db.Stations.ToList();

                var query1 = db.Stations.Single(x => x.StationName == "一工位");
                var query2 = db.Stations.Single(x => x.StationName == "二工位");
                var query3 = db.Stations.Single(x => x.StationName == "三工位");
                var query4 = db.Stations.Single(x => x.StationName == "四工位");


                var queryDet = db.Detectors.ToList();
                foreach (var item in queryDet)
                {
                    //重置工位，归零
                    item.StationValue = 0;
                }

                foreach (var item in Station1)
                {
                    queryDet.Single(x => x.Id == item.Id).StationValue = query1.Value;
                }
                foreach (var item in Station2)
                {
                    queryDet.Single(x => x.Id == item.Id).StationValue = query2.Value;
                }
                foreach (var item in Station3)
                {
                    queryDet.Single(x => x.Id == item.Id).StationValue = query3.Value;
                }
                foreach (var item in Station4)
                {
                    queryDet.Single(x => x.Id == item.Id).StationValue = query4.Value;
                }

                db.SaveChanges();


            }
        }

    }
}