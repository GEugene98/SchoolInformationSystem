using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorkScheduler.Models.Scheduler;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Workflow;

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

        public IEnumerable<Models.Shared.File> PutFilesInDb(string transactionId, string schoolId)
        {
            var files = new List<Models.Shared.File>();

            var transactionDirectory = new DirectoryInfo(Path.Combine(RootPath, schoolId, $"{DateTime.Now.Year}", transactionId));

            if(!transactionDirectory.Exists || transactionDirectory.GetFiles().Count() == 0)
            {
                return files;
            }

            var foundFiles = transactionDirectory.GetFiles();

            foreach(var f in foundFiles)
            {
                var file = new Models.Shared.File
                {
                    Name = f.Name,
                    Path = Path.Combine(schoolId, $"{DateTime.Now.Year}", transactionId, f.Name),
                    Extension = f.Extension.Replace(".", string.Empty),
                    SizeMb = (f.Length / 1048576)
                };

                files.Add(file);
            }

            Db.Files.AddRange(files);

            Db.SaveChanges();

            return files;
        }

        public string AddFile(IFormFile file, string transactionId, string schoolId)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            string userFileName = Path.GetFileName(fileContent.FileName.Trim('"'));

            var transactionDirectory = new DirectoryInfo(Path.Combine(RootPath, schoolId, $"{DateTime.Now.Year}", transactionId));

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

        public void RemoveUnclaimedFiles(string transactionId, string schoolId)
        {
            var transactionDir = new DirectoryInfo(Path.Combine(RootPath, schoolId, DateTime.Now.Date.Year.ToString(), transactionId));

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

        public void RemoveFile(string fileName, string transactionId, string schoolId)
        {
            string physicalPath = Path.Combine(RootPath, schoolId, $"{DateTime.Now.Year}", transactionId, fileName);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }

        public void BindFilesToTicket(IEnumerable<Models.Shared.File> uploadedFiles, long ticketId, TicketFileType bindingType)
        {
            var ticketFiles = new List<TicketFile>();

            foreach (var f in uploadedFiles)
            {
                var ticketFile = new TicketFile
                {
                    TicketId = ticketId,
                    FileId = f.Id,
                    Type = bindingType
                };

                ticketFiles.Add(ticketFile);
            }

            Db.TicketFiles.AddRange(ticketFiles);
            Db.SaveChanges();
        }

        public void BindFilesToIncomingDocument(IEnumerable<Models.Shared.File> uploadedFiles, int incomingDocumentId)
        {
            var documentFiles = new List<IncomingDocumentFile>();

            foreach (var f in uploadedFiles)
            {
                var docFile = new IncomingDocumentFile
                {
                    IncomingDocumentId = incomingDocumentId,
                    FileId = f.Id
                };

                documentFiles.Add(docFile);
            }

            Db.IncomingDocumentFiles.AddRange(documentFiles);
            Db.SaveChanges();
        }

        public void BindFilesToOutgoingDocument(IEnumerable<Models.Shared.File> uploadedFiles, int outgoingDocumentId)
        {
            var documentFiles = new List<OutgoingDocumentFile>();

            foreach (var f in uploadedFiles)
            {
                var docFile = new OutgoingDocumentFile
                {
                    OutgoingDocumentId = outgoingDocumentId,
                    FileId = f.Id
                };

                documentFiles.Add(docFile);
            }

            Db.OutgoingDocumentFiles.AddRange(documentFiles);
            Db.SaveChanges();
        }
    }
}
