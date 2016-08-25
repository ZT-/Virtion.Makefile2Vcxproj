using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Virtion.helper
{
    public class ObjectToXml
    {
        static XmlDocument doc = new XmlDocument();
        public static void Load(object obj, string path)
        {
            doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", ""));
            string className = "Project";
            XmlElement node = doc.CreateElement(className, "http://schemas.microsoft.com/developer/msbuild/2003");
            node.SetAttribute("DefaultTargets", "Build");
            node.SetAttribute("ToolsVersion", "12.0");

            doc.AppendChild(node);
            Process(obj, node);

            //Console.WriteLine(doc.OuterXml);
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
