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
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                var query = db.Detectors.ToList();
                DetectorItems = new ObservableCollection<DetectorModel>(query);
            }
        }

        public void Save()
        {
            //废话太多，有待修改
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
                    item.StationId = 0;
                }

                foreach (var item in Station1)
                {
                    queryDet.Single(x => x.Id == item.Id).StationId = query1.Id;
                }
                foreach (var item in Station2)
                {
                    queryDet.Single(x => x.Id == item.Id).StationId = query2.Id;
                }
                foreach (var item in Station3)
                {
                    queryDet.Single(x => x.Id == item.Id).StationId = query3.Id;
                }
                foreach (var item in Station4)
                {
                    queryDet.Single(x => x.Id == item.Id).StationId = query4.Id;
                }

                db.SaveChanges();


            }
        }

    }
}