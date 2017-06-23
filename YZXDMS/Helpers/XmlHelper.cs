using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace YZXDMS.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">要序列化的实例</param>
        /// <param name="filePath">文件保存地址,带.xml后缀</param>
        public static void serializeToXml<T>(T obj,string filePath)
        {
            XmlSerializer serialize = new XmlSerializer(typeof(T));
            using (XmlTextWriter xtw = new XmlTextWriter(filePath, Encoding.Default))
            {
                xtw.Formatting = Formatting.Indented;
                serialize.Serialize(xtw, obj);
            }
        }
        /// <summary>
        /// 反序列化.
        /// 返回null，为文档错误。
        /// </summary>
        /// <typeparam name="T">泛型-反序列化后的类型</typeparam>
        /// <param name="filePath">反序列化的xml文档</param>
        /// <returns>返回null，为文档错误。</returns>
        public static T DeserializerXml<T>(string filePath)
        {
            try
            {
                XmlSerializer Deserializer = new XmlSerializer(typeof(T));
                using (XmlTextReader xtr = new XmlTextReader(filePath))
                    return (T)Deserializer.Deserialize(xtr);
            }
            catch (Exception)
            {
                //不抛异常，返回 default（T） null值
            }

            return default(T);
        }
    }
}
