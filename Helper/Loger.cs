using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Loger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public static async void  Log(string s)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {s}");
            //await Task.Run(()=>Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {s}"));
        }
    }
}
