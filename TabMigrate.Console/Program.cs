using System;
using System.Collections.Generic;
using System.Threading;

namespace TabMigrate.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var compiledArgs = new List<string>(args);
            if(compiledArgs.Count==0)
            {
                PrintUsage();
                return;;
            }
            compiledArgs.Add("-command");
            compiledArgs.Add("siteImport");
            CommandLineParser commandLine = new CommandLineParser(compiledArgs);
            TaskMaster task = null;
            //Parse the details from the command line
            try
            {
                task = TaskMaster.FromCommandLine(commandLine);
            }
            catch (Exception exParseCommandLine)
            {
                ConsoleColor currentColor = System.Console.ForegroundColor;
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("Error parsing command line: " + exParseCommandLine);
                System.Console.ForegroundColor = currentColor;
                return;
            }

            Thread workThread = task.CreateWorkThread();
            workThread.Start();
            workThread.Join();
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine($@"
Usage: TabMigrate 
    -toSiteUserId <USERID> 
    -toSiteUserPassword <USERPASSWORD> 
    -importDirectory <SOURCE DIRECTORY> 
    -toSiteUrl <TABLEAU SITE URL> 
    -toSiteIsSiteAdmin <true/false> 
    -remapDataserverReferences <true/false> 
    -remapContentOwnership <true/false> 
    -logVerbose <true/false> 
    -logFile <LOG FILE PATH> 
    -errorFile <ERROR FILE PATH> 
    -manualStepsFile <PATH TO MANUAL STEPS> 
    -targetProject <TARGET PROJECT NAME>
");
        }
    }
}
