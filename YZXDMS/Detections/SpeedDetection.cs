using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.Models;
using YZXDMS.Helper;
using System.IO.Ports;

namespace YZXDMS.Detections
{
    /// <summary>
    /// 速度检测
    /// </summary>
    public class SpeedDetection
    {
        /// <summary>
        /// 取值模式
        /// </summary>
        SpeedValueMode _valueMode;

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
        /// <summary>
        /// 当前模块使用的串口
        /// </summary>
        private SerialPort port;
        

        /// <summary>
        /// 带参构造。
        /// 后期追加配置信息
        /// </summary>
        /// <param name="port"></param>
        public SpeedDetection(SerialPort port) 
        {
            this.port = port;
            //Init();
        }

        /// <summary>
        /// 设备初始化
        /// </summary>
        public void Init()
        {
            if (port.IsOpen)
                return;

            port.DataReceived += Port_DataReceived;
            //因为需要先进行设备复位，所以在此处打开串口
            OpenPort();
            Reset();

        }

        private void Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            ////协议
            //var protocol = _dectect.config.Protocol;
            ////根据取值模式选择
            //switch (_valueMode)
            //{
            //    case SpeedValueMode.RealTime:
            //        break;
            //    case SpeedValueMode.Final:
            //        break;
            //}

            ////解析计算结果。。。。

            //try
            //{
            //    //两种情况

            //    //1。正常
            //    _resultData = new Random(Guid.NewGuid().GetHashCode()).Next(1, 100);
            //    _resutlStatus = true;

            //    //2.有异常
            //    //throw new Exception();

            //}
            //catch (Exception)
            //{
            //    _resutlStatus = false;
            //    _resultData = -1;

            //    //上抛异常或在此进行处理
            //    throw;
            //}

            var serialPort = sender as System.IO.Ports.SerialPort;
            byte[] bytesData = new byte[0];
            byte[] bytesTemp = new byte[0];
            int bytesRead;
            byte result = 0x00;

            try
            {
                //获取接收缓冲区中字节数
                bytesRead = serialPort.BytesToRead;
                //保存上一次没处理完的数据
                if (bytesData.Length > 0)
                {
                    bytesTemp = new byte[bytesData.Length];
                    bytesData.CopyTo(bytesTemp, 0);
                    bytesData = new byte[bytesRead + bytesData.Length];
                    bytesTemp.CopyTo(bytesData, 0);
                }
                else
                {
                    bytesData = new byte[bytesRead];
                    bytesTemp = new byte[0];
                }
                //保存本次接收的数据
                for (int i = 0; i < bytesRead; i++)
                {
                    bytesData[bytesTemp.Length + i] = Convert.ToByte(serialPort.ReadByte());//read all data
                }
                //后加的代码，否则容易下标越界IndexOutOfRangeException
                if (bytesData.Length < 3)
                    return;
                for (int i = 0; i < bytesData.Length; i++)
                {                    
                    if ((bytesData[i] == 0xAA) && (bytesData[i + 2] == 0x0D))
                    {
                        result = bytesData[i + 1];
                        if (result != 0x00)
                        {
                            _resultData = new Random(Guid.NewGuid().GetHashCode()).Next(50, 100);
                            _resutlStatus = true;

                        }
                        //_resutlStatus = result
                        i += 2;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


        ///// <summary>
        ///// 设置取值模式,或根据配置文件设置
        ///// </summary>
        ///// <param name="mode"></param>
        //public void SetSpeedValueMode(SpeedValueMode mode)
        //{
        //    this._valueMode = mode;
        //}

        /// <summary>
        /// 获取速度结果
        /// </summary>
        /// <returns></returns>
        public int GetSpeedData()
        {
            //System.Threading.Thread.Sleep(1000);

            return _resultData;
            //if (_resutlStatus)
            //    return _resultData;
            //else
            //    return -1;
        }

        public void OpenPort()
        {
            if (port.IsOpen)
                return;//throw new Exception();
            else
                port.Open();
            
        }

        public void ClosePort()
        {
            if (port.IsOpen)
                port.Close();
            else
                return;
        }

       
        /// <summary>
        /// 开始检测
        /// </summary>
        public void DetectStart()
        {            
            if (_detectStatus)
                return;

            byte[] data = new byte[3];
            data[0] = 0xAA;
            data[1] = 0x01;
            data[2] = 0x0D;

            port.Write(data, 0, 3);
            _detectStatus = true;
        }

        /// <summary>
        /// 气泵升
        /// </summary>
        public void PumpUp()
        {
            byte[] data = new byte[3];
            data[0] = 0xAA;
            data[1] = 0x02;
            data[2] = 0x0D;

            port.Write(data, 0, 3);
            _pumpStatus = true;     
                 
        }

        /// <summary>
        /// 气泵降
        /// </summary>
        public void PumpDown()
        {
            byte[] data = new byte[3];
            data[0] = 0xAA;
            data[1] = 0x03;
            data[2] = 0x0D;

            port.Write(data, 0, 3);
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
