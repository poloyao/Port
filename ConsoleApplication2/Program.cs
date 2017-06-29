using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //SetLatticeScreenMessage("ad");
        }

        


        public static void SetLatticeScreenMessage(string message)
        {

            var temp1 = 0x11;
            var temp2 = 0x4C;

            var temp1_old = (temp1 << 24) >> 24;
            var temp2_old = temp2 << 8;


            //byte[] byteOld = new byte[] {0x00,0x32, 0xC9, 0xEE, 0xDB, 0xDA, 0xCA, 0xD0, 0xB0, 0xB2, 0xB3, 0xB5, 0xBC, 0xEC, 0xB2, 0xE2, 0xD3, 0xD0, 0xCF, 0xDE, 0xB9, 0xAB, 0xCB, 0xBE, 0xBB, 0xB6, 0xD3, 0xAD, 0xC4, 0xFA, 0x21};


           // char[] strBuf = new char[256];
            uint angle = 0;

            var messChar = Encoding.Default.GetBytes(message.ToCharArray());
            
            //var messChar = new byte[] { 0x76,0xCA};
            byte[] data = new byte[messChar.Length + 5];

            data[0] = 0x00;
            data[1] = 0x32;
            for (int i = 0; i < messChar.Length; i++)
            {
                //angle += (char)data[i];
                data[i + 2] = (byte)messChar[i];
            }

            for (int i = 0; i < data.Length; i++)
            {
                angle += (byte)data[i];
            }

            angle = angle ^ 0x5A5A;
            byte angle_1 = (byte)((angle << 24) >> 24);
            byte angle_2 = (byte)(angle >> 8);

            data[data.Length - 3] = (byte)angle_1;
            data[data.Length - 2] = (byte)angle_2;

            data[data.Length - 1] = 0x0D;


            byte[] resutStr = new byte[data.Length - 5];
            for (int i = 0; i < resutStr.Length; i++)
            {
                resutStr[i] = data[i + 2];
            }

            string result = Encoding.Default.GetString(resutStr);
            Console.WriteLine(result);


            //port.Write(data, 0, data.Length);
        }


        public static void Weigh()
        {
            byte[] data = new byte[] { 0x3D, 0x2E, 0x30, 0x30, 0x31, 0x30, 0x30, 0x30, 0x30 };

            string resutl = "";
            for (int i = data.Length; i > 0; i--)
            {
                resutl += (char)data[i - 1];
            }

            Console.WriteLine(resutl);

            Console.Read();


        }

        public static void led(string message)
        {
            int m_nlenght = 2;
            byte[] m_strbuff = new byte[256];
            int angle = 0;

            var mess = message.ToCharArray();

            m_strbuff[0] = 0x00;
            m_strbuff[1] = 0x32;

            do
            {
                m_strbuff[m_nlenght] = (Byte)mess[m_nlenght - 2];
                m_nlenght++;

            } while (mess[m_nlenght - 2] != 0x00 && m_nlenght < 255);

            for (int i = 0; i < m_nlenght; i++)
            {
                angle += m_strbuff[i];
            }

            angle = angle ^ 0x5A5A;
            var angleTemp = angle;
            byte angle_1 = (byte)((angleTemp << 24) >> 24);
            var angleTemp2 = angle;
            byte angle_2 = (byte)(angleTemp2 >> 8);

            m_strbuff[m_nlenght] = angle_1;
            m_strbuff[m_nlenght + 1] = angle_2;
            m_strbuff[m_nlenght + 2] = 0x0d;

            


        }

    }
}
