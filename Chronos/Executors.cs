using System;
using System.Collections.Generic;

namespace Chronos
{
    public static class Executors
    {

        public static string GetPluginExecutor(string extension)
        {
            var executors = new Dictionary<string, string>(){
                {".ps1", @"\WindowsPowerShell\v1.0\powershell.exe"},
                {".vbs", @"\cscript.exe"},
                {".wsf", @"\WScript.exe"}
            };

            try
            {
                return Environment.SystemDirectory + executors[extension];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetIntialParameters(string extension)
        {
            // create possible initial parameters, depending on file extension
            var intialParameters = new Dictionary<string, string>(){
                {".ps1", @"-File "},
                {".vbs", @"//NoLogo "},
            };
            
            // return its corresponding 
            try
            {
                return intialParameters[extension];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
