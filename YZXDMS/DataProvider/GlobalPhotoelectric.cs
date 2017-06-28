using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;
using YZXDMS.Helpers;

namespace YZXDMS.DataProvider
{
    public class GlobalPhotoelectric
    {

        private static List<PhotoelectricModel> items { get; set; } = new List<PhotoelectricModel>();

        private static Queue<AssistPort> PhtoelectricItems { get; set; } = new Queue<AssistPort>();
        

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

            var portItems = Helpers.DeviceHelper.AssistPortItems;
            var photoelecticeItems = portItems.Where(x => x.Assist.DeviceType == AssistDeviceType.光电设备).ToList();
            

            foreach (var phots in photoelecticeItems)
            {
                //获取总路数
                int total = phots.Assist.PortConfig.RouteTotal;

                for (int i = 0; i < total; i++)
                {
                    PhotoelectricModel ppm = new PhotoelectricModel();
                    ppm.device = phots.Assist;
                    ppm.Route = i + 1;
                    items.Add(ppm);
                }
                //开启串口并设置接收事件
                if (!phots.Port.IsOpen)
                {
                    phots.Port.DataReceived += (s, e) =>
                    {
                        //处理光电数据，并分配到相应位置
                        //
                        ReadPortInfo(s, phots);
                    };
                    phots.Port.Open();
                }

            }
        }
        
        private static void ReadPortInfo(object s, AssistPort phots)
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
                        //pm.IsTrigger = result > 0x00 ? true : false;

                        var triggerList = items.Where(x => x.device == phots.Assist).ToList();
                        if (result > 0x00)
                        {

                            var aaa = result & (byte)Enum.Parse(typeof(PortRoute), PortRoute.通道一.ToString());
                            //if(result & PortRoute.通道一 > 0x00)
                        //    switch (result)
                        //    {
                        //        case 0x01:
                        //            triggerList.Single(x => x.Route == 1).IsTrigger = true;
                        //            break;
                        //        case 0x02:
                        //            triggerList.Single(x => x.Route == 2).IsTrigger = true;
                        //            break;
                        //        case 0x03:
                        //            triggerList.Single(x => x.Route == 1).IsTrigger = true;
                        //            triggerList.Single(x => x.Route == 2).IsTrigger = true;
                        //            break;
                        //    }
                        }
                        else
                        {
                            triggerList.Single(x => x.Route == 1).IsTrigger = false;
                            triggerList.Single(x => x.Route == 2).IsTrigger = false;
                        }

                        i += 2;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        [Flags]
        public enum PortRoute
        {
            通道一 = 0x01,
            通道二 = 0x02,
            通道三 = 0x04,
            通道四 = 0x08,
            通道五 = 0x10,
            通道六 = 0x20,
            通道七 = 0x40,
            通道八 = 0x80,
        }


        /// <summary>
        /// 获取指定光电线路的实例
        /// </summary>
        /// <param name="device"></param>
        /// <param name="routeIndex"></param>
        /// <returns></returns>
        public static PhotoelectricModel GetPhotoelectric(AssistDevice device, int routeIndex)
        {
            return items.Single(x => x.device == device && x.Route == routeIndex);
        }

        /// <summary>
        /// 获取指定光电线路的实例
        /// </summary>
        /// <param name="assistRoute"></param>
        /// <returns></returns>
        public static PhotoelectricModel GetPhotoelectric(AssistRoute assistRoute)
        {
            return items.Single(x => x.device.PortConfig == assistRoute.PortConfig && x.Route == assistRoute.RouteNumber);
        }
        
    }


    public class PhotoelectricModel: System.ComponentModel.INotifyPropertyChanged
    {
        public AssistDevice device { get; set; }

        public int Route { get; set; }

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
