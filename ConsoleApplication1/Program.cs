using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //File.WriteAllText("XMLFile1.xml")

            //XElement doc = XElement.Load("XMLFile1.xml");
            //IEnumerable<XElement> childList =  from el in doc.Elements()
            //                                   select el;
            //foreach (XElement e in childList)
            //    Console.WriteLine(e);

            //XDocument doc = XDocument.Load("XMLFile1.xml");
            //IEnumerable<XElement> childList =
            //    from el in doc.Elements()
            //    select el;
            //foreach (XElement e in childList)
            //    Console.WriteLine(e);

            




           

            Person ps = new Person() { name = "李四", age = 20 };

            #region 实体类的序列化和反序列化
            XmlHelper.serializeToXml(ps);
            Person p = XmlHelper.DeserializerXml<Person>("Info.xml");
            XDocument doc = XDocument.Load("Info.xml");
            Console.WriteLine("实体类反序列化结果：");
            Console.WriteLine("姓名：" + p.name + "年龄：" + p.age);
            #endregion
            Console.WriteLine("---------分割线-------");
            #region 集合类的序列化和反序列化
            Persons pos = new Persons() { data = new List<Person> { ps } };
            //pos.data = new List<Person>() { ps };
            XmlHelper.serializeToXml(pos);
            Persons po = XmlHelper.DeserializerXml<Persons>("Info.xml");
            Console.WriteLine("集合类反序列化结果：");
            po.data.ForEach(item => Console.WriteLine("姓名：" + item.name + "年龄：" + item.age));
            #endregion



            Console.Read();

        }



    }


    /// 实体类序列化
    /// </summary>
    [Serializable]
    public class Person
    {
        //[XmlIgnore] //此字段不序列化
        public string name { get; set; }
        public int age { get; set; }
    }
    /// <summary>
    /// 集合类序列化
    /// </summary>
    public class Persons
    {
        public List<Person> data { get; set; }
    }
    /// <summary>
    /// 序列化与反序列化帮助类--XMLHelper
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">要序列化的实例</param>
        public static void serializeToXml<T>(T obj)
        {
            XDocument doc = new XDocument(
                new XElement("Root",
                new XElement("Child", "content")
              )
            );
            doc.Save("Root.xml");
            XmlSerializer serialize = new XmlSerializer(typeof(T));
            using (XmlTextWriter xtw = new XmlTextWriter("Info.xml", Encoding.Default))
            {
                //xtw.Indentation = 2;
                xtw.Formatting = Formatting.Indented;
                serialize.Serialize(xtw, obj);
            }
        }
        /// <summary>
        /// Deserialize
        /// </summary>
        /// <typeparam name="T">泛型-反序列化后的类型</typeparam>
        /// <param name="data">反序列化的xml文档</param>
        /// <returns></returns>
        public static T DeserializerXml<T>(string data)
        {
            XmlSerializer Deserializer = new XmlSerializer(typeof(T));
            using (XmlTextReader xtr = new XmlTextReader(data))
                return (T)Deserializer.Deserialize(xtr);
        }
    }


}
