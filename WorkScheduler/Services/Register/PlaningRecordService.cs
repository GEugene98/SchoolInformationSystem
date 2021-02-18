using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WorkScheduler.Services.Register
{
    public class PlaningRecordService
    {
        protected readonly string RootPath;
        protected Context Db;

        public PlaningRecordService(Context context, IConfiguration configuration)
        {
            Db = context;
            RootPath = configuration.GetValue<string>("UploadPath");
        }

        public void ParseExcelAndWriteToDB(IFormFile file)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var transactionDirectory = new DirectoryInfo(Path.Combine(RootPath, "Tmp", Guid.NewGuid().ToString()));

            if (!transactionDirectory.Exists)
            {
                transactionDirectory.Create();
            }

            string fullPhysicalPath = Path.Combine(transactionDirectory.FullName, Path.GetFileName(fileContent.FileName.Trim('"')));

            using (FileStream stream = new FileStream(fullPhysicalPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                file.CopyTo(stream);
            }
        }
    }
}
