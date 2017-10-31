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

       public virtual DbSet<DBSystemTime> DBSystemTime { get; set; }

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
            context.Set<StationModel>().Add(new StationModel() {  StationName = "外检项", Value = 5 ,IsAutoTest = false});

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
    

}
