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
                    port.BaudRate = item.BaudRate;
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

    }
    /// <summary>
    /// 串口实例和配置
    /// </summary>
    public class PortWithConfig
    {
        public SerialPort Port { get; set; }

        public PortConfig Config { get; set; }
    }

    /// <summary>
    /// 全局光电控制器Photovoltaic Control
    /// </summary>
    public class GlobalPVC
    {
        private static List<PVCModel> items { get; set; }

        private static readonly GlobalPVC instance = new GlobalPVC();

        public static GlobalPVC GetInstance()
        {
            return instance;
        }
        private GlobalPVC() { Init(); }

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
                    //PVCModel pvc = new PVCModel()
                    //{
                    //    PortConfig = item,
                    //}
                    if (!item.Port.IsOpen)
                    {
                        item.Port.DataReceived += (s, e) =>
                         {
                             ReadPortInfo(s);
                         };
                        item.Port.Open();
                    }
                }


            }
        }

        /// <summary>
        /// 光电串口接收事件
        /// </summary>
        /// <param name="s"></param>
        private void ReadPortInfo(object s)
        {
            throw new NotImplementedException();
        }
    }

    public class PVCModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PortConfig PortConfig { get; set; }
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
  

    }

}
