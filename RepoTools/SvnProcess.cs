using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    static internal class SvnProcess
    {
        public static SvnProcessObject StartSvnProcess(string WorkingDirectory, string Arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "svn.exe";
            processStartInfo.WorkingDirectory = WorkingDirectory;
            processStartInfo.Arguments = Arguments;
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

            SvnProcessObject svnProcessObject = new();
            svnProcessObject.StandardOutput = standardOutput;
            svnProcessObject.ErrorOutput = errorOutput;
            svnProcessObject.ExitCode = process.ExitCode;

            return svnProcessObject;
        }
    }
}
