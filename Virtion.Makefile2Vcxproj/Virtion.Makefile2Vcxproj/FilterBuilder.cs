using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtion.helper;
using Virtion.Makefile2Vcxproj.Struct;

namespace Virtion.Makefile2Vcxproj
{
    public class FilterBuilder
    {
        public string ProjectName = "TestProject";
        private MakeFileParser parser;

        public FilterBuilder(MakeFileParser parser)
        {
            this.parser = parser;
        }

        private ItemGroup CreateFilterItemGroup()
        {
            var group = new ItemGroup();
            group.Filters = new List<Filter>();
            group.Filters.Add(new Filter()
            {
                Include = "Source",
                UniqueIdentifier = "{" + UuidHelper.NewUuidString() + "}",
                Extensions = "cpp;c;cc;cxx;def;odl;idl;hpj;bat;asm;asmx"
            });

            group.Filters.Add(new Filter()
            {
                Include = "Header",
                UniqueIdentifier = "{" + UuidHelper.NewUuidString() + "}",
                Extensions = "h;hh;hpp;hxx;hm;inl;inc;xsd"
            });

            group.Filters.Add(new Filter()
            {
                Include = "Resource",
                UniqueIdentifier = "{" + UuidHelper.NewUuidString() + "}",
                Extensions = "rc;ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe;resx;tiff;tif;png;wav;mfcribbon-ms"
            });

            return group;
        }

        private ItemGroup CreateSourceItemGroup()
        {
            var group = new ItemGroup();
            group.ClCompiles = new List<ClCompile>();
            foreach (var name in parser.SourceList)
            {
                group.ClCompiles.Add(new ClCompile()
                {
                    Include = name,
                    Filter = "Source"
                });
            }
            return group;
        }

        private ItemGroup CreateHeaderItemGroup()
        {
            var group = new ItemGroup();
            group.ClIncludes = new List<ClInclude>();
            foreach (var name in parser.HeaderList)
            {
                group.ClIncludes.Add(new ClInclude()
                {
                    Include = name,
                    Filter = "Header"
                });
            }
            return group;
        }

        private ItemGroup CreateResItemGroup()
        {
            var group = new ItemGroup();
            group.ResourceCompiles = new List<ResourceCompile>();
            group.ResourceCompiles.Add(new ResourceCompile()
            {
                Include = parser.RcPath,
                Filter = "Resource"
            });
            return group;
        }

        public void BuildProject(string name, string rootPath)
        {
            this.ProjectName = name;
            var project = new ProjectFilter();
            project.FilterDefine = CreateFilterItemGroup();
            project.SourceFilter = CreateSourceItemGroup();
            project.HeaderFilter = CreateHeaderItemGroup();
            project.ResFilter = CreateResItemGroup();

            string path = rootPath + name + ".vcxproj.filters";
            ObjectToXml.Load(project, path);
            Console.WriteLine(">" + path);
        }

    }

}
