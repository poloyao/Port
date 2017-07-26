using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Helpers
{
    public class DataHelper
    {
        /// <summary>
        /// 引用类型实体间包含值的比较。
        /// 相同为true，
        /// 仅能比较包含项没有其他引用类型的情况
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        public static bool EntityComparison<T>(T item1, T item2) where T : class
        {
            System.Reflection.PropertyInfo[] properties = item1.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
           
            foreach (var properInfo in properties)
            {
                var t1 = properInfo.GetValue(item1);
                var t2 = properInfo.GetValue(item2);

                if (!t1.Equals(t2))
                    return false;
            }
            return true;

        }


        void ass()
        {
            bbb b1 = new bbb();
            bbb b2 = new bbb();
            if (b1 == b2)
            {

            }
            var ssss = EntityComparison(b1, b2);
        }

        public class bbb
        {
            public string Name { get; set; }
        }

    }
}
