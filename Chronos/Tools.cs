using System;
using System.IO;
using System.Linq;

namespace Chronos
{
    class Tools
    {
        public static void Help (){
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
        
        public static void ClearCache() {
            string[] dir = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Cache\");
            foreach (string item in dir){
                var obj = item.Split(new char[]{'.'});
                if (String.Equals(obj.Last(),"chronos")){
                    File.Delete(item);
                }
            }
        }

        public static bool ValidateParams(string[] args) {

            if (args == null || args.Length == 1)
            {
                if (String.Equals(args[0].ToLower(), "clearcache"))
                {
                    try
                    {
                        ClearCache();
                        Console.WriteLine(String.Format("Chronos Cache cleared."));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(String.Format("Something wrong. {0}", e.Message));
                    }
                    return false;
                }
                Help();
                return false;
            }

            if (args.Length < 3){
                Help();
                return false;
            }

            if (!File.Exists(args[2].Replace("\"",""))) {
                Console.WriteLine(String.Format("File {0} doesn't exists",args[2]));
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

        public static string FormatParams(string x)
        {
            if (x.Contains(' '))
                x = '"' + x + '"';
            return x;
        }
        
        public static string[] FormatParams(string[] x)
        {
            for (int i = 0; i < x.Length; i++)
                if (x[i].Contains(' '))
                    x[i] = '"' + x[i] + '"';
            return x;
        }

        //public static string CollectHash(object obj) {
            
        //    string parameters = "";

        //    Type myType = obj.GetType();
        //    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

        //    foreach (PropertyInfo prop in props)
        //    {
        //        object propValue = prop.GetValue(obj, null);
        //        if (propValue != null)
        //        {
        //            parameters += propValue;
        //        }
        //    }

        //    var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    var utf8 = new UTF8Encoding();
        //    return BitConverter.ToString(md5.ComputeHash(utf8.GetBytes(parameters))).Replace("-", "");
        //}
    }
}
