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
        public ObservableCollection<DetectorModel> Station5 { get; set; } = new ObservableCollection<DetectorModel>();

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
                query = query.OrderBy(x => x.StationIndex).ToList();
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
                        case 5:
                            Station5.Add(item);
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

                var query1 = db.Stations.Single(x => x.Value == 1);
                var query2 = db.Stations.Single(x => x.Value == 2);
                var query3 = db.Stations.Single(x => x.Value == 3);
                var query4 = db.Stations.Single(x => x.Value == 4);
                var query5 = db.Stations.Single(x => x.Value == 5);

                var queryDet = db.Detectors.ToList();
                foreach (var item in queryDet)
                {
                    //重置工位，归零
                    item.StationValue = 0;
                }

                //foreach (var item in Station1)
                //{
                //    queryDet.Single(x => x.Id == item.Id).StationValue = query1.Value;
                //}              

                //foreach (var item in Station2)
                //{
                //    queryDet.Single(x => x.Id == item.Id).StationValue = query2.Value;
                //}
                //foreach (var item in Station3)
                //{
                //    queryDet.Single(x => x.Id == item.Id).StationValue = query3.Value;
                //}
                //foreach (var item in Station4)
                //{
                //    queryDet.Single(x => x.Id == item.Id).StationValue = query4.Value;
                //}
                //foreach (var item in Station5)
                //{
                //    queryDet.Single(x => x.Id == item.Id).StationValue = query5.Value;
                //}

                for (int i = 0; i < Station1.Count(); i++)
                {
                    var query = queryDet.Single(x => x.Id == Station1[i].Id);
                    query.StationValue = query1.Value;
                    query.StationIndex = i + 1;
                }

                for (int i = 0; i < Station2.Count(); i++)
                {
                    var query = queryDet.Single(x => x.Id == Station2[i].Id);
                    query.StationValue = query2.Value;
                    query.StationIndex = i + 1;
                }

                for (int i = 0; i < Station3.Count(); i++)
                {
                    var query = queryDet.Single(x => x.Id == Station3[i].Id);
                    query.StationValue = query3.Value;
                    query.StationIndex = i + 1;
                }

                for (int i = 0; i < Station4.Count(); i++)
                {
                    var query = queryDet.Single(x => x.Id == Station4[i].Id);
                    query.StationValue = query4.Value;
                    query.StationIndex = i + 1;
                }
                for (int i = 0; i < Station5.Count(); i++)
                {
                    var query = queryDet.Single(x => x.Id == Station5[i].Id);
                    query.StationValue = query5.Value;
                    query.StationIndex = i + 1;
                }

                db.SaveChanges();


            }
        }

    }
}