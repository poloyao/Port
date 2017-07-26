using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Helpers
{
    class SerialPortHelper
    {

        SerialPort Comm;

        static byte[] ringBuffer = new byte[40];  //建立环状缓冲区
        static int ringBufferWrite = 0;      //缓冲区写指针
        static int ringBufferRead = 0;      //缓冲区读指针
        static int ringBufferLenght = 0;    //缓冲区数据长度
        static int result = 0;
        static int ringBufferReaded = 0;

        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //-------------------------------------------------//
            //  若串口接收缓冲区内有数据，就把数据转存入环状缓冲区内。
            //-------------------------------------------------//
            while (Comm.BytesToRead > 0)
            {
                if (ringBufferLenght < 40)
                {
                    Comm.Read(ringBuffer, ringBufferWrite, 1);
                    
                    if (ringBufferLenght == 13)
                    {
                        // ringBuffer[ringBufferWrite]
                    }

                    if (ringBufferWrite < 39)
                        ringBufferWrite += 1;
                    else
                        ringBufferWrite = 0;
                    ringBufferLenght += 1;
                }
            }
            

            //-------------------------------------------------//
            //  帧数据格式如下，
            //1 0xAA    起始字节
            //2 0xXX    参数地址
            //3+4  0xXX+0xXX    参数数据共2byte
            //5 0x55    结束字节
            //  若环状缓冲区内数据多于5个，即表示可能有至少1帧数据，从第
            //一个数据开始检查，如果首数据为帧起始字节即0xAA，则检查本帧
            //中的其它数据是否正常，若正常，则可以确认接收到了一帧完整的数
            //据，此时就可以从帧地址位取出变量地址，从帧数据位取出变量数
            //据，然后通过BeiginInvoke的方式更新UI并把环状缓冲区内的本
            //帧数据删除。若首数据并非起始字节，则删除之。
            //-------------------------------------------------//
            while (ringBufferLenght >= 5)
            {
                //对比标志位
                if ((ringBuffer[ringBufferRead] == 0xAA) &&
                    (ringBuffer[(ringBufferRead + 4) % 40] == 0x55) &&
                    (ringBuffer[(ringBufferRead + 1) % 40] > 0) &&
                    (ringBuffer[(ringBufferRead + 1) % 40] <= 4))
                {
                    //------------------------------------------//
                    //  检测到了正确的数据帧,
                    //  首先把数据送入静态变量中，方便在其它地方使用，然后
                    //删除环状缓冲区中的数据，最后更新UI。
                    //------------------------------------------//
                    result = (ringBuffer[ringBufferRead + 2] << 8) +
                                        ringBuffer[ringBufferRead + 3];
                    ringBufferReaded = ringBuffer[ringBufferRead + 1];
                    if (ringBufferRead + 5 > 39)
                        ringBufferRead -= 35;
                    else
                        ringBufferRead += 5;
                    ringBufferLenght -= 5;
                  
                    //结果-------------------

                    //-----------------------

                }
                else
                {
                    //------------------------------------//
                    //  首数据并非起始字节或其它原因，
                    //头递增且数据长度递减
                    //------------------------------------//
                    if (ringBufferRead < 39)
                        ringBufferRead += 1;
                    else
                        ringBufferRead = 0;
                    ringBufferLenght -= 1;
                }
            }
        }
    }
}
