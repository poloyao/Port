using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;
using YZXDMS.Helper;

namespace YZXDMS.Detections
{
    /// <summary>
    /// 速度检测
    /// </summary>
    public class SpeedDetection
    {
        SpeedValueMode _valueMode;
        DetectionModel _dectect;
        /// <summary>
        /// 气泵升降状态，初始值true升，false降
        /// </summary>
        bool _pumpStatus = true;
        /// <summary>
        /// 检测状态
        /// </summary>
        bool _detectStatus;

        /// <summary>
        /// 结果数据
        /// </summary>
        int _resultData;
        /// <summary>
        /// 结果状态
        /// </summary>
        bool _resutlStatus;


        public SpeedDetection()
        {
            Init();
           // _dectect.config.Port.DataReceived += Port_DataReceived;
        }

        void Init()
        {
            //获取此检测模块所有涉及的内容
            _dectect = DeviceHelper.GetDetectionInfo(DetectionType.Speed);
            _dectect.config.Port.DataReceived += Port_DataReceived;
            //OpenPort();
            //Reset();
        }

        /// <summary>
        /// 设置取值模式,或根据配置文件设置
        /// </summary>
        /// <param name="mode"></param>
        public void SetSpeedValueMode(SpeedValueMode mode)
        {
            this._valueMode = mode;
        }

        /// <summary>
        /// 获取速度结果
        /// </summary>
        /// <returns></returns>
        public int GetSpeedData()
        {
            System.Threading.Thread.Sleep(1000);

            if (_resutlStatus)
                return _resultData;
            else
                return -1;
        }

        private void OpenPort()
        {
            if (_dectect.config.Port.IsOpen)
                return;//throw new Exception();

            _dectect.config.Port.DataReceived += Port_DataReceived;
            if (!_dectect.config.Port.IsOpen)
                _dectect.config.Port.Open();
        }

        private void Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //协议
            var protocol = _dectect.config.Protocol;
            //根据取值模式选择
            switch (_valueMode)
            {
                case SpeedValueMode.RealTime:
                    break;
                case SpeedValueMode.Final:
                    break;
            }

            //解析计算结果。。。。

            try
            {
                //两种情况

                //1。正常
                _resultData = new Random(Guid.NewGuid().GetHashCode()).Next(1, 100);
                _resutlStatus = true;

                //2.有异常
                //throw new Exception();

            }
            catch (Exception)
            {
                _resutlStatus = false;
                _resultData = -1;

                //上抛异常或在此进行处理
                throw;
            }
        }

        /// <summary>
        /// 开始检测
        /// </summary>
        public void DetectStart()
        {            
            if (_detectStatus)
                return;
            _dectect.config.Port.Write("伪命令");
            _detectStatus = true;
        }

        /// <summary>
        /// 气泵升
        /// </summary>
        public void PumpUp()
        {
            _dectect.config.Port.Write("伪命令");
            _pumpStatus = true;     
                 
        }

        /// <summary>
        /// 气泵降
        /// </summary>
        public void PumpDown()
        {
            _dectect.config.Port.Write("伪命令");
            _pumpStatus = false;
        }

        /// <summary>
        /// 终止
        /// </summary>
        public void SpeedStop()
        {
            Reset();
        }

        /// <summary>
        /// 设备复位
        /// </summary>
        public void Reset()
        {
            _resultData = 0;
            _resutlStatus = false;
            _detectStatus = false;
            PumpUp();
        }
    }
}
