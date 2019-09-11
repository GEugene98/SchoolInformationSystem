using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkScheduler.Models.Identity;
using WorkScheduler.Services;
using WorkScheduler.ViewModels;

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        protected Context Db;
        protected UserManager<User> UserManager;
        protected SchedulerService SchedulerService;
        protected NotificationService NotificationService;
        private readonly Logger Logger;

        public ScheduleController(Context context, UserManager<User> userManager, SchedulerService schedulerService, NotificationService notificationService)
        {
            Db = context;
            UserManager = userManager;
            SchedulerService = schedulerService;
            NotificationService = notificationService;
            Logger = Logger.GetInstance();
        }

        [HttpPost("ForDay")]
        public IActionResult ForDay([FromBody]DateTime date, bool showMine = false)
        {
            date = date.AddHours(3);
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schedule = SchedulerService.MakeScheduleForDay(date, currentUser, showMine);
            return Ok(schedule);
        }

        [HttpPost("ForPeriod")]
        public IActionResult ForPeriod([FromBody]GeneralScheduleViewModel model, bool showMine = false)
        {
            var start = model.Start.AddHours(3);
            var end = model.End.AddHours(3);

            if (start.ToShortDateString() == "01.01.0001")
            {
                return BadRequest();
            }
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schedule = SchedulerService.MakeScheduleForPeriod(start, end, currentUser, showMine);
            return Ok(schedule);
        }

        [HttpGet("AddWorkSchedule")]
        public IActionResult AddWorkSchedule(int academicYearId, int activityId, string name)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            try
            {
                SchedulerService.AddWorkSchedule(currentUser.Id, activityId, academicYearId, name);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpGet("GetWorkSchedule")]
        public IActionResult GetWorkSchedule(int scheduleId)
        {
            var s = SchedulerService.GetSchedule(scheduleId);

            var schedule = new WorkScheduleViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Activity = new ActivityViewModel
                {
                    Id = s.Activity.Id,
                    Name = s.Activity.Name
                },
                AcademicYear = new AcademicYearViewModel
                {
                    Id = s.AcademicYear.Id,
                    Name = s.AcademicYear.Name
                }
            };

            return Ok(schedule);
        }

        [HttpGet("MyWorkSchedules")]
        public IActionResult MyWorkSchedules()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            return Ok(SchedulerService.GetWorkSchedules(currentUser.Id));
        }

        [HttpGet("Actions")]
        public IActionResult GetActions(int workScheduleId)
        {
            return Ok(SchedulerService.GetActionsFor(workScheduleId));
        }

        [HttpPost("AllowConfirm")]
        public async Task<IActionResult> AllowConfirm([FromBody]IEnumerable<int> actionIds)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            await SchedulerService.AllowConfirm(actionIds, schoolId);
            return Ok();
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost("Confirm")]
        public async Task<IActionResult> Confirm([FromBody]IEnumerable<int> actionIds)
        {
            var schoolId = (int)Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;
            await SchedulerService.Confirm(actionIds, schoolId);
            return Ok();
        }

        [Authorize(Roles = "Администратор")]
        [HttpPost("CancelConfirming")]
        public async Task<IActionResult> CancelConfirming([FromBody]IEnumerable<int> actionIds)
        {
            await SchedulerService.CancelConfirming(actionIds);
            return Ok();
        }

        [Authorize(Roles = "Директор")]
        [HttpPost("Accept")]
        public async Task<IActionResult> Accept([FromBody]IEnumerable<int> actionIds)
        {
            await SchedulerService.Accept(actionIds);
            return Ok();
        }

        [Authorize(Roles = "Директор")]
        [HttpPost("CancelAccepting")]
        public async Task<IActionResult> CancelAccepting([FromBody]IEnumerable<int> actionIds)
        {
            await SchedulerService.CancelAccepting(actionIds);
            return Ok();
        }

        [HttpGet("SendSchedule")]
        public IActionResult SendSchedule(int scheduleId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var schedule = SchedulerService.GetSchedule(scheduleId);
            var recievers = new List<User>() { currentUser };
            var actions = SchedulerService.GetActionsFor(scheduleId);
            NotificationService.SendSchedule(schedule, actions, recievers);
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int scheduleId)
        {
            try
            {
                SchedulerService.DeleteSchedule(scheduleId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody]WorkScheduleViewModel schedule)
        {
            try
            {
                SchedulerService.EditSchedule(schedule);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}