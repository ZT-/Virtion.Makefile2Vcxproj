using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    public class ClCompile
    {
        [XmlAttribute]
        public string Include;

        public string PrecompiledHeader;
        public string WarningLevel;
        public string Optimization;
        public string PreprocessorDefinitions;
        public string SDLCheck;
        public string AdditionalIncludeDirectories;
        public string AdditionalOptions;
        public string Filter;
    }

}
