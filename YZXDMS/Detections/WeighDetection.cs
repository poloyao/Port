using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace YZXDMS.Detections
{
    public class WeighDetection
    {
        private SerialPort port;


        public WeighDetection(SerialPort port)
        {
            this.port = port;

        }

        public void Init()
        {
            if (port.IsOpen)
            {
                //有疑问，假设其他设备正在用的情况下，进行了强制关闭，并打开，会不会造成混乱？
                port.Close();
            }

            port.DataReceived += Port_DataReceived;


            OpenPort();
            Reset();

        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }


        public void OpenPort()
        {
            if (port.IsOpen)
                port.Close();
        }

        public void Reset()
        {

        }

        public void GetData()
        {


        }
    }
}
