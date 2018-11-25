using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Services;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ActionController : Controller
    {
        protected Context Db;
        protected SchedulerService SchedulerService;
        protected UserManager<User> UserManager;

        public ActionController(Context context, SchedulerService schedulerService, UserManager<User> userManager)
        {
            Db = context;
            SchedulerService = schedulerService;
            UserManager = userManager;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]ActionViewModel action, int workScheduleId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            action.Date = action.Date.AddHours(3);
            action.EndDate = action.EndDate.AddHours(3);

            ActionStatus status = ActionStatus.New;

            var isAdmin = await UserManager.IsInRoleAsync(currentUser, "Администратор");
            var isDirector = await UserManager.IsInRoleAsync(currentUser, "Директор");

            if (isAdmin)
                status = ActionStatus.Confirmed;

            if (isDirector)
                status = ActionStatus.Accepted;

            try
            {
                if (action.EndDate.Date.ToShortDateString() == "01.01.0001" || action.EndDate == null)
                {
                    SchedulerService.AddAction(currentUser.Id, workScheduleId, action, status);
                }
                else
                {
                    while (action.Date.Date <= action.EndDate.Date)
                    {
                        SchedulerService.AddAction(currentUser.Id, workScheduleId, action, status);
                        action.Date = action.Date.AddDays(1);
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody]ActionViewModel action)
        {
            action.Date = action.Date.AddHours(3);

            try
            {
                SchedulerService.EditAction(action);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody]ActionViewModel action, int workScheduleId)
        {
            try
            {
                SchedulerService.UpdateAction(action);
            }
            catch (Exception e)
            {
                BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int actionId)
        {
            SchedulerService.DeleteAction(actionId);
            return Ok();
        }

        [HttpGet("GetActionsToMake")]
        public IActionResult GetActionsToMake(ActionStatus targetStatus)
        {
            var actions = SchedulerService.GetActionsToMake(targetStatus);
            return Ok(actions);
        }
    }
}