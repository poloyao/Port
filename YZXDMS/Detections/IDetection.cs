using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Model;

namespace YZXDMS.Detections
{
    /// <summary>
    /// 检测项目通用接口
    /// </summary>
    public interface IDetection
    {
        /// <summary>
        /// 设置串口
        /// </summary>
        /// <param name="port"></param>
        void SetPort(SerialPort port);
        /// <summary>
        /// 设置配置信息
        /// </summary>
        void SetCongfig();

        /// <summary>
        /// 设置待检测车籍信息
        /// </summary>
        /// <param name="carInfo"></param>
        void SetCarInfo(CarInfo carInfo);

        /// <summary>
        /// 获取车籍信息
        /// </summary>
        /// <returns></returns>
        CarInfo GetCarInfo();

        /// <summary>
        /// 设备初始化
        /// </summary>
        void DeviceInit();
        
        /// <summary>
        /// 终止检测
        /// </summary>
        void StopDetect();

        /// <summary>
        /// 设备复位
        /// </summary>
        void Reset();

        ///// <summary>
        ///// 获取检测结果
        ///// </summary>
        ///// <returns></returns>
        //object GetResultData();

        /// <summary>
        /// 设置点阵屏关联
        /// </summary>
        /// <param name="LSO"></param>
        void SetLatticeScreen(ILatticeScreenOperate LSO);

        /// <summary>
        /// 当前状态，根据枚举DetectionStatus回值
        /// </summary>
        /// <returns></returns>
        DetectionStatus GetCurrentStatus();
    }

    /// <summary>
    /// 检测状态
    /// </summary>
    public enum DetectionStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        IDLE,
        /// <summary>
        /// 工作中
        /// </summary>
        WORK,
        /// <summary>
        /// 异常
        /// </summary>
        ABN

    }

    ///// <summary>
    ///// 制动力轴数
    ///// </summary>
    //public enum BFDAxis
    //{

    //}

    /// <summary>
    /// 制动力检测接口
    /// </summary>
    public interface IBFDDetection : IDetection
    {
        /// <summary>
        /// 启动检测
        /// </summary>
        /// <param name="asis"></param>
        void StartDetect(int asis);
    }

    /// <summary>
    /// 速度检测接口
    /// </summary>
    public interface ISpeedDetection : IDetection
    {
        ///// <summary>
        ///// 获取速度检测结果
        ///// </summary>
        ///// <returns></returns>
        //Speed GetSpeedResultData();

        /// <summary>
        /// 启动检测，返回检测结果
        /// </summary>
        IList<Speed> StartDetect();
    }

    




    //public abstract class SpeedDetectionBase : ISpeedDetection
    //{
        
        
    //    protected DetectionStatus IsStatus;

    //    public abstract void DeviceInit();
    //    public virtual DetectionStatus GetCurrentStatus()
    //    {
    //        return IsStatus;
    //    }
    //    public abstract Speed GetSpeedResultData();
    //    public abstract void Reset();
    //    public abstract void SetCarInfo(CarInfo carInfo);
    //    public abstract void SetCongfig();
    //    public abstract void SetLatticeScreen(ILatticeScreenOperate LSO);
    //    public abstract void SetPort(SerialPort port);
    //    public abstract void StartDetect();
    //    public abstract void StopDetect();
    //}


    //public class asdaajeee : SpeedDetectionBase
    //{

    //    public asdaajeee():base()
    //    {
    //        IsStatus = DetectionStatus.IDLE;
    //    }
        

    //    public override void DeviceInit()
    //    {
    //        throw new NotImplementedException();
    //    }
        
    //    public override Speed GetSpeedResultData()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Reset()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetCarInfo(CarInfo carInfo)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetCongfig()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetLatticeScreen(ILatticeScreenOperate LSO)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void SetPort(SerialPort port)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void StartDetect()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void StopDetect()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}


