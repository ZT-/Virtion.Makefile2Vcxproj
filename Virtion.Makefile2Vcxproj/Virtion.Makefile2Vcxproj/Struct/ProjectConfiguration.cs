using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{

    public class ProjectConfiguration
    {
        [XmlAttribute]
        public string Include;

        public string Configuration;
        public string Platform;
    }
}
