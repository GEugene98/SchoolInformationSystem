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
        public IActionResult GetById(int id)
        {
            var checklist = ChecklistService.GetChecklistById(id);
            return Ok(checklist);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody]ChecklistViewModel checklist)
        {
            try
            {
                var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
                ChecklistService.AddChecklist(checklist, currentUser.Id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody]ChecklistViewModel checklist)
        {
            try
            {
                ChecklistService.EditChecklist(checklist);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                ChecklistService.DeleteChecklist(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

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