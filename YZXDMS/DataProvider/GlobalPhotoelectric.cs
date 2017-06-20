using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;

namespace YZXDMS.DataProvider
{
    public class GlobalPhotoelectric
    {

        private static Queue<PhotoelectricModel> items { get; set; } = new Queue<PhotoelectricModel>();

        private static readonly GlobalPhotoelectric instance = new GlobalPhotoelectric();

        public static GlobalPhotoelectric GetInstance()
        {
            return instance;
        }

        private GlobalPhotoelectric()
        {
            Init();
        }

        void Init()
        {
            var speedInfo = Helper.DeviceHelper.GetDetectionInfo(Models.DetectionType.Speed);
            var plm = speedInfo.AssistList.First(x => x.AssistDevice.AssistType == Models.AssistDeviceType.Photoelectric);
            items.Enqueue(new PhotoelectricModel() { device = plm.AssistDevice });
            PhotoelectricModel pm = GlobalPhotoelectric.GetPhotoelectricModel(plm.AssistDevice);
            pm.device.config.Port.DataReceived += (s, e) =>
            {
                var serialPort = s as System.IO.Ports.SerialPort;
                byte[] bytesData = new byte[0];
                byte[] bytesTemp = new byte[0];
                int bytesRead;
                byte result = 0x00;

                try
                {
                    //获取接收缓冲区中字节数
                    bytesRead = serialPort.BytesToRead;
                    //保存上一次没处理完的数据
                    if (bytesData.Length > 0)
                    {
                        bytesTemp = new byte[bytesData.Length];
                        bytesData.CopyTo(bytesTemp, 0);
                        bytesData = new byte[bytesRead + bytesData.Length];
                        bytesTemp.CopyTo(bytesData, 0);
                    }
                    else
                    {
                        bytesData = new byte[bytesRead];
                        bytesTemp = new byte[0];
                    }
                    //保存本次接收的数据
                    for (int i = 0; i < bytesRead; i++)
                    {
                        bytesData[bytesTemp.Length + i] = Convert.ToByte(serialPort.ReadByte());//read all data
                    }

                    for (int i = 0; i < bytesData.Length; i++)
                    {
                        if ((bytesData[i] == 0xAA) && (bytesData[i + 2] == 0x0D))
                        {
                            result = bytesData[i + 1];
                            pm.IsTrigger = result > 0x00 ? true : false;

                            i += 2;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            };

            pm.device.config.Port.Open();
        }

        /// <summary>
        /// 获取指定灯光的当前结果
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool GetIsTrigger(AssistDeviceModel device)
        {
            return items.Single(x => x.device == device).IsTrigger;
        }

        public static PhotoelectricModel GetPhotoelectricModel(AssistDeviceModel device)
        {
            return items.Single(x => x.device == device);
            //return new PhotoelectricModel();
        }
    }


    public class PhotoelectricModel: System.ComponentModel.INotifyPropertyChanged
    {
        public AssistDeviceModel device { get; set; }

        public bool IsTrigger
        {
            get
            {
                return isTrigger;
            }

            set
            {
                isTrigger = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsTrigger"));
            }
        }

        bool isTrigger;
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
