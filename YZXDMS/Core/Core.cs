using DevExpress.Xpf.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
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
            //OnRemoveDetectionHandler += RemoveDetection;
        }

        /// <summary>
        /// 全局检测模式
        /// </summary>
        public static DetectionMode DetectionMode { get; set; }


        public static DispatcherTimer MasterDispatcherTimer = new DispatcherTimer();

        public delegate void RemoveDetectionDelegate(WaitDetection item);
        /// <summary>
        /// 委托移除列表
        /// </summary>
        public static event RemoveDetectionDelegate OnRemoveDetectionHandler;

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
        /// 当前检测队列锁
        /// </summary>
        private static object SyncCurrentDetectionList = new object();
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
                if (CurrentDetectionList.Where(x => x.Id == item.Id && x.jylsh == item.jylsh).Count() > 0)
                {
                    continue;
                }
                CurrentDetectionList.Add(item);
            }

            DetectionInit();
        }

        /// <summary>
        /// 初始化检测结果列表
        /// </summary>
        static void DetectionInit()
        {
            var db = GetDBProvider();
            if (ResultItems == null)
                ResultItems = new ObservableCollectionCore<DetectResult>();
            foreach (var item in CurrentDetectionList)
            {
                var cid = db.GetCarInfoItem(item.CarInfoId).HPHM; ;
                if (ResultItems.Where(x => x.CarID == cid).Count() > 0)
                    continue;

                DetectResult dr = new DetectResult();
                dr.CarID = cid;//db.GetCarInfoItem(item.CarInfoId).HPHM;
                dr.SerialData = item.jylsh;
                ResultItems.Add(dr);
            }
            //ResultItems[0].Speed = DetectResultStatus.Wait;
            ResultItems[0].Balancer = DetectResultStatus.NotChecked;
            //ResultItems[0].Brake = DetectResultStatus.NotChecked;

            ResultItems[1].Speed = DetectResultStatus.NotChecked;
            //ResultItems[1].Balancer = DetectResultStatus.NotChecked;
            ResultItems[1].Bottom = DetectResultStatus.NotChecked;

            ResultItems[2].Speed = DetectResultStatus.NotChecked;
            ResultItems[2].Shape = DetectResultStatus.NotChecked;
            //ResultItems[2].Balancer = DetectResultStatus.NotChecked;

            ResultItems[3].Speed = DetectResultStatus.NotChecked;
        }
        /// <summary>
        /// 追加新检测车辆到检测结果列表
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
        /// 将检测完毕的车辆移除出检测结果列表
        /// </summary>
        /// <param name="item"></param>
        static void RemoveDetection(WaitDetection item)
        {
            var sing = ResultItems.Single(x => x.SerialData == item.jylsh);
            //var index = ResultItems.IndexOf(sing);
            // ResultItems.BeginUpdate();
            ResultItems.Remove(sing);
            // ResultItems.EndUpdate();
            CurrentDetectionList.Remove(CurrentDetectionList[0]);
        }


        /// <summary>
        /// 添加车辆到当前检测结果列表
        /// </summary>
        /// <param name="item"></param>
        public static void AddCurrentCar(WaitDetection item)
        {
            var db = GetDBProvider();
            db.SetWaitDetection(item, 1);
            AppendDetection(item);
            GetCurrentDetectionList();
        }




        ///// <summary>
        ///// 速度检测项目
        ///// </summary>
        //static ISpeedDetection sd = new TestSpeedDetection();

        static ISpeedDetection speedD;// = new TestSpeedDetection();

        static IDetection shapeD;// = new TestShapeDetection();

        static IDetection brakeD;// = new TestBrakeDetection();

        static IDetection bottomD;// = new TestBottomDetection();

        static IDetection balacerD;// = new TestBalancerDetection();

        private static readonly object SDLock = new object();

        static readonly object ResultLock = new object();

        static Queue<DetectResult> ResultQueue = new Queue<DetectResult>();

        static Queue<WaitDetection> currentQueue = new Queue<WaitDetection>();

        static Queue<DetectResult> SpeedQueue = new Queue<DetectResult>();
        static Queue<DetectResult> ShapeQueue = new Queue<DetectResult>();
        static Queue<DetectResult> BalancerQueue = new Queue<DetectResult>();
        static Queue<DetectResult> BottomQueue = new Queue<DetectResult>();
        static Queue<DetectResult> BrakeQueue = new Queue<DetectResult>();

        /// <summary>
        /// 初始化检测项目的串口信息
        /// </summary>
        static void InitDetectionDevice()
        {
            speedD = new TestSpeedDetection();
            //ILatticeScreenOperate lso = new LatticeScreenOperate();
            ////获取速度模块
            //var speedDetect = Helpers.DeviceHelper.GetDetection(DetectionType.速度);
            ////获取速度串口
            //var speedInfo = Helpers.DeviceHelper.GetDetectionInfo(speedDetect);
            //speedD.SetPort(speedInfo.Port);
            //var leds = speedDetect.AssistList.Where(x => x.PortConfig.DeviceType == Models.DeviceType.灯屏设备);
            //leds.First().PortConfig.


            //创建速度模块实例
            //speed = new SpeedDetection(speedInfo.Port, speedInfo.Detection.PortConfig);
            //speed.Init();

            ////获取速度模块使用的光电设备
            //var gd = speedDetect.AssistList.Single(x => x.Assist.DeviceType == Models.AssistDeviceType.光电设备 && x.RouteNumber == 1);
            ////获取光电状态实例
            //var pm = GlobalPhotoelectric.GetPhotoelectric(gd);
            //lso.SetPort()
            //speedD.SetLatticeScreen()

            shapeD = new TestShapeDetection(); 

            brakeD = new TestBrakeDetection();

            bottomD = new TestBottomDetection();

            balacerD = new TestBalancerDetection();
        }

        /// <summary>
        /// 开始检测,获取当前待检列表车辆，
        /// </summary>
        public static void StartDetection()
        {

            //判断是否需要进入此检测模块
            if (CurrentDetectionList.Count() == 0)
                return;        
            
            //Semaphore se = new Semaphore(1, 2);
            //se.WaitOne();
            //se.Release();

            lock (ResultLock)
            {
                for (int i = 0; i < ResultItems.Count(); i++)
                {
                    var item = ResultItems[i];

                    if (ResultQueue.Contains(item))
                    {
                        continue;
                    }

                    ResultQueue.Enqueue(item);
                    StartDetect(item);
                    //创建各检测项目的队列
                    SpeedQueue.Enqueue(item);
                    ShapeQueue.Enqueue(item);
                    BalancerQueue.Enqueue(item);
                    BottomQueue.Enqueue(item);
                    BrakeQueue.Enqueue(item);
                }
            }

        }


        private static void StartDetect(DetectResult currentResult)
        {
            Console.WriteLine($"创建{currentResult.CarID} 线程");
            var db = GetDBProvider();
            Task task = new TaskFactory().StartNew(() =>
            {

            })
            #region MyRegion
            //.ContinueWith((action) =>
            //{
            //    Console.WriteLine($"启动{currentResult.CarID} 线程");
            //    #region MyRegion
            //    {
            //        #region MyRegion

            //        if (currentResult.Speed != DetectResultStatus.Wait)
            //            return;


            //        while (true)
            //        {

            //            ////获取当前状态
            //            switch (sd.GetCurrentStatus())
            //            {
            //                case DetectionStatus.IDLE:
            //                    break;
            //                case DetectionStatus.WORK:
            //                    continue;
            //                case DetectionStatus.ABN:
            //                    continue;
            //            }
            //            lock (SDLock)
            //            {

            //                if (currentResult.Speed == DetectResultStatus.NotChecked)
            //                    return;


            //                //传入车籍后，应申明当前状态为WORK
            //                sd.SetCurrentStatusWORK();
            //                var carinfo = db.GetCarInfoItem(currentResult.CarID);                              

            //                sd.SetCarInfo(carinfo);

            //                //Console.WriteLine($"{currentTemp.Id} 线程开始---运行");
            //                var speedResult = sd.StartDetect();
            //                if (speedResult == null)
            //                {
            //                    //检测失败
            //                    return;
            //                }
            //                //保存结果
            //                db.AddSpeed(speedResult);

            //                //更新当前显示结果集
            //                //测试虚假延迟
            //                Thread.Sleep(2000);
            //                //Console.WriteLine($"{currentTemp.Id} 线程开始---复位");
            //                sd.Reset();
            //                currentResult.Speed = DetectResultStatus.Qualified;

            //                break;
            //            }
            //        }
            //        #endregion
            //    }
            //    #endregion
            //})
            #endregion
            .ContinueWith((action) =>
            {
                StartSpeedUnit(currentResult);
            })
            .ContinueWith((action) =>
            {
                StartShapeUnit(currentResult);
            })
            .ContinueWith((action) =>
            {
                StartBalancerUnit(currentResult);
            })
            .ContinueWith((action) =>
            {
                StartBottomUnit(currentResult);
            })
            .ContinueWith((action) =>
            {
                StartBrakeUnit(currentResult);
            })
            .ContinueWith((action) =>
            {
                //全部检测项目完成后更新队列
                //将完毕检验完毕的移除队列
                //MyBug 不合格车辆是否暂不移除？

                //lock (SyncCurrentDetectionList)
                //{
                //    var sing = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);
                //    ResultItems.Remove(sing);
                //}

                //CurrentDetectionList.Remove(currentTemp);
                //lock (SDLock)
                //{
                Console.WriteLine($"移除{currentResult.CarID}");
                ResultQueue.Dequeue();
                //}
            });

        }
        private static void StartSpeedUnit(DetectResult currentResult)
        {
            var db = GetDBProvider();
            bool isWhile = true;
            while (isWhile)
            {
                if (SpeedQueue.Peek() != currentResult)
                    continue;

                switch (speedD.GetCurrentStatus())
                {
                    case DetectionStatus.IDLE:
                        isWhile = false;
                        break;
                    case DetectionStatus.WORK:
                        continue;
                    case DetectionStatus.ABN:
                        continue;
                }
            }

            speedD.SetCurrentStatusWORK();
            Console.WriteLine($"{currentResult.CarID} 使用Speed");
            //模拟其他项目
            Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(5, 10) * 1000);

            if (currentResult.Speed == DetectResultStatus.NotChecked)
            {
                speedD.Reset();
                SpeedQueue.Dequeue();
                Console.WriteLine($"{currentResult.CarID} 使用Speed完毕");
                return;
            }

            //传入车籍后
            var carinfo = db.GetCarInfoItem(currentResult.CarID);
            speedD.SetCarInfo(carinfo);
            var speedResult = speedD.StartDetect();
            if (speedResult == null)
            {
                //检测失败
                //return;
                throw new Exception("检测失败");
            }
            //保存结果
            db.AddSpeed(speedResult);

            //更新当前显示结果集
            currentResult.Speed = DetectResultStatus.Qualified;

            speedD.Reset();
            SpeedQueue.Dequeue();
            Console.WriteLine($"{currentResult.CarID} 使用Speed完毕");
        }
        private static void StartBrakeUnit(DetectResult currentResult)
        {
            bool isWhile = true;
            while (isWhile)
            {
                if (BrakeQueue.Peek() != currentResult)
                    continue;

                switch (brakeD.GetCurrentStatus())
                {
                    case DetectionStatus.IDLE:
                        isWhile = false;
                        break;
                    case DetectionStatus.WORK:
                        continue;
                    case DetectionStatus.ABN:
                        continue;
                }
            }

            brakeD.SetCurrentStatusWORK();
            Console.WriteLine($"{currentResult.CarID} 使用Brake");
            //模拟其他项目
            Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(5, 10) * 1000);

            if (currentResult.Brake == DetectResultStatus.NotChecked)
            {
                brakeD.Reset();
                BrakeQueue.Dequeue();
                Console.WriteLine($"{currentResult.CarID} 使用Brake完毕");
                return;
            }

            currentResult.Brake = DetectResultStatus.Qualified;

            brakeD.Reset();
            BrakeQueue.Dequeue();
            Console.WriteLine($"{currentResult.CarID} 使用Brake完毕");
        }

        private static void StartBottomUnit(DetectResult currentResult)
        {
            bool isWhile = true;
            while (isWhile)
            {
                if (BottomQueue.Peek() != currentResult)
                    continue;

                switch (bottomD.GetCurrentStatus())
                {
                    case DetectionStatus.IDLE:
                        isWhile = false;
                        break;
                    case DetectionStatus.WORK:
                        continue;
                    case DetectionStatus.ABN:
                        continue;
                }
            }

            bottomD.SetCurrentStatusWORK();
            Console.WriteLine($"{currentResult.CarID} 使用Bottom");
            //模拟其他项目
            Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(5, 10) * 1000);

            if (currentResult.Bottom == DetectResultStatus.NotChecked)
            {
                bottomD.Reset();
                BottomQueue.Dequeue();
                Console.WriteLine($"{currentResult.CarID} 使用Bottom完毕");
                return;
            }

            currentResult.Bottom = DetectResultStatus.Qualified;

            bottomD.Reset();
            BottomQueue.Dequeue();
            Console.WriteLine($"{currentResult.CarID} 使用Bottom完毕");
        }

        private static void StartBalancerUnit(DetectResult currentResult)
        {
            bool isWhile = true;
            while (isWhile)
            {
                if (BalancerQueue.Peek() != currentResult)
                    continue;

                switch (balacerD.GetCurrentStatus())
                {
                    case DetectionStatus.IDLE:
                        isWhile = false;
                        break;
                    case DetectionStatus.WORK:
                        continue;
                    case DetectionStatus.ABN:
                        continue;
                }
            }

            balacerD.SetCurrentStatusWORK();
            Console.WriteLine($"{currentResult.CarID} 使用Balancer");
            //模拟其他项目
            Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(5, 10) * 1000);

            if (currentResult.Balancer == DetectResultStatus.NotChecked)
            {
                balacerD.Reset();
                BalancerQueue.Dequeue();
                Console.WriteLine($"{currentResult.CarID} 使用Balancer完毕");
                return;
            }

            currentResult.Balancer = DetectResultStatus.Qualified;

            balacerD.Reset();
            BalancerQueue.Dequeue();
            Console.WriteLine($"{currentResult.CarID} 使用Balancer完毕");
        }

        private static void StartShapeUnit(DetectResult currentResult)
        {
            bool isWhile = true;
            while (isWhile)
            {
                if (ShapeQueue.Peek() != currentResult)
                    continue;

                switch (shapeD.GetCurrentStatus())
                {
                    case DetectionStatus.IDLE:
                        isWhile = false;
                        break;
                    case DetectionStatus.WORK:
                        continue;
                    case DetectionStatus.ABN:
                        continue;
                }
            }

            shapeD.SetCurrentStatusWORK();
            Console.WriteLine($"{currentResult.CarID} 使用Shape");

            //判断是否要执行此项目
            if (currentResult.Shape == DetectResultStatus.NotChecked)
            {
                shapeD.Reset();
                ShapeQueue.Dequeue();
                Console.WriteLine($"{currentResult.CarID} 使用Shape完毕");
                return;
            }

            Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(5, 10) * 1000);

            currentResult.Shape = DetectResultStatus.Qualified;

            shapeD.Reset();
            ShapeQueue.Dequeue();
            Console.WriteLine($"{currentResult.CarID} 使用Shape完毕");
        }

        ///// <summary>
        ///// 开始检测
        ///// </summary>
        ///// <param name="currentTemp"></param>
        //private static void StartDetect(WaitDetection currentTemp)
        //{
        //    Console.WriteLine($"创建{currentTemp.Id} 线程");
        //    var db = GetDBProvider();
        //    Task task = new TaskFactory().StartNew(() =>
        //    {

        //    })
        //    .ContinueWith((action) =>
        //    {
        //        Console.WriteLine($"启动{currentTemp.Id} 线程");
        //        #region MyRegion
        //        {
        //            #region MyRegion

        //            var single = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);//.Speed = DetectResultStatus.Qualified;
        //            var index = ResultItems.IndexOf(single);
        //            if (ResultItems[index].Speed != DetectResultStatus.Wait)
        //                return;


        //            while (true)
        //            {

        //                ////获取当前状态
        //                switch (sd.GetCurrentStatus())
        //                {
        //                    case DetectionStatus.IDLE:
        //                        break;
        //                    case DetectionStatus.WORK:
        //                        continue;
        //                    case DetectionStatus.ABN:
        //                        continue;
        //                }
        //                lock (SDLock)
        //                {
        //                    //判断是否需要开始检测此车籍
        //                    if (currentQueue.Count() > 0)
        //                    {
        //                        if (currentQueue.Peek() == currentTemp)
        //                        {
        //                            //currentQueue.Dequeue();
        //                        }
        //                        else
        //                        {
        //                            continue;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("线程错误");
        //                    }

        //                    if (ResultItems[index].Speed == DetectResultStatus.NotChecked)
        //                        return;


        //                    //传入车籍后，应申明当前状态为WORK
        //                    sd.SetCurrentStatusWORK();
        //                    Console.WriteLine($"{currentTemp.Id} 线程开始---工作");
        //                    var carinfo = db.GetCarInfoItem(currentTemp.CarInfoId);

        //                    sd.SetCarInfo(carinfo);

        //                    //Console.WriteLine($"{currentTemp.Id} 线程开始---运行");
        //                    var speedResult = sd.StartDetect();
        //                    if (speedResult == null)
        //                    {
        //                        //检测失败
        //                        return;
        //                    }
        //                    //保存结果
        //                    db.AddSpeed(speedResult);

        //                    //更新当前显示结果集
        //                    //测试虚假延迟
        //                    Thread.Sleep(2000);
        //                    //Console.WriteLine($"{currentTemp.Id} 线程开始---复位");
        //                    sd.Reset();
        //                    ResultItems[index].Speed = DetectResultStatus.Qualified;

        //                    break;
        //                }
        //            }
        //            #endregion
        //        }
        //        #endregion
        //    })
        //    .ContinueWith((action) =>
        //    {
        //        var single = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);//.Speed = DetectResultStatus.Qualified;
        //        var index = ResultItems.IndexOf(single);
        //        if (ResultItems[index].Shape != DetectResultStatus.Wait)
        //            return;
        //        //模拟其他项目
        //        bool isWhile = true;
        //        while (isWhile)
        //        {


        //            switch (shapeD.GetCurrentStatus())
        //            {
        //                case DetectionStatus.IDLE:
        //                    isWhile = false;
        //                    break;
        //                case DetectionStatus.WORK:
        //                    continue;
        //                case DetectionStatus.ABN:
        //                    continue;
        //            }
        //        }

        //        shapeD.SetCurrentStatusWORK();


        //        Thread.Sleep(2000);
        //        Console.WriteLine($"{currentTemp.Id} [Shape] 线程开始");


        //        if (ResultItems[index].Shape == DetectResultStatus.NotChecked)
        //        {
        //            shapeD.Reset();
        //            return;
        //        }

        //        ResultItems[index].Shape = DetectResultStatus.Qualified;

        //        shapeD.Reset();

        //    })
        //    .ContinueWith((action) =>
        //    {
        //        var single = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);//.Speed = DetectResultStatus.Qualified;
        //        var index = ResultItems.IndexOf(single);
        //        if (ResultItems[index].Balancer != DetectResultStatus.Wait)
        //            return;
        //        bool isWhile = true;
        //        while (isWhile)
        //        {


        //            switch (balacerD.GetCurrentStatus())
        //            {
        //                case DetectionStatus.IDLE:
        //                    isWhile = false;
        //                    break;
        //                case DetectionStatus.WORK:
        //                    continue;
        //                case DetectionStatus.ABN:
        //                    continue;
        //            }
        //        }

        //        balacerD.SetCurrentStatusWORK();
        //        //模拟其他项目
        //        Thread.Sleep(2000);
        //        Console.WriteLine($"{currentTemp.Id} [Balancer] 线程开始");
        //        if (ResultItems[index].Balancer == DetectResultStatus.NotChecked)
        //        {
        //            balacerD.Reset();
        //            return;
        //        }

        //        ResultItems[index].Balancer = DetectResultStatus.Qualified;

        //        balacerD.Reset();
        //    })
        //    .ContinueWith((action) =>
        //    {
        //        var single = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);//.Speed = DetectResultStatus.Qualified;
        //        var index = ResultItems.IndexOf(single);
        //        if (ResultItems[index].Bottom != DetectResultStatus.Wait)
        //            return;

        //        bool isWhile = true;
        //        while (isWhile)
        //        {


        //            switch (bottomD.GetCurrentStatus())
        //            {
        //                case DetectionStatus.IDLE:
        //                    isWhile = false;
        //                    break;
        //                case DetectionStatus.WORK:
        //                    continue;
        //                case DetectionStatus.ABN:
        //                    continue;
        //            }
        //        }

        //        bottomD.SetCurrentStatusWORK();
        //        //模拟其他项目
        //        Thread.Sleep(2000);
        //        Console.WriteLine($"{currentTemp.Id} [bottom] 线程开始");

        //        if (ResultItems[index].Bottom == DetectResultStatus.NotChecked)
        //        {
        //            bottomD.Reset();
        //            return;
        //        }

        //        ResultItems[index].Bottom = DetectResultStatus.Qualified;

        //        bottomD.Reset();
        //    })
        //    .ContinueWith((action) =>
        //    {
        //        var single = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);//.Speed = DetectResultStatus.Qualified;
        //        var index = ResultItems.IndexOf(single);
        //        if (ResultItems[index].Brake != DetectResultStatus.Wait)
        //            return;

        //        bool isWhile = true;
        //        while (isWhile)
        //        {


        //            switch (brakeD.GetCurrentStatus())
        //            {
        //                case DetectionStatus.IDLE:
        //                    isWhile = false;
        //                    break;
        //                case DetectionStatus.WORK:
        //                    continue;
        //                case DetectionStatus.ABN:
        //                    continue;
        //            }
        //        }

        //        brakeD.SetCurrentStatusWORK();
        //        //模拟其他项目
        //        Thread.Sleep(2000);
        //        Console.WriteLine($"{currentTemp.Id} [Brake] 线程开始");

        //        if (ResultItems[index].Brake == DetectResultStatus.NotChecked)
        //        {
        //            brakeD.Reset();
        //            return;
        //        }

        //        ResultItems[index].Brake = DetectResultStatus.Qualified;

        //        brakeD.Reset();
        //    })
        //    .ContinueWith((action) =>
        //    {
        //        //全部检测项目完成后更新队列
        //        //将完毕检验完毕的移除队列
        //        //MyBug 不合格车辆是否暂不移除？

        //        //lock (SyncCurrentDetectionList)
        //        //{
        //        //    var sing = ResultItems.Single(x => x.SerialData == currentTemp.jylsh);
        //        //    ResultItems.Remove(sing);
        //        //}

        //        //CurrentDetectionList.Remove(currentTemp);

        //        currentQueue.Dequeue();
        //    });

        //}
    }
}
