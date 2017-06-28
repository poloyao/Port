using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace YZXDMS.Detections
{
    public  class LatticeScreen
    {

        SerialPort port;
        private LatticeScreen()
        { }

        public LatticeScreen(SerialPort port)
        {
            this.port = port;
        }


        public void Init()
        {
            //如果设备在初始化的时候就被打开了，说明有争抢现象发生，
            if (port.IsOpen)
                throw new Exception();

            port.DataReceived += Port_DataReceived;

            OpenPort();
            Reset();
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            //00
            //0D


            //2.32
            //3.----倒数4 内容
            //倒数3校验1
            //2 高
            //


            var serialPort = sender as System.IO.Ports.SerialPort;
            byte[] bytesData = new byte[0];
            byte[] bytesTemp = new byte[0];
            int bytesRead;
            byte result = 0x00;


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

            for (int i = 0; i < bytesData.Length; i++)
            {
                if ((bytesData[i] == 0x00) && (bytesData[i + 2] == 0x0D))
                {
                    result = bytesData[i + 1];
                   if (result > 0x00)
                    {

                    }

                    i += 2;
                }
            }


        }

        public void OpenPort()
        {
            if (!port.IsOpen)
                port.Open(); 
        }

        public void ClosePort()
        {
            if (port.IsOpen)
                port.Close();
        }


        public void Reset()
        {
        }

        /// <summary>
        /// 点阵屏写入
        /// </summary>
        /// <param name="message"></param>
        public void SetLatticeScreenMessage(string message)
        {
            
            int angle = 0;
            
            var messChar = message.ToCharArray();
            byte[] data = new byte[messChar.Length + 4];
            
            data[0] = 0x00;
            data[1] = 0x32;
            for (int i = 0; i < messChar.Length; i++)
            {
                angle += messChar[i];
                data[i + 2] = (byte)messChar[i];
            }

            angle = angle ^ 0x5A5A;
            var angle_1 = (byte)(angle << 24) >> 24;
            var angle_2 = (byte)angle >> 8;

            data[messChar.Length + 2] = (byte)angle_1;
            data[messChar.Length + 3] = (byte)angle_2;

            data[messChar.Length + 4] = 0x0D;
            
            port.Write(data, 0, data.Length);
        }

    }
}
