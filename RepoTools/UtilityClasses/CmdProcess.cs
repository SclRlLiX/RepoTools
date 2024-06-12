using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools.UtilityClasses
{
    internal static class CmdProcess
    {
        public static CmdProcessObject StartCmdProcess(string Arguments, string WorkingDirectory = @"C:\Windows\System32")
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.WorkingDirectory = WorkingDirectory;
            processStartInfo.Arguments = $@"c\ {Arguments}";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            string? errorOutput = null;
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { errorOutput += e.Data; });

            process.Start();


            // Read Output
            // To avoid deadlocks, use an asynchronous read operation on at least one of the streams.  
            process.BeginErrorReadLine();
            string standardOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();


            Debug.WriteLine("ErrorOutput:" + errorOutput);
            Debug.WriteLine("StandardOutput:" + standardOutput);

            CmdProcessObject cmdProcessObject = new();
            cmdProcessObject.StandardOutput = standardOutput;
            cmdProcessObject.ErrorOutput = errorOutput;
            cmdProcessObject.ExitCode = process.ExitCode;

            Debug.WriteLine("ExitCode: " + process.ExitCode);
            Debug.WriteLine("Arguments: " + Arguments);

            return cmdProcessObject;
        }
    }
}
