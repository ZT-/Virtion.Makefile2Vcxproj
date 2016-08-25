using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class ProjectFilter
    {
        [XmlAttribute]
        public string DefaultTargets = "Build";
        [XmlAttribute]
        public string ToolsVersion = "12.0";

        [XmlElement("ItemGroup")]
        public ItemGroup FilterDefine;

        [XmlElement("ItemGroup")]
        public ItemGroup SourceFilter;

        [XmlElement("ItemGroup")]
        public ItemGroup HeaderFilter;

        [XmlElement("ItemGroup")]
        public ItemGroup ResFilter;

    }
}
