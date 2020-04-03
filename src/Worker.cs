using System;
using System.Threading;
using System.Configuration;
using FluentScheduler;
using Helper;

class Program
{
    /// <summary>
    /// 
    /// </summary>
    public static bool running;



    public static void Loop()
    {
        Loger.Log("Started");

        var taskfactory = new Taskfactory();

        JobManager.Initialize(taskfactory);
        
        while (running)
        {
            //Log("Running");
            Thread.Sleep(2000);
        }

        Loger.Log("Stopped");
    }

    public static void Main()
    {
        var th = new Thread(Loop);

        running = true;
        th.Start();

        //var msg= Console.ReadLine();
        //Log($"Received message \"{msg}\" from the Monitor");

        //running = false;
        th.Join();
        
    }
}