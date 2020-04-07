using FluentScheduler;
using Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class IncreaseBackup : IJob
{
    public static string MysqlBinDir = ConfigurationManager.AppSettings["MysqlBinDir"].ToString_V();
    public static string MysqlDataDir = ConfigurationManager.AppSettings["MysqlDataDir"].ToString_V();
    public static string Server = ConfigurationManager.AppSettings["Server"].ToString_V();
    public static string Database = ConfigurationManager.AppSettings["Database"].ToString_V();
    public static string Password = ConfigurationManager.AppSettings["Password"].ToString_V();
    public static string User = ConfigurationManager.AppSettings["User"].ToString_V();
    public static string BackupDir = ConfigurationManager.AppSettings["BackupDir"].ToString_V();
    public static int BackupLeft = ConfigurationManager.AppSettings["BackupLeft"].ToInt_V();
    public static int IncreaseBackupLeft = ConfigurationManager.AppSettings["IncreaseBackupLeft"].ToInt_V();

    public void Execute()
    {
        Loger.Log("mysql database increase backup starting ");
        try
        {
            DateTime date = DateTime.Now;

            string dailypath = $"{date.Year}-{date.Month}-{date.Day}";

            string IncreaseBackupFiles = $"{date.Year}-{date.Month}-{date.Day}-{date.Hour}-{date.Minute}.sql";

            string allBackupPath = Path.Combine(BackupDir,dailypath);

            string backupfile = Path.Combine(allBackupPath, "AllBackUP.sql");

            string IncreaseBackupFilePath = Path.Combine(allBackupPath, IncreaseBackupFiles);

            bool isresult =Directory.Exists(backupfile);

            if (!File.Exists(backupfile))
            {
                Loger.Log($"{backupfile} not exist");
                return;
            }

            //刷新log_bin
            string result= exeHelper.RunExeByProcess(Path.Combine(MysqlBinDir, "mysql.exe"),
            $" --user={User} --password={Password} --host={Server} --execute=\"flush logs;\"");
            Loger.Log($"mysql.exe output: {result}");

            DateTime AllBackupTime = new FileInfo(backupfile).CreationTime;

            Loger.Log($"All Backup Time output: {AllBackupTime.ToLongTimeString()}");

            DirectoryInfo di = new DirectoryInfo(MysqlDataDir);

            var allLogBinArr = di.GetFiles("mysql-bin.*");

            Loger.Log("Log_bin count:"+ allLogBinArr.Length);

            //清除log_bin
            if (allLogBinArr.Length > 3)
            {

               var allLogBinArrDesc = allLogBinArr.OrderByDescending(o=>o.CreationTime).ToList();

                result = exeHelper.RunExeByProcess(Path.Combine(MysqlBinDir, "mysql.exe"), $" --user={User} --password={Password} --host={Server} --execute=\"purge master logs to '{allLogBinArrDesc[2].Name}';\"");
                Loger.Log($"mysql.exe output: {result}");
            }

            List<FileInfo> LogbinArr = di.GetFiles("mysql-bin.*").Where(x => x.CreationTime > AllBackupTime).ToList();

            Loger.Log($"Log bin arr: {LogbinArr.Count}");

            for (int i = 0; i < LogbinArr.Count-2; i++)
            {
                result = exeHelper.RunExeByProcess(Path.Combine(MysqlBinDir, "mysqlbinlog.exe"),
                   $" --user={User} --password={Password} --host={Server} --read-from-remote-server --base64-output=\"decode-rows\" --database={Database} --result-file={IncreaseBackupFilePath} {LogbinArr[i].Name}");

                Loger.Log($"mysqlbinlog.exe output: {result}");
            }

            Loger.Log($"Increase backup end");
        }
        catch (Exception ex)
        {
            Loger.Log("Increase backup Error：" + ex.Message);
        }
    }
}
