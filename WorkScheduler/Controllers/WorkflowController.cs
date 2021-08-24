using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WorkflowController : Controller
    {
        protected Context Db;
        protected WorkflowService WorkflowService;
        private FileService FileService;

        public WorkflowController(Context context, WorkflowService workflowService, FileService fileService)
        {
            Db = context;
            WorkflowService = workflowService;
            FileService = fileService;
        }

        [HttpGet("GetIncomingDocuments")]
        public IActionResult GetIncomingDocuments()
        {
            var currentUser = Db.Users.First(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            return Ok(WorkflowService.GetIncomingDocuments(schoolId));
        }

        [HttpGet("GetOutgoingDocuments")]
        public IActionResult GetOutgoingDocuments()
        {
            var currentUser = Db.Users.First(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            return Ok(WorkflowService.GetOutgoingDocuments(schoolId));
        }

        [HttpPost("CreateIncomingDocument")]
        public IActionResult CreateIncomingDocument([FromBody] IncomingDocumentViewModel document)
        {
            string transactionId = Request.Headers["transaction-id"];

            var currentUser = Db.Users.First(u => u.UserName == this.User.Identity.Name);
            var schoolId = currentUser.SchoolId.ToString();

            var uploadedFiles = FileService.PutFilesInDb(transactionId, schoolId);

            var id = WorkflowService.CreateIncomingDocument(document, (int)currentUser.SchoolId);

            if (uploadedFiles.Count() != 0)
            {
                FileService.BindFilesToIncomingDocument(uploadedFiles, id);
            }

            return Ok(id);
        }

        [HttpPost("CreateOutgoingDocument")]
        public IActionResult CreateOutgoingDocument([FromBody] OutgoingDocumentViewModel document)
        {
            string transactionId = Request.Headers["transaction-id"];

            var currentUser = Db.Users.First(u => u.UserName == this.User.Identity.Name);
            var schoolId = currentUser.SchoolId.ToString();

            var uploadedFiles = FileService.PutFilesInDb(transactionId, schoolId);

            var id = WorkflowService.CreateOutgoingDocument(document, (int)currentUser.SchoolId);

            if (uploadedFiles.Count() != 0)
            {
                FileService.BindFilesToOutgoingDocument(uploadedFiles, id);
            }

            return Ok(id);
        }

        [HttpPost("UpdateIncomingDocument")]
        public IActionResult UpdateIncomingDocument([FromBody] IncomingDocumentViewModel document)
        {
            try
            {
                WorkflowService.UpdateIncomingDocument(document);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        [HttpPost("UpdateOutgoingDocument")]
        public IActionResult UpdateOutgoingDocument([FromBody] OutgoingDocumentViewModel document)
        {
            try
            {
                WorkflowService.UpdateOutgoingDocument(document);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        [HttpDelete("DeleteIncomingDocument")]
        public IActionResult DeleteIncomingDocument(int id)
        {
            try
            {
                WorkflowService.DeleteIncomingDocument(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        [HttpDelete("DeleteOutgoingDocument")]
        public IActionResult DeleteOutgoingDocument(int id)
        {
            try
            {
                WorkflowService.DeleteOutgoingDocument(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}
