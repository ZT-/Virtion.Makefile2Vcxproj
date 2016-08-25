using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class ImportGroup
    {
        [XmlAttribute]
        public string Label;

        [XmlAttribute]
        public string Condition;

        [XmlElement("Import")]
        public List<Import> Imports;

        [XmlElement("ClCompile")]
        public List<ClCompile> ClCompiles;

        [XmlElement("ClCompile")]
        public List<ClInclude> ClIncludes;

        [XmlElement("ClCompile")]
        public List<ResourceCompile> ResourceCompiles;

        [XmlElement("Filter")]
        public List<Filter> Filters;

    }
}
