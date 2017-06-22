using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.DataProvider;
using YZXDMS.Detections;

namespace YZXDMS.Core
{
    public class Core
    {
        //public void Speed()
        //{
        //    PhotoelectricModel pm = GlobalPhotoelectric.GetPhotoelectricModel(new Models.AssistDeviceModel());
        //    pm.PropertyChanged += Pm_PropertyChanged;
        //    pm.IsTrigger = true;
        //}

        //private void Pm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "IsTrigger")
        //    {
        //        SpeedDetection speed = new SpeedDetection();
        //        GlobalLatticeScreen.SetMessageInfo(new Models.AssistDeviceModel(), "光电到位");
        //        try
        //        {
        //            speed.PumpDown();
        //            speed.DetectStart();
        //            var result = speed.GetSpeedData();
        //            speed.PumpUp();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
                
        //    }
        //}
    }
}
