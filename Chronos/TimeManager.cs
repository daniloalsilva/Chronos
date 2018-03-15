using System;
using System.Text;

namespace Chronos
{
    public class TimeManager
    {

        #region Variables
        /// <summary>
        /// Script Path from plugin
        /// </summary>
        public string ScriptPath { get; set; }

        /// <summary>
        /// Time to keep the cache from executed plugin
        /// </summary>
        public int CacheTime { get; set; }

        public string CachePath { get; set; }

        public string CacheFilePath { get; set; }

        /// <summary>
        /// Get the last execution time from plugin described on TimeManager scriptPath
        /// </summary>
        public DateTime LastExecutionTime { get; set; }


        /// <summary>
        /// Last result from plugin execution, keeped in cache
        /// </summary>
        public string LastExecutionResult { get; set; }


        /// <summary>
        /// Timeout form 
        /// </summary>
        public int Timeout { get; set; }

        public string Parameters { get; set; }

        public string Name { get; set; }

        public string PluginExecutor { get; set; }

        public string IntialParameters { get; set; }

        public int LastExitCode { get; set; }

        #endregion

        /// <summary>
        /// Compare current object TimeManager with TimeManager used in parameters.
        /// </summary>
        /// <param name="olderTimer">TimeManager obj</param>
        /// <returns>true, if cache time exceeded value or some attribute of object like ScriptPath are not equal; false if cache time is not exceeded</returns>
        public bool CompareManagers(TimeManager olderTimer) {
            try
            {
                if (String.Equals(this.ScriptPath, olderTimer.ScriptPath))
                {
                    TimeSpan timeInterval = this.LastExecutionTime.Subtract(olderTimer.LastExecutionTime);
                    if (timeInterval < new TimeSpan(0, this.CacheTime, 0))
                    {
                        return false;
                    }
                    else { return true; }
                }
                else { return true; }
            }
            catch (Exception) { return true; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}_{1}_{2}_{3}", Environment.MachineName, this.ScriptPath, this.IntialParameters, this.Parameters);
            return sb.ToString().ToBase64String();
        }

        public string GetMD5HashCode()
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var utf8 = new UTF8Encoding();
            return BitConverter.ToString(md5.ComputeHash(utf8.GetBytes(this.ToString()))).Replace("-", "");
        }

        public void UpdateCachePath() 
        {
            // set cache file name with hash of TimeManager object (avoid create cache files with same name)
            this.CacheFilePath = this.CachePath + this.Name + "_" + this.GetMD5HashCode() + ".chronos";
        }

    }

    public static class StringExtension
    {
        public static string ToBase64String(this string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
