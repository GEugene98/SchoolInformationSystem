using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WorkScheduler.Services
{
    public class FileService
    {
        protected readonly string RootPath;
        protected readonly Context Db;
        protected readonly IConfiguration Configuration;
        private readonly Logger Logger;

        public FileService(Context context, IConfiguration configuration)
        {
            Db = context;
            Logger = Logger.GetInstance();
            Configuration = configuration;
            RootPath = configuration.GetValue<string>("UploadPath");
        }

        public string AddFile(IFormFile file, string transactionId)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            string userFileName = Path.GetFileName(fileContent.FileName.Trim('"'));

            var transactionDirectory = new DirectoryInfo(Path.Combine(RootPath, $"{DateTime.Now.Month} {DateTime.Now.Year}", transactionId));

            if (!transactionDirectory.Exists)
            {
                transactionDirectory.Create();
            }

            string fullPhysicalPath = Path.Combine(transactionDirectory.FullName, userFileName);

            using (FileStream stream = new FileStream(fullPhysicalPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                file.CopyTo(stream);
            }

            return userFileName;
        }

        public void RemoveUnclaimedFiles(string transactionId)
        {
            var transactionDir = new DirectoryInfo(Path.Combine(RootPath, DateTime.Now.Date.ToString(), transactionId));

            try
            {
                transactionDir.Delete(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        public string GetFullPath(int fileId)
        {
            var file = Db.Files.FirstOrDefault(f => f.Id == fileId);

            if (file == null)
            {
                throw new Exception("Файл был удален");
            }

            return Path.Combine(RootPath, file.Path);
        }

        public void RemoveFile(string fileName, string transactionId)
        {
            string physicalPath = Path.Combine(RootPath, $"{DateTime.Now.Month} {DateTime.Now.Year}", transactionId, fileName);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }
    }
}
