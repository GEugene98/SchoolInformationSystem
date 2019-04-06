using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;

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

        [HttpGet("MyChecklists")]
        public IActionResult MyChecklists()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var checklists = ChecklistService.GetChecklists(currentUser.Id);
            return Ok(checklists);
        }
    }
}