using FluentScheduler;
using Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AllBackup : IJob
{

    public static string MysqlBinDir = ConfigurationManager.AppSettings["MysqlBinDir"].ToString_V();
    public static string MysqlDataDir = ConfigurationManager.AppSettings["MysqlDataDir"].ToString_V();
    public static string Server = ConfigurationManager.AppSettings["Server"].ToString_V();
    public static string Database = ConfigurationManager.AppSettings["Database"].ToString_V();
    public static string Password = ConfigurationManager.AppSettings["Password"].ToString_V();
    public static string User = ConfigurationManager.AppSettings["User"].ToString_V();
    public static string BackupDir = ConfigurationManager.AppSettings["BackupDir"].ToString_V();
    public static int BackupLeft = ConfigurationManager.AppSettings["BackupLeft"].ToInt_V();

    public void Execute()
    {
        Loger.Log("mysql database all backup starting ");
        try
        {
            //保留几天的备份
            string[] directories = Directory.GetDirectories(BackupDir);

            if (directories.Length > BackupLeft - 1)
            {
                List<string> templist = directories.OrderBy(o => o).ToList();
                for (int i = templist.Count; i > BackupLeft; i--)
                {
                    Directory.Delete(Path.Combine(BackupDir, templist[i]), true);
                }
            }

            DateTime date = DateTime.Now;

            string dailypath = $"{date.Year}-{date.Month}-{date.Day}";

            string allBackupPath = Path.Combine(BackupDir, dailypath);

            if (!Directory.Exists(allBackupPath))
            {
                Directory.CreateDirectory(allBackupPath);
            }

            string backupfile = Path.Combine(allBackupPath, "AllBackUP.sql");
            //string backupzip = $backuppath + $database + "_" + $timestamp + ".zip"

            Loger.Log(backupfile);

            string fileName = Path.Combine(MysqlBinDir, "mysqldump.exe");

            //Loger.Log("Password:"+Password);
            string result = exeHelper.RunExeByProcess(fileName,
                $" --user={User} --password={Password} --host={Server} --lock-all-tables --flush-logs --default-character-set=utf8 --hex-blob  {Database} --result-file={backupfile}");

            Loger.Log($"Mysqldump.exe output: {result}");
            Loger.Log($"Allbackup end");

        }
        catch (Exception ex)
        {

            Loger.Log("All backup Error：" + ex.Message);
        }
    }
}