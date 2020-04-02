using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using FluentScheduler;

  public class Taskfactory : Registry
    {
        public Taskfactory()
        {
            var url = ConfigurationManager.AppSettings["UrlToPing"].ToString();

            Schedule<AllBackup>().ToRunNow().AndEvery(2).Seconds();
        }
    }

    public class AllBackup:IJob
    {
        public void Execute()
        {
            Console.WriteLine("mysql backup");
        }
    }


