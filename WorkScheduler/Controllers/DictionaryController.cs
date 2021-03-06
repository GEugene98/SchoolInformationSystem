﻿using System;
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
using WorkScheduler.ViewModels.Register;

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
            var academicYears = Db.AcademicYears.OrderByDescending(a => a.Start).Select(a => new AcademicYearViewModel
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToList();

            return Ok(academicYears);
        }

        [HttpGet("Organizations")]
        public IActionResult Organizations()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var organizations = Db.Organizations
                .Where(o => o.SchoolId == currentUser.SchoolId)
                .Select(o => new DictionaryViewModel<int>
                {
                    Id = o.Id,
                    Name = o.Name
                })
                .ToList();

            return Ok(organizations);
        }

        [HttpGet("Associations")]
        public IActionResult Associations(AssociationType type, int academicYearId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            var associations = Db.Associations.Include(a => a.User) 
                .Where(a => a.Type == type && a.SchoolId == currentUser.SchoolId && a.AcademicYearId == academicYearId)
                .OrderBy(a => a.Name)
                .Select(a => new AssociationViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    User = a.User != null ? new UserViewModel
                    {
                        Id = a.User.Id,
                        FullName = $"{a.User.LastName} {a.User.FirstName} {a.User.SurName}"
                    } : null
                })
                .ToList();

            return Ok(associations);
        }

        [HttpGet("Groups")]
        public IActionResult GroupsByAssociation(int academicYearId, int associationId)
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var groups = Db.AssociationGroups
                .Where(ag => ag.AssociationId == associationId && ag.Group.SchoolId == currentUser.SchoolId && ag.Group.AcademicYearId == academicYearId)
                .Select(ag => ag.Group)
                .OrderBy(g => g.Name)
                .Select(a => new DictionaryViewModel<int>
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList();

            return Ok(groups);
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

            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            foreach (var user in Db.Users.Where(u => !u.LockoutEnabled && u.SchoolId == currentUser.SchoolId))
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


        [HttpGet("ActionNames")]
        public IActionResult ActionNames()
        {
            var result = Db.Users.Include(u => u.School).FirstOrDefault(u => u.UserName == this.User.Identity.Name).School.ActionNamesToMakeProtocolJSON;
            return Ok(result);
        }

        [HttpPost("UpdateActionNames")]
        public IActionResult UpdateActionNames([FromBody] FuckingPOSTBody<string> actionNames)
        {
            var school = Db.Users.Include(u => u.School).FirstOrDefault(u => u.UserName == this.User.Identity.Name).School;
            school.ActionNamesToMakeProtocolJSON = actionNames.Body;
            Db.SaveChanges();

            return Ok();
        }

        public class FuckingPOSTBody<TBody>
        {
            public TBody Body { get; set; }
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

        [HttpPost("SaveUser")]
        async public Task<IActionResult> SaveUser([FromBody] UserViewModel user)
        {
            var foundUser = Db.Users.FirstOrDefault(u => u.Id == user.Id);

            foundUser.FirstName = user.FirstName;
            foundUser.LastName = user.LastName;
            foundUser.SurName = user.SurName;
            foundUser.Email = user.Email;

            var foundBindings = Db.UserRoles.Where(ur => ur.UserId == foundUser.Id);
            Db.UserRoles.RemoveRange(foundBindings);
            Db.SaveChanges();

            await UserManager.AddToRoleAsync(foundUser, user.Role);

            return Ok();
        }

        [HttpGet("Users")]
        async public Task<IActionResult> Users()
        {
            var currentUser = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            var users = Db.Users.Where(u => !u.LockoutEnabled && u.SchoolId == currentUser.SchoolId).ToList();

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
                    Roles = roles,
                    Email = user.Email,
                    Role = roles.FirstOrDefault(),

                    CanAccept = user.CanAccept,
                    CanConfirm = user.CanConfirm,
                    CanSeeAllChecklists = user.CanSeeAllChecklists,
                    CanSeeAllProtocols = user.CanSeeAllProtocols,
                    CanSeeAllSchedules = user.CanSeeAllSchedules,
                    CanUseChecklists = user.CanUseChecklists
                };

                u.FullName = u.GetShortNameForm();

                userViewModels.Add(u);
            }

            return Ok(userViewModels.OrderBy(u => u.LastName));
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
        public IActionResult AllActivity([FromBody] IEnumerable<DateTime> range)
        {
            var schoolId = Db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name).SchoolId;

            var dateFrom = range.ToArray()[0].AddHours(3);
            var dateTo = range.ToArray()[1].AddHours(3);

            var result = Db.LoginLogs
                .Include(l => l.User)
                .Where(l => l.LoggedOn.Date >= dateFrom.Date && l.LoggedOn.Date <= dateTo.Date && l.User.SchoolId == schoolId)
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

            var schedulesToAccept = Db.WorkSchedules.AsNoTracking()
                .Where(ws => ws.User.SchoolId == currentUser.SchoolId && ws.Actions.Any(a => a.Status == ActionStatus.Confirmed && !a.IsDeleted))
                .Count();

            var schedulesToConfirm = Db.WorkSchedules.AsNoTracking()
                .Where(ws => ws.User.SchoolId == currentUser.SchoolId && ws.Actions.Any(a => a.Status == ActionStatus.NeedConfirm && !a.IsDeleted))
                .Count();

            var userChecklists = Db.Checklists.Where(c => c.UserId == currentUser.Id);
            var unseenChecklistTickets = Db.Tickets.Where(t => t.Notify && userChecklists.FirstOrDefault(c => t.ChecklistId == c.Id) != null).Count();

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

            notifications.Add(new DictionaryViewModel<string>
            {
                Id = "unseenChecklistTickets",
                Name = unseenChecklistTickets.ToString()
            });

            return Ok(notifications);
        }

    }
}