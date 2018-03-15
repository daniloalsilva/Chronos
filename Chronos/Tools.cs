using System;
using System.IO;
using System.Linq;

namespace Chronos
{
    class Tools
    {
        private static void Help()
        {
            Console.WriteLine(@"
 ┌─┐┬ ┬┬─┐┌─┐┌┐┌┌┬┐┌─┐
 │  ├─┤├┬┘│ ││││├┼┤└─┐
 └─┘┴ ┴┴└─└─┘┘└┘└┴┘└─┘
 
 Software used to store a cache from a software/plugin/script
 execution for a specified time. 

 If execution time exceeded defined 'timeout', Chronos will
 terminate plugin execution.
 
 Stored cache data will be allocated on current execution directory.
 ──────────────────────────────────────────────────────────────────────────────
 Examples:
 ------------------------------------------------------------------------------
    C:\>SomeApp\Chronos.exe [params]
       ^
       └─── cache will be created in C:\Cache
 ------------------------------------------------------------------------------
    C:\SomeApp>Chronos.exe [params]
              ^
              └─── cache will be created in C:\SomeApp\Cache
 ──────────────────────────────────────────────────────────────────────────────
 Params Explanation:
 ------------------------------------------------------------------------------
    Chronos.exe 30 10 C:\Windows\System32\cmd.exe /c dir
                ^  ^  ^                           ^    ^
                │  │  └ plugin path        plugin └──┬─┘
                │  └─── cache time (min)   params ───┘
                └────── timeout (sec)
 ------------------------------------------------------------------------------
    Chronos.exe clearCache
                ^        ^
                └────┬───┘
                     └─── clear all Chronos cache
 ──────────────────────────────────────────────────────────────────────────────
 Use Examples:
 ------------------------------------------------------------------------------
    Chronos.exe 300 60 C:\Scripts\InstallWebFeatures.ps1
    Chronos.exe 30 10 C:\monitoring\check_uptime.vbs 50 60
    Chronos.exe clearCache
");
        }

        private static void ClearCache()
        {
            string[] dir = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Cache\");
            foreach (string item in dir)
            {
                var obj = item.Split(new char[] { '.' });
                if (String.Equals(obj.Last(), "chronos"))
                {
                    File.Delete(item);
                }
            }
        }

        public static bool ValidateParams(string[] args)
        {

            if (args == null || args.Length == 1)
            {
                if (String.Equals(args[0].ToLower(), "clearcache"))
                {
                    try
                    {
                        ClearCache();
                        Console.WriteLine("Chronos Cache cleared.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Something wrong. {e.Message}");
                    }
                    return false;
                }
                Help();
                return false;
            }

            if (args.Length < 3)
            {
                Help();
                return false;
            }

            if (!File.Exists(args[2].Replace("\"", "")))
            {
                Console.WriteLine($"File {args[2]} doesn't exists");
                Help();
                return false;
            }
            try
            {
                Int32.Parse(args[0]);
                Int32.Parse(args[1]);
                return true;
            }
            catch (Exception)
            {
                Help();
                return false;
            }
        }

        public static string FormatPath(string scriptPath)
        {
            return scriptPath.Contains(' ') ? $"\"{scriptPath}\"" : scriptPath; 
        }

        public static string[] FormatPath(string[] scriptPath)
        {
            for (var i = 0; i < scriptPath.Length; i++)
                if (scriptPath[i].Contains(' '))
                    scriptPath[i] = $"\"{scriptPath[i]}\"";
            return scriptPath;
        }
    }
}
