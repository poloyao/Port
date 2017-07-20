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
        public  PVCModel GetItem(int portId,int route)
        {
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
                        item.Port.Open();
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
            //throw new NotImplementedException();
            //System.Threading.Thread.Sleep(5000);
            foreach (var item in pvcs)
            {
                item.IsTrigger = !item.IsTrigger;
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
