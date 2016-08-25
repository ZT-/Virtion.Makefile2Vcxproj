# Virtion.Makefile2Vcxproj
This tool use to convert makefile to vs2013 .vcxproj on windows

#Usage
```
Usage: <makefile> <project name> [Option]
Example: Virtion.Makefile2Vcxproj.exe   c:\MyProject\Makefile   MyProject  -d
Option:
         [-d]: Dump anlysis info
```

#Feature
+ Convert makefile to vs2013 .vcxproj 
+ Ceate .vcxproj.filters
+ Show used option about cl.exe and link.exe  


#TODO
+ Support "!include" other makefile file.
+ Specific classification of options about cl.exe and link.exe.
