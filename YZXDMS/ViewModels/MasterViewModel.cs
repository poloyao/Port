using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.DataProvider;
using YZXDMS.Detections;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class MasterViewModel
    {
        /// <summary>
        /// 速度光电状态
        /// </summary>
        public virtual bool IsPLM { get; set; }

        /// <summary>
        /// 速度点阵
        /// </summary>
        public virtual string DGC { get; set; }

        public virtual int PumpStatus { get; set; }

        public virtual int ResultData { get; set; }

        bool isPLMTemp;

        /// <summary>
        /// 为true时阻碍下次触发流程直到为false
        /// </summary>
        bool isSpeed;

        SpeedDetection speed;

        Stopwatch sw = new Stopwatch();

        
        


        public MasterViewModel()
        {
            
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            isSpeed = false;
        }


        public void InitSpeedDevice()
        {
            DGC = "Wait...";
            //获取速度模块
            var speedDetect = Helper.DeviceHelper.GetDetection(Models.DetectionType.速度);
            //获取速度串口
            var speedInfo = Helper.DeviceHelper.GetDetectionInfo(speedDetect);

            //创建速度模块实例
            speed = new SpeedDetection(speedInfo.Port);
            speed.Init();

            //获取速度模块使用的光电设备
            //speedInfo.Detection.Assist
            var gd = speedDetect.AssistList.Single(x => x.Assist.DeviceType == Models.AssistDeviceType.Photoelectric && x.RouteNumber == 1);
            //获取光电状态实例
            var pm = GlobalPhotoelectric.GetPhotoelectric(gd);
            //监听光电触发状态
            pm.PropertyChanged += Pm_PropertyChanged;

        }

        bool isTrig;

        private void Pm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsTrigger")
            {
                IsPLM = (sender as PhotoelectricModel).IsTrigger;
                //如果触发则执行
                if(IsPLM)
                {
                    if (isSpeed)
                        return;
                    isSpeed = true;

                    try
                    {
                        Task task = new TaskFactory().StartNew(() =>
                        {
                            try
                            {
                                //开启串口
                                speed.OpenPort();
                                DGC = "Ready!";
                                //DGC = "The Pump is Down";
                                //气泵降
                                speed.PumpDown();
                                PumpStatus = 1;
                                //开始检测
                                speed.DetectStart();
                                //提示加油门
                                System.Threading.Thread.Sleep(1000);
                                DGC = "3";
                                System.Threading.Thread.Sleep(1000);
                                DGC = "2";
                                System.Threading.Thread.Sleep(1000);
                                DGC = "1";
                                System.Threading.Thread.Sleep(1000);
                                DGC = "Go Go Go!";

                                //此处应起线程监控变化
                                //此出具体操作，测试员向谁发请求？
                                System.Threading.Thread.Sleep(2000);
                                ResultData = speed.GetSpeedData();
                                DGC = "Complete!";
                                System.Threading.Thread.Sleep(3000);
                                DGC = "The Pump Is Up";
                                //气泵升
                                speed.PumpUp();
                                PumpStatus = 0;
                                //复位
                                speed.Reset();
                                //关闭串口
                                speed.ClosePort();
                                System.Threading.Thread.Sleep(3000);
                                DGC = "End";
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                          
                        });


                    }
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            Console.WriteLine(inner.Message);
                        }
                    }
                  
                    
                }

            }
        }



    }
}