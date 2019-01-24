using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WorkScheduler
{
    public class Logger
    {
        private static Logger instance;
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        private Logger()
        { }

        public static Logger GetInstance()
        {
            if (instance == null)
                instance = new Logger();
            return instance;
        }

        public void Log(string message)
        {
            File.AppendAllText(Path.Combine(path, "InformationSystemLogs.txt"), $"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}]  {message}\n\n");
        }

        public void Error(string message)
        {
            File.AppendAllText(Path.Combine(path, "InformationSystemErrors.txt"), $"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}]  {message}\n\n");
        }
    }
}
