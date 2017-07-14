using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Model;
using YZXDMS.Models;

namespace YZXDMS.DataProvider
{
    public class SQLServerDBProvider : IDBProvider
    {

        public bool AddCarInfoItem(CarInfo item)
        {
            //using (YZXEntities db = new YZXEntities())
            //{
            //    //检查item完整性
            //    db.CarInfo.Add(item);
            //    db.SaveChanges();
            //}

            YZXEntities db = new YZXEntities();
            bool _result = false;
            try
            {
                //检查item完整性
                db.CarInfo.Add(item);
                db.SaveChanges();
                _result = true;
            }
            catch (Exception)
            {
                _result = false;
            }
            finally
            {
                db.Dispose();                
            }
            return _result;
            
        }

        public void AddSpeed(IList<Speed> speeds)
        {
            YZXEntities db = new YZXEntities();
            var connection = db.Database.Connection;
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            var tran = connection.BeginTransaction();
            try
            {
                //内部使用的还是引用类型，后期应将实体继承iclone接口，
                //Speed[] copySp = new Speed[speeds.Count];
                //speeds.CopyTo(copySp, 0);


                db.Database.UseTransaction(tran);
                //speeds[0].jylsh
                var spCPD = speeds.Single(x => x.Mode == (int)DetectionMode.CPD);
                var spSPD = speeds.Single(x => x.Mode == (int)DetectionMode.SPD);
                switch (Core.Core.DetectionMode)
                {
                    case DetectionMode.ALL:
                        db.Speed.Add(spCPD);
                        db.Speed.Add(spSPD);
                        db.SaveChanges();
                        db.Speed.Single(x => x.Id == spCPD.Id).Status = 1;
                        db.Speed.Single(x => x.Id == spSPD.Id).Status = 1;
                        //db.Speed.Where(x=>x.jylsh)
                        break;
                    case DetectionMode.CPD:                        
                        db.Speed.Add(spCPD);
                        db.SaveChanges();
                        db.Speed.Single(x => x.Id == spCPD.Id).Status = 1;
                        break;
                    case DetectionMode.SPD:
                        db.Speed.Add(spSPD);
                        db.SaveChanges();
                        db.Speed.Single(x => x.Id == spSPD.Id).Status = 1;
                        break;
                }
                db.SaveChanges();
                tran.Commit();
            }
            catch (Exception er)
            {
                tran.Rollback();
            }
            finally
            {
                tran.Dispose();
                db.Dispose();
            }


        }

        public bool AddWaitDetection(CarInfo item)
        {
            bool _result = false;
            YZXEntities db = new YZXEntities();
            try
            {
                WaitDetection wd = new WaitDetection();
                wd.jylsh = Guid.NewGuid().ToString();
                wd.CarInfoId = item.Id;
                db.WaitDetection.Add(wd);
                db.SaveChanges();
                _result = true;
            }
            catch (Exception)
            {
                _result = false;
            }
            finally
            {
                db.Dispose();
            }

            return _result;

        }

        public CarInfo GetCarInfoItem(string carID)
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.CarInfo.Single(x => x.HPHM == carID);
                return query;
            }
        }

        public CarInfo GetCarInfoItem(int id)
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.CarInfo.Single(x => x.Id == id);
                return query;
            }
        }

        public IList<CarInfo> GetCarInfoList()
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.CarInfo.ToList();
                return query;
            }
        }

        public IList<Speed> GetSpeedList(string lsh)
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.Speed.ToList();
                return query;
            }
        }

        public WaitDetection GetWaitDetection(int id)
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.WaitDetection.SingleOrDefault(x => x.Id == id);
                return query;
            }
        }

        public IList<WaitDetection> GetWaitDetectionList()
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.WaitDetection.Where(x => x.Status == 0).ToList();
                return query;
            }
        }

        public IList<WaitDetection> GetWaitDetectionList(int status, int line)
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.WaitDetection.Where(x => x.Status == status && x.LineID == line).ToList();
                return query;
            }
        }

        public LoginResult Login(string account, string pwd)
        {
            
            YZXEntities db = new YZXEntities();
            
            LoginResult result;
            try
            {
                var query = db.Users.SingleOrDefault(x => x.Account == account && x.PWD == pwd);
                if (query != null)
                {
                    return new LoginResult() { IsSuccess = true, User = query };
                }
                else
                {
                    var queryErr = db.Users.SingleOrDefault(x => x.Account == account);
                    if (queryErr == null)
                        return new LoginResult() { IsSuccess = false, Message = "用户不存在" };
                    return new LoginResult() { IsSuccess = false, Message = "密码错误" };
                }
            }
            catch (Exception)
            {
                result = new LoginResult() { IsSuccess = false, Message = "网络中断！" };
            }
            finally
            {
                db.Dispose();
            }
            return result;

        }

        public bool SetWaitDetection(WaitDetection wd, int line)
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.WaitDetection.Single(x => x.Id == wd.Id);//.LineID = line;
                query.LineID = line;
                query.Status = 1;
                db.SaveChanges();
                return true;
            }
        }
    }
}
