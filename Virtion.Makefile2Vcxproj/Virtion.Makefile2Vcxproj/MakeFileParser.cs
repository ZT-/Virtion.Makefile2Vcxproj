﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Virtion.helper;

namespace Virtion.Makefile2Vcxproj
{
    class OBJCommand
    {
        public string Target;
        public List<string> Command;
        public OBJCommand()
        {
            this.Command = new List<string>();
        }
    }

    class MakeFileParser
    {
        public List<String> CommandLine;
        public List<OBJCommand> ObjCommands;
        public Dictionary<string, string> VariableMap;
        public List<string> ClArgList;
        public List<string> IncludeList;
        public List<string> DefineList;
        public List<string> SourceList;
        public List<string> HeaderList;

        public MakeFileParser()
        {
            this.CommandLine = new List<string>();
            this.VariableMap = new Dictionary<string, string>();
            this.ClArgList = new List<string>();
            this.IncludeList = new List<string>();
            this.DefineList = new List<string>();
            this.SourceList = new List<string>();
            this.HeaderList = new List<string>();
        }

        private bool IsBlankLine(string s)
        {
            if (string.IsNullOrEmpty(s) == true)
            {
                return true;
            }
            foreach (var c in s)
            {
                if (c == ' ' || c == '\n' || c == '\r' || c == '\t')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool Load(string path)
        {
            if (File.Exists(path) == false)
            {
                Console.WriteLine("File is not found! " + path);
                return false;
            }
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            string command = "";
            bool needCombin = false;
            while ((line = sr.ReadLine()) != null)
            {
                if (IsBlankLine(line) || line[0] == '#')
                {
                    continue;
                }
                if (line.EndsWith("\\") == true)
                {
                    needCombin = true;
                    command += line.Substring(0, line.Length - 1);
                }
                else
                {
                    if (needCombin == true)
                    {
                        command += line;
                    }
                    else
                    {
                        command = line;
                    }
                    needCombin = false;
                }

                if (needCombin == false)
                {
                    //Console.WriteLine(command);
                    CommandLine.Add(command);
                    command = "";
                }
            }
            return true;
        }

        private string StringTrim(string s)
        {
            if (string.IsNullOrEmpty(s) == true)
            {
                return s;
            }
            return s.Trim();
        }

        private void ParserVariable(string s)
        {
            int index = s.IndexOf("=");
            if (index < 0)
            {
                return;
            }
            string varName = s.Substring(0, index);
            string value = s.Substring(index + 1);
            this.VariableMap[varName.Trim()] = FillVariable(value.Trim());
        }

        private string FillVariable(string s)
        {
            foreach (var item in this.VariableMap)
            {
                s = s.Replace("$(" + item.Key + ")", item.Value);
            }
            return s;
        }

        private void FilterOBJCommand()
        {
            this.ObjCommands = new List<OBJCommand>();
            bool needNext = false;
            OBJCommand command = null;
            for (int i = 0; i < this.CommandLine.Count; i++)
            {
                string s = this.CommandLine[i];
                if (needNext == true)
                {
                    if (s[0] == '\t')
                    {
                        command.Command.Add(FillVariable(s));
                        continue;
                    }
                    else
                    {
                        command = null;
                        needNext = false;
                    }
                }

                if (s.IndexOf(" : ") > 0)
                {
                    command = new OBJCommand()
                    {
                        Target = this.FillVariable(s),
                    };
                    this.ObjCommands.Add(command);
                    needNext = true;
                }
                else
                {
                    this.ParserVariable(s);
                }
            }
        }

        private bool IsSourceFile(string s)
        {
            if (s.EndsWith(".cpp", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".c", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".cc", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".cxx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private bool IsHeaderFile(string s)
        {
            if (s.EndsWith(".h", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".hpp", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".hh", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".hxx", StringComparison.OrdinalIgnoreCase) ||
               s.EndsWith(".inl", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".inc", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private void AnalysisCl(string s)
        {
            if (s.IndexOf("cl.exe", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return;
            }
            StringStream stringStream = new StringStream(s);
            while (stringStream.IsEof() == false)
            {
                string arg = stringStream.ReadString();
                if (arg == "-I")
                {
                    arg = stringStream.ReadString();
                    if (this.IncludeList.IndexOf(arg) < 0)
                    {
                        IncludeList.Add(arg);
                        Console.WriteLine(arg);
                    }
                    continue;
                }

                if (arg == "-D")
                {
                    arg = stringStream.ReadString();
                    if (this.DefineList.IndexOf(arg) < 0)
                    {
                        DefineList.Add(arg);
                        Console.WriteLine(arg);
                    }
                    continue;
                }

                if (ClArgList.IndexOf(arg) < 0)
                {
                    ClArgList.Add(arg);
                    Console.WriteLine(arg);
                }
            }
        }

        private void AnalysisFile(string s)
        {
            StringStream stringStream = new StringStream(s);
            while (stringStream.IsEof() == false)
            {
                string arg = stringStream.ReadString();
                if (this.IsSourceFile(arg) == true)
                {
                    if (this.SourceList.IndexOf(arg) < 0)
                    {
                        this.SourceList.Add(arg);
                        Console.WriteLine(arg);
                    }
                    continue;
                }
                if (this.IsHeaderFile(arg) == true)
                {
                    if (this.HeaderList.IndexOf(arg) < 0)
                    {
                        this.HeaderList.Add(arg);
                        Console.WriteLine(arg);
                    }
                    continue;
                }
            }
        }

        public bool StartParse()
        {
            this.FilterOBJCommand();
            if (this.ObjCommands.Count == 0)
            {
                return false;
            }

            foreach (var command in ObjCommands)
            {
                AnalysisFile(command.Target);
                //Console.WriteLine(command.Target);
                foreach (var c in command.Command)
                {
                    AnalysisCl(c);
                    // Console.WriteLine(c);
                }
            }
            return true;
        }



    }
}
