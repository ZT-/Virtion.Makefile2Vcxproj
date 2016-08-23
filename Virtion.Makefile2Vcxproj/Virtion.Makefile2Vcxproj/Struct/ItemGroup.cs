using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class ItemGroup
    {
        [XmlAttribute]
        public string Label;
        [XmlElement]
        public List<ProjectConfiguration> ProjectConfiguration;

        [XmlElement("ClInclude")]
        public List<ClInclude> ClIncludes;
        [XmlElement("ClCompile")]
        public List<ClCompile> ClCompiles;

    }




}
