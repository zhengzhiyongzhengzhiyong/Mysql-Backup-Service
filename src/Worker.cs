using System;
using System.Threading;
using System.Configuration;
using FluentScheduler;

class Program
{
    public static bool running;

    public static void Log(string s)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {s}");
    }

    public static void Loop()
    {
        Log("Started SampleWorker(c# version)");

        //JobManager.Initialize(new Taskfactory());
        //while (running)
        //{
        //    Log("Running");
        //    var url = ConfigurationManager.AppSettings["UrlToPing"].ToString();
        //    Log(url);
        //    Thread.Sleep(2000);
        //}
        Log("Stopped SampleWorker(c# version)");
    }

    public static void Main()
    {
        //var th = new Thread(Loop);

        //running = true;
        //th.Start();

        var msg
            = Console.ReadLine();
        Log($"Received message \"{msg}\" from the Monitor");

        //running = false;
        //th.Join();
    }
}