﻿using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.DataProvider;
using YZXDMS.Detections;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using YZXDMS.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.POCO;
using System.Windows.Threading;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class MasterViewModel
    {
        ISpeedDetection sd;

        public virtual ObservableCollectionCore<DetectResult> ResultItems { get; set; }

        protected IDispatcherService dispatcherService { get { return this.GetService<IDispatcherService>(); } }

   

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MasterViewModel()
        {
            //SpeedInit();
            Init();

        }

        ~MasterViewModel()
        {
            if (dispatcherTimer.IsEnabled)
                dispatcherTimer.Stop();
            
        }

        void Init()
        {

            ResultItems = Core.Core.ResultItems;

            //dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //dispatcherTimer.IsEnabled = true;
            //dispatcherTimer.Tick += dispatcherTimer_Tick;
            //dispatcherTimer.Start();

            //CRC16Helper.GetInstance().ReadYLZ(20f);
            //var speeds = Core.Core.GetDBProvider().GetSpeedList("");

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var speeds = Core.Core.GetDBProvider().GetSpeedList("");


            if(speeds.Count() > 0)
            {
                ResultItems.BeginUpdate();
                ResultItems[0].CarID = "完成";
                ResultItems.EndUpdate();
                dispatcherTimer.Stop();
            }
        }

        /// <summary>
        /// 速度台初始化
        /// </summary>
        void SpeedInit()
        {
            sd = new TestSpeedDetection();
            var sdStatus = sd.GetCurrentStatus();
            
            sd.SetPort(new System.IO.Ports.SerialPort());


           
            
        }
        
        public void StartAll()
        {

            Task task = new TaskFactory().StartNew((new Action(() =>
             {
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

                     sd.SetCarInfo(new CarInfo() { Name = "辽A4392F" });

                     sd.StartDetect();

                    // var srd = sd.GetSpeedResultData();

                     dispatcherService.BeginInvoke(() =>
                     {
                         ResultItems.BeginUpdate();
                         ResultItems[0].CarID = "完成";
                         ResultItems.EndUpdate();
                     });

                     
                     
                     break;
                 }
             })));
            
        }

        public void SpeedStart()
        {
            //StartAll();
            Core.Core.StartDetection();

        }

    }

    /// <summary>
    /// 检测结果状态
    /// </summary>
    public enum DetectResultStatus
    {
        /// <summary>
        /// 默认待检
        /// </summary>
        Wait = 0,
        /// <summary>
        /// 合格 O
        /// </summary>
        Qualified,
        /// <summary>
        /// 不合格 X
        /// </summary>
        Unqualified,
         /// <summary>
         /// 未检 -
         /// </summary>
        NotChecked,
    }


    /// <summary>
    /// 检测结果
    /// </summary>
    public class DetectResult
    {
        public string CarID { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialData { get; set; }

        /// <summary>
        /// 最终结果
        /// </summary>
        public bool Final { get; set; }


        /// <summary>
        /// 外检
        /// </summary>
        public DetectResultStatus Shape { get; set; }
        /// <summary>
        /// 侧滑
        /// </summary>
        public DetectResultStatus SideSlide { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public DetectResultStatus Speed { get; set; }
        /// <summary>
        /// 灯光
        /// </summary>
        public DetectResultStatus Light { get; set; }
        /// <summary>
        /// 制动
        /// </summary>
        public DetectResultStatus Brake { get; set; }
        /// <summary>
        /// 称重
        /// </summary>
        public DetectResultStatus Weigh { get; set; }
        /// <summary>
        /// 地盘
        /// </summary>
        public DetectResultStatus bottom { get; set; }
        /// <summary>
        /// 地盘间隙
        /// </summary>
        public DetectResultStatus BoottomInterval { get; set; }
        /// <summary>
        /// 声级计
        /// </summary>
        public DetectResultStatus SoundLevel { get; set; }
        /// <summary>
        /// 功率
        /// </summary>
        public DetectResultStatus Power { get; set; }
        /// <summary>
        /// 油耗
        /// </summary>
        public DetectResultStatus FuelConsumption { get; set; }
        /// <summary>
        /// 尾气
        /// </summary>
        public DetectResultStatus Exhaust { get; set; }
        /// <summary>
        /// 探平衡仪
        /// </summary>
        public DetectResultStatus Balancer { get; set; }
    }
    
    //[POCOViewModel]
    //public class MasterViewModel
    //{
    //    /// <summary>
    //    /// 速度光电状态
    //    /// </summary>
    //    public virtual bool IsPLM { get; set; }

    //    /// <summary>
    //    /// 速度点阵
    //    /// </summary>
    //    public virtual string DGC { get; set; }

    //    /// <summary>
    //    /// 气泵状态
    //    /// </summary>
    //    public virtual int PumpStatus { get; set; }

    //    /// <summary>
    //    /// 结果集
    //    /// </summary>
    //    public virtual int ResultData { get; set; }

    //    bool isPLMTemp;

    //    /// <summary>
    //    /// 为true时阻碍下次触发流程直到为false
    //    /// </summary>
    //    bool isSpeed;

    //    SpeedDetection speed;








    //    public MasterViewModel()
    //    {

    //    }

    //    /// <summary>
    //    /// 重置
    //    /// </summary>
    //    public void Reset()
    //    {
    //        isSpeed = false;
    //    }


    //    public void InitSpeedDevice()
    //    {
    //        DGC = "Wait...";
    //        //获取速度模块
    //        var speedDetect = Helpers.DeviceHelper.GetDetection(Models.DetectionType.速度);
    //        //获取速度串口
    //        var speedInfo = Helpers.DeviceHelper.GetDetectionInfo(speedDetect);

    //        //创建速度模块实例
    //        speed = new SpeedDetection(speedInfo.Port,speedInfo.Detection.PortConfig);
    //        speed.Init();

    //        ////获取速度模块使用的光电设备
    //        //var gd = speedDetect.AssistList.Single(x => x.Assist.DeviceType == Models.AssistDeviceType.光电设备 && x.RouteNumber == 1);
    //        ////获取光电状态实例
    //        //var pm = GlobalPhotoelectric.GetPhotoelectric(gd);
    //        ////监听光电触发状态
    //        //pm.PropertyChanged += Pm_PropertyChanged;

    //    }

    //    bool isTrig;

    //    private void Pm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        if (e.PropertyName == "IsTrigger")
    //        {
    //            IsPLM = (sender as PhotoelectricModel).IsTrigger;
    //            //如果触发则执行
    //            if(IsPLM)
    //            {
    //                if (isSpeed)
    //                    return;
    //                isSpeed = true;

    //                try
    //                {
    //                    Task task = new TaskFactory().StartNew(() =>
    //                    {
    //                        try
    //                        {
    //                            //开启串口
    //                            speed.OpenPort();
    //                            DGC = "Ready!";
    //                            //DGC = "The Pump is Down";
    //                            //气泵降
    //                            speed.PumpDown();
    //                            PumpStatus = 1;
    //                            //开始检测
    //                            speed.DetectStart();
    //                            //提示加油门
    //                            System.Threading.Thread.Sleep(1000);
    //                            DGC = "3";
    //                            System.Threading.Thread.Sleep(1000);
    //                            DGC = "2";
    //                            System.Threading.Thread.Sleep(1000);
    //                            DGC = "1";
    //                            System.Threading.Thread.Sleep(1000);
    //                            DGC = "Go Go Go!";

    //                            //此处应起线程监控变化
    //                            //此出具体操作，测试员向谁发请求？
    //                            System.Threading.Thread.Sleep(2000);
    //                            ResultData = speed.GetSpeedData();
    //                            DGC = "Complete!";
    //                            System.Threading.Thread.Sleep(3000);
    //                            DGC = "The Pump Is Up";
    //                            //气泵升
    //                            speed.PumpUp();
    //                            PumpStatus = 0;
    //                            //复位
    //                            speed.Reset();
    //                            //关闭串口
    //                            speed.ClosePort();
    //                            System.Threading.Thread.Sleep(3000);
    //                            DGC = "End";
    //                        }
    //                        catch (Exception ex)
    //                        {

    //                            throw;
    //                        }

    //                    });


    //                }
    //                catch (AggregateException ex)
    //                {
    //                    foreach (Exception inner in ex.InnerExceptions)
    //                    {
    //                        Console.WriteLine(inner.Message);
    //                    }
    //                }


    //            }

    //        }
    //    }

    //    public void GetLatticeScreen()
    //    {
    //        System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();
    //        sp.BaudRate = 9600;
    //        sp.DataBits = 8;
    //        sp.Parity = System.IO.Ports.Parity.None;
    //        sp.PortName = "COM1";
    //        sp.StopBits = System.IO.Ports.StopBits.One;

    //        LatticeScreen ls = new LatticeScreen(sp);


    //        ls.SetLatticeScreenMessage("sdasdasd");



    //    }

    //}
}