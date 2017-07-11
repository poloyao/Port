using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YZXDMS.DataProvider;
using YZXDMS.Detections;
using YZXDMS.Model;

namespace YZXDMS.Core
{
    public class Core
    {
        #region 属性



        #endregion


        private static readonly Core instance = new Core();


        /// <summary>
        /// 获取Core单例
        /// </summary>
        /// <returns></returns>
        public static Core GetInstance()
        {
            return instance;
        }

        private Core() { }


        public static IDBProvider GetDBProvider()
        {
            return new SQLServerDBProvider();
        }

        public static Users User { get; set; }




        public static void GetDeviceConfig()
        {

        }


        public static void SetDeviceConfig()
        {

        }


    }
}
