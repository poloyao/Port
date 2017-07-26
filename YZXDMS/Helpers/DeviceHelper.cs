using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;
using System.IO.Ports;
using Newtonsoft.Json;
using YZXDMS.DataProvider;

namespace YZXDMS.Helpers
{
    /// <summary>
    /// 设备帮助类
    /// </summary>
    public class DeviceHelper
    {
        private static readonly DeviceHelper instance = new DeviceHelper();

        private DeviceHelper() { }

        public DeviceHelper GetInstance()
        {
            return instance;
        }

        
        
        /// <summary>
        /// 根据检测类型获取此单元的配置信息
        /// </summary>
        /// <param name="dt"></param>
        public void GetDetectorUnit(DetectionType dt)
        {
            
            var pwcs = GlobalPort.GetInstance().PortWithConfigItems;
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                //var sss = pwcs.Where(x => x.Config.DeviceType == DeviceType.外检).ToList();
                List<PortWithConfig> queryItems = new List<PortWithConfig>();
                switch (dt)
                {
                    case DetectionType.外检:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.外检).ToList();
                        break;
                    case DetectionType.侧滑:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.侧滑).ToList();
                        break;
                    case DetectionType.速度:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.速度).ToList();
                        break;
                    case DetectionType.灯光:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.灯光).ToList();
                        break;
                    case DetectionType.制动:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.制动).ToList();
                        break;
                    case DetectionType.称重:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.称重).ToList();
                        break;
                    case DetectionType.底盘:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.底盘).ToList();
                        break;
                    case DetectionType.底盘间隙:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.底盘间隙).ToList();
                        break;
                    case DetectionType.声级计:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.声级计).ToList();
                        break;
                    case DetectionType.功率:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.功率).ToList();
                        break;
                    case DetectionType.油耗:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.油耗).ToList();
                        break;
                    case DetectionType.尾气:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.尾气).ToList();
                        break;
                    case DetectionType.探平衡仪:
                        queryItems = pwcs.Where(x => x.Config.DeviceType == DeviceType.探平衡仪).ToList();
                        break;
                    default:
                        break;
                }



            }
        }
        
        



    }


    ///// <summary>
    ///// 设备帮助类
    ///// </summary>
    //public class DeviceHelper
    //{
    //    /// <summary>
    //    /// 检测项目信息列表
    //    /// </summary>
    //    public static List<Detection> DetectionItems { get; private set; } = new List<Detection>();


    //    /// <summary>
    //    /// 检测项目串口信息
    //    /// </summary>
    //    private static List<DetectionPort> detectPortItems = new List<DetectionPort>();

    //    /// <summary>
    //    /// 配置文件的串口列表
    //    /// </summary>
    //    private static List<PortConfig> portConfigItems;

    //    //private static List<VMDetection> 

    //    /// <summary>
    //    /// 辅助设备串口信息，多路同串口公用一个
    //    /// </summary>
    //    public static List<AssistPort> AssistPortItems { get; private set; } = new List<AssistPort>();



    //    private static readonly DeviceHelper instance = new DeviceHelper();

    //    public static DeviceHelper GetInstance()
    //    {
    //        return instance;
    //    }

    //    private DeviceHelper()
    //    {
    //        InitPort();

    //    }

    //    void InitPort()
    //    {
    //        //DetectionItems = XmlHelper.DeserializerXml<List<Detection>>("Detection.xml");

    //        CreatPortList();

    //        #region 模拟生成conf信息

    //        //DetectionItems.Add(new Detection()
    //        //{
    //        //    Name = "速度",
    //        //    DetectionType = DetectionType.速度,
    //        //    PortConfig = new PortConfig()
    //        //    {
    //        //        Name = "速度",
    //        //        Protocol = "速度协议",
    //        //        PortName = PortIndex.COM10,
    //        //        BaudRate = 9600,
    //        //        Parity = Parity.None,
    //        //        DataBits = 8,
    //        //        StopBits = StopBits.One,
    //        //        DeviceType = DeviceType.速度,

    //        //    },
    //        //    AssistList = new List<AssistRoute>()
    //        //    {
    //        //        new AssistRoute()
    //        //        {
    //        //            RouteNumber = 1,
    //        //            Assist = new AssistDevice()
    //        //            {
    //        //                DeviceType = AssistDeviceType.Photoelectric,
    //        //                PortConfig = new PortConfig()
    //        //                {
    //        //                        Name = "光电",
    //        //                    Protocol = "光电协议",
    //        //                    PortName = PortIndex.COM1,
    //        //                    BaudRate  = 9600,
    //        //                    Parity = Parity.None,
    //        //                    DataBits = 8,
    //        //                    StopBits = StopBits.One,
    //        //                    DeviceType = DeviceType.光电设备,

    //        //                    RouteTotal = 8
    //        //                },
    //        //                //RouteTotal = 8
    //        //            }
    //        //        }
    //        //    }
    //        //});

    //        //XmlHelper.serializeToXml(DetectionItems, "Detection.xml");


    //        #endregion

    //        #region 生成port列表

    //        //if (DetectionItems == null)
    //        //{

    //        //    return;
    //        //    //throw new Exception("没有配置工位信息");
    //        //}

    //        //foreach (var item in DetectionItems)
    //        //{
    //        //    var conf = item.PortConfig;
    //        //    SerialPort sp = new SerialPort();
    //        //    sp.PortName = conf.PortName.ToString();
    //        //    sp.BaudRate = conf.BaudRate;
    //        //    sp.Parity = conf.Parity;
    //        //    sp.DataBits = conf.DataBits;
    //        //    sp.StopBits = conf.StopBits;

    //        //    detectPortItems.Add(new DetectionPort()
    //        //    {
    //        //        Detection = item,
    //        //        Port = sp
    //        //    });

    //        //    foreach (var assist in item.AssistList)
    //        //    {
    //        //        var aConf = assist.Assist.PortConfig;

    //        //        //保证辅助设备多路公用一个port
    //        //        var quaryAssist = AssistPortItems.SingleOrDefault(x => x.Assist == assist.Assist);
    //        //        if (quaryAssist != null)
    //        //        {
    //        //            continue;
    //        //         }

    //        //        SerialPort assistsp = new SerialPort();
    //        //        assistsp.PortName = aConf.PortName.ToString();
    //        //        assistsp.BaudRate = aConf.BaudRate;
    //        //        assistsp.Parity = aConf.Parity;
    //        //        assistsp.DataBits = aConf.DataBits;
    //        //        assistsp.StopBits = aConf.StopBits;

    //        //        AssistPortItems.Add(new AssistPort()
    //        //        {
    //        //            Port = assistsp,
    //        //            Assist = assist.Assist
    //        //        });
    //        //    }
    //        //}
    //        #endregion

    //        #region Delete



    //        //var port = new System.IO.Ports.SerialPort("COM12", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);



    //        //Detections.Add(new Detection()
    //        //{
    //        //    Name = "速度",
    //        //    DType = DetectionType.Speed,
    //        //    config = new PortConfigModel()
    //        //    {
    //        //        DeviceProperty = DeviceProperty.检测设备,
    //        //        StartMode = StartMode.即用即关,
    //        //        Protocol = "XX协议",
    //        //        Port = new System.IO.Ports.SerialPort("COM10", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One)
    //        //    },
    //        //    AssistList = new List<AssistDeviceOrder>()
    //        //    {
    //        //        new AssistDeviceOrder()
    //        //        {
    //        //            AssistDevice = new AssistDeviceModel()
    //        //            {
    //        //                AssistType = AssistDeviceType.Photoelectric,
    //        //                Name = "速度光电",
    //        //                config = new PortConfigModel()
    //        //                {
    //        //                    DeviceProperty = DeviceProperty.辅助设备,
    //        //                    StartMode = StartMode.保持开启,
    //        //                    Protocol = "XX协议",
    //        //                    Port = port
    //        //                }
    //        //            }
    //        //        }
    //        //    }
    //        //});

    //        #endregion

    //    }

    //    /// <summary>
    //    /// 生成串口实例列表
    //    /// </summary>
    //    void CreatPortList()
    //    {
    //        if (DetectionItems == null)
    //        {
    //            return;
    //            //throw new Exception("没有配置工位信息");
    //        }

    //        foreach (var item in DetectionItems)
    //        {
    //            var conf = item.PortConfig;
    //            SerialPort sp = new SerialPort();
    //            sp.PortName = conf.PortName.ToString();
    //            sp.BaudRate = conf.BaudRate;
    //            sp.Parity = conf.Parity;
    //            sp.DataBits = conf.DataBits;
    //            sp.StopBits = conf.StopBits;

    //            detectPortItems.Add(new DetectionPort()
    //            {
    //                Detection = item,
    //                Port = sp
    //            });

    //            foreach (var assist in item.AssistList)
    //            {
    //                var aConf = assist.PortConfig;

    //                //保证辅助设备多路公用一个port
    //                //var quaryAssist = AssistPortItems.SingleOrDefault(x => x.Assist == assist);
    //                //if (quaryAssist != null)
    //                //{
    //                //    continue;
    //                //}

    //                SerialPort assistsp = new SerialPort();
    //                assistsp.PortName = aConf.PortName.ToString();
    //                assistsp.BaudRate = aConf.BaudRate;
    //                assistsp.Parity = aConf.Parity;
    //                assistsp.DataBits = aConf.DataBits;
    //                assistsp.StopBits = aConf.StopBits;


    //                var ad = new AssistDevice();
    //                ad.PortConfig = assist.PortConfig;
    //                switch (assist.PortConfig.DeviceType.ToString())
    //                {
    //                    case "灯屏设备":
    //                        ad.DeviceType = AssistDeviceType.灯屏设备;
    //                        break;
    //                    case "光电设备":
    //                        ad.DeviceType = AssistDeviceType.光电设备;
    //                        break;
    //                    case "录像设备":
    //                        ad.DeviceType = AssistDeviceType.录像设备;
    //                        break;
    //                    case "拍照设备":
    //                        ad.DeviceType = AssistDeviceType.拍照设备;
    //                        break;
    //                    default:
    //                        break;
    //                }


    //                AssistPortItems.Add(new AssistPort()
    //                {
    //                    Port = assistsp,
    //                    Assist = ad
    //                });
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 获取配置文件的串口列表
    //    /// </summary>
    //    /// <returns></returns>
    //    public static List<PortConfig> GetPortConfigItems()
    //    {
    //        //if (portConfigItems != null)
    //        //    return portConfigItems;

    //        //如果没有Port.xml文件，则搜索Detection.xml文件，根据此文件结构创建port.xml文件
    //        List<PortConfig> Results = new List<PortConfig>();
    //        Results = Helpers.XmlHelper.DeserializerXml<List<PortConfig>>("Port.xml");
    //        portConfigItems = Results;
    //        return Results;


    //        //if (Results == null)
    //        //{
    //        //    List<Detection> det = Helpers.XmlHelper.DeserializerXml<List<Detection>>("Detection.xml");
    //        //    if (det != null)
    //        //    {
    //        //        Results = new List<PortConfig>();

    //        //        foreach (var detItem in det)
    //        //        {
    //        //            if (detItem.PortConfig == null)
    //        //                continue;
    //        //            Results.Add(detItem.PortConfig);

    //        //            if (detItem.AssistList == null)
    //        //                continue;
    //        //            foreach (var assistItem in detItem.AssistList)
    //        //            {
    //        //                if (assistItem.Assist.PortConfig == null)
    //        //                    continue;

    //        //                //此处可以忽略，在循环完毕后，删除同类项。但需要创建比较器
    //        //                List<PortConfig> tempds = new List<PortConfig>();
    //        //                foreach (var ds in Results)
    //        //                {
    //        //                    var comp = Helpers.DataHelper.EntityComparison(ds, assistItem.Assist.PortConfig);
    //        //                    if (!comp)
    //        //                        tempds.Add(assistItem.Assist.PortConfig);
    //        //                }
    //        //                tempds.ForEach(x => { Results.Insert(Results.Count(), x); });
    //        //            }
    //        //        }

    //        //        //var kkkk = Items.Distinct(System.Collections.Generic.Comparer.Default);
    //        //    }
    //        //}

    //    }


    //    /// <summary>
    //    /// 获取指定检测项目串口信息
    //    /// </summary>
    //    /// <param name="detection"></param>
    //    /// <returns></returns>

    //    public static DetectionPort GetDetectionInfo(Detection detection)
    //    {
    //        var query = detectPortItems.Single(x => x.Detection == detection);
    //        return query;
    //    }

    //    /// <summary>
    //    /// 获取检测项目
    //    /// </summary>
    //    /// <param name="detectionType"></param>
    //    /// <returns></returns>
    //    public static Detection GetDetection(DetectionType detectionType)
    //    {
    //        var query = DetectionItems.Single(x => x.DetectionType == detectionType);
    //        return query;
    //    }


    //    /// <summary>
    //    /// 获取指定辅助串口信息
    //    /// </summary>
    //    /// <param name="assist"></param>
    //    /// <returns></returns>
    //    public static AssistPort GetAssistPortInfo(AssistDevice assist)
    //    {
    //        var query = AssistPortItems.Single(x => x.Assist == assist);
    //        return query;
    //    }


    //    #region 删除



    //    ///// <summary>
    //    ///// 获取检测模块
    //    ///// </summary>
    //    ///// <param name="detectionType">检测类型</param>
    //    ///// <returns></returns>
    //    //public static Detection GetDetectionInfo(DetectionType detectionType)
    //    //{
    //    //    var query = Detections.SingleOrDefault(x => x.DetectionType == detectionType);
    //    //    if (query == null)
    //    //        return new Detection();//throw new Exception();
    //    //    return query;
    //    //}

    //    ///// <summary>
    //    ///// 获取检测模块的串口信息
    //    ///// </summary>
    //    ///// <param name="detectionType"></param>
    //    ///// <returns></returns>
    //    //public static PortConfigModel GetDetectionPortInfo(DetectionType detectionType)
    //    //{
    //    //    var query = GetDetectionInfo(detectionType);
    //    //    return query.config;
    //    //}

    //    ///// <summary>
    //    ///// 获取指定辅助串口
    //    ///// </summary>
    //    ///// <param name="detectionType"></param>
    //    ///// <param name="assistType"></param>
    //    ///// <param name="index"></param>
    //    ///// <returns></returns>
    //    //public static PortConfigModel GetAssistPortInfo(DetectionType detectionType, AssistDeviceType assistType, int index)
    //    //{

    //    //    var query = Detections.SingleOrDefault(x => x.DType == detectionType);
    //    //    var query2 = query.AssistList.SingleOrDefault(x => x.AssistDevice.AssistType == assistType && x.Index == index);
    //    //    return query2.AssistDevice.config;
    //    //}



    //    ///// <summary>
    //    ///// 获取工位信息
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //public StationModel GetStationInfo()
    //    //{
    //    //    return null;
    //    //}

    //    #endregion

    //}

    ///// <summary>
    ///// 检测项目串口信息
    ///// </summary>
    //public class DetectionPort
    //{
    //    /// <summary>
    //    /// 串口实例
    //    /// </summary>
    //    public SerialPort Port { get; set; }
    //    /// <summary>
    //    /// 配置信息
    //    /// </summary>
    //    public Detection Detection { get; set; }

    //}
    ///// <summary>
    ///// 辅助设备串口信息
    ///// </summary>
    //public class AssistPort
    //{
    //    /// <summary>
    //    /// 串口信息
    //    /// </summary>
    //    public SerialPort Port { get; set; }
    //    /// <summary>
    //    /// 关联设备配置
    //    /// </summary>
    //    public AssistDevice Assist { get; set; }
    //}





}
