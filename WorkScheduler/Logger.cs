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
        private string path = @"C:\";

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
            File.WriteAllText(@"C:\InnerApplicationsLogs.txt", $"\nIS: {message}");
        }

        public void Error(string message)
        {
            File.WriteAllText(@"C:\InnerApplicationsErrors.txt", $"\nIS: {message}");
        }
    }
}
