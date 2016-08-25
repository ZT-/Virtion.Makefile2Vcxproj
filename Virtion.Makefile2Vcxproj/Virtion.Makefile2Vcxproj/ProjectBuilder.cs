using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Virtion.helper;
using Virtion.Makefile2Vcxproj.Struct;

namespace Virtion.Makefile2Vcxproj
{
    class ProjectBuilder
    {
        public string ProjectName = "TestProject";
        private MakeFileParser parser;

        public ProjectBuilder(MakeFileParser parser)
        {
            this.parser = parser;
        }

        private ItemGroup CreateProjectHeader()
        {
            var item = new ItemGroup();
            item.Label = "ProjectConfigurations";
            item.ProjectConfiguration = new List<ProjectConfiguration>();
            item.ProjectConfiguration.Add(new ProjectConfiguration()
            {
                Include = "Debug|Win32",
                Configuration = "Debug",
                Platform = "Win32"
            });
            return item;
        }

        private PropertyGroup CreateGlobalsPropertyGroup()
        {
            var item = new PropertyGroup()
            {
                Label = "Globals",
                ProjectGuid = "{" + UuidHelper.NewUuidString() + "}",
                Keyword = "Win32Proj",
                RootNamespace = ProjectName
            };
            return item;
        }

        private Import CreateDefaultImport()
        {
            return new Import()
            {
                Project = "$(VCTargetsPath)\\Microsoft.Cpp.Default.props"
            };
        }

        private PropertyGroup CreateConfigurationPropertyGroup()
        {
            var item = new PropertyGroup()
            {
                Condition = "'$(Configuration)|$(Platform)'=='Debug|Win32'",
                Label = "Configuration",
                ConfigurationType = "Application",
                UseDebugLibraries = "true",
                PlatformToolset = "v120",
                CharacterSet = "MultiByte", //<CharacterSet>Unicode</CharacterSet>
                UseOfMfc = "false",
                LinkIncremental = "false",
                CLRSupport = "false"
            };
            return item;
        }

        private Import CreateCppPropsImport()
        {
            return new Import()
            {
                Project = "$(VCTargetsPath)\\Microsoft.Cpp.props"
            };
        }

        private ImportGroup CreateExtensionSettingsImportGroup()
        {
            return new ImportGroup()
            {
                Label = "ExtensionSettings"
            };
        }

        private ImportGroup CreatePropertySheetsImportGroup()
        {
            var item = new ImportGroup()
            {
                Label = "PropertySheets",
                Condition = "'$(Configuration)|$(Platform)'=='Debug|Win32'"
            };
            item.Imports = new List<Import>();
            item.Imports.Add(new Import()
            {
                Project = "$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props",
                Condition = "exists('$(UserRootDir)\\Microsoft.Cpp.$(Platform).user.props')",
                Label = "LocalAppDataPlatform"
            });
            return item;
        }

        private PropertyGroup CreateUserMacrosPropertyGroup()
        {
            var item = new PropertyGroup()
            {
                Label = "UserMacros",
            };
            return item;
        }

        private PropertyGroup CreateConditionPropertyGroup()
        {
            var item = new PropertyGroup()
            {
                Condition = "'$(Configuration)|$(Platform)'=='Debug|Win32'",
                LinkIncremental = "true"
            };
            return item;
        }

        private ClCompile CreateClCompile()
        {
            string includeString = "";
            string definitionsString = "";
            foreach (var name in parser.IncludeList)
            {
                includeString += name + ";";
            }
            includeString += "%(AdditionalIncludeDirectories)";

            foreach (var name in parser.DefineList)
            {
                definitionsString += name + ";";
            }
            definitionsString += "%(PreprocessorDefinitions)";

            var item = new ClCompile()
            {
                PrecompiledHeader = "NotUsing",
                WarningLevel = "Level3",
                Optimization = "Disabled",
                SDLCheck = "true",
                PreprocessorDefinitions = definitionsString,
                AdditionalIncludeDirectories = includeString
            };
            return item;
        }

        private Link CreateLink()
        {
            string libPathString = "";
            foreach (var name in parser.LibPathList)
            {
                libPathString += name + ";";
            }
            libPathString += "%(AdditionalLibraryDirectories)";

            string libString = "";
            foreach (var name in parser.LibList)
            {
                libString += name + ";";
            }
            libString += "%(AdditionalDependencies)";

            var item = new Link()
            {
                SubSystem = "Console",
                GenerateDebugInformation = "true",
                AdditionalLibraryDirectories = libPathString,
                AdditionalDependencies = libString
            };
            return item;
        }

        private ItemDefinitionGroup CreateComplierItemDefinitionGroup()
        {
            var item = new ItemDefinitionGroup()
            {
                Condition = "'$(Configuration)|$(Platform)'=='Debug|Win32'"
            };
            item.ClCompile = CreateClCompile();
            item.Link = CreateLink();
            return item;
        }

        private ItemGroup CreateSourceGroup()
        {
            var item = new ItemGroup();
            item.ClCompiles = new List<ClCompile>();
            foreach (var name in this.parser.SourceList)
            {
                item.ClCompiles.Add(new ClCompile()
                {
                    Include = name
                });
            }
            return item;
        }

        private ItemGroup CreateHeaderGroup()
        {
            var item = new ItemGroup();
            item.ClIncludes = new List<ClInclude>();
            foreach (var name in this.parser.HeaderList)
            {
                item.ClIncludes.Add(new ClInclude()
                {
                    Include = name
                });
            }
            return item;
        }

        private ItemGroup CreateRcGroup()
        {
            var item = new ItemGroup();
            item.ResourceCompiles = new List<ResourceCompile>();
            item.ResourceCompiles.Add(new ResourceCompile()
            {
                Include = parser.RcPath
            });
            return item;
        }

        private Import CreateTargetsImport()
        {
            return new Import()
            {
                Project = "$(VCTargetsPath)\\Microsoft.Cpp.targets"
            };
        }

        private ImportGroup CreateExtensionTargets()
        {
            var item = new ImportGroup()
            {
                Label = "ExtensionTargets"
            };
            return item;
        }

        public void BuildProject(string name, string rootPath)
        {
            var project = new Project();
            project.ProjectHeader = CreateProjectHeader();
            project.GlobalsPropertyGroup = CreateGlobalsPropertyGroup();
            project.DefaultPropsImport = CreateDefaultImport();
            project.ConfigurationPropertyGroup = CreateConfigurationPropertyGroup();
            project.CppPropsImport = CreateCppPropsImport();
            project.ExtensionSettingsImportGroup = CreateExtensionSettingsImportGroup();
            project.PropertySheetsImportGroup = CreatePropertySheetsImportGroup();
            project.UserMacrosPropertyGroup = CreateUserMacrosPropertyGroup();
            project.ConditionPropertyGroup = CreateConditionPropertyGroup();
            project.ComplierItemDefinitionGroup = CreateComplierItemDefinitionGroup();
            project.SourceItemGroup = CreateSourceGroup();
            project.HeaderItemGroup = CreateHeaderGroup();
            project.ResItemGroup = CreateRcGroup();
            project.TargetsImport = CreateTargetsImport();
            project.ExtensionTargetsImportGroup = CreateExtensionTargets();

            string path = rootPath + name + ".vcxproj";
            ObjectToXml.Load(project, path);
            Console.WriteLine(">" + path);
        }


    }
}
