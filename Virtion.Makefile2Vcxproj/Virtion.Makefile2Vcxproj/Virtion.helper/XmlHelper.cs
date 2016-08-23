using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Virtion.helper
{
    class XmlHelper
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion

    }

    public class ObjectToXml
    {
        static XmlDocument doc = new XmlDocument();
        public static void Load(object obj, string path)
        {
            doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", ""));
            string className = obj.GetType().Name;
            XmlElement node = doc.CreateElement(className, "http://schemas.microsoft.com/developer/msbuild/2003");
            node.SetAttribute("DefaultTargets", "Build");
            node.SetAttribute("ToolsVersion", "12.0");

            doc.AppendChild(node);
            Process(obj, node);

            Console.WriteLine(doc.OuterXml);
            doc.Save(path);
            string s= File.ReadAllText(path);
            s = s.Replace("xmlns=\"\"", "");
            File.WriteAllText(path,s);
        }
        private static void Process(object obj, XmlElement node)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var fieldInfo in fields)
            {
                var value = fieldInfo.GetValue(obj);
                if (value == null)
                {
                    continue;
                }
                var type = value.GetType();
                string typeName = type.Name;
                if (typeName == "List`1")
                {
                    MethodInfo indexMethod = type.GetMethod("get_Item");
                    MethodInfo countMethod = type.GetMethod("get_Count");
                    int count = (int)countMethod.Invoke(value, null);
                    for (int i = 0; i < count; i++)
                    {
                        object newObject = indexMethod.Invoke(value, new object[] { i });
                        var childNode = doc.CreateElement(newObject.GetType().Name,null);
                        node.AppendChild(childNode);
                        Process(newObject, childNode);
                    }
                    continue;
                }

                var list = fieldInfo.GetCustomAttributesData();
                if (list.Count > 0)
                {
                    var attributesData = list[0];
                    if (attributesData.AttributeType == typeof(XmlAttributeAttribute))
                    {
                        node.SetAttribute(fieldInfo.Name, value.ToString());
                        continue;
                    }

                    if (attributesData.AttributeType == typeof(XmlElementAttribute))
                    {
                        var childNode = doc.CreateElement(typeName, null);
                        node.AppendChild(childNode);
                        Process(value, childNode);
                        continue;
                    }
                }

                if (typeName.ToLower() == "string" || typeName.ToLower() == "int32"
                    || typeName.ToLower() == "double" || typeName.ToLower() == "bool")
                {
                    var child = doc.CreateElement(fieldInfo.Name,null);
                    child.InnerText = value.ToString();
                    node.AppendChild(child);
                }
            }

            if (node.IsEmpty == true && node.HasAttributes == false)
            {
                node.RemoveAll();
            }
        }

    }

}
