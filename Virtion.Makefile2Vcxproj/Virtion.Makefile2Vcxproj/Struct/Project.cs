using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Virtion.Makefile2Vcxproj.Struct
{
    [XmlRoot(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Project
    {
        [XmlAttribute]
        public string DefaultTargets = "Build";
        [XmlAttribute]
        public string ToolsVersion = "12.0";

        [XmlElement("ItemGroup")]
        public ItemGroup ProjectHeader;

        [XmlElement("PropertyGroup")]
        public PropertyGroup GlobalsPropertyGroup;

        [XmlElement("Import")]
        public Import DefaultPropsImport;

        [XmlElement("PropertyGroup")]
        public PropertyGroup ConfigurationPropertyGroup;

        [XmlElement("Import")]
        public Import CppPropsImport;

        [XmlElement("ImportGroup")]
        public ImportGroup ExtensionSettingsImportGroup;

        [XmlElement("ImportGroup")]
        public ImportGroup PropertySheetsImportGroup;

        [XmlElement("PropertyGroup")]
        public PropertyGroup UserMacrosPropertyGroup;

        [XmlElement("PropertyGroup")]
        public PropertyGroup ConditionPropertyGroup;

        [XmlElement("ItemDefinitionGroup")]
        public ItemDefinitionGroup ComplierItemDefinitionGroup;

        [XmlElement("ItemGroup")]
        public ItemGroup SourceItemGroup;

        [XmlElement("ItemGroup")]
        public ItemGroup HeaderItemGroup;

        [XmlElement("ItemGroup")]
        public ItemGroup ResItemGroup;

        [XmlElement("Import")]
        public Import TargetsImport;

        [XmlElement("ImportGroup")]
        public ImportGroup ExtensionTargetsImportGroup;

        public Project()
        {
        }
    }



}
