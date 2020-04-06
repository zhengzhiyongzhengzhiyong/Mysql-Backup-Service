using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
     public class exeHelper
    {
        public static string RunExeByProcess(string exePath, string argument)
        {
            //开启新线程
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //调用的exe的名称
            process.StartInfo.FileName = exePath;
            //传递进exe的参数
            process.StartInfo.Arguments = argument;
            process.StartInfo.UseShellExecute = false;
            //不显示exe的界面
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            process.StandardInput.AutoFlush = true;

            string result = null;
            while (!process.StandardOutput.EndOfStream)
            {
                result += process.StandardOutput.ReadLine() + Environment.NewLine;
            }
            process.WaitForExit();
            return result;
        }
    }
}
