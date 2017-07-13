using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace YZXDMS
{
    public class CRC16Helper
    {

        #region MyRegion


        private static readonly byte[] auchCRCHi = new byte[]
            {
                0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00,
0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80,
0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80,
0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00,
0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00,
0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40
            };

        private static readonly byte[] auchCRCLo = new byte[]
            {
                0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2,
0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8,
0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D,
0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13,
0xD3, 0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF,
0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8,
0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC,
0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26, 0x22, 0xE2, 0xE3, 0x23,
0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66,
0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9,
0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C,
0xB4, 0x74, 0x75, 0xB5, 0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1,
0x71, 0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56,
0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E,
0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C, 0x44,
0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81,
0x80, 0x40
            };



        public static ushort CRC16(Byte[] buffer, int sset, int eset)
        {
            byte crcHi = 0xff;  // 高位初始化
            byte crcLo = 0xff;  // 低位初始化
            for (int i = sset; i <= eset; i++)
            {
                int crcIndex = crcHi ^ buffer[i]; //查找crc表值
                crcHi = (byte)(crcLo ^ auchCRCHi[crcIndex]);
                crcLo = auchCRCLo[crcIndex];
            }
            return (ushort)(crcHi << 8 | crcLo);
        }

        private static readonly CRC16Helper instance = new CRC16Helper();

        public static CRC16Helper GetInstance()
        {
            return instance;
        }

        private CRC16Helper()
        {
            SPOpen();
        }

        #endregion

        SerialPort sp = new SerialPort();
        private static readonly object _object = new object();

        byte[] bytesData = new byte[0];
        float pressDat;

        private void SPOpen()
        {
            if (sp.IsOpen)
                throw new Exception("串口已打开,请先关闭");
            sp.PortName = "COM3";
            sp.BaudRate = 19200;
            sp.StopBits = StopBits.One;
            sp.DataBits = 8;
            sp.Parity = Parity.None;
            sp.ReceivedBytesThreshold = 1;

            sp.DataReceived += Sp_DataReceived;

            sp.Open();
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = sender as System.IO.Ports.SerialPort;
            #region MyRegion
             //int n = sp.BytesToRead;//先记录下来，避免某种原因，操作几次之间时间长，缓存不一致  
            //byte[] buf = new byte[n+5];//声明一个临时数组存储当前来的串口数据  
            //sp.Read(buf, 0, n+5);//读取缓冲数据  
            //if (n+5 > 10)
            //{
            //    byte[] conbuf = new byte[4];
            //    conbuf[0] = buf[4];
            //    conbuf[1] = buf[3];
            //    conbuf[2] = buf[6];
            //    conbuf[3] = buf[5];
            //    float fPressDat = BitConverter.ToSingle(conbuf, 0);
            //    conbuf[0] = buf[8];
            //    conbuf[1] = buf[7];
            //    conbuf[2] = buf[10];
            //    conbuf[3] = buf[9];
            //    float fElecDat = BitConverter.ToSingle(conbuf, 0);


            //    MessageBox.Show("当前压力：" + Convert.ToString(fPressDat));

            //}
            #endregion    

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
            //后加的代码，否则容易下标越界IndexOutOfRangeException
            if (bytesData.Length < 3)
                return;

            if (bytesData.Length > 10)
            {
                //02 04 08
                for (int i = 0; i < bytesData.Length; i++)
                {
                    if (i <= bytesData.Length - 10 - i)
                    {
                        if (bytesData[i] == 0x02 && bytesData[i+ 1] == 0x04 && bytesData[i+ 2] == 0x08)
                        {
                            byte[] conbuf = new byte[4];
                            conbuf[0] = bytesData[i + 4];
                            conbuf[1] = bytesData[i + 3];
                            conbuf[2] = bytesData[i + 6];
                            conbuf[3] = bytesData[i + 5];
                            float fPressDat = BitConverter.ToSingle(conbuf, 0);
                            conbuf[0] = bytesData[i + 8];
                            conbuf[1] = bytesData[i + 7];
                            conbuf[2] = bytesData[i + 10];
                            conbuf[3] = bytesData[i + 9];
                            float fElecDat = BitConverter.ToSingle(conbuf, 0);

                            pressDat = fPressDat;
                        }
                    }
                    else
                    {
                        break;
                    }
                }



            }
        }


        /// <summary>
        /// 压力清零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void YLQL()
        {

            byte[] by = new byte[6];
            by[0] = 0x02;
            by[1] = 0x67;
            by[2] = 0x45;
            by[3] = 0x01;

            by[4] = 0x00;
            by[5] = 0x01;

            //by[5] = 0x00;
            //by[4] = 0x01;

            var crc = CRC16Helper.CRC16(by, 0, by.Length - 1);
            var bit = BitConverter.GetBytes(crc);

            byte[] by2 = new byte[by.Length + 2];
            for (int i = 0; i < by.Length; i++)
            {
                by2[i] = by[i];
            }
            by2[by.Length] = bit[1];
            by2[by.Length + 1] = bit[0];


            if (sp.IsOpen)
                sp.Write(by2, 0, by2.Length);


        }

        /// <summary>
        /// 造压，传入的造压值为原始的Mpa 0.4等
        /// </summary>
        /// <param name="zyz"></param>
        public void ZY(float zyz)
        {
            byte[] by = new byte[11];
            by[0] = 0x02;
            by[1] = 0x10;
            by[2] = 0x21;
            by[3] = 0x00;

            by[4] = 0x00;
            by[5] = 0x02;

            by[6] = 0x04;
            float _zyz = zyz * 1000;
            var sinby = BitConverter.GetBytes(_zyz);
            by[7] = sinby[1];
            by[8] = sinby[0];
            by[9] = sinby[3];
            by[10] = sinby[2];


            var crc = CRC16Helper.CRC16(by, 0, by.Length - 1);
            var bit = BitConverter.GetBytes(crc);

            byte[] by2 = new byte[by.Length + 2];
            for (int i = 0; i < by.Length; i++)
            {
                by2[i] = by[i];
            }
            by2[by.Length] = bit[1];
            by2[by.Length + 1] = bit[0];


            if (sp.IsOpen)
                sp.Write(by2, 0, by2.Length);
        }

        /// <summary>
        /// 仅读取压力值
        /// </summary>
        /// <returns></returns>
        public float ReadYLZ()
        {
            byte[] by = new byte[6];
            by[0] = 0x02;
            by[1] = 0x04;
            by[2] = 0x20;
            by[3] = 0x00;

            by[4] = 0x00;
            by[5] = 0x04;

            var crc = CRC16Helper.CRC16(by, 0, by.Length - 1);
            var bit = BitConverter.GetBytes(crc);
            byte[] by2 = new byte[by.Length + 2];
            for (int i = 0; i < by.Length; i++)
            {
                by2[i] = by[i];
            }
            by2[by.Length] = bit[1];
            by2[by.Length + 1] = bit[0];

            if (sp.IsOpen)
                sp.Write(by2, 0, by2.Length);
            //等待2秒造压
            System.Threading.Thread.Sleep(2000);
            return pressDat;
        }

        /// <summary>
        /// 读取压力值,根据传入值判断,tt为最长等待时间默认5秒,默认模糊区间100分之十
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="tt"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool ReadYLZ(float temp, int tt = 5000)
        {
            byte[] by = new byte[6];
            by[0] = 0x02;
            by[1] = 0x04;
            by[2] = 0x20;
            by[3] = 0x00;

            by[4] = 0x00;
            by[5] = 0x04;

            var crc = CRC16Helper.CRC16(by, 0, by.Length - 1);
            var bit = BitConverter.GetBytes(crc);
            byte[] by2 = new byte[by.Length + 2];
            for (int i = 0; i < by.Length; i++)
            {
                by2[i] = by[i];
            }
            by2[by.Length] = bit[1];
            by2[by.Length + 1] = bit[0];

            if (sp.IsOpen)
                sp.Write(by2, 0, by2.Length);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //var intervalTemp = temp / 1000;

            //临时
            float lsNum = 200f;

            while (true)
            {
                var pressDatTemp = temp * 1000;
                var intervalTemp = pressDatTemp / 100;
                //var readpressDat = pressDat;
                Console.WriteLine(pressDat + "-----" + lsNum);
                if (pressDat < pressDatTemp + intervalTemp && pressDat > pressDatTemp - intervalTemp)
                {
                    Console.WriteLine(true);
                    return true;
                }

                ////无压力源模拟方法
                //lsNum += 100;
                //if (lsNum < pressDatTemp + intervalTemp && lsNum > pressDatTemp - intervalTemp )
                //{
                //    Console.WriteLine(true);
                //    return true;
                //}
                //if (pressDatTemp == 0)
                //{
                //    Console.WriteLine(true);
                //    return true;
                //}


                //if (pressDat < 29000 && pressDat > 25000)
                //{
                //    return true;
                //}
                //if (sw.ElapsedMilliseconds > tt)
                //{
                //    //return false;
                //}
                System.Threading.Thread.Sleep(1000);
                sp.Write(by2, 0, by2.Length);
            }
        }
    }
}
