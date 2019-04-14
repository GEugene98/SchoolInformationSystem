using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WorkScheduler.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DictionaryController : Controller
    {
        protected Context Db;
        protected UserManager<User> UserManager;
        protected TicketService TicketService;
        protected CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");

        public DictionaryController(Context context, UserManager<User> userManager, TicketService ticketService)
        {
            Db = context;
            UserManager = userManager;
            TicketService = ticketService;
        }

        [HttpGet("AcademicYears")]
        public IActionResult AcademicYears()
        {
            var academicYears = Db.AcademicYears.Select(a => new AcademicYearViewModel
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToList();

            return Ok(academicYears);
        }

        [HttpGet("ConfirmationForms")]
        public IActionResult ConfirmationForms()
        {
            var confForms = Db.ConfirmationForms.Select(c => new ConfirmationFormViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

            return Ok(confForms);
        }

        [HttpGet("Responsibles")]
        public async Task<IActionResult> Responsibles()
        {
            var responsibles = new List<UserViewModel>();

            foreach (var user in Db.Users.Where(u => !u.LockoutEnabled))
            {
                var userRoles = (await UserManager.GetRolesAsync(user)).Where(r => r != "");

                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SurName = user.SurName,
                    Roles = userRoles
                };

                userModel.FullName = userModel.GetShortNameForm();

                responsibles.Add(userModel);
            }

            return Ok(responsibles
                .Where(r => r.LastName != "Тест")
                .OrderBy(r => r.LastName));
        }

        [HttpGet("Roles")]
        public IActionResult Roles()
        {
            var roles = Db.Roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            });

            return Ok(roles);
        }

        [HttpGet("Activities")]
        public IActionResult Activities()
        {
            var activities = Db.Activities.Select(a => new ActivityViewModel
            {
                Id = a.Id,
                Name = a.Name,
                Color = a.Color
            })
            .ToList();

            return Ok(activities);
        }

        [HttpGet("AddActivity")]
        public IActionResult AddActivity(ActivityViewModel activity)
        {
            var existingActivity = Db.Activities.FirstOrDefault(a => a.Id == activity.Id);

            if (existingActivity != null)
            {
                return BadRequest("Такое направление деятельности уже существует");
            }

            Db.Activities.Add(new Activity
            {
                Id = activity.Id,
                Name = activity.Name,
                Color = activity.Color
            });

            Db.SaveChanges();

            return Ok();
        }

        [HttpGet("EditActivity")]
        public IActionResult EditActivity(ActivityViewModel activity)
        {
            var existingActivity = Db.Activities.FirstOrDefault(a => a.Id == activity.Id);

            if (existingActivity == null)
            {
                return BadRequest("Такого направления деятельности не существует или оно было удалено");
            }

            existingActivity.Name = activity.Name;
            existingActivity.Color = activity.Color;

            Db.SaveChanges();

            return Ok();
        }

        [HttpGet("DeleteActivity")]
        public IActionResult DeleteActivity(ActivityViewModel activity)
        {
            var existingActivity = Db.Activities.FirstOrDefault(a => a.Id == activity.Id);

            if (existingActivity == null)
            {
                return BadRequest("Такого направления деятельности не существует или оно было удалено");
            }

            if (Db.WorkSchedules.FirstOrDefault(ws => ws.ActivityId == existingActivity.Id) != null)
            {
                return BadRequest("Существуют планы, которые ссылаются на выбранное направление деятельности. Удаление невозможно.");
            }

            Db.Activities.Remove(existingActivity);

            Db.SaveChanges();

            return Ok();
        }

        [HttpGet("Users")]
        async public Task<IActionResult> Users()
        {
            var users = Db.Users.Where(u => !u.LockoutEnabled).ToList();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {

                var roles = await UserManager.GetRolesAsync(user);

                var u = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SurName = user.SurName,
                    Name = user.UserName,
                    Roles = roles
                };

                u.FullName = u.GetShortNameForm();

                userViewModels.Add(u);
            }

            return Ok(userViewModels);
        }

        [Authorize(Roles = "Директор")]
        [HttpGet("UserActivity")]
        public IActionResult UserActivity(string userId)
        {
            var result = Db.LoginLogs
                .Where(l => l.UserId == userId)
                .OrderBy(l => l.LoggedOn)
                .ToList()
                .Select(l => $"{l.LoggedOn.ToShortDateString()} {culture.DateTimeFormat.GetDayName(l.LoggedOn.DayOfWeek).ToLower()} в {l.LoggedOn.ToShortTimeString()} был выполнен вход");

            return Ok(result);
        }

        [Authorize(Roles = "Директор")]
        [HttpPost("AllActivity")]
        public IActionResult AllActivity([FromBody]IEnumerable<DateTime> range)
        {
            var dateFrom = range.ToArray()[0].AddHours(3);
            var dateTo = range.ToArray()[1].AddHours(3);

            var result = Db.LoginLogs
                .Include(l => l.User)
                .Where(l => l.LoggedOn.Date >= dateFrom.Date && l.LoggedOn.Date <= dateTo.Date)
                .OrderBy(l => l.LoggedOn)
                .ToList()
                .Select(l => $"Пользователь {l.User.LastName} {l.User.FirstName} {l.User.SurName} выполнил(а) вход {l.LoggedOn.ToShortDateString()} {culture.DateTimeFormat.GetDayName(l.LoggedOn.DayOfWeek).ToLower()} в {l.LoggedOn.ToShortTimeString()}");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Notifications")]
        public IActionResult Notifications()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var notifications = new List<DictionaryViewModel<string>>();
            var ticketCount = TicketService.GetAssignedTicketCount(currentUser.Id);

            notifications.Add(new DictionaryViewModel<string>
            {
                Id = "assignedTickets",
                Name = ticketCount.ToString()
            });

            var schedulesToAccept = Db.WorkSchedules.Where(ws => ws.Actions.Any(a => a.Status == ActionStatus.Confirmed)).Count();
            var schedulesToConfirm = Db.WorkSchedules.Where(ws => ws.Actions.Any(a => a.Status == ActionStatus.NeedConfirm)).Count();

            notifications.Add(new DictionaryViewModel<string>
            {
                Id = "schedulesToAccept",
                Name = schedulesToAccept.ToString()
            });

            notifications.Add(new DictionaryViewModel<string>
            {
                Id = "schedulesToConfirm",
                Name = schedulesToConfirm.ToString()
            });

            return Ok(notifications);
        }

    }
}