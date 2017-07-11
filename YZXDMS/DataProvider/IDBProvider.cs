using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Model;
using YZXDMS.Models;

namespace YZXDMS.DataProvider
{
    /// <summary>
    /// 数据提供接口
    /// </summary>
    public interface IDBProvider
    {


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        LoginResult Login(string account, string pwd);


        /// <summary>
        /// 获取待检未分配车辆信息表
        /// </summary>
        /// <returns></returns>
        IList<WaitDetection> GetWaitDetectionList();

        /// <summary>
        /// 获取指定状态待检车辆信息表
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<WaitDetection> GetWaitDetectionList(int status,int line);

        /// <summary>
        /// 获取指定id的待检车辆信息,
        /// 无符合条件则回传null
        /// </summary>
        /// <returns></returns>
        WaitDetection GetWaitDetection(int id);
        /// <summary>
        /// 分配、更改指定待检车辆
        /// </summary>
        /// <param name="wd"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        bool SetWaitDetection(WaitDetection wd, int line);
        /// <summary>
        /// 根据车籍CarInfo添加待检车辆
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool AddWaitDetection(CarInfo item);

        /// <summary>
        /// 获取车籍列表
        /// </summary>
        /// <returns></returns>
        IList<CarInfo> GetCarInfoList();

        /// <summary>
        /// 添加车籍
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool AddCarInfoItem(CarInfo item);




    }
}
