using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FluentScheduler;
using Helper;

public class Taskfactory : Registry
{
    public static int AllbackupTimer = ConfigurationManager.AppSettings["allBackup"].ToInt_V();
    public static int IncreasebackupTimer = ConfigurationManager.AppSettings["increaseBackup"].ToInt_V();
    public Taskfactory()
    {
        Schedule<AllBackup>().ToRunNow().AndEvery(10).Minutes();
        Schedule<IncreaseBackup>().ToRunNow().AndEvery(10).Minutes();
    }
}




