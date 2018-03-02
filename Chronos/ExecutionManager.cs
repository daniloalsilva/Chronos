﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;

namespace Chronos
{
    class ExecutionResult {
        public ExecutionResult(int exitCode, string resultContent)
        {
            this.exitCode = exitCode;
            this.resultContent = resultContent;
        }

        private int exitCode;
        public int ExitCode
        {
            get { return exitCode; }
            set { exitCode = value; }
        }

        private string resultContent;
        public string ResultContent
        {
            get { return resultContent; }
            set { resultContent = value; }
        }
    }

    class ExecutionManager
    {
        public ExecutionManager() { }

        List<TimeManager> executionList;

        public static TimeManager LoadSingleConfiguration(TimeManager cachedScript)
        {
            if (!Directory.Exists(cachedScript.CachePath)) { Directory.CreateDirectory(cachedScript.CachePath); };
            try
            {
                using (StreamReader reader = new StreamReader(cachedScript.CacheFilePath))
                {
                    return JsonConvert.DeserializeObject<TimeManager>(reader.ReadToEnd());
                }
            }
            catch (Exception) { return new TimeManager(); }
        }

        /// <summary>
        /// Parse args to get correct value to cacheTime, Timeout, ScriptPath from plugin and params for plugin execution.
        /// Data Parsers will consider:
        ///     Timeout = arg 0
        ///     CacheTime = arg 1
        ///     ScriptPath = arg 2
        /// </summary>
        /// <param name="args"></param>
        public static ExecutionResult ExecutePlugin(string[] args)
        {
            // store actual data from execution request
            TimeManager pluginRequest = new TimeManager()
            {
                Timeout = Int32.Parse(args[0]) * 1000,
                CacheTime = Int32.Parse(args[1]),
                ScriptPath = args[2],
                Name = Path.GetFileName(args[2]),
                LastExecutionTime = DateTime.Now,
                CachePath = Directory.GetCurrentDirectory() + @"\Cache\"
            };

            for (int i = 3; i < args.Length; i++){ pluginRequest.Parameters += args[i] + " "; }

            if (pluginRequest.Parameters == null) { pluginRequest.Parameters = String.Empty; }

            pluginRequest.UpdateCachePath();

            TimeManager lastExecution = ExecutionManager.LoadSingleConfiguration(pluginRequest);
            if (pluginRequest.CompareManagers(lastExecution))
            {
                ExecutionResult result = StartExecution(pluginRequest);
                
                pluginRequest.LastExecutionResult = result.ResultContent;
                pluginRequest.LastExitCode = result.ExitCode;

                using (StreamWriter writer = new StreamWriter(pluginRequest.CacheFilePath))
                {
                    writer.Write(JsonConvert.SerializeObject(pluginRequest));
                }
                return result;
            }
            else {
                return new ExecutionResult(lastExecution.LastExitCode, lastExecution.LastExecutionResult);
            }
        }

        private static ExecutionResult StartExecution(TimeManager timeManager)
        {
            timeManager.PluginExecutor = Executors.GetPluginExecutor(Path.GetExtension(timeManager.ScriptPath).ToLower());
            timeManager.IntialParameters = Executors.GetIntialParameters(Path.GetExtension(timeManager.ScriptPath).ToLower());

            // Inicio da execução do processo
            using (Process proc = new Process())
            {
                try
                {
                    // Diferenciando Scripts executados diretamente
                    ProcessStartInfo procStartInfo;
                    if (timeManager.PluginExecutor == String.Empty)
                        procStartInfo = new ProcessStartInfo(Tools.FormatParams(timeManager.ScriptPath), timeManager.Parameters.Trim());
                    else
                        procStartInfo = new ProcessStartInfo(
                            Tools.FormatParams(timeManager.PluginExecutor),
                            timeManager.IntialParameters + Tools.FormatParams(timeManager.ScriptPath) + " " + timeManager.Parameters.Trim());

                    /// Os comandos abaixo não necessários para redirecionar a saída dos Scripts
                    /// As mesmas serão redirecionadas para o StreamReader Process.StandardOutput
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;

                    // Mantém o promtp em Background
                    procStartInfo.CreateNoWindow = true;

                    // Criamos um processo e assimilamos ao mesmo o ProcessStartInfo
                    proc.StartInfo = procStartInfo;
                    proc.Start();

                    // Mata o processo caso o timeout de execução seja excedido
                    if (!proc.WaitForExit(timeManager.Timeout))
                    {
                        proc.Kill();
                        Console.WriteLine(timeManager.Name + " Timed Out - "); //- Executou mais que "+timeout+" msec");
                    }
                    else
                        Console.Write("");

                    // Exibe o Output do comando executado
                    return new ExecutionResult(proc.ExitCode, proc.StandardOutput.ReadToEnd());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new ExecutionResult(3, e.Message);
                }
                finally
                {
                    proc.Close();
                }
            }
        }
    }
}