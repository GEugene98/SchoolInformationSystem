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

        public FileController(FileService fileService)
        {
            FileService = fileService;
        }

        [HttpPost("[action]")]
        public IActionResult UploadTemporaryFile(IEnumerable<IFormFile> files)
        {
            string transactionId = Request.Headers["transaction-id"];

            if (files == null) return BadRequest();

            foreach (var file in files)
            {
                FileService.AddFile(file, transactionId);
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult RemoveTemporaryFiles(string[] fileNames)
        {
            if (fileNames == null) return BadRequest();

            string transactionId = Request.Headers["transaction-id"];

            foreach (var fileName in fileNames)
            {
                FileService.RemoveFile(fileName, transactionId);
            }

            return Ok();
        }

        [HttpDelete("[action]")]
        public void RemoveUnclaimedFiles(string transactionId)
        {
            FileService.RemoveUnclaimedFiles(transactionId);
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