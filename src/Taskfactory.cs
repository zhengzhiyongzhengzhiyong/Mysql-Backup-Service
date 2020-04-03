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
    }
}

public class AllBackup : IJob
{

    public static string MysqlBinDir = ConfigurationManager.AppSettings["MysqlBinDir"].ToString_V();
    public static string MysqlDataDir = ConfigurationManager.AppSettings["MysqlDataDir"].ToString_V();
    public static string Server = ConfigurationManager.AppSettings["Server"].ToString_V();
    public static string Database = ConfigurationManager.AppSettings["Database"].ToString_V();
    public static string Password = ConfigurationManager.AppSettings["MysqlBinDir"].ToString_V();
    public static string User = ConfigurationManager.AppSettings["User"].ToString_V();
    public static string BackupDir = ConfigurationManager.AppSettings["BackupDir"].ToString_V();
    public static int BackupLeft = ConfigurationManager.AppSettings["BackupLeft"].ToInt_V();

    public void Execute()
    {

        Loger.Log("mysql database backup starting ");
        try
        {
            //保留几天的备份
            string[] directories = Directory.GetDirectories(BackupDir);


            if (directories.Length > BackupLeft - 1)
            {
                List<string> templist = directories.OrderBy(o => o).ToList();
                for (int i = templist.Count; i > BackupLeft; i--)
                {
                    Directory.Delete(templist[i], true);
                }
            }

            DateTime date = DateTime.Now;

            string dailypath = $"{date.Year}-{date.Month}-{date.Day}-{date.Hour}";

            string allBackupPath = Path.Combine(BackupDir, dailypath);

            if (!Directory.Exists(allBackupPath))
            {
                Directory.CreateDirectory(allBackupPath);
            }

            string backupfile = Path.Combine(allBackupPath , "AllBackUP.sql");
            //string backupzip = $backuppath + $database + "_" + $timestamp + ".zip"


            string output = string.Empty;
            var psi = new ProcessStartInfo()
            {
                FileName = MysqlBinDir + "/mysqldump.exe", //path to your *.exe
                Arguments = $" -u{User} -p{Password} -h{Server} --lock-all-tables --flush-logs --default-character-set=utf8 --hex-blob  {Database} >  {backupfile}", //arguments
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true //no window, you can't show it anyway
            };
            var p = Process.Start(psi);
            output = p.StandardOutput.ReadToEnd();

            Loger.Log($"Mysqldump.exe output: {output}");
            Loger.Log($"Allbackup end");

        }
        catch (Exception ex)
        {

            Loger.Log("All backup Error：" + ex.Message);
        }
    }
}


