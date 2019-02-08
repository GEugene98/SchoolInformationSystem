using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkScheduler;

namespace WorkSheduler
{
    ////ƒл€ разворачивани€ приложени€ в виде сервиса Windows
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        var logger = Logger.GetInstance();
    //        try
    //        {
    //            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
    //            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

    //            var host = WebHost.CreateDefaultBuilder(args)
    //                .UseStartup<Startup>()
    //                .UseContentRoot(pathToContentRoot)
    //                .UseUrls("http://+:5007")
    //                .Build();

    //            host.RunAsService();
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.Error(ex.Message);
    //        }

    //    }
    //}

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
