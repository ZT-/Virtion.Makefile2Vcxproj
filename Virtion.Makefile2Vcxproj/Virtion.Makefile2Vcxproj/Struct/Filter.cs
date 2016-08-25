using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class Filter
    {
        [XmlAttribute]
        public string Include;

        public string UniqueIdentifier;
        public string Extensions;
    }
}
