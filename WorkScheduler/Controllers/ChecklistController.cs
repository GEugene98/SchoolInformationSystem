using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;
using WorkScheduler.ViewModels.Scheduler;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChecklistController : Controller
    {
        protected Context Db;
        protected ChecklistService ChecklistService;

        public ChecklistController(Context context, ChecklistService checklistService)
        {
            Db = context;
            ChecklistService = checklistService;
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var checklist = ChecklistService.GetChecklistById(id);
            return Ok(checklist);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody]ChecklistViewModel checklist)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            ChecklistService.AddChecklist(checklist, currentUser.Id);
            return Ok();
        }

        [HttpGet("MyChecklists")]
        public IActionResult MyChecklists()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var checklists = ChecklistService.GetChecklists(currentUser.Id);
            return Ok(checklists);
        }
    }
}