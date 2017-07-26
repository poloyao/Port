using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;

namespace YZXDMS.DataProvider
{
    public class SQLiteDBContext : DbContext
    {
        public virtual DbSet<PortConfig> Ports { get; set; }

        public virtual DbSet<DetectorModel> Detectors { get; set; }

        public virtual DbSet<StationModel> Stations { get; set; }

        public virtual DbSet<AssistModel> Assist { get; set; }

        public SQLiteDBContext() : base("ConfigDb")
        {
            Configuar();
        }

        public SQLiteDBContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configuar();
        }

        private void Configuar()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortConfig>();
            modelBuilder.Entity<DetectorModel>();
            modelBuilder.Entity<StationModel>();
            modelBuilder.Entity<AssistModel>();

            var initializer = new SqliteDBInitializer(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }

    public class SqliteDBInitializer : SqliteCreateDatabaseIfNotExists<SQLiteDBContext>
    {
        public SqliteDBInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
        {

        }

        protected override void Seed(SQLiteDBContext context)
        {
            base.Seed(context);

            context.Set<StationModel>().Add(new StationModel() {  StationName = "一工位", Value = 1 });
            context.Set<StationModel>().Add(new StationModel() {  StationName = "二工位", Value = 2 });
            context.Set<StationModel>().Add(new StationModel() {  StationName = "三工位", Value = 3 });
            context.Set<StationModel>().Add(new StationModel() {  StationName = "四工位", Value = 4 });
            context.Set<StationModel>().Add(new StationModel() {  StationName = "外检项", Value = 5 });

            foreach (DeviceType item in Enum.GetValues(typeof(DeviceType)))
            {
                var port = new PortConfig() {  Name = $"{item.ToString()}（设备）", PortName = PortIndex.COM1, RouteTotal = 1, DeviceType = item };
                context.Set<PortConfig>().Add(port);
                context.SaveChanges();
                if ((int)item < 100)
                {
                    context.Set<DetectorModel>().Add(new DetectorModel() {  DetectorName = $"{item.ToString()}", DetectorType = (DetectionType)(item), PortId = port.Id, StationValue = 0 });
                }
            }




        }

    }

    ///// <summary>
    ///// 串口端口
    ///// </summary>
    //public enum PortIndex
    //{
    //    COM1,
    //    COM2,
    //    COM3,
    //    COM4,
    //    COM5,
    //    COM6,
    //    COM7,
    //    COM8,
    //    COM9,
    //    COM10,
    //    COM11,
    //    COM12,
    //    COM13,
    //    COM14,
    //    COM15,
    //    COM16,
    //    COM17,
    //    COM18,
    //    COM19,
    //    COM20,
    //    COM21,
    //    COM22,
    //    COM23,
    //    COM24,
    //    COM25,
    //    COM26,
    //    COM27,
    //    COM28,
    //    COM29,
    //    COM30,
    //    COM31,
    //    COM32,

    //}

    ///// <summary>
    ///// 设备类型,
    ///// 100之前为主设备，100之后未辅助设备
    ///// </summary>
    //[Flags]
    //public enum DeviceType
    //{
    //    外检 = 0,
    //    侧滑,
    //    速度,
    //    灯光,
    //    制动,
    //    称重,
    //    底盘,
    //    底盘间隙,
    //    声级计,
    //    功率,
    //    油耗,
    //    尾气,
    //    探平衡仪,

    //    灯屏设备 = 101,
    //    光电设备 = 102,
    //    录像设备 = 103,
    //    拍照设备 = 104,
    //}

    //[Table("PortConfig")]
    //public class PortConfig : ICloneable
    //{
    //    public long Id { get; set; }
    //    /// <summary>
    //    /// 串口名称
    //    /// </summary>
    //    [Display(Name = "串口名称")]
    //    public virtual string Name { get; set; }
    //    [Display(Name = "端口")]
    //    public PortIndex PortName { get; set; }
    //    [Display(Name = "波特率")]
    //    public int BaudRate { get; set; }
    //    [Display(Name = "数据位")]
    //    public int DataBits { get; set; } = 8;
    //    [Display(Name = "奇偶校验")]
    //    public Parity Parity { get; set; }
    //    [Display(Name = "停止位")]
    //    public StopBits StopBits { get; set; } = StopBits.One;

    //    /// <summary>
    //    /// 通道数量
    //    /// </summary>
    //    [Display(Name = "通道数量")]

    //    public int RouteTotal { get; set; } = 1;
    //    /// <summary>
    //    /// 协议厂家
    //    /// </summary>
    //    [Display(Name = "协议厂家")]
    //    public String Protocol { get; set; }

    //    /// <summary>
    //    /// 设备类型
    //    /// </summary>
    //    [Display(Name = "设备类型")]
    //    public DeviceType DeviceType { get; set; }

    //    public object Clone()
    //    {
    //        var result = new PortConfig();
    //        result.Name = this.Name;
    //        result.PortName = this.PortName;
    //        result.BaudRate = this.BaudRate;
    //        result.DataBits = this.DataBits;
    //        result.Parity = this.Parity;
    //        result.StopBits = this.StopBits;
    //        result.Protocol = this.Protocol;
    //        result.DeviceType = this.DeviceType;
    //        result.RouteTotal = this.RouteTotal;

    //        return result;
    //    }

    //    public static PortConfig Create()
    //    {
    //        var result = new PortConfig();
    //        result.Name = "默认";
    //        result.PortName = PortIndex.COM1;
    //        result.BaudRate = 9600;
    //        result.DataBits = 8;
    //        result.Parity = Parity.None;
    //        result.StopBits = StopBits.One;
    //        result.Protocol = "无";
    //        result.DeviceType = DeviceType.光电设备;

    //        result.RouteTotal = 1;

    //        return result;
    //    }
    //}

    //[Table("Detector")]
    //public class DetectorModel
    //{
    //    public long Id { get; set; }
    //    public string DetectorName { get; set; }
    //    public PortConfig port { get; set; }

    //    public List<AssistModel> AssistList { get; set; }

    //}

    //public class AssistModel
    //{
    //    public long Id { get; set; }
    //    public PortConfig port { get; set; }

    //    public int Asist { get; set; }
    //}

    //[Table("Station")]
    //public class StationModel
    //{
    //    public long Id { get; set; }
    //    public string StationName { get; set; }

    //    public List<DetectorModel> Detectors { get; set; }
    //}

}
