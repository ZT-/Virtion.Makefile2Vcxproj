using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Virtion.helper;

namespace Virtion.Makefile2Vcxproj
{
    class Program
    {
        private static void Usage()
        {
            string s = "Virtion.Makefile2Vcxproj makefile to .vcxproj\n" +
            "Usage: <makefile> <project name> [Option] \n" +
            "Example: Virtion.Makefile2Vcxproj.exe   c:\\MyProject\\Makefile   MyProject  -d\n" +
            "Option: \n" +
            "\t [-d]: Dump anlysis info\n"
            ;
            Console.WriteLine(s);
        }

        private static void Dump(MakeFileParser parser)
        {
            Console.BufferHeight = 200;
            string s = "\tVirtion.Makefile2Vcxproj  Anlysis  Info\n";
            s += "=======================================================";
            Console.WriteLine(s);
            Console.WriteLine("Source :");

            Console.BufferHeight += parser.SourceList.Count;
            foreach (var name in parser.SourceList)
            {
                Console.WriteLine("\t" + name);
            }

            Console.BufferHeight += parser.HeaderList.Count;
            Console.WriteLine("Header :");
            foreach (var name in parser.HeaderList)
            {
                Console.WriteLine("\t" + name);
            }

            Console.WriteLine("Res :");
            Console.WriteLine("\t" + parser.RcPath);

            Console.WriteLine("Cl Complier Define :");
            foreach (var name in parser.DefineList)
            {
                Console.WriteLine("\t" + name);
            }
            Console.BufferHeight += parser.DefineList.Count;

            Console.WriteLine("Cl Include Path :");
            foreach (var name in parser.IncludeList)
            {
                Console.WriteLine("\t" + name);
            }
            Console.BufferHeight += parser.IncludeList.Count;

            Console.WriteLine("Cl Other Option :");
            foreach (var name in parser.ClArgList)
            {
                Console.WriteLine("\t" + name);
            }
            Console.BufferHeight += parser.ClArgList.Count;

            Console.WriteLine("Link Include Path :");
            foreach (var name in parser.LibPathList)
            {
                Console.WriteLine("\t" + name);
            }
            Console.BufferHeight += parser.LibPathList.Count;


            Console.BufferHeight += parser.LibList.Count;
            Console.WriteLine("Link Depend Lib :");
            foreach (var name in parser.LibList)
            {
                Console.WriteLine("\t" + name);
            }

            Console.BufferHeight += parser.LinkArgList.Count;
            Console.WriteLine("Link Depend Lib :");
            foreach (var name in parser.LinkArgList)
            {
                Console.WriteLine("\t" + name);
            }

            s = "=======================================================";
            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return;
            }

            if (File.Exists(args[0]) == false)
            {
                Console.WriteLine("Can't open file " + args[0]);
                return;
            }

            string path = Path.GetDirectoryName(Path.GetFullPath(args[0])) + "\\";
            MakeFileParser parser = new MakeFileParser();
            if (parser.Load(args[0]) == true)
            {
                parser.StartParse();

                if (args.Length >= 3 && (args[2].ToLower() == "-d"))
                {
                    Dump(parser);
                }

                ProjectBuilder projectBuilder = new ProjectBuilder(parser);
                projectBuilder.BuildProject(args[1], path);

                FilterBuilder filterBuilder = new FilterBuilder(parser);
                filterBuilder.BuildProject(args[1], path);
            }
            Console.ReadKey();
        }

    }
}
