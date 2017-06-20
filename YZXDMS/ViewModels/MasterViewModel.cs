using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.DataProvider;
using YZXDMS.Detections;
using System.Linq;
using System.Diagnostics;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class MasterViewModel
    {

        public virtual int IsPLM { get; set; }

        public virtual string DGC { get; set; }

        public virtual int PumpStatus { get; set; }


        public virtual int ResultData { get; set; }

        bool isPLMTemp;
        bool isSpeed;

        SpeedDetection speed;

        Stopwatch sw = new Stopwatch();

        public MasterViewModel()
        {

        }

        public void Reset()
        {
            isSpeed = false;
        }

        public void InitDevice()
        {
            var speedInfo = Helper.DeviceHelper.GetDetectionInfo(Models.DetectionType.Speed);
            var plm = speedInfo.AssistList.First(x => x.AssistDevice.AssistType == Models.AssistDeviceType.Photoelectric).AssistDevice;
            PhotoelectricModel pm = GlobalPhotoelectric.GetPhotoelectricModel(plm);
            pm.PropertyChanged += Pm_PropertyChanged;
            speed = new SpeedDetection();

        }

        private void Pm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsTrigger")
            {
                IsPLM = (sender as PhotoelectricModel).IsTrigger == true ? 3 : 0;

                if (IsPLM == 3)
                {
                    if (!isPLMTemp)
                    {
                        if (!sw.IsRunning)
                            sw.Start();
                        isPLMTemp = true;
                        GlobalLatticeScreen.SetMessageInfo(new Models.AssistDeviceModel(), "光电到位");
                        DGC = "Triggered is GD!";
                        //暂时关闭光电
                        //(sender as PhotoelectricModel).device.config.Port.Close();
                    }
                    else
                    {
                        try
                        {
                            if (sw.ElapsedMilliseconds < 1000)
                                return;
                            if (isSpeed)
                                return;
                            isSpeed = true;
                            System.Threading.Thread.Sleep(2000);
                            //IsPLM = 0;
                            DGC = "The Pump is Down";
                            speed.PumpDown();
                            PumpStatus = 1;
                            System.Threading.Thread.Sleep(3000);
                            speed.DetectStart();
                            DGC = "Ready!";
                            System.Threading.Thread.Sleep(2000);
                            DGC = "3";
                            System.Threading.Thread.Sleep(1000);
                            DGC = "2";
                            System.Threading.Thread.Sleep(1000);
                            DGC = "1";
                            System.Threading.Thread.Sleep(1000);
                            DGC = "Go Go Go!";
                            System.Threading.Thread.Sleep(3000);
                            ResultData = speed.GetSpeedData();
                            DGC = "Complete!";
                            System.Threading.Thread.Sleep(3000);
                            DGC = "The Pump Is Up";
                            speed.PumpUp();
                            PumpStatus = 0;
                            speed.Reset();
                            System.Threading.Thread.Sleep(3000);
                            DGC = "";
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
                

            }
        }
    }
}