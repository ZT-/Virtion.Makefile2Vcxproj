using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class PropertyGroup
    {
        [XmlAttribute]
        public string Label;
        [XmlAttribute]
        public string Condition;


        public string ProjectGuid;
        public string Keyword;
        public string RootNamespace;


        public string ConfigurationType;
        public string UseDebugLibraries;
        public string PlatformToolset;
        public string CharacterSet;
        public string UseOfMfc;
        public string CLRSupport;

        public string LinkIncremental;

    }
}
