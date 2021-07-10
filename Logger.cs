using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DnDTool
{
    /// <summary>
    /// Static class for logging messages (infos, errors)
    /// </summary>
    public static class Logger
    {
        // Always in /Log/ directory
        static string logPath = Directory.GetCurrentDirectory() + @"/Log/";

        public static void Log(string msg, string err = null)
        {
            // If folder doesn't exist, create it
            Directory.CreateDirectory(logPath);

            // Log anything into common .txt
            using (StreamWriter sw = File.AppendText(logPath + "log.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(msg);
                sw.WriteLine();
            }

            // Log err into separate .txt
            if (err != null)
            {
                using (StreamWriter sw = File.AppendText(logPath + $"err{DateTime.Now}.txt"))
                {
                    sw.WriteLine(msg);
                    sw.WriteLine(err);
                    sw.WriteLine();
                }
            }
        }
    }
}
