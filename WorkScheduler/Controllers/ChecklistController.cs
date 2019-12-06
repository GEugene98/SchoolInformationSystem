using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Services;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Scheduler;
using WorkScheduler.ViewModels.Scheduler.Filtering;

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

        [HttpPost("GetById")]
        public IActionResult GetById(int id, [FromBody]RequestDetails<ChecklistFilter> requestDetails)
        {
            var checklist = ChecklistService.GetChecklistById(id);

            if (requestDetails.Filter != null)
            {
                if (!String.IsNullOrWhiteSpace(requestDetails.Filter.Date))
                    checklist.Tickets = checklist.Tickets.Where(t => t.Date.GetValueOrDefault().ToShortDateString().Contains(requestDetails.Filter.Date));
                if (!String.IsNullOrWhiteSpace(requestDetails.Filter.Created))
                    checklist.Tickets = checklist.Tickets.Where(t => t.Created.GetValueOrDefault().ToShortDateString().Contains(requestDetails.Filter.Created));
                if (!String.IsNullOrWhiteSpace(requestDetails.Filter.Name))
                    checklist.Tickets = checklist.Tickets.Where(t => t.Name.ToUpper().Contains(requestDetails.Filter.Name.ToUpper()));
            }

            if (requestDetails.SortDirection == SortDirection.Ascending)
                checklist.Tickets = checklist.Tickets.OrderBy(t => t.GetType().GetProperty(requestDetails.SortProperty).GetValue(t, null)).ThenBy(t => t.Hours).ThenBy(t => t.Minutes);
            else
                checklist.Tickets = checklist.Tickets.OrderByDescending(t => t.GetType().GetProperty(requestDetails.SortProperty).GetValue(t, null)).ThenByDescending(t => t.Hours).ThenByDescending(t => t.Minutes);

            var response = new Response<ChecklistViewModel>()
            {
                TotalItemCount = checklist.Tickets.Count(),
                PageCount = (int)Math.Ceiling(((decimal)checklist.Tickets.Count() / (decimal)requestDetails.PageSize))
            };

            checklist.Tickets = checklist.Tickets.Skip((requestDetails.PageNumber - 1) * requestDetails.PageSize).Take(requestDetails.PageSize);

            response.Body = checklist;

            return Ok(response);
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

        [HttpGet("GetOther")]
        public IActionResult GetOther()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schoolId = (int)currentUser.SchoolId;

            var checklists = ChecklistService.GetOtherChecklists(schoolId, currentUser.Id);
            return Ok(checklists);
        }
    }
}