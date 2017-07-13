using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.DataProvider;
using YZXDMS.Detections;
using YZXDMS.Model;
using YZXDMS.Models;
using YZXDMS.ViewModels;

namespace YZXDMS.Core
{
    public class Core
    {
        #region 属性



        #endregion


        private static readonly Core instance = new Core();


        /// <summary>
        /// 获取Core单例
        /// </summary>
        /// <returns></returns>
        public static Core GetInstance()
        {
            return instance;
        }

        private Core()
        {
            CurrentLine = 1;
            GetCurrentDetectionList();
        }

        /// <summary>
        /// 全局检测模式
        /// </summary>
        public static  DetectionMode DetectionMode { get; set; }

        /// <summary>
        /// 当前检测线号
        /// </summary>
        public static int CurrentLine { get; set; }

        /// <summary>
        /// 当前检测结果列表
        /// </summary>
        public static ObservableCollectionCore<DetectResult> ResultItems { get; set; }
        /// <summary>
        /// 当前检测队列
        /// </summary>
        public static ObservableCollectionCore<WaitDetection> CurrentDetectionList { get; set; }

        /// <summary>
        /// 获取数据操作实例
        /// </summary>
        /// <returns></returns>
        public static IDBProvider GetDBProvider()
        {
            return new SQLServerDBProvider();
        }

        public static Users User { get; set; }

        public static void GetDeviceConfig()
        {

        }


        public static void SetDeviceConfig()
        {

        }


        /// <summary>
        /// 设置当前检测队列车辆信息
        /// </summary>
        static void GetCurrentDetectionList()
        {
            var db = GetDBProvider();
            var gwd = db.GetWaitDetectionList(1, CurrentLine);
            if (CurrentDetectionList == null)
                CurrentDetectionList = new ObservableCollectionCore<WaitDetection>();
            foreach (var item in gwd)
            {
                CurrentDetectionList.Add(item);
            }

            DetectionInit();
        }

        /// <summary>
        /// 初始化检测列表
        /// </summary>
        static void DetectionInit()
        {
            var db = GetDBProvider();
            if (ResultItems == null)
                ResultItems = new ObservableCollectionCore<DetectResult>();
            foreach (var item in CurrentDetectionList)
            {
                DetectResult dr = new DetectResult();
                dr.CarID = db.GetCarInfoItem(item.CarInfoId).HPHM;
                dr.SerialData = item.jylsh;
                ResultItems.Add(dr);
            }
        }
        /// <summary>
        /// 追加新检测车辆
        /// </summary>
        /// <param name="item"></param>
        static void AppendDetection(WaitDetection item)
        {
            var db = GetDBProvider();
            DetectResult dr = new DetectResult();
            dr.CarID = db.GetCarInfoItem(item.CarInfoId).HPHM;
            dr.SerialData = item.jylsh;
            ResultItems.Add(dr);
        }


        /// <summary>
        /// 添加车辆到当前检测列表
        /// </summary>
        /// <param name="item"></param>
        public static void AddCurrentCar(WaitDetection item)
        {
            var db = GetDBProvider();
            db.SetWaitDetection(item, 1);
            AppendDetection(item);
        }

     



        static ISpeedDetection sd;
        /// <summary>
        /// 开始检测,获取当前待检列表车辆，
        /// </summary>
        public static void StartDetection()
        {
            var db = GetDBProvider();
            //获取当前待检车辆列表
            //var gwd = db.GetWaitDetectionList(1, CurrentLine);
            sd = new TestSpeedDetection();

            Task task = new TaskFactory().StartNew((new Action(() =>
            {
                //判断是否需要进入此检测模块

                while (true)
                {
                    //获取当前状态
                    switch (sd.GetCurrentStatus())
                    {
                        case DetectionStatus.IDLE:
                            break;
                        case DetectionStatus.WORK:
                            continue;
                        case DetectionStatus.ABN:
                            continue;
                    }
                    var carinfo = db.GetCarInfoItem(CurrentDetectionList[0].CarInfoId);
                    sd.SetCarInfo(carinfo);

                    var speedResult = sd.StartDetect();
                    if (speedResult == null)
                    {
                        //检测失败
                        return;
                    }
                    //保存结果
                    db.AddSpeed(speedResult);
                    //更新当前显示结果集
                    var single = ResultItems.Single(x => x.SerialData == CurrentDetectionList[0].jylsh);//.Speed = DetectResultStatus.Qualified;
                    var index = ResultItems.IndexOf(single);
                    //ResultItems.BeginUpdate();
                    ResultItems[index].Speed = DetectResultStatus.Qualified;
                    //ResultItems.EndUpdate();

                    //全部完成后更新队列



                    //var srd = sd.GetSpeedResultData();

                    //根据结果


                    //dispatcherService.BeginInvoke(() =>
                    //{
                    //    ResultItems.BeginUpdate();
                    //    ResultItems[0].CarID = "完成";
                    //    ResultItems.EndUpdate();
                    //});
                    break;
                }
            })));
        }




    }
}
