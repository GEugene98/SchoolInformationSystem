using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private FileService FileService { get; set; }
        protected Context Db;

        public FileController(Context context, FileService fileService)
        {
            FileService = fileService;
            Db = context;
        }

        [HttpPost("[action]")]
        public IActionResult UploadTemporaryFiles(IEnumerable<IFormFile> files)
        {
            var schoolId = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId.ToString();

            string transactionId = Request.Headers["transaction-id"];

            if (files == null) return BadRequest();

            foreach (var file in files)
            {
                FileService.AddFile(file, transactionId, schoolId);
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult RemoveTemporaryFiles(string[] fileNames)
        {
            var schoolId = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId.ToString();

            if (fileNames == null) return BadRequest();

            string transactionId = Request.Headers["transaction-id"];

            foreach (var fileName in fileNames)
            {
                FileService.RemoveFile(fileName, transactionId, schoolId);
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public void RemoveUnclaimedFiles(string transactionId)
        {
            var schoolId = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId.ToString();
            
            FileService.RemoveUnclaimedFiles(transactionId, schoolId);
        }

        [HttpGet("[action]")]
        public IActionResult Download(int fileId)
        {
            try
            {
                //string filePath = fileService.GetFullPath(fileId);
                //string fileType = "application/octet-stream";
                //string fileName = Path.GetFileName(filePath);

                //return PhysicalFile(filePath, fileType, fileName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}