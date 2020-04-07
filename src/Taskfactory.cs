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
        switch (AllbackupTimer)
        {
            case 0:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Days().At(3,0);
                break;
            case 1:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
                break;
            case 2:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Tuesday).At(3, 0);
                break;
            case 3:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Wednesday).At(3, 0);
                break;
            case 4:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Wednesday).At(3, 0);
                break;
            case 5:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Friday).At(3, 0);
                break;
            case 6:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Saturday).At(3, 0);
                break;
            case 7:
                Schedule<AllBackup>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Sunday).At(3, 0);
                break;
        }
        if (IncreasebackupTimer > 5)
        { 
            Schedule<IncreaseBackup>().ToRunNow().AndEvery(IncreasebackupTimer).Minutes(); 
        }
        else
        {
            Schedule<IncreaseBackup>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}




