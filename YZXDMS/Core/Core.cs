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
            //当前检测线号 1
            CurrentLine = 1;
            GetCurrentDetectionList();
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
                dr.CarID = cid;
                dr.SerialData = item.jylsh;
                ResultItems.Add(dr);
            }

            #region 测试
                     
            ////ResultItems[0].Speed = DetectResultStatus.Wait;
            //ResultItems[0].Balancer = DetectResultStatus.NotChecked;
            ////ResultItems[0].Brake = DetectResultStatus.NotChecked;

            //ResultItems[1].Speed = DetectResultStatus.NotChecked;
            ////ResultItems[1].Balancer = DetectResultStatus.NotChecked;
            //ResultItems[1].Bottom = DetectResultStatus.NotChecked;

            //ResultItems[2].Speed = DetectResultStatus.NotChecked;
            //ResultItems[2].Shape = DetectResultStatus.NotChecked;
            ////ResultItems[2].Balancer = DetectResultStatus.NotChecked;

            //ResultItems[3].Speed = DetectResultStatus.NotChecked;

            #endregion
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
            ResultItems.Remove(sing);
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



        

        static ISpeedDetection speedD;

        static IDetection shapeD;
        static IDetection brakeD;
        static IDetection bottomD;
        static IDetection balacerD;

        private static readonly object SDLock = new object();

        static readonly object ResultLock = new object();

        /// <summary>
        /// 结果集队列
        /// </summary>
        static Queue<DetectResult> ResultQueue = new Queue<DetectResult>();
        
        /// <summary>
        /// 速度队列
        /// </summary>
        static Queue<DetectResult> SpeedQueue = new Queue<DetectResult>();
        /// <summary>
        /// 外检队列
        /// </summary>
        static Queue<DetectResult> ShapeQueue = new Queue<DetectResult>();
        /// <summary>
        /// 探平衡仪队列
        /// </summary>
        static Queue<DetectResult> BalancerQueue = new Queue<DetectResult>();
        /// <summary>
        /// 地盘队列
        /// </summary>
        static Queue<DetectResult> BottomQueue = new Queue<DetectResult>();
        /// <summary>
        /// 制动队列
        /// </summary>
        static Queue<DetectResult> BrakeQueue = new Queue<DetectResult>();


        

        /// <summary>
        /// 初始化检测项目的串口信息
        /// </summary>
        public static void InitDetectionDevice()
        {
            #region MyRegion


            //speedD = new TestSpeedDetection();

            //获取此项目的光电
            //var pvc =  GlobalPVC.GetInstance().GetItem(1, 1);

            //while (true)
            //{
            //    if (pvc.IsTrigger)
            //        break;
            //}

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

            #endregion

            InitSpeedDetection();

            shapeD = new TestShapeDetection(); 

            brakeD = new TestBrakeDetection();

            bottomD = new TestBottomDetection();

            balacerD = new TestBalancerDetection();
        }

        /// <summary>
        /// 初始化速度检测单元
        /// </summary>
         static void InitSpeedDetection()
        {
            //初始化检测模块
            speedD = new TestSpeedDetection();
            //获取工位中速度检测模块信息
            var speedUnit = GlobalStation.GetInstance().GetUintInStation(DetectionType.速度);
            if (speedUnit == null)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("未检测到速度模块");
                return;
            }
            //设置此检测项目的串口信息
            var speedPort = GlobalPort.GetInstance().GetPort(speedUnit.PortId);
            //此处设置
            speedD.SetPort(speedPort.Port);

            //获取此项目所调用的所有assis辅助信息
            List<AssistModel> assistItems;
            assistItems = GlobalPort.GetAssistitems(speedUnit);

            List<PVCModel> pvcs = new List<PVCModel>();
            if (assistItems != null)
            {
                //按index升序排列
                foreach (var item in assistItems.OrderBy(x => x.Index).ToList())
                {
                    if (item.AssistType == AssistDeviceType.光电设备)
                    {
                        var pvc = GlobalPVC.GetInstance().GetItem(item.PortId, item.Route);
                        if (pvc != null)
                            pvcs.Add(GlobalPVC.GetInstance().GetItem(item.PortId, item.Route));
                    }
                }
                //传入所有的光电
                speedD.SetPVCs(pvcs);
                speedD.SetLatticeScreen(new LatticeScreenOperate());
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("未检测到速度模块所用的辅助设备");
                return;
            }
            
        }

     

        /// <summary>
        /// 开始检测,获取当前待检列表车辆，
        /// </summary>
        public static void StartDetection()
        {

            //判断是否需要进入此检测模块
            if (CurrentDetectionList.Count() == 0)
                return;
            
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
                    //创建各检测项目的队列
                    SpeedQueue.Enqueue(item);
                    ShapeQueue.Enqueue(item);
                    BalancerQueue.Enqueue(item);
                    BottomQueue.Enqueue(item);
                    BrakeQueue.Enqueue(item);
                    //启动
                    StartDetect(item);
                    
                   
                }
            }

        }

        /// <summary>
        /// 启动检测项目组合
        /// </summary>
        /// <param name="currentResult"></param>
        private static void StartDetect(DetectResult currentResult)
        {
            Console.WriteLine($"创建{currentResult.CarID} 线程");
            var db = GetDBProvider();
            Task task = new TaskFactory().StartNew(() =>
            {
                //首线程，处理准备逻辑
            })
         
            .ContinueWith((action) =>
            {
                //StartSpeedUnit(currentResult);
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

                //等会再移除
                Thread.Sleep(2000);

                lock (SyncCurrentDetectionList)
                {
                    var sing = ResultItems.Single(x => x.SerialData == currentResult.SerialData);
                    ResultItems.Remove(sing);
                }
                var _current = CurrentDetectionList.Single(x => x.jylsh == currentResult.SerialData);
                CurrentDetectionList.Remove(_current);
                lock (SDLock)
                {
                    Console.WriteLine($"移除{currentResult.CarID}");
                    ResultQueue.Dequeue();
                }
            });

        }

        #region 启动检测项目单元

       
        private static void StartSpeedUnit(DetectResult currentResult)
        {
            var db = GetDBProvider();
            bool isWhile = true;
            while (isWhile)
            {
                Thread.Sleep(30);
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
            SimulationSleep();

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
                Thread.Sleep(30);
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
            SimulationSleep();

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
                Thread.Sleep(30);
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
            SimulationSleep();

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
                Thread.Sleep(30);
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
            SimulationSleep();

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
                Thread.Sleep(30);
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
            //模拟其他项目
            SimulationSleep();

            //判断是否要执行此项目
            if (currentResult.Shape == DetectResultStatus.NotChecked)
            {
                shapeD.Reset();
                ShapeQueue.Dequeue();
                Console.WriteLine($"{currentResult.CarID} 使用Shape完毕");
                return;
            }            

            currentResult.Shape = DetectResultStatus.Qualified;

            shapeD.Reset();
            ShapeQueue.Dequeue();
            Console.WriteLine($"{currentResult.CarID} 使用Shape完毕");
        }



        #endregion


        #region 模拟辅助

        /// <summary>
        /// 模拟等待延迟
        /// </summary>
        private static void SimulationSleep()
        {
            Thread.Sleep(new Random(Guid.NewGuid().GetHashCode()).Next(1,3) * 1000);
        }


        #endregion

    }
}
