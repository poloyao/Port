using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;
using System.IO.Ports;

namespace YZXDMS.DataProvider
{

    /// <summary>
    /// 全局串口信息
    /// </summary>
    public class GlobalPort
    {
        public List<PortWithConfig> PortWithConfigItems { get; set; }

        private static readonly GlobalPort instance = new GlobalPort();

        public static GlobalPort GetInstance()
        {
            return instance;
        }
        private GlobalPort() { Init(); }

        void Init()
        {
            PortWithConfigItems = new List<PortWithConfig>();
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                var ps = db.Ports.ToList();
                foreach (var item in ps)
                {
                    var port = new SerialPort();
                    port.PortName = item.PortName.ToString();
                    port.BaudRate = 9600;//item.BaudRate;
                    port.DataBits = item.DataBits;
                    port.Parity = item.Parity;
                    port.StopBits = item.StopBits;
                    PortWithConfig pwc = new PortWithConfig()
                    {
                        Port = port,
                        Config = item
                    };
                    PortWithConfigItems.Add(pwc);
                }
            }
        }

        /// <summary>
        /// 获取指定串口信息，返回PortWithConfig
        /// </summary>
        /// <param name="portId"></param>
        /// <returns></returns>
        public PortWithConfig GetPort(long portId)
        {
            return PortWithConfigItems.Single(x => x.Config.Id == portId);
        }

        /// <summary>
        /// 获取检测模块所用到的所有辅助设备信息
        /// </summary>
        /// <param name="speedUnit"></param>
        /// <returns></returns>
        public static List<AssistModel> GetAssistitems(DetectorModel speedUnit)
        {
            List<AssistModel> assistItems;
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                assistItems = db.Assist.ToList().Where(x => x.DetectorId == speedUnit.Id).ToList();
            }

            return assistItems;
        }

    }

    /// <summary>
    /// 全局工位信息
    /// </summary>
    public class GlobalStation
    {
        private static readonly GlobalStation instance = new GlobalStation();
        public static GlobalStation GetInstance()
        {
            return instance;
        }

        private GlobalStation() { Init();  }

        public List<DetectorModel> Station1 = new List<DetectorModel>();
        public List<DetectorModel> Station2 = new List<DetectorModel>();
        public List<DetectorModel> Station3 = new List<DetectorModel>();
        public List<DetectorModel> Station4 = new List<DetectorModel>();
        public List<DetectorModel> Station5 = new List<DetectorModel>();



        void Init()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                //读取检测项目信息
                var query = db.Detectors.ToList();
                //将项目分配到各工位
                foreach (var item in query)
                {
                    switch (item.StationValue)
                    {
                        case 1:
                            Station1.Add(item);
                            break;
                        case 2:
                            Station2.Add(item);
                            break;
                        case 3:
                            Station3.Add(item);
                            break;
                        case 4:
                            Station4.Add(item);
                            break;
                        case 5:
                            Station5.Add(item);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取所在工位的此检测项目信息
        /// </summary>
        /// <param name="dt"></param>
        public DetectorModel GetUintInStation(DetectionType dt)
        {
            Init();
            foreach (var item in Station1)
            {
                if (item.DetectorType == dt)
                {
                    return item;
                }
            }
            foreach (var item in Station2)
            {
                if (item.DetectorType == dt)
                {
                    return item;
                }
            }
            foreach (var item in Station3)
            {
                if (item.DetectorType == dt)
                {
                    return item;
                }
            }
            foreach (var item in Station4)
            {
                if (item.DetectorType == dt)
                {
                    return item;
                }
            }

            return null;


        }

        //  var pvc = GlobalPVC.GetInstance().GetItem(5, 1);

        public void GetPVCConfig()
        {

        }


    }

    

    /// <summary>
    /// 全局光电控制器Photovoltaic Control
    /// </summary>
    public class GlobalPVC
    {
        /// <summary>
        /// 光电通道组
        /// </summary>
        private static List<PVCModel> items { get; set; }

        private static readonly GlobalPVC instance = new GlobalPVC();

        public static GlobalPVC GetInstance()
        {
            return instance;
        }
        private GlobalPVC() { Init(); }
        

        /// <summary>
        /// 获取指定光电单路通道实体（PVCModel）
        /// </summary>
        /// <param name="portId">配置Id</param>
        /// <param name="route">通道数</param>
        /// <returns></returns>
        public  PVCModel GetItem(long portId,int route)
        {
            Init();
            if (items != null)
            {
                return items.SingleOrDefault(x => x.PortConfig.Id == portId && x.Route == route);
            }
            return null;
        }

        

        /// <summary>
        /// 初始化所有的光电设备，并开始接收数据
        /// </summary>
        void Init()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                //获取所有光电设备
                var query = GlobalPort.GetInstance().PortWithConfigItems.Where(x => x.Config.DeviceType == DeviceType.光电设备).ToList();

                foreach (var item in query)
                {
                    if (!item.Port.IsOpen)
                    {
                        List<PVCModel> _pvsList = new List<PVCModel>();
                        for (int i = 0; i < item.Config.RouteTotal; i++)
                        {
                            PVCModel pm = new PVCModel();
                            pm.Route = i + 1;
                            pm.PortConfig = item.Config;                            
                            _pvsList.Add(pm);
                        }
                        items = new List<PVCModel>();
                        items.AddRange(_pvsList);

                        item.Port.DataReceived += (s, e) =>
                         {
                             ReadPortInfo(s, _pvsList);
                         };
                        //MyBug 此处应该开启串口
                        //item.Port.Open();
                    }
                }
            }
        }
        

        /// <summary>
        /// 光电串口接收事件，并更改通道状态
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pvcs">此光电的所有通道实例</param>
        private void ReadPortInfo(object s, List<PVCModel> pvcs)
        {
            //MyBug 伪实现，将多有通道的值改变
            //foreach (var item in pvcs)
            //{
            //    item.IsTrigger = !item.IsTrigger;
            //}

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
                //后加的代码，否则容易下标越界IndexOutOfRangeException
                if (bytesData.Length < 3)
                    return;
                for (int i = 0; i < bytesData.Length; i++)
                {
                    if ((bytesData[i] == 0xAA) && (bytesData[i + 2] == 0x0D))
                    {
                        result = bytesData[i + 1];
                        if (result != 0x00)
                        {
                            pvcs.ForEach(x => { x.IsTrigger = true; });
                        }
                        else
                        {
                            pvcs.ForEach(x => { x.IsTrigger = false; });
                        }
                        i += 2;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


    }

    /// <summary>
    /// 光电单路通道实体
    /// </summary>
    public class PVCModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PortConfig PortConfig { get; set; }
        /// <summary>
        /// 所在通道
        /// </summary>
        public int Route { get; set; }

        /// <summary>
        /// 是否触发
        /// </summary>
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
  

    }

}
