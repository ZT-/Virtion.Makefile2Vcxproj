using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Virtion.helper;
using Virtion.Makefile2Vcxproj.Struct;

namespace Virtion.Makefile2Vcxproj
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeFileParser parser = new MakeFileParser();
            if (parser.Load("makefile") == true)
            {
                parser.StartParse();
            }

            ProjectBuilder projectBuilder = new ProjectBuilder(parser);
            projectBuilder.BuildProject("Test1.vcxproj");

        }
    }
}
