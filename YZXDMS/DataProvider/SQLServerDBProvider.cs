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

        public bool AddWaitDetection(CarInfo item)
        {
            bool _result = false;
            YZXEntities db = new YZXEntities();
            try
            {
                WaitDetection wd = new WaitDetection();
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

        public IList<CarInfo> GetCarInfoList()
        {
            using (YZXEntities db = new YZXEntities())
            {
                var query = db.CarInfo.ToList();
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
            using (YZXEntities db = new YZXEntities())
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
