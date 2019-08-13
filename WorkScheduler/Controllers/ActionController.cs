using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.Models.Enums;
using WorkScheduler.Models.Identity;
using WorkScheduler.Services;
using WorkScheduler.ViewModels;
using Action = WorkScheduler.Models.Action;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ActionController : Controller
    {
        protected Context Db;
        protected SchedulerService SchedulerService;
        protected UserManager<User> UserManager;
        private readonly Logger Logger;

        public ActionController(Context context, SchedulerService schedulerService, UserManager<User> userManager)
        {
            Db = context;
            SchedulerService = schedulerService;
            UserManager = userManager;
            Logger = Logger.GetInstance();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]ActionViewModel action, int workScheduleId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            action.Date = action.Date.AddHours(3);

            if (action.EndDate != null)
            {
                action.EndDate = action.EndDate.Value.AddHours(3);
            }

            ActionStatus status = ActionStatus.New;

            var isAdmin = await UserManager.IsInRoleAsync(currentUser, "Администратор");
            var isDirector = await UserManager.IsInRoleAsync(currentUser, "Директор");

            if (isAdmin)
                status = ActionStatus.Confirmed;

            if (isDirector)
                status = ActionStatus.Accepted;

            try
            {
                if (!action.EndDate.HasValue || action.EndDate.Value.ToShortDateString() == "01.01.0001")
                {
                    SchedulerService.AddAction(currentUser.Id, workScheduleId, action, status);
                }
                else
                {
                    while (action.Date.Date <= action.EndDate.Value.Date)
                    {
                        if (action.Date.DayOfWeek == DayOfWeek.Saturday || action.Date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            action.Date = action.Date.AddDays(1);
                            continue;
                        }

                        SchedulerService.AddAction(currentUser.Id, workScheduleId, action, status);
                        action.Date = action.Date.AddDays(1);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
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
                Logger.Error(e.ToString());
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
                Logger.Error(e.ToString());
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
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var actions = SchedulerService.GetActionsToMake(targetStatus, currentUser);
            return Ok(actions);
        }

        [HttpPost("Export")]
        public IActionResult Export([FromBody]ExportActionsRequest request)
        {
            var actions = Db.Actions
                .Include(a => a.WorkSchedule.AcademicYear)
                .Include(a => a.ActionUsers)
                .Where(a => request.ActionIds.Contains(a.Id))
                .ToList();

            ActionStatus targetStatus;

            if (this.User.IsInRole("Директор"))
            {
                targetStatus = ActionStatus.Accepted;
            }
            else if(this.User.IsInRole("Администратор"))
            {
                targetStatus = ActionStatus.Confirmed;
            }
            else
            {
                targetStatus = ActionStatus.New;
            }

            var targetSchedule = Db.WorkSchedules.Include(a => a.AcademicYear).FirstOrDefault(s => s.Id == request.TargetScheduleId);
            var currentSchedule = actions.FirstOrDefault().WorkSchedule;

            if(targetSchedule.Id == currentSchedule.Id)
            {
                return BadRequest("Невозможно экспортировать мероприятия из текущего плана в текущий");
            }

            if(request.Replace)
            {
                foreach(var a in actions)
                {
                    a.WorkScheduleId = request.TargetScheduleId;
                    a.Status = targetStatus;
                    if (targetSchedule.AcademicYear.Start.Year > currentSchedule.AcademicYear.Start.Year) 
                    {
                        a.Date = a.Date.AddYears(1);
                    }
                }

                Db.SaveChanges();
                return Ok();
            }

            foreach(var a in actions)
            {
                var newActionDate = a.Date;
                
                if (targetSchedule.AcademicYear.Start.Year > currentSchedule.AcademicYear.Start.Year) 
                {
                    newActionDate = a.Date.AddYears(1);
                }

                var action = new Action 
                {
                    Name = a.Name,
                    Date = newActionDate,
                    ConfirmationFormId = a.ConfirmationFormId,
                    WorkScheduleId = request.TargetScheduleId,
                    Status = targetStatus,
                    IsDeleted = false
                };

                Db.Actions.Add(action);

                Db.SaveChanges();

                var aUsers = a.ActionUsers.ToList();

                foreach(var aUser in aUsers)
                {
                    aUser.ActionId = action.Id;
                }

                Db.ActionUsers.AddRange(aUsers);

                Db.SaveChanges();               
            }
            
            return Ok();
        }
    }
    
    public class ExportActionsRequest {
        public IEnumerable<int> ActionIds { get; set; } 
        public int TargetScheduleId { get; set; }
        public bool Replace { get; set; }
    }
}

