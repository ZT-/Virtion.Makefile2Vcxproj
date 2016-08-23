using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class ItemDefinitionGroup
    {
        [XmlAttribute]
        public string Condition;

        [XmlElement("ClCompile")]
        public ClCompile ClCompile;

        [XmlElement("Link")]
        public Link Link;
    }
}
