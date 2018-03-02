using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chronos
{
    public class Executors
    {

        public static string GetPluginExecutor(string timeManager)
        {
            Dictionary<string, string> executors = new Dictionary<string, string>(){
                {".ps1", @"\WindowsPowerShell\v1.0\powershell.exe"},
                {".vbs", @"\cscript.exe"},
                {".wsf", @"C:\Windows\System32\WScript.exe"}
            };

            try
            {
                return Environment.SystemDirectory + executors[timeManager];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetIntialParameters(string timeManager)
        {
            Dictionary<string, string> intialParameters = new Dictionary<string, string>(){
                {".ps1", @"-File "},
                {".vbs", @"//NoLogo "},
            };
            
            try
            {
                return intialParameters[timeManager];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
